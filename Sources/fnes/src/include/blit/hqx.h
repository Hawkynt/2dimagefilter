#include "blit/shared.h"
#include "etc/hqx.h"

/* Blitter framework. */

static INLINE void _blit_hqx (int size, BITMAP *src, BITMAP *dest, int
   x_base, int y_base)
{
   unsigned short *in;
   int *out;
   int w, h, wm, hm;
   int y;

   RT_ASSERT(src);
   RT_ASSERT(dest);

   if ((size != 2) && (size != 3) && (size != 4))
      WARN_BREAK_GENERIC();
   if ((size == 2) && (!blitter_size_check (dest, 512, 480)))
      return;
   if ((size == 3) && (!blitter_size_check (dest, 768, 720)))
      return;
   if ((size == 4) && (!blitter_size_check (dest, 1024, 960)))
      return;

   /* Check buffers. */
   if (!blit_buffer_in || !blit_buffer_out)
      WARN_BREAK_GENERIC();

   /* Set buffers. */
   in  = (unsigned short *)blit_buffer_in;
   out = (int            *)blit_buffer_out;

   /* Calculate sizes. */
   w = src->w;
   h = src->h;
   wm = (src->w * size);
   hm = (src->h * size);

   /* Import source bitmap to input buffer. */
   for (y = 0; y < h; y++)
   {
      int x;

      for (x = 0; x < w; x++)
      {
         UINT8 c;
         UINT8 r, g, b;

         c = FAST_GETPIXEL8(src, x, y);

         r = ((getr8 (c) >> 3) & 0x1f);
         g = ((getg8 (c) >> 2) & 0x3f);
         b = ((getb8 (c) >> 3) & 0x1f);

         in[((y * w) + x)] = ((r << 11) | (g << 5) | b);
      }
   }

   switch (size)
   {
      case 2:
      {
         /* Perform an HQ2X filtering operation. */

         hq2x ((unsigned char *)in, (unsigned char *)out, w, h, (wm * sizeof
            (int)));

         break;
      }

      case 3:
      {
         /* Perform an HQ3X filtering operation. */

         hq3x ((unsigned char *)in, (unsigned char *)out, w, h, (wm * sizeof
            (int)));

         break;
      }

      case 4:
      {
         /* Perform an HQ4X filtering operation. */

         hq4x ((unsigned char *)in, (unsigned char *)out, w, h, (wm * sizeof
            (int)));

         break;
      }

      default:
         WARN_GENERIC();
   }


   /* Export out buffer to destination bitmap. */
   for (y = 0; y < hm; y++)
   {
      int yo = (y_base + y);
      int x;

      for (x = 0; x < wm; x++)
      {
         int xo = (x_base + x);
         int c;
         UINT8 r, g, b;
         int d;

         c = out[((y * wm) + x)];

         r = ((c >> 16) & 0xff);
         g = ((c >> 8) & 0xff);
         b = (c & 0xff);

         d = video_create_color (r, g, b);

         switch (color_depth)
         {
            case 8:
            {
               FAST_PUTPIXEL8(dest, xo, yo, d);

               break;
            }

            case 15:
            case 16:
            {
               FAST_PUTPIXEL16(dest, xo, yo, d);

               break;
            }

            case 24:
            {
               FAST_PUTPIXEL24(dest, xo, yo, d);

               break;
            }

            case 32:
            {
               FAST_PUTPIXEL32(dest, xo, yo, d);

               break;
            }

            default:
               WARN_GENERIC();
         }
      }
   }
}

/* Initializer framework. */

static INLINE void _init_hqx (int size, BITMAP *src, BITMAP *dest)
{
   int w, h, wm, hm;

   RT_ASSERT(src);
   RT_ASSERT(dest);

   /* Calculate sizes. */
   w = src->w;
   h = src->h;
   wm = (src->w * size);
   hm = (src->h * size);

   /* Allocate input buffer. */
   blit_buffer_in = malloc (((w * h) * sizeof (unsigned short)));
   if (!blit_buffer_in)
      WARN_BREAK_GENERIC ();

   /* Allocate output buffer. */
   blit_buffer_out = malloc (((wm * hm) * sizeof (int)));
   if (!blit_buffer_out)
   {
      WARN_GENERIC();
      free (blit_buffer_in);
      return;
   }

   blit_x_offset = ((dest->w / 2) - (wm / 2));
   blit_y_offset = ((dest->h / 2) - (hm / 2));
}

/* Deinitializer. */

static void deinit_hqx (void)
{
   /* Destroy buffers. */

   if (blit_buffer_in)
      free (blit_buffer_in);
   if (blit_buffer_out)
      free (blit_buffer_out);

   blit_buffer_in  = NULL;
   blit_buffer_out = NULL;
}

/* Wrappers. */
/* TODO: Maybe wrap these in macros to reduce code redundancy? */

static void init_hq2x (BITMAP *src, BITMAP *dest)
{
   // RT_ASSERT(src);
   // RT_ASSERT(dest);

   _init_hqx (2, src, dest);
}

static void blit_hq2x (BITMAP *src, BITMAP *dest, int x_base, int y_base)
{
   // RT_ASSERT(src);
   // RT_ASSERT(dest);

   _blit_hqx (2, src, dest, x_base, y_base);
}

static void init_hq3x (BITMAP *src, BITMAP *dest)
{
   // RT_ASSERT(src);
   // RT_ASSERT(dest);

   _init_hqx (3, src, dest);
}

static void blit_hq3x (BITMAP *src, BITMAP *dest, int x_base, int y_base)
{
   // RT_ASSERT(src);
   // RT_ASSERT(dest);

   _blit_hqx (3, src, dest, x_base, y_base);
}

static void init_hq4x (BITMAP *src, BITMAP *dest)
{
   // RT_ASSERT(src);
   // RT_ASSERT(dest);

   _init_hqx (4, src, dest);
}

static void blit_hq4x (BITMAP *src, BITMAP *dest, int x_base, int y_base)
{
   // RT_ASSERT(src);
   // RT_ASSERT(dest);

   _blit_hqx (4, src, dest, x_base, y_base);
}

/* Interfaces. */

static const BLITTER blitter_hq2x =
{
   init_hq2x, deinit_hqx,
   blit_hq2x
};

static const BLITTER blitter_hq3x =
{
   init_hq3x, deinit_hqx,
   blit_hq3x
};

static const BLITTER blitter_hq4x =
{
   init_hq4x, deinit_hqx,
   blit_hq4x
};
