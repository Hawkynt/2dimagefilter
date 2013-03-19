using System.Diagnostics.Contracts;

using Imager;
using Imager.Interface;

namespace Classes.ImageManipulators.Scalers {
  internal class NqScaler : AScaler {
    private readonly NqScalerType _type;
    private readonly NqMode _mode;
    private readonly byte _scaleFactorX;
    private readonly byte _scaleFactorY;

    #region Implementation of AScaler
    public override cImage Apply(cImage source) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, this._mode));
    }
    public override byte ScaleFactorX { get { return (this._scaleFactorX); } }
    public override byte ScaleFactorY { get { return (this._scaleFactorY); } }
    #endregion

    public NqScaler(NqScalerType type, NqMode mode) {
      var info = cImage.GetPixelScalerInfo(type);
      this._type = type;
      this._mode = mode;
      this._scaleFactorX = info.Item1;
      this._scaleFactorY = info.Item2;
    }
  }
}
