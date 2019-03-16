

/*

FakeNES - A portable, Open Source NES emulator.

Distributed under the Clarified Artistic License.

memory.h: CPU memory access definitions.

Copyright (c) 2001-2006, Randy McDowell.
Copyright (c) 2001-2006, Charles Bilyue'.

This is free software.  See 'LICENSE' for details.
You must read and accept the license prior to use.

This file contains declarations necessary for the
emulation core functions for the Ricoh RP2A03G CPU, as
well as preprocessor definitions used to control the
compilation and operation of the emulation core.

*/


#ifndef CORE_MEMORY_H
#define CORE_MEMORY_H

#ifdef __cplusplus
extern "C" {
#endif


#include "core.h"


                               /* Compilation options:       */
/*  Uses FN2A03_Read, FN2A03_Write, FN2A03_Fetch #included */
/* from cpu.h */
#define INLINE_MEMORY_HANDLERS

#define FAST_STACK             /* Separate stack handlers    */
#define FAST_ZP                /* Separate zeropage handlers */

/*
 FAST_STACK

  If this #define is present, FN2A03_Read_Stack() and
 FN2A03_Write_Stack() must be present.  If this #define is absent,
 FN2A03_Read() and FN2A03_Write() will handle stack accesses.
*/
#ifndef FAST_STACK
#define FN2A03_Read_Stack(A)    (FN2A03_Read((UINT16) 0x100+(UINT8 (A))))
#define FN2A03_Write_Stack(A,D) (FN2A03_Write((UINT16) 0x100+(UINT8 (A)), (D)))
#endif

/*
 FAST_ZP

  If this #define is present, FN2A03_Read_ZP() and
 FN2A03_Write_ZP() must be present.  If this #define is absent,
 FN2A03_Read() and FN2A03_Write() will handle zero page accesses.
*/
#ifndef FAST_ZP
#define FN2A03_Read_ZP(A)    (FN2A03_Read((UINT8 (A))))
#define FN2A03_Write_ZP(A,D) (FN2A03_Write((UINT8 (A)), (D)))
#endif


/*
 FN2A03_Read/Write/Fetch()

  These functions are called when a memory access occurs.
 Fetch is used for reading opcode bytes from executing code.

  These are not part of the CPU core and must be supplied by
 the user.
*/
#ifndef INLINE_MEMORY_HANDLERS
void FN2A03_Write(UINT16 Addr,UINT8 Value);
UINT8 FN2A03_Read(UINT16 Addr);
UINT8 FN2A03_Fetch(UINT16 Addr);

#ifdef FAST_STACK
UINT8 FN2A03_Read_Stack(UINT8 S);
void FN2A03_Write_Stack(UINT8 S,UINT8 Value);
#endif

#ifdef FAST_ZP
UINT8 FN2A03_Read_ZP(UINT8 S);
void FN2A03_Write_ZP(UINT8 S,UINT8 Value);
#endif
#else
#include "cpu.h"
#endif


#ifdef __cplusplus
}
#endif

#endif /* CORE_MEMORY_H */
