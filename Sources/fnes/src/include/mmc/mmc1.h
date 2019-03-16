

/* Mapper #1 (MMC1). */

/* This mapper is only partially supported. */


static int mmc1_init (void);

static void mmc1_reset (void);


static void mmc1_save_state (PACKFILE *, int);

static void mmc1_load_state (PACKFILE *, int);


static const MMC mmc_mmc1 =
{
    1, "MMC1",

    mmc1_init, mmc1_reset,


    "MMC1\0\0\0\0",

    mmc1_save_state, mmc1_load_state
};


static UINT8 mmc1_bit_stream;
static UINT8 mmc1_bit_counter;


static UINT8 mmc1_register[4];

static UINT8 mmc1_previous_register = 0x0000;

static UINT8 mmc1_256k_bank_num;

static UINT8 mmc1_cpu_bank[2];


#define MMC1_MIRRORING_ADDRESS_BIT  1
#define MMC1_MIRRORING_MODE_BIT     2
#define MMC1_PRG_BANK_SELECT_BIT    4
#define MMC1_PRG_BANK_SIZE_BIT      8
#define MMC1_VROM_BANK_SIZE_BIT     0x10
#define MMC1_USE_256K_SELECT_1_BIT  0x10

#define MMC1_256K_SELECT_0_BIT      0x10

#define MMC1_256K_SELECT_1_BIT      0x10


static INLINE void mmc1_update_prg_banking (void)
{
  cpu_set_read_address_16k_rom_block (0x8000, (mmc1_256k_bank_num * (256 / 16)) + mmc1_cpu_bank[0]);
  cpu_set_read_address_16k_rom_block (0xC000, (mmc1_256k_bank_num * (256 / 16)) + mmc1_cpu_bank[1]);
}


static INLINE void mmc1_update_prg_bank_select (void)
{
    if (mmc1_register[0] & MMC1_PRG_BANK_SIZE_BIT)
    /* 16k PRG banking? */
    {
        int mmc1_bank_number = mmc1_register[3] & 0x0F;

        if (mmc1_register[0] & MMC1_PRG_BANK_SELECT_BIT)
        /* 8000-BFFF PRG banking? */
        {
            mmc1_cpu_bank [0] = mmc1_bank_number;
            mmc1_cpu_bank [1] = (ROM_PRG_ROM_PAGES - 1) & 0x0F;
        }
        else
        /* No, C000-FFFF PRG banking */
        {
            mmc1_cpu_bank [0] = 0;
            mmc1_cpu_bank [1] = mmc1_bank_number;
        }
    }
    else
    /* No, 32k PRG banking */
    {
        int mmc1_bank_number = (mmc1_register[3] & 0x0F & ~1);

        mmc1_cpu_bank [0] = mmc1_bank_number;
        mmc1_cpu_bank [1] = mmc1_bank_number + 1;
    }
}


void mmc1_update_256k_prg_banking (void)
{
    /* Handle >256K addressing */
    if (mmc1_register[0] & MMC1_USE_256K_SELECT_1_BIT)
    {
        mmc1_256k_bank_num =
            (mmc1_register[1] & MMC1_256K_SELECT_0_BIT) >> 4;
        if (mmc1_register[0] & MMC1_PRG_BANK_SIZE_BIT)    /* Er...? */
        {
            mmc1_256k_bank_num |=
                ((mmc1_register[2] & MMC1_256K_SELECT_1_BIT) >> 3);
        }
    }
    else
    {
        mmc1_256k_bank_num =
            (mmc1_register[1] & MMC1_256K_SELECT_0_BIT) ? 3 : 0;
    }

    mmc1_update_prg_banking();
}


/* passed a register selection address, 0-3 */
static void mmc1_set_register_value (unsigned address, UINT8 value)
{
    mmc1_register[address] = value;

    switch (address)
    {
	case 0:
        {
            if (mmc1_register[0] & MMC1_MIRRORING_MODE_BIT)  
            /* standard h/v mirroring? */
            {
                /* which way? */
                ppu_set_mirroring (
                    (mmc1_register[0] & MMC1_MIRRORING_ADDRESS_BIT) ?
                    MIRRORING_HORIZONTAL : MIRRORING_VERTICAL);
            }
	    else
            /* one-screen mirroring */
            {
                /* which page? */
                ppu_set_mirroring (
                    (mmc1_register[0] & MMC1_MIRRORING_ADDRESS_BIT) ?
                    MIRRORING_ONE_SCREEN_2400 : MIRRORING_ONE_SCREEN_2000);
            }

            mmc1_update_prg_bank_select();


            /* Handle >256K addressing */
            mmc1_update_256k_prg_banking();

            mmc1_update_prg_banking();


            if (ROM_CHR_ROM_PAGES > 0)
            {
                if (mmc1_register[0] & MMC1_VROM_BANK_SIZE_BIT)
                /* 4k VROM mapping? */
                {
                    int mmc1_bank_number = mmc1_register[1] * 4;

                    /* swap 4k of CHR-ROM */
                    ppu_set_ram_1k_pattern_vrom_block (0 << 10,
                        mmc1_bank_number);
                    ppu_set_ram_1k_pattern_vrom_block (1 << 10,
                        mmc1_bank_number + 1);
                    ppu_set_ram_1k_pattern_vrom_block (2 << 10,
                        mmc1_bank_number + 2);
                    ppu_set_ram_1k_pattern_vrom_block (3 << 10,
                        mmc1_bank_number + 3);


                    mmc1_bank_number = mmc1_register[2] * 4;

                    /* swap other 4k of CHR-ROM */
                    ppu_set_ram_1k_pattern_vrom_block (4 << 10,
                        mmc1_bank_number);
                    ppu_set_ram_1k_pattern_vrom_block (5 << 10,
                        mmc1_bank_number + 1);
                    ppu_set_ram_1k_pattern_vrom_block (6 << 10,
                        mmc1_bank_number + 2);
                    ppu_set_ram_1k_pattern_vrom_block (7 << 10,
                        mmc1_bank_number + 3);
                }
                else
                /* No, 8k VROM mapping */
                {
                    int mmc1_bank_number = (mmc1_register[1] & ~1) * 4;

                    /* swap 8k of CHR-ROM */ //never called??
                    ppu_set_ram_1k_pattern_vrom_block (0 << 10,
                        mmc1_bank_number);
                    ppu_set_ram_1k_pattern_vrom_block (1 << 10,
                        mmc1_bank_number + 1);
                    ppu_set_ram_1k_pattern_vrom_block (2 << 10,
                        mmc1_bank_number + 2);
                    ppu_set_ram_1k_pattern_vrom_block (3 << 10,
                        mmc1_bank_number + 3);
                    ppu_set_ram_1k_pattern_vrom_block (4 << 10,
                        mmc1_bank_number + 4);
                    ppu_set_ram_1k_pattern_vrom_block (5 << 10,
                        mmc1_bank_number + 5);
                    ppu_set_ram_1k_pattern_vrom_block (6 << 10,
                        mmc1_bank_number + 6);
                    ppu_set_ram_1k_pattern_vrom_block (7 << 10,
                        mmc1_bank_number + 7);
                }
            }

            break;
        }
	
	case 1:
        {
            /* Handle >256K addressing */
            mmc1_update_256k_prg_banking();

            mmc1_update_prg_banking();

            if (ROM_CHR_ROM_PAGES > 0)
            {
                if (mmc1_register[0] & MMC1_VROM_BANK_SIZE_BIT)
                /* 4k VROM mapping? */
                {
                    int mmc1_bank_number = mmc1_register[1] * 4;

                    /* swap 4k of CHR-ROM */ //works!!!
                    ppu_set_ram_1k_pattern_vrom_block (0 << 10,
                        mmc1_bank_number);
                    ppu_set_ram_1k_pattern_vrom_block (1 << 10,
                        mmc1_bank_number + 1);
                    ppu_set_ram_1k_pattern_vrom_block (2 << 10,
                        mmc1_bank_number + 2);
                    ppu_set_ram_1k_pattern_vrom_block (3 << 10,
                        mmc1_bank_number + 3);
                }
                else
                /* No, 8k VROM mapping */
                {
                    int mmc1_bank_number = (mmc1_register[1] & ~1) * 4;

                    /* swap 8k of CHR-ROM */ //never called??
                    ppu_set_ram_1k_pattern_vrom_block (0 << 10,
                        mmc1_bank_number);
                    ppu_set_ram_1k_pattern_vrom_block (1 << 10,
                        mmc1_bank_number + 1);
                    ppu_set_ram_1k_pattern_vrom_block (2 << 10,
                        mmc1_bank_number + 2);
                    ppu_set_ram_1k_pattern_vrom_block (3 << 10,
                        mmc1_bank_number + 3);
                    ppu_set_ram_1k_pattern_vrom_block (4 << 10,
                        mmc1_bank_number + 4);
                    ppu_set_ram_1k_pattern_vrom_block (5 << 10,
                        mmc1_bank_number + 5);
                    ppu_set_ram_1k_pattern_vrom_block (6 << 10,
                        mmc1_bank_number + 6);
                    ppu_set_ram_1k_pattern_vrom_block (7 << 10,
                        mmc1_bank_number + 7);
                }
            }

            break;
        }

        case 2:
        {
            int mmc1_bank_number = mmc1_register[2];

            /* Handle >256K addressing */
            mmc1_update_256k_prg_banking();

            mmc1_update_prg_banking();

            if (ROM_CHR_ROM_PAGES == 0)
            {
                return;
            }

            if (mmc1_register[0] & MMC1_VROM_BANK_SIZE_BIT)
            /* 4k VROM mapping? */
            {
                int mmc1_bank_number = mmc1_register[2] * 4;

                /* swap other 4k of CHR-ROM */ //works!!!
                ppu_set_ram_1k_pattern_vrom_block (4 << 10,
                    mmc1_bank_number);
                ppu_set_ram_1k_pattern_vrom_block (5 << 10,
                    mmc1_bank_number + 1);
                ppu_set_ram_1k_pattern_vrom_block (6 << 10,
                    mmc1_bank_number + 2);
                ppu_set_ram_1k_pattern_vrom_block (7 << 10,
                    mmc1_bank_number + 3);
            }

            break;
        }

        case 3:
        {
            mmc1_update_prg_bank_select();

            mmc1_update_prg_banking();

            break;
        }
    }
}


static void mmc1_write (UINT16 address, UINT8 value)
{
    int mmc1_current_register;
        
    mmc1_current_register = (address & 0x6000) >> 13;

    /* just in case the write is to a different register */
    if (mmc1_current_register != mmc1_previous_register)
    {
        mmc1_bit_counter = 0;
	mmc1_bit_stream = 0x00;
        mmc1_previous_register = mmc1_current_register;
    }

    /* check the reset flag */
    if (value & 0x80)
    {
        mmc1_bit_counter = 0;
	mmc1_bit_stream = 0x00;
        mmc1_set_register_value (0, mmc1_register[0] | 0x0C);
	return;
    }

    /* buffer the 5 bit writes into mmc1_bit_stream */
    if (value & 0x01) mmc1_bit_stream |= (1 << mmc1_bit_counter); //neat idea, nester dude!

    mmc1_bit_counter++;
    if (mmc1_bit_counter < 5) return;

    /* pack the buffered bits into the register */
    mmc1_set_register_value (mmc1_current_register, mmc1_bit_stream);

    /* clean up buffer & counter */
    mmc1_bit_stream = 0x00;
    mmc1_bit_counter = 0;
}

static void mmc1_reset (void)
{
    int index;

    /* Select first 16k page in lower 16k. */
    /* Select last 16k page in upper 16k. */
    mmc1_set_register_value (0, 0x0C);
    mmc1_set_register_value (1, 0x00);
    mmc1_set_register_value (2, 0x00);
    mmc1_set_register_value (3, 0x00);


    if (ROM_CHR_ROM_PAGES)
    {
        /* Select first 8k page. */

        for (index = 0; index < 8; index ++)
        {
            ppu_set_ram_1k_pattern_vrom_block (index << 10, index);
        }
    }    
}


static int mmc1_init (void)
{
    if (ROM_CHR_ROM_PAGES == 0)
    {
        /* No VROM is present. */

        ppu_set_ram_8k_pattern_vram ();
    }

 
    cpu_set_write_handler_32k (0x8000, mmc1_write);

    mmc1_reset ();

    return 0;
}


static void mmc1_save_state (PACKFILE * file, int version)
{
    pack_putc (mmc1_previous_register, file);

    pack_putc (mmc1_bit_stream, file);
    pack_putc (mmc1_bit_counter, file);

    pack_fwrite (mmc1_register, 4, file);
}


static void mmc1_load_state (PACKFILE * file, int version)
{
    mmc1_previous_register = pack_getc (file);

    mmc1_bit_stream = pack_getc (file);
    mmc1_bit_counter = pack_getc (file);


    mmc1_set_register_value (0, pack_getc (file));
    mmc1_set_register_value (1, pack_getc (file));
    mmc1_set_register_value (2, pack_getc (file));
    mmc1_set_register_value (3, pack_getc (file));
}

