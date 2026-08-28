#region (c)2008-2026 Hawkynt
/*
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

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
using System.IO;
using System.Linq;

namespace Classes {
  /// <summary>
  /// Turns one command line that names a set of files into the command line that processes them
  /// one by one.
  /// <para>
  /// A <c>/load</c> whose file name contains <c>*</c> or <c>?</c> repeats everything up to the
  /// next <c>/load</c> once per matching file. Inside that repeat, a <c>*</c> in any other file
  /// name stands for the matched file's base name, so a target can be named after its source:
  /// </para>
  /// <code>
  /// /load *.png /resize auto "HQ 2x" /save big\*.png
  /// </code>
  /// <para>
  /// This runs before parsing, so the engine only ever sees ordinary file names and nothing
  /// downstream has to know that batches exist.
  /// </para>
  /// </summary>
  internal static class WildcardExpansion {

    /// <summary>
    /// Commands whose argument is a file name that a matched base name can be substituted into.
    /// </summary>
    private static readonly string[] _FILE_COMMANDS = {
      ScriptSerializer.LOAD_COMMAND_NAME,
      ScriptSerializer.SAVE_COMMAND_NAME,
      ScriptSerializer.SCRIPT_COMMAND_NAME,
    };

    /// <summary>
    /// Determines whether a file name selects a set rather than a single file.
    /// </summary>
    /// <param name="value">The file name.</param>
    /// <returns><c>true</c> when it contains a wildcard.</returns>
    public static bool ContainsWildcard(string value)
      => value != null && (value.IndexOf('*') >= 0 || value.IndexOf('?') >= 0)
    ;

    /// <summary>
    /// Expands every wildcard <c>/load</c> in a command line.
    /// </summary>
    /// <param name="arguments">The arguments as given.</param>
    /// <param name="matcher">
    /// Resolves a pattern to the files it names, in the order they should be processed. Injected
    /// so this can be exercised without a directory full of images.
    /// </param>
    /// <returns>
    /// The expanded arguments, or the originals unchanged when no <c>/load</c> carries a wildcard.
    /// </returns>
    public static string[] Expand(string[] arguments, Func<string, string[]> matcher = null) {
      if (arguments == null || arguments.Length < 1)
        return arguments;

      if (!_HasWildcardLoad(arguments))
        return arguments;

      matcher = matcher ?? _MatchFiles;

      var result = new List<string>(arguments.Length);
      var index = 0;
      while (index < arguments.Length) {
        if (!_IsLoad(arguments, index) || !ContainsWildcard(arguments[index + 1])) {
          result.Add(arguments[index++]);
          continue;
        }

        var pattern = arguments[index + 1];
        var segment = _TakeSegment(arguments, index);
        index += segment.Count;

        var matches = matcher(pattern) ?? new string[0];
        foreach (var match in matches)
          result.AddRange(_Substitute(segment, match));
      }

      return result.ToArray();
    }

    /// <summary>
    /// Determines whether any <c>/load</c> in the arguments carries a wildcard.
    /// </summary>
    private static bool _HasWildcardLoad(string[] arguments) {
      for (var i = 0; i < arguments.Length - 1; ++i)
        if (_IsLoad(arguments, i) && ContainsWildcard(arguments[i + 1]))
          return true;

      return false;
    }

    private static bool _IsLoad(string[] arguments, int index)
      => index + 1 < arguments.Length
      && string.Equals(arguments[index], ScriptSerializer.LOAD_COMMAND_NAME, StringComparison.OrdinalIgnoreCase)
    ;

    /// <summary>
    /// Takes the run of arguments a wildcard load owns: itself, its pattern, and everything up to
    /// the next <c>/load</c>.
    /// </summary>
    private static List<string> _TakeSegment(string[] arguments, int start) {
      var segment = new List<string> { arguments[start], arguments[start + 1] };
      for (var i = start + 2; i < arguments.Length; ++i) {
        if (_IsLoad(arguments, i) || string.Equals(arguments[i], ScriptSerializer.LOAD_COMMAND_NAME, StringComparison.OrdinalIgnoreCase))
          break;

        segment.Add(arguments[i]);
      }

      return segment;
    }

    /// <summary>
    /// Fills a segment in for one matched file: the pattern becomes that file, and a <c>*</c> in
    /// any other file name becomes its base name.
    /// </summary>
    private static IEnumerable<string> _Substitute(List<string> segment, string match) {
      var baseName = Path.GetFileNameWithoutExtension(match);

      // the load's own argument is the file itself, not a name built from it
      yield return segment[0];
      yield return match;

      for (var i = 2; i < segment.Count; ++i) {
        var isFileArgument = i > 0 && _FILE_COMMANDS.Contains(segment[i - 1], StringComparer.OrdinalIgnoreCase);
        yield return isFileArgument && ContainsWildcard(segment[i])
          ? segment[i].Replace("*", baseName)
          : segment[i];
      }
    }

    /// <summary>
    /// Resolves a pattern against the file system, ordered by name so a batch is reproducible.
    /// </summary>
    private static string[] _MatchFiles(string pattern) {
      try {
        var directory = Path.GetDirectoryName(pattern);
        var searchPattern = Path.GetFileName(pattern);
        if (string.IsNullOrEmpty(searchPattern))
          return new string[0];

        var files = Directory.GetFiles(string.IsNullOrEmpty(directory) ? "." : directory, searchPattern);
        Array.Sort(files, StringComparer.OrdinalIgnoreCase);

        // keep the caller's spelling: a bare pattern stays a bare name
        return string.IsNullOrEmpty(directory)
          ? files.Select(Path.GetFileName).ToArray()
          : files;
      } catch (Exception) {
        // an unusable pattern simply matches nothing; the parser reports the empty run
        return new string[0];
      }
    }
  }
}
