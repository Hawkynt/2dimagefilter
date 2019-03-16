/* FakeNES - A free, portable, Open Source NES emulator.

   timing.h: Declarations for the timing system.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef TIMING_H_INCLUDED
#define TIMING_H_INCLUDED
#include <math.h>
#include "audio.h"
#include "apu.h"
#include "common.h"
#include "core.h"
#include "gui.h"
#include "rom.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

#define MACHINE_RATE_NTSC (1789772.727272727 / 29780.5) // 60.098814 Hz
#define MACHINE_RATE_PAL  (1662607.0 / 33247.5)         // 50.006978 Hz

extern ENUM machine_region;
extern ENUM machine_type;
extern ENUM machine_timing;

extern ENUM cpu_usage;

extern BOOL speed_cap;
extern int frame_skip;
extern int timing_fps;
extern int timing_hertz;
extern int timing_audio_fps;

extern REAL timing_speed_multiplier;
extern BOOL timing_half_speed;

extern unsigned timing_clock;

extern int frames_to_execute;

extern int machine_init (void);
extern void machine_exit (void);
extern void machine_reset (void);

extern void suspend_timing (void);
extern void resume_timing (void);

enum
{
   MACHINE_REGION_AUTOMATIC,
   MACHINE_REGION_NTSC,
   MACHINE_REGION_PAL
};

enum
{
   MACHINE_TYPE_NTSC,
   MACHINE_TYPE_PAL
};

enum
{
   MACHINE_TIMING_SMOOTH,
   MACHINE_TIMING_ACCURATE
};

enum
{
   CPU_USAGE_PASSIVE,
   CPU_USAGE_NORMAL,
   CPU_USAGE_AGGRESSIVE
};

#define SCANLINE_CLOCKS       341
#define RENDER_CLOCKS         256
#define HBLANK_CLOCKS         (SCANLINE_CLOCKS - RENDER_CLOCKS)
#define HBLANK_CLOCKS_BEFORE_VRAM_ADDRESS_FIXUP (320 - 256)
#define TOTAL_LINES_NTSC      262
#define TOTAL_LINES_PAL       312
#define FIRST_DISPLAYED_LINE  0
#define LAST_DISPLAYED_LINE   239
#define FIRST_VBLANK_LINE     240

#define SCANLINE_LENGTH       SCANLINE_CLOCKS

static INLINE REAL timing_get_speed_ratio (void)
{
   REAL ratio;

   /* This gets the speed ratio relative to the normal clock rate, as
      affected by any speed modifiers.

      < 1.0 = slower
        1.0 = normal
      > 1.0 = faster
      */

   ratio = 1.0;

   ratio *= timing_speed_multiplier;

   if (timing_half_speed)
      ratio /= 2.0;

   return (ratio);
}

static INLINE REAL timing_get_speed (void)
{
   REAL scalar;

   scalar = ((machine_type == MACHINE_TYPE_NTSC) ? MACHINE_RATE_NTSC : MACHINE_RATE_PAL);

   switch (machine_timing)
   {
      case MACHINE_TIMING_SMOOTH:
         scalar = floor (scalar);   /* Approximated. */

      case MACHINE_TIMING_ACCURATE:
         break;                     /* Real(unmodified). */

      default:
      {
         WARN_GENERIC();
         break;
      }
   }

   /* Apply any speed modifiers. */
   scalar *= timing_get_speed_ratio ();

   return (scalar);
}

static INLINE void timing_update_speed (void)
{
   if (!gui_is_active)
   {
      /* Cycle timers to match new emulation speeds. */
      suspend_timing ();
      resume_timing ();
   }

   /* Cycle audio to match new emulation speeds. */
   audio_exit ();
   audio_init ();

   if (rom_is_loaded)
      apu_update ();
}

static INLINE void timing_update_machine_type (void)
{
   /* This function resyncs machine_type to the value of machine_region. */

   switch (machine_region)
   {
      case MACHINE_REGION_AUTOMATIC:
      {
         if (rom_is_loaded)
         {
            /* Try to determine a suitable machine type by searching for
               country codes in the ROM's filename. */

            if (ustrstr (global_rom.filename, "(E)"))
            {
               /* Europe. */
               machine_type = MACHINE_TYPE_PAL;
            }
            else
            {
               /* Default to NTSC. */
               machine_type = MACHINE_TYPE_NTSC;
            }
         }
         else  
         {
            /* Default to NTSC. */
            machine_type = MACHINE_TYPE_NTSC;
         }

         break;
      }

      case MACHINE_REGION_NTSC:
      {
         /* NTSC (60 Hz). */
         machine_type = MACHINE_TYPE_NTSC;

         break;
      }

      case MACHINE_REGION_PAL:
      {
         /* PAL (50 Hz). */
         machine_type = MACHINE_TYPE_PAL;

         break;
      }
   }

   timing_update_speed ();
}

static REAL timing_get_frequency (void)
{
   REAL scalar;

   /* Gets the frequency of the CPU core, in Hz.

      This is (or should be, as closely as possible) concurrent with the
      values returned by cpu_get_cycles().

      This is used primarily by the APU. */

   scalar = SCANLINE_CLOCKS;

   if (machine_type == MACHINE_TYPE_NTSC)
      scalar *= TOTAL_LINES_NTSC;
   else
      scalar *= TOTAL_LINES_PAL;

   scalar *= timing_get_speed ();

   return (scalar);
}

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !TIMING_H_INCLUDED */
