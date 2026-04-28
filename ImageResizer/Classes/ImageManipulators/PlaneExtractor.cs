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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;

using Hawkynt.ColorProcessing;
using Hawkynt.Drawing;

namespace Classes.ImageManipulators {
  /// <summary>
  /// Maps each source pixel through a projector <c>Color → byte</c> and writes a greyscale
  /// 32bpp ARGB bitmap. The actual projector is wired by <c>SupportedManipulators</c> from
  /// <c>UpstreamPipeline.PlaneExtractors()</c>; this adapter just walks the pixels.
  /// </summary>
  [Description("Color component extractors")]
  internal class PlaneExtractor : IImageManipulator {
    private readonly Func<Color, byte> _projector;

    #region Implementation of IImageManipulator
    public bool SupportsWidth => false;
    public bool SupportsHeight => false;
    public bool SupportsRepetitionCount => false;
    public bool SupportsGridCentering => false;
    public bool ChangesResolution => false;
    public bool SupportsThresholds => false;
    public bool SupportsRadius => false;
    public string Description { get; }

    // Plane extractors are pure colour-space projections with no tunable parameters.
    public IReadOnlyList<ParameterDescriptor> Parameters => ImageManipulatorDefaults.EmptyParameters;
    public IImageManipulator CreateWith(IReadOnlyDictionary<string, object> values) => this;

    #endregion

    public Bitmap Apply(Bitmap source) {
      Contract.Requires(source != null);
      var width = source.Width;
      var height = source.Height;
      var result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
      using (var src = source.Lock(ImageLockMode.ReadOnly))
      using (var dst = result.Lock(ImageLockMode.WriteOnly)) {
        for (var y = 0; y < height; ++y)
        for (var x = 0; x < width; ++x) {
          var grey = this._projector(src[x, y]);
          dst[x, y] = Color.FromArgb(255, grey, grey, grey);
        }
      }
      return result;
    }

    public PlaneExtractor(Func<Color, byte> projector, string description) {
      Contract.Requires(projector != null);
      this._projector = projector;
      this.Description = description;
    }
  }
}
