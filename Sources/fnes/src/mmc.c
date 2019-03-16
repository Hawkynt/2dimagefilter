/* FakeNES - A free, portable, Open Source NES emulator.

   mmc.c: Implementation of the MMC emulation.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#include <stdio.h>
#include <string.h>
#include "apu.h"
#include "common.h"
#include "cpu.h"
#include "debug.h"
#include "gui.h"
#include "mmc.h"
#include "ppu.h"
#include "rom.h"
#include "timing.h"
#include "types.h"

int (*mmc_hblank_start) (int);
int (*mmc_scanline_start) (int);
int (*mmc_scanline_end) (int);
void (*mmc_predict_irqs) (cpu_time_t cycles);
void (*mmc_check_latches) (UINT16);

static int mmc_name_table_count;
static int mmc_pattern_vram_in_use;


int mmc_get_name_table_count(void)
{
    return mmc_name_table_count;
}


int mmc_uses_pattern_vram(void)
{
    return mmc_pattern_vram_in_use;
}


static void null_save_state (PACKFILE *, int);

static void null_load_state (PACKFILE *, int);


#include "mmc/mmc1.h"

#include "mmc/mmc3.h"

#include "mmc/mmc2and4.h"

#include "mmc/mmc5.h"


#include "mmc/unrom.h"

#include "mmc/cnrom.h"

#include "mmc/aorom.h"

#include "mmc/gnrom.h"


#include "mmc/bandai.h"

#include "mmc/dreams.h"

#include "mmc/nina.h"

#include "mmc/sunsoft4.h"


#include "mmc/vrc6.h"


#include "mmc/ffe_f3.h"


#define MMC_FIRST_LIST_ITEM(id)     \
    if (mmc_ ##id.number == rom -> mapper_number)       \
        rom -> current_mmc = &mmc_ ##id

#define MMC_NEXT_LIST_ITEM(id)      \
    else if (mmc_ ##id.number == rom -> mapper_number)  \
        rom -> current_mmc = &mmc_ ##id

#define MMC_LAST_LIST_ITEM()        \
    else rom -> current_mmc = NIL


static int none_init (void);

static void none_reset (void);


const MMC mmc_none =
{
    0, "No mapper",

    none_init, none_reset,


    "NONE\0\0\0\0",

    null_save_state, null_load_state
};


void none_reset (void)
{
    /* Do nothing. */
}


int none_init (void)
{
    /* Select first 32k page. */

    cpu_set_read_address_32k_rom_block (0x8000, 0);


    if (ROM_CHR_ROM_PAGES > 0)
    {
        int index;

        /* Select first 8k page. */

        for (index = 0; index < 8; index ++)
        {
            ppu_set_ram_1k_pattern_vrom_block ((index << 10), index);
        }
    }
    else
    {
        /* No VROM is present. */

        ppu_set_ram_8k_pattern_vram ();
    }


    return (0);
}


static void null_save_state (PACKFILE * file, int version)
{
    /* Do nothing. */
}


static void null_load_state (PACKFILE * file, int version)
{
    /* Do nothing. */
}


void mmc_request (ROM * rom)
{
    MMC_FIRST_LIST_ITEM (none);     /* No mapper. */


    /* Nintendo MMCs. */

    MMC_NEXT_LIST_ITEM (mmc1);      /* MMC1. */

    MMC_NEXT_LIST_ITEM (mmc2);      /* MMC2. */

    MMC_NEXT_LIST_ITEM (mmc3);      /* MMC3. */

    MMC_NEXT_LIST_ITEM (mmc4);      /* MMC4. */

    MMC_NEXT_LIST_ITEM (mmc5);      /* MMC5. */


    /* Other MMCs. */

    MMC_NEXT_LIST_ITEM (unrom);     /* UNROM. */
                          
    MMC_NEXT_LIST_ITEM (cnrom);     /* CNROM. */

    MMC_NEXT_LIST_ITEM (aorom);     /* AOROM. */

    MMC_NEXT_LIST_ITEM (gnrom);     /* GNROM. */


    MMC_NEXT_LIST_ITEM (bandai);    /* Bandai. */

    MMC_NEXT_LIST_ITEM (dreams);    /* Color Dreams. */

    MMC_NEXT_LIST_ITEM (nina);      /* NINA-001. */

    MMC_NEXT_LIST_ITEM (sunsoft4);  /* Sunsoft mapper #4. */


    MMC_NEXT_LIST_ITEM (vrc6);      /* VRC6. */

    MMC_NEXT_LIST_ITEM (vrc6v);     /* VRC6. */


    MMC_NEXT_LIST_ITEM (ffe_f3);    /* FFE F3xxx. */


    MMC_LAST_LIST_ITEM ();          /* Unsupported mapper. */
}


int mmc_init (void)
{
    int index;


    for (index = 0x8000; index < (64 << 10); index += (8 << 10))
    {
        cpu_set_write_address_8k (index, dummy_write);
    }


    mmc_hblank_start = NIL;

    mmc_scanline_start = NIL;

    mmc_scanline_end = NIL;


    mmc_predict_irqs = NIL;


    mmc_check_latches = NIL;


    mmc_name_table_count =
        (global_rom.control_byte_1 & ROM_CTRL_FOUR_SCREEN) ? 4 : 2;

    mmc_pattern_vram_in_use = (ROM_CHR_ROM_PAGES == 0);


    if (mmc_pattern_vram_in_use)
    {
        /* No VROM is present. */

        ppu_set_ram_8k_pattern_vram ();
    }
    else
    {
        int index;

        /* Select first 8k page. */

        for (index = 0; index < 8; index ++)
        {
            ppu_set_ram_1k_pattern_vrom_block ((index << 10), index);
        }
    }


    apu_set_exsound (APU_EXSOUND_NONE);


    if (ROM_CURRENT_MMC == NIL)
    {
        return (1);
    }


    if (! gui_is_active)
    {
        printf ("Using memory mapper #%u (%s) (%d PRG, %d CHR).\n\n", ROM_MAPPER_NUMBER,
            ROM_CURRENT_MMC -> name, ROM_PRG_ROM_PAGES, ROM_CHR_ROM_PAGES);
    }


    return (ROM_CURRENT_MMC -> init ());
}


void mmc_reset (void)
{
    ROM_CURRENT_MMC -> reset ();
}


void mmc_save_state (PACKFILE * file, int version)
{
    if (ROM_CURRENT_MMC -> save_state)
    {
        pack_fwrite (ROM_CURRENT_MMC -> id, 8, file);


        ROM_CURRENT_MMC -> save_state (file, version);
    }
}


void mmc_load_state (PACKFILE * file, int version)
{
    UINT8 signature [8];


    if (ROM_CURRENT_MMC -> load_state)
    {
        pack_fread (signature, 8, file);


        ROM_CURRENT_MMC -> load_state (file, version);
    }
}
