/* FakeNES - A free, portable, Open Source NES emulator.

   rewind.h: Definitions for the game rewinder.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef REWIND_H_INCLUDED
#define REWIND_H_INCLUDED
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

int rewind_init (void);
void rewind_exit (void);
void rewind_clear (void);
BOOL rewind_save_snapshot (void);
BOOL rewind_load_snapshot (void);
BOOL rewind_is_enabled (void);

#ifdef __cplusplus
}
#endif
#endif   /* !REWIND_H_INCLUDED */
