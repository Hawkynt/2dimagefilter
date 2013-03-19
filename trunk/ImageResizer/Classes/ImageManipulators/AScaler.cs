using Imager;

namespace Classes.ImageManipulators {
  internal abstract class AScaler : IImageManipulator {
    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (false); } }
    public bool SupportsHeight { get { return (false); } }
    public bool SupportsRepetitionCount { get { return (true); } }
    public bool SupportsGridCentering { get { return (false); } }
    public bool ChangesResolution { get { return (true); } }
    public bool SupportsThresholds { get { return (true); } }
    #endregion

    public abstract cImage Apply(cImage source);
    public abstract byte ScaleFactorX { get; }
    public abstract byte ScaleFactorY { get; }
  }
}
