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
  internal static class libBasic {
    /// <summary>
    /// Horizontal scanlines
    /// </summary>
    public static void HorizontalScanlines(PixelWorker<sPixel> worker , float grayFactor) {
      var pixel = worker.SourceP0P0();
      worker.TargetP0P0( pixel);
      var factor = grayFactor / 100f + 1f;
      worker.TargetP0P1(pixel * factor);
    }

    /// <summary>
    /// Vertical scanlines
    /// </summary>
    public static void VerticalScanlines(PixelWorker<sPixel>worker , float grayFactor) {
      var pixel = worker.SourceP0P0();
      worker.TargetP0P0(pixel);
      var factor = grayFactor / 100f + 1f;
      worker.TargetP1P0( pixel * factor);
    }

  }
}
