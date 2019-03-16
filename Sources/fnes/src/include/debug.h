/* FakeNES - A free, portable, Open Source NES emulator.

   debug.h: Debug interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef DEBUG_H_INCLUDED
#define DEBUG_H_INCLUDED
#include <allegro.h>
#include "common.h"
#include "log.h"
#ifdef __cplusplus
extern "C" {
#endif

#ifdef DEBUG
#define DEBUG_PRINTF(format, args...)  log_printf ( format , ## args )
#else
#define DEBUG_PRINTF(format, args...)
#endif

/* Warning macros to help with debugging. */
#define WARN(message)   \
{  \
   allegro_message ("WARNING\n\n" message "\n\nat line %d of %s", \
      __LINE__, __FILE__); \
   log_printf ("Warning: " message " (line %d, %s)", __LINE__, __FILE__);  \
}

#define WARN_GENERIC()  WARN("Possible code fault")

#define WARN_BREAK(message) { \
   WARN(message); \
   return;  \
}

#define WARN_BREAK_GENERIC()  WARN_BREAK("Possible code fault")

#define RT_ASSERT(cond) {  \
   if (!(cond)) { \
      WARN("Runtime assertion error"); \
      exit (-1);  \
   }  \
}

#ifdef __cplusplus
}
#endif
#endif   /* !DEBUG_H_INCLUDED */
