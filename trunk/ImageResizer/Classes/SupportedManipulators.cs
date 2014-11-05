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

    #region plane extractors
.Concat(
    new[] {
      new KeyValuePair<string, IImageManipulator>("Red",new PlaneExtractor(c=>c.Red,"Returns only the red channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Green",new PlaneExtractor(c=>c.Green,"Returns only the green channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Blue",new PlaneExtractor(c=>c.Blue,"Returns only the blue channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Alpha",new PlaneExtractor(c=>c.Alpha,"Returns only the alpha channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Luminance",new PlaneExtractor(c=>c.Luminance,"Returns only the luminance channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("ChrominanceU",new PlaneExtractor(c=>c.ChrominanceU,"Returns only the chroma-U channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("ChrominanceV",new PlaneExtractor(c=>c.ChrominanceV,"Returns only the chroma-V channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("u",new PlaneExtractor(c=>c.u,"Returns only the alternate chroma-U of the source image.")),
      new KeyValuePair<string, IImageManipulator>("v",new PlaneExtractor(c=>c.v,"Returns only the alternate chroma-V channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Hue",new PlaneExtractor(c=>c.Hue,"Returns only the hue channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Hue Colored",new PlaneExtractor(c=>c.HueColored,"Returns the colorized hue channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Brightness",new PlaneExtractor(c=>c.Brightness,"Returns only the brightness channel of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Min",new PlaneExtractor(c=>c.Min,"Returns only the minimum component of the RGB channels of the source image.")),
      new KeyValuePair<string, IImageManipulator>("Max",new PlaneExtractor(c=>c.Max,"Returns only the maximum component of the RGB channels of the source image.")),
      new KeyValuePair<string, IImageManipulator>("ExtractColors",new PlaneExtractor(c=>c.ExtractColors,"Tries to extract the full saturated colors of the source image.")),
      new KeyValuePair<string, IImageManipulator>("ExtractDeltas",new PlaneExtractor(c=>c.ExtractDeltas,"The difference between the original source image and the hue-colored result.")),
    }
    )
    #endregion

.ToArray();
  }
}
