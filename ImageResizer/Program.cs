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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ImageResizer {
  class Program {
    /// <summary>
    /// This is the command line parameter that force the app to run in GUI mode
    /// </summary>
    private const string _FORCE_GUI_CLP_NAME = "/FORCEGUI";

    [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow")]
    private static extern IntPtr _GetConsoleWindow();

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
      if (args != null && args.Length > 0 && args[0] != _FORCE_GUI_CLP_NAME) {

        // execute CLI
        Environment.Exit(CLI.ParseCommandLineArguments(args));

      } else {
        var consoleHandle = _GetConsoleWindow();

        // run GUI
        if (consoleHandle == IntPtr.Zero || AppDomain.CurrentDomain.FriendlyName.Contains(".vshost") || (args != null && args.Length > 0 && args[0] == _FORCE_GUI_CLP_NAME)) {

          // we either have no console window or we're started from within visual studio or we are forced into GUI mode
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);

          Application.Run(new MainForm());
        } else {

          // we found a console attached to us, so restart ourselves without one
          Process.Start(new ProcessStartInfo(Assembly.GetEntryAssembly().Location, _FORCE_GUI_CLP_NAME) {
            CreateNoWindow = true,
            UseShellExecute = false
          });
        }
      }

    }
  }
}
