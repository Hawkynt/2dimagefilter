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
using System.Collections.Generic;
using System.ComponentModel;

/*
 * Thanks to Pascal Getreuer who wrote some of these interpolation kernels in C++
 * See http://www.ipol.im/pub/art/2011/g_lmii/
 * I copied the comments over from the original sources and made changes to the code as I progressed.
 * 
 */

namespace Imager.Classes {
  public enum KernelType {
    [Description("Resizes the source image using a rectangular window function.")]
    Rectangular,
    [Description("Resizes the source image using a bicubic kernel function.")]
    Bicubic,
    [Description("Resizes the source image using the schaum 2nd-order kernel function with radius 1.5.")]
    Schaum2,
    [Description("Resizes the source image using the schaum 3rd-order kernel function with radius 2.")]
    Schaum3,
    [Description("Resizes the source image using the β-Spline 2nd-order kernel function.")]
    BSpline2,
    [Description("Resizes the source image using the β-Spline 3rd-order kernel function.")]
    BSpline3,
    [Description("Resizes the source image using the β-Spline 5th-order kernel function.")]
    BSpline5,
    [Description("Resizes the source image using the β-Spline 7th-order kernel function.")]
    BSpline7,
    [Description("Resizes the source image using the β-Spline 9th-order kernel function.")]
    BSpline9,
    [Description("Resizes the source image using the β-Spline 11th-order kernel function.")]
    BSpline11,
    [Description("Resizes the source image using the o-Moms 3rd-order kernel function.")]
    OMoms3,
    [Description("Resizes the source image using the o-Moms 5th-order kernel function.")]
    OMoms5,
    [Description("Resizes the source image using the o-Moms 7th-order kernel function.")]
    OMoms7
  }

  /// <summary>
  /// Contains all fixed-radius interpolation kernels.
  /// </summary>
  internal static class Kernels {
    public delegate double FixedRadiusKernelMethod(float n);

    public struct FixedRadiusKernelInfo {
      public FixedRadiusKernelMethod Kernel;
      public float KernelRadius;
      public bool KernelNormalize;
      public float[] PrefilterAlpha;
      public float PrefilterScale;
    }

    /// <summary>
    /// Lookup table for interpolation kernels
    /// </summary>
    internal static readonly Dictionary<KernelType, FixedRadiusKernelInfo> KERNELS = new Dictionary<KernelType, FixedRadiusKernelInfo> {
      {KernelType.Rectangular,new FixedRadiusKernelInfo{Kernel= _RectangularKernel,KernelRadius = 0.51f}},
      {KernelType.Bicubic,new FixedRadiusKernelInfo{Kernel= _BicubicKernel,KernelRadius = 2}},
      {KernelType.Schaum2,new FixedRadiusKernelInfo{Kernel=_Schaum2Kernel,KernelRadius = 1.51f}},
      {KernelType.Schaum3,new FixedRadiusKernelInfo{Kernel=_Schaum3Kernel,KernelRadius = 2}},
      {KernelType.BSpline2,new FixedRadiusKernelInfo{Kernel=_BSpline2Kernel,KernelRadius = 1.5f,PrefilterAlpha = new[] {
        -1.715728752538099e-1f /* exact value: -3 + sqrt(8) */
      } ,PrefilterScale = 8}},
      {KernelType.BSpline3,new FixedRadiusKernelInfo{Kernel=_BSpline3Kernel,KernelRadius = 2f,PrefilterAlpha = new[]{
        -2.679491924311227e-1f /* exact value: -2 + sqrt(3) */
      } ,PrefilterScale = 6}},
      {KernelType.BSpline5,new FixedRadiusKernelInfo{Kernel=_BSpline5Kernel,KernelRadius = 3f,PrefilterAlpha = new[]{
        -4.309628820326465e-2f, /* exact: sqrt(13*sqrt(105)+135)/sqrt(2)-sqrt(105)/2-13/2.0 */
        -4.305753470999738e-1f  /* exact: sqrt(105)/2+sqrt(135-13*sqrt(105))/sqrt(2)-13/2.0 */
      } ,PrefilterScale = 120}},
      {KernelType.BSpline7,new FixedRadiusKernelInfo{Kernel=_BSpline7Kernel,KernelRadius = 4f,PrefilterAlpha = new[] {
        -9.148694809608277e-3f, 
        -1.225546151923267e-1f, 
        -5.352804307964382e-1f
      },PrefilterScale = 5040}},
      {KernelType.BSpline9,new FixedRadiusKernelInfo{Kernel=_BSpline9Kernel,KernelRadius = 5f,PrefilterAlpha = new[] {
        -2.121306903180818e-3f,
        -4.322260854048175e-2f,
        -2.017505201931532e-1f, 
        -6.079973891686259e-1f
      },PrefilterScale = 362880}},
      {KernelType.BSpline11,new FixedRadiusKernelInfo{Kernel=_BSpline11Kernel,KernelRadius = 6f,PrefilterAlpha = new[] {
        -5.105575344465021e-4f, 
        -1.666962736623466e-2f, 
        -8.975959979371331e-2f,
        -2.721803492947859e-1f, 
        -6.612660689007345e-1f
      },PrefilterScale = 39916800}},
      {KernelType.OMoms3,new FixedRadiusKernelInfo{Kernel=_OMoms3Kernel,KernelRadius = 2,PrefilterAlpha = new[] {
        -3.441311542550502e-1f /* exact: (sqrt(105) - 13)/8 */
      },PrefilterScale = 21/4f}},
      {KernelType.OMoms5,new FixedRadiusKernelInfo{Kernel=_OMoms5Kernel,KernelRadius = 3,PrefilterAlpha = new[] {
        -7.092571896868541e-2f, 
        -4.758127100084396e-1f
      },PrefilterScale = 7920/107f}},
      {KernelType.OMoms7,new FixedRadiusKernelInfo{Kernel=_OMoms7Kernel,KernelRadius = 4,PrefilterAlpha = new[] {
        -1.976842538386140e-2f,
        -1.557007746773578e-1f,
        -5.685376180022930e-1f
      },PrefilterScale = 675675/346f}},
    };

    #region math lib wrappers
    private static float _Abs(float x) {
      return (x < 0 ? -x : x);
    }
    #endregion

    /// <summary>
    /// Nearest Neighbor interpolation kernel (KernelRadius = 0.5)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _RectangularKernel(float x) {
      if (-0.5f <= x && x < 0.5f)
        return 1;
      return 0;
    }

    /// <summary>
    /// Bicubic interpolation kernel (KernelRadius = 2)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _BicubicKernel(float x) {
      const float alpha = -0.5f;

      x = _Abs(x);

      if (x < 2) {
        if (x <= 1)
          return ((alpha + 2) * x - (alpha + 3)) * x * x + 1;
        return ((alpha * x - 5 * alpha) * x + 8 * alpha) * x - 4 * alpha;
      }
      return 0;
    }

    // TODO: quadratic

    /// <summary>
    /// Quadratic Schaum kernel (KernelRadius = 1.5)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _Schaum2Kernel(float x) {
      x = _Abs(x);

      /* This kernel is discontinuous.  At discontinuous points, it takes the
      average value of the left and right limits. */
      if (x < 0.5f)
        return 1 - x * x;
      if (x == 0.5f)
        return 0.5625f;
      if (x < 1.5f)
        return (x - 3) * x / 2 + 1;
      if (x == 1.5f)
        return -0.0625f;
      return 0;
    }

    /// <summary>
    /// Cubic Schaum kernel (KernelRadius = 2)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _Schaum3Kernel(float x) {
      x = _Abs(x);
      if (x <= 1)
        return ((x - 2) * x - 1) * x / 2 + 1;
      if (x < 2)
        return ((-x + 6) * x - 11) * x / 6 + 1;
      return 0;
    }

    // TODO: general b-spline
    // TODO: p-spline
    // TODO: hermite-spline

    /// <summary>
    /// Quadratic B-spline kernel (KernelRadius = 1.5)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _BSpline2Kernel(float x) {
      x = _Abs(x);
      if (x <= 0.5f)
        return 0.75f - x * x;
      if (x < 1.5f) {
        x = 1.5f - x;
        return x * x / 2;
      }
      return 0;
    }

    /// <summary>
    /// Cubic B-spline kernel (KernelRadius = 2)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _BSpline3Kernel(float x) {
      x = _Abs(x);
      if (x < 1)
        return (x / 2 - 1) * x * x + 0.66666666666666667f;
      if (x < 2) {
        x = 2 - x;
        return x * x * x / 6;
      }
      return 0;
    }

    /// <summary>
    /// Quintic B-spline kernel (KernelRadius = 3)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _BSpline5Kernel(float x) {
      x = _Abs(x);
      if (x <= 1) {
        var xSqr = x * x;
        return (((-10 * x + 30) * xSqr - 60) * xSqr + 66) / 120;
      }
      if (x < 2) {
        x = 2 - x;
        return (1 + (5 + (10 + (10 + (5 - 5 * x) * x) * x) * x) * x) / 120;
      }
      if (x < 3) {
        x = 3 - x;
        var xSqr = x * x;
        return xSqr * xSqr * x / 120;
      }
      return 0;
    }

    /// <summary>
    /// Septic B-spline kernel (KernelRadius = 4)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _BSpline7Kernel(float x) {
      x = _Abs(x);
      if (x <= 1) {
        var xSqr = x * x;
        return ((((35 * x - 140) * xSqr + 560) * xSqr - 1680) * xSqr + 2416) / 5040;
      }
      if (x <= 2) {
        x = 2 - x;
        return (120 + (392 + (504 + (280 + (-84 + (-42 + 21 * x) * x) * x * x) * x) * x) * x) / 5040;
      }
      if (x < 3) {
        x = 3 - x;
        return (((((((-7 * x + 7) * x + 21) * x + 35) * x + 35) * x + 21) * x + 7) * x + 1) / 5040;
      }
      if (x < 4) {
        x = 4 - x;
        var xSqr = x * x;
        return xSqr * xSqr * xSqr * x / 5040;
      }
      return 0;
    }

    /// <summary>
    /// Nonic B-spline kernel (KernelRadius = 5)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _BSpline9Kernel(float x) {
      x = _Abs(x);

      if (x <= 1) {
        var xSqr = x * x;
        return (((((-63 * x + 315) * xSqr - 2100) * xSqr + 11970) * xSqr - 44100) * xSqr + 78095) / 181440;
      }
      if (x <= 2) {
        x = 2 - x;
        return (14608 + (36414 + (34272 + (11256 + (-4032 + (-4284 + (-672 + (504 + (252 - 84 * x) * x) * x) * x) * x) * x) * x) * x) * x) / 362880;
      }
      if (x <= 3) {
        x = 3 - x;
        return (502 + (2214 + (4248 + (4536 + (2772 + (756 + (-168 + (-216 + (-72 + 36 * x) * x) * x) * x) * x) * x) * x) * x) * x) / 362880;
      }
      if (x < 4) {
        x = 4 - x;
        return (1 + (9 + (36 + (84 + (126 + (126 + (84 + (36 + (9 - 9 * x) * x) * x) * x) * x) * x) * x) * x) * x) / 362880;
      }
      if (x < 5) {
        x = 5 - x;
        var xCube = x * x * x;
        return xCube * xCube * xCube / 362880;
      }
      return 0;
    }

    /// <summary>
    /// 11th-Degree B-spline kernel (KernelRadius = 6)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _BSpline11Kernel(float x) {
      x = _Abs(x);

      if (x <= 1) {
        var xSqr = x * x;
        return (15724248 + (-7475160 + (1718640 + (-255024 + (27720 + (-2772 + 462 * x) * xSqr) * xSqr) * xSqr) * xSqr) * xSqr) / 39916800;
      }
      if (x <= 2) {
        x = 2 - x;
        return (2203488 + (4480872 + (3273600 + (574200 + (-538560
          + (-299376 + (39600 + (7920 + (-2640 + (-1320
            + 330 * x) * x) * x) * x) * x * x) * x) * x) * x) * x) * x) / 39916800;
      }
      if (x <= 3) {
        x = 3 - x;
        return (152637 + (515097 + (748275 + (586575 + (236610 + (12474
          + (-34650 + (-14850 + (-495 + (1485
            + (495 - 165 * x) * x) * x) * x) * x) * x) * x) * x) * x) * x) * x) / 39916800;
      }
      if (x < 4) {
        x = 4 - x;
        return (2036 + (11132 + (27500 + (40260 + (38280 + (24024 + (9240
          + (1320 + (-660 + (-440 + (-110
            + 55 * x) * x) * x) * x) * x) * x) * x) * x) * x) * x) * x) / 39916800;
      }
      if (x < 5) {
        x = 5 - x;
        return (1 + (11 + (55 + (165 + (330 + (462 + (462 + (330 + (165
          + (55 + (11 - 11 * x) * x) * x) * x) * x) * x) * x) * x) * x) * x) * x) / 39916800;
      }
      if (x < 6) {
        x = 6 - x;
        var xSqr = x * x;
        var xPow4 = xSqr * xSqr;
        return xPow4 * xPow4 * xSqr * x / 39916800;
      }
      return 0;
    }

    /// <summary>
    /// Cubic o-Moms kernel (KernelRadius = 2)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _OMoms3Kernel(float x) {
      x = _Abs(x);
      if (x < 1)
        return ((x / 2 - 1) * x + 1 / 14.0f) * x + 13 / 21.0f;
      if (x < 2)
        return ((-x / 6 + 1) * x - 85 / 42.0f) * x + 29 / 21.0f;
      return 0;
    }

    /// <summary>
    /// Quintic oMoms kernel (KernelRadius = 3)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _OMoms5Kernel(float x) {
      x = _Abs(x);

      if (x <= 1)
        return (((((-10 * x + 30) * x - (200 / 33.0f)) * x - (540 / 11.0f)) * x - (5 / 33.0f)) * x + (687 / 11.0f)) / 120;
      if (x < 2)
        return (((((330 * x - 2970) * x + 10100) * x
            - 14940) * x + 6755) * x + 2517) / 7920;
      if (x < 3) {
        x = 3 - x;
        var xSqr = x * x;
        return ((xSqr + (20 / 33.0f)) * xSqr + (1 / 66.0f)) * x / 120;
      }
      return 0;
    }

    /// <summary>
    /// Septic oMoms kernel (KernelRadius = 4)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static double _OMoms7Kernel(float x) {
      x = _Abs(x);

      if (x <= 1)
        return (((((((15015 * x - 60060) * x + 21021) * x + 180180) * x + 2695) * x - 629244) * x + 21) * x + 989636) / 2162160;
      if (x <= 2) {
        x = 2 - x;
        return (x * (x * (x * (x * (x * (x * (5005 * x - 10010) - 13013) - 10010) + 54285) + 119350) + 106267) + 36606) / 1201200;
      }
      if (x <= 3) {
        x = 3 - x;
        return (x * (x * (x * (x * (x * (x * (-15015 * x + 15015) + 24024) + 90090) + 102410) + 76230) + 31164) + 5536) / 10810800;
      }
      if (x < 4) {
        x = 4 - x;
        var xSqr = x * x;
        return (x * (xSqr * (xSqr * (2145 * xSqr + 3003) + 385) + 3)) / 10810800;
      }
      return 0;
    }

  }
}
