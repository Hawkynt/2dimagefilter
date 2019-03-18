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
using Classes;
using Imager;
using Imager.Interface;
using System;
using System.Drawing;
using System.Linq;

namespace PixelArtScaling {
  class SupportedManipulators {

    internal delegate cImage ImageManipulator(cImage sourceImage, Rectangle sourceRectangle);

    public static readonly Tuple<string, ScalerInformation, ImageManipulator>[] Manipulators =
      new Tuple<string, ScalerInformation, ImageManipulator>[0]

    #region add pixel resizer

.Concat(
          from p in ReflectionUtils.GetEnumValues<PixelScalerType>()
          select
            Tuple.Create(
              ReflectionUtils.GetDisplayNameForEnumValue(p),
              cImage.GetScalerInformation(p),
              new ImageManipulator((i, r) => i.ApplyScaler(p, r)))
        )
    #endregion

    #region add xbr resizer

.Concat(
          from p in ReflectionUtils.GetEnumValues<XbrScalerType>()
          select
            Tuple.Create(
              ReflectionUtils.GetDisplayNameForEnumValue(p) + " <NoBlend>",
              cImage.GetScalerInformation(p),
              new ImageManipulator((i, r) => i.ApplyScaler(p, false, r)))
        )
        .Concat(
          from p in ReflectionUtils.GetEnumValues<XbrScalerType>()
          select
            Tuple.Create(
              ReflectionUtils.GetDisplayNameForEnumValue(p),
              cImage.GetScalerInformation(p),
              new ImageManipulator((i, r) => i.ApplyScaler(p, true, r)))
        )
    #endregion
    #region xbrz
.Concat(
          from p in ReflectionUtils.GetEnumValues<XbrzScalerType>()
          select
            Tuple.Create(
              ReflectionUtils.GetDisplayNameForEnumValue(p),
              cImage.GetScalerInformation(p),
              new ImageManipulator((i, r) => i.ApplyScaler(p, r)))
        )
    #endregion

    #region add nq resizer

.Concat(
          from p in ReflectionUtils.GetEnumValues<NqScalerType>()
          from m in ReflectionUtils.GetEnumValues<NqMode>()
          select
            Tuple.Create(
              ReflectionUtils.GetDisplayNameForEnumValue(p) +
              (m == NqMode.Normal ? string.Empty : " " + ReflectionUtils.GetDisplayNameForEnumValue(m)),
              cImage.GetScalerInformation(p),
              new ImageManipulator((i, r) => i.ApplyScaler(p, m, r)))
        )
    #endregion

.ToArray();
  }
}
