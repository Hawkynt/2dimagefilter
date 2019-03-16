/**
 * @file lkernels.h
 * @brief Interpolation kernel and basis functions for linear interpolation
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

#ifndef _LKERNELS_H_
#define _LKERNELS_H_

/** @brief struct describing a linear interpolation method */
typedef const struct interpmethodstruct
{
    /** @brief Name of the method */
    const char *Name;
    /** @brief Interpolation kernel or basis function */
    float (*Kernel)(float);
    /** @brief Support radius of Kernel */
    float KernelRadius;
    /** @brief Nonzero value indicates that the kernel should be normalized */
    int KernelNormalize;
    /** @brief The number of filter pairs for prefilting */
    int PrefilterNumAlpha;
    /** @brief Array of prefilter coefficients */
    const float *PrefilterAlpha;
    /** @brief Constant scale factor to use wiht prefiltering */
    float PrefilterScale;
} interpmethod;

interpmethod *GetInterpMethod(const char *Name);

float NearestNeighborKernel(float x);

#endif /* _LKERNELS_H_ */
