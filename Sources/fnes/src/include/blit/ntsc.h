#include "blit/shared.h"
#include "etc/snes_ntsc.h"

/* Variables. */

static snes_ntsc_t       _ntsc_ntsc;
static snes_ntsc_setup_t _ntsc_setup;
static int               _ntsc_phase;
static int               _ntsc_scanline_doubling;
static BOOL              _ntsc_interpolation;

/* Blitter. */

static void blit_ntsc (BITMAP *src, BITMAP *dest, int x_base, int y_base)
{
   int w, h, wm, hm, hx;
   unsigned short *in;
   unsigned short *out;
   int y;
             
   RT_ASSERT(src);
   RT_ASSERT(dest);

   /* Calculate sizes. */
   w = src->w;
   h = src->h;
   wm = SNES_NTSC_OUT_WIDTH(w);
   hm = 240;         /* Output height from ntsc_blit(). */
   hx = (240 * 2);   /* Output height from blit_ntsc(). */

   if (!blitter_size_check (dest, wm, hx))
      return;

   /* Check buffers. */
   if (!blit_buffer_in || !blit_buffer_out)
      WARN_BREAK_GENERIC();

   /* Set buffers. */
   in  = (unsigned short *)blit_buffer_in;
   out = (unsigned short *)blit_buffer_out;

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

   /* Force 'phase' to 0 if 'setup.merge_fields' is on. */
   _ntsc_phase ^= 1;
   if (_ntsc_setup.merge_fields)
      _ntsc_phase = 0;

   /* Perform an NTSC filtering operation. */
   snes_ntsc_blit (&_ntsc_ntsc, in, w, _ntsc_phase, w, h, out, (wm * 2));

   /* Export out buffer to destination bitmap. */
   for (y = 0; y < hm; y++)
   {
      int yo = (y_base + (y * 2));
      int x;

      for (x = 0; x < wm; x++)
      {
         int xo = (x_base + x);
         int c;
         int r, g, b;
         BOOL modify = FALSE;
         int d1, d2 = d2;  /* Kill warning. */
         int yi;

         c = out[((y * wm) + x)];

         r = (((c >> 11) & 0x1f) << 3);
         g = (((c >> 5) & 0x3f) << 2);
         b = ((c & 0x1f) << 3);

         d1 = video_create_color (r, g, b);

         if (_ntsc_interpolation)
         {
            /* Interpolate missing scanlines. */
            
            if (y < (hm - 1))
            {
               c = out[(((y + 1) * wm) + x)];
   
               r += (((c >> 11) & 0x1f) << 3);
               g += (((c >> 5) & 0x3f) << 2);
               b += ((c & 0x1f) << 3);
   
               r /= 2;
               g /= 2;
               b /= 2;
   
               modify = TRUE;
            }
         }

         switch (_ntsc_scanline_doubling)
         {
            case 0:  /* Normal. */
               break;

            case 1:  /* Brighten. */
            {
               /* Supposed to be 8.33, but we approximate to avoid floating
                  point math here. */
               r = fix ((r + (r / 8)), 0, 255);
               g = fix ((g + (g / 8)), 0, 255);
               b = fix ((b + (b / 8)), 0, 255);

               modify = TRUE;

               break;
            }

            case 2:  /* Darken. */
            {
               r = fix ((r - (r / 8)), 0, 255);
               g = fix ((g - (g / 8)), 0, 255);
               b = fix ((b - (b / 8)), 0, 255);

               modify = TRUE;

               break;
            }

            default:
               WARN_GENERIC();
         }

         if (modify)
            d2 = video_create_color (r, g, b);
         else
            d2 = d1;

         yi = (yo + 1);

         switch (color_depth)
         {
            case 8:
            {                                
               FAST_PUTPIXEL8(dest, xo, yo, d1);
               FAST_PUTPIXEL8(dest, xo, yi, d2);

               break;
            }

            case 15:
            case 16:
            {
               FAST_PUTPIXEL16(dest, xo, yo, d1);
               FAST_PUTPIXEL16(dest, xo, yi, d2);

               break;
            }

            case 24:
            {
               FAST_PUTPIXEL24(dest, xo, yo, d1);
               FAST_PUTPIXEL24(dest, xo, yi, d2);

               break;
            }

            case 32:
            {
               FAST_PUTPIXEL32(dest, xo, yo, d1);
               FAST_PUTPIXEL32(dest, xo, yi, d2);

               break;
            }

            default:
               WARN_GENERIC();
         }
      }
   }
}

/* Initializer. */

static void init_ntsc (BITMAP *src, BITMAP *dest)
{
   int preset;
   int merge_fields, doubling, interpolation;
   snes_ntsc_setup_t *setup;
   int w, h, wm, hm, hx;
   
   RT_ASSERT(src);
   RT_ASSERT(dest);

   /* Get setup structure. */
   setup = &_ntsc_setup;

   /* Load configuration. */

   preset = get_config_int ("ntsc", "preset", -1);

   if (preset != -1)
   {
      const snes_ntsc_setup_t *presets[5];

      /* Load a preset. */

      preset = fix (preset, 0, 4);

      presets[0] = &snes_ntsc_composite;   /* Default. */
      presets[1] = &snes_ntsc_composite;
      presets[2] = &snes_ntsc_svideo;
      presets[3] = &snes_ntsc_rgb;
      presets[4] = &snes_ntsc_monochrome;

      memcpy (setup, presets[preset], sizeof (snes_ntsc_setup_t));

      set_config_int ("ntsc", "preset",      -1);
      set_config_int ("ntsc", "hue",         ROUND(setup->hue         * 100.0f));
      set_config_int ("ntsc", "saturation",  ROUND(setup->saturation  * 100.0f));
      set_config_int ("ntsc", "contrast",    ROUND(setup->contrast    * 100.0f));
      set_config_int ("ntsc", "brightness",  ROUND(setup->brightness  * 100.0f));
      set_config_int ("ntsc", "sharpness",   ROUND(setup->sharpness   * 100.0f));
      set_config_int ("ntsc", "gamma",       ROUND(setup->gamma       * 100.0f));
      set_config_int ("ntsc", "resolution",  ROUND(setup->resolution  * 100.0f));
      set_config_int ("ntsc", "artifacts",   ROUND(setup->artifacts   * 100.0f));
      set_config_int ("ntsc", "fringing",    ROUND(setup->fringing    * 100.0f));
      set_config_int ("ntsc", "bleed",       ROUND(setup->bleed       * 100.0f));
      set_config_int ("ntsc", "hue_warping", ROUND(setup->hue_warping * 100.0f));
   }
   else
   {
      int hue, saturation, contrast, brightness, sharpness, hue_warping,
         gamma, resolution, artifacts, fringing, bleed;

      /* All parameters range from -100 to +100. */
   
      hue          = get_config_int ("ntsc", "hue",          0);
      saturation   = get_config_int ("ntsc", "saturation",   0);
      contrast     = get_config_int ("ntsc", "contrast",     0);
      brightness   = get_config_int ("ntsc", "brightness",   0);
      sharpness    = get_config_int ("ntsc", "sharpness",    0);
      gamma        = get_config_int ("ntsc", "gamma",        0);
      resolution   = get_config_int ("ntsc", "resolution",   0);
      artifacts    = get_config_int ("ntsc", "artifacts",    0);
      fringing     = get_config_int ("ntsc", "fringing",     0);
      bleed        = get_config_int ("ntsc", "bleed",        0);
      hue_warping  = get_config_int ("ntsc", "hue_warping",  0);

      /* Initialize ntsc. */
   
      memset (setup, 0, sizeof (snes_ntsc_setup_t));
   
      setup->hue         = (hue         / 100.0f);
      setup->saturation  = (saturation  / 100.0f);
      setup->contrast    = (contrast    / 100.0f);
      setup->brightness  = (brightness  / 100.0f);
      setup->sharpness   = (sharpness   / 100.0f);
      setup->hue_warping = (hue_warping / 100.0f);
      setup->gamma       = (gamma       / 100.0f);
      setup->resolution  = (resolution  / 100.0f);
      setup->artifacts   = (artifacts   / 100.0f);
      setup->fringing    = (fringing    / 100.0f);
      setup->bleed       = (bleed       / 100.0f);
   }

   merge_fields  = get_config_int ("ntsc", "merge_fields", 1);
   doubling      = get_config_int ("ntsc", "doubling",     0);
   interpolation = get_config_int ("ntsc", "interpolated", 1);

   setup->merge_fields         = merge_fields;
   _ntsc_scanline_doubling = fix (doubling, 0, 2);
   _ntsc_interpolation     = interpolation;

   snes_ntsc_init (&_ntsc_ntsc, setup);

   /* Calculate sizes. */
   w = src->w;
   h = src->h;
   wm = SNES_NTSC_OUT_WIDTH(w);
   hm = 240;         /* Output height from snes_ntsc_blit(). */
   hx = (240 * 2);   /* Output height from blit_ntsc(). */

   /* Allocate input buffer. */
   blit_buffer_in = malloc (((w * h) * sizeof (unsigned short)));
   if (!blit_buffer_in)
      WARN_BREAK_GENERIC();

   /* Allocate output buffer. */
   blit_buffer_out = malloc (((wm * hm) * sizeof (unsigned short)));
   if (!blit_buffer_out)
   {
      WARN_GENERIC();
      free (blit_buffer_in);
      return;
   }

   blit_x_offset = ((dest->w / 2) - (wm / 2));
   blit_y_offset = ((dest->h / 2) - (hx / 2));
}

/* Deinitializer. */

static void deinit_ntsc (void)
{
   /* Destroy buffers. */

   if (blit_buffer_in)
   {
      free (blit_buffer_in);
      blit_buffer_in = NULL;
   }

   if (blit_buffer_out)
   {
      free (blit_buffer_out);
      blit_buffer_out = NULL;
   }
}

/* Interface. */

static const BLITTER blitter_ntsc =
{
   init_ntsc, deinit_ntsc,
   blit_ntsc
};
