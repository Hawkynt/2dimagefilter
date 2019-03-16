/* Mapper #66 (GNROM). */
/* This mapper is fully supported. */

#include "mmc/shared.h"

static int gnrom_init (void);
static void gnrom_reset (void);
static void gnrom_save_state (PACKFILE *, int);
static void gnrom_load_state (PACKFILE *, int);

static const MMC mmc_gnrom =
{
   66, "GNROM",
   gnrom_init, gnrom_reset,
   "GNROM\0\0\0",
   gnrom_save_state, gnrom_load_state
};

static UINT8 gnrom_last_write = 0;

#define GNROM_PRG_ROM_MASK 0xf0
#define GNROM_CHR_ROM_MASK 0x0f

static void gnrom_write (UINT16 address, UINT8 value)
{
   int chr_page, prg_page;
   int index;

   /* Store page #s for state saving. */
   gnrom_last_write = value;

   /* Extract ROM page # (xxxx0000). */
   prg_page = ((value & GNROM_PRG_ROM_MASK) >> 4);

   /* Select requested 32k ROM page. */
   cpu_set_read_address_32k_rom_block (0x8000, prg_page);

   /* Extract CHR-ROM page # (0000xxxx). */
   chr_page = (value & GNROM_CHR_ROM_MASK);

   /* Convert 8k page # to 1k. */
   chr_page *= 8;

   /* Select requested 8k CHR-ROM page. */
   for (index = 0; index < 8; index++)
      ppu_set_ram_1k_pattern_vrom_block ((index << 10), (chr_page + index));
}

static void gnrom_reset (void)
{
   int index;

   /* Select first 32k ROM page. */
   cpu_set_read_address_32k_rom_block (0x8000, MMC_FIRST_ROM_BLOCK);

   /* Select first 8k CHR-ROM page. */
   for (index = 0; index < 8; index++)
      ppu_set_ram_1k_pattern_vrom_block ((index << 10), index);
}

static int gnrom_init (void)
{
   if (mmc_pattern_vram_in_use)
   {
      /* Mapper requires some CHR ROM */
      WARN_GENERIC();
      return (-1);
   }

   /* Install write handler. */
   cpu_set_write_handler_32k (0x8000, gnrom_write);

   /* Set initial mappings. */
   gnrom_reset ();

   /* Return success. */
   return (0);
}

static void gnrom_save_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   pack_putc (gnrom_last_write, file);
}

static void gnrom_load_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   gnrom_write (0x8000, pack_getc (file));
}
