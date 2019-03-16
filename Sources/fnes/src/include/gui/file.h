/* Custom file selector. */

/* Drive letters for Windows.  They are appended to the directory list. */
#define FS_DRIVE_FIRST  'A'
#define FS_DRIVE_LAST   'Z'

/* We store each file and directory name in this special structure designed
   to minimize memory usage. */

typedef struct FS_LIST_ENTRY
{
   UCHAR *text;   /* Pointer to the text, allocated by malloc(). */
   int size;      /* Size of the text, including null terminator. */

} FS_LIST_ENTRY;

static struct
{
   const UCHAR *ext;

   USTRING path;

   DIALOG *objshow, *objfiles, *objdirs, *objfile, *objok;

   FS_LIST_ENTRY *files;
   int num_files;

   FS_LIST_ENTRY *dirs;
   int num_dirs;

   BOOL show_hidden;

} fs_info;

static int fs_sorter (const void *a, const void *b)
{
   const FS_LIST_ENTRY *ax, *bx;

   RT_ASSERT(a);
   RT_ASSERT(b);

   ax = (const FS_LIST_ENTRY *)a;
   bx = (const FS_LIST_ENTRY *)b;

   return (ustricmp (ax->text, bx->text));
}

static INLINE void fs_set_path (const UCHAR *dirname)
{
   /* Helper function to set 'dirname' as the new path (it may either be
      a relative path, or an absolute path, including a drive letter on
      DOS/Windows), and update all relative information. */

   USTRING buffer;
   USTRING *path;

   RT_ASSERT(dirname);

   if (is_relative_filename (dirname))
   {
      /* Copy path to buffer. */
      ustrzncpy (buffer, sizeof (buffer), fs_info.path, ustrsize
         (fs_info.path));
   
      /* Strip out filename (if any). */
      replace_filename (buffer, buffer, empty_string, (sizeof (buffer) - 1));

      /* Append directory name. */
      ustrzncat (buffer, sizeof (buffer), dirname, ustrsize (dirname));
   }
   else
   {
      /* Set directory. */
      ustrzncpy (buffer, sizeof (buffer), dirname, ustrsize (dirname));
   }

   /* Append path separator. */
   put_backslash (buffer);

   /* Canonicalize the path into proper form. */
   canonicalize_filename (buffer, buffer, (sizeof (buffer) - 1));

   /* Copy buffer to path. */
   ustrzncpy (fs_info.path, sizeof (fs_info.path), buffer, ustrsize
      (buffer));

   /* Update filename textbox. */
   ustrzncpy (fs_info.objfile->dp, fs_info.objfile->d1, buffer, ustrsize
      (buffer));

   /* Refresh everything. */

   if (fs_info.files)
   {
      free (fs_info.files);
      fs_info.files = NULL;
   }

   if (fs_info.dirs)
   {
      free (fs_info.dirs);
      fs_info.dirs = NULL;
   }

   fs_info.num_files = -1;
   fs_info.num_dirs  = -1;

   scare_mouse ();

   object_message (fs_info.objfiles, MSG_DRAW, 0);
   object_message (fs_info.objdirs,  MSG_DRAW, 0);
   object_message (fs_info.objfile,  MSG_DRAW, 0);

   unscare_mouse ();
}

static int fs_get_file_callback (const char *filename, int attrib, void
   *param)
{
   FS_LIST_ENTRY *entry;

   RT_ASSERT(filename);

   /* Manually strip out Unix-style hidden files. */
   if ((ugetc (filename) == '.') &&
       !fs_info.show_hidden)
   {
      return (0);
   }

   if (!fs_info.files)
   {
      /* Allocate first entry. */
      fs_info.files = malloc (sizeof (FS_LIST_ENTRY));
   }
   else
   {
      /* Allocate a new entry. */

      fs_info.files = realloc (fs_info.files, (sizeof (FS_LIST_ENTRY) *
         (fs_info.num_files + 1)));
   }
   
   if (!fs_info.files)
   {
      WARN("Out of memory");
      return (-1);
   }

   /* Strip out preceeding garbage. */
   filename = get_filename (filename);

   entry = &fs_info.files[fs_info.num_files];

   entry->size = ustrsizez (filename);

   entry->text = malloc (entry->size);
   if (!entry->text)
   {
      WARN("Out of memory");
      return (-1);
   }

   ustrzncpy (entry->text, entry->size, filename, ustrsize (filename));

   fs_info.num_files++;

   return (0);
}

static int fs_get_directory_callback (const char *filename, int attrib, void
   *param)
{
   USTRING buffer;
   FS_LIST_ENTRY *entry;

   RT_ASSERT(filename);

   if (param)
   {
      /* An alternate (or identical) filename has been passed in 'param',
         meaning it didn't come from Allegro, so we leave it intact. */
      filename = param;
   }
   else
   {
      /* Strip out preceeding garbage. */
      filename = get_filename (filename);

      /* Avoid superfluous directory entries. */
      if ((ustrncmp (filename, ".", ustrlen (filename)) == 0) ||
          (ustrncmp (filename, "..", ustrlen (filename)) == 0))
      {
         return (0);
      }

      /* Manually strip out Unix-style hidden directories. */
      if ((ugetc (filename) == '.') &&
          !fs_info.show_hidden)
      {                      
         return (0);
      }
   }

   if (!fs_info.dirs)
   {
      /* Allocate first entry. */
      fs_info.dirs = malloc (sizeof (FS_LIST_ENTRY));
   }
   else
   {
      /* Allocate a new entry. */

      fs_info.dirs = realloc (fs_info.dirs, (sizeof (FS_LIST_ENTRY) *
         (fs_info.num_dirs + 1)));
   }

   if (!fs_info.dirs)
   {
      WARN("Out of memory");
      return (-1);
   }

   /* Copy filename to buffer. */
   ustrzncpy (buffer, sizeof (buffer), filename, ustrsize (filename));

   /* Append path separator. */
   put_backslash (buffer);
   
   entry = &fs_info.dirs[fs_info.num_dirs];

   entry->size = ustrsizez (buffer);

   entry->text = malloc (entry->size);
   if (!entry->text)
   {
      WARN("Out of memory");
      return (-1);
   }

   ustrzncpy (entry->text, entry->size, buffer, ustrsize (buffer));

   fs_info.num_dirs++;

   return (0);           
}

static int file_select_dialog_show_hidden_files_checkbox (DIALOG *dialog)
{
   RT_ASSERT(dialog);

   fs_info.show_hidden = TRUE_OR_FALSE(dialog->flags & D_SELECTED);

   /* Refresh file and directory lists. */

   if (fs_info.files)
   {
      free (fs_info.files);
      fs_info.files = NULL;
   }

   if (fs_info.dirs)
   {
      free (fs_info.dirs);
      fs_info.dirs = NULL;
   }

   fs_info.num_files = -1;
   fs_info.num_dirs  = -1;

   scare_mouse ();

   object_message (fs_info.objfiles, MSG_DRAW, 0);
   object_message (fs_info.objdirs,  MSG_DRAW, 0);

   unscare_mouse ();

   return (D_O_K);
}

static int file_select_dialog_file_list (DIALOG *dialog)
{
   /* This function gets called when an entry in the files listbox is
      clicked.

      'dialog->d1' holds the entry index. */

   int index;
   FS_LIST_ENTRY *entry;
   const UCHAR *filename;
   USTRING buffer;

   RT_ASSERT(dialog);

   /* Get file index. */
   index = dialog->d1;

   if ((index < 0) || (index >= fs_info.num_files))
      return (D_O_K);

   entry = &fs_info.files[index];

   if ((!entry) || (!entry->text))
   {
      /* This shouldn't happen, but we handle it just in case. */
      return (D_O_K);
   }

   filename = entry->text;

   /* Copy path to buffer. */
   ustrzncpy (buffer, sizeof (buffer), fs_info.path, ustrsize
      (fs_info.path));

   /* Replace filename (if any) with a new one. */
   replace_filename (buffer, buffer, filename, (sizeof (buffer) - 1));

   /* Copy buffer back to path. */
   ustrzncpy (fs_info.path, sizeof (fs_info.path), buffer, ustrsize
      (buffer));

   /* Update filename textbox. */
   ustrzncpy (fs_info.objfile->dp, fs_info.objfile->d1, buffer, ustrsize
      (buffer));

   /* Move cursor to the end of the string. */
   fs_info.objfile->d2 = ustrlen (buffer);

   /* Redraw filename textbox with the updated path and cursor positon. */

   scare_mouse ();
   object_message (fs_info.objfile, MSG_DRAW, 0);
   unscare_mouse ();

   return (D_O_K);
}

static char *file_select_dialog_file_list_filler (int index, int
   *list_size)
{
   if (fs_info.num_files == -1)
   {
      USTRING buffer;
      USTRING ext;
      UCHAR *mask;

      /* Copy path to buffer. */
      ustrzncpy (buffer, sizeof (buffer), fs_info.path, ustrsize
         (fs_info.path));

      /* Strip out filename (if any). */
      replace_filename (buffer, buffer, empty_string, (sizeof (buffer) -
         1));

      /* Append path separator. */
      put_backslash (buffer);

      if (fs_info.files)
      {
         /* Destroy existing file list. */
         free (fs_info.files);
         fs_info.files = NULL;
      }

      /* Clear file counter. */
      fs_info.num_files = 0;

      /* Copy extension list to a temporary buffer since ustrtok() changes
         the contents fo the string passed to it. */
      ustrzncpy (ext, sizeof (ext), fs_info.ext, ustrsize (fs_info.ext));

      /* Get first extension mask. */
      mask = ustrtok (ext, ";");

      do
      {
         USTRING buffer2;

         /* Copy buffer to secondary buffer. */
         ustrzncpy (buffer2, sizeof (buffer2), buffer, ustrsize (buffer));

         /* Append wildcards. */
         ustrzncat (buffer2, sizeof (buffer2), mask, ustrsize (mask));
   
         /* Build file list. */
         if (fs_info.show_hidden)
         {
            /* Including hidden files. */
            for_each_file_ex (buffer2, 0, FA_DIREC, fs_get_file_callback,
               NULL);
         }
         else
         {
            /* Excluding hidden files. */
            for_each_file_ex (buffer2, 0, (FA_DIREC | FA_HIDDEN |
               FA_SYSTEM), fs_get_file_callback, NULL);
         }

         /* Get next extension mask. */
         mask = ustrtok (NULL, ";");

      }  while (mask);

      if (fs_info.num_files > 0)
      {
         /* Sort the list alphabetically. */
         qsort (fs_info.files, fs_info.num_files, sizeof (FS_LIST_ENTRY),
            fs_sorter);

         /* Enable double-click. */
         fs_info.objfiles->flags |= D_EXIT;
      }
      else
      {
         /* Disable double-click. */
         fs_info.objfiles->flags &= ~D_EXIT;
      }
   }

   if (index >= 0)
   {
      FS_LIST_ENTRY *entry;

      entry = &fs_info.files[index];

      if (!entry)
         return (NULL);

      return (entry->text);
   }
   else
   {
      RT_ASSERT(list_size);

      *list_size = fs_info.num_files;

      return (NULL);
   }
}

static int file_select_dialog_directory_list (DIALOG *dialog)
{
   /* This function gets called when an entry in the directories listbox is
      clicked.

      'dialog->d1' holds the entry index. */

   int index;
   FS_LIST_ENTRY *entry;

   RT_ASSERT(dialog);

   /* Get directory index. */
   index = dialog->d1;

   if ((index < 0) || (index >= fs_info.num_dirs))
      return (D_O_K);

   entry = &fs_info.dirs[index];

   if ((!entry) || (!entry->text))
   {
      /* This shouldn't happen, but we handle it just in case. */
      return (D_O_K);
   }

   fs_set_path (entry->text);

   return (D_O_K);
}

static char *file_select_dialog_directory_list_filler (int index, int
   *list_size)
{
   if (fs_info.num_dirs == -1)
   {
      USTRING buffer;
      UINT32 drive_list;
      int index;
      BOOL is_root = FALSE;

      /* Copy path to buffer. */
      ustrzncpy (buffer, sizeof (buffer), fs_info.path, ustrsize
         (fs_info.path));

      /* Strip out filename (if any). */
      replace_filename (buffer, buffer, empty_string, (sizeof (buffer) -
         1));

      /* Append path separator. */
      put_backslash (buffer);

      /* TODO: Make sure the following code works properly on all supported
         operating systems (including Mac OS X). */
#if defined(ALLEGRO_WINDOWS) || defined(ALLEGRO_DOS)
      /* Root example: C:\ */
      if (ustrlen (buffer) < 4)
         is_root = TRUE;
#else
      /* Root example: / */
      if (ustrlen (buffer) < 2)
         is_root = TRUE;
#endif

      /* Append wildcards. */
      ustrncat (buffer, "*", (sizeof (buffer) - 1));

      if (fs_info.dirs)
      {
         /* Destroy existing directory list. */
         free (fs_info.dirs);
         fs_info.dirs = NULL;
      }

      /* Clear directory counter. */
      fs_info.num_dirs = 0;

      /* Manually add the otherwise hidden ".." entry for easier navigation
         (applicable to non-root paths only). */
      if (!is_root)
      {
         /* Note: This uses the same callback hack as the drive code. */
         fs_get_directory_callback ("..", FA_DIREC, "..");
      }

      /* Get directory list. */
      if (fs_info.show_hidden)
      {
         /* Including hidden directories. */
         for_each_file_ex (buffer, FA_DIREC, 0, fs_get_directory_callback,
            NULL);
      }
      else
      {
         /* Excluding hidden directories. */
         for_each_file_ex (buffer, FA_DIREC, (FA_HIDDEN | FA_SYSTEM),
            fs_get_directory_callback, NULL);
      }

      if (fs_info.num_dirs > 0)
      {
         /* Sort the list alphabetically. */
         qsort (fs_info.dirs, fs_info.num_dirs, sizeof (FS_LIST_ENTRY),
            fs_sorter);
      }

#if (defined (ALLEGRO_DOS) || defined (ALLEGRO_WINDOWS))

      /* Append logical drives. */

#ifdef ALLEGRO_WINDOWS
      drive_list = GetLogicalDrives ();
#else
      drive_list = 0xffffffff;
#endif

      for (index = FS_DRIVE_FIRST; index <= FS_DRIVE_LAST; index++)
      {
         if (!(drive_list & (1 << (index - FS_DRIVE_FIRST))))
         {
            /* Drive is not present. */
            continue;
         }

         /* Build logical path from drive letter index. */
         uszprintf (buffer, sizeof (buffer), "%c:\\", index);

         /* Clever... but botchy. */
         fs_get_directory_callback (buffer, FA_DIREC, buffer);
      }

#endif   /* (ALLEGRO_DOS || ALLEGRO_WINDOWS) */
   }

   if (index >= 0)
   {
      FS_LIST_ENTRY *entry;

      entry = &fs_info.dirs[index];

      if (!entry)
         return (NULL);

      return (entry->text);
   }
   else
   {
      RT_ASSERT(list_size);

      *list_size = fs_info.num_dirs;

      return (NULL);
   }
}

static int file_select_dialog_filename (DIALOG *dialog)
{
   /* This function is called when the ENTER key is pressed inside the
      filename textbox. */

   RT_ASSERT(dialog);

   /* Canonicalize the path into proper form. */
   canonicalize_filename (dialog->dp, dialog->dp, (dialog->d1 - 1));

   if (file_exists (dialog->dp, FA_DIREC, NULL))
   {
      /* This is *definitely* a directory name. */
                   
      fs_set_path (dialog->dp);

      return (D_REDRAW);
   }

   if (ustrlen (get_filename (dialog->dp)) > 0)
   {
      /* This is a filename. */

      if (exists (dialog->dp))
      {
         ustrzncpy (fs_info.path, sizeof (fs_info.path), dialog->dp,
            ustrsize (dialog->dp));
   
         return (D_CLOSE);
      }
      else
      {
         gui_alert ("Error", "The specified file could not be found.", NULL,
            NULL, "&OK", NULL, 'o', 0);

         return (D_REDRAW);
      }
   }

   /* This is *probably* a directory name. */
   fs_set_path (dialog->dp);

   return (D_REDRAW);
}

static INLINE int gui_file_select (const UCHAR *title, const UCHAR *caption,
   UCHAR *path, int path_max, const UCHAR *ext)
{
   /* Displays a file selector.

     This function assumes that 'path' is already null-terminated, and that
     'path_max' contains the size of 'path', including the null terminator.
     */

   DIALOG *dialog;
   DIALOG *objcaption;
   USTRING filename_buffer;
   int result;

   RT_ASSERT(title);
   RT_ASSERT(caption);
   RT_ASSERT(path);
   RT_ASSERT(ext);

   /* Canonicalize the path into proper form. */
   canonicalize_filename (path, path, (path_max - 1));

   /* Set up fs_info structure. */

   memset (&fs_info, 0, sizeof (fs_info));

   ustrzncpy (fs_info.path, sizeof (fs_info.path), path, ustrsize (path));

   fs_info.ext = ext;

   fs_info.num_files = -1;
   fs_info.num_dirs  = -1;

   /* Load configuration. */
   fs_info.show_hidden = TRUE_OR_FALSE(get_config_int ("gui", "show_hidden",
      FALSE));

   /* Create dialog. */

   dialog = create_dialog (file_select_dialog_base, title);
   if (!dialog)
      return (0);

   /* Get objects. */

   objcaption = &dialog[FILE_SELECT_DIALOG_CAPTION_LABEL];

   fs_info.objshow  = &dialog[FILE_SELECT_DIALOG_SHOW_HIDDEN_FILES_CHECKBOX];
   fs_info.objfiles = &dialog[FILE_SELECT_DIALOG_FILE_LIST];
   fs_info.objdirs  = &dialog[FILE_SELECT_DIALOG_DIRECTORY_LIST];
   fs_info.objfile  = &dialog[FILE_SELECT_DIALOG_FILENAME];
   fs_info.objok    = &dialog[FILE_SELECT_DIALOG_OK_BUTTON];

   /* Set up objects. */

   objcaption->dp2 = (char *)caption;

   fs_info.objshow->dp2 = file_select_dialog_show_hidden_files_checkbox;
  
   if (fs_info.show_hidden)
      fs_info.objshow->flags |= D_SELECTED;

   fs_info.objfiles->dp  = file_select_dialog_file_list_filler;
   fs_info.objfiles->dp3 = file_select_dialog_file_list;

   fs_info.objdirs->dp  = file_select_dialog_directory_list_filler;
   fs_info.objdirs->dp2 = file_select_dialog_directory_list;

   ustrzncpy (filename_buffer, sizeof (filename_buffer), fs_info.path,
      ustrsize (fs_info.path));

   fs_info.objfile->d1  = ((sizeof (filename_buffer) / MAX_UCHAR_LENGTH) - 1);
   fs_info.objfile->dp  = filename_buffer;
   fs_info.objfile->dp2 = file_select_dialog_filename;

   /* Show dialog. */

   result = show_dialog (dialog, -1);

   /* Save configuration. */
   set_config_int ("gui", "show_hidden", fs_info.show_hidden);

   /* Destroy file and directory lists. */

   if (fs_info.files)
      free (fs_info.files);

   if (fs_info.dirs)
      free (fs_info.dirs);

   /* Destroy dialog. */
   unload_dialog (dialog);

   if ((result == FILE_SELECT_DIALOG_FILE_LIST) ||
       (result == FILE_SELECT_DIALOG_FILENAME) ||
       (result == FILE_SELECT_DIALOG_OK_BUTTON))
   {
      ustrzncpy (path, path_max, fs_info.path, ustrsize (fs_info.path));

      /* Return success. */
      return (1);
   }

   /* Cancelled. */
   return (0);
}
