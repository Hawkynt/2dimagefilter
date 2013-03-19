using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

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
    internal delegate sPixel[] NqKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8);

    /// <summary>
    /// A struct containing all needed information for processing a single pixel.
    /// I'll be passing that by reference to allow stackalloc and avoid successive stack copying.
    /// </summary>
    internal struct NqFilterData {
      public cImage sourceImage;
      public int srcX;
      public int srcY;
      public cImage targetImage;
      public int tgtX;
      public int tgtY;
      public byte scaleX;
      public byte scaleY;
      public NqKernel kernel;
    }
    /// <summary>
    /// The NQ filter itself
    /// </summary>
    /// <param name="filterData">The filter data.</param>
    internal delegate void NqFilter(ref NqFilterData filterData);

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
      var modeHandler = GetPixelScalerInfo(mode);

      var startX = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Left);
      var startY = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Top);

      var endX = filterRegion == null ? this.Width : Math.Min(this.Width, filterRegion.Value.Right);
      var endY = filterRegion == null ? this.Height : Math.Min(this.Height, filterRegion.Value.Bottom);

      var result = new cImage((endX - startX) * info.Item1, (endY - startY) * info.Item2);

      // run through scaler
      Parallel.ForEach(
        Partitioner.Create(startY, endY),
        () => 0,
        (range, _, threadStorage) => {

          // prepare stack allocated storage
          var filterData = new NqFilterData {
            kernel = info.Item3,
            scaleX = info.Item1,
            scaleY = info.Item2,
            sourceImage = this,
            targetImage = result
          };

          // multiply here to save time during the loop
          filterData.tgtY = range.Item2 * filterData.scaleY;
          var tgtEndX = endX * filterData.scaleX;

          for (filterData.srcY = range.Item2; filterData.srcY > range.Item1; ) {
            filterData.srcY--;
            filterData.tgtY -= filterData.scaleY;
            filterData.tgtX = tgtEndX;
            for (filterData.srcX = endX; filterData.srcX > startX; ) {
              filterData.srcX--;
              filterData.tgtX -= filterData.scaleX;
              modeHandler(ref filterData);
            }
          }
          return (threadStorage);
        },
        _ => { }
      );
      return (result);
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
  }
}
