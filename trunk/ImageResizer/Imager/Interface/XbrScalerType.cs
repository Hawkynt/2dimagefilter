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

using Imager.Classes;

namespace Imager.Interface {
  public enum XbrScalerType {
    #region xbr group
    [EnumDisplayName("XBR 2x")]
    [Description("Hyllian's XBR 2x")]
    Xbr2,
    [EnumDisplayName("XBR 3x")]
    [Description("Hyllian's XBR 3x")]
    Xbr3,
    [EnumDisplayName("XBR 3x(modified)")]
    [Description("Hyllian's XBR 3x (non-original version)")]
    Xbr3Modified,
    [EnumDisplayName("XBR 4x")]
    [Description("Hyllian's XBR 4x")]
    Xbr4,
    #endregion
  }
}
