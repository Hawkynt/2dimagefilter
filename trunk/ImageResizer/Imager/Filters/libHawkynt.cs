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
  static class libHawkynt {
    // just a bad TV effect
    public static void voidTV2X(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      byte byteY = stPixel.Y;
      objTgt[intTgtX + 0, intTgtY + 0] = new sPixel(stPixel.R, 0, 0);
      objTgt[intTgtX + 1, intTgtY + 0] = new sPixel(0, stPixel.G, 0);
      objTgt[intTgtX + 0, intTgtY + 1] = new sPixel(0, 0, stPixel.B);
      objTgt[intTgtX + 1, intTgtY + 1] = new sPixel(byteY, byteY, byteY);
    }
    // another bad one
    public static void voidTV3X(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      byte byteY = stPixel.Y;
      sbyte sbyteAP = (sbyte)(1 - ((intSrcX % 2) << 1));
      objTgt[intTgtX + 0, intTgtY + 0] = new sPixel(stPixel.R, 0, 0);
      objTgt[intTgtX + 1, intTgtY + 0] = new sPixel(0, stPixel.G, 0);
      objTgt[intTgtX + 2, intTgtY + 0] = new sPixel(0, 0, stPixel.B);
      objTgt[intTgtX + 0, intTgtY + 1] = new sPixel(0, 0, 0);
      objTgt[intTgtX + 1, intTgtY + 1] = new sPixel(0, 0, 0);
      objTgt[intTgtX + 2, intTgtY + 1] = new sPixel(0, 0, 0);
      objTgt[intTgtX + 0, intTgtY - sbyteAP] = new sPixel(stPixel.R, 0, 0);
      objTgt[intTgtX + 1, intTgtY + sbyteAP] = new sPixel(0, stPixel.G, 0);
      objTgt[intTgtX + 2, intTgtY - sbyteAP] = new sPixel(0, 0, stPixel.B);
    }

  }
}
