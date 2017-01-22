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

#if NETFX_45
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

    /// <summary>
    /// Copies 32-bit blocks from source to target.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="sourceOffset">The source offset.</param>
    /// <param name="target">The target.</param>
    /// <param name="targetOffset">The target offset.</param>
    /// <param name="count">The count.</param>
    private static unsafe void _CopyBlock(int* source, int sourceOffset, int* target, int targetOffset, int count) {
      source += sourceOffset;
      target += targetOffset;

      // copy 64-bit as long as possible
      while (count > 1) {
        *((long*)target) = *((long*)source);
        source += 2;
        target += 2;
        count -= 2;
      }

      // copy remaining 32-bit 
      if (count > 0)
        *(target) = *(source);
    }

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
      BitmapData bitmapData = null;
      try {
        bitmapData = result.LockBits(
          new Rectangle(0, 0, width, height),
          ImageLockMode.WriteOnly,
          PixelFormat.Format32bppArgb
        );

        var sourceImageData = this._imageData;
        var sourceStrideInDWords = this._width;
        var targetStrideInBytes = bitmapData.Stride;
        if (sx == 0 && width == this._width && (sourceStrideInDWords << 2) == targetStrideInBytes) {

          // special case, copy whole lines of same strides
          Parallel.ForEach(
            Partitioner.Create(sy, sy + height),
            () => 0,
            (range, _, threadStorage) => {
              var minY = range.Item1;
              var maxY = range.Item2;
              unsafe
              {
                // copy all lines in one block
                fixed (sPixel* sourceFix = sourceImageData)
                  _CopyBlock(
                    (int*)sourceFix,
                    minY * sourceStrideInDWords,
                    (int*)bitmapData.Scan0.ToPointer(),
                    (minY - sy) * (targetStrideInBytes >> 2),
                    (maxY - minY) * sourceStrideInDWords
                  );
              }
              return threadStorage;
            },
            _ => { }
          );
        } else {
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
                  var sourceOffset = minY * sourceStrideInDWords + sx;
                  var targetOffset = (byte*)bitmapData.Scan0.ToPointer() + minY * targetStrideInBytes;
                  for (var y = minY; y < maxY; ++y) {

                    // copy the needed pixels of one line and move on to the next
                    _CopyBlock((int*)sourceFix, sourceOffset, (int*)targetOffset, 0, width);
                    sourceOffset += sourceStrideInDWords;
                    targetOffset += targetStrideInBytes;
                  }
                }
              }
              return threadStorage;

            },
            _ => { }
            );
        }
      } finally {
        if (bitmapData != null)
          result.UnlockBits(bitmapData);
      }
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
            _CopyBlock((int*)ptrOffset, 0, (int*)target, y * width, width);
            ptrOffset += width << 2;
            ptrOffset += intFillX;
          }
        }
      }
      bitmap.UnlockBits(bitmapData);

      return (result);
    }
  }
}
