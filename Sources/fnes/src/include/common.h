/* FakeNES - A free, portable, Open Source NES emulator.

   common.h: Global common definitions.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef COMMON_H_INCLUDED
#define COMMON_H_INCLUDED
#include <allegro.h>
#include "debug.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

#undef TRUE
#define TRUE   1
#undef FALSE
#define FALSE  0

#define TRUE_OR_FALSE(x)   ((x) ? TRUE : FALSE)

#undef NULL
#define NULL   0

#ifdef __cplusplus
/* Cleaner lowercase versions for use in 'pure' C++ code. */
#define true   TRUE
#define false  FALSE
#define null   NULL

#define inline    INLINE
#define linear    INLINE   // for functions that are called only once
#endif

#define ROUND(x)  ((x) + 0.5)

#ifndef M_PI
#define M_PI      3.14159265358979323846
#endif

/* <+KittyCat> $ grep EPSILON include/3dobject.h
   <+KittyCat> #define EPSILON (1.0f/1024.0f)
   */
#define EPSILON   (1.0 / 1024.0)

/* Macro to compare 2 REALs. */
#define COMPARE_TWO_REALS(a, b)  \
   TRUE_OR_FALSE(((a) >= ((b) - EPSILON)) && ((a) <= ((b) + EPSILON)))

/* TODO: Remove all references to NIL and correct compiler warnings. */
#define NIL    0

extern int saved_argc;
extern char **saved_argv;   /* Saved from main(), needed for ALUT. */

static INLINE int fix (int value, int base, int limit)
{
   if (value < base)
      value = base;
   if (value > limit)
      value = limit;

   return (value);
}

static INLINE REAL fixf (REAL value, REAL base, REAL limit)
{
   if (value < base)
      value = base;
   if (value > limit)
      value = limit;

   return (value);
}

#define MAX3(a, b, c)   (MAX((a), MAX((b), (c))))
      
#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !COMMON_H_INCLUDED */
