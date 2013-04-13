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
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

using Classes.ScriptActions;

using ImageResizer.Properties;

namespace Classes {
  /// <summary>
  /// The command line interface for the application.
  /// </summary>
  internal static class CLI {

    /// <summary>
    /// Parses the command line arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    public static CLIExitCode ParseCommandLineArguments(string[] arguments) {
      if (arguments == null || arguments.Length < 1)
        return (CLIExitCode.OK);

      var engine = new ScriptEngine();
      var line = string.Join(" ", arguments.Select(a => string.Format(@"""{0}""", a)));
      Console.WriteLine("Executing the following script:");
      Console.WriteLine(line);
      Console.WriteLine();

      // load script from command line parameters
      try {
        ScriptSerializer.LoadFromString(engine, line);
      } catch (ScriptSerializerException e) {
        _ShowHelp();
        return (e.ErrorType);
      }

      // execute script
      try {
        engine.RepeatActions(_PreAction, _PostAction);
      } catch (Exception e) {
        Console.WriteLine(e.Message);
        return (CLIExitCode.RuntimeError);
      }

      return (CLIExitCode.OK);
    }

    private static void _PreAction(ScriptEngine engine, IScriptAction command) {
      var loadCommand = command as LoadFileCommand;
      if (loadCommand != null) {
        Console.WriteLine("Loading from file " + loadCommand.FileName);
        return;
      }

      var saveCommand = command as SaveFileCommand;
      if (saveCommand != null) {
        Console.WriteLine("Saving to file " + saveCommand.FileName);
      }

      var resizeCommand = command as ResizeCommand;
      if (resizeCommand != null) {
        Console.WriteLine("Applying filter     : {0}", SupportedManipulators.MANIPULATORS.First(k => k.Value == resizeCommand.Manipulator).Key);
        Console.WriteLine("  Target percentage : {0}", (resizeCommand.Percentage == 0 ? "auto" : resizeCommand.Percentage + "%"));
        Console.WriteLine("  Target width      : {0}", (resizeCommand.Width == 0 ? "auto" : resizeCommand.Width + "pixels"));
        Console.WriteLine("  Target height     : {0}", (resizeCommand.Height == 0 ? "auto" : resizeCommand.Height + "pixels"));
        Console.WriteLine("  Hori. BPH         : {0}", resizeCommand.HorizontalBph);
        Console.WriteLine("  Vert. BPH         : {0}", resizeCommand.VerticalBph);
        Console.WriteLine("  Use Thresholds    : {0}", resizeCommand.UseThresholds);
        Console.WriteLine("  Centered Grid     : {0}", resizeCommand.UseCenteredGrid);
        Console.WriteLine("  Radius            : {0}", resizeCommand.Radius);
        Console.WriteLine("  Repeat            : {0} times", resizeCommand.Count);
      }

    }

    private static void _PostAction(ScriptEngine engine, IScriptAction command) {

      var loadCommand = command as LoadFileCommand;
      if (loadCommand != null) {
        Console.WriteLine("  File   : {0} Bytes", new FileInfo(loadCommand.FileName).Length);
        Console.WriteLine("  Width  : {0} Pixel", engine.SourceImage.Width);
        Console.WriteLine("  Height : {0} Pixel", engine.SourceImage.Height);
        Console.WriteLine("  Size   : {0:0.00} MegaPixel", (engine.SourceImage.Width * engine.SourceImage.Height / 1000000.0));
        Console.WriteLine("  Type   : {0}", ImageCodecInfo.GetImageDecoders().First(d => d.FormatID == engine.GdiSource.RawFormat.Guid).FormatDescription);
        Console.WriteLine("  Format : {0}", engine.GdiSource.PixelFormat);
        return;
      }

      var saveCommand = command as SaveFileCommand;
      if (saveCommand != null) {
        var reloadedImage = Image.FromFile(saveCommand.FileName);
        Console.WriteLine("  File   : {0} Bytes", new FileInfo(saveCommand.FileName).Length);
        Console.WriteLine("  Width  : {0} Pixel", reloadedImage.Width);
        Console.WriteLine("  Height : {0} Pixel", reloadedImage.Height);
        Console.WriteLine("  Size   : {0:0.00} MegaPixel", (reloadedImage.Width * reloadedImage.Height / 1000000.0));
        Console.WriteLine("  Type   : {0}", ImageCodecInfo.GetImageDecoders().First(d => d.FormatID == reloadedImage.RawFormat.Guid).FormatDescription);
        Console.WriteLine("  Format : {0}", reloadedImage.PixelFormat);
      }
    }

    /// <summary>
    /// Shows the CLI help.
    /// </summary>
    private static void _ShowHelp() {

      var longestFilterNameLength = SupportedManipulators.MANIPULATORS.Select(k => k.Key.Length).Max();

      // we're loading the help text as a template from an internal resource and then filling out the fields
      var lines = Resources.CLIHelpText
        .Replace("{appname}", ReflectionUtils.GetEntryAssemblyAttribute<AssemblyProductAttribute>(p => p.Product).ToString())
        .Replace("{version}", ReflectionUtils.GetEntryAssemblyAttribute<AssemblyFileVersionAttribute>(v => v.Version).ToString())
        .Replace("{copyright}", ReflectionUtils.GetEntryAssemblyAttribute<AssemblyCopyrightAttribute>(c => c.Copyright).ToString())
        .Replace("{location}", Path.GetFileName(Assembly.GetEntryAssembly().Location))
        .Replace("{filterlist}", string.Join(Environment.NewLine,
          from i in SupportedManipulators.MANIPULATORS
          let d = ReflectionUtils.GetDescriptionForClass(i.Value.GetType())
          group i by d into g
          select string.Format("{0}{1}:", g.Key, _GetSupportedParameterStringFromManipulator(g.First().Value)) + Environment.NewLine + string.Join(
            Environment.NewLine,
            g.Select(j => string.Format("{0,-" + longestFilterNameLength + "}", j.Key))
          ) + Environment.NewLine)
        )
        .Replace("{centered}", ScriptSerializer.CENTERED_GRID_PARAMETER_NAME)
        .Replace("{repeat}", ScriptSerializer.REPEAT_PARAMETER_NAME)
        .Replace("{thresholds}", ScriptSerializer.THRESHOLDS_PARAMETER_NAME)
        .Replace("{radius}", ScriptSerializer.RADIUS_PARAMETER_NAME)
        .Replace("{vbounds}", ScriptSerializer.VBOUNDS_PARAMETER_NAME)
        .Replace("{hbounds}", ScriptSerializer.HBOUNDS_PARAMETER_NAME)
        .Replace("{save}", ScriptSerializer.SAVE_COMMAND_NAME)
        .Replace("{load}", ScriptSerializer.LOAD_COMMAND_NAME)
        .Replace("{script}", ScriptSerializer.SCRIPT_COMMAND_NAME)
        .Replace("{resize}", ScriptSerializer.RESIZE_COMMAND_NAME)
        .Replace("{stdin}", ScriptSerializer.STDIN_COMMAND_NAME)
        .Replace("{stdout}", ScriptSerializer.STDOUT_COMMAND_NAME)
        ;
      Console.WriteLine(lines);
    }

    /// <summary>
    /// Gets the list of supported parameters for the given manipulator.
    /// </summary>
    /// <param name="manipulator">The manipulator.</param>
    /// <returns>A text representing the supported parameters.</returns>
    private static string _GetSupportedParameterStringFromManipulator(IImageManipulator manipulator) {
      if (manipulator == null)
        return (null);

      var result = new List<string>();

      if (manipulator.SupportsWidth)
        result.Add("width");

      if (manipulator.SupportsHeight)
        result.Add("height");

      if (manipulator.SupportsRepetitionCount)
        result.Add(ScriptSerializer.REPEAT_PARAMETER_NAME);

      if (manipulator.SupportsThresholds)
        result.Add(ScriptSerializer.THRESHOLDS_PARAMETER_NAME);

      if (manipulator.SupportsRadius)
        result.Add(ScriptSerializer.RADIUS_PARAMETER_NAME);

      if (manipulator.SupportsGridCentering)
        result.Add(ScriptSerializer.CENTERED_GRID_PARAMETER_NAME);

      return (result.Count < 1 ? null : " (" + string.Join(", ", result) + ")");
    }

    /// <summary>
    /// Saves an image and adjust jpeg quality if saving to jpeg.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="image">The image.</param>
    /// <returns><c>true</c> on success; otherwise, <c>false</c>.</returns>
    internal static CLIExitCode SaveHelper(string filename, Image image) {
      Contract.Requires(filename != null);

      if (image == null)
        return (CLIExitCode.NothingToSave);

      var extension = Path.GetExtension(filename);
      if (extension != null)
        extension = extension.ToUpperInvariant();

      try {
        if (extension == ".JPG" || extension == ".JPEG") {
          var codecs = ImageCodecInfo.GetImageEncoders();
          codecs = codecs.Where(info => info != null && info.MimeType == "image/jpeg").ToArray();
          if (codecs.Length <= 0) {
            return (CLIExitCode.JpegNotSupportedOnThisPlatform);
          }
          Contract.Assume(Encoder.Quality != null);
          image.Save(filename, codecs[0], new EncoderParameters {
            Param = new[] {
              new EncoderParameter(Encoder.Quality, (long)100)
            }
          });
        } else if (extension == ".BMP")
          image.Save(filename, ImageFormat.Bmp);
        else if (extension == ".GIF")
          image.Save(filename, ImageFormat.Gif);
        else if (extension == ".TIF")
          image.Save(filename, ImageFormat.Tiff);
        else
          image.Save(filename, ImageFormat.Png);
      } catch (Exception) {
        return (CLIExitCode.ExceptionDuringImageWrite);
      }

      return (CLIExitCode.OK);
    }

  }
}
