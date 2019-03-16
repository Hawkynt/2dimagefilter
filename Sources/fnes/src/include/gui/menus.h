/* Define helper macros. */
#define DEFINE_MENU(name)           static MENU * name = NULL
#define DEFINE_MENU_CALLBACK(func)  static int func (void)
#define IMPORT_MENU(menu)           (MENU *) & menu
#define MENU_SPLITTER               { "",   NULL, NULL, 0, NULL }
#define MENU_ENDCAP                 { NULL, NULL, NULL, 0, NULL }

DEFINE_MENU(main_open_recent_menu);
DEFINE_MENU(main_replay_select_menu);
DEFINE_MENU(main_replay_record_menu);
DEFINE_MENU(main_replay_play_menu);
DEFINE_MENU(main_replay_menu);
DEFINE_MENU(main_menu);
DEFINE_MENU(machine_save_state_select_menu);
DEFINE_MENU(machine_save_state_autosave_menu);
DEFINE_MENU(machine_save_state_menu);
DEFINE_MENU(machine_region_menu);
DEFINE_MENU(machine_speed_up_down_menu);
DEFINE_MENU(machine_frame_skip_menu);
DEFINE_MENU(machine_menu);
DEFINE_MENU(audio_channels_menu);
DEFINE_MENU(audio_output_buffer_menu);
DEFINE_MENU(audio_output_menu);
DEFINE_MENU(audio_record_menu);
DEFINE_MENU(audio_menu);
DEFINE_MENU(video_driver_dos_menu);
DEFINE_MENU(video_driver_windows_menu);
DEFINE_MENU(video_driver_linux_menu);
DEFINE_MENU(video_driver_unix_menu);
DEFINE_MENU(video_driver_menu);
DEFINE_MENU(video_resolution_proportionate_menu);
DEFINE_MENU(video_resolution_menu);
DEFINE_MENU(video_color_depth_menu);
DEFINE_MENU(video_buffer_menu);
DEFINE_MENU(video_blitter_menu);
DEFINE_MENU(video_filters_menu);
DEFINE_MENU(video_layers_menu);
DEFINE_MENU(video_palette_menu);
DEFINE_MENU(video_menu);
DEFINE_MENU(options_input_menu);
DEFINE_MENU(options_cpu_usage_menu);
DEFINE_MENU(options_gui_theme_menu);
DEFINE_MENU(options_menu);
DEFINE_MENU(netplay_menu);
DEFINE_MENU(help_menu);
DEFINE_MENU(top_menu);

DEFINE_MENU_CALLBACK(main_open_recent_menu_0);
DEFINE_MENU_CALLBACK(main_open_recent_menu_1);
DEFINE_MENU_CALLBACK(main_open_recent_menu_2);
DEFINE_MENU_CALLBACK(main_open_recent_menu_3);
DEFINE_MENU_CALLBACK(main_open_recent_menu_4);
DEFINE_MENU_CALLBACK(main_open_recent_menu_5);
DEFINE_MENU_CALLBACK(main_open_recent_menu_6);
DEFINE_MENU_CALLBACK(main_open_recent_menu_7);
DEFINE_MENU_CALLBACK(main_open_recent_menu_8);
DEFINE_MENU_CALLBACK(main_open_recent_menu_9);
DEFINE_MENU_CALLBACK(main_open_recent_menu_lock);
DEFINE_MENU_CALLBACK(main_open_recent_menu_clear);

static const MENU main_open_recent_menu_base[] =
{
   { NULL,     main_open_recent_menu_0,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_1,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_2,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_3,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_4,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_5,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_6,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_7,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_8,     NULL, 0, NULL },
   { NULL,     main_open_recent_menu_9,     NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Lock",  main_open_recent_menu_lock,  NULL, 0, NULL },
   { "&Clear", main_open_recent_menu_clear, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(main_replay_select_menu_0);
DEFINE_MENU_CALLBACK(main_replay_select_menu_1);
DEFINE_MENU_CALLBACK(main_replay_select_menu_2);
DEFINE_MENU_CALLBACK(main_replay_select_menu_3);
DEFINE_MENU_CALLBACK(main_replay_select_menu_4);
DEFINE_MENU_CALLBACK(main_replay_select_menu_5);
DEFINE_MENU_CALLBACK(main_replay_select_menu_6);
DEFINE_MENU_CALLBACK(main_replay_select_menu_7);
DEFINE_MENU_CALLBACK(main_replay_select_menu_8);
DEFINE_MENU_CALLBACK(main_replay_select_menu_9);

static const MENU main_replay_select_menu_base[] =
{
   { NULL, main_replay_select_menu_0, NULL, 0, NULL },
   { NULL, main_replay_select_menu_1, NULL, 0, NULL },
   { NULL, main_replay_select_menu_2, NULL, 0, NULL },
   { NULL, main_replay_select_menu_3, NULL, 0, NULL },
   { NULL, main_replay_select_menu_4, NULL, 0, NULL },
   { NULL, main_replay_select_menu_5, NULL, 0, NULL },
   { NULL, main_replay_select_menu_6, NULL, 0, NULL },
   { NULL, main_replay_select_menu_7, NULL, 0, NULL },
   { NULL, main_replay_select_menu_8, NULL, 0, NULL },
   { NULL, main_replay_select_menu_9, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(main_replay_record_menu_start);
DEFINE_MENU_CALLBACK(main_replay_record_menu_stop);

static const MENU main_replay_record_menu_base[] =
{
   { "&Start... (F12)", main_replay_record_menu_start, NULL, 0,          NULL },
   { "S&top     (F12)", main_replay_record_menu_stop,  NULL, D_DISABLED, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(main_replay_play_menu_start);
DEFINE_MENU_CALLBACK(main_replay_play_menu_stop);

static const MENU main_replay_play_menu_base[] =
{
   { "&Start", main_replay_play_menu_start, NULL,          0, NULL },
   { "S&top",  main_replay_play_menu_stop,  NULL, D_DISABLED, NULL },
   MENU_ENDCAP
};

static const MENU main_replay_menu_base[] =
{
   { "&Select", NULL, IMPORT_MENU(main_replay_select_menu), 0, NULL },
   MENU_SPLITTER,
   { "&Record", NULL, IMPORT_MENU(main_replay_record_menu), 0, NULL },
   { "&Play",   NULL, IMPORT_MENU(main_replay_play_menu),   0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(main_menu_resume);
DEFINE_MENU_CALLBACK(main_menu_open);
DEFINE_MENU_CALLBACK(main_menu_close);
DEFINE_MENU_CALLBACK(main_menu_save_snapshot);
DEFINE_MENU_CALLBACK(main_menu_advance_frame);
DEFINE_MENU_CALLBACK(main_menu_view_console);
DEFINE_MENU_CALLBACK(main_menu_view_log);
DEFINE_MENU_CALLBACK(main_menu_exit);

static const MENU main_menu_base[] =
{
   { "&Resume        (ESC)", main_menu_resume,        NULL,                               0, NULL },
   MENU_SPLITTER,                                                                       
   { "&Open...",             main_menu_open,          NULL,                               0, NULL },
   { "Open R&ecent",         NULL,                    IMPORT_MENU(main_open_recent_menu), 0, NULL },
   { "&Close",               main_menu_close,         NULL,                               0, NULL },
   MENU_SPLITTER,            
   { "Re&play",              NULL,                    IMPORT_MENU(main_replay_menu),      0, NULL },
   MENU_SPLITTER,            
   { "&Save Snapshot (F1)",  main_menu_save_snapshot, NULL,                               0, NULL },
   { "&Advance Frame",       main_menu_advance_frame, NULL,                                    0, NULL },
   MENU_SPLITTER,
   { "&View Console...",     main_menu_view_console,  NULL,                               0, NULL },
   { "View &Log...",         main_menu_view_log,      NULL,                               0, NULL },
   MENU_SPLITTER,       
   { "E&xit",                main_menu_exit,          NULL,                               0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(machine_save_state_select_menu_0);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_1);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_2);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_3);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_4);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_5);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_6);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_7);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_8);
DEFINE_MENU_CALLBACK(machine_save_state_select_menu_9);

static const MENU machine_save_state_select_menu_base[] =
{
   { NULL, machine_save_state_select_menu_0, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_1, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_2, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_3, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_4, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_5, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_6, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_7, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_8, NULL, 0, NULL },
   { NULL, machine_save_state_select_menu_9, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(machine_save_state_autosave_menu_disabled);
DEFINE_MENU_CALLBACK(machine_save_state_autosave_menu_10_seconds);
DEFINE_MENU_CALLBACK(machine_save_state_autosave_menu_30_seconds);
DEFINE_MENU_CALLBACK(machine_save_state_autosave_menu_60_seconds);
DEFINE_MENU_CALLBACK(machine_save_state_autosave_menu_custom);

static const MENU machine_save_state_autosave_menu_base[] =
{
   { "&Disabled",      machine_save_state_autosave_menu_disabled,   NULL, 0, NULL },
   MENU_SPLITTER,
   { "&1: 10 Seconds", machine_save_state_autosave_menu_10_seconds, NULL, 0, NULL },
   { "&2: 30 Seconds", machine_save_state_autosave_menu_30_seconds, NULL, 0, NULL },
   { "&3: 60 Seconds", machine_save_state_autosave_menu_60_seconds, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Custom...",     machine_save_state_autosave_menu_custom,     NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(machine_save_state_menu_quick_save);
DEFINE_MENU_CALLBACK(machine_save_state_menu_quick_load);
DEFINE_MENU_CALLBACK(machine_save_state_menu_save);
DEFINE_MENU_CALLBACK(machine_save_state_menu_restore);
DEFINE_MENU_CALLBACK(machine_save_state_menu_select);

static const MENU machine_save_state_menu_base[] =
{
   { "&Quick Save (F3)", machine_save_state_menu_quick_save, NULL,                                          0, NULL },
   { "Quick &Load (F4)", machine_save_state_menu_quick_load, NULL,                                          0, NULL },
   MENU_SPLITTER,
   { "&Select",          NULL,                               IMPORT_MENU(machine_save_state_select_menu),   0, NULL },
   MENU_SPLITTER,                                    
   { "S&ave...    (F5)", machine_save_state_menu_save,       NULL,                                          0, NULL },
   { "&Restore    (F6)", machine_save_state_menu_restore,    NULL,                                          0, NULL },
   MENU_SPLITTER,
   { "A&utosave",        NULL,                               IMPORT_MENU(machine_save_state_autosave_menu), 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(machine_region_menu_automatic);
DEFINE_MENU_CALLBACK(machine_region_menu_ntsc);
DEFINE_MENU_CALLBACK(machine_region_menu_pal);

static const MENU machine_region_menu_base[] =
{
   { "&Automatic", machine_region_menu_automatic, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&NTSC",      machine_region_menu_ntsc,      NULL, 0, NULL },
   { "&PAL",       machine_region_menu_pal,       NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(machine_speed_up_down_menu_50_percent);
DEFINE_MENU_CALLBACK(machine_speed_up_down_menu_100_percent);
DEFINE_MENU_CALLBACK(machine_speed_up_down_menu_200_percent);
DEFINE_MENU_CALLBACK(machine_speed_up_down_menu_custom);

static const MENU machine_speed_up_down_menu_base[] =
{
   { "&1: 50%",    machine_speed_up_down_menu_50_percent,  NULL, 0, NULL },
   { "&2: 100%",   machine_speed_up_down_menu_100_percent, NULL, 0, NULL },
   { "&3: 200%",   machine_speed_up_down_menu_200_percent, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Custom...", machine_speed_up_down_menu_custom,      NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(machine_frame_skip_menu_automatic);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_disabled);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_1_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_2_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_3_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_4_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_5_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_6_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_7_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_8_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_9_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_10_frames);
DEFINE_MENU_CALLBACK(machine_frame_skip_menu_custom);

static const MENU machine_frame_skip_menu_base[] =
{
   { "&Automatic", machine_frame_skip_menu_automatic, NULL, 0, NULL },
   { "&Disabled",  machine_frame_skip_menu_disabled,  NULL, 0, NULL },
   MENU_SPLITTER,
   { "&1 Frames",  machine_frame_skip_menu_1_frames,  NULL, 0, NULL },
   { "&2 Frames",  machine_frame_skip_menu_2_frames,  NULL, 0, NULL },
   { "&3 Frames",  machine_frame_skip_menu_3_frames,  NULL, 0, NULL },
   { "&4 Frames",  machine_frame_skip_menu_4_frames,  NULL, 0, NULL },
   { "&5 Frames",  machine_frame_skip_menu_5_frames,  NULL, 0, NULL },
   { "&6 Frames",  machine_frame_skip_menu_6_frames,  NULL, 0, NULL },
   { "&7 Frames",  machine_frame_skip_menu_7_frames,  NULL, 0, NULL },
   { "&8 Frames",  machine_frame_skip_menu_8_frames,  NULL, 0, NULL },
   { "&9 Frames",  machine_frame_skip_menu_9_frames,  NULL, 0, NULL },
   { "1&0 Frames", machine_frame_skip_menu_10_frames, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Custom...", machine_frame_skip_menu_custom,    NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(machine_menu_soft_reset);
DEFINE_MENU_CALLBACK(machine_menu_hard_reset);
DEFINE_MENU_CALLBACK(machine_menu_timing_smoothest);
DEFINE_MENU_CALLBACK(machine_menu_timing_most_accurate);
DEFINE_MENU_CALLBACK(machine_menu_speed_cap);
DEFINE_MENU_CALLBACK(machine_menu_cheat_manager);

static const MENU machine_menu_base[] =
{
   { "&Soft Reset",       machine_menu_soft_reset,           NULL,                                    0, NULL },
   { "&Hard Reset",       machine_menu_hard_reset,           NULL,                                    0, NULL },
   MENU_SPLITTER,                                                                      
   { "Timing",            NULL,                              NULL,                                    0, NULL },
   { "  S&moothest",      machine_menu_timing_smoothest,     NULL,                                    0, NULL },
   { "  Most &Accurate",  machine_menu_timing_most_accurate, NULL,                                    0, NULL },
   MENU_SPLITTER,
   { "Sa&ve State",       NULL,                              IMPORT_MENU(machine_save_state_menu),    0, NULL },
   MENU_SPLITTER,
   { "Speed &Up/Down",    NULL,                              IMPORT_MENU(machine_speed_up_down_menu), 0, NULL },
   { "Speed &Cap",        machine_menu_speed_cap,            NULL,                                    0, NULL },
   { "&Frame Skip",       NULL,                              IMPORT_MENU(machine_frame_skip_menu),    0, NULL },
   MENU_SPLITTER,
   { "&Region",           NULL,                              IMPORT_MENU(machine_region_menu),        0, NULL },
   MENU_SPLITTER,
   { "Ch&eat Manager...", machine_menu_cheat_manager,        NULL,                                    0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(netplay_menu_start_as_server);
DEFINE_MENU_CALLBACK(netplay_menu_start_as_client);

static const MENU netplay_menu_base[] =
{
   { "&Start as Server...", netplay_menu_start_as_server, NULL, 0, NULL },
   { "Start as &Client...", netplay_menu_start_as_client, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(audio_channels_menu_square_1);
DEFINE_MENU_CALLBACK(audio_channels_menu_square_2);
DEFINE_MENU_CALLBACK(audio_channels_menu_triangle);
DEFINE_MENU_CALLBACK(audio_channels_menu_noise);
DEFINE_MENU_CALLBACK(audio_channels_menu_dmc);
DEFINE_MENU_CALLBACK(audio_channels_menu_extra_1);
DEFINE_MENU_CALLBACK(audio_channels_menu_extra_2);
DEFINE_MENU_CALLBACK(audio_channels_menu_extra_3);
DEFINE_MENU_CALLBACK(audio_channels_menu_enable_all);
DEFINE_MENU_CALLBACK(audio_channels_menu_disable_all);

static const MENU audio_channels_menu_base[] =
{
   { "&Square 1",    audio_channels_menu_square_1,    NULL, 0, NULL },
   { "Square &2",    audio_channels_menu_square_2,    NULL, 0, NULL },
   { "&Triangle",    audio_channels_menu_triangle,    NULL, 0, NULL },
   { "&Noise",       audio_channels_menu_noise,       NULL, 0, NULL },
   { "&DMC",         audio_channels_menu_dmc,         NULL, 0, NULL },
   { "&Extra 1",     audio_channels_menu_extra_1,     NULL, 0, NULL },
   { "E&xtra 2",     audio_channels_menu_extra_2,     NULL, 0, NULL },
   { "Extra &3",     audio_channels_menu_extra_3,     NULL, 0, NULL },
   MENU_SPLITTER,
   { "En&able All",  audio_channels_menu_enable_all,  NULL, 0, NULL },
   { "D&isable All", audio_channels_menu_disable_all, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(audio_output_buffer_menu_1_frame);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_2_frames);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_3_frames);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_4_frames);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_5_frames);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_6_frames);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_7_frames);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_8_frames);
DEFINE_MENU_CALLBACK(audio_output_buffer_menu_custom);

static const MENU audio_output_buffer_menu_base[] =
{
   { "&1 Frame",   audio_output_buffer_menu_1_frame,  NULL, 0, NULL },
   { "&2 Frames",  audio_output_buffer_menu_2_frames, NULL, 0, NULL },
   { "&3 Frames",  audio_output_buffer_menu_3_frames, NULL, 0, NULL },
   { "&4 Frames",  audio_output_buffer_menu_4_frames, NULL, 0, NULL },
   { "&5 Frames",  audio_output_buffer_menu_5_frames, NULL, 0, NULL },
   { "&6 Frames",  audio_output_buffer_menu_6_frames, NULL, 0, NULL },
   { "&7 Frames",  audio_output_buffer_menu_7_frames, NULL, 0, NULL },
   { "&8 Frames",  audio_output_buffer_menu_8_frames, NULL, 0, NULL },
   MENU_SPLITTER,        
   { "&Custom...", audio_output_buffer_menu_custom,   NULL, 0, NULL },
   MENU_ENDCAP
};                                             

DEFINE_MENU_CALLBACK(audio_output_menu_subsystem_allegro);
DEFINE_MENU_CALLBACK(audio_output_menu_subsystem_openal);
DEFINE_MENU_CALLBACK(audio_output_menu_sampling_rate_22050_hz);
DEFINE_MENU_CALLBACK(audio_output_menu_sampling_rate_44100_hz);
DEFINE_MENU_CALLBACK(audio_output_menu_sampling_rate_48000_hz);
DEFINE_MENU_CALLBACK(audio_output_menu_sampling_rate_custom);
DEFINE_MENU_CALLBACK(audio_output_menu_mixing_mono);
DEFINE_MENU_CALLBACK(audio_output_menu_mixing_stereo);
DEFINE_MENU_CALLBACK(audio_output_menu_mixing_stereo_reversed);
                                                  
static const MENU audio_output_menu_base[] =
{
   { "Subsystem",            NULL,                                     NULL,                                  0, NULL },
   { "  &Allegro",           audio_output_menu_subsystem_allegro,      NULL,                                  0, NULL },
   { "  &OpenAL",            audio_output_menu_subsystem_openal,       NULL,                                  0, NULL },
   MENU_SPLITTER,          
   { "Sampling Rate",        NULL,                                     NULL,                                  0, NULL },
   { "  &1: 22050 Hz",       audio_output_menu_sampling_rate_22050_hz, NULL,                                  0, NULL },
   { "  &2: 44100 Hz",       audio_output_menu_sampling_rate_44100_hz, NULL,                                  0, NULL },
   { "  &3: 48000 Hz",       audio_output_menu_sampling_rate_48000_hz, NULL,                                  0, NULL },
   { "  ",                   NULL,                                     NULL,                                  0, NULL },
   { "  &Custom...",         audio_output_menu_sampling_rate_custom,   NULL,                                  0, NULL },
   MENU_SPLITTER,
   { "Mixing",               NULL,                                     NULL,                                  0, NULL },
   { "  &Mono",              audio_output_menu_mixing_mono,            NULL,                                  0, NULL },
   { "  &Stereo",            audio_output_menu_mixing_stereo,          NULL,                                  0, NULL },
   { "  Stereo (&Reversed)", audio_output_menu_mixing_stereo_reversed, NULL,                                  0, NULL },
   MENU_SPLITTER,
   { "&Buffer",              NULL,                                     IMPORT_MENU(audio_output_buffer_menu), 0, NULL },
   MENU_ENDCAP          
};                                             
                          
DEFINE_MENU_CALLBACK(audio_record_menu_start);
DEFINE_MENU_CALLBACK(audio_record_menu_stop);

static const MENU audio_record_menu_base[] =
{
   { "&Start", audio_record_menu_start, NULL, 0,          NULL },
   { "S&top",  audio_record_menu_stop,  NULL, D_DISABLED, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(audio_menu_enable_apu);
DEFINE_MENU_CALLBACK(audio_menu_enable_output);
DEFINE_MENU_CALLBACK(audio_menu_emulation_fast);
DEFINE_MENU_CALLBACK(audio_menu_emulation_accurate);
DEFINE_MENU_CALLBACK(audio_menu_emulation_ultra);
DEFINE_MENU_CALLBACK(audio_menu_volume_increase);
DEFINE_MENU_CALLBACK(audio_menu_volume_decrease);
DEFINE_MENU_CALLBACK(audio_menu_volume_custom);
DEFINE_MENU_CALLBACK(audio_menu_volume_reset);

/* Slot in which the "current volume" text is to placed. */
#define AUDIO_MENU_VOLUME_TEXT   12

static const MENU audio_menu_base[] =
{
   { "&Enable APU",      audio_menu_enable_apu,         NULL,                             0, NULL },
   { "Enable &Output",   audio_menu_enable_output,      NULL,                             0, NULL },
   MENU_SPLITTER,                                                                      
   { "Emulation",        NULL,                          NULL,                             0, NULL },
   { "  &Fast",          audio_menu_emulation_fast,     NULL,                             0, NULL },
   { "  &Accurate",      audio_menu_emulation_accurate, NULL,                             0, NULL },
   { "  &Ultra",         audio_menu_emulation_ultra,    NULL,                             0, NULL },
   { "  ",               NULL,                          NULL,                             0, NULL },
   { "  &Channels",      NULL,                          IMPORT_MENU(audio_channels_menu), 0, NULL },
   MENU_SPLITTER,                                                                      
   { "Out&put",          NULL,                          IMPORT_MENU(audio_output_menu),   0, NULL },
   MENU_SPLITTER,
   /* Kludge to make MENU_FROM_BASE() load the menu. */
   { "insert text here", NULL,                          NULL,                             0, NULL },
   { "  &Increase (+)",  audio_menu_volume_increase,    NULL,                             0, NULL },
   { "  &Decrease (-)",  audio_menu_volume_decrease,    NULL,                             0, NULL },
   { "  ",               NULL,                          NULL,                             0, NULL },
   { "  Cu&stom...",     audio_menu_volume_custom,      NULL,                             0, NULL },
   { "  Rese&t",         audio_menu_volume_reset,       NULL,                             0, NULL },
   MENU_SPLITTER,                                                       
   { "&Record",          NULL,                          IMPORT_MENU(audio_record_menu),   0, NULL },
   MENU_ENDCAP
};

#ifdef ALLEGRO_DOS

DEFINE_MENU_CALLBACK(video_driver_dos_menu_vga);
DEFINE_MENU_CALLBACK(video_driver_dos_menu_vga_mode_x);
DEFINE_MENU_CALLBACK(video_driver_dos_menu_vesa);
DEFINE_MENU_CALLBACK(video_driver_dos_menu_vesa_2_banked);
DEFINE_MENU_CALLBACK(video_driver_dos_menu_vesa_2_linear);
DEFINE_MENU_CALLBACK(video_driver_dos_menu_vesa_3);
DEFINE_MENU_CALLBACK(video_driver_dos_menu_vesa_vbe_af);

static const MENU video_driver_dos_menu_base[] =
{
   { "&VGA",           video_driver_dos_menu_vga,           NULL, 0, NULL },
   { "VGA Mode-&X",    video_driver_dos_menu_vga_mode_x,    NULL, 0, NULL },
   MENU_SPLITTER,
   { "V&ESA",          video_driver_dos_menu_vesa,          NULL, 0, NULL },
   { "VESA 2 &Banked", video_driver_dos_menu_vesa_2_banked, NULL, 0, NULL },
   { "VESA 2 &Linear", video_driver_dos_menu_vesa_2_linear, NULL, 0, NULL },
   { "VE&SA 3",        video_driver_dos_menu_vesa_3,        NULL, 0, NULL },
   { "VESA VBE/&AF",   video_driver_dos_menu_vesa_vbe_af,   NULL, 0, NULL },
   MENU_ENDCAP
};

#else /* ALLEGRO_DOS */

static const MENU video_driver_dos_menu_base[] =
{
   MENU_ENDCAP
};

#endif   /* !ALLEGRO_DOS */

#ifdef ALLEGRO_WINDOWS

DEFINE_MENU_CALLBACK(video_driver_windows_menu_directx);
DEFINE_MENU_CALLBACK(video_driver_windows_menu_directx_window);
DEFINE_MENU_CALLBACK(video_driver_windows_menu_directx_overlay);
DEFINE_MENU_CALLBACK(video_driver_windows_menu_gdi);

static const MENU video_driver_windows_menu_base[] =
{
   { "&DirectX",         video_driver_windows_menu_directx,         NULL, 0, NULL },
   { "DirectX &Window",  video_driver_windows_menu_directx_window,  NULL, 0, NULL },
   { "DirectX &Overlay", video_driver_windows_menu_directx_overlay, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&GDI",             video_driver_windows_menu_gdi,             NULL, 0, NULL },
   MENU_ENDCAP
};

#else /* ALLEGRO_WINDOWS */

static const MENU video_driver_windows_menu_base[] =
{
   MENU_ENDCAP
};

#endif   /* !ALLEGRO_WINDOWS */

#ifdef ALLEGRO_LINUX

DEFINE_MENU_CALLBACK(video_driver_linux_menu_vga);
DEFINE_MENU_CALLBACK(video_driver_linux_menu_vga_mode_x);
DEFINE_MENU_CALLBACK(video_driver_linux_menu_vesa_vbe_af);
DEFINE_MENU_CALLBACK(video_driver_linux_menu_framebuffer);
DEFINE_MENU_CALLBACK(video_driver_linux_menu_svgalib);

static const MENU video_driver_linux_menu_base[] =
{
   { "&VGA",         video_driver_linux_menu_vga,         NULL, 0, NULL },
   { "VGA Mode-&X",  video_driver_linux_menu_vga_mode_x,  NULL, 0, NULL },
   MENU_SPLITTER,
   { "VESA VBE/&AF", video_driver_linux_menu_vesa_vbe_af, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Framebuffer", video_driver_linux_menu_framebuffer, NULL, 0, NULL },
   { "&SVGAlib",     video_driver_linux_menu_svgalib,     NULL, 0, NULL },
   MENU_ENDCAP
};

#else /* ALLEGRO_LINUX */

static const MENU video_driver_linux_menu_base[] =
{
   MENU_ENDCAP
};

#endif   /* !ALLEGRO_LINUX */

#ifdef ALLEGRO_UNIX

DEFINE_MENU_CALLBACK(video_driver_unix_menu_x_windows);
DEFINE_MENU_CALLBACK(video_driver_unix_menu_x_windows_full);
DEFINE_MENU_CALLBACK(video_driver_unix_menu_x_dga);
DEFINE_MENU_CALLBACK(video_driver_unix_menu_x_dga_full);
DEFINE_MENU_CALLBACK(video_driver_unix_menu_x_dga_2);

static const MENU video_driver_unix_menu_base[] =
{
   { "X &Windows",      video_driver_unix_menu_x_windows,      NULL, 0, NULL },
   { "X &Windows Full", video_driver_unix_menu_x_windows_full, NULL, 0, NULL },
   MENU_SPLITTER,
   { "X/&DGA",          video_driver_unix_menu_x_dga,          NULL, 0, NULL },
   { "X/&DGA FULL",     video_driver_unix_menu_x_dga_full,     NULL, 0, NULL },
   { "X/D&GA 2",        video_driver_unix_menu_x_dga_2,        NULL, 0, NULL },
   MENU_ENDCAP
};

#else /* ALLEGRO_UNIX */

static const MENU video_driver_unix_menu_base[] =
{
   MENU_ENDCAP
};

#endif   /* !ALLEGRO_UNIX */

DEFINE_MENU_CALLBACK(video_driver_menu_automatic);

#ifdef USE_ALLEGROGL

DEFINE_MENU_CALLBACK(video_driver_menu_opengl);
DEFINE_MENU_CALLBACK(video_driver_menu_opengl_full);
DEFINE_MENU_CALLBACK(video_driver_menu_opengl_win);

#endif   /* USE_ALLEGROGL */

static const MENU video_driver_menu_base[] =
{
   { "&Automatic",   video_driver_menu_automatic,   NULL,                                   0, NULL },
   MENU_SPLITTER,

#ifdef USE_ALLEGROGL

   { "&OpenGL",      video_driver_menu_opengl,      NULL,                                   0, NULL },
   { "OpenGL &Full", video_driver_menu_opengl_full, NULL,                                   0, NULL },
   { "OpenGL &Win",  video_driver_menu_opengl_win,  NULL,                                   0, NULL },
   MENU_SPLITTER,

#endif   /* USE_ALLEGROGL */

   /* Yay.  The cost of prettiness. :) */

#ifdef ALLEGRO_DOS
   { "&DOS",         NULL,                          IMPORT_MENU(video_driver_dos_menu),     0, NULL },
#endif
#ifdef ALLEGRO_WINDOWS
   { "&Windows",     NULL,                          IMPORT_MENU(video_driver_windows_menu), 0, NULL },
#endif
#ifdef ALLEGRO_LINUX
   { "&Linux",       NULL,                          IMPORT_MENU(video_driver_linux_menu),   0, NULL },
#endif
#ifdef ALLEGRO_UNIX
   { "&Unix",        NULL,                          IMPORT_MENU(video_driver_unix_menu),    0, NULL },
#endif
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_256_224);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_256_240);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_512_448);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_512_480);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_768_672);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_768_720);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_1024_896);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_1024_960);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_1280_1120);
DEFINE_MENU_CALLBACK(video_resolution_proportionate_menu_1280_1200);

static const MENU video_resolution_proportionate_menu_base[] =
{
   { " &1: 256x224",   video_resolution_proportionate_menu_256_224,   NULL, 0, NULL },
   { " &2: 256x240",   video_resolution_proportionate_menu_256_240,   NULL, 0, NULL },
   { " &3: 512x448",   video_resolution_proportionate_menu_512_448,   NULL, 0, NULL },
   { " &4: 512x480",   video_resolution_proportionate_menu_512_480,   NULL, 0, NULL },
   { " &5: 768x672",   video_resolution_proportionate_menu_768_672,   NULL, 0, NULL },
   { " &6: 768x720",   video_resolution_proportionate_menu_768_720,   NULL, 0, NULL },
   { " &7: 1024x896",  video_resolution_proportionate_menu_1024_896,  NULL, 0, NULL },
   { " &8: 1024x960",  video_resolution_proportionate_menu_1024_960,  NULL, 0, NULL },
   { " &9: 1280x1120", video_resolution_proportionate_menu_1280_1120, NULL, 0, NULL },
   { "1&0: 1280x1200", video_resolution_proportionate_menu_1280_1200, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_resolution_menu_320_240);
DEFINE_MENU_CALLBACK(video_resolution_menu_400_300);
DEFINE_MENU_CALLBACK(video_resolution_menu_640_480);
DEFINE_MENU_CALLBACK(video_resolution_menu_800_600);
DEFINE_MENU_CALLBACK(video_resolution_menu_1024_768);
DEFINE_MENU_CALLBACK(video_resolution_menu_1152_864);
DEFINE_MENU_CALLBACK(video_resolution_menu_1280_960);
DEFINE_MENU_CALLBACK(video_resolution_menu_1280_1024);
DEFINE_MENU_CALLBACK(video_resolution_menu_1600_1200);
DEFINE_MENU_CALLBACK(video_resolution_menu_custom);

static const MENU video_resolution_menu_base[] =
{
   { "&Proportionate", NULL,                            IMPORT_MENU(video_resolution_proportionate_menu), 0, NULL },
   MENU_SPLITTER,
   { "&1: 320x240",    video_resolution_menu_320_240,   NULL,                                             0, NULL },
   { "&2: 400x300",    video_resolution_menu_400_300,   NULL,                                             0, NULL },
   { "&3: 640x480",    video_resolution_menu_640_480,   NULL,                                             0, NULL },
   { "&4: 800x600",    video_resolution_menu_800_600,   NULL,                                             0, NULL },
   { "&5: 1024x768",   video_resolution_menu_1024_768,  NULL,                                             0, NULL },
   { "&6: 1152x864",   video_resolution_menu_1152_864,  NULL,                                             0, NULL },
   { "&7: 1280x960",   video_resolution_menu_1280_960,  NULL,                                             0, NULL },
   { "&8: 1280x1024",  video_resolution_menu_1280_1024, NULL,                                             0, NULL },
   { "&9: 1600x1200",  video_resolution_menu_1600_1200, NULL,                                             0, NULL },
   MENU_SPLITTER,
   { "&Custom...",     video_resolution_menu_custom,     NULL,                                             0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_color_depth_menu_paletted_8_bit);
DEFINE_MENU_CALLBACK(video_color_depth_menu_true_color_15_bit);
DEFINE_MENU_CALLBACK(video_color_depth_menu_true_color_16_bit);
DEFINE_MENU_CALLBACK(video_color_depth_menu_true_color_24_bit);
DEFINE_MENU_CALLBACK(video_color_depth_menu_true_color_32_bit);

static const MENU video_color_depth_menu_base[] =
{
   { "&Paletted (8-bit)",    video_color_depth_menu_paletted_8_bit,    NULL, 0, NULL },
   { "&True Color (15-bit)", video_color_depth_menu_true_color_15_bit, NULL, 0, NULL },
   { "True &Color (16-bit)", video_color_depth_menu_true_color_16_bit, NULL, 0, NULL },
   { "True C&olor (24-bit)", video_color_depth_menu_true_color_24_bit, NULL, 0, NULL },
   { "True Co&lor (32-bit)", video_color_depth_menu_true_color_32_bit, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_buffer_menu_match_resolution);
DEFINE_MENU_CALLBACK(video_buffer_menu_256_240);
DEFINE_MENU_CALLBACK(video_buffer_menu_320_240);
DEFINE_MENU_CALLBACK(video_buffer_menu_512_480);
DEFINE_MENU_CALLBACK(video_buffer_menu_640_480);
DEFINE_MENU_CALLBACK(video_buffer_menu_256_256);
DEFINE_MENU_CALLBACK(video_buffer_menu_512_512);
DEFINE_MENU_CALLBACK(video_buffer_menu_custom);

static const MENU video_buffer_menu_base[] =
{
   { "&Match Resolution", video_buffer_menu_match_resolution, NULL, 0, NULL },
   MENU_SPLITTER,
   { "Standard",          NULL,                               NULL, 0, NULL },
   { "  &1: 256x240",     video_buffer_menu_256_240,          NULL, 0, NULL },
   { "  &2: 320x240",     video_buffer_menu_320_240,          NULL, 0, NULL },
   { "  &3: 512x480",     video_buffer_menu_512_480,          NULL, 0, NULL },
   { "  &4: 640x480",     video_buffer_menu_640_480,          NULL, 0, NULL },
   { "Square",            NULL,                               NULL, 0, NULL },
   { "  &5: 256x256",     video_buffer_menu_256_256,          NULL, 0, NULL },
   { "  &6: 512x512",     video_buffer_menu_512_512,          NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Custom...",        video_buffer_menu_custom,           NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_blitter_menu_automatic);
DEFINE_MENU_CALLBACK(video_blitter_menu_normal);
DEFINE_MENU_CALLBACK(video_blitter_menu_des);
DEFINE_MENU_CALLBACK(video_blitter_menu_interpolated_2x);
DEFINE_MENU_CALLBACK(video_blitter_menu_interpolated_2x_hq);
DEFINE_MENU_CALLBACK(video_blitter_menu_2xscl);
DEFINE_MENU_CALLBACK(video_blitter_menu_desii);
DEFINE_MENU_CALLBACK(video_blitter_menu_super_2xscl);
DEFINE_MENU_CALLBACK(video_blitter_menu_ultra_2xscl);
DEFINE_MENU_CALLBACK(video_blitter_menu_hq2x);
DEFINE_MENU_CALLBACK(video_blitter_menu_interpolated_3x);
DEFINE_MENU_CALLBACK(video_blitter_menu_hq3x);
DEFINE_MENU_CALLBACK(video_blitter_menu_hq4x);
DEFINE_MENU_CALLBACK(video_blitter_menu_stretched);
DEFINE_MENU_CALLBACK(video_blitter_menu_ntsc);
DEFINE_MENU_CALLBACK(video_blitter_menu_configure);

static const MENU video_blitter_menu_base[] =
{
   { "&Automatic",             video_blitter_menu_automatic,       NULL, 0, NULL },
   MENU_SPLITTER,
   { "Static",                      NULL,                                  NULL, 0, NULL },
   { "   &1: Normal",               video_blitter_menu_normal,             NULL, 0, NULL },
   { "   &2: Interpolated 2X",      video_blitter_menu_interpolated_2x,    NULL, 0, NULL },
   { "   &3: Interpolated 2X (HQ)", video_blitter_menu_interpolated_2x_hq, NULL, 0, NULL },
   { "   &4: Interpolated 3X",      video_blitter_menu_interpolated_3x,    NULL, 0, NULL },
   { "   &5: DES",                  video_blitter_menu_des,                NULL, 0, NULL },
   { "   &6: DESii",                video_blitter_menu_desii,              NULL, 0, NULL },
   { "   &7: 2xSCL",                video_blitter_menu_2xscl,              NULL, 0, NULL },
   { "   &8: Super 2xSCL",          video_blitter_menu_super_2xscl,        NULL, 0, NULL },
   { "   &9: Ultra 2xSCL",          video_blitter_menu_ultra_2xscl,        NULL, 0, NULL },
   { "  1&0: HQ2X",                 video_blitter_menu_hq2x,               NULL, 0, NULL },
   { "  11: &HQ3X",                 video_blitter_menu_hq3x,               NULL, 0, NULL },
   { "  12: H&Q4X",                 video_blitter_menu_hq4x,               NULL, 0, NULL },
   { "Dynamic",                     NULL,                                  NULL, 0, NULL },
   { "  13: &Stretched",            video_blitter_menu_stretched,          NULL, 0, NULL },
   { "  14: &NTSC",                 video_blitter_menu_ntsc,               NULL, 0, NULL },
   MENU_SPLITTER,                                                         
   { "&Configure...",               video_blitter_menu_configure,          NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_filters_menu_scanlines_25_percent);
DEFINE_MENU_CALLBACK(video_filters_menu_scanlines_50_percent);
DEFINE_MENU_CALLBACK(video_filters_menu_scanlines_100_percent);

static const MENU video_filters_menu_base[] =
{
   { "&Scanlines (25%)",  video_filters_menu_scanlines_25_percent,  NULL, 0, NULL },
   { "S&canlines (50%)",  video_filters_menu_scanlines_50_percent,  NULL, 0, NULL },
   { "Sc&anlines (100%)", video_filters_menu_scanlines_100_percent, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_layers_menu_sprites_a);
DEFINE_MENU_CALLBACK(video_layers_menu_sprites_b);
DEFINE_MENU_CALLBACK(video_layers_menu_background);
DEFINE_MENU_CALLBACK(video_layers_menu_hide_horizontal_scrolling);
DEFINE_MENU_CALLBACK(video_layers_menu_hide_vertical_scrolling);
DEFINE_MENU_CALLBACK(video_layers_menu_flip_mirroring);

static const MENU video_layers_menu_base[] =
{
   { "&Sprites A  (F7)",     video_layers_menu_sprites_a,                 NULL, 0, NULL },
   { "Sprites &B  (F7)",     video_layers_menu_sprites_b,                 NULL, 0, NULL },
   { "B&ackground (F8)",     video_layers_menu_background,                NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Hide Hor. Scrolling", video_layers_menu_hide_horizontal_scrolling, NULL, 0, NULL },
   { "Hide &Ver. Scrolling", video_layers_menu_hide_vertical_scrolling,   NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Flip Mirroring",      video_layers_menu_flip_mirroring,            NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_palette_menu_ntsc_color);
DEFINE_MENU_CALLBACK(video_palette_menu_ntsc_grayscale);
DEFINE_MENU_CALLBACK(video_palette_menu_gnuboy);
DEFINE_MENU_CALLBACK(video_palette_menu_nester);
DEFINE_MENU_CALLBACK(video_palette_menu_nesticle);
DEFINE_MENU_CALLBACK(video_palette_menu_modern_ntsc);
DEFINE_MENU_CALLBACK(video_palette_menu_modern_pal);
DEFINE_MENU_CALLBACK(video_palette_menu_ega_mode_1);
DEFINE_MENU_CALLBACK(video_palette_menu_ega_mode_2);
DEFINE_MENU_CALLBACK(video_palette_menu_custom);

static const MENU video_palette_menu_base[] =
{
   { "&NTSC Color",     video_palette_menu_ntsc_color,     NULL, 0, NULL },
   { "NTSC &Grayscale", video_palette_menu_ntsc_grayscale, NULL, 0, NULL },
   { "gn&uboy",         video_palette_menu_gnuboy,         NULL, 0, NULL },
   { "N&ESter",         video_palette_menu_nester,         NULL, 0, NULL },
   { "NE&Sticle",       video_palette_menu_nesticle,       NULL, 0, NULL },
   { "&Modern NTSC",    video_palette_menu_modern_ntsc,    NULL, 0, NULL },
   { "Modern &PAL",     video_palette_menu_modern_pal,     NULL, 0, NULL },
   { "EG&A (Mode 1)",   video_palette_menu_ega_mode_1,     NULL, 0, NULL },
   { "EGA (M&ode 2)",   video_palette_menu_ega_mode_2,     NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Custom",         video_palette_menu_custom,         NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(video_menu_fullscreen);
DEFINE_MENU_CALLBACK(video_menu_page_buffer);
DEFINE_MENU_CALLBACK(video_menu_vsync);
DEFINE_MENU_CALLBACK(video_menu_color);

static const MENU video_menu_base[] =
{                    
   { "&Fullscreen",  video_menu_fullscreen,     NULL,                                0, NULL },
   MENU_SPLITTER,
   { "&Page Buffer", video_menu_page_buffer,    NULL,                                0, NULL },
   { "&VSync",       video_menu_vsync,          NULL,                                0, NULL },
   MENU_SPLITTER,                                                                    
   { "&Driver",      NULL,                      IMPORT_MENU(video_driver_menu),      0, NULL },
   { "&Resolution",  NULL,                      IMPORT_MENU(video_resolution_menu),  0, NULL },
   { "&Color Depth", NULL,                      IMPORT_MENU(video_color_depth_menu), 0, NULL },
   { "&Buffer",      NULL,                      IMPORT_MENU(video_buffer_menu),      0, NULL },
   MENU_SPLITTER,          
   { "B&litter",     NULL,                      IMPORT_MENU(video_blitter_menu),     0, NULL },
   { "F&ilters",     NULL,                      IMPORT_MENU(video_filters_menu),     0, NULL },
   MENU_SPLITTER,
   { "L&ayers",      NULL,                      IMPORT_MENU(video_layers_menu),      0, NULL },
   MENU_SPLITTER,
   { "Pal&ette",     NULL,                      IMPORT_MENU(video_palette_menu),     0, NULL },
   { "C&olor...",    video_menu_color,          NULL,                                0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(options_input_menu_enable_zapper);
DEFINE_MENU_CALLBACK(options_input_menu_configure);

static const MENU options_input_menu_base[] =
{
   { "&Enable Zapper", options_input_menu_enable_zapper, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&Configure...",  options_input_menu_configure,     NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(options_cpu_usage_menu_passive);
DEFINE_MENU_CALLBACK(options_cpu_usage_menu_normal);
DEFINE_MENU_CALLBACK(options_cpu_usage_menu_aggressive);

static const MENU options_cpu_usage_menu_base[] =
{
   { "&Passive/Laptop", options_cpu_usage_menu_passive,    NULL, 0, NULL },
   { "&Normal",         options_cpu_usage_menu_normal,     NULL, 0, NULL },
   { "&Aggressive",     options_cpu_usage_menu_aggressive, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(options_gui_theme_menu_classic);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_stainless_steel);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_zero_4);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_panta);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_xodiac);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_monochrome);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_essence);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_voodoo);
DEFINE_MENU_CALLBACK(options_gui_theme_menu_hugs_and_kisses);

static const MENU options_gui_theme_menu_base[] =
{
   { "Complete Themes",       NULL,                                   NULL, 0, NULL },
   { "  &1: Classic",         options_gui_theme_menu_classic,         NULL, 0, NULL },
   { "  &2: stainless Steel", options_gui_theme_menu_stainless_steel, NULL, 0, NULL },
   { "  &3: Zero 4",          options_gui_theme_menu_zero_4,          NULL, 0, NULL },
   { "  &4: Panta",           options_gui_theme_menu_panta,           NULL, 0, NULL },
   MENU_SPLITTER,
   { "Color Themes",          NULL,                                   NULL, 0, NULL },
   { "  &5: Xodiac",          options_gui_theme_menu_xodiac,          NULL, 0, NULL },
   { "  &6: Monochrome",      options_gui_theme_menu_monochrome,      NULL, 0, NULL },
   { "  &7: Essence",         options_gui_theme_menu_essence,         NULL, 0, NULL },
   { "  &8: Voodoo",          options_gui_theme_menu_voodoo,          NULL, 0, NULL },
   { "  &9: Hugs and Kisses", options_gui_theme_menu_hugs_and_kisses, NULL, 0, NULL },
   MENU_ENDCAP
};

DEFINE_MENU_CALLBACK(options_menu_show_status);
DEFINE_MENU_CALLBACK(options_menu_reset_clock);
DEFINE_MENU_CALLBACK(options_menu_paths);

static const MENU options_menu_base[] =
{
   { "&Show Status (F2)", options_menu_show_status,  NULL,                                0, NULL },
   { "&Reset Clock",      options_menu_reset_clock,  NULL,                                0, NULL },
   MENU_SPLITTER,
   { "&Input",            NULL,                      IMPORT_MENU(options_input_menu),     0, NULL },
#ifndef ALLEGRO_DOS
   { "&CPU Usage",        NULL,                      IMPORT_MENU(options_cpu_usage_menu), 0, NULL },
   MENU_SPLITTER,
#endif
   { "&GUI Theme",        NULL,                      IMPORT_MENU(options_gui_theme_menu), 0, NULL },
   MENU_SPLITTER,
   { "&Paths...",         options_menu_paths,        NULL,                                0, NULL },
   MENU_ENDCAP      
};

DEFINE_MENU_CALLBACK(help_menu_shortcuts);
DEFINE_MENU_CALLBACK(help_menu_about);
DEFINE_MENU_CALLBACK(help_menu_version);

static const MENU help_menu_base[] =
{
   { "&Shortcuts...", help_menu_shortcuts, NULL, 0, NULL },
   MENU_SPLITTER,
   { "&About...",     help_menu_about,     NULL, 0, NULL },
   { "&Version...",   help_menu_version,     NULL, 0, NULL },
   MENU_ENDCAP
};

static const MENU top_menu_base[] =
{ 
   { "&Main",    NULL, IMPORT_MENU(main_menu),    0, NULL },
   { "M&achine", NULL, IMPORT_MENU(machine_menu), 0, NULL },
   { "A&udio",   NULL, IMPORT_MENU(audio_menu),   0, NULL },
   { "&Video",   NULL, IMPORT_MENU(video_menu),   0, NULL },
   { "&Options", NULL, IMPORT_MENU(options_menu), 0, NULL },
#ifdef USE_HAWKNL
   { "&NetPlay", NULL, IMPORT_MENU(netplay_menu), 0, NULL },
#endif
   { "&Help",    NULL, IMPORT_MENU(help_menu),    0, NULL },
   MENU_ENDCAP
};

/* Undefine helper macros. */
#undef DEFINE_MENU
#undef DEFINE_MENU_CALLBACK
#undef IMPORT_MENU
#undef MENU_SPLITTER
#undef MENU_ENDCAP

#define SET_FLAG(var, flag, value) \
   if (value)  \
      var |= flag;   \
   else  \
      var &= ~ flag; \

enum
{
   MENU_PROPERTY_CHECKED,
   MENU_PROPERTY_ENABLED
};

static INLINE void set_menu_object_property (MENU *object, ENUM property,
   BOOL value)
{
   /* This function sets the boolean property 'property' of the menu or menu
      item specified by 'menu' to 'value'. */

   RT_ASSERT(object);

   switch (property)
   {
      case MENU_PROPERTY_CHECKED:
      {
         SET_FLAG(object->flags, D_SELECTED, value);

         break;
      }

      case MENU_PROPERTY_ENABLED:
      {
         SET_FLAG(object->flags, D_DISABLED, !value);

         break;
      }

      default:
         WARN_GENERIC();
   }
}

#undef SET_FLAG

static INLINE BOOL set_menu_property (MENU *menu, void *callback, ENUM
   property, BOOL value)
{
   /* Recursive function that searches through the menu specified in 'menu'
      to find a menu item with a callback function matching 'callback', then
      sets it's boolean property specified by 'property' to 'value'.
      Returns TRUE if the item was found and all recursion levels should
      break out of the search, or FALSE if the item was not found and the
      search should continue. */

   int index = 0;

   RT_ASSERT(menu);
   RT_ASSERT(callback);

   while (menu[index].text || menu[index].proc)
   {
      MENU *item = &menu[index];

      if (item->child)
      {
         /* Recurse to the next sublevel. */
         if (set_menu_property (item->child, callback, property, value))
            return (TRUE);
      }
      else
      {
         if (item->proc == callback)
         {
            set_menu_object_property (item, property, value);

            return (TRUE);
         }
      }

      index++;
   }

   return (FALSE);
}

static INLINE BOOL set_submenu_property (MENU *menu, const MENU *submenu,
   ENUM property, BOOL value)
{
   /* Recursive function that searches through the menu specified in 'menu'
      to find the first reference to the submenu specified in 'submenu' and
      sets it's boolean property specified by 'property' to 'value'.
      Returns TRUE if the item was found and all recursion levels should
      break out of the search, or FALSE if the item was not found and the
      search should continue. */

   int index = 0;

   RT_ASSERT(menu);
   RT_ASSERT(submenu);

   while (menu[index].text || menu[index].proc)
   {
      MENU *item = &menu[index];

      if (item->child)
      {
         if (item->child == submenu)
         {
            set_menu_object_property (item, property, value);
   
            return (TRUE);
         }
         else
         {
            /* Recurse to the next sublevel. */
            if (set_submenu_property (item->child, submenu, property,
               value))
            {
               return (TRUE);
            }
         }
      }

      index++;
   }

   return (FALSE);
}

#define SET_MENU_ITEM_CHECKED(item, checked) \
   set_menu_property (top_menu, item, MENU_PROPERTY_CHECKED, checked)
#define SET_MENU_ITEM_ENABLED(item, enabled) \
   set_menu_property (top_menu, item, MENU_PROPERTY_ENABLED, enabled)
#define SET_SUBMENU_ENABLED(menu, submenu, enabled)   \
   set_submenu_property (menu, submenu, MENU_PROPERTY_ENABLED, enabled)

#define CHECK_MENU_ITEM(item)          SET_MENU_ITEM_CHECKED(item, TRUE)
#define UNCHECK_MENU_ITEM(item)        SET_MENU_ITEM_CHECKED(item, FALSE)
#define TOGGLE_MENU_ITEM               SET_MENU_ITEM_CHECKED
#define ENABLE_MENU_ITEM(item)         SET_MENU_ITEM_ENABLED(item, TRUE)
#define DISABLE_MENU_ITEM(item)        SET_MENU_ITEM_ENABLED(item, FALSE)
#define ENABLE_SUBMENU(submenu)  \
   SET_SUBMENU_ENABLED(top_menu, submenu, TRUE)
#define DISABLE_SUBMENU(submenu) \
   SET_SUBMENU_ENABLED(top_menu, submenu, FALSE)

static INLINE MENU *load_menu (const MENU *menu)
{
   MENU *new_menu;
   int size = 0;
   int index = 0;

   RT_ASSERT(menu);

   while (menu[index].text || menu[index].proc)
   {
       size += sizeof (MENU);

       index++;
   }

   /* Once more for the end marker. */
   size += sizeof (MENU);

   if (!(new_menu = malloc (size)))
   {
      WARN("Failed to allocate menu structure");

      return (NULL);
   }

   memcpy (new_menu, menu, size);

   /* Reset counter. */
   index = 0;

   while (new_menu[index].text || new_menu[index].proc)
   {
      MENU *item = &new_menu[index];

      /* Import menu by reference. */
      if (item->child)
         item->child = *(MENU **)item->child;

      index++;
   }

   return (new_menu);
}

static INLINE void unload_menu (MENU *menu)
{
   RT_ASSERT(menu);

   free (menu);
}

#define MENU_FROM_BASE(name)  (name = load_menu (name ##_base))

static INLINE void load_menus (void)
{
   MENU_FROM_BASE(main_open_recent_menu);
   MENU_FROM_BASE(main_replay_select_menu);
   MENU_FROM_BASE(main_replay_record_menu);
   MENU_FROM_BASE(main_replay_play_menu);
   MENU_FROM_BASE(main_replay_menu);
   MENU_FROM_BASE(main_menu);
   MENU_FROM_BASE(machine_save_state_select_menu);
   MENU_FROM_BASE(machine_save_state_autosave_menu);
   MENU_FROM_BASE(machine_save_state_menu);
   MENU_FROM_BASE(machine_region_menu);
   MENU_FROM_BASE(machine_speed_up_down_menu);
   MENU_FROM_BASE(machine_frame_skip_menu);
   MENU_FROM_BASE(machine_menu);
   MENU_FROM_BASE(audio_output_buffer_menu);
   MENU_FROM_BASE(audio_output_menu);
   MENU_FROM_BASE(audio_channels_menu);
   MENU_FROM_BASE(audio_record_menu);
   MENU_FROM_BASE(audio_menu);
   MENU_FROM_BASE(video_driver_dos_menu);
   MENU_FROM_BASE(video_driver_windows_menu);
   MENU_FROM_BASE(video_driver_linux_menu);
   MENU_FROM_BASE(video_driver_unix_menu);
   MENU_FROM_BASE(video_driver_menu);
   MENU_FROM_BASE(video_resolution_proportionate_menu);
   MENU_FROM_BASE(video_resolution_menu);
   MENU_FROM_BASE(video_color_depth_menu);
   MENU_FROM_BASE(video_buffer_menu);
   MENU_FROM_BASE(video_blitter_menu);
   MENU_FROM_BASE(video_filters_menu);
   MENU_FROM_BASE(video_layers_menu);
   MENU_FROM_BASE(video_palette_menu);
   MENU_FROM_BASE(video_menu);
   MENU_FROM_BASE(options_input_menu);
   MENU_FROM_BASE(options_cpu_usage_menu);
   MENU_FROM_BASE(options_gui_theme_menu);
   MENU_FROM_BASE(options_menu);
   MENU_FROM_BASE(netplay_menu);
   MENU_FROM_BASE(help_menu);
   MENU_FROM_BASE(top_menu);
}

#undef MENU_FROM_BASE

static INLINE void unload_menus (void)
{
   unload_menu (main_open_recent_menu);
   unload_menu (main_replay_select_menu);
   unload_menu (main_replay_record_menu);
   unload_menu (main_replay_play_menu);
   unload_menu (main_replay_menu);
   unload_menu (main_menu);
   unload_menu (machine_save_state_select_menu);
   unload_menu (machine_save_state_autosave_menu);
   unload_menu (machine_save_state_menu);
   unload_menu (machine_region_menu);
   unload_menu (machine_speed_up_down_menu);
   unload_menu (machine_frame_skip_menu);
   unload_menu (machine_menu);
   unload_menu (audio_output_buffer_menu);
   unload_menu (audio_output_menu);
   unload_menu (audio_channels_menu);
   unload_menu (audio_record_menu);
   unload_menu (audio_menu);
   unload_menu (video_driver_dos_menu);
   unload_menu (video_driver_windows_menu);
   unload_menu (video_driver_linux_menu);
   unload_menu (video_driver_unix_menu);
   unload_menu (video_driver_menu);
   unload_menu (video_resolution_proportionate_menu);
   unload_menu (video_resolution_menu);
   unload_menu (video_color_depth_menu);
   unload_menu (video_buffer_menu);
   unload_menu (video_blitter_menu);
   unload_menu (video_filters_menu);
   unload_menu (video_layers_menu);
   unload_menu (video_palette_menu);
   unload_menu (video_menu);
   unload_menu (options_input_menu);
   unload_menu (options_cpu_usage_menu);
   unload_menu (options_gui_theme_menu);
   unload_menu (options_menu);
   unload_menu (netplay_menu);
   unload_menu (help_menu);
   unload_menu (top_menu);
}
