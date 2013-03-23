#region (c)2008-2013 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010-2013 Hawkynt

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
  static class libBasic {
    /// <summary>
    /// Horizontal scanlines
    /// </summary>
    public static void HorizontalScanlines(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, float grayFactor) {
      var pixel = sourceImage[srcX, srcY];
      targetImage[tgtX, tgtY] = pixel;
      var factor = grayFactor / 100f + 1f;
      targetImage[tgtX, tgtY + 1] = pixel * factor;
    }

    /// <summary>
    /// Vertical scanlines
    /// </summary>
    public static void VerticalScanlines(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY, float grayFactor) {
      var pixel = sourceImage[srcX, srcY];
      targetImage[tgtX, tgtY] = pixel;
      var factor = grayFactor / 100f + 1f;
      targetImage[tgtX + 1, tgtY] = pixel * factor;
    }

  }
}
