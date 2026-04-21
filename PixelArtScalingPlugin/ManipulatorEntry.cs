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
using System.Drawing;

using Hawkynt.ColorProcessing.Resizing;

using Imager;
using Imager.Pipelines;

namespace PixelArtScaling {

  /// <summary>
  /// One entry in the plugin's filter dropdown. Polymorphic so single-scale entries (most local
  /// scalers, filters, plane extractors), multi-scale fixed entries (HQ / LQ / Bicubic /
  /// CRT-* / etc. — pick one of N supported scales), and arbitrary resamplers all share
  /// the same dispatch path.
  /// </summary>
  internal abstract class ManipulatorEntry {
    public string Name { get; }
    public string Description { get; }
    protected ManipulatorEntry(string name, string description) {
      this.Name = name;
      this.Description = description;
    }

    /// <summary>True when the entry honours the user-supplied target width/height/scale-percent.</summary>
    public abstract bool SupportsCustomDimensions { get; }

    /// <summary>Computes the output canvas rectangle Paint.NET should allocate.</summary>
    public abstract Rectangle ComputeTargetRectangle(Rectangle source, int userTargetWidth, int userTargetHeight);

    /// <summary>Runs the manipulator. <paramref name="targetWidth"/>/<paramref name="targetHeight"/> are honoured by resamplers (free) and multi-scale fixed entries (snapped to the nearest supported scale). Ignored by single-scale fixed entries.</summary>
    public abstract cImage Apply(cImage source, Rectangle sourceRectangle, int targetWidth, int targetHeight);
  }

  /// <summary>Fixed single-scale entry — output dimensions are <c>source × (ScaleX, ScaleY)</c> and the user W/H sliders are ignored.</summary>
  internal sealed class FixedScaleEntry : ManipulatorEntry {
    public byte ScaleX { get; }
    public byte ScaleY { get; }
    private readonly Func<cImage, Rectangle, cImage> _apply;

    public FixedScaleEntry(string name, string description, byte scaleX, byte scaleY, Func<cImage, Rectangle, cImage> apply)
      : base(name, description) {
      this.ScaleX = scaleX;
      this.ScaleY = scaleY;
      this._apply = apply;
    }

    public override bool SupportsCustomDimensions => false;

    public override Rectangle ComputeTargetRectangle(Rectangle source, int _, int __) => new Rectangle(
      source.X * this.ScaleX,
      source.Y * this.ScaleY,
      source.Width * this.ScaleX,
      source.Height * this.ScaleY
    );

    public override cImage Apply(cImage source, Rectangle sourceRectangle, int _, int __) => this._apply(source, sourceRectangle);
  }

  /// <summary>
  /// Multi-scale fixed entry — the algorithm advertises a discrete list of supported scales (e.g. HQ supports
  /// 2×2 / 3×3 / 4×4 / 2×3 / 2×4). The user's Target W/H or Scale (%) input snaps to the nearest of those.
  /// </summary>
  internal sealed class ScaleVariantEntry : ManipulatorEntry {
    public ScaleFactor[] SupportedScales { get; }
    private readonly Func<cImage, Rectangle, int, int, cImage> _apply;

    public ScaleVariantEntry(string name, string description, ScaleFactor[] supportedScales, Func<cImage, Rectangle, int, int, cImage> apply)
      : base(name, description) {
      this.SupportedScales = supportedScales;
      this._apply = apply;
    }

    public override bool SupportsCustomDimensions => true;

    public override Rectangle ComputeTargetRectangle(Rectangle source, int userTargetWidth, int userTargetHeight) {
      var snapped = UpstreamPipeline.SnapToNearestSupportedScale(this.SupportedScales, source.Width, source.Height, userTargetWidth, userTargetHeight);
      return new Rectangle(0, 0, source.Width * snapped.X, source.Height * snapped.Y);
    }

    public override cImage Apply(cImage source, Rectangle sourceRectangle, int targetWidth, int targetHeight) {
      var snapped = UpstreamPipeline.SnapToNearestSupportedScale(this.SupportedScales, sourceRectangle.Width, sourceRectangle.Height, targetWidth, targetHeight);
      return this._apply(source, sourceRectangle, sourceRectangle.Width * snapped.X, sourceRectangle.Height * snapped.Y);
    }
  }

  /// <summary>Variable-target resampler entry — output dimensions come from the user's W/H or Scale (%) sliders, no snapping.</summary>
  internal sealed class ResampleEntry : ManipulatorEntry {
    private readonly Func<cImage, Rectangle, int, int, cImage> _resample;

    public ResampleEntry(string name, string description, Func<cImage, Rectangle, int, int, cImage> resample)
      : base(name, description) {
      this._resample = resample;
    }

    public override bool SupportsCustomDimensions => true;

    public override Rectangle ComputeTargetRectangle(Rectangle source, int userTargetWidth, int userTargetHeight) {
      var w = userTargetWidth > 0 ? userTargetWidth : source.Width;
      var h = userTargetHeight > 0 ? userTargetHeight : source.Height;
      return new Rectangle(0, 0, w, h);
    }

    public override cImage Apply(cImage source, Rectangle sourceRectangle, int targetWidth, int targetHeight) {
      var w = targetWidth > 0 ? targetWidth : sourceRectangle.Width;
      var h = targetHeight > 0 ? targetHeight : sourceRectangle.Height;
      return this._resample(source, sourceRectangle, w, h);
    }
  }
}
