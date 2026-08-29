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
using System.Collections.Generic;
using System.Linq;

namespace Classes {
  /// <summary>
  /// Splits the manipulator registry by the category each entry is registered under, so the
  /// method dropdown can show one group at a time instead of several hundred entries at once.
  /// <para>
  /// Categories are derived when the registry is built, never hard-coded here - a new one appears
  /// in the list as soon as something is registered under it.
  /// </para>
  /// </summary>
  internal static class ManipulatorCategories {

    /// <summary>
    /// The pseudo-category that selects everything.
    /// </summary>
    public const string ALL = "(all)";

    /// <summary>
    /// Gets the category an entry belongs to.
    /// </summary>
    /// <param name="key">The registered name, e.g. <c>"Upscaler: HQ 2x"</c>.</param>
    /// <returns>The category, or <c>null</c> when the name carries none.</returns>
    public static string GetCategory(string key) {
      if (key == null)
        return null;

      var index = key.IndexOf(ScriptSerializer.CATEGORY_SEPARATOR, StringComparison.Ordinal);
      return index < 0 ? null : key.Substring(0, index);
    }

    /// <summary>
    /// Lists the categories present in a registry, alphabetically, with <see cref="ALL"/> first.
    /// </summary>
    /// <param name="manipulators">The registry.</param>
    /// <returns>The category names.</returns>
    public static string[] List(KeyValuePair<string, IImageManipulator>[] manipulators) {
      var categories = (manipulators ?? new KeyValuePair<string, IImageManipulator>[0])
        .Select(pair => GetCategory(pair.Key))
        .Where(category => category != null)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(category => category, StringComparer.OrdinalIgnoreCase)
        ;

      return new[] { ALL }.Concat(categories).ToArray();
    }

    /// <summary>
    /// Narrows a registry to one category.
    /// </summary>
    /// <param name="manipulators">The registry.</param>
    /// <param name="category">The category, or <see cref="ALL"/>/<c>null</c> for everything.</param>
    /// <returns>The entries in that category, in registry order.</returns>
    public static KeyValuePair<string, IImageManipulator>[] Filter(KeyValuePair<string, IImageManipulator>[] manipulators, string category) {
      if (manipulators == null)
        return new KeyValuePair<string, IImageManipulator>[0];

      if (category == null || category == ALL)
        return manipulators;

      return manipulators
        .Where(pair => string.Equals(GetCategory(pair.Key), category, StringComparison.OrdinalIgnoreCase))
        .ToArray()
        ;
    }

    /// <summary>
    /// Finds an entry's position in a list, so a category change can keep the current selection
    /// rather than snapping the user back to the first method.
    /// </summary>
    /// <param name="manipulators">The list to search.</param>
    /// <param name="manipulator">The manipulator to locate.</param>
    /// <returns>Its index, or <c>-1</c> when the list does not contain it.</returns>
    public static int IndexOf(KeyValuePair<string, IImageManipulator>[] manipulators, IImageManipulator manipulator) {
      if (manipulators == null || manipulator == null)
        return -1;

      for (var i = 0; i < manipulators.Length; ++i)
        if (ReferenceEquals(manipulators[i].Value, manipulator))
          return i;

      return -1;
    }
  }
}
