/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   crc32.cpp: CRC32 calculation routines by TRAC. */

#include "../include/common.h"
#include "crc32.h"

static bool initialized = false;

static linear void init (void);
static linear uint32 start (void);
static linear void end (uint32 &crc32);
static void update (uint32 &crc32, uint8 value);

uint32 make_crc32 (const uint8 *buffer, unsigned size)
{
   RT_ASSERT(buffer);

   if (!initialized)
      init ();

   uint32 crc32 = start ();

   for (unsigned offset = 0; offset < size; offset++)
      update (crc32, buffer[offset]);

   end (crc32);

   return (crc32);
}

#define TABLE_SIZE   256

static uint32 table[TABLE_SIZE];
static const uint32 seed = 0xFFFFFFFF;

static linear void init (void)
{
   for (int index = 0; index < TABLE_SIZE; index++)
   {
      uint32 value = index;

      for (int bit = 0; bit < 8; bit++)
      {
         if (value & 1)
            value = ((value >> 1) ^ 0xEDB88320);
         else
            value >>= 1;
      }

      table[index] = value;
   }

   initialized = true;
}

static linear uint32 start (void)
{
   return (seed);
}

static linear void end (uint32 &crc32)
{
   crc32 ^= seed;
}

static void update (uint32 &crc32, uint8 value)
{           
   crc32 = (table[((crc32 ^ value) & 0xFF)] ^ ((crc32 >> 8) & 0x00FFFFFF));
}
