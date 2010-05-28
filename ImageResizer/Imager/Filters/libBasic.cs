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
