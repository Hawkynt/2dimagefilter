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

namespace System.Windows.Media.Imaging {
  internal static partial class WriteableBitmapExtensions {


    #region nested types

    /// <summary>
    /// A bitmapdata that automatically gets properly disposed.
    /// </summary>
    public interface IDisposableWriteableBitmap : IDisposable {
      WriteableBitmap Bitmap { get; }
    }

    /// <summary>
    /// A bitmapdata that automatically gets properly disposed.
    /// </summary>
    private class DisposableWriteableBitmap : IDisposableWriteableBitmap {

      private bool _isDisposed;

      public DisposableWriteableBitmap(WriteableBitmap source) {
        this.Bitmap = source;
        source.Lock();
      }

      #region Implementation of IDisposable

      ~DisposableWriteableBitmap() => this.Dispose();

      public void Dispose() {
        if (this._isDisposed)
          return;

        this._isDisposed = true;
        this.Bitmap.Unlock();
      }

      #endregion

      #region Implementation of IDisposableBitmapData

      public WriteableBitmap Bitmap { get; }

      #endregion
    }

    #endregion

    public static IDisposableWriteableBitmap LockData(this WriteableBitmap @this)=>new DisposableWriteableBitmap(@this);

  }
}
