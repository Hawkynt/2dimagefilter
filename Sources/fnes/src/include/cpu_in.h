static INLINE UINT8 FN2A03_Read (UINT16);

/* Wrapper for memory patching. */
#define CPU_PATCH_FIXUP(value)   (value + cpu_patch_table[address])

/* Macros to make things more maintainable (these are #undef'ed at the end
   of the file to prevent any possible conflicts). */
#define CPU_PAGE_2K_ADDRESS_SHIFT   11
#define CPU_PAGE_2K_SIZE            2048
#define CPU_PAGE_16K_SIZE           16384
#define CPU_PAGE_NK_ADDRESS_MASK(n) (~((n << 10) - 1))
#define CPU_PAGE_NK_SIZE(n)         (n << 10)

static INLINE void cpu_set_read_address_2k (UINT16 start, UINT8 *address)
{
   int index;
 
   /* ignore low bits */
   start &= CPU_PAGE_NK_ADDRESS_MASK(2);

   index = (start >> CPU_PAGE_2K_ADDRESS_SHIFT);

   cpu_block_2k_read_address[index] = (address - start);
   cpu_block_2k_read_handler[index] = cpu_read_direct_safeguard;

   /* Check if we are using patches. */
   if (cpu_patch_count > 0)
   {
      for (index = 0; index < cpu_patch_count; index++)
      {
         CPU_PATCH *patch = &cpu_patch_info[index];

         if (!patch->enabled)
            continue;

         if ((patch->address < (start + CPU_PAGE_2K_SIZE)) &&
             (patch->address >= start))
         {
            UINT8 value;

            if (patch->active)
            {
               /* Disable patch. */
               patch->active = FALSE;
               cpu_patch_table[patch->address] = 0;
            }

            value = FN2A03_Read (patch->address);
            if (value == patch->match_value)
            {
               /* Enable patch. */
               patch->active = TRUE;
               cpu_patch_table[patch->address] = (patch->value - value);
            }
         }
      }
   }
}

static INLINE void cpu_set_write_address_2k (UINT16 start, UINT8 *address)
{
   int index;

   /* ignore low bits */
   start &= CPU_PAGE_NK_ADDRESS_MASK(2);

   index = (start >> CPU_PAGE_2K_ADDRESS_SHIFT);

   cpu_block_2k_write_address[index] = (address - start);
   cpu_block_2k_write_handler[index] = cpu_write_direct_safeguard;
}

static INLINE void cpu_set_read_handler_2k (UINT16 start, UINT8 (*handler)
   (UINT16 address))
{
   int index;

   /* ignore low bits */
   start &= CPU_PAGE_NK_ADDRESS_MASK(2);

   index = (start >> CPU_PAGE_2K_ADDRESS_SHIFT);

   cpu_block_2k_read_address[index] = 0;
   cpu_block_2k_read_handler[index] = handler;
}

static INLINE void cpu_set_write_handler_2k (UINT16 start, void (*handler)
   (UINT16 address, UINT8 value))
{
   int index;

   /* ignore low bits */
   start &= CPU_PAGE_NK_ADDRESS_MASK(2);

   index = (start >> CPU_PAGE_2K_ADDRESS_SHIFT);

   cpu_block_2k_write_address[index] = 0;
   cpu_block_2k_write_handler[index] = handler;
}

#define CPU_READ_ADDRESS_SETTER(block_size, page_size)   \
   static INLINE void cpu_set_read_address_ ##block_size ##k (UINT16 \
      start, UINT8 *address)  \
   {  \
      int size;   \
      \
      /* ignore low bits */   \
      start &= CPU_PAGE_NK_ADDRESS_MASK(block_size);  \
      size = CPU_PAGE_NK_SIZE(page_size); \
      \
      cpu_set_read_address_## page_size ##k (start, address);  \
      cpu_set_read_address_## page_size ##k ((start + size), (address + \
         size));  \
   }

CPU_READ_ADDRESS_SETTER(4,  2)
CPU_READ_ADDRESS_SETTER(8,  4)
CPU_READ_ADDRESS_SETTER(16, 8)
CPU_READ_ADDRESS_SETTER(32, 16)

#undef CPU_READ_ADDRESS_SETTER

#define CPU_WRITE_ADDRESS_SETTER(block_size, page_size)  \
   static INLINE void cpu_set_write_address_## block_size ##k (UINT16   \
      start, UINT8 *address)  \
   {  \
      int size;   \
      \
      /* ignore low bits */   \
      start &= CPU_PAGE_NK_ADDRESS_MASK(block_size);  \
      size = CPU_PAGE_NK_SIZE(page_size); \
      \
      cpu_set_write_address_ ##page_size ##k (start, address); \
      cpu_set_write_address_ ##page_size ##k ((start + size), (address +   \
         size));  \
      \
   }

CPU_WRITE_ADDRESS_SETTER(4,  2)
CPU_WRITE_ADDRESS_SETTER(8,  4)
CPU_WRITE_ADDRESS_SETTER(16, 8)
CPU_WRITE_ADDRESS_SETTER(32, 16)

#undef CPU_WRITE_ADDRESS_SETTER

#define CPU_READ_HANDLER_SETTER(block_size, page_size)   \
   static INLINE void cpu_set_read_handler_ ##block_size ##k (UINT16 \
      start, UINT8 (*handler) (UINT16 address)) \
   {  \
      int size;   \
      \
      /* ignore low bits */   \
      start &= CPU_PAGE_NK_ADDRESS_MASK(block_size);  \
      size = CPU_PAGE_NK_SIZE(page_size); \
      \
      cpu_set_read_handler_ ##page_size ##k (start, handler);  \
      cpu_set_read_handler_ ##page_size ##k ((start + size), handler);  \
   }

CPU_READ_HANDLER_SETTER(4,  2)
CPU_READ_HANDLER_SETTER(8,  4)
CPU_READ_HANDLER_SETTER(16, 8)
CPU_READ_HANDLER_SETTER(32, 16)

#undef CPU_READ_HANDLER_SETTER

#define CPU_WRITE_HANDLER_SETTER(block_size, page_size)  \
   static INLINE void cpu_set_write_handler_## block_size ##k (UINT16   \
      start, void (*handler) (UINT16 address, UINT8 value)) \
   {  \
      int size;   \
      \
      /* ignore low bits */   \
      start &= CPU_PAGE_NK_ADDRESS_MASK(block_size);  \
      size = CPU_PAGE_NK_SIZE(page_size); \
      \
      cpu_set_write_handler_ ##page_size ##k (start, handler); \
      cpu_set_write_handler_ ##page_size ##k ((start + size), handler); \
   }

CPU_WRITE_HANDLER_SETTER(4,  2)
CPU_WRITE_HANDLER_SETTER(8,  4)
CPU_WRITE_HANDLER_SETTER(16, 8)
CPU_WRITE_HANDLER_SETTER(32, 16)

#undef CPU_WRITE_HANDLER_SETTER

/* ----- ROM-specific banking ----- */

static INLINE void cpu_set_read_address_8k_rom_block (UINT16 start, int
   block)
{
   UINT8 *address;

   address = (ROM_PRG_ROM + (((block & 1) + (ROM_PRG_ROM_PAGE_LOOKUP[((block
      / 2) & ROM_PRG_ROM_PAGE_OVERFLOW_MASK)] * 2)) << 13));

   cpu_set_read_address_8k (start, address);
}

static INLINE void cpu_set_read_address_16k_rom_block (UINT16 start, int
   block)
{
   UINT8 *address;

   address = (ROM_PRG_ROM + (ROM_PRG_ROM_PAGE_LOOKUP[(block &
      ROM_PRG_ROM_PAGE_OVERFLOW_MASK)] << 14));

   cpu_set_read_address_16k (start, address);
}

static INLINE void cpu_set_read_address_32k_rom_block (UINT16 start, int
   block)
{
   block <<= 1;

   cpu_set_read_address_16k_rom_block (start, block);
   cpu_set_read_address_16k_rom_block ((start + CPU_PAGE_16K_SIZE), (block +
      1));
}

/* ----- FN2A03 Routines ----- */

static INLINE UINT8 FN2A03_Fetch (UINT16 Addr)
{
   int index;

   // printf ("FN2A03_Fetch at $%04x.\n", Addr);

   index = (Addr >> CPU_PAGE_2K_ADDRESS_SHIFT);

   if (cpu_block_2k_read_address[index] != 0)
   {
      UINT16 address = Addr;

      return (CPU_PATCH_FIXUP(cpu_block_2k_read_address[index][Addr]));
   }
   else
   {
      return (cpu_block_2k_read_handler[index] (Addr));
   }
}

static INLINE UINT8 FN2A03_Read (UINT16 Addr)
{
   int index;

   // printf ("FN2A03_Read at $%04x.\n", Addr);

   index = (Addr >> CPU_PAGE_2K_ADDRESS_SHIFT);

   if (cpu_block_2k_read_address[index] != 0)
   {
      UINT16 address = Addr;

      return (CPU_PATCH_FIXUP(cpu_block_2k_read_address[index][Addr]));
   }
   else
   {
      return (cpu_block_2k_read_handler[index] (Addr));
   }
}

static INLINE void FN2A03_Write (UINT16 Addr, UINT8 Value)
{
   int index;

   // printf ("FN2A03_Write at $%04x ($%02x).\n", Addr, Value);

   index = (Addr >> CPU_PAGE_2K_ADDRESS_SHIFT);

   if (cpu_block_2k_write_address[index] != 0)
      cpu_block_2k_write_address[index][Addr] = Value;
   else
      cpu_block_2k_write_handler[index] (Addr, Value);
}

#ifdef INLINE_WITH_MACROS

#define FN2A03_Read_Stack(S)        (cpu_ram[(0x100 + (UINT8)(S))])
#define FN2A03_Write_Stack(S,Value) (cpu_ram[(0x100 + (UINT8)(S))] = Value)
#define FN2A03_Read_ZP(S)           (cpu_ram[(UINT8)(S)])
#define FN2A03_Write_ZP(S,Value)    (cpu_ram[(UINT8)(S)] = Value)

#else /* INLINE_WITH_MACROS */

static INLINE UINT8 FN2A03_Read_Stack (UINT8 S)
{
   return (cpu_ram[(0x100 + S)]);
}

static INLINE void FN2A03_Write_Stack (UINT8 S, UINT8 Value)
{
   cpu_ram[(0x100 + S)] = Value;
}

static INLINE UINT8 FN2A03_Read_ZP (UINT8 S)
{
   return (cpu_ram[S]);
}

static INLINE void FN2A03_Write_ZP (UINT8 S, UINT8 Value)
{
   cpu_ram[S] = Value;
}

#endif   /* !INLINE_WITH_MACROS */

/* --- CPU routines. --- */

static INLINE UINT8 cpu_read (UINT16 address)
{
   return (FN2A03_Read (address));
}

static INLINE void cpu_write (UINT16 address, UINT8 value)
{
   FN2A03_Write (address, value);
}

static INLINE void cpu_execute (int cycles)
{
   cpu_context.ICount += cycles;

   FN2A03_Run (&cpu_context);
}

static INLINE void cpu_consume_cycles (int cycles)
{
   FN2A03_consume_cycles (&cpu_context, cycles);
}

/* Remove macros to prevent conflicts. */
#undef CPU_PAGE_2K_ADDRESS_SHIFT
#undef CPU_PAGE_2K_SIZE
#undef CPU_PAGE_16K_SIZE
#undef CPU_PAGE_NK_ADDRESS_MASK
#undef CPU_PAGE_NK_SIZE
