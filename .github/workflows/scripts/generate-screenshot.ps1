param(
  [Parameter(Mandatory = $true)]
  [string]$Executable,

  [Parameter(Mandatory = $true)]
  [string]$Output
)

$ErrorActionPreference = 'Stop'

Add-Type -AssemblyName System.Drawing

Add-Type -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public static class WindowScreenshot {
  [StructLayout(LayoutKind.Sequential)]
  private struct RECT {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
  }

  [DllImport("user32.dll")]
  private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

  [DllImport("user32.dll")]
  private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, uint flags);

  public static void Save(IntPtr windowHandle, string fileName) {
    if (windowHandle == IntPtr.Zero)
      throw new ArgumentException("A valid window handle is required.", nameof(windowHandle));

    if (!GetWindowRect(windowHandle, out var bounds))
      throw new InvalidOperationException("Could not determine the application window bounds.");

    var width = bounds.Right - bounds.Left;
    var height = bounds.Bottom - bounds.Top;
    if (width <= 0 || height <= 0)
      throw new InvalidOperationException($"Application window has invalid bounds {width}x{height}.");

    using (var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
    using (var graphics = Graphics.FromImage(bitmap)) {
      var hdc = graphics.GetHdc();
      try {
        const uint PW_RENDERFULLCONTENT = 0x00000002;
        if (!PrintWindow(windowHandle, hdc, PW_RENDERFULLCONTENT))
          throw new InvalidOperationException("PrintWindow failed to render the application window.");
      } finally {
        graphics.ReleaseHdc(hdc);
      }

      bitmap.Save(fileName, ImageFormat.Png);
    }
  }
}
'@ -ReferencedAssemblies System.Drawing

function New-DemoImage {
  param([Parameter(Mandatory = $true)][string]$Path)

  $bitmap = [System.Drawing.Bitmap]::new(96, 64)
  $graphics = [System.Drawing.Graphics]::FromImage($bitmap)

  try {
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::None
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::Half

    $sky = [System.Drawing.Color]::FromArgb(255, 35, 45, 73)
    $far = [System.Drawing.Color]::FromArgb(255, 58, 71, 104)
    $near = [System.Drawing.Color]::FromArgb(255, 39, 52, 72)
    $ground = [System.Drawing.Color]::FromArgb(255, 28, 37, 48)
    $grass = [System.Drawing.Color]::FromArgb(255, 68, 124, 88)
    $light = [System.Drawing.Color]::FromArgb(255, 246, 221, 128)
    $orange = [System.Drawing.Color]::FromArgb(255, 222, 123, 72)
    $red = [System.Drawing.Color]::FromArgb(255, 177, 69, 73)
    $blue = [System.Drawing.Color]::FromArgb(255, 74, 141, 190)
    $white = [System.Drawing.Color]::FromArgb(255, 233, 239, 245)
    $dark = [System.Drawing.Color]::FromArgb(255, 18, 23, 31)

    $graphics.Clear($sky)

    foreach ($star in @(
      @(8, 7), @(19, 13), @(31, 5), @(43, 10), @(54, 4), @(67, 13), @(83, 7), @(90, 17),
      @(13, 24), @(36, 20), @(59, 24), @(76, 21)
    )) {
      $graphics.FillRectangle([System.Drawing.SolidBrush]::new($light), $star[0], $star[1], 2, 2)
    }

    $lightBrush = [System.Drawing.SolidBrush]::new($light)
    try {
      $graphics.FillRectangle($lightBrush, 72, 8, 12, 12)
      $graphics.FillRectangle([System.Drawing.SolidBrush]::new($sky), 72, 8, 4, 4)
    } finally {
      $lightBrush.Dispose()
    }

    $graphics.FillPolygon([System.Drawing.SolidBrush]::new($far), @(
      [System.Drawing.Point]::new(0, 39),
      [System.Drawing.Point]::new(17, 25),
      [System.Drawing.Point]::new(31, 38),
      [System.Drawing.Point]::new(48, 22),
      [System.Drawing.Point]::new(67, 39),
      [System.Drawing.Point]::new(82, 28),
      [System.Drawing.Point]::new(95, 38),
      [System.Drawing.Point]::new(95, 48),
      [System.Drawing.Point]::new(0, 48)
    ))

    $graphics.FillPolygon([System.Drawing.SolidBrush]::new($near), @(
      [System.Drawing.Point]::new(0, 45),
      [System.Drawing.Point]::new(15, 35),
      [System.Drawing.Point]::new(29, 45),
      [System.Drawing.Point]::new(43, 33),
      [System.Drawing.Point]::new(59, 46),
      [System.Drawing.Point]::new(77, 34),
      [System.Drawing.Point]::new(95, 45),
      [System.Drawing.Point]::new(95, 53),
      [System.Drawing.Point]::new(0, 53)
    ))

    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($ground), 0, 48, 96, 16)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($grass), 0, 48, 96, 4)
    for ($x = 0; $x -lt 96; $x += 8) {
      $graphics.FillRectangle([System.Drawing.SolidBrush]::new($near), $x, 56, 4, 4)
      $graphics.FillRectangle([System.Drawing.SolidBrush]::new($far), $x + 4, 60, 4, 4)
    }

    # Tiny hero sprite. The deliberately hard one-pixel diagonals make scaler differences obvious.
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($dark), 42, 30, 12, 18)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($orange), 44, 26, 8, 8)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($red), 43, 25, 10, 3)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($white), 45, 29, 2, 2)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($white), 50, 29, 2, 2)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($blue), 43, 34, 10, 8)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($blue), 39, 35, 4, 3)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($blue), 53, 35, 4, 3)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($red), 43, 42, 4, 6)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($red), 50, 42, 4, 6)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($light), 57, 35, 7, 2)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($light), 62, 33, 2, 6)

    # A few high-contrast tiles provide circles, diagonals and checker patterns in one tiny source.
    for ($y = 0; $y -lt 12; ++$y) {
      for ($x = 0; $x -lt 12; ++$x) {
        if ((($x + $y) % 2) -eq 0) {
          $bitmap.SetPixel(7 + $x, 39 + $y, $white)
        } else {
          $bitmap.SetPixel(7 + $x, 39 + $y, $dark)
        }
      }
    }

    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($orange), 75, 41, 12, 10)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($red), 77, 39, 8, 2)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($dark), 78, 44, 2, 2)
    $graphics.FillRectangle([System.Drawing.SolidBrush]::new($dark), 83, 44, 2, 2)

    $bitmap.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
  } finally {
    $graphics.Dispose()
    $bitmap.Dispose()
  }
}

function Write-DemoConfiguration {
  param([Parameter(Mandatory = $true)][string]$ExecutablePath)

  $configurationPath = Join-Path (Split-Path -Parent $ExecutablePath) 'config.xml'
  @'
<?xml version="1.0" encoding="utf-8"?>
<Configuration>
  <SourceSizeMode value="Zoom" />
  <TargetSizeMode value="Zoom" />
  <WindowBounds value="32,32,1180,760" />
  <WindowState value="Normal" />
  <ResizeMethod value="Upscaler: HQ 2x" />
  <MethodCategory value="Upscaler" />
  <UseThresholds value="True" />
  <UseCenteredGrid value="False" />
  <KeepAspect value="True" />
  <RepetitionCount value="1" />
  <Radius value="1" />
</Configuration>
'@ | Set-Content -Path $configurationPath -Encoding UTF8
}

$executablePath = (Resolve-Path $Executable).Path
$outputPath = [System.IO.Path]::GetFullPath($Output)
$outputDirectory = Split-Path -Parent $outputPath
if ($outputDirectory)
  [System.IO.Directory]::CreateDirectory($outputDirectory) | Out-Null

$demoPath = Join-Path $env:RUNNER_TEMP '2dimagefilter-screenshot-demo.png'
New-DemoImage -Path $demoPath
Write-DemoConfiguration -ExecutablePath $executablePath

$process = $null
try {
  $process = Start-Process -FilePath $executablePath -ArgumentList @($demoPath) -PassThru

  try { $null = $process.WaitForInputIdle(10000) } catch { }

  $deadline = [DateTime]::UtcNow.AddSeconds(20)
  do {
    Start-Sleep -Milliseconds 200
    $process.Refresh()
    if ($process.HasExited)
      throw "ImageResizer exited before its main window was ready (exit code $($process.ExitCode))."
  } while ($process.MainWindowHandle -eq 0 -and [DateTime]::UtcNow -lt $deadline)

  if ($process.MainWindowHandle -eq 0)
    throw 'ImageResizer did not create a main window within 20 seconds.'

  # Loading schedules a 300 ms preview. The deterministic 96x64 demo completes quickly, but leave
  # a little headroom for a cold GitHub-hosted Windows runner before capturing the final target pane.
  Start-Sleep -Seconds 3
  $process.Refresh()

  [WindowScreenshot]::Save($process.MainWindowHandle, $outputPath)

  $result = Get-Item $outputPath
  if ($result.Length -lt 10000)
    throw "Generated screenshot is suspiciously small ($($result.Length) bytes)."
} finally {
  if ($process -and !$process.HasExited) {
    $null = $process.CloseMainWindow()
    if (!$process.WaitForExit(5000))
      $process.Kill()
  }

  Remove-Item $demoPath -Force -ErrorAction SilentlyContinue
}
