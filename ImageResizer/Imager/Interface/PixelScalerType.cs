using System.ComponentModel;

namespace Imager.Interface {
  public enum PixelScalerType {

    #region scanline effects
    [Description("-50% Scanlines")]
    HorizontalHalfDarkScanlines,
    [Description("+50% Scanlines")]
    HorizontalHalfLightScanlines,
    [Description("+100% Scanlines")]
    HorizontalFullLightScanlines,

    [Description("-50% VScanlines")]
    VerticalHalfDarkScanlines,
    [Description("+50% VScanlines")]
    VerticalHalfLightScanlines,
    [Description("+100% VScanlines")]
    VerticalFullLightScanlines,
    #endregion

    #region CRT effects
    [Description("MAME TV 2x")]
    MameTv,
    [Description("MAME TV 3x")]
    MameTv3,
    [Description("MAME RGB 2x")]
    MameRgb,
    [Description("MAME RGB 3x")]
    MameRgb3,
    [Description("Hawkynt TV 2x")]
    HawkyntTv,
    [Description("Hawkynt TV 3x")]
    HawkyntTv3,
    #endregion

    #region VBA special
    [Description("Bilinear Plus Original")]
    BilinearPlusOriginal,
    [Description("Bilinear Plus")]
    BilinearPlus,
    #endregion

    #region eagle group
    [Description("Eagle 2x")]
    Eagle,
    [Description("Eagle 3x")]
    Eagle3,
    [Description("Eagle 3xB")]
    Eagle3B,
    [Description("SuperEagle")]
    SuperEagle,
    #endregion

    #region sai group
    [Description("SaI 2x")]
    SaI,
    [Description("Super SaI")]
    SuperSaI,
    #endregion

    #region scale nx group
    [Description("AdvInterp 2x")]
    AdvInterp2,
    [Description("AdvInterp 3x")]
    AdvInterp3,
    [Description("Scale 2x")]
    Scale2,
    [Description("Scale 3x")]
    Scale3,
    #endregion

    #region epx group
    [Description("EPXB")]
    EpxB,
    [Description("EPXC")]
    EpxC,
    [Description("EPX3")]
    Epx3,
    #endregion
  }
}
