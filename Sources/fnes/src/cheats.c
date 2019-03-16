/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   cheats.c: Decoder routines for cheat devices. */

#include <ctype.h>
#include <stdio.h>
#include <string.h>
#include "cheats.h"
#include "common.h"
#include "cpu.h"
#include "types.h"

static INLINE int decode_raw (const UINT8 *code, UINT16 *address, UINT8
   *value, UINT8 *match_value)
{
   /* NESticle RAW 7 or 10 digit code. */

   int decoded_address, decoded_value, decoded_match_value;

   if (sscanf (code, "%04x?%02x:%02x", &decoded_address,
      &decoded_match_value, &decoded_value) < 3)
   {
      if (sscanf (code, "%04x:%02x", &decoded_address, &decoded_value) < 2)
         return (1);

      decoded_match_value = cpu_read (decoded_address);
   }

   *address     = decoded_address;
   *value       = decoded_value;
   *match_value = decoded_match_value;

   return (0);
}

/* Descrambles game-genie codes.  Note that codes are passed in and out both
   as 32-bit integers - the needed fields are simply coalesced by this
   function.
   */
#define GG_NYBBLE_HIGH_BITS   \
   ((1 << 31)  \
   | (1 << 27) \
   | (1 << 23) \
   | (1 << 19) \
   | (1 << 15) \
   | (1 << 11) \
   | (1 << 7)  \
   | (1 << 3))

#define GG_EVEN_NYBBLES \
   ((15 << 24) \
   | (15 << 16)   \
   | (15 << 8) \
   | (15))

#define GG_ODD_NYBBLES  (GG_EVEN_NYBBLES << 4)

#define GG_SWAP_ADDRESS_NYBBLE_1 (15 << 12)
#define GG_SWAP_ADDRESS_NYBBLE_2 (15 << 16)

#define GG_NOT_ADDRESS_SWAP   \
   (~(GG_SWAP_ADDRESS_NYBBLE_1 | GG_SWAP_ADDRESS_NYBBLE_2))

#define GG_CODE_SIZE_BIT            23
#define GG_SMALL_CODE_VALUE_BIT_3   3
#define GG_LARGE_CODE_VALUE_BIT_3   27

static INLINE UINT32 gg_descramble (UINT32 scrambled)
{
   UINT32 value, compare, address;

   /* Rotate nybble MSBs right one */
   scrambled = (scrambled & ~GG_NYBBLE_HIGH_BITS) | ((scrambled &
      GG_NYBBLE_HIGH_BITS) >> 4) | ((scrambled & (1 << 3)) << 28);

   /* Swap even and odd nybbles */
   scrambled = ((scrambled & GG_EVEN_NYBBLES) << 4) | ((scrambled &
      GG_ODD_NYBBLES) >> 4);

   /* Swap misplaced address nybbles */
   scrambled = (scrambled & GG_NOT_ADDRESS_SWAP) | ((scrambled &
      GG_SWAP_ADDRESS_NYBBLE_1) << 4) | ((scrambled &
         GG_SWAP_ADDRESS_NYBBLE_2) >> 4);

   if (!(scrambled & (1 << GG_CODE_SIZE_BIT)))
   {
      scrambled = (scrambled & ~(1 << GG_LARGE_CODE_VALUE_BIT_3) & ~(1 <<
         GG_SMALL_CODE_VALUE_BIT_3)) | ((scrambled & (1 <<
            GG_SMALL_CODE_VALUE_BIT_3)) << (GG_LARGE_CODE_VALUE_BIT_3 -
               GG_SMALL_CODE_VALUE_BIT_3));
   }

   return (scrambled);
}

#define GG_MAP_DIGIT(digit, value)  \
   case digit: \
      return (value);

static int gg_decode_digit (UINT8 digit)
{
   digit = toupper (digit);

   switch (digit)
   {
      /* First set. */
      GG_MAP_DIGIT('A', 0x0);
      GG_MAP_DIGIT('P', 0x1);
      GG_MAP_DIGIT('Z', 0x2);
      GG_MAP_DIGIT('L', 0x3);
      GG_MAP_DIGIT('G', 0x4);
      GG_MAP_DIGIT('I', 0x5);
      GG_MAP_DIGIT('T', 0x6);
      GG_MAP_DIGIT('Y', 0x7);

      /* Second set. */
      GG_MAP_DIGIT('E', 0x8);
      GG_MAP_DIGIT('O', 0x9);
      GG_MAP_DIGIT('X', 0xA);
      GG_MAP_DIGIT('U', 0xB);
      GG_MAP_DIGIT('K', 0xC);
      GG_MAP_DIGIT('S', 0xD);
      GG_MAP_DIGIT('V', 0xE);
      GG_MAP_DIGIT('N', 0xF);

      default:
         break;
   }

   return (0);
}

static INLINE int decode_game_genie (const UINT8 *code, UINT16 *address,
   UINT8 *value, UINT8 *match_value)
{
   /* NES Game Genie 6 or 8 digit codes. */

   int length;
   int shifts;
   int index;
   UINT32 decoded = 0;

   length = strlen (code);
   if ((length != 6) && (length != 8))
      return (1);

   shifts = 28;

   for (index = 0; index < length; index++)
   {
      decoded |= (gg_decode_digit (code[index]) << shifts);

      shifts -= 4;
   }

   decoded = gg_descramble (decoded);

   *address = ((decoded >> 8)  & ((1 << 16) - 1));
   *value   = ((decoded >> 24) & ((1 << 8)  - 1));

   if (length == 8)
   {
      *match_value = (decoded & ((1 << 8) - 1));
   }
   else
   {
      *address += 0x8000;
      *match_value = cpu_read (*address);
   }

   return (0);
}

int cheats_decode (const UINT8 *code, UINT16 *address, UINT8 *value,
   UINT8 *match_value)
{
   if ((strlen (code) == 7) || (strlen (code) == 10))
   {
      /* NESticle RAW 7 or 10 digit code. */
      return (decode_raw (code, address, value, match_value));
   }
   else if ((strlen (code) == 6) || (strlen (code) == 8))
   {
      /* Game genie 6 or 8 digit code. */
      return (decode_game_genie (code, address, value, match_value));
   }
   else
   {
      /* Invalid # of digits. */
      return (1);
   }
}
