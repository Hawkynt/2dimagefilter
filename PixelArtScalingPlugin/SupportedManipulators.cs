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

    #region local pixel scalers (PixelScalerType)
.Concat(
    from p in ReflectionUtils.GetEnumValues<PixelScalerType>()
    let info = cImage.GetScalerInformation(p)
    let capture = p
    select (ManipulatorEntry)new FixedScaleEntry(
      ReflectionUtils.GetDisplayNameForEnumValue(capture),
      info.Description,
      info.ScaleFactorX, info.ScaleFactorY,
      (img, r) => img.ApplyScaler(capture, r)
    ))
    #endregion

    #region local XBR scalers (XbrScalerType, both blend modes)
.Concat(
    from p in ReflectionUtils.GetEnumValues<XbrScalerType>()
    let info = cImage.GetScalerInformation(p)
    let capture = p
    select (ManipulatorEntry)new FixedScaleEntry(
      ReflectionUtils.GetDisplayNameForEnumValue(capture) + " <NoBlend>",
      info.Description,
      info.ScaleFactorX, info.ScaleFactorY,
      (img, r) => img.ApplyScaler(capture, false, r)
    ))
.Concat(
    from p in ReflectionUtils.GetEnumValues<XbrScalerType>()
    let info = cImage.GetScalerInformation(p)
    let capture = p
    select (ManipulatorEntry)new FixedScaleEntry(
      ReflectionUtils.GetDisplayNameForEnumValue(capture),
      info.Description,
      info.ScaleFactorX, info.ScaleFactorY,
      (img, r) => img.ApplyScaler(capture, true, r)
    ))
    #endregion

    #region local XBRZ scalers (XbrzScalerType)
.Concat(
    from p in ReflectionUtils.GetEnumValues<XbrzScalerType>()
    let info = cImage.GetScalerInformation(p)
    let capture = p
    select (ManipulatorEntry)new FixedScaleEntry(
      ReflectionUtils.GetDisplayNameForEnumValue(capture),
      info.Description,
      info.ScaleFactorX, info.ScaleFactorY,
      (img, r) => img.ApplyScaler(capture, r)
    ))
    #endregion

    #region local NQ scalers (NqScalerType × NqMode)
.Concat(
    from p in ReflectionUtils.GetEnumValues<NqScalerType>()
    from m in ReflectionUtils.GetEnumValues<NqMode>()
    let info = cImage.GetScalerInformation(p)
    let captureP = p
    let captureM = m
    select (ManipulatorEntry)new FixedScaleEntry(
      ReflectionUtils.GetDisplayNameForEnumValue(captureP) + (captureM == NqMode.Normal ? string.Empty : " " + ReflectionUtils.GetDisplayNameForEnumValue(captureM)),
      info.Description,
      info.ScaleFactorX, info.ScaleFactorY,
      (img, r) => img.ApplyScaler(captureP, captureM, r)
    ))
    #endregion

    #region upstream pixel scalers (Scaler: …) — single dropdown entry per algorithm; user W/H or Scale (%) snaps to a supported variant for multi-scale algorithms
.Concat(
    from s in UpstreamPipeline.PixelScalers()
    where s.SupportedScales.Length == 1
    let capture = s
    let only = capture.SupportedScales[0]
    let scaleX = (byte)System.Math.Max(1, System.Math.Min(byte.MaxValue, only.X))
    let scaleY = (byte)System.Math.Max(1, System.Math.Min(byte.MaxValue, only.Y))
    select (ManipulatorEntry)new FixedScaleEntry(
      "Scaler: " + capture.Name,
      capture.Description,
      scaleX, scaleY,
      (img, r) => cImage.FromBitmap(capture.Apply(img.ToBitmap(), r.Width * scaleX, r.Height * scaleY))
    ))
.Concat(
    from s in UpstreamPipeline.PixelScalers()
    where s.SupportedScales.Length > 1
    let capture = s
    let scalesText = string.Join(", ", capture.SupportedScales.Select(UpstreamPipeline.FormatScaleSuffix))
    select (ManipulatorEntry)new ScaleVariantEntry(
      "Scaler: " + capture.Name,
      capture.Description + " — supports " + scalesText + "; Target W/H or Scale (%) snaps to the nearest variant.",
      capture.SupportedScales,
      (img, _, w, h) => cImage.FromBitmap(capture.Apply(img.ToBitmap(), w, h))
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

    #region upstream resamplers (Resampler: …) — variable user-supplied target width/height
.Concat(
    from r in UpstreamPipeline.Resamplers()
    let capture = r
    select (ManipulatorEntry)new ResampleEntry(
      "Resampler: " + capture.Name,
      capture.Description,
      (img, _, w, h) => cImage.FromBitmap(capture.Resample(img.ToBitmap(), w, h))
    ))
    #endregion

      .OrderBy(e => e.Name, System.StringComparer.OrdinalIgnoreCase)
      .ToArray();
  }
}
