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
using System.Collections.Generic;
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

    /// <summary>
    /// Narrows <see cref="cmbResizeMethod"/> to one manipulator category. Created in code by
    /// <see cref="_CreateCategoryFilter"/>.
    /// </summary>
    private ComboBox _cmbCategory;

    /// <summary>
    /// Sets the target size as a percentage of the source. Created in code by
    /// <see cref="_CreatePercentageControl"/>.
    /// </summary>
    private NumericUpDown _nudPercentage;

    /// <summary>
    /// Guards the percentage control and the width/height boxes against driving each other in a
    /// loop while one is being updated from the other.
    /// </summary>
    private bool _isApplyingPercentage;

    /// <summary>
    /// Toggles the <c>.irs</c> file association. Checked while scripts open with this build.
    /// </summary>
    private ToolStripMenuItem _associateScriptsItem;
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

      this._CreateCategoryFilter();
      this._CreatePercentageControl();

      this.cmbHorizontalBPH.DataSource = Enum.GetValues(typeof(OutOfBoundsMode));
      this.cmbVerticalBPH.DataSource = Enum.GetValues(typeof(OutOfBoundsMode));

      this._SourceImage = null;

      this.sfdSave.InitialDirectory =
        this.ofdOpenFile.InitialDirectory =
          Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

      this.chkUseThresholds.Checked = false;

      this._LoadConfigurationSettings();
      this.FormClosing += (s, e) => this._SaveConfigurationSettings();

      // Must exist before the initial load: _SchedulePreview() no-ops while the timer is
      // null, so constructing it later left a command-line file sitting without a preview.
      // Preview-debounce timer — 300 ms after the last parameter change fires a preview render.
      this._previewDebounce = new System.Windows.Forms.Timer { Interval = 300 };
      this._previewDebounce.Tick += this._OnPreviewDebounceTick;

      if (fileToOpenOnStart != null)
        this._LoadImageFromFileName(fileToOpenOnStart);

      // "Tools" menu added in code to avoid touching the generated Designer.cs. Inserted
      // left of "Help" — that's the conventional place for a Tools menu (File/Edit/View/
      // ...tools... /Window/Help across Windows apps).
      var toolsMenu = new ToolStripMenuItem("&Tools");
      var reduceColorsItem = new ToolStripMenuItem("&Reduce Colours…", null, this._OnReduceColoursClicked);
      toolsMenu.DropDownItems.Add(reduceColorsItem);
      toolsMenu.DropDownItems.Add(new ToolStripSeparator());
      this._associateScriptsItem = new ToolStripMenuItem("&Associate " + ScriptSerializer.DEFAULT_FILE_EXTENSION + " scripts", null, this._OnAssociateScriptsClicked);
      toolsMenu.DropDownItems.Add(this._associateScriptsItem);
      toolsMenu.DropDownOpening += (s, e) => this._RefreshScriptAssociationState();
      var helpIndex = this.msMain.Items.IndexOf(this.helpToolStripMenuItem);
      if (helpIndex >= 0)
        this.msMain.Items.Insert(helpIndex, toolsMenu);
      else
        this.msMain.Items.Add(toolsMenu);

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
    /// Adds the category selector above the method dropdown.
    /// <para>
    /// The registry holds several hundred methods, which is more than a single dropdown can
    /// present usefully; picking a category narrows it to one kind of algorithm. Built in code
    /// rather than in the Designer, the same way the Tools menu is, so the generated file stays
    /// untouched.
    /// </para>
    /// </summary>
    private void _CreateCategoryFilter() {
      var group = this.cmbResizeMethod.Parent;
      if (group == null)
        return;

      this._cmbCategory = new ComboBox {
        Name = "cmbCategory",
        DropDownStyle = ComboBoxStyle.DropDownList,
        Location = this.cmbResizeMethod.Location,
        Size = this.cmbResizeMethod.Size,
        Anchor = this.cmbResizeMethod.Anchor,
        TabIndex = this.cmbResizeMethod.TabIndex,
      };

      // make room: everything from the method dropdown down moves out of the way
      var offset = this._cmbCategory.Height + 6;
      foreach (Control control in group.Controls)
        if (control.Top >= this.cmbResizeMethod.Top)
          control.Top += offset;

      group.Height += offset;
      group.Controls.Add(this._cmbCategory);

      this._cmbCategory.Items.AddRange(ManipulatorCategories.List(SupportedManipulators.MANIPULATORS));
      this._cmbCategory.SelectedIndex = 0;
      this._cmbCategory.SelectedIndexChanged += this._OnCategoryChanged;
    }

    /// <summary>
    /// Narrows the method dropdown to the chosen category, keeping the current method selected
    /// when it is still on offer.
    /// </summary>
    private void _OnCategoryChanged(object sender, EventArgs e) {
      var previous = this.cmbResizeMethod.SelectedValue as IImageManipulator;
      var methods = ManipulatorCategories.Filter(SupportedManipulators.MANIPULATORS, this._cmbCategory.SelectedItem as string);
      if (methods.Length < 1)
        return;

      this.cmbResizeMethod.DataSource = methods;

      var index = ManipulatorCategories.IndexOf(methods, previous);
      this.cmbResizeMethod.SelectedIndex = index < 0 ? 0 : index;
    }

    /// <summary>
    /// Adds the percentage control to the target resolution group.
    /// <para>
    /// The fixed 2x..10x buttons only cover whole factors; a percentage covers everything between
    /// and is what the command line has always accepted as <c>/resize &lt;p&gt;%</c>.
    /// </para>
    /// </summary>
    private void _CreatePercentageControl() {
      var group = this.nudWidth.Parent;
      if (group == null)
        return;

      // The height row is the template for the new one. Its label sits a few pixels lower than
      // its box, so both offsets are copied rather than guessed.
      var heightLabel = group.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Height", StringComparison.OrdinalIgnoreCase));
      var rowHeight = this.nudHeight.Height + 6;
      var newRowTop = this.nudHeight.Top + rowHeight;

      this._nudPercentage = new NumericUpDown {
        Name = "nudPercentage",
        Location = new Point(this.nudHeight.Left, newRowTop),
        Size = this.nudHeight.Size,
        Anchor = this.nudHeight.Anchor,
        Minimum = 1,
        Maximum = 6400,
        Value = 100,
        Increment = 25,
      };

      // everything strictly below the height row moves down to make space - the height label
      // itself sits below the box's top, so the boundary is the box's bottom edge
      var heightRowBottom = this.nudHeight.Top + this.nudHeight.Height;
      foreach (Control control in group.Controls)
        if (control.Top >= heightRowBottom)
          control.Top += rowHeight;

      group.Height += rowHeight;
      group.Controls.Add(this._nudPercentage);

      if (heightLabel != null)
        group.Controls.Add(new Label {
          Name = "lblPercentage",
          Text = "Percent",
          Location = new Point(heightLabel.Left, heightLabel.Top + rowHeight),
          Anchor = heightLabel.Anchor,
          TextAlign = heightLabel.TextAlign,
          // "Percent" is wider than the label this row was copied from
          AutoSize = true,
        });

      this._nudPercentage.ValueChanged += this._OnPercentageChanged;
    }

    /// <summary>
    /// Applies the percentage to the width and height boxes.
    /// </summary>
    private void _OnPercentageChanged(object sender, EventArgs e) {
      if (this._isApplyingPercentage)
        return;

      var source = this._scriptEngine.SourceImage;
      if (source == null)
        return;

      TargetDimensions.FromPercentage(
        source.Width,
        source.Height,
        (double)this._nudPercentage.Value,
        out var width,
        out var height,
        (int)this.nudWidth.Maximum
      );

      this._isApplyingPercentage = true;
      try {
        this.nudWidth.Value = width;
        this.nudHeight.Value = height;
      } finally {
        this._isApplyingPercentage = false;
      }
    }

    /// <summary>
    /// Follows the width box with the percentage control, so the two never disagree about what
    /// the target size is.
    /// </summary>
    private void _SyncPercentageFromWidth() {
      if (this._isApplyingPercentage || this._nudPercentage == null)
        return;

      var source = this._scriptEngine.SourceImage;
      if (source == null)
        return;

      var percentage = (decimal)TargetDimensions.ToPercentage(source.Width, (int)this.nudWidth.Value);
      if (percentage < this._nudPercentage.Minimum || percentage > this._nudPercentage.Maximum)
        return;

      this._isApplyingPercentage = true;
      try {
        this._nudPercentage.Value = decimal.Round(percentage, 0);
      } finally {
        this._isApplyingPercentage = false;
      }
    }

    /// <summary>
    /// Ticks the menu item when scripts currently open with this build.
    /// </summary>
    private void _RefreshScriptAssociationState() {
      try {
        using (var classes = ScriptFileAssociation.OpenUserClasses())
          this._associateScriptsItem.Checked = ScriptFileAssociation.IsRegistered(classes, Application.ExecutablePath);
      } catch (Exception) {
        // a registry we cannot read just means we cannot show the state
        this._associateScriptsItem.Checked = false;
      }
    }

    /// <summary>
    /// Associates or disassociates <c>.irs</c> scripts with this build.
    /// </summary>
    private void _OnAssociateScriptsClicked(object sender, EventArgs e) {
      try {
        using (var classes = ScriptFileAssociation.OpenUserClasses()) {
          var executable = Application.ExecutablePath;
          if (ScriptFileAssociation.IsRegistered(classes, executable))
            ScriptFileAssociation.Unregister(classes);
          else
            ScriptFileAssociation.Register(classes, executable);
        }

        ScriptFileAssociation.NotifyShell();
        this._RefreshScriptAssociationState();
      } catch (Exception exception) {
        MessageBox.Show(
          "The file association could not be changed." + Environment.NewLine + exception.Message,
          "Associate scripts",
          MessageBoxButtons.OK,
          MessageBoxIcon.Warning
        );
      }
    }

    /// <summary>
    /// Loads and applies the configuration settings.
    /// </summary>
    private void _LoadConfigurationSettings() {
      if (Config.SourceSizeMode != null)
        this._SourceImageSizeMode = Config.SourceSizeMode.Value;

      if (Config.TargetSizeMode != null)
        this._TargetImageSizeMode = Config.TargetSizeMode.Value;

      this._RestoreWindowPlacement();

      // category before method: narrowing the list would otherwise drop the restored selection
      if (Config.MethodCategory != null && this._cmbCategory != null && this._cmbCategory.Items.Contains(Config.MethodCategory))
        this._cmbCategory.SelectedItem = Config.MethodCategory;

      if (Config.ResizeMethod != null)
        this._SelectMethodByName(Config.ResizeMethod);

      if (Config.HorizontalBph != null)
        this.cmbHorizontalBPH.SelectedItem = Config.HorizontalBph.Value;

      if (Config.VerticalBph != null)
        this.cmbVerticalBPH.SelectedItem = Config.VerticalBph.Value;

      if (Config.UseThresholds != null)
        this.chkUseThresholds.Checked = Config.UseThresholds.Value;

      if (Config.UseCenteredGrid != null)
        this.chkUseCenteredGrid.Checked = Config.UseCenteredGrid.Value;

      if (Config.KeepAspect != null)
        this.chkKeepAspect.Checked = Config.KeepAspect.Value;

      if (Config.RepetitionCount != null)
        this.nudRepetitionCount.Value = _Clamp(Config.RepetitionCount.Value, this.nudRepetitionCount);

      if (Config.Radius != null)
        this.nudRadius.Value = _Clamp((decimal)Config.Radius.Value, this.nudRadius);
    }

    /// <summary>
    /// Captures everything worth remembering into <see cref="Config"/>. Runs while the window is
    /// closing, before <c>Program</c> writes the file.
    /// </summary>
    private void _SaveConfigurationSettings() {
      // Maximized and minimized bounds describe the current state, not the size to come back to
      Config.WindowBounds = this.WindowState == FormWindowState.Normal ? this.Bounds : this.RestoreBounds;
      Config.WindowState = this.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : this.WindowState;

      Config.MethodCategory = this._cmbCategory?.SelectedItem as string;
      Config.ResizeMethod = this._SelectedMethodName();
      Config.HorizontalBph = this.cmbHorizontalBPH.SelectedItem as OutOfBoundsMode?;
      Config.VerticalBph = this.cmbVerticalBPH.SelectedItem as OutOfBoundsMode?;
      Config.UseThresholds = this.chkUseThresholds.Checked;
      Config.UseCenteredGrid = this.chkUseCenteredGrid.Checked;
      Config.KeepAspect = this.chkKeepAspect.Checked;
      Config.RepetitionCount = (int)this.nudRepetitionCount.Value;
      Config.Radius = (float)this.nudRadius.Value;
    }

    /// <summary>
    /// Puts the window back where it was, unless that is off every screen - a monitor that is no
    /// longer attached would otherwise strand the window out of reach.
    /// </summary>
    private void _RestoreWindowPlacement() {
      var bounds = Config.WindowBounds;
      if (bounds != null && Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(bounds.Value))) {
        this.StartPosition = FormStartPosition.Manual;
        this.Bounds = bounds.Value;
      }

      if (Config.WindowState != null && Config.WindowState.Value != FormWindowState.Minimized)
        this.WindowState = Config.WindowState.Value;
    }

    /// <summary>
    /// Gets the registered name of the selected method.
    /// </summary>
    private string _SelectedMethodName() {
      var selected = this.cmbResizeMethod.SelectedValue as IImageManipulator;
      var index = ManipulatorCategories.IndexOf(SupportedManipulators.MANIPULATORS, selected);
      return index < 0 ? null : SupportedManipulators.MANIPULATORS[index].Key;
    }

    /// <summary>
    /// Selects a method by its registered name, if the current list still offers it.
    /// </summary>
    /// <param name="name">The registered name.</param>
    private void _SelectMethodByName(string name) {
      if (!(this.cmbResizeMethod.DataSource is KeyValuePair<string, IImageManipulator>[] methods))
        return;

      for (var i = 0; i < methods.Length; ++i)
        if (string.Equals(methods[i].Key, name, StringComparison.OrdinalIgnoreCase)) {
          this.cmbResizeMethod.SelectedIndex = i;
          return;
        }
    }

    /// <summary>
    /// Keeps a remembered value inside what its control accepts - the limits can change between
    /// builds, and an out-of-range assignment throws.
    /// </summary>
    private static decimal _Clamp(decimal value, NumericUpDown control)
      => value < control.Minimum ? control.Minimum : value > control.Maximum ? control.Maximum : value
    ;

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
