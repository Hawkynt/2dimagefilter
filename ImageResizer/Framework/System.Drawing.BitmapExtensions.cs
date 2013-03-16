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
using System.Windows;
using System.Windows.Media.Imaging;
namespace System.Drawing {
  public static partial class BitmapExtensions {
    [Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    /// <summary>
    /// Copies the given Bitmap into a BitmapSource.
    /// </summary>
    /// <param name="This">This Bitmap.</param>
    /// <returns>The copy of the Bitmap as a BitmapSource-instance.</returns>
    public static BitmapSource AsBitmapSource(this Bitmap This) {
      Contract.Requires(This != null);
      var hndlBitmap = This.GetHbitmap();
      var result = Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
          hndlBitmap,
          IntPtr.Zero,
          Int32Rect.Empty,
          BitmapSizeOptions.FromEmptyOptions()
      );
      result.Freeze();
      DeleteObject(hndlBitmap);
      return (result);
    }
  }
}
