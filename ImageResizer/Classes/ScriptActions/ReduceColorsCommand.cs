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

using System.Drawing;
using Hawkynt.ColorProcessing.Dithering;
using Hawkynt.ColorProcessing.Quantization;
using Imager;
using Imager.Pipelines;

namespace Classes.ScriptActions {
  /// <summary>
  /// Palette-reduction pipeline step. Produces a new target image by running
  /// <see cref="UpstreamPipeline.ApplyQuantization"/> on the source with the user-selected
  /// quantizer + ditherer + palette size. Result lands in the target pane — the user then
  /// presses Switch to reuse it as the source for a subsequent scaler.
  /// </summary>
  internal sealed class ReduceColorsCommand : IScriptAction {
    public bool ChangesSourceImage => false;
    public bool ChangesTargetImage => true;
    public bool ProvidesNewGdiSource => false;

    public QuantizerDescriptor Quantizer { get; }
    public DithererDescriptor Ditherer { get; }
    public ushort PaletteSize { get; }

    public ReduceColorsCommand(QuantizerDescriptor quantizer, DithererDescriptor ditherer, ushort paletteSize) {
      this.Quantizer = quantizer;
      this.Ditherer = ditherer;
      this.PaletteSize = paletteSize;
    }

    public bool Execute() {
      var source = this.SourceImage;
      if (source == null || this.Quantizer == null)
        return false;

      using (var src = source.ToBitmap())
      using (var reduced = UpstreamPipeline.ApplyQuantization(src, this.Quantizer, this.Ditherer, this.PaletteSize))
        this.TargetImage = cImage.FromBitmap(reduced);

      return true;
    }

    public Bitmap GdiSource => null;
    public cImage SourceImage { get; set; }
    public cImage TargetImage { get; set; }
  }
}
