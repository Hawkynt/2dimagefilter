using System.Diagnostics.Contracts;
using System.Drawing.Drawing2D;

using Imager;

namespace Classes.ImageManipulators {
  internal class Interpolator : IImageManipulator {
    private readonly InterpolationMode _type;

    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (true); } }
    public bool SupportsHeight { get { return (true); } }
    public bool SupportsRepetitionCount { get { return (false); } }
    public bool SupportsGridCentering { get { return (false); } }
    public bool ChangesResolution { get { return (true); } }
    public bool SupportsThresholds { get { return (false); } }
    #endregion

    public cImage Apply(cImage source, int width, int height) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, width, height));
    }

    public Interpolator(InterpolationMode type) {
      this._type = type;
    }
  }
}
