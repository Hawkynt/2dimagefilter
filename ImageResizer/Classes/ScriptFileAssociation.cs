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
using System.Runtime.InteropServices;

using Microsoft.Win32;

namespace Classes {
  /// <summary>
  /// Registers <c>.irs</c> scripts with this executable, so double clicking one runs it.
  /// <para>
  /// Everything is written under <c>HKEY_CURRENT_USER\Software\Classes</c>, which needs no
  /// elevation and affects only the current user. The registry root is a parameter so this can be
  /// exercised against a scratch key instead of the user's real associations.
  /// </para>
  /// </summary>
  internal static class ScriptFileAssociation {

    /// <summary>The identifier the extension points at.</summary>
    public const string PROGRAM_IDENTIFIER = "ImageResizer.Script";

    /// <summary>What Explorer shows as the file type.</summary>
    public const string FILE_TYPE_DESCRIPTION = "ImageResizer Script";

    private const string _USER_CLASSES = @"Software\Classes";
    private const string _COMMAND_PATH = PROGRAM_IDENTIFIER + @"\shell\open\command";
    private const string _ICON_PATH = PROGRAM_IDENTIFIER + @"\DefaultIcon";

    #region shell notification

    private const int _SHCNE_ASSOCCHANGED = 0x08000000;
    private const int _SHCNF_IDLIST = 0x0000;

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern void SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

    /// <summary>
    /// Tells Explorer the associations changed, so the new icon and verb appear without a logoff.
    /// </summary>
    public static void NotifyShell() {
      try {
        SHChangeNotify(_SHCNE_ASSOCCHANGED, _SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
      } catch (Exception) {
        // cosmetic only - the association itself is already written
      }
    }

    #endregion

    /// <summary>
    /// Opens the per-user class registrations.
    /// </summary>
    /// <returns>The key; the caller disposes it.</returns>
    public static RegistryKey OpenUserClasses() => Registry.CurrentUser.CreateSubKey(_USER_CLASSES);

    /// <summary>
    /// The command line Explorer runs for a script.
    /// </summary>
    /// <param name="executablePath">The executable.</param>
    /// <returns>The command, with the quoted file placeholder.</returns>
    public static string BuildCommand(string executablePath) => "\"" + executablePath + "\" \"%1\"";

    /// <summary>
    /// Determines whether scripts are currently associated with an executable.
    /// </summary>
    /// <param name="classesRoot">The class registrations to look in.</param>
    /// <param name="executablePath">The executable to check for.</param>
    /// <returns><c>true</c> when the extension resolves to this executable.</returns>
    public static bool IsRegistered(RegistryKey classesRoot, string executablePath) {
      if (classesRoot == null || string.IsNullOrWhiteSpace(executablePath))
        return false;

      using (var extension = classesRoot.OpenSubKey(ScriptSerializer.DEFAULT_FILE_EXTENSION)) {
        if (!string.Equals(extension?.GetValue(null) as string, PROGRAM_IDENTIFIER, StringComparison.OrdinalIgnoreCase))
          return false;
      }

      using (var command = classesRoot.OpenSubKey(_COMMAND_PATH))
        return string.Equals(command?.GetValue(null) as string, BuildCommand(executablePath), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Associates scripts with an executable.
    /// </summary>
    /// <param name="classesRoot">The class registrations to write to.</param>
    /// <param name="executablePath">The executable to point at.</param>
    public static void Register(RegistryKey classesRoot, string executablePath) {
      if (classesRoot == null || string.IsNullOrWhiteSpace(executablePath))
        return;

      using (var extension = classesRoot.CreateSubKey(ScriptSerializer.DEFAULT_FILE_EXTENSION))
        extension?.SetValue(null, PROGRAM_IDENTIFIER);

      using (var program = classesRoot.CreateSubKey(PROGRAM_IDENTIFIER))
        program?.SetValue(null, FILE_TYPE_DESCRIPTION);

      using (var icon = classesRoot.CreateSubKey(_ICON_PATH))
        icon?.SetValue(null, executablePath + ",0");

      using (var command = classesRoot.CreateSubKey(_COMMAND_PATH))
        command?.SetValue(null, BuildCommand(executablePath));
    }

    /// <summary>
    /// Removes the association.
    /// <para>
    /// Only the extension entry this code owns is removed, and only when it still points at our
    /// identifier - another program may have taken the extension over since, and stealing it back
    /// on the way out would be worse than leaving it.
    /// </para>
    /// </summary>
    /// <param name="classesRoot">The class registrations to clean.</param>
    public static void Unregister(RegistryKey classesRoot) {
      if (classesRoot == null)
        return;

      using (var extension = classesRoot.OpenSubKey(ScriptSerializer.DEFAULT_FILE_EXTENSION))
        if (string.Equals(extension?.GetValue(null) as string, PROGRAM_IDENTIFIER, StringComparison.OrdinalIgnoreCase))
          classesRoot.DeleteSubKeyTree(ScriptSerializer.DEFAULT_FILE_EXTENSION, false);

      classesRoot.DeleteSubKeyTree(PROGRAM_IDENTIFIER, false);
    }
  }
}
