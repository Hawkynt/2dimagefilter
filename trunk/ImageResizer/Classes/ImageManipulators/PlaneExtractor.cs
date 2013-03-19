using System;
using System.Diagnostics.Contracts;

using Imager;

namespace Classes.ImageManipulators {
  internal class PlaneExtractor : IImageManipulator {
    private readonly Func<cImage, cImage> _planeExtractionFunction;

    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (false); } }
    public bool SupportsHeight { get { return (false); } }
    public bool SupportsRepetitionCount { get { return (false); } }
    public bool SupportsGridCentering { get { return (false); } }
    public bool ChangesResolution { get { return (false); } }
    public bool SupportsThresholds { get { return (false); } }
    #endregion

    public cImage Apply(cImage source) {
      return (this._planeExtractionFunction(source));
    }

    public PlaneExtractor(Func<cImage, cImage> planeExtractionFunction) {
      Contract.Requires(planeExtractionFunction != null);
      this._planeExtractionFunction = planeExtractionFunction;
    }

  }
}
