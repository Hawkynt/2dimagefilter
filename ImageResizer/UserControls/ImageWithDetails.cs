#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ImageResizer.UserControls {
  /// <summary>
  /// This is just a control with an image and a details pane below it.
  /// </summary>
  [DefaultEvent("Click")]
  public partial class ImageWithDetails : UserControl {

    #region props

    public new event EventHandler Click;

    [DefaultValue(PictureBoxSizeMode.Normal)]
    public PictureBoxSizeMode SizeMode {
      get => this.pbImage.SizeMode;
      set {
        this.pbImage.SizeMode = value;
        this._CenterPictureBox();
      }
    }

    [DefaultValue(null)]
    public Image Image {
      get => this.pbImage.Image;
      set {
        this.pbImage.Image = value;
        this._CenterPictureBox();
        this._dimensionsText = value == null ? string.Empty : string.Format("{0} x {1}", value.Width, value.Height);
        this._RefreshDetails();
      }
    }

    private string _dimensionsText = string.Empty;
    private string _statusText;

    /// <summary>
    /// Optional status note rendered alongside the dimensions label — e.g. <c>"Preview — rendering…"</c>
    /// during an auto-preview. Assign <c>null</c> or empty to clear. Shown in italics.
    /// </summary>
    public string StatusText {
      get => this._statusText;
      set {
        this._statusText = value;
        this._RefreshDetails();
      }
    }

    private void _RefreshDetails() {
      var hasStatus = !string.IsNullOrEmpty(this._statusText);
      if (hasStatus) {
        this.lDetails.Font = new Font(this.lDetails.Font, FontStyle.Italic);
        this.lDetails.Text = string.IsNullOrEmpty(this._dimensionsText)
          ? this._statusText
          : this._dimensionsText + " — " + this._statusText;
      } else {
        this.lDetails.Font = new Font(this.lDetails.Font, FontStyle.Regular);
        this.lDetails.Text = this._dimensionsText;
      }
    }
    #endregion

    public ImageWithDetails() {
      this.InitializeComponent();
    }

    protected void _EventWrapper(object sender, EventArgs args) => this.OnClick(args);
    protected new void OnClick(EventArgs e) => this.Click?.Invoke(this, e);

    private void _CenterPictureBox() {
      var pictureBox = this.pbImage;
      var panel = this.pnImage;

      if (this.SizeMode == PictureBoxSizeMode.AutoSize || this.SizeMode == PictureBoxSizeMode.StretchImage || this.SizeMode == PictureBoxSizeMode.Zoom) {
        pictureBox.Dock = DockStyle.Fill;
        panel.AutoScroll = false;
        return;
      }

      var image = this.Image;
      if (image == null) {
        pictureBox.Dock = DockStyle.Fill;
        panel.AutoScroll = false;
        return;
      }

      pictureBox.Dock = DockStyle.None;
      pictureBox.Width = image.Width;
      pictureBox.Height = image.Height;
      pictureBox.Left = Math.Max(0, (panel.Width - image.Width) / 2);
      pictureBox.Top = Math.Max(0, (panel.Height - image.Height) / 2);

      panel.AutoScroll = image.Width > panel.Width || image.Height > panel.Height;
    }

    private void pnImage_SizeChanged(object sender, EventArgs e) => this._CenterPictureBox();
  }
}
