/* Nofrendo (c) 1998-2000 Matthew Conte (matt@conte.com)

   This program is free software; you can redistribute it and/or
   modify it under the terms of version 2 of the GNU Library General 
   Public License as published by the Free Software Foundation.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
   Library General Public License for more details.  To obtain a 
   copy of the GNU Library General Public License, write to the Free 
   Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

   Any permitted reproduction of these routines, in whole or in part,
   must bear this legend.

   Heavily modified for FakeNES by randilyn.
   Portions (c) 2001-2006 FakeNES Team. */

#ifndef APU_H_INCLUDED
#define APU_H_INCLUDED
#include <allegro.h>
#include "common.h"
#include "core.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

#define APU_REGS   24

/* Maximum number of channels to send to the DSP (mono = 1, stereo = 2). */
#define APU_MIXER_MAX_CHANNELS   2

typedef struct apu_envelope_s
{
   UINT8 timer;               /* save */
   UINT8 period;              /* do not save */
   UINT8 counter;             /* save */
   BOOL fixed;                /* do not save */
   UINT8 fixed_volume;        /* do not save */
   BOOL dirty;                /* save */

} apu_envelope_t;

typedef struct apu_sweep_s
{
   BOOL enabled;              /* do not save */
   UINT8 timer;               /* save */
   UINT8 period;              /* do not save */
   UINT8 shifts;              /* do not save */
   BOOL invert;               /* do not save */
   BOOL increment;            /* do not save */
   BOOL dirty;                /* save */
             
} apu_sweep_t;

typedef struct apu_chan_s
{
   /* General. */
   UINT8 output;                 /* save */
   UINT8 volume;                 /* save for squares, noise, and dmc */
   BOOL looping;                 /* do not save */
   BOOL silence;                 /* save for squares, noise, and dmc */

   /* Timer. */
   INT16 timer;                  /* save */
   UINT16 period;                /* save for squares */

   /* Length counter (all except dmc). */
   UINT8 length;                 /* save */
   BOOL length_disable;          /* do not save */
   
   /* Envelope generator (square/noise). */
   apu_envelope_t envelope;

   /* Sweep unit (squares). */
   apu_sweep_t sweep;

   /* Sequencer (squares/triangle). */
   UINT8 sequence_step;          /* save */

   /* Linear counter (triangle). */
   UINT8 linear_length;          /* save */
   BOOL halt_counter;            /* save */
   UINT8 cached_linear_length;   /* do not save */

   /* Square. */
   UINT8 duty_cycle;            /* do not save */

   /* Noise. */
   UINT16 xor_tap;               /* do not save */
   UINT16 shift16;               /* save */

   /* DMC. */
   BOOL enabled;                 /* save */
   UINT16 address;               /* save */
   UINT16 dma_length;            /* save */
   UINT8 cur_byte;               /* save */
   UINT8 sample_bits;            /* save */
   UINT8 counter;                /* save */
   UINT8 shift_reg;              /* save */
   BOOL irq_gen;                 /* do not save */
   BOOL irq_occurred;            /* save */
   UINT16 cached_address;        /* do not save */
   UINT16 cached_dmalength;      /* do not save */

} apu_chan_t;

typedef struct apu_s
{
   apu_chan_t square[2];
   apu_chan_t triangle;
   apu_chan_t noise;
   apu_chan_t dmc;

   /* Delta value for timers. */
   cpu_time_t timer_delta;

   /* Mixer. */
   struct
   {            
      BOOL can_process;
      int channels;
      cpu_time_t delta_cycles;
      REAL inputs[APU_MIXER_MAX_CHANNELS];
      REAL accumulators[APU_MIXER_MAX_CHANNELS];
      REAL sample_cache[APU_MIXER_MAX_CHANNELS];
      REAL filter[APU_MIXER_MAX_CHANNELS];
      REAL accumulated_samples;
      REAL max_samples;

   } mixer;

   /* State. */
   UINT8 regs[APU_REGS];            /* save */

   /* Timestamp of the last call to process(). */
   cpu_time_t clock_counter;

   /* Frame sequencer & frame IRQs. */
   INT16 sequence_counter;          /* save */
   UINT8 sequence_step;             /* save */
   UINT8 sequence_steps;            /* do not save */
   BOOL frame_irq_gen;              /* do not save */
   BOOL frame_irq_occurred;         /* save */

   /* IRQ prediction. */
   cpu_time_t prediction_timestamp; /* save */
   cpu_time_t prediction_cycles;    /* save */

} apu_t;

/* Function prototypes */
extern void apu_load_config (void);
extern void apu_save_config (void);
extern int apu_init (void);
extern void apu_exit (void);
extern void apu_reset (void);
extern void apu_update (void);
extern void apu_start_frame (void);
extern void apu_end_frame (void);
extern void apu_set_exsound (ENUM type);
extern UINT8 apu_read (UINT16 address);
extern void apu_write (UINT16 address, UINT8 value);
extern void apu_predict_irqs (cpu_time_t cycles);
extern void apu_save_state (PACKFILE *file, int version);
extern void apu_load_state (PACKFILE *file, int version);

typedef struct apu_options_s
{
   BOOL enabled;
   ENUM emulation;
   BOOL stereo;

   /* Channels. */
   BOOL enable_square_1;
   BOOL enable_square_2;
   BOOL enable_triangle;
   BOOL enable_noise;
   BOOL enable_dmc;
   BOOL enable_extra_1;
   BOOL enable_extra_2;
   BOOL enable_extra_3;

} apu_options_t;

extern apu_options_t apu_options;

enum
{
   APU_EMULATION_FAST = 0,
   APU_EMULATION_ACCURATE,
   APU_EMULATION_ULTRA
};

enum
{
   APU_EXSOUND_NONE = 0,
   APU_EXSOUND_MMC5,
   APU_EXSOUND_VRC6
};

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !APU_H_INCLUDED */
                          
