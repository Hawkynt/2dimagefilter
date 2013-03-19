using System.Diagnostics.Contracts;
using Imager;
using Imager.Classes;

namespace Classes.ImageManipulators {
  internal class Resampler : IImageManipulator {
    private readonly KernelType _type;

    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (true); } }
    public bool SupportsHeight { get { return (true); } }
    public bool SupportsRepetitionCount { get { return (false); } }
    public bool SupportsGridCentering { get { return (true); } }
    public bool SupportsThresholds { get { return (false); } }
    public bool ChangesResolution { get { return (true); } }
    #endregion

    public cImage Apply(cImage source, int width, int height, bool useCenteredGrid) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, width, height, useCenteredGrid));
    }

    public Resampler(KernelType type) {
      this._type = type;
    }
  }
}
