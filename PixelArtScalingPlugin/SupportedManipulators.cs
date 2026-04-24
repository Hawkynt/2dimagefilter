#region (c)2008-2019 Hawkynt
/*
 *  cImage
 *  Image filtering library
    Copyright (C) 2008-2019 Hawkynt

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
using Classes;
using Imager;
using Imager.Interface;
using Imager.Pipelines;
using System.Linq;

namespace PixelArtScaling {
  internal static class SupportedManipulators {

    public static readonly ManipulatorEntry[] Manipulators =
      new ManipulatorEntry[0]

    // Local PixelScalerType / XbrScalerType / XbrzScalerType / NqScalerType blocks
    // removed — their entries are provided by the upstream rescaler block below.

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
      (img, r) => cImage.FromBitmap(capture.Apply(img.ToBitmap(), r.Width * scaleX, r.Height * scaleY, useThresholds: false))
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
      (img, _, w, h) => cImage.FromBitmap(capture.Apply(img.ToBitmap(), w, h, useThresholds: false))
    ))
    #endregion

    #region upstream filters (Filter: …)
.Concat(
    from f in UpstreamPipeline.Filters()
    let capture = f
    select (ManipulatorEntry)new FixedScaleEntry(
      "Filter: " + capture.Name,
      capture.Description,
      1, 1,
      (img, _) => cImage.FromBitmap(capture.Apply(img.ToBitmap()))
    ))
    #endregion

    #region upstream colour-space plane extractors (Plane: …)
.Concat(
    from p in UpstreamPipeline.PlaneExtractors()
    let capture = p
    select (ManipulatorEntry)new FixedScaleEntry(
      "Plane: " + capture.Name,
      capture.Description,
      1, 1,
      (img, _) => new cImage(img, capture.Extract)
    ))
    #endregion

    #region upstream resamplers (Resampler:/Downsampler: …) — variable user-supplied target width/height; honours OOB + canvas colour + centred-grid options
.Concat(
    from r in UpstreamPipeline.Resamplers()
    let capture = r
    select (ManipulatorEntry)new ResampleEntry(
      UpstreamPipeline.ClassifyResampler(capture) + ": " + capture.Name,
      capture.Description,
      (img, _, w, h, options) => cImage.FromBitmap(capture.Resample(img.ToBitmap(), w, h, options.HorizontalMode, options.VerticalMode, options.CanvasColor, options.UseCenteredGrid))
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
  }
}
