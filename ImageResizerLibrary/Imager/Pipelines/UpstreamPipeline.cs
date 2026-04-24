#region (c)2008-2026 Hawkynt
/*
 *  cImage
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

using Hawkynt.ColorProcessing.Dithering;
using Hawkynt.ColorProcessing.Filtering;
using Hawkynt.ColorProcessing.Quantization;
using Hawkynt.ColorProcessing.Resizing;
using Hawkynt.ColorProcessing.Spaces.Cmyk;
using Hawkynt.ColorProcessing.Spaces.Cylindrical;
using Hawkynt.ColorProcessing.Spaces.Lab;
using Hawkynt.ColorProcessing.Spaces.Perceptual;
using Hawkynt.ColorProcessing.Spaces.Yuv;
using Hawkynt.ColorProcessing.Working;
using Hawkynt.Drawing;
using Hawkynt.Drawing.ColorDomain;

namespace Imager.Pipelines {
  /// <summary>
  /// Pure factory layer over the upstream <c>FrameworkExtensions.System.Drawing</c> registries.
  /// Yields ready-to-call delegates so consumers (the standalone exe and the Paint.NET plugin)
  /// can wrap them in their own manipulator types without duplicating the per-algorithm wiring.
  /// </summary>
  public static class UpstreamPipeline {

    #region rescalers (pattern-based, fixed integer scale — upscale/downscale)

    public readonly struct RescalerInfo {
      public readonly string Name;
      public readonly string Description;
      /// <summary>The scale factors this algorithm advertises. Length 1 = single fixed scale; length &gt; 1 = pick one at apply time.</summary>
      public readonly ScaleFactor[] SupportedScales;
      /// <summary>Applies the rescaler to <paramref name="targetWidth"/> × <paramref name="targetHeight"/>; the upstream library routes to the matching supported variant. The <c>useThresholds</c> flag maps to <c>ScalerQuality.HighQuality</c> (Oklab distance thresholds) vs <c>ScalerQuality.Fast</c> (exact byte compare).</summary>
      public readonly Func<Bitmap, int, int, bool, Bitmap> Apply;

      public RescalerInfo(string name, string description, ScaleFactor[] supportedScales, Func<Bitmap, int, int, bool, Bitmap> apply) {
        this.Name = name; this.Description = description;
        this.SupportedScales = supportedScales; this.Apply = apply;
      }
    }

    public static IEnumerable<RescalerInfo> Rescalers() {
      foreach (var s in ScalerRegistry.Rescalers) {
        if (s.Type.ContainsGenericParameters)
          continue;
        var capture = s;
        var scales = capture.SupportedScales;
        if (scales == null || scales.Length == 0)
          scales = new[] { new ScaleFactor(1, 1) };
        yield return new RescalerInfo(
          capture.Name,
          ComposeDescription(capture.Description, capture.Name, capture.Author),
          scales,
          (b, w, h, useThresholds) => _UpscaleAtDimensions(capture, b, w, h, useThresholds)
        );
      }
    }

    /// <summary>
    /// Picks the <see cref="ScaleFactor"/> whose target dimensions best match the requested
    /// <paramref name="targetWidth"/>×<paramref name="targetHeight"/>, builds a rescaler instance
    /// parameterised for that factor (via <c>(int)</c>/<c>(int,int)</c> constructor probing), and
    /// dispatches through <see cref="ScalerDescriptor.Upscale(Bitmap, object, ScalerQuality)"/>.
    /// <para>
    /// The <paramref name="useThresholds"/> flag maps to <c>ScalerQuality.HighQuality</c>
    /// (Oklab distance-threshold pattern matching, perceptually uniform) vs
    /// <c>ScalerQuality.Fast</c> (exact byte comparison — faster, pixel-art-friendly).
    /// </para>
    /// <para>
    /// This bypasses the upstream <see cref="ScalerDescriptor.Scale(Bitmap, int, int)"/> which for
    /// rescalers silently ignores the passed dimensions and always falls back to
    /// <c>CreateDefault()</c> — e.g. "MLAA 4x" / "XBR 3x" / "HQ 4x" would all silently scale 2x.
    /// </para>
    /// </summary>
    private static Bitmap _UpscaleAtDimensions(ScalerDescriptor descriptor, Bitmap source, int targetWidth, int targetHeight, bool useThresholds) {
      var scales = descriptor.SupportedScales;
      var quality = useThresholds ? ScalerQuality.HighQuality : ScalerQuality.Fast;
      if (scales == null || scales.Length == 0)
        return descriptor.Upscale(source, quality);

      var chosen = SnapToNearestSupportedScale(scales, source.Width, source.Height, targetWidth, targetHeight);
      var scaler = _BuildScaler(descriptor.Type, chosen) ?? descriptor.CreateDefault();
      return descriptor.Upscale(source, scaler, quality);
    }

    /// <summary>
    /// Builds a scaler instance of <paramref name="type"/> parameterised for <paramref name="scale"/>.
    /// Walks the public constructors longest-first; fills the first <c>int</c> param with
    /// <c>scale.X</c>, the second <c>int</c> param (if any) with <c>scale.Y</c>, and every
    /// remaining parameter with its declared default value. Returns <c>null</c> if no constructor
    /// can be satisfied — caller falls back to <c>CreateDefault()</c>.
    /// </summary>
    /// <remarks>
    /// Works for <c>Xbr(int, bool, bool)</c>, <c>Mlaa(int, MlaaVariant)</c>,
    /// <c>Hq(int, int, HqMode)</c>, <c>HawkyntTv(int)</c>, <c>DotMatrix(int)</c>, etc., without
    /// needing a per-scaler switch. The parameterless constructor branch is skipped so we never
    /// shadow <c>CreateDefault()</c> unintentionally.
    /// </remarks>
    private static object _BuildScaler(Type type, ScaleFactor scale) {
      foreach (var ctor in type.GetConstructors().OrderByDescending(c => c.GetParameters().Length)) {
        var parameters = ctor.GetParameters();
        if (parameters.Length == 0)
          continue;
        if (parameters[0].ParameterType != typeof(int))
          continue;

        var args = new object[parameters.Length];
        args[0] = (int)scale.X;

        var consumedY = false;
        var ok = true;
        for (var i = 1; i < parameters.Length; ++i) {
          var p = parameters[i];
          if (!consumedY && p.ParameterType == typeof(int)) {
            args[i] = (int)scale.Y;
            consumedY = true;
          } else if (p.HasDefaultValue) {
            args[i] = p.DefaultValue;
          } else {
            ok = false;
            break;
          }
        }
        if (!ok)
          continue;

        try { return ctor.Invoke(args); } catch { }
      }
      return null;
    }

    /// <summary>Renders a scale factor as a short human-readable suffix (<c>"2x"</c> for square, <c>"2x3"</c> otherwise).</summary>
    public static string FormatScaleSuffix(ScaleFactor scale) => scale.X == scale.Y ? scale.X + "x" : scale.X + "x" + scale.Y;

    /// <summary>Classifies a rescaler by direction from its supported integer scale factors.
    /// <list type="bullet">
    /// <item><c>"Upscaler"</c> — any factor exceeds 1×1 (e.g. XBR2/3/4, HQ2x).</item>
    /// <item><c>"Filter"</c> — every factor is exactly 1×1 (e.g. DES, which is a pass-through edge filter).</item>
    /// <item><c>"Downscaler"</c> — any factor is strictly &lt; 1×1. Nothing registered currently matches.</item>
    /// </list>
    /// </summary>
    public static string ClassifyRescaler(RescalerInfo r) {
      if (r.SupportedScales == null || r.SupportedScales.Length == 0)
        return "Filter";
      var hasUp = false;
      var hasDown = false;
      var hasIdentity = false;
      foreach (var s in r.SupportedScales) {
        if (s.X > 1 || s.Y > 1) hasUp = true;
        else if (s.X < 1 || s.Y < 1) hasDown = true;
        else hasIdentity = true;
      }
      if (hasUp) return "Upscaler";
      if (hasDown) return "Downscaler";
      return hasIdentity ? "Filter" : "Filter";
    }

    /// <summary>Classifies a resampler by direction. Returns <c>"Downsampler"</c> when the name identifies a downscale-only algorithm (e.g. DPID, SSIM Downscale); otherwise <c>"Resampler"</c> for bidirectional kernels.</summary>
    public static string ClassifyResampler(ResamplerInfo r)
      => r.Name.IndexOf("Downscale", System.StringComparison.OrdinalIgnoreCase) >= 0
        ? "Downsampler"
        : "Resampler";

    /// <summary>Picks the supported scale whose target dimensions are closest (Manhattan distance) to <paramref name="requestedWidth"/> × <paramref name="requestedHeight"/>.</summary>
    public static ScaleFactor SnapToNearestSupportedScale(ScaleFactor[] scales, int sourceWidth, int sourceHeight, int requestedWidth, int requestedHeight) {
      if (scales == null || scales.Length == 0)
        return new ScaleFactor(1, 1);
      var best = scales[0];
      var bestDist = long.MaxValue;
      foreach (var s in scales) {
        var dx = (long)sourceWidth * s.X - requestedWidth;
        var dy = (long)sourceHeight * s.Y - requestedHeight;
        var dist = Math.Abs(dx) + Math.Abs(dy);
        if (dist < bestDist) { bestDist = dist; best = s; }
      }
      return best;
    }

    #endregion

    #region resamplers (variable target)

    /// <summary>Delegate for the full-control resample API: Bitmap in, width × height × OOB/canvas/centred-grid → Bitmap out.</summary>
    public delegate Bitmap ResampleFunc(
      Bitmap source,
      int targetWidth,
      int targetHeight,
      Imager.Interface.OutOfBoundsMode horizontalMode,
      Imager.Interface.OutOfBoundsMode verticalMode,
      Color canvasColor,
      bool useCenteredGrid
    );

    public readonly struct ResamplerInfo {
      public readonly string Name;
      public readonly string Description;
      /// <summary>Full-control resample API — forwards OOB modes, canvas colour and centred-grid flag into the upstream pipeline.</summary>
      public readonly ResampleFunc Resample;
      /// <summary>Kernel support radius (the resampler samples over <c>[-Radius, +Radius]</c>). Zero for non-kernel resamplers.</summary>
      public readonly int KernelRadius;
      /// <summary>Closed-form 1-D kernel weight function if the upstream resampler implements <c>IKernelResampler</c>; <c>null</c> for edge/content-aware or fixed-tap ones where no separable weight exists.</summary>
      public readonly Func<float, float>? EvaluateKernel;

      public ResamplerInfo(string name, string description, ResampleFunc resample, int kernelRadius, Func<float, float>? evaluateKernel) {
        this.Name = name; this.Description = description;
        this.Resample = resample;
        this.KernelRadius = kernelRadius;
        this.EvaluateKernel = evaluateKernel;
      }
    }

    public static IEnumerable<ResamplerInfo> Resamplers() {
      foreach (var s in ScalerRegistry.Resamplers) {
        if (s.Type.ContainsGenericParameters)
          continue;
        var capture = s;

        int kernelRadius = 0;
        Func<float, float>? evaluateKernel = null;
        try {
          if (capture.CreateDefault() is IKernelResampler kernelResampler) {
            kernelRadius = kernelResampler.Radius;
            evaluateKernel = kernelResampler.EvaluateWeight;
          }
        } catch {
          // Descriptor.CreateDefault() can throw for types with required non-default args; skip kernel data.
        }

        yield return new ResamplerInfo(
          capture.Name,
          ComposeDescription(capture.Description, capture.Name, capture.Author),
          (b, w, h, xMode, yMode, canvas, centred) =>
            capture.Resample(b, w, h, _TranslateOob(xMode), _TranslateOob(yMode), canvas, centred),
          kernelRadius,
          evaluateKernel
        );
      }
    }

    /// <summary>Translates the local (consumer-facing) OOB enum into the upstream enum. The local one is kept because the exe's script serialization + plugin token use stable member names that differ from upstream.</summary>
    private static System.Drawing.Extensions.ColorProcessing.Resizing.OutOfBoundsMode _TranslateOob(Imager.Interface.OutOfBoundsMode mode) => mode switch {
      Imager.Interface.OutOfBoundsMode.ConstantExtension => System.Drawing.Extensions.ColorProcessing.Resizing.OutOfBoundsMode.Const,
      Imager.Interface.OutOfBoundsMode.HalfSampleSymmetric => System.Drawing.Extensions.ColorProcessing.Resizing.OutOfBoundsMode.Half,
      Imager.Interface.OutOfBoundsMode.WholeSampleSymmetric => System.Drawing.Extensions.ColorProcessing.Resizing.OutOfBoundsMode.Whole,
      Imager.Interface.OutOfBoundsMode.WrapAround => System.Drawing.Extensions.ColorProcessing.Resizing.OutOfBoundsMode.Wrap,
      Imager.Interface.OutOfBoundsMode.Transparent => System.Drawing.Extensions.ColorProcessing.Resizing.OutOfBoundsMode.Transparent,
      _ => System.Drawing.Extensions.ColorProcessing.Resizing.OutOfBoundsMode.Const,
    };

    #endregion

    #region filters (same size)

    public readonly struct FilterInfo {
      public readonly string Name;
      public readonly string Description;
      public readonly Func<Bitmap, Bitmap> Apply;
      public FilterInfo(string name, string description, Func<Bitmap, Bitmap> apply) {
        this.Name = name; this.Description = description; this.Apply = apply;
      }
    }

    public static IEnumerable<FilterInfo> Filters() {
      foreach (var f in FilterRegistry.All) {
        if (f.Type.ContainsGenericParameters)
          continue;
        var capture = f;
        yield return new FilterInfo(
          capture.Name,
          ComposeDescription(capture.Description, capture.Name, capture.Author),
          b => capture.Apply(b, ScalerQuality.HighQuality)
        );
      }
    }

    #endregion

    #region plane extractors (single colour-space component, sPixel → byte)

    public readonly struct PlaneExtractorInfo {
      public readonly string Name;
      public readonly string Description;
      public readonly Func<sPixel, byte> Extract;
      public PlaneExtractorInfo(string name, string description, Func<sPixel, byte> extract) {
        this.Name = name; this.Description = description; this.Extract = extract;
      }
    }

    public static IEnumerable<PlaneExtractorInfo> PlaneExtractors() {
      yield return Plane("Oklab L",
        "Perceptual lightness (Oklab L*) via upstream color pipeline — linearised sRGB, better than ITU luma.",
        l => { var ok = default(LinearRgbaFToOklabF).Project(in l); return ToByte(ok.C1); });

      yield return Plane("Oklab a (green↔red)",
        "Oklab a axis — negative = green, positive = red. Zero-centered, mapped to 0..255.",
        l => { var ok = default(LinearRgbaFToOklabF).Project(in l); return ToByte(ok.C2 * 0.5f + 0.5f); });

      yield return Plane("Oklab b (blue↔yellow)",
        "Oklab b axis — negative = blue, positive = yellow. Zero-centered, mapped to 0..255.",
        l => { var ok = default(LinearRgbaFToOklabF).Project(in l); return ToByte(ok.C3 * 0.5f + 0.5f); });

      yield return Plane("OkLCh Chroma",
        "OkLCh chroma (perceptual colorfulness).",
        l => { var ok = default(LinearRgbaFToOklchF).Project(in l); return ToByte(ok.C2); });

      yield return Plane("OkLCh Hue",
        "OkLCh hue angle (periodic).",
        l => { var ok = default(LinearRgbaFToOklchF).Project(in l); return HueToByte(ok.C3); });

      yield return Plane("CIE Lab L",
        "CIE L* (1976) via upstream pipeline.",
        l => { var lab = default(LinearRgbaFToLabF).Project(in l); return ToByte(lab.C1); });

      yield return Plane("HSL Hue",
        "HSL hue (periodic).",
        l => { var hsl = default(LinearRgbaFToHslF).Project(in l); return HueToByte(hsl.H); });

      yield return Plane("HSL Saturation",
        "HSL saturation.",
        l => { var hsl = default(LinearRgbaFToHslF).Project(in l); return ToByte(hsl.S); });

      yield return Plane("HSL Lightness",
        "HSL lightness.",
        l => { var hsl = default(LinearRgbaFToHslF).Project(in l); return ToByte(hsl.L); });

      yield return Plane("HSV Saturation",
        "HSV saturation.",
        l => { var hsv = default(LinearRgbaFToHsvF).Project(in l); return ToByte(hsv.S); });

      yield return Plane("HSV Value",
        "HSV value (max of R/G/B).",
        l => { var hsv = default(LinearRgbaFToHsvF).Project(in l); return ToByte(hsv.V); });

      yield return Plane("HWB Whiteness",
        "HWB whiteness component.",
        l => { var hwb = default(LinearRgbaFToHwbF).Project(in l); return ToByte(hwb.C2); });

      yield return Plane("HWB Blackness",
        "HWB blackness component.",
        l => { var hwb = default(LinearRgbaFToHwbF).Project(in l); return ToByte(hwb.C3); });

      yield return Plane("LCh Chroma",
        "CIE LCh chroma.",
        l => { var lch = default(LinearRgbaFToLchF).Project(in l); return ToByte(lch.C2); });

      yield return Plane("LCh Hue",
        "CIE LCh hue (periodic).",
        l => { var lch = default(LinearRgbaFToLchF).Project(in l); return HueToByte(lch.C3); });

      yield return Plane("CMYK Cyan",
        "CMYK cyan ink.",
        l => { var cmyk = default(LinearRgbaFToCmykF).Project(in l); return ToByte(cmyk.C1); });

      yield return Plane("CMYK Magenta",
        "CMYK magenta ink.",
        l => { var cmyk = default(LinearRgbaFToCmykF).Project(in l); return ToByte(cmyk.C2); });

      yield return Plane("CMYK Yellow",
        "CMYK yellow ink.",
        l => { var cmyk = default(LinearRgbaFToCmykF).Project(in l); return ToByte(cmyk.C3); });

      yield return Plane("CMYK Key",
        "CMYK key (black) ink.",
        l => { var cmyk = default(LinearRgbaFToCmykF).Project(in l); return ToByte(cmyk.C4); });

      yield return Plane("YCbCr BT.601 Y",
        "BT.601 luma (digital SD).",
        l => { var y = default(LinearRgbaFToYCbCrBt601F).Project(in l); return ToByte(y.Y); });

      yield return Plane("YCbCr BT.601 Cb",
        "BT.601 blue-difference chroma. Zero-centered, mapped to 0..255.",
        l => { var y = default(LinearRgbaFToYCbCrBt601F).Project(in l); return ToByte(y.Cb + 0.5f); });

      yield return Plane("YCbCr BT.601 Cr",
        "BT.601 red-difference chroma. Zero-centered, mapped to 0..255.",
        l => { var y = default(LinearRgbaFToYCbCrBt601F).Project(in l); return ToByte(y.Cr + 0.5f); });

      yield return Plane("YCbCr BT.709 Y",
        "BT.709 luma (digital HD).",
        l => { var y = default(LinearRgbaFToYCbCrBt709F).Project(in l); return ToByte(y.Y); });

      yield return Plane("YCbCr BT.709 Cb",
        "BT.709 blue-difference chroma. Zero-centered, mapped to 0..255.",
        l => { var y = default(LinearRgbaFToYCbCrBt709F).Project(in l); return ToByte(y.Cb + 0.5f); });

      yield return Plane("YCbCr BT.709 Cr",
        "BT.709 red-difference chroma. Zero-centered, mapped to 0..255.",
        l => { var y = default(LinearRgbaFToYCbCrBt709F).Project(in l); return ToByte(y.Cr + 0.5f); });

      yield return Plane("YUV Y",
        "Analog YUV luma.",
        l => { var y = default(LinearRgbaFToYuvF).Project(in l); return ToByte(y.Y); });

      yield return Plane("YUV U",
        "Analog YUV U (blue projection). Zero-centered, mapped to 0..255.",
        l => { var y = default(LinearRgbaFToYuvF).Project(in l); return ToByte(y.U + 0.5f); });

      yield return Plane("YUV V",
        "Analog YUV V (red projection). Zero-centered, mapped to 0..255.",
        l => { var y = default(LinearRgbaFToYuvF).Project(in l); return ToByte(y.V + 0.5f); });
    }

    #endregion

    #region quantizers + ditherers (palette reduction → 8bpp indexed → blitted back to 32bpp)

    /// <summary>
    /// Applies <paramref name="quantizer"/> + optional <paramref name="ditherer"/> at
    /// <paramref name="paletteSize"/> colours to <paramref name="source"/> and blits the 8bpp
    /// indexed result back onto a 32bppArgb canvas. The sole entry point for the
    /// Reduce Colours UI / <c>ReduceColorsCommand</c>.
    /// </summary>
    public static Bitmap ApplyQuantization(Bitmap source, QuantizerDescriptor quantizer, DithererDescriptor ditherer, ushort paletteSize) {
      var histogram = ComputeHistogram(source);
      var palette = ComputePalette(histogram, quantizer, paletteSize);
      return ApplyPaletteWithDither(source, palette, ditherer);
    }

    /// <summary>
    /// Builds a (colour → count) histogram of <paramref name="source"/>. Exposed so callers
    /// that render the same source through many (quantizer, ditherer) combos can cache it and
    /// skip the ~200 ms pixel walk on every repeat.
    /// </summary>
    public static Dictionary<int, uint> ComputeHistogram(Bitmap source) {
      var histogram = new Dictionary<int, uint>();
      using (var sl = source.Lock(ImageLockMode.ReadOnly)) {
        var height = source.Height;
        var width = source.Width;
        for (var y = 0; y < height; ++y)
        for (var x = 0; x < width; ++x) {
          var argb = sl[x, y].ToArgb();
          histogram[argb] = histogram.TryGetValue(argb, out var c) ? c + 1 : 1u;
        }
      }
      return histogram;
    }

    /// <summary>
    /// Reduces <paramref name="histogram"/> to a palette of at most <paramref name="paletteSize"/>
    /// colours using <paramref name="quantizer"/>. Pure function of (histogram, quantizer,
    /// paletteSize) — cache the result when the user picks a ditherer on the same quantizer.
    /// </summary>
    public static Color[] ComputePalette(Dictionary<int, uint> histogram, QuantizerDescriptor quantizer, ushort paletteSize) {
      var quantizerAdapter = new ColorQuantizerAdapter(quantizer.CreateDefault());
      var palette = quantizerAdapter.ReduceColorsTo(
        paletteSize,
        histogram.Select(kv => (Color.FromArgb(kv.Key), kv.Value))
      );
      if (palette.Length == 0)
        palette = new[] { Color.Black };
      return palette;
    }

    /// <summary>
    /// Applies a pre-computed <paramref name="palette"/> to <paramref name="source"/> with the
    /// requested <paramref name="ditherer"/>, and blits the 8bpp indexed result back onto a
    /// 32bppArgb canvas. The only stage that has to re-run when the user picks a different
    /// ditherer on the same (quantizer, palette size).
    /// </summary>
    /// <remarks>
    /// The final indexed→ARGB conversion used to be <c>Graphics.DrawImage(indexed, 0, 0)</c>,
    /// but GDI+ rasterization carries a process-wide critical section that made concurrent
    /// thumbnail + detail renders serialize on the same GDI+ lock. Replaced with a direct
    /// <c>LockBits</c> + byte-indexed palette lookup in unsafe C#: no rasterization, no GDI+
    /// lock, works on raw managed memory at full CPU speed per thread.
    /// </remarks>
    public static unsafe Bitmap ApplyPaletteWithDither(Bitmap source, Color[] palette, DithererDescriptor ditherer) {
      var width = source.Width;
      var height = source.Height;
      var indexed = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
      try {
        var cp = indexed.Palette;
        for (var i = 0; i < cp.Entries.Length; ++i)
          cp.Entries[i] = i < palette.Length ? palette[i] : Color.Black;
        indexed.Palette = cp;

        var ditherAdapter = new ColorDithererAdapter(
          ditherer != null ? ditherer.CreateDefault() : default(NoDithering)
        );
        using (var sl = source.Lock(ImageLockMode.ReadOnly)) {
          var targetData = indexed.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format8bppIndexed
          );
          try {
            ditherAdapter.Dither(sl, targetData, palette);
          } finally {
            indexed.UnlockBits(targetData);
          }
        }

        // Build a 256-entry ARGB LUT from the palette for fast byte→int lookup.
        var paletteArgb = new int[256];
        for (var i = 0; i < paletteArgb.Length; ++i)
          paletteArgb[i] = (i < palette.Length ? palette[i] : Color.Black).ToArgb();

        // Direct palette lookup indexed→ARGB. No Graphics, no rasterization, no cross-thread
        // GDI+ serialization — just memory access on each bitmap's pinned BitmapData.
        var result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        var idxData = indexed.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
        BitmapData resData = null;
        try {
          resData = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
          var idxPtr = (byte*)idxData.Scan0;
          var resPtr = (byte*)resData.Scan0;
          var idxStride = idxData.Stride;
          var resStride = resData.Stride;
          for (var y = 0; y < height; ++y) {
            var idxRow = idxPtr + y * idxStride;
            var resRow = (int*)(resPtr + y * resStride);
            for (var x = 0; x < width; ++x)
              resRow[x] = paletteArgb[idxRow[x]];
          }
        } finally {
          indexed.UnlockBits(idxData);
          if (resData != null) result.UnlockBits(resData);
        }
        return result;
      } finally {
        indexed.Dispose();
      }
    }

    #endregion

    // Blend modes (IBlendMode / BlendModeRegistry upstream) are intentionally not exposed as
    // single-image manipulators. They are two-operand ops (background ⊕ overlay); applying one
    // to a single image with overlay = source is a per-channel tone-map, not image processing.
    // When a blend UI is added to the plugin/exe later it should take two images.

    #region helpers

    private static PlaneExtractorInfo Plane(string name, string description, Func<LinearRgbaF, byte> projector) {
      Func<sPixel, byte> extract = px => {
        var linear = ColorAdapter.ToLinearRgbaF(px.Color);
        return projector(linear);
      };
      return new PlaneExtractorInfo(name, description, extract);
    }

    private static byte ToByte(float f) {
      if (float.IsNaN(f)) return 0;
      if (f <= 0f) return 0;
      if (f >= 1f) return 255;
      return (byte)(f * 255f + 0.5f);
    }

    private static byte HueToByte(float h) {
      h = h - (float)Math.Floor(h);
      if (float.IsNaN(h)) return 0;
      return (byte)(h * 255f + 0.5f);
    }

    private static string ComposeDescription(string description, string name, string author) {
      var basePart = string.IsNullOrEmpty(description) ? name : description;
      if (string.IsNullOrEmpty(author))
        return basePart;
      return basePart + " (by " + author + ")";
    }

    #endregion
  }
}
