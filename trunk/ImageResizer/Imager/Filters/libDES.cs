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
  internal static class libDES {
    /// <summary>
    /// DES filters from FNES
    /// </summary>
    public static void DES(PixelWorker<sPixel> worker) {
      var n = worker.SourceP0M1();
      var w = worker.SourceM1P0();
      var c = worker.SourceP0P0();
      var e = worker.SourceP1P0();
      var s = worker.SourceP0P1();


      var p0 = (((w.IsLike(n)) && (n.IsNotLike(e)) && (w.IsNotLike(s))) ? w : c);
      var p1 = (((n.IsLike(e)) && (n.IsNotLike(w)) && (e.IsNotLike(s))) ? e : c);
      var p2 = (((w.IsLike(s)) && (w.IsNotLike(n)) && (s.IsNotLike(e))) ? w : c);
      var p3 = (((s.IsLike(e)) && (w.IsNotLike(s)) && (n.IsNotLike(e))) ? e : c);

      var d = sPixel.Interpolate(p0, p1, p2, p3);

      worker.TargetP0P0(d);
    }

    public static void DES2(PixelWorker<sPixel> worker) {
      var n = worker.SourceP0M1();
      var w = worker.SourceM1P0();
      var c = worker.SourceP0P0();
      var e = worker.SourceP1P0();
      var s = worker.SourceP0P1();
      var se = worker.SourceP1P1();

      var p0 = (((w.IsLike(n)) && (n.IsNotLike(e)) && (w.IsNotLike(s))) ? w : c);
      var p1 = (((n.IsLike(e)) && (n.IsNotLike(w)) && (e.IsNotLike(s))) ? e : c);
      var p2 = (((w.IsLike(s)) && (w.IsNotLike(n)) && (s.IsNotLike(e))) ? w : c);
      var p3 = (((s.IsLike(e)) && (w.IsNotLike(s)) && (n.IsNotLike(e))) ? e : c);

      var cx = c;
      var ce = sPixel.Interpolate(c, e, 3, 1);
      var cs = sPixel.Interpolate(c, s, 3, 1);
      var cse = sPixel.Interpolate(c, se, 3, 1);


      var d1 = sPixel.Interpolate(p0, cx, 3, 1);
      var d2 = sPixel.Interpolate(p1, ce, 3, 1);
      var d3 = sPixel.Interpolate(p2, cs, 3, 1);
      var d4 = sPixel.Interpolate(p3, cse, 3, 1);

      worker.TargetP0P0(d1);
      worker.TargetP1P0(d2);
      worker.TargetP0P1(d3);
      worker.TargetP1P1(d4);
    }
  } // end class
} // end namespace
