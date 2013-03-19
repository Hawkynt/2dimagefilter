using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

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
      {XbrScalerType.Xbr3, Tuple.Create<byte, byte, XbrFilter>(3, 3, libXBR.Xbr3X)},
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
          for (var y = range.Item2; y > range.Item1; ) {
            --y;
            for (var x = endX; x > startX; ) {
              --x;
              info.Item3(this, x, y, result, x * info.Item1, y * info.Item2, allowAlphaBlending);
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
    internal static Tuple<byte, byte, XbrFilter> GetPixelScalerInfo(XbrScalerType type) {
      Tuple<byte, byte, XbrFilter> info;
      if (XBR_SCALERS.TryGetValue(type, out info))
        return (info);
      throw new NotSupportedException(string.Format("XBR scaler '{0}' not supported.", type));
    }
  }
}
