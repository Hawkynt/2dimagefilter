

using System;
using System.Runtime.CompilerServices;

namespace Imager.Filters {
  internal class libXBRz {

    public class ScaleSize {
      public static readonly ScaleSize TIMES2 = new ScaleSize(_SCALER2_X);
      public static readonly ScaleSize TIMES3 = new ScaleSize(_SCALER3_X);
      public static readonly ScaleSize TIMES4 = new ScaleSize(_SCALER4_X);
      public static readonly ScaleSize TIMES5 = new ScaleSize(_SCALER5_X);

      private ScaleSize(IScaler scaler) {
        this.scaler = scaler;
        this.size = scaler.Scale();
      }

      internal IScaler scaler;
      public int size;
    }

    public class ScalerCfg {
      // These are the default values:
      public double luminanceWeight = 1;
      public double equalColorTolerance = 30;
      public double dominantDirectionThreshold = 3.6;
      public double steepDirectionThreshold = 2.2;
    }

    private static readonly ScalerCfg _CONFIGURATION = new ScalerCfg();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void _AlphaBlend(int n, int m, ImagePointer dstPtr, sPixel col) {
      dstPtr.SetPixel(sPixel.Interpolate(col, dstPtr.GetPixel(), n, m - n));
    }

    //fill block with the given color
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void _FillBlock(sPixel[] trg, int trgi, int pitch, sPixel col, int blockSize) {
      for (var y = 0; y < blockSize; ++y, trgi += pitch)
        for (var x = 0; x < blockSize; ++x)
          trg[trgi + x] = col;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double _Square(double value) {
      return value * value;
    }

    private static double _DistYCbCr(sPixel pix1, sPixel pix2, double lumaWeight) {
      //http://en.wikipedia.org/wiki/YCbCr#ITU-R_BT.601_conversion
      //YCbCr conversion is a matrix multiplication => take advantage of linearity by subtracting first!
      var rDiff = pix1.Red - pix2.Red;
      var gDiff = pix1.Green - pix2.Green;
      var bDiff = pix1.Blue - pix2.Blue;

      const double kB = 0.0722; //ITU-R BT.709 conversion
      const double kR = 0.2126; //
      const double kG = 1 - kB - kR;

      const double scaleB = 0.5 / (1 - kB);
      const double scaleR = 0.5 / (1 - kR);

      var y = kR * rDiff + kG * gDiff + kB * bDiff; //[!], analog YCbCr!
      var cB = scaleB * (bDiff - y);
      var cR = scaleR * (rDiff - y);

      // Skip division by 255.
      // Also skip square root here by pre-squaring the
      // config option equalColorTolerance.
      //return Math.sqrt(square(lumaWeight * y) + square(c_b) + square(c_r));
      return _Square(lumaWeight * y) + _Square(cB) + _Square(cR);
    }

    // private static double distNonLinearRGB(
    //  int pix1,
    //  int pix2
    // ) {
    //  //non-linear rgb: http://www.compuphase.com/cmetric.htm
    //  double r_diff = ((pix1 & redMask) - (pix2 & redMask)) >> 16; //we may delay division by 255 to after matrix multiplication
    //  double g_diff = ((pix1 & greenMask) - (pix2 & greenMask)) >> 8; //
    //  double b_diff = (pix1 & blueMask) - (pix2 & blueMask); //subtraction for int is noticeable faster than for double
    //
    //  double r_avg = (double)(((pix1 & redMask) + (pix2 & redMask)) >> 16) / 2;
    //  return ((2 + r_avg / 255) * square(r_diff) + 4 * square(g_diff) + (2 + (255 - r_avg) / 255) * square(b_diff));
    // }

    private static double _ColorDist(sPixel pix1, sPixel pix2, double luminanceWeight) {
      return pix1 == pix2 ? 0 : _DistYCbCr(pix1, pix2, luminanceWeight);

      //  return distNonLinearRGB(pix1, pix2);
    }

    private enum BlendType : byte {
      // These blend types must fit into 2 bits.
      BlendNone = 0, //do not blend
      BlendNormal = 1, //a normal indication to blend
      BlendDominant = 2, //a strong indication to blend
    }

    private class BlendResult {
      public BlendType f;
      public BlendType g;
      public BlendType j;
      public BlendType k;

      public void Reset() {
        this.f = this.g = this.j = this.k = BlendType.BlendNone;
      }
    }

    private class Kernel_3X3 {
      public readonly sPixel[] _ = new sPixel[3 * 3];
    }

    private class Kernel_4X4 {
      public sPixel b, c;
      public sPixel e, f, g, h;
      public sPixel i, j, k, l;
      public sPixel n, o;
    }

    /*
  input kernel area naming convention:
  -----------------
  | A | B | C | D |
  ----|---|---|---|
  | E | F | G | H | //evalute the four corners between F, G, J, K
  ----|---|---|---| //input pixel is at position F
  | I | J | K | L |
  ----|---|---|---|
  | M | N | O | P |
  -----------------
  */

    //detect blend direction
    private static void _PreProcessCorners(Kernel_4X4 kernel, BlendResult blendResult, IColorDist preProcessCornersColorDist) {
      blendResult.Reset();

      if ((kernel.f == kernel.g && kernel.j == kernel.k) || (kernel.f == kernel.j && kernel.g == kernel.k))
        return;

      var dist = preProcessCornersColorDist;

      var weight = 4;
      var jg = dist._(kernel.i, kernel.f) + dist._(kernel.f, kernel.c) + dist._(kernel.n, kernel.k) + dist._(kernel.k, kernel.h) + weight * dist._(kernel.j, kernel.g);
      var fk = dist._(kernel.e, kernel.j) + dist._(kernel.j, kernel.o) + dist._(kernel.b, kernel.g) + dist._(kernel.g, kernel.l) + weight * dist._(kernel.f, kernel.k);

      if (jg < fk) {

        var dominantGradient = _CONFIGURATION.dominantDirectionThreshold * jg < fk;
        if (kernel.f != kernel.g && kernel.f != kernel.j)
          blendResult.f = dominantGradient ? BlendType.BlendDominant : BlendType.BlendNormal;

        if (kernel.k != kernel.j && kernel.k != kernel.g)
          blendResult.k = dominantGradient ? BlendType.BlendDominant : BlendType.BlendNormal;

      } else if (fk < jg) {

        var dominantGradient = _CONFIGURATION.dominantDirectionThreshold * fk < jg;
        if (kernel.j != kernel.f && kernel.j != kernel.k)
          blendResult.j = dominantGradient ? BlendType.BlendDominant : BlendType.BlendNormal;

        if (kernel.g != kernel.f && kernel.g != kernel.k)
          blendResult.g = dominantGradient ? BlendType.BlendDominant : BlendType.BlendNormal;

      }
    }

    private static class Rot {
      // Cache the 4 rotations of the 9 positions, a to i.
      public static readonly int[] _ = new int[9 * 4];

      static Rot() {
        const int
          a = 0,
          b = 1,
          c = 2,
          d = 3,
          e = 4,
          f = 5,
          g = 6,
          h = 7,
          i = 8;

        var deg0 = new[] {
          a, b, c,
          d, e, f,
          g, h, i
        };

        var deg90 = new[] {
          g, d, a,
          h, e, b,
          i, f, c
        };

        var deg180 = new[] {
          i, h, g,
          f, e, d,
          c, b, a
        };

        var deg270 = new[] {
          c, f, i,
          b, e, h,
          a, d, g
        };

        var rotation = new[] {
          deg0, deg90, deg180, deg270
        };

        for (var rotDeg = 0; rotDeg < 4; rotDeg++)
          for (var x = 0; x < 9; x++)
            _[(x << 2) + rotDeg] = rotation[rotDeg][x];
      }
    }

    /*
  input kernel area naming convention:
  -------------
  | A | B | C |
  ----|---|---|
  | D | E | F | //input pixel is at position E
  ----|---|---|
  | G | H | I |
  -------------
  */

    private static void _ScalePixel(
      IScaler scaler,
      RotationDegree rotDeg,
      Kernel_3X3 ker,
      sPixel[] trg,
      int trgi,
      int trgWidth,
      byte blendInfo,//result of preprocessing all four corners of pixel "e"
      IColorEq scalePixelColorEq,
      IColorDist scalePixelColorDist,
      OutputMatrix outputMatrix
      ) {
      //int a = kernel._[Rot._[(0 << 2) + rotDeg]];
      var b = ker._[Rot._[(1 << 2) + (int)rotDeg]];
      var c = ker._[Rot._[(2 << 2) + (int)rotDeg]];
      var d = ker._[Rot._[(3 << 2) + (int)rotDeg]];
      var e = ker._[Rot._[(4 << 2) + (int)rotDeg]];
      var f = ker._[Rot._[(5 << 2) + (int)rotDeg]];
      var g = ker._[Rot._[(6 << 2) + (int)rotDeg]];
      var h = ker._[Rot._[(7 << 2) + (int)rotDeg]];
      var i = ker._[Rot._[(8 << 2) + (int)rotDeg]];

      var blend = BlendInfo.Rotate(blendInfo, rotDeg);

      if (BlendInfo.GetBottomR(blend) == BlendType.BlendNone)
        return;

      var eq = scalePixelColorEq;
      var dist = scalePixelColorDist;

      bool doLineBlend;

      if (BlendInfo.GetBottomR(blend) >= BlendType.BlendDominant)
        doLineBlend = true;

        //make sure there is no second blending in an adjacent
      //rotation for this pixel: handles insular pixels, mario eyes
      //but support double-blending for 90? corners
      else if (BlendInfo.GetTopR(blend) != BlendType.BlendNone && !eq._(e, g))
        doLineBlend = false;

      else if (BlendInfo.GetBottomL(blend) != BlendType.BlendNone && !eq._(e, c))
        doLineBlend = false;

        //no full blending for L-shapes; blend corner only (handles "mario mushroom eyes")
      else if (eq._(g, h) && eq._(h, i) && eq._(i, f) && eq._(f, c) && !eq._(e, i))
        doLineBlend = false;

      else
        doLineBlend = true;

      //choose most similar color
      var px = dist._(e, f) <= dist._(e, h) ? f : h;

      var output = outputMatrix;
      output.Move(rotDeg, trgi);

      if (!doLineBlend) {
        scaler.BlendCorner(px, output);
        return;
      }

      //test sample: 70% of values max(fg, hc) / min(fg, hc)
      //are between 1.1 and 3.7 with median being 1.9
      var fg = dist._(f, g);
      var hc = dist._(h, c);

      var haveShallowLine = _CONFIGURATION.steepDirectionThreshold * fg <= hc && e != g && d != g;
      var haveSteepLine = _CONFIGURATION.steepDirectionThreshold * hc <= fg && e != c && b != c;

      if (haveShallowLine) {
        if (haveSteepLine)
          scaler.BlendLineSteepAndShallow(px, output);
        else
          scaler.BlendLineShallow(px, output);
      } else {
        if (haveSteepLine)
          scaler.BlendLineSteep(px, output);
        else
          scaler.BlendLineDiagonal(px, output);
      }
    }

    //scaler policy: see "Scaler2x" reference implementation

    private class ColorDistA : IColorDist {
      public double _(sPixel col1, sPixel col2) {
        return _ColorDist(col1, col2, _CONFIGURATION.luminanceWeight);
      }
    }

    private class ColorEqA : IColorEq {
      private readonly double _eqColorThres;

      public ColorEqA(double a) {
        this._eqColorThres = a;
      }

      public bool _(sPixel col1, sPixel col2) {
        return _ColorDist(col1, col2, _CONFIGURATION.luminanceWeight) < this._eqColorThres;
      }
    }

    public static void ScaleImage(ScaleSize scaleSize, sPixel[] src, sPixel[] trg, int srcWidth, int srcHeight, int xFirst, int yFirst, int xLast, int yLast) {
      yFirst = Math.Max(yFirst, 0);
      yLast = Math.Min(yLast, srcHeight);

      if (yFirst >= yLast || srcWidth <= 0)
        return;

      var trgWidth = srcWidth * scaleSize.size;

      //temporary buffer for "on the fly preprocessing"
      var preProcBuffer = new byte[srcWidth];

      var ker4 = new Kernel_4X4();

      var preProcessCornersColorDist = new ColorDistA();

      //initialize preprocessing buffer for first row:
      //detect upper left and right corner blending
      //this cannot be optimized for adjacent processing
      //stripes; we must not allow for a memory race condition!
      if (yFirst > 0) {
        var y = yFirst - 1;

        var sM1 = srcWidth * Math.Max(y - 1, 0);
        var s0 = srcWidth * y; //center line
        var sP1 = srcWidth * Math.Min(y + 1, srcHeight - 1);
        var sP2 = srcWidth * Math.Min(y + 2, srcHeight - 1);

        for (var x = xFirst; x < xLast; ++x) {
          var xM1 = Math.Max(x - 1, 0);
          var xP1 = Math.Min(x + 1, srcWidth - 1);
          var xP2 = Math.Min(x + 2, srcWidth - 1);

          //read sequentially from memory as far as possible
          ker4.b = src[sM1 + x];
          ker4.c = src[sM1 + xP1];

          ker4.e = src[s0 + xM1];
          ker4.f = src[s0 + x];
          ker4.g = src[s0 + xP1];
          ker4.h = src[s0 + xP2];

          ker4.i = src[sP1 + xM1];
          ker4.j = src[sP1 + x];
          ker4.k = src[sP1 + xP1];
          ker4.l = src[sP1 + xP2];

          ker4.n = src[sP2 + x];
          ker4.o = src[sP2 + xP1];

          var blendResult = new BlendResult();
          _PreProcessCorners(ker4, blendResult, preProcessCornersColorDist); // writes to blendResult
          /*
     preprocessing blend result:
     ---------
     | F | G | //evalute corner between F, G, J, K
     ----|---| //input pixel is at position F
     | J | K |
     ---------
     */
          preProcBuffer[x] = BlendInfo.SetTopR(preProcBuffer[x], blendResult.j);

          if (x + 1 < srcWidth)
            preProcBuffer[x + 1] = BlendInfo.SetTopL(preProcBuffer[x + 1], blendResult.k);
        }
      }

      var eqColorThres = _Square(_CONFIGURATION.equalColorTolerance);

      var scalePixelColorEq = new ColorEqA(eqColorThres);
      var scalePixelColorDist = new ColorDistA();
      var outputMatrix = new OutputMatrix(scaleSize.size, trg, trgWidth);

      var ker3 = new Kernel_3X3();

      for (var y = yFirst; y < yLast; ++y) {
        //consider MT "striped" access
        var trgi = scaleSize.size * y * trgWidth;

        var sM1 = srcWidth * Math.Max(y - 1, 0);
        var s0 = srcWidth * y; //center line
        var sP1 = srcWidth * Math.Min(y + 1, srcHeight - 1);
        var sP2 = srcWidth * Math.Min(y + 2, srcHeight - 1);

        byte blendXy1 = 0;

        for (var x = xFirst; x < xLast; ++x, trgi += scaleSize.size) {
          var xM1 = Math.Max(x - 1, 0);
          var xP1 = Math.Min(x + 1, srcWidth - 1);
          var xP2 = Math.Min(x + 2, srcWidth - 1);

          //evaluate the four corners on bottom-right of current pixel
          //blend_xy for current (x, y) position
          byte blendXy;
          {
            //read sequentially from memory as far as possible
            ker4.b = src[sM1 + x];
            ker4.c = src[sM1 + xP1];

            ker4.e = src[s0 + xM1];
            ker4.f = src[s0 + x];
            ker4.g = src[s0 + xP1];
            ker4.h = src[s0 + xP2];

            ker4.i = src[sP1 + xM1];
            ker4.j = src[sP1 + x];
            ker4.k = src[sP1 + xP1];
            ker4.l = src[sP1 + xP2];

            ker4.n = src[sP2 + x];
            ker4.o = src[sP2 + xP1];

            var blendResult = new BlendResult();
            _PreProcessCorners(ker4, blendResult, preProcessCornersColorDist); // writes to blendResult

            /*
      preprocessing blend result:
      ---------
      | F | G | //evaluate corner between F, G, J, K
      ----|---| //current input pixel is at position F
      | J | K |
      ---------
      */

            //all four corners of (x, y) have been determined at
            //this point due to processing sequence!
            blendXy = BlendInfo.SetBottomR(preProcBuffer[x], blendResult.f);

            //set 2nd known corner for (x, y + 1)
            blendXy1 = BlendInfo.SetTopR(blendXy1, blendResult.j);
            //store on current buffer position for use on next row
            preProcBuffer[x] = blendXy1;

            //set 1st known corner for (x + 1, y + 1) and
            //buffer for use on next column
            blendXy1 = BlendInfo.SetTopL(0, blendResult.k);

            if (x + 1 < srcWidth)
              //set 3rd known corner for (x + 1, y)
              preProcBuffer[x + 1] = BlendInfo.SetBottomL(preProcBuffer[x + 1], blendResult.g);
          }

          //fill block of size scale * scale with the given color
          //  //place *after* preprocessing step, to not overwrite the
          //  //results while processing the the last pixel!
          _FillBlock(trg, trgi, trgWidth, src[s0 + x], scaleSize.size);

          //blend four corners of current pixel
          if (blendXy == 0)
            continue;

          const int a = 0, b = 1, c = 2, d = 3, e = 4, f = 5, g = 6, h = 7, i = 8;

          //read sequentially from memory as far as possible
          ker3._[a] = src[sM1 + xM1];
          ker3._[b] = src[sM1 + x];
          ker3._[c] = src[sM1 + xP1];

          ker3._[d] = src[s0 + xM1];
          ker3._[e] = src[s0 + x];
          ker3._[f] = src[s0 + xP1];

          ker3._[g] = src[sP1 + xM1];
          ker3._[h] = src[sP1 + x];
          ker3._[i] = src[sP1 + xP1];

          _ScalePixel(scaleSize.scaler, RotationDegree.Rot0, ker3, trg, trgi, trgWidth, blendXy, scalePixelColorEq, scalePixelColorDist, outputMatrix);
          _ScalePixel(scaleSize.scaler, RotationDegree.Rot90, ker3, trg, trgi, trgWidth, blendXy, scalePixelColorEq, scalePixelColorDist, outputMatrix);
          _ScalePixel(scaleSize.scaler, RotationDegree.Rot180, ker3, trg, trgi, trgWidth, blendXy, scalePixelColorEq, scalePixelColorDist, outputMatrix);
          _ScalePixel(scaleSize.scaler, RotationDegree.Rot270, ker3, trg, trgi, trgWidth, blendXy, scalePixelColorEq, scalePixelColorDist, outputMatrix);
        }
      }
    }

    private interface IColorEq {
      bool _(sPixel col1, sPixel col2);
    }

    private interface IColorDist {
      double _(sPixel col1, sPixel col2);
    }

    private static class BlendInfo {
      public static BlendType GetTopL(byte b) {
        return (BlendType)((b) & 0x3);
      }

      public static BlendType GetTopR(byte b) {
        return (BlendType)((b >> 2) & 0x3);
      }

      public static BlendType GetBottomR(byte b) {
        return (BlendType)((b >> 4) & 0x3);
      }

      public static BlendType GetBottomL(byte b) {
        return (BlendType)((b >> 6) & 0x3);
      }

      public static byte SetTopL(byte b, BlendType bt) {
        return (byte)(b | (byte)bt);
      }

      public static byte SetTopR(byte b, BlendType bt) {
        return (byte)(b | ((byte)bt << 2));
      }

      public static byte SetBottomR(byte b, BlendType bt) {
        return (byte)(b | ((byte)bt << 4));
      }

      public static byte SetBottomL(byte b, BlendType bt) {
        return (byte)(b | ((byte)bt << 6));
      }

      public static byte Rotate(byte b, RotationDegree rotDeg) {
        var l = (int)rotDeg << 1;
        var r = 8 - l;

        return (byte)(b << l | b >> r);
      }
    }

    //clock-wise
    internal enum RotationDegree {
      Rot0 = 0,
      Rot90 = 1,
      Rot180 = 2,
      Rot270 = 3,
    }

    private const int _MAX_ROTS = 4; // Number of 90 degree rotations
    private const int _MAX_SCALE = 5; // Highest possible scale
    private const int _MAX_SCALE_SQUARED = _MAX_SCALE * _MAX_SCALE;
    private static readonly Tuple[] _MATRIX_ROTATION;

    //calculate input matrix coordinates after rotation at program startup
    static libXBRz() {
      _MATRIX_ROTATION = new Tuple[(_MAX_SCALE - 1) * _MAX_SCALE_SQUARED * _MAX_ROTS];
      for (var n = 2; n < _MAX_SCALE + 1; n++)
        for (var r = 0; r < _MAX_ROTS; r++) {
          var nr = (n - 2) * (_MAX_ROTS * _MAX_SCALE_SQUARED) + r * _MAX_SCALE_SQUARED;
          for (var i = 0; i < _MAX_SCALE; i++)
            for (var j = 0; j < _MAX_SCALE; j++)
              _MATRIX_ROTATION[nr + i * _MAX_SCALE + j] =
                _BuildMatrixRotation(r, i, j, n);
        }
    }

    private static Tuple _BuildMatrixRotation(int rotDeg, int i, int j, int n) {
      int iOld;
      int jOld;

      if (rotDeg == 0) {
        iOld = i;
        jOld = j;
      } else {
        //old coordinates before rotation!
        var old = _BuildMatrixRotation(rotDeg - 1, i, j, n);
        iOld = n - 1 - old.Item2;
        jOld = old.Item1;
      }

      return new Tuple(iOld, jOld);
    }

    //access matrix area, top-left at position "out" for image with given width
    internal class OutputMatrix {
      private readonly ImagePointer _output;
      private int _outi;
      private readonly int _outWidth;
      private readonly int _n;
      private int _nr;

      public OutputMatrix(int scale, sPixel[] output, int outWidth) {
        this._n = (scale - 2) * (_MAX_ROTS * _MAX_SCALE_SQUARED);
        this._output = new ImagePointer(output);
        this._outWidth = outWidth;
      }

      public void Move(RotationDegree rotDeg, int outi) {
        this._nr = this._n + (int)rotDeg * _MAX_SCALE_SQUARED;
        this._outi = outi;
      }

      public ImagePointer Reference(int i, int j) {
        var rot = _MATRIX_ROTATION[this._nr + i * _MAX_SCALE + j];
        this._output.Position(this._outi + rot.Item2 + rot.Item1 * this._outWidth);
        return this._output
          ;
      }
    }

    private class Tuple {
      public int Item1 { get; private set; }
      public int Item2 { get; private set; }

      public Tuple(int i, int j) {
        this.Item1 = i;
        this.Item2 = j;
      }
    }

    internal class ImagePointer {
      private readonly sPixel[] _imageData;
      private int _offset;

      public ImagePointer(sPixel[] imageData) {
        this._imageData = imageData;
      }

      public void Position(int offset) {
        this._offset = offset;
      }

      public sPixel GetPixel() {
        return this._imageData[this._offset];
      }

      public void SetPixel(sPixel val) {
        this._imageData[this._offset] = val;
      }
    }

    internal interface IScaler {
      int Scale();
      void BlendLineSteep(sPixel col, OutputMatrix output);
      void BlendLineSteepAndShallow(sPixel col, OutputMatrix output);
      void BlendLineShallow(sPixel col, OutputMatrix output);
      void BlendLineDiagonal(sPixel col, OutputMatrix output);
      void BlendCorner(sPixel col, OutputMatrix output);
    }

    private class Scaler_2X : IScaler {
      private const int _SCALE = 2;

      public int Scale() {
        return _SCALE;
      }

      public void BlendLineShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(_SCALE - 1, 0), col);
        _AlphaBlend(3, 4, output.Reference(_SCALE - 1, 1), col);
      }

      public void BlendLineSteep(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(0, _SCALE - 1), col);
        _AlphaBlend(3, 4, output.Reference(1, _SCALE - 1), col);
      }

      public void BlendLineSteepAndShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(1, 0), col);
        _AlphaBlend(1, 4, output.Reference(0, 1), col);
        _AlphaBlend(5, 6, output.Reference(1, 1), col); //[!] fixes 7/8 used in xBR
      }

      public void BlendLineDiagonal(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 2, output.Reference(1, 1), col);
      }

      public void BlendCorner(sPixel col, OutputMatrix output) {
        //model a round corner
        _AlphaBlend(21, 100, output.Reference(1, 1), col); //exact: 1 - pi/4 = 0.2146018366
      }
    }

    private class Scaler_3X : IScaler {
      private const int _SCALE = 3;

      public int Scale() {
        return _SCALE;
      }

      public void BlendLineShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(_SCALE - 1, 0), col);
        _AlphaBlend(1, 4, output.Reference(_SCALE - 2, 2), col);
        _AlphaBlend(3, 4, output.Reference(_SCALE - 1, 1), col);
        output.Reference(_SCALE - 1, 2).SetPixel(col);
      }

      public void BlendLineSteep(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(0, _SCALE - 1), col);
        _AlphaBlend(1, 4, output.Reference(2, _SCALE - 2), col);
        _AlphaBlend(3, 4, output.Reference(1, _SCALE - 1), col);
        output.Reference(2, _SCALE - 1).SetPixel(col);
      }

      public void BlendLineSteepAndShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(2, 0), col);
        _AlphaBlend(1, 4, output.Reference(0, 2), col);
        _AlphaBlend(3, 4, output.Reference(2, 1), col);
        _AlphaBlend(3, 4, output.Reference(1, 2), col);
        output.Reference(2, 2).SetPixel(col);
      }

      public void BlendLineDiagonal(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 8, output.Reference(1, 2), col);
        _AlphaBlend(1, 8, output.Reference(2, 1), col);
        _AlphaBlend(7, 8, output.Reference(2, 2), col);
      }

      public void BlendCorner(sPixel col, OutputMatrix output) {
        //model a round corner
        _AlphaBlend(45, 100, output.Reference(2, 2), col); //exact: 0.4545939598
        //alphaBlend(14, 1000, out.ref(2, 1), col); //0.01413008627 -> negligable
        //alphaBlend(14, 1000, out.ref(1, 2), col); //0.01413008627
      }
    }

    private class Scaler_4X : IScaler {
      private const int _SCALE = 4;

      public int Scale() {
        return _SCALE;
      }

      public void BlendLineShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(_SCALE - 1, 0), col);
        _AlphaBlend(1, 4, output.Reference(_SCALE - 2, 2), col);
        _AlphaBlend(3, 4, output.Reference(_SCALE - 1, 1), col);
        _AlphaBlend(3, 4, output.Reference(_SCALE - 2, 3), col);
        output.Reference(_SCALE - 1, 2).SetPixel(col);
        output.Reference(_SCALE - 1, 3).SetPixel(col);
      }

      public void BlendLineSteep(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(0, _SCALE - 1), col);
        _AlphaBlend(1, 4, output.Reference(2, _SCALE - 2), col);
        _AlphaBlend(3, 4, output.Reference(1, _SCALE - 1), col);
        _AlphaBlend(3, 4, output.Reference(3, _SCALE - 2), col);
        output.Reference(2, _SCALE - 1).SetPixel(col);
        output.Reference(3, _SCALE - 1).SetPixel(col);
      }

      public void BlendLineSteepAndShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(3, 4, output.Reference(3, 1), col);
        _AlphaBlend(3, 4, output.Reference(1, 3), col);
        _AlphaBlend(1, 4, output.Reference(3, 0), col);
        _AlphaBlend(1, 4, output.Reference(0, 3), col);
        _AlphaBlend(1, 3, output.Reference(2, 2), col); //[!] fixes 1/4 used in xBR
        output.Reference(3, 3).SetPixel(col);
        output.Reference(3, 2).SetPixel(col);
        output.Reference(2, 3).SetPixel(col);
      }

      public void BlendLineDiagonal(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 2, output.Reference(_SCALE - 1, _SCALE / 2), col);
        _AlphaBlend(1, 2, output.Reference(_SCALE - 2, _SCALE / 2 + 1), col);
        output.Reference(_SCALE - 1, _SCALE - 1).SetPixel(col);
      }

      public void BlendCorner(sPixel col, OutputMatrix output) {
        //model a round corner
        _AlphaBlend(68, 100, output.Reference(3, 3), col); //exact: 0.6848532563
        _AlphaBlend(9, 100, output.Reference(3, 2), col); //0.08677704501
        _AlphaBlend(9, 100, output.Reference(2, 3), col); //0.08677704501
      }
    }

    private class Scaler_5X : IScaler {
      private const int _SCALE = 5;

      public int Scale() {
        return _SCALE;
      }

      public void BlendLineShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(_SCALE - 1, 0), col);
        _AlphaBlend(1, 4, output.Reference(_SCALE - 2, 2), col);
        _AlphaBlend(1, 4, output.Reference(_SCALE - 3, 4), col);
        _AlphaBlend(3, 4, output.Reference(_SCALE - 1, 1), col);
        _AlphaBlend(3, 4, output.Reference(_SCALE - 2, 3), col);
        output.Reference(_SCALE - 1, 2).SetPixel(col);
        output.Reference(_SCALE - 1, 3).SetPixel(col);
        output.Reference(_SCALE - 1, 4).SetPixel(col);
        output.Reference(_SCALE - 2, 4).SetPixel(col);
      }

      public void BlendLineSteep(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(0, _SCALE - 1), col);
        _AlphaBlend(1, 4, output.Reference(2, _SCALE - 2), col);
        _AlphaBlend(1, 4, output.Reference(4, _SCALE - 3), col);
        _AlphaBlend(3, 4, output.Reference(1, _SCALE - 1), col);
        _AlphaBlend(3, 4, output.Reference(3, _SCALE - 2), col);
        output.Reference(2, _SCALE - 1).SetPixel(col);
        output.Reference(3, _SCALE - 1).SetPixel(col);
        output.Reference(4, _SCALE - 1).SetPixel(col);
        output.Reference(4, _SCALE - 2).SetPixel(col);
      }

      public void BlendLineSteepAndShallow(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 4, output.Reference(0, _SCALE - 1), col);
        _AlphaBlend(1, 4, output.Reference(2, _SCALE - 2), col);
        _AlphaBlend(3, 4, output.Reference(1, _SCALE - 1), col);
        _AlphaBlend(1, 4, output.Reference(_SCALE - 1, 0), col);
        _AlphaBlend(1, 4, output.Reference(_SCALE - 2, 2), col);
        _AlphaBlend(3, 4, output.Reference(_SCALE - 1, 1), col);
        output.Reference(2, _SCALE - 1).SetPixel(col);
        output.Reference(3, _SCALE - 1).SetPixel(col);
        output.Reference(_SCALE - 1, 2).SetPixel(col);
        output.Reference(_SCALE - 1, 3).SetPixel(col);
        output.Reference(4, _SCALE - 1).SetPixel(col);
        _AlphaBlend(2, 3, output.Reference(3, 3), col);
      }

      public void BlendLineDiagonal(sPixel col, OutputMatrix output) {
        _AlphaBlend(1, 8, output.Reference(_SCALE - 1, _SCALE / 2), col);
        _AlphaBlend(1, 8, output.Reference(_SCALE - 2, _SCALE / 2 + 1), col);
        _AlphaBlend(1, 8, output.Reference(_SCALE - 3, _SCALE / 2 + 2), col);
        _AlphaBlend(7, 8, output.Reference(4, 3), col);
        _AlphaBlend(7, 8, output.Reference(3, 4), col);
        output.Reference(4, 4).SetPixel(col);
      }

      public void BlendCorner(sPixel col, OutputMatrix output) {
        //model a round corner
        _AlphaBlend(86, 100, output.Reference(4, 4), col); //exact: 0.8631434088
        _AlphaBlend(23, 100, output.Reference(4, 3), col); //0.2306749731
        _AlphaBlend(23, 100, output.Reference(3, 4), col); //0.2306749731
        //alphaBlend(8, 1000, out.ref(4, 2), col); //0.008384061834 -> negligable
        //alphaBlend(8, 1000, out.ref(2, 4), col); //0.008384061834
      }
    }

    private static readonly IScaler _SCALER2_X = new Scaler_2X();
    private static readonly IScaler _SCALER3_X = new Scaler_3X();
    private static readonly IScaler _SCALER4_X = new Scaler_4X();
    private static readonly IScaler _SCALER5_X = new Scaler_5X();
  }
}
