/* FakeNES - A free, portable, Open Source NES emulator.

   dsp.h: Declarations for the digital sound processor.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef DSP_H_INCLUDED
#define DSP_H_INCLUDED
#include "apu.h"
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

#define DSP_MAX_CHANNELS   APU_MIXER_MAX_CHANNELS

typedef REAL DSP_SAMPLE;

extern REAL dsp_master_volume;

extern int dsp_init (void);
extern void dsp_exit (void);
extern int dsp_open (unsigned, int);
extern void dsp_close (void);
extern void dsp_start (void);
extern void dsp_write (const DSP_SAMPLE *, unsigned);
extern void dsp_end (void);
extern unsigned dsp_get_buffer_size (void);
extern void dsp_set_channel_enabled (int, ENUM, BOOL);
extern BOOL dsp_get_channel_enabled (int);
extern void dsp_enable_channel (int);
extern void dsp_disable_channel (int);
extern void dsp_set_channel_params (int, REAL, REAL);
extern void dsp_set_effector_enabled (FLAGS, ENUM, BOOL);
extern BOOL dsp_get_effector_enabled (FLAGS);
extern void dsp_render (void *, int, int, BOOL);
extern int dsp_open_wav (const UCHAR *, int, int, int);
extern void dsp_close_wav (void);

enum
{
   DSP_SET_ENABLED_MODE_SET,
   DSP_SET_ENABLED_MODE_INVERT
};

enum
{
   DSP_EFFECTOR_SWAP_CHANNELS = (1 << 1),
};

/* Helper macros. */

#define DSP_ENABLE_CHANNEL_EX(channel, enable) \
   dsp_set_channel_enabled (channel, DSP_SET_ENABLED_MODE_SET, enable)

#define DSP_ENABLE_EFFECTOR(effector) \
   dsp_set_effector_enabled (effector, DSP_SET_ENABLED_MODE_SET, TRUE)
#define DSP_DISABLE_EFFECTOR(effector) \
   dsp_set_effector_enabled (effector, DSP_SET_ENABLED_MODE_SET, FALSE)
#define DSP_TOGGLE_EFFECTOR(effector) \
   dsp_set_effector_enabled (effector, DSP_SET_ENABLED_MODE_INVERT, 0)

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !DSP_H_INCLUDED */
