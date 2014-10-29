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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Classes.ScriptActions;
using Imager.Interface;
using word = System.UInt16;

namespace Classes {
  internal class ScriptSerializer {
    #region consts
    /// <summary>
    /// Default file extensions for script files.
    /// </summary>
    public const string DEFAULT_FILE_EXTENSION = ".irs";
    /// <summary>
    /// Used to identify the dimensions
    /// </summary>
    private static readonly Regex _DIMENSIONS_REGEX = new Regex(@"^(auto)|(w(?<width>[0-9]+))|(h(?<height>[0-9]+))|((?<width>[0-9]+)x(?<height>[0-9]+))|((?<percent>)%)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    /// <summary>
    /// Used to identify filter name and parameters
    /// </summary>
    private static readonly Regex _FILTER_REGEX = new Regex(@"^(?<filter>.*?)(\((?<params>[^\)]*?)\))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal const string CONST_BOUNDS_VALUE = "const";
    internal const string HALF_BOUNDS_VALUE = "half";
    internal const string WHOLE_BOUNDS_VALUE = "whole";
    internal const string WRAP_BOUNDS_VALUE = "wrap";

    internal const string REPEAT_PARAMETER_NAME = "repeat";
    internal const string RADIUS_PARAMETER_NAME = "radius";
    internal const string CENTERED_GRID_PARAMETER_NAME = "centered";
    internal const string THRESHOLDS_PARAMETER_NAME = "thresholds";
    internal const string HBOUNDS_PARAMETER_NAME = "hbounds";
    internal const string VBOUNDS_PARAMETER_NAME = "vbounds";

    internal const string LOAD_COMMAND_NAME = "/load";
    internal const string SCRIPT_COMMAND_NAME = "/script";
    internal const string SAVE_COMMAND_NAME = "/save";
    internal const string RESIZE_COMMAND_NAME = "/resize";
    internal const string STDOUT_COMMAND_NAME = "/stdout";
    internal const string STDIN_COMMAND_NAME = "/stdin";

    #endregion
    /// <summary>
    /// Serializes the state of the script engine.
    /// </summary>
    /// <param name="engine">The engine.</param>
    /// <returns>A makro to repeat the script engine actions.</returns>
    public static string SerializeState(ScriptEngine engine) {
      Contract.Requires(engine != null);
      var tasks = engine.Actions;
      return (string.Join(Environment.NewLine, tasks.Select(_GetCommandTextForAction).Where(l => !string.IsNullOrWhiteSpace(l))));
    }

    /// <summary>
    /// Saves the script engine's tasks to a file.
    /// </summary>
    /// <param name="engine">The engine.</param>
    /// <param name="filename">The filename.</param>
    public static void SaveToFile(ScriptEngine engine, string filename) {
      Contract.Requires(engine != null);
      Contract.Requires(filename != null);
      File.WriteAllText(filename, SerializeState(engine));
    }

    /// <summary>
    /// Loads from file.
    /// </summary>
    /// <param name="engine">The engine.</param>
    /// <param name="filename">The filename.</param>
    public static void LoadFromFile(ScriptEngine engine, string filename) {
      Contract.Requires(engine != null);
      Contract.Requires(filename != null);
      var content = File.ReadAllLines(filename);
      for (var lineNumber = 0; lineNumber < content.Length; lineNumber++) {
        var line = content[lineNumber];
        var cliExitCode = _ParseScriptLine(line, engine);
        if (cliExitCode != CLIExitCode.OK)
          throw new ScriptSerializerException(filename, lineNumber, cliExitCode);
      }
    }

    /// <summary>
    /// Loads from string.
    /// </summary>
    /// <param name="engine">The engine.</param>
    /// <param name="line">The line.</param>
    public static void LoadFromString(ScriptEngine engine, string line) {
      Contract.Requires(engine != null);
      var cliExitCode = _ParseScriptLine(line, engine);
      if (cliExitCode != CLIExitCode.OK)
        throw new ScriptSerializerException(null, 1, cliExitCode);
    }

    /// <summary>
    /// Parses a script line and adds the action from it to the engine's list.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <param name="engine">The engine.</param>
    private static CLIExitCode _ParseScriptLine(string line, ScriptEngine engine) {
      Contract.Requires(engine != null);
      if (string.IsNullOrWhiteSpace(line))
        return (CLIExitCode.OK);

      var arguments = line.SplitWithQuotes(' ').ToArray();
      var length = arguments.Length;
      if (length < 1)
        return (CLIExitCode.OK);

      var i = 0;
      while (i < length) {
        var command = arguments[i++].ToLowerInvariant();

        // skip empty stuff
        if (command.IsNullOrWhiteSpace())
          continue;

        switch (command) {
          #region /STDIN
          case STDIN_COMMAND_NAME: {
            engine.AddWithoutExecution(new LoadStdInCommand());
            engine.AddWithoutExecution(new NullTransformCommand());
            break;
          }
          #endregion
          #region /STDOUT
          case STDOUT_COMMAND_NAME: {
            engine.AddWithoutExecution(new SaveStdOutCommand());
            break;
          }
          #endregion
          #region /LOAD
          case LOAD_COMMAND_NAME: {
            if (length - i < 1) {
              return (CLIExitCode.TooLessArguments);
            }
            var filename = arguments[i++];
            if (filename == null) {
              return (CLIExitCode.FilenameMustNotBeNull);
            }

            engine.AddWithoutExecution(new LoadFileCommand(filename));
            engine.AddWithoutExecution(new NullTransformCommand());
            break;
          }
          #endregion
          #region /RESIZE
          case RESIZE_COMMAND_NAME: {
            if (length - i < 2) {
              return (CLIExitCode.TooLessArguments);
            }
            var dimensions = arguments[i++].Trim();
            var filterName = arguments[i++].Trim();

            var result = _ParseResizeCommand(engine, dimensions, filterName);
            if (result != CLIExitCode.OK)
              return (result);
            break;
          }
          #endregion
          #region /SAVE
          case SAVE_COMMAND_NAME: {
            if (length - i < 1) {
              return (CLIExitCode.TooLessArguments);
            }
            var filename = arguments[i++];
            if (filename == null) {
              return (CLIExitCode.FilenameMustNotBeNull);
            }

            engine.AddWithoutExecution(new SaveFileCommand(filename));
            break;
          }
          #endregion
          #region /SCRIPT
          case SCRIPT_COMMAND_NAME: {
            if (length - i < 1) {
              return (CLIExitCode.TooLessArguments);
            }
            var filename = arguments[i++];
            if (filename == null) {
              return (CLIExitCode.FilenameMustNotBeNull);
            }

            LoadFromFile(engine, filename);
            break;
          }
          #endregion
          default: {
            return (CLIExitCode.UnknownParameter);
          }
        }
      }
      return (CLIExitCode.OK);
    }


    /// <summary>
    /// Parses the resize command.
    /// </summary>
    /// <param name="engine">The engine.</param>
    /// <param name="dimensions">The dimensions.</param>
    /// <param name="filterName">Name of the filter(and parameters if any).</param>
    /// <returns></returns>
    private static CLIExitCode _ParseResizeCommand(ScriptEngine engine, string dimensions, string filterName) {
      Contract.Requires(engine != null);
      Contract.Requires(dimensions != null);
      Contract.Requires(filterName != null);

      var match = _DIMENSIONS_REGEX.Match(dimensions);
      if (!match.Success)
        return (CLIExitCode.InvalidTargetDimensions);

      var width = match.Groups["width"].Value;
      var height = match.Groups["height"].Value;
      var percent = match.Groups["percent"].Value;

      word targetWidth = 0;
      word targetHeight = 0;
      word targetPercent = 0;

      if (!(string.IsNullOrWhiteSpace(width) || word.TryParse(width, out targetWidth)))
        return (CLIExitCode.CouldNotParseDimensionsAsWord);

      if (!(string.IsNullOrWhiteSpace(height) || word.TryParse(height, out targetHeight)))
        return (CLIExitCode.CouldNotParseDimensionsAsWord);

      if (!(string.IsNullOrWhiteSpace(percent) || word.TryParse(percent, out targetPercent)))
        return (CLIExitCode.CouldNotParseDimensionsAsWord);

      var useAspect = targetWidth == 0 || targetHeight == 0;

      var filterMatch = _FILTER_REGEX.Match(filterName);
      if (!filterMatch.Success)
        return (CLIExitCode.InvalidFilterDescription);

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

        #region supported parameter handling
        foreach (var pair in parameters) {
          switch (pair.Key) {
            case VBOUNDS_PARAMETER_NAME: {
              var value = pair.Value.ToLower();
              if (value == CONST_BOUNDS_VALUE)
                vbounds = OutOfBoundsMode.ConstantExtension;
              else if (value == HALF_BOUNDS_VALUE)
                vbounds = OutOfBoundsMode.HalfSampleSymmetric;
              else if (value == WHOLE_BOUNDS_VALUE)
                vbounds = OutOfBoundsMode.WholeSampleSymmetric;
              else if (value == WRAP_BOUNDS_VALUE)
                vbounds = OutOfBoundsMode.WrapAround;
              else
                return (CLIExitCode.InvalidOutOfBoundsMode);
              break;
            }
            case HBOUNDS_PARAMETER_NAME: {
              var value = pair.Value.ToLower();
              if (value == CONST_BOUNDS_VALUE)
                hbounds = OutOfBoundsMode.ConstantExtension;
              else if (value == HALF_BOUNDS_VALUE)
                hbounds = OutOfBoundsMode.HalfSampleSymmetric;
              else if (value == WHOLE_BOUNDS_VALUE)
                hbounds = OutOfBoundsMode.WholeSampleSymmetric;
              else if (value == WRAP_BOUNDS_VALUE)
                hbounds = OutOfBoundsMode.WrapAround;
              else
                return (CLIExitCode.InvalidOutOfBoundsMode);
              break;
            }
            case RADIUS_PARAMETER_NAME: {
              if (!float.TryParse(pair.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out radius))
                return (CLIExitCode.CouldNotParseParameterAsFloat);
              break;
            }
            case REPEAT_PARAMETER_NAME: {
              if (!byte.TryParse(pair.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out repeat))
                return (CLIExitCode.CouldNotParseParameterAsByte);
              break;
            }
            case CENTERED_GRID_PARAMETER_NAME: {
              useCenteredGrid = pair.Value == "1";
              break;
            }
            case THRESHOLDS_PARAMETER_NAME: {
              useThresholds = pair.Value == "1";
              break;
            }
            default: {
              return (CLIExitCode.UnknownParameter);
            }
          }
        }
        #endregion

      }

      // find the given manipulator
      var manipulator = SupportedManipulators.MANIPULATORS
        .Where(resizer => string.Compare(resizer.Key, filterName, true) == 0)
        .Select(kvp => kvp.Value)
        .FirstOrDefault()
      ;

      if (manipulator == null)
        return (CLIExitCode.UnknownFilter);

      engine.AddWithoutExecution(new ResizeCommand(true, manipulator, targetWidth, targetHeight, targetPercent, useAspect, hbounds, vbounds, repeat, useThresholds, useCenteredGrid, radius));
      return (CLIExitCode.OK);
    }

    /// <summary>
    /// Gets the command text for a given action.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The command text needed to reproduce the action.</returns>
    private static string _GetCommandTextForAction(IScriptAction action) {
      Contract.Requires(action != null);
      var loadAction = action as LoadFileCommand;
      if (loadAction != null)
        return (string.Format(@"{0} ""{1}""", LOAD_COMMAND_NAME, loadAction.FileName));

      var saveAction = action as SaveFileCommand;
      if (saveAction != null)
        return (string.Format(@"{0} ""{1}""", SAVE_COMMAND_NAME, saveAction.FileName));

      var resizeAction = action as ResizeCommand;
      if (resizeAction != null) {
        var manipulator = resizeAction.Manipulator;
        string dimensions;
        if (manipulator.SupportsWidth && manipulator.SupportsHeight)
          dimensions = resizeAction.Percentage > 0 ? string.Format("{0}%", resizeAction.Percentage) : string.Format("{0}x{1}", resizeAction.Width, resizeAction.Height);
        else if (manipulator.SupportsWidth)
          dimensions = string.Format("w{0}", resizeAction.Width);
        else if (manipulator.SupportsHeight)
          dimensions = string.Format("h{0}", resizeAction.Height);
        else
          dimensions = "auto";

        var parameters = new List<string> {
          HBOUNDS_PARAMETER_NAME+"="+_ConvertOutOfBoundsModeToText(resizeAction.HorizontalBph),
          VBOUNDS_PARAMETER_NAME+"="+_ConvertOutOfBoundsModeToText(resizeAction.VerticalBph),
        };

        if (manipulator.SupportsRepetitionCount && resizeAction.Count > 1)
          parameters.Add(REPEAT_PARAMETER_NAME + "=" + resizeAction.Count.ToString(CultureInfo.InvariantCulture));

        if (manipulator.SupportsThresholds)
          parameters.Add(THRESHOLDS_PARAMETER_NAME + "=" + (resizeAction.UseThresholds ? "1" : "0"));

        if (manipulator.SupportsGridCentering)
          parameters.Add(CENTERED_GRID_PARAMETER_NAME + "=" + (resizeAction.UseCenteredGrid ? "1" : "0"));

        if (manipulator.SupportsRadius)
          parameters.Add(RADIUS_PARAMETER_NAME + "=" + resizeAction.Radius.ToString(CultureInfo.InvariantCulture));

        return (string.Format(@"{0} {1} ""{2}({3})""", RESIZE_COMMAND_NAME, dimensions, SupportedManipulators.MANIPULATORS.First(m => m.Value == manipulator).Key, string.Join(", ", parameters)));
      }

      return (null);
    }

    /// <summary>
    /// Converts the out of bounds mode to it's textual representation.
    /// </summary>
    /// <param name="mode">The mode.</param>
    /// <returns></returns>
    private static string _ConvertOutOfBoundsModeToText(OutOfBoundsMode mode) {
      if (mode == OutOfBoundsMode.ConstantExtension)
        return (CONST_BOUNDS_VALUE);
      if (mode == OutOfBoundsMode.HalfSampleSymmetric)
        return (HALF_BOUNDS_VALUE);
      if (mode == OutOfBoundsMode.WholeSampleSymmetric)
        return (WHOLE_BOUNDS_VALUE);
      if (mode == OutOfBoundsMode.WrapAround)
        return (WRAP_BOUNDS_VALUE);
      throw new NotImplementedException();
    }

  }
}
