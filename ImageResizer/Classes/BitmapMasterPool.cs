#region (c)2008-2026 Hawkynt
/*
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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Classes {

  /// <summary>
  /// File-keyed LRU pool of "master" <see cref="Bitmap"/> instances. The pool owns each master
  /// and never lets a worker thread mutate or lock it; UI code receives a reference for display
  /// and workers always check out a fresh privately-owned <see cref="Bitmap.Clone()"/>-style copy
  /// via <see cref="CheckoutClone"/>.
  /// <para>
  /// Rationale: a recent UI smoke crash in <c>PictureBox.OnPaint -&gt; Graphics.DrawImage</c> with
  /// "Bitmap region is already locked" was caused by a worker <c>Task.Run</c> calling LockBits on
  /// the same engine-owned bitmap that <c>iwhSourceImage</c> was simultaneously painting. The pool
  /// makes that contention impossible by design: the master is read-only from the consumer's
  /// perspective, and any worker that needs pixel access calls <see cref="CheckoutClone"/> on the
  /// UI thread to obtain a private copy.
  /// </para>
  /// <para>
  /// File records are keyed by absolute path (case-insensitive on Windows) plus last-write mtime;
  /// reopening the same file with unchanged mtime is an instant cache hit. Synthetic records
  /// (results of an Apply commit, drag-dropped raw bitmaps, etc.) are keyed by an opaque
  /// <c>"synthetic://N"</c> string the caller stores.
  /// </para>
  /// </summary>
  // TODO: thumbnail pool — a sibling pool keyed off (master-key, target-size) producing decoded
  // thumbnails for list previews. Out of scope for this iteration; the master pool ships first.
  internal sealed class BitmapMasterPool : IDisposable {

    /// <summary>
    /// One pool record. <see cref="Master"/> is owned by the pool and disposed on eviction or
    /// pool-disposal; consumers receive references for display only.
    /// </summary>
    private sealed class Record {
      public string Key;
      public bool IsSynthetic;
      public DateTime? LastWriteUtc;
      public Bitmap Master;
    }

    private readonly LinkedList<Record> _lru = new LinkedList<Record>();
    private readonly Dictionary<string, LinkedListNode<Record>> _byKey
      = new Dictionary<string, LinkedListNode<Record>>(StringComparer.OrdinalIgnoreCase);
    private readonly int _maxRecords;
    private int _syntheticCounter;
    private bool _disposed;

    /// <summary>
    /// Creates a new pool. <paramref name="maxRecords"/> bounds the LRU; once exceeded, the
    /// least-recently-used record is evicted (its master <see cref="Bitmap"/> is disposed).
    /// </summary>
    public BitmapMasterPool(int maxRecords = 8) {
      Contract.Requires(maxRecords > 0);
      this._maxRecords = maxRecords;
    }

    /// <summary>
    /// Loads (or returns the cached master for) the file at <paramref name="absolutePath"/>.
    /// Cache hit when path + last-write-mtime both match. The returned <see cref="Bitmap"/>
    /// is owned by the pool — the caller MUST NOT lock or mutate it.
    /// </summary>
    public Bitmap LoadOrGet(string absolutePath) {
      Contract.Requires(!string.IsNullOrWhiteSpace(absolutePath));
      this._ThrowIfDisposed();

      var key = Path.GetFullPath(absolutePath);
      var mtime = File.GetLastWriteTimeUtc(key);

      if (this._byKey.TryGetValue(key, out var existing)) {
        var record = existing.Value;
        if (!record.IsSynthetic && record.LastWriteUtc == mtime) {
          // Cache hit — promote to MRU and hand back the master reference.
          this._lru.Remove(existing);
          this._lru.AddLast(existing);
          return record.Master;
        }

        // Stale (mtime moved) — evict and re-load.
        this._RemoveNode(existing);
      }

      var master = _LoadFromDisk(key);
      this._InsertNew(new Record {
        Key = key,
        IsSynthetic = false,
        LastWriteUtc = mtime,
        Master = master,
      });
      return master;
    }

    /// <summary>
    /// Inserts a synthetic master not backed by a file (e.g. an Apply result, a drag-dropped raw
    /// bitmap). Returns the opaque key the caller stores to refer to this record later. The
    /// pool takes ownership of <paramref name="master"/>; the caller must not dispose it.
    /// </summary>
    public string InsertSynthetic(Bitmap master) {
      Contract.Requires(master != null);
      this._ThrowIfDisposed();

      var key = "synthetic://" + (++this._syntheticCounter);
      this._InsertNew(new Record {
        Key = key,
        IsSynthetic = true,
        LastWriteUtc = null,
        Master = master,
      });
      return key;
    }

    /// <summary>
    /// Returns the master <see cref="Bitmap"/> for a previously-known key (file path or
    /// <see cref="InsertSynthetic"/>-returned key). UI may display the returned reference but
    /// MUST NOT lock or mutate it. Returns <c>null</c> when the key has been evicted.
    /// </summary>
    public Bitmap GetMaster(string key) {
      Contract.Requires(!string.IsNullOrWhiteSpace(key));
      this._ThrowIfDisposed();

      if (!this._byKey.TryGetValue(key, out var node))
        return null;

      // Touch — this counts as recent use.
      this._lru.Remove(node);
      this._lru.AddLast(node);
      return node.Value.Master;
    }

    /// <summary>
    /// Returns a fresh privately-owned clone of the master at <paramref name="key"/>. The clone
    /// is performed on the calling thread, which SHOULD be the UI thread to avoid GDI+ contention
    /// with concurrent <c>PictureBox</c> paints (WinForms serialises WM_PAINT on the UI thread,
    /// so a same-thread clone cannot collide with a paint). Caller owns and disposes the result.
    /// </summary>
    /// <exception cref="KeyNotFoundException">When <paramref name="key"/> is unknown.</exception>
    public Bitmap CheckoutClone(string key) {
      Contract.Requires(!string.IsNullOrWhiteSpace(key));
      this._ThrowIfDisposed();

      if (!this._byKey.TryGetValue(key, out var node))
        throw new KeyNotFoundException("BitmapMasterPool: no record for key '" + key + "'.");

      // Touch — checking out is "use".
      this._lru.Remove(node);
      this._lru.AddLast(node);

      var master = node.Value.Master;
      var clone = new Bitmap(master.Width, master.Height, PixelFormat.Format32bppArgb);
      using (var g = Graphics.FromImage(clone))
        g.DrawImageUnscaled(master, 0, 0);
      return clone;
    }

    /// <summary>
    /// Replaces the master at <paramref name="key"/> with <paramref name="newMaster"/>; the
    /// previous master is disposed. Use after an Apply commit when the working source is being
    /// promoted in place. The pool takes ownership of <paramref name="newMaster"/>.
    /// </summary>
    public void Replace(string key, Bitmap newMaster) {
      Contract.Requires(!string.IsNullOrWhiteSpace(key));
      Contract.Requires(newMaster != null);
      this._ThrowIfDisposed();

      if (!this._byKey.TryGetValue(key, out var node))
        throw new KeyNotFoundException("BitmapMasterPool: no record for key '" + key + "'.");

      var record = node.Value;
      var oldMaster = record.Master;
      record.Master = newMaster;
      // Replacing is "use"; promote to MRU.
      this._lru.Remove(node);
      this._lru.AddLast(node);
      if (!ReferenceEquals(oldMaster, newMaster))
        oldMaster?.Dispose();
    }

    /// <summary>
    /// Drops the record for <paramref name="key"/>; its master <see cref="Bitmap"/> is disposed.
    /// No-op when <paramref name="key"/> is unknown.
    /// </summary>
    public void Evict(string key) {
      Contract.Requires(!string.IsNullOrWhiteSpace(key));
      this._ThrowIfDisposed();

      if (!this._byKey.TryGetValue(key, out var node))
        return;
      this._RemoveNode(node);
    }

    /// <summary>
    /// Disposes every cached master. The pool is unusable afterwards.
    /// </summary>
    public void Dispose() {
      if (this._disposed)
        return;
      this._disposed = true;

      foreach (var node in this._byKey.Values)
        node.Value.Master?.Dispose();

      this._byKey.Clear();
      this._lru.Clear();
    }

    /// <summary>Number of records currently held; exposed for tests / diagnostics.</summary>
    public int Count => this._byKey.Count;

    private void _InsertNew(Record record) {
      while (this._byKey.Count >= this._maxRecords) {
        var head = this._lru.First;
        if (head == null)
          break;
        this._RemoveNode(head);
      }

      var node = this._lru.AddLast(record);
      this._byKey[record.Key] = node;
    }

    private void _RemoveNode(LinkedListNode<Record> node) {
      this._lru.Remove(node);
      this._byKey.Remove(node.Value.Key);
      node.Value.Master?.Dispose();
    }

    private void _ThrowIfDisposed() {
      if (this._disposed)
        throw new ObjectDisposedException(nameof(BitmapMasterPool));
    }

    /// <summary>
    /// Loads a file into a freshly-allocated <see cref="Bitmap"/> that does not retain a handle
    /// to the source stream. Mirrors the discipline used by the upstream golden harness
    /// (<c>GoldenHarness.LoadInput</c>): <see cref="Image.FromFile"/> keeps the file mapped until
    /// the image is disposed, so we draw it into a fresh 32bpp ARGB bitmap and discard the loader.
    /// </summary>
    private static Bitmap _LoadFromDisk(string absolutePath) {
      using (var loaded = Image.FromFile(absolutePath))
        return BitmapLoader.CopyPreservingTransparency(loaded);
    }
  }
}
