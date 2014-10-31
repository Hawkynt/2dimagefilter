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
  internal static class libVBA {
    /// <summary>
    /// Bilinear Plus Original
    /// </summary>
    public static void BilinearPlusOriginal(PixelWorker<sPixel>worker ) {
      var c00 = worker.SourceP0P0();
      var c01 = worker.SourceP1P0();
      var c10 = worker.SourceP0P1();
      var c11 = worker.SourceP1P1();

      var e00 = sPixel.Interpolate(c00, c01, c10, 5, 2, 1);
      var e01 = sPixel.Interpolate(c00, c01);
      var e10 = sPixel.Interpolate(c00, c10);
      var e11 = sPixel.Interpolate(c00, c01, c10, c11);

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }

    /// <summary>
    /// Bilinear Plus
    /// </summary>
    public static void BilinearPlus(PixelWorker<sPixel>worker ) {
      var c00 = worker.SourceP0P0();
      var c01 = worker.SourceP1P0();
      var c10 = worker.SourceP0P1();
      var c11 = worker.SourceP1P1();

      const float gamma = 14f / 16f;

      var e00 = sPixel.Interpolate(c00, c01, c10, 10, 2, 2) * gamma;
      var e01 = sPixel.Interpolate(c00, c01);
      var e10 = sPixel.Interpolate(c00, c10);
      var e11 = sPixel.Interpolate(c00, c01, c10, c11);

      worker.TargetP0P0(e00);
      worker.TargetP1P0(e01);
      worker.TargetP0P1(e10);
      worker.TargetP1P1(e11);
    }
  } // end class
} // end namespace
