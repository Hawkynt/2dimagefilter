using System;
using System.Collections.Generic;

/*
 * Thanks to Pascal Getreuer who wrote these interpolation kernels in C++
 * See http://www.ipol.im/pub/art/2011/g_lmii/
 * I copied the comments over from the original sources and made changes to the code as I progressed.
 * 
 */

namespace Imager.Classes {
  public enum KernelType {
    NearestNeighbor,
    Bilinear,
    Bicubic,
    Lanczos2,
    Lanczos3,
    Lanczos4,
    Schaum2,
    Schaum3,
    BSpline2,
    BSpline3,
    BSpline5,
    BSpline7,
    BSpline9,
    BSpline11,
    OMoms3,
    OMoms5,
    OMoms7
  }

  internal static class Kernels {
    public delegate float KernelDelegate(float n);

    public struct InterpMethod {
      public KernelDelegate Kernel;
      public float KernelRadius;
      public bool KernelNormalize;
      public float[] PrefilterAlpha;
      public float PrefilterScale;
    }

    internal static readonly Dictionary<KernelType, InterpMethod> KERNELS = new Dictionary<KernelType, InterpMethod> {
      {KernelType.NearestNeighbor,new InterpMethod{Kernel= NearestNeighborKernel,KernelRadius = 0.51f,PrefilterScale = 1}},
      {KernelType.Bilinear,new InterpMethod{Kernel= BilinearKernel,KernelRadius = 1,PrefilterScale = 1}},
      {KernelType.Bicubic,new InterpMethod{Kernel= BicubicKernel,KernelRadius = 2,PrefilterScale = 1}},
      {KernelType.Lanczos2,new InterpMethod{Kernel=f=>LanczosKernel(f,2),KernelRadius = 2,KernelNormalize = true,PrefilterScale = 1}},
      {KernelType.Lanczos3,new InterpMethod{Kernel=f=>LanczosKernel(f,3),KernelRadius = 3,KernelNormalize = true,PrefilterScale = 1}},
      {KernelType.Lanczos4,new InterpMethod{Kernel=f=>LanczosKernel(f,4),KernelRadius = 4,KernelNormalize = true,PrefilterScale = 1}},
      {KernelType.Schaum2,new InterpMethod{Kernel=Schaum2Kernel,KernelRadius = 1.51f,PrefilterScale = 1}},
      {KernelType.Schaum3,new InterpMethod{Kernel=Schaum3Kernel,KernelRadius = 2,PrefilterScale = 1}},
      {KernelType.BSpline2,new InterpMethod{Kernel=BSpline2Kernel,KernelRadius = 1.5f,PrefilterAlpha = new[] {
        -1.715728752538099e-1f /* exact value: -3 + sqrt(8) */
      } ,PrefilterScale = 8}},
      {KernelType.BSpline3,new InterpMethod{Kernel=BSpline3Kernel,KernelRadius = 2f,PrefilterAlpha = new[]{
        -2.679491924311227e-1f /* exact value: -2 + sqrt(3) */
      } ,PrefilterScale = 6}},
      {KernelType.BSpline5,new InterpMethod{Kernel=BSpline5Kernel,KernelRadius = 3f,PrefilterAlpha = new[]{
        -4.309628820326465e-2f, /* exact: sqrt(13*sqrt(105)+135)/sqrt(2)-sqrt(105)/2-13/2.0 */
        -4.305753470999738e-1f  /* exact: sqrt(105)/2+sqrt(135-13*sqrt(105))/sqrt(2)-13/2.0 */
      } ,PrefilterScale = 120}},
      {KernelType.BSpline7,new InterpMethod{Kernel=BSpline7Kernel,KernelRadius = 4f,PrefilterAlpha = new[] {
        -9.148694809608277e-3f, 
        -1.225546151923267e-1f, 
        -5.352804307964382e-1f
      },PrefilterScale = 5040}},
      {KernelType.BSpline9,new InterpMethod{Kernel=BSpline9Kernel,KernelRadius = 5f,PrefilterAlpha = new[] {
        -2.121306903180818e-3f,
        -4.322260854048175e-2f,
        -2.017505201931532e-1f, 
        -6.079973891686259e-1f
      },PrefilterScale = 362880}},
      {KernelType.BSpline11,new InterpMethod{Kernel=BSpline11Kernel,KernelRadius = 6f,PrefilterAlpha = new[] {
        -5.105575344465021e-4f, 
        -1.666962736623466e-2f, 
        -8.975959979371331e-2f,
        -2.721803492947859e-1f, 
        -6.612660689007345e-1f
      },PrefilterScale = 39916800}},
      {KernelType.OMoms3,new InterpMethod{Kernel=OMoms3Kernel,KernelRadius = 2,PrefilterAlpha = new[] {
        -3.441311542550502e-1f /* exact: (sqrt(105) - 13)/8 */
      },PrefilterScale = 21/4f}},
      {KernelType.OMoms5,new InterpMethod{Kernel=OMoms5Kernel,KernelRadius = 3,PrefilterAlpha = new[] {
        -7.092571896868541e-2f, 
        -4.758127100084396e-1f
      },PrefilterScale = 7920/107f}},
      {KernelType.OMoms7,new InterpMethod{Kernel=OMoms7Kernel,KernelRadius = 4,PrefilterAlpha = new[] {
        -1.976842538386140e-2f,
        -1.557007746773578e-1f,
        -5.685376180022930e-1f
      },PrefilterScale = 675675/346f}},
    };

    private const float M_PI = (float)Math.PI;

    private static float Sin(float x) {
      return ((float)Math.Sin(x));
    }

    /// <summary>
    /// Nearest Neighbor interpolation kernel (KernelRadius = 0.5)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static float NearestNeighborKernel(float x) {
      if (-0.5f <= x && x < 0.5f)
        return 1;
      return 0;
    }

    /// <summary>
    /// Bilinear interpolation kernel (KernelRadius = 1)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static float BilinearKernel(float x) {
      x = Math.Abs(x);
      if (x < 1)
        return 1 - x;
      return 0;
    }

    /// <summary>
    /// Bicubic interpolation kernel (KernelRadius = 2)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static float BicubicKernel(float x) {
      const float alpha = -0.5f;

      x = Math.Abs(x);

      if (x < 2) {
        if (x <= 1)
          return ((alpha + 2) * x - (alpha + 3)) * x * x + 1;
        return ((alpha * x - 5 * alpha) * x + 8 * alpha) * x - 4 * alpha;
      }
      return 0;
    }

    /// <summary>
    /// Lanczos-n interpolation kernel (KernelRadius = n)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static float LanczosKernel(float x, float radius) {
      if (-radius < x && x < radius) {
        if (x != 0)
          return Sin(M_PI * x) * Sin((M_PI / radius) * x) / ((M_PI * M_PI / radius) * x * x);
        return 1;
      }
      return 0;
    }

    /// <summary>
    /// Quadratic Schaum kernel (KernelRadius = 1.5)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static float Schaum2Kernel(float x) {
      x = Math.Abs(x);

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
    private static float Schaum3Kernel(float x) {
      x = Math.Abs(x);
      if (x <= 1)
        return ((x - 2) * x - 1) * x / 2 + 1;
      if (x < 2)
        return ((-x + 6) * x - 11) * x / 6 + 1;
      return 0;
    }

    /// <summary>
    /// Quadratic B-spline kernel (KernelRadius = 1.5)
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static float BSpline2Kernel(float x) {
      x = Math.Abs(x);
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
    private static float BSpline3Kernel(float x) {
      x = Math.Abs(x);
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
    private static float BSpline5Kernel(float x) {
      x = Math.Abs(x);
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
    private static float BSpline7Kernel(float x) {
      x = Math.Abs(x);
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
    private static float BSpline9Kernel(float x) {
      x = Math.Abs(x);

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
    private static float BSpline11Kernel(float x) {
      x = Math.Abs(x);

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
    private static float OMoms3Kernel(float x) {
      x = Math.Abs(x);
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
    private static float OMoms5Kernel(float x) {
      x = Math.Abs(x);

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
    private static float OMoms7Kernel(float x) {
      x = Math.Abs(x);

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
