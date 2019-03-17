#region (c)2010-2020 Hawkynt
/*
  This file is part of Hawkynt's .NET Framework extensions.

    Hawkynt's .NET Framework extensions are free software: 
    you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Hawkynt's .NET Framework extensions is distributed in the hope that 
    it will be useful, but WITHOUT ANY WARRANTY; without even the implied 
    warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
    the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Hawkynt's .NET Framework extensions.  
    If not, see <http://www.gnu.org/licenses/>.
*/
#endregion
using System.Drawing;

namespace System.Windows {
  public static partial class RectExtensions {
    /// <summary>
    /// Converts the given System.Windows.Rect into a System.Drawing.Rectangle
    /// </summary>
    /// <param name="this">This System.Windows.Rect</param>
    /// <returns>Converted Rectangle</returns>
    public static Rectangle ToRectangle(this Rect @this) {
      return new Rectangle((int)@this.X, (int)@this.Y, (int)@this.Width, (int)@this.Height);
    }
  }
}
