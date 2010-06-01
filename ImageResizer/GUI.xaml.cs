using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using nImager;

namespace ImageResizer {

  /// <summary>
  /// Interaktionslogik für Window1.xaml
  /// </summary>
  public partial class Window1 : Window {
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    public struct sPixelBlitter {
      public Func<Bitmap,Bitmap> ptrPixelCalculator;
      public int intScaleX;
      public int intScaleY;
      public sPixelBlitter(int intScaleX, int intScaleY, Func<Bitmap,Bitmap> ptrPixelCalculator) {
        this.intScaleX = intScaleX;
        this.intScaleY = intScaleY;
        this.ptrPixelCalculator = ptrPixelCalculator;
      }
    }

    public struct sImageResizer {
      public string szName;
      public System.Drawing.Drawing2D.InterpolationMode InterpolationMode;
      public sPixelBlitter structPixelBlitter;
      public sImageResizer(string szName, sPixelBlitter structPixelBlitter, System.Drawing.Drawing2D.InterpolationMode InterpolationMode) {
        this.szName = szName;
        this.structPixelBlitter = structPixelBlitter;
        this.InterpolationMode = InterpolationMode;
      }
      public sImageResizer(string szName)
        : this(szName, new sPixelBlitter(), System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic) {
      }
      public sImageResizer(string szName, sPixelBlitter structPixelBlitter)
        : this(szName, structPixelBlitter, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic) {
      }
      public sImageResizer(string szName, System.Drawing.Drawing2D.InterpolationMode InterpolationMode)
        : this(szName, new sPixelBlitter(), InterpolationMode) {
      }
      public override string ToString() {
        return this.szName;
      }
    }

    public Window1() {
      InitializeComponent();
      this.cbThresholds.IsChecked = sPixel.AllowThresholds;
      this.lbMethods.Items.Add(new sImageResizer("Pixel", System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor));
      this.lbMethods.Items.Add(new sImageResizer("BiLinear", System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear));
      this.lbMethods.Items.Add(new sImageResizer("BiCubic"));
      // add image filters from cImage
      Array.ForEach(cImage.Filters, stFilter => {
        if (!string.IsNullOrEmpty(stFilter.Name)) {
          this.lbMethods.Items.Add(new sImageResizer(stFilter.Name, new sPixelBlitter(stFilter.ScaleX, stFilter.ScaleY,
            delegate(Bitmap objSource) {
              Bitmap objRet = null;
              cImage objImage = new cImage(objSource);
              if (objImage != null) {
                objImage = objImage.FilterImage(stFilter.Name);
                objRet = objImage.ToBitmap();
              }
              return (objRet);
            })));
        } else {
          // just skip null entries
        }
      });

      this.lbMethods.SelectedIndex = 0;
    }

    private void voidResize_Click(object sender, RoutedEventArgs e) {
      sPixel.AllowThresholds=(bool)this.cbThresholds.IsChecked ;
      BitmapSource objSourceImage = (BitmapSource)this.imgSource.Source;
      int intX, intY;
      int.TryParse(this.txtWidth.Text.ToString(), out intX);
      int.TryParse(this.txtHeight.Text.ToString(), out intY);
      BitmapSource objTargetImage = this.objResizeImage(objSourceImage, (sImageResizer)this.lbMethods.SelectedValue, intX, intY);
      this.imgTarget.Source = objTargetImage;
      this.imgTarget.Visibility = Visibility.Visible;
      this.lblTgtDim.Content = string.Format("{0} x {1}", objTargetImage.PixelWidth, objTargetImage.PixelHeight);
      this.lblTgtDim.Visibility = Visibility.Visible;
      this.miSave.IsEnabled = true;
    }

    private void voidOpen_Click(object sender, RoutedEventArgs e) {
      System.Windows.Forms.OpenFileDialog objFileDialog = new System.Windows.Forms.OpenFileDialog();
      objFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png";
      objFileDialog.Title = "Select Image to resize";
      objFileDialog.InitialDirectory=Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)+System.IO.Path.DirectorySeparatorChar +"ScaleTest"; 
      if (objFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
        if (objFileDialog.FileName != null) {
          try {
            BitmapImage objBitmapImage = new BitmapImage(new Uri(objFileDialog.FileName));
            this.imgSource.Source = objBitmapImage;
            this.imgSource.Visibility = Visibility.Visible;
            this.lblSrcDim.Visibility = Visibility.Visible;
            this.lblSrcDim.Content = string.Format("{0} x {1}", objBitmapImage.PixelWidth, objBitmapImage.PixelHeight);
            this.btnResize.IsEnabled = true;
          } catch (Exception excE) {
            System.Windows.MessageBox.Show(string.Format("Could not load {0}\r\n\r\n{1}", objFileDialog.FileName, excE.Message));
          }
        } else {
          // file error
        }
      } else {
        // dialog cancelled
      };
    }

    private void voidSave_Click(object sender, RoutedEventArgs e) {
      if (this.miSave.IsEnabled) {
        // TODO: ask for filename
        Bitmap objBitmap = BitmapImage2Bitmap((BitmapSource)this.imgTarget.Source);
        string strPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + System.IO.Path.DirectorySeparatorChar + "TMP.BMP";
        objBitmap.Save(strPath, ImageFormat.Bmp);
        System.Diagnostics.Process.Start(strPath);
      } else {
        // can not save without target image
      }
    }

    private void voidClose_Click(object sender, RoutedEventArgs e) {
      this.lblSrcDim.Visibility = Visibility.Hidden;
      this.imgSource.Visibility = Visibility.Hidden;
      this.lblTgtDim.Visibility = Visibility.Hidden;
      this.imgTarget.Visibility = Visibility.Hidden;
      this.btnResize.IsEnabled = false;
      this.miSave.IsEnabled = false;
    }

    private void voidExit_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }

    private BitmapSource objResizeImage(BitmapSource objSource, sImageResizer structImageResizer, int intWidth, int intHeight) {
      BitmapSource objRet;

      Bitmap objBitmapSrc = BitmapImage2Bitmap(objSource);
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
          objGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
          objGraphics.InterpolationMode = structImageResizer.InterpolationMode;
          objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
          //draw the image into the target bitmap                
          objGraphics.DrawImage(objBitmapSrc, 0, 0, objBitmapTgt.Width, objBitmapTgt.Height);
        }
        objBitmapSrc.Dispose();
        objRet = Bitmap2BitmapImage(objBitmapTgt);
      }
      return (objRet);
    }

    private static Bitmap BitmapImage2Bitmap(BitmapSource objSource) {
      Bitmap objRet;
      using (System.IO.MemoryStream objMemoryStream = new System.IO.MemoryStream()) {
        BmpBitmapEncoder objBmpBitmapEncoder = new BmpBitmapEncoder();
        objBmpBitmapEncoder.Frames.Add(BitmapFrame.Create(objSource));
        objBmpBitmapEncoder.Save(objMemoryStream);
        objRet = new Bitmap(objMemoryStream);
        objRet = new Bitmap(objRet);
      }
      return (objRet);
    }

    private static BitmapSource Bitmap2BitmapImage(Bitmap objSource) {
      BitmapSource objRet;
      IntPtr hndlBitmap = objSource.GetHbitmap();
      objRet = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
        hndlBitmap,
        IntPtr.Zero,
        Int32Rect.Empty,
        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()
        );
      DeleteObject(hndlBitmap);
      return (objRet);
    }
  }
}
