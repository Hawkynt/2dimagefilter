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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Classes;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// End-to-end coverage: runs the real <c>ImageResizer.exe</c> with real command lines and checks
  /// what it wrote and what it exited with. This is the layer that broke in issue #33 - every unit
  /// underneath was fine, the assembled program was not.
  /// <para>
  /// No pixel comparison happens here; these tests assert dimensions, container format, exit codes
  /// and diagnostics. Visual regressions need reference images, which is a separate concern.
  /// </para>
  /// </summary>
  [TestFixture]
  public class CommandLineEndToEndTests {

    /// <summary>A single run must never outlive this; a hung child would otherwise hang the suite.</summary>
    private const int _TIMEOUT_MILLISECONDS = 60000;

    private static readonly string _EXECUTABLE = Path.Combine(
      Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) ?? ".",
      "ImageResizer.exe"
    );

    private TemporaryDirectory _directory;

    [OneTimeSetUp]
    public void OneTimeSetUp()
      => Assert.That(File.Exists(_EXECUTABLE), Is.True, $"the executable under test is missing at {_EXECUTABLE}")
    ;

    [SetUp]
    public void SetUp() => this._directory = new TemporaryDirectory();

    [TearDown]
    public void TearDown() => this._directory.Dispose();

    #region helpers

    /// <summary>The outcome of one process run.</summary>
    private sealed class RunResult {
      public int ExitCode { get; set; }

      /// <summary>
      /// Standard output verbatim. Kept as bytes because it carries image data whenever /stdout
      /// is in play, and decoding that as text would mangle it.
      /// </summary>
      public byte[] StandardOutputBytes { get; set; }

      public string StandardError { get; set; }

      public string StandardOutput => Encoding.UTF8.GetString(this.StandardOutputBytes);

      public CLIExitCode Code => (CLIExitCode)this.ExitCode;

      /// <summary>Whether standard output is exactly one PNG and nothing else.</summary>
      public bool StandardOutputIsPng
        => this.StandardOutputBytes.Length > 8 && this.StandardOutputBytes.Take(8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })
      ;

      /// <summary>The dimensions in the IHDR chunk of the PNG on standard output.</summary>
      public Size StandardOutputPngSize {
        get {
          var header = this.StandardOutputBytes;
          return new Size(
            (header[16] << 24) | (header[17] << 16) | (header[18] << 8) | header[19],
            (header[20] << 24) | (header[21] << 16) | (header[22] << 8) | header[23]
          );
        }
      }
    }

    /// <summary>
    /// Runs the executable with the given arguments in the scratch directory.
    /// </summary>
    /// <param name="arguments">The arguments, each quoted as needed.</param>
    /// <returns>The outcome.</returns>
    private RunResult _Run(params string[] arguments) => this._Execute(null, null, arguments);

    /// <summary>
    /// Runs the executable from a working directory other than the scratch directory, the way
    /// Explorer launches something that lives elsewhere.
    /// </summary>
    /// <param name="workingDirectory">The directory to start in.</param>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The outcome.</returns>
    private RunResult _RunIn(string workingDirectory, params string[] arguments)
      => this._Execute(null, workingDirectory, arguments)
    ;

    private RunResult _RunWithInput(byte[] standardInput, params string[] arguments)
      => this._Execute(standardInput, null, arguments)
    ;

    /// <summary>
    /// Runs the executable, optionally feeding it standard input.
    /// </summary>
    /// <param name="standardInput">Bytes to pipe in, or <c>null</c> for no input.</param>
    /// <param name="workingDirectory">The directory to start in, or <c>null</c> for the scratch directory.</param>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The outcome.</returns>
    private RunResult _Execute(byte[] standardInput, string workingDirectory, string[] arguments) {
      var startInfo = new ProcessStartInfo(_EXECUTABLE) {
        Arguments = string.Join(" ", arguments.Select(_Quote)),
        WorkingDirectory = workingDirectory ?? this._directory.Path,
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        RedirectStandardInput = standardInput != null,
      };

      using (var process = Process.Start(startInfo)) {
        // drain both pipes before waiting, or a full buffer deadlocks the child
        var standardOutput = new MemoryStream();
        var outputPump = process.StandardOutput.BaseStream.CopyToAsync(standardOutput);
        var standardError = process.StandardError.ReadToEndAsync();

        if (standardInput != null)
          using (var input = process.StandardInput.BaseStream)
            input.Write(standardInput, 0, standardInput.Length);

        if (!process.WaitForExit(_TIMEOUT_MILLISECONDS)) {
          process.Kill();
          Assert.Fail("the executable did not terminate within {0} ms", _TIMEOUT_MILLISECONDS);
        }

        outputPump.Wait(_TIMEOUT_MILLISECONDS);

        return new RunResult {
          ExitCode = process.ExitCode,
          StandardOutputBytes = standardOutput.ToArray(),
          StandardError = standardError.Result,
        };
      }
    }

    private static string _Quote(string argument)
      => argument.IndexOf(' ') < 0 ? argument : "\"" + argument + "\""
    ;

    /// <summary>Writes a source image into the scratch directory and returns its bare file name.</summary>
    private string _Source(string fileName = "in.png", int width = 16, int height = 16) {
      TestBitmaps.WriteTo(this._directory.File(fileName), width, height);
      return fileName;
    }

    private Size _SizeOf(string fileName) => TestBitmaps.SizeOf(this._directory.File(fileName));

    private bool _Exists(string fileName) => File.Exists(this._directory.File(fileName));

    #endregion

    #region happy path

    [Test]
    public void LoadAndSave_CopiesTheImageThrough() {
      var result = this._Run("/load", this._Source(), "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(16, 16)));
    }

    /// <summary>Regression for issue #33 - this exact command line used to exit with RuntimeError.</summary>
    [Test]
    public void TheCommandLineFromIssue33_Works() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "HQ 2x", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(32, 32)));
    }

    [TestCase("Upscaler: HQ 2x", 32, 32)]
    [TestCase("HQ 2x", 32, 32)]
    [TestCase("Upscaler: XBR 3x", 48, 48)]
    [TestCase("Upscaler: xBRZ 4x", 64, 64)]
    [TestCase("Upscaler: Scale 2x", 32, 32)]
    [TestCase("Upscaler: Eagle", 32, 32)]
    public void FixedFactorUpscalers_ProduceTheirFactor(string filter, int expectedWidth, int expectedHeight) {
      var result = this._Run("/load", this._Source(), "/resize", "auto", filter, "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(expectedWidth, expectedHeight)));
    }

    [TestCase("72x92", 72, 92)]
    [TestCase("w32", 32, 32)]
    [TestCase("h48", 48, 48)]
    [TestCase("325%", 52, 52)]
    [TestCase("100%", 16, 16)]
    // a percentage that rounds below a pixel used to end the run with a runtime error
    [TestCase("1%", 1, 1)]
    [TestCase("50%", 8, 8)]
    public void EveryDimensionForm_ReachesItsTarget(string dimensions, int expectedWidth, int expectedHeight) {
      var result = this._Run("/load", this._Source(), "/resize", dimensions, "Resampler: Bicubic", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(expectedWidth, expectedHeight)));
    }

    [Test]
    public void FiltersChain_LeftToRight() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "XBR 3x", "/resize", "w128", "Bicubic", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png").Width, Is.EqualTo(128));
    }

    [Test]
    public void SeveralSavesInOneRun_AllProduceFiles() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "HQ 2x", "/save", "a.png", "/save", "b.bmp");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._Exists("a.png"), Is.True);
      Assert.That(this._Exists("b.bmp"), Is.True);
    }

    [Test]
    public void SeveralFilesInOneRun_AreProcessedIndependently() {
      this._Source("one.png", 8, 8);
      this._Source("two.png", 12, 12);

      var result = this._Run(
        "/load", "one.png", "/resize", "auto", "HQ 2x", "/save", "one-out.png",
        "/load", "two.png", "/resize", "auto", "HQ 2x", "/save", "two-out.png"
      );

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("one-out.png"), Is.EqualTo(new Size(16, 16)));
      Assert.That(this._SizeOf("two-out.png"), Is.EqualTo(new Size(24, 24)));
    }

    [TestCase("out.png")]
    [TestCase("out.bmp")]
    [TestCase("out.gif")]
    [TestCase("out.jpg")]
    [TestCase("out.tif")]
    public void TheOutputExtension_PicksTheContainerFormat(string target) {
      var result = this._Run("/load", this._Source(), "/save", target);

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(TestBitmaps.RawFormatOf(this._directory.File(target)), Is.EqualTo(TestBitmaps.FormatFor(target)));
    }

    [Test]
    public void APathWithSpaces_IsHandled() {
      this._Source("my source.png");

      var result = this._Run("/load", "my source.png", "/save", "my target.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._Exists("my target.png"), Is.True);
    }

    [Test]
    public void FilterParameters_AreAccepted() {
      var result = this._Run("/load", this._Source(), "/resize", "32x32", "Resampler: Bicubic(vbounds=wrap,hbounds=wrap,centered=0)", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(32, 32)));
    }

    #endregion

    #region scripts

    [Test]
    public void AScriptFile_RunsItsCommands() {
      this._Source();
      File.WriteAllText(this._directory.File("chain.irs"), "/load \"in.png\"\r\n/resize auto \"Upscaler: XBR 4x\"\r\n/save \"out.png\"\r\n");

      var result = this._Run("/script", "chain.irs");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(64, 64)));
    }

    [Test]
    public void AScriptFile_ComposesWithSurroundingCommands() {
      this._Source();
      File.WriteAllText(this._directory.File("filter.irs"), "/resize auto \"Upscaler: HQ 2x\"\r\n");

      var result = this._Run("/load", "in.png", "/script", "filter.irs", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(32, 32)));
    }

    [Test]
    public void ABadLineInAScript_IsReportedWithItsOrigin() {
      File.WriteAllText(this._directory.File("bad.irs"), "/resize auto \"NoSuchFilter\"\r\n");

      var result = this._Run("/script", "bad.irs");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.UnknownFilter));
      Assert.That(result.StandardError, Does.Contain("bad.irs"));
    }

    #endregion

    #region batches

    /// <summary>
    /// Issue #31: naming a set of files instead of writing the same lines out per sprite.
    /// </summary>
    [Test]
    public void AWildcardLoad_ProcessesEveryMatch() {
      this._Source("alpha.png");
      this._Source("beta.png");
      this._Source("gamma.png");
      Directory.CreateDirectory(Path.Combine(this._directory.Path, "big"));

      var result = this._Run("/load", "*.png", "/resize", "auto", "HQ 2x", "/save", @"big\*.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      foreach (var name in new[] { "alpha", "beta", "gamma" })
        Assert.That(TestBitmaps.SizeOf(Path.Combine(this._directory.Path, "big", name + ".png")), Is.EqualTo(new Size(32, 32)), name);
    }

    [Test]
    public void AWildcardLoad_NamesEachTargetAfterItsSource() {
      this._Source("one.png");
      this._Source("two.png");

      var result = this._Run("/load", "*.png", "/resize", "auto", "HQ 2x", "/save", "*.bmp");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._Exists("one.bmp"), Is.True);
      Assert.That(this._Exists("two.bmp"), Is.True);
    }

    [Test]
    public void AWildcardThatMatchesNothing_IsNotAnError() {
      var result = this._Run("/load", "*.tga", "/save", "*.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(result.StandardError, Does.Contain("No files matched"));
    }

    [Test]
    public void APlainFileName_StillProcessesExactlyOneFile() {
      this._Source("only.png");

      var result = this._Run("/load", "only.png", "/resize", "auto", "HQ 2x", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(32, 32)));
    }

    #endregion

    #region dropped scripts

    /// <summary>
    /// Issue #12: a script dropped on the executable, or double clicked through a file
    /// association, arrives as a bare file name. It used to be taken for an image to open.
    /// </summary>
    [Test]
    public void AScriptPassedAsABareFileName_IsRun() {
      this._Source();
      File.WriteAllText(this._directory.File("drop.irs"), "/load \"in.png\"\r\n/resize auto \"Upscaler: XBR 4x\"\r\n/save \"out.png\"\r\n");

      var result = this._Run("drop.irs");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(64, 64)));
    }

    [Test]
    public void ADroppedScript_ResolvesItsPathsAgainstItsOwnFolder() {
      this._Source();
      File.WriteAllText(this._directory.File("drop.irs"), "/load \"in.png\"\r\n/resize auto \"Upscaler: HQ 2x\"\r\n/save \"out.png\"\r\n");
      var elsewhere = Path.Combine(this._directory.Path, "elsewhere");
      Directory.CreateDirectory(elsewhere);

      // launched from a directory that is not the script's, the way Explorer does it
      var result = this._RunIn(elsewhere, Path.Combine(this._directory.Path, "drop.irs"));

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._Exists("out.png"), Is.True, "the script's own folder is what its relative paths mean");
    }

    [Test]
    public void SeveralDroppedScripts_RunInOrder() {
      this._Source();
      File.WriteAllText(this._directory.File("one.irs"), "/load \"in.png\"\r\n/resize auto \"Upscaler: HQ 2x\"\r\n/save \"one.png\"\r\n");
      File.WriteAllText(this._directory.File("two.irs"), "/load \"one.png\"\r\n/resize auto \"Upscaler: HQ 2x\"\r\n/save \"two.png\"\r\n");

      var result = this._Run("one.irs", "two.irs");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("two.png"), Is.EqualTo(new Size(64, 64)));
    }

    [Test]
    public void TheScriptExtension_IsMatchedCaseInsensitively() {
      this._Source();
      File.WriteAllText(this._directory.File("drop.IRS"), "/load \"in.png\"\r\n/save \"out.png\"\r\n");

      var result = this._Run("drop.IRS");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._Exists("out.png"), Is.True);
    }

    [Test]
    public void AnUnreadableScript_IsAnExitCodeRatherThanACrash() {
      var result = this._Run("/script", "absent.irs");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.ScriptFileCouldNotBeRead));
      Assert.That(result.StandardError, Does.Not.Contain("Unhandled Exception"));
    }

    #endregion

    #region help

    [TestCase("/?")]
    [TestCase("-?")]
    [TestCase("/help")]
    [TestCase("--help")]
    [TestCase("/h")]
    public void EveryHelpSwitch_PrintsTheHelpAndSucceeds(string switchText) {
      var result = this._Run(switchText);

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK));
      Assert.That(result.StandardOutput, Does.Contain("How to use"), "help that was asked for is the run's output");
    }

    [Test]
    public void TheHelp_ListsTheSupportedFilters() {
      var result = this._Run("/?");

      Assert.That(result.StandardOutput, Does.Contain("Supported filter methods"));
      Assert.That(result.StandardOutput, Does.Contain("Upscaler: HQ 2x"));
    }

    [Test]
    public void TheHelp_DoesNotAnnounceAnError() {
      var result = this._Run("/?");

      Assert.That(result.StandardOutput, Does.Not.Contain("ERROR"));
    }

    #endregion

    #region failure paths

    [Test]
    public void AnUnknownCommand_IsRejectedAndExplained() {
      var result = this._Run("/frobnicate", "in.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.UnknownParameter));
      Assert.That(result.StandardError, Does.Contain("ERROR"));
    }

    [Test]
    public void AnUnknownFilter_IsRejectedAndExplained() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "NoSuchFilter", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.UnknownFilter));
      Assert.That(result.StandardError, Does.Contain("ERROR"));
      Assert.That(this._Exists("out.png"), Is.False, "nothing may be written when the script never parsed");
    }

    // CLIExitCode is internal, so it cannot appear in a public signature - the expectation
    // travels as its numeric value instead.
    [TestCase("wobble", (int)CLIExitCode.InvalidTargetDimensions)]
    [TestCase("w72x92", (int)CLIExitCode.InvalidTargetDimensions)]
    [TestCase("65536x1", (int)CLIExitCode.CouldNotParseDimensionsAsWord)]
    public void MalformedDimensions_AreRejected(string dimensions, int expected) {
      var result = this._Run("/load", this._Source(), "/resize", dimensions, "Bicubic", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo((CLIExitCode)expected));
    }

    [Test]
    public void AMissingArgument_IsRejected() {
      var result = this._Run("/load");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.TooLessArguments));
    }

    [Test]
    public void AnUnknownFilterParameter_IsRejected() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "Bicubic(bogus=1)", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.UnknownParameter));
    }

    [Test]
    public void AnInvalidOutOfBoundsMode_IsRejected() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "Bicubic(vbounds=nope)", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.InvalidOutOfBoundsMode));
    }

    [Test]
    public void AMissingSourceFile_FailsAtRuntimeWithoutWritingAnything() {
      var result = this._Run("/load", "absent.png", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.RuntimeError));
      Assert.That(this._Exists("out.png"), Is.False);
    }

    [Test]
    public void ASourceThatIsNotAnImage_FailsAtRuntime() {
      File.WriteAllText(this._directory.File("garbage.png"), "not an image");

      var result = this._Run("/load", "garbage.png", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.RuntimeError));
    }

    [Test]
    public void SavingWithNothingLoaded_FailsAtRuntime() {
      var result = this._Run("/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.RuntimeError));
    }

    [Test]
    public void AFailingRun_StillPrintsTheHelpSoTheUserCanRecover() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "NoSuchFilter");

      Assert.That(result.StandardError, Does.Contain("How to use"));
    }

    #endregion

    #region piping

    /// <summary>
    /// Regression for issue #15: the progress reports used to go to standard output, so whatever
    /// consumed the pipe received several hundred bytes of text where it expected a PNG header.
    /// </summary>
    [Test]
    public void StdOut_CarriesNothingButTheImage() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "HQ 2x", "/stdout");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(result.StandardOutputIsPng, Is.True, "standard output must start with the PNG signature, not with a progress report");
      Assert.That(result.StandardOutputPngSize, Is.EqualTo(new Size(32, 32)));
    }

    [Test]
    public void StdOut_KeepsTheDiagnosticsOnStandardError() {
      var result = this._Run("/load", this._Source(), "/stdout");

      Assert.That(result.StandardError, Does.Contain("Executing the following script"));
      Assert.That(result.StandardOutputIsPng, Is.True);
    }

    [Test]
    public void StdIn_ReadsAnImageFromThePipe() {
      var source = File.ReadAllBytes(this._directory.File(this._Source()));

      var result = this._RunWithInput(source, "/stdin", "/resize", "auto", "HQ 2x", "/save", "out.png");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.OK), result.StandardError);
      Assert.That(this._SizeOf("out.png"), Is.EqualTo(new Size(32, 32)));
    }

    /// <summary>
    /// The end-to-end shape issue #15 was really about: one instance's output feeding the next.
    /// </summary>
    [Test]
    public void StdOutOfOneRun_FeedsStdInOfTheNext() {
      var first = this._Run("/load", this._Source(), "/resize", "auto", "HQ 2x", "/stdout");

      var second = this._RunWithInput(first.StandardOutputBytes, "/stdin", "/resize", "auto", "XBR 3x", "/stdout");

      Assert.That(second.Code, Is.EqualTo(CLIExitCode.OK), second.StandardError);
      Assert.That(second.StandardOutputIsPng, Is.True);
      Assert.That(second.StandardOutputPngSize, Is.EqualTo(new Size(96, 96)), "16 through HQ 2x is 32, through XBR 3x is 96");
    }

    [Test]
    public void SeveralStdOutCommands_EachEmitAnImage() {
      var single = this._Run("/load", this._Source(), "/stdout");

      var twice = this._Run("/load", this._Source(), "/stdout", "/stdout");

      Assert.That(twice.Code, Is.EqualTo(CLIExitCode.OK), twice.StandardError);
      Assert.That(twice.StandardOutputBytes.Length, Is.EqualTo(single.StandardOutputBytes.Length * 2));
    }

    [Test]
    public void AFailingRun_LeavesStandardOutputEmpty() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "NoSuchFilter", "/stdout");

      Assert.That(result.Code, Is.EqualTo(CLIExitCode.UnknownFilter));
      Assert.That(result.StandardOutputBytes, Is.Empty, "a consumer of the pipe must not receive the help text");
      Assert.That(result.StandardError, Is.Not.Empty);
    }

    #endregion

    #region diagnostics

    [Test]
    public void TheRunEchoesTheScriptItIsAboutToExecute() {
      var result = this._Run("/load", this._Source(), "/save", "out.png");

      Assert.That(result.StandardError, Does.Contain("Executing the following script"));
    }

    [Test]
    public void LoadDiagnostics_ReportTheRealContainerFormat() {
      var result = this._Run("/load", this._Source(), "/save", "out.png");

      Assert.That(result.StandardError, Does.Contain("Type   : PNG"), "MemoryBmp here would mean the in-memory copy is being described");
      Assert.That(result.StandardError, Does.Not.Contain("details unavailable"));
    }

    [Test]
    public void ResizeDiagnostics_NameTheFilterThatRan() {
      var result = this._Run("/load", this._Source(), "/resize", "auto", "HQ 2x", "/save", "out.png");

      Assert.That(result.StandardError, Does.Contain("Upscaler: HQ 2x"), "the canonical name, not the abbreviation that was typed");
    }

    #endregion

  }
}
