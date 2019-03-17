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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
namespace System.Drawing {
  internal static partial class BitmapExtensions {

    #region nested types

    /// <summary>
    /// A bitmapdata that automatically gets properly disposed.
    /// </summary>
    public interface IDisposableBitmapData : IDisposable {
      BitmapData BitmapData { get; }
    }

    /// <summary>
    /// A bitmapdata that automatically gets properly disposed.
    /// </summary>
    private class DisposableBitmapData:IDisposableBitmapData {

      private readonly Bitmap _source;
      private bool _isDisposed;

      public DisposableBitmapData(Bitmap source,Rectangle rect,ImageLockMode mode,PixelFormat format) {
        this._source = source;
        this.BitmapData= source.LockBits(rect,mode,format);
      }

      #region Implementation of IDisposable

      ~DisposableBitmapData() => this.Dispose();

      public void Dispose() {
        if (this._isDisposed)
          return;

        this._isDisposed = true;
        this._source.UnlockBits(this.BitmapData);
      }

      #endregion

      #region Implementation of IDisposableBitmapData

      public BitmapData BitmapData { get; }

      #endregion
    }

    /// <summary>
    /// A bitmap handle that automatically gets properly disposed.
    /// </summary>
    private class DisposableBitmapHandle : IDisposable {

      private static class NativeMethods {

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(IntPtr hObject);

      }

      public IntPtr Handle { get; }
      private bool _isDisposed;

      public DisposableBitmapHandle(Bitmap source) => this.Handle = source.GetHbitmap();

      #region IDisposable

      ~DisposableBitmapHandle() => this.Dispose();

      public void Dispose() {
        if (this._isDisposed)
          return;

        this._isDisposed = true;
        if(this.Handle!=IntPtr.Zero)
          NativeMethods.DeleteObject(this.Handle);
      }

      #endregion
    }

    #endregion

    /// <summary>
    /// Copies the given Bitmap into a BitmapSource.
    /// </summary>
    /// <param name="this">This Bitmap.</param>
    /// <returns>The copy of the Bitmap as a BitmapSource-instance.</returns>
    public static BitmapSource AsBitmapSource(this Bitmap @this) {
      Contract.Requires(@this != null);
      using(var handle=new DisposableBitmapHandle(@this)) {
        var result = Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
          handle.Handle,
          IntPtr.Zero,
          Int32Rect.Empty,
          BitmapSizeOptions.FromEmptyOptions()
        );
        result.Freeze();
        return result;
      }
    }

    /// <summary>
    /// Locks a bitmap for write mode only.
    /// </summary>
    /// <param name="this">This <see cref="Bitmap">Bitmap</see>-instance</param>
    /// <param name="rect">A custom rectangle to lock; defaults to full bitmap dimensions</param>
    /// <returns>A disposable bitmap data instance</returns>
    public static IDisposableBitmapData LockForWrite(this Bitmap @this, Rectangle? rect = null)
      => new DisposableBitmapData(@this, rect ?? new Rectangle(0, 0, @this.Width, @this.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)
    ;

    /// <summary>
    /// Locks a bitmap for read mode only.
    /// </summary>
    /// <param name="this">This <see cref="Bitmap">Bitmap</see>-instance</param>
    /// <param name="rect">A custom rectangle to lock; defaults to full bitmap dimensions</param>
    /// <returns>A disposable bitmap data instance</returns>
    public static IDisposableBitmapData LockForRead(this Bitmap @this, Rectangle? rect = null)
      => new DisposableBitmapData(@this, rect ?? new Rectangle(0, 0, @this.Width, @this.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
    ;

  }
}
