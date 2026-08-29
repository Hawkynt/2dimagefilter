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

namespace Classes {
  /// <summary>
  /// Works out target dimensions from a relative size. Shared by the command line's
  /// <c>/resize &lt;p&gt;%</c> and the window's percentage and scale-factor controls so the two
  /// cannot drift apart.
  /// </summary>
  internal static class TargetDimensions {

    /// <summary>
    /// The smallest image that still exists. Rounding a small source down by a large factor
    /// otherwise lands on zero, and a zero-sized bitmap cannot be allocated - which used to end
    /// the run with a runtime error rather than a very small image.
    /// </summary>
    public const int MINIMUM = 1;

    /// <summary>
    /// Scales one dimension.
    /// </summary>
    /// <param name="source">The source length in pixels.</param>
    /// <param name="factor">The factor to apply; 1.0 keeps the length.</param>
    /// <param name="maximum">The largest length the caller can represent.</param>
    /// <returns>The scaled length, at least <see cref="MINIMUM"/> and at most <paramref name="maximum"/>.</returns>
    public static int Scale(int source, double factor, int maximum = ushort.MaxValue) {
      if (source <= 0 || factor <= 0)
        return MINIMUM;

      var scaled = Math.Round(source * factor, MidpointRounding.ToEven);
      if (scaled < MINIMUM)
        return MINIMUM;

      return scaled > maximum ? maximum : (int)scaled;
    }

    /// <summary>
    /// Scales both dimensions by a percentage.
    /// </summary>
    /// <param name="sourceWidth">The source width.</param>
    /// <param name="sourceHeight">The source height.</param>
    /// <param name="percentage">The percentage; 100 keeps the size.</param>
    /// <param name="width">Receives the target width.</param>
    /// <param name="height">Receives the target height.</param>
    /// <param name="maximum">The largest length the caller can represent.</param>
    public static void FromPercentage(int sourceWidth, int sourceHeight, double percentage, out int width, out int height, int maximum = ushort.MaxValue) {
      width = Scale(sourceWidth, percentage / 100d, maximum);
      height = Scale(sourceHeight, percentage / 100d, maximum);
    }

    /// <summary>
    /// Expresses a target size as a percentage of a source size, so the percentage control can
    /// follow along when width or height is edited directly.
    /// </summary>
    /// <param name="source">The source length.</param>
    /// <param name="target">The target length.</param>
    /// <returns>The percentage, or <c>100</c> when the source has no size.</returns>
    public static double ToPercentage(int source, int target)
      => source <= 0 ? 100d : target * 100d / source
    ;
  }
}
