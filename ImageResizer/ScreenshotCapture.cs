#region (c)2008-2026 Hawkynt
/*
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.
 */
#endregion

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Classes;

namespace ImageResizer {
  /// <summary>
  /// Produces the deterministic application screenshot used by the repository documentation.
  /// </summary>
  internal static class ScreenshotCapture {

    private const int _DEMO_WIDTH = 96;
    private const int _DEMO_HEIGHT = 64;

    /// <summary>
    /// Renders the real main form with deterministic pixel-art input and saves its client area.
    /// </summary>
    /// <param name="outputPath">Destination PNG file.</param>
    public static void Save(string outputPath) {
      if (string.IsNullOrWhiteSpace(outputPath))
        throw new ArgumentException("A screenshot output path is required.", nameof(outputPath));

      var fullOutputPath = Path.GetFullPath(outputPath);
      var outputDirectory = Path.GetDirectoryName(fullOutputPath);
      if (!string.IsNullOrEmpty(outputDirectory))
        Directory.CreateDirectory(outputDirectory);

      var demoPath = Path.Combine(Path.GetTempPath(), "2dimagefilter-screenshot-" + Guid.NewGuid().ToString("N") + ".png");
      try {
        using (var demo = CreateDemoImage())
          demo.Save(demoPath, ImageFormat.Png);

        _ConfigureDemo();

        using (var view = new MainForm(demoPath)) {
          view.StartPosition = FormStartPosition.Manual;
          view.Location = Point.Empty;
          view.Size = new Size(1180, 760);
          view.Show();

          // Loading a source schedules the normal 300 ms auto-preview. Pump the WinForms queue so
          // that the screenshot exercises the same path as an interactive launch rather than
          // capturing a form before its asynchronous preview has had a chance to finish.
          _PumpMessages(TimeSpan.FromSeconds(2));
          view.PerformLayout();
          view.Refresh();
          Application.DoEvents();

          using (var screenshot = new Bitmap(view.ClientSize.Width, view.ClientSize.Height, PixelFormat.Format32bppArgb)) {
            view.DrawToBitmap(screenshot, new Rectangle(Point.Empty, view.ClientSize));
            screenshot.Save(fullOutputPath, ImageFormat.Png);
          }
        }
      } finally {
        Config.Reset();
        try {
          File.Delete(demoPath);
        } catch (IOException) {
          // The temporary demo is disposable documentation input; a cleanup race is not fatal.
        } catch (UnauthorizedAccessException) {
          // Same as above: never turn a successful screenshot into a failure during cleanup.
        }
      }
    }

    /// <summary>
    /// Creates compact source art with hard edges, diagonals and a checker pattern so scaler
    /// behavior is visible in the generated documentation screenshot.
    /// </summary>
    /// <returns>A new caller-owned bitmap.</returns>
    internal static Bitmap CreateDemoImage() {
      var bitmap = new Bitmap(_DEMO_WIDTH, _DEMO_HEIGHT, PixelFormat.Format32bppArgb);
      using (var graphics = Graphics.FromImage(bitmap)) {
        graphics.Clear(Color.FromArgb(35, 45, 73));

        _FillPolygon(graphics, Color.FromArgb(58, 71, 104),
          new Point(0, 40), new Point(17, 25), new Point(31, 39), new Point(48, 22),
          new Point(67, 40), new Point(82, 28), new Point(95, 39), new Point(95, 52), new Point(0, 52));
        _FillPolygon(graphics, Color.FromArgb(39, 52, 72),
          new Point(0, 47), new Point(15, 35), new Point(29, 47), new Point(43, 33),
          new Point(59, 48), new Point(77, 34), new Point(95, 47), new Point(95, 55), new Point(0, 55));

        _FillRectangle(graphics, Color.FromArgb(28, 37, 48), 0, 49, 96, 15);
        _FillRectangle(graphics, Color.FromArgb(68, 124, 88), 0, 49, 96, 4);

        var light = Color.FromArgb(246, 221, 128);
        foreach (var star in new[] {
          new Point(8, 7), new Point(19, 13), new Point(31, 5), new Point(43, 10),
          new Point(54, 4), new Point(67, 13), new Point(83, 7), new Point(90, 17),
          new Point(13, 24), new Point(36, 20), new Point(59, 24), new Point(76, 21),
        })
          _FillRectangle(graphics, light, star.X, star.Y, 2, 2);

        _FillRectangle(graphics, light, 72, 8, 12, 12);
        _FillRectangle(graphics, Color.FromArgb(35, 45, 73), 72, 8, 4, 4);

        var dark = Color.FromArgb(18, 23, 31);
        var orange = Color.FromArgb(222, 123, 72);
        var red = Color.FromArgb(177, 69, 73);
        var blue = Color.FromArgb(74, 141, 190);
        var white = Color.FromArgb(233, 239, 245);

        // Tiny hero sprite with deliberately hard one-pixel diagonals and high-contrast details.
        _FillRectangle(graphics, dark, 42, 30, 12, 19);
        _FillRectangle(graphics, orange, 44, 26, 8, 8);
        _FillRectangle(graphics, red, 43, 25, 10, 3);
        _FillRectangle(graphics, white, 45, 29, 2, 2);
        _FillRectangle(graphics, white, 50, 29, 2, 2);
        _FillRectangle(graphics, blue, 43, 34, 10, 8);
        _FillRectangle(graphics, blue, 39, 35, 4, 3);
        _FillRectangle(graphics, blue, 53, 35, 4, 3);
        _FillRectangle(graphics, red, 43, 42, 4, 7);
        _FillRectangle(graphics, red, 50, 42, 4, 7);
        _FillRectangle(graphics, light, 57, 35, 7, 2);
        _FillRectangle(graphics, light, 62, 33, 2, 6);

        for (var y = 0; y < 12; ++y)
          for (var x = 0; x < 12; ++x)
            bitmap.SetPixel(7 + x, 39 + y, ((x + y) & 1) == 0 ? white : dark);

        _FillRectangle(graphics, orange, 75, 41, 12, 10);
        _FillRectangle(graphics, red, 77, 39, 8, 2);
        _FillRectangle(graphics, dark, 78, 44, 2, 2);
        _FillRectangle(graphics, dark, 83, 44, 2, 2);
      }

      return bitmap;
    }

    private static void _ConfigureDemo() {
      Config.Reset();
      Config.SourceSizeMode = PictureBoxSizeMode.Zoom;
      Config.TargetSizeMode = PictureBoxSizeMode.Zoom;
      Config.WindowBounds = new Rectangle(32, 32, 1180, 760);
      Config.WindowState = FormWindowState.Normal;
      Config.MethodCategory = "Upscaler";
      Config.ResizeMethod = "Upscaler: HQ 2x";
      Config.UseThresholds = true;
      Config.UseCenteredGrid = false;
      Config.KeepAspect = true;
      Config.RepetitionCount = 1;
      Config.Radius = 1f;
    }

    private static void _PumpMessages(TimeSpan duration) {
      var timer = Stopwatch.StartNew();
      do {
        Application.DoEvents();
        Thread.Sleep(20);
      } while (timer.Elapsed < duration);
    }

    private static void _FillRectangle(Graphics graphics, Color color, int x, int y, int width, int height) {
      using (var brush = new SolidBrush(color))
        graphics.FillRectangle(brush, x, y, width, height);
    }

    private static void _FillPolygon(Graphics graphics, Color color, params Point[] points) {
      using (var brush = new SolidBrush(color))
        graphics.FillPolygon(brush, points);
    }
  }
}
