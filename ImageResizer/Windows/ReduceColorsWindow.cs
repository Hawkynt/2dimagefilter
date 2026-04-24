#region (c)2008-2026 Hawkynt
/*
 *  cImage
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt
 */
#endregion

using System;
using System.Drawing;
using System.Windows.Forms;
using Hawkynt.ColorProcessing.Dithering;
using Hawkynt.ColorProcessing.Quantization;
using ImageResizer.UserControls;

namespace ImageResizer.Windows {
  /// <summary>
  /// Modal host for <see cref="ReduceColorsPanel"/>. Returns the user's choice via
  /// <see cref="PickedQuantizer"/>/<see cref="PickedDitherer"/>/<see cref="PaletteSize"/>
  /// when Apply is clicked; caller turns those into a <c>ReduceColorsCommand</c>.
  /// </summary>
  internal sealed class ReduceColorsWindow : Form {
    private readonly ReduceColorsPanel _panel;

    public QuantizerDescriptor PickedQuantizer { get; private set; }
    public DithererDescriptor PickedDitherer { get; private set; }
    public ushort PaletteSize { get; private set; }

    public ReduceColorsWindow(Bitmap source) {
      this.Text = "Reduce Colours";
      // 440 px fixed left sidebar + ≥720 px right-pane for the zoomable detail preview + chrome.
      this.Size = new Size(1400, 820);
      this.MinimumSize = new Size(1100, 640);
      this.StartPosition = FormStartPosition.CenterParent;
      this.MinimizeBox = false;
      this.ShowIcon = false;
      this._panel = new ReduceColorsPanel();
      this._panel.ApplyRequested += this._OnApplyRequested;
      this.Controls.Add(this._panel);
      this._panel.SetSource(source);
    }

    private void _OnApplyRequested(object sender, ReduceColorsPanel.ApplyRequestedArgs e) {
      this.PickedQuantizer = e.Quantizer;
      this.PickedDitherer = e.Ditherer;
      this.PaletteSize = e.PaletteSize;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }
}
