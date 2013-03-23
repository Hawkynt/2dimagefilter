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
using Imager.Classes;

namespace Imager.Interface {
  public enum PixelScalerType {

    #region scanline effects
    [EnumDisplayName("-50% Scanlines")]
    HorizontalHalfDarkScanlines,
    [EnumDisplayName("+50% Scanlines")]
    HorizontalHalfLightScanlines,
    [EnumDisplayName("+100% Scanlines")]
    HorizontalFullLightScanlines,

    [EnumDisplayName("-50% VScanlines")]
    VerticalHalfDarkScanlines,
    [EnumDisplayName("+50% VScanlines")]
    VerticalHalfLightScanlines,
    [EnumDisplayName("+100% VScanlines")]
    VerticalFullLightScanlines,
    #endregion

    #region CRT effects
    [EnumDisplayName("MAME TV 2x")]
    MameTv,
    [EnumDisplayName("MAME TV 3x")]
    MameTv3,
    [EnumDisplayName("MAME RGB 2x")]
    MameRgb,
    [EnumDisplayName("MAME RGB 3x")]
    MameRgb3,
    [EnumDisplayName("Hawkynt TV 2x")]
    HawkyntTv,
    [EnumDisplayName("Hawkynt TV 3x")]
    HawkyntTv3,
    #endregion

    #region VBA special
    [EnumDisplayName("Bilinear Plus Original")]
    BilinearPlusOriginal,
    [EnumDisplayName("Bilinear Plus")]
    BilinearPlus,
    #endregion

    #region eagle group
    [EnumDisplayName("Eagle 2x")]
    Eagle,
    [EnumDisplayName("Eagle 3x")]
    Eagle3,
    [EnumDisplayName("Eagle 3xB")]
    Eagle3B,
    [EnumDisplayName("SuperEagle")]
    SuperEagle,
    #endregion

    #region sai group
    [EnumDisplayName("SaI 2x")]
    SaI,
    [EnumDisplayName("Super SaI")]
    SuperSaI,
    #endregion

    #region scale nx group
    [EnumDisplayName("AdvInterp 2x")]
    AdvInterp2,
    [EnumDisplayName("AdvInterp 3x")]
    AdvInterp3,
    [EnumDisplayName("Scale 2x")]
    Scale2,
    [EnumDisplayName("Scale 3x")]
    Scale3,
    #endregion

    #region epx group
    [EnumDisplayName("EPXB")]
    EpxB,
    [EnumDisplayName("EPXC")]
    EpxC,
    [EnumDisplayName("EPX3")]
    Epx3,
    #endregion
  }
}
