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

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Extensions.ColorProcessing.Resizing;

using PaintDotNet.Effects;

namespace PixelArtScaling {
  /// <summary>
  /// Carries the user's settings between the dialog and the effect's render pipeline.
  /// </summary>
  internal sealed class PluginConfigToken : EffectConfigToken {
    public string FilterName { get; set; }
    public ScaleMode Mode { get; set; } = ScaleMode.Percent;

    /// <summary>Percentage for X-axis (and Y-axis when aspect-ratio is locked). 100 = identity.</summary>
    public int PercentX { get; set; } = 100;
    /// <summary>Percentage for Y-axis; used independently only when aspect-ratio is unlocked.</summary>
    public int PercentY { get; set; } = 100;

    /// <summary>Factor for X-axis (and Y-axis when aspect-ratio is locked). 1.0 = identity.</summary>
    public double FactorX { get; set; } = 1.0;
    /// <summary>Factor for Y-axis; used independently only when aspect-ratio is unlocked.</summary>
    public double FactorY { get; set; } = 1.0;

    /// <summary>Absolute target width in pixels. 0 = fall back to Percent/Factor.</summary>
    public int TargetWidth { get; set; }
    /// <summary>Absolute target height in pixels. 0 = fall back to Percent/Factor.</summary>
    public int TargetHeight { get; set; }
    public bool LockAspectRatio { get; set; } = true;

    /// <summary>Horizontal out-of-bounds handling mode (forwarded to upstream resamplers).</summary>
    public OutOfBoundsMode HorizontalOobMode { get; set; } = OutOfBoundsMode.ConstantExtension;
    /// <summary>Vertical out-of-bounds handling mode (forwarded to upstream resamplers).</summary>
    public OutOfBoundsMode VerticalOobMode { get; set; } = OutOfBoundsMode.ConstantExtension;
    /// <summary>Canvas fill colour used when either axis is in <see cref="OutOfBoundsMode.FlatColor"/> mode — painted around the source image.</summary>
    public Color CanvasColor { get; set; } = Color.Transparent;
    /// <summary>When <c>true</c>, destination pixel centres are aligned with source coordinates; when <c>false</c>, top-left corners are. Upstream resamplers honour this per-call.</summary>
    public bool UseCenteredGrid { get; set; } = true;

    /// <summary>
    /// Tunable parameter overrides for the currently selected manipulator entry, keyed by
    /// <c>ParameterDescriptor.Name</c>. <c>null</c> or empty when the entry is non-parametric
    /// or the user hasn't touched any field. Consumed by the effect's
    /// <c>OnSetRenderInfo</c> via <c>ManipulatorEntry.CreateWith(values)</c>.
    /// </summary>
    public Dictionary<string, object> ParameterValues { get; set; }

    public PluginConfigToken() { }

    public PluginConfigToken(PluginConfigToken cloneFrom) : base(cloneFrom) {
      this.FilterName = cloneFrom.FilterName;
      this.Mode = cloneFrom.Mode;
      this.PercentX = cloneFrom.PercentX;
      this.PercentY = cloneFrom.PercentY;
      this.FactorX = cloneFrom.FactorX;
      this.FactorY = cloneFrom.FactorY;
      this.TargetWidth = cloneFrom.TargetWidth;
      this.TargetHeight = cloneFrom.TargetHeight;
      this.LockAspectRatio = cloneFrom.LockAspectRatio;
      this.HorizontalOobMode = cloneFrom.HorizontalOobMode;
      this.VerticalOobMode = cloneFrom.VerticalOobMode;
      this.CanvasColor = cloneFrom.CanvasColor;
      this.UseCenteredGrid = cloneFrom.UseCenteredGrid;
      this.ParameterValues = cloneFrom.ParameterValues == null
        ? null
        : new Dictionary<string, object>(cloneFrom.ParameterValues);
    }

    public override object Clone() => new PluginConfigToken(this);
  }

  internal enum ScaleMode {
    /// <summary>Scale percentage (100 = identity).</summary>
    Percent,
    /// <summary>Scale factor (1.0 = identity).</summary>
    Factor,
    /// <summary>Absolute output dimensions in pixels.</summary>
    Size
  }
}
