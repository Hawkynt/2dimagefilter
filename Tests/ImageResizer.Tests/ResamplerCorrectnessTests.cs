#region (c)2008-2026 Hawkynt
/*
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

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
using System.Drawing;
using System.Drawing.Extensions.ColorProcessing.Resizing;
using System.Linq;

using Classes;
using Classes.ImageManipulators;
using Classes.ScriptActions;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Properties every resampler has to satisfy, checked against the whole registry rather than
  /// against stored reference images - so a new algorithm is covered the moment it is registered.
  /// </summary>
  [TestFixture]
  public class ResamplerCorrectnessTests {

    /// <summary>Every entry that takes an explicit target width and height.</summary>
    private static IEnumerable<TestCaseData> Resamplers
      => SupportedManipulators.MANIPULATORS
        .Where(pair => pair.Value.SupportsWidth && pair.Value.SupportsHeight)
        .Select(pair => new TestCaseData(pair.Key).SetName("{m}(" + pair.Key.Replace(".", "_") + ")"))
    ;

    /// <summary>
    /// Runs a named resampler over a bitmap.
    /// </summary>
    /// <param name="filterName">The registered name.</param>
    /// <param name="source">The source bitmap.</param>
    /// <param name="width">The target width.</param>
    /// <param name="height">The target height.</param>
    /// <returns>The result; the caller owns it.</returns>
    private static Bitmap _Resample(string filterName, Bitmap source, int width, int height) {
      var command = new ResizeCommand(
        false,
        SupportedManipulators.MANIPULATORS.First(pair => pair.Key == filterName).Value,
        (ushort)width, (ushort)height, 0, false,
        OutOfBoundsMode.ConstantExtension, OutOfBoundsMode.ConstantExtension,
        1, true, true, 1f
      ) {
        SourceImage = source,
      };

      command.Execute();
      return command.TargetImage;
    }

    /// <summary>A bitmap filled with one colour throughout.</summary>
    private static Bitmap _Flat(int size, Color colour) {
      var result = new Bitmap(size, size);
      using (var graphics = Graphics.FromImage(result))
      using (var brush = new SolidBrush(colour))
        graphics.FillRectangle(brush, 0, 0, size, size);

      return result;
    }

    /// <summary>Reports the darkest and brightest red channel value in a bitmap.</summary>
    private static void _RedRange(Bitmap bitmap, out int minimum, out int maximum) {
      minimum = int.MaxValue;
      maximum = int.MinValue;
      for (var y = 0; y < bitmap.Height; ++y)
      for (var x = 0; x < bitmap.Width; ++x) {
        var value = bitmap.GetPixel(x, y).R;
        if (value < minimum)
          minimum = value;

        if (value > maximum)
          maximum = value;
      }
    }

    /// <summary>
    /// Resampling a uniform image can only ever produce that same uniform image - every kernel
    /// sums to one over a constant signal. A deviation means the weights are not normalised or
    /// that samples are being taken from outside the source, which is what produced the one pixel
    /// halo around every GDI+ result.
    /// </summary>
    [TestCaseSource(nameof(Resamplers))]
    public void AUniformImage_SurvivesDownscalingUnchanged(string filterName) {
      using (var source = _Flat(64, Color.FromArgb(255, 128, 128, 128)))
      using (var result = _Resample(filterName, source, 40, 40)) {
        _RedRange(result, out var minimum, out var maximum);

        Assert.That(minimum, Is.EqualTo(128).Within(1), filterName);
        Assert.That(maximum, Is.EqualTo(128).Within(1), filterName);
      }
    }

    [TestCaseSource(nameof(Resamplers))]
    public void AUniformImage_SurvivesUpscalingUnchanged(string filterName) {
      using (var source = _Flat(32, Color.FromArgb(255, 128, 128, 128)))
      using (var result = _Resample(filterName, source, 96, 96)) {
        _RedRange(result, out var minimum, out var maximum);

        Assert.That(minimum, Is.EqualTo(128).Within(1), filterName);
        Assert.That(maximum, Is.EqualTo(128).Within(1), filterName);
      }
    }

    [TestCaseSource(nameof(Resamplers))]
    public void EveryResampler_HitsTheRequestedSize(string filterName) {
      using (var source = _Flat(32, Color.Gray))
      using (var result = _Resample(filterName, source, 45, 21)) {
        Assert.That(result.Width, Is.EqualTo(45), filterName);
        Assert.That(result.Height, Is.EqualTo(21), filterName);
      }
    }

    /// <summary>
    /// Regression: the GDI+ wrappers drew into <c>-1,-1,W+1,H+1</c> to hide an edge artefact,
    /// which offset and stretched the image. A one-to-one nearest-neighbour resample is the
    /// cheapest way to see it - it has to be the identity.
    /// </summary>
    [Test]
    public void OneToOneNearestNeighbour_ReturnsTheSourceUnchanged() {
      using (var source = TestBitmaps.Create(48, 48))
      using (var result = _Resample("Resampler: NearestNeighbor <GDI+>", source, 48, 48)) {
        var differing = 0;
        for (var y = 0; y < source.Height; ++y)
        for (var x = 0; x < source.Width; ++x)
          if (source.GetPixel(x, y).ToArgb() != result.GetPixel(x, y).ToArgb())
            ++differing;

        Assert.That(differing, Is.Zero, "resampling to the same size must not move or rescale anything");
      }
    }

    /// <summary>
    /// Issue #17: the Kopf-Lischinski depixelizer is implemented upstream but the discovery that
    /// builds the scaler registry never picked it up, so it was registered by hand. These pin
    /// that it is reachable and that it actually depixelizes.
    /// </summary>
    [Test]
    public void KopfLischinski_IsRegistered()
      => Assert.That(SupportedManipulators.MANIPULATORS.Any(pair => pair.Key == "Resampler: Kopf-Lischinski"), Is.True)
    ;

    [Test]
    public void KopfLischinski_ReconstructsRatherThanReplicating() {
      using (var source = _PixelArt())
      using (var depixelized = _Resample("Resampler: Kopf-Lischinski", source, 64, 64))
      using (var replicated = _Resample("Resampler: Nearest Neighbor", source, 64, 64)) {
        var differing = 0;
        for (var y = 0; y < 64; ++y)
        for (var x = 0; x < 64; ++x)
          if (depixelized.GetPixel(x, y).ToArgb() != replicated.GetPixel(x, y).ToArgb())
            ++differing;

        Assert.That(differing, Is.GreaterThan(0), "a depixelizer that matches nearest neighbour is not depixelizing");
      }
    }

    [Test]
    public void KopfLischinski_KeepsTheColoursItWasGiven() {
      using (var source = _PixelArt())
      using (var result = _Resample("Resampler: Kopf-Lischinski", source, 64, 64)) {
        // the interior of a solid block must still be that block's colour
        Assert.That(result.GetPixel(32, 32).A, Is.GreaterThan(0));
      }
    }

    /// <summary>A small two-colour glyph - the kind of input depixelization exists for.</summary>
    private static Bitmap _PixelArt(int size = 8) {
      var result = new Bitmap(size, size);
      for (var y = 0; y < size; ++y)
      for (var x = 0; x < size; ++x) {
        var inside = System.Math.Abs(x - size / 2) + System.Math.Abs(y - size / 2) < size / 2;
        result.SetPixel(x, y, inside ? Color.FromArgb(255, 200, 30, 30) : Color.FromArgb(255, 255, 240, 220));
      }

      return result;
    }

    /// <summary>
    /// The halo the GDI+ workaround was hiding: interpolating at an edge pulled in the transparent
    /// surround, leaving a bright border on an otherwise uniform image.
    /// </summary>
    [TestCase("Resampler: Bicubic <GDI+>")]
    [TestCase("Resampler: HighQualityBicubic <GDI+>")]
    [TestCase("Resampler: HighQualityBilinear <GDI+>")]
    [TestCase("Resampler: Bilinear <GDI+>")]
    [TestCase("Resampler: NearestNeighbor <GDI+>")]
    public void GdiPlusResamplers_LeaveNoBorderHalo(string filterName) {
      using (var source = _Flat(64, Color.FromArgb(255, 128, 128, 128)))
      using (var result = _Resample(filterName, source, 100, 100)) {
        var border = new List<int>();
        for (var x = 0; x < result.Width; ++x) {
          border.Add(result.GetPixel(x, 0).R);
          border.Add(result.GetPixel(x, result.Height - 1).R);
        }

        for (var y = 0; y < result.Height; ++y) {
          border.Add(result.GetPixel(0, y).R);
          border.Add(result.GetPixel(result.Width - 1, y).R);
        }

        Assert.That(border, Is.All.EqualTo(128), filterName);
      }
    }
  }
}
