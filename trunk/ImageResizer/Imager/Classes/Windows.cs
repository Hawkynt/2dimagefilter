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
// TODO: radius&alpha/sigma parametrized resampling (as some windowing functions already support a radius and an alpha/sigma param)
// Note: There are still some todos in here, mostly when I could not get a good formula for the given window. If you have one, please send me a link.
using System;
using System.Collections.Generic;
using System.ComponentModel;
/*
 * Helpful links to understand what's going on in here:
 * http://www.virtins.com/doc/D1003/D1003.shtml#3.20
 * http://en.wikipedia.org/wiki/Window_function
 * 
 * Note: Kernel functions usually run from -radius to +radius, whereas windowing functions usually run from 0 to N
 * That's why we shift the x values by radius to get the n and create N by 2 * radius + 1.
 * This way, we can use the well-known windows formulas instead of deriving new forms for the kernels.
 * (Hopefully this makes it easier to understand though it costs a performance penalty.)
 * 
 */
namespace Imager.Classes {
  public enum WindowType {
    [Description("Resizes the source image using a triangular window function.")]
    Triangular,
    [Description("Resizes the source image using the welch window function.")]
    Welch,
    [Description("Resizes the source image using the hann window function.")]
    Hann,
    [Description("Resizes the source image using the hamming window function.")]
    Hamming,
    [Description("Resizes the source image using the blackman window function.")]
    Blackman,
    [Description("Resizes the source image using the nuttal window function.")]
    Nuttal,
    [Description("Resizes the source image using the blackman-nuttal window function.")]
    BlackmanNuttal,
    [Description("Resizes the source image using the blackman-harris window function.")]
    BlackmanHarris,
    [Description("Resizes the source image using the flat-top window function.")]
    FlatTop,
    [Description("Resizes the source image using the power-of-cosine window function.")]
    PowerOfCosine,
    [Description("Resizes the source image using the cosine window function.")]
    Cosine,
    [Description("Resizes the source image using the gaussian window function.")]
    Gauss,
    [Description("Resizes the source image using the tukey window function.")]
    Tukey,
    [Description("Resizes the source image using the poisson window function.")]
    Poisson,
    [Description("Resizes the source image using the bartlett-hann window function.")]
    BartlettHann,
    [Description("Resizes the source image using the hanning-poisson window function.")]
    HanningPoisson,
    [Description("Resizes the source image using the bohman window function.")]
    Bohman,
    [Description("Resizes the source image using the cauchy window function.")]
    Cauchy,
    [Description("Resizes the source image using a lanczos window function.")]
    Lanczos,
  }

  /// <summary>
  /// Contains all radius-adjustable kernels.
  /// </summary>
  internal static class Windows {
    public delegate double RadiusFreeKernelMethod(float n, float radius);

    public struct RadiusFreeKernelInfo {
      public RadiusFreeKernelMethod Kernel;
      public bool KernelNormalize;
      public float[] PrefilterAlpha;
      public float PrefilterScale;
      /// <summary>
      /// Creates a fixed radius kernel with the given radius.
      /// </summary>
      /// <param name="radius">The radius.</param>
      /// <returns></returns>
      public Kernels.FixedRadiusKernelInfo WithRadius(float radius) {
        var kernel = this.Kernel;
        var kernelNormalize = this.KernelNormalize;
        var prefilterAlpha = this.PrefilterAlpha;
        var prefilterScale = this.PrefilterScale;
        return (new Kernels.FixedRadiusKernelInfo { Kernel = f => kernel(f, radius), KernelRadius = radius, KernelNormalize = kernelNormalize, PrefilterAlpha = prefilterAlpha, PrefilterScale = prefilterScale });
      }
    }

    /// <summary>
    /// Lookup table for windowing functions
    /// </summary>
    internal static readonly Dictionary<WindowType, RadiusFreeKernelInfo> WINDOWS = new Dictionary<WindowType, RadiusFreeKernelInfo> {
      {WindowType.Triangular,new RadiusFreeKernelInfo{Kernel =  _TriangularWindow}},
      {WindowType.Welch,new RadiusFreeKernelInfo{Kernel= _WelchWindow}},
      {WindowType.Hann,new RadiusFreeKernelInfo{Kernel= _HannWindow}},
      {WindowType.Hamming,new RadiusFreeKernelInfo{Kernel= (f,r)=>_HammingWindow(f,0.53836d,r)}},
      {WindowType.Blackman,new RadiusFreeKernelInfo{Kernel= (f,r)=>_BlackmanWindow(f,-(2d*7938d)/18608d + 1,r)}},
      {WindowType.Nuttal,new RadiusFreeKernelInfo{Kernel= _NuttalWindow}},
      {WindowType.BlackmanNuttal,new RadiusFreeKernelInfo{Kernel= _BlackmanNuttalWindow}},
      {WindowType.BlackmanHarris,new RadiusFreeKernelInfo{Kernel= _BlackmanHarrisWindow}},
      {WindowType.FlatTop,new RadiusFreeKernelInfo{Kernel= _FlatTopWindow,KernelNormalize = true}},
      {WindowType.PowerOfCosine,new RadiusFreeKernelInfo{Kernel= (f,r)=>_PowerOfCosine(f,1.5f,r),KernelNormalize = true}},
      {WindowType.Cosine,new RadiusFreeKernelInfo{Kernel= _CosineWindow}},
      {WindowType.Gauss,new RadiusFreeKernelInfo{Kernel= (f,r)=>_GaussianWindow(f,0.4f,r)}},
      {WindowType.Tukey,new RadiusFreeKernelInfo{Kernel= (f,r)=>_TukeyWindow(f,0.5f,r)}},
      {WindowType.Poisson,new RadiusFreeKernelInfo{Kernel= (f,r)=>_PoissonWindow(f,60,r)}},
      {WindowType.BartlettHann,new RadiusFreeKernelInfo{Kernel= _BartlettHann}},
      {WindowType.HanningPoisson,new RadiusFreeKernelInfo{Kernel=(f,r)=> _HanningPoisson(f,2,r)}},
      {WindowType.Bohman,new RadiusFreeKernelInfo{Kernel=_BohmanWindow}},
      {WindowType.Cauchy,new RadiusFreeKernelInfo{Kernel=(f,r)=>_CauchyWindow(f,3,r)}},
      {WindowType.Lanczos,new RadiusFreeKernelInfo{Kernel=_LanczosKernel,KernelNormalize = true}},
    };

    #region math lib wrappers
    private const double _M_PI = Math.PI;

    private static float _Abs(float x) {
      return (x < 0 ? -x : x);
    }

    private static double _Abs(double x) {
      return (x < 0 ? -x : x);
    }

    private static double _Sin(double x) {
      return (Math.Sin(x));
    }

    private static double _Cos(double x) {
      return (Math.Cos(x));
    }

    private static double _Pow(double x, double n) {
      return (Math.Pow(x, n));
    }

    private static double _Exp(double x) {
      return (Math.Exp(x));
    }

    private static double _Sinc(double x) {
      var w = _M_PI * x;
      return (Math.Sin(w) / w);
    }
    #endregion


    /// <summary>
    /// Bilinear interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _TriangularWindow(float x, float radius) {
      x = _Abs(x);
      if (x < radius)
        return 1 - x / radius;
      return 0;
    }

    // TODO: parzen/de la Vallé Poussin window

    /// <summary>
    /// Welch interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _WelchWindow(float x, float radius) {
      x = _Abs(x);
      if (x < radius)
        return 1 - _Pow(x / radius, 2);
      return 0;
    }

    /// <summary>
    /// Hann interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _HannWindow(float x, float radius) {
      x = _Abs(x);
      if (x < radius)
        return (0.5f * (1 + _Cos(_M_PI * x / radius)));
      return (0);
    }

    /// <summary>
    /// Hamming interpolation kernel .
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="alpha">The alpha.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _HammingWindow(float x, double alpha, float radius) {
      x = _Abs(x);
      if (x < radius)
        return (alpha + (1 - alpha) * _Cos(_M_PI * x / radius));
      return (0);
    }

    /// <summary>
    /// Blackman interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="alpha">The alpha.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _BlackmanWindow(float x, double alpha, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      var a0 = (1 - alpha) / 2d;
      const double a1 = 0.5;
      var a2 = alpha / 2d;
      var doublePiScaled = 2 * _M_PI * n / (N - 1);
      var w = a0 - a1 * _Cos(doublePiScaled) + a2 * _Cos(2 * doublePiScaled);
      return (w);
    }

    // TODO: mitchell

    /// <summary>
    /// Nuttal interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _NuttalWindow(float x, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      const double a0 = 0.355768d;
      const double a1 = 0.487396d;
      const double a2 = 0.144232d;
      const double a3 = 0.012604d;
      var doublePiScaled = 2 * _M_PI * n / (N - 1);
      var w = a0 - a1 * _Cos(doublePiScaled) + a2 * _Cos(2 * doublePiScaled) - a3 * _Cos(3 * doublePiScaled);
      return (w);
    }

    /// <summary>
    /// Blackman-Nuttal interpolation kernel .
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _BlackmanNuttalWindow(float x, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      const double a0 = 0.3635819d;
      const double a1 = 0.4891775d;
      const double a2 = 0.1365995d;
      const double a3 = 0.0106411d;
      var doublePiScaled = 2 * _M_PI * n / (N - 1);
      var w = a0 - a1 * _Cos(doublePiScaled) + a2 * _Cos(2 * doublePiScaled) - a3 * _Cos(3 * doublePiScaled);
      return (w);
    }

    /// <summary>
    /// Blackman-Harris interpolation kernel .
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _BlackmanHarrisWindow(float x, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      const double a0 = 0.35875d;
      const double a1 = 0.48829d;
      const double a2 = 0.14128d;
      const double a3 = 0.01168d;
      var doublePiScaled = 2 * _M_PI * n / (N - 1);
      var w = a0 - a1 * _Cos(doublePiScaled) + a2 * _Cos(2 * doublePiScaled) - a3 * _Cos(3 * doublePiScaled);
      return (w);
    }

    /// <summary>
    /// Flat-Top interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _FlatTopWindow(float x, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      const double a0 = 1d;
      const double a1 = 1.93d;
      const double a2 = 1.29d;
      const double a3 = 0.388d;
      const double a4 = 0.028d;
      var doublePiScaled = 2 * _M_PI * n / (N - 1);
      var w = a0 - a1 * _Cos(doublePiScaled) + a2 * _Cos(2 * doublePiScaled) - a3 * _Cos(3 * doublePiScaled) + a4 * _Cos(4 * doublePiScaled);
      return (w);
    }

    /// <summary>
    /// Power of cosine interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="alpha">The alpha.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _PowerOfCosine(float x, float alpha, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      var w = _Pow(_Cos(_M_PI * n / (N - 1) - _M_PI / 2d), alpha);
      return (w);
    }

    /// <summary>
    /// Cosine interpolation kernel
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _CosineWindow(float x, float radius) {
      x = _Abs(x);
      if (x < radius)
        return (_Cos(_M_PI / 2d * x / radius));
      return (0);
    }

    /// <summary>
    /// Gaussian interpolation window.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="sigma">The sigma.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _GaussianWindow(float x, float sigma, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var w = _Exp(-0.5 * _Pow(x / (sigma * radius), 2));
      return (w);
    }

    /// <summary>
    /// Tukey interpolation window.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="alpha">The alpha.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _TukeyWindow(float x, float alpha, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      double w;

      if (n <= (alpha * (N - 1) / 2d))
        w = 0.5 * (1 + _Cos(_M_PI * (2 * n / (alpha * (N - 1)) - 1)));
      else if (n <= ((N - 1) * (1 - alpha / 2d)))
        w = 1;
      else
        w = 0.5 * (1 + _Cos(_M_PI * (2 * n / (alpha * (N - 1)) - 2 / alpha - 1)));

      return (w);
    }

    // TODO: Planck-Taper interpolation window
    // TODO: dpss/slepian
    // TODO: kaiser/kaiser-bessel
    // TODO: dolph-chebyshev

    /// <summary>
    /// Poisson/Exponential interpolation window.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="d">The d.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _PoissonWindow(float x, float d, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      var r = (N / 2) / (d / 8.69);
      var w = _Exp(-_Abs(n - (N - 1) / 2d) * (1 / r));

      return (w);
    }

    /// <summary>
    /// Bartlett-Hann interpolation window.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _BartlettHann(float x, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      const double a0 = 0.62;
      const double a1 = 0.48;
      const double a2 = 0.38;

      var w = a0 - a1 * _Abs(n / (N - 1) - 0.5) - a2 * _Cos(2 * _M_PI * n / (N - 1));

      return (w);
    }

    /// <summary>
    /// Hanning-Poisson interpolation window.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="alpha">The alpha.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _HanningPoisson(float x, float alpha, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      var w = 0.5 * (1 - _Cos(2 * _M_PI * n / (N - 1))) * _Exp((-alpha * _Abs(N - 1 - 2 * n)) / (N - 1));
      return (w);
    }

    // TODO: sinc
    // TODO: riemann

    /// <summary>
    /// Bohman interpolation window.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _BohmanWindow(float x, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var factor = _Abs(x / radius);
      var w = (1 - factor) * _Cos(_M_PI * factor) + _Sin(_M_PI * factor) / _M_PI;
      return (w);
    }

    /// <summary>
    /// Cauchy interpolation window.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="alpha">The alpha.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _CauchyWindow(float x, float alpha, float radius) {
      if (x < -radius || x > radius)
        return (0);

      var n = (double)x + radius;
      var N = 2d * radius + 1;

      var w = 1 / (1 + _Pow(alpha * _Abs(x) / radius, 2));
      return (w);
    }

    /// <summary>
    /// Lanczos interpolation kernel 
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static double _LanczosKernel(float x, float radius) {
      if (x < -radius || x > radius)
        return (0);

      if (x != 0)
        return _Sin(_M_PI * x) * _Sin((_M_PI / radius) * x) / ((_M_PI * _M_PI / radius) * x * x);

      return 1;
    }

    // TODO: rife-vincent


  }
}
