/* FakeNES - A free, portable, Open Source NES emulator.

   video.h: Declarations for the video interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef VIDEO_H_INCLUDED
#define VIDEO_H_INCLUDED
#include "common.h"
#include "debug.h"
#include "gui.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

volatile int video_message_duration;

enum
{
   VIDEO_BLITTER_AUTOMATIC = -1,
   VIDEO_BLITTER_NORMAL,
   VIDEO_BLITTER_DES,
   VIDEO_BLITTER_INTERPOLATED_2X,
   VIDEO_BLITTER_INTERPOLATED_2X_HQ,
   VIDEO_BLITTER_2XSCL,
   VIDEO_BLITTER_DESII,
   VIDEO_BLITTER_SUPER_2XSCL,
   VIDEO_BLITTER_ULTRA_2XSCL,
   VIDEO_BLITTER_HQ2X,
   VIDEO_BLITTER_NTSC,
   VIDEO_BLITTER_INTERPOLATED_3X,
   VIDEO_BLITTER_HQ3X,
   VIDEO_BLITTER_HQ4X,
   VIDEO_BLITTER_STRETCHED
};

#define VIDEO_FILTER_SCANLINES_LOW      1
#define VIDEO_FILTER_SCANLINES_MEDIUM   2
#define VIDEO_FILTER_SCANLINES_HIGH     4

int video_buffer_width;
int video_buffer_height;

BOOL video_display_status;
BOOL video_enable_page_buffer;
BOOL video_enable_vsync;
BOOL video_force_fullscreen;
int video_cached_color_depth; /* Read only. */

int video_driver;
   
BITMAP *base_video_buffer;
BITMAP *video_buffer;

FONT *small_font;

LIST video_edge_clipping;

#define VIDEO_EDGE_CLIPPING_HORIZONTAL (1 << 0)
#define VIDEO_EDGE_CLIPPING_VERTICAL   (1 << 1)

RGB *video_palette;

int video_init (void);
int video_reinit (void);
int video_init_buffer (void);
void video_exit (void);
void video_blit (BITMAP *);
void video_filter (void);
void video_handle_keypress (int, int);
void video_set_palette (RGB *);
void video_set_palette_id (int);
int video_get_palette_id (void);
int video_create_color_dither (int, int, int, int, int);
int video_create_gradient (int, int, int, int, int);
void video_create_gui_gradient (GUI_COLOR *, GUI_COLOR *, int);
void video_set_blitter (ENUM);
ENUM video_get_blitter (void);
void video_blitter_reinit (void);
void video_set_filter_list (LIST);
LIST video_get_filter_list (void);
void video_set_resolution (int, int);
int video_get_color_depth (void);
void video_set_color_depth (int);
void video_set_driver (int);
BOOL video_is_opengl_mode (void);
void video_show_bitmap (BITMAP *, ENUM, BOOL);

void video_message (const UCHAR *, ...);

UINT8 video_color_map[32][32][32];

static INLINE int video_create_color (int r, int g, int b)
{
   /* Note: Don't use the makecol() or makecol8() functions here, as they
      don't appear to be inlined by Allegro. */

   switch (video_cached_color_depth)
   {
      case 8:
         return (video_color_map[(r >> 3)][(g >> 3)][(b >> 3)]);

      case 15:
         return (makecol15 (r, g, b));

      case 16:
         return (makecol16 (r, g, b));

      case 24:
         return (makecol24 (r, g, b));

      case 32:
         return (makecol32 (r, g, b));

      default:
         WARN_GENERIC();
   }

   return (0);
}

#ifdef __cplusplus
}
#endif
#endif   /* !VIDEO_H_INCLUDED */
