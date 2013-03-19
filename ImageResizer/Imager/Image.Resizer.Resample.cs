using System.Drawing;
using Imager.Classes;

namespace Imager {
  public partial class cImage {

    /// <summary>
    /// Applies the pixel scaler for float32 images.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="centeredGrid">if set to <c>true</c> [centered grid].</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(KernelType type, int width, int height, bool centeredGrid, Rectangle? filterRegion = null) {
      var fpImage = FloatImage.FromImage(this, filterRegion);
      var fpResult = fpImage.Resize(width, height, type, centeredGrid);
      var result = fpResult.ToImage();
      return (result);
    }
  }
}
