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
  static class libVBA {
    // Bilinear Plus Original
    public static void voidBilinearPlusOriginal(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stA = objSrc[intSrcX + 0, intSrcY + 0];
      sPixel stB = objSrc[intSrcX + 1, intSrcY + 0];
      sPixel stC = objSrc[intSrcX + 0, intSrcY + 1];
      sPixel stD = objSrc[intSrcX + 1, intSrcY + 1];

      sPixel stE00 = sPixel.Interpolate(stA, stB, stC, 5, 2, 1);
      sPixel stE01 = sPixel.Interpolate(stA, stB);
      sPixel stE10 = sPixel.Interpolate(stA, stC);
      sPixel stE11 = sPixel.Interpolate(stA, stB, stC, stD);

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }
    // Bilinear Plus
    public static void voidBilinearPlus(cImage objSrc, int intSrcX, int intSrcY, cImage objTgt, int intTgtX, int intTgtY, byte byteScaleX, byte byteScaleY, object objParam) {
      sPixel stA = objSrc[intSrcX + 0, intSrcY + 0];
      sPixel stB = objSrc[intSrcX + 1, intSrcY + 0];
      sPixel stC = objSrc[intSrcX + 0, intSrcY + 1];
      sPixel stD = objSrc[intSrcX + 1, intSrcY + 1];

      sPixel stE00 = sPixel.Interpolate(stA, stB, stC, 10, 2, 2)*(14f/16f);
      sPixel stE01 = sPixel.Interpolate(stA, stB);
      sPixel stE10 = sPixel.Interpolate(stA, stC);
      sPixel stE11 = sPixel.Interpolate(stA, stB, stC, stD);

      objTgt[intTgtX + 0, intTgtY + 0] = stE00;
      objTgt[intTgtX + 1, intTgtY + 0] = stE01;
      objTgt[intTgtX + 0, intTgtY + 1] = stE10;
      objTgt[intTgtX + 1, intTgtY + 1] = stE11;
    }
  } // end class
} // end namespace
