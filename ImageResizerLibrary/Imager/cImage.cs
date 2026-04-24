#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

// TODO: seamless carving aka content-aware resizing
// TODO: smart filtering http://www.hiend3d.com/smartflt.html
// TODO: on transparent out of bounds mode, resize source image first, apply filter, resize back
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Imager.Interface;

namespace Imager {
  /// <summary>
  /// A bitmap image
  /// </summary>
  public partial class cImage : ICloneable {
    #region fields
    // image data
    /// <summary>
    /// An array containing the images' pixel data
    /// </summary>
    private readonly sPixel[] _imageData;
    /// <summary>
    /// The images' width
    /// </summary>
    private readonly int _width;
    /// <summary>
    /// The images' height
    /// </summary>
    private readonly int _height;

    #endregion

    #region properties
    /// <summary>
    /// Gets the width of the image.
    /// </summary>
    /// <value>The width.</value>
    public int Width => this._width;

    /// <summary>
    /// Gets the height of the image.
    /// </summary>
    /// <value>The height.</value>
    public int Height => this._height;

    /// <summary>
    /// Gets the a new instance containing a greyscale image of the red values only.
    /// </summary>
    /// <value>The greyscale image from the red components.</value>
    public cImage Red => new cImage(this, pixel => pixel.Red);

    /// <summary>
    /// Gets the a new instance containing a greyscale image of the green values only.
    /// </summary>
    /// <value>The greyscale image from the green components.</value>
    public cImage Green => new cImage(this, pixel => pixel.Green);

    /// <summary>
    /// Gets the a new instance containing a greyscale image of the blue values only.
    /// </summary>
    /// <value>The greyscale image from the blue components.</value>
    public cImage Blue => new cImage(this, pixel => pixel.Blue);

    /// <summary>
    /// Gets the a new instance containing a greyscale image of the alpha values only.
    /// </summary>
    /// <value>The greyscale image from the alpha components.</value>
    public cImage Alpha => new cImage(this, pixel => pixel.Alpha);

    // Custom plane-extractor properties (u, v, Brightness, ExtractColors, ExtractDeltas, HueColored)
    // removed — consumers now go through upstream Plane: descriptors in ColorProcessing.Spaces.

    // OOB mode setters removed — after the local XBR/XBRz/NQ/resampler migration, no reader remains.
    #endregion
    #region ctor dtor idx
    /// <summary>
    /// Initializes a new instance of the <see cref="cImage"/> class.
    /// </summary>
    /// <param name="width">Width of the image.</param>
    /// <param name="height">Height of the image.</param>
    public cImage(int width, int height) {
      this._width = width;
      this._height = height;
      this._imageData = new sPixel[width * height];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="cImage"/> class from a given one.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    public cImage(cImage sourceImage)
      : this(sourceImage?._width ?? 0, sourceImage?._height ?? 0) {
      if (sourceImage == null)
        return;

      for (long index = 0; index < sourceImage._imageData.LongLength; index++)
        this._imageData[index] = sourceImage._imageData[index];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="cImage"/> class by filtering a given one.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    /// <param name="filterFunction">The filter.</param>
    public cImage(cImage sourceImage, Func<sPixel, sPixel> filterFunction)
      : this(sourceImage?._width ?? 0, sourceImage?._height ?? 0) {
      if (sourceImage == null)
        return;

      var width = sourceImage._width;
      Parallel.ForEach(Partitioner.Create(0, this._height), () => 0, (range, _, threadStorage) => {
        for (var y = range.Item2; y > range.Item1;) {
          --y;
          for (var x = width; x > 0;) {
            --x;
            this[x, y] = filterFunction(sourceImage[x, y]);
          }
        }
        return threadStorage;
      }, _ => {
      });
    }

    /// <summary>
    /// Initializes a new greyscale instance of the <see cref="cImage"/> class by filtering a given one.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    /// <param name="colorFilter">The greyscale filter.</param>
    public cImage(cImage sourceImage, Func<sPixel, byte> colorFilter)
      : this(sourceImage == null ? 0 : sourceImage._width, sourceImage == null ? 0 : sourceImage._height) {
      if (sourceImage == null)
        return;

      var width = sourceImage._width;
      Parallel.ForEach(Partitioner.Create(0, this._height), () => 0, (range, _, threadStorage) => {
        for (var y = range.Item2; y > range.Item1;) {
          --y;
          for (var x = width; x > 0;) {
            --x;
            this[x, y] = sPixel.FromGrey(colorFilter(sourceImage[x, y]));
          }
        }
        return threadStorage;
      }, _ => {
      });
    }

    /// <summary>
    /// Gets or sets the <see cref="sPixel"/> with the specified X, Y coordinates.
    /// </summary>
    /// <value>The pixel</value>
    public sPixel this[int x, int y] {
      get { return this.GetPixel(x, y); }
      set { this.SetPixel(x, y, value); }
    }

#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal void SetPixel(int x, int y, sPixel value) {
      var width = this._width;
      var height = this._height;

      if (x < width && y < height && x >= 0 && y >= 0)
        this._imageData[y * width + x] = value;
    }

#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal sPixel GetPixel(int x, int y) {
      var width = this._width;
      var height = this._height;

      // OOB now clamps to edge — the handler-based dispatch is gone with the local scalers.
      if (x < 0) x = 0; else if (x >= width) x = width - 1;
      if (y < 0) y = 0; else if (y >= height) y = height - 1;

      return this._imageData[y * width + x];
    }

#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal sPixel[] GetImageData() => this._imageData;

    #endregion

    /// <summary>
    /// Fills the image with the specified color.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    /// <param name="alpha">The alpha-value.</param>
    public void Fill(byte red, byte green, byte blue, byte alpha = 255) => this.Fill(new sPixel(red, green, blue, alpha));

    /// <summary>
    /// Fills the image with the specified pixel.
    /// </summary>
    /// <param name="pixel">The pixel instance.</param>
    public void Fill(sPixel pixel) => Parallel.For(0, this._imageData.LongLength, offset => this._imageData[offset] = pixel);

    #region ICloneable Members
    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public object Clone() => new cImage(this);

    #endregion
  }
}
