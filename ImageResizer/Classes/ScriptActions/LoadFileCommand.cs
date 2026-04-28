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
using System.Diagnostics.Contracts;
using System.Drawing;

namespace Classes.ScriptActions {
  /// <summary>
  /// Loads an image file into the source slot. When a <see cref="BitmapMasterPool"/> is wired in
  /// via <see cref="IPoolAwareScriptAction.MasterPool"/> the bitmap goes through the pool —
  /// the engine adopts it non-owning, and a subsequent reload of the same file with unchanged
  /// mtime is an instant cache hit. Without a pool the legacy path runs: <see cref="Image.FromFile"/>
  /// into a standalone <see cref="Bitmap"/> the engine then owns and disposes.
  /// </summary>
  internal class LoadFileCommand : IPoolAwareScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage => true;
    public bool ChangesTargetImage => true;
    public bool ProvidesNewGdiSource => true;

    public bool Execute() {
      var pool = this.MasterPool;
      if (pool != null) {
        // Pool-managed path: pool owns the bitmap; we report the key so the engine adopts non-owning.
        var master = pool.LoadOrGet(this.FileName);
        this.SourceImage = this.GdiSource = master;
        this.PoolSourceKey = this.FileName;
        return true;
      }

      // Legacy engine-owned path: copy the loader output into a standalone Bitmap so the file
      // handle is released before we hand ownership to the engine.
      using (var image = Image.FromFile(this.FileName))
        this.SourceImage = this.GdiSource = new Bitmap(image);

      this.PoolSourceKey = null;
      return true;
    }

    public Bitmap SourceImage { get; set; }

    public Bitmap TargetImage {
      get => null;
      set { }
    }

    public Bitmap GdiSource { get; private set; }

    public string PoolSourceKey { get; private set; }

    public BitmapMasterPool MasterPool { get; set; }
    #endregion

    public string FileName { get; }

    public LoadFileCommand(string fileName) {
      Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
      this.FileName = fileName;
    }

  }
}
