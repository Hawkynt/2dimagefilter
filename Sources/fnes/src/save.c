/* FakeNES - A free, portable, Open Source NES emulator.

   save.c: Implementation of the save data routines.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "apu.h"
#include "common.h"
#include "cpu.h"
#include "debug.h"
#include "input.h"
#include "ppu.h"
#include "rom.h"
#include "save.h"
#include "timing.h"
#include "types.h"

/* --- Utility functions. --- */

/* Text that appears in "unused" menu slots for states and replays. */
#define UNUSED_SLOT_TEXT   "Empty"

/* FNSS version supported/created. */
#define FNSS_VERSION 0x200

static INLINE BOOL fnss_save_chunk (PACKFILE *file, int version, const char
   *id, void (*save_state) (PACKFILE *, int))
{
   /* This function opens a chunk for the component with the specified
      4-character ID, then invokes it's save handler within that chunk.

      Returns TRUE on success, FALSE on failure. */

   PACKFILE *chunk;

   RT_ASSERT(file);
   RT_ASSERT(id);
   RT_ASSERT(save_state);

   /* Write ID. */
   pack_fwrite (id, 4, file);

   /* Open chunk. */
   chunk = pack_fopen_chunk (file, FALSE);
   if (!chunk)
   {
      WARN_GENERIC();
      return (FALSE);
   }

   /* Invoke save handler. */
   save_state (chunk, version);

   /* Close chunk. */
   pack_fclose_chunk (chunk);

   /* Return success. */
   return (TRUE);
}

static INLINE BOOL fnss_load_chunk (PACKFILE *file, int version, const char
   *id, void (*load_state) (PACKFILE *, int))
{
   /* Same as above, but or loading instead. */

   PACKFILE *chunk;
   UINT8 signature[4];

   RT_ASSERT(file);
   RT_ASSERT(id);
   RT_ASSERT(load_state);

   /* Read ID. */
   pack_fread (signature, 4, file);

   /* We ignore signatures for now, this will be used in the future to load
      chunks in any order. */

   /* Open chunk. */
   chunk = pack_fopen_chunk (file, FALSE);
   if (!chunk)
   {
      WARN_GENERIC();
      return (FALSE);
   }

   /* Invoke load handler. */
   load_state (chunk, version);

   /* Close chunk. */
   pack_fclose_chunk (chunk);

   /* Return success. */
   return (TRUE);
}

static INLINE BOOL fnss_save (PACKFILE *file, const UCHAR *title)
{
   UINT16 version;
   UCHAR save_title[NEW_SAVE_TITLE_SIZE];
   int size;

   /* Core FNSS (FakeNES save state) saving code.  Returns TRUE if the save
      suceeded or FALSE if the save failed (which can't happen). */

   RT_ASSERT(file);
   RT_ASSERT(title);

   /* Set version. */
   version = FNSS_VERSION;

   /* Write signature. */
   pack_fwrite ("FNSS", 4, file);
   
   /* Write version number. */
   pack_iputw (version, file);
   
   /* Write title. */
   /* Version 1.03 of the format adds variable-length titles, up to a
      maximum of 255 bytes (no NULL character is needed). */

   USTRING_CLEAR_SIZE(save_title, sizeof (save_title));
   ustrncat (save_title, title, (sizeof (save_title) - 1));

   size = ustrsize (save_title);
   pack_putc (size, file);

   pack_fwrite (save_title, size, file);
   
   /* Write CRC32s. */
   pack_iputl (global_rom.trainer_crc32, file);
   pack_iputl (global_rom.prg_rom_crc32, file);
   pack_iputl (global_rom.chr_rom_crc32, file);
   
   /* Write CPU chunk. */
   fnss_save_chunk (file, version, "CPU\0", cpu_save_state);

   /* Write MMC chunk. */
   fnss_save_chunk (file, version, "MMC\0", mmc_save_state);
   
   /* Write PPU chunk. */
   fnss_save_chunk (file, version, "PPU\0", ppu_save_state);
   
   /* Write APU chunk. */
   fnss_save_chunk (file, version, "APU\0", apu_save_state);
   
   /* Write CTRL chunk. */
   fnss_save_chunk (file, version, "CTRL", input_save_state);

   /* Return success. */
   return (TRUE);
}

static INLINE BOOL fnss_load (PACKFILE *file)
{
   /* Core FNSS (FakeNES save state) loading code.  Returns TRUE if the
      load suceeded or FALSE if the load failed. */

   UINT8 signature[4];
   UINT16 version;
   UCHAR title[NEW_SAVE_TITLE_SIZE_Z];
   UINT32 trainer_crc;
   UINT32 prg_rom_crc;
   UINT32 chr_rom_crc;

   RT_ASSERT(file);

   /* Fetch signature. */
   pack_fread (signature, 4, file);

   /* Verify signature. */
   if (strncmp (signature, "FNSS", 4))
   {
      /* Verification failed. */
      return (FALSE);
   }

   /* Fetch version number. */
   version = pack_igetw (file);

   /* Verify version number. */

   if (version < 0x200)
   {
      /* Only save states of version 2.xx are supported. */
      return (FALSE);
   }

   if (version > FNSS_VERSION)
   {
      /* Save state file is of a future version. */
      return (FALSE);
   }

   /* Fetch save title. */

   USTRING_CLEAR_SIZE(title, sizeof (title));
   pack_fread (title, pack_getc (file), file);

   /* Fetch CRC32s. */
   trainer_crc = pack_igetl (file);
   prg_rom_crc = pack_igetl (file);
   chr_rom_crc = pack_igetl (file);

   /* Verify CRC32s. */
   if ((trainer_crc != global_rom.trainer_crc32) ||
       (prg_rom_crc != global_rom.prg_rom_crc32) ||
       (chr_rom_crc != global_rom.chr_rom_crc32))
   {
      /* Verification failed. */
      return (FALSE);
   }

   /* Reset the virtual machine to it's initial state. */
   machine_reset ();

   /* Keep these in order!

      Version 1.00 had a broken chunk order that might've caused problems
      in the chunk-based code, but completely broke the raw code. */

   /* Load CPU chunk. */
   fnss_load_chunk (file, version, "CPU\0", cpu_load_state);

   /* Load MMC chunk. */
   fnss_load_chunk (file, version, "MMC\0", mmc_load_state);

   /* Load PPU chunk. */
   fnss_load_chunk (file, version, "PPU\0", ppu_load_state);

   /* Load APU chunk. */
   fnss_load_chunk (file, version, "APU\0", apu_load_state);

   /* Load CTRL chunk. */
   fnss_load_chunk (file, version, "CTRL", input_load_state);

   /* Return success. */
   return (TRUE);
}

static INLINE BOOL fnss_save_raw (PACKFILE *file)
{
   int version;

   /* This function saves a minimalist save state.  Used primarily for game
      snapshots created by the real-time game rewinding feature. */

   RT_ASSERT(file);

   /* Set version. */
   version = FNSS_VERSION;

   /* Dump CPU state. */
   cpu_save_state (file, version);

   /* Dump MMC state. */
   mmc_save_state (file, version);

   /* Dump PPU state. */
   ppu_save_state (file, version);

   /* Dump APU state. */
   apu_save_state (file, version);

   /* Dump input state. */
   input_save_state (file, version);

   /* Return success. */
   return (TRUE);
}

static INLINE BOOL fnss_load_raw (PACKFILE *file)
{
   /* Same as fnss_save_raw(), but for loading instead. */

   int version;

   RT_ASSERT(file);

   /* Set version. */
   version = FNSS_VERSION;

   /* Reset the virtual machine to it's initial state. */
   machine_reset ();

   /* Restore CPU state. */
   cpu_load_state (file, version);

   /* Restore MMC state. */
   mmc_load_state (file, version);

   /* Restore PPU state. */
   ppu_load_state (file, version);

   /* Restore APU state. */
   apu_load_state (file, version);

   /* Restore input state. */
   input_load_state (file, version);

   /* Return success. */
   return (TRUE);
}

static INLINE UCHAR *get_save_filename (UCHAR *filename, const UCHAR *ext,
   int size)
{
   /* THis function builds a path and filename suitable for the storage of
      save data, using the name of the ROM combined with the extension
      'ext', and stores up to 'size' Unicode characters in 'filename'. */

   USTRING path;

   RT_ASSERT(filename);
   RT_ASSERT(ext);

   /* Grab the filename of the currently loaded ROM. */
   ustrzcpy (path, sizeof (path), get_filename (global_rom.filename));

   /* Merge it with our save path. */
   get_save_path (path, sizeof (path));

   /* Change the extension. */
   replace_extension (path, path, ext, sizeof (path));

   /* Copy to output. */
   USTRING_CLEAR_SIZE(filename, size);
   ustrncat (filename, path, (size - 1));

   return (filename);
}

static INLINE UCHAR *get_state_filename (UCHAR *filename, int index, int
   size)
{
   /* This function generates the path and filename for the state file
      associated with the state slot 'index'.  State files are stored in
      the save path, and have a .fn# extension.  If 'index' is -1, the
      quicksave feature is used instead, which has a dedicated state file
      with a .fsv extension. */

   USTRING ext;

   RT_ASSERT(filename);

   /* Build extension. */
   if (index == -1)
   {
      USTRING_CLEAR(ext);
      ustrncat (ext, "fsv", (sizeof (ext) - 1));
   }
   else
      uszprintf (ext, sizeof (ext), "fn%d", index);

   /* Generate filename. */
   get_save_filename (filename, ext, size);

   return (filename);
}

static INLINE UCHAR *get_replay_filename (UCHAR *filename, int index, int
   size)
{
   /* This function generates the path and filename for the replay file
      associated with the replay slot 'index'.  Replay files are stored in
      the save path, and have a .fr# extension. */

   USTRING ext;

   RT_ASSERT(filename);

   /* Build extension. */
   uszprintf (ext, sizeof (ext), "fr%d", index);

   /* Generate filename. */
   get_save_filename (filename, ext, size);

   return (filename);
}

static INLINE UCHAR *get_save_title (const UCHAR *filename, UCHAR *title,
   int size)
{
   /* This function retrives the save title from the state or replay file
      specified in 'filename'.  Returns either the retrieved title,
      "Untitled" if the retrieved title was zero-length, OR
      UNUSED_SLOT_TEXT if the file could not be opened. */

   PACKFILE *file;
   USTRING save_title;

   RT_ASSERT(filename);
   RT_ASSERT(title);

   USTRING_CLEAR(save_title);

   file = pack_fopen (filename, "r");

   if (file)
   {
      UINT8 signature[4];
      UINT16 version;

      /* Probably don't need to verify these... */
      pack_fread (signature, 4, file);
      version = pack_igetw (file);

      if (version <= 0x102)
         pack_fread (save_title, SAVE_TITLE_SIZE, file);
      else
         pack_fread (save_title, pack_getc (file), file);

      pack_fclose (file);

      if (ustrlen (save_title) == 0)
         ustrncat (save_title, "Untitled", (sizeof (save_title) - 1));
   }
   else
   {
      ustrncat (save_title, UNUSED_SLOT_TEXT, (sizeof (save_title) - 1));
   }

   /* Copy to output. */
   ustrzncpy (title, size, save_title, sizeof (save_title));

   return (title);
}

static UCHAR *get_patches_filename (UCHAR *filename, int size)
{
   /* This function generates the path and filename for the patches (aka
      cheats) file.  Patch table files are plain text, stored in the save
      path, and have a .fpt extension. */

   get_save_filename (filename, "fpt", size);

   return (filename);
}

static UCHAR *get_sram_filename (UCHAR *filename, int size)
{
   /* This function generates the path and filename for the save RAM (SRAM)
      Save RAM is stored in the save path, and has a .sav extension. */

   get_save_filename (filename, "sav", size);

   return (filename);
}

/* --- Public functions. --- */

/* -- Replay functions. -- */

UCHAR *get_replay_title (int index, UCHAR *title, int size)
{
   /* This function gets the title of the replay # 'index' and stores up
      to 'size' characters of it in 'title'.  Returns a copy of 'title'.
      */

   USTRING filename;

   /* Generate filename. */
   get_replay_filename (filename, index, sizeof (filename));

   /* Retrieve title. */
   get_save_title (filename, title, size);

   return (title);
}

static PACKFILE *replay_file = NULL;
static PACKFILE *replay_file_chunk = NULL;

BOOL open_replay (int index, const char *mode, const UCHAR *title)
{
   /* This function begins reading or writing an FNSS-format save state
      with an open REPL(replay) chunk appended to it.  The REPL chunk must
      later be closed by a call to close_replay().  Returns TRUE on
      success, or FALSE on failure. */

   USTRING filename;
   PACKFILE *file;

   RT_ASSERT(mode);

   /* Generate filename. */
   get_replay_filename (filename, index, sizeof (filename));

   if (strcmp (mode, "r") == 0)
   {
      /* Open for reading. */

      PACKFILE *chunk;
      UINT8 signature[4];

      file = pack_fopen (filename, "r");
      if (!file)
         return (FALSE);

      /* Load state. */
      if (!fnss_load (file))
      {
         /* Load failed. */
         pack_fclose (file);
         return (FALSE);
      }
   
      /* Load suceeded. */
   
      /* Open REPL chunk. */
      pack_fread (signature, 4, file);
   
      /* Verify REPL chunk. */
      if (strncmp (signature, "REPL", 4))
      {
         /* Verification failed. */
         pack_fclose (file);
         return (FALSE);
      }

      replay_file_chunk = pack_fopen_chunk (file, FALSE);
      replay_file = file;

      return (TRUE);
   }
   else if (strcmp (mode, "w") == 0)
   {
      /* Open for writing. */

      PACKFILE *chunk;

      RT_ASSERT(title);

      file = pack_fopen (filename, "w");
      if (!file)
         return (FALSE);

      /* Save state. */
      fnss_save (file, title);

      /* Open REPL chunk. */
      pack_fwrite ("REPL", 4, file);
      chunk = pack_fopen_chunk (file, FALSE);

      replay_file = file;
      replay_file_chunk = chunk;

      return (TRUE);
   }
   else
   {
      /* Invalid mode. */
      return (FALSE);
   }
}

void close_replay (void)
{
   /* This function closes a replay file previously opened by
      open_replay(). */

   /* TODO: Make sure replay file is open. */

   pack_fclose_chunk (replay_file_chunk);
   pack_fclose (replay_file);
}

BOOL get_replay_data (UINT8 *data)
{
   /* This function reads 8-bit replay data from an open replay file that
      was opened in read mode.  Returns TRUE if the end of file was reached
      during this operation (the replay has finished playing), or FALSE if
      there is still more data to be read. */

   /* TODO: Make sure replay file is open. */

   *data = pack_getc (replay_file_chunk);

   return (pack_feof (replay_file_chunk));
}

void save_replay_data (UINT8 data)
{
   /* This function writes 8-bit replay data to an open replay file that was
      opened in write mode. */

   /* TODO: Make sure replay file is open. */

   pack_putc (data, replay_file_chunk);
}

/* --- Save state functions. --- */

UCHAR *get_state_title (int index, UCHAR *title, int size)
{
   /* This function gets the title of the state # 'index' and stores up
      to 'size' characters of it in 'title'.  Returns a copy of 'title'.
      */

   USTRING filename;

   /* Generate filename. */
   get_state_filename (filename, index, sizeof (filename));

   /* Retrieve title. */
   get_save_title (filename, title, size);

   return (title);
}

BOOL save_state (int index, const UCHAR *title)
{
   /* index == -1 == quicksave. */

   USTRING filename;
   PACKFILE *file;

   RT_ASSERT(title);

   /* Generate filename. */
   get_state_filename (filename, index, sizeof (filename));

   /* Open file. */
   file = pack_fopen (filename, "w");
   if (!file)
      return (FALSE);

   /* Save state. */
   fnss_save (file, title);

   /* Close file. */
   pack_fclose (file);

   return (TRUE);
}

BOOL load_state (int index)
{
   /* index == -1 == quickload. */

   USTRING filename;
   PACKFILE *file;

   /* Generate filename. */
   get_state_filename (filename, index, sizeof (filename));

   /* Open file. */
   file = pack_fopen (filename, "r");
   if (!file)
      return (FALSE);

   /* Load state. */
   fnss_load (file);

   /* Close file. */
   pack_fclose (file);

   return (TRUE);
}

BOOL save_state_raw (PACKFILE *file)
{
   /* Global alias for fnss_save_raw(). */

   RT_ASSERT(file);

   return (fnss_save_raw (file));
}

BOOL load_state_raw (PACKFILE *file)
{
   /* Global alias for fnss_load_raw(). */

   RT_ASSERT(file);

   return (fnss_load_raw (file));
}

BOOL check_save_state (int index)
{
   /* index == -1 == quicksave.

      This function tells whether or not a save state exists.  It is usually
      used to make sure that a call to save_state() succeeded when the
      return value of said call cannot be compared directly.

      This function does *NOT* check if a save state is "valid". :b */

   USTRING filename;

   /* Generate filename. */
   get_state_filename (filename, index, sizeof (filename));

   return (exists (filename));
}

/* --- Patches. --- */

BOOL load_patches (void)
{
   USTRING filename;
   int version;
   int index;

   /* Clear patch information. */
   memset (cpu_patch_info, NULL, sizeof (cpu_patch_info));
   cpu_patch_count = 0;

   /* Get filename. */
   get_patches_filename (filename, sizeof (filename));

   if (!exists (filename))
      return (FALSE);

   push_config_state ();
   set_config_file (filename);

   /* Fetch and verify version. */
   version = get_config_hex ("header", "version", 0x0101);
   if (version > 0x101)
   {
      /* Verification failed. */
      pop_config_state ();
      return (FALSE);
   }

   cpu_patch_count = get_config_int ("header", "patch_count", 0);
   if (cpu_patch_count > CPU_MAX_PATCHES)
      cpu_patch_count = CPU_MAX_PATCHES;
   else if (cpu_patch_count < 0)
      cpu_patch_count = 0;

   for (index = 0; index < cpu_patch_count; index++)
   {
      CPU_PATCH *patch = &cpu_patch_info[index];
      USTRING section;

      USTRING_CLEAR(section);
      uszprintf (section, sizeof (section), "patch%02d", index);

      /* Read title. */
      ustrncat (patch->title, get_config_string (section, "title", "?"),
         MIN(NEW_SAVE_TITLE_SIZE_Z, (USTRING_SIZE - 1)));

      /* Load data. */
      patch->address     = get_config_hex (section, "address",     0xffff);
      patch->value       = get_config_hex (section, "value",       0xff);
      patch->match_value = get_config_hex (section, "match_value", 0xff);
      patch->enabled     = get_config_int (section, "enabled",     FALSE);

      /* Deactivate patch. */
      patch->active = FALSE;
   }

   pop_config_state ();

   return (TRUE);
}

BOOL save_patches (void)
{
   USTRING filename;
   int index;

   /* Get filename. */
   get_patches_filename (filename, sizeof (filename));

   if (exists (filename))
   {
      /* Delete old tables. */
      remove (filename);
   }

   if (cpu_patch_count == 0)
      return (FALSE);

   push_config_state ();
   set_config_file (filename);

   set_config_hex ("header", "version",     0x0100);
   set_config_int ("header", "patch_count", cpu_patch_count);

   for (index = 0; index < cpu_patch_count; index++)
   {
      CPU_PATCH *patch = &cpu_patch_info[index];
      USTRING section;
      USTRING *title;

      USTRING_CLEAR(section);
      uszprintf (section, sizeof (section), "patch%02d", index);

      /* Save data. */
      set_config_string (section, "title",       patch->title);
      set_config_hex    (section, "address",     patch->address);
      set_config_hex    (section, "value",       patch->value);
      set_config_hex    (section, "match_value", patch->match_value);
      set_config_int    (section, "enabled",     patch->enabled);
   }

   pop_config_state ();

   return (TRUE);
}

/* --- Save RAM (SRAM). --- */

BOOL load_sram (void)
{
   USTRING filename;
   PACKFILE *file;

   /* Make sure cart contains SRAM. */
   if (!global_rom.sram_flag)
      return (FALSE);

   /* Get filename. */
   get_sram_filename (filename, sizeof (filename));

   if (!exists (filename))
      return (FALSE);

   /* Open file. */
   file = pack_fopen (filename, "r");
   if (!file)
      return (FALSE);

   /* Load data. */
   pack_fread (cpu_sram, CPU_SRAM_SIZE, file);

   /* Close file. */
   pack_fclose (file);

   return (TRUE);
}

BOOL save_sram (void)
{
   USTRING filename;
   PACKFILE *file;

   /* Make sure cart contains SRAM. */
   if (!global_rom.sram_flag)
      return (FALSE);

   /* Get filename. */
   get_sram_filename (filename, sizeof (filename));

   /* Open file. */
   file = pack_fopen (filename, "w");
   if (!file)
      return (FALSE);

   /* Save data. */
   pack_fwrite (cpu_sram, CPU_SRAM_SIZE, file);

   /* Close file. */
   pack_fclose (file);

   return (TRUE);
}

/* --- Miscellaneous. --- */

UCHAR *get_save_path (UCHAR *filename, int size)
{                                     
   /* This function places the filename part of 'filename' at the end of
      the currently defined save path.

      This function modifies 'filename' in-place, so it should not be
      constant.  'size' is the absolute maximum size of 'filename'. */

   USTRING path;

   RT_ASSERT(filename);

   USTRING_CLEAR(path);
   ustrncat (path, get_config_string ("gui", "save_path", "./"), (sizeof
      (path) - 1));
   put_backslash (path);
   ustrncat (path, get_filename (filename), (sizeof (path) - 1));
   
   /* Copy to output. */
   USTRING_CLEAR_SIZE(filename, size);
   ustrncat (filename, path, (size - 1));

   return (filename);
}

UCHAR *fix_save_title (UCHAR *title, int size)
{
   /* This function compares 'title' against UNUSED_SLOT_TEXT, and if they
      are found to be the same, it replaces it with "Untitled" instead.

      Without this function, we might end up with untitled state and
      replay files with UNUSED_SLOT_TEXT as their title (lifted directly
      rom their cooresponding menu slot), and that would be icky.

      If 'title' is found to not be equal to UNUSED_SLOT_TEXT, then it
      will remain unchanged, but will still be returned. */

   RT_ASSERT(title);

   if (ustrncmp (title, UNUSED_SLOT_TEXT, size) == 0)
   {
      USTRING_CLEAR_SIZE(title, size);
      ustrncat (title, "Untitled", (size - 1));
   }

   return (title);
}
