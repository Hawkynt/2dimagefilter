using System;
using System.Drawing;

using Imager.Interface;

namespace Imager.Classes {
  /// <summary>
  /// Holds the different pixel blitters.
  /// </summary>
  internal struct PixelBlitter {
    /// <summary>
    /// The delegate that generates anew image from an existing one.
    /// </summary>
    public readonly Func<Bitmap, OutOfBoundsMode, OutOfBoundsMode, Bitmap> PixelCalculator;
    /// <summary>
    /// Width is multiplied with this value.
    /// </summary>
    public readonly int ScaleX;
    /// <summary>
    /// Height is multiplied with this value.
    /// </summary>
    public readonly int ScaleY;
    /// <summary>
    /// Creates a new pixel blitter structure.
    /// </summary>
    /// <param name="scaleX">Width is multiplied with this value.</param>
    /// <param name="scaleY">Height is multiplied with this value.</param>
    /// <param name="pixelCalculator">The delegate that generates anew image from an existing one.</param>
    public PixelBlitter(int scaleX, int scaleY, Func<Bitmap, OutOfBoundsMode, OutOfBoundsMode, Bitmap> pixelCalculator) {
      this.ScaleX = scaleX;
      this.ScaleY = scaleY;
      this.PixelCalculator = pixelCalculator;
    }
  }

}
