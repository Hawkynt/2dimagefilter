/**********************************************************************************
  Snes9x - Portable Super Nintendo Entertainment System (TM) emulator.

  (c) Copyright 1996 - 2002  Gary Henderson (gary.henderson@ntlworld.com),
                             Jerremy Koot (jkoot@snes9x.com)

  (c) Copyright 2002 - 2004  Matthew Kendora

  (c) Copyright 2002 - 2005  Peter Bortas (peter@bortas.org)

  (c) Copyright 2004 - 2005  Joel Yliluoma (http://iki.fi/bisqwit/)

  (c) Copyright 2001 - 2006  John Weidman (jweidman@slip.net)

  (c) Copyright 2002 - 2006  funkyass (funkyass@spam.shaw.ca),
                             Kris Bleakley (codeviolation@hotmail.com)

  (c) Copyright 2002 - 2007  Brad Jorsch (anomie@users.sourceforge.net),
                             Nach (n-a-c-h@users.sourceforge.net),
                             zones (kasumitokoduck@yahoo.com)

  (c) Copyright 2006 - 2007  nitsuja


  BS-X C emulator code
  (c) Copyright 2005 - 2006  Dreamer Nom,
                             zones

  C4 x86 assembler and some C emulation code
  (c) Copyright 2000 - 2003  _Demo_ (_demo_@zsnes.com),
                             Nach,
                             zsKnight (zsknight@zsnes.com)

  C4 C++ code
  (c) Copyright 2003 - 2006  Brad Jorsch,
                             Nach

  DSP-1 emulator code
  (c) Copyright 1998 - 2006  _Demo_,
                             Andreas Naive (andreasnaive@gmail.com)
                             Gary Henderson,
                             Ivar (ivar@snes9x.com),
                             John Weidman,
                             Kris Bleakley,
                             Matthew Kendora,
                             Nach,
                             neviksti (neviksti@hotmail.com)

  DSP-2 emulator code
  (c) Copyright 2003         John Weidman,
                             Kris Bleakley,
                             Lord Nightmare (lord_nightmare@users.sourceforge.net),
                             Matthew Kendora,
                             neviksti


  DSP-3 emulator code
  (c) Copyright 2003 - 2006  John Weidman,
                             Kris Bleakley,
                             Lancer,
                             z80 gaiden

  DSP-4 emulator code
  (c) Copyright 2004 - 2006  Dreamer Nom,
                             John Weidman,
                             Kris Bleakley,
                             Nach,
                             z80 gaiden

  OBC1 emulator code
  (c) Copyright 2001 - 2004  zsKnight,
                             pagefault (pagefault@zsnes.com),
                             Kris Bleakley,
                             Ported from x86 assembler to C by sanmaiwashi

  SPC7110 and RTC C++ emulator code
  (c) Copyright 2002         Matthew Kendora with research by
                             zsKnight,
                             John Weidman,
                             Dark Force

  S-DD1 C emulator code
  (c) Copyright 2003         Brad Jorsch with research by
                             Andreas Naive,
                             John Weidman

  S-RTC C emulator code
  (c) Copyright 2001-2006    byuu,
                             John Weidman

  ST010 C++ emulator code
  (c) Copyright 2003         Feather,
                             John Weidman,
                             Kris Bleakley,
                             Matthew Kendora

  Super FX x86 assembler emulator code
  (c) Copyright 1998 - 2003  _Demo_,
                             pagefault,
                             zsKnight,

  Super FX C emulator code
  (c) Copyright 1997 - 1999  Ivar,
                             Gary Henderson,
                             John Weidman

  Sound DSP emulator code is derived from SNEeSe and OpenSPC:
  (c) Copyright 1998 - 2003  Brad Martin
  (c) Copyright 1998 - 2006  Charles Bilyue'

  SH assembler code partly based on x86 assembler code
  (c) Copyright 2002 - 2004  Marcus Comstedt (marcus@mc.pp.se)

  2xSaI filter
  (c) Copyright 1999 - 2001  Derek Liauw Kie Fa

  HQ2x, HQ3x, HQ4x filters
  (c) Copyright 2003         Maxim Stepin (maxim@hiend3d.com)

  Win32 GUI code
  (c) Copyright 2003 - 2006  blip,
                             funkyass,
                             Matthew Kendora,
                             Nach,
                             nitsuja

  Mac OS GUI code
  (c) Copyright 1998 - 2001  John Stiles
  (c) Copyright 2001 - 2007  zones


  Specific ports contains the works of other authors. See headers in
  individual files.


  Snes9x homepage: http://www.snes9x.com

  Permission to use, copy, modify and/or distribute Snes9x in both binary
  and source form, for non-commercial purposes, is hereby granted without
  fee, providing that this license information and copyright notice appear
  with all copies and any derived work.

  This software is provided 'as-is', without any express or implied
  warranty. In no event shall the authors be held liable for any damages
  arising from the use of this software or it's derivatives.

  Snes9x is freeware for PERSONAL USE only. Commercial users should
  seek permission of the copyright holders first. Commercial use includes,
  but is not limited to, charging money for Snes9x or software derived from
  Snes9x, including Snes9x or derivatives in commercial game bundles, and/or
  using Snes9x as a promotion for your commercial product.

  The copyright holders request that bug fixes and improvements to the code
  should be forwarded to them so everyone can benefit from the modifications
  in future versions.

  Super NES and Super Nintendo Entertainment System are trademarks of
  Nintendo Co., Limited and its subsidiary companies.
**********************************************************************************/

#include "../snes9x.h"
#include "../memmap.h"
#include "../debug.h"
#include "../cpuexec.h"
#include "../ppu.h"
#include "../snapshot.h"
#include "../apu.h"
#include "../display.h"
#include "../gfx.h"
#include "../soundux.h"
#include "../movie.h"
#include "../netplay.h"
#include "../s9xlua.h"
#include "../screenshot.h"

#include "wsnes9x.h"
#include "CDirectDraw.h"
#include "CDirectSound.h"

#include "render.h"
#include "WAVOutput.h"
#include "AVIOutput.h"
#include "wlanguage.h"
#include "lazymacro.h"
#include <direct.h>

#include <io.h>
//#define DEBUGGER

#include "direct3d.h"
extern CDirect3D Direct3D;

//#define GENERATE_OFFSETS_H

//#ifdef GENERATE_OFFSETS_H
//FILE *offsets_h = NULL;
//#define main generate_offsets_h
//#define S9xSTREAM offsets_h
//#include "offsets.cpp"
//#endif

struct SJoyState Joystick [16];
uint32 joypads [8];
bool8 do_frame_adjust=false;

// avi variables
static uint8* avi_buffer = NULL;
//static uint8* avi_sound_buffer = NULL;
static int avi_sound_bytes_per_sample = 0;
//static int avi_sound_samples_per_update = 0;
static int avi_width = 0;
static int avi_height = 0;
static int avi_pitch = 0;
static int avi_image_size = 0;
static uint32 avi_skip_frames = 0;
//void Convert8To24 (SSurface *src, SSurface *dst, RECT *srect);
void Convert16To24 (SSurface *src, SSurface *dst, RECT *srect);
void Convert16To32 (SSurface *src, SSurface *dst, RECT *srect);
void DoWAVOpen(const char* filename);
void DoWAVClose(int reason);
void DoAVIOpen(const char* filename);
void DoAVIClose(int reason);
static void DoAVIVideoFrame(void);
static void BuildAVIVideoFrame1X (void);
static void BuildAVIVideoFrame2X (void);
static void BuildAVIVideoFrame1XHiRes (void);
static void BuildAVIVideoFrame2XHiRes (void);
bool ReInitSound(int mode);

#define BMP_PITCH(width, bpp)   ((((width)*(bpp)+31)/8)&~3)

void S9xUpdateJoyState ();
void S9xWinScanJoypads ();

typedef struct
{
    uint8 red;
    uint8 green;
    uint8 blue;
} Colour;

void ConvertDepth (SSurface *src, SSurface *dst, RECT *);
static Colour FixedColours [256];
static uint8 palette [0x10000];

FILE *trace_fs = NULL;

int __fastcall Normalize (int cur, int min, int max)
{
    int Result = 0;

    if ((max - min) == 0)
        return (Result);

    Result = cur - min;
    Result = (Result * 200) / (max - min);
    Result -= 100;

    return (Result);
}

void S9xTextMode( void)
{
}

void S9xGraphicsMode ()
{
}

void S9xExit( void)
{
	if(Settings.SPC7110)
		(*CleanUp7110)();
	S9xLuaStop();
    SendMessage (GUI.hWnd, WM_COMMAND, ID_FILE_EXIT, 0);
}

const char *S9xGetFilename (const char *ex, enum s9x_getdirtype dirtype)
{
    static char filename [PATH_MAX + 1];
    char dir [_MAX_DIR + 1];
    char drive [_MAX_DRIVE + 1];
    char fname [_MAX_FNAME + 1];
    char ext [_MAX_EXT + 1];
   _splitpath (Memory.ROMFilename, drive, dir, fname, ext);
   _snprintf(filename, sizeof(filename), "%s" SLASH_STR "%s%s",
             S9xGetDirectory(dirtype), fname, ex);
    return (filename);
}

const char *S9xGetFilenameRel (const char *ex)
{
	static char filename [PATH_MAX + 1];
	char dir [_MAX_DIR + 1];
	char drive [_MAX_DRIVE + 1];
	char fname [_MAX_FNAME + 1];
	char ext [_MAX_EXT + 1];

	if (Memory.ROMFilename[0] == '\0')
		strcpy(filename, "");
	else {
		_splitpath (Memory.ROMFilename, drive, dir, fname, ext);
		_makepath (filename, "", "", fname, ex);
	}
	return filename;
}
const void S9xGetLastDirectory (char* buffer, int buf_len)
{
	if(buf_len <= 0)
		return;

	GetCurrentDirectory(buf_len, buffer);
}

#define IS_SLASH(x) ((x) == '\\' || (x) == '/')
static char startDirectory [PATH_MAX];
static bool startDirectoryValid = false;

// Note: S9xGetDirectory changes the current directory
const char *S9xGetDirectory (enum s9x_getdirtype dirtype)
{
//	_fullpath
	if(!startDirectoryValid)
	{
		// directory from which the executable was launched
//		GetCurrentDirectory(PATH_MAX, startDirectory);

		// directory of the executable's location:
		GetModuleFileName(NULL, startDirectory, PATH_MAX);
        for(int i=strlen(startDirectory); i>=0; i--){
            if(IS_SLASH(startDirectory[i])){
                startDirectory[i]='\0';
                break;
            }
        }

		startDirectoryValid = true;
	}

	SetCurrentDirectory(startDirectory); // makes sure relative paths are relative to the application's location

	const char* rv = startDirectory;

    switch(dirtype){
	  default:
      case DEFAULT_DIR:
	  case HOME_DIR:
		  break;

	  case SCREENSHOT_DIR:
		  rv = GUI.ScreensDir;
		  break;

      case ROM_DIR:
		  rv = GUI.RomDir;
		  break;

      case SRAM_DIR:
		  rv = GUI.SRAMFileDir;
		  break;

	  case BIOS_DIR:
		  rv = GUI.BiosDir;
		  break;

      case SPC_DIR:
		  rv = GUI.SPCDir;
		  break;

	  case PATCH_DIR:
	  case CHEAT_DIR:
		  rv = GUI.PatchDir;
		  break;

	  case SNAPSHOT_DIR:
		  rv = GUI.FreezeFileDir;
		  break;

	  case ROMFILENAME_DIR: {
			static char filename [PATH_MAX];
			strcpy(filename, Memory.ROMFilename);
			if(!filename[0])
				rv = GUI.RomDir;
			for(int i=strlen(filename); i>=0; i--){
				if(IS_SLASH(filename[i])){
					filename[i]='\0';
					break;
				}
			}
			rv = filename;
		}
		break;
    }

	mkdir(rv);

	return rv;
}

///*extern "C"*/ const char *S9xGetFilename (const char *e)
//{
//    static char filename [_MAX_PATH + 1];
//    char drive [_MAX_DRIVE + 1];
//    char dir [_MAX_DIR + 1];
//    char fname [_MAX_FNAME + 1];
//    char ext [_MAX_EXT + 1];
//
//    if (strlen (GUI.FreezeFileDir))
//    {
//        _splitpath (Memory.ROMFilename, drive, dir, fname, ext);
//        strcpy (filename, GUI.FreezeFileDir);
//        strcat (filename, TEXT("\\"));
//        strcat (filename, fname);
//        strcat (filename, e);
//    }
//    else
//    {
//        _splitpath (Memory.ROMFilename, drive, dir, fname, ext);
//        _makepath (filename, drive, dir, fname, e);
//    }
//
//    return (filename);
//}

const char *S9xGetFilenameInc (const char *e, enum s9x_getdirtype dirtype)
{
    static char filename [PATH_MAX + 1];
    char dir [_MAX_DIR + 1];
    char drive [_MAX_DRIVE + 1];
    char fname [_MAX_FNAME + 1];
    char ext [_MAX_EXT + 1];
    unsigned int i=0;
    const char *d;

    _splitpath (Memory.ROMFilename, drive, dir, fname, ext);
    d=S9xGetDirectory(dirtype);
    do {
        _snprintf(filename, sizeof(filename), "%s\\%s%03d%s", d, fname, i, e);
        i++;
    } while(_access (filename, 0) == 0 && i!=0);

    return (filename);
}

bool8 S9xOpenSnapshotFile( const char *fname, bool8 read_only, STREAM *file)
{
    char filename [_MAX_PATH + 1];
    char drive [_MAX_DRIVE + 1];
    char dir [_MAX_DIR + 1];
    char fn [_MAX_FNAME + 1];
    char ext [_MAX_EXT + 1];

    _splitpath( fname, drive, dir, fn, ext);
    _makepath( filename, drive, dir, fn, ext[0] == '\0' ? ".000" : ext);

    if (read_only)
    {
	if ((*file = OPEN_STREAM (filename, "rb")))
	    return (TRUE);
    }
    else
    {
	if ((*file = OPEN_STREAM (filename, "wb")))
	    return (TRUE);
        FILE *fs = fopen (filename, "rb");
        if (fs)
        {
            sprintf (String, "Freeze file \"%s\" exists but is read only",
                     filename);
            fclose (fs);
            S9xMessage (S9X_ERROR, S9X_FREEZE_FILE_NOT_FOUND, String);
        }
        else
        {
            sprintf (String, "Cannot create freeze file \"%s\". Directory is read-only or does not exist.", filename);

            S9xMessage (S9X_ERROR, S9X_FREEZE_FILE_NOT_FOUND, String);
        }
    }
    return (FALSE);
}

void S9xCloseSnapshotFile( STREAM file)
{
    CLOSE_STREAM (file);
}

void S9xMessage (int type, int, const char *str)
{
#ifdef DEBUGGER
    static FILE *out = NULL;

    if (out == NULL)
        out = fopen ("out.txt", "w");

    fprintf (out, "%s\n", str);
#endif

    S9xSetInfoString (str);

	// if we can't draw on the screen, messagebox it
	// also send to stderr/stdout depending on message type
	switch(type)
	{
		case S9X_INFO:
			if(Settings.StopEmulation)
				fprintf(stdout, "%s\n", str);
			break;
		case S9X_WARNING:
			fprintf(stdout, "%s\n", str);
			if(Settings.StopEmulation)
				MessageBox(GUI.hWnd, str, "Warning",     MB_OK | MB_ICONWARNING);
			break;
		case S9X_ERROR:
			fprintf(stderr, "%s\n", str);
			if(Settings.StopEmulation)
				MessageBox(GUI.hWnd, str, "Error",       MB_OK | MB_ICONERROR);
			break;
		case S9X_FATAL_ERROR:
			fprintf(stderr, "%s\n", str);
			if(Settings.StopEmulation)
				MessageBox(GUI.hWnd, str, "Fatal Error", MB_OK | MB_ICONERROR);
			break;
		default:
				fprintf(stdout, "%s\n", str);
			break;
	}
}

extern uint8 *syncSoundBuffer;

static RECT dstRect = { 0, 512, 0, 448 };

extern unsigned long START;

void S9xSyncSpeed( void)
{
#ifdef NETPLAY_SUPPORT
    if (Settings.NetPlay)
    {
#if defined (NP_DEBUG) && NP_DEBUG == 2
        printf ("CLIENT: SyncSpeed @%d\n", timeGetTime () - START);
#endif
        S9xWinScanJoypads ();

		LONG prev;
        BOOL success;

	// Wait for heart beat from server
        if ((success = ReleaseSemaphore (GUI.ClientSemaphore, 1, &prev)) &&
            prev == 0)
        {
            // No heartbeats already arrived, have to wait for one.
            // Mop up the ReleaseSemaphore test above...
            WaitForSingleObject (GUI.ClientSemaphore, 0);

            // ... and then wait for the real sync-signal from the
            // client loop thread.
            NetPlay.PendingWait4Sync = WaitForSingleObject (GUI.ClientSemaphore, 100) != WAIT_OBJECT_0;
#if defined (NP_DEBUG) && NP_DEBUG == 2
            if (NetPlay.PendingWait4Sync)
                printf ("CLIENT: PendingWait4Sync1 @%d\n", timeGetTime () - START);
#endif
            IPPU.RenderThisFrame = TRUE;
            IPPU.SkippedFrames = 0;
        }
        else
        {
            if (success)
            {
                // Once for the ReleaseSemaphore above...
                WaitForSingleObject (GUI.ClientSemaphore, 0);
                if (prev == 4 && NetPlay.Waiting4EmulationThread)
                {
                    // Reached the lower behind count threshold - tell the
                    // server its safe to start sending sync pulses again.
                    NetPlay.Waiting4EmulationThread = FALSE;
                    S9xNPSendPause (FALSE);
                }

#if defined (NP_DEBUG) && NP_DEBUG == 2
                if (prev > 1)
                {
                    printf ("CLIENT: SyncSpeed prev: %d @%d\n", prev, timeGetTime () - START);
                }
#endif
            }
            else
            {
#ifdef NP_DEBUG
                printf ("*** CLIENT: SyncSpeed: Release failed @ %d\n", timeGetTime () - START);
#endif
            }

            // ... and again to mop up the already-waiting sync-signal
            NetPlay.PendingWait4Sync = WaitForSingleObject (GUI.ClientSemaphore, 200) != WAIT_OBJECT_0;
#if defined (NP_DEBUG) && NP_DEBUG == 2
            if (NetPlay.PendingWait4Sync)
                printf ("CLIENT: PendingWait4Sync2 @%d\n", timeGetTime () - START);
#endif

	    if (IPPU.SkippedFrames < NetPlay.MaxFrameSkip)
	    {
		IPPU.SkippedFrames++;
		IPPU.RenderThisFrame = FALSE;
	    }
	    else
	    {
		IPPU.RenderThisFrame = TRUE;
		IPPU.SkippedFrames = 0;
	    }
        }
        // Give up remainder of time-slice to any other waiting threads,
        // if they need any time, that is.
        Sleep (0);
        if (!NetPlay.PendingWait4Sync)
        {
            NetPlay.FrameCount++;
            S9xNPStepJoypadHistory ();
        }
    }
    else
#endif

    // Lua gets a crack at screwing with the results
    if (S9xLuaSpeed() > 0)
	return;

    if (!Settings.TurboMode && Settings.SkipFrames == AUTO_FRAMERATE &&
		!GUI.AVIOut && !Settings.HighSpeedSeek)
    {
		if (!do_frame_adjust)
		{
			IPPU.RenderThisFrame = TRUE;
			IPPU.SkippedFrames = 0;
		}
		else
		{
			if (IPPU.SkippedFrames < Settings.AutoMaxSkipFrames)
			{
				IPPU.SkippedFrames++;
				IPPU.RenderThisFrame = FALSE;
			}
			else
			{
				IPPU.RenderThisFrame = TRUE;
				IPPU.SkippedFrames = 0;
			}
		}
	}
    else
    {
	uint32 SkipFrames;
	if((Settings.TurboMode || Settings.HighSpeedSeek) && !GUI.AVIOut)
		SkipFrames = Settings.TurboSkipFrames;
	else
		SkipFrames = (Settings.SkipFrames == AUTO_FRAMERATE) ? 0 : Settings.SkipFrames;
	if (++IPPU.FrameSkip >= SkipFrames)
	{
	    IPPU.FrameSkip = 0;
	    IPPU.SkippedFrames = 0;
	    IPPU.RenderThisFrame = TRUE;
	}
	else
	{
	    IPPU.SkippedFrames++;
		IPPU.RenderThisFrame = (GUI.AVIOut!=0);
	}
    }
}

const char *S9xBasename (const char *f)
{
    const char *p;
    if ((p = strrchr (f, '/')) != NULL || (p = strrchr (f, '\\')) != NULL)
	return (p + 1);

#ifdef __DJGPP
    if (p = strrchr (f, SLASH_CHAR))
	return (p + 1);
#endif

    return (f);
}

bool8 S9xReadMousePosition (int which, int &x, int &y, uint32 &buttons)
{
    if (which == 0)
    {
        x = GUI.MouseX;
        y = GUI.MouseY;
        buttons = GUI.MouseButtons;
        return (TRUE);
    }

    return (FALSE);
}

bool S9xGetState (WORD KeyIdent)
{
	if(KeyIdent == 0 || KeyIdent == VK_ESCAPE) // if it's the 'disabled' key, it's never pressed
		return true;

	if(!GUI.BackgroundInput && GUI.hWnd != GetForegroundWindow())
		return true;

    if (KeyIdent & 0x8000) // if it's a joystick 'key':
    {
        int j = (KeyIdent >> 8) & 15;

		S9xUpdateJoyState();

        switch (KeyIdent & 0xff)
        {
            case 0: return !Joystick [j].Left;
            case 1: return !Joystick [j].Right;
            case 2: return !Joystick [j].Up;
            case 3: return !Joystick [j].Down;
            case 4: return !Joystick [j].PovLeft;
            case 5: return !Joystick [j].PovRight;
            case 6: return !Joystick [j].PovUp;
            case 7: return !Joystick [j].PovDown;
			case 49:return !Joystick [j].PovDnLeft;
			case 50:return !Joystick [j].PovDnRight;
			case 51:return !Joystick [j].PovUpLeft;
			case 52:return !Joystick [j].PovUpRight;
            case 41:return !Joystick [j].ZUp;
            case 42:return !Joystick [j].ZDown;
            case 43:return !Joystick [j].RUp;
            case 44:return !Joystick [j].RDown;
            case 45:return !Joystick [j].UUp;
            case 46:return !Joystick [j].UDown;
            case 47:return !Joystick [j].VUp;
            case 48:return !Joystick [j].VDown;

            default:
                if ((KeyIdent & 0xff) > 40)
                    return true; // not pressed

                return !Joystick [j].Button [(KeyIdent & 0xff) - 8];
        }
    }

	// the pause key is special, need this to catch all presses of it
	// Both GetKeyState and GetAsyncKeyState cannot catch it anyway,
	// so this should be handled in WM_KEYDOWN message.
	if(KeyIdent == VK_PAUSE)
	{
		return true; // not pressed
//		if(GetAsyncKeyState(VK_PAUSE)) // not &'ing this with 0x8000 is intentional and necessary
//			return false;
	}

	if(KeyIdent == VK_CAPITAL || KeyIdent == VK_NUMLOCK || KeyIdent == VK_SCROLL)
		return ((GetKeyState(KeyIdent) & 0x01) == 0);
	else
		return ((GetAsyncKeyState(KeyIdent) & 0x8000) == 0);
	//return ((GetKeyState (KeyIdent) & 0x80) == 0);
}

void CheckAxis (int val, int min, int max, bool &first, bool &second)
{
    if (Normalize (val, min, max) < -S9X_JOY_NEUTRAL)
    {
        second = false;
        first = true;
    }
    else
        first = false;

    if (Normalize (val, min, max) > S9X_JOY_NEUTRAL)
    {
        first = false;
        second = true;
    }
    else
        second = false;
}

void S9xUpdateJoyState ()
{
	JOYINFOEX jie;

	for (int C = 0; C != 16; C ++)
	{
		if (Joystick[C].Attached)
		{
			jie.dwSize = sizeof (jie);
			jie.dwFlags = JOY_RETURNALL;

			if (joyGetPosEx (JOYSTICKID1+C, &jie) != JOYERR_NOERROR)
			{
				Joystick[C].Attached = false;
				continue;
			}

			CheckAxis (jie.dwXpos,
			           Joystick[C].Caps.wXmin, Joystick[C].Caps.wXmax,
			           Joystick[C].Left, Joystick[C].Right);
			CheckAxis (jie.dwYpos,
			           Joystick[C].Caps.wYmin, Joystick[C].Caps.wYmax,
			           Joystick[C].Up, Joystick[C].Down);
			CheckAxis (jie.dwZpos,
			           Joystick[C].Caps.wZmin, Joystick[C].Caps.wZmax,
			           Joystick[C].ZUp, Joystick[C].ZDown);
			CheckAxis (jie.dwRpos,
			           Joystick[C].Caps.wRmin, Joystick[C].Caps.wRmax,
			           Joystick[C].RUp, Joystick[C].RDown);
			CheckAxis (jie.dwUpos,
			           Joystick[C].Caps.wUmin, Joystick[C].Caps.wUmax,
			           Joystick[C].UUp, Joystick[C].UDown);
			CheckAxis (jie.dwVpos,
			           Joystick[C].Caps.wVmin, Joystick[C].Caps.wVmax,
			           Joystick[C].VUp, Joystick[C].VDown);

			switch (jie.dwPOV)
			{
				case JOY_POVBACKWARD:
					Joystick[C].PovDown = true;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = false;
					 break;
				case 4500:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = true;
					break;
				case 13500:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = true;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = false;
					break;
				case 22500:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = true;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = false;
					break;
				case 31500:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = true;
					Joystick[C].PovUpRight = false;
					break;

				case JOY_POVFORWARD:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = true;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = false;
					break;

				case JOY_POVLEFT:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = true;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = false;
					break;

				case JOY_POVRIGHT:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = true;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = false;
					break;

				default:
					Joystick[C].PovDown = false;
					Joystick[C].PovUp = false;
					Joystick[C].PovLeft = false;
					Joystick[C].PovRight = false;
					Joystick[C].PovDnLeft = false;
					Joystick[C].PovDnRight = false;
					Joystick[C].PovUpLeft = false;
					Joystick[C].PovUpRight = false;
					break;
			}

			for (int B = 0; B < 32; B ++)
				Joystick[C].Button[B] = (jie.dwButtons & (1 << B)) != 0;
		}
	}
}

void S9xWinScanJoypads ()
{
    uint8 PadState[2];

	S9xUpdateJoyState();

    for (int J = 0; J < 8; J++)
    {
        if (Joypad [J].Enabled)
        {
			// toggle checks
			{
       	     	PadState[0]  = 0;
				PadState[0] |= ToggleJoypadStorage[J].R||TurboToggleJoypadStorage[J].R      ?  16 : 0;
				PadState[0] |= ToggleJoypadStorage[J].L||TurboToggleJoypadStorage[J].L      ?  32 : 0;
				PadState[0] |= ToggleJoypadStorage[J].X||TurboToggleJoypadStorage[J].X      ?  64 : 0;
				PadState[0] |= ToggleJoypadStorage[J].A||TurboToggleJoypadStorage[J].A      ? 128 : 0;

	            PadState[1]  = 0;
				PadState[1] |= ToggleJoypadStorage[J].Right||TurboToggleJoypadStorage[J].Right   ?   1 : 0;
				PadState[1] |= ToggleJoypadStorage[J].Left||TurboToggleJoypadStorage[J].Left     ?   2 : 0;
				PadState[1] |= ToggleJoypadStorage[J].Down||TurboToggleJoypadStorage[J].Down     ?   4 : 0;
				PadState[1] |= ToggleJoypadStorage[J].Up||TurboToggleJoypadStorage[J].Up         ?   8 : 0;
				PadState[1] |= ToggleJoypadStorage[J].Start||TurboToggleJoypadStorage[J].Start   ?  16 : 0;
				PadState[1] |= ToggleJoypadStorage[J].Select||TurboToggleJoypadStorage[J].Select ?  32 : 0;
				PadState[1] |= ToggleJoypadStorage[J].Y||TurboToggleJoypadStorage[J].Y           ?  64 : 0;
				PadState[1] |= ToggleJoypadStorage[J].B||TurboToggleJoypadStorage[J].B           ? 128 : 0;
			}
			// auto-hold AND regular key/joystick presses
			if(S9xGetState(Joypad[J+8].Left))
			{
				PadState[0] ^= (!S9xGetState(Joypad[J].R)||!S9xGetState(Joypad[J+8].R))      ?  16 : 0;
				PadState[0] ^= (!S9xGetState(Joypad[J].L)||!S9xGetState(Joypad[J+8].L))      ?  32 : 0;
				PadState[0] ^= (!S9xGetState(Joypad[J].X)||!S9xGetState(Joypad[J+8].X))      ?  64 : 0;
				PadState[0] ^= (!S9xGetState(Joypad[J].A)||!S9xGetState(Joypad[J+8].A))      ? 128 : 0;

				PadState[1] ^= (!S9xGetState(Joypad[J].Right))  ?   1 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Right_Up))  ? 1 + 8 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Right_Down)) ? 1 + 4 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Left))   ?   2 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Left_Up)) ?   2 + 8 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Left_Down)) ?  2 + 4 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Down))   ?   4 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Up))     ?   8 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Start)||!S9xGetState(Joypad[J+8].Start))  ?  16 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Select)||!S9xGetState(Joypad[J+8].Select)) ?  32 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].Y)||!S9xGetState(Joypad[J+8].Y))      ?  64 : 0;
				PadState[1] ^= (!S9xGetState(Joypad[J].B)||!S9xGetState(Joypad[J+8].B))      ? 128 : 0;
			}

			bool turbofy = !S9xGetState(Joypad[J+8].Up); // All Mod for turbo

			//handle turbo case! (autofire / auto-fire)
			if(turbofy || ((GUI.TurboMask&TURBO_A_MASK))&&(PadState[0]&128) || !S9xGetState(Joypad[J+8].A      )) PadState[0]^=(joypads[J]&128);
			if(turbofy || ((GUI.TurboMask&TURBO_B_MASK))&&(PadState[1]&128) || !S9xGetState(Joypad[J+8].B      )) PadState[1]^=((joypads[J]&(128<<8))>>8);
			if(turbofy || ((GUI.TurboMask&TURBO_Y_MASK))&&(PadState[1]&64) || !S9xGetState(Joypad[J+8].Y       )) PadState[1]^=((joypads[J]&(64<<8))>>8);
			if(turbofy || ((GUI.TurboMask&TURBO_X_MASK))&&(PadState[0]&64) || !S9xGetState(Joypad[J+8].X       )) PadState[0]^=(joypads[J]&64);
			if(turbofy || ((GUI.TurboMask&TURBO_L_MASK))&&(PadState[0]&32) || !S9xGetState(Joypad[J+8].L       )) PadState[0]^=(joypads[J]&32);
			if(turbofy || ((GUI.TurboMask&TURBO_R_MASK))&&(PadState[0]&16) || !S9xGetState(Joypad[J+8].R       )) PadState[0]^=(joypads[J]&16);
			if(turbofy || ((GUI.TurboMask&TURBO_STA_MASK))&&(PadState[1]&16) || !S9xGetState(Joypad[J+8].Start )) PadState[1]^=((joypads[J]&(16<<8))>>8);
			if(turbofy || ((GUI.TurboMask&TURBO_SEL_MASK))&&(PadState[1]&32) || !S9xGetState(Joypad[J+8].Select)) PadState[1]^=((joypads[J]&(32<<8))>>8);
			if(           ((GUI.TurboMask&TURBO_LEFT_MASK))&&(PadState[1]&2)                                    ) PadState[1]^=((joypads[J]&(2<<8))>>8);
			if(           ((GUI.TurboMask&TURBO_UP_MASK))&&(PadState[1]&8)                                      ) PadState[1]^=((joypads[J]&(8<<8))>>8);
			if(           ((GUI.TurboMask&TURBO_RIGHT_MASK))&&(PadState[1]&1)                                   ) PadState[1]^=((joypads[J]&(1<<8))>>8);
			if(           ((GUI.TurboMask&TURBO_DOWN_MASK))&&(PadState[1]&4)                                    ) PadState[1]^=((joypads[J]&(4<<8))>>8);

			if(TurboToggleJoypadStorage[J].A     ) PadState[0]^=(joypads[J]&128);
			if(TurboToggleJoypadStorage[J].B     ) PadState[1]^=((joypads[J]&(128<<8))>>8);
			if(TurboToggleJoypadStorage[J].Y     ) PadState[1]^=((joypads[J]&(64<<8))>>8);
			if(TurboToggleJoypadStorage[J].X     ) PadState[0]^=(joypads[J]&64);
			if(TurboToggleJoypadStorage[J].L     ) PadState[0]^=(joypads[J]&32);
			if(TurboToggleJoypadStorage[J].R     ) PadState[0]^=(joypads[J]&16);
			if(TurboToggleJoypadStorage[J].Start ) PadState[1]^=((joypads[J]&(16<<8))>>8);
			if(TurboToggleJoypadStorage[J].Select) PadState[1]^=((joypads[J]&(32<<8))>>8);
			if(TurboToggleJoypadStorage[J].Left  ) PadState[1]^=((joypads[J]&(2<<8))>>8);
			if(TurboToggleJoypadStorage[J].Up    ) PadState[1]^=((joypads[J]&(8<<8))>>8);
			if(TurboToggleJoypadStorage[J].Right ) PadState[1]^=((joypads[J]&(1<<8))>>8);
			if(TurboToggleJoypadStorage[J].Down  ) PadState[1]^=((joypads[J]&(4<<8))>>8);
			//end turbo case...


			// enforce left+right/up+down disallowance here to
			// avoid recording unused l+r/u+d that will cause desyncs
			// when played back with l+r/u+d is allowed
			if(!Settings.UpAndDown)
			{
				if((PadState[1] & 2) != 0)
					PadState[1] &= ~(1);
				if((PadState[1] & 8) != 0)
					PadState[1] &= ~(4);
			}

            joypads [J] = PadState [0] | (PadState [1] << 8) | 0x80000000;
        }
        else
            joypads [J] = 0;
    }

	// input from macro
	for (int J = 0; J < 8; J++)
	{
		if(MacroIsEnabled(J))
		{
			uint16 userPadState = joypads[J] & 0xFFFF;
			uint16 macroPadState = MacroInput(J);
			uint16 newPadState;

			switch(GUI.MacroInputMode)
			{
			case MACRO_INPUT_MOV:
				newPadState = macroPadState;
				break;
			case MACRO_INPUT_OR:
				newPadState = macroPadState | userPadState;
				break;
			case MACRO_INPUT_XOR:
				newPadState = macroPadState ^ userPadState;
				break;
			default:
				newPadState = userPadState;
				break;
			}

			PadState[0] = (uint8) ( newPadState       & 0xFF);
			PadState[1] = (uint8) ((newPadState >> 8) & 0xFF);

			// enforce left+right/up+down disallowance here to
			// avoid recording unused l+r/u+d that will cause desyncs
			// when played back with l+r/u+d is allowed
			if(!Settings.UpAndDown)
			{
				if((PadState[1] & 2) != 0)
					PadState[1] &= ~(1);
				if((PadState[1] & 8) != 0)
					PadState[1] &= ~(4);
			}

			joypads [J] = PadState [0] | (PadState [1] << 8) | 0x80000000;
		}
	}

#ifdef NETPLAY_SUPPORT
    if (Settings.NetPlay)
	{
		// Send joypad position update to server
		S9xNPSendJoypadUpdate (joypads [GUI.NetplayUseJoypad1 ? 0 : NetPlay.Player-1]);

		// set input from network
		for (int J = 0; J < NP_MAX_CLIENTS; J++)
			joypads[J] = S9xNPGetJoypad (J);
	}
#endif
}

PALETTEENTRY S9xPaletteEntry[256];
void S9xSetPalette( void)
{
	if(GUI.outputMethod==DIRECTDRAW) {
		LPDIRECTDRAWPALETTE lpDDTemp;
		//uint16 Brightness = IPPU.MaxBrightness * 140;

		// Only update the palette structures if needed
		if( GUI.ScreenDepth == 8)
		{
	//        if (Settings.SixteenBit)
			{
				for (int i = 0; i < 256; i++)
				{
					S9xPaletteEntry[i].peRed   = FixedColours [i].red;
					S9xPaletteEntry[i].peGreen = FixedColours [i].green;
					S9xPaletteEntry[i].peBlue  = FixedColours [i].blue;
				}
			}
	//        else
	//        {
	//            for (int i = 0; i < 256; i ++)
	//            {
	//                S9xPaletteEntry[i].peRed   = (((PPU.CGDATA [i] >>  0) & 0x1F) * Brightness) >> 8;
	//                S9xPaletteEntry[i].peGreen = (((PPU.CGDATA [i] >>  5) & 0x1F) * Brightness) >> 8;
	//                S9xPaletteEntry[i].peBlue  = (((PPU.CGDATA [i] >> 10) & 0x1F) * Brightness) >> 8;
	//            }
	//        }

			DirectDraw.lpDDSPrimary2->GetPalette (&lpDDTemp);
			if (lpDDTemp != DirectDraw.lpDDPalette)
				DirectDraw.lpDDSPrimary2->SetPalette (DirectDraw.lpDDPalette);
			DirectDraw.lpDDPalette->SetEntries (0, 0, 256, S9xPaletteEntry);
		}
	}
}

bool LockSurface2 (LPDIRECTDRAWSURFACE2 lpDDSurface, SSurface *lpSurface)
{
    DDSURFACEDESC ddsd;
    HRESULT hResult;
    int retry;

    ddsd.dwSize = sizeof (ddsd);

    retry = 0;
    while (true)
    {
        hResult = lpDDSurface->Lock( NULL, &ddsd, DDLOCK_WAIT, NULL);

        if( hResult == DD_OK)
        {
            lpSurface->Width = ddsd.dwWidth;
            lpSurface->Height = ddsd.dwHeight;
            lpSurface->Pitch = ddsd.lPitch;
            lpSurface->Surface = (unsigned char *)ddsd.lpSurface;
            return (true);
        }

        if (hResult == DDERR_SURFACELOST)
        {
            retry++;
            if (retry > 5)
                return (false);

            hResult = lpDDSurface->Restore();
            GUI.ScreenCleared = true;
            if (hResult != DD_OK)
                return (false);

            continue;
        }

        if (hResult != DDERR_WASSTILLDRAWING)
            return (false);
    }
}

BYTE *ScreenBuf1 = NULL;
BYTE *ScreenBuf2 = NULL;
BYTE *ScreenBuffer = NULL;

static bool8 locked_surface = FALSE;
static SSurface Dst;

bool8 S9xInitUpdate (void)
{
#ifdef WIN_SKIPRENDER_OPTIMIZATION
	// under these specific conditions, blit the game screen directly without calling any render function
	// but, this causes problems when GUI.Scale changes during a frame (which happens when previewing filters)
    if (GUI.outputMethod==DIRECTDRAW && GUI.Scale == FILTER_NONE &&
        (GUI.Stretch || !GUI.FullScreen) &&
        !GUI.NeedDepthConvert &&
        ((DirectDraw.lpDDSOffScreen2 &&
          LockSurface2 (DirectDraw.lpDDSOffScreen2, &Dst))))
    {
		GFX.Pitch = Dst.Pitch;
        GFX.Screen = (uint16*)Dst.Surface;
        DirectDraw.lpDDSOffScreen2->Unlock (Dst.Surface);
        locked_surface = TRUE;
    }
    else
#endif
    {
        locked_surface = FALSE;
#ifdef USE_OPENGL
        // For OpenGL lock the screen buffer size width to 512 (might be
        // overriden in S9xStartScreenRefresh) so the buffer can be uploaded
        // into texture memory with a single OpenGL call.
//        if (Settings.OpenGLEnable)
        if (OPENGL_MODE)
            GFX.RealPPL = 512 * sizeof (uint16);
        else
#endif
        GFX.RealPPL = EXT_PITCH;
        GFX.Screen = (uint16*)ScreenBuffer;
    }

        GFX.PPL = GFX.Pitch >> 1;

	return (TRUE);
}

#define RenderMethod ((Src.Height > SNES_HEIGHT_EXTENDED || Src.Width == 512) ? RenderMethodHiRes : RenderMethod)

bool8 S9xContinueUpdate(int Width, int Height)
{
	// called every other frame during interlace

	// avi writing
	DoAVIVideoFrame();

	return true;
}

extern bool in_display_dlg;

bool8 S9xDeinitUpdate (int Width, int Height/*, bool8 sixteen_bit*/)
{
    SSurface Src;
    LPDIRECTDRAWSURFACE2 lpDDSurface2 = NULL;
    LPDIRECTDRAWSURFACE2 pDDSurface = NULL;
    bool PrimarySurfaceLockFailed = false;
    RECT srcRect;

    Src.Width = Width;
	if(Height%SNES_HEIGHT)
	    Src.Height = Height;
	else
	{
		if(Height==SNES_HEIGHT)
			Src.Height=SNES_HEIGHT_EXTENDED;
		else Src.Height=SNES_HEIGHT_EXTENDED<<1;
	}
    Src.Pitch = GFX.Pitch;
    Src.Surface = (BYTE*)GFX.Screen;
    int srcDepth = 16;

	const int OrigHeight = Height;
	Height = Src.Height;

	if(GUI.NotifySoundDSPRead)
	{
		static char whatWereRead[256];

		strcpy(whatWereRead, "");
		if(IAPU.KONNotifier)
			strcat(whatWereRead, "KON ");
		if(IAPU.KOFFNotifier)
			strcat(whatWereRead, "KOFF ");
		if(IAPU.OUTXNotifier)
			strcat(whatWereRead, "OUTX ");
		if(IAPU.ENVXNotifier)
			strcat(whatWereRead, "ENVX ");
		if(IAPU.ENDXNotifier)
			strcat(whatWereRead, "ENDX ");

		if(strcmp(whatWereRead, ""))
		{
			static char infoStr[256];

			sprintf(infoStr, "Sound: %sread", whatWereRead);
			GFX.InfoString = infoStr;
			GFX.InfoStringTimeout = 1;
		}
	}
	IAPU.KONNotifier = false;
	IAPU.KOFFNotifier = false;
	IAPU.OUTXNotifier = false;
	IAPU.ENVXNotifier = false;
	IAPU.ENDXNotifier = false;

	if(!GFX.Repainting && Settings.TakeScreenshot)
		GFX.InfoString = NULL; // remove text message for screenshot (even if GUI.MessagesInImage=true)

	if(!Settings.AutoDisplayMessages)
	{
		if(GUI.MessagesInImage)
		{
			S9xDisplayMessages(GFX.Screen, GFX.RealPPL, IPPU.RenderedScreenWidth, IPPU.RenderedScreenHeight, 1);
		}
		if (Settings.LuaDrawingsInScreen && !GFX.Repainting) {
			S9xLuaGui(GFX.Screen, IPPU.RenderedScreenWidth, IPPU.RenderedScreenHeight, 16, GFX.Pitch);
			S9xLuaClearGui();
		}
	}

	if(!GFX.Repainting && Settings.TakeScreenshot)
		S9xDoScreenshot(IPPU.RenderedScreenWidth, IPPU.RenderedScreenHeight);

	// avi writing
	DoAVIVideoFrame();

	if(!Settings.AutoDisplayMessages && !Settings.LuaDrawingsInScreen && !GFX.Repainting) {
		S9xLuaGui(Src.Surface, Width, Height, srcDepth, Src.Pitch);
		S9xLuaClearGui();
	}

	GUI.ScreenCleared = true;

    SelectRenderMethod ();

    if (!VOODOO_MODE && !OPENGL_MODE)
    {
		// Clear some of the old SNES rendered image
		// when the resolution becomes lower in x or y,
		// otherwise the image processors (filters) might access
		// some of the old rendered data at the edges.
        {
            static int LastWidth = 0;
            static int LastHeight = 0;

            if (Width < LastWidth)
            {
                const int hh = max(LastHeight, OrigHeight);
                for (int i = 0; i < hh; i++)
                    memset (GFX.Screen + i * (GFX.Pitch>>1) + Width*1, 0, 4);
            }
            if (OrigHeight < LastHeight)
			{
                const int ww = max(LastWidth, Width);
                for (int i = OrigHeight; i < LastHeight ; i++)
                    memset (GFX.Screen + i * (GFX.Pitch>>1), 0, ww * 2);

				// also old clear extended height stuff from drawing surface
				if((int)Src.Height > OrigHeight)
					for (int i = OrigHeight; i < (int)Src.Height ; i++)
						memset (Src.Surface + i * Src.Pitch, 0, Src.Pitch);
			}
            LastWidth = Width;
            LastHeight = OrigHeight;
        }
		if(GUI.outputMethod==DIRECT3D) {
			//do the actual rendering
			Direct3D.render(Src);
		} else {
			if (locked_surface)
			{
				lpDDSurface2 = DirectDraw.lpDDSOffScreen2;
				PrimarySurfaceLockFailed = true;
				srcRect.top	= 0;
				srcRect.bottom = Height - (GUI.HeightExtend?0:15);
				srcRect.left   = 0;
				srcRect.right  = Width;
			}
			else
			{
				DDSCAPS caps;
				caps.dwCaps = DDSCAPS_BACKBUFFER;

				if (DirectDraw.lpDDSPrimary2->GetAttachedSurface (&caps, &pDDSurface) != DD_OK ||
					pDDSurface == NULL)
				{
					lpDDSurface2 = DirectDraw.lpDDSPrimary2;
				}
				else
					lpDDSurface2 = pDDSurface;

				// this check seems to mess up fullscreen unstretched
				//if (GUI.Stretch || GUI.Scale == 1 || !GUI.FullScreen ||
				//    !LockSurface2 (lpDDSurface2, &Dst))
				{
					lpDDSurface2 = DirectDraw.lpDDSOffScreen2;
					if (!LockSurface2 (lpDDSurface2, &Dst))
						return (false);

					PrimarySurfaceLockFailed = true;
				}

				if (!GUI.DepthConverted)
				{
	//				if (GUI.Scale == FILTER_NONE)
	//                {
	//                    srcRect.left = srcRect.top = 0;
	//                    srcRect.right = Width;
	//                    srcRect.bottom = Height;
	////						Dst.Pitch = Src.Pitch;
	//						Dst.Width = Src.Width;
	//						Dst.Height = Src.Height;
	//                    ConvertDepth (&Src, &Dst, &srcRect);
	//					if(!Settings.AutoDisplayMessages)
	//						S9xDisplayMessages ((uint16*)Dst.Surface, Dst.Pitch/2, srcRect.right-srcRect.left, srcRect.bottom-srcRect.top - ((in_display_dlg && GUI.HeightExtend) ? GetFilterScale(GUI.Scale) : 0), GetFilterScale(GUI.Scale));
	//                }
	//                else
					{
						SSurface tmp;
						static BYTE buf [256 * 239 * 4*3*3];

	//					int width = Dst.Width;
	//					int height = Dst.Height;

						tmp.Surface = buf;
	//                    tmp.Pitch = MAX_SNES_WIDTH * 4;
	//                    tmp.Width = MAX_SNES_WIDTH;
	//                    tmp.Height = MAX_SNES_HEIGHT;
						if(GUI.Scale == FILTER_NONE) {
							tmp.Pitch = Src.Pitch;
							tmp.Width = Src.Width;
							tmp.Height = Src.Height;
						} else {
							tmp.Pitch = Dst.Pitch;
							tmp.Width = Dst.Width;
							tmp.Height = Dst.Height;
						}
						RenderMethod (Src, tmp, &srcRect);
						if(!Settings.AutoDisplayMessages && !GUI.MessagesInImage)
							S9xDisplayMessages ((uint16*)tmp.Surface, tmp.Pitch/2, srcRect.right-srcRect.left, srcRect.bottom-srcRect.top - ((in_display_dlg && GUI.HeightExtend) ? GetFilterScale(GUI.Scale) : 0), GetFilterScale(GUI.Scale));
	//					tmp.Pitch = Src.Pitch;
						ConvertDepth (&tmp, &Dst, &srcRect);
					}
				}
				else
				{
					RenderMethod (Src, Dst, &srcRect);
					if(!Settings.AutoDisplayMessages && !GUI.MessagesInImage)
						S9xDisplayMessages ((uint16*)Dst.Surface, Dst.Pitch/2, srcRect.right-srcRect.left, srcRect.bottom-srcRect.top - ((in_display_dlg && GUI.HeightExtend) ? GetFilterScale(GUI.Scale) : 0), GetFilterScale(GUI.Scale));
				}
			}

			RECT lastRect = GUI.SizeHistory [GUI.FlipCounter % GUI.NumFlipFrames];
			if (PrimarySurfaceLockFailed)
			{
				POINT p;

				if (GUI.Stretch)
				{
				/*	p.x = p.y = 0;

					ClientToScreen (GUI.hWnd, &p);
					GetClientRect (GUI.hWnd, &dstRect);
					OffsetRect(&dstRect, p.x, p.y);
					*/
					p.x = p.y = 0;
					ClientToScreen (GUI.hWnd, &p);
					GetClientRect (GUI.hWnd, &dstRect);
	//				dstRect.bottom = int(double(dstRect.bottom) * double(239.0 / 240.0));

					if(GUI.AspectRatio)
					{
						int width = dstRect.right - dstRect.left;
						int height = dstRect.bottom - dstRect.top;

						int oldWidth = GUI.AspectWidth;
						int oldHeight = GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT;
						int newWidth, newHeight;

						if(oldWidth * height > oldHeight * width)
						{
							newWidth = oldWidth*width/oldWidth;
							newHeight = oldHeight*width/oldWidth;
						}
						else
						{
							newWidth = oldWidth*height/oldHeight;
							newHeight = oldHeight*height/oldHeight;
						}
						int xOffset = (width - newWidth)/2;
						int yOffset = (height - newHeight)/2;

						dstRect.right = dstRect.left + newWidth;
						dstRect.bottom = dstRect.top + newHeight;

						OffsetRect(&dstRect, p.x + xOffset, p.y + yOffset);
					}
					else
					{
						OffsetRect(&dstRect, p.x, p.y);
					}
				}
				else
				{
					GetClientRect (GUI.hWnd, &dstRect);
					int width = srcRect.right - srcRect.left;
					int height = srcRect.bottom - srcRect.top;

					//if (GUI.Scale == 1)
					//{
					//	width = MAX_SNES_WIDTH;
					//	if (height < 240)
					//		height *= 2;
					//}
					p.x = ((dstRect.right - dstRect.left) - width) >> 1;
		 			p.y = ((dstRect.bottom - dstRect.top) - height) >> 1;

					if(p.y < 0) p.y = 0;
					if(p.x < 0) p.x = 0;

					ClientToScreen (GUI.hWnd, &p);

					dstRect.top = p.y;
					dstRect.left = p.x;
					dstRect.bottom = dstRect.top + height;
					dstRect.right  = dstRect.left + width;
				}
			}
			else
				dstRect = srcRect;

			lpDDSurface2->Unlock (Dst.Surface);
			if (PrimarySurfaceLockFailed)
			{
				DDSCAPS caps;
				caps.dwCaps = DDSCAPS_BACKBUFFER;

				if (DirectDraw.lpDDSPrimary2->GetAttachedSurface (&caps, &pDDSurface) != DD_OK ||
					pDDSurface == NULL)
				{
					lpDDSurface2 = DirectDraw.lpDDSPrimary2;
				}
				else
					lpDDSurface2 = pDDSurface;

				// actually draw it onto the screen (unless in fullscreen mode; see UpdateBackBuffer() for that)
				while (lpDDSurface2->Blt (&dstRect, DirectDraw.lpDDSOffScreen2, &srcRect, DDBLT_WAIT, NULL) == DDERR_SURFACELOST)
					lpDDSurface2->Restore ();
			}

			RECT rect;
			DDBLTFX fx;

			memset (&fx, 0, sizeof (fx));
			fx.dwSize = sizeof (fx);

			if (GUI.FlipCounter >= GUI.NumFlipFrames)
			{
				if (lastRect.top < dstRect.top)
				{
					rect.top = lastRect.top;
					rect.bottom = dstRect.top;
					rect.left = min(lastRect.left, dstRect.left);
					rect.right = max(lastRect.right, dstRect.right);
					lpDDSurface2->Blt (&rect, NULL, &rect,
									   DDBLT_WAIT | DDBLT_COLORFILL, &fx);
				}
				if (lastRect.bottom > dstRect.bottom)
				{
					rect.left = min(lastRect.left, dstRect.left);
					rect.right = max(lastRect.right, dstRect.right);
					rect.top = dstRect.bottom;
					rect.bottom = lastRect.bottom;
					lpDDSurface2->Blt (&rect, NULL, &rect,
									   DDBLT_WAIT | DDBLT_COLORFILL, &fx);
				}
				if (lastRect.left < dstRect.left)
				{
					rect.left = lastRect.left;
					rect.right = dstRect.left;
					rect.top = dstRect.top;
					rect.bottom = dstRect.bottom;
					lpDDSurface2->Blt (&rect, NULL, &rect,
									   DDBLT_WAIT | DDBLT_COLORFILL, &fx);
				}
				if (lastRect.right > dstRect.right)
				{
					rect.left = dstRect.right;
					rect.right = lastRect.right;
					rect.top = dstRect.top;
					rect.bottom = dstRect.bottom;
					lpDDSurface2->Blt (&rect, NULL, &rect,
									   DDBLT_WAIT | DDBLT_COLORFILL, &fx);
				}
			}

			DirectDraw.lpDDSPrimary2->Flip (NULL, GUI.Vsync?DDFLIP_WAIT:DDFLIP_NOVSYNC);
		}
	}
    else
    {
        srcRect.top    = 0;
        srcRect.bottom = Height;
        srcRect.left   = 0;
        srcRect.right  = Width;
        dstRect = srcRect;
		RenderMethod (Src, Dst, &srcRect);
//		if(!Settings.AutoDisplayMessages)
//			S9xDisplayMessages ((uint16*)Dst.Surface, Dst.Pitch/2, srcRect.right-srcRect.left, srcRect.bottom-srcRect.top - ((in_display_dlg && GUI.HeightExtend) ? GetFilterScale(GUI.Scale) : 0), GetFilterScale(GUI.Scale));
    }

    GUI.SizeHistory [GUI.FlipCounter % GUI.NumFlipFrames] = dstRect;
    GUI.FlipCounter++;

    return (true);
}

void InitSnes9X( void)
{
#ifdef DEBUGGER
//    extern FILE *trace;

//    trace = fopen( "SNES9X.TRC", "wt");
    freopen( "SNES9X.OUT", "wt", stdout);
    freopen( "SNES9X.ERR", "wt", stderr);

//    CPU.Flags |= TRACE_FLAG;
//    APU.Flags |= TRACE_FLAG;
#endif

//#ifdef GENERATE_OFFSETS_H
//    offsets_h = fopen ("offsets.h", "wt");
//    generate_offsets_h (0, NULL);
//    fclose (offsets_h);
//#endif

    Memory.Init();

	extern void S9xPostRomInit();
	Memory.PostRomInitFunc = S9xPostRomInit;

    ScreenBuf1 = new BYTE [EXT_PITCH * EXT_HEIGHT];
    ScreenBuf2 = new BYTE [EXT_PITCH * EXT_HEIGHT];

    ScreenBuffer = ScreenBuf1 + EXT_OFFSET;
    memset (ScreenBuf1, 0, EXT_PITCH * EXT_HEIGHT);
    memset (ScreenBuf2, 0, EXT_PITCH * EXT_HEIGHT);

    GFX.Pitch = EXT_PITCH;
    GFX.RealPPL = EXT_PITCH;
    GFX.Screen = (uint16*)(ScreenBuf1 + EXT_OFFSET);

    S9xSetWinPixelFormat ();
    S9xGraphicsInit();

	//InitializeCriticalSection(&GUI.SoundCritSect); // must be called before process config file
	//CoInitializeEx(NULL, COINIT_MULTITHREADED); // FIXME: this limits file manipulation on GetOpenFileName (?)
	CoInitialize(NULL);

	S9xWinInitSound();

#ifdef USE_GLIDE
    Settings.GlideEnable = FALSE;
#endif

	S9xMovieInit ();

    for (int C = 0; C != 16; C ++)
        Joystick[C].Attached = joyGetDevCaps (JOYSTICKID1+C, &Joystick[C].Caps,
                                              sizeof( JOYCAPS)) == JOYERR_NOERROR;
}
extern "C"{
void DeinitS9x()
{
	if(ScreenBuf1)
		delete [] ScreenBuf1;
	if(ScreenBuf2)
		delete [] ScreenBuf2;
	S9xWinDeinitSound();
	DeleteCriticalSection(&GUI.SoundCritSect);
	CoUninitialize();
	if(GUI.GunSight)
		DestroyCursor(GUI.GunSight);//= LoadCursor (hInstance, MAKEINTRESOURCE (IDC_CURSOR_SCOPE));
    if(GUI.Arrow)
		DestroyCursor(GUI.Arrow);// = LoadCursor (NULL, IDC_ARROW);
	if(GUI.Accelerators)
		DestroyAcceleratorTable(GUI.Accelerators);// = LoadAccelerators (hInstance, MAKEINTRESOURCE (IDR_SNES9X_ACCELERATORS));
}
}
int ffs (uint32 mask)
{
    int m = 0;
    if (mask)
    {
        while (!(mask & (1 << m)))
            m++;

        return (m);
    }

    return (0);
}

void S9xSetWinPixelFormat ()
{
    extern int Init_2xSaI (uint32 BitFormat);

    S9xSetRenderPixelFormat (RGB565);
    Init_2xSaI (565);
    GUI.NeedDepthConvert = FALSE;
	GUI.DepthConverted = !GUI.NeedDepthConvert;

    if (VOODOO_MODE)
    {
        GUI.ScreenDepth = 16;
        GUI.RedShift = 11;
        GUI.GreenShift = 5;
        GUI.BlueShift = 0;
//        Settings.SixteenBit = TRUE;
    }
    else
    if (OPENGL_MODE)
    {
        GUI.ScreenDepth = 16;
        GUI.RedShift = 11;
        GUI.GreenShift = 6;
        GUI.BlueShift = 1;
//        Settings.SixteenBit = TRUE;
	S9xSetRenderPixelFormat (RGB5551);
	Init_2xSaI (555);
    }
    else
	if (GUI.outputMethod==DIRECT3D)
    {
		Direct3D.setSnes9xColorFormat();
	}
	else
	{

        GUI.ScreenDepth = DirectDraw.DDPixelFormat.dwRGBBitCount;
        if (GUI.ScreenDepth == 15)
            GUI.ScreenDepth = 16;

        GUI.RedShift = ffs (DirectDraw.DDPixelFormat.dwRBitMask);
        GUI.GreenShift = ffs (DirectDraw.DDPixelFormat.dwGBitMask);
        GUI.BlueShift = ffs (DirectDraw.DDPixelFormat.dwBBitMask);

        if((DirectDraw.DDPixelFormat.dwFlags&DDPF_RGB) != 0 &&
           GUI.ScreenDepth == 16 &&
           DirectDraw.DDPixelFormat.dwRBitMask == 0xF800 &&
           DirectDraw.DDPixelFormat.dwGBitMask == 0x07E0 &&
           DirectDraw.DDPixelFormat.dwBBitMask == 0x001F)
        {
            S9xSetRenderPixelFormat (RGB565);
            Init_2xSaI (565);
        }
        else
            if( (DirectDraw.DDPixelFormat.dwFlags&DDPF_RGB) != 0 &&
                GUI.ScreenDepth == 16 &&
                DirectDraw.DDPixelFormat.dwRBitMask == 0x7C00 &&
                DirectDraw.DDPixelFormat.dwGBitMask == 0x03E0 &&
                DirectDraw.DDPixelFormat.dwBBitMask == 0x001F)
            {
                S9xSetRenderPixelFormat (RGB555);
                Init_2xSaI (555);
            }
            else
                if((DirectDraw.DDPixelFormat.dwFlags&DDPF_RGB) != 0 &&
                   GUI.ScreenDepth == 16 &&
                   DirectDraw.DDPixelFormat.dwRBitMask == 0x001F &&
                   DirectDraw.DDPixelFormat.dwGBitMask == 0x07E0 &&
                   DirectDraw.DDPixelFormat.dwBBitMask == 0xF800)
                {
                    S9xSetRenderPixelFormat (BGR565);
                    Init_2xSaI (565);
                }
                else
                    if( (DirectDraw.DDPixelFormat.dwFlags&DDPF_RGB) != 0 &&
                        GUI.ScreenDepth == 16 &&
                        DirectDraw.DDPixelFormat.dwRBitMask == 0x001F &&
                        DirectDraw.DDPixelFormat.dwGBitMask == 0x03E0 &&
                        DirectDraw.DDPixelFormat.dwBBitMask == 0x7C00)
                    {
                        S9xSetRenderPixelFormat (BGR555);
                        Init_2xSaI (555);
                    }
                    else
                        if (DirectDraw.DDPixelFormat.dwRGBBitCount == 8 ||
                            DirectDraw.DDPixelFormat.dwRGBBitCount == 24 ||
                            DirectDraw.DDPixelFormat.dwRGBBitCount == 32)
                        {
                            S9xSetRenderPixelFormat (RGB565);
                            Init_2xSaI (565);
                        }

        if (!VOODOO_MODE &&
            !OPENGL_MODE &&
            ((GUI.ScreenDepth == 8 /*&& Settings.SixteenBit*/) ||
//             (GUI.ScreenDepth == 16 && !Settings.SixteenBit) ||
             GUI.ScreenDepth == 24 || GUI.ScreenDepth == 32))
            GUI.NeedDepthConvert = TRUE;

		GUI.DepthConverted = !GUI.NeedDepthConvert;

        if (//Settings.SixteenBit &&
            (GUI.ScreenDepth == 24 || GUI.ScreenDepth == 32))
        {
            GUI.RedShift += 3;
            GUI.GreenShift += 3;
            GUI.BlueShift += 3;
        }
    }


    int l = 0;
    int i;

    for (i = 0; i < 6; i++)
    {
	int r = (i * 31) / (6 - 1);
	for (int j = 0; j < 6; j++)
	{
	    int g = (j * 31) / (6 - 1);
	    for (int k = 0; k < 6; k++)
	    {
		int b = (k * 31) / (6 - 1);

		FixedColours [l].red = r << 3;
		FixedColours [l].green = g << 3;
		FixedColours [l++].blue = b << 3;
	    }
	}
    }

    int *color_diff = new int [0x10000];
    int diffr, diffg, diffb, maxdiff = 0, won = 0, lost;
    int r, d = 8;
    for (r = 0; r <= (int) MAX_RED; r++)
    {
	int cr, g, q;

	int k = 6 - 1;
	cr = (r * k) / MAX_RED;
	q  = (r * k) % MAX_RED;
	if (q > d && cr < k)
	    cr++;
	diffr = abs (cr * k - r);
	for (g = 0; g <= (int) MAX_GREEN; g++)
	{
	    int cg, b;

	    k  = 6 - 1;
	    cg = (g * k) / MAX_GREEN;
	    q  = (g * k) % MAX_GREEN;
	    if(q > d && cg < k)
		cg++;
	    diffg = abs (cg * k - g);
	    for (b = 0; b <= (int) MAX_BLUE; b++)
	    {
		int cb;
		int rgb = BUILD_PIXEL2(r, g, b);

		k  = 6 - 1;
		cb = (b * k) / MAX_BLUE;
		q  = (b * k) % MAX_BLUE;
		if (q > d && cb < k)
		    cb++;
		diffb = abs (cb * k - b);
		palette[rgb] = (cr * 6 + cg) * 6 + cb;
		color_diff[rgb] = diffr + diffg + diffb;
		if (color_diff[rgb] > maxdiff)
		    maxdiff = color_diff[rgb];
	    }
	}
    }

    while (maxdiff > 0 && l < 256)
    {
	int newmaxdiff = 0;
	lost = 0; won++;
	for (r = MAX_RED; r >= 0; r--)
	{
	    int g;

	    for (g = MAX_GREEN; g >= 0; g--)
	    {
		int b;

		for (b = MAX_BLUE; b >= 0; b--)
		{
		    int rgb = BUILD_PIXEL2(r, g, b);

		    if (color_diff[rgb] == maxdiff)
		    {
			if (l >= 256)
			    lost++;
			else
			{
			    FixedColours [l].red = r << 3;
			    FixedColours [l].green = g << 3;
			    FixedColours [l].blue = b << 3;
			    palette [rgb] = l++;
			}
			color_diff[rgb] = 0;
		    }
		    else
			if (color_diff[rgb] > newmaxdiff)
			    newmaxdiff = color_diff[rgb];

		}
	    }
	}
	maxdiff = newmaxdiff;
    }
    delete [] color_diff;
}

void Convert8To32 (SSurface *src, SSurface *dst, RECT *srect)
{
    uint32 brightness = IPPU.MaxBrightness >> 1;
    uint32 conv [256];
    int height = srect->bottom - srect->top;
    int width = srect->right - srect->left;
    int offset1 = srect->top * src->Pitch + srect->left;
    int offset2 = 0;//((dst->Height - height) >> 1) * dst->Pitch +
//        ((dst->Width - width) >> 1) * sizeof (uint32);

    for (int p = 0; p < 256; p++)
    {
        uint32 pixel = PPU.CGDATA [p];
        conv [p] = (((pixel & 0x1f) * brightness) << GUI.RedShift) |
                   ((((pixel >> 5) & 0x1f) * brightness) << GUI.GreenShift) |
                   ((((pixel >> 10) & 0x1f) * brightness) << GUI.BlueShift);
    }
    for (register int y = 0; y < height; y++)
    {
        register uint8 *s = ((uint8 *) src->Surface + y * src->Pitch + offset1);
        register uint32 *d = (uint32 *) ((uint8 *) dst->Surface +
                                         y * dst->Pitch + offset2);
        for (register int x = 0; x < width; x++)
            *d++ = conv [PPU.CGDATA [*s++]];
    }
}

void Convert16To32 (SSurface *src, SSurface *dst, RECT *srect)
{
    int height = srect->bottom - srect->top;
    int width = srect->right - srect->left;
    int offset1 = srect->top * src->Pitch + srect->left * 2;
    int offset2 = 0;//((dst->Height - height) >> 1) * dst->Pitch +
        //((dst->Width - width) >> 1) * sizeof (uint32);

    for (register int y = 0; y < height; y++)
    {
        register uint16 *s = (uint16 *) ((uint8 *) src->Surface + y * src->Pitch + offset1);
        register uint32 *d = (uint32 *) ((uint8 *) dst->Surface +
                                         y * dst->Pitch + offset2);
        for (register int x = 0; x < width; x++)
        {
            uint32 pixel = *s++;
            *d++ = (((pixel >> 11) & 0x1f) << GUI.RedShift) |
                   (((pixel >> 6) & 0x1f) << GUI.GreenShift) |
                   ((pixel & 0x1f) << GUI.BlueShift);
        }
    }
}

//void Convert8To24 (SSurface *src, SSurface *dst, RECT *srect)
//{
//    uint32 brightness = IPPU.MaxBrightness >> 1;
//    uint8 levels [32];
//    int height = srect->bottom - srect->top;
//    int width = srect->right - srect->left;
//    int offset1 = srect->top * src->Pitch + srect->left;
//    int offset2 = ((dst->Height - height) >> 1) * dst->Pitch +
//        ((dst->Width - width) >> 1) * 3;
//
//    for (int l = 0; l < 32; l++)
//	levels [l] = l * brightness;
//
//    for (register int y = 0; y < height; y++)
//    {
//        register uint8 *s = ((uint8 *) src->Surface + y * src->Pitch + offset1);
//        register uint8 *d = ((uint8 *) dst->Surface + y * dst->Pitch + offset2);
//
//#ifdef LSB_FIRST
//        if (GUI.RedShift < GUI.BlueShift)
//#else
//	if (GUI.RedShift > GUI.BlueShift)
//#endif
//        {
//            // Order is RGB
//            for (register int x = 0; x < width; x++)
//            {
//                uint16 pixel = PPU.CGDATA [*s++];
//                *(d + 0) = levels [(pixel & 0x1f)];
//                *(d + 1) = levels [((pixel >> 5) & 0x1f)];
//                *(d + 2) = levels [((pixel >> 10) & 0x1f)];
//                d += 3;
//            }
//        }
//        else
//        {
//            // Order is BGR
//            for (register int x = 0; x < width; x++)
//            {
//                uint16 pixel = PPU.CGDATA [*s++];
//                *(d + 0) = levels [((pixel >> 10) & 0x1f)];
//                *(d + 1) = levels [((pixel >> 5) & 0x1f)];
//                *(d + 2) = levels [(pixel & 0x1f)];
//                d += 3;
//            }
//        }
//    }
//}

void Convert16To24 (SSurface *src, SSurface *dst, RECT *srect)
{
	const int height = srect->bottom - srect->top;
	const int width = srect->right - srect->left;
	const int offset1 = srect->top * src->Pitch + srect->left * 2;
	const int offset2 = 0;//((dst->Height - height) >> 1) * dst->Pitch + ((dst->Width - width) >> 1) * 3;

	for (int y = 0; y < height; y++)
	{
		register uint16 *s = (uint16 *) ((uint8 *) src->Surface + y * src->Pitch + offset1);
		register uint8 *d = ((uint8 *) dst->Surface + y * dst->Pitch + offset2);

#ifdef LSB_FIRST
		if (GUI.RedShift < GUI.BlueShift)
#else
		if (GUI.RedShift > GUI.BlueShift)
#endif
		{
			// Order is RGB
			for (int x = 0; x < width; x++)
			{
				uint32 pixel = *s++;
				*(d + 0) = (pixel >> (11 - 3)) & 0xf8;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel & 0x1f) << 3;
				d += 3;
			}
		}
		else
		{
			// Order is BGR
			for (int x = 0; x < width; x++)
			{
				uint32 pixel = *s++;
				*(d + 0) = (pixel & 0x1f) << 3;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel >> (11 - 3)) & 0xf8;
				d += 3;
			}
		}
	}
}

void Convert16To8 (SSurface *src, SSurface *dst, RECT *srect)
{
    int height = srect->bottom - srect->top;
    int width = srect->right - srect->left;
    int offset1 = srect->top * src->Pitch + srect->left * 2;
    int offset2 = 0;//((dst->Height - height) >> 1) * dst->Pitch +
        //((dst->Width - width) >> 1);

    for (register int y = 0; y < height; y++)
    {
        register uint16 *s = (uint16 *) ((uint8 *) src->Surface + y * src->Pitch + offset1);
        register uint8 *d = ((uint8 *) dst->Surface + y * dst->Pitch + offset2);

        for (register int x = 0; x < width; x++)
            *d++ = palette [*s++];
    }
}

void Convert8To16 (SSurface *src, SSurface *dst, RECT *srect)
{
    uint32 levels [32];
    uint32 conv [256];
    int height = srect->bottom - srect->top;
    int width = srect->right - srect->left;
    int offset1 = srect->top * src->Pitch + srect->left;
    int offset2 = 0;//((dst->Height - height) >> 1) * dst->Pitch +
        //((dst->Width - width) >> 1) * sizeof (uint16);

    for (int l = 0; l < 32; l++)
	levels [l] = (l * IPPU.MaxBrightness) >> 4;

    for (int p = 0; p < 256; p++)
    {
        uint32 pixel = PPU.CGDATA [p];

        conv [p] = (levels [pixel & 0x1f] << GUI.RedShift) |
                   (levels [(pixel >> 5) & 0x1f] << GUI.GreenShift) |
                   (levels [(pixel >> 10) & 0x1f] << GUI.BlueShift);
    }
    for (register int y = 0; y < height; y++)
    {
        register uint8 *s = ((uint8 *) src->Surface + y * src->Pitch + offset1);
        register uint16 *d = (uint16 *) ((uint8 *) dst->Surface +
                                         y * dst->Pitch + offset2);
        for (register int x = 0; x < width; x += 2)
        {
            *(uint32 *) d = conv [*s] | (conv [*(s + 1)] << 16);
            s += 2;
            d += 2;
        }
    }
}

void ConvertDepth (SSurface *src, SSurface *dst, RECT *srect)
{
    // SNES image has been rendered in 16-bit, RGB565 format
    switch (GUI.ScreenDepth)
    {
        case 8:
            Convert16To8 (src, dst, srect);
            break;
        case 15: // is this right?
        case 16:
            break;
        case 24:
            Convert16To24 (src, dst, srect);
            break;
        case 32:
            Convert16To32 (src, dst, srect);
            break;
    }

	//srect->left = (dst->Width - src->Width) >> 1;
 //   srect->right = srect->left + src->Width;
 //   srect->top = (dst->Height - src->Height) >> 1;
 //   srect->bottom = srect->top + src->Height;
}

void S9xAutoSaveSRAM ()
{
    Memory.SaveSRAM (S9xGetFilename (".srm", SRAM_DIR));
}

void S9xSetPause (uint32 mask)
{
	Settings.ForcedPause |= mask;
}

void S9xClearPause (uint32 mask)
{
    Settings.ForcedPause &= ~mask;
    if (!Settings.ForcedPause)
    {
        // Wake up the main loop thread just if its blocked in a GetMessage call.
        PostMessage (GUI.hWnd, WM_NULL, 0, 0);
    }
}

static int S9xCompareSDD1IndexEntries (const void *p1, const void *p2)
{
    return (*(uint32 *) p1 - *(uint32 *) p2);
}

void S9xLoadSDD1Data ()
{
    char filename [_MAX_PATH + 1];
    char index [_MAX_PATH + 1];
    char data [_MAX_PATH + 1];

	Settings.SDD1Pack=FALSE;
    Memory.FreeSDD1Data ();

    if (strncmp (Memory.ROMName, TEXT("Star Ocean"), 10) == 0)
	{
		if(strlen(GUI.StarOceanPack)!=0)
			strcpy(filename, GUI.StarOceanPack);
		else Settings.SDD1Pack=TRUE;
	}
    else if(strncmp(Memory.ROMName, TEXT("STREET FIGHTER ALPHA2"), 21)==0)
	{
		if(Memory.ROMRegion==1)
		{
			if(strlen(GUI.SFA2NTSCPack)!=0)
				strcpy(filename, GUI.SFA2NTSCPack);
			else Settings.SDD1Pack=TRUE;
		}
		else
		{
			if(strlen(GUI.SFA2PALPack)!=0)
				strcpy(filename, GUI.SFA2PALPack);
			else Settings.SDD1Pack=TRUE;
		}
	}
	else
	{
		if(strlen(GUI.SFZ2Pack)!=0)
			strcpy(filename, GUI.SFZ2Pack);
		else Settings.SDD1Pack=TRUE;
	}

	if(Settings.SDD1Pack==TRUE)
		return;

    strcpy (index, filename);
    strcat (index, "\\SDD1GFX.IDX");
    strcpy (data, filename);
    strcat (data, "\\SDD1GFX.DAT");

    FILE *fs = fopen (index, "rb");
    int len = 0;

    if (fs)
    {
        // Index is stored as a sequence of entries, each entry being
        // 12 bytes consisting of:
        // 4 byte key: (24bit address & 0xfffff * 16) | translated block
        // 4 byte ROM offset
        // 4 byte length
        fseek (fs, 0, SEEK_END);
        len = ftell (fs);
        rewind (fs);
        Memory.SDD1Index = (uint8 *) malloc (len);
        fread (Memory.SDD1Index, 1, len, fs);
        fclose (fs);
        Memory.SDD1Entries = len / 12;

        if (!(fs = fopen (data, "rb")))
        {
            free ((char *) Memory.SDD1Index);
            Memory.SDD1Index = NULL;
            Memory.SDD1Entries = 0;
        }
        else
        {
            fseek (fs, 0, SEEK_END);
            len = ftell (fs);
            rewind (fs);
            Memory.SDD1Data = (uint8 *) malloc (len);
            fread (Memory.SDD1Data, 1, len, fs);
            fclose (fs);

            qsort (Memory.SDD1Index, Memory.SDD1Entries, 12,
                   S9xCompareSDD1IndexEntries);
        }
    }
}

bool JustifierOffscreen()
{
	return (bool)((GUI.MouseButtons&2)!=0);
}

//void JustifierButtons(uint32& justifiers)
//{
//	if(IPPU.Controller==SNES_JUSTIFIER_2)
//	{
//		if((GUI.MouseButtons&1)||(GUI.MouseButtons&2))
//		{
//			justifiers|=0x00200;
//		}
//		if(GUI.MouseButtons&4)
//		{
//			justifiers|=0x00800;
//		}
//	}
//	else
//	{
//		if((GUI.MouseButtons&1)||(GUI.MouseButtons&2))
//		{
//			justifiers|=0x00100;
//		}
//		if(GUI.MouseButtons&4)
//		{
//			justifiers|=0x00400;
//		}
//	}
//}

#ifdef MK_APU_RESAMPLE
void ResampleTo16000HzM16(uint16* input, uint16*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i++)
	{
		output[i]=(input[i*2]+input[(2*i)+1])>>1;
	}
}

void ResampleTo16000HzS16(uint16* input, uint16*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i+=2)
	{
		output[i]=(input[i*2]+input[(2*(i+1))])>>1;
		output[i+1]=(input[(i*2)+1]+input[(2*(i+1))+1])>>1;
	}
}
void ResampleTo8000HzM16(uint16* input, uint16*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i++)
	{
		output[i]=(input[i*4]+input[(4*i)+1]+input[(4*i)+2]+input[(4*i)+3])>>2;
	}
}

void ResampleTo8000HzS16(uint16* input, uint16*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i+=2)
	{
		output[i]=(input[i*4]+input[(4*i)+2]+input[(4*(i+1))]+input[(4*(i+1))+2])>>2;
		output[i+1]=(input[(i*4)+1]+input[(4*i)+3]+input[(4*(i+1))+1]+input[(4*(i+1))+3])>>2;
	}
}

void ResampleTo16000HzM8(uint8* input, uint8*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i++)
	{
		output[i]=(input[i*2]+input[(2*i)+1])>>1;
	}
}

void ResampleTo16000HzS8(uint8* input, uint8*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i+=2)
	{
		output[i]=(input[i*2]+input[(2*(i+1))])>>1;
		output[i+1]=(input[(i*2)+1]+input[(2*(i+1))+1])>>1;
	}
}
void ResampleTo8000HzM8(uint8* input, uint8*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i++)
	{
		output[i]=(input[i*4]+input[(4*i)+1]+input[(4*i)+2]+input[(4*i)+3])>>2;
	}
}

void ResampleTo8000HzS8(uint8* input, uint8*output,int output_samples)
{
	int i=0;
	for(i=0;i<output_samples;i+=2)
	{
		output[i]=(input[i*4]+input[(4*i)+2]+input[(4*(i+1))]+input[(4*(i+1))+2])>>2;
		output[i+1]=(input[(i*4)+1]+input[(4*i)+3]+input[(4*(i+1))+1]+input[(4*(i+1))+3])>>2;
	}
}

#endif

void DoWAVOpen(const char* filename)
{
	// close current instance
	if(GUI.WAVOut)
	{
		WAVClose(&GUI.WAVOut);
		GUI.WAVOut = NULL;
	}

	if(!S9xWinIsSoundActive())
	{
		DoWAVClose(2);
		return;
	}

	// create new writer
	WAVCreate(&GUI.WAVOut);

	WAVEFORMATEX wfx;

	wfx.wFormatTag = WAVE_FORMAT_PCM;
	wfx.nChannels = Settings.Stereo ? 2 : 1;
	wfx.nSamplesPerSec = Settings.SoundPlaybackRate;
	wfx.nBlockAlign = (Settings.SixteenBitSound ? 2 : 1) * (Settings.Stereo ? 2 : 1);
	wfx.wBitsPerSample = Settings.SixteenBitSound ? 16 : 8;
	wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
	wfx.cbSize = 0;

	WAVSetSoundFormat(&wfx, GUI.WAVOut);

	if(!WAVBegin(filename, GUI.WAVOut))
	{
		DoWAVClose(2);
		GUI.WAVOut = NULL;
		return;
	}

	// assume this function is called outside emulation core
//	so.err_counter = 0;
}

void DoWAVClose(int reason)
{
	if(!GUI.WAVOut)
	{
		return;
	}

	WAVClose(&GUI.WAVOut);
	GUI.WAVOut = NULL;

	switch(reason)
	{
	case 2:
		// create WAV failed
		S9xMessage(S9X_INFO, S9X_WAV_INFO, WAV_CREATION_FAILED);
		break;
	default:
		// print nothing
		break;
	}
}

void DoAVIOpen(const char* filename)
{
	// close current instance
	if(GUI.AVIOut)
	{
		AVIClose(&GUI.AVIOut);
		GUI.AVIOut = NULL;
	}

	// create new writer
	AVICreate(&GUI.AVIOut);

	int framerate = Memory.ROMFramesPerSecond;
	int frameskip = Settings.SkipFrames;
	// IPPU.RenderThisFrame must be true during AVI recording
//	if(frameskip == AUTO_FRAMERATE)
		frameskip = 1;
//	else
//		frameskip++;

	// IPPU.RenderThisFrame must be true during AVI recording
	AVISetFramerate(framerate, frameskip, GUI.AVIOut);

	avi_width = IPPU.RenderedScreenWidth;
	avi_height = IPPU.RenderedScreenHeight;
	avi_skip_frames = Settings.SkipFrames;

	if(GUI.AVIDoubleScale && avi_width <= SNES_WIDTH)
		avi_width = SNES_WIDTH*2;
	else if(!GUI.AVIDoubleScale && avi_width > SNES_WIDTH)
		avi_width = SNES_WIDTH;
	if(GUI.HeightExtend && avi_height < SNES_HEIGHT_EXTENDED)
		avi_height = SNES_HEIGHT_EXTENDED;
	if(GUI.AVIDoubleScale)
		avi_height *= 2;
	if(avi_height % 2 != 0) // most codecs can't handle odd-height images
		avi_height++;

	avi_pitch = BMP_PITCH(avi_width, 24);
	avi_image_size = avi_pitch * avi_height;

	BITMAPINFOHEADER bi;
	memset(&bi, 0, sizeof(bi));
	bi.biSize = 0x28;
	bi.biPlanes = 1;
	bi.biBitCount = 24;
	bi.biWidth = avi_width;
	bi.biHeight = avi_height;
	bi.biSizeImage = avi_image_size;

	AVISetVideoFormat(&bi, GUI.AVIOut);

	WAVEFORMATEX wfx;

	wfx.wFormatTag = WAVE_FORMAT_PCM;
	wfx.nChannels = Settings.Stereo ? 2 : 1;
	wfx.nSamplesPerSec = Settings.SoundPlaybackRate;
	wfx.nBlockAlign = (Settings.SixteenBitSound ? 2 : 1) * (Settings.Stereo ? 2 : 1);
	wfx.wBitsPerSample = Settings.SixteenBitSound ? 16 : 8;
	wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
	wfx.cbSize = 0;

	if(!Settings.Mute && S9xWinIsSoundActive())
	{
		AVISetSoundFormat(&wfx, GUI.AVIOut);
	}

	if(!AVIBegin(filename, GUI.AVIOut))
	{
		DoAVIClose(2);
		GUI.AVIOut = NULL;
		return;
	}

//	avi_sound_samples_per_update = (wfx.nSamplesPerSec * frameskip) / framerate;
	avi_sound_bytes_per_sample = wfx.nBlockAlign;

	// init buffers
	avi_buffer = new uint8[avi_image_size];
//	avi_sound_buffer = new uint8[avi_sound_samples_per_update * avi_sound_bytes_per_sample];

	// assume this function is called outside emulation core
//	so.err_counter = 0;
}

void DoAVIClose(int reason)
{
	if(!GUI.AVIOut)
	{
		return;
	}

	AVIClose(&GUI.AVIOut);
	GUI.AVIOut = NULL;

	delete [] avi_buffer;
//	delete [] avi_sound_buffer;

	avi_buffer = NULL;
//	avi_sound_buffer = NULL;

	switch(reason)
	{
	case 1:
		// emu settings changed
		S9xMessage(S9X_INFO, S9X_AVI_INFO, AVI_CONFIGURATION_CHANGED);
		break;
	case 2:
		// create AVI failed
		S9xMessage(S9X_INFO, S9X_AVI_INFO, AVI_CREATION_FAILED);
		break;
	default:
		// print nothing
		break;
	}
}

static void DoAVIVideoFrame(void)
{
	if(GFX.Repainting || !GUI.AVIOut)
	{
		return;
	}

	// check configuration
	const WAVEFORMATEX* pwfex = NULL;
	WAVEFORMATEX wfx;
	wfx.wFormatTag = WAVE_FORMAT_PCM;
    wfx.nChannels = Settings.Stereo ? 2 : 1;
    wfx.nSamplesPerSec = Settings.SoundPlaybackRate;
    wfx.nBlockAlign = (Settings.SixteenBitSound ? 2 : 1) * (Settings.Stereo ? 2 : 1);
    wfx.wBitsPerSample = Settings.SixteenBitSound ? 16 : 8;
    wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
    wfx.cbSize = 0;
	if(//avi_width != Width ||
		//avi_height != Height ||
		avi_skip_frames != Settings.SkipFrames ||
		(AVIGetSoundFormat(GUI.AVIOut, &pwfex) && memcmp(pwfex, &wfx, sizeof(WAVEFORMATEX))))
	{
		DoAVIClose(1);
		ReInitSound(1);			//reenable sound output
		return;
	}

	// convert to bitdepth 24
	const int snesWidth = IPPU.RenderedScreenWidth;
	const int snesHeight = IPPU.RenderedScreenHeight;
	if(snesWidth < SNES_WIDTH*2) // normal
	{
		if(avi_width < snesWidth*2) // 1x
		{
			BuildAVIVideoFrame1X();
		}
		else // 2x
		{
			BuildAVIVideoFrame2X();
		}
	}
	else // high-res
	{
		if(avi_width < snesWidth) // 1x
		{
			BuildAVIVideoFrame1XHiRes();
		}
		else // 2x
		{
			BuildAVIVideoFrame2XHiRes();
		}
	}

	// write to AVI
	AVIAddVideoFrame(avi_buffer, GUI.AVIOut);

	// sound samples should be added independently via AVIAddSoundSamples
}

// Dst: GFX.Screen 256xH  (1x1) 16bpp top-down
// Src: avi_buffer 256xH  (1x1) 24bpp bottom-up
static void BuildAVIVideoFrame1X (void)
{
	const int snesWidth = IPPU.RenderedScreenWidth;
	const int snesHeight = IPPU.RenderedScreenHeight;
	const int width = min(snesWidth, avi_width);
	const int height = min(snesHeight, avi_height);
	const int pitch = GFX.Pitch;
#ifdef LSB_FIRST
	const bool order_is_rgb = (GUI.RedShift < GUI.BlueShift);
#else
	const bool order_is_rgb = (GUI.RedShift > GUI.BlueShift);
#endif
	const int src_step = pitch - width*2;
	const int dst_step = -(avi_pitch + width*3);
	const int image_offset = (avi_height - height) * avi_pitch;
	uint16 *s = GFX.Screen;
	uint8  *d = &avi_buffer[(avi_height - 1) * avi_pitch];

	for(int y = 0; y < height; y++)
	{
		for(int x = 0; x < width; x++)
		{
			if(order_is_rgb)
			{
				// Order is RGB
				uint32 pixel = *s++;
				*(d + 0) = (pixel >> (11 - 3)) & 0xf8;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel & 0x1f) << 3;
				d += 3;
			}
			else
			{
				// Order is BGR
				uint32 pixel = *s++;
				*(d + 0) = (pixel & 0x1f) << 3;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel >> (11 - 3)) & 0xf8;
				d += 3;
			}
		}
		s = (uint16*)((uint8*)s + src_step);
		d += dst_step;
	}

	// black out what we might have missed
	if(image_offset > 0)
		memset(avi_buffer, 0, image_offset);
}

// Dst: GFX.Screen 256xH  (1x1) 16bpp top-down
// Src: avi_buffer 512x2H (2x2) 24bpp bottom-up
static void BuildAVIVideoFrame2X (void)
{
	const int snesWidth = IPPU.RenderedScreenWidth;
	const int snesHeight = IPPU.RenderedScreenHeight;
	const int width = min(snesWidth, avi_width/2);
	const int height = min(snesHeight, avi_height/2);
	const int pitch = GFX.Pitch;
#ifdef LSB_FIRST
	const bool order_is_rgb = (GUI.RedShift < GUI.BlueShift);
#else
	const bool order_is_rgb = (GUI.RedShift > GUI.BlueShift);
#endif
	const int src_step[2] = { -width*2, pitch - width*2 };
	const int dst_step = -(avi_pitch + width*3*2);
	const int image_offset = (avi_height - height*2) * avi_pitch;
	uint16 *s = GFX.Screen;
	uint8  *d = &avi_buffer[(avi_height - 1) * avi_pitch];

	for(int y = 0; y < height*2; y++)
	{
		for(int x = 0; x < width; x++)
		{
			if(order_is_rgb)
			{
				// Order is RGB
				uint32 pixel = *s++;
				*(d + 0) = *(d + 3) = (pixel >> (11 - 3)) & 0xf8;
				*(d + 1) = *(d + 4) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = *(d + 5) = (pixel & 0x1f) << 3;
				d += 6;
			}
			else
			{
				// Order is BGR
				uint32 pixel = *s++;
				*(d + 0) = *(d + 3) = (pixel & 0x1f) << 3;
				*(d + 1) = *(d + 4) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = *(d + 5) = (pixel >> (11 - 3)) & 0xf8;
				d += 6;
			}
		}
		s = (uint16*)((uint8*)s + src_step[y % 2]);
		d += dst_step;
	}

	// black out what we might have missed
	if(image_offset > 0)
		memset(avi_buffer, 0, image_offset);
}

// Dst: GFX.Screen 512xH  (2x1) 16bpp top-down
// Src: avi_buffer 256xH  (1x1) 24bpp bottom-up
static void BuildAVIVideoFrame1XHiRes (void)
{
	const int snesWidth = IPPU.RenderedScreenWidth;
	const int snesHeight = IPPU.RenderedScreenHeight;
	const int width = min(snesWidth/2, avi_width);
	const int height = min(snesHeight, avi_height);
	const int pitch = GFX.Pitch;
#ifdef LSB_FIRST
	const bool order_is_rgb = (GUI.RedShift < GUI.BlueShift);
#else
	const bool order_is_rgb = (GUI.RedShift > GUI.BlueShift);
#endif
	const int src_step = pitch - width*2*2;
	const int dst_step = -(avi_pitch + width*3);
	const int image_offset = (avi_height - height) * avi_pitch;
	uint16 *s = GFX.Screen;
	uint8  *d = &avi_buffer[(avi_height - 1) * avi_pitch];

	#define Interp(c1, c2) \
		(c1 == c2) ? c1 : \
		(((((c1 & 0x07E0)      + (c2 & 0x07E0)) >> 1) & 0x07E0) + \
		((((c1 & 0xF81F)      + (c2 & 0xF81F)) >> 1) & 0xF81F))

	for(int y = 0; y < height; y++)
	{
		for(int x = 0; x < width; x++)
		{
			if(order_is_rgb)
			{
				// Order is RGB
				uint32 pixel = Interp(s[0],s[1]);
				s += 2;
				*(d + 0) = (pixel >> (11 - 3)) & 0xf8;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel & 0x1f) << 3;
				d += 3;
			}
			else
			{
				// Order is BGR
				uint32 pixel = Interp(s[0],s[1]);
				s += 2;
				*(d + 0) = (pixel & 0x1f) << 3;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel >> (11 - 3)) & 0xf8;
				d += 3;
			}
		}
		s = (uint16*)((uint8*)s + src_step);
		d += dst_step;
	}

	// black out what we might have missed
	if(image_offset > 0)
		memset(avi_buffer, 0, image_offset);
}

// Dst: GFX.Screen 512xH  (1x1) 16bpp top-down
// Src: avi_buffer 512x2H (1x2) 24bpp bottom-up
static void BuildAVIVideoFrame2XHiRes (void)
{
	const int snesWidth = IPPU.RenderedScreenWidth;
	const int snesHeight = IPPU.RenderedScreenHeight;
	const int width = min(snesWidth, avi_width);
	const int height = min(snesHeight, avi_height/2);
	const int pitch = GFX.Pitch;
#ifdef LSB_FIRST
	const bool order_is_rgb = (GUI.RedShift < GUI.BlueShift);
#else
	const bool order_is_rgb = (GUI.RedShift > GUI.BlueShift);
#endif
	const int src_step[2] = { -width*2, pitch - width*2 };
	const int dst_step = -(avi_pitch + width*3);
	const int image_offset = (avi_height - height*2) * avi_pitch;
	uint16 *s = GFX.Screen;
	uint8  *d = &avi_buffer[(avi_height - 1) * avi_pitch];

	for(int y = 0; y < height*2; y++)
	{
		for(int x = 0; x < width; x++)
		{
			if(order_is_rgb)
			{
				// Order is RGB
				uint32 pixel = *s++;
				*(d + 0) = (pixel >> (11 - 3)) & 0xf8;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel & 0x1f) << 3;
				d += 3;
			}
			else
			{
				// Order is BGR
				uint32 pixel = *s++;
				*(d + 0) = (pixel & 0x1f) << 3;
				*(d + 1) = (pixel >> (6 - 3)) & 0xf8;
				*(d + 2) = (pixel >> (11 - 3)) & 0xf8;
				d += 3;
			}
		}
		s = (uint16*)((uint8*)s + src_step[y % 2]);
		d += dst_step;
	}

	// black out what we might have missed
	if(image_offset > 0)
		memset(avi_buffer, 0, image_offset);
}
