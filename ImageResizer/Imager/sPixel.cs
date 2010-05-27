using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nImager {
  public struct sPixel : ICloneable, System.Runtime.Serialization.ISerializable {
    private UInt32 _dwordPixel;

    #region caches
    private class cRGBCache {
      private Dictionary<UInt32, byte> _hashCache = new Dictionary<uint, byte>();
      public cRGBCache(){
      }
      public bool TryGetValue(UInt32 dwordKey,out byte byteData) {
        return(this._hashCache.TryGetValue(dwordKey,out byteData));
      }
      public byte this[UInt32 dwordKey] {
        set {
          lock (this._hashCache)
            if (!this._hashCache.ContainsKey(dwordKey))
              this._hashCache.Add(dwordKey, value);
        }
        get {
          return(this._hashCache[dwordKey]);
        }
      }
    }
    private static cRGBCache _hashCache_Y = new cRGBCache();
    private static cRGBCache _hashCache_U = new cRGBCache();
    private static cRGBCache _hashCache_V = new cRGBCache();

    private static cRGBCache _hashCache_u = new cRGBCache();
    private static cRGBCache _hashCache_v = new cRGBCache();

    private static cRGBCache _hashCache_Brightness = new cRGBCache();
    private static cRGBCache _hashCache_Hue = new cRGBCache();
    #endregion
    private static byte _byteFloat2Byte(float fltA) {
      byte byteRet;
      if (fltA < byte.MinValue)
        byteRet = byte.MinValue;
      else if (fltA > byte.MaxValue)
        byteRet = byte.MaxValue;
      else
        byteRet = (byte)fltA;
      return (byteRet);
    }
    #region Properties

    public byte Min {
      get {
        return ((this.R < this.G) && (this.R < this.B) ? this.R : this.G < this.B ? this.G : this.B);
      }
    }
    public byte Max {
      get {
        return ((this.R > this.G) && (this.R > this.B) ? this.R : this.G > this.B ? this.G : this.B);
      }
    }
    public byte Y {
      get {
        byte byteRet;
        UInt32 dwordC = this._dwordPixel;
        if (!_hashCache_Y.TryGetValue(dwordC, out byteRet)) {
          byteRet = _byteFloat2Byte(this.R * 0.299f + this.G * 0.587f + this.B * 0.114f);
          _hashCache_Y[dwordC] = byteRet;
        }
        return (byteRet);
      }
    }
    public byte U {
      get {
        byte byteRet;
        UInt32 dwordC = this._dwordPixel;
        if (!_hashCache_U.TryGetValue(dwordC, out byteRet)) {
          byteRet = _byteFloat2Byte(127.5f + this.R * 0.5f - this.G * 0.418688f - this.B * 0.081312f);
          _hashCache_U[dwordC] = byteRet;
        }
        return (byteRet);
      }
    }
    public byte V {
      get {
        byte byteRet;
        UInt32 dwordC = this._dwordPixel;
        if (!_hashCache_V.TryGetValue(dwordC, out byteRet)) {
          byteRet = _byteFloat2Byte(127.5f - this.R * 0.168736f - this.G * 0.331264f + this.B * 0.5f);
          _hashCache_V[dwordC] = byteRet;
        }
        return (byteRet);
      }
    }
    public byte u {
      get {
        byte byteRet;
        UInt32 dwordC = this._dwordPixel;
        if (!_hashCache_u.TryGetValue(dwordC, out byteRet)) {
          byteRet = _byteFloat2Byte(this.R * 0.5f + this.G * 0.418688f + this.B * 0.081312f);
          _hashCache_u[dwordC] = byteRet;
        }
        return (byteRet);
      }
    }
    public byte v {
      get {
        byte byteRet;
        UInt32 dwordC = this._dwordPixel;
        if (!_hashCache_v.TryGetValue(dwordC, out byteRet)) {
          byteRet = _byteFloat2Byte(this.R * 0.168736f + this.G * 0.331264f + this.B * 0.5f);
          _hashCache_v[dwordC] = byteRet;
        }
        return (byteRet);
      }
    }
    public byte Brightness {
      get {
        byte byteRet;
        UInt32 dwordC = this._dwordPixel;
        if (!_hashCache_Brightness.TryGetValue(dwordC, out byteRet)) {
          byteRet = (byte)((this.R << 1 + this.G << 1 + this.B << 1 + this.R + this.G) >> 3);
          _hashCache_Brightness[dwordC] = byteRet;
        }
        return (byteRet);
      }
    }
    public byte Hue {
      get {
        byte byteRet;
        UInt32 dwordC = this._dwordPixel;
        if (!_hashCache_Hue.TryGetValue(dwordC, out byteRet)) {
          float fltRet;
          byte byteR = this.R;
          byte byteG = this.G;
          byte byteB = this.B;
          byte byteMin = this.Min;
          byte byteMax = this.Max;
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
          if (fltRet < 0)
            fltRet += 360;
          fltRet *= (255f / 360f);
          _hashCache_Hue[dwordC] = (byte)fltRet;
        }
        return (byteRet);
      }
    }

    public System.Drawing.Color Color {
      get {
        return (System.Drawing.Color.FromArgb(this.R, this.G, this.B));
      }
      set {
        this.SetRGB(value.R, value.G, value.B);
      }
    }
    public void SetRGB(byte byteR, byte byteG, byte byteB) {
      this._dwordPixel = (UInt32)byteR << 16 | (UInt32)byteG << 8 | (UInt32)byteB;
    }
    public byte R {
      get {
        return ((byte)(this._dwordPixel >> 16));
      }
      set {
        this.SetRGB(value, this.G, this.B);
      }
    }
    public byte G {
      get {
        return ((byte)(this._dwordPixel >> 8));
      }
      set {
        this.SetRGB(this.R, value, this.B);
      }
    }
    public byte B {
      get {
        return ((byte)this._dwordPixel);
      }
      set {
        this.SetRGB(this.R, this.G, value);
      }
    }
    #endregion
    #region ctor
    public sPixel(sPixel stPixel) {
      this._dwordPixel = stPixel._dwordPixel;
    }
    public sPixel(System.Drawing.Color objColor) {
      this._dwordPixel = 0;
      this.Color = objColor;
    }
    public sPixel(byte byteR, byte byteG, byte byteB) {
      this._dwordPixel = 0;
      this.SetRGB(byteR, byteG, byteB);
    }
    #endregion
    public override string ToString() {
      return ("(" + this._dwordPixel.ToString("X6") + ") Red:" + this.R + ", Green:" + this.G + ", Blue:" + B);
    }
    public override int GetHashCode() {
      return ((int)this._dwordPixel);
    }
    #region operators
    public static sPixel operator +(sPixel stA, sPixel stB) {
      int intR = stA.R + stB.R;
      int intG = stA.G + stB.G;
      int intB = stA.B + stB.B;
      intR = intR > byte.MaxValue ? byte.MaxValue : intR;
      intG = intG > byte.MaxValue ? byte.MaxValue : intG;
      intB = intB > byte.MaxValue ? byte.MaxValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    public static sPixel operator -(sPixel stA, sPixel stB) {
      int intR = stA.R - stB.R;
      int intG = stA.G - stB.G;
      int intB = stA.B - stB.B;
      intR = intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    public static sPixel operator !(sPixel stA) {
      return (255 - stA);
    }
    public static sPixel operator +(sPixel stA, byte byteD) {
      int intR = stA.R + byteD;
      int intG = stA.G + byteD;
      int intB = stA.B + byteD;
      intR = intR > byte.MaxValue ? byte.MaxValue : intR;
      intG = intG > byte.MaxValue ? byte.MaxValue : intG;
      intB = intB > byte.MaxValue ? byte.MaxValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    public static sPixel operator *(sPixel stA, float fltD) {
      int intR = (int)(stA.R * fltD);
      int intG = (int)(stA.G * fltD);
      int intB = (int)(stA.B * fltD);
      intR = intR > byte.MaxValue ? byte.MaxValue : intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG > byte.MaxValue ? byte.MaxValue : intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB > byte.MaxValue ? byte.MaxValue : intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    public static sPixel operator -(byte byteD, sPixel stA) {
      int intR = byteD - stA.R;
      int intG = byteD - stA.G;
      int intB = byteD - stA.B;
      intR = intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    public static sPixel operator -(sPixel stA, byte byteD) {
      int intR = stA.R - byteD;
      int intG = stA.G - byteD;
      int intB = stA.B - byteD;
      intR = intR < byte.MinValue ? byte.MinValue : intR;
      intG = intG < byte.MinValue ? byte.MinValue : intG;
      intB = intB < byte.MinValue ? byte.MinValue : intB;
      return (new sPixel((byte)intR, (byte)intG, (byte)intB));
    }
    public static sPixel operator /(sPixel stA, float fltD) {
      return (stA * (1f / fltD));
    }
    public static sPixel operator +(byte byteD, sPixel stA) {
      return (stA + byteD);
    }
    public static sPixel operator *(float fltD, sPixel stA) {
      return (stA * fltD);
    }
    public static bool operator ==(sPixel stA, sPixel stB) {
      return (stA._dwordPixel == stB._dwordPixel);
    }
    public static bool operator !=(sPixel stA, sPixel stB) {
      return (stA._dwordPixel != stB._dwordPixel);
    }
    public override bool Equals(object objA) {
      return ((objA is sPixel) && (((sPixel)objA)._dwordPixel == this._dwordPixel));
    }
    public bool Equals(sPixel stA) {
      return (stA._dwordPixel == this._dwordPixel);
    }
    private const byte byteYTrigger = 48;
    private const byte byteUTrigger = 7;
    private const byte byteVTrigger = 6;
    public static bool AllowThresholds = true;
    public bool IsLike(sPixel stA) {
      bool boolRet;
      if (AllowThresholds) {
        if (
          (Math.Abs(this.Y - stA.Y) > byteYTrigger) ||
          (Math.Abs(this.U - stA.U) > byteUTrigger) ||
          (Math.Abs(this.V - stA.V) > byteVTrigger)
          ) {
          boolRet = false;
        } else {
          boolRet = true;
        }
      } else {
        boolRet = this == stA;
      }
      return (boolRet);
    }
    public bool IsNotLike(sPixel stA) {
      return (!this.IsLike(stA));
    }
    #endregion

    #region optimized interpolators
    public static sPixel Interpolate(sPixel stA, sPixel stB) {
      return (new sPixel(
        (byte)((stA.R + stB.R) >> 1),
        (byte)((stA.G + stB.G) >> 1),
        (byte)((stA.B + stB.B) >> 1)
      ));
    }
    public static sPixel Interpolate(sPixel stA, sPixel stB, sPixel stC) {
      return (new sPixel(
        /*
        (byte)((stA.R + stB.R + stC.R) / 3),
        (byte)((stA.G + stB.G + stC.G) / 3),
        (byte)((stA.B + stB.B + stC.B) / 3)
        */
        (byte)(((stA.R + stB.R + stC.R) * 86) >> 8),
        (byte)(((stA.G + stB.G + stC.G) * 86) >> 8),
        (byte)(((stA.B + stB.B + stC.B) * 86) >> 8)
      ));
    }
    public static sPixel Interpolate(sPixel stA, sPixel stB, sPixel stC,sPixel stD) {
      return (new sPixel(
        (byte)((stA.R + stB.R + stC.R + stD.R) >> 2),
        (byte)((stA.G + stB.G + stC.G + stD.G) >> 2),
        (byte)((stA.B + stB.B + stC.B + stD.B) >> 2)
      ));
    }
    #endregion
    #region generic interpolators
    public static sPixel Interpolate(sPixel stA, sPixel stB, byte byteA, byte byteB) {
      UInt16 dwordW = (UInt16)(byteA + byteB);
      return (new sPixel(
        (byte)((stA.R * byteA + stB.R * byteB) / dwordW),
        (byte)((stA.G * byteA + stB.G * byteB) / dwordW),
        (byte)((stA.B * byteA + stB.B * byteB) / dwordW)
      ));
    }
    public static sPixel Interpolate(sPixel stA, sPixel stB, sPixel stC, byte byteA, byte byteB, byte byteC) {
      UInt16 dwordW = (UInt16)(byteA + byteB + byteC);
      return (new sPixel(
        (byte)((stA.R * byteA + stB.R * byteB + stC.R * byteC) / dwordW),
        (byte)((stA.G * byteA + stB.G * byteB + stC.G * byteC) / dwordW),
        (byte)((stA.B * byteA + stB.B * byteB + stC.B * byteC) / dwordW)
      ));
    }
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
    public object Clone() {
      return (new sPixel(this));
    }
    #endregion

    #region ISerializable Members
    public sPixel(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) {
      this._dwordPixel = (UInt32)info.GetValue("value", typeof(UInt32));
    }
    public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) {
      info.AddValue("value", this._dwordPixel);
    }
    #endregion
  }
    
}
