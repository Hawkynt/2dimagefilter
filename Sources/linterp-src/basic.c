/**
 * @file basic.c
 * @brief Memory management, portable types, math constants, and timing
 * @author Pascal Getreuer <getreuer@gmail.com>
 *
 * This file implements a function Clock, a timer with millisecond
 * precision.  In order to obtain timing at high resolution, platform-
 * specific functions are needed:
 *
 *    - On Windows systems, the GetSystemTime function is used.
 *    - On POSIX systems, the gettimeofday function is used.
 *
 * Otherwise as a fallback, time.h time is used, and in this case Clock has
 * only second accuracy.  This file attempts to detect whether the platform
 * is POSIX or Windows and defines Clock accordingly.  A particular
 * implementation can be forced by defining USE_GETSYSTEMTIME,
 * USE_GETTIMEOFDAY, or USE_TIME.
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
#include <stdarg.h>
#include "basic.h"


/* Autodetect whether to use Windows, POSIX,
   or fallback implementation for Clock.  */
#if !defined(USE_GETSYSTEMTIME) && !defined(USE_GETTIMEOFDAY) && !defined(USE_TIME)
#   if defined(WIN32) || defined(_WIN32) || defined(WIN64) || defined(_WIN64)
#       define USE_GETSYSTEMTIME
#   elif defined(unix) || defined(__unix__) || defined(__unix)
#       include <unistd.h>
#       if (_POSIX_TIMERS) || (_POSIX_VERSION >= 200112L)
#           define USE_GETTIMEOFDAY
#       endif
#   endif
#endif

/* Define Clock(), get the system clock in milliseconds */
#if defined(USE_GETSYSTEMTIME)
#define WIN32_LEAN_AND_MEAN
#include <windows.h>

unsigned long Clock()   /* Windows implementation */
{
    static SYSTEMTIME TimeVal;
    GetSystemTime(&TimeVal);
    return (unsigned long)((unsigned long)TimeVal.wMilliseconds
        + 1000*((unsigned long)TimeVal.wSecond
        + 60*((unsigned long)TimeVal.wMinute
        + 60*((unsigned long)TimeVal.wHour
        + 24*(unsigned long)TimeVal.wDay))));
}
#elif defined(USE_GETTIMEOFDAY)
#include <unistd.h>
#include <sys/time.h>

unsigned long Clock()   /* POSIX implementation */
{
    struct timeval TimeVal;
    gettimeofday(&TimeVal, NULL);
    return (unsigned long)(TimeVal.tv_usec/1000 + TimeVal.tv_sec*1000);
}
#else
#include <time.h>

unsigned long Clock()   /* Fallback implementation */
{
    time_t RawTime;
    struct tm *TimeVal;
    time(&RawTime);
    TimeVal = localtime(&RawTime);
    return (unsigned long)(1000*((unsigned long)TimeVal->tm_sec
        + 60*((unsigned long)TimeVal->tm_min
        + 60*((unsigned long)TimeVal->tm_hour
        + 24*(unsigned long)TimeVal->tm_mday))));
}
#endif


/** @brief malloc with an error message on failure. */
void *MallocWithErrorMessage(size_t Size)
{
    void *Ptr;

    if(!(Ptr = malloc(Size)))
        ErrorMessage("Memory allocation of %u bytes failed.\n", Size);

    return Ptr;
}


/** @brief realloc with an error message and free on failure. */
void *ReallocWithErrorMessage(void *Ptr, size_t Size)
{
    void *NewPtr;

    if(!(NewPtr = realloc(Ptr, Size)))
    {
        ErrorMessage("Memory reallocation of %u bytes failed.\n", Size);
        Free(Ptr);  /* Free the previous block on failure */
    }

    return NewPtr;
}


/** @brief Redefine this function to customize error messages. */
void ErrorMessage(const char *Format, ...)
{
    va_list Args;

    va_start(Args, Format);
    /* Write a formatted error message to stderr */
    vfprintf(stderr, Format, Args);
    va_end(Args);
}
