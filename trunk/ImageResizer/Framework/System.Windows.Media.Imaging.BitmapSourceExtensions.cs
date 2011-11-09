using System.Drawing;
namespace System.Windows.Media.Imaging {
  public static class BitmapSourceExtensions {
    public static Bitmap AsBitmap(this BitmapSource objSource) {
      Bitmap result;
      using (var objMemoryStream = new IO.MemoryStream()) {
        var objBmpBitmapEncoder = new BmpBitmapEncoder();
        objBmpBitmapEncoder.Frames.Add(BitmapFrame.Create(objSource));
        objBmpBitmapEncoder.Save(objMemoryStream);
        result = new Bitmap(objMemoryStream);
        result = new Bitmap(result);
      }
      return (result);
    }
  }
}
