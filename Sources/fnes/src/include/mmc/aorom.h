/* Mapper #7 (AOROM). */
/* This mapper is fully supported. */

#include "mmc/shared.h"

static int aorom_init (void);
static void aorom_reset (void);
static void aorom_save_state (PACKFILE *, int);
static void aorom_load_state (PACKFILE *, int);

static const MMC mmc_aorom =
{
   7, "AOROM",
   aorom_init, aorom_reset,
   "AOROM\0\0\0",
   aorom_save_state, aorom_load_state
};

static UINT8 aorom_last_write = 0;

#define AOROM_MIRRORING_BIT   (1 << 4)

static void aorom_write (UINT16 address, UINT8 value)
{
   /* Save page # and mirroring for state saving. */
   aorom_last_write = value;

   /* Is this reversed? */
   ppu_set_mirroring (((value & AOROM_MIRRORING_BIT) ?
      MIRRORING_ONE_SCREEN_2400 : MIRRORING_ONE_SCREEN_2000));

   /* Mask off upper 4 bits. */
   value &= 0x0f;

   /* Select requested 32k page. */
   cpu_set_read_address_32k_rom_block (0x8000, value);
}

static void aorom_reset (void)
{
   /* Select first 32k page. */
   cpu_set_read_address_32k_rom_block (0x8000, 0);
}

static int aorom_init (void)
{
   /* No VROM hardware. */
   ppu_set_ram_8k_pattern_vram ();
   mmc_pattern_vram_in_use = TRUE;

   /* Set the default mirroring. */
   mmc_name_table_count = 2;
   ppu_set_mirroring (MIRRORING_ONE_SCREEN_2000);

   /* Install write handler. */
   cpu_set_write_handler_32k (0x8000, aorom_write);

   /* Set initial mappings. */
   aorom_reset ();

   /* Return success. */
   return (0);
}

static void aorom_save_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   pack_putc (aorom_last_write, file);
}

static void aorom_load_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   aorom_write (0x8000, pack_getc (file));
}
