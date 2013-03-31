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

      this._scriptEngine.ExecuteAction(new TargetToSourceCommand());
      this._SourceImage = this._scriptEngine.GdiSource;
      this._TargetImage = this._scriptEngine.GdiTarget;
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
        var sourceImage = this.iwhSourceImage.Image;
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
      Process.Start("https://code.google.com/p/2dimagefilter/w/list");
    }

    private void executeToolStripMenuItem_Click(object sender, EventArgs e) {
      this._scriptEngine.RepeatActions();
      this._SourceImage = this._scriptEngine.GdiSource;
      this._TargetImage = this._scriptEngine.GdiTarget;
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
      ScriptSerializer.LoadFromFile(this._scriptEngine, filename);
    }
  }
}
