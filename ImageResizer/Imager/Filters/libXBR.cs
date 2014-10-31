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


using System.Diagnostics.Contracts;

namespace Imager.Filters {
  internal static class libXBR {
    /// <summary>
    /// This is the XBR2x by Hyllian (see http://board.byuu.org/viewtopic.php?f=10&t=2248)
    /// </summary>
    public static void Xbr2X(PixelWorker<sPixel> worker , bool allowAlphaBlending) {
      Contract.Assume(worker!= null);
      var pa = worker.SourceM1M1();
      var pb = worker.SourceP0M1();
      var pc = worker.SourceP1M1();

      var pd = worker.SourceM1P0();
      var pe = worker.SourceP0P0();
      var pf = worker.SourceP1P0();

      var pg = worker.SourceM1P1();
      var ph = worker.SourceP0P1();
      var pi = worker.SourceP1P1();

      var a1 = worker.SourceM1M2();
      var b1 = worker.SourceP0M2();
      var c1 = worker.SourceP1M2();

      var a0 = worker.SourceM2M1();
      var d0 = worker.SourceM2P0();
      var g0 = worker.SourceM2P1();

      var c4 = worker.SourceP2M1();
      var f4 = worker.SourceP2P0();
      var i4 = worker.SourceP2P1();

      var g5 = worker.SourceM1P2();
      var h5 = worker.SourceP0P2();
      var i5 = worker.SourceP1P2();

      sPixel e1, e2, e3;
      var e0 = e1 = e2 = e3 = pe;

      _Kernel2Xv5(pe, pi, ph, pf, pg, pc, pd, pb, f4, i4, h5, i5, ref e1, ref e2, ref e3, allowAlphaBlending);
      _Kernel2Xv5(pe, pc, pf, pb, pi, pa, ph, pd, b1, c1, f4, c4, ref e0, ref e3, ref e1, allowAlphaBlending);
      _Kernel2Xv5(pe, pa, pb, pd, pc, pg, pf, ph, d0, a0, b1, a1, ref e2, ref e1, ref e0, allowAlphaBlending);
      _Kernel2Xv5(pe, pg, pd, ph, pa, pi, pb, pf, h5, g5, d0, g0, ref e3, ref e0, ref e2, allowAlphaBlending);

      worker.TargetP0P0(e0);
      worker.TargetP1P0(e1);
      worker.TargetP0P1(e2);
      worker.TargetP1P1(e3);
    }

    /// <summary>
    /// This is the XBR3x by Hyllian (see http://board.byuu.org/viewtopic.php?f=10&t=2248)
    /// </summary>
    public static void Xbr3X(PixelWorker<sPixel> worker ,  bool allowAlphaBlending, bool useOriginalImplementation) {
      Contract.Assume(worker != null);
      var pa = worker.SourceM1M1();
      var pb = worker.SourceP0M1();
      var pc = worker.SourceP1M1();

      var pd = worker.SourceM1P0();
      var pe = worker.SourceP0P0();
      var pf = worker.SourceP1P0();

      var pg = worker.SourceM1P1();
      var ph = worker.SourceP0P1();
      var pi = worker.SourceP1P1();

      var a1 = worker.SourceM1M2();
      var b1 = worker.SourceP0M2();
      var c1 = worker.SourceP1M2();

      var a0 = worker.SourceM2M1();
      var d0 = worker.SourceM2P0();
      var g0 = worker.SourceM2P1();

      var c4 = worker.SourceP2M1();
      var f4 = worker.SourceP2P0();
      var i4 = worker.SourceP2P1();

      var g5 = worker.SourceM1P2();
      var h5 = worker.SourceP0P2();
      var i5 = worker.SourceP1P2();

      sPixel e1, e2, e3, e4, e5, e6, e7, e8;
      var e0 = e1 = e2 = e3 = e4 = e5 = e6 = e7 = e8 = pe;

      _Kernel3X(pe, pi, ph, pf, pg, pc, pd, pb, f4, i4, h5, i5, ref e2, ref e5, ref e6, ref e7, ref e8, allowAlphaBlending, useOriginalImplementation);
      _Kernel3X(pe, pc, pf, pb, pi, pa, ph, pd, b1, c1, f4, c4, ref e0, ref e1, ref e8, ref e5, ref e2, allowAlphaBlending, useOriginalImplementation);
      _Kernel3X(pe, pa, pb, pd, pc, pg, pf, ph, d0, a0, b1, a1, ref e6, ref e3, ref e2, ref e1, ref e0, allowAlphaBlending, useOriginalImplementation);
      _Kernel3X(pe, pg, pd, ph, pa, pi, pb, pf, h5, g5, d0, g0, ref e8, ref e7, ref e0, ref e3, ref e6, allowAlphaBlending, useOriginalImplementation);

      worker.TargetP0P0(e0);
      worker.TargetP1P0(e1);
      worker.TargetP2P0(e2);
      worker.TargetP0P1(e3);
      worker.TargetP1P1(e4);
      worker.TargetP2P1(e5);
      worker.TargetP0P2(e6);
      worker.TargetP1P2(e7);
      worker.TargetP2P2(e8);
    }

    /// <summary>
    /// This is the XBR4x by Hyllian (see http://board.byuu.org/viewtopic.php?f=10&t=2248)
    /// </summary>
    public static void Xbr4X(PixelWorker<sPixel> worker, bool allowAlphaBlending) {
      Contract.Assume(worker != null);
      var pa = worker.SourceM1M1();
      var pb = worker.SourceP0M1();
      var pc = worker.SourceP1M1();

      var pd = worker.SourceM1P0();
      var pe = worker.SourceP0P0();
      var pf = worker.SourceP1P0();

      var pg = worker.SourceM1P1();
      var ph = worker.SourceP0P1();
      var pi = worker.SourceP1P1();

      var a1 = worker.SourceM1M2();
      var b1 = worker.SourceP0M2();
      var c1 = worker.SourceP1M2();

      var a0 = worker.SourceM2M1();
      var d0 = worker.SourceM2P0();
      var g0 = worker.SourceM2P1();

      var c4 = worker.SourceP2M1();
      var f4 = worker.SourceP2P0();
      var i4 = worker.SourceP2P1();

      var g5 = worker.SourceM1P2();
      var h5 = worker.SourceP0P2();
      var i5 = worker.SourceP1P2();

      sPixel e1, e2, e3, e4, e5, e6, e7, e8, e9, ea, eb, ec, ed, ee, ef;
      var e0 = e1 = e2 = e3 = e4 = e5 = e6 = e7 = e8 = e9 = ea = eb = ec = ed = ee = ef = pe;

      _Kernel4Xv2(pe, pi, ph, pf, pg, pc, pd, pb, f4, i4, h5, i5, ref ef, ref ee, ref eb, ref e3, ref e7, ref ea, ref ed, ref ec, allowAlphaBlending);
      _Kernel4Xv2(pe, pc, pf, pb, pi, pa, ph, pd, b1, c1, f4, c4, ref e3, ref e7, ref e2, ref e0, ref e1, ref e6, ref eb, ref ef, allowAlphaBlending);
      _Kernel4Xv2(pe, pa, pb, pd, pc, pg, pf, ph, d0, a0, b1, a1, ref e0, ref e1, ref e4, ref ec, ref e8, ref e5, ref e2, ref e3, allowAlphaBlending);
      _Kernel4Xv2(pe, pg, pd, ph, pa, pi, pb, pf, h5, g5, d0, g0, ref ec, ref e8, ref ed, ref ef, ref ee, ref e9, ref e4, ref e0, allowAlphaBlending);

      worker.TargetP0P0(e0);
      worker.TargetP1P0(e1);
      worker.TargetP2P0(e2);
      worker.TargetP3P0(e3);
      worker.TargetP0P1(e4);
      worker.TargetP1P1(e5);
      worker.TargetP2P1(e6);
      worker.TargetP3P1(e7);
      worker.TargetP0P2(e8);
      worker.TargetP1P2(e9);
      worker.TargetP2P2(ea);
      worker.TargetP3P2(eb);
      worker.TargetP0P3(ec);
      worker.TargetP1P3(ed);
      worker.TargetP2P3(ee);
      worker.TargetP3P3(ef);
    }

    private static uint _YuvDifference(sPixel a, sPixel b) {
      return (a.AbsDifference(b));
    }

    private static bool _IsEqual(sPixel a, sPixel b) {
      return (a.IsLike(b));
    }

    private static void _AlphaBlend32W(ref sPixel dst, sPixel src, bool blend) {
      if (blend)
        dst = sPixel.Interpolate(dst, src, 7, 1);
    }

    private static void _AlphaBlend64W(ref sPixel dst, sPixel src, bool blend) {
      if (blend)
        dst = sPixel.Interpolate(dst, src, 3, 1);
    }

    private static void _AlphaBlend128W(ref sPixel dst, sPixel src, bool blend) {
      if (blend)
        dst = sPixel.Interpolate(dst, src);
    }

    private static void _AlphaBlend192W(ref sPixel dst, sPixel src, bool blend) {
      dst = blend ? sPixel.Interpolate(dst, src, 1, 3) : src;
    }

    private static void _AlphaBlend224W(ref sPixel dst, sPixel src, bool blend) {
      dst = blend ? sPixel.Interpolate(dst, src, 1, 7) : src;
    }

    #region 2x
    private static void _LeftUp2_2X(ref sPixel n3, ref sPixel n2, out sPixel n1, sPixel pixel, bool blend) {
      _AlphaBlend224W(ref n3, pixel, blend);
      _AlphaBlend64W(ref n2, pixel, blend);
      n1 = n2;
    }

    private static void _Left2_2X(ref sPixel n3, ref sPixel n2, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n3, pixel, blend);
      _AlphaBlend64W(ref n2, pixel, blend);
    }
    private static void _Up2_2X(ref sPixel n3, ref sPixel n1, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n3, pixel, blend);
      _AlphaBlend64W(ref n1, pixel, blend);
    }

    private static void _Dia_2X(ref sPixel n3, sPixel pixel, bool blend) {
      _AlphaBlend128W(ref n3, pixel, blend);
    }

    private static void _Kernel2Xv5(sPixel pe, sPixel pi, sPixel ph, sPixel pf, sPixel pg, sPixel pc, sPixel pd, sPixel pb, sPixel f4, sPixel i4, sPixel h5, sPixel i5, ref sPixel n1, ref sPixel n2, ref sPixel n3, bool blend) {
      var ex = (pe != ph && pe != pf);
      if (!ex)
        return;
      var e = (_YuvDifference(pe, pc) + _YuvDifference(pe, pg) + _YuvDifference(pi, h5) + _YuvDifference(pi, f4)) + (_YuvDifference(ph, pf) << 2);
      var i = (_YuvDifference(ph, pd) + _YuvDifference(ph, i5) + _YuvDifference(pf, i4) + _YuvDifference(pf, pb)) + (_YuvDifference(pe, pi) << 2);
      var px = (_YuvDifference(pe, pf) <= _YuvDifference(pe, ph)) ? pf : ph;
      if ((e < i) && (!_IsEqual(pf, pb) && !_IsEqual(ph, pd) || _IsEqual(pe, pi) && (!_IsEqual(pf, i4) && !_IsEqual(ph, i5)) || _IsEqual(pe, pg) || _IsEqual(pe, pc))) {
        var ke = _YuvDifference(pf, pg);
        var ki = _YuvDifference(ph, pc);
        var ex2 = (pe != pc && pb != pc);
        var ex3 = (pe != pg && pd != pg);
        if (((ke << 1) <= ki) && ex3 || (ke >= (ki << 1)) && ex2) {
          if (((ke << 1) <= ki) && ex3)
            _Left2_2X(ref n3, ref n2, px, blend);
          if ((ke >= (ki << 1)) && ex2)
            _Up2_2X(ref n3, ref n1, px, blend);
        } else
          _Dia_2X(ref n3, px, blend);

      } else if (e <= i) {
        _AlphaBlend64W(ref n3, px, blend);
      }
    }
    #endregion
    #region 3x
    private static void _LeftUp2_3X(ref sPixel n7, out sPixel n5, ref sPixel n6, ref sPixel n2, out sPixel n8, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n7, pixel, blend);
      _AlphaBlend64W(ref n6, pixel, blend);
      n5 = n7;
      n2 = n6;
      n8 = pixel;
    }

    private static void _Left2_3X(ref sPixel n7, ref sPixel n5, ref sPixel n6, out sPixel n8, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n7, pixel, blend);
      _AlphaBlend64W(ref n5, pixel, blend);
      _AlphaBlend64W(ref n6, pixel, blend);
      n8 = pixel;
    }

    private static void _Up2_3X(ref sPixel n5, ref sPixel n7, ref sPixel n2, out sPixel n8, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n5, pixel, blend);
      _AlphaBlend64W(ref n7, pixel, blend);
      _AlphaBlend64W(ref n2, pixel, blend);
      n8 = pixel;
    }

    private static void _Dia_3X(ref sPixel n8, ref sPixel n5, ref sPixel n7, sPixel pixel, bool blend) {
      _AlphaBlend224W(ref n8, pixel, blend);
      _AlphaBlend32W(ref n5, pixel, blend);
      _AlphaBlend32W(ref n7, pixel, blend);
    }

    private static void _Kernel3X(sPixel pe, sPixel pi, sPixel ph, sPixel pf, sPixel pg, sPixel pc, sPixel pd, sPixel pb, sPixel f4, sPixel i4, sPixel h5, sPixel i5, ref sPixel n2, ref sPixel n5, ref sPixel n6, ref sPixel n7, ref sPixel n8, bool blend, bool useOriginalImplementation) {
      var ex = (pe != ph && pe != pf);
      if (!ex)
        return;

      var e = (_YuvDifference(pe, pc) + _YuvDifference(pe, pg) + _YuvDifference(pi, h5) + _YuvDifference(pi, f4)) + (_YuvDifference(ph, pf) << 2);
      var i = (_YuvDifference(ph, pd) + _YuvDifference(ph, i5) + _YuvDifference(pf, i4) + _YuvDifference(pf, pb)) + (_YuvDifference(pe, pi) << 2);

      bool state;
      if (useOriginalImplementation)
        state = ((e < i) && (!_IsEqual(pf, pb) && !_IsEqual(ph, pd) || _IsEqual(pe, pi) && (!_IsEqual(pf, i4) && !_IsEqual(ph, i5)) || _IsEqual(pe, pg) || _IsEqual(pe, pc)));
      else
        state = ((e < i) && (!_IsEqual(pf, pb) && !_IsEqual(pf, pc) || !_IsEqual(ph, pd) && !_IsEqual(ph, pg) || _IsEqual(pe, pi) && (!_IsEqual(pf, f4) && !_IsEqual(pf, i4) || !_IsEqual(ph, h5) && !_IsEqual(ph, i5)) || _IsEqual(pe, pg) || _IsEqual(pe, pc)));

      if (state) {
        var ke = _YuvDifference(pf, pg);
        var ki = _YuvDifference(ph, pc);
        var ex2 = (pe != pc && pb != pc);
        var ex3 = (pe != pg && pd != pg);
        var px = (_YuvDifference(pe, pf) <= _YuvDifference(pe, ph)) ? pf : ph;
        if (((ke << 1) <= ki) && ex3 && (ke >= (ki << 1)) && ex2) {
          _LeftUp2_3X(ref n7, out n5, ref n6, ref n2, out n8, px, blend);
        } else if (((ke << 1) <= ki) && ex3) {
          _Left2_3X(ref n7, ref n5, ref n6, out  n8, px, blend);
        } else if ((ke >= (ki << 1)) && ex2) {
          _Up2_3X(ref n5, ref n7, ref  n2, out n8, px, blend);
        } else {
          _Dia_3X(ref n8, ref n5, ref n7, px, blend);
        }
      } else if (e <= i) {
        _AlphaBlend128W(ref n8, ((_YuvDifference(pe, pf) <= _YuvDifference(pe, ph)) ? pf : ph), blend);
      }
    }
    #endregion
    #region 4x
    private static void _LeftUp2(out sPixel n15, out sPixel n14, out sPixel n11, ref sPixel n13, ref sPixel n12, out sPixel n10, out sPixel n7, out sPixel n3, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n13, pixel, blend);
      _AlphaBlend64W(ref n12, pixel, blend);
      n15 = n14 = n11 = pixel;
      n10 = n3 = n12;
      n7 = n13;
    }

    private static void _Left2(out sPixel n15, out sPixel n14, ref sPixel n11, ref sPixel n13, ref sPixel n12, ref sPixel n10, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n11, pixel, blend);
      _AlphaBlend192W(ref n13, pixel, blend);
      _AlphaBlend64W(ref n10, pixel, blend);
      _AlphaBlend64W(ref n12, pixel, blend);
      n14 = pixel;
      n15 = pixel;
    }

    private static void _Up2(out sPixel n15, ref sPixel n14, out sPixel n11, ref sPixel n3, ref sPixel n7, ref sPixel n10, sPixel pixel, bool blend) {
      _AlphaBlend192W(ref n14, pixel, blend);
      _AlphaBlend192W(ref n7, pixel, blend);
      _AlphaBlend64W(ref n10, pixel, blend);
      _AlphaBlend64W(ref n3, pixel, blend);
      n11 = pixel;
      n15 = pixel;
    }

    private static void _Dia(out sPixel n15, ref sPixel n14, ref sPixel n11, sPixel pixel, bool blend) {
      _AlphaBlend128W(ref n11, pixel, blend);
      _AlphaBlend128W(ref n14, pixel, blend);
      n15 = pixel;
    }

    private static void _Kernel4Xv2(sPixel pe, sPixel pi, sPixel ph, sPixel pf, sPixel pg, sPixel pc, sPixel pd, sPixel pb, sPixel f4, sPixel i4, sPixel h5, sPixel i5, ref sPixel n15, ref sPixel n14, ref sPixel n11, ref sPixel n3, ref sPixel n7, ref sPixel n10, ref sPixel n13, ref sPixel n12, bool blend) {
      var ex = (pe != ph && pe != pf);
      if (!ex)
        return;
      var e = (_YuvDifference(pe, pc) + _YuvDifference(pe, pg) + _YuvDifference(pi, h5) + _YuvDifference(pi, f4)) + (_YuvDifference(ph, pf) << 2);
      var i = (_YuvDifference(ph, pd) + _YuvDifference(ph, i5) + _YuvDifference(pf, i4) + _YuvDifference(pf, pb)) + (_YuvDifference(pe, pi) << 2);
      var px = (_YuvDifference(pe, pf) <= _YuvDifference(pe, ph)) ? pf : ph;
      if ((e < i) && (!_IsEqual(pf, pb) && !_IsEqual(ph, pd) || _IsEqual(pe, pi) && (!_IsEqual(pf, i4) && !_IsEqual(ph, i5)) || _IsEqual(pe, pg) || _IsEqual(pe, pc))) {
        var ke = _YuvDifference(pf, pg);
        var ki = _YuvDifference(ph, pc);
        var ex2 = (pe != pc && pb != pc);
        var ex3 = (pe != pg && pd != pg);
        if (((ke << 1) <= ki) && ex3 || (ke >= (ki << 1)) && ex2) {
          if (((ke << 1) <= ki) && ex3)
            _Left2(out n15, out n14, ref n11, ref n13, ref n12, ref n10, px, blend);
          if ((ke >= (ki << 1)) && ex2)
            _Up2(out n15, ref n14, out n11, ref n3, ref n7, ref n10, px, blend);
        } else
          _Dia(out n15, ref n14, ref n11, px, blend);

      } else if (e <= i) {
        _AlphaBlend128W(ref n15, px, blend);
      }
    }
    #endregion
  }
}
