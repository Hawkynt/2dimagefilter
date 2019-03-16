#include "blit/shared.h"

/* Blitters. */

static void blit_des (BITMAP *src, BITMAP *dest, int x_base, int y_base)
{
   int y;

   RT_ASSERT(src);
   RT_ASSERT(dest);

   if (!blitter_size_check (dest, 256, 240))
      return;

   for (y = 0; y < src->h; y++)
   {
      int yo = (y_base + y);
      int x;

      for (x = 0; x < src->w; x++)
      {
         int xo = (x_base + x);
         UINT8 c;
         UINT8 w = 0, e = 0, s = 0, n = 0;
         int p[4];
         int i;
         int r = 0, g = 0, b = 0;
         int d;

         c = FAST_GETPIXEL8(src, x, y);

         if (x > 0)              w = FAST_GETPIXEL8(src, (x - 1), y);
         if ((x + 1) < src->w)   e = FAST_GETPIXEL8(src, (x + 1), y);
         if ((y + 1) < src->h)   s = FAST_GETPIXEL8(src, x, (y + 1));
         if (y > 0)              n = FAST_GETPIXEL8(src, x, (y - 1));

         p[0] = (((w == n) && (n != e) && (w != s)) ? w : c);
         p[1] = (((n == e) && (n != w) && (e != s)) ? e : c);
         p[2] = (((w == s) && (w != n) && (s != e)) ? w : c);
         p[3] = (((s == e) && (w != s) && (n != e)) ? e : c);

         for (i = 0; i < 4; i++)
         {  
            r += getr8 (p[i]);
            g += getg8 (p[i]);
            b += getb8 (p[i]);
         }

         r /= 4;
         g /= 4;
         b /= 4;

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

static void blit_desii (BITMAP *src, BITMAP *dest, int x_base, int y_base)
{
   int y;

   RT_ASSERT(src);
   RT_ASSERT(dest);

   if (!blitter_size_check (dest, 512, 480))
      return;

   for (y = 0; y < src->h; y++)
   {
      int yo = (y_base + (y * 2));
      int x;

      for (x = 0; x < src->w; x++)
      {
         int xo = (x_base + (x * 2));
         UINT8 c;
         UINT8 w = 0, e = 0, s = 0, n = 0, se = 0;
         int cx;
         int ce, cs, cse;
         int p[4];
         int i;
         int d1, d2, d3, d4;
         int xi, yi;

         c = FAST_GETPIXEL8(src, x, y);

         if (x > 0)              w = FAST_GETPIXEL8(src, (x - 1), y);
         if ((x + 1) < src->w)   e = FAST_GETPIXEL8(src, (x + 1), y);
         if ((y + 1) < src->h)   s = FAST_GETPIXEL8(src, x, (y + 1));
         if (y > 0)              n = FAST_GETPIXEL8(src, x, (y - 1));

         if (((x + 1) < src->w) && ((y + 1) < src->h))
            se = FAST_GETPIXEL8(src, (x + 1), (y + 1));

         if (color_depth == 8)
            cx = c;
         else
            cx = palette_color[c];

         ce  = mixpal (c, e);
         cs  = mixpal (c, s);
         cse = mixpal (c, se);

         p[0] = (((w == n) && (n != e) && (w != s)) ? w : c);
         p[1] = (((n == e) && (n != w) && (e != s)) ? e : c);
         p[2] = (((w == s) && (w != n) && (s != e)) ? w : c);
         p[3] = (((s == e) && (w != s) && (n != e)) ? e : c);

         if (color_depth > 8)
         {
            for (i = 0; i < 4; i++)
               p[i] = palette_color[p[i]];
         }

         d1 = mix (p[0], cx);
         d2 = mix (p[1], ce);
         d3 = mix (p[2], cs);
         d4 = mix (p[3], cse);

         xi = (xo + 1);
         yi = (yo + 1);

         switch (color_depth)
         {
            case 8:
            {
               FAST_PUTPIXEL8(dest, xo, yo, d1);
               FAST_PUTPIXEL8(dest, xi, yo, d2);
               FAST_PUTPIXEL8(dest, xo, yi, d3);
               FAST_PUTPIXEL8(dest, xi, yi, d4);

               break;
            }

            case 15:
            case 16:
            {
               FAST_PUTPIXEL16(dest, xo, yo, d1);
               FAST_PUTPIXEL16(dest, xi, yo, d2);
               FAST_PUTPIXEL16(dest, xo, yi, d3);
               FAST_PUTPIXEL16(dest, xi, yi, d4);
                                         
               break;
            }

            case 24:
            {
               FAST_PUTPIXEL24(dest, xo, yo, d1);
               FAST_PUTPIXEL24(dest, xi, yo, d2);
               FAST_PUTPIXEL24(dest, xo, yi, d3);
               FAST_PUTPIXEL24(dest, xi, yi, d4);

               break;
            }

            case 32:
            {
               FAST_PUTPIXEL32(dest, xo, yo, d1);
               FAST_PUTPIXEL32(dest, xi, yo, d2);
               FAST_PUTPIXEL32(dest, xo, yi, d3);
               FAST_PUTPIXEL32(dest, xi, yi, d4);

               break;
            }

            default:
               WARN_GENERIC();
         }
      }
   }
}

/* Initializers. */

static void init_des (BITMAP *src, BITMAP *dest)
{
   RT_ASSERT(src);
   RT_ASSERT(dest);

   blit_x_offset = ((dest->w / 2) - (src->w / 2));
   blit_y_offset = ((dest->h / 2) - (src->h / 2));
}

static void init_desii (BITMAP *src, BITMAP *dest)
{
   RT_ASSERT(src);
   RT_ASSERT(dest);

   blit_x_offset = ((dest->w / 2) - ((src->w * 2) / 2));
   blit_y_offset = ((dest->h / 2) - ((src->h * 2) / 2));
}

/* Interfaces. */

static const BLITTER blitter_des =
{
   init_des, NULL,
   blit_des
};

static const BLITTER blitter_desii =
{
   init_desii, NULL,
   blit_desii
};
