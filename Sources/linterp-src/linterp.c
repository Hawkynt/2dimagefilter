/**
 * @file linterp.c
 * @brief Linear interpolation
 * @author Pascal Getreuer <getreuer@gmail.com>
 *
 * This file implements \c LinInterp2d for interpolating at arbitrary
 * sample locations with a compactly supported interpolation kernel,
 * \c LinScale2d for interpolating on a uniform grid of sample
 * locations with a compactly supported interpolation kernel, and
 * \c FourierScale2d for interpolating on a uniform grid using Fourier
 * interpolation.
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

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <fftw3.h>

#include "basic.h"
#include "linterp.h"
#include "lkernels.h"

/** @brief Clamp X to [A, B] */
#define CLAMP(X,A,B)    (((X) < (A)) ? (A) : (((X) > (B)) ? (B) : (X)))


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


/** @brief Boundary extension methods */
int (*ExtensionMethod[3])(int, int) =
    {ConstExtension, HSymExtension, WSymExtension};


/**
 * @brief 2D linear interpolation (arbitrary resampling)
 * @param Dest pointer to destination array
 * @param Src pointer to source array
 * @param SrcWidth, SrcHeight dimensions of the source data
 * @param X, Y the sampling locations
 * @param NumSamples the number of samples
 * @param Kernel the interpolation kernel to use
 * @param KernelRadius the radius of support of the kernel
 * @param KernelNormalize if nonzero, weights are normalized to sum to 1
 * @param Boundary boundary handling
 * @return 1 on success, 0 on failure.
 *
 * This routine implements the computation
 *     Dest[k] = sum_m sum_n Src[m,n] Kernel(X[k] - m) Kernel(Y[k] - n),
 *     k = 0, ..., NumSamples-1.
 * The source image Src is interpolated at points (X[0],Y[0]), (X[1],Y[1]), ...
 * (X[NumSamples-1],Y[NumSamples-1]).  Src should be in row-major order as
 *     Src[m + SrcWidth*n] = (m,n)th pixel value,
 *     m = 0, ..., SrcWidth-1,
 *     n = 0, ..., SrcHeight-1.
 * Dest should have space for at least NumSamples elements.  The pixels of Src
 * are logically positioned at the integers with the upper-left corner (Src[0])
 * corresponding to (0,0).  In other words, if X[k] = m and Y[k] = n, then
 *     Dest[k] = Src[m + SrcWidth*n].
 * If (X[k],Y[k]) is outside of [0,SrcWidth-1] x [0,SrcHeight-1], then Src is
 * extrapolated with half-sample even symmetry.
 *
 * Kernel should be a function with the calling syntax
 *     float Kernel(float x).
 * Kernel should be zero (or approximately zero) for |x| >= KernelRadius.
 *
 * If KernelNormalize is nonzero, the computation includes a normalizing
 * denominator,
 *     Dest[k] = 1/Z[k] sum_m sum_n Src[m,n] Kernel(X[k] - m) Kernel(Y[k] - n),
 * where Z[k] is
 *     Z[k] = sum_m sum_n Kernel(X[k] - m) Kernel(Y[k] - n).
 * The normalization ensures that the interpolation exactly reproduces constant
 * functions.  Normalization is needed for example with the Lanczos kernels,
 * which do not reproduce constants.
 *
 * This normalization is equivalent to, but more efficient than, applying
 * \c LinInterp2d with the normalized kernel
 *     NormalizedKernel(x) = Kernel(x) / sum_n Kernel(x - n).
 * On the other hand, if Kernel is already normalized, it is slightly more
 * efficient to use KernelNormalize = 0.
 */
int LinInterp2d(float *Dest, const float *Src, int SrcWidth, int SrcHeight,
    float *X, float *Y, int NumSamples,
    float (*Kernel)(float), float KernelRadius, int KernelNormalize,
    boundaryhandling Boundary)
{
    int (*Extension)(int, int) = ExtensionMethod[Boundary];
    const int KernelWidth = (int)ceil(2*KernelRadius);
    float *KernelXBuf = NULL, *KernelYBuf = NULL;
    float Weight, Sum, DenomSum;
    int IndexX0, IndexY0, IndexX, IndexY, SrcRowOffset;
    int k, m, n, Success = 0;

    if(!Dest || !Src || SrcWidth <= 0 || SrcHeight <= 0 || !X || !Y
        || NumSamples < 0 || !Kernel || KernelRadius < 0
        || !(KernelXBuf = (float *)Malloc(sizeof(float)*(KernelWidth)))
        || !(KernelYBuf = (float *)Malloc(sizeof(float)*(KernelWidth))))
        goto Catch;

    if(!KernelNormalize)
        for(k = 0; k < NumSamples; k++)
        {
            IndexX0 = (int)ceil(X[k] - KernelRadius);
            IndexY0 = (int)ceil(Y[k] - KernelRadius);
            Sum = 0.0f;

            /* Evaluate the kernel */
            for(m = 0; m < KernelWidth; m++)
                KernelXBuf[m] = Kernel(X[k] - (IndexX0 + m));

            for(n = 0; n < KernelWidth; n++)
                KernelYBuf[n] = Kernel(Y[k] - (IndexY0 + n));

            /* Compute the interpolated value at (X[k], Y[k]) */
            for(n = 0; n < KernelWidth; n++)
            {
                IndexY = Extension(SrcHeight, IndexY0 + n);
                SrcRowOffset = SrcWidth*IndexY;

                for(m = 0; m < KernelWidth; m++)
                {
                    IndexX = Extension(SrcWidth, IndexX0 + m);
                    Sum += Src[IndexX + SrcRowOffset]
                        * KernelXBuf[m] * KernelYBuf[n];
                }
            }

            Dest[k] = Sum;
        }
    else
        for(k = 0; k < NumSamples; k++)
        {
            IndexX0 = (int)ceil(X[k] - KernelRadius);
            IndexY0 = (int)ceil(Y[k] - KernelRadius);
            Sum = DenomSum = 0.0f;

            /* Evaluate the kernel */
            for(m = 0; m < KernelWidth; m++)
                KernelXBuf[m] = Kernel(X[k] - (IndexX0 + m));

            for(n = 0; n < KernelWidth; n++)
                KernelYBuf[n] = Kernel(Y[k] - (IndexY0 + n));

            /* Compute the interpolated value at (X[k], Y[k]) */
            for(n = 0; n < KernelWidth; n++)
            {
                IndexY = Extension(SrcHeight, IndexY0 + n);
                SrcRowOffset = SrcWidth*IndexY;

                for(m = 0; m < KernelWidth; m++)
                {
                    IndexX = Extension(SrcWidth, IndexX0 + m);
                    Weight = KernelXBuf[m] * KernelYBuf[n];
                    Sum += Src[IndexX + SrcRowOffset] * Weight;
                    DenomSum += Weight;
                }
            }

            Dest[k] = Sum / DenomSum;
        }

    Success = 1;
Catch:
    Free(KernelYBuf);
    Free(KernelXBuf);
    return Success;
}


/**
* @brief Make a sampling grid for scaling and rotating an image
* @param X, Y array of sample locations
* @param GridWidth, GridHeight the dimensions of the sampling grid
* @param SrcWidth, SrcHeight the size of the source image
* @param Scale the scale factor (> 1 for finer resolution)
* @param Theta  rotation angle (counter clockwise)
* @return 1 on success, 0 on failure.
*
* This routine creates sampling locations (X[0],Y[0]), (X[1],Y[1]), ...
* (X[N-1],Y[N-1]), where the number of samples is N = GridWidth*GridHeight,
* such that interpolating the source image at these locations produces an
* scaled and rotated version of the image of size GridWidth by GridHeight.
*
* The memory for X and Y is allocated by this routine.  It is the
* responsibility of the caller to call Free(X), Free(Y) to release this memory
* when done.
*/
int MakeScaleRotationGrid(float **X, float **Y, int *GridWidth,
    int *GridHeight, int SrcWidth, int SrcHeight, float Scale, float Theta)
{
    const int ScaleWidth = floor(Scale*SrcWidth + 0.5f);
    const int ScaleHeight = floor(Scale*SrcHeight + 0.5f);
    const float x0 = (float)ScaleWidth/2.0f;
    const float y0 = (float)ScaleHeight/2.0f;
    const float CosTheta = (float)cos(Theta);
    const float SinTheta = (float)sin(Theta);
    float CurX, CurY, XStart, YStart, *XPtr, *YPtr;
    int m, n;


    if(!X || !Y || !GridWidth || !GridHeight
        || SrcWidth <= 0 || SrcHeight <= 0 || Scale <= 0)
        return 0;

    *X = *Y = 0;

    /* Determine the support of the transformed image. */
    XStart = -fabs(CosTheta)*x0 - fabs(SinTheta)*y0;
    YStart = -fabs(SinTheta)*x0 - fabs(CosTheta)*y0;
    *GridWidth = floor(ScaleWidth*fabs(CosTheta)
        + ScaleHeight*fabs(SinTheta) + 0.5f);
    *GridHeight = floor(ScaleWidth*fabs(SinTheta)
        + ScaleHeight*fabs(CosTheta) + 0.5f);

    if(!(*X = (float *)Malloc(sizeof(float)*(*GridWidth)*(*GridHeight)))
        || !(*Y = (float *)Malloc(sizeof(float)*(*GridWidth)*(*GridHeight))))
    {
        *GridWidth = *GridHeight = 0;

        Free(*X);
        return 0;
    }

    XPtr = *X;
    YPtr = *Y;

    /* Create the sampling grid */
    for(n = 0; n < *GridHeight; n++)
        for(m = 0; m < *GridWidth; m++)
        {
            CurX = XStart + m;
            CurY = YStart + n;
            *(XPtr++) = (x0 + CosTheta*CurX - SinTheta*CurY) / Scale;
            *(YPtr++) = (y0 + SinTheta*CurX + CosTheta*CurY) / Scale;
        }

    return 1;
}


typedef struct
{
    float *Coeff;
    int16_t *Pos;
    int Width;
} scalescanfilter;


static void ScaleScan(float *Dest, int DestStride, int DestWidth,
    const float *Src, int SrcStride,
    scalescanfilter Filter)
{
    float Sum;
    int x, k, SrcIndex;


    for(x = 0; x < DestWidth; x++)
    {
        SrcIndex = Filter.Pos[x] * SrcStride;
        Sum = 0;

        for(k = 0; k < Filter.Width; k++, SrcIndex += SrcStride)
            Sum += Filter.Coeff[k] * Src[SrcIndex];

        *Dest = Sum;
        Dest += DestStride;
        Filter.Coeff += Filter.Width;
    }
}


/**
 * @brief Create scanline interpolation filter to be applied with ScaleScan
 * @param Filter pointer to scalescanfilter struct
 * @param DestWidth width after interpolation
 * @param XStart leftmost sampling location (in input coordinates)
 * @param XStep the length between successive samples (in input coordinates)
 * @param SrcWidth width of the input
 * @param Kernel interpolation kernel function to use
 * @param KernelRadius kernel support radius
 * @param KernelNormalize if nonzero, filter rows are normalized to sum to 1
 * @param Boundary boundary handling
 * @return 1 on success, 0 on failure.
 *
 * This routine creates a scalescanfilter for 1-D interpolation of samples at
 * the locations
 *    XStart + n*XStep, n = 0, ..., DestWidth - 1,
 * where the pixels of the source are logically located at the integers.  Half-
 * sample even symmetric extension is used to handle the boundaries.
 */
static int MakeScaleScanFilter(scalescanfilter *Filter,
    int DestWidth, float XStart, float XStep, int SrcWidth,
    float (*Kernel)(float), float KernelRadius, int KernelNormalize,
    boundaryhandling Boundary)
{
    int (*Extension)(int, int) = ExtensionMethod[Boundary];
    const int KernelWidth = (int)ceil(2*KernelRadius);
    const int FilterWidth = (SrcWidth < KernelWidth) ? SrcWidth : KernelWidth;
    float *FilterCoeff = NULL;
    int16_t *FilterPos = NULL;
    float SrcX, Sum;
    int n, DestX, Pos, MaxPos;


    if(!(FilterCoeff = (float *)Malloc(sizeof(float)*FilterWidth*DestWidth))
        || !(FilterPos = (int16_t *)Malloc(sizeof(int16_t)*DestWidth)))
    {
        Free(FilterCoeff);
        Filter->Coeff = NULL;
        Filter->Pos = 0;
        Filter->Width = 0;
        return 0;
    }

    Filter->Coeff = FilterCoeff;
    Filter->Pos = FilterPos;
    Filter->Width = FilterWidth;
    MaxPos = SrcWidth - FilterWidth;

    for(DestX = 0; DestX < DestWidth; DestX++)
    {
        SrcX = XStart + XStep*DestX;
        Pos = (int)ceil(SrcX - KernelRadius);

        if(Pos < 0 || MaxPos < Pos)
        {
            FilterPos[DestX] = CLAMP(Pos, 0, MaxPos);

            for(n = 0; n < FilterWidth; n++)
                FilterCoeff[n] = 0;

            for(n = 0; n < KernelWidth; n++)
                FilterCoeff[Extension(SrcWidth, Pos + n) - FilterPos[DestX]]
                    += Kernel(SrcX - (Pos + n));
        }
        else
        {
            FilterPos[DestX] = Pos;

            for(n = 0; n < FilterWidth; n++)
                FilterCoeff[n] = Kernel(SrcX - (Pos + n));
        }

        if(KernelNormalize)	/* Normalize */
        {
            Sum = 0;

            for(n = 0; n < FilterWidth; n++)
                Sum += FilterCoeff[n];

            for(n = 0; n < FilterWidth; n++)
                FilterCoeff[n] /= Sum;
        }

        FilterCoeff += FilterWidth;
    }

    return 1;
}


/**
 * @brief Scale image with a compact support interpolation kernel
 *
 * @param Dest pointer to memory for holding the interpolated image
 * @param DestWidth width of the output image
 * @param XStart leftmost sampling location (in input coordinates)
 * @param XStep the length between successive samples (in input coordinates)
 * @param DestHeight height of the output image
 * @param YStart uppermost sampling location (in input coordinates)
 * @param YStep the length between successive samples (in input coordinates)
 * @param Src the input image
 * @param SrcWidth, SrcHeight, NumChannels input image dimensions
 * @param Kernel interpolation kernel function to use
 * @param KernelRadius kernel support radius
 * @param KernelNormalize if nonzero, filter rows are normalized to sum to 1
 * @param Boundary boundary handling
 *
 * @return 1 on success, 0 on failure.
 *
 * This is a generic linear interpolation routine to scale an image using any
 * compactly supported interpolation kernel.  The kernel is applied separably
 * along both dimensions.  Half-sample even symmetric extension is used to
 * handle the boundaries.
 *
 * The interpolation is computed so that Dest[m + DestWidth*n] is the
 * interpolation of Src at sampling location
 *     (XStart + m*XStep, YStart + n*YStep)
 * for m = 0, ..., DestWidth - 1, n = 0, ..., DestHeight - 1, where the
 * pixels of Src are located at the integers.
 *
 * The implementation follows the approach taken in ffmpeg's swscale library.
 * First a "scanline filter" is constructed, a sparse matrix such that
 * multiplying with a row of the input image produces an interpolated row in
 * the output image.  Similarly a second matrix is constructed for
 * interpolating columns.  The interpolation itself is then essentially two
 * sparse matrix times dense matrix multiplies.
 */
int LinScale2d(float *Dest, int DestWidth, float XStart, float XStep,
    int DestHeight, float YStart, float YStep,
    const float *Src, int SrcWidth, int SrcHeight, int NumChannels,
    float (*Kernel)(float), float KernelRadius, int KernelNormalize,
    boundaryhandling Boundary)
{
    const int SrcNumPixels = SrcWidth*SrcHeight;
    const int DestNumPixels = DestWidth*DestHeight;
    scalescanfilter HFilter = {NULL, 0, 0}, VFilter = {NULL, 0, 0};
    float *Buf = NULL;
    int x, y, Channel, Success = 0;


    if(!Dest || DestWidth <= 0 || DestHeight <= 0 || !Src
        || SrcWidth <= 0 || SrcHeight <= 0 || NumChannels <= 0
        || !Kernel || KernelRadius < 0)
        return 0;
    if(!(Buf = (float *)Malloc(sizeof(float)*SrcWidth*DestHeight))
        || !MakeScaleScanFilter(&HFilter, DestWidth, XStart, XStep,
            SrcWidth, Kernel, KernelRadius, KernelNormalize, Boundary)
        || !MakeScaleScanFilter(&VFilter, DestHeight, YStart, YStep,
            SrcHeight, Kernel, KernelRadius, KernelNormalize, Boundary))
        goto Catch;

    for(Channel = 0; Channel < NumChannels; Channel++)
    {
        for(x = 0; x < SrcWidth; x++)
            ScaleScan(Buf + x, SrcWidth, DestHeight,
                Src + x, SrcWidth, VFilter);

        for(y = 0; y < DestHeight; y++)
            ScaleScan(Dest + y*DestWidth, 1, DestWidth,
                Buf + y*SrcWidth, 1, HFilter);

        Src += SrcNumPixels;
        Dest += DestNumPixels;
    }

    Success = 1;
Catch:
    Free(VFilter.Pos);
    Free(VFilter.Coeff);
    Free(HFilter.Pos);
    Free(HFilter.Coeff);
    Free(Buf);
    return Success;
}


static int FourierScaleScan(float *Dest,
    int DestStride, int DestScanStride, int DestChannelStride, int DestScanSize,
    const float *Src,
    int SrcStride, int SrcScanStride, int SrcChannelStride, int SrcScanSize,
    int NumScans, int NumChannels, float XStart, double PsfSigma,
    boundaryhandling Boundary)
{
    const int SrcPadScanSize = 2*(SrcScanSize
        - ((Boundary == BOUNDARY_WSYMMETRIC) ? 1:0));
    const int DestPadScanSize = (Boundary == BOUNDARY_HSYMMETRIC) ?
        (2*DestScanSize)
        : (2*DestScanSize - floor((2.0f*DestScanSize)/SrcScanSize + 0.5f));
    const int ReflectOffset = SrcPadScanSize
        - ((Boundary == BOUNDARY_HSYMMETRIC) ? 1:0);
    const int SrcDftSize = SrcPadScanSize/2 + 1;
    const int DestDftSize = DestPadScanSize/2 + 1;
    const int BufSpatialNumEl = DestPadScanSize*NumScans*NumChannels;
    const int BufDftNumEl = 2*DestDftSize*NumScans*NumChannels;
    float *BufSpatial = NULL, *BufDft = NULL, *Modulation = NULL, *Ptr;
    fftwf_plan Plan = 0;
    fftwf_iodim Dims[1], HowManyDims[1];
    float Temp, Denom;
    int i, Scan, Channel, Success = 0;


    if((Boundary != BOUNDARY_HSYMMETRIC && Boundary != BOUNDARY_WSYMMETRIC)
        || !(BufSpatial = (float *)fftwf_malloc(sizeof(float)*BufSpatialNumEl))
        || !(BufDft = (float *)fftwf_malloc(sizeof(float)*BufDftNumEl)))
        goto Catch;

    if(XStart != 0)
    {
        if(!(Modulation = (float *)Malloc(sizeof(float)*2*DestDftSize)))
            goto Catch;

        for(i = 0; i < DestDftSize; i++)
        {
            Temp = M_2PI*XStart*i/SrcPadScanSize;
            Modulation[2*i + 0] = cos(Temp);
            Modulation[2*i + 1] = sin(Temp);
        }
    }

    /* Fill BufSpatial with the input and symmetrize */
    for(Channel = 0; Channel < NumChannels; Channel++)
    {
        for(Scan = 0; Scan < NumScans; Scan++)
        {
            for(i = 0; i < SrcScanSize; i++)
                BufSpatial[i + SrcPadScanSize*(Scan + NumScans*Channel)]
                    = Src[SrcStride*i + SrcScanStride*Scan
                    + SrcChannelStride*Channel];

            for(; i < SrcPadScanSize; i++)
                BufSpatial[i + SrcPadScanSize*(Scan + NumScans*Channel)]
                    = Src[SrcStride*(ReflectOffset - i)
                    + SrcScanStride*Scan + SrcChannelStride*Channel];
        }
    }

    /* Initialize DFT buffer to zeros (there is no "fftwf_calloc").  Note that
    it is not safely portable to use memset for this purpose.
    http://c-faq.com/malloc/calloc.html  */
    for(i = 0; i < BufDftNumEl; i++)
        BufDft[i] = 0.0f;

    /* Perform DFT real-to-complex transform */
    Dims[0].n = SrcPadScanSize;
    Dims[0].is = 1;
    Dims[0].os = 1;
    HowManyDims[0].n = NumScans*NumChannels;
    HowManyDims[0].is = SrcPadScanSize;
    HowManyDims[0].os = DestDftSize;

    if(!(Plan = fftwf_plan_guru_dft_r2c(1, Dims, 1, HowManyDims, BufSpatial,
        (fftwf_complex *)BufDft, FFTW_ESTIMATE | FFTW_DESTROY_INPUT)))
        goto Catch;

    fftwf_execute(Plan);
    fftwf_destroy_plan(Plan);

    if(PsfSigma == 0)
        for(Channel = 0, Ptr = BufDft; Channel < NumChannels; Channel++)
            for(Scan = 0; Scan < NumScans; Scan++, Ptr += 2*DestDftSize)
                for(i = 0; i < SrcDftSize; i++)
                {
                    Ptr[2*i + 0] /= SrcPadScanSize;
                    Ptr[2*i + 1] /= SrcPadScanSize;
                }
    else
    {
        /* Also divide by the Gaussian point spread function in this case */
        Temp = SrcPadScanSize / (M_2PI * PsfSigma);
        Temp = 2*Temp*Temp;

        for(i = 0; i < SrcDftSize; i++)
        {
            if(i <= DestScanSize)
                Denom = exp(-(i*i)/Temp);
            else
                Denom = exp(-((DestPadScanSize - i)*(DestPadScanSize - i))/Temp);

            Denom *= SrcPadScanSize;

            for(Channel = 0; Channel < NumChannels; Channel++)
                for(Scan = 0; Scan < NumScans; Scan++)
                {
                    BufDft[2*(i + DestDftSize*(Scan + NumScans*Channel)) + 0]
                        /= Denom;
                    BufDft[2*(i + DestDftSize*(Scan + NumScans*Channel)) + 1]
                        /= Denom;
                }
        }
    }

    /* If XStart is nonzero, modulate the DFT to translate the result */
    if(XStart != 0)
        for(Channel = 0, Ptr = BufDft; Channel < NumChannels; Channel++)
            for(Scan = 0; Scan < NumScans; Scan++, Ptr += 2*DestDftSize)
                for(i = 0; i < SrcDftSize; i++)
                {
                    /* Complex multiply */
                    Temp = Ptr[2*i + 0]*Modulation[2*i + 0]
                        - Ptr[2*i + 1]*Modulation[2*i + 1];
                    Ptr[2*i + 1] = Ptr[2*i + 0]*Modulation[2*i + 1]
                        + Ptr[2*i + 1]*Modulation[2*i + 0];
                    Ptr[2*i + 0] = Temp;
                }

    /* Perform inverse DFT complex-to-real transform */
    Dims[0].n = DestPadScanSize;
    Dims[0].is = 1;
    Dims[0].os = 1;
    HowManyDims[0].n = NumScans*NumChannels;
    HowManyDims[0].is = DestDftSize;
    HowManyDims[0].os = DestPadScanSize;

    if(!(Plan = fftwf_plan_guru_dft_c2r(1, Dims, 1, HowManyDims,
        (fftwf_complex *)BufDft, BufSpatial,
        FFTW_ESTIMATE | FFTW_DESTROY_INPUT)))
        goto Catch;

    fftwf_execute(Plan);
    fftwf_destroy_plan(Plan);

    /* Fill Dest with the result (and trim padding) */
    for(Channel = 0; Channel < NumChannels; Channel++)
    {
        for(Scan = 0; Scan < NumScans; Scan++)
        {
            for(i = 0; i < DestScanSize; i++)
                Dest[DestStride*i + DestScanStride*Scan
                    + DestChannelStride*Channel]
                    = BufSpatial[i + DestPadScanSize*(Scan + NumScans*Channel)];
        }
    }

    Success = 1;
Catch:
    Free(Modulation);
    if(BufDft)
        fftwf_free(BufDft);
    if(BufSpatial)
        fftwf_free(BufSpatial);
    fftwf_cleanup();
    return Success;
}


/**
 * @brief Scale image with Fourier zero padding
 *
 * @param Dest pointer to memory for holding the interpolated image
 * @param DestWidth output image width
 * @param XStart leftmost sample location (in input coordinates)
 * @param DestHeight output image height
 * @param YStart uppermost sample location (in input coordinates)
 * @param Src the input image
 * @param SrcWidth, SrcHeight, NumChannels input image dimensions
 * @param PsfSigma Gaussian PSF standard deviation
 *  @param Boundary boundary handling
 *
 * @return 1 on success, 0 on failure.
 *
 * The image is first mirror folded with half-sample even symmetry to avoid
 * boundary artifacts, then transformed with a real-to-complex DFT.
 *
 * The interpolation is computed so that Dest[m + DestWidth*n] is the
 * interpolation of Input at sampling location
 *    (XStart + m*SrcWidth/DestWidth, YStart + n*SrcHeight/DestHeight)
 * for m = 0, ..., DestWidth - 1, n = 0, ..., DestHeight - 1, where the
 * pixels of Src are located at the integers.
 */
int FourierScale2d(float *Dest, int DestWidth, float XStart,
    int DestHeight, float YStart,
    const float *Src, int SrcWidth, int SrcHeight, int NumChannels,
    double PsfSigma, boundaryhandling Boundary)
{
    float *Buf = NULL;
    int Success = 0;

    unsigned long StartTime, StopTime;


    if(!Dest || DestWidth < SrcWidth || DestHeight < SrcHeight || !Src
        || SrcWidth <= 0 || SrcHeight <= 0 || NumChannels <= 0 || PsfSigma < 0
        || !(Buf = (float *)Malloc(sizeof(float)*SrcWidth*DestHeight*3)))
        return 0;

    StartTime = Clock();

    /* Scale the image vertically */
    if(!FourierScaleScan(Buf, SrcWidth, 1, SrcWidth*DestHeight, DestHeight,
        Src, SrcWidth, 1, SrcWidth*SrcHeight, SrcHeight,
        SrcWidth, 3, YStart, PsfSigma, Boundary))
        goto Catch;

    /* Scale the image horizontally */
    if(!FourierScaleScan(Dest, 1, DestWidth, DestWidth*DestHeight, DestWidth,
        Buf, 1, SrcWidth, SrcWidth*DestHeight, SrcWidth,
        DestHeight, 3, XStart, PsfSigma, Boundary))
        goto Catch;

    StopTime = Clock();
    printf("CPU Time: %.3f s\n\n", 0.001*(StopTime - StartTime));

    Success = 1;
Catch:
    Free(Buf);
    return Success;
}
