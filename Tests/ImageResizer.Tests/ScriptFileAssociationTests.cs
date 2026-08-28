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

using Classes;

using Microsoft.Win32;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers the <c>.irs</c> file association. Everything runs against a scratch key created for
  /// the test and deleted afterwards - the user's real associations are never touched.
  /// </summary>
  [TestFixture]
  public class ScriptFileAssociationTests {

    private const string _EXECUTABLE = @"C:\Program Files\ImageResizer\ImageResizer.exe";
    private const string _OTHER_EXECUTABLE = @"D:\elsewhere\ImageResizer.exe";

    private string _scratchPath;
    private RegistryKey _classes;

    [SetUp]
    public void SetUp() {
      this._scratchPath = @"Software\ImageResizer.Tests\" + Guid.NewGuid().ToString("N");
      this._classes = Registry.CurrentUser.CreateSubKey(this._scratchPath);
    }

    [TearDown]
    public void TearDown() {
      this._classes?.Dispose();
      try {
        Registry.CurrentUser.DeleteSubKeyTree(this._scratchPath, false);
      } catch (Exception) {
        // a leftover scratch key must not fail the run
      }
    }

    #region registering

    [Test]
    public void RegisteringMakesTheExtensionResolveToTheExecutable() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.True);
    }

    [Test]
    public void RegisteringPointsTheExtensionAtTheProgramIdentifier() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      using (var extension = this._classes.OpenSubKey(ScriptSerializer.DEFAULT_FILE_EXTENSION))
        Assert.That(extension.GetValue(null), Is.EqualTo(ScriptFileAssociation.PROGRAM_IDENTIFIER));
    }

    [Test]
    public void RegisteringWritesTheOpenCommandWithAQuotedPlaceholder() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      using (var command = this._classes.OpenSubKey(ScriptFileAssociation.PROGRAM_IDENTIFIER + @"\shell\open\command"))
        Assert.That(command.GetValue(null), Is.EqualTo("\"" + _EXECUTABLE + "\" \"%1\""));
    }

    [Test]
    public void TheCommandQuotesTheFile_SoAPathWithSpacesSurvives()
      => Assert.That(ScriptFileAssociation.BuildCommand(_EXECUTABLE), Does.EndWith("\"%1\""))
    ;

    [Test]
    public void RegisteringWritesADescriptionAndAnIcon() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      using (var program = this._classes.OpenSubKey(ScriptFileAssociation.PROGRAM_IDENTIFIER))
        Assert.That(program.GetValue(null), Is.EqualTo(ScriptFileAssociation.FILE_TYPE_DESCRIPTION));

      using (var icon = this._classes.OpenSubKey(ScriptFileAssociation.PROGRAM_IDENTIFIER + @"\DefaultIcon"))
        Assert.That(icon.GetValue(null), Is.EqualTo(_EXECUTABLE + ",0"));
    }

    [Test]
    public void RegisteringTwiceIsHarmless() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.True);
    }

    [Test]
    public void RegisteringAgainFromANewLocationRepointsTheAssociation() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      ScriptFileAssociation.Register(this._classes, _OTHER_EXECUTABLE);

      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _OTHER_EXECUTABLE), Is.True);
      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.False);
    }

    #endregion

    #region reporting state

    [Test]
    public void NothingIsRegisteredToBeginWith()
      => Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.False)
    ;

    [Test]
    public void AnAssociationToADifferentExecutable_IsNotOurs() {
      ScriptFileAssociation.Register(this._classes, _OTHER_EXECUTABLE);

      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.False);
    }

    [Test]
    public void AnExtensionOwnedBySomethingElse_IsNotOurs() {
      using (var extension = this._classes.CreateSubKey(ScriptSerializer.DEFAULT_FILE_EXTENSION))
        extension.SetValue(null, "SomeOtherEditor.Script");

      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.False);
    }

    [Test]
    public void TheExecutableIsMatchedCaseInsensitively() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE.ToUpperInvariant()), Is.True);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void AnEmptyExecutable_IsNeverRegistered(string executable)
      => Assert.That(ScriptFileAssociation.IsRegistered(this._classes, executable), Is.False)
    ;

    [Test]
    public void AMissingRegistryRoot_IsNotRegistered()
      => Assert.That(ScriptFileAssociation.IsRegistered(null, _EXECUTABLE), Is.False)
    ;

    #endregion

    #region removing

    [Test]
    public void UnregisteringRemovesTheAssociation() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);

      ScriptFileAssociation.Unregister(this._classes);

      Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.False);
      Assert.That(this._classes.OpenSubKey(ScriptFileAssociation.PROGRAM_IDENTIFIER), Is.Null);
    }

    [Test]
    public void UnregisteringWhenNothingIsRegistered_IsHarmless()
      => Assert.That(() => ScriptFileAssociation.Unregister(this._classes), Throws.Nothing)
    ;

    /// <summary>
    /// Another program may have taken the extension over since. Stealing it back on the way out
    /// would be worse than leaving it alone.
    /// </summary>
    [Test]
    public void UnregisteringLeavesAnExtensionOwnedBySomethingElseAlone() {
      ScriptFileAssociation.Register(this._classes, _EXECUTABLE);
      using (var extension = this._classes.CreateSubKey(ScriptSerializer.DEFAULT_FILE_EXTENSION))
        extension.SetValue(null, "SomeOtherEditor.Script");

      ScriptFileAssociation.Unregister(this._classes);

      using (var extension = this._classes.OpenSubKey(ScriptSerializer.DEFAULT_FILE_EXTENSION))
        Assert.That(extension?.GetValue(null), Is.EqualTo("SomeOtherEditor.Script"));
    }

    [Test]
    public void RegisteringAndRemovingRoundTrips() {
      for (var i = 0; i < 3; ++i) {
        ScriptFileAssociation.Register(this._classes, _EXECUTABLE);
        Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.True);

        ScriptFileAssociation.Unregister(this._classes);
        Assert.That(ScriptFileAssociation.IsRegistered(this._classes, _EXECUTABLE), Is.False);
      }
    }

    #endregion

  }
}
