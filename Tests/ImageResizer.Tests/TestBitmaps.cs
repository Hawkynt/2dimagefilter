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

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageResizer.Tests {
  /// <summary>
  /// Throwaway bitmaps and files for tests. Everything lands under a per-run temp directory that
  /// <see cref="TemporaryDirectory"/> removes again, so a failing test cannot leak into the next.
  /// </summary>
  internal static class TestBitmaps {

    /// <summary>
    /// Creates a bitmap with a deterministic, non-uniform pattern - a flat fill would let a
    /// broken scaler look correct.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>The bitmap; the caller owns it.</returns>
    public static Bitmap Create(int width = 16, int height = 16) {
      var result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
      for (var y = 0; y < height; ++y)
      for (var x = 0; x < width; ++x)
        result.SetPixel(x, y, Color.FromArgb(255, x * 255 / Math.Max(1, width - 1), y * 255 / Math.Max(1, height - 1), (x ^ y) & 0xff));

      return result;
    }

    /// <summary>
    /// Writes a bitmap to disk.
    /// </summary>
    /// <param name="path">The target path; its extension picks the format.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns><paramref name="path"/>.</returns>
    public static string WriteTo(string path, int width = 16, int height = 16) {
      using (var bitmap = Create(width, height))
        bitmap.Save(path, FormatFor(path));

      return path;
    }

    /// <summary>
    /// Maps a file extension to the format <see cref="Image.Save(string, ImageFormat)"/> needs.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The format.</returns>
    public static ImageFormat FormatFor(string path) {
      switch (Path.GetExtension(path)?.ToUpperInvariant()) {
        case ".BMP": return ImageFormat.Bmp;
        case ".GIF": return ImageFormat.Gif;
        case ".JPG":
        case ".JPEG": return ImageFormat.Jpeg;
        case ".TIF":
        case ".TIFF": return ImageFormat.Tiff;
        default: return ImageFormat.Png;
      }
    }

    /// <summary>
    /// Reads back the dimensions of an image file without keeping it open.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The size.</returns>
    public static Size SizeOf(string path) {
      using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
      using (var image = Image.FromStream(stream, false, false))
        return image.Size;
    }

    /// <summary>
    /// Reads back the container format of an image file without keeping it open.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The raw format.</returns>
    public static ImageFormat RawFormatOf(string path) {
      using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
      using (var image = Image.FromStream(stream, false, false))
        return image.RawFormat;
    }
  }

  /// <summary>
  /// A scratch directory that deletes itself on <see cref="Dispose"/>.
  /// </summary>
  internal sealed class TemporaryDirectory : IDisposable {

    public string Path { get; }

    public TemporaryDirectory() {
      this.Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ImageResizer.Tests", Guid.NewGuid().ToString("N"));
      Directory.CreateDirectory(this.Path);
    }

    /// <summary>
    /// Combines a file name with this directory.
    /// </summary>
    /// <param name="fileName">The file name.</param>
    /// <returns>The full path.</returns>
    public string File(string fileName) => System.IO.Path.Combine(this.Path, fileName);

    public void Dispose() {
      try {
        Directory.Delete(this.Path, true);
      } catch (IOException) {
        // a leaked handle must not turn into a test failure
      }
    }
  }
}
