#include "blit/shared.h"

/* Blitters. */

static void blit_normal (BITMAP *src, BITMAP *dest, int x_base, int y_base)
{
   RT_ASSERT(src);
   RT_ASSERT(dest);

   if (!blitter_size_check (dest, 256, 240))
      return;

   blit (src, dest, 0, 0, x_base, y_base, src->w, src->h);
}

static int stretch_width  = 512;
static int stretch_height = 480;

static void blit_stretched (BITMAP *src, BITMAP *dest, int x_base, int
   y_base)
{
   /* Stretched blitter.

      blit_buffer_in - BITMAP* to a bitmap the size of 'src' used for color
                       conversion (allocated by init_stretched()).

      stretch_width  - Output width.
      stretch_height - Output height.
      */

   RT_ASSERT(src);
   RT_ASSERT(dest);

   if (!blitter_size_check (dest, stretch_width, stretch_height))
      return;

   if (color_depth != 8)
   {
      BITMAP *buffer;

      if (!blit_buffer_in)
         WARN_BREAK_GENERIC();

      buffer = (BITMAP *)blit_buffer_in;

      blit (src, buffer, 0, 0, 0, 0, src->w, src->h);

      stretch_blit (buffer, dest, 0, 0, buffer->w, buffer->h, x_base,
         y_base, stretch_width, stretch_height);
   }
   else
   {
      stretch_blit (src, dest, 0, 0, src->w, src->h, x_base, y_base,
         stretch_width, stretch_height);
   }
}

/* Initializers. */

static void init_normal (BITMAP *src, BITMAP *dest)
{
   RT_ASSERT(src);
   RT_ASSERT(dest);

   blit_x_offset = ((dest->w / 2) - (src->w / 2));
   blit_y_offset = ((dest->h / 2) - (src->h / 2));
}

static void init_stretched (BITMAP *src, BITMAP *dest)
{
   RT_ASSERT(src);
   RT_ASSERT(dest);

   /* Load configuration. */

   stretch_width  = get_config_int ("video", "stretch_width",  stretch_width);
   stretch_height = get_config_int ("video", "stretch_height", stretch_height);

   if (color_depth != 8)
   {
      blit_buffer_in = (void *)create_bitmap (src->w, src->h);
      if (!blit_buffer_in)
         WARN_BREAK_GENERIC();
   }

   blit_x_offset = ((dest->w / 2) - (stretch_width / 2));
   blit_y_offset = ((dest->h / 2) - (stretch_height / 2));
}

/* Deinitializers. */

static void deinit_stretched (void)
{
   /* Destroy buffer. */

   if (blit_buffer_in)
      destroy_bitmap ((BITMAP *)blit_buffer_in);

   blit_buffer_in = NULL;
}

/* Interfaces. */

static const BLITTER blitter_normal =
{
   init_normal, NULL,
   blit_normal
};

static const BLITTER blitter_stretched =
{
   init_stretched, deinit_stretched,
   blit_stretched
};
