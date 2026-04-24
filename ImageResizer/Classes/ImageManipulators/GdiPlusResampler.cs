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

using Imager.Pipelines;

namespace Classes.ImageManipulators {
  /// <summary>
  /// Factory for GDI+ <see cref="InterpolationMode"/>-based resamplers. Each built entry is a
  /// standard <see cref="BitmapResamplerAdapter"/> whose operation is a thin wrapper over
  /// <see cref="Graphics.DrawImage(Image, Rectangle)"/> with the requested interpolation mode.
  /// Exposed as a system-API baseline for visual comparison against the upstream resamplers.
  ///
  /// OOB modes, canvas colour and centred-grid flag on <see cref="UpstreamPipeline.ResampleFunc"/>
  /// are ignored: GDI+ has no equivalent knobs. The <c>-1,-1,W+1,H+1</c> draw rectangle works
  /// around a long-standing GDI+ bug that otherwise paints a white pixel on the top/left edge
  /// (see http://forums.asp.net/t/1031961.aspx/1). This matches the behaviour of the retired
  /// local <c>Interpolator</c> / <c>cImage.ApplyScaler(InterpolationMode, …)</c> path exactly.
  /// </summary>
  internal static class GdiPlusResampler {

    public static BitmapResamplerAdapter Create(InterpolationMode mode) {
      var description = DescribeMode(mode);
      return new BitmapResamplerAdapter(
        description,
        (src, w, h, _, _, _, _) => Resample(src, w, h, mode)
      );
    }

    private static Bitmap Resample(Bitmap source, int targetWidth, int targetHeight, InterpolationMode mode) {
      var result = new Bitmap(targetWidth, targetHeight);
      using (var graphics = Graphics.FromImage(result)) {
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = mode;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        // -1/-1 + size+1 draw rectangle avoids the GDI+ white-edge pixel bug.
        graphics.DrawImage(source, -1, -1, targetWidth + 1, targetHeight + 1);
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
