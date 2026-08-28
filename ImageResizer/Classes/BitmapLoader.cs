#region (c)2008-2026 Hawkynt
/*
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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Classes {
  /// <summary>
  /// Turns a freshly loaded image into the standalone 32bpp ARGB bitmap the pipeline works on.
  /// </summary>
  internal static class BitmapLoader {

    /// <summary>
    /// Copies an image into an independent bitmap without losing anything.
    /// <para>
    /// The copy is a pixel clone rather than a redraw. Drawing an image onto a fresh surface
    /// composites it, and compositing a fully transparent pixel throws its colour away - the
    /// magenta behind a sprite's transparent background came out as black, so the background
    /// colour had to be restored by hand afterwards.
    /// </para>
    /// <para>
    /// Cloning also sidesteps the DPI trap that the redraw needed an explicit pixel rectangle to
    /// avoid: it works in pixels and never consults the resolution metadata.
    /// </para>
    /// </summary>
    /// <para>
    /// The pixels are copied out byte for byte rather than cloned: a clone can share the loader's
    /// storage, which keeps the source file mapped and locked, and releasing the file is the
    /// whole reason this copy exists.
    /// </para>
    /// <param name="image">The loaded image; the caller still owns and disposes it.</param>
    /// <returns>An independent bitmap holding no reference to the loader or its stream.</returns>
    public static Bitmap CopyPreservingTransparency(Image image) {
      if (!(image is Bitmap bitmap)) {
        // metafiles and the like have no pixels to copy; compositing is all there is
        var drawn = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(drawn))
          graphics.DrawImage(image, 0, 0, image.Width, image.Height);

        return drawn;
      }

      var bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
      var copy = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);

      // LockBits converts an indexed or lower-depth source on the way out, palette alpha included
      var read = bitmap.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
      try {
        var write = copy.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        try {
          var row = new byte[bounds.Width * 4];
          for (var y = 0; y < bounds.Height; ++y) {
            Marshal.Copy(read.Scan0 + y * read.Stride, row, 0, row.Length);
            Marshal.Copy(row, 0, write.Scan0 + y * write.Stride, row.Length);
          }
        } finally {
          copy.UnlockBits(write);
        }
      } finally {
        bitmap.UnlockBits(read);
      }

      return copy;
    }
  }
}
