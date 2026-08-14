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
using System.Linq;

using Classes;
using Classes.ImageManipulators;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Invariants of the manipulator registry. These are the assumptions the parser, the help text
  /// and the GUI dropdown all rest on; a new algorithm that breaks one of them breaks all three.
  /// </summary>
  [TestFixture]
  public class SupportedManipulatorsTests {

    /// <summary>Categories the registration code is allowed to produce.</summary>
    private static readonly string[] _KNOWN_CATEGORIES = {
      "Upscaler", "Downscaler", "Resampler", "Downsampler", "Filter", "Plane",
    };

    [Test]
    public void TheRegistryIsNotEmpty()
      => Assert.That(SupportedManipulators.MANIPULATORS, Is.Not.Empty)
    ;

    [Test]
    public void EveryEntryHasAManipulator() {
      var empty = SupportedManipulators.MANIPULATORS.Where(pair => pair.Value == null).Select(pair => pair.Key);

      Assert.That(empty, Is.Empty);
    }

    [Test]
    public void EveryKeyIsNonEmpty() {
      var blank = SupportedManipulators.MANIPULATORS.Where(pair => string.IsNullOrWhiteSpace(pair.Key));

      Assert.That(blank.Count(), Is.Zero);
    }

    [Test]
    public void KeysAreUniqueIgnoringCase() {
      var duplicates = SupportedManipulators.MANIPULATORS
        .GroupBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        ;

      Assert.That(duplicates, Is.Empty, "a duplicate key makes one of the two unreachable by name");
    }

    [Test]
    public void EveryKeyCarriesACategory() {
      var uncategorised = SupportedManipulators.MANIPULATORS
        .Where(pair => pair.Key.IndexOf(ScriptSerializer.CATEGORY_SEPARATOR, StringComparison.Ordinal) < 0)
        .Select(pair => pair.Key)
        ;

      Assert.That(uncategorised, Is.Empty);
    }

    [Test]
    public void EveryCategoryIsOneOfTheDocumentedOnes() {
      var unexpected = SupportedManipulators.MANIPULATORS
        .Select(pair => pair.Key.Substring(0, pair.Key.IndexOf(ScriptSerializer.CATEGORY_SEPARATOR, StringComparison.Ordinal)))
        .Distinct()
        .Where(category => !_KNOWN_CATEGORIES.Contains(category))
        ;

      Assert.That(unexpected, Is.Empty, "README and AGENTS.md list the categories - keep them in sync");
    }

    [Test]
    public void EveryEntryIsReachableByItsFullName() {
      var unreachable = SupportedManipulators.MANIPULATORS
        .Where(pair => !ReferenceEquals(ScriptSerializer.FindManipulator(SupportedManipulators.MANIPULATORS, pair.Key, out _), pair.Value))
        .Select(pair => pair.Key)
        ;

      Assert.That(unreachable, Is.Empty);
    }

    [Test]
    public void TheRegistryIsSortedByKey() {
      var keys = SupportedManipulators.MANIPULATORS.Select(pair => pair.Key).ToArray();

      Assert.That(keys, Is.Ordered.Using((System.Collections.IComparer)StringComparer.OrdinalIgnoreCase), "the help text and the dropdown present it as-is");
    }

    [Test]
    public void EveryEntryHasADescription() {
      var undescribed = SupportedManipulators.MANIPULATORS
        .Where(pair => string.IsNullOrWhiteSpace(pair.Value.Description))
        .Select(pair => pair.Key)
        ;

      Assert.That(undescribed, Is.Empty);
    }

    [Test]
    public void EveryEntryExposesAParameterList() {
      var missing = SupportedManipulators.MANIPULATORS
        .Where(pair => pair.Value.Parameters == null)
        .Select(pair => pair.Key)
        ;

      Assert.That(missing, Is.Empty, "callers enumerate it unconditionally");
    }

    [Test]
    public void UpscalersChangeResolution() {
      var offenders = SupportedManipulators.MANIPULATORS
        .Where(pair => pair.Key.StartsWith("Upscaler" + ScriptSerializer.CATEGORY_SEPARATOR, StringComparison.Ordinal))
        .Where(pair => !pair.Value.ChangesResolution)
        .Select(pair => pair.Key)
        ;

      Assert.That(offenders, Is.Empty);
    }

    [Test]
    public void FiltersDoNotChangeResolution() {
      var offenders = SupportedManipulators.MANIPULATORS
        .Where(pair => pair.Key.StartsWith("Filter" + ScriptSerializer.CATEGORY_SEPARATOR, StringComparison.Ordinal))
        .Where(pair => pair.Value.ChangesResolution)
        .Select(pair => pair.Key)
        ;

      Assert.That(offenders, Is.Empty);
    }

    [Test]
    public void ResamplersAcceptBothDimensions() {
      var offenders = SupportedManipulators.MANIPULATORS
        .Where(pair => pair.Value is BitmapResamplerAdapter)
        .Where(pair => !(pair.Value.SupportsWidth && pair.Value.SupportsHeight))
        .Select(pair => pair.Key)
        ;

      Assert.That(offenders, Is.Empty);
    }

    [Test]
    public void NonParametricManipulators_ReturnThemselvesFromCreateWith() {
      var manipulator = SupportedManipulators.MANIPULATORS.First(pair => pair.Value.Parameters.Count == 0).Value;

      Assert.That(manipulator.CreateWith(null), Is.SameAs(manipulator), "callers chain CreateWith unconditionally");
    }

    [Test]
    public void TheWellKnownAlgorithmsAreStillRegistered() {
      var keys = SupportedManipulators.MANIPULATORS.Select(pair => pair.Key).ToArray();

      Assert.That(keys, Does.Contain("Upscaler: HQ 2x"));
      Assert.That(keys, Does.Contain("Upscaler: XBR 3x"));
      Assert.That(keys, Does.Contain("Upscaler: Scale 2x"));
      Assert.That(keys, Does.Contain("Upscaler: Eagle"));
      Assert.That(keys, Does.Contain("Resampler: Bicubic"));
    }
  }
}
