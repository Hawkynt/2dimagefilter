using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using nImager;

namespace ImageResizer {

  /// <summary>
  /// Interaktionslogik für Window1.xaml
  /// </summary>
  public partial class Window1 {
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

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

    public Window1() {
      InitializeComponent();
      this.cbThresholds.IsChecked = sPixel.AllowThresholds;
      this.lbMethods.Items.Add(new sImageResizer("Pixel", InterpolationMode.NearestNeighbor));
      this.lbMethods.Items.Add(new sImageResizer("BiLinear", InterpolationMode.HighQualityBilinear));
      this.lbMethods.Items.Add(new sImageResizer("BiCubic"));
      // add image filters from cImage
      Array.ForEach(cImage.Filters, stFilter => {
        if (!string.IsNullOrEmpty(stFilter.Name)) {
          this.lbMethods.Items.Add(new sImageResizer(stFilter.Name, new sPixelBlitter(stFilter.ScaleX, stFilter.ScaleY,
            objSource => new cImage(objSource).FilterImage(stFilter.Name).ToBitmap())));
        } else {
          // just skip null entries
        }
      });

      this.lbMethods.SelectedIndex = 0;
      this.btResize.Click += (objSender, objEA) => this._voidResize_Click();
      this.btSwitch.Click += (objSender, objEA) => this._voidSwitch_Click();
      this.btRepeat.Click += (objSender, objEA) => this._voidRepeat_Click();
    }

    private void _voidRepeat_Click() {
      sPixel.AllowThresholds = (bool)this.cbThresholds.IsChecked;
      BitmapSource objSourceImage = (BitmapSource)this.imgTarget.Source;
      int intX, intY;
      int.TryParse(this.txtWidth.Text, out intX);
      int.TryParse(this.txtHeight.Text, out intY);
      BitmapSource objTargetImage = objResizeImage(objSourceImage, (sImageResizer)this.lbMethods.SelectedValue, intX, intY);
      this.imgTarget.Source = objTargetImage;
      this.imgTarget.Visibility = Visibility.Visible;
      this.lblTgtDim.Content = string.Format("{0} x {1}", objTargetImage.PixelWidth, objTargetImage.PixelHeight);
      this.lblTgtDim.Visibility = Visibility.Visible;
      this.miSave.IsEnabled = true;
    }

    private void _voidSwitch_Click() {
      BitmapSource objSourceImage = (BitmapSource)this.imgTarget.Source;
      this.imgSource.Source = objSourceImage;
      this.lblSrcDim.Content = string.Format("{0} x {1}", objSourceImage.PixelWidth, objSourceImage.PixelHeight);
    }

    private void _voidResize_Click() {
      sPixel.AllowThresholds = (bool)this.cbThresholds.IsChecked;
      BitmapSource objSourceImage = (BitmapSource)this.imgSource.Source;
      int intX, intY;
      int.TryParse(this.txtWidth.Text, out intX);
      int.TryParse(this.txtHeight.Text, out intY);
      BitmapSource objTargetImage = objResizeImage(objSourceImage, (sImageResizer)this.lbMethods.SelectedValue, intX, intY);
      this.imgTarget.Source = objTargetImage;
      this.imgTarget.Visibility = Visibility.Visible;
      this.lblTgtDim.Content = string.Format("{0} x {1}", objTargetImage.PixelWidth, objTargetImage.PixelHeight);
      this.lblTgtDim.Visibility = Visibility.Visible;
      this.miSave.IsEnabled = true;
      this.btRepeat.IsEnabled = true;
    }

    private void _voidOpen_Click(object sender, RoutedEventArgs e) {
      System.Windows.Forms.OpenFileDialog objFileDialog = new System.Windows.Forms.OpenFileDialog {
        Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png",
        Title = "Select Image to resize",
        InitialDirectory =
          Environment.GetFolderPath(
            Environment.SpecialFolder.MyPictures) +
          System.IO.Path.DirectorySeparatorChar + "ScaleTest"
      };
      if (objFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;
      if (objFileDialog.FileName == null) {
      } else {
        try {
          BitmapImage objBitmapImage = new BitmapImage(new Uri(objFileDialog.FileName));
          this.imgSource.Source = objBitmapImage;
          this.imgSource.Visibility = Visibility.Visible;
          this.lblSrcDim.Visibility = Visibility.Visible;
          this.lblSrcDim.Content = string.Format("{0} x {1}", objBitmapImage.PixelWidth, objBitmapImage.PixelHeight);
          this.btResize.IsEnabled = true;
          this.btRepeat.IsEnabled = false;
          this.btSwitch.IsEnabled = true;
        } catch (Exception excE) {
          MessageBox.Show(string.Format("Could not load {0}\r\n\r\n{1}", objFileDialog.FileName, excE.Message));
        }
      }
    }

    private void voidSave_Click(object sender, RoutedEventArgs e) {
      if (!this.miSave.IsEnabled) {
        // can not save without target image
      } else {
        // TODO: ask for filename
        Bitmap objBitmap = BitmapImage2Bitmap((BitmapSource)this.imgTarget.Source);
        string strPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +
                         System.IO.Path.DirectorySeparatorChar + "TMP.BMP";
        objBitmap.Save(strPath, ImageFormat.Bmp);
        System.Diagnostics.Process.Start(strPath);
      }
    }

    private void voidClose_Click(object sender, RoutedEventArgs e) {
      this.lblSrcDim.Visibility = Visibility.Hidden;
      this.imgSource.Visibility = Visibility.Hidden;
      this.lblTgtDim.Visibility = Visibility.Hidden;
      this.imgTarget.Visibility = Visibility.Hidden;
      this.btResize.IsEnabled = false;
      this.miSave.IsEnabled = false;
    }

    private void voidExit_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }

    private static BitmapSource objResizeImage(BitmapSource objSource, sImageResizer structImageResizer, int intWidth, int intHeight) {
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
          objGraphics.CompositingQuality = CompositingQuality.HighQuality;
          objGraphics.InterpolationMode = structImageResizer.InterpolationMode;
          objGraphics.SmoothingMode = SmoothingMode.HighQuality;
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
        BitmapSizeOptions.FromEmptyOptions()
        );
      DeleteObject(hndlBitmap);
      return (objRet);
    }
  }
}
