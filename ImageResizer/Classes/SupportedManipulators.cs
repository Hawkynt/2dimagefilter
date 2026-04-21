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
using Classes.ImageManipulators.Scalers;
using Imager;
using Imager.Classes;
using Imager.Interface;
using Imager.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace Classes {
  internal static class SupportedManipulators {

    public static readonly KeyValuePair<string, IImageManipulator>[] MANIPULATORS = new KeyValuePair<string, IImageManipulator>[0]

    #region add interpolators
.Concat(
    from p in cImage.INTERPOLATORS
    select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p) + " <GDI+>", new Interpolator(p))
    )
    #endregion

    #region add resampler
.Concat(
    from p in ReflectionUtils.GetEnumValues<KernelType>()
    select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p), new Resampler(p))
    )
.Concat(
    from p in ReflectionUtils.GetEnumValues<WindowType>()
    select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p), new RadiusResampler(p))
    )

    #endregion

    #region add pixel resizer
.Concat(
      from p in ReflectionUtils.GetEnumValues<PixelScalerType>()
      select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p), new PixelScaler(p))
    )
    #endregion

    #region add xbr resizer
.Concat(
      from p in ReflectionUtils.GetEnumValues<XbrScalerType>()
      where p!=XbrScalerType.Xbr5
      select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p) + " <NoBlend>", new XbrScaler(p, false))
)
.Concat(
      from p in ReflectionUtils.GetEnumValues<XbrScalerType>()
      select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p), new XbrScaler(p, true))
)
    #endregion

    #region add xbrz resizer
.Concat(
      from p in ReflectionUtils.GetEnumValues<XbrzScalerType>()
      select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p), new XbrzScaler(p))
)
    #endregion

    #region add nq resizer
.Concat(
      from p in ReflectionUtils.GetEnumValues<NqScalerType>()
      from m in ReflectionUtils.GetEnumValues<NqMode>()
      select new KeyValuePair<string, IImageManipulator>(ReflectionUtils.GetDisplayNameForEnumValue(p) + (m == NqMode.Normal ? string.Empty : " " + ReflectionUtils.GetDisplayNameForEnumValue(m)), new NqScaler(p, m))
    )
    #endregion

    #region plane extractors (local-only; upstream-equivalent ones live under "Plane:" prefix)
.Concat(
    new[] {
      new KeyValuePair<string, IImageManipulator>("Red",new PlaneExtractor(c=>c.Red,"Raw red channel (gamma-encoded sRGB).")),
      new KeyValuePair<string, IImageManipulator>("Green",new PlaneExtractor(c=>c.Green,"Raw green channel (gamma-encoded sRGB).")),
      new KeyValuePair<string, IImageManipulator>("Blue",new PlaneExtractor(c=>c.Blue,"Raw blue channel (gamma-encoded sRGB).")),
      new KeyValuePair<string, IImageManipulator>("Alpha",new PlaneExtractor(c=>c.Alpha,"Returns only the alpha channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("u",new PlaneExtractor(c=>c.u,"Alternate chroma-U (sPixel-specific positive-sum form, no upstream equivalent).")),
      new KeyValuePair<string, IImageManipulator>("v",new PlaneExtractor(c=>c.v,"Alternate chroma-V (sPixel-specific positive-sum form, no upstream equivalent).")),
      new KeyValuePair<string, IImageManipulator>("Hue Colored",new PlaneExtractor(c=>c.HueColored,"Returns the colorized hue channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Brightness",new PlaneExtractor(c=>c.Brightness,"Brightness as 3R+3G+2B / 8 — non-standard formula, no upstream equivalent.")),
      new KeyValuePair<string, IImageManipulator>("ExtractColors",new PlaneExtractor(c=>c.ExtractColors,"Tries to extract the full saturated colors of the source image.")),
      new KeyValuePair<string, IImageManipulator>("ExtractDeltas",new PlaneExtractor(c=>c.ExtractDeltas,"The difference between the original source image and the hue-colored result.")),
    }
    )
    #endregion

    #region add upstream pixel scalers (fixed factor, from UpstreamPipeline) — one entry per (algorithm, supported scale)
.Concat(
    from s in UpstreamPipeline.PixelScalers()
    from scale in s.SupportedScales
    let multi = s.SupportedScales.Length > 1
    let suffix = multi ? " " + UpstreamPipeline.FormatScaleSuffix(scale) : string.Empty
    let scaleX = (int)scale.X
    let scaleY = (int)scale.Y
    let captured = s
    select new KeyValuePair<string, IImageManipulator>(
      "Scaler: " + s.Name + suffix,
      new BitmapFixedAdapter(
        multi ? s.Description + " — " + UpstreamPipeline.FormatScaleSuffix(scale) + " variant" : s.Description,
        changesResolution: true,
        b => captured.Apply(b, b.Width * scaleX, b.Height * scaleY)
      )
    )
    )
    #endregion

    #region add upstream resamplers (user width/height, from UpstreamPipeline)
.Concat(
    from s in UpstreamPipeline.Resamplers()
    select new KeyValuePair<string, IImageManipulator>(
      "Resampler: " + s.Name,
      new BitmapResamplerAdapter(s.Description, s.Resample, s.Kernel)
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

.OrderBy(kv => kv.Key, System.StringComparer.OrdinalIgnoreCase)
.ToArray();
  }
}
