/* FakeNES - A free, portable, Open Source NES emulator.

   rewind.c: Implementation of the the game rewinder.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#include <stdlib.h>
#include <string.h>
#include "common.h"
#include "debug.h"
#include "rewind.h"
#include "save.h"
#include "shared/bufferfile.h"
#include "timing.h"
#include "types.h"
#ifdef USE_ZLIB
#include <zlib.h>
#endif

/* This value must be large enough to accomodate *all* save data in it's
   unpacked state after being pulled from the rewinder queue.

   Note that save data for some mappers (such as MMC5) can get quite large.

   Recommended 32 kB minimum
   */
#define MAX_UNPACK_SIZE 32768

/* Whether or not real-time game rewinding is enabled.  Disabling this sweet
   feature can give a significant speed boost and reduced memory usage.

   Default: TRUE
   */
static BOOL enabled = TRUE;

/* Frame rate at which snapshots are saved/loaded.  Larger values mean
   smoother backtracking at the cost of memory and a more constant speed it.

   This should be a fractional value that represents a normalized percentage
   of the current emulation speed (e.g, 0.5 of 50 Hz would be 25 FPS).

   Default: 0.5
   Minimum: Value of EPSILON
   Maximum: 1.0
   */
static REAL frame_rate = 0.5;

/* How long the rewinder can backtrack, in seconds.  This is combined with
   'frame_rate' to form the final value of 'max_queue_size'.

   Default: 10
   Minimum: 1
   Maximum: NONE
   */
static int seconds = 10;

/* Compression level (when available).

   Default: 1
   Minimum: 0 (disabled)
   Maximum: 9
   */
static int compression_level = 1;

/* Maximum number of enries in the queue, computed from 'seconds'. */
static int max_queue_size = 0;

/* Frame counter, used to enforce the frame rate. */
static int wait_frames = 0;

/* Queue. */

typedef struct _QUEUE_FRAME
{
   void *data;
   unsigned data_size;
   struct _QUEUE_FRAME *prev;
   struct _QUEUE_FRAME *next;

} QUEUE_FRAME;

typedef struct _QUEUE
{
   QUEUE_FRAME *first;
   QUEUE_FRAME *last;
   int size;

} QUEUE;

static QUEUE queue;

/* Queue routines (defined later). */
static BOOL pack (void *, long *);
static BOOL unpack (void *, long *, void *, long);
static BOOL enqueue (QUEUE_FRAME *);
static QUEUE_FRAME *unenqueue (void);
static QUEUE_FRAME *dequeue (void);

int rewind_init (void)
{
   /* Load configuration. */

   enabled           = get_config_int   ("rewind", "enabled",    enabled);
   frame_rate        = get_config_float ("rewind", "frame_rate", frame_rate);
   seconds           = get_config_int   ("rewind", "seconds",    seconds);
   compression_level = get_config_int   ("rewind", "compress",   compression_level);

   /* Enforce sane limits. */

   if (frame_rate < EPSILON)
      frame_rate = EPSILON;
   if (frame_rate > 1.0f)
      frame_rate = 1.0f;

   if (seconds < 1)
      seconds = 1;

   if (compression_level < 0)
      compression_level = 0;
   if (compression_level > 9)
      compression_level = 9;

   /* Calculate rough maximum queue size.
   
      TODO: Add the necessary code here (and elsewhere) to make this
      automatically adjust to the current emulation speed.  As-is, it
      produces a queue about 20% larger than necessary for PAL.
      */
   max_queue_size = ROUND(((60.0f * frame_rate) * seconds));

   /* Clear everything. */
   rewind_clear ();

   /* Return success. */
   return (0);
}

void rewind_exit (void)
{
   /* Clear everything. */
   rewind_clear ();

   /* Save configuration. */

   set_config_int   ("rewind", "enabled",    enabled);
   set_config_float ("rewind", "frame_rate", frame_rate);
   set_config_int   ("rewind", "seconds",    seconds);

#ifdef USE_ZLIB

   set_config_int ("rewind", "compress", compression_level);

#endif
}

void rewind_clear (void)
{
   QUEUE_FRAME *frame;

   if (!enabled)
      return;

   /* Clear queue. */

   /* Get first frame in queue. */
   frame = queue.first;

   while (frame)
   {
      QUEUE_FRAME *next = frame->next;

      if (frame->data)
      {
         /* Destroy frame data buffer. */
         free (frame->data);
      }

      /* Destroy frame. */
      free (frame);

      /* Advance to the next frame. */
      frame = next;
   }

   queue.first = NULL;
   queue.last  = NULL;
   queue.size  = 0;

   /* Clear frame counter. */
   wait_frames = 0;
}

BOOL rewind_save_snapshot (void)
{
   QUEUE_FRAME *frame;
   PACKFILE *file;
   UINT8 *buffer;
   long size;
   REAL speed;

   if (!enabled)
      return (FALSE);

   if (wait_frames > 0)
      wait_frames--;
   if (wait_frames > 0)
      return (FALSE);

   if (queue.size >= max_queue_size)
   {
      /* The queue is currently full - dequeue and destroy the oldest frame
         to make room for the new one. */
      frame = dequeue ();
      if (!frame)
      {
         WARN_GENERIC();
         return (FALSE);
      }

      if (frame->data)
      {
         /* Destroy frame data buffer. */
         free (frame->data);
      }

      /* Clear frame for reuse. */
      /* memset (frame, 0, sizeof (QUEUE_FRAME)); */
   }
   else
   {
      /* Allocate a new frame. */
      frame = malloc (sizeof (QUEUE_FRAME));
      if (!frame)
      {
         WARN_GENERIC();
         return (FALSE);
      }

      /* Clear frame. */
      /* memset (frame, 0, sizeof (QUEUE_FRAME)); */
   }

   /* Open buffer file. */
   file = BufferFile_open ();
   if (!file)
   {
      WARN_GENERIC();
      free (frame);
      return (FALSE);
   }

   /* Save snapshot. */
   if (!save_state_raw (file))
   {
      WARN_GENERIC();
      pack_fclose (file);
      free (frame);
      return (FALSE);
   }

   /* Get buffer. */
   BufferFile_get_buffer (file, &buffer, &size);

   /* Compress data. */
   if (!pack (buffer, &size))
   {
      WARN_GENERIC();
      pack_fclose (file);
      free (frame);
      return (FALSE);
   }

   /* Allocate frame data buffer. */
   frame->data = malloc (size);
   if (!frame->data)
   {
      WARN_GENERIC();
      pack_fclose (file);
      free (frame);
      return (FALSE);
   }

   /* Copy data to frame data buffer. */
   memcpy (frame->data, buffer, size);

   /* Set data size. */
   frame->data_size = size;

   /* Close buffer file. */
   pack_fclose (file);

   /* Enqueue frame. */
   if (!enqueue (frame))
   {
      /* Enqueue failed. */
      WARN_GENERIC();
      free (frame->data);
      free (frame);
      return (FALSE);
   }

   /* Set frame counter. */

   speed = timing_get_speed ();

   wait_frames = ROUND((speed / (speed * frame_rate)));

   /* Return success. */
   return (TRUE);
}

BOOL rewind_load_snapshot (void)
{
   QUEUE_FRAME *frame;
   PACKFILE *file;
   UINT8 *buffer;
   long size;
   REAL speed;

   if (!enabled)
      return (FALSE);

   if (wait_frames > 0)
      wait_frames--;
   if (wait_frames > 0)
      return (FALSE);

   if (queue.size <= 0)
   {
      /* Queue is empty. */
      WARN_GENERIC();
      return (FALSE);
   }

   /* Fetch most recent frame. */
   frame = unenqueue ();
   if (!frame)
   {
      WARN_GENERIC();
      return (FALSE);
   }

   if (!frame->data)
   {
      /* This shouldn't have been allowed to slip through. */
      WARN_GENERIC();
      free (frame);
      return (FALSE);
   }

   /* Allocate buffer. */
   buffer = malloc (MAX_UNPACK_SIZE);
   if (!buffer)
   {
      WARN_GENERIC();
      free (frame);
      return (FALSE);
   }

   size = MAX_UNPACK_SIZE;

   /* Uncompress data. */
   if (!unpack (buffer, &size, frame->data, frame->data_size))
   {
      WARN_GENERIC();
      free (buffer);
      free (frame->data);
      free (frame);
      return (FALSE);
   }

   /* Open buffer file. */
   file = BufferFile_open ();
   if (!file)
   {
      WARN_GENERIC();
      free (buffer);
      free (frame->data);
      free (frame);
      return (FALSE);
   }

   /* Copy buffer contents to buffer file. */
   if (pack_fwrite (buffer, size, file) < size)
   {
      WARN_GENERIC();
      pack_fclose (file);
      free (buffer);
      free (frame->data);
      free (frame);
      return (FALSE);
   }

   /* Destroy buffer. */
   free (buffer);

   /* Seek back to the beginning. */
   pack_fseek (file, 0);

   /* Load snapshot. */
   if (!load_state_raw (file))
   {
      WARN_GENERIC();
      pack_fclose (file);
      free (frame->data);
      free (frame);
      return (FALSE);
   }

   /* Close buffer file. */
   pack_fclose (file);

   /* Destroy frame data buffer. */
   free (frame->data);

   /* Destroy frame. */
   free (frame);

   /* Set frame counter. */

   speed = timing_get_speed ();

   wait_frames = ROUND((speed / (speed * frame_rate)));

   /* Return success. */
   return (TRUE);
}

BOOL rewind_is_enabled (void)
{
   return (enabled);
}

/* --- Internal functions. --- */

static BOOL pack (void *buffer, long *size)
{
   /* In-place Deflate compression. */

   void *packbuf;
   long packsize;

   RT_ASSERT(buffer);
   RT_ASSERT(size);

#ifdef USE_ZLIB

   if (compression_level == 0)
   {
      /* Nothing to do. */
      return (TRUE);
   }

   /* We add 16 bytes of slack just in case. */
   packsize = (((*size * 1.01) + 12) + 16);

   packbuf = malloc (packsize);
   if (!packbuf)
      return (FALSE);

   if (compress2 (packbuf, &packsize, buffer, *size, compression_level) !=
      Z_OK)
   {
      WARN_GENERIC();
      free (packbuf);
      return (FALSE);
   }

   memcpy (buffer, packbuf, packsize);
   *size = packsize;
   
   free (packbuf);

#endif   /* USE_ZLIB */

   return (TRUE);
}

static BOOL unpack (void *outbuf, long *max, void *buffer, long size)
{
   /* Same as above, but decompression instead.  If zlib is not available,
      this performs a simple buffer copy instead.

      outbuf - The output buffer.
      max    - The maximum size of the output buffer (will be set to it's
               actual length after decompression).
      buffer - The input buffer.
      size   - The size of the data in the input buffer. */

   RT_ASSERT(outbuf);
   RT_ASSERT(max);
   RT_ASSERT(buffer);

#ifdef USE_ZLIB

   if (compression_level == 0)
   {
      /* Perform a normal buffer copy. */

      memcpy (outbuf, buffer, size);
      *max = size;

      return (TRUE);
   }

   if (uncompress (outbuf, max, buffer, size) != Z_OK)
   {
      WARN_GENERIC();
      return (FALSE);
   }

#else /* USE_ZLIB */

   /* Perform a normal buffer copy. */

   memcpy (outbuf, buffer, size);
   *max = size;

#endif   /* !USE_ZLIB */

   return (TRUE);
}

static BOOL enqueue (QUEUE_FRAME *frame)
{
   QUEUE_FRAME *last;

   if (queue.size >= max_queue_size)
   {
      WARN_GENERIC();
      return (FALSE);
   }

   /* Get last frame in queue. */
   last = queue.last;

   if (last)
   {
      /* Add frame to queue. */
      queue.last = frame;
   
      /* Set up links. */
      last->next = frame;
      frame->prev = last;
      frame->next = NULL;
   }
   else
   {
      /* Queue is empty, add this as the first and last frame. */
      queue.first = frame;
      queue.last  = frame;

      /* Clear links. */
      frame->prev = NULL;
      frame->next = NULL;
   }

   /* Increment counter. */
   queue.size++;

   /* Return success. */
   return (TRUE);
}

static QUEUE_FRAME *unenqueue (void)
{
   QUEUE_FRAME *frame;

   if (queue.size <= 0)
   {
      /* Queue is empty. */
      WARN_GENERIC();
      return (NULL);
   }

   /* Get last frame in queue. */
   frame = queue.last;

   /* Remove frame from queue. */
   queue.last = frame->prev;

   /* Clear links. */

   if (queue.last)
      queue.last->next = NULL;

   frame->prev = NULL;
   frame->next = NULL;

   /* Decrement counter. */
   queue.size--;

   if (queue.size <= 0)
   {
      /* Queue is empty again - clear stale root pointer. */
      queue.first = NULL;
   }

   return (frame);
}

static QUEUE_FRAME *dequeue (void)
{
   QUEUE_FRAME *frame;

   if (queue.size <= 0)
   {
      /* Queue is empty. */
      WARN_GENERIC();
      return (NULL);
   }

   /* Get first frame in queue. */
   frame = queue.first;

   /* Remove frame from queue. */
   queue.first = frame->next;

   /* Clear links. */

   if (queue.first)
      queue.first->prev = NULL;

   frame->prev = NULL;
   frame->next = NULL;

   /* Decrement counter. */
   queue.size--;

   return (frame);
}
