using nImager;

namespace nImager.Filters {
  static class libEagle {
    // good old Eagle Engine modified by Hawkynt to support thresholds
    public static void voidEagle2x(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[qwordSrcX, qwordSrcY];
      sPixel stC0 = objSrc[qwordSrcX - 1, qwordSrcY - 1];
      sPixel stC1 = objSrc[qwordSrcX, qwordSrcY - 1];
      sPixel stC2 = objSrc[qwordSrcX + 1, qwordSrcY - 1];
      sPixel stC3 = objSrc[qwordSrcX - 1, qwordSrcY];
      sPixel stC4 = objSrc[qwordSrcX, qwordSrcY];
      sPixel stC5 = objSrc[qwordSrcX + 1, qwordSrcY];
      sPixel stC6 = objSrc[qwordSrcX - 1, qwordSrcY + 1];
      sPixel stC7 = objSrc[qwordSrcX, qwordSrcY + 1];
      sPixel stC8 = objSrc[qwordSrcX + 1, qwordSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      if ((stC1.IsLike(stC0)) && (stC1.IsLike(stC3)))
        stE00 = sPixel.Interpolate(stC0, stC1, stC3);

      if ((stC1.IsLike(stC2)) && (stC1.IsLike(stC5)))
        stE01 = sPixel.Interpolate(stC1, stC2, stC5);

      if ((stC7.IsLike(stC6)) && (stC7.IsLike(stC3)))
        stE10 = sPixel.Interpolate(stC3, stC6, stC7);

      if ((stC7.IsLike(stC8)) && (stC7.IsLike(stC5)))
        stE11 = sPixel.Interpolate(stC5, stC7, stC8);

      objTgt[qwordTgtX + 0, qwordTgtY + 0] = stE00;
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = stE01;
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = stE10;
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = stE11;
    }

    // AFAIK there is no eagle 3x so I made one (Hawkynt)
    public static void voidEagle3x(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[qwordSrcX, qwordSrcY];
      sPixel stC0 = objSrc[qwordSrcX - 1, qwordSrcY - 1];
      sPixel stC1 = objSrc[qwordSrcX, qwordSrcY - 1];
      sPixel stC2 = objSrc[qwordSrcX + 1, qwordSrcY - 1];
      sPixel stC3 = objSrc[qwordSrcX - 1, qwordSrcY];
      sPixel stC4 = objSrc[qwordSrcX, qwordSrcY];
      sPixel stC5 = objSrc[qwordSrcX + 1, qwordSrcY];
      sPixel stC6 = objSrc[qwordSrcX - 1, qwordSrcY + 1];
      sPixel stC7 = objSrc[qwordSrcX, qwordSrcY + 1];
      sPixel stC8 = objSrc[qwordSrcX + 1, qwordSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE10 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;

      if ((stC1.IsLike(stC0)) && (stC1.IsLike(stC3)))
        stE00 = sPixel.Interpolate(stC1, stC0, stC3);

      if ((stC1.IsLike(stC2)) && (stC1.IsLike(stC5)))
        stE02 = sPixel.Interpolate(stC1, stC2, stC5);

      if ((stC7.IsLike(stC6)) && (stC7.IsLike(stC3)))
        stE20 = sPixel.Interpolate(stC7, stC6, stC3);

      if ((stC7.IsLike(stC8)) && (stC7.IsLike(stC5)))
        stE22 = sPixel.Interpolate(stC7, stC8, stC5);
      
      if ((stC1.IsLike(stC0)) && (stC1.IsLike(stC3)) && (stC1.IsLike(stC2)) && (stC1.IsLike(stC5)))
        stE01 = sPixel.Interpolate(sPixel.Interpolate(stC1, stC0, stC3), sPixel.Interpolate(stC1, stC2, stC5));

      if ((stC1.IsLike(stC2)) && (stC1.IsLike(stC5)) && (stC7.IsLike(stC8)) && (stC7.IsLike(stC5)))
        stE12 = sPixel.Interpolate(sPixel.Interpolate(stC1, stC2, stC5), sPixel.Interpolate(stC7, stC8, stC5));

      if ((stC7.IsLike(stC6)) && (stC7.IsLike(stC3)) && (stC7.IsLike(stC8)) && (stC7.IsLike(stC5)))
        stE21 = sPixel.Interpolate(sPixel.Interpolate(stC7, stC6, stC3), sPixel.Interpolate(stC7, stC8, stC5));

      if ((stC1.IsLike(stC0)) && (stC1.IsLike(stC3)) && (stC7.IsLike(stC6)) && (stC7.IsLike(stC3)))
        stE10 = sPixel.Interpolate(sPixel.Interpolate(stC1, stC0, stC3), sPixel.Interpolate(stC7, stC6, stC3));

      objTgt[qwordTgtX + 0, qwordTgtY + 0] = stE00;
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = stE01;
      objTgt[qwordTgtX + 2, qwordTgtY + 0] = stE02;
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = stE10;
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = stC4;
      objTgt[qwordTgtX + 2, qwordTgtY + 1] = stE12;
      objTgt[qwordTgtX + 0, qwordTgtY + 2] = stE20;
      objTgt[qwordTgtX + 1, qwordTgtY + 2] = stE21;
      objTgt[qwordTgtX + 2, qwordTgtY + 2] = stE22;
    }
  }


}
