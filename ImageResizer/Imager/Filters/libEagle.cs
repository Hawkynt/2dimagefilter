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
  internal static class libEagle {
    /// <summary>
    /// good old Eagle Engine modified by Hawkynt to support thresholds
    /// </summary>
    public static void Eagle2x(PixelWorker<sPixel>worker ) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;
      if ((c1.IsLike(c0)) && (c1.IsLike(c3)))
        e00 = sPixel.Interpolate(c1, c0, c3);

      if ((c2.IsLike(c1)) && (c2.IsLike(c5)))
        e01 = sPixel.Interpolate(c2, c1, c5);

      if ((c6.IsLike(c3)) && (c6.IsLike(c7)))
        e10 = sPixel.Interpolate(c6, c3, c7);

      if ((c7.IsLike(c5)) && (c7.IsLike(c8)))
        e11 = sPixel.Interpolate(c7, c5, c8);

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }

    /// <summary>
    /// AFAIK there is no eagle 3x so I made one (Hawkynt)
    /// </summary>
    public static void Eagle3x(PixelWorker<sPixel> worker) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
      sPixel e01, e02, e10, e12, e20, e21, e22;
      var e00 = e01 = e02 = e10 = e12 = e20 = e21 = e22 = c4;

      if ((c0.IsLike(c1)) && (c0.IsLike(c3)))
        e00 = sPixel.Interpolate(c0, c1, c3);

      if ((c2.IsLike(c1)) && (c2.IsLike(c5)))
        e02 = sPixel.Interpolate(c2, c1, c5);

      if ((c6.IsLike(c3)) && (c6.IsLike(c7)))
        e20 = sPixel.Interpolate(c6, c3, c7);

      if ((c8.IsLike(c5)) && (c8.IsLike(c7)))
        e22 = sPixel.Interpolate(c8, c5, c7);

      if ((c0.IsLike(c1)) && (c0.IsLike(c3)) && (c2.IsLike(c1)) && (c2.IsLike(c5)))
        e01 = sPixel.Interpolate(sPixel.Interpolate(c0, c1, c3), sPixel.Interpolate(c2, c1, c5));

      if ((c2.IsLike(c1)) && (c2.IsLike(c5)) && (c8.IsLike(c5)) && (c8.IsLike(c7)))
        e12 = sPixel.Interpolate(sPixel.Interpolate(c2, c1, c5), sPixel.Interpolate(c8, c5, c7));

      if ((c6.IsLike(c7)) && (c6.IsLike(c3)) && (c8.IsLike(c5)) && (c8.IsLike(c7)))
        e21 = sPixel.Interpolate(sPixel.Interpolate(c6, c7, c3), sPixel.Interpolate(c8, c5, c7));

      if ((c0.IsLike(c1)) && (c0.IsLike(c3)) && (c6.IsLike(c7)) && (c6.IsLike(c3)))
        e10 = sPixel.Interpolate(sPixel.Interpolate(c0, c1, c3), sPixel.Interpolate(c6, c3, c7));

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP2P0(e02);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(c4);
      worker.TargetP2P1(e12);
      worker.TargetP0P2(e20);
      worker.TargetP1P2(e21);
      worker.TargetP2P2(e22);
    }

    /// <summary>
    /// another one that takes into account that normal eagle means that 3 surroundings should be equal
    /// looks ugly sometimes depends heavily on source image
    /// </summary>
    public static void Eagle3xB(PixelWorker<sPixel> worker) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
      sPixel e01, e02, e10, e12, e20, e21, e22;
      var e00 = e01 = e02 = e10 = e12 = e20 = e21 = e22 = c4;

      if ((c0.IsLike(c1)) && (c0.IsLike(c3)))
        e00 = sPixel.Interpolate(c0, c1, c3);

      if ((c2.IsLike(c1)) && (c2.IsLike(c5)))
        e02 = sPixel.Interpolate(c2, c1, c5);

      if ((c6.IsLike(c3)) && (c6.IsLike(c7)))
        e20 = sPixel.Interpolate(c6, c3, c7);

      if ((c8.IsLike(c5)) && (c8.IsLike(c7)))
        e22 = sPixel.Interpolate(c8, c5, c7);

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP2P0(e02);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(c4);
      worker.TargetP2P1(e12);
      worker.TargetP0P2(e20);
      worker.TargetP1P2(e21);
      worker.TargetP2P2(e22);
    }
  } // end class
} // end namespace
