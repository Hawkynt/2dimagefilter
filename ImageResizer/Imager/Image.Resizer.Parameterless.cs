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
    /// The kernel of a parameterless pixel scaler.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    /// <param name="srcX">The source X.</param>
    /// <param name="srcY">The source Y.</param>
    /// <param name="targetImage">The target image.</param>
    /// <param name="tgtX">The target X.</param>
    /// <param name="tgtY">The target Y.</param>
    internal delegate void ParameterlessPixelScaler(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY);

    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    internal static readonly Dictionary<PixelScalerType, Tuple<byte, byte, ParameterlessPixelScaler>> PIXEL_SCALERS = new Dictionary<PixelScalerType, Tuple<byte, byte, ParameterlessPixelScaler>> {
      {PixelScalerType.HorizontalHalfDarkScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,(s,sx,sy,t,tx,ty)=>libBasic.HorizontalScanlines(s,sx,sy,t,tx,ty,-50f))},
      {PixelScalerType.HorizontalHalfLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,(s,sx,sy,t,tx,ty)=>libBasic.HorizontalScanlines(s,sx,sy,t,tx,ty,+50f))},
      {PixelScalerType.HorizontalFullLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(1,2,(s,sx,sy,t,tx,ty)=>libBasic.HorizontalScanlines(s,sx,sy,t,tx,ty,+100f))},
      {PixelScalerType.VerticalHalfDarkScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,(s,sx,sy,t,tx,ty)=>libBasic.VerticalScanlines(s,sx,sy,t,tx,ty,-50f))},
      {PixelScalerType.VerticalHalfLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,(s,sx,sy,t,tx,ty)=>libBasic.VerticalScanlines(s,sx,sy,t,tx,ty,+50f))},
      {PixelScalerType.VerticalFullLightScanlines,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,1,(s,sx,sy,t,tx,ty)=>libBasic.VerticalScanlines(s,sx,sy,t,tx,ty,+100f))},

      {PixelScalerType.MameTv,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libMAME.Tv2x)},
      {PixelScalerType.MameTv3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libMAME.Tv3x)},
      {PixelScalerType.MameRgb,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libMAME.Rgb2x)},
      {PixelScalerType.MameRgb3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libMAME.Rgb3x)},
      {PixelScalerType.HawkyntTv,Tuple.Create<byte, byte, ParameterlessPixelScaler>(2,2,libHawkynt.Tv2x)},
      {PixelScalerType.HawkyntTv3,Tuple.Create<byte, byte, ParameterlessPixelScaler>(3,3,libHawkynt.Tv3x)},

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
              info.Item3(
                this,
                x,
                y,
                result,
                x * info.Item1,
                y * info.Item2
                );
            }
          }
          return (threadStorage);
        },
        _ => { }
      );
      return (result);
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
  }
}
