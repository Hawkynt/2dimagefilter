/* FakeNES - A free, portable, Open Source NES emulator.

   cpu.h: Declarations for the CPU abstraction.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef CPU_H_INCLUDED
#define CPU_H_INCLUDED
#include <allegro.h>
#include "common.h"
#include "core.h"
#include "rom.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

/* Use macros instead of inline functions for stack
   and zero-page handlers. */
#define INLINE_WITH_MACROS

#define CPU_INTERRUPT_NONE              0
#define CPU_INTERRUPT_NMI               1
#define CPU_INTERRUPT_IRQ_BASE          2

/* Maskable IRQ, cleared after an IRQ is acknowledged. */
#define CPU_INTERRUPT_IRQ_SINGLE_SHOT   2
#define CPU_INTERRUPT_IRQ           CPU_INTERRUPT_IRQ_SINGLE_SHOT
#define CPU_INTERRUPT_IRQ_SOURCE(x) (CPU_INTERRUPT_IRQ_BASE + 1 + (x))
/* pAPU Delta Modulation Channel IRQ. */
#define CPU_INTERRUPT_IRQ_DMC           CPU_INTERRUPT_IRQ_SOURCE(0)
/* Frame IRQ. */
#define CPU_INTERRUPT_IRQ_FRAME         CPU_INTERRUPT_IRQ_SOURCE(1)
/* MMC-specific IRQ. */
#define CPU_INTERRUPT_IRQ_MMC           CPU_INTERRUPT_IRQ_SOURCE(2)
#define CPU_INTERRUPT_IRQ_SOURCE_MAX    FN2A03_INT_IRQ_SOURCE_MAX

#define CPU_RAM_SIZE    65536
#define CPU_SRAM_SIZE   8192

extern UINT8 cpu_ram[CPU_RAM_SIZE];
extern UINT8 cpu_sram[CPU_SRAM_SIZE];

extern FN2A03 cpu_context;

extern INT8 cpu_patch_table[CPU_RAM_SIZE];

/* Number of used entries in cpu_patch_info. */
extern int cpu_patch_count;

/* Format of cpu_patch_info. */
/* Designed primarily for use with Game Genie. */

typedef struct _CPU_PATCH
{
   BOOL active;
   USTRING title;
   UINT16 address;
   UINT8 value;
   UINT8 match_value;
   BOOL enabled;

} CPU_PATCH;

/* Try to keep it fast - 15 patches limit. */
#define CPU_MAX_PATCHES 15

extern CPU_PATCH cpu_patch_info[CPU_MAX_PATCHES];

extern int cpu_init (void);
extern void cpu_memmap_init (void);
extern void cpu_exit (void);
extern void cpu_reset (void);

extern void cpu_interrupt (int type);
extern void cpu_clear_interrupt (int type);
extern void cpu_queue_interrupt (int type, cpu_time_t time);

extern UINT16 *cpu_active_pc;

extern void cpu_free_prg_rom (const ROM *);
extern UINT8 *cpu_get_prg_rom_pages (ROM *);

extern void cpu_enable_sram (void);
extern void cpu_disable_sram (void);

#define CPU_MAX_BLOCKS  (64 / 2)

extern UINT8 dummy_read[(8 << 10)];
extern UINT8 dummy_write[(8 << 10)];

extern UINT8 *cpu_block_2k_read_address[CPU_MAX_BLOCKS];
extern UINT8 *cpu_block_2k_write_address[CPU_MAX_BLOCKS];

extern UINT8 (*cpu_block_2k_read_handler[CPU_MAX_BLOCKS]) (UINT16);
extern void  (*cpu_block_2k_write_handler[CPU_MAX_BLOCKS]) (UINT16, UINT8);

extern UINT8 cpu_read_direct_safeguard (UINT16);
extern void cpu_write_direct_safeguard (UINT16, UINT8);

extern void cpu_start_new_scanline (void);

extern cpu_time_t cpu_get_cycles_line (void);
extern cpu_time_t cpu_get_cycles (void);
extern cpu_time_t cpu_get_elapsed_cycles (cpu_time_t *timestamp);

extern void cpu_save_state (PACKFILE *, int);
extern void cpu_load_state (PACKFILE *, int);

#include "cpu_in.h"

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !CPU_H_INCLUDED */
