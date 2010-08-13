#region (c)2010 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010 Hawkynt

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
    private const byte byteYTrigger = 48;
    private const byte byteUTrigger = 7;
    private const byte byteVTrigger = 6;

    /// <summary>
    /// The value holding the red, green and blue component
    /// </summary>
    private UInt32 _dwordPixel;

    #region caches
    private static readonly cRGBCache _hashCache_Y = new cRGBCache();
    private static readonly cRGBCache _hashCache_U = new cRGBCache();
    private static readonly cRGBCache _hashCache_V = new cRGBCache();

    private static readonly cRGBCache _hashCache_u = new cRGBCache();
    private static readonly cRGBCache _hashCache_v = new cRGBCache();

    private static readonly cRGBCache _hashCache_Brightness = new cRGBCache();
    private static readonly cRGBCache _hashCache_Hue = new cRGBCache();
    #endregion
    #region private methods
    /// <summary>
    /// Clips a float value within 0-255 range and returns it.
    /// </summary>
    /// <param name="fltA">The float value to clip.</param>
    /// <returns>The clipped value</returns>
    private static byte _byteFloat2Byte(float fltA) {
      return((fltA < byte.MinValue)?byte.MinValue:(fltA > byte.MaxValue)?byte.MaxValue:(byte)fltA);
    }
    /// <summary>
    /// Gets the value for red, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="dwordVal">The pixel value.</param>
    /// <returns>The red component</returns>
    private static byte _byteR(UInt32 dwordVal) {
      return ((byte)(dwordVal >> 16));
    }
    /// <summary>
    /// Gets the value for green, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="dwordVal">The pixel value.</param>
    /// <returns>The green component</returns>
    private static byte _byteG(UInt32 dwordVal) {
      return ((byte)(dwordVal >> 8));
    }
    /// <summary>
    /// Gets the value for blue, hopefully the compiler inlines that.
    /// </summary>
    /// <param name="dwordVal">The pixel value.</param>
    /// <returns>The blue component</returns>
    private static byte _byteB(UInt32 dwordVal) {
      return ((byte)(dwordVal));
    }
    #endregion
    #region Properties
    /// <summary>
    /// <c>true</c> when IsLike and IsNotLike should allow little differencies in comparison; otherwise, <c>false</c>.
    /// </summary>
    public static bool AllowThresholds = true;
    /// <summary>
    /// Gets the minimum value of R, G and B.
    /// </summary>
    /// <value>The minimum.</value>
    public byte Min {
      get {
        return ((this.R < this.G) && (this.R < this.B) ? this.R : this.G < this.B ? this.G : this.B);
      }
    }
    /// <summary>
    /// Gets the maximum value of R, G and B.
    /// </summary>
    /// <value>The maximum.</value>
    public byte Max {
      get {
        return ((this.R > this.G) && (this.R > this.B) ? this.R : this.G > this.B ? this.G : this.B);
      }
    }
    /// <summary>
    /// Gets the luminance(Y).
    /// </summary>
    /// <value>The Y-value.</value>
    public byte Y {
      get {
        return (_hashCache_Y.GetOrAdd(this._dwordPixel, dwordC => _byteFloat2Byte(_byteR(dwordC) * 0.299f + _byteG(dwordC) * 0.587f + _byteB(dwordC) * 0.114f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(U).
    /// </summary>
    /// <value>The U-value.</value>
    public byte U {
      get {
        return (_hashCache_U.GetOrAdd(this._dwordPixel, dwordC => _byteFloat2Byte(127.5f + _byteR(dwordC) * 0.5f - _byteG(dwordC) * 0.418688f - _byteB(dwordC) * 0.081312f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(V).
    /// </summary>
    /// <value>The V-value.</value>
    public byte V {
      get {
        return (_hashCache_V.GetOrAdd(this._dwordPixel, dwordC => _byteFloat2Byte(127.5f - _byteR(dwordC) * 0.168736f - _byteG(dwordC) * 0.331264f + _byteB(dwordC) * 0.5f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(u).
    /// </summary>
    /// <value>The u-value.</value>
    public byte u {
      get {
        return (_hashCache_u.GetOrAdd(this._dwordPixel, dwordC => _byteFloat2Byte(_byteR(dwordC) * 0.5f + _byteG(dwordC) * 0.418688f + _byteB(dwordC) * 0.081312f)));
      }
    }
    /// <summary>
    /// Gets the chrominance(v).
    /// </summary>
    /// <value>The v-value.</value>
    public byte v {
      get {
        return (_hashCache_v.GetOrAdd(this._dwordPixel, dwordC => _byteFloat2Byte(_byteR(dwordC) * 0.168736f + _byteG(dwordC) * 0.331264f + _byteB(dwordC) * 0.5f)));
      }
    }
    /// <summary>
    /// Gets the brightness.
    /// </summary>
    /// <value>The brightness.</value>
    public byte Brightness {
      get {
        return (_hashCache_Brightness.GetOrAdd(this._dwordPixel, dwordC => (byte)((_byteR(dwordC) * 3 + _byteG(dwordC) * 3 + _byteB(dwordC) * 2) >> 3)));
        //byteRet = (byte)((this.R << 1 + this.G << 1 + this.B << 1 + this.R + this.G) >> 3);
      }
    }
    /// <summary>
    /// Gets the hue.
    /// </summary>
    /// <value>The hue.</value>
    public byte Hue {
      get {
        return (_hashCache_Hue.GetOrAdd(this._dwordPixel, dwordC => {
          float fltRet;
          byte byteR = _byteR(dwordC);
          byte byteG = _byteG(dwordC);
          byte byteB = _byteB(dwordC);
          byte byteMin = Math.Min(Math.Min(byteR, byteG), byteB);
          byte byteMax = Math.Max(Math.Max(byteR, byteG), byteB);
          if (byteMin == byteMax)
            fltRet = 0;
          else if (byteR == byteMax)
            fltRet = 60 * (0 + (byteG - byteB) / (byteMax - byteMin));
          else if (byteG == byteMax)
            fltRet = 60 * (2 + (byteB - byteR) / (byteMax - byteMin));
          else if (byteB == byteMax)
            fltRet = 60 * (4 + (byteR - byteG) / (byteMax - byteMin));
          else
            fltRet = 0;
          while (fltRet < 0)
            fltRet += 360;
          while (fltRet > 360)
            fltRet -= 360;
          fltRet *= (256f / 360f);
          return ((byte)fltRet);
        }));
      }
    }
    /// <summary>
    /// Gets an instance of type Color or sets the actual pixel to that color.
    /// </summary>
    /// <value>The color.</value>
    public Color Color {
      get {
        return (Color.FromArgb(this.R, this.G, this.B));
      }
      set {
        this.SetRGB(value.R, value.G, value.B);
      }
    }
    /// <summary>
    /// Sets the red, green and blue value for that pixel.
    /// </summary>
    /// <param name="byteR">The red-value.</param>
    /// <param name="byteG">The green-value.</param>
    /// <param name="byteB">The blue-value.</param>
    public void SetRGB(byte byteR, byte byteG, byte byteB) {
      this._dwordPixel = (UInt32)byteR << 16 | (UInt32)byteG << 8 | byteB;
    }
    /// <summary>
    /// Gets or sets the red component.
    /// </summary>
    /// <value>The red-value.</value>
    public byte R {
      get {
        return (_byteR(this._dwordPixel));
      }
      set {
        this.SetRGB(value, this.G, this.B);
      }
    }
    /// <summary>
    /// Gets or sets the green component.
    /// </summary>
    /// <value>The green-value.</value>
    public byte G {
      get {
        return (_byteG(this._dwordPixel));
      }
      set {
        this.SetRGB(this.R, value, this.B);
      }
    }
    /// <summary>
    /// Gets or sets the blue component.
    /// </summary>
    /// <value>The blue-value.</value>
    public byte B {
      get {
        return (_byteB(this._dwordPixel));
      }
      set {
        this.SetRGB(this.R, this.G, value);
      }
    }
    #endregion
    #region ctor
    /// <summary>
    /// Factory to create a <see cref="sPixel"/> instance from red, green and blue value.
    /// </summary>
    /// <param name="byteR">The red-value.</param>
    /// <param name="byteG">The green-value.</param>
    /// <param name="byteB">The blue-value.</param>
    /// <returns></returns>
    public static sPixel FromRGB(byte byteR, byte byteG, byte byteB) {
      return (new sPixel(byteR, byteG, byteB));
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using an existing one.
    /// </summary>
    /// <param name="stPixel">The pixel instance to copy from.</param>
    public sPixel(sPixel stPixel) {
      this._dwordPixel = stPixel._dwordPixel;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using an instance of a the <see cref="Color"/> class.
    /// </summary>
    /// <param name="objColor">The color.</param>
    public sPixel(Color objColor) {
      this._dwordPixel = 0;
      this.Color = objColor;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="sPixel"/> struct by using red, green and blue component.
    /// </summary>
    /// <param name="byteR">The red-value.</param>
    /// <param name="byteG">The green-value.</param>
    /// <param name="byteB">The blue-value.</param>
    public sPixel(byte byteR, byte byteG, byte byteB) {
      this._dwordPixel = (UInt32)byteR << 16 | (UInt32)byteG << 8 | byteB;
    }
    #endregion
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString() {
      return ("(" + this._dwordPixel.ToString("X6") + ") Red:" + this.R + ", Green:" + this.G + ", Blue:" + B);
    }
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode() {
      return ((int)this._dwordPixel);
    }
    #region operators
    /// <summary>
    /// Sums up the components of two instances and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="stA">The first instance.</param>
    /// <param name="stB">The second instance.</param>
    /// <returns>A new instance where each component gets added and overflows were clipped.</returns>
    public static sPixel operator +(sPixel stA, sPixel stB) {
      int intR = stA.R + stB.R;
      int intG = stA.G + stB.G;
      int intB = stA.B + stB.B;
      intR = intR > byte.MaxValue ? byte.MaxValue : intR;
      intG = intG > byte.MaxValue ? byte.MaxValue : intG;
      intB = intB > byte.MaxValue ? byte.MaxValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    /// <summary>
    /// Substract the components of two instances and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="stA">The instance to substract from.</param>
    /// <param name="stB">The instance that should be substracted.</param>
    /// <returns>A new instance where each component gets substracted and underflows were clipped.</returns>
    public static sPixel operator -(sPixel stA, sPixel stB) {
      int intR = stA.R - stB.R;
      int intG = stA.G - stB.G;
      int intB = stA.B - stB.B;
      intR = intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    /// <summary>
    /// Inverts the given color.
    /// </summary>
    /// <param name="stA">The instance to invert.</param>
    /// <returns>A new instance with the negative color.</returns>
    public static sPixel operator !(sPixel stA) {
      return (255 - stA);
    }
    /// <summary>
    /// Adds a value to all components and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="stA">The instance to add to.</param>
    /// <param name="byteD">The value that should be added to each component.</param>
    /// <returns>A new instance containing the clipped values.</returns>
    public static sPixel operator +(sPixel stA, byte byteD) {
      int intR = stA.R + byteD;
      int intG = stA.G + byteD;
      int intB = stA.B + byteD;
      intR = intR > byte.MaxValue ? byte.MaxValue : intR;
      intG = intG > byte.MaxValue ? byte.MaxValue : intG;
      intB = intB > byte.MaxValue ? byte.MaxValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    /// <summary>
    /// Multiplies each color component with a specific value and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="stA">The instance to multiply.</param>
    /// <param name="fltD">The value to be multiplied with.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator *(sPixel stA, float fltD) {
      int intR = (int)(stA.R * fltD);
      int intG = (int)(stA.G * fltD);
      int intB = (int)(stA.B * fltD);
      intR = intR > byte.MaxValue ? byte.MaxValue : intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG > byte.MaxValue ? byte.MaxValue : intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB > byte.MaxValue ? byte.MaxValue : intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    /// <summary>
    /// Substracts all color components from a given value and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="byteD">The value to substract from.</param>
    /// <param name="stA">The isntance that holds the color components.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator -(byte byteD, sPixel stA) {
      int intR = byteD - stA.R;
      int intG = byteD - stA.G;
      int intB = byteD - stA.B;
      intR = intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    /// <summary>
    /// Substract a given value from all color components and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="stA">The instance to substract from.</param>
    /// <param name="byteD">The value to substract.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator -(sPixel stA, byte byteD) {
      int intR = stA.R - byteD;
      int intG = stA.G - byteD;
      int intB = stA.B - byteD;
      intR = intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    /// <summary>
    /// Divides each color component by a given value and returns the results clipped within a 0-255 range.
    /// </summary>
    /// <param name="stA">The instance to be divided.</param>
    /// <param name="fltD">The value to divide by.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator /(sPixel stA, float fltD) {
      return (stA * (1f / fltD));
    }
    /// <summary>
    /// Adds a value to all components and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="byteD">The value that should be added to each component.</param>
    /// <param name="stA">The instance to add to.</param>
    /// <returns>A new instance containing the clipped values.</returns>
    public static sPixel operator +(byte byteD, sPixel stA) {
      return (stA + byteD);
    }
    /// <summary>
    /// Multiplies each color component with a specific value and clips the result within a 0-255 range.
    /// </summary>
    /// <param name="fltD">The value to be multiplied with.</param>
    /// <param name="stA">The instance to multiply.</param>
    /// <returns>A new instance with the clipped values.</returns>
    public static sPixel operator *(float fltD, sPixel stA) {
      return (stA * fltD);
    }
    /// <summary>
    /// Test for equality of all color components.
    /// </summary>
    /// <param name="stA">The first instance.</param>
    /// <param name="stB">The second instance.</param>
    /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(sPixel stA, sPixel stB) {
      return (stA._dwordPixel == stB._dwordPixel);
    }
    /// <summary>
    /// Test for inequality of at least one color component.
    /// </summary>
    /// <param name="stA">The first instance.</param>
    /// <param name="stB">The second instance.</param>
    /// <returns><c>true</c> if both instances differ in at least one color component; otherwise, <c>false</c>.</returns>
    public static bool operator !=(sPixel stA, sPixel stB) {
      return (stA._dwordPixel != stB._dwordPixel);
    }
    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="objA">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object objA) {
      return ((objA is sPixel) && (((sPixel)objA)._dwordPixel == this._dwordPixel));
    }
    /// <summary>
    /// Determines whether the specified <see cref="sPixel"/> is equal to this instance.
    /// </summary>
    /// <param name="stA">The <see cref="sPixel"/> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="sPixel"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(sPixel stA) {
      return (stA._dwordPixel == this._dwordPixel);
    }
    /// <summary>
    /// Determines whether the specified <see cref="sPixel"/> instance is similar to this instance.
    /// </summary>
    /// <param name="stA">The instance to compare to.</param>
    /// <returns>
    /// 	<c>true</c> if the specified instance is alike; otherwise, <c>false</c>.
    /// </returns>
    public bool IsLike(sPixel stA) {
      if (AllowThresholds) {
        int intTmp = this.V - stA.V;
        if (intTmp > byteVTrigger || intTmp < -byteVTrigger)
          return false;
        intTmp = this.Y - stA.Y;
        if (intTmp > byteYTrigger || intTmp < -byteYTrigger)
          return false;
        intTmp = this.U - stA.U;
        return intTmp <= byteUTrigger && intTmp >= -byteUTrigger;
      } else {
        return( this == stA);
      }
    }
    /// <summary>
    /// Determines whether this instance is not like the specified <see cref="sPixel"/> instance.
    /// </summary>
    /// <param name="stA">The instance to compare to.</param>
    /// <returns>
    /// 	<c>true</c> if the specified instance is not alike; otherwise, <c>false</c>.
    /// </returns>
    public bool IsNotLike(sPixel stA) {
      return (!this.IsLike(stA));
    }
    #endregion
    #region optimized interpolators
    /// <summary>
    /// Interpolates two <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="stA">The first pixel instance.</param>
    /// <param name="stB">The second pixel instance.</param>
    /// <returns>A new instance with the interpolated color values.</returns>
    public static sPixel Interpolate(sPixel stA, sPixel stB) {
      return (new sPixel(
        (byte)((stA.R + stB.R) >> 1),
        (byte)((stA.G + stB.G) >> 1),
        (byte)((stA.B + stB.B) >> 1)
      ));
    }
    /// <summary>
    /// Interpolates three <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="stA">The first pixel instance.</param>
    /// <param name="stB">The second pixel instance.</param>
    /// <param name="stC">The third pixel instance.</param>
    /// <returns>A new instance with the interpolated color values.</returns>
    public static sPixel Interpolate(sPixel stA, sPixel stB, sPixel stC) {
      return (new sPixel(
        (byte)((stA.R + stB.R + stC.R) / 3),
        (byte)((stA.G + stB.G + stC.G) / 3),
        (byte)((stA.B + stB.B + stC.B) / 3)
      ));
    }
    /// <summary>
    /// Interpolates four <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="stA">The first pixel instance.</param>
    /// <param name="stB">The second pixel instance.</param>
    /// <param name="stC">The third pixel instance.</param>
    /// <param name="stD">The fourth pixel instance.</param>
    /// <returns>A new instance with the interpolated color values.</returns>
    public static sPixel Interpolate(sPixel stA, sPixel stB, sPixel stC, sPixel stD) {
      return (new sPixel(
        (byte)((stA.R + stB.R + stC.R + stD.R) >> 2),
        (byte)((stA.G + stB.G + stC.G + stD.G) >> 2),
        (byte)((stA.B + stB.B + stC.B + stD.B) >> 2)
      ));
    }
    #endregion
    #region generic interpolators
    /// <summary>
    /// Weighted interpolation of two <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="stA">The first instance.</param>
    /// <param name="stB">The second instance.</param>
    /// <param name="byteA">The quantifier for the first instance.</param>
    /// <param name="byteB">The quantifier for the second instance.</param>
    /// <returns>A new instance from the interpolated components.</returns>
    public static sPixel Interpolate(sPixel stA, sPixel stB, byte byteA, byte byteB) {
      UInt16 dwordW = (UInt16)(byteA + byteB);
      return (new sPixel(
        (byte)((stA.R * byteA + stB.R * byteB) / dwordW),
        (byte)((stA.G * byteA + stB.G * byteB) / dwordW),
        (byte)((stA.B * byteA + stB.B * byteB) / dwordW)
      ));
    }
    /// <summary>
    /// Weighted interpolation of three <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="stA">The first instance.</param>
    /// <param name="stB">The second instance.</param>
    /// <param name="stC">The third instance.</param>
    /// <param name="byteA">The quantifier for the first instance.</param>
    /// <param name="byteB">The quantifier for the second instance.</param>
    /// <param name="byteC">The quantifier for the third instance.</param>
    /// <returns>A new instance from the interpolated components.</returns>
    public static sPixel Interpolate(sPixel stA, sPixel stB, sPixel stC, byte byteA, byte byteB, byte byteC) {
      UInt16 dwordW = (UInt16)(byteA + byteB + byteC);
      return (new sPixel(
        (byte)((stA.R * byteA + stB.R * byteB + stC.R * byteC) / dwordW),
        (byte)((stA.G * byteA + stB.G * byteB + stC.G * byteC) / dwordW),
        (byte)((stA.B * byteA + stB.B * byteB + stC.B * byteC) / dwordW)
      ));
    }
    /// <summary>
    /// Weighted interpolation of four <see cref="sPixel"/> instances.
    /// </summary>
    /// <param name="stA">The first instance.</param>
    /// <param name="stB">The second instance.</param>
    /// <param name="stC">The third instance.</param>
    /// <param name="stD">The fourth instance.</param>
    /// <param name="byteA">The quantifier for the first instance.</param>
    /// <param name="byteB">The quantifier for the second instance.</param>
    /// <param name="byteC">The quantifier for the third instance.</param>
    /// <param name="byteD">The quantifier for the fourth instance.</param>
    /// <returns>A new instance from the interpolated components.</returns>
    public static sPixel Interpolate(sPixel stA, sPixel stB, sPixel stC, sPixel stD, byte byteA, byte byteB, byte byteC, byte byteD) {
      UInt16 dwordW = (UInt16)(byteA + byteB + byteC + byteD);
      return (new sPixel(
        (byte)((stA.R * byteA + stB.R * byteB + stC.R * byteC + stD.R * byteD) / dwordW),
        (byte)((stA.G * byteA + stB.G * byteB + stC.G * byteC + stD.G * byteD) / dwordW),
        (byte)((stA.B * byteA + stB.B * byteB + stC.B * byteC + stD.B * byteD) / dwordW)
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
    /// <param name="objSerializationInfo">The serialization info.</param>
    /// <param name="objStreamingContext">The streaming context.</param>
    public sPixel(SerializationInfo objSerializationInfo, StreamingContext objStreamingContext) {
      this._dwordPixel = (UInt32)objSerializationInfo.GetValue("value", typeof(UInt32));
    }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <param name="objSerializationInfo">The serialization info.</param>
    /// <param name="objStreamingContext">The streaming context.</param>
    public void GetObjectData(SerializationInfo objSerializationInfo, StreamingContext objStreamingContext) {
      objSerializationInfo.AddValue("value", this._dwordPixel);
    }
    #endregion
  } // end struct
} // end namespace
