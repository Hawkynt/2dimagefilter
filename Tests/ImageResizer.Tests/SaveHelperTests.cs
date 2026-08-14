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

using System.Drawing.Imaging;
using System.IO;

using Classes;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers <see cref="CLI.SaveHelper"/>: the extension-to-encoder mapping and the failure codes
  /// it reports instead of throwing.
  /// </summary>
  [TestFixture]
  public class SaveHelperTests {

    private TemporaryDirectory _directory;

    [SetUp]
    public void SetUp() => this._directory = new TemporaryDirectory();

    [TearDown]
    public void TearDown() => this._directory.Dispose();

    [TestCase("out.png", "Png")]
    [TestCase("out.bmp", "Bmp")]
    [TestCase("out.gif", "Gif")]
    [TestCase("out.tif", "Tiff")]
    [TestCase("out.jpg", "Jpeg")]
    [TestCase("out.jpeg", "Jpeg")]
    public void Extension_PicksTheEncoder(string fileName, string expectedFormat) {
      var file = this._directory.File(fileName);

      using (var bitmap = TestBitmaps.Create())
        Assert.That(CLI.SaveHelper(file, bitmap), Is.EqualTo(CLIExitCode.OK));

      Assert.That(TestBitmaps.RawFormatOf(file), Is.EqualTo(typeof(ImageFormat).GetProperty(expectedFormat).GetValue(null)));
    }

    [TestCase("out.PNG")]
    [TestCase("out.JPG")]
    public void ExtensionMatching_IsCaseInsensitive(string fileName) {
      var file = this._directory.File(fileName);

      using (var bitmap = TestBitmaps.Create())
        Assert.That(CLI.SaveHelper(file, bitmap), Is.EqualTo(CLIExitCode.OK));
    }

    [TestCase("out.xyz")]
    [TestCase("out")]
    public void UnknownOrMissingExtension_FallsBackToPng(string fileName) {
      var file = this._directory.File(fileName);

      using (var bitmap = TestBitmaps.Create())
        CLI.SaveHelper(file, bitmap);

      Assert.That(TestBitmaps.RawFormatOf(file), Is.EqualTo(ImageFormat.Png));
    }

    [Test]
    public void NothingToSave_IsReportedNotThrown()
      => Assert.That(CLI.SaveHelper(this._directory.File("out.png"), null), Is.EqualTo(CLIExitCode.NothingToSave))
    ;

    [Test]
    public void AnUnwritableTarget_IsReportedNotThrown() {
      // a directory can never be opened as a file
      var target = this._directory.File("subdirectory");
      Directory.CreateDirectory(target);

      using (var bitmap = TestBitmaps.Create())
        Assert.That(CLI.SaveHelper(target, bitmap), Is.EqualTo(CLIExitCode.ExceptionDuringImageWrite));
    }

    [Test]
    public void SavingTwiceToTheSamePath_Succeeds() {
      var file = this._directory.File("out.png");

      using (var bitmap = TestBitmaps.Create(5, 5))
        CLI.SaveHelper(file, bitmap);

      using (var bitmap = TestBitmaps.Create(10, 10))
        Assert.That(CLI.SaveHelper(file, bitmap), Is.EqualTo(CLIExitCode.OK));

      Assert.That(TestBitmaps.SizeOf(file).Width, Is.EqualTo(10));
    }

    [Test]
    public void ASuccessfulSave_LeavesOnlyTheTargetBehind() {
      var file = this._directory.File("out.png");

      using (var bitmap = TestBitmaps.Create())
        CLI.SaveHelper(file, bitmap);

      Assert.That(Directory.GetFiles(this._directory.Path), Is.EqualTo(new[] { file }), "the atomic-save temp file must be gone");
    }
  }
}
