/* FakeNES - A free, portable, Open Source NES emulator.

   types.h: Portable type definitions.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef TYPES_H_INCLUDED
#define TYPES_H_INCLUDED
#include <allegro.h>
#ifdef ALLEGRO_WINDOWS
#include <winalleg.h>
#endif
#include <string.h>
#include "common.h"
#ifdef __cplusplus
extern "C" {
#endif

/* Base types. */
typedef unsigned char fakenes_uint8_t;
typedef signed char   fakenes_int8_t;

#ifdef C99_TYPES
   typedef uint16_t       fakenes_uint16_t;
   typedef int16_t        fakenes_int16_t;
#elif SIZEOF_SHORT == 2
   typedef unsigned short fakenes_uint16_t;
   typedef signed short   fakenes_int16_t;
#elif SIZEOF_INT == 2
   typedef unsigned int   fakenes_uint16_t;
   typedef signed int     fakenes_int16_t;
#else                             
#  error No 16-bit type could be found.
#endif

#ifdef C99_TYPES
   typedef uint32_t       fakenes_uint32_t;
   typedef int32_t        fakenes_int32_t;
#elif SIZEOF_INT == 4
   typedef unsigned int  fakenes_uint32_t;
   typedef signed int    fakenes_int32_t;
#elif SIZEOF_LONG == 4
   typedef unsigned long fakenes_uint32_t;
   typedef signed long   fakenes_int32_t;
#else
#  error No 32-bit type could be found.
#endif

typedef int             fakenes_enum_t;   /* Enumeration index. */
typedef unsigned        fakenes_flags_t;  /* Flags. */
typedef double          fakenes_real_t;   /* Real number. */
typedef char            fakenes_char_t;   /* ASCII character. */
typedef signed char     fakenes_bool_t;   /* Boolean value. */
typedef char            fakenes_uchar_t;  /* Unicode character. */
typedef fakenes_flags_t fakenes_list_t;   /* List of flags. */

/* Maximum length (in fakenes_uchar_t's) of a Unicode character. */
#define MAX_UCHAR_LENGTH   4

/* String data types. */
#define STRING_SIZE_BASE   1024  /* Typical size. */
#define STRING_SIZE        (STRING_SIZE_BASE * sizeof(fakenes_char_t))
#define USTRING_SIZE       (STRING_SIZE_BASE * sizeof(fakenes_uchar_t))

typedef fakenes_char_t  fakenes_string_t[STRING_SIZE_BASE];
typedef fakenes_uchar_t fakenes_ustring_t[STRING_SIZE_BASE];

/* Pair data type for CPU core. */
typedef union
{
   struct
   {
#ifdef LSB_FIRST
      fakenes_uint8_t low, high;
#else
      fakenes_uint8_t high, low;
#endif

   } bytes;

   fakenes_uint16_t word;

} fakenes_pair_t;

/* Shorthand aliases. */
/* typedef where possible, otherwise #define. */
typedef fakenes_uint8_t  UINT8;
typedef fakenes_int8_t   INT8;
typedef fakenes_uint16_t UINT16;
typedef fakenes_int16_t  INT16;

#ifdef ALLEGRO_WINDOWS
   /* Override Win32 typedefs. */
#  define UINT32 fakenes_uint32_t
#  define INT32  fakenes_int32_t
#else
   typedef fakenes_uint32_t UINT32;
   typedef fakenes_int32_t  INT32;
#endif

typedef fakenes_enum_t  ENUM;
typedef fakenes_flags_t FLAGS;
typedef fakenes_real_t  REAL;
typedef fakenes_char_t  CHAR;

#ifdef ALLEGRO_WINDOWS
   /* Override Win23 typedefs. */
#  define BOOL  fakenes_bool_t
#  define UCHAR fakenes_uchar_t
#else
   typedef fakenes_bool_t  BOOL;
   typedef fakenes_uchar_t UCHAR;
#endif

typedef fakenes_list_t    LIST;
typedef fakenes_string_t  STRING;
typedef fakenes_ustring_t USTRING;
typedef fakenes_pair_t    PAIR;

#ifdef __cplusplus
/* Lowercase C++ style aliases. */
typedef fakenes_uint8_t  uint8;
typedef fakenes_int8_t   int8;
typedef fakenes_uint16_t uint16;
typedef fakenes_int16_t  int16;
typedef fakenes_uint32_t uint32;
typedef fakenes_int32_t  int32;
typedef fakenes_real_t   real;
#endif

/* List access macros. */
#define LIST_ADD(list, flags)       (list |= flags)
#define LIST_REMOVE(list, flags)    (list &= ~flags)
#define LIST_TOGGLE(list, flags)    (list ^= flags)
#define LIST_COMPARE(list, flags)   TRUE_OR_FALSE(list & flags)

/* String clearing macros. */
#define STRING_CLEAR_SIZE(str, size)   memset (str, 0, size)
#define USTRING_CLEAR_SIZE             STRING_CLEAR_SIZE
#define STRING_CLEAR(str)              STRING_CLEAR_SIZE(str, STRING_SIZE)
#define USTRING_CLEAR(str)             USTRING_CLEAR_SIZE(str, USTRING_SIZE)

#ifdef __cplusplus                     
}
#endif
#endif   /* !TYPES_H_INCLUDED */
