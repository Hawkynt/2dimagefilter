﻿#region (c)2008-2015 Hawkynt
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
#if NETFX_45
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
#if NETFX_45
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
          rect: new Rectangle(0, 0, width, height),
          flags: ImageLockMode.WriteOnly,
          format: System.Drawing.Imaging.PixelFormat.Format32bppArgb
        );

        var sourceImageData = this._imageData;
        var sourceStrideInDWords = this._width;
        var targetStrideInBytes = bitmapData.Stride;
        var targetImageData = bitmapData.Scan0;

        if (sx == 0 && width == this._width && (sourceStrideInDWords << 2) == targetStrideInBytes) {

          // special case, copy whole lines of same strides
          Parallel.ForEach(
            source: Partitioner.Create(sy, sy + height),
            localInit: () => 0,
            body:
              (range, _, threadStorage) => {
                var minY = range.Item1;
                var maxY = range.Item2;
                unsafe {
                  // copy all lines in one block
                  fixed (sPixel* sourceFix = sourceImageData)
                    _CopyBlock(
                      source: (int*)sourceFix,
                      sourceOffset: minY * sourceStrideInDWords,
                      target: (int*)targetImageData.ToPointer(),
                      targetOffset: (minY - sy) * (targetStrideInBytes >> 2),
                      count: (maxY - minY) * sourceStrideInDWords
                    );
                }
                return threadStorage;
              },
            localFinally: _ => { }
          );
        } else {
          Parallel.ForEach(
            source: Partitioner.Create(sy, sy + height),
            localInit: () => 0,
            body:
              (range, _, threadStorage) => {
                var minY = range.Item1;
                var maxY = range.Item2;
                unsafe {
                  fixed (sPixel* sourceFix = sourceImageData) {
                    var sourceOffset = minY * sourceStrideInDWords + sx;
                    var targetOffset = (byte*)targetImageData.ToPointer() + minY * targetStrideInBytes;
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
            localFinally: _ => { }
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

      BitmapData bitmapData = null;
      try {
        bitmapData = bitmap.LockBits(
          rect: new Rectangle(0, 0, width, height),
          flags: ImageLockMode.ReadOnly,
          format: System.Drawing.Imaging.PixelFormat.Format32bppArgb
          );

        var targetStrideInDWords = width;
        var sourceStride = bitmapData.Stride;
        var sourceImageData = bitmapData.Scan0;

        if (targetStrideInDWords << 2 == sourceStride) {

          // special case...source and target stride are identical -> copy whole blocks of data
          Parallel.ForEach(
            source: Partitioner.Create(0, height),
            localInit: () => 0,
            body:
              (range, _, threadStorage) => {
                var minY = range.Item1;
                var maxY = range.Item2;
                unsafe {
                  // copy all lines in one block
                  fixed (sPixel* sourceFix = result._imageData)
                    _CopyBlock(
                      source: (int*)sourceImageData.ToPointer(),
                      sourceOffset: minY * (sourceStride >> 2),
                      target: (int*)sourceFix,
                      targetOffset: minY * targetStrideInDWords,
                      count: (maxY - minY) * targetStrideInDWords
                      );
                }
                return threadStorage;
              },
            localFinally: _ => { }
            );
        } else {

          // fall back to line by line copy
          var intFillX = sourceStride - width * 4;
          unsafe {
            fixed (sPixel* target = result._imageData) {
              var ptrOffset = (byte*)sourceImageData.ToPointer();
              for (var y = 0; y < height; y++) {
                _CopyBlock((int*)ptrOffset, 0, (int*)target, y * width, width);
                ptrOffset += width << 2;
                ptrOffset += intFillX;
              }
            }
          }
        }
      } finally {
        if (bitmapData != null)
          bitmap.UnlockBits(bitmapData);
      }
      return (result);
    }

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

      try {
        result.Lock();

        var sourceImageData = this._imageData;
        var sourceStrideInDWords = this._width;
        var targetStrideInBytes = result.BackBufferStride;
        var targetImageData = result.BackBuffer;

        if (sx == 0 && width == this._width && (sourceStrideInDWords << 2) == targetStrideInBytes) {

          // special case, copy whole lines of same strides
          Parallel.ForEach(
            source: Partitioner.Create(sy, sy + height),
            localInit: () => 0,
            body:
              (range, _, threadStorage) => {
                var minY = range.Item1;
                var maxY = range.Item2;
                unsafe {
                  // copy all lines in one block
                  fixed (sPixel* sourceFix = sourceImageData)
                    _CopyBlock(
                      source: (int*)sourceFix,
                      sourceOffset: minY * sourceStrideInDWords,
                      target: (int*)targetImageData.ToPointer(),
                      targetOffset: (minY - sy) * (targetStrideInBytes >> 2),
                      count: (maxY - minY) * sourceStrideInDWords
                    );
                }
                return threadStorage;
              },
            localFinally: _ => { }
          );
        } else {
          Parallel.ForEach(
            source: Partitioner.Create(sy, sy + height),
            localInit: () => 0,
            body:
              (range, _, threadStorage) => {
                var minY = range.Item1;
                var maxY = range.Item2;
                unsafe {
                  fixed (sPixel* sourceFix = sourceImageData) {
                    var sourceOffset = minY * sourceStrideInDWords + sx;
                    var targetOffset = (byte*)targetImageData.ToPointer() + minY * targetStrideInBytes;
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
            localFinally: _ => { }
          );
        }
      } finally {
        result.Unlock();
      }
      return (result);
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
        return (null);

      var result = new cImage(bitmapSource.PixelWidth, bitmapSource.PixelHeight);

      var height = result._height;
      var width = result._width;

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

      try {
        writeableBitmap.Lock();

        var targetStrideInDWords = width;
        var sourceStride = writeableBitmap.BackBufferStride;
        var sourceImageData = writeableBitmap.BackBuffer;

        if (targetStrideInDWords << 2 == sourceStride) {

          // special case...source and target stride are identical -> copy whole blocks of data
          Parallel.ForEach(
            source: Partitioner.Create(0, height),
            localInit: () => 0,
            body:
              (range, _, threadStorage) => {
                var minY = range.Item1;
                var maxY = range.Item2;

                unsafe {
                  // copy all lines in one block
                  fixed (sPixel* sourceFix = result._imageData)
                    _CopyBlock(
                      source: (int*)sourceImageData.ToPointer(),
                      sourceOffset: minY * (sourceStride >> 2),
                      target: (int*)sourceFix,
                      targetOffset: minY * targetStrideInDWords,
                      count: (maxY - minY) * targetStrideInDWords
                      );
                }
                return threadStorage;
              },
            localFinally: _ => { }
            );
        } else {
          // fall back to line by line copy
          var intFillX = sourceStride - sourceStride * 4;
          unsafe {
            fixed (sPixel* target = result._imageData) {
              var ptrOffset = (byte*)sourceImageData.ToPointer();
              for (var y = 0; y < height; y++) {
                _CopyBlock((int*)ptrOffset, 0, (int*)target, y * width, width);
                ptrOffset += width << 2;
                ptrOffset += intFillX;
              }
            }
          }
        }
      } finally {
        writeableBitmap.Unlock();
      }
      return (result);
    }
  }
}