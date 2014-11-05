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
  internal static class libKreed {
    // used for 2xSaI, Super Eagle, Super 2xSaI
    // using thresholds when comparing (Hawkynt)
    private static int _Conc2D(sPixel c00, sPixel c01, sPixel c10, sPixel c11) {
      var result = 0;

      var acAreAlike = c00.IsLike(c10);
      var x = acAreAlike ? 1 : 0;
      var y = (c01.IsLike(c10) && !(acAreAlike)) ? 1 : 0;

      var adAreAlike = c00.IsLike(c11);
      x += adAreAlike ? 1 : 0;
      y += (c01.IsLike(c11) && !(adAreAlike)) ? 1 : 0;

      if (x <= 1)
        result++;
      if (y <= 1)
        result--;

      return (result);
    }

    // TODO: to be really exact, the comparisons are not that right by comparing to already interpolated values
    // TODO: when interpolating 3 or more points I'm using already calculated interpolations and weight them further
    //       which is not the mathematically correct approach, but it's enough - at least for now
    /// <summary>
    /// Kreed's SuperEagle modified by Hawkynt to allow thresholds
    /// </summary>
    public static void SuperEagle(PixelWorker<sPixel> worker) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var d4 = worker.SourceP2P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
      var d5 = worker.SourceP2P1();
      var d1 = worker.SourceP0P2();
      var d2 = worker.SourceP1P2();

      sPixel e00 = c4, e11 = c4;
      sPixel e01, e10;
      if (c4.IsLike(c8)) {
        var c48 = sPixel.Interpolate(c4, c8);
        if (c7.IsLike(c5)) {
          var c57 = sPixel.Interpolate(c5, c7);
          var conc2D = 0;
          conc2D += _Conc2D(c57, c48, c6, d1);
          conc2D += _Conc2D(c57, c48, c3, c1);
          conc2D += _Conc2D(c57, c48, d2, d5);
          conc2D += _Conc2D(c57, c48, c2, d4);

          if (conc2D > 0) {
            e10 = c57;
            e01 = c57;
            e11 = sPixel.Interpolate(c48, c57);
            e00 = sPixel.Interpolate(c48, c57);
          } else if (conc2D < 0) {
            e10 = sPixel.Interpolate(c48, c57);
            e01 = sPixel.Interpolate(c48, c57);
          } else {
            e10 = c57;
            e01 = c57;
          }
        } else {
          if (c48.IsLike(c1) && c48.IsLike(d5))
            e01 = sPixel.Interpolate(sPixel.Interpolate(c48, c1, d5), c5, 3, 1);
          else if (c48.IsLike(c1))
            e01 = sPixel.Interpolate(sPixel.Interpolate(c48, c1), c5, 3, 1);
          else if (c48.IsLike(d5))
            e01 = sPixel.Interpolate(sPixel.Interpolate(c48, d5), c5, 3, 1);
          else
            e01 = sPixel.Interpolate(c48, c5);

          if (c48.IsLike(d2) && c48.IsLike(c3))
            e10 = sPixel.Interpolate(sPixel.Interpolate(c48, d2, c3), c7, 3, 1);
          else if (c48.IsLike(d2))
            e10 = sPixel.Interpolate(sPixel.Interpolate(c48, d2), c7, 3, 1);
          else if (c48.IsLike(c3))
            e10 = sPixel.Interpolate(sPixel.Interpolate(c48, c3), c7, 3, 1);
          else
            e10 = sPixel.Interpolate(c48, c7);

        }
      } else {
        if (c7.IsLike(c5)) {
          var c57 = sPixel.Interpolate(c5, c7);
          e01 = c57;
          e10 = c57;

          if (c57.IsLike(c6) && c57.IsLike(c2))
            e00 = sPixel.Interpolate(sPixel.Interpolate(c57, c6, c2), c4, 3, 1);
          else if (c57.IsLike(c6))
            e00 = sPixel.Interpolate(sPixel.Interpolate(c57, c6), c4, 3, 1);
          else if (c57.IsLike(c2))
            e00 = sPixel.Interpolate(sPixel.Interpolate(c57, c2), c4, 3, 1);
          else
            e00 = sPixel.Interpolate(c57, c4);

          if (c57.IsLike(d4) && c57.IsLike(d1))
            e11 = sPixel.Interpolate(sPixel.Interpolate(c57, d4, d1), c8, 3, 1);
          else if (c57.IsLike(d4))
            e11 = sPixel.Interpolate(sPixel.Interpolate(c57, d4), c8, 3, 1);
          else if (c57.IsLike(d1))
            e11 = sPixel.Interpolate(sPixel.Interpolate(c57, d1), c8, 3, 1);
          else
            e11 = sPixel.Interpolate(c57, c8);

        } else {
          e11 = sPixel.Interpolate(c8, c7, c5, 6, 1, 1);
          e00 = sPixel.Interpolate(c4, c7, c5, 6, 1, 1);
          e10 = sPixel.Interpolate(c7, c4, c8, 6, 1, 1);
          e01 = sPixel.Interpolate(c5, c4, c8, 6, 1, 1);
        }
      }

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }

    /// <summary>
    /// Derek Liauw Kie Fa's 2XSaI
    /// </summary>
    public static void SaI2X(PixelWorker<sPixel> worker) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var d3 = worker.SourceP2M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var d4 = worker.SourceP2P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
      var d5 = worker.SourceP2P1();
      var d0 = worker.SourceM1P2();
      var d1 = worker.SourceP0P2();
      var d2 = worker.SourceP1P2();

      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;

      if (c4.IsLike(c8) && c5.IsNotLike(c7)) {
        var c48 = sPixel.Interpolate(c4, c8);
        if ((c48.IsLike(c1) && c5.IsLike(d5)) || (c48.IsLike(c7) && c48.IsLike(c2) && c5.IsNotLike(c1) && c5.IsLike(d3))) {
          //nothing
        } else {
          e01 = sPixel.Interpolate(c48, c5);
        }

        if ((c48.IsLike(c3) && c7.IsLike(d2)) || (c48.IsLike(c5) && c48.IsLike(c6) && c3.IsNotLike(c7) && c7.IsLike(d0))) {
          //nothing
        } else {
          e10 = sPixel.Interpolate(c48, c7);
        }
      } else if (c5.IsLike(c7) && c4.IsNotLike(c8)) {
        var c57 = sPixel.Interpolate(c5, c7);
        if ((c57.IsLike(c2) && c4.IsLike(c6)) || (c57.IsLike(c1) && c57.IsLike(c8) && c4.IsNotLike(c2) && c4.IsLike(c0))) {
          e01 = c57;
        } else {
          e01 = sPixel.Interpolate(c4, c57);
        }

        if ((c57.IsLike(c6) && c4.IsLike(c2)) || (c57.IsLike(c3) && c57.IsLike(c8) && c4.IsNotLike(c6) && c4.IsLike(c0))) {
          e10 = c57;
        } else {
          e10 = sPixel.Interpolate(c4, c57);
        }
        e11 = c57;
      } else if (c4.IsLike(c8) && c5.IsLike(c7)) {
        var c48 = sPixel.Interpolate(c4, c8);
        var c57 = sPixel.Interpolate(c5, c7);
        if (c48.IsNotLike(c57)) {
          var conc2D = 0;
          conc2D += _Conc2D(c48, c57, c3, c1);
          conc2D -= _Conc2D(c57, c48, d4, c2);
          conc2D -= _Conc2D(c57, c48, c6, d1);
          conc2D += _Conc2D(c48, c57, d5, d2);

          if (conc2D < 0) {
            e11 = c57;
          } else if (conc2D == 0) {
            e11 = sPixel.Interpolate(c48, c57);
          }
          e10 = sPixel.Interpolate(c48, c57);
          e01 = sPixel.Interpolate(c48, c57);
        }
      } else {
        e11 = sPixel.Interpolate(c4, c5, c7, c8);

        if (c4.IsLike(c7) && c4.IsLike(c2) && c5.IsNotLike(c1) && c5.IsLike(d3)) {
          //nothing
        } else if (c5.IsLike(c1) && c5.IsLike(c8) && c4.IsNotLike(c2) && c4.IsLike(c0)) {
          e01 = sPixel.Interpolate(c5, c1, c8);
        } else {
          e01 = sPixel.Interpolate(c4, c5);
        }

        if (c4.IsLike(c5) && c4.IsLike(c6) && c3.IsNotLike(c7) && c7.IsLike(d0)) {
          //nothing
        } else if (c7.IsLike(c3) && c7.IsLike(c8) && c4.IsNotLike(c6) && c4.IsLike(c0)) {
          e10 = sPixel.Interpolate(c7, c3, c8);
        } else {
          e10 = sPixel.Interpolate(c4, c7);
        }
      }

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }

    /// <summary>
    /// Kreed's SuperSaI
    /// </summary>
    public static void SuperSaI(PixelWorker<sPixel> worker) {
      var c0 = worker.SourceM1M1();
      var c1 = worker.SourceP0M1();
      var c2 = worker.SourceP1M1();
      var d3 = worker.SourceP2M1();
      var c3 = worker.SourceM1P0();
      var c4 = worker.SourceP0P0();
      var c5 = worker.SourceP1P0();
      var d4 = worker.SourceP2P0();
      var c6 = worker.SourceM1P1();
      var c7 = worker.SourceP0P1();
      var c8 = worker.SourceP1P1();
      var d5 = worker.SourceP2P1();
      var d0 = worker.SourceM1P2();
      var d1 = worker.SourceP0P2();
      var d2 = worker.SourceP1P2();
      var d6 = worker.SourceP2P2();

      sPixel e01, e10, e11;
      var e00 = e01 = e11 = c4;

      if (c7.IsLike(c5) && c4.IsNotLike(c8)) {
        var c57 = sPixel.Interpolate(c7, c5);
        e11 = c57;
        e01 = c57;
      } else if (c4.IsLike(c8) && c7.IsNotLike(c5)) {
        //nothing
      } else if (c4.IsLike(c8) && c7.IsLike(c5)) {
        var c57 = sPixel.Interpolate(c7, c5);
        var c48 = sPixel.Interpolate(c4, c8);
        var conc2D = 0;
        conc2D += _Conc2D(c57, c48, c6, d1);
        conc2D += _Conc2D(c57, c48, c3, c1);
        conc2D += _Conc2D(c57, c48, d2, d5);
        conc2D += _Conc2D(c57, c48, c2, d4);

        if (conc2D > 0) {
          e11 = c57;
          e01 = c57;
        } else if (conc2D == 0) {
          e11 = sPixel.Interpolate(c48, c57);
          e01 = sPixel.Interpolate(c48, c57);
        }
      } else {
        if (c8.IsLike(c5) && c8.IsLike(d1) && c7.IsNotLike(d2) && c8.IsNotLike(d0)) {
          e11 = sPixel.Interpolate(sPixel.Interpolate(c8, c5, d1), c7, 3, 1);
        } else if (c7.IsLike(c4) && c7.IsLike(d2) && c7.IsNotLike(d6) && c8.IsNotLike(d1)) {
          e11 = sPixel.Interpolate(sPixel.Interpolate(c7, c4, d2), c8, 3, 1);
        } else {
          e11 = sPixel.Interpolate(c7, c8);
        }
        if (c5.IsLike(c8) && c5.IsLike(c1) && c5.IsNotLike(c0) && c4.IsNotLike(c2)) {
          e01 = sPixel.Interpolate(sPixel.Interpolate(c5, c8, c1), c4, 3, 1);
        } else if (c4.IsLike(c7) && c4.IsLike(c2) && c5.IsNotLike(c1) && c4.IsNotLike(d3)) {
          e01 = sPixel.Interpolate(sPixel.Interpolate(c4, c7, c2), c5, 3, 1);
        } else {
          e01 = sPixel.Interpolate(c4, c5);
        }
      }
      if (c4.IsLike(c8) && c4.IsLike(c3) && c7.IsNotLike(c5) && c4.IsNotLike(d2)) {
        e10 = sPixel.Interpolate(c7, sPixel.Interpolate(c4, c8, c3));
      } else if (c4.IsLike(c6) && c4.IsLike(c5) && c7.IsNotLike(c3) && c4.IsNotLike(d0)) {
        e10 = sPixel.Interpolate(c7, sPixel.Interpolate(c4, c6, c5));
      } else {
        e10 = c7;
      }

      if (c7.IsLike(c5) && c7.IsLike(c6) && c4.IsNotLike(c8) && c7.IsNotLike(c2)) {
        e00 = sPixel.Interpolate(sPixel.Interpolate(c7, c5, c6), c4);
      } else if (c7.IsLike(c3) && c7.IsLike(c8) && c4.IsNotLike(c6) && c7.IsNotLike(c0)) {
        e00 = sPixel.Interpolate(sPixel.Interpolate(c7, c3, c8), c4);
      }

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }


  } // end class
} // end namespace
