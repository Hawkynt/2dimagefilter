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

using System;
using System.Drawing;
using System.Drawing.Extensions.ColorProcessing.Resizing;
using System.IO;
using System.Linq;

using Classes;
using Classes.ScriptActions;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers the individual script actions - what each one reads, what it writes and how it fails.
  /// </summary>
  [TestFixture]
  public class ScriptActionTests {

    private TemporaryDirectory _directory;

    [SetUp]
    public void SetUp() => this._directory = new TemporaryDirectory();

    [TearDown]
    public void TearDown() => this._directory.Dispose();

    #region /load

    [Test]
    public void LoadFileCommand_ReadsTheFileIntoTheSourceSlot() {
      var file = TestBitmaps.WriteTo(this._directory.File("in.png"), 23, 17);
      var command = new LoadFileCommand(file);

      Assert.That(command.Execute(), Is.True);

      Assert.That(command.SourceImage.Width, Is.EqualTo(23));
      Assert.That(command.SourceImage.Height, Is.EqualTo(17));
    }

    [Test]
    public void LoadFileCommand_KeepsTheFileName() {
      var command = new LoadFileCommand("whatever.png");

      Assert.That(command.FileName, Is.EqualTo("whatever.png"));
    }

    [Test]
    public void LoadFileCommand_ReleasesTheFileHandle() {
      var file = TestBitmaps.WriteTo(this._directory.File("in.png"));
      new LoadFileCommand(file).Execute();

      Assert.That(() => File.Delete(file), Throws.Nothing, "the loader must not keep the file open");
    }

    [Test]
    public void LoadFileCommand_ReportsNoPoolKeyWithoutAPool() {
      var file = TestBitmaps.WriteTo(this._directory.File("in.png"));
      var command = new LoadFileCommand(file);

      command.Execute();

      Assert.That(command.PoolSourceKey, Is.Null);
    }

    [Test]
    public void LoadFileCommand_OnAMissingFile_Throws() {
      var command = new LoadFileCommand(this._directory.File("nope.png"));

      Assert.That(() => command.Execute(), Throws.InstanceOf<FileNotFoundException>());
    }

    [Test]
    public void LoadFileCommand_OnAFileThatIsNotAnImage_Throws() {
      var file = this._directory.File("garbage.png");
      File.WriteAllText(file, "this is not a PNG");
      var command = new LoadFileCommand(file);

      Assert.That(() => command.Execute(), Throws.InstanceOf<OutOfMemoryException>().Or.InstanceOf<ArgumentException>());
    }

    [TestCase(".png")]
    [TestCase(".bmp")]
    [TestCase(".gif")]
    [TestCase(".jpg")]
    [TestCase(".tif")]
    public void LoadFileCommand_ReadsEveryFormatTheSaverWrites(string extension) {
      var file = TestBitmaps.WriteTo(this._directory.File("in" + extension), 8, 8);
      var command = new LoadFileCommand(file);

      Assert.That(command.Execute(), Is.True);
      Assert.That(command.SourceImage.Width, Is.EqualTo(8));
    }

    #endregion

    #region /save

    [Test]
    public void SaveFileCommand_WritesTheTargetToDisk() {
      var file = this._directory.File("out.png");
      var command = new SaveFileCommand(file) { TargetImage = TestBitmaps.Create(9, 11) };

      Assert.That(command.Execute(), Is.True);

      Assert.That(TestBitmaps.SizeOf(file), Is.EqualTo(new Size(9, 11)));
    }

    [Test]
    public void SaveFileCommand_WithNothingInTheTarget_Throws() {
      var command = new SaveFileCommand(this._directory.File("out.png"));

      Assert.That(() => command.Execute(), Throws.InstanceOf<NullReferenceException>());
    }

    [Test]
    public void SaveFileCommand_DoesNotDisposeTheTargetItWrote() {
      var command = new SaveFileCommand(this._directory.File("out.png")) { TargetImage = TestBitmaps.Create() };

      command.Execute();

      Assert.That(() => command.TargetImage.Width, Throws.Nothing, "the engine still owns it");
    }

    [Test]
    public void SaveFileCommand_OverwritesAnExistingFile() {
      var file = TestBitmaps.WriteTo(this._directory.File("out.png"), 4, 4);
      var command = new SaveFileCommand(file) { TargetImage = TestBitmaps.Create(12, 12) };

      command.Execute();

      Assert.That(TestBitmaps.SizeOf(file), Is.EqualTo(new Size(12, 12)));
    }

    [Test]
    public void SaveFileCommand_LeavesNoTemporaryFilesBehind() {
      var file = this._directory.File("out.png");
      new SaveFileCommand(file) { TargetImage = TestBitmaps.Create() }.Execute();

      Assert.That(Directory.GetFiles(this._directory.Path), Is.EqualTo(new[] { file }));
    }

    #endregion

    #region transfer actions

    [Test]
    public void NullTransformCommand_CopiesTheSourceIntoTheTarget() {
      var source = TestBitmaps.Create(7, 5);
      var command = new NullTransformCommand { SourceImage = source };

      command.Execute();

      Assert.That(command.TargetImage, Is.Not.SameAs(source), "an alias would make the two slots share a lifetime");
      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(7, 5)));
    }

    [Test]
    public void NullTransformCommand_WithoutASource_ProducesNoTarget() {
      var command = new NullTransformCommand();

      command.Execute();

      Assert.That(command.TargetImage, Is.Null);
    }

    [Test]
    public void TargetToSourceCommand_MovesTheTargetIntoTheSource() {
      var target = TestBitmaps.Create(6, 6);
      var command = new TargetToSourceCommand { TargetImage = target };

      command.Execute();

      Assert.That(command.SourceImage.Size, Is.EqualTo(new Size(6, 6)));
      Assert.That(command.SourceImage, Is.Not.SameAs(target));
      Assert.That(command.TargetImage, Is.Null);
    }

    [Test]
    public void TargetToSourceCommand_WithoutATarget_ClearsTheSource() {
      var command = new TargetToSourceCommand { SourceImage = TestBitmaps.Create() };

      command.Execute();

      Assert.That(command.SourceImage, Is.Null);
    }

    #endregion

    #region /resize

    /// <summary>
    /// Builds a resize command the way the parser would, against a resampler that honours
    /// explicit width and height.
    /// </summary>
    private static ResizeCommand _Resize(ushort width, ushort height, ushort percentage, bool maintainAspect)
      => new ResizeCommand(
        false,
        SupportedManipulators.MANIPULATORS.First(m => m.Key == "Resampler: Bicubic").Value,
        width, height, percentage, maintainAspect,
        OutOfBoundsMode.ConstantExtension, OutOfBoundsMode.ConstantExtension,
        1, true, true, 1f
      )
    ;

    [Test]
    public void Resize_ToExplicitDimensions_HitsThemExactly() {
      var command = _Resize(40, 30, 0, false);
      command.SourceImage = TestBitmaps.Create(20, 20);

      command.Execute();

      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(40, 30)));
    }

    [TestCase(100, 20, 20)]
    [TestCase(200, 40, 40)]
    [TestCase(50, 10, 10)]
    [TestCase(325, 65, 65)]
    public void Resize_ByPercentage_ScalesBothAxes(int percentage, int expectedWidth, int expectedHeight) {
      var command = _Resize(0, 0, (ushort)percentage, true);
      command.SourceImage = TestBitmaps.Create(20, 20);

      command.Execute();

      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(expectedWidth, expectedHeight)));
    }

    [Test]
    public void Resize_WidthOnly_DerivesTheHeightFromTheAspectRatio() {
      var command = _Resize(60, 0, 0, true);
      command.SourceImage = TestBitmaps.Create(30, 20);

      command.Execute();

      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(60, 40)));
    }

    [Test]
    public void Resize_HeightOnly_DerivesTheWidthFromTheAspectRatio() {
      var command = _Resize(0, 40, 0, true);
      command.SourceImage = TestBitmaps.Create(30, 20);

      command.Execute();

      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(60, 40)));
    }

    [Test]
    public void Resize_ToASinglePixel_Works() {
      var command = _Resize(1, 1, 0, false);
      command.SourceImage = TestBitmaps.Create(32, 32);

      command.Execute();

      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(1, 1)));
    }

    [Test]
    public void Resize_OfASinglePixelSource_Works() {
      var command = _Resize(8, 8, 0, false);
      command.SourceImage = TestBitmaps.Create(1, 1);

      command.Execute();

      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(8, 8)));
    }

    [Test]
    public void Resize_KeepsItsParameters() {
      var command = new ResizeCommand(
        false,
        SupportedManipulators.MANIPULATORS.First(m => m.Key == "Resampler: Bicubic").Value,
        1, 2, 3, true,
        OutOfBoundsMode.WrapAround, OutOfBoundsMode.HalfSampleSymmetric,
        7, false, false, 2.5f
      );

      Assert.That(command.Width, Is.EqualTo(1));
      Assert.That(command.Height, Is.EqualTo(2));
      Assert.That(command.Percentage, Is.EqualTo(3));
      Assert.That(command.MaintainAspect, Is.True);
      Assert.That(command.HorizontalBph, Is.EqualTo(OutOfBoundsMode.WrapAround));
      Assert.That(command.VerticalBph, Is.EqualTo(OutOfBoundsMode.HalfSampleSymmetric));
      Assert.That(command.Count, Is.EqualTo(7));
      Assert.That(command.UseThresholds, Is.False);
      Assert.That(command.UseCenteredGrid, Is.False);
      Assert.That(command.Radius, Is.EqualTo(2.5f));
    }

    [Test]
    public void FixedFactorUpscaler_IgnoresRequestedDimensions() {
      var command = new ResizeCommand(
        false,
        SupportedManipulators.MANIPULATORS.First(m => m.Key == "Upscaler: HQ 2x").Value,
        999, 999, 0, false,
        OutOfBoundsMode.ConstantExtension, OutOfBoundsMode.ConstantExtension,
        1, true, true, 1f
      );
      command.SourceImage = TestBitmaps.Create(16, 16);

      command.Execute();

      Assert.That(command.TargetImage.Size, Is.EqualTo(new Size(32, 32)), "the factor is the algorithm's, not the caller's");
    }

    #endregion

  }
}
