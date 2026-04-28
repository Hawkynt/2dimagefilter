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

namespace Classes.ScriptActions {
  /// <summary>
  /// Promotes the target image into the source slot and clears the target. Pre-M5 (cImage)
  /// the same bitmap was simply aliased into both slots; with engine-owned <see cref="Bitmap"/>
  /// instances we must <see cref="Bitmap.Clone()"/> instead — the engine's TargetImage setter
  /// would otherwise dispose the just-promoted source bitmap when we null the target.
  /// </summary>
  internal class TargetToSourceCommand : IScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage => true;
    public bool ChangesTargetImage => true;
    public bool ProvidesNewGdiSource => false;

    public bool Execute() {
      this.SourceImage = this.TargetImage == null ? null : (Bitmap)this.TargetImage.Clone();
      this.TargetImage = null;
      return true;
    }

    public Bitmap GdiSource => null;

    public Bitmap SourceImage { get; set; }
    public Bitmap TargetImage { get; set; }
    public string PoolSourceKey => null;
    #endregion
  }
}
