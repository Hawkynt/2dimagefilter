#region (c)2010 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
  static class libKreed {
    // used for 2xSaI, Super Eagle, Super 2xSaI
    // using thresholds when comparing (Hawkynt)
    private static int _intConc2d(sPixel stColA, sPixel stColB, sPixel stColC, sPixel stColD) {
      int intRet = 0;

      bool boolAC = stColA .IsLike(stColC);
      int intX = boolAC ? 1 : 0;
      int intY = (stColB.IsLike(stColC) && !(boolAC)) ? 1 : 0;

      bool boolAD = stColA .IsLike(stColD);
      intX += boolAD ? 1 : 0;
      intY += (stColB.IsLike(stColD) && !(boolAD)) ? 1 : 0;

      if (intX <= 1)
        intRet++;
      if (intY <= 1)
        intRet--;

      return (intRet);
    }

    // TODO: to be really exact, the comparisons are not that right by comparing to already interpolated values
    // TODO: when interpolating 3 or more points I'm using already calculated interpolations and weight them further
    //       which is not the mathematically correct approach, but it's enough - at least for now
    // Kreed's SuperEagle modified by Hawkynt to allow thresholds
    public static void voidSuperEagle(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      sPixel stD0 = objSrc[intSrcX - 1, intSrcY + 2];
      sPixel stD1 = objSrc[intSrcX, intSrcY + 2];
      sPixel stD2 = objSrc[intSrcX + 1, intSrcY + 2];
      sPixel stD3 = objSrc[intSrcX + 2, intSrcY - 1];
      sPixel stD4 = objSrc[intSrcX + 2, intSrcY];
      sPixel stD5 = objSrc[intSrcX + 2, intSrcY + 1];

      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;
      if (stC4.IsLike(stC8)) {
        sPixel stC48 = sPixel.Interpolate(stC4, stC8);
        if (stC7.IsLike(stC5)) {
          sPixel stC57 = sPixel.Interpolate(stC5, stC7);
          int intR = 0;
          intR += _intConc2d(stC57, stC48, stC6, stD1);
          intR += _intConc2d(stC57, stC48, stC3, stC1);
          intR += _intConc2d(stC57, stC48, stD2, stD5);
          intR += _intConc2d(stC57, stC48, stC2, stD4);

          if (intR > 0) {
            stE10 = stC57;
            stE01 = stC57;
            stE11 = sPixel.Interpolate(stC48, stC57);
            stE00 = sPixel.Interpolate(stC48, stC57);
          } else if (intR < 0) {
            stE10 = sPixel.Interpolate(stC48, stC57);
            stE01 = sPixel.Interpolate(stC48, stC57);
          } else {
            stE10 = stC57;
            stE01 = stC57;
          }
        } else {
          if (stC48.IsLike(stC1) && stC48.IsLike(stD5))
            stE01 = sPixel.Interpolate(sPixel.Interpolate(stC48, stC1, stD5), stC5, 3, 1);
          else if (stC48.IsLike(stC1))
            stE01 = sPixel.Interpolate(sPixel.Interpolate(stC48, stC1), stC5, 3, 1);
          else if (stC48.IsLike(stD5))
            stE01 = sPixel.Interpolate(sPixel.Interpolate(stC48, stD5), stC5, 3, 1);
          else
            stE01 = sPixel.Interpolate(stC48, stC5);
          
          if (stC48 .IsLike(stD2) && stC48.IsLike(stC3))
            stE10 = sPixel.Interpolate(sPixel.Interpolate(stC48,stD2,stC3), stC7, 3, 1);
          else if (stC48 .IsLike(stD2))
            stE10 = sPixel.Interpolate(sPixel.Interpolate(stC48,stD2), stC7, 3, 1);
          else if (stC48.IsLike(stC3))
            stE10 = sPixel.Interpolate(sPixel.Interpolate(stC48,stC3), stC7, 3, 1);
          else
            stE10 = sPixel.Interpolate(stC48, stC7);

        }
      } else {
        if (stC7.IsLike(stC5)) {
          sPixel stC57 = sPixel.Interpolate(stC5, stC7);
          stE01 = stC57;
          stE10 = stC57;
          
          if (stC57.IsLike (stC6) && stC57.IsLike(stC2))
            stE00 = sPixel.Interpolate(sPixel.Interpolate(stC57,stC6,stC2), stC4, 3, 1);
          else if (stC57.IsLike(stC6))
            stE00 = sPixel.Interpolate(sPixel.Interpolate(stC57,stC6), stC4, 3, 1);
          else if (stC57.IsLike(stC2))
            stE00 = sPixel.Interpolate(sPixel.Interpolate(stC57,stC2), stC4, 3, 1);
          else
            stE00 = sPixel.Interpolate(stC57, stC4);

          if (stC57.IsLike(stD4) && stC57.IsLike(stD1))
            stE11 = sPixel.Interpolate(sPixel.Interpolate(stC57,stD4,stD1), stC8, 3, 1);
          else if (stC57.IsLike(stD4))
            stE11 = sPixel.Interpolate(sPixel.Interpolate(stC57,stD4), stC8, 3, 1);
          else if (stC57.IsLike(stD1))
            stE11 = sPixel.Interpolate(sPixel.Interpolate(stC57,stD1), stC8, 3, 1);
          else
            stE11 = sPixel.Interpolate(stC57, stC8);

        } else {
          stE11 = sPixel.Interpolate(stC8, stC7, stC5, 6, 1, 1);
          stE00 = sPixel.Interpolate(stC4, stC7, stC5, 6, 1, 1);
          stE10 = sPixel.Interpolate(stC7, stC4, stC8, 6, 1, 1);
          stE01 = sPixel.Interpolate(stC5, stC4, stC8, 6, 1, 1);
        }
      }

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }

    // Derek Liauw Kie Fa's 2XSaI
    public static void voidSaI2X(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      sPixel stD0 = objSrc[intSrcX - 1, intSrcY + 2];
      sPixel stD1 = objSrc[intSrcX, intSrcY + 2];
      sPixel stD2 = objSrc[intSrcX + 1, intSrcY + 2];
      sPixel stD3 = objSrc[intSrcX + 2, intSrcY - 1];
      sPixel stD4 = objSrc[intSrcX + 2, intSrcY];
      sPixel stD5 = objSrc[intSrcX + 2, intSrcY + 1];

      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;

      if (stC4 .IsLike(stC8) && stC5.IsNotLike(stC7)) {
        sPixel stC48 = sPixel.Interpolate(stC4, stC8);
        if ((stC48.IsLike(stC1) && stC5.IsLike(stD5)) || (stC48.IsLike(stC7) && stC48.IsLike(stC2) && stC5.IsNotLike(stC1) && stC5.IsLike(stD3))) {
          //nothing
        } else {
          stE01 = sPixel.Interpolate(stC48, stC5);
        }

        if ((stC48.IsLike(stC3) && stC7.IsLike(stD2)) || (stC48.IsLike(stC5) && stC48.IsLike(stC6) && stC3.IsNotLike(stC7) && stC7.IsLike(stD0))) {
          //nothing
        } else {
          stE10 = sPixel.Interpolate(stC48, stC7);
        }
      } else if (stC5.IsLike(stC7) && stC4.IsNotLike(stC8)) {
        sPixel stC57 = sPixel.Interpolate(stC5, stC7);
        if ((stC57.IsLike(stC2) && stC4.IsLike(stC6)) || (stC57.IsLike(stC1) && stC57.IsLike(stC8) && stC4.IsNotLike(stC2) && stC4.IsLike(stC0))) {
          stE01 = stC57;
        } else {
          stE01 = sPixel.Interpolate(stC4, stC57);
        }

        if ((stC57.IsLike(stC6) && stC4.IsLike(stC2)) || (stC57.IsLike(stC3) && stC57.IsLike(stC8) && stC4.IsNotLike(stC6) && stC4.IsLike(stC0))) {
          stE10 = stC57;
        } else {
          stE10 = sPixel.Interpolate(stC4, stC57);
        }
        stE11 = stC57;
      } else if (stC4.IsLike(stC8) && stC5.IsLike(stC7)) {
        sPixel stC48 = sPixel.Interpolate(stC4, stC8);
        sPixel stC57 = sPixel.Interpolate(stC5, stC7);
        if (stC48.IsNotLike(stC57)) {
          int intR = 0;
          intR += _intConc2d(stC48, stC57, stC3, stC1);
          intR -= _intConc2d(stC57, stC48, stD4, stC2);
          intR -= _intConc2d(stC57, stC48, stC6, stD1);
          intR += _intConc2d(stC48, stC57, stD5, stD2);

          if (intR < 0) {
            stE11 = stC57;
          } else if (intR == 0) {
            stE11 = sPixel.Interpolate(stC48, stC57);
          }
          stE10 = sPixel.Interpolate(stC48, stC57);
          stE01 = sPixel.Interpolate(stC48, stC57);
        }
      } else {
        stE11 = sPixel.Interpolate(stC4, stC5, stC7, stC8);

        if (stC4.IsLike(stC7) && stC4.IsLike(stC2) && stC5.IsNotLike(stC1) && stC5.IsLike(stD3)) {
          //nothing
        } else if (stC5.IsLike(stC1) && stC5.IsLike(stC8) && stC4.IsNotLike(stC2) && stC4.IsLike(stC0)) {
          stE01 = sPixel.Interpolate(stC5, stC1, stC8);
        } else {
          stE01 = sPixel.Interpolate(stC4, stC5);
        }

        if (stC4.IsLike(stC5) && stC4.IsLike(stC6) && stC3.IsNotLike(stC7) && stC7.IsLike(stD0)) {
          //nothing
        } else if (stC7.IsLike(stC3) && stC7.IsLike(stC8) && stC4.IsNotLike(stC6) && stC4.IsLike(stC0)) {
          stE10 = sPixel.Interpolate(stC7, stC3, stC8);
        } else {
          stE10 = sPixel.Interpolate(stC4, stC7);
        }
      }

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }

    // Kreed's SuperSaI
    public static void voidSuperSaI(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
      sPixel stD0 = objSrc[intSrcX - 1, intSrcY + 2];
      sPixel stD1 = objSrc[intSrcX, intSrcY + 2];
      sPixel stD2 = objSrc[intSrcX + 1, intSrcY + 2];
      sPixel stD3 = objSrc[intSrcX + 2, intSrcY - 1];
      sPixel stD4 = objSrc[intSrcX + 2, intSrcY];
      sPixel stD5 = objSrc[intSrcX + 2, intSrcY + 1];
      sPixel stD6 = objSrc[intSrcX + 2, intSrcY + 2];

      sPixel stE00 = stC4;
      sPixel stE01 = stC4;
      sPixel stE10 = stC4;
      sPixel stE11 = stC4;

      if (stC7.IsLike(stC5) && stC4.IsNotLike(stC8)) {
        sPixel stC57=sPixel.Interpolate(stC7, stC5);
        stE11 = stC57;
        stE01 = stC57;
      } else if (stC4.IsLike(stC8) && stC7.IsNotLike(stC5)) {
        //nothing
      } else if (stC4.IsLike(stC8) && stC7.IsLike(stC5)) {
        sPixel stC57 = sPixel.Interpolate(stC7, stC5);
        sPixel stC48 = sPixel.Interpolate(stC4, stC8);
        int intR = 0;
        intR += _intConc2d(stC57, stC48, stC6, stD1);
        intR += _intConc2d(stC57, stC48, stC3, stC1);
        intR += _intConc2d(stC57, stC48, stD2, stD5);
        intR += _intConc2d(stC57, stC48, stC2, stD4);

        if (intR > 0) {
          stE11 = stC57;
          stE01 = stC57;
        } else if (intR == 0) {
          stE11 = sPixel.Interpolate(stC48, stC57);
          stE01 = sPixel.Interpolate(stC48, stC57);
        }
      } else {
        if (stC8.IsLike(stC5) && stC8.IsLike(stD1) && stC7.IsNotLike(stD2) && stC8.IsNotLike(stD0)) {
          stE11 = sPixel.Interpolate(sPixel.Interpolate(stC8,stC5,stD1), stC7, 3, 1);
        } else if (stC7.IsLike(stC4) && stC7.IsLike(stD2) && stC7.IsNotLike(stD6) && stC8.IsNotLike(stD1)) {
          stE11 = sPixel.Interpolate(sPixel.Interpolate(stC7,stC4,stD2), stC8, 3, 1);
        } else {
          stE11 = sPixel.Interpolate(stC7, stC8);
        }
        if (stC5.IsLike(stC8) && stC5.IsLike(stC1) && stC5.IsNotLike(stC0) && stC4.IsNotLike(stC2)) {
          stE01 = sPixel.Interpolate(sPixel.Interpolate(stC5,stC8,stC1), stC4, 3, 1);
        } else if (stC4.IsLike(stC7) && stC4.IsLike(stC2) && stC5.IsNotLike(stC1) && stC4.IsNotLike(stD3)) {
          stE01 = sPixel.Interpolate(sPixel.Interpolate(stC4,stC7,stC2), stC5, 3, 1);
        } else {
          stE01 = sPixel.Interpolate(stC4, stC5);
        }
      }
      if (stC4.IsLike(stC8) && stC4.IsLike(stC3) && stC7.IsNotLike(stC5) && stC4.IsNotLike(stD2)) {
        stE10 = sPixel.Interpolate(stC7, sPixel.Interpolate(stC4,stC8,stC3));
      } else if (stC4.IsLike (stC6) && stC4.IsLike(stC5) && stC7.IsNotLike(stC3) && stC4.IsNotLike(stD0)) {
        stE10 = sPixel.Interpolate(stC7, sPixel.Interpolate(stC4,stC6,stC5));
      } else {
        stE10 = stC7;
      }
      
      if (stC7.IsLike(stC5) && stC7.IsLike(stC6) && stC4 .IsNotLike(stC8) && stC7 .IsNotLike(stC2)) {
        stE00 = sPixel.Interpolate(sPixel.Interpolate(stC7,stC5,stC6), stC4);
      } else if (stC7.IsLike(stC3) && stC7.IsLike(stC8) && stC4.IsNotLike(stC6) && stC7.IsNotLike(stC0)) {
        stE00 = sPixel.Interpolate(sPixel.Interpolate(stC7,stC3,stC8), stC4);
      }

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }


  } // end class
} // end namespace
