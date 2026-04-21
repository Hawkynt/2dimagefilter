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
using Imager.Classes;

namespace Classes.ImageManipulators {
  /// <summary>
  /// Wraps a NuGet-provided bitmap resampler — takes a target width and height from the user.
  /// </summary>
  [Description("Upstream bitmap resampler")]
  internal class BitmapResamplerAdapter : IImageManipulator {

    private readonly Func<Bitmap, int, int, Bitmap> _operation;

    public BitmapResamplerAdapter(string description, Func<Bitmap, int, int, Bitmap> operation, Kernels.FixedRadiusKernelInfo? kernelInfo = null) {
      this.Description = description;
      this._operation = operation;
      this.KernelInfo = kernelInfo;
    }

    #region Implementation of IImageManipulator
    public bool SupportsWidth => true;
    public bool SupportsHeight => true;
    public bool SupportsRepetitionCount => false;
    public bool SupportsGridCentering => false;
    public bool SupportsThresholds => false;
    public bool SupportsRadius => false;
    public bool ChangesResolution => true;
    public string Description { get; }
    #endregion

    /// <summary>
    /// Optional 1-D kernel shape for the kernel chart. <c>null</c> when no equivalent local
    /// kernel definition is available; the chart will be hidden in that case.
    /// </summary>
    public Kernels.FixedRadiusKernelInfo? KernelInfo { get; }

    public cImage Apply(cImage source, int width, int height) {
      using (var input = source.ToBitmap()) {
        using (var output = this._operation(input, width, height)) {
          return cImage.FromBitmap(output);
        }
      }
    }
  }
}
