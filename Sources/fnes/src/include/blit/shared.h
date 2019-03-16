#ifndef BLIT_SHARED_H_INCLUDED
#define BLIT_SHARED_H_INCLUDED

/* Blitter structure. */

typedef struct _BLITTER
{
   void (*init) (BITMAP *, BITMAP *);
   void (*deinit) (void);
   void (*blit) (BITMAP *, BITMAP *, int, int);

} BLITTER;

/* Pixel access macros. */

#define FAST_GETPIXEL8(bmp, x, y)      (bmp->line[y][x])
#define FAST_GETPIXEL32(bmp, x, y)     (((UINT32 *)bmp->line[y])[x])

#define FAST_PUTPIXEL8(bmp, x, y, c)   (bmp->line[y][x] = c)
#define FAST_PUTPIXEL16(bmp, x, y, c)  (((UINT16 *)bmp->line[y])[x] = c)
#define FAST_PUTPIXEL24(bmp, x, y, c)  (_putpixel24 (bmp, x, y, c))
#define FAST_PUTPIXEL32(bmp, x, y, c)  (((UINT32 *)bmp->line[y])[x] = c)

/* Utility functions. */

static INLINE BOOL blitter_size_check (BITMAP *bmp, int width, int height)
{
   int y;

   if ((bmp->w >= width) && (bmp->h >= height))
      return (TRUE);

   y = ((bmp->h / 2) - (text_height (font) / 2));

   textout_centre_ex (bmp, font, "Your buffer is too small.", (bmp->w / 2),
      y, VIDEO_COLOR_WHITE, -1);

   y += ((text_height (font) + 1) * 2);

   textprintf_centre_ex (bmp, font, (bmp->w / 2), y, VIDEO_COLOR_WHITE, -1,
      "It must be %dx%d pixels or larger", width, height);

   y += (text_height (font) + 1);

   textprintf_centre_ex (bmp, font, (bmp->w / 2), y, VIDEO_COLOR_WHITE, -1,
      "to use this blitter.");
                                                                                                                \
   return (FALSE);
}

static INLINE int mixpal (int color_a, int color_b)
{
   /* Paletted mixing routine (3:1). */

   const RGB *ca = &internal_palette[color_a];
   const RGB *cb = &internal_palette[color_b];
   int r, g, b;

   /* 0 - 63 --> 0 - 127. */
   r = ((ca->r * 3) + cb->r);
   g = ((ca->g * 3) + cb->g);
   b = ((ca->b * 3) + cb->b);

   return (video_create_color (r, g, b));
}

static INLINE int mix (int color_a, int color_b)
{
   /* True color mixing routine (3:1). */

   int r, g, b;

   r = (getr (color_a) * 3);
   g = (getg (color_a) * 3);
   b = (getb (color_a) * 3);

   r += getr (color_b);
   g += getg (color_b);
   b += getb (color_b);

   r /= 4;
   g /= 4;
   b /= 4;

   return (video_create_color (r, g, b));
}

#define RED_CMASK_16       0xF800
#define GREEN_CMASK_16     0x07E0
#define BLUE_CMASK_16      0X001F

static INLINE unsigned mix16 (unsigned c1, unsigned c2)
{
   return ((((((c1 & RED_CMASK_16) + (c2 & RED_CMASK_16)) >> 1) & RED_CMASK_16) |
            ((((c1 & GREEN_CMASK_16) + (c2 & GREEN_CMASK_16)) >> 1) & GREEN_CMASK_16) |
            ((((c1 & BLUE_CMASK_16) + (c2 & BLUE_CMASK_16)) >> 1) & BLUE_CMASK_16)));
}

static INLINE int unmix (int color_a, int color_b)
{
   /* A variant of an unsharp mask, without the blur part. */

   int ra, ga, ba;
   int rb, gb, bb;
   int r, g, b;

   ra = getr (color_a);
   ga = getg (color_a);
   ba = getb (color_a);

   rb = getr (color_b);
   gb = getg (color_b);
   bb = getb (color_b);

   r = ((fix ((ra + (ra - rb)), 0, 255) + rb) >> 1);
   g = ((fix ((ga + (ga - gb)), 0, 255) + gb) >> 1);
   b = ((fix ((ba + (ba - bb)), 0, 255) + bb) >> 1);

   return (video_create_color (r, g, b));
}

#endif   /* !BLIT_SHARED_H_INCLUDED */
