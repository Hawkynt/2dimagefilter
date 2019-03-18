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
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
// Compiler options:  /unsafe /optimize /debug- /target:library /out:"D:\_COPYAPPS\Paint.NET\Effects\Untitled.dll"
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
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
  public class PixelArtScalingEffectPlugin : PropertyBasedEffect {
    public static string StaticName => "2D Image Filter";

    public static string StaticSubMenu => "Tools";

    public static Image StaticIcon => Resources.App;

    public PixelArtScalingEffectPlugin(): base(StaticName, StaticIcon, StaticSubMenu, EffectFlags.Configurable) { }

    public enum PropertyNames {
      FilterType,
      FilterToClipboard
    }

    private string _filterName;
    private string _oldFilterName;
    private bool _copyToClipboard;
    private bool _oldCopyToClipboard;

    private Surface _lastFilteredImage;
    private Rectangle _lastTargetRectangle;

    private readonly object _locker = new object();


    protected override PropertyCollection OnCreatePropertyCollection() {
      return new PropertyCollection(
        new Property[] {
          new StaticListChoiceProperty(
            PropertyNames.FilterType,
            (from i in SupportedManipulators.Manipulators select (object)i.Item1).ToArray()),
          new Int32Property(PropertyNames.FilterToClipboard, 0,0,255), 
        });
    }

    protected override ControlInfo OnCreateConfigUI(PropertyCollection props) {
      const string BT_RENDER_TO_CLIPBOARD = "Render to Clipboard";
      const string DESCR_RENDER_TO_CLIPBOARD = "This will render the filter directly to the clipboard.";

      var defaultConfigUi = CreateDefaultConfigUI(props);
      defaultConfigUi.SetPropertyControlValue(
        PropertyNames.FilterType,
        ControlInfoPropertyNames.DisplayName,
        "Scaling Algorithm");

      defaultConfigUi.SetPropertyControlValue(
        PropertyNames.FilterToClipboard,
        ControlInfoPropertyNames.DisplayName,
        string.Empty);
      defaultConfigUi.SetPropertyControlType(
        PropertyNames.FilterToClipboard,
        PropertyControlType.IncrementButton
      );
      defaultConfigUi.SetPropertyControlValue(
        PropertyNames.FilterToClipboard,
        ControlInfoPropertyNames.ButtonText,
        BT_RENDER_TO_CLIPBOARD);
      defaultConfigUi.SetPropertyControlValue(
        PropertyNames.FilterToClipboard,
        ControlInfoPropertyNames.Description,
        DESCR_RENDER_TO_CLIPBOARD);
      return defaultConfigUi;
    }

    /// <summary>
    ///   Called whenever the UI for the plugin is changed somehow.  
    ///   May be called more than once for the same change; uses test-lock-test to prevent doing the same thing twice.
    ///   This code handles all of the resource allocation & initiates all of the computations needed for the plugin effect.
    /// </summary>
    /// <param name = "newToken"></param>
    /// <param name = "dstArgs"></param>
    /// <param name = "srcArgs"></param>
    protected override void OnSetRenderInfo(
      PropertyBasedEffectConfigToken newToken,
      RenderArgs dstArgs,
      RenderArgs srcArgs) {
      this._filterName = _GetFilterName(newToken);
      this._copyToClipboard = _IsCopyToClipboard(newToken);

      // if something has changed, this is not a 'phantom' method call due to threading
      if (this._filterName != this._oldFilterName || this._copyToClipboard != this._oldCopyToClipboard) {
        lock (this._locker) {
          if (this._filterName != this._oldFilterName) {
            this._oldFilterName = this._filterName;

            var filterParameters = SupportedManipulators.Manipulators.FirstOrDefault(i => i.Item1 == this._filterName);
            Debug.Assert(filterParameters!=null);
            
            var inputSurface = srcArgs.Surface;
            var sourceRectangle = this.EnvironmentParameters.GetSelection(inputSurface.Bounds).GetBoundsInt();
            var targetRectangle =new Rectangle(
              sourceRectangle.X * filterParameters.Item2.ScaleFactorX,
              sourceRectangle.Y * filterParameters.Item2.ScaleFactorY,
              sourceRectangle.Width* filterParameters.Item2.ScaleFactorX,
              sourceRectangle.Height * filterParameters.Item2.ScaleFactorY
            );
            
            var image = this._CreateImageFromSurface(inputSurface);
            var filtered = filterParameters.Item3(image, sourceRectangle);
            this._lastFilteredImage = this._CreateSurfaceFromImage(filtered, targetRectangle);
            this._lastTargetRectangle = targetRectangle;
          }
          if (this._copyToClipboard != this._oldCopyToClipboard) {
            this._oldCopyToClipboard = this._copyToClipboard;

            if (this._copyToClipboard)
              this._DirectToClipboard();
          }
        }
      }

      // pass along control
      base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
    }

    private static string _GetFilterName(PropertyBasedEffectConfigToken newToken) 
      => (string)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.FilterType).Value
    ;

    private static bool _IsCopyToClipboard(PropertyBasedEffectConfigToken newToken) 
      => newToken.GetProperty<Int32Property>(PropertyNames.FilterToClipboard).Value != 0
    ;

    private cImage _CreateImageFromSurface(Surface surface) 
      => cImage.FromBitmap(surface.CreateAliasedBitmap())
    ;

    private Surface _CreateSurfaceFromImage(cImage image, Rectangle rect) {
      var bitmap = image.ToBitmap();
      var selection = bitmap.Clone(rect, bitmap.PixelFormat);
      var result = Surface.CopyFromBitmap(selection);
      return result;
    }

    private void _DirectToClipboard() {
      var clipboardThread =
        new Thread(
          () => {
            Document.FromImage(this._lastFilteredImage.CreateAliasedBitmap());
            Clipboard.SetDataObject(this._lastFilteredImage.CreateAliasedBitmap(), true);
          });
      clipboardThread.SetApartmentState(ApartmentState.STA);
      clipboardThread.Start();
      clipboardThread.Join();
    }

    /// <summary>
    ///   Renders each region of interest (ROI)
    /// </summary>
    /// <param name = "rois"></param>
    /// <param name = "startIndex"></param>
    /// <param name = "length"></param>
    protected override void OnRender(Rectangle[] rois, int startIndex, int length) {
      if (length == 0 || this._lastFilteredImage == null)
        return;

      var selectedRectangle = this._lastTargetRectangle;
      for (var i = startIndex; i < startIndex + length; ++i)
        this._Render(this.DstArgs.Surface, rois[i], selectedRectangle);
    }

    /// <summary>
    /// Renders the given region of interest from the interal temporary surface to the given destination.
    /// Assumes the destination is large enough to accomodate the data.
    /// </summary>
    /// <param name="dst">the destination surface</param>
    /// <param name="rect">the RIO</param>
    /// <param name="selectionCopy">The selection copy.</param>
    private void _Render(Surface dst, Rectangle rect, Rectangle selectionCopy) {
      for (var y = rect.Top; y < rect.Bottom; y++) {
        for (var x = rect.Left; x < rect.Right; x++)
          dst[x, y] = this._lastFilteredImage[x - selectionCopy.Left, y - selectionCopy.Top];
      }
    }


    protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection props) {

      // Change the effect's window title
      var name =
        ((AssemblyProductAttribute)
          this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;
      var version = this.GetType().Assembly.GetName().Version;
      props[ControlInfoPropertyNames.WindowTitle].Value = name + " v" + version;
      base.OnCustomizeConfigUIWindowProperties(props);
    }
  }
}