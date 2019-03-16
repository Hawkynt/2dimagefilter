/* VRAM address bit layout          */
/* -YYY VHyy yyyx xxxx              */
/* x = x tile offset in name table  */
/* y = y tile offset in name table  */
/* H = horizontal name table        */
/* V = vertical name table          */
/* Y = y line offset in tile        */

/* dummy reads for write-back cache line loading */
static void dummy_read_line(UINT8 *buffer)
{
#ifdef ALLEGRO_I386
    if ((cpu_family == 4 &&
        (cpu_model == 7 || cpu_model == 8 || cpu_model == 15)) ||
        (cpu_family == 5 && cpu_model == 5))
    {
        buffer [0] |=
            buffer [16] |
            buffer [16*2] |
            buffer [16*3] |
            buffer [16*4] |
            buffer [16*5] |
            buffer [16*6] |
            buffer [16*7] |
            buffer [16*8] |
            buffer [16*9] |
            buffer [16*10] |
            buffer [16*11] |
            buffer [16*12] |
            buffer [16*13] |
            buffer [16*14] |
            buffer [16*15];
    }

    if (cpu_family == 5)
    {
        buffer [0] |=
            buffer [16*2] |
            buffer [16*4] |
            buffer [16*6] |
            buffer [16*8] |
            buffer [16*10] |
            buffer [16*12] |
            buffer [16*14];
    }
#endif
}


#define PBT_NO_MASK
#define PBT_MASK { if (color == 0) continue; }
#define PBT_NO_DISPLAY(OFFSET)
#define PBT_DISPLAY(OFFSET) \
    { \
        plot_buffer [plot_pixel + OFFSET] = \
            ((ppu_background_palette [color] & palette_mask) + PALETTE_ADJUST); \
    }

#define PLOT_BACKGROUND_TILE_PIXEL(OFFSET,MASK,DISPLAY) \
    do \
    { \
        color = cache_address [sub_x + (OFFSET)] & attribute; \
 \
        MASK \
 \
        background_pixels [8 + plot_pixel + (OFFSET)] = color; \
 \
        DISPLAY(OFFSET) \
    } while (FALSE);


#define PLOT_BACKGROUND_TILE_LOOP(MASK,DISPLAY) \
    for (; sub_x < 8; sub_x ++, plot_pixel ++) \
    { \
        UINT8 color; \
 \
        PLOT_BACKGROUND_TILE_PIXEL(0,MASK,DISPLAY) \
    }


#define PLOT_BACKGROUND_TILE_FULL(MASK,DISPLAY) \
    { \
        UINT8 color; \
 \
        PLOT_BACKGROUND_TILE_PIXEL(0,MASK,DISPLAY) \
        PLOT_BACKGROUND_TILE_PIXEL(1,MASK,DISPLAY) \
        PLOT_BACKGROUND_TILE_PIXEL(2,MASK,DISPLAY) \
        PLOT_BACKGROUND_TILE_PIXEL(3,MASK,DISPLAY) \
        PLOT_BACKGROUND_TILE_PIXEL(4,MASK,DISPLAY) \
        PLOT_BACKGROUND_TILE_PIXEL(5,MASK,DISPLAY) \
        PLOT_BACKGROUND_TILE_PIXEL(6,MASK,DISPLAY) \
        PLOT_BACKGROUND_TILE_PIXEL(7,MASK,DISPLAY) \
        plot_pixel += 8; \
    }


static void ppu_render_background (int line)
{
    int attribute_address;
    int name_table;
    UINT8 *name_table_address;
    UINT8 *plot_buffer = PPU_GET_LINE_ADDRESS(video_buffer, line);
    int plot_pixel = 0;
    UINT8 attribute_byte = 0;
    int bg_vram_address;

    int x, sub_x;
    int y, sub_y;

    if (ppu_enable_background_layer)
    {
        /* dummy reads for write-back cache line loading */
        dummy_read_line(plot_buffer);
    }

    dummy_read_line(background_pixels + 8);

    memset (plot_buffer, ((ppu_background_palette [0] & palette_mask) + PALETTE_ADJUST), 256);

    if (PPU_SPRITES_ENABLED)
    {
        /* used for sprite pixel allocation and collision detection */
        memset (background_pixels + 8, 0, 256);
    }


    name_table = (vram_address >> 10) & 3;
    name_table_address = name_tables_read[name_table];

    y = (vram_address >> 5) & 0x1F;
    sub_y = (vram_address >> 12) & 7;

    attribute_address =
     /* Y position */
     ((y >> 2) * 8) +
     /* X position */
     ((vram_address & 0x1F) >> 2);

    if (vram_address & 3)
    /* fetch and shift first attribute byte */
    {
        attribute_byte =
            name_table_address [0x3C0 + attribute_address] >>
            ( ( (y & 2) * 2 + (vram_address & 2)));
    }

    bg_vram_address = vram_address & 0x3FF;


    /* If background clip left edge is enabled, then skip the entirety
     * of the first tile
     */
    /* Draw the background. */
    for (x = 0; x < (256 / 8) + 1; x ++)
    {
        unsigned tile_name, tile_address;
        UINT8 *cache_address;
        unsigned cache_bank, cache_index;
        UINT8 cache_tag;
        UINT8 attribute;

        if (!(bg_vram_address & 3))
        /* fetch and shift attribute byte */
        {
            attribute_byte =
                name_table_address [0x3C0 + attribute_address];
            if (y & 2) attribute_byte >>= 4;
        }

        tile_name = name_table_address [bg_vram_address];
        tile_address = ((tile_name * 16) + background_tileset);

        if (mmc_check_latches)
        {
            if ((tile_name >= 0xFD) && (tile_name <= 0xFE))
            {
                mmc_check_latches(tile_address);
            }
        }
   
    
        cache_bank = tile_address >> 10;
        cache_index = ((tile_address & 0x3FF) / 2) + sub_y;

        cache_tag = ppu_vram_block_background_cache_tag_address [cache_bank]
            [cache_index];

        if (cache_tag)
        /* some non-transparent pixels */
        {
            cache_address =
                ppu_vram_block_background_cache_address [cache_bank] +
                cache_index * 8;

            attribute = attribute_table [attribute_byte & 3];

            if (x > 1)
            {
                sub_x = 0;
            }
            else
            {
                if (PPU_BACKGROUND_CLIP_ENABLED)
                {
                    if (x == 0)
                    {
                        sub_x = 8;
                        plot_pixel += 8 - x_offset;
                    }
                    else /* (x == 1) */
                    {
                        sub_x = x_offset;
                        plot_pixel += x_offset;
                    }
                }
                else
                {
                    if (x == 0)
                    {
                        sub_x = x_offset;
                    }
                    else
                    {
                        sub_x = 0;
                    }
                }
            }

            if (sub_x == 0)
            {
                if (cache_tag != 0xFF)
                /* some transparent pixels */
                {
                    if (ppu_enable_background_layer)
                    {
                        PLOT_BACKGROUND_TILE_FULL(PBT_MASK,PBT_DISPLAY)
                    }
                    else
                    {
                        PLOT_BACKGROUND_TILE_FULL(PBT_MASK,PBT_NO_DISPLAY)
                    }
                }
                else
                /* no transparent pixels */
                {
                    if (ppu_enable_background_layer)
                    {
                        PLOT_BACKGROUND_TILE_FULL(PBT_NO_MASK,PBT_DISPLAY)
                    }
                    else
                    {
                        PLOT_BACKGROUND_TILE_FULL(PBT_NO_MASK,PBT_NO_DISPLAY)
                    }
                }
            }
            else
            {
                if (cache_tag != 0xFF)
                /* some transparent pixels */
                {
                    if (ppu_enable_background_layer)
                    {
                        PLOT_BACKGROUND_TILE_LOOP(PBT_MASK,PBT_DISPLAY)
                    }
                    else
                    {
                        PLOT_BACKGROUND_TILE_LOOP(PBT_MASK,PBT_NO_DISPLAY)
                    }
                }
                else
                /* no transparent pixels */
                {
                    if (ppu_enable_background_layer)
                    {
                        PLOT_BACKGROUND_TILE_LOOP(PBT_NO_MASK,PBT_DISPLAY)
                    }
                    else
                    {
                        PLOT_BACKGROUND_TILE_LOOP(PBT_NO_MASK,PBT_NO_DISPLAY)
                    }
                }
            }
        }
        else
        {
            if (x == 0)
            {
                plot_pixel += 8 - x_offset;
            }
            else
            {
                plot_pixel += 8;
            }
        }

        ++bg_vram_address;

        /* next name byte */
        if (!(bg_vram_address & 1))
        /* new attribute */
        {
            if (!(bg_vram_address & 2))
            /* new attribute byte */
            {
                ++attribute_address;

                if ((bg_vram_address & 0x1F) == 0)
                /* horizontal name table toggle */
                {
                    name_table ^= 1;
                    name_table_address = name_tables_read[name_table];

                    /* handle address wrap */
                    bg_vram_address = (bg_vram_address - (1 << 5));
                    attribute_address -= (1 << 3);
                }
            }
            else
            /* same attribute byte */
            {
                attribute_byte >>= 2;
            }
        }
    }
    vram_address = bg_vram_address + (name_table << 10) + (sub_y << 12);
}
