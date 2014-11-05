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

using System.Runtime.CompilerServices;

namespace Imager.Filters {
  internal static class lib2xSCL {
    /// <summary>
    /// FNES' 2xSCL
    /// </summary>
    /// <param name="worker">The worker.</param>
    public static void Do2XScl(PixelWorker<sPixel> worker) {
      var n = worker.SourceP0M1();
      var w = worker.SourceM1P0();
      var c = worker.SourceP0P0();
      var e = worker.SourceP1P0();
      var s = worker.SourceP0P1();

      var p0 = (((w.IsLike(n)) && (n.IsNotLike(e)) && (w.IsNotLike(s))) ? w : c);
      var p1 = (((n.IsLike(e)) && (n.IsNotLike(w)) && (e.IsNotLike(s))) ? e : c);
      var p2 = (((w.IsLike(s)) && (w.IsNotLike(n)) && (s.IsNotLike(e))) ? w : c);
      var p3 = (((s.IsLike(e)) && (w.IsNotLike(s)) && (n.IsNotLike(e))) ? e : c);

      worker.TargetP0P0(p0);
      worker.TargetP1P0(p1);
      worker.TargetP0P1(p2);
      worker.TargetP1P1(p3);
    }

    public static void DoSuper2XScl(PixelWorker<sPixel> worker) {
      var n = worker.SourceP0M1();
      var w = worker.SourceM1P0();
      var c = worker.SourceP0P0();
      var e = worker.SourceP1P0();
      var s = worker.SourceP0P1();

      var wx = w;
      var ex = e;
      var cw = _Mixpal(c, w);
      var ce = _Mixpal(c, e);

      var p0 = (((w.IsLike(n)) && (n.IsNotLike(e)) && (w.IsNotLike(s))) ? wx : cw);
      var p1 = (((n.IsLike(e)) && (n.IsNotLike(w)) && (e.IsNotLike(s))) ? ex : ce);
      var p2 = (((w.IsLike(s)) && (w.IsNotLike(n)) && (s.IsNotLike(e))) ? wx : cw);
      var p3 = (((s.IsLike(e)) && (w.IsNotLike(s)) && (n.IsNotLike(e))) ? ex : ce);

      worker.TargetP0P0(p0);
      worker.TargetP1P0(p1);
      worker.TargetP0P1(p2);
      worker.TargetP1P1(p3);
    }

    public static void DoUltra2XScl(PixelWorker<sPixel> worker) {
      var n = worker.SourceP0M1();
      var w = worker.SourceM1P0();
      var c = worker.SourceP0P0();
      var e = worker.SourceP1P0();
      var s = worker.SourceP0P1();

      var cx = c;
      var wx = w;
      var ex = e;
      var cw = _Mixpal(c, w);
      var ce = _Mixpal(c, e);

      var p0 = (((w.IsLike(n)) && (n.IsNotLike(e)) && (w.IsNotLike(s))) ? wx : cw);
      var p1 = (((n.IsLike(e)) && (n.IsNotLike(w)) && (e.IsNotLike(s))) ? ex : ce);
      var p2 = (((w.IsLike(s)) && (w.IsNotLike(n)) && (s.IsNotLike(e))) ? wx : cw);
      var p3 = (((s.IsLike(e)) && (w.IsNotLike(s)) && (n.IsNotLike(e))) ? ex : ce);

      p0 = _Unmix(p0, cx);
      p1 = _Unmix(p1, cx);
      p2 = _Unmix(p2, cx);
      p3 = _Unmix(p3, cx);

      worker.TargetP0P0(p0);
      worker.TargetP1P0(p1);
      worker.TargetP0P1(p2);
      worker.TargetP1P1(p3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static sPixel _Mixpal(sPixel c1, sPixel c2) {
      return (sPixel.Interpolate(c1, c2, 3, 1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static sPixel _Unmix(sPixel c1, sPixel c2) {

      /* A variant of an unsharp mask, without the blur part. */

      var ra = c1.Red;
      var ga = c1.Green;
      var ba = c1.Blue;

      var rb = c2.Red;
      var gb = c2.Green;
      var bb = c2.Blue;

      var r = ((_Fix((ra + (ra - rb)), 0, 255) + rb) >> 1);
      var g = ((_Fix((ga + (ga - gb)), 0, 255) + gb) >> 1);
      var b = ((_Fix((ba + (ba - bb)), 0, 255) + bb) >> 1);

      return (sPixel.FromRGBA(r, g, b, (c1.Alpha + c2.Alpha) >> 1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int _Fix(int n, int min, int max) {
      return (n < min ? min : n > max ? max : n);
    }

  } // end class
} // end namespace
