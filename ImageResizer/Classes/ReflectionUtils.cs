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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

using Imager.Classes;

namespace Classes {
  static class ReflectionUtils {
    /// <summary>
    /// Gets a typed enumeration of all values of a given enumeration.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <returns>All values.</returns>
    public static IEnumerable<T> GetEnumValues<T>() where T : struct {
      Contract.Requires(typeof(T).IsEnum, "Only enums supported");
      return (from T i in Enum.GetValues(typeof(T))
              select i);
    }

    /// <summary>
    /// Gets a descriptive name for a given enum value by trying to find a description attribute first or using the name for the given value.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>A descriptive text.</returns>
    public static string GetDisplayNameForEnumValue<T>(T value) where T : struct {
      Contract.Requires(typeof(T).IsEnum, "Only enum supported");
      var valueName = Enum.GetName(typeof(T), value);
      var fieldInfo = typeof(T).GetMember(valueName)[0];
      var descriptionAttribute = (EnumDisplayNameAttribute)fieldInfo.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false).FirstOrDefault();
      return (descriptionAttribute == null ? valueName : descriptionAttribute.Name);
    }

    /// <summary>
    /// Gets a descriptive name for a given enum value by trying to find a description attribute first or using the name for the given value.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>A descriptive text.</returns>
    public static string GetDescriptionForEnumValue<T>(T value) where T : struct {
      Contract.Requires(typeof(T).IsEnum, "Only enum supported");
      var valueName = Enum.GetName(typeof(T), value);
      var fieldInfo = typeof(T).GetMember(valueName)[0];
      var descriptionAttribute = (DescriptionAttribute)fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
      return (descriptionAttribute == null ? null : descriptionAttribute.Description);
    }

  }
}
