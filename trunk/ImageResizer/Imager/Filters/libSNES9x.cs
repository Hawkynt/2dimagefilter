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

namespace nImager.Filters {
  static class libSNES9x {
    // SNES9x's EPXB modified by Hawkynt to support thresholds
    public static void voidEPXB(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
  
      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }
    
    // SNES9x's EPX3 modified by Hawkynt to support thresholds
    public static void voidEPX3(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
          if (boolEQ13)
            stE00 = sPixel.Interpolate(stC1, stC3);
          if (boolEQ51)
            stE02 = sPixel.Interpolate(stC5, stC1);
          if (boolEQ37)
            stE20 = sPixel.Interpolate(stC3, stC7);
          if (boolEQ75)
            stE22 = sPixel.Interpolate(stC7, stC5);
        }
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
    
    // SNES9x's EPXC modified by Hawkynt to support thresholds
    public static void voidEPXC(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
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
          sPixel stC3A;
          if ((boolEQ13 && boolNQ46) && (boolEQ37 && boolNQ40))
            stC3A = sPixel.Interpolate( stC3,stC1,stC7);
          else if (boolEQ13 && boolNQ46)
            stC3A = sPixel.Interpolate( stC3, stC1);
          else if (boolEQ37 && boolNQ40)
            stC3A = sPixel.Interpolate( stC3, stC7);
          else
            stC3A = stC4;
          
          sPixel stC7B;
          if ((boolEQ37 && boolNQ48) && (boolEQ75 && boolNQ46))
            stC7B = sPixel.Interpolate(stC7,stC3,stC5);
          else if (boolEQ37 && boolNQ48)
            stC7B = sPixel.Interpolate(stC7,stC3);
          else if (boolEQ75 && boolNQ46)
            stC7B = sPixel.Interpolate(stC7,stC5);
          else
            stC7B = stC4;

          sPixel stC5C;
          if ((boolEQ75 && boolNQ42) && (boolEQ51 && boolNQ48))
            stC5C = sPixel.Interpolate( stC5,stC1,stC7);
          else if (boolEQ75 && boolNQ42)
            stC5C = sPixel.Interpolate( stC5,stC7);
          else if (boolEQ51 && boolNQ48)
            stC5C = sPixel.Interpolate( stC5,stC1);
          else
            stC5C = stC4;

          sPixel stC1D;
          
          if ((boolEQ51 && boolNQ40) && (boolEQ13 && boolNQ42))
            stC1D = sPixel.Interpolate(stC1,stC3,stC5);
          else if (boolEQ51 && boolNQ40) 
            stC1D = sPixel.Interpolate(stC1,stC5);
          else if (boolEQ13 && boolNQ42)
            stC1D = sPixel.Interpolate(stC1,stC3);
          else
            stC1D = stC4;
      
          if (boolEQ13)
            stE00 = sPixel.Interpolate(stC1, stC3);
          if (boolEQ51)
            stE01 = sPixel.Interpolate(stC5, stC1);
          if (boolEQ37)
            stE10 = sPixel.Interpolate(stC3, stC7);
          if (boolEQ75)
            stE11 = sPixel.Interpolate(stC7, stC5);

          stE00 = sPixel.Interpolate(stE00, stC1D, stC3A, stC4, 5, 1, 1, 1);
          stE01 = sPixel.Interpolate(stE01, stC7B, stC5C, stC4, 5, 1, 1, 1);
          stE10 = sPixel.Interpolate(stE10, stC3A, stC7B, stC4, 5, 1, 1, 1);
          stE11 = sPixel.Interpolate(stE11, stC5C, stC1D, stC4, 5, 1, 1, 1);
        
        } else {
          
          if (boolEQ13)
            stE00 = sPixel.Interpolate(stC1, stC3);
          if (boolEQ51)
            stE01 = sPixel.Interpolate(stC5, stC1);
          if (boolEQ37)
            stE10 = sPixel.Interpolate(stC3, stC7);
          if (boolEQ75)
            stE11 = sPixel.Interpolate(stC7, stC5);

          stE00 = sPixel.Interpolate(stC4, stE00, 3, 1);
          stE01 = sPixel.Interpolate(stC4, stE01, 3, 1);
          stE10 = sPixel.Interpolate(stC4, stE10, 3, 1);
          stE11 = sPixel.Interpolate(stC4, stE11, 3, 1);
           
       }
      }

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }
    

  } // end class
} // end namespace
