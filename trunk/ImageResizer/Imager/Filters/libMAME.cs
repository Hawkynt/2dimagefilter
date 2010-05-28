using nImager;

namespace nImager.Filters {
  static class libMAME {
    // MAME's TV effect in 2x
    public static void voidTV2X(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      sPixel stSubPixel = stPixel * (5f / 8f);
      objTgt[intTgtX + 0, intTgtY + 0] = stPixel;
      objTgt[intTgtX + 1, intTgtY + 0] = stPixel;
      objTgt[intTgtX + 0, intTgtY + 1] = stSubPixel;
      objTgt[intTgtX + 1, intTgtY + 1] = stSubPixel;
    }

    // MAME's TV effect 3x
    public static void voidTV3X(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      sPixel stSubPixel = stPixel * (5f / 8f);
      sPixel stSubPixel2 = stPixel * (5f / 16f);
      objTgt[intTgtX + 0, intTgtY + 0] = stPixel;
      objTgt[intTgtX + 1, intTgtY + 0] = stPixel;
      objTgt[intTgtX + 2, intTgtY + 0] = stPixel;
      objTgt[intTgtX + 0, intTgtY + 1] = stSubPixel;
      objTgt[intTgtX + 1, intTgtY + 1] = stSubPixel;
      objTgt[intTgtX + 2, intTgtY + 1] = stSubPixel;
      objTgt[intTgtX + 0, intTgtY + 2] = stSubPixel2;
      objTgt[intTgtX + 1, intTgtY + 2] = stSubPixel2;
      objTgt[intTgtX + 2, intTgtY + 2] = stSubPixel2;
    }

    // MAME's RGB 2x
    public static void voidRGB2X(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      objTgt[intTgtX + 0, intTgtY + 0] = new sPixel(stPixel.R, 0, 0);
      objTgt[intTgtX + 1, intTgtY + 0] = new sPixel(0, stPixel.G, 0);
      objTgt[intTgtX + 0, intTgtY + 1] = new sPixel(0, 0, stPixel.B);
      objTgt[intTgtX + 1, intTgtY + 1] = stPixel;
    }

    // MAME's RGB 3x
    public static void voidRGB3X(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stPixel = objSrc[intSrcX, intSrcY];
      objTgt[intTgtX + 0, intTgtY + 0] = stPixel;
      objTgt[intTgtX + 1, intTgtY + 0] = new sPixel(0, stPixel.G, 0);
      objTgt[intTgtX + 2, intTgtY + 0] = new sPixel(0, 0, stPixel.B);
      objTgt[intTgtX + 0, intTgtY + 1] = new sPixel(0, 0, stPixel.B);
      objTgt[intTgtX + 1, intTgtY + 1] = stPixel;
      objTgt[intTgtX + 2, intTgtY + 1] = new sPixel(stPixel.R, 0, 0);
      objTgt[intTgtX + 0, intTgtY + 2] = new sPixel(stPixel.R, 0, 0);
      objTgt[intTgtX + 1, intTgtY + 2] = new sPixel(0, stPixel.G, 0);
      objTgt[intTgtX + 2, intTgtY + 2] = stPixel;
    }

    // MAME's AdvInterp2x, very similar to Scale2x but uses interpolation, modified by Hawkynt to support thresholds
    public static void voidAdvInterp2x(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stC1 = objSrc[intSrcX + 0, intSrcY - 1];
      sPixel stC3 = objSrc[intSrcX - 1, intSrcY + 0];
      sPixel stC4 = objSrc[intSrcX + 0, intSrcY + 0];
      sPixel stC5 = objSrc[intSrcX + 1, intSrcY + 0];
      sPixel stC7 = objSrc[intSrcX + 0, intSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      if (stC1.IsNotLike(stC7) && stC3.IsNotLike(stC5)) {
        if (stC3.IsLike(stC1))
          stE00 = sPixel.Interpolate(sPixel.Interpolate(stC1, stC3), stC4, 5, 3);
        if (stC5.IsLike(stC1))
          stE01 = sPixel.Interpolate(sPixel.Interpolate(stC1, stC5), stC4, 5, 3);
        if (stC3.IsLike(stC7))
          stE10 = sPixel.Interpolate(sPixel.Interpolate(stC7, stC3), stC4, 5, 3);
        if (stC5.IsLike(stC7))
          stE11 = sPixel.Interpolate(sPixel.Interpolate(stC7, stC5), stC4, 5, 3);
      }
      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }

    // MAME's AdvInterp3x, very similar to Scale3x but uses interpolation, modified by Hawkynt to support thresholds
    public static void voidAdvInterp3x(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stC0 = objSrc[intSrcX - 1, intSrcY - 1];
      sPixel stC1 = objSrc[intSrcX + 0, intSrcY - 1];
      sPixel stC2 = objSrc[intSrcX + 1, intSrcY - 1];
      sPixel stC3 = objSrc[intSrcX - 1, intSrcY + 0];
      sPixel stC4 = objSrc[intSrcX + 0, intSrcY + 0];
      sPixel stC5 = objSrc[intSrcX + 1, intSrcY + 0];
      sPixel stC6 = objSrc[intSrcX - 1, intSrcY + 1];
      sPixel stC7 = objSrc[intSrcX + 0, intSrcY + 1];
      sPixel stC8 = objSrc[intSrcX + 1, intSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;
      if (stC1.IsNotLike(stC7) && stC3.IsNotLike(stC5)) {
        if (stC3.IsLike(stC1)) {
          stE00 = sPixel.Interpolate(sPixel.Interpolate( stC3,stC1), stC4, 5, 3);
        };
        if (stC1.IsLike(stC5)) {
          stE02 = sPixel.Interpolate(sPixel.Interpolate( stC5,stC1), stC4, 5, 3);
        };
        if (stC3.IsLike(stC7)) {
          stE20 = sPixel.Interpolate(sPixel.Interpolate( stC3,stC7), stC4, 5, 3);
        };
        if (stC7.IsLike(stC5)) {
          stE22 = sPixel.Interpolate(sPixel.Interpolate( stC7,stC5), stC4, 5, 3);
        };

        if (
          (stC3.IsLike(stC1) && stC4.IsNotLike(stC2)) &&
          (stC5.IsLike(stC1) && stC4.IsNotLike(stC0))
        )
          stE01 = sPixel.Interpolate(stC1, stC3, stC5);
        else if (stC3.IsLike(stC1) && stC4.IsNotLike(stC2))
          stE01 = sPixel.Interpolate(stC3, stC1);
        else if (stC5.IsLike(stC1) && stC4.IsNotLike(stC0))
          stE01 = sPixel.Interpolate(stC5, stC1);

        if (
          (stC3.IsLike(stC1) && stC4.IsNotLike(stC6)) &&
          (stC3.IsLike(stC7) && stC4.IsNotLike(stC0))
        )
          stE10 = sPixel.Interpolate(stC3, stC1, stC7);
        else if (stC3.IsLike(stC1) && stC4.IsNotLike(stC6))
          stE10 = sPixel.Interpolate(stC3, stC1);
        else if (stC3.IsLike(stC7) && stC4.IsNotLike(stC0))
          stE10 = sPixel.Interpolate(stC3, stC7);

        if (
          (stC5.IsLike(stC1) && stC4.IsNotLike(stC8)) &&
          (stC5.IsLike(stC7) && stC4.IsNotLike(stC2))
        )
          stE12 = sPixel.Interpolate(stC5, stC1, stC7);
        else if (stC5.IsLike(stC1) && stC4.IsNotLike(stC8))
          stE12 = sPixel.Interpolate(stC5, stC1);
        else if (stC5.IsLike(stC7) && stC4.IsNotLike(stC2))
          stE12 = sPixel.Interpolate(stC5, stC7);

        if (
          (stC3.IsLike(stC7) && stC4.IsNotLike(stC8)) &&
          (stC5.IsLike(stC7) && stC4.IsNotLike(stC6))
        )
          stE21 = sPixel.Interpolate(stC7, stC3, stC5);
        else if (stC3.IsLike(stC7) && stC4.IsNotLike(stC8))
          stE21 = sPixel.Interpolate(stC3, stC7);
        else if (stC5.IsLike(stC7) && stC4.IsNotLike(stC6))
          stE21 = sPixel.Interpolate(stC5, stC7);
      }
      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 2, intTgtY + 0] = stE02;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
      objTgt[intTgtX + 2, intTgtY + 1] = stE12;
      objTgt[intTgtX + 0, intTgtY + 2] = stE20;
      objTgt[intTgtX + 1, intTgtY + 2] = stE21;
      objTgt[intTgtX + 2, intTgtY + 2] = stE22;
    }

    // Andrea Mazzoleni's Scale2X modified by Hawkynt to support thresholds
    public static void voidScale2x(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stC0 = objSrc[intSrcX - 1, intSrcY - 1];
      sPixel stC1 = objSrc[intSrcX + 0, intSrcY - 1];
      sPixel stC2 = objSrc[intSrcX + 1, intSrcY - 1];
      sPixel stC3 = objSrc[intSrcX - 1, intSrcY + 0];
      sPixel stC4 = objSrc[intSrcX + 0, intSrcY + 0];
      sPixel stC5 = objSrc[intSrcX + 1, intSrcY + 0];
      sPixel stC6 = objSrc[intSrcX - 1, intSrcY + 1];
      sPixel stC7 = objSrc[intSrcX + 0, intSrcY + 1];
      sPixel stC8 = objSrc[intSrcX + 1, intSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      if (stC3.IsNotLike(stC5) && stC1.IsNotLike(stC7)) {
        if (stC1.IsLike(stC3)) {
          stE00 = sPixel.Interpolate(stC1, stC3);
        };
        if (stC1.IsLike(stC5)) {
          stE01 = sPixel.Interpolate(stC1, stC5);
        };
        if (stC7.IsLike(stC3)) {
          stE10 = sPixel.Interpolate(stC7, stC3);
        };
        if (stC7.IsLike(stC5)) {
          stE11 = sPixel.Interpolate(stC7, stC5);
        };
      }
      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }
    
    // Andrea Mazzoleni's Scale3X modified by Hawkynt to support thresholds
    public static void voidScale3x(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stC0 = objSrc[intSrcX - 1, intSrcY - 1];
      sPixel stC1 = objSrc[intSrcX + 0, intSrcY - 1];
      sPixel stC2 = objSrc[intSrcX + 1, intSrcY - 1];
      sPixel stC3 = objSrc[intSrcX - 1, intSrcY + 0];
      sPixel stC4 = objSrc[intSrcX + 0, intSrcY + 0];
      sPixel stC5 = objSrc[intSrcX + 1, intSrcY + 0];
      sPixel stC6 = objSrc[intSrcX - 1, intSrcY + 1];
      sPixel stC7 = objSrc[intSrcX + 0, intSrcY + 1];
      sPixel stC8 = objSrc[intSrcX + 1, intSrcY + 1];
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;
      if (stC1.IsNotLike(stC7) && stC3.IsNotLike(stC5)) {
        if (stC3.IsLike(stC1))
          stE00 = sPixel.Interpolate(stC3, stC1);
        if (stC1.IsLike(stC5))
          stE02 = sPixel.Interpolate(stC1, stC5);
        if (stC3.IsLike(stC7))
          stE20 = sPixel.Interpolate(stC3, stC7);
        if (stC7.IsLike(stC5))
          stE22 = sPixel.Interpolate(stC7, stC5);

        if (
          (stC3.IsLike(stC1) && stC4.IsNotLike(stC2)) &&
          (stC5.IsLike(stC1) && stC4.IsNotLike(stC0))
        )
          stE01 = sPixel.Interpolate(stC1, stC3, stC5);
        else if (stC3.IsLike(stC1) && stC4.IsNotLike(stC2))
          stE01 = sPixel.Interpolate(stC3, stC1);
        else if (stC5.IsLike(stC1) && stC4.IsNotLike(stC0))
          stE01 = sPixel.Interpolate(stC5, stC1);

        if (
          (stC3.IsLike(stC1) && stC4.IsNotLike(stC6)) &&
          (stC3.IsLike(stC7) && stC4.IsNotLike(stC0))
        )
          stE10 = sPixel.Interpolate(stC3, stC1, stC7);
        else if (stC3.IsLike(stC1) && stC4.IsNotLike(stC6))
          stE10 = sPixel.Interpolate(stC3, stC1);
        else if (stC3.IsLike(stC7) && stC4.IsNotLike(stC0))
          stE10 = sPixel.Interpolate(stC3, stC7);

        if (
          (stC5.IsLike(stC1) && stC4.IsNotLike(stC8)) &&
          (stC5.IsLike(stC7) && stC4.IsNotLike(stC2))
        )
          stE12 = sPixel.Interpolate(stC5, stC1, stC7);
        else if (stC5.IsLike(stC1) && stC4.IsNotLike(stC8))
          stE12 = sPixel.Interpolate(stC5, stC1);
        else if (stC5.IsLike(stC7) && stC4.IsNotLike(stC2))
          stE12 = sPixel.Interpolate(stC5, stC7);

        if (
          (stC3.IsLike(stC7) && stC4.IsNotLike(stC8)) &&
          (stC5.IsLike(stC7) && stC4.IsNotLike(stC6))
        )
          stE21 = sPixel.Interpolate(stC7, stC3, stC5);
        else if (stC3.IsLike(stC7) && stC4.IsNotLike(stC8))
          stE21 = sPixel.Interpolate(stC3, stC7);
        else if (stC5.IsLike(stC7) && stC4.IsNotLike(stC6))
          stE21 = sPixel.Interpolate(stC5, stC7);

      }
      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 2, intTgtY + 0] = stE02;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
      objTgt[intTgtX + 2, intTgtY + 1] = stE12;
      objTgt[intTgtX + 0, intTgtY + 2] = stE20;
      objTgt[intTgtX + 1, intTgtY + 2] = stE21;
      objTgt[intTgtX + 2, intTgtY + 2] = stE22;
    }
    

  } // end class
} // end namespace
