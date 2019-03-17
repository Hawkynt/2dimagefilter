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
using System.Diagnostics.Contracts;
using System.Drawing;
using Imager;
using Imager.Classes;

namespace Classes.ImageManipulators {
  [Description("General purpose filters")]
  internal class Resampler : IImageManipulator {
    private readonly KernelType _type;

    #region Implementation of IImageManipulator
    public bool SupportsWidth => (true);
    public bool SupportsHeight => (true);
    public bool SupportsRepetitionCount => (false);
    public bool SupportsGridCentering => (true);
    public bool SupportsThresholds => (false);
    public bool SupportsRadius => (false);
    public bool ChangesResolution => (true);
    public string Description => (ReflectionUtils.GetDescriptionForEnumValue(this._type));

    #endregion

    public cImage Apply(cImage source, int width, int height, bool useCenteredGrid) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, width, height, useCenteredGrid, default(Rectangle?)));
    }

    public Resampler(KernelType type) {
      this._type = type;
    }

    public Kernels.FixedRadiusKernelInfo GetKernelMethodInfo() {
      return (Kernels.KERNELS[this._type]);
    }
  }
}
