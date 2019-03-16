/**
 * @file linterpcli.c
 * @brief Linear Methods for Image Interpolation main program
 * @author Pascal Getreuer <getreuer@gmail.com>
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

/**
 * @mainpage
 * @htmlinclude readme.html
 */

#include <string.h>
#include <ctype.h>
#include <math.h>

#include "imageio.h"
#include "linterp.h"
#include "strutil.h"

/* Set to 1 for verbose program output */
#define VERBOSE 0


/** @brief struct representing an image */
typedef struct
{
    /** @brief Float image data */
    float *Data;
    /** @brief Image width */
    int Width;
    /** @brief Image height */
    int Height;
} imagef;


/** @brief struct of program parameters */
typedef struct
{
    /** @brief Input file name */
    char *InputFile;
    /** @brief Output file name */
    char *OutputFile;
    /** @brief Quality for saving JPEG images (0 to 100) */
    int JpegQuality;
    /** @brief Nonzero means to sample on the centered grid */
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
    int InterpWidth;
    /** @brief Interpolated image height in pixels */
    int InterpHeight;
    /** @brief Rotation, counter clockwise in degrees */
    float Rotation;
    /** @brief Interpolation method to use */
    char *Method;
    /** @brief Gaussian point spread function standard deviation */
    float PsfSigma;
} programparams;


static int ParseParams(programparams *Param, int argc, char *argv[]);
static int ParseScaling(programparams *Param, int InputWidth, int InputHeight);

static void PrintHelpMessage()
{
    printf("Linear interpolation demo, P. Getreuer 2010-2011\n\n");
    printf("Usage: linterp [options] <input file> <output file>\n\n"
        "Only " READIMAGE_FORMATS_SUPPORTED " images are supported.\n\n");
    printf("Options:\n");
    printf("   -m <method>  interpolation method to apply, choices for <method> are\n");
    printf("                nearest      nearest neighbor (pixel duplication)\n");
    printf("                bilinear     standard bilinear interpolation\n");
    printf("                bicubic      Keys bicubic with parameter -0.5\n");
    printf("                lanczosN     Lanczos radius-N sinc approximation,\n");
    printf("                             N = 2, 3, 4\n");
    printf("                bsplineN     B-spline of degree N,\n");
    printf("                             N = 2, 3, 5, 7, 9, 11\n");
    printf("                omomsN       o-Moms of degree N,\n");
    printf("                             N = 3, 5, 7\n");
    printf("                fourier      Fourier zero-padding (sinc)\n\n");
    printf("   -x <scale>              the scale factor (may be non-integer)\n");
    printf("   -x <x-scale>,<y-scale>  set horizontal and vertical scale factors\n");
    printf("   -x <width>x<height>     set maximum interpolated size in pixels, \n");
    printf("                           preserves aspect ratio\n");
    printf("   -x <width>x<height>^    set minimum interpolated size in pixels, \n");
    printf("                           preserves aspect ratio\n");
    printf("   -x <width>x<height>!    set actual interpolated size in pixels, \n");
    printf("                           ignores aspect ratio\n\n");
    printf("   -r <number>  rotation, counter clockwise in degrees\n");
    printf("                (if specified, preserves aspect ratio regardless of -x)\n");
    printf("   -p <number>  sigma_h, the blur size of the point spread function\n");
    printf("   -b <ext>     extension to use for boundary handling, choices for <ext> are\n");
    printf("                const        constant extension\n");
    printf("                hsym         half-sample symmetric\n");
    printf("                wsym         whole-sample symmetric\n");
    printf("   -g <grid>    grid to use for resampling, choices for <grid> are\n"
           "                centered     grid with centered alignment (default)\n"
           "                topleft      the top-left anchored grid\n\n");
#ifdef LIBJPEG_SUPPORT
    printf("   -q <number>  quality for saving JPEG images (0 to 100)\n\n");
#endif
    printf("Example: 4.5x cubic B-spline scaling, sigma_h = 0.35\n"
        "   linterp -m bspline3 -x 4.5 -p 0.35 frog.bmp interpolation.bmp\n");
}


float GaussianPsf(float x, const void *Param)
{
    float Sigma = *((float *)Param);
    return exp(-(x*x)/(2*Sigma*Sigma)) / (sqrt(M_2PI)*Sigma);
}


imagef ScaleRotateImage(imagef v, programparams Param)
{
    interpmethod *Method;
    const int InputNumPixels = v.Width*v.Height;
    imagef u = {NULL, 0, 0};
    float *Coeff = NULL, *X = NULL, *Y = NULL;
    float XStart, XStep, YStart, YStep, Theta;
    int NumCoeffs, Channel;


    if(!(Method = GetInterpMethod(Param.Method)))
    {
        fprintf(stderr, "Unknown interpolation method.\n");
        goto Catch;
    }

    Theta = Param.Rotation*M_PI/180;

    /* Prefilter the image if necessary */
    if(Param.PsfSigma > 0)
    {
        if(Param.Boundary != BOUNDARY_HSYMMETRIC
            && Param.Boundary != BOUNDARY_WSYMMETRIC)
        {
            fprintf(stderr,
"PSF prefiltering only supported for half- and whole-sample symmetric\n"
"boundary extension.\n");
            goto Catch;
        }

        NumCoeffs = 2 + ceil(Method->KernelRadius + 5*Param.PsfSigma);

        if(!(Coeff = (float *)Malloc(sizeof(float)*NumCoeffs)))
        {
            fprintf(stderr, "Memory allocation failed.\n");
            goto Catch;
        }

#if VERBOSE > 0
        printf("PSF prefiltering\n");
#endif
        PsfConvCoeff(Coeff, NumCoeffs, GaussianPsf,
            (const void *)&Param.PsfSigma, Method->Kernel,
            Method->KernelRadius, Method->KernelNormalize);
        PsfPreFilter(v.Data, v.Width, v.Height, 3,
            Coeff, NumCoeffs, Param.Boundary);
    }
    else if(Method->PrefilterNumAlpha)
    {
#if VERBOSE > 0
        printf("Prefiltering\n");
#endif
        PrefilterImage(v.Data, v.Width, v.Height, 3, Method->PrefilterAlpha,
            Method->PrefilterNumAlpha, Method->PrefilterScale, Param.Boundary);
    }

    if(Theta == 0)  /* Scaling without rotation */
    {
        u.Width = Param.InterpWidth;
        u.Height = Param.InterpHeight;

#if VERBOSE > 0
        printf("Scaling %dx%d -> %dx%d\n",
            v.Width, v.Height, u.Width, u.Height);
#endif

        if(!(u.Data = (float *)Malloc(sizeof(float)*3*u.Width*u.Height)))
            goto Catch;

        XStep = 1.0/Param.ScaleX;
        YStep = 1.0/Param.ScaleY;

        if(Param.CenteredGrid)
        {
            XStart = (XStep - 1.0)/2;
            YStart = (YStep - 1.0)/2;
        }
        else
            XStart = YStart = 0;

        if(!LinScale2d(u.Data, u.Width, XStart, XStep, u.Height, YStart, YStep,
            v.Data, v.Width, v.Height, 3, Method->Kernel, Method->KernelRadius,
            Method->KernelNormalize, Param.Boundary))
            goto Catch;
    }
    else	/* Scaling and rotation */
    {
        /* Create a list of locations (X[n],Y[n]) that sample a grid of size
           u.Width by u.Height rotated by Theta and scaled by factor Scale.

           For other interpolation operations like a perspective transformation,
           rewrite this step so that (X[n],Y[n]) is the desired nth sampling
           location and set u.Width and u.Height accordingly. */
        if(!MakeScaleRotationGrid(&X, &Y, &u.Width, &u.Height,
            v.Width, v.Height, (Param.ScaleX + Param.ScaleY)/2, Theta))
            goto Catch;

#if VERBOSE > 0
        printf("Scaling and rotating %dx%d -> %dx%d\n",
            v.Width, v.Height, u.Width, u.Height);
#endif

        if(!(u.Data = (float *)Malloc(sizeof(float)*3*u.Width*u.Height)))
            goto Catch;

        for(Channel = 0; Channel < 3; Channel++)
            if(!LinInterp2d(u.Data + Channel*u.Width*u.Height,
                v.Data + Channel*InputNumPixels, v.Width, v.Height,
                X, Y, u.Width*u.Height, Method->Kernel,
                Method->KernelRadius, Method->KernelNormalize, Param.Boundary))
                goto Catch;

        Free(Y);
        Free(X);
    }

    Free(Coeff);
    return u;
Catch:
    Free(u.Data);
    Free(Y);
    Free(X);
    Free(Coeff);
    u.Data = 0;
    u.Width = u.Height = 0;
    return u;
}


int main(int argc, char *argv[])
{
    programparams Param;
    imagef v = {NULL, 0, 0}, u = {NULL, 0, 0};
    unsigned long StartTime;
    float XStart, YStart;


    if(!ParseParams(&Param, argc, argv))
        return 0;

    /* Read the input image */
    if(!(v.Data = (float *)ReadImage(&v.Width, &v.Height, Param.InputFile,
         IMAGEIO_FLOAT | IMAGEIO_RGB | IMAGEIO_PLANAR)))
        return 1;
    else if(v.Width < 3 || v.Height < 3)
    {
        fprintf(stderr, "Image is too small (%dx%d).\n", v.Width, v.Height);
        goto Catch;
    }

    if(!ParseScaling(&Param, v.Width, v.Height))
        goto Catch;

    StartTime = Clock();

    if(!strcmp(Param.Method, "fourier") || !strcmp(Param.Method, "sinc"))
    {
        if(Param.Rotation != 0)
        {
            fprintf(stderr, "Rotation is not supported with Fourier interpolation.\n");
            goto Catch;
        }
        else if(Param.Boundary != BOUNDARY_HSYMMETRIC
            && Param.Boundary != BOUNDARY_WSYMMETRIC)
        {
            fprintf(stderr,
"Fourier interpolation is only supported for half- and whole-sample\n"
"symmetric boundary extension.\n");
            goto Catch;
        }

        u.Width = Param.InterpWidth;
        u.Height = Param.InterpHeight;

#if VERBOSE > 0
        printf("Fourier scaling %dx%d -> %dx%d\n",
                v.Width, v.Height, u.Width, u.Height);
#endif

        if(!(u.Data = (float *)Malloc(sizeof(float)*3*u.Width*u.Height)))
            goto Catch;

        if(Param.CenteredGrid)
        {
            XStart = v.Width/(2.0f*u.Width) - 0.5f;
            YStart = v.Height/(2.0f*u.Height) - 0.5f;
        }
        else
            XStart = YStart = 0;

        if(!FourierScale2d(u.Data, u.Width, XStart, u.Height, YStart,
            v.Data, v.Width, v.Height, 3, Param.PsfSigma, Param.Boundary))
            goto Catch;
    }
    else
        u = ScaleRotateImage(v, Param);

    if(!u.Data)
    {
        fprintf(stderr, "Error in computation.\n");
        goto Catch;
    }

    printf("CPU Time: %.3f\n", (Clock() - StartTime)*0.001f);

    /* Write output */
    if(!WriteImage(u.Data, u.Width, u.Height, Param.OutputFile,
        IMAGEIO_FLOAT | IMAGEIO_RGB | IMAGEIO_PLANAR, Param.JpegQuality))
        fprintf(stderr, "Error writing output file.\n");

Catch:
    Free(u.Data);
    Free(v.Data);
    return 0;
}


static int ParseParams(programparams *Param, int argc, char *argv[])
{
    static char *DefaultOutputFile = (char *)"out.bmp";
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
    Param->CenteredGrid = 1;
    Param->Boundary = BOUNDARY_HSYMMETRIC;
    Param->ScaleStr = (char *)"1";
    Param->Rotation = 0;
    Param->Method = (char *)"bspline3";
    Param->PsfSigma = 0;
    Param->JpegQuality = 80;

    for(i = 1; i < argc;)
    {
        if(argv[i] && argv[i][0] == '-')
        {
            if((OptionChar = argv[i][1]) == 0)
            {
                fprintf(stderr, "Invalid parameter format.\n");
                return 0;
            }

            if(argv[i][2])
                OptionString = &argv[i][2];
            else if(++i < argc)
                OptionString = argv[i];
            else
            {
                fprintf(stderr, "Invalid parameter format.\n");
                return 0;
            }

            switch(OptionChar)
            {
            case 'g':
                if(!strcmp(OptionString, "centered")
                    || !strcmp(OptionString, "center"))
                    Param->CenteredGrid = 1;
                else if(!strcmp(OptionString, "topleft")
                    || !strcmp(OptionString, "top-left"))
                    Param->CenteredGrid = 0;
                else
                {
                    fprintf(stderr, "Grid must be either \"centered\" or \"topleft\".\n");
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
                else
                {
                    fprintf(stderr, "Invalid boundary extension.\n");
                    return 0;
                }
                break;
            case 'x':
                Param->ScaleStr = OptionString;
                break;
            case 'r':
                Param->Rotation = atof(OptionString);
                break;
            case 'm':
                Param->Method = OptionString;
                break;
            case 'p':
                Param->PsfSigma = atof(OptionString);

                if(Param->PsfSigma < 0.0)
                {
                    fprintf(stderr, "Point spread blur size must be nonnegative.\n");
                    return 0;
                }
                break;
#ifdef LIBJPEG_SUPPORT
            case 'q':
                Param->JpegQuality = atoi(OptionString);

                if(Param->JpegQuality <= 0 || Param->JpegQuality > 100)
                {
                    fprintf(stderr, "JPEG quality must be between 0 and 100.\n");
                    return 0;
                }
                break;
#endif
            case '-':
                PrintHelpMessage();
                return 0;
            default:
                if(isprint(OptionChar))
                    fprintf(stderr, "Unknown option \"-%c\".\n", OptionChar);
                else
                    fprintf(stderr, "Unknown option.\n");

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

    /* Display the chosen parameters */
    printf("Interpolation with %s\n", Param->Method);
    return 1;
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
        Param->InterpWidth = (int)ceil(Param->ScaleX*InputWidth);
        Param->InterpHeight = (int)ceil(Param->ScaleY*InputHeight);
#if VERBOSE > 0
        printf("Scaling by %g (%dx%d to %dx%d)\n", Param->ScaleX,
            InputWidth, InputHeight,
            Param->InterpWidth, Param->InterpHeight);
#endif
    }
    else if(*StrPtr == ',')
    {   /* Syntax <scalex>,<scaley> */
        StrPtr++;
        Param->ScaleX = Number;

        if(!ParseNumber(&Number, &StrPtr, 1))
            goto Catch;

        Param->ScaleY = Number;
        Param->InterpWidth = (int)ceil(Param->ScaleX*InputWidth);
        Param->InterpHeight = (int)ceil(Param->ScaleY*InputHeight);
#if VERBOSE > 0
        printf("Scaling by %g,%g (%dx%d to %dx%d)\n",
            Param->ScaleX, Param->ScaleY,
            InputWidth, InputHeight,
            Param->InterpWidth, Param->InterpHeight);
#endif
    }
    else if(*StrPtr == 'x' || *StrPtr == 'X')
    {   /* Syntax <width>x<height>... */
        StrPtr = Param->ScaleStr;

        /* Reparse as integer */
        if(!ParseNumber(&Number, &StrPtr, 0)
            || !(*StrPtr == 'x' || *StrPtr == 'X'))
            goto Catch;

        StrPtr++;
        Param->InterpWidth = (int)floor(Number + 0.5);

        if(!ParseNumber(&Number, &StrPtr, 0))
            goto Catch;

        Param->InterpHeight = (int)floor(Number + 0.5);
        Param->ScaleX = (double)Param->InterpWidth / (double)InputWidth;
        Param->ScaleY = (double)Param->InterpHeight / (double)InputHeight;
        EatWhitespace(&StrPtr);

        switch(*StrPtr)
        {
        case '\0': /* <width>x<height> Max size given, preserve aspect ratio */
            if(InputHeight*Param->InterpWidth
                <= InputWidth*Param->InterpHeight)
            {
                Param->ScaleY = Param->ScaleX;
                Param->InterpHeight =
                    (int)floor(Param->ScaleY*InputHeight + 0.5);
#if VERBOSE > 0
                printf("Scaling %dx%d to %dx(%d) (preserving aspect ratio)\n",
                    InputWidth, InputHeight,
                    Param->InterpWidth, Param->InterpHeight);
#endif
            }
            else
            {
                Param->ScaleX = Param->ScaleY;
                Param->InterpWidth =
                    (int)floor(Param->ScaleX*InputWidth + 0.5);
#if VERBOSE > 0
                printf("Scaling %dx%d to (%d)x%d (preserving aspect ratio)\n",
                    InputWidth, InputHeight,
                    Param->InterpWidth, Param->InterpHeight);
#endif
            }
            break;
        case '^': /* <width>x<height>^ Min size given, preserve aspect ratio */
            if(InputHeight*Param->InterpWidth
                >= InputWidth*Param->InterpHeight)
            {
                Param->ScaleY = Param->ScaleX;
                Param->InterpHeight =
                    (int)floor(Param->ScaleY*InputHeight + 0.5);
#if VERBOSE > 0
                printf("Scaling %dx%d to %dx(%d)^ (preserving aspect ratio)\n",
                    InputWidth, InputHeight,
                    Param->InterpWidth, Param->InterpHeight);
#endif
            }
            else
            {
                Param->ScaleX = Param->ScaleY;
                Param->InterpWidth =
                    (int)floor(Param->ScaleX*InputWidth + 0.5);
#if VERBOSE > 0
                printf("Scaling %dx%d to (%d)x%d^ (preserving aspect ratio)\n",
                    InputWidth, InputHeight,
                    Param->InterpWidth, Param->InterpHeight);
#endif
            }
            break;
        case '!': /* <width>x<height>! Actual size given */
#if VERBOSE > 0
            printf("Scaling %dx%d to %dx%d!\n",
                    InputWidth, InputHeight,
                    Param->InterpWidth, Param->InterpHeight);
#endif
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
