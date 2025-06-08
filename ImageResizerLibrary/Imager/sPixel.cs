﻿#region (c)2008-2019 Hawkynt
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

using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using dword = System.UInt32;

namespace Imager {
  /// <summary>
  /// A pixel (dword) 32Bits wide, 8 Bits for red, green, blue and alpha values.
  /// </summary>
  public struct sPixel : ICloneable, ISerializable {
    private const byte _LUMINANCE_TRIGGER = 48;
    private const byte _CHROMA_U_TRIGGER = 7;
    private const byte _CHROMA_V_TRIGGER = 6;
    private const int _RGB_MASK = 0xffffff;
    private const int _ALPHA_SHIFT = 24;
    private const int _RED_SHIFT = 16;
    private const int _GREEN_SHIFT = 8;
    private const int _BLUE_SHIFT = 0;

    /// <summary>
    /// The value holding the red, green, blue and alpha component
    /// </summary>
    private readonly dword _argbBytes;

    #region caches

    private static readonly cRGBCache _CACHE_Y = new cRGBCache();
    private static readonly cRGBCache _CACHE_U = new cRGBCache();
    private static readonly cRGBCache _CACHE_V = new cRGBCache();

    private static readonly cRGBCache _CACHE_ALTERNATE_U = new cRGBCache();
    private static readonly cRGBCache _CACHE_ALTERNATE_V = new cRGBCache();

    private static readonly cRGBCache _CACHE_BRIGHTNESS = new cRGBCache();
    private static readonly cRGBCache _CACHE_HUE = new cRGBCache();

    #endregion

    #region private methods

    /// <summary>
    /// Gets the maximum of three bytes.
    /// </summary>
    /// <param name="a">The a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <returns></returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _Max(byte a, byte b, byte c) {
      var d = a > b ? a : b;
      return d > c ? d : c;
    }

    /// <summary>
    /// Gets the minimum of three bytes.
    /// </summary>
    /// <param name="a">The a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <returns></returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _Min(byte a, byte b, byte c) {
      var d = a < b ? a : b;
      return d < c ? d : c;
    }

    /// <summary>
    /// Converts a byte color component to single floating point.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static float _Byte2Single(byte value) => value / 255f;

    /// <summary>
    /// Converts a byte color component to double floating point.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static double _Byte2Double(byte value) => value / 255d;

    /// <summary>
    /// Clamps ints to the max byte range.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _TopClamp(int value) => value > byte.MaxValue ? byte.MaxValue : (byte)value;

    /// <summary>
    /// Clamps ints to the byte range.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _FullClamp(int value) => value > byte.MaxValue ? byte.MaxValue : value < byte.MinValue ? byte.MinValue : (byte)value;

    /// <summary>
    /// Clips a float value within 0-255 range and returns it.
    /// </summary>
    /// <param name="value">The float value to clip.</param>
    /// <returns>The clipped value</returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _FullClamp(float value) => value > byte.MaxValue ? byte.MaxValue : value < byte.MinValue ? byte.MinValue : (byte)value;

    /// <summary>
    /// Gets the value for alpha, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="rgbBytes">The pixel value.</param>
    /// <returns>The alpha component</returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _GetAlpha(uint rgbBytes) => (byte)(rgbBytes >> _ALPHA_SHIFT);

    /// <summary>
    /// Gets the value for red, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="rgbBytes">The pixel value.</param>
    /// <returns>The red component</returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _GetRed(uint rgbBytes) => (byte)(rgbBytes >> _RED_SHIFT);

    /// <summary>
    /// Gets the value for green, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="rgbBytes">The pixel value.</param>
    /// <returns>The green component</returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _GetGreen(uint rgbBytes) => (byte)(rgbBytes >> _GREEN_SHIFT);

    /// <summary>
    /// Gets the value for blue, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="rgbBytes">The pixel value.</param>
    /// <returns>The blue component</returns>
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static byte _GetBlue(uint rgbBytes) => (byte)(rgbBytes >> _BLUE_SHIFT);

    #endregion

    #region Properties

    /// <summary>
    /// Black
    /// </summary>
    public static readonly sPixel Black = FromGrey(byte.MinValue);

    /// <summary>
    /// White
    /// </summary>
    public static readonly sPixel White = FromGrey(byte.MaxValue);

    /// <summary>
    /// Transparent
    /// </summary>
    public static readonly sPixel Transparent = FromRGBA(byte.MaxValue, byte.MinValue, byte.MaxValue, byte.MinValue);

    /// <summary>
    /// Purple
    /// </summary>
    public static readonly sPixel Purple = FromRGBA(byte.MaxValue, byte.MinValue, byte.MaxValue);

    /// <summary>
    /// <c>true</c> when IsLike and IsNotLike should allow little differencies in comparison; otherwise, <c>false</c>.
    /// </summary>
    public static bool AllowThresholds = true;

    /// <summary>
    /// Gets the minimum value of Red, Green and Blue.
    /// </summary>
    /// <value>The minimum.</value>
    public byte Minimum => _Min(this.Red, this.Green, this.Blue);

    /// <summary>
    /// Gets the maximum value of Red, Green and Blue.
    /// </summary>
    /// <value>The maximum.</value>
    public byte Maximum => _Max(this.Red, this.Green, this.Blue);

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
        var add = (byte.MaxValue - max);
        float baseR = r;
        float baseG = g;
        float baseB = b;
        */
        float min = _Min(red, green, blue);
        var baseR = red - min;
        var baseG = green - min;
        var baseB = blue - min;
        var factorR = byte.MaxValue / baseR;
        var factorG = byte.MaxValue / baseG;
        var factorB = byte.MaxValue / baseB;
        var useFactor = Math.Min(factorR, Math.Min(factorG, factorB));
        baseR = (float)(Math.Floor(baseR * useFactor / ColorExtractionFactor) * ColorExtractionFactor);
        baseG = (float)(Math.Floor(baseG * useFactor / ColorExtractionFactor) * ColorExtractionFactor);
        baseB = (float)(Math.Floor(baseB * useFactor / ColorExtractionFactor) * ColorExtractionFactor);
        return new sPixel((byte)baseR, (byte)baseG, (byte)baseB, this.Alpha);
      }
    }

    public sPixel ExtractDeltas {
      get {
        var red = this.Red;
        var green = this.Green;
        var blue = this.Blue;
        var color = this.ExtractColors;
        return
          new sPixel((byte)(color.Red - red), (byte)(color.Green - green), (byte)(color.Blue - blue), this.Alpha);
      }
    }

    /// <summary>
    /// Gets the luminance(Y).
    /// </summary>
    /// <value>The Y-value.</value>
    public byte Luminance => _CACHE_Y.GetOrAdd(this._argbBytes & _RGB_MASK, _CalculateLuminance);
    private static byte _CalculateLuminance(uint rgbBytes)
      => _TopClamp((_GetRed(rgbBytes) * 299 + _GetGreen(rgbBytes) * 587 + _GetBlue(rgbBytes) * 114) / 1000)
      ;

    /// <summary>
    /// Gets the chrominance(U).
    /// </summary>
    /// <value>The U-value.</value>
    public byte ChrominanceU => _CACHE_U.GetOrAdd(this._argbBytes & _RGB_MASK, _CalculateChrominanceU);
    private static byte _CalculateChrominanceU(uint rgbBytes)
      => _FullClamp((127500000 + _GetRed(rgbBytes) * 500000 - _GetGreen(rgbBytes) * 418688 - _GetBlue(rgbBytes) * 081312) / 1000000)
      ;
    /// <summary>
    /// Gets the chrominance(V).
    /// </summary>
    /// <value>The V-value.</value>
    public byte ChrominanceV => _CACHE_V.GetOrAdd(this._argbBytes & _RGB_MASK, _CalculateChrominanceV);
    private static byte _CalculateChrominanceV(uint rgbBytes)
      => _FullClamp((127500000 - _GetRed(rgbBytes) * 168736 - _GetGreen(rgbBytes) * 331264 + _GetBlue(rgbBytes) * 500000) / 1000000)
      ;

    /// <summary>
    /// Gets the chrominance(u).
    /// </summary>
    /// <value>The u-value.</value>
    public byte u {
      get {
        return
          _CACHE_ALTERNATE_U.GetOrAdd(
            this._argbBytes & _RGB_MASK,
            rgbBytes =>
              _TopClamp(
                (_GetRed(rgbBytes) * 500000 + _GetGreen(rgbBytes) * 418688 + _GetBlue(rgbBytes) * 081312) / 1000000));
      }
    }

    /// <summary>
    /// Gets the chrominance(v).
    /// </summary>
    /// <value>The v-value.</value>
    public byte v {
      get {
        return
          _CACHE_ALTERNATE_V.GetOrAdd(
            this._argbBytes & _RGB_MASK,
            rgbBytes =>
              _TopClamp(
                (_GetRed(rgbBytes) * 168736 + _GetGreen(rgbBytes) * 331264 + _GetBlue(rgbBytes) * 500000) / 1000000));
      }
    }

    /// <summary>
    /// Gets the brightness.
    /// </summary>
    /// <value>The brightness.</value>
    public byte Brightness {
      get {
        return
          _CACHE_BRIGHTNESS.GetOrAdd(
            this._argbBytes & _RGB_MASK,
            dwordC => (byte)((_GetRed(dwordC) * 3 + _GetGreen(dwordC) * 3 + _GetBlue(dwordC) * 2) >> 3));
      }
    }

    /// <summary>
    /// Gets the hue.
    /// </summary>
    /// <value>The hue.</value>
    public byte Hue {
      get {
        return _CACHE_HUE.GetOrAdd(
          this._argbBytes & _RGB_MASK,
          rgbBytes => {
            float hue;
            var red = _GetRed(rgbBytes);
            var green = _GetGreen(rgbBytes);
            var blue = _GetBlue(rgbBytes);
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
            return (byte)(hue * conversionFactor);
          });
      }
    }

    /// <summary>
    /// Gets an instance of type Color or sets the actual pixel to that color.
    /// </summary>
    /// <value>The color.</value>
    public Color Color => Color.FromArgb(this.Alpha, this.Red, this.Green, this.Blue);

    #region byte values

    /// <summary>
    /// Gets or sets the alpha component.
    /// </summary>
    /// <value>The alpha-value.</value>
    public byte Alpha => _GetAlpha(this._argbBytes);

    /// <summary>
    /// Gets or sets the red component.
    /// </summary>
    /// <value>The red-value.</value>
    public byte Red => _GetRed(this._argbBytes);

    /// <summary>
    /// Gets or sets the green component.
    /// </summary>
    /// <value>The green-value.</value>
    public byte Green => _GetGreen(this._argbBytes);

    /// <summary>
    /// Gets or sets the blue component.
    /// </summary>
    /// <value>The blue-value.</value>
    public byte Blue => _GetBlue(this._argbBytes);

    #endregion

    #region float values

    public double DoubleRed => _Byte2Double(this.Red);
    public float SingleRed => _Byte2Single(this.Red);
    public double DoubleGreen => _Byte2Double(this.Green);
    public float SingleGreen => _Byte2Single(this.Green);
    public double DoubleBlue => _Byte2Double(this.Blue);
    public float SingleBlue => _Byte2Single(this.Blue);
    public double DoubleAlpha => _Byte2Double(this.Alpha);
    public float SingleAlpha => _Byte2Single(this.Alpha);

    #endregion

    #endregion

    #region ctor

    public static sPixel FromFloat(float red, float green, float blue, float alpha = 1f) {
      return new sPixel(
        _FullClamp(red * 255 + .5f),
        _FullClamp(green * 255 + .5f),
        _FullClamp(blue * 255 + .5f),
        _FullClamp(alpha * 255 + .5f)
      );
    }

    /// <summary>
    /// Factory to create a <see cref="sPixel"/> instance from red, green and blue value.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    /// <param name="alpha">The alpha-value.</param>
    /// <returns></returns>
    public static sPixel FromRGBA(byte red, byte green, byte blue, byte alpha = byte.MaxValue) => new sPixel(red, green, blue, alpha);

    /// <summary>
    /// Factory to create a <see cref="sPixel"/> instance from red, green and blue value.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    /// <param name="alpha">The alpha-value.</param>
    /// <returns></returns>
    public static sPixel FromRGBA(int red, int green, int blue, int alpha = byte.MaxValue) {
      return
        new sPixel(
          _FullClamp((float)red),
          _FullClamp((float)green),
          _FullClamp((float)blue),
          _FullClamp((float)alpha));
    }

    /// <summary>
    /// Factory to create a <see cref="sPixel"/> instance from grey value.
    /// </summary>
    /// <param name="grey">The grey value.</param>
    /// <param name="alpha">The alpha value.</param>
    /// <returns></returns>
    public static sPixel FromGrey(byte grey, byte alpha = byte.MaxValue) => new sPixel(grey, alpha);

    /// <summary>
    /// Prevents a default instance of the <see cref="sPixel"/> struct from being created.
    /// </summary>
    /// <param name="argbData">The RGB data.</param>
    private sPixel(dword argbData) {
      this._argbBytes = argbData;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using an existing one.
    /// </summary>
    /// <param name="pixel">The pixel instance to copy from.</param>
    public sPixel(sPixel pixel)
      : this(pixel._argbBytes) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using a grey value.
    /// </summary>
    /// <param name="grey">The grey value.</param>
    /// <param name="alpha">The alpha value.</param>
    public sPixel(byte grey, byte alpha = byte.MaxValue)
      : this(grey, grey, grey, alpha) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using an instance of a the <see cref="Color"/> class.
    /// </summary>
    /// <param name="color">The color.</param>
    public sPixel(Color color)
      : this(color.R, color.G, color.B, color.A) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using red, green and blue component.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    /// <param name="alpha">The alpha-value.</param>
    public sPixel(byte red, byte green, byte blue, byte alpha = byte.MaxValue) {
      this._argbBytes = (uint)alpha << _ALPHA_SHIFT | (uint)red << _RED_SHIFT | (uint)green << _GREEN_SHIFT | (uint)blue << _BLUE_SHIFT;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using red, green and blue component.
    /// </summary>
    /// <param name="red">The red-value.</param>
    /// <param name="green">The green-value.</param>
    /// <param name="blue">The blue-value.</param>
    /// <param name="alpha">The alpha.</param>
    public sPixel(double red, double green, double blue, double alpha = 1)
      : this((byte)(red * byte.MaxValue), (byte)(green * byte.MaxValue), (byte)(blue * byte.MaxValue), (byte)(alpha * byte.MaxValue)) {
      Contract.Requires(red >= 0 && red <= 1);
      Contract.Requires(green >= 0 && green <= 1);
      Contract.Requires(blue >= 0 && blue <= 1);
      Contract.Requires(alpha >= 0 && alpha <= 1);
    }

    #endregion

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString() => $"({this._argbBytes:X8}) Red:{this.Red}, Green:{this.Green}, Blue:{this.Blue}, Alpha:{this.Alpha}";

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode() => (int)this._argbBytes;

    #region operators

    /// <summary>
    /// Sums up the components of two instances and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <returns>A new instance where each component gets added and overflows were clipped.</returns>
    public static sPixel operator +(sPixel pixel, sPixel pixel2) {
      int red, green, blue, alpha;
      if (byte.MaxValue < (red = pixel.Red + pixel2.Red))
        red = byte.MaxValue;
      if (byte.MaxValue < (green = pixel.Green + pixel2.Green))
        green = byte.MaxValue;
      if (byte.MaxValue < (blue = pixel.Blue + pixel2.Blue))
        blue = byte.MaxValue;
      if (byte.MaxValue < (alpha = pixel.Alpha + pixel2.Alpha))
        alpha = byte.MaxValue;

      return new sPixel((byte)red, (byte)green, (byte)blue, (byte)alpha);
    }

    /// <summary>
    /// Substract the components of two instances and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel1">The instance to substract from.</param>
    /// <param name="pixel2">The instance that should be substracted.</param>
    /// <returns>A new instance where each component gets substracted and underflows were clipped.</returns>
    public static sPixel operator -(sPixel pixel1, sPixel pixel2) {
      int red, green, blue, alpha;
      if (byte.MinValue > (red = pixel1.Red - pixel2.Red))
        red = byte.MinValue;
      if (byte.MinValue > (green = pixel1.Green - pixel2.Green))
        green = byte.MinValue;
      if (byte.MinValue > (blue = pixel1.Blue - pixel2.Blue))
        blue = byte.MinValue;
      if (byte.MinValue > (alpha = pixel1.Alpha - pixel2.Alpha))
        alpha = byte.MinValue;

      return new sPixel((byte)red, (byte)green, (byte)blue, (byte)alpha);
    }

    /// <summary>
    /// Inverts the given color.
    /// </summary>
    /// <param name="pixel">The instance to invert.</param>
    /// <returns>A new instance with the negative color.</returns>
    public static sPixel operator !(sPixel pixel) => byte.MaxValue - pixel;

    /// <summary>
    /// Adds a value to all components and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to add to.</param>
    /// <param name="grey">The value that should be added to each component.</param>
    /// <returns>A new instance containing the clipped values.</returns>
    public static sPixel operator +(sPixel pixel, byte grey) {
      int red, green, blue;
      if (byte.MaxValue < (red = pixel.Red + grey))
        red = byte.MaxValue;
      if (byte.MaxValue < (green = pixel.Green + grey))
        green = byte.MaxValue;
      if (byte.MaxValue < (blue = pixel.Blue + grey))
        blue = byte.MaxValue;

      return new sPixel((byte)red, (byte)green, (byte)blue, pixel.Alpha);
    }

    /// <summary>
    /// Multiplies each color component with a specific value and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to multiply.</param>
    /// <param name="gamma">The value to be multiplied with.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator *(sPixel pixel, float gamma) {
      var red = _FullClamp(pixel.Red * gamma);
      var green = _FullClamp(pixel.Green * gamma);
      var blue = _FullClamp(pixel.Blue * gamma);
      return new sPixel(red, green, blue, pixel.Alpha);
    }

    /// <summary>
    /// Substracts all color components from a given value and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="grey">The value to substract from.</param>
    /// <param name="pixel">The isntance that holds the color components.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator -(byte grey, sPixel pixel) {
      int red, green, blue;
      if (byte.MinValue > (red = grey - pixel.Red))
        red = byte.MinValue;
      if (byte.MinValue > (green = grey - pixel.Green))
        green = byte.MinValue;
      if (byte.MinValue > (blue = grey - pixel.Blue))
        blue = byte.MinValue;

      return new sPixel((byte)red, (byte)green, (byte)blue, pixel.Alpha);
    }

    /// <summary>
    /// Substract a given value from all color components and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to substract from.</param>
    /// <param name="grey">The value to substract.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator -(sPixel pixel, byte grey) {
      int red, green, blue;
      if (byte.MinValue > (red = pixel.Red - grey))
        red = byte.MinValue;
      if (byte.MinValue > (green = pixel.Green - grey))
        green = byte.MinValue;
      if (byte.MinValue > (blue = pixel.Blue - grey))
        blue = byte.MinValue;

      return new sPixel((byte)red, (byte)green, (byte)blue, pixel.Alpha);
    }

    /// <summary>
    /// Divides each color component by a given value and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="pixel">The instance to be divided.</param>
    /// <param name="gamma">The value to divide by.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator /(sPixel pixel, float gamma) => pixel * (1f / gamma);

    /// <summary>
    /// Adds a value to all components and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="grey">The value that should be added to each component.</param>
    /// <param name="pixel">The instance to add to.</param>
    /// <returns>A new instance containing the clipped values.</returns>
    public static sPixel operator +(byte grey, sPixel pixel) => pixel + grey;

    /// <summary>
    /// Multiplies each color component with a specific value and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="gamma">The value to be multiplied with.</param>
    /// <param name="pixel">The instance to multiply.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator *(float gamma, sPixel pixel) => pixel * gamma;

    /// <summary>
    /// Test for equality of all color components.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(sPixel pixel1, sPixel pixel2) => pixel1._argbBytes == pixel2._argbBytes;

    /// <summary>
    /// Test for inequality of at least one color component.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <returns><c>true</c> if both instances differ in at least one color component; otherwise, <c>false</c>.</returns>
    public static bool operator !=(sPixel pixel1, sPixel pixel2) => pixel1._argbBytes != pixel2._argbBytes;

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="o">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object o) => o is sPixel && ((sPixel)o)._argbBytes == this._argbBytes;

    /// <summary>
    /// Determines whether the specified <see cref="sPixel"/> is equal to this instance.
    /// </summary>
    /// <param name="pixel">The <see cref="sPixel"/> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="sPixel"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(sPixel pixel) => pixel._argbBytes == this._argbBytes;

    /// <summary>
    /// Determines whether the specified <see cref="sPixel"/> instance is similar to this instance.
    /// </summary>
    /// <param name="pixel">The instance to compare to.</param>
    /// <returns>
    /// 	<c>true</c> if the specified instance is alike; otherwise, <c>false</c>.
    /// </returns>
    public bool IsLike(sPixel pixel) {
      if (!AllowThresholds)
        return this == pixel;

      var delta = this.Luminance - pixel.Luminance;
      if (delta.Abs() > _LUMINANCE_TRIGGER)
        return false;

      delta = this.ChrominanceV - pixel.ChrominanceV;
      if (delta.Abs() > _CHROMA_V_TRIGGER)
        return false;

      delta = this.ChrominanceU - pixel.ChrominanceU;
      return delta.Abs() <= _CHROMA_U_TRIGGER;
    }

    /// <summary>
    /// Determines whether this instance is not like the specified <see cref="sPixel"/> instance.
    /// </summary>
    /// <param name="pixel">The instance to compare to.</param>
    /// <returns>
    /// 	<c>true</c> if the specified instance is not alike; otherwise, <c>false</c>.
    /// </returns>
    public bool IsNotLike(sPixel pixel) => !this.IsLike(pixel);

    /// <summary>
    /// Calculates the absolute difference between to pixels.
    /// </summary>
    /// <param name="pixel">The pixel to differ to.</param>
    /// <returns>The absolute difference.</returns>
    public uint AbsDifference(sPixel pixel) {
      return _LUMINANCE_TRIGGER * (this.Luminance - pixel.Luminance).Abs()
             + _CHROMA_V_TRIGGER * (this.ChrominanceV - pixel.ChrominanceV).Abs()
             + _CHROMA_U_TRIGGER * (this.ChrominanceU - pixel.ChrominanceU).Abs();
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
      return new sPixel(
        (byte)((pixel1.Red + pixel2.Red) >> 1),
        (byte)((pixel1.Green + pixel2.Green) >> 1),
        (byte)((pixel1.Blue + pixel2.Blue) >> 1),
        (byte)((pixel1.Alpha + pixel2.Alpha) >> 1)
      );
    }

    /// <summary>
    /// Interpolates three <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first pixel instance.</param>
    /// <param name="pixel2">The second pixel instance.</param>
    /// <param name="pixel3">The third pixel instance.</param>
    /// <returns>A new instance with the interpolated color values.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2, sPixel pixel3) {
      return new sPixel(
        (byte)((pixel1.Red + pixel2.Red + pixel3.Red) / 3),
        (byte)((pixel1.Green + pixel2.Green + pixel3.Green) / 3),
        (byte)((pixel1.Blue + pixel2.Blue + pixel3.Blue) / 3),
        (byte)((pixel1.Alpha + pixel2.Alpha + pixel3.Alpha) / 3)
      );
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
      return new sPixel(
        (byte)((pixel1.Red + pixel2.Red + pixel3.Red + pixel4.Red) >> 2),
        (byte)((pixel1.Green + pixel2.Green + pixel3.Green + pixel4.Green) >> 2),
        (byte)((pixel1.Blue + pixel2.Blue + pixel3.Blue + pixel4.Blue) >> 2),
        (byte)((pixel1.Alpha + pixel2.Alpha + pixel3.Alpha + pixel4.Alpha) >> 2)
      );
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
      var total = (ushort)(quantifier1 + quantifier2);
      return new sPixel(
        (byte)((pixel1.Red * quantifier1 + pixel2.Red * quantifier2) / total),
        (byte)((pixel1.Green * quantifier1 + pixel2.Green * quantifier2) / total),
        (byte)((pixel1.Blue * quantifier1 + pixel2.Blue * quantifier2) / total),
        (byte)((pixel1.Alpha * quantifier1 + pixel2.Alpha * quantifier2) / total)
      );
    }

    /// <summary>
    /// Weighted interpolation of two <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="pixel1">The first instance.</param>
    /// <param name="pixel2">The second instance.</param>
    /// <param name="quantifier1">The quantifier for the first instance.</param>
    /// <param name="quantifier2">The quantifier for the second instance.</param>
    /// <returns>A new instance from the interpolated components.</returns>
    public static sPixel Interpolate(sPixel pixel1, sPixel pixel2, int quantifier1, int quantifier2) {
      var total = (uint)(quantifier1 + quantifier2);
      return new sPixel(
        (byte)((pixel1.Red * quantifier1 + pixel2.Red * quantifier2) / total),
        (byte)((pixel1.Green * quantifier1 + pixel2.Green * quantifier2) / total),
        (byte)((pixel1.Blue * quantifier1 + pixel2.Blue * quantifier2) / total),
        (byte)((pixel1.Alpha * quantifier1 + pixel2.Alpha * quantifier2) / total)
      );
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
    public static sPixel Interpolate(
      sPixel pixel1,
      sPixel pixel2,
      sPixel pixel3,
      byte quantifier1,
      byte quantifier2,
      byte quantifier3) {
      var total = (ushort)(quantifier1 + quantifier2 + quantifier3);
      return new sPixel(
        (byte)((pixel1.Red * quantifier1 + pixel2.Red * quantifier2 + pixel3.Red * quantifier3) / total),
        (byte)((pixel1.Green * quantifier1 + pixel2.Green * quantifier2 + pixel3.Green * quantifier3) / total),
        (byte)((pixel1.Blue * quantifier1 + pixel2.Blue * quantifier2 + pixel3.Blue * quantifier3) / total),
        (byte)((pixel1.Alpha * quantifier1 + pixel2.Alpha * quantifier2 + pixel3.Alpha * quantifier3) / total)
      );
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
    public static sPixel Interpolate(
      sPixel pixel1,
      sPixel pixel2,
      sPixel pixel3,
      sPixel pixel4,
      byte quantifier1,
      byte quantifier2,
      byte quantifier3,
      byte quantifier4) {
      var total = (ushort)(quantifier1 + quantifier2 + quantifier3 + quantifier4);
      return new sPixel(
        (byte)
        ((pixel1.Red * quantifier1 + pixel2.Red * quantifier2 + pixel3.Red * quantifier3 + pixel4.Red * quantifier4) /
         total),
        (byte)
        ((pixel1.Green * quantifier1 + pixel2.Green * quantifier2 + pixel3.Green * quantifier3 +
          pixel4.Green * quantifier4) / total),
        (byte)
        ((pixel1.Blue * quantifier1 + pixel2.Blue * quantifier2 + pixel3.Blue * quantifier3 +
          pixel4.Blue * quantifier4) / total),
        (byte)
        ((pixel1.Alpha * quantifier1 + pixel2.Alpha * quantifier2 + pixel3.Alpha * quantifier3 +
          pixel4.Alpha * quantifier4) / total)
      );
    }

    #endregion

    #region ICloneable Members

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public object Clone() => new sPixel(this);

    #endregion

    #region ISerializable Members

    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by deserializing it.
    /// </summary>
    /// <param name="serializationInfo">The serialization info.</param>
    /// <param name="_">The streaming context.</param>
    public sPixel(SerializationInfo serializationInfo, StreamingContext _) {
      this._argbBytes = (dword)serializationInfo.GetValue("value", typeof(dword));
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <param name="serializationInfo">The serialization info.</param>
    /// <param name="_">The streaming context.</param>
    public void GetObjectData(SerializationInfo serializationInfo, StreamingContext _)
      => serializationInfo.AddValue("value", this._argbBytes)
      ;

    #endregion
  } // end struct

  internal static partial class MathExtensions {
    [TargetedPatchingOptOut("")]
#if SUPPORTS_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static uint Abs(this int v) {
#if DEBUG
      return (uint)(v < 0 ? -v : v);
#else
      var mask = v >> 31;
      var r = (uint)((v ^ mask) - mask);
      return (r);
#endif
    }
  }
} // end namespace
