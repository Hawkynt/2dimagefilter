/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   bufferfile.h: "Buffer file" I/O routines by randilyn. */

#ifndef BUFFERFILE_H_INCLUDED
#define BUFFERFILE_H_INCLUDED
#include <allegro.h>
#include "../include/common.h"
#ifdef __cplusplus
extern "C" {
#endif

PACKFILE *BufferFile_open (void);
void BufferFile_get_buffer (PACKFILE *pf, UINT8 **buffer, long *size);

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* BUFFERFILE_H_INCLUDED */
