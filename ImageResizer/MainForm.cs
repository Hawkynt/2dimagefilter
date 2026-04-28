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
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Classes;
using Classes.ImageManipulators;
using Classes.ScriptActions;

using ImageResizer.Properties;

using System.Drawing.Extensions.ColorProcessing.Resizing;
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
    /// Canvas colour used as the OOB constant when either border-pixel-handling combo is set to FlatColor.
    /// Defaults to <see cref="Color.Transparent"/>.
    /// </summary>
    private Color _canvasColor = Color.Transparent;
    /// <summary>
    /// The used scripting engine.
    /// </summary>
    private readonly ScriptEngine _scriptEngine = new ScriptEngine();

    /// <summary>
    /// File-keyed LRU pool of master <see cref="Bitmap"/> instances. The pool owns each master;
    /// the UI references them but never modifies/locks. Workers obtain private copies via
    /// <see cref="BitmapMasterPool.CheckoutClone"/>. Disposed on form close after detaching the
    /// PictureBox to avoid a paint racing with master disposal.
    /// </summary>
    private readonly BitmapMasterPool _masterPool = new BitmapMasterPool(maxRecords: 8);

    // Debounced preview: each relevant change (method, dimensions, OOB, etc.) calls
    // _SchedulePreview, which restarts the timer; 300 ms after the last change the timer
    // tick kicks off an async preview render into iwhTargetImage.
    private System.Windows.Forms.Timer _previewDebounce;
    private CancellationTokenSource _previewCts;

    // The preview-owned bitmap currently shown in iwhTargetImage, if any. We OWN it and must
    // dispose it when replacing. Stays null when the target pane displays an engine-owned
    // bitmap (scriptEngine.GdiTarget) — disposing that would corrupt engine state and crash
    // the next PictureBox paint via ImageAnimator.CanAnimate(FrameDimensionsList).
    private Bitmap _previewOwnedTarget;

    // Parameter bag for the currently selected manipulator, when that manipulator surfaces a
    // non-empty <see cref="Hawkynt.ColorProcessing.ParameterDescriptor"/> set. Reset on every
    // selection change. Apply paths read this back through <see cref="_BindManipulatorParameters"/>
    // and call <see cref="IImageManipulator.CreateWith"/> before handing the manipulator to
    // <see cref="Classes.ScriptActions.ResizeCommand"/>.
    private ManipulatorParameterBag _currentParameterBag;
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
        // Intentionally NOT scheduling a preview here. Apply's continuation re-assigns
        // _SourceImage = scriptEngine.GdiSource which would otherwise fire a spurious preview
        // that clobbers the authoritative Apply result. Explicit load sites
        // (_LoadImageFromFileName and SetSource-style callers) schedule their own preview.
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
        // Transitioning to an engine-owned bitmap — dispose any preview-owned bitmap we
        // were previously showing, but never touch `value` itself (ScriptEngine still owns it).
        var previouslyOwned = this._previewOwnedTarget;
        this._previewOwnedTarget = null;
        this.iwhTargetImage.Image = value;
        previouslyOwned?.Dispose();
      }
    }

    private PictureBoxSizeMode _SourceImageSizeMode {
      get { return this.iwhSourceImage.SizeMode; }
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
      get { return this.iwhTargetImage.SizeMode; }
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

      // Wire the master pool into the engine so LoadFileCommand routes through pool.LoadOrGet
      // and the engine adopts pool-managed source bitmaps non-owning. The pool itself is owned
      // by the form and disposed on form close (after the PictureBox is detached).
      this._scriptEngine.MasterPool = this._masterPool;

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

      this.chkUseThresholds.Checked = false;

      this._LoadConfigurationSettings();

      if (fileToOpenOnStart != null)
        this._LoadImageFromFileName(fileToOpenOnStart);

      // "Tools" menu added in code to avoid touching the generated Designer.cs. Inserted
      // left of "Help" — that's the conventional place for a Tools menu (File/Edit/View/
      // ...tools... /Window/Help across Windows apps).
      var toolsMenu = new ToolStripMenuItem("&Tools");
      var reduceColorsItem = new ToolStripMenuItem("&Reduce Colours…", null, this._OnReduceColoursClicked);
      toolsMenu.DropDownItems.Add(reduceColorsItem);
      var helpIndex = this.msMain.Items.IndexOf(this.helpToolStripMenuItem);
      if (helpIndex >= 0)
        this.msMain.Items.Insert(helpIndex, toolsMenu);
      else
        this.msMain.Items.Add(toolsMenu);

      // Preview-debounce timer — 300 ms after the last parameter change fires a preview render.
      this._previewDebounce = new System.Windows.Forms.Timer { Interval = 300 };
      this._previewDebounce.Tick += this._OnPreviewDebounceTick;

      // Checkboxes that affect the preview output but have no Designer-generated handler.
      this.chkUseThresholds.CheckedChanged += (s, e) => this._SchedulePreview();
      this.chkUseCenteredGrid.CheckedChanged += (s, e) => this._SchedulePreview();
      this.chkKeepAspect.CheckedChanged += (s, e) => this._SchedulePreview();
      this.cmbHorizontalBPH.SelectedIndexChanged += (s, e) => { this._UpdateCanvasColorVisibility(); this._SchedulePreview(); };
      this.cmbVerticalBPH.SelectedIndexChanged += (s, e) => { this._UpdateCanvasColorVisibility(); this._SchedulePreview(); };

      // PropertyGrid value changes (parametric manipulator tuning) re-trigger the auto-preview
      // so the user sees the effect of a tweaked parameter without clicking Apply.
      this.pgManipulatorParameters.PropertyValueChanged += (s, e) => this._SchedulePreview();
    }

    private void _OnReduceColoursClicked(object sender, EventArgs e) {
      var src = this.iwhSourceImage.Image as System.Drawing.Bitmap
             ?? (this.iwhSourceImage.Image == null ? null : new System.Drawing.Bitmap(this.iwhSourceImage.Image));
      if (src == null) {
        MessageBox.Show(this, "Load a source image first.", "Reduce Colours", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      using (var dlg = new Windows.ReduceColorsWindow(src)) {
        if (dlg.ShowDialog(this) != DialogResult.OK || dlg.PickedQuantizer == null) return;
        var command = new Classes.ScriptActions.ReduceColorsCommand(dlg.PickedQuantizer, dlg.PickedDitherer, dlg.PaletteSize);
        this._ExecuteScriptActions(command);
      }
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
    /// If the current selection has a parameter surface and the user has changed at least one
    /// value, returns a freshly-bound manipulator instance via <see cref="IImageManipulator.CreateWith"/>;
    /// otherwise returns <paramref name="manipulator"/> unchanged. Centralises the apply-time
    /// binding so the explicit Apply path and the auto-preview path stay in sync.
    /// </summary>
    private IImageManipulator _BindManipulatorParameters(IImageManipulator manipulator) {
      var bag = this._currentParameterBag;
      if (manipulator == null || bag == null || !bag.HasOverrides)
        return manipulator;
      return manipulator.CreateWith(bag.ToValues()) ?? manipulator;
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

      if (targetWidth <= 0 && method.SupportsWidth || targetHeight <= 0 && method.SupportsHeight) {
        MessageBox.Show(Resources.txNeedWidthAndHeightAboveZero, Resources.ttNeedWidthAndHeightAboveZero, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        return;
      }

      // Giant-target guard: GDI+ refuses bitmaps beyond ~32767 per side, and a 5 GB+ target
      // often OOMs even with 64-bit VAS. Warn + confirm rather than let it crash mid-render.
      const int gdiPlusMaxDim = 32767;
      const long bitmapPixelBudget = 500_000_000L; // ≈ 2 GB ARGB, single allocation
      if (method.SupportsWidth && method.SupportsHeight
          && (targetWidth > gdiPlusMaxDim || targetHeight > gdiPlusMaxDim
              || (long)targetWidth * targetHeight > bitmapPixelBudget)) {
        var answer = MessageBox.Show(this,
          $"The requested target size {targetWidth}×{targetHeight} exceeds safe GDI+ limits " +
          $"(max {gdiPlusMaxDim} per side, ≤ {bitmapPixelBudget / 1_000_000} megapixels total). " +
          "Continuing will very likely run out of memory or fail inside GDI+. Proceed anyway?",
          "Very large target",
          MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (answer != DialogResult.Yes) return;
      }

      var bound = this._BindManipulatorParameters(method);
      var command = new ResizeCommand(applyToTarget, bound, targetWidth, targetHeight, 0, maintainAspect, horizontalBph, verticalBph, repetitionCount, useThresholds, useCenteredGrid, radius, this._canvasColor);

      this._ExecuteScriptActions(command);
    }

    /// <summary>
    /// Executes the given script actions.
    /// </summary>
    /// <param name="commands">The commands.</param>
    private void _ExecuteScriptActions(params IScriptAction[] commands) {
      Contract.Requires(commands != null);

      // Any in-flight preview would clobber the authoritative result on completion; kill it.
      this._previewDebounce?.Stop();
      this._previewCts?.Cancel();

      // Detach the target PictureBox from any bitmap BEFORE kicking off the Async work.
      // ScriptEngine.TargetImage setter disposes its cached _gdiTarget as part of the
      // compute; if the PictureBox still references that bitmap, a WM_PAINT arriving
      // mid-dispose crashes inside ImageAnimator.CanAnimate → FrameDimensionsList.
      // Route through our own setter so any preview-owned bitmap is cleaned up too.
      this._TargetImage = null;

      // tell the user that we're busy
      this.msMain.Enabled =
        this.tlpMainLayout.Enabled =
          !(this.tssBusy.Visible = true);

      // Capture current source/effective-target dims on the UI thread so the post-failure
      // classifier can annotate generic "Invalid parameter" GDI+ errors with something
      // actionable ("target ≥ 32767 px — exceeds GDI+ limit").
      var srcBitmap = this._scriptEngine.GdiSource;
      var srcW = srcBitmap?.Width ?? 0;
      var srcH = srcBitmap?.Height ?? 0;

      this.Async(() => {
        // filter image
        var stopwatch = new Stopwatch();
        stopwatch.Restart();

        Exception failure = null;
        try {
          foreach (var command in commands)
            this._scriptEngine.ExecuteAction(command);
        } catch (OutOfMemoryException ex) {
          failure = ex;
        } catch (Exception ex) {
          failure = ex;
        }

        var gdiSource = this._scriptEngine.GdiSource;
        var gdiTarget = this._scriptEngine.GdiTarget;
        stopwatch.Stop();

        this.SafelyInvoke(() => {
          // Always restore the "busy" state so the UI isn't dead-locked on failure.
          this.msMain.Enabled =
            this.tlpMainLayout.Enabled =
              !(this.tssBusy.Visible = false);
          this.Enabled = true;

          if (failure != null) {
            var unwrapped = _Unwrap(failure);
            var tag = _ClassifyResizeFailure(unwrapped, srcW, srcH);
            this.iwhTargetImage.StatusText = "Resize failed — " + tag;
            MessageBox.Show(this, tag, "Resize failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
          }

          this._SourceImage = gdiSource;
          this._TargetImage = gdiTarget;
          this.iwhTargetImage.StatusText = null; // explicit apply → no longer a "Preview"
          // Preview forces Zoom to fit the target panel; explicit Apply restores the user's preference.
          if (Config.TargetSizeMode.HasValue)
            this.iwhTargetImage.SizeMode = Config.TargetSizeMode.Value;

          this.tssBenchmark.Text = stopwatch.ElapsedMilliseconds + "ms";
          this.tssBenchmark.Visible = true;
        });
      });
    }

    /// <summary>
    /// Refreshes the kernel chart if the currently selected manipulator is an upstream
    /// separable-kernel resampler; hides it otherwise.
    /// </summary>
    private void _RefreshKernelChart() {
      var chart = this.chtKernel;
      var dataPointCollection = chart.Series[0].Points;
      dataPointCollection.Clear();
      chart.Visible = false;

      if (!(this.cmbResizeMethod.SelectedValue is BitmapResamplerAdapter upstream))
        return;
      if (upstream.EvaluateKernel == null || upstream.KernelRadius <= 0)
        return;

      var radius = upstream.KernelRadius;
      var kernel = upstream.EvaluateKernel;
      for (var x = (float)-radius; x <= radius; x += 0.001f)
        dataPointCollection.AddXY(Math.Round(x, 3), Math.Round(kernel(x), 3));
      chart.ChartAreas[0].AxisX.Minimum = -radius;
      chart.ChartAreas[0].AxisX.Maximum = radius;
      chart.Visible = true;
    }

    /// <summary>
    /// Maps an exception from <see cref="_ExecuteScriptActions"/> to a human-actionable message.
    /// The GDI+ <c>ArgumentException("Ungültiger Parameter")</c> in particular is a black-box
    /// error from the native library — usually "you asked for a bitmap larger than I can make"
    /// (max 32767 per side, or the address space can't hold it). This classifier inspects the
    /// source dimensions + failure type + stack trace to emit something useful instead.
    /// </summary>
    private static string _ClassifyResizeFailure(Exception ex, int sourceWidth, int sourceHeight) {
      const int gdiPlusMaxDim = 32767;
      if (ex is OutOfMemoryException)
        return $"Out of memory — the target bitmap is too large for GDI+ / your address space.\nSource was {sourceWidth}×{sourceHeight}. Try a smaller scale or start from a downsampled source.";

      var isGdi = ex.StackTrace != null && ex.StackTrace.IndexOf("System.Drawing", StringComparison.Ordinal) >= 0;
      if (isGdi && (ex is ArgumentException || ex is System.Runtime.InteropServices.ExternalException)) {
        var guess = string.Empty;
        if (sourceWidth >= gdiPlusMaxDim / 2 || sourceHeight >= gdiPlusMaxDim / 2)
          guess = $" Source is {sourceWidth}×{sourceHeight} — most rescalers double or triple it, which pushes past GDI+'s {gdiPlusMaxDim}-px-per-side limit.";
        return "GDI+ rejected the target bitmap — typically the requested dimensions exceed " +
               gdiPlusMaxDim + " pixels per side or the total pixel count overflows." + guess +
               "\n(Original error: " + ex.GetType().Name + ": " + ex.Message + ")";
      }

      return ex.GetType().Name + ": " + ex.Message;
    }

    /// <summary>
    /// Peels <see cref="System.Reflection.TargetInvocationException"/> and
    /// <see cref="AggregateException"/> wrappers so the error UI shows the useful inner
    /// exception (e.g. <c>OutOfMemoryException</c>, <c>ArgumentException</c> from GDI+)
    /// instead of the generic "TargetInvocationException" envelope.
    /// </summary>
    private static Exception _Unwrap(Exception ex) {
      for (var i = 0; i < 8 && ex != null; ++i) {
        if (ex is System.Reflection.TargetInvocationException && ex.InnerException != null) {
          ex = ex.InnerException;
          continue;
        }
        if (ex is AggregateException agg && agg.InnerException != null) {
          ex = agg.InnerException;
          continue;
        }
        break;
      }
      return ex;
    }

    /// <summary>Show the canvas-colour swatch only when either OOB combo is set to FlatColor.</summary>
    private void _UpdateCanvasColorVisibility() {
      var horizontal = this.cmbHorizontalBPH.SelectedItem as OutOfBoundsMode?;
      var vertical = this.cmbVerticalBPH.SelectedItem as OutOfBoundsMode?;
      var show = horizontal == OutOfBoundsMode.FlatColor || vertical == OutOfBoundsMode.FlatColor;
      this.lblCanvasColor.Visible = show;
      this.pnCanvasColor.Visible = show;
    }

    private void pnCanvasColor_Paint(object sender, PaintEventArgs e) {
      var rect = this.pnCanvasColor.ClientRectangle;
      if (this._canvasColor.A < 255) {
        // Checkerboard so transparency / partial alpha is visually obvious.
        const int cell = 4;
        for (var y = 0; y < rect.Height; y += cell)
        for (var x = 0; x < rect.Width; x += cell) {
          var brush = (((x / cell) + (y / cell)) & 1) == 0 ? System.Drawing.Brushes.LightGray : System.Drawing.Brushes.White;
          e.Graphics.FillRectangle(brush, x, y, cell, cell);
        }
      }
      using var fill = new System.Drawing.SolidBrush(this._canvasColor);
      e.Graphics.FillRectangle(fill, rect);
    }

    private void pnCanvasColor_Click(object sender, EventArgs e) {
      using var dlg = new ColorDialog {
        AllowFullOpen = true,
        FullOpen = true,
        AnyColor = true,
        Color = this._canvasColor.A == 0 ? Color.White : Color.FromArgb(255, this._canvasColor),
      };
      if (dlg.ShowDialog(this) != DialogResult.OK) return;
      this._canvasColor = Color.FromArgb(255, dlg.Color);
      this.pnCanvasColor.Invalidate();
      this._SchedulePreview();
    }

    private void pnCanvasColor_DoubleClick(object sender, EventArgs e) {
      this._canvasColor = Color.Transparent;
      this.pnCanvasColor.Invalidate();
      this._SchedulePreview();
    }

    /// <summary>
    /// (Re)starts the preview debounce. Any call resets the timer to 300 ms. Called from
    /// the method dropdown + the dimension/OOB change handlers.
    /// </summary>
    private void _SchedulePreview() {
      if (this._previewDebounce == null) return;
      this._previewDebounce.Stop();
      this._previewDebounce.Start();
    }

    private void _OnPreviewDebounceTick(object sender, EventArgs e) {
      this._previewDebounce.Stop();
      this._RenderPreviewAsync();
    }

    /// <summary>
    /// Maximum width or height of the bitmap fed into a preview. Anything larger is
    /// bicubic-downscaled before running the method so a 12800×12800 source scaled 4x doesn't
    /// allocate a 10 GB target and OOM. Applied only to previews — explicit Apply still uses
    /// the full-resolution source.
    /// </summary>
    private const int _PREVIEW_MAX_SOURCE_DIM = 1024;

    /// <summary>
    /// Render an auto-preview into <c>iwhTargetImage</c> using the currently selected method
    /// + dimensions, without disabling the rest of the UI and without mutating the ScriptEngine
    /// state (so an explicit Apply still works as before). Supersedes any in-flight preview.
    /// </summary>
    private void _RenderPreviewAsync() {
      this._previewCts?.Cancel();
      var cts = this._previewCts = new CancellationTokenSource();
      var token = cts.Token;

      var method = this.cmbResizeMethod.SelectedValue as IImageManipulator;
      var fullSource = this._scriptEngine.SourceImage;
      if (method == null || fullSource == null) return;

      // Bind parameter overrides into a fresh manipulator instance so the preview reflects
      // whatever the user has typed into the PropertyGrid. Snapshot here on the UI thread —
      // the Task.Run continuation must not touch the bag.
      method = this._BindManipulatorParameters(method);

      // Snapshot UI state up-front — everything below runs on a worker thread.
      var targetWidth = (word)this.nudWidth.Value;
      var targetHeight = (word)this.nudHeight.Value;
      var maintainAspect = this.chkKeepAspect.Checked;
      var useThresholds = this.chkUseThresholds.Checked;
      var useCenteredGrid = this.chkUseCenteredGrid.Checked;
      var repetitionCount = (byte)this.nudRepetitionCount.Value;
      var horizontalBph = (OutOfBoundsMode)this.cmbHorizontalBPH.SelectedItem;
      var verticalBph = (OutOfBoundsMode)this.cmbVerticalBPH.SelectedItem;
      var radius = (float)this.nudRadius.Value;
      var capturedCanvasColor = this._canvasColor;

      if (targetWidth <= 0 && method.SupportsWidth) return;
      if (targetHeight <= 0 && method.SupportsHeight) return;

      // Track-C: obtain a privately-owned source clone so the worker thread never touches the
      // engine-owned / pool-owned master that iwhSourceImage is simultaneously painting. WinForms
      // WM_PAINT is serialised on the UI thread, so this same-thread clone cannot collide with
      // PictureBox.OnPaint. Without a clone, a WM_PAINT arriving mid-Apply on the worker would
      // race with the manipulator's LockBits and crash with "Bitmap region is already locked".
      // Preferred path: pool.CheckoutClone(currentKey) — full-size clone owned by the caller.
      // Fallback (legacy, e.g. a drag-dropped raw bitmap not yet routed through the pool):
      // clone here on the UI thread the same way the pool would.
      Bitmap previewSourceOwned;
      word previewTargetWidth = targetWidth;
      word previewTargetHeight = targetHeight;
      string sizeNote = null;
      try {
        var poolKey = this._scriptEngine.CurrentSourceKey;
        Bitmap fullClone;
        if (poolKey != null) {
          fullClone = this._masterPool.CheckoutClone(poolKey);
        } else {
          fullClone = new Bitmap(fullSource.Width, fullSource.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
          using var gFull = Graphics.FromImage(fullClone);
          gFull.DrawImageUnscaled(fullSource, 0, 0);
        }

        // Guard 1: shrink giant sources so every method/scale stays in a sane memory budget.
        // The pool always returns a full-size clone; the preview shrink lives downstream.
        if (fullClone.Width > _PREVIEW_MAX_SOURCE_DIM || fullClone.Height > _PREVIEW_MAX_SOURCE_DIM) {
          var ratio = (double)_PREVIEW_MAX_SOURCE_DIM / Math.Max(fullClone.Width, fullClone.Height);
          previewSourceOwned = new Bitmap((int)(fullClone.Width * ratio), (int)(fullClone.Height * ratio), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
          using (var g = Graphics.FromImage(previewSourceOwned)) {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawImage(fullClone, 0, 0, previewSourceOwned.Width, previewSourceOwned.Height);
          }
          // Scale the requested target dims by the same ratio so the preview preserves aspect.
          previewTargetWidth = (word)Math.Max(1, Math.Round(targetWidth * ratio));
          previewTargetHeight = (word)Math.Max(1, Math.Round(targetHeight * ratio));
          sizeNote = " — downsized from " + fullClone.Width + "×" + fullClone.Height;
          fullClone.Dispose();
        } else {
          previewSourceOwned = fullClone;
        }
      } catch (Exception ex) {
        var msg = (ex.GetType().Name + ": " + ex.Message).Replace('\n', ' ').Replace('\r', ' ');
        if (msg.Length > 140) msg = msg.Substring(0, 137) + "…";
        this.iwhTargetImage.StatusText = "Preview — " + msg;
        return;
      }

      // Guard 2: refuse previews whose target would still be obviously too big.
      const long pixelBudget = 16_000_000L; // ~64 MB ARGB
      if ((long)previewTargetWidth * previewTargetHeight > pixelBudget) {
        previewSourceOwned.Dispose();
        this.iwhTargetImage.StatusText = "Preview skipped — target " + previewTargetWidth + "×" + previewTargetHeight + " too large";
        return;
      }

      this.iwhTargetImage.StatusText = "Preview — rendering…";

      Task.Run(() => {
        Bitmap previewSource = previewSourceOwned;
        try {
          var command = new ResizeCommand(false, method, previewTargetWidth, previewTargetHeight, 0, maintainAspect, horizontalBph, verticalBph, repetitionCount, useThresholds, useCenteredGrid, radius, capturedCanvasColor) {
            SourceImage = previewSource,
            TargetImage = null,
          };
          var sw = Stopwatch.StartNew();
          command.Execute();
          sw.Stop();
          if (token.IsCancellationRequested) {
            command.TargetImage?.Dispose();
            return;
          }
          var bmp = command.TargetImage;
          var note = sizeNote;
          this.SafelyInvoke(() => {
            if (token.IsCancellationRequested) { bmp?.Dispose(); return; }
            // Only dispose what we previously allocated — never touch engine-owned
            // bitmaps that might still be referenced by ScriptEngine.GdiTarget.
            var previouslyOwned = this._previewOwnedTarget;
            this._previewOwnedTarget = bmp;
            // Zoom so the full preview is visible regardless of how much larger than the panel
            // it is; explicit Apply restores the user's configured SizeMode.
            this.iwhTargetImage.SizeMode = PictureBoxSizeMode.Zoom;
            this.iwhTargetImage.Image = bmp;
            previouslyOwned?.Dispose();
            this.iwhTargetImage.StatusText = "Preview (" + sw.ElapsedMilliseconds + " ms)" + (note ?? string.Empty);
          });
        } catch (OutOfMemoryException) {
          this.SafelyInvoke(() => { this.iwhTargetImage.StatusText = "Preview — out of memory (source or target too large)"; });
        } catch (Exception ex) {
          var msg = (ex.GetType().Name + ": " + ex.Message).Replace('\n', ' ').Replace('\r', ' ');
          if (msg.Length > 140) msg = msg.Substring(0, 137) + "…";
          this.SafelyInvoke(() => { this.iwhTargetImage.StatusText = "Preview — " + msg; });
        } finally {
          // We always own previewSource (cloned on the UI thread before Task.Run); release whether
          // the render succeeded, errored, or was cancelled. The engine-owned source bitmap was
          // never touched by this worker.
          previewSource.Dispose();
        }
      }, token);
    }

    /// <summary>
    /// Loads the image from the given filename into the GUI.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    private void _LoadImageFromFileName(string fileName) {
      try {
        // Detach both PictureBoxes from the previous engine-owned bitmaps before LoadFileCommand
        // runs — its commit will dispose the previous source and target, and a still-attached
        // PictureBox would crash mid-paint via ImageAnimator.CanAnimate(FrameDimensionsList).
        this.iwhSourceImage.Image = null;
        this._TargetImage = null;
        var scriptEngine = this._scriptEngine;
        scriptEngine.ExecuteAction(new LoadFileCommand(fileName));
        this._SourceImage = scriptEngine.GdiSource;
        this._lastSaveFileName = null;
        // Fresh image → kick off the auto-preview so the target pane reflects the
        // currently selected method without waiting for the user to touch anything.
        this._SchedulePreview();
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
    /// Determines whether or not the given file extension is usable for the program.
    /// </summary>
    /// <param name="extension">The extension.</param>
    /// <returns><c>true</c> if we accept this file extensions; otherwise, <c>false</c>.</returns>
    private static bool _IsSupportedFileExtension(string extension) {
      if (string.IsNullOrWhiteSpace(extension))
        return false;
      extension = extension.Trim().ToUpper();
      if (extension == ".JPEG" || extension == ".JPG")
        return true;
      if (extension == ".BMP")
        return true;
      if (extension == ".PNG")
        return true;
      if (extension == ".GIF")
        return true;
      if (extension == ".TIF" || extension == ".TIFF")
        return true;
      return false;
    }

    /// <summary>
    /// Gets all supported file names from a Drag'N'Drop operation.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
    /// <returns>The list of files which could be accepted.</returns>
    private static string[] _GetSupportedFiles(DragEventArgs e) {
      var files = e == null ? null : ((Array)e.Data.GetData(DataFormats.FileDrop)).OfType<string>().ToArray();
      if (files == null || files.Length < 1)
        return null;
      return files.Where(f => _IsSupportedFileExtension(Path.GetExtension(f)) || string.Equals(ScriptSerializer.DEFAULT_FILE_EXTENSION, Path.GetExtension(f))).ToArray();
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
