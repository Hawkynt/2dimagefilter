/* FakeNES - A free, portable, Open Source NES emulator.

   data.h: Datafile interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef DATA_H_INCLUDED
#define DATA_H_INCLUDED
#include "common.h"
#ifdef __cplusplus
extern "C" {
#endif
#include "datafile.h"

#define DATA(id)           (datafile_data[DATAFILE_ ##id].dat)
#define DATA_INDEX(id)     DATAFILE_##id
#define DATA_TO_BITMAP(id) ((BITMAP *)DATA(id))
#define DATA_TO_FONT(id)   ((FONT *)DATA(id))
#define DATA_TO_RGB(id)    ((RGB *)DATA(id))
#define DATA_TO_SAMPLE(id) ((SAMPLE *)DATA(id))

#ifdef __cplusplus
}
#endif
#endif   /* !DATA_H_INCLUDED */
