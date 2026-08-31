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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Classes;


namespace ImageResizer {
  class Program {
    #region consts

    /// <summary>
    /// This is the command line parameter that force the app to run in GUI mode
    /// </summary>
    private const string _FORCE_GUI_CLP_NAME = "/FORCEGUI";

    /// <summary>
    /// Generates the deterministic documentation screenshot and exits.
    /// </summary>
    private const string _SCREENSHOT_CLP_NAME = "--screenshot";

    /// <summary>
    /// The name and full path to the currently running executable.
    /// </summary>
    private static readonly string _THIS_EXECUTABLES_FILE_NAME = Assembly.GetEntryAssembly().Location;

    /// <summary>
    /// This is the name of the configuration file.
    /// </summary>
    private static readonly string _CONFIGURATION_FILE_NAME = Path.Combine(Path.GetDirectoryName(_THIS_EXECUTABLES_FILE_NAME), "config.xml");

    #endregion

    #region imports
    [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow")]
    private static extern IntPtr _GetConsoleWindow();
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args) {

      /*
       * This works as following:
       * First we look for command line parameters and if there are any of them present, we run the CLI version.
       * If there are no parameters, we try to find out if we are run inside a console and if so, we spawn a new copy of ourselves without a console.
       * If there is no console at all, we show the GUI.
       * This way we're both a CLI and a GUI.
       */

      var screenshotIndex = args == null ? -1 : Array.IndexOf(args, _SCREENSHOT_CLP_NAME);
      if (screenshotIndex >= 0) {
        var outputPath = screenshotIndex + 1 < args.Length ? args[screenshotIndex + 1] : "screenshot.png";
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ScreenshotCapture.Save(outputPath);
        Environment.Exit((int)CLIExitCode.OK);
        return;
      }

      var firstParam = args != null && args.Length > 0 ? args[0] : null;
      var fileToOpenOnStart = firstParam != _FORCE_GUI_CLP_NAME && File.Exists(firstParam) ? firstParam : null;

      // Scripts dropped on the executable, or opened through a file association, arrive as plain
      // file names - the same shape as an image to open. They are something to run, so they take
      // the CLI path with each one turned into a /script command.
      if (_AreAllScriptFiles(args)) {
        var scriptArguments = _ToScriptArguments(args);
        _EnterScriptDirectory(args[0]);
        Environment.Exit((int)CLI.ParseCommandLineArguments(scriptArguments));
      }

      if (firstParam != null && firstParam != _FORCE_GUI_CLP_NAME && fileToOpenOnStart == null) {

        // execute CLI if arguments are given which are not forcing into gui or a valid filename
        var result = CLI.ParseCommandLineArguments(args);
        Environment.Exit((int)result);
      } else {
        var consoleHandle = _GetConsoleWindow();

        // run GUI
        var host = AppDomain.CurrentDomain.FriendlyName;
        if (consoleHandle == IntPtr.Zero || AppDomain.CurrentDomain.FriendlyName.Contains(".vshost") || firstParam == _FORCE_GUI_CLP_NAME || fileToOpenOnStart != null) {

          // we either have no console window or we're started from within visual studio or we are forced into GUI mode
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);
          Config.Load(_CONFIGURATION_FILE_NAME);
          Application.Run(new MainForm(fileToOpenOnStart));
          Config.Save(_CONFIGURATION_FILE_NAME);
          Environment.Exit((int)CLIExitCode.OK);
        } else {

          // we found a console attached to us, so restart ourselves without one
          Process.Start(new ProcessStartInfo(_THIS_EXECUTABLES_FILE_NAME, _FORCE_GUI_CLP_NAME) {
            CreateNoWindow = true,
            UseShellExecute = false
          });
          Environment.Exit((int)CLIExitCode.RestartingInGuiMode);
        }
      }

    }

    /// <summary>
    /// Determines whether every argument names an existing script file.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns><c>true</c> when there is at least one and all of them are scripts.</returns>
    private static bool _AreAllScriptFiles(string[] args)
      => args != null && args.Length > 0 && args.All(_IsScriptFile)
    ;

    /// <summary>
    /// Determines whether a path names an existing script file.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns><c>true</c> when it exists and carries the script extension.</returns>
    private static bool _IsScriptFile(string path)
      => !string.IsNullOrWhiteSpace(path)
      && string.Equals(Path.GetExtension(path), ScriptSerializer.DEFAULT_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase)
      && File.Exists(path)
    ;

    /// <summary>
    /// Turns a list of script files into the command line that runs them in order.
    /// <para>
    /// The paths are made absolute here because <see cref="_EnterScriptDirectory"/> moves the
    /// working directory afterwards, and a relative path would then point somewhere else.
    /// </para>
    /// </summary>
    /// <param name="paths">The script files.</param>
    /// <returns>The arguments.</returns>
    private static string[] _ToScriptArguments(string[] paths) {
      var result = new List<string>(paths.Length * 2);
      foreach (var path in paths) {
        result.Add(ScriptSerializer.SCRIPT_COMMAND_NAME);
        result.Add(Path.GetFullPath(path));
      }

      return result.ToArray();
    }

    /// <summary>
    /// Makes a script's own folder the working directory.
    /// <para>
    /// A script names its images relatively, and the working directory it inherits when Explorer
    /// launches it is not its own - dropping a script on the executable gives the executable's
    /// folder. Resolving against the script instead is what makes a script and its images travel
    /// together, and is how Explorer runs a batch file.
    /// </para>
    /// </summary>
    /// <param name="scriptPath">The script being run.</param>
    private static void _EnterScriptDirectory(string scriptPath) {
      try {
        var directory = Path.GetDirectoryName(Path.GetFullPath(scriptPath));
        if (!string.IsNullOrEmpty(directory))
          Directory.SetCurrentDirectory(directory);
      } catch (Exception) {
        // an unusable path is the script runner's problem to report, not ours
      }
    }
  }
}
