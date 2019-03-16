/* FakeNES - A free, portable, Open Source NES emulator.

   core.h: Declarations for the RP2A03G CPU emulation.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   This file contains declarations necessary for the emulation core
   functions for the Ricoh RP2A03G CPU, as well as preprocessor definitions
   used to control the compilation and operation of the emulation core. */

#ifndef CORE_H_INCLUDED
#define CORE_H_INCLUDED
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

                               /* Compilation options:       */
#define CYCLE_LENGTH 3         /* Number of cycles that one  */
                               /* CPU cycle uses.            */
/* #define ALT_DEBUG */        /* Compile debugging version  */
/* #define LSB_FIRST */        /* Compile for low-endian CPU */

#define FN2A03_INT_NONE     0  /* No interrupt required      */
#define FN2A03_INT_IRQ_NONE 0  /* No interrupt required      */
#define FN2A03_INT_NMI      1  /* Non-maskable interrupt     */

#define FN2A03_INT_IRQ_BASE         2
/* Maskable IRQ, cleared after an IRQ is acknowledged */
#define FN2A03_INT_IRQ_SINGLE_SHOT  (FN2A03_INT_IRQ_BASE)
/* Maskable IRQs cleared via FN2A03_Clear_Interrupt() */
#define FN2A03_INT_IRQ_SOURCE(x)    (FN2A03_INT_IRQ_BASE + 1 + (x))
#define FN2A03_INT_IRQ_SOURCE_MAX   (31 - 1)

#define FN2A03_INT_IRQ_SOURCES      (FN2A03_INT_IRQ_SOURCE_MAX + 1)

                               /* 2A03 status flags:         */
#define C_FLAG    (1 << 0)     /* 1: Carry occured           */
#define Z_FLAG    (1 << 1)     /* 1: Result is zero          */
#define I_FLAG    (1 << 2)     /* 1: Interrupts disabled     */
#define D_FLAG    (1 << 3)     /* 1: Decimal mode            */
#define B_FLAG    (1 << 4)     /* Break [0 on stk after int] */
#define R_FLAG    (1 << 5)     /* Always 1                   */
#define V_FLAG    (1 << 6)     /* 1: Overflow occured        */
#define N_FLAG    (1 << 7)     /* 1: Result is negative      */


/* These macros pack flags into the CPU's own format (for push */
/* to the stack or display in a debugger) or unpack flags from */
/* the CPU's format (for pop from the stack). */

#define FN2A03_Pack_Flags(R) \
    (((R)->N & N_FLAG) | ((R)->V ? V_FLAG : 0) | ((R)->D ? D_FLAG : 0) | \
    ((R)->I ? I_FLAG : 0) | ((R)->Z ? 0 : Z_FLAG) | ((R)->C ? C_FLAG : 0) | \
    R_FLAG | B_FLAG)
#define FN2A03_Unpack_Flags(R,P) \
    (R)->N = (P) & N_FLAG; (R)->V = (P) & V_FLAG; (R)->D = (P) & D_FLAG; \
    (R)->I = (P) & I_FLAG; (R)->Z = (P) & Z_FLAG ? 0 : 1; \
    (R)->C = (P) & C_FLAG;


/*
 The following data types must be defined.
  UINT8     unsigned    sizeof(UINT8) == 1
  INT8      signed      sizeof(INT8) == 1
  UINT16    unsigned    sizeof(UINT16) == 2
  UINT32    unsigned    sizeof(UINT32) == 4
  PAIR      union       sizeof(PAIR) == 2
   { UINT16 word; struct { UINT8 low, high } bytes; }
*/

typedef UINT32 cpu_time_t;    /* Absolute. */
typedef INT32  cpu_rtime_t;   /* Relative. */

typedef struct
{
  /* CPU registers and program counter   */
  PAIR PC;
  UINT8 A,X,Y,S;
  UINT8 N,V,D,I,Z,C;   /* CPU status flags - Z flag set when Z == 0 */

  cpu_time_t ICount;  /*  FN2A03_Run will deduct cycles from */
                      /* here, executing while it is above   */
                      /* zero.                               */
  cpu_time_t Cycles;  /* Elapsed cycles since last cleared   */
  cpu_time_t IBackup; /* Private, don't touch                */
  UINT32 IRequest;    /* Logic state of the IRQ line, each    */
                      /*  bit reserved for a different source */
  cpu_time_t IRQTable[FN2A03_INT_IRQ_SOURCES];
  UINT32 IRequestQ;   /* Queued interrupt request sources    */
  UINT16 Trap;        /* Set Trap to address to trace from   */
  UINT8 AfterCLI;     /* Private, don't touch                */
  UINT8 TrapBadOps;   /* Set to 1 to warn of illegal opcodes */
  UINT8 Trace;        /* Set Trace=1 to start tracing        */
  UINT8 Jammed;       /* Private, don't touch                */
} FN2A03;

/*
 FN2A03_Init()

 This function performs any necessary initial core initialization.
*/
void FN2A03_Init(void);

/*
 FN2A03_Reset()

  This function is used to reset the CPU context to a state
 resembling hardware reset or power-on.  This function or
 FN2A03_Init() should be called at least once before any calls
 to FN2A03_Run() are made.
*/
void FN2A03_Reset(FN2A03 *R);

/*
 FN2A03_Exec()

  This function will execute a single RP2A03G opcode. It will
 then return next PC, and updated context in R.
*/
UINT16 FN2A03_Exec(FN2A03 *R);

/*
 FN2A03_Clear_Interrupt()

  This function clears a maskable interrupt source previously raised by
 FN2A03_Interrupt().
*/
void FN2A03_Clear_Interrupt(FN2A03 *R,UINT8 Type);

/* FN2A03_Queue_Interrupt()
   Schedules an automatic IRQ-only interrupt to take place at 'Time'. */
void FN2A03_Queue_Interrupt(FN2A03 *R,UINT8 Type,cpu_time_t Time);

/*
 FN2A03_Interrupt()

  This function requests an interrupt of the specified type.
 FN2A03_INT_NMI will raise a non-maskable interrupt.
 FN2A03_INT_IRQ_SINGLE_SHOT will raise a maskable interrupt to be cleared
 after a single acknowledgement, and FN2A03_INT_IRQ_SOURCE(x) will raise
 a maskable interrupt to be cleared later by FN2A03_Clear_Interrupt().
  No interrupts are acknowledged while the CPU is jammed.  Maskable
 interrupts will not be acknowledged until the I flag is clear.
*/
void FN2A03_Interrupt(FN2A03 *R,UINT8 Type);

/*
 FN2A03_Run()

  This function will execute RP2A03G code until the cycle
 counter expires.
*/
void FN2A03_Run(FN2A03 *R);

/*
 FN2A03_consume_cycles()

  This function will steal the requested count of clock cycles
 from the CPU.
*/
static INLINE void FN2A03_consume_cycles (FN2A03 *R, int cycles)
{
    R->Cycles += cycles * CYCLE_LENGTH;
}

/*
 FN2A03_Debug()

  This function should exist if ALT_DEBUG is #defined. When
 Trace!=0, it is called after each opcode executed by the
 CPU, and passed the RP2A03G context. Emulation exits if
 FN2A03_Debug() returns 0.
*/
UINT8 FN2A03_Debug(FN2A03 *R);


#ifdef __cplusplus
}
#endif
#endif   /* !CORE_H_INCLUDED */
