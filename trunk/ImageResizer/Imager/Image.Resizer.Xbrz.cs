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
using Classes;
using Imager.Filters;
using Imager.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Imager {
  public partial class cImage {
    /// <summary>
    /// The XBRz filter
    /// </summary>
    /// <param name="src">The source.</param>
    /// <param name="tgt">The target.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="minY">The minimum y.</param>
    /// <param name="maxY">The maximum y.</param>
    internal delegate void XbrzFilter(sPixel[] src, sPixel[] tgt, int width, int height, int minX, int minY, int maxX, int maxY);

    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    internal static readonly Dictionary<XbrzScalerType, Tuple<byte, byte, XbrzFilter>> XBRz_SCALERS = new Dictionary<XbrzScalerType, Tuple<byte, byte, XbrzFilter>> {
      {XbrzScalerType.Xbrz2, Tuple.Create<byte, byte, XbrzFilter>(2, 2, (src,tgt,w,h,minx,miny,maxx,maxy)=>libXBRz.ScaleImage(libXBRz.ScaleSize.TIMES2, src,tgt,w,h,minx,miny,maxx,maxy))},
      {XbrzScalerType.Xbrz3, Tuple.Create<byte, byte, XbrzFilter>(3, 3, (src,tgt,w,h,minx,miny,maxx,maxy)=>libXBRz.ScaleImage(libXBRz.ScaleSize.TIMES3, src,tgt,w,h,minx,miny,maxx,maxy))},
      {XbrzScalerType.Xbrz4, Tuple.Create<byte, byte, XbrzFilter>(4, 4, (src,tgt,w,h,minx,miny,maxx,maxy)=>libXBRz.ScaleImage(libXBRz.ScaleSize.TIMES4, src,tgt,w,h,minx,miny,maxx,maxy))},
      {XbrzScalerType.Xbrz5, Tuple.Create<byte, byte, XbrzFilter>(5, 5, (src,tgt,w,h,minx,miny,maxx,maxy)=>libXBRz.ScaleImage(libXBRz.ScaleSize.TIMES5, src,tgt,w,h,minx,miny,maxx,maxy))},
    };

    /// <summary>
    /// Applies the XBR pixel scaler.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(XbrzScalerType type, Rectangle? filterRegion = null) {
      var info = GetPixelScalerInfo(type);
      var scaleX = info.Item1;
      var scaleY = info.Item2;
      var scaler = info.Item3;

      if (filterRegion == null)
        filterRegion = new Rectangle(0, 0, this.Width, this.Height);

      var result = new cImage(this.Width * scaleX, this.Height * scaleY);
      // TODO: generic pixel loop
      scaler(this.GetImageData(), result.GetImageData(), this.Width, this.Height, filterRegion.Value.Left, filterRegion.Value.Top, filterRegion.Value.Right, filterRegion.Value.Bottom);
      return (result);
    }

    /// <summary>
    /// Gets the pixel scaler info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    internal static Tuple<byte, byte, XbrzFilter> GetPixelScalerInfo(XbrzScalerType type) {
      Tuple<byte, byte, XbrzFilter> info;
      if (XBRz_SCALERS.TryGetValue(type, out info))
        return (info);
      throw new NotSupportedException(string.Format("XBRz scaler '{0}' not supported.", type));
    }

    /// <summary>
    /// Gets the scaler information.
    /// </summary>
    /// <param name="type">The type of XBR scaler.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static ScalerInformation GetScalerInformation(XbrzScalerType type) {
      Tuple<byte, byte, XbrzFilter> info;
      if (XBRz_SCALERS.TryGetValue(type, out info))
        return (new ScalerInformation(ReflectionUtils.GetDisplayNameForEnumValue(type), ReflectionUtils.GetDescriptionForEnumValue(type), info.Item1, info.Item2));
      throw new NotSupportedException(string.Format("XBRz scaler '{0}' not supported.", type));
    }
  }
}
