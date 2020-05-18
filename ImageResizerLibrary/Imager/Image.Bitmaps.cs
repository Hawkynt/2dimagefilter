#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

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
using System.Collections.Concurrent;
using System.Drawing;
#if NET45
using System.Runtime.CompilerServices;
#endif
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imager {
  partial class cImage {
    /// <summary>
    /// Copies 32-bit blocks from source to target.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="sourceOffset">The source offset.</param>
    /// <param name="target">The target.</param>
    /// <param name="targetOffset">The target offset.</param>
    /// <param name="count">The count.</param>
#if NET45
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static unsafe void _CopyBlock(int* source, int sourceOffset, int* target, int targetOffset, int count) {
      source += sourceOffset;
      target += targetOffset;

      // copy 64-bit as long as possible
      while (count > 1) {
        *(long*) target = *(long*) source;
        source += 2;
        target += 2;
        count -= 2;
      }

      // copy remaining 32-bit 
      if (count > 0)
        *target = *source;
    }

    private static unsafe void _CopyPixels(int x, int y, int width, int height, sPixel[] sourceData, int sourceStride, int sourceWidth, int sourceHeight, IntPtr targetData, int targetStride, int targetWidth, int targetHeight) {
      fixed (sPixel* source = sourceData)
        _CopyPixels(x, y, width, height, (int*) source, sourceWidth, (int*) targetData.ToPointer(), targetWidth, sourceStride, targetStride >> 2);
    }

    private static unsafe void _CopyPixels(int x, int y, int width, int height, IntPtr sourceData, int sourceStride, int sourceWidth, int sourceHeight, sPixel[] targetData, int targetStride, int targetWidth, int targetHeight) {
      fixed (sPixel* target = targetData)
        _CopyPixels(x, y, width, height, (int*) sourceData.ToPointer(), sourceWidth, (int*) target, targetWidth, sourceStride >> 2, targetStride);
    }

#if NET45
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static unsafe void _CopyPixels(int x, int y, int width, int height, int* source, int sourceWidth, int* target, int targetWidth, int sourceStrideNormalized, int targetStrideNormalized) {
      if (x == 0 && targetWidth == sourceWidth && sourceStrideNormalized == targetStrideNormalized) {
        // We can copy pixel data directly
        Parallel.ForEach(
          source: Partitioner.Create(y, y + height),
          localInit: () => 0,
          body:
          (range, _, threadStorage) => {
            var minY = range.Item1;
            var maxY = range.Item2;

            _CopyBlock(
              source: source,
              sourceOffset: (minY - y) * sourceStrideNormalized,
              target: target,
              targetOffset: minY * targetStrideNormalized,
              count: (maxY - minY) * targetStrideNormalized
            );

            return threadStorage;
          },
          localFinally: _ => { }
        );
      } else {
        // Unfortunately we should make some extra effort to copy the data
        Parallel.ForEach(
          source: Partitioner.Create(y, y + height),
          localInit: () => 0,
          body:
          (range, _, threadStorage) => {
            var minY = range.Item1;
            var maxY = range.Item2;


            for (var yLine = minY; yLine < maxY; yLine++) {
              _CopyBlock(
                source: source,
                sourceOffset: (yLine - y) * sourceStrideNormalized + x,
                target: target,
                targetOffset: targetStrideNormalized,
                count: width
              );
            }

            return threadStorage;
          },
          localFinally: _ => { }
        );
      }
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
      using (var data = result.LockForWrite()) {
        var bitmapData = data.BitmapData;
        _CopyPixels(sx, sy, width, height, this._imageData, this._width, this._width, this._height, bitmapData.Scan0, bitmapData.Stride, bitmapData.Width, bitmapData.Height);
      }

      return result;
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
        return null;

      var result = new cImage(bitmap.Width, bitmap.Height);

      using (var data = bitmap.LockForRead()) {
        var bitmapData = data.BitmapData;
        _CopyPixels(0, 0, result._width, result._height, bitmapData.Scan0, bitmapData.Stride, bitmapData.Width, bitmapData.Height, result._imageData, result._width, result._width, result._height);
      }

      return result;
    }


#if NET45
    /// <summary>
    /// Converts this image to a <see cref="BitmapSource"/> instance.
    /// </summary>
    /// <param name="sx">The start x.</param>
    /// <param name="sy">The start y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>
    /// The <see cref="BitmapSource"/> instance
    /// </returns>
    public BitmapSource ToBitmapSource(int sx, int sy, int width, int height) {
      var result = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
      using (var data = result.LockData())
        _CopyPixels(sx, sy, width, height, this._imageData, this._width, this._width, this._height, data.Bitmap.BackBuffer, data.Bitmap.BackBufferStride, data.Bitmap.PixelWidth, data.Bitmap.PixelHeight);

      return result;
    }

    /// <summary>
    /// Converts this image to a <see cref="BitmapSource"/> instance.
    /// </summary>
    /// <returns>The <see cref="BitmapSource"/> instance</returns>
    public BitmapSource ToBitmapSource() => this.ToBitmapSource(0, 0, this._width, this._height);

    // NOTE: Bitmap objects does not support parallel read-outs blame Microsoft
    /// <summary>
    /// Initializes a new instance of the <see cref="cImage"/> class from a <see cref="BitmapSource"/> instance.
    /// </summary>
    /// <param name="bitmapSource">The bitmap.</param>
    public static cImage FromBitmapSource(BitmapSource bitmapSource) {
      if (bitmapSource == null)
        return null;

      var result = new cImage(bitmapSource.PixelWidth, bitmapSource.PixelHeight);

      var finalSource = bitmapSource;

      if (bitmapSource.Format != PixelFormats.Bgra32) {
        var formatedBitmapSource = new FormatConvertedBitmap();

        formatedBitmapSource.BeginInit();
        formatedBitmapSource.Source = bitmapSource;
        formatedBitmapSource.DestinationFormat = PixelFormats.Bgra32;
        formatedBitmapSource.EndInit();

        finalSource = formatedBitmapSource;
      }

      var writeableBitmap = new WriteableBitmap(finalSource);

      using (var data = writeableBitmap.LockData())
        _CopyPixels(0, 0, result._width, result._height, data.Bitmap.BackBuffer, data.Bitmap.BackBufferStride, data.Bitmap.PixelWidth, data.Bitmap.PixelHeight, result._imageData, result._width, result._width, result._height);

      return result;
    }

#endif

  }
}
