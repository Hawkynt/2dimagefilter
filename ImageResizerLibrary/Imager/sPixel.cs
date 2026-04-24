#region (c)2008-2026 Hawkynt
/*
 *  cImage
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

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

using System;
using System.Drawing;
using System.Runtime.Serialization;
using dword = System.UInt32;

namespace Imager {
  /// <summary>
  /// 32-bit BGRA pixel. Retained as a storage struct for <see cref="cImage._imageData"/>
  /// and the Bitmap marshalling in <c>Image.Bitmaps.cs</c>, which reinterprets this struct
  /// as <c>int*</c> in pinned blocks.
  /// </summary>
  /// <remarks>
  /// Earlier revisions of this struct exposed dozens of colour-space projections, operators,
  /// caches, and similarity predicates used by the local XBR / XBRz / NQ / resampler pipeline.
  /// After that pipeline migrated to the upstream <c>FrameworkExtensions.System.Drawing</c>
  /// registries, the only remaining consumers need byte-channel access + a <see cref="Color"/>
  /// view. Everything else has been removed.
  /// </remarks>
  public struct sPixel : ISerializable {

    private const int _RGB_MASK = 0xffffff;
    private const int _ALPHA_SHIFT = 24;
    private const int _RED_SHIFT = 16;
    private const int _GREEN_SHIFT = 8;
    private const int _BLUE_SHIFT = 0;

    /// <summary>The packed ARGB storage.</summary>
    private readonly dword _argbBytes;

    #region ctors

    public sPixel(byte red, byte green, byte blue, byte alpha = 255) {
      this._argbBytes = (dword)(
        ((dword)alpha << _ALPHA_SHIFT)
        | ((dword)red << _RED_SHIFT)
        | ((dword)green << _GREEN_SHIFT)
        | ((dword)blue << _BLUE_SHIFT)
      );
    }

    public sPixel(float red, float green, float blue, double alpha = 1.0)
      : this(_Float2Byte(red), _Float2Byte(green), _Float2Byte(blue), _Float2Byte((float)alpha)) { }

    public sPixel(double red, double green, double blue, double alpha = 1.0)
      : this((float)red, (float)green, (float)blue, alpha) { }

    /// <summary>Serialization ctor (required by <see cref="ISerializable"/>).</summary>
    private sPixel(SerializationInfo info, StreamingContext context) {
      this._argbBytes = (dword)info.GetUInt32("ARGB");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      info.AddValue("ARGB", (uint)this._argbBytes);
    }

    /// <summary>Greyscale convenience factory.</summary>
    public static sPixel FromGrey(byte value, byte alpha = 255) => new sPixel(value, value, value, alpha);

    #endregion

    #region accessors

    public byte Alpha => (byte)((this._argbBytes >> _ALPHA_SHIFT) & 0xff);
    public byte Red => (byte)((this._argbBytes >> _RED_SHIFT) & 0xff);
    public byte Green => (byte)((this._argbBytes >> _GREEN_SHIFT) & 0xff);
    public byte Blue => (byte)((this._argbBytes >> _BLUE_SHIFT) & 0xff);

    public Color Color => Color.FromArgb(this.Alpha, this.Red, this.Green, this.Blue);

    #endregion

    private static byte _Float2Byte(float value) {
      if (float.IsNaN(value) || value <= 0f) return 0;
      if (value >= 1f) return 255;
      return (byte)(value * 255f + 0.5f);
    }

    public override int GetHashCode() => (int)this._argbBytes;
    public override bool Equals(object obj) => obj is sPixel p && p._argbBytes == this._argbBytes;
    public override string ToString() => $"ARGB({this.Alpha}, {this.Red}, {this.Green}, {this.Blue})";
  }
}
