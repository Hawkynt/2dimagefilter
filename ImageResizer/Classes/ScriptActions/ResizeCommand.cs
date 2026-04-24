#region (c)2008-2015 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2015 Hawkynt

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

using System.Drawing.Extensions.ColorProcessing.Resizing;

using Classes.ImageManipulators;
using Imager;

using word = System.UInt16;

namespace Classes.ScriptActions {
  internal class ResizeCommand : IScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage => false;
    public bool ChangesTargetImage => true;
    public bool ProvidesNewGdiSource => false;

    public bool Execute() {
      var source = this._applyToTarget ? this.TargetImage : this.SourceImage;

      var width = this.Width;
      var height = this.Height;

      // pverwrite dimensions from percentage if needed
      var percentage = this.Percentage;
      if (percentage > 0) {
        width = (word)Math.Round(source.Width * percentage / 100d);
        height = (word)Math.Round(source.Height * percentage / 100d);
      }

      // correct aspect ratio if needed
      if (this.MaintainAspect) {
        if (width == 0) {
          width = (word)Math.Round((double)height * source.Width / source.Height);
        } else {
          height = (word)Math.Round((double)width * source.Height / source.Width);
        }
      }

      // UseThresholds + HorizontalBph/VerticalBph retained in the serialization contract but no longer
      // thread any scaler behaviour after the local sPixel-based XBR/XBRz/NQ migration.

      cImage result = null;
      var method = this.Manipulator;
      var planeExtractor = method as PlaneExtractor;
      var bitmapFixed = method as BitmapFixedAdapter;
      var bitmapResampler = method as BitmapResamplerAdapter;

      if (bitmapFixed != null) {
        result = bitmapFixed.Apply(source, this.UseThresholds);
      } else if (bitmapResampler != null) {
        result = bitmapResampler.Apply(source, width, height, this.HorizontalBph, this.VerticalBph, System.Drawing.Color.Transparent, this.UseCenteredGrid);
      } else if (planeExtractor != null) {
        result = planeExtractor.Apply(source);
      }

      this.TargetImage = result;
      return true;
    }

    public Bitmap GdiSource => null;
    public cImage SourceImage { get; set; }
    public cImage TargetImage { get; set; }
    
    #endregion

    public IImageManipulator Manipulator { get; }
    public word Width { get; }
    public word Height { get; }
    public bool MaintainAspect { get; }
    public OutOfBoundsMode HorizontalBph { get; }
    public OutOfBoundsMode VerticalBph { get; }
    public byte Count { get; }
    public bool UseThresholds { get; }
    public bool UseCenteredGrid { get; }
    public float Radius { get; }
    public word Percentage { get; }

    private readonly bool _applyToTarget;

    public ResizeCommand(bool applyToTarget, IImageManipulator manipulator, word width, word height, word percentage, bool maintainAspect, OutOfBoundsMode horizontalBph, OutOfBoundsMode verticalBph, byte count, bool useThresholds, bool useCenteredGrid, float radius) {
      this._applyToTarget = applyToTarget;
      this.Manipulator = manipulator;
      this.Width = width;
      this.Height = height;
      this.MaintainAspect = maintainAspect;
      this.HorizontalBph = horizontalBph;
      this.VerticalBph = verticalBph;
      this.Count = count;
      this.UseThresholds = useThresholds;
      this.UseCenteredGrid = useCenteredGrid;
      this.Radius = radius;
      this.Percentage = percentage;
    }
  }
}
