#region (c)2008-2026 Hawkynt
/*
 *  cImage
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using Imager.Pipelines;

namespace Classes.ImageManipulators {
  /// <summary>
  /// Factory for GDI+ <see cref="InterpolationMode"/>-based resamplers. Each built entry is a
  /// standard <see cref="BitmapResamplerAdapter"/> whose operation is a thin wrapper over
  /// <see cref="Graphics.DrawImage(Image, Rectangle)"/> with the requested interpolation mode.
  /// Exposed as a system-API baseline for visual comparison against the upstream resamplers.
  ///
  /// OOB modes, canvas colour and centred-grid flag on <see cref="UpstreamPipeline.ResampleFunc"/>
  /// are ignored: GDI+ has no equivalent knobs.
  /// </summary>
  internal static class GdiPlusResampler {

    public static BitmapResamplerAdapter Create(InterpolationMode mode) {
      var description = DescribeMode(mode);
      return new BitmapResamplerAdapter(
        description,
        (src, w, h, _, _, _, _) => Resample(src, w, h, mode)
      );
    }

    /// <summary>
    /// Resamples through GDI+.
    /// <para>
    /// Interpolating near an edge needs samples from beyond it, and GDI+ takes those from the
    /// transparent surround unless told otherwise - which is what stamped a one pixel halo around
    /// every output. <see cref="WrapMode.TileFlipXY"/> mirrors the source at its borders instead,
    /// so the edge is interpolated against real colour.
    /// </para>
    /// <para>
    /// The previous workaround for that halo drew into <c>-1,-1,W+1,H+1</c>. It hid the border by
    /// pushing it outside the bitmap, but in doing so it offset the image by a pixel and stretched
    /// it by one in each direction, so the result was never the requested resampling.
    /// </para>
    /// </summary>
    /// <param name="source">The source bitmap.</param>
    /// <param name="targetWidth">The target width.</param>
    /// <param name="targetHeight">The target height.</param>
    /// <param name="mode">The interpolation mode.</param>
    /// <returns>The resampled bitmap.</returns>
    private static Bitmap Resample(Bitmap source, int targetWidth, int targetHeight, InterpolationMode mode) {
      var result = new Bitmap(targetWidth, targetHeight, PixelFormat.Format32bppArgb);
      result.SetResolution(source.HorizontalResolution, source.VerticalResolution);

      using (var graphics = Graphics.FromImage(result)) {
        // SourceCopy: write the resampled pixels as they are instead of blending them over the
        // transparent bitmap we just allocated
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = mode;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using (var attributes = new ImageAttributes()) {
          attributes.SetWrapMode(WrapMode.TileFlipXY);
          graphics.DrawImage(
            source,
            new Rectangle(0, 0, targetWidth, targetHeight),
            0, 0, source.Width, source.Height,
            GraphicsUnit.Pixel,
            attributes
          );
        }
      }

      return result;
    }

    private static string DescribeMode(InterpolationMode mode) {
      switch (mode) {
        case InterpolationMode.NearestNeighbor:
          return "Nearest neighbor interpolation using the Microsoft GDI+ API.";
        case InterpolationMode.Bilinear:
          return "Bilinear interpolation using the Microsoft GDI+ API. No prefiltering is done. This mode is not suitable for shrinking an image below 50 percent of its original size.";
        case InterpolationMode.Bicubic:
          return "Bicubic interpolation using the Microsoft GDI+ API. No prefiltering is done. This mode is not suitable for shrinking an image below 25 percent of its original size.";
        case InterpolationMode.HighQualityBilinear:
          return "Bilinear interpolation using the Microsoft GDI+ API. Prefiltering is performed to ensure high-quality shrinking.";
        case InterpolationMode.HighQualityBicubic:
          return "Bicubic interpolation using the Microsoft GDI+ API. Prefiltering is performed to ensure high-quality shrinking.";
        default:
          return "GDI+ " + mode + " interpolation.";
      }
    }
  }
}
