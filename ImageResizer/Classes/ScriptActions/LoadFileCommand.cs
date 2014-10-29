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
using System.Diagnostics.Contracts;
using System.Drawing;

using Imager;

namespace Classes.ScriptActions {
  internal class LoadFileCommand : IScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage { get { return (true); } }

    public bool ChangesTargetImage { get { return (true); } }
    public bool ProvidesNewGdiSource { get { return (true); } }

    public bool Execute() {
      this.SourceImage = cImage.FromBitmap(this.GdiSource = (Bitmap)Image.FromFile(this._fileName));
      return (true);
    }

    public cImage SourceImage { get; set; }

    public cImage TargetImage { get { return (null); } set { } }

    public Bitmap GdiSource { get; private set; }
    #endregion

    private readonly string _fileName;

    public string FileName { get { return (this._fileName); } }

    public LoadFileCommand(string fileName) {
      Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
      this._fileName = fileName;
    }

  }
}
