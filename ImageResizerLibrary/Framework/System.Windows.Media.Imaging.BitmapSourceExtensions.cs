#region (c)2010-2020 Hawkynt
/*
  This file is part of Hawkynt's .NET Framework extensions.

    Hawkynt's .NET Framework extensions are free software: 
    you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Hawkynt's .NET Framework extensions is distributed in the hope that 
    it will be useful, but WITHOUT ANY WARRANTY; without even the implied 
    warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
    the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Hawkynt's .NET Framework extensions.  
    If not, see <http://www.gnu.org/licenses/>.
*/
#endregion
using System.Diagnostics.Contracts;
using System.Drawing;
namespace System.Windows.Media.Imaging {
  internal static partial class BitmapSourceExtensions {
    /// <summary>
    /// Converts the given BitmapSource into a Bitmap.
    /// </summary>
    /// <param name="this">This BitmapSource.</param>
    /// <returns>The copy of the BitmapSource as a Bitmap-Instance.</returns>
    public static Bitmap AsBitmap(this BitmapSource @this) {
      Contract.Requires(@this != null);
      using (var memoryStream = new IO.MemoryStream()) {
        var bitmapEncoder = new BmpBitmapEncoder();
        var bitmapFrame = BitmapFrame.Create(@this);
        bitmapEncoder.Frames.Add(bitmapFrame);
        bitmapEncoder.Save(memoryStream);
        using (var temp = new Bitmap(memoryStream))
          return new Bitmap(temp);
      }
    }
  }
}
