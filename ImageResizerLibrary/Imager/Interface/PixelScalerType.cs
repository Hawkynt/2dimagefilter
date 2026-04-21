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

using Imager.Classes;
using System.ComponentModel;

namespace Imager.Interface {
  public enum PixelScalerType {

    #region scanline effects
    [EnumDisplayName("-50% Scanlines")]
    [Description("A scanline which is 50% darker is inserted every second line.")]
    HorizontalHalfDarkScanlines,
    [EnumDisplayName("+50% Scanlines")]
    [Description("A scanline which is 50% lighter is inserted every second line.")]
    HorizontalHalfLightScanlines,
    [EnumDisplayName("+100% Scanlines")]
    [Description("A scanline which is 100% lighter is inserted every second line.")]
    HorizontalFullLightScanlines,

    [EnumDisplayName("-50% VScanlines")]
    [Description("A scanline which is 50% darker is inserted every second column.")]
    VerticalHalfDarkScanlines,
    [EnumDisplayName("+50% VScanlines")]
    [Description("A scanline which is 50% lighter is inserted every second column.")]
    VerticalHalfLightScanlines,
    [EnumDisplayName("+100% VScanlines")]
    [Description("A scanline which is 100% lighter is inserted every second column.")]
    VerticalFullLightScanlines,
    #endregion

    #region CRT effects
    [EnumDisplayName("Hawkynt TV 3x")]
    [Description("Hawkynt's TV effect, uses no more than 256 shades of red, green and blue (=768 colors) to display images. (Legacy: alternating-column pattern not reproduced by upstream HawkyntTv.)")]
    HawkyntTv3,
    #endregion

  }
}
