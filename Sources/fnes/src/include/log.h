/* FakeNES - A free, portable, Open Source NES emulator.

   log.h: Declarations for the logging functions.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef LOG_H_INCLUDED
#define LOG_H_INCLUDED
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

void log_open (const char *);
void log_close (void);
void log_printf (const UCHAR *, ...);
UCHAR *get_log_text (void);
void console_clear (void);
void console_printf (const UCHAR *, ...);
UCHAR *get_console_text (void);

#ifdef __cplusplus
}
#endif
#endif   /* !LOG_H_INCLUDED */

