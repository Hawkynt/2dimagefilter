
namespace Imager.Interface {
  public class ScalerInformation {
    private readonly string _displayName;
    private readonly string _description;
    private readonly byte _scaleFactorX;
    private readonly byte _scaleFactorY;
    public ScalerInformation(string displayName, string description, byte scaleFactorX, byte scaleFactorY) {
      this._description = description;
      this._scaleFactorX = scaleFactorX;
      this._scaleFactorY = scaleFactorY;
      this._displayName = displayName;
    }

    public string Description {
      get { return this._description; }
    }

    public byte ScaleFactorX {
      get { return this._scaleFactorX; }
    }

    public byte ScaleFactorY {
      get { return this._scaleFactorY; }
    }

    public string DisplayName {
      get { return this._displayName; }
    }
  }
}
