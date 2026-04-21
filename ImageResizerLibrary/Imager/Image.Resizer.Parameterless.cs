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
using Imager.Filters;
using Imager.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
#if NET45_OR_GREATER
using System.Windows;
#endif

namespace Imager {
  partial class cImage {
    /// <summary>
    /// The kernel of a parameterless pixel scaler.
    /// </summary>
    public delegate void ParameterlessPixelScaler(IPixelWorker<sPixel> worker);

    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    public static readonly IReadOnlyDictionary<PixelScalerType, Tuple<byte, byte, ParameterlessPixelScaler>> PIXEL_SCALERS = new Dictionary<PixelScalerType, Tuple<byte, byte, ParameterlessPixelScaler>> {
      {PixelScalerType.HorizontalHalfDarkScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,w=>libBasic.HorizontalScanlines(w,-50f))},
      {PixelScalerType.HorizontalHalfLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,w=>libBasic.HorizontalScanlines(w,+50f))},
      {PixelScalerType.HorizontalFullLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,w=>libBasic.HorizontalScanlines(w,+100f))},
      {PixelScalerType.VerticalHalfDarkScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,w=>libBasic.VerticalScanlines(w,-50f))},
      {PixelScalerType.VerticalHalfLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,w=>libBasic.VerticalScanlines(w,+50f))},
      {PixelScalerType.VerticalFullLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,w=>libBasic.VerticalScanlines(w,+100f))},

      {PixelScalerType.HawkyntTv3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,2,libHawkynt.Tv3x)},

    };

    /// <summary>
    /// Applies the pixel scaler without any parameters.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(PixelScalerType type, Rectangle? filterRegion = null) {
      var info = GetPixelScalerInfo(type);
      var scaleX = info.Item1;
      var scaleY = info.Item2;
      var scaler = info.Item3;

      return this._RunLoop(filterRegion, scaleX, scaleY, w => scaler(w));
    }

#if NET45_OR_GREATER

    /// <summary>
    /// Applies the pixel scaler without any parameters.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(PixelScalerType type, Rect? filterRegion = null) 
      => this.ApplyScaler(type, filterRegion?.ToRectangle())
    ;

#endif

    /// <summary>
    /// Gets the parameterless pixel scaler info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static Tuple<byte, byte, ParameterlessPixelScaler> GetPixelScalerInfo(PixelScalerType type) 
      => PIXEL_SCALERS.TryGetValue(type, out var info) 
        ? info 
        : throw new NotSupportedException($"Parameterless scaler '{type}' not supported.")
    ;

    /// <summary>
    /// Gets the scaler information.
    /// </summary>
    /// <param name="type">The type of pixel scaler.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static ScalerInformation GetScalerInformation(PixelScalerType type) 
      => PIXEL_SCALERS.TryGetValue(type, out var info) 
        ? new ScalerInformation(ReflectionUtils.GetDisplayNameForEnumValue(type), ReflectionUtils.GetDescriptionForEnumValue(type), info.Item1, info.Item2) 
        : throw new NotSupportedException($"Parameterless scaler '{type}' not supported.")
    ;

  }
}
