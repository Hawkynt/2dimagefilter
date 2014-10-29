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
using System.Diagnostics.Contracts;

using Imager;
using Imager.Interface;

namespace Classes.ImageManipulators.Scalers {
  internal class NqScaler : AScaler {
    private readonly NqScalerType _type;
    private readonly NqMode _mode;
    private readonly byte _scaleFactorX;
    private readonly byte _scaleFactorY;

    #region Implementation of AScaler
    public override cImage Apply(cImage source) {
      Contract.Requires(source != null);
      return (source.ApplyScaler(this._type, this._mode));
    }
    public override byte ScaleFactorX { get { return (this._scaleFactorX); } }
    public override byte ScaleFactorY { get { return (this._scaleFactorY); } }
    public override string Description { get { return (ReflectionUtils.GetDescriptionForEnumValue(this._type)); } }
    #endregion

    public NqScaler(NqScalerType type, NqMode mode) {
      var info = cImage.GetPixelScalerInfo(type);
      this._type = type;
      this._mode = mode;
      this._scaleFactorX = info.Item1;
      this._scaleFactorY = info.Item2;
    }
  }
}
