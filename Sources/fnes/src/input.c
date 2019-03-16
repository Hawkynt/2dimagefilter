/* FakeNES - A free, portable, Open Source NES emulator.

   input.c: Implementation of the input abstraction.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */
               
#include <allegro.h>
#include <stdio.h>
#include <string.h>
#include "audio.h"
#include "common.h"
#include "data.h"
#include "debug.h"
#include "gui.h"
#include "input.h"
#include "netplay.h"
#include "ppu.h"
#include "rom.h"
#include "save.h"
#include "timing.h"
#include "types.h"
#include "video.h"

/* What mode were are currently in.  Technically, this should be called
   'input_modes', since more than mode is possible. :p */
LIST input_mode = 0;

/* Nothing special here.  This string simply contains the current message
   that the player is typing in the chat box. */
USTRING input_chat_text;

/* How often input_process() will attempt an autosave, in frames. */
int input_autosave_interval = 0;

/* Player button states.  This is not a boolean array - values stored in it
   must be either 0 (off) or 1 (on). */
static int player_buttons[INPUT_PLAYERS][INPUT_BUTTONS];

enum { BUTTON_OFF = 0 };
enum { BUTTON_ON  = 1 };

#define BUTTON_ON_OR_OFF(x)   (x ? BUTTON_ON : BUTTON_OFF)

/* Button modifiers such as auto and turbo. */
static LIST button_modifiers[INPUT_PLAYERS][INPUT_BUTTONS];

/* There's a maximum of 4 of these due to how they are saved (see below). */
enum {
   BUTTON_MODIFIER_AUTO     = (1 << 0),
   BUTTON_MODIFIER_TURBO    = (1 << 1),
   BUTTON_MODIFIER_UNUSED_1 = (1 << 2),
   BUTTON_MODIFIER_UNUSED_2 = (1 << 3),
};

/* Defines determining how button modifiers are packed into/loaded from a
   LIST for storage in the configuration file. */
#define MODIFIER_SHIFTS 4
#define MODIFIER_MASK   0xf
                            
/* Whether or not Zapper (light gun) emulation is enabled.  Having this
   enabled when not necessary can make frameskipping less efficient, since
   sometimes the PPU has to render skipped frames for Zapper hitscan. */
BOOL input_enable_zapper = FALSE;

/* Which input device a given player is using. */
static ENUM input_devices[INPUT_PLAYERS];

/* Other emulation-related variables. */
static int zapper_mask = 0;
static UINT8 last_write = 0;
static UINT8 current_read_p1 = 0;
static UINT8 current_read_p2 = 0;

/* Keyboard configuration.  A slightly updated version of the old system
   is used which now handles large (2**31) scancodes without crashing. */
static int key1_scancodes[INPUT_BUTTONS];
static int key2_scancodes[INPUT_BUTTONS];

/* Number of supported joystick devices. */
#define NUM_JOYSTICKS   4

/* Format:
      Axxx
         Stick  (0-F)
         Axis   (0-F)
         Phase  (0-1)
      Bxx
         Button (0-FF)
         */
static char joystick_defaults[] =
   { "B01 B00 B02 B03 A010 A011 A000 A001\0" };

typedef struct _JOYSTICK_DATA
{
   ENUM type;

   /* For axis-based input. */
   int stick;
   int axis;
   int phase;

   /* For button-based input. */
   int index;

} JOYSTICK_DATA;

static JOYSTICK_DATA joystick_data[NUM_JOYSTICKS][INPUT_BUTTONS];

enum
{  
   JOYSTICK_DATA_TYPE_NONE,
   JOYSTICK_DATA_TYPE_AXIS,
   JOYSTICK_DATA_TYPE_BUTTON
};

/* Joystick sensetivity threshold. */
#define JOYSTICK_SENSETIVITY  72

/* TODO: Move into configurable variables. */
/* TODO: These should somehow use the dimensions of 'screen_buffer' instead,
   but that BITMAP* is not currently made public. */
#define MOUSE_SENSETIVITY_X   (SCREEN_W / 128)
#define MOUSE_SENSETIVITY_Y   (SCREEN_W / 128)

/* Number of frames to countdown before input will be processed again (this
   only applies to INPUT_MODE_PLAY). */
static int wait_frames = 0;

/* General options. */
static BOOL allow_conflicts = FALSE;
static BOOL toggled_auto    = FALSE;
static BOOL merge_players   = FALSE;
static REAL turbo_rate      = 0.5;

/* Cached values of player button states used for toggled autofire. */
static BOOL auto_cache[INPUT_PLAYERS][INPUT_BUTTONS];

/* Turbo stuff.  We keep it here so we can reset it. */
static int turbo_phase  = 0;
static int turbo_frames = 0;

static INLINE void load_keyboard_config (void)
{
   STRING defaults;
   STRING buffer;

   /* Build default key config in a portable manner. */
   sprintf (defaults, "%d %d %d %d %d %d %d %d", KEY_ALT, KEY_LCONTROL,
      KEY_TAB, KEY_ENTER, KEY_UP, KEY_DOWN, KEY_LEFT, KEY_RIGHT);

   /* Unicode->ASCII here, is it a problem? */
   sprintf (buffer, "%s", get_config_string ("input", "key1_buttons",
      defaults));

   if (sscanf (buffer, "%d%d%d%d%d%d%d%d",
         &key1_scancodes[0], &key1_scancodes[1], &key1_scancodes[2],
         &key1_scancodes[3], &key1_scancodes[4], &key1_scancodes[5],
         &key1_scancodes[6], &key1_scancodes[7]) < INPUT_BUTTONS)
   {
      sscanf (defaults, "%d%d%d%d%d%d%d%d",
         &key1_scancodes[0], &key1_scancodes[1], &key1_scancodes[2],
         &key1_scancodes[3], &key1_scancodes[4], &key1_scancodes[5],
         &key1_scancodes[6], &key1_scancodes[7]);
   }

   sprintf (buffer, "%s", get_config_string ("input", "key2_buttons",
      defaults));

   if (sscanf (buffer, "%d%d%d%d%d%d%d%d",
         &key2_scancodes[0], &key2_scancodes[1], &key2_scancodes[2],
         &key2_scancodes[3], &key2_scancodes[4], &key2_scancodes[5],
         &key2_scancodes[6], &key2_scancodes[7]) < INPUT_BUTTONS)
   {
      sscanf (defaults, "%d%d%d%d%d%d%d%d",
         &key2_scancodes[0], &key2_scancodes[1], &key2_scancodes[2],
         &key2_scancodes[3], &key2_scancodes[4], &key2_scancodes[5],
         &key2_scancodes[6], &key2_scancodes[7]);
   }
}

static INLINE void save_keyboard_config (void)
{
   STRING buffer;

   sprintf (buffer, "%d %d %d %d %d %d %d %d",
      key1_scancodes[0], key1_scancodes[1], key1_scancodes[2],
      key1_scancodes[3], key1_scancodes[4], key1_scancodes[5],
      key1_scancodes[6], key1_scancodes[7]);

   set_config_string ("input", "key1_buttons", buffer);

   sprintf (buffer, "%d %d %d %d %d %d %d %d",
      key2_scancodes[0], key2_scancodes[1], key2_scancodes[2],
      key2_scancodes[3], key2_scancodes[4], key2_scancodes[5],
      key2_scancodes[6], key2_scancodes[7]);

   set_config_string ("input", "key2_buttons", buffer);
}

static INLINE void load_joystick_config (void)
{
   int index;

   for (index = 0; index < NUM_JOYSTICKS; index++)
   {
      USTRING key;
      STRING buffer;
      int buttons[INPUT_BUTTONS];
      int subindex;

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "joy%d_buttons", (index + 1));

      /* TODO: Possible conflict here going from Unicode to ASCII? */
      sprintf (buffer, "%s", get_config_string ("input", key,
         joystick_defaults));

      if (sscanf (buffer, "%x%x%x%x%x%x%x%x",
            &buttons[0], &buttons[1], &buttons[2], &buttons[3], &buttons[4],
            &buttons[5], &buttons[6], &buttons[7]) < INPUT_BUTTONS)
      {
         /* Configuration is corrupt - restore defaults. */

         sscanf (joystick_defaults, "%x%x%x%x%x%x%x%x",
            &buttons[0], &buttons[1], &buttons[2], &buttons[3], &buttons[4],
            &buttons[5], &buttons[6], &buttons[7]);
      }

      for (subindex = 0; subindex < INPUT_BUTTONS; subindex++)
      {
         JOYSTICK_DATA *data = &joystick_data[index][subindex];
         int value = buttons[subindex];

         /* Fill info structure. */

         if (value >= 0xa000)
            data->type = JOYSTICK_DATA_TYPE_AXIS;
         else
            data->type = JOYSTICK_DATA_TYPE_BUTTON;

         switch (data->type)
         {
            case JOYSTICK_DATA_TYPE_AXIS:
            {
               data->stick = ((value >> 8) & 0xf);
               data->axis = ((value >> 4) & 0xf);
               data->phase = (value & 1);

               break;
            }

            case JOYSTICK_DATA_TYPE_BUTTON:
            {
               data->index = (value & 0xff);

               break;
            }

            default:
               WARN_GENERIC();
         }
      }
   }
}

static INLINE void save_joystick_config (void)
{
   int index;

   for (index = 0; index < NUM_JOYSTICKS; index++)
   {
      STRING buffer;
      int subindex;
      int buttons[INPUT_BUTTONS];
      USTRING key;

      for (subindex = 0; subindex < INPUT_BUTTONS; subindex++)
      {
         const JOYSTICK_DATA *data = &joystick_data[index][subindex];
         int value = 0; /* Kill warning. */

         switch (data->type)
         {
            case JOYSTICK_DATA_TYPE_AXIS:
            {
               value = 0xa000;
               value |= ((data->stick & 0xf) << 8);
               value |= ((data->axis & 0xf) << 4);
               value |= (data->phase & 1);

               break;
            }

            case JOYSTICK_DATA_TYPE_BUTTON:
            {
               value = 0xb00;
               value |= (data->index & 0xff);

               break;
            }

            default:
               WARN_GENERIC();
         }

         buttons[subindex] = value;
      }

      sprintf (buffer, "%x %x %x %x %x %x %x %x",
         buttons[0], buttons[1], buttons[2], buttons[3], buttons[4],
         buttons[5], buttons[6], buttons[7]);

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "joy%d_buttons", (index + 1));

      set_config_string ("input", key, buffer);
   }
}

static INLINE void load_modifier_config (void)
{
   int player;

   for (player = 0; player < INPUT_PLAYERS; player++)
   {
      USTRING key;
      LIST list;
      int button;

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "player_%d_modifiers", (player + 1));

      list = get_config_int ("input", key, 0);

      for (button = 0; button < INPUT_BUTTONS; button++)
      {
         button_modifiers[player][button] = (list & MODIFIER_MASK);
         list >>= MODIFIER_SHIFTS;
      }
   }
}

static INLINE void save_modifier_config (void)
{
   int player;

   for (player = 0; player < INPUT_PLAYERS; player++)
   {
      LIST list = 0;
      int button;
      USTRING key;

      /* We have to go backwards when saving. ;p */

      for (button = (INPUT_BUTTONS - 1); button >= 0; button--)
      {
         list <<= MODIFIER_SHIFTS;
         list |= button_modifiers[player][button];
      }

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "player_%d_modifiers", (player + 1));

      set_config_int ("input", key, list);
   }
}

void input_load_config (void)
{
   int player;
   
   for (player = 0; player < INPUT_PLAYERS; player++)
   {
      USTRING key;
      ENUM defaults;

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "player_%d_device", (player + 1));

      if (player == 0)
         defaults = INPUT_DEVICE_KEYS_1;
      else
         defaults = INPUT_DEVICE_NONE;

      input_devices[player] = get_config_int ("input", key, defaults);
   }

   input_enable_zapper = get_config_int ("input", "enable_zapper", input_enable_zapper);

   /* This should probably be moved to another section. */
   input_autosave_interval = get_config_int ("timing", "autosave_interval", input_autosave_interval);

   load_keyboard_config ();
   load_joystick_config ();

   load_modifier_config ();

   allow_conflicts = get_config_int   ("input", "allow_conflicts", allow_conflicts);
   toggled_auto    = get_config_int   ("input", "toggled_auto",    toggled_auto);
   merge_players   = get_config_int   ("input", "merge_players",   merge_players);
   turbo_rate      = get_config_float ("input", "turbo_rate",      turbo_rate);
   
   if (turbo_rate < EPSILON)
      turbo_rate = EPSILON;
}

void input_save_config (void)
{
   int player;

   for (player = 0; player < INPUT_PLAYERS; player++)
   {
      USTRING key;

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "player_%d_device", (player + 1));

      set_config_int ("input", key, input_devices[player]);
   }

   set_config_int ("input", "enable_zapper", input_enable_zapper);

   set_config_int ("timing", "autosave_interval", input_autosave_interval);

   save_keyboard_config ();
   save_joystick_config ();

   save_modifier_config ();

   set_config_int   ("input", "allow_conflicts", allow_conflicts);
   set_config_int   ("input", "toggled_auto",    toggled_auto);
   set_config_int   ("input", "merge_players",   merge_players);
   set_config_float ("input", "turbo_rate",      turbo_rate);
}

int input_init (void)
{
   install_keyboard ();
   install_mouse ();

   if (load_joystick_data (NULL) != 0)
   {
      /* load_joystick_data() failed; reinitialize joystick system. */
      install_joystick (JOY_TYPE_AUTODETECT);
   }

   /* Load configuration. */
   input_load_config ();

   /* Clear chat text buffer. */
   USTRING_CLEAR(input_chat_text);

   /* Enter gameplay mode. */
   input_mode = INPUT_MODE_PLAY;

   /* Reset everything. */
   input_reset ();

   /* Return success. */
   return (0);
}

void input_exit (void)
{
   /* Remove drivers. */

   remove_keyboard ();
   remove_mouse ();
   remove_joystick ();

   /* Save configuration. */
   input_save_config ();
}

void input_reset (void)
{
   int player;

   /* Reset button states. */

   for (player = 0; player < INPUT_PLAYERS; player++)
   {
      int *buttons = &player_buttons[player][0];
      int index;

      for (index = 0; index < INPUT_BUTTONS; index++)
      {
         buttons[index] = BUTTON_ON;

         /* Sync to autofire cache. */
         auto_cache[player][index] = buttons[index];
      }
   }

   /* Clear variables. */
   wait_frames      = 0;
   last_write       = 0;
   current_read_p1  = 0;
   current_read_p2  = 0;
   turbo_phase      = 0;
   turbo_frames     = 0;
}

UINT8 input_read (UINT16 address)               
{
   int index;

   if (!input_enable_zapper)
   {
      /* Zapper disabled; clear mask. */
      zapper_mask = 0;
   }

   switch (address)
   {
      case 0x4016:
      {
         /* 1st and 3rd players. */

         if (current_read_p1 == 19)
         {
            /* Signature. */

            /* Increment read counter. */
            current_read_p1++;

            /* Return ?? */
            return (0x01);
         }
         else if ((current_read_p1 > 7) &&
                  (current_read_p1 < 16))
         {
            /* Player 3 button status. */

             /* Get button index. */
             index = (current_read_p1 - 8);

             /* Increment read counter. */
             current_read_p1++;

             /* Return button status. */
             return ((player_buttons[INPUT_PLAYER_3][index] | 0x40));
         }
         else if ((current_read_p1 > 15) &&
                  (current_read_p1 < 23))
         {
            /* Ignored. */

            /* Increment read counter. */
            current_read_p1++;

            /* Return nothing. */
            return (0);
         }
         else if (current_read_p1 == 23)
         {
            /* Strobe flip-flop. */

            /* Clear read counter. */
            current_read_p1 = 0;

            /* Return nothing. */
            return (0);
         }
         else
         {
            /* Player 1 button status. */

            return ((player_buttons[INPUT_PLAYER_1][current_read_p1++] |
               0x40));
         }

         break;
      }

      case 0x4017:
      {
         /* 2nd and 4th players. */

         if (current_read_p2 == 18)
         {
            /* Signature. */

            /* Increment read counter. */
            current_read_p2++;
          
            /* Return ?? */
            return ((0x01 | zapper_mask));
         }
         else if ((current_read_p2 > 7) &&
                  (current_read_p2 < 16))
         {
            /* Player 4 button status. */

            /* Get button index. */
            index = (current_read_p2 - 8);

            /* Increment read counter. */
            current_read_p2++;

            /* Return button status. */
            return (((player_buttons[INPUT_PLAYER_4][index] | 0x40) |
               zapper_mask));
         }
         else if ((current_read_p2 > 15) &&
                  (current_read_p2 < 23))
         {
            /* Ignored. */

            /* Increment read counter. */
            current_read_p2++;

            /* Return nothing. */
            return (zapper_mask);
         }
         else if (current_read_p2 == 23)
         {
            /* Strobe flip-flop. */

            /* Clear read counter. */
            current_read_p2 = 0;

            /* Return nothing. */
            return (zapper_mask);
         }
         else
         {
            /* Player 2 button status. */

            return (((player_buttons[INPUT_PLAYER_2][current_read_p2++] |
               0x40) | zapper_mask));
         }

         break;
      }

      default:
      {
         /* Untrapped port. */
         return (0);
      }

      break;
   }
}

void input_write (UINT16 address, UINT8 value)
{
   switch (address)
   {
      case 0x4016:
      {
         /* 1st and 3rd players. */

         if ((!(value & 0x01)) && (last_write & 0x01))
         {
            /* Full strobe. */

            /* Clear read counters. */
            current_read_p1 = 0;
            current_read_p2 = 0;
         }

         last_write = value;

         break;
      }

      case 0x4017:
      {
         /* 2nd and 4th players. */

         /* Do nothing. */
         break;
      }
      
      default:
      {
         /* Untrapped port. */
         break;
      }
   }
}

void input_update_zapper_offsets (void)
{
   BOOL adjust_mouse = FALSE;

   input_zapper_x_offset = (mouse_x - mouse_x_focus);
   input_zapper_y_offset = (mouse_y - mouse_y_focus);

   /* Trigger is left mouse button. */
   input_zapper_trigger = TRUE_OR_FALSE(mouse_b & 1);

   /* Perform bounds checking */
   if (input_zapper_x_offset >= 256)
   {
      input_zapper_x_offset = 255;
      adjust_mouse = TRUE;
   }

   if (input_zapper_y_offset >= 240)
   {
      input_zapper_y_offset = 239;
      adjust_mouse = TRUE;
   }

   if (adjust_mouse)
      position_mouse (input_zapper_x_offset, input_zapper_y_offset);

   if ((input_zapper_x_offset < 256) && (input_zapper_y_offset < 240))
      input_zapper_on_screen = TRUE;

   if (!input_zapper_on_screen)
      input_update_zapper ();
}

void input_update_zapper (void)
{
   int pixel;

   zapper_mask = 0x08;

   if (input_zapper_trigger)
   {
      /* Left button. */
      zapper_mask |= 0x10;
   }

   if (input_zapper_on_screen)
   {
      /* Note the (- 1) here is because NES colors in the video buffer begin
         at 1 instead of 0.  This will need to be rewritten if the PPU is
         updated with scalar rendering, regardless. */
      pixel = (_getpixel (video_buffer, input_zapper_x_offset,
         input_zapper_y_offset) - 1);

      /* Check for white. */
      if ((pixel == 32) || (pixel == 48))
         zapper_mask &= ~0x08;
   }
}

static INLINE BOOL joystick_reader (int which, int button)
{
   JOYSTICK_DATA *data = &joystick_data[which][button];

   switch (data->type)
   {
      case JOYSTICK_DATA_TYPE_AXIS:
      {
         int meta, pos;

         if (joy[which].stick[data->stick].flags & JOYFLAG_UNSIGNED)
         {
            /* This is probably a throttle control - require
               double the threshold. */
            meta = (JOYSTICK_SENSETIVITY << 1);
         }
         else
            meta = JOYSTICK_SENSETIVITY;

         pos = joy[which].stick[data->stick].axis[data->axis].pos;

         if (data->phase > 0)
            return (pos >= +meta);
         else
            return (pos <= -meta);

         break;
      }

      case JOYSTICK_DATA_TYPE_BUTTON:
      {
         return (joy[which].button[data->index].b);

         break;
      }

      break;
   }

   return (FALSE);
}

static INLINE void do_keyboard_1 (int player)
{
   int *buttons = &player_buttons[player][0];
   int index;

   /* Some keyboards may require polling. */
   if (keyboard_needs_poll ())
      poll_keyboard ();

   for (index = 0; index < INPUT_BUTTONS; index++)
      buttons[index] = BUTTON_ON_OR_OFF(key[key1_scancodes[index]]);
}

static INLINE void do_keyboard_2 (int player)
{
   int *buttons = &player_buttons[player][0];
   int index;

   /* Some keyboards may require polling. */
   if (keyboard_needs_poll ())
      poll_keyboard ();

   for (index = 0; index < INPUT_BUTTONS; index++)
      buttons[index] = BUTTON_ON_OR_OFF(key[key2_scancodes[index]]);
}

static INLINE void do_joystick (int player, int which)
{
   int *buttons = &player_buttons[player][0];
   int index;

   /* Some joysticks may require polling. */
   poll_joystick ();

   for (index = 0; index < INPUT_BUTTONS; index++)
      buttons[index] = BUTTON_ON_OR_OFF(joystick_reader (which, index));
}

static INLINE void do_mouse (int player)
{
   int *buttons = &player_buttons[player][0];
   static int last_mouse_z = 0;
   BOOL scroll = FALSE;
   int mickey_x, mickey_y;
   BOOL left, right, up, down;

   /* Some mice may require polling. */
   if (mouse_needs_poll ())
      poll_mouse ();

   /* Mouse wheel is used for Select. */
   if (mouse_z != last_mouse_z)
   {
      last_mouse_z = mouse_z;
      scroll = TRUE;
   }

   /* We use mickeys for an infinite range of movement. */
   get_mouse_mickeys (&mickey_x, &mickey_y);

   /* Directional control. */
   left  = (mickey_x < -MOUSE_SENSETIVITY_X);
   right = (mickey_x > +MOUSE_SENSETIVITY_X);
   up    = (mickey_y < -MOUSE_SENSETIVITY_Y);
   down  = (mickey_y > +MOUSE_SENSETIVITY_Y);

   buttons[INPUT_BUTTON_A]      = BUTTON_ON_OR_OFF(mouse_b & 2);
   buttons[INPUT_BUTTON_B]      = BUTTON_ON_OR_OFF(mouse_b & 1);
   buttons[INPUT_BUTTON_SELECT] = BUTTON_ON_OR_OFF(scroll);
   buttons[INPUT_BUTTON_START]  = BUTTON_ON_OR_OFF(mouse_b & 4);
   buttons[INPUT_BUTTON_UP]     = BUTTON_ON_OR_OFF(up);
   buttons[INPUT_BUTTON_DOWN]   = BUTTON_ON_OR_OFF(down);
   buttons[INPUT_BUTTON_LEFT]   = BUTTON_ON_OR_OFF(left);
   buttons[INPUT_BUTTON_RIGHT]  = BUTTON_ON_OR_OFF(right);
}

void input_process (void)
{
   int player;

   if (!(input_mode & INPUT_MODE_REPLAY_RECORD))
   {
      /* Timed autosave code.  Only executes when a replay isn't currently
         recording, but is allowed to execute while a replay is playing. */

      if (input_autosave_interval > 0)
      {
         static int frames = 0;

         if (++frames == ROUND(input_autosave_interval * timing_get_speed ()))
         {
            /* Simulate keypress. */
            gui_handle_keypress (0, KEY_F3);

            /* Reset frame counter. */
            frames = 0;
         }
      }
   }

   if (input_mode & INPUT_MODE_REPLAY_PLAY)
   {
       /* Replay playback code.  Reads data from the replay file and feeds
          it directly into the player button states. */

      for (player = 0; player < INPUT_PLAYERS; player++)
      {
         int *buttons = &player_buttons[player][0];
         UINT8 data;
         BOOL eof;
         int index;

         eof = get_replay_data (&data);

         for (index = 0; index < INPUT_BUTTONS; index++)
             buttons[index] = BUTTON_ON_OR_OFF(data & (1 << index));

         if (eof)
         {
            /* End of replay reached. */
            gui_stop_replay ();
            return;
         }
      }
   }

   if (!(input_mode & INPUT_MODE_PLAY))
   {
      /* The remaining code should only execute in the normal gameplay
         mode - bail out. */
      return;
   }

   if (wait_frames > 0)
      wait_frames--;
   if (wait_frames > 0)
      return;

   /* Phase 1 - Gathering input and applying modifiers. */

   for (player = 0; player < INPUT_PLAYERS; player++)
   {
      int *buttons = &player_buttons[player][0];
      int button;

      switch (input_devices[player])
      {
         case INPUT_DEVICE_NONE:
         {
            for (button = 0; button < INPUT_BUTTONS; button++)
               buttons[button] = BUTTON_OFF;

            break;
         }

         case INPUT_DEVICE_KEYS_1:
         {
            do_keyboard_1 (player);

            break;
         }
 
         case INPUT_DEVICE_KEYS_2:
         {
            do_keyboard_2 (player);
 
            break;
         }
 
         case INPUT_DEVICE_JOYSTICK_1:
         case INPUT_DEVICE_JOYSTICK_2:
         case INPUT_DEVICE_JOYSTICK_3:
         case INPUT_DEVICE_JOYSTICK_4:
         {
            do_joystick (player, (input_devices[player] -
               INPUT_DEVICE_JOYSTICK_1));
 
            break;
         }
 
         case INPUT_DEVICE_MOUSE:
         {
            do_mouse (player);
 
            break;
         }
      }

      /* Apply modifiers. */

      for (button = 0; button < INPUT_BUTTONS; button++)
      {
         const LIST *list = &button_modifiers[player][button];

         if (LIST_COMPARE(*list, BUTTON_MODIFIER_AUTO))
         {
            if (toggled_auto)
            {
               if (buttons[button] == BUTTON_ON)
               {
                  /* Invert auto status and cache it for next time. */
                  auto_cache[player][button] =
                     BUTTON_ON_OR_OFF(!auto_cache[player][button]);
               }

               buttons[button] = auto_cache[player][button];
            }
            else
            {
               /* Always on. */
               buttons[button] = BUTTON_ON;
            }
         }

         if (LIST_COMPARE(*list, BUTTON_MODIFIER_TURBO))
            buttons[button] = MIN(buttons[button], turbo_phase);
      }
   }

   /* Phase 2 - Merging players together. */

   if (merge_players)
   {
      for (player = 0; player < INPUT_PLAYERS; player++)
      {
         int *buttons = &player_buttons[player][0];

         switch (player)
         {
            case INPUT_PLAYER_1:
            {
               int button;

               /* Merge player 3 with player 1. */

               for (button = 0; button < INPUT_BUTTONS; button++)
               {
                  buttons[button] = MAX(buttons[button],
                     player_buttons[INPUT_PLAYER_3][button]);

                  player_buttons[INPUT_PLAYER_3][button] = buttons[button];
               }

               break;
            }

            case INPUT_PLAYER_2:
            {
               int button;

               /* Merge player 4 with player 2. */

               for (button = 0; button < INPUT_BUTTONS; button++)
               {
                  buttons[button] = MAX(buttons[button],
                     player_buttons[INPUT_PLAYER_4][button]);

                  player_buttons[INPUT_PLAYER_4][button] = buttons[button];
               }

               break;
            }

            default:
               break;
         }
      }
   }

   /* Phase 3 - Preventing conflicts. */

   if (!allow_conflicts)
   {
      for (player = 0; player < INPUT_PLAYERS; player++)
      {
         int *buttons = &player_buttons[player][0];

         /* Fix up conflicting directional controls. */

         if (buttons[INPUT_BUTTON_UP] && buttons[INPUT_BUTTON_DOWN])
         {
            /* Prevent up and down from being pressed at the same time */
   
            buttons[INPUT_BUTTON_UP]   = BUTTON_OFF;
            buttons[INPUT_BUTTON_DOWN] = BUTTON_OFF;
         }
    
         if (buttons[INPUT_BUTTON_LEFT] && buttons[INPUT_BUTTON_RIGHT])
         {
            /* Prevent left and right from being pressed at the same time */
   
            buttons[INPUT_BUTTON_LEFT]  = BUTTON_OFF;
            buttons[INPUT_BUTTON_RIGHT] = BUTTON_OFF;
         }
      }
   }

   /* Part IV - Saving replay data. */

   if (input_mode & INPUT_MODE_REPLAY_RECORD)
   {
      /* Send player button states to the replay file. */

      for (player = 0; player < INPUT_PLAYERS; player++)
      {
         int *buttons = &player_buttons[player][0];
         UINT8 data = 0;
         int button;
      
         for (button = 0; button < INPUT_BUTTONS; button++)
         {
            if (buttons[button] == BUTTON_ON)
               data |= (1 << button);
         }
 
         save_replay_data (data);
      }
   }

   if (--turbo_frames <= 0)
   {
      REAL speed;

      /* Invert phase. */

      turbo_phase = BUTTON_ON_OR_OFF(!turbo_phase);

      /* Set frame counter. */
   
      speed = timing_get_speed ();
   
      turbo_frames = ROUND((speed / (speed * turbo_rate)));
   }
}

void input_handle_keypress (int c, int scancode)
{
   switch (scancode)
   {
      case KEY_BACKSPACE:
      {
         if (!(input_mode & INPUT_MODE_CHAT))
         {
            /* Switch to chat mode. */
            input_mode &= ~INPUT_MODE_PLAY;
            input_mode |= INPUT_MODE_CHAT;

            /* Play sound. */
            play_sample (DATA_TO_SAMPLE(CHAT_WINDOW_SOUND), 255, 128, 1000, FALSE);

            return;
         }

         break;
      }

      default:
         break;
   }

   /* We need to be in chat mode to proceed further. */
   if (!(input_mode & INPUT_MODE_CHAT))
      return;

   /* TODO: Make these Unicode calls protect against buffer overflow. */

   switch (scancode)
   {
      case KEY_BACKSPACE:
      {
         if (ustrlen (input_chat_text) > 0)
         {
            /* Remove the last character from the buffer. */
            uremove (input_chat_text, (ustrlen (input_chat_text) - 1));
         }

         break;
      }

      case KEY_ENTER:
      {
         if (ustrlen (input_chat_text) > 0)
         {
            if (netplay_mode)
            {
               /* Send message over network. */
               netplay_send_message (input_chat_text);
            }
            else
            {
               /* Just show message locally. */
               video_message (input_chat_text);
               video_message_duration = 5000;

               /* Play sound. */
               play_sample (DATA_TO_SAMPLE(CHAT_RECIEVE_SOUND), 255, 128, 1000, FALSE);
            }

            /* Clear buffer. */
            USTRING_CLEAR(input_chat_text);
         }

         input_mode &= ~INPUT_MODE_CHAT;

         if (!(input_mode & INPUT_MODE_REPLAY_PLAY))
            input_mode |= INPUT_MODE_PLAY;

         wait_frames = ROUND(timing_get_speed () / 2.0f);

         return;                 
      }

      default:
      {
         /* Not sure if this is correct, but it's better than a crash! */

         if (ustrsizez (input_chat_text) < (((int)sizeof (input_chat_text) -
            uwidth_max (U_CURRENT)) - 1))
         {
            /* Add character to the end of the buffer. */
            uinsert (input_chat_text, ustrlen (input_chat_text), c);

            /* Play sound. */
            play_sample (DATA_TO_SAMPLE(CHAT_TYPE_SOUND), 255, 128, 1000, FALSE);
         }

         break;
      }
   }
}

ENUM input_get_player_device (ENUM player)
{
   return (input_devices[player]);
}

void input_set_player_device (ENUM player, ENUM device)
{
   input_devices[player] = device;
}

void input_map_player_button (ENUM player, ENUM button)
{
   ENUM device;
   int passes = 0;

   /* This function scans 'player's input device for changes on all
      supported inputs, then updates their configuration for 'button'.

      It should *only* be called by the GUI, since it uses gui_heartbeat(),
      and makes no attempts to provide it's own user interface. */

   device = input_devices[player];

   switch (device)
   {
      case INPUT_DEVICE_NONE:
         return;

      case INPUT_DEVICE_JOYSTICK_1:
      case INPUT_DEVICE_JOYSTICK_2:
      case INPUT_DEVICE_JOYSTICK_3:
      case INPUT_DEVICE_JOYSTICK_4:
      {
         int index;

         index = (device - INPUT_DEVICE_JOYSTICK_1);

         if ((joy[index].flags & JOYFLAG_CALIB_DIGITAL) ||
             (joy[index].flags & JOYFLAG_CALIB_ANALOGUE) ||
             (joy[index].flags & JOYFLAG_CALIBRATE))
         {
            gui_alert ("Error", "This device must be calibrated first.",
               NULL, NULL, "&OK", NULL, 'o', 0);

            gui_message (-1, "Button mapping cancelled");

            return;
         }

         break;
      }

      default:
        break;
   }

   /* Clear keyboard buffer just in case. */
   clear_keybuf ();

   while (TRUE)
   {
      /* Some keyboards may require polling. */
      if (keyboard_needs_poll ())
         poll_keyboard ();

      if (key[KEY_ESC])
      {
         /* Cancelled. */

         /* Clear keyboard buffer to keep input configuration dialog from
            being closed by the ESC signal. */
         clear_keybuf ();

         gui_message (-1, "Button mapping cancelled.");

         return;
      }

      switch (device)
      {
         case INPUT_DEVICE_KEYS_1:
         case INPUT_DEVICE_KEYS_2:
         {
            int scancode = -1;
            int index;

            /* Check for modifier keys first, since they do not count
               as keypresses. */
            for (index = KEY_MODIFIERS; index < KEY_MAX; index++)
            {
               if (key[index])
                  scancode = index;
            }

            if (keypressed ())
               ureadkey (&scancode);

            if (scancode == -1)
               break;

            switch (device)
            {
               case INPUT_DEVICE_KEYS_1:
               {
                  key1_scancodes[button] = scancode;

                  break;
               }

               case INPUT_DEVICE_KEYS_2:
               {
                  key2_scancodes[button] = scancode;

                  break;
               }

               default:
                  WARN_GENERIC();
            }

            gui_message (-1, "Button mapped to keyboard scancode %xh.",
               scancode);

            return;
         }

         case INPUT_DEVICE_JOYSTICK_1:
         case INPUT_DEVICE_JOYSTICK_2:
         case INPUT_DEVICE_JOYSTICK_3:
         case INPUT_DEVICE_JOYSTICK_4:
         {
            int index, stick, but;
            ENUM type = JOYSTICK_DATA_TYPE_NONE;
            int d1 = 0, d2 = 0, d3 = 0;   /* Kill warnings. */
            JOYSTICK_DATA *data;

            poll_joystick ();

            index = (device - INPUT_DEVICE_JOYSTICK_1);

            /* Scan axis. */

            for (stick = 0; stick < joy[index].num_sticks; stick++)
            {
               int meta;
               int axis;

               if (joy[index].stick[stick].flags & JOYFLAG_UNSIGNED)
               {
                  /* This is probably a throttle control - require
                     double the threshold. */
                  meta = (JOYSTICK_SENSETIVITY << 1);
               }
               else
                  meta = JOYSTICK_SENSETIVITY;

               for (axis = 0; axis < joy[index].stick[stick].num_axis;
                  axis++)
               {
                  int pos;

                  pos = joy[index].stick[stick].axis[axis].pos;

                  if ((pos >= +meta) && (passes == 0))
                  {
                     gui_alert ("Uh-oh", "You seem to have an active "
                        "throttle or other constant-fire mechanism on your "
                        "controller.", "You might want to disable it for "
                        "automapping to work properly.", NULL, "&OK", NULL,
                           'o', 0);
                  }

                  if (pos <= -meta)
                  {
                     type = JOYSTICK_DATA_TYPE_AXIS;
                     d1 = stick;
                     d2 = axis;
                     d3 = 0;
                  }
                  else if (pos >= +meta)
                  {
                     type = JOYSTICK_DATA_TYPE_AXIS;
                     d1 = stick;
                     d2 = axis;
                     d3 = 1;
                  }
               }
            }

            /* Scan buttons. */

            for (but = 0; but < joy[index].num_buttons; but++)
            {
               if (joy[index].button[but].b)
               {
                  type = JOYSTICK_DATA_TYPE_BUTTON;
                  d1 = but;
               }
            }

            if (type == JOYSTICK_DATA_TYPE_NONE)
            {
               /* Couldn't find any valid mappings. */
               break;
            }

            data = &joystick_data[index][button];

            switch (type)
            {
               case JOYSTICK_DATA_TYPE_AXIS:
               {
                  data->type  = type;
                  data->stick = d1;
                  data->axis  = d2;
                  data->phase = d3;

                  gui_message (-1, "Button mapped to joystick %d, stick "
                     "%d, axis %d, phase %d.", index, d1, d2, d3);

                  return;
               }

               case JOYSTICK_DATA_TYPE_BUTTON:
               {
                  data->type  = type;
                  data->index = d1;

                  gui_message (-1, "Button mapped to joystick %d, button "
                     "%d.", index, d1);

                  return;
               }

               default:
                  WARN_GENERIC();
            }

            break;
         }

         case INPUT_DEVICE_MOUSE:
            break;

         default:
            WARN_GENERIC();
      }

      gui_heartbeat ();

      passes++;
   }
}

int input_get_player_button_param (ENUM player, ENUM button, ENUM param)
{
   LIST *list;

   list = &button_modifiers[player][button];

   switch (param)
   {
      case INPUT_PLAYER_BUTTON_PARAM_AUTO:
         return (LIST_COMPARE(*list, BUTTON_MODIFIER_AUTO));

      case INPUT_PLAYER_BUTTON_PARAM_TURBO:
         return (LIST_COMPARE(*list, BUTTON_MODIFIER_TURBO));

      default:
         WARN_GENERIC();
   }

   return (0);
}

void input_set_player_button_param (ENUM player, ENUM button, ENUM param,
   int value)
{
   LIST *list;

   list = &button_modifiers[player][button];

   switch (param)
   {
      case INPUT_PLAYER_BUTTON_PARAM_AUTO:
      {
         if (value)
            LIST_ADD(*list, BUTTON_MODIFIER_AUTO);
         else
            LIST_REMOVE(*list, BUTTON_MODIFIER_AUTO);

         break;
      }

      case INPUT_PLAYER_BUTTON_PARAM_TURBO:
      {
         if (value)
            LIST_ADD(*list, BUTTON_MODIFIER_TURBO);
         else
            LIST_REMOVE(*list, BUTTON_MODIFIER_TURBO);

         break;
      }

      default:
         WARN_GENERIC();
   }
}

void input_save_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   pack_putc (last_write, file);

   pack_putc (current_read_p1, file);
   pack_putc (current_read_p2, file);
}

void input_load_state (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   last_write = pack_getc (file);

   current_read_p1 = pack_getc (file);
   current_read_p2 = pack_getc (file);
}
