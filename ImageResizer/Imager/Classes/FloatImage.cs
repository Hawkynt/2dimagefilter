#region (c)2008-2013 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010-2013 Hawkynt

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
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Threading.Tasks;

using Imager.Interface;

namespace Imager.Classes {
  public class FloatImage {
    #region fields
    private readonly float[] _redPlane;
    private readonly float[] _greenPlane;
    private readonly float[] _bluePlane;
    private readonly float[] _alphaPlane;

    private readonly OutOfBoundsMode _verticalOutOfBoundsMode;
    private readonly OutOfBoundsMode _horizontalOutOfBoundsMode;
    private readonly int _width;
    private readonly int _height;
    #endregion

    #region props
    public OutOfBoundsMode VerticalOutOfBoundsMode { get { return this._verticalOutOfBoundsMode; } }
    public OutOfBoundsMode HorizontalOutOfBoundsMode { get { return this._horizontalOutOfBoundsMode; } }
    public int Width { get { return this._width; } }
    public int Height { get { return this._height; } }
    #endregion

    #region ctor
    private FloatImage(int width, int height, OutOfBoundsMode horizontalOutOfBoundsMode, OutOfBoundsMode verticalOutOfBoundsMode) {
      this._width = width;
      this._height = height;
      this._horizontalOutOfBoundsMode = horizontalOutOfBoundsMode;
      this._verticalOutOfBoundsMode = verticalOutOfBoundsMode;

      // allocate space
      var totalElements = width * height;
      this._redPlane = new float[totalElements];
      this._greenPlane = new float[totalElements];
      this._bluePlane = new float[totalElements];
      this._alphaPlane = new float[totalElements];
    }
    #endregion

    #region converter
    /// <summary>
    /// Converts a given image into a floating point one.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="filterRegion">The filter region.</param>
    /// <returns></returns>
    public static FloatImage FromImage(cImage image, Rectangle? filterRegion) {
      Contract.Requires(image != null);

      var startX = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Left);
      var startY = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Top);

      var endX = filterRegion == null ? image.Width : Math.Min(image.Width, filterRegion.Value.Right);
      var endY = filterRegion == null ? image.Height : Math.Min(image.Height, filterRegion.Value.Bottom);

      var width = endX - startX;
      var height = endY - startY;

      var result = new FloatImage(width, height, image.HorizontalOutOfBoundsMode, image.VerticalOutOfBoundsMode);

      // copy image data
      Parallel.ForEach(
        Partitioner.Create(startY, endY),
        () => 0,
        (range, _, threadStorage) => {
          var i = (range.Item1 - startY) * width;
          for (var y = range.Item1; y < range.Item2; ++y) {
            for (var x = startX; x < endX; ++x) {
              var color = image[x, y];
              result._redPlane[i] = color.SingleRed;
              result._greenPlane[i] = color.SingleGreen;
              result._bluePlane[i] = color.SingleBlue;
              result._alphaPlane[i] = color.SingleAlpha;
              ++i;
            }
          }
          return (threadStorage);
        },
        _ => { }
      );
      return (result);
    }

    /// <summary>
    /// Converts this floating point image to a normal image.
    /// </summary>
    /// <returns></returns>
    public cImage ToImage() {
      var width = this.Width;
      var height = this.Height;
      var result = new cImage(width, height) {
        HorizontalOutOfBoundsMode = this.HorizontalOutOfBoundsMode,
        VerticalOutOfBoundsMode = this.VerticalOutOfBoundsMode
      };

      // copy image data
      Parallel.ForEach(
        Partitioner.Create(0, height),
        () => 0,
        (range, _, threadStorage) => {
          var i = range.Item1 * width;
          for (var y = range.Item1; y < range.Item2; ++y) {
            for (var x = 0; x < width; ++x) {
              result[x, y] = sPixel.FromFloat(
                this._redPlane[i],
                this._greenPlane[i],
                this._bluePlane[i],
                this._alphaPlane[i]
              );
              ++i;
            }
          }
          return (threadStorage);
        },
        _ => { }
      );

      return (result);
    }
    #endregion

    #region get components
    public float Red(int x, int y) {
      return (_GetValueFromPlane(this._redPlane, x, y, this._width, this._height, this._horizontalOutOfBoundsMode, this._verticalOutOfBoundsMode));
    }

    public float Green(int x, int y) {
      return (_GetValueFromPlane(this._greenPlane, x, y, this._width, this._height, this._horizontalOutOfBoundsMode, this._verticalOutOfBoundsMode));
    }

    public float Blue(int x, int y) {
      return (_GetValueFromPlane(this._bluePlane, x, y, this._width, this._height, this._horizontalOutOfBoundsMode, this._verticalOutOfBoundsMode));
    }

    public float Alpha(int x, int y) {
      return (_GetValueFromPlane(this._alphaPlane, x, y, this._width, this._height, this._horizontalOutOfBoundsMode, this._verticalOutOfBoundsMode));
    }
    #endregion

    #region utils
    private static float _GetValueFromPlane(float[] plane, int x, int y, int width, int height, OutOfBoundsMode horizontalOutOfBoundsMode, OutOfBoundsMode verticalOutOfBoundsMode) {
      x = OutOfBoundsUtils.GetBoundsCheckedCoordinate(x, width, horizontalOutOfBoundsMode);
      y = OutOfBoundsUtils.GetBoundsCheckedCoordinate(y, height, verticalOutOfBoundsMode);
      return (plane[y * width + x]);
    }
    #endregion

    #region magic stuff
    /// <summary>
    /// Resizes the image to the specified dimensions using the given kernel type.
    /// </summary>
    /// <param name="destWidth">Width of the destination.</param>
    /// <param name="destHeight">Height of the destination.</param>
    /// <param name="method">The kernel method.</param>
    /// <param name="centeredGrid">if set to <c>true</c> using a centered grid; otherwise, using top-left aligned.</param>
    /// <returns>The resized image</returns>
    public FloatImage Resize(int destWidth, int destHeight, KernelType method, bool centeredGrid) {
      return (this._Resize(destWidth, destHeight, Kernels.KERNELS[method], centeredGrid));
    }

    /// <summary>
    /// Resizes the image to the specified dimensions using the given kernel type.
    /// </summary>
    /// <param name="destWidth">Width of the destination.</param>
    /// <param name="destHeight">Height of the destination.</param>
    /// <param name="method">The kernel method.</param>
    /// <param name="centeredGrid">if set to <c>true</c> using a centered grid; otherwise, using top-left aligned.</param>
    /// <returns>The resized image</returns>
    public FloatImage Resize(int destWidth, int destHeight, WindowType method, float radius, bool centeredGrid) {
      return (this._Resize(destWidth, destHeight, Windows.WINDOWS[method].WithRadius(radius), centeredGrid));
    }

    /*
     * This region is a C++ -> C# conversion from the original sources of Pascal Getreuer <getreuer@gmail.com>
     */
    /// <summary>
    /// Resizes the image to the specified dimensions using the given kernel type.
    /// </summary>
    /// <param name="destWidth">Width of the destination.</param>
    /// <param name="destHeight">Height of the destination.</param>
    /// <param name="interpolationInfo">The interpolation method info.</param>
    /// <param name="centeredGrid">if set to <c>true</c> using a centered grid; otherwise, using top-left aligned.</param>
    /// <returns>
    /// The resized image
    /// </returns>
    private FloatImage _Resize(int destWidth, int destHeight, Kernels.FixedRadiusKernelInfo interpolationInfo, bool centeredGrid) {
      var xScale = (float)destWidth / this.Width;
      var yScale = (float)destHeight / this.Height;
      var xStep = 1 / xScale;
      var yStep = 1 / yScale;
      float xStart, yStart;
      if (centeredGrid) {
        xStart = (xStep - 1) / 2;
        yStart = (yStep - 1) / 2;
      } else {
        xStart = yStart = 0;
      }
      var result = new FloatImage(destWidth, destHeight, this._horizontalOutOfBoundsMode, this._verticalOutOfBoundsMode);

      // prefilter image if necessary
      if (interpolationInfo.PrefilterAlpha != null && interpolationInfo.PrefilterAlpha.Length > 0)
        this._PrefilterImage(interpolationInfo.PrefilterAlpha, interpolationInfo.PrefilterScale);

      // resample
      this._LinScale2D(
        result,
        xStart,
        xStep,
        yStart,
        yStep,
        interpolationInfo.Kernel,
        interpolationInfo.KernelRadius,
        interpolationInfo.KernelNormalize,
        OutOfBoundsUtils.GetHandlerOrCrash(this.HorizontalOutOfBoundsMode),
        OutOfBoundsUtils.GetHandlerOrCrash(this.VerticalOutOfBoundsMode)
      );
      return (result);
    }

    /// <summary>
    /// Apply a cascade of first-order recursive filter pairs to an image
    /// </summary>
    /// <param name="alpha">array of alpha values</param>
    /// <param name="constantFactor">constant multiplicative factor to apply</param>
    private void _PrefilterImage(float[] alpha, float constantFactor) {
      Contract.Requires(alpha != null);
      var numPixels = this.Width * this.Height;

      /* Square the ConstantFactor for two spatial dimensions */
      constantFactor = constantFactor * constantFactor;

      foreach (var plane in new[] {
        this._redPlane,
        this._greenPlane,
        this._bluePlane,
        this._alphaPlane
      }) {
        for (var x = 0; x < Width; x++)
          foreach (var t in alpha)
            _PrefilterScan(plane, x, this.Width, this.Height, t, this._verticalOutOfBoundsMode);

        for (var y = 0; y < Height; y++)
          foreach (var t in alpha)
            _PrefilterScan(plane, +this.Width * y, 1, this.Width, t, this._horizontalOutOfBoundsMode);

        for (var k = 0; k < numPixels; k++)
          plane[k] *= constantFactor;

      }
    }

    /// <summary>
    /// 1D in-place filtering with a first-order recursive filter pair
    /// Applies the causal recursive filter
    ///     1/(1 - alpha z^-1)
    /// followed by the anti-causal recursive filter
    ///     -alpha/(1 - alpha z).
    /// The coefficient alpha must satisify |alpha| &lt; 1 for stability.
    /// 
    /// With respect to boundary handling, filtering is computed with relative
    /// accuracy Eps = 1e-4 for half- and whole-sample symmetric boundaries and it
    /// is exact for constant extension.  Note, however, that for constant extension
    /// the infinite grid result is not exactly constant beyond the boundaries
    /// (rather it decays to constant).
    /// 
    /// </summary>
    /// <param name="plane">Data pointer to data to be filtered</param>
    /// <param name="offset">The offset.</param>
    /// <param name="stride">stride between successive elements</param>
    /// <param name="n">number of samples</param>
    /// <param name="alpha">filter coefficient</param>
    /// <param name="outOfBoundsMode">the kind of boundary handling to use</param>
    private static void _PrefilterScan(float[] plane, int offset, int stride, int n, float alpha, OutOfBoundsMode outOfBoundsMode) {
      Contract.Requires(plane != null);
      const float eps = 1e-4f;
      float sum, weight;
      int i, iEnd;

      var n0 = (int)Math.Ceiling(Math.Log(eps) / Math.Log(Math.Abs(alpha)));

      if (n0 > n)
        n0 = n;

      switch (outOfBoundsMode) {
        case OutOfBoundsMode.ConstantExtension: {
          sum = plane[offset + 0] / (1 - alpha);
          break;
        }
        case OutOfBoundsMode.WholeSampleSymmetric: {
          sum = plane[offset + 0];
          weight = 1;
          iEnd = n0 * stride;

          for (i = stride; i < iEnd; i += stride) {
            weight *= alpha;
            sum += plane[offset + i] * weight;
          }
          break;
        }
        default: {
          /* BOUNDARY_HSYMMETRIC */
          sum = plane[offset + 0] * (1 + alpha);
          weight = alpha;
          iEnd = n0 * stride;

          for (i = stride; i < iEnd; i += stride) {
            weight *= alpha;
            sum += plane[offset + i] * weight;
          }
          break;
        }
      }

      var last = plane[offset + 0] = sum;
      iEnd = (n - 1) * stride;

      for (i = stride; i < iEnd; i += stride) {
        plane[offset + i] += alpha * last;
        last = plane[offset + i];
      }

      switch (outOfBoundsMode) {
        case OutOfBoundsMode.ConstantExtension: {
          last = plane[offset + iEnd] = (alpha * (-plane[offset + iEnd] + (alpha - 1) * alpha * last))
            / ((alpha - 1) * (alpha * alpha - 1));
          break;
        }
        case OutOfBoundsMode.WholeSampleSymmetric: {
          plane[offset + iEnd] += alpha * last;
          last = plane[offset + iEnd] = (alpha / (alpha * alpha - 1))
            * (plane[offset + iEnd] + alpha * plane[offset + iEnd - stride]);
          break;
        }
        default: {
          plane[offset + iEnd] += alpha * last;
          last = plane[offset + iEnd] *= alpha / (alpha - 1);
          break;
        }
      }

      for (i = iEnd - stride; i >= 0; i -= stride) {
        plane[offset + i] = alpha * (last - plane[offset + i]);
        last = plane[offset + i];
      }
    }

    private struct ScaleScanFilter {
      public float[] Coeff;
      public short[] Pos;
      public int Width;
    }

    /// <summary>
    /// Create scanline interpolation filter to be applied with ScaleScan
    /// This routine creates a scalescanfilter for 1-D interpolation of samples at
    /// the locations
    ///    XStart + n*XStep, n = 0, ..., DestWidth - 1,
    /// where the pixels of the source are logically located at the integers.  Half-
    /// sample even symmetric extension is used to handle the boundaries.
    /// </summary>
    /// <param name="destWidth">width after interpolation</param>
    /// <param name="xstart">leftmost sampling location (in input coordinates)</param>
    /// <param name="xstep">the length between successive samples (in input coordinates)</param>
    /// <param name="srcWidth">width of the input</param>
    /// <param name="kernel">interpolation kernel function to use</param>
    /// <param name="kernelRadius">kernel support radius</param>
    /// <param name="kernelNormalize">if set to <c>true</c> filter rows are normalized to sum to 1</param>
    /// <param name="boundary">boundary handling</param>
    /// <returns></returns>
    /// 
    /// 
    private static ScaleScanFilter _MakeScaleScanFilter(int destWidth, float xstart, float xstep, int srcWidth, Kernels.FixedRadiusKernelMethod kernel, float kernelRadius, bool kernelNormalize, OutOfBoundsUtils.OutOfBoundsHandler boundary) {
      Contract.Requires(kernel != null);

      var kernelWidth = (int)Math.Ceiling(2 * kernelRadius);
      var filterWidth = (srcWidth < kernelWidth) ? srcWidth : kernelWidth;
      var filterCoeff = new float[filterWidth * destWidth];
      var filterPos = new short[destWidth];

      var result = new ScaleScanFilter {
        Coeff = filterCoeff,
        Pos = filterPos,
        Width = filterWidth
      };

      var maxPos = srcWidth - filterWidth;

      var coeffIndex = 0;
      for (var destX = 0; destX < destWidth; destX++) {
        var srcX = xstart + xstep * destX;
        var pos = (int)Math.Ceiling(srcX - kernelRadius);

        if (pos < 0 || maxPos < pos) {
          filterPos[destX] = (short)(pos < 0 ? 0 : pos > maxPos ? maxPos : pos);

          for (var n = 0; n < filterWidth; n++)
            filterCoeff[coeffIndex + n] = 0;

          for (var n = 0; n < kernelWidth; n++)
            filterCoeff[coeffIndex + OutOfBoundsUtils.GetBoundsCheckedCoordinate(pos + n, srcWidth, boundary) - filterPos[destX]]
                += (float)kernel(srcX - (pos + n));
        } else {
          filterPos[destX] = (short)pos;

          for (var n = 0; n < filterWidth; n++)
            filterCoeff[coeffIndex + n] = (float)kernel(srcX - (pos + n));
        }

        if (kernelNormalize)	/* Normalize */ {
          var sum = 0f;

          for (var n = 0; n < filterWidth; n++)
            sum += filterCoeff[coeffIndex + n];

          for (var n = 0; n < filterWidth; n++)
            filterCoeff[coeffIndex + n] /= sum;
        }

        coeffIndex += filterWidth;
      }

      return (result);
    }

    /// <summary>
    /// Scale image with a compact support interpolation kernel
    /// This is a generic linear interpolation routine to scale an image using any
    /// compactly supported interpolation kernel.  The kernel is applied separably
    /// along both dimensions.  Half-sample even symmetric extension is used to
    /// handle the boundaries.
    /// 
    /// The interpolation is computed so that Dest[m + DestWidth*n] is the
    /// interpolation of Src at sampling location
    ///     (XStart + m*XStep, YStart + n*YStep)
    /// for m = 0, ..., DestWidth - 1, n = 0, ..., DestHeight - 1, where the
    /// pixels of Src are located at the integers.
    /// 
    /// The implementation follows the approach taken in ffmpeg's swscale library.
    /// First a "scanline filter" is constructed, a sparse matrix such that
    /// multiplying with a row of the input image produces an interpolated row in
    /// the output image.  Similarly a second matrix is constructed for
    /// interpolating columns.  The interpolation itself is then essentially two
    /// sparse matrix times dense matrix multiplies.
    /// </summary>
    /// <param name="destination">pointer to memory for holding the interpolated image</param>
    /// <param name="xStart">leftmost sampling location (in input coordinates)</param>
    /// <param name="xStep">the length between successive samples (in input coordinates)</param>
    /// <param name="yStart">uppermost sampling location (in input coordinates)</param>
    /// <param name="yStep">the length between successive samples (in input coordinates)</param>
    /// <param name="kernel">interpolation kernel function to use</param>
    /// <param name="kernelRadius">kernel support radius</param>
    /// <param name="kernelNormalize">if set to <c>true</c> filter rows are normalized to sum to 1</param>
    /// <param name="horizontalOutOfBoundsHandler">The horizontal out of bounds handler.</param>
    /// <param name="verticalOutOfBoundsHandler">The vertical out of bounds handler.</param>
    private void _LinScale2D(FloatImage destination, float xStart, float xStep, float yStart, float yStep, Kernels.FixedRadiusKernelMethod kernel, float kernelRadius, bool kernelNormalize, OutOfBoundsUtils.OutOfBoundsHandler horizontalOutOfBoundsHandler, OutOfBoundsUtils.OutOfBoundsHandler verticalOutOfBoundsHandler) {
      Contract.Requires(destination != null);
      Contract.Requires(kernel != null);
      Contract.Requires(!(kernelRadius < 0));

      var srcWidth = this.Width;
      var srcHeight = this.Height;

      var destHeight = destination.Height;
      var destWidth = destination.Width;

      var buffer = new float[srcWidth * destHeight];
      var hFilter = _MakeScaleScanFilter(destWidth, xStart, xStep, srcWidth, kernel, kernelRadius, kernelNormalize, horizontalOutOfBoundsHandler);
      var vFilter = _MakeScaleScanFilter(destHeight, yStart, yStep, srcHeight, kernel, kernelRadius, kernelNormalize, verticalOutOfBoundsHandler);

      foreach (var plane in new[] {
        Tuple.Create(this._redPlane,destination._redPlane),
        Tuple.Create(this._greenPlane,destination._greenPlane),
        Tuple.Create(this._bluePlane,destination._bluePlane),
        Tuple.Create(this._alphaPlane,destination._alphaPlane),
      }) {
        for (var x = 0; x < srcWidth; x++)
          _ScaleScan(buffer, x, srcWidth, destHeight, plane.Item1, x, srcWidth, vFilter);

        for (var y = 0; y < destHeight; y++)
          _ScaleScan(plane.Item2, y * destWidth, 1, destWidth, buffer, y * srcWidth, 1, hFilter);
      }
    }

    private static void _ScaleScan(float[] dest, int destOrigin, int destStride, int destWidth, float[] src, int srcOrigin, int srcStride, ScaleScanFilter filter) {
      Contract.Requires(dest != null);
      Contract.Requires(src != null);
      var coeffIndex = 0;
      var destIndex = 0;
      for (var x = 0; x < destWidth; x++) {
        var srcIndex = filter.Pos[x] * srcStride;
        var sum = 0f;

        for (var k = 0; k < filter.Width; k++, srcIndex += srcStride)
          sum += filter.Coeff[coeffIndex + k] * src[srcOrigin + srcIndex];

        dest[destOrigin + destIndex] = sum;
        destIndex += destStride;
        coeffIndex += filter.Width;
      }
    }

    #endregion

  }
}
