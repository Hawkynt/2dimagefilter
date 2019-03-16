/* FakeNES - A free, portable, Open Source NES emulator.

   core.c: Implementation of the RP2A03G CPU emulation.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   This file contains emulation core functions for the Ricoh RP2A03G CPU, as
   used in the Nintendo Famicom (Family Computer) and NES (Nintendo
   Entertainment System). */

#include <allegro.h>
#include <stdio.h>
#include "core.h"
#include "core/tables.h"
#include "core/memory.h"
#include "log.h"

/* define some macros to help improve readability */

/* fetch an opcode byte */
#define Fetch(A)            (FN2A03_Fetch((A)))
/* read/write a data byte */
#define Read(A)             (FN2A03_Read((A)))
#define Write(A,D)          (FN2A03_Write((A),(D)))

/* read/write a stack byte */
#define Read_Stack(A)       (FN2A03_Read_Stack((A)))
#define Write_Stack(A,D)    (FN2A03_Write_Stack((A),(D)))

/* read/write a zero page byte */
#define Read_ZP(A)          (FN2A03_Read_ZP((A)))
#define Write_ZP(A,D)       (FN2A03_Write_ZP((A),(D)))

/* fetch two opcode bytes (absolute address) */
#define Fetch16(Rg) \
    Rg.bytes.low = Fetch(PC.word + 1); Rg.bytes.high = Fetch(PC.word + 2)

/* These macros are used for pushing and pulling data on the stack. */
#define Push(Rg)        Write_Stack(R->S,Rg); R->S--
#define Pull(Rg)        R->S++; Rg=Read_Stack(R->S)
#define Push16(Rg)      Push(Rg.bytes.high); Push(Rg.bytes.low)
#define Pull16(Rg)      Pull(Rg.bytes.low); Pull(Rg.bytes.high)


/* These macros pack flags into the CPU's own format (for push */
/* to the stack or display in a debugger) or unpack flags from */
/* the CPU's format (for pop from the stack). */

#define Pack_Flags()    FN2A03_Pack_Flags(R)
#define Unpack_Flags(P) FN2A03_Unpack_Flags((R),(P))

/* This macro is used to set the Negative and Zero flags based */
/* on a result. */
#define Update_NZ(Value) (R->N=R->Z=Value)


/* Addressing mode macros */
#include "core/addr.h"
/* Instruction macros */
#include "core/insns.h"


//#define BINARY_DUMP
#ifdef BINARY_DUMP
FILE *ops_dmp;
#endif

/*
 FN2A03_Init()

 This function performs any necessary initial core initialization.
*/
void FN2A03_Init(void)
{
  int i;

  /* Initialize the instruction cycle count table. */
  for (i = 0; i < 256; i++)
  {
      Cycles[i] = BaseCycles[i] * CYCLE_LENGTH;
  }

#ifdef BINARY_DUMP
  ops_dmp = fopen("ops.dmp", "wb");
#endif
}

/*
 FN2A03_Reset()

  This function is used to reset the CPU context to a state
 resembling hardware reset or power-on.  This function or
 FN2A03_Init() should be called at least once before any calls
 to FN2A03_Run() are made.
*/
void FN2A03_Reset(FN2A03 *R)
{
  FN2A03_Init();

  R->A=R->X=R->Y=0x00;

  R->N=R->V=R->D=R->C=0;
  R->I=1;
  R->Z=0;       /* 0 == set */

  R->S=0xFF;
  R->PC.bytes.low=Read(0xFFFC);
  R->PC.bytes.high=Read(0xFFFD);   
  R->ICount=R->Cycles=0;
  R->IRequest=R->IRequestQ=FN2A03_INT_IRQ_NONE;
  R->AfterCLI=0;
  R->Jammed=0;
}

#ifdef DEBUG
static int opcode_count=0;
static UINT8 opcode_trace[10];
#endif

void FN2A03_report_bad_opcode(UINT8 opcode, UINT16 address)
{
    log_printf ("[FN2A03] Unrecognized instruction: $%02X at PC=$%04X",
        opcode, address);
}


#ifdef DEBUG
void FN2A03_opcode_fallback_trace(void)
{
    log_printf("Opcode fallback trace:");
    for (opcode_count = 0; opcode_count < 10; opcode_count++)
    {
        log_printf("$%02X ",opcode_trace[opcode_count]);
        opcode_trace[opcode_count] = 0;
    }
    opcode_count = 0;
}
#endif


#define OPCODE_PROLOG(x) \
    case x: {
#define OPCODE_EXIT      break;
#define OPCODE_EPILOG    OPCODE_EXIT }

#define OPCODE_PROLOG_DEFAULT \
    default: {

#if 0
/*
 FN2A03_Exec()

  This function will execute a single RP2A03G opcode. It will
 then return next PC, and updated context in R.
*/
UINT16 FN2A03_Exec(FN2A03 *R)
{
  UINT8 opcode, cycles;
  PAIR PC;

  PC.word=R->PC.word;
  opcode=Fetch(PC.word);
  cycles=Cycles[opcode];
  R->Cycles+=cycles;
#ifdef DEBUG
  opcode_trace[opcode_count++]=opcode;
  if (opcode_count==10) opcode_count=0;
#endif
  switch(opcode)
  {
    PAIR address, result;
    UINT8 zero_page_address, data;
#include "core/codes.h"
  }

  R->PC.word=PC.word;

  /* We are done */
  return(PC.word);
}
#endif


/*
 FN2A03_Clear_Interrupt()

  This function clears a maskable interrupt source previously raised by
 FN2A03_Interrupt().
*/
void FN2A03_Clear_Interrupt(FN2A03 *R,UINT8 Type)
{
    if (Type >= FN2A03_INT_IRQ_SOURCE(0) &&
        Type <= FN2A03_INT_IRQ_SOURCE(FN2A03_INT_IRQ_SOURCE_MAX))
    {
        const UINT32 Mask = (1 << (Type - FN2A03_INT_IRQ_BASE));
        R->IRequest &= ~Mask;
        R->IRequestQ &= ~Mask;
    }
}

/* FN2A03_Queue_Interrupt()
   Schedules an automatic IRQ-only interrupt to take place at 'Time'. */
void FN2A03_Queue_Interrupt(FN2A03 *R,UINT8 Type,cpu_time_t Time)
{            
    if (Type >= FN2A03_INT_IRQ_SOURCE(0) &&
        Type <= FN2A03_INT_IRQ_SOURCE(FN2A03_INT_IRQ_SOURCE_MAX))
    {
        const int Index = (Type - FN2A03_INT_IRQ_BASE);
        R->IRQTable[Index] = Time;
        R->IRequestQ |= (1 << Index);
    }
}

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
void FN2A03_Interrupt(FN2A03 *R,UINT8 Type)
{
    UINT16 vector;

    if (R->Jammed) return;

    if (Type == FN2A03_INT_IRQ_SINGLE_SHOT ||
        (Type >= FN2A03_INT_IRQ_SOURCE(0) &&
        Type <= FN2A03_INT_IRQ_SOURCE(FN2A03_INT_IRQ_SOURCE_MAX)))
    {
        R->IRequest |= (1 << (Type - FN2A03_INT_IRQ_BASE));
    }

    if ((Type == FN2A03_INT_NMI) || (R->IRequest && !(R->I)))
    {
        UINT8 P;

        R->Cycles += 7 * CYCLE_LENGTH;
        Push16(R->PC);
        P = Pack_Flags() & ~B_FLAG;
        Push(P);
        /* R->D = 0; */
        R->I=1;
        if (Type == FN2A03_INT_NMI) vector = 0xFFFA;
        else
        {
            vector = 0xFFFE;
            R->IRequest &= ~(1 << (FN2A03_INT_IRQ_SINGLE_SHOT -
                FN2A03_INT_IRQ_BASE));
        }
        R->PC.bytes.low=Read(vector);
        R->PC.bytes.high=Read(vector + 1);
    }
}


/*
 FN2A03_Run()

  This function will execute RP2A03G code until the cycle
 counter expires.
*/
void FN2A03_Run(FN2A03 *R)
{
  if (R->Jammed) return;

  for(;;)
  {
    PAIR PC;
    PC.word=R->PC.word;

    while ((cpu_rtime_t)(R->ICount - R->Cycles) > 0)
    {
      UINT8 opcode, cycles;

#ifdef ALT_DEBUG
      /* Turn tracing on when reached trap address */
      if(PC.word==R->Trap) R->Trace=1;
      /* Call single-step debugger, exit if requested */
      if(R->Trace)
      {
        R->PC.word=PC.word;
        if(!FN2A03_Debug(R)) return;
      }
#endif

      opcode=Fetch(PC.word);
#ifdef BINARY_DUMP
  R->PC.word=PC.word;
  if (ops_dmp)
  {
   fputc(R->PC.bytes.high, ops_dmp);
   fputc(R->PC.bytes.low, ops_dmp);
   fputc(R->A, ops_dmp);
   fputc(R->X, ops_dmp);
   fputc(R->Y, ops_dmp);
   fputc(R->S, ops_dmp);
   fputc(Pack_Flags(), ops_dmp);
   fputc(opcode, ops_dmp);
   fwrite(&R->Cycles, 1, 4, ops_dmp);
   fwrite(&R->ICount, 1, 4, ops_dmp);
  }
#endif

      cycles=Cycles[opcode];
      R->Cycles+=cycles;
#ifdef DEBUG
      opcode_trace[opcode_count++]=opcode;
      if (opcode_count==10) opcode_count=0;
#endif
      switch(opcode)
      {
        PAIR address, result;
        UINT8 zero_page_address, data;
#include "core/codes.h"
      }

      /* Check for queued interrupt requests. */
      if (R->IRequestQ)
      {
         int Index;

         for (Index = 0; Index < FN2A03_INT_IRQ_SOURCES; Index++)
         {
            const UINT32 Mask = (1 << Index);

            if ((R->IRequestQ & Mask) &&
                (R->IRQTable[Index] <= R->Cycles))
            {
               //log_printf ("CPU: Interrupt unqueued at %u cycles (optimal was %u cycles).\n", R->Cycles, R->IRQTable[Index]);
               R->IRequestQ &= ~Mask;
               R->IRequest |= Mask;
            }
         }

         if (R->IRequest && (!(R->I)) && !R->AfterCLI)
         {             
            R->PC.word = PC.word;
            FN2A03_Interrupt (R, FN2A03_INT_NONE);
            PC.word = R->PC.word;
         }
      }
    }

    R->PC.word=PC.word;

    /* cycle counter expired, or we wouldn't be here */
    if (!R->AfterCLI) return;

    /* If we have come after CLI, get FN2A03_INT_? from IRequest */
    R->ICount = R->IBackup;   /* Restore the ICount        */
    R->AfterCLI=0;            /* Done with AfterCLI state  */

    /* Process pending interrupts */
    if (R->IRequest) FN2A03_Interrupt(R, FN2A03_INT_NONE);
  }

  /* Execution stopped */
  return;
}
