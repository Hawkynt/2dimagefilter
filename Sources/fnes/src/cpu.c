/* FakeNES - A free, portable, Open Source NES emulator.

   cpu.c: Implementation of the CPU abstraction.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#include <stdio.h>
#include <string.h>
#include "apu.h"
#include "common.h"
#include "core.h"
#include "cpu.h"
#include "debug.h"
#include "input.h"
#include "mmc.h"
#include "ppu.h"
#include "rom.h"
#include "save.h"
#include "shared/crc32.h"
#include "types.h"

/* FN2A03 context. */
FN2A03 cpu_context;

/* Memory map. */
UINT8 cpu_ram[CPU_RAM_SIZE];
UINT8 cpu_sram[CPU_SRAM_SIZE];

UINT8 dummy_read[(8 << 10)];
UINT8 dummy_write[(8 << 10)];
UINT8 *cpu_block_2k_read_address[CPU_MAX_BLOCKS];
UINT8 *cpu_block_2k_write_address[CPU_MAX_BLOCKS];
UINT8 (*cpu_block_2k_read_handler[CPU_MAX_BLOCKS]) (UINT16 address);
void (*cpu_block_2k_write_handler[CPU_MAX_BLOCKS]) (UINT16 address, UINT8 value);
UINT8 cpu_read_direct_safeguard (UINT16 address);
void cpu_write_direct_safeguard (UINT16 address, UINT8 value);

/* Memory patch support. */
INT8 cpu_patch_table[CPU_RAM_SIZE];
int cpu_patch_count = 0;
CPU_PATCH cpu_patch_info[CPU_MAX_PATCHES];

/* Program Counter. */
UINT16 *cpu_active_pc = NULL;

int cpu_init (void)
{
   cpu_memmap_init ();

   memset (cpu_ram,  0, sizeof (cpu_ram));
   memset (cpu_sram, 0, sizeof (cpu_sram));

   /* Trainer support - copy the trainer into the memory map */
   if (global_rom.control_byte_1 & ROM_CTRL_TRAINER)
       memcpy ((cpu_sram + 0x1000), global_rom.trainer, ROM_TRAINER_SIZE);

   load_sram ();

   cpu_active_pc = &cpu_context.PC.word;

   cpu_context.TrapBadOps = TRUE;

   memset (dummy_read,  0, sizeof (dummy_read));
   memset (dummy_write, 0, sizeof (dummy_write));

   cpu_reset ();

   return (0);
}

UINT8 cpu_read_direct_safeguard (UINT16 address)
{
   return (CPU_PATCH_FIXUP(cpu_block_2k_read_address[(address >> 11)][address]));
}

void cpu_write_direct_safeguard (UINT16 address, UINT8 value)
{
   cpu_block_2k_write_address[(address >> 11)][address] = value;
}

UINT8 cpu_read_2000_3fff_handler (UINT16 address)
{
   return (ppu_read (address));
}

void cpu_write_2000_3fff_handler (UINT16 address, UINT8 value)
{
   ppu_write (address, value);
}

UINT8 cpu_read_4000_47ff_handler (UINT16 address)
{
   if (address == 0x4014)
      return (ppu_read (address));
   else if (address <= 0x4015)
      return (apu_read (address));
   else if ((address == 0x4016) || (address == 0x4017))
      return (input_read (address));

   return (0);
}

void cpu_write_4000_47ff_handler (UINT16 address, UINT8 value)
{
   if (address == 0x4014)
   {
      ppu_write (address, value);
   }
   else if (address <= 0x4017)
   {
      apu_write (address, value);

      if ((address == 0x4016) || (address == 0x4017))
         input_write (address, value);
   }
}

void cpu_memmap_init (void)
{
   int index;

   /* Load patches from patch table. */
   if (rom_is_loaded)
      load_patches ();

   memset (cpu_patch_table, 0, sizeof (cpu_patch_table));

   /* Start with a clean memory map */
   for (index = 0; index < (64 << 10); index += (2 << 10))
   {
      cpu_set_read_address_2k  (index, dummy_read);
      cpu_set_write_address_2k (index, dummy_write);
   }

   /* Map in RAM */
   for (index = 0; index < (8 << 10); index += (2 << 10))
   {
       cpu_set_read_address_2k  (index, cpu_ram);
       cpu_set_write_address_2k (index, cpu_ram);
   }

   /* Map in SRAM */
   cpu_enable_sram ();

   /* Map in registers */

   cpu_set_read_handler_8k  (0x2000, cpu_read_2000_3fff_handler);
   cpu_set_write_handler_8k (0x2000, cpu_write_2000_3fff_handler);

   cpu_set_read_handler_2k  (0x4000, cpu_read_4000_47ff_handler);
   cpu_set_write_handler_2k (0x4000, cpu_write_4000_47ff_handler);

    if ((global_rom.control_byte_1 & ROM_CTRL_TRAINER))
    {
        /* compute CRC32 for trainer */
        global_rom.trainer_crc32 = make_crc32 (global_rom.trainer,
            ROM_TRAINER_SIZE);
    }
    else
    {
        global_rom.trainer_crc32 = 0;
    }

    /* compute CRC32 for PRG ROM */
    global_rom.prg_rom_crc32 = make_crc32 (global_rom.prg_rom,
        (global_rom.prg_rom_pages * 0x4000));
}

void cpu_exit (void)
{

#ifdef DEBUG

    FILE * dump_file;

    int address;


    dump_file = fopen ("cpudump.ram", "wb");


    if (dump_file)
    {
        for (address = 0; address < 65536; address ++)
        {
            fputc (cpu_read (address), dump_file);
        }


        fclose (dump_file);
    }

#endif

   /* Save SRAM. */
   save_sram ();

   /* Save patches. */
   save_patches ();
}

void cpu_free_prg_rom (const ROM *rom)
{
   RT_ASSERT(rom);

   if (rom->prg_rom)
      free (rom->prg_rom);
}

UINT8 *cpu_get_prg_rom_pages (ROM *rom)
{
   int num_pages;
   int copycount, missing, count, next, pages_mirror_size;

   RT_ASSERT(rom);

   num_pages = rom->prg_rom_pages;

    /* Compute a mask used to wrap invalid PRG ROM page numbers.
     *  As PRG ROM uses a 16k page size, this mask is based
     *  on a 16k page size.
     */
    if (((num_pages * 2 - 1) & (num_pages - 1)) == (num_pages - 1))
    /* compute mask for even power of two */
    {
        pages_mirror_size = num_pages;
    }
    else
    /* compute mask */
    {
        int i;

        /* compute the smallest even power of 2 greater than
           PRG ROM page count, and use that to compute the mask */
        for (i = 0; (num_pages >> i) > 0; i++);

        pages_mirror_size = (1 << i);
    }

    rom->prg_rom_page_overflow_mask = (pages_mirror_size - 1);

    /* identify-map all the present pages */
    for (copycount = 0; copycount < num_pages; copycount++)
    {
        rom->prg_rom_page_lookup[copycount] = copycount;
    }

    /* mirror-map all the not-present pages */
    for (next = num_pages, missing = pages_mirror_size - num_pages,
        count = 1; missing; count <<= 1, missing >>= 1)
    {
        if (missing & 1)
        {
            for (copycount = count; copycount; copycount--, next++)
            {
                rom->prg_rom_page_lookup[next] =
                  rom->prg_rom_page_lookup[(next - count)];
            }
        }
    }

    /* 16k PRG ROM page size */
    rom->prg_rom = malloc ((num_pages * 0x4000));
    if (rom->prg_rom)
    {
        /* initialize to a known value for areas not present in image */
        memset (rom->prg_rom, 0xff, (num_pages * 0x4000));
    }

    return (rom->prg_rom);
}

void cpu_enable_sram (void)
{
   cpu_set_read_address_8k  (0x6000, cpu_sram);
   cpu_set_write_address_8k (0x6000, cpu_sram);
}

void cpu_disable_sram (void)
{
   cpu_set_read_address_8k  (0x6000, dummy_read);
   cpu_set_write_address_8k (0x6000, dummy_write);
}

void cpu_reset (void)
{
   /* Save SRAM. */
   save_sram ();

   FN2A03_Reset (&cpu_context);
}

void cpu_interrupt (int type)
{
   switch (type)
   {
      case CPU_INTERRUPT_NONE:
         break;

      case CPU_INTERRUPT_IRQ_SINGLE_SHOT:
      {
         FN2A03_Interrupt (&cpu_context, FN2A03_INT_IRQ_SINGLE_SHOT);

         break;
      }

      case CPU_INTERRUPT_NMI:
      {
         FN2A03_Interrupt (&cpu_context, FN2A03_INT_NMI);

         break;
      }

      default:
      {
         FN2A03_Interrupt (&cpu_context, FN2A03_INT_IRQ_SOURCE((type -
            CPU_INTERRUPT_IRQ_SOURCE(0))));

         break;
      }
   }
}

void cpu_clear_interrupt (int type)
{
   switch (type)
   {
      case CPU_INTERRUPT_IRQ_SINGLE_SHOT:
      case CPU_INTERRUPT_NMI:
         break;

      default:
      {
         FN2A03_Clear_Interrupt (&cpu_context, FN2A03_INT_IRQ_SOURCE((type -
            CPU_INTERRUPT_IRQ_SOURCE(0))));

         break;
      }
   }
}

void cpu_queue_interrupt (int type, cpu_time_t time)
{
   switch (type)
   {
      case CPU_INTERRUPT_IRQ_SINGLE_SHOT:
      case CPU_INTERRUPT_NMI:
         break;

      default:
      {
         FN2A03_Queue_Interrupt (&cpu_context, FN2A03_INT_IRQ_SOURCE((type -
            CPU_INTERRUPT_IRQ_SOURCE(0))), time);

         break;
      }
   }
}

static cpu_time_t scanline_start_cycle = 0;

void cpu_start_new_scanline (void)
{
   scanline_start_cycle = cpu_context.ICount;
}

cpu_time_t cpu_get_cycles_line (void)
{
   return ((cpu_context.Cycles - scanline_start_cycle));
}

cpu_time_t cpu_get_cycles (void)
{
   return (cpu_context.Cycles);
}

cpu_time_t cpu_get_elapsed_cycles (cpu_time_t *timestamp)
{
   cpu_time_t elapsed_cycles;

   //RT_ASSERT(timestamp);

   elapsed_cycles = (cpu_context.Cycles - *timestamp);
   *timestamp = cpu_context.Cycles;

   return (elapsed_cycles);
}

void cpu_save_state (PACKFILE *file, int version)
{
   int index;

   RT_ASSERT(file);

   /* Save CPU registers. */

   pack_iputw (cpu_context.PC.word, file);

   pack_putc (cpu_context.A, file);
   pack_putc (cpu_context.X, file);
   pack_putc (cpu_context.Y, file);
   pack_putc (cpu_context.S, file);

   pack_putc (FN2A03_Pack_Flags (&cpu_context), file);

   /* Save cycle counters. */

   pack_iputl (cpu_context.ICount, file);
   pack_iputl (cpu_context.Cycles, file);

   /* Save IRQ state. */
   pack_iputl (cpu_context.IRequest, file);

   for (index = 0; index < FN2A03_INT_IRQ_SOURCES; index++)
      pack_iputl (cpu_context.IRQTable[index], file);

   pack_iputl (cpu_context.IRequestQ, file);

   /* Save execution state. */
   pack_putc (cpu_context.Jammed, file);

   /* Save NES internal RAM. */
   pack_fwrite (cpu_ram, 0x800, file);

   /* Save cartridge expansion RAM. */
   pack_fwrite (cpu_sram, 0x2000, file);
}

void cpu_load_state (PACKFILE *file, int version)
{
   UINT8 P;
   int index;

   RT_ASSERT(file);

   /* Restore CPU registers. */

   cpu_context.PC.word = pack_igetw (file);

   cpu_context.A = pack_getc (file);
   cpu_context.X = pack_getc (file);
   cpu_context.Y = pack_getc (file);
   cpu_context.S = pack_getc (file);

   P = pack_getc (file);
   FN2A03_Unpack_Flags (&cpu_context, P);

   /* Restore cycle counters. */

   cpu_context.ICount = pack_igetl (file);
   cpu_context.Cycles = pack_igetl (file);

   /* Restore IRQ state. */
   cpu_context.IRequest = pack_igetl (file);

   for (index = 0; index < FN2A03_INT_IRQ_SOURCES; index++)
      cpu_context.IRQTable[index] = pack_igetl (file);

   cpu_context.IRequestQ = pack_igetl (file);

   /* Restore execution state. */
   cpu_context.Jammed = pack_getc (file);

   /* Restore NES internal RAM. */
   pack_fread (cpu_ram, 0x800, file);

   /* Restore cartridge expansion RAM. */
   pack_fread (cpu_sram, 0x2000, file);
}
