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

using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing.Drawing2D;

using Imager;

namespace Classes.ImageManipulators {
  [Description("GDI+ .NET internal filters")]
  internal class Interpolator : IImageManipulator {
    private readonly InterpolationMode _type;

    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (true); } }
    public bool SupportsHeight { get { return (true); } }
    public bool SupportsRepetitionCount { get { return (false); } }
    public bool SupportsGridCentering { get { return (false); } }
    public bool SupportsRadius { get { return (false); } }
    public bool ChangesResolution { get { return (true); } }
    public bool SupportsThresholds { get { return (false); } }
    public string Description {
      get {
        switch (this._type) {
          case InterpolationMode.NearestNeighbor: {
            return ("Nearest neighbor interpolation using the Microsoft GDI+ API.");
          }
          case InterpolationMode.Bilinear: {
            return ("Bilinear interpolation using the Microsoft GDI+ API. No prefiltering is done. This mode is not suitable for shrinking an image below 50 percent of its original size.");
          }
          case InterpolationMode.Bicubic: {
            return ("Bicubic interpolation using the Microsoft GDI+ API. No prefiltering is done. This mode is not suitable for shrinking an image below 25 percent of its original size.");
          }
          case InterpolationMode.HighQualityBilinear: {
            return ("Bilinear interpolation using the Microsoft GDI+ API. Prefiltering is performed to ensure high-quality shrinking.");
          }
          case InterpolationMode.HighQualityBicubic: {
            return ("Bicubic interpolation using the Microsoft GDI+ API. Prefiltering is performed to ensure high-quality shrinking.");
          }
          default: {
            return (null);
          }
        }
      }
    }
    #endregion

    public cImage Apply(cImage source, int width, int height) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, width, height));
    }

    public Interpolator(InterpolationMode type) {
      this._type = type;
    }
  }
}
