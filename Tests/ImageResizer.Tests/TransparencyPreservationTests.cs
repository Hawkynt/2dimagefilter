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

using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

using Classes;
using Classes.ScriptActions;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// A transparent pixel still has a colour, and sprite workflows depend on it: the background
  /// index of a paletted sprite has to survive a trip through the application or it has to be
  /// restored by hand afterwards. These pin that it does.
  /// </summary>
  [TestFixture]
  public class TransparencyPreservationTests {

    /// <summary>The colour hiding behind full transparency.</summary>
    private static readonly Color _BACKGROUND = Color.FromArgb(0, 255, 0, 255);

    private TemporaryDirectory _directory;

    [SetUp]
    public void SetUp() => this._directory = new TemporaryDirectory();

    [TearDown]
    public void TearDown() => this._directory.Dispose();

    #region helpers

    /// <summary>A sprite with a transparent-but-coloured border and an opaque middle.</summary>
    private static Bitmap _Sprite(int size = 16) {
      var result = new Bitmap(size, size, PixelFormat.Format32bppArgb);
      for (var y = 0; y < size; ++y)
      for (var x = 0; x < size; ++x) {
        var inside = x >= size / 4 && x < size * 3 / 4 && y >= size / 4 && y < size * 3 / 4;
        result.SetPixel(x, y, inside ? Color.FromArgb(255, 0, 128, 0) : _BACKGROUND);
      }

      return result;
    }

    /// <summary>An 8 bit paletted sprite whose first palette entry is the transparent background.</summary>
    private static Bitmap _PalettedSprite(int size = 16) {
      var result = new Bitmap(size, size, PixelFormat.Format8bppIndexed);

      var palette = result.Palette;
      palette.Entries[0] = _BACKGROUND;
      palette.Entries[1] = Color.FromArgb(255, 0, 128, 0);
      result.Palette = palette;

      var data = result.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
      try {
        var row = new byte[data.Stride];
        for (var y = 0; y < size; ++y) {
          for (var x = 0; x < size; ++x) {
            var inside = x >= size / 4 && x < size * 3 / 4 && y >= size / 4 && y < size * 3 / 4;
            row[x] = (byte)(inside ? 1 : 0);
          }

          Marshal.Copy(row, 0, data.Scan0 + y * data.Stride, data.Stride);
        }
      } finally {
        result.UnlockBits(data);
      }

      return result;
    }

    private static Color _TopLeft(Bitmap bitmap) => bitmap.GetPixel(0, 0);

    #endregion

    #region the copy itself

    [Test]
    public void CopyingAnImage_KeepsTheColourUnderTransparency() {
      using (var source = _Sprite())
      using (var copy = BitmapLoader.CopyPreservingTransparency(source)) {
        Assert.That(_TopLeft(copy).ToArgb(), Is.EqualTo(_BACKGROUND.ToArgb()));
      }
    }

    [Test]
    public void CopyingAnImage_KeepsTheOpaquePixels() {
      using (var source = _Sprite())
      using (var copy = BitmapLoader.CopyPreservingTransparency(source)) {
        Assert.That(copy.GetPixel(8, 8).ToArgb(), Is.EqualTo(Color.FromArgb(255, 0, 128, 0).ToArgb()));
      }
    }

    [Test]
    public void CopyingAnImage_ProducesAThirtyTwoBitArgbBitmap() {
      using (var source = _PalettedSprite())
      using (var copy = BitmapLoader.CopyPreservingTransparency(source)) {
        Assert.That(copy.PixelFormat, Is.EqualTo(PixelFormat.Format32bppArgb));
      }
    }

    [Test]
    public void CopyingAPalettedImage_KeepsItsTransparentPaletteEntry() {
      using (var source = _PalettedSprite())
      using (var copy = BitmapLoader.CopyPreservingTransparency(source)) {
        Assert.That(_TopLeft(copy).A, Is.Zero);
        Assert.That(_TopLeft(copy).ToArgb(), Is.EqualTo(_BACKGROUND.ToArgb()));
      }
    }

    [Test]
    public void CopyingKeepsTheDimensions() {
      using (var source = _Sprite(23))
      using (var copy = BitmapLoader.CopyPreservingTransparency(source)) {
        Assert.That(copy.Size, Is.EqualTo(new Size(23, 23)));
      }
    }

    #endregion

    #region through the loader

    [Test]
    public void LoadingASprite_KeepsTheColourUnderTransparency() {
      var file = this._directory.File("sprite.png");
      using (var sprite = _Sprite())
        sprite.Save(file, ImageFormat.Png);

      var command = new LoadFileCommand(file);
      command.Execute();

      Assert.That(_TopLeft(command.SourceImage).ToArgb(), Is.EqualTo(_BACKGROUND.ToArgb()));
    }

    [Test]
    public void LoadingAPalettedSprite_KeepsTheColourUnderTransparency() {
      var file = this._directory.File("paletted.png");
      using (var sprite = _PalettedSprite())
        sprite.Save(file, ImageFormat.Png);

      var command = new LoadFileCommand(file);
      command.Execute();

      Assert.That(_TopLeft(command.SourceImage).A, Is.Zero);
      Assert.That(_TopLeft(command.SourceImage).ToArgb(), Is.EqualTo(_BACKGROUND.ToArgb()));
    }

    [Test]
    public void LoadingStillReleasesTheFile() {
      var file = this._directory.File("sprite.png");
      using (var sprite = _Sprite())
        sprite.Save(file, ImageFormat.Png);

      new LoadFileCommand(file).Execute();

      Assert.That(() => System.IO.File.Delete(file), Throws.Nothing, "cloning must not keep the loader's handle open");
    }

    #endregion

    #region through a whole run

    /// <summary>
    /// Issue #32: the background colour of a paletted sprite has to come out the other side of a
    /// scale unchanged, or it has to be put back by hand before the sprite is usable again.
    /// </summary>
    [TestCase("Upscaler: XBR NoBlend 4x")]
    [TestCase("Upscaler: HQ 2x")]
    [TestCase("Upscaler: Scale 2x")]
    public void ScalingASprite_KeepsItsTransparentBackgroundColour(string filter) {
      var input = this._directory.File("sprite.png");
      var output = this._directory.File("out.png");
      using (var sprite = _Sprite())
        sprite.Save(input, ImageFormat.Png);

      var engine = new ScriptEngine();
      ScriptSerializer.LoadFromString(engine, $@"""/load"" ""{input}"" ""/resize"" ""auto"" ""{filter}"" ""/save"" ""{output}""");
      engine.RepeatActions();

      using (var stream = new System.IO.FileStream(output, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
      using (var result = new Bitmap(stream)) {
        Assert.That(_TopLeft(result).A, Is.Zero, "the background must stay transparent");
        Assert.That(_TopLeft(result).ToArgb(), Is.EqualTo(_BACKGROUND.ToArgb()), "and must keep the colour it had");
      }
    }

    [Test]
    public void ScalingASprite_LeavesExactlyOneColourUnderTheTransparency() {
      var input = this._directory.File("sprite.png");
      var output = this._directory.File("out.png");
      using (var sprite = _Sprite())
        sprite.Save(input, ImageFormat.Png);

      var engine = new ScriptEngine();
      ScriptSerializer.LoadFromString(engine, $@"""/load"" ""{input}"" ""/resize"" ""auto"" ""Upscaler: XBR NoBlend 4x"" ""/save"" ""{output}""");
      engine.RepeatActions();

      using (var stream = new System.IO.FileStream(output, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
      using (var result = new Bitmap(stream)) {
        var transparent = Enumerable
          .Range(0, result.Height)
          .SelectMany(y => Enumerable.Range(0, result.Width).Select(x => result.GetPixel(x, y)))
          .Where(colour => colour.A == 0)
          .Select(colour => Color.FromArgb(255, colour).ToArgb())
          .Distinct()
          .ToArray()
          ;

        Assert.That(transparent, Is.EqualTo(new[] { Color.FromArgb(255, _BACKGROUND).ToArgb() }));
      }
    }

    #endregion

  }
}
