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
using System.Drawing.Extensions.ColorProcessing.Resizing;
using System.Globalization;
using System.Linq;
using System.Threading;

using Classes;
using Classes.ScriptActions;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers the script/command line parser. Everything here is reachable from both the CLI and
  /// the GUI's script loader, and none of it needs a display or an actual image.
  /// </summary>
  [TestFixture]
  public class ScriptSerializerTests {

    #region helpers

    /// <summary>
    /// Parses a script line into a fresh engine.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <returns>The engine holding the parsed actions.</returns>
    private static ScriptEngine _Parse(string line) {
      var engine = new ScriptEngine();
      ScriptSerializer.LoadFromString(engine, line);
      return engine;
    }

    /// <summary>
    /// Parses a script line and returns the exit code it failed with.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <returns>The exit code.</returns>
    private static CLIExitCode _ParseExpectingFailure(string line) {
      var exception = Assert.Throws<ScriptSerializerException>(() => _Parse(line));
      return exception.ErrorType;
    }

    /// <summary>
    /// Parses a script line consisting of a single /resize and returns the resulting command.
    /// </summary>
    /// <param name="dimensions">The dimensions argument.</param>
    /// <param name="filter">The filter argument.</param>
    /// <returns>The resize command.</returns>
    private static ResizeCommand _ParseResize(string dimensions, string filter)
      => (ResizeCommand)_Parse($@"""/resize"" ""{dimensions}"" ""{filter}""").Actions.Single()
    ;

    /// <summary>
    /// Quotes arguments the way <see cref="CLI"/> hands a command line to the parser.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The script line.</returns>
    private static string _AsCommandLine(params string[] arguments)
      => string.Join(" ", arguments.Select(a => $@"""{a}"""))
    ;

    /// <summary>A manipulator that does nothing; only its identity matters here.</summary>
    private sealed class FakeManipulator : IImageManipulator {
      public bool SupportsWidth => false;
      public bool SupportsHeight => false;
      public bool SupportsRepetitionCount => false;
      public bool SupportsGridCentering => false;
      public bool SupportsThresholds => false;
      public bool SupportsRadius => false;
      public bool ChangesResolution => false;
      public string Description => "fake";
      public IReadOnlyList<Hawkynt.ColorProcessing.ParameterDescriptor> Parameters => ImageManipulatorDefaults.EmptyParameters;
      public IImageManipulator CreateWith(IReadOnlyDictionary<string, object> values) => this;
    }

    private static KeyValuePair<string, IImageManipulator> _Entry(string key, IImageManipulator manipulator)
      => new KeyValuePair<string, IImageManipulator>(key, manipulator)
    ;

    #endregion

    #region commands

    [Test]
    public void EmptyLine_ProducesNoActions() {
      Assert.That(_Parse(string.Empty).Actions, Is.Empty);
      Assert.That(_Parse("   ").Actions, Is.Empty);
    }

    [Test]
    public void Load_AddsLoadCommandWithFileName() {
      var actions = _Parse(_AsCommandLine("/load", "in.png")).Actions.ToArray();

      Assert.That(actions[0], Is.InstanceOf<LoadFileCommand>());
      Assert.That(((LoadFileCommand)actions[0]).FileName, Is.EqualTo("in.png"));
    }

    [Test]
    public void Load_KeepsSpacesInQuotedFileNames() {
      var actions = _Parse(_AsCommandLine("/load", @"C:\my images\in.png")).Actions.ToArray();

      Assert.That(((LoadFileCommand)actions[0]).FileName, Is.EqualTo(@"C:\my images\in.png"));
    }

    [Test]
    public void Save_AddsSaveCommandWithFileName() {
      var actions = _Parse(_AsCommandLine("/save", "out.png")).Actions.ToArray();

      Assert.That(actions[0], Is.InstanceOf<SaveFileCommand>());
      Assert.That(((SaveFileCommand)actions[0]).FileName, Is.EqualTo("out.png"));
    }

    [Test]
    public void StdInAndStdOut_AddTheirCommands() {
      var actions = _Parse(_AsCommandLine("/stdin", "/stdout")).Actions.ToArray();

      Assert.That(actions.Any(a => a is LoadStdInCommand), Is.True);
      Assert.That(actions.Any(a => a is SaveStdOutCommand), Is.True);
    }

    [Test]
    public void CommandNames_AreCaseInsensitive() {
      Assert.That(_Parse(_AsCommandLine("/LOAD", "in.png")).Actions.First(), Is.InstanceOf<LoadFileCommand>());
    }

    [Test]
    public void FullChain_KeepsCommandOrder() {
      var actions = _Parse(_AsCommandLine("/load", "in.png", "/resize", "auto", "Upscaler: HQ 2x", "/save", "out.png"))
        .Actions
        .Where(a => !(a is NullTransformCommand))
        .ToArray()
        ;

      Assert.That(actions.Select(a => a.GetType()), Is.EqualTo(new[] {
        typeof(LoadFileCommand),
        typeof(ResizeCommand),
        typeof(SaveFileCommand),
      }));
    }

    [Test]
    public void UnknownCommand_IsRejected()
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/frobnicate", "in.png")), Is.EqualTo(CLIExitCode.UnknownParameter))
    ;

    [TestCase("/load")]
    [TestCase("/save")]
    [TestCase("/script")]
    public void CommandWithoutItsArgument_IsRejected(string command)
      => Assert.That(_ParseExpectingFailure(_AsCommandLine(command)), Is.EqualTo(CLIExitCode.TooLessArguments))
    ;

    [Test]
    public void ResizeWithoutFilter_IsRejected()
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", "auto")), Is.EqualTo(CLIExitCode.TooLessArguments))
    ;

    #endregion

    #region dimensions

    [Test]
    public void Auto_LeavesEveryDimensionUnset() {
      var command = _ParseResize("auto", "Resampler: Bicubic");

      Assert.That(command.Width, Is.EqualTo(0));
      Assert.That(command.Height, Is.EqualTo(0));
      Assert.That(command.Percentage, Is.EqualTo(0));
      Assert.That(command.MaintainAspect, Is.True);
    }

    [Test]
    public void WidthOnly_SetsWidthAndKeepsAspect() {
      var command = _ParseResize("w72", "Resampler: Bicubic");

      Assert.That(command.Width, Is.EqualTo(72));
      Assert.That(command.Height, Is.EqualTo(0));
      Assert.That(command.MaintainAspect, Is.True);
    }

    [Test]
    public void HeightOnly_SetsHeightAndKeepsAspect() {
      var command = _ParseResize("h92", "Resampler: Bicubic");

      Assert.That(command.Width, Is.EqualTo(0));
      Assert.That(command.Height, Is.EqualTo(92));
      Assert.That(command.MaintainAspect, Is.True);
    }

    [Test]
    public void BothDimensions_DropAspectMaintenance() {
      var command = _ParseResize("72x92", "Resampler: Bicubic");

      Assert.That(command.Width, Is.EqualTo(72));
      Assert.That(command.Height, Is.EqualTo(92));
      Assert.That(command.MaintainAspect, Is.False);
    }

    /// <summary>
    /// Regression: the percentage group used to capture nothing, so every <c>&lt;p&gt;%</c>
    /// silently degraded into <c>auto</c>.
    /// </summary>
    [TestCase("1%", 1)]
    [TestCase("100%", 100)]
    [TestCase("325%", 325)]
    [TestCase("65535%", 65535)]
    public void Percentage_IsParsed(string dimensions, int expected) {
      var command = _ParseResize(dimensions, "Resampler: Bicubic");

      Assert.That(command.Percentage, Is.EqualTo(expected));
      Assert.That(command.Width, Is.EqualTo(0));
      Assert.That(command.Height, Is.EqualTo(0));
    }

    [TestCase("0x0")]
    [TestCase("65535x65535")]
    [TestCase("w0")]
    [TestCase("w65535")]
    public void DimensionsAtTheWordBoundary_AreAccepted(string dimensions)
      => Assert.That(() => _ParseResize(dimensions, "Resampler: Bicubic"), Throws.Nothing)
    ;

    [TestCase("65536x1")]
    [TestCase("1x65536")]
    [TestCase("w65536")]
    [TestCase("h65536")]
    [TestCase("65536%")]
    public void DimensionsAboveTheWordBoundary_AreRejected(string dimensions)
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", dimensions, "Resampler: Bicubic")), Is.EqualTo(CLIExitCode.CouldNotParseDimensionsAsWord))
    ;

    /// <summary>
    /// Regression: the alternation was not grouped, so <c>^</c> and <c>$</c> anchored the first
    /// and last branch only and anything wrapped around a valid branch was accepted.
    /// </summary>
    [TestCase("")]
    [TestCase("wobble")]
    [TestCase("junk-w72-junk")]
    [TestCase("w72x92")]
    [TestCase("xw72")]
    [TestCase("72x")]
    [TestCase("x92")]
    [TestCase("%")]
    [TestCase("325 %")]
    [TestCase("-72x92")]
    public void MalformedDimensions_AreRejected(string dimensions)
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", dimensions, "Resampler: Bicubic")), Is.EqualTo(CLIExitCode.InvalidTargetDimensions))
    ;

    [TestCase("AUTO")]
    [TestCase("W72")]
    [TestCase("72X92")]
    public void Dimensions_AreCaseInsensitive(string dimensions)
      => Assert.That(() => _ParseResize(dimensions, "Resampler: Bicubic"), Throws.Nothing)
    ;

    #endregion

    #region filter names

    [Test]
    public void FullyQualifiedFilterName_Resolves()
      => Assert.That(_ParseResize("auto", "Upscaler: HQ 2x").Manipulator, Is.Not.Null)
    ;

    /// <summary>
    /// Regression for issue #33: manipulator keys gained a category prefix, which invalidated
    /// every command line and script written before it existed.
    /// </summary>
    [TestCase("HQ 2x")]
    [TestCase("XBR 3x")]
    [TestCase("Bicubic")]
    [TestCase("Scale 2x")]
    public void FilterNameWithoutItsCategory_StillResolves(string filterName)
      => Assert.That(_ParseResize("auto", filterName).Manipulator, Is.Not.Null)
    ;

    [Test]
    public void FilterNameWithoutItsCategory_ResolvesToTheSameManipulatorAsTheFullName()
      => Assert.That(_ParseResize("auto", "HQ 2x").Manipulator, Is.SameAs(_ParseResize("auto", "Upscaler: HQ 2x").Manipulator))
    ;

    [TestCase("hq 2X")]
    [TestCase("upscaler: HQ 2X")]
    [TestCase("UPSCALER: hq 2x")]
    public void FilterNames_AreCaseInsensitive(string filterName)
      => Assert.That(_ParseResize("auto", filterName).Manipulator, Is.Not.Null)
    ;

    [TestCase("NoSuchFilter")]
    [TestCase("NoSuchCategory: HQ 2x")]
    [TestCase("")]
    public void UnknownFilter_IsRejected(string filterName)
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", "auto", filterName)), Is.EqualTo(CLIExitCode.UnknownFilter))
    ;

    [Test]
    public void ExactKeyWins_OverABareNameFromAnotherCategory() {
      var exact = new FakeManipulator();
      var other = new FakeManipulator();
      var manipulators = new[] {
        _Entry("Filter: Upscaler: Sharpen", other),
        _Entry("Upscaler: Sharpen", exact),
      };

      var result = ScriptSerializer.FindManipulator(manipulators, "Upscaler: Sharpen", out var isAmbiguous);

      Assert.That(result, Is.SameAs(exact));
      Assert.That(isAmbiguous, Is.False);
    }

    [Test]
    public void BareNameOfferedByTwoCategories_IsAmbiguous() {
      var manipulators = new[] {
        _Entry("Upscaler: Sharpen", new FakeManipulator()),
        _Entry("Filter: Sharpen", new FakeManipulator()),
      };

      var result = ScriptSerializer.FindManipulator(manipulators, "Sharpen", out var isAmbiguous);

      Assert.That(result, Is.Null);
      Assert.That(isAmbiguous, Is.True);
    }

    [Test]
    public void AmbiguousFilter_IsReportedAsSuchByTheParser() {
      // the real registry has no ambiguous bare name, so this pins the plumbing rather than the data
      var manipulators = new[] {
        _Entry("Upscaler: Sharpen", new FakeManipulator()),
        _Entry("Filter: Sharpen", new FakeManipulator()),
      };

      ScriptSerializer.FindManipulator(manipulators, "Sharpen", out var isAmbiguous);

      Assert.That(isAmbiguous, Is.True);
    }

    [Test]
    public void EveryRegisteredFilter_IsReachableByItsFullKey() {
      var unreachable = SupportedManipulators.MANIPULATORS
        .Where(pair => !ReferenceEquals(ScriptSerializer.FindManipulator(SupportedManipulators.MANIPULATORS, pair.Key, out _), pair.Value))
        .Select(pair => pair.Key)
        .ToArray()
        ;

      Assert.That(unreachable, Is.Empty);
    }

    #endregion

    #region filter parameters

    [Test]
    public void NoParameters_YieldTheDefaults() {
      var command = _ParseResize("auto", "Resampler: Bicubic");

      Assert.That(command.Count, Is.EqualTo(1));
      Assert.That(command.Radius, Is.EqualTo(1f));
      Assert.That(command.UseThresholds, Is.True);
      Assert.That(command.UseCenteredGrid, Is.True);
      Assert.That(command.HorizontalBph, Is.EqualTo(OutOfBoundsMode.ConstantExtension));
      Assert.That(command.VerticalBph, Is.EqualTo(OutOfBoundsMode.ConstantExtension));
    }

    [Test]
    public void BareNumberParameter_IsTheRepetitionCount()
      => Assert.That(_ParseResize("auto", "Upscaler: HQ 2x(3)").Count, Is.EqualTo(3))
    ;

    [TestCase("repeat=1", 1)]
    [TestCase("repeat=255", 255)]
    public void RepeatAtItsBoundaries_IsAccepted(string parameters, int expected)
      => Assert.That(_ParseResize("auto", $"Resampler: Bicubic({parameters})").Count, Is.EqualTo(expected))
    ;

    [TestCase("repeat=256")]
    [TestCase("repeat=-1")]
    [TestCase("repeat=x")]
    public void RepeatOutsideAByte_IsRejected(string parameters)
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", "auto", $"Resampler: Bicubic({parameters})")), Is.EqualTo(CLIExitCode.CouldNotParseParameterAsByte))
    ;

    [Test]
    public void Radius_IsParsedInvariantOfTheCurrentCulture() {
      var previous = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
      try {
        Assert.That(_ParseResize("auto", "Resampler: Bicubic(radius=1.5)").Radius, Is.EqualTo(1.5f));
      } finally {
        Thread.CurrentThread.CurrentCulture = previous;
      }
    }

    [Test]
    public void NonNumericRadius_IsRejected()
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", "auto", "Resampler: Bicubic(radius=abc)")), Is.EqualTo(CLIExitCode.CouldNotParseParameterAsFloat))
    ;

    [TestCase("const", OutOfBoundsMode.ConstantExtension)]
    [TestCase("half", OutOfBoundsMode.HalfSampleSymmetric)]
    [TestCase("whole", OutOfBoundsMode.WholeSampleSymmetric)]
    [TestCase("wrap", OutOfBoundsMode.WrapAround)]
    [TestCase("transparent", OutOfBoundsMode.FlatColor)]
    public void EveryOutOfBoundsMode_IsUnderstood(string value, OutOfBoundsMode expected) {
      var command = _ParseResize("auto", $"Resampler: Bicubic(hbounds={value},vbounds={value})");

      Assert.That(command.HorizontalBph, Is.EqualTo(expected));
      Assert.That(command.VerticalBph, Is.EqualTo(expected));
    }

    [Test]
    public void UnknownOutOfBoundsMode_IsRejected()
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", "auto", "Resampler: Bicubic(vbounds=nope)")), Is.EqualTo(CLIExitCode.InvalidOutOfBoundsMode))
    ;

    [TestCase("thresholds=0", false)]
    [TestCase("thresholds=1", true)]
    public void Thresholds_AreParsed(string parameters, bool expected)
      => Assert.That(_ParseResize("auto", $"Resampler: Bicubic({parameters})").UseThresholds, Is.EqualTo(expected))
    ;

    [TestCase("centered=0", false)]
    [TestCase("centered=1", true)]
    public void CenteredGrid_IsParsed(string parameters, bool expected)
      => Assert.That(_ParseResize("auto", $"Resampler: Bicubic({parameters})").UseCenteredGrid, Is.EqualTo(expected))
    ;

    [Test]
    public void SeveralParameters_AreAllApplied() {
      var command = _ParseResize("auto", "Resampler: Bicubic(radius=2.25, repeat=4, vbounds=wrap, hbounds=half, thresholds=0, centered=0)");

      Assert.That(command.Radius, Is.EqualTo(2.25f));
      Assert.That(command.Count, Is.EqualTo(4));
      Assert.That(command.VerticalBph, Is.EqualTo(OutOfBoundsMode.WrapAround));
      Assert.That(command.HorizontalBph, Is.EqualTo(OutOfBoundsMode.HalfSampleSymmetric));
      Assert.That(command.UseThresholds, Is.False);
      Assert.That(command.UseCenteredGrid, Is.False);
    }

    [Test]
    public void UnknownParameter_IsRejected()
      => Assert.That(_ParseExpectingFailure(_AsCommandLine("/resize", "auto", "Resampler: Bicubic(bogus=1)")), Is.EqualTo(CLIExitCode.UnknownParameter))
    ;

    #endregion

    #region round trip

    /// <summary>
    /// Note that a manipulator's capability flags decide what gets written: parameters it does
    /// not advertise (radius on a resampler, repetitions on a fixed-factor upscaler) are dropped
    /// by <see cref="ScriptSerializer.SerializeState"/> and so are not round-trip material.
    /// </summary>
    [Test]
    public void SerializedState_ParsesBackIntoTheSameCommands() {
      var original = _Parse(_AsCommandLine("/load", "in.png", "/resize", "72x92", "Resampler: Bicubic(vbounds=wrap,hbounds=half,centered=0)", "/save", "out.png"));

      var reparsed = new ScriptEngine();
      foreach (var line in ScriptSerializer.SerializeState(original).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
        ScriptSerializer.LoadFromString(reparsed, line);

      var before = original.Actions.OfType<ResizeCommand>().Single();
      var after = reparsed.Actions.OfType<ResizeCommand>().Single();

      Assert.That(after.Manipulator, Is.SameAs(before.Manipulator));
      Assert.That(after.Width, Is.EqualTo(before.Width));
      Assert.That(after.Height, Is.EqualTo(before.Height));
      Assert.That(after.MaintainAspect, Is.EqualTo(before.MaintainAspect));
      Assert.That(after.VerticalBph, Is.EqualTo(before.VerticalBph));
      Assert.That(after.HorizontalBph, Is.EqualTo(before.HorizontalBph));
      Assert.That(after.UseCenteredGrid, Is.EqualTo(before.UseCenteredGrid));
    }

    [Test]
    public void SerializedFilterNames_AreFullyQualifiedAndParseBack() {
      var original = _Parse(_AsCommandLine("/resize", "auto", "HQ 2x"));

      var serialized = ScriptSerializer.SerializeState(original);

      Assert.That(serialized, Does.Contain("Upscaler: HQ 2x"));
      Assert.That(() => _Parse(serialized), Throws.Nothing);
    }

    #endregion

  }
}
