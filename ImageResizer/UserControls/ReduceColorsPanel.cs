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
 */
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Classes;
using Hawkynt.ColorProcessing.Dithering;
using Hawkynt.ColorProcessing.Quantization;

namespace ImageResizer.UserControls {
  /// <summary>
  /// Sidebar panel for palette reduction. Layout top-to-bottom:
  /// palette-size slider → quantizer strip → ditherer strip (populated after quant pick) →
  /// preview-size combo → Apply. Dispatches the chosen combo via the <see cref="ApplyRequested"/>
  /// event; parent forms convert that into a <c>ReduceColorsCommand</c>.
  /// </summary>
  internal sealed class ReduceColorsPanel : UserControl {

    private readonly ThumbnailService _thumbs = new ThumbnailService();
    private readonly TrackBar _paletteSlider;
    private readonly Label _paletteLabel;
    private readonly FlowLayoutPanel _quantStrip;
    private readonly FlowLayoutPanel _ditherStrip;
    private readonly ComboBox _previewSizeCombo;
    private readonly Button _applyButton;
    private readonly Label _stageHint;
    private readonly _ZoomableView _detailPreview;
    private readonly Label _detailStatus;
    private readonly Label _statsLabel;
    private readonly _HistogramView _histogramView;

    private readonly Dictionary<ThumbnailKey, Tile> _tilesByKey = new Dictionary<ThumbnailKey, Tile>();

    private QuantizerDescriptor _pickedQuantizer;
    private DithererDescriptor _pickedDitherer;

    private System.Threading.CancellationTokenSource _detailCts;

    // Debounce palette-size slider: TrackBar.ValueChanged fires on every tick while dragging,
    // but rebuilding the quant strip + ~20 thumbnails per tick is both slow and races the
    // in-flight renders from the previous value. The timer collapses a burst of ticks into
    // one rebuild 250 ms after the user settles.
    private System.Windows.Forms.Timer _paletteDebounce;

    public event EventHandler<ApplyRequestedArgs> ApplyRequested;

    public ReduceColorsPanel() {
      this.Dock = DockStyle.Fill;
      this.AutoScroll = false;

      // Outer 2-pane split: left = quant/dither controls (original stack),
      // right = zoomable detail preview. SplitContainer gives the user a draggable splitter
      // and a fixed-Panel1 resize policy so the right detail pane absorbs window resizes.
      // Note: do NOT set Panel{1,2}MinSize here — the SplitContainer's default Width is 150,
      // and setting MinSize on a freshly-constructed control triggers an internal
      // SplitterDistance clamp that throws "SplitterDistance must be between Panel1MinSize
      // and Width - Panel2MinSize" because the constraint is unsatisfiable at Width=150.
      // Final MinSize + SplitterDistance are applied in the HandleCreated handler below,
      // by which point the SplitContainer is parented and has its real Width.
      var outer = new SplitContainer {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Vertical,
        FixedPanel = FixedPanel.Panel1,
        SplitterWidth = 6,
      };

      var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 10, Padding = new Padding(4) };
      root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 0 stats label
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 1 histogram
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 2 palette label
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 3 palette slider
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 4 stage hint
      root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));  // 5 quant strip
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 6 dither hint
      root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));  // 7 dither strip
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 8 preview size row
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // 9 apply button

      // 0/1 — source stats + R/G/B histogram. Both fill on SetSource.
      this._statsLabel = new Label { Text = "Source: —", Dock = DockStyle.Top, AutoSize = true };
      this._histogramView = new _HistogramView { Dock = DockStyle.Top, Height = 80, BorderStyle = BorderStyle.FixedSingle };
      root.Controls.Add(this._statsLabel, 0, 0);
      root.Controls.Add(this._histogramView, 0, 1);

      // 2/3 — palette size
      this._paletteLabel = new Label { Text = "Palette size: 256 colours", Dock = DockStyle.Top, AutoSize = true };
      this._paletteSlider = new TrackBar { Dock = DockStyle.Top, Minimum = 2, Maximum = 256, Value = 256, TickFrequency = 16, LargeChange = 16, SmallChange = 2 };
      this._paletteSlider.ValueChanged += this._OnPaletteChanged;
      root.Controls.Add(this._paletteLabel, 0, 2);
      root.Controls.Add(this._paletteSlider, 0, 3);

      // 4 — stage hint
      this._stageHint = new Label { Text = "Pick a quantizer below:", Dock = DockStyle.Top, AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
      root.Controls.Add(this._stageHint, 0, 4);

      // 5 — quant strip (with its own scroll)
      this._quantStrip = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight, BorderStyle = BorderStyle.FixedSingle };
      this._quantStrip.Scroll += (s, e) => this._UpdatePriorities();
      root.Controls.Add(this._quantStrip, 0, 5);

      // 6 — "after quant → ditherer" hint
      root.Controls.Add(new Label { Text = "Then pick a ditherer:", Dock = DockStyle.Top, AutoSize = true, Font = new Font(this.Font, FontStyle.Bold), Padding = new Padding(0, 6, 0, 0) }, 0, 6);

      // 7 — ditherer strip
      this._ditherStrip = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight, BorderStyle = BorderStyle.FixedSingle };
      this._ditherStrip.Scroll += (s, e) => this._UpdatePriorities();
      root.Controls.Add(this._ditherStrip, 0, 7);

      // 8 — preview size
      var sizeRow = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
      sizeRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      sizeRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
      sizeRow.Controls.Add(new Label { Text = "Thumbnail size:", AutoSize = true, Padding = new Padding(0, 6, 6, 0) }, 0, 0);
      this._previewSizeCombo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
      this._previewSizeCombo.Items.AddRange(new object[] { 64, 96, 128, 256 });
      this._previewSizeCombo.SelectedItem = 96;
      this._previewSizeCombo.SelectedIndexChanged += this._OnPreviewSizeChanged;
      sizeRow.Controls.Add(this._previewSizeCombo, 1, 0);
      root.Controls.Add(sizeRow, 0, 8);

      // 9 — apply
      this._applyButton = new Button { Text = "Apply to target", Dock = DockStyle.Top, Enabled = false, Height = 28 };
      this._applyButton.Click += this._OnApplyClicked;
      root.Controls.Add(this._applyButton, 0, 9);

      // Right column — zoomable detail preview + status line.
      var detailRoot = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(4) };
      detailRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
      detailRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
      detailRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      this._detailPreview = new _ZoomableView { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(250, 250, 250) };
      this._detailStatus = new Label { Dock = DockStyle.Bottom, AutoSize = false, Height = 20, TextAlign = ContentAlignment.MiddleLeft, Font = new Font(this.Font, FontStyle.Italic), ForeColor = Color.DimGray, Text = "Detail preview — pick a quantizer" };
      detailRoot.Controls.Add(this._detailPreview, 0, 0);
      detailRoot.Controls.Add(this._detailStatus, 0, 1);

      outer.Panel1.Controls.Add(root);
      outer.Panel2.Controls.Add(detailRoot);
      this.Controls.Add(outer);
      // Apply MinSize + SplitterDistance once the SplitContainer is parented and has a real
      // Width. Setting MinSize before this point throws because the SplitterDistance clamp
      // can't satisfy [Panel1MinSize, Width - Panel2MinSize] when Width is the default 150.
      outer.HandleCreated += (s, e) => {
        try {
          // Cap MinSize at half of the actual width so neither side can exceed available space.
          var halfWidth = Math.Max(50, outer.Width / 2 - 10);
          outer.Panel1MinSize = Math.Min(360, halfWidth);
          outer.Panel2MinSize = Math.Min(360, halfWidth);
          outer.SplitterDistance = Math.Min(440, outer.Width - outer.Panel2MinSize);
        } catch { /* parent too narrow even for the clamped values; let WinForms keep defaults */ }
      };

      this._thumbs.ThumbnailReady += this._OnThumbnailReady;
      this._thumbs.ThumbnailStarted += this._OnThumbnailStarted;

      this._paletteDebounce = new System.Windows.Forms.Timer { Interval = 250 };
      this._paletteDebounce.Tick += this._OnPaletteDebounceTick;

      // Do NOT populate here — the hosting window always calls SetSource right after
      // constructing us. Populating twice back-to-back lets the second call try to
      // Dispose() tiles from the first while their handles are still being created,
      // which throws InvalidOperationException("Dispose during CreateHandle()").
    }

    /// <summary>Replace the working source. Invalidates all thumbnails and re-renders the quant strip.</summary>
    public void SetSource(Bitmap source) {
      this._thumbs.ReplaceSource(source);
      this._pickedQuantizer = null;
      this._pickedDitherer = null;
      this._applyButton.Enabled = false;
      this._tilesByKey.Clear();
      this._ditherStrip.Controls.Clear();
      this._detailHistogram = null;
      this._cachedPalette = null;
      this._cachedPaletteQuantizer = null;
      // Stats BEFORE populating the quant strip so our LockBits pass doesn't race the
      // thumbnail workers' DrawImage on the same Bitmap (GDI+ isn't thread-safe; concurrent
      // accesses to the same Bitmap instance serialize inside GDI+ and make both paths slow).
      this._RefreshSourceStats(source);
      this._PopulateQuantStrip();
    }

    private void _RefreshSourceStats(Bitmap source) {
      if (source == null) {
        this._statsLabel.Text = "Source: —";
        this._histogramView.SetHistograms(null, null, null);
        return;
      }
      var w = source.Width;
      var h = source.Height;
      var mp = (w * (long)h) / 1_000_000.0;
      // Single LockBits pass — computes exact RGB-distinct count + R/G/B histograms together.
      // On 12 MP source ≈ 80 ms, on 0.2 MP imperceptible. Synchronous to guarantee no GDI+
      // contention with the worker pool.
      var stats = this._thumbs.GetSourceStats();
      this._statsLabel.Text = $"Source: {w} × {h} ({mp:0.0} MP), {stats.DistinctCount:N0} distinct RGB colours";
      this._histogramView.SetHistograms(stats.R, stats.G, stats.B);
    }

    private int _PreviewSize => (int)(this._previewSizeCombo.SelectedItem ?? 96);
    private ushort _PaletteSize => (ushort)this._paletteSlider.Value;

    /// <summary>
    /// Safely tears down the children of a container. Directly calling <c>c.Dispose()</c>
    /// throws <c>InvalidOperationException</c> if the control is mid <c>CreateHandle()</c>
    /// (e.g. panel populated twice back-to-back while the host window is being shown).
    /// Snapshot + Remove + Dispose sidesteps that: Remove detaches the control from the
    /// parent handle-creation chain, then Dispose runs against a free-standing control.
    /// </summary>
    private static void _DisposeChildren(Control container) {
      var snapshot = new Control[container.Controls.Count];
      container.Controls.CopyTo(snapshot, 0);
      container.Controls.Clear();
      foreach (var c in snapshot)
        try { c.Dispose(); } catch { /* if it was already being torn down, leave to GC */ }
    }

    private void _PopulateQuantStrip() {
      this._quantStrip.SuspendLayout();
      _DisposeChildren(this._quantStrip);

      foreach (var q in QuantizerRegistry.All) {
        if (q.DeclaringType.ContainsGenericParameters) continue;
        var tile = new Tile(q.Name);
        tile.Clicked += (s, e) => this._OnQuantizerPicked(q, tile);
        tile.Resize(this._PreviewSize);
        this._quantStrip.Controls.Add(tile);

        var key = new ThumbnailKey(this._thumbs.SourceVersion, q.Name, null, this._PaletteSize, this._PreviewSize);
        this._tilesByKey[key] = tile;
        this._RequestIfNeeded(key, q, null, priorityVisibleInStrip: true);
      }
      this._quantStrip.ResumeLayout();
    }

    private void _PopulateDitherStrip() {
      this._ditherStrip.SuspendLayout();
      _DisposeChildren(this._ditherStrip);

      if (this._pickedQuantizer == null) { this._ditherStrip.ResumeLayout(); return; }

      // First tile = "no dither" (explicit NoDithering), matches the quant-only baseline.
      var noDitherTile = new Tile("(no dither)");
      noDitherTile.Clicked += (s, e) => this._OnDithererPicked(null, noDitherTile);
      noDitherTile.Resize(this._PreviewSize);
      this._ditherStrip.Controls.Add(noDitherTile);
      var noDitherKey = new ThumbnailKey(this._thumbs.SourceVersion, this._pickedQuantizer.Name, null, this._PaletteSize, this._PreviewSize);
      this._tilesByKey[noDitherKey] = noDitherTile;
      this._RequestIfNeeded(noDitherKey, this._pickedQuantizer, null, priorityVisibleInStrip: true);

      foreach (var d in DithererRegistry.All) {
        if (d.DeclaringType.ContainsGenericParameters) continue;
        if (string.Equals(d.DeclaringType.Name, nameof(NoDithering), StringComparison.Ordinal)) continue;
        var tile = new Tile(d.Name);
        tile.Clicked += (s, e) => this._OnDithererPicked(d, tile);
        tile.Resize(this._PreviewSize);
        this._ditherStrip.Controls.Add(tile);
        var key = new ThumbnailKey(this._thumbs.SourceVersion, this._pickedQuantizer.Name, d.Name, this._PaletteSize, this._PreviewSize);
        this._tilesByKey[key] = tile;
        this._RequestIfNeeded(key, this._pickedQuantizer, d, priorityVisibleInStrip: true);
      }
      this._ditherStrip.ResumeLayout();
    }

    private void _RequestIfNeeded(ThumbnailKey key, QuantizerDescriptor q, DithererDescriptor d, bool priorityVisibleInStrip) {
      if (this._thumbs.TryGetCached(key, out var bmp)) {
        if (this._tilesByKey.TryGetValue(key, out var tile) && tile != null && !tile.IsDisposed)
          tile.SetBitmap(bmp);
        return;
      }
      var priority = priorityVisibleInStrip ? 1000 : 100;
      this._thumbs.Request(key, q, d, priority);
    }

    private void _OnPaletteChanged(object sender, EventArgs e) {
      // Live label update — but the expensive rebuild is debounced so dragging the slider
      // doesn't rebuild ~20 thumbnails on every tick and race/crash.
      this._paletteLabel.Text = $"Palette size: {this._paletteSlider.Value} colours";
      if (this._paletteDebounce == null) return;
      this._paletteDebounce.Stop();
      this._paletteDebounce.Start();
    }

    private void _OnPaletteDebounceTick(object sender, EventArgs e) {
      this._paletteDebounce.Stop();
      // invalidate everything (palette-size change invalidates every thumb + palette cache)
      this._tilesByKey.Clear();
      this._cachedPalette = null;
      this._cachedPaletteQuantizer = null;
      this._PopulateQuantStrip();
      this._RefreshDitherStripForCurrentQuant();
    }

    private void _OnPreviewSizeChanged(object sender, EventArgs e) {
      foreach (var t in this._tilesByKey.Values) t?.Resize(this._PreviewSize);
      this._tilesByKey.Clear();
      this._PopulateQuantStrip();
      this._RefreshDitherStripForCurrentQuant();
    }

    /// <summary>
    /// Rebuilds the dither strip for the currently-picked quantizer, or hides it if
    /// the source's distinct-colour count is already ≤ palette size (dithering would
    /// just add noise without suppressing banding that isn't there).
    /// </summary>
    private void _RefreshDitherStripForCurrentQuant() {
      if (this._pickedQuantizer == null) return;
      if (this._thumbs.DistinctColorCount <= this._PaletteSize) {
        _DisposeChildren(this._ditherStrip);
        this._stageHint.Text = $"Quantizer: {this._pickedQuantizer.Name} — palette {this._PaletteSize} ≥ source colours, no dithering needed. Click Apply.";
      } else {
        this._PopulateDitherStrip();
        this._stageHint.Text = $"Quantizer: {this._pickedQuantizer.Name} — pick a ditherer below, or Apply.";
      }
    }

    private void _OnQuantizerPicked(QuantizerDescriptor q, Tile tile) {
      this._pickedQuantizer = q;
      this._pickedDitherer = null;
      foreach (Control c in this._quantStrip.Controls) if (c is Tile qt) qt.SetSelected(qt == tile);

      // Drop dither-strip entries from the queue (priorities stale) and rebuild.
      var sv = this._thumbs.SourceVersion;
      this._thumbs.Invalidate(k => k.SourceVersion == sv && k.DithererName != null);

      // Lossless quantization? Skip dithering entirely — it would just add noise
      // where no banding exists to hide. Compare the source's distinct-colour count
      // (capped/sampled in ThumbnailService) against the requested palette size.
      var distinct = this._thumbs.DistinctColorCount;
      if (distinct <= this._PaletteSize) {
        _DisposeChildren(this._ditherStrip);
        this._stageHint.Text = $"Quantizer: {q.Name} — source has {distinct:N0} colours ≤ palette {this._PaletteSize}, no dithering needed. Click Apply.";
      } else {
        this._PopulateDitherStrip();
        this._stageHint.Text = $"Quantizer: {q.Name} — pick a ditherer below, or Apply.";
      }
      this._applyButton.Enabled = true;
      this._ScheduleDetailRender();
    }

    private void _OnDithererPicked(DithererDescriptor d, Tile tile) {
      this._pickedDitherer = d;
      foreach (Control c in this._ditherStrip.Controls) if (c is Tile dt) dt.SetSelected(dt == tile);
      this._ScheduleDetailRender();
    }

    /// <summary>
    /// Spawns a background render of the currently selected (quant, dither, palette) combo at
    /// up to <c>_DETAIL_SIZE</c>² and hands the result to <see cref="_detailPreview"/>. Cancels
    /// any in-flight detail render.
    /// <para>
    /// Three-tier cache so picks are as cheap as possible:
    /// </para>
    /// <list type="bullet">
    /// <item><c>_detailHistogram</c> — colour histogram of the detail master; invalidated on
    /// source change only.</item>
    /// <item><c>_cachedPalette</c> — palette computed from (<c>histogram</c>, quantizer,
    /// paletteSize); invalidated on source change, quantizer change, or palette-slider change.</item>
    /// <item>Ditherer runs every pick (unavoidable — its output is the preview image).</item>
    /// </list>
    /// So: picking the same quant + different ditherer only re-runs the dither stage.
    /// Picking different quant re-runs palette + dither. Both are cheaper than re-running
    /// the full histogram-build each time.
    /// </summary>
    private const int _DETAIL_SIZE = 1024;
    private Dictionary<int, uint> _detailHistogram;
    private Color[] _cachedPalette;
    private QuantizerDescriptor _cachedPaletteQuantizer;
    private ushort _cachedPalettePaletteSize;

    private void _ScheduleDetailRender() {
      this._detailCts?.Cancel();
      var cts = this._detailCts = new System.Threading.CancellationTokenSource();
      var token = cts.Token;

      var q = this._pickedQuantizer;
      if (q == null) return;
      var d = this._pickedDitherer;
      var palette = this._PaletteSize;

      this._detailStatus.Text = "Detail preview — rendering…";

      System.Threading.Tasks.Task.Run(() => {
        // Let the thumbnail workers (Normal priority, dedicated Threads) win the CPU while
        // the detail render is running — otherwise the user watches the quant strip stall
        // while we crunch the 1024² detail. Reset at the end because this thread will be
        // returned to the ThreadPool and reused elsewhere.
        var prevPriority = System.Threading.Thread.CurrentThread.Priority;
        System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.BelowNormal;

        // Rent a pre-cloned detail master from the pool. No GDI+ contention: each concurrent
        // render (theoretically only 1 at a time post-cancellation, but safely up to 2) has
        // its own Bitmap instance, so LockBits inside ApplyPaletteWithDither can't race.
        var copy = this._thumbs.RentDetailMaster(_DETAIL_SIZE);
        if (copy == null) {
          System.Threading.Thread.CurrentThread.Priority = prevPriority;
          return;
        }
        try {
          if (token.IsCancellationRequested) return;
          var targetW = copy.Width;
          var targetH = copy.Height;
          var sw = System.Diagnostics.Stopwatch.StartNew();

          // Stage 1 — histogram: once per source.
          var hist = System.Threading.Volatile.Read(ref this._detailHistogram);
          if (hist == null) {
            hist = Imager.Pipelines.UpstreamPipeline.ComputeHistogram(copy);
            System.Threading.Volatile.Write(ref this._detailHistogram, hist);
          }
          if (token.IsCancellationRequested) return;

          // Stage 2 — palette: once per (source, quantizer, paletteSize).
          Color[] pal;
          lock (this) {
            if (this._cachedPalette != null
                && ReferenceEquals(this._cachedPaletteQuantizer, q)
                && this._cachedPalettePaletteSize == palette) {
              pal = this._cachedPalette;
            } else {
              pal = Imager.Pipelines.UpstreamPipeline.ComputePalette(hist, q, palette);
              this._cachedPalette = pal;
              this._cachedPaletteQuantizer = q;
              this._cachedPalettePaletteSize = palette;
            }
          }
          if (token.IsCancellationRequested) return;

          // Stage 3 — apply palette + dither: always.
          var result = Imager.Pipelines.UpstreamPipeline.ApplyPaletteWithDither(copy, pal, d);
          sw.Stop();
          if (token.IsCancellationRequested) { result?.Dispose(); return; }
          this.BeginInvoke(new Action(() => {
            if (token.IsCancellationRequested || this.IsDisposed) { result?.Dispose(); return; }
            this._detailPreview.SetImage(result);
            var ditherLabel = d == null ? "no dither" : d.Name;
            this._detailStatus.Text = $"Detail: {q.Name} + {ditherLabel} @ {palette} colours — {targetW}×{targetH} ({sw.ElapsedMilliseconds} ms)";
          }));
        } catch (Exception ex) {
          try {
            this.BeginInvoke(new Action(() => { this._detailStatus.Text = "Detail preview — error: " + ex.Message; }));
          } catch { }
        } finally {
          this._thumbs.ReturnDetailMaster(copy);
          System.Threading.Thread.CurrentThread.Priority = prevPriority;
        }
      }, token);
    }

    private void _OnApplyClicked(object sender, EventArgs e) {
      if (this._pickedQuantizer == null) return;
      this.ApplyRequested?.Invoke(this, new ApplyRequestedArgs(this._pickedQuantizer, this._pickedDitherer, this._PaletteSize));
    }

    private void _OnThumbnailReady(object sender, ThumbnailService.Result r) {
      if (this.InvokeRequired) { try { this.BeginInvoke(new Action(() => this._OnThumbnailReady(sender, r))); } catch { } return; }
      if (this._tilesByKey.TryGetValue(r.Key, out var tile) && tile != null && !tile.IsDisposed)
        tile.SetBitmap(r.Bitmap);
    }

    private void _OnThumbnailStarted(object sender, ThumbnailKey key) {
      if (this.InvokeRequired) { try { this.BeginInvoke(new Action(() => this._OnThumbnailStarted(sender, key))); } catch { } return; }
      if (this._tilesByKey.TryGetValue(key, out var tile) && tile != null && !tile.IsDisposed)
        tile.SetRendering(true);
    }

    private void _UpdatePriorities() {
      // On scroll: visible tiles → high priority, off-screen → lower but not zero.
      var sv = this._thumbs.SourceVersion;
      this._thumbs.Reprioritise(k => {
        if (k.SourceVersion != sv) return 0;
        if (!this._tilesByKey.TryGetValue(k, out var tile) || tile == null || tile.IsDisposed) return 100;
        var host = tile.Parent as ScrollableControl;
        if (host == null) return 100;
        var tileRect = new Rectangle(tile.Location, tile.Size);
        var visibleRect = new Rectangle(host.AutoScrollPosition.X * -1, host.AutoScrollPosition.Y * -1, host.ClientSize.Width, host.ClientSize.Height);
        return tileRect.IntersectsWith(visibleRect) ? 1000 : 100;
      });
    }

    protected override void Dispose(bool disposing) {
      if (disposing) {
        this._paletteDebounce?.Stop();
        this._paletteDebounce?.Dispose();
        this._detailCts?.Cancel();
        this._thumbs.Dispose();
      }
      base.Dispose(disposing);
    }

    /// <summary>Event payload for Apply — parent form turns it into a <c>ReduceColorsCommand</c>.</summary>
    public sealed class ApplyRequestedArgs : EventArgs {
      public QuantizerDescriptor Quantizer { get; }
      public DithererDescriptor Ditherer { get; }
      public ushort PaletteSize { get; }
      public ApplyRequestedArgs(QuantizerDescriptor q, DithererDescriptor d, ushort paletteSize) { this.Quantizer = q; this.Ditherer = d; this.PaletteSize = paletteSize; }
    }

    /// <summary>A single thumbnail tile: square image area + name label + selection border + rendering overlay.</summary>
    private sealed class Tile : Panel {
      private readonly PictureBox _pb = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.FromArgb(240, 240, 240) };
      private readonly Label _label = new Label { Dock = DockStyle.Bottom, AutoSize = false, Height = 18, TextAlign = ContentAlignment.MiddleCenter };
      private readonly Label _status = new Label {
        Text = "rendering…",
        AutoSize = false,
        TextAlign = ContentAlignment.MiddleCenter,
        BackColor = Color.FromArgb(180, 255, 255, 224),
        Font = new Font("Segoe UI", 8F, FontStyle.Italic),
        ForeColor = Color.DimGray,
        Visible = false,
      };
      public event EventHandler Clicked;

      public Tile(string labelText) {
        this._label.Text = labelText;
        this.Margin = new Padding(4);
        this.Padding = new Padding(2);
        this.BorderStyle = BorderStyle.FixedSingle;
        // Add Label FIRST so the dock layout gives it Bottom 18px before _pb's Fill claims
        // the remainder. Reverse order leaves _pb at its preferred 100px and _label fighting
        // for layout space, which manifests as the image rendering small at the top-left.
        this.Controls.Add(this._label);
        this.Controls.Add(this._pb);
        // Status overlay sits on top of the PictureBox (same parent/bounds). Because WinForms
        // Controls don't support alpha, we use a light-tinted opaque label as a visible badge.
        this._pb.Controls.Add(this._status);
        this._status.Dock = DockStyle.Bottom;
        this._status.Height = 16;
        this._pb.Click += (s, e) => this.Clicked?.Invoke(this, EventArgs.Empty);
        this.Click += (s, e) => this.Clicked?.Invoke(this, EventArgs.Empty);
        this._label.Click += (s, e) => this.Clicked?.Invoke(this, EventArgs.Empty);
        this._status.Click += (s, e) => this.Clicked?.Invoke(this, EventArgs.Empty);
      }

      public void SetBitmap(Bitmap b) {
        // Clone protects the tile from the cached bitmap being disposed later (e.g. source
        // replaced, palette-size change invalidating cache entries). But Clone() itself can
        // throw ArgumentException("Invalid parameter") if `b` was disposed between worker
        // cache-write and UI BeginInvoke delivery — skip in that case, tile keeps old image.
        if (b == null) return;
        Bitmap clone;
        try { clone = (Bitmap)b.Clone(); } catch { return; }
        var old = this._pb.Image;
        this._pb.Image = clone;
        old?.Dispose();
        this._status.Visible = false;
      }

      public void SetRendering(bool on) {
        // Only show the overlay while the tile still has no bitmap (or a stale one); once
        // rendering finishes SetBitmap hides it anyway.
        this._status.Visible = on;
      }

      public void SetSelected(bool on) => this.BackColor = on ? SystemColors.Highlight : SystemColors.Control;

      public void Resize(int previewSize) {
        this.Width = previewSize + 8;
        this.Height = previewSize + 28;
        // _pb is Dock=Fill — it auto-takes (TileHeight - LabelHeight - padding). No explicit height.
      }
    }

    /// <summary>
    /// Renders three overlaid 256-bin R/G/B histograms. Each channel is drawn as a semi-
    /// transparent polyline so overlapping bins reveal their combined shape.
    /// </summary>
    private sealed class _HistogramView : Panel {
      private int[] _r, _g, _b;

      public _HistogramView() {
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        this.BackColor = Color.Black;
      }

      public void SetHistograms(int[] r, int[] g, int[] b) {
        this._r = r;
        this._g = g;
        this._b = b;
        this.Invalidate();
      }

      protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        e.Graphics.Clear(Color.Black);
        if (this._r == null || this._g == null || this._b == null) {
          using (var br = new SolidBrush(Color.DimGray))
            e.Graphics.DrawString("(no histogram)", this.Font, br, new PointF(4, 4));
          return;
        }
        var max = 0;
        for (var i = 0; i < 256; ++i) {
          if (this._r[i] > max) max = this._r[i];
          if (this._g[i] > max) max = this._g[i];
          if (this._b[i] > max) max = this._b[i];
        }
        if (max <= 0) return;
        var w = this.ClientSize.Width;
        var h = this.ClientSize.Height - 2;
        using (var rPen = new Pen(Color.FromArgb(180, 255, 80, 80), 1f))
        using (var gPen = new Pen(Color.FromArgb(180, 80, 255, 80), 1f))
        using (var bPen = new Pen(Color.FromArgb(180, 80, 160, 255), 1f)) {
          e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
          _DrawPolyline(e.Graphics, this._r, max, w, h, rPen);
          _DrawPolyline(e.Graphics, this._g, max, w, h, gPen);
          _DrawPolyline(e.Graphics, this._b, max, w, h, bPen);
        }
      }

      private static void _DrawPolyline(Graphics g, int[] hist, int max, int width, int height, Pen pen) {
        var pts = new PointF[256];
        for (var i = 0; i < 256; ++i) {
          var x = i * (width - 1) / 255f;
          var y = height - 1 - (hist[i] * (height - 1) / (float)max);
          pts[i] = new PointF(x, y);
        }
        g.DrawLines(pen, pts);
      }
    }

    /// <summary>
    /// Image display with mouse-drag pan and mousewheel zoom. On <see cref="SetImage"/> the
    /// new image replaces the current one and the view auto-fits to the panel. Subsequent
    /// wheel/drag interactions preserve the user's current zoom + pan.
    /// </summary>
    private sealed class _ZoomableView : Panel {
      private Bitmap _image;
      private float _zoom = 1.0f;
      private PointF _imagePos; // top-left of image in panel coordinates
      private Point _dragStart;
      private PointF _dragImageStart;
      private bool _dragging;
      // Set when SetImage runs before the panel has a valid ClientSize. Cleared by _FitToView
      // once the panel is sized (typically the first OnPaint after the layout settles).
      private bool _needsFit;

      public _ZoomableView() {
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        this.Cursor = Cursors.Hand;
      }

      public void SetImage(Bitmap img) {
        var old = this._image;
        this._image = img;
        old?.Dispose();
        this._needsFit = true;
        this._FitToView();
        this.Invalidate();
      }

      private void _FitToView() {
        if (this._image == null || this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
          return;
        var sx = (float)this.ClientSize.Width / this._image.Width;
        var sy = (float)this.ClientSize.Height / this._image.Height;
        this._zoom = Math.Min(sx, sy);
        var displayW = this._image.Width * this._zoom;
        var displayH = this._image.Height * this._zoom;
        this._imagePos = new PointF((this.ClientSize.Width - displayW) / 2f, (this.ClientSize.Height - displayH) / 2f);
        this._needsFit = false;
      }

      protected override void OnResize(EventArgs e) {
        base.OnResize(e);
        this._FitToView();
        this.Invalidate();
      }

      protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        if (this._image == null) {
          using (var br = new SolidBrush(Color.DimGray))
            e.Graphics.DrawString("(no detail preview yet)", this.Font, br, new PointF(10, 10));
          return;
        }
        // Defer-fit recovery: SetImage is sometimes invoked before the panel's layout
        // has settled (ClientSize == 0,0). _FitToView early-returns in that state, leaving
        // _zoom = 1.0f and _imagePos = (0,0) — image renders at native size top-left.
        // First paint with valid ClientSize is the right moment to retry the fit.
        if (this._needsFit) this._FitToView();
        e.Graphics.InterpolationMode = this._zoom >= 2.0f
          ? System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor  // show actual pixels when zoomed in
          : System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        var dest = new RectangleF(this._imagePos.X, this._imagePos.Y, this._image.Width * this._zoom, this._image.Height * this._zoom);
        e.Graphics.DrawImage(this._image, dest, new RectangleF(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
      }

      protected override void OnMouseDown(MouseEventArgs e) {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left || this._image == null) return;
        this._dragging = true;
        this._dragStart = e.Location;
        this._dragImageStart = this._imagePos;
        this.Cursor = Cursors.SizeAll;
      }

      protected override void OnMouseMove(MouseEventArgs e) {
        base.OnMouseMove(e);
        if (!this._dragging) return;
        var dx = e.X - this._dragStart.X;
        var dy = e.Y - this._dragStart.Y;
        this._imagePos = new PointF(this._dragImageStart.X + dx, this._dragImageStart.Y + dy);
        this.Invalidate();
      }

      protected override void OnMouseUp(MouseEventArgs e) {
        base.OnMouseUp(e);
        this._dragging = false;
        this.Cursor = Cursors.Hand;
      }

      protected override void OnMouseWheel(MouseEventArgs e) {
        base.OnMouseWheel(e);
        if (this._image == null) return;
        // Zoom centred on the cursor so the user sees the region they're pointing at.
        var oldZoom = this._zoom;
        var factor = e.Delta > 0 ? 1.25f : 1f / 1.25f;
        var newZoom = Math.Max(0.05f, Math.Min(32f, oldZoom * factor));
        if (Math.Abs(newZoom - oldZoom) < 0.0001f) return;
        var mouseImgX = (e.X - this._imagePos.X) / oldZoom;
        var mouseImgY = (e.Y - this._imagePos.Y) / oldZoom;
        this._zoom = newZoom;
        this._imagePos = new PointF(e.X - mouseImgX * newZoom, e.Y - mouseImgY * newZoom);
        this.Invalidate();
      }

      protected override bool IsInputKey(Keys keyData) => true;

      protected override void Dispose(bool disposing) {
        if (disposing) this._image?.Dispose();
        base.Dispose(disposing);
      }
    }
  }
}
