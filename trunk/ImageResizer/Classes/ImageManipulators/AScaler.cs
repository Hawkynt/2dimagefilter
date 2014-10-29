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

using System.ComponentModel;

using Imager;

namespace Classes.ImageManipulators {
  [Description("Pixel art filters")]
  internal abstract class AScaler : IImageManipulator {
    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (false); } }
    public bool SupportsHeight { get { return (false); } }
    public bool SupportsRepetitionCount { get { return (true); } }
    public bool SupportsGridCentering { get { return (false); } }
    public bool SupportsRadius { get { return (false); } }
    public bool ChangesResolution { get { return (true); } }
    public bool SupportsThresholds { get { return (true); } }
    public abstract string Description { get; }
    #endregion

    public abstract cImage Apply(cImage source);
    public abstract byte ScaleFactorX { get; }
    public abstract byte ScaleFactorY { get; }
  }
}
