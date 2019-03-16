/**
 * @file adaptlob.h
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

#ifndef _ADAPTLOB_H_
#define _ADAPTLOB_H_

float AdaptLob(float (*f)(float, const void*), float a, float b,
    float Tol, const void *Param);

#endif /* _ADAPTLOB_H_ */
