using System.Diagnostics.Contracts;

using Imager;
using Imager.Interface;

namespace Classes.ImageManipulators.Scalers {
  internal class XbrScaler : AScaler {
    private readonly XbrScalerType _type;
    private readonly bool _allowAlphaBlending;
    private readonly byte _scaleFactorX;
    private readonly byte _scaleFactorY;

    #region Implementation of AScaler
    public override cImage Apply(cImage source) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, this._allowAlphaBlending));
    }
    public override byte ScaleFactorX { get { return (this._scaleFactorX); } }
    public override byte ScaleFactorY { get { return (this._scaleFactorY); } }
    #endregion

    public XbrScaler(XbrScalerType type, bool allowAlphaBlending) {
      var info = cImage.GetPixelScalerInfo(type);
      this._type = type;
      this._allowAlphaBlending = allowAlphaBlending;
      this._scaleFactorX = info.Item1;
      this._scaleFactorY = info.Item2;
    }
  }
}
