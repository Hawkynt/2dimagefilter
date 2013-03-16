using System.Diagnostics.Contracts;
using System.Drawing.Drawing2D;

namespace Imager.Classes {

  /// <summary>
  /// An image resizer
  /// </summary>
  internal struct ImageResizerToken {
    public readonly string Name;
    public readonly InterpolationMode InterpolationMode;
    public readonly PixelBlitter Blitter;
    public ImageResizerToken(string name, PixelBlitter blitter, InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic) {
      Contract.Requires(name != null);
      this.Name = name;
      this.Blitter = blitter;
      this.InterpolationMode = interpolationMode;
    }
    public ImageResizerToken(string name, InterpolationMode interpolationMode)
      : this(name, new PixelBlitter(), interpolationMode) {
    }
    public override string ToString() {
      Contract.Assume(this.Name != null);
      return this.Name;
    }
  }

}
