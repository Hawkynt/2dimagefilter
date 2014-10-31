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
  internal static class libHawkynt {
    /// <summary>
    /// just a bad old-school TV effect
    /// </summary>
    public static void Tv2x(PixelWorker<sPixel> worker) {
      var pixel = worker.SourceP0P0();
      var luminance = pixel.Luminance;
      worker.TargetP0P0(new sPixel(pixel.Red, 0, 0, pixel.Alpha));
      worker.TargetP1P0(new sPixel(0, pixel.Green, 0, pixel.Alpha));
      worker.TargetP0P1(new sPixel(0, 0, pixel.Blue, pixel.Alpha));
      worker.TargetP1P1(sPixel.FromGrey(luminance, pixel.Alpha));
    }

    /// <summary>
    /// another bad one a made for MS-Dos in 1998
    /// </summary>
    public static void Tv3x(PixelWorker<sPixel> worker) {
      var pixel = worker.SourceP0P0();
      worker.TargetP0P0(new sPixel(pixel.Red, 0, 0, pixel.Alpha));
      worker.TargetP1P0(new sPixel(0, pixel.Green, 0, pixel.Alpha));
      worker.TargetP2P0(new sPixel(0, 0, pixel.Blue, pixel.Alpha));
      if ((worker.SourceX() & 1) == 0) {
        worker.TargetP0P1(new sPixel(pixel.Red, 0, 0, pixel.Alpha));
        if (worker.SourceY() > 0)
          worker.TargetP1M1(new sPixel(0, pixel.Green, 0, pixel.Alpha));
        worker.TargetP2P1(new sPixel(0, 0, pixel.Blue, pixel.Alpha));
      } else {
        if (worker.SourceY() > 0)
          worker.TargetP0M1(new sPixel(pixel.Red, 0, 0, pixel.Alpha));
        worker.TargetP1P1(new sPixel(0, pixel.Green, 0, pixel.Alpha));
        if (worker.SourceY() > 0)
          worker.TargetP2M1(new sPixel(0, 0, pixel.Blue, pixel.Alpha));
      }

    }

  }
}
