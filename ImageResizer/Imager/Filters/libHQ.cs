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
  static class libHQ {
    #region Common
    /// <summary>
    /// body for HQ2x etc.
    /// </summary>
    public static void ComplexFilter(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte scaleX, byte scaleY, object kernel) {
      var c0 = sourceImage[srcX - 1, srcY - 1];
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c2 = sourceImage[srcX + 1, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c6 = sourceImage[srcX - 1, srcY + 1];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      var c8 = sourceImage[srcX + 1, srcY + 1];
      byte pattern = 0;
      if ((c4.IsNotLike(c0)))
        pattern |= 1;
      if ((c4.IsNotLike(c1)))
        pattern |= 2;
      if ((c4.IsNotLike(c2)))
        pattern |= 4;
      if ((c4.IsNotLike(c3)))
        pattern |= 8;
      if ((c4.IsNotLike(c5)))
        pattern |= 16;
      if ((c4.IsNotLike(c6)))
        pattern |= 32;
      if ((c4.IsNotLike(c7)))
        pattern |= 64;
      if ((c4.IsNotLike(c8)))
        pattern |= 128;
      var result = ((cImage.sFilter.HqFilterKernel)kernel)(pattern, c0, c1, c2, c3, c4, c5, c6, c7, c8);
      byte offset = 0;
      for (byte y = 0; y < scaleY; y++)
        for (byte x = 0; x < scaleX; x++)
          targetImage[tgtX + x, tgtY + y] = result[offset++];
    } // end sub

    /// <summary>
    /// body for HQ2xBold etc. as seen in SNES9x
    /// </summary>
    public static void ComplexFilterBold(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte scaleX, byte scaleY, object kernel) {
      var c0 = sourceImage[srcX - 1, srcY - 1];
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c2 = sourceImage[srcX + 1, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c6 = sourceImage[srcX - 1, srcY + 1];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      var c8 = sourceImage[srcX + 1, srcY + 1];
      var brightness = new[] { 
        c0.Brightness,
        c1.Brightness,
        c2.Brightness,
        c3.Brightness,
        c4.Brightness,
        c5.Brightness,
        c6.Brightness,
        c7.Brightness,
        c8.Brightness
      };
      var avgBrightness = (byte)((
        brightness[0] +
        brightness[1] +
        brightness[2] +
        brightness[3] +
        brightness[4] +
        brightness[5] +
        brightness[6] +
        brightness[7] +
        brightness[8]
        ) / 9);
      var dc4 = c4.Brightness > avgBrightness;
      byte pattern = 0;
      if ((c4.IsNotLike(c0)) && ((brightness[0] > avgBrightness) != dc4))
        pattern |= 1;
      if ((c4.IsNotLike(c1)) && ((brightness[1] > avgBrightness) != dc4))
        pattern |= 2;
      if ((c4.IsNotLike(c2)) && ((brightness[2] > avgBrightness) != dc4))
        pattern |= 4;
      if ((c4.IsNotLike(c3)) && ((brightness[3] > avgBrightness) != dc4))
        pattern |= 8;
      if ((c4.IsNotLike(c5)) && ((brightness[5] > avgBrightness) != dc4))
        pattern |= 16;
      if ((c4.IsNotLike(c6)) && ((brightness[6] > avgBrightness) != dc4))
        pattern |= 32;
      if ((c4.IsNotLike(c7)) && ((brightness[7] > avgBrightness) != dc4))
        pattern |= 64;
      if ((c4.IsNotLike(c8)) && ((brightness[8] > avgBrightness) != dc4))
        pattern |= 128;
      var result = ((cImage.sFilter.HqFilterKernel)kernel)(pattern, c0, c1, c2, c3, c4, c5, c6, c7, c8);
      byte offset = 0;
      for (byte y = 0; y < scaleY; y++)
        for (byte x = 0; x < scaleX; x++)
          targetImage[tgtX + x, tgtY + y] = result[offset++];
    } // end sub

    /// <summary>
    /// body for HQ2xSmart etc. as seen in SNES9x
    /// </summary>
    public static void ComplexFilterSmart(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, byte scaleX, byte scaleY, object kernel) {
      var c0 = sourceImage[srcX - 1, srcY - 1];
      var c2 = sourceImage[srcX + 1, srcY - 1];
      var c4 = sourceImage[srcX, srcY];
      var c6 = sourceImage[srcX - 1, srcY + 1];
      var c8 = sourceImage[srcX + 1, srcY + 1];
      if (c0.IsLike(c4) || c2.IsLike(c4) || c6.IsLike(c4) || c8.IsLike(c4))
        ComplexFilter(sourceImage, srcX, srcY, targetImage, tgtX, tgtY, scaleX, scaleY, kernel);
      else
        ComplexFilterBold(sourceImage, srcX, srcY, targetImage, tgtX, tgtY, scaleX, scaleY, kernel);
    } // end sub
    #endregion

    #region filter casepathes

    #region standard HQ2x casepath
    public static sPixel[] Hq2xKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;
      switch (pattern) {
        #region HQ2x PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 10:
        case 138: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          e00 = c1.IsNotLike(c3) ? sPixel.Interpolate(c4, c0, 3, 1) : sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
        }
        break;
        case 11:
        case 139: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          e00 = c1.IsNotLike(c3) ? c4 : sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
        }
        break;
        case 14:
        case 142: {
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 3, 1);
            e01 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 3, 3, 2);
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          }
        }
        break;
        case 15:
        case 143: {
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 3, 3, 2);
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 18:
        case 50: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e01 = c1.IsNotLike(c5) ? sPixel.Interpolate(c4, c2, 3, 1) : sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
        }
        break;
        case 19:
        case 51: {
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e01 = sPixel.Interpolate(c1, c5, c4, 3, 3, 2);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 22:
        case 54: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e01 = c1.IsNotLike(c5) ? c4 : sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
        }
        break;
        case 23:
        case 55: {
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e01 = sPixel.Interpolate(c1, c5, c4, 3, 3, 2);
          }
        }
        break;
        case 24: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 25: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 26:
        case 31: {
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e00 = c1.IsNotLike(c3) ? c4 : sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = c1.IsNotLike(c5) ? c4 : sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
        }
        break;
        case 27: {
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e00 = c1.IsNotLike(c3) ? c4 : sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
        }
        break;
        case 28: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 29: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 30: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e01 = c1.IsNotLike(c5) ? c4 : sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 42:
        case 170: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 3, 3, 2);
            e10 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          }
        }
        break;
        case 43:
        case 171: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 3, 3, 2);
            e10 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          e00 = c1.IsNotLike(c3) ? sPixel.Interpolate(c4, c0, 3, 1) : sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
        }
        break;
        case 47:
        case 175: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
        }
        break;
        case 56: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 57: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 58: {
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 59: {
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 60: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 61: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
        }
        break;
        case 62: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 63: {
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, c8, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 66: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 67: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 70: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 71: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 72:
        case 76: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 73:
        case 77: {
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
            e10 = sPixel.Interpolate(c3, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 74:
        case 107: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 75: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 78: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 79: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 80:
        case 81: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 82:
        case 214: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 83: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 84:
        case 85: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e11 = sPixel.Interpolate(c4, c8, 3, 1);
          } else {
            e01 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
            e11 = sPixel.Interpolate(c5, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 86: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 87: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 88:
        case 248: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 89: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 90: {
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 91: {
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 92: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 93: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 94: {
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 95: {
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 98: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 99: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 102: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 103: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
        }
        break;
        case 104:
        case 108: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 105:
        case 109: {
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
            e10 = sPixel.Interpolate(c3, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 106: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 110: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 111: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, c8, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
        }
        break;
        case 112:
        case 113: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          if (c7.IsNotLike(c5)) {
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = sPixel.Interpolate(c4, c8, 3, 1);
          } else {
            e10 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
            e11 = sPixel.Interpolate(c5, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 114: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 115: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 116:
        case 117: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 118: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 119: {
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e01 = sPixel.Interpolate(c1, c5, c4, 3, 3, 2);
          }
        }
        break;
        case 120: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 121: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 122: {
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 123: {
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 124: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 125: {
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
            e10 = sPixel.Interpolate(c3, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 126: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 127: {
          e11 = sPixel.Interpolate(c4, c8, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 146:
        case 178: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 3, 1);
            e11 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e01 = sPixel.Interpolate(c1, c5, c4, 3, 3, 2);
            e11 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 150:
        case 182: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e01 = sPixel.Interpolate(c1, c5, c4, 3, 3, 2);
            e11 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          }
        }
        break;
        case 151:
        case 183: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 152: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 153: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 154: {
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 155: {
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 156: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 157: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 158: {
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 159: {
          e10 = sPixel.Interpolate(c4, c6, c7, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 184: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 185: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 186: {
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 187: {
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 3, 3, 2);
            e10 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          }
        }
        break;
        case 188: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 189: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 190: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e01 = sPixel.Interpolate(c1, c5, c4, 3, 3, 2);
            e11 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          }
        }
        break;
        case 191: {
          e10 = sPixel.Interpolate(c4, c7, 3, 1);
          e11 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 194: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 195: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 198: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 199: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 200:
        case 204: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          if (c7.IsNotLike(c3)) {
            e10 = sPixel.Interpolate(c4, c6, 3, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e10 = sPixel.Interpolate(c3, c7, c4, 3, 3, 2);
            e11 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
        }
        break;
        case 202: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 203: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 206: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 207: {
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 3, 3, 2);
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          }
        }
        break;
        case 208:
        case 209: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 210: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 211: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 212:
        case 213: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
            e11 = sPixel.Interpolate(c5, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 215: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c6, 2, 1, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 216: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 217: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 218: {
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 219: {
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 220: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 221: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
            e11 = sPixel.Interpolate(c5, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 222: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 223: {
          e10 = sPixel.Interpolate(c4, c6, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 226: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 227: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 230: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 231: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 232:
        case 236: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e10 = sPixel.Interpolate(c3, c7, c4, 3, 3, 2);
            e11 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 233:
        case 237: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
        }
        break;
        case 234: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 235: {
          e01 = sPixel.Interpolate(c4, c2, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 238: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e10 = sPixel.Interpolate(c3, c7, c4, 3, 3, 2);
            e11 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 239: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
        }
        break;
        case 240:
        case 241: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          if (c7.IsNotLike(c5)) {
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
            e11 = sPixel.Interpolate(c5, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 242: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 243: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          if (c7.IsNotLike(c5)) {
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
            e11 = sPixel.Interpolate(c5, c7, c4, 3, 3, 2);
          }
        }
        break;
        case 244:
        case 245: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
        }
        break;
        case 246: {
          e00 = sPixel.Interpolate(c4, c0, c3, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 247: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 249: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c2, 2, 1, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 250: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 251: {
          e01 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 252: {
          e00 = sPixel.Interpolate(c4, c0, c1, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
        }
        break;
        case 253: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
        }
        break;
        case 254: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 255: {
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, 
      e10, e11, 
      });
    }
    #endregion
    #region standard HQ2x3 casepath
    public static sPixel[] Hq2x3Kernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e10, e11, e20, e21;
      var e00 = e01 = e10 = e11 = e20 = e21 = c4;
      switch (pattern) {
        #region HQ2x3 PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 10:
        case 138: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 11:
        case 139: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = sPixel.Interpolate(c4, c5, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 10, 5, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 7, 6, 3);
            e10 = sPixel.Interpolate(c4, c3, 13, 3);
          }
        }
        break;
        case 15:
        case 143: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c5, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 10, 5, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 7, 6, 3);
            e10 = sPixel.Interpolate(c4, c3, 13, 3);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 18:
        case 50: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 13, 3);
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 7, 6, 3);
            e01 = sPixel.Interpolate(c1, c5, c4, 10, 5, 1);
            e11 = sPixel.Interpolate(c4, c5, 13, 3);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 22:
        case 54: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 23:
        case 55: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 7, 6, 3);
            e01 = sPixel.Interpolate(c1, c5, c4, 10, 5, 1);
            e11 = sPixel.Interpolate(c4, c5, 13, 3);
          }
        }
        break;
        case 24: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 25: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 26:
        case 31: {
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 27: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 28: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 29: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 30: {
          e10 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 42:
        case 170: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 5, 4);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
            e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          }
        }
        break;
        case 43:
        case 171: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 5, 4);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
            e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 47:
        case 175: {
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 56: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 57: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 58: {
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 59: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, c5, 10, 3, 3);
          }
          if ((c1.IsNotLike(c5) && c1.IsLike(c3))) {
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 60: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 61: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
        }
        break;
        case 62: {
          e10 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 63: {
          e10 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 66: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 67: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 70: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 71: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 72:
        case 76: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 73:
        case 77: {
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
            e20 = sPixel.Interpolate(c7, c3, c4, 7, 5, 4);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 74:
        case 107: {
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          }
        }
        break;
        case 75: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 78: {
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 79: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c5, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c5, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 80:
        case 81: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 82:
        case 214: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          if (c7.IsNotLike(c5)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
          }
        }
        break;
        case 83: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 84:
        case 85: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
            e11 = sPixel.Interpolate(c4, c5, 1, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c7, c5, c4, 7, 5, 4);
          }
        }
        break;
        case 86: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 87: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c3, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 88:
        case 248: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
          }
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 89: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
        }
        break;
        case 90: {
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 91: {
          e11 = c4;
          if (c1.IsLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, c5, 10, 3, 3);
          }
          if ((c1.IsNotLike(c5) && c1.IsLike(c3))) {
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
          }
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 92: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
        }
        break;
        case 93: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
        }
        break;
        case 94: {
          e10 = c4;
          if (c1.IsLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, c3, 10, 3, 3);
          }
          if ((c1.IsLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
          }
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 95: {
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 98: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 99: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 102: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 103: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
        }
        break;
        case 104:
        case 108: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 105:
        case 109: {
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
            e20 = sPixel.Interpolate(c7, c3, c4, 7, 5, 4);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 106: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 110: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 111: {
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 112:
        case 113: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c3, 13, 3);
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e11 = sPixel.Interpolate(c4, c5, 13, 3);
            e20 = sPixel.Interpolate(c7, c4, c3, 7, 6, 3);
            e21 = sPixel.Interpolate(c7, c5, c4, 10, 5, 1);
          }
        }
        break;
        case 114: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 115: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 116:
        case 117: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 13, 3)) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
        }
        break;
        case 118: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 119: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 7, 6, 3);
            e01 = sPixel.Interpolate(c1, c5, c4, 10, 5, 1);
            e11 = sPixel.Interpolate(c4, c5, 13, 3);
          }
        }
        break;
        case 120: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 121: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e11 = c4;
          if (c7.IsLike(c5)) {
            e21 = sPixel.Interpolate(c4, c5, c7, 10, 3, 3);
          }
          if ((c7.IsNotLike(c5) && c7.IsLike(c3))) {
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          }
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
          }
        }
        break;
        case 122: {
          e11 = c4;
          if (c7.IsLike(c5)) {
            e21 = sPixel.Interpolate(c4, c5, c7, 10, 3, 3);
          }
          if ((c7.IsNotLike(c5) && c7.IsLike(c3))) {
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          }
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
          }
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 123: {
          e10 = c4;
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          }
        }
        break;
        case 124: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 125: {
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
            e20 = sPixel.Interpolate(c7, c3, c4, 7, 5, 4);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
        }
        break;
        case 126: {
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 127: {
          if (c1.IsLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
          }
          if ((c1.IsNotLike(c5) && c1.IsLike(c3))) {
            e01 = sPixel.Interpolate(c4, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e01 = c4;
          }
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c8, c7, 12, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 10, 3, 3);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
          e11 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, 15, 1));
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 146:
        case 178: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e11 = c4;
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c1, c5, c4, 7, 5, 4);
            e11 = sPixel.Interpolate(c4, c5, 1, 1);
            e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 150:
        case 182: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c1, c5, c4, 7, 5, 4);
            e11 = sPixel.Interpolate(c4, c5, 1, 1);
            e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          }
        }
        break;
        case 151:
        case 183: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 152: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 153: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 154: {
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 155: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 156: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 157: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 158: {
          e10 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, c3, 10, 3, 3);
          }
          if ((c1.IsLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 159: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 184: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 185: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 186: {
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 187: {
          e11 = c4;
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 5, 4);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
            e20 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          }
        }
        break;
        case 188: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 189: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 190: {
          e10 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
            e11 = c4;
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c1, c5, c4, 7, 5, 4);
            e11 = sPixel.Interpolate(c4, c5, 1, 1);
            e21 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          }
        }
        break;
        case 191: {
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 194: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 195: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 198: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 199: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 200:
        case 204: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = sPixel.Interpolate(c4, c5, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 13, 3);
            e20 = sPixel.Interpolate(c7, c3, c4, 10, 5, 1);
            e21 = sPixel.Interpolate(c7, c4, c5, 7, 6, 3);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
        }
        break;
        case 202: {
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 203: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 206: {
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 13, 3)) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 207: {
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c5, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 10, 5, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 7, 6, 3);
            e10 = sPixel.Interpolate(c4, c3, 13, 3);
          }
        }
        break;
        case 208:
        case 209: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 210: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 211: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 212:
        case 213: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
            e11 = sPixel.Interpolate(c4, c5, 1, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c7, c5, c4, 7, 5, 4);
          }
        }
        break;
        case 215: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          if (c7.IsNotLike(c5)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 216: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 217: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 218: {
          e10 = c4;
          if (c7.IsLike(c3)) {
            e20 = sPixel.Interpolate(c4, c3, c7, 10, 3, 3);
          }
          if ((c7.IsLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
          }
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 219: {
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 220: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          if (c7.IsLike(c3)) {
            e20 = sPixel.Interpolate(c4, c3, c7, 10, 3, 3);
          }
          if ((c7.IsLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
          }
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 221: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
            e11 = sPixel.Interpolate(c4, c5, 1, 1);
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c7, c5, c4, 7, 5, 4);
          }
        }
        break;
        case 222: {
          e10 = c4;
          e11 = c4;
          if (c7.IsNotLike(c5)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
          }
        }
        break;
        case 223: {
          if (c1.IsLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
          }
          if ((c1.IsLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c4, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e00 = c4;
          }
          if (c7.IsNotLike(c5)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c6, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          e10 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, 15, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 10, 3, 3);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 226: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 227: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 230: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 231: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
        }
        break;
        case 232:
        case 236: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c5, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 13, 3);
            e20 = sPixel.Interpolate(c7, c3, c4, 10, 5, 1);
            e21 = sPixel.Interpolate(c7, c4, c5, 7, 6, 3);
          }
        }
        break;
        case 233:
        case 237: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
        }
        break;
        case 234: {
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c5, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c5, c7, 12, 3, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 13, 3)) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 235: {
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          }
        }
        break;
        case 238: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = sPixel.Interpolate(c4, c5, 13, 3);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 13, 3);
            e20 = sPixel.Interpolate(c7, c3, c4, 10, 5, 1);
            e21 = sPixel.Interpolate(c7, c4, c5, 7, 6, 3);
          }
        }
        break;
        case 239: {
          e01 = sPixel.Interpolate(c4, c5, 13, 3);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 13, 3);
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 240:
        case 241: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c3, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 13, 3);
            e20 = sPixel.Interpolate(c7, c4, c3, 7, 6, 3);
            e21 = sPixel.Interpolate(c7, c5, c4, 10, 5, 1);
          }
        }
        break;
        case 242: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c3, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 12, 3, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          e01 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 13, 3)) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 243: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c3, 13, 3);
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 13, 3);
            e20 = sPixel.Interpolate(c7, c4, c3, 7, 6, 3);
            e21 = sPixel.Interpolate(c7, c5, c4, 10, 5, 1);
          }
        }
        break;
        case 244:
        case 245: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
        }
        break;
        case 246: {
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
          }
        }
        break;
        case 247: {
          e00 = sPixel.Interpolate(c4, c3, 13, 3);
          e10 = sPixel.Interpolate(c4, c3, 13, 3);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 13, 3);
          e21 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 249: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          e10 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 250: {
          e00 = sPixel.Interpolate(c4, c0, 13, 3);
          e01 = sPixel.Interpolate(c4, c2, 13, 3);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
          }
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 251: {
          if (c7.IsLike(c5)) {
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          if ((c7.IsNotLike(c5) && c7.IsLike(c3))) {
            e21 = sPixel.Interpolate(c4, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e21 = c4;
          }
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 10, 3, 3);
          }
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, 15, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c2, c1, 12, 3, 1);
          }
        }
        break;
        case 252: {
          e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
          }
          e21 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
        }
        break;
        case 253: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
        }
        break;
        case 254: {
          if (c7.IsLike(c3)) {
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
          }
          if ((c7.IsLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c4, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e20 = c4;
          }
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, 15, 1));
          if (c7.IsNotLike(c5)) {
            e11 = c4;
            e21 = c4;
          } else {
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 10, 3, 3);
          }
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c0, 13, 3);
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c0, c1, 12, 3, 1);
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
          }
        }
        break;
        case 255: {
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, 
      e10, e11, 
      e20, e21, 
      });
    }
    #endregion
    #region standard HQ2x4 casepath
    public static sPixel[] Hq2x4Kernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e10, e11, e20, e21, e30, e31;
      var e00 = e01 = e10 = e11 = e20 = e21 = e30 = e31 = c4;
      switch (pattern) {
        #region HQ2x4 PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 10:
        case 138: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 11:
        case 139: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e01 = sPixel.Interpolate(c4, c5, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 9, 7);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, c1, 8, 5, 3);
          }
        }
        break;
        case 15:
        case 143: {
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c5, 3, 1);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 9, 7);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, c1, 8, 5, 3);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 18:
        case 50: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c4, 1, 1);
            e01 = sPixel.Interpolate(c1, c5, 9, 7);
            e11 = sPixel.Interpolate(c4, c5, c1, 8, 5, 3);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 22:
        case 54: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 23:
        case 55: {
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c4, 1, 1);
            e01 = sPixel.Interpolate(c1, c5, 9, 7);
            e11 = sPixel.Interpolate(c4, c5, c1, 8, 5, 3);
          }
        }
        break;
        case 24: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 25: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 26:
        case 31: {
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 27: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 28: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 29: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 30: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
        }
        break;
        case 42:
        case 170: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
            e20 = sPixel.Interpolate(c4, c7, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 4, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, c1, 3, 3, 2);
            e20 = sPixel.Interpolate(c4, c3, c7, 9, 6, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 11, 3, 2);
          }
        }
        break;
        case 43:
        case 171: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 4, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, c1, 3, 3, 2);
            e20 = sPixel.Interpolate(c4, c3, c7, 9, 6, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 11, 3, 2);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 47:
        case 175: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c5, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c5, 9, 4, 3);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 56: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 57: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 58: {
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 59: {
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 60: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 61: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
        }
        break;
        case 62: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 63: {
          e10 = c4;
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c8, c7, 5, 2, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 66: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 67: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 70: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 71: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 72:
        case 76: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 73:
        case 77: {
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 5, 3);
            e10 = sPixel.Interpolate(c4, c1, 7, 1);
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 11, 3, 2);
            e10 = sPixel.Interpolate(c4, c3, c1, 9, 6, 1);
            e20 = sPixel.Interpolate(c3, c4, c7, 3, 3, 2);
            e30 = sPixel.Interpolate(c7, c3, c4, 4, 3, 1);
          }
        }
        break;
        case 74:
        case 107: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 75: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 78: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 79: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 80:
        case 81: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 82:
        case 214: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 83: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 84:
        case 85: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 5, 3);
            e11 = sPixel.Interpolate(c4, c1, 7, 1);
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 11, 3, 2);
            e11 = sPixel.Interpolate(c4, c5, c1, 9, 6, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 3, 3, 2);
            e31 = sPixel.Interpolate(c7, c5, c4, 4, 3, 1);
          }
        }
        break;
        case 86: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 87: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 88:
        case 248: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 89: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 90: {
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 91: {
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 92: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 93: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 94: {
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 95: {
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 98: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 99: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 102: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 103: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
        }
        break;
        case 104:
        case 108: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 105:
        case 109: {
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 5, 3);
            e10 = sPixel.Interpolate(c4, c1, 7, 1);
            e20 = c4;
            e30 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 11, 3, 2);
            e10 = sPixel.Interpolate(c4, c3, c1, 9, 6, 1);
            e20 = sPixel.Interpolate(c3, c4, c7, 3, 3, 2);
            e30 = sPixel.Interpolate(c7, c3, c4, 4, 3, 1);
          }
        }
        break;
        case 106: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 110: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 111: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, c8, 6, 1, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 112:
        case 113: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e30 = sPixel.Interpolate(c4, c3, 3, 1);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c4, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c5, 9, 7);
          }
        }
        break;
        case 114: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 115: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 116:
        case 117: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 118: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 119: {
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c4, 1, 1);
            e01 = sPixel.Interpolate(c1, c5, 9, 7);
            e11 = sPixel.Interpolate(c4, c5, c1, 8, 5, 3);
          }
        }
        break;
        case 120: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 121: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 122: {
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = sPixel.Interpolate(c4, c8, 13, 3);
            e31 = sPixel.Interpolate(c4, c8, 11, 5);
          } else {
            e21 = sPixel.Interpolate(c4, c5, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 123: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 124: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 125: {
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 5, 3);
            e10 = sPixel.Interpolate(c4, c1, 7, 1);
            e20 = c4;
            e30 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 11, 3, 2);
            e10 = sPixel.Interpolate(c4, c3, c1, 9, 6, 1);
            e20 = sPixel.Interpolate(c3, c4, c7, 3, 3, 2);
            e30 = sPixel.Interpolate(c7, c3, c4, 4, 3, 1);
          }
        }
        break;
        case 126: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 127: {
          e10 = c4;
          e21 = sPixel.Interpolate(c4, c8, 13, 3);
          e31 = sPixel.Interpolate(c4, c8, 11, 5);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 146:
        case 178: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e01 = sPixel.Interpolate(c1, c5, c4, 4, 3, 1);
            e11 = sPixel.Interpolate(c4, c5, c1, 3, 3, 2);
            e21 = sPixel.Interpolate(c4, c5, c7, 9, 6, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 11, 3, 2);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 150:
        case 182: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e01 = sPixel.Interpolate(c1, c5, c4, 4, 3, 1);
            e11 = sPixel.Interpolate(c4, c5, c1, 3, 3, 2);
            e21 = sPixel.Interpolate(c4, c5, c7, 9, 6, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 11, 3, 2);
          }
        }
        break;
        case 151:
        case 183: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, c3, 9, 4, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 152: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 153: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 154: {
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 155: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 156: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 157: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 158: {
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 159: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 184: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 185: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 186: {
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 187: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 4, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, c1, 3, 3, 2);
            e20 = sPixel.Interpolate(c4, c3, c7, 9, 6, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 11, 3, 2);
          }
        }
        break;
        case 188: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 189: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 190: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e31 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e01 = sPixel.Interpolate(c1, c5, c4, 4, 3, 1);
            e11 = sPixel.Interpolate(c4, c5, c1, 3, 3, 2);
            e21 = sPixel.Interpolate(c4, c5, c7, 9, 6, 1);
            e31 = sPixel.Interpolate(c4, c7, c5, 11, 3, 2);
          }
        }
        break;
        case 191: {
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 194: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 195: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 198: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 199: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 200:
        case 204: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
            e31 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c7, c3, 9, 7);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
        }
        break;
        case 202: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 203: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 206: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 207: {
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = sPixel.Interpolate(c4, c5, 3, 1);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 9, 7);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, c1, 8, 5, 3);
          }
        }
        break;
        case 208:
        case 209: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 210: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 211: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 212:
        case 213: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 5, 3);
            e11 = sPixel.Interpolate(c4, c1, 7, 1);
            e21 = c4;
            e31 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 11, 3, 2);
            e11 = sPixel.Interpolate(c4, c5, c1, 9, 6, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 3, 3, 2);
            e31 = sPixel.Interpolate(c7, c5, c4, 4, 3, 1);
          }
        }
        break;
        case 215: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c6, 6, 1, 1);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 216: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 217: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 218: {
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 219: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 220: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 13, 3);
            e30 = sPixel.Interpolate(c4, c6, 11, 5);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 221: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, 5, 3);
            e11 = sPixel.Interpolate(c4, c1, 7, 1);
            e21 = c4;
            e31 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 11, 3, 2);
            e11 = sPixel.Interpolate(c4, c5, c1, 9, 6, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 3, 3, 2);
            e31 = sPixel.Interpolate(c7, c5, c4, 4, 3, 1);
          }
        }
        break;
        case 222: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 223: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 13, 3);
          e30 = sPixel.Interpolate(c4, c6, 11, 5);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 226: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 227: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 230: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 231: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 232:
        case 236: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c7, c3, 9, 7);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 233:
        case 237: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c5, 9, 4, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c5, c1, 11, 3, 2);
          e20 = c4;
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
        }
        break;
        case 234: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 11, 5);
            e10 = sPixel.Interpolate(c4, c0, 13, 3);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 235: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, c5, 6, 1, 1);
          e20 = c4;
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 238: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c7, c3, 9, 7);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 239: {
          e01 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = c4;
          e21 = sPixel.Interpolate(c4, c5, 3, 1);
          e31 = sPixel.Interpolate(c4, c5, 3, 1);
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 240:
        case 241: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, 3, 1);
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c4, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c5, 9, 7);
          }
        }
        break;
        case 242: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = sPixel.Interpolate(c4, c2, 11, 5);
            e11 = sPixel.Interpolate(c4, c2, 13, 3);
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 243: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, 3, 1);
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c4, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c5, 9, 7);
          }
        }
        break;
        case 244:
        case 245: {
          e00 = sPixel.Interpolate(c4, c1, c3, 9, 4, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 11, 3, 2);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 246: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, c3, 6, 1, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 247: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e30 = sPixel.Interpolate(c4, c3, 3, 1);
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 249: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c2, c1, 5, 2, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 250: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
        }
        break;
        case 251: {
          e01 = sPixel.Interpolate(c4, c2, 11, 5);
          e11 = sPixel.Interpolate(c4, c2, 13, 3);
          e20 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 252: {
          e00 = sPixel.Interpolate(c4, c0, c1, 5, 2, 1);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e21 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 253: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = c4;
          e21 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
        }
        break;
        case 254: {
          e00 = sPixel.Interpolate(c4, c0, 11, 5);
          e10 = sPixel.Interpolate(c4, c0, 13, 3);
          e21 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 255: {
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, 
      e10, e11, 
      e20, e21, 
      e30, e31, 
      });
    }
    #endregion
    #region standard HQ3x casepath
    public static sPixel[] Hq3xKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e02, e10, e11, e12, e20, e21, e22;
      var e00 = e01 = e02 = e10 = e11 = e12 = e20 = e21 = e22 = c4;
      switch (pattern) {
        #region HQ3x PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 10:
        case 138: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 3, 1);
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 11:
        case 139: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 3, 1);
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c5, 3, 1);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 15:
        case 143: {
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c5, 3, 1);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 18:
        case 50: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e12 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 22:
        case 54: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 23:
        case 55: {
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 24: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 25: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 26:
        case 31: {
          e01 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e12 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 27: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 28: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 29: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 30: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 42:
        case 170: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 3, 1);
            e01 = c4;
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          }
        }
        break;
        case 43:
        case 171: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 47:
        case 175: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 56: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 57: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 58: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 59: {
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 60: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 61: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 62: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 63: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e12 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 66: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 67: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 70: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 71: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 72:
        case 76: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
        }
        break;
        case 73:
        case 77: {
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
          }
        }
        break;
        case 74:
        case 107: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
          }
        }
        break;
        case 75: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 78: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 79: {
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 80:
        case 81: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = sPixel.Interpolate(c4, c8, 3, 1);
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 82:
        case 214: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e22 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
          }
        }
        break;
        case 83: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 84:
        case 85: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e12 = c4;
            e21 = c4;
            e22 = sPixel.Interpolate(c4, c8, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = sPixel.Interpolate(c5, c4, 3, 1);
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 86: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 87: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 88:
        case 248: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e21 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
          }
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 89: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 90: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 91: {
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 92: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 93: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 94: {
          e10 = c4;
          e11 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 95: {
          e01 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e12 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 98: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 99: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 102: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 103: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
        }
        break;
        case 104:
        case 108: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
        }
        break;
        case 105:
        case 109: {
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
          }
        }
        break;
        case 106: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
        }
        break;
        case 110: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
        }
        break;
        case 111: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 112:
        case 113: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e22 = sPixel.Interpolate(c4, c8, 3, 1);
          } else {
            e12 = sPixel.Interpolate(c4, c5, 3, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c7, c4, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 114: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 115: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 116:
        case 117: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 118: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 119: {
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 120: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
        }
        break;
        case 121: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 122: {
          e01 = c4;
          e11 = c4;
          e12 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          e22 = (c7.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c8, 3, 1)) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 123: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
          }
        }
        break;
        case 124: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = c4;
          e12 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
        }
        break;
        case 125: {
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = c4;
          e12 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
          }
        }
        break;
        case 126: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 127: {
          e11 = c4;
          e22 = sPixel.Interpolate(c4, c8, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e12 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 146:
        case 178: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e12 = c4;
            e22 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c5, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 150:
        case 182: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
            e22 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c5, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 151:
        case 183: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 152: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 153: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 154: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 155: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 156: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 157: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 158: {
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 159: {
          e01 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 184: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 185: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 186: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 187: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = c4;
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          }
        }
        break;
        case 188: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 189: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
        }
        break;
        case 190: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
            e22 = sPixel.Interpolate(c4, c7, 3, 1);
          } else {
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c5, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 191: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 3, 1);
          e22 = sPixel.Interpolate(c4, c7, 3, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 194: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 195: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 198: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 199: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 200:
        case 204: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = c4;
            e22 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c7, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 202: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 203: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 206: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 207: {
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c5, 3, 1);
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 208:
        case 209: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 210: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 211: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 212:
        case 213: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = sPixel.Interpolate(c5, c4, 3, 1);
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 215: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e22 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 216: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 217: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 218: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 219: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 220: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c6, 3, 1)) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 221: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = sPixel.Interpolate(c5, c4, 3, 1);
            e21 = sPixel.Interpolate(c4, c7, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 222: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e22 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
          }
        }
        break;
        case 223: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e22 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 226: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 227: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 230: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 231: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
        }
        break;
        case 232:
        case 236: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
            e22 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c7, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 233:
        case 237: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 234: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (sPixel.Interpolate(c4, c0, 3, 1)) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 235: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
          }
        }
        break;
        case 238: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
            e22 = sPixel.Interpolate(c4, c5, 3, 1);
          } else {
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c7, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 239: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 3, 1);
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 240:
        case 241: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 3, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c7, c4, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 242: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          e02 = (c1.IsNotLike(c5)) ? (sPixel.Interpolate(c4, c2, 3, 1)) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 243: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 3, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c7, c4, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 244:
        case 245: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 246: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
          }
        }
        break;
        case 247: {
          e00 = sPixel.Interpolate(c4, c3, 3, 1);
          e01 = c4;
          e10 = sPixel.Interpolate(c4, c3, 3, 1);
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 3, 1);
          e21 = c4;
          e22 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 249: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e10 = c4;
          e11 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 250: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          e21 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
          }
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
        }
        break;
        case 251: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
            e21 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
          }
        }
        break;
        case 252: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e11 = c4;
          e12 = c4;
          e21 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
          }
          e22 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 253: {
          e00 = sPixel.Interpolate(c4, c1, 3, 1);
          e01 = sPixel.Interpolate(c4, c1, 3, 1);
          e02 = sPixel.Interpolate(c4, c1, 3, 1);
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 254: {
          e00 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e10 = c4;
            e20 = c4;
          } else {
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
          }
          if (c7.IsNotLike(c5)) {
            e12 = c4;
            e21 = c4;
            e22 = c4;
          } else {
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
          }
        }
        break;
        case 255: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, e02, 
      e10, e11, e12, 
      e20, e21, e22, 
      });
    }
    #endregion
    #region standard HQ4x casepath
    public static sPixel[] Hq4xKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e02, e03, e10, e11, e12, e13, e20, e21, e22, e23, e30, e31, e32, e33;
      var e00 = e01 = e02 = e03 = e10 = e11 = e12 = e13 = e20 = e21 = e22 = e23 = e30 = e31 = e32 = e33 = c4;
      switch (pattern) {
        #region HQ4x PATTERNS

        case 0:
        case 1:
        case 4:
        case 5:
        case 32:
        case 33:
        case 36:
        case 37:
        case 128:
        case 129:
        case 132:
        case 133:
        case 160:
        case 161:
        case 164:
        case 165: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 2:
        case 34:
        case 130:
        case 162: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 3:
        case 35:
        case 131:
        case 163: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 6:
        case 38:
        case 134:
        case 166: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 7:
        case 39:
        case 135:
        case 167: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 8:
        case 12:
        case 136:
        case 140: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 9:
        case 13:
        case 137:
        case 141: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 10:
        case 138: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
            e11 = c4;
          }
        }
        break;
        case 11:
        case 139: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e02 = sPixel.Interpolate(c4, c5, 7, 1);
            e03 = sPixel.Interpolate(c4, c5, 5, 3);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, 5, 3);
            e02 = sPixel.Interpolate(c1, c4, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c1, c4, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          }
        }
        break;
        case 15:
        case 143: {
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c5, 7, 1);
            e03 = sPixel.Interpolate(c4, c5, 5, 3);
            e10 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, 5, 3);
            e02 = sPixel.Interpolate(c1, c4, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c1, c4, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          }
        }
        break;
        case 16:
        case 17:
        case 48:
        case 49: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 18:
        case 50: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 5, 3);
            e01 = sPixel.Interpolate(c4, c3, 7, 1);
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 5, 3);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, c4, 2, 1, 1);
          }
        }
        break;
        case 20:
        case 21:
        case 52:
        case 53: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 22:
        case 54: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 23:
        case 55: {
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 5, 3);
            e01 = sPixel.Interpolate(c4, c3, 7, 1);
            e02 = c4;
            e03 = c4;
            e12 = c4;
            e13 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 5, 3);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, c4, 2, 1, 1);
          }
        }
        break;
        case 24: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 25: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 26:
        case 31: {
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 27: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 28: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 29: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 30: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 40:
        case 44:
        case 168:
        case 172: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 41:
        case 45:
        case 169:
        case 173: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
        }
        break;
        case 42:
        case 170: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
            e20 = sPixel.Interpolate(c4, c7, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c1, 5, 3);
            e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
            e20 = sPixel.Interpolate(c3, c4, 3, 1);
            e30 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 43:
        case 171: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c7, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c1, 5, 3);
            e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
            e20 = sPixel.Interpolate(c3, c4, 3, 1);
            e30 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
        }
        break;
        case 47:
        case 175: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
          e23 = sPixel.Interpolate(c4, c5, c7, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c5, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 56: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 57: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 58: {
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 59: {
          e11 = c4;
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 60: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 61: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 62: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 63: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, c8, 5, 2, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 64:
        case 65:
        case 68:
        case 69: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 66: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 67: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 70: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 71: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 72:
        case 76: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 73:
        case 77: {
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 5, 3);
            e10 = sPixel.Interpolate(c4, c1, 7, 1);
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 5, 3);
            e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 74:
        case 107: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 75: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 78: {
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
        }
        break;
        case 79: {
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 80:
        case 81: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 82:
        case 214: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 83: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 84:
        case 85: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e03 = sPixel.Interpolate(c4, c1, 5, 3);
            e13 = sPixel.Interpolate(c4, c1, 7, 1);
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e03 = sPixel.Interpolate(c4, c5, 3, 1);
            e13 = sPixel.Interpolate(c5, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c7, 5, 3);
            e32 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 86: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 87: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 88:
        case 248: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e21 = c4;
          e22 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 89: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 90: {
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 91: {
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 92: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 93: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 94: {
          e12 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 95: {
          e11 = c4;
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 96:
        case 97:
        case 100:
        case 101: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 98: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 99: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 102: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 103: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
        }
        break;
        case 104:
        case 108: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 105:
        case 109: {
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 5, 3);
            e10 = sPixel.Interpolate(c4, c1, 7, 1);
            e20 = c4;
            e21 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 5, 3);
            e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 106: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 110: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 111: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, c8, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 112:
        case 113: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e30 = sPixel.Interpolate(c4, c3, 5, 3);
            e31 = sPixel.Interpolate(c4, c3, 7, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c4, c7, 2, 1, 1);
            e30 = sPixel.Interpolate(c4, c7, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, 3, 1);
            e32 = sPixel.Interpolate(c7, c5, 5, 3);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 114: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 115: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 116:
        case 117: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 118: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 119: {
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c1.IsNotLike(c5)) {
            e00 = sPixel.Interpolate(c4, c3, 5, 3);
            e01 = sPixel.Interpolate(c4, c3, 7, 1);
            e02 = c4;
            e03 = c4;
            e12 = c4;
            e13 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 5, 3);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, c4, 2, 1, 1);
          }
        }
        break;
        case 120: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 121: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e21 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 122: {
          e21 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e22 = sPixel.Interpolate(c4, c8, 7, 1);
            e23 = sPixel.Interpolate(c4, c8, 3, 1);
            e32 = sPixel.Interpolate(c4, c8, 3, 1);
            e33 = sPixel.Interpolate(c4, c8, 5, 3);
          } else {
            e22 = c4;
            e23 = sPixel.Interpolate(c4, c5, 3, 1);
            e32 = sPixel.Interpolate(c4, c7, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 123: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 124: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
        }
        break;
        case 125: {
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, 5, 3);
            e10 = sPixel.Interpolate(c4, c1, 7, 1);
            e20 = c4;
            e21 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c3, 3, 1);
            e10 = sPixel.Interpolate(c3, c4, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 5, 3);
            e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
        }
        break;
        case 126: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 127: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c8, 7, 1);
          e23 = sPixel.Interpolate(c4, c8, 3, 1);
          e32 = sPixel.Interpolate(c4, c8, 3, 1);
          e33 = sPixel.Interpolate(c4, c8, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 144:
        case 145:
        case 176:
        case 177: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 146:
        case 178: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
            e23 = sPixel.Interpolate(c4, c7, 7, 1);
            e33 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e02 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, 5, 3);
            e23 = sPixel.Interpolate(c5, c4, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 148:
        case 149:
        case 180:
        case 181: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 150:
        case 182: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e12 = c4;
            e13 = c4;
            e23 = sPixel.Interpolate(c4, c7, 7, 1);
            e33 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e02 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, 5, 3);
            e23 = sPixel.Interpolate(c5, c4, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 151:
        case 183: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = c4;
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = c4;
          e13 = c4;
          e20 = sPixel.Interpolate(c4, c3, c7, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
          e31 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 152: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 153: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 154: {
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 155: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 156: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 157: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 158: {
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 159: {
          e02 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, c6, 5, 2, 1);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 184: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 185: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 186: {
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 187: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
            e11 = c4;
            e20 = sPixel.Interpolate(c4, c7, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c1, 5, 3);
            e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
            e20 = sPixel.Interpolate(c3, c4, 3, 1);
            e30 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 188: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 189: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
        }
        break;
        case 190: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e12 = c4;
            e13 = c4;
            e23 = sPixel.Interpolate(c4, c7, 7, 1);
            e33 = sPixel.Interpolate(c4, c7, 5, 3);
          } else {
            e02 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, 5, 3);
            e23 = sPixel.Interpolate(c5, c4, 3, 1);
            e33 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 191: {
          e01 = c4;
          e02 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = sPixel.Interpolate(c4, c7, 7, 1);
          e21 = sPixel.Interpolate(c4, c7, 7, 1);
          e22 = sPixel.Interpolate(c4, c7, 7, 1);
          e23 = sPixel.Interpolate(c4, c7, 7, 1);
          e30 = sPixel.Interpolate(c4, c7, 5, 3);
          e31 = sPixel.Interpolate(c4, c7, 5, 3);
          e32 = sPixel.Interpolate(c4, c7, 5, 3);
          e33 = sPixel.Interpolate(c4, c7, 5, 3);
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 192:
        case 193:
        case 196:
        case 197: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 194: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 195: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 198: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 199: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 200:
        case 204: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
            e32 = sPixel.Interpolate(c4, c5, 7, 1);
            e33 = sPixel.Interpolate(c4, c5, 5, 3);
          } else {
            e20 = sPixel.Interpolate(c3, c4, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c3, 5, 3);
            e32 = sPixel.Interpolate(c7, c4, 3, 1);
            e33 = sPixel.Interpolate(c4, c7, 3, 1);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
        }
        break;
        case 202: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
        }
        break;
        case 203: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 206: {
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
        }
        break;
        case 207: {
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e02 = sPixel.Interpolate(c4, c5, 7, 1);
            e03 = sPixel.Interpolate(c4, c5, 5, 3);
            e10 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, 5, 3);
            e02 = sPixel.Interpolate(c1, c4, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c1, c4, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          }
        }
        break;
        case 208:
        case 209: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 210: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 211: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 212:
        case 213: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e03 = sPixel.Interpolate(c4, c1, 5, 3);
            e13 = sPixel.Interpolate(c4, c1, 7, 1);
            e22 = c4;
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e03 = sPixel.Interpolate(c4, c5, 3, 1);
            e13 = sPixel.Interpolate(c5, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c7, 5, 3);
            e32 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 215: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = c4;
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = c4;
          e13 = c4;
          e20 = sPixel.Interpolate(c4, c3, c6, 5, 2, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 216: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 217: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 218: {
          e22 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 219: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 220: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e22 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = sPixel.Interpolate(c4, c6, 3, 1);
            e21 = sPixel.Interpolate(c4, c6, 7, 1);
            e30 = sPixel.Interpolate(c4, c6, 5, 3);
            e31 = sPixel.Interpolate(c4, c6, 3, 1);
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e21 = c4;
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 221: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e03 = sPixel.Interpolate(c4, c1, 5, 3);
            e13 = sPixel.Interpolate(c4, c1, 7, 1);
            e22 = c4;
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e03 = sPixel.Interpolate(c4, c5, 3, 1);
            e13 = sPixel.Interpolate(c5, c4, 3, 1);
            e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c7, 5, 3);
            e32 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 222: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 223: {
          e02 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = sPixel.Interpolate(c4, c6, 3, 1);
          e21 = sPixel.Interpolate(c4, c6, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c6, 5, 3);
          e31 = sPixel.Interpolate(c4, c6, 3, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 224:
        case 225:
        case 228:
        case 229: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 226: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 227: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 230: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 231: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
        }
        break;
        case 232:
        case 236: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
            e30 = c4;
            e31 = c4;
            e32 = sPixel.Interpolate(c4, c5, 7, 1);
            e33 = sPixel.Interpolate(c4, c5, 5, 3);
          } else {
            e20 = sPixel.Interpolate(c3, c4, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c3, 5, 3);
            e32 = sPixel.Interpolate(c7, c4, 3, 1);
            e33 = sPixel.Interpolate(c4, c7, 3, 1);
          }
        }
        break;
        case 233:
        case 237: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c5, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, c5, 6, 1, 1);
          e13 = sPixel.Interpolate(c4, c5, c1, 5, 2, 1);
          e20 = c4;
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e31 = c4;
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
        }
        break;
        case 234: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = sPixel.Interpolate(c4, c0, 5, 3);
            e01 = sPixel.Interpolate(c4, c0, 3, 1);
            e10 = sPixel.Interpolate(c4, c0, 3, 1);
            e11 = sPixel.Interpolate(c4, c0, 7, 1);
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
            e11 = c4;
          }
        }
        break;
        case 235: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, c2, 5, 2, 1);
          e20 = c4;
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e31 = c4;
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 238: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
            e30 = c4;
            e31 = c4;
            e32 = sPixel.Interpolate(c4, c5, 7, 1);
            e33 = sPixel.Interpolate(c4, c5, 5, 3);
          } else {
            e20 = sPixel.Interpolate(c3, c4, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c3, 5, 3);
            e32 = sPixel.Interpolate(c7, c4, 3, 1);
            e33 = sPixel.Interpolate(c4, c7, 3, 1);
          }
        }
        break;
        case 239: {
          e01 = c4;
          e02 = sPixel.Interpolate(c4, c5, 7, 1);
          e03 = sPixel.Interpolate(c4, c5, 5, 3);
          e10 = c4;
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c5, 7, 1);
          e13 = sPixel.Interpolate(c4, c5, 5, 3);
          e20 = c4;
          e21 = c4;
          e22 = sPixel.Interpolate(c4, c5, 7, 1);
          e23 = sPixel.Interpolate(c4, c5, 5, 3);
          e31 = c4;
          e32 = sPixel.Interpolate(c4, c5, 7, 1);
          e33 = sPixel.Interpolate(c4, c5, 5, 3);
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 240:
        case 241: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          if (c7.IsNotLike(c5)) {
            e22 = c4;
            e23 = c4;
            e30 = sPixel.Interpolate(c4, c3, 5, 3);
            e31 = sPixel.Interpolate(c4, c3, 7, 1);
            e32 = c4;
            e33 = c4;
          } else {
            e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c4, c7, 2, 1, 1);
            e30 = sPixel.Interpolate(c4, c7, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, 3, 1);
            e32 = sPixel.Interpolate(c7, c5, 5, 3);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 242: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = c4;
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = sPixel.Interpolate(c4, c2, 3, 1);
            e03 = sPixel.Interpolate(c4, c2, 5, 3);
            e12 = sPixel.Interpolate(c4, c2, 7, 1);
            e13 = sPixel.Interpolate(c4, c2, 3, 1);
          } else {
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = c4;
            e13 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 243: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          if (c7.IsNotLike(c5)) {
            e22 = c4;
            e23 = c4;
            e30 = sPixel.Interpolate(c4, c3, 5, 3);
            e31 = sPixel.Interpolate(c4, c3, 7, 1);
            e32 = c4;
            e33 = c4;
          } else {
            e22 = sPixel.Interpolate(c4, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c4, c7, 2, 1, 1);
            e30 = sPixel.Interpolate(c4, c7, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, 3, 1);
            e32 = sPixel.Interpolate(c7, c5, 5, 3);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 244:
        case 245: {
          e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
          e01 = sPixel.Interpolate(c4, c1, c3, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c3, c1, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = c4;
          e23 = c4;
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = c4;
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 246: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c3, c0, 5, 2, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = c4;
          e23 = c4;
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = c4;
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 247: {
          e00 = sPixel.Interpolate(c4, c3, 5, 3);
          e01 = sPixel.Interpolate(c4, c3, 7, 1);
          e02 = c4;
          e10 = sPixel.Interpolate(c4, c3, 5, 3);
          e11 = sPixel.Interpolate(c4, c3, 7, 1);
          e12 = c4;
          e13 = c4;
          e20 = sPixel.Interpolate(c4, c3, 5, 3);
          e21 = sPixel.Interpolate(c4, c3, 7, 1);
          e22 = c4;
          e23 = c4;
          e30 = sPixel.Interpolate(c4, c3, 5, 3);
          e31 = sPixel.Interpolate(c4, c3, 7, 1);
          e32 = c4;
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 249: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, c2, 5, 2, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e31 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 250: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e21 = c4;
          e22 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 251: {
          e02 = sPixel.Interpolate(c4, c2, 3, 1);
          e03 = sPixel.Interpolate(c4, c2, 5, 3);
          e11 = c4;
          e12 = sPixel.Interpolate(c4, c2, 7, 1);
          e13 = sPixel.Interpolate(c4, c2, 3, 1);
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e31 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 252: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, c0, 5, 2, 1);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e32 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 253: {
          e00 = sPixel.Interpolate(c4, c1, 5, 3);
          e01 = sPixel.Interpolate(c4, c1, 5, 3);
          e02 = sPixel.Interpolate(c4, c1, 5, 3);
          e03 = sPixel.Interpolate(c4, c1, 5, 3);
          e10 = sPixel.Interpolate(c4, c1, 7, 1);
          e11 = sPixel.Interpolate(c4, c1, 7, 1);
          e12 = sPixel.Interpolate(c4, c1, 7, 1);
          e13 = sPixel.Interpolate(c4, c1, 7, 1);
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e31 = c4;
          e32 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
        }
        break;
        case 254: {
          e00 = sPixel.Interpolate(c4, c0, 5, 3);
          e01 = sPixel.Interpolate(c4, c0, 3, 1);
          e10 = sPixel.Interpolate(c4, c0, 3, 1);
          e11 = sPixel.Interpolate(c4, c0, 7, 1);
          e12 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e32 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 255: {
          e01 = c4;
          e02 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e31 = c4;
          e32 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, e02, e03, 
      e10, e11, e12, e13, 
      e20, e21, e22, e23, 
      e30, e31, e32, e33, 
      });
    }
    #endregion
    #region standard LQ2x casepath
    public static sPixel[] Lq2xKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;
      switch (pattern) {
        #region LQ2x PATTERNS
        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          e00 = c3;
          e01 = c3;
          e10 = c3;
          e11 = c3;
        }
        break;
        case 10:
        case 138: {
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e00 = (c1.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c1, c3, 2, 1, 1));
        }
        break;
        case 14:
        case 142: {
          e10 = c0;
          e11 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 3, 3, 2);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          e10 = c4;
          e11 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 3, 3, 2);
            e01 = sPixel.Interpolate(c4, c1, 3, 1);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          e00 = c0;
          e10 = c0;
          e11 = c0;
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 19:
        case 51: {
          e10 = c2;
          e11 = c2;
          if (c1.IsNotLike(c5)) {
            e00 = c2;
            e01 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c5, c2, 3, 3, 2);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          e10 = c3;
          e11 = c3;
          if (c1.IsNotLike(c5)) {
            e00 = c3;
            e01 = c3;
          } else {
            e00 = sPixel.Interpolate(c3, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c5, c3, 3, 3, 2);
          }
        }
        break;
        case 26: {
          e10 = c0;
          e11 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 31:
        case 95: {
          e10 = c4;
          e11 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 42:
        case 170: {
          e01 = c0;
          e11 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 3, 3, 2);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          e01 = c2;
          e11 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 3, 3, 2);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
        }
        break;
        case 47:
        case 175: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
        }
        break;
        case 58:
        case 154:
        case 186: {
          e10 = c0;
          e11 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 6, 1, 1));
        }
        break;
        case 59: {
          e10 = c2;
          e11 = c2;
          e00 = (c1.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 6, 1, 1));
        }
        break;
        case 63: {
          e10 = c4;
          e11 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          e00 = c0;
          e01 = c0;
          e11 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          e01 = c1;
          e11 = c1;
          if (c7.IsNotLike(c3)) {
            e00 = c1;
            e10 = c1;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 3, 1);
            e10 = sPixel.Interpolate(c3, c7, c1, 3, 3, 2);
          }
        }
        break;
        case 74: {
          e01 = c0;
          e11 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
        }
        break;
        case 78:
        case 202:
        case 206: {
          e01 = c0;
          e11 = c0;
          e10 = c7.IsNotLike(c3) ? c0 : sPixel.Interpolate(c0, c3, c7, 6, 1, 1);
          e00 = c1.IsNotLike(c3) ? c0 : sPixel.Interpolate(c0, c1, c3, 6, 1, 1);
        }
        break;
        case 79: {
          e01 = c4;
          e11 = c4;
          e10 = c7.IsNotLike(c3) ? c4 : sPixel.Interpolate(c4, c3, c7, 6, 1, 1);
          e00 = c1.IsNotLike(c3) ? c4 : sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c7.IsNotLike(c5) ? c0 : sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
        }
        break;
        case 81:
        case 209:
        case 217: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c7.IsNotLike(c5) ? c1 : sPixel.Interpolate(c1, c5, c7, 2, 1, 1);
        }
        break;
        case 82:
        case 214:
        case 222: {
          e00 = c0;
          e10 = c0;
          e11 = c7.IsNotLike(c5) ? c0 : sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          e01 = c1.IsNotLike(c5) ? c0 : sPixel.Interpolate(c0, c1, c5, 2, 1, 1);
        }
        break;
        case 83:
        case 115: {
          e00 = c2;
          e10 = c2;
          e11 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 6, 1, 1));
        }
        break;
        case 84:
        case 212: {
          e00 = c0;
          e10 = c0;
          if (c7.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c5, 3, 1);
            e11 = sPixel.Interpolate(c5, c7, c0, 3, 3, 2);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          e00 = c1;
          e10 = c1;
          if (c7.IsNotLike(c5)) {
            e01 = c1;
            e11 = c1;
          } else {
            e01 = sPixel.Interpolate(c1, c5, 3, 1);
            e11 = sPixel.Interpolate(c5, c7, c1, 3, 3, 2);
          }
        }
        break;
        case 87: {
          e00 = c3;
          e10 = c3;
          e11 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 2, 1, 1));
        }
        break;
        case 88:
        case 248:
        case 250: {
          e00 = c0;
          e01 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
        }
        break;
        case 89:
        case 93: {
          e00 = c1;
          e01 = c1;
          e10 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 6, 1, 1));
        }
        break;
        case 90: {
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 6, 1, 1));
        }
        break;
        case 91: {
          e10 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 6, 1, 1));
        }
        break;
        case 92: {
          e00 = c0;
          e01 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
        }
        break;
        case 94: {
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 107:
        case 123: {
          e01 = c2;
          e11 = c2;
          e10 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c1, c3, 2, 1, 1));
        }
        break;
        case 111: {
          e01 = c4;
          e11 = c4;
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
        }
        break;
        case 112:
        case 240: {
          e00 = c0;
          e01 = c0;
          if (c7.IsNotLike(c5)) {
            e10 = c0;
            e11 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c7, 3, 1);
            e11 = sPixel.Interpolate(c5, c7, c0, 3, 3, 2);
          }
        }
        break;
        case 113:
        case 241: {
          e00 = c1;
          e01 = c1;
          if (c7.IsNotLike(c5)) {
            e10 = c1;
            e11 = c1;
          } else {
            e10 = sPixel.Interpolate(c1, c7, 3, 1);
            e11 = sPixel.Interpolate(c5, c7, c1, 3, 3, 2);
          }
        }
        break;
        case 114: {
          e00 = c0;
          e10 = c0;
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 6, 1, 1));
        }
        break;
        case 116: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
        }
        break;
        case 117: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 6, 1, 1));
        }
        break;
        case 121: {
          e00 = c1;
          e01 = c1;
          e10 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 6, 1, 1));
        }
        break;
        case 122: {
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 6, 1, 1));
        }
        break;
        case 126: {
          e00 = c0;
          e11 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 127: {
          e11 = c4;
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          e00 = c0;
          e10 = c0;
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c5, c0, 3, 3, 2);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = c2;
          e10 = c2;
          e11 = c2;
          e01 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 6, 1, 1));
        }
        break;
        case 151:
        case 183: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 14, 1, 1));
        }
        break;
        case 158: {
          e10 = c0;
          e11 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 159: {
          e10 = c4;
          e11 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 191: {
          e10 = c4;
          e11 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          e00 = c0;
          e01 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e11 = c0;
          } else {
            e10 = sPixel.Interpolate(c3, c7, c0, 3, 3, 2);
            e11 = sPixel.Interpolate(c0, c7, 3, 1);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = c1;
          e01 = c1;
          e11 = c1;
          e10 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 6, 1, 1));
        }
        break;
        case 211: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 2, 1, 1));
        }
        break;
        case 215: {
          e00 = c3;
          e10 = c3;
          e11 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 14, 1, 1));
        }
        break;
        case 218: {
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 6, 1, 1));
        }
        break;
        case 219: {
          e01 = c2;
          e10 = c2;
          e11 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c1, c3, 2, 1, 1));
        }
        break;
        case 220: {
          e00 = c0;
          e01 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 6, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
        }
        break;
        case 223: {
          e10 = c4;
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        case 233:
        case 237: {
          e00 = c1;
          e01 = c1;
          e11 = c1;
          e10 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 14, 1, 1));
        }
        break;
        case 234: {
          e01 = c0;
          e11 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 6, 1, 1));
        }
        break;
        case 235: {
          e01 = c2;
          e11 = c2;
          e10 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 14, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c1, c3, 2, 1, 1));
        }
        break;
        case 239: {
          e01 = c4;
          e11 = c4;
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
        }
        break;
        case 242: {
          e00 = c0;
          e10 = c0;
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 6, 1, 1));
        }
        break;
        case 243: {
          e00 = c2;
          e01 = c2;
          if (c7.IsNotLike(c5)) {
            e10 = c2;
            e11 = c2;
          } else {
            e10 = sPixel.Interpolate(c2, c7, 3, 1);
            e11 = sPixel.Interpolate(c5, c7, c2, 3, 3, 2);
          }
        }
        break;
        case 244: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 14, 1, 1));
        }
        break;
        case 245: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 14, 1, 1));
        }
        break;
        case 246: {
          e00 = c0;
          e10 = c0;
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 247: {
          e00 = c3;
          e10 = c3;
          e11 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 14, 1, 1));
        }
        break;
        case 249: {
          e00 = c1;
          e01 = c1;
          e10 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 2, 1, 1));
        }
        break;
        case 251: {
          e01 = c2;
          e10 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c1, c3, 2, 1, 1));
        }
        break;
        case 252: {
          e00 = c0;
          e01 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 14, 1, 1));
        }
        break;
        case 253: {
          e00 = c1;
          e01 = c1;
          e10 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 14, 1, 1));
        }
        break;
        case 254: {
          e00 = c0;
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 255: {
          e10 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 14, 1, 1));
          e11 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 14, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 14, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 14, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, 
      e10, e11, 
      });
    }
    #endregion
    #region standard LQ2x3 casepath
    public static sPixel[] Lq2x3Kernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e10, e11, e20, e21;
      var e00 = e01 = e10 = e11 = e20 = e21 = c4;
      switch (pattern) {
        #region LQ2x3 PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e21 = c1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e20 = c2;
          e21 = c2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          e00 = c3;
          e01 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
        }
        break;
        case 10:
        case 138: {
          e11 = c0;
          e20 = c0;
          e21 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c0, c1, 15, 1);
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          e11 = c2;
          e20 = c2;
          e21 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
            e10 = sPixel.Interpolate(c2, c3, 15, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e11 = c0;
          e20 = c0;
          e21 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 10, 5, 1);
            e01 = sPixel.Interpolate(c0, c1, 5, 3);
            e10 = sPixel.Interpolate(c0, c3, 13, 3);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          e11 = c4;
          e20 = c4;
          e21 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 10, 5, 1);
            e01 = sPixel.Interpolate(c4, c1, 5, 3);
            e10 = sPixel.Interpolate(c4, c3, 13, 3);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          e10 = c0;
          e20 = c0;
          e21 = c0;
          if (c1.IsNotLike(c5)) {
            e00 = c0;
            e01 = c0;
            e11 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = c2;
          e20 = c2;
          e21 = c2;
          if (c1.IsNotLike(c5)) {
            e00 = c2;
            e01 = c2;
            e11 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, 5, 3);
            e01 = sPixel.Interpolate(c1, c5, c2, 10, 5, 1);
            e11 = sPixel.Interpolate(c2, c5, 13, 3);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          e10 = c3;
          e20 = c3;
          e21 = c3;
          if (c1.IsNotLike(c5)) {
            e00 = c3;
            e01 = c3;
            e11 = c3;
          } else {
            e00 = sPixel.Interpolate(c3, c1, 5, 3);
            e01 = sPixel.Interpolate(c1, c5, c3, 10, 5, 1);
            e11 = sPixel.Interpolate(c3, c5, 13, 3);
          }
        }
        break;
        case 26: {
          e20 = c0;
          e21 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
          }
        }
        break;
        case 31:
        case 95: {
          e20 = c4;
          e21 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 42:
        case 170: {
          e11 = c0;
          e21 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
            e20 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 7, 5, 4);
            e01 = sPixel.Interpolate(c0, c1, 15, 1);
            e10 = sPixel.Interpolate(c0, c3, 1, 1);
            e20 = sPixel.Interpolate(c0, c3, 13, 3);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          e11 = c2;
          e21 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
            e20 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 5, 4);
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
            e20 = sPixel.Interpolate(c2, c3, 13, 3);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 10, 3, 3));
        }
        break;
        case 47:
        case 175: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 58:
        case 154:
        case 186: {
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 10, 3, 3));
        }
        break;
        case 59: {
          e11 = c2;
          e20 = c2;
          e21 = c2;
          if (c1.IsLike(c5)) {
            e01 = sPixel.Interpolate(c2, c1, c5, 10, 3, 3);
          }
          if ((c1.IsNotLike(c5) && c1.IsLike(c3))) {
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e01 = c2;
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c2, c3, 15, 1);
          }
        }
        break;
        case 63: {
          e10 = c4;
          e20 = c4;
          e21 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          e00 = c0;
          e01 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c0, c7, 15, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          e01 = c1;
          e11 = c1;
          if (c7.IsNotLike(c3)) {
            e00 = c1;
            e10 = c1;
            e20 = c1;
            e21 = c1;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 13, 3);
            e10 = sPixel.Interpolate(c1, c3, 1, 1);
            e20 = sPixel.Interpolate(c7, c3, c1, 7, 5, 4);
            e21 = sPixel.Interpolate(c1, c7, 15, 1);
          }
        }
        break;
        case 74: {
          e10 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e21 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c0, c7, 15, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c0, c1, 15, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e21 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 10, 3, 3));
        }
        break;
        case 79: {
          e11 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c4, c1, 15, 1);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          if (c7.IsNotLike(c5)) {
            e11 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
            e20 = sPixel.Interpolate(c0, c7, 15, 1);
            e21 = sPixel.Interpolate(c0, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          if (c7.IsNotLike(c5)) {
            e11 = c1;
            e20 = c1;
            e21 = c1;
          } else {
            e11 = sPixel.Interpolate(c1, c5, 15, 1);
            e20 = sPixel.Interpolate(c1, c7, 15, 1);
            e21 = sPixel.Interpolate(c1, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          e10 = c0;
          e11 = c0;
          if (c7.IsNotLike(c5)) {
            e20 = c0;
            e21 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c7, 15, 1);
            e21 = sPixel.Interpolate(c0, c5, c7, 6, 5, 5);
          }
          if (c1.IsNotLike(c5)) {
            e00 = c0;
            e01 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
          }
        }
        break;
        case 83:
        case 115: {
          e00 = c2;
          e10 = c2;
          e11 = c2;
          e20 = c2;
          e21 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 10, 3, 3));
        }
        break;
        case 84:
        case 212: {
          e00 = c0;
          e10 = c0;
          if (c7.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c5, 13, 3);
            e11 = sPixel.Interpolate(c0, c5, 1, 1);
            e20 = sPixel.Interpolate(c0, c7, 15, 1);
            e21 = sPixel.Interpolate(c7, c5, c0, 7, 5, 4);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          e00 = c1;
          e10 = c1;
          if (c7.IsNotLike(c5)) {
            e01 = c1;
            e11 = c1;
            e20 = c1;
            e21 = c1;
          } else {
            e01 = sPixel.Interpolate(c1, c5, 13, 3);
            e11 = sPixel.Interpolate(c1, c5, 1, 1);
            e20 = sPixel.Interpolate(c1, c7, 15, 1);
            e21 = sPixel.Interpolate(c7, c5, c1, 7, 5, 4);
          }
        }
        break;
        case 87: {
          e10 = c3;
          e20 = c3;
          e21 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e00 = c3;
            e01 = c3;
            e11 = c3;
          } else {
            e00 = sPixel.Interpolate(c3, c1, 15, 1);
            e01 = sPixel.Interpolate(c3, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c3, c5, 15, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          e00 = c0;
          e01 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
          }
          if (c7.IsNotLike(c5)) {
            e11 = c0;
            e21 = c0;
          } else {
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
            e21 = sPixel.Interpolate(c0, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 89:
        case 93:
        case 253: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 10, 3, 3));
        }
        break;
        case 90: {
          e10 = c0;
          e11 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 10, 3, 3));
        }
        break;
        case 91: {
          e11 = c2;
          if (c1.IsLike(c5)) {
            e01 = sPixel.Interpolate(c2, c1, c5, 10, 3, 3);
          }
          if ((c1.IsNotLike(c5) && c1.IsLike(c3))) {
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e01 = c2;
          }
          e20 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c2, c3, 15, 1);
          }
        }
        break;
        case 92: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 10, 3, 3));
        }
        break;
        case 94: {
          e10 = c0;
          if (c1.IsLike(c3)) {
            e00 = sPixel.Interpolate(c0, c1, c3, 10, 3, 3);
          }
          if ((c1.IsLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e00 = c0;
          }
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
          }
        }
        break;
        case 107:
        case 123: {
          e10 = c2;
          e11 = c2;
          if (c7.IsNotLike(c3)) {
            e20 = c2;
            e21 = c2;
          } else {
            e20 = sPixel.Interpolate(c2, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c2, c7, 15, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
          }
        }
        break;
        case 111: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c7, 15, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 112:
        case 240: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          if (c7.IsNotLike(c5)) {
            e11 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e11 = sPixel.Interpolate(c0, c5, 13, 3);
            e20 = sPixel.Interpolate(c0, c7, 9, 7);
            e21 = sPixel.Interpolate(c7, c5, c0, 10, 5, 1);
          }
        }
        break;
        case 113:
        case 241: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          if (c7.IsNotLike(c5)) {
            e11 = c1;
            e20 = c1;
            e21 = c1;
          } else {
            e11 = sPixel.Interpolate(c1, c5, 13, 3);
            e20 = sPixel.Interpolate(c1, c7, 9, 7);
            e21 = sPixel.Interpolate(c7, c5, c1, 10, 5, 1);
          }
        }
        break;
        case 114: {
          e00 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 10, 3, 3));
        }
        break;
        case 116:
        case 244: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 10, 3, 3));
        }
        break;
        case 117:
        case 245: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e21 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 10, 3, 3));
        }
        break;
        case 121: {
          e00 = c1;
          e01 = c1;
          e11 = c1;
          if (c7.IsLike(c5)) {
            e21 = sPixel.Interpolate(c1, c5, c7, 10, 3, 3);
          }
          if ((c7.IsNotLike(c5) && c7.IsLike(c3))) {
            e21 = sPixel.Interpolate(c1, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e21 = c1;
          }
          if (c7.IsNotLike(c3)) {
            e10 = c1;
            e20 = c1;
          } else {
            e10 = sPixel.Interpolate(c1, c3, 15, 1);
            e20 = sPixel.Interpolate(c1, c3, c7, 6, 5, 5);
          }
        }
        break;
        case 122: {
          e11 = c0;
          if (c7.IsLike(c5)) {
            e21 = sPixel.Interpolate(c0, c5, c7, 10, 3, 3);
          }
          if ((c7.IsNotLike(c5) && c7.IsLike(c3))) {
            e21 = sPixel.Interpolate(c0, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e21 = c0;
          }
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
          }
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 10, 3, 3));
        }
        break;
        case 126: {
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c0, c7, 15, 1);
          }
          if (c1.IsNotLike(c5)) {
            e00 = c0;
            e01 = c0;
            e11 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
          }
        }
        break;
        case 127: {
          if (c1.IsLike(c5)) {
            e01 = sPixel.Interpolate(c4, c1, c5, 6, 5, 5);
          }
          if ((c1.IsNotLike(c5) && c1.IsLike(c3))) {
            e01 = sPixel.Interpolate(c4, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e01 = c4;
          }
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c4, c7, 15, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 10, 3, 3);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
          e11 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, 15, 1));
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          e10 = c0;
          e20 = c0;
          if (c1.IsNotLike(c5)) {
            e00 = c0;
            e01 = c0;
            e11 = c0;
            e21 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
            e01 = sPixel.Interpolate(c1, c5, c0, 7, 5, 4);
            e11 = sPixel.Interpolate(c0, c5, 1, 1);
            e21 = sPixel.Interpolate(c0, c5, 13, 3);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = c2;
          e10 = c2;
          e11 = c2;
          e20 = c2;
          e21 = c2;
          e01 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 10, 3, 3));
        }
        break;
        case 151:
        case 183: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 10, 3, 3));
        }
        break;
        case 158: {
          e10 = c0;
          e20 = c0;
          e21 = c0;
          if (c1.IsLike(c3)) {
            e00 = sPixel.Interpolate(c0, c1, c3, 10, 3, 3);
          }
          if ((c1.IsLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e00 = c0;
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
          }
        }
        break;
        case 159: {
          e11 = c4;
          e20 = c4;
          e21 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
            e10 = sPixel.Interpolate(c4, c3, 15, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 191: {
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          e00 = c0;
          e01 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 13, 3);
            e20 = sPixel.Interpolate(c7, c3, c0, 10, 5, 1);
            e21 = sPixel.Interpolate(c0, c7, 9, 7);
          }
        }
        break;
        case 201:
        case 205:
        case 233:
        case 237: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e21 = c1;
          e20 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 10, 3, 3));
        }
        break;
        case 211: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          if (c7.IsNotLike(c5)) {
            e11 = c2;
            e20 = c2;
            e21 = c2;
          } else {
            e11 = sPixel.Interpolate(c2, c5, 15, 1);
            e20 = sPixel.Interpolate(c2, c7, 15, 1);
            e21 = sPixel.Interpolate(c2, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 215: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          if (c7.IsNotLike(c5)) {
            e20 = c3;
            e21 = c3;
          } else {
            e20 = sPixel.Interpolate(c3, c7, 15, 1);
            e21 = sPixel.Interpolate(c3, c5, c7, 6, 5, 5);
          }
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 10, 3, 3));
        }
        break;
        case 218: {
          e10 = c0;
          if (c7.IsLike(c3)) {
            e20 = sPixel.Interpolate(c0, c3, c7, 10, 3, 3);
          }
          if ((c7.IsLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c0, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e20 = c0;
          }
          if (c7.IsNotLike(c5)) {
            e11 = c0;
            e21 = c0;
          } else {
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
            e21 = sPixel.Interpolate(c0, c5, c7, 6, 5, 5);
          }
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 10, 3, 3));
        }
        break;
        case 219: {
          if (c7.IsNotLike(c5)) {
            e11 = c2;
            e20 = c2;
            e21 = c2;
          } else {
            e11 = sPixel.Interpolate(c2, c5, 15, 1);
            e20 = sPixel.Interpolate(c2, c7, 15, 1);
            e21 = sPixel.Interpolate(c2, c5, c7, 6, 5, 5);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
            e10 = sPixel.Interpolate(c2, c3, 15, 1);
          }
        }
        break;
        case 220: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          if (c7.IsLike(c3)) {
            e20 = sPixel.Interpolate(c0, c3, c7, 10, 3, 3);
          }
          if ((c7.IsLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c0, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e20 = c0;
          }
          if (c7.IsNotLike(c5)) {
            e11 = c0;
            e21 = c0;
          } else {
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
            e21 = sPixel.Interpolate(c0, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 223: {
          if (c1.IsLike(c3)) {
            e00 = sPixel.Interpolate(c4, c1, c3, 6, 5, 5);
          }
          if ((c1.IsLike(c5) && c1.IsNotLike(c3))) {
            e00 = sPixel.Interpolate(c4, c1, 15, 1);
          }
          if ((c1.IsNotLike(c5) && c1.IsNotLike(c3))) {
            e00 = c4;
          }
          if (c7.IsNotLike(c5)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c7, 15, 1);
            e21 = sPixel.Interpolate(c4, c5, c7, 6, 5, 5);
          }
          e10 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, 15, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, c5, 10, 3, 3);
            e11 = sPixel.Interpolate(c4, c5, 15, 1);
          }
        }
        break;
        case 234: {
          e01 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
            e21 = sPixel.Interpolate(c0, c7, 15, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 10, 3, 3));
        }
        break;
        case 235: {
          e10 = c2;
          e11 = c2;
          e21 = c2;
          e20 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 10, 3, 3));
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
          }
        }
        break;
        case 239: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
        }
        break;
        case 242: {
          e00 = c0;
          e10 = c0;
          if (c7.IsNotLike(c5)) {
            e11 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
            e20 = sPixel.Interpolate(c0, c7, 15, 1);
            e21 = sPixel.Interpolate(c0, c5, c7, 6, 5, 5);
          }
          e01 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 10, 3, 3));
        }
        break;
        case 243: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          if (c7.IsNotLike(c5)) {
            e11 = c2;
            e20 = c2;
            e21 = c2;
          } else {
            e11 = sPixel.Interpolate(c2, c5, 13, 3);
            e20 = sPixel.Interpolate(c2, c7, 9, 7);
            e21 = sPixel.Interpolate(c7, c5, c2, 10, 5, 1);
          }
        }
        break;
        case 246: {
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 10, 3, 3));
          if (c1.IsNotLike(c5)) {
            e00 = c0;
            e01 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
          }
        }
        break;
        case 247: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 10, 3, 3));
        }
        break;
        case 249: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e20 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 10, 3, 3));
          if (c7.IsNotLike(c5)) {
            e11 = c1;
            e21 = c1;
          } else {
            e11 = sPixel.Interpolate(c1, c5, 15, 1);
            e21 = sPixel.Interpolate(c1, c5, c7, 6, 5, 5);
          }
        }
        break;
        case 251: {
          if (c7.IsLike(c5)) {
            e21 = sPixel.Interpolate(c2, c5, c7, 6, 5, 5);
          }
          if ((c7.IsNotLike(c5) && c7.IsLike(c3))) {
            e21 = sPixel.Interpolate(c2, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e21 = c2;
          }
          if (c7.IsNotLike(c3)) {
            e10 = c2;
            e20 = c2;
          } else {
            e10 = sPixel.Interpolate(c2, c3, 15, 1);
            e20 = sPixel.Interpolate(c2, c3, c7, 10, 3, 3);
          }
          e11 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, 15, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, c3, 6, 5, 5);
            e01 = sPixel.Interpolate(c2, c1, 15, 1);
          }
        }
        break;
        case 252: {
          e00 = c0;
          e01 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 15, 1);
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
          }
          e21 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 10, 3, 3));
        }
        break;
        case 254: {
          if (c7.IsLike(c3)) {
            e20 = sPixel.Interpolate(c0, c3, c7, 6, 5, 5);
          }
          if ((c7.IsLike(c5) && c7.IsNotLike(c3))) {
            e20 = sPixel.Interpolate(c0, c7, 15, 1);
          }
          if ((c7.IsNotLike(c5) && c7.IsNotLike(c3))) {
            e20 = c0;
          }
          e10 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, 15, 1));
          if (c7.IsNotLike(c5)) {
            e11 = c0;
            e21 = c0;
          } else {
            e11 = sPixel.Interpolate(c0, c5, 15, 1);
            e21 = sPixel.Interpolate(c0, c5, c7, 10, 3, 3);
          }
          if (c1.IsNotLike(c5)) {
            e00 = c0;
            e01 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, 15, 1);
            e01 = sPixel.Interpolate(c0, c1, c5, 6, 5, 5);
          }
        }
        break;
        case 255: {
          e10 = c4;
          e11 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 10, 3, 3));
          e21 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 10, 3, 3));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 10, 3, 3));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 10, 3, 3));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, 
      e10, e11, 
      e20, e21, 
      });
    }
    #endregion
    #region standard LQ2x4 casepath
    public static sPixel[] Lq2x4Kernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e10, e11, e20, e21, e30, e31;
      var e00 = e01 = e10 = e11 = e20 = e21 = e30 = e31 = c4;
      switch (pattern) {
        #region LQ2x4 PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e21 = c1;
          e30 = c1;
          e31 = c1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e20 = c2;
          e21 = c2;
          e30 = c2;
          e31 = c2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          e00 = c3;
          e01 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
          e30 = c3;
          e31 = c3;
        }
        break;
        case 10:
        case 138: {
          e01 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c0, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          e01 = c2;
          e11 = c2;
          e20 = c2;
          e21 = c2;
          e30 = c2;
          e31 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 9, 7);
            e01 = sPixel.Interpolate(c0, c1, 1, 1);
            e10 = sPixel.Interpolate(c0, c3, c1, 8, 5, 3);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e30 = c4;
          e31 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 9, 7);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, c1, 8, 5, 3);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          e00 = c0;
          e10 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = c2;
          e20 = c2;
          e21 = c2;
          e30 = c2;
          e31 = c2;
          if (c1.IsNotLike(c5)) {
            e00 = c2;
            e01 = c2;
            e11 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, 1, 1);
            e01 = sPixel.Interpolate(c1, c5, 9, 7);
            e11 = sPixel.Interpolate(c2, c5, c1, 8, 5, 3);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          e10 = c3;
          e20 = c3;
          e21 = c3;
          e30 = c3;
          e31 = c3;
          if (c1.IsNotLike(c5)) {
            e00 = c3;
            e01 = c3;
            e11 = c3;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c5, 9, 7);
            e11 = sPixel.Interpolate(c3, c5, c1, 8, 5, 3);
          }
        }
        break;
        case 26: {
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c0, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 31:
        case 95: {
          e20 = c4;
          e21 = c4;
          e30 = c4;
          e31 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 42:
        case 170: {
          e01 = c0;
          e11 = c0;
          e21 = c0;
          e31 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
            e20 = c0;
            e30 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 4, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, c1, 3, 3, 2);
            e20 = sPixel.Interpolate(c0, c3, 5, 3);
            e30 = sPixel.Interpolate(c0, c3, 7, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          e01 = c2;
          e11 = c2;
          e21 = c2;
          e31 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
            e20 = c2;
            e30 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 4, 3, 1);
            e10 = sPixel.Interpolate(c2, c3, c1, 3, 3, 2);
            e20 = sPixel.Interpolate(c2, c3, 5, 3);
            e30 = sPixel.Interpolate(c2, c3, 7, 1);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
        }
        break;
        case 47:
        case 175: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e30 = c4;
          e31 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 58:
        case 154:
        case 186: {
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 59: {
          e20 = c2;
          e21 = c2;
          e30 = c2;
          e31 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c2;
            e11 = c2;
          } else {
            e01 = sPixel.Interpolate(c2, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c2, c5, 7, 1);
          }
        }
        break;
        case 63: {
          e10 = c4;
          e20 = c4;
          e21 = c4;
          e30 = c4;
          e31 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e21 = c0;
          e31 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          e01 = c1;
          e11 = c1;
          e21 = c1;
          e31 = c1;
          if (c7.IsNotLike(c3)) {
            e00 = c1;
            e10 = c1;
            e20 = c1;
            e30 = c1;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 7, 1);
            e10 = sPixel.Interpolate(c1, c3, 5, 3);
            e20 = sPixel.Interpolate(c1, c3, c7, 3, 3, 2);
            e30 = sPixel.Interpolate(c7, c3, c1, 4, 3, 1);
          }
        }
        break;
        case 74: {
          e01 = c0;
          e11 = c0;
          e21 = c0;
          e31 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c0, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          e01 = c0;
          e11 = c0;
          e21 = c0;
          e31 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 7, 1);
            e30 = sPixel.Interpolate(c0, c7, c3, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
        }
        break;
        case 79: {
          e01 = c4;
          e11 = c4;
          e21 = c4;
          e31 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 7, 1);
            e30 = sPixel.Interpolate(c4, c7, c3, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e30 = c0;
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c0, c5, 2, 1, 1);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e30 = c1;
          if (c7.IsNotLike(c5)) {
            e21 = c1;
            e31 = c1;
          } else {
            e21 = sPixel.Interpolate(c1, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c1, c5, 2, 1, 1);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          e00 = c0;
          e10 = c0;
          e20 = c0;
          e30 = c0;
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c0, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 83:
        case 115: {
          e00 = c2;
          e10 = c2;
          e20 = c2;
          e30 = c2;
          if (c7.IsNotLike(c5)) {
            e21 = c2;
            e31 = c2;
          } else {
            e21 = sPixel.Interpolate(c2, c5, 7, 1);
            e31 = sPixel.Interpolate(c2, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c2;
            e11 = c2;
          } else {
            e01 = sPixel.Interpolate(c2, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c2, c5, 7, 1);
          }
        }
        break;
        case 84:
        case 212: {
          e00 = c0;
          e10 = c0;
          e20 = c0;
          e30 = c0;
          if (c7.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
            e21 = c0;
            e31 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c5, 7, 1);
            e11 = sPixel.Interpolate(c0, c5, 5, 3);
            e21 = sPixel.Interpolate(c0, c5, c7, 3, 3, 2);
            e31 = sPixel.Interpolate(c7, c5, c0, 4, 3, 1);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          e00 = c1;
          e10 = c1;
          e20 = c1;
          e30 = c1;
          if (c7.IsNotLike(c5)) {
            e01 = c1;
            e11 = c1;
            e21 = c1;
            e31 = c1;
          } else {
            e01 = sPixel.Interpolate(c1, c5, 7, 1);
            e11 = sPixel.Interpolate(c1, c5, 5, 3);
            e21 = sPixel.Interpolate(c1, c5, c7, 3, 3, 2);
            e31 = sPixel.Interpolate(c7, c5, c1, 4, 3, 1);
          }
        }
        break;
        case 87: {
          e00 = c3;
          e10 = c3;
          e20 = c3;
          e30 = c3;
          if (c7.IsNotLike(c5)) {
            e21 = c3;
            e31 = c3;
          } else {
            e21 = sPixel.Interpolate(c3, c5, 7, 1);
            e31 = sPixel.Interpolate(c3, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c3;
            e11 = c3;
          } else {
            e01 = sPixel.Interpolate(c1, c3, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c3, c5, 3, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c0, c5, 2, 1, 1);
          }
        }
        break;
        case 89:
        case 93: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          if (c7.IsNotLike(c3)) {
            e20 = c1;
            e30 = c1;
          } else {
            e20 = sPixel.Interpolate(c1, c3, 7, 1);
            e30 = sPixel.Interpolate(c1, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c1;
            e31 = c1;
          } else {
            e21 = sPixel.Interpolate(c1, c5, 7, 1);
            e31 = sPixel.Interpolate(c1, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 90: {
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 7, 1);
            e30 = sPixel.Interpolate(c0, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 7, 1);
            e31 = sPixel.Interpolate(c0, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 91: {
          if (c7.IsNotLike(c3)) {
            e20 = c2;
            e30 = c2;
          } else {
            e20 = sPixel.Interpolate(c2, c3, 7, 1);
            e30 = sPixel.Interpolate(c2, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c2;
            e31 = c2;
          } else {
            e21 = sPixel.Interpolate(c2, c5, 7, 1);
            e31 = sPixel.Interpolate(c2, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c2;
            e11 = c2;
          } else {
            e01 = sPixel.Interpolate(c2, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c2, c5, 7, 1);
          }
        }
        break;
        case 92: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 7, 1);
            e30 = sPixel.Interpolate(c0, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 7, 1);
            e31 = sPixel.Interpolate(c0, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 94: {
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 7, 1);
            e30 = sPixel.Interpolate(c0, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 7, 1);
            e31 = sPixel.Interpolate(c0, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 107:
        case 123: {
          e01 = c2;
          e11 = c2;
          e21 = c2;
          e31 = c2;
          if (c7.IsNotLike(c3)) {
            e20 = c2;
            e30 = c2;
          } else {
            e20 = sPixel.Interpolate(c2, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c2, c3, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 111: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e21 = c4;
          e31 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 112:
        case 240: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c0, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c5, 9, 7);
          }
        }
        break;
        case 113:
        case 241: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          if (c7.IsNotLike(c5)) {
            e21 = c1;
            e30 = c1;
            e31 = c1;
          } else {
            e21 = sPixel.Interpolate(c1, c5, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c1, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c5, 9, 7);
          }
        }
        break;
        case 114: {
          e00 = c0;
          e10 = c0;
          e20 = c0;
          e30 = c0;
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 7, 1);
            e31 = sPixel.Interpolate(c0, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 116: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e30 = c0;
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 7, 1);
            e31 = sPixel.Interpolate(c0, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 117: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e30 = c1;
          if (c7.IsNotLike(c5)) {
            e21 = c1;
            e31 = c1;
          } else {
            e21 = sPixel.Interpolate(c1, c5, 7, 1);
            e31 = sPixel.Interpolate(c1, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 121: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          if (c7.IsNotLike(c3)) {
            e20 = c1;
            e30 = c1;
          } else {
            e20 = sPixel.Interpolate(c1, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c1, c3, 2, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c1;
            e31 = c1;
          } else {
            e21 = sPixel.Interpolate(c1, c5, 7, 1);
            e31 = sPixel.Interpolate(c1, c7, c5, 5, 2, 1);
          }
        }
        break;
        case 122: {
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 7, 1);
            e31 = sPixel.Interpolate(c0, c7, c5, 5, 2, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 126: {
          e00 = c0;
          e10 = c0;
          e21 = c0;
          e31 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 127: {
          e10 = c4;
          e21 = c4;
          e31 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c3, c4, 2, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e11 = c4;
          } else {
            e01 = sPixel.Interpolate(c1, c4, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c5, 3, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          e00 = c0;
          e10 = c0;
          e20 = c0;
          e30 = c0;
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
            e21 = c0;
            e31 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c5, c0, 4, 3, 1);
            e11 = sPixel.Interpolate(c0, c5, c1, 3, 3, 2);
            e21 = sPixel.Interpolate(c0, c5, 5, 3);
            e31 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = c2;
          e10 = c2;
          e20 = c2;
          e21 = c2;
          e30 = c2;
          e31 = c2;
          if (c1.IsNotLike(c5)) {
            e01 = c2;
            e11 = c2;
          } else {
            e01 = sPixel.Interpolate(c2, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c2, c5, 7, 1);
          }
        }
        break;
        case 151:
        case 183: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
          e30 = c3;
          e31 = c3;
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 6, 1, 1));
        }
        break;
        case 158: {
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 159: {
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e30 = c4;
          e31 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 191: {
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e30 = c4;
          e31 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e21 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c7, c3, 9, 7);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e21 = c1;
          e31 = c1;
          if (c7.IsNotLike(c3)) {
            e20 = c1;
            e30 = c1;
          } else {
            e20 = sPixel.Interpolate(c1, c3, 7, 1);
            e30 = sPixel.Interpolate(c1, c7, c3, 5, 2, 1);
          }
        }
        break;
        case 211: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e20 = c2;
          e30 = c2;
          if (c7.IsNotLike(c5)) {
            e21 = c2;
            e31 = c2;
          } else {
            e21 = sPixel.Interpolate(c2, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c2, c5, 2, 1, 1);
          }
        }
        break;
        case 215: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e30 = c3;
          if (c7.IsNotLike(c5)) {
            e21 = c3;
            e31 = c3;
          } else {
            e21 = sPixel.Interpolate(c3, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c3, c5, 2, 1, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 6, 1, 1));
        }
        break;
        case 218: {
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 7, 1);
            e30 = sPixel.Interpolate(c0, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c0, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 219: {
          e01 = c2;
          e11 = c2;
          e20 = c2;
          e30 = c2;
          if (c7.IsNotLike(c5)) {
            e21 = c2;
            e31 = c2;
          } else {
            e21 = sPixel.Interpolate(c2, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c2, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 220: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 7, 1);
            e30 = sPixel.Interpolate(c0, c7, c3, 5, 2, 1);
          }
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c0, c5, 2, 1, 1);
          }
        }
        break;
        case 223: {
          e11 = c4;
          e20 = c4;
          e30 = c4;
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e31 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c4, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 2, 1, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        case 233:
        case 237: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e21 = c1;
          e31 = c1;
          e30 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 6, 1, 1));
        }
        break;
        case 234: {
          e01 = c0;
          e11 = c0;
          e21 = c0;
          e31 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 5, 2, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
        }
        break;
        case 235: {
          e01 = c2;
          e11 = c2;
          e20 = c2;
          e21 = c2;
          e31 = c2;
          e30 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 6, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 239: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e31 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
        }
        break;
        case 242: {
          e00 = c0;
          e10 = c0;
          e20 = c0;
          e30 = c0;
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e31 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c0, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, c5, 5, 2, 1);
            e11 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 243: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e20 = c2;
          if (c7.IsNotLike(c5)) {
            e21 = c2;
            e30 = c2;
            e31 = c2;
          } else {
            e21 = sPixel.Interpolate(c2, c5, c7, 8, 5, 3);
            e30 = sPixel.Interpolate(c2, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c5, 9, 7);
          }
        }
        break;
        case 244: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
        }
        break;
        case 245: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e21 = c1;
          e30 = c1;
          e31 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 6, 1, 1));
        }
        break;
        case 246: {
          e00 = c0;
          e10 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 247: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
          e30 = c3;
          e31 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 6, 1, 1));
        }
        break;
        case 249: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e30 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 6, 1, 1));
          if (c7.IsNotLike(c5)) {
            e21 = c1;
            e31 = c1;
          } else {
            e21 = sPixel.Interpolate(c1, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c1, c5, 2, 1, 1);
          }
        }
        break;
        case 251: {
          e01 = c2;
          e11 = c2;
          e20 = c2;
          e30 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 6, 1, 1));
          if (c7.IsNotLike(c5)) {
            e21 = c2;
            e31 = c2;
          } else {
            e21 = sPixel.Interpolate(c2, c5, 3, 1);
            e31 = sPixel.Interpolate(c7, c2, c5, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 252: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e21 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
          e31 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
        }
        break;
        case 253: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          e21 = c1;
          e30 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 6, 1, 1));
          e31 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 6, 1, 1));
        }
        break;
        case 254: {
          e00 = c0;
          e10 = c0;
          e21 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c7, c0, c3, 2, 1, 1);
          }
          e31 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 6, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e11 = c0;
          } else {
            e01 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 255: {
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 6, 1, 1));
          e31 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 6, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 6, 1, 1));
          e01 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 6, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, 
      e10, e11, 
      e20, e21, 
      e30, e31, 
      });
    }
    #endregion
    #region standard LQ3x casepath
    public static sPixel[] Lq3xKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e02, e10, e11, e12, e20, e21, e22;
      var e00 = e01 = e02 = e10 = e11 = e12 = e20 = e21 = e22 = c4;
      switch (pattern) {
        #region LQ3x PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          e00 = c2;
          e01 = c2;
          e02 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          e00 = c3;
          e01 = c3;
          e02 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
        }
        break;
        case 10:
        case 138: {
          e02 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 7, 7, 2);
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          e02 = c2;
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 7, 2);
            e01 = sPixel.Interpolate(c2, c1, 7, 1);
            e10 = sPixel.Interpolate(c2, c3, 7, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e02 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c0, 3, 1);
            e02 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          e11 = c4;
          e12 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e02 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 3, 1);
            e02 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c4, c3, 3, 1);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          e00 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
            e12 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = c2;
          e11 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          if (c1.IsNotLike(c5)) {
            e00 = c2;
            e01 = c2;
            e02 = c2;
            e12 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c2, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c2, c5, 3, 1);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          if (c1.IsNotLike(c5)) {
            e00 = c3;
            e01 = c3;
            e02 = c3;
            e12 = c3;
          } else {
            e00 = sPixel.Interpolate(c3, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c3, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c3, c5, 3, 1);
          }
        }
        break;
        case 26: {
          e01 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 7, 7, 2);
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e12 = c0;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 31:
        case 95: {
          e01 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e12 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 42:
        case 170: {
          e02 = c0;
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
            e20 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c0, 3, 1);
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          e02 = c2;
          e11 = c2;
          e12 = c2;
          e21 = c2;
          e22 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
            e20 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c2, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c2, 3, 1);
            e20 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
        }
        break;
        case 47:
        case 175: {
          e01 = c4;
          e02 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 58:
        case 154:
        case 186: {
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 59: {
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 7, 2);
            e01 = sPixel.Interpolate(c2, c1, 7, 1);
            e10 = sPixel.Interpolate(c2, c3, 7, 1);
          }
          e02 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 2, 1, 1));
        }
        break;
        case 63: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e12 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e11 = c0;
          e12 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          e01 = c1;
          e02 = c1;
          e11 = c1;
          e12 = c1;
          e22 = c1;
          if (c7.IsNotLike(c3)) {
            e00 = c1;
            e10 = c1;
            e20 = c1;
            e21 = c1;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 3, 1);
            e10 = sPixel.Interpolate(c3, c1, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c1, c7, 3, 1);
          }
        }
        break;
        case 74: {
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e21 = c0;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c0, 7, 7, 2);
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
        }
        break;
        case 79: {
          e02 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e22 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          if (c7.IsNotLike(c5)) {
            e12 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c0, 7, 7, 2);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          if (c7.IsNotLike(c5)) {
            e12 = c1;
            e21 = c1;
            e22 = c1;
          } else {
            e12 = sPixel.Interpolate(c1, c5, 7, 1);
            e21 = sPixel.Interpolate(c1, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c1, 7, 7, 2);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          e00 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          if (c7.IsNotLike(c5)) {
            e21 = c0;
            e22 = c0;
          } else {
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c0, 7, 7, 2);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
          }
        }
        break;
        case 83:
        case 115: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 2, 1, 1));
        }
        break;
        case 84:
        case 212: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          if (c7.IsNotLike(c5)) {
            e02 = c0;
            e12 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c5, 3, 1);
            e12 = sPixel.Interpolate(c5, c0, 3, 1);
            e21 = sPixel.Interpolate(c0, c7, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          e00 = c1;
          e01 = c1;
          e10 = c1;
          e11 = c1;
          e20 = c1;
          if (c7.IsNotLike(c5)) {
            e02 = c1;
            e12 = c1;
            e21 = c1;
            e22 = c1;
          } else {
            e02 = sPixel.Interpolate(c1, c5, 3, 1);
            e12 = sPixel.Interpolate(c5, c1, 3, 1);
            e21 = sPixel.Interpolate(c1, c7, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 87: {
          e00 = c3;
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
          e22 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c3;
            e02 = c3;
            e12 = c3;
          } else {
            e01 = sPixel.Interpolate(c3, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c3, 7, 7, 2);
            e12 = sPixel.Interpolate(c3, c5, 7, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e11 = c0;
          e21 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
          }
          if (c7.IsNotLike(c5)) {
            e12 = c0;
            e22 = c0;
          } else {
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c0, 7, 7, 2);
          }
        }
        break;
        case 89:
        case 93:
        case 253: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e21 = c1;
          e20 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 2, 1, 1));
        }
        break;
        case 90: {
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 91: {
          e11 = c2;
          e12 = c2;
          e21 = c2;
          e20 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 7, 2);
            e01 = sPixel.Interpolate(c2, c1, 7, 1);
            e10 = sPixel.Interpolate(c2, c3, 7, 1);
          }
          e02 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 2, 1, 1));
        }
        break;
        case 92: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
        }
        break;
        case 94: {
          e10 = c0;
          e11 = c0;
          e21 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
            e12 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 107:
        case 123: {
          e02 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e22 = c2;
          if (c7.IsNotLike(c3)) {
            e20 = c2;
            e21 = c2;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c2, 7, 7, 2);
            e21 = sPixel.Interpolate(c2, c7, 7, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 7, 2);
            e01 = sPixel.Interpolate(c2, c1, 7, 1);
          }
        }
        break;
        case 111: {
          e01 = c4;
          e02 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e22 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 112:
        case 240: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          if (c7.IsNotLike(c5)) {
            e12 = c0;
            e20 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e12 = sPixel.Interpolate(c0, c5, 3, 1);
            e20 = sPixel.Interpolate(c0, c7, 3, 1);
            e21 = sPixel.Interpolate(c7, c0, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 113:
        case 241: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          if (c7.IsNotLike(c5)) {
            e12 = c1;
            e20 = c1;
            e21 = c1;
            e22 = c1;
          } else {
            e12 = sPixel.Interpolate(c1, c5, 3, 1);
            e20 = sPixel.Interpolate(c1, c7, 3, 1);
            e21 = sPixel.Interpolate(c7, c1, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 114: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 116:
        case 244: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
        }
        break;
        case 117:
        case 245: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e20 = c1;
          e21 = c1;
          e22 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 2, 1, 1));
        }
        break;
        case 121: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e11 = c1;
          e12 = c1;
          if (c7.IsNotLike(c3)) {
            e10 = c1;
            e20 = c1;
            e21 = c1;
          } else {
            e10 = sPixel.Interpolate(c1, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c1, 7, 7, 2);
            e21 = sPixel.Interpolate(c1, c7, 7, 1);
          }
          e22 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 2, 1, 1));
        }
        break;
        case 122: {
          e01 = c0;
          e11 = c0;
          e12 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
          }
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 126: {
          e00 = c0;
          e11 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
            e12 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 127: {
          e11 = c4;
          e22 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e21 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c7, c4, 7, 7, 2);
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c4, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e12 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c5, c4, 7, 7, 2);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          e00 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
            e12 = c0;
            e22 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c5, c0, 3, 1);
            e22 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e02 = (c1.IsNotLike(c5)) ? (c2) : (sPixel.Interpolate(c2, c1, c5, 2, 1, 1));
        }
        break;
        case 151:
        case 183: {
          e00 = c3;
          e01 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          e02 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 2, 1, 1));
        }
        break;
        case 158: {
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
            e12 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
          }
        }
        break;
        case 159: {
          e01 = c4;
          e11 = c4;
          e12 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 191: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e11 = c0;
          e12 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 1, 1);
            e21 = sPixel.Interpolate(c7, c0, 3, 1);
            e22 = sPixel.Interpolate(c0, c7, 3, 1);
          }
        }
        break;
        case 201:
        case 205:
        case 233:
        case 237: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e21 = c1;
          e22 = c1;
          e20 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 2, 1, 1));
        }
        break;
        case 211: {
          e00 = c2;
          e01 = c2;
          e02 = c2;
          e10 = c2;
          e11 = c2;
          e20 = c2;
          if (c7.IsNotLike(c5)) {
            e12 = c2;
            e21 = c2;
            e22 = c2;
          } else {
            e12 = sPixel.Interpolate(c2, c5, 7, 1);
            e21 = sPixel.Interpolate(c2, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c2, 7, 7, 2);
          }
        }
        break;
        case 215: {
          e00 = c3;
          e01 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e20 = c3;
          if (c7.IsNotLike(c5)) {
            e21 = c3;
            e22 = c3;
          } else {
            e21 = sPixel.Interpolate(c3, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c3, 7, 7, 2);
          }
          e02 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 2, 1, 1));
        }
        break;
        case 218: {
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e12 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c0, 7, 7, 2);
          }
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 219: {
          e02 = c2;
          e11 = c2;
          e20 = c2;
          if (c7.IsNotLike(c5)) {
            e12 = c2;
            e21 = c2;
            e22 = c2;
          } else {
            e12 = sPixel.Interpolate(c2, c5, 7, 1);
            e21 = sPixel.Interpolate(c2, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c2, 7, 7, 2);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 7, 2);
            e01 = sPixel.Interpolate(c2, c1, 7, 1);
            e10 = sPixel.Interpolate(c2, c3, 7, 1);
          }
        }
        break;
        case 220: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e20 = (c7.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e12 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c0, 7, 7, 2);
          }
        }
        break;
        case 223: {
          e11 = c4;
          e20 = c4;
          if (c7.IsNotLike(c5)) {
            e21 = c4;
            e22 = c4;
          } else {
            e21 = sPixel.Interpolate(c4, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c4, 7, 7, 2);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c4, 7, 7, 2);
            e10 = sPixel.Interpolate(c4, c3, 7, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c4;
            e02 = c4;
            e12 = c4;
          } else {
            e01 = sPixel.Interpolate(c4, c1, 7, 1);
            e02 = sPixel.Interpolate(c4, c1, c5, 2, 1, 1);
            e12 = sPixel.Interpolate(c4, c5, 7, 1);
          }
        }
        break;
        case 234: {
          e01 = c0;
          e02 = c0;
          e11 = c0;
          e12 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
            e21 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c0) : (sPixel.Interpolate(c0, c1, c3, 2, 1, 1));
        }
        break;
        case 235: {
          e02 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e21 = c2;
          e22 = c2;
          e20 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 7, 2);
            e01 = sPixel.Interpolate(c2, c1, 7, 1);
          }
        }
        break;
        case 239: {
          e01 = c4;
          e02 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e22 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 242: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          if (c7.IsNotLike(c5)) {
            e12 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c0, 7, 7, 2);
          }
          e02 = (c1.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c1, c5, 2, 1, 1));
        }
        break;
        case 243: {
          e00 = c2;
          e01 = c2;
          e02 = c2;
          e10 = c2;
          e11 = c2;
          if (c7.IsNotLike(c5)) {
            e12 = c2;
            e20 = c2;
            e21 = c2;
            e22 = c2;
          } else {
            e12 = sPixel.Interpolate(c2, c5, 3, 1);
            e20 = sPixel.Interpolate(c2, c7, 3, 1);
            e21 = sPixel.Interpolate(c7, c2, 3, 1);
            e22 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 246: {
          e00 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
          }
        }
        break;
        case 247: {
          e00 = c3;
          e01 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e20 = c3;
          e21 = c3;
          e22 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 2, 1, 1));
        }
        break;
        case 249: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          e21 = c1;
          e20 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e12 = c1;
            e22 = c1;
          } else {
            e12 = sPixel.Interpolate(c1, c5, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c1, 7, 7, 2);
          }
        }
        break;
        case 251: {
          e02 = c2;
          e11 = c2;
          if (c7.IsNotLike(c3)) {
            e10 = c2;
            e20 = c2;
            e21 = c2;
          } else {
            e10 = sPixel.Interpolate(c2, c3, 7, 1);
            e20 = sPixel.Interpolate(c2, c3, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c2, c7, 7, 1);
          }
          if (c7.IsNotLike(c5)) {
            e12 = c2;
            e22 = c2;
          } else {
            e12 = sPixel.Interpolate(c2, c5, 7, 1);
            e22 = sPixel.Interpolate(c5, c7, c2, 7, 7, 2);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, c2, 7, 7, 2);
            e01 = sPixel.Interpolate(c2, c1, 7, 1);
          }
        }
        break;
        case 252: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e11 = c0;
          e12 = c0;
          e21 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
          }
          e22 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
        }
        break;
        case 254: {
          e00 = c0;
          e11 = c0;
          if (c7.IsNotLike(c3)) {
            e10 = c0;
            e20 = c0;
          } else {
            e10 = sPixel.Interpolate(c0, c3, 7, 1);
            e20 = sPixel.Interpolate(c3, c7, c0, 7, 7, 2);
          }
          if (c7.IsNotLike(c5)) {
            e12 = c0;
            e21 = c0;
            e22 = c0;
          } else {
            e12 = sPixel.Interpolate(c0, c5, 7, 1);
            e21 = sPixel.Interpolate(c0, c7, 7, 1);
            e22 = sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e01 = c0;
            e02 = c0;
          } else {
            e01 = sPixel.Interpolate(c0, c1, 7, 1);
            e02 = sPixel.Interpolate(c1, c5, c0, 7, 7, 2);
          }
        }
        break;
        case 255: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e20 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e22 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e02 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, e02, 
      e10, e11, e12, 
      e20, e21, e22, 
      });
    }
    #endregion
    #region standard LQ4x casepath
    public static sPixel[] Lq4xKernel(byte pattern, sPixel c0, sPixel c1, sPixel c2, sPixel c3, sPixel c4, sPixel c5, sPixel c6, sPixel c7, sPixel c8) {
      sPixel e01, e02, e03, e10, e11, e12, e13, e20, e21, e22, e23, e30, e31, e32, e33;
      var e00 = e01 = e02 = e03 = e10 = e11 = e12 = e13 = e20 = e21 = e22 = e23 = e30 = e31 = e32 = e33 = c4;
      switch (pattern) {
        #region LQ4x PATTERNS

        case 0:
        case 2:
        case 4:
        case 6:
        case 8:
        case 12:
        case 16:
        case 20:
        case 24:
        case 28:
        case 32:
        case 34:
        case 36:
        case 38:
        case 40:
        case 44:
        case 48:
        case 52:
        case 56:
        case 60:
        case 64:
        case 66:
        case 68:
        case 70:
        case 96:
        case 98:
        case 100:
        case 102:
        case 128:
        case 130:
        case 132:
        case 134:
        case 136:
        case 140:
        case 144:
        case 148:
        case 152:
        case 156:
        case 160:
        case 162:
        case 164:
        case 166:
        case 168:
        case 172:
        case 176:
        case 180:
        case 184:
        case 188:
        case 192:
        case 194:
        case 196:
        case 198:
        case 224:
        case 226:
        case 228:
        case 230: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
        }
        break;
        case 1:
        case 5:
        case 9:
        case 13:
        case 17:
        case 21:
        case 25:
        case 29:
        case 33:
        case 37:
        case 41:
        case 45:
        case 49:
        case 53:
        case 57:
        case 61:
        case 65:
        case 69:
        case 97:
        case 101:
        case 129:
        case 133:
        case 137:
        case 141:
        case 145:
        case 149:
        case 153:
        case 157:
        case 161:
        case 165:
        case 169:
        case 173:
        case 177:
        case 181:
        case 185:
        case 189:
        case 193:
        case 197:
        case 225:
        case 229: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
          e23 = c1;
          e30 = c1;
          e31 = c1;
          e32 = c1;
          e33 = c1;
        }
        break;
        case 3:
        case 35:
        case 67:
        case 99:
        case 131:
        case 163:
        case 195:
        case 227: {
          e00 = c2;
          e01 = c2;
          e02 = c2;
          e03 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e30 = c2;
          e31 = c2;
          e32 = c2;
          e33 = c2;
        }
        break;
        case 7:
        case 39:
        case 71:
        case 103:
        case 135:
        case 167:
        case 199:
        case 231: {
          e00 = c3;
          e01 = c3;
          e02 = c3;
          e03 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e13 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          e23 = c3;
          e30 = c3;
          e31 = c3;
          e32 = c3;
          e33 = c3;
        }
        break;
        case 10:
        case 138: {
          e02 = c0;
          e03 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 1, 1);
            e10 = sPixel.Interpolate(c0, c3, 1, 1);
          }
        }
        break;
        case 11:
        case 27:
        case 75:
        case 139:
        case 155:
        case 203: {
          e02 = c2;
          e03 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e30 = c2;
          e31 = c2;
          e32 = c2;
          e33 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
          }
        }
        break;
        case 14:
        case 142: {
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e02 = c0;
            e03 = c0;
            e10 = c0;
            e11 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, 5, 3);
            e02 = sPixel.Interpolate(c1, c0, 3, 1);
            e03 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c0, c1, 2, 1, 1);
            e11 = sPixel.Interpolate(c0, c1, c3, 6, 1, 1);
          }
        }
        break;
        case 15:
        case 143:
        case 207: {
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e30 = c4;
          e31 = c4;
          e32 = c4;
          e33 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e02 = c4;
            e03 = c4;
            e10 = c4;
            e11 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c3, 5, 3);
            e02 = sPixel.Interpolate(c1, c4, 3, 1);
            e03 = sPixel.Interpolate(c4, c1, 3, 1);
            e10 = sPixel.Interpolate(c3, c1, c4, 2, 1, 1);
            e11 = sPixel.Interpolate(c4, c1, c3, 6, 1, 1);
          }
        }
        break;
        case 18:
        case 22:
        case 30:
        case 50:
        case 54:
        case 62:
        case 86:
        case 118: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 19:
        case 51: {
          e10 = c2;
          e11 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e30 = c2;
          e31 = c2;
          e32 = c2;
          e33 = c2;
          if (c1.IsNotLike(c5)) {
            e00 = c2;
            e01 = c2;
            e02 = c2;
            e03 = c2;
            e12 = c2;
            e13 = c2;
          } else {
            e00 = sPixel.Interpolate(c2, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c2, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 5, 3);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c2, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, c2, 2, 1, 1);
          }
        }
        break;
        case 23:
        case 55:
        case 119: {
          e10 = c3;
          e11 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          e23 = c3;
          e30 = c3;
          e31 = c3;
          e32 = c3;
          e33 = c3;
          if (c1.IsNotLike(c5)) {
            e00 = c3;
            e01 = c3;
            e02 = c3;
            e03 = c3;
            e12 = c3;
            e13 = c3;
          } else {
            e00 = sPixel.Interpolate(c3, c1, 3, 1);
            e01 = sPixel.Interpolate(c1, c3, 3, 1);
            e02 = sPixel.Interpolate(c1, c5, 5, 3);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c3, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, c3, 2, 1, 1);
          }
        }
        break;
        case 26: {
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 1, 1);
            e10 = sPixel.Interpolate(c0, c3, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 31:
        case 95: {
          e11 = c4;
          e12 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e30 = c4;
          e31 = c4;
          e32 = c4;
          e33 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 42:
        case 170: {
          e02 = c0;
          e03 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
            e11 = c0;
            e20 = c0;
            e30 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c0, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c1, 5, 3);
            e11 = sPixel.Interpolate(c0, c1, c3, 6, 1, 1);
            e20 = sPixel.Interpolate(c3, c0, 3, 1);
            e30 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 43:
        case 171:
        case 187: {
          e02 = c2;
          e03 = c2;
          e12 = c2;
          e13 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e31 = c2;
          e32 = c2;
          e33 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
            e11 = c2;
            e20 = c2;
            e30 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, c3, 2, 1, 1);
            e10 = sPixel.Interpolate(c3, c1, 5, 3);
            e11 = sPixel.Interpolate(c2, c1, c3, 6, 1, 1);
            e20 = sPixel.Interpolate(c3, c2, 3, 1);
            e30 = sPixel.Interpolate(c2, c3, 3, 1);
          }
        }
        break;
        case 46:
        case 174: {
          e02 = c0;
          e03 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 47:
        case 175: {
          e01 = c4;
          e02 = c4;
          e03 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e30 = c4;
          e31 = c4;
          e32 = c4;
          e33 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 58:
        case 154:
        case 186: {
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 3, 1);
            e03 = sPixel.Interpolate(c0, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 59: {
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e30 = c2;
          e31 = c2;
          e32 = c2;
          e33 = c2;
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c2;
            e03 = c2;
            e13 = c2;
          } else {
            e02 = sPixel.Interpolate(c2, c1, 3, 1);
            e03 = sPixel.Interpolate(c2, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c2, c5, 3, 1);
          }
        }
        break;
        case 63: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e30 = c4;
          e31 = c4;
          e32 = c4;
          e33 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 72:
        case 76:
        case 104:
        case 106:
        case 108:
        case 110:
        case 120:
        case 124: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e32 = c0;
          e33 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
        }
        break;
        case 73:
        case 77:
        case 105:
        case 109:
        case 125: {
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e22 = c1;
          e23 = c1;
          e32 = c1;
          e33 = c1;
          if (c7.IsNotLike(c3)) {
            e00 = c1;
            e10 = c1;
            e20 = c1;
            e21 = c1;
            e30 = c1;
            e31 = c1;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 3, 1);
            e10 = sPixel.Interpolate(c3, c1, 3, 1);
            e20 = sPixel.Interpolate(c3, c7, 5, 3);
            e21 = sPixel.Interpolate(c1, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c1, c3, 2, 1, 1);
          }
        }
        break;
        case 74: {
          e02 = c0;
          e03 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e32 = c0;
          e33 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 1, 1);
            e10 = sPixel.Interpolate(c0, c3, 1, 1);
          }
        }
        break;
        case 78:
        case 202:
        case 206: {
          e02 = c0;
          e03 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e32 = c0;
          e33 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c0, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 79: {
          e02 = c4;
          e03 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e32 = c4;
          e33 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c4, c3, 3, 1);
            e30 = sPixel.Interpolate(c4, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 3, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
        }
        break;
        case 80:
        case 208:
        case 210:
        case 216: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e30 = c0;
          e31 = c0;
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 1, 1);
            e32 = sPixel.Interpolate(c0, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 81:
        case 209:
        case 217: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
          e30 = c1;
          e31 = c1;
          if (c7.IsNotLike(c5)) {
            e23 = c1;
            e32 = c1;
            e33 = c1;
          } else {
            e23 = sPixel.Interpolate(c1, c5, 1, 1);
            e32 = sPixel.Interpolate(c1, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 82:
        case 214:
        case 222: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e30 = c0;
          e31 = c0;
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 1, 1);
            e32 = sPixel.Interpolate(c0, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 83:
        case 115: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e30 = c2;
          e31 = c2;
          if (c7.IsNotLike(c5)) {
            e23 = c2;
            e32 = c2;
            e33 = c2;
          } else {
            e23 = sPixel.Interpolate(c2, c5, 3, 1);
            e32 = sPixel.Interpolate(c2, c7, 3, 1);
            e33 = sPixel.Interpolate(c2, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c2;
            e03 = c2;
            e13 = c2;
          } else {
            e02 = sPixel.Interpolate(c2, c1, 3, 1);
            e03 = sPixel.Interpolate(c2, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c2, c5, 3, 1);
          }
        }
        break;
        case 84:
        case 212: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e30 = c0;
          e31 = c0;
          if (c7.IsNotLike(c5)) {
            e03 = c0;
            e13 = c0;
            e22 = c0;
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e03 = sPixel.Interpolate(c0, c5, 3, 1);
            e13 = sPixel.Interpolate(c5, c0, 3, 1);
            e22 = sPixel.Interpolate(c0, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c7, 5, 3);
            e32 = sPixel.Interpolate(c7, c0, c5, 2, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 85:
        case 213:
        case 221: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e20 = c1;
          e21 = c1;
          e30 = c1;
          e31 = c1;
          if (c7.IsNotLike(c5)) {
            e03 = c1;
            e13 = c1;
            e22 = c1;
            e23 = c1;
            e32 = c1;
            e33 = c1;
          } else {
            e03 = sPixel.Interpolate(c1, c5, 3, 1);
            e13 = sPixel.Interpolate(c5, c1, 3, 1);
            e22 = sPixel.Interpolate(c1, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c7, 5, 3);
            e32 = sPixel.Interpolate(c7, c1, c5, 2, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 87: {
          e00 = c3;
          e01 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          e30 = c3;
          e31 = c3;
          if (c7.IsNotLike(c5)) {
            e23 = c3;
            e32 = c3;
            e33 = c3;
          } else {
            e23 = sPixel.Interpolate(c3, c5, 3, 1);
            e32 = sPixel.Interpolate(c3, c7, 3, 1);
            e33 = sPixel.Interpolate(c3, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c3;
            e03 = c3;
            e13 = c3;
          } else {
            e02 = sPixel.Interpolate(c1, c3, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c3, c5, 1, 1);
          }
        }
        break;
        case 88:
        case 248:
        case 250: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 1, 1);
            e32 = sPixel.Interpolate(c0, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 89:
        case 93: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e21 = c1;
          e22 = c1;
          if (c7.IsNotLike(c3)) {
            e20 = c1;
            e30 = c1;
            e31 = c1;
          } else {
            e20 = sPixel.Interpolate(c1, c3, 3, 1);
            e30 = sPixel.Interpolate(c1, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c1, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c1;
            e32 = c1;
            e33 = c1;
          } else {
            e23 = sPixel.Interpolate(c1, c5, 3, 1);
            e32 = sPixel.Interpolate(c1, c7, 3, 1);
            e33 = sPixel.Interpolate(c1, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 90: {
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c0, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 3, 1);
            e32 = sPixel.Interpolate(c0, c7, 3, 1);
            e33 = sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 3, 1);
            e03 = sPixel.Interpolate(c0, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 91: {
          e11 = c2;
          e12 = c2;
          e21 = c2;
          e22 = c2;
          if (c7.IsNotLike(c3)) {
            e20 = c2;
            e30 = c2;
            e31 = c2;
          } else {
            e20 = sPixel.Interpolate(c2, c3, 3, 1);
            e30 = sPixel.Interpolate(c2, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c2, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c2;
            e32 = c2;
            e33 = c2;
          } else {
            e23 = sPixel.Interpolate(c2, c5, 3, 1);
            e32 = sPixel.Interpolate(c2, c7, 3, 1);
            e33 = sPixel.Interpolate(c2, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c2;
            e03 = c2;
            e13 = c2;
          } else {
            e02 = sPixel.Interpolate(c2, c1, 3, 1);
            e03 = sPixel.Interpolate(c2, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c2, c5, 3, 1);
          }
        }
        break;
        case 92: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c0, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 3, 1);
            e32 = sPixel.Interpolate(c0, c7, 3, 1);
            e33 = sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 94: {
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c0, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 3, 1);
            e32 = sPixel.Interpolate(c0, c7, 3, 1);
            e33 = sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 107:
        case 123: {
          e02 = c2;
          e03 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e32 = c2;
          e33 = c2;
          if (c7.IsNotLike(c3)) {
            e20 = c2;
            e30 = c2;
            e31 = c2;
          } else {
            e20 = sPixel.Interpolate(c2, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c2, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
          }
        }
        break;
        case 111: {
          e01 = c4;
          e02 = c4;
          e03 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e32 = c4;
          e33 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 112:
        case 240: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          if (c7.IsNotLike(c5)) {
            e22 = c0;
            e23 = c0;
            e30 = c0;
            e31 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e22 = sPixel.Interpolate(c0, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c0, c7, 2, 1, 1);
            e30 = sPixel.Interpolate(c0, c7, 3, 1);
            e31 = sPixel.Interpolate(c7, c0, 3, 1);
            e32 = sPixel.Interpolate(c7, c5, 5, 3);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 113:
        case 241: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          if (c7.IsNotLike(c5)) {
            e22 = c1;
            e23 = c1;
            e30 = c1;
            e31 = c1;
            e32 = c1;
            e33 = c1;
          } else {
            e22 = sPixel.Interpolate(c1, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c1, c7, 2, 1, 1);
            e30 = sPixel.Interpolate(c1, c7, 3, 1);
            e31 = sPixel.Interpolate(c7, c1, 3, 1);
            e32 = sPixel.Interpolate(c7, c5, 5, 3);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 114: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e30 = c0;
          e31 = c0;
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 3, 1);
            e32 = sPixel.Interpolate(c0, c7, 3, 1);
            e33 = sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 3, 1);
            e03 = sPixel.Interpolate(c0, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 116: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e30 = c0;
          e31 = c0;
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 3, 1);
            e32 = sPixel.Interpolate(c0, c7, 3, 1);
            e33 = sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 117: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
          e30 = c1;
          e31 = c1;
          if (c7.IsNotLike(c5)) {
            e23 = c1;
            e32 = c1;
            e33 = c1;
          } else {
            e23 = sPixel.Interpolate(c1, c5, 3, 1);
            e32 = sPixel.Interpolate(c1, c7, 3, 1);
            e33 = sPixel.Interpolate(c1, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 121: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e21 = c1;
          e22 = c1;
          if (c7.IsNotLike(c3)) {
            e20 = c1;
            e30 = c1;
            e31 = c1;
          } else {
            e20 = sPixel.Interpolate(c1, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c1, c7, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c1;
            e32 = c1;
            e33 = c1;
          } else {
            e23 = sPixel.Interpolate(c1, c5, 3, 1);
            e32 = sPixel.Interpolate(c1, c7, 3, 1);
            e33 = sPixel.Interpolate(c1, c5, c7, 2, 1, 1);
          }
        }
        break;
        case 122: {
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 3, 1);
            e32 = sPixel.Interpolate(c0, c7, 3, 1);
            e33 = sPixel.Interpolate(c0, c5, c7, 2, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 3, 1);
            e03 = sPixel.Interpolate(c0, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 126: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e32 = c0;
          e33 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 127: {
          e01 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e32 = c4;
          e33 = c4;
          if (c7.IsNotLike(c3)) {
            e20 = c4;
            e30 = c4;
            e31 = c4;
          } else {
            e20 = sPixel.Interpolate(c3, c4, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c4, c7, 1, 1);
          }
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c4;
            e03 = c4;
            e13 = c4;
          } else {
            e02 = sPixel.Interpolate(c1, c4, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c4, c5, 1, 1);
          }
        }
        break;
        case 146:
        case 150:
        case 178:
        case 182:
        case 190: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e12 = c0;
            e13 = c0;
            e23 = c0;
            e33 = c0;
          } else {
            e02 = sPixel.Interpolate(c1, c0, c5, 2, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e12 = sPixel.Interpolate(c0, c1, c5, 6, 1, 1);
            e13 = sPixel.Interpolate(c5, c1, 5, 3);
            e23 = sPixel.Interpolate(c5, c0, 3, 1);
            e33 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 147:
        case 179: {
          e00 = c2;
          e01 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e30 = c2;
          e31 = c2;
          e32 = c2;
          e33 = c2;
          if (c1.IsNotLike(c5)) {
            e02 = c2;
            e03 = c2;
            e13 = c2;
          } else {
            e02 = sPixel.Interpolate(c2, c1, 3, 1);
            e03 = sPixel.Interpolate(c2, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c2, c5, 3, 1);
          }
        }
        break;
        case 151:
        case 183: {
          e00 = c3;
          e01 = c3;
          e02 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e13 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          e23 = c3;
          e30 = c3;
          e31 = c3;
          e32 = c3;
          e33 = c3;
          e03 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 2, 1, 1));
        }
        break;
        case 158: {
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = c0;
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 159: {
          e02 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e30 = c4;
          e31 = c4;
          e32 = c4;
          e33 = c4;
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 191: {
          e01 = c4;
          e02 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e30 = c4;
          e31 = c4;
          e32 = c4;
          e33 = c4;
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 200:
        case 204:
        case 232:
        case 236:
        case 238: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e22 = c0;
          e23 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e21 = c0;
            e30 = c0;
            e31 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e20 = sPixel.Interpolate(c3, c0, c7, 2, 1, 1);
            e21 = sPixel.Interpolate(c0, c3, c7, 6, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c7, c3, 5, 3);
            e32 = sPixel.Interpolate(c7, c0, 3, 1);
            e33 = sPixel.Interpolate(c0, c7, 3, 1);
          }
        }
        break;
        case 201:
        case 205: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e21 = c1;
          e22 = c1;
          e23 = c1;
          e32 = c1;
          e33 = c1;
          if (c7.IsNotLike(c3)) {
            e20 = c1;
            e30 = c1;
            e31 = c1;
          } else {
            e20 = sPixel.Interpolate(c1, c3, 3, 1);
            e30 = sPixel.Interpolate(c1, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c1, c7, 3, 1);
          }
        }
        break;
        case 211: {
          e00 = c2;
          e01 = c2;
          e02 = c2;
          e03 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e30 = c2;
          e31 = c2;
          if (c7.IsNotLike(c5)) {
            e23 = c2;
            e32 = c2;
            e33 = c2;
          } else {
            e23 = sPixel.Interpolate(c2, c5, 1, 1);
            e32 = sPixel.Interpolate(c2, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 215: {
          e00 = c3;
          e01 = c3;
          e02 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e13 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          e30 = c3;
          e31 = c3;
          if (c7.IsNotLike(c5)) {
            e23 = c3;
            e32 = c3;
            e33 = c3;
          } else {
            e23 = sPixel.Interpolate(c3, c5, 1, 1);
            e32 = sPixel.Interpolate(c3, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          e03 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 2, 1, 1));
        }
        break;
        case 218: {
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c0, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 1, 1);
            e32 = sPixel.Interpolate(c0, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 3, 1);
            e03 = sPixel.Interpolate(c0, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 219: {
          e02 = c2;
          e03 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e30 = c2;
          e31 = c2;
          if (c7.IsNotLike(c5)) {
            e23 = c2;
            e32 = c2;
            e33 = c2;
          } else {
            e23 = sPixel.Interpolate(c2, c5, 1, 1);
            e32 = sPixel.Interpolate(c2, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
          }
        }
        break;
        case 220: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 3, 1);
            e30 = sPixel.Interpolate(c0, c3, c7, 2, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 3, 1);
          }
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 1, 1);
            e32 = sPixel.Interpolate(c0, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 223: {
          e02 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e30 = c4;
          e31 = c4;
          if (c7.IsNotLike(c5)) {
            e23 = c4;
            e32 = c4;
            e33 = c4;
          } else {
            e23 = sPixel.Interpolate(c4, c5, 1, 1);
            e32 = sPixel.Interpolate(c4, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c4;
            e01 = c4;
            e10 = c4;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c4, 1, 1);
            e10 = sPixel.Interpolate(c3, c4, 1, 1);
          }
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        case 233:
        case 237: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
          e23 = c1;
          e31 = c1;
          e32 = c1;
          e33 = c1;
          e30 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 2, 1, 1));
        }
        break;
        case 234: {
          e02 = c0;
          e03 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e32 = c0;
          e33 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c0;
            e01 = c0;
            e10 = c0;
          } else {
            e00 = sPixel.Interpolate(c0, c1, c3, 2, 1, 1);
            e01 = sPixel.Interpolate(c0, c1, 3, 1);
            e10 = sPixel.Interpolate(c0, c3, 3, 1);
          }
        }
        break;
        case 235: {
          e02 = c2;
          e03 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e23 = c2;
          e31 = c2;
          e32 = c2;
          e33 = c2;
          e30 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 2, 1, 1));
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
          }
        }
        break;
        case 239: {
          e01 = c4;
          e02 = c4;
          e03 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e31 = c4;
          e32 = c4;
          e33 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
        }
        break;
        case 242: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e30 = c0;
          e31 = c0;
          if (c7.IsNotLike(c5)) {
            e23 = c0;
            e32 = c0;
            e33 = c0;
          } else {
            e23 = sPixel.Interpolate(c0, c5, 1, 1);
            e32 = sPixel.Interpolate(c0, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 3, 1);
            e03 = sPixel.Interpolate(c0, c1, c5, 2, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 3, 1);
          }
        }
        break;
        case 243: {
          e00 = c2;
          e01 = c2;
          e02 = c2;
          e03 = c2;
          e10 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e20 = c2;
          e21 = c2;
          if (c7.IsNotLike(c5)) {
            e22 = c2;
            e23 = c2;
            e30 = c2;
            e31 = c2;
            e32 = c2;
            e33 = c2;
          } else {
            e22 = sPixel.Interpolate(c2, c5, c7, 6, 1, 1);
            e23 = sPixel.Interpolate(c5, c2, c7, 2, 1, 1);
            e30 = sPixel.Interpolate(c2, c7, 3, 1);
            e31 = sPixel.Interpolate(c7, c2, 3, 1);
            e32 = sPixel.Interpolate(c7, c5, 5, 3);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 244: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
        }
        break;
        case 245: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
          e23 = c1;
          e30 = c1;
          e31 = c1;
          e32 = c1;
          e33 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 2, 1, 1));
        }
        break;
        case 246: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e20 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e30 = c0;
          e31 = c0;
          e32 = c0;
          e33 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 247: {
          e00 = c3;
          e01 = c3;
          e02 = c3;
          e10 = c3;
          e11 = c3;
          e12 = c3;
          e13 = c3;
          e20 = c3;
          e21 = c3;
          e22 = c3;
          e23 = c3;
          e30 = c3;
          e31 = c3;
          e32 = c3;
          e33 = (c7.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c5, c7, 2, 1, 1));
          e03 = (c1.IsNotLike(c5)) ? (c3) : (sPixel.Interpolate(c3, c1, c5, 2, 1, 1));
        }
        break;
        case 249: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
          e31 = c1;
          e30 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e23 = c1;
            e32 = c1;
            e33 = c1;
          } else {
            e23 = sPixel.Interpolate(c1, c5, 1, 1);
            e32 = sPixel.Interpolate(c1, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
        }
        break;
        case 251: {
          e02 = c2;
          e03 = c2;
          e11 = c2;
          e12 = c2;
          e13 = c2;
          e20 = c2;
          e21 = c2;
          e22 = c2;
          e31 = c2;
          e30 = (c7.IsNotLike(c3)) ? (c2) : (sPixel.Interpolate(c2, c3, c7, 2, 1, 1));
          if (c7.IsNotLike(c5)) {
            e23 = c2;
            e32 = c2;
            e33 = c2;
          } else {
            e23 = sPixel.Interpolate(c2, c5, 1, 1);
            e32 = sPixel.Interpolate(c2, c7, 1, 1);
            e33 = sPixel.Interpolate(c5, c7, 1, 1);
          }
          if (c1.IsNotLike(c3)) {
            e00 = c2;
            e01 = c2;
            e10 = c2;
          } else {
            e00 = sPixel.Interpolate(c1, c3, 1, 1);
            e01 = sPixel.Interpolate(c1, c2, 1, 1);
            e10 = sPixel.Interpolate(c2, c3, 1, 1);
          }
        }
        break;
        case 252: {
          e00 = c0;
          e01 = c0;
          e02 = c0;
          e03 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e13 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e32 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
          e33 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
        }
        break;
        case 253: {
          e00 = c1;
          e01 = c1;
          e02 = c1;
          e03 = c1;
          e10 = c1;
          e11 = c1;
          e12 = c1;
          e13 = c1;
          e20 = c1;
          e21 = c1;
          e22 = c1;
          e23 = c1;
          e31 = c1;
          e32 = c1;
          e30 = (c7.IsNotLike(c3)) ? (c1) : (sPixel.Interpolate(c1, c3, c7, 2, 1, 1));
          e33 = (c7.IsNotLike(c5)) ? (c1) : (sPixel.Interpolate(c1, c5, c7, 2, 1, 1));
        }
        break;
        case 254: {
          e00 = c0;
          e01 = c0;
          e10 = c0;
          e11 = c0;
          e12 = c0;
          e21 = c0;
          e22 = c0;
          e23 = c0;
          e32 = c0;
          if (c7.IsNotLike(c3)) {
            e20 = c0;
            e30 = c0;
            e31 = c0;
          } else {
            e20 = sPixel.Interpolate(c0, c3, 1, 1);
            e30 = sPixel.Interpolate(c3, c7, 1, 1);
            e31 = sPixel.Interpolate(c0, c7, 1, 1);
          }
          e33 = (c7.IsNotLike(c5)) ? (c0) : (sPixel.Interpolate(c0, c5, c7, 2, 1, 1));
          if (c1.IsNotLike(c5)) {
            e02 = c0;
            e03 = c0;
            e13 = c0;
          } else {
            e02 = sPixel.Interpolate(c0, c1, 1, 1);
            e03 = sPixel.Interpolate(c1, c5, 1, 1);
            e13 = sPixel.Interpolate(c0, c5, 1, 1);
          }
        }
        break;
        case 255: {
          e01 = c4;
          e02 = c4;
          e10 = c4;
          e11 = c4;
          e12 = c4;
          e13 = c4;
          e20 = c4;
          e21 = c4;
          e22 = c4;
          e23 = c4;
          e31 = c4;
          e32 = c4;
          e30 = (c7.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c3, c7, 2, 1, 1));
          e33 = (c7.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c5, c7, 2, 1, 1));
          e00 = (c1.IsNotLike(c3)) ? (c4) : (sPixel.Interpolate(c4, c1, c3, 2, 1, 1));
          e03 = (c1.IsNotLike(c5)) ? (c4) : (sPixel.Interpolate(c4, c1, c5, 2, 1, 1));
        }
        break;
        #endregion
      }
      return (new[]{
      e00, e01, e02, e03, 
      e10, e11, e12, e13, 
      e20, e21, e22, e23, 
      e30, e31, e32, e33, 
      });
    }
    #endregion

    #endregion

  } // end class
} // end namespace
