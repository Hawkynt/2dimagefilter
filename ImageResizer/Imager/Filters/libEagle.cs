using nImager;

namespace nImager.Filters {
  static class libEagle {
    // good old Eagle Engine modified by Hawkynt to support thresholds
    public static void voidEagle2x(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      sPixel stC0 = objSrc[intSrcX - 1, intSrcY - 1];
      sPixel stC1 = objSrc[intSrcX, intSrcY - 1];
      sPixel stC2 = objSrc[intSrcX + 1, intSrcY - 1];
      sPixel stC3 = objSrc[intSrcX - 1, intSrcY];
      sPixel stC4 = objSrc[intSrcX, intSrcY];
      sPixel stC5 = objSrc[intSrcX + 1, intSrcY];
      sPixel stC6 = objSrc[intSrcX - 1, intSrcY + 1];
      sPixel stC7 = objSrc[intSrcX, intSrcY + 1];
      sPixel stC8 = objSrc[intSrcX + 1, intSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      if ((stC1.IsLike(stC0)) && (stC1.IsLike(stC3)))
        stE00 = sPixel.Interpolate(stC1, stC0, stC3);

      if ((stC2.IsLike(stC1)) && (stC2.IsLike(stC5)))
        stE01 = sPixel.Interpolate(stC2, stC1, stC5);

      if ((stC6.IsLike(stC3)) && (stC6.IsLike(stC7)))
        stE10 = sPixel.Interpolate(stC6, stC3, stC7);

      if ((stC7.IsLike(stC5)) && (stC7.IsLike(stC8)))
        stE11 = sPixel.Interpolate(stC7, stC5, stC8);

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }

    // AFAIK there is no eagle 3x so I made one (Hawkynt)
    public static void voidEagle3x(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      sPixel stC0 = objSrc[intSrcX - 1, intSrcY - 1];
      sPixel stC1 = objSrc[intSrcX, intSrcY - 1];
      sPixel stC2 = objSrc[intSrcX + 1, intSrcY - 1];
      sPixel stC3 = objSrc[intSrcX - 1, intSrcY];
      sPixel stC4 = objSrc[intSrcX, intSrcY];
      sPixel stC5 = objSrc[intSrcX + 1, intSrcY];
      sPixel stC6 = objSrc[intSrcX - 1, intSrcY + 1];
      sPixel stC7 = objSrc[intSrcX, intSrcY + 1];
      sPixel stC8 = objSrc[intSrcX + 1, intSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE10 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;

      if ((stC0.IsLike(stC1)) && (stC0.IsLike(stC3)))
        stE00 = sPixel.Interpolate(stC0, stC1, stC3);

      if ((stC2.IsLike(stC1)) && (stC2.IsLike(stC5)))
        stE02 = sPixel.Interpolate(stC2, stC1, stC5);

      if ((stC6.IsLike(stC3)) && (stC6.IsLike(stC7)))
        stE20 = sPixel.Interpolate(stC6, stC3, stC7);

      if ((stC8.IsLike(stC5)) && (stC8.IsLike(stC7)))
        stE22 = sPixel.Interpolate(stC8, stC5, stC7);

      if ((stC0.IsLike(stC1)) && (stC0.IsLike(stC3)) && (stC2.IsLike(stC1)) && (stC2.IsLike(stC5)))
        stE01 = sPixel.Interpolate(sPixel.Interpolate(stC0, stC1, stC3), sPixel.Interpolate(stC2, stC1, stC5));

      if ((stC2.IsLike(stC1)) && (stC2.IsLike(stC5)) && (stC8.IsLike(stC5)) && (stC8.IsLike(stC7)))
        stE12 = sPixel.Interpolate(sPixel.Interpolate(stC2, stC1, stC5), sPixel.Interpolate(stC8, stC5, stC7));

      if ((stC6.IsLike(stC7)) && (stC6.IsLike(stC3)) && (stC8.IsLike(stC5)) && (stC8.IsLike(stC7)))
        stE21 = sPixel.Interpolate(sPixel.Interpolate(stC6, stC7, stC3), sPixel.Interpolate(stC8, stC5, stC7));

      if ((stC0.IsLike(stC1)) && (stC0.IsLike(stC3)) && (stC6.IsLike(stC7)) && (stC6.IsLike(stC3)))
        stE10 = sPixel.Interpolate(sPixel.Interpolate(stC0, stC1, stC3), sPixel.Interpolate(stC6, stC3, stC7));

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 2, intTgtY + 0] = stE02;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stC4;
      objTgt[intTgtX + 2, intTgtY + 1] = stE12;
      objTgt[intTgtX + 0, intTgtY + 2] = stE20;
      objTgt[intTgtX + 1, intTgtY + 2] = stE21;
      objTgt[intTgtX + 2, intTgtY + 2] = stE22;
    }

    // another one that takes into account that normal eagle means that 3 surroundings should be equal
    // looks ugly sometimes depends heavily on source image
    public static void voidEagle3xB(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      sPixel stC0 = objSrc[intSrcX - 1, intSrcY - 1];
      sPixel stC1 = objSrc[intSrcX, intSrcY - 1];
      sPixel stC2 = objSrc[intSrcX + 1, intSrcY - 1];
      sPixel stC3 = objSrc[intSrcX - 1, intSrcY];
      sPixel stC4 = objSrc[intSrcX, intSrcY];
      sPixel stC5 = objSrc[intSrcX + 1, intSrcY];
      sPixel stC6 = objSrc[intSrcX - 1, intSrcY + 1];
      sPixel stC7 = objSrc[intSrcX, intSrcY + 1];
      sPixel stC8 = objSrc[intSrcX + 1, intSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE10 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;

      if ((stC0.IsLike(stC1)) && (stC0.IsLike(stC3)))
        stE00 = sPixel.Interpolate(stC0, stC1, stC3);

      if ((stC2.IsLike(stC1)) && (stC2.IsLike(stC5)))
        stE02 = sPixel.Interpolate(stC2, stC1, stC5);

      if ((stC6.IsLike(stC3)) && (stC6.IsLike(stC7)))
        stE20 = sPixel.Interpolate(stC6, stC3, stC7);

      if ((stC8.IsLike(stC5)) && (stC8.IsLike(stC7)))
        stE22 = sPixel.Interpolate(stC8, stC5, stC7);
      
      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 2, intTgtY + 0] = stE02;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stC4;
      objTgt[intTgtX + 2, intTgtY + 1] = stE12;
      objTgt[intTgtX + 0, intTgtY + 2] = stE20;
      objTgt[intTgtX + 1, intTgtY + 2] = stE21;
      objTgt[intTgtX + 2, intTgtY + 2] = stE22;
    }
  } // end class
} // end namespace
