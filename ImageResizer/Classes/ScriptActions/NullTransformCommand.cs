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
  /// Copies <see cref="IScriptAction.SourceImage"/> into <see cref="IScriptAction.TargetImage"/>.
  /// We <see cref="Bitmap.Clone()"/> the source so the engine's source / target slots stay
  /// independently owned — disposing one must not invalidate the other.
  /// </summary>
  internal class NullTransformCommand : IScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage => false;
    public bool ChangesTargetImage => true;
    public bool ProvidesNewGdiSource => false;

    public bool Execute() {
      this.TargetImage = this.SourceImage == null ? null : (Bitmap)this.SourceImage.Clone();
      return true;
    }

    public Bitmap GdiSource => null;

    public Bitmap SourceImage { get; set; }
    public Bitmap TargetImage { get; set; }
    public string PoolSourceKey => null;
    #endregion
  }
}
