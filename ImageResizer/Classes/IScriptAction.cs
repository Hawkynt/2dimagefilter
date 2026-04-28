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

using System.Drawing;

namespace Classes {
  internal interface IScriptAction {

    bool ChangesSourceImage { get; }
    bool ChangesTargetImage { get; }
    bool ProvidesNewGdiSource { get; }

    bool Execute();
    Bitmap GdiSource { get; }

    Bitmap SourceImage { get; set; }
    Bitmap TargetImage { get; set; }

    /// <summary>
    /// When non-null, the action's <see cref="SourceImage"/> output is owned by
    /// <see cref="ScriptEngine.MasterPool"/> and the engine must adopt it via
    /// <see cref="ScriptEngine.SetSourceImageNonOwning"/> rather than the regular owning setter.
    /// Null means "engine owns whatever I produced" (the legacy contract). Wired through
    /// Track-C's BitmapMasterPool plumbing; legacy actions return null.
    /// </summary>
    string PoolSourceKey { get; }
  }

  /// <summary>
  /// Optional capability marker for script actions that route through the
  /// <see cref="BitmapMasterPool"/>. The engine sets <see cref="MasterPool"/> before calling
  /// <see cref="IScriptAction.Execute"/>; the action uses it to obtain pool-managed source
  /// bitmaps and reports the resulting key via <see cref="IScriptAction.PoolSourceKey"/>.
  /// Actions that don't implement this interface are treated as engine-owning (legacy).
  /// </summary>
  internal interface IPoolAwareScriptAction : IScriptAction {
    BitmapMasterPool MasterPool { get; set; }
  }
}
