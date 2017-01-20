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

using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Imager {
  partial class cImage {
    /// <summary>
    /// Converts this image to a <see cref="Bitmap"/> instance.
    /// </summary>
    /// <param name="sx">The start x.</param>
    /// <param name="sy">The start y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>
    /// The <see cref="Bitmap"/> instance
    /// </returns>
    public Bitmap ToBitmap(int sx, int sy, int width, int height) {
      var result = new Bitmap(width, height);
      var bitmapData = result.LockBits(
        new Rectangle(0, 0, width, height),
        ImageLockMode.WriteOnly,
        PixelFormat.Format32bppArgb
      );

      var targetStride = bitmapData.Stride;
      var fillBytes = targetStride - bitmapData.Width * 4;
      var sourceStride = this._width;
      var sourceImageData = this._imageData;

      Parallel.ForEach(
        Partitioner.Create(sy, sy + height),
        () => 0,
        (range, _, threadStorage) => {
          var minY = range.Item1;
          var maxY = range.Item2;
          unsafe
          {
            fixed (sPixel* sourceFix = sourceImageData)
            {
              var offset = (byte*)bitmapData.Scan0.ToPointer() + minY * targetStride;
              for (var y = minY; y < maxY; ++y) {
                var sourceOffset = sourceStride * y + sx;
                for (var x = sx; x < sx + width; x++) {
                  var pixel = sourceFix[sourceOffset];
                  sourceOffset++;

                  *(offset + 3) = pixel.Alpha;
                  *(offset + 2) = pixel.Red;
                  *(offset + 1) = pixel.Green;
                  *(offset + 0) = pixel.Blue;
                  offset += 4;
                }
                offset += fillBytes;
              }
            }
          }
          return threadStorage;

        },
        _ => { }
      );
      result.UnlockBits(bitmapData);
      return (result);
    }

    /// <summary>
    /// Converts this image to a <see cref="Bitmap"/> instance.
    /// </summary>
    /// <returns>The <see cref="Bitmap"/> instance</returns>
    public Bitmap ToBitmap() => this.ToBitmap(0, 0, this._width, this._height);

    // NOTE: Bitmap objects does not support parallel read-outs blame Microsoft
    /// <summary>
    /// Initializes a new instance of the <see cref="cImage"/> class from a <see cref="Bitmap"/> instance.
    /// </summary>
    /// <param name="bitmap">The bitmap.</param>
    public static cImage FromBitmap(Bitmap bitmap) {
      if (bitmap == null)
        return (null);

      var result = new cImage(bitmap.Width, bitmap.Height);

      var height = result._height;
      var width = result._width;

      var bitmapData = bitmap.LockBits(
        new Rectangle(0, 0, width, height),
        ImageLockMode.ReadOnly,
        PixelFormat.Format32bppArgb
      );
      var intFillX = bitmapData.Stride - bitmapData.Width * 4;
      unsafe
      {
        fixed (sPixel* target = result._imageData)
        {
          var ptrOffset = (byte*)bitmapData.Scan0.ToPointer();
          for (var y = 0; y < height; y++) {
            var offset = y * width;
            for (var x = 0; x < width; x++) {
              target[offset] = new sPixel(*(ptrOffset + 2), *(ptrOffset + 1), *(ptrOffset + 0), *(ptrOffset + 3));
              offset++;
              ptrOffset += 4;
            }
            ptrOffset += intFillX;
          }
        }
      }
      bitmap.UnlockBits(bitmapData);

      return (result);
    }
  }
}
