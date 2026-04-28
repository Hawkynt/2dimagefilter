#region (c)2008-2026 Hawkynt
/*
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
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;

namespace Classes {
  /// <summary>
  /// Holds the source/target <see cref="Bitmap"/> pair plus the executed-action history. After
  /// the M5 migration these are the authoritative bitmap fields — there is no separate cImage
  /// representation any more, and no GDI cache because the engine already speaks GDI+.
  /// Setters dispose the previous bitmap so each transfer point has clear ownership.
  /// <para>
  /// Track-C addition: when a source bitmap is owned by a <see cref="BitmapMasterPool"/> the
  /// engine holds a non-owning reference instead. <see cref="SetSourceImageNonOwning"/> records
  /// the pool key and tells the setter not to dispose. <see cref="CurrentSourceKey"/> exposes
  /// the key so other code can ask "what's the pool key for the current source?".
  /// </para>
  /// </summary>
  internal class ScriptEngine {

    private Bitmap _sourceImage;
    private Bitmap _targetImage;

    /// <summary>
    /// True while the current <see cref="_sourceImage"/> is owned by <see cref="MasterPool"/>;
    /// the engine must not dispose it on setter replace. Reset to false whenever an action's
    /// commit re-assigns <see cref="SourceImage"/> through the regular owning setter.
    /// </summary>
    private bool _sourceIsPoolManaged;

    /// <summary>
    /// Pool key for the current source, when <see cref="_sourceIsPoolManaged"/> is true.
    /// <c>null</c> means "current source is engine-owned".
    /// </summary>
    private string _currentSourceKey;

    /// <summary>
    /// Optional <see cref="BitmapMasterPool"/> the engine routes file loads through. Set by
    /// <see cref="ImageResizer.MainForm"/> at construction; <c>null</c> in CLI / headless paths
    /// where pooling adds no value (single-shot pipelines that never re-display the source).
    /// </summary>
    public BitmapMasterPool MasterPool { get; set; }

    /// <summary>
    /// Pool key for the source bitmap currently held by the engine, or <c>null</c> when the
    /// source is engine-owned. Used by the preview path to request a clone via
    /// <see cref="BitmapMasterPool.CheckoutClone"/> instead of cloning the engine's bitmap on
    /// the worker thread (which races <c>PictureBox.OnPaint</c>).
    /// </summary>
    public string CurrentSourceKey => this._sourceIsPoolManaged ? this._currentSourceKey : null;

    /// <summary>
    /// Gets or sets the source image. Setter disposes the previous bitmap — the engine owns it.
    /// </summary>
    public Bitmap SourceImage {
      get => this._sourceImage;
      private set {
        var previous = this._sourceImage;
        var previousWasPoolManaged = this._sourceIsPoolManaged;
        this._sourceImage = value;
        // The new value comes through the legacy owning path; clear the non-owning flags.
        this._sourceIsPoolManaged = false;
        this._currentSourceKey = null;
        // Only dispose when the previous source was engine-owned — pool-managed bitmaps belong
        // to the pool and disposing one mid-operation corrupts every subsequent CheckoutClone.
        if (!previousWasPoolManaged && !ReferenceEquals(previous, value))
          previous?.Dispose();
      }
    }

    /// <summary>
    /// Replaces <see cref="SourceImage"/> with a pool-managed bitmap. The engine holds a
    /// non-owning reference; the bitmap is owned by <see cref="MasterPool"/> and must not be
    /// disposed by the engine. <paramref name="poolKey"/> is the key the pool returned (file
    /// path for <see cref="BitmapMasterPool.LoadOrGet"/>, opaque token for
    /// <see cref="BitmapMasterPool.InsertSynthetic"/>); it's surfaced through
    /// <see cref="CurrentSourceKey"/> so callers can later request clones from the pool.
    /// </summary>
    public void SetSourceImageNonOwning(Bitmap bitmap, string poolKey) {
      Contract.Requires(bitmap != null);
      Contract.Requires(!string.IsNullOrWhiteSpace(poolKey));

      var previous = this._sourceImage;
      var previousWasPoolManaged = this._sourceIsPoolManaged;
      this._sourceImage = bitmap;
      this._sourceIsPoolManaged = true;
      this._currentSourceKey = poolKey;
      // Dispose the previous bitmap only if the engine owned it; pool-managed bitmaps stay alive.
      if (!previousWasPoolManaged && !ReferenceEquals(previous, bitmap))
        previous?.Dispose();
    }

    /// <summary>
    /// Gets or sets the target image. Setter disposes the previous bitmap — the engine owns it.
    /// </summary>
    public Bitmap TargetImage {
      get => this._targetImage;
      private set {
        var previous = this._targetImage;
        this._targetImage = value;
        if (!ReferenceEquals(previous, value))
          previous?.Dispose();
      }
    }

    /// <summary>
    /// Backwards-compat alias for the source bitmap. Pre-M5 the engine cached a separate GDI+
    /// rendering of the cImage source pane; now SourceImage *is* the bitmap and this alias just
    /// returns it. Kept so MainForm / EventHandlers / CLI continue to compile.
    /// </summary>
    public Bitmap GdiSource => this._sourceImage;

    /// <summary>
    /// Backwards-compat alias for the target bitmap. See <see cref="GdiSource"/>.
    /// </summary>
    public Bitmap GdiTarget => this._targetImage;

    private readonly List<IScriptAction> _actionList = new List<IScriptAction>();

    public bool IsSourceImageChanged { get; private set; }

    public bool IsTargetImageChanged { get; private set; }

    public void Clear() => this._actionList.Clear();

    /// <summary>
    /// Note: We're returning an enumeration so our list stays safe and is not modified by another class.
    /// </summary>
    public IEnumerable<IScriptAction> Actions => this._actionList.Select(t => t);

    public void ExecuteAction(IScriptAction action) => this._ExecuteAction(action, true);

    public void RepeatActions(Action<ScriptEngine, IScriptAction> preAction = null, Action<ScriptEngine, IScriptAction> postAction = null) {
      var actions = this._actionList;
      foreach (var action in actions) {
        preAction?.Invoke(this, action);
        this._ExecuteAction(action, false);
        postAction?.Invoke(this, action);
      }
    }

    public void AddWithoutExecution(IScriptAction action) {
      Contract.Requires(action != null);
      this._actionList.Add(action);
    }

    /// <summary>
    /// Runs <paramref name="action"/> against the current (source, target) pair, then promotes
    /// any new bitmaps the action produced into the engine's authoritative slots — disposing
    /// the previous occupants. Action-internal bitmaps that the engine doesn't adopt (e.g. an
    /// intermediate the action forgot to dispose) are left to GC; the action contract is that
    /// the bitmap it sets on SourceImage / TargetImage becomes engine-owned, unless the action
    /// reports a non-null <see cref="IScriptAction.PoolSourceKey"/> — in which case ownership
    /// stays with <see cref="MasterPool"/> and the engine adopts non-owning.
    /// </summary>
    private void _ExecuteAction(IScriptAction action, bool addToList) {
      Contract.Requires(action != null);

      action.SourceImage = this.SourceImage;
      action.TargetImage = this.TargetImage;

      // Wire the master pool into pool-aware actions before Execute. Legacy actions don't see it.
      if (action is IPoolAwareScriptAction poolAware)
        poolAware.MasterPool = this.MasterPool;

      this.IsSourceImageChanged = false;
      this.IsTargetImageChanged = false;

      var result = action.Execute();
      Contract.Assert(result, "action failed somehow");

      if (addToList)
        this.AddWithoutExecution(action);

      if (action.ChangesSourceImage) {
        var poolKey = action.PoolSourceKey;
        if (poolKey != null && action.SourceImage != null)
          this.SetSourceImageNonOwning(action.SourceImage, poolKey);
        else
          this.SourceImage = action.SourceImage;
        this.IsSourceImageChanged = true;
      }

      if (action.ChangesTargetImage) {
        this.TargetImage = action.TargetImage;
        this.IsTargetImageChanged = true;
      }
    }

    /// <summary>
    /// Removes everything since the last source change.
    /// </summary>
    public void RevertToLastSource() {
      var actions = this._actionList;
      while (actions.Any() && !actions.Last().ChangesSourceImage)
        actions.RemoveAt(actions.Count - 1);
    }
  }
}
