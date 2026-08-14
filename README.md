# 2D Image Filter

[![License](https://img.shields.io/github/license/Hawkynt/2dimagefilter)](https://github.com/Hawkynt/2dimagefilter/blob/main/LICENSE)
[![Language](https://img.shields.io/github/languages/top/Hawkynt/2dimagefilter?color=8957D5)](https://github.com/Hawkynt/2dimagefilter)

[![CI](https://github.com/Hawkynt/2dimagefilter/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Hawkynt/2dimagefilter/actions/workflows/ci.yml)
![Last Commit](https://img.shields.io/github/last-commit/Hawkynt/2dimagefilter?branch=main)
![Activity](https://img.shields.io/github/commit-activity/m/Hawkynt/2dimagefilter)

[![Stars](https://img.shields.io/github/stars/Hawkynt/2dimagefilter?color=FFD700)](https://github.com/Hawkynt/2dimagefilter/stargazers)
[![Forks](https://img.shields.io/github/forks/Hawkynt/2dimagefilter?color=008080)](https://github.com/Hawkynt/2dimagefilter/network/members)
[![Issues](https://img.shields.io/github/issues/Hawkynt/2dimagefilter)](https://github.com/Hawkynt/2dimagefilter/issues)
![Code Size](https://img.shields.io/github/languages/code-size/Hawkynt/2dimagefilter?color=4CAF50)
![Repo Size](https://img.shields.io/github/repo-size/Hawkynt/2dimagefilter?color=FF9800)

[![Release](https://img.shields.io/github/v/release/Hawkynt/2dimagefilter)](https://github.com/Hawkynt/2dimagefilter/releases/latest)
[![Nightly](https://img.shields.io/github/v/release/Hawkynt/2dimagefilter?include_prereleases&sort=date&filter=nightly-*&label=nightly&color=FF9800)](https://github.com/Hawkynt/2dimagefilter/releases)
[![Downloads](https://img.shields.io/github/downloads/Hawkynt/2dimagefilter/total)](https://github.com/Hawkynt/2dimagefilter/releases)

> A comprehensive collection of pixel art scaling algorithms for upscaling low-resolution computer and console graphics.

2D Image Filter is a powerful library that brings together the most popular image scaling algorithms specifically designed for pixel art and low-resolution graphics. Unlike traditional image scaling methods that often blur or distort pixel art, these algorithms preserve the crisp, clean aesthetic while intelligently enlarging images.

### 🎯 Project Goals

- **Algorithm Collection**: Gather all available pixel art scaling filters in one comprehensive library
- **Enhanced Flexibility**: Convert rigid color comparisons into parameterized "IsLike" functions
- **Wide Compatibility**: Support various graphics types with configurable similarity thresholds
- **Performance**: Optimized implementations using unsafe code for maximum speed

### 🔧 Key Innovation

Traditional scaling algorithms use hard-coded comparisons:
```csharp
(color1 == color2) ? color1 : color3
```

Our enhanced approach uses flexible similarity functions:
```csharp
(color1.IsLike(color2)) ? Interpolate(color1, color2) : color3
```

## ✨ Features

### 📦 Multiple Distribution Formats
- **Standalone Application**: GUI application for interactive image processing
- **Paint.NET Plugin**: Seamless integration with Paint.NET editor
- **Library**: .NET library for programmatic use in your applications

### 🎨 Supported Scaling Algorithms

#### Classic Pixel Art Scalers
- **Eagle Family**: Eagle 2x/3x, Super Eagle
- **SaI Family**: 2xSaI, Super2xSaI (Kreed/DOSBox)
- **Scale Family**: Scale2x/3x (MAME - Andrea Mazzoleni)
- **AdvInterp**: AdvInterp2x/3x (MAME)

#### High Quality Scalers  
- **HQ Family**: HQ2x/3x/4x (Maxim Stepin)
- **LQ Family**: LQ2x/3x/4x (SNES9x/AdvMAME)
- **nQ Family**: nQx Bold and Smart versions

#### Modern Advanced Scalers
- **XBR Family**: XBR2x/3x/4x Normal and NonBlend (Hyllian)
- **XBRz**: High quality scaling (Zenju)
- **Reverse AA**: Anti-aliasing filter (Hyllian)

#### Specialized Effects
- **CRT Effects**: MAME TV/RGB, Hawkynt TV effects
- **Scanlines**: Horizontal/vertical scanline effects
- **Bilinear Plus**: VBA enhanced bilinear filtering
- **FNES Filters**: DES, 2xSCL variants

#### Resampling Kernels
- Comprehensive collection of windowing functions
- Bicubic, Lanczos, and exotic mathematical kernels
- Support for custom radius and parameters

## 📦 Installation & Usage

### Prerequisites
- .NET Framework 4.5 or higher
- Windows Vista/7/8/10/11

### Quick Start

#### Option 1: Standalone Application
1. Download from [Releases](https://github.com/Hawkynt/2dimagefilter/releases)
2. Extract and run `ImageResizer.exe`
3. Load your image and select a scaling algorithm
4. Configure parameters and export the result

#### Option 2: Paint.NET Plugin
1. Download the Paint.NET plugin
2. Extract to your Paint.NET Effects folder
3. Restart Paint.NET
4. Find "Pixel Art Scaling" in the Effects menu

#### Option 3: Command Line Interface
```bash
ImageResizer.exe load input.png resize auto "Upscaler: HQ 2x" save output.png
ImageResizer.exe load sprite.bmp resize 400% "Upscaler: XBR 3x" save scaled_sprite.png
```

### Building from Source
```bash
# Clone the repository
git clone https://github.com/Hawkynt/2dimagefilter.git
cd 2dimagefilter

# Build the solution
dotnet build ImageResizer.slnx -c Release

# Or build individual projects
dotnet build ImageResizerLibrary/ImageResizerLibrary.csproj
dotnet build ImageResizer/ImageResizer.csproj
```

## 📚 Documentation

### Command Line Usage
The application supports powerful command-line scripting.

> **Filter names** use the category prefix shown in the GUI dropdown. Scaling prefixes are directional: `Upscaler: ` (integer upscalers such as HQ, XBR, Eagle, Scale), `Downscaler: ` (integer downscalers, future), `Resampler: ` (bidirectional arbitrary-ratio kernels such as Bicubic, Lanczos), `Downsampler: ` (downscale-only resamplers such as DPID, SSIM Downscale). Other categories: `Filter: `, `Plane: `, `Quantize: `, `Dither: `, `Blend: `. Pass the full label (prefix included) as the filter argument.

```bash
# Basic usage
ImageResizer.exe load <input> resize <dimensions> <method> save <output>

# Dimension formats
resize auto "Upscaler: Scale 2x"          # Auto-detect from algorithm
resize 320x240 "Resampler: Bicubic"       # Specific dimensions
resize w128 "Upscaler: HQ 2x"             # Width only (height auto)
resize h96 "Upscaler: Eagle"              # Height only (width auto)
resize 200% "Upscaler: XBR 3x"            # Percentage scaling

# Parameter examples
resize auto "Resampler: Bicubic(radius=1.5,vbounds=wrap)"
resize 2x2 "Upscaler: HQ 2x(thresholds=1,repeat=2)"
```

### Supported Parameters
- `radius`: Filter radius for resampling kernels
- `thresholds`: Enable/disable similarity thresholds
- `repeat`: Number of filter repetitions
- `vbounds`/`hbounds`: Out-of-bounds handling (const, half, whole, wrap, transparent)
- `centered`: Use centered grid for filtering

## 🎮 Perfect For

- **Pixel Art**: Classic video game sprites and artwork
- **Retro Gaming**: Emulator enhancement and ROM hacking
- **Digital Art**: Low-resolution artwork enlargement  
- **Game Development**: Asset upscaling for modern displays
- **Academic Research**: Comparative analysis of scaling algorithms

## 🏆 Algorithm Credits

This project implements algorithms from numerous sources:

- **Eagle/Super Eagle**: Derek Liauw Kie Fa (Kreed), ZSNES team
- **Scale2x/3x**: Andrea Mazzoleni (MAME)
- **HQ2x/3x/4x**: Maxim Stepin
- **XBR**: Hyllian
- **XBRz**: Zenju  
- **Resampling Kernels**: Pascal Getreuer
- **FNES Filters**: FNES emulator team
- **VBA Enhancements**: VBA-rr team

## 🤝 Contributing

Contributions are welcome! Whether you want to:
- Add new scaling algorithms
- Improve existing implementations
- Fix bugs or enhance performance
- Improve documentation

Please feel free to open an issue or submit a pull request.

## 📊 Downloads

- **Standalone Application**: [Download v2.0.0](https://github.com/Hawkynt/2dimagefilter/releases/download/2.0.0/Standalone.zip)
- **Paint.NET Plugin**: [Download v2.0.0](https://github.com/Hawkynt/2dimagefilter/releases/download/2.0.0/PaintDotNetPlugin.zip)

## ❤️ Support

If this project saves you time or money, consider supporting its development:

[![GitHub Sponsors](https://img.shields.io/badge/GitHub-Sponsor-EA4AAA?logo=githubsponsors)](https://github.com/sponsors/Hawkynt)
[![PayPal](https://img.shields.io/badge/PayPal-Donate-00457C?logo=paypal)](https://www.paypal.me/hawkynt)

## 📜 License

Licensed under LGPL-3.0-or-later — see [LICENSE](LICENSE).
