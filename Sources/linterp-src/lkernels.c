/**
 * @file lkernels.c
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

#include <math.h>
#include <string.h>
#include "lkernels.h"

#ifndef M_PI
/** @brief The constant pi */
#define M_PI    3.14159265358979323846264338327950288
#endif

/** @brief Macro to get the number of elements in a static array */
#define NUMEL(x)    (sizeof(x)/sizeof(*(x)))


/**
 * @brief Bilinear interpolation kernel (KernelRadius = 0.5)
 * @param x coordinate at which to evaluate the kernel
 * @return the kernel value at x
 */
float NearestNeighborKernel(float x)
{
    if(-0.5f <= x && x < 0.5f)
        return 1;
    else
        return 0;
}


/** @brief Bilinear interpolation kernel (KernelRadius = 1) */
static float BilinearKernel(float x)
{
    x = fabs(x);

    if(x < 1)
        return 1 - x;
    else
        return 0;
}


/** @brief Bicubic interpolation kernel (KernelRadius = 2) */
static float BicubicKernel(float x)
{
    const float alpha = -0.5f;

    x = fabs(x);

    if(x < 2)
    {
        if(x <= 1)
            return ((alpha + 2)*x - (alpha + 3))*x*x + 1;
        else
            return ((alpha*x - 5*alpha)*x + 8*alpha)*x - 4*alpha;
    }
    else
        return 0;
}


/** @brief Lanczos-2 interpolation kernel (KernelRadius = 2) */
static float Lanczos2Kernel(float x)
{
    if(-2 < x && x < 2)
    {
        if(x != 0)
            return sin(M_PI*x)*sin((M_PI/2)*x) / ((M_PI*M_PI/2)*x*x);
        else
            return 1;
    }
    else
        return 0;
}


/** @brief Lanczos-3 interpolation kernel (KernelRadius = 3) */
static float Lanczos3Kernel(float x)
{
    if(-3 < x && x < 3)
    {
        if(x != 0)
            return sin(M_PI*x)*sin((M_PI/3)*x) / ((M_PI*M_PI/3)*x*x);
        else
            return 1;
    }
    else
        return 0;
}


/** @brief Lanczos-4 interpolation kernel (KernelRadius = 4) */
static float Lanczos4Kernel(float x)
{
    if(-4 < x && x < 4)
    {
        if(x != 0)
            return sin(M_PI*x)*sin((M_PI/4)*x) / ((M_PI*M_PI/4)*x*x);
        else
            return 1;
    }
    else
        return 0;
}


/** @brief Quadratic B-spline prefilter */
static const float BSpline2Prefilter[1] =
    {-1.715728752538099e-1}; /* exact value: -3 + sqrt(8) */

/** @brief Quadratic B-spline kernel (KernelRadius = 1.5) */
static float BSpline2Kernel(float x)
{
    x = fabs(x);

    if(x <= 0.5f)
        return 0.75f - x*x;
    else if(x < 1.5f)
    {
        x = 1.5f - x;
        return x*x/2;
    }
    else
        return 0;
}


/** @brief Quadratic Schaum kernel (KernelRadius = 1.5) */
static float Schaum2Kernel(float x)
{
    x = fabs(x);

    /* This kernel is discontinuous.  At discontinuous points, it takes the
    average value of the left and right limits. */
    if(x < 0.5f)
        return 1 - x*x;
    else if(x == 0.5f)
        return 0.5625;
    else if(x < 1.5f)
        return (x - 3)*x/2 + 1;
    else if(x == 1.5f)
        return -0.0625;
    else
        return 0;
}


/** @brief Cubic Schaum kernel (KernelRadius = 2) */
static float Schaum3Kernel(float x)
{
    x = fabs(x);

    if(x <= 1)
        return ((x - 2)*x - 1)*x/2 + 1;
    else if(x < 2)
        return ((-x + 6)*x - 11)*x/6 + 1;
    else
        return 0;
}



/** @brief Cubic B-spline prefilter coefficients */
static const float BSpline3Prefilter[1] =
    {-2.679491924311227e-1}; /* exact value: -2 + sqrt(3) */

/** @brief Cubic B-spline kernel (KernelRadius = 2) */
static float BSpline3Kernel(float x)
{
    x = fabs(x);

    if(x < 1)
        return (x/2 - 1)*x*x + 0.66666666666666667f;
    else if(x < 2)
    {
        x = 2 - x;
        return x*x*x/6;
    }
    else
        return 0;
}


/** @brief Quintic B-spline prefilter coefficients */
static const float BSpline5Prefilter[2] =
    {-4.309628820326465e-2, /* exact: sqrt(13*sqrt(105)+135)/sqrt(2)-sqrt(105)/2-13/2.0 */
    -4.305753470999738e-1}; /* exact: sqrt(105)/2+sqrt(135-13*sqrt(105))/sqrt(2)-13/2.0 */

/** @brief Quintic B-spline kernel (KernelRadius = 3) */
static float BSpline5Kernel(float x)
{
    x = fabs(x);

    if(x <= 1)
    {
        float xSqr = x*x;
        return (((-10*x + 30)*xSqr - 60)*xSqr + 66) / 120;
    }
    else if(x < 2)
    {
        x = 2 - x;
        return (1 + (5 + (10 + (10 + (5 - 5*x)*x)*x)*x)*x) / 120;
    }
    else if(x < 3)
    {
        float xSqr;
        x = 3 - x;
        xSqr = x*x;
        return xSqr*xSqr*x / 120;
    }
    else
        return 0;
}


/** @brief Septic B-spline prefilter coefficients */
static const float BSpline7Prefilter[3] =
    {-9.148694809608277e-3, -1.225546151923267e-1, -5.352804307964382e-1};

/** @brief Septic B-spline kernel (KernelRadius = 4) */
static float BSpline7Kernel(float x)
{
    x = fabs(x);

    if(x <= 1)
    {
        float xSqr = x*x;
        return ((((35*x - 140)*xSqr + 560)*xSqr - 1680)*xSqr + 2416) / 5040;
    }
    else if(x <= 2)
    {
        x = 2 - x;
        return (120 + (392 + (504 + (280 + (-84 + (-42 +
            21*x)*x)*x*x)*x)*x)*x) / 5040;
    }
    else if(x < 3)
    {
        x = 3 - x;
        return (((((((-7*x + 7)*x + 21)*x + 35)*x + 35)*x
            + 21)*x + 7)*x + 1) / 5040;
    }
    else if(x < 4)
    {
        float xSqr;
        x = 4 - x;
        xSqr = x*x;
        return xSqr*xSqr*xSqr*x / 5040;
    }
    else
        return 0;
}


/** @brief Nonic B-spline prefilter coefficients */
static const float BSpline9Prefilter[4] =
    {-2.121306903180818e-3, -4.322260854048175e-2,
    -2.017505201931532e-1, -6.079973891686259e-1};

/** @brief Nonic B-spline kernel (KernelRadius = 5) */
static float BSpline9Kernel(float x)
{
    x = fabs(x);

    if(x <= 1)
    {
        float xSqr = x*x;
        return (((((-63*x + 315)*xSqr - 2100)*xSqr + 11970)*xSqr
            - 44100)*xSqr + 78095) / 181440;
    }
    else if(x <= 2)
    {
        x = 2 - x;
        return (14608 + (36414 + (34272 + (11256 + (-4032 + (-4284 + (-672
            + (504 + (252 - 84*x)*x)*x)*x)*x)*x)*x)*x)*x) / 362880;
    }
    else if(x <= 3)
    {
        x = 3 - x;
        return (502 + (2214 + (4248 + (4536 + (2772 + (756 + (-168 + (-216
            + (-72 + 36*x)*x)*x)*x)*x)*x)*x)*x)*x) / 362880;
    }
    else if(x < 4)
    {
        x = 4 - x;
        return (1 + (9 + (36 + (84 + (126 + (126 + (84 + (36 + (9
            - 9*x)*x)*x)*x)*x)*x)*x)*x)*x) / 362880;
    }
    else if(x < 5)
    {
        float xCube;
        x = 5 - x;
        xCube = x*x*x;
        return xCube*xCube*xCube / 362880;
    }
    else
        return 0;
}


/** @brief 11th-Degree B-spline prefilter coefficients */
static const float BSpline11Prefilter[5] =
    {-5.105575344465021e-4, -1.666962736623466e-2, -8.975959979371331e-2,
    -2.721803492947859e-1, -6.612660689007345e-1};

/** @brief 11th-Degree B-spline kernel (KernelRadius = 6) */
static float BSpline11Kernel(float x)
{
    x = fabs(x);

    if(x <= 1)
    {
        float xSqr = x*x;
        return (15724248 + (-7475160 + (1718640 + (-255024 + (27720
            + (-2772 + 462*x)*xSqr)*xSqr)*xSqr)*xSqr)*xSqr) / 39916800;
    }
    else if(x <= 2)
    {
        x = 2 - x;
        return (2203488 + (4480872 + (3273600 + (574200 + (-538560
            + (-299376 + (39600 + (7920 + (-2640 + (-1320
            + 330*x)*x)*x)*x)*x*x)*x)*x)*x)*x)*x) / 39916800;
    }
    else if(x <= 3)
    {
        x = 3 - x;
        return (152637 + (515097 + (748275 + (586575 + (236610 + (12474
            + (-34650 + (-14850 + (-495 + (1485
            + (495-165*x)*x)*x)*x)*x)*x)*x)*x)*x)*x)*x) / 39916800;
    }
    else if(x < 4)
    {
        x = 4 - x;
        return (2036 + (11132 + (27500 + (40260 + (38280 + (24024 + (9240
            + (1320 + (-660 + (-440 + (-110
            + 55*x)*x)*x)*x)*x)*x)*x)*x)*x)*x)*x) / 39916800;
    }
    else if(x < 5)
    {
        x = 5 - x;
        return (1 + (11 + (55 + (165 + (330 + (462 + (462 + (330 + (165
            + (55 + (11 - 11*x)*x)*x)*x)*x)*x)*x)*x)*x)*x)*x) / 39916800;
    }
    else if(x < 6)
    {
        float xSqr, xPow4;
        x = 6 - x;
        xSqr = x*x;
        xPow4 = xSqr*xSqr;
        return xPow4*xPow4*xSqr*x / 39916800;
    }
    else
        return 0;
}


/** @brief Cubic o-Moms prefilter coefficients */
static const float OMoms3Prefilter[1] =
    {-3.441311542550502e-1}; /* exact: (sqrt(105) - 13)/8 */

/** @brief Cubic o-Moms kernel (KernelRadius = 2) */
static float OMoms3Kernel(float x)
{
    x = fabs(x);

    if(x < 1)
        return ((x/2 - 1)*x + 1/14.0f)*x + 13/21.0f;
    else if(x < 2)
        return ((-x/6 + 1)*x - 85/42.0f)*x + 29/21.0f;
    else
        return 0;
}


/** @brief Quintic o-Moms prefilter coefficients */
static const float OMoms5Prefilter[2] =
    {-7.092571896868541e-2, -4.758127100084396e-1};

/** @brief Quintic oMoms kernel (KernelRadius = 3) */
static float OMoms5Kernel(float x)
{
    x = fabs(x);

    if(x <= 1)
        return (((((-10*x + 30)*x - (200/33.0f))*x
            - (540/11.0f))*x - (5/33.0f))*x + (687/11.0)) / 120;
    else if(x < 2)
        return (((((330*x - 2970)*x + 10100)*x
            - 14940)*x + 6755)*x + 2517)/7920;
    else if(x < 3)
    {
        float xSqr;
        x = 3 - x;
        xSqr = x*x;
        return ((xSqr + (20/33.0f))*xSqr + (1/66.0f))*x / 120;
    }
    else
        return 0;
}

/** @brief Septic oMoms prefilter coefficients */
static const float OMoms7Prefilter[3] =
    {-1.976842538386140e-2, -1.557007746773578e-1, -5.685376180022930e-1};

/** @brief Septic oMoms kernel (KernelRadius = 4) */
static float OMoms7Kernel(float x)
{
    x = fabs(x);

    if(x <= 1)
        return (((((((15015*x - 60060)*x + 21021)*x + 180180)*x + 2695)*x
            - 629244)*x + 21)*x + 989636) / 2162160;
    else if(x <= 2)
    {
        x = 2 - x;
        return (x*(x*(x*(x*(x*(x*(5005*x - 10010) - 13013) - 10010) + 54285)
            + 119350) + 106267) + 36606) / 1201200;
    }
    else if(x <= 3)
    {
        x = 3 - x;
        return (x*(x*(x*(x*(x*(x*(-15015*x + 15015) + 24024) + 90090)
            + 102410) + 76230) + 31164) + 5536) / 10810800;
    }
    else if(x < 4)
    {
        float xSqr;
        x = 4 - x;
        xSqr = x*x;
        return (x*(xSqr*(xSqr*(2145*xSqr + 3003) + 385) + 3)) / 10810800;
    }
    else
        return 0;
}


/** @brief Table of all the interpolation methods */
static interpmethod InterpMethodTable[] =
    {{"nearest", NearestNeighborKernel, 0.51f, 0, 0, 0, 1},
    {"bilinear", BilinearKernel, 1, 0, 0, 0, 1},
    {"bicubic", BicubicKernel, 2, 0, 0, 0, 1},
    {"lanczos2", Lanczos2Kernel, 2, 1, 0, 0, 1},
    {"lanczos3", Lanczos3Kernel, 3, 1, 0, 0, 1},
    {"lanczos4", Lanczos4Kernel, 4, 1, 0, 0, 1},
    {"schaum2", Schaum2Kernel, 1.51f, 0, 0, 0, 1},
    {"schaum3", Schaum3Kernel, 2, 0, 0, 0, 1},
    {"bspline2", BSpline2Kernel, 1.5f, 0,
        NUMEL(BSpline2Prefilter), BSpline2Prefilter, 8},
    {"bspline3", BSpline3Kernel, 2, 0,
        NUMEL(BSpline3Prefilter), BSpline3Prefilter, 6},
    {"bspline5", BSpline5Kernel, 3, 0,
        NUMEL(BSpline5Prefilter), BSpline5Prefilter, 120},
    {"bspline7", BSpline7Kernel, 4, 0,
        NUMEL(BSpline7Prefilter), BSpline7Prefilter, 5040},
    {"bspline9", BSpline9Kernel, 5, 0,
        NUMEL(BSpline9Prefilter), BSpline9Prefilter, 362880},
    {"bspline11", BSpline11Kernel, 6, 0,
        NUMEL(BSpline11Prefilter), BSpline11Prefilter, 39916800},
    {"omoms3", OMoms3Kernel, 2, 0,
        NUMEL(OMoms3Prefilter), OMoms3Prefilter, 21/4.0f},
    {"omoms5", OMoms5Kernel, 3, 0,
        NUMEL(OMoms5Prefilter), OMoms5Prefilter, 7920/107.0f},
    {"omoms7", OMoms7Kernel, 4, 0,
        NUMEL(OMoms7Prefilter), OMoms7Prefilter, 675675/346.0f}};


/**
 * @brief Get the interpmethod struct for an interpolation method by name
 * @param Name name of the interpolation method
 * @return pointer to interpmethod struct on success, null pointer on failure.
 *
 * Choices are
 *   - "nearest"
 *   - "bilinear"
 *   - "bicubic"
 *   - "lanczosN" with N = 2, 3, or 4
 *   - "schaumN" with N = 2 or 3
 *   - "bsplineN" with N = 2, 3, 5, 7, 9, or 11
 *   - "omomsN" with N = 3, 5 or 7
 */
interpmethod *GetInterpMethod(const char *Name)
{
    int i;

    for(i = 0; i < (int)NUMEL(InterpMethodTable); i++)
        if(!strcmp(InterpMethodTable[i].Name, Name))
            return &InterpMethodTable[i];

    return 0;
}
