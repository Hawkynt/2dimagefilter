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

namespace System {
  internal static partial class TypeExtensions {
    /// <summary>
    /// Gets the assembly attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="this">This type.</param>
    /// <param name="inherit">if set to <c>true</c> inherited attributes would also be returned; otherwise, not.</param>
    /// <param name="index">The index to use if multiple attributes were found of that kind.</param>
    /// <returns>The given attribute instance.</returns>
    public static TAttribute GetAssemblyAttribute<TAttribute>(this Type @this, bool inherit = false, int index = 0) 
      => (TAttribute)@this.Assembly.GetCustomAttributes(typeof(TAttribute), inherit)[index]
    ;

  }
}
