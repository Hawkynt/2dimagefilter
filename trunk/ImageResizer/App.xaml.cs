using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using Imager;

namespace ImageResizer {
  /// <summary>
  /// Holds the different pixel blitters.
  /// </summary>
  public struct PixelBlitter {
    /// <summary>
    /// The delegate that generates anew image from an existing one.
    /// </summary>
    public readonly Func<Bitmap, Bitmap> PixelCalculator;
    /// <summary>
    /// Width is multiplied with this value.
    /// </summary>
    public readonly int ScaleX;
    /// <summary>
    /// Height is multiplied with this value.
    /// </summary>
    public readonly int ScaleY;
    /// <summary>
    /// Creates a new pixel blitter structure.
    /// </summary>
    /// <param name="scaleX">Width is multiplied with this value.</param>
    /// <param name="scaleY">Height is multiplied with this value.</param>
    /// <param name="pixelCalculator">The delegate that generates anew image from an existing one.</param>
    public PixelBlitter(int scaleX, int scaleY, Func<Bitmap, Bitmap> pixelCalculator) {
      this.ScaleX = scaleX;
      this.ScaleY = scaleY;
      this.PixelCalculator = pixelCalculator;
    }
  }

  /// <summary>
  /// An image resizer
  /// </summary>
  public struct ImageResizerToken {
    public readonly string Name;
    public readonly InterpolationMode InterpolationMode;
    public readonly PixelBlitter Blitter;
    public ImageResizerToken(string name, PixelBlitter blitter, InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic) {
      Contract.Requires(name != null);
      this.Name = name;
      this.Blitter = blitter;
      this.InterpolationMode = interpolationMode;
    }
    public ImageResizerToken(string name, InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic)
      : this(name, new PixelBlitter(), interpolationMode) {
    }
    public override string ToString() {
      Contract.Assume(this.Name != null);
      return this.Name;
    }
  }

  /// <summary>
  /// Interaktionslogik für "App.xaml"
  /// </summary>
  public partial class App {
    /// <summary>
    /// A list containing all available resize methods
    /// </summary>
    public readonly List<ImageResizerToken> ImageResizers = new List<ImageResizerToken> {
      new ImageResizerToken("Pixel", InterpolationMode.NearestNeighbor),
      new ImageResizerToken("BiLinear", InterpolationMode.HighQualityBilinear),
      new ImageResizerToken("BiCubic",InterpolationMode.HighQualityBicubic)
    };

    /// <summary>
    /// An image loaded through the CLI
    /// </summary>
    public Bitmap CurrentImage;

    public App() {
      sPixel.AllowThresholds = true;
    }

    // ReSharper disable InconsistentNaming
    private void Application_Startup(object sender, StartupEventArgs e) {
      // ReSharper restore InconsistentNaming

      // add image filters from cImage
      Contract.Assume(cImage.Filters != null);
      Array.ForEach(cImage.Filters, filter => {
        if (!string.IsNullOrEmpty(filter.Name))
          this.ImageResizers.Add(new ImageResizerToken(filter.Name, new PixelBlitter(filter.ScaleX, filter.ScaleY, source => new cImage(source).FilterImage(filter.Name).ToBitmap())));
      });

      // parse command line if needed
      if (e == null || e.Args.Length <= 0) {
        // no command line given
        return;
      }

      var arguments = e.Args;
      var i = 0;
      var length = arguments.Length;
      while (i < length) {
        var command = arguments[i++].ToUpperInvariant();
        switch (command) {
          case "/LOAD": {
            if (length - i < 1) {
              this._ShowHelp();
              return;
            }
            var filename = arguments[i++];
            if (filename == null) {
              this._ShowHelp();
              return;
            }
            this.CurrentImage = (Bitmap)Image.FromFile(filename);
            break;
          }
          case "/RESIZE": {
            if (length - i < 2) {
              this._ShowHelp();
              return;
            }
            var dimensions = arguments[i++].Trim();
            var filterName = arguments[i++].Trim();
            var positionOfX = dimensions.IndexOf('x');
            if (positionOfX <= 0) {
              this._ShowHelp();
              return;
            }
            int intX, intY;
            if (!int.TryParse(dimensions.Substring(0, positionOfX), out intX) || !int.TryParse(dimensions.Substring(positionOfX + 1), out intY)) {
              this._ShowHelp();
              return;
            }
            int repeat;
            positionOfX = filterName.IndexOf('(');
            if (positionOfX > 0 && filterName.EndsWith(")")) {
              if (!int.TryParse(filterName.Substring(positionOfX + 1, filterName.Length - positionOfX - 2), out repeat))
                repeat = 1;

              filterName = filterName.Substring(0, positionOfX);
              if (repeat < 1)
                repeat = 1;
            } else
              repeat = 1;
            if (this.CurrentImage == null) {
              MessageBox.Show("Unable to resize file: there is nothing to resize!", "Fatal Error");
              return;
            }
            Console.WriteLine(@"{0} {1} {2} {3}", intX, intY, repeat, filterName);
            Contract.Assume(this.ImageResizers != null);
            var matches = this.ImageResizers.Where(resizer => string.Compare(resizer.Name, filterName, true) == 0).ToArray();
            if (matches.Length <= 0) {
              MessageBox.Show(
                "Unable to resize file: there is no filter named '" + filterName + "'!", "Fatal Error");
              return;
            }
            for (var j = 0; j < repeat; j++) {
              var image = this.CurrentImage;
              this.CurrentImage = FilterAndResizeImage(image, matches[0], intX, intY);
              image.Dispose();
            }
            break;
          }
          case "/SAVE": {
            if (length - i < 1) {
              this._ShowHelp();
              return;
            }
            var filename = arguments[i++];
            if (filename == null) {
              this._ShowHelp();
              return;
            }
            var ext = Path.GetExtension(filename);
            var extension = ext == null ? null : ext.ToUpperInvariant();
            var image = this.CurrentImage;
            if (image == null) {
              MessageBox.Show("Unable to save file: there is nothing to save!", "Fatal Error");
              return;
            }
            if (extension == ".JPG" || extension == ".JPEG") {
              var codecs = ImageCodecInfo.GetImageEncoders();
              codecs = codecs.Where(info => info != null && info.MimeType == "image/jpeg").ToArray();
              if (codecs.Length <= 0) {
                MessageBox.Show("System has no support to save as JPEG !", "Fatal Error");
                return;
              }
              Contract.Assume(Encoder.Quality != null);
              image.Save(filename, codecs[0], new EncoderParameters { Param = new[] { new EncoderParameter(Encoder.Quality, (long)100) } });
            } else
              image.Save(filename);
            break;
          }
          case "/EXIT": {
            this.Shutdown(0);
            break;
          }
          default: {
            this._ShowHelp();
            return;
          }
        }
      }
    }

    private void _ShowHelp() {
      System.Reflection.Assembly objAssembly = System.Reflection.Assembly.GetExecutingAssembly();
      var lines = string.Join(
        Environment.NewLine,
        new[] {
          Path.GetFileName(objAssembly.Location)
            + " [/load <source>] [/resize <x>x<y> <method>[(<repeat>)]] [/save <target>] ... [/exit]",
          "  + /load    - loads a file into the source buffer",
          "    + <source> - the source file to resize",
          "  + /save    - saves the image in the source buffer to a file",
          "    + <target> - the target file to write",
          "  + /resize  - resizes the image in the source buffer and stores the result back to the source buffer",
          "    + <x>      - the final width in pixels for the target, 0 for auto",
          "    + <y>      - the final height in pixels for the target, 0 for auto",
          "    + <method> - the method to use",
          "    + <repeat> - the number of repetitions using this method",
          "  + /exit      - quits the program without showing the gui",
          "You can load and process multiple files at once by loading after saving again.",
          "  + ie. /load 1.bmp /resize 10x10 Pixel /save 1.jpg /load 2.bmp /resize 10x10 Pixel /save 2.jpg",
          "You can also save to multiple files by adding another save parameter.",
          "  + ie. /load 1.bmp /resize 10x10 Pixel /save 1.jpg /save 2.jpg",
          "Even preprocessing using multiple filters is possible by adding another resize parameter.",
          "  + ie. /load 1.bmp /resize 10x10 Pixel /resize 0x0 Scale2x /save 1.jpg"
        });
      MessageBox.Show(lines, "Command-Line Help");
      Console.WriteLine(lines);
    }

    /// <summary>
    /// Filters a given bitmap and resizes it.
    /// </summary>
    /// <param name="source">The source image.</param>
    /// <param name="imageResizerToken">The image resizer.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>A new bitmap filtered when requested and resized to the given target dimensions.</returns>
    public static Bitmap FilterAndResizeImage(Bitmap source, ImageResizerToken imageResizerToken, int width, int height) {
      var filter = imageResizerToken.Blitter.PixelCalculator;
      if (filter == null)
        return (_ResizeImage(source, width, height, imageResizerToken.InterpolationMode));
      using (var temporaryImage = filter(source))
        return (_ResizeImage(temporaryImage, width, height, imageResizerToken.InterpolationMode));
    }

    /// <summary>
    /// Resizes a given image.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="width">The width; 0 means keep original.</param>
    /// <param name="height">The height; 0 means keep original.</param>
    /// <param name="mode">The mode.</param>
    /// <returns>A new bitmap with the desired dimensions.</returns>
    private static Bitmap _ResizeImage(Image source, int width, int height, InterpolationMode mode) {
      Contract.Requires(source != null);
      var result = new Bitmap((width == 0 ? source.Width : width), (height == 0 ? source.Height : height));
      using (var graphics = Graphics.FromImage(result)) {
        //set the resize quality modes to high quality                
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = mode;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //draw the image into the target bitmap                
        //graphics.DrawImage(source, 0, 0, result.Width, result.Height);
        // FIXME: this is a hack to prevent the microsoft bug from creating a white pixel on top and left border (see http://forums.asp.net/t/1031961.aspx/1)
        graphics.DrawImage(source, -1, -1, result.Width + 1, result.Height + 1);
      }
      return (result);
    }

  }
}
