/**
 * @file lprefilt.h
 * @brief Prefiltering for linear interpolation
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

#ifndef _LPREFILT_H_
#define _LPREFILT_H_

#include "linterp.h"

void PrefilterImage(float *Data, int Width, int Height, int NumChannels,
    const float *alpha, int NumFilterPairs, float ConstantFactor,
    boundaryhandling Boundary);

void PsfConvCoeff(float *Coeff, int NumCoeffs,
    float (*Psf)(float, const void*), const void *PsfParams,
    float (*Kernel)(float), float KernelRadius, int KernelNormalize);

int PsfPreFilter(float *Data, int Width, int Height, int NumChannels,
    const float *Coeff, int NumCoeffs, boundaryhandling Boundary);

#endif /* _LPREFILT_H_ */
