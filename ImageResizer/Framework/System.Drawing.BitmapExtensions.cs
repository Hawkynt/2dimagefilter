using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Media.Imaging;
namespace System.Drawing {
  public static class BitmapExtensions {
    [Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

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
