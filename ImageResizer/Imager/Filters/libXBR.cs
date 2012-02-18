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
using System;

#if !NET35
using System.Diagnostics.Contracts;
#endif

namespace Imager.Filters {
  public static class libXBR {
    /// <summary>
    /// This is the XBR2x by Hyllian (see http://board.byuu.org/viewtopic.php?f=10&t=2248)
    /// </summary>
    public static void Xbr2X(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
#if !NET35
      Contract.Assume(sourceImage != null);
#endif
      var PA = sourceImage[srcX - 1, srcY - 1];
      var PB = sourceImage[srcX + 0, srcY - 1];
      var PC = sourceImage[srcX + 1, srcY - 1];
      var PD = sourceImage[srcX - 1, srcY + 0];
      var PE = sourceImage[srcX + 0, srcY + 0];
      var PF = sourceImage[srcX + 1, srcY + 0];
      var PG = sourceImage[srcX - 1, srcY + 1];
      var PH = sourceImage[srcX + 0, srcY + 1];
      var PI = sourceImage[srcX + 1, srcY + 1];
      var A1 = sourceImage[srcX - 1, srcY - 2];
      var B1 = sourceImage[srcX + 0, srcY - 2];
      var C1 = sourceImage[srcX + 1, srcY - 2];
      var A0 = sourceImage[srcX - 2, srcY - 1];
      var D0 = sourceImage[srcX - 2, srcY + 0];
      var G0 = sourceImage[srcX - 2, srcY + 1];
      var C4 = sourceImage[srcX + 2, srcY - 1];
      var F4 = sourceImage[srcX + 2, srcY + 0];
      var I4 = sourceImage[srcX + 2, srcY - 1];
      var G5 = sourceImage[srcX - 1, srcY + 2];
      var H5 = sourceImage[srcX + 0, srcY + 2];
      var I5 = sourceImage[srcX + 1, srcY + 2];

      sPixel E1, E2, E3;
      var E0 = E1 = E2 = E3 = PE;

      FILTRO_2X(PE, PI, PH, PF, PG, PC, PD, PB, F4, I4, H5, I5, ref E1, ref E2, ref E3);
      FILTRO_2X(PE, PC, PF, PB, PI, PA, PH, PD, B1, C1, F4, C4, ref E0, ref E3, ref E1);
      FILTRO_2X(PE, PA, PB, PD, PC, PG, PF, PH, D0, A0, B1, A1, ref E2, ref E1, ref E0);
      FILTRO_2X(PE, PG, PD, PH, PA, PI, PB, PF, H5, G5, D0, G0, ref E3, ref E0, ref E2);

      targetImage[tgtX + 0, tgtY + 0] = E0;
      targetImage[tgtX + 1, tgtY + 0] = E1;
      targetImage[tgtX + 0, tgtY + 1] = E2;
      targetImage[tgtX + 1, tgtY + 1] = E3;
    }

    /// <summary>
    /// This is the XBR3x by Hyllian (see http://board.byuu.org/viewtopic.php?f=10&t=2248)
    /// </summary>
    public static void Xbr3X(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
#if !NET35
      Contract.Assume(sourceImage != null);
#endif
      var PA = sourceImage[srcX - 1, srcY - 1];
      var PB = sourceImage[srcX + 0, srcY - 1];
      var PC = sourceImage[srcX + 1, srcY - 1];
      var PD = sourceImage[srcX - 1, srcY + 0];
      var PE = sourceImage[srcX + 0, srcY + 0];
      var PF = sourceImage[srcX + 1, srcY + 0];
      var PG = sourceImage[srcX - 1, srcY + 1];
      var PH = sourceImage[srcX + 0, srcY + 1];
      var PI = sourceImage[srcX + 1, srcY + 1];
      var A1 = sourceImage[srcX - 1, srcY - 2];
      var B1 = sourceImage[srcX + 0, srcY - 2];
      var C1 = sourceImage[srcX + 1, srcY - 2];
      var A0 = sourceImage[srcX - 2, srcY - 1];
      var D0 = sourceImage[srcX - 2, srcY + 0];
      var G0 = sourceImage[srcX - 2, srcY + 1];
      var C4 = sourceImage[srcX + 2, srcY - 1];
      var F4 = sourceImage[srcX + 2, srcY + 0];
      var I4 = sourceImage[srcX + 2, srcY - 1];
      var G5 = sourceImage[srcX - 1, srcY + 2];
      var H5 = sourceImage[srcX + 0, srcY + 2];
      var I5 = sourceImage[srcX + 1, srcY + 2];

      sPixel E1, E2, E3, E4, E5, E6, E7, E8;
      var E0 = E1 = E2 = E3 = E4 = E5 = E6 = E7 = E8 = PE;

      FILTRO_3X(PE, PI, PH, PF, PG, PC, PD, PB, F4, I4, H5, I5, ref E2, ref E5, ref E6, ref E7, ref E8);
      FILTRO_3X(PE, PC, PF, PB, PI, PA, PH, PD, B1, C1, F4, C4, ref E0, ref E1, ref E8, ref E5, ref E2);
      FILTRO_3X(PE, PA, PB, PD, PC, PG, PF, PH, D0, A0, B1, A1, ref E6, ref E3, ref E2, ref E1, ref E0);
      FILTRO_3X(PE, PG, PD, PH, PA, PI, PB, PF, H5, G5, D0, G0, ref E8, ref E7, ref E0, ref E3, ref E6);

      targetImage[tgtX + 0, tgtY + 0] = E0;
      targetImage[tgtX + 1, tgtY + 0] = E1;
      targetImage[tgtX + 2, tgtY + 0] = E2;
      targetImage[tgtX + 0, tgtY + 1] = E3;
      targetImage[tgtX + 1, tgtY + 1] = E4;
      targetImage[tgtX + 2, tgtY + 1] = E5;
      targetImage[tgtX + 0, tgtY + 2] = E6;
      targetImage[tgtX + 1, tgtY + 2] = E7;
      targetImage[tgtX + 2, tgtY + 2] = E8;

    }

    /// <summary>
    /// This is the XBR4x by Hyllian (see http://board.byuu.org/viewtopic.php?f=10&t=2248)
    /// </summary>
    public static void Xbr4X(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte _, byte __, object ___) {
#if !NET35
      Contract.Assume(sourceImage != null);
#endif
      var PA = sourceImage[srcX - 1, srcY - 1];
      var PB = sourceImage[srcX + 0, srcY - 1];
      var PC = sourceImage[srcX + 1, srcY - 1];
      var PD = sourceImage[srcX - 1, srcY + 0];
      var PE = sourceImage[srcX + 0, srcY + 0];
      var PF = sourceImage[srcX + 1, srcY + 0];
      var PG = sourceImage[srcX - 1, srcY + 1];
      var PH = sourceImage[srcX + 0, srcY + 1];
      var PI = sourceImage[srcX + 1, srcY + 1];
      var A1 = sourceImage[srcX - 1, srcY - 2];
      var B1 = sourceImage[srcX + 0, srcY - 2];
      var C1 = sourceImage[srcX + 1, srcY - 2];
      var A0 = sourceImage[srcX - 2, srcY - 1];
      var D0 = sourceImage[srcX - 2, srcY + 0];
      var G0 = sourceImage[srcX - 2, srcY + 1];
      var C4 = sourceImage[srcX + 2, srcY - 1];
      var F4 = sourceImage[srcX + 2, srcY + 0];
      var I4 = sourceImage[srcX + 2, srcY - 1];
      var G5 = sourceImage[srcX - 1, srcY + 2];
      var H5 = sourceImage[srcX + 0, srcY + 2];
      var I5 = sourceImage[srcX + 1, srcY + 2];

      sPixel E1, E2, E3, E4, E5, E6, E7, E8, E9, EA, EB, EC, ED, EE, EF;
      var E0 = E1 = E2 = E3 = E4 = E5 = E6 = E7 = E8 = E9 = EA = EB = EC = ED = EE = EF = PE;

      FILTRO_4X(PE, PI, PH, PF, PG, PC, PD, PB, F4, I4, H5, I5, ref EF, ref EE, ref EB, ref E3, ref E7, ref EA, ref ED, ref EC);
      FILTRO_4X(PE, PC, PF, PB, PI, PA, PH, PD, B1, C1, F4, C4, ref E3, ref E7, ref E2, ref E0, ref E1, ref E6, ref EB, ref EF);
      FILTRO_4X(PE, PA, PB, PD, PC, PG, PF, PH, D0, A0, B1, A1, ref E0, ref E1, ref E4, ref EC, ref E8, ref E5, ref E2, ref E3);
      FILTRO_4X(PE, PG, PD, PH, PA, PI, PB, PF, H5, G5, D0, G0, ref EC, ref E8, ref ED, ref EF, ref EE, ref E9, ref E4, ref E0);

      targetImage[tgtX + 0, tgtY + 0] = E0;
      targetImage[tgtX + 1, tgtY + 0] = E1;
      targetImage[tgtX + 2, tgtY + 0] = E2;
      targetImage[tgtX + 3, tgtY + 0] = E3;
      targetImage[tgtX + 0, tgtY + 1] = E4;
      targetImage[tgtX + 1, tgtY + 1] = E5;
      targetImage[tgtX + 2, tgtY + 1] = E6;
      targetImage[tgtX + 3, tgtY + 1] = E7;
      targetImage[tgtX + 0, tgtY + 2] = E8;
      targetImage[tgtX + 1, tgtY + 2] = E9;
      targetImage[tgtX + 2, tgtY + 2] = EA;
      targetImage[tgtX + 3, tgtY + 2] = EB;
      targetImage[tgtX + 0, tgtY + 3] = EC;
      targetImage[tgtX + 1, tgtY + 3] = ED;
      targetImage[tgtX + 2, tgtY + 3] = EE;
      targetImage[tgtX + 3, tgtY + 3] = EF;
    }

    private static int RGBtoYUV(sPixel A) {
      var r = A.Red;
      var g = A.Green;
      var b = A.Blue;
      var y = ((r << 4) + (g << 5) + (b << 2));
      var u = (-r - (g << 1) + (b << 2));
      var v = ((r << 1) - (g << 1) - (b >> 1));
      return (y + u + v);
    }

    private static int df(sPixel A, sPixel B) {
      return (Math.Abs(RGBtoYUV(A) - RGBtoYUV(B)));
    }

    private static bool eq(sPixel A, sPixel B) {
      return (df(A, B) < 155);
    }


    private static void ALPHA_BLEND_32_W(ref sPixel dst, sPixel src) {
      dst = sPixel.Interpolate(dst, src, 7, 1);
    }

    private static void ALPHA_BLEND_64_W(ref sPixel dst, sPixel src) {
      dst = sPixel.Interpolate(dst, src, 3, 1);
    }

    private static void ALPHA_BLEND_128_W(ref sPixel dst, sPixel src) {
      dst = sPixel.Interpolate(dst, src);
    }

    private static void ALPHA_BLEND_192_W(ref sPixel dst, sPixel src) {
      dst = sPixel.Interpolate(dst, src, 1, 3);
    }

    private static void ALPHA_BLEND_224_W(ref sPixel dst, sPixel src) {
      dst = sPixel.Interpolate(dst, src, 1, 7);
    }

    #region 2x
    private static void LEFT_UP_2_2X(ref sPixel N3, ref sPixel N2, ref sPixel N1, sPixel PIXEL) {
      ALPHA_BLEND_224_W(ref N3, PIXEL);
      ALPHA_BLEND_64_W(ref N2, PIXEL);
      N1 = N2;
    }

    private static void LEFT_2_2X(ref sPixel N3, ref sPixel N2, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N3, PIXEL);
      ALPHA_BLEND_64_W(ref N2, PIXEL);
    }
    private static void UP_2_2X(ref sPixel N3, ref sPixel N1, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N3, PIXEL);
      ALPHA_BLEND_64_W(ref N1, PIXEL);
    }

    private static void DIA_2X(ref sPixel N3, sPixel PIXEL) {
      ALPHA_BLEND_128_W(ref N3, PIXEL);
    }

    private static void FILTRO_2X(sPixel PE, sPixel PI, sPixel PH, sPixel PF, sPixel PG, sPixel PC, sPixel PD, sPixel PB, sPixel F4, sPixel I4, sPixel H5, sPixel I5, ref sPixel N1, ref sPixel N2, ref sPixel N3) {
      var ex = (PE != PH && PE != PF);
      if (ex) {
        var e = (df(PE, PC) + df(PE, PG) + df(PI, H5) + df(PI, F4)) + (df(PH, PF) << 2);
        var i = (df(PH, PD) + df(PH, I5) + df(PF, I4) + df(PF, PB)) + (df(PE, PI) << 2);
        if ((e < i)
          && (!eq(PF, PB) && !eq(PH, PD) || eq(PE, PI) && (!eq(PF, I4) && !eq(PH, I5)) || eq(PE, PG) || eq(PE, PC))) {
          var ke = df(PF, PG);
          var ki = df(PH, PC);
          var ex2 = (PE != PC && PB != PC);
          var ex3 = (PE != PG && PD != PG);
          var px = (df(PE, PF) <= df(PE, PH)) ? PF : PH;
          if (((ke << 1) <= ki) && ex3 && (ke >= (ki << 1)) && ex2) {
            LEFT_UP_2_2X(ref N3, ref N2, ref N1, px);
          } else if (((ke << 1) <= ki) && ex3) {
            LEFT_2_2X(ref N3, ref N2, px);
          } else if ((ke >= (ki << 1)) && ex2) {
            UP_2_2X(ref N3, ref N1, px);
          } else {
            DIA_2X(ref N3, px);
          }
        } else if (e <= i) {
          ALPHA_BLEND_128_W(ref N3, ((df(PE, PF) <= df(PE, PH)) ? PF : PH));
        }
      }
    }
    #endregion
    #region 3x
    private static void LEFT_UP_2_3X(ref sPixel N7, ref sPixel N5, ref sPixel N6, ref sPixel N2, ref sPixel N8, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N7, PIXEL);
      ALPHA_BLEND_64_W(ref N6, PIXEL);
      N5 = N7;
      N2 = N6;
      N8 = PIXEL;
    }

    private static void LEFT_2_3X(ref sPixel N7, ref sPixel N5, ref sPixel N6, ref sPixel N8, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N7, PIXEL);
      ALPHA_BLEND_64_W(ref N5, PIXEL);
      ALPHA_BLEND_64_W(ref N6, PIXEL);
      N8 = PIXEL;
    }

    private static void UP_2_3X(ref sPixel N5, ref sPixel N7, ref sPixel N2, ref sPixel N8, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N5, PIXEL);
      ALPHA_BLEND_64_W(ref N7, PIXEL);
      ALPHA_BLEND_64_W(ref N2, PIXEL);
      N8 = PIXEL;
    }

    private static void DIA_3X(ref sPixel N8, ref sPixel N5, ref sPixel N7, sPixel PIXEL) {
      ALPHA_BLEND_224_W(ref N8, PIXEL);
      ALPHA_BLEND_32_W(ref N5, PIXEL);
      ALPHA_BLEND_32_W(ref N7, PIXEL);
    }

    private static void FILTRO_3X(sPixel PE, sPixel PI, sPixel PH, sPixel PF, sPixel PG, sPixel PC, sPixel PD, sPixel PB, sPixel F4, sPixel I4, sPixel H5, sPixel I5, ref sPixel N2, ref sPixel N5, ref sPixel N6, ref sPixel N7, ref sPixel N8) {
      var ex = (PE != PH && PE != PF);
      if (ex) {
        var e = (df(PE, PC) + df(PE, PG) + df(PI, H5) + df(PI, F4)) + (df(PH, PF) << 2);
        var i = (df(PH, PD) + df(PH, I5) + df(PF, I4) + df(PF, PB)) + (df(PE, PI) << 2);
        if ((e < i)
          &&
          (!eq(PF, PB) && !eq(PF, PC) || !eq(PH, PD) && !eq(PH, PG)
            || eq(PE, PI) && (!eq(PF, F4) && !eq(PF, I4) || !eq(PH, H5) && !eq(PH, I5)) || eq(PE, PG) || eq(PE, PC))) {
          var ke = df(PF, PG);
          var ki = df(PH, PC);
          var ex2 = (PE != PC && PB != PC);
          var ex3 = (PE != PG && PD != PG);
          var px = (df(PE, PF) <= df(PE, PH)) ? PF : PH;
          if (((ke << 1) <= ki) && ex3 && (ke >= (ki << 1)) && ex2) {
            LEFT_UP_2_3X(ref N7, ref N5, ref N6, ref N2, ref N8, px);
          } else if (((ke << 1) <= ki) && ex3) {
            LEFT_2_3X(ref N7, ref  N5, ref N6, ref N8, px);
          } else if ((ke >= (ki << 1)) && ex2) {
            UP_2_3X(ref N5, ref N7, ref N2, ref N8, px);
          } else {
            DIA_3X(ref N8, ref  N5, ref N7, px);
          }
        } else if (e <= i) {
          ALPHA_BLEND_128_W(ref N8, ((df(PE, PF) <= df(PE, PH)) ? PF : PH));
        }
      }
    }
    #endregion
    #region 4x
    private static void LEFT_UP_2(ref sPixel N15, ref sPixel N14, ref sPixel N11, ref sPixel N13, ref sPixel N12, ref sPixel N10, ref sPixel N7, ref sPixel N3, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N13, PIXEL);
      ALPHA_BLEND_64_W(ref N12, PIXEL);
      N15 = N14 = N11 = PIXEL;
      N10 = N3 = N12;
      N7 = N13;
    }

    private static void LEFT_2(ref sPixel N15, ref sPixel N14, ref sPixel N11, ref sPixel N13, ref sPixel N12, ref sPixel N10, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N11, PIXEL);
      ALPHA_BLEND_192_W(ref N13, PIXEL);
      ALPHA_BLEND_64_W(ref N10, PIXEL);
      ALPHA_BLEND_64_W(ref N12, PIXEL);
      N14 = PIXEL;
      N15 = PIXEL;
    }

    private static void UP_2(ref sPixel N15, ref sPixel N14, ref sPixel N11, ref sPixel N3, ref sPixel N7, ref sPixel N10, sPixel PIXEL) {
      ALPHA_BLEND_192_W(ref N14, PIXEL);
      ALPHA_BLEND_192_W(ref N7, PIXEL);
      ALPHA_BLEND_64_W(ref N10, PIXEL);
      ALPHA_BLEND_64_W(ref N3, PIXEL);
      N11 = PIXEL;
      N15 = PIXEL;
    }

    private static void DIA(ref sPixel N15, ref sPixel N14, ref sPixel N11, sPixel PIXEL) {
      ALPHA_BLEND_128_W(ref N11, PIXEL);
      ALPHA_BLEND_128_W(ref N14, PIXEL);
      N15 = PIXEL;
    }

    private static void FILTRO_4X(sPixel PE, sPixel PI, sPixel PH, sPixel PF, sPixel PG, sPixel PC, sPixel PD, sPixel PB, sPixel F4, sPixel I4, sPixel H5, sPixel I5, ref sPixel N15, ref sPixel N14, ref sPixel N11, ref sPixel N3, ref sPixel N7, ref sPixel N10, ref sPixel N13, ref sPixel N12) {
      var ex = (PE != PH && PE != PF);
      if (ex) {
        var e = (df(PE, PC) + df(PE, PG) + df(PI, H5) + df(PI, F4)) + (df(PH, PF) << 2);
        var i = (df(PH, PD) + df(PH, I5) + df(PF, I4) + df(PF, PB)) + (df(PE, PI) << 2);
        if ((e < i) && (!eq(PF, PB) && !eq(PH, PD) || eq(PE, PI) && (!eq(PF, I4) && !eq(PH, I5)) || eq(PE, PG) || eq(PE, PC))) {
          var ke = df(PF, PG);
          var ki = df(PH, PC);
          var ex2 = (PE != PC && PB != PC);
          var ex3 = (PE != PG && PD != PG);
          var px = (df(PE, PF) <= df(PE, PH)) ? PF : PH;
          if (((ke << 1) <= ki) && ex3 && (ke >= (ki << 1)) && ex2) {
            LEFT_UP_2(ref N15, ref  N14, ref N11, ref N13, ref N12, ref N10, ref N7, ref N3, px);
          } else if (((ke << 1) <= ki) && ex3) {
            LEFT_2(ref N15, ref N14, ref N11, ref N13, ref N12, ref N10, px);
          } else if ((ke >= (ki << 1)) && ex2) {
            UP_2(ref N15, ref N14, ref N11, ref N3, ref N7, ref N10, px);
          } else {
            DIA(ref N15, ref N14, ref N11, px);
          }
        } else if (e <= i) {
          ALPHA_BLEND_128_W(ref N15, ((df(PE, PF) <= df(PE, PH)) ? PF : PH));
        }
      }
    }
    #endregion
  }
}
