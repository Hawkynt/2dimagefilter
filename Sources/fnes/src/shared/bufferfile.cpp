/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   bufferfile.cpp: "Buffer file" I/O routines by randilyn. */

#include <allegro.h>        
#include <vector>
#include "../include/common.h"
#include "bufferfile.h"

class BufferFile {
public:
   std::vector<uint8> data;
   long pos, size, max;
};

#define CHUNK_SIZE   4096
                     
static const PACKFILE_VTABLE *get_packfile_vtable (void);

PACKFILE *BufferFile_open (void)
{
   BufferFile *bufferFile = new BufferFile;
   if (!bufferFile)
   {
      WARN("Out of memory");
      return (null);

   }

   bufferFile->pos = 0;
   bufferFile->size = 0;
   bufferFile->max = 0;

   PACKFILE *pf = pack_fopen_vtable (get_packfile_vtable (), bufferFile);
   if (!pf)
   {
      WARN("Call to pack_fopen_vtable() failed");
      delete bufferFile;
      return (null);
   }

   return (pf);
}

void BufferFile_get_buffer (PACKFILE *pf, uint8 **buffer, long *size)
{
   RT_ASSERT(pf);
   RT_ASSERT(buffer);
   RT_ASSERT(size);

   BufferFile *bufferFile = (BufferFile *)pf->userdata;

   *buffer = &bufferFile->data[0];
   *size = bufferFile->size;
}

// Internal functions follow.

static int BufferFile_fclose (void *userdata)
{
   RT_ASSERT(userdata);

   BufferFile *bufferFile = (BufferFile *)userdata;

   // Clean up.
   bufferFile->data.clear ();

   delete bufferFile;

   return (0);
}

static int BufferFile_getc (void *userdata)
{
   RT_ASSERT(userdata);

   BufferFile *bufferFile = (BufferFile *)userdata;

   if (bufferFile->pos >= bufferFile->size)
   {
      // EOF
      return (0);
   }

   return (bufferFile->data[bufferFile->pos++]);
}                  

static int BufferFile_ungetc (int c, void *userdata)
{
   RT_ASSERT(userdata);

   // Not supported.
   return (EOF);
}

static long BufferFile_fread (void *p, long n, void *userdata)
{
   RT_ASSERT(p);
   RT_ASSERT(userdata);

   BufferFile *bufferFile = (BufferFile *)userdata;

   if (bufferFile->pos >= bufferFile->size)
   {
      // EOF
      return (0);
   }

   long size = (bufferFile->size - bufferFile->pos);
   if (n > size)
      n = size;

   memcpy (p, (&bufferFile->data[0] + bufferFile->pos), n);
   bufferFile->pos += n;

   return (n);
}

static int BufferFile_putc (int c, void *userdata)
{
   RT_ASSERT(userdata);

   BufferFile *bufferFile = (BufferFile *)userdata;

   if (bufferFile->pos >= bufferFile->size)
   {
      // EOF

      if (bufferFile->pos >= (bufferFile->size + 1))
      {
         // Out of bounds
         return (0);
      }

      bufferFile->size++;
      if (bufferFile->size > bufferFile->max)
      {
         bufferFile->max = (bufferFile->size + CHUNK_SIZE);
         bufferFile->data.reserve (bufferFile->max);
      }

      bufferFile->data.resize (bufferFile->size);
   }

   bufferFile->data[bufferFile->pos++] = c;

   return (c);
}
                                                               
static long BufferFile_fwrite (const void *p, long n, void *userdata)
{
   RT_ASSERT(p);
   RT_ASSERT(userdata);

   BufferFile *bufferFile = (BufferFile *)userdata;

   if (bufferFile->pos >= bufferFile->size)
   {
      if (bufferFile->pos >= (bufferFile->size + 1))
         return (0);

      bufferFile->size += n;
      if (bufferFile->size > bufferFile->max)
      {
         bufferFile->max = (bufferFile->size + CHUNK_SIZE);
         bufferFile->data.reserve (bufferFile->max);
      }                               

      bufferFile->data.resize (bufferFile->size);
   }

   memcpy ((&bufferFile->data[0] + bufferFile->pos), p, n);
   bufferFile->pos += n;

   return (n);
}

static int BufferFile_fseek (void *userdata, int offset)
{
   RT_ASSERT(userdata);

   BufferFile *bufferFile = (BufferFile *)userdata;

   if ((offset < 0) || (offset > bufferFile->size))
   {
      // Out of bounds.
      return (-1);
   }

   bufferFile->pos = offset;

   return (0);
}

static int BufferFile_feof (void *userdata)
{
   RT_ASSERT(userdata);

   BufferFile *bufferFile = (BufferFile *)userdata;

   if (bufferFile->pos >= bufferFile->size)
      return (true);

   return (false);
}

static int BufferFile_ferror (void *userdata)
{
   RT_ASSERT(userdata);

   // Not supported.
   return (0);
}

static PACKFILE_VTABLE packfile_vtable;

static const PACKFILE_VTABLE *get_packfile_vtable (void)
{
   PACKFILE_VTABLE *vtable = &packfile_vtable;

   memset (vtable, 0, sizeof(PACKFILE_VTABLE));

   vtable->pf_fclose = BufferFile_fclose;
   vtable->pf_getc   = BufferFile_getc;
   vtable->pf_ungetc = BufferFile_ungetc;
   vtable->pf_fread  = BufferFile_fread;
   vtable->pf_putc   = BufferFile_putc;
   vtable->pf_fwrite = BufferFile_fwrite;
   vtable->pf_fseek  = BufferFile_fseek;
   vtable->pf_feof   = BufferFile_feof;
   vtable->pf_ferror = BufferFile_ferror;

   return (vtable);
}
