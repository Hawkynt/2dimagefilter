/**
 * @file imageio.c
 * @brief Implements ReadImage and WriteImage functions
 * @author Pascal Getreuer <getreuer@gmail.com>
 *
 * Two high-level functions are provided, \c ReadImage and \c WriteImage, for
 * reading and writing image BMP, JPEG, PNG, and TIFF files.  The desired
 * format of the image data can be specified to \c ReadImage for how to return
 * the data (and similarly to \c WriteImage for how it should interpret the
 * data).  Formatting options allow specifying the datatype of the components,
 * conversion to grayscale, channel ordering, interleaved vs. planar, and
 * row-major vs. column-major.
 *
 * \c ReadImage automatically detects the format of the image being read so
 * that the format does not need to be supplied explicitly.  \c WriteImage
 * infers the file format from the file extension.
 *
 * Also included is a function \c IdentifyImageType to guess the file type (BMP,
 * JPEG, PNG, TIFF, and a few other formats) from the file header's magic
 * numbers without reading the image.
 *
 * Support for BMP reading and writing is native: BMP reading supports 1-, 2-,
 * 4-, 8-, 16-, 32-bit uncompressed, RLE, and bitfield images; BMP writing is
 * limited to 24-bit uncompressed.  The implementation calls libjpeg, libpng,
 * and libtiff to handle JPEG, PNG, and TIFF images.
 *
 *
 * Copyright (c) 2010-2011, Pascal Getreuer
 * All rights reserved.
 *
 * This program is free software: you can use, modify and/or
 * redistribute it under the terms of the simplified BSD License. You
 * should have received a copy of this license along this program. If
 * not, see <http://www.opensource.org/licenses/bsd-license.html>.
 */

#include <string.h>
#include <ctype.h>
#include "imageio.h"

#ifdef LIBPNG_SUPPORT
#include <png.h>
#if PNG_LIBPNG_VER < 10400
/* For compatibility with older libpng */
#define png_set_expand_gray_1_2_4_to_8	png_set_gray_1_2_4_to_8
#endif
#endif
#ifdef LIBTIFF_SUPPORT
#include <tiffio.h>
#endif
#ifdef LIBJPEG_SUPPORT
#include <jpeglib.h>
#include <setjmp.h>
#endif

/** @brief Buffer size to use for BMP file I/O */
#define FILE_BUFFER_CAPACITY    (1024*4)

#define ROUNDCLAMPF(x)   ((x < 0.0f) ? 0 : \
    ((x > 1.0f) ? 255 : (uint8_t)(255.0f*(x) + 0.5f)))
#define ROUNDCLAMP(x)   ((x < 0.0) ? 0 : \
    ((x > 1.0) ? 255 : (uint8_t)(255.0*(x) + 0.5)))


/** @brief Case-insensitive test to see if String ends with Suffix */
static int StringEndsWith(const char *String, const char *Suffix)
{
    unsigned i, StringLength = strlen(String), SuffixLength = strlen(Suffix);

    if(StringLength < SuffixLength)
        return 0;

    String += StringLength - SuffixLength;

    for(i = 0; i < SuffixLength; i++)
        if(tolower(String[i]) != tolower(Suffix[i]))
            return 0;

    return 1;
}


/** @brief Fill an image with a color */
static void FillImage(uint32_t *Image, int Width, int Height, uint32_t Color)
{
    int x, y;

    if(Image)
        for(y = 0; y < Height; y++, Image += Width)
            for(x = 0; x < Width; x++)
                Image[x] = Color;
}


/**
 * @brief Check use of color and alpha, and count number of distinct colors
 * @param NumColors set by the routine to the number of unique colors
 * @param UseColor set to 1 if the image is not grayscale
 * @param UseAlpha set to 1 if the image alpha is not constant 255
 * @param Image pointer to U8 RGBA interleaved image data
 * @param Width, Height dimensions of the image
 * @return pointer to a color palette with NumColors entries or NULL if the
 * number of distinct colors exceeds 256.
 *
 * This routine checks whether an RGBA image makes use of color and alpha, and
 * constructs a palette if the number of distinct colors is 256 or fewer.  This
 * information is useful for writing image files with smaller file size.
 */
static uint32_t *GetImagePalette(int *NumColors, int *UseColor, int *UseAlpha,
    const uint32_t *Image, int Width, int Height)
{
    const int MaxColors = 256;
    uint32_t *Palette = NULL;
    uint32_t Pixel;
    int x, y, i, Red, Green, Blue, Alpha;


    if(!UseColor || !NumColors || !UseAlpha)
        return NULL;
    else if(!Image
        || !(Palette = (uint32_t *)Malloc(sizeof(uint32_t)*MaxColors)))
    {
        *NumColors = -1;
        *UseColor = *UseAlpha = 1;
        return NULL;
    }

    *NumColors = *UseColor = *UseAlpha = 0;

    for(y = 0; y < Height; y++)
    {
        for(x = 0; x < Width; x++)
        {
            Pixel = *(Image++);
            Red = ((uint8_t *)&Pixel)[0];
            Green = ((uint8_t *)&Pixel)[1];
            Blue = ((uint8_t *)&Pixel)[2];
            Alpha = ((uint8_t *)&Pixel)[3];

            if(Red != Green || Red != Blue)     /* Check color */
                *UseColor = 1;

            if(Alpha != 255)                    /* Check alpha */
                *UseAlpha = 1;

            /* Check Palette colors (if *NumColors != -1) */
            for(i = 0; i < *NumColors; i++)
                if(Pixel == Palette[i])
                    break;

            if(i == *NumColors)
            {
                if(i < MaxColors)
                {   /* Add new color to Palette */
                    Palette[i] = Pixel;
                    (*NumColors)++;
                }
                else
                {   /* Maximum size for Palette exceeded */
                    Free(Palette);
                    Palette = NULL;
                    *NumColors = -1;    /* Don't check Palette colors */
                }
            }
        }
    }

    return Palette;
}


/** @brief Read a 16-bit little Endian word from File */
static uint16_t ReadWordLE(FILE *File)
{
    uint16_t w;
    w = (uint16_t) getc(File);
    w |= ((uint16_t) getc(File) << 8);
    return w;
}


/** @brief Read a 32-bit little Endian double word from File */
static uint32_t ReadDWordLE(FILE *File)
{
    uint32_t dw;
    dw = (uint32_t) getc(File);
    dw |= ((uint32_t) getc(File) << 8);
    dw |= ((uint32_t) getc(File) << 16);
    dw |= ((uint32_t) getc(File) << 24);
    return dw;
}


/** @brief Write a 16-bit word in little Endian format */
static void WriteWordLE(uint16_t w, FILE *File)
{
    putc(w & 0xFF, File);
    putc((w & 0xFF00) >> 8, File);
}


/** @brief Write a 32-bit double word in little Endian format */
static void WriteDWordLE(uint32_t dw, FILE *File)
{
    putc(dw & 0xFF, File);
    putc((dw & 0xFF00) >> 8, File);
    putc((dw & 0xFF0000) >> 16, File);
    putc((dw & 0xFF000000) >> 24, File);
}


/** @brief Internal function for reading 1-bit BMP */
static int ReadBmp1Bit(uint32_t *Image, int Width, int Height, FILE *File, const uint32_t *Palette)
{
    int RowPadding = (-(Width+7)/8)&3;
    int x, y, Bit;
    unsigned Code;

    Image += ((long int)Width)*((long int)Height - 1);

    for(y = Height; y; y--, Image -= Width)
    {
        if(feof(File))
            return 0;

        for(x = 0; x < Width;)
        {
            Code = getc(File);

            for(Bit = 7; Bit >= 0 && x < Width; Bit--, Code <<= 1)
                Image[x++] = Palette[(Code & 0x80) ? 1:0];
        }

        for(x = RowPadding; x; x--)
            getc(File); /* Skip padding bytes at the end of the row */
    }

    return 1;
}


/** @brief Internal function for reading 4-bit BMP */
static int ReadBmp4Bit(uint32_t *Image, int Width, int Height, FILE *File, const uint32_t *Palette)
{
    int RowPadding = (-(Width+1)/2)&3;
    int x, y;
    unsigned Code;

    Image += ((long int)Width)*((long int)Height - 1);

    for(y = Height; y; y--, Image -= Width)
    {
        if(feof(File))
            return 0;

        for(x = 0; x < Width;)
        {
            Code = getc(File);
            Image[x++] = Palette[(Code & 0xF0) >> 4];

            if(x < Width)
                Image[x++] = Palette[Code & 0x0F];
        }

        for(x = RowPadding; x; x--)
            getc(File); /* Skip padding bytes at the end of the row */
    }

    return 1;
}


/** @brief Internal function for reading 4-bit RLE-compressed BMP */
static int ReadBmp4BitRle(uint32_t *Image, int Width, int Height, FILE *File, const uint32_t *Palette)
{
    int x, y, dy, k;
    unsigned Count, Value;
    uint32_t ColorH, ColorL;

    FillImage(Image, Width, Height, Palette[0]);
    Image += ((long int)Width)*((long int)Height - 1);

    for(x = 0, y = Height; y;)
    {
        if(feof(File))
            return 0;

        Count = getc(File);
        Value = getc(File);

        if(!Count)
        {	/* Count = 0 is the escape code */
            switch(Value)
            {
            case 0: 	/* End of line */
                Image -= Width;
                x = 0;
                y--;
                break;
            case 1: 	/* End of bitmap */
                return 1;
            case 2: 	/* Delta */
                x += getc(File);
                dy = getc(File);
                y -= dy;
                Image -= dy*Width;

                if(x >= Width || y < 0)
                    return 0;
                break;
            default:	/* Read a run of uncompressed data (Value = length of run) */
                Count = k = Value;

                if(x >= Width)
                    return 0;

                do
                {
                    Value = getc(File);
                    Image[x++] = Palette[(Value & 0xF0) >> 4];

                    if(x >= Width)
                        break;

                    if(--k)
                    {
                        Image[x++] = Palette[Value & 0x0F];
                        k--;

                        if(x >= Width)
                            break;
                    }
                }while(k);

                if(((Count + 1)/2) & 1)
                    getc(File); /* Padding for word align */
            }
        }
        else
        {	/* Run of pixels (Count = length of run) */
            ColorH = Palette[(Value & 0xF0) >> 4];
            ColorL = Palette[Value & 0xF];

            if(x >= Width)
                return 0;

            do
            {
                Image[x++] = ColorH;
                Count--;

                if(x >= Width)
                    break;

                if(Count)
                {
                    Image[x++] = ColorL;
                    Count--;

                    if(x >= Width)
                        break;
                }
            }while(Count);
        }
    }

    return 1;
}


/** @brief Internal function for reading 8-bit BMP */
static int ReadBmp8Bit(uint32_t *Image, int Width, int Height, FILE *File, const uint32_t *Palette)
{
    int RowPadding = (-Width)&3;
    int x, y;

    Image += ((long int)Width)*((long int)Height - 1);

    for(y = Height; y; y--, Image -= Width)
    {
        if(feof(File))
            return 0;

        for(x = 0; x < Width; x++)
            Image[x] = Palette[getc(File) & 0xFF];

        for(x = RowPadding; x; x--)
            getc(File); /* Skip padding bytes at the end of the row */
    }

    return 1;
}


/** @brief Internal function for reading 8-bit RLE-compressed BMP */
static int ReadBmp8BitRle(uint32_t *Image, int Width, int Height, FILE *File, const uint32_t *Palette)
{
    int x, y, dy, k;
    unsigned Count, Value;
    uint32_t Color;

    FillImage(Image, Width, Height, Palette[0]);
    Image += ((long int)Width)*((long int)Height - 1);

    for(x = 0, y = Height; y;)
    {
        if(feof(File))
            return 0;

        Count = getc(File);
        Value = getc(File);

        if(!Count)
        {	/* Count = 0 is the escape code */
            switch(Value)
            {
            case 0: 	/* End of line */
                Image -= Width;
                x = 0;
                y--;
                break;
            case 1: 	/* End of bitmap */
                return 1;
            case 2: 	/* Delta */
                x += getc(File);
                dy = getc(File);
                y -= dy;
                Image -= dy*Width;

                if(x >= Width || y < 0)
                    return 0;
                break;
            default:	/* Read a run of uncompressed data (Value = length of run) */
                Count = k = Value;

                do
                {
                    if(x >= Width)
                        break;

                    Image[x++] = Palette[getc(File) & 0xFF];
                }while(--k);

                if(Count&1)
                    getc(File); /* Padding for word align */
            }
        }
        else
        {	/* Run of pixels equal to Value (Count = length of run) */
            Color = Palette[Value & 0xFF];

            do
            {
                if(x >= Width)
                    break;

                Image[x++] = Color;
            }while(--Count);
        }
    }

    return 1;
}


/** @brief Internal function for reading 24-bit BMP */
static int ReadBmp24Bit(uint32_t *Image, int Width, int Height, FILE *File)
{
    uint8_t *ImagePtr = (uint8_t *)Image;
    int RowPadding = (-3*Width)&3;
    int x, y;


    Width <<= 2;
    ImagePtr += ((long int)Width)*((long int)Height - 1);

    for(y = Height; y; y--, ImagePtr -= Width)
    {
        if(feof(File))
            return 0;

        for(x = 0; x < Width; x += 4)
        {
            ImagePtr[x+3] = 255;        /* Set alpha            */
            ImagePtr[x+2] = getc(File); /* Read blue component  */
            ImagePtr[x+1] = getc(File); /* Read green component */
            ImagePtr[x+0] = getc(File); /* Read red component   */
        }

        for(x = RowPadding; x; x--)
            getc(File); /* Skip padding bytes at the end of the row */
    }

    return 1;
}

/** @brief Internal function for determining bit shifts in bitfield BMP */
static void GetMaskShifts(uint32_t Mask, int *LeftShift, int *RightShift)
{
    int Shift = 0, BitCount = 0;

    if(!Mask)
    {
        *LeftShift = 0;
        *RightShift = 0;
        return;
    }

    while(!(Mask & 1))	/* Find the first true bit */
    {
        Mask >>= 1;
        ++Shift;
    }

    /* Adjust the result for scaling to 8-bit quantities */
    while(Mask & 1)		/* Count the number of true bits */
    {
        Mask >>= 1;
        ++BitCount;
    }

    /* Compute a signed shift (right is positive) */
    Shift += BitCount - 8;

    if(Shift >= 0)
    {
        *LeftShift = 0;
        *RightShift = Shift;
    }
    else
    {
        *LeftShift = -Shift;
        *RightShift = 0;
    }
}

/** @brief Internal function for reading 16-bit BMP */
static int ReadBmp16Bit(uint32_t *Image, int Width, int Height, FILE *File,
    uint32_t RedMask, uint32_t GreenMask, uint32_t BlueMask, uint32_t AlphaMask)
{
    uint8_t *ImagePtr = (uint8_t *)Image;
    uint32_t Code;
    int RowPadding = (-2*Width)&3;
    int RedLeftShift, GreenLeftShift, BlueLeftShift, AlphaLeftShift;
    int RedRightShift, GreenRightShift, BlueRightShift, AlphaRightShift;
    int x, y;

    GetMaskShifts(RedMask, &RedLeftShift, &RedRightShift);
    GetMaskShifts(GreenMask, &GreenLeftShift, &GreenRightShift);
    GetMaskShifts(BlueMask, &BlueLeftShift, &BlueRightShift);
    GetMaskShifts(AlphaMask, &AlphaLeftShift, &AlphaRightShift);
    Width <<= 2;
    ImagePtr += ((long int)Width)*((long int)Height - 1);

    for(y = Height; y; y--, ImagePtr -= Width)
    {
        if(feof(File))
            return 0;

        for(x = 0; x < Width; x += 4)
        {
            Code = ReadWordLE(File);
            /* By the Windows 4.x BMP specification, color component masks must be contiguous
            [http://www.fileformat.info/format/bmp/egff.htm].  So we can decode the bitfields
            by bitwise AND with the mask and applying a bitshift.*/
            ImagePtr[x+3] = ((Code & AlphaMask) >> AlphaRightShift) << AlphaLeftShift;
            ImagePtr[x+2] = ((Code & BlueMask ) >> BlueRightShift ) << BlueLeftShift;
            ImagePtr[x+1] = ((Code & GreenMask) >> GreenRightShift) << GreenLeftShift;
            ImagePtr[x+0] = ((Code & RedMask  ) >> RedRightShift  ) << RedLeftShift;
        }

        for(x = RowPadding; x; x--)
            getc(File); /* Skip padding bytes at the end of the row */
    }

    return 1;
}


/** @brief Internal function for reading 32-bit BMP */
static int ReadBmp32Bit(uint32_t *Image, int Width, int Height, FILE *File,
    uint32_t RedMask, uint32_t GreenMask, uint32_t BlueMask, uint32_t AlphaMask)
{
    uint8_t *ImagePtr;
    uint32_t Code;
    int RedLeftShift, GreenLeftShift, BlueLeftShift, AlphaLeftShift;
    int RedRightShift, GreenRightShift, BlueRightShift, AlphaRightShift;
    int x, y;

    GetMaskShifts(RedMask, &RedLeftShift, &RedRightShift);
    GetMaskShifts(GreenMask, &GreenLeftShift, &GreenRightShift);
    GetMaskShifts(BlueMask, &BlueLeftShift, &BlueRightShift);
    GetMaskShifts(AlphaMask, &AlphaLeftShift, &AlphaRightShift);
    Width <<= 2;
    ImagePtr = (uint8_t *)Image + ((long int)Width)*((long int)Height - 1);

    for(y = Height; y; y--, ImagePtr -= Width)
    {
        if(feof(File))
            return 0;

        for(x = 0; x < Width; x += 4)
        {
            Code = ReadDWordLE(File);
            /* By the Windows 4.x BMP specification, color component masks must be contiguous
            [http://www.fileformat.info/format/bmp/egff.htm].  So we can decode the bitfields
            by bitwise AND with the mask and applying a bitshift.*/
            ImagePtr[x+3] = ((Code & AlphaMask) >> AlphaRightShift) << AlphaLeftShift;
            ImagePtr[x+2] = ((Code & BlueMask ) >> BlueRightShift ) << BlueLeftShift;
            ImagePtr[x+1] = ((Code & GreenMask) >> GreenRightShift) << GreenLeftShift;
            ImagePtr[x+0] = ((Code & RedMask  ) >> RedRightShift  ) << RedLeftShift;
        }
    }

    return 1;
}

/**
* @brief Read a BMP (Windows Bitmap) image file as RGBA data
*
* @param Image, Width, Height pointers to be filled with the pointer
*        to the image data and the image dimensions.
* @param File stdio FILE pointer pointing to the beginning of the BMP file
*
* @return 1 on success, 0 on failure
*
* This function is called by \c ReadImage to read BMP images.  Before calling
* \c ReadBmp, the caller should open \c File as a FILE pointer in binary read
* mode.  When \c ReadBmp is complete, the caller should close \c File.
*/
static int ReadBmp(uint32_t **Image, int *Width, int *Height, FILE *File)
{
    uint32_t *Palette = NULL;
    uint8_t *PalettePtr;
    long int ImageDataOffset, InfoSize;
    unsigned i, NumPlanes, BitsPerPixel, Compression, NumColors;
    uint32_t RedMask, GreenMask, BlueMask, AlphaMask;
    int Success = 0, Os2Bmp;
    uint8_t Magic[2];

    *Image = NULL;
    *Width = *Height = 0;
    fseek(File, 0, SEEK_SET);

    Magic[0] = getc(File);
    Magic[1] = getc(File);

    if(!(Magic[0] == 0x42 && Magic[1] == 0x4D) /* Verify the magic numbers */
        || fseek(File, 8, SEEK_CUR))         /* Skip the reserved fields */
    {
        ErrorMessage("Invalid BMP header.\n");
        goto Catch;
    }

    ImageDataOffset = ReadDWordLE(File);
    InfoSize = ReadDWordLE(File);

    /* Read the info header */
    if(InfoSize < 12)
    {
        ErrorMessage("Invalid BMP info header.\n");
        goto Catch;
    }

    if((Os2Bmp = (InfoSize == 12)))  /* This is an OS/2 V1 infoheader */
    {
        *Width = (int)ReadWordLE(File);
        *Height = (int)ReadWordLE(File);
        NumPlanes = (unsigned)ReadWordLE(File);
        BitsPerPixel = (unsigned)ReadWordLE(File);
        Compression = 0;
        NumColors = 0;
        RedMask = 0x00FF0000;
        GreenMask = 0x0000FF00;
        BlueMask = 0x000000FF;
        AlphaMask = 0xFF000000;
    }
    else
    {
        *Width = abs((int)ReadDWordLE(File));
        *Height = abs((int)ReadDWordLE(File));
        NumPlanes = (unsigned)ReadWordLE(File);
        BitsPerPixel = (unsigned)ReadWordLE(File);
        Compression = (unsigned)ReadDWordLE(File);
        fseek(File, 12, SEEK_CUR);
        NumColors = (unsigned)ReadDWordLE(File);
        fseek(File, 4, SEEK_CUR);
        RedMask = ReadDWordLE(File);
        GreenMask = ReadDWordLE(File);
        BlueMask = ReadDWordLE(File);
        AlphaMask = ReadDWordLE(File);
    }

    /* Check for problems or unsupported compression modes */
    if(*Width > MAX_IMAGE_SIZE || *Height > MAX_IMAGE_SIZE)
    {
        ErrorMessage("Image dimensions exceed MAX_IMAGE_SIZE.\n");
        goto Catch;
    }

    if(feof(File) || NumPlanes != 1 || Compression > 3)
        goto Catch;

    /* Allocate the image data */
    if(!(*Image = (uint32_t *)Malloc(sizeof(uint32_t)*((long int)*Width)*((long int)*Height))))
        goto Catch;

    /* Read palette */
    if(BitsPerPixel <= 8)
    {
        fseek(File, 14 + InfoSize, SEEK_SET);

        if(!NumColors)
            NumColors = 1 << BitsPerPixel;

        if(!(Palette = (uint32_t *)Malloc(sizeof(uint32_t)*256)))
            goto Catch;

        for(i = 0, PalettePtr = (uint8_t *)Palette; i < NumColors; i++)
        {
            PalettePtr[3] = 255;          /* Set alpha            */
            PalettePtr[2] = getc(File);   /* Read blue component  */
            PalettePtr[1] = getc(File);   /* Read green component */
            PalettePtr[0] = getc(File);   /* Read red component   */
            PalettePtr += 4;

            if(!Os2Bmp)
                getc(File); /* Skip extra byte (for non-OS/2 bitmaps) */
        }

        for(; i < 256; i++)  /* Fill the rest of the palette with the first color */
            Palette[i] = Palette[0];
    }

    if(fseek(File, ImageDataOffset, SEEK_SET) || feof(File))
    {
        ErrorMessage("File error.\n");
        goto Catch;
    }

    /*** Read the bitmap image data ***/
    switch(Compression)
    {
        case 0: /* Uncompressed data */
            switch(BitsPerPixel)
            {
            case 1: /* Read 1-bit uncompressed indexed data */
                Success = ReadBmp1Bit(*Image, *Width, *Height, File, Palette);
                break;
            case 4: /* Read 4-bit uncompressed indexed data */
                Success = ReadBmp4Bit(*Image, *Width, *Height, File, Palette);
                break;
            case 8: /* Read 8-bit uncompressed indexed data */
                Success = ReadBmp8Bit(*Image, *Width, *Height, File, Palette);
                break;
            case 24: /* Read 24-bit BGR image data */
                Success = ReadBmp24Bit(*Image, *Width, *Height, File);
                break;
            case 16: /* Read 16-bit data */
                Success = ReadBmp16Bit(*Image, *Width, *Height, File,
                    0x001F << 10, 0x001F << 5, 0x0001F, 0);
                break;
            case 32: /* Read 32-bit BGRA image data */
                Success = ReadBmp32Bit(*Image, *Width, *Height, File,
                    0x00FF0000, 0x0000FF00, 0x000000FF, 0xFF000000);
                break;
            }
            break;
        case 1: /* 8-bit RLE */
            if(BitsPerPixel == 8)
                Success = ReadBmp8BitRle(*Image, *Width, *Height, File, Palette);
            break;
        case 2: /* 4-bit RLE */
            if(BitsPerPixel == 4)
                Success = ReadBmp4BitRle(*Image, *Width, *Height, File, Palette);
            break;
        case 3: /* Bitfields data */
            switch(BitsPerPixel)
            {
            case 16: /* Read 16-bit bitfields data */
                Success = ReadBmp16Bit(*Image, *Width, *Height, File,
                    RedMask, GreenMask, BlueMask, AlphaMask);
                break;
            case 32: /* Read 32-bit bitfields data */
                Success = ReadBmp32Bit(*Image, *Width, *Height, File,
                    RedMask, GreenMask, BlueMask, AlphaMask);
                break;
            }
            break;
    }

    if(!Success)
        ErrorMessage("Error reading BMP data.\n");

Catch:	/* There was a problem, clean up and exit */
    if(Palette)
        Free(Palette);

    if(!Success && *Image)
        Free(*Image);

    return Success;
}


/**
* @brief Write a BMP image
*
* @param Image pointer to RGBA image data
* @param Width, Height the image dimensions
* @param File stdio FILE pointer
*
* @return 1 on success, 0 on failure
*
* This function is called by \c WriteImage to write BMP images.  The caller
* should open \c File in binary write mode.  When \c WriteBmp is complete,
* the caller should close \c File.
*
* The image is generally saved in uncompressed 24-bit RGB format.  But where
* possible, the image is saved using an 8-bit palette for a substantial
* decrease in file size.  The image data is always saved losslessly.
*
* @note The alpha channel is lost when saving to BMP.  It is possible to write
*       the alpha channel in a 32-bit BMP image, however, such images are not
*       widely supported.  RGB 24-bit BMP on the other hand is well supported.
*/
static int WriteBmp(const uint32_t *Image, int Width, int Height, FILE *File)
{
    const uint8_t *ImagePtr = (uint8_t *)Image;
    uint32_t *Palette = NULL;
    uint32_t Pixel;
    long int ImageSize;
    int UsePalette, NumColors, UseColor, UseAlpha;
    int x, y, i, RowPadding, Success = 0;


    if(!Image)
        return 0;

    Palette = GetImagePalette(&NumColors, &UseColor, &UseAlpha,
        Image, Width, Height);

    /* Decide whether to use 8-bit palette or 24-bit RGB format */
    if(Palette && 2*NumColors < Width*Height)
        UsePalette = 1;
    else
        UsePalette = NumColors = 0;

    /* Tell File to use buffering */
    setvbuf(File, 0, _IOFBF, FILE_BUFFER_CAPACITY);

    if(UsePalette)
    {
        RowPadding = (-Width)&3;
        ImageSize = (Width + RowPadding)*((long int)Height);
    }
    else
    {
        RowPadding = (-3*Width)&3;
        ImageSize = (3*Width + RowPadding)*((long int)Height);
    }

    /*** Write the header ***/

    /* Write the BMP header */
    putc(0x42, File);                       /* Magic numbers             */
    putc(0x4D, File);

    /* Filesize */
    WriteDWordLE(54 + 4*NumColors + ImageSize, File);

    WriteDWordLE(0, File);                  /* Reserved fields           */
    WriteDWordLE(54 + 4*NumColors, File);   /* Image data offset */

    /* Write the infoheader */
    WriteDWordLE(40, File);                 /* Infoheader size           */
    WriteDWordLE(Width, File);              /* Image width               */
    WriteDWordLE(Height, File);             /* Image height              */
    WriteWordLE(1, File);                   /* Number of colorplanes     */
    WriteWordLE((UsePalette) ? 8:24, File); /* Bits per pixel */
    WriteDWordLE(0, File);                  /* Compression method (none) */
    WriteDWordLE(ImageSize, File);          /* Image size                */
    WriteDWordLE(2835, File);               /* HResolution (2835=72dpi)  */
    WriteDWordLE(2835, File);               /* VResolution               */

    /* Number of colors */
    WriteDWordLE((!UsePalette || NumColors == 256) ? 0:NumColors, File);

    WriteDWordLE(0, File);                  /* Important colors          */

    if(ferror(File))
    {
        ErrorMessage("Error during write to file.\n");
        goto Catch;
    }

    if(UsePalette)
    {   /* Write the Palette */
        for(i = 0; i < NumColors; i++)
        {
            Pixel = Palette[i];
            putc(((uint8_t *)&Pixel)[2], File);     /* Blue   */
            putc(((uint8_t *)&Pixel)[1], File);     /* Green  */
            putc(((uint8_t *)&Pixel)[0], File);     /* Red    */
            putc(0, File);                          /* Unused */
        }
    }

    /* Write the image data */
    Width <<= 2;
    ImagePtr += ((long int)Width)*((long int)Height - 1);

    for(y = Height; y; y--, ImagePtr -= Width)
    {
        if(UsePalette)
        {   /* 8-bit palette image data */
            for(x = 0; x < Width; x += 4)
            {
                Pixel = *((uint32_t *)(ImagePtr + x));

                for(i = 0; i < NumColors; i++)
                    if(Pixel == Palette[i])
                        break;

                putc(i, File);
            }
        }
        else
        {   /* 24-bit RGB image data */
            for(x = 0; x < Width; x += 4)
            {
                putc(ImagePtr[x+2], File);  /* Write blue component  */
                putc(ImagePtr[x+1], File);  /* Write green component */
                putc(ImagePtr[x+0], File);  /* Write red component   */
            }
        }

        for(x = RowPadding; x; x--)         /* Write row padding */
            putc(0, File);
    }

    if(ferror(File))
    {
        ErrorMessage("Error during write to file.\n");
        goto Catch;
    }

    Success = 1;
Catch:
    if(Palette)
        Free(Palette);
    return Success;
}


#ifdef LIBJPEG_SUPPORT
/**
* @brief Struct that assists in customizing libjpeg error management
*
* This struct is used in combination with JerrExit (static function defined
* here in utiljpeg.c) to have control over how libjpeg errors are displayed.
*/
typedef struct{
    struct jpeg_error_mgr pub;
    jmp_buf jmpbuf;
} hooked_jerr;


/** @brief Callback for displaying libjpeg errors */
METHODDEF(void) JerrExit(j_common_ptr cinfo)
{
    hooked_jerr *Jerr = (hooked_jerr *) cinfo->err;
    (*cinfo->err->output_message)(cinfo);
    longjmp(Jerr->jmpbuf, 1);
}


/**
* @brief Read a JPEG (Joint Picture Experts Group) image file as RGBA data
*
* @param Image, Width, Height pointers to be filled with the pointer
*        to the image data and the image dimensions.
* @param File stdio FILE pointer pointing to the beginning of the BMP file
*
* @return 1 on success, 0 on failure
*
* This function is called by \c ReadImage to read JPEG images.  Before calling
* \c ReadJpeg, the caller should open \c File as a FILE pointer in binary read
* mode.  When \c ReadJpeg is complete, the caller should close \c File.
*/
static int ReadJpeg(uint32_t **Image, int *Width, int *Height, FILE *File)
{
    struct jpeg_decompress_struct cinfo;
    hooked_jerr Jerr;
    JSAMPARRAY Buffer;
    uint8_t *ImagePtr;
    unsigned i, RowSize;

    *Image = 0;
    *Width = *Height = 0;
    cinfo.err = jpeg_std_error(&Jerr.pub);
    Jerr.pub.error_exit = JerrExit;

    if(setjmp(Jerr.jmpbuf))
        goto Catch;	/* If this code is reached, libjpeg has signaled an error. */

    jpeg_create_decompress(&cinfo);
    jpeg_stdio_src(&cinfo, File);
    jpeg_read_header(&cinfo, 1);
    cinfo.out_color_space = JCS_RGB;   /* Ask for RGB image data */
    jpeg_start_decompress(&cinfo);
    *Width = (int)cinfo.output_width;
    *Height = (int)cinfo.output_height;

    if(*Width > MAX_IMAGE_SIZE || *Height > MAX_IMAGE_SIZE)
    {
        ErrorMessage("Image dimensions exceed MAX_IMAGE_SIZE.\n");
        jpeg_abort_decompress(&cinfo);
        goto Catch;
    }

    /* Allocate image memory */
    if(!(*Image = (uint32_t *)Malloc(sizeof(uint32_t)
        *((size_t)*Width)*((size_t)*Height))))
    {
        jpeg_abort_decompress(&cinfo);
        goto Catch;
    }

    /* Allocate a one-row-high array that will go away when done */
    RowSize = cinfo.output_width * cinfo.output_components;
    Buffer = (*cinfo.mem->alloc_sarray) ((j_common_ptr) &cinfo,
        JPOOL_IMAGE, RowSize, 1);
    ImagePtr = (uint8_t *)*Image;

    while(cinfo.output_scanline < cinfo.output_height)
        for(jpeg_read_scanlines(&cinfo, Buffer, 1), i = 0; i < RowSize; i += 3)
        {
            *(ImagePtr++) = Buffer[0][i];   /* Red   */
            *(ImagePtr++) = Buffer[0][i+1]; /* Green */
            *(ImagePtr++) = Buffer[0][i+2]; /* Blue  */
            *(ImagePtr++) = 0xFF;
        }

    jpeg_finish_decompress(&cinfo);
    jpeg_destroy_decompress(&cinfo);
    return 1;

Catch:
    if(*Image)
        Free(*Image);

    *Width = *Height = 0;
    jpeg_destroy_decompress(&cinfo);
    return 0;
}


/**
* @brief Write a JPEG image as RGB data
*
* @param Image pointer to RGBA image data
* @param Width, Height the image dimensions
* @param File stdio FILE pointer
*
* @return 1 on success, 0 on failure
*
* This function is called by \c WriteImage to write JPEG images.  The caller
* should open \c File in binary write mode.  When \c WriteJpeg is complete,
* the caller should close \c File.
*
* @note The alpha channel is lost when saving to JPEG since the JPEG format
*       does not support RGBA images.  (It is in principle possible to store
*       four channels in a JPEG as a CMYK image, but storing alpha this way
*       is strange.)
*/
static int WriteJpeg(const uint32_t *Image, int Width, int Height,
    FILE *File, int Quality)
{
    struct jpeg_compress_struct cinfo;
    hooked_jerr Jerr;
    uint8_t *Buffer = 0, *ImagePtr;
    unsigned i, RowSize;


    if(!Image)
        return 0;

    cinfo.err = jpeg_std_error(&Jerr.pub);
    Jerr.pub.error_exit = JerrExit;

    if(setjmp(Jerr.jmpbuf))
        goto Catch;	/* If this code is reached, libjpeg has signaled an error. */

    jpeg_create_compress(&cinfo);
    jpeg_stdio_dest(&cinfo, File);
    cinfo.image_width = Width;
    cinfo.image_height = Height;
    cinfo.input_components = 3;
    cinfo.in_color_space = JCS_RGB;
    jpeg_set_defaults(&cinfo);
    jpeg_set_quality(&cinfo, (Quality < 100) ? Quality : 100, 1);
    jpeg_start_compress(&cinfo, 1);

    RowSize = 3*Width;
    ImagePtr = (uint8_t *)Image;

    if(!(Buffer = (uint8_t *)Malloc(RowSize)))
        goto Catch;

    while(cinfo.next_scanline < cinfo.image_height)
    {
        for(i = 0; i < RowSize; i += 3)
        {
            Buffer[i] = ImagePtr[0];   /* Red   */
            Buffer[i+1] = ImagePtr[1]; /* Green */
            Buffer[i+2] = ImagePtr[2]; /* Blue  */
            ImagePtr += 4;
        }

        jpeg_write_scanlines(&cinfo, &Buffer, 1);
    }

    if(Buffer)
        Free(Buffer);

    jpeg_finish_compress(&cinfo);
    jpeg_destroy_compress(&cinfo);
    return 1;
Catch:
    if(Buffer)
        Free(Buffer);

    jpeg_destroy_compress(&cinfo);
    return 0;
}
#endif /* LIBJPEG_SUPPORT */


#ifdef LIBPNG_SUPPORT
/**
* @brief Read a PNG (Portable Network Graphics) image file as RGBA data
*
* @param Image, Width, Height pointers to be filled with the pointer
*        to the image data and the image dimensions.
* @param File stdio FILE pointer pointing to the beginning of the PNG file
*
* @return 1 on success, 0 on failure
*
* This function is called by \c ReadImage to read PNG images.  Before calling
* \c ReadPng, the caller should open \c File as a FILE pointer in binary read
* mode.  When \c ReadPng is complete, the caller should close \c File.
*/
static int ReadPng(uint32_t **Image, int *Width, int *Height, FILE *File)
{
    png_bytep *RowPointers;
    png_byte Header[8];
    png_structp Png;
    png_infop Info;
    png_uint_32 PngWidth, PngHeight;
    int BitDepth, ColorType, InterlaceType;
    unsigned Row;

    *Image = 0;
    *Width = *Height = 0;

    /* Check that file is a PNG file */
    if(fread(Header, 1, 8, File) != 8 || png_sig_cmp(Header, 0, 8))
        return 0;

    /* Read the info header */
    if(!(Png = png_create_read_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL))
        || !(Info = png_create_info_struct(Png)))
    {
        if(Png)
            png_destroy_read_struct(&Png, (png_infopp)NULL, (png_infopp)NULL);

        return 0;
    }

    if(setjmp(png_jmpbuf(Png)))
        goto Catch; /* If this code is reached, libpng has signaled an error. */

    png_init_io(Png, File);
    png_set_sig_bytes(Png, 8);
    png_set_user_limits(Png, MAX_IMAGE_SIZE, MAX_IMAGE_SIZE);
    png_read_info(Png, Info);
    png_get_IHDR(Png, Info, &PngWidth, &PngHeight, &BitDepth, &ColorType,
        &InterlaceType, (int*)NULL, (int*)NULL);
    *Width = (int)PngWidth;
    *Height = (int)PngHeight;

    /* Tell libpng to convert everything to 32-bit RGBA */
    if(ColorType == PNG_COLOR_TYPE_PALETTE)
        png_set_palette_to_rgb(Png);
    if(ColorType == PNG_COLOR_TYPE_GRAY && BitDepth < 8)
        png_set_expand_gray_1_2_4_to_8(Png);
    if(ColorType == PNG_COLOR_TYPE_GRAY || ColorType == PNG_COLOR_TYPE_GRAY_ALPHA)
        png_set_gray_to_rgb(Png);
    if(png_get_valid(Png, Info, PNG_INFO_tRNS))
        png_set_tRNS_to_alpha(Png);

    png_set_strip_16(Png);
    png_set_filler(Png, 0xFF, PNG_FILLER_AFTER);

    png_set_interlace_handling(Png);
    png_read_update_info(Png, Info);

    /* Allocate image memory and row pointers */
    if(!(*Image = (uint32_t *)Malloc(sizeof(uint32_t)
        *((size_t)*Width)*((size_t)*Height)))
        || !(RowPointers = (png_bytep *)Malloc(sizeof(png_bytep)
        *PngHeight)))
        goto Catch;

    for(Row = 0; Row < PngHeight; Row++)
        RowPointers[Row] = (png_bytep)(*Image + PngWidth*Row);

    /* Read the image data */
    png_read_image(Png, RowPointers);
    Free(RowPointers);
    png_destroy_read_struct(&Png, &Info, (png_infopp)NULL);
    return 1;

Catch:
    if(*Image)
        Free(*Image);

    *Width = *Height = 0;
    png_destroy_read_struct(&Png, &Info, (png_infopp)NULL);
    return 0;
}


/**
* @brief Write a PNG image
*
* @param Image pointer to RGBA image data
* @param Width, Height the image dimensions
* @param File stdio FILE pointer
*
* @return 1 on success, 0 on failure
*
* This function is called by \c WriteImage to write PNG images.  The caller
* should open \c File in binary write mode.  When \c WritePng is complete,
* the caller should close \c File.
*
* The image is written as 8-bit grayscale, indexed (PLTE), indexed with
* transparent colors (PLTE+tRNS), RGB, or RGBA data (in that order of
* preference) depending on the image data to encourage smaller file size.  The
* image data is always saved losslessly.  In principle, PNG can also make use
* of the pixel bit depth (1, 2, 4, 8, or 16) to reduce the file size further,
* but it is not done here.
*/
static int WritePng(const uint32_t *Image, int Width, int Height, FILE *File)
{
    const uint32_t *ImagePtr;
    uint32_t *Palette = NULL;
    uint8_t *RowBuffer;
    png_structp Png;
    png_infop Info;
    png_color PngPalette[256];
    png_byte PngTrans[256];
    uint32_t Pixel;
    int PngColorType, NumColors, UseColor, UseAlpha;
    int x, y, i, Success = 0;


    if(!Image)
        return 0;

    if(!(RowBuffer = (uint8_t *)Malloc(4*Width)))
        return 0;

    if(!(Png = png_create_write_struct(PNG_LIBPNG_VER_STRING,
        NULL, NULL, NULL))
        || !(Info = png_create_info_struct(Png)))
    {
        if(Png)
            png_destroy_write_struct(&Png, (png_infopp)NULL);

        Free(RowBuffer);
        return 0;
    }

    if(setjmp(png_jmpbuf(Png)))
    {   /* If this code is reached, libpng has signaled an error. */
        goto Catch;
    }

    /* Configure PNG output */
    png_init_io(Png, File);
    png_set_compression_level(Png, Z_BEST_COMPRESSION);

    Palette = GetImagePalette(&NumColors, &UseColor, &UseAlpha,
        Image, Width, Height);

    /* The PNG image is written according to the analysis of GetImagePalette */
    if(Palette && UseColor)
        PngColorType = PNG_COLOR_TYPE_PALETTE;
    else if(UseAlpha)
        PngColorType = PNG_COLOR_TYPE_RGB_ALPHA;
    else if(UseColor)
        PngColorType = PNG_COLOR_TYPE_RGB;
    else
        PngColorType = PNG_COLOR_TYPE_GRAY;

    png_set_IHDR(Png, Info, Width, Height, 8, PngColorType,
        PNG_INTERLACE_NONE, PNG_COMPRESSION_TYPE_BASE, PNG_FILTER_TYPE_BASE);

    if(PngColorType == PNG_COLOR_TYPE_PALETTE)
    {
        for(i = 0; i < NumColors; i++)
        {
            Pixel = Palette[i];
            PngPalette[i].red = ((uint8_t *)&Pixel)[0];
            PngPalette[i].green = ((uint8_t *)&Pixel)[1];
            PngPalette[i].blue = ((uint8_t *)&Pixel)[2];
            PngTrans[i] = ((uint8_t *)&Pixel)[3];
        }

        png_set_PLTE(Png, Info, PngPalette, NumColors);

        if(UseAlpha)
            png_set_tRNS(Png, Info, PngTrans, NumColors, NULL);
    }

    png_write_info(Png, Info);

    for(y = 0, ImagePtr = Image; y < Height; y++, ImagePtr += Width)
    {
        switch(PngColorType)
        {
        case PNG_COLOR_TYPE_RGB_ALPHA:
            png_write_row(Png, (png_bytep)Image);
            break;
        case PNG_COLOR_TYPE_RGB:
            for(x = 0; x < Width; x++)
            {
                Pixel = ImagePtr[x];
                RowBuffer[3*x + 0] = ((uint8_t *)&Pixel)[0];
                RowBuffer[3*x + 1] = ((uint8_t *)&Pixel)[1];
                RowBuffer[3*x + 2] = ((uint8_t *)&Pixel)[2];
            }

            png_write_row(Png, (png_bytep)RowBuffer);
            break;
        case PNG_COLOR_TYPE_GRAY:
            for(x = 0; x < Width; x++)
            {
                Pixel = ImagePtr[x];
                RowBuffer[x] = ((uint8_t *)&Pixel)[0];
            }

            png_write_row(Png, (png_bytep)RowBuffer);
            break;
        case PNG_COLOR_TYPE_PALETTE:
            for(x = 0; x < Width; x++)
            {
                Pixel = ImagePtr[x];

                for(i = 0; i < NumColors; i++)
                    if(Pixel == Palette[i])
                        break;

                RowBuffer[x] = i;
            }

            png_write_row(Png, (png_bytep)RowBuffer);
            break;
        }
    }

    png_write_end(Png, Info);
    Success = 1;
Catch:
    if(Palette)
        Free(Palette);
    png_destroy_write_struct(&Png, &Info);
    Free(RowBuffer);
    return Success;
}
#endif /* LIBPNG_SUPPORT */


#ifdef LIBTIFF_SUPPORT
/**
* @brief Read a TIFF (Tagged Information File Format) image file as RGBA data
*
* @param Image, Width, Height pointers to be filled with the pointer
*        to the image data and the image dimensions.
* @param File stdio FILE pointer pointing to the beginning of the PNG file
*
* @return 1 on success, 0 on failure
*
* This function is called by \c ReadImage to read TIFF images.  Before calling
* \c ReadTiff, the caller should open \c File as a FILE pointer in binary read
* mode.  When \c ReadTiff is complete, the caller should close \c File.
*/
static int ReadTiff(uint32_t **Image, int *Width, int *Height,
    const char *FileName, unsigned Directory)
{
    TIFF *Tiff;
    uint32 ImageWidth, ImageHeight;

    *Image = 0;
    *Width = *Height = 0;

    if(!(Tiff = TIFFOpen(FileName, "r")))
    {
        ErrorMessage("TIFFOpen failed to open file.\n");
        return 0;
    }

    TIFFSetDirectory(Tiff, Directory);
    TIFFGetField(Tiff, TIFFTAG_IMAGEWIDTH, &ImageWidth);
    TIFFGetField(Tiff, TIFFTAG_IMAGELENGTH, &ImageHeight);
    *Width = (int)ImageWidth;
    *Height = (int)ImageHeight;

    if(*Width > MAX_IMAGE_SIZE || *Height > MAX_IMAGE_SIZE)
    {
        ErrorMessage("Image dimensions exceed MAX_IMAGE_SIZE.\n");
        goto Catch;
    }

    if(!(*Image = (uint32_t *)Malloc(sizeof(uint32_t)*ImageWidth*ImageHeight)))
        goto Catch;

    if(!TIFFReadRGBAImageOriented(Tiff, ImageWidth, ImageHeight, *Image,
        ORIENTATION_TOPLEFT, 1))
        goto Catch;

    TIFFClose(Tiff);
    return 1;

Catch:
    if(*Image)
        Free(*Image);

    *Width = *Height = 0;
    TIFFClose(Tiff);
    return 0;
}


/**
* @brief Write a TIFF image as RGBA data
*
* @param Image pointer to RGBA image data
* @param Width, Height the image dimensions
* @param File stdio FILE pointer
*
* @return 1 on success, 0 on failure
*
* This function is called by \c WriteImage to write TIFF images.  The caller
* should open \c File in binary write mode.  When \c WriteTiff is complete,
* the caller should close \c File.
*/
static int WriteTiff(const uint32_t *Image, int Width, int Height,
    const char *FileName)
{
    TIFF *Tiff;
    uint16 Alpha = EXTRASAMPLE_ASSOCALPHA;

    if(!Image)
        return 0;

    if(!(Tiff = TIFFOpen(FileName, "w")))
    {
        ErrorMessage("TIFFOpen failed to open file.\n");
        return 0;
    }

    if(TIFFSetField(Tiff, TIFFTAG_IMAGEWIDTH, Width) != 1
        || TIFFSetField(Tiff, TIFFTAG_IMAGELENGTH, Height) != 1
        || TIFFSetField(Tiff, TIFFTAG_SAMPLESPERPIXEL, 4) != 1
        || TIFFSetField(Tiff, TIFFTAG_PHOTOMETRIC, PHOTOMETRIC_RGB) != 1
        || TIFFSetField(Tiff, TIFFTAG_EXTRASAMPLES, 1, &Alpha) != 1
        || TIFFSetField(Tiff, TIFFTAG_BITSPERSAMPLE, 8) != 1
        || TIFFSetField(Tiff, TIFFTAG_ORIENTATION, ORIENTATION_TOPLEFT) != 1
        || TIFFSetField(Tiff, TIFFTAG_PLANARCONFIG, PLANARCONFIG_CONTIG) != 1
        /* Compression can be COMPRESSION_NONE, COMPRESSION_DEFLATE,
        COMPRESSION_LZW, or COMPRESSION_JPEG */
        || TIFFSetField(Tiff, TIFFTAG_COMPRESSION, COMPRESSION_DEFLATE) != 1)
    {
        ErrorMessage("TIFFSetField failed.\n");
        TIFFClose(Tiff);
        return 0;
    }

    if(TIFFWriteEncodedStrip(Tiff, 0, (tdata_t)Image, 4*((size_t)Width)*((size_t)Height)) < 0)
    {
        ErrorMessage("Error writing data to file.\n");
        TIFFClose(Tiff);
        return 0;
    }

    TIFFClose(Tiff);
    return 1;
}
#endif /* LIBTIFF_SUPPORT */


/** @brief Convert from RGBA U8 to a specified format */
static void *ConvertToFormat(uint32_t *Src, int Width, int Height,
    unsigned Format)
{
    const int NumPixels = Width*Height;
    const int NumChannels = (Format & IMAGEIO_GRAYSCALE) ?
        1 : ((Format & IMAGEIO_STRIP_ALPHA) ? 3 : 4);
    const int ChannelStride = (Format & IMAGEIO_PLANAR) ? NumPixels : 1;
    const int ChannelStride2 = 2*ChannelStride;
    const int ChannelStride3 = 3*ChannelStride;
    double *DestD;
    float *DestF;
    uint8_t *DestU8;
    uint32_t Pixel;
    int Order[4] = {0, 1, 2, 3};
    int i, x, y, PixelStride, RowStride;


    PixelStride = (Format & IMAGEIO_PLANAR) ? 1 : NumChannels;

    if(Format & IMAGEIO_COLUMNMAJOR)
    {
        RowStride = PixelStride;
        PixelStride *= Height;
    }
    else
        RowStride = Width*PixelStride;

    if(Format & IMAGEIO_BGRFLIP)
    {
        Order[0] = 2;
        Order[2] = 0;
    }

    if((Format & IMAGEIO_AFLIP) && !(Format & IMAGEIO_STRIP_ALPHA))
    {
        Order[3] = Order[2];
        Order[2] = Order[1];
        Order[1] = Order[0];
        Order[0] = 3;
    }

    switch(Format & (IMAGEIO_U8 | IMAGEIO_SINGLE | IMAGEIO_DOUBLE))
    {
    case IMAGEIO_U8:  /* Destination type is uint8_t */
        if(!(DestU8  = (uint8_t *)Malloc(sizeof(uint8_t)*NumChannels*NumPixels)))
            return NULL;

        switch(NumChannels)
        {
        case 1: /* Convert RGBA U8 to grayscale U8 */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestU8[i] = (uint8_t)(0.299f*((uint8_t *)&Pixel)[0]
                        + 0.587f*((uint8_t *)&Pixel)[1]
                        + 0.114f*((uint8_t *)&Pixel)[2] + 0.5f);
                }
            break;
        case 3: /* Convert RGBA U8 to RGB (or BGR) U8 */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestU8[i] = ((uint8_t *)&Pixel)[Order[0]];
                    DestU8[i + ChannelStride] = ((uint8_t *)&Pixel)[Order[1]];
                    DestU8[i + ChannelStride2] = ((uint8_t *)&Pixel)[Order[2]];
                }
            break;
        case 4: /* Convert RGBA U8 to RGBA (or BGRA, ARGB, or ABGR) U8 */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestU8[i] = ((uint8_t *)&Pixel)[Order[0]];
                    DestU8[i + ChannelStride] = ((uint8_t *)&Pixel)[Order[1]];
                    DestU8[i + ChannelStride2] = ((uint8_t *)&Pixel)[Order[2]];
                    DestU8[i + ChannelStride3] = ((uint8_t *)&Pixel)[Order[3]];
                }
            break;
        }
        return DestU8;
    case IMAGEIO_SINGLE:  /* Destination type is float */
        if(!(DestF = (float *)Malloc(sizeof(float)*NumChannels*NumPixels)))
            return NULL;

        switch(NumChannels)
        {
        case 1: /* Convert RGBA U8 to grayscale float */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestF[i] = 1.172549019607843070675535e-3f*((uint8_t *)&Pixel)[0]
                        + 2.301960784313725357840079e-3f*((uint8_t *)&Pixel)[1]
                        + 4.470588235294117808150007e-4f*((uint8_t *)&Pixel)[2];
                }
            break;
        case 3: /* Convert RGBA U8 to RGB (or BGR) float */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestF[i] = ((uint8_t *)&Pixel)[Order[0]]/255.0f;
                    DestF[i + ChannelStride] = ((uint8_t *)&Pixel)[Order[1]]/255.0f;
                    DestF[i + ChannelStride2] = ((uint8_t *)&Pixel)[Order[2]]/255.0f;
                }
            break;
        case 4: /* Convert RGBA U8 to RGBA (or BGRA, ARGB, or ABGR) float */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestF[i] = ((uint8_t *)&Pixel)[Order[0]]/255.0f;
                    DestF[i + ChannelStride] = ((uint8_t *)&Pixel)[Order[1]]/255.0f;
                    DestF[i + ChannelStride2] = ((uint8_t *)&Pixel)[Order[2]]/255.0f;
                    DestF[i + ChannelStride3] = ((uint8_t *)&Pixel)[Order[3]]/255.0f;
                }
            break;
        }
        return DestF;
    case IMAGEIO_DOUBLE:  /* Destination type is double */
        if(!(DestD = (double *)Malloc(sizeof(double)*NumChannels*NumPixels)))
            return NULL;

        switch(NumChannels)
        {
        case 1: /* Convert RGBA U8 to grayscale double */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestD[i] = 1.172549019607843070675535e-3*((uint8_t *)&Pixel)[0]
                        + 2.301960784313725357840079e-3*((uint8_t *)&Pixel)[1]
                        + 4.470588235294117808150007e-4*((uint8_t *)&Pixel)[2];
                }
            break;
        case 3: /* Convert RGBA U8 to RGB (or BGR) double */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestD[i] = ((uint8_t *)&Pixel)[Order[0]]/255.0;
                    DestD[i + ChannelStride] = ((uint8_t *)&Pixel)[Order[1]]/255.0;
                    DestD[i + ChannelStride2] = ((uint8_t *)&Pixel)[Order[2]]/255.0;
                }
            break;
        case 4: /* Convert RGBA U8 to RGBA (or BGRA, ARGB, or ABGR) double */
            for(y = 0; y < Height; y++, Src += Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    Pixel = Src[x];
                    DestD[i] = ((uint8_t *)&Pixel)[Order[0]]/255.0;
                    DestD[i + ChannelStride] = ((uint8_t *)&Pixel)[Order[1]]/255.0;
                    DestD[i + ChannelStride2] = ((uint8_t *)&Pixel)[Order[2]]/255.0;
                    DestD[i + ChannelStride3] = ((uint8_t *)&Pixel)[Order[3]]/255.0;
                }
            break;
        }
        return DestD;
    default:
        return NULL;
    }
}


/** @brief Convert from a specified format to RGBA U8 */
static uint32_t *ConvertFromFormat(void *Src, int Width, int Height,
    unsigned Format)
{
    const int NumPixels = Width*Height;
    const int NumChannels = (Format & IMAGEIO_GRAYSCALE) ?
        1 : ((Format & IMAGEIO_STRIP_ALPHA) ? 3 : 4);
    const int ChannelStride = (Format & IMAGEIO_PLANAR) ? NumPixels : 1;
    const int ChannelStride2 = 2*ChannelStride;
    const int ChannelStride3 = 3*ChannelStride;
    double *SrcD = (double *)Src;
    float *SrcF = (float *)Src;
    uint8_t *SrcU8 = (uint8_t *)Src;
    uint8_t *Dest, *DestPtr;
    int Order[4] = {0, 1, 2, 3};
    int i, x, y, PixelStride, RowStride;


    if(!(Dest = (uint8_t *)Malloc(sizeof(uint32_t)*NumPixels)))
        return NULL;

    DestPtr = Dest;
    PixelStride = (Format & IMAGEIO_PLANAR) ? 1 : NumChannels;

    if(Format & IMAGEIO_COLUMNMAJOR)
    {
        RowStride = PixelStride;
        PixelStride *= Height;
    }
    else
        RowStride = Width*PixelStride;

    if(Format & IMAGEIO_BGRFLIP)
    {
        Order[0] = 2;
        Order[2] = 0;
    }

    if((Format & IMAGEIO_AFLIP) && !(Format & IMAGEIO_STRIP_ALPHA))
    {
        Order[3] = Order[2];
        Order[2] = Order[1];
        Order[1] = Order[0];
        Order[0] = 3;
    }

    switch(Format & (IMAGEIO_U8 | IMAGEIO_SINGLE | IMAGEIO_DOUBLE))
    {
    case IMAGEIO_U8:  /* Source type is uint8_t */
        switch(NumChannels)
        {
        case 1: /* Convert grayscale U8 to RGBA U8 */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x] =
                    DestPtr[4*x + 1] =
                    DestPtr[4*x + 2] = SrcU8[i];
                    DestPtr[4*x + 3] = 255;
                }
            break;
        case 3: /* Convert RGB (or BGR) U8 to RGBA U8 */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x + Order[0]] = SrcU8[i];
                    DestPtr[4*x + Order[1]] = SrcU8[i + ChannelStride];
                    DestPtr[4*x + Order[2]] = SrcU8[i + ChannelStride2];
                    DestPtr[4*x + 3] = 255;
                }
            break;
        case 4: /* Convert RGBA U8 to RGBA (or BGRA, ARGB, or ABGR) U8 */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x + Order[0]] = SrcU8[i];
                    DestPtr[4*x + Order[1]] = SrcU8[i + ChannelStride];
                    DestPtr[4*x + Order[2]] = SrcU8[i + ChannelStride2];
                    DestPtr[4*x + Order[3]] = SrcU8[i + ChannelStride3];
                }
            break;
        }
        break;
    case IMAGEIO_SINGLE:  /* Source type is float */
        switch(NumChannels)
        {
        case 1: /* Convert grayscale float to RGBA U8 */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x] =
                    DestPtr[4*x + 1] =
                    DestPtr[4*x + 2] = ROUNDCLAMPF(SrcF[i]);
                    DestPtr[4*x + 3] = 255;
                }
            break;
        case 3: /* Convert RGBA U8 to RGB (or BGR) float */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x + Order[0]] = ROUNDCLAMPF(SrcF[i]);
                    DestPtr[4*x + Order[1]] = ROUNDCLAMPF(SrcF[i + ChannelStride]);
                    DestPtr[4*x + Order[2]] = ROUNDCLAMPF(SrcF[i + ChannelStride2]);
                    DestPtr[4*x + 3] = 255;
                }
            break;
        case 4: /* Convert RGBA U8 to RGBA (or BGRA, ARGB, or ABGR) float */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x + Order[0]] = ROUNDCLAMPF(SrcF[i]);
                    DestPtr[4*x + Order[1]] = ROUNDCLAMPF(SrcF[i + ChannelStride]);
                    DestPtr[4*x + Order[2]] = ROUNDCLAMPF(SrcF[i + ChannelStride2]);
                    DestPtr[4*x + Order[3]] = ROUNDCLAMPF(SrcF[i + ChannelStride3]);
                }
            break;
        }
        break;
    case IMAGEIO_DOUBLE:  /* Source type is double */
        switch(NumChannels)
        {
        case 1: /* Convert grayscale double to RGBA U8 */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x] =
                    DestPtr[4*x + 1] =
                    DestPtr[4*x + 2] = ROUNDCLAMP(SrcD[i]);
                    DestPtr[4*x + 3] = 255;
                }
            break;
        case 3: /* Convert RGB (or BGR) double to RGBA U8 */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x + Order[0]] = ROUNDCLAMP(SrcD[i]);
                    DestPtr[4*x + Order[1]] = ROUNDCLAMP(SrcD[i + ChannelStride]);
                    DestPtr[4*x + Order[2]] = ROUNDCLAMP(SrcD[i + ChannelStride2]);
                    DestPtr[4*x + 3] = 255;;
                }
            break;
        case 4: /* Convert RGBA (or BGRA, ARGB, or ABGR) double to RGBA U8 */
            for(y = 0; y < Height; y++, DestPtr += 4*Width)
                for(x = 0, i = RowStride*y; x < Width; x++, i += PixelStride)
                {
                    DestPtr[4*x + Order[0]] = ROUNDCLAMP(SrcD[i]);
                    DestPtr[4*x + Order[1]] = ROUNDCLAMP(SrcD[i + ChannelStride]);
                    DestPtr[4*x + Order[2]] = ROUNDCLAMP(SrcD[i + ChannelStride2]);
                    DestPtr[4*x + Order[3]] = ROUNDCLAMP(SrcD[i + ChannelStride3]);
                }
            break;
        }
        break;
    default:
        return NULL;
    }

    return (uint32_t *)Dest;
}


/**
 * @brief Identify the file type of an image file by its magic numbers
 * @param Type destination buffer with space for at least 5 chars
 * @param FileName image file name
 * @return 1 on successful identification, 0 on failure.
 *
 * The routine fills Type with an identifying string.  If there is an error
 * or the file type is unknown, Type is set to a null string.
 */
int IdentifyImageType(char *Type, const char *FileName)
{
    FILE *File;
    uint32_t Magic;


    Type[0] = '\0';

    if(!(File = fopen(FileName, "rb")))
        return 0;

    /* Determine the file format by reading the first 4 bytes */
    Magic = ((uint32_t)getc(File));
    Magic |= ((uint32_t)getc(File)) << 8;
    Magic |= ((uint32_t)getc(File)) << 16;
    Magic |= ((uint32_t)getc(File)) << 24;

    /* Test for errors */
    if(ferror(File))
    {
        fclose(File);
        return 0;
    }

    fclose(File);

    if((Magic & 0x0000FFFFL) == 0x00004D42L)                /* BMP */
        strcpy(Type, "BMP");
    else if((Magic & 0x00FFFFFFL) == 0x00FFD8FFL)           /* JPEG/JFIF */
        strcpy(Type, "JPEG");
    else if(Magic == 0x474E5089L)                           /* PNG */
        strcpy(Type, "PNG");
    else if(Magic == 0x002A4949L || Magic == 0x2A004D4DL)   /* TIFF */
        strcpy(Type, "TIFF");
    else if(Magic == 0x38464947L)                           /* GIF */
        strcpy(Type, "GIF");
    else if(Magic == 0x474E4D8AL)                           /* MNG */
        strcpy(Type, "MNG");
    else if((Magic & 0xF0FF00FFL) == 0x0001000AL            /* PCX */
        && ((Magic >> 8) & 0xFF) < 6)
        strcpy(Type, "PCX");
    else
        return 0;

    return 1;
}


/**
* @brief Read an image file as 32-bit RGBA data
*
* @param Width, Height pointers to be filled with the image dimensions
* @param FileName image file name
* @param Format specifies the desired format for the image
*
* @return Pointer to the image data, or null on failure
*
* The calling syntax is that the filename is the input and \c Width,
* and \c Height and the returned pointer are outputs.  \c ReadImage allocates
* memory for the image as one contiguous block of memory and returns a
* pointer.  It is the responsibility of the caller to call \c Free on this
* pointer when done to release this memory.
*
* A non-null pointer indicates success.  On failure, the returned pointer
* is null, and \c Width and \c Height are set to 0.
*
* The Format argument is used by specifying one of the data type options
*
*  - IMAGEIO_U8:            unsigned 8-bit components
*  - IMAGEIO_SINGLE:        float components
*  - IMAGEIO_DOUBLE:        double components
*
* and one of the channel options
*
*  - IMAGEIO_GRAYSCALE:     grayscale data
*  - IMAGEIO_RGB:           RGB color data (red is the first channel)
*  - IMAGEIO_BGR:           BGR color data (blue is the first channel)
*  - IMAGEIO_RGBA:          RGBA color+alpha data
*  - IMAGEIO_BGRA:          BGRA color+alpha data
*  - IMAGEIO_ARGB:          ARGB color+alpha data
*  - IMAGEIO_ABGR:          ABGR color+alpha data
*
* and optionally either or both of the ordering options
*
*  - IMAGEIO_PLANAR:        planar order instead of interleaved components
*  - IMAGEIO_COLUMNMAJOR:   column major order instead of row major order
*
@code
    uint32_t *Image;
    int Width, Height;

    if(!(Image = (uint32_t *)ReadImage(&Width, &Height, "myimage.bmp",
        IMAGEIO_U8 | IMAGEIO_RGBA)))
        return 0;

    printf("Read image of size %dx%d\n", Width, Height);

    ...

    Free(Image);
@endcode
*
* With the default formatting IMAGEIO_U8 | IMAGEIO_RGBA, the image is
* organized in standard row major top-down 32-bit RGBA order.  The image
* is organized as
@verbatim
    (Top left)                                             (Top right)
    Image[0]                Image[1]        ...  Image[Width-1]
    Image[Width]            Image[Width+1]  ...  Image[2*Width]
    ...                     ...             ...  ...
    Image[Width*(Height-1)] ...             ...  Image[Width*Height-1]
    (Bottom left)                                       (Bottom right)
@endverbatim
* Each element \c Image[k] represents one RGBA pixel, which is a 32-bit
* bitfield.  The components of pixel \c Image[k] can be unpacked as
@code
    uint8_t *Component = (uint8_t *)&Image[k];
    uint8_t Red = Component[0];
    uint8_t Green = Component[1];
    uint8_t Blue = Component[2];
    uint8_t Alpha = Component[3];
@endcode
* Each component is an unsigned 8-bit integer value with range 0-255.  Most
* images do not have alpha information, in which case the alpha component
* is set to value 255 (full opacity).
*
* With IMAGEIO_SINGLE or IMAGEIO_DOUBLE, the components are values in the
* range 0 to 1.
*/
void *ReadImage(int *Width, int *Height,
    const char *FileName, unsigned Format)
{
    void *Image = NULL;
    uint32_t *ImageU8 = NULL;
    FILE *File;
    char Type[8];


    IdentifyImageType(Type, FileName);

    if(!(File = fopen(FileName, "rb")))
    {
        ErrorMessage("Unable to open file \"%s\".\n", FileName);
        return 0;
    }

    if(!strcmp(Type, "BMP"))
    {
        if(!ReadBmp(&ImageU8, Width, Height, File))
            ErrorMessage("Failed to read \"%s\".\n", FileName);
    }
    else if(!strcmp(Type, "JPEG"))
    {
#ifdef LIBJPEG_SUPPORT
        if(!(ReadJpeg(&ImageU8, Width, Height, File)))
            ErrorMessage("Failed to read \"%s\".\n", FileName);
#else
        ErrorMessage("File \"%s\" is a JPEG image.\n"
                     "Compile with LIBJPEG_SUPPORT to enable JPEG reading.\n",
                     FileName);
#endif
    }
    else if(!strcmp(Type, "PNG"))
    {
#ifdef LIBPNG_SUPPORT
        if(!(ReadPng(&ImageU8, Width, Height, File)))
            ErrorMessage("Failed to read \"%s\".\n", FileName);
#else
        ErrorMessage("File \"%s\" is a PNG image.\n"
                     "Compile with LIBPNG_SUPPORT to enable PNG reading.\n",
                     FileName);
#endif
    }
    else if(!strcmp(Type, "TIFF"))
    {
#ifdef LIBTIFF_SUPPORT
        fclose(File);

        if(!(ReadTiff(&ImageU8, Width, Height, FileName, 0)))
            ErrorMessage("Failed to read \"%s\".\n", FileName);

        File = NULL;
#else
        ErrorMessage("File \"%s\" is a TIFF image.\n"
                     "Compile with LIBTIFF_SUPPORT to enable TIFF reading.\n",
                     FileName);
#endif
    }
    else
    {
        /* File format is unsupported. */
        if(Type[0])
            ErrorMessage("File \"%s\" is a %s image.", FileName, Type);
        else
            ErrorMessage("File \"%s\" is an unrecognized format.", FileName);
        fprintf(stderr, "\nSorry, only " READIMAGE_FORMATS_SUPPORTED " reading is supported.\n");
    }

    if(File)
        fclose(File);

    if(ImageU8 && Format)
    {
        Image = ConvertToFormat(ImageU8, *Width, *Height, Format);
        Free(ImageU8);
    }
    else
        Image = ImageU8;

    return Image;
}


/**
* @brief Write an image file from 8-bit RGBA image data
*
* @param Image pointer to the image data
* @param Width, Height image dimensions
* @param FileName image file name
* @param Format specifies how the data is formatted (see ReadImage)
* @param Quality the JPEG image quality (between 0 and 100)
*
* @return 1 on success, 0 on failure
*
* The input \c Image should be a 32-bit RGBA image stored as in the
* description of \c ReadImage.  \c WriteImage writes to \c FileName in the
* file format specified by its extension.  If saving a JPEG image, the
* \c Quality argument specifies the quality factor (between 0 and 100).
* \c Quality has no effect on other formats.
*
* The return value indicates success with 1 or failure with 0.
*/
int WriteImage(void *Image, int Width, int Height,
    const char *FileName, unsigned Format, int Quality)
{
    FILE *File;
    uint32_t *ImageU8;
    enum {BMP_FORMAT, JPEG_FORMAT, PNG_FORMAT, TIFF_FORMAT} FileFormat;
    int Success = 0;

    if(!Image || Width <= 0 || Height <= 0)
    {
        ErrorMessage("Null image.\n");
        ErrorMessage("Failed to write \"%s\".\n", FileName);
        return 0;
    }

    if(StringEndsWith(FileName, ".bmp"))
        FileFormat = BMP_FORMAT;
    else if(StringEndsWith(FileName, ".jpg")
        || StringEndsWith(FileName, ".jpeg"))
    {
        FileFormat = JPEG_FORMAT;
#ifndef LIBJPEG_SUPPORT
        ErrorMessage("Failed to write \"%s\".\n", FileName);
        ErrorMessage("Compile with LIBJPEG_SUPPORT to enable JPEG writing.\n");
        return 0;
#endif
    }
    else if(StringEndsWith(FileName, ".png"))
    {
        FileFormat = PNG_FORMAT;
#ifndef LIBPNG_SUPPORT
        ErrorMessage("Failed to write \"%s\".\n", FileName);
        ErrorMessage("Compile with LIBPNG_SUPPORT to enable PNG writing.\n");
        return 0;
#endif
    }
    else if(StringEndsWith(FileName, ".tif")
        || StringEndsWith(FileName, ".tiff"))
    {
        FileFormat = TIFF_FORMAT;
#ifndef LIBTIFF_SUPPORT
        ErrorMessage("Failed to write \"%s\".\n", FileName);
        ErrorMessage("Compile with LIBTIFF_SUPPORT to enable TIFF writing.\n");
        return 0;
#endif
    }
    else
    {
        ErrorMessage("Failed to write \"%s\".\n", FileName);

        if(StringEndsWith(FileName, ".gif"))
            ErrorMessage("GIF is not supported.  ");
        else if(StringEndsWith(FileName, ".mng"))
            ErrorMessage("MNG is not supported.  ");
        else if(StringEndsWith(FileName, ".pcx"))
            ErrorMessage("PCX is not supported.  ");
        else
            ErrorMessage("Unable to determine format from extension.\n");

        ErrorMessage("Sorry, only " WRITEIMAGE_FORMATS_SUPPORTED " writing is supported.\n");
        return 0;
    }

    if(!(File = fopen(FileName, "wb")))
    {
        ErrorMessage("Unable to write to file \"%s\".\n", FileName);
        return 0;
    }

    if(!(ImageU8 = ConvertFromFormat(Image, Width, Height, Format)))
        return 0;

    switch(FileFormat)
    {
    case BMP_FORMAT:
        Success = WriteBmp(ImageU8, Width, Height, File);
        break;
    case JPEG_FORMAT:
#ifdef LIBJPEG_SUPPORT
        Success = WriteJpeg(ImageU8, Width, Height, File, Quality);
#else
        /* Dummy operation to avoid unused variable warning if compiled without
        libjpeg.  Note that execution returns above if Format == JPEG_FORMAT
        and LIBJPEG_SUPPORT is undefined. */
        Success = Quality;
#endif
        break;
    case PNG_FORMAT:
#ifdef LIBPNG_SUPPORT
        Success = WritePng(ImageU8, Width, Height, File);
#endif
        break;
    case TIFF_FORMAT:
#ifdef LIBTIFF_SUPPORT
        fclose(File);
        Success = WriteTiff(ImageU8, Width, Height, FileName);
        File = 0;
#endif
        break;
    }

    if(!Success)
        ErrorMessage("Failed to write \"%s\".\n", FileName);

    Free(ImageU8);

    if(File)
        fclose(File);

    return Success;
}
