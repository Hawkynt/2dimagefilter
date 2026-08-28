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

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers turning one command line that names a set of files into the command line that
  /// processes them one at a time. The file system is stubbed, so these say nothing about
  /// what is on disk - only about the rewriting.
  /// </summary>
  [TestFixture]
  public class WildcardExpansionTests {

    private static readonly Func<string, string[]> _THREE = _ => new[] { "a.png", "b.png", "c.png" };
    private static readonly Func<string, string[]> _NONE = _ => new string[0];

    #region recognising a pattern

    [TestCase("*.png", true)]
    [TestCase("sprite?.png", true)]
    [TestCase("in.png", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void APatternIsAFileNameWithAWildcard(string value, bool expected)
      => Assert.That(WildcardExpansion.ContainsWildcard(value), Is.EqualTo(expected))
    ;

    #endregion

    #region leaving things alone

    [Test]
    public void ACommandLineWithoutAPattern_IsUntouched() {
      var arguments = new[] { "/load", "in.png", "/resize", "auto", "HQ 2x", "/save", "out.png" };

      Assert.That(WildcardExpansion.Expand(arguments, _THREE), Is.SameAs(arguments));
    }

    [Test]
    public void AWildcardSomewhereOtherThanALoad_IsUntouched() {
      var arguments = new[] { "/load", "in.png", "/save", "out*.png" };

      Assert.That(WildcardExpansion.Expand(arguments, _THREE), Is.SameAs(arguments));
    }

    [TestCase(null)]
    public void NoArguments_AreUntouched(string[] arguments)
      => Assert.That(WildcardExpansion.Expand(arguments, _THREE), Is.Null)
    ;

    [Test]
    public void AnEmptyCommandLine_IsUntouched()
      => Assert.That(WildcardExpansion.Expand(new string[0], _THREE), Is.Empty)
    ;

    [Test]
    public void ATrailingLoadWithoutAFileName_IsUntouched() {
      var arguments = new[] { "/resize", "auto", "HQ 2x", "/load" };

      Assert.That(WildcardExpansion.Expand(arguments, _THREE), Is.SameAs(arguments));
    }

    #endregion

    #region expanding

    [Test]
    public void EachMatchGetsItsOwnRunOfTheWholeSegment() {
      var result = WildcardExpansion.Expand(new[] { "/load", "*.png", "/resize", "auto", "HQ 2x", "/save", "out.png" }, _THREE);

      Assert.That(result, Is.EqualTo(new[] {
        "/load", "a.png", "/resize", "auto", "HQ 2x", "/save", "out.png",
        "/load", "b.png", "/resize", "auto", "HQ 2x", "/save", "out.png",
        "/load", "c.png", "/resize", "auto", "HQ 2x", "/save", "out.png",
      }));
    }

    [Test]
    public void AStarInTheTargetBecomesTheSourcesBaseName() {
      var result = WildcardExpansion.Expand(new[] { "/load", "*.png", "/save", @"big\*.png" }, _THREE);

      Assert.That(result, Is.EqualTo(new[] {
        "/load", "a.png", "/save", @"big\a.png",
        "/load", "b.png", "/save", @"big\b.png",
        "/load", "c.png", "/save", @"big\c.png",
      }));
    }

    [Test]
    public void SeveralTargetsEachGetTheBaseName() {
      var result = WildcardExpansion.Expand(new[] { "/load", "*.png", "/save", "*.png", "/save", "*.bmp" }, _ => new[] { "a.png" });

      Assert.That(result, Is.EqualTo(new[] { "/load", "a.png", "/save", "a.png", "/save", "a.bmp" }));
    }

    [Test]
    public void ATargetWithoutAStar_IsLeftAsWritten() {
      var result = WildcardExpansion.Expand(new[] { "/load", "*.png", "/save", "fixed.png" }, _ => new[] { "a.png" });

      Assert.That(result, Is.EqualTo(new[] { "/load", "a.png", "/save", "fixed.png" }));
    }

    [Test]
    public void MatchingNothing_ProducesNothingToRun()
      => Assert.That(WildcardExpansion.Expand(new[] { "/load", "*.png", "/save", "out.png" }, _NONE), Is.Empty)
    ;

    [Test]
    public void ArgumentsBeforeThePattern_AreKept() {
      var result = WildcardExpansion.Expand(new[] { "/stdin", "/load", "*.png", "/save", "*.png" }, _ => new[] { "a.png" });

      Assert.That(result, Is.EqualTo(new[] { "/stdin", "/load", "a.png", "/save", "a.png" }));
    }

    [Test]
    public void APlainLoadAfterAPattern_StartsItsOwnSegment() {
      var result = WildcardExpansion.Expand(new[] { "/load", "*.png", "/save", "*.out", "/load", "z.png", "/save", "z.out" }, _ => new[] { "a.png", "b.png" });

      Assert.That(result, Is.EqualTo(new[] {
        "/load", "a.png", "/save", "a.out",
        "/load", "b.png", "/save", "b.out",
        "/load", "z.png", "/save", "z.out",
      }));
    }

    [Test]
    public void TwoPatterns_ExpandIndependently() {
      var matcher = new Func<string, string[]>(pattern => pattern.StartsWith("x") ? new[] { "x1.png" } : new[] { "y1.png", "y2.png" });

      var result = WildcardExpansion.Expand(new[] { "/load", "x*.png", "/save", "*.a", "/load", "y*.png", "/save", "*.b" }, matcher);

      Assert.That(result, Is.EqualTo(new[] {
        "/load", "x1.png", "/save", "x1.a",
        "/load", "y1.png", "/save", "y1.b",
        "/load", "y2.png", "/save", "y2.b",
      }));
    }

    [Test]
    public void TheLoadCommandIsMatchedCaseInsensitively() {
      var result = WildcardExpansion.Expand(new[] { "/LOAD", "*.png", "/save", "*.out" }, _ => new[] { "a.png" });

      Assert.That(result, Is.EqualTo(new[] { "/LOAD", "a.png", "/save", "a.out" }));
    }

    [Test]
    public void ABaseNameDropsTheExtension() {
      var result = WildcardExpansion.Expand(new[] { "/load", "*.bmp", "/save", "*.png" }, _ => new[] { @"C:\art\sprite.bmp" });

      Assert.That(result, Is.EqualTo(new[] { "/load", @"C:\art\sprite.bmp", "/save", "sprite.png" }));
    }

    #endregion

  }
}
