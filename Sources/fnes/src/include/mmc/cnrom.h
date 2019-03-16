/* Mapper #3 (CNROM). */
/* This mapper is fully supported. */

#include "mmc/shared.h"

static int cnrom_init (void);
static void cnrom_reset (void);
static void cnrom_save_state (PACKFILE *, int);
static void cnrom_load_state (PACKFILE *, int);

static const MMC mmc_cnrom =
{
   3, "CNROM",
   cnrom_init, cnrom_reset,
   "CNROM\0\0\0",
   cnrom_save_state, cnrom_load_state
};

static UINT8 cnrom_last_write = 0;

static void cnrom_write (UINT16 address, UINT8 value)
{
   int index;

   /* Store page # for state saving. */
   cnrom_last_write = value;

   /* Convert 8k page # to 1k. */
   value *= 8;

   /* Select requested 8k CHR-ROM page. */
   for (index = 0; index < 8; index++)
      ppu_set_ram_1k_pattern_vrom_block ((index << 10), (value + index));
}

static void cnrom_reset (void)
{
   int index;

   /* Select first 32k ROM page. */
   cpu_set_read_address_32k_rom_block (0x8000, MMC_FIRST_ROM_BLOCK);

   /* Select first 8k CHR-ROM page. */
   for (index = 0; index < 8; index++)
      ppu_set_ram_1k_pattern_vrom_block ((index << 10), index);
}

static int cnrom_init (void)
{
   if (mmc_pattern_vram_in_use)
   {
      /* Mapper requires some CHR ROM */
      WARN_GENERIC();
      return (-1);
   }

   /* Install write handler. */
   cpu_set_write_handler_32k (0x8000, cnrom_write);

   /* Set initial mappings. */
   cnrom_reset ();

   /* Return success. */
   return (0);
}

static void cnrom_save_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   pack_putc (cnrom_last_write, file);
}

static void cnrom_load_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   cnrom_write (0x8000, pack_getc (file));
}
