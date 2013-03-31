#region (c)2008-2013 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010-2013 Hawkynt

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

using Classes.ImageManipulators;
using Imager;
using Imager.Interface;

using word = System.UInt16;

namespace Classes.ScriptActions {
  internal class ResizeCommand : IScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage { get { return (false); } }

    public bool ChangesTargetImage { get { return (true); } }
    public bool ProvidesNewGdiSource { get { return (false); } }

    public bool Execute() {
      var source = this._applyToTarget ? this.TargetImage : this.SourceImage;

      var width = this._width;
      var height = this._height;

      // pverwrite dimensions from percentage if needed
      var percentage = this._percentage;
      if (percentage > 0) {
        width = (word)Math.Round(source.Width * percentage / 100d);
        height = (word)Math.Round(source.Height * percentage / 100d);
      }

      // correct aspect ratio if needed
      if (this._maintainAspect) {
        if (width == 0) {
          width = (word)Math.Round((double)height * source.Width / source.Height);
        } else {
          height = (word)Math.Round((double)width * source.Height / source.Width);
        }
      }

      sPixel.AllowThresholds = this._useThresholds;
      source.HorizontalOutOfBoundsMode = this._horizontalBph;
      source.VerticalOutOfBoundsMode = this._verticalBph;

      cImage result = null;
      var method = this._manipulator;
      var scaler = method as AScaler;
      var interpolator = method as Interpolator;
      var planeExtractor = method as PlaneExtractor;
      var resampler = method as Resampler;
      var radiusResampler = method as RadiusResampler;

      if (scaler != null) {
        result = source;
        for (var i = 0; i < this._count; i++)
          result = scaler.Apply(result);
      } else {
        if (interpolator != null)
          result = interpolator.Apply(source, width, height);
        else if (planeExtractor != null)
          result = planeExtractor.Apply(source);
        else if (resampler != null)
          result = resampler.Apply(source, width, height, this._useCenteredGrid);
        else if (radiusResampler != null)
          result = radiusResampler.Apply(source, width, height, this._radius, this._useCenteredGrid);
      }

      this.TargetImage = result;
      return (true);
    }

    public Bitmap GdiSource { get { return (null); } }

    public cImage SourceImage { get; set; }

    public cImage TargetImage { get; set; }
    #endregion

    private readonly IImageManipulator _manipulator;
    public IImageManipulator Manipulator { get { return (this._manipulator); } }

    private readonly word _width;
    public word Width { get { return (this._width); } }

    private readonly word _height;
    public word Height { get { return (this._height); } }

    private readonly bool _maintainAspect;
    public bool MaintainAspect { get { return (this._maintainAspect); } }

    private readonly OutOfBoundsMode _horizontalBph;
    public OutOfBoundsMode HorizontalBph { get { return (this._horizontalBph); } }

    private readonly OutOfBoundsMode _verticalBph;
    public OutOfBoundsMode VerticalBph { get { return (this._verticalBph); } }

    private readonly byte _count;
    public byte Count { get { return (this._count); } }

    private readonly bool _useThresholds;
    public bool UseThresholds { get { return (this._useThresholds); } }

    private readonly bool _useCenteredGrid;
    public bool UseCenteredGrid { get { return (this._useCenteredGrid); } }

    private readonly float _radius;
    public float Radius { get { return (this._radius); } }

    private readonly word _percentage;
    public word Percentage { get { return (this._percentage); } }

    private readonly bool _applyToTarget;

    public ResizeCommand(bool applyToTarget, IImageManipulator manipulator, word width, word height, word percentage, bool maintainAspect, OutOfBoundsMode horizontalBph, OutOfBoundsMode verticalBph, byte count, bool useThresholds, bool useCenteredGrid, float radius) {
      this._applyToTarget = applyToTarget;
      this._manipulator = manipulator;
      this._width = width;
      this._height = height;
      this._maintainAspect = maintainAspect;
      this._horizontalBph = horizontalBph;
      this._verticalBph = verticalBph;
      this._count = count;
      this._useThresholds = useThresholds;
      this._useCenteredGrid = useCenteredGrid;
      this._radius = radius;
      this._percentage = percentage;
    }
  }
}
