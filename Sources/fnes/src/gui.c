/* FakeNES - A free, portable, Open Source NES emulator.

   gui.c: Implementation of the object-based GUI.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#ifdef USE_ALLEGROGL
#include <alleggl.h>
#endif
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "apu.h"
#include "audio.h"
#include "cheats.h"
#include "common.h"
#include "cpu.h"
#include "data.h"
#include "debug.h"
#include "dsp.h"
#include "gui.h"
#include "input.h"
#include "log.h"
#include "mmc.h"
#include "netplay.h"
#include "ppu.h"
#include "rom.h"
#include "save.h"
#include "timing.h"
#include "types.h"
#include "version.h"
#include "video.h"

static BITMAP *gui_buffer = NULL;

static int dialog_x = 0;
static int dialog_y = 0;
static BOOL restart_dialog = FALSE;
 
GUI_THEME gui_theme;
ENUM gui_theme_id = -1;
const GUI_THEME *last_theme = NULL;

RGB *gui_image_palette = NULL;
static BITMAP *gui_mouse_sprite = NULL;
static BITMAP *background_image = NULL;

BOOL gui_is_active = FALSE;
static BOOL gui_needs_restart = FALSE;
static BOOL want_exit = FALSE;

static USTRING message_buffer;

static PALETTE custom_palette;

static int save_state_index = 0; /* For save states. */
static int replay_index = 0;     /* For replays. */

/* Text that appears in "unused" menu slots for recent items. */
#define UNUSED_SLOT_TEXT   "Empty"

/* Number of slots available in each of the associated menus. */
#define OPEN_RECENT_SLOTS  10
#define REPLAY_SLOTS       10
#define SAVE_STATE_SLOTS   10

static USTRING open_recent_filenames[OPEN_RECENT_SLOTS];
static USTRING open_recent_menu_texts[OPEN_RECENT_SLOTS];
static USTRING replay_titles[REPLAY_SLOTS];
static USTRING replay_menu_texts[REPLAY_SLOTS];
static USTRING save_state_titles[SAVE_STATE_SLOTS];
static USTRING save_state_menu_texts[SAVE_STATE_SLOTS];

static BOOL lock_recent = FALSE;

/* Keep these in order! */
#include "gui/themes.h"
#include "gui/objects.h"
#include "gui/menus.h"
#include "gui/dialogs.h"
#include "gui/util.h"
#include "gui/file.h"

static INLINE void update_menus (void)
{
   static USTRING audio_menu_volume_text;

#ifndef USE_OPENAL
   DISABLE_MENU_ITEM(audio_output_menu_subsystem_openal);
#endif

   if (!rom_is_loaded)                
   {
      DISABLE_MENU_ITEM(main_menu_resume);
      DISABLE_MENU_ITEM(main_menu_close);
      DISABLE_SUBMENU(main_replay_menu);
      DISABLE_MENU_ITEM(main_menu_save_snapshot);
      DISABLE_MENU_ITEM(main_menu_advance_frame);
      DISABLE_MENU_ITEM(machine_menu_soft_reset);
      DISABLE_MENU_ITEM(machine_menu_hard_reset);
      DISABLE_SUBMENU(machine_save_state_menu);
      DISABLE_MENU_ITEM(machine_menu_cheat_manager);
      DISABLE_SUBMENU(audio_record_menu);
      DISABLE_MENU_ITEM(video_layers_menu_flip_mirroring);
      DISABLE_MENU_ITEM(options_menu_reset_clock);
   }

   /* Page buffer and VSync are not supported in OpenGL mode. */
   SET_MENU_ITEM_ENABLED(video_menu_page_buffer, !video_is_opengl_mode ());
   SET_MENU_ITEM_ENABLED(video_menu_vsync,       !video_is_opengl_mode ());

   TOGGLE_MENU_ITEM(main_open_recent_menu_lock, lock_recent);

   TOGGLE_MENU_ITEM(main_replay_select_menu_0, (replay_index == 0));
   TOGGLE_MENU_ITEM(main_replay_select_menu_1, (replay_index == 1));
   TOGGLE_MENU_ITEM(main_replay_select_menu_2, (replay_index == 2));
   TOGGLE_MENU_ITEM(main_replay_select_menu_3, (replay_index == 3));
   TOGGLE_MENU_ITEM(main_replay_select_menu_4, (replay_index == 4));

   TOGGLE_MENU_ITEM(machine_menu_timing_smoothest,     (machine_timing == MACHINE_TIMING_SMOOTH));
   TOGGLE_MENU_ITEM(machine_menu_timing_most_accurate, (machine_timing == MACHINE_TIMING_ACCURATE));

   TOGGLE_MENU_ITEM(machine_save_state_select_menu_0, (save_state_index == 0));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_1, (save_state_index == 1));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_2, (save_state_index == 2));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_3, (save_state_index == 3));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_4, (save_state_index == 4));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_5, (save_state_index == 5));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_6, (save_state_index == 6));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_7, (save_state_index == 7));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_8, (save_state_index == 8));
   TOGGLE_MENU_ITEM(machine_save_state_select_menu_9, (save_state_index == 9));

   TOGGLE_MENU_ITEM(machine_save_state_autosave_menu_disabled,   (input_autosave_interval == 0));
   TOGGLE_MENU_ITEM(machine_save_state_autosave_menu_10_seconds, (input_autosave_interval == 10));
   TOGGLE_MENU_ITEM(machine_save_state_autosave_menu_30_seconds, (input_autosave_interval == 30));
   TOGGLE_MENU_ITEM(machine_save_state_autosave_menu_60_seconds, (input_autosave_interval == 60));

   TOGGLE_MENU_ITEM(machine_region_menu_automatic, (machine_region == MACHINE_REGION_AUTOMATIC));
   TOGGLE_MENU_ITEM(machine_region_menu_ntsc,      (machine_region == MACHINE_REGION_NTSC));
   TOGGLE_MENU_ITEM(machine_region_menu_pal,       (machine_region == MACHINE_REGION_PAL));

   TOGGLE_MENU_ITEM(machine_speed_up_down_menu_50_percent,  COMPARE_TWO_REALS(timing_speed_multiplier, 0.5));
   TOGGLE_MENU_ITEM(machine_speed_up_down_menu_100_percent, COMPARE_TWO_REALS(timing_speed_multiplier, 1.0));
   TOGGLE_MENU_ITEM(machine_speed_up_down_menu_200_percent, COMPARE_TWO_REALS(timing_speed_multiplier, 2.0));

   TOGGLE_MENU_ITEM(machine_menu_speed_cap, speed_cap);

   TOGGLE_MENU_ITEM(machine_frame_skip_menu_automatic, (frame_skip == -1));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_disabled,  (frame_skip == 0));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_1_frames,  (frame_skip == 1));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_2_frames,  (frame_skip == 2));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_3_frames,  (frame_skip == 3));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_4_frames,  (frame_skip == 4));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_5_frames,  (frame_skip == 5));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_6_frames,  (frame_skip == 6));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_7_frames,  (frame_skip == 7));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_8_frames,  (frame_skip == 8));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_9_frames,  (frame_skip == 9));
   TOGGLE_MENU_ITEM(machine_frame_skip_menu_10_frames, (frame_skip == 10));

   TOGGLE_MENU_ITEM(audio_menu_enable_apu,    apu_options.enabled);
   TOGGLE_MENU_ITEM(audio_menu_enable_output, audio_enable_output);

   TOGGLE_MENU_ITEM(audio_menu_emulation_fast,     (apu_options.emulation == APU_EMULATION_FAST));
   TOGGLE_MENU_ITEM(audio_menu_emulation_accurate, (apu_options.emulation == APU_EMULATION_ACCURATE));
   TOGGLE_MENU_ITEM(audio_menu_emulation_ultra,    (apu_options.emulation == APU_EMULATION_ULTRA));

   TOGGLE_MENU_ITEM(audio_channels_menu_square_1, apu_options.enable_square_1);
   TOGGLE_MENU_ITEM(audio_channels_menu_square_2, apu_options.enable_square_2);
   TOGGLE_MENU_ITEM(audio_channels_menu_triangle, apu_options.enable_triangle);
   TOGGLE_MENU_ITEM(audio_channels_menu_noise,    apu_options.enable_noise);
   TOGGLE_MENU_ITEM(audio_channels_menu_dmc,      apu_options.enable_dmc);
   TOGGLE_MENU_ITEM(audio_channels_menu_extra_1,  apu_options.enable_extra_1);
   TOGGLE_MENU_ITEM(audio_channels_menu_extra_2,  apu_options.enable_extra_2);
   TOGGLE_MENU_ITEM(audio_channels_menu_extra_3,  apu_options.enable_extra_3);

   TOGGLE_MENU_ITEM(audio_output_menu_subsystem_allegro, (audio_subsystem == AUDIO_SUBSYSTEM_ALLEGRO));
   TOGGLE_MENU_ITEM(audio_output_menu_subsystem_openal,  (audio_subsystem == AUDIO_SUBSYSTEM_OPENAL));

   TOGGLE_MENU_ITEM(audio_output_menu_sampling_rate_22050_hz, (audio_sample_rate == 22050));
   TOGGLE_MENU_ITEM(audio_output_menu_sampling_rate_44100_hz, (audio_sample_rate == 44100));
   TOGGLE_MENU_ITEM(audio_output_menu_sampling_rate_48000_hz, (audio_sample_rate == 48000));

   TOGGLE_MENU_ITEM(audio_output_menu_mixing_mono,            !apu_options.stereo);
   TOGGLE_MENU_ITEM(audio_output_menu_mixing_stereo,          (apu_options.stereo && !dsp_get_effector_enabled (DSP_EFFECTOR_SWAP_CHANNELS)));
   TOGGLE_MENU_ITEM(audio_output_menu_mixing_stereo_reversed, (apu_options.stereo && dsp_get_effector_enabled (DSP_EFFECTOR_SWAP_CHANNELS)));

   TOGGLE_MENU_ITEM(audio_output_buffer_menu_1_frame,  (audio_buffer_length == 1));
   TOGGLE_MENU_ITEM(audio_output_buffer_menu_2_frames, (audio_buffer_length == 2));
   TOGGLE_MENU_ITEM(audio_output_buffer_menu_3_frames, (audio_buffer_length == 3));
   TOGGLE_MENU_ITEM(audio_output_buffer_menu_4_frames, (audio_buffer_length == 4));
   TOGGLE_MENU_ITEM(audio_output_buffer_menu_5_frames, (audio_buffer_length == 5));
   TOGGLE_MENU_ITEM(audio_output_buffer_menu_6_frames, (audio_buffer_length == 6));
   TOGGLE_MENU_ITEM(audio_output_buffer_menu_7_frames, (audio_buffer_length == 7));
   TOGGLE_MENU_ITEM(audio_output_buffer_menu_8_frames, (audio_buffer_length == 8));

#ifdef ALLEGRO_DOS

   TOGGLE_MENU_ITEM(video_driver_dos_menu_vga,           (gfx_driver->id == GFX_VGA));
   TOGGLE_MENU_ITEM(video_driver_dos_menu_vga_mode_x,    (gfx_driver->id == GFX_MODEX));
   TOGGLE_MENU_ITEM(video_driver_dos_menu_vesa,          (gfx_driver->id == GFX_VESA1));
   TOGGLE_MENU_ITEM(video_driver_dos_menu_vesa_2_banked, (gfx_driver->id == GFX_VESA2B));
   TOGGLE_MENU_ITEM(video_driver_dos_menu_vesa_2_linear, (gfx_driver->id == GFX_VESA2L));
   TOGGLE_MENU_ITEM(video_driver_dos_menu_vesa_3,        (gfx_driver->id == GFX_VESA3));
   TOGGLE_MENU_ITEM(video_driver_dos_menu_vesa_vbe_af,   (gfx_driver->id == GFX_VBEAF));

#endif   /* ALLEGRO_DOS */

#ifdef ALLEGRO_WINDOWS

   TOGGLE_MENU_ITEM(video_driver_windows_menu_directx,         (gfx_driver->id == GFX_DIRECTX));
   TOGGLE_MENU_ITEM(video_driver_windows_menu_directx_window,  (gfx_driver->id == GFX_DIRECTX_WIN));
   TOGGLE_MENU_ITEM(video_driver_windows_menu_directx_overlay, (gfx_driver->id == GFX_DIRECTX_OVL));
   TOGGLE_MENU_ITEM(video_driver_windows_menu_gdi,             (gfx_driver->id == GFX_GDI));

#endif   /* ALLEGRO_WINDOWS */

#ifdef ALLEGRO_LINUX

   TOGGLE_MENU_ITEM(video_driver_linux_menu_vga,         (gfx_driver->id == GFX_VGA));
   TOGGLE_MENU_ITEM(video_driver_linux_menu_vga_mode_x,  (gfx_driver->id == GFX_MODEX));
   TOGGLE_MENU_ITEM(video_driver_linux_menu_vesa_vbe_af, (gfx_driver->id == GFX_VBEAF));
#ifdef GFX_FBCON
   TOGGLE_MENU_ITEM(video_driver_linux_menu_framebuffer, (gfx_driver->id == GFX_FBCON));
#endif
#ifdef GFX_SVGALIB
   TOGGLE_MENU_ITEM(video_driver_linux_menu_svgalib,     (gfx_driver->id == GFX_SVGALIB));
#endif

#endif   /* ALLEGRO_LINUX */

#ifdef ALLEGRO_UNIX

   TOGGLE_MENU_ITEM(video_driver_unix_menu_x_windows,      (gfx_driver->id == GFX_XWINDOWS));
   TOGGLE_MENU_ITEM(video_driver_unix_menu_x_windows_full, (gfx_driver->id == GFX_XWINDOWS_FULLSCREEN));
   TOGGLE_MENU_ITEM(video_driver_unix_menu_x_dga,          (gfx_driver->id == GFX_XDGA));
   TOGGLE_MENU_ITEM(video_driver_unix_menu_x_dga_full,     (gfx_driver->id == GFX_XDGA_FULLSCREEN));
   TOGGLE_MENU_ITEM(video_driver_unix_menu_x_dga_2,        (gfx_driver->id == GFX_XDGA2));

#endif   /* ALLEGRO_UNIX */

#ifdef USE_ALLEGROGL

   TOGGLE_MENU_ITEM(video_driver_menu_opengl_full, (gfx_driver->id == GFX_OPENGL_FULLSCREEN));
   TOGGLE_MENU_ITEM(video_driver_menu_opengl_win,  (gfx_driver->id == GFX_OPENGL_WINDOWED));

#endif   /* USE_ALLEGROGL */

   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_256_224,   ((SCREEN_W == 256)  && (SCREEN_H == 224)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_256_240,   ((SCREEN_W == 256)  && (SCREEN_H == 240)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_512_448,   ((SCREEN_W == 512)  && (SCREEN_H == 448)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_512_480,   ((SCREEN_W == 512)  && (SCREEN_H == 480)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_768_672,   ((SCREEN_W == 768)  && (SCREEN_H == 672)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_768_720,   ((SCREEN_W == 768)  && (SCREEN_H == 720)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_1024_896,  ((SCREEN_W == 1024) && (SCREEN_H == 896)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_1024_960,  ((SCREEN_W == 1024) && (SCREEN_H == 960)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_1280_1120, ((SCREEN_W == 1280) && (SCREEN_H == 1120)));
   TOGGLE_MENU_ITEM(video_resolution_proportionate_menu_1280_1200, ((SCREEN_W == 1280) && (SCREEN_H == 1200)));

   TOGGLE_MENU_ITEM(video_resolution_menu_320_240,   ((SCREEN_W == 320)  && (SCREEN_H == 240)));
   TOGGLE_MENU_ITEM(video_resolution_menu_400_300,   ((SCREEN_W == 400)  && (SCREEN_H == 300)));
   TOGGLE_MENU_ITEM(video_resolution_menu_640_480,   ((SCREEN_W == 640)  && (SCREEN_H == 480)));
   TOGGLE_MENU_ITEM(video_resolution_menu_800_600,   ((SCREEN_W == 800)  && (SCREEN_H == 600)));
   TOGGLE_MENU_ITEM(video_resolution_menu_1024_768,  ((SCREEN_W == 1024) && (SCREEN_H == 768)));
   TOGGLE_MENU_ITEM(video_resolution_menu_1152_864,  ((SCREEN_W == 1152) && (SCREEN_H == 864)));
   TOGGLE_MENU_ITEM(video_resolution_menu_1280_960,  ((SCREEN_W == 1280) && (SCREEN_H == 960)));
   TOGGLE_MENU_ITEM(video_resolution_menu_1280_1024, ((SCREEN_W == 1280) && (SCREEN_H == 1024)));
   TOGGLE_MENU_ITEM(video_resolution_menu_1600_1200, ((SCREEN_W == 1600) && (SCREEN_H == 1200)));

   TOGGLE_MENU_ITEM(video_color_depth_menu_paletted_8_bit,    (video_get_color_depth () == 8));
   TOGGLE_MENU_ITEM(video_color_depth_menu_true_color_15_bit, (video_get_color_depth () == 15));
   TOGGLE_MENU_ITEM(video_color_depth_menu_true_color_16_bit, (video_get_color_depth () == 16));
   TOGGLE_MENU_ITEM(video_color_depth_menu_true_color_24_bit, (video_get_color_depth () == 24));
   TOGGLE_MENU_ITEM(video_color_depth_menu_true_color_32_bit, (video_get_color_depth () == 32));

   TOGGLE_MENU_ITEM(video_buffer_menu_match_resolution, ((video_buffer_width == -1)  && (video_buffer_height == -1)));
   TOGGLE_MENU_ITEM(video_buffer_menu_256_240,          ((video_buffer_width == 256) && (video_buffer_height == 240)));
   TOGGLE_MENU_ITEM(video_buffer_menu_320_240,          ((video_buffer_width == 320) && (video_buffer_height == 240)));
   TOGGLE_MENU_ITEM(video_buffer_menu_512_480,          ((video_buffer_width == 512) && (video_buffer_height == 480)));
   TOGGLE_MENU_ITEM(video_buffer_menu_640_480,          ((video_buffer_width == 640) && (video_buffer_height == 480)));
   TOGGLE_MENU_ITEM(video_buffer_menu_256_256,          ((video_buffer_width == 256) && (video_buffer_height == 256)));
   TOGGLE_MENU_ITEM(video_buffer_menu_512_512,          ((video_buffer_width == 512) && (video_buffer_height == 512)));

   TOGGLE_MENU_ITEM(video_blitter_menu_automatic,          (video_get_blitter () == VIDEO_BLITTER_AUTOMATIC));
   TOGGLE_MENU_ITEM(video_blitter_menu_normal,             (video_get_blitter () == VIDEO_BLITTER_NORMAL));
   TOGGLE_MENU_ITEM(video_blitter_menu_des,                (video_get_blitter () == VIDEO_BLITTER_DES));
   TOGGLE_MENU_ITEM(video_blitter_menu_interpolated_2x,    (video_get_blitter () == VIDEO_BLITTER_INTERPOLATED_2X));
   TOGGLE_MENU_ITEM(video_blitter_menu_interpolated_2x_hq, (video_get_blitter () == VIDEO_BLITTER_INTERPOLATED_2X_HQ));
   TOGGLE_MENU_ITEM(video_blitter_menu_2xscl,              (video_get_blitter () == VIDEO_BLITTER_2XSCL));
   TOGGLE_MENU_ITEM(video_blitter_menu_desii,              (video_get_blitter () == VIDEO_BLITTER_DESII));
   TOGGLE_MENU_ITEM(video_blitter_menu_super_2xscl,        (video_get_blitter () == VIDEO_BLITTER_SUPER_2XSCL));
   TOGGLE_MENU_ITEM(video_blitter_menu_ultra_2xscl,        (video_get_blitter () == VIDEO_BLITTER_ULTRA_2XSCL));
   TOGGLE_MENU_ITEM(video_blitter_menu_hq2x,               (video_get_blitter () == VIDEO_BLITTER_HQ2X));
   TOGGLE_MENU_ITEM(video_blitter_menu_ntsc,               (video_get_blitter () == VIDEO_BLITTER_NTSC));
   TOGGLE_MENU_ITEM(video_blitter_menu_interpolated_3x,    (video_get_blitter () == VIDEO_BLITTER_INTERPOLATED_3X));
   TOGGLE_MENU_ITEM(video_blitter_menu_hq3x,               (video_get_blitter () == VIDEO_BLITTER_HQ3X));
   TOGGLE_MENU_ITEM(video_blitter_menu_hq4x,               (video_get_blitter () == VIDEO_BLITTER_HQ4X));
   TOGGLE_MENU_ITEM(video_blitter_menu_stretched,          (video_get_blitter () == VIDEO_BLITTER_STRETCHED));

   TOGGLE_MENU_ITEM(video_filters_menu_scanlines_25_percent,  (video_get_filter_list () & VIDEO_FILTER_SCANLINES_LOW));
   TOGGLE_MENU_ITEM(video_filters_menu_scanlines_50_percent,  (video_get_filter_list () & VIDEO_FILTER_SCANLINES_MEDIUM));
   TOGGLE_MENU_ITEM(video_filters_menu_scanlines_100_percent, (video_get_filter_list () & VIDEO_FILTER_SCANLINES_HIGH));

   TOGGLE_MENU_ITEM(video_menu_fullscreen,  video_force_fullscreen);
   TOGGLE_MENU_ITEM(video_menu_page_buffer, video_enable_page_buffer);
   TOGGLE_MENU_ITEM(video_menu_vsync,       video_enable_vsync);

   TOGGLE_MENU_ITEM(video_palette_menu_ntsc_color,     (video_get_palette_id () == DATA_INDEX(DEFAULT_PALETTE)));
   TOGGLE_MENU_ITEM(video_palette_menu_ntsc_grayscale, (video_get_palette_id () == DATA_INDEX(GRAYSCALE_PALETTE)));
   TOGGLE_MENU_ITEM(video_palette_menu_gnuboy,         (video_get_palette_id () == DATA_INDEX(GNUBOY_PALETTE)));
   TOGGLE_MENU_ITEM(video_palette_menu_nester,         (video_get_palette_id () == DATA_INDEX(NESTER_PALETTE)));
   TOGGLE_MENU_ITEM(video_palette_menu_nesticle,       (video_get_palette_id () == DATA_INDEX(NESTICLE_PALETTE)));
   TOGGLE_MENU_ITEM(video_palette_menu_modern_ntsc,    (video_get_palette_id () == DATA_INDEX(MODERN_NTSC_PALETTE)));
   TOGGLE_MENU_ITEM(video_palette_menu_modern_pal,     (video_get_palette_id () == DATA_INDEX(MODERN_PAL_PALETTE)));
   TOGGLE_MENU_ITEM(video_palette_menu_ega_mode_1,     (video_get_palette_id () == DATA_INDEX(EGA_PALETTE_1)));
   TOGGLE_MENU_ITEM(video_palette_menu_ega_mode_2,     (video_get_palette_id () == DATA_INDEX(EGA_PALETTE_2)));
   TOGGLE_MENU_ITEM(video_palette_menu_custom,         (video_get_palette_id () == -1));

   TOGGLE_MENU_ITEM(video_layers_menu_sprites_a,                 ppu_enable_sprite_layer_a);
   TOGGLE_MENU_ITEM(video_layers_menu_sprites_b,                 ppu_enable_sprite_layer_b);
   TOGGLE_MENU_ITEM(video_layers_menu_background,                ppu_enable_background_layer);
   TOGGLE_MENU_ITEM(video_layers_menu_hide_horizontal_scrolling, (video_edge_clipping & VIDEO_EDGE_CLIPPING_HORIZONTAL));
   TOGGLE_MENU_ITEM(video_layers_menu_hide_vertical_scrolling,   (video_edge_clipping & VIDEO_EDGE_CLIPPING_VERTICAL));

   TOGGLE_MENU_ITEM(options_input_menu_enable_zapper, input_enable_zapper);

   TOGGLE_MENU_ITEM(options_menu_show_status, video_display_status);

   TOGGLE_MENU_ITEM(options_cpu_usage_menu_passive,    (cpu_usage == CPU_USAGE_PASSIVE));
   TOGGLE_MENU_ITEM(options_cpu_usage_menu_normal,     (cpu_usage == CPU_USAGE_NORMAL));
   TOGGLE_MENU_ITEM(options_cpu_usage_menu_aggressive, (cpu_usage == CPU_USAGE_AGGRESSIVE));

   TOGGLE_MENU_ITEM(options_gui_theme_menu_classic,         (last_theme == &classic_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_stainless_steel, (last_theme == &stainless_steel_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_zero_4,          (last_theme == &zero_4_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_panta,           (last_theme == &panta_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_xodiac,          (last_theme == &xodiac_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_monochrome,      (last_theme == &monochrome_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_essence,         (last_theme == &essence_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_voodoo,          (last_theme == &voodoo_theme));
   TOGGLE_MENU_ITEM(options_gui_theme_menu_hugs_and_kisses, (last_theme == &hugs_and_kisses_theme));

   /* TODO: Find a better way to do this. */
   uszprintf (audio_menu_volume_text, sizeof (audio_menu_volume_text),
      "Current volume: %d%%", (int)ROUND((dsp_master_volume * 100.0)));
   audio_menu[AUDIO_MENU_VOLUME_TEXT].text = audio_menu_volume_text;
}

int gui_init (void)
{
   int index;

   /* Set up replacement objects. */
   gui_menu_draw_menu = sl_draw_menu;
   gui_menu_draw_menu_item = sl_draw_menu_item;

   /* Set up menus & dialogs. */
   load_menus ();
   load_dialogs ();

#ifdef ALLEGRO_DOS

   CHECK_MENU_ITEM(video_menu_fullscreen);
   DISABLE_MENU_ITEM(video_menu_fullscreen);

#endif   /* ALLEGRO_DOS */

#ifdef ALLEGRO_LINUX

#ifndef GFX_FBCON
   DISABLE_MENU_ITEM(video_driver_linux_menu_framebuffer);
#endif
#ifndef GFX_SVGALIB
   DISABLE_MENU_ITEM(video_driver_linux_menu_svgalib);
#endif

#endif   /* !ALLEGRO_LINUX */

   /* Select default palette. */
   CHECK_MENU_ITEM(video_palette_menu_modern_ntsc);

   /* Load configuration */
   gui_theme_id = get_config_int ("gui", "theme",       GUI_THEME_PANTA);
   lock_recent  = get_config_int ("gui", "lock_recent", FALSE);

   /* Load up recent items. */

   main_open_recent_menu_clear ();

   for (index = 0; index < OPEN_RECENT_SLOTS; index++)
   {
      USTRING key;
      const char *path;
      UCHAR *filename = open_recent_filenames[index];
      UCHAR *text     = open_recent_menu_texts[index];
      MENU  *menu     = &main_open_recent_menu[index];

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "recent%d", index);

      path = get_config_string ("gui", key, NULL);
      if (!path)
         continue;

      uszprintf (filename, USTRING_SIZE, "%s", path);
      uszprintf (text,     USTRING_SIZE, "&%d: %s", index, get_filename
         (path));

      /* Update menu. */
      menu->text = text;

      /* Enable menu. */
      menu->flags &= ~D_DISABLED;
   }

   /* Cheap hack to fix palette. */
   gui_is_active = TRUE;
   set_theme ();
   gui_is_active = FALSE;

   return (0);
}

void gui_exit (void)
{
   int index;
   STRING save_path;
   STRING host;

   /* Save configuration. */
   STRING_CLEAR(save_path);
   strncpy (save_path, get_config_string ("gui", "save_path", "./"),
      sizeof (save_path) - 1);
   set_config_string ("gui", "save_path", save_path);

   STRING_CLEAR(host);
   strncpy (host, get_config_string ("netplay", "host", ""), (sizeof(host) - 1));
   set_config_string ("netplay", "host", host);

   set_config_int ("gui", "theme",       gui_theme_id);
   set_config_int ("gui", "lock_recent", lock_recent);

   /* Save recent items. */

   for (index = 0; index < OPEN_RECENT_SLOTS; index++)
   {
      USTRING key;
      const UCHAR *filename = open_recent_filenames[index];

      if (!filename)
         continue;

      USTRING_CLEAR(key);
      uszprintf (key, sizeof (key), "recent%d", index);

      set_config_string ("gui", key, filename);
   }

   unload_menus ();
   unload_dialogs ();
}

int show_gui (BOOL first_run)
{
   want_exit = FALSE;

   do
   {
      int result;

      /* Clear restart flag. */
      gui_needs_restart = FALSE;

      /* Open GUI. */
   
      result = gui_open ();
   
      if (result != 0)
      {
         WARN("Failed to open GUI");
         return ((8 + result));
      }
   
      if (first_run)
      {
         /* Show console. */
         main_menu_view_console ();

         /* Clear flag. */
         first_run = FALSE;
      }

      /* Update menu states. */
      update_menus ();

      /* Run main dialog. */
      run_dialog (main_dialog, -1);

      /* Close GUI. */
      gui_close (want_exit);

   } while (gui_needs_restart);

   return (want_exit);
}

int gui_alert (const UCHAR *title, const UCHAR *s1, const UCHAR *s2, const
   UCHAR *s3, const UCHAR *b1, const UCHAR *b2, int c1, int c2)
{
   /* Alert dialog with 1 or 2 buttons.  The title, first string, and first
      button are required.  The rest are optional and may be NULL.

      This function can even be called when the GUI isn't open.  It'll
      automatically open the GUI long enough to display the dialog. */

   int result;
   BOOL gui_opened = FALSE;
   DIALOG *dialog;
   DIALOG *objframe;
   DIALOG *objxbutton;
   DIALOG *objstr1, *objstr2, *objstr3;
   DIALOG *objbtn1, *objbtn2;
   int s1len, s2len, s3len;
   int collapse = 0;

   RT_ASSERT(title);
   RT_ASSERT(s1);
   RT_ASSERT(b1);

   /* Handle any NULL entries. */

   if (!s2) s2 = empty_string;
   if (!s3) s3 = empty_string;

   if (ustrlen (s2) == 0)
      collapse += (text_height (font) + 3);
   if (ustrlen (s3) == 0)
      collapse += (text_height (font) + 3);

   if (!gui_is_active)
   {
      /* Open GUI. */
   
      result = gui_open ();
   
      if (result != 0)
      {
         WARN("Failed to open GUI");
         return ((8 + result));
      }

      gui_opened = TRUE;
   }

   /* Create dialog. */
   dialog = load_dialog (alert_dialog_base);
   if (!dialog)
   {
      WARN("Failed to create dialog structure");
      return (-1);
   }

   /* Get all objects. */

   objframe   = &dialog[ALERT_DIALOG_FRAME];
   objxbutton = &dialog[ALERT_DIALOG_CLOSE_BUTTON];
   objstr1    = &dialog[ALERT_DIALOG_STRING_1];
   objstr2    = &dialog[ALERT_DIALOG_STRING_2];
   objstr3    = &dialog[ALERT_DIALOG_STRING_3];
   objbtn1    = &dialog[ALERT_DIALOG_BUTTON_1];
   objbtn2    = &dialog[ALERT_DIALOG_BUTTON_2];

   /* Calculate string lengths (in pixels). */

   s1len = text_length (font, s1);
   s2len = text_length (font, s2);
   s3len = text_length (font, s3);

   /* Set up frame. */

   objframe->w   = (9 + MAX3(s1len, s2len, s3len) + 9);
   objframe->dp2 = (char *)title;

   objxbutton->x = ((objframe->w - objxbutton->w) - 4);

   /* Set up strings. */

   objstr1->x   = ((objframe->w / 2) - (s1len / 2));
   objstr1->dp2 = (char *)s1;

   objstr2->x   = ((objframe->w / 2) - (s2len / 2));
   objstr2->dp2 = (char *)s2;

   objstr3->x   = ((objframe->w / 2) - (s3len / 2));
   objstr3->dp2 = (char *)s3;

   /* Set up buttons. */

   objbtn1->x   = ((objframe->w / 2) - (objbtn1->w / 2));
   objbtn1->dp  = (char *)b1;
   objbtn1->key = c1;

   objbtn1->y -= collapse;

   if (b2)
   {
      objbtn1->x -= ((objbtn2->w / 2) + 4);

      objbtn2->x   = ((objbtn1->x + objbtn1->w) + 8);
      objbtn2->dp  = (char *)b2;
      objbtn2->key = c2;

      objbtn2->y -= collapse;
   }
   else
   {
      /* Hide the unused button. */

      objbtn2->flags |= D_HIDDEN;
      objbtn2->flags |= D_DISABLED;
   }

   /* Collapse frame. */
   objframe->h -= collapse;

   /* Show dialog. */
   result = show_dialog (dialog, ALERT_DIALOG_BUTTON_1);

   /* Destroy dialog. */
   unload_dialog (dialog);

   if (gui_opened)
   {
      /* Close GUI. */
      gui_close (FALSE);
   }

   if (result == ALERT_DIALOG_BUTTON_1)
      return (1); /* OK. */
   else
      return (2); /* Cancel. */
}

void gui_message (int color, const UCHAR *message, ...)
{
   va_list format;

   RT_ASSERT(message);

   va_start (format, message);
   uvszprintf (message_buffer, USTRING_SIZE, message, format);
   va_end (format);

   if (color == -1)
      color = GUI_TEXT_COLOR;

   draw_message (color);
}

void gui_heartbeat (void)
{
   /* Called in varous places to process NetPlay (if active), refresh the
      screen, and rest() to minimize CPU usage in the GUI. */

   if (netplay_mode)
      netplay_process ();

   refresh ();

   if (cpu_usage == CPU_USAGE_PASSIVE)
      rest (1);
   else if (cpu_usage == CPU_USAGE_NORMAL)
      rest (0);
}

void gui_handle_keypress (int c, int scancode)
{
   switch (scancode)
   {
      case KEY_F1:
      {
         /* Save snapshot. */
         main_menu_save_snapshot ();

         break;
      }

      case KEY_F2:
      {
         /* Toggle status display. */
         options_menu_show_status ();

         break;
      }

      case KEY_F3:
      {
         /* Quick save state. */
         machine_save_state_menu_quick_save ();

         /* See if the save succeeded. */
         if (check_save_state (-1))
            message_local ("QuickSave: OK");
         else
            message_local ("QuickSave: Failed");
            
         break;
      }

      case KEY_F4:
      {
         /* Quick load state. */

         if (!(input_mode & INPUT_MODE_REPLAY))
            machine_save_state_menu_quick_load ();

         break;
      }

      case KEY_F5:
      {
         /* Save state. */
         machine_save_state_menu_save ();

         break;
      }

      case KEY_F6:
      {
         /* Load state. */

         if (!(input_mode & INPUT_MODE_REPLAY))
            machine_save_state_menu_restore ();

         break;
      }

      case KEY_F7:
      {
         /* Toggle sprites. */

         video_layers_menu_sprites_a ();
         video_layers_menu_sprites_b ();

         break;
      }

      case KEY_F8:
      {
         /* Toggle background. */
         video_layers_menu_background ();

         break;
      }

      case KEY_F9:
      {
         /* Toggle half speed mode. */

         timing_half_speed = !timing_half_speed;
         timing_update_speed ();

         break;
      }

      case KEY_F12:
      {
         /* Start/stop replay recording. */

         if (!(input_mode & INPUT_MODE_REPLAY_PLAY))
         {
            if (input_mode & INPUT_MODE_REPLAY_RECORD)
               main_replay_record_menu_stop ();
            else
               main_replay_record_menu_start ();

            break;
         }
      }

      case KEY_0:
      case KEY_1:
      case KEY_2:
      case KEY_3:
      case KEY_4:
      case KEY_5:
      case KEY_6:
      case KEY_7:
      case KEY_8:
      case KEY_9:
      {
         /* Select state slot. */

         if (!(input_mode & INPUT_MODE_CHAT))
         {
            save_state_index = (scancode - KEY_0);
    
            message_local ("Machine state slot set to %d.",
               save_state_index);
         }

         break;
      }

      case KEY_MINUS:
      case KEY_MINUS_PAD:
      {
         audio_menu_volume_decrease ();
         break;
      }

      case KEY_EQUALS:
      case KEY_PLUS_PAD:
      {
         audio_menu_volume_increase ();
         break;
      }

      default:
         break;
   }
}

void gui_stop_replay (void)
{
   main_replay_play_menu_stop ();
}

void gui_set_theme (const GUI_THEME *theme)
{
   int index;

   RT_ASSERT(theme);

   last_theme = theme;

   memcpy (&gui_theme, theme, sizeof (GUI_THEME));

   video_set_palette (NULL);

   for (index = 0; index < GUI_TOTAL_COLORS; index++)
      pack_color (&gui_theme[index]);

   gui_bg_color = GUI_FILL_COLOR;
   gui_fg_color = GUI_TEXT_COLOR;
   gui_mg_color = GUI_DISABLED_COLOR;
}

/* --- Utility functions. --- */

static INLINE void set_autosave (int interval)
{
   /* This function simply sets the save state autosave interval to
      'interval' seconds (in game speed, not real world speed :b). */

   input_autosave_interval = interval;
   update_menus ();

   if (interval <= 0)
      message_local ("Autosave disabled.");
   else
      message_local ("Autosave interval set to %d seconds.", interval);
}

static INLINE void close_file (void);
static int main_replay_menu_select (void);

static INLINE int load_file (const UCHAR *filename)
{
   /* This function loads the ROM specified by filename.  The file is NOT
      automatically added to the recent items list.  That must be done
      manually (currently by main_menu_open()).

      The return value of this function should be passed back to the calling
      dialog (e.g, D_CLOSE to close it and start the emulation, etc.). */

   ROM rom;

   if (load_rom (filename, &rom) != 0)
   {
      gui_message (GUI_ERROR_COLOR, "Failed to load ROM!");

      return (D_O_K);
   }
   else
   {
      USTRING scratch;

      if (rom_is_loaded)
      {
         /* Close currently open ROM and save data. */
         close_file ();
      }

      memcpy (&global_rom, &rom, sizeof (ROM));

      /* Update save state titles. */
      machine_save_state_menu_select ();
      /* Update replay titles. */
      main_replay_menu_select ();

      rom_is_loaded = TRUE;

      /* Initialize machine. */
      machine_init ();

      /* Clear the game clock. */
      options_menu_reset_clock ();

      ENABLE_MENU_ITEM(main_menu_resume);
      ENABLE_MENU_ITEM(main_menu_close);
      ENABLE_SUBMENU(main_replay_menu);
      ENABLE_MENU_ITEM(main_menu_save_snapshot);
      ENABLE_MENU_ITEM(main_menu_advance_frame);
      ENABLE_MENU_ITEM(machine_menu_soft_reset);
      ENABLE_MENU_ITEM(machine_menu_hard_reset);
      ENABLE_SUBMENU(machine_save_state_menu);
      ENABLE_MENU_ITEM(machine_menu_cheat_manager);
      ENABLE_SUBMENU(audio_record_menu);
      ENABLE_MENU_ITEM(video_layers_menu_flip_mirroring);
      ENABLE_MENU_ITEM(options_menu_reset_clock);
      
      /* Update window title. */
      uszprintf (scratch, sizeof (scratch), "FakeNES - %s", get_filename
         (global_rom.filename));
      set_window_title (scratch);

      return (D_CLOSE);
   }
}

static INLINE void close_file (void)
{
   /* Unloads the current ROM and returns the emulator to it's default
      state. */

   /* Save SRAM. */
   save_sram ();      

   /* Save patches. */
   save_patches ();

   /* Close machine. */
   machine_exit ();

   /* Unload ROM. */
   free_rom (&global_rom);
   rom_is_loaded = FALSE;

   update_menus ();
}

static int open_lobby (void)
{
   /* This function handles the entire GUI end of the NetPlay lobby.  It
      does not return until the NetPlay session has been terminated.

      Returns one of the following:
         D_O_K   - The NetPlay session has been closed, by pressing either
                   the [ x] close button or the Cancel button.
         D_CLOSE - The Netplay session is still open, all neccessary data
                   has been distributed and subsequently loaded, and control
                   should be transfered to the main loop. */

   BITMAP *bmp;                       
   DIALOG *dialog;
   int index = 0;
   DIALOG *obj_frame;
   DIALOG *obj_chat;
   DIALOG *obj_list;
   DIALOG *obj_message;
   DIALOG *obj_load;
   DIALOG *obj_ok;
   USTRING chat;
   USTRING list;
   USTRING message;
   DIALOG_PLAYER *player;
   int object_id;

   bmp = gui_get_screen ();

   /* Clear screen. */
   clear_bitmap (bmp);

   /* Get dialog. */
   dialog = lobby_dialog;

   /* Center dialog. */
   centre_dialog (dialog);

   while (dialog[index].d1 != SL_FRAME_END)
   {
      /* Update colors. */

      DIALOG *object = &dialog[index];

      object->fg = GUI_TEXT_COLOR;
      object->bg = gui_bg_color;

      index++;
   }

   /* Get dialog objects. */
   obj_frame   = &dialog[LOBBY_DIALOG_FRAME];
   obj_chat    = &dialog[LOBBY_DIALOG_CHAT];
   obj_list    = &dialog[LOBBY_DIALOG_LIST];
   obj_message = &dialog[LOBBY_DIALOG_MESSAGE];
   obj_load    = &dialog[LOBBY_DIALOG_LOAD_BUTTON];
   obj_ok      = &dialog[LOBBY_DIALOG_OK_BUTTON];

   /* Set up dialog objects. */

   obj_frame->dp3 = DATA_TO_FONT(LARGE_FONT);

   obj_chat->bg = makecol (0, 0, 0);
   obj_chat->fg = makecol (240, 240, 240);
   obj_chat->d1 = ((sizeof (chat) / MAX_UCHAR_LENGTH) - 1);
   obj_chat->dp = chat;

   obj_list->bg = makecol (0, 0, 0);
   obj_list->fg = makecol (240, 240, 240);
   obj_list->d1 = ((sizeof (list) / MAX_UCHAR_LENGTH) - 1);
   obj_list->dp = list;

   obj_message->d1 = ((sizeof (message) / MAX_UCHAR_LENGTH) - 1);
   obj_message->dp = message;

   if (netplay_mode != NETPLAY_MODE_SERVER_OPEN)
      obj_load->flags |= D_DISABLED;

   obj_ok->flags |= D_DISABLED;

   /* Clear text buffers. */
   USTRING_CLEAR(chat);
   USTRING_CLEAR(list);
   USTRING_CLEAR(message);
        
   /* Run dialog. */

   player = init_dialog (dialog, -1);
   if (!player)
   {
      gui_message (GUI_ERROR_COLOR, "Failed to create dialog player!");
      return (D_O_K);
   }

   while (update_dialog (player))
   {
      netplay_enumerate_chat (chat, sizeof(chat));
      netplay_enumerate_clients (list, sizeof(list));

      scare_mouse ();
      object_message (obj_chat, MSG_DRAW, 0);
      object_message (obj_list, MSG_DRAW, 0);
      unscare_mouse ();

      gui_heartbeat ();
   }

   object_id = shutdown_dialog (player);

   switch (object_id)
   {
      case LOBBY_DIALOG_OK_BUTTON:
         return (D_CLOSE);

      default:
      {
         /* End NetPlay session. */
         netplay_close ();
   
         /* Clear screen. */
         clear_bitmap (bmp);
      
         /* Draw background. */
         draw_background ();
      
         message_local ("NetPlay session closed.");

         return (D_O_K);
      }
   }
}

/* --- Menu handlers. --- */

static int main_menu_resume (void)
{
    return (D_CLOSE);
}

static int main_menu_open (void)
{
   USTRING path;
   BOOL locked;
   int result;
   USTRING scratch;

   if (rom_is_loaded)
   {
      if (gui_alert ("Confirmation", "A ROM is already loaded.", "If you continue, any unsaved progress will be lost.", "Are you sure?", "&OK", "&Cancel", 0, 0) == 2)
      {
         /* Cancelled. */
         return (D_O_K);
      }
   }

   /* Retrive path from configuration file. */
   USTRING_CLEAR(path);
#ifdef POSIX
   ustrncat (path, get_config_string ("gui", "open_path", "~/"), (sizeof(path) - 1));
#else
   ustrncat (path, get_config_string ("gui", "open_path", "/"), (sizeof(path) - 1));
#endif

   locked = get_config_int ("gui", "lock_paths", FALSE);

#ifdef USE_ZLIB
   result = gui_file_select ("Open", "Supported file types (*.NES, *.GZ, *.ZIP)", path, sizeof(path), "*.nes;*.gz;*.zip");
#else
   result = gui_file_select ("Open", "Supported file types (*.NES)", path, sizeof(path), "*.nes");
#endif

   if (!locked)
   {  
      /* Update path. */
      set_config_string ("gui", "open_path", replace_filename (scratch, path, "", sizeof (scratch)));
   }

   if (result != 0)
   {
      /* Dialog was OK'ed. */

      int result;

      result = load_file (path);
      if ((result == D_CLOSE) && !lock_recent)
      {
         /* Load succeeded; add file to recent items list. */

         int index;

         /* Move all existing entries down by 1 slot. */
         for (index = (OPEN_RECENT_SLOTS - 2); index >= 0; index--)
            ustrncpy (open_recent_filenames[(index + 1)], open_recent_filenames[index], USTRING_SIZE);

         /* Add new entry to the beginning of the list. */
         uszprintf (open_recent_filenames[0], USTRING_SIZE, "%s", path);

         /* Update menus. */

         for (index = 0; index < OPEN_RECENT_SLOTS; index++)
         {
            const UCHAR *filename = open_recent_filenames[index];
            UCHAR       *text     = open_recent_menu_texts[index];
            MENU        *menu     = &main_open_recent_menu[index];

            if (filename[0])
            {
               /* Build menu text. */
               uszprintf (text, USTRING_SIZE, "&%d: %s", index, get_filename (filename));

               /* Enable menu. */
               menu->flags &= ~D_DISABLED;
            }
            else
            {
               /* Build menu text. */
               uszprintf (text, USTRING_SIZE, "&%d: %s", index, UNUSED_SLOT_TEXT);

               /* Disable menu. */
               menu->flags |= D_DISABLED;
            }

            /* Set menu text. */
            menu->text = text;
         }
      }

      return (result);
   }

   /* Dialog was cancelled. */
   return (D_O_K);
}

#define OPEN_RECENT_MENU_HANDLER(index) \
   static int main_open_recent_menu_##index (void)  \
   {  \
      if (rom_is_loaded) \
      { \
         if (gui_alert ("Confirmation", "A ROM is already loaded.", "If you continue, any unsaved progress will be lost.", "Are you sure?", "&OK", "&Cancel", 0, 0) == 2) \
         { \
            /* Cancelled. */ \
            return (D_O_K); \
         } \
      } \
      \
      return (load_file (open_recent_filenames[index])); \
   }

OPEN_RECENT_MENU_HANDLER(0)
OPEN_RECENT_MENU_HANDLER(1)
OPEN_RECENT_MENU_HANDLER(2)
OPEN_RECENT_MENU_HANDLER(3)
OPEN_RECENT_MENU_HANDLER(4)
OPEN_RECENT_MENU_HANDLER(5)
OPEN_RECENT_MENU_HANDLER(6)
OPEN_RECENT_MENU_HANDLER(7)
OPEN_RECENT_MENU_HANDLER(8)
OPEN_RECENT_MENU_HANDLER(9)

#undef OPEN_RECENT_MENU_HANDLER

static int main_open_recent_menu_lock (void)
{
   lock_recent = !lock_recent;
   update_menus ();

   return (D_O_K);
}

static int main_open_recent_menu_clear (void)
{
   int index;

   for (index = 0; index < OPEN_RECENT_SLOTS; index++)
   {
      UCHAR *filename = open_recent_filenames[index];
      UCHAR *text     = open_recent_menu_texts[index];
      MENU  *menu     = &main_open_recent_menu[index];

      USTRING_CLEAR(filename);

      /* Build menu text. */
      uszprintf (text, USTRING_SIZE, "&%d: %s", index, UNUSED_SLOT_TEXT);

      /* Update menu. */
      menu->text = text;

      /* Disable menu. */
      menu->flags |= D_DISABLED;
   }

   return (D_O_K);
}

static int main_menu_close (void)
{
   if (gui_alert ("Confirmation", "If you continue, any unsaved progress will be lost.", "Are you sure you want to unload the ROM?", NULL, "&OK", "&Cancel", 0, 0) == 2)
   {
      /* Cancelled. */
      return (D_O_K);
   }

   /* Unload ROM. */
   close_file ();

   /* Clear the screen and restore background. */
   cycle_video ();

   return (D_REDRAW);
}

#define REPLAY_SELECT_MENU_HANDLER(index) \
   static int main_replay_select_menu_##index (void)  \
   {  \
      replay_index = index;   \
      update_menus ();  \
      message_local ("Replay slot set to %d.", index);   \
      return (D_O_K);   \
   }

REPLAY_SELECT_MENU_HANDLER(0)
REPLAY_SELECT_MENU_HANDLER(1)
REPLAY_SELECT_MENU_HANDLER(2)
REPLAY_SELECT_MENU_HANDLER(3)
REPLAY_SELECT_MENU_HANDLER(4)
REPLAY_SELECT_MENU_HANDLER(5)
REPLAY_SELECT_MENU_HANDLER(6)
REPLAY_SELECT_MENU_HANDLER(7)
REPLAY_SELECT_MENU_HANDLER(8)
REPLAY_SELECT_MENU_HANDLER(9)

#undef REPLAY_SELECT_MENU_HANDLER

static int main_replay_menu_select (void)
{
   int index;

   for (index = 0; index < REPLAY_SLOTS; index++)
   {
      UCHAR *title;
      UCHAR *text;

      title = replay_titles[index];
      text = replay_menu_texts[index];

      /* Get title. */
      get_replay_title (index, title, USTRING_SIZE);

      /* Build menu text. */
      uszprintf (text, USTRING_SIZE, "&%d: %s", index, title);

      /* Update menu. */
      main_replay_select_menu[index].text = text;
   }

   return (D_O_K);
}

static int main_replay_record_menu_start (void)
{
   USTRING title;

   /* Duplicate title. */
   ustrncpy (title, replay_titles[replay_index], USTRING_SIZE);

   /* Patch up duplicate. */
   fix_save_title (title, sizeof (title));
                 
   if (gui_is_active)
   {
      DIALOG *dialog;
      DIALOG *objtitle;

      /* Allow user to customize title before save. */

      dialog = main_replay_record_start_dialog;

      objtitle = &dialog[MAIN_REPLAY_RECORD_START_DIALOG_TITLE];

      objtitle->d1 = NEW_SAVE_TITLE_SIZE;
      objtitle->dp = title;

      if (show_dialog (dialog, -1) !=
         MAIN_REPLAY_RECORD_START_DIALOG_OK_BUTTON)
      {
         /* Dialog was cancelled. */
         return (D_O_K);
      }
   }

   /* Open replay file. */
   if (!open_replay (replay_index, "w", title))
   {
      gui_message (GUI_ERROR_COLOR, "Failed to open new machine state "
         "file.");

      return (D_O_K);
   }

   DISABLE_MENU_ITEM(main_menu_open);
   DISABLE_SUBMENU(main_open_recent_menu);
   DISABLE_MENU_ITEM(main_menu_close);
   DISABLE_MENU_ITEM(main_replay_record_menu_start);
   ENABLE_MENU_ITEM(main_replay_record_menu_stop);
   DISABLE_SUBMENU(main_replay_select_menu);
   DISABLE_SUBMENU(main_replay_play_menu);
   DISABLE_SUBMENU(machine_save_state_autosave_menu);
   DISABLE_SUBMENU(netplay_menu);

   /* Enter replay recording mode. */
   input_mode |= INPUT_MODE_REPLAY;
   input_mode |= INPUT_MODE_REPLAY_RECORD;
 
   message_local ("Replay recording session started.");
 
   /* Update replay titles. */
   main_replay_menu_select ();

   return (D_CLOSE);
}

static int main_replay_record_menu_stop (void)
{
   /* Close replay. */
   close_replay ();

   /* Exit replay recording mode. */
   input_mode &= ~INPUT_MODE_REPLAY;
   input_mode &= ~INPUT_MODE_REPLAY_RECORD;

   ENABLE_MENU_ITEM(main_menu_open);
   ENABLE_SUBMENU(main_open_recent_menu);
   ENABLE_MENU_ITEM(main_menu_close);
   ENABLE_MENU_ITEM(main_replay_record_menu_start);
   DISABLE_MENU_ITEM(main_replay_record_menu_stop);
   ENABLE_SUBMENU(main_replay_select_menu);
   ENABLE_SUBMENU(main_replay_play_menu);
   ENABLE_SUBMENU(machine_save_state_autosave_menu);
   ENABLE_SUBMENU(netplay_menu);

   message_local ("Replay recording session stopped.");

   return (D_O_K);
}

static int main_replay_play_menu_start (void)
{
   if (!open_replay (replay_index, "r", NULL))
   {                       
      gui_message (GUI_ERROR_COLOR, "Failed to open machine state file.");

      return (D_O_K);
   }

   DISABLE_MENU_ITEM(main_menu_open);
   DISABLE_SUBMENU(main_open_recent_menu);
   DISABLE_MENU_ITEM(main_menu_close);
   DISABLE_MENU_ITEM(main_replay_play_menu_start);
   ENABLE_MENU_ITEM(main_replay_play_menu_stop);
   DISABLE_SUBMENU(main_replay_select_menu);
   DISABLE_SUBMENU(main_replay_record_menu);
   DISABLE_MENU_ITEM(machine_menu_soft_reset);
   DISABLE_MENU_ITEM(machine_menu_hard_reset);
   DISABLE_MENU_ITEM(machine_save_state_menu_quick_load);
   DISABLE_MENU_ITEM(machine_save_state_menu_restore);
   DISABLE_MENU_ITEM(machine_menu_cheat_manager);
   DISABLE_SUBMENU(netplay_menu);

   /* Enter replay playback mode. */
   input_mode &= ~INPUT_MODE_PLAY;
   input_mode |= INPUT_MODE_REPLAY;
   input_mode |= INPUT_MODE_REPLAY_PLAY;

   message_local ("Replay playback started.");

   return (D_CLOSE);
}

static int main_replay_play_menu_stop (void)
{
   /* Close replay. */
   close_replay ();

   /* Exit replay playback mode. */
   if (!(input_mode & INPUT_MODE_CHAT))
      input_mode |= INPUT_MODE_PLAY;
   input_mode &= ~INPUT_MODE_REPLAY;
   input_mode &= ~INPUT_MODE_REPLAY_PLAY;

   ENABLE_MENU_ITEM(main_menu_open);
   ENABLE_SUBMENU(main_open_recent_menu);
   ENABLE_MENU_ITEM(main_menu_close);
   ENABLE_MENU_ITEM(main_replay_play_menu_start);
   DISABLE_MENU_ITEM(main_replay_play_menu_stop);
   ENABLE_SUBMENU(main_replay_select_menu);
   ENABLE_SUBMENU(main_replay_record_menu);
   ENABLE_MENU_ITEM(machine_menu_soft_reset);
   ENABLE_MENU_ITEM(machine_menu_hard_reset);
   ENABLE_MENU_ITEM(machine_save_state_menu_quick_load);
   ENABLE_MENU_ITEM(machine_save_state_menu_restore);
   ENABLE_MENU_ITEM(machine_menu_cheat_manager);
   ENABLE_SUBMENU(netplay_menu);

   if (gui_is_active)
      message_local ("Replay playback stopped.");
   else
      message_local ("Replay playback finished.");

   return (D_O_K);
}

static int main_menu_save_snapshot (void)
{
   int index;

   for (index = 0; index < 999; index++)
   {
      USTRING path;
      USTRING filename;

      uszprintf (filename, sizeof (filename), "%s_%03d.pcx", get_filename
         (global_rom.filename), index);

      /* Merge it with our save path. */
      get_save_path (filename, sizeof (filename));

      if (exists (filename))
         continue;

      save_bitmap (filename, video_buffer, video_palette);

      message_local ("Snapshot saved to %s.", filename);

      return (D_O_K);
   }

   gui_message (GUI_ERROR_COLOR, "Couldn't find a suitable image "
      "filename.");

   return (D_O_K);
}

static int main_menu_advance_frame (void)
{
   frames_to_execute = 1;

   return (D_CLOSE);
}

static int main_menu_view_console (void)
{
   DIALOG *dialog;
   DIALOG *objframe;
   DIALOG *objview;

   /* Create dialog. */
   dialog = load_dialog (viewer_dialog_base);
   if (!dialog)
   {
      WARN("Failed to create dialog structure");
      return (-1);
   }

   /* Get objects. */

   objframe = &dialog[VIEWER_DIALOG_FRAME];
   objview  = &dialog[VIEWER_DIALOG_TEXT];

   /* Set up objects. */

   objframe->dp2 = "Console";

   objview->dp = get_console_text ();

   /* Show dialog. */
   show_dialog (dialog, -1);

   /* Destroy dialog. */
   unload_dialog (dialog);

   return (D_O_K);
}

static int main_menu_view_log (void)
{
   DIALOG *dialog;
   DIALOG *objframe;
   DIALOG *objview;

   /* Create dialog. */
   dialog = load_dialog (viewer_dialog_base);
   if (!dialog)
   {
      WARN("Failed to create dialog structure");
      return (-1);
   }

   /* Get objects. */

   objframe = &dialog[VIEWER_DIALOG_FRAME];
   objview  = &dialog[VIEWER_DIALOG_TEXT];

   /* Set up objects. */

   objframe->dp2 = "Log";

   objview->dp = get_log_text ();

   /* Show dialog. */
   show_dialog (dialog, -1);

   /* Destroy dialog. */
   unload_dialog (dialog);

   return (D_O_K);
}

static int main_menu_exit (void)
{
   if (rom_is_loaded)
   {
      /* Confirm exit. */

      if (gui_alert ("Confirmation", "A ROM is currently loaded.", "If you continue, any unsaved progress will be lost.", "Really exit?", "&OK", "&Cancel", 0, 0) == 2)
      {
         /* Cancelled. */
         return (D_O_K);
      }
      else
      {
         want_exit = TRUE;
      
         return (D_CLOSE);
      }
   }
   else
   {
      /* Just exit. */

      want_exit = TRUE;
   
      return (D_CLOSE);
   }
}

#define SAVE_STATE_SELECT_MENU_HANDLER(index)   \
   static int machine_save_state_select_menu_##index (void)   \
   {  \
      save_state_index = index;  \
      update_menus ();  \
      message_local ("Machine state slot set to %d.", index);  \
      return (D_O_K);   \
   }

SAVE_STATE_SELECT_MENU_HANDLER(0);
SAVE_STATE_SELECT_MENU_HANDLER(1);
SAVE_STATE_SELECT_MENU_HANDLER(2);
SAVE_STATE_SELECT_MENU_HANDLER(3);
SAVE_STATE_SELECT_MENU_HANDLER(4);
SAVE_STATE_SELECT_MENU_HANDLER(5);
SAVE_STATE_SELECT_MENU_HANDLER(6);
SAVE_STATE_SELECT_MENU_HANDLER(7);
SAVE_STATE_SELECT_MENU_HANDLER(8);
SAVE_STATE_SELECT_MENU_HANDLER(9);

#undef SAVE_STATE_MENU_HANDLER

static int machine_save_state_menu_quick_save (void)
{
   if (!save_state (-1, "QUICKSAVE"))
   {
      gui_message (GUI_ERROR_COLOR, "Quick Save failed.");

      return (D_O_K);
   }

   return (D_CLOSE);
}

static int machine_save_state_menu_quick_load (void)
{
   if (!load_state (-1))
   {
      gui_message (GUI_ERROR_COLOR, "Quick Load failed.");

      return (D_O_K);
   }

   return (D_CLOSE);
}

static int machine_save_state_menu_save (void)
{
   USTRING title;
   USTRING filename;

   /* Duplicate title. */
   ustrncpy (title, save_state_titles[save_state_index], sizeof (title));

   /* Patch up duplicate. */
   fix_save_title (title, sizeof (title));

   if (gui_is_active)
   {
      DIALOG *dialog;
      DIALOG *objtitle;

      /* Allow user to customize title before save. */

      dialog = machine_save_state_save_dialog;

      objtitle = &dialog[MACHINE_SAVE_STATE_SAVE_DIALOG_TITLE];

      objtitle->d1 = NEW_SAVE_TITLE_SIZE;
      objtitle->dp = title;

      if (show_dialog (dialog, -1) !=
         MACHINE_SAVE_STATE_SAVE_DIALOG_OK_BUTTON)
      {
         /* Cancelled. */
         return (D_O_K);
      }
   }

   if (!save_state (save_state_index, title))
   {
      gui_message (GUI_ERROR_COLOR, "Failed to open new machine state "
         "file.");

      return (D_O_K);
   }

   message_local ("Machine state saved to slot %d.", save_state_index);

   /* Update save state titles. */
   machine_save_state_menu_select ();

   return (D_CLOSE);
}

static int machine_save_state_menu_restore (void)
{
   if (!load_state (save_state_index))
   {
      gui_message (GUI_ERROR_COLOR, "Failed to open machine state file.");

      return (D_O_K);
   }

   message_local ("Machine state loaded from slot %d.", save_state_index);

   return (D_CLOSE);
}

static int machine_save_state_menu_select (void)
{
   int index;

   for (index = 0; index < SAVE_STATE_SLOTS; index++)
   {
      UCHAR *title;
      UCHAR *text;

      title = save_state_titles[index];
      text = save_state_menu_texts[index];

      /* Get title. */
      get_state_title (index, title, USTRING_SIZE);

      /* Build menu text. */
      uszprintf (text, USTRING_SIZE, "&%d: %s", index, title);

      /* Update menu. */
      machine_save_state_select_menu[index].text = text;
   }

   return (D_O_K);
}

static int machine_save_state_autosave_menu_disabled (void)
{
   set_autosave (0);

   return (D_O_K);
}

static int machine_save_state_autosave_menu_10_seconds (void)
{
   set_autosave (10);

   return (D_O_K);
}

static int machine_save_state_autosave_menu_30_seconds (void)
{
   set_autosave (30);

   return (D_O_K);
}

static int machine_save_state_autosave_menu_60_seconds (void)
{
   set_autosave (60);

   return (D_O_K);
}

static int machine_save_state_autosave_menu_custom (void)
{
   int seconds;

   seconds = input_autosave_interval;

   if (get_integer_input ("Custom", &seconds, "seconds"))
      set_autosave (seconds);

   return (D_O_K);
}

static int machine_region_menu_automatic (void)
{
   machine_region = MACHINE_REGION_AUTOMATIC;
   timing_update_machine_type ();
   update_menus ();

   message_local ("System region set to automatic.");

   return (D_O_K);
}

static int machine_region_menu_ntsc (void)
{
   machine_region = MACHINE_REGION_NTSC;
   timing_update_machine_type ();
   update_menus ();

   message_local ("System region set to NTSC.");

   return (D_O_K);
}

static int machine_region_menu_pal (void)
{
   machine_region = MACHINE_REGION_PAL;
   timing_update_machine_type ();
   update_menus ();

   message_local ("System region set to PAL.");

   return (D_O_K);
}

static int machine_speed_up_down_menu_50_percent (void)
{
   timing_speed_multiplier = 0.5;
   timing_update_speed ();

   update_menus ();

   message_local ("Machine speed factor set to 50%%.");

   return (D_O_K);
}

static int machine_speed_up_down_menu_100_percent (void)
{
   timing_speed_multiplier = 1.0;
   timing_update_speed ();

   update_menus ();

   message_local ("Machine speed factor set to 100%%.");

   return (D_O_K);
}

static int machine_speed_up_down_menu_200_percent (void)
{
   timing_speed_multiplier = 2.0;
   timing_update_speed ();

   update_menus ();

   message_local ("Machine speed factor set to 200%%.");

   return (D_O_K);
}

static int machine_speed_up_down_menu_custom (void)
{
   REAL value;

   value = (timing_speed_multiplier * 100.0);

   if (get_float_input ("Custom", &value, "percent"))
   {
      timing_speed_multiplier = (value / 100.0);
      timing_update_speed ();

      update_menus ();

      message_local ("Machine speed factor set to custom.");
   }

   return (D_O_K);
}

static int machine_frame_skip_menu_automatic (void)
{
   frame_skip = -1;
   update_menus ();

   message_local ("Frame skip set to automatic.");

   return (D_O_K);
}

static int machine_frame_skip_menu_disabled (void)
{
   frame_skip = 0;
   update_menus ();

   message_local ("Frame skip disabled.");

   return (D_O_K);
}

#define FRAME_SKIP_MENU_HANDLER(frames)   \
   static int machine_frame_skip_menu_##frames##_frames (void) \
   {  \
      frame_skip = frames; \
      update_menus ();  \
      message_local ("Frame skip set to %d frames.", frames);  \
      return (D_O_K);   \
   }

FRAME_SKIP_MENU_HANDLER(1)
FRAME_SKIP_MENU_HANDLER(2)
FRAME_SKIP_MENU_HANDLER(3)
FRAME_SKIP_MENU_HANDLER(4)
FRAME_SKIP_MENU_HANDLER(5)
FRAME_SKIP_MENU_HANDLER(6)
FRAME_SKIP_MENU_HANDLER(7)
FRAME_SKIP_MENU_HANDLER(8)
FRAME_SKIP_MENU_HANDLER(9)
FRAME_SKIP_MENU_HANDLER(10)

#undef FRAME_SKIP_MENU_HANDLER

static int machine_frame_skip_menu_custom (void)
{
   int frames;

   frames = frame_skip;

   if (get_integer_input ("Custom", &frames, "frames"))
   {
      frame_skip = frames;
      update_menus ();
   
      message_local ("Frame skip set to %d frames.", frames);
   }

   return (D_O_K);
}

static int machine_menu_soft_reset (void)
{
   machine_reset ();

   /* Clear the game clock. */
   options_menu_reset_clock ();

   return (D_CLOSE);
}

static int machine_menu_hard_reset (void)
{
   machine_exit ();
   machine_init ();

   /* Clear the game clock. */
   options_menu_reset_clock ();

   return (D_CLOSE);
}

static int machine_menu_timing_smoothest (void)
{
   machine_timing = MACHINE_TIMING_SMOOTH;
   timing_update_speed ();
   update_menus ();

   message_local ("Machine timing mode set to smoothest.");

   return (D_O_K);
}

static int machine_menu_timing_most_accurate (void)
{
   machine_timing = MACHINE_TIMING_ACCURATE;
   timing_update_speed ();
   update_menus ();

   message_local ("Machine timing mode set to most accurate.");

   return (D_O_K);
}

static int machine_menu_speed_cap (void)
{
   speed_cap = !speed_cap;
   update_menus ();

   message_local ("Speed cap %s.", get_enabled_text (speed_cap));

   return (D_O_K);
}

static int machine_menu_cheat_manager (void)
{
   if (show_dialog (machine_cheat_manager_dialog, -1) ==
      MACHINE_CHEAT_MANAGER_DIALOG_SAVE_BUTTON)
   {
      save_patches ();
   }

   return (D_O_K);
}

static int audio_menu_enable_apu (void)
{
   apu_options.enabled = !apu_options.enabled;
   update_menus ();

   apu_update ();

   message_local ("APU emulation %s.",
      get_enabled_text (apu_options.enabled));

   return (D_O_K);
}

static int audio_menu_enable_output (void)
{
   audio_enable_output = !audio_enable_output;
   update_menus ();

   cycle_audio ();

   message_local ("Audio output %s.",
      get_enabled_text (audio_enable_output));

   return (D_O_K);
}

static int audio_menu_emulation_fast (void)
{
   apu_options.emulation = APU_EMULATION_FAST;
   update_menus ();

   apu_update ();

   message_local ("APU emulation quality set to fast.");

   return (D_O_K);
}

static int audio_menu_emulation_accurate (void)
{
   apu_options.emulation = APU_EMULATION_ACCURATE;
   update_menus ();

   apu_update ();

   message_local ("APU emulation quality set to accurate.");

   return (D_O_K);
}

static int audio_menu_emulation_ultra (void)
{
   apu_options.emulation = APU_EMULATION_ULTRA;
   update_menus ();

   apu_update ();

   message_local ("APU emulation quality set to ultra.");

   return (D_O_K);
}

#define AUDIO_CHANNELS_MENU_HANDLER(id, name)  \
   static int audio_channels_menu_##id (void) \
   { \
      BOOL *enabled = &apu_options.enable_##id ; \
      *enabled = !*enabled; \
      update_menus (); \
      apu_update (); \
      message_local ("APU " name " %s.", get_enabled_text (*enabled)); \
      return (D_O_K); \
   }

AUDIO_CHANNELS_MENU_HANDLER(square_1, "first square wave channel")
AUDIO_CHANNELS_MENU_HANDLER(square_2, "second square wave channel")
AUDIO_CHANNELS_MENU_HANDLER(triangle, "triangle wave channel")
AUDIO_CHANNELS_MENU_HANDLER(noise,    "noise channel")
AUDIO_CHANNELS_MENU_HANDLER(dmc,      "delta modulation channel")
AUDIO_CHANNELS_MENU_HANDLER(extra_1,  "first expansion channel")
AUDIO_CHANNELS_MENU_HANDLER(extra_2,  "second expansion channel")
AUDIO_CHANNELS_MENU_HANDLER(extra_3,  "third expansion channel")

#undef AUDIO_CHANNELS_MENU_HANDLER

static int audio_channels_menu_enable_all (void)
{               
   apu_options.enable_square_1 = TRUE;
   apu_options.enable_square_2 = TRUE;
   apu_options.enable_triangle = TRUE;
   apu_options.enable_noise    = TRUE;
   apu_options.enable_dmc      = TRUE;
   apu_options.enable_extra_1  = TRUE;
   apu_options.enable_extra_2  = TRUE;
   apu_options.enable_extra_3  = TRUE;

   update_menus ();

   apu_update ();

   message_local ("All APU channels enabled.");

   return (D_O_K);
}

static int audio_channels_menu_disable_all (void)
{
   apu_options.enable_square_1 = FALSE;
   apu_options.enable_square_2 = FALSE;
   apu_options.enable_triangle = FALSE;
   apu_options.enable_noise    = FALSE;
   apu_options.enable_dmc      = FALSE;
   apu_options.enable_extra_1  = FALSE;
   apu_options.enable_extra_2  = FALSE;
   apu_options.enable_extra_3  = FALSE;

   update_menus ();

   apu_update ();

   message_local ("All APU channels disabled.");

   return (D_O_K);
}

static int audio_output_menu_subsystem_allegro (void)
{
   audio_subsystem = AUDIO_SUBSYSTEM_ALLEGRO;
   update_menus ();

   cycle_audio ();

   message_local ("Audio subsystem set to Allegro.");

   return (D_O_K);
}

static int audio_output_menu_subsystem_openal (void)
{
   audio_subsystem = AUDIO_SUBSYSTEM_OPENAL;
   update_menus ();

   cycle_audio ();

   message_local ("Audio subsystem set to OpenAL.");

   return (D_O_K);
}

#define AUDIO_OUTPUT_MENU_SAMPLING_RATE_HANDLER(rate)  \
   static int audio_output_menu_sampling_rate_##rate##_hz (void) \
   {  \
      audio_sample_rate = rate;  \
      update_menus ();  \
      cycle_audio ();   \
      message_local ("Audio sampling rate set to %d Hz.", rate);  \
      return (D_O_K);   \
   }

AUDIO_OUTPUT_MENU_SAMPLING_RATE_HANDLER(22050)
AUDIO_OUTPUT_MENU_SAMPLING_RATE_HANDLER(44100)
AUDIO_OUTPUT_MENU_SAMPLING_RATE_HANDLER(48000)

#undef AUDIO_OUTPUT_MENU_SAMPLING_RATE_HANDLER

static int audio_output_menu_sampling_rate_custom (void)
{
   int rate;

   rate = audio_sample_rate;

   if (get_integer_input ("Custom", &rate, "Hz"))
   {
      audio_sample_rate = rate;
      update_menus ();

      cycle_audio ();

      message_local ("Audio sampling rate set to %d Hz.", rate);
   }

   return (D_O_K);
}

static int audio_output_menu_mixing_mono (void)
{
   apu_options.stereo = FALSE;
   update_menus ();

   cycle_audio ();

   gui_message (GUI_TEXT_COLOR, "Audio output set to mono.");

   return (D_O_K);
}

static int audio_output_menu_mixing_stereo (void)
{
   apu_options.stereo = TRUE;

   dsp_set_effector_enabled (DSP_EFFECTOR_SWAP_CHANNELS,
      DSP_SET_ENABLED_MODE_SET, FALSE);

   update_menus ();
    
   cycle_audio ();
   
   message_local ("Audio output set to stereo.");

   return (D_O_K);
}

static int audio_output_menu_mixing_stereo_reversed (void)
{
   apu_options.stereo = TRUE;

   dsp_set_effector_enabled (DSP_EFFECTOR_SWAP_CHANNELS,
      DSP_SET_ENABLED_MODE_SET, TRUE);

   update_menus ();
    
   cycle_audio ();
   
   message_local ("Audio output set to reverse stereo.");

   return (D_O_K);
}

#define AUDIO_OUTPUT_BUFFER_MENU_HANDLER(size, units)   \
   static int audio_output_buffer_menu_##size##_##units (void)   \
   {  \
      audio_buffer_length = size; \
      update_menus ();  \
      cycle_audio ();   \
      message_local ("Audio buffer size set to %d %s.", size,  \
         "##units##");  \
      return (D_O_K);   \
   }
                        
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(1, frame)
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(2, frames)
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(3, frames)
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(4, frames)
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(5, frames)
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(6, frames)
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(7, frames)
AUDIO_OUTPUT_BUFFER_MENU_HANDLER(8, frames)

#undef AUDIO_OUTPUT_BUFFER_MENU_HANDLER

static int audio_output_buffer_menu_custom (void) 
{
   int frames;

   frames = audio_buffer_length;

   if (get_integer_input ("Custom", &frames, "frames"))
   {
      audio_buffer_length = frames;
      update_menus ();

      cycle_audio ();

      message_local ("Audio buffer size set to %d frames.", frames);
   }

   return (D_O_K);
}

static int audio_menu_volume_increase (void)
{
   dsp_master_volume += 0.25;
   if (dsp_master_volume > 4.0)
      dsp_master_volume = 4.0;

   update_menus ();

   message_local ("Audio master volume level increased to %d%%.",
      (int)ROUND(dsp_master_volume * 100.0));

   return (D_O_K);
}

static int audio_menu_volume_decrease (void)
{
   dsp_master_volume -= 0.25;
   if (dsp_master_volume < 0)
      dsp_master_volume = 0;

   update_menus ();

   message_local ("Audio master volume level decreased to %d%%.",
      (int)ROUND(dsp_master_volume * 100.0));

   return (D_O_K);
}

static int audio_menu_volume_custom (void)
{
   int percent;

   percent = ROUND(dsp_master_volume * 100.0);

   if (get_integer_input ("Custom", &percent, "percent"))
   {
      dsp_master_volume = (percent / 100.0);
      update_menus ();
   
      message_local ("Audio master volume level set to %d%%.", percent);
   }

   return (D_O_K);
}

static int audio_menu_volume_reset (void)
{
   dsp_master_volume = 1.0;
   update_menus ();

   message_local ("Audio master volume level reset to %d%%.",
      (int)ROUND(dsp_master_volume * 100.0));

   return (D_O_K);
}

static int audio_record_menu_start (void)
{
   int index;

   for (index = 0; index < 999; index++)
   {
      USTRING filename;

      uszprintf (filename, sizeof (filename), "%s_%03d.wav", get_filename
         (global_rom.filename), index);

      /* Merge it with our save path. */
      get_save_path (filename, sizeof (filename));

      if (exists (filename))
         continue;

      if (dsp_open_wav (filename, audio_sample_rate,
            (apu_options.stereo ? 2 : 1), audio_sample_size) == 0)
      {
         DISABLE_MENU_ITEM(audio_record_menu_start);
         ENABLE_MENU_ITEM(audio_record_menu_stop);
      }
   
      message_local ("Audio WAV recording started to %s.", filename);

      return (D_O_K);
   }
   
   gui_message (GUI_ERROR_COLOR, "Couldn't find a suitable image "
      "filename.");

   return (D_O_K);
}

static int audio_record_menu_stop (void)
{
   dsp_close_wav ();

   ENABLE_MENU_ITEM(audio_record_menu_start);
   DISABLE_MENU_ITEM(audio_record_menu_stop);

   message_local ("Audio WAV recording stopped.");

   return (D_O_K);
}

static int video_menu_fullscreen (void)
{
   video_force_fullscreen = !video_force_fullscreen;
   video_reinit ();

   gui_needs_restart = TRUE;
   return (D_CLOSE);
}

static int video_menu_page_buffer (void)
{
   video_enable_page_buffer = !video_enable_page_buffer;
   update_menus ();

   video_reinit ();

   cycle_video ();

   message_local ("Page buffering %s.", get_enabled_text
      (video_enable_page_buffer));

   return (D_O_K);
}

static int video_menu_vsync (void)
{
   video_enable_vsync = !video_enable_vsync;
   update_menus ();

   message_local ("VSync %s.", get_enabled_text (video_enable_vsync));

   return (D_O_K);
}

static int video_menu_color (void)
{
   DIALOG *dialog;
   DIALOG *objhue, *objsat, *objbright, *objcon, *objgamma;
   int result;

   /* Get dialog. */
   dialog = video_color_dialog;
   
   /* Get slider objects. */
   
   objhue    = &dialog[VIDEO_COLOR_DIALOG_HUE];
   objsat    = &dialog[VIDEO_COLOR_DIALOG_SATURATION];
   objbright = &dialog[VIDEO_COLOR_DIALOG_BRIGHTNESS];
   objcon    = &dialog[VIDEO_COLOR_DIALOG_CONTRAST];
   objgamma  = &dialog[VIDEO_COLOR_DIALOG_GAMMA];
   
   /* Load configuration. */
   
   objhue->d2    = (get_config_int ("video", "hue",        0) + 100);
   objsat->d2    = (get_config_int ("video", "saturation", 0) + 100);
   objbright->d2 = (get_config_int ("video", "brightness", 0) + 100);
   objcon->d2    = (get_config_int ("video", "contrast",   0) + 100);
   objgamma->d2  = (get_config_int ("video", "gamma",      0) + 100);
   
   /* Show dialog. */
   result = show_dialog (dialog, -1);

   if (result == VIDEO_COLOR_DIALOG_SAVE_BUTTON)
   {
      /* Save configuration. */
   
      set_config_int ("video", "hue",        (objhue->d2    - 100));
      set_config_int ("video", "saturation", (objsat->d2    - 100));
      set_config_int ("video", "brightness", (objbright->d2 - 100));
      set_config_int ("video", "contrast",   (objcon->d2    - 100));
      set_config_int ("video", "gamma",      (objgamma->d2  - 100));
   
      /* Reinitialize palette to the load new configuration. */
      video_set_palette (NULL);
   
      /* Display changes. */
      cycle_video ();
   }
   else if (result == VIDEO_COLOR_DIALOG_RESET_BUTTON)
   {
      /* Save defaults. */
   
      set_config_int ("video", "hue",        0);
      set_config_int ("video", "saturation", 0);
      set_config_int ("video", "brightness", 0);
      set_config_int ("video", "contrast",   0);
      set_config_int ("video", "gamma",      0);
   
      /* Reinitialize palette to the load new configuration. */
      video_set_palette (NULL);
   
      /* Display changes. */
      cycle_video ();
   }

   return (D_O_K);
}

#define DRIVER_MENU_HANDLER(driver, id)   \
   static int video_driver_menu_##driver (void) \
   {  \
      video_set_driver (id);  \
      gui_needs_restart = TRUE;  \
      return (D_CLOSE); \
   }

#define DRIVER_MENU_HANDLER_EX(system, driver, id) \
   static int video_driver_##system##_menu_##driver (void)   \
   {  \
      video_set_driver (id);  \
      gui_needs_restart = TRUE;  \
      return (D_CLOSE); \
   }

DRIVER_MENU_HANDLER(automatic, GFX_AUTODETECT)

#ifdef ALLEGRO_DOS

DRIVER_MENU_HANDLER_EX(dos, vga,           GFX_VGA)
DRIVER_MENU_HANDLER_EX(dos, vga_mode_x,    GFX_MODEX)
DRIVER_MENU_HANDLER_EX(dos, vesa,          GFX_VESA1)
DRIVER_MENU_HANDLER_EX(dos, vesa_2_banked, GFX_VESA2B)
DRIVER_MENU_HANDLER_EX(dos, vesa_2_linear, GFX_VESA2L)
DRIVER_MENU_HANDLER_EX(dos, vesa_3,        GFX_VESA3)
DRIVER_MENU_HANDLER_EX(dos, vesa_vbe_af,   GFX_VBEAF)

#endif   /* ALLEGRO_DOS */

#ifdef ALLEGRO_WINDOWS

DRIVER_MENU_HANDLER_EX(windows, directx,         GFX_DIRECTX)
DRIVER_MENU_HANDLER_EX(windows, directx_window,  GFX_DIRECTX_WIN)
DRIVER_MENU_HANDLER_EX(windows, directx_overlay, GFX_DIRECTX_OVL)
DRIVER_MENU_HANDLER_EX(windows, gdi,             GFX_GDI)
                   
#endif   /* ALLEGRO_WINDOWS */

#ifdef ALLEGRO_LINUX

DRIVER_MENU_HANDLER_EX(linux, vga,         GFX_VGA)
DRIVER_MENU_HANDLER_EX(linux, vga_mode_x,  GFX_MODEX)
DRIVER_MENU_HANDLER_EX(linux, vesa_vbe_af, GFX_VBEAF)
#ifdef GFX_FBCON   
DRIVER_MENU_HANDLER_EX(linux, framebuffer, GFX_FBCON)
#else              
DRIVER_MENU_HANDLER_EX(linux, framebuffer, NULL)
#endif
#ifdef GFX_SVGALIB
DRIVER_MENU_HANDLER_EX(linux, svgalib,     GFX_SVGALIB)
#else
DRIVER_MENU_HANDLER_EX(linux, svgalib,     NULL)
#endif

#endif   /* ALLEGRO_LINUX */

#ifdef ALLEGRO_UNIX

DRIVER_MENU_HANDLER_EX(unix, x_windows,      GFX_XWINDOWS)
DRIVER_MENU_HANDLER_EX(unix, x_windows_full, GFX_XWINDOWS_FULLSCREEN)
DRIVER_MENU_HANDLER_EX(unix, x_dga,          GFX_XDGA)
DRIVER_MENU_HANDLER_EX(unix, x_dga_full,     GFX_XDGA_FULLSCREEN)
DRIVER_MENU_HANDLER_EX(unix, x_dga_2,        GFX_XDGA2)

#endif   /* ALLEGRO_UNIX */

#ifdef USE_ALLEGROGL

DRIVER_MENU_HANDLER(opengl,      GFX_OPENGL)
DRIVER_MENU_HANDLER(opengl_full, GFX_OPENGL_FULLSCREEN)
DRIVER_MENU_HANDLER(opengl_win,  GFX_OPENGL_WINDOWED)

#endif   /* USE_ALLEGROGL */

#undef DRIVER_MENU_HANDLER
#undef DRIVER_MENU_HANDLER_EX

#define RESOLUTION_MENU_HANDLER(width, height)  \
   static int video_resolution_menu_##width##_##height (void)   \
   {  \
      video_set_resolution (width, height);  \
      gui_needs_restart = TRUE;  \
      return (D_CLOSE); \
   }

#define RESOLUTION_MENU_HANDLER_EX(type, width, height)  \
   static int video_resolution_##type##_menu_##width##_##height (void)   \
   {  \
      video_set_resolution (width, height);  \
      gui_needs_restart = TRUE;  \
      return (D_CLOSE); \
   }

RESOLUTION_MENU_HANDLER_EX(proportionate, 256,  224)
RESOLUTION_MENU_HANDLER_EX(proportionate, 256,  240)
RESOLUTION_MENU_HANDLER_EX(proportionate, 512,  448)
RESOLUTION_MENU_HANDLER_EX(proportionate, 512,  480)
RESOLUTION_MENU_HANDLER_EX(proportionate, 768,  672)
RESOLUTION_MENU_HANDLER_EX(proportionate, 768,  720)
RESOLUTION_MENU_HANDLER_EX(proportionate, 1024, 896)
RESOLUTION_MENU_HANDLER_EX(proportionate, 1024, 960)
RESOLUTION_MENU_HANDLER_EX(proportionate, 1280, 1120)
RESOLUTION_MENU_HANDLER_EX(proportionate, 1280, 1200)

RESOLUTION_MENU_HANDLER(320,  240)
RESOLUTION_MENU_HANDLER(400,  300)
RESOLUTION_MENU_HANDLER(640,  480)
RESOLUTION_MENU_HANDLER(800,  600)
RESOLUTION_MENU_HANDLER(1024, 768)
RESOLUTION_MENU_HANDLER(1152, 864)
RESOLUTION_MENU_HANDLER(1280, 960)
RESOLUTION_MENU_HANDLER(1280, 1024)
RESOLUTION_MENU_HANDLER(1600, 1200)

#undef RESOLUTION_MENU_HANDLER
#undef RESOLUTION_MENU_HANDLER_EX

static int video_resolution_menu_custom (void)
{
   int width, height;

   width  = SCREEN_W;
   height = SCREEN_H;

   if (get_resolution_input ("Custom", &width, &height))
   {
      video_set_resolution (width, height);

      gui_needs_restart = TRUE;
      return (D_CLOSE);
   }
      
   return (D_O_K);
}

static int video_color_depth_menu_paletted_8_bit (void)
{
   video_set_color_depth (8);

   gui_needs_restart = TRUE;
   return (D_CLOSE);
}

static int video_color_depth_menu_true_color_15_bit (void)
{
   video_set_color_depth (15);

   gui_needs_restart = TRUE;
   return (D_CLOSE);
}

static int video_color_depth_menu_true_color_16_bit (void)
{
   video_set_color_depth (16);

   gui_needs_restart = TRUE;
   return (D_CLOSE);
}

static int video_color_depth_menu_true_color_24_bit (void)
{
   video_set_color_depth (24);

   gui_needs_restart = TRUE;
   return (D_CLOSE);
}

static int video_color_depth_menu_true_color_32_bit (void)
{
   video_set_color_depth (32);

   gui_needs_restart = TRUE;
   return (D_CLOSE);
}

static int video_buffer_menu_match_resolution (void)
{
   video_buffer_width  =
   video_buffer_height = -1;
   video_init_buffer ();

   update_menus ();
   cycle_video ();

   return (D_O_K);
}

#define BUFFER_MENU_HANDLER(width, height)  \
   static int video_buffer_menu_##width##_##height (void)   \
   {  \
      video_buffer_width = width;   \
      video_buffer_height = height; \
      video_init_buffer ();   \
      update_menus ();  \
      cycle_video ();   \
      return (D_O_K);   \
   }

BUFFER_MENU_HANDLER(256, 240)
BUFFER_MENU_HANDLER(320, 240)
BUFFER_MENU_HANDLER(512, 480)
BUFFER_MENU_HANDLER(640, 480)
BUFFER_MENU_HANDLER(256, 256)
BUFFER_MENU_HANDLER(512, 512)

#undef BUFFER_MENU_HANDLER

static int video_buffer_menu_custom (void)
{
   int width, height;

   width  = video_buffer_width;
   height = video_buffer_height;

   if (get_resolution_input ("Custom", &width, &height))
   {
      video_buffer_width = width;
      video_buffer_height = height;
      video_init_buffer ();

      update_menus ();

      cycle_video ();
   }
      
   return (D_O_K);
}

#define BLITTER_MENU_HANDLER(name, caption, id) \
   static int video_blitter_menu_##name (void)   \
   {  \
      video_set_blitter (id); \
      update_menus ();  \
      cycle_video ();   \
      message_local ("Video blitter set to %s.", caption);  \
      return (D_O_K);   \
   }

BLITTER_MENU_HANDLER(automatic,          "automatic",            VIDEO_BLITTER_AUTOMATIC)
BLITTER_MENU_HANDLER(normal,             "normal",               VIDEO_BLITTER_NORMAL)
BLITTER_MENU_HANDLER(des,                "des engine",           VIDEO_BLITTER_DES)
BLITTER_MENU_HANDLER(interpolated_2x,    "interpolated (2x)",    VIDEO_BLITTER_INTERPOLATED_2X)
BLITTER_MENU_HANDLER(interpolated_2x_hq, "interpolated (2x HQ)", VIDEO_BLITTER_INTERPOLATED_2X_HQ)
BLITTER_MENU_HANDLER(2xscl,              "2xSCL engine",         VIDEO_BLITTER_2XSCL)
BLITTER_MENU_HANDLER(desii,              "des 2 engine",         VIDEO_BLITTER_DESII)
BLITTER_MENU_HANDLER(super_2xscl,        "super 2xSCL engine",   VIDEO_BLITTER_SUPER_2XSCL)
BLITTER_MENU_HANDLER(ultra_2xscl,        "ultra 2xSCL engine",   VIDEO_BLITTER_ULTRA_2XSCL)
BLITTER_MENU_HANDLER(hq2x,               "hq2x filter",          VIDEO_BLITTER_HQ2X)
BLITTER_MENU_HANDLER(ntsc,               "ntsc engine",          VIDEO_BLITTER_NTSC)
BLITTER_MENU_HANDLER(interpolated_3x,    "interpolated (3x)",    VIDEO_BLITTER_INTERPOLATED_3X)
BLITTER_MENU_HANDLER(hq3x,               "hq3x filter",          VIDEO_BLITTER_HQ3X)
BLITTER_MENU_HANDLER(hq4x,               "hq4x filter",          VIDEO_BLITTER_HQ4X)
BLITTER_MENU_HANDLER(stretched,          "stretched",            VIDEO_BLITTER_STRETCHED)

#undef BLITTER_MENU_HANDLER

static int video_blitter_menu_configure (void)
{
   switch (video_get_blitter ())
   {
      case VIDEO_BLITTER_NTSC:
      {
         DIALOG *dialog;
         DIALOG *objhue, *objsat, *objhuew, *objbright, *objcon, *objgamma,
            *objsharp, *objres, *objart, *objbleed, *objfring, *objmerge,
            *objdbl[3], *objinterp;
         int merge_fields, doubling, interpolated;
         int result;
         int index;

         /* Create dialog. */

         dialog = create_dialog (ntsc_config_dialog_base, "ntsc");
         if (!dialog)
            break;

         /* Get slider objects. */

         objhue     = &dialog[NTSC_CONFIG_DIALOG_HUE];
         objhuew    = &dialog[NTSC_CONFIG_DIALOG_HUE_WARPING];
         objsat     = &dialog[NTSC_CONFIG_DIALOG_SATURATION];
         objbright  = &dialog[NTSC_CONFIG_DIALOG_BRIGHTNESS];
         objcon     = &dialog[NTSC_CONFIG_DIALOG_CONTRAST];
         objgamma   = &dialog[NTSC_CONFIG_DIALOG_GAMMA];
         objsharp   = &dialog[NTSC_CONFIG_DIALOG_SHARPNESS];
         objres     = &dialog[NTSC_CONFIG_DIALOG_RESOLUTION];
         objart     = &dialog[NTSC_CONFIG_DIALOG_ARTIFACTS];
         objbleed   = &dialog[NTSC_CONFIG_DIALOG_COLOR_BLEED];
         objfring   = &dialog[NTSC_CONFIG_DIALOG_COLOR_FRINGING];
         objmerge   = &dialog[NTSC_CONFIG_DIALOG_REDUCE_FLICKER];
         objdbl[0]  = &dialog[NTSC_CONFIG_DIALOG_SCANLINE_DOUBLING_NORMAL];
         objdbl[1]  = &dialog[NTSC_CONFIG_DIALOG_SCANLINE_DOUBLING_BRIGHTEN];
         objdbl[2]  = &dialog[NTSC_CONFIG_DIALOG_SCANLINE_DOUBLING_DARKEN];
         objinterp  = &dialog[NTSC_CONFIG_DIALOG_INTERPOLATED];

         /* Load configuration. */

         objhue->d2    = (get_config_int ("ntsc", "hue",         0) + 100);
         objhuew->d2   = (get_config_int ("ntsc", "hue_warping", 0) + 100);
         objsat->d2    = (get_config_int ("ntsc", "saturation",  0) + 100);
         objbright->d2 = (get_config_int ("ntsc", "brightness",  0) + 100);
         objcon->d2    = (get_config_int ("ntsc", "contrast",    0) + 100);
         objgamma->d2  = (get_config_int ("ntsc", "gamma",       0) + 100);
         objsharp->d2  = (get_config_int ("ntsc", "sharpness",   0) + 100);
         objres->d2    = (get_config_int ("ntsc", "resolution",  0) + 100);
         objart->d2    = (get_config_int ("ntsc", "artifacts",   0) + 100);
         objbleed->d2  = (get_config_int ("ntsc", "bleed",       0) + 100);
         objfring->d2  = (get_config_int ("ntsc", "fringing",    0) + 100);

         merge_fields = get_config_int ("ntsc", "merge_fields", 1);
         if (merge_fields)
            objmerge->flags |= D_SELECTED;
         
         doubling = fix (get_config_int ("ntsc", "doubling", 0), 0, 2);

         objdbl[doubling]->flags |= D_SELECTED;

         interpolated = get_config_int ("ntsc", "interpolated", 1);
         if (interpolated)
            objinterp->flags |= D_SELECTED;

         /* Show dialog. */
         result = show_dialog (dialog, -1);

         /* Destroy dialog. */
         unload_dialog (dialog);

         if (result == NTSC_CONFIG_DIALOG_SAVE_BUTTON)
         {
            /* Save configuration. */

            set_config_int ("ntsc", "hue",         (objhue->d2    - 100));
            set_config_int ("ntsc", "hue_warping", (objhuew->d2   - 100));
            set_config_int ("ntsc", "saturation",  (objsat->d2    - 100));
            set_config_int ("ntsc", "brightness",  (objbright->d2 - 100));
            set_config_int ("ntsc", "contrast",    (objcon->d2    - 100));
            set_config_int ("ntsc", "gamma",       (objgamma->d2  - 100));
            set_config_int ("ntsc", "sharpness",   (objsharp->d2  - 100));
            set_config_int ("ntsc", "resolution",  (objres->d2    - 100));
            set_config_int ("ntsc", "artifacts",   (objart->d2    - 100));
            set_config_int ("ntsc", "bleed",       (objbleed->d2  - 100));
            set_config_int ("ntsc", "fringing",    (objfring->d2  - 100));

            merge_fields = ((objmerge->flags & D_SELECTED) ? 1 : 0);

            set_config_int ("ntsc", "merge_fields", merge_fields);

            for (index = 0; index < 3; index++)
            {
               if (objdbl[index]->flags & D_SELECTED)
                  doubling = index;
            }

            set_config_int ("ntsc", "doubling", doubling);

            interpolated = ((objinterp->flags & D_SELECTED) ? 1 : 0);

            set_config_int ("ntsc", "interpolated", interpolated);

            /* Reinitialize blitter to the load new configuration. */
            video_blitter_reinit ();

            /* Display changes. */
            cycle_video ();
         }
         else if (result == NTSC_CONFIG_DIALOG_SET_BUTTON)
         {
            DIALOG *objpres[5];
            int preset = -1;

            /* Set a preset. */

            objpres[0] = &dialog[NTSC_CONFIG_DIALOG_PRESETS_DEFAULT];
            objpres[1] = &dialog[NTSC_CONFIG_DIALOG_PRESETS_COMPOSITE];
            objpres[2] = &dialog[NTSC_CONFIG_DIALOG_PRESETS_SVIDEO];
            objpres[3] = &dialog[NTSC_CONFIG_DIALOG_PRESETS_RGB];
            objpres[4] = &dialog[NTSC_CONFIG_DIALOG_PRESETS_MONOCHROME];

            for (index = 0; index < 5; index++)
            {
               if (objpres[index]->flags & D_SELECTED)
                  preset = index;
            }

            set_config_int ("ntsc", "preset", preset);

            /* Reinitialize blitter to the load new configuration. */
            video_blitter_reinit ();

            /* Display changes. */
            cycle_video ();
         }

         break;
      }

      case VIDEO_BLITTER_STRETCHED:
      {
         int width, height;

         /* Load configuration. */

         width  = get_config_int ("video", "stretch_width",  512);
         height = get_config_int ("video", "stretch_height", 480);

         if (get_resolution_input ("Stretched", &width, &height))
         {
            /* Save configuration. */

            set_config_int ("video", "stretch_width",  width);
            set_config_int ("video", "stretch_height", height);

            /* Reinitialize blitter to the load new configuration. */
            video_blitter_reinit ();

            /* Display changes. */
            cycle_video ();
         }

         break;
      }

      default:
      {
         gui_alert ("Error", "There are no configuration parameters "
            "available for the selected blitter.", NULL, NULL, "&OK", NULL,
               'o', 0);

         break;
      }
   }

   return (D_O_K);
}

static int video_filters_menu_scanlines_25_percent (void)
{
   LIST filters;

   filters = video_get_filter_list ();
   LIST_TOGGLE(filters, VIDEO_FILTER_SCANLINES_LOW);
   video_set_filter_list (filters);

   update_menus ();

   cycle_video ();

   message_local ("Scanlines video filter %s.", get_enabled_text_ex
      ((filters & VIDEO_FILTER_SCANLINES_LOW), "enabled (25%)"));

   return (D_O_K);
}

static int video_filters_menu_scanlines_50_percent (void)
{
   LIST filters;

   filters = video_get_filter_list ();
   LIST_TOGGLE(filters, VIDEO_FILTER_SCANLINES_MEDIUM);
   video_set_filter_list (filters);

   update_menus ();

   cycle_video ();

   message_local ("Scanlines video filter %s.", get_enabled_text_ex
      ((filters & VIDEO_FILTER_SCANLINES_MEDIUM), "enabled (50%)"));

   return (D_O_K);
}

static int video_filters_menu_scanlines_100_percent (void)
{
   LIST filters;

   filters = video_get_filter_list ();
   LIST_TOGGLE(filters, VIDEO_FILTER_SCANLINES_HIGH);
   video_set_filter_list (filters);

   update_menus ();

   cycle_video ();

   message_local ("Scanlines video filter %s.", get_enabled_text_ex
      ((filters & VIDEO_FILTER_SCANLINES_HIGH), "enabled (100%)"));

   return (D_O_K);
}

static int video_layers_menu_sprites_a (void)
{
   ppu_enable_sprite_layer_a = !ppu_enable_sprite_layer_a;
   update_menus ();

   message_local ("Video sprites layer A %s.", get_enabled_text
      (ppu_enable_sprite_layer_a));

   return (D_O_K);
}

static int video_layers_menu_sprites_b (void)
{
   ppu_enable_sprite_layer_b = !ppu_enable_sprite_layer_b;
   update_menus ();

   message_local ("Video sprites layer B %s.", get_enabled_text
      (ppu_enable_sprite_layer_b));

   return (D_O_K);
}


static int video_layers_menu_background (void)
{
   ppu_enable_background_layer = !ppu_enable_background_layer;
   update_menus ();

   message_local ("Video background layer %s.", get_enabled_text
      (ppu_enable_background_layer));

   return (D_O_K);
}

static int video_layers_menu_hide_horizontal_scrolling (void)
{
   LIST clipping = video_edge_clipping;

   if (clipping & VIDEO_EDGE_CLIPPING_HORIZONTAL)
      clipping &= ~VIDEO_EDGE_CLIPPING_HORIZONTAL;
   else
      clipping |= VIDEO_EDGE_CLIPPING_HORIZONTAL;

   video_edge_clipping = clipping;

   update_menus ();

   message_local ("Video horizontal edge clipping %s.", get_enabled_text
      (video_edge_clipping & VIDEO_EDGE_CLIPPING_HORIZONTAL));

   return (D_O_K);
}

static int video_layers_menu_hide_vertical_scrolling (void)
{
   LIST clipping = video_edge_clipping;

   if (clipping & VIDEO_EDGE_CLIPPING_VERTICAL)
      clipping &= ~VIDEO_EDGE_CLIPPING_VERTICAL;
   else
      clipping |= VIDEO_EDGE_CLIPPING_VERTICAL;

   video_edge_clipping = clipping;

   update_menus ();

   message_local ("Video vertical edge clipping %s.", get_enabled_text
      (video_edge_clipping & VIDEO_EDGE_CLIPPING_VERTICAL));

   return (D_O_K);
}

static int video_layers_menu_flip_mirroring (void)
{
   ppu_invert_mirroring ();

   return (D_CLOSE);
}

#define PALETTE_MENU_HANDLER(name, caption, id) \
   static int video_palette_menu_##name (void)   \
   {  \
      video_set_palette (DATA_TO_RGB(id));  \
      video_set_palette_id (DATA_INDEX(id)); \
      update_menus ();  \
      cycle_video ();   \
      message_local ("Video palette set to %s.", caption);  \
      return (D_O_K);   \
   }

#ifdef ALLEGRO_WINDOWS
/* Kludge to get around a conflict with Win32 API. */
#undef DEFAULT_PALETTE
#endif

PALETTE_MENU_HANDLER(ntsc_color,     "NTSC color",     DEFAULT_PALETTE)
PALETTE_MENU_HANDLER(ntsc_grayscale, "NTSC grayscale", GRAYSCALE_PALETTE)
PALETTE_MENU_HANDLER(gnuboy,         "gnuboy",         GNUBOY_PALETTE)
PALETTE_MENU_HANDLER(nester,         "NESter",         NESTER_PALETTE)
PALETTE_MENU_HANDLER(nesticle,       "NESticle",       NESTICLE_PALETTE)
PALETTE_MENU_HANDLER(modern_ntsc,    "modern NTSC",    MODERN_NTSC_PALETTE)
PALETTE_MENU_HANDLER(modern_pal,     "modern PAL",     MODERN_PAL_PALETTE)
PALETTE_MENU_HANDLER(ega_mode_1,     "EGA (mode 1)",   EGA_PALETTE_1)
PALETTE_MENU_HANDLER(ega_mode_2,     "EGA (mode 2)",   EGA_PALETTE_2)

#undef PALETTE_MENU_HANDLER

static int video_palette_menu_custom (void)
{
   PACKFILE *file;
   int index;

   file = pack_fopen ("fakenes.pal", "r");

   if (!file)
   {
      gui_message (GUI_ERROR_COLOR, "Error opening FAKENES.PAL!");

      return (D_O_K);
   }

   memset (custom_palette, 0, sizeof (PALETTE));

   for (index = 1; index <= 64; index++)
   {
      custom_palette[index].r = ROUND((pack_getc (file) / 4.0));
      custom_palette[index].g = ROUND((pack_getc (file) / 4.0));
      custom_palette[index].b = ROUND((pack_getc (file) / 4.0));
   }

   pack_fclose (file);

   video_set_palette (((RGB *)custom_palette));
   video_set_palette_id (-1);

   update_menus ();

   cycle_video ();

   message_local ("Video palette set to custom.");
    
   return (D_O_K);
}

static int options_menu_show_status (void)
{
   video_display_status = (! video_display_status);
   update_menus ();

   return (D_O_K);
}

static int options_menu_reset_clock (void)
{
   timing_clock = 0;

   return (D_O_K);
}

static int options_menu_paths (void)
{
   USTRING open_path, save_path;
   BOOL locked;
   DIALOG *dialog;
   DIALOG *objopen, *objlock, *objsave;

   /* Load configuration. */

   USTRING_CLEAR(open_path);
   ustrncat (open_path, get_config_string ("gui", "open_path", "/"),
      (sizeof (open_path) - 1));

   USTRING_CLEAR(save_path);
   ustrncat (save_path, get_config_string ("gui", "save_path", "./"),
      (sizeof (save_path) - 1));

   locked = get_config_int ("gui", "lock_paths", FALSE);
   
   /* Get dialog. */
   dialog = options_paths_dialog;

   /* Get dialog objects. */

   objopen = &dialog[OPTIONS_PATHS_DIALOG_OPEN_PATH];
   objlock = &dialog[OPTIONS_PATHS_DIALOG_LOCKED];
   objsave = &dialog[OPTIONS_PATHS_DIALOG_SAVE_PATH];

   /* Set up objects. */

   objopen->dp = open_path;
   objopen->d1 = ((sizeof (open_path) / MAX_UCHAR_LENGTH) - 1);

   objsave->dp = save_path;
   objsave->d1 = ((sizeof (save_path) / MAX_UCHAR_LENGTH) - 1);

   if (locked)
      objlock->flags |= D_SELECTED;

   /* Show dialog. */

   if (show_dialog (dialog, -1) == OPTIONS_PATHS_DIALOG_OK_BUTTON)
   {
      /* Save configuration. */

      set_config_string ("gui", "open_path", open_path);
      set_config_string ("gui", "save_path", save_path);

      set_config_int ("gui", "lock_paths",
         TRUE_OR_FALSE(objlock->flags & D_SELECTED));
   }

   return (D_O_K);
}


static int options_input_menu_enable_zapper (void)
{
   input_enable_zapper = !input_enable_zapper;
   update_menus ();

   message_local ("Zapper emulation %s.", get_enabled_text
      (input_enable_zapper));

   return (D_O_K);
}

static int options_input_menu_configure (void)
{
   BOOL allow_conflicts, toggled_auto, merge_players;
   REAL turbo_rate;
   DIALOG *dialog;
   DIALOG *objconf, *objauto, *objmerge, *objturbo;

   /* Load configuration. */

   allow_conflicts = get_config_int   ("input", "allow_conflicts", FALSE);
   toggled_auto    = get_config_int   ("input", "toggled_auto",    FALSE);
   merge_players   = get_config_int   ("input", "merge_players",   FALSE);
   turbo_rate      = get_config_float ("input", "turbo_rate",      0.5);

   /* Get dialog. */

   dialog = options_input_configure_dialog;

   /* Get dialog objects. */

   objconf  = &dialog[OPTIONS_INPUT_CONFIGURE_DIALOG_ALLOW_CONFLICTS];
   objauto  = &dialog[OPTIONS_INPUT_CONFIGURE_DIALOG_TOGGLED_AUTO];
   objmerge = &dialog[OPTIONS_INPUT_CONFIGURE_DIALOG_MERGE_PLAYERS];
   objturbo = &dialog[OPTIONS_INPUT_CONFIGURE_DIALOG_TURBO];

   /* Set up objects. */

   if (allow_conflicts)
      objconf->flags |= D_SELECTED;

   if (toggled_auto)
      objauto->flags |= D_SELECTED;

   if (merge_players)
      objmerge->flags |= D_SELECTED;

   objturbo->d2 = ROUND((turbo_rate * 100.0));

   /* Show dialog. */   

   if (show_dialog (dialog, -1) ==
      OPTIONS_INPUT_CONFIGURE_DIALOG_SAVE_BUTTON)
   {
      /* Save configuration. */

      allow_conflicts = TRUE_OR_FALSE(objconf->flags  & D_SELECTED);
      toggled_auto    = TRUE_OR_FALSE(objauto->flags  & D_SELECTED);
      merge_players   = TRUE_OR_FALSE(objmerge->flags & D_SELECTED);

      turbo_rate = (objturbo->d2 / 100.0);

      /* Save existing configuration so we don't lose it. */
      input_save_config ();

      /* Make any necessary changes. */

      set_config_int   ("input", "allow_conflicts", allow_conflicts);
      set_config_int   ("input", "toggled_auto",    toggled_auto);
      set_config_int   ("input", "merge_players",   merge_players);
      set_config_float ("input", "turbo_rate",      turbo_rate);

      /* Reload configuration with our changes. */
      input_load_config ();
   }

   return (D_O_K);
}

static int options_cpu_usage_menu_passive (void)
{
    cpu_usage = CPU_USAGE_PASSIVE;
    update_menus ();

    message_local ("System CPU usage set to passive.");

    return (D_O_K);
}

static int options_cpu_usage_menu_normal (void)
{
    cpu_usage = CPU_USAGE_NORMAL;
    update_menus ();

    message_local ("System CPU usage set to normal.");

    return (D_O_K);
}

static int options_cpu_usage_menu_aggressive (void)
{
    cpu_usage = CPU_USAGE_AGGRESSIVE;
    update_menus ();

    message_local ("System CPU usage set to aggressive.");

    return (D_O_K);
}

#define OPTIONS_GUI_THEME_MENU_HANDLER(name)   \
   static int options_gui_theme_menu_##name (void) \
   {  \
      set_##name##_theme ();  \
      gui_needs_restart = TRUE;  \
      return (D_CLOSE); \
   }

OPTIONS_GUI_THEME_MENU_HANDLER(classic)
OPTIONS_GUI_THEME_MENU_HANDLER(stainless_steel)
OPTIONS_GUI_THEME_MENU_HANDLER(zero_4)
OPTIONS_GUI_THEME_MENU_HANDLER(panta)
OPTIONS_GUI_THEME_MENU_HANDLER(xodiac)
OPTIONS_GUI_THEME_MENU_HANDLER(monochrome)
OPTIONS_GUI_THEME_MENU_HANDLER(essence)
OPTIONS_GUI_THEME_MENU_HANDLER(voodoo)
OPTIONS_GUI_THEME_MENU_HANDLER(hugs_and_kisses)

#undef OPTIONS_GUI_THEME_MENU_HANDLER

static int netplay_menu_start_as_server (void)
{
   STRING host;
   int port;
   STRING port_str;
   USTRING nick;
   DIALOG *dialog;
   DIALOG *obj_host_label;
   DIALOG *obj_host;
   DIALOG *obj_port;
   DIALOG *obj_nick;

   /* Load configuration. */

   STRING_CLEAR(host);

   port = get_config_int ("netplay", "port", NETPLAY_DEFAULT_PORT);

   STRING_CLEAR(port_str);
   sprintf (port_str, "%d", port);

   USTRING_CLEAR(nick);
   ustrncat (nick, get_config_string ("netplay", "nick", "Player"), (sizeof(nick) - 1));

   /* Get dialog. */
   dialog = netplay_dialog;

   /* Get dialog objects. */
   obj_host_label = &dialog[NETPLAY_DIALOG_HOST_LABEL];
   obj_host       = &dialog[NETPLAY_DIALOG_HOST];
   obj_port       = &dialog[NETPLAY_DIALOG_PORT];
   obj_nick       = &dialog[NETPLAY_DIALOG_NICK];

   /* Set up dialog objects. */

   obj_host_label->flags |= D_DISABLED;
  
   obj_host->d1 = 0;
   obj_host->dp = host;
   obj_host->flags |= D_DISABLED;

   obj_port->d1 = ((sizeof (port_str) / MAX_UCHAR_LENGTH) - 1);
   obj_port->dp = port_str;

   obj_nick->d1 = ((sizeof (nick) / MAX_UCHAR_LENGTH) - 1);
   obj_nick->dp = nick;

   /* Display dialog. */
   if (show_dialog (dialog, -1) != NETPLAY_DIALOG_OK_BUTTON)
      return (D_O_K);

   /* Integerize port. */
   port = atoi (port_str);

   /* Save configuration. */
   set_config_int    ("netplay", "port", port);
   set_config_string ("netplay", "nick", nick);

   /* Start NetPlay session. */

   if (!netplay_open_server (port))
   {
      gui_message (GUI_ERROR_COLOR, "Failed to open server!");
      return (D_O_K);
   }

   message_local ("NetPlay session opened.");

   /* Set nickname. */
   netplay_set_nickname (nick);

   /* Open lobby. */
   return (open_lobby ());
}

static int netplay_menu_start_as_client (void)
{
   STRING host;
   int port;
   STRING port_str;
   USTRING nick;
   DIALOG *dialog;
   DIALOG *obj_host_label;
   DIALOG *obj_host;
   DIALOG *obj_port;
   DIALOG *obj_nick;
          
   /* Load configuration. */

   STRING_CLEAR(host);
   strncat (host, get_config_string ("netplay", "host", ""), (sizeof(host) - 1));

   port = get_config_int ("netplay", "port", NETPLAY_DEFAULT_PORT);

   STRING_CLEAR(port_str);
   sprintf (port_str, "%d", port);

   USTRING_CLEAR(nick);
   ustrncat (nick, get_config_string ("netplay", "nick", "Player"), (sizeof(nick) - 1));

   /* Get dialog. */
   dialog = netplay_dialog;

   /* Get dialog objects. */
   obj_host_label = &dialog[NETPLAY_DIALOG_HOST_LABEL];
   obj_host       = &dialog[NETPLAY_DIALOG_HOST];
   obj_port       = &dialog[NETPLAY_DIALOG_PORT];
   obj_nick       = &dialog[NETPLAY_DIALOG_NICK];

   /* Set up dialog objects. */

   obj_host_label->flags &= ~D_DISABLED;

   obj_host->d1 = ((sizeof (host) / MAX_UCHAR_LENGTH) - 1);
   obj_host->dp = host;
   obj_host->flags &= ~D_DISABLED;

   obj_port->d1 = ((sizeof (port_str) / MAX_UCHAR_LENGTH) - 1);
   obj_port->dp = port_str;

   obj_nick->d1 = ((sizeof (nick) / MAX_UCHAR_LENGTH) - 1);
   obj_nick->dp = nick;

   /* Display dialog. */
   if (show_dialog (dialog, -1) != NETPLAY_DIALOG_OK_BUTTON)
      return (D_O_K);

   /* Integerize port. */
   port = atoi (port_str);

   /* Save configuration. */
   set_config_string ("netplay", "host", host);
   set_config_int    ("netplay", "port", port);
   set_config_string ("netplay", "nick", nick);

   /* Start NetPlay session. */

   if (!netplay_open_client (host, port))
   {
      gui_message (GUI_ERROR_COLOR, "Failed to connect to remote host!");
      return (D_O_K);
   }

   message_local ("NetPlay session opened.");

   /* Set nickname. */
   netplay_set_nickname (nick);

   /* Open lobby. */
   return (open_lobby ());
}

static int help_menu_shortcuts (void)
{
   show_dialog (help_shortcuts_dialog, -1);

   return (D_O_K);
}

static int help_menu_about (void)
{
   show_dialog (help_about_dialog, -1);

   return (D_O_K);
}

static int help_menu_version (void)
{
   gui_alert ("Version", "FakeNES version " VERSION_STRING " "
      ALLEGRO_PLATFORM_STR, "Get the latest from "
         "http://fakenes.sourceforge.net/.", NULL, "&OK", NULL, 'o', 0);

   return (D_O_K);
}


/* ---- Dialog handlers. ---- */


static int machine_cheat_manager_dialog_list (DIALOG *dialog)
{
   CPU_PATCH *patch;
   DIALOG *main_dialog;
   DIALOG *obj_enabled;

   RT_ASSERT(dialog);

   if (cpu_patch_count == 0)
      return (D_O_K);

   patch = &cpu_patch_info[dialog->d1];

   /* Get main dialog. */
   main_dialog = machine_cheat_manager_dialog;

   /* Get "Enabled" checkbox. */
   obj_enabled = &main_dialog[MACHINE_CHEAT_MANAGER_DIALOG_ENABLED_CHECKBOX];

   if (patch->enabled)
      obj_enabled->flags |= D_SELECTED;
   else
      obj_enabled->flags &= ~D_SELECTED;

   scare_mouse ();
   object_message (obj_enabled, MSG_DRAW, 0);
   unscare_mouse ();

   return (D_O_K);
}

static int machine_cheat_manager_dialog_add (DIALOG *dialog)
{
   DIALOG *main_dialog;
   DIALOG *obj_title;
   DIALOG *obj_code;
   USTRING title;
   USTRING code;
   CPU_PATCH *patch;
   UINT8 value;

   if (cpu_patch_count >= CPU_MAX_PATCHES)
   {
      gui_alert ("Error", "The patch list is already full.", NULL, NULL,
         "&OK", NULL, 'o', 0);

      return (D_O_K);
   }

   /* Get dialog. */
   main_dialog = machine_cheat_manager_add_dialog;

   /* Get dialog objects. */
   obj_title = &main_dialog[MACHINE_CHEAT_MANAGER_ADD_DIALOG_TITLE];
   obj_code  = &main_dialog[MACHINE_CHEAT_MANAGER_ADD_DIALOG_CODE];

   /* Set up dialog objects. */

   USTRING_CLEAR(title);
   obj_title->d1 = NEW_SAVE_TITLE_SIZE;
   obj_title->dp = title;

   USTRING_CLEAR(code);
   obj_code->d1 = (11 - 1);
   obj_code->dp = code;

   /* Show dialog. */
   if (show_dialog (main_dialog, -1) !=
      MACHINE_CHEAT_MANAGER_ADD_DIALOG_OK_BUTTON)
   {
      return (D_O_K);
   }

   patch = &cpu_patch_info[cpu_patch_count];

   if (cheats_decode (code, &patch->address, &patch->value,
      &patch->match_value) != 0)
   {
      gui_alert ("Error", "You must enter a valid Game Genie (or NESticle "
         "raw) code.", NULL, NULL, "&OK", NULL, 'o', 0);

      return (D_O_K);
   }

   /* Copy title. */
   ustrncat (patch->title, title, (USTRING_SIZE - 1));

   /* Enable patch. */
   patch->enabled = TRUE;

   cpu_patch_count++;

   if ((value = cpu_read (patch->address)) == patch->match_value)
   {
      /* Activate patch. */
      patch->active = TRUE;

      cpu_patch_table[patch->address] = (patch->value - value);
   }

   return (D_REDRAW);
}

static int machine_cheat_manager_dialog_remove (DIALOG *dialog)
{
   DIALOG *main_dialog;
   int start;
   CPU_PATCH *src;
   int index;

   RT_ASSERT(dialog);

   if (cpu_patch_count == 0)
      return (D_O_K);

   main_dialog = machine_cheat_manager_dialog;

   start = main_dialog[MACHINE_CHEAT_MANAGER_DIALOG_LIST].d1;
   src = &cpu_patch_info[start];

   /* Disable patch. */
   if (src->active)
   {
      if (gui_alert ("Confirmation", "Really deactivate and remove this "
         "patch?", NULL, NULL, "&OK", "&Cancel", 'o', 'c') == 2)
      {
         return (D_O_K);
      }

      cpu_patch_table[src->address] = 0;
   }

   for (index = (start + 1); index < cpu_patch_count; index++)
   {
      CPU_PATCH *dest;

      src = &cpu_patch_info[index];
      dest = &cpu_patch_info[(index - 1)];

      memcpy (dest, src, sizeof (CPU_PATCH));
   }

   src = &cpu_patch_info[(cpu_patch_count - 1)];

   memset (src, 0, sizeof (CPU_PATCH));
   sprintf (src->title, "?");

   cpu_patch_count--;

   if (cpu_patch_count == 0)
   {
      main_dialog[MACHINE_CHEAT_MANAGER_DIALOG_ENABLED_CHECKBOX].flags &=
         ~D_SELECTED;
   }

   return (D_REDRAW);
}

static int machine_cheat_manager_dialog_enabled (DIALOG *dialog)
{
   DIALOG *obj_list;
   CPU_PATCH *patch;

   RT_ASSERT(dialog);

   if (cpu_patch_count == 0)
   {
      dialog->flags &= ~D_SELECTED;

      return (D_O_K);
   }

   obj_list = &machine_cheat_manager_dialog[MACHINE_CHEAT_MANAGER_DIALOG_LIST];

   patch = &cpu_patch_info[obj_list->d1];

   patch->enabled = TRUE_OR_FALSE(dialog->flags & D_SELECTED);

   /* Toggle patch. */
   if (!patch->enabled && patch->active)
   {
      patch->active = FALSE;

      cpu_patch_table[patch->address] = 0;
   }
   else if (patch->enabled && !patch->active)
   {
      UINT8 value;

      if ((value = cpu_read (patch->address)) == patch->match_value)
      {
         /* Enable patch. */
         patch->active = TRUE;
    
         cpu_patch_table[patch->address] = (patch->value - value);
      }
   }

   scare_mouse ();
   object_message (obj_list, MSG_DRAW, 0);
   unscare_mouse ();

   return (D_O_K);
}

static USTRING machine_cheat_manager_dialog_list_texts[CPU_MAX_PATCHES];

static char *machine_cheat_manager_dialog_list_filler (int index, int *list_size)
{
   if (index >= 0)
   {
      CPU_PATCH *patch = &cpu_patch_info[index];
      UCHAR *text = machine_cheat_manager_dialog_list_texts[index];

      USTRING_CLEAR(text);
      uszprintf (text, USTRING_SIZE, "$%04x -$%02x +$%02x %s ",
         patch->address, patch->match_value, patch->value, (patch->active ?
            "Active" : " Idle "));

      /* Copy title. */
      ustrncat (text, patch->title, (USTRING_SIZE - 1));
      
      return (text);
   }
   else
   {
      RT_ASSERT(list_size);

      *list_size = cpu_patch_count;

      return (NULL);
   }
}

static int selected_player = -1;
static int selected_player_device = INPUT_DEVICE_NONE;

static int options_input_configure_dialog_player_select (DIALOG *dialog)
{
   DIALOG *main_dialog;
   int first, last;
   int index;

   RT_ASSERT(dialog);

   selected_player = dialog->d2;
   selected_player_device = input_get_player_device (selected_player);

   main_dialog = options_input_configure_dialog;

   first = OPTIONS_INPUT_CONFIGURE_DIALOG_DEVICE_0_SELECT;
   last  = OPTIONS_INPUT_CONFIGURE_DIALOG_DEVICE_7_SELECT;

   for (index = first; index <= last; index++)
      main_dialog[index].flags &= ~D_SELECTED;

   main_dialog[(first + selected_player_device)].flags |= D_SELECTED;

   scare_mouse ();

   for (index = first; index <= last; index++)
      object_message (&main_dialog[index],  MSG_DRAW, 0);

   first = OPTIONS_INPUT_CONFIGURE_DIALOG_SET_BUTTON_AUTO_1;
   last  = OPTIONS_INPUT_CONFIGURE_DIALOG_SET_BUTTON_AUTO_8;

   for (index = first; index <= last; index++)
   {
      DIALOG *dialog = &main_dialog[index];

      /* d2 = button. */

      if (input_get_player_button_param (selected_player, dialog->d2,
         INPUT_PLAYER_BUTTON_PARAM_AUTO))
      {
         dialog->flags |= D_SELECTED;
      }
      else
      {
         dialog->flags &= ~D_SELECTED;
      }

      object_message (dialog, MSG_DRAW, 0);
   }

   first = OPTIONS_INPUT_CONFIGURE_DIALOG_SET_BUTTON_TURBO_1;
   last  = OPTIONS_INPUT_CONFIGURE_DIALOG_SET_BUTTON_TURBO_8;

   for (index = first; index <= last; index++)
   {
      DIALOG *dialog = &main_dialog[index];

      /* d2 = button. */

      if (input_get_player_button_param (selected_player, dialog->d2,
         INPUT_PLAYER_BUTTON_PARAM_TURBO))
      {
         dialog->flags |= D_SELECTED;
      }
      else
      {
         dialog->flags &= ~D_SELECTED;
      }

      object_message (dialog, MSG_DRAW, 0);
   }

   unscare_mouse ();

   return (D_O_K);
}

static int options_input_configure_dialog_device_select (DIALOG *dialog)
{
   RT_ASSERT(dialog);

   if (selected_player < 0)
   {
      gui_alert ("Error", "Please select a player to modify first.", NULL,
         NULL, "&OK", NULL, 'o', 0);

      return (D_O_K);
   }

   selected_player_device = dialog->d2;

   input_set_player_device (selected_player, selected_player_device);

   return (D_O_K);
}

static int options_input_configure_dialog_set_buttons (DIALOG *dialog)
{
   int button;

   RT_ASSERT(dialog);

   if (selected_player < 0)
   {
      gui_alert ("Error", "Please select a player to modify first.", NULL,
         NULL, "&OK", NULL, 'o', 0);

      if (dialog->proc == sl_checkbox)
         dialog->flags ^= D_SELECTED;

      return (D_O_K);
   }

   button = dialog->d2;

   switch (dialog->d1)
   {
      case 0:  /* Map button. */
      {
         if (selected_player_device == INPUT_DEVICE_NONE)
         {
            gui_alert ("Error", "The selected player is currently "
               "disabled.", NULL, NULL, "&OK", NULL, 'o', 0);
      
            return (D_O_K);
         }
      
         if (selected_player_device == INPUT_DEVICE_MOUSE)
         {
            gui_alert ("Error", "Unable to set buttons for mouse at this "
               "time.", NULL, NULL, "&OK", NULL, 'o', 0);
      
            return (D_O_K);
         }
      
         message_local ("Scanning for device changes, press ESC to cancel.");
          
         input_map_player_button (selected_player, button);

         break;
      }

      case 1:  /* Set auto. */
      {
         input_set_player_button_param (selected_player, button,
            INPUT_PLAYER_BUTTON_PARAM_AUTO, (dialog->flags & D_SELECTED));

         break;
      }

      case 2:  /* Set turbo. */
      {
         input_set_player_button_param (selected_player, button,
            INPUT_PLAYER_BUTTON_PARAM_TURBO, (dialog->flags & D_SELECTED));
            
         break;
      }

      default:
         WARN_GENERIC();
   }

   return (D_O_K);
}

static int options_input_configure_dialog_calibrate (DIALOG *dialog)
{
   RT_ASSERT(dialog);

   if (selected_player < 0)
   {
      gui_alert ("Error", "Please select a player to modify first.", NULL,
         NULL, "&OK", NULL, 'o', 0);

      return (D_O_K);
   }

   switch (selected_player_device)
   {
      case INPUT_DEVICE_JOYSTICK_1:
      case INPUT_DEVICE_JOYSTICK_2:
      {
         int index;

         index = (selected_player_device - INPUT_DEVICE_JOYSTICK_1);

         while (joy[index].flags & JOYFLAG_CALIBRATE)
         {
            int scancode;

            message_local ("%s, and press any key.",
               calibrate_joystick_name (index));

            while (!keypressed ())
               gui_heartbeat ();

            ureadkey (&scancode);

            if (scancode == KEY_ESC)
            {
               gui_message (GUI_ERROR_COLOR, "Joystick calibration "
                  "cancelled.");
    
               return (D_O_K);
            }

            if (calibrate_joystick (index) != 0)
            {
               gui_alert ("Error", "An unknown error occured while "
                  "attempting to calibrate the device.", NULL, NULL, "&OK",
                     NULL, 'o', 0);
   
               return (D_O_K);
            }
         }

         gui_alert ("Calibration Complete", "The selected device has been "
            "calibrated.", NULL, NULL, "&Save", NULL, 's', 0);

         save_joystick_data (NULL);

         break;
      }

      default:
      {
         gui_alert ("Error", "The selected device does not require "
            "calibration.", NULL, NULL, "&OK", NULL, 'o', 0);

         break;
      }
   }

   return (D_O_K);
}

static int lobby_dialog_load (void)
{
   /* Note: D_CLOSE means success, D_O_K means failure. */
   if (main_menu_open () == D_CLOSE)
   {
      DIALOG *obj_ok;

      obj_ok = &lobby_dialog[LOBBY_DIALOG_OK_BUTTON];

      obj_ok->flags &= ~D_DISABLED;

      scare_mouse ();
      object_message (obj_ok, MSG_DRAW, 0);
      unscare_mouse ();
   }

   return (D_O_K);
}
