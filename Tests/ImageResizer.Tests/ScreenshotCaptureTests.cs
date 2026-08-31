#region (c)2008-2026 Hawkynt
/*
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.
 */
#endregion

using System.Drawing;

using NUnit.Framework;

namespace ImageResizer.Tests {
  [TestFixture]
  public class ScreenshotCaptureTests {

    [Test]
    public void CreateDemoImage_IsDeterministic() {
      using (var first = ScreenshotCapture.CreateDemoImage())
      using (var second = ScreenshotCapture.CreateDemoImage()) {
        Assert.That(first.Size, Is.EqualTo(new Size(96, 64)));
        Assert.That(second.Size, Is.EqualTo(first.Size));

        for (var y = 0; y < first.Height; ++y)
          for (var x = 0; x < first.Width; ++x)
            Assert.That(second.GetPixel(x, y), Is.EqualTo(first.GetPixel(x, y)), $"Pixel mismatch at {x},{y}.");
      }
    }

    [Test]
    public void CreateDemoImage_ContainsRepresentativeHighContrastPatterns() {
      using (var image = ScreenshotCapture.CreateDemoImage()) {
        Assert.That(image.GetPixel(7, 39), Is.Not.EqualTo(image.GetPixel(8, 39)), "Checker pattern disappeared.");
        Assert.That(image.GetPixel(45, 29), Is.Not.EqualTo(image.GetPixel(44, 29)), "Hero face contrast disappeared.");
        Assert.That(image.GetPixel(72, 8), Is.Not.EqualTo(image.GetPixel(76, 12)), "Moon cutout disappeared.");
      }
    }
  }
}
