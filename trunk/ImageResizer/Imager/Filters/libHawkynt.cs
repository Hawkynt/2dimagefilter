#region (c)2010-2011 Hawkynt
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

namespace nImager.Filters {
  static class libHawkynt {
    // just a bad TV effect
    public static void Tv2x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var pixel = sourceImage[srcX, srcY];
      var luminance = pixel.Luminance;
      targetImage[tgtX + 0, tgtY + 0] = new sPixel(pixel.Red, 0, 0);
      targetImage[tgtX + 1, tgtY + 0] = new sPixel(0, pixel.Green, 0);
      targetImage[tgtX + 0, tgtY + 1] = new sPixel(0, 0, pixel.Blue);
      targetImage[tgtX + 1, tgtY + 1] = sPixel.FromGrey(luminance);
    }
    // another bad one
    public static void Tv3x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var pixel = sourceImage[srcX, srcY];
      var ap = (sbyte)(1 - ((srcX & 1) << 1));
      targetImage[tgtX + 0, tgtY + 0] = new sPixel(pixel.Red, 0, 0);
      targetImage[tgtX + 1, tgtY + 0] = new sPixel(0, pixel.Green, 0);
      targetImage[tgtX + 2, tgtY + 0] = new sPixel(0, 0, pixel.Blue);
      targetImage[tgtX + 0, tgtY + 1] = sPixel.Black;
      targetImage[tgtX + 1, tgtY + 1] = sPixel.Black;
      targetImage[tgtX + 2, tgtY + 1] = sPixel.Black;
      targetImage[tgtX + 0, tgtY - ap] = new sPixel(pixel.Red, 0, 0);
      targetImage[tgtX + 1, tgtY + ap] = new sPixel(0, pixel.Green, 0);
      targetImage[tgtX + 2, tgtY - ap] = new sPixel(0, 0, pixel.Blue);
    }

  }
}
