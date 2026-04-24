#region (c)2008-2026 Hawkynt
/*
 *  cImage
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
using System.ComponentModel;
using System.Drawing;

using Imager;

namespace Classes.ImageManipulators {
  /// <summary>
  /// Wraps a NuGet-provided bitmap operation that does not take user-supplied dimensions —
  /// e.g. a rescaler (fixed integer factor) or a same-size filter. Rescalers opt into the
  /// threshold-aware ctor: the <c>useThresholds</c> flag maps to upstream
  /// <c>ScalerQuality.Fast</c> (exact comparison, fastest) vs <c>ScalerQuality.HighQuality</c>
  /// (Oklab distance-threshold comparison).
  /// </summary>
  [Description("Upstream bitmap pipeline (fixed output)")]
  internal class BitmapFixedAdapter : IImageManipulator {

    private readonly Func<Bitmap, bool, Bitmap> _operation;

    /// <summary>Filter/same-size or threshold-agnostic rescaler wiring.</summary>
    public BitmapFixedAdapter(string description, bool changesResolution, Func<Bitmap, Bitmap> operation)
      : this(description, changesResolution, supportsThresholds: false, (b, _) => operation(b)) { }

    /// <summary>Rescaler wiring that exposes an Oklab-distance threshold path.</summary>
    public BitmapFixedAdapter(string description, bool changesResolution, bool supportsThresholds, Func<Bitmap, bool, Bitmap> operation) {
      this.Description = description;
      this.ChangesResolution = changesResolution;
      this.SupportsThresholds = supportsThresholds;
      this._operation = operation;
    }

    #region Implementation of IImageManipulator
    public bool SupportsWidth => false;
    public bool SupportsHeight => false;
    public bool SupportsRepetitionCount => false;
    public bool SupportsGridCentering => false;
    public bool SupportsThresholds { get; }
    public bool SupportsRadius => false;
    public bool ChangesResolution { get; }
    public string Description { get; }
    #endregion

    public cImage Apply(cImage source) => this.Apply(source, useThresholds: false);

    public cImage Apply(cImage source, bool useThresholds) {
      using (var input = source.ToBitmap()) {
        using (var output = this._operation(input, useThresholds)) {
          return cImage.FromBitmap(output);
        }
      }
    }
  }
}
