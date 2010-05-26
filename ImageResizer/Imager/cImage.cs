#region (c)2010 Hawkynt
/*
 * cImage 
 * Image filtering library by Hawkynt
 * This is a C# port of my former classImage perl library.
 * You can use and modify my code as long as you give me a credit and 
 * inform me about updates, changes new features and modification. 
 * Distribution and selling is allowed. Would be nice if you give some 
 * payback.
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nImager.Filters;
using System.Threading.Tasks;

namespace nImager {
  public class cImage {
    #region helper structs
    public struct sFilter {
      public byte ScaleX;
      public byte ScaleY;
      public string Name;
      public object Parameter;
      public Action<cImage, ulong, ulong, cImage, ulong, ulong, byte, byte, object> FilterFunction;
      public sFilter(string strName, byte byteScaleX, byte byteScaleY, Action<cImage, ulong, ulong, cImage, ulong, ulong, byte, byte, object> ptrFilter) :
        this(strName, byteScaleX, byteScaleY, ptrFilter, null) {
      }
      public sFilter(string strName, byte byteScaleX, byte byteScaleY, Action<cImage, ulong, ulong, cImage, ulong, ulong, byte, byte, object> ptrFilter, object objParam) {
        this.Name = strName;
        this.Parameter = objParam;
        this.ScaleX = byteScaleX;
        this.ScaleY = byteScaleY;
        this.FilterFunction = ptrFilter;
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
      new sFilter("Eagle 2x",2,2,libEagle.voidEagle2x),
      new sFilter("Eagle 3x",3,3,libEagle.voidEagle3x),
      new sFilter("Super Eagle",2,2,libKreed.voidSuperEagle),
      new sFilter("SaI 2x",2,2,libKreed.voidSaI2X),
      new sFilter("Super SaI",2,2,libKreed.voidSuperSaI),
      new sFilter("AdvInterp 2x",2,2,libMAME.voidAdvInterp2x),
      new sFilter("AdvInterp 3x",3,3,libMAME.voidAdvInterp3x),
      new sFilter("Scale 2x",2,2,libMAME.voidScale2x),
      new sFilter("Scale 3x",3,3,libMAME.voidScale3x),
      new sFilter("EPXB",2,2,libSNES9x.voidEPXB),
      new sFilter("EPX3",3,3,libSNES9x.voidEPX3),
      //new sFilter("EPXC 2x",2,2,libMAME.voidEPXC),
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
      
      new sFilter()
    };
    #endregion

    // image data
    private sPixel[] _arrImageData = null;
    private ulong _qwordWidth = 0;
    private ulong _qwordHeight = 0;

    #region properties
    public static sFilter[] Filters {
      get {
        return (cImage._arrFilters.ToArray());
      }
    }
    public ulong Width {
      get {
        return (this._qwordWidth);
      }
    }
    public ulong Height {
      get {
        return (this._qwordHeight);
      }
    }
    #endregion
    #region ctor dtor idx
    // NOTE: Bitmap objects does not support parallel read-outs blame Microsoft
    public cImage(System.Drawing.Bitmap objBitmap)
      : this(objBitmap != null ? (ulong)objBitmap.Width : 0, objBitmap != null ? (ulong)objBitmap.Height : 0) {
      
      for (int intY = 0; intY < objBitmap.Height; intY++)
        for (int intX = 0; intX < objBitmap.Width; intX++)
          this[(ulong)intX, (ulong)intY] = new sPixel(objBitmap.GetPixel(intX, intY));
    }
    public cImage(ulong qwordWidth, ulong qwordHeight) {
      this._qwordWidth = qwordWidth;
      this._qwordHeight = qwordHeight;
      this._arrImageData = new sPixel[qwordWidth * qwordHeight];
    }
    public sPixel this[ulong qwordX, ulong qwordY] {
      get {
        if (qwordX >= this._qwordWidth)
          qwordX = this._qwordWidth - 1;
        if (qwordY >= this._qwordHeight)
          qwordY = this._qwordHeight - 1;
        
        return (this._arrImageData[qwordY * this._qwordWidth + qwordX]);
      }
      set {
        if(qwordX<this._qwordWidth && qwordY<this._qwordHeight)
          this._arrImageData[qwordY * this._qwordWidth + qwordX] = value;
      }
    }
    ~cImage() {
      this._arrImageData = null;
    }
    #endregion
    #region generic image filter
    private cImage _objFilterImage(sFilter stFilter) {
      cImage objRet;
      ulong dwordNewWidth = (ulong)(this._qwordWidth * stFilter.ScaleX);
      ulong dwordNewHeight = (ulong)(this._qwordHeight * stFilter.ScaleY);
      objRet = new cImage(dwordNewWidth, dwordNewHeight);
      Parallel.For(0, (long)this._qwordHeight, qwordSrcY => {
        Parallel.For(0, (long)this._qwordWidth, qwordSrcX => {
          stFilter.FilterFunction(this, (ulong)qwordSrcX, (ulong)qwordSrcY, objRet, (ulong)qwordSrcX * stFilter.ScaleX, (ulong)qwordSrcY * stFilter.ScaleY, stFilter.ScaleX, stFilter.ScaleY, stFilter.Parameter);
        });
      });
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
      if (stFilter.FilterFunction != null)
        objRet = this._objFilterImage(stFilter);
      return (objRet);
    }
    #endregion
    public System.Drawing.Bitmap ToBitmap() {
      System.Drawing.Bitmap objRet = new System.Drawing.Bitmap((int)this.Width, (int)this.Height);
      // NOTE: fucking bitmap does not allow parallel writes
      for (ulong qwordY = 0; qwordY < this._qwordHeight; qwordY++)
        for (ulong qwordX = 0; qwordX < this._qwordWidth; qwordX++)
          objRet.SetPixel((int)qwordX, (int)qwordY, this[qwordX, qwordY].Color);
      return (objRet);
    }
    public void Fill(byte byteR, byte byteG, byte byteB) {
      this.Fill(new sPixel(byteR, byteG, byteB));
    }
    public void Fill(sPixel stPixel) {
      Parallel.For(0, this._arrImageData.LongLength, qwordOffset => this._arrImageData[qwordOffset] = stPixel);
    }
  }
}
