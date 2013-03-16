using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Imager;
using Imager.Classes;
using Imager.Interface;


namespace ImageResizer {
  class Program {
    [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow")]
    private static extern IntPtr _GetConsoleWindow();

    /// <summary>
    /// A list containing all available resize methods
    /// </summary>
    internal static readonly List<ImageResizerToken> IMAGE_RESIZERS = new List<ImageResizerToken> {
      new ImageResizerToken("Pixel", InterpolationMode.NearestNeighbor),
      new ImageResizerToken("BiLinear", InterpolationMode.HighQualityBilinear),
      new ImageResizerToken("BiCubic",InterpolationMode.HighQualityBicubic)
    };

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args) {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);


      // add image filters from cImage
      Contract.Assume(cImage.Filters != null);
      Array.ForEach(cImage.Filters, filter => {
        if (!string.IsNullOrEmpty(filter.Name))
          IMAGE_RESIZERS.Add(
            new ImageResizerToken(
              filter.Name,
              new PixelBlitter(
                filter.ScaleX,
                filter.ScaleY,
                (source, h, v) => new cImage(source) {
                  HorizontalOutOfBoundsMode = h,
                  VerticalOutOfBoundsMode = v
                }.FilterImage(filter.Name).ToBitmap()
              )
            )
          );
      });

      /*
       * This works as following:
       * First we look for command line parameters and if there are any of them present, we run the CLI version.
       * If there are no parameters, we try to find out if we are run inside a console and if so, we spawn a new copy of ourselves without a console.
       * If there is no console at all, we show the GUI.
       * This way we're both a CLI and a GUI.
       */
      if (args != null && args.Length > 0) {

        // execute CLI
        Environment.Exit(CLI.ParseCommandLineArguments(args));

      } else {
        var consoleHandle = _GetConsoleWindow();

        // run GUI
        if (consoleHandle == IntPtr.Zero || AppDomain.CurrentDomain.FriendlyName.Contains(".vshost"))

          // we either have no console window or we're started from within visual studio
          Application.Run(new MainForm());
        else {

          // we found a console attached to us, so restart ourselves without one
          Process.Start(new ProcessStartInfo(Assembly.GetEntryAssembly().Location) {
            CreateNoWindow = true,
            UseShellExecute = false
          });
        }
      }

    }

    /// <summary>
    /// Filters a given bitmap and resizes it.
    /// </summary>
    /// <param name="source">The source image.</param>
    /// <param name="imageResizerToken">The image resizer.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="verticalOutOfBoundsMode">The vertical out of bounds mode.</param>
    /// <param name="horizontalOutOfBoundsMode">The horizontal out of bounds mode.</param>
    /// <returns>
    /// A new bitmap filtered when requested and resized to the given target dimensions.
    /// </returns>
    internal static Bitmap FilterAndResizeImage(Image source, ImageResizerToken imageResizerToken, int width, int height, OutOfBoundsMode verticalOutOfBoundsMode, OutOfBoundsMode horizontalOutOfBoundsMode) {
      var filter = imageResizerToken.Blitter.PixelCalculator;
      if (filter == null)
        return (_ResizeImage(source, width, height, imageResizerToken.InterpolationMode));
      using (var temporaryImage = filter((Bitmap)source, horizontalOutOfBoundsMode, verticalOutOfBoundsMode))
        return (_ResizeImage(temporaryImage, width, height, imageResizerToken.InterpolationMode));
    }

    /// <summary>
    /// Resizes a given image.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="width">The width; 0 means keep original.</param>
    /// <param name="height">The height; 0 means keep original.</param>
    /// <param name="mode">The mode.</param>
    /// <returns>
    /// A new bitmap with the desired dimensions.
    /// </returns>
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
