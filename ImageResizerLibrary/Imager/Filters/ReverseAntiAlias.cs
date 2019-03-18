#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

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

using System;

namespace Imager.Filters {
  internal static class ReverseAntiAlias {
    /// <summary>
    /// Christoph Feck's (christoph@maxiom.de) Reverse Anti-Alias filter
    /// TODO: make mathutils and join clamp, min and max with the ones from spixel
    /// </summary>
    /// <param name="worker">The worker.</param>
    public static void Process(IPixelWorker<sPixel>worker ) {

      var b1 = worker.SourceP0M2();
      var b = worker.SourceP0M1();
      var d = worker.SourceM1P0();
      var e = worker.SourceP0P0();
      var f = worker.SourceP1P0();
      var h = worker.SourceP0P1();
      var h5 = worker.SourceP0P2();
      var d0 = worker.SourceM2P0();
      var f4 = worker.SourceP2P0();

      var redPart = _ReverseAntiAlias(b1.Red, b.Red, d.Red, e.Red, f.Red, h.Red, h5.Red, d0.Red, f4.Red);
      var greenPart = _ReverseAntiAlias(b1.Green, b.Green, d.Green, e.Green, f.Green, h.Green, h5.Green, d0.Green, f4.Green);
      var bluePart = _ReverseAntiAlias(b1.Blue, b.Blue, d.Blue, e.Blue, f.Blue, h.Blue, h5.Blue, d0.Blue, f4.Blue);
      var alphaPart = _ReverseAntiAlias(b1.Alpha, b.Alpha, d.Alpha, e.Alpha, f.Alpha, h.Alpha, h5.Alpha, d0.Alpha, f4.Alpha);

      worker.TargetP0P0( sPixel.FromRGBA(redPart.Item1, greenPart.Item1, bluePart.Item1, alphaPart.Item1));
      worker.TargetP1P0(sPixel.FromRGBA(redPart.Item2, greenPart.Item2, bluePart.Item2, alphaPart.Item2));
      worker.TargetP0P1( sPixel.FromRGBA(redPart.Item3, greenPart.Item3, bluePart.Item3, alphaPart.Item3));
      worker.TargetP1P1( sPixel.FromRGBA(redPart.Item4, greenPart.Item4, bluePart.Item4, alphaPart.Item4));
    }

    /// <summary>
    /// The internal function which is called for each channel separately.
    /// </summary>
    /// <param name="b1">The b1.</param>
    /// <param name="b">The B.</param>
    /// <param name="d">The D.</param>
    /// <param name="e">The E.</param>
    /// <param name="f">The F.</param>
    /// <param name="h">The H.</param>
    /// <param name="h5">The h5.</param>
    /// <param name="d0">The d0.</param>
    /// <param name="f4">The f4.</param>
    /// <returns></returns>
    private static Tuple<int, int, int, int> _ReverseAntiAlias(int b1, int b, int d, int e, int f, int h, int h5, int d0, int f4) {

      var n1 = b1;
      var n2 = b;
      var s = e;
      var n3 = h;
      var n4 = h5;
      var aa = n2 - n1;
      var bb = s - n2;
      var cc = n3 - s;
      var dd = n4 - n3;

      var tilt = (7 * (bb + cc) - 3 * (aa + dd)) / 16;

      var m = s < 128 ? 2 * s : 2 * (byte.MaxValue - s);

      m = _Min(m, 2 * _Abs(bb));
      m = _Min(m, 2 * _Abs(cc));

      tilt = _Clamp(tilt, -m, m);

      var s1 = s + tilt / 2;
      var s0 = s1 - tilt;

      n1 = d0;
      n2 = d;
      s = s0;
      n3 = f;
      n4 = f4;
      aa = n2 - n1;
      bb = s - n2;
      cc = n3 - s;
      dd = n4 - n3;

      tilt = (7 * (bb + cc) - 3 * (aa + dd)) / 16;

      m = s < 128 ? 2 * s : 2 * (byte.MaxValue - s);

      m = _Min(m, 2 * _Abs(bb));
      m = _Min(m, 2 * _Abs(cc));

      tilt = _Clamp(tilt, -m, m);

      var e1 = s + tilt / 2;
      var e0 = e1 - tilt;

      s = s1;
      bb = s - n2;
      cc = n3 - s;

      tilt = (7 * (bb + cc) - 3 * (aa + dd)) / 16;

      m = s < 128 ? 2 * s : 2 * (byte.MaxValue - s);

      m = _Min(m, 2 * _Abs(bb));
      m = _Min(m, 2 * _Abs(cc));

      tilt = _Clamp(tilt, -m, m);

      var e3 = s + tilt / 2;
      var e2 = e3 - tilt;

      return Tuple.Create(e0, e1, e2, e3);
    }

    private static int _Abs(int a) => a < 0 ? -a : a;
    private static int _Min(int a, int b) => a < b ? a : b;
    private static int _Clamp(int v, int min, int max) => v < min ? min : v > max ? max : v;

  }
}
