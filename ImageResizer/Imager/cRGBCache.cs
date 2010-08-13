#region (c)2010 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010 Hawkynt

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
 * This is a C# port of my former classImage perl library.
 * You can use and modify my code as long as you give me a credit and 
 * inform me about updates, changes new features and modification. 
 * Distribution and selling is allowed. Would be nice if you give some 
 * payback.
*/
#endregion
#define PREFERARRAYCACHE
using System;
namespace nImager {
  /// <summary>
  /// A little cache that holds calculation results based on the three color components red, green and blue.
  /// </summary>
  public class cRGBCache {
#if PREFERARRAYCACHE
    private readonly byte[] _arrCache = new byte[256 * 256 * 256];
    private readonly byte[] _arrExists = new byte[256 * 256 * 256];

    /// <summary>
    /// Gets a value directly from the cache or first calculates that value and writes it the cache.
    /// </summary>
    /// <param name="dwordKey">The 32-bit color code.</param>
    /// <param name="ptrFactory">The factory that would calculate a result if it's not already in the cache.</param>
    /// <returns>The calculation result.</returns>
    public byte GetOrAdd(UInt32 dwordKey, Func<UInt32, byte> ptrFactory) {
      byte byteRet;
      if (_arrExists[dwordKey] != 0) {
        byteRet = _arrCache[dwordKey];
      } else {
        byteRet = ptrFactory(dwordKey);
        this._arrCache[dwordKey] = byteRet;
        this._arrExists[dwordKey] = 1;
        System.Threading.Thread.MemoryBarrier();
      }
      return (byteRet);
    }
#else
    /// <summary>
    /// Our thread-safe dictionary cache
    /// </summary>
    private readonly ConcurrentDictionary<UInt32, byte> _hashCache = new ConcurrentDictionary<uint, byte>();
    /// <summary>
    /// Gets a value directly from the cache or first calculates that value and writes it the cache.
    /// </summary>
    /// <param name="dwordKey">The 32-bit color code.</param>
    /// <param name="ptrFactory">The factory that would calculate a result if it's not already in the cache.</param>
    /// <returns>The calculation result.</returns>
    public byte GetOrAdd(UInt32 dwordKey, Func<UInt32, byte> ptrFactory) {
      return (this._hashCache.GetOrAdd(dwordKey, ptrFactory));
    }
#endif
  } // end class
} // end namespace