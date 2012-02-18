#region (c)2008-2014 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010-2011 Hawkynt

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

namespace Imager.Filters {
  static class libVBA {
    /// <summary>
    /// Bilinear Plus Original
    /// </summary>
    public static void BilinearPlusOriginal(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var c00 = sourceImage[srcX + 0, srcY + 0];
      var c01 = sourceImage[srcX + 1, srcY + 0];
      var c10 = sourceImage[srcX + 0, srcY + 1];
      var c11 = sourceImage[srcX + 1, srcY + 1];

      var e00 = sPixel.Interpolate(c00, c01, c10, 5, 2, 1);
      var e01 = sPixel.Interpolate(c00, c01);
      var e10 = sPixel.Interpolate(c00, c10);
      var e11 = sPixel.Interpolate(c00, c01, c10, c11);

      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
    }

    /// <summary>
    /// Bilinear Plus
    /// </summary>
    public static void BilinearPlus(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var c00 = sourceImage[srcX + 0, srcY + 0];
      var c01 = sourceImage[srcX + 1, srcY + 0];
      var c10 = sourceImage[srcX + 0, srcY + 1];
      var c11 = sourceImage[srcX + 1, srcY + 1];

      const float gamma = 14f / 16f;

      var e00 = sPixel.Interpolate(c00, c01, c10, 10, 2, 2) * gamma;
      var e01 = sPixel.Interpolate(c00, c01);
      var e10 = sPixel.Interpolate(c00, c10);
      var e11 = sPixel.Interpolate(c00, c01, c10, c11);

      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
    }
  } // end class
} // end namespace
