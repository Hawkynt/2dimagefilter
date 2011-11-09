#region (c)2010-2011 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010-2011 Hawkynt

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
 * This is a C# port of my former classImage perl library.
 * You can use and modify my code as long as you give me a credit and 
 * inform me about updates, changes new features and modification. 
 * Distribution and selling is allowed. Would be nice if you give some 
 * payback.
 * 
 * Mapping usually is implemented as
 *
 * 2x:
 * C0 C1 C2     00  01
 * C3 C4 C5 =>
 * C6 C7 C8     10  11
 * 
 * 3x:
 * C0 C1 C2    00 01 02
 * C3 C4 C5 => 10 11 12
 * C6 C7 C8    20 21 22
      
 */
#endregion
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using nImager.Filters;

namespace nImager {
  /// <summary>
  /// A bitmap image
  /// </summary>
  public class cImage : ICloneable {
    #region helper structs
    /// <summary>
    /// A filter structure containing necessary fields used in filtering.
    /// </summary>
    public struct sFilter {
      /// <summary>
      /// The scale factor in X-direction.
      /// </summary>
      public readonly byte ScaleX;
      /// <summary>
      /// The scale factor in Y-direction.
      /// </summary>
      public readonly byte ScaleY;
      /// <summary>
      /// The name of the filter.
      /// </summary>
      public readonly string Name;
      /// <summary>
      /// Additional parameters.
      /// </summary>
      public readonly object Parameter;
      /// <summary>
      /// An action that filters a specified pixel from the source image into a destination area.
      /// </summary>
      public readonly Action<cImage, int, int, cImage, int, int, byte, byte, object> FilterFunctionFunction;
      /// <summary>
      /// A function that takes an image and creates a new one based on that one.
      /// </summary>
      public readonly Func<cImage, cImage> CreationFunction;
      /// <summary>
      /// Initializes a new instance of the <see cref="sFilter"/> struct.
      /// </summary>
      /// <param name="name">Name of the filter.</param>
      /// <param name="creatorFunction">The function that creates the new image with the same dimensions.</param>
      public sFilter(string name, Func<cImage, cImage> creatorFunction)
        : this(name) {
        this.CreationFunction = creatorFunction;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="sFilter"/> struct.
      /// </summary>
      /// <param name="name">Name of the filter.</param>
      /// <param name="scaleX">The X-scale factor, defaults to <c>1</c>.</param>
      /// <param name="scaleY">The Y-scale factor, defaults to <c>1</c>.</param>
      /// <param name="filterFunction">The filter function, defaults to <c>null</c>.</param>
      /// <param name="parameter">The additional parameters, default to <c>null</c>.</param>
      public sFilter(string name, byte scaleX = 1, byte scaleY = 1, Action<cImage, int, int, cImage, int, int, byte, byte, object> filterFunction = null, object parameter = null) {
        this.Name = name;
        this.Parameter = parameter;
        this.ScaleX = scaleX;
        this.ScaleY = scaleY;
        this.FilterFunctionFunction = filterFunction;
        this.CreationFunction = source => new cImage(source.Width * scaleX, source.Height * scaleY);
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="sFilter"/> struct.
      /// </summary>
      /// <param name="name">Name of the filter.</param>
      /// <param name="scaleX">The X-scale factor.</param>
      /// <param name="scaleY">The Y-scale factor.</param>
      /// <param name="filterFunction">The filter function.</param>
      /// <param name="hqFilter">The hq filter kernel.</param>
      public sFilter(string name, byte scaleX, byte scaleY, Action<cImage, int, int, cImage, int, int, byte, byte, object> filterFunction, Func<byte, sPixel, sPixel, sPixel, sPixel, sPixel, sPixel, sPixel, sPixel, sPixel, sPixel[]> hqFilter)
        : this(name, scaleX, scaleY, filterFunction, (object)hqFilter) {
      }

    }
    #endregion

    #region class fields
    /// <summary>
    /// available image filters
    /// </summary>
    private static readonly sFilter[] _imageFilters = new[]{
      new sFilter("-50% Scanlines",1,2,libBasic.HorizontalScanlines,-50f),
      new sFilter("+50% Scanlines",1,2,libBasic.HorizontalScanlines,50f),
      new sFilter("+100% Scanlines",1,2,libBasic.HorizontalScanlines,100f),
      new sFilter("-50% VScanlines",2,1,libBasic.VerticalScanlines,-50f),
      new sFilter("+50% VScanlines",2,1,libBasic.VerticalScanlines,50f),
      new sFilter("+100% VScanlines",2,1,libBasic.VerticalScanlines,100f),
      new sFilter("MAME TV 2x",2,2,libMAME.Tv2x),
      new sFilter("MAME TV 3x",3,3,libMAME.Tv3x),
      new sFilter("MAME RGB 2x",2,2,libMAME.Rgb2x),
      new sFilter("MAME RGB 3x",3,3,libMAME.Rgb3x),
      new sFilter("Hawkynt TV 2x",2,2,libHawkynt.Tv2x),
      new sFilter("Hawkynt TV 3x",3,3,libHawkynt.Tv3x),
      
      new sFilter("Bilinear Plus Original",2,2,libVBA.BilinearPlusOriginal),
      new sFilter("Bilinear Plus",2,2,libVBA.BilinearPlus),
      new sFilter("Eagle 2x",2,2,libEagle.Eagle2x),
      new sFilter("Eagle 3x",3,3,libEagle.Eagle3x),
      new sFilter("Eagle 3xB",3,3,libEagle.Eagle3xB),
      new sFilter("Super Eagle",2,2,libKreed.SuperEagle),
      new sFilter("SaI 2x",2,2,libKreed.SaI2X),
      new sFilter("Super SaI",2,2,libKreed.SuperSaI),
      new sFilter("AdvInterp 2x",2,2,libMAME.AdvInterp2x),
      new sFilter("AdvInterp 3x",3,3,libMAME.AdvInterp3x),
      new sFilter("Scale 2x",2,2,libMAME.Scale2x),
      new sFilter("Scale 3x",3,3,libMAME.Scale3x),
      new sFilter("EPXB",2,2,libSNES9x.EpxB),
      new sFilter("EPXC",2,2,libSNES9x.EpxC),
      new sFilter("EPX3",3,3,libSNES9x.Epx3),
      new sFilter("HQ 2x",2,2,libHQ.ComplexFilter,libHQ.Hq2xKernel),
      new sFilter("HQ 2x3",2,3,libHQ.ComplexFilter,libHQ.Hq2x3Kernel),
      new sFilter("HQ 2x4",2,4,libHQ.ComplexFilter,libHQ.Hq2x4Kernel),
      new sFilter("HQ 3x",3,3,libHQ.ComplexFilter,libHQ.Hq3xKernel),
      new sFilter("HQ 4x",4,4,libHQ.ComplexFilter,libHQ.Hq4xKernel),
      new sFilter("HQ 2x Bold",2,2,libHQ.ComplexFilterBold,libHQ.Hq2xKernel),
      new sFilter("HQ 2x3 Bold",2,3,libHQ.ComplexFilterBold,libHQ.Hq2x3Kernel),
      new sFilter("HQ 2x4 Bold",2,4,libHQ.ComplexFilterBold,libHQ.Hq2x4Kernel),
      new sFilter("HQ 3x Bold",3,3,libHQ.ComplexFilterBold,libHQ.Hq3xKernel),
      new sFilter("HQ 4x Bold",4,4,libHQ.ComplexFilterBold,libHQ.Hq4xKernel),
      new sFilter("HQ 2x Smart",2,2,libHQ.ComplexFilterSmart,libHQ.Hq2xKernel),
      new sFilter("HQ 2x3 Smart",2,3,libHQ.ComplexFilterSmart,libHQ.Hq2x3Kernel),
      new sFilter("HQ 2x4 Smart",2,4,libHQ.ComplexFilterSmart,libHQ.Hq2x4Kernel),
      new sFilter("HQ 3x Smart",3,3,libHQ.ComplexFilterSmart,libHQ.Hq3xKernel),
      new sFilter("HQ 4x Smart",4,4,libHQ.ComplexFilterSmart,libHQ.Hq4xKernel),
      new sFilter("LQ 2x",2,2,libHQ.ComplexFilter,libHQ.Lq2xKernel),
      new sFilter("LQ 2x3",2,3,libHQ.ComplexFilter,libHQ.Lq2x3Kernel),
      new sFilter("LQ 2x4",2,4,libHQ.ComplexFilter,libHQ.Lq2x4Kernel),
      new sFilter("LQ 3x",3,3,libHQ.ComplexFilter,libHQ.Lq3xKernel),
      new sFilter("LQ 4x",4,4,libHQ.ComplexFilter,libHQ.Lq4xKernel),
      new sFilter("LQ 2x Bold",2,2,libHQ.ComplexFilterBold,libHQ.Lq2xKernel),
      new sFilter("LQ 2x3 Bold",2,3,libHQ.ComplexFilterBold,libHQ.Lq2x3Kernel),
      new sFilter("LQ 2x4 Bold",2,4,libHQ.ComplexFilterBold,libHQ.Lq2x4Kernel),
      new sFilter("LQ 3x Bold",3,3,libHQ.ComplexFilterBold,libHQ.Lq3xKernel),
      new sFilter("LQ 4x Bold",4,4,libHQ.ComplexFilterBold,libHQ.Lq4xKernel),
      new sFilter("LQ 2x Smart",2,2,libHQ.ComplexFilterSmart,libHQ.Lq2xKernel),
      new sFilter("LQ 2x3 Smart",2,3,libHQ.ComplexFilterSmart,libHQ.Lq2x3Kernel),
      new sFilter("LQ 2x4 Smart",2,4,libHQ.ComplexFilterSmart,libHQ.Lq2x4Kernel),
      new sFilter("LQ 3x Smart",3,3,libHQ.ComplexFilterSmart,libHQ.Lq3xKernel),
      new sFilter("LQ 4x Smart",4,4,libHQ.ComplexFilterSmart,libHQ.Lq4xKernel),
      
      new sFilter("Red",image=>image.Red),
      new sFilter("Green",image=>image.Green),
      new sFilter("Blue",image=>image.Blue),
      new sFilter("Luminance",image=>image.Luminance),
      new sFilter("ChrominanceU",image=>image.ChrominanceU),
      new sFilter("ChrominanceV",image=>image.ChrominanceV),
      new sFilter("u",image=>image.u),
      new sFilter("v",image=>image.v),
      new sFilter("Hue",image=>image.Hue),
      new sFilter("Hue Colored",image=>image.HueColored),
      new sFilter("Brightness",image=>image.Brightness),
      new sFilter("Min",image=>image.Min),
      new sFilter("Max",image=>image.Max),

      new sFilter("ExtractColors",image=>image.ExtractColors),
      new sFilter("ExtractDeltas",image=>image.ExtractDeltas)
    };
    #endregion

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

    #region properties
    /// <summary>
    /// Gets the available image filters.
    /// </summary>
    /// <value>The filters.</value>
    public static sFilter[] Filters {
      get {
        return (_imageFilters);
      }
    }
    /// <summary>
    /// Gets the width of the image.
    /// </summary>
    /// <value>The width.</value>
    public int Width {
      get {
        return (this._width);
      }
    }
    /// <summary>
    /// Gets the height of the image.
    /// </summary>
    /// <value>The height.</value>
    public int Height {
      get {
        return (this._height);
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the red values only.
    /// </summary>
    /// <value>The greyscale image from the red components.</value>
    public cImage Red {
      get {
        return (new cImage(this, pixel => pixel.Red));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the green values only.
    /// </summary>
    /// <value>The greyscale image from the green components.</value>
    public cImage Green {
      get {
        return (new cImage(this, pixel => pixel.Green));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the blue values only.
    /// </summary>
    /// <value>The greyscale image from the blue components.</value>
    public cImage Blue {
      get {
        return (new cImage(this, pixel => pixel.Blue));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the luminance values only.
    /// </summary>
    /// <value>The greyscale image from the luminance components.</value>
    public cImage Luminance {
      get {
        return (new cImage(this, pixel => pixel.Luminance));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the color(U) values only.
    /// </summary>
    /// <value>The greyscale image from the color(U) components.</value>
    public cImage ChrominanceU {
      get {
        return (new cImage(this, pixel => pixel.ChrominanceU));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the color(V) values only.
    /// </summary>
    /// <value>The greyscale image from the color(V) components.</value>
    public cImage ChrominanceV {
      get {
        return (new cImage(this, pixel => pixel.ChrominanceV));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the color(u) values only.
    /// </summary>
    /// <value>The greyscale image from the color(u) components.</value>
    public cImage u {
      get {
        return (new cImage(this, pixel => pixel.u));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the color(v) values only.
    /// </summary>
    /// <value>The greyscale image from the color(v) components.</value>
    public cImage v {
      get {
        return (new cImage(this, pixel => pixel.v));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the brightness values only.
    /// </summary>
    /// <value>The greyscale image from the brightness components.</value>
    public cImage Brightness {
      get {
        return (new cImage(this, pixel => pixel.Brightness));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the minimum values only.
    /// </summary>
    /// <value>The greyscale image from the minimum of all components.</value>
    public cImage Min {
      get {
        return (new cImage(this, pixel => pixel.Min));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the maximum values only.
    /// </summary>
    /// <value>The greyscale image from the maximum of all components.</value>
    public cImage Max {
      get {
        return (new cImage(this, pixel => pixel.Max));
      }
    }
    /// <summary>
    /// Extracts the colors from an image and returns a new image with only base colors.
    /// </summary>
    public cImage ExtractColors {
      get {
        return (new cImage(this, pixel => pixel.ExtractColors));
      }
    }
    /// <summary>
    /// Extracts the grey deltas for use with an image that is color extracted.
    /// </summary>
    public cImage ExtractDeltas {
      get {
        return (new cImage(this, pixel => pixel.ExtractDeltas));
      }
    }
    /// <summary>
    /// Gets the a new instance containing a greyscale image of the hue values only.
    /// </summary>
    /// <value>The greyscale image from the hue components.</value>
    public cImage Hue {
      get {
        return (new cImage(this, pixel => pixel.Hue));
      }
    }
    /// <summary>
    /// Gets the a new instance containing an image of the hue values only.
    /// </summary>
    /// <value>The image from the hue components.</value>
    public cImage HueColored {
      get {
        return (new cImage(this, pixel => {
          const float conversionFactor = 360f / 256f;
          var hue = pixel.Hue * conversionFactor;
          const float saturation = 1f;
          const float value = 1f;
          float red, green, blue;

          if (hue < 0.001) {
            red = green = blue = 0.5f;
          } else {
            hue = hue / 60f;
            var i = (byte)Math.Floor(hue);
            var f = hue - i;
            const float p = value * (1 - saturation);
            var q = value * (1 - saturation * f);
            var t = value * (1 - saturation * (1 - f));
            switch (i) {
              case 0: {
                red = value;
                green = t;
                blue = p;
                break;
              }
              case 1: {
                red = q;
                green = value;
                blue = p;
                break;
              }
              case 2: {
                red = p;
                green = value;
                blue = t;
                break;
              }
              case 3: {
                red = p;
                green = q;
                blue = value;
                break;
              }
              case 4: {
                red = t;
                green = p;
                blue = value;
                break;
              }
              case 5: {
                red = value;
                green = p;
                blue = q;
                break;
              }
              default: {
                throw new NotSupportedException();
              }
            }
          }
          return (new sPixel(red, green, blue));
        }));
      }
    }
    #endregion
    #region ctor dtor idx
    // NOTE: Bitmap objects does not support parallel read-outs blame Microsoft
    /// <summary>
    /// Initializes a new instance of the <see cref="cImage"/> class from a <see cref="Bitmap"/> instance.
    /// </summary>
    /// <param name="bitmap">The bitmap.</param>
    public cImage(Bitmap bitmap)
      : this(bitmap != null ? bitmap.Width : 0, bitmap != null ? bitmap.Height : 0) {
      if (bitmap == null)
        return;
      var height = this._height;
      var width = this._width;

      var bitmapData = bitmap.LockBits(
        new Rectangle(0, 0, width, height),
        ImageLockMode.ReadOnly,
        PixelFormat.Format24bppRgb
      );
      var intFillX = bitmapData.Stride - bitmapData.Width * 3;
      unsafe {
        var ptrOffset = (byte*)bitmapData.Scan0.ToPointer();
        for (var intY = 0; intY < height; intY++) {
          for (var intX = 0; intX < width; intX++) {
            this[intX, intY] = new sPixel(*(ptrOffset + 2), *(ptrOffset + 1), *(ptrOffset + 0));
            ptrOffset += 3;
          }
          ptrOffset += intFillX;
        }
      }
      bitmap.UnlockBits(bitmapData);
    }
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
      : this(sourceImage._width, sourceImage._height) {
      for (long index = 0; index < sourceImage._imageData.LongLength; index++)
        this._imageData[index] = sourceImage._imageData[index];
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="cImage"/> class by filtering a given one.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    /// <param name="filterFunction">The filter.</param>
    public cImage(cImage sourceImage, Func<sPixel, sPixel> filterFunction) {
      this._width = sourceImage._width;
      this._height = sourceImage._height;
      this._imageData = new sPixel[sourceImage._imageData.LongLength];
      Parallel.ForEach(Partitioner.Create(0, this._height), () => 0, (range, _, threadStorage) => {
        for (var y = range.Item1; y < range.Item2; y++)
          for (var x = 0; x < this._width; x++)
            this[x, y] = filterFunction(sourceImage[x, y]);
        return (threadStorage);
      }, _ => {
      });
    }
    /// <summary>
    /// Initializes a new greyscale instance of the <see cref="cImage"/> class by filtering a given one.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    /// <param name="colorFilter">The greyscale filter.</param>
    public cImage(cImage sourceImage, Func<sPixel, byte> colorFilter) {
      this._width = sourceImage._width;
      this._height = sourceImage._height;
      this._imageData = new sPixel[sourceImage._imageData.LongLength];
      Parallel.ForEach(Partitioner.Create(0, this._height), () => 0, (range, _, threadStorage) => {
        for (var y = range.Item1; y < range.Item2; y++)
          for (var x = 0; x < this._width; x++)
            this[x, y] = sPixel.FromGrey(colorFilter(sourceImage[x, y]));
        return (threadStorage);
      }, _ => {
      });
    }
    /// <summary>
    /// Gets or sets the <see cref="nImager.sPixel"/> with the specified X, Y coordinates.
    /// </summary>
    /// <value>The pixel</value>
    public sPixel this[int x, int y] {
      get {
        if (x < 0)
          x = 0;
        if (y < 0)
          y = 0;
        if (x >= this._width)
          x = this._width - 1;
        if (y >= this._height)
          y = this._height - 1;

        return (this._imageData[y * this._width + x]);
      }
      set {
        if (x < this._width && y < this._height && x >= 0 && y >= 0)
          this._imageData[y * this._width + x] = value;
      }
    }
    #endregion
    #region generic image filter
    /// <summary>
    /// Filters this image by using a given filter structure.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <returns>A new instance containing the filtered image.</returns>
    private cImage _FilterImage(sFilter filter) {
      var result = filter.CreationFunction(this);
      if (filter.FilterFunctionFunction != null) {
        Parallel.ForEach(
          Partitioner.Create(0, this._height),
          () => 0,
          (range, _, threadStorage) => {
            for (var y = range.Item1; y < range.Item2; y++)
              for (var x = 0; x < this._width; x++)
                filter.FilterFunctionFunction(this, x, y, result, x * filter.ScaleX, y * filter.ScaleY, filter.ScaleX, filter.ScaleY, filter.Parameter);
            return (threadStorage);
          },
          _ => {
          }
        );
      }
      return (result);
    }
    /// <summary>
    /// Filters the current image using a named filter.
    /// </summary>
    /// <param name="filterName">The name of the filter.</param>
    /// <returns>A new instance containing the filtered image or <c>null</c>, if the specified filter could not be found.</returns>
    public cImage FilterImage(string filterName) {
      var filter =
        _imageFilters.FirstOrDefault(
          f => string.Equals(f.Name, filterName, StringComparison.InvariantCultureIgnoreCase));

      return (
        (filter.FilterFunctionFunction != null || filter.CreationFunction != null)
        ? this._FilterImage(filter)
        : null
      );
    }

    #endregion
    /// <summary>
    /// Converts this image to a <see cref="Bitmap"/> instance.
    /// </summary>
    /// <returns>The <see cref="Bitmap"/> instance</returns>
    public Bitmap ToBitmap() {
      var width = this._width;
      var height = this._height;
      var result = new Bitmap(width, height);
      // NOTE: fucking bitmap does not allow parallel writes
      var bitmapData = result.LockBits(
        new Rectangle(0, 0, result.Width, result.Height),
        ImageLockMode.WriteOnly,
        PixelFormat.Format24bppRgb
      );
      var fillBytes = bitmapData.Stride - bitmapData.Width * 3;
      unsafe {
        var offset = (byte*)bitmapData.Scan0.ToPointer();
        for (var y = 0; y < height; y++) {
          for (var x = 0; x < width; x++) {
            *(offset + 0) = this[x, y].Blue;
            *(offset + 1) = this[x, y].Green;
            *(offset + 2) = this[x, y].Red;
            offset += 3;
          }
          offset += fillBytes;
        }
      }
      result.UnlockBits(bitmapData);
      return (result);
    }
    /// <summary>
    /// Fills the image with the specified color.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    public void Fill(byte red, byte green, byte blue) {
      this.Fill(new sPixel(red, green, blue));
    }
    /// <summary>
    /// Fills the image with the specified pixel.
    /// </summary>
    /// <param name="pixel">The pixel instance.</param>
    public void Fill(sPixel pixel) {
      Parallel.For(0, this._imageData.LongLength, offset => this._imageData[offset] = pixel);
    }

    #region ICloneable Members
    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public object Clone() {
      return (new cImage(this));
    }
    #endregion
  }
}
