using System.Diagnostics.Contracts;

using Imager;
using Imager.Interface;

namespace Classes.ImageManipulators.Scalers {
  internal class PixelScaler : AScaler {
    private readonly PixelScalerType _type;
    private readonly byte _scaleFactorX;
    private readonly byte _scaleFactorY;

    #region Implementation of AScaler
    public override cImage Apply(cImage source) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type));
    }
    public override byte ScaleFactorX { get { return (this._scaleFactorX); } }
    public override byte ScaleFactorY { get { return (this._scaleFactorY); } }
    #endregion

    public PixelScaler(PixelScalerType type) {
      var info = cImage.GetPixelScalerInfo(type);
      this._type = type;
      this._scaleFactorX = info.Item1;
      this._scaleFactorY = info.Item2;
    }
  }
}
