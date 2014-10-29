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
  internal static class libSNES9x {
    /// <summary>
    /// SNES9x's EPXB modified by Hawkynt to support thresholds
    /// </summary>
    public static void EpxB(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY) {
      var c0 = sourceImage[srcX - 1, srcY - 1];
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c2 = sourceImage[srcX + 1, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c6 = sourceImage[srcX - 1, srcY + 1];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      var c8 = sourceImage[srcX + 1, srcY + 1];
      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;
      if (
    c3.IsNotLike(c5) &&
    c1.IsNotLike(c7) && ( // diagonal
      (
        c4.IsLike(c3) ||
        c4.IsLike(c7) ||
        c4.IsLike(c5) ||
        c4.IsLike(c1) || ( // edge smoothing
          (
            c0.IsNotLike(c8) ||
            c4.IsLike(c6) ||
            c4.IsLike(c2)
          ) && (
            c6.IsNotLike(c2) ||
            c4.IsLike(c0) ||
            c4.IsLike(c8)
          )
        )
      )
    )
  ) {
        if (
          c1.IsLike(c3) && (
            c4.IsNotLike(c0) ||
            c4.IsNotLike(c8) ||
            c1.IsNotLike(c2) ||
            c3.IsNotLike(c6)
          )
        ) {
          e00 = sPixel.Interpolate(c1, c3);
        }
        if (
          c5.IsLike(c1) && (
            c4.IsNotLike(c2) ||
            c4.IsNotLike(c6) ||
            c5.IsNotLike(c8) ||
            c1.IsNotLike(c0)
          )
        ) {
          e01 = sPixel.Interpolate(c5, c1);
        }
        if (
          c3.IsLike(c7) && (
            c4.IsNotLike(c6) ||
            c4.IsNotLike(c2) ||
            c3.IsNotLike(c0) ||
            c7.IsNotLike(c8)
          )
        ) {
          e10 = sPixel.Interpolate(c3, c7);
        }
        if (
          c7.IsLike(c5) && (
            c4.IsNotLike(c8) ||
            c4.IsNotLike(c0) ||
            c7.IsNotLike(c6) ||
            c5.IsNotLike(c2)
          )
        ) {
          e11 = sPixel.Interpolate(c7, c5);
        }
      }

      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
    }

    /// <summary>
    /// SNES9x's EPX3 modified by Hawkynt to support thresholds
    /// </summary>
    public static void Epx3(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY) {
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

      if (c3.IsNotLike(c5) && c7.IsNotLike(c1)) {
        var neq40 = c4.IsNotLike(c0);
        var neq41 = c4.IsNotLike(c1);
        var neq42 = c4.IsNotLike(c2);
        var neq43 = c4.IsNotLike(c3);
        var neq45 = c4.IsNotLike(c5);
        var neq46 = c4.IsNotLike(c6);
        var neq47 = c4.IsNotLike(c7);
        var neq48 = c4.IsNotLike(c8);

        var eq13 = c1.IsLike(c3) && (neq40 || neq48 || c1.IsNotLike(c2) || c3.IsNotLike(c6));
        var eq37 = c3.IsLike(c7) && (neq46 || neq42 || c3.IsNotLike(c0) || c7.IsNotLike(c8));
        var eq75 = c7.IsLike(c5) && (neq48 || neq40 || c7.IsNotLike(c6) || c5.IsNotLike(c2));
        var eq51 = c5.IsLike(c1) && (neq42 || neq46 || c5.IsNotLike(c8) || c1.IsNotLike(c0));
        if (
          (!neq40) ||
          (!neq41) ||
          (!neq42) ||
          (!neq43) ||
          (!neq45) ||
          (!neq46) ||
          (!neq47) ||
          (!neq48)
        ) {
          if (eq13)
            e00 = sPixel.Interpolate(c1, c3);
          if (eq51)
            e02 = sPixel.Interpolate(c5, c1);
          if (eq37)
            e20 = sPixel.Interpolate(c3, c7);
          if (eq75)
            e22 = sPixel.Interpolate(c7, c5);

          if ((eq51 && neq40) && (eq13 && neq42))
            e01 = sPixel.Interpolate(c1, c3, c5);
          else if (eq51 && neq40)
            e01 = sPixel.Interpolate(c1, c5);
          else if (eq13 && neq42)
            e01 = sPixel.Interpolate(c1, c3);

          if ((eq13 && neq46) && (eq37 && neq40))
            e10 = sPixel.Interpolate(c3, c1, c7);
          else if (eq13 && neq46)
            e10 = sPixel.Interpolate(c3, c1);
          else if (eq37 && neq40)
            e10 = sPixel.Interpolate(c3, c7);

          if ((eq75 && neq42) && (eq51 && neq48))
            e12 = sPixel.Interpolate(c5, c1, c7);
          else if (eq75 && neq42)
            e12 = sPixel.Interpolate(c5, c7);
          else if (eq51 && neq48)
            e12 = sPixel.Interpolate(c5, c1);

          if ((eq37 && neq48) && (eq75 && neq46))
            e21 = sPixel.Interpolate(c7, c3, c5);
          else if (eq75 && neq46)
            e21 = sPixel.Interpolate(c7, c5);
          else if (eq37 && neq48)
            e21 = sPixel.Interpolate(c7, c3);

        } else {
          if (eq13)
            e00 = sPixel.Interpolate(c1, c3);
          if (eq51)
            e02 = sPixel.Interpolate(c5, c1);
          if (eq37)
            e20 = sPixel.Interpolate(c3, c7);
          if (eq75)
            e22 = sPixel.Interpolate(c7, c5);
        }
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
    /// SNES9x's EPXC modified by Hawkynt to support thresholds
    /// </summary>
    public static void EpxC(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY) {
      var c0 = sourceImage[srcX - 1, srcY - 1];
      var c1 = sourceImage[srcX + 0, srcY - 1];
      var c2 = sourceImage[srcX + 1, srcY - 1];
      var c3 = sourceImage[srcX - 1, srcY + 0];
      var c4 = sourceImage[srcX + 0, srcY + 0];
      var c5 = sourceImage[srcX + 1, srcY + 0];
      var c6 = sourceImage[srcX - 1, srcY + 1];
      var c7 = sourceImage[srcX + 0, srcY + 1];
      var c8 = sourceImage[srcX + 1, srcY + 1];
      sPixel e01, e10, e11;
      var e00 = e01 = e10 = e11 = c4;

      if (c3.IsNotLike(c5) && c7.IsNotLike(c1)) {
        var neq40 = c4.IsNotLike(c0);
        var neq41 = c4.IsNotLike(c1);
        var neq42 = c4.IsNotLike(c2);
        var neq43 = c4.IsNotLike(c3);
        var neq45 = c4.IsNotLike(c5);
        var neq46 = c4.IsNotLike(c6);
        var neq47 = c4.IsNotLike(c7);
        var neq48 = c4.IsNotLike(c8);

        var eq13 = c1.IsLike(c3) && (neq40 || neq48 || c1.IsNotLike(c2) || c3.IsNotLike(c6));
        var eq37 = c3.IsLike(c7) && (neq46 || neq42 || c3.IsNotLike(c0) || c7.IsNotLike(c8));
        var eq75 = c7.IsLike(c5) && (neq48 || neq40 || c7.IsNotLike(c6) || c5.IsNotLike(c2));
        var eq51 = c5.IsLike(c1) && (neq42 || neq46 || c5.IsNotLike(c8) || c1.IsNotLike(c0));
        if (
          (!neq40) ||
          (!neq41) ||
          (!neq42) ||
          (!neq43) ||
          (!neq45) ||
          (!neq46) ||
          (!neq47) ||
          (!neq48)
        ) {
          sPixel c3A;
          if ((eq13 && neq46) && (eq37 && neq40))
            c3A = sPixel.Interpolate(c3, c1, c7);
          else if (eq13 && neq46)
            c3A = sPixel.Interpolate(c3, c1);
          else if (eq37 && neq40)
            c3A = sPixel.Interpolate(c3, c7);
          else
            c3A = c4;

          sPixel c7B;
          if ((eq37 && neq48) && (eq75 && neq46))
            c7B = sPixel.Interpolate(c7, c3, c5);
          else if (eq37 && neq48)
            c7B = sPixel.Interpolate(c7, c3);
          else if (eq75 && neq46)
            c7B = sPixel.Interpolate(c7, c5);
          else
            c7B = c4;

          sPixel c5C;
          if ((eq75 && neq42) && (eq51 && neq48))
            c5C = sPixel.Interpolate(c5, c1, c7);
          else if (eq75 && neq42)
            c5C = sPixel.Interpolate(c5, c7);
          else if (eq51 && neq48)
            c5C = sPixel.Interpolate(c5, c1);
          else
            c5C = c4;

          sPixel c1D;

          if ((eq51 && neq40) && (eq13 && neq42))
            c1D = sPixel.Interpolate(c1, c3, c5);
          else if (eq51 && neq40)
            c1D = sPixel.Interpolate(c1, c5);
          else if (eq13 && neq42)
            c1D = sPixel.Interpolate(c1, c3);
          else
            c1D = c4;

          if (eq13)
            e00 = sPixel.Interpolate(c1, c3);
          if (eq51)
            e01 = sPixel.Interpolate(c5, c1);
          if (eq37)
            e10 = sPixel.Interpolate(c3, c7);
          if (eq75)
            e11 = sPixel.Interpolate(c7, c5);

          e00 = sPixel.Interpolate(e00, c1D, c3A, c4, 5, 1, 1, 1);
          e01 = sPixel.Interpolate(e01, c7B, c5C, c4, 5, 1, 1, 1);
          e10 = sPixel.Interpolate(e10, c3A, c7B, c4, 5, 1, 1, 1);
          e11 = sPixel.Interpolate(e11, c5C, c1D, c4, 5, 1, 1, 1);

        } else {

          if (eq13)
            e00 = sPixel.Interpolate(c1, c3);
          if (eq51)
            e01 = sPixel.Interpolate(c5, c1);
          if (eq37)
            e10 = sPixel.Interpolate(c3, c7);
          if (eq75)
            e11 = sPixel.Interpolate(c7, c5);

          e00 = sPixel.Interpolate(c4, e00, 3, 1);
          e01 = sPixel.Interpolate(c4, e01, 3, 1);
          e10 = sPixel.Interpolate(c4, e10, 3, 1);
          e11 = sPixel.Interpolate(c4, e11, 3, 1);

        }
      }

      targetImage[tgtX + 0, tgtY + 0] = e00;
      targetImage[tgtX + 1, tgtY + 0] = e01;
      targetImage[tgtX + 0, tgtY + 1] = e10;
      targetImage[tgtX + 1, tgtY + 1] = e11;
    }


  } // end class
} // end namespace
