using nImager;

namespace nImager.Filters {
  static class libSNES9x {
    // TODO: implement EPXC
    
    // SNES9x's EPXB modified by Hawkynt to support thresholds
    public static void voidEPXB(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      if (
    stC3.IsNotLike(stC5) &&
    stC1.IsNotLike(stC7) && ( // diagonal
      (
        stC4.IsLike(stC3) ||
        stC4.IsLike(stC7) ||
        stC4.IsLike(stC5) ||
        stC4.IsLike(stC1) || ( // edge smoothing
          (
            stC0.IsNotLike(stC8) ||
            stC4.IsLike(stC6) ||
            stC4.IsLike(stC2)
          ) && (
            stC6.IsNotLike(stC2) ||
            stC4.IsLike(stC0) ||
            stC4.IsLike(stC8)
          )
        )
      )
    )
  ) {
        if (
          stC1.IsLike(stC3) && (
            stC4.IsNotLike(stC0) ||
            stC4.IsNotLike(stC8) ||
            stC1.IsNotLike(stC2) ||
            stC3.IsNotLike(stC6)
          )
        ) {
          stE00 = sPixel.Interpolate(stC1, stC3);
        }
        if (
          stC5.IsLike(stC1) && (
            stC4.IsNotLike(stC2) ||
            stC4.IsNotLike(stC6) ||
            stC5.IsNotLike(stC8) ||
            stC1.IsNotLike(stC0)
          )
        ) {
          stE01 = sPixel.Interpolate(stC5, stC1);
        }
        if (
          stC3.IsLike(stC7) && (
            stC4.IsNotLike(stC6) ||
            stC4.IsNotLike(stC2) ||
            stC3.IsNotLike(stC0) ||
            stC7.IsNotLike(stC8)
          )
        ) {
          stE10 = sPixel.Interpolate(stC3, stC7);
        }
        if (
          stC7.IsLike(stC5) && (
            stC4.IsNotLike(stC8) ||
            stC4.IsNotLike(stC0) ||
            stC7.IsNotLike(stC6) ||
            stC5.IsNotLike(stC2)
          )
        ) {
          stE11 = sPixel.Interpolate(stC7, stC5);
        }
      }
  
      objTgt[qwordTgtX + 0, qwordTgtY + 0] = stE00;
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = stE01;
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = stE10;
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = stE11;
    }
    
    // SNES9x's EPX3 modified by Hawkynt to support thresholds
    public static void voidEPX3(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      sPixel stE11 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;

      if (stC3.IsNotLike(stC5) && stC7.IsNotLike(stC1)) {
        bool boolNQ40 = stC4.IsNotLike(stC0);
        bool boolNQ41 = stC4.IsNotLike(stC1);
        bool boolNQ42 = stC4.IsNotLike(stC2);
        bool boolNQ43 = stC4.IsNotLike(stC3);
        bool boolNQ45 = stC4.IsNotLike(stC5);
        bool boolNQ46 = stC4.IsNotLike(stC6);
        bool boolNQ47 = stC4.IsNotLike(stC7);
        bool boolNQ48 = stC4.IsNotLike(stC8);

        bool boolEQ13 = stC1.IsLike(stC3) && (boolNQ40 || boolNQ48 || stC1.IsNotLike(stC2) || stC3.IsNotLike(stC6));
        bool boolEQ37 = stC3.IsLike(stC7) && (boolNQ46 || boolNQ42 || stC3.IsNotLike(stC0) || stC7.IsNotLike(stC8));
        bool boolEQ75 = stC7.IsLike(stC5) && (boolNQ48 || boolNQ40 || stC7.IsNotLike(stC6) || stC5.IsNotLike(stC2));
        bool boolEQ51 = stC5.IsLike(stC1) && (boolNQ42 || boolNQ46 || stC5.IsNotLike(stC8) || stC1.IsNotLike(stC0));
        if (
          (!boolNQ40) ||
          (!boolNQ41) ||
          (!boolNQ42) ||
          (!boolNQ43) ||
          (!boolNQ45) ||
          (!boolNQ46) ||
          (!boolNQ47) ||
          (!boolNQ48)
        ) {
          if (boolEQ13)
            stE00 = sPixel.Interpolate(stC1, stC3);
          if (boolEQ51)
            stE02 = sPixel.Interpolate(stC5, stC1);
          if (boolEQ37)
            stE20 = sPixel.Interpolate(stC3, stC7);
          if (boolEQ75)
            stE22 = sPixel.Interpolate(stC7, stC5);

          if ((boolEQ51 && boolNQ40) && (boolEQ13 && boolNQ42))
            stE01 = sPixel.Interpolate(stC1, stC3, stC5);
          else if (boolEQ51 && boolNQ40)
            stE01 = sPixel.Interpolate(stC1, stC5);
          else if (boolEQ13 && boolNQ42)
            stE01 = sPixel.Interpolate(stC1, stC3);

          if ((boolEQ13 && boolNQ46) && (boolEQ37 && boolNQ40))
            stE10 = sPixel.Interpolate(stC3, stC1, stC7);
          else if (boolEQ13 && boolNQ46)
            stE10 = sPixel.Interpolate(stC3, stC1);
          else if (boolEQ37 && boolNQ40)
            stE10 = sPixel.Interpolate(stC3, stC7);

          if ((boolEQ75 && boolNQ42) && (boolEQ51 && boolNQ48))
            stE12 = sPixel.Interpolate(stC5, stC1, stC7);
          else if (boolEQ75 && boolNQ42)
            stE12 = sPixel.Interpolate(stC5, stC7);
          else if (boolEQ51 && boolNQ48)
            stE12 = sPixel.Interpolate(stC5, stC1);

          if ((boolEQ37 && boolNQ48) && (boolEQ75 && boolNQ46))
            stE21 = sPixel.Interpolate(stC7, stC3, stC5);
          else if (boolEQ75 && boolNQ46)
            stE21 = sPixel.Interpolate(stC7, stC5);
          else if (boolEQ37 && boolNQ48)
            stE21 = sPixel.Interpolate(stC7, stC3);

        } else {
          if (boolEQ13 && boolNQ47 && boolNQ45)
            stE00 = sPixel.Interpolate(stC1, stC3);
          if (boolEQ51 && boolNQ43 && boolNQ47)
            stE02 = sPixel.Interpolate(stC5, stC1);
          if (boolEQ37 && boolNQ45 && boolNQ41)
            stE20 = sPixel.Interpolate(stC3, stC7);
          if (boolEQ75 && boolNQ41 && boolNQ43)
            stE22 = sPixel.Interpolate(stC7, stC5);
        }
      }

      objTgt[qwordTgtX + 0, qwordTgtY + 0] = stE00;
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = stE01;
      objTgt[qwordTgtX + 2, qwordTgtY + 0] = stE02;
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = stE10;
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = stE11;
      objTgt[qwordTgtX + 2, qwordTgtY + 1] = stE12;
      objTgt[qwordTgtX + 0, qwordTgtY + 2] = stE20;
      objTgt[qwordTgtX + 1, qwordTgtY + 2] = stE21;
      objTgt[qwordTgtX + 2, qwordTgtY + 2] = stE22;
    }
    

  } // end class
} // end namespace
