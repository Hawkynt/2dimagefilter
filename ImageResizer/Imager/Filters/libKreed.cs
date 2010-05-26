using nImager;
// TODO: this module is completely unable to work with thresholds I'll fix it later (Hawkynt)
namespace nImager.Filters {
  static class libKreed {
    // used for 2xSaI, Super Eagle, Super 2xSaI
    private static int _intConc2d(sPixel stColA, sPixel stColB, sPixel stColC, sPixel stColD) {
      int intRet=0;
      
      bool boolAC = stColA == stColC;
      int intX = boolAC ? 1 : 0;
      int intY = ((stColB == stColC) && !(boolAC)) ? 1 : 0;
      
      bool boolAD = stColA == stColD;
      intX += boolAD ? 1 : 0;
      intY += ((stColB == stColD) && !(boolAD)) ? 1 : 0;

      if (intX <= 1)
        intRet++;
      if (intY <= 1)
        intRet--;
      
      return (intRet);
    }

    // Kreed's SuperEagle
    public static void voidSuperEagle(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      sPixel stD0 = objSrc[qwordSrcX - 1, qwordSrcY + 2];
      sPixel stD1 = objSrc[qwordSrcX, qwordSrcY + 2];
      sPixel stD2 = objSrc[qwordSrcX + 1, qwordSrcY + 2];
      sPixel stD3 = objSrc[qwordSrcX + 2, qwordSrcY - 1];
      sPixel stD4 = objSrc[qwordSrcX + 2, qwordSrcY];
      sPixel stD5 = objSrc[qwordSrcX + 2, qwordSrcY + 1];

      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      if ((stC4 != stC8)) {
        if ((stC7 == stC5)) {
          stE01 = stC7;
          stE10 = stC7;
          if ((stC6 == stC7) || (stC5 == stC2)) {
            stE00 = sPixel.Interpolate(stC7, stC4, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC5);
          }

          if ((stC5 == stD4) || (stC7 == stD1)) {
            stE11 = sPixel.Interpolate(stC7, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC7, stC8);
          }
        } else {
          stE11 = sPixel.Interpolate(stC8, stC7, stC5, 6, 1, 1);
          stE00 = sPixel.Interpolate(stC4, stC7, stC5, 6, 1, 1);
          stE10 = sPixel.Interpolate(stC7, stC4, stC8, 6, 1, 1);
          stE01 = sPixel.Interpolate(stC5, stC4, stC8, 6, 1, 1);
        }
      } else {
        if ((stC7 != stC5)) {
          if ((stC1 == stC4) || (stC8 == stD5)) {
            stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC5);
          }

          if ((stC8 == stD2) || (stC3 == stC4)) {
            stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC7, stC8);
          }
        } else {
          int intR = 0;
          intR += _intConc2d(stC5, stC4, stC6, stD1);
          intR += _intConc2d(stC5, stC4, stC3, stC1);
          intR += _intConc2d(stC5, stC4, stD2, stD5);
          intR += _intConc2d(stC5, stC4, stC2, stD4);

          if (intR > 0) {
            stE10 = stC7;
            stE01 = stC7;
            stE11 = sPixel.Interpolate(stC4, stC5);
            stE00 = sPixel.Interpolate(stC4, stC5);
          } else if (intR < 0) {
            stE10 = sPixel.Interpolate(stC4, stC5);
            stE01 = sPixel.Interpolate(stC4, stC5);
          } else {
            stE10 = stC7;
            stE01 = stC7;
          }
        }
      }

      objTgt[qwordTgtX + 0, qwordTgtY + 0] = stE00;
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = stE01;
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = stE10;
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = stE11;
    }

    // Derek Liauw Kie Fa's 2XSaI
    public static void voidSaI2X(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      sPixel stD0 = objSrc[qwordSrcX - 1, qwordSrcY + 2];
      sPixel stD1 = objSrc[qwordSrcX, qwordSrcY + 2];
      sPixel stD2 = objSrc[qwordSrcX + 1, qwordSrcY + 2];
      sPixel stD3 = objSrc[qwordSrcX + 2, qwordSrcY - 1];
      sPixel stD4 = objSrc[qwordSrcX + 2, qwordSrcY];
      sPixel stD5 = objSrc[qwordSrcX + 2, qwordSrcY + 1];

      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;

      if ((stC4 == stC8) && (stC5 != stC7)) {
        if (((stC4 == stC1) && (stC5 == stD5)) ||
          ((stC4 == stC7) && (stC4 == stC2) && (stC5 != stC1) && (stC5 == stD3))) {
          //nothing
        } else {
          stE01 = sPixel.Interpolate(stC4, stC5);
        }

        if (((stC4 == stC3) && (stC7 == stD2)) ||
          ((stC4 == stC5) && (stC4 == stC6) && (stC3 != stC7) && (stC7 == stD0))) {
          //nothing
        } else {
          stE10 = sPixel.Interpolate(stC4, stC7);
        }
      } else if ((stC5 == stC7) && (stC4 != stC8)) {
        if (((stC5 == stC2) && (stC4 == stC6)) ||
          ((stC5 == stC1) && (stC5 == stC8) && (stC4 != stC2) && (stC4 == stC0))) {
          stE01 = stC5;
        } else {
          stE01 = sPixel.Interpolate(stC4, stC5);
        }

        if (((stC7 == stC6) && (stC4 == stC2)) ||
          ((stC7 == stC3) && (stC7 == stC8) && (stC4 != stC6) && (stC4 == stC0))) {
          stE10 = stC7;
        } else {
          stE10 = sPixel.Interpolate(stC4, stC7);
        }
        stE11 = stC5;
      } else if ((stC4 == stC8) && (stC5 == stC7)) {
        if ((stC4 != stC5)) {
          int intR = 0;
          intR += _intConc2d(stC4, stC5, stC3, stC1);
          intR -= _intConc2d(stC5, stC4, stD4, stC2);
          intR -= _intConc2d(stC5, stC4, stC6, stD1);
          intR += _intConc2d(stC4, stC5, stD5, stD2);

          if (intR < 0) {
            stE11 = stC5;
          } else if (intR == 0) {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, stC8);
          }
          stE10 = sPixel.Interpolate(stC4, stC7);
          stE01 = sPixel.Interpolate(stC4, stC5);
        }
      } else {
        stE11 = sPixel.Interpolate(stC4, stC5, stC7, stC8);

        if ((stC4 == stC7) && (stC4 == stC2)
          && (stC5 != stC1) && (stC5 == stD3)) {
          //nothing
        } else if ((stC5 == stC1) && (stC5 == stC8)
          && (stC4 != stC2) && (stC4 == stC0)) {
          stE01 = stC5;
        } else {
          stE01 = sPixel.Interpolate(stC4, stC5);
        }

        if ((stC4 == stC5) && (stC4 == stC6)
          && (stC3 != stC7) && (stC7 == stD0)) {
          //nothing
        } else if ((stC7 == stC3) && (stC7 == stC8)
          && (stC4 != stC6) && (stC4 == stC0)) {
          stE10 = stC7;
        } else {
          stE10 = sPixel.Interpolate(stC4, stC7);
        }
      }

      objTgt[qwordTgtX + 0, qwordTgtY + 0] = stE00;
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = stE01;
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = stE10;
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = stE11;
    }

    // Kreed's SuperSaI
    public static void voidSuperSaI(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      sPixel stD0 = objSrc[qwordSrcX - 1, qwordSrcY + 2];
      sPixel stD1 = objSrc[qwordSrcX, qwordSrcY + 2];
      sPixel stD2 = objSrc[qwordSrcX + 1, qwordSrcY + 2];
      sPixel stD3 = objSrc[qwordSrcX + 2, qwordSrcY - 1];
      sPixel stD4 = objSrc[qwordSrcX + 2, qwordSrcY];
      sPixel stD5 = objSrc[qwordSrcX + 2, qwordSrcY + 1];
      sPixel stD6 = objSrc[qwordSrcX + 2, qwordSrcY + 2];

      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;

      if ((stC7 == stC5) && (stC4 != stC8)) {
        stE11 = stC7;
        stE01 = stC7;
      } else if ((stC4 == stC8) && (stC7 != stC5)) {
        //nothing
      } else if ((stC4 == stC8) && (stC7 == stC5)) {
        int intR = 0;
        intR += _intConc2d(stC5, stC4, stC6, stD1);
        intR += _intConc2d(stC5, stC4, stC3, stC1);
        intR += _intConc2d(stC5, stC4, stD2, stD5);
        intR += _intConc2d(stC5, stC4, stC2, stD4);

        if (intR > 0) {
          stE11 = stC5;
          stE01 = stC5;
        } else if (intR == 0) {
          stE11 = sPixel.Interpolate(stC4, stC5);
          stE01 = sPixel.Interpolate(stC4, stC5);
        }
      } else {
        if ((stC5 == stC8) && (stC8 == stD1) && (stC7 != stD2) && (stC8 != stD0)) {
          stE11 = sPixel.Interpolate(stC8, stC7, 3, 1);
        } else if ((stC4 == stC7) && (stC7 == stD2) && (stD1 != stC8) && (stC7 != stD6)) {
          stE11 = sPixel.Interpolate(stC7, stC8, 3, 1);
        } else {
          stE11 = sPixel.Interpolate(stC7, stC8);
        }
        if ((stC5 == stC8) && (stC5 == stC1) && (stC4 != stC2) && (stC5 != stC0)) {
          stE01 = sPixel.Interpolate(stC5, stC4, 3, 1);
        } else if ((stC4 == stC7) && (stC4 == stC2) && (stC1 != stC5) && (stC4 != stD3)) {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
        } else {
          stE01 = sPixel.Interpolate(stC4, stC5);
        }
      }
      if ((stC4 == stC8) && (stC7 != stC5) && (stC3 == stC4) && (stC4 != stD2)) {
        stE10 = sPixel.Interpolate(stC7, stC4);
      } else if ((stC4 == stC6) && (stC5 == stC4) && (stC3 != stC7) && (stC4 != stD0)) {
        stE10 = sPixel.Interpolate(stC7, stC4);
      } else {
        stE10 = stC7;
      }
      if ((stC7 == stC5) && (stC4 != stC8) && (stC6 == stC7) && (stC7 != stC2)) {
        stE00 = sPixel.Interpolate(stC7, stC4);
      } else if ((stC3 == stC7) && (stC8 == stC7) && (stC6 != stC4) && (stC7 != stC0)) {
        stE00 = sPixel.Interpolate(stC7, stC4);
      }

      objTgt[qwordTgtX + 0, qwordTgtY + 0] = stE00;
      objTgt[qwordTgtX + 1, qwordTgtY + 0] = stE01;
      objTgt[qwordTgtX + 0, qwordTgtY + 1] = stE10;
      objTgt[qwordTgtX + 1, qwordTgtY + 1] = stE11;
    }


  } // end class
} // end namespace
