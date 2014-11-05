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
    [EnumDisplayName("MAME TV 2x")]
    [Description("MAME's interlace filter, tries to emulate a TV-CRT screen.")]
    MameTv,
    [EnumDisplayName("MAME TV 3x")]
    [Description("MAME's interlace filter, tries to emulate a TV-CRT screen.")]
    MameTv3,
    [EnumDisplayName("MAME RGB 2x")]
    [Description("MAME's RGB filter, tries to emulate a LCD-Screen.")]
    MameRgb,
    [EnumDisplayName("MAME RGB 3x")]
    [Description("MAME's RGB filter, tries to emulate a LCD-Screen.")]
    MameRgb3,
    [EnumDisplayName("Hawkynt TV 2x")]
    [Description("Hawkynt's TV effect, uses no more than 256 shades of red, green, blue and grey (=1024 colors) to display images.")]
    HawkyntTv,
    [EnumDisplayName("Hawkynt TV 3x")]
    [Description("Hawkynt's TV effect, uses no more than 256 shades of red, green and blue (=768 colors) to display images.")]
    HawkyntTv3,
    #endregion

    #region VBA special
    [EnumDisplayName("Bilinear Plus Original")]
    [Description("VBA's bilinear plus filter, in the original mode of operation.")]
    BilinearPlusOriginal,
    [EnumDisplayName("Bilinear Plus")]
    [Description("VBA's bilinear plus filter.")]
    BilinearPlus,
    #endregion

    #region eagle group
    [EnumDisplayName("Eagle 2x")]
    [Description("The original eagle filter.")]
    Eagle,
    [EnumDisplayName("Eagle 3x")]
    [Description("The eagle filter for using in 3-times enlargement by Hawkynt.")]
    Eagle3,
    [EnumDisplayName("Eagle 3xB")]
    [Description("The eagle filter for using in 3-times enlargement by Hawkynt (alternate version).")]
    Eagle3B,
    [EnumDisplayName("SuperEagle")]
    [Description("SNES9x's SuperEagle filter from Derek Liauw Kie Fa aka Kreed.")]
    SuperEagle,
    #endregion

    #region sai group
    [EnumDisplayName("SaI 2x")]
    [Description("SNES9x's 2xSaI filter from Derek Liauw Kie Fa aka Kreed.")]
    SaI,
    [EnumDisplayName("Super SaI")]
    [Description("SNES9x's Super2xSaI filter from Derek Liauw Kie Fa aka Kreed.")]
    SuperSaI,
    #endregion

    #region scale nx group
    [EnumDisplayName("AdvInterp 2x")]
    [Description("MAME's double-scaling from Andrea Mazzoleni which does use interpolation.")]
    AdvInterp2,
    [EnumDisplayName("AdvInterp 3x")]
    [Description("MAME's tripple-scaling from Andrea Mazzoleni which does use interpolation.")]
    AdvInterp3,
    [EnumDisplayName("Scale 2x")]
    [Description("MAME's double-scaling from Andrea Mazzoleni which does not use interpolation.")]
    Scale2,
    [EnumDisplayName("Scale 3x")]
    [Description("MAME's tripple-scaling from Andrea Mazzoleni which does not use interpolation.")]
    Scale3,
    #endregion

    #region epx group
    [EnumDisplayName("EPXB")]
    [Description("SNES9x-ReRecording's EPX-B scaler.")]
    EpxB,
    [EnumDisplayName("EPXC")]
    [Description("SNES9x-ReRecording's EPX-C scaler.")]
    EpxC,
    [EnumDisplayName("EPX3")]
    [Description("SNES9x-ReRecording's EPX-3 scaler.")]
    Epx3,
    #endregion

    #region reverse AA
    [EnumDisplayName("Reverse AA")]
    [Description("Hyllian's OpenGL reverse anti-alias filter.")]
    ReverseAntiAlias,
    #endregion

    #region FNES
    [EnumDisplayName("DES")]
    [Description("FNES' DES filter.")]
    DES,
    [EnumDisplayName("DES II")]
    [Description("FNES' DES2 filter.")]
    DES2,
    [EnumDisplayName("2xSCL")]
    [Description("FNES' 2xSCL filter.")]
    Normal2xScl,
    [EnumDisplayName("Super 2xSCL")]
    [Description("FNES' Super 2xSCL filter.")]
    Super2xScl,
    [EnumDisplayName("Ultra 2xSCL")]
    [Description("FNES' Ultra 2xSCL filter.")]
    Ultra2xScl,
    #endregion
  }
}
