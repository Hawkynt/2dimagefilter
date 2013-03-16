using System;
using System.Drawing;
using System.Windows.Forms;

using ImageResizer.Properties;

using Imager.Classes;

using word = System.UInt16;

using Imager;
using Imager.Interface;

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
        this.btRepeat.Enabled =
          this.btSwitch.Enabled =
            this.saveToolStripMenuItem.Enabled =
              this.saveAsToolStripMenuItem.Enabled =
                value != null;
        this.iwhTargetImage.Image = value;
      }
    }
    #endregion

    #region ctor
    public MainForm() {
      InitializeComponent();

      this.cbResizeMethod.DataSource = Program.IMAGE_RESIZERS;
      this.cbResizeMethod.SelectedIndex = 0;

      this.cbHorizontalBPH.DataSource = Enum.GetValues(typeof(OutOfBoundsMode));
      this.cbVerticalBPH.DataSource = Enum.GetValues(typeof(OutOfBoundsMode));

      this._SourceImage = null;

      this.sfdSave.InitialDirectory =
        this.ofdOpenFile.InitialDirectory =
          Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

      this.cUseThresholds.Checked = sPixel.AllowThresholds;
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

      if (fileDialog.ShowDialog() != DialogResult.OK)
        return;

      var fileName = fileDialog.FileName;

      if (fileName == null)
        return;

      try {
        this._SourceImage = Image.FromFile(fileName);
        this._lastSaveFileName = null;
      } catch (Exception exception) {
        MessageBox.Show(string.Format(Resources.txCouldNotLoadImage, fileName, exception.Message), Resources.ttCouldNotLoadImage, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
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
      if (dialog.ShowDialog() != DialogResult.OK)
        return;

      var fileName = dialog.FileName;
      if (fileName == null)
        return;

      // store the name to use later on
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
    }
    #endregion

    /// <summary>
    /// Resizes the given image with the currently set parameters from the GUI.
    /// </summary>
    /// <param name="sourceImage">The source image.</param>
    private void _ScaleImageWithCurrentParameters(Image sourceImage) {
      var method = (ImageResizerToken)this.cbResizeMethod.SelectedItem;
      var targetWidth = (word)this.nudWidth.Value;
      var targetHeight = (word)this.nudHeight.Value;
      var useThresholds = this.cUseThresholds.Checked;
      var horizontalBph = (OutOfBoundsMode)this.cbHorizontalBPH.SelectedItem;
      var verticalBph = (OutOfBoundsMode)this.cbVerticalBPH.SelectedItem;

      sPixel.AllowThresholds = useThresholds;

      // tell the user that we're busy
      this.msMain.Enabled =
        this.tlpMainLayout.Enabled =
          !(this.tssBusy.Visible = true);

      this.Async(() => {
        var result = Program.FilterAndResizeImage(sourceImage, method, targetWidth, targetHeight, horizontalBph, verticalBph);
        this.SafelyInvoke(() => {
          this._TargetImage = result;

          // let the user know, that we're no longer busy
          this.msMain.Enabled =
            this.tlpMainLayout.Enabled =
              !(this.tssBusy.Visible = false);

          this.Enabled = true;
        });
      });
    }

  }
}
