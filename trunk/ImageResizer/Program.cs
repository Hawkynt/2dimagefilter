using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ImageResizer {
  class Program {
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
      if (args != null && args.Length > 0) {

        // execute CLI
        Environment.Exit(CLI.ParseCommandLineArguments(args));

      } else {
        var consoleHandle = _GetConsoleWindow();

        // run GUI
        if (consoleHandle == IntPtr.Zero || AppDomain.CurrentDomain.FriendlyName.Contains(".vshost")) {

          // we either have no console window or we're started from within visual studio
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);

          Application.Run(new MainForm());
        } else {

          // we found a console attached to us, so restart ourselves without one
          Process.Start(new ProcessStartInfo(Assembly.GetEntryAssembly().Location) {
            CreateNoWindow = true,
            UseShellExecute = false
          });
        }
      }

    }
  }
}
