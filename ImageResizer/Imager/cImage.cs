#region (c)2010 Hawkynt
/*
 * cImage 
 * Image filtering library by Hawkynt
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
using System.Linq;
using nImager.Filters;
using System.Threading.Tasks;

namespace nImager {
  public class cImage:ICloneable {
    #region helper structs
    public struct sFilter {
      public byte ScaleX;
      public byte ScaleY;
      public string Name;
      public object Parameter;
      public Action<cImage, int, int, cImage, int, int, byte, byte, object> FilterFunction;
      public Func<cImage, cImage> CreationFunction;
      public sFilter(string strName, Func<cImage, cImage> ptrFunc):this(strName,1,1,null) {
        this.CreationFunction = ptrFunc;
      }
      public sFilter(string strName, byte byteScaleX, byte byteScaleY, Action<cImage, int, int, cImage, int, int, byte, byte, object> ptrFilter) :
        this(strName, byteScaleX, byteScaleY, ptrFilter, null) {
      }
      public sFilter(string strName, byte byteScaleX, byte byteScaleY, Action<cImage, int, int, cImage, int, int, byte, byte, object> ptrFilter, object objParam) {
        this.Name = strName;
        this.Parameter = objParam;
        this.ScaleX = byteScaleX;
        this.ScaleY = byteScaleY;
        this.FilterFunction = ptrFilter;
        this.CreationFunction=objSource=>new cImage(objSource.Width*byteScaleX,objSource.Height*byteScaleY);
      }
    }
    #endregion

    #region class fields
    // available filters
    private static sFilter[] _arrFilters = new sFilter[]{
      /*
      new sFilter("-50% Scanlines",1,2,libBasic.voidHScanlines,-50f),
      new sFilter("+50% Scanlines",1,2,libBasic.voidHScanlines,50f),
      new sFilter("+100% Scanlines",1,2,libBasic.voidHScanlines,100f),
      new sFilter("-50% VScanlines",2,1,libBasic.voidVScanlines,-50f),
      new sFilter("+50% VScanlines",2,1,libBasic.voidVScanlines,50f),
      new sFilter("+100% VScanlines",2,1,libBasic.voidVScanlines,100f),
      new sFilter("MAME TV 2x",2,2,libMAME.voidTV2X),
      new sFilter("MAME TV 3x",3,3,libMAME.voidTV3X),
      new sFilter("MAME RGB 2x",2,2,libMAME.voidRGB2X),
      new sFilter("MAME RGB 3x",3,3,libMAME.voidRGB3X),
      new sFilter("Hawkynt TV 2x",2,2,libHawkynt.voidTV2X),
      new sFilter("Hawkynt TV 3x",3,3,libHawkynt.voidTV3X),
      */
      new sFilter("Bilinear Plus Original",2,2,libVBA.voidBilinearPlusOriginal),
      new sFilter("Bilinear Plus",2,2,libVBA.voidBilinearPlus),
      new sFilter("Eagle 2x",2,2,libEagle.voidEagle2x),
      new sFilter("Eagle 3x",3,3,libEagle.voidEagle3x),
      new sFilter("Eagle 3xB",3,3,libEagle.voidEagle3xB),
      new sFilter("Super Eagle",2,2,libKreed.voidSuperEagle),
      new sFilter("SaI 2x",2,2,libKreed.voidSaI2X),
      new sFilter("Super SaI",2,2,libKreed.voidSuperSaI),
      new sFilter("AdvInterp 2x",2,2,libMAME.voidAdvInterp2x),
      new sFilter("AdvInterp 3x",3,3,libMAME.voidAdvInterp3x),
      new sFilter("Scale 2x",2,2,libMAME.voidScale2x),
      new sFilter("Scale 3x",3,3,libMAME.voidScale3x),
      new sFilter("EPXB",2,2,libSNES9x.voidEPXB),
      new sFilter("EPXC",2,2,libSNES9x.voidEPXC),
      new sFilter("EPX3",3,3,libSNES9x.voidEPX3),
      new sFilter("HQ 2x",2,2,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrHQ2x)),
      new sFilter("HQ 2x3",2,3,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrHQ2x3)),
      new sFilter("HQ 2x4",2,4,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrHQ2x4)),
      new sFilter("HQ 3x",3,3,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrHQ3x)),
      new sFilter("HQ 4x",4,4,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrHQ4x)),
      new sFilter("HQ 2x Bold",2,2,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrHQ2x)),
      new sFilter("HQ 2x3 Bold",2,3,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrHQ2x3)),
      new sFilter("HQ 2x4 Bold",2,4,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrHQ2x4)),
      new sFilter("HQ 3x Bold",3,3,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrHQ3x)),
      new sFilter("HQ 4x Bold",4,4,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrHQ4x)),
      new sFilter("HQ 2x Smart",2,2,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrHQ2x)),
      new sFilter("HQ 2x3 Smart",2,3,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrHQ2x3)),
      new sFilter("HQ 2x4 Smart",2,4,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrHQ2x4)),
      new sFilter("HQ 3x Smart",3,3,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrHQ3x)),
      new sFilter("HQ 4x Smart",4,4,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrHQ4x)),
      new sFilter("LQ 2x",2,2,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrLQ2x)),
      new sFilter("LQ 2x3",2,3,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrLQ2x3)),
      new sFilter("LQ 2x4",2,4,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrLQ2x4)),
      new sFilter("LQ 3x",3,3,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrLQ3x)),
      new sFilter("LQ 4x",4,4,libHQ.voidComplex_nQwXh,new libHQ.delHQFilter(libHQ._arrLQ4x)),
      new sFilter("LQ 2x Bold",2,2,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrLQ2x)),
      new sFilter("LQ 2x3 Bold",2,3,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrLQ2x3)),
      new sFilter("LQ 2x4 Bold",2,4,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrLQ2x4)),
      new sFilter("LQ 3x Bold",3,3,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrLQ3x)),
      new sFilter("LQ 4x Bold",4,4,libHQ.voidComplex_nQwXhBold,new libHQ.delHQFilter(libHQ._arrLQ4x)),
      new sFilter("LQ 2x Smart",2,2,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrLQ2x)),
      new sFilter("LQ 2x3 Smart",2,3,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrLQ2x3)),
      new sFilter("LQ 2x4 Smart",2,4,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrLQ2x4)),
      new sFilter("LQ 3x Smart",3,3,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrLQ3x)),
      new sFilter("LQ 4x Smart",4,4,libHQ.voidComplex_nQwXhSmart,new libHQ.delHQFilter(libHQ._arrLQ4x)),
      
      new sFilter("Red",objS=>objS.R),
      new sFilter("Green",objS=>objS.G),
      new sFilter("Blue",objS=>objS.B),
      new sFilter("Y",objS=>objS.Y),
      new sFilter("U",objS=>objS.U),
      new sFilter("V",objS=>objS.V),
      new sFilter("u",objS=>objS.u),
      new sFilter("v",objS=>objS.v),
      new sFilter("Hue",objS=>objS.Hue),
      new sFilter("Brightness",objS=>objS.Brightness),
      new sFilter("Min",objS=>objS.Min),
      new sFilter("Max",objS=>objS.Max)
    };
    #endregion

    // image data
    private sPixel[] _arrImageData = null;
    private int _intWidth = 0;
    private int _intHeight = 0;

    #region properties
    public static sFilter[] Filters {
      get {
        return (cImage._arrFilters.ToArray());
      }
    }
    // get width
    public int Width {
      get {
        return (this._intWidth);
      }
    }
    // get height
    public int Height {
      get {
        return (this._intHeight);
      }
    }
    // get red component
    public cImage R {
      get {
        return (new cImage(this, stA => stA.R));
      }
    }
    // get green component
    public cImage G {
      get {
        return (new cImage(this, stA => stA.G));
      }
    }
    // get blue component
    public cImage B {
      get {
        return (new cImage(this, stA => stA.B));
      }
    }
    // get Y component
    public cImage Y {
      get {
        return (new cImage(this, stA => stA.Y));
      }
    }
    // get U component
    public cImage U {
      get {
        return (new cImage(this, stA => stA.U));
      }
    }
    // get V component
    public cImage V {
      get {
        return (new cImage(this, stA => stA.V));
      }
    }
    // get u component
    public cImage u {
      get {
        return (new cImage(this, stA => stA.u));
      }
    }
    // get v component
    public cImage v {
      get {
        return (new cImage(this, stA => stA.v));
      }
    }
    // get Brightness component
    public cImage Brightness {
      get {
        return (new cImage(this, stA => stA.Brightness));
      }
    }
    // get min component from RGB
    public cImage Min {
      get {
        return (new cImage(this, stA => stA.Min));
      }
    }
    // get max component from RGB
    public cImage Max {
      get {
        return (new cImage(this, stA => stA.Max));
      }
    }
    // get hue component
    public cImage Hue {
      get {
        return (new cImage(this, stA => stA.Hue));
      }
    }
    
    
    #endregion
    #region ctor dtor idx
    // NOTE: Bitmap objects does not support parallel read-outs blame Microsoft
    public cImage(System.Drawing.Bitmap objBitmap)
      : this(objBitmap != null ? objBitmap.Width : 0, objBitmap != null ? objBitmap.Height : 0) {

      System.Drawing.Imaging.BitmapData objBitmapData = objBitmap.LockBits(
        new System.Drawing.Rectangle(0, 0, (int)this._intWidth, (int)this._intHeight),
        System.Drawing.Imaging.ImageLockMode.ReadOnly,
        System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );
      int intFillX = objBitmapData.Stride - objBitmapData.Width * 3;
      unsafe {
        byte* ptrOffset = (byte*)objBitmapData.Scan0.ToPointer();
        for (int intY = 0; intY < this._intHeight; intY++) {
          for (int intX = 0; intX < this._intWidth; intX++) {
            this[intX, intY] = new sPixel(*(ptrOffset+2), *(ptrOffset + 1), *(ptrOffset + 0));
            ptrOffset += 3;
          }
          ptrOffset += intFillX;
        }
      }
      objBitmap.UnlockBits(objBitmapData);
      /*
      for (int intY = 0; intY < objBitmap.Height; intY++)
        for (int intX = 0; intX < objBitmap.Width; intX++)
          this[(ulong)intX, (ulong)intY] = new sPixel(objBitmap.GetPixel(intX, intY));
      */
    }
    // normal ctor
    public cImage(int intWidth, int intHeight) {
      this._intWidth = intWidth;
      this._intHeight = intHeight;
      this._arrImageData = new sPixel[intWidth * intHeight];
    }
    // copy ctor
    public cImage(cImage objSource):this(objSource._intWidth,objSource._intHeight) {
      for (long lngI = 0; lngI < objSource._arrImageData.LongLength; lngI++)
        this._arrImageData[lngI] = objSource._arrImageData[lngI];
    }
    // filter ctor
    public cImage(cImage objSource, Func<sPixel, sPixel> ptrFilter) {
      this._intWidth = objSource._intWidth;
      this._intHeight = objSource._intHeight;
      this._arrImageData = new sPixel[objSource._arrImageData.LongLength];
      Parallel.For(0, this._intHeight, intY => {
        for (int intX = 0; intX < this._intWidth; intX++)
          this[intX, intY] = ptrFilter(objSource[intX, intY]);
      });
    }
    // filter greyscale ctor
    public cImage(cImage objSource, Func<sPixel, byte> ptrFilter) {
      this._intWidth = objSource._intWidth;
      this._intHeight = objSource._intHeight;
      this._arrImageData = new sPixel[objSource._arrImageData.LongLength];
      Parallel.For(0, this._intHeight, intY => {
        for (int intX = 0; intX < this._intWidth; intX++) {
          byte byteD = ptrFilter(objSource[intX, intY]);
          this[intX, intY] = new sPixel(byteD,byteD,byteD);
        }
      });
    }
    // idx
    public sPixel this[int intX, int intY] {
      get {
        if (intX < 0)
          intX = 0;
        if (intY < 0)
          intY = 0;
        if (intX >= this._intWidth)
          intX = this._intWidth - 1;
        if (intY >= this._intHeight)
          intY = this._intHeight - 1;
        
        return (this._arrImageData[intY * this._intWidth + intX]);
      }
      set {
        if(intX<this._intWidth && intY<this._intHeight && intX>=0 && intY>=0)
          this._arrImageData[intY * this._intWidth + intX] = value;
      }
    }
    ~cImage() {
      this._arrImageData = null;
    }
    #endregion
    #region generic image filter
    private cImage _objFilterImage(sFilter stFilter) {
      cImage objRet = stFilter.CreationFunction(this);
      if (stFilter.FilterFunction != null) {
        Parallel.For(0, this._intHeight, intSrcY => {
          for (int intSrcX = 0; intSrcX < this._intWidth; intSrcX++) {
            stFilter.FilterFunction(this, intSrcX, intSrcY, objRet, intSrcX * stFilter.ScaleX, intSrcY * stFilter.ScaleY, stFilter.ScaleX, stFilter.ScaleY, stFilter.Parameter);
          };
        });
      }
      return (objRet);
    }
    public cImage FilterImage(string strFilter) {
      cImage objRet = null;
      strFilter = strFilter.ToLower();
      sFilter stFilter = default(sFilter);
      lock (cImage._arrFilters)
        for (int intI = 0; intI < cImage._arrFilters.Length && stFilter.FilterFunction == null; intI++)
          if (cImage._arrFilters[intI].Name.ToLower() == strFilter)
            stFilter = cImage._arrFilters[intI];
      if (stFilter.FilterFunction != null || stFilter.CreationFunction!=null)
        objRet = this._objFilterImage(stFilter);
      return (objRet);
    }
    #endregion
    public System.Drawing.Bitmap ToBitmap() {
      System.Drawing.Bitmap objRet = new System.Drawing.Bitmap(this.Width, this.Height);
      // NOTE: fucking bitmap does not allow parallel writes
      System.Drawing.Imaging.BitmapData objBitmapData = objRet.LockBits(
        new System.Drawing.Rectangle(0, 0, objRet.Width, objRet.Height),
        System.Drawing.Imaging.ImageLockMode.WriteOnly,
        System.Drawing.Imaging.PixelFormat.Format24bppRgb
      );
      int intFillX = objBitmapData.Stride - objBitmapData.Width * 3;
      unsafe {
        byte* ptrOffset = (byte*)objBitmapData.Scan0.ToPointer();
        for (int intY = 0; intY < this._intHeight; intY++) {
          for (int intX = 0; intX < this._intWidth; intX++) {
            *(ptrOffset+0) = this[intX, intY].B;
            *(ptrOffset+1) = this[intX, intY].G;
            *(ptrOffset+2) = this[intX, intY].R;
            ptrOffset += 3;
          }
          ptrOffset += intFillX;
        }
      }
      objRet.UnlockBits(objBitmapData);
      /*
      for (ulong qwordY = 0; qwordY < this._qwordHeight; qwordY++)
        for (ulong qwordX = 0; qwordX < this._qwordWidth; qwordX++)
          objRet.SetPixel((int)qwordX, (int)qwordY, this[qwordX, qwordY].Color);
      */
      return (objRet);
    }
    public void Fill(byte byteR, byte byteG, byte byteB) {
      this.Fill(new sPixel(byteR, byteG, byteB));
    }
    public void Fill(sPixel stPixel) {
      Parallel.For(0, this._arrImageData.LongLength, qwordOffset => this._arrImageData[qwordOffset] = stPixel);
    }

    #region ICloneable Members
    public object Clone() {
      return (new cImage(this));
    }
    #endregion
  }
}
