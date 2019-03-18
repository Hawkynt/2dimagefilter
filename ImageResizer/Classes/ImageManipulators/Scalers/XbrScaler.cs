#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

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

using System.Drawing;
using Imager;
using Imager.Interface;

namespace Classes.ImageManipulators.Scalers {
  internal class XbrScaler : AScaler {
    private readonly XbrScalerType _type;
    private readonly bool _allowAlphaBlending;

    #region Implementation of AScaler
    public override cImage Apply(cImage source) => source.ApplyScaler(this._type, this._allowAlphaBlending, default(Rectangle?));
    public override byte ScaleFactorX { get; }
    public override byte ScaleFactorY { get; }
    public override string Description => ReflectionUtils.GetDescriptionForEnumValue(this._type);

    #endregion

    public XbrScaler(XbrScalerType type, bool allowAlphaBlending) {
      var info = cImage.GetPixelScalerInfo(type);
      this._type = type;
      this._allowAlphaBlending = allowAlphaBlending;
      this.ScaleFactorX = info.Item1;
      this.ScaleFactorY = info.Item2;
    }
  }
}
