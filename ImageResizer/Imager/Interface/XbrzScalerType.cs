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

using Imager.Classes;
using System.ComponentModel;

namespace Imager.Interface {
  public enum XbrzScalerType {
    [EnumDisplayName("XBRz 2x")]
    [Description("Zenju's XBRz 2x")]
    Xbrz2,
    [EnumDisplayName("XBRz 3x")]
    [Description("Zenju's XBRz 3x")]
    Xbrz3,
    [EnumDisplayName("XBRz 4x")]
    [Description("Zenju's XBRz 4x")]
    Xbrz4,
    [EnumDisplayName("XBRz 5x")]
    [Description("Zenju's XBRz 5x")]
    Xbrz5,
  }
}
