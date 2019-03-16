

/*

FakeNES - A portable, Open Source NES emulator.

Distributed under the Clarified Artistic License.

addr.h: CPU addressing mode emulation macros.

Copyright (c) 2001-2006, Randy McDowell.
Copyright (c) 2001-2006, Charles Bilyue'.

This is free software.  See 'LICENSE' for details.
You must read and accept the license prior to use.

This file contains macros for emulating addressing mode
behavior, for the Ricoh RP2A03G CPU.

*/


/* Addressing modes */
/* These macros calculate effective addresses, store them in */
/* an implied variable, and update the Program Counter register. */

/* address set to 'zero_page_address' */
#define Address_Zero_Page() \
    zero_page_address = Fetch(PC.word + 1); PC.word += 2

#define Address_Zero_Page_Index_X() \
    Address_Zero_Page(); zero_page_address += R->X

#define Address_Zero_Page_Index_Y() \
    Address_Zero_Page(); zero_page_address += R->Y

/* address set to 'address' */
#define Address_Zero_Page_Indirect() \
    address.bytes.low = Read_ZP(zero_page_address); \
    address.bytes.high = Read_ZP(zero_page_address + 1)

#define Address_Indexed_Indirect_X() \
    Address_Zero_Page_Index_X(); \
    Address_Zero_Page_Indirect()

#define Address_Absolute() \
    Fetch16(address); PC.word += 3

#define Address_Absolute_Index_X(Rg) \
    Address_Absolute(address); address.word += R->X

#define Address_Absolute_Index_Y(Rg) \
    Address_Absolute(address); address.word += R->Y

#define Address_Indirect_Indexed_Y(address) \
    Address_Zero_Page(); \
    Address_Zero_Page_Indirect(); \
    address.word += R->Y


/* These macros calculate and read from effective addresses. */

#define Read_Immediate(Rg) \
    Rg=Fetch(PC.word + 1); PC.word += 2


#define Read_Zero_Page(Rg) \
    Address_Zero_Page(); Rg = Read_ZP(zero_page_address)

#define Read_Zero_Page_Index_X(Rg) \
    Address_Zero_Page_Index_X(); Rg = Read_ZP(zero_page_address)

#define Read_Zero_Page_Index_Y(Rg) \
    Address_Zero_Page_Index_Y(); Rg = Read_ZP(zero_page_address)


#define Read_Indexed_Indirect_X(Rg) \
    Address_Indexed_Indirect_X(); Rg = Read(address.word)

#define Read_Absolute(Rg) \
    Address_Absolute(); Rg = Read(address.word)

#define Read_Absolute_Indexed(Rg,Index) \
    address.bytes.low += Index; \
    Rg=Read(address.word); \
    if (address.bytes.low < Index) \
    { \
        R->Cycles += CYCLE_LENGTH; \
        address.word += 0x100; \
        Rg = Read(address.word); \
    }

#define Read_Absolute_Index_X(Rg) \
    Address_Absolute(); \
    Read_Absolute_Indexed(Rg, R->X)

#define Read_Absolute_Index_Y(Rg) \
    Address_Absolute(); \
    Read_Absolute_Indexed(Rg, R->Y)

#define Read_Indirect_Indexed_Y(Rg) \
    Address_Zero_Page(); \
    Address_Zero_Page_Indirect(); \
    Read_Absolute_Indexed(Rg, R->Y)

/* These macros calculate and write to effective addresses. */

#define Write_Zero_Page(Rg) \
    Address_Zero_Page(); Write_ZP(zero_page_address, Rg)

#define Write_Zero_Page_Index_X(Rg) \
    Address_Zero_Page_Index_X(); Write_ZP(zero_page_address, Rg)

#define Write_Zero_Page_Index_Y(Rg) \
    Address_Zero_Page_Index_Y(); Write_ZP(zero_page_address, Rg)


#define Write_Indexed_Indirect_X(Rg) \
    Address_Indexed_Indirect_X(); Write(address.word, Rg)

#define Write_Absolute(Rg) \
    Address_Absolute(); Write(address.word, Rg)

#define Write_Absolute_Indexed(Rg,Index) \
    address.bytes.low += Index; \
    Read(address.word); \
    if (address.bytes.low < Index) \
    { \
        address.word += 0x100; \
    } \
    Write(address.word, Rg);

#define Write_Absolute_Index_X(Rg) \
    Address_Absolute(); \
    Write_Absolute_Indexed(Rg, R->X)

#define Write_Absolute_Index_Y(Rg) \
    Address_Absolute(); \
    Write_Absolute_Indexed(Rg, R->Y)

#define Write_Indirect_Indexed_Y(Rg) \
    Address_Zero_Page(); \
    Address_Zero_Page_Indirect(); \
    Write_Absolute_Indexed(Rg, R->Y)

/* These macros calculate effective addresses and perform */
/* read-modify-write operations to the addressed data. */

#define RMW_Zero_Page(Cmd) \
    Address_Zero_Page(); \
    data = Read_ZP(zero_page_address); \
    Write_ZP(zero_page_address, data); \
    Cmd(data); \
    Write_ZP(zero_page_address, data)

#define RMW_Zero_Page_Index_X(Cmd) \
    Address_Zero_Page_Index_X(); \
    data = Read_ZP(zero_page_address); \
    Write_ZP(zero_page_address, data); \
    Cmd(data); \
    Write_ZP(zero_page_address, data)

#define RMW_Absolute(Cmd) \
    Address_Absolute(); \
    data = Read(address.word); \
    Write(address.word, data); \
    Cmd(data); \
    Write(address.word, data)

#define RMW_Absolute_Index_X(Cmd) \
    Address_Absolute(); \
    address.bytes.low += R->X; \
    Read(address.word); \
    if (address.bytes.low < R->X) \
    { \
        address.word += 0x100; \
    } \
    data = Read(address.word); \
    Write(address.word, data); \
    Cmd(data); \
    Write(address.word, data)

