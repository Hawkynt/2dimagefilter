#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

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

namespace Imager.Interface {
  /// <summary>
  /// How to treat pixels read outside the image bounds.
  /// Retained for the exe script serializer + plugin UI; not currently threaded through any scaler
  /// (a previous iteration's local XBR/XBRz/resampler pipeline consumed these; post-migration those
  /// paths use upstream Bitmap-based scalers that do their own edge handling).
  /// </summary>
  public enum OutOfBoundsMode {
    /// <summary>aaa abcde eee</summary>
    ConstantExtension = 0,
    /// <summary>cba abcde edc</summary>
    HalfSampleSymmetric,
    /// <summary>dcb abcde dcb</summary>
    WholeSampleSymmetric,
    /// <summary>cde abcde abc</summary>
    WrapAround,
    /// <summary>ttt abcde ttt</summary>
    Transparent,
  }
}
