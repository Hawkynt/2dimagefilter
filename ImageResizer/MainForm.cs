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

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Classes;
using Classes.ImageManipulators;
using Classes.ScriptActions;

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
    /// The last used filename for SaveAs.
    /// </summary>
    private string _lastSaveFileName;
    /// <summary>
    /// The used scripting engine.
    /// </summary>
    private readonly ScriptEngine _scriptEngine = new ScriptEngine();
    #endregion

    #region props
    /// <summary>
    /// Gets or sets the source image.
    /// </summary>
    /// <value>
    /// The source image.
    /// </value>
    private Image _SourceImage {
      set {
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
      set {
        this.butRepeat.Enabled =
          this.butSwitch.Enabled =
            this.saveToolStripMenuItem.Enabled =
              this.saveAsToolStripMenuItem.Enabled =
                this.tssBenchmark.Visible =
                  value != null;
        this.iwhTargetImage.Image = value;
      }
    }

    private PictureBoxSizeMode _SourceImageSizeMode {
      get { return (this.iwhSourceImage.SizeMode); }
      set {
        Config.SourceSizeMode = this.iwhSourceImage.SizeMode = value;
        this.stretchToolStripMenuItem.Checked =
          this.centerToolStripMenuItem.Checked =
            this.zoomToolStripMenuItem.Checked = false;

        switch (value) {
          case PictureBoxSizeMode.StretchImage: {
            this.stretchToolStripMenuItem.Checked = true;
            break;
          }
          case PictureBoxSizeMode.CenterImage: {
            this.centerToolStripMenuItem.Checked = true;
            break;
          }
          case PictureBoxSizeMode.Zoom: {
            this.zoomToolStripMenuItem.Checked = true;
            break;
          }
        }
      }
    }

    private PictureBoxSizeMode _TargetImageSizeMode {
      get { return (this.iwhTargetImage.SizeMode); }
      set {
        Config.TargetSizeMode = this.iwhTargetImage.SizeMode = value;
        this.stretchToolStripMenuItem1.Checked =
          this.centerToolStripMenuItem1.Checked =
            this.zoomToolStripMenuItem1.Checked = false;

        switch (value) {
          case PictureBoxSizeMode.StretchImage: {
            this.stretchToolStripMenuItem1.Checked = true;
            break;
          }
          case PictureBoxSizeMode.CenterImage: {
            this.centerToolStripMenuItem1.Checked = true;
            break;
          }
          case PictureBoxSizeMode.Zoom: {
            this.zoomToolStripMenuItem1.Checked = true;
            break;
          }
        }
      }
    }

    #endregion

    #region ctor
    public MainForm(string fileToOpenOnStart = null) {
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

      if (fileToOpenOnStart != null)
        this._LoadImageFromFileName(fileToOpenOnStart);

    }

    #endregion

    /// <summary>
    /// Loads and applies the configuration settings.
    /// </summary>
    private void _LoadConfigurationSettings() {
      if (Config.SourceSizeMode != null)
        this._SourceImageSizeMode = Config.SourceSizeMode.Value;

      if (Config.TargetSizeMode != null)
        this._TargetImageSizeMode = Config.TargetSizeMode.Value;
    }

    /// <summary>
    /// Resizes the given image with the currently set parameters from the GUI.
    /// </summary>
    private void _ScaleImageWithCurrentParameters(bool applyToTarget) {
      var method = (IImageManipulator)this.cmbResizeMethod.SelectedValue;
      var targetWidth = (word)this.nudWidth.Value;
      var targetHeight = (word)this.nudHeight.Value;
      var maintainAspect = this.chkKeepAspect.Checked;
      var useThresholds = this.chkUseThresholds.Checked;
      var useCenteredGrid = this.chkUseCenteredGrid.Checked;
      var repetitionCount = (byte)this.nudRepetitionCount.Value;
      var horizontalBph = (OutOfBoundsMode)this.cmbHorizontalBPH.SelectedItem;
      var verticalBph = (OutOfBoundsMode)this.cmbVerticalBPH.SelectedItem;
      var radius = (float)this.nudRadius.Value;

      if ((targetWidth <= 0 && method.SupportsWidth) || (targetHeight <= 0 && method.SupportsHeight)) {
        MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        return;
      }

      var command = new ResizeCommand(applyToTarget, method, targetWidth, targetHeight, 0, maintainAspect, horizontalBph, verticalBph, repetitionCount, useThresholds, useCenteredGrid, radius);

      this._ExecuteScriptActions(command);
    }

    /// <summary>
    /// Executes the given script actions.
    /// </summary>
    /// <param name="commands">The commands.</param>
    private void _ExecuteScriptActions(params IScriptAction[] commands) {
      Contract.Requires(commands != null);

      // tell the user that we're busy
      this.msMain.Enabled =
        this.tlpMainLayout.Enabled =
          !(this.tssBusy.Visible = true);

      this.Async(() => {
        // filter image
        var stopwatch = new Stopwatch();
        stopwatch.Restart();

        foreach (var command in commands)
          this._scriptEngine.ExecuteAction(command);

        var gdiSource = this._scriptEngine.GdiSource;
        var gdiTarget = this._scriptEngine.GdiTarget;
        stopwatch.Stop();

        this.SafelyInvoke(() => {
          this._SourceImage = gdiSource;
          this._TargetImage = gdiTarget;

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
        var scriptEngine = this._scriptEngine;
        scriptEngine.ExecuteAction(new LoadFileCommand(fileName));
        this._SourceImage = scriptEngine.GdiSource;
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

      var image = this.iwhSourceImage.Image;
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
    /// <param name="source">The source.</param>
    /// <param name="method">The method.</param>
    /// <param name="targetWidth">Width of the target.</param>
    /// <param name="targetHeight">Height of the target.</param>
    /// <param name="horizontalBh">The horizontal bounds handling.</param>
    /// <param name="verticalBh">The vertical bounds handling.</param>
    /// <param name="useThresholds">if set to <c>true</c> [use thresholds].</param>
    /// <param name="useCenteredGrid">if set to <c>true</c> [use centered grid].</param>
    /// <param name="repetitionCount">The repetition count.</param>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    internal static cImage FilterImage(cImage source, IImageManipulator method, ushort targetWidth, ushort targetHeight, OutOfBoundsMode horizontalBh, OutOfBoundsMode verticalBh, bool useThresholds, bool useCenteredGrid, byte repetitionCount, float radius) {
      Contract.Requires(source != null);
      sPixel.AllowThresholds = useThresholds;
      source.HorizontalOutOfBoundsMode = horizontalBh;
      source.VerticalOutOfBoundsMode = verticalBh;

      cImage result = null;
      var scaler = method as AScaler;
      var interpolator = method as Interpolator;
      var planeExtractor = method as PlaneExtractor;
      var resampler = method as Resampler;
      var radiusResampler = method as RadiusResampler;

      if (scaler != null) {
        result = source;
        for (var i = 0; i < repetitionCount; i++)
          result = scaler.Apply(result);
      } else if (interpolator != null)
        if (targetWidth <= 0 || targetHeight <= 0)
          MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        else
          result = interpolator.Apply(source, targetWidth, targetHeight);
      else if (planeExtractor != null)
        result = planeExtractor.Apply(source);
      else if (resampler != null)
        if (targetWidth <= 0 || targetHeight <= 0)
          MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        else
          result = resampler.Apply(source, targetWidth, targetHeight, useCenteredGrid);
      else if (radiusResampler != null)
        if (targetWidth <= 0 || targetHeight <= 0)
          MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        else
          result = radiusResampler.Apply(source, targetWidth, targetHeight, radius, useCenteredGrid);

      return (result);
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

    /// <summary>
    /// Gets all supported file names from a Drag'N'Drop operation.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
    /// <returns>The list of files which could be accepted.</returns>
    private static string[] _GetSupportedFiles(DragEventArgs e) {
      var files = e == null ? null : ((Array)e.Data.GetData(DataFormats.FileDrop)).OfType<string>().ToArray();
      if (files == null || files.Length < 1)
        return (null);
      return (files.Where(f => _IsSupportedFileExtension(Path.GetExtension(f)) || string.Equals(ScriptSerializer.DEFAULT_FILE_EXTENSION, Path.GetExtension(f))).ToArray());
    }

    /// <summary>
    /// Applies the given script file to the source image.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    private void _ApplyScriptFile(string fileName) {
      var localEngine = new ScriptEngine();
      localEngine.AddWithoutExecution(new NullTransformCommand());
      ScriptSerializer.LoadFromFile(localEngine, fileName);
      this._ExecuteScriptActions(localEngine.Actions.ToArray());
    }
  }
}
