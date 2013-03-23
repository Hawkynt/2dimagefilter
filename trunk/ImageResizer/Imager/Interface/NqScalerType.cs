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
using Imager.Classes;

namespace Imager.Interface {
  public enum NqScalerType {

    #region hq group
    [EnumDisplayName("HQ 2x")]
    Hq2,
    [EnumDisplayName("HQ 2x3")]
    Hq2X3,
    [EnumDisplayName("HQ 2x4")]
    Hq2X4,
    [EnumDisplayName("HQ 3x")]
    Hq3,
    [EnumDisplayName("HQ 4x")]
    Hq4,
    #endregion

    #region lq group
    [EnumDisplayName("LQ 2x")]
    Lq2,
    [EnumDisplayName("LQ 2x3")]
    Lq2X3,
    [EnumDisplayName("LQ 2x4")]
    Lq2X4,
    [EnumDisplayName("LQ 3x")]
    Lq3,
    [EnumDisplayName("LQ 4x")]
    Lq4,
    #endregion
  }
}
