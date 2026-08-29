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
using System.Drawing;
using System.Drawing.Extensions.ColorProcessing.Resizing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Classes {
  /// <summary>
  /// The settings that survive between sessions.
  /// <para>
  /// Each one is declared once in <see cref="_SETTINGS"/> with the pair of conversions it needs;
  /// loading and saving both walk that table, so the two can never disagree about a name and
  /// adding a setting is one entry rather than two matching branches.
  /// </para>
  /// <para>
  /// Anything unreadable is skipped rather than fatal: a configuration written by a newer build,
  /// or hand-edited into nonsense, must not stop the application from starting.
  /// </para>
  /// </summary>
  internal static class Config {

    #region consts

    private const string _ROOT_NODE_NAME = "Configuration";
    private const string _VALUE_ATTRIBUTE_NAME = "value";

    #endregion

    #region props

    public static string LastSaveDirectory { get; set; }
    public static string LastLoadDirectory { get; set; }
    public static PictureBoxSizeMode? SourceSizeMode { get; set; }
    public static PictureBoxSizeMode? TargetSizeMode { get; set; }

    /// <summary>The window's restored position and size, so it comes back where it was left.</summary>
    public static Rectangle? WindowBounds { get; set; }

    /// <summary>Whether the window was maximized. Never <see cref="FormWindowState.Minimized"/>.</summary>
    public static FormWindowState? WindowState { get; set; }

    /// <summary>The registered name of the last used method, e.g. <c>"Upscaler: HQ 2x"</c>.</summary>
    public static string ResizeMethod { get; set; }

    /// <summary>The last selected method category, or <c>null</c> for all of them.</summary>
    public static string MethodCategory { get; set; }

    public static OutOfBoundsMode? HorizontalBph { get; set; }
    public static OutOfBoundsMode? VerticalBph { get; set; }
    public static bool? UseThresholds { get; set; }
    public static bool? UseCenteredGrid { get; set; }
    public static bool? KeepAspect { get; set; }
    public static int? RepetitionCount { get; set; }
    public static float? Radius { get; set; }

    #endregion

    #region the table

    /// <summary>One persisted setting: its element name and how it converts to and from text.</summary>
    private sealed class Setting {
      public string Name { get; }

      /// <summary>Renders the current value, or <c>null</c> when there is nothing to write.</summary>
      public Func<string> Read { get; }

      /// <summary>Applies a value read back from the file. Only called with non-empty text.</summary>
      public Action<string> Write { get; }

      public Setting(string name, Func<string> read, Action<string> write) {
        this.Name = name;
        this.Read = read;
        this.Write = write;
      }
    }

    private static string _FromEnum<TEnum>(TEnum? value) where TEnum : struct
      => value?.ToString()
    ;

    private static void _ToEnum<TEnum>(string text, Action<TEnum?> assign) where TEnum : struct {
      if (Enum.TryParse<TEnum>(text, true, out var parsed))
        assign(parsed);
    }

    private static string _FromBool(bool? value)
      => value?.ToString(CultureInfo.InvariantCulture)
    ;

    private static void _ToBool(string text, Action<bool?> assign) {
      if (bool.TryParse(text, out var parsed))
        assign(parsed);
    }

    private static string _FromRectangle(Rectangle? value)
      => value == null
        ? null
        : string.Join(",", new[] { value.Value.X, value.Value.Y, value.Value.Width, value.Value.Height }.Select(v => v.ToString(CultureInfo.InvariantCulture)))
    ;

    private static void _ToRectangle(string text, Action<Rectangle?> assign) {
      var parts = text.Split(',');
      if (parts.Length != 4)
        return;

      var numbers = new int[4];
      for (var i = 0; i < 4; ++i)
        if (!int.TryParse(parts[i].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out numbers[i]))
          return;

      // a window with no extent is not something to restore
      if (numbers[2] <= 0 || numbers[3] <= 0)
        return;

      assign(new Rectangle(numbers[0], numbers[1], numbers[2], numbers[3]));
    }

    private static readonly Setting[] _SETTINGS = {
      new Setting("LastSaveDirectory", () => LastSaveDirectory, v => LastSaveDirectory = v),
      new Setting("LastLoadDirectory", () => LastLoadDirectory, v => LastLoadDirectory = v),
      new Setting("SourceSizeMode", () => _FromEnum(SourceSizeMode), v => _ToEnum<PictureBoxSizeMode>(v, p => SourceSizeMode = p)),
      new Setting("TargetSizeMode", () => _FromEnum(TargetSizeMode), v => _ToEnum<PictureBoxSizeMode>(v, p => TargetSizeMode = p)),
      new Setting("WindowBounds", () => _FromRectangle(WindowBounds), v => _ToRectangle(v, r => WindowBounds = r)),
      new Setting("WindowState", () => _FromEnum(WindowState), v => _ToEnum<FormWindowState>(v, p => WindowState = p == FormWindowState.Minimized ? FormWindowState.Normal : p)),
      new Setting("ResizeMethod", () => ResizeMethod, v => ResizeMethod = v),
      new Setting("MethodCategory", () => MethodCategory, v => MethodCategory = v),
      new Setting("HorizontalBph", () => _FromEnum(HorizontalBph), v => _ToEnum<OutOfBoundsMode>(v, p => HorizontalBph = p)),
      new Setting("VerticalBph", () => _FromEnum(VerticalBph), v => _ToEnum<OutOfBoundsMode>(v, p => VerticalBph = p)),
      new Setting("UseThresholds", () => _FromBool(UseThresholds), v => _ToBool(v, p => UseThresholds = p)),
      new Setting("UseCenteredGrid", () => _FromBool(UseCenteredGrid), v => _ToBool(v, p => UseCenteredGrid = p)),
      new Setting("KeepAspect", () => _FromBool(KeepAspect), v => _ToBool(v, p => KeepAspect = p)),
      new Setting("RepetitionCount", () => RepetitionCount?.ToString(CultureInfo.InvariantCulture), v => {
        if (int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) && parsed > 0)
          RepetitionCount = parsed;
      }),
      new Setting("Radius", () => Radius?.ToString("R", CultureInfo.InvariantCulture), v => {
        if (float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) && parsed > 0)
          Radius = parsed;
      }),
    };

    #endregion

    /// <summary>
    /// Loads the configuration from the specified file. A missing or foreign file leaves every
    /// setting at its default.
    /// </summary>
    /// <param name="configurationFile">The configuration file.</param>
    public static void Load(string configurationFile) {
      if (string.IsNullOrWhiteSpace(configurationFile) || !File.Exists(configurationFile))
        return;

      var root = XElement.Load(configurationFile);

      // if root node is different, skip loading
      if (!string.Equals(root.Name.LocalName, _ROOT_NODE_NAME, StringComparison.CurrentCultureIgnoreCase))
        return;

      var byName = _SETTINGS.ToDictionary(setting => setting.Name, StringComparer.OrdinalIgnoreCase);

      foreach (var node in root.Elements()) {
        var value = node.Attribute(_VALUE_ATTRIBUTE_NAME)?.Value;
        if (string.IsNullOrWhiteSpace(value))
          continue;

        // an element this build does not know is left alone, not an error
        if (byName.TryGetValue(node.Name.LocalName, out var setting))
          setting.Write(value);
      }
    }

    /// <summary>
    /// Saves the configuration to the specified file.
    /// </summary>
    /// <param name="configurationFile">The configuration file.</param>
    public static void Save(string configurationFile) {
      var root = new XElement(_ROOT_NODE_NAME);
      foreach (var setting in _SETTINGS)
        root.Add(new XElement(setting.Name, new XAttribute(_VALUE_ATTRIBUTE_NAME, setting.Read() ?? string.Empty)));

      root.Save(configurationFile);
    }

    /// <summary>
    /// Forgets every setting. Exists for tests, which share this static state between cases.
    /// </summary>
    public static void Reset() {
      LastSaveDirectory = LastLoadDirectory = ResizeMethod = MethodCategory = null;
      SourceSizeMode = TargetSizeMode = null;
      WindowBounds = null;
      WindowState = null;
      HorizontalBph = VerticalBph = null;
      UseThresholds = UseCenteredGrid = KeepAspect = null;
      RepetitionCount = null;
      Radius = null;
    }
  }
}
