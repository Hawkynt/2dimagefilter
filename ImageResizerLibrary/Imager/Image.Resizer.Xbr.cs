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

using System;
using System.Collections.Generic;
using System.Drawing;
#if NET45_OR_GREATER
using System.Windows;
#endif
using Classes;
using Imager.Filters;
using Imager.Interface;

namespace Imager {
  partial class cImage {
    /// <summary>
    /// The XBR filter itself
    /// </summary>
    /// <param name="worker">The worker.</param>
    /// <param name="allowAlphaBlending">if set to <c>true</c> [allow alpha blending].</param>
    public delegate void XbrFilter(IPixelWorker<sPixel> worker , bool allowAlphaBlending);

    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    public static readonly IReadOnlyDictionary<XbrScalerType, Tuple<byte, byte, XbrFilter>> XBR_SCALERS = new Dictionary<XbrScalerType, Tuple<byte, byte, XbrFilter>> {
      {XbrScalerType.Xbr2, Tuple.Create<byte, byte, XbrFilter>(2, 2, libXBR.Xbr2X)},
      {XbrScalerType.Xbr3, Tuple.Create<byte, byte, XbrFilter>(3, 3, (worker, a) => libXBR.Xbr3X(worker, a, true))},
      {XbrScalerType.Xbr3Modified, Tuple.Create<byte, byte, XbrFilter>(3, 3, (worker, a) => libXBR.Xbr3X(worker, a, false))},
      {XbrScalerType.Xbr4, Tuple.Create<byte, byte, XbrFilter>(4, 4, libXBR.Xbr4X)},
      {XbrScalerType.Xbr5, Tuple.Create<byte, byte, XbrFilter>(5, 5, (worker, a) => libXBR.Xbr5X(worker))},
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

      return this._RunLoop(filterRegion, scaleX, scaleY, worker => scaler(worker, allowAlphaBlending));
    }

#if NET45_OR_GREATER

    /// <summary>
    /// Applies the XBR pixel scaler.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="allowAlphaBlending">if set to <c>true</c> [allow alpha blending].</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(XbrScalerType type, bool allowAlphaBlending, Rect? filterRegion = null) 
      => this.ApplyScaler(type, allowAlphaBlending, filterRegion?.ToRectangle())
    ;

#endif

    /// <summary>
    /// Gets the pixel scaler info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static Tuple<byte, byte, XbrFilter> GetPixelScalerInfo(XbrScalerType type) 
      => XBR_SCALERS.TryGetValue(type, out var info) 
        ? info 
        : throw new NotSupportedException($"XBR scaler '{type}' not supported.")
    ;

    /// <summary>
    /// Gets the scaler information.
    /// </summary>
    /// <param name="type">The type of XBR scaler.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static ScalerInformation GetScalerInformation(XbrScalerType type) 
      => XBR_SCALERS.TryGetValue(type, out var info) 
        ? new ScalerInformation(ReflectionUtils.GetDisplayNameForEnumValue(type), ReflectionUtils.GetDescriptionForEnumValue(type), info.Item1, info.Item2) 
        : throw new NotSupportedException($"XBR scaler '{type}' not supported.")
    ;

  }
}
