/**
 * @file linterp.h
 * @brief Linear interpolation
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

#ifndef _LINTERP_H_
#define _LINTERP_H_

typedef enum
{
    BOUNDARY_CONSTANT = 0,
    BOUNDARY_HSYMMETRIC = 1,
    BOUNDARY_WSYMMETRIC = 2
} boundaryhandling;

int LinScale2d(float *Dest, int DestWidth, float XStart, float XStep,
    int DestHeight, float YStart, float YStep,
    const float *Src, int SrcWidth, int SrcHeight, int NumChannels,
    float (*Kernel)(float), float KernelRadius, int KernelNormalize,
    boundaryhandling Boundary);

int FourierScale2d(float *Dest, int DestWidth, float XStart,
    int DestHeight, float YStart,
    const float *Src, int SrcWidth, int SrcHeight, int NumChannels,
    double PsfSigma, boundaryhandling Boundary);

int LinInterp2d(float *Dest, const float *Src, int SrcWidth, int SrcHeight,
    float *X, float *Y, int NumSamples,
    float (*Kernel)(float), float KernelRadius, int KernelNormalize,
    boundaryhandling Boundary);

int MakeScaleRotationGrid(float **X, float **Y, int *GridWidth,
    int *GridHeight, int SrcWidth, int SrcHeight, float Scale, float Theta);

#include "lprefilt.h"
#include "lkernels.h"

#endif /* _LINTERP_H_ */
