

/*

FakeNES - A portable, Open Source NES emulator.

Distributed under the Clarified Artistic License.

codes.h: CPU opcode emulation macros.

Copyright (c) 2001-2006, Randy McDowell.
Copyright (c) 2001-2006, Charles Bilyue'.

This is free software.  See 'LICENSE' for details.
You must read and accept the license prior to use.

This file contains opcode emulation functions for the
Ricoh RP2A03G CPU, as used in the Nintendo Famicom (Family
Computer) and NES (Nintendo Entertainment System).

*/


OPCODE_PROLOG(0x10) /* BPL * REL */
    Insn_Branch(!(R->N&N_FLAG)) OPCODE_EXIT
OPCODE_EPILOG

OPCODE_PROLOG(0x30) /* BMI * REL */
    Insn_Branch(R->N&N_FLAG) OPCODE_EXIT
OPCODE_EPILOG

OPCODE_PROLOG(0xD0) /* BNE * REL */
    Insn_Branch(R->Z) OPCODE_EXIT
OPCODE_EPILOG

OPCODE_PROLOG(0xF0) /* BEQ * REL */
    Insn_Branch(!R->Z) OPCODE_EXIT
OPCODE_EPILOG

OPCODE_PROLOG(0x90) /* BCC * REL */
    Insn_Branch(!R->C) OPCODE_EXIT
OPCODE_EPILOG

OPCODE_PROLOG(0xB0) /* BCS * REL */
    Insn_Branch(R->C) OPCODE_EXIT
OPCODE_EPILOG

OPCODE_PROLOG(0x50) /* BVC * REL */
    Insn_Branch(!R->V) OPCODE_EXIT
OPCODE_EPILOG

OPCODE_PROLOG(0x70) /* BVS * REL */
    Insn_Branch(R->V) OPCODE_EXIT
OPCODE_EPILOG


OPCODE_PROLOG(0x40) /* RTI */
    UINT8 P;
    Pull(P);
    if (R->IRequest && (!(P & I_FLAG) && R->I))
    {
      R->AfterCLI = 1;
      R->IBackup = R->ICount;
      R->ICount = 1;
    }
    Unpack_Flags(P); Pull16(PC);
OPCODE_EPILOG

OPCODE_PROLOG(0x60) /* RTS */
    Pull16(PC); PC.word++;
OPCODE_EPILOG


OPCODE_PROLOG(0x20) /* JSR $ssss ABS */
    Fetch16(address);
    PC.word += 2;
    Push16(PC);
    PC.word = address.word;
OPCODE_EPILOG

OPCODE_PROLOG(0x4C) /* JMP $ssss ABS */
    Fetch16(address);
    PC.word = address.word;
OPCODE_EPILOG

OPCODE_PROLOG(0x6C) /* JMP ($ssss) ABSINDIR */
    Fetch16(address);
    PC.bytes.low = Read(address.word);
    address.bytes.low++;
    PC.bytes.high = Read(address.word);
OPCODE_EPILOG


OPCODE_PROLOG(0x00) /* BRK */
  UINT8 P;
  PC.word += 2;
  Push16(PC);
  P = Pack_Flags() | B_FLAG;
  Push(P);
  R->I = 1;
  PC.bytes.low = Read(0xFFFE);
  PC.bytes.high = Read(0xFFFF);
OPCODE_EPILOG


OPCODE_PROLOG(0x58) /* CLI */
    if (R->IRequest && R->I)
    {
      R->AfterCLI = 1;
      R->IBackup = R->ICount;
      R->ICount = 1;
    }
    R->I = 0;
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x28) /* PLP */
    UINT8 P;
    Pull(P);
    if (R->IRequest && (!(P & I_FLAG) && R->I))
    {
      R->AfterCLI = 1;
      R->IBackup = R->ICount;
      R->ICount = 1;
    }
    Unpack_Flags(P);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x08) /* PHP */
    UINT8 P;
    P = Pack_Flags(); Push(P);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x18) /* CLC */
    R->C = 0;
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xB8) /* CLV */
    R->V = 0;
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xD8) /* CLD */
    R->D = 0;
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x38) /* SEC */
    R->C = 1;
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xF8) /* SED */
    R->D = 1;
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x78) /* SEI */
    R->I = 1;
    PC.word++;
OPCODE_EPILOG


OPCODE_PROLOG(0x48) /* PHA */
    Push(R->A);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x68) /* PLA */
    Pull(R->A); Update_NZ(R->A);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x98) /* TYA */
    R->A = R->Y; Update_NZ(R->A);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xA8) /* TAY */
    R->Y = R->A; Update_NZ(R->Y);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xC8) /* INY */
    Insn_INC(R->Y);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x88) /* DEY */
    Insn_DEC(R->Y);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x8A) /* TXA */
    R->A = R->X; Update_NZ(R->A);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xAA) /* TAX */
    R->X = R->A; Update_NZ(R->X);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xE8) /* INX */
    Insn_INC(R->X);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xCA) /* DEX */
    Insn_DEC(R->X);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xEA) /* NOP */
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x9A) /* TXS */
    R->S = R->X;
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0xBA) /* TSX */
    R->X = R->S; Update_NZ(R->X);
    PC.word++;
OPCODE_EPILOG


OPCODE_PROLOG(0x24) /* BIT $ss ZP */
    Read_Zero_Page(data); Insn_BIT(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x2C) /* BIT $ssss ABS */
    Read_Absolute(data); Insn_BIT(data);
OPCODE_EPILOG


OPCODE_PROLOG(0x04) /* NOP $ss ZP */
    Read_Zero_Page(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x05) /* ORA $ss ZP */
    Read_Zero_Page(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x06) /* ASL $ss ZP */
    RMW_Zero_Page(Insn_ASL);
OPCODE_EPILOG

OPCODE_PROLOG(0x07) /* SLO $ss ZP */
    RMW_Zero_Page(Insn_SLO);
OPCODE_EPILOG

OPCODE_PROLOG(0x25) /* AND $ss ZP */
    Read_Zero_Page(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x26) /* ROL $ss ZP */
    RMW_Zero_Page(Insn_ROL);
OPCODE_EPILOG

OPCODE_PROLOG(0x45) /* EOR $ss ZP */
    Read_Zero_Page(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x46) /* LSR $ss ZP */
    RMW_Zero_Page(Insn_LSR);
OPCODE_EPILOG

OPCODE_PROLOG(0x65) /* ADC $ss ZP */
    Read_Zero_Page(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x66) /* ROR $ss ZP */
    RMW_Zero_Page(Insn_ROR);
OPCODE_EPILOG

OPCODE_PROLOG(0x84) /* STY $ss ZP */
    Write_Zero_Page(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0x85) /* STA $ss ZP */
    Write_Zero_Page(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0x86) /* STX $ss ZP */
    Write_Zero_Page(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xA4) /* LDY $ss ZP */
    Read_Zero_Page(R->Y); Update_NZ(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0xA5) /* LDA $ss ZP */
    Read_Zero_Page(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xA6) /* LDX $ss ZP */
    Read_Zero_Page(R->X); Update_NZ(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xC4) /* CPY $ss ZP */
    Read_Zero_Page(data); Insn_CMP(R->Y, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xC5) /* CMP $ss ZP */
    Read_Zero_Page(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xC6) /* DEC $ss ZP */
    RMW_Zero_Page(Insn_DEC);
OPCODE_EPILOG

OPCODE_PROLOG(0xE4) /* CPX $ss ZP */
    Read_Zero_Page(data); Insn_CMP(R->X, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xE5) /* SBC $ss ZP */
    Read_Zero_Page(data); Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xE6) /* INC $ss ZP */
    RMW_Zero_Page(Insn_INC);
OPCODE_EPILOG


OPCODE_PROLOG(0x0D) /* ORA $ssss ABS */
    Read_Absolute(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x0E) /* ASL $ssss ABS */
    RMW_Absolute(Insn_ASL);
OPCODE_EPILOG

OPCODE_PROLOG(0x0F) /* SLO $ssss ABS */
    RMW_Absolute(Insn_SLO);
OPCODE_EPILOG

OPCODE_PROLOG(0x2D) /* AND $ssss ABS */
    Read_Absolute(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x2E) /* ROL $ssss ABS */
    RMW_Absolute(Insn_ROL);
OPCODE_EPILOG

OPCODE_PROLOG(0x4D) /* EOR $ssss ABS */
    Read_Absolute(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x4E) /* LSR $ssss ABS */
    RMW_Absolute(Insn_LSR);
OPCODE_EPILOG

OPCODE_PROLOG(0x6D) /* ADC $ssss ABS */
    Read_Absolute(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x6E) /* ROR $ssss ABS */
    RMW_Absolute(Insn_ROR);
OPCODE_EPILOG

OPCODE_PROLOG(0x8C) /* STY $ssss ABS */
    Write_Absolute(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0x8D) /* STA $ssss ABS */
    Write_Absolute(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0x8E) /* STX $ssss ABS */
    Write_Absolute(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xAC) /* LDY $ssss ABS */
    Read_Absolute(R->Y); Update_NZ(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0xAD) /* LDA $ssss ABS */
    Read_Absolute(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xAE) /* LDX $ssss ABS */
    Read_Absolute(R->X); Update_NZ(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xCC) /* CPY $ssss ABS */
    Read_Absolute(data); Insn_CMP(R->Y, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xCD) /* CMP $ssss ABS */
    Read_Absolute(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xCE) /* DEC $ssss ABS */
    RMW_Absolute(Insn_DEC);
OPCODE_EPILOG

OPCODE_PROLOG(0xEC) /* CPX $ssss ABS */
    Read_Absolute(data); Insn_CMP(R->X, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xED) /* SBC $ssss ABS */
    Read_Absolute(data); Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xEE) /* INC $ssss ABS */
    RMW_Absolute(Insn_INC);
OPCODE_EPILOG


OPCODE_PROLOG(0x09) /* ORA #$ss IMM */
    Read_Immediate(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x29) /* AND #$ss IMM */
    Read_Immediate(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x49) /* EOR #$ss IMM */
    Read_Immediate(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x69) /* ADC #$ss IMM */
    Read_Immediate(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x80) /* NOP #$ss IMM */
    Read_Immediate(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xA0) /* LDY #$ss IMM */
    Read_Immediate(R->Y); Update_NZ(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0xA2) /* LDX #$ss IMM */
    Read_Immediate(R->X); Update_NZ(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xA9) /* LDA #$ss IMM */
    Read_Immediate(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xC0) /* CPY #$ss IMM */
    Read_Immediate(data); Insn_CMP(R->Y, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xC9) /* CMP #$ss IMM */
    Read_Immediate(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xE0) /* CPX #$ss IMM */
    Read_Immediate(data); Insn_CMP(R->X, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xE9) /* SBC #$ss IMM */
    Read_Immediate(data); Insn_SBC(data);
OPCODE_EPILOG


OPCODE_PROLOG(0x15) /* ORA $ss,x ZP,x */
    Read_Zero_Page_Index_X(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x16) /* ASL $ss,x ZP,x */
    RMW_Zero_Page_Index_X(Insn_ASL);
OPCODE_EPILOG

OPCODE_PROLOG(0x35) /* AND $ss,x ZP,x */
    Read_Zero_Page_Index_X(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x36) /* ROL $ss,x ZP,x */
    RMW_Zero_Page_Index_X(Insn_ROL);
OPCODE_EPILOG

OPCODE_PROLOG(0x55) /* EOR $ss,x ZP,x */
    Read_Zero_Page_Index_X(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x56) /* LSR $ss,x ZP,x */
    RMW_Zero_Page_Index_X(Insn_LSR);
OPCODE_EPILOG

OPCODE_PROLOG(0x74) /* NOP $ss,x ZP,x */
    Read_Zero_Page_Index_X(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x75) /* ADC $ss,x ZP,x */
    Read_Zero_Page_Index_X(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x76) /* ROR $ss,x ZP,x */
    RMW_Zero_Page_Index_X(Insn_ROR);
OPCODE_EPILOG

OPCODE_PROLOG(0x94) /* STY $ss,x ZP,x */
    Write_Zero_Page_Index_X(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0x95) /* STA $ss,x ZP,x */
    Write_Zero_Page_Index_X(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0x96) /* STX $ss,y ZP,y */
    Write_Zero_Page_Index_Y(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xB4) /* LDY $ss,x ZP,x */
    Read_Zero_Page_Index_X(R->Y); Update_NZ(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0xB5) /* LDA $ss,x ZP,x */
    Read_Zero_Page_Index_X(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xB6) /* LDX $ss,y ZP,y */
    Read_Zero_Page_Index_Y(R->X); Update_NZ(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xD5) /* CMP $ss,x ZP,x */
    Read_Zero_Page_Index_X(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xD6) /* DEC $ss,x ZP,x */
    RMW_Zero_Page_Index_X(Insn_DEC);
OPCODE_EPILOG

OPCODE_PROLOG(0xF5) /* SBC $ss,x ZP,x */
    Read_Zero_Page_Index_X(data); Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xF6) /* INC $ss,x ZP,x */
    RMW_Zero_Page_Index_X(Insn_INC);
OPCODE_EPILOG


OPCODE_PROLOG(0x19) /* ORA $ssss,y ABS,y */
    Read_Absolute_Index_Y(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x1D) /* ORA $ssss,x ABS,x */
    Read_Absolute_Index_X(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x1E) /* ASL $ssss,x ABS,x */
    RMW_Absolute_Index_X(Insn_ASL);
OPCODE_EPILOG

OPCODE_PROLOG(0x39) /* AND $ssss,y ABS,y */
    Read_Absolute_Index_Y(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x3D) /* AND $ssss,x ABS,x */
    Read_Absolute_Index_X(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x3E) /* ROL $ssss,x ABS,x */
    RMW_Absolute_Index_X(Insn_ROL);
OPCODE_EPILOG

OPCODE_PROLOG(0x59) /* EOR $ssss,y ABS,y */
    Read_Absolute_Index_Y(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x5D) /* EOR $ssss,x ABS,x */
    Read_Absolute_Index_X(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x5E) /* LSR $ssss,x ABS,x */
    RMW_Absolute_Index_X(Insn_LSR);
OPCODE_EPILOG

OPCODE_PROLOG(0x79) /* ADC $ssss,y ABS,y */
    Read_Absolute_Index_Y(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x7D) /* ADC $ssss,x ABS,x */
    Read_Absolute_Index_X(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x7E) /* ROR $ssss,x ABS,x */
    RMW_Absolute_Index_X(Insn_ROR);
OPCODE_EPILOG

OPCODE_PROLOG(0x99) /* STA $ssss,y ABS,y */
    Write_Absolute_Index_Y(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0x9D) /* STA $ssss,x ABS,x */
    Write_Absolute_Index_X(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xB9) /* LDA $ssss,y ABS,y */
    Read_Absolute_Index_Y(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xBC) /* LDY $ssss,x ABS,x */
    Read_Absolute_Index_X(R->Y); Update_NZ(R->Y);
OPCODE_EPILOG

OPCODE_PROLOG(0xBD) /* LDA $ssss,x ABS,x */
    Read_Absolute_Index_X(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xBE) /* LDX $ssss,y ABS,y */
    Read_Absolute_Index_Y(R->X); Update_NZ(R->X);
OPCODE_EPILOG

OPCODE_PROLOG(0xD9) /* CMP $ssss,y ABS,y */
    Read_Absolute_Index_Y(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xDD) /* CMP $ssss,x ABS,x */
    Read_Absolute_Index_X(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xDE) /* DEC $ssss,x ABS,x */
    RMW_Absolute_Index_X(Insn_DEC);
OPCODE_EPILOG

OPCODE_PROLOG(0xF9) /* SBC $ssss,y ABS,y */
    Read_Absolute_Index_Y(data); Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xFD) /* SBC $ssss,x ABS,x */
    Read_Absolute_Index_X(data); Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xFE) /* INC $ssss,x ABS,x */
    RMW_Absolute_Index_X(Insn_INC);
OPCODE_EPILOG


OPCODE_PROLOG(0x01) /* ORA ($ss,x) */
    Read_Indexed_Indirect_X(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x11) /* ORA ($ss),y */
    Read_Indirect_Indexed_Y(data); Insn_ORA(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x21) /* AND ($ss,x) */
    Read_Indexed_Indirect_X(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x31) /* AND ($ss),y */
    Read_Indirect_Indexed_Y(data); Insn_AND(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x41) /* EOR ($ss,x) */
    Read_Indexed_Indirect_X(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x51) /* EOR ($ss),y */
    Read_Indirect_Indexed_Y(data); Insn_EOR(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x61) /* ADC ($ss,x) */
    Read_Indexed_Indirect_X(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x71) /* ADC ($ss),y */
    Read_Indirect_Indexed_Y(data); Insn_ADC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0x81) /* STA ($ss,x) */
    Write_Indexed_Indirect_X(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0x91) /* STA ($ss),y */
    Write_Indirect_Indexed_Y(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xA1) /* LDA ($ss,x) */
    Read_Indexed_Indirect_X(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xB1) /* LDA ($ss),y */
    Read_Indirect_Indexed_Y(R->A); Update_NZ(R->A);
OPCODE_EPILOG

OPCODE_PROLOG(0xC1) /* CMP ($ss,x) */
    Read_Indexed_Indirect_X(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xD1) /* CMP ($ss),y */
    Read_Indirect_Indexed_Y(data); Insn_CMP(R->A, data);
OPCODE_EPILOG

OPCODE_PROLOG(0xE1) /* SBC ($ss,x) */
    Read_Indexed_Indirect_X(data); Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xF1) /* SBC ($ss),y */
    Read_Indirect_Indexed_Y(data); Insn_SBC(data);
OPCODE_EPILOG


OPCODE_PROLOG(0x0A) /* ASL a ACC */
    Insn_ASL(R->A);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x2A) /* ROL a ACC */
    Insn_ROL(R->A);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x4A) /* LSR a ACC */
    Insn_LSR(R->A);
    PC.word++;
OPCODE_EPILOG

OPCODE_PROLOG(0x6A) /* ROR a ACC */
    Insn_ROR(R->A);
    PC.word++;
OPCODE_EPILOG


OPCODE_PROLOG(0xEF) /* INS abcd */
    Address_Absolute();
    data = Read(address.word) + 1;
    Write(address.word, data);
    Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xFF) /* INS abcd,X */
    Address_Absolute();
    address.word += R->X;
    data = Read(address.word) + 1;
    Write(address.word, data);
    Insn_SBC(data);
OPCODE_EPILOG

OPCODE_PROLOG(0xF2) /* JAM */
    R->Jammed = 1;
OPCODE_EPILOG

OPCODE_PROLOG_DEFAULT
    if(R->TrapBadOps)
    {
        printf
        (
            "[FN2A03] Unrecognized instruction: $%02X at PC=$%04X\n",
            Fetch(PC.word),(UINT16)(PC.word)
        );
    }
#ifdef DEBUG
        printf("\nOpcode fallback trace:\n\n");
        for (opcode_count = 0; opcode_count < 10; opcode_count++)
        {
            printf("$%02X ",opcode_trace[opcode_count]);
            opcode_trace[opcode_count] = 0;
        }
        printf("\n\n");
        opcode_count = 0;
#endif
OPCODE_EPILOG
