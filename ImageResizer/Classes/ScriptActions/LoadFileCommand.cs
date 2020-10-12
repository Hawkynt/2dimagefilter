#region (c)2008-2020 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2020 Hawkynt

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
    public bool ChangesSourceImage => true;

    public bool ChangesTargetImage => true;
    public bool ProvidesNewGdiSource => true;

    public bool Execute() {
      // releasing file handle owned by Image.FromFile by creating a copy of the original image
      using(var image=Image.FromFile(this.FileName))
        this.SourceImage = cImage.FromBitmap(this.GdiSource = new Bitmap(image));

      return true;
    }

    public cImage SourceImage { get; set; }

    public cImage TargetImage {
      get => null;
      set { }
    }

    public Bitmap GdiSource { get; private set; }
    #endregion

    public string FileName { get; }

    public LoadFileCommand(string fileName) {
      Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
      this.FileName = fileName;
    }

  }
}
