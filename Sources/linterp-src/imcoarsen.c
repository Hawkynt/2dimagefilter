/**
 * @file imcoarsen.c
 * @brief Image coarsening utility program
 * @author Pascal Getreuer <getreuer@gmail.com>
 *
 * This file implements the imcoarsen program, a command line tool for
 * coarsening an image by Gaussain smoothing followed by downsampling.
 * See the routine PrintHelpMessage for usage details.
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

#include <math.h>
#include <string.h>
#include <ctype.h>

#include "imageio.h"
#include "strutil.h"

#define VERBOSE 0

/** @brief Approximate Gaussian with this number of standard deviations */
#define NUMSTDS 4


/** @brief struct representing an image */
typedef struct
{
    /** @brief 32-bit RGBA image data */
    uint32_t *Data;
    /** @brief Image width */
    int Width;
    /** @brief Image height */
    int Height;
} image;

typedef enum
{
    BOUNDARY_CONSTANT = 0,
    BOUNDARY_HSYMMETRIC = 1,
    BOUNDARY_WSYMMETRIC = 2,
    BOUNDARY_PERIODIC = 3
} boundaryhandling;

/** @brief struct of program parameters */
typedef struct
{
    /** @brief Input file name */
    char *InputFile;
    /** @brief Output file name */
    char *OutputFile;
    /** @brief Quality for saving JPEG images (0 to 100) */
    int JpegQuality;
    /** @brief If true, sample on the centered grid */
    int CenteredGrid;
    /** @brief Type of boundary handling */
    boundaryhandling Boundary;
     /** @brief Scaling option string */
    char *ScaleStr;
    /** @brief Horizontal scale factor */
    double ScaleX;
    /** @brief Vertical scale factor */
    double ScaleY;
    /** @brief Interpolated image width in pixels */
    int CoarseWidth;
    /** @brief Interpolated image height in pixels */
    int CoarseHeight;
    /** @brief Gaussian point spread function standard deviation */
    float PsfSigma;
} programparams;


int ParseParams(programparams *Param, int argc, char *argv[]);
static int ParseScaling(programparams *Param, int InputWidth, int InputHeight);
int Coarsen(image v, image u, programparams Param);

/** @brief Print program usage help message */
void PrintHelpMessage()
{
    printf("Image coarsening utility, P. Getreuer 2010-2011\n\n");
    printf("Usage: imcoarsen [options] <input file> <output file>\n\n"
            "Only " READIMAGE_FORMATS_SUPPORTED " images are supported.\n\n");
    printf("Options:\n");
    printf("   -x <number>             coarsening factor (>= 1.0, may be non-integer)\n");
    printf("   -x <x-scale>,<y-scale>  set horizontal and vertical coarsening factors\n");
    printf("   -x <width>x<height>     set maximum coarsened size in pixels, \n");
    printf("                           preserves aspect ratio\n");
    printf("   -x <width>x<height>^    set minimum coarsened size in pixels, \n");
    printf("                           preserves aspect ratio\n");
    printf("   -x <width>x<height>!    set actual coarsened size in pixels, \n");
    printf("                           ignores aspect ratio\n\n");
    
    printf("   -p <number>  sigma_h, the blur size of the Gaussian point spread function\n"
           "                in units of output pixels.\n");
    printf("   -b <ext>     extension to use for boundary handling, choices for <ext> are\n");
    printf("                const        constant extension\n");
    printf("                hsym         half-sample symmetric (default)\n");
    printf("                wsym         whole-sample symmetric\n");
    printf("                per          periodic\n");
    printf("   -g <grid>    grid to use for resampling, choices for <grid> are\n"
           "                centered     grid with centered alignment (default)\n"
           "                topleft      the top-left anchored grid\n\n");
#ifdef LIBJPEG_SUPPORT
    printf("   -q <number>  quality for saving JPEG images (0 to 100)\n\n");
#endif
    printf("Example: coarsen by factor 2.5\n"
        "   imcoarsen -x 2.5 -p 0.35 frog.bmp coarse.bmp\n");
}


int main(int argc, char *argv[])
{
    programparams Param;
    image u = {NULL, 0, 0}, v = {NULL, 0, 0};
    int Status = 1;


    if(!ParseParams(&Param, argc, argv))
        return 0;

    /* Read the input image */
    if(!(u.Data = (uint32_t *)ReadImage(&u.Width, &u.Height, Param.InputFile,
        IMAGEIO_U8 | IMAGEIO_RGBA)))
        goto Catch;
    
    if(!ParseScaling(&Param, u.Width, u.Height))
        goto Catch;

    if(Param.ScaleX >= u.Width || Param.ScaleY >= u.Height)
    {
        ErrorMessage("Image is too small for scale factor.\n");
        goto Catch;
    }

    /* Allocate the output image */
    v.Width = Param.CoarseWidth;
    v.Height = Param.CoarseHeight;
    
#if VERBOSE > 0
    printf("%dx%d input -> %dx%d output\n", u.Width, u.Height, v.Width, v.Height);
#endif

    if(!(v.Data = (uint32_t *)Malloc(sizeof(uint32_t)*
        ((long int)v.Width)*((long int)v.Height))))
        goto Catch;

    /* Convolution followed by downsampling */
    if(!Coarsen(v, u, Param))
        goto Catch;

    /* Write the output image */
    if(!WriteImage(v.Data, v.Width, v.Height, Param.OutputFile,
        IMAGEIO_U8 | IMAGEIO_RGBA, Param.JpegQuality))
        goto Catch;
#if VERBOSE > 0
    else
        printf("Output written to \"%s\".\n", Param.OutputFile);
#endif

    Status = 0;	/* Finished successfully, set exit status to zero. */

Catch:
    Free(v.Data);
    Free(u.Data);
    return Status;
}


float Sqr(float x)
{
    return x*x;
}

/**
 * @brief Boundary handling function for constant extension
 * @param N is the data length
 * @param i is an index into the data
 * @return an index that is always between 0 and N - 1
 */
static int ConstExtension(int N, int i)
{
    if(i < 0)
        return 0;
    else if(i >= N)
        return N - 1;
    else
        return i;
}


/**
 * @brief Boundary handling function for half-sample symmetric extension
 * @param N is the data length
 * @param i is an index into the data
 * @return an index that is always between 0 and N - 1
 */
static int HSymExtension(int N, int i)
{
    while(1)
    {
        if(i < 0)
            i = -1 - i;
        else if(i >= N)
            i = (2*N - 1) - i;
        else
            return i;
    }
}


/**
 * @brief Boundary handling function for whole-sample symmetric extension
 * @param N is the data length
 * @param i is an index into the data
 * @return an index that is always between 0 and N - 1
 */
static int WSymExtension(int N, int i)
{
    while(1)
    {
        if(i < 0)
            i = -i;
        else if(i >= N)
            i = (2*N - 2) - i;
        else
            return i;
    }
}


/**
 * @brief Boundary handling function for periodic extension
 * @param N is the data length
 * @param i is an index into the data
 * @return an index that is always between 0 and N - 1
 */
static int PerExtension(int N, int i)
{
    while(1)
    {
        if(i < 0)
            i += N;
        else if(i >= N)
            i -= N;
        else
            return i;
    }
}


int (*ExtensionMethod[4])(int, int) =
    {ConstExtension, HSymExtension, WSymExtension, PerExtension};


int Coarsen(image v, image u, programparams Param)
{
    int (*Extension)(int, int) = ExtensionMethod[Param.Boundary];
    const float PsfRadiusX = NUMSTDS*Param.PsfSigma*Param.ScaleX;
    const float PsfRadiusY = NUMSTDS*Param.PsfSigma*Param.ScaleY;
    const int PsfWidth = 1 + ((PsfRadiusX == 0) ? 1 : (int)ceil(2*PsfRadiusX));
    const int PsfHeight = 1 + ((PsfRadiusY == 0) ? 1 : (int)ceil(2*PsfRadiusY));
    float *Temp = NULL, *PsfBuf = NULL;
    float ExpDenomX, ExpDenomY, Weight, Sum[4], DenomSum;
    float XStart, YStart, X, Y;
    uint32_t Pixel;
    int IndexX0, IndexY0, SrcOffset, DestOffset;
    int x, y, n, c, Success = 0;


    if(!(Temp = (float *)Malloc(sizeof(float)*4*v.Width*u.Height))
        || !(PsfBuf = (float *)Malloc(sizeof(float)
            *((PsfWidth >= PsfHeight) ? PsfWidth : PsfHeight))))
        goto Catch;

    ExpDenomX = 2 * Sqr(Param.PsfSigma*Param.ScaleX);
    ExpDenomY = 2 * Sqr(Param.PsfSigma*Param.ScaleY);

    if(Param.CenteredGrid)
    {
        XStart = (1/Param.ScaleX - 1)/2;
        YStart = (1/Param.ScaleY - 1)/2;
    }
    else
        XStart = YStart = 0;

    for(x = 0; x < v.Width; x++)
    {
        X = (-XStart + x)*Param.ScaleX;
        IndexX0 = (int)ceil(X - PsfRadiusX - 0.5f);
        DenomSum = 0;

        /* Evaluate the PSF */
        for(n = 0; n < PsfWidth; n++)
        {
            PsfBuf[n] = Sqr(X - (IndexX0 + n));

            if(!n || PsfBuf[n] < DenomSum)
                DenomSum = PsfBuf[n];
        }

        if(ExpDenomX > 0)
            for(n = 0; n < PsfWidth; n++)
                PsfBuf[n] = (float)exp((DenomSum - PsfBuf[n]) / ExpDenomX);
        else if(IndexX0 == (int)floor(X - PsfRadiusX + 0.5f))
        {   /* If PsfSigma = 0, sample the nearest neighbor */
            PsfBuf[0] = 1;
            PsfBuf[1] = 0;
        }
        else /* At a half integer, average the two nearest neighbors */
            PsfBuf[0] = PsfBuf[1] = 0.5f;

        DenomSum = 0;

        for(n = 0; n < PsfWidth; n++)
            DenomSum += PsfBuf[n];

        for(y = 0, SrcOffset = 0, DestOffset = x; y < u.Height;
            y++, SrcOffset += u.Width, DestOffset += v.Width)
        {
            Sum[0] = Sum[1] = Sum[2] = Sum[3] = 0;

            for(n = 0; n < PsfWidth; n++)
            {
                Weight = PsfBuf[n];
                Pixel = u.Data[Extension(u.Width, IndexX0 + n) + SrcOffset];

                for(c = 0; c < 4; c++)
                    Sum[c] += (float)((uint8_t *)&Pixel)[c] * Weight;
            }

            for(c = 0; c < 4; c++)
                Temp[4*DestOffset + c] = Sum[c] / DenomSum;
        }
    }

    for(y = 0; y < v.Height; y++, v.Data += v.Width)
    {
        Y = (-YStart + y)*Param.ScaleY;
        IndexY0 = (int)ceil(Y - PsfRadiusY - 0.5f);
        DenomSum = 0;

        /* Evaluate the PSF */
        for(n = 0; n < PsfHeight; n++)
        {
            PsfBuf[n] = Sqr(Y - (IndexY0 + n));

            if(!n || PsfBuf[n] < DenomSum)
                DenomSum = PsfBuf[n];
        }

        if(ExpDenomY > 0)
            for(n = 0; n < PsfHeight; n++)
                PsfBuf[n] = (float)exp((DenomSum - PsfBuf[n]) / ExpDenomY);
        else if(IndexY0 == (int)floor(Y - PsfRadiusY + 0.5f))
        {   /* If PsfSigma = 0, sample the nearest neighbor */
            PsfBuf[0] = 1;
            PsfBuf[1] = 0;
        }
        else /* At a half integer, average the two nearest neighbors */
            PsfBuf[0] = PsfBuf[1] = 0.5f;

        DenomSum = 0;

        for(n = 0; n < PsfHeight; n++)
            DenomSum += PsfBuf[n];

        for(x = 0; x < v.Width; x++)
        {
            Sum[0] = Sum[1] = Sum[2] = Sum[3] = 0;

            for(n = 0; n < PsfHeight; n++)
            {
                SrcOffset = x + v.Width*Extension(u.Height, IndexY0 + n);
                Weight = PsfBuf[n];

                for(c = 0; c < 4; c++)
                    Sum[c] += Temp[4*SrcOffset + c] * Weight;
            }

            for(c = 0; c < 4; c++)
                ((uint8_t *)&Pixel)[c] = (int)(Sum[c] / DenomSum + 0.5f);

            v.Data[x] = Pixel;
        }
    }

    Success = 1;
Catch:
    Free(PsfBuf);
    Free(Temp);
    return Success;
}


/*
 * Parse the scaling option string
 *
 * Syntax
 * <scale>              Scale by factor <scale>
 * <scalex>,<scaley>    Scale width and height individually
 * <width>x<height>     Max size given, aspect ratio preserved
 * <width>x<height>^    Min size given, aspect ratio preserved
 * <width>x<height>!    Actual size given, aspect ratio ignored
 */
static int ParseScaling(programparams *Param, int InputWidth, int InputHeight)
{
    const char *StrPtr = Param->ScaleStr;
    double Number;

    if(!ParseNumber(&Number, &StrPtr, 1))
        goto Catch;

    if(!EatWhitespace(&StrPtr))
    {   /* Syntax <scale> */
        Param->ScaleX = Param->ScaleY = Number;
        Param->CoarseWidth = (int)ceil(InputWidth/Param->ScaleX);
        Param->CoarseHeight = (int)ceil(InputHeight/Param->ScaleY);
    }
    else if(*StrPtr == ',')
    {   /* Syntax <scalex>,<scaley> */
        StrPtr++;
        Param->ScaleX = Number;

        if(!ParseNumber(&Number, &StrPtr, 1))
            goto Catch;

        Param->ScaleY = Number;
        Param->CoarseWidth = (int)ceil(InputWidth/Param->ScaleX);
        Param->CoarseHeight = (int)ceil(InputHeight/Param->ScaleY);
    }
    else if(*StrPtr == 'x' || *StrPtr == 'X')
    {   /* Syntax <width>x<height>... */
        StrPtr = Param->ScaleStr;

        /* Reparse as integer */
        if(!ParseNumber(&Number, &StrPtr, 0)
            || !(*StrPtr == 'x' || *StrPtr == 'X'))
            goto Catch;

        StrPtr++;
        Param->CoarseWidth = (int)floor(Number + 0.5);

        if(!ParseNumber(&Number, &StrPtr, 0))
            goto Catch;

        Param->CoarseHeight = (int)floor(Number + 0.5);
        Param->ScaleX = ((double)InputWidth) / ((double)Param->CoarseWidth);
        Param->ScaleY = ((double)InputHeight) / ((double)Param->CoarseHeight);
        EatWhitespace(&StrPtr);

        switch(*StrPtr)
        {
        case '\0': /* <width>x<height> Max size given, preserve aspect ratio */
            if(InputHeight*Param->CoarseWidth
                <= InputWidth*Param->CoarseHeight)
            {
                Param->ScaleY = Param->ScaleX;
                Param->CoarseHeight =
                    (int)floor(InputHeight/Param->ScaleY + 0.5);
            }
            else
            {
                Param->ScaleX = Param->ScaleY;
                Param->CoarseWidth =
                    (int)floor(InputWidth/Param->ScaleX + 0.5);
            }
            break;
        case '^': /* <width>x<height>^ Min size given, preserve aspect ratio */
            if(InputHeight*Param->CoarseWidth
                >= InputWidth*Param->CoarseHeight)
            {
                Param->ScaleY = Param->ScaleX;
                Param->CoarseHeight =
                    (int)floor(InputHeight/Param->ScaleY + 0.5);
            }
            else
            {
                Param->ScaleX = Param->ScaleY;
                Param->CoarseWidth =
                    (int)floor(InputWidth/Param->ScaleX + 0.5);
            }
            break;
        case '!': /* <width>x<height>! Actual size given */
            break;
        default:
            goto Catch;
        }
    }
    else
        goto Catch;

    return 1;
Catch:
    ErrorMessage("Invalid scaling option \"%s\".\n", Param->ScaleStr);
    return 0;
}


int ParseParams(programparams *Param, int argc, char *argv[])
{
    static char *DefaultOutputFile = (char *)"out.bmp";
    static char *DefaultScaleStr = (char *)"1";
    char *OptionString;
    char OptionChar;
    int i;


    if(argc < 2)
    {
        PrintHelpMessage();
        return 0;
    }

    /* Set parameter defaults */
    Param->InputFile = 0;
    Param->OutputFile = DefaultOutputFile;
    Param->JpegQuality = 99;
    Param->ScaleStr = DefaultScaleStr;
    Param->PsfSigma = 0.35f;
    Param->CenteredGrid = 1;
    Param->Boundary = BOUNDARY_HSYMMETRIC;

    for(i = 1; i < argc;)
    {
        if(argv[i] && argv[i][0] == '-')
        {
            if((OptionChar = argv[i][1]) == 0)
            {
                ErrorMessage("Invalid parameter format.\n");
                return 0;
            }

            if(argv[i][2])
                OptionString = &argv[i][2];
            else if(++i < argc)
                OptionString = argv[i];
            else
            {
                ErrorMessage("Invalid parameter format.\n");
                return 0;
            }

            switch(OptionChar)
            {
            case 'x':
                Param->ScaleStr = OptionString;
                break;
            case 'p':
                Param->PsfSigma = (float)atof(OptionString);

                if(Param->PsfSigma < 0.0)
                {
                    ErrorMessage("Point spread blur size must be nonnegative.\n");
                    return 0;
                }
                break;
            case 'b':
                if(!strcmp(OptionString, "const"))
                    Param->Boundary = BOUNDARY_CONSTANT;
                else if(!strcmp(OptionString, "hsym"))
                    Param->Boundary = BOUNDARY_HSYMMETRIC;
                else if(!strcmp(OptionString, "wsym"))
                    Param->Boundary = BOUNDARY_WSYMMETRIC;
                else if(!strcmp(OptionString, "per"))
                    Param->Boundary = BOUNDARY_PERIODIC;
                else
                {
                    ErrorMessage("Boundary extension must be either \"const\", \"hsym\", or \"wsym\".\n");
                    return 0;
                }
                break;
            case 'g':
                if(!strcmp(OptionString, "centered")
                    || !strcmp(OptionString, "center"))
                    Param->CenteredGrid = 1;
                else if(!strcmp(OptionString, "topleft")
                    || !strcmp(OptionString, "top-left"))
                    Param->CenteredGrid = 0;
                else
                {
                    ErrorMessage("Grid must be either \"centered\" or \"topleft\".\n");
                    return 0;
                }
                break;

#ifdef LIBJPEG_SUPPORT
            case 'q':
                Param->JpegQuality = atoi(OptionString);

                if(Param->JpegQuality <= 0 || Param->JpegQuality > 100)
                {
                    ErrorMessage("JPEG quality must be between 0 and 100.\n");
                    return 0;
                }
                break;
#endif
            case '-':
                PrintHelpMessage();
                return 0;
            default:
                if(isprint(OptionChar))
                    ErrorMessage("Unknown option \"-%c\".\n", OptionChar);
                else
                    ErrorMessage("Unknown option.\n");

                return 0;
            }

            i++;
        }
        else
        {
            if(!Param->InputFile)
                Param->InputFile = argv[i];
            else
                Param->OutputFile = argv[i];

            i++;
        }
    }

    if(!Param->InputFile)
    {
        PrintHelpMessage();
        return 0;
    }

    return 1;
}
