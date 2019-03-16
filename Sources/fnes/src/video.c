/* FakeNES - A free, portable, Open Source NES emulator.

   video.c: Implementation of the video interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#ifdef USE_ALLEGROGL
#include <alleggl.h>
#endif
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "audio.h"
#include "common.h"
#include "cpu.h"
#include "data.h"
#include "debug.h"
#include "gui.h"
#include "hsl.h"
#include "input.h"
#include "log.h"
#include "ppu.h"
#include "rom.h"
#include "timing.h"
#include "types.h"
#include "video.h"

#ifdef USE_ALLEGROGL

/* Botch. */
static BOOL allegro_gl_installed = FALSE;

#endif   /* USE_ALLEGROGL */

int video_buffer_width = 320;
int video_buffer_height = 240;

static BITMAP *page_buffer = NULL;
static BITMAP *screen_buffer = NULL;
static BITMAP *status_buffer = NULL;

#define MAX_MESSAGES    10

static USTRING video_messages[MAX_MESSAGES];

volatile int video_message_duration = 0;

static void video_message_timer (void)
{
   if (video_message_duration > 0)
      video_message_duration -= 1000;
}

END_OF_STATIC_FUNCTION (video_message_timer);

BOOL video_display_status = FALSE;
BOOL video_enable_page_buffer = FALSE;
BOOL video_enable_vsync = FALSE;
BOOL video_force_fullscreen = FALSE;
int video_cached_color_depth = 0;   /* Read only. */

int video_driver = 0;

BITMAP *base_video_buffer = NULL;
BITMAP *video_buffer = NULL;
static BITMAP *mouse_sprite_remove_buffer = NULL;

FONT *small_font = NULL;

static int screen_width  = 640;
static int screen_height = 480;
static int color_depth   = -1;

/* Color controls (-100 to 100). */
static int video_hue        = 0;
static int video_saturation = 0;
static int video_brightness = 0;
static int video_contrast   = 0;
static int video_gamma      = 0;
                         
static LIST filter_list = 0;

#define VIDEO_COLOR_BLACK   palette_color[0]
#define VIDEO_COLOR_WHITE   palette_color[33]

/* 15-bit color mapping for 256 color modes. */
UINT8 video_color_map[32][32][32];

static BOOL preserve_video_buffer = FALSE;
static BOOL preserve_palette = FALSE;

static PALETTE internal_palette;
RGB * video_palette = NULL;
static int video_palette_id = -1;

static BOOL using_custom_font = FALSE;

LIST video_edge_clipping = 0;

/* Blitter API. */
#include "blit/shared.h"

/* Blit buffers. */
static void *blit_buffer_in  = NULL;
static void *blit_buffer_out = NULL;

/* Blitter variables. */
static int blitter_id         = VIDEO_BLITTER_NORMAL;
static const BLITTER *blitter = NULL;   /* Blitter interface. */
static int blit_x_offset      = 0;      
static int blit_y_offset      = 0;      

/* Blitters. */
#include "blit/2xscl.h"
#include "blit/des.h"
#include "blit/hqx.h"
#include "blit/interp.h"
#include "blit/ntsc.h"
#include "blit/std.h"

static void switch_out_callback (void)
{
   if (!gui_is_active)
      audio_suspend ();
}

static void switch_in_callback (void)
{
   if (!gui_is_active)
      audio_resume ();
}

int video_init (void)
{
   int driver;
   int width, height;
   int result;
   const CHAR *font_file;

   log_printf ("VIDEO: Entering video_init().");

   /* Install message timer. */

   LOCK_VARIABLE (video_message_duration);
   LOCK_FUNCTION (video_message_timer);
   install_int_ex (video_message_timer, BPS_TO_TIMER(1));

   /* Load configuration. */

   log_printf ("VIDEO: Loading configuration.");

   video_driver             = get_config_id  ("video", "driver",             video_driver);
   screen_width             = get_config_int ("video", "screen_width",       screen_width);
   screen_height            = get_config_int ("video", "screen_height",      screen_height);
   color_depth              = get_config_int ("video", "color_depth",        color_depth);
   video_force_fullscreen   = get_config_int ("video", "force_fullscreen",   video_force_fullscreen);
   video_buffer_width       = get_config_int ("video", "buffer_width",       video_buffer_width);
   video_buffer_height      = get_config_int ("video", "buffer_height",      video_buffer_height);
   blitter_id               = get_config_int ("video", "blitter",            blitter_id);
   filter_list              = get_config_int ("video", "filter_list",        filter_list);
   video_hue                = get_config_int ("video", "hue",                video_hue);
   video_saturation         = get_config_int ("video", "saturation",         video_saturation);
   video_brightness         = get_config_int ("video", "brightness",         video_brightness);
   video_contrast           = get_config_int ("video", "contrast",           video_contrast);
   video_gamma              = get_config_int ("video", "gamma",              video_gamma);
   video_display_status     = get_config_int ("video", "display_status",     video_display_status);
   video_enable_page_buffer = get_config_int ("video", "enable_page_buffer", video_enable_page_buffer);
   video_enable_vsync       = get_config_int ("video", "enable_vsync",       video_enable_vsync);
   video_edge_clipping      = get_config_int ("video", "edge_clipping",      video_edge_clipping);

   /* Determine which driver to use. */

   if (video_driver == GFX_AUTODETECT)
   {
      if (video_force_fullscreen)
      {
         driver = GFX_AUTODETECT_FULLSCREEN;
      }
      else
      {
         int depth;

         depth = desktop_color_depth ();

         /* Attempt to detect a windowed environment.  This has a side
            effect of changing the default color depth to that of the
            desktop. */

         if (depth > 0)
         {
            driver = GFX_AUTODETECT_WINDOWED;

            if (color_depth == -1)
               color_depth = depth;
         }
         else
         {
            driver = GFX_AUTODETECT;
         }
      }
   }
   else
   {
      driver = video_driver;
   }

#ifdef USE_ALLEGROGL

   if ((driver == GFX_OPENGL) ||
       (driver == GFX_OPENGL_FULLSCREEN) ||
       (driver == GFX_OPENGL_WINDOWED))
   {
      log_printf ("VIDEO: Installing AllegroGL.\n");

      /* Install AllegroGL. */
      install_allegro_gl ();

      /* Due to a bug in AllegroGL, we must make sure to remove it before
         accessing any other video modes.  However, we must set this flag
         to know that we've installed it, because there doesn't appear to
         be any other way to tell. */
      allegro_gl_installed = TRUE;

      /* Hint at which modes we want for OpenGL. */
                                       
      allegro_gl_set (AGL_FULLSCREEN,  video_force_fullscreen);
      allegro_gl_set (AGL_WINDOWED,   !video_force_fullscreen);

      allegro_gl_set (AGL_SUGGEST, (AGL_FULLSCREEN | AGL_WINDOWED));
   }

#endif   /* USE_ALLEGROGL */

   /* Set color depth. */

   if (color_depth == -1)
   {
      /* No windowed environment present to autodetect a color depth from;
         default to 256 colors. */

      color_depth = 8;
   }

   if ((color_depth != 8)  &&
       (color_depth != 15) &&
       (color_depth != 16) &&
       (color_depth != 24) &&
       (color_depth != 32))
   {   
      WARN("Invalid color depth");
      return (1);
   }

   /* Let filters know directly what color depth we're using. */
   video_cached_color_depth = color_depth;

   set_color_depth (color_depth);

   /* Enter graphics mode. */

   log_printf ("VIDEO: Entering graphics mode.");

   if (set_gfx_mode (driver, screen_width, screen_height, 0, 0) != 0)
   {
      WARN("set_gfx_mode() failed");
      return (2);
   }

#ifdef USE_ALLEGROGL

   if (video_is_opengl_mode ())
   {
      log_printf ("VIDEO: Setting up OpenGL.\n");

      /* Enable OpenGL texturing. */
      glEnable (GL_TEXTURE_2D);

      /* Disable page buffering, since it is pointless. */
      video_enable_page_buffer = FALSE;

      /* Disable VSync, since it crashes AllegroGL. */
      video_enable_vsync = FALSE;
   }

#endif   /* USE_ALLEGROGL */

   if (color_depth != 8)
      set_color_conversion ((COLORCONV_TOTAL | COLORCONV_KEEP_TRANS));
       
   /* Create page buffer. */

   if (video_enable_page_buffer)
   {
      log_printf ("VIDEO: Creating page buffer.");

      page_buffer = create_video_bitmap (SCREEN_W, SCREEN_H);
      if (!page_buffer)
      {
         WARN("Failed to create page buffer");
         video_enable_page_buffer = FALSE;
      }
   }
   else
   {
      page_buffer = NULL;
   }

   if (!preserve_video_buffer)
   {
      /* Create video buffer. */

      log_printf ("VIDEO: Creating video buffer.");

      base_video_buffer = create_bitmap_ex (8, ((8 + 256) + 8), ((16 + 240)
         + 16));
      video_buffer = create_sub_bitmap (base_video_buffer, 8, 16, 256, 240);

      if (!base_video_buffer || !video_buffer)
      {
         WARN("Couldn't create video buffer");
         return (3);
      }

      clear_bitmap (base_video_buffer);
   }

   /* TODO: Is this really neccessary?  Maybe we can remove this stuff when
      the PPU gets a new internal buffer format. */
   mouse_sprite_remove_buffer = create_bitmap_ex (8, 16, 16);
   if (!mouse_sprite_remove_buffer)
   {
      WARN_GENERIC();
      return (4);
   }

   /* Create screen buffer.
      Note: This automatically sets up the blitter, too. =P */

   if ((result = video_init_buffer ()) != 0)
      return ((8 + result));

   /* Set up palette. */

   if (preserve_palette)
   {
      /* Use existing palette. */
      video_set_palette (NULL);
   }
   else
   {
      /* Set default palette. */
      video_set_palette    (DATA_TO_RGB(MODERN_NTSC_PALETTE));
      video_set_palette_id (DATA_INDEX(MODERN_NTSC_PALETTE));
   }

   /* Set up filters. */

   video_set_filter_list (filter_list);

   /* Set up fonts. */

   small_font = DATA_TO_FONT(SMALL_FONT);

   font_file = get_config_string ("gui", "font", "");

   if ((strlen (font_file) > 1) && (exists (font_file)))
   {
      font = load_font (font_file, NULL, NULL);

      if (font)
      {
         using_custom_font = TRUE;
      }
      else
      {
         WARN("Font load failed");

         font = DATA_TO_FONT(SMALL_FONT_CLEAN);
         using_custom_font = FALSE;
      }
   }
   else
   {
      /* Reset just in case. */

      font = DATA_TO_FONT (SMALL_FONT_CLEAN);
      using_custom_font = FALSE;
   }
      
   if (is_windowed_mode ())
   {
      set_display_switch_mode (SWITCH_BACKGROUND);
   }
   else
   {
      set_display_switch_mode (SWITCH_AMNESIA);

      /* Install callbacks. */
      set_display_switch_callback (SWITCH_IN,  switch_in_callback);
      set_display_switch_callback (SWITCH_OUT, switch_out_callback);
   }

   log_printf ("VIDEO: Exiting video_init().");

   /* Return success. */
   return (0);
}

int video_reinit (void)
{
   int result;

   preserve_video_buffer = TRUE;
   preserve_palette = TRUE;

   video_exit ();

   result = video_init ();

   if (result == 0)
   {
      preserve_video_buffer = FALSE;
      preserve_palette = FALSE;
   }

   return (result);
}

int video_init_buffer (void)
{
   int width, height;

   /* Fun. */

   width  = ((video_buffer_width  == -1) ? SCREEN_W : video_buffer_width);
   height = ((video_buffer_height == -1) ? SCREEN_H : video_buffer_height);

   if (screen_buffer)
      destroy_bitmap (screen_buffer);

   log_printf ("VIDEO: Creating screen buffer.");

   screen_buffer = create_bitmap (width, height);
   if (!screen_buffer)
   {
      WARN("Couldn't create screen buffer");
      return (1);
   }

   clear_bitmap (screen_buffer);

   if (status_buffer)
      destroy_bitmap (status_buffer);

   /* Create status buffer. */
   status_buffer = create_sub_bitmap (screen_buffer, 8, (screen_buffer->h -
      116), 80, 100);
   if (!status_buffer)
   {
      destroy_bitmap (screen_buffer);
      screen_buffer = NULL;

      WARN("Failed to create status buffer");
      return (2);
   }

   /* Set up blitter. */
   video_set_blitter (blitter_id);

   /* Return success. */
   return (0);
}

void video_exit (void)
{
   log_printf ("VIDEO: Entering video_exit().");

   if (!is_windowed_mode ())
   {
      /* Remove callbacks. */
      remove_display_switch_callback (switch_in_callback);
      remove_display_switch_callback (switch_out_callback);
   }

   /* Remove message timer. */
   remove_int (video_message_timer);

   if (using_custom_font)
   {
      /* Destroy font. */
      destroy_font (font);
      using_custom_font = FALSE;
   }

   if (blitter)
   {
      /* Deinitializer blitter. */

      if (blitter->deinit)
         blitter->deinit ();

      blitter = NULL;
   }

   /* Destroy buffers. */

   if (mouse_sprite_remove_buffer)
   {
      destroy_bitmap (mouse_sprite_remove_buffer);
      mouse_sprite_remove_buffer = NULL;
   }

   if (!preserve_video_buffer)
   {
      log_printf ("VIDEO: Destroying video buffer.");

      if (video_buffer)
      {
         destroy_bitmap (video_buffer);
         video_buffer = NULL;
      }

      if (base_video_buffer)
      {
         destroy_bitmap (base_video_buffer);
         video_buffer = NULL;
      }
   }

   log_printf ("VIDEO: Destroying screen buffer.");

   if (status_buffer)
   {
      destroy_bitmap (status_buffer);
      status_buffer = NULL;
   }

   if (screen_buffer)
   {
      destroy_bitmap (screen_buffer);
      screen_buffer = NULL;
   }

   if (page_buffer)
   {
      log_printf ("VIDEO: Destroying page buffer.");

      destroy_bitmap (page_buffer);
      page_buffer = NULL;
   }

   log_printf ("VIDEO: Exiting graphics mode.");

   /* Return to text mode. */
   set_gfx_mode (GFX_TEXT, 0, 0, 0, 0);

#ifdef USE_ALLEGROGL

   if (allegro_gl_installed)
   {
      log_printf ("VIDEO: Uninstalling AllegroGL.");

      /* Remove AllegroGL and restore Allegro GFX drivers. */
      remove_allegro_gl ();

      /* Clear flag. */
      allegro_gl_installed = FALSE;
   }

#endif   /* USE_ALLEGRO_GL */

   /* Save configuration. */

   log_printf ("VIDEO: Saving configuration.");

   set_config_id  ("video", "driver",             video_driver);
   set_config_int ("video", "screen_width",       screen_width);
   set_config_int ("video", "screen_height",      screen_height);
   set_config_int ("video", "color_depth",        color_depth);
   set_config_int ("video", "force_fullscreen",   video_force_fullscreen);
   set_config_int ("video", "buffer_width",       video_buffer_width);
   set_config_int ("video", "buffer_height",      video_buffer_height);
   set_config_int ("video", "blitter",            blitter_id);
   set_config_int ("video", "filter_list",        filter_list);
   set_config_int ("video", "hue",                video_hue);
   set_config_int ("video", "saturation",         video_saturation);
   set_config_int ("video", "brightness",         video_brightness);
   set_config_int ("video", "contrast",           video_contrast);
   set_config_int ("video", "gamma",              video_gamma);
   set_config_int ("video", "display_status",     video_display_status);
   set_config_int ("video", "enable_page_buffer", video_enable_page_buffer);
   set_config_int ("video", "enable_vsync",       video_enable_vsync);
   set_config_int ("video", "edge_clipping",      video_edge_clipping);

   log_printf ("VIDEO: Exiting video_exit().");
}


static INLINE void shadow_textout (BITMAP *bitmap, FONT *font, const UCHAR
   *text, int x, int y, int color)
{
   RT_ASSERT(bitmap);
   RT_ASSERT(font);
   RT_ASSERT(text);

   textout_ex (bitmap, font, text, (x + 1), (y + 1), VIDEO_COLOR_BLACK, -1);
   textout_ex (bitmap, font, text, x, y, color, -1);
}

static INLINE void shadow_textprintf (BITMAP *bitmap, FONT *font, int x, int
   y, int color, const UCHAR *text, ...)
{
   va_list format;
   USTRING buffer;

   RT_ASSERT(bitmap);
   RT_ASSERT(font);
   RT_ASSERT(text);

   va_start (format, text);
   uvszprintf (buffer, sizeof (buffer), text, format);
   va_end (format);

   /* Pass on to textout. */
   shadow_textout (bitmap, font, buffer, x, y, color);
}


/* Todo: Find a better way to do all this. */

static INLINE void display_status (BITMAP *bitmap, FONT *font, int color)
{
   int y = 0;
   int indent, line, spacer;
   int hours = 0, minutes = 0, seconds = 0;
   unsigned offset;

   RT_ASSERT(bitmap);
   RT_ASSERT(font);

   indent = text_length (font, "XXX");
   line   = ROUND((text_height (font) * 1.67));
   spacer = ROUND((line * 1.33));

   /* Convert seconds-elapsed to hours, minutes, and seconds. */

   for (offset = 0; offset < timing_clock; offset++)
   {
      seconds++;
      if (seconds >= 60)
      {
         seconds -= 60;
         minutes++;
         if (minutes >= 60)
         {
            minutes -= 60;
            hours++;
            if (hours > 60)
               hours = 60;
         }
      }
   }

   shadow_textprintf (bitmap, font, 0, y, color, "%02d:%02d:%02d",
      hours, minutes, seconds);

   y += spacer;

   shadow_textout (bitmap, font, "Video:", 0, y, color);
   y += line;

   shadow_textprintf (bitmap, font, indent, y, color, "%02d FPS",
      timing_fps);
   y += spacer;

   shadow_textout (bitmap, font, "Audio:", 0, y, color);
   y += line;

   if (audio_enable_output)
   {
      shadow_textprintf (bitmap, font, indent, y, color, "%02d FPS",
         timing_audio_fps);
   }
   else
   {
      shadow_textout (bitmap, font, "Disabled", indent, y, color);
   }

   y += spacer;

   shadow_textout (bitmap, font, "Core:",  0, y, color);
   y += line;

   shadow_textprintf (bitmap, font, indent, y, color, "%02d/%g Hz",
      timing_hertz, (double)timing_get_speed ());
   y += line;

   shadow_textprintf (bitmap, font, indent, y, color, "PC: $%04X",
      *cpu_active_pc);
}


static void draw_messages (void);

static void erase_messages (void);


static int flash_tick = 0;


void video_blit (BITMAP *bitmap)
{
   BITMAP *dest;

   RT_ASSERT(bitmap);

   if (!rom_is_loaded)
      return;

   if (video_edge_clipping)
   {
      int w, h;

      /* Calculate sizes. */
      w = (video_buffer->w - 1);
      h = (video_buffer->h - 1);

      if (video_edge_clipping & VIDEO_EDGE_CLIPPING_HORIZONTAL)
      {
         /* Left edge. */
         rectfill (video_buffer, 0, 0, 8, h, 15);
   
         /* Right edge. */
         rectfill (video_buffer, (w - 8), 0, w, h, 15);
      }

      if (video_edge_clipping & VIDEO_EDGE_CLIPPING_VERTICAL)
      {
         /* Top edge. */
         rectfill (video_buffer, 0, 0, w, 8, 15);
                                          
         /* Bottom edge. */
         rectfill (video_buffer, 0, (h - 8), w, h, 15);
      }
   }

   if (input_enable_zapper && !gui_is_active)
   {
      /* Draw Zapper sprite. */

      blit (video_buffer, mouse_sprite_remove_buffer,
         (input_zapper_x_offset - 7), (input_zapper_y_offset - 7), 0, 0, 16,
            16);

      masked_blit (DATA_TO_BITMAP(GUN_SPRITE), video_buffer, 0, 0,
         (input_zapper_x_offset - 7), (input_zapper_y_offset - 7), 16, 16);
   }

   /* Perform blitting operation. */
   blitter->blit (video_buffer, screen_buffer, blit_x_offset,
      blit_y_offset);

   if (input_enable_zapper && !gui_is_active)
   {
      /* Undraw Zapper sprite. */

      blit (mouse_sprite_remove_buffer, video_buffer, 0, 0,
         (input_zapper_x_offset - 7), (input_zapper_y_offset - 7), 16, 16);
   }

   /* Apply filters. */
   video_filter ();

   if (video_display_status && !gui_is_active)
      display_status (status_buffer, small_font, VIDEO_COLOR_WHITE);

   if (((video_message_duration > 0) ||
        (input_mode & INPUT_MODE_CHAT)) && !gui_is_active)
   {
      /* Draw messages. */
      draw_messages ();
   }

#ifdef USE_ALLEGROGL

   if (video_is_opengl_mode () && (bitmap == screen))
      goto glblit;

#endif   /* USE_ALLEGROGL */

   /* Send screen buffer to screen. */

   if (page_buffer && (bitmap == screen))
   {
      /* Reduce screen tearing by blitting to VRAM first, then doing a
         VRAM to VRAM blit to the visible portion of the screen, since
         such blits are much faster.  Of course, we could just do page
         flipping, but this way we keep things simple and compatible. */

      /* Draw to page buffer first, then to screen (see above). */
      dest = page_buffer;
   }
   else
   {
      /* Draw directly to bitmap. */
      dest = bitmap;
   }

   acquire_bitmap (dest);

   if ((dest == screen) && video_enable_vsync)
      vsync ();

   if ((screen_buffer->w != dest->w) || (screen_buffer->h != dest->h))
   {
      /* Scaling is required. */

      stretch_blit (screen_buffer, dest, 0, 0, screen_buffer->w,
         screen_buffer->h, 0, 0, dest->w, dest->h);
   }
   else
   {
      /* No scaling is required. */

      blit (screen_buffer, dest, 0, 0, 0, 0, screen_buffer->w,
         screen_buffer->h);
   }

   release_bitmap (dest);

   if (dest != bitmap)
   {
      acquire_bitmap (bitmap);

      if ((bitmap == screen) && video_enable_vsync)
         vsync ();

      blit (dest, bitmap, 0, 0, 0, 0, dest->w, dest->h);

      release_bitmap (bitmap);
   }

#ifdef USE_ALLEGROGL

   glblit:
   {
      if (video_is_opengl_mode () && (bitmap == screen))
         video_show_bitmap (screen_buffer, 2, FALSE);
   }

#endif   /* USE_ALLEGROGL */

   if (((video_message_duration > 0) ||
        (input_mode & INPUT_MODE_CHAT)) && !gui_is_active)
   {
      /* Undraw messages. */
      erase_messages ();
   }

   /* Clear status buffer. */
   clear (status_buffer);
}

void video_show_bitmap (BITMAP *bitmap, ENUM quality, BOOL with_mouse)
{
   /* Generic buffer-to-screen display function.

      Under OpenGL, uses direct OpenGL calls instead of AllegroGL's
      emulation layer.  It can optionally provide software drawing of the
      mouse pointer, which is used by the GUI routines.

      Quality settings:
         0 - Default.
         1 - OpenGL Map Nearest.
         2 - OpenGL Map Linear. */

   RT_ASSERT(bitmap);

#ifdef USE_ALLEGROGL

   if (video_is_opengl_mode ())
   {
      BITMAP *saved = NULL;   /* Qwell warnings. */
      int saved_x = 0, saved_y = 0;
      GLuint texture_id;

      if (with_mouse)
      {
         /* Save mouse coordinates. */
         saved_x = (mouse_x - mouse_x_focus);
         saved_y = (mouse_y - mouse_y_focus);
   
         saved = create_bitmap (mouse_sprite->w, mouse_sprite->h);
   
         if (saved)
         {
            blit (bitmap, saved, saved_x, saved_y, 0, 0, saved->w,
               saved->h);
         }

         /* Draw mouse pointer. */
         draw_sprite (bitmap, mouse_sprite, saved_x, saved_y);
      }

      /* Create and upload texture. */

      texture_id = allegro_gl_make_texture (bitmap);
      if (texture_id == 0)
         WARN("Creation of OpenGL texture failed");

      /* Select texture. */
      glBindTexture (GL_TEXTURE_2D, texture_id);

      /* Set texture properties. */

      switch (quality)
      {
         case 0:  /* Default. */
            break;

         case 1:  /* OpenGL Map Nearest. */
         {
            glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
               GL_NEAREST);
            glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
               GL_NEAREST);

            break;
         }

         case 2:  /* OpenGL Map Linear. */
         {
            glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
               GL_LINEAR);
            glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
               GL_LINEAR);

            break;
         }

         default:
            WARN_GENERIC();
      }

      /* Clamp edges. */

      glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_S,     GL_CLAMP);
      glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_T,     GL_CLAMP);

      /* Disable environmental modifications. */
      glTexEnvf (GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);

      /* Draw quad. */

      glBegin (GL_QUADS);

         glTexCoord2f (0, 0);
         glVertex3f (-1.0f, -1.0f, 0);

         glTexCoord2f (1.0f, 0);
         glVertex3f (1.0f, -1.0f, 0);

         glTexCoord2f (1.0f, 1.0f);
         glVertex3f (1.0f, 1.0f, 0);
                           
         glTexCoord2f (0, 1.0f);
         glVertex3f (-1.0f, 1.0f, 0);

      glEnd ();

      /* Update screen. */
      allegro_gl_flip ();

      /* Delete texture. */
      glDeleteTextures (1, &texture_id);

      if (with_mouse && saved)
      {
         /* Erase mouse pointer. */
         blit (saved, bitmap, 0, 0, saved_x, saved_y, saved->w, saved->h);

         destroy_bitmap (saved);
      }

      return;
   }

#endif   /* USE_ALLEGROGL */

   blit (bitmap, screen, 0, 0, 0, 0, bitmap->w, bitmap->h);
}

void video_handle_keypress (int c, int scancode)
{
    if (input_mode & INPUT_MODE_CHAT)
        return;

    switch (scancode)
    {
        case KEY_F10:

            video_brightness -= 5;

            if (video_brightness < -100)
            {
                video_brightness = -100;
            }


            video_set_palette (video_palette);


            break;


        case KEY_F11:

            video_brightness += 5;

            if (video_brightness > 100)
            {
               video_brightness = 100;
            }


            video_set_palette (video_palette);


            break;


        default:

            break;
    }
}


#define NES_PALETTE_SIZE            64
#define NES_PALETTE_START           1
#define NES_PALETTE_END             (NES_PALETTE_START + NES_PALETTE_SIZE)

#define GUI_GRADIENT_PALETTE_SIZE   64
#define GUI_GRADIENT_PALETTE_START  (NES_PALETTE_END + 1)
#define GUI_GRADIENT_PALETTE_END    (GUI_GRADIENT_PALETTE_START + GUI_GRADIENT_PALETTE_SIZE)

#define GUI_COLORS_PALETTE_SIZE     GUI_TOTAL_COLORS
#define GUI_COLORS_PALETTE_START    (GUI_GRADIENT_PALETTE_END + 1)
#define GUI_COLORS_PALETTE_END      (GUI_COLORS_PALETTE_START + GUI_COLORS_PALETTE_SIZE)

#define GUI_IMAGE_PALETTE_SIZE      112
#define GUI_IMAGE_PALETTE_START     (256 - GUI_IMAGE_PALETTE_SIZE)
#define GUI_IMAGE_PALETTE_END       (GUI_IMAGE_PALETTE_START + GUI_IMAGE_PALETTE_SIZE)

static COLOR_MAP half_transparency_map;

void video_set_palette (RGB *palette)
{
   /* Sets a palette.  'palette' may be set to NULL to just reload the
      configuration with the existing palette. */

   PALETTE temp_pal;
   int index;
   int r, g, b;

   if (palette)
   {
      /* Set palette pointer so the GUI, etc. knows which one we are
         using. */
      video_palette = palette;

      /* Copy to internal palette. */
      memcpy (internal_palette, palette, sizeof (internal_palette));
   }
   else
   {
      /* Looks like it's a call to update the palette to changed parameters,
         so we should grab updated parameters from the config file. */

      video_hue        = get_config_int ("video", "hue",        video_hue);
      video_saturation = get_config_int ("video", "saturation", video_saturation);
      video_brightness = get_config_int ("video", "brightness", video_brightness);
      video_contrast   = get_config_int ("video", "contrast",   video_contrast);
      video_gamma      = get_config_int ("video", "gamma",      video_gamma);
   }

   /* Copy the internal palette to the temporary palette to keep from
      modifying the original. */
   memcpy (temp_pal, internal_palette, sizeof (temp_pal));

   for (index = NES_PALETTE_START; index < NES_PALETTE_END; index++)
   {
      RGB *rgb = &temp_pal[index];
      REAL hue, saturation, brightness, contrast, gamma;
      REAL h, s, l;
      REAL nr, ng, nb;

      /* Convert color control variables to a normalized format. */
      hue        = (video_hue        / 100.0);
      saturation = (video_saturation / 100.0);
      brightness = (video_brightness / 100.0);
      contrast   = (video_contrast   / 100.0);
      gamma      = (video_gamma      / 100.0);

      /* Collapse hue, brightness, and gamma. */
      hue        /= 2.0;
      brightness /= 2.0;

      /* Convert from 0-63 range to 0-255 range. */
      r = ROUND(((rgb->r / 63.0) * 255.0));
      g = ROUND(((rgb->g / 63.0) * 255.0));
      b = ROUND(((rgb->b / 63.0) * 255.0));

      /* Convert to HSL. */
      rgb_to_hsl (r, g, b, &h, &s, &l);

      /* Apply hue control. */
      if (hue)
         h += hue;

      /* Apply saturation control. */
      if (saturation)
      {
         if (saturation > 0)
            s += (s * saturation);
         else
            s += saturation;
      }

      /* Clip values. */
      h = fixf (h, 0, 1.0);
      s = fixf (s, 0, 1.0);

      /* Convert back to RGB. */
      hsl_to_rgb (h, s, l, &r, &g, &b);

      /* Convert to normalized floating point. */
      nr = (r / 255.0);
      ng = (g / 255.0);
      nb = (b / 255.0);

      /* Apply contrast control. */
      if (contrast)
      {
         nr += (nr * contrast);
         ng += (ng * contrast);
         nb += (nb * contrast);

         if (contrast < 0)
         {
            REAL delta;

            delta = fabs (0.5 - fabs (contrast));

            nr += delta;
            ng += delta;
            nb += delta;
         }
      }

      /* Apply gamma control. */
      if (gamma)
      {
         nr = pow (nr, (1.0 - gamma));
         ng = pow (ng, (1.0 - gamma));
         nb = pow (nb, (1.0 - gamma));
      }

      /* Apply brightness control. */
      if (brightness)
      {
         nr += brightness;
         ng += brightness;
         nb += brightness;
      }

      /* Clip values. */
      nr = fixf (nr, 0, 1.0);
      ng = fixf (ng, 0, 1.0);
      nb = fixf (nb, 0, 1.0);

      /* Convert back to 0-63 range. */
      rgb->r = ROUND((nr * 63.0));
      rgb->g = ROUND((ng * 63.0));
      rgb->b = ROUND((nb * 63.0));
   }

   if (gui_is_active)
   {
      video_create_gui_gradient (&gui_theme[0], &gui_theme[1],
         GUI_GRADIENT_PALETTE_SIZE);
    
      for (index = GUI_GRADIENT_PALETTE_START; index <
         GUI_GRADIENT_PALETTE_END; index++)
      {
         RGB *rgb = &temp_pal[index];
         GUI_COLOR color;
    
         video_create_gui_gradient (&color, NULL, NULL);
    
         rgb->r = ROUND((color.r * 63.0));
         rgb->g = ROUND((color.g * 63.0));
         rgb->b = ROUND((color.b * 63.0));
      }
    
      for (index = GUI_COLORS_PALETTE_START; index < GUI_COLORS_PALETTE_END;
         index++)
      {
         RGB *rgb = &temp_pal[index];
         int color;
    
         color = (index - GUI_COLORS_PALETTE_START);
    
         rgb->r = ROUND((gui_theme[color].r * 63.0));
         rgb->g = ROUND((gui_theme[color].g * 63.0));
         rgb->b = ROUND((gui_theme[color].b * 63.0));
      }
    
      for (index = GUI_IMAGE_PALETTE_START; index < GUI_IMAGE_PALETTE_END;
         index++)
      {
         RGB *rgb = &temp_pal[index];

         rgb->r = gui_image_palette[index].r;
         rgb->g = gui_image_palette[index].g;
         rgb->b = gui_image_palette[index].b;
      }
   }

   /* Set the new palette. */
   set_palette (temp_pal);

   /* Build color map. */

   if (color_depth == 8)
   {
      for (r = 0; r < 32; r++)
      {
         for (g = 0; g < 32; g++)
         {
            for (b = 0; b < 32; b++)
            {
               int rm, gm, bm;
   
               rm = ROUND(((r / 31.0) * 255.0));
               gm = ROUND(((g / 31.0) * 255.0));
               bm = ROUND(((b / 31.0) * 255.0));
   
               video_color_map[r][g][b] = makecol (rm, gm, bm);
            }
         }
      }
   }

   /* Build transparency table. */

   set_trans_blender (0, 0, 0, 127);

   create_blender_table (&half_transparency_map, temp_pal, NULL);
   color_map = &half_transparency_map;
}


void video_set_palette_id (int id)
{
   video_palette_id = id;
}


int video_get_palette_id (void)
{
   return (video_palette_id);
}


static int dither_table [4] [4] =
{
    {  0,  2,  0, -2 },
    {  2,  0, -2,  0 },
    {  0, -2,  0,  2 },
    { -2,  0,  2,  0 }
};


/*
static int dither_table [4] [4] =
{
    { -8,  0, -6,  2 },
    {  4, -4,  6, -2 },
    { -5,  3, -7,  1 },
    {  7, -1,  5, -3 }
};
*/


int video_create_color_dither (int r, int g, int b, int x, int y)
{
    if (color_depth < 24)
    {
        x &= 3;
    
        y &= 3;
    
    
        r = fix ((r + dither_table [y] [x]), 0, 255);
    
        g = fix ((g + dither_table [y] [x]), 0, 255);
    
        b = fix ((b + dither_table [y] [x]), 0, 255);
    }


    return (video_create_color (r, g, b));
}


#define GRADIENT_SHIFTS         16


#define GRADIENT_MULTIPLIER     (255 << GRADIENT_SHIFTS)


static int gradient_start [3];

static int gradient_end [3];


static float gradient_delta [3];


static int gradient_slice;


static int gradient_last_x;


int video_create_gradient (int start, int end, int slices, int x, int y)
{
    if (slices)
    {
        gradient_start [0] = (getr (start) << GRADIENT_SHIFTS);

        gradient_start [1] = (getg (start) << GRADIENT_SHIFTS);

        gradient_start [2] = (getb (start) << GRADIENT_SHIFTS);


        gradient_end [0] = (getr (end) << GRADIENT_SHIFTS);

        gradient_end [1] = (getg (end) << GRADIENT_SHIFTS);

        gradient_end [2] = (getb (end) << GRADIENT_SHIFTS);


        gradient_delta [0] = ((gradient_end [0] - gradient_start [0]) / slices);

        gradient_delta [1] = ((gradient_end [1] - gradient_start [1]) / slices);

        gradient_delta [2] = ((gradient_end [2] - gradient_start [2]) / slices);


        gradient_slice = 0;


        gradient_last_x = -1;


        return (NULL);
    }
    else
    {
        int red;

        int green;

        int blue;


        red = (gradient_start [0] + (gradient_delta [0] * gradient_slice));

        green = (gradient_start [1] + (gradient_delta [1] * gradient_slice));

        blue = (gradient_start [2] + (gradient_delta [2] * gradient_slice));


        red >>= GRADIENT_SHIFTS;

        green >>= GRADIENT_SHIFTS;

        blue >>= GRADIENT_SHIFTS;


        if (gradient_last_x != x)
        {
            gradient_last_x = x;


            gradient_slice ++;
        }


        return (video_create_color_dither (red, green, blue, x, y));
    }
}
 

void video_create_gui_gradient (GUI_COLOR * start, GUI_COLOR * end, int slices)
{
    if (slices)
    {
        gradient_start [0] = (start -> r * GRADIENT_MULTIPLIER);

        gradient_start [1] = (start -> g * GRADIENT_MULTIPLIER);

        gradient_start [2] = (start -> b * GRADIENT_MULTIPLIER);


        gradient_end [0] = (end -> r * GRADIENT_MULTIPLIER);

        gradient_end [1] = (end -> g * GRADIENT_MULTIPLIER);

        gradient_end [2] = (end -> b * GRADIENT_MULTIPLIER);


        gradient_delta [0] = ((gradient_end [0] - gradient_start [0]) / slices);

        gradient_delta [1] = ((gradient_end [1] - gradient_start [1]) / slices);

        gradient_delta [2] = ((gradient_end [2] - gradient_start [2]) / slices);


        gradient_slice = 0;
    }
    else
    {
        start -> r = ((gradient_start [0] + (gradient_delta [0] * gradient_slice)) / GRADIENT_MULTIPLIER);

        start -> g = ((gradient_start [1] + (gradient_delta [1] * gradient_slice)) / GRADIENT_MULTIPLIER);

        start -> b = ((gradient_start [2] + (gradient_delta [2] * gradient_slice)) / GRADIENT_MULTIPLIER);


        gradient_slice ++;
    }
}

static INLINE int get_automatic_blitter (void)
{
   if ((screen_buffer->w >= 1024) && (screen_buffer->h >= 960))
      return (VIDEO_BLITTER_HQ4X);
   if ((screen_buffer->w >= 768) && (screen_buffer->h >= 720))
      return (VIDEO_BLITTER_HQ3X);
   if ((screen_buffer->w >= 512) && (screen_buffer->h >= 480))
      return (VIDEO_BLITTER_HQ2X);
   else
      return (VIDEO_BLITTER_DES);
}

#define BLITTER_SWITCH(id, name) \
   case id :   \
   {  \
      blitter = & blitter_##name ;  \
      break;   \
   }

void video_set_blitter (ENUM id)
{
   int selected_blitter;

   if (blitter)
   {
      if (blitter->deinit)
      {
         /* Deinitialize blitter. */
         blitter->deinit ();
      }
   }

   blitter_id = id;

   if (blitter_id == VIDEO_BLITTER_AUTOMATIC)
      selected_blitter = get_automatic_blitter ();
   else
      selected_blitter = blitter_id;

   switch (selected_blitter)
   {
      BLITTER_SWITCH(VIDEO_BLITTER_NORMAL,             normal)
      BLITTER_SWITCH(VIDEO_BLITTER_DES,                des)
      BLITTER_SWITCH(VIDEO_BLITTER_INTERPOLATED_2X,    interpolated_2x)
      BLITTER_SWITCH(VIDEO_BLITTER_INTERPOLATED_2X_HQ, interpolated_2x_hq)
      BLITTER_SWITCH(VIDEO_BLITTER_DESII,              desii)
      BLITTER_SWITCH(VIDEO_BLITTER_2XSCL,              2xscl)
      BLITTER_SWITCH(VIDEO_BLITTER_SUPER_2XSCL,        super_2xscl)
      BLITTER_SWITCH(VIDEO_BLITTER_ULTRA_2XSCL,        ultra_2xscl)
      BLITTER_SWITCH(VIDEO_BLITTER_HQ2X,               hq2x)
      BLITTER_SWITCH(VIDEO_BLITTER_NTSC,               ntsc)
      BLITTER_SWITCH(VIDEO_BLITTER_INTERPOLATED_3X,    interpolated_3x)
      BLITTER_SWITCH(VIDEO_BLITTER_HQ3X,               hq3x)
      BLITTER_SWITCH(VIDEO_BLITTER_HQ4X,               hq4x)
      BLITTER_SWITCH(VIDEO_BLITTER_STRETCHED,          stretched)

      default:
         WARN_GENERIC();
   }

   if (blitter->init)
   {
      /* Initialize blitter. */
      blitter->init (video_buffer, screen_buffer);
   }

   clear (screen_buffer);
}

#undef BLITTER_SWITCH

ENUM video_get_blitter (void)
{
   return (blitter_id);
}

void video_blitter_reinit (void)
{
   /* Reinitializes the current blitter.  Usually used to reload the updated
      blitter parameters (if any) from the configuration file. */

   if (blitter)
   {
      if (blitter->deinit)
      {
         /* Deinitialize blitter. */
         blitter->deinit ();
      }

      if (blitter->init)
      {
         /* Initialize blitter. */
         blitter->init (video_buffer, screen_buffer);
      }
   }
}

void video_set_resolution (int width, int height)
{
    int old_width;

    int old_height;


    if ((width == SCREEN_W) && (height == SCREEN_H))
    {
        return;
    }


    old_width = screen_width;

    old_height = screen_height;


    screen_width = width;

    screen_height = height;


    preserve_video_buffer = TRUE;

    preserve_palette = TRUE;


    video_exit ();


    if (video_init () != 0)
    {
        set_config_int ("video", "screen_width", old_width);

        set_config_int ("video", "screen_height", old_height);


        video_init ();
    }


    preserve_video_buffer = FALSE;

    preserve_palette = FALSE;
}

int video_get_color_depth (void)
{
    return (color_depth);
}


void video_set_color_depth (int depth)
{
    int old_depth;


    if (color_depth == depth)
    {
        return;
    }


    old_depth = color_depth;


    color_depth = depth;


    preserve_video_buffer = TRUE;

    preserve_palette = TRUE;


    video_exit ();


    if (video_init () != 0)
    {
        set_config_int ("video", "color_depth", old_depth);


        video_init ();
    }


    preserve_video_buffer = FALSE;

    preserve_palette = FALSE;
}


void video_set_driver (int driver)
{
    int old_driver;


    if (gfx_driver -> id == driver)
    {
        return;
    }


    old_driver = gfx_driver -> id;


    video_driver = driver;


    preserve_video_buffer = TRUE;

    preserve_palette = TRUE;


    video_exit ();


    if (video_init () != 0)
    {
        video_driver = old_driver;


        video_init ();
    }


    preserve_video_buffer = FALSE;

    preserve_palette = FALSE;
}


BOOL video_is_opengl_mode (void)
{
   /* Returns TRUE if we're in an OpenGL mode, obviously. ;b */

#ifdef USE_ALLEGROGL

   return (((gfx_driver->id == GFX_OPENGL) ||
            (gfx_driver->id == GFX_OPENGL_FULLSCREEN) || 
            (gfx_driver->id == GFX_OPENGL_WINDOWED)));

#endif   /* USE_ALLEGROGL */

   /* Not built with AllegroGL. */
   return (FALSE);
}

void video_set_filter_list (LIST filters)
{
    filter_list = filters;


    clear (screen_buffer);
}


LIST video_get_filter_list (void)
{
    return (filter_list);
}


void video_filter (void)
{
    int y;


    if (filter_list & VIDEO_FILTER_SCANLINES_HIGH)
    {
        for (y = 0; y < screen_buffer -> h; y += 2)
        {
            hline (screen_buffer, blit_x_offset, y, screen_buffer -> w, 0);
        }
    }

    if (filter_list & VIDEO_FILTER_SCANLINES_MEDIUM)
    {
        set_trans_blender (0, 0, 0, 127);


        drawing_mode (DRAW_MODE_TRANS, NULL, 0, 0);


        for (y = 0; y < screen_buffer -> h; y += 2)
        {
            hline (screen_buffer, blit_x_offset, y, screen_buffer -> w, makecol (0, 0, 0));
        }


        solid_mode ();
    }

    if (filter_list & VIDEO_FILTER_SCANLINES_LOW)
    {
        set_trans_blender (0, 0, 0, 63);


        drawing_mode (DRAW_MODE_TRANS, NULL, 0, 0);


        for (y = 0; y < screen_buffer -> h; y += 2)
        {
            hline (screen_buffer, blit_x_offset, y, screen_buffer -> w, makecol (0, 0, 0));
        }


        solid_mode ();
    }
}


void video_message (const UCHAR *message, ...)
{
    va_list format;


    int index;


    USTRING buffer;


    va_start (format, message);

    uvszprintf (buffer, USTRING_SIZE, message, format);

    va_end (format);


    for (index = 0; index < (MAX_MESSAGES - 1); index ++)
    {
        ustrzcpy (&video_messages [index] [0], USTRING_SIZE, &video_messages [(index + 1)] [0]);
    }


    ustrzcpy (&video_messages [(MAX_MESSAGES - 1)] [0], USTRING_SIZE, buffer);


    log_printf ("%s\n", buffer);
}


static INLINE int get_messages_height (void)
{
    int index;


    int height = 0;


    for (index = 0; index < MAX_MESSAGES; index ++)
    {
        int length;


        length = text_length (font, &video_messages [index] [0]);


        if (length > (screen_buffer->w - 8))
        {
            height += ((text_height (font) + 1) * 2);
        }
        else
        {
            height += (text_height (font) + 1);
        }
    }


    return (height);
}


static void draw_messages (void)
{
    int index;


    int x;

    int y;


    int x2;

    int y2;


    int height;


    int height_text;


    int gray;

    int silver;


    BOOL box = (input_mode & INPUT_MODE_CHAT);


    height = get_messages_height ();


    height_text = text_height (font);


    gray = video_create_color (127, 127, 127);

    silver = video_create_color (191, 191, 191);


    x = 0;

    y = ((screen_buffer->h - (((height_text + 6) + height) + 3)) - 1);


    x2 = (screen_buffer->w - 1);

    y2 = (screen_buffer->h - 1);


    if (box)
    {
        drawing_mode (DRAW_MODE_TRANS, NULL, 0, 0);
    
        rectfill (screen_buffer, x, y, x2, y2, gray);
    
    
        solid_mode ();
    
        rect (screen_buffer, x, y, x2, y2, VIDEO_COLOR_WHITE);
    }


    x = 3;

    y ++;


    x2 = ((screen_buffer->w - 1) - 1);


    if (box)
    {
        hline (screen_buffer, x, y, x2, VIDEO_COLOR_BLACK);
    }


    x = 0;

    y = ((screen_buffer->h - (height_text + 5)) - 1);


    if (box)
    {
        hline (screen_buffer, x, y, x2, VIDEO_COLOR_WHITE);
    }


    x = 3;

    y ++;


    if (box)
    {
        hline (screen_buffer, x, y, x2, VIDEO_COLOR_BLACK);
    }


    x = 4;

    y = ((screen_buffer->h - ((height_text + 6) + height)) - 1);


    x2 = ((screen_buffer->w - 4) - 1);


    for (index = 0; index < MAX_MESSAGES; index ++)
    {
        UINT8 * token;


        USTRING buffer;


        memcpy (buffer, &video_messages [index] [0], USTRING_SIZE);


        for (token = strtok (&video_messages [index] [0], " "); token; token = strtok (NULL, " "))
        {
            int length;


            length = text_length (font, token);


            if ((x + length) > x2)
            {
                x = 9;

                y += (height_text + 1);
            }

            
            if (index == (MAX_MESSAGES - 1))
            {
                shadow_textout (screen_buffer, font, token, x, y, VIDEO_COLOR_WHITE);
            }
            else if (box)
            {
                shadow_textout (screen_buffer, font, token, x, y, silver);
            }


            x += (length + (text_length (font, " ") + 1));
        }


        memcpy (&video_messages [index] [0], buffer, USTRING_SIZE);


        x = 4;

        y += (height_text + 1);
    }


    y = ((screen_buffer->h - (height_text + 2)) - 1);


    if (box)
    {
        USTRING buffer;

        /* TODO: Make this message scroll horizontally when it is too long
           to be displayed entirely. */

        USTRING_CLEAR(buffer);
        ustrncat (buffer, input_chat_text, (sizeof (buffer) - 1));
        ustrncat (buffer, "_", (sizeof (buffer) - 1));

        shadow_textout (screen_buffer, font, buffer, x, y, VIDEO_COLOR_WHITE);
    }
}


static void erase_messages (void)
{
    int x;

    int y;


    int x2;

    int y2;


    int height;


    int height_text;


    height = get_messages_height ();


    height_text = text_height (font);


    x = 0;

    y = ((screen_buffer->h - (((height_text + 6) + height) + 3)) - 1);


    x2 = (screen_buffer->w - 1);

    y2 = (screen_buffer->h - 1);


    rectfill (screen_buffer, x, y, x2, y2, VIDEO_COLOR_BLACK);
}
