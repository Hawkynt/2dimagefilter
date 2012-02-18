using System.Diagnostics.Contracts;
using System.Drawing;
namespace System.Windows.Media.Imaging {
  public static class BitmapSourceExtensions {
    public static Bitmap AsBitmap(this BitmapSource This) {
      Contract.Requires(This != null);
      Bitmap result;
      using (var memoryStream = new IO.MemoryStream()) {
        var bitmapEncoder = new BmpBitmapEncoder();
        bitmapEncoder.Frames.Add(BitmapFrame.Create(This));
        bitmapEncoder.Save(memoryStream);
        using (var temp = new Bitmap(memoryStream))
          result = new Bitmap(temp);
      }
      return (result);
    }
  }
}
