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

using PaintDotNet;
using PaintDotNet.Effects;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

using Imager;

namespace PixelArtScaling {

  public class PluginSupportInfo : IPluginSupportInfo {
    private readonly Type _thisType = typeof(PluginSupportInfo);

    public string Author => this._thisType.GetAssemblyAttribute<AssemblyCompanyAttribute>().Company;
    public string Copyright => this._thisType.GetAssemblyAttribute<AssemblyCopyrightAttribute>().Copyright;
    public string DisplayName => this._thisType.GetAssemblyAttribute<AssemblyProductAttribute>().Product;
    public Version Version => this._thisType.Assembly.GetName().Version;
    public Uri WebsiteUri => new Uri("https://github.com/Hawkynt/2dimagefilter");
  }

  [PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "2d Image Filter")]
  public sealed class PixelArtScalingEffectPlugin : Effect {
    public static string StaticName => "2D Image Filter";
    public static string StaticSubMenu => "Tools";
    public static Image StaticIcon => Resources.App;

    private static readonly Dictionary<string, ManipulatorEntry> _ENTRIES_BY_NAME =
      SupportedManipulators.Manipulators.ToDictionary(e => e.Name, e => e);

    public PixelArtScalingEffectPlugin() : base(StaticName, StaticIcon, StaticSubMenu, EffectFlags.Configurable) { }

    public override EffectConfigDialog CreateConfigDialog() => new PluginConfigDialog();

    private Surface _filteredSurface;
    private Rectangle _targetRectangle;
    private readonly object _renderLock = new object();
    private string _lastSignature;

    protected override void OnSetRenderInfo(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs) {
      var token = parameters as PluginConfigToken;
      if (token == null) {
        base.OnSetRenderInfo(parameters, dstArgs, srcArgs);
        return;
      }

      lock (this._renderLock) {
        var signature = _BuildSignature(token, srcArgs.Surface);
        if (signature == this._lastSignature && this._filteredSurface != null) {
          base.OnSetRenderInfo(parameters, dstArgs, srcArgs);
          return;
        }
        this._lastSignature = signature;

        if (!_ENTRIES_BY_NAME.TryGetValue(token.FilterName ?? string.Empty, out var entry))
          entry = SupportedManipulators.Manipulators[0];

        var sourceSurface = srcArgs.Surface;
        var sourceRect = this.EnvironmentParameters.GetSelection(sourceSurface.Bounds).GetBoundsInt();
        var (userW, userH) = _ResolveTargetSize(token, sourceRect.Width, sourceRect.Height);
        var targetRect = entry.ComputeTargetRectangle(sourceRect, userW, userH);

        var input = cImage.FromBitmap(sourceSurface.CreateAliasedBitmap());
        var options = new ResampleOptions(
          token.HorizontalOobMode,
          token.VerticalOobMode,
          token.CanvasColor,
          token.UseCenteredGrid
        );
        var filtered = entry.Apply(input, sourceRect, userW, userH, options);
        var newSurface = _CreateSurfaceFromImage(filtered, targetRect);

        var old = this._filteredSurface;
        this._filteredSurface = newSurface;
        this._targetRectangle = targetRect;
        old?.Dispose();
      }

      base.OnSetRenderInfo(parameters, dstArgs, srcArgs);
    }

    public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length) {
      Surface filtered;
      Rectangle target;
      lock (this._renderLock) {
        filtered = this._filteredSurface;
        target = this._targetRectangle;
      }
      if (filtered == null || length == 0) return;

      var dst = dstArgs.Surface;
      var fw = filtered.Width;
      var fh = filtered.Height;
      for (var i = startIndex; i < startIndex + length; ++i) {
        var rect = rois[i];
        for (var y = rect.Top; y < rect.Bottom; y++) {
          var sy = y - target.Top;
          if (sy < 0 || sy >= fh) continue;
          for (var x = rect.Left; x < rect.Right; x++) {
            var sx = x - target.Left;
            if (sx < 0 || sx >= fw) continue;
            dst[x, y] = filtered[sx, sy];
          }
        }
      }
    }

    protected override void OnDispose(bool disposing) {
      if (disposing) {
        this._filteredSurface?.Dispose();
        this._filteredSurface = null;
      }
      base.OnDispose(disposing);
    }

    private static bool _OobAppliesToEntry(ManipulatorEntry entry) {
      // Upstream-prefixed entries go through the Bitmap-based adapters that don't honour cImage OOB.
      var name = entry?.Name;
      if (string.IsNullOrEmpty(name)) return false;
      if (name.StartsWith("Scaler:") || name.StartsWith("Resampler:") || name.StartsWith("Filter:") || name.StartsWith("Plane:"))
        return false;
      return true;
    }

    private static string _BuildSignature(PluginConfigToken token, Surface src) {
      return string.Concat(
        token.FilterName, "|", (int)token.Mode, "|",
        token.PercentX, "/", token.PercentY, "|",
        token.FactorX.ToString("R"), "/", token.FactorY.ToString("R"), "|",
        token.TargetWidth, "x", token.TargetHeight, "|",
        token.LockAspectRatio ? "1" : "0", "|",
        (int)token.HorizontalOobMode, "/", (int)token.VerticalOobMode, "|",
        token.CanvasColor.ToArgb().ToString("X8"), "|",
        token.UseCenteredGrid ? "1" : "0", "|",
        src.Width, "x", src.Height
      );
    }

    private static (int width, int height) _ResolveTargetSize(PluginConfigToken token, int sourceW, int sourceH) {
      double sx, sy;
      switch (token.Mode) {
        case ScaleMode.Percent:
          sx = token.PercentX / 100.0;
          sy = token.LockAspectRatio ? sx : token.PercentY / 100.0;
          return (Math.Max(1, (int)Math.Round(sourceW * sx)), Math.Max(1, (int)Math.Round(sourceH * sy)));
        case ScaleMode.Factor:
          sx = token.FactorX;
          sy = token.LockAspectRatio ? sx : token.FactorY;
          return (Math.Max(1, (int)Math.Round(sourceW * sx)), Math.Max(1, (int)Math.Round(sourceH * sy)));
        case ScaleMode.Size:
        default: {
          var w = token.TargetWidth > 0 ? token.TargetWidth : sourceW;
          var h = token.TargetHeight > 0 ? token.TargetHeight : sourceH;
          if (token.LockAspectRatio && token.TargetWidth > 0 && sourceW > 0)
            h = Math.Max(1, (int)Math.Round((double)w * sourceH / sourceW));
          return (w, h);
        }
      }
    }

    private static Surface _CreateSurfaceFromImage(cImage image, Rectangle rect) {
      var bitmap = image.ToBitmap();
      var clipRect = new Rectangle(
        Math.Max(0, Math.Min(bitmap.Width - 1, rect.X)),
        Math.Max(0, Math.Min(bitmap.Height - 1, rect.Y)),
        Math.Max(1, Math.Min(bitmap.Width - rect.X, rect.Width)),
        Math.Max(1, Math.Min(bitmap.Height - rect.Y, rect.Height))
      );
      var selection = bitmap.Clone(clipRect, bitmap.PixelFormat);
      return Surface.CopyFromBitmap(selection);
    }
  }
}
