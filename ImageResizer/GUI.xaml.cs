using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;
using Imager;

namespace ImageResizer {

  /// <summary>
  /// Interaktionslogik für Window1.xaml
  /// </summary>
  public partial class Window1 {

    private Bitmap _TargetImage {
      set {
        Contract.Requires(value != null);
        var targetImage = value.AsBitmapSource();
        this.imgTarget.Source = targetImage;
        this.imgTarget.Visibility = Visibility.Visible;
        this.lblTgtDim.Content = string.Format("{0} x {1}", targetImage.PixelWidth, targetImage.PixelHeight);
        this.lblTgtDim.Visibility = Visibility.Visible;
        this.miSave.IsEnabled = true;
      }
    }

    public Window1() {
      InitializeComponent();
      this.cbThresholds.IsChecked = sPixel.AllowThresholds;
      this.lbMethods.Items.Clear();
      var app = ((App)Application.Current);
      this.lbMethods.ItemsSource = app.ImageResizers;
      this.lbMethods.SelectedIndex = 0;
      this.btResize.Click += (_, __) => this._voidResize_Click();
      this.btSwitch.Click += (_, __) => this._voidSwitch_Click();
      this.btRepeat.Click += (_, __) => this._voidRepeat_Click();

      var bitmap = app.CurrentImage;
      if (bitmap != null)
        this.imgSource.Source = bitmap.AsBitmapSource();
    }

    private void _voidRepeat_Click() {
      sPixel.AllowThresholds = this.cbThresholds.IsChecked.GetValueOrDefault();
      var sourceImage = (BitmapSource)this.imgTarget.Source;
      int intX, intY;
      int.TryParse(this.txtWidth.Text, out intX);
      int.TryParse(this.txtHeight.Text, out intY);

      using (var srcBitmap = sourceImage.AsBitmap())
      using (var tgtBitmap = App.FilterAndResizeImage(srcBitmap, (ImageResizerToken)this.lbMethods.SelectedValue, intX, intY))
        this._TargetImage = tgtBitmap;
    }

    private void _voidSwitch_Click() {
      var targetImage = (BitmapSource)this.imgTarget.Source;
      this.imgSource.Source = targetImage;
      this.lblSrcDim.Content = string.Format("{0} x {1}", targetImage.PixelWidth, targetImage.PixelHeight);
    }

    private void _voidResize_Click() {
      sPixel.AllowThresholds = this.cbThresholds.IsChecked.GetValueOrDefault();
      var sourceImage = (BitmapSource)this.imgSource.Source;
      int intX, intY;
      int.TryParse(this.txtWidth.Text, out intX);
      int.TryParse(this.txtHeight.Text, out intY);
      using (var srcBitmap = sourceImage.AsBitmap())
      using (var tgtBitmap = App.FilterAndResizeImage(srcBitmap, (ImageResizerToken)this.lbMethods.SelectedValue, intX, intY))
        this._TargetImage = tgtBitmap;
      this.btRepeat.IsEnabled = true;
    }

    private void _voidOpen_Click(object _, RoutedEventArgs __) {
      var fileDialog = new System.Windows.Forms.OpenFileDialog {
        Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png",
        Title = "Select Image to resize",
        InitialDirectory =
          Environment.GetFolderPath(
            Environment.SpecialFolder.MyPictures) +
          System.IO.Path.DirectorySeparatorChar + "ScaleTest"
      };

      if (fileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;
      if (fileDialog.FileName == null)
        return;

      try {
        var bitmapImage = new BitmapImage(new Uri(fileDialog.FileName));
        this.imgSource.Source = bitmapImage;
        this.imgSource.Visibility = Visibility.Visible;
        this.lblSrcDim.Visibility = Visibility.Visible;
        this.lblSrcDim.Content = string.Format("{0} x {1}", bitmapImage.PixelWidth, bitmapImage.PixelHeight);
        this.btResize.IsEnabled = true;
        this.btRepeat.IsEnabled = false;
        this.btSwitch.IsEnabled = true;
      } catch (Exception exception) {
        MessageBox.Show(string.Format("Could not load {0}\r\n\r\n{1}", fileDialog.FileName, exception.Message));
      }
    }

    private void voidSave_Click(object _, RoutedEventArgs __) {
      // can not save without target image
      if (!this.miSave.IsEnabled)
        return;

      // TODO: ask for filename
      var filename = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + System.IO.Path.DirectorySeparatorChar + "TMP.BMP";
      using (var bitmap = ((BitmapSource)this.imgTarget.Source).AsBitmap())
        bitmap.Save(filename, ImageFormat.Bmp);

      System.Diagnostics.Process.Start(filename);
    }

    private void voidClose_Click(object _, RoutedEventArgs __) {
      this.lblSrcDim.Visibility = Visibility.Hidden;
      this.imgSource.Visibility = Visibility.Hidden;
      this.lblTgtDim.Visibility = Visibility.Hidden;
      this.imgTarget.Visibility = Visibility.Hidden;
      this.btResize.IsEnabled = false;
      this.miSave.IsEnabled = false;
    }

    private void voidExit_Click(object _, RoutedEventArgs __) {
      this.Close();
    }
  }
}
