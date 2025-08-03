# 2D Image Filter

[![License](https://img.shields.io/badge/License-GPL_3.0-blue)](https://licenses.nuget.org/GPL-3.0-or-later)
![Language](https://img.shields.io/github/languages/top/Hawkynt/2dImagefilter?color=purple)

> A comprehensive collection of pixel art scaling algorithms for upscaling low-resolution computer and console graphics.

## 📖 Overview

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

## 🚀 Features

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

## 💻 Installation & Usage

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
ImageResizer.exe load input.png resize auto "HQ 2x" save output.png
ImageResizer.exe load sprite.bmp resize 400% "XBR 3x" save scaled_sprite.png
```

### Building from Source
```bash
# Clone the repository
git clone https://github.com/Hawkynt/2dimagefilter.git
cd 2dimagefilter

# Build the solution
dotnet build ImageResizer.sln -c Release

# Or build individual projects
dotnet build ImageResizerLibrary/ImageResizerLibrary.csproj
dotnet build ImageResizer/ImageResizer.csproj
```

## 📚 Documentation

### Command Line Usage
The application supports powerful command-line scripting:

```bash
# Basic usage
ImageResizer.exe load <input> resize <dimensions> <method> save <output>

# Dimension formats
resize auto "Scale2x"          # Auto-detect from algorithm
resize 320x240 "Bicubic"       # Specific dimensions  
resize w128 "HQ 2x"            # Width only (height auto)
resize h96 "Eagle"             # Height only (width auto)
resize 200% "XBR 3x"           # Percentage scaling

# Parameter examples
resize auto "Bicubic(radius=1.5,vbounds=wrap)"
resize 2x2 "HQ 2x(thresholds=1,repeat=2)"
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

## 📄 License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.

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
