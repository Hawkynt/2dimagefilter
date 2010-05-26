using nImager;

namespace nImager.Filters {
  static class libBasic{
    public static void voidHScanlines(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[qwordSrcX, qwordSrcY];
      objTgt[qwordTgtX, qwordTgtY] = stPixel;
      float fltFactor = (float)objParam / 100f + 1f;
      objTgt[qwordTgtX, qwordTgtY + 1] = stPixel * fltFactor;
    }
    public static void voidVScanlines(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[qwordSrcX, qwordSrcY];
      objTgt[qwordTgtX, qwordTgtY] = stPixel;
      float fltFactor = (float)objParam / 100f + 1f;
      objTgt[qwordTgtX + 1, qwordTgtY] = stPixel * fltFactor;
    }
    
  }
}
