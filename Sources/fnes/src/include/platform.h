/* FakeNES - A free, portable, Open Source NES emulator.

   platform.h: Declarations for the platform interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef PLATFORM_H_INCLUDED
#define PLATFORM_H_INCLUDED
#include "common.h"
#ifdef __cplusplus
extern "C" {
#endif

int platform_init (void);
void platform_exit (void);

#ifdef __cplusplus
}
#endif
#endif   /* !PLATFORM_H_INCLUDED */
