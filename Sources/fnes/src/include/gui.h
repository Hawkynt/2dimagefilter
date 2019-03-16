/* FakeNES - A free, portable, Open Source NES emulator.

   gui.h: Declarations for the object-based GUI.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef GUI_H_INCLUDED
#define GUI_H_INCLUDED
#include <allegro.h>
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

typedef struct _GUI_COLOR
{
    REAL r, g, b;
    int packed;

} GUI_COLOR;

#define GUI_GRADIENT_START_COLOR    (gui_theme[0].packed)
#define GUI_GRADIENT_END_COLOR      (gui_theme[1].packed)
#define GUI_BACKGROUND_COLOR        (gui_theme[2].packed)
#define GUI_FILL_COLOR              (gui_theme[3].packed)
#define GUI_MENU_BAR_COLOR          (gui_theme[4].packed)
#define GUI_BORDER_COLOR            (gui_theme[5].packed)
#define GUI_TEXT_COLOR              (gui_theme[6].packed)
#define GUI_LIGHT_SHADOW_COLOR      (gui_theme[7].packed)
#define GUI_SHADOW_COLOR            (gui_theme[8].packed)
#define GUI_SELECTED_COLOR          (gui_theme[9].packed)
#define GUI_DISABLED_COLOR          (gui_theme[10].packed)
#define GUI_ERROR_COLOR             (gui_theme[11].packed)
#define GUI_TOTAL_COLORS            12

typedef GUI_COLOR GUI_THEME[GUI_TOTAL_COLORS];

extern BOOL gui_is_active;
extern GUI_THEME gui_theme;
extern RGB *gui_image_palette;

extern int gui_init (void);
extern void gui_exit (void);
extern int show_gui (BOOL);
extern int gui_alert (const UCHAR *, const UCHAR *, const UCHAR *, const UCHAR *, const UCHAR *, const UCHAR *, int, int);
extern void gui_message (int, const UCHAR *, ...);
extern void gui_heartbeat (void);
extern void gui_handle_keypress (int, int);
extern void gui_stop_replay (void);
extern void gui_set_theme (const GUI_THEME *);

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !GUI_H_INCLUDED */
