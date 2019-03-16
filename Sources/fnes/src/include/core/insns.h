

/*

FakeNES - A portable, Open Source NES emulator.

Distributed under the Clarified Artistic License.

insns.h: CPU instructions emulation macros.

Copyright (c) 2001-2006, Randy McDowell.
Copyright (c) 2001-2006, Charles Bilyue'.

This is free software.  See 'LICENSE' for details.
You must read and accept the license prior to use.

This file contains macros for emulating instruction
specific behavior, for the Ricoh RP2A03G CPU.

*/


/* These macros cover instruction-specific behavior */
#define Insn_Branch(condition) \
    data = Fetch(PC.word + 1); \
    PC.word += 2; \
    if (condition) \
    { \
        R->Cycles += CYCLE_LENGTH; \
        PC.bytes.low += data; \
        if ((data & 0x80) ? (PC.bytes.low >= data) : (PC.bytes.low < data)) \
        { \
            Fetch(PC.word); \
            R->Cycles += CYCLE_LENGTH; \
            if (data & 0x80) PC.word -= 0x100; \
            else PC.word += 0x100; \
        } \
    }

#define Insn_ADC(Rg) \
    result.word = R->A + Rg + (R->C ? 1 : 0); \
    R->V = (~(R->A ^ Rg) & (R->A ^ result.bytes.low) & 0x80); \
    R->C = result.bytes.high; \
    R->A = result.bytes.low; \
    Update_NZ(result.bytes.low);

/* Warning! C_FLAG is inverted before SBC and after it */
#define Insn_SBC(Rg) \
    result.word = R->A - Rg - (R->C ? 0 : 1); \
    R->V = ((R->A ^ Rg) & (R->A ^ result.bytes.low) & 0x80); \
    R->C = result.bytes.high + 1; \
    R->A = result.bytes.low; \
    Update_NZ(result.bytes.low);

#define Insn_CMP(Rg1,Rg2) \
  result.word = Rg1 - Rg2; \
  R->C = result.bytes.high + 1; \
  Update_NZ(result.bytes.low);

#define Insn_BIT(Rg) \
  R->N = Rg; \
  R->V = Rg & V_FLAG; \
  R->Z = Rg & R->A;

#define Insn_AND(Rg)    R->A &= Rg; Update_NZ(R->A)
#define Insn_ORA(Rg)    R->A |= Rg; Update_NZ(R->A)
#define Insn_EOR(Rg)    R->A ^= Rg; Update_NZ(R->A)
#define Insn_INC(Rg)    Rg++; Update_NZ(Rg)
#define Insn_DEC(Rg)    Rg--; Update_NZ(Rg)

#define Insn_SLO(Rg)    Insn_ASL(Rg); Insn_ORA(Rg)
#define Insn_ASL(Rg)    R->C = Rg & 0x80; Rg <<= 1; Update_NZ(Rg)
#define Insn_LSR(Rg)    R->C = Rg & 1; Rg >>= 1; Update_NZ(Rg)
#define Insn_ROL(Rg)    result.bytes.low = (Rg << 1) | (R->C ? 1 : 0); \
                        R->C = Rg & 0x80; Rg = result.bytes.low; \
                        Update_NZ(Rg)
#define Insn_ROR(Rg)    result.bytes.low = (Rg >> 1) | (R->C ? 0x80 : 0); \
                        R->C = Rg & 1; Rg = result.bytes.low; \
                        Update_NZ(Rg)

