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

using System.Drawing.Extensions.ColorProcessing.Resizing;

using Imager;
using Imager.Pipelines;

namespace Classes.ImageManipulators {
  /// <summary>
  /// Wraps an upstream bitmap resampler — takes a target width and height from the user.
  /// Forwards out-of-bounds mode, canvas fill colour and centred-grid flag into the upstream pipeline.
  /// </summary>
  [Description("Upstream bitmap resampler")]
  internal class BitmapResamplerAdapter : IImageManipulator {

    private readonly UpstreamPipeline.ResampleFunc _operation;

    public BitmapResamplerAdapter(string description, UpstreamPipeline.ResampleFunc operation, int kernelRadius = 0, Func<float, float> evaluateKernel = null) {
      this.Description = description;
      this._operation = operation;
      this.KernelRadius = kernelRadius;
      this.EvaluateKernel = evaluateKernel;
    }

    #region Implementation of IImageManipulator
    public bool SupportsWidth => true;
    public bool SupportsHeight => true;
    public bool SupportsRepetitionCount => false;
    public bool SupportsGridCentering => true;
    public bool SupportsThresholds => false;
    public bool SupportsRadius => false;
    public bool ChangesResolution => true;
    public string Description { get; }
    #endregion

    /// <summary>Kernel support radius (the resampler samples over <c>[-Radius, +Radius]</c>). Zero when the upstream resampler is not a separable-kernel one.</summary>
    public int KernelRadius { get; }

    /// <summary>Closed-form 1-D kernel weight function for the chart, or <c>null</c> for edge-aware / content-adaptive / fixed-tap resamplers where no separable weight exists.</summary>
    public Func<float, float> EvaluateKernel { get; }

    public cImage Apply(cImage source, int width, int height)
      => this.Apply(source, width, height, OutOfBoundsMode.ConstantExtension, OutOfBoundsMode.ConstantExtension, Color.Transparent, useCenteredGrid: true);

    public cImage Apply(cImage source, int width, int height, OutOfBoundsMode horizontalMode, OutOfBoundsMode verticalMode, Color canvasColor, bool useCenteredGrid) {
      using (var input = source.ToBitmap())
      using (var output = this._operation(input, width, height, horizontalMode, verticalMode, canvasColor, useCenteredGrid))
        return cImage.FromBitmap(output);
    }
  }
}
