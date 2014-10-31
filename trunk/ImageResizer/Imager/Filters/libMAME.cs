#region (c)2008-2015 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2015 Hawkynt

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
 */
#endregion

namespace Imager.Filters {
  internal static class libMAME {
    private const float _gamma58 = 5f / 8f;
    private const float _gamma516 = 5f / 16f;

    /// <summary>
    /// MAME's TV effect in 2x
    /// </summary>
    public static void Tv2x(PixelWorker<sPixel> worker) {
      var pixel = worker.SourceP0P0();
      var subPixel = pixel * _gamma58;
      worker.TargetP0P0(pixel);
      worker.TargetP1P0(pixel);
      worker.TargetP0P1(subPixel);
      worker.TargetP1P1(subPixel);
    }

    /// <summary>
    /// MAME's TV effect 3x
    /// </summary>
    public static void Tv3x(PixelWorker<sPixel> worker) {
      var pixel = worker.SourceP0P0();
      var subPixel = pixel * _gamma58;
      var subPixel2 = pixel * _gamma516;
      worker.TargetP0P0(pixel);
      worker.TargetP1P0(pixel);
      worker.TargetP2P0(pixel);
      worker.TargetP0P1(subPixel);
      worker.TargetP1P1(subPixel);
      worker.TargetP2P1(subPixel);
      worker.TargetP0P2(subPixel2);
      worker.TargetP1P2(subPixel2);
      worker.TargetP2P2(subPixel2);
    }

    /// <summary>
    /// MAME's RGB 2x
    /// </summary>
    public static void Rgb2x(PixelWorker<sPixel> worker) {
      var pixel = worker.SourceP0P0();
      worker.TargetP0P0(new sPixel(pixel.Red, 0, 0, pixel.Alpha));
      worker.TargetP1P0(new sPixel(0, pixel.Green, 0, pixel.Alpha));
      worker.TargetP0P1(new sPixel(0, 0, pixel.Blue, pixel.Alpha));
      worker.TargetP1P1(pixel);
    }

    /// <summary>
    /// MAME's RGB 3x
    /// </summary>
    public static void Rgb3x(PixelWorker<sPixel> worker) {
      var pixel = worker.SourceP0P0();
      worker.TargetP0P0(pixel);
      worker.TargetP1P0(new sPixel(0, pixel.Green, 0, pixel.Alpha));
      worker.TargetP2P0(new sPixel(0, 0, pixel.Blue, pixel.Alpha));
      worker.TargetP0P1(new sPixel(0, 0, pixel.Blue, pixel.Alpha));
      worker.TargetP1P1(pixel);
      worker.TargetP2P1(new sPixel(pixel.Red, 0, 0, pixel.Alpha));
      worker.TargetP0P2(new sPixel(pixel.Red, 0, 0, pixel.Alpha));
      worker.TargetP1P2(new sPixel(0, pixel.Green, 0, pixel.Alpha));
      worker.TargetP2P2(pixel);
    }

    /// <summary>
    /// MAME's AdvInterp2x, very similar to Scale2x but uses interpolation, modified by Hawkynt to support thresholds
    /// </summary>
    public static void AdvInterp2x(PixelWorker<sPixel> worker) {
      var c1 = worker.SourceP0M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var c7 = worker.SourceP0P1();
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
      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }

    /// <summary>
    /// MAME's AdvInterp3x, very similar to Scale3x but uses interpolation, modified by Hawkynt to support thresholds
    /// </summary>
    public static void AdvInterp3x(PixelWorker<sPixel> worker) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
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
      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP2P0(e02);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
      worker.TargetP2P1(e12);
      worker.TargetP0P2(e20);
      worker.TargetP1P2(e21);
      worker.TargetP2P2(e22);
    }

    /// <summary>
    /// Andrea Mazzoleni's Scale2X modified by Hawkynt to support thresholds
    /// </summary>
    public static void Scale2x(PixelWorker<sPixel> worker) {
      var c1 = worker.SourceP0M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var c7 = worker.SourceP0P1();
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
      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }

    /// <summary>
    /// Andrea Mazzoleni's Scale3X modified by Hawkynt to support thresholds
    /// </summary>
    public static void Scale3x(PixelWorker<sPixel> worker) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
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
      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP2P0(e02);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
      worker.TargetP2P1(e12);
      worker.TargetP0P2(e20);
      worker.TargetP1P2(e21);
      worker.TargetP2P2(e22);
    }


  } // end class
} // end namespace
