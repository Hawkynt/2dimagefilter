/* FakeNES - A free, portable, Open Source NES emulator.

   audiolib.h: Declarations for the audio library.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef AUDIOLIB_H_INCLUDED
#define AUDIOLIB_H_INCLUDED
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

int audiolib_init (void);
void audiolib_exit (void);
int audiolib_play (void);
void audiolib_stop (void);
void *audiolib_get_buffer (void);
void audiolib_free_buffer (void);
void audiolib_suspend (void);
void audiolib_resume (void);

#ifdef __cplusplus
}
#endif
#endif   /* !AUDIOLIB_H_INCLUDED */
