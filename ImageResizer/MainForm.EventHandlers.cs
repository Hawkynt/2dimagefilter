#region (c)2008-2015 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2015 Hawkynt

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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Classes;
using Classes.ScriptActions;

using ImageResizer.Properties;
using ImageResizer.Windows;

/*
 * This file contains all event handlers for the main form.
 * 
 */

namespace ImageResizer {
  partial class MainForm {
    private void btResize_Click(object _, EventArgs __) {
      this._scriptEngine.RevertToLastSource();
      this._ScaleImageWithCurrentParameters(false);
    }

    private void btSwitch_Click(object sender, EventArgs e) {
      // Detach both PictureBoxes BEFORE running the action — TargetToSourceCommand causes the
      // engine to dispose the old source bitmap (replaced by a clone of the target) and the old
      // target bitmap (set to null). Either disposal would crash a still-attached PictureBox
      // mid-paint via ImageAnimator.CanAnimate(FrameDimensionsList).
      this.iwhSourceImage.Image = null;
      this._TargetImage = null;

      this._scriptEngine.ExecuteAction(new TargetToSourceCommand());
      this._SourceImage = this._scriptEngine.GdiSource;
      this._TargetImage = this._scriptEngine.GdiTarget;
      // No preview: user is deliberately working with the switched pair; an auto-preview
      // would clobber the target pane.
    }

    private void btRepeat_Click(object sender, EventArgs e) {
      this._ScaleImageWithCurrentParameters(true);
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

      var scriptEngine = this._scriptEngine;
      if (this.nudWidth.Value < 1)
        this.nudWidth.Value = scriptEngine.GdiSource.Width;

      if (this.nudHeight.Value < 1)
        this.nudHeight.Value = scriptEngine.GdiSource.Height;
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
      var image = this.iwhTargetImage.Image;
      if (image == null)
        return;

      var fileName = this._lastSaveFileName;
      if (fileName == null) {
        this.saveAsToolStripMenuItem_Click(sender, e);
        return;
      }

      this._scriptEngine.ExecuteAction(new SaveFileCommand(fileName));

      var result = CLI.SaveHelper(fileName, image);
      if (result == CLIExitCode.JpegNotSupportedOnThisPlatform)
        MessageBox.Show(Resources.txNoJpegSupport, Resources.ttNoJpegSupport);
      else if (result == CLIExitCode.NothingToSave)
        MessageBox.Show(Resources.txNothingToSave, Resources.ttNothingToSave);
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

      var scriptEngine = this._scriptEngine;

      if (!(this.nudWidth.Enabled = method != null && method.SupportsWidth))
        this.nudWidth.Value = scriptEngine.GdiTarget == null ? scriptEngine.GdiSource == null ? 0 : scriptEngine.GdiSource.Width : scriptEngine.GdiTarget.Width;

      if (!(this.nudHeight.Enabled = method != null && method.SupportsHeight))
        this.nudHeight.Value = scriptEngine.GdiTarget == null ? scriptEngine.GdiSource == null ? 0 : scriptEngine.GdiSource.Height : scriptEngine.GdiTarget.Height;

      this.chkUseCenteredGrid.Enabled = method != null && method.SupportsGridCentering;
      this.chkUseThresholds.Enabled = method != null && method.SupportsThresholds;

      if (!(this.nudRepetitionCount.Enabled = method != null && method.SupportsRepetitionCount))
        this.nudRepetitionCount.Value = 1;

      this.nudRadius.Enabled = method != null && method.SupportsRadius;

      // Show / hide the PropertyGrid for the selected manipulator's parameter surface.
      // Stash the bag on the form so the apply path can read its values back without
      // re-parsing the grid; null when the manipulator has no tunable parameters.
      var parameters = method?.Parameters;
      if (parameters != null && parameters.Count > 0) {
        var bag = ManipulatorParameterBag.CreateFor(parameters);
        this._currentParameterBag = bag;
        this.pgManipulatorParameters.SelectedObject = bag;
        this.pgManipulatorParameters.Visible = true;
      } else {
        this._currentParameterBag = null;
        this.pgManipulatorParameters.SelectedObject = null;
        this.pgManipulatorParameters.Visible = false;
      }

      this._SchedulePreview();
    }

    private void nudRadius_ValueChanged(object sender, EventArgs e) {
      this._RefreshKernelChart();
      this._SchedulePreview();
    }

    private void stretchToolStripMenuItem_Click(object sender, EventArgs e) {
      this._SourceImageSizeMode = PictureBoxSizeMode.StretchImage;
    }

    private void centerToolStripMenuItem_Click(object sender, EventArgs e) {
      this._SourceImageSizeMode = PictureBoxSizeMode.CenterImage;
    }

    private void zoomToolStripMenuItem_Click(object sender, EventArgs e) {
      this._SourceImageSizeMode = PictureBoxSizeMode.Zoom;
    }

    private void stretchToolStripMenuItem1_Click(object sender, EventArgs e) {
      this._TargetImageSizeMode = PictureBoxSizeMode.StretchImage;
    }

    private void centerToolStripMenuItem1_Click(object sender, EventArgs e) {
      this._TargetImageSizeMode = PictureBoxSizeMode.CenterImage;
    }

    private void zoomToolStripMenuItem1_Click(object sender, EventArgs e) {
      this._TargetImageSizeMode = PictureBoxSizeMode.Zoom;
    }

    private void iwhSourceImage_DragEnter(object sender, DragEventArgs e) {
      if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
        var files = _GetSupportedFiles(e);
        if (files == null || files.Length < 1)
          return;

        e.Effect = DragDropEffects.Copy;
        return;
      }
      if (e.Data.GetDataPresent(DataFormats.Bitmap)) {
        e.Effect = DragDropEffects.Copy;
        return;
      }
      e.Effect = DragDropEffects.None;
    }

    private void iwhSourceImage_DragDrop(object sender, DragEventArgs e) {
      if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
        var files = _GetSupportedFiles(e);
        if (files == null || files.Length < 1)
          return;

        if (_IsSupportedFileExtension(Path.GetExtension(files[0])))
          this._LoadImageFromFileName(files[0]);
        else
          this._ApplyScriptFile(files[0]);
        return;
      }
      if (e.Data.GetDataPresent(DataFormats.Bitmap)) {
        var data = e.Data.GetData(DataFormats.Bitmap) as Image;
        if (data == null)
          return;
        this._SourceImage = data;
        this._lastSaveFileName = null;
        this._SchedulePreview();
        return;
      }
    }

    private void chkKeepAspect_CheckedChanged(object sender, EventArgs e) {
      var value = this.chkKeepAspect.Checked;
      if (value) {
        var sourceImage = this.iwhSourceImage.Image;
        if (sourceImage == null)
          return;

        this._CorrectAspectRatioIfNeeded(false);
      }
    }

    private void nudWidth_ValueChanged(object sender, EventArgs e) {
      this._CorrectAspectRatioIfNeeded(false);
      this._SyncPercentageFromWidth();
      this._SchedulePreview();
    }

    private void nudHeight_ValueChanged(object sender, EventArgs e) {
      this._CorrectAspectRatioIfNeeded(true);
      this._SchedulePreview();
    }

    private void btnScale2x_Click(object sender, EventArgs e) => this._ApplyScaleFactor(2);
    private void btnScale3x_Click(object sender, EventArgs e) => this._ApplyScaleFactor(3);
    private void btnScale4x_Click(object sender, EventArgs e) => this._ApplyScaleFactor(4);
    private void btnScale5x_Click(object sender, EventArgs e) => this._ApplyScaleFactor(5);
    private void btnScale6x_Click(object sender, EventArgs e) => this._ApplyScaleFactor(6);
    private void btnScale10x_Click(object sender, EventArgs e) => this._ApplyScaleFactor(10);

    private void _ApplyScaleFactor(int factor) {
      var source = this._scriptEngine.SourceImage;
      if (source == null) return;

      var maximum = (int)this.nudWidth.Maximum;
      this.nudWidth.Value = TargetDimensions.Scale(source.Width, factor, maximum);
      this.nudHeight.Value = TargetDimensions.Scale(source.Height, factor, maximum);
    }

    private void showToolStripMenuItem_Click(object sender, EventArgs e) {
      MessageBox.Show(ScriptSerializer.SerializeState(this._scriptEngine), "Script", MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    private void clearToolStripMenuItem_Click(object sender, EventArgs e) {
      this._scriptEngine.Clear();
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
      var window = new AboutWindow();
      window.ShowDialog();
    }

    private void wikiToolStripMenuItem_Click(object sender, EventArgs e) {
      Process.Start(Resources.urlWiki);
    }

    private void executeToolStripMenuItem_Click(object sender, EventArgs e) {
      var srcBitmap = this._scriptEngine.GdiSource;
      var srcW = srcBitmap?.Width ?? 0;
      var srcH = srcBitmap?.Height ?? 0;
      // Detach both PictureBoxes before re-running the script — RepeatActions will dispose
      // intermediate engine-owned bitmaps and we mustn't leave the UI referencing them.
      this.iwhSourceImage.Image = null;
      this._TargetImage = null;
      try {
        this._scriptEngine.RepeatActions();
        this._SourceImage = this._scriptEngine.GdiSource;
        this._TargetImage = this._scriptEngine.GdiTarget;
      } catch (Exception ex) {
        var msg = _ClassifyResizeFailure(_Unwrap(ex), srcW, srcH);
        this.iwhTargetImage.StatusText = "Script execute failed — " + msg;
        MessageBox.Show(this, msg, "Script execute failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void saveToolStripMenuItem1_Click(object sender, EventArgs e) {
      var engine = this._scriptEngine;
      if (!engine.Actions.Any()) {
        MessageBox.Show(Resources.txNoScriptToSave, Resources.ttNoScriptToSave, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      var dialog = this.sfdSaveScript;
      if (dialog.ShowDialog() != DialogResult.OK)
        return;

      var filename = dialog.FileName;
      ScriptSerializer.SaveToFile(engine, filename);
    }

    private void loadToolStripMenuItem_Click(object sender, EventArgs e) {
      var dialog = this.ofdOpenScript;
      if (dialog.ShowDialog() != DialogResult.OK)
        return;

      var filename = dialog.FileName;
      this._scriptEngine.Clear();
      ScriptSerializer.LoadFromFile(this._scriptEngine, filename);
    }
  }
}
