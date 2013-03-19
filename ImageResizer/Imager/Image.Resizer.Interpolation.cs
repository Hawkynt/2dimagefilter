using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Imager {
  public partial class cImage {
    /// <summary>
    /// Stores all available parameterless pixel scalers.
    /// </summary>
    internal static readonly InterpolationMode[] INTERPOLATORS = new[] {
      InterpolationMode.NearestNeighbor,
      InterpolationMode.Bilinear,
      InterpolationMode.Bicubic,
    };

    /// <summary>
    /// Applies the GDI+ pixel scaler.
    /// </summary>
    /// <param name="type">The type of scaler to use.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="filterRegion">The filter region, if any.</param>
    /// <returns>
    /// The rescaled image.
    /// </returns>
    public cImage ApplyScaler(InterpolationMode type, int width, int height, Rectangle? filterRegion = null) {
      if (!((IList<InterpolationMode>)INTERPOLATORS).Contains(type))
        throw new NotSupportedException(string.Format("Interpolation mode '{0}' not supported.", type));

      var startX = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Left);
      var startY = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Top);

      var endX = filterRegion == null ? this.Width : Math.Min(this.Width, filterRegion.Value.Right);
      var endY = filterRegion == null ? this.Height : Math.Min(this.Height, filterRegion.Value.Bottom);

      // run through scaler
      var bitmap = new Bitmap(width, height);
      using (var graphics = Graphics.FromImage(bitmap)) {

        //set the resize quality modes to high quality                
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = type;
        graphics.SmoothingMode = SmoothingMode.HighQuality;

        //draw the image into the target bitmap                
        //graphics.DrawImage(source, 0, 0, result.Width, result.Height);

        // FIXME: this is a hack to prevent the microsoft bug from creating a white pixel on top and left border (see http://forums.asp.net/t/1031961.aspx/1)
        graphics.DrawImage(filterRegion == null ? this.ToBitmap() : this.ToBitmap(startX, startY, endX - startX, endY - startY), -1, -1, bitmap.Width + 1, bitmap.Height + 1);
      }
      var result = FromBitmap(bitmap);
      result.HorizontalOutOfBoundsMode = this.HorizontalOutOfBoundsMode;
      result.VerticalOutOfBoundsMode = this.VerticalOutOfBoundsMode;
      return (result);

    }

  }
}
