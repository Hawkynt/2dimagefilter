# Image Resizer #

![https://lh3.googleusercontent.com/-tht7lZDifps/UVOYkmRZBvI/AAAAAAAAAfE/U4ZSlIUojoc/s640/Image%2520Resizer.png](https://lh3.googleusercontent.com/-tht7lZDifps/UVOYkmRZBvI/AAAAAAAAAfE/U4ZSlIUojoc/s640/Image%2520Resizer.png)

|     Table of Contents:   |
|:-------------------------|
|                          |

**Image resizer** is a free software designed for resizing image especially for upscaling low res pixel art image multiple times. This software contains many pixel art scaling algorithm that only can be found in video game emulators as image filter.

## Usage ##

### GUI ###

The user interface is very straightforward, you just:
  * Choose an image that you want to upscale;
  * Choose scaling method that you desire (I recommended use xBR 4x, but another method is also worth to be tried);
  * Set another parameter if necessary;
  * Save as your image.

### Command Line Interface ###

```
ImageResizer [/load <source>] [/resize <dimensions> <method>[(<repeat>|<paramlist>)]] [/save <target>] [/script <scriptfile>]...
```

Example:

`ImageResizer /load image_in.bmp /resize auto "XBR 3x(2,thresholds=0,vbounds=wrap)" /save imageout.png`

Notes:

|  `/load`          | Loads a file into the buffer.|
|:------------------|:-----------------------------|
|    `<source>`     | the source file to resize    |
|  `/save`          | Saves the image in the buffer to a file.|
|    `<target>`     | the target file to write     |
|  `/script`         | Executes a file containing more commands.|
|    `<scriptfile>`     | the script file to use       |
|  `/resize`        | Resizes the image in the buffer and stores the result back to the buffer.|
|    `<dimensions>` | `auto | <x>w | <y>h | <x>x<y> | <p>%`|
|                   |                   If only width or height is specified, the other dimension is auto-detected by aspect ratio|
|      `auto`       | determine target dimensions from used resizing filter|
|      `<x>`        | the final width in pixels for the target|
|      `<y>`        | the final height in pixels for the target|
|      `<p>`        | the percentage to resize eg 400% for 4-times resizing|
|    `<method>`     | the method to use            |
|    `<repeat>`     | the number of repetitions using this method|
|    `<paramlist>`  | a list of parameters to apply, separated using ',' and assigned using '='; eg. radius=4, centeredGrid=0|
|      `radius`     | a floating point value setting the radius of the filter|
|      `centered`   | 1 - use centered grid, 0 - do not use centered grid|
|      `thresholds` | 1 - use thresholds, 0 - do not use thresholds|
|      `repeat`     | a value 1-255 setting the number of repetitions to apply|
|      `vbounds`    | vertical out of bounds handling: const, half, whole, wrap|
|      `hbounds`    | horizontal out of bounds handling: const, half, whole, wrap|

You can load and process multiple files at once by loading after saving again, such as:

`ImageResizer /load 1.bmp /resize 10x10 Pixel /save 1.jpg /load 2.bmp /resize 10x10 Pixel /save 2.jpg`

You can also save to multiple files by adding another save parameter, such as:

`ImageResizer /load 1.bmp /resize 10x10 Pixel /save 1.jpg /save 2.jpg`

Even preprocessing using multiple filters is possible by adding another resize parameter, such as:

`ImageResizer /load 1.bmp /resize 10x10 Pixel /resize auto Scale2x /save 1.jpg`

## Limitations ##
The limitation of pixel art algorithms are they only capable for upscaling picture in predefined size, such as 2x, 3x, or 4x and cannot be used to shrink images. The solution when you want to resize your picture with custom size is to upscale the picture with the pixel art scaling algorithms first, then shrink it with Lanczos or another general purpose algorithm until the desired size.

Example: a pixel art image with dimension 2x2 pixel want to be scaled until 7x7 pixel (almost 4 times of its original size). So the solution is by using - let's say - Hq4x filter to magnify it until 8x8 pixel, then shrink it with lanczos filter to 7x7 pixel. The image will looks perfectly with this method.

## Comparison ##

|![https://lh6.googleusercontent.com/-Rg70-bYny_U/UVOYd43bWoI/AAAAAAAAAes/q-TDJw_Z5Bk/s800/Erick.png](https://lh6.googleusercontent.com/-Rg70-bYny_U/UVOYd43bWoI/AAAAAAAAAes/q-TDJw_Z5Bk/s800/Erick.png)|![https://lh3.googleusercontent.com/-weaoAT8G9nU/UVOYf2eA7bI/AAAAAAAAAe8/mxnz4VYVzrg/s800/Erick_4x.png](https://lh3.googleusercontent.com/-weaoAT8G9nU/UVOYf2eA7bI/AAAAAAAAAe8/mxnz4VYVzrg/s800/Erick_4x.png)|![https://lh4.googleusercontent.com/-ALsCKxYF99U/UVOYfJIMPGI/AAAAAAAAAe0/SZdYw4JXSbg/s800/Erick_4x(HqxSmart).png](https://lh4.googleusercontent.com/-ALsCKxYF99U/UVOYfJIMPGI/AAAAAAAAAe0/SZdYw4JXSbg/s800/Erick_4x(HqxSmart).png)|
|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|![https://lh3.googleusercontent.com/-s2yYs4pr49E/UVOYQJzzLDI/AAAAAAAAAd8/acGdEHwYI60/s800/Boss.png](https://lh3.googleusercontent.com/-s2yYs4pr49E/UVOYQJzzLDI/AAAAAAAAAd8/acGdEHwYI60/s800/Boss.png)  |![https://lh3.googleusercontent.com/-y5-DsdFOru0/UVOYR6WsB8I/AAAAAAAAAeE/4D7S7VBwW6U/s800/Boss_4x.png](https://lh3.googleusercontent.com/-y5-DsdFOru0/UVOYR6WsB8I/AAAAAAAAAeE/4D7S7VBwW6U/s800/Boss_4x.png)  |![https://lh3.googleusercontent.com/-tO3vuuhFF5Q/UVOYVAAHWWI/AAAAAAAAAeM/AsgwmGFaEYw/s800/Boss_4x(xBR).png](https://lh3.googleusercontent.com/-tO3vuuhFF5Q/UVOYVAAHWWI/AAAAAAAAAeM/AsgwmGFaEYw/s800/Boss_4x(xBR).png)            |

If there is any questions related the usage of this software and pixel art scaling algorithm, feel free to ask in the comment form below. Please don't mind to write your impression when using this software.