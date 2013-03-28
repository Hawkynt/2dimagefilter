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
using System.ComponentModel;
using System.Diagnostics.Contracts;

using Imager;

namespace Classes.ImageManipulators {
  [Description("Color component extractors")]
  internal class PlaneExtractor : IImageManipulator {
    private readonly Func<cImage, cImage> _planeExtractionFunction;
    private readonly string _description;

    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (false); } }
    public bool SupportsHeight { get { return (false); } }
    public bool SupportsRepetitionCount { get { return (false); } }
    public bool SupportsGridCentering { get { return (false); } }
    public bool ChangesResolution { get { return (false); } }
    public bool SupportsThresholds { get { return (false); } }
    public bool SupportsRadius { get { return (false); } }
    public string Description { get { return (this._description); } }
    #endregion

    public cImage Apply(cImage source) {
      return (this._planeExtractionFunction(source));
    }

    public PlaneExtractor(Func<cImage, cImage> planeExtractionFunction, string description) {
      Contract.Requires(planeExtractionFunction != null);
      this._planeExtractionFunction = planeExtractionFunction;
      this._description = description;
    }

  }
}
