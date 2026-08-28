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

using Classes;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers the relative-size arithmetic the command line's <c>/resize &lt;p&gt;%</c> and the
  /// window's percentage and scale-factor controls all share.
  /// </summary>
  [TestFixture]
  public class TargetDimensionsTests {

    #region scaling one dimension

    [TestCase(100, 1.0, 100)]
    [TestCase(100, 2.0, 200)]
    [TestCase(100, 0.5, 50)]
    [TestCase(16, 3.25, 52)]
    public void ScalingMultipliesTheLength(int source, double factor, int expected)
      => Assert.That(TargetDimensions.Scale(source, factor), Is.EqualTo(expected))
    ;

    /// <summary>
    /// Regression: a small source scaled far down used to round to zero, and a zero-sized bitmap
    /// cannot be allocated - the run ended in a runtime error instead of a very small image.
    /// </summary>
    [TestCase(16, 0.01)]
    [TestCase(16, 0.02)]
    [TestCase(1, 0.5)]
    [TestCase(3, 0.1)]
    public void ScalingNeverFallsBelowOnePixel(int source, double factor)
      => Assert.That(TargetDimensions.Scale(source, factor), Is.EqualTo(TargetDimensions.MINIMUM))
    ;

    [TestCase(0)]
    [TestCase(-5)]
    public void ASourceWithoutSize_YieldsTheMinimum(int source)
      => Assert.That(TargetDimensions.Scale(source, 2.0), Is.EqualTo(TargetDimensions.MINIMUM))
    ;

    [TestCase(0.0)]
    [TestCase(-1.0)]
    public void ANonPositiveFactor_YieldsTheMinimum(double factor)
      => Assert.That(TargetDimensions.Scale(100, factor), Is.EqualTo(TargetDimensions.MINIMUM))
    ;

    [Test]
    public void ScalingIsCappedAtTheMaximum()
      => Assert.That(TargetDimensions.Scale(1000, 1000, 65535), Is.EqualTo(65535))
    ;

    [Test]
    public void TheCapIsHonouredWhenTheCallerLowersIt()
      => Assert.That(TargetDimensions.Scale(100, 10, 500), Is.EqualTo(500))
    ;

    [Test]
    public void ScalingRoundsToTheNearestPixel() {
      Assert.That(TargetDimensions.Scale(10, 1.14), Is.EqualTo(11));
      Assert.That(TargetDimensions.Scale(10, 1.13), Is.EqualTo(11));
      Assert.That(TargetDimensions.Scale(10, 1.11), Is.EqualTo(11));
      Assert.That(TargetDimensions.Scale(10, 1.10), Is.EqualTo(11));
    }

    #endregion

    #region percentages

    [TestCase(100, 16, 16, 16, 16)]
    [TestCase(200, 16, 16, 32, 32)]
    [TestCase(325, 16, 16, 52, 52)]
    [TestCase(50, 16, 24, 8, 12)]
    public void APercentageScalesBothAxes(int percentage, int sourceWidth, int sourceHeight, int expectedWidth, int expectedHeight) {
      TargetDimensions.FromPercentage(sourceWidth, sourceHeight, percentage, out var width, out var height);

      Assert.That(width, Is.EqualTo(expectedWidth));
      Assert.That(height, Is.EqualTo(expectedHeight));
    }

    [Test]
    public void ATinyPercentageStillProducesAnImage() {
      TargetDimensions.FromPercentage(16, 16, 1, out var width, out var height);

      Assert.That(width, Is.EqualTo(1));
      Assert.That(height, Is.EqualTo(1));
    }

    [Test]
    public void APercentageRespectsTheCap() {
      TargetDimensions.FromPercentage(1000, 1000, 100000, out var width, out var height, 65535);

      Assert.That(width, Is.EqualTo(65535));
      Assert.That(height, Is.EqualTo(65535));
    }

    [Test]
    public void ANonSquareSourceKeepsItsRatioUnderAPercentage() {
      TargetDimensions.FromPercentage(400, 100, 50, out var width, out var height);

      Assert.That(width, Is.EqualTo(200));
      Assert.That(height, Is.EqualTo(50));
    }

    #endregion

    #region reading a percentage back

    [TestCase(100, 200, 200)]
    [TestCase(100, 100, 100)]
    [TestCase(100, 50, 50)]
    [TestCase(16, 52, 325)]
    public void ATargetIsExpressedAsAPercentageOfTheSource(int source, int target, double expected)
      => Assert.That(TargetDimensions.ToPercentage(source, target), Is.EqualTo(expected).Within(0.001))
    ;

    [TestCase(0)]
    [TestCase(-1)]
    public void ASourceWithoutSize_ReadsBackAsUnchanged(int source)
      => Assert.That(TargetDimensions.ToPercentage(source, 50), Is.EqualTo(100))
    ;

    [Test]
    public void ScalingAndReadingBack_AgreeWithEachOther() {
      TargetDimensions.FromPercentage(160, 160, 325, out var width, out _);

      Assert.That(TargetDimensions.ToPercentage(160, width), Is.EqualTo(325).Within(0.5));
    }

    #endregion

  }
}
