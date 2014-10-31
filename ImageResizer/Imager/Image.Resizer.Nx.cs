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

// TODO: consider a second alpha handling mode like in https://code.google.com/p/hqx-sharp/
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
    /// The NQ kernel.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    /// <param name="c0">The c0.</param>
    /// <param name="c1">The c1.</param>
    /// <param name="c2">The c2.</param>
    /// <param name="c3">The c3.</param>
    /// <param name="c4">The c4.</param>
    /// <param name="c5">The c5.</param>
    /// <param name="c6">The c6.</param>
    /// <param name="c7">The c7.</param>
    /// <param name="c8">The c8.</param>
    /// <returns></returns>
    internal delegate void NqKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8,PixelWorker<sPixel> worker);

    /// <summary>
    /// The NQ filter itself
    /// </summary>
    internal delegate void NqFilter(PixelWorker<sPixel> worker , byte scx, byte scy, NqKernel kernel);

    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    internal static readonly Dictionary<NqScalerType, Tuple<byte, byte, NqKernel>> NQ_SCALERS = new Dictionary<NqScalerType, Tuple<byte, byte, NqKernel>> {
      {NqScalerType.Hq2,Tuple.Create<byte, byte, NqKernel>(2,2,libHQ.Hq2xKernel)},
      {NqScalerType.Hq2X3,Tuple.Create<byte, byte, NqKernel>(2,3,libHQ.Hq2x3Kernel)},
      {NqScalerType.Hq2X4,Tuple.Create<byte, byte, NqKernel>(2,4,libHQ.Hq2x4Kernel)},
      {NqScalerType.Hq3,Tuple.Create<byte, byte, NqKernel>(3,3,libHQ.Hq3xKernel)},
      {NqScalerType.Hq4,Tuple.Create<byte, byte, NqKernel>(4,4,libHQ.Hq4xKernel)},

      {NqScalerType.Lq2,Tuple.Create<byte, byte, NqKernel>(2,2,libHQ.Lq2xKernel)},
      {NqScalerType.Lq2X3,Tuple.Create<byte, byte, NqKernel>(2,3,libHQ.Lq2x3Kernel)},
      {NqScalerType.Lq2X4,Tuple.Create<byte, byte, NqKernel>(2,4,libHQ.Lq2x4Kernel)},
      {NqScalerType.Lq3,Tuple.Create<byte, byte, NqKernel>(3,3,libHQ.Lq3xKernel)},
      {NqScalerType.Lq4,Tuple.Create<byte, byte, NqKernel>(4,4,libHQ.Lq4xKernel)},
    };

    /// <summary>
    /// The different NQ modes.
    /// </summary>
    internal static readonly Dictionary<NqMode, NqFilter> NQ_MODES = new Dictionary<NqMode, NqFilter> {
      {NqMode.Normal,libHQ.ComplexFilter},
      {NqMode.Bold,libHQ.ComplexFilterBold},
      {NqMode.Smart,libHQ.ComplexFilterSmart},
    };

    /// <summary>
    /// Applies the NQ pixel scaler.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="mode">The mode.</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(NqScalerType type, NqMode mode, Rectangle? filterRegion = null) {
      var info = GetPixelScalerInfo(type);
      var scaler = GetPixelScalerInfo(mode);

      var scaleX = info.Item1;
      var scaleY = info.Item2;
      var kernel = info.Item3;

      return (this._RunLoop(filterRegion, scaleX, scaleY, worker => scaler(worker, scaleX, scaleY, kernel)));
    }

    /// <summary>
    /// Gets the pixel scaler info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    internal static Tuple<byte, byte, NqKernel> GetPixelScalerInfo(NqScalerType type) {
      Tuple<byte, byte, NqKernel> info;
      if (NQ_SCALERS.TryGetValue(type, out info))
        return (info);
      throw new NotSupportedException(string.Format("NQ scaler '{0}' not supported.", type));
    }

    /// <summary>
    /// Gets the pixel scaler info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    internal static NqFilter GetPixelScalerInfo(NqMode type) {
      NqFilter info;
      if (NQ_MODES.TryGetValue(type, out info))
        return (info);
      throw new NotSupportedException(string.Format("NQ mode '{0}' not supported.", type));
    }

    /// <summary>
    /// Gets the scaler information.
    /// </summary>
    /// <param name="type">The type of nq scaler.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static ScalerInformation GetScalerInformation(NqScalerType type) {
      Tuple<byte, byte, NqKernel> info;
      if (NQ_SCALERS.TryGetValue(type, out info))
        return (new ScalerInformation(ReflectionUtils.GetDisplayNameForEnumValue(type), ReflectionUtils.GetDescriptionForEnumValue(type), info.Item1, info.Item2));
      throw new NotSupportedException(string.Format("NQ scaler '{0}' not supported.", type));
    }
  }
}
