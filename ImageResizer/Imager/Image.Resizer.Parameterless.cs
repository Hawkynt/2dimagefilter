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
  /// <summary>
  /// 
  /// </summary>
  public partial class cImage {
    /// <summary>
    /// The kernel of a parameterless pixel scaler.
    /// </summary>
    internal delegate void ParameterlessPixelScaler(PixelWorker<sPixel> worker);

    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    internal static readonly Dictionary<PixelScalerType, Tuple<byte, byte, ParameterlessPixelScaler>> PIXEL_SCALERS = new Dictionary<PixelScalerType, Tuple<byte, byte, ParameterlessPixelScaler>> {
      {PixelScalerType.HorizontalHalfDarkScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,w=>libBasic.HorizontalScanlines(w,-50f))},
      {PixelScalerType.HorizontalHalfLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,w=>libBasic.HorizontalScanlines(w,+50f))},
      {PixelScalerType.HorizontalFullLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,w=>libBasic.HorizontalScanlines(w,+100f))},
      {PixelScalerType.VerticalHalfDarkScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,w=>libBasic.VerticalScanlines(w,-50f))},
      {PixelScalerType.VerticalHalfLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,w=>libBasic.VerticalScanlines(w,+50f))},
      {PixelScalerType.VerticalFullLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,w=>libBasic.VerticalScanlines(w,+100f))},

      {PixelScalerType.MameTv,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libMAME.Tv2x)},
      {PixelScalerType.MameTv3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libMAME.Tv3x)},
      {PixelScalerType.MameRgb,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libMAME.Rgb2x)},
      {PixelScalerType.MameRgb3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libMAME.Rgb3x)},
      {PixelScalerType.HawkyntTv,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libHawkynt.Tv2x)},
      {PixelScalerType.HawkyntTv3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,2,libHawkynt.Tv3x)},

      {PixelScalerType.BilinearPlusOriginal,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libVBA.BilinearPlusOriginal)},
      {PixelScalerType.BilinearPlus,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libVBA.BilinearPlus)},

      {PixelScalerType.Eagle,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libEagle.Eagle2x)},
      {PixelScalerType.Eagle3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libEagle.Eagle3x)},
      {PixelScalerType.Eagle3B,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libEagle.Eagle3xB)},
      {PixelScalerType.SuperEagle,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libKreed.SuperEagle)},

      {PixelScalerType.SaI,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libKreed.SaI2X)},
      {PixelScalerType.SuperSaI,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libKreed.SuperSaI)},
      
      {PixelScalerType.AdvInterp2,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libMAME.AdvInterp2x)},
      {PixelScalerType.AdvInterp3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libMAME.AdvInterp3x)},
      {PixelScalerType.Scale2,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libMAME.Scale2x)},
      {PixelScalerType.Scale3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libMAME.Scale3x)},
      
      {PixelScalerType.EpxB,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libSNES9x.EpxB)},
      {PixelScalerType.EpxC,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libSNES9x.EpxC)},
      {PixelScalerType.Epx3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libSNES9x.Epx3)},

      {PixelScalerType.ReverseAntiAlias,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,ReverseAntiAlias.Process)},
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

      return (this._RunLoop(filterRegion, scaleX, scaleY, w=>scaler(w)));
    }

    /// <summary>
    /// Gets the parameterless pixel scaler info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    internal static Tuple<byte, byte, ParameterlessPixelScaler> GetPixelScalerInfo(PixelScalerType type) {
      Tuple<byte, byte, ParameterlessPixelScaler> info;
      if (PIXEL_SCALERS.TryGetValue(type, out info))
        return (info);
      throw new NotSupportedException(string.Format("Parameterless scaler '{0}' not supported.", type));
    }

    /// <summary>
    /// Gets the scaler information.
    /// </summary>
    /// <param name="type">The type of pixel scaler.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static ScalerInformation GetScalerInformation(PixelScalerType type) {
      Tuple<byte, byte, ParameterlessPixelScaler> info;
      if (PIXEL_SCALERS.TryGetValue(type, out info))
        return (new ScalerInformation(ReflectionUtils.GetDisplayNameForEnumValue(type), ReflectionUtils.GetDescriptionForEnumValue(type), info.Item1, info.Item2));
      throw new NotSupportedException(string.Format("Parameterless scaler '{0}' not supported.", type));
    }
  }
}
