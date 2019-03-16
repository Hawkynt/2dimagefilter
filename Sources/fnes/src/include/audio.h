/* FakeNES - A free, portable, Open Source NES emulator.

   audio.h: Declarations for the audio interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef AUDIO_H_INCLUDED
#define AUDIO_H_INCLUDED
#include "apu.h"
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

extern BOOL audio_enable_output;
extern ENUM audio_subsystem;
extern int audio_sample_rate;
extern int audio_sample_size;
extern BOOL audio_unsigned_samples;
extern int audio_buffer_length;

extern int audio_buffer_frame_size_samples;
extern unsigned audio_buffer_frame_size_bytes;
extern int audio_buffer_size_samples;
extern unsigned audio_buffer_size_bytes;
extern volatile int audio_fps;

extern int audio_init (void);
extern void audio_exit (void);
extern void audio_update (void);
extern void audio_suspend (void);
extern void audio_resume (void);
extern void *audio_get_buffer (void);
extern void audio_free_buffer (void);

/* Helper macros. */
#define AUDIO_STEREO    apu_options.stereo
#define AUDIO_CHANNELS  (AUDIO_STEREO ? 2 : 1)

/* Subsystems. */
enum
{
   AUDIO_SUBSYSTEM_NONE,
   AUDIO_SUBSYSTEM_ALLEGRO,
   AUDIO_SUBSYSTEM_OPENAL
};

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !AUDIO_H_INCLUDED */
