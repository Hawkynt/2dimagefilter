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
using System.Linq;

using Classes;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers the grouping behind the method dropdown's category selector. The dropdown itself is
  /// two lines of wiring; the decisions live here where they can be checked.
  /// </summary>
  [TestFixture]
  public class ManipulatorCategoriesTests {

    private sealed class FakeManipulator : IImageManipulator {
      public bool SupportsWidth => false;
      public bool SupportsHeight => false;
      public bool SupportsRepetitionCount => false;
      public bool SupportsGridCentering => false;
      public bool SupportsThresholds => false;
      public bool SupportsRadius => false;
      public bool ChangesResolution => false;
      public string Description => "fake";
      public IReadOnlyList<Hawkynt.ColorProcessing.ParameterDescriptor> Parameters => ImageManipulatorDefaults.EmptyParameters;
      public IImageManipulator CreateWith(IReadOnlyDictionary<string, object> values) => this;
    }

    private static KeyValuePair<string, IImageManipulator> _Entry(string key)
      => new KeyValuePair<string, IImageManipulator>(key, new FakeManipulator())
    ;

    private static KeyValuePair<string, IImageManipulator>[] _Registry()
      => new[] {
        _Entry("Filter: Blur"),
        _Entry("Resampler: Bicubic"),
        _Entry("Upscaler: HQ 2x"),
        _Entry("Upscaler: XBR 3x"),
      }
    ;

    #region category of a name

    [TestCase("Upscaler: HQ 2x", "Upscaler")]
    [TestCase("Resampler: Bicubic <GDI+>", "Resampler")]
    [TestCase("Plane: Oklab L", "Plane")]
    public void TheCategoryIsWhatPrecedesTheSeparator(string key, string expected)
      => Assert.That(ManipulatorCategories.GetCategory(key), Is.EqualTo(expected))
    ;

    [TestCase("NoCategoryHere")]
    [TestCase("")]
    [TestCase(null)]
    public void ANameWithoutASeparator_HasNoCategory(string key)
      => Assert.That(ManipulatorCategories.GetCategory(key), Is.Null)
    ;

    [Test]
    public void OnlyTheFirstSeparatorCounts()
      => Assert.That(ManipulatorCategories.GetCategory("Filter: Channel: Red"), Is.EqualTo("Filter"))
    ;

    #endregion

    #region listing

    [Test]
    public void TheListLeadsWithAll()
      => Assert.That(ManipulatorCategories.List(_Registry()).First(), Is.EqualTo(ManipulatorCategories.ALL))
    ;

    [Test]
    public void TheListIsDistinctAndAlphabetical()
      => Assert.That(ManipulatorCategories.List(_Registry()), Is.EqualTo(new[] { ManipulatorCategories.ALL, "Filter", "Resampler", "Upscaler" }))
    ;

    [Test]
    public void AnEmptyRegistry_StillOffersAll()
      => Assert.That(ManipulatorCategories.List(new KeyValuePair<string, IImageManipulator>[0]), Is.EqualTo(new[] { ManipulatorCategories.ALL }))
    ;

    [Test]
    public void ANullRegistry_StillOffersAll()
      => Assert.That(ManipulatorCategories.List(null), Is.EqualTo(new[] { ManipulatorCategories.ALL }))
    ;

    [Test]
    public void EntriesWithoutACategory_AreNotListed()
      => Assert.That(ManipulatorCategories.List(new[] { _Entry("Loose"), _Entry("Filter: Blur") }), Is.EqualTo(new[] { ManipulatorCategories.ALL, "Filter" }))
    ;

    #endregion

    #region filtering

    [Test]
    public void FilteringByACategory_KeepsOnlyItsEntries() {
      var result = ManipulatorCategories.Filter(_Registry(), "Upscaler");

      Assert.That(result.Select(pair => pair.Key), Is.EqualTo(new[] { "Upscaler: HQ 2x", "Upscaler: XBR 3x" }));
    }

    [Test]
    public void FilteringByAll_KeepsEverything()
      => Assert.That(ManipulatorCategories.Filter(_Registry(), ManipulatorCategories.ALL).Length, Is.EqualTo(4))
    ;

    [Test]
    public void FilteringByNull_KeepsEverything()
      => Assert.That(ManipulatorCategories.Filter(_Registry(), null).Length, Is.EqualTo(4))
    ;

    [Test]
    public void FilteringIsCaseInsensitive()
      => Assert.That(ManipulatorCategories.Filter(_Registry(), "upSCALer").Length, Is.EqualTo(2))
    ;

    [Test]
    public void FilteringByAnAbsentCategory_KeepsNothing()
      => Assert.That(ManipulatorCategories.Filter(_Registry(), "Downscaler"), Is.Empty)
    ;

    [Test]
    public void FilteringPreservesRegistryOrder() {
      var result = ManipulatorCategories.Filter(_Registry(), ManipulatorCategories.ALL);

      Assert.That(result.Select(pair => pair.Key), Is.EqualTo(_Registry().Select(pair => pair.Key)));
    }

    #endregion

    #region keeping the selection

    [Test]
    public void TheCurrentMethod_IsFoundInANarrowedList() {
      var registry = _Registry();
      var wanted = registry[3].Value;

      var narrowed = ManipulatorCategories.Filter(registry, "Upscaler");

      Assert.That(ManipulatorCategories.IndexOf(narrowed, wanted), Is.EqualTo(1));
    }

    [Test]
    public void AMethodOutsideTheNarrowedList_IsNotFound() {
      var registry = _Registry();

      var narrowed = ManipulatorCategories.Filter(registry, "Upscaler");

      Assert.That(ManipulatorCategories.IndexOf(narrowed, registry[0].Value), Is.EqualTo(-1));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void LocatingNothing_IsNotFound(bool nullList) {
      var registry = nullList ? null : _Registry();
      var manipulator = nullList ? new FakeManipulator() : null;

      Assert.That(ManipulatorCategories.IndexOf(registry, manipulator), Is.EqualTo(-1));
    }

    #endregion

    #region against the real registry

    [Test]
    public void EveryRealCategoryOffersAtLeastOneMethod() {
      var categories = ManipulatorCategories.List(SupportedManipulators.MANIPULATORS).Skip(1);

      foreach (var category in categories)
        Assert.That(ManipulatorCategories.Filter(SupportedManipulators.MANIPULATORS, category), Is.Not.Empty, category);
    }

    [Test]
    public void TheCategoriesPartitionTheRegistry() {
      var categories = ManipulatorCategories.List(SupportedManipulators.MANIPULATORS).Skip(1);
      var total = categories.Sum(category => ManipulatorCategories.Filter(SupportedManipulators.MANIPULATORS, category).Length);

      Assert.That(total, Is.EqualTo(SupportedManipulators.MANIPULATORS.Length), "every entry belongs to exactly one category");
    }

    [Test]
    public void NarrowingTheRealRegistry_IsAlwaysSmallerThanAll() {
      var upscalers = ManipulatorCategories.Filter(SupportedManipulators.MANIPULATORS, "Upscaler");

      Assert.That(upscalers.Length, Is.GreaterThan(0));
      Assert.That(upscalers.Length, Is.LessThan(SupportedManipulators.MANIPULATORS.Length));
    }

    #endregion

  }
}
