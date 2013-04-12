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
  internal static class libHawkynt {
    /// <summary>
    /// just a bad old-school TV effect
    /// </summary>
    public static void Tv2x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY) {
      var pixel = sourceImage[srcX, srcY];
      var luminance = pixel.Luminance;
      targetImage[tgtX + 0, tgtY + 0] = new sPixel(pixel.Red, 0, 0, pixel.Alpha);
      targetImage[tgtX + 1, tgtY + 0] = new sPixel(0, pixel.Green, 0, pixel.Alpha);
      targetImage[tgtX + 0, tgtY + 1] = new sPixel(0, 0, pixel.Blue, pixel.Alpha);
      targetImage[tgtX + 1, tgtY + 1] = sPixel.FromGrey(luminance, pixel.Alpha);
    }

    /// <summary>
    /// another bad one a made for MS-Dos in 1998
    /// </summary>
    public static void Tv3x(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY) {
      var pixel = sourceImage[srcX, srcY];
      var ap = (sbyte)(1 - ((srcX & 1) << 1));
      targetImage[tgtX + 0, tgtY + 0] = new sPixel(pixel.Red, 0, 0, pixel.Alpha);
      targetImage[tgtX + 1, tgtY + 0] = new sPixel(0, pixel.Green, 0, pixel.Alpha);
      targetImage[tgtX + 2, tgtY + 0] = new sPixel(0, 0, pixel.Blue, pixel.Alpha);
      targetImage[tgtX + 0, tgtY + ap] = new sPixel(pixel.Red, 0, 0, pixel.Alpha);
      targetImage[tgtX + 1, tgtY - ap] = new sPixel(0, pixel.Green, 0, pixel.Alpha);
      targetImage[tgtX + 2, tgtY + ap] = new sPixel(0, 0, pixel.Blue, pixel.Alpha);

    }

  }
}
