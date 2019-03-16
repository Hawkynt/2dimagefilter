#include "blit/shared.h"

/* Blitters. */

static void blit_2xscl (BITMAP *src, BITMAP *dest, int x_base, int y_base)
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
         UINT8 w = 0, e = 0, s = 0, n = 0;
         UINT8 p1, p2, p3, p4;
         int xi, yi;

         c = FAST_GETPIXEL8(src, x, y);

         if (x > 0)              w = FAST_GETPIXEL8(src, (x - 1), y);
         if ((x + 1) < src->w)   e = FAST_GETPIXEL8(src, (x + 1), y);
         if ((y + 1) < src->h)   s = FAST_GETPIXEL8(src, x, (y + 1));
         if (y > 0)              n = FAST_GETPIXEL8(src, x, (y - 1));

         p1 = (((w == n) && (n != e) && (w != s)) ? w : c);
         p2 = (((n == e) && (n != w) && (e != s)) ? e : c);
         p3 = (((w == s) && (w != n) && (s != e)) ? w : c);
         p4 = (((s == e) && (w != s) && (n != e)) ? e : c);

         xi = (xo + 1);
         yi = (yo + 1);

         if (color_depth == 8)
         {
            FAST_PUTPIXEL8(dest, xo, yo, p1);
            FAST_PUTPIXEL8(dest, xi, yo, p2);
            FAST_PUTPIXEL8(dest, xo, yi, p3);
            FAST_PUTPIXEL8(dest, xi, yi, p4);
         }
         else
         {
            int d1, d2, d3, d4;

            d1 = palette_color[p1];
            d2 = palette_color[p2];
            d3 = palette_color[p3];
            d4 = palette_color[p4];

            switch (color_depth)
            {
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
}

static void blit_super_2xscl (BITMAP *src, BITMAP *dest, int x_base, int
   y_base)
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
         UINT8 w = 0, e = 0, s = 0, n = 0;
         int wx, ex;
         int cw, ce;
         int p1, p2, p3, p4;
         int xi, yi;

         c = FAST_GETPIXEL8(src, x, y);

         if (x > 0)              w = FAST_GETPIXEL8(src, (x - 1), y);
         if ((x + 1) < src->w)   e = FAST_GETPIXEL8(src, (x + 1), y);
         if ((y + 1) < src->h)   s = FAST_GETPIXEL8(src, x, (y + 1));
         if (y > 0)              n = FAST_GETPIXEL8(src, x, (y - 1));

         if (color_depth == 8)
         {
            wx = w;
            ex = e;
         }
         else
         {
            wx = palette_color[w];
            ex = palette_color[e];
         }

         cw = mixpal (c, w);
         ce = mixpal (c, e);

         p1 = (((w == n) && (n != e) && (w != s)) ? wx : cw);
         p2 = (((n == e) && (n != w) && (e != s)) ? ex : ce);
         p3 = (((w == s) && (w != n) && (s != e)) ? wx : cw);
         p4 = (((s == e) && (w != s) && (n != e)) ? ex : ce);

         xi = (xo + 1);
         yi = (yo + 1);

         switch (color_depth)
         {
            case 8:
            {
               FAST_PUTPIXEL8(dest, xo, yo, p1);
               FAST_PUTPIXEL8(dest, xi, yo, p2);
               FAST_PUTPIXEL8(dest, xo, yi, p3);
               FAST_PUTPIXEL8(dest, xi, yi, p4);

               break;
            }

            case 15:
            case 16:
            {
               FAST_PUTPIXEL16(dest, xo, yo, p1);
               FAST_PUTPIXEL16(dest, xi, yo, p2);
               FAST_PUTPIXEL16(dest, xo, yi, p3);
               FAST_PUTPIXEL16(dest, xi, yi, p4);

               break;
            }

            case 24:
            {
               FAST_PUTPIXEL24(dest, xo, yo, p1);
               FAST_PUTPIXEL24(dest, xi, yo, p2);
               FAST_PUTPIXEL24(dest, xo, yi, p3);
               FAST_PUTPIXEL24(dest, xi, yi, p4);

               break;
            }

            case 32:
            {
               FAST_PUTPIXEL32(dest, xo, yo, p1);
               FAST_PUTPIXEL32(dest, xi, yo, p2);
               FAST_PUTPIXEL32(dest, xo, yi, p3);
               FAST_PUTPIXEL32(dest, xi, yi, p4);

               break;
            }

            default:
               WARN_GENERIC();
         }
      }
   }
}

static void blit_ultra_2xscl (BITMAP *src, BITMAP *dest, int x_base,
    int y_base)
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
         UINT8 w = 0, e = 0, s = 0, n = 0;
         int cx, wx, ex;
         int cw, ce;
         int p1, p2, p3, p4;
         int xi, yi;

         c = FAST_GETPIXEL8(src, x, y);

         if (x > 0)              w = FAST_GETPIXEL8(src, (x - 1), y);
         if ((x + 1) < src->w)   e = FAST_GETPIXEL8(src, (x + 1), y);
         if ((y + 1) < src->h)   s = FAST_GETPIXEL8(src, x, (y + 1));
         if (y > 0)              n = FAST_GETPIXEL8(src, x, (y - 1));

         if (color_depth == 8)
         {
            cx = c;
            wx = w;
            ex = e;
         }
         else
         {
            cx = palette_color[c];
            wx = palette_color[w];
            ex = palette_color[e];
         }

         cw = mixpal (c, w);
         ce = mixpal (c, e);

         p1 = (((w == n) && (n != e) && (w != s)) ? wx : cw);
         p2 = (((n == e) && (n != w) && (e != s)) ? ex : ce);
         p3 = (((w == s) && (w != n) && (s != e)) ? wx : cw);
         p4 = (((s == e) && (w != s) && (n != e)) ? ex : ce);

         p1 = unmix (p1, cx);
         p2 = unmix (p2, cx);
         p3 = unmix (p3, cx);
         p4 = unmix (p4, cx);

         xi = (xo + 1);
         yi = (yo + 1);

         switch (color_depth)
         {
            case 8:
            {
               FAST_PUTPIXEL8(dest, xo, yo, p1);
               FAST_PUTPIXEL8(dest, xi, yo, p2);
               FAST_PUTPIXEL8(dest, xo, yi, p3);
               FAST_PUTPIXEL8(dest, xi, yi, p4);

               break;
            }

            case 15:
            case 16:
            {
               FAST_PUTPIXEL16(dest, xo, yo, p1);
               FAST_PUTPIXEL16(dest, xi, yo, p2);
               FAST_PUTPIXEL16(dest, xo, yi, p3);
               FAST_PUTPIXEL16(dest, xi, yi, p4);

               break;
            }

            case 24:
            {
               FAST_PUTPIXEL24(dest, xo, yo, p1);
               FAST_PUTPIXEL24(dest, xi, yo, p2);
               FAST_PUTPIXEL24(dest, xo, yi, p3);
               FAST_PUTPIXEL24(dest, xi, yi, p4);

               break;
            }

            case 32:
            {
               FAST_PUTPIXEL32(dest, xo, yo, p1);
               FAST_PUTPIXEL32(dest, xi, yo, p2);
               FAST_PUTPIXEL32(dest, xo, yi, p3);
               FAST_PUTPIXEL32(dest, xi, yi, p4);

               break;
            }

            default:
               WARN_GENERIC();
         }
      }
   }
}

/* Initializer. */

static void init_2xscl (BITMAP *src, BITMAP *dest)
{
   RT_ASSERT(src);
   RT_ASSERT(dest);

   blit_x_offset = ((dest->w / 2) - ((src->w * 2) / 2));
   blit_y_offset = ((dest->h / 2) - ((src->h * 2) / 2));
}

/* Interfaces. */

static const BLITTER blitter_2xscl =
{
   init_2xscl, NULL,
   blit_2xscl
};

static const BLITTER blitter_super_2xscl =
{
   init_2xscl, NULL,
   blit_super_2xscl
};

static const BLITTER blitter_ultra_2xscl =
{
   init_2xscl, NULL,
   blit_super_2xscl
};
