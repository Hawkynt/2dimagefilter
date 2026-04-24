#region (c)2008-2015 Hawkynt
/*
 *  cImage
 *  Image filtering library
    Copyright (C) 2008-2015 Hawkynt

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
using Classes.ImageManipulators;
using Imager;
using Imager.Interface;
using Imager.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace Classes {
  internal static class SupportedManipulators {

    public static readonly KeyValuePair<string, IImageManipulator>[] MANIPULATORS = new KeyValuePair<string, IImageManipulator>[0]

    #region add GDI+ resamplers (system-API passthrough — comparison baseline, not a library algorithm)
.Concat(
    from p in cImage.INTERPOLATORS
    select new KeyValuePair<string, IImageManipulator>("Resampler: " + ReflectionUtils.GetDisplayNameForEnumValue(p) + " <GDI+>", new Interpolator(p))
    )
    #endregion

    // Local kernel-based Resampler(KernelType) and RadiusResampler(WindowType) blocks removed —
    // their entries are now provided by the upstream resampler block below (via UpstreamPipeline.Resamplers()).

    // Local PixelScaler / XbrScaler / XbrzScaler / NqScaler blocks removed —
    // their entries are provided by the upstream rescaler block below.

    // Local plane-extractor block removed — upstream Plane: … entries cover
    // RGB/colour-space projections via ColorProcessing.Spaces (Oklab/Lab/HSL/HSV/HWB/YCbCr/YUV/CMYK/LCh).
    // The six custom sPixel-specific extractors (u, v, Brightness, ExtractColors, ExtractDeltas, HueColored)
    // have been dropped; they were not reachable from the plugin and were redundant / non-standard on the exe side.

    #region add upstream rescalers (fixed factor, from UpstreamPipeline) — one entry per (algorithm, supported scale)
.Concat(
    from s in UpstreamPipeline.Rescalers()
    from scale in s.SupportedScales
    let multi = s.SupportedScales.Length > 1
    let suffix = multi ? " " + UpstreamPipeline.FormatScaleSuffix(scale) : string.Empty
    let scaleX = (int)scale.X
    let scaleY = (int)scale.Y
    let captured = s
    let isFilter = scaleX == 1 && scaleY == 1
    select new KeyValuePair<string, IImageManipulator>(
      UpstreamPipeline.ClassifyRescaler(s) + ": " + s.Name + suffix,
      new BitmapFixedAdapter(
        multi ? s.Description + " — " + UpstreamPipeline.FormatScaleSuffix(scale) + " variant" : s.Description,
        changesResolution: !isFilter,
        supportsThresholds: true,
        (b, useThresholds) => captured.Apply(b, b.Width * scaleX, b.Height * scaleY, useThresholds)
      )
    )
    )
    #endregion

    #region add upstream resamplers (user width/height, from UpstreamPipeline)
.Concat(
    from s in UpstreamPipeline.Resamplers()
    select new KeyValuePair<string, IImageManipulator>(
      UpstreamPipeline.ClassifyResampler(s) + ": " + s.Name,
      new BitmapResamplerAdapter(s.Description, s.Resample, s.KernelRadius, s.EvaluateKernel)
    )
    )
    #endregion

    #region add upstream-colorspace plane extractors (from UpstreamPipeline)
.Concat(
    from p in UpstreamPipeline.PlaneExtractors()
    select new KeyValuePair<string, IImageManipulator>(
      "Plane: " + p.Name,
      new PlaneExtractor(src => new cImage(src, p.Extract), p.Description)
    )
    )
    #endregion

    #region add upstream filters (same size, from UpstreamPipeline)
.Concat(
    from f in UpstreamPipeline.Filters()
    select new KeyValuePair<string, IImageManipulator>(
      "Filter: " + f.Name,
      new BitmapFixedAdapter(f.Description, changesResolution: false, f.Apply)
    )
    )
    #endregion

    // Pure "Quantize: …" entries dropped — palette reduction without a ditherer posterises.
    //
    // "Blend: …" entries dropped — blend modes are inherently two-operand (background ⊕ overlay).
    // Applied to a single image with overlay = source they collapse to per-channel tone maps
    // (Multiply → x², Screen → 2x-x², Difference → 0, …) which isn't useful image processing.
    //
    // "Dither: …" entries dropped — dithering without explicit quantizer+palette-size control is
    // meaningless; the user-facing path is the Tools → Reduce Colours dialog.

.OrderBy(kv => kv.Key, System.StringComparer.OrdinalIgnoreCase)
.ToArray();
  }
}
