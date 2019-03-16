/* FakeNES - A free, portable, Open Source NES emulator.

   audiolib.c: Implementation of the audio library.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#include <stdlib.h>
#include <string.h>
#include "audio.h"
#include "audiolib.h"
#include "common.h"
#include "debug.h"
#include "types.h"

typedef struct _AUDIOLIB_DRIVER
{
   int (*init) (void);
   void (*deinit) (void);
   int (*play) (void);
   void (*stop) (void);
   void *(*get_buffer) (void);
   void (*free_buffer) (void);
   void (*suspend) (void);
   void (*resume) (void);

} AUDIOLIB_DRIVER;

static const AUDIOLIB_DRIVER *audiolib_driver = NULL;
               
static int set_driver (void);

int audiolib_init (void)
{
   DEBUG_PRINTF("audiolib_init()\n");

   return (set_driver ());
}

void audiolib_exit (void)
{
   DEBUG_PRINTF("audiolib_exit()\n");

   if (audiolib_driver)
   {
      if (audiolib_driver->deinit)
      {
         /* Deinitialize driver. */
         audiolib_driver->deinit ();
      }

      audiolib_driver = NULL;
   }
}

int audiolib_play (void)
{
   DEBUG_PRINTF("audiolib_play()\n");

   if (!audiolib_driver)
      return (0);

   if (audiolib_driver->play)
      return (audiolib_driver->play ());
   else
      return (0);
}

void audiolib_stop (void)
{
   DEBUG_PRINTF("audiolib_stop()\n");

   if (!audiolib_driver)
      return;

   if (audiolib_driver->stop)
      audiolib_driver->stop ();
}

void *audiolib_get_buffer (void)
{
   DEBUG_PRINTF("audiolib_get_buffer()\n");

   if (!audiolib_driver)
      return (NULL);

   if (audiolib_driver->get_buffer)
      return (audiolib_driver->get_buffer ());
   else
      return (NULL);
}

void audiolib_free_buffer (void)
{
   DEBUG_PRINTF("audiolib_free_buffer()\n");

   if (!audiolib_driver)
      return;

   if (audiolib_driver->free_buffer)
      audiolib_driver->free_buffer ();
}

void audiolib_suspend (void)
{
   DEBUG_PRINTF("audiolib_suspend()\n");

   if (!audiolib_driver)
      return;

   if (audiolib_driver->suspend)
      audiolib_driver->suspend ();
}

void audiolib_resume (void)
{
   DEBUG_PRINTF("audiolib_resume()\n");

   if (!audiolib_driver)
      return;

   if (audiolib_driver->resume)
      audiolib_driver->resume ();
}

/* --- Allegro functions. --- */

static AUDIOSTREAM *audiolib_allegro_stream = NULL;

static int audiolib_allegro_init (void)
{
   /* Force interpolation. */
   set_mixer_quality (2);

   set_volume_per_voice (0);
        
   if (install_sound (DIGI_AUTODETECT, MIDI_NONE, NULL) != 0)
   {
      WARN_GENERIC();
      return (1);
   }

   if (digi_driver->id == DIGI_NONE)
   {
      WARN_GENERIC();
      return (2);
   }

   /* Autodetect settings. */

   if (audio_sample_rate == -1)
       audio_sample_rate = get_mixer_frequency ();
   if (audio_sample_size == -1)
       audio_sample_size = get_mixer_bits ();

   /* Allegro always uses unsigned samples. */
   audio_unsigned_samples = TRUE;

   /* Return success. */
   return (0);
}

static void audiolib_allegro_deinit (void)
{
   stop_audio_stream (audiolib_allegro_stream);

   /* Remove Allegro sound driver so that it doesn't conflict. */
   remove_sound ();
}

static int audiolib_allegro_play (void)
{
   /* Create stream. */

   if (!(audiolib_allegro_stream = play_audio_stream
      (audio_buffer_size_samples, audio_sample_size, AUDIO_STEREO,
         audio_sample_rate, 255, 128)))
   {
      WARN_GENERIC();
      return (1);
   }

   /* Pause stream. */
   voice_stop (audiolib_allegro_stream->voice);

   /* Return success. */
   return (0);
}

static void audiolib_allegro_stop (void)
{
   stop_audio_stream (audiolib_allegro_stream);
   audiolib_allegro_stream = NULL;
}

static void *audiolib_allegro_get_buffer (void)
{
   return (get_audio_stream_buffer (audiolib_allegro_stream));
}

static void audiolib_allegro_free_buffer (void)
{
   free_audio_stream_buffer (audiolib_allegro_stream);

   /* Play stream if we haven't already. */
   voice_start (audiolib_allegro_stream->voice);
}

static void audiolib_allegro_suspend (void)
{
   voice_stop (audiolib_allegro_stream->voice);
}

static void audiolib_allegro_resume (void)
{
   voice_start (audiolib_allegro_stream->voice);
}

static const AUDIOLIB_DRIVER audiolib_allegro_driver =
{
   audiolib_allegro_init,
   audiolib_allegro_deinit,
   audiolib_allegro_play,
   audiolib_allegro_stop,
   audiolib_allegro_get_buffer,
   audiolib_allegro_free_buffer,
   audiolib_allegro_suspend,
   audiolib_allegro_resume
};

/* --- OpenAL functions. --- */

#ifdef USE_OPENAL
#include <AL/al.h>
#include <AL/alut.h>

/* OpenAL AUDIOSTREAM-like streaming routines. */

typedef struct ALSTREAM
{
   ALuint source;       // The source we are playing on
   int len;             // The length of the sample buffer (in samples)
   int len_bytes;       // The length of the sample buffer (in bytes)
   int bits;            // The bits per sample
   int stereo;          // Mono or stereo
   int freq;            // The sampling frequency, in Hz
   void *buffer;        // The sample buffer
   ALuint alformat;     // Format of sample buffer according to OpenAL
   ALuint *albuffers;   // OpenAL buffers for this stream

} ALSTREAM;

static ALSTREAM *play_al_stream (int, int, int, int);
static void stop_al_stream (ALSTREAM *);
static void *get_al_stream_buffer (ALSTREAM *);
static void free_al_stream_buffer (ALSTREAM *);

#define AL_CHECK() { \
   int error;  \
   if ((error = alGetError ()) != AL_NO_ERROR) {   \
      allegro_message ("OpenAL error #%d at line %d of %s", error,   \
         __LINE__, __FILE__); \
      exit (-1);  \
   }  \
}

#define NUM_BUFFERS  2

static ALSTREAM *play_al_stream (int len, int bits, int stereo, int freq)
{
   ALSTREAM *stream;
   int index;

   stream = malloc (sizeof (ALSTREAM));
   if (!stream)
      return (NULL);

   stream->len = len;
   stream->bits = bits;
   stream->stereo = stereo;
   stream->freq = freq;

   stream->len_bytes = len;
   if (bits == 16)
      stream->len_bytes *= 2;
   if (stereo)
      stream->len_bytes *= 2;

   stream->buffer = malloc (stream->len_bytes);
   if (!stream->buffer)
   {
      free (stream);
      return (NULL);
   }

   memset (stream->buffer, 0, stream->len_bytes);

   stream->albuffers = malloc ((sizeof (ALuint) * NUM_BUFFERS));
   if (!stream->albuffers)
   {
      free (stream->buffer);
      free (stream);
      return (NULL);
   }

   if (stereo)
   {
      if (bits == 16)
         stream->alformat = AL_FORMAT_STEREO16;
      else
         stream->alformat = AL_FORMAT_STEREO8;
   }
   else
   {
      if (bits == 16)
         stream->alformat = AL_FORMAT_MONO16;
      else
         stream->alformat = AL_FORMAT_MONO8;
   }

   alGenBuffers (NUM_BUFFERS, stream->albuffers);
   AL_CHECK();

   alGenSources (1, &stream->source);
   AL_CHECK();

   for (index = 0; index < NUM_BUFFERS; index++)
   {
      int buffer = stream->albuffers[index];

      alBufferData (buffer, stream->alformat, stream->buffer,
         stream->len_bytes, stream->freq);
      AL_CHECK();
   }

   alSourceQueueBuffers (stream->source, NUM_BUFFERS, stream->albuffers);
   AL_CHECK();

   alSourcePlay (stream->source);
   AL_CHECK();

   return (stream);
}

static void stop_al_stream (ALSTREAM *stream)
{
   RT_ASSERT(stream);
   RT_ASSERT(stream->albuffers);
   RT_ASSERT(stream->buffer);

   alSourceStop (stream->source);
   AL_CHECK();

   alDeleteSources (1, &stream->source);
   AL_CHECK();

   alDeleteBuffers (NUM_BUFFERS, stream->albuffers);
   AL_CHECK();

   free (stream->albuffers);
   free (stream->buffer);
   free (stream);
}

static ALuint floating_buffer = 0;

static void *get_al_stream_buffer (ALSTREAM *stream)
{
   int processed;

   RT_ASSERT(stream);
   RT_ASSERT(stream->buffer);

   alGetSourcei (stream->source, AL_BUFFERS_PROCESSED, &processed);
   AL_CHECK();

   if (processed == 0)
      return (NULL);

   alSourceUnqueueBuffers (stream->source, 1, &floating_buffer);
   AL_CHECK();

   return (stream->buffer);
}

static void free_al_stream_buffer (ALSTREAM *stream)
{
   int state;

   RT_ASSERT(stream);
   RT_ASSERT(stream->buffer);

   alBufferData (floating_buffer, stream->alformat, stream->buffer,
      stream->len_bytes, stream->freq);
   AL_CHECK();

   alSourceQueueBuffers (stream->source, 1, &floating_buffer);
   AL_CHECK();

   alGetSourcei (stream->source, AL_SOURCE_STATE, &state);
   AL_CHECK();

   if (state == AL_STOPPED)
   {
      alSourcePlay (stream->source);
      AL_CHECK();
   }
}

static ALSTREAM *audiolib_openal_stream = NULL;

static int audiolib_openal_init (void)
{
   static BOOL initialized = FALSE;

   if (!initialized)
   {
      /* Hack for freealut not liking being initialized/deinitialized more
         than once on Linux and possibly other Unices. */

      alutInit (&saved_argc, saved_argv);
      AL_CHECK();

      /* Install exit handler. */
      atexit (alutExit);

      /* Make sure we don't get initialized again. */
      initialized = TRUE;
   }

   /* Autodetect settings. */

   if (audio_sample_rate == -1)
       audio_sample_rate = 44100;
   if (audio_sample_size == -1)
       audio_sample_size = 16;

   if (audio_sample_size == 8)
      audio_unsigned_samples = TRUE;
   else
      audio_unsigned_samples = FALSE;

   /* Return success. */
   return (0);
}

static void audiolib_openal_deinit (void)
{
   stop_al_stream (audiolib_openal_stream);

   /* Due to a bug in freealut, we don't call it's exit routine until we're
      actually exiting the program (via atexit()). */
   /* alutExit (); */
}

static int audiolib_openal_play (void)
{
   /* Create stream. */

   if (!(audiolib_openal_stream = play_al_stream (audio_buffer_size_samples,
      audio_sample_size, AUDIO_STEREO, audio_sample_rate)))
   {
      WARN_GENERIC();
      return (1);
   }

   /* Return success. */
   return (0);
}

static void audiolib_openal_stop (void)
{
   stop_al_stream (audiolib_openal_stream);
   audiolib_openal_stream = NULL;
}

static void *audiolib_openal_get_buffer (void)
{
   return (get_al_stream_buffer (audiolib_openal_stream));
}

static void audiolib_openal_free_buffer (void)
{
   free_al_stream_buffer (audiolib_openal_stream);
}

static void audiolib_openal_suspend (void)
{
   alSourceStop (audiolib_openal_stream->source);
   AL_CHECK ();
}

static void audiolib_openal_resume (void)
{
   alSourcePlay (audiolib_openal_stream->source);
   AL_CHECK ();
}

static AUDIOLIB_DRIVER audiolib_openal_driver =
{
   audiolib_openal_init,
   audiolib_openal_deinit,
   audiolib_openal_play,
   audiolib_openal_stop,
   audiolib_openal_get_buffer,
   audiolib_openal_free_buffer,
   audiolib_openal_suspend,
   audiolib_openal_resume
};

#endif   /* USE_OPENAL */

/* --- Internal functions. --- */

static int set_driver (void)
{
   switch (audio_subsystem)
   {
      case AUDIO_SUBSYSTEM_NONE:
      {
         audiolib_driver = NULL;

         break;
      }

      case AUDIO_SUBSYSTEM_ALLEGRO:
      {
         audiolib_driver = &audiolib_allegro_driver;

         break;
      }

#ifdef USE_OPENAL

      case AUDIO_SUBSYSTEM_OPENAL:
      {
         audiolib_driver = &audiolib_openal_driver;

         break;
      }

#endif   /* USE_OPENAL */

      default:
      {
         WARN_GENERIC();
         audiolib_driver = NULL;
         return (1);
      }
   }

   if (audiolib_driver)
   {
      if (audiolib_driver->init)
      {
         /* Initialize driver. */
         return (audiolib_driver->init ());
      }
   }

   return (0);
}
