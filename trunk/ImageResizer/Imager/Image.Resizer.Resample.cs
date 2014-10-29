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

    /// <summary>
    /// Applies the pixel scaler for float32 images.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="radius">The radius.</param>
    /// <param name="centeredGrid">if set to <c>true</c> [centered grid].</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(WindowType type, int width, int height, float radius, bool centeredGrid, Rectangle? filterRegion = null) {
      var fpImage = FloatImage.FromImage(this, filterRegion);
      var fpResult = fpImage.Resize(width, height, type, radius, centeredGrid);
      var result = fpResult.ToImage();
      return (result);
    }
  }
}
