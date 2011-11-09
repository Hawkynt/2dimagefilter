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
using System.Drawing;
using System.Runtime.Serialization;
namespace nImager {
  /// <summary>
  /// A pixel (dword) 32Bits wide, 8 Bits for red, green and blue values.
  /// The top 8-Bits of this dword are unused.
  /// </summary>
  public struct sPixel : ICloneable, ISerializable {
    private const byte luminanceTrigger = 48;
    private const byte chromaUTrigger = 7;
    private const byte chromaVTrigger = 6;

    /// <summary>
    /// The value holding the red, green and blue component
    /// </summary>
    private UInt32 _rgbBytes;

    #region caches
    private static readonly cRGBCache _cacheY = new cRGBCache();
    private static readonly cRGBCache _cacheU = new cRGBCache();
    private static readonly cRGBCache _cacheV = new cRGBCache();

    private static readonly cRGBCache _cacheAlternateU = new cRGBCache();
    private static readonly cRGBCache _cacheAlternateV = new cRGBCache();

    private static readonly cRGBCache _cacheBrightness = new cRGBCache();
    private static readonly cRGBCache _cacheHue = new cRGBCache();
    #endregion
    #region private methods
    /// <summary>
    /// Clips a float value within 0-255 range and returns it.
    /// </summary>
    /// <param name="value">The float value to clip.</param>
    /// <returns>The clipped value</returns>
    private static byte _Float2Byte(float value) {
      return ((value < byte.MinValue) ? byte.MinValue : (value > byte.MaxValue) ? byte.MaxValue : (byte)value);
    }
    /// <summary>
    /// Gets the value for red, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="rgbBytes">The pixel value.</param>
    /// <returns>The red component</returns>
    private static byte _getRed(UInt32 rgbBytes) {
      return ((byte)(rgbBytes >> 16));
    }
    /// <summary>
    /// Gets the value for green, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="rgbBytes">The pixel value.</param>
    /// <returns>The green component</returns>
    private static byte _getGreen(UInt32 rgbBytes) {
      return ((byte)(rgbBytes >> 8));
    }
    /// <summary>
    /// Gets the value for blue, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="rgbBytes">The pixel value.</param>
    /// <returns>The blue component</returns>
    private static byte _getBlue(UInt32 rgbBytes) {
      return ((byte)(rgbBytes));
    }
    #endregion
    #region Properties

    /// <summary>
    /// Black
    /// </summary>
    public static readonly sPixel Black = FromGrey(0);
    /// <summary>
    /// White
    /// </summary>
    public static readonly sPixel White = FromGrey(255);
    /// <summary>
    /// <c>true</c> when IsLike and IsNotLike should allow little differencies in comparison; otherwise, <c>false</c>.
    /// </summary>
    public static bool AllowThresholds = true;
    /// <summary>
    /// Gets the minimum value of Red, Green and Blue.
    /// </summary>
    /// <value>The minimum.</value>
    public byte Min {
      get {
        return ((this.Red < this.Green) && (this.Red < this.Blue) ? this.Red : this.Green < this.Blue ? this.Green : this.Blue);
      }
    }
    /// <summary>
    /// Gets the maximum value of Red, Green and Blue.
    /// </summary>
    /// <value>The maximum.</value>
    public byte Max {
      get {
        return ((this.Red > this.Green) && (this.Red > this.Blue) ? this.Red : this.Green > this.Blue ? this.Green : this.Blue);
      }
    }
    /// <summary>
    /// Factor that is used to avoid noise in color extraction.
    /// The higher the factor, the lesser colors will be detected.
    /// </summary>
    public static double ColorExtractionFactor = 4;
    /// <summary>
    /// Extract the base color.
    /// </summary>
    public sPixel ExtractColors {
      get {
        var red = this.Red;
        var green = this.Green;
        var blue = this.Blue;
        /*
        var max = r > g ? r > b ? r : b > g ? b : g : b > g ? b : g;
        var add = (255 - max);
        float baseR = r;
        float baseG = g;
        float baseB = b;
        */
        float min = red < green ? red < blue ? red : blue < green ? blue : green : blue < green ? blue : green;
        var baseR = red - min;
        var baseG = green - min;
        var baseB = blue - min;
        var factorR = 255 / baseR;
        var factorG = 255 / baseG;
        var factorB = 255 / baseB;
        var useFactor = Math.Min(factorR, Math.Min(factorG, factorB));
        baseR = (float)(Math.Floor((baseR * useFactor) / ColorExtractionFactor) * ColorExtractionFactor);
        baseG = (float)(Math.Floor((baseG * useFactor) / ColorExtractionFactor) * ColorExtractionFactor);
        baseB = (float)(Math.Floor((baseB * useFactor) / ColorExtractionFactor) * ColorExtractionFactor);
        return (new sPixel((byte)baseR, (byte)baseG, (byte)baseB));
      }
    }
    public sPixel ExtractDeltas {
      get {
        var red = this.Red;
        var green = this.Green;
        var blue = this.Blue;
        var color = this.ExtractColors;
        return (new sPixel((byte)(color.Red - red), (byte)(color.Green - green), (byte)(color.Blue - blue)));
      }
    }
    /// <summary>
    /// Gets the luminance(Y).
    /// </summary>
    /// <value>The Y-value.</value>
    public byte Luminance {
      get {
        return (_cacheY.GetOrAdd(this._rgbBytes, rgbBytes => _Float2Byte(_getRed(rgbBytes) * 0.299f + _getGreen(rgbBytes) * 0.587f + _getBlue(rgbBytes) * 0.114f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(U).
    /// </summary>
    /// <value>The U-value.</value>
    public byte ChrominanceU {
      get {
        return (_cacheU.GetOrAdd(this._rgbBytes, rgbBytes => _Float2Byte(127.5f + _getRed(rgbBytes) * 0.5f - _getGreen(rgbBytes) * 0.418688f - _getBlue(rgbBytes) * 0.081312f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(V).
    /// </summary>
    /// <value>The V-value.</value>
    public byte ChrominanceV {
      get {
        return (_cacheV.GetOrAdd(this._rgbBytes, rgbBytes => _Float2Byte(127.5f - _getRed(rgbBytes) * 0.168736f - _getGreen(rgbBytes) * 0.331264f + _getBlue(rgbBytes) * 0.5f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(u).
    /// </summary>
    /// <value>The u-value.</value>
    public byte u {
      get {
        return (_cacheAlternateU.GetOrAdd(this._rgbBytes, rgbBytes => _Float2Byte(_getRed(rgbBytes) * 0.5f + _getGreen(rgbBytes) * 0.418688f + _getBlue(rgbBytes) * 0.081312f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(v).
    /// </summary>
    /// <value>The v-value.</value>
    public byte v {
      get {
        return (_cacheAlternateV.GetOrAdd(this._rgbBytes, rgbBytes => _Float2Byte(_getRed(rgbBytes) * 0.168736f + _getGreen(rgbBytes) * 0.331264f + _getBlue(rgbBytes) * 0.5f)));
      }
    }
    /// <summary>
    /// Gets the brightness.
    /// </summary>
    /// <value>The brightness.</value>
    public byte Brightness {
      get {
        return (_cacheBrightness.GetOrAdd(this._rgbBytes, dwordC => (byte)((_getRed(dwordC) * 3 + _getGreen(dwordC) * 3 + _getBlue(dwordC) * 2) >> 3)));
        //byteRet = (byte)((this.Red << 1 + this.Green << 1 + this.Blue << 1 + this.Red + this.Green) >> 3);
      }
    }
    /// <summary>
    /// Gets the hue.
    /// </summary>
    /// <value>The hue.</value>
    public byte Hue {
      get {
        return (_cacheHue.GetOrAdd(this._rgbBytes, rgbBytes => {
          float hue;
          var red = _getRed(rgbBytes);
          var green = _getGreen(rgbBytes);
          var blue = _getBlue(rgbBytes);
          var min = Math.Min(Math.Min(red, green), blue);
          var max = Math.Max(Math.Max(red, green), blue);
          float delta = max - min;
          if (max == min)
            hue = 0;
          else if (red == max)
            hue = 60 * (0 + (green - blue) / delta);
          else if (green == max)
            hue = 60 * (2 + (blue - red) / delta);
          else if (blue == max)
            hue = 60 * (4 + (red - green) / delta);
          else
            hue = 0;
          while (hue < 0)
            hue += 360;
          while (hue >= 360)
            hue -= 360;
          const float conversionFactor = 256f / 360f;
          return ((byte)(hue * conversionFactor));
        }));
      }
    }
    /// <summary>
    /// Gets an instance of type Color or sets the actual pixel to that color.
    /// </summary>
    /// <value>The color.</value>
    public Color Color {
      get {
        return (Color.FromArgb(this.Red, this.Green, this.Blue));
      }
      set {
        this.SetRGB(value.R, value.G, value.B);
      }
    }
    /// <summary>
    /// Sets the red, green and blue value for that pixel.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    public void SetRGB(byte red, byte green, byte blue) {
      this._rgbBytes = (UInt32)red << 16 | (UInt32)green << 8 | blue;
    }
    /// <summary>
    /// Gets or sets the red component.
    /// </summary>
    /// <value>The red-value.</value>
    public byte Red {
      get {
        return (_getRed(this._rgbBytes));
      }
      set {
        this.SetRGB(value, this.Green, this.Blue);
      }
    }
    /// <summary>
    /// Gets or sets the green component.
    /// </summary>
    /// <value>The green-value.</value>
    public byte Green {
      get {
        return (_getGreen(this._rgbBytes));
      }
      set {
        this.SetRGB(this.Red, value, this.Blue);
      }
    }
    /// <summary>
    /// Gets or sets the blue component.
    /// </summary>
    /// <value>The blue-value.</value>
    public byte Blue {
      get {
        return (_getBlue(this._rgbBytes));
      }
      set {
        this.SetRGB(this.Red, this.Green, value);
      }
    }
    #endregion
    #region ctor
    /// <summary>
    /// Factory to create a <see cref="sPixel"/> instance from red, green and blue value.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    /// <returns></returns>
    public static sPixel FromRGB(byte red, byte green, byte blue) {
      return (new sPixel(red, green, blue));
    }
    /// <summary>
    /// Factory to create a <see cref="sPixel"/> instance from grey value.
    /// </summary>
    /// <param name="grey">The grey value.</param>
    /// <returns></returns>
    public static sPixel FromGrey(byte grey) {
      return (new sPixel(grey));
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using an existing one.
    /// </summary>
    /// <param name="pixel">The pixel instance to copy from.</param>
    public sPixel(sPixel pixel) {
      this._rgbBytes = pixel._rgbBytes;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using a grey value.
    /// </summary>
    /// <param name="grey">The grey value.</param>
    public sPixel(byte grey)
      : this(grey, grey, grey) {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using an instance of a the <see cref="Color"/> class.
    /// </summary>
    /// <param name="color">The color.</param>
    public sPixel(Color color) {
      this._rgbBytes = 0;
      this.Color = color;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using red, green and blue component.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    public sPixel(byte red, byte green, byte blue) {
      this._rgbBytes = (UInt32)red << 16 | (UInt32)green << 8 | blue;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using red, green and blue component.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    public sPixel(double red, double green, double blue)
      : this((byte)(red * 255), (byte)(green * 255), (byte)(blue * 255)) {
    }
    #endregion
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString() {
      return ("(" + this._rgbBytes.ToString("X6") + ") Red:" + this.Red + ", Green:" + this.Green + ", Blue:" + this.Blue);
    }
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode() {
      return ((int)this._rgbBytes);
    }
    #region operators
    /// <summary>
    /// Sums up the components of two instances and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <returns>A new instance where each component gets added and overflows were clipped.</returns>
    public static sPixel operator +(sPixel pixel1, sPixel pixel2) {
      var red = pixel1.Red + pixel2.Red;
      var green = pixel1.Green + pixel2.Green;
      var blue = pixel1.Blue + pixel2.Blue;
      red = red > byte.MaxValue ? byte.MaxValue : red;
      green = green > byte.MaxValue ? byte.MaxValue : green;
      blue = blue > byte.MaxValue ? byte.MaxValue : blue;
      return (new sPixel((byte)red, (byte)green, (byte)blue));
    }
    /// <summary>
    /// Substract the components of two instances and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel1">The instance to substract from.</param>
    /// <param name="pixel2">The instance that should be substracted.</param>
    /// <returns>A new instance where each component gets substracted and underflows were clipped.</returns>
    public static sPixel operator -(sPixel pixel1, sPixel pixel2) {
      var red = pixel1.Red - pixel2.Red;
      var green = pixel1.Green - pixel2.Green;
      var blue = pixel1.Blue - pixel2.Blue;
      red = red < byte.MinValue ? byte.MinValue : red;
      green = green < byte.MinValue ? byte.MinValue : green;
      blue = blue < byte.MinValue ? byte.MinValue : blue;
      return (new sPixel((byte)red, (byte)green, (byte)blue));
    }
    /// <summary>
    /// Inverts the given color.
    /// </summary>
    /// <param name="pixel">The instance to invert.</param>
    /// <returns>A new instance with the negative color.</returns>
    public static sPixel operator !(sPixel pixel) {
      return (255 - pixel);
    }
    /// <summary>
    /// Adds a value to all components and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to add to.</param>
    /// <param name="grey">The value that should be added to each component.</param>
    /// <returns>A new instance containing the clipped values.</returns>
    public static sPixel operator +(sPixel pixel, byte grey) {
      var red = pixel.Red + grey;
      var green = pixel.Green + grey;
      var blue = pixel.Blue + grey;
      red = red > byte.MaxValue ? byte.MaxValue : red;
      green = green > byte.MaxValue ? byte.MaxValue : green;
      blue = blue > byte.MaxValue ? byte.MaxValue : blue;
      return (new sPixel((byte)red, (byte)green, (byte)blue));
    }
    /// <summary>
    /// Multiplies each color component with a specific value and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to multiply.</param>
    /// <param name="gamma">The value to be multiplied with.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator *(sPixel pixel, float gamma) {
      var red = (int)(pixel.Red * gamma);
      var green = (int)(pixel.Green * gamma);
      var blue = (int)(pixel.Blue * gamma);
      red = red > byte.MaxValue ? byte.MaxValue : red < byte.MinValue ? byte.MinValue : red;
      green = green > byte.MaxValue ? byte.MaxValue : green < byte.MinValue ? byte.MinValue : green;
      blue = blue > byte.MaxValue ? byte.MaxValue : blue < byte.MinValue ? byte.MinValue : blue;
      return (new sPixel((byte)red, (byte)green, (byte)blue));
    }
    /// <summary>
    /// Substracts all color components from a given value and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="grey">The value to substract from.</param>
    /// <param name="pixel">The isntance that holds the color components.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator -(byte grey, sPixel pixel) {
      var red = grey - pixel.Red;
      var green = grey - pixel.Green;
      var blue = grey - pixel.Blue;
      red = red < byte.MinValue ? byte.MinValue : red;
      green = green < byte.MinValue ? byte.MinValue : green;
      blue = blue < byte.MinValue ? byte.MinValue : blue;
      return (new sPixel((byte)red, (byte)green, (byte)blue));
    }
    /// <summary>
    /// Substract a given value from all color components and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to substract from.</param>
    /// <param name="grey">The value to substract.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator -(sPixel pixel, byte grey) {
      var red = pixel.Red - grey;
      var green = pixel.Green - grey;
      var blue = pixel.Blue - grey;
      red = red < byte.MinValue ? byte.MinValue : red;
      green = green < byte.MinValue ? byte.MinValue : green;
      blue = blue < byte.MinValue ? byte.MinValue : blue;
      return (new sPixel((byte)red, (byte)green, (byte)blue));
    }
    /// <summary>
    /// Divides each color component by a given value and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to be divided.</param>
    /// <param name="gamma">The value to divide by.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator /(sPixel pixel, float gamma) {
      return (pixel * (1f / gamma));
    }
    /// <summary>
    /// Adds a value to all components and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="grey">The value that should be added to each component.</param>
    /// <param name="pixel">The instance to add to.</param>
    /// <returns>A new instance containing the clipped values.</returns>
    public static sPixel operator +(byte grey, sPixel pixel) {
      return (pixel + grey);
    }
    /// <summary>
    /// Multiplies each color component with a specific value and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="gamma">The value to be multiplied with.</param>
    /// <param name="pixel">The instance to multiply.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator *(float gamma, sPixel pixel) {
      return (pixel * gamma);
    }
    /// <summary>
    /// Test for equality of all color components.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(sPixel pixel1, sPixel pixel2) {
      return (pixel1._rgbBytes == pixel2._rgbBytes);
    }
    /// <summary>
    /// Test for inequality of at least one color component.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <returns><c>true</c> if both instances differ in at least one color component; otherwise, <c>false</c>.</returns>
    public static bool operator !=(sPixel pixel1, sPixel pixel2) {
      return (pixel1._rgbBytes != pixel2._rgbBytes);
    }
    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="o">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object o) {
      return ((o is sPixel) && (((sPixel)o)._rgbBytes == this._rgbBytes));
    }
    /// <summary>
    /// Determines whether the specified <see cref="sPixel"/> is equal to this instance.
    /// </summary>
    /// <param name="pixel">The <see cref="sPixel"/> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="sPixel"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(sPixel pixel) {
      return (pixel._rgbBytes == this._rgbBytes);
    }
    /// <summary>
    /// Determines whether the specified <see cref="sPixel"/> instance is similar to this instance.
    /// </summary>
    /// <param name="pixel">The instance to compare to.</param>
    /// <returns>
    /// 	<c>true</c> if the specified instance is alike; otherwise, <c>false</c>.
    /// </returns>
    public bool IsLike(sPixel pixel) {
      if (!AllowThresholds)
        return (this == pixel);
      var delta = this.ChrominanceV - pixel.ChrominanceV;
      if (delta > chromaVTrigger || delta < -chromaVTrigger)
        return false;
      delta = this.Luminance - pixel.Luminance;
      if (delta > luminanceTrigger || delta < -luminanceTrigger)
        return false;
      delta = this.ChrominanceU - pixel.ChrominanceU;
      return delta <= chromaUTrigger && delta >= -chromaUTrigger;
    }

    /// <summary>
    /// Determines whether this instance is not like the specified <see cref="sPixel"/> instance.
    /// </summary>
    /// <param name="pixel">The instance to compare to.</param>
    /// <returns>
    /// 	<c>true</c> if the specified instance is not alike; otherwise, <c>false</c>.
    /// </returns>
    public bool IsNotLike(sPixel pixel) {
      return (!this.IsLike(pixel));
    }
    #endregion
    #region optimized interpolators
    /// <summary>
    /// Interpolates two <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first pixel instance.</param>
    /// <param name="pixel2">The second pixel instance.</param>
    /// <returns>A new instance with the interpolated color values.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2) {
      return (new sPixel(
        (byte)((pixel1.Red + pixel2.Red) >> 1),
        (byte)((pixel1.Green + pixel2.Green) >> 1),
        (byte)((pixel1.Blue + pixel2.Blue) >> 1)
      ));
    }
    /// <summary>
    /// Interpolates three <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first pixel instance.</param>
    /// <param name="pixel2">The second pixel instance.</param>
    /// <param name="pixel3">The third pixel instance.</param>
    /// <returns>A new instance with the interpolated color values.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2, sPixel pixel3) {
      return (new sPixel(
        (byte)((pixel1.Red + pixel2.Red + pixel3.Red) / 3),
        (byte)((pixel1.Green + pixel2.Green + pixel3.Green) / 3),
        (byte)((pixel1.Blue + pixel2.Blue + pixel3.Blue) / 3)
      ));
    }
    /// <summary>
    /// Interpolates four <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first pixel instance.</param>
    /// <param name="pixel2">The second pixel instance.</param>
    /// <param name="pixel3">The third pixel instance.</param>
    /// <param name="pixel4">The fourth pixel instance.</param>
    /// <returns>A new instance with the interpolated color values.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2, sPixel pixel3, sPixel pixel4) {
      return (new sPixel(
        (byte)((pixel1.Red + pixel2.Red + pixel3.Red + pixel4.Red) >> 2),
        (byte)((pixel1.Green + pixel2.Green + pixel3.Green + pixel4.Green) >> 2),
        (byte)((pixel1.Blue + pixel2.Blue + pixel3.Blue + pixel4.Blue) >> 2)
      ));
    }
    #endregion
    #region generic interpolators
    /// <summary>
    /// Weighted interpolation of two <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <param name="quantifier1">The quantifier for the first instance.</param>
    /// <param name="quantifier2">The quantifier for the second instance.</param>
    /// <returns>A new instance from the interpolated components.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2, byte quantifier1, byte quantifier2) {
      var total = (UInt16)(quantifier1 + quantifier2);
      return (new sPixel(
        (byte)((pixel1.Red * quantifier1 + pixel2.Red * quantifier2) / total),
        (byte)((pixel1.Green * quantifier1 + pixel2.Green * quantifier2) / total),
        (byte)((pixel1.Blue * quantifier1 + pixel2.Blue * quantifier2) / total)
      ));
    }
    /// <summary>
    /// Weighted interpolation of three <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <param name="pixel3">The third instance.</param>
    /// <param name="quantifier1">The quantifier for the first instance.</param>
    /// <param name="quantifier2">The quantifier for the second instance.</param>
    /// <param name="quantifier3">The quantifier for the third instance.</param>
    /// <returns>A new instance from the interpolated components.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2, sPixel pixel3, byte quantifier1, byte quantifier2, byte quantifier3) {
      var total = (UInt16)(quantifier1 + quantifier2 + quantifier3);
      return (new sPixel(
        (byte)((pixel1.Red * quantifier1 + pixel2.Red * quantifier2 + pixel3.Red * quantifier3) / total),
        (byte)((pixel1.Green * quantifier1 + pixel2.Green * quantifier2 + pixel3.Green * quantifier3) / total),
        (byte)((pixel1.Blue * quantifier1 + pixel2.Blue * quantifier2 + pixel3.Blue * quantifier3) / total)
      ));
    }
    /// <summary>
    /// Weighted interpolation of four <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <param name="pixel3">The third instance.</param>
    /// <param name="pixel4">The fourth instance.</param>
    /// <param name="quantifier1">The quantifier for the first instance.</param>
    /// <param name="quantifier2">The quantifier for the second instance.</param>
    /// <param name="quantifier3">The quantifier for the third instance.</param>
    /// <param name="quantifier4">The quantifier for the fourth instance.</param>
    /// <returns>A new instance from the interpolated components.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2, sPixel pixel3, sPixel pixel4, byte quantifier1, byte quantifier2, byte quantifier3, byte quantifier4) {
      var total = (UInt16)(quantifier1 + quantifier2 + quantifier3 + quantifier4);
      return (new sPixel(
        (byte)((pixel1.Red * quantifier1 + pixel2.Red * quantifier2 + pixel3.Red * quantifier3 + pixel4.Red * quantifier4) / total),
        (byte)((pixel1.Green * quantifier1 + pixel2.Green * quantifier2 + pixel3.Green * quantifier3 + pixel4.Green * quantifier4) / total),
        (byte)((pixel1.Blue * quantifier1 + pixel2.Blue * quantifier2 + pixel3.Blue * quantifier3 + pixel4.Blue * quantifier4) / total)
      ));
    }
    #endregion
    #region ICloneable Members
    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public object Clone() {
      return (new sPixel(this));
    }
    #endregion
    #region ISerializable Members
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by deserializing it.
    /// </summary>
    /// <param name="serializationInfo">The serialization info.</param>
    /// <param name="_">The streaming context.</param>
    public sPixel(SerializationInfo serializationInfo, StreamingContext _) {
      this._rgbBytes = (UInt32)serializationInfo.GetValue("value", typeof(UInt32));
    }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <param name="serializationInfo">The serialization info.</param>
    /// <param name="_">The streaming context.</param>
    public void GetObjectData(SerializationInfo serializationInfo, StreamingContext _) {
      serializationInfo.AddValue("value", this._rgbBytes);
    }
    #endregion
  } // end struct
} // end namespace
