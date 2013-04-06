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

using System.Collections.Generic;

namespace System {
  internal static partial class StringExtensions {
    /// <summary>
    /// Determines whether the given string is <c>null</c> or whitespace-only.
    /// </summary>
    /// <param name="This">The this.</param>
    /// <returns>
    ///   <c>true</c> if the given string is <c>null</c> or whitespace-only; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrWhiteSpace(this string This) {
      return (string.IsNullOrWhiteSpace(This));
    }

    /// <summary>
    /// Splits a string by a given delimiter and respects any quotes.
    /// </summary>
    /// <param name="This">This String.</param>
    /// <param name="delimiter">The delimiter.</param>
    /// <param name="quotingCharacter">The quoting character.</param>
    /// <returns></returns>
    public static IEnumerable<string> SplitWithQuotes(this string This, char delimiter = ',', char quotingCharacter = '"') {
      if (This == null)
        yield break;

      var lastPiece = string.Empty;
      var isInQuoteMode = false;

      var length = This.Length;
      for (var index = 0; index < length; index++) {
        var currentChar = This[index];
        if (isInQuoteMode) {
          if (currentChar == quotingCharacter)
            isInQuoteMode = false;
          else
            lastPiece += currentChar;
        } else {
          if (currentChar == delimiter) {
            yield return lastPiece;
            lastPiece = string.Empty;
          } else if (currentChar == quotingCharacter)
            isInQuoteMode = true;
          else
            lastPiece += currentChar;
        }
      }
      yield return lastPiece;
    }
  }
}
