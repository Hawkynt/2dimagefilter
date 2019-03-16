/**
 * @file adaptlob.c
 * @brief Adaptive Lobatto quadrature
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

#include <stdlib.h>
#include <stdio.h>
#include <math.h>

#define MACHINE_PRECISION   2.2204e-16

#define ALPHA   0.816496580927726
#define BETA    0.447213595499958


static int Termination2;

static double AdaptLobStep(float (*f)(float, const void*), double a, double b,
    double fa, double fb, double Tol, double hmin, const void *Param);


/**
* @brief Adaptive Lobatto quadrature
*
* @param f integrand
* @param a, b integration interval
* @param Tol absolute error tolerance
* @param Param parameter to pass to f
*
* Based on W. Gander and W. Gautschi, "Adaptive Quadrature Revisited", 1998,
* the algorithm used by MATLAB's quadl function.
*/
float AdaptLob(float (*f)(float, const void*), float a, float b,
    float Tol, const void *Param)
{
    Termination2 = 0;
    return (float)AdaptLobStep(f, a, b, f(a, Param), f(b, Param),
        Tol, MACHINE_PRECISION*(b - a)/1024.0, Param);
}


static double AdaptLobStep(float (*f)(float, const void*), double a, double b,
    double fa, double fb, double Tol, double hmin, const void *Param)
{
    double m, h, Q, fmll, fml, fm, fmr, fmrr;


    m = 0.5*(a + b);
    h = 0.5*(b - a);

    if(h < hmin || m == a || m == b)
    {
        if(Termination2 == 0)
        {
            fprintf(stderr, "Minimum step size reached.\n");
            Termination2 = 1;
        }

        return h*(fa + fb);
    }

    fmll = f(m - ALPHA*h, Param);
    fml = f(m - BETA*h, Param);
    fm = f(m, Param);
    fmr = f(m + BETA*h, Param);
    fmrr = f(m + ALPHA*h, Param);
    Q = (h/1470.0)*(77.0*(fa + fb) + 432.0*(fmll + fmrr)
        + 625.0*(fml + fmr) + 672.0*fm);

    if(fabs(Q - (h/6.0)*(fa + fb + 5.0*(fml + fmr))) <= Tol)
        return Q;
    else	/* Accumulate in double precision */
        return (double)AdaptLobStep(f, a, m - ALPHA*h, fa, fmll, Tol, hmin, Param)
            + (double)AdaptLobStep(f, m - ALPHA*h, m - BETA*h, fmll, fml, Tol, hmin, Param)
            + (double)AdaptLobStep(f, m - BETA*h, m, fml, fm, Tol, hmin, Param)
            + (double)AdaptLobStep(f, m, m + BETA*h, fm, fmr, Tol, hmin, Param)
            + (double)AdaptLobStep(f, m + BETA*h, m + ALPHA*h, fmr, fmrr, Tol, hmin, Param)
            + (double)AdaptLobStep(f, m + ALPHA*h, b, fmrr, fb, Tol, hmin, Param);
}
