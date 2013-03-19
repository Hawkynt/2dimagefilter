using System.ComponentModel;

namespace Imager.Interface {
  public enum NqScalerType {

    #region hq group
    [Description("HQ 2x")]
    Hq2,
    [Description("HQ 2x3")]
    Hq2X3,
    [Description("HQ 2x4")]
    Hq2X4,
    [Description("HQ 3x")]
    Hq3,
    [Description("HQ 4x")]
    Hq4,
    #endregion

    #region lq group
    [Description("LQ 2x")]
    Lq2,
    [Description("LQ 2x3")]
    Lq2X3,
    [Description("LQ 2x4")]
    Lq2X4,
    [Description("LQ 3x")]
    Lq3,
    [Description("LQ 4x")]
    Lq4,
    #endregion
  }
}
