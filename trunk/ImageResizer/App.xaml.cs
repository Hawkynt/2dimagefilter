using nImager;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;
using System.Drawing.Imaging;

namespace ImageResizer {
  public struct sPixelBlitter {
    public Func<Bitmap, Bitmap> ptrPixelCalculator;
    public int intScaleX;
    public int intScaleY;
    public sPixelBlitter(int intScaleX, int intScaleY, Func<Bitmap, Bitmap> ptrPixelCalculator) {
      this.intScaleX = intScaleX;
      this.intScaleY = intScaleY;
      this.ptrPixelCalculator = ptrPixelCalculator;
    }
  }

  public struct sImageResizer {
    public string szName;
    public InterpolationMode InterpolationMode;
    public sPixelBlitter structPixelBlitter;
    public sImageResizer(string szName, sPixelBlitter structPixelBlitter, InterpolationMode InterpolationMode) {
      this.szName = szName;
      this.structPixelBlitter = structPixelBlitter;
      this.InterpolationMode = InterpolationMode;
    }
    public sImageResizer(string szName)
      : this(szName, new sPixelBlitter(), InterpolationMode.HighQualityBicubic) {
    }
    public sImageResizer(string szName, sPixelBlitter structPixelBlitter)
      : this(szName, structPixelBlitter, InterpolationMode.HighQualityBicubic) {
    }
    public sImageResizer(string szName, InterpolationMode InterpolationMode)
      : this(szName, new sPixelBlitter(), InterpolationMode) {
    }
    public override string ToString() {
      return this.szName;
    }
  }

  /// <summary>
  /// Interaktionslogik für "App.xaml"
  /// </summary>
  public partial class App
  {
    /// <summary>
    /// A list containing all available resize methods
    /// </summary>
    public List<sImageResizer> arrImageResizers = new List<sImageResizer> {
      new sImageResizer("Pixel", InterpolationMode.NearestNeighbor),
      new sImageResizer("BiLinear", InterpolationMode.HighQualityBilinear),
      new sImageResizer("BiCubic")
    };
    
    /// <summary>
    /// An image loaded through the CLI
    /// </summary>
    public BitmapSource objBitmapSource = null;
  
    public App() {
      // TODO: tests here
      sPixel.AllowThresholds = true;
    }

    private void Application_Startup(object sender, System.Windows.StartupEventArgs e) {
      // add image filters from cImage
      Array.ForEach(cImage.Filters, stFilter => {
        if (!string.IsNullOrEmpty(stFilter.Name)) {
          arrImageResizers.Add(new sImageResizer(stFilter.Name, new sPixelBlitter(stFilter.ScaleX, stFilter.ScaleY,
            objSource => new cImage(objSource).FilterImage(stFilter.Name).ToBitmap())));
        } else {
          // just skip null entries
        }
      });
      
      // parse command line if needed
      if (e != null && e.Args != null && e.Args.Length > 0) {
        string[] arrArgs = e.Args;
        int intI = 0;
        int intLen = arrArgs.Length;
        while (intI < intLen) {
          string strCommand = arrArgs[intI++].ToUpperInvariant();
          switch (strCommand) {
            case "/LOAD": {
              if (intLen - intI >= 1) {
                string strFile = arrArgs[intI++];
                this.objBitmapSource = new BitmapImage(new Uri(strFile));
              } else {
                this.voidShowHelp();
                intI = intLen;
              }
              break;
            }
            case "/RESIZE": {
              if (intLen - intI >= 2) {
                string strDim = arrArgs[intI++].Trim();
                string strFilter = arrArgs[intI++].Trim();
                int intPos = strDim.IndexOf('x');
                if (intPos > 0) {
                  int intX, intY;
                  if (int.TryParse(strDim.Substring(0, intPos), out intX) && int.TryParse(strDim.Substring(intPos + 1), out intY)) {
                    int intRepeat;
                    intPos = strFilter.IndexOf('(');
                    if (intPos > 0 && strFilter.EndsWith(")")) {
                      if (!int.TryParse(strFilter.Substring(intPos + 1, strFilter.Length - intPos - 2), out intRepeat)) {
                        intRepeat = 1;
                      }
                      strFilter = strFilter.Substring(0, intPos);
                      if (intRepeat < 1)
                        intRepeat = 1;
                    } else {
                      intRepeat = 1;
                    }
                    if (this.objBitmapSource != null) {
                      Console.WriteLine(intX + " " + intY + " " + intRepeat + " " + strFilter);
                      sImageResizer[] arrMatches = this.arrImageResizers.Where(stImageResize => string.Compare(stImageResize.szName, strFilter, true) == 0).ToArray();
                      if (arrMatches.Length > 0) {
                        for (int intJ = 0; intJ < intRepeat; intJ++)
                          this.objBitmapSource = objResizeImage(this.objBitmapSource, arrMatches[0], intX, intY);
                      } else {
                        MessageBox.Show("Unable to resize file: there is no filter named '" + strFilter + "'!", "Fatal Error");
                        intI = intLen;
                      }
                    } else {
                      MessageBox.Show("Unable to resize file: there is nothing to resize!", "Fatal Error");
                      intI = intLen;
                    }
                  } else {
                    this.voidShowHelp();
                    intI = intLen;
                  }
                } else {
                  this.voidShowHelp();
                  intI = intLen;
                }
              } else {
                this.voidShowHelp();
                intI = intLen;
              }
              break;
            }
            case "/SAVE": {
              if (intLen - intI >= 1) {
                string strFile = arrArgs[intI++];
                string strExt = Path.GetExtension(strFile).ToUpperInvariant();
                if (this.objBitmapSource != null) {
                  if (strExt == ".JPG" || strExt == ".JPEG") {
                    ImageCodecInfo[] arrCodecs = ImageCodecInfo.GetImageEncoders();
                    if (arrCodecs != null) {
                      arrCodecs = arrCodecs.Where(objInfo => objInfo.MimeType == "image/jpeg").ToArray();
                      if (arrCodecs.Length > 0) {
                        this.objBitmapSource.AsBitmap().Save(
                          strFile,
                          arrCodecs[0],
                          new EncoderParameters {
                            Param = new EncoderParameter[] { 
                              new EncoderParameter(Encoder.Quality,(long)100),
                            }
                          }
                        );
                      } else {
                        MessageBox.Show("System has no support to save as JPEG !", "Fatal Error");
                        intI = intLen;
                      }
                    } else {
                      MessageBox.Show("System seems to support no encoders !", "Fatal Error");
                      intI = intLen;
                    }
                  } else {
                    this.objBitmapSource.AsBitmap().Save(strFile);
                  }
                } else {
                  MessageBox.Show("Unable to save file: there is nothing to save!", "Fatal Error");
                  intI = intLen;
                }
              } else {
                this.voidShowHelp();
                intI = intLen;
              }
              break;
            }
            case "/EXIT": {
              this.Shutdown(0);
              break;
            }
            default: {
              this.voidShowHelp();
              intI = intLen;
              break;
            }
          }
        }
      } else {
        // no command line given
      }
    }

    private void voidShowHelp() {
      System.Reflection.Assembly objAssembly=System.Reflection.Assembly.GetExecutingAssembly();
      MessageBox.Show(string.Join("\r\n", new string[] { 
        Path.GetFileName(objAssembly.Location)+" [/load <source>] [/resize <x>x<y> <method>[(<repeat>)]] [/save <target>] ... [/exit]",
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
        "You can load and process multiple file at once by loading after saving again.",
        "  + ie. /load 1.bmp /resize 10x10 Pixel /save 1.jpg /load 2.bmp /resize 10x10 Pixel /save 2.jpg",
        "You can also save to multiple files by adding another save parameter.",
        "  + ie. /load 1.bmp /resize 10x10 Pixel /save 1.jpg /save 2.jpg",
        "Even preprocessing using multiple filters is possible by adding another resize parameter.",
        "  + ie. /load 1.bmp /resize 10x10 Pixel /resize 0x0 Scale2x /save 1.jpg"
      }), "Command-Line Help");
    }

    public static BitmapSource objResizeImage(BitmapSource objSource, sImageResizer structImageResizer, int intWidth, int intHeight) {
      BitmapSource objRet;

      Bitmap objBitmapSrc = objSource.AsBitmap();
      if (structImageResizer.structPixelBlitter.ptrPixelCalculator != null) {
        Bitmap objBitmapTmp = structImageResizer.structPixelBlitter.ptrPixelCalculator.Invoke(objBitmapSrc);
        objBitmapSrc.Dispose();
        objBitmapSrc = objBitmapTmp;
      } else {
        // no pixel resizer given
      }

      using (Bitmap objBitmapTgt = new Bitmap((intWidth == 0 ? objBitmapSrc.Width : intWidth), (intHeight == 0 ? objBitmapSrc.Height : intHeight))) {
        using (Graphics objGraphics = Graphics.FromImage(objBitmapTgt)) {
          //set the resize quality modes to high quality                
          objGraphics.CompositingQuality = CompositingQuality.HighQuality;
          objGraphics.InterpolationMode = structImageResizer.InterpolationMode;
          objGraphics.SmoothingMode = SmoothingMode.HighQuality;
          //draw the image into the target bitmap                
          objGraphics.DrawImage(objBitmapSrc, 0, 0, objBitmapTgt.Width, objBitmapTgt.Height);
        }
        objBitmapSrc.Dispose();
        objRet = objBitmapTgt.AsBitmapSource();
      }
      return (objRet);
    }


  }
}
