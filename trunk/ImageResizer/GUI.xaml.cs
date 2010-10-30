using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;
using nImager;

namespace ImageResizer {

  /// <summary>
  /// Interaktionslogik für Window1.xaml
  /// </summary>
  public partial class Window1 {
    public Window1() {
      InitializeComponent();
      this.cbThresholds.IsChecked = sPixel.AllowThresholds;
      this.lbMethods.Items.Clear();
      this.lbMethods.ItemsSource = ((App)Application.Current).arrImageResizers;
      this.lbMethods.SelectedIndex = 0;
      this.btResize.Click += (objSender, objEA) => this._voidResize_Click();
      this.btSwitch.Click += (objSender, objEA) => this._voidSwitch_Click();
      this.btRepeat.Click += (objSender, objEA) => this._voidRepeat_Click();

      if (((App)Application.Current).objBitmapSource != null)
        this.imgSource.Source = ((App)Application.Current).objBitmapSource;
    }

    private void _voidRepeat_Click() {
      sPixel.AllowThresholds = (bool)this.cbThresholds.IsChecked;
      BitmapSource objSourceImage = (BitmapSource)this.imgTarget.Source;
      int intX, intY;
      int.TryParse(this.txtWidth.Text, out intX);
      int.TryParse(this.txtHeight.Text, out intY);
      BitmapSource objTargetImage = App.objResizeImage(objSourceImage, (sImageResizer)this.lbMethods.SelectedValue, intX, intY);
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
      BitmapSource objTargetImage = App.objResizeImage(objSourceImage, (sImageResizer)this.lbMethods.SelectedValue, intX, intY);
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
        Bitmap objBitmap = ((BitmapSource)this.imgTarget.Source).AsBitmap();
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
  }
}
