using nImager;

namespace nImager.Filters {
  static class libHQ {
    #region Common
    public delegate sPixel[] delHQFilter(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8);
    // body for HQ2x etc.
    public static void voidComplex_nQwXh(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stC0 = objSrc[qwordSrcX - 1, qwordSrcY - 1];
      sPixel stC1 = objSrc[qwordSrcX, qwordSrcY - 1];
      sPixel stC2 = objSrc[qwordSrcX + 1, qwordSrcY - 1];
      sPixel stC3 = objSrc[qwordSrcX - 1, qwordSrcY];
      sPixel stC4 = objSrc[qwordSrcX, qwordSrcY];
      sPixel stC5 = objSrc[qwordSrcX + 1, qwordSrcY];
      sPixel stC6 = objSrc[qwordSrcX - 1, qwordSrcY + 1];
      sPixel stC7 = objSrc[qwordSrcX, qwordSrcY + 1];
      sPixel stC8 = objSrc[qwordSrcX + 1, qwordSrcY + 1];
      byte bytePattern = 0;
      if ((stC4.IsNotLike(stC0)))
        bytePattern |= 1;
      if ((stC4.IsNotLike(stC1)))
        bytePattern |= 2;
      if ((stC4.IsNotLike(stC2)))
        bytePattern |= 4;
      if ((stC4.IsNotLike(stC3)))
        bytePattern |= 8;
      if ((stC4.IsNotLike(stC5)))
        bytePattern |= 16;
      if ((stC4.IsNotLike(stC6)))
        bytePattern |= 32;
      if ((stC4.IsNotLike(stC7)))
        bytePattern |= 64;
      if ((stC4.IsNotLike(stC8)))
        bytePattern |= 128;
      sPixel[] arrResult = ((delHQFilter)objParam)(bytePattern, stC0, stC1, stC2, stC3, stC4, stC5, stC6, stC7, stC8);
      byte byteI = 0;
      for (byte byteY = 0; byteY < byteScaleY; byteY++) {
        for (byte byteX = 0; byteX < byteScaleX; byteX++) {
          objTgt[qwordTgtX + byteX, qwordTgtY + byteY] = arrResult[byteI++];
        }
      }
    } // end sub

    // body for HQ2xBold etc. as seen in SNES9x
    public static void voidComplex_nQwXhBold(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stC0 = objSrc[qwordSrcX - 1, qwordSrcY - 1];
      sPixel stC1 = objSrc[qwordSrcX, qwordSrcY - 1];
      sPixel stC2 = objSrc[qwordSrcX + 1, qwordSrcY - 1];
      sPixel stC3 = objSrc[qwordSrcX - 1, qwordSrcY];
      sPixel stC4 = objSrc[qwordSrcX, qwordSrcY];
      sPixel stC5 = objSrc[qwordSrcX + 1, qwordSrcY];
      sPixel stC6 = objSrc[qwordSrcX - 1, qwordSrcY + 1];
      sPixel stC7 = objSrc[qwordSrcX, qwordSrcY + 1];
      sPixel stC8 = objSrc[qwordSrcX + 1, qwordSrcY + 1];
      byte byteAvgBrightness = (byte)((
        stC0.Brightness +
        stC1.Brightness +
        stC2.Brightness +
        stC3.Brightness +
        stC4.Brightness +
        stC5.Brightness +
        stC6.Brightness +
        stC7.Brightness +
        stC8.Brightness
        ) / 9);
      bool boolDC4 = stC4.Brightness > byteAvgBrightness;
      byte bytePattern = 0;
      if ((stC4.IsNotLike(stC0)) && ((stC0.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 1;
      if ((stC4.IsNotLike(stC1)) && ((stC1.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 2;
      if ((stC4.IsNotLike(stC2)) && ((stC2.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 4;
      if ((stC4.IsNotLike(stC3)) && ((stC3.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 8;
      if ((stC4.IsNotLike(stC5)) && ((stC5.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 16;
      if ((stC4.IsNotLike(stC6)) && ((stC6.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 32;
      if ((stC4.IsNotLike(stC7)) && ((stC7.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 64;
      if ((stC4.IsNotLike(stC8)) && ((stC8.Brightness > byteAvgBrightness) != boolDC4))
        bytePattern |= 128;
      sPixel[] arrResult = ((delHQFilter)objParam)(bytePattern, stC0, stC1, stC2, stC3, stC4, stC5, stC6, stC7, stC8);
      byte byteI = 0;
      for (byte byteY = 0; byteY < byteScaleY; byteY++) {
        for (byte byteX = 0; byteX < byteScaleX; byteX++) {
          objTgt[qwordTgtX + byteX, qwordTgtY + byteY] = arrResult[byteI++];
        }
      }
    } // end sub

    // body for HQ2xSmart etc. as seen in SNES9x
    public static void voidComplex_nQwXhSmart(cImage objSrc, ulong qwordSrcX, ulong qwordSrcY, cImage objTgt, ulong qwordTgtX, ulong qwordTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stC0 = objSrc[qwordSrcX - 1, qwordSrcY - 1];
      sPixel stC2 = objSrc[qwordSrcX + 1, qwordSrcY - 1];
      sPixel stC4 = objSrc[qwordSrcX, qwordSrcY];
      sPixel stC6 = objSrc[qwordSrcX - 1, qwordSrcY + 1];
      sPixel stC8 = objSrc[qwordSrcX + 1, qwordSrcY + 1];
      if (stC0.IsLike(stC4) || stC2.IsLike(stC4) || stC6.IsLike(stC4) || stC8.IsLike(stC4))
        libHQ.voidComplex_nQwXh(objSrc, qwordSrcX, qwordSrcY, objTgt, qwordTgtX, qwordTgtY, byteScaleX, byteScaleY, objParam);
      else
        libHQ.voidComplex_nQwXhBold(objSrc, qwordSrcX, qwordSrcY, objTgt, qwordTgtX, qwordTgtY, byteScaleX, byteScaleY, objParam);
    } // end sub
    #endregion

    #region filter casepathes

    #region standard HQ2x casepath
    public static sPixel[] _arrHQ2x(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      switch (bytePattern) {
        #region HQ2x PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 10:
        case 138: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 11:
        case 139: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 3, 3, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          }
        }
        break;
        case 15:
        case 143: {
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 3, 3, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 18:
        case 50: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 3, 3, 2);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 22:
        case 54: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 23:
        case 55: {
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 3, 3, 2);
          }
        }
        break;
        case 24: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 25: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 26:
        case 31: {
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 27: {
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 28: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 29: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 30: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 42:
        case 170: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 3, 3, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          }
        }
        break;
        case 43:
        case 171: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 3, 3, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
        }
        break;
        case 56: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 57: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 58: {
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 59: {
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 60: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 61: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
        }
        break;
        case 62: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 63: {
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 66: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 67: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 70: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 71: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 72:
        case 76: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 73:
        case 77: {
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC3, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 74:
        case 107: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 75: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 78: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 79: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 80:
        case 81: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 82:
        case 214: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 83: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 84:
        case 85: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 86: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 87: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 88:
        case 248: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 89: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 90: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 91: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 92: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 93: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 94: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 95: {
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 98: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 99: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 102: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 103: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
        }
        break;
        case 104:
        case 108: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 105:
        case 109: {
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC3, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 106: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 110: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 111: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC8, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
        }
        break;
        case 112:
        case 113: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 114: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 115: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 116:
        case 117: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 118: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 119: {
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 3, 3, 2);
          }
        }
        break;
        case 120: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 121: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 122: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 123: {
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 124: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 125: {
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC3, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 126: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 127: {
          stE11 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 146:
        case 178: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 150:
        case 182: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 152: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 153: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 154: {
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 155: {
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 156: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 157: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 158: {
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 159: {
          stE10 = sPixel.Interpolate(stC4, stC6, stC7, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 184: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 185: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 186: {
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 187: {
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 3, 3, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          }
        }
        break;
        case 188: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 189: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 190: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          }
        }
        break;
        case 191: {
          stE10 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 194: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 195: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 198: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 199: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 200:
        case 204: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC3, stC7, stC4, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
        }
        break;
        case 202: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 203: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 206: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 207: {
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 3, 3, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          }
        }
        break;
        case 208:
        case 209: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 210: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 211: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 212:
        case 213: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 215: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC6, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 216: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 217: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 218: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 219: {
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 220: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 221: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 222: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 223: {
          stE10 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 226: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 227: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 230: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 231: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 232:
        case 236: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC3, stC7, stC4, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
        }
        break;
        case 234: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 235: {
          stE01 = sPixel.Interpolate(stC4, stC2, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 238: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC3, stC7, stC4, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 239: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
        }
        break;
        case 240:
        case 241: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          if (stC7.IsNotLike(stC5)) {
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 242: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 243: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC4, 3, 3, 2);
          }
        }
        break;
        case 244:
        case 245: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC3, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 247: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC2, 2, 1, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 250: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 251: {
          stE01 = sPixel.Interpolate(stC4, stC2, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 252: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
        }
        break;
        case 253: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 255: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, 
      stE10, stE11, 
      });
    }
    #endregion
    #region standard HQ2x3 casepath
    public static sPixel[] _arrHQ2x3(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      switch (bytePattern) {
        #region HQ2x3 PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 10:
        case 138: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 11:
        case 139: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 10, 5, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 7, 6, 3);
            stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          }
        }
        break;
        case 15:
        case 143: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 10, 5, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 7, 6, 3);
            stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 18:
        case 50: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 7, 6, 3);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 10, 5, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 22:
        case 54: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 23:
        case 55: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 7, 6, 3);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 10, 5, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          }
        }
        break;
        case 24: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 25: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 26:
        case 31: {
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 27: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 28: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 29: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 30: {
          stE10 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 42:
        case 170: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 5, 4);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          }
        }
        break;
        case 43:
        case 171: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 5, 4);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 56: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 57: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 58: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 59: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsLike(stC3))) {
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 60: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 61: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
        }
        break;
        case 62: {
          stE10 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 63: {
          stE10 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 66: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 67: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 70: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 71: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 72:
        case 76: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 73:
        case 77: {
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE20 = sPixel.Interpolate(stC7, stC3, stC4, 7, 5, 4);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 74:
        case 107: {
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          }
        }
        break;
        case 75: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 78: {
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 79: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC5, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 80:
        case 81: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 82:
        case 214: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
          }
        }
        break;
        case 83: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 84:
        case 85: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
            stE11 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC7, stC5, stC4, 7, 5, 4);
          }
        }
        break;
        case 86: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 87: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC3, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 88:
        case 248: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 89: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 90: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 91: {
          stE11 = stC4;
          if (stC1.IsLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsLike(stC3))) {
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          }
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 92: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 93: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 94: {
          stE10 = stC4;
          if (stC1.IsLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if ((stC1.IsLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          }
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 95: {
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 98: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 99: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 102: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 103: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
        }
        break;
        case 104:
        case 108: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 105:
        case 109: {
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE20 = sPixel.Interpolate(stC7, stC3, stC4, 7, 5, 4);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 106: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 110: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 111: {
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 112:
        case 113: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
            stE20 = sPixel.Interpolate(stC7, stC4, stC3, 7, 6, 3);
            stE21 = sPixel.Interpolate(stC7, stC5, stC4, 10, 5, 1);
          }
        }
        break;
        case 114: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 115: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 116:
        case 117: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 118: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 119: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 7, 6, 3);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 10, 5, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          }
        }
        break;
        case 120: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 121: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE11 = stC4;
          if (stC7.IsLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsLike(stC3))) {
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
          }
        }
        break;
        case 122: {
          stE11 = stC4;
          if (stC7.IsLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsLike(stC3))) {
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 123: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          }
        }
        break;
        case 124: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 125: {
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE20 = sPixel.Interpolate(stC7, stC3, stC4, 7, 5, 4);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
        }
        break;
        case 126: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 127: {
          if (stC1.IsLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsLike(stC3))) {
            stE01 = sPixel.Interpolate(stC4, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE01 = stC4;
          }
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC8, stC7, 12, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 146:
        case 178: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE11 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 7, 5, 4);
            stE11 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 150:
        case 182: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 7, 5, 4);
            stE11 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 152: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 153: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 154: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 155: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 156: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 157: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 158: {
          stE10 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if ((stC1.IsLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 159: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 184: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 185: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 186: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 187: {
          stE11 = stC4;
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 5, 4);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE20 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          }
        }
        break;
        case 188: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 189: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 190: {
          stE10 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
            stE11 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 7, 5, 4);
            stE11 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          }
        }
        break;
        case 191: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 194: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 195: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 198: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 199: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 200:
        case 204: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE20 = sPixel.Interpolate(stC7, stC3, stC4, 10, 5, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, stC5, 7, 6, 3);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
        }
        break;
        case 202: {
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 203: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 206: {
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 207: {
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 10, 5, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 7, 6, 3);
            stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          }
        }
        break;
        case 208:
        case 209: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 210: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 211: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 212:
        case 213: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
            stE11 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC7, stC5, stC4, 7, 5, 4);
          }
        }
        break;
        case 215: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 216: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 217: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 218: {
          stE10 = stC4;
          if (stC7.IsLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if ((stC7.IsLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 219: {
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 220: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          if (stC7.IsLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if ((stC7.IsLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 221: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
            stE11 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC7, stC5, stC4, 7, 5, 4);
          }
        }
        break;
        case 222: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
          }
        }
        break;
        case 223: {
          if (stC1.IsLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
          }
          if ((stC1.IsLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC4, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = stC4;
          }
          if (stC7.IsNotLike(stC5)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC6, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 226: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 227: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 230: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 231: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
        }
        break;
        case 232:
        case 236: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE20 = sPixel.Interpolate(stC7, stC3, stC4, 10, 5, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, stC5, 7, 6, 3);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
        }
        break;
        case 234: {
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 12, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 235: {
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          }
        }
        break;
        case 238: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE20 = sPixel.Interpolate(stC7, stC3, stC4, 10, 5, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, stC5, 7, 6, 3);
          }
        }
        break;
        case 239: {
          stE01 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 240:
        case 241: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
            stE20 = sPixel.Interpolate(stC7, stC4, stC3, 7, 6, 3);
            stE21 = sPixel.Interpolate(stC7, stC5, stC4, 10, 5, 1);
          }
        }
        break;
        case 242: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 12, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 243: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 13, 3);
            stE20 = sPixel.Interpolate(stC7, stC4, stC3, 7, 6, 3);
            stE21 = sPixel.Interpolate(stC7, stC5, stC4, 10, 5, 1);
          }
        }
        break;
        case 244:
        case 245: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 246: {
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
          }
        }
        break;
        case 247: {
          stE00 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 13, 3);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 249: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          stE10 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 250: {
          stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 251: {
          if (stC7.IsLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsLike(stC3))) {
            stE21 = sPixel.Interpolate(stC4, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE21 = stC4;
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC2, stC1, 12, 3, 1);
          }
        }
        break;
        case 252: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 253: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 254: {
          if (stC7.IsLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
          }
          if ((stC7.IsLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC4, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = stC4;
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
            stE21 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC0, stC1, 12, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
          }
        }
        break;
        case 255: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, 
      stE10, stE11, 
      stE20, stE21, 
      });
    }
    #endregion
    #region standard HQ2x4 casepath
    public static sPixel[] _arrHQ2x4(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE30 = stC4;
      sPixel stE31 = stC4;
      switch (bytePattern) {
        #region HQ2x4 PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 10:
        case 138: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 11:
        case 139: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 9, 7);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, stC1, 8, 5, 3);
          }
        }
        break;
        case 15:
        case 143: {
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 9, 7);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, stC1, 8, 5, 3);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 18:
        case 50: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, 9, 7);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 8, 5, 3);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 22:
        case 54: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 23:
        case 55: {
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, 9, 7);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 8, 5, 3);
          }
        }
        break;
        case 24: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 25: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 26:
        case 31: {
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 27: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 28: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 29: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 30: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
        }
        break;
        case 42:
        case 170: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
            stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 4, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, stC1, 3, 3, 2);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 9, 6, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 11, 3, 2);
          }
        }
        break;
        case 43:
        case 171: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 4, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, stC1, 3, 3, 2);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 9, 6, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 11, 3, 2);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC5, 9, 4, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 56: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 57: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 58: {
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 59: {
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 60: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 61: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
        }
        break;
        case 62: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 63: {
          stE10 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, stC7, 5, 2, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 66: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 67: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 70: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 71: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 72:
        case 76: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 73:
        case 77: {
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 11, 3, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, stC1, 9, 6, 1);
            stE20 = sPixel.Interpolate(stC3, stC4, stC7, 3, 3, 2);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 4, 3, 1);
          }
        }
        break;
        case 74:
        case 107: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 75: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 78: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 79: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 80:
        case 81: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 82:
        case 214: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 83: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 84:
        case 85: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 11, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 9, 6, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 3, 3, 2);
            stE31 = sPixel.Interpolate(stC7, stC5, stC4, 4, 3, 1);
          }
        }
        break;
        case 86: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 87: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 88:
        case 248: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 89: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 90: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 91: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 92: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 93: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 94: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 95: {
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 98: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 99: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 102: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 103: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
        }
        break;
        case 104:
        case 108: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 105:
        case 109: {
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 11, 3, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, stC1, 9, 6, 1);
            stE20 = sPixel.Interpolate(stC3, stC4, stC7, 3, 3, 2);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 4, 3, 1);
          }
        }
        break;
        case 106: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 110: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 111: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, stC8, 6, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 112:
        case 113: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC5, 9, 7);
          }
        }
        break;
        case 114: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 115: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 116:
        case 117: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 118: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 119: {
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, 9, 7);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 8, 5, 3);
          }
        }
        break;
        case 120: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 121: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 122: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
            stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 123: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 124: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 125: {
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 11, 3, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, stC1, 9, 6, 1);
            stE20 = sPixel.Interpolate(stC3, stC4, stC7, 3, 3, 2);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 4, 3, 1);
          }
        }
        break;
        case 126: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 127: {
          stE10 = stC4;
          stE21 = sPixel.Interpolate(stC4, stC8, 13, 3);
          stE31 = sPixel.Interpolate(stC4, stC8, 11, 5);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 146:
        case 178: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 4, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 3, 3, 2);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 9, 6, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 11, 3, 2);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 150:
        case 182: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 4, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 3, 3, 2);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 9, 6, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 11, 3, 2);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, stC3, 9, 4, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 152: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 153: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 154: {
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 155: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 156: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 157: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 158: {
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 159: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 184: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 185: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 186: {
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 187: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 4, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, stC1, 3, 3, 2);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 9, 6, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 11, 3, 2);
          }
        }
        break;
        case 188: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 189: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 190: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC4, 4, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 3, 3, 2);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 9, 6, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, stC5, 11, 3, 2);
          }
        }
        break;
        case 191: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 194: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 195: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 198: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 199: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 200:
        case 204: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
            stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC7, stC3, 9, 7);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
        }
        break;
        case 202: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 203: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 206: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 207: {
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 9, 7);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, stC1, 8, 5, 3);
          }
        }
        break;
        case 208:
        case 209: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 210: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 211: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 212:
        case 213: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 11, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 9, 6, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 3, 3, 2);
            stE31 = sPixel.Interpolate(stC7, stC5, stC4, 4, 3, 1);
          }
        }
        break;
        case 215: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 6, 1, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 216: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 217: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 218: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 219: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 220: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
            stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 221: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 11, 3, 2);
            stE11 = sPixel.Interpolate(stC4, stC5, stC1, 9, 6, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 3, 3, 2);
            stE31 = sPixel.Interpolate(stC7, stC5, stC4, 4, 3, 1);
          }
        }
        break;
        case 222: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 223: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 13, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 11, 5);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 226: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 227: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 230: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 231: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 232:
        case 236: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC7, stC3, 9, 7);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC5, 9, 4, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC5, stC1, 11, 3, 2);
          stE20 = stC4;
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
        }
        break;
        case 234: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
            stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 235: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, stC5, 6, 1, 1);
          stE20 = stC4;
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 238: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC7, stC3, 9, 7);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 239: {
          stE01 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = stC4;
          stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE31 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 240:
        case 241: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC5, 9, 7);
          }
        }
        break;
        case 242: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
            stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 243: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC5, 9, 7);
          }
        }
        break;
        case 244:
        case 245: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 9, 4, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 11, 3, 2);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, stC3, 6, 1, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 247: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC2, stC1, 5, 2, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 250: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
        }
        break;
        case 251: {
          stE01 = sPixel.Interpolate(stC4, stC2, 11, 5);
          stE11 = sPixel.Interpolate(stC4, stC2, 13, 3);
          stE20 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 252: {
          stE00 = sPixel.Interpolate(stC4, stC0, stC1, 5, 2, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 253: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = sPixel.Interpolate(stC4, stC0, 11, 5);
          stE10 = sPixel.Interpolate(stC4, stC0, 13, 3);
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 255: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, 
      stE10, stE11, 
      stE20, stE21, 
      stE30, stE31, 
      });
    }
    #endregion
    #region standard HQ3x casepath
    public static sPixel[] _arrHQ3x(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;
      switch (bytePattern) {
        #region HQ3x PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 10:
        case 138: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 11:
        case 139: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 15:
        case 143: {
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 18:
        case 50: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE12 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 22:
        case 54: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 23:
        case 55: {
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 24: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 25: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 26:
        case 31: {
          stE01 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 27: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 28: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 29: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 30: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 42:
        case 170: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE01 = stC4;
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 43:
        case 171: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 56: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 57: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 58: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 59: {
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 60: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 61: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 62: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 63: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 66: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 67: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 70: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 71: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 72:
        case 76: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
        }
        break;
        case 73:
        case 77: {
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
        }
        break;
        case 74:
        case 107: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
          }
        }
        break;
        case 75: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 78: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 79: {
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 80:
        case 81: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 82:
        case 214: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
          }
        }
        break;
        case 83: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 84:
        case 85: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE12 = stC4;
            stE21 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 86: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 87: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 88:
        case 248: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 89: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 90: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 91: {
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 92: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 93: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 94: {
          stE10 = stC4;
          stE11 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 95: {
          stE01 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 98: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 99: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 102: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 103: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
        }
        break;
        case 104:
        case 108: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
        }
        break;
        case 105:
        case 109: {
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
        }
        break;
        case 106: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
        }
        break;
        case 110: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
        }
        break;
        case 111: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 112:
        case 113: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 114: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 115: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 116:
        case 117: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 118: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 119: {
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 120: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
        }
        break;
        case 121: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 122: {
          stE01 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 123: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
          }
        }
        break;
        case 124: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
        }
        break;
        case 125: {
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
        }
        break;
        case 126: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 127: {
          stE11 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 146:
        case 178: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE12 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 150:
        case 182: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 152: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 153: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 154: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 155: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 156: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 157: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 158: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 159: {
          stE01 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 184: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 185: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 186: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 187: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 188: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 189: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
        }
        break;
        case 190: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 191: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 194: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 195: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 198: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 199: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 200:
        case 204: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 202: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 203: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 206: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 207: {
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 208:
        case 209: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 210: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 211: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 212:
        case 213: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 215: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 216: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 217: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 218: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 219: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 220: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 221: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 222: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
          }
        }
        break;
        case 223: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 226: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 227: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 230: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 231: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
        }
        break;
        case 232:
        case 236: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 234: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 235: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
          }
        }
        break;
        case 238: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
            stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 239: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 240:
        case 241: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 242: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 243: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 244:
        case 245: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
          }
        }
        break;
        case 247: {
          stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE01 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 250: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
        }
        break;
        case 251: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
          }
        }
        break;
        case 252: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 253: {
          stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
            stE20 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC4;
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
          }
        }
        break;
        case 255: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, stE02, 
      stE10, stE11, stE12, 
      stE20, stE21, stE22, 
      });
    }
    #endregion
    #region standard HQ4x casepath
    public static sPixel[] _arrHQ4x(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE03 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE12 = stC4;
      sPixel stE13 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;
      sPixel stE23 = stC4;
      sPixel stE30 = stC4;
      sPixel stE31 = stC4;
      sPixel stE32 = stC4;
      sPixel stE33 = stC4;
      switch (bytePattern) {
        #region HQ4x PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 10:
        case 138: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE11 = stC4;
          }
        }
        break;
        case 11:
        case 139: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, 5, 3);
            stE02 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, stC4, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 15:
        case 143: {
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
            stE10 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, 5, 3);
            stE02 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, stC4, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 18:
        case 50: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 5, 3);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, stC4, 2, 1, 1);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 22:
        case 54: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 23:
        case 55: {
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE02 = stC4;
            stE03 = stC4;
            stE12 = stC4;
            stE13 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 5, 3);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, stC4, 2, 1, 1);
          }
        }
        break;
        case 24: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 25: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 26:
        case 31: {
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 27: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 28: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 29: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 30: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
        }
        break;
        case 42:
        case 170: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
            stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
            stE20 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 43:
        case 171: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
            stE20 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC7, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC5, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 56: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 57: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 58: {
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 59: {
          stE11 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 60: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 61: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 62: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 63: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, stC8, 5, 2, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 66: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 67: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 70: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 71: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 72:
        case 76: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 73:
        case 77: {
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 5, 3);
            stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 74:
        case 107: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 75: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 78: {
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
        }
        break;
        case 79: {
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 80:
        case 81: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 82:
        case 214: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 83: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 84:
        case 85: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE03 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE13 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC7, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 86: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 87: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 88:
        case 248: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE21 = stC4;
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 89: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 90: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 91: {
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 92: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 93: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 94: {
          stE12 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 95: {
          stE11 = stC4;
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 98: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 99: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 102: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 103: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
        }
        break;
        case 104:
        case 108: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 105:
        case 109: {
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE20 = stC4;
            stE21 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 5, 3);
            stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 106: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 110: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 111: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, stC8, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 112:
        case 113: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC4, stC7, 2, 1, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE32 = sPixel.Interpolate(stC7, stC5, 5, 3);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 114: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 115: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 116:
        case 117: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 118: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 119: {
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE02 = stC4;
            stE03 = stC4;
            stE12 = stC4;
            stE13 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 5, 3);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, stC4, 2, 1, 1);
          }
        }
        break;
        case 120: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 121: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 122: {
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
            stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          } else {
            stE22 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 123: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 124: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
        }
        break;
        case 125: {
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE20 = stC4;
            stE21 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 5, 3);
            stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
        }
        break;
        case 126: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 127: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC8, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC8, 3, 1);
          stE33 = sPixel.Interpolate(stC4, stC8, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 146:
        case 178: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, 5, 3);
            stE23 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 150:
        case 182: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE12 = stC4;
            stE13 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, 5, 3);
            stE23 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = stC4;
          stE13 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC7, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 152: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 153: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 154: {
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 155: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 156: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 157: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 158: {
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 159: {
          stE02 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, stC6, 5, 2, 1);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 184: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 185: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 186: {
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 187: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
            stE11 = stC4;
            stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
            stE20 = sPixel.Interpolate(stC3, stC4, 3, 1);
            stE30 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 188: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 189: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
        }
        break;
        case 190: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE12 = stC4;
            stE13 = stC4;
            stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, 5, 3);
            stE23 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 191: {
          stE01 = stC4;
          stE02 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC7, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC7, 5, 3);
          stE33 = sPixel.Interpolate(stC4, stC7, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 194: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 195: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 198: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 199: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 200:
        case 204: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
        }
        break;
        case 202: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
        }
        break;
        case 203: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 206: {
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
        }
        break;
        case 207: {
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
            stE10 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, 5, 3);
            stE02 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, stC4, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 208:
        case 209: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 210: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 211: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 212:
        case 213: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE22 = stC4;
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE13 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC7, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 215: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = stC4;
          stE13 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, stC6, 5, 2, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 216: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 217: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 218: {
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 219: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 220: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
            stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE21 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 221: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE22 = stC4;
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE13 = sPixel.Interpolate(stC5, stC4, 3, 1);
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC7, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 222: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 223: {
          stE02 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC6, 3, 1);
          stE21 = sPixel.Interpolate(stC4, stC6, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC6, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC6, 3, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 226: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 227: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 230: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 231: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
        }
        break;
        case 232:
        case 236: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
            stE30 = stC4;
            stE31 = stC4;
            stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC5, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC1, 5, 2, 1);
          stE20 = stC4;
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE31 = stC4;
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 234: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
            stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
            stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE11 = stC4;
          }
        }
        break;
        case 235: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, stC2, 5, 2, 1);
          stE20 = stC4;
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE31 = stC4;
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 238: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
            stE30 = stC4;
            stE31 = stC4;
            stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
            stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE33 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
        }
        break;
        case 239: {
          stE01 = stC4;
          stE02 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE03 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE10 = stC4;
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE20 = stC4;
          stE21 = stC4;
          stE22 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE23 = sPixel.Interpolate(stC4, stC5, 5, 3);
          stE31 = stC4;
          stE32 = sPixel.Interpolate(stC4, stC5, 7, 1);
          stE33 = sPixel.Interpolate(stC4, stC5, 5, 3);
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 240:
        case 241: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
            stE23 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC4, stC7, 2, 1, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE32 = sPixel.Interpolate(stC7, stC5, 5, 3);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 242: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
            stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
            stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = stC4;
            stE13 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 243: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
            stE23 = stC4;
            stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
            stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC4, stC7, 2, 1, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, 3, 1);
            stE32 = sPixel.Interpolate(stC7, stC5, 5, 3);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 244:
        case 245: {
          stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          stE01 = sPixel.Interpolate(stC4, stC1, stC3, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC3, stC1, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = stC4;
          stE23 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC3, stC0, 5, 2, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = stC4;
          stE23 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 247: {
          stE00 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE02 = stC4;
          stE10 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE11 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE12 = stC4;
          stE13 = stC4;
          stE20 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE21 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE22 = stC4;
          stE23 = stC4;
          stE30 = sPixel.Interpolate(stC4, stC3, 5, 3);
          stE31 = sPixel.Interpolate(stC4, stC3, 7, 1);
          stE32 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, stC2, 5, 2, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE31 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 250: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE21 = stC4;
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 251: {
          stE02 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE03 = sPixel.Interpolate(stC4, stC2, 5, 3);
          stE11 = stC4;
          stE12 = sPixel.Interpolate(stC4, stC2, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC2, 3, 1);
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE31 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 252: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, stC0, 5, 2, 1);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE32 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 253: {
          stE00 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE02 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE03 = sPixel.Interpolate(stC4, stC1, 5, 3);
          stE10 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE11 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE12 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE13 = sPixel.Interpolate(stC4, stC1, 7, 1);
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = sPixel.Interpolate(stC4, stC0, 5, 3);
          stE01 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stC0, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stC0, 7, 1);
          stE12 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE32 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 255: {
          stE01 = stC4;
          stE02 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, stE02, stE03, 
      stE10, stE11, stE12, stE13, 
      stE20, stE21, stE22, stE23, 
      stE30, stE31, stE32, stE33, 
      });
    }
    #endregion
    #region standard LQ2x casepath
    public static sPixel[] _arrLQ2x(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      switch (bytePattern) {
        #region LQ2x PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          stE00 = stC3;
          stE01 = stC3;
          stE10 = stC3;
          stE11 = stC3;
        }
        break;
        case 10:
        case 138: {
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE10 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 3, 3, 2);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 3, 3, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 3, 1);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          stE00 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = stC2;
          stE11 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC2;
            stE01 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC2, 3, 3, 2);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          stE10 = stC3;
          stE11 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC3;
            stE01 = stC3;
          } else {
            stE00 = sPixel.Interpolate(stC3, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC3, 3, 3, 2);
          }
        }
        break;
        case 26: {
          stE10 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 31:
        case 95: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 42:
        case 170: {
          stE01 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 3, 3, 2);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          stE01 = stC2;
          stE11 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 3, 3, 2);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
        }
        break;
        case 58:
        case 154:
        case 186: {
          stE10 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 59: {
          stE10 = stC2;
          stE11 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 63: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          stE00 = stC0;
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          stE01 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE00 = stC1;
            stE10 = stC1;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC7, stC1, 3, 3, 2);
          }
        }
        break;
        case 74: {
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 79: {
          stE01 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 83:
        case 115: {
          stE00 = stC2;
          stE10 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 84:
        case 212: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC0, 3, 3, 2);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          stE00 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE01 = stC1;
            stE11 = stC1;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC1, 3, 3, 2);
          }
        }
        break;
        case 87: {
          stE00 = stC3;
          stE10 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC3;
          } else {
            stE11 = sPixel.Interpolate(stC3, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          stE00 = stC0;
          stE01 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 89:
        case 93: {
          stE00 = stC1;
          stE01 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 90: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 91: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC2;
          } else {
            stE10 = sPixel.Interpolate(stC2, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 92: {
          stE00 = stC0;
          stE01 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 94: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 107:
        case 123: {
          stE01 = stC2;
          stE11 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC2;
          } else {
            stE10 = sPixel.Interpolate(stC2, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 111: {
          stE01 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
        }
        break;
        case 112:
        case 240: {
          stE00 = stC0;
          stE01 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE10 = stC0;
            stE11 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC0, 3, 3, 2);
          }
        }
        break;
        case 113:
        case 241: {
          stE00 = stC1;
          stE01 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE10 = stC1;
            stE11 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC7, 3, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC1, 3, 3, 2);
          }
        }
        break;
        case 114: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 116: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 117: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 121: {
          stE00 = stC1;
          stE01 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 122: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 126: {
          stE00 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 127: {
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC0, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 158: {
          stE10 = stC0;
          stE11 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 159: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 191: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          stE00 = stC0;
          stE01 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE11 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC3, stC7, stC0, 3, 3, 2);
            stE11 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = stC1;
          stE01 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, stC7, 6, 1, 1);
          }
        }
        break;
        case 211: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 215: {
          stE00 = stC3;
          stE10 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC3;
          } else {
            stE11 = sPixel.Interpolate(stC3, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 218: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 219: {
          stE01 = stC2;
          stE10 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 220: {
          stE00 = stC0;
          stE01 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 223: {
          stE10 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = stC1;
          stE01 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, stC7, 14, 1, 1);
          }
        }
        break;
        case 234: {
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 235: {
          stE01 = stC2;
          stE11 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC2;
          } else {
            stE10 = sPixel.Interpolate(stC2, stC3, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 239: {
          stE01 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
        }
        break;
        case 242: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 243: {
          stE00 = stC2;
          stE01 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE10 = stC2;
            stE11 = stC2;
          } else {
            stE10 = sPixel.Interpolate(stC2, stC7, 3, 1);
            stE11 = sPixel.Interpolate(stC5, stC7, stC2, 3, 3, 2);
          }
        }
        break;
        case 244: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 14, 1, 1);
          }
        }
        break;
        case 245: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, stC7, 14, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 247: {
          stE00 = stC3;
          stE10 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC3;
          } else {
            stE11 = sPixel.Interpolate(stC3, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = stC1;
          stE01 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 251: {
          stE01 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC2;
          } else {
            stE10 = sPixel.Interpolate(stC2, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 252: {
          stE00 = stC0;
          stE01 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 14, 1, 1);
          }
        }
        break;
        case 253: {
          stE00 = stC1;
          stE01 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, stC7, 14, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 255: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, stC7, 14, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, stC7, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 14, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 14, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, 
      stE10, stE11, 
      });
    }
    #endregion
    #region standard LQ2x3 casepath
    public static sPixel[] _arrLQ2x3(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      switch (bytePattern) {
        #region LQ2x3 PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          stE21 = stC1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          stE00 = stC3;
          stE01 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
        }
        break;
        case 10:
        case 138: {
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 15, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 10, 5, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC0, stC3, 13, 3);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 10, 5, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 5, 3);
            stE10 = sPixel.Interpolate(stC4, stC3, 13, 3);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          stE10 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC0;
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC2;
            stE01 = stC2;
            stE11 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, 5, 3);
            stE01 = sPixel.Interpolate(stC1, stC5, stC2, 10, 5, 1);
            stE11 = sPixel.Interpolate(stC2, stC5, 13, 3);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          stE10 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC3;
            stE01 = stC3;
            stE11 = stC3;
          } else {
            stE00 = sPixel.Interpolate(stC3, stC1, 5, 3);
            stE01 = sPixel.Interpolate(stC1, stC5, stC3, 10, 5, 1);
            stE11 = sPixel.Interpolate(stC3, stC5, 13, 3);
          }
        }
        break;
        case 26: {
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
          }
        }
        break;
        case 31:
        case 95: {
          stE20 = stC4;
          stE21 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 42:
        case 170: {
          stE11 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 7, 5, 4);
            stE01 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, 13, 3);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          stE11 = stC2;
          stE21 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
            stE20 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 5, 4);
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
            stE20 = sPixel.Interpolate(stC2, stC3, 13, 3);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 58:
        case 154:
        case 186: {
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 59: {
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          if (stC1.IsLike(stC5)) {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 10, 3, 3);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsLike(stC3))) {
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE01 = stC2;
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC2, stC3, 15, 1);
          }
        }
        break;
        case 63: {
          stE10 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          stE00 = stC0;
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          stE01 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE00 = stC1;
            stE10 = stC1;
            stE20 = stC1;
            stE21 = stC1;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 13, 3);
            stE10 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE20 = sPixel.Interpolate(stC7, stC3, stC1, 7, 5, 4);
            stE21 = sPixel.Interpolate(stC1, stC7, 15, 1);
          }
        }
        break;
        case 74: {
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC0, stC1, 15, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 79: {
          stE11 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC4, stC1, 15, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
            stE20 = stC1;
            stE21 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC1, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC1, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC0;
            stE01 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
          }
        }
        break;
        case 83:
        case 115: {
          stE00 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 84:
        case 212: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC5, 13, 3);
            stE11 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE20 = sPixel.Interpolate(stC0, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC7, stC5, stC0, 7, 5, 4);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          stE00 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE01 = stC1;
            stE11 = stC1;
            stE20 = stC1;
            stE21 = stC1;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, 13, 3);
            stE11 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE20 = sPixel.Interpolate(stC1, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC7, stC5, stC1, 7, 5, 4);
          }
        }
        break;
        case 87: {
          stE10 = stC3;
          stE20 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC3;
          } else {
            stE21 = sPixel.Interpolate(stC3, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC3;
            stE01 = stC3;
            stE11 = stC3;
          } else {
            stE00 = sPixel.Interpolate(stC3, stC1, 15, 1);
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC3, stC5, 15, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          stE00 = stC0;
          stE01 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
            stE21 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 89:
        case 93:
        case 253: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 90: {
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 91: {
          stE11 = stC2;
          if (stC1.IsLike(stC5)) {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 10, 3, 3);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsLike(stC3))) {
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE01 = stC2;
          }
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC2, stC3, 15, 1);
          }
        }
        break;
        case 92: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 94: {
          stE10 = stC0;
          if (stC1.IsLike(stC3)) {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
          if ((stC1.IsLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = stC0;
          }
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
          }
        }
        break;
        case 107:
        case 123: {
          stE10 = stC2;
          stE11 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
            stE21 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC2, stC7, 15, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
          }
        }
        break;
        case 111: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC7, 15, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 112:
        case 240: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, 13, 3);
            stE20 = sPixel.Interpolate(stC0, stC7, 9, 7);
            stE21 = sPixel.Interpolate(stC7, stC5, stC0, 10, 5, 1);
          }
        }
        break;
        case 113:
        case 241: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
            stE20 = stC1;
            stE21 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, 13, 3);
            stE20 = sPixel.Interpolate(stC1, stC7, 9, 7);
            stE21 = sPixel.Interpolate(stC7, stC5, stC1, 10, 5, 1);
          }
        }
        break;
        case 114: {
          stE00 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 116:
        case 244: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 117:
        case 245: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 121: {
          stE00 = stC1;
          stE01 = stC1;
          stE11 = stC1;
          if (stC7.IsLike(stC5)) {
            stE21 = sPixel.Interpolate(stC1, stC5, stC7, 10, 3, 3);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsLike(stC3))) {
            stE21 = sPixel.Interpolate(stC1, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE21 = stC1;
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
            stE20 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 6, 5, 5);
          }
        }
        break;
        case 122: {
          stE11 = stC0;
          if (stC7.IsLike(stC5)) {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsLike(stC3))) {
            stE21 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE21 = stC0;
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 126: {
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC0;
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
          }
        }
        break;
        case 127: {
          if (stC1.IsLike(stC5)) {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 5, 5);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsLike(stC3))) {
            stE01 = sPixel.Interpolate(stC4, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE01 = stC4;
          }
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC4, stC7, 15, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE11 = stC4;
          } else {
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          stE10 = stC0;
          stE20 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC0;
            stE01 = stC0;
            stE11 = stC0;
            stE21 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, stC0, 7, 5, 4);
            stE11 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, 13, 3);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 158: {
          stE10 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsLike(stC3)) {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
          if ((stC1.IsLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = stC0;
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
          }
        }
        break;
        case 159: {
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 191: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          stE00 = stC0;
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 13, 3);
            stE20 = sPixel.Interpolate(stC7, stC3, stC0, 10, 5, 1);
            stE21 = sPixel.Interpolate(stC0, stC7, 9, 7);
          }
        }
        break;
        case 201:
        case 205:
        case 233:
        case 237: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE21 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 10, 3, 3);
          }
        }
        break;
        case 211: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
            stE20 = stC2;
            stE21 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC2, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC2, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 215: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE20 = stC3;
            stE21 = stC3;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC3, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 218: {
          stE10 = stC0;
          if (stC7.IsLike(stC3)) {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 10, 3, 3);
          }
          if ((stC7.IsLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = stC0;
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
            stE21 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 219: {
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
            stE20 = stC2;
            stE21 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC2, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC2, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 15, 1);
          }
        }
        break;
        case 220: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          if (stC7.IsLike(stC3)) {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 10, 3, 3);
          }
          if ((stC7.IsLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = stC0;
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
            stE21 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 223: {
          if (stC1.IsLike(stC3)) {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 5, 5);
          }
          if ((stC1.IsLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = sPixel.Interpolate(stC4, stC1, 15, 1);
          }
          if ((stC1.IsNotLike(stC5) && stC1.IsNotLike(stC3))) {
            stE00 = stC4;
          }
          if (stC7.IsNotLike(stC5)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC3)) {
            stE10 = stC4;
          } else {
            stE10 = sPixel.Interpolate(stC4, stC3, 15, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
            stE11 = sPixel.Interpolate(stC4, stC5, 15, 1);
          }
        }
        break;
        case 234: {
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
            stE21 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 235: {
          stE10 = stC2;
          stE11 = stC2;
          stE21 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
          }
        }
        break;
        case 239: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
        }
        break;
        case 242: {
          stE00 = stC0;
          stE10 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC7, 15, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 6, 5, 5);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 243: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
            stE20 = stC2;
            stE21 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, 13, 3);
            stE20 = sPixel.Interpolate(stC2, stC7, 9, 7);
            stE21 = sPixel.Interpolate(stC7, stC5, stC2, 10, 5, 1);
          }
        }
        break;
        case 246: {
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC0;
            stE01 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
          }
        }
        break;
        case 247: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC3;
          } else {
            stE21 = sPixel.Interpolate(stC3, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        case 249: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC1;
            stE21 = stC1;
          } else {
            stE11 = sPixel.Interpolate(stC1, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC1, stC5, stC7, 6, 5, 5);
          }
        }
        break;
        case 251: {
          if (stC7.IsLike(stC5)) {
            stE21 = sPixel.Interpolate(stC2, stC5, stC7, 6, 5, 5);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsLike(stC3))) {
            stE21 = sPixel.Interpolate(stC2, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE21 = stC2;
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC2;
            stE20 = stC2;
          } else {
            stE10 = sPixel.Interpolate(stC2, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC2, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC2;
          } else {
            stE11 = sPixel.Interpolate(stC2, stC5, 15, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, stC3, 6, 5, 5);
            stE01 = sPixel.Interpolate(stC2, stC1, 15, 1);
          }
        }
        break;
        case 252: {
          stE00 = stC0;
          stE01 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
        }
        break;
        case 254: {
          if (stC7.IsLike(stC3)) {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 6, 5, 5);
          }
          if ((stC7.IsLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = sPixel.Interpolate(stC0, stC7, 15, 1);
          }
          if ((stC7.IsNotLike(stC5) && stC7.IsNotLike(stC3))) {
            stE20 = stC0;
          }
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 15, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE11 = stC0;
            stE21 = stC0;
          } else {
            stE11 = sPixel.Interpolate(stC0, stC5, 15, 1);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC0;
            stE01 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, 15, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 6, 5, 5);
          }
        }
        break;
        case 255: {
          stE10 = stC4;
          stE11 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 10, 3, 3);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, stC7, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 10, 3, 3);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 10, 3, 3);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, 
      stE10, stE11, 
      stE20, stE21, 
      });
    }
    #endregion
    #region standard LQ2x4 casepath
    public static sPixel[] _arrLQ2x4(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE30 = stC4;
      sPixel stE31 = stC4;
      switch (bytePattern) {
        #region LQ2x4 PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE30 = stC1;
          stE31 = stC1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE30 = stC2;
          stE31 = stC2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          stE00 = stC3;
          stE01 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE30 = stC3;
          stE31 = stC3;
        }
        break;
        case 10:
        case 138: {
          stE01 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC0, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          stE01 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 9, 7);
            stE01 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, stC1, 8, 5, 3);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 9, 7);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, stC1, 8, 5, 3);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          stE00 = stC0;
          stE10 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC2;
            stE01 = stC2;
            stE11 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, 9, 7);
            stE11 = sPixel.Interpolate(stC2, stC5, stC1, 8, 5, 3);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          stE10 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC3;
            stE01 = stC3;
            stE11 = stC3;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC5, 9, 7);
            stE11 = sPixel.Interpolate(stC3, stC5, stC1, 8, 5, 3);
          }
        }
        break;
        case 26: {
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC0, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 31:
        case 95: {
          stE20 = stC4;
          stE21 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 42:
        case 170: {
          stE01 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 4, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, stC1, 3, 3, 2);
            stE20 = sPixel.Interpolate(stC0, stC3, 5, 3);
            stE30 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          stE01 = stC2;
          stE11 = stC2;
          stE21 = stC2;
          stE31 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
            stE20 = stC2;
            stE30 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 4, 3, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, stC1, 3, 3, 2);
            stE20 = sPixel.Interpolate(stC2, stC3, 5, 3);
            stE30 = sPixel.Interpolate(stC2, stC3, 7, 1);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 58:
        case 154:
        case 186: {
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 59: {
          stE20 = stC2;
          stE21 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
            stE11 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC2, stC5, 7, 1);
          }
        }
        break;
        case 63: {
          stE10 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          stE01 = stC1;
          stE11 = stC1;
          stE21 = stC1;
          stE31 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE00 = stC1;
            stE10 = stC1;
            stE20 = stC1;
            stE30 = stC1;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 7, 1);
            stE10 = sPixel.Interpolate(stC1, stC3, 5, 3);
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 3, 3, 2);
            stE30 = sPixel.Interpolate(stC7, stC3, stC1, 4, 3, 1);
          }
        }
        break;
        case 74: {
          stE01 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC0, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          stE01 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC0, stC7, stC3, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
        }
        break;
        case 79: {
          stE01 = stC4;
          stE11 = stC4;
          stE21 = stC4;
          stE31 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC4, stC7, stC3, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC0, stC5, 2, 1, 1);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          stE30 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
            stE31 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          stE00 = stC0;
          stE10 = stC0;
          stE20 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC0, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 83:
        case 115: {
          stE00 = stC2;
          stE10 = stC2;
          stE20 = stC2;
          stE30 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
            stE31 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC2, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
            stE11 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC2, stC5, 7, 1);
          }
        }
        break;
        case 84:
        case 212: {
          stE00 = stC0;
          stE10 = stC0;
          stE20 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 5, 3);
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 3, 3, 2);
            stE31 = sPixel.Interpolate(stC7, stC5, stC0, 4, 3, 1);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          stE00 = stC1;
          stE10 = stC1;
          stE20 = stC1;
          stE30 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE01 = stC1;
            stE11 = stC1;
            stE21 = stC1;
            stE31 = stC1;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, 7, 1);
            stE11 = sPixel.Interpolate(stC1, stC5, 5, 3);
            stE21 = sPixel.Interpolate(stC1, stC5, stC7, 3, 3, 2);
            stE31 = sPixel.Interpolate(stC7, stC5, stC1, 4, 3, 1);
          }
        }
        break;
        case 87: {
          stE00 = stC3;
          stE10 = stC3;
          stE20 = stC3;
          stE30 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC3;
            stE31 = stC3;
          } else {
            stE21 = sPixel.Interpolate(stC3, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC3, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
            stE11 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC3, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC3, stC5, 3, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC0, stC5, 2, 1, 1);
          }
        }
        break;
        case 89:
        case 93: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
            stE30 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC1, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
            stE31 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC1, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 90: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC0, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 91: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
            stE30 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC2, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
            stE31 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC2, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
            stE11 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC2, stC5, 7, 1);
          }
        }
        break;
        case 92: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC0, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 94: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC0, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 107:
        case 123: {
          stE01 = stC2;
          stE11 = stC2;
          stE21 = stC2;
          stE31 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
            stE30 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC2, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 111: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE21 = stC4;
          stE31 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 112:
        case 240: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC0, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC5, 9, 7);
          }
        }
        break;
        case 113:
        case 241: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
            stE30 = stC1;
            stE31 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC1, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC5, 9, 7);
          }
        }
        break;
        case 114: {
          stE00 = stC0;
          stE10 = stC0;
          stE20 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 116: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 117: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          stE30 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
            stE31 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC1, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 121: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
            stE30 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC1, stC3, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
            stE31 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC1, stC7, stC5, 5, 2, 1);
          }
        }
        break;
        case 122: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, stC5, 5, 2, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 126: {
          stE00 = stC0;
          stE10 = stC0;
          stE21 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 127: {
          stE10 = stC4;
          stE21 = stC4;
          stE31 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC3, stC4, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE11 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC4, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC5, 3, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          stE00 = stC0;
          stE10 = stC0;
          stE20 = stC0;
          stE30 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC5, stC0, 4, 3, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, stC1, 3, 3, 2);
            stE21 = sPixel.Interpolate(stC0, stC5, 5, 3);
            stE31 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = stC2;
          stE10 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC2;
            stE11 = stC2;
          } else {
            stE01 = sPixel.Interpolate(stC2, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC2, stC5, 7, 1);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 158: {
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 159: {
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 191: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC7, stC3, 9, 7);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE21 = stC1;
          stE31 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
            stE30 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC1, stC7, stC3, 5, 2, 1);
          }
        }
        break;
        case 211: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE30 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
            stE31 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC2, stC5, 2, 1, 1);
          }
        }
        break;
        case 215: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE30 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC3;
            stE31 = stC3;
          } else {
            stE21 = sPixel.Interpolate(stC3, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 218: {
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC0, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC0, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 219: {
          stE01 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE30 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
            stE31 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC2, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 220: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE30 = sPixel.Interpolate(stC0, stC7, stC3, 5, 2, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC0, stC5, 2, 1, 1);
          }
        }
        break;
        case 223: {
          stE11 = stC4;
          stE20 = stC4;
          stE30 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE31 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC4, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE31 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC1;
          } else {
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 6, 1, 1);
          }
        }
        break;
        case 234: {
          stE01 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 5, 2, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
        }
        break;
        case 235: {
          stE01 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE31 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC2;
          } else {
            stE30 = sPixel.Interpolate(stC2, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 239: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE31 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 242: {
          stE00 = stC0;
          stE10 = stC0;
          stE20 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE31 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC0, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, stC5, 5, 2, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 243: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
            stE30 = stC2;
            stE31 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, stC7, 8, 5, 3);
            stE30 = sPixel.Interpolate(stC2, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC5, 9, 7);
          }
        }
        break;
        case 244: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC0;
          } else {
            stE31 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 245: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE30 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC1;
          } else {
            stE31 = sPixel.Interpolate(stC1, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = stC0;
          stE10 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC0;
          } else {
            stE31 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 247: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE30 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC3;
          } else {
            stE31 = sPixel.Interpolate(stC3, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC1;
          } else {
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC1;
            stE31 = stC1;
          } else {
            stE21 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 251: {
          stE01 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC2;
          } else {
            stE30 = sPixel.Interpolate(stC2, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC2;
            stE31 = stC2;
          } else {
            stE21 = sPixel.Interpolate(stC2, stC5, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC2, stC5, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 252: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC0;
          } else {
            stE31 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 253: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC1;
          } else {
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC1;
          } else {
            stE31 = sPixel.Interpolate(stC1, stC5, stC7, 6, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = stC0;
          stE10 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC7, stC0, stC3, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC0;
          } else {
            stE31 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE11 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 255: {
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 6, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE31 = stC4;
          } else {
            stE31 = sPixel.Interpolate(stC4, stC5, stC7, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, stC5, 6, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, 
      stE10, stE11, 
      stE20, stE21, 
      stE30, stE31, 
      });
    }
    #endregion
    #region standard LQ3x casepath
    public static sPixel[] _arrLQ3x(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE12 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;
      switch (bytePattern) {
        #region LQ3x PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          stE00 = stC2;
          stE01 = stC2;
          stE02 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          stE00 = stC3;
          stE01 = stC3;
          stE02 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
        }
        break;
        case 10:
        case 138: {
          stE02 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          stE02 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC2, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 7, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE02 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC0, 3, 1);
            stE02 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          stE11 = stC4;
          stE12 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE02 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE02 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 3, 1);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          stE00 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
            stE12 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC2;
            stE01 = stC2;
            stE02 = stC2;
            stE12 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC2, stC5, 3, 1);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC3;
            stE01 = stC3;
            stE02 = stC3;
            stE12 = stC3;
          } else {
            stE00 = sPixel.Interpolate(stC3, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC3, stC5, 3, 1);
          }
        }
        break;
        case 26: {
          stE01 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE12 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 31:
        case 95: {
          stE01 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 42:
        case 170: {
          stE02 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC0, 3, 1);
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          stE02 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
            stE20 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC2, 3, 1);
            stE20 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = stC4;
          stE02 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 58:
        case 154:
        case 186: {
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 59: {
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC2, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 63: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          stE01 = stC1;
          stE02 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE22 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE00 = stC1;
            stE10 = stC1;
            stE20 = stC1;
            stE21 = stC1;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC1, stC7, 3, 1);
          }
        }
        break;
        case 74: {
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC0, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 79: {
          stE02 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC0, 7, 7, 2);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC1;
            stE21 = stC1;
            stE22 = stC1;
          } else {
            stE12 = sPixel.Interpolate(stC1, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC1, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC1, 7, 7, 2);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          stE00 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC0, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
          }
        }
        break;
        case 83:
        case 115: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC2;
          } else {
            stE22 = sPixel.Interpolate(stC2, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 84:
        case 212: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE02 = stC0;
            stE12 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE12 = sPixel.Interpolate(stC5, stC0, 3, 1);
            stE21 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          stE00 = stC1;
          stE01 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE20 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE02 = stC1;
            stE12 = stC1;
            stE21 = stC1;
            stE22 = stC1;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE12 = sPixel.Interpolate(stC5, stC1, 3, 1);
            stE21 = sPixel.Interpolate(stC1, stC7, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 87: {
          stE00 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC3;
          } else {
            stE22 = sPixel.Interpolate(stC3, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC3;
            stE02 = stC3;
            stE12 = stC3;
          } else {
            stE01 = sPixel.Interpolate(stC3, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC3, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC3, stC5, 7, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC0;
            stE22 = stC0;
          } else {
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC0, 7, 7, 2);
          }
        }
        break;
        case 89:
        case 93:
        case 253: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE21 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC1;
          } else {
            stE22 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 90: {
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 91: {
          stE11 = stC2;
          stE12 = stC2;
          stE21 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC2;
          } else {
            stE22 = sPixel.Interpolate(stC2, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC2, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 92: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 94: {
          stE10 = stC0;
          stE11 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
            stE12 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 107:
        case 123: {
          stE02 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE22 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
            stE21 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC2, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC2, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC2, stC1, 7, 1);
          }
        }
        break;
        case 111: {
          stE01 = stC4;
          stE02 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 112:
        case 240: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC0;
            stE20 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE12 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE20 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE21 = sPixel.Interpolate(stC7, stC0, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 113:
        case 241: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC1;
            stE20 = stC1;
            stE21 = stC1;
            stE22 = stC1;
          } else {
            stE12 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE20 = sPixel.Interpolate(stC1, stC7, 3, 1);
            stE21 = sPixel.Interpolate(stC7, stC1, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 114: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 116:
        case 244: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 117:
        case 245: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC1;
          } else {
            stE22 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 121: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC1;
            stE20 = stC1;
            stE21 = stC1;
          } else {
            stE10 = sPixel.Interpolate(stC1, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC1, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC1, stC7, 7, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC1;
          } else {
            stE22 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 122: {
          stE01 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 126: {
          stE00 = stC0;
          stE11 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
            stE12 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 127: {
          stE11 = stC4;
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE21 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC7, stC4, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC5, stC4, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          stE00 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
            stE12 = stC0;
            stE22 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC5, stC0, 3, 1);
            stE22 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = stC3;
          stE01 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC3;
          } else {
            stE02 = sPixel.Interpolate(stC3, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 158: {
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
            stE12 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
          }
        }
        break;
        case 159: {
          stE01 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 191: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE21 = sPixel.Interpolate(stC7, stC0, 3, 1);
            stE22 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
        }
        break;
        case 201:
        case 205:
        case 233:
        case 237: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 211: {
          stE00 = stC2;
          stE01 = stC2;
          stE02 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC2;
            stE21 = stC2;
            stE22 = stC2;
          } else {
            stE12 = sPixel.Interpolate(stC2, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC2, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC2, 7, 7, 2);
          }
        }
        break;
        case 215: {
          stE00 = stC3;
          stE01 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE20 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC3;
            stE22 = stC3;
          } else {
            stE21 = sPixel.Interpolate(stC3, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC3, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC3;
          } else {
            stE02 = sPixel.Interpolate(stC3, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 218: {
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC0, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 219: {
          stE02 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC2;
            stE21 = stC2;
            stE22 = stC2;
          } else {
            stE12 = sPixel.Interpolate(stC2, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC2, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC2, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC2, stC1, 7, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 7, 1);
          }
        }
        break;
        case 220: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC0, 7, 7, 2);
          }
        }
        break;
        case 223: {
          stE11 = stC4;
          stE20 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE21 = stC4;
            stE22 = stC4;
          } else {
            stE21 = sPixel.Interpolate(stC4, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC4, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC4, 7, 7, 2);
            stE10 = sPixel.Interpolate(stC4, stC3, 7, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC4;
            stE02 = stC4;
            stE12 = stC4;
          } else {
            stE01 = sPixel.Interpolate(stC4, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
            stE12 = sPixel.Interpolate(stC4, stC5, 7, 1);
          }
        }
        break;
        case 234: {
          stE01 = stC0;
          stE02 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
            stE21 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 235: {
          stE02 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC2, stC1, 7, 1);
          }
        }
        break;
        case 239: {
          stE01 = stC4;
          stE02 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 242: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC0, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 243: {
          stE00 = stC2;
          stE01 = stC2;
          stE02 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC2;
            stE20 = stC2;
            stE21 = stC2;
            stE22 = stC2;
          } else {
            stE12 = sPixel.Interpolate(stC2, stC5, 3, 1);
            stE20 = sPixel.Interpolate(stC2, stC7, 3, 1);
            stE21 = sPixel.Interpolate(stC7, stC2, 3, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
          }
        }
        break;
        case 247: {
          stE00 = stC3;
          stE01 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC3;
          } else {
            stE22 = sPixel.Interpolate(stC3, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC3;
          } else {
            stE02 = sPixel.Interpolate(stC3, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE21 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC1;
            stE22 = stC1;
          } else {
            stE12 = sPixel.Interpolate(stC1, stC5, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC1, 7, 7, 2);
          }
        }
        break;
        case 251: {
          stE02 = stC2;
          stE11 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC2;
            stE20 = stC2;
            stE21 = stC2;
          } else {
            stE10 = sPixel.Interpolate(stC2, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC2, stC3, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC2, stC7, 7, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC2;
            stE22 = stC2;
          } else {
            stE12 = sPixel.Interpolate(stC2, stC5, 7, 1);
            stE22 = sPixel.Interpolate(stC5, stC7, stC2, 7, 7, 2);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, stC2, 7, 7, 2);
            stE01 = sPixel.Interpolate(stC2, stC1, 7, 1);
          }
        }
        break;
        case 252: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = stC0;
          stE11 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE10 = stC0;
            stE20 = stC0;
          } else {
            stE10 = sPixel.Interpolate(stC0, stC3, 7, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, stC0, 7, 7, 2);
          }
          if (stC7.IsNotLike(stC5)) {
            stE12 = stC0;
            stE21 = stC0;
            stE22 = stC0;
          } else {
            stE12 = sPixel.Interpolate(stC0, stC5, 7, 1);
            stE21 = sPixel.Interpolate(stC0, stC7, 7, 1);
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE01 = stC0;
            stE02 = stC0;
          } else {
            stE01 = sPixel.Interpolate(stC0, stC1, 7, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, stC0, 7, 7, 2);
          }
        }
        break;
        case 255: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC4;
          } else {
            stE22 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, stE02, 
      stE10, stE11, stE12, 
      stE20, stE21, stE22, 
      });
    }
    #endregion
    #region standard LQ4x casepath
    public static sPixel[] _arrLQ4x(byte bytePattern, sPixel stC0, sPixel stC1, sPixel stC2, sPixel stC3, sPixel stC4, sPixel stC5, sPixel stC6, sPixel stC7, sPixel stC8) {
      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE02 = stC4;
      sPixel stE03 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      sPixel stE12 = stC4;
      sPixel stE13 = stC4;
      sPixel stE20 = stC4;
      sPixel stE21 = stC4;
      sPixel stE22 = stC4;
      sPixel stE23 = stC4;
      sPixel stE30 = stC4;
      sPixel stE31 = stC4;
      sPixel stE32 = stC4;
      sPixel stE33 = stC4;
      switch (bytePattern) {
        #region LQ4x PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE23 = stC1;
          stE30 = stC1;
          stE31 = stC1;
          stE32 = stC1;
          stE33 = stC1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          stE00 = stC2;
          stE01 = stC2;
          stE02 = stC2;
          stE03 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          stE32 = stC2;
          stE33 = stC2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          stE00 = stC3;
          stE01 = stC3;
          stE02 = stC3;
          stE03 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE13 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          stE23 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          stE32 = stC3;
          stE33 = stC3;
        }
        break;
        case 10:
        case 138: {
          stE02 = stC0;
          stE03 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 1, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          stE02 = stC2;
          stE03 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          stE32 = stC2;
          stE33 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
          }
        }
        break;
        case 14:
        case 142: {
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE02 = stC0;
            stE03 = stC0;
            stE10 = stC0;
            stE11 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, 5, 3);
            stE02 = sPixel.Interpolate(stC1, stC0, 3, 1);
            stE03 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC0, stC1, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE02 = stC4;
            stE03 = stC4;
            stE10 = stC4;
            stE11 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, 5, 3);
            stE02 = sPixel.Interpolate(stC1, stC4, 3, 1);
            stE03 = sPixel.Interpolate(stC4, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, stC4, 2, 1, 1);
            stE11 = sPixel.Interpolate(stC4, stC1, stC3, 6, 1, 1);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 19:
        case 51: {
          stE10 = stC2;
          stE11 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          stE32 = stC2;
          stE33 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC2;
            stE01 = stC2;
            stE02 = stC2;
            stE03 = stC2;
            stE12 = stC2;
            stE13 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 5, 3);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC2, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, stC2, 2, 1, 1);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          stE10 = stC3;
          stE11 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          stE23 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          stE32 = stC3;
          stE33 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE00 = stC3;
            stE01 = stC3;
            stE02 = stC3;
            stE03 = stC3;
            stE12 = stC3;
            stE13 = stC3;
          } else {
            stE00 = sPixel.Interpolate(stC3, stC1, 3, 1);
            stE01 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE02 = sPixel.Interpolate(stC1, stC5, 5, 3);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC3, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 26: {
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 31:
        case 95: {
          stE11 = stC4;
          stE12 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 42:
        case 170: {
          stE02 = stC0;
          stE03 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
            stE11 = stC0;
            stE20 = stC0;
            stE30 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC0, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC0, stC1, stC3, 6, 1, 1);
            stE20 = sPixel.Interpolate(stC3, stC0, 3, 1);
            stE30 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          stE02 = stC2;
          stE03 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE31 = stC2;
          stE32 = stC2;
          stE33 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
            stE11 = stC2;
            stE20 = stC2;
            stE30 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, stC3, 2, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, 5, 3);
            stE11 = sPixel.Interpolate(stC2, stC1, stC3, 6, 1, 1);
            stE20 = sPixel.Interpolate(stC3, stC2, 3, 1);
            stE30 = sPixel.Interpolate(stC2, stC3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          stE02 = stC0;
          stE03 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 47:
        case 175: {
          stE01 = stC4;
          stE02 = stC4;
          stE03 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 58:
        case 154:
        case 186: {
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 59: {
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          stE32 = stC2;
          stE33 = stC2;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
            stE03 = stC2;
            stE13 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC2, stC5, 3, 1);
          }
        }
        break;
        case 63: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE22 = stC1;
          stE23 = stC1;
          stE32 = stC1;
          stE33 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE00 = stC1;
            stE10 = stC1;
            stE20 = stC1;
            stE21 = stC1;
            stE30 = stC1;
            stE31 = stC1;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE10 = sPixel.Interpolate(stC3, stC1, 3, 1);
            stE20 = sPixel.Interpolate(stC3, stC7, 5, 3);
            stE21 = sPixel.Interpolate(stC1, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 74: {
          stE02 = stC0;
          stE03 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 1, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          stE02 = stC0;
          stE03 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 79: {
          stE02 = stC4;
          stE03 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC4, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 3, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE30 = stC1;
          stE31 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC1;
            stE32 = stC1;
            stE33 = stC1;
          } else {
            stE23 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC1, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 83:
        case 115: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC2;
            stE32 = stC2;
            stE33 = stC2;
          } else {
            stE23 = sPixel.Interpolate(stC2, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC2, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC2, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
            stE03 = stC2;
            stE13 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC2, stC5, 3, 1);
          }
        }
        break;
        case 84:
        case 212: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE03 = stC0;
            stE13 = stC0;
            stE22 = stC0;
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE03 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE13 = sPixel.Interpolate(stC5, stC0, 3, 1);
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC7, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC0, stC5, 2, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE30 = stC1;
          stE31 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE03 = stC1;
            stE13 = stC1;
            stE22 = stC1;
            stE23 = stC1;
            stE32 = stC1;
            stE33 = stC1;
          } else {
            stE03 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, 3, 1);
            stE22 = sPixel.Interpolate(stC1, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC7, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC1, stC5, 2, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 87: {
          stE00 = stC3;
          stE01 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC3;
            stE32 = stC3;
            stE33 = stC3;
          } else {
            stE23 = sPixel.Interpolate(stC3, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC3, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC3, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC3;
            stE03 = stC3;
            stE13 = stC3;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC3, stC5, 1, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 89:
        case 93: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
            stE30 = stC1;
            stE31 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC1, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC1;
            stE32 = stC1;
            stE33 = stC1;
          } else {
            stE23 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC1, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 90: {
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 91: {
          stE11 = stC2;
          stE12 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
            stE30 = stC2;
            stE31 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC2, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC2, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC2;
            stE32 = stC2;
            stE33 = stC2;
          } else {
            stE23 = sPixel.Interpolate(stC2, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC2, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC2, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
            stE03 = stC2;
            stE13 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC2, stC5, 3, 1);
          }
        }
        break;
        case 92: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 94: {
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 107:
        case 123: {
          stE02 = stC2;
          stE03 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE32 = stC2;
          stE33 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC2;
            stE30 = stC2;
            stE31 = stC2;
          } else {
            stE20 = sPixel.Interpolate(stC2, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC2, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
          }
        }
        break;
        case 111: {
          stE01 = stC4;
          stE02 = stC4;
          stE03 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 112:
        case 240: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC0;
            stE23 = stC0;
            stE30 = stC0;
            stE31 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE22 = sPixel.Interpolate(stC0, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC0, stC7, 2, 1, 1);
            stE30 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC0, 3, 1);
            stE32 = sPixel.Interpolate(stC7, stC5, 5, 3);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 113:
        case 241: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC1;
            stE23 = stC1;
            stE30 = stC1;
            stE31 = stC1;
            stE32 = stC1;
            stE33 = stC1;
          } else {
            stE22 = sPixel.Interpolate(stC1, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC1, stC7, 2, 1, 1);
            stE30 = sPixel.Interpolate(stC1, stC7, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC1, 3, 1);
            stE32 = sPixel.Interpolate(stC7, stC5, 5, 3);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 114: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 116: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 117: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE30 = stC1;
          stE31 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC1;
            stE32 = stC1;
            stE33 = stC1;
          } else {
            stE23 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC1, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 121: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
            stE30 = stC1;
            stE31 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC1, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC1;
            stE32 = stC1;
            stE33 = stC1;
          } else {
            stE23 = sPixel.Interpolate(stC1, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC1, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 122: {
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 3, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 126: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 127: {
          stE01 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC4;
            stE30 = stC4;
            stE31 = stC4;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC4, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC4, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC4;
            stE03 = stC4;
            stE13 = stC4;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC4, stC5, 1, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE12 = stC0;
            stE13 = stC0;
            stE23 = stC0;
            stE33 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC1, stC0, stC5, 2, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE12 = sPixel.Interpolate(stC0, stC1, stC5, 6, 1, 1);
            stE13 = sPixel.Interpolate(stC5, stC1, 5, 3);
            stE23 = sPixel.Interpolate(stC5, stC0, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          stE00 = stC2;
          stE01 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          stE32 = stC2;
          stE33 = stC2;
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC2;
            stE03 = stC2;
            stE13 = stC2;
          } else {
            stE02 = sPixel.Interpolate(stC2, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC2, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC2, stC5, 3, 1);
          }
        }
        break;
        case 151:
        case 183: {
          stE00 = stC3;
          stE01 = stC3;
          stE02 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE13 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          stE23 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          stE32 = stC3;
          stE33 = stC3;
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC3;
          } else {
            stE03 = sPixel.Interpolate(stC3, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 158: {
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 159: {
          stE02 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 191: {
          stE01 = stC4;
          stE02 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE21 = stC0;
            stE30 = stC0;
            stE31 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC3, stC0, stC7, 2, 1, 1);
            stE21 = sPixel.Interpolate(stC0, stC3, stC7, 6, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC7, stC3, 5, 3);
            stE32 = sPixel.Interpolate(stC7, stC0, 3, 1);
            stE33 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
        }
        break;
        case 201:
        case 205: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE23 = stC1;
          stE32 = stC1;
          stE33 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC1;
            stE30 = stC1;
            stE31 = stC1;
          } else {
            stE20 = sPixel.Interpolate(stC1, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC1, stC7, 3, 1);
          }
        }
        break;
        case 211: {
          stE00 = stC2;
          stE01 = stC2;
          stE02 = stC2;
          stE03 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC2;
            stE32 = stC2;
            stE33 = stC2;
          } else {
            stE23 = sPixel.Interpolate(stC2, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC2, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 215: {
          stE00 = stC3;
          stE01 = stC3;
          stE02 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE13 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC3;
            stE32 = stC3;
            stE33 = stC3;
          } else {
            stE23 = sPixel.Interpolate(stC3, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC3;
          } else {
            stE03 = sPixel.Interpolate(stC3, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 218: {
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 219: {
          stE02 = stC2;
          stE03 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE30 = stC2;
          stE31 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC2;
            stE32 = stC2;
            stE33 = stC2;
          } else {
            stE23 = sPixel.Interpolate(stC2, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC2, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
          }
        }
        break;
        case 220: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 3, 1);
            stE30 = sPixel.Interpolate(stC0, stC3, stC7, 2, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 3, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 223: {
          stE02 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE30 = stC4;
          stE31 = stC4;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC4;
            stE32 = stC4;
            stE33 = stC4;
          } else {
            stE23 = sPixel.Interpolate(stC4, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC4, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
            stE01 = stC4;
            stE10 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC4, 1, 1);
            stE10 = sPixel.Interpolate(stC3, stC4, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 233:
        case 237: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE23 = stC1;
          stE31 = stC1;
          stE32 = stC1;
          stE33 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC1;
          } else {
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
          }
        }
        break;
        case 234: {
          stE02 = stC0;
          stE03 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE32 = stC0;
          stE33 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC0;
            stE01 = stC0;
            stE10 = stC0;
          } else {
            stE00 = sPixel.Interpolate(stC0, stC1, stC3, 2, 1, 1);
            stE01 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE10 = sPixel.Interpolate(stC0, stC3, 3, 1);
          }
        }
        break;
        case 235: {
          stE02 = stC2;
          stE03 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE23 = stC2;
          stE31 = stC2;
          stE32 = stC2;
          stE33 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC2;
          } else {
            stE30 = sPixel.Interpolate(stC2, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
          }
        }
        break;
        case 239: {
          stE01 = stC4;
          stE02 = stC4;
          stE03 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          stE33 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
        }
        break;
        case 242: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC0;
            stE32 = stC0;
            stE33 = stC0;
          } else {
            stE23 = sPixel.Interpolate(stC0, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC0, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 3, 1);
            stE03 = sPixel.Interpolate(stC0, stC1, stC5, 2, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 3, 1);
          }
        }
        break;
        case 243: {
          stE00 = stC2;
          stE01 = stC2;
          stE02 = stC2;
          stE03 = stC2;
          stE10 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          if (stC7.IsNotLike(stC5)) {
            stE22 = stC2;
            stE23 = stC2;
            stE30 = stC2;
            stE31 = stC2;
            stE32 = stC2;
            stE33 = stC2;
          } else {
            stE22 = sPixel.Interpolate(stC2, stC5, stC7, 6, 1, 1);
            stE23 = sPixel.Interpolate(stC5, stC2, stC7, 2, 1, 1);
            stE30 = sPixel.Interpolate(stC2, stC7, 3, 1);
            stE31 = sPixel.Interpolate(stC7, stC2, 3, 1);
            stE32 = sPixel.Interpolate(stC7, stC5, 5, 3);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 244: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC0;
          } else {
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 245: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE23 = stC1;
          stE30 = stC1;
          stE31 = stC1;
          stE32 = stC1;
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC1;
          } else {
            stE33 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 246: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE20 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE30 = stC0;
          stE31 = stC0;
          stE32 = stC0;
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC0;
          } else {
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 247: {
          stE00 = stC3;
          stE01 = stC3;
          stE02 = stC3;
          stE10 = stC3;
          stE11 = stC3;
          stE12 = stC3;
          stE13 = stC3;
          stE20 = stC3;
          stE21 = stC3;
          stE22 = stC3;
          stE23 = stC3;
          stE30 = stC3;
          stE31 = stC3;
          stE32 = stC3;
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC3;
          } else {
            stE33 = sPixel.Interpolate(stC3, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC3;
          } else {
            stE03 = sPixel.Interpolate(stC3, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        case 249: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE31 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC1;
          } else {
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC1;
            stE32 = stC1;
            stE33 = stC1;
          } else {
            stE23 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC1, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
        }
        break;
        case 251: {
          stE02 = stC2;
          stE03 = stC2;
          stE11 = stC2;
          stE12 = stC2;
          stE13 = stC2;
          stE20 = stC2;
          stE21 = stC2;
          stE22 = stC2;
          stE31 = stC2;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC2;
          } else {
            stE30 = sPixel.Interpolate(stC2, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE23 = stC2;
            stE32 = stC2;
            stE33 = stC2;
          } else {
            stE23 = sPixel.Interpolate(stC2, stC5, 1, 1);
            stE32 = sPixel.Interpolate(stC2, stC7, 1, 1);
            stE33 = sPixel.Interpolate(stC5, stC7, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC2;
            stE01 = stC2;
            stE10 = stC2;
          } else {
            stE00 = sPixel.Interpolate(stC1, stC3, 1, 1);
            stE01 = sPixel.Interpolate(stC1, stC2, 1, 1);
            stE10 = sPixel.Interpolate(stC2, stC3, 1, 1);
          }
        }
        break;
        case 252: {
          stE00 = stC0;
          stE01 = stC0;
          stE02 = stC0;
          stE03 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE13 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE32 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC0;
          } else {
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 253: {
          stE00 = stC1;
          stE01 = stC1;
          stE02 = stC1;
          stE03 = stC1;
          stE10 = stC1;
          stE11 = stC1;
          stE12 = stC1;
          stE13 = stC1;
          stE20 = stC1;
          stE21 = stC1;
          stE22 = stC1;
          stE23 = stC1;
          stE31 = stC1;
          stE32 = stC1;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC1;
          } else {
            stE30 = sPixel.Interpolate(stC1, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC1;
          } else {
            stE33 = sPixel.Interpolate(stC1, stC5, stC7, 2, 1, 1);
          }
        }
        break;
        case 254: {
          stE00 = stC0;
          stE01 = stC0;
          stE10 = stC0;
          stE11 = stC0;
          stE12 = stC0;
          stE21 = stC0;
          stE22 = stC0;
          stE23 = stC0;
          stE32 = stC0;
          if (stC7.IsNotLike(stC3)) {
            stE20 = stC0;
            stE30 = stC0;
            stE31 = stC0;
          } else {
            stE20 = sPixel.Interpolate(stC0, stC3, 1, 1);
            stE30 = sPixel.Interpolate(stC3, stC7, 1, 1);
            stE31 = sPixel.Interpolate(stC0, stC7, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC0;
          } else {
            stE33 = sPixel.Interpolate(stC0, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE02 = stC0;
            stE03 = stC0;
            stE13 = stC0;
          } else {
            stE02 = sPixel.Interpolate(stC0, stC1, 1, 1);
            stE03 = sPixel.Interpolate(stC1, stC5, 1, 1);
            stE13 = sPixel.Interpolate(stC0, stC5, 1, 1);
          }
        }
        break;
        case 255: {
          stE01 = stC4;
          stE02 = stC4;
          stE10 = stC4;
          stE11 = stC4;
          stE12 = stC4;
          stE13 = stC4;
          stE20 = stC4;
          stE21 = stC4;
          stE22 = stC4;
          stE23 = stC4;
          stE31 = stC4;
          stE32 = stC4;
          if (stC7.IsNotLike(stC3)) {
            stE30 = stC4;
          } else {
            stE30 = sPixel.Interpolate(stC4, stC3, stC7, 2, 1, 1);
          }
          if (stC7.IsNotLike(stC5)) {
            stE33 = stC4;
          } else {
            stE33 = sPixel.Interpolate(stC4, stC5, stC7, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC3)) {
            stE00 = stC4;
          } else {
            stE00 = sPixel.Interpolate(stC4, stC1, stC3, 2, 1, 1);
          }
          if (stC1.IsNotLike(stC5)) {
            stE03 = stC4;
          } else {
            stE03 = sPixel.Interpolate(stC4, stC1, stC5, 2, 1, 1);
          }
        }
        break;
        #endregion
      }
      return (new sPixel[]{
      stE00, stE01, stE02, stE03, 
      stE10, stE11, stE12, stE13, 
      stE20, stE21, stE22, stE23, 
      stE30, stE31, stE32, stE33, 
      });
    }
    #endregion

    #endregion

  } // end class
} // end namespace
