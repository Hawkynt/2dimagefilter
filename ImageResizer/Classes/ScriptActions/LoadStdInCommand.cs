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
using System.Drawing;

namespace Classes.ScriptActions {
  internal class LoadStdInCommand : IScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage => true;
    public bool ChangesTargetImage => true;
    public bool ProvidesNewGdiSource => true;

    public bool Execute() {
      using (var stream = Console.OpenStandardInput())
      using (var image = Image.FromStream(stream, false))
        this.SourceImage = this.GdiSource = BitmapLoader.CopyPreservingTransparency(image);
      return true;
    }

    public Bitmap SourceImage { get; set; }

    public Bitmap TargetImage {
      get => null;
      set { }
    }

    public Bitmap GdiSource { get; private set; }
    public string PoolSourceKey => null;
    #endregion

  }
}
