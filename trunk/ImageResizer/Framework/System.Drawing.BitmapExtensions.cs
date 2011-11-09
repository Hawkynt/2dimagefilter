using System.Windows.Media.Imaging;
using System.Windows;
namespace System.Drawing {
  public static class BitmapExtensions {
    [Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    public static BitmapSource AsBitmapSource(this Bitmap objSource) {
      var hndlBitmap = objSource.GetHbitmap();
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
