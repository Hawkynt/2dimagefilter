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
using nImager;

namespace nImager.Filters {
  static class libBasic{
    public static void voidHScanlines(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      objTgt[intTgtX, intTgtY] = stPixel;
      float fltFactor = (float)objParam / 100f + 1f;
      objTgt[intTgtX, intTgtY + 1] = stPixel * fltFactor;
    }
    public static void voidVScanlines(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      objTgt[intTgtX, intTgtY] = stPixel;
      float fltFactor = (float)objParam / 100f + 1f;
      objTgt[intTgtX + 1, intTgtY] = stPixel * fltFactor;
    }
    
  }
}
