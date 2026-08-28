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
using System.Drawing.Extensions.ColorProcessing.Resizing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using Classes;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers <see cref="Config"/> persistence. The settings are static, so every test resets them.
  /// </summary>
  [TestFixture]
  public class ConfigTests {

    private TemporaryDirectory _directory;

    [SetUp]
    public void SetUp() {
      this._directory = new TemporaryDirectory();
      _Reset();
    }

    [TearDown]
    public void TearDown() {
      _Reset();
      this._directory.Dispose();
    }

    private static void _Reset() => Config.Reset();

    #region round trip

    [Test]
    public void EverySetting_SurvivesASaveLoadCycle() {
      var file = this._directory.File("config.xml");
      Config.LastLoadDirectory = @"C:\loads";
      Config.LastSaveDirectory = @"C:\saves";
      Config.SourceSizeMode = PictureBoxSizeMode.Zoom;
      Config.TargetSizeMode = PictureBoxSizeMode.CenterImage;
      Config.WindowBounds = new Rectangle(12, 34, 800, 600);
      Config.WindowState = FormWindowState.Maximized;
      Config.ResizeMethod = "Upscaler: HQ 2x";
      Config.MethodCategory = "Upscaler";
      Config.HorizontalBph = OutOfBoundsMode.WrapAround;
      Config.VerticalBph = OutOfBoundsMode.HalfSampleSymmetric;
      Config.UseThresholds = true;
      Config.UseCenteredGrid = false;
      Config.KeepAspect = true;
      Config.RepetitionCount = 4;
      Config.Radius = 2.5f;
      Config.Save(file);
      _Reset();

      Config.Load(file);

      Assert.That(Config.LastLoadDirectory, Is.EqualTo(@"C:\loads"));
      Assert.That(Config.LastSaveDirectory, Is.EqualTo(@"C:\saves"));
      Assert.That(Config.SourceSizeMode, Is.EqualTo(PictureBoxSizeMode.Zoom));
      Assert.That(Config.TargetSizeMode, Is.EqualTo(PictureBoxSizeMode.CenterImage));
      Assert.That(Config.WindowBounds, Is.EqualTo(new Rectangle(12, 34, 800, 600)));
      Assert.That(Config.WindowState, Is.EqualTo(FormWindowState.Maximized));
      Assert.That(Config.ResizeMethod, Is.EqualTo("Upscaler: HQ 2x"));
      Assert.That(Config.MethodCategory, Is.EqualTo("Upscaler"));
      Assert.That(Config.HorizontalBph, Is.EqualTo(OutOfBoundsMode.WrapAround));
      Assert.That(Config.VerticalBph, Is.EqualTo(OutOfBoundsMode.HalfSampleSymmetric));
      Assert.That(Config.UseThresholds, Is.True);
      Assert.That(Config.UseCenteredGrid, Is.False);
      Assert.That(Config.KeepAspect, Is.True);
      Assert.That(Config.RepetitionCount, Is.EqualTo(4));
      Assert.That(Config.Radius, Is.EqualTo(2.5f));
    }

    #endregion

    #region window placement

    [TestCase("12,34,800,600", true)]
    [TestCase("0,0,1,1", true)]
    [TestCase("12,34,0,600", false)]
    [TestCase("12,34,800,0", false)]
    [TestCase("12,34,800", false)]
    [TestCase("a,b,c,d", false)]
    [TestCase("", false)]
    public void OnlyAWindowWithExtentIsRestored(string value, bool expected) {
      var file = this._directory.File("config.xml");
      File.WriteAllText(file, $@"<Configuration><WindowBounds value=""{value}"" /></Configuration>");

      Config.Load(file);

      Assert.That(Config.WindowBounds != null, Is.EqualTo(expected));
    }

    [Test]
    public void AMinimizedWindowIsRememberedAsNormal() {
      var file = this._directory.File("config.xml");
      File.WriteAllText(file, @"<Configuration><WindowState value=""Minimized"" /></Configuration>");

      Config.Load(file);

      Assert.That(Config.WindowState, Is.EqualTo(FormWindowState.Normal), "nobody wants to reopen a minimized window");
    }

    [TestCase("RepetitionCount", "0")]
    [TestCase("RepetitionCount", "-1")]
    [TestCase("RepetitionCount", "many")]
    [TestCase("Radius", "0")]
    [TestCase("Radius", "-2")]
    [TestCase("Radius", "wide")]
    public void ANonsensicalNumberIsIgnored(string element, string value) {
      var file = this._directory.File("config.xml");
      File.WriteAllText(file, $@"<Configuration><{element} value=""{value}"" /></Configuration>");

      Config.Load(file);

      Assert.That(Config.RepetitionCount, Is.Null);
      Assert.That(Config.Radius, Is.Null);
    }

    [Test]
    public void RadiusIsWrittenAndReadInvariantOfTheCurrentCulture() {
      var file = this._directory.File("config.xml");
      var previous = System.Threading.Thread.CurrentThread.CurrentCulture;
      System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
      try {
        Config.Radius = 1.5f;
        Config.Save(file);
        _Reset();
        Config.Load(file);

        Assert.That(Config.Radius, Is.EqualTo(1.5f));
      } finally {
        System.Threading.Thread.CurrentThread.CurrentCulture = previous;
      }
    }

    [Test]
    public void UnsetSettings_SaveAndLoadAsUnset() {
      var file = this._directory.File("config.xml");

      Config.Save(file);
      Config.Load(file);

      Assert.That(Config.LastLoadDirectory, Is.Null);
      Assert.That(Config.SourceSizeMode, Is.Null);
    }

    [Test]
    public void Save_ProducesReadableXml() {
      var file = this._directory.File("config.xml");
      Config.LastSaveDirectory = @"C:\saves";

      Config.Save(file);

      Assert.That(File.ReadAllText(file), Does.Contain("Configuration").And.Contain(@"C:\saves"));
    }

    [Test]
    public void PathsWithXmlSpecialCharacters_SurviveTheRoundTrip() {
      var file = this._directory.File("config.xml");
      Config.LastLoadDirectory = @"C:\a&b\<c>\""d""";
      Config.Save(file);
      _Reset();

      Config.Load(file);

      Assert.That(Config.LastLoadDirectory, Is.EqualTo(@"C:\a&b\<c>\""d"""));
    }

    #endregion

    #region tolerated input

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void LoadingAnEmptyPath_IsIgnored(string path) {
      Assert.That(() => Config.Load(path), Throws.Nothing);
      Assert.That(Config.LastLoadDirectory, Is.Null);
    }

    [Test]
    public void LoadingAMissingFile_IsIgnored() {
      Assert.That(() => Config.Load(this._directory.File("absent.xml")), Throws.Nothing);
      Assert.That(Config.LastLoadDirectory, Is.Null);
    }

    [Test]
    public void AForeignRootElement_IsIgnored() {
      var file = this._directory.File("other.xml");
      File.WriteAllText(file, @"<SomethingElse><LastLoadDirectory value=""C:\x"" /></SomethingElse>");

      Config.Load(file);

      Assert.That(Config.LastLoadDirectory, Is.Null);
    }

    [Test]
    public void UnknownElements_AreSkippedWithoutLosingTheKnownOnes() {
      var file = this._directory.File("config.xml");
      File.WriteAllText(file, @"<Configuration><Bogus value=""1"" /><LastLoadDirectory value=""C:\x"" /></Configuration>");

      Config.Load(file);

      Assert.That(Config.LastLoadDirectory, Is.EqualTo(@"C:\x"));
    }

    [Test]
    public void ElementsWithoutAValueAttribute_AreSkipped() {
      var file = this._directory.File("config.xml");
      File.WriteAllText(file, @"<Configuration><LastLoadDirectory /></Configuration>");

      Config.Load(file);

      Assert.That(Config.LastLoadDirectory, Is.Null);
    }

    [Test]
    public void AnUnparseableSizeMode_LeavesTheSettingUnset() {
      var file = this._directory.File("config.xml");
      File.WriteAllText(file, @"<Configuration><SourceSizeMode value=""NotAMode"" /></Configuration>");

      Config.Load(file);

      Assert.That(Config.SourceSizeMode, Is.Null);
    }

    [Test]
    public void ElementNames_AreMatchedCaseInsensitively() {
      var file = this._directory.File("config.xml");
      File.WriteAllText(file, @"<configuration><lastloaddirectory value=""C:\x"" /></configuration>");

      Config.Load(file);

      Assert.That(Config.LastLoadDirectory, Is.EqualTo(@"C:\x"));
    }

    #endregion

    #region rejected input

    [Test]
    public void MalformedXml_Throws() {
      var file = this._directory.File("broken.xml");
      File.WriteAllText(file, "<Configuration><unclosed>");

      Assert.That(() => Config.Load(file), Throws.InstanceOf<XmlException>());
    }

    #endregion

  }
}
