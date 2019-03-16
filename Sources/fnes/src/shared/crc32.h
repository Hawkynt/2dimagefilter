/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   crc32.h: CRC32 calculation routines by TRAC. */

#ifndef CRC32_H_INCLUDED
#define CRC32_H_INCLUDED
#include "../include/common.h"
#ifdef __cplusplus
extern "C" {
#endif

UINT32 make_crc32 (const UINT8 *buffer, unsigned size);

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !CRC32_H_INCLUDED */
