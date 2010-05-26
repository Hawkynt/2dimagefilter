using nImager;

namespace nImager.Filters {
  static class libHawkynt {
    // just a bad TV effect
    public static void voidTV2X(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[qwordSrcX, qwordSrcY];
      byte byteY = stPixel.Y;
      objTgt[qwordTgtX + 0, qwordTgtY + 0] = new sPixel(stPixel.R, 0, 0);
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = new sPixel(0, stPixel.G, 0);
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = new sPixel(0, 0, stPixel.B);
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = new sPixel(byteY, byteY, byteY);
    }
    // another bad one
    public static void voidTV3X(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[qwordSrcX, qwordSrcY];
      byte byteY = stPixel.Y;
      sbyte sbyteAP = (sbyte)(1 - ((qwordSrcX % 2) << 1));
      objTgt[qwordTgtX + 0, qwordTgtY + 0] = new sPixel(stPixel.R, 0, 0);
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = new sPixel(0, stPixel.G, 0);
      objTgt[qwordTgtX + 2, qwordTgtY + 0] = new sPixel(0, 0, stPixel.B);
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = new sPixel(0, 0, 0);
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = new sPixel(0, 0, 0);
      objTgt[qwordTgtX + 2, qwordTgtY + 1] = new sPixel(0, 0, 0);
      objTgt[qwordTgtX + 0, (ulong)((long)qwordTgtY - sbyteAP)] = new sPixel(stPixel.R, 0, 0);
      objTgt[qwordTgtX + 1, (ulong)((long)qwordTgtY + sbyteAP)] = new sPixel(0, stPixel.G, 0);
      objTgt[qwordTgtX + 2, (ulong)((long)qwordTgtY - sbyteAP)] = new sPixel(0, 0, stPixel.B);
    }

  }
}
