/* ----- Sprite rendering routines. ----- */

#define PPU_SPRITE_V_FLIP_BIT       0x80
#define PPU_SPRITE_H_FLIP_BIT       0x40
#define PPU_SPRITE_PRIORITY_BIT     0x20


static INT8 sprite_list_needs_recache;
static UINT8 sprites_on_line [LAST_DISPLAYED_LINE + 1] [8];
static UINT8 sprite_count_on_line [LAST_DISPLAYED_LINE + 1];
static INT8 sprite_overflow_on_line [LAST_DISPLAYED_LINE + 1];


static void recache_sprite_list (void)
{
    int line, sprite;

    for (line = 0; line <= LAST_DISPLAYED_LINE; line++)
    {
        sprite_overflow_on_line [line] = 0;
        sprite_count_on_line [line] = 0;
    }

    for (sprite = 0; sprite < 64; sprite++)
    {
        int first_y, last_y;

        first_y = ppu_spr_ram [(sprite * 4) + 0] + 1;
        last_y = first_y + sprite_height - 1;

        /* vertical clipping */
        if (last_y >= 239) last_y = 239;

        for (line = first_y; line <= last_y; line++)
        {
            if (sprite_count_on_line [line] == 8)
            {
                sprite_overflow_on_line [line] = PPU_SPRITE_OVERFLOW_BIT;
                continue;
            }

            sprites_on_line [line] [sprite_count_on_line [line]++] = sprite;
        }
    }

    sprite_list_needs_recache = FALSE;
}


static INLINE void ppu_render_sprite (int sprite, int line)
{
    int x, y, sub_x, sub_y;

    int first_x, last_x, last_y;

    int tile;

    UINT8 attribute;

    int address, priority;

    int flip_h, flip_v;

    UINT8 *cache_address;


    /* Offset. */

    sprite *= 4;


    x = ppu_spr_ram [sprite + 3];

    /* perform horizontal clipping */

    if (x <= 256 - 8)
    /* sprite not partially off right edge */
    {
        last_x = 7;

        if (PPU_SPRITES_CLIP_ENABLED && x < 8)
        /* is sprite clipped on left-edge? */
        {
         if (x == 0)
         /* sprite fully clipped? */
         {
          return;
         }

         /* sprite partially clipped */
         first_x = 8 - x;

        }
        else
        /* sprite not clipped on left edge */
        {
            first_x = 0;
        }
    }
    else
    /* sprite clipped on right edge */
    {
        first_x = 0;
        last_x = 256 - x - 1;
    }


    tile = ppu_spr_ram [sprite + 1];


    if (sprite_height == 8)
    {
        /* Draw 8x8 sprites. */

        address = ((tile * 16) + sprite_tileset);
    
    }
    else
    {
        /* Draw 8x16 sprites. */

        if (! (tile & 1))
        {
            address = (tile * 16);
        }
        else
        {
            address = (((tile - 1) * 16) + 0x1000);
        }

    }

    /* Line in sprite to draw. */
    y = line - (ppu_spr_ram [sprite] + 1);

    /* Vertical flipping. */
    flip_v = (ppu_spr_ram [sprite + 2] & PPU_SPRITE_V_FLIP_BIT);

    if (flip_v)
    {
        y = sprite_height - 1 - y;
    }


    if (mmc_check_latches)
    {
        int address2 = address + y;

        if (y >= 8) address2 += 8;

        if (((address2 & 0xfff) >= 0xfd0) &&
            ((address2 & 0xfff) <= 0xfef))
        {
            mmc_check_latches (address2);
        }
    }

    cache_address = ppu_vram_block_sprite_cache_address [address >> 10] +
        ((address & 0x3FF) / 2 * 8) + (y * 8);

    attribute = attribute_table [ppu_spr_ram [sprite + 2] & 3];


    /* Horizontal flipping. */
    flip_h = (ppu_spr_ram [sprite + 2] & PPU_SPRITE_H_FLIP_BIT);


    priority = (ppu_spr_ram [sprite + 2] & PPU_SPRITE_PRIORITY_BIT);

    if (sprite == 0 && PPU_BACKGROUND_ENABLED)
    /* sprite 0 collision detection */
    {
        if (priority)
        /* low priority, plot under background */
        {
            for (sub_x = first_x; sub_x <= last_x; sub_x ++)
            {
                UINT8 color;

                if (flip_h)
                {
                    color = cache_address [(7 - sub_x)];
                }
                else
                {
                    color = cache_address [sub_x];
                }

                /* Transparency. */
                if ((color &= attribute) == 0)
                {
                    continue;
                }


                /* Background transparency & sprite 0 collision. */
                if (background_pixels [8 + (x + sub_x)])
                {
                    background_pixels [8 + (x + sub_x)] = 16;

                    if (!first_sprite_this_line)
                    {
                        first_sprite_this_line =
                            (x + sub_x) + 1 + DOTS_HBLANK_BEFORE_RENDER;
                    }
                    continue;
                }
                else
                {
                    background_pixels [8 + (x + sub_x)] = 16;
                }

                color = ((ppu_sprite_palette [color] & palette_mask) + PALETTE_ADJUST);


                PPU_PUTPIXEL (video_buffer,
                    (x + sub_x), line, color);

            }
        }
        else
        /* high priority, plot over background */
        {
            for (sub_x = first_x; sub_x <= last_x; sub_x ++)
            {
                UINT8 color;

                if (flip_h)
                {
                    color = cache_address [(7 - sub_x)];
                }
                else
                {
                    color = cache_address [sub_x];
                }

                /* Transparency. */
                if ((color &= attribute) == 0)
                {
                    continue;
                }


                /* Sprite 0 collision. */
                if (background_pixels [8 + (x + sub_x)])
                {
                    if (!first_sprite_this_line)
                    {
                        first_sprite_this_line =
                            (x + sub_x) + 1 + DOTS_HBLANK_BEFORE_RENDER;
                    }
                }

                /* Sprite 0 will always get its pixels... */
                background_pixels [8 + (x + sub_x)] = 16;

                color = ((ppu_sprite_palette [color] & palette_mask) + PALETTE_ADJUST);


                PPU_PUTPIXEL (video_buffer,
                    (x + sub_x), line, color);

            }
        }
    }
    else
    /* normal plot */
    {
        if (priority)
        /* low priority, plot under background */
        {
            if (ppu_enable_sprite_layer_a)
            {
                for (sub_x = first_x; sub_x <= last_x; sub_x ++)
                {
                    UINT8 color;

                    if (flip_h)
                    {
                        color = cache_address [(7 - sub_x)];
                    }
                    else
                    {
                        color = cache_address [sub_x];
                    }

                    /* Transparency. */
                    if ((color &= attribute) == 0)
                    {
                        continue;
                    }


                    /* Transparency. */
                    if (background_pixels [8 + (x + sub_x)])
                    {
                        background_pixels [8 + (x + sub_x)] = 16;
                        continue;
                    }
                    else
                    {
                        background_pixels [8 + (x + sub_x)] = 16;
                    }

                   color = ((ppu_sprite_palette [color] & palette_mask) + PALETTE_ADJUST);


                    PPU_PUTPIXEL (video_buffer,
                        (x + sub_x), line, color);
                }

            }
        }
        else
        /* high priority, plot over background */
        {
            if (ppu_enable_sprite_layer_b)
            {
                for (sub_x = first_x; sub_x <= last_x; sub_x ++)
                {
                    UINT8 color;

                    if (flip_h)
                    {
                        color = cache_address [(7 - sub_x)];
                    }
                    else
                    {
                        color = cache_address [sub_x];
                    }

                    /* Transparency. */
                    if ((color &= attribute) == 0)
                    {
                        continue;
                    }


                    /* Transparency. */
                    if (background_pixels [8 + (x + sub_x)] >= 16)
                    {
                        continue;
                    }
                    else
                    {
                        background_pixels [8 + (x + sub_x)] = 16;
                    }

                    color = ((ppu_sprite_palette [color] & palette_mask) + PALETTE_ADJUST);


                    PPU_PUTPIXEL (video_buffer,
                        (x + sub_x), line, color);
                }

            }
        }
    }
}


static void ppu_render_sprites (int line)
{
    int i, priority;


    if (sprite_list_needs_recache)
    {
        recache_sprite_list ();
    }

    if (sprite_count_on_line [line] == 0) return;

    for (i = 0; i < sprite_count_on_line [line]; i++)
    {
        int sprite = sprites_on_line [line] [i];

        ppu_render_sprite (sprite, line);
    }
}
