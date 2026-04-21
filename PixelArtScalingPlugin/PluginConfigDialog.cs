#region (c)2008-2026 Hawkynt
/*
 *  cImage
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Hawkynt.ColorProcessing.Resizing;

using PaintDotNet;
using PaintDotNet.Effects;

using Imager;
using Imager.Interface;

namespace PixelArtScaling {
  internal sealed class PluginConfigDialog : EffectConfigDialog {

    private const int _PREVIEW_DEBOUNCE_MS = 200;
    private const float _MAX_ZOOM = 64f;  // display pixels per filtered pixel

    // --- algorithm + quick filter ---
    private ComboBox _algorithmCombo;
    private TextBox _algorithmFilter;

    // --- scale mode ---
    private GroupBox _scaleModeHost;
    private TableLayoutPanel _scaleModeStack;
    // Free-dimension controls (Resampler / local cImage-based):
    private RadioButton _modeRadioPercent, _modeRadioFactor, _modeRadioSize;
    private GroupBox _percentBox, _factorBox, _sizeBox;
    private NumericUpDown _percentX, _percentY, _factorX, _factorY, _targetWidth, _targetHeight;
    private TrackBar _widthSlider, _heightSlider;
    private CheckBox _lockAr;
    // Variant-scale controls (ScaleVariantEntry only):
    private Panel _variantPanel;
    private ComboBox _variantCombo;

    // --- OOB + color ---
    private ComboBox _oobXCombo, _oobYCombo;
    private Button _oobColorButton;

    // --- copy + preview + property grid ---
    private Button _copyButton;
    private PreviewCanvas _previewCanvas;
    private Label _previewLabel, _algorithmInfo, _scaleInfo;
    private PropertyGrid _propertyGrid;
    private Button _okButton, _cancelButton;

    // --- plumbing ---
    private System.Windows.Forms.Timer _debounceTimer;
    private CancellationTokenSource _renderCts;
    private readonly SemaphoreSlim _renderGate = new SemaphoreSlim(1, 1);
    private bool _suppressEvents;
    private bool _initialPreviewScheduled;
    private Color _oobColor = Color.Black;

    // --- cached filtered image + zoom/pan viewport ---
    private Bitmap _cachedFiltered;      // owned by dialog; shown by _previewCanvas
    private float _viewZoom = 1f;        // display pixels per filtered pixel
    private float _viewCenterX;          // filtered-image coord at canvas centre
    private float _viewCenterY;
    private bool _panning;
    private Point _panAnchor;            // mouse pos at pan start
    private PointF _panCentreAtStart;    // _viewCenter* at pan start

    public PluginConfigDialog() {
      this.Text = "2D Image Filter";
      this.MinimumSize = new Size(1200, 820);
      this.Size = new Size(1360, 880);
      this.StartPosition = FormStartPosition.CenterParent;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.MaximizeBox = true;
      this.MinimizeBox = false;
      this.AutoScaleMode = AutoScaleMode.Font;
      this._BuildLayout();
      this.Shown += (s, e) => {
        if (!this._initialPreviewScheduled) {
          this._initialPreviewScheduled = true;
          this._SchedulePreview();
        }
      };
    }

    #region EffectConfigDialog hooks

    protected override void InitialInitToken() => this.theEffectToken = new PluginConfigToken {
      FilterName = SupportedManipulators.Manipulators[0].Name,
    };

    protected override void InitDialogFromToken(EffectConfigToken effectToken) {
      var token = (PluginConfigToken)effectToken;
      this._suppressEvents = true;
      try {
        if (!string.IsNullOrEmpty(token.FilterName)) {
          var idx = this._algorithmCombo.Items.IndexOf(token.FilterName);
          if (idx >= 0) this._algorithmCombo.SelectedIndex = idx;
        }
        switch (token.Mode) {
          case ScaleMode.Factor: this._modeRadioFactor.Checked = true; break;
          case ScaleMode.Size:   this._modeRadioSize.Checked = true; break;
          default:               this._modeRadioPercent.Checked = true; break;
        }
        this._percentX.Value = _Clamp(this._percentX, token.PercentX);
        this._percentY.Value = _Clamp(this._percentY, token.PercentY);
        this._factorX.Value = _Clamp(this._factorX, (decimal)token.FactorX);
        this._factorY.Value = _Clamp(this._factorY, (decimal)token.FactorY);
        this._targetWidth.Value = _Clamp(this._targetWidth, token.TargetWidth);
        this._targetHeight.Value = _Clamp(this._targetHeight, token.TargetHeight);
        this._widthSlider.Value = Math.Min(this._widthSlider.Maximum, Math.Max(this._widthSlider.Minimum, token.TargetWidth));
        this._heightSlider.Value = Math.Min(this._heightSlider.Maximum, Math.Max(this._heightSlider.Minimum, token.TargetHeight));
        this._lockAr.Checked = token.LockAspectRatio;
        this._oobXCombo.SelectedItem = token.HorizontalOobMode;
        this._oobYCombo.SelectedItem = token.VerticalOobMode;
        this._oobColor = token.OobColor == Color.Empty ? Color.Black : token.OobColor;
      } finally {
        this._suppressEvents = false;
      }
      this._PopulateVariantCombo();
      this._UpdateModeVisibility();
      this._UpdateAlgorithmInfo();
      this._UpdateOobColorButton();
      if (this._initialPreviewScheduled) this._SchedulePreview();
    }

    private static decimal _Clamp(NumericUpDown n, decimal v) => Math.Max(n.Minimum, Math.Min(n.Maximum, v));

    protected override void InitTokenFromDialog() {
      var token = (PluginConfigToken)this.theEffectToken;
      token.FilterName = this._algorithmCombo.SelectedItem as string ?? token.FilterName;
      token.Mode = this._CurrentMode();
      token.PercentX = (int)this._percentX.Value;
      token.PercentY = (int)this._percentY.Value;
      token.FactorX = (double)this._factorX.Value;
      token.FactorY = (double)this._factorY.Value;
      token.TargetWidth = (int)this._targetWidth.Value;
      token.TargetHeight = (int)this._targetHeight.Value;
      token.LockAspectRatio = this._lockAr.Checked;
      token.HorizontalOobMode = (OutOfBoundsMode)(this._oobXCombo.SelectedItem ?? OutOfBoundsMode.ConstantExtension);
      token.VerticalOobMode = (OutOfBoundsMode)(this._oobYCombo.SelectedItem ?? OutOfBoundsMode.ConstantExtension);
      token.OobColor = this._oobColor;
    }

    private ScaleMode _CurrentMode() {
      if (this._modeRadioFactor.Checked) return ScaleMode.Factor;
      if (this._modeRadioSize.Checked) return ScaleMode.Size;
      return ScaleMode.Percent;
    }

    #endregion

    #region layout

    private void _BuildLayout() {
      var root = new TableLayoutPanel {
        Dock = DockStyle.Fill,
        ColumnCount = 1,
        RowCount = 3,
        Padding = new Padding(10),
      };
      root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this.Controls.Add(root);

      root.Controls.Add(this._BuildControlsRow(), 0, 0);
      root.Controls.Add(this._BuildPreviewSplit(), 0, 1);
      root.Controls.Add(this._BuildButtonBar(), 0, 2);

      this._debounceTimer = new System.Windows.Forms.Timer { Interval = _PREVIEW_DEBOUNCE_MS };
      this._debounceTimer.Tick += this._OnDebounceTick;

      this._algorithmCombo.BeginUpdate();
      foreach (var entry in SupportedManipulators.Manipulators)
        this._algorithmCombo.Items.Add(entry.Name);
      this._algorithmCombo.EndUpdate();
      if (this._algorithmCombo.Items.Count > 0) this._algorithmCombo.SelectedIndex = 0;

      foreach (var oob in (OutOfBoundsMode[])Enum.GetValues(typeof(OutOfBoundsMode))) {
        this._oobXCombo.Items.Add(oob);
        this._oobYCombo.Items.Add(oob);
      }
      this._oobXCombo.SelectedItem = OutOfBoundsMode.ConstantExtension;
      this._oobYCombo.SelectedItem = OutOfBoundsMode.ConstantExtension;

      this._modeRadioPercent.Checked = true;
      this._PopulateVariantCombo();
      this._UpdateModeVisibility();
      this._UpdateOobColorButton();
    }

    private Control _BuildControlsRow() {
      var panel = new TableLayoutPanel {
        Dock = DockStyle.Top,
        ColumnCount = 3,
        RowCount = 1,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
      };
      panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
      panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
      panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
      panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

      panel.Controls.Add(this._BuildAlgorithmAndOobColumn(), 0, 0);
      panel.Controls.Add(this._BuildScaleModeColumn(), 1, 0);
      panel.Controls.Add(this._BuildInfoColumn(), 2, 0);
      return panel;
    }

    private Control _BuildAlgorithmAndOobColumn() {
      var table = new TableLayoutPanel {
        Dock = DockStyle.Top,
        ColumnCount = 2,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        Margin = new Padding(0, 0, 10, 0),
      };
      table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
      table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

      _AddLabelledField(table, 0, "Algorithm:", out this._algorithmCombo, this._OnAlgorithmChanged);

      table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      table.Controls.Add(_Label("Quick filter:"), 0, 1);
      this._algorithmFilter = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 2, 0, 2) };
      this._algorithmFilter.TextChanged += this._OnFilterTextChanged;
      table.Controls.Add(this._algorithmFilter, 1, 1);

      _AddLabelledField(table, 2, "OOB X:", out this._oobXCombo, this._OnOobChanged);
      _AddLabelledField(table, 3, "OOB Y:", out this._oobYCombo, this._OnOobChanged);

      table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      table.Controls.Add(_Label("OOB colour:"), 0, 4);
      this._oobColorButton = new Button {
        Dock = DockStyle.Fill,
        Height = 28,
        AutoSize = false,
        Text = string.Empty,
        Margin = new Padding(0, 2, 0, 2),
        FlatStyle = FlatStyle.Flat,
        UseVisualStyleBackColor = false,
      };
      this._oobColorButton.FlatAppearance.BorderColor = SystemColors.ControlDarkDark;
      this._oobColorButton.FlatAppearance.BorderSize = 1;
      this._oobColorButton.Click += this._OnPickOobColour;
      table.Controls.Add(this._oobColorButton, 1, 4);

      table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._copyButton = new Button {
        Text = "  Copy preview to clipboard",
        Image = Resources.Clipboard,
        ImageAlign = ContentAlignment.MiddleLeft,
        TextAlign = ContentAlignment.MiddleRight,
        Padding = new Padding(4, 2, 8, 2),
        AutoSize = true,
        FlatStyle = FlatStyle.System,
        Margin = new Padding(0, 6, 0, 0),
        MinimumSize = new Size(0, 32),
      };
      this._copyButton.Click += this._OnCopyClicked;
      table.Controls.Add(this._copyButton, 1, 5);

      table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      table.SetColumnSpan(_AddInfoBanner(table), 2);
      return table;
    }

    private static Label _AddInfoBanner(TableLayoutPanel host) {
      var banner = new Label {
        Text =
          "Note: Paint.NET effects cannot resize the canvas.\r\n" +
          "To keep a scaled result at its new dimensions:\r\n" +
          "  1. Click \"Copy preview to clipboard\" above.\r\n" +
          "  2. Cancel this dialog (OK would squash the result back onto the current canvas).\r\n" +
          "  3. Edit → Paste into New Image  (Ctrl+Shift+V).",
        AutoSize = true,
        MaximumSize = new Size(420, 0),
        ForeColor = SystemColors.ControlText,
        BackColor = Color.FromArgb(255, 252, 220),
        BorderStyle = BorderStyle.FixedSingle,
        Padding = new Padding(6),
        Margin = new Padding(0, 8, 0, 0),
      };
      host.Controls.Add(banner, 0, host.RowStyles.Count - 1);
      return banner;
    }

    private static void _AddLabelledField(TableLayoutPanel host, int row, string label, out ComboBox combo, EventHandler changed) {
      host.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      host.Controls.Add(_Label(label), 0, row);
      combo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.System, MaxDropDownItems = 20, Margin = new Padding(0, 2, 0, 2) };
      combo.SelectedIndexChanged += changed;
      host.Controls.Add(combo, 1, row);
    }

    private Control _BuildScaleModeColumn() {
      this._scaleModeHost = new GroupBox {
        Text = "Scale mode",
        Dock = DockStyle.Top,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        Padding = new Padding(8),
        Margin = new Padding(0, 0, 10, 0),
      };

      // Single TableLayoutPanel hosting all scale-mode controls. Rows are AutoSize so content fits.
      this._scaleModeStack = new TableLayoutPanel {
        Dock = DockStyle.Top,
        ColumnCount = 1,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
      };
      this._scaleModeHost.Controls.Add(this._scaleModeStack);

      // VARIANT COMBO (shown only for ScaleVariantEntry) ---------------------
      this._variantPanel = new Panel {
        Dock = DockStyle.Top,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        Margin = new Padding(0, 0, 0, 4),
      };
      var variantTable = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
      variantTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
      variantTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
      variantTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      variantTable.Controls.Add(_Label("Supported scale:"), 0, 0);
      this._variantCombo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.System, Margin = new Padding(0, 2, 0, 2) };
      this._variantCombo.SelectedIndexChanged += (s, e) => { if (!this._suppressEvents) this._SchedulePreview(); };
      variantTable.Controls.Add(this._variantCombo, 1, 0);
      this._variantPanel.Controls.Add(variantTable);
      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._variantPanel);
      this._variantPanel.Visible = false;

      // FREE-DIMENSION CONTROLS ---------------------------------------------
      this._modeRadioPercent = _ModeRadio("Percent");
      this._modeRadioPercent.CheckedChanged += this._OnModeChanged;
      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._modeRadioPercent);

      this._percentBox = _InnerBox();
      this._percentX = _Numeric(0, 1, 2000, 100, 10);
      this._percentY = _Numeric(0, 1, 2000, 100, 10);
      this._percentBox.Controls.Add(_XYRow("X:", this._percentX, "%", "Y:", this._percentY, "%"));
      this._percentX.ValueChanged += (s, e) => this._OnAxisValueChanged(s, true, ScaleMode.Percent);
      this._percentY.ValueChanged += (s, e) => this._OnAxisValueChanged(s, false, ScaleMode.Percent);
      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._percentBox);

      this._modeRadioFactor = _ModeRadio("Factor");
      this._modeRadioFactor.CheckedChanged += this._OnModeChanged;
      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._modeRadioFactor);

      this._factorBox = _InnerBox();
      this._factorX = _Numeric(2, 0.01m, 20m, 1, 0.1m);
      this._factorY = _Numeric(2, 0.01m, 20m, 1, 0.1m);
      this._factorBox.Controls.Add(_XYRow("X:", this._factorX, "×", "Y:", this._factorY, "×"));
      this._factorX.ValueChanged += (s, e) => this._OnAxisValueChanged(s, true, ScaleMode.Factor);
      this._factorY.ValueChanged += (s, e) => this._OnAxisValueChanged(s, false, ScaleMode.Factor);
      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._factorBox);

      this._modeRadioSize = _ModeRadio("Absolute (pixels)");
      this._modeRadioSize.CheckedChanged += this._OnModeChanged;
      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._modeRadioSize);

      // Size inner: TableLayoutPanel so Dock=Fill sliders actually stretch (FlowLayoutPanel would ignore Dock).
      this._sizeBox = _InnerBox();
      var sizeInner = new TableLayoutPanel {
        Dock = DockStyle.Fill,
        ColumnCount = 1,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
      };
      sizeInner.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      sizeInner.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      sizeInner.RowStyles.Add(new RowStyle(SizeType.AutoSize));

      this._targetWidth = _Numeric(0, 0, 16384, 0, 8);
      this._targetHeight = _Numeric(0, 0, 16384, 0, 8);
      var xyRow = _XYRow("W:", this._targetWidth, "px", "H:", this._targetHeight, "px");
      xyRow.Dock = DockStyle.Fill;
      sizeInner.Controls.Add(xyRow, 0, 0);

      this._widthSlider = _Slider();
      this._heightSlider = _Slider();
      this._widthSlider.ValueChanged += this._OnWidthSliderChanged;
      this._heightSlider.ValueChanged += this._OnHeightSliderChanged;
      this._targetWidth.ValueChanged += (s, e) => this._OnAxisValueChanged(s, true, ScaleMode.Size);
      this._targetHeight.ValueChanged += (s, e) => this._OnAxisValueChanged(s, false, ScaleMode.Size);
      sizeInner.Controls.Add(this._widthSlider, 0, 1);
      sizeInner.Controls.Add(this._heightSlider, 0, 2);
      this._sizeBox.Controls.Add(sizeInner);

      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._sizeBox);

      // AR LOCK
      this._lockAr = new CheckBox {
        Text = "Lock aspect ratio",
        AutoSize = true,
        Checked = true,
        Margin = new Padding(4, 8, 0, 0),
      };
      this._lockAr.CheckedChanged += this._OnLockArChanged;
      this._scaleModeStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._scaleModeStack.Controls.Add(this._lockAr);

      return this._scaleModeHost;
    }

    private static RadioButton _ModeRadio(string text) => new RadioButton {
      Text = text,
      AutoSize = true,
      Margin = new Padding(0, 2, 0, 2),
    };

    private static GroupBox _InnerBox() => new GroupBox {
      Text = string.Empty,
      Dock = DockStyle.Top,
      AutoSize = true,
      AutoSizeMode = AutoSizeMode.GrowAndShrink,
      Padding = new Padding(6, 4, 6, 6),
      Margin = new Padding(18, 0, 0, 4),
    };

    private static TrackBar _Slider() => new TrackBar {
      Dock = DockStyle.Fill,
      Minimum = 0,
      Maximum = 16384,
      TickStyle = TickStyle.None,
      LargeChange = 64,
      SmallChange = 8,
      AutoSize = false,
      Height = 36,
      Margin = new Padding(0, 2, 0, 0),
    };

    private Control _BuildInfoColumn() {
      var host = new TableLayoutPanel {
        Dock = DockStyle.Fill,
        ColumnCount = 1,
        RowCount = 3,
        AutoSize = false,
      };
      host.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      host.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      host.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

      this._scaleInfo = new Label { Dock = DockStyle.Fill, AutoSize = false, Height = 22, ForeColor = SystemColors.GrayText, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 2, 0, 2) };
      this._algorithmInfo = new Label { Dock = DockStyle.Fill, AutoSize = false, Height = 70, Padding = new Padding(6), BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.TopLeft, Margin = new Padding(0, 2, 0, 6) };
      this._propertyGrid = new PropertyGrid {
        Dock = DockStyle.Fill,
        HelpVisible = false,
        ToolbarVisible = false,
        PropertySort = PropertySort.Alphabetical,
      };

      host.Controls.Add(this._scaleInfo, 0, 0);
      host.Controls.Add(this._algorithmInfo, 0, 1);
      host.Controls.Add(this._propertyGrid, 0, 2);
      return host;
    }

    private static Label _Label(string text) => new Label {
      Text = text,
      Dock = DockStyle.Fill,
      TextAlign = ContentAlignment.MiddleLeft,
      AutoSize = false,
      Height = 26,
      Margin = new Padding(0, 4, 6, 4),
    };

    private static NumericUpDown _Numeric(int decimals, decimal min, decimal max, decimal def, decimal increment) => new NumericUpDown {
      DecimalPlaces = decimals,
      Minimum = min,
      Maximum = max,
      Value = def,
      Increment = increment,
      ThousandsSeparator = true,
      Width = 86,
      Margin = new Padding(0, 2, 4, 2),
    };

    private static Control _XYRow(string labelX, NumericUpDown nx, string suffixX, string labelY, NumericUpDown ny, string suffixY) {
      var row = new FlowLayoutPanel {
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
      };
      row.Controls.Add(new Label { Text = labelX, AutoSize = true, Margin = new Padding(0, 6, 2, 0) });
      row.Controls.Add(nx);
      row.Controls.Add(new Label { Text = suffixX, AutoSize = true, Margin = new Padding(2, 6, 14, 0) });
      row.Controls.Add(new Label { Text = labelY, AutoSize = true, Margin = new Padding(0, 6, 2, 0) });
      row.Controls.Add(ny);
      row.Controls.Add(new Label { Text = suffixY, AutoSize = true, Margin = new Padding(2, 6, 0, 0) });
      return row;
    }

    private Control _BuildPreviewSplit() {
      var host = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = new Padding(0, 10, 0, 10) };
      host.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
      host.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
      this._previewLabel = new Label {
        Text = "Preview — drag to pan · mouse-wheel to zoom",
        Dock = DockStyle.Fill,
        AutoSize = false,
        Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold),
        TextAlign = ContentAlignment.MiddleLeft,
      };
      host.Controls.Add(this._previewLabel, 0, 0);

      this._previewCanvas = new PreviewCanvas { Dock = DockStyle.Fill };
      this._previewCanvas.Paint += this._OnCanvasPaint;
      this._previewCanvas.Resize += (s, e) => { this._ClampViewCentre(); this._previewCanvas.Invalidate(); };
      this._previewCanvas.MouseDown += this._OnCanvasMouseDown;
      this._previewCanvas.MouseMove += this._OnCanvasMouseMove;
      this._previewCanvas.MouseUp += this._OnCanvasMouseUp;
      this._previewCanvas.MouseWheel += this._OnCanvasMouseWheel;
      this._previewCanvas.Cursor = Cursors.Hand;
      host.Controls.Add(this._previewCanvas, 0, 1);
      return host;
    }

    // Panel subclass that takes keyboard focus on hover so MouseWheel routes to it.
    private sealed class PreviewCanvas : Panel {
      public PreviewCanvas() {
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        this.TabStop = false;
        this.BorderStyle = BorderStyle.FixedSingle;
        this.BackColor = Color.FromArgb(245, 245, 245);
      }
      protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); if (this.CanFocus) this.Focus(); }
    }

    private Control _BuildButtonBar() {
      var bar = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 8, 0, 0), Dock = DockStyle.Fill };
      this._cancelButton = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true, Margin = new Padding(6, 0, 0, 0) };
      this._cancelButton.Click += (s, e) => { this.Close(); };
      this._okButton = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
      this._okButton.Click += (s, e) => { this.FinishTokenUpdate(); this.Close(); };
      bar.Controls.AddRange(new Control[] { this._cancelButton, this._okButton });
      this.AcceptButton = this._okButton;
      this.CancelButton = this._cancelButton;
      return bar;
    }

    #endregion

    #region variant-combo populate + selection

    private void _PopulateVariantCombo() {
      var entry = this._SelectedEntry() as ScaleVariantEntry;
      this._suppressEvents = true;
      try {
        this._variantCombo.Items.Clear();
        if (entry == null) return;
        foreach (var scale in entry.SupportedScales)
          this._variantCombo.Items.Add(new VariantItem(scale));
        if (this._variantCombo.Items.Count > 0) this._variantCombo.SelectedIndex = 0;
      } finally {
        this._suppressEvents = false;
      }
    }

    private readonly struct VariantItem {
      public readonly ScaleFactor Scale;
      public VariantItem(ScaleFactor s) { this.Scale = s; }
      public override string ToString() => this.Scale.X == this.Scale.Y ? this.Scale.X + "×" : this.Scale.X + "× × " + this.Scale.Y + "×";
    }

    #endregion

    #region event handlers

    private void _OnFilterTextChanged(object sender, EventArgs e) {
      var needle = this._algorithmFilter.Text?.Trim() ?? string.Empty;
      var previous = this._algorithmCombo.SelectedItem as string;
      this._suppressEvents = true;
      try {
        this._algorithmCombo.BeginUpdate();
        this._algorithmCombo.Items.Clear();
        foreach (var entry in SupportedManipulators.Manipulators) {
          if (needle.Length == 0 || entry.Name.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0)
            this._algorithmCombo.Items.Add(entry.Name);
        }
        if (previous != null && this._algorithmCombo.Items.IndexOf(previous) is int idx && idx >= 0)
          this._algorithmCombo.SelectedIndex = idx;
        else if (this._algorithmCombo.Items.Count > 0)
          this._algorithmCombo.SelectedIndex = 0;
        this._algorithmCombo.EndUpdate();
      } finally {
        this._suppressEvents = false;
      }
      this._UpdateAlgorithmInfo();
      this._SchedulePreview();
    }

    private void _OnAlgorithmChanged(object sender, EventArgs e) {
      if (this._suppressEvents) return;
      this._PopulateVariantCombo();
      this._UpdateAlgorithmInfo();
      this._UpdateModeVisibility();
      this._SchedulePreview();
    }

    private void _OnModeChanged(object sender, EventArgs e) {
      if (this._suppressEvents) return;
      this._UpdateModeVisibility();
      this._SchedulePreview();
    }

    private void _OnLockArChanged(object sender, EventArgs e) {
      if (this._suppressEvents) return;
      if (this._lockAr.Checked) {
        this._suppressEvents = true;
        try {
          var mode = this._CurrentMode();
          if (mode == ScaleMode.Percent) this._percentY.Value = this._percentX.Value;
          else if (mode == ScaleMode.Factor) this._factorY.Value = this._factorX.Value;
          else this._UpdateSizeAspect(this._targetWidth);
        } finally { this._suppressEvents = false; }
      }
      this._UpdateModeVisibility();
      this._SchedulePreview();
    }

    private void _OnAxisValueChanged(object sender, bool isX, ScaleMode mode) {
      if (this._suppressEvents) return;
      if (this._lockAr.Checked) {
        this._suppressEvents = true;
        try {
          if (mode == ScaleMode.Percent) {
            if (isX) this._percentY.Value = this._percentX.Value; else this._percentX.Value = this._percentY.Value;
          } else if (mode == ScaleMode.Factor) {
            if (isX) this._factorY.Value = this._factorX.Value; else this._factorX.Value = this._factorY.Value;
          } else {
            this._UpdateSizeAspect((NumericUpDown)sender);
          }
        } finally { this._suppressEvents = false; }
      }
      if (mode == ScaleMode.Size) {
        this._suppressEvents = true;
        try {
          if (sender == this._targetWidth) this._widthSlider.Value = Math.Max(this._widthSlider.Minimum, Math.Min(this._widthSlider.Maximum, (int)this._targetWidth.Value));
          if (sender == this._targetHeight) this._heightSlider.Value = Math.Max(this._heightSlider.Minimum, Math.Min(this._heightSlider.Maximum, (int)this._targetHeight.Value));
        } finally { this._suppressEvents = false; }
      }
      this._SchedulePreview();
    }

    private void _UpdateSizeAspect(NumericUpDown driver) {
      var src = this.EffectSourceSurface;
      if (src == null || src.Width <= 0 || src.Height <= 0) return;
      if (driver == this._targetWidth && this._targetWidth.Value > 0) {
        var h = (int)Math.Round((double)this._targetWidth.Value * src.Height / src.Width);
        if (h > 0 && h <= this._targetHeight.Maximum) this._targetHeight.Value = h;
      } else if (driver == this._targetHeight && this._targetHeight.Value > 0) {
        var w = (int)Math.Round((double)this._targetHeight.Value * src.Width / src.Height);
        if (w > 0 && w <= this._targetWidth.Maximum) this._targetWidth.Value = w;
      }
    }

    private void _OnWidthSliderChanged(object sender, EventArgs e) {
      if (this._suppressEvents) return;
      this._suppressEvents = true;
      try { this._targetWidth.Value = _Clamp(this._targetWidth, this._widthSlider.Value); } finally { this._suppressEvents = false; }
      this._OnAxisValueChanged(this._targetWidth, true, ScaleMode.Size);
    }

    private void _OnHeightSliderChanged(object sender, EventArgs e) {
      if (this._suppressEvents) return;
      this._suppressEvents = true;
      try { this._targetHeight.Value = _Clamp(this._targetHeight, this._heightSlider.Value); } finally { this._suppressEvents = false; }
      this._OnAxisValueChanged(this._targetHeight, false, ScaleMode.Size);
    }

    private void _OnOobChanged(object sender, EventArgs e) {
      if (this._suppressEvents) return;
      this._UpdateOobColorButton();
      this._SchedulePreview();
    }

    private void _OnPickOobColour(object sender, EventArgs e) {
      using (var dlg = new ColorDialog { Color = this._oobColor == Color.Empty ? Color.Black : this._oobColor, FullOpen = true, AnyColor = true }) {
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        this._oobColor = dlg.Color;
        this._UpdateOobColorButton();
        this._SchedulePreview();
      }
    }

    private void _UpdateOobColorButton() {
      var xIsConst = (this._oobXCombo.SelectedItem as OutOfBoundsMode?) == OutOfBoundsMode.ConstantExtension;
      var yIsConst = (this._oobYCombo.SelectedItem as OutOfBoundsMode?) == OutOfBoundsMode.ConstantExtension;
      var applies = this._OobAppliesToSelectedEntry();
      this._oobColorButton.Enabled = applies && (xIsConst || yIsConst);
      this._oobColorButton.BackColor = this._oobColor;
      this._oobColorButton.FlatAppearance.BorderColor = this._oobColorButton.Enabled ? SystemColors.ControlDarkDark : SystemColors.ControlDark;
      this._oobColorButton.Text = string.Empty;
    }

    private bool _OobAppliesToSelectedEntry() {
      var name = this._algorithmCombo.SelectedItem as string;
      if (name == null) return false;
      if (name.StartsWith("Scaler:") || name.StartsWith("Resampler:") || name.StartsWith("Filter:") || name.StartsWith("Plane:"))
        return false;
      return true;
    }

    private void _OnCopyClicked(object sender, EventArgs e) {
      try {
        var img = this._cachedFiltered;
        if (img == null) return;
        byte[] pngBytes;
        using (var owned = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(owned)) {
          g.CompositingMode = CompositingMode.SourceCopy;
          g.DrawImageUnscaled(img, 0, 0);
          using (var ms = new MemoryStream()) {
            owned.Save(ms, ImageFormat.Png);
            pngBytes = ms.ToArray();
          }
        }
        var data = new DataObject();
        data.SetData("PNG", false, new MemoryStream(pngBytes));
        using (var roundtrip = (Bitmap)Image.FromStream(new MemoryStream(pngBytes)))
          data.SetImage(roundtrip);
        Clipboard.SetDataObject(data, true);
      } catch (Exception ex) {
        MessageBox.Show(this, "Could not copy to clipboard:\n" + ex.Message, "2D Image Filter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void _SchedulePreview() {
      if (this._debounceTimer == null) return;
      this._debounceTimer.Interval = _PREVIEW_DEBOUNCE_MS;
      this._debounceTimer.Stop();
      this._debounceTimer.Start();
    }

    #endregion

    #region preview canvas paint + drag-to-pan + wheel-zoom (live, no refilter)

    private void _OnCanvasPaint(object sender, PaintEventArgs e) {
      var g = e.Graphics;
      var bmp = this._cachedFiltered;
      if (bmp == null) return;
      var cs = this._previewCanvas.ClientSize;
      if (cs.Width <= 0 || cs.Height <= 0) return;

      g.InterpolationMode = InterpolationMode.NearestNeighbor;
      g.PixelOffsetMode = PixelOffsetMode.Half;
      g.SmoothingMode = SmoothingMode.None;

      var destW = bmp.Width * this._viewZoom;
      var destH = bmp.Height * this._viewZoom;
      var destX = cs.Width / 2f - this._viewCenterX * this._viewZoom;
      var destY = cs.Height / 2f - this._viewCenterY * this._viewZoom;
      g.DrawImage(bmp, new RectangleF(destX, destY, destW, destH), new RectangleF(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
    }

    private void _OnCanvasMouseDown(object sender, MouseEventArgs e) {
      if (e.Button != MouseButtons.Left || this._cachedFiltered == null) return;
      this._panning = true;
      this._panAnchor = e.Location;
      this._panCentreAtStart = new PointF(this._viewCenterX, this._viewCenterY);
      this._previewCanvas.Cursor = Cursors.SizeAll;
      this._previewCanvas.Capture = true;
      this._previewCanvas.Focus();
    }

    private void _OnCanvasMouseMove(object sender, MouseEventArgs e) {
      if (!this._panning || this._cachedFiltered == null) return;
      var dx = (e.X - this._panAnchor.X) / this._viewZoom;
      var dy = (e.Y - this._panAnchor.Y) / this._viewZoom;
      this._viewCenterX = this._panCentreAtStart.X - dx;
      this._viewCenterY = this._panCentreAtStart.Y - dy;
      this._ClampViewCentre();
      this._previewCanvas.Invalidate();
    }

    private void _OnCanvasMouseUp(object sender, MouseEventArgs e) {
      if (!this._panning) return;
      this._panning = false;
      this._previewCanvas.Capture = false;
      this._previewCanvas.Cursor = Cursors.Hand;
    }

    private void _OnCanvasMouseWheel(object sender, MouseEventArgs e) {
      if (this._cachedFiltered == null) return;
      var cs = this._previewCanvas.ClientSize;
      if (cs.Width <= 0 || cs.Height <= 0) return;

      // Filtered-image coord under the cursor, before zoom change.
      var fx = this._viewCenterX + (e.X - cs.Width / 2f) / this._viewZoom;
      var fy = this._viewCenterY + (e.Y - cs.Height / 2f) / this._viewZoom;

      var factor = e.Delta > 0 ? 1.25f : 0.8f;
      var newZoom = this._viewZoom * factor;
      var fitZoom = this._FitZoom();
      newZoom = Math.Max(fitZoom, Math.Min(_MAX_ZOOM, newZoom));
      if (Math.Abs(newZoom - this._viewZoom) < 1e-4f) return;
      this._viewZoom = newZoom;

      // Keep the filtered pixel under the cursor anchored.
      this._viewCenterX = fx - (e.X - cs.Width / 2f) / this._viewZoom;
      this._viewCenterY = fy - (e.Y - cs.Height / 2f) / this._viewZoom;
      this._ClampViewCentre();
      this._previewCanvas.Invalidate();
    }

    private float _FitZoom() {
      var bmp = this._cachedFiltered;
      if (bmp == null) return 1f;
      var cs = this._previewCanvas.ClientSize;
      if (cs.Width <= 0 || cs.Height <= 0) return 1f;
      return Math.Min((float)cs.Width / bmp.Width, (float)cs.Height / bmp.Height);
    }

    private void _ClampViewCentre() {
      var bmp = this._cachedFiltered;
      if (bmp == null) return;
      var cs = this._previewCanvas.ClientSize;
      if (cs.Width <= 0 || cs.Height <= 0) return;

      // Visible half-size in filtered-image coords.
      var halfW = cs.Width / 2f / this._viewZoom;
      var halfH = cs.Height / 2f / this._viewZoom;
      // Clamp so we don't pan entirely past the image; allow centre to roam within [halfW..W-halfW] when image bigger than view,
      // otherwise lock to image centre.
      if (bmp.Width * this._viewZoom <= cs.Width) this._viewCenterX = bmp.Width / 2f;
      else this._viewCenterX = Math.Max(halfW, Math.Min(bmp.Width - halfW, this._viewCenterX));
      if (bmp.Height * this._viewZoom <= cs.Height) this._viewCenterY = bmp.Height / 2f;
      else this._viewCenterY = Math.Max(halfH, Math.Min(bmp.Height - halfH, this._viewCenterY));
    }

    private void _ResetViewToFit() {
      var bmp = this._cachedFiltered;
      if (bmp == null) return;
      this._viewZoom = this._FitZoom();
      this._viewCenterX = bmp.Width / 2f;
      this._viewCenterY = bmp.Height / 2f;
    }

    #endregion

    #region preview pipeline — snapshot → background render → BeginInvoke back

    private readonly struct UiSnapshot {
      public readonly ManipulatorEntry Entry;
      public readonly ScaleMode Mode;
      public readonly double PercentX, PercentY, FactorX, FactorY;
      public readonly int TargetWidth, TargetHeight;
      public readonly bool LockAr;
      public readonly OutOfBoundsMode OobX, OobY;
      public readonly bool ApplyOob;
      public readonly Color OobColor;
      /// <summary>When non-default, forces the pixel-scaler to this supported variant (ScaleVariantEntry).</summary>
      public readonly ScaleFactor? VariantScale;

      public UiSnapshot(ManipulatorEntry entry, ScaleMode mode, double px, double py, double fx, double fy, int tw, int th, bool lockAr, OutOfBoundsMode oobX, OutOfBoundsMode oobY, bool applyOob, Color oobColor, ScaleFactor? variantScale) {
        this.Entry = entry; this.Mode = mode;
        this.PercentX = px; this.PercentY = py;
        this.FactorX = fx; this.FactorY = fy;
        this.TargetWidth = tw; this.TargetHeight = th;
        this.LockAr = lockAr;
        this.OobX = oobX; this.OobY = oobY;
        this.ApplyOob = applyOob; this.OobColor = oobColor;
        this.VariantScale = variantScale;
      }

      public (int w, int h) ResolveTarget(int srcW, int srcH) {
        if (this.Entry == null || !this.Entry.SupportsCustomDimensions) return (srcW, srcH);
        if (this.VariantScale.HasValue)
          return (Math.Max(1, srcW * this.VariantScale.Value.X), Math.Max(1, srcH * this.VariantScale.Value.Y));
        double sx, sy;
        switch (this.Mode) {
          case ScaleMode.Percent:
            sx = this.PercentX / 100.0;
            sy = this.LockAr ? sx : this.PercentY / 100.0;
            return (Math.Max(1, (int)Math.Round(srcW * sx)), Math.Max(1, (int)Math.Round(srcH * sy)));
          case ScaleMode.Factor:
            sx = this.FactorX;
            sy = this.LockAr ? sx : this.FactorY;
            return (Math.Max(1, (int)Math.Round(srcW * sx)), Math.Max(1, (int)Math.Round(srcH * sy)));
          case ScaleMode.Size:
          default: {
            var w = this.TargetWidth > 0 ? this.TargetWidth : srcW;
            var h = this.TargetHeight > 0 ? this.TargetHeight : srcH;
            if (this.LockAr && this.TargetWidth > 0 && srcW > 0)
              h = Math.Max(1, (int)Math.Round((double)w * srcH / srcW));
            return (w, h);
          }
        }
      }
    }

    private UiSnapshot _TakeSnapshot() {
      ScaleFactor? variant = null;
      if (this._SelectedEntry() is ScaleVariantEntry && this._variantCombo.SelectedItem is VariantItem vi)
        variant = vi.Scale;
      return new UiSnapshot(
        this._SelectedEntry(),
        this._CurrentMode(),
        (double)this._percentX.Value,
        (double)this._percentY.Value,
        (double)this._factorX.Value,
        (double)this._factorY.Value,
        (int)this._targetWidth.Value,
        (int)this._targetHeight.Value,
        this._lockAr.Checked,
        (OutOfBoundsMode)(this._oobXCombo.SelectedItem ?? OutOfBoundsMode.ConstantExtension),
        (OutOfBoundsMode)(this._oobYCombo.SelectedItem ?? OutOfBoundsMode.ConstantExtension),
        this._OobAppliesToSelectedEntry(),
        this._oobColor,
        variant
      );
    }

    private async void _OnDebounceTick(object sender, EventArgs e) {
      this._debounceTimer.Stop();
      if (!this.IsHandleCreated || this.IsDisposed) return;
      if (this.InvokeRequired) {
        try { this.BeginInvoke(new EventHandler(this._OnDebounceTick), sender, e); } catch { }
        return;
      }

      UiSnapshot snapshot;
      Bitmap sourceSnapshot;
      try {
        snapshot = this._TakeSnapshot();
        var surface = this.EffectSourceSurface;
        if (snapshot.Entry == null || surface == null) return;
        using (var alias = surface.CreateAliasedBitmap()) {
          sourceSnapshot = new Bitmap(alias.Width, alias.Height, PixelFormat.Format32bppArgb);
          using (var g = Graphics.FromImage(sourceSnapshot)) {
            g.CompositingMode = CompositingMode.SourceCopy;
            g.DrawImageUnscaled(alias, 0, 0);
          }
        }
      } catch (Exception ex) {
        this._SafeSetLabel(this._previewLabel, "Snapshot error: " + ex.Message);
        return;
      }

      this._renderCts?.Cancel();
      var cts = this._renderCts = new CancellationTokenSource();
      try {
        await this._RenderPreviewAsync(snapshot, sourceSnapshot, cts.Token).ConfigureAwait(false);
      } catch (OperationCanceledException) { } catch (Exception ex) {
        this._SafeSetLabel(this._previewLabel, "Preview error: " + ex.Message);
      } finally {
        sourceSnapshot.Dispose();
      }
    }

    private async Task _RenderPreviewAsync(UiSnapshot snapshot, Bitmap sourceSnapshot, CancellationToken cancel) {
      var entry = snapshot.Entry;
      var (targetW, targetH) = snapshot.ResolveTarget(sourceSnapshot.Width, sourceSnapshot.Height);
      this._SafeSetLabel(this._previewLabel, $"Preview ({sourceSnapshot.Width}×{sourceSnapshot.Height} → {targetW}×{targetH}) — drag to pan · mouse-wheel to zoom");

      await this._renderGate.WaitAsync(cancel).ConfigureAwait(false);
      Bitmap result = null;
      try {
        result = await Task.Run(() => {
          cancel.ThrowIfCancellationRequested();
          using (var own = new Bitmap(sourceSnapshot)) {
            var image = cImage.FromBitmap(own);
            if (snapshot.ApplyOob) { image.HorizontalOutOfBoundsMode = snapshot.OobX; image.VerticalOutOfBoundsMode = snapshot.OobY; }
            var fullRect = new Rectangle(0, 0, own.Width, own.Height);
            var output = entry.Apply(image, fullRect, targetW, targetH);
            cancel.ThrowIfCancellationRequested();
            return output.ToBitmap();
          }
        }, cancel).ConfigureAwait(false);
      } finally {
        this._renderGate.Release();
      }

      if (cancel.IsCancellationRequested) { result?.Dispose(); return; }

      var r = result;
      this._SafeRunOnUi(() => {
        if (this._previewCanvas == null || this._previewCanvas.IsDisposed) { r?.Dispose(); return; }
        var old = this._cachedFiltered;
        var oldDims = old == null ? (w: -1, h: -1) : (w: old.Width, h: old.Height);
        this._cachedFiltered = r;
        // On first bitmap or dimension change, fit. Otherwise keep current zoom/pan.
        if (old == null || oldDims.w != r.Width || oldDims.h != r.Height)
          this._ResetViewToFit();
        else
          this._ClampViewCentre();
        old?.Dispose();
        this._previewCanvas.Invalidate();
      });
    }

    private void _SafeSetLabel(Label target, string text) {
      if (target == null) return;
      this._SafeRunOnUi(() => { target.Text = text; });
    }

    private void _SafeRunOnUi(Action action) {
      if (action == null) return;
      if (!this.IsHandleCreated || this.IsDisposed) return;
      if (this.InvokeRequired) {
        try { this.BeginInvoke(action); } catch { }
      } else {
        try { action(); } catch { }
      }
    }

    #endregion

    #region helpers

    private ManipulatorEntry _SelectedEntry() {
      var name = this._algorithmCombo.SelectedItem as string;
      if (name == null) return null;
      foreach (var e in SupportedManipulators.Manipulators) if (e.Name == name) return e;
      return null;
    }

    private void _UpdateModeVisibility() {
      var entry = this._SelectedEntry();
      var isVariant = entry is ScaleVariantEntry;
      var isResampler = entry is ResampleEntry;
      var freeMode = isResampler;
      var mode = this._CurrentMode();

      this._variantPanel.Visible = isVariant;
      this._modeRadioPercent.Visible = !isVariant;
      this._modeRadioFactor.Visible = !isVariant;
      this._modeRadioSize.Visible = !isVariant;
      this._percentBox.Visible = !isVariant;
      this._factorBox.Visible = !isVariant;
      this._sizeBox.Visible = !isVariant;
      this._lockAr.Visible = !isVariant;

      this._modeRadioPercent.Enabled = freeMode;
      this._modeRadioFactor.Enabled = freeMode;
      this._modeRadioSize.Enabled = freeMode;
      this._lockAr.Enabled = freeMode;

      this._percentX.Enabled = freeMode && mode == ScaleMode.Percent;
      this._percentY.Enabled = freeMode && mode == ScaleMode.Percent && !this._lockAr.Checked;
      this._factorX.Enabled = freeMode && mode == ScaleMode.Factor;
      this._factorY.Enabled = freeMode && mode == ScaleMode.Factor && !this._lockAr.Checked;
      this._targetWidth.Enabled = freeMode && mode == ScaleMode.Size;
      this._targetHeight.Enabled = freeMode && mode == ScaleMode.Size && !this._lockAr.Checked;
      this._widthSlider.Enabled = this._targetWidth.Enabled;
      this._heightSlider.Enabled = this._targetHeight.Enabled;

      this._variantCombo.Enabled = isVariant;

      var oobApplies = this._OobAppliesToSelectedEntry();
      this._oobXCombo.Enabled = oobApplies;
      this._oobYCombo.Enabled = oobApplies;
      this._UpdateOobColorButton();
    }

    private void _UpdateAlgorithmInfo() {
      var entry = this._SelectedEntry();
      if (entry == null) {
        this._algorithmInfo.Text = string.Empty;
        this._scaleInfo.Text = string.Empty;
        this._propertyGrid.SelectedObject = null;
        return;
      }
      this._algorithmInfo.Text = entry.Description ?? string.Empty;
      if (entry is ScaleVariantEntry sve) {
        var scales = string.Join(", ", sve.SupportedScales.Select(s => Imager.Pipelines.UpstreamPipeline.FormatScaleSuffix(s)));
        this._scaleInfo.Text = "Supported scales: " + scales;
      } else if (entry is ResampleEntry) {
        this._scaleInfo.Text = "Free target dimensions.";
      } else if (entry is FixedScaleEntry fse) {
        this._scaleInfo.Text = "Fixed scale: " + fse.ScaleX + "×" + fse.ScaleY;
      } else {
        this._scaleInfo.Text = string.Empty;
      }
      this._propertyGrid.SelectedObject = new AlgorithmInfoDto(entry);
    }

    private sealed class AlgorithmInfoDto {
      private readonly ManipulatorEntry _entry;
      public AlgorithmInfoDto(ManipulatorEntry entry) { this._entry = entry; }

      [Category("General")]
      public string Name => this._entry?.Name ?? string.Empty;

      [Category("General")]
      public string Type {
        get {
          if (this._entry is ScaleVariantEntry) return "Multi-scale fixed (pick a supported variant)";
          if (this._entry is ResampleEntry) return "Resampler (free target W/H)";
          if (this._entry is FixedScaleEntry) return "Single-scale fixed";
          return this._entry?.GetType().Name ?? string.Empty;
        }
      }

      [Category("General")]
      [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
      public string Description => this._entry?.Description ?? string.Empty;

      [Category("Scale")]
      public bool SupportsCustomDimensions => this._entry != null && this._entry.SupportsCustomDimensions;

      [Category("Scale")]
      public string SupportedScales {
        get {
          if (this._entry is ScaleVariantEntry sve)
            return string.Join(", ", sve.SupportedScales.Select(s => Imager.Pipelines.UpstreamPipeline.FormatScaleSuffix(s)));
          if (this._entry is FixedScaleEntry fse)
            return fse.ScaleX + "×" + fse.ScaleY;
          return "(variable)";
        }
      }
    }

    #endregion

    protected override void Dispose(bool disposing) {
      if (disposing) {
        this._renderCts?.Cancel();
        this._renderCts?.Dispose();
        this._debounceTimer?.Dispose();
        this._renderGate?.Dispose();
        this._cachedFiltered?.Dispose();
        this._cachedFiltered = null;
      }
      base.Dispose(disposing);
    }
  }
}
