#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

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
using System.Linq.Expressions;
using System.Reflection;

using Classes.ScriptActions;

using ImageResizer.Properties;

namespace Classes {
  /// <summary>
  /// The command line interface for the application.
  /// </summary>
  internal static class CLI {

    /// <summary>
    /// The arguments that ask for the help text rather than for work.
    /// </summary>
    private static readonly string[] _HELP_ARGUMENTS = { "/?", "-?", "--?", "/h", "-h", "/help", "--help" };

    /// <summary>
    /// Where progress reports and errors go.
    /// <para>
    /// Never standard output: <see cref="ScriptActions.SaveStdOutCommand"/> writes a PNG there, and
    /// anything else on that stream corrupts the image for whatever is on the other end of the
    /// pipe. Standard output carries image data and nothing else.
    /// </para>
    /// </summary>
    private static TextWriter _Diagnostics => Console.Error;

    /// <summary>
    /// Parses the command line arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    public static CLIExitCode ParseCommandLineArguments(string[] arguments) {
      if (arguments == null || arguments.Length < 1)
        return CLIExitCode.OK;

      // only the leading argument, so a file that happens to be named like a switch stays a file
      if (_HELP_ARGUMENTS.Contains(arguments[0], StringComparer.OrdinalIgnoreCase)) {
        // help that was asked for is the output of the run, so it goes to standard output
        _ShowHelp(Console.Out);
        return CLIExitCode.OK;
      }

      var engine = new ScriptEngine();
      var line = string.Join(" ", arguments.Select(a => string.Format(@"""{0}""", a)));
      _Diagnostics.WriteLine("Executing the following script:");
      _Diagnostics.WriteLine(line);
      _Diagnostics.WriteLine();

      // load script from command line parameters
      try {
        ScriptSerializer.LoadFromString(engine, line);
      } catch (ScriptSerializerException e) {
        _ShowError(e);
        _ShowHelp(_Diagnostics);
        return e.ErrorType;
      }

      // execute script
      try {
        engine.RepeatActions(_PreAction, _PostAction);
      } catch (Exception e) {
        _Diagnostics.WriteLine(e.Message);
        return CLIExitCode.RuntimeError;
      }

      return CLIExitCode.OK;
    }

    private static void _PreAction(ScriptEngine engine, IScriptAction command) {
      switch (command) {
        case LoadFileCommand loadCommand:
          _Diagnostics.WriteLine("Loading from file " + loadCommand.FileName);
          return;
        case SaveFileCommand saveCommand:
          _Diagnostics.WriteLine("Saving to file " + saveCommand.FileName);
          break;
        case ResizeCommand resizeCommand:
          _Diagnostics.WriteLine("Applying filter     : {0}", _GetManipulatorName(resizeCommand.Manipulator));
          _Diagnostics.WriteLine("  Target percentage : {0}", resizeCommand.Percentage == 0 ? "auto" : resizeCommand.Percentage + "%");
          _Diagnostics.WriteLine("  Target width      : {0}", resizeCommand.Width == 0 ? "auto" : resizeCommand.Width + "pixels");
          _Diagnostics.WriteLine("  Target height     : {0}", resizeCommand.Height == 0 ? "auto" : resizeCommand.Height + "pixels");
          _Diagnostics.WriteLine("  Hori. BPH         : {0}", resizeCommand.HorizontalBph);
          _Diagnostics.WriteLine("  Vert. BPH         : {0}", resizeCommand.VerticalBph);
          _Diagnostics.WriteLine("  Use Thresholds    : {0}", resizeCommand.UseThresholds);
          _Diagnostics.WriteLine("  Centered Grid     : {0}", resizeCommand.UseCenteredGrid);
          _Diagnostics.WriteLine("  Radius            : {0}", resizeCommand.Radius);
          _Diagnostics.WriteLine("  Repeat            : {0} times", resizeCommand.Count);
          break;
      }
    }

    private static void _PostAction(ScriptEngine engine, IScriptAction command) {
      switch (command) {
        case LoadFileCommand loadCommand:
          _PrintImageFileDetails(loadCommand.FileName);
          return;
        case SaveFileCommand saveCommand:
          _PrintImageFileDetails(saveCommand.FileName);
          break;
      }
    }

    /// <summary>
    /// Prints size and format details of an image file.
    /// <para>
    /// The details are read back from the file rather than from the engine's bitmaps on purpose:
    /// the engine holds in-memory copies whose <see cref="Image.RawFormat"/> is
    /// <see cref="ImageFormat.MemoryBmp"/>, and GDI+ registers no decoder for that - looking one
    /// up used to throw and take the whole run down with it.
    /// </para>
    /// <para>
    /// This is diagnostic output; it must never be able to abort a pipeline, hence the blanket
    /// catch.
    /// </para>
    /// </summary>
    /// <param name="fileName">The image file to describe.</param>
    private static void _PrintImageFileDetails(string fileName) {
      try {
        _Diagnostics.WriteLine("  File   : {0} Bytes", new FileInfo(fileName).Length);

        // no validation and no embedded colour management - we only want the header
        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var image = Image.FromStream(stream, false, false)) {
          _Diagnostics.WriteLine("  Width  : {0} Pixel", image.Width);
          _Diagnostics.WriteLine("  Height : {0} Pixel", image.Height);
          _Diagnostics.WriteLine("  Size   : {0:0.00} MegaPixel", image.Width * image.Height / 1000000.0);
          _Diagnostics.WriteLine("  Type   : {0}", _GetFormatDescription(image.RawFormat));
          _Diagnostics.WriteLine("  Format : {0}", image.PixelFormat);
        }
      } catch (Exception e) {
        _Diagnostics.WriteLine("  (details unavailable: {0})", e.Message);
      }
    }

    /// <summary>
    /// Gets the human-readable description of an image format, e.g. <c>"PNG"</c>.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>The decoder's description, or the format's own name when no decoder knows it.</returns>
    private static string _GetFormatDescription(ImageFormat format)
      => ImageCodecInfo.GetImageDecoders().FirstOrDefault(d => d.FormatID == format.Guid)?.FormatDescription ?? format.ToString()
    ;

    /// <summary>
    /// Gets the registered name of a manipulator.
    /// </summary>
    /// <param name="manipulator">The manipulator.</param>
    /// <returns>The name it is registered under, or its description when it is not registered.</returns>
    private static string _GetManipulatorName(IImageManipulator manipulator) {
      foreach (var pair in SupportedManipulators.MANIPULATORS)
        if (ReferenceEquals(pair.Value, manipulator))
          return pair.Key;

      return ReflectionUtils.GetDescriptionForClass(manipulator.GetType());
    }

    /// <summary>
    /// Explains why a script could not be parsed. Without this the CLI answered every malformed
    /// command line with nothing but the help text, leaving the user to guess which token it
    /// choked on.
    /// </summary>
    /// <param name="exception">The parse failure.</param>
    private static void _ShowError(ScriptSerializerException exception) {
      var origin = exception.Filename == null
        ? string.Empty
        : string.Format(" in {0}, line {1}", exception.Filename, exception.LineNumber)
        ;

      _Diagnostics.WriteLine("ERROR{0}: {1}", origin, _GetErrorText(exception.ErrorType));
      _Diagnostics.WriteLine();
    }

    /// <summary>
    /// Gets a human-readable explanation for an exit code.
    /// </summary>
    /// <param name="exitCode">The exit code.</param>
    /// <returns>The explanation.</returns>
    private static string _GetErrorText(CLIExitCode exitCode) {
      switch (exitCode) {
        case CLIExitCode.UnknownParameter:
          return "Unknown command or filter parameter - see the list of supported filter methods below.";
        case CLIExitCode.TooLessArguments:
          return "A command is missing arguments.";
        case CLIExitCode.FilenameMustNotBeNull:
          return "A file name is missing.";
        case CLIExitCode.InvalidTargetDimensions:
          return "Invalid target dimensions - expected auto, w<x>, h<y>, <x>x<y> or <p>%.";
        case CLIExitCode.CouldNotParseDimensionsAsWord:
          return "Target dimensions out of range - width, height and percentage must be 0-65535.";
        case CLIExitCode.UnknownFilter:
          return "Unknown filter method - see the list of supported filter methods below.";
        case CLIExitCode.AmbiguousFilter:
          return "Ambiguous filter method - more than one category provides it, so prefix it with the category shown in the list below.";
        case CLIExitCode.InvalidFilterDescription:
          return "Invalid filter description - expected <method>[(<repeat>|<paramlist>)].";
        case CLIExitCode.CouldNotParseParameterAsFloat:
          return "A filter parameter that must be a floating point value is not one.";
        case CLIExitCode.CouldNotParseParameterAsByte:
          return "A filter parameter that must be a value 0-255 is not one.";
        case CLIExitCode.InvalidOutOfBoundsMode:
          return "Invalid out of bounds mode - expected const, half, whole, wrap or transparent.";
        default:
          return exitCode.ToString();
      }
    }

    /// <summary>
    /// Shows the CLI help.
    /// </summary>
    /// <param name="writer">Where to write it: standard output when the user asked for help,
    /// <see cref="_Diagnostics"/> when it accompanies an error.</param>
    private static void _ShowHelp(TextWriter writer) {

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
      writer.WriteLine(lines);
    }

    /// <summary>
    /// Gets the list of supported parameters for the given manipulator.
    /// </summary>
    /// <param name="manipulator">The manipulator.</param>
    /// <returns>A text representing the supported parameters.</returns>
    private static string _GetSupportedParameterStringFromManipulator(IImageManipulator manipulator) {
      if (manipulator == null)
        return null;

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

      return result.Count < 1 ? null : " (" + string.Join(", ", result) + ")";
    }

    /// <summary>
    /// Saves an image and adjust jpeg quality if saving to jpeg.
    /// </summary>
    /// <param name="fullFilePath">The filename.</param>
    /// <param name="image">The image.</param>
    /// <returns><c>true</c> on success; otherwise, <c>false</c>.</returns>
    internal static CLIExitCode SaveHelper(string fullFilePath, Image image) {
      Contract.Requires(fullFilePath != null);

      if (image == null)
        return CLIExitCode.NothingToSave;

      var extension = Path.GetExtension(fullFilePath)?.ToUpperInvariant();

      // atomic save - temp file first, than rename, remove existing file
      var temporaryFileName = _GetTempFileName(fullFilePath);
      try {

        switch (extension) {
          case ".JPG":
          case ".JPEG": {
            var codecs = ImageCodecInfo.GetImageEncoders();
            codecs = codecs.Where(info => info != null && info.MimeType == "image/jpeg").ToArray();
            if (codecs.Length <= 0) {
              return CLIExitCode.JpegNotSupportedOnThisPlatform;
            }
            Contract.Assume(Encoder.Quality != null);
            image.Save(temporaryFileName, codecs[0], new EncoderParameters {
              Param = new[] {
                new EncoderParameter(Encoder.Quality, (long)100)
              }
            });
            break;
          }
          case ".BMP":
            image.Save(temporaryFileName, ImageFormat.Bmp);
            break;
          case ".GIF":
            image.Save(temporaryFileName, ImageFormat.Gif);
            break;
          case ".TIF":
            image.Save(temporaryFileName, ImageFormat.Tiff);
            break;
          default:
            image.Save(temporaryFileName, ImageFormat.Png);
            break;
        }
        
        if(!File.Exists(temporaryFileName))
          return CLIExitCode.TargetFileCouldNotBeSaved;

        File.Copy(temporaryFileName, fullFilePath,true);
        File.Delete(temporaryFileName);
      } catch (Exception) {
        if (!File.Exists(temporaryFileName))
          return CLIExitCode.ExceptionDuringImageWrite;

        // removing temp file again
        _TryDeleteFile(temporaryFileName);
        return CLIExitCode.ExceptionDuringImageWrite;
      }

      return CLIExitCode.OK;
    }

    private static bool _TryDeleteFile(string fileName) {
      try {
        File.Delete(fileName);
        return true;
      } catch {
        return false;
      }
    }

    private static string _GetTempFileName(string fileName) {
      var extension = Path.GetExtension(fileName);
      var i = 0;
      for (;;) {
        var result = Path.ChangeExtension(fileName, i++ + extension);
        if (!File.Exists(result))
          return result;
      }
    }

  }
}
