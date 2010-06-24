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
