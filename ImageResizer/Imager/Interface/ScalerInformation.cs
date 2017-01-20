
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

    public string Description => this._description;

    public byte ScaleFactorX => this._scaleFactorX;

    public byte ScaleFactorY => this._scaleFactorY;

    public string DisplayName => this._displayName;
  }
}
