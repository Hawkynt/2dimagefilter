/* FakeNES - A free, portable, Open Source NES emulator.

   cheats.h: Declarations for cheat decoders.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef CHEATS_H_INCLUDED
#define CHEATS_H_INCLUDED
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

extern int cheats_decode (const UINT8 *code, UINT16 *address, UINT8 *value, UINT8 *match_value);

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !CHEATS_H_INCLUDED */
