/* FakeNES - A free, portable, Open Source NES emulator.

   rom.h: Declarations for the ROM file handling.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef ROM_H_INCLUDED
#define ROM_H_INCLUDED
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

typedef struct _ROM ROM;

/* We include 'mmc.h' (after the declaration of 'ROM') here instead of above
   to avoid a mutual-inclusion conflict. */
#include "mmc.h"

extern ROM global_rom;
extern BOOL rom_is_loaded;
    
struct _ROM
{
   const MMC *current_mmc;             /* Memory mapping controller. */
   UINT8 *trainer;                     /* Pointer to trainer data. */
   UINT8 *prg_rom;                     /* Pointer to PRG-ROM data. */
   UINT8 *chr_rom;                     /* Pointer to CHR-ROM data. */
   UINT32 trainer_crc32;               /* Checksum for trainer. */
   UINT32 prg_rom_crc32;               /* Checksum for PRG-ROM. */
   UINT32 chr_rom_crc32;               /* Checksum for CHR-ROM. */
   UINT8 *chr_rom_cache;               /* CHR-ROM cache buffer. */
   UINT8 *chr_rom_cache_tag;           /* CHR-ROM cache tag buffer. */
   int prg_rom_pages;                  /* Number of PRG-ROM pages. */
   int chr_rom_pages;                  /* Nimber of CHR-ROM pages. */
   UINT8 control_byte_1;               /* Header control byte #1. */
   UINT8 control_byte_2;               /* Header control byte #2. */
   int mapper_number;                  /* Number of MMC/mapper to use. */
   UINT8 chr_rom_page_overflow_mask;   /* CHR-ROM bank # wrapping mask. */
   UINT8 chr_rom_page_lookup[256];     /* ?? */
   UINT8 prg_rom_page_overflow_mask;   /* PRG-ROM bank # wrapping mask. */
   UINT8 prg_rom_page_lookup[256];     /* ?? */
   BOOL sram_flag;                     /* If Save RAM/SRAM is present. */
   USTRING filename;                   /* Full Unicode filename. */
};

typedef struct _INES_HEADER
{
   UINT8 signature[4];
   UINT8 prg_rom_pages;
   UINT8 chr_rom_pages;
   UINT8 control_byte_1;
   UINT8 control_byte_2;
   UINT8 reserved[8];

} INES_HEADER;

extern int load_rom (const UCHAR*, ROM *);
extern int load_rom_from_zip (const UCHAR*, ROM *);
extern void free_rom (const ROM *);

#define ROM_TRAINER_SIZE      512

/* Control byte flags. */
#define ROM_CTRL_MIRRORING    (1 << 0)
#define ROM_CTRL_BATTERY      (1 << 1)
#define ROM_CTRL_TRAINER      (1 << 2)
#define ROM_CTRL_FOUR_SCREEN  (1 << 2)

/* Shortcut macros. */
#define ROM_PRG_ROM_PAGES              global_rom.prg_rom_pages
#define ROM_CHR_ROM_PAGES              global_rom.chr_rom_pages
#define ROM_MAPPER_NUMBER              global_rom.mapper_number
#define ROM_CURRENT_MMC                global_rom.current_mmc
#define ROM_PRG_ROM                    global_rom.prg_rom
#define ROM_CHR_ROM                    global_rom.chr_rom
#define ROM_CHR_ROM_CACHE              global_rom.chr_rom_cache
#define ROM_CHR_ROM_CACHE_TAG          global_rom.chr_rom_cache_tag
#define ROM_PRG_ROM_PAGE_LOOKUP        global_rom.prg_rom_page_lookup
#define ROM_PRG_ROM_PAGE_OVERFLOW_MASK global_rom.prg_rom_page_overflow_mask
#define ROM_CHR_ROM_PAGE_LOOKUP        global_rom.chr_rom_page_lookup
#define ROM_CHR_ROM_PAGE_OVERFLOW_MASK global_rom.chr_rom_page_overflow_mask

/* Memory map helper macros. */
#define ROM_PAGE_16K(page) (ROM_PRG_ROM + (page * 0x4000))
#define ROM_PAGE_8K(page)  (ROM_PRG_ROM + (page * 0x2000))
#define FIRST_ROM_PAGE     ROM_PRG_ROM
#define LAST_ROM_PAGE      (ROM_PAGE_16K((ROM_PRG_ROM_PAGES - 1)))

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !ROM_H_INCLUDED */
