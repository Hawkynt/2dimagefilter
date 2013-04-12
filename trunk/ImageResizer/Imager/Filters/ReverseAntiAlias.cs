
using System;

namespace Imager.Filters {
  internal static class ReverseAntiAlias {
    /// <summary>
    /// Christoph Feck's (christoph@maxiom.de) Reverse Anti-Alias filter
    /// </summary>
    /// <param name="sourceImage"></param>
    /// <param name="srcX"></param>
    /// <param name="srcY"></param>
    /// <param name="targetImage"></param>
    /// <param name="tgtX"></param>
    /// <param name="tgtY"></param>
    public static void Process(cImage sourceImage, int srcX, int srcY, cImage targetImage, int tgtX, int tgtY) {

      var B1 = sourceImage[srcX + 0, srcY - 2];
      var B = sourceImage[srcX + 0, srcY - 1];
      var D = sourceImage[srcX - 1, srcY + 0];
      var E = sourceImage[srcX + 0, srcY + 0];
      var F = sourceImage[srcX + 1, srcY + 0];
      var H = sourceImage[srcX + 0, srcY + 1];
      var H5 = sourceImage[srcX + 0, srcY + 2];
      var D0 = sourceImage[srcX - 2, srcY + 0];
      var F4 = sourceImage[srcX + 2, srcY + 0];

      var redPart = _ReverseAntiAlias(B1.Red, B.Red, D.Red, E.Red, F.Red, H.Red, H5.Red, D0.Red, F4.Red);
      var greenPart = _ReverseAntiAlias(B1.Green, B.Green, D.Green, E.Green, F.Green, H.Green, H5.Green, D0.Green, F4.Green);
      var bluePart = _ReverseAntiAlias(B1.Blue, B.Blue, D.Blue, E.Blue, F.Blue, H.Blue, H5.Blue, D0.Blue, F4.Blue);
      var alphaPart = _ReverseAntiAlias(B1.Alpha, B.Alpha, D.Alpha, E.Alpha, F.Alpha, H.Alpha, H5.Alpha, D0.Alpha, F4.Alpha);

      targetImage[tgtX + 0, tgtY + 0] = sPixel.FromRGBA(redPart.Item1, greenPart.Item1, bluePart.Item1, alphaPart.Item1);
      targetImage[tgtX + 1, tgtY + 0] = sPixel.FromRGBA(redPart.Item2, greenPart.Item2, bluePart.Item2, alphaPart.Item2);
      targetImage[tgtX + 0, tgtY + 1] = sPixel.FromRGBA(redPart.Item3, greenPart.Item3, bluePart.Item3, alphaPart.Item3);
      targetImage[tgtX + 1, tgtY + 1] = sPixel.FromRGBA(redPart.Item4, greenPart.Item4, bluePart.Item4, alphaPart.Item4);
    }

    /// <summary>
    /// The internal function which is called for each channel separately.
    /// </summary>
    /// <param name="b1">The b1.</param>
    /// <param name="b">The B.</param>
    /// <param name="d">The D.</param>
    /// <param name="e">The E.</param>
    /// <param name="f">The F.</param>
    /// <param name="h">The H.</param>
    /// <param name="h5">The h5.</param>
    /// <param name="d0">The d0.</param>
    /// <param name="f4">The f4.</param>
    /// <returns></returns>
    private static Tuple<int, int, int, int> _ReverseAntiAlias(int b1, int b, int d, int e, int f, int h, int h5, int d0, int f4) {

      var n1 = b1;
      var n2 = b;
      var s = e;
      var n3 = h;
      var n4 = h5;
      var aa = n2 - n1;
      var bb = s - n2;
      var cc = n3 - s;
      var dd = n4 - n3;

      var tilt = (7 * (bb + cc) - 3 * (aa + dd)) / 16;

      var m = (s < 128) ? 2 * s : 2 * (byte.MaxValue - s);

      m = min(m, 2 * abs(bb));
      m = min(m, 2 * abs(cc));

      tilt = clamp(tilt, -m, m);

      var s1 = s + tilt / 2;
      var s0 = s1 - tilt;

      n1 = d0;
      n2 = d;
      s = s0;
      n3 = f;
      n4 = f4;
      aa = n2 - n1;
      bb = s - n2;
      cc = n3 - s;
      dd = n4 - n3;

      tilt = (7 * (bb + cc) - 3 * (aa + dd)) / 16;

      m = (s < 128) ? 2 * s : 2 * (byte.MaxValue - s);

      m = min(m, 2 * abs(bb));
      m = min(m, 2 * abs(cc));

      tilt = clamp(tilt, -m, m);

      var e1 = s + tilt / 2;
      var e0 = e1 - tilt;

      s = s1;
      bb = s - n2;
      cc = n3 - s;

      tilt = (7 * (bb + cc) - 3 * (aa + dd)) / 16;

      m = (s < 128) ? 2 * s : 2 * (byte.MaxValue - s);

      m = min(m, 2 * abs(bb));
      m = min(m, 2 * abs(cc));

      tilt = clamp(tilt, -m, m);

      var e3 = s + tilt / 2;
      var e2 = e3 - tilt;

      return (Tuple.Create(e0, e1, e2, e3));
    }

    private static int abs(int a) {
      return (a < 0 ? -a : a);
    }

    private static int min(int a, int b) {
      return (a < b ? a : b);
    }

    private static int clamp(int v, int min, int max) {
      return (v < min ? min : v > max ? max : v);
    }
  }
}
