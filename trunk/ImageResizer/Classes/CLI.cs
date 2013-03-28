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
// TODO: support for processing a script file
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using ImageResizer;
using ImageResizer.Properties;

using Imager;
using Imager.Interface;
using word = System.UInt16;

namespace Classes {
  internal static class CLI {

    #region consts
    /// <summary>
    /// Used to identify the dimensions
    /// </summary>
    private static readonly Regex _DIMENSIONS_REGEX = new Regex(@"^(auto)|(w(?<width>[0-9]+))|(h(?<height>[0-9]+))|((?<width>[0-9]+)x(?<height>[0-9]+))|((?<percent>)%)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    /// <summary>
    /// Used to identify filter name and parameters
    /// </summary>
    private static readonly Regex _FILTER_REGEX = new Regex(@"^(?<filter>.*?)(\((?<params>.*?)\))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    #endregion

    /// <summary>
    /// Parses the command line arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    public static CLIExitCode ParseCommandLineArguments(string[] arguments) {
      if (arguments == null || arguments.Length < 1)
        return (CLIExitCode.OK);

      var i = 0;
      var length = arguments.Length;
      cImage buffer = null;
      while (i < length) {
        var command = arguments[i++].ToUpperInvariant();

        switch (command) {
          #region /LOAD
          case "/LOAD": {
            if (length - i < 1) {
              _ShowHelp();
              return (CLIExitCode.TooLessArguments);
            }
            var filename = arguments[i++];
            if (filename == null) {
              _ShowHelp();
              return (CLIExitCode.FilenameMustNotBeNull);
            }

            Console.WriteLine(string.Format("Loading from file \"{0}\"", filename));
            var result = _LoadImageFromFileName(filename);

            // error during load, exit
            if (result.Item1 != CLIExitCode.OK)
              return (result.Item1);

            buffer = result.Item2;
            break;
          }
          #endregion
          #region /RESIZE
          case "/RESIZE": {
            if (length - i < 2) {
              _ShowHelp();
              return (CLIExitCode.TooLessArguments);
            }
            var dimensions = arguments[i++].Trim();
            var filterName = arguments[i++].Trim();

            var result = _ParseResizeCommand(dimensions, filterName, ref buffer);
            if (result != CLIExitCode.OK)
              return (result);
            break;
          }
          #endregion
          #region /SAVE
          case "/SAVE": {
            if (length - i < 1) {
              _ShowHelp();
              return (CLIExitCode.TooLessArguments);
            }
            var filename = arguments[i++];
            if (filename == null) {
              _ShowHelp();
              return (CLIExitCode.FilenameMustNotBeNull);
            }

            Console.WriteLine(string.Format("Writing to file \"{0}\"", filename));
            var result = _SaveImageToFileName(buffer, filename);

            // error during save, exit
            if (result != CLIExitCode.OK)
              return (result);

            break;
          }
          #endregion
          #region /EXIT
          case "/EXIT": {
            return (CLIExitCode.OK);
          }
          #endregion
          default: {
            _ShowHelp();
            return (CLIExitCode.UnknownParameter);
          }
        }
      }
      return (CLIExitCode.OK);
    }

    /// <summary>
    /// Parses the resize command.
    /// </summary>
    /// <param name="dimensions">The dimensions.</param>
    /// <param name="filterName">Name of the filter(and parameters if any).</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    private static CLIExitCode _ParseResizeCommand(string dimensions, string filterName, ref cImage buffer) {
      Contract.Requires(dimensions != null);
      Contract.Requires(filterName != null);

      var match = _DIMENSIONS_REGEX.Match(dimensions);
      if (!match.Success) {
        _ShowHelp();
        return (CLIExitCode.InvalidTargetDimensions);
      }

      var width = match.Groups["width"].Value;
      var height = match.Groups["height"].Value;
      var percent = match.Groups["percent"].Value;

      word targetWidth = 0;
      word targetHeight = 0;
      word targetPercent = 0;

      if (!(string.IsNullOrWhiteSpace(width) || word.TryParse(width, out targetWidth))) {
        _ShowHelp();
        return (CLIExitCode.CouldNotParseDimensionsAsWord);
      }

      if (!(string.IsNullOrWhiteSpace(height) || word.TryParse(height, out targetHeight))) {
        _ShowHelp();
        return (CLIExitCode.CouldNotParseDimensionsAsWord);
      }

      if (!(string.IsNullOrWhiteSpace(percent) || word.TryParse(percent, out targetPercent))) {
        _ShowHelp();
        return (CLIExitCode.CouldNotParseDimensionsAsWord);
      }

      if (targetPercent != 0) {

        // only percentage is given
        if (buffer == null) {
          _DisplayErrorMessage(Resources.txNothingToResize, Resources.ttNothingToResize);
          return (CLIExitCode.NothingToResize);
        }
        targetWidth = (word)Math.Round(buffer.Width * targetPercent / 100.0);
        targetHeight = (word)Math.Round(buffer.Height * targetPercent / 100.0);
      } else if (targetWidth == 0) {

        // aspect-based, height given
        if (buffer == null) {
          _DisplayErrorMessage(Resources.txNothingToResize, Resources.ttNothingToResize);
          return (CLIExitCode.NothingToResize);
        }
        targetWidth = (word)Math.Round((double)targetHeight * buffer.Width / buffer.Height);
      } else if (targetHeight == 0) {

        // aspect-based, width given
        if (buffer == null) {
          _DisplayErrorMessage(Resources.txNothingToResize, Resources.ttNothingToResize);
          return (CLIExitCode.NothingToResize);
        }
        targetHeight = (word)Math.Round((double)targetWidth * buffer.Height / buffer.Width);
      }

      var filterMatch = _FILTER_REGEX.Match(filterName);
      if (!filterMatch.Success) {
        _ShowHelp();
        return (CLIExitCode.InvalidFilterDescription);
      }

      var filterParams = filterMatch.Groups["params"].Value;
      filterName = filterMatch.Groups["filter"].Value;

      var useThresholds = true;
      var useCenteredGrid = true;
      var repeat = (byte)1;
      var radius = 1f;
      var vbounds = OutOfBoundsMode.ConstantExtension;
      var hbounds = OutOfBoundsMode.ConstantExtension;

      if (!string.IsNullOrWhiteSpace(filterParams) && !byte.TryParse(filterParams, out repeat)) {
        repeat = 1;

        // parse parameterlist, splitted with "," and assigned using "="
        var parameters = (
          from p in filterParams.Split(',')
          let idx = p.IndexOf('=')
          where idx > 0
          let name = p.Substring(0, idx).Trim().ToLower()
          let value = p.Substring(idx + 1).Trim()
          select new KeyValuePair<string, string>(name, value)
        );

        foreach (var pair in parameters) {
          switch (pair.Key) {
            case "vbounds": {
              var value = pair.Value.ToLower();
              if (value == "const")
                vbounds = OutOfBoundsMode.ConstantExtension;
              else if (value == "half")
                vbounds = OutOfBoundsMode.HalfSampleSymmetric;
              else if (value == "whole")
                vbounds = OutOfBoundsMode.WholeSampleSymmetric;
              else if (value == "wrap")
                vbounds = OutOfBoundsMode.WrapAround;
              else {
                _ShowHelp();
                return (CLIExitCode.InvalidOutOfBoundsMode);
              }
              break;
            }
            case "hbounds": {
              var value = pair.Value.ToLower();
              if (value == "const")
                hbounds = OutOfBoundsMode.ConstantExtension;
              else if (value == "half")
                hbounds = OutOfBoundsMode.HalfSampleSymmetric;
              else if (value == "whole")
                hbounds = OutOfBoundsMode.WholeSampleSymmetric;
              else if (value == "wrap")
                hbounds = OutOfBoundsMode.WrapAround;
              else {
                _ShowHelp();
                return (CLIExitCode.InvalidOutOfBoundsMode);
              }
              break;
            }
            case "radius": {
              if (!float.TryParse(pair.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out radius)) {
                _ShowHelp();
                return (CLIExitCode.CouldNotParseParameterAsFloat);
              }
              break;
            }
            case "repeat": {
              if (!byte.TryParse(pair.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out repeat)) {
                _ShowHelp();
                return (CLIExitCode.CouldNotParseParameterAsByte);
              }
              break;
            }
            case "centered": {
              useCenteredGrid = pair.Value == "1";
              break;
            }
            case "thresholds": {
              useThresholds = pair.Value == "1";
              break;
            }
            default: {
              _ShowHelp();
              return (CLIExitCode.UnknownParameter);
            }
          }
        }
      }

      var result = _ResizeImage(buffer, targetWidth, targetHeight, filterName, hbounds, vbounds, useThresholds, useCenteredGrid, repeat, radius);
      if (result.Item1 != CLIExitCode.OK)
        return (result.Item1);

      buffer = result.Item2;
      return (CLIExitCode.OK);
    }

    /// <summary>
    /// Resizes the given image.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="filterName">Name of the filter.</param>
    /// <param name="horizontalBph">The horizontal BPH.</param>
    /// <param name="verticalBph">The vertical BPH.</param>
    /// <param name="useThresholds">if set to <c>true</c> [use thresholds].</param>
    /// <param name="useCenteredGrid">if set to <c>true</c> [use centered grid].</param>
    /// <param name="repeat">The repeat.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    private static Tuple<CLIExitCode, cImage> _ResizeImage(cImage image, word width, word height, string filterName, OutOfBoundsMode horizontalBph, OutOfBoundsMode verticalBph, bool useThresholds, bool useCenteredGrid, byte repeat, float radius) {
      if (image == null) {
        _DisplayErrorMessage(Resources.txNothingToResize, Resources.ttNothingToResize);
        return (Tuple.Create(CLIExitCode.NothingToResize, (cImage)null));
      }
      Console.WriteLine(@"{0} {1} {2} {3}", width, height, repeat, filterName);
      var imageResizerTokens = SupportedManipulators.MANIPULATORS;
      Contract.Assume(imageResizerTokens != null);
      var match = imageResizerTokens.Where(resizer => string.Compare(resizer.Key, filterName, true) == 0).Select(kvp => kvp.Value).FirstOrDefault();
      if (match == null) {
        _DisplayErrorMessage(string.Format(Resources.txUnknownFilter, filterName), Resources.ttUnknownFilter);
        return (Tuple.Create(CLIExitCode.UnknownFilter, image));
      }
      image = MainForm.FilterImage(image, match, width, height, horizontalBph, verticalBph, useThresholds, useCenteredGrid, repeat, radius);

      // resize target image if width/height were specified and differ from current result image's size
      if (width != 0 && height != 0 && (width != image.Width || height != image.Height))
        image = image.ApplyScaler(InterpolationMode.Bicubic, width, height);

      return (Tuple.Create(CLIExitCode.OK, image));
    }

    /// <summary>
    /// Loads an image into memory.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <returns></returns>
    private static Tuple<CLIExitCode, cImage> _LoadImageFromFileName(string filename) {
      Contract.Requires(filename != null);
      Bitmap result;
      try {
        result = (Bitmap)Image.FromFile(filename);
      } catch (Exception e) {
        _DisplayErrorMessage(string.Format(Resources.txCouldNotLoadImage, filename, e.Message), Resources.ttCouldNotLoadImage);
        return (Tuple.Create(CLIExitCode.ExceptionDuringImageLoad, (cImage)null));
      }
      Console.WriteLine("  File   : {0} Bytes", new FileInfo(filename).Length);
      Console.WriteLine("  Width  : {0} Pixel", result.Width);
      Console.WriteLine("  Height : {0} Pixel", result.Height);
      Console.WriteLine("  Size   : {0:0.00} MegaPixel", (result.Width * result.Height / 1000000.0));
      Console.WriteLine("  Type   : {0}", ImageCodecInfo.GetImageDecoders().First(d => d.FormatID == result.RawFormat.Guid).FormatDescription);
      Console.WriteLine("  Format : {0}", result.PixelFormat);
      return (Tuple.Create(CLIExitCode.OK, cImage.FromBitmap(result)));
    }

    /// <summary>
    /// Saves an image and checks if it was saved successfully.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="filename">The filename.</param>
    /// <returns></returns>
    private static CLIExitCode _SaveImageToFileName(cImage image, string filename) {
      Contract.Requires(image != null);
      Contract.Requires(filename != null);

      var result = SaveHelper(filename, image.ToBitmap());

      if (result == CLIExitCode.OK) {

        // reload image to find out if everything went right
        var reloadedImage = _LoadImageFromFileName(filename);
        if (reloadedImage.Item1 != CLIExitCode.OK)
          return (reloadedImage.Item1);

      } else if (result == CLIExitCode.NothingToSave)
        _DisplayErrorMessage(Resources.txNothingToSave, Resources.ttNothingToSave);
      else if (result == CLIExitCode.JpegNotSupportedOnThisPlatform)
        _DisplayErrorMessage(Resources.txNoJpegSupport, Resources.ttNoJpegSupport);

      return (result);
    }

    /// <summary>
    /// Shows the CLI help.
    /// </summary>
    private static void _ShowHelp() {
      System.Reflection.Assembly objAssembly = System.Reflection.Assembly.GetExecutingAssembly();
      var lines = string.Join(
        Environment.NewLine,
        new[] {
          Path.GetFileName(objAssembly.Location)
            + " [/load <source>] [/resize <dimensions> <method>[(<repeat>|<paramlist>)]] [/save <target>] ... [/exit]",
          string.Empty,
          "  /load          - Loads a file into the buffer.",
          "    <source>     - the source file to resize",
          string.Empty,
          "  /save          - Saves the image in the buffer to a file.",
          "    <target>     - the target file to write",
          string.Empty,
          "  /resize        - Resizes the image in the buffer and stores the result back to the buffer.",
          "    <dimensions> - auto | <x>w | <y>h | <x>x<y> | <p>%",
          "                   If only width or height is specified, the other dimension is auto-detected by aspect ratio",
          "      auto       - determine target dimensions from used resizing filter",
          "      <x>        - the final width in pixels for the target",
          "      <y>        - the final height in pixels for the target",
          "      <p>        - the percentage to resize eg 400% for 4-times resizing",
          "    <method>     - the method to use",
          "    <repeat>     - the number of repetitions using this method",
          "    <paramlist>  - a list of parameters to apply, separated using ',' and assigned using '='; eg. radius=4, centeredGrid=0",
          "      radius     - a floating point value setting the radius of the filter",
          "      centered   - 1 - use centered grid, 0 - do not use centered grid",
          "      thresholds - 1 - use thresholds, 0 - do not use thresholds",
          "      repeat     - a value 1-255 setting the number of repetitions to apply",
          "      vbounds    - vertical out of bounds handling: const, half, whole, wrap",
          "      hbounds    - horizontal out of bounds handling: const, half, whole, wrap",
          string.Empty,
          "  /exit          - Quits the program without showing the GUI.",
          string.Empty,
          string.Empty,
          "You can load and process multiple files at once by loading after saving again.",
          "ie. /load 1.bmp /resize 10x10 Pixel /save 1.jpg /load 2.bmp /resize 10x10 Pixel /save 2.jpg",
          string.Empty,
          "You can also save to multiple files by adding another save parameter.",
          "ie. /load 1.bmp /resize 10x10 Pixel /save 1.jpg /save 2.jpg",
          string.Empty,
          "Even preprocessing using multiple filters is possible by adding another resize parameter.",
          "ie. /load 1.bmp /resize 10x10 Pixel /resize auto Scale2x /save 1.jpg",
          string.Empty,
          "Supported filter methods:"
        }.Concat(SupportedManipulators.MANIPULATORS.Select(f => "  " + f.Key)));

      Console.WriteLine(lines);
    }

    /// <summary>
    /// Displays an error message in the console.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="title">The title.</param>
    private static void _DisplayErrorMessage(string text, string title) {
      Console.WriteLine(@" ***** {0} *****", (title ?? "ERROR"));
      Console.WriteLine(text);
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
        if (extension == ".JPG"
          || extension == ".JPEG") {
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
