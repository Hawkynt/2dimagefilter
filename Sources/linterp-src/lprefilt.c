/**
 * @file lprefilt.c
 * @brief Prefiltering for linear interpolation
 * @author Pascal Getreuer <getreuer@gmail.com>
 *
 * This file implements the prefiltering \c PrefilterImage on the input
 * image needed before performing interpolation needed with B-splines
 * and o-Moms.  Also implemented here is \c PsfPreFilter, the
 * prefiltering adjustment needed when interpolating with a PSF.
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

#include <stdlib.h>
#include <math.h>
#include <fftw3.h>
#include "adaptlob.h"
#include "lprefilt.h"


/**
 * @brief 1D in-place filtering with a first-order recursive filter pair
 * @param Data pointer to data to be filtered
 * @param Stride stride between successive elements
 * @param N number of samples
 * @param alpha filter coefficient
 * @param Boundary the kind of boundary handling to use
 *
 * Applies the causal recursive filter
 *     1/(1 - alpha z^-1)
 * followed by the anti-causal recursive filter
 *     -alpha/(1 - alpha z).
 * The coefficient alpha must satisify |alpha| < 1 for stability.
 *
 * With respect to boundary handling, filtering is computed with relative
 * accuracy Eps = 1e-4 for half- and whole-sample symmetric boundaries and it
 * is exact for constant extension.  Note, however, that for constant extension
 * the infinite grid result is not exactly constant beyond the boundaries
 * (rather it decays to constant).
 */
static void PrefilterScan(float *Data, int Stride, int N, float alpha,
    boundaryhandling Boundary)
{
    const float Eps = 1e-4;
    float Sum, Weight, Last;
    int i, iEnd, n0;

    n0 = (int)ceil(log(Eps)/log(fabs(alpha)));

    if(n0 > N)
        n0 = N;

    switch(Boundary)
    {
        case BOUNDARY_CONSTANT:
            Sum = Data[0]/(1 - alpha);
            break;
        case BOUNDARY_WSYMMETRIC:
            Sum = Data[0];
            Weight = 1;
            iEnd = n0*Stride;

            for(i = Stride; i < iEnd; i += Stride)
            {
                Weight *= alpha;
                Sum += Data[i]*Weight;
            }
            break;
        default: /* BOUNDARY_HSYMMETRIC */
            Sum = Data[0]*(1 + alpha);
            Weight = alpha;
            iEnd = n0*Stride;

            for(i = Stride; i < iEnd; i += Stride)
            {
                Weight *= alpha;
                Sum += Data[i]*Weight;
            }
            break;
    }

    Last = Data[0] = Sum;
    iEnd = (N - 1)*Stride;

    for(i = Stride; i < iEnd; i += Stride)
    {
        Data[i] += alpha*Last;
        Last = Data[i];
    }

    switch(Boundary)
    {
        case BOUNDARY_CONSTANT:
            Last = Data[iEnd] = (alpha*(-Data[iEnd] + (alpha - 1)*alpha*Last))
                /((alpha - 1)*(alpha*alpha - 1));
            break;
        case BOUNDARY_HSYMMETRIC:
            Data[iEnd] += alpha*Last;
            Last = Data[iEnd] *= alpha/(alpha - 1);
            break;
        case BOUNDARY_WSYMMETRIC:
            Data[iEnd] += alpha*Last;
            Last = Data[iEnd] = (alpha/(alpha*alpha - 1))
                * ( Data[iEnd] + alpha*Data[iEnd - Stride] );
            break;
    }

    for(i = iEnd - Stride; i >= 0; i -= Stride)
    {
        Data[i] = alpha*(Last - Data[i]);
        Last = Data[i];
    }
}


/**
 * @brief Apply a cascade of first-order recursive filter pairs to an image
 * @param Data the image data
 * @param Width, Height, NumChannels image dimensions
 * @param alpha array of alpha values
 * @param NumFilterPairs the number of filter pairs
 * @param ConstantFactor constant multiplicative factor to apply
 * @param Boundary the kind of boundary handling to use
 */
void PrefilterImage(float *Data, int Width, int Height, int NumChannels,
    const float *alpha, int NumFilterPairs, float ConstantFactor,
    boundaryhandling Boundary)
{
    const int NumPixels = Width*Height;
    int k, x, y, Channel;


    /* Square the ConstantFactor for two spatial dimensions */
    ConstantFactor = ConstantFactor*ConstantFactor;

    for(Channel = 0; Channel < NumChannels; Channel++)
    {
        for(x = 0; x < Width; x++)
            for(k = 0; k < NumFilterPairs; k++)
                PrefilterScan(Data + x, Width, Height, alpha[k], Boundary);

        for(y = 0; y < Height; y++)
            for(k = 0; k < NumFilterPairs; k++)
                PrefilterScan(Data + Width*y, 1, Width, alpha[k], Boundary);

        for(k = 0; k < NumPixels; k++)
            Data[k] *= ConstantFactor;

        Data += NumPixels;
    }
}


typedef struct {
    float (*Kernel)(float);
    float KernelRadius;
    float (*Psf)(float, const void*);
    const void *PsfParams;
    float x;
} convolutionparams;


static float ConvIntegrand(float t, const void *Params)
{
    float (*Kernel)(float) = ((convolutionparams*)Params)->Kernel;
    float (*Psf)(float, const void*) = ((convolutionparams*)Params)->Psf;
    const void *PsfParams = ((convolutionparams*)Params)->PsfParams;
    float x = ((convolutionparams*)Params)->x;

    return Kernel(t) * Psf(x - t, PsfParams);
}


static float ConvIntegrandNormalized(float t, const void *Params)
{
    float (*Kernel)(float) = ((convolutionparams*)Params)->Kernel;
    const int R = 2*ceil(((convolutionparams*)Params)->KernelRadius);
    float (*Psf)(float, const void*) = ((convolutionparams*)Params)->Psf;
    const void *PsfParams = ((convolutionparams*)Params)->PsfParams;
    float Sum, x = ((convolutionparams*)Params)->x;
    int r;

    for(r = -R, Sum = 0; r <= R; r++)
        Sum += Kernel(t - r);

    return (Kernel(t)/Sum) * Psf(x - t, PsfParams);
}


/**
 * @brief Compute PSF*kernel convolution coefficients
 * @param Coeff pointer to coefficients destination array
 * @param NumCoeffs number of coefficients to compute
 * @param Psf the point-spread function (PSF)
 * @param PsfParams parameters to pass to Psf
 * @param Kernel the interpolation kernel or basis function to use
 * @param KernelRadius support radius of Kernel
 * @param KernelNormalize if nonzero, Kernel is normalized to sum to 1
 *
 * The coefficients are the convolution
 *    Coeff[m] = (Psf * Kernel)(m)
 * for m = 0, ..., NumCoeffs - 1.  Integration is approximated using adaptive
 * Gauss-Lobatto quadrature.
 *
 * The result of this routine is used by \c PsfPreFilter.
 */
void PsfConvCoeff(float *Coeff, int NumCoeffs,
    float (*Psf)(float, const void*), const void *PsfParams,
    float (*Kernel)(float), float KernelRadius, int KernelNormalize)
{
    float (*ConvFun)(float, const void*) = ConvIntegrand;
    convolutionparams ConvParams;
    int m;

    ConvParams.Kernel = Kernel;
    ConvParams.KernelRadius = KernelRadius;
    ConvParams.Psf = Psf;
    ConvParams.PsfParams = PsfParams;

    if(KernelNormalize)
        ConvFun = ConvIntegrandNormalized;

    for(m = 0; m < NumCoeffs; m++)
    {
        ConvParams.x = m;
        Coeff[m] = AdaptLob(ConvFun, -KernelRadius, KernelRadius,
            1e-7f, (const void *)&ConvParams);
    }
}


/**
* @brief 1D in-place prefiltering to adjust interpolation for PSF
* @param Data pointer to data to be filtered
* @param Stride stride between successive elements
* @param ScanSize number of samples in a scan
* @param ScanStride stride between successive scans
* @param NumScans number of scans
* @param ChannelStride stride between successive channels
* @param NumChannels number of channels
* @param Coeff the PSF adjustment coefficients
* @param NumCoeffs number of coefficients
* @param Boundary the boundary handling to use
*
* Convolves Data with the inverse of
*     Coeff[0] + Coeff[1] (z + z^-1) + Coeff[2] (z^2 + z^-2) + ...
* via the DCT transform.  Boundary handling can be either half- or whole-
* sample symmetric (constant extension is not supported).
*
* Half-sample symmetric convolution is performed through DCT transforms as
*   f * g = DCT-III( DCT-I(f) . DCT-II(g) )
* where * denotes convolution and . denotes pointwise multiplication.  Signal
* f is whole-sample symmetric, signal g is half-sample symmetric, and the
* result is half-sample symmetric.
*
* Whole-sample symmetric convolution is performed as
*   f * g = DCT-I( DCT-I(f) . DCT-I(g) ).
*/
static int PsfPreFilterScan(float *Data, int Stride, int ScanSize,
    int ScanStride, int NumScans, int ChannelStride, int NumChannels,
    const float *Coeff, int NumCoeffs, boundaryhandling Boundary)
{
    const int PadCoeff = ScanSize + ((Boundary == BOUNDARY_HSYMMETRIC) ? 1:0);
    float *Buf = NULL, *CoeffDct = NULL;
    fftwf_plan Plan = 0;
    fftwf_iodim Dims[1], HowManyDims[2];
    fftw_r2r_kind Kind[1];
    int i, Scan, Channel, Success = 0;


    if((Boundary != BOUNDARY_HSYMMETRIC && Boundary != BOUNDARY_WSYMMETRIC)
        || !(Buf = (float *)fftwf_malloc(sizeof(float)*ScanSize*NumScans*NumChannels))
        || !(CoeffDct = (float *)fftwf_malloc(sizeof(float)*PadCoeff)))
        goto Catch;

    for(i = 0; i < NumCoeffs && i < PadCoeff; i++)
        Buf[i] = Coeff[i];
    for(; i < PadCoeff; i++)
        Buf[i] = 0;

    /* Perform DCT-I */
    if(!(Plan = fftwf_plan_r2r_1d(PadCoeff, Buf, CoeffDct,
        FFTW_REDFT00, FFTW_ESTIMATE | FFTW_DESTROY_INPUT)))
        goto Catch;

    fftwf_execute(Plan);
    fftwf_destroy_plan(Plan);

    /* Incorporate the normalization scale factor into the coefficients */
    for(i = 0; i < ScanSize; i++)
        CoeffDct[i] *= 2*(PadCoeff - 1);

    /* Forward transform of the image data */
    Dims[0].n = ScanSize;
    Dims[0].is = Stride;
    Dims[0].os = 1;
    HowManyDims[0].n = NumChannels;
    HowManyDims[0].is = ChannelStride;
    HowManyDims[0].os = ScanSize*NumScans;
    HowManyDims[1].n = NumScans;
    HowManyDims[1].is = ScanStride;
    HowManyDims[1].os = ScanSize;

    /* Use DCT-II for half-sample symmetric boundaries,
    DCT-I for whole-sample symmetric. */
    Kind[0] = (Boundary == BOUNDARY_HSYMMETRIC) ? FFTW_REDFT10 : FFTW_REDFT00;

    if(!(Plan = fftwf_plan_guru_r2r(1, Dims, 2, HowManyDims, Data,
        Buf, Kind, FFTW_ESTIMATE | FFTW_DESTROY_INPUT)))
        goto Catch;

    fftwf_execute(Plan);
    fftwf_destroy_plan(Plan);

    /* Divide */
    for(Channel = 0; Channel < NumChannels; Channel++)
    {
        for(Scan = 0; Scan < NumScans; Scan++)
        {
            for(i = 0; i < ScanSize; i++)
                Buf[i + ScanSize*(Scan + NumScans*Channel)] /= CoeffDct[i];
        }
    }

    /* Perform inverse transform */
    Dims[0].n = ScanSize;
    Dims[0].is = 1;
    Dims[0].os = Stride;
    HowManyDims[0].n = NumChannels;
    HowManyDims[0].is = ScanSize*NumScans;
    HowManyDims[0].os = ChannelStride;
    HowManyDims[1].n = NumScans;
    HowManyDims[1].is = ScanSize;
    HowManyDims[1].os = ScanStride;

    /* Use DCT-III (inverse of DCT-II) for half-sample symmetric boundaries,
    DCT-I (inverse of DCT-I) for whole-sample symmetric. */
    Kind[0] = (Boundary == BOUNDARY_HSYMMETRIC) ? FFTW_REDFT01 : FFTW_REDFT00;

    if(!(Plan = fftwf_plan_guru_r2r(1, Dims, 2, HowManyDims, Buf,
        Data, Kind, FFTW_ESTIMATE | FFTW_DESTROY_INPUT)))
        goto Catch;

    fftwf_execute(Plan);
    fftwf_destroy_plan(Plan);
    Success = 1;
Catch:
    if(CoeffDct)
        fftwf_free(CoeffDct);
    if(Buf)
        fftwf_free(Buf);
    fftwf_cleanup();
    return Success;
}


/**
 * @brief Prefiltering to adjust interpolation for PSF
 * @param Data pointer to image data to be filtered
 * @param Width, Height, NumChannels image dimensions
 * @param Coeff the PSF adjustment coefficients from \c PsfConvCoeff
 * @param NumCoeffs number of coefficients
 * @param Boundary boundary handling
 *
 * Convolves Data with the inverse of
 *     Coeff[0] + Coeff[1] (z + z^-1) + Coeff[2] (z^2 + z^-2) + ...
 * via the DCT transform.  Boundary handling can be either half- or whole-
 * sample symmetric (constant extension is not supported).
 *
 * Half-sample symmetric boundary handling is considerably more efficient,
 * about 25% shorter runtime than when using whole-sample symmetry.
 */
int PsfPreFilter(float *Data, int Width, int Height, int NumChannels,
    const float *Coeff, int NumCoeffs, boundaryhandling Boundary)
{
    if(!Data || !Coeff)
        return 0;

    if(!PsfPreFilterScan(Data, Width, Height, 1, Width, Width*Height,
            NumChannels, Coeff, NumCoeffs, Boundary))
        return 0;

    if(!PsfPreFilterScan(Data, 1, Width, Width, Height, Width*Height,
            NumChannels, Coeff, NumCoeffs, Boundary))
        return 0;

    return 1;
}
