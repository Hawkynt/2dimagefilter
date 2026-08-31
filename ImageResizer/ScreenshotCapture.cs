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
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Classes;

using Hawkynt.ColorProcessing.Dithering;
using Hawkynt.ColorProcessing.Quantization;

using ImageResizer.UserControls;
using ImageResizer.Windows;

namespace ImageResizer {
  /// <summary>
  /// Produces deterministic application screenshots used by the repository documentation.
  /// </summary>
  internal static class ScreenshotCapture {

    private const int _PIXEL_ART_WIDTH = 96;
    private const int _PIXEL_ART_HEIGHT = 64;
    private const int _QUANTIZATION_WIDTH = 320;
    private const int _QUANTIZATION_HEIGHT = 200;

    /// <summary>
    /// Renders the real main form with deterministic pixel-art input and saves its client area.
    /// Kept for the original single-screenshot command.
    /// </summary>
    public static void Save(string outputPath) {
      if (string.IsNullOrWhiteSpace(outputPath))
        throw new ArgumentException("A screenshot output path is required.", nameof(outputPath));

      var demoPath = Path.Combine(Path.GetTempPath(), "2dimagefilter-screenshot-" + Guid.NewGuid().ToString("N") + ".png");
      try {
        using (var demo = CreatePixelArtDemoImage())
          demo.Save(demoPath, ImageFormat.Png);

        _SaveMainWindow(Path.GetFullPath(outputPath), demoPath);
      } finally {
        Config.Reset();
        _TryDelete(demoPath);
      }
    }

    /// <summary>
    /// Generates the documentation demo inputs and every standalone-application window screenshot.
    /// The demo inputs are intentionally left in <paramref name="outputDirectory"/> so CI can upload
    /// them alongside the captures when a rendering failure needs inspecting.
    /// </summary>
    public static void SaveAll(string outputDirectory) {
      if (string.IsNullOrWhiteSpace(outputDirectory))
        throw new ArgumentException("A screenshot output directory is required.", nameof(outputDirectory));

      var directory = Path.GetFullPath(outputDirectory);
      Directory.CreateDirectory(directory);

      var pixelArtDemoPath = Path.Combine(directory, "demo-pixel-art.png");
      var quantizationDemoPath = Path.Combine(directory, "demo-quantization.png");
      using (var demo = CreatePixelArtDemoImage())
        demo.Save(pixelArtDemoPath, ImageFormat.Png);
      using (var demo = CreateQuantizationDemoImage())
        demo.Save(quantizationDemoPath, ImageFormat.Png);

      try {
        _SaveMainWindow(Path.Combine(directory, "image-resizer.png"), pixelArtDemoPath);
        _SaveReduceColoursWindow(Path.Combine(directory, "reduce-colours.png"), quantizationDemoPath);
      } finally {
        Config.Reset();
      }
    }

    /// <summary>
    /// Compatibility name retained for the first screenshot tests.
    /// </summary>
    internal static Bitmap CreateDemoImage() => CreatePixelArtDemoImage();

    /// <summary>
    /// Creates compact source art with hard edges, diagonals and a checker pattern so scaler
    /// behavior is visible in the generated main-window screenshot.
    /// </summary>
    internal static Bitmap CreatePixelArtDemoImage() {
      var bitmap = new Bitmap(_PIXEL_ART_WIDTH, _PIXEL_ART_HEIGHT, PixelFormat.Format32bppArgb);
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

    /// <summary>
    /// Creates a small synthetic photo/test-card with thousands of colours, smooth gradients,
    /// hard edges and neutral ramps. It makes palette reduction and dithering differences visible
    /// while remaining deterministic and cheap enough for every branch push.
    /// </summary>
    internal static Bitmap CreateQuantizationDemoImage() {
      var bitmap = new Bitmap(_QUANTIZATION_WIDTH, _QUANTIZATION_HEIGHT, PixelFormat.Format32bppArgb);
      for (var y = 0; y < bitmap.Height; ++y) {
        var fy = y / (double)(bitmap.Height - 1);
        for (var x = 0; x < bitmap.Width; ++x) {
          var fx = x / (double)(bitmap.Width - 1);
          var wave = (Math.Sin((fx * 3.0 + fy * 1.75) * Math.PI) + 1.0) * 0.5;
          var r = (int)Math.Round(255.0 * Math.Min(1.0, 0.08 + 0.92 * fx));
          var g = (int)Math.Round(255.0 * Math.Min(1.0, 0.10 + 0.90 * fy));
          var b = (int)Math.Round(255.0 * (0.12 + 0.88 * wave));
          bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
        }
      }

      using (var graphics = Graphics.FromImage(bitmap)) {
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        _FillEllipse(graphics, Color.FromArgb(220, 250, 188, 72), 26, 26, 82, 82);
        _FillEllipse(graphics, Color.FromArgb(210, 54, 164, 214), 84, 52, 118, 92);
        _FillEllipse(graphics, Color.FromArgb(190, 224, 72, 126), 170, 24, 112, 112);
        _FillRectangle(graphics, Color.FromArgb(225, 22, 31, 42), 18, 145, 284, 28);

        for (var i = 0; i < 16; ++i) {
          var level = i * 17;
          _FillRectangle(graphics, Color.FromArgb(level, level, level), 22 + i * 17, 150, 15, 18);
        }

        using (var pen = new Pen(Color.FromArgb(230, 248, 248, 248), 2f))
          for (var x = -40; x < bitmap.Width; x += 18)
            graphics.DrawLine(pen, x, 0, x + 120, bitmap.Height);
      }

      return bitmap;
    }

    private static void _SaveMainWindow(string outputPath, string demoPath) {
      _EnsureOutputDirectory(outputPath);
      _ConfigureMainDemo();

      using (var view = new MainForm(demoPath)) {
        view.StartPosition = FormStartPosition.Manual;
        view.Location = Point.Empty;
        view.Size = new Size(1180, 760);
        view.Show();

        var target = _GetField<ImageWithDetails>(view, "iwhTargetImage");
        _PumpMessagesUntil(
          () => target.Image != null && target.StatusText.StartsWith("Preview (", StringComparison.Ordinal),
          TimeSpan.FromSeconds(10),
          "The main-window demo preview did not finish rendering."
        );

        _SaveForm(view, outputPath);
      }
    }

    private static void _SaveReduceColoursWindow(string outputPath, string demoPath) {
      _EnsureOutputDirectory(outputPath);
      using (var source = new Bitmap(demoPath))
      using (var view = new ReduceColorsWindow(source)) {
        view.StartPosition = FormStartPosition.Manual;
        view.Location = Point.Empty;
        view.Size = new Size(1400, 820);
        view.Show();
        _PumpMessages(TimeSpan.FromMilliseconds(300));

        var panel = _GetField<Control>(view, "_panel");
        _PrepareReductionDemo(panel);
        var status = _GetField<Label>(panel, "_detailStatus");
        _PumpMessagesUntil(
          () => status.Text.StartsWith("Detail:", StringComparison.Ordinal),
          TimeSpan.FromSeconds(15),
          "The quantization/dithering detail preview did not finish rendering."
        );

        _SaveForm(view, outputPath);
      }
    }

    private static void _PrepareReductionDemo(Control panel) {
      var slider = _GetField<TrackBar>(panel, "_paletteSlider");
      slider.Value = 16;
      _InvokePrivate(panel, "_OnPaletteDebounceTick", null, EventArgs.Empty);
      Application.DoEvents();

      var quantizer = QuantizerRegistry.FindByName("Median Cut")
        ?? QuantizerRegistry.All.First(q => !q.DeclaringType.ContainsGenericParameters);
      var quantStrip = _GetField<FlowLayoutPanel>(panel, "_quantStrip");
      var quantTile = _FindTile(quantStrip, quantizer.Name);
      _InvokePrivate(panel, "_OnQuantizerPicked", quantizer, quantTile);
      Application.DoEvents();

      var ditherer = DithererRegistry.FindByName("ErrorDiffusion_FloydSteinberg")
        ?? DithererRegistry.FindByNameContaining("FloydSteinberg").FirstOrDefault(d => !d.DeclaringType.ContainsGenericParameters)
        ?? DithererRegistry.GetByType(DitheringType.ErrorDiffusion).First(d => !d.DeclaringType.ContainsGenericParameters);
      var ditherStrip = _GetField<FlowLayoutPanel>(panel, "_ditherStrip");
      var ditherTile = _FindTile(ditherStrip, ditherer.Name);
      _InvokePrivate(panel, "_OnDithererPicked", ditherer, ditherTile);
    }

    private static Control _FindTile(Control strip, string labelText) {
      foreach (Control tile in strip.Controls)
        foreach (Control child in tile.Controls)
          if (child is Label label && string.Equals(label.Text, labelText, StringComparison.Ordinal))
            return tile;

      throw new InvalidOperationException("Could not find generated preview tile '" + labelText + "'.");
    }

    private static T _GetField<T>(object owner, string name) where T : class {
      var field = owner.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
      if (field == null)
        throw new MissingFieldException(owner.GetType().FullName, name);

      var value = field.GetValue(owner) as T;
      if (value == null)
        throw new InvalidOperationException("Field '" + name + "' did not contain a " + typeof(T).Name + ".");

      return value;
    }

    private static void _InvokePrivate(object owner, string name, params object[] arguments) {
      var method = owner.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
      if (method == null)
        throw new MissingMethodException(owner.GetType().FullName, name);

      method.Invoke(owner, arguments);
    }

    private static void _ConfigureMainDemo() {
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

    private static void _SaveForm(Form view, string outputPath) {
      view.PerformLayout();
      view.Refresh();
      Application.DoEvents();

      using (var screenshot = new Bitmap(view.ClientSize.Width, view.ClientSize.Height, PixelFormat.Format32bppArgb)) {
        view.DrawToBitmap(screenshot, new Rectangle(Point.Empty, view.ClientSize));
        screenshot.Save(outputPath, ImageFormat.Png);
      }
    }

    private static void _PumpMessagesUntil(Func<bool> condition, TimeSpan timeout, string failureMessage) {
      var timer = Stopwatch.StartNew();
      while (!condition()) {
        if (timer.Elapsed >= timeout)
          throw new TimeoutException(failureMessage);

        Application.DoEvents();
        Thread.Sleep(20);
      }
      Application.DoEvents();
    }

    private static void _PumpMessages(TimeSpan duration) {
      var timer = Stopwatch.StartNew();
      do {
        Application.DoEvents();
        Thread.Sleep(20);
      } while (timer.Elapsed < duration);
    }

    private static void _EnsureOutputDirectory(string outputPath) {
      var directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
      if (!string.IsNullOrEmpty(directory))
        Directory.CreateDirectory(directory);
    }

    private static void _TryDelete(string path) {
      try {
        File.Delete(path);
      } catch (IOException) {
      } catch (UnauthorizedAccessException) {
      }
    }

    private static void _FillRectangle(Graphics graphics, Color color, int x, int y, int width, int height) {
      using (var brush = new SolidBrush(color))
        graphics.FillRectangle(brush, x, y, width, height);
    }

    private static void _FillEllipse(Graphics graphics, Color color, int x, int y, int width, int height) {
      using (var brush = new SolidBrush(color))
        graphics.FillEllipse(brush, x, y, width, height);
    }

    private static void _FillPolygon(Graphics graphics, Color color, params Point[] points) {
      using (var brush = new SolidBrush(color))
        graphics.FillPolygon(brush, points);
    }
  }
}
