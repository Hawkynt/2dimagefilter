/**
 * @file strutil.h
 * @brief String utility functions
 * @author Pascal Getreuer <getreuer@gmail.com>
 *
 *
 * Copyright (c) 2011, Pascal Getreuer
 * All rights reserved.
 *
 * This program is free software: you can use, modify and/or
 * redistribute it under the terms of the simplified BSD License. You
 * should have received a copy of this license along this program. If
 * not, see <http://www.opensource.org/licenses/bsd-license.html>.
 */

#ifndef _STRUTIL_H_
#define _STRUTIL_H_

int EatWhitespace(const char **StrPtr);
int ParseNumber(double *Number, const char **StrPtr, int FloatAllowed);

#endif /* _STRUTIL_H_ */
