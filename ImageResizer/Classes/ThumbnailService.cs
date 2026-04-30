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
 */
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using Hawkynt.ColorProcessing.Dithering;
using Hawkynt.ColorProcessing.Quantization;
using Imager.Pipelines;

namespace Classes {
  /// <summary>
  /// Cache key for thumbnail rendering. Includes a monotonic <see cref="SourceVersion"/> so
  /// that replacing the source bitmap implicitly invalidates all cached tiles without a
  /// manual <c>Clear</c> call.
  /// </summary>
  internal readonly struct ThumbnailKey : IEquatable<ThumbnailKey> {
    public readonly long SourceVersion;
    public readonly string QuantizerName;
    public readonly string DithererName; // null = no-dither
    public readonly ushort PaletteSize;
    public readonly int PreviewSize;

    public ThumbnailKey(long sourceVersion, string quantizerName, string dithererName, ushort paletteSize, int previewSize) {
      this.SourceVersion = sourceVersion;
      this.QuantizerName = quantizerName ?? string.Empty;
      this.DithererName = dithererName; // nullable on purpose
      this.PaletteSize = paletteSize;
      this.PreviewSize = previewSize;
    }

    public bool Equals(ThumbnailKey other)
      => this.SourceVersion == other.SourceVersion
      && this.PaletteSize == other.PaletteSize
      && this.PreviewSize == other.PreviewSize
      && string.Equals(this.QuantizerName, other.QuantizerName, StringComparison.Ordinal)
      && string.Equals(this.DithererName, other.DithererName, StringComparison.Ordinal);

    public override bool Equals(object obj) => obj is ThumbnailKey k && this.Equals(k);
    public override int GetHashCode() {
      unchecked {
        var h = this.SourceVersion.GetHashCode();
        h = (h * 397) ^ this.QuantizerName.GetHashCode();
        h = (h * 397) ^ (this.DithererName?.GetHashCode() ?? 0);
        h = (h * 397) ^ this.PaletteSize.GetHashCode();
        h = (h * 397) ^ this.PreviewSize.GetHashCode();
        return h;
      }
    }
  }

  /// <summary>
  /// Prioritised work queue for thumbnail rendering. Consumers call <see cref="Request"/>
  /// whenever a tile needs rendering, and <see cref="Reprioritise"/> on scroll or selection
  /// change so visible tiles leap-frog off-screen ones. The single background worker dequeues
  /// the highest-priority item; higher <c>priority</c> wins.
  /// </summary>
  internal sealed class ThumbnailService : IDisposable {

    /// <summary>Payload of a completed thumbnail render. Consumer owns <see cref="Bitmap"/> and must dispose on replace.</summary>
    public sealed class Result {
      public ThumbnailKey Key;
      public Bitmap Bitmap;
    }

    // Cached completed thumbnails. Bitmaps here are owned by the service and disposed on Clear/Dispose.
    private readonly Dictionary<ThumbnailKey, Bitmap> _cache = new Dictionary<ThumbnailKey, Bitmap>();
    private readonly object _cacheGate = new object();

    // Palette-build short-circuit: when the user picks a quantizer, every dither tile in the
    // strip + the detail preview render the SAME (source, quantizer, paletteSize) combo. The
    // upstream pipeline factors that into ComputeHistogram + ComputePalette + ApplyPaletteWithDither;
    // we cache the first two so each thumbnail/detail render only pays for the third.
    //   _cachedHistogram is per SourceVersion (single entry; replaced on source swap).
    //   _paletteCache is per (SourceVersion, quantName, paletteSize) — invalidated on source swap.
    private Dictionary<int, uint> _cachedHistogram;
    private long _cachedHistogramVersion = -1;
    private readonly Dictionary<(long version, string quantName, ushort paletteSize), Color[]> _paletteCache
      = new Dictionary<(long, string, ushort), Color[]>();
    private readonly object _paletteCacheGate = new object();

    // Pre-downscaled "master" bitmap pool per preview size. Built lazily on first render for
    // a given size; each pool holds N clones (N = worker count for thumbnail pools, fewer for
    // detail) so every concurrent render owns its own Bitmap instance and GDI+ LockBits won't
    // serialize them against each other.
    //
    // Keys: positive ints are thumbnail preview sizes; negative ints are detail-master slots
    // (-maxSide → detail master for that size).
    //
    // <see cref="ConcurrentDictionary"/> + <see cref="Lazy{T}"/> so:
    //  • Pool LOOKUP is lock-free — no worker blocks just because we're building a pool.
    //  • Pool BUILD for a given key runs exactly once (Lazy.Value with publication-thread-safety).
    //  • Builds for DIFFERENT keys proceed in parallel — thumbnail pools don't wait on the
    //    detail pool's ~500 ms HighQualityBicubic downsample.
    private readonly ConcurrentDictionary<int, Lazy<_BitmapPool>> _pools = new ConcurrentDictionary<int, Lazy<_BitmapPool>>();
    // Retained for ReplaceSource/Dispose where we need a global clear barrier.
    private readonly object _masterGate = new object();

    // Pending work items, one per cache key. Reprioritising rewrites the Priority field in place.
    private readonly Dictionary<ThumbnailKey, Work> _pending = new Dictionary<ThumbnailKey, Work>();
    private readonly object _queueGate = new object();

    // Keys that a worker has claimed (pulled from _pending) but not yet finished rendering.
    // Used for the "currently rendering" UI indicator.
    private readonly HashSet<ThumbnailKey> _inFlight = new HashSet<ThumbnailKey>();
    private readonly object _inFlightGate = new object();

    private readonly Thread[] _workers;
    private volatile bool _disposed;

    /// <summary>Resolved source bitmap (owned by the UI layer; we read-only-clone into thumbnails).</summary>
    public volatile Bitmap SourceBitmap;

    public long SourceVersion { get; private set; }

    /// <summary>
    /// Combined source analysis (distinct-RGB count + R/G/B histograms), computed in a
    /// single <c>LockBits</c> pass and cached per <see cref="SourceVersion"/>. Split getters
    /// (<see cref="DistinctColorCount"/>, <see cref="Histograms"/>) project from this struct.
    /// </summary>
    /// <remarks>
    /// <c>DistinctCount</c> is the exact number of distinct 24-bit RGB triples (alpha ignored);
    /// computed with a 2²⁴-bit bit-array so we don't need a HashSet and therefore don't need a
    /// cap to stay fast. The whole pass is ~1 s on a 164 MP source.
    /// </remarks>
    public readonly struct SourceStats {
      public readonly int DistinctCount;
      public readonly int[] R, G, B;     // 256-bin histograms
      public SourceStats(int distinct, int[] r, int[] g, int[] b) {
        this.DistinctCount = distinct; this.R = r; this.G = g; this.B = b;
      }
    }

    /// <summary>
    /// Lazily computes combined source stats on first access. One <c>LockBits</c> pass, no
    /// ThreadPool round-trip — callers should invoke from a background thread themselves if
    /// they don't want to block the UI. Calling this BEFORE starting thumbnail workers avoids
    /// GDI+ Bitmap contention: concurrent LockBits/DrawImage on the same Bitmap instance
    /// serialize inside GDI+ and make both paths feel glacial even on tiny sources.
    /// </summary>
    public SourceStats GetSourceStats() {
      lock (this._statsGate) {
        if (this._statsComputed) return this._stats;
        this._stats = _ComputeSourceStats(this.SourceBitmap);
        this._statsComputed = true;
        return this._stats;
      }
    }

    /// <summary>Convenience projection for legacy callers: distinct-colour count.</summary>
    public int DistinctColorCount => this.GetSourceStats().DistinctCount;
    /// <summary>Convenience projection for legacy callers: (R, G, B) histograms.</summary>
    public (int[] R, int[] G, int[] B) Histograms {
      get { var s = this.GetSourceStats(); return (s.R, s.G, s.B); }
    }

    private readonly object _statsGate = new object();
    private bool _statsComputed;
    private SourceStats _stats;


    public event EventHandler<Result> ThumbnailReady;

    /// <summary>Fires on the worker thread when a key is pulled from the pending queue and starts rendering.</summary>
    public event EventHandler<ThumbnailKey> ThumbnailStarted;

    public ThumbnailService() : this(0) { }

    /// <param name="workerCount">0 = auto (ProcessorCount/2, min 2). Multiple workers render thumbnails in parallel.</param>
    public ThumbnailService(int workerCount) {
      if (workerCount <= 0) workerCount = Math.Max(2, Environment.ProcessorCount / 2);
      this._workers = new Thread[workerCount];
      for (var i = 0; i < workerCount; ++i) {
        this._workers[i] = new Thread(this._WorkerLoop) {
          IsBackground = true,
          Name = "ThumbnailRender-" + i,
        };
        this._workers[i].Start();
      }
    }

    /// <summary>True if the given key is currently being rendered by a worker.</summary>
    public bool IsRendering(ThumbnailKey key) {
      lock (this._inFlightGate) return this._inFlight.Contains(key);
    }

    /// <summary>Replace the source image; invalidates all previously requested tiles (cache keyed on SourceVersion).</summary>
    public void ReplaceSource(Bitmap source) {
      lock (this._queueGate) {
        this._pending.Clear();
        Monitor.PulseAll(this._queueGate);
      }
      lock (this._cacheGate) {
        foreach (var kv in this._cache) kv.Value?.Dispose();
        this._cache.Clear();
      }
      lock (this._masterGate) {
        foreach (var kv in this._pools)
          if (kv.Value != null && kv.Value.IsValueCreated) kv.Value.Value.Dispose();
        this._pools.Clear();
      }
      lock (this._statsGate) {
        this._statsComputed = false;
        this._stats = default;
      }
      lock (this._paletteCacheGate) {
        this._cachedHistogram = null;
        this._cachedHistogramVersion = -1;
        this._paletteCache.Clear();
      }
      this.SourceBitmap = source;
      this.SourceVersion++;
    }

    /// <summary>
    /// Single-pass source analysis: walks every pixel once, updating a 2²⁴-bit RGB presence
    /// bitmap (for exact distinct-RGB counting, alpha ignored) plus R/G/B histograms. Runs with
    /// exactly one <c>LockBits(Format32bppArgb)</c> on the source — no HashSet, no sampling.
    /// <para>
    /// The bit array is 2 MiB flat; setting one bit per pixel is cache-friendly sequential
    /// work. Measured ≈ 1 s on 164 MP (12800² source), ≈ 5 ms on 0.2 MP — well under the
    /// cost of the HashSet variant for anything that actually has &gt; 256 colours.
    /// </para>
    /// </summary>
    private static unsafe SourceStats _ComputeSourceStats(Bitmap source) {
      var r = new int[256];
      var g = new int[256];
      var b = new int[256];
      if (source == null) return new SourceStats(0, r, g, b);

      var bounds = new Rectangle(0, 0, source.Width, source.Height);
      BitmapData data = null;
      try {
        data = source.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var stride = data.Stride;
        var rgbBits = new ulong[1 << (24 - 6)]; // 2²⁴ bits / 64-bit word = 262144 longs = 2 MiB
        var distinct = 0;
        var basePtr = (byte*)data.Scan0;
        for (var y = 0; y < source.Height; ++y) {
          var row = basePtr + y * stride;
          for (var x = 0; x < source.Width; ++x) {
            var px = x * 4;
            var bv = row[px + 0];
            var gv = row[px + 1];
            var rv = row[px + 2];
            b[bv]++;
            g[gv]++;
            r[rv]++;
            var rgb = (rv << 16) | (gv << 8) | bv;
            var bucket = rgb >> 6;
            var mask = 1UL << (rgb & 63);
            if ((rgbBits[bucket] & mask) == 0) {
              rgbBits[bucket] |= mask;
              ++distinct;
            }
          }
        }
        return new SourceStats(distinct, r, g, b);
      } finally {
        if (data != null) source.UnlockBits(data);
      }
    }

    /// <summary>Try to grab a rendered thumbnail from cache; <c>false</c> means "not ready, call <see cref="Request"/>".</summary>
    public bool TryGetCached(ThumbnailKey key, out Bitmap bitmap) {
      lock (this._cacheGate)
        return this._cache.TryGetValue(key, out bitmap);
    }

    /// <summary>Enqueue (or re-prioritise) a render request.</summary>
    public void Request(ThumbnailKey key, QuantizerDescriptor quantizer, DithererDescriptor ditherer, int priority) {
      lock (this._cacheGate)
        if (this._cache.ContainsKey(key))
          return;

      lock (this._queueGate) {
        if (this._pending.TryGetValue(key, out var existing)) {
          if (priority > existing.Priority) existing.Priority = priority;
          return;
        }
        this._pending[key] = new Work { Key = key, Quantizer = quantizer, Ditherer = ditherer, Priority = priority };
        Monitor.Pulse(this._queueGate);
      }
    }

    /// <summary>Bulk reprioritise — e.g. on scroll, pass a function that returns the new priority per key.</summary>
    public void Reprioritise(Func<ThumbnailKey, int> newPriorityFor) {
      lock (this._queueGate) {
        foreach (var work in this._pending.Values)
          work.Priority = newPriorityFor(work.Key);
        Monitor.PulseAll(this._queueGate);
      }
    }

    /// <summary>Drop every pending request that matches <paramref name="match"/>. Used when the ditherer strip is invalidated by a new quantizer pick.</summary>
    public void Invalidate(Predicate<ThumbnailKey> match) {
      lock (this._queueGate) {
        var victims = new List<ThumbnailKey>();
        foreach (var kv in this._pending)
          if (match(kv.Key)) victims.Add(kv.Key);
        foreach (var v in victims) this._pending.Remove(v);
      }
    }

    private Work _TakeHighest() {
      lock (this._queueGate) {
        while (!this._disposed) {
          Work best = null;
          foreach (var kv in this._pending)
            if (best == null || kv.Value.Priority > best.Priority) best = kv.Value;
          if (best != null) {
            this._pending.Remove(best.Key);
            return best;
          }
          Monitor.Wait(this._queueGate, TimeSpan.FromSeconds(1));
        }
        return null;
      }
    }

    private void _WorkerLoop() {
      while (!this._disposed) {
        var work = this._TakeHighest();
        if (work == null) return;

        lock (this._inFlightGate) this._inFlight.Add(work.Key);
        this.ThumbnailStarted?.Invoke(this, work.Key);

        Bitmap rendered = null;
        try {
          rendered = this._Render(work);
        } catch {
          // swallow — a broken quantizer shouldn't take down the worker.
        }

        Result finished = null;
        if (rendered != null)
          lock (this._cacheGate) {
            // Version race: if the source was replaced mid-render, throw the result away.
            // Disposed race: the service was torn down (window closed) mid-render; drop result.
            if (this._disposed || work.Key.SourceVersion != this.SourceVersion) {
              rendered.Dispose();
              rendered = null;
            } else {
              if (this._cache.TryGetValue(work.Key, out var previous)) previous?.Dispose();
              this._cache[work.Key] = rendered;
              finished = new Result { Key = work.Key, Bitmap = rendered };
            }
          }

        lock (this._inFlightGate) this._inFlight.Remove(work.Key);

        if (finished != null && !this._disposed)
          this.ThumbnailReady?.Invoke(this, finished);
      }
    }

    private Bitmap _Render(Work work) {
      var pool = this._GetOrBuildThumbnailPool(work.Key.PreviewSize);
      if (pool == null) return null;

      // Rent a pre-cloned master; GDI+ LockBits can't race because each worker owns its
      // own Bitmap instance for the duration of the call.
      var copy = pool.Rent();
      try {
        // Reuse the cached (histogram, palette) for this (sourceVersion, quantizer, paletteSize)
        // tuple. Every dither tile in the strip — and the detail preview — share the same
        // upstream (Quantizer × PaletteSize), so the histogram and palette are computed at most
        // once per source-version+quantizer combo regardless of how many ditherers are tried.
        var palette = this._GetOrComputePalette(copy, work.Key.SourceVersion, work.Quantizer, work.Key.PaletteSize);
        return UpstreamPipeline.ApplyPaletteWithDither(copy, palette, work.Ditherer);
      } finally {
        pool.Return(copy);
      }
    }

    /// <summary>Returns the palette for (sourceVersion, quantizer, paletteSize), reusing a
    /// cached one if available. The histogram cache is shared across all quantizers for a
    /// given source-version (it depends only on the source).</summary>
    private Color[] _GetOrComputePalette(Bitmap source, long sourceVersion, QuantizerDescriptor quantizer, ushort paletteSize) {
      var key = (sourceVersion, quantizer.Name, paletteSize);
      lock (this._paletteCacheGate) {
        if (this._paletteCache.TryGetValue(key, out var cached))
          return cached;
      }
      // Histogram is shared across quantizers — cache by SourceVersion.
      Dictionary<int, uint> histogram;
      lock (this._paletteCacheGate) {
        if (this._cachedHistogramVersion == sourceVersion && this._cachedHistogram != null) {
          histogram = this._cachedHistogram;
        } else {
          // Drop histogram outside the lock; ComputeHistogram does a per-pixel walk and we
          // don't want to block other workers building their palettes from the cached one.
          histogram = null;
        }
      }
      if (histogram == null) {
        histogram = UpstreamPipeline.ComputeHistogram(source);
        lock (this._paletteCacheGate) {
          // Late-write: another worker may have populated it concurrently — reuse if so.
          if (this._cachedHistogramVersion == sourceVersion && this._cachedHistogram != null)
            histogram = this._cachedHistogram;
          else {
            this._cachedHistogram = histogram;
            this._cachedHistogramVersion = sourceVersion;
          }
        }
      }
      var palette = UpstreamPipeline.ComputePalette(histogram, quantizer, paletteSize);
      lock (this._paletteCacheGate) {
        // Late-write race: keep whatever's already there; otherwise stash this one.
        if (this._paletteCache.TryGetValue(key, out var existing))
          return existing;
        this._paletteCache[key] = palette;
      }
      return palette;
    }

    /// <summary>
    /// Rents a detail-master bitmap from the pool keyed on <paramref name="maxSide"/>.
    /// The caller MUST call <see cref="ReturnDetailMaster"/> after use (wrap in try/finally).
    /// Returns <c>null</c> if the source is unset. Builds the pool lazily on first call.
    /// </summary>
    public Bitmap RentDetailMaster(int maxSide) {
      var pool = this._GetOrBuildDetailPool(maxSide);
      return pool?.Rent();
    }

    /// <summary>Returns a bitmap previously rented via <see cref="RentDetailMaster"/>.</summary>
    public void ReturnDetailMaster(Bitmap bmp) {
      if (bmp == null) return;
      // Scan detail pools (negative keys) for the one that owns this bitmap. No lock needed
      // — ConcurrentDictionary snapshot enumeration is thread-safe.
      foreach (var kv in this._pools) {
        if (kv.Key >= 0) continue;
        var lazy = kv.Value;
        if (lazy == null || !lazy.IsValueCreated) continue;
        if (lazy.Value.Owns(bmp)) { lazy.Value.Return(bmp); return; }
      }
      // Came from a pool that was already torn down (e.g. ReplaceSource raced with a render).
      // Dispose directly — leak-free fallback.
      bmp.Dispose();
    }

    /// <summary>Builds (or returns cached) the thumbnail pool for a given preview size.</summary>
    private _BitmapPool _GetOrBuildThumbnailPool(int previewSize) {
      var lazy = this._pools.GetOrAdd(previewSize, size => new Lazy<_BitmapPool>(
        () => _BuildThumbnailPool(this.SourceBitmap, size, this._workers.Length),
        LazyThreadSafetyMode.ExecutionAndPublication));
      return lazy.Value;
    }

    /// <summary>Builds (or returns cached) the detail-preview pool for a given max-side.</summary>
    private _BitmapPool _GetOrBuildDetailPool(int maxSide) {
      var lazy = this._pools.GetOrAdd(-maxSide, _ => new Lazy<_BitmapPool>(
        () => _BuildDetailPool(this.SourceBitmap, maxSide),
        LazyThreadSafetyMode.ExecutionAndPublication));
      return lazy.Value;
    }

    private static _BitmapPool _BuildThumbnailPool(Bitmap src, int previewSize, int workerCount) {
      if (src == null) return null;
      var master = new Bitmap(previewSize, previewSize, PixelFormat.Format32bppArgb);
      using (var g = Graphics.FromImage(master)) {
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        var ratio = Math.Min((float)previewSize / src.Width, (float)previewSize / src.Height);
        var w = Math.Max(1, (int)(src.Width * ratio));
        var h = Math.Max(1, (int)(src.Height * ratio));
        var x = (previewSize - w) / 2;
        var y = (previewSize - h) / 2;
        g.Clear(Color.Transparent);
        g.DrawImage(src, new Rectangle(x, y, w, h));
      }
      // One clone per worker → every concurrent thumbnail render owns its own Bitmap.
      var pool = new _BitmapPool(master, workerCount);
      master.Dispose();
      return pool;
    }

    private static _BitmapPool _BuildDetailPool(Bitmap src, int maxSide) {
      if (src == null) return null;
      var ratio = Math.Min(1.0, (double)maxSide / Math.Max(src.Width, src.Height));
      var w = Math.Max(1, (int)(src.Width * ratio));
      var h = Math.Max(1, (int)(src.Height * ratio));
      Bitmap master;
      if (ratio >= 1.0) {
        master = new Bitmap(src);
      } else {
        master = new Bitmap(w, h, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(master)) {
          g.InterpolationMode = InterpolationMode.HighQualityBicubic;
          g.PixelOffsetMode = PixelOffsetMode.HighQuality;
          g.DrawImage(src, 0, 0, w, h);
        }
      }
      // Detail renders are serialised by cancellation (new pick cancels old) so 2 clones
      // is enough — one in use, one available in case of overlap during cancellation.
      var pool = new _BitmapPool(master, 2);
      master.Dispose();
      return pool;
    }

    /// <summary>
    /// Fixed-capacity bitmap pool: pre-allocates N clones of a template bitmap on construction,
    /// hands them out via <see cref="Rent"/> / <see cref="Return"/>. Blocks (with a generous
    /// timeout) if the pool is exhausted so callers never get null during normal operation.
    /// </summary>
    private sealed class _BitmapPool : IDisposable {
      private readonly Stack<Bitmap> _available;
      private readonly List<Bitmap> _owned;     // every clone we minted — for Owns + Dispose
      private readonly SemaphoreSlim _slots;
      private readonly object _gate = new object();
      private bool _disposed;

      public _BitmapPool(Bitmap template, int count) {
        if (count < 1) count = 1;
        this._available = new Stack<Bitmap>(count);
        this._owned = new List<Bitmap>(count);
        this._slots = new SemaphoreSlim(count, count);
        for (var i = 0; i < count; ++i) {
          var clone = (Bitmap)template.Clone();
          this._available.Push(clone);
          this._owned.Add(clone);
        }
      }

      public Bitmap Rent() {
        this._slots.Wait();
        lock (this._gate) {
          if (this._disposed) return null;
          return this._available.Pop();
        }
      }

      public void Return(Bitmap bmp) {
        if (bmp == null) return;
        lock (this._gate) {
          if (this._disposed) { bmp.Dispose(); return; }
          this._available.Push(bmp);
        }
        this._slots.Release();
      }

      public bool Owns(Bitmap bmp) {
        lock (this._gate) {
          if (this._disposed) return false;
          return this._owned.Contains(bmp);
        }
      }

      public void Dispose() {
        lock (this._gate) {
          if (this._disposed) return;
          this._disposed = true;
          foreach (var b in this._owned) b?.Dispose();
          this._owned.Clear();
          this._available.Clear();
        }
        this._slots.Dispose();
      }
    }

    public void Dispose() {
      // Fire-and-forget: flip the disposed flag, wake every worker so they exit at the next
      // _TakeHighest / loop-top check, then return immediately. Joining here made window close
      // lag up to (workerCount × 2 s) while each thread finished whatever large render it was
      // mid-flight (ApplyQuantization on 2048² bitmaps is not cheap). Workers are
      // IsBackground=true so they won't prevent process exit; the cache/master cleanup below
      // races with their final writes but those guard on _disposed before touching state.
      this._disposed = true;
      lock (this._queueGate) Monitor.PulseAll(this._queueGate);
      lock (this._cacheGate) {
        foreach (var kv in this._cache) kv.Value?.Dispose();
        this._cache.Clear();
      }
      lock (this._masterGate) {
        foreach (var kv in this._pools)
          if (kv.Value != null && kv.Value.IsValueCreated) kv.Value.Value.Dispose();
        this._pools.Clear();
      }
      lock (this._inFlightGate) this._inFlight.Clear();
    }

    private sealed class Work {
      public ThumbnailKey Key;
      public QuantizerDescriptor Quantizer;
      public DithererDescriptor Ditherer;
      public int Priority;
    }
  }
}
