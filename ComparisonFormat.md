# Comparison of Raster Image Format #

|     Table of Contents:   |
|:-------------------------|
|                          |

Image file formats are standardized means of organizing and storing digital images. Image files are composed of digital data in one of these formats that can be rasterized for use on a computer display or printer. An image file format may store data in uncompressed, compressed, or vector formats. Once rasterized, an image becomes a grid of pixels, each of which has a number of bits to designate its color equal to the color depth of the device displaying it.

Image Resizer supports lossless compression format, such as BMP, PNG, and GIF. Beside of that, Image Resizer also supports lossy compression format, such as JPEG or JPG.

## Lossless Compression ##
**Lossless** means that the image is made smaller, but at no detriment to the quality. There are also different colour depths (palettes): Indexed color and Direct color.

With Indexed it means that the image can only store a limited number of colours (usually 256) that are chosen by the image author, with Direct it means that you can store many thousands of colours that have not been chosen by the author.

### BMP - Lossless / Indexed and Direct ###

![https://lh4.googleusercontent.com/-TNyJZfX2E-M/UWAjWNePrOI/AAAAAAAAAjI/luCx_xqXvUo/s800/BMPvsGIF.png](https://lh4.googleusercontent.com/-TNyJZfX2E-M/UWAjWNePrOI/AAAAAAAAAjI/luCx_xqXvUo/s800/BMPvsGIF.png)

**BMP** is an old format. It is Lossless (no image data is lost on save) but there's also little to no compression at all, meaning saving as BMP results in VERY large file sizes. It can have palettes of both Indexed and Direct, but that's a small consolation. The file sizes are so unnecessarily large that nobody ever really uses this format.

Good for: Nothing really. There isn't anything BMP excels at, or isn't done better by other formats.

### GIF - Lossless / Indexed only ###

![https://lh6.googleusercontent.com/-bys_2vwYhHM/UWAjbBLZe9I/AAAAAAAAAjQ/6HLJ0370CB4/s800/GIFvsJPEG.png](https://lh6.googleusercontent.com/-bys_2vwYhHM/UWAjbBLZe9I/AAAAAAAAAjQ/6HLJ0370CB4/s800/GIFvsJPEG.png)

**GIF** uses lossless compression, meaning that you can save the image over and over and never lose any data. The file sizes are much smaller than BMP, because good compression is actually used, but it can only store an Indexed palette. This means that there can only be a maximum of 256 different colours in the file. That sounds like quite a small amount, and it is.

GIF images can also be animated and have transparency.

Good for: Logos, line drawings, and other simple images that need to be small. Only really used for websites.

### PNG-8 - Lossless / Indexed ###

![https://lh3.googleusercontent.com/-6nVIc1DdLfQ/UWAk8sy4ujI/AAAAAAAAAjg/tJUheUUPWsE/s800/PNG-8vsGIF.png](https://lh3.googleusercontent.com/-6nVIc1DdLfQ/UWAk8sy4ujI/AAAAAAAAAjg/tJUheUUPWsE/s800/PNG-8vsGIF.png)

**PNG** is a newer format, and PNG-8 (the indexed version of PNG) is really a good replacement for GIFs. Sadly, however, it has a few drawbacks: Firstly it cannot support animation like GIF can (well it can, but only Firefox seems to support it, unlike GIF animation which is supported by every browser). Secondly it has some support issues with older browsers like IE6. Thirdly, important software like Photoshop have very poor implementation of the format. (Damn you, Adobe!) PNG-8 can only store 256 colours, like GIFs.

Good for: The main thing that PNG-8 does better than GIFs is having support for Alpha Transparency.

### PNG-24 - Lossless / Direct ###

PNG-24 is a great format that combines Lossless encoding with Direct color (thousands of colours, just like JPEG). It's very much like BMP in that regard, except that PNG actually compresses images, so it results in much smaller files. Unfortunately PNG-24 files will still be much bigger than JPEGs, GIFs and PNG-8s, so you still need to consider if you really want to use one.

Even though PNG-24s allow thousands of colours while having compression, they are not intended to replace JPEG images. A photograph saved as a PNG-24 will likely be at least 5 times larger than a equivalent JPEG image, which very little improvement in visible quality. (Of course, this may be a desirable outcome if you're not concerned about filesize, and want to get the best quality image you can.)

Just like PNG-8, PNG-24 supports alpha-transparency, too.

## Lossy Compression ##

**Lossy** means the image is made (even) smaller, but at a detriment to the quality. Different from lossy compression, if an image is saved in a Lossy format over and over, the image quality would get progressively worse and worse.

### JPEG - Lossy / Direct ###

![https://lh3.googleusercontent.com/-qB_out6DkuI/UWAk5hh4ukI/AAAAAAAAAjY/XccBigjNB-w/s800/JPEGvsGIF.png](https://lh3.googleusercontent.com/-qB_out6DkuI/UWAk5hh4ukI/AAAAAAAAAjY/XccBigjNB-w/s800/JPEGvsGIF.png)

**JPEG** or **JPG** images were designed to make detailed photographic images as small as possible by removing information that the human eye won't notice. As a result it's a Lossy format, and saving the same file over and over will result in more data being lost over time. It has a palette of thousands of colours and so is great for photographs, but the lossy compression means it's bad for logos and line drawings: Not only will they look fuzzy, but such images will also have a larger file-size compared to GIFs!

Good for: Photographs. Also, gradients.