static UINT32 tile_decode_table_plane_0[16];
static UINT32 tile_decode_table_plane_1[16];

#define VRAM_CACHE_TILE_ADDRESS(TILE,Y) \
 (ppu_pattern_vram_cache + (TILE * 8 + Y) * 8)

#define VRAM_CACHE_TILE_TAG_ADDRESS(TILE,Y) \
 (ppu_pattern_vram_cache_tag + (TILE * 8 + Y))

#define CHR_ROM_CACHE_TILE_ADDRESS(TILE,Y) \
 (ROM_CHR_ROM_CACHE + (TILE * 8 + Y) * 8)

#define CHR_ROM_CACHE_TILE_TAG_ADDRESS(TILE,Y) \
 (ROM_CHR_ROM_CACHE_TAG + (TILE * 8 + Y))

static void ppu_cache_init (void)
{
    int i;

    /* calculate the tile decoding lookup tables */
    for (i = 0; i < 16; i++)
    {
        int pixels0 = 0, pixels1 = 0;

#ifdef LSB_FIRST
        if (i & 8)
        {
            pixels0 |= (0xFC | 1);
            pixels1 |= (0xFC | 2);
        }
        if (i & 4)
        {
            pixels0 |= (0xFC | 1) << 8;
            pixels1 |= (0xFC | 2) << 8;
        }
        if (i & 2)
        {
            pixels0 |= (0xFC | 1) << 16;
            pixels1 |= (0xFC | 2) << 16;
        }
        if (i & 1)
        {
            pixels0 |= (0xFC | 1) << 24;
            pixels1 |= (0xFC | 2) << 24;
        }
#else
        if (i & 8)
        {
            pixels0 |= (0xFC | 1) << 24;
            pixels1 |= (0xFC | 2) << 24;
        }
        if (i & 4)
        {
            pixels0 |= (0xFC | 1) << 16;
            pixels1 |= (0xFC | 2) << 16;
        }
        if (i & 2)
        {
            pixels0 |= (0xFC | 1) << 8;
            pixels1 |= (0xFC | 2) << 8;
        }
        if (i & 1)
        {
            pixels0 |= (0xFC | 1);
            pixels1 |= (0xFC | 2);
        }
#endif
        tile_decode_table_plane_0 [i] = pixels0;
        tile_decode_table_plane_1 [i] = pixels1;
    }

}

static void clear_vram_set (int vram_block)
{
    ppu_vram_dirty_set_begin [vram_block] = -2;
    ppu_vram_dirty_set_end [vram_block] = -2;
}


void ppu_cache_all_vram (void)
{
    int tile, num_tiles, i;

    num_tiles = sizeof(ppu_pattern_vram) / (8 * 2);

    for (tile = 0; tile < num_tiles; tile++)
    {
        int y;

        for (y = 0; y < 8; y++)
        {
            UINT32 pixels0_3, pixels4_7;

            pixels0_3 = tile_decode_table_plane_0
                [(ppu_pattern_vram [tile * 16 + y]) >> 4];
            pixels4_7 = tile_decode_table_plane_0
                [(ppu_pattern_vram [tile * 16 + y]) & 0x0F];

            pixels0_3 |= tile_decode_table_plane_1
                [(ppu_pattern_vram [tile * 16 + y + 8]) >> 4];
            pixels4_7 |= tile_decode_table_plane_1
                [(ppu_pattern_vram [tile * 16 + y + 8]) & 0x0F];
            

            *(UINT32 *) VRAM_CACHE_TILE_ADDRESS(tile,y) =
                pixels0_3;
            *(UINT32 *) (VRAM_CACHE_TILE_ADDRESS(tile,y) + 4) =
                pixels4_7;

            *VRAM_CACHE_TILE_TAG_ADDRESS(tile,y) =
                ppu_pattern_vram [tile * 16 + y] |
                ppu_pattern_vram [tile * 16 + y + 8];
        }
    }

    for (i = 0; i < FIRST_VROM_BLOCK; i++)
    {
        clear_vram_set (i);
    }

    ppu_vram_cache_needs_update = FALSE;
}


void ppu_cache_chr_rom_pages (void)
{
    int tile, num_tiles;

    /* 8k CHR ROM page size, 2-bitplane 8x8 tiles */
    num_tiles = (ROM_CHR_ROM_PAGES * 0x2000) / (8 * 2);

    for (tile = 0; tile < num_tiles; tile++)
    {
        int y;

        for (y = 0; y < 8; y++)
        {
            UINT32 pixels0_3, pixels4_7;

            pixels0_3 = tile_decode_table_plane_0
                [(ROM_CHR_ROM [tile * 16 + y]) >> 4];
            pixels4_7 = tile_decode_table_plane_0
                [(ROM_CHR_ROM [tile * 16 + y]) & 0x0F];

            pixels0_3 |= tile_decode_table_plane_1
                [(ROM_CHR_ROM [tile * 16 + y + 8]) >> 4];
            pixels4_7 |= tile_decode_table_plane_1
                [(ROM_CHR_ROM [tile * 16 + y + 8]) & 0x0F];
            

            *(UINT32 *) CHR_ROM_CACHE_TILE_ADDRESS(tile,y) =
                pixels0_3;
            *(UINT32 *) (CHR_ROM_CACHE_TILE_ADDRESS(tile,y) + 4) =
                pixels4_7;

            *CHR_ROM_CACHE_TILE_TAG_ADDRESS(tile,y) =
                ROM_CHR_ROM [tile * 16 + y] |
                ROM_CHR_ROM [tile * 16 + y + 8];
        }
    }
}


static void recache_vram_set (int vram_block)
{
    int tile, begin, end;

    begin = ppu_vram_dirty_set_begin [vram_block] + (vram_block * 0x400) / (8 * 2);
    end = ppu_vram_dirty_set_end [vram_block] + (vram_block * 0x400) / (8 * 2);

    for (tile = begin; tile <= end; tile++)
    {
        int y;

        for (y = 0; y < 8; y++)
        {
            UINT32 pixels0_3, pixels4_7;

            pixels0_3 = tile_decode_table_plane_0
                [(ppu_pattern_vram [tile * 16 + y]) >> 4];
            pixels4_7 = tile_decode_table_plane_0
                [(ppu_pattern_vram [tile * 16 + y]) & 0x0F];

            pixels0_3 |= tile_decode_table_plane_1
                [(ppu_pattern_vram [tile * 16 + y + 8]) >> 4];
            pixels4_7 |= tile_decode_table_plane_1
                [(ppu_pattern_vram [tile * 16 + y + 8]) & 0x0F];
            

            *(UINT32 *) VRAM_CACHE_TILE_ADDRESS(tile,y) =
                pixels0_3;
            *(UINT32 *) (VRAM_CACHE_TILE_ADDRESS(tile,y) + 4) =
                pixels4_7;

            *VRAM_CACHE_TILE_TAG_ADDRESS(tile,y) =
                ppu_pattern_vram [tile * 16 + y] |
                ppu_pattern_vram [tile * 16 + y + 8];
        }
    }
}


#define vram_set_needs_recache(set) (ppu_vram_dirty_set_end [set] >= 0)

static void recache_vram_sets (void)
{
    int i;

    for (i = 0; i < FIRST_VROM_BLOCK; i++)
    {
        if (vram_set_needs_recache(i))
        {
            recache_vram_set (i);
            clear_vram_set (i);
        }
    }
    ppu_vram_cache_needs_update = FALSE;
}



