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
using System;
using System.Collections.Generic;
using System.Drawing;
using Classes;
using Imager.Filters;
using Imager.Interface;

namespace Imager {
  public partial class cImage {
    /// <summary>
    /// The XBR filter itself
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    /// <param name="srcX">The SRC X.</param>
    /// <param name="srcY">The SRC Y.</param>
    /// <param name="targetImage">The target image.</param>
    /// <param name="tgtX">The TGT X.</param>
    /// <param name="tgtY">The TGT Y.</param>
    /// <param name="allowAlphaBlending">if set to <c>true</c> [allow alpha blending].</param>
    internal delegate void XbrFilter(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, bool allowAlphaBlending);

    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    internal static readonly Dictionary<XbrScalerType, Tuple<byte, byte, XbrFilter>> XBR_SCALERS = new Dictionary<XbrScalerType, Tuple<byte, byte, XbrFilter>> {
      {XbrScalerType.Xbr2, Tuple.Create<byte, byte, XbrFilter>(2, 2, libXBR.Xbr2X)},
      {XbrScalerType.Xbr3, Tuple.Create<byte, byte, XbrFilter>(3, 3, (s, sx, sy, t, tx, ty, a) => libXBR.Xbr3X(s, sx, sy, t, tx, ty, a, true))},
      {XbrScalerType.Xbr3Modified, Tuple.Create<byte, byte, XbrFilter>(3, 3, (s, sx, sy, t, tx, ty, a) => libXBR.Xbr3X(s, sx, sy, t, tx, ty, a, false))},
      {XbrScalerType.Xbr4, Tuple.Create<byte, byte, XbrFilter>(4, 4, libXBR.Xbr4X)},
    };

    /// <summary>
    /// Applies the XBR pixel scaler.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="allowAlphaBlending">if set to <c>true</c> [allow alpha blending].</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(XbrScalerType type, bool allowAlphaBlending, Rectangle? filterRegion = null) {
      var info = GetPixelScalerInfo(type);
      var scaleX = info.Item1;
      var scaleY = info.Item2;
      var scaler = info.Item3;

      return (this._RunLoop(filterRegion, scaleX, scaleY, (s, sx, sy, t, tx, ty) => scaler(s, sx, sy, t, tx, ty, allowAlphaBlending)));
    }

    /// <summary>
    /// Gets the pixel scaler info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    internal static Tuple<byte, byte, XbrFilter> GetPixelScalerInfo(XbrScalerType type) {
      Tuple<byte, byte, XbrFilter> info;
      if (XBR_SCALERS.TryGetValue(type, out info))
        return (info);
      throw new NotSupportedException(string.Format("XBR scaler '{0}' not supported.", type));
    }

    /// <summary>
    /// Gets the scaler information.
    /// </summary>
    /// <param name="type">The type of XBR scaler.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static ScalerInformation GetScalerInformation(XbrScalerType type) {
      Tuple<byte, byte, XbrFilter> info;
      if (XBR_SCALERS.TryGetValue(type, out info))
        return (new ScalerInformation(ReflectionUtils.GetDisplayNameForEnumValue(type), ReflectionUtils.GetDescriptionForEnumValue(type), info.Item1, info.Item2));
      throw new NotSupportedException(string.Format("XBR scaler '{0}' not supported.", type));
    }
  }
}
