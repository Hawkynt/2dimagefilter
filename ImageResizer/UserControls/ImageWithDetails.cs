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
      get { return (this.pbImage.SizeMode); }
      set {
        this.pbImage.SizeMode = value;
        this._CenterPictureBox();
      }
    }

    [DefaultValue(null)]
    public Image Image {
      get { return (this.pbImage.Image); }
      set {
        this.pbImage.Image = value;
        this._CenterPictureBox();

        if (value == null) {
          this.lDetails.Text = string.Empty;
          return;
        }

        var width = value.Width;
        var height = value.Height;
        this.lDetails.Text = string.Format("{0} x {1}", width, height);
      }
    }
    #endregion

    public ImageWithDetails() {
      InitializeComponent();
    }

    protected void _EventWrapper(object sender, EventArgs args) {
      this.OnClick(args);
    }

    protected new void OnClick(EventArgs e) {
      var handler = this.Click;
      if (handler != null)
        handler(this, e);
    }

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

    private void pnImage_SizeChanged(object sender, EventArgs e) {
      this._CenterPictureBox();
    }

  }
}
