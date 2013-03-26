#region (c)2008-2013 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010-2013 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion
// TODO: script recorder/player
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Classes;
using Classes.ImageManipulators;
using ImageResizer.Properties;
using Imager;
using Imager.Interface;
using word = System.UInt16;

namespace ImageResizer {

  /// <summary>
  /// Our main GUI.
  /// </summary>
  public partial class MainForm : Form {
    #region fields
    /// <summary>
    /// The currently shown source image.
    /// </summary>
    private Image _sourceImage;
    /// <summary>
    /// The currently shown target image.
    /// </summary>
    private Image _targetImage;
    /// <summary>
    /// The last used filename for SaveAs.
    /// </summary>
    private string _lastSaveFileName;
    #endregion

    #region props
    /// <summary>
    /// Gets or sets the source image.
    /// </summary>
    /// <value>
    /// The source image.
    /// </value>
    private Image _SourceImage {
      get {
        return (this._sourceImage);
      }
      set {
        this._sourceImage = value;
        this.gbActions.Enabled =
          this.closeToolStripMenuItem.Enabled =
          value != null;
        this._TargetImage = null;
        this.iwhSourceImage.Image = value;
        this._CorrectAspectRatioIfNeeded(false);
      }
    }

    /// <summary>
    /// Gets or sets the target image.
    /// </summary>
    /// <value>
    /// The target image.
    /// </value>
    private Image _TargetImage {
      get {
        return (this._targetImage);
      }
      set {
        this._targetImage = value;
        this.butRepeat.Enabled =
          this.butSwitch.Enabled =
            this.saveToolStripMenuItem.Enabled =
              this.saveAsToolStripMenuItem.Enabled =
                this.tssBenchmark.Visible =
                  value != null;
        this.iwhTargetImage.Image = value;
      }
    }
    #endregion

    #region ctor
    public MainForm() {
      InitializeComponent();

      //this.cbResizeMethod.DataSource = Program.IMAGE_RESIZERS;
      this.cmbResizeMethod.DataSource = SupportedManipulators.MANIPULATORS;
      this.cmbResizeMethod.DisplayMember = "Key";
      this.cmbResizeMethod.ValueMember = "Value";

      this.cmbResizeMethod.SelectedIndex = 0;

      this.cmbHorizontalBPH.DataSource = Enum.GetValues(typeof(OutOfBoundsMode));
      this.cmbVerticalBPH.DataSource = Enum.GetValues(typeof(OutOfBoundsMode));

      this._SourceImage = null;

      this.sfdSave.InitialDirectory =
        this.ofdOpenFile.InitialDirectory =
          Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

      this.chkUseThresholds.Checked = sPixel.AllowThresholds;

      this._LoadConfigurationSettings();
    }

    #endregion

    #region event handlers
    private void btResize_Click(object _, EventArgs __) {
      this._ScaleImageWithCurrentParameters(this._SourceImage);
    }

    private void btSwitch_Click(object sender, EventArgs e) {
      this._SourceImage = this._TargetImage;
    }

    private void btRepeat_Click(object sender, EventArgs e) {
      this._ScaleImageWithCurrentParameters(this._TargetImage);
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e) {

      // ask for filename
      var fileDialog = this.ofdOpenFile;
      fileDialog.InitialDirectory = string.IsNullOrWhiteSpace(Config.LastLoadDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) : Config.LastLoadDirectory;

      if (fileDialog.ShowDialog() != DialogResult.OK)
        return;

      var fileName = fileDialog.FileName;
      Config.LastLoadDirectory = Path.GetDirectoryName(fileName);

      if (fileName == null)
        return;

      this._LoadImageFromFileName(fileName);
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
      var image = this._TargetImage;
      if (image == null)
        return;

      var fileName = this._lastSaveFileName;
      if (fileName == null) {
        this.saveAsToolStripMenuItem_Click(sender, e);
        return;
      }

      CLI.SaveHelper(fileName, image);
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {

      // ask for filename
      var dialog = this.sfdSave;
      dialog.InitialDirectory = string.IsNullOrWhiteSpace(Config.LastSaveDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) : Config.LastSaveDirectory;

      if (dialog.ShowDialog() != DialogResult.OK)
        return;

      var fileName = dialog.FileName;
      if (fileName == null)
        return;

      // store the name to use later on
      Config.LastSaveDirectory = Path.GetDirectoryName(fileName);
      this._lastSaveFileName = fileName;

      this.saveToolStripMenuItem_Click(sender, e);
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
      this._SourceImage = null;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void iwhSourceImage_Click(object sender, EventArgs e) {
      this.openToolStripMenuItem_Click(sender, e);
    }

    private void iwhTargetImage_Click(object sender, EventArgs e) {
      this.saveToolStripMenuItem_Click(sender, e);

      // start the image with the associated system handler
      var lastSaveFileName = this._lastSaveFileName;
      if (lastSaveFileName != null && File.Exists(lastSaveFileName))
        Process.Start(lastSaveFileName);
    }

    private void cbResizeMethod_SelectedValueChanged(object sender, EventArgs e) {
      var method = this.cmbResizeMethod.SelectedValue as IImageManipulator;

      this.txtDescription.Text = method == null ? null : method.Description;

      this._RefreshKernelChart();

      if (!(this.nudWidth.Enabled = method != null && method.SupportsWidth))
        this.nudWidth.Value = 0;

      if (!(this.nudHeight.Enabled = method != null && method.SupportsHeight))
        this.nudHeight.Value = 0;

      this.chkUseCenteredGrid.Enabled = method != null && method.SupportsGridCentering;
      this.chkUseThresholds.Enabled = method != null && method.SupportsThresholds;

      if (!(this.nudRepetitionCount.Enabled = method != null && method.SupportsRepetitionCount))
        this.nudRepetitionCount.Value = 1;

      this.nudRadius.Enabled = method != null && method.SupportsRadius;
    }

    private void nudRadius_ValueChanged(object sender, EventArgs e) {
      this._RefreshKernelChart();
    }

    private void stretchToolStripMenuItem_Click(object sender, EventArgs e) {
      Config.SourceSizeMode = (this.iwhSourceImage.SizeMode = PictureBoxSizeMode.StretchImage);
    }

    private void centerToolStripMenuItem_Click(object sender, EventArgs e) {
      Config.SourceSizeMode = (this.iwhSourceImage.SizeMode = PictureBoxSizeMode.CenterImage);
    }

    private void zoomToolStripMenuItem_Click(object sender, EventArgs e) {
      Config.SourceSizeMode = (this.iwhSourceImage.SizeMode = PictureBoxSizeMode.Zoom);
    }

    private void stretchToolStripMenuItem1_Click(object sender, EventArgs e) {
      Config.TargetSizeMode = (this.iwhTargetImage.SizeMode = PictureBoxSizeMode.StretchImage);
    }

    private void centerToolStripMenuItem1_Click(object sender, EventArgs e) {
      Config.TargetSizeMode = (this.iwhTargetImage.SizeMode = PictureBoxSizeMode.CenterImage);
    }

    private void zoomToolStripMenuItem1_Click(object sender, EventArgs e) {
      Config.TargetSizeMode = (this.iwhTargetImage.SizeMode = PictureBoxSizeMode.Zoom);
    }

    private void iwhSourceImage_DragEnter(object sender, DragEventArgs e) {
      if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
        var files = (Array)e.Data.GetData(DataFormats.FileDrop);
        if (files != null && files.Length > 0 && _IsSupportedFileExtension(Path.GetExtension((string)files.GetValue(0)))) {
          e.Effect = DragDropEffects.Copy;
          return;
        }
      }
      if (e.Data.GetDataPresent(DataFormats.Bitmap)) {
        e.Effect = DragDropEffects.Copy;
        return;
      }
      e.Effect = DragDropEffects.None;
    }

    private void iwhSourceImage_DragDrop(object sender, DragEventArgs e) {
      if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
        var files = (Array)e.Data.GetData(DataFormats.FileDrop);
        if (files == null || files.Length <= 0 || !_IsSupportedFileExtension(Path.GetExtension((string)files.GetValue(0))))
          return;
        this._LoadImageFromFileName((string)files.GetValue(0));
        return;
      }
      if (e.Data.GetDataPresent(DataFormats.Bitmap)) {
        var data = e.Data.GetData(DataFormats.Bitmap) as Image;
        if (data == null)
          return;
        this._SourceImage = data;
        this._lastSaveFileName = null;
        return;
      }
    }

    private void chkKeepAspect_CheckedChanged(object sender, EventArgs e) {
      var value = this.chkKeepAspect.Checked;
      if (value) {
        var sourceImage = this._SourceImage;
        if (sourceImage == null)
          return;

        this._CorrectAspectRatioIfNeeded(false);
      }
    }

    private void nudWidth_ValueChanged(object sender, EventArgs e) {
      this._CorrectAspectRatioIfNeeded(false);
    }

    private void nudHeight_ValueChanged(object sender, EventArgs e) {
      this._CorrectAspectRatioIfNeeded(true);
    }

    #endregion

    /// <summary>
    /// Loads and applies the configuration settings.
    /// </summary>
    private void _LoadConfigurationSettings() {
      if (Config.SourceSizeMode != null)
        this.iwhSourceImage.SizeMode = Config.SourceSizeMode.Value;

      if (Config.TargetSizeMode != null)
        this.iwhTargetImage.SizeMode = Config.TargetSizeMode.Value;
    }

    /// <summary>
    /// Resizes the given image with the currently set parameters from the GUI.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    private void _ScaleImageWithCurrentParameters(Image sourceImage) {
      var method = (IImageManipulator)this.cmbResizeMethod.SelectedValue;
      var targetWidth = (word)this.nudWidth.Value;
      var targetHeight = (word)this.nudHeight.Value;
      var useThresholds = this.chkUseThresholds.Checked;
      var useCenteredGrid = this.chkUseCenteredGrid.Checked;
      var repetitionCount = (byte)this.nudRepetitionCount.Value;
      var horizontalBph = (OutOfBoundsMode)this.cmbHorizontalBPH.SelectedItem;
      var verticalBph = (OutOfBoundsMode)this.cmbVerticalBPH.SelectedItem;
      var radius = (float)this.nudRadius.Value;

      // tell the user that we're busy
      this.msMain.Enabled =
        this.tlpMainLayout.Enabled =
          !(this.tssBusy.Visible = true);

      this.Async(() => {
        // filter image
        var stopwatch = new Stopwatch();
        stopwatch.Restart();
        var result = FilterImage(sourceImage, method, targetWidth, targetHeight, horizontalBph, verticalBph, useThresholds, useCenteredGrid, repetitionCount, radius);
        stopwatch.Stop();

        this.SafelyInvoke(() => {
          this._TargetImage = result;

          this.tssBenchmark.Text = stopwatch.ElapsedMilliseconds + "ms";
          this.tssBenchmark.Visible = true;

          // let the user know, that we're no longer busy
          this.msMain.Enabled =
            this.tlpMainLayout.Enabled =
              !(this.tssBusy.Visible = false);

          this.Enabled = true;
        });
      });
    }

    /// <summary>
    /// Refreshes the kernel chart if necessary or hides it when not applicable.
    /// </summary>
    private void _RefreshKernelChart() {
      var method = this.cmbResizeMethod.SelectedValue as IImageManipulator;

      var chart = this.chtKernel;
      var dataPointCollection = chart.Series[0].Points;
      dataPointCollection.Clear();
      chart.Visible = false;

      var kernelBasedResampler = method as Resampler;
      var kernelBasedRadiusResampler = method as RadiusResampler;
      if (kernelBasedResampler == null && kernelBasedRadiusResampler == null)
        return;

      var info = kernelBasedRadiusResampler == null ? kernelBasedResampler.GetKernelMethodInfo() : kernelBasedRadiusResampler.GetKernelMethodInfo((float)this.nudRadius.Value);
      for (var x = -info.KernelRadius; x <= info.KernelRadius; x += 0.001f)
        dataPointCollection.AddXY(Math.Round(x, 3), Math.Round(info.Kernel(x), 3));
      chart.ChartAreas[0].AxisX.Minimum = -Math.Round(info.KernelRadius, 1);
      chart.ChartAreas[0].AxisX.Maximum = Math.Round(info.KernelRadius, 1);
      chart.Visible = true;
    }

    /// <summary>
    /// Loads the image from the given filename into the GUI.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    private void _LoadImageFromFileName(string fileName) {
      try {
        this._SourceImage = Image.FromFile(fileName);
        this._lastSaveFileName = null;
      } catch (Exception exception) {
        MessageBox.Show(string.Format(Resources.txCouldNotLoadImage, fileName, exception.Message), Resources.ttCouldNotLoadImage, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    /// <summary>
    /// Corrects target width/height if forced to keep aspect ratio.
    /// </summary>
    /// <param name="useHeight">if set to <c>true</c> we calculate target width from height; otherwise, we calculate target height from width.</param>
    private void _CorrectAspectRatioIfNeeded(bool useHeight) {
      if (!this.chkKeepAspect.Checked)
        return;

      var image = this._SourceImage;
      if (image == null)
        return;

      var width = this.nudWidth.Value;
      var height = this.nudHeight.Value;
      if (useHeight) {
        width = Math.Round(height * image.Width / image.Height);
      } else {
        height = Math.Round(width * image.Height / image.Width);
      }

      if (width != this.nudWidth.Value)
        this.nudWidth.Value = width;

      if (height != this.nudHeight.Value)
        this.nudHeight.Value = height;
    }

    /// <summary>
    /// Filters the image.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    /// <param name="method">The method.</param>
    /// <param name="targetWidth">Width of the target.</param>
    /// <param name="targetHeight">Height of the target.</param>
    /// <param name="horizontalBph">The horizontal BPH.</param>
    /// <param name="verticalBph">The vertical BPH.</param>
    /// <param name="useThresholds">if set to <c>true</c> [use thresholds].</param>
    /// <param name="useCenteredGrid">if set to <c>true</c> [use centered grid].</param>
    /// <param name="repetitionCount">The repetition count.</param>
    /// <returns></returns>
    internal static Bitmap FilterImage(Image sourceImage, IImageManipulator method, ushort targetWidth, ushort targetHeight, OutOfBoundsMode horizontalBph, OutOfBoundsMode verticalBph, bool useThresholds, bool useCenteredGrid, byte repetitionCount, float radius) {
      sPixel.AllowThresholds = useThresholds;
      var source = cImage.FromBitmap((Bitmap)sourceImage);
      source.HorizontalOutOfBoundsMode = horizontalBph;
      source.VerticalOutOfBoundsMode = verticalBph;

      cImage target = null;
      var scaler = method as AScaler;
      var interpolator = method as Interpolator;
      var planeExtractor = method as PlaneExtractor;
      var resampler = method as Resampler;
      var radiusResampler = method as RadiusResampler;

      if (scaler != null) {
        target = source;
        for (var i = 0; i < repetitionCount; i++)
          target = scaler.Apply(target);
      } else if (interpolator != null)
        if (targetWidth <= 0 || targetHeight <= 0)
          MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        else
          target = interpolator.Apply(source, targetWidth, targetHeight);
      else if (planeExtractor != null)
        target = planeExtractor.Apply(source);
      else if (resampler != null)
        if (targetWidth <= 0 || targetHeight <= 0)
          MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        else
          target = resampler.Apply(source, targetWidth, targetHeight, useCenteredGrid);
      else if (radiusResampler != null)
        if (targetWidth <= 0 || targetHeight <= 0)
          MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        else
          target = radiusResampler.Apply(source, targetWidth, targetHeight, radius, useCenteredGrid);

      var result = target == null ? null : target.ToBitmap();
      return result;
    }

    /// <summary>
    /// Determines whether or not the given file extension is usable for the program.
    /// </summary>
    /// <param name="extension">The extension.</param>
    /// <returns><c>true</c> if we accept this file extensions; otherwise, <c>false</c>.</returns>
    private static bool _IsSupportedFileExtension(string extension) {
      if (string.IsNullOrWhiteSpace(extension))
        return (false);
      extension = extension.Trim().ToUpper();
      if (extension == ".JPEG" || extension == ".JPG")
        return (true);
      if (extension == ".BMP")
        return (true);
      if (extension == ".PNG")
        return (true);
      if (extension == ".GIF")
        return (true);
      if (extension == ".TIF" || extension == ".TIFF")
        return (true);
      return (false);
    }

  }
}
