#region (c)2008-2026 Hawkynt
/*
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

using Hawkynt.Drawing;

using Imager.Pipelines;

namespace PixelArtScaling {
  internal static class SupportedManipulators {

    /// <summary>
    /// Walks the <paramref name="source"/> Bitmap pixel-by-pixel through <paramref name="extract"/>
    /// and writes a 32bpp greyscale result. The plugin's plane-extractor entries previously delegated
    /// this through <c>cImage</c>; the M5 migration moved it to a direct <see cref="IBitmapLocker"/>
    /// loop so the plugin no longer touches the retired <c>cImage</c> wrapper.
    /// </summary>
    private static Bitmap _ExtractPlane(Bitmap source, System.Func<Color, byte> extract) {
      var width = source.Width;
      var height = source.Height;
      var result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
      using (var src = source.Lock(ImageLockMode.ReadOnly))
      using (var dst = result.Lock(ImageLockMode.WriteOnly)) {
        for (var y = 0; y < height; ++y)
        for (var x = 0; x < width; ++x) {
          var grey = extract(src[x, y]);
          dst[x, y] = Color.FromArgb(255, grey, grey, grey);
        }
      }
      return result;
    }

    public static readonly ManipulatorEntry[] Manipulators =
      new ManipulatorEntry[0]

    #region upstream rescalers (Upscaler:/Downscaler: …) — single dropdown entry per algorithm; user W/H or Scale (%) snaps to a supported variant for multi-scale algorithms
.Concat(
    from s in UpstreamPipeline.Rescalers()
    where s.SupportedScales.Length == 1
    let capture = s
    let only = capture.SupportedScales[0]
    let scaleX = (byte)System.Math.Max(1, System.Math.Min(byte.MaxValue, only.X))
    let scaleY = (byte)System.Math.Max(1, System.Math.Min(byte.MaxValue, only.Y))
    select (ManipulatorEntry)new FixedScaleEntry(
      UpstreamPipeline.ClassifyRescaler(capture) + ": " + capture.Name,
      capture.Description,
      scaleX, scaleY,
      (img, r) => capture.Apply(img, r.Width * scaleX, r.Height * scaleY, false)
    ))
.Concat(
    from s in UpstreamPipeline.Rescalers()
    where s.SupportedScales.Length > 1
    let capture = s
    let scalesText = string.Join(", ", capture.SupportedScales.Select(UpstreamPipeline.FormatScaleSuffix))
    select (ManipulatorEntry)new ScaleVariantEntry(
      UpstreamPipeline.ClassifyRescaler(capture) + ": " + capture.Name,
      capture.Description + " — supports " + scalesText + "; Target W/H or Scale (%) snaps to the nearest variant.",
      capture.SupportedScales,
      (img, _, w, h) => capture.Apply(img, w, h, false)
    ))
    #endregion

    #region upstream filters (Filter: …) — parametric variants thread Parameters/CreateWith through
.Concat(
    from f in UpstreamPipeline.Filters()
    let capture = f
    select (ManipulatorEntry)_BuildFilterEntry(capture)
    )
    #endregion

    #region upstream colour-space plane extractors (Plane: …)
.Concat(
    from p in UpstreamPipeline.PlaneExtractors()
    let capture = p
    select (ManipulatorEntry)new FixedScaleEntry(
      "Plane: " + capture.Name,
      capture.Description,
      1, 1,
      (img, _) => _ExtractPlane(img, capture.Extract)
    ))
    #endregion

    #region upstream resamplers (Resampler:/Downsampler: …) — variable user-supplied target width/height; honours OOB + canvas colour + centred-grid options
.Concat(
    from r in UpstreamPipeline.Resamplers()
    let capture = r
    select (ManipulatorEntry)new ResampleEntry(
      UpstreamPipeline.ClassifyResampler(capture) + ": " + capture.Name,
      capture.Description,
      (img, _, w, h, options) => capture.Resample(img, w, h, options.HorizontalMode, options.VerticalMode, options.CanvasColor, options.UseCenteredGrid)
    ))
    #endregion

    // "Quantize: …" entries dropped — palette reduction without a ditherer produces posterised/banded output.
    //
    // "Blend: …" entries dropped — blend modes are inherently two-operand (background ⊕ overlay).
    // Applying one to a single image with overlay = source collapses to a per-channel tone map that
    // isn't useful image processing (Multiply → x², Screen → 2x-x², Difference → 0, …).
    //
    // "Dither: …" entries dropped — dithering without explicit quantizer+palette-size control is
    // meaningless; the user-facing path is the Reduce Colours dialog (exe) / future plugin UI.

      .OrderBy(e => e.Name, System.StringComparer.OrdinalIgnoreCase)
      .ToArray();

    /// <summary>
    /// Wraps an upstream <see cref="UpstreamPipeline.FilterInfo"/> in a <see cref="FixedScaleEntry"/>.
    /// When the filter advertises a non-empty <c>Parameters</c> list (i.e. it was registered through
    /// <c>FilterRegistry.RegisterParametric</c>), threads the parameter surface and a CreateWith
    /// rebuilder into the entry so the plugin's PropertyGrid can render and apply tunable values.
    /// Mirrors PG1's <c>SupportedManipulators._BuildFilterAdapter</c> in the WinForms exe.
    /// </summary>
    private static FixedScaleEntry _BuildFilterEntry(UpstreamPipeline.FilterInfo filter) {
      var displayName = "Filter: " + filter.Name;
      if (filter.Parameters == null || filter.Parameters.Count == 0 || filter.CreateWith == null)
        return new FixedScaleEntry(displayName, filter.Description, 1, 1, (img, _) => filter.Apply(img));

      System.Func<System.Collections.Generic.IReadOnlyDictionary<string, object>, FixedScaleEntry> rebuild = null;
      rebuild = values => {
        var bound = filter.CreateWith(values);
        return new FixedScaleEntry(
          displayName,
          bound.Description,
          1, 1,
          (img, _) => bound.Apply(img),
          bound.Parameters,
          rebuild
        );
      };

      return new FixedScaleEntry(
        displayName,
        filter.Description,
        1, 1,
        (img, _) => filter.Apply(img),
        filter.Parameters,
        rebuild
      );
    }
  }
}
