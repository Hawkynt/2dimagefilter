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
using System.Reflection;

using Imager.Classes;

namespace Classes {
  internal static class ReflectionUtils {
    /// <summary>
    /// Gets a typed enumeration of all values of a given enumeration.
    /// </summary>
    /// <typeparam name="TEnumeration">The type of enumeration.</typeparam>
    /// <returns>All values.</returns>
    public static IEnumerable<TEnumeration> GetEnumValues<TEnumeration>() where TEnumeration : struct {
      Contract.Requires(typeof(TEnumeration).IsEnum, "Only enums supported");
      return (from TEnumeration i in Enum.GetValues(typeof(TEnumeration))
              select i);
    }

    /// <summary>
    /// Gets a descriptive name for a given enum value by trying to find a description attribute first or using the name for the given value.
    /// </summary>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>A descriptive text.</returns>
    public static string GetDisplayNameForEnumValue<TEnumeration>(TEnumeration value) where TEnumeration : struct {
      Contract.Requires(typeof(TEnumeration).IsEnum, "Only enum supported");
      var valueName = Enum.GetName(typeof(TEnumeration), value);
      var fieldInfo = typeof(TEnumeration).GetMember(valueName)[0];
      var descriptionAttribute = (EnumDisplayNameAttribute)fieldInfo.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false).FirstOrDefault();
      return (descriptionAttribute == null ? valueName : descriptionAttribute.Name);
    }

    /// <summary>
    /// Gets a descriptive name for a given enum value by trying to find a description attribute first or using the name for the given value.
    /// </summary>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>A descriptive text.</returns>
    public static string GetDescriptionForEnumValue<TEnumeration>(TEnumeration value) where TEnumeration : struct {
      Contract.Requires(typeof(TEnumeration).IsEnum, "Only enum supported");
      var valueName = Enum.GetName(typeof(TEnumeration), value);
      var fieldInfo = typeof(TEnumeration).GetMember(valueName)[0];
      var descriptionAttribute = (DescriptionAttribute)fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
      return (descriptionAttribute == null ? null : descriptionAttribute.Description);
    }

    /// <summary>
    /// Gets an attribute from the entry assembly.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="getter">The getter.</param>
    /// <returns>The attribute value.</returns>
    public static object GetEntryAssemblyAttribute<TAttribute>(Func<TAttribute, object> getter) where TAttribute : Attribute {
      Contract.Requires(getter != null);
      var assembly = Assembly.GetEntryAssembly();
      var attribute = assembly.GetCustomAttributes(typeof(TAttribute), true).First();
      return (getter((TAttribute)attribute));
    }

    /// <summary>
    /// Gets the description for a class.
    /// </summary>
    /// <param name="classType">Type of the class.</param>
    /// <returns>
    /// A descriptive text
    /// </returns>
    public static string GetDescriptionForClass(Type classType) {
      Contract.Requires(classType != null);
      var attribute = (DescriptionAttribute)classType.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
      if (attribute == null) {

        // when no description attribute was found
        var name = classType.FullName;
        var lastDot = name == null ? -1 : name.LastIndexOf('.');

        // when there is no dot in the type name, just return the name
        if (lastDot < 0)
          return (name);

        // otherwise return the last part of the name
        return (name.Substring(lastDot + 1));
      }

      return (attribute.Description);
    }
  }
}
