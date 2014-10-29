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
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Imager;

namespace Classes {
  internal class ScriptEngine {

    /// <summary>
    /// Current source image.
    /// </summary>
    private cImage _sourceImage;
    /// <summary>
    /// Current source image as a GDI+ version.
    /// </summary>
    private Bitmap _gdiSource;
    /// <summary>
    /// Current target image.
    /// </summary>
    private cImage _targetImage;
    /// <summary>
    /// Current source image as a GDI+ version.
    /// </summary>
    private Bitmap _gdiTarget;

    /// <summary>
    /// Gets or sets the source image.
    /// </summary>
    /// <value>
    /// The source image.
    /// </value>
    public cImage SourceImage {
      get { return (this._sourceImage); }
      private set {
        this._sourceImage = value;
        if (this._gdiSource != null)
          this._gdiSource.Dispose();
        this._gdiSource = null;
      }
    }

    /// <summary>
    /// Gets or sets the target image.
    /// </summary>
    /// <value>
    /// The target image.
    /// </value>
    public cImage TargetImage {
      get { return (this._targetImage); }
      private set {
        this._targetImage = value;
        if (this._gdiTarget != null)
          this._gdiTarget.Dispose();
        this._gdiTarget = null;
      }
    }

    /// <summary>
    /// Gets the GDI source.
    /// </summary>
    public Bitmap GdiSource { get { return (this._gdiSource ?? (this._gdiSource = this._sourceImage == null ? null : this._sourceImage.ToBitmap())); } }

    /// <summary>
    /// Gets the GDI target.
    /// </summary>
    public Bitmap GdiTarget { get { return (this._gdiTarget ?? (this._gdiTarget = this._targetImage == null ? null : this._targetImage.ToBitmap())); } }

    /// <summary>
    /// Current list of actions.
    /// </summary>
    private readonly List<IScriptAction> _actionList = new List<IScriptAction>();

    /// <summary>
    /// Gets a value indicating whether this instance is source image changed.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is source image changed; otherwise, <c>false</c>.
    /// </value>
    public bool IsSourceImageChanged { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance is target image changed.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is target image changed; otherwise, <c>false</c>.
    /// </value>
    public bool IsTargetImageChanged { get; private set; }

    /// <summary>
    /// Clears the action list.
    /// </summary>
    public void Clear() {
      this._actionList.Clear();
    }

    /// <summary>
    /// Gets the actions.
    /// Note: We're creating an enumeration so our own list stays save and is not modified by another class.
    /// </summary>
    public IEnumerable<IScriptAction> Actions { get { return (this._actionList.Select(t => t)); } }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="action">The action.</param>
    public void ExecuteAction(IScriptAction action) {
      this._ExecuteAction(action, true);
    }

    /// <summary>
    /// Repeats the actions from the action list.
    /// </summary>
    /// <param name="preAction">The pre action.</param>
    /// <param name="postAction">The post action.</param>
    public void RepeatActions(Action<ScriptEngine, IScriptAction> preAction = null, Action<ScriptEngine, IScriptAction> postAction = null) {
      var actions = this._actionList;
      foreach (var action in actions) {
        if (preAction != null)
          preAction(this, action);
        this._ExecuteAction(action, false);
        if (postAction != null)
          postAction(this, action);
      }
    }

    /// <summary>
    /// Adds the given action without executing it.
    /// </summary>
    /// <param name="action">The action.</param>
    public void AddWithoutExecution(IScriptAction action) {
      Contract.Requires(action != null);
      this._actionList.Add(action);
    }

    /// <summary>
    /// Executes the given action.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="addToList">if set to <c>true</c> the action will be added to the action list afterwards.</param>
    private void _ExecuteAction(IScriptAction action, bool addToList) {
      Contract.Requires(action != null);

      action.SourceImage = this.SourceImage;
      action.TargetImage = this.TargetImage;

      this.IsSourceImageChanged = false;
      this.IsTargetImageChanged = false;

      var result = action.Execute();

      // execution of action failed
      Contract.Assert(result, "action failed somehow");

      if (addToList)
        this.AddWithoutExecution(action);

      if (action.ChangesSourceImage) {
        this.SourceImage = action.SourceImage;
        this.IsSourceImageChanged = true;
      }

      if (action.ChangesTargetImage) {
        this.TargetImage = action.TargetImage;
        this.IsTargetImageChanged = true;
      }

      if (action.ProvidesNewGdiSource)
        this._gdiSource = action.GdiSource;
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
