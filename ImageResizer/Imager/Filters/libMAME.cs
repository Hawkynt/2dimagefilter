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
  static class libMAME {
    private const float _gamma58 = 5f / 8f;
    private const float _gamma516 = 5f / 16f;

    /// <summary>
    /// MAME's TV effect in 2x
    /// </summary>
    public static void Tv2x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var pixel = sourceImage[srcX, srcY];
      var subPixel = pixel * _gamma58;
      targetImage[tgtX + 0, tgtY + 0] = pixel;
      targetImage[tgtX + 1, tgtY + 0] = pixel;
      targetImage[tgtX + 0, tgtY + 1] = subPixel;
      targetImage[tgtX + 1, tgtY + 1] = subPixel;
    }

    /// <summary>
    /// MAME's TV effect 3x
    /// </summary>
    public static void Tv3x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var pixel = sourceImage[srcX, srcY];
      var subPixel = pixel * _gamma58;
      var subPixel2 = pixel * _gamma516;
      targetImage[tgtX + 0, tgtY + 0] = pixel;
      targetImage[tgtX + 1, tgtY + 0] = pixel;
      targetImage[tgtX + 2, tgtY + 0] = pixel;
      targetImage[tgtX + 0, tgtY + 1] = subPixel;
      targetImage[tgtX + 1, tgtY + 1] = subPixel;
      targetImage[tgtX + 2, tgtY + 1] = subPixel;
      targetImage[tgtX + 0, tgtY + 2] = subPixel2;
      targetImage[tgtX + 1, tgtY + 2] = subPixel2;
      targetImage[tgtX + 2, tgtY + 2] = subPixel2;
    }

    /// <summary>
    /// MAME's RGB 2x
    /// </summary>
    public static void Rgb2x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var pixel = sourceImage[srcX, srcY];
      targetImage[tgtX + 0, tgtY + 0] = new sPixel(pixel.Red, 0, 0, pixel.Alpha);
      targetImage[tgtX + 1, tgtY + 0] = new sPixel(0, pixel.Green, 0, pixel.Alpha);
      targetImage[tgtX + 0, tgtY + 1] = new sPixel(0, 0, pixel.Blue, pixel.Alpha);
      targetImage[tgtX + 1, tgtY + 1] = pixel;
    }

    /// <summary>
    /// MAME's RGB 3x
    /// </summary>
    public static void Rgb3x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var pixel = sourceImage[srcX, srcY];
      targetImage[tgtX + 0, tgtY + 0] = pixel;
      targetImage[tgtX + 1, tgtY + 0] = new sPixel(0, pixel.Green, 0, pixel.Alpha);
      targetImage[tgtX + 2, tgtY + 0] = new sPixel(0, 0, pixel.Blue, pixel.Alpha);
      targetImage[tgtX + 0, tgtY + 1] = new sPixel(0, 0, pixel.Blue, pixel.Alpha);
      targetImage[tgtX + 1, tgtY + 1] = pixel;
      targetImage[tgtX + 2, tgtY + 1] = new sPixel(pixel.Red, 0, 0, pixel.Alpha);
      targetImage[tgtX + 0, tgtY + 2] = new sPixel(pixel.Red, 0, 0, pixel.Alpha);
      targetImage[tgtX + 1, tgtY + 2] = new sPixel(0, pixel.Green, 0, pixel.Alpha);
      targetImage[tgtX + 2, tgtY + 2] = pixel;
    }

    /// <summary>
    /// MAME's AdvInterp2x, very similar to Scale2x but uses interpolation, modified by Hawkynt to support thresholds
    /// </summary>
    public static void AdvInterp2x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;
      if (c1.IsNotLike(c7) && c3.IsNotLike(c5)) {
        if (c3.IsLike(c1))
          e00 = sPixel.Interpolate(sPixel.Interpolate(c1, c3), c4, 5, 3);
        if (c5.IsLike(c1))
          e01 = sPixel.Interpolate(sPixel.Interpolate(c1, c5), c4, 5, 3);
        if (c3.IsLike(c7))
          e10 = sPixel.Interpolate(sPixel.Interpolate(c7, c3), c4, 5, 3);
        if (c5.IsLike(c7))
          e11 = sPixel.Interpolate(sPixel.Interpolate(c7, c5), c4, 5, 3);
      }
      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
    }

    /// <summary>
    /// MAME's AdvInterp3x, very similar to Scale3x but uses interpolation, modified by Hawkynt to support thresholds
    /// </summary>
    public static void AdvInterp3x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var c0 = sourceImage[srcX - 1, srcY - 1];
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c2 = sourceImage[srcX + 1, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c6 = sourceImage[srcX - 1, srcY + 1];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      var c8 = sourceImage[srcX + 1, srcY + 1];
      sPixel e01, e02, e10, e11, e12, e20, e21, e22;
      var e00 = e01 = e02 = e10 = e11 = e12 = e20 = e21 = e22 = c4;
      if (c1.IsNotLike(c7) && c3.IsNotLike(c5)) {
        if (c3.IsLike(c1)) {
          e00 = sPixel.Interpolate(sPixel.Interpolate(c3, c1), c4, 5, 3);
        }
        if (c1.IsLike(c5)) {
          e02 = sPixel.Interpolate(sPixel.Interpolate(c5, c1), c4, 5, 3);
        }
        if (c3.IsLike(c7)) {
          e20 = sPixel.Interpolate(sPixel.Interpolate(c3, c7), c4, 5, 3);
        }
        if (c7.IsLike(c5)) {
          e22 = sPixel.Interpolate(sPixel.Interpolate(c7, c5), c4, 5, 3);
        }

        if (
          (c3.IsLike(c1) && c4.IsNotLike(c2)) &&
          (c5.IsLike(c1) && c4.IsNotLike(c0))
        )
          e01 = sPixel.Interpolate(c1, c3, c5);
        else if (c3.IsLike(c1) && c4.IsNotLike(c2))
          e01 = sPixel.Interpolate(c3, c1);
        else if (c5.IsLike(c1) && c4.IsNotLike(c0))
          e01 = sPixel.Interpolate(c5, c1);

        if (
          (c3.IsLike(c1) && c4.IsNotLike(c6)) &&
          (c3.IsLike(c7) && c4.IsNotLike(c0))
        )
          e10 = sPixel.Interpolate(c3, c1, c7);
        else if (c3.IsLike(c1) && c4.IsNotLike(c6))
          e10 = sPixel.Interpolate(c3, c1);
        else if (c3.IsLike(c7) && c4.IsNotLike(c0))
          e10 = sPixel.Interpolate(c3, c7);

        if (
          (c5.IsLike(c1) && c4.IsNotLike(c8)) &&
          (c5.IsLike(c7) && c4.IsNotLike(c2))
        )
          e12 = sPixel.Interpolate(c5, c1, c7);
        else if (c5.IsLike(c1) && c4.IsNotLike(c8))
          e12 = sPixel.Interpolate(c5, c1);
        else if (c5.IsLike(c7) && c4.IsNotLike(c2))
          e12 = sPixel.Interpolate(c5, c7);

        if (
          (c3.IsLike(c7) && c4.IsNotLike(c8)) &&
          (c5.IsLike(c7) && c4.IsNotLike(c6))
        )
          e21 = sPixel.Interpolate(c7, c3, c5);
        else if (c3.IsLike(c7) && c4.IsNotLike(c8))
          e21 = sPixel.Interpolate(c3, c7);
        else if (c5.IsLike(c7) && c4.IsNotLike(c6))
          e21 = sPixel.Interpolate(c5, c7);
      }
      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 2, tgtY + 0] = e02;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
      targetImage[tgtX + 2, tgtY + 1] = e12;
      targetImage[tgtX + 0, tgtY + 2] = e20;
      targetImage[tgtX + 1, tgtY + 2] = e21;
      targetImage[tgtX + 2, tgtY + 2] = e22;
    }

    /// <summary>
    /// Andrea Mazzoleni's Scale2X modified by Hawkynt to support thresholds
    /// </summary>
    public static void Scale2x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;
      if (c3.IsNotLike(c5) && c1.IsNotLike(c7)) {
        if (c1.IsLike(c3)) {
          e00 = sPixel.Interpolate(c1, c3);
        }
        if (c1.IsLike(c5)) {
          e01 = sPixel.Interpolate(c1, c5);
        }
        if (c7.IsLike(c3)) {
          e10 = sPixel.Interpolate(c7, c3);
        }
        if (c7.IsLike(c5)) {
          e11 = sPixel.Interpolate(c7, c5);
        }
      }
      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
    }

    /// <summary>
    /// Andrea Mazzoleni's Scale3X modified by Hawkynt to support thresholds
    /// </summary>
    public static void Scale3x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
      var c0 = sourceImage[srcX - 1, srcY - 1];
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c2 = sourceImage[srcX + 1, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c6 = sourceImage[srcX - 1, srcY + 1];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      var c8 = sourceImage[srcX + 1, srcY + 1];
      sPixel e01, e02, e10, e11, e12, e20, e21, e22 = c4;
      var e00 = e01 = e02 = e10 = e11 = e12 = e20 = e21 = e22 = c4;
      if (c1.IsNotLike(c7) && c3.IsNotLike(c5)) {
        if (c3.IsLike(c1))
          e00 = sPixel.Interpolate(c3, c1);
        if (c1.IsLike(c5))
          e02 = sPixel.Interpolate(c1, c5);
        if (c3.IsLike(c7))
          e20 = sPixel.Interpolate(c3, c7);
        if (c7.IsLike(c5))
          e22 = sPixel.Interpolate(c7, c5);

        if (
          (c3.IsLike(c1) && c4.IsNotLike(c2)) &&
          (c5.IsLike(c1) && c4.IsNotLike(c0))
        )
          e01 = sPixel.Interpolate(c1, c3, c5);
        else if (c3.IsLike(c1) && c4.IsNotLike(c2))
          e01 = sPixel.Interpolate(c3, c1);
        else if (c5.IsLike(c1) && c4.IsNotLike(c0))
          e01 = sPixel.Interpolate(c5, c1);

        if (
          (c3.IsLike(c1) && c4.IsNotLike(c6)) &&
          (c3.IsLike(c7) && c4.IsNotLike(c0))
        )
          e10 = sPixel.Interpolate(c3, c1, c7);
        else if (c3.IsLike(c1) && c4.IsNotLike(c6))
          e10 = sPixel.Interpolate(c3, c1);
        else if (c3.IsLike(c7) && c4.IsNotLike(c0))
          e10 = sPixel.Interpolate(c3, c7);

        if (
          (c5.IsLike(c1) && c4.IsNotLike(c8)) &&
          (c5.IsLike(c7) && c4.IsNotLike(c2))
        )
          e12 = sPixel.Interpolate(c5, c1, c7);
        else if (c5.IsLike(c1) && c4.IsNotLike(c8))
          e12 = sPixel.Interpolate(c5, c1);
        else if (c5.IsLike(c7) && c4.IsNotLike(c2))
          e12 = sPixel.Interpolate(c5, c7);

        if (
          (c3.IsLike(c7) && c4.IsNotLike(c8)) &&
          (c5.IsLike(c7) && c4.IsNotLike(c6))
        )
          e21 = sPixel.Interpolate(c7, c3, c5);
        else if (c3.IsLike(c7) && c4.IsNotLike(c8))
          e21 = sPixel.Interpolate(c3, c7);
        else if (c5.IsLike(c7) && c4.IsNotLike(c6))
          e21 = sPixel.Interpolate(c5, c7);

      }
      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 2, tgtY + 0] = e02;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
      targetImage[tgtX + 2, tgtY + 1] = e12;
      targetImage[tgtX + 0, tgtY + 2] = e20;
      targetImage[tgtX + 1, tgtY + 2] = e21;
      targetImage[tgtX + 2, tgtY + 2] = e22;
    }


  } // end class
} // end namespace
