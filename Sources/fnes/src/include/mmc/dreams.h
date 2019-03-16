/* Mapper #11 (Color Dreams). */
/* This mapper is fully supported. */

#include "mmc/shared.h"

static int dreams_init (void);
static void dreams_reset (void);
static void dreams_save_state (PACKFILE *, int);
static void dreams_load_state (PACKFILE *, int);

static const MMC mmc_dreams =
{
   11, "Color Dreams",
   dreams_init, dreams_reset,
   "DREAMS\0\0",
   dreams_save_state, dreams_load_state
};

static UINT8 dreams_last_write = 0;

#define DREAMS_PRG_ROM_MASK   0x0f
#define DREAMS_CHR_ROM_MASK   0x70

static void dreams_write (UINT16 address, UINT8 value)
{
   int prg_page, chr_page;
   int index;

   /* Store page #s for state saving. */
   dreams_last_write = value;

   /* Extract ROM page # (0000xxxx). */
   prg_page = (value & DREAMS_PRG_ROM_MASK);

   /* Select requested 32k ROM page. */
   cpu_set_read_address_32k_rom_block (0x8000, prg_page);

   /* Extract CHR-ROM page # (0xxx0000). */
   chr_page = ((value & DREAMS_CHR_ROM_MASK) >> 4);

   /* Convert 8k page # to 1k. */
   chr_page *= 8;

   /* Select requested 8k CHR-ROM page. */
   for (index = 0; index < 8; index++)
      ppu_set_ram_1k_pattern_vrom_block ((index << 10), (chr_page + index));
}

static void dreams_reset (void)
{
   int index;

   /* Select first 32k ROM page. */
   cpu_set_read_address_32k_rom_block (0x8000, MMC_FIRST_ROM_BLOCK);

   /* Select first 8k CHR-ROM page. */
   for (index = 0; index < 8; index++)
      ppu_set_ram_1k_pattern_vrom_block ((index << 10), index);
}

static int dreams_init (void)
{
   if (mmc_pattern_vram_in_use)
   {
      /* Mapper requires some CHR ROM */
      WARN_GENERIC();
      return (-1);
   }

   /* Install write handler. */
   cpu_set_write_handler_32k (0x8000, dreams_write);

   /* Set initial mappings. */
   dreams_reset ();

   /* Return success. */
   return (0);
}

static void dreams_save_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   pack_putc (dreams_last_write, file);
}

static void dreams_load_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   dreams_write (0x8000, pack_getc (file));
}
