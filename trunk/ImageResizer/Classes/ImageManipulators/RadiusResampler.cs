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
using Imager;
using Imager.Classes;

namespace Classes.ImageManipulators {
  [Description("Radius-based filters")]
  internal class RadiusResampler : IImageManipulator {
    private readonly WindowType _type;

    #region Implementation of IImageManipulator
    public bool SupportsWidth { get { return (true); } }
    public bool SupportsHeight { get { return (true); } }
    public bool SupportsRepetitionCount { get { return (false); } }
    public bool SupportsGridCentering { get { return (true); } }
    public bool SupportsThresholds { get { return (false); } }
    public bool SupportsRadius { get { return (true); } }
    public bool ChangesResolution { get { return (true); } }
    public string Description { get { return (ReflectionUtils.GetDescriptionForEnumValue(this._type)); } }
    #endregion

    public cImage Apply(cImage source, int width, int height, float radius, bool useCenteredGrid) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, width, height, radius, useCenteredGrid));
    }

    public RadiusResampler(WindowType type) {
      this._type = type;
    }

    public Kernels.FixedRadiusKernelInfo GetKernelMethodInfo(float radius) {
      return (Windows.WINDOWS[this._type].WithRadius(radius));
    }
  }
}
