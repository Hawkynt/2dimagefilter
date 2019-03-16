/* Mapper #24 and #26 (Konami VRC6 and VRC6V). */
/* These mappers are fully supported. */
/* IRQ prediction is supported. */

#include "mmc/shared.h"

static int vrc6_init (void);
static int vrc6v_init (void);
static void vrc6_reset (void);
static void vrc6_save_state (PACKFILE *, int);
static void vrc6_load_state (PACKFILE *, int);

static const MMC mmc_vrc6 =
{
   24,
   "Konami VRC6 + ExSound",
   vrc6_init,
   vrc6_reset,
   "VRC6\0\0\0\0",
   vrc6_save_state,
   vrc6_load_state
};

static const MMC mmc_vrc6v =
{
   26,
   "Konami VRC6V + ExSound",
   vrc6v_init,
   vrc6_reset,
   "VRC6V\0\0\0",
   vrc6_save_state,
   vrc6_load_state
};

static const ENUM vrc6_mirroring_table[] =
{
   MIRRORING_VERTICAL,
   MIRRORING_HORIZONTAL,
   MIRRORING_ONE_SCREEN_2000,
   MIRRORING_ONE_SCREEN_2400
};

static const UINT8 vrc6_mirroring_mask = 0x0c;

static BOOL vrc6_swap_address_pins = FALSE;

static UINT8 vrc6_prg_bank[2];
static UINT8 vrc6_chr_bank[8];

/* Internal ~1.79MHz clock (scaled to ~5.37MHz for efficiency). */
static cpu_time_t vrc6_clock_counter = 0;

/* How often the IRQ counter gets clocked in CYCLE MODE. */
#define VRC6_IRQ_CYCLE_LENGTH    CYCLE_LENGTH                  /* 1*3 */
/* How often the IRQ counter gets clocked in SCANLINE MODE. */
#define VRC6_IRQ_SCANLINE_LENGTH SCANLINE_LENGTH               /* 341 */

#define VRC6_IRQ_FREQUENCY \
   ((vrc6_irq_reg & 0x04) ? VRC6_IRQ_CYCLE_LENGTH : VRC6_IRQ_SCANLINE_LENGTH)

static UINT16 vrc6_irq_timer   = 0;
static UINT8  vrc6_irq_counter = 0x00;
static UINT8  vrc6_irq_latch   = 0x00;
static UINT8  vrc6_irq_reg     = 0x00;
static BOOL   vrc6_enable_irqs = FALSE;

/* IRQ prediction. */
static cpu_time_t vrc6_prediction_timestamp = 0;
static cpu_time_t vrc6_prediction_cycles = 0;

static void vrc6_update_irq_counter (cpu_time_t cycles, BOOL allow_irq)
{
   /* Slave function used by both emulation and prediction(simulation). */

   cpu_time_t count, offset;

   if (!vrc6_enable_irqs)
      return;

   /* Speed hack.  Shouldn't hurt accuracy. =) */
   cycles /= CYCLE_LENGTH;

   /* Cache it for efficiency. */
   count = VRC6_IRQ_FREQUENCY;

   for (offset = 0; offset < cycles; offset++)
   {
      vrc6_irq_timer += CYCLE_LENGTH;
      if (vrc6_irq_timer >= count)
      {
         vrc6_irq_timer -= count;
   
         if (vrc6_irq_counter == 0xFF)
         {
            vrc6_irq_counter = vrc6_irq_latch;

            if (allow_irq)
               cpu_queue_interrupt (CPU_INTERRUPT_IRQ_MMC, (vrc6_prediction_timestamp + offset));
         }
         else
            vrc6_irq_counter++;
      }
   }
}

static void vrc6_predict_irq (cpu_time_t cycles)
{
   UINT16 saved_irq_timer;
   UINT8 saved_irq_counter;

   if (!vrc6_enable_irqs)
      return;

   /* Save vars since we're just simulating. */
   saved_irq_timer = vrc6_irq_timer;
   saved_irq_counter = vrc6_irq_counter;

   /* Remember to allow IRQs here. */
   /* Just go through the motions... */
   vrc6_update_irq_counter (cycles, TRUE);

   /* Restore saved vars. */
   vrc6_irq_timer = saved_irq_timer;
   vrc6_irq_counter = saved_irq_counter;
}

static void vrc6_repredict_irq (void)
{
   /* This needs to be called whenver a possible mid-scanline change to the
      IRQ parameters occurs (to update prediction). */

   cpu_time_t cycles, cycles_remaining;

   cycles = cpu_get_cycles ();

   cycles_remaining = (cycles - vrc6_prediction_timestamp);
   if (cycles_remaining <= 0)
      return;

   if (cycles_remaining > vrc6_prediction_cycles)
      cycles_remaining = vrc6_prediction_cycles;

   vrc6_predict_irq (cycles_remaining);
}

static void vrc6_process (void)
{
   /* Call this before accessing the state of the mapper - before reads,
      writes, and state-sensetive emulation. */

   const cpu_time_t elapsed_cycles = cpu_get_elapsed_cycles (&vrc6_clock_counter);
   if (elapsed_cycles == 0)
      return;

   /* *Don't* allow IRQs here, or it'll conflict with prediction. */
   vrc6_update_irq_counter (elapsed_cycles, FALSE);
}

static void vrc6_predict_irqs (cpu_time_t cycles)
{
   /* Wrapper for vrc6_predict_irq() that is exposed to the MMC interfce,
      thus must take care of a few specific things. */

   /* Save parameters for re-prediction if a mid-scanline change occurs. */
   vrc6_prediction_timestamp = cpu_get_cycles ();
   vrc6_prediction_cycles = cycles;

   /* Sync state. */
   vrc6_process ();

   vrc6_predict_irq (cycles);
}

static void vrc6_update_prg_bank (int bank)
{
   switch (bank)
   {
      case 0:  /* 16k ROM page select. */
      {
         cpu_set_read_address_16k_rom_block (0x8000, vrc6_prg_bank[0]);
         break;
      }

      case 1:  /* 8k ROM page select. */
      {
         cpu_set_read_address_8k_rom_block (0xC000, vrc6_prg_bank[1]);
         break;
      }

      default:
         WARN_GENERIC();
   }
}

static void vrc6_update_chr_bank (int bank)
{
   if (ROM_CHR_ROM_PAGES <= 0)
      return;

   /* Set new VROM banking. */
   ppu_set_ram_1k_pattern_vrom_block ((bank << 10), vrc6_chr_bank[bank]);
}

static void vrc6_write (UINT16 address, UINT8 value)
{
   int major, minor;

   /* Sync state. */
   vrc6_process ();

   /* Swap address pins. */
   if (vrc6_swap_address_pins)
      address = ((address & 0xfffc) | ((address >> 1) & 1) | ((address << 1) & 2));

   /* Extract command indexes. */
   major = (address & 0xf000);
   minor = (address & 0x000f);

   switch (major)
   {
      case 0x8000:
      {
         if (minor != 0x0000)
            break;

         /* Set requested 16k ROM page at $8000. */
         vrc6_prg_bank[0] = value;
         vrc6_update_prg_bank (0);

         break;
      }

      case 0xb000:
      {
         /* Mirroring select. */

         if (minor != 0x0003)
            break;

         /* Discard unused bits. */
         value = ((value & vrc6_mirroring_mask) >> 2);

         /* Use value from LUT. */
         ppu_set_mirroring (vrc6_mirroring_table[value]);

         break;
      }

      case 0xc000:
      {
         if (minor != 0x0000)
            break;

         /* Set requested 8k ROM page at $C000. */
         vrc6_prg_bank[1] = value;
         vrc6_update_prg_bank (1);

         break;
      }

      case 0xd000:
      {
         if (minor >= 0x0004)
            break;

         /* Set requested 1k CHR-ROM page. */
         vrc6_chr_bank[minor] = value;
         vrc6_update_chr_bank (minor);

         break;
      }

      case 0xe000:
      {
         if (minor >= 0x0004)
            break;

         /* Set requested 1k CHR-ROM page. */
         vrc6_chr_bank[(minor + 4)] = value;
         vrc6_update_chr_bank ((minor + 4));

         break;
      }

      case 0xf000:
      {
         switch (minor)
         {
            case 0x0000:
            {
               /* Set the IRQ counter load value. */
               vrc6_irq_latch = value;

               /* Update prediction. */
               vrc6_repredict_irq ();

               break;
            }

            case 0x0001:
            {
               /* Save the value for future writes to $F002. */
               vrc6_irq_reg = value;

               /* Enable or disable the IRQ counter. */
               vrc6_enable_irqs = TRUE_OR_FALSE(value & 0x02);

               /* If enabled... */
               if (vrc6_enable_irqs)
               {
                  /* Load the counter with the value from the $F000 latch. */
                  vrc6_irq_counter = vrc6_irq_latch;

                  /* Reset the timer. */
                  vrc6_irq_timer = 0;
               }

               /* Reset IRQ status. */
               cpu_clear_interrupt (CPU_INTERRUPT_IRQ_MMC);

               /* Update prediction. */
               vrc6_repredict_irq ();

               break;
            }

            case 0x0002:
            {
               /* Swap IRQ counter enable flags. */
               vrc6_enable_irqs = TRUE_OR_FALSE(vrc6_irq_reg & 0x01);

               /* Reset IRQ status. */
               cpu_clear_interrupt (CPU_INTERRUPT_IRQ_MMC);

               /* Update prediction. */
               vrc6_repredict_irq ();

               break;
            }         

            default:
               break;
         }

         break;
      }

      default:
         break;
   } 

   /* Send to ExSound. */
   apu_write (address, value);
}

static void vrc6_reset (void)
{
   /* Reset PRG banking. */

   /* Select first 16k page in lower 16k. */
   vrc6_prg_bank[0] = 0;
   vrc6_update_prg_bank (0);

   /* Select last 16k page in upper 16k. */
   cpu_set_read_address_16k (0xc000, LAST_ROM_PAGE);

   vrc6_prg_bank[1] = ((ROM_PRG_ROM_PAGES * 2) - 2);
   vrc6_update_prg_bank (1);

   /* Reset internal clock. */
   vrc6_clock_counter = 0;

   /* Reset IRQ variables. */
   vrc6_irq_timer   = 0;
   vrc6_irq_counter = 0x00;
   vrc6_irq_latch   = 0x00;
   vrc6_irq_reg     = 0x00;
   vrc6_enable_irqs = FALSE;

   vrc6_prediction_timestamp = 0;
   vrc6_prediction_cycles = 0;
}

static int vrc6_base_init (void)
{
   int index;

   /* Install write handler. */
   cpu_set_write_handler_32k (0x8000, vrc6_write);

   /* Install IRQ predicter. */
   mmc_predict_irqs = vrc6_predict_irqs;

   /* Select ExSound chip. */
   apu_set_exsound (APU_EXSOUND_VRC6);

   /* Set initial mappings and reset variables. */
   vrc6_reset ();

   /* Return success. */
   return (0);
}

static int vrc6_init (void)
{
   /* Disable address pin swap. */
   vrc6_swap_address_pins = FALSE;

   return (vrc6_base_init ());
}

static int vrc6v_init (void)
{
   /* Pins A0 and A1 are swapped in VRC6V. */
   vrc6_swap_address_pins = TRUE;

   return (vrc6_base_init ());
}

static void vrc6_save_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   /* Save banking. */
   pack_fwrite (vrc6_prg_bank, 2, file);
   pack_fwrite (vrc6_chr_bank, 8, file);

   /* Save internal clock. */
   pack_iputl (vrc6_clock_counter, file);

   /* Save IRQ status. */
   pack_iputw (vrc6_irq_timer,             file);
   pack_putc  (vrc6_irq_counter,           file);
   pack_putc  (vrc6_irq_latch,             file);
   pack_putc  (vrc6_irq_reg,               file);        
   pack_putc  ((vrc6_enable_irqs ? 1 : 0), file);

   pack_iputl (vrc6_prediction_timestamp, file);
   pack_iputl (vrc6_prediction_cycles,    file);
}

static void vrc6_load_state (PACKFILE *file, int version)
{
   int index;

   RT_ASSERT(file);

   /* Restore banking. */
   pack_fread (vrc6_prg_bank, 2, file);
   pack_fread (vrc6_chr_bank, 8, file);

   vrc6_update_prg_bank (0);
   vrc6_update_prg_bank (1);

   for (index = 0; index < 8; index++)
      vrc6_update_chr_bank (index);

   /* Restore internal clock. */
   vrc6_clock_counter = pack_igetl (file);

   /* Restore IRQ status. */
   vrc6_irq_timer   = pack_igetw (file);
   vrc6_irq_counter = pack_getc  (file);
   vrc6_irq_latch   = pack_getc  (file);
   vrc6_irq_reg     = pack_getc  (file);
   vrc6_enable_irqs = TRUE_OR_FALSE(pack_getc (file));

   vrc6_prediction_timestamp = pack_igetl (file);
   vrc6_prediction_cycles    = pack_igetl (file);
}
