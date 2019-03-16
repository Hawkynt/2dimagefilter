/* FakeNES - A free, portable, Open Source NES emulator.

   ppu.c: Declarations for the PPU emulation.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef PPU_H_INCLUDED
#define PPU_H_INCLUDED
#include <allegro.h>
#include "common.h"
#include "rom.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif


#define PPU_DISPLAY_LINES   240

/* Register $2000. */

#define PPU_VBLANK_NMI_FLAG_BIT     (1 << 7)

#define PPU_PPU_SLAVE_BIT           (1 << 6)

#define PPU_SPRITE_SIZE_BIT         (1 << 5)

#define PPU_BACKGROUND_TILESET_BIT  (1 << 4)

#define PPU_SPRITE_TILESET_BIT      (1 << 3)

#define PPU_ADDRESS_INCREMENT_BIT   (1 << 2)


#define PPU_NAME_TABLE_SELECT       (3 << 0)


#define PPU_WANT_VBLANK_NMI     \
    (ppu_register_2000 & PPU_VBLANK_NMI_FLAG_BIT)


/* Register $2001. */

#define PPU_COLOR_INTENSITY                 (7 << 5)


#define PPU_SPRITES_ENABLE_BIT              (1 << 4)

#define PPU_BACKGROUND_ENABLE_BIT           (1 << 3)

#define PPU_SPRITES_SHOW_LEFT_EDGE_BIT      (1 << 2)

#define PPU_BACKGROUND_SHOW_LEFT_EDGE_BIT   (1 << 1)

#define PPU_MONOCHROME_DISPLAY_BIT          (1 << 0)


#define PPU_BACKGROUND_ENABLED  \
    (ppu_register_2001 & PPU_BACKGROUND_ENABLE_BIT)

#define PPU_SPRITES_ENABLED     \
    (ppu_register_2001 & PPU_SPRITES_ENABLE_BIT)


#define PPU_BACKGROUND_CLIP_ENABLED     \
    (! (ppu_register_2001 & PPU_BACKGROUND_SHOW_LEFT_EDGE_BIT))

#define PPU_SPRITES_CLIP_ENABLED        \
    (! (ppu_register_2001 & PPU_SPRITES_SHOW_LEFT_EDGE_BIT))


/* Register $2002. */

#define PPU_VBLANK_FLAG_BIT         (1 << 7)

#define PPU_SPRITE_0_COLLISION_BIT  (1 << 6)

#define PPU_SPRITE_OVERFLOW_BIT     (1 << 5)


#define PPU_MAP_RAM         1

#define PPU_MAP_BACKGROUND  2

#define PPU_MAP_SPRITES     4


UINT8 ppu_register_2000;

UINT8 ppu_register_2001;

int ppu_enable_sprite_layer_a;

int ppu_enable_sprite_layer_b;

int ppu_enable_background_layer;


int ppu_scanline;

int ppu_frame_last_line;


int background_enabled;

int sprites_enabled;


UINT8 * one_screen_base_address;


void ppu_free_chr_rom (const ROM *);


UINT8 * ppu_get_chr_rom_pages (ROM *);

void ppu_cache_chr_rom_pages (void);


void ppu_set_ram_1k_pattern_vram_block (UINT16, int);

void ppu_set_ram_1k_pattern_vrom_block (UINT16, int);

void ppu_set_ram_1k_pattern_vrom_block_ex (UINT16, int, int);

void ppu_set_ram_8k_pattern_vram (void);


int ppu_init (void);

void ppu_exit (void);


void ppu_reset (void);


UINT8 ppu_read (UINT16);

void ppu_write (UINT16, UINT8);


void ppu_clear (void);


void ppu_vblank (void);

void ppu_vblank_nmi (void);


void ppu_start_line (void);

void ppu_end_line (void);


void ppu_stub_render_line (int);

void ppu_render_line (int);


void ppu_start_frame (void);


void ppu_start_render (void);

void ppu_end_render (void);


void ppu_set_mirroring_one_screen (void);


int ppu_get_mirroring (void);

void ppu_set_mirroring (int);


void ppu_invert_mirroring (void);


void ppu_set_name_table_internal (int, int);

void ppu_set_name_table_address (int, UINT8 *);

void ppu_set_name_table_address_rom (int, UINT8 *);

void ppu_set_name_table_address_vrom (int, int);


enum
{
    MIRRORING_HORIZONTAL, MIRRORING_VERTICAL,


    MIRRORING_FOUR_SCREEN,


    MIRRORING_ONE_SCREEN,

    MIRRORING_ONE_SCREEN_2000, MIRRORING_ONE_SCREEN_2400,

    MIRRORING_ONE_SCREEN_2800, MIRRORING_ONE_SCREEN_2C00
};


void ppu_save_state (PACKFILE *, int);

void ppu_load_state (PACKFILE *, int);


#ifdef __cplusplus
}
#endif
#endif   /* !PPU_H_INCLUDED */
