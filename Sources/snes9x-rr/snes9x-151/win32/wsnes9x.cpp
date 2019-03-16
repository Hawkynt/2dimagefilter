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

#pragma comment(linker, \
    "\"/manifestdependency:type='Win32' "\
    "name='Microsoft.Windows.Common-Controls' "\
    "version='6.0.0.0' "\
    "processorArchitecture='*' "\
    "publicKeyToken='6595b64144ccf1df' "\
    "language='*'\"")


//  Win32 GUI code
//  (c) Copyright 2003-2006 blip, Nach, Matthew Kendora, funkyass and nitsuja

#ifdef __MINGW32__
#define _WIN32_IE 0x0501
#define _WIN32_WINNT 0x0501
#endif

#include <shlobj.h>
#include <objidl.h>
#include <shlwapi.h>

#define COMPILE_MULTIMON_STUBS
#include <multimon.h>

#include "wsnes9x.h"
#include "CDirectDraw.h"
#include "../snes9x.h"
#include "../version.h"
#include "../memmap.h"
#include "../cpuexec.h"
#include "../display.h"
#include "../3d.h"
#include "../cheats.h"
#include "../netplay.h"
#include "../apu.h"
#include "../movie.h"
#include "../controls.h"
#include "../conffile.h"
#include "../soundux.h"
#include "../disasm.h"
#include "WAVOutput.h"
#include "AVIOutput.h"
#include "InputCustom.h"
#include "lazymacro.h"
#include "ram_search.h"
#include "ramwatch.h"
#include "oldramsearch.h"
#include <vector>
#include <queue>

#include "direct3d.h"

#include <tchar.h>

#if (((defined(_MSC_VER) && _MSC_VER >= 1300)) || defined(__MINGW32__))
	// both MINGW and VS.NET use fstream instead of fstream.h which is deprecated
	#include <fstream>
	using namespace std;
#else
	// for VC++ 6
	#include <fstream.h>
#endif

#include <sys/stat.h>
//#include "string_cache.h"
#include "wlanguage.h"
#include "../language.h"

//uncomment to find memory leaks, with a line in WinMain
//#define CHECK_MEMORY_LEAKS

#ifdef CHECK_MEMORY_LEAKS
	#include <crtdbg.h>
#endif

#include <commctrl.h>
#include <winsock.h>
#include <io.h>
#include <time.h>
#include <direct.h>
#include <stddef.h>

extern SNPServer NPServer;

#ifdef USE_OPENGL
OpenGLData OpenGL;
#endif

#include <ctype.h>

#include "../s9xlua.h"

#ifdef _MSC_VER
#define F_OK 0
#define X_OK 1
#define W_OK 2
#define R_OK 4
#endif

__int64 PCBase, PCFrameTime, PCFrameTimeNTSC, PCFrameTimePAL, PCStart, PCEnd;
DWORD PCStartTicks, PCEndTicks;

#ifdef RTC_DEBUGGER
INT_PTR CALLBACK SPC7110rtc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
#endif
INT_PTR CALLBACK DlgSP7PackConfig(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgSoundConf(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgInfoProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgAboutProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgEmulatorProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);

INT_PTR CALLBACK DlgOpenROMProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgMultiROMProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DlgChildSplitProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgNPProgress(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgPackConfigProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgNetConnect(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgNPOptions(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgFrameSkipSettings(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgFunky(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgInputConfig(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgHotkeyConfig(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgCreateMovie(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgOpenMovie(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
HRESULT CALLBACK EnumModesCallback( LPDDSURFACEDESC lpDDSurfaceDesc, LPVOID lpContext);
INT_PTR CALLBACK DlgSeekProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK DlgStringInputProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);

extern HWND LuaConsoleHWnd;
extern INT_PTR CALLBACK DlgLuaScriptDialog(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);

INT_PTR CALLBACK test(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);


#ifdef RTC_DEBUGGER
//Messages for sliders (for some reason they don't get included during the build)
#define TBM_GETPOS			(WM_USER)
#define TBM_GETRANGEMIN		(WM_USER+1)
#define TBM_GETRANGEMAX		(WM_USER+2)
#define TBM_GETTIC			(WM_USER+3)
#define TBM_SETTIC			(WM_USER+4)
#define TBM_SETPOS			(WM_USER+5)
#define TBM_SETRANGE			(WM_USER+6)
#define TBM_SETRANGEMIN		(WM_USER+7)
#define TBM_SETRANGEMAX		(WM_USER+8)
#define TBM_CLEARTICS		(WM_USER+9)
#define TBM_SETSEL			(WM_USER+10)
#define TBM_SETSELSTART		(WM_USER+11)
#define TBM_SETSELEND		(WM_USER+12)
#define TBM_GETPTICS			(WM_USER+14)
#define TBM_GETTICPOS		(WM_USER+15)
#define TBM_GETNUMTICS		(WM_USER+16)
#define TBM_GETSELSTART		(WM_USER+17)
#define TBM_GETSELEND		(WM_USER+18)
#define TBM_CLEARSEL			(WM_USER+19)
#define TBM_SETTICFREQ		(WM_USER+20)
#define TBM_SETPAGESIZE		(WM_USER+21)
#define TBM_GETPAGESIZE		(WM_USER+22)
#define TBM_SETLINESIZE		(WM_USER+23)
#define TBM_GETLINESIZE		(WM_USER+24)
#define TBM_GETTHUMBRECT	(WM_USER+25)
#define TBM_GETCHANNELRECT	(WM_USER+26)
#define TBM_SETTHUMBLENGTH	(WM_USER+27)
#define TBM_GETTHUMBLENGTH	(WM_USER+28)
#endif

#define NOTKNOWN "Unknown Company "
#define HEADER_SIZE 512
#define INFO_LEN (0xFF - 0xC0)

#define WM_CUSTKEYDOWN	(WM_USER+50)
#define WM_CUSTKEYUP	(WM_USER+51)

static int KeyInDelayMSec;
static int KeyInRepeatMSec;

extern HWND RamSearchHWnd;
extern LRESULT CALLBACK RamSearchProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);

#define NUM_HOTKEY_CONTROLS 20

const int IDC_LABEL_HK_Table[NUM_HOTKEY_CONTROLS] = {
	IDC_LABEL_HK1 , IDC_LABEL_HK2 , IDC_LABEL_HK3 , IDC_LABEL_HK4 , IDC_LABEL_HK5 ,
	IDC_LABEL_HK6 , IDC_LABEL_HK7 , IDC_LABEL_HK8 , IDC_LABEL_HK9 , IDC_LABEL_HK10,
	IDC_LABEL_HK11, IDC_LABEL_HK12, IDC_LABEL_HK13, IDC_LABEL_HK14, IDC_LABEL_HK15,
	IDC_LABEL_HK16, IDC_LABEL_HK17, IDC_LABEL_HK18, IDC_LABEL_HK19, IDC_LABEL_HK20,
};
const int IDC_HOTKEY_Table[NUM_HOTKEY_CONTROLS] = {
	IDC_HOTKEY1 , IDC_HOTKEY2 , IDC_HOTKEY3 , IDC_HOTKEY4 , IDC_HOTKEY5 ,
	IDC_HOTKEY6 , IDC_HOTKEY7 , IDC_HOTKEY8 , IDC_HOTKEY9 , IDC_HOTKEY10,
	IDC_HOTKEY11, IDC_HOTKEY12, IDC_HOTKEY13, IDC_HOTKEY14, IDC_HOTKEY15,
	IDC_HOTKEY16, IDC_HOTKEY17, IDC_HOTKEY18, IDC_HOTKEY19, IDC_HOTKEY20,
};

LPCTSTR hotkeyPageTitle[] = {
	_T("File"),
	_T("Savestate #1"),
	_T("Savestate #2"),
	_T("Controls #1"),
	_T("Controls #2"),
	_T("Speed"),
	_T("Graphics"),
	_T("Sound"),
	_T("Movie / Display"),
	_T("Tools"),
	_T("NUM_HOTKEY_PAGE"),
};

/*****************************************************************************/
/* Global variables                                                          */
/*****************************************************************************/
struct sGUI GUI;
typedef struct sExtList
{
	TCHAR* extension;
	bool compressed;
	struct sExtList* next;
} ExtList;
HANDLE SoundEvent;

ExtList* valid_ext=NULL;
void MakeExtFile(void);
void LoadExts(void);

extern FILE *trace_fs;
extern SCheatData Cheat;
extern bool8 do_frame_adjust;

HINSTANCE g_hInst;

#ifdef DEBUGGER
extern "C" void Trace ();
#endif



static const char *rom_filename = NULL;

CDirect3D Direct3D;
CDirectDraw DirectDraw;

struct SJoypad Joypad[16] = {
    {
        true,					/* Joypad 1 enabled */
			VK_LEFT, VK_RIGHT, VK_UP, VK_DOWN,	/* Left, Right, Up, Down */
			0, 0, 0, 0,             /* Left_Up, Left_Down, Right_Up, Right_Down */
			VK_SPACE, VK_RETURN,    /* Start, Select */
			'V', 'C',				/* A B */
			'D', 'X',				/* X Y */
			'A', 'S'				/* L R */
    },
    {
			true,                                  /* Joypad 2 enabled */
				'J', 'L', 'I', 'K',	/* Left, Right, Up, Down */
				0, 0, 0, 0,         /* Left_Up, Left_Down, Right_Up, Right_Down */
				'P', 'O',          /* Start, Select */
				'H', 'G',			/* A B */
				'T', 'F',			/* X Y */
				'Y', 'U'			/* L R */
		},
		{
				false,                                  /* Joypad 3 disabled */
					0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0,
					0, 0, 0, 0, 0, 0
			},
			{
					false,                                  /* Joypad 4 disabled */
						0, 0, 0, 0,
						0, 0, 0, 0,
						0, 0,
						0, 0, 0, 0, 0, 0
				},
				{
						false,                                  /* Joypad 5 disabled */
							0, 0, 0, 0,
							0, 0, 0, 0,
							0, 0,
							0, 0, 0, 0, 0, 0
					},
				{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },/* Joypad 6 disabled */
				{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },/* Joypad 7 disabled */
				{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },/* Joypad 8 disabled */
	{
			false,                                  /* Joypad 1 Turbo disabled */
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,                                  /* Joypad 2 Turbo disabled */
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,                                  /* Joypad 3 Turbo disabled */
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,                                  /* Joypad 4 Turbo disabled */
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,                                  /* Joypad 5 Turbo disabled */
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },/* Joypad 6 Turbo disabled */
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },/* Joypad 7 Turbo disabled */
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },/* Joypad 8 Turbo disabled */
};

// stores on/off toggle info for each key of each controller
SJoypad ToggleJoypadStorage [8] = {
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },
};

SJoypad TurboToggleJoypadStorage [8] = {
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
	{
			false,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0,
				0, 0, 0, 0, 0, 0
		},
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },
			{ false, 0, 0, 0, 0,  0, 0, 0, 0,  0, 0,  0, 0, 0, 0, 0, 0 },
};

SCustomKeys CustomKeys;
void HotkeyUpResetGame ();
void HotkeyUpScopePause ();
void HotkeyUpFastForward ();
void HotkeyRecentROM0 (bool justPressed);
void HotkeyRecentROM1 (bool justPressed);
void HotkeyRecentROM2 (bool justPressed);
void HotkeyRecentROM3 (bool justPressed);
void HotkeyRecentROM4 (bool justPressed);
void HotkeyRecentROM5 (bool justPressed);
void HotkeyRecentROM6 (bool justPressed);
void HotkeyRecentROM7 (bool justPressed);
void HotkeyRecentROM8 (bool justPressed);
void HotkeyRecentROM9 (bool justPressed);
void HotkeyOpenROM (bool justPressed);
void HotkeyOpenMultiCart (bool justPressed);
void HotkeyPause (bool justPressed);
void HotkeyResetGame (bool justPressed);
void HotkeySaveScreenShot (bool justPressed);
void HotkeySaveSPC (bool justPressed);
void HotkeySaveSRAM (bool justPressed);
void HotkeySaveSPC7110Log (bool justPressed);
void HotkeyRecordAVI (bool justPressed);
void HotkeySave0 (bool justPressed);
void HotkeySave1 (bool justPressed);
void HotkeySave2 (bool justPressed);
void HotkeySave3 (bool justPressed);
void HotkeySave4 (bool justPressed);
void HotkeySave5 (bool justPressed);
void HotkeySave6 (bool justPressed);
void HotkeySave7 (bool justPressed);
void HotkeySave8 (bool justPressed);
void HotkeySave9 (bool justPressed);
void HotkeyLoad0 (bool justPressed);
void HotkeyLoad1 (bool justPressed);
void HotkeyLoad2 (bool justPressed);
void HotkeyLoad3 (bool justPressed);
void HotkeyLoad4 (bool justPressed);
void HotkeyLoad5 (bool justPressed);
void HotkeyLoad6 (bool justPressed);
void HotkeyLoad7 (bool justPressed);
void HotkeyLoad8 (bool justPressed);
void HotkeyLoad9 (bool justPressed);
void HotkeySelectSave0 (bool justPressed);
void HotkeySelectSave1 (bool justPressed);
void HotkeySelectSave2 (bool justPressed);
void HotkeySelectSave3 (bool justPressed);
void HotkeySelectSave4 (bool justPressed);
void HotkeySelectSave5 (bool justPressed);
void HotkeySelectSave6 (bool justPressed);
void HotkeySelectSave7 (bool justPressed);
void HotkeySelectSave8 (bool justPressed);
void HotkeySelectSave9 (bool justPressed);
void HotkeySlotPlus (bool justPressed);
void HotkeySlotMinus (bool justPressed);
void HotkeySlotSave (bool justPressed);
void HotkeySlotLoad (bool justPressed);
void HotkeyToggleJoypad0 (bool justPressed);
void HotkeyToggleJoypad1 (bool justPressed);
void HotkeyToggleJoypad2 (bool justPressed);
void HotkeyToggleJoypad3 (bool justPressed);
void HotkeyToggleJoypad4 (bool justPressed);
void HotkeyToggleJoypad5 (bool justPressed);
void HotkeyToggleJoypad6 (bool justPressed);
void HotkeyToggleJoypad7 (bool justPressed);
void HotkeyJoypadSwap (bool justPressed);
void HotkeySwitchControllers (bool justPressed);
void HotkeyTurboA (bool justPressed);
void HotkeyTurboB (bool justPressed);
void HotkeyTurboY (bool justPressed);
void HotkeyTurboX (bool justPressed);
void HotkeyTurboL (bool justPressed);
void HotkeyTurboR (bool justPressed);
void HotkeyTurboStart (bool justPressed);
void HotkeyTurboSelect (bool justPressed);
void HotkeyTurboLeft (bool justPressed);
void HotkeyTurboUp (bool justPressed);
void HotkeyTurboRight (bool justPressed);
void HotkeyTurboDown (bool justPressed);
void HotkeyScopeTurbo (bool justPressed);
void HotkeyScopePause (bool justPressed);
void HotkeySpeedUp (bool justPressed);
void HotkeySpeedDown (bool justPressed);
void HotkeySkipUp (bool justPressed);
void HotkeySkipDown (bool justPressed);
void HotkeyFastForward (bool justPressed);
void HotkeyToggleFastForward (bool justPressed);
void HotkeyFrameAdvance (bool justPressed);
void HotkeyBGL1 (bool justPressed);
void HotkeyBGL2 (bool justPressed);
void HotkeyBGL3 (bool justPressed);
void HotkeyBGL4 (bool justPressed);
void HotkeyBGL5 (bool justPressed);
void HotkeyClippingWindows (bool justPressed);
void HotkeyTransparency (bool justPressed);
void HotkeyHDMA (bool justPressed);
void HotkeyBGLHack (bool justPressed);
void HotkeyInterpMode7 (bool justPressed);
void HotkeyGLCube (bool justPressed);
void HotkeyToggleSound0 (bool justPressed);
void HotkeyToggleSound1 (bool justPressed);
void HotkeyToggleSound2 (bool justPressed);
void HotkeyToggleSound3 (bool justPressed);
void HotkeyToggleSound4 (bool justPressed);
void HotkeyToggleSound5 (bool justPressed);
void HotkeyToggleSound6 (bool justPressed);
void HotkeyToggleSound7 (bool justPressed);
void HotkeyMoviePlay (bool justPressed);
void HotkeyMovieRecord (bool justPressed);
void HotkeyMovieStop (bool justPressed);
void HotkeyReadOnly (bool justPressed);
void HotkeyShowPressed (bool justPressed);
void HotkeyFrameAndLagCount (bool justPressed);
void HotkeyFrameCount (bool justPressed);
void HotkeyLagCount (bool justPressed);
void HotkeyResetLagCounter (bool justPressed);
void HotkeyToggleMacro0 (bool justPressed);
void HotkeyToggleMacro1 (bool justPressed);
void HotkeyToggleMacro2 (bool justPressed);
void HotkeyToggleMacro3 (bool justPressed);
void HotkeyToggleMacro4 (bool justPressed);
void HotkeyToggleMacro5 (bool justPressed);
void HotkeyToggleMacro6 (bool justPressed);
void HotkeyToggleMacro7 (bool justPressed);
void HotkeyEditMacro (bool justPressed);
void HotkeyToggleCheats (bool justPressed);
void HotkeyLoadLuaScript(bool justPressed);
void HotkeyReloadLuaScript(bool justPressed);
void HotkeyStopLuaScript(bool justPressed);

bool IsLastCustomKey (const SCustomKey *key)
{
	return (key->key == 0xFFFF && key->modifiers == 0xFFFF);
}

void SetLastCustomKey (SCustomKey *key)
{
	key->key = 0xFFFF;
	key->modifiers = 0xFFFF;
}

void ZeroCustomKeys (SCustomKeys *keys)
{
	UINT i = 0;

	SetLastCustomKey(&keys->LastItem);
	while (!IsLastCustomKey(&keys->key[i])) {
		keys->key[i].key = 0;
		keys->key[i].modifiers = 0;
		i++;
	};
}

void InitCustomKeys (SCustomKeys *keys)
{
	UINT i = 0;

	SetLastCustomKey(&keys->LastItem);
	while (!IsLastCustomKey(&keys->key[i])) {
		keys->key[i].key = 0;
		keys->key[i].modifiers = 0;
		keys->key[i].handleKeyDown = NULL;
		keys->key[i].handleKeyUp = NULL;
		keys->key[i].page = NUM_HOTKEY_PAGE;
		keys->key[i].name = NULL;
		keys->key[i].timing = PROCESS_NOW;
		i++;
	};

	// set handlers
	keys->RecentROM[0].handleKeyDown = HotkeyRecentROM0;
	keys->RecentROM[1].handleKeyDown = HotkeyRecentROM1;
	keys->RecentROM[2].handleKeyDown = HotkeyRecentROM2;
	keys->RecentROM[3].handleKeyDown = HotkeyRecentROM3;
	keys->RecentROM[4].handleKeyDown = HotkeyRecentROM4;
	keys->RecentROM[5].handleKeyDown = HotkeyRecentROM5;
	keys->RecentROM[6].handleKeyDown = HotkeyRecentROM6;
	keys->RecentROM[7].handleKeyDown = HotkeyRecentROM7;
	keys->RecentROM[8].handleKeyDown = HotkeyRecentROM8;
	keys->RecentROM[9].handleKeyDown = HotkeyRecentROM9;
	keys->OpenROM.handleKeyDown = HotkeyOpenROM;
	keys->OpenMultiCart.handleKeyDown = HotkeyOpenMultiCart;
	keys->Pause.handleKeyDown = HotkeyPause;
	keys->ResetGame.handleKeyDown = HotkeyResetGame; keys->ResetGame.handleKeyUp = HotkeyUpResetGame;
	keys->SaveScreenShot.handleKeyDown = HotkeySaveScreenShot;
	keys->SaveSPC.handleKeyDown = HotkeySaveSPC;
	keys->SaveSRAM.handleKeyDown = HotkeySaveSRAM;
	keys->SaveSPC7110Log.handleKeyDown = HotkeySaveSPC7110Log;
	keys->RecordAVI.handleKeyDown = HotkeyRecordAVI;
	keys->Save[0].handleKeyDown = HotkeySave0;
	keys->Save[1].handleKeyDown = HotkeySave1;
	keys->Save[2].handleKeyDown = HotkeySave2;
	keys->Save[3].handleKeyDown = HotkeySave3;
	keys->Save[4].handleKeyDown = HotkeySave4;
	keys->Save[5].handleKeyDown = HotkeySave5;
	keys->Save[6].handleKeyDown = HotkeySave6;
	keys->Save[7].handleKeyDown = HotkeySave7;
	keys->Save[8].handleKeyDown = HotkeySave8;
	keys->Save[9].handleKeyDown = HotkeySave9;
	keys->Load[0].handleKeyDown = HotkeyLoad0;
	keys->Load[1].handleKeyDown = HotkeyLoad1;
	keys->Load[2].handleKeyDown = HotkeyLoad2;
	keys->Load[3].handleKeyDown = HotkeyLoad3;
	keys->Load[4].handleKeyDown = HotkeyLoad4;
	keys->Load[5].handleKeyDown = HotkeyLoad5;
	keys->Load[6].handleKeyDown = HotkeyLoad6;
	keys->Load[7].handleKeyDown = HotkeyLoad7;
	keys->Load[8].handleKeyDown = HotkeyLoad8;
	keys->Load[9].handleKeyDown = HotkeyLoad9;
	keys->SelectSave[0].handleKeyDown = HotkeySelectSave0;
	keys->SelectSave[1].handleKeyDown = HotkeySelectSave1;
	keys->SelectSave[2].handleKeyDown = HotkeySelectSave2;
	keys->SelectSave[3].handleKeyDown = HotkeySelectSave3;
	keys->SelectSave[4].handleKeyDown = HotkeySelectSave4;
	keys->SelectSave[5].handleKeyDown = HotkeySelectSave5;
	keys->SelectSave[6].handleKeyDown = HotkeySelectSave6;
	keys->SelectSave[7].handleKeyDown = HotkeySelectSave7;
	keys->SelectSave[8].handleKeyDown = HotkeySelectSave8;
	keys->SelectSave[9].handleKeyDown = HotkeySelectSave9;
	keys->SlotPlus.handleKeyDown = HotkeySlotPlus;
	keys->SlotMinus.handleKeyDown = HotkeySlotMinus;
	keys->SlotSave.handleKeyDown = HotkeySlotSave;
	keys->SlotLoad.handleKeyDown = HotkeySlotLoad;
	keys->ToggleJoypad[0].handleKeyDown = HotkeyToggleJoypad0;
	keys->ToggleJoypad[1].handleKeyDown = HotkeyToggleJoypad1;
	keys->ToggleJoypad[2].handleKeyDown = HotkeyToggleJoypad2;
	keys->ToggleJoypad[3].handleKeyDown = HotkeyToggleJoypad3;
	keys->ToggleJoypad[4].handleKeyDown = HotkeyToggleJoypad4;
	keys->ToggleJoypad[5].handleKeyDown = HotkeyToggleJoypad5;
	keys->ToggleJoypad[6].handleKeyDown = HotkeyToggleJoypad6;
	keys->ToggleJoypad[7].handleKeyDown = HotkeyToggleJoypad7;
	keys->JoypadSwap.handleKeyDown = HotkeyJoypadSwap;
	keys->SwitchControllers.handleKeyDown = HotkeySwitchControllers;
	keys->TurboA.handleKeyDown = HotkeyTurboA;
	keys->TurboB.handleKeyDown = HotkeyTurboB;
	keys->TurboY.handleKeyDown = HotkeyTurboY;
	keys->TurboX.handleKeyDown = HotkeyTurboX;
	keys->TurboL.handleKeyDown = HotkeyTurboL;
	keys->TurboR.handleKeyDown = HotkeyTurboR;
	keys->TurboStart.handleKeyDown = HotkeyTurboStart;
	keys->TurboSelect.handleKeyDown = HotkeyTurboSelect;
	keys->TurboLeft.handleKeyDown = HotkeyTurboLeft;
	keys->TurboUp.handleKeyDown = HotkeyTurboUp;
	keys->TurboRight.handleKeyDown = HotkeyTurboRight;
	keys->TurboDown.handleKeyDown = HotkeyTurboDown;
	keys->ScopeTurbo.handleKeyDown = HotkeyScopeTurbo;
	keys->ScopePause.handleKeyDown = HotkeyScopePause; keys->ScopePause.handleKeyUp = HotkeyUpScopePause;
	keys->SpeedUp.handleKeyDown = HotkeySpeedUp;
	keys->SpeedDown.handleKeyDown = HotkeySpeedDown;
	keys->SkipUp.handleKeyDown = HotkeySkipUp;
	keys->SkipDown.handleKeyDown = HotkeySkipDown;
	keys->FastForward.handleKeyDown = HotkeyFastForward; keys->FastForward.handleKeyUp = HotkeyUpFastForward;
	keys->ToggleFastForward.handleKeyDown = HotkeyToggleFastForward;
	keys->FrameAdvance.handleKeyDown = HotkeyFrameAdvance;
	keys->BGL1.handleKeyDown = HotkeyBGL1;
	keys->BGL2.handleKeyDown = HotkeyBGL2;
	keys->BGL3.handleKeyDown = HotkeyBGL3;
	keys->BGL4.handleKeyDown = HotkeyBGL4;
	keys->BGL5.handleKeyDown = HotkeyBGL5;
	keys->ClippingWindows.handleKeyDown = HotkeyClippingWindows;
	keys->Transparency.handleKeyDown = HotkeyTransparency;
	keys->HDMA.handleKeyDown = HotkeyHDMA;
	keys->BGLHack.handleKeyDown = HotkeyBGLHack;
	keys->InterpMode7.handleKeyDown = HotkeyInterpMode7;
	keys->GLCube.handleKeyDown = HotkeyGLCube;
	keys->ToggleSound[0].handleKeyDown = HotkeyToggleSound0;
	keys->ToggleSound[1].handleKeyDown = HotkeyToggleSound1;
	keys->ToggleSound[2].handleKeyDown = HotkeyToggleSound2;
	keys->ToggleSound[3].handleKeyDown = HotkeyToggleSound3;
	keys->ToggleSound[4].handleKeyDown = HotkeyToggleSound4;
	keys->ToggleSound[5].handleKeyDown = HotkeyToggleSound5;
	keys->ToggleSound[6].handleKeyDown = HotkeyToggleSound6;
	keys->ToggleSound[7].handleKeyDown = HotkeyToggleSound7;
	keys->MoviePlay.handleKeyDown = HotkeyMoviePlay;
	keys->MovieRecord.handleKeyDown = HotkeyMovieRecord;
	keys->MovieStop.handleKeyDown = HotkeyMovieStop;
	keys->ReadOnly.handleKeyDown = HotkeyReadOnly;
	keys->ShowPressed.handleKeyDown = HotkeyShowPressed;
	keys->FrameCountOnly.handleKeyDown = HotkeyFrameCount;
	keys->LagCountOnly.handleKeyDown = HotkeyLagCount;
	keys->FrameCount.handleKeyDown = HotkeyFrameAndLagCount;
	keys->ResetLagCounter.handleKeyDown = HotkeyResetLagCounter;
	keys->ToggleMacro[0].handleKeyDown = HotkeyToggleMacro0;
	keys->ToggleMacro[1].handleKeyDown = HotkeyToggleMacro1;
	keys->ToggleMacro[2].handleKeyDown = HotkeyToggleMacro2;
	keys->ToggleMacro[3].handleKeyDown = HotkeyToggleMacro3;
	keys->ToggleMacro[4].handleKeyDown = HotkeyToggleMacro4;
	keys->ToggleMacro[5].handleKeyDown = HotkeyToggleMacro5;
	keys->ToggleMacro[6].handleKeyDown = HotkeyToggleMacro6;
	keys->ToggleMacro[7].handleKeyDown = HotkeyToggleMacro7;
	keys->EditMacro.handleKeyDown = HotkeyEditMacro;
	keys->ToggleCheats.handleKeyDown = HotkeyToggleCheats;
	keys->LoadLuaScript.handleKeyDown = HotkeyLoadLuaScript;
	keys->ReloadLuaScript.handleKeyDown = HotkeyReloadLuaScript;
	keys->StopLuaScript.handleKeyDown = HotkeyStopLuaScript;

	// name
	keys->RecentROM[0].name = _T("Recent ROM 1");
	keys->RecentROM[1].name = _T("Recent ROM 2");
	keys->RecentROM[2].name = _T("Recent ROM 3");
	keys->RecentROM[3].name = _T("Recent ROM 4");
	keys->RecentROM[4].name = _T("Recent ROM 5");
	keys->RecentROM[5].name = _T("Recent ROM 6");
	keys->RecentROM[6].name = _T("Recent ROM 7");
	keys->RecentROM[7].name = _T("Recent ROM 8");
	keys->RecentROM[8].name = _T("Recent ROM 9");
	keys->RecentROM[9].name = _T("Recent ROM 10");
	keys->OpenROM.name = _T("Open ROM...");
	keys->OpenMultiCart.name = _T("Open MultiCart...");
	keys->Pause.name = _T("Pause");
	keys->ResetGame.name = _T("Reset");
	keys->SaveScreenShot.name = _T("Save Screenshot");
	keys->SaveSPC.name = _T("Save SPC");
	keys->SaveSRAM.name = _T("Save SRAM");
	keys->SaveSPC7110Log.name = _T("Save SPC7110 Log");
	keys->RecordAVI.name = _T("Record/Stop AVI");
	keys->Save[0].name = _T("Save State to Slot 0");
	keys->Save[1].name = _T("Save State to Slot 1");
	keys->Save[2].name = _T("Save State to Slot 2");
	keys->Save[3].name = _T("Save State to Slot 3");
	keys->Save[4].name = _T("Save State to Slot 4");
	keys->Save[5].name = _T("Save State to Slot 5");
	keys->Save[6].name = _T("Save State to Slot 6");
	keys->Save[7].name = _T("Save State to Slot 7");
	keys->Save[8].name = _T("Save State to Slot 8");
	keys->Save[9].name = _T("Save State to Slot 9");
	keys->Load[0].name = _T("Load State to Slot 0");
	keys->Load[1].name = _T("Load State to Slot 1");
	keys->Load[2].name = _T("Load State to Slot 2");
	keys->Load[3].name = _T("Load State to Slot 3");
	keys->Load[4].name = _T("Load State to Slot 4");
	keys->Load[5].name = _T("Load State to Slot 5");
	keys->Load[6].name = _T("Load State to Slot 6");
	keys->Load[7].name = _T("Load State to Slot 7");
	keys->Load[8].name = _T("Load State to Slot 8");
	keys->Load[9].name = _T("Load State to Slot 9");
	keys->SelectSave[0].name = _T("Savestate Slot 0");
	keys->SelectSave[1].name = _T("Savestate Slot 1");
	keys->SelectSave[2].name = _T("Savestate Slot 2");
	keys->SelectSave[3].name = _T("Savestate Slot 3");
	keys->SelectSave[4].name = _T("Savestate Slot 4");
	keys->SelectSave[5].name = _T("Savestate Slot 5");
	keys->SelectSave[6].name = _T("Savestate Slot 6");
	keys->SelectSave[7].name = _T("Savestate Slot 7");
	keys->SelectSave[8].name = _T("Savestate Slot 8");
	keys->SelectSave[9].name = _T("Savestate Slot 9");
	keys->SlotPlus.name = _T("Next Savestate Slot");
	keys->SlotMinus.name = _T("Previous Savestate Slot");
	keys->SlotSave.name = _T("Save State");
	keys->SlotLoad.name = _T("Load State");
	keys->ToggleJoypad[0].name = _T("Joypad 1 On/Off");
	keys->ToggleJoypad[1].name = _T("Joypad 2 On/Off");
	keys->ToggleJoypad[2].name = _T("Joypad 3 On/Off");
	keys->ToggleJoypad[3].name = _T("Joypad 4 On/Off");
	keys->ToggleJoypad[4].name = _T("Joypad 5 On/Off");
	keys->ToggleJoypad[5].name = _T("Joypad 6 On/Off");
	keys->ToggleJoypad[6].name = _T("Joypad 7 On/Off");
	keys->ToggleJoypad[7].name = _T("Joypad 8 On/Off");
	keys->JoypadSwap.name = _T("Joypad Swap");
	keys->SwitchControllers.name = _T("Switch Controllers");
	keys->TurboA.name = _T("Turbo A mode");
	keys->TurboB.name = _T("Turbo B mode");
	keys->TurboY.name = _T("Turbo Y mode");
	keys->TurboX.name = _T("Turbo X mode");
	keys->TurboL.name = _T("Turbo L mode");
	keys->TurboR.name = _T("Turbo R mode");
	keys->TurboStart.name = _T("Turbo Start mode");
	keys->TurboSelect.name = _T("Turbo Select mode");
	keys->TurboLeft.name = _T("Turbo Left mode");
	keys->TurboUp.name = _T("Turbo Up mode");
	keys->TurboRight.name = _T("Turbo Right mode");
	keys->TurboDown.name = _T("Turbo Down mode");
	keys->ScopeTurbo.name = _T("Super Scope Turbo");
	keys->ScopePause.name = _T("Super Scope Pause");
	keys->SpeedUp.name = _T("Speed Up");
	keys->SpeedDown.name = _T("Speed Down");
	keys->SkipUp.name = _T("Increase Frame Skip");
	keys->SkipDown.name = _T("Decrease Frame Skip");
	keys->FastForward.name = _T("Fast Forward");
	keys->ToggleFastForward.name = _T("Fast Forward Toggle");
	keys->FrameAdvance.name = _T("Frame Advance");
	keys->BGL1.name = _T("Graphics Layer 1");
	keys->BGL2.name = _T("Graphics Layer 2");
	keys->BGL3.name = _T("Graphics Layer 3");
	keys->BGL4.name = _T("Graphics Layer 4");
	keys->BGL5.name = _T("Sprites Layer");
	keys->ClippingWindows.name = _T("Clipping Windows");
	keys->Transparency.name = _T("Transparency");
	keys->HDMA.name = _T("HDMA Emulation");
	keys->GLCube.name = _T("GLCube Mode");
	keys->BGLHack.name = _T("BG Layering hack");
	keys->InterpMode7.name = _T("Interpolate Mode 7");
	keys->ToggleSound[0].name = _T("Sound Channel 0");
	keys->ToggleSound[1].name = _T("Sound Channel 1");
	keys->ToggleSound[2].name = _T("Sound Channel 2");
	keys->ToggleSound[3].name = _T("Sound Channel 3");
	keys->ToggleSound[4].name = _T("Sound Channel 4");
	keys->ToggleSound[5].name = _T("Sound Channel 5");
	keys->ToggleSound[6].name = _T("Sound Channel 6");
	keys->ToggleSound[7].name = _T("Sound Channel 7");
	keys->MoviePlay.name = _T("Movie Play...");
	keys->MovieRecord.name = _T("Movie Record...");
	keys->MovieStop.name = _T("Movie Stop");
	keys->ReadOnly.name = _T("Read-Only Toggle");
	keys->ShowPressed.name = _T("Input Display Toggle");
	keys->FrameCount.name = _T("Frame/Lag Counter Toggle");
	keys->FrameCountOnly.name = _T("Frame Counter Toggle");
	keys->LagCountOnly.name = _T("Lag Counter Toggle");
	keys->ResetLagCounter.name = _T("Lag Counter Reset");
	keys->ToggleMacro[0].name = _T("Macro 0");
	keys->ToggleMacro[1].name = _T("Macro 1");
	keys->ToggleMacro[2].name = _T("Macro 2");
	keys->ToggleMacro[3].name = _T("Macro 3");
	keys->ToggleMacro[4].name = _T("Macro 4");
	keys->ToggleMacro[5].name = _T("Macro 5");
	keys->ToggleMacro[6].name = _T("Macro 6");
	keys->ToggleMacro[7].name = _T("Macro 7");
	keys->EditMacro.name = _T("Edit Macro");
	keys->ToggleCheats.name = _T("Toggle Cheats");
	keys->LoadLuaScript.name = _T("Load Lua Script");
	keys->ReloadLuaScript.name = _T("Reload Lua Script");
	keys->StopLuaScript.name = _T("Stop Lua Script");

	// category
	keys->RecentROM[0].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[1].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[2].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[3].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[4].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[5].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[6].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[7].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[8].page = HOTKEY_PAGE_FILE;
	keys->RecentROM[9].page = HOTKEY_PAGE_FILE;
	keys->OpenROM.page = HOTKEY_PAGE_FILE;
	keys->OpenMultiCart.page = HOTKEY_PAGE_FILE;
	keys->Pause.page = HOTKEY_PAGE_FILE;
	keys->ResetGame.page = HOTKEY_PAGE_FILE;
	keys->SaveScreenShot.page = HOTKEY_PAGE_FILE;
	keys->SaveSPC.page = HOTKEY_PAGE_FILE;
	keys->SaveSRAM.page = HOTKEY_PAGE_FILE;
	keys->SaveSPC7110Log.page = HOTKEY_PAGE_FILE;
	keys->RecordAVI.page = HOTKEY_PAGE_FILE;
	keys->Save[0].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[1].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[2].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[3].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[4].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[5].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[6].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[7].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[8].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Save[9].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[0].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[1].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[2].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[3].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[4].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[5].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[6].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[7].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[8].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->Load[9].page = HOTKEY_PAGE_SAVESTATE_1;
	keys->SelectSave[0].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[1].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[2].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[3].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[4].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[5].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[6].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[7].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[8].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SelectSave[9].page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SlotPlus.page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SlotMinus.page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SlotSave.page = HOTKEY_PAGE_SAVESTATE_2;
	keys->SlotLoad.page = HOTKEY_PAGE_SAVESTATE_2;
	keys->ToggleJoypad[0].page = HOTKEY_PAGE_CONTROLS_1;
	keys->ToggleJoypad[1].page = HOTKEY_PAGE_CONTROLS_1;
	keys->ToggleJoypad[2].page = HOTKEY_PAGE_CONTROLS_1;
	keys->ToggleJoypad[3].page = HOTKEY_PAGE_CONTROLS_1;
	keys->ToggleJoypad[4].page = HOTKEY_PAGE_CONTROLS_1;
//	keys->ToggleJoypad[5].page = HOTKEY_PAGE_CONTROLS_1;
//	keys->ToggleJoypad[6].page = HOTKEY_PAGE_CONTROLS_1;
//	keys->ToggleJoypad[7].page = HOTKEY_PAGE_CONTROLS_1;
	keys->JoypadSwap.page = HOTKEY_PAGE_CONTROLS_1;
	keys->SwitchControllers.page = HOTKEY_PAGE_CONTROLS_1;
	keys->TurboA.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboB.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboY.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboX.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboL.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboR.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboStart.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboSelect.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboLeft.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboUp.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboRight.page = HOTKEY_PAGE_CONTROLS_2;
	keys->TurboDown.page = HOTKEY_PAGE_CONTROLS_2;
	keys->ScopeTurbo.page = HOTKEY_PAGE_CONTROLS_2;
	keys->ScopePause.page = HOTKEY_PAGE_CONTROLS_2;
	keys->SpeedUp.page = HOTKEY_PAGE_SPEED;
	keys->SpeedDown.page = HOTKEY_PAGE_SPEED;
	keys->SkipUp.page = HOTKEY_PAGE_SPEED;
	keys->SkipDown.page = HOTKEY_PAGE_SPEED;
	keys->FastForward.page = HOTKEY_PAGE_SPEED;
	keys->ToggleFastForward.page = HOTKEY_PAGE_SPEED;
	keys->FrameAdvance.page = HOTKEY_PAGE_SPEED;
	keys->BGL1.page = HOTKEY_PAGE_GRAPHICS;
	keys->BGL2.page = HOTKEY_PAGE_GRAPHICS;
	keys->BGL3.page = HOTKEY_PAGE_GRAPHICS;
	keys->BGL4.page = HOTKEY_PAGE_GRAPHICS;
	keys->BGL5.page = HOTKEY_PAGE_GRAPHICS;
	keys->ClippingWindows.page = HOTKEY_PAGE_GRAPHICS;
	keys->Transparency.page = HOTKEY_PAGE_GRAPHICS;
	keys->HDMA.page = HOTKEY_PAGE_GRAPHICS;
//	keys->BGLHack.page = HOTKEY_PAGE_GRAPHICS;
//	keys->InterpMode7.page = HOTKEY_PAGE_GRAPHICS;
//	keys->GLCube.page = HOTKEY_PAGE_GRAPHICS;
	keys->ToggleSound[0].page = HOTKEY_PAGE_SOUND;
	keys->ToggleSound[1].page = HOTKEY_PAGE_SOUND;
	keys->ToggleSound[2].page = HOTKEY_PAGE_SOUND;
	keys->ToggleSound[3].page = HOTKEY_PAGE_SOUND;
	keys->ToggleSound[4].page = HOTKEY_PAGE_SOUND;
	keys->ToggleSound[5].page = HOTKEY_PAGE_SOUND;
	keys->ToggleSound[6].page = HOTKEY_PAGE_SOUND;
	keys->ToggleSound[7].page = HOTKEY_PAGE_SOUND;
	keys->MoviePlay.page = HOTKEY_PAGE_MOVIE;
	keys->MovieRecord.page = HOTKEY_PAGE_MOVIE;
	keys->MovieStop.page = HOTKEY_PAGE_MOVIE;
	keys->ReadOnly.page = HOTKEY_PAGE_MOVIE;
	keys->ShowPressed.page = HOTKEY_PAGE_MOVIE;
	keys->FrameCount.page = HOTKEY_PAGE_MOVIE;
	keys->FrameCountOnly.page = HOTKEY_PAGE_MOVIE;
	keys->LagCountOnly.page = HOTKEY_PAGE_MOVIE;
	keys->ResetLagCounter.page = HOTKEY_PAGE_MOVIE;
	keys->ToggleMacro[0].page = HOTKEY_PAGE_TOOLS;
	keys->ToggleMacro[1].page = HOTKEY_PAGE_TOOLS;
	keys->ToggleMacro[2].page = HOTKEY_PAGE_TOOLS;
	keys->ToggleMacro[3].page = HOTKEY_PAGE_TOOLS;
	keys->ToggleMacro[4].page = HOTKEY_PAGE_TOOLS;
	keys->ToggleCheats.page = HOTKEY_PAGE_TOOLS;
	keys->EditMacro.page = HOTKEY_PAGE_TOOLS;
	keys->LoadLuaScript.page = HOTKEY_PAGE_TOOLS;
	keys->ReloadLuaScript.page = HOTKEY_PAGE_TOOLS;
	keys->StopLuaScript.page = HOTKEY_PAGE_TOOLS;

	// timings
	CustomKeys.OpenROM.timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.OpenMultiCart.timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.ResetGame.timing = PROCESS_AFTER_MANUAL_ADVANCE;
	CustomKeys.SaveSRAM.timing = PROCESS_AFTER_AUTO_ADVANCE;
	for (i = 0; i < 10; i++)
	{
		CustomKeys.Save[i].timing = PROCESS_AFTER_MANUAL_ADVANCE;
		CustomKeys.Load[i].timing = PROCESS_AFTER_AUTO_ADVANCE;
	}
	CustomKeys.SlotSave.timing = PROCESS_AFTER_MANUAL_ADVANCE;
	CustomKeys.SlotLoad.timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[0].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[1].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[2].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[3].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[4].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[5].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[6].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[7].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[8].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.RecentROM[9].timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.MoviePlay.timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.MovieRecord.timing = PROCESS_AFTER_AUTO_ADVANCE;
	CustomKeys.MovieStop.timing = PROCESS_AFTER_AUTO_ADVANCE;

	// default keys
	keys->RecentROM[0].key = VK_F1; keys->RecentROM[0].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[1].key = VK_F2; keys->RecentROM[1].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[2].key = VK_F3; keys->RecentROM[2].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[3].key = VK_F4; keys->RecentROM[3].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[4].key = VK_F5; keys->RecentROM[4].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[5].key = VK_F6; keys->RecentROM[5].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[6].key = VK_F7; keys->RecentROM[6].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[7].key = VK_F8; keys->RecentROM[7].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[8].key = VK_F9; keys->RecentROM[8].modifiers = CUSTKEY_CTRL_MASK;
	keys->RecentROM[9].key = VK_F10; keys->RecentROM[9].modifiers = CUSTKEY_CTRL_MASK;
	keys->OpenROM.key = 'O'; keys->OpenROM.modifiers = CUSTKEY_CTRL_MASK;
	keys->Pause.key = VK_PAUSE;
	keys->ResetGame.key = 'R'; keys->ResetGame.modifiers = CUSTKEY_CTRL_MASK;
	keys->SpeedUp.key = 0xBB;
	keys->SpeedDown.key = 0xBD;
	keys->FrameAdvance.key = 0xDC;
//	keys->SkipUp.key = 0xBB; keys->SkipUp.modifiers = CUSTKEY_SHIFT_MASK;
//	keys->SkipDown.key = 0xBD; keys->SkipDown.modifiers = CUSTKEY_SHIFT_MASK;
	keys->ScopeTurbo.key = 0xC0;
	keys->ScopePause.key = 0xBF;
	keys->FrameCount.key = 0xBE;
	keys->ReadOnly.key = '8'; keys->ReadOnly.modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[0].key = VK_F1; keys->Save[0].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[1].key = VK_F2; keys->Save[1].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[2].key = VK_F3; keys->Save[2].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[3].key = VK_F4; keys->Save[3].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[4].key = VK_F5; keys->Save[4].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[5].key = VK_F6; keys->Save[5].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[6].key = VK_F7; keys->Save[6].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[7].key = VK_F8; keys->Save[7].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[8].key = VK_F9; keys->Save[8].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Save[9].key = VK_F10; keys->Save[9].modifiers = CUSTKEY_SHIFT_MASK;
	keys->Load[0].key = VK_F1;
	keys->Load[1].key = VK_F2;
	keys->Load[2].key = VK_F3;
	keys->Load[3].key = VK_F4;
	keys->Load[4].key = VK_F5;
	keys->Load[5].key = VK_F6;
	keys->Load[6].key = VK_F7;
	keys->Load[7].key = VK_F8;
	keys->Load[8].key = VK_F9;
	keys->Load[9].key = VK_F10;
	keys->FastForward.key = VK_TAB;
	keys->ShowPressed.key = 0xBC;
	keys->SaveScreenShot.key = VK_F12;
	keys->BGL1.key = '1';
	keys->BGL2.key = '2';
	keys->BGL3.key = '3';
	keys->BGL4.key = '4';
	keys->BGL5.key = '5';
	keys->ClippingWindows.key = '8';
	keys->Transparency.key = '9';
	keys->HDMA.key = '0';
	keys->JoypadSwap.key = '6';
	keys->SwitchControllers.key = '7';
	keys->TurboA.key = VK_NEXT; keys->TurboA.modifiers = CUSTKEY_SHIFT_MASK;
	keys->TurboB.key = VK_END; keys->TurboB.modifiers = CUSTKEY_SHIFT_MASK;
	keys->TurboY.key = VK_HOME; keys->TurboY.modifiers = CUSTKEY_SHIFT_MASK;
	keys->TurboX.key = VK_PRIOR; keys->TurboX.modifiers = CUSTKEY_SHIFT_MASK;
	keys->TurboL.key = VK_INSERT; keys->TurboL.modifiers = CUSTKEY_SHIFT_MASK;
	keys->TurboR.key = VK_DELETE; keys->TurboR.modifiers = CUSTKEY_SHIFT_MASK;
}

void CopyCustomKeys (SCustomKeys *dst, const SCustomKeys *src)
{
	UINT i = 0;

	do {
		dst->key[i] = src->key[i];
	} while (!IsLastCustomKey(&src->key[i++]));
}

const int idJoypad[] = {
	ID_JOYPAD_1,
	ID_JOYPAD_2,
	ID_JOYPAD_3,
	ID_JOYPAD_4,
	ID_JOYPAD_5,
};

struct SSoundDrivers 
{
    int ident;
    int interfaceId;
} SoundDrivers[9] = {
    {ID_SOUNDINTERFACE_DIRECTSOUND, WIN_SNES9X_DIRECT_SOUND_DRIVER},
    {ID_SOUNDINTERFACE_FMOD_DIRECTSOUND, WIN_FMOD_DIRECT_SOUND_DRIVER},
    {ID_SOUNDINTERFACE_FMOD_WAVE, WIN_FMOD_WAVE_SOUND_DRIVER},
    {ID_SOUNDINTERFACE_FMOD_A3D, WIN_FMOD_A3D_SOUND_DRIVER},
    {ID_SOUNDINTERFACE_XAUDIO2, WIN_XAUDIO2_SOUND_DRIVER},
    {ID_SOUNDINTERFACE_FMODEX_DEFAULT, WIN_FMODEX_DEFAULT_DRIVER},
    {ID_SOUNDINTERFACE_FMODEX_ASIO, WIN_FMODEX_ASIO_DRIVER}
};

struct SSoundRates
{
    uint32 rate;
    int ident;
} SoundRates[9] = {
    { 8000, ID_SOUND_8000HZ},
    {11025, ID_SOUND_11025HZ},
    {16000, ID_SOUND_16000HZ},
    {22050, ID_SOUND_22050HZ},
    {30000, ID_SOUND_30000HZ},
    {32000, ID_SOUND_32000HZ},
    {35000, ID_SOUND_35000HZ},
    {44100, ID_SOUND_44100HZ},
    {48000, ID_SOUND_48000HZ}
};

//static uint32 FrameTimings[] = {
//	4000, 4000, 8333, 11667, 16667, 20000, 33333, 66667, 133333, 300000, 500000, 1000000, 1000000
//};
struct SFrameTimingRates
{
	double rate;
	int ident;
} FrameTimingRates[] = {
	{4.00, ID_FRAMESKIP_THROTTLE_400},
	{4.00, ID_FRAMESKIP_THROTTLE_400},
	{2.00, ID_FRAMESKIP_THROTTLE_200},
	{1.50, ID_FRAMESKIP_THROTTLE_150},
	{1.00, ID_FRAMESKIP_THROTTLE_100},
	{0.75, ID_FRAMESKIP_THROTTLE_75},
	{0.50, ID_FRAMESKIP_THROTTLE_50},
	{0.25, ID_FRAMESKIP_THROTTLE_25},
	{0.06, ID_FRAMESKIP_THROTTLE_6},
	{0.06, ID_FRAMESKIP_THROTTLE_6},
};

struct SFrameSkipAmounts
{
	uint32 amount;
	int ident;
} FrameSkipAmounts[] = {
	{0, ID_FRAMESKIP_0},
	{1, ID_FRAMESKIP_1},
	{2, ID_FRAMESKIP_2},
	{3, ID_FRAMESKIP_3},
	{4, ID_FRAMESKIP_4},
	{5, ID_FRAMESKIP_5},
	{6, ID_FRAMESKIP_6},
	{7, ID_FRAMESKIP_7},
	{8, ID_FRAMESKIP_8},
	{9, ID_FRAMESKIP_9},
};

struct SThreadPriorities
{
	int priority;
	int ident;
} ThreadPriorities[] = {
	{THREAD_PRIORITY_HIGHEST, ID_PRIORITY_HIGHEST},
	{THREAD_PRIORITY_ABOVE_NORMAL, ID_PRIORITY_ABOVENORMAL},
	{THREAD_PRIORITY_NORMAL, ID_PRIORITY_NORMAL},
	{THREAD_PRIORITY_BELOW_NORMAL, ID_PRIORITY_BELOWNORMAL},
};

struct SMixIntervals
{
	int ms;
	int ident;
} MixIntervals[] = {
	{10, ID_SOUND_MIXINTERVAL_10MS},
	{20, ID_SOUND_MIXINTERVAL_20MS},
	{30, ID_SOUND_MIXINTERVAL_30MS},
	{40, ID_SOUND_MIXINTERVAL_40MS},
	{50, ID_SOUND_MIXINTERVAL_50MS},
	{60, ID_SOUND_MIXINTERVAL_60MS},
	{70, ID_SOUND_MIXINTERVAL_70MS},
	{80, ID_SOUND_MIXINTERVAL_80MS},
	{90, ID_SOUND_MIXINTERVAL_90MS},
	{100, ID_SOUND_MIXINTERVAL_100MS},
	{110, ID_SOUND_MIXINTERVAL_110MS},
	{120, ID_SOUND_MIXINTERVAL_120MS},
	{130, ID_SOUND_MIXINTERVAL_130MS},
	{140, ID_SOUND_MIXINTERVAL_140MS},
	{150, ID_SOUND_MIXINTERVAL_150MS},
};

// Languages supported by Snes9X: Windows
// 0 - English [Default]
// 1 - Dutch/Nederlands
struct sLanguages Languages[] = {
	{ IDR_MENU_US,
		TEXT("DirectX failed to initialize!"),
		TEXT("DirectDraw failed to set the selected display mode!"),
		TEXT("DirectSound failed to initialize; no sound will be played."),
		TEXT("These settings won't take effect until you restart the emulator."),
		TEXT("The frame timer failed to initialize, please do NOT select the automatic framerate option or Snes9X will crash!")},
	{ IDR_MENU_NL,
	TEXT("Er is een fout opgetreden tijdens het initalizeren van DirectX!"),
	TEXT("Er is een fout opgetreden tijdens het verander van scherm modus!"),
	TEXT("Er is een fout opgetreden tijdens het initializeren van DirectSound, er zal geen geluid worden afgespeeld."),
	TEXT("Deze opties worden pas toegepast als de emulator opnieuw is opgestart."),
	TEXT("Er is een fout opgetreden tijdens het initializeren van de frame timer, kies NIET de automatische framerate optie want dan zal Snes9X crashen!")}
};

struct OpenMovieParams
{
	char Path[_MAX_PATH];
	bool8 ReadOnly;
	bool8 DisplayInput;
	uint8 ControllersMask;
	uint8 Opts;
	uint8 SyncFlags;
	wchar_t Metadata[MOVIE_MAX_METADATA];
};



BOOL ClientToSNESScreen(PPOINT ppt, bool clip) {
	POINT cur = { ppt->x, ppt->y };
	POINT snespt;

	int screenWidth = IPPU.RenderedScreenWidth;
	int screenHeight = GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT;
	if (IPPU.RenderedScreenWidth > SNES_WIDTH) screenHeight *= 2;
	RECT clientRect;
	GetClientRect(GUI.hWnd, &clientRect);
	int clientWidth = clientRect.right - clientRect.left;
	int clientHeight = clientRect.bottom - clientRect.top;

	if (!GUI.Stretch && !VOODOO_MODE && !OPENGL_MODE) {
		POINT start;
		RECT filterRect;
		int filterWidth, filterHeight;

		GetFilterRect(GUI.Scale, &filterRect);
		filterWidth = filterRect.right - filterRect.left;
		filterHeight = filterRect.bottom - filterRect.top;

		start.x = (clientWidth - filterWidth) / 2;
		start.y = (clientHeight - filterHeight) / 2;
		snespt.x = (LONG)((cur.x - start.x) * ((float) screenWidth / filterWidth));
		snespt.y = (LONG)((cur.y - start.y) * ((float) screenHeight / filterHeight));
	}
	else {
		if (GUI.AspectRatio) {
			float snesAspect = (float) GUI.AspectWidth / (GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT);
			float renderedWidth = (float) clientWidth, renderedHeight = renderedWidth / snesAspect;
			int xOffset = 0, yOffset = 0;
			if (renderedHeight <= clientHeight) {
				yOffset = (clientHeight - (int) renderedHeight) / 2;
			}
			else {
				renderedHeight = (float) clientHeight;
				renderedWidth = renderedHeight * snesAspect;
				xOffset = (clientWidth - (int) renderedWidth) / 2;
			}
			snespt.x = (LONG)((cur.x - xOffset) * ((float) screenWidth / (int) renderedWidth));
			snespt.y = (LONG)((cur.y - yOffset) * ((float) screenHeight / (int) renderedHeight));
		}
		else {
			snespt.x = (LONG)(cur.x * ((float) screenWidth / clientWidth));
			snespt.y = (LONG)(cur.y * ((float) screenHeight / clientHeight));
		}
	}

	if (IPPU.RenderedScreenWidth > SNES_WIDTH)
		snespt.y /= 2;
	if (clip) {
		if (snespt.x < 0)
			snespt.x = 0;
		if (snespt.y < 0)
			snespt.y = 0;
		if (snespt.x >= IPPU.RenderedScreenWidth)
			snespt.x = IPPU.RenderedScreenWidth - 1;
		if (snespt.y >= IPPU.RenderedScreenHeight)
			snespt.y = IPPU.RenderedScreenHeight - 1;
	}

	if (ppt) {
		ppt->x = snespt.x;
		ppt->y = snespt.y;
		return TRUE;
	}
	else
		return FALSE;
}

BOOL GetCursorPosSNES(LPPOINT lpPoint, bool clip) {
	POINT cur;

	GetCursorPos(&cur);
	ScreenToClient(GUI.hWnd, &cur);
	ClientToSNESScreen(&cur, clip);
	if (lpPoint) {
		lpPoint->x = cur.x;
		lpPoint->y = cur.y;
		return TRUE;
	}
	else
		return FALSE;
}


std::vector<dMode> dm;
/*****************************************************************************/
/* WinProc                                                                   */
/*****************************************************************************/
void DoWAVOpen(const char* filename);
void DoWAVClose(int reason);
void DoAVIOpen(const char* filename);
void DoAVIClose(int reason);
bool ReInitSound(int mode);
bool SetupSound(void);
void RestoreGUIDisplay ();
void RestoreSNESDisplay ();
void FreezeUnfreeze (int slot, bool8 freeze);
void CheckDirectoryIsWritable (const char *filename);
static void CheckMenuStates ();
static void ResetFrameTimer ();
void Update_Old_RAM_Watch ();
bool8 LoadROM (const char *filename);
bool8 LoadMultiROM (const char *filename, const char *filename2);
#ifdef NETPLAY_SUPPORT
static void EnableServer (bool8 enable);
#endif
void WinDeleteRecentGamesList ();
const char* WinParseCommandLineAndLoadConfigFile (char *line);
void WinRegisterConfigItems ();
void WinSaveConfigFile ();
void WinSetDefaultValues ();
void WinLockConfigFile ();
void WinUnlockConfigFile ();
void WinCleanupConfigData ();

#include "../ppu.h"
#include "../snapshot.h"
extern "C" const char *S9xGetFilename (const char *, enum s9x_getdirtype dirtype);
const char *S9xGetFilenameInc (const char *);
HMENU GetRecentSubMenu ();
void S9xSetRecentGames ();
void S9xAddToRecentGames (const char *filename);
void S9xRemoveFromRecentGames (int i);
HMENU GetFilterSubMenu ();
void S9xSetFilters ();

#define ID_FILTER_MIN   0xfe00
#define ID_FILTER_MAX   0xfeff
#define ID_RECENT_MIN   0xff00
#define ID_RECENT_MAX   0xffff

extern void S9xReRefresh();

bool8 MustDeferMessage(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
int   MustDeferKeyMessage(WPARAM wParam, LPARAM lParam, int modifiers);
void DispatchMessagesInQueue();
void WarnIfDeferredMessagesExist();
int GetModifiers(int key);

static void absToRel(char* relPath, const char* absPath, const char* baseDir)
{
	strcpy(relPath, absPath);
	if(!strncasecmp(absPath, baseDir, strlen(baseDir)))
	{
		char temp [MAX_PATH];
		temp[MAX_PATH-3]='\0';
		const char* relative = absPath+strlen(baseDir);
		while(relative[0]=='\\' || relative[0]=='/')
			relative++;
		relPath[0]='.'; relPath[1]='\\';
		strcpy(relPath+2, relative);
	}
}

BOOL SendMenuCommand (UINT uID)
{
	MENUITEMINFO mii;

	CheckMenuStates();

	mii.cbSize = sizeof(mii);
	mii.fMask  = MIIM_STATE;
	if (!GetMenuItemInfo(GUI.hMenu, uID, FALSE, &mii))
		return FALSE;
	if (!(mii.fState & MFS_DISABLED))
		return SendMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(uID),(LPARAM)(NULL));
	else
		return FALSE;
}

BOOL PostMenuCommand (UINT uID)
{
	MENUITEMINFO mii;

	CheckMenuStates();

	mii.cbSize = sizeof(mii);
	mii.fMask  = MIIM_STATE;
	if (!GetMenuItemInfo(GUI.hMenu, uID, FALSE, &mii))
		return FALSE;
	if (!(mii.fState & MFS_DISABLED))
		return PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(uID),(LPARAM)(NULL));
	else
		return FALSE;
}

void S9xMouseOn ()
{
	if(Settings.StopEmulation)
		return;

    if (GUI.ControllerOption==SNES_MOUSE || GUI.ControllerOption==SNES_MOUSE_SWAPPED)
    {
		if(Settings.Paused)
			SetCursor (GUI.Arrow);
		else
	        SetCursor (NULL);
    }
    else if (GUI.ControllerOption!=SNES_SUPERSCOPE && GUI.ControllerOption!=SNES_JUSTIFIER && GUI.ControllerOption!=SNES_JUSTIFIER_2)
    {
        SetCursor (GUI.Arrow);
        GUI.CursorTimer = 60;
    }
    else
	{
		if(Settings.Paused)
			SetCursor (GUI.GunSight);
		else
	        SetCursor (NULL);
	}
}

void ChangeInputDevice(void)
{
	Settings.MouseMaster = false;
	Settings.JustifierMaster = false;
	Settings.SuperScopeMaster = false;
	Settings.MultiPlayer5Master = false;

	CheckMenuItem(GUI.hMenu, IDM_ENABLE_MULTITAP, MFS_UNCHECKED);
	CheckMenuItem(GUI.hMenu, IDM_JUSTIFIER, MFS_UNCHECKED);
	CheckMenuItem(GUI.hMenu, IDM_MOUSE_TOGGLE, MFS_UNCHECKED);
	CheckMenuItem(GUI.hMenu, IDM_SCOPE_TOGGLE, MFS_UNCHECKED);
	CheckMenuItem(GUI.hMenu, IDM_MOUSE_SWAPPED, MFS_UNCHECKED);
	CheckMenuItem(GUI.hMenu, IDM_JUSTIFIERS, MFS_UNCHECKED);
	CheckMenuItem(GUI.hMenu, IDM_MULTITAP8, MFS_UNCHECKED);
	CheckMenuItem(GUI.hMenu, IDM_SNES_JOYPAD, MFS_UNCHECKED);

	switch(GUI.ControllerOption)
	{
	case SNES_MOUSE:
		Settings.MouseMaster = true;
		S9xSetController(0, CTL_MOUSE,      0, 0, 0, 0);
		S9xSetController(1, CTL_JOYPAD,     1, 0, 0, 0);
		CheckMenuItem(GUI.hMenu, IDM_MOUSE_TOGGLE, MFS_CHECKED);
		break;
	case SNES_MOUSE_SWAPPED:
		Settings.MouseMaster = true;
		S9xSetController(0, CTL_JOYPAD,     0, 0, 0, 0);
		S9xSetController(1, CTL_MOUSE,      1, 0, 0, 0);
		CheckMenuItem(GUI.hMenu, IDM_MOUSE_SWAPPED, MFS_CHECKED);
		break;
	case SNES_SUPERSCOPE:
		Settings.SuperScopeMaster = true;
		S9xSetController(0, CTL_JOYPAD,     0, 0, 0, 0);
		S9xSetController(1, CTL_SUPERSCOPE, 0, 0, 0, 0);
		CheckMenuItem(GUI.hMenu, IDM_SCOPE_TOGGLE, MFS_CHECKED);
		break;
	case SNES_MULTIPLAYER5:
		Settings.MultiPlayer5Master = true;
		S9xSetController(0, CTL_JOYPAD,     0, 0, 0, 0);
		S9xSetController(1, CTL_MP5,        1, 2, 3, 4);
		CheckMenuItem(GUI.hMenu, IDM_ENABLE_MULTITAP, MFS_CHECKED);
		break;
	case SNES_MULTIPLAYER8:
		Settings.MultiPlayer5Master = true;
		S9xSetController(0, CTL_MP5,        0, 1, 2, 3);
		S9xSetController(1, CTL_MP5,        4, 5, 6, 7);
		CheckMenuItem(GUI.hMenu, IDM_ENABLE_MULTITAP, MFS_CHECKED);
		break;
	case SNES_JUSTIFIER:
		Settings.JustifierMaster = true;
		S9xSetController(0, CTL_JOYPAD,     0, 0, 0, 0);
		S9xSetController(1, CTL_JUSTIFIER,  0, 0, 0, 0);
		CheckMenuItem(GUI.hMenu, IDM_JUSTIFIER, MFS_CHECKED);
		break;
	case SNES_JUSTIFIER_2:
		Settings.JustifierMaster = true;
		S9xSetController(0, CTL_JOYPAD,     0, 0, 0, 0);
		S9xSetController(1, CTL_JUSTIFIER,  1, 0, 0, 0);
		CheckMenuItem(GUI.hMenu, IDM_JUSTIFIERS, MFS_CHECKED);
		break;
	default:
	case SNES_JOYPAD:
		S9xSetController(0, CTL_JOYPAD,     0, 0, 0, 0);
		S9xSetController(1, CTL_JOYPAD,     1, 0, 0, 0);
		CheckMenuItem(GUI.hMenu, IDM_SNES_JOYPAD, MFS_CHECKED);
		break;
	}

    GUI.ControlForced = 0xff;
}

static void CenterCursor()
{
	if(GUI.ControllerOption==SNES_MOUSE || GUI.ControllerOption==SNES_MOUSE_SWAPPED)
	{
		if(GUI.hWnd == GetActiveWindow() && !S9xMoviePlaying())
		{
			POINT cur, middle;
			RECT size;

			GetClientRect (GUI.hWnd, &size);
			middle.x = (size.right - size.left) >> 1;
			middle.y = (size.bottom - size.top) >> 1;
			ClientToScreen (GUI.hWnd, &middle);
			GetCursorPos (&cur);
			int dX = middle.x-cur.x;
			int dY = middle.y-cur.y;
			if(dX || dY)
			{
				GUI.MouseX -= dX;
				GUI.MouseY -= dY;
				SetCursorPos (middle.x, middle.y);
//				GUI.IgnoreNextMouseMove = true;
			}
		}
	}
}


void S9xRestoreWindowTitle ()
{
    TCHAR buf [100];
    sprintf (buf, TEXT(WINDOW_TITLE), SNES9X_NAME_AND_VERSION);
    SetWindowText (GUI.hWnd, buf);
}

void SwitchToGDI()
{
	if (GUI.outputMethod==DIRECTDRAW && !VOODOO_MODE && !OPENGL_MODE)
    {
        IPPU.ColorsChanged = true;
        DirectDraw.lpDD->FlipToGDISurface();
        GUI.FlipCounter = 0;
        DirectDraw.lpDDSPrimary2->SetPalette (NULL);
    }
}

static void S9xClearSurface (LPDIRECTDRAWSURFACE2 lpDDSurface)
{
    DDBLTFX fx;

    memset (&fx, 0, sizeof (fx));
    fx.dwSize = sizeof (fx);

    while (lpDDSurface->Blt (NULL, DirectDraw.lpDDSPrimary2, NULL, DDBLT_WAIT, NULL) == DDERR_SURFACELOST)
        lpDDSurface->Restore ();
}

void UpdateBackBuffer()
{
    GUI.ScreenCleared = true;

    if (GUI.outputMethod==DIRECTDRAW && !VOODOO_MODE && !OPENGL_MODE && GUI.FullScreen)
    {
        SwitchToGDI();

        DDBLTFX fx;

        memset (&fx, 0, sizeof (fx));
        fx.dwSize = sizeof (fx);

        while (DirectDraw.lpDDSPrimary2->Blt (NULL, NULL, NULL, DDBLT_WAIT | DDBLT_COLORFILL, &fx) == DDERR_SURFACELOST)
            DirectDraw.lpDDSPrimary2->Restore ();

        if (GetMenu (GUI.hWnd) != NULL)
            DrawMenuBar (GUI.hWnd);

        GUI.FlipCounter = 0;
        DDSCAPS caps;
        caps.dwCaps = DDSCAPS_BACKBUFFER;

        LPDIRECTDRAWSURFACE2 pDDSurface;

        if (DirectDraw.lpDDSPrimary2->GetAttachedSurface (&caps, &pDDSurface) == DD_OK &&
            pDDSurface != NULL)
        {
            S9xClearSurface (pDDSurface);
            DirectDraw.lpDDSPrimary2->Flip (NULL, DDFLIP_WAIT);
            while (DirectDraw.lpDDSPrimary2->GetFlipStatus (DDGFS_ISFLIPDONE) != DD_OK)
                Sleep (0);
			if(DirectDraw.DoubleBuffered)
	            S9xClearSurface (pDDSurface);
        }
    }
    else
    {
        if (GetMenu( GUI.hWnd) != NULL)
            DrawMenuBar (GUI.hWnd);
    }
}

void ToggleMenuBar ()
{
	// TODO: keeping ClientRect after the change would be better
	if (GetMenu (GUI.hWnd) == NULL) {
		SetMenu (GUI.hWnd, GUI.hMenu);
		GUI.HideMenu = false;
	}
	else {
		SetMenu (GUI.hWnd, NULL);
		GUI.HideMenu = true;
	}
}

void ToggleFullScreen ()
{
	bool maximized = GUI.window_maximized;
	int wasFullScreen = GUI.FullScreen;

    S9xSetPause (PAUSE_TOGGLE_FULL_SCREEN);

#ifdef USE_GLIDE
    if (VOODOO_MODE)
    {
		//        S9xGlideEnable (FALSE);
		//        GUI.Scale = 0;
		//        MoveWindow (GUI.hWnd, GUI.window_size.left,
		//                    GUI.window_size.top,
		//                    GUI.window_size.right - GUI.window_size.left,
		//                    GUI.window_size.bottom - GUI.window_size.top, TRUE);
    }
    else
#endif
	if(GUI.EmulateFullscreen) {
		HMONITOR hm;
		MONITORINFO mi;
		GUI.EmulatedFullScreen = !GUI.EmulatedFullScreen;
		if(GUI.EmulatedFullScreen) {
			GetWindowRect (GUI.hWnd, &GUI.window_size);
			if(GetMenu(GUI.hWnd)!=NULL)
				SetMenu(GUI.hWnd,NULL);
			SetWindowLong (GUI.hWnd, GWL_STYLE, WS_POPUP|WS_VISIBLE);
			hm = MonitorFromWindow(GUI.hWnd,MONITOR_DEFAULTTONEAREST);
			mi.cbSize = sizeof(mi);
			GetMonitorInfo(hm,&mi);
			SetWindowPos (GUI.hWnd, HWND_TOP, mi.rcMonitor.left, mi.rcMonitor.top, mi.rcMonitor.right - mi.rcMonitor.left, mi.rcMonitor.bottom - mi.rcMonitor.top, SWP_DRAWFRAME|SWP_FRAMECHANGED);
		} else {
			bool maximized = GUI.window_maximized;
			SetWindowLong( GUI.hWnd, GWL_STYLE, WS_POPUPWINDOW|WS_CAPTION|
                   WS_THICKFRAME|WS_VISIBLE|WS_MINIMIZEBOX|WS_MAXIMIZEBOX);
			SetMenu(GUI.hWnd,GUI.hMenu);
			SetWindowPos (GUI.hWnd, HWND_NOTOPMOST, GUI.window_size.left, GUI.window_size.top, GUI.window_size.right - GUI.window_size.left, GUI.window_size.bottom - GUI.window_size.top, SWP_DRAWFRAME|SWP_FRAMECHANGED);
			if(maximized)
				ShowWindow(GUI.hWnd, SW_MAXIMIZE);
		}
	} else {
#ifdef USE_OPENGL
		if (OPENGL_MODE)
		{
			if (!GUI.FullScreen)
			{
				DEVMODE dmScreenSettings;

				memset (&dmScreenSettings, 0, sizeof(dmScreenSettings));
				dmScreenSettings.dmSize = sizeof(dmScreenSettings);
				dmScreenSettings.dmPelsWidth = GUI.Width;
				dmScreenSettings.dmPelsHeight = GUI.Height;
				dmScreenSettings.dmBitsPerPel = 16; //bits;
				dmScreenSettings.dmFields = DM_BITSPERPEL | DM_PELSWIDTH |
					DM_PELSHEIGHT;
				if (ChangeDisplaySettings (&dmScreenSettings, CDS_FULLSCREEN) == DISP_CHANGE_SUCCESSFUL)
				{
					GUI.FullScreen = TRUE;
					SetWindowLong (GUI.hWnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
					SetWindowPos (GUI.hWnd, HWND_TOP, 0, 0, GUI.Width,
						GUI.Height,
						SWP_DRAWFRAME|SWP_FRAMECHANGED);
				}
			}
			else
			{
				SetWindowLong (GUI.hWnd, GWL_STYLE, WS_POPUPWINDOW|WS_CAPTION|
					WS_VISIBLE|WS_MINIMIZEBOX|WS_MAXIMIZEBOX|(GUI.windowResizeLocked?0:WS_THICKFRAME));
				SetWindowPos (GUI.hWnd, HWND_TOP,
					GUI.window_size.left,
					GUI.window_size.top,
					GUI.window_size.right - GUI.window_size.left,
					GUI.window_size.bottom - GUI.window_size.top,
					SWP_DRAWFRAME|SWP_FRAMECHANGED);
				ChangeDisplaySettings (NULL, 0);
				if(maximized)
					ShowWindow(GUI.hWnd, SW_MAXIMIZE);
				GUI.FullScreen = FALSE;
			}
		}
		else
#endif
			//if (!VOODOO_MODE && !GUI.FullScreen)
			//	GetWindowRect (GUI.hWnd, &GUI.window_size);

		if(GUI.outputMethod==DIRECT3D) {
			GUI.FullScreen = !GUI.FullScreen;
			if(GUI.FullScreen) {
				if (!maximized)
					GetWindowRect (GUI.hWnd, &GUI.window_size);
				if(!Direct3D.setFullscreen(true))
					GUI.FullScreen = false;
				if(GetMenu(GUI.hWnd)!=NULL)
					SetMenu(GUI.hWnd,NULL);
				SetWindowLong (GUI.hWnd, GWL_STYLE, WS_POPUP|WS_VISIBLE);
				SetWindowPos (GUI.hWnd, HWND_TOP, 0, 0, 0, 0, SWP_DRAWFRAME|SWP_FRAMECHANGED|SWP_NOMOVE|SWP_NOSIZE);
			}
			if(!GUI.FullScreen) {
				GUI.FullScreen = wasFullScreen!=0;
				Direct3D.setFullscreen(false);
				SetWindowLong( GUI.hWnd, GWL_STYLE, WS_POPUPWINDOW|WS_CAPTION|
					WS_VISIBLE|WS_MINIMIZEBOX|WS_MAXIMIZEBOX|(GUI.windowResizeLocked?0:WS_THICKFRAME));
				if (!GUI.HideMenu)
					SetMenu(GUI.hWnd,GUI.hMenu);
				SetWindowPos (GUI.hWnd, HWND_NOTOPMOST, GUI.window_size.left, GUI.window_size.top, GUI.window_size.right - GUI.window_size.left, GUI.window_size.bottom - GUI.window_size.top, SWP_DRAWFRAME|SWP_FRAMECHANGED);
				GUI.FullScreen = false;
			}
		} else {
			if (!GUI.FullScreen) {
				if (!maximized)
					GetWindowRect (GUI.hWnd, &GUI.window_size);
			}
			GUI.FullScreen = !GUI.FullScreen;
			if (!DirectDraw.SetDisplayMode (GUI.Width, GUI.Height, max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes)), GUI.Depth, GUI.RefreshRate, !GUI.FullScreen, GUI.tripleBuffering))
			{
				MessageBox( GUI.hWnd, Languages[ GUI.Language].errModeDD, TEXT("Snes9X - DirectDraw(2)"), MB_OK | MB_ICONSTOP);
				S9xClearPause (PAUSE_TOGGLE_FULL_SCREEN);
				GUI.FullScreen = !GUI.FullScreen;
				return;
			}

			if (GUI.FullScreen)
			{
				if(GetMenu(GUI.hWnd)!=NULL)
					SetMenu(GUI.hWnd,NULL);
			}
			if (!GUI.FullScreen)
			{
				SwitchToGDI();
				if (!GUI.HideMenu)
					SetMenu(GUI.hWnd,GUI.hMenu);
				MoveWindow (GUI.hWnd, GUI.window_size.left,
					GUI.window_size.top,
					GUI.window_size.right - GUI.window_size.left,
					GUI.window_size.bottom - GUI.window_size.top, TRUE);
			}
		}
	}

	if (!GUI.FullScreen) {
		if(maximized)
			ShowWindow(GUI.hWnd, SW_MAXIMIZE);
	}

	S9xGraphicsDeinit();
	S9xSetWinPixelFormat ();
	S9xInitUpdate();
	S9xGraphicsInit();

	IPPU.RenderThisFrame = true;
	UpdateBackBuffer();

	S9xClearPause (PAUSE_TOGGLE_FULL_SCREEN);
}

void S9xDisplayStateChange (const char *str, bool8 on)
{
    static char string [100];

    sprintf (string, "%s %s", str, on ? "on" : "off");
    S9xSetInfoString (string);
}

void S9xDisplaySpeedChange ()
{
	static char string [100];

	sprintf (string, "Speed: %.0f%% (%.1f ms/frame)", ((Settings.PAL?Settings.FrameTimePAL:Settings.FrameTimeNTSC) * 100.0f) / (float)Settings.FrameTime, Settings.FrameTime*0.001f);
	S9xSetInfoString (string);
}

void S9xSetRunSpeed(double rate)
{
	uint32 normalFrameTime = (Settings.PAL?Settings.FrameTimePAL:Settings.FrameTimeNTSC);

	Settings.FrameTime = (uint32)(normalFrameTime/rate+0.5);

	ResetFrameTimer ();
	S9xDisplaySpeedChange();
}

static void UpdateScale(RenderFilter & Scale, RenderFilter & NextScale)
{
	if (IS_GL_OR_GLIDE(Scale) == IS_GL_OR_GLIDE(NextScale))
		Scale = NextScale;
	else
	{
		if (IS_GLIDE_MODE(NextScale) || (IS_GL_MODE(NextScale)
#ifdef USE_OPENGL
			&& !S9xOpenGLInit ()
#endif
			) || (!IS_GL_OR_GLIDE(NextScale) &&
			(GUI.outputMethod==DIRECT3D)?!Direct3D.initialize(GUI.hWnd):!DirectDraw.InitDirectDraw()
			))
		{
			Scale = FILTER_NONE;
			NextScale = FILTER_NONE;
		    MessageBox (GUI.hWnd, TEXT(WINPROC_FILTER_RESTART), TEXT(SNES9X_INFO), MB_OK | MB_ICONINFORMATION);
		}
		else
		{
			S9xGraphicsDeinit();
			Scale = NextScale;
			Settings.OpenGLEnable = IS_GL_MODE(Scale);
			Settings.GlideEnable = IS_GLIDE_MODE(Scale);
			S9xSetWinPixelFormat ();
			S9xInitUpdate();
			S9xGraphicsInit();
		}
	}
}

void S9xResizeWindow(int width, int height, bool changeShowState)
{
	bool maximized = GUI.window_maximized;
	int widthDiff, heightDiff;
	RECT rcWnd, rcCli;

	if (GUI.FullScreen)
		return;

	if (maximized) {
		ShowWindow(GUI.hWnd, SW_RESTORE);
		GUI.window_maximized = false;
	}

	GetWindowRect (GUI.hWnd, &GUI.window_size);

	// FIXME: resize twice for multi-line menubar,
	// though it may frarely fails on resizing to the exact size.
	for (int i = 0; i < 2; i++) {
		GetWindowRect(GUI.hWnd, &rcWnd);
		GetClientRect(GUI.hWnd, &rcCli);
		widthDiff = (rcWnd.right - rcWnd.left) - (rcCli.right - rcCli.left);
		heightDiff = (rcWnd.bottom - rcWnd.top) - (rcCli.bottom - rcCli.top);

		GUI.window_size.right = GUI.window_size.left + width + widthDiff;
		GUI.window_size.bottom = GUI.window_size.top + height + heightDiff;
		if (!GUI.FullScreen)
			SetWindowPos (GUI.hWnd, HWND_NOTOPMOST, GUI.window_size.left, GUI.window_size.top, GUI.window_size.right - GUI.window_size.left, GUI.window_size.bottom - GUI.window_size.top, SWP_DRAWFRAME|SWP_FRAMECHANGED);
	}

	if (!changeShowState && maximized) {
		ShowWindow(GUI.hWnd, SW_MAXIMIZE);
		GUI.window_maximized = true;
	}
}

static char InfoString [100];
static uint32 prevPadReadFrame = (uint32)-1;
static bool skipNextFrameStop = false;

int HandleKeyMessage(WPARAM wParam, LPARAM lParam, int modifiers)
{
	// update toggles
	for (int J = 0; J < 5; J++)
	{
		extern bool S9xGetState (WORD KeyIdent);
		if(Joypad[J].Enabled && (!S9xGetState(Joypad[J+8].Left))) // enabled and Togglify
		{
			SJoypad & p = ToggleJoypadStorage[J];
			if(wParam == Joypad[J].L) p.L = !p.L;
			if(wParam == Joypad[J].R) p.R = !p.R;
			if(wParam == Joypad[J].A) p.A = !p.A;
			if(wParam == Joypad[J].B) p.B = !p.B;
			if(wParam == Joypad[J].Y) p.Y = !p.Y;
			if(wParam == Joypad[J].X) p.X = !p.X;
			if(wParam == Joypad[J].Start) p.Start = !p.Start;
			if(wParam == Joypad[J].Select) p.Select = !p.Select;
			if(wParam == Joypad[J].Left) p.Left = !p.Left;
			if(wParam == Joypad[J].Right) p.Right = !p.Right;
			if(wParam == Joypad[J].Up) p.Up = !p.Up;
			if(wParam == Joypad[J].Down) p.Down = !p.Down;
///					if(wParam == Joypad[J].Left_Down) p.Left_Down = !p.Left_Down;
///					if(wParam == Joypad[J].Left_Up) p.Left_Up = !p.Left_Up;
///					if(wParam == Joypad[J].Right_Down) p.Right_Down = !p.Right_Down;
///					if(wParam == Joypad[J].Right_Up) p.Right_Up = !p.Right_Up;
			if(!Settings.UpAndDown)
			{
				if(p.Left && p.Right)
					p.Left = p.Right = false;
				if(p.Up && p.Down)
					p.Up = p.Down = false;
			}
		}
		if(Joypad[J].Enabled && (!S9xGetState(Joypad[J+8].Down))) // enabled and turbo-togglify (TurboTog)
		{
			SJoypad & p = TurboToggleJoypadStorage[J];
			if(wParam == Joypad[J].L) p.L = !p.L;
			if(wParam == Joypad[J].R) p.R = !p.R;
			if(wParam == Joypad[J].A) p.A = !p.A;
			if(wParam == Joypad[J].B) p.B = !p.B;
			if(wParam == Joypad[J].Y) p.Y = !p.Y;
			if(wParam == Joypad[J].X) p.X = !p.X;
			if(wParam == Joypad[J].Start) p.Start = !p.Start;
			if(wParam == Joypad[J].Select) p.Select = !p.Select;
			if(wParam == Joypad[J].Left) p.Left = !p.Left;
			if(wParam == Joypad[J].Right) p.Right = !p.Right;
			if(wParam == Joypad[J].Up) p.Up = !p.Up;
			if(wParam == Joypad[J].Down) p.Down = !p.Down;
///					if(wParam == Joypad[J].Left_Down) p.Left_Down = !p.Left_Down;
///					if(wParam == Joypad[J].Left_Up) p.Left_Up = !p.Left_Up;
///					if(wParam == Joypad[J].Right_Down) p.Right_Down = !p.Right_Down;
///					if(wParam == Joypad[J].Right_Up) p.Right_Up = !p.Right_Up;
/*					if(!Settings.UpAndDown)
			{
				if(p.Left && p.Right && )
					p.Left = p.Right = false;
				if(p.Up && p.Down)
					p.Up = p.Down = false;
			}*/
		}
		if(wParam == Joypad[J+8].Right) // clear all
		{
			{
				SJoypad & p = ToggleJoypadStorage[J];
				p.L = false;
				p.R = false;
				p.A = false;
				p.B = false;
				p.Y = false;
				p.X = false;
				p.Start = false;
				p.Select = false;
				p.Left = false;
				p.Right = false;
				p.Up = false;
				p.Down = false;
			}
			{
				SJoypad & p = TurboToggleJoypadStorage[J];
				p.L = false;
				p.R = false;
				p.A = false;
				p.B = false;
				p.Y = false;
				p.X = false;
				p.Start = false;
				p.Select = false;
				p.Left = false;
				p.Right = false;
				p.Up = false;
				p.Down = false;
			}
			//MacroDisableAll();
			MacroChangeState(J, false);
		}
	}


	bool hitHotKey = false;

	if(!(wParam == 0 || wParam == VK_ESCAPE)) // if it's the 'disabled' key, it's never pressed as a hotkey
	{
		SCustomKey *key = CustomKeys.key;
		while (!IsLastCustomKey(key)) {
			if (wParam == key->key && modifiers == key->modifiers && key->handleKeyDown) {
				key->handleKeyDown(lParam & 0x40000000 ? false : true);
				hitHotKey = true;
			}
			key++;
		}

		// don't pull down menu if alt is a hotkey or the menu isn't there, unless no game is running
		if(!Settings.StopEmulation && ((wParam == VK_MENU || wParam == VK_F10) && (hitHotKey || GetMenu (GUI.hWnd) == NULL) && !GetAsyncKeyState(VK_F4)))
			return 0;
	}

	if(!hitHotKey)
	switch (wParam)
	{
		case VK_ESCAPE:
			if(GUI.FullScreen && !GUI.EmulateFullscreen)
			{
				ToggleFullScreen();
			}
			else
			{
				ToggleMenuBar();
			}

			UpdateBackBuffer();
			break;
	}
	return 1;
}

static bool DoOpenRomDialog(char filename [_MAX_PATH], bool noCustomDlg = false)
{
	if(GUI.CustomRomOpen && !noCustomDlg)
	{
		try
		{
			INITCOMMONCONTROLSEX icex;
			icex.dwSize = sizeof(INITCOMMONCONTROLSEX);
			icex.dwICC   = ICC_LISTVIEW_CLASSES|ICC_TREEVIEW_CLASSES;
			InitCommonControlsEx(&icex); // this could cause failure if the common control DLL isn't found

			return (1 <= DialogBoxParam(g_hInst, MAKEINTRESOURCE(IDD_OPEN_ROM), GUI.hWnd, DlgOpenROMProc, (LPARAM)filename));
		}
		catch(...) {} // use standard dialog if the special one fails

		GUI.CustomRomOpen = false; // if crashed, turn off custom for next time
	}

	// standard file dialog
	{
		OPENFILENAME ofn;
		static char szFileName[MAX_PATH] = {0};
		char szPathName[MAX_PATH];
		_fullpath(szPathName, S9xGetDirectory(ROM_DIR), MAX_PATH);

		// a limited strcat that doesn't mind null characters
#define strcat0(to,from) do{memcpy(to,from,sizeof(from)-1);to+=sizeof(from)-1;}while(false)

		// make filter string using entries in valid_ext
		char lpfilter [8192] = {0};
		char* lpfilterptr = (char*)lpfilter;
		for(int i=0; i<2; i++)
		{
			if(!i)
				strcat0(lpfilterptr, FILE_INFO_ROM_FILE_TYPE);
			else
				strcat0(lpfilterptr, FILE_INFO_UNCROM_FILE_TYPE);
			strcat0(lpfilterptr, "\0");
			if(valid_ext) // add valid extensions to string
			{
				ExtList* ext = valid_ext;
				int extlen_approx = 0;
				bool first = true;
				while(ext && (extlen_approx < 2048))
				{
					if((!i || !ext->compressed) && ext->extension && strlen(ext->extension) < 256)
					{
						if(!first)
							strcat(lpfilterptr, ";*.");
						else
						{
							strcat(lpfilterptr, "*.");
							first = false;
						}
						strcat(lpfilterptr, ext->extension);
						extlen_approx += strlen(ext->extension) + 3;
					}
					ext = ext->next;
				}
				lpfilterptr += strlen(lpfilterptr);
			}
			else
				strcat0(lpfilterptr, "*.smc");
			strcat0(lpfilterptr, "\0");
		}
		strcat0(lpfilterptr, FILE_INFO_ANY_FILE_TYPE);
		strcat0(lpfilterptr, "\0*.*\0\0");

		ZeroMemory((LPVOID)&ofn, sizeof(OPENFILENAME));
		ofn.lStructSize = sizeof(OPENFILENAME);
		ofn.hwndOwner = GUI.hWnd;
		ofn.lpstrFilter = lpfilter;
		ofn.lpstrFile = szFileName;
		ofn.lpstrDefExt = "smc";
		ofn.nMaxFile = MAX_PATH;
		ofn.Flags = OFN_HIDEREADONLY | OFN_FILEMUSTEXIST;
		ofn.lpstrInitialDir = szPathName;
		if(GetOpenFileName(&ofn))
		{
			strncpy(filename, ofn.lpstrFile, _MAX_PATH);
			return true;
		}
		return false;
	}
}

bool WinLoadROM(const char *filename)
{
	bool result = false;

#ifdef NETPLAY_SUPPORT
	if (Settings.NetPlay && !Settings.NetPlayServer)
	{
		S9xMessage (S9X_INFO, S9X_NETPLAY_NOT_SERVER,
			WINPROC_DISCONNECT);
		return result;
	}
#endif

	if (!Settings.StopEmulation)
	{
		Memory.SaveSRAM (S9xGetFilename (".srm", SRAM_DIR));
		S9xSaveCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
	}
	result = LoadROM (filename)!=0;
	Settings.StopEmulation = !result;
	if (!Settings.StopEmulation)
	{
		bool8 loadedSRAM = Memory.LoadSRAM (S9xGetFilename (".srm", SRAM_DIR));
		if(!loadedSRAM) // help migration from earlier Snes9x versions by checking ROM directory for savestates
			Memory.LoadSRAM (S9xGetFilename (".srm", ROMFILENAME_DIR));
		S9xLoadCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
		S9xAddToRecentGames (filename);
		CheckDirectoryIsWritable (S9xGetFilename (".---", SNAPSHOT_DIR));
		CheckMenuStates ();
#ifdef NETPLAY_SUPPORT
		if (NPServer.SendROMImageOnConnect)
			S9xNPServerQueueSendingROMImage ();
		else
			S9xNPServerQueueSendingLoadROMRequest (Memory.ROMName);
#endif
	}

	if(GUI.ControllerOption == SNES_SUPERSCOPE)
		SetCursor (GUI.GunSight);
	else
	{
		SetCursor (GUI.Arrow);
		GUI.CursorTimer = 60;
	}
	Settings.Paused = false;
	return result;
}

bool WinMoviePlay(const char* filename)
{
	struct MovieInfo info;
	int err;

	bool abort_anyway = false;
	if (Settings.StopEmulation) {
		SendMenuCommand(ID_FILE_LOAD_GAME);
		if (Settings.StopEmulation)
			return false;
		//abort_anyway = true;
	}

	err = S9xMovieGetInfo(filename, &info);
	if (err != SUCCESS) {
		_TCHAR* err_string = MOVIE_ERR_COULD_NOT_OPEN;
		switch(err)
		{
		case FILE_NOT_FOUND:
			err_string = MOVIE_ERR_NOT_FOUND_SHORT;
			break;
		case WRONG_FORMAT:
			err_string = MOVIE_ERR_WRONG_FORMAT_SHORT;
			break;
		case WRONG_VERSION:
			err_string = MOVIE_ERR_WRONG_VERSION_SHORT;
			break;
		}
		S9xSetInfoString(err_string);
		return false;
	}

//	if (S9xMovieActive()) {
//		if (MessageBox(GUI.hWnd, TEXT("Snes9x has already opened a movie, still want to open another movie yet?"), TEXT(SNES9X_INFO), MB_YESNO|MB_ICONQUESTION) == IDNO)
//			return false;
//	}

	while (info.ROMCRC32 != Memory.ROMCRC32 || strcmp(info.RawROMName, Memory.RawROMName) != 0) {
		char temp[512];
		sprintf(temp, "Movie's ROM: crc32=%08X, name=%s\nCurrent ROM: crc32=%08X, name=%s\n\nstill want to play the movie?",
			info.ROMCRC32, info.ROMName, Memory.ROMCRC32, Memory.ROMName);
		int sel = MessageBox(GUI.hWnd, temp, TEXT(SNES9X_INFO), MB_ABORTRETRYIGNORE|MB_ICONQUESTION);
		switch (sel) {
		case IDABORT:
			return false;
		case IDRETRY:
			SendMenuCommand(ID_FILE_LOAD_GAME);
			if (Settings.StopEmulation)
				return false;
			//abort_anyway = true;
			break;
		default:
			goto romcheck_exit;
		}
	}
	romcheck_exit:
	if (abort_anyway)
		return false;

	if (info.SyncFlags & MOVIE_SYNC_DATA_EXISTS)
	{
		Settings.UpAndDown = (info.SyncFlags & MOVIE_SYNC_LEFTRIGHT) ? true : false;
//		Settings.SoundSync = (info.SyncFlags & MOVIE_SYNC_SYNCSOUND) ? 1 : 0; // currently, it doesn't affect movie sync (general speaking), so leave the user setting
	}

	S9xMovieOpen (filename, GUI.MovieReadOnly);
	if(err != SUCCESS)
	{
		_TCHAR* err_string = MOVIE_ERR_COULD_NOT_OPEN;
		switch(err)
		{
		case FILE_NOT_FOUND:
			err_string = MOVIE_ERR_NOT_FOUND_SHORT;
			break;
		case WRONG_FORMAT:
			err_string = MOVIE_ERR_WRONG_FORMAT_SHORT;
			break;
		case WRONG_VERSION:
			err_string = MOVIE_ERR_WRONG_VERSION_SHORT;
			break;
		}
		S9xSetInfoString(err_string);
		return false;
	}
	return true;
}

char multiRomA [MAX_PATH] = {0}; // lazy, should put in sGUI and add init to {0} somewhere
char multiRomB [MAX_PATH] = {0};


static bool startingMovie = false;

extern HWND inputMacroHWND;

#define MOVIE_LOCKED_SETTING	if(S9xMovieActive()) {MessageBox(GUI.hWnd,TEXT("That setting is locked while a movie is active."),TEXT("Notice"),MB_OK|MB_ICONEXCLAMATION); break;}

LRESULT CALLBACK WinProc(
						 HWND hWnd,
						 UINT uMsg,
						 WPARAM wParam,
						 LPARAM lParam)
{
    unsigned int i;
    //bool showFPS;
#ifdef NETPLAY_SUPPORT
    char hostname [100];
#endif
	int modifiers = 0;

    if (ICPU.SavedAtOp)
    {
        if (MustDeferMessage(hWnd, uMsg, wParam, lParam))
            return 0;
    }

    switch (uMsg)
    {
	case WM_CREATE:
		g_hInst = ((LPCREATESTRUCT)lParam)->hInstance;
#ifndef MK_APU
		DeleteMenu(GUI.hMenu,IDM_CATCH_UP_SOUND,MF_BYCOMMAND);
#endif
		DragAcceptFiles(hWnd, GUI.allowDropFiles);
		return 0;
	case WM_KEYDOWN:
		if(wParam != VK_PAUSE)
			break;
	case WM_SYSKEYDOWN:
	case WM_CUSTKEYDOWN:
		{
			modifiers = GetModifiers(wParam);
			if(uMsg == WM_CUSTKEYDOWN)
			{
				if(wParam == VK_PAUSE || modifiers == CUSTKEY_ALT_MASK)
				{
					// WM_KEYDOWN or WM_SYSKEYDOWN should handle this
					break;
				}
			}

			if(!HandleKeyMessage(wParam,lParam, modifiers))
				return 0;
	        break;
		}

	case WM_KEYUP:
		if(wParam != VK_PAUSE)
			break;
	case WM_SYSKEYUP:
	case WM_CUSTKEYUP:
		{
			SCustomKey *key = CustomKeys.key;
			int modifiers = 0;
			if (wParam == VK_MENU)    modifiers |= CUSTKEY_ALT_MASK;
			if (wParam == VK_CONTROL) modifiers |= CUSTKEY_CTRL_MASK;
			if (wParam == VK_SHIFT)   modifiers |= CUSTKEY_SHIFT_MASK;
			while (!IsLastCustomKey(key)) {
				if ((wParam == key->key || (modifiers != 0 && modifiers == key->modifiers)) && key->handleKeyUp) {
					key->handleKeyUp();
				}
				key++;
			}
		}
		break;
	case WM_DROPFILES: {
		HDROP hDrop;
		//UINT fileNo;
		UINT fileCount;
		char filename[PATH_MAX];

		hDrop = (HDROP)wParam;
		fileCount = DragQueryFile(hDrop, 0xFFFFFFFF, NULL, 0);
		if (fileCount > 0) {
			DragQueryFile(hDrop, 0, filename, COUNT(filename));

			SetActiveWindow(hWnd);

			LPCTSTR ext = PathFindExtension(filename);
			if (lstrcmpi(ext, ".smv") == 0) {
				WinMoviePlay(filename);
			}
			else if (lstrcmpi(ext, ".lua") == 0) {
				if (S9xLoadLuaCode(filename)) {
					// success, there is nothing to do
				} else {
					// MessageBox(hDlg, "Oops", "Script not loaded", MB_OK); // Errors are displayed by the Lua code.
				}
			}
			else if (lstrcmpi(ext, ".wch") == 0) {
				if (!Settings.StopEmulation) {
					SendMenuCommand(ID_RAM_WATCH);
					Load_Watches(true, filename);
				}
			}
			else {
				bool extIsValid = false;

				if (ext[0] != _T('\0')) {
					if (valid_ext) // add valid extensions to string
					{
						ExtList* validExtNode = valid_ext;
						while (validExtNode)
						{
							extIsValid = (lstrcmpi(&ext[1], validExtNode->extension) == 0) ? true : false;
							if (extIsValid)
								break;
							validExtNode = validExtNode->next;
						}
					}
					else {
						extIsValid = (lstrcmpi(ext, ".smv") == 0) ? true : false;
					}
				}

				if (extIsValid) {
					if (!WinLoadROM(filename)) {
						MessageBox (hWnd, TEXT(ERR_CORRUPT_ROM), TEXT(SNES9X_INFO), MB_OK | MB_ICONSTOP);
					}
				}
				else {
					MessageBox (hWnd, TEXT("Couldn't handle the file having such extension."), TEXT(SNES9X_INFO), MB_OK | MB_ICONINFORMATION);
				}
			}
		}
		DragFinish(hDrop);
	}	break;
	case WM_COMMAND:
		switch (wParam & 0xffff)
		{
		case ID_FILE_WAV_RECORDING:
			if (!GUI.WAVOut)
				PostMessage(GUI.hWnd, WM_COMMAND, ID_FILE_WRITE_WAV, NULL);
			else
				PostMessage(GUI.hWnd, WM_COMMAND, ID_FILE_STOP_WAV, NULL);
			break;

		case ID_FILE_WRITE_WAV:
			{
				RestoreGUIDisplay ();  //exit DirectX
				OPENFILENAME  ofn;
				char  szFileName[MAX_PATH];
				char  szPathName[MAX_PATH];
				SetCurrentDirectory(S9xGetDirectory(DEFAULT_DIR));
				_fullpath(szPathName, GUI.SPCDir, MAX_PATH);
				mkdir(szPathName);

				strcpy(szFileName, S9xGetFilenameRel("wav"));

				ZeroMemory( (LPVOID)&ofn, sizeof(OPENFILENAME) );
				ofn.lStructSize = sizeof(OPENFILENAME);
				ofn.hwndOwner = GUI.hWnd;
				ofn.lpstrFilter = FILE_INFO_WAV_FILE_TYPE "\0*.wav\0" FILE_INFO_ANY_FILE_TYPE "\0*.*\0\0";
				ofn.lpstrFile = szFileName;
				ofn.lpstrDefExt = "wav";
				ofn.nMaxFile = MAX_PATH;
				ofn.Flags = OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT;
				ofn.lpstrInitialDir = szPathName;
				if(GetSaveFileName( &ofn ))
				{
					//ReInitSound(0);			// disable sound output 
					DoWAVOpen(szFileName);
				}
				RestoreSNESDisplay ();// re-enter after dialog
			}
			break;
		case ID_FILE_STOP_WAV:
			DoWAVClose(0);
			//ReInitSound(1);				// reenable sound output
			break;

		case ID_FILE_AVI_RECORDING:
			if (!GUI.AVIOut)
				PostMessage(GUI.hWnd, WM_COMMAND, ID_FILE_WRITE_AVI, NULL);
			else
				PostMessage(GUI.hWnd, WM_COMMAND, ID_FILE_STOP_AVI, NULL);
			break;

		case ID_FILE_WRITE_AVI:
			{
				RestoreGUIDisplay ();  //exit DirectX
				OPENFILENAME  ofn;
				char  szFileName[MAX_PATH];
				char  szPathName[MAX_PATH];
				SetCurrentDirectory(S9xGetDirectory(DEFAULT_DIR));
				_fullpath(szPathName, GUI.MovieDir, MAX_PATH);
				mkdir(szPathName);

				strcpy(szFileName, S9xGetFilenameRel("avi"));

				ZeroMemory( (LPVOID)&ofn, sizeof(OPENFILENAME) );
				ofn.lStructSize = sizeof(OPENFILENAME);
				ofn.hwndOwner = GUI.hWnd;
				ofn.lpstrFilter = FILE_INFO_AVI_FILE_TYPE "\0*.avi\0" FILE_INFO_ANY_FILE_TYPE "\0*.*\0\0";
				ofn.lpstrFile = szFileName;
				ofn.lpstrDefExt = "avi";
				ofn.nMaxFile = MAX_PATH;
				ofn.Flags = OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT;
				ofn.lpstrInitialDir = szPathName;
				if(GetSaveFileName( &ofn ))
				{
					//ReInitSound(0);			// disable sound output 
					DoAVIOpen(szFileName);
				}
				RestoreSNESDisplay ();// re-enter after dialog
			}
			break;
		case ID_FILE_STOP_AVI:
			DoAVIClose(0);
			//ReInitSound(1);				// reenable sound output
			break;

		case ID_AVI_DOUBLE_SCALE:
			GUI.AVIDoubleScale = !GUI.AVIDoubleScale;
			break;

		case ID_FILE_MOVIE_SEEK:
                        RestoreGUIDisplay ();
			DialogBox(g_hInst, MAKEINTRESOURCE(IDD_MOVIE_SEEK), hWnd, DlgSeekProc);
                        RestoreSNESDisplay ();
			break;
		case ID_MOVIE_CONTINUE:
			break;
		case ID_MOVIE_RESTART:
			S9xMovieRestart();
			break;
		case ID_MOVIE_READONLY:
			if (S9xMovieActive())
				S9xMovieToggleRecState();
			else
				GUI.MovieReadOnly = !GUI.MovieReadOnly;
			break;
		case ID_FILE_MOVIE_STOP:
			S9xMovieStop(FALSE);
			break;
		case ID_FILE_MOVIE_PLAY:
			{
				RestoreGUIDisplay ();  //exit DirectX
				OpenMovieParams op;
				memset(&op, 0, sizeof(op));
				if(DialogBoxParam(g_hInst, MAKEINTRESOURCE(IDD_OPENMOVIE), hWnd, DlgOpenMovie, (LPARAM)&op) &&
					op.Path[0]!='\0')
				{
					int err=S9xMovieOpen (op.Path, op.ReadOnly);
					if(err!=SUCCESS)
					{
						_TCHAR* err_string=TEXT(MOVIE_ERR_COULD_NOT_OPEN);
						switch(err)
						{
						case FILE_NOT_FOUND:
							err_string=TEXT(MOVIE_ERR_NOT_FOUND);
							break;
						case WRONG_FORMAT:
							err_string=TEXT(MOVIE_ERR_WRONG_FORMAT);
							break;
						case WRONG_VERSION:
							err_string=TEXT(MOVIE_ERR_WRONG_VERSION);
							break;
						}
						MessageBox( hWnd, err_string, TEXT(SNES9X_INFO), MB_OK);
					}
				}
				RestoreSNESDisplay ();// re-enter after dialog
			}
			break;
		case IDD_FILE_LUA_OPEN: 
			{
				if(!LuaConsoleHWnd)
					LuaConsoleHWnd = CreateDialog(g_hInst, MAKEINTRESOURCE(IDD_LUA), hWnd, (DLGPROC) DlgLuaScriptDialog);
				else
					SetForegroundWindow(LuaConsoleHWnd);
			}
			break;
		case IDD_FILE_LUA_CLOSE_ALL:
			{
				if(LuaConsoleHWnd)
					PostMessage(LuaConsoleHWnd, WM_CLOSE, 0, 0);
			}
			break;
		case ID_FILE_MOVIE_RECORD:
			{
				RestoreGUIDisplay ();  //exit DirectX
				OpenMovieParams op;
				memset(&op, 0, sizeof(op));
				if(DialogBoxParam(g_hInst, MAKEINTRESOURCE(IDD_CREATEMOVIE), hWnd, DlgCreateMovie, (LPARAM)&op) &&
					op.Path[0]!='\0')
				{
					if(Settings.ShutdownMaster)
					{
						static bool seenItOnce = false;
						if(!seenItOnce)
						{
							seenItOnce = true;
							MessageBox(hWnd, MOVIE_SHUTDOWNMASTER_WARNING, SNES9X_WARN, MB_OK);
						}
					}

					startingMovie = true;
					int err=S9xMovieCreate (op.Path, op.ControllersMask, op.Opts, op.Metadata, wcslen(op.Metadata));
					startingMovie = false;
					if(err!=SUCCESS)
					{
						_TCHAR* err_string=TEXT(MOVIE_ERR_COULD_NOT_OPEN);
						switch(err)
						{
						case FILE_NOT_FOUND:
							err_string=TEXT(MOVIE_ERR_NOT_FOUND);
							break;
						case WRONG_FORMAT:
							err_string=TEXT(MOVIE_ERR_WRONG_FORMAT);
							break;
						case WRONG_VERSION:
							err_string=TEXT(MOVIE_ERR_WRONG_VERSION);
							break;
						}
						MessageBox( hWnd, err_string, TEXT(SNES9X_INFO), MB_OK);
					}
				}
				RestoreSNESDisplay ();// re-enter after dialog
			}
			break;
		case IDM_GFX_PACKS:
			RestoreGUIDisplay ();  //exit DirectX
			DialogBox(g_hInst, MAKEINTRESOURCE(IDD_GFX_PACK), hWnd, DlgPackConfigProc);
			RestoreSNESDisplay ();// re-enter after dialog
			break;
		case IDM_CATCH_UP_SOUND:
			Settings.SampleCatchup=!Settings.SampleCatchup;
			if(Settings.SampleCatchup)
				CheckMenuItem(GUI.hMenu, IDM_CATCH_UP_SOUND, MFS_CHECKED);
			else CheckMenuItem(GUI.hMenu, IDM_CATCH_UP_SOUND, MFS_UNCHECKED);
			break;
		case IDM_SNES_JOYPAD:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_JOYPAD;
			ChangeInputDevice();
			break;
		case IDM_ENABLE_MULTITAP:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_MULTIPLAYER5;
			ChangeInputDevice();
			break;
		case IDM_SCOPE_TOGGLE:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_SUPERSCOPE;
			ChangeInputDevice();
			break;
		case IDM_JUSTIFIER:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_JUSTIFIER;
			ChangeInputDevice();
			break;
		case IDM_MOUSE_TOGGLE:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_MOUSE;
			ChangeInputDevice();
			break;
		case IDM_MOUSE_SWAPPED:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_MOUSE_SWAPPED;
			ChangeInputDevice();
			break;
		case IDM_MULTITAP8:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_MULTIPLAYER8;
			ChangeInputDevice();
			break;
		case IDM_JUSTIFIERS:
			MOVIE_LOCKED_SETTING
			GUI.ControllerOption = SNES_JUSTIFIER_2;
			ChangeInputDevice();
			break;

			//start turbo
		case ID_TURBO_R:
			GUI.TurboMask^=TURBO_R_MASK;
			if(GUI.TurboMask&TURBO_R_MASK)
				S9xSetInfoString (WINPROC_TURBO_R_ON);
			else S9xSetInfoString (WINPROC_TURBO_R_OFF);
			break;
		case ID_TURBO_L:
			GUI.TurboMask^=TURBO_L_MASK;
			if(GUI.TurboMask&TURBO_L_MASK)
				S9xSetInfoString (WINPROC_TURBO_L_ON);
			else S9xSetInfoString (WINPROC_TURBO_L_OFF);
			break;
		case ID_TURBO_A:
			GUI.TurboMask^=TURBO_A_MASK;
			if(GUI.TurboMask&TURBO_A_MASK)
				S9xSetInfoString (WINPROC_TURBO_A_ON);
			else S9xSetInfoString (WINPROC_TURBO_A_OFF);
			break;
		case ID_TURBO_B:
			GUI.TurboMask^=TURBO_B_MASK;
			if(GUI.TurboMask&TURBO_B_MASK)
				S9xSetInfoString (WINPROC_TURBO_B_ON);
			else S9xSetInfoString (WINPROC_TURBO_B_OFF);
			break;
		case ID_TURBO_Y:
			GUI.TurboMask^=TURBO_Y_MASK;
			if(GUI.TurboMask&TURBO_Y_MASK)
				S9xSetInfoString (WINPROC_TURBO_Y_ON);
			else S9xSetInfoString (WINPROC_TURBO_Y_OFF);
			break;
		case ID_TURBO_X:
			GUI.TurboMask^=TURBO_X_MASK;
			if(GUI.TurboMask&TURBO_X_MASK)
				S9xSetInfoString (WINPROC_TURBO_X_ON);
			else S9xSetInfoString (WINPROC_TURBO_X_OFF);
			break;
		case ID_TURBO_START:
			GUI.TurboMask^=TURBO_STA_MASK;
			if(GUI.TurboMask&TURBO_STA_MASK)
				S9xSetInfoString (WINPROC_TURBO_START_ON);
			else S9xSetInfoString (WINPROC_TURBO_START_OFF);
			break;
		case ID_TURBO_SELECT:
			GUI.TurboMask^=TURBO_SEL_MASK;
			if(GUI.TurboMask&TURBO_SEL_MASK)
				S9xSetInfoString (WINPROC_TURBO_SEL_ON);
			else S9xSetInfoString (WINPROC_TURBO_SEL_OFF);
			break;
		case ID_TURBO_LEFT:
			GUI.TurboMask^=TURBO_LEFT_MASK;
			if(GUI.TurboMask&TURBO_LEFT_MASK)
				S9xSetInfoString (WINPROC_TURBO_LEFT_ON);
			else S9xSetInfoString (WINPROC_TURBO_LEFT_OFF);
			break;
		case ID_TURBO_UP:
			GUI.TurboMask^=TURBO_UP_MASK;
			if(GUI.TurboMask&TURBO_UP_MASK)
				S9xSetInfoString (WINPROC_TURBO_UP_ON);
			else S9xSetInfoString (WINPROC_TURBO_UP_OFF);
			break;
		case ID_TURBO_RIGHT:
			GUI.TurboMask^=TURBO_RIGHT_MASK;
			if(GUI.TurboMask&TURBO_RIGHT_MASK)
				S9xSetInfoString (WINPROC_TURBO_RIGHT_ON);
			else S9xSetInfoString (WINPROC_TURBO_RIGHT_OFF);
			break;
		case ID_TURBO_DOWN:
			GUI.TurboMask^=TURBO_DOWN_MASK;
			if(GUI.TurboMask&TURBO_DOWN_MASK)
				S9xSetInfoString (WINPROC_TURBO_DOWN_ON);
			else S9xSetInfoString (WINPROC_TURBO_DOWN_OFF);
			break;
			//end turbo
		case ID_OPTIONS_DISPLAY:
			{
				int old_scale = GUI.NextScale;
//				bool old_stretch = GUI.Stretch;
				bool wasFullScreen = GUI.FullScreen;

				RestoreGUIDisplay ();
				//showFPS = Settings.DisplayFrameRate ? true : false;
				//if (!VOODOO_MODE && !GUI.FullScreen)
				//	GetWindowRect (GUI.hWnd, &GUI.window_size);
				DialogBox(g_hInst, MAKEINTRESOURCE(IDD_NEWDISPLAY), hWnd, DlgFunky);
				//_DirectXConfig (DirectDraw.lpDD, &Settings, &GUI, &showFPS);

				//Settings.DisplayFrameRate = showFPS;
				SwitchToGDI();
				if (GUI.NextScale != old_scale)
				{
					UpdateScale((RenderFilter &)old_scale, GUI.NextScale);
				}
				GUI.ScaleHiRes = GUI.NextScaleHiRes;
				RestoreSNESDisplay ();
				if (!GUI.FullScreen && wasFullScreen)
				{
					MoveWindow (GUI.hWnd, GUI.window_size.left,
						GUI.window_size.top,
						GUI.window_size.right - GUI.window_size.left,
						GUI.window_size.bottom - GUI.window_size.top, TRUE);
				}
				S9xGraphicsDeinit();
//				S9xDeinitUpdate();
				S9xSetWinPixelFormat ();
				S9xInitUpdate();
				S9xGraphicsInit();

				IPPU.RenderThisFrame = false;

//				if (old_stretch != GUI.Stretch || old_scale != GUI.Scale)
				{
					RECT rect;
					GetClientRect (GUI.hWnd, &rect);
					InvalidateRect (GUI.hWnd, &rect, true);
				}
				break;
			}

		case ID_OPTIONS_JOYPAD:
            RestoreGUIDisplay ();
			DialogBox(g_hInst, MAKEINTRESOURCE(IDD_INPUTCONFIG), hWnd, DlgInputConfig);
            RestoreSNESDisplay ();
            break;

		case ID_OPTIONS_KEYCUSTOM:
            RestoreGUIDisplay ();
			DialogBox(g_hInst, MAKEINTRESOURCE(IDD_KEYCUSTOM), hWnd, DlgHotkeyConfig);
            RestoreSNESDisplay ();
            break;

		case ID_OPTIONS_INPUT_MACRO:
			RestoreGUIDisplay ();
			if(!inputMacroHWND) // create and show non-modal macro editor window
			{
				CreateDialog(g_hInst, MAKEINTRESOURCE(IDD_MACRO_SETTINGS), hWnd, DlgInputMacro); // non-modal/modeless
				ShowWindow(inputMacroHWND, SW_SHOW);
			}
			else // already open so just reactivate the window
			{
				if(!IsWindowVisible(inputMacroHWND))
					ShowWindow(inputMacroHWND, SW_SHOW);
				SetActiveWindow(inputMacroHWND);
			}
			RestoreSNESDisplay ();
			break;

		case ID_FILE_LOADMULTICART:
			{
#ifdef NETPLAY_SUPPORT
				if (Settings.NetPlay && !Settings.NetPlayServer)
				{
					S9xMessage (S9X_INFO, S9X_NETPLAY_NOT_SERVER, WINPROC_DISCONNECT);
					break;
				}
#endif
				RestoreGUIDisplay ();

				const bool ok = (1 <= DialogBoxParam(g_hInst, MAKEINTRESOURCE(IDD_MULTICART), GUI.hWnd, DlgMultiROMProc, (LPARAM)NULL));

				if(ok)
				{
					if (!Settings.StopEmulation)
					{
						Memory.SaveSRAM (S9xGetFilename (".srm", SRAM_DIR));
						S9xSaveCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
					}
					Settings.StopEmulation = !LoadMultiROM (multiRomA, multiRomB);
					if (!Settings.StopEmulation)
					{
						bool8 loadedSRAM = Memory.LoadSRAM (S9xGetFilename (".srm", SRAM_DIR));
						if(!loadedSRAM) // help migration from earlier Snes9x versions by checking ROM directory for savestates
							Memory.LoadSRAM (S9xGetFilename (".srm", ROMFILENAME_DIR));
						S9xLoadCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
//						S9xAddToRecentGames (multiRomA, multiRomB);
						CheckDirectoryIsWritable (S9xGetFilename (".---", SNAPSHOT_DIR));
						CheckMenuStates ();
#ifdef NETPLAY_SUPPORT
						// still valid with multicart ???
						if (NPServer.SendROMImageOnConnect)
							S9xNPServerQueueSendingROMImage ();
						else
							S9xNPServerQueueSendingLoadROMRequest (Memory.ROMName);
#endif
					}

					if(GUI.ControllerOption == SNES_SUPERSCOPE)
						SetCursor (GUI.GunSight);
					else
					{
						SetCursor (GUI.Arrow);
						GUI.CursorTimer = 60;
					}
					Settings.Paused = false;
				}

				RestoreSNESDisplay ();
				GUI.ScreenCleared = true;
			}
			break;

		case ID_FILE_LOAD_GAME:
			{
				char filename [_MAX_PATH];

				RestoreGUIDisplay ();

				if(DoOpenRomDialog(filename))
					WinLoadROM(filename);

				RestoreSNESDisplay ();
				GUI.ScreenCleared = true;
			}
			break;

		case ID_FILE_EXIT:
            S9xSetPause (PAUSE_EXIT);
#ifdef USE_GLIDE
            S9xGlideEnable (FALSE);
#endif
            PostMessage (hWnd, WM_DESTROY, 0, 0);
            break;

		case ID_WINDOW_HIDEMENUBAR:
            ToggleMenuBar();
            GUI.ScreenCleared = true;
            break;

		case ID_LANGUAGE_ENGLISH:
            GUI.Language = 0;

            SetMenu( GUI.hWnd, LoadMenu( GUI.hInstance, MAKEINTRESOURCE( Languages[ GUI.Language].idMenu)));
            DestroyMenu( GUI.hMenu);
            GUI.hMenu = GetMenu( GUI.hWnd);
            break;
		case ID_LANGUAGE_NEDERLANDS:
            GUI.Language = 1;

            SetMenu( GUI.hWnd, LoadMenu( GUI.hInstance, MAKEINTRESOURCE( Languages[ GUI.Language].idMenu)));
            DestroyMenu( GUI.hMenu);
            GUI.hMenu = GetMenu( GUI.hWnd);
            break;
#ifdef NETPLAY_SUPPORT
		case ID_NETPLAY_SERVER:
            S9xRestoreWindowTitle ();
            EnableServer (!Settings.NetPlayServer);
			if(Settings.NetPlayServer)
			{
				char localhostmsg [512];
				// FIXME: need winsock2.h for this, don't know how to include it
				//struct addrinfo *aiList = NULL;
				//if(getaddrinfo("localhost", Settings.Port, NULL, &aiList) == 0)
				//{
				//	sprintf(localhostmsg, "Your server address is: %s", aiList->ai_canonname);
				//	MessageBox(GUI.hWnd,localhostmsg,"Note",MB_OK);
				//}
				//else
				{
					char localhostname [256];
					gethostname(localhostname,256);
					sprintf(localhostmsg, "Your host name is: %s\nYour port number is: %d", localhostname, Settings.Port);
					MessageBox(GUI.hWnd,localhostmsg,"Note",MB_OK);
				}
			}
            break;
        case ID_NETPLAY_CONNECT:
            RestoreGUIDisplay ();
			if(1<=DialogBoxParam(g_hInst, MAKEINTRESOURCE(IDD_NETCONNECT), hWnd, DlgNetConnect,(LPARAM)&hostname))

            {


				S9xSetPause (PAUSE_NETPLAY_CONNECT);

                if (!S9xNPConnectToServer (hostname, Settings.Port,
					Memory.ROMName))
                {
                    S9xClearPause (PAUSE_NETPLAY_CONNECT);
                }
            }

			RestoreSNESDisplay ();
            break;
        case ID_NETPLAY_DISCONNECT:
            if (Settings.NetPlay)
            {
                Settings.NetPlay = FALSE;
                S9xNPDisconnect ();
            }
            if (Settings.NetPlayServer)
            {
                Settings.NetPlayServer = FALSE;
                S9xNPStopServer ();
            }
            break;
        case ID_NETPLAY_OPTIONS:
			{
				bool8 old_netplay_server = Settings.NetPlayServer;
				RestoreGUIDisplay ();
				if(1<=DialogBox(g_hInst, MAKEINTRESOURCE(IDD_NPOPTIONS), hWnd, DlgNPOptions))
				{
					if (old_netplay_server != Settings.NetPlayServer)
					{
						Settings.NetPlayServer = old_netplay_server;
						S9xRestoreWindowTitle ();
						EnableServer (!Settings.NetPlayServer);
					}
				}
				RestoreSNESDisplay ();
				break;
			}
        case ID_NETPLAY_SYNC:
            S9xNPServerQueueSyncAll ();
            break;
        case ID_NETPLAY_ROM:
            if (NPServer.SyncByReset)
            {
			if (MessageBox (GUI.hWnd, TEXT(WINPROC_NET_RESTART), TEXT(SNES9X_WARN),
											MB_OKCANCEL | MB_ICONWARNING) == IDCANCEL)
											break;
            }
            S9xNPServerQueueSendingROMImage ();
            break;
        case ID_NETPLAY_SEND_ROM_ON_CONNECT:
            NPServer.SendROMImageOnConnect ^= TRUE;
            break;
        case ID_NETPLAY_SYNC_BY_RESET:
            NPServer.SyncByReset ^= TRUE;
            break;
#endif
		case ID_SOUNDINTERFACE_DIRECTSOUND:
		case ID_SOUNDINTERFACE_XAUDIO2:
		case ID_SOUNDINTERFACE_FMOD_DIRECTSOUND:
		case ID_SOUNDINTERFACE_FMOD_WAVE:
		case ID_SOUNDINTERFACE_FMOD_A3D:
		case ID_SOUNDINTERFACE_FMODEX_DEFAULT:
		case ID_SOUNDINTERFACE_FMODEX_ASIO:
			for( i = 0; i < COUNT(SoundDrivers); i ++)
				if (SoundDrivers[i].ident == (int) wParam)
				{
					EnterCriticalSection(&GUI.SoundCritSect);
					Settings.SoundDriver = SoundDrivers [i].interfaceId;
					if (!ReInitSound(1)) // !SetupSound()
					{	MessageBox( GUI.hWnd, Languages[ GUI.Language].errInitDS, TEXT(SNES9X_DXS), MB_OK | MB_ICONINFORMATION);	}
					LeaveCriticalSection(&GUI.SoundCritSect);
					break;
				}
				break;
			break;
		case ID_SOUND_8000HZ:
		case ID_SOUND_11025HZ:
		case ID_SOUND_16000HZ:
		case ID_SOUND_22050HZ:
		case ID_SOUND_30000HZ:
		case ID_SOUND_35000HZ:
		case ID_SOUND_44100HZ:
		case ID_SOUND_48000HZ:
		case ID_SOUND_32000HZ:
			for( i = 0; i < COUNT(SoundRates); i ++)
				if (SoundRates[i].ident == (int) wParam)
				{
					Settings.SoundPlaybackRate = SoundRates [i].rate;
					if (!ReInitSound(1)) // !SetupSound()
					{	MessageBox( GUI.hWnd, Languages[ GUI.Language].errInitDS, TEXT(SNES9X_DXS), MB_OK | MB_ICONINFORMATION);	}
					break;
				}
				break;

        case ID_CHANNELS_CHANNEL1: S9xToggleSoundChannel(0); break;
        case ID_CHANNELS_CHANNEL2: S9xToggleSoundChannel(1); break;
        case ID_CHANNELS_CHANNEL3: S9xToggleSoundChannel(2); break;
        case ID_CHANNELS_CHANNEL4: S9xToggleSoundChannel(3); break;
        case ID_CHANNELS_CHANNEL5: S9xToggleSoundChannel(4); break;
        case ID_CHANNELS_CHANNEL6: S9xToggleSoundChannel(5); break;
        case ID_CHANNELS_CHANNEL7: S9xToggleSoundChannel(6); break;
        case ID_CHANNELS_CHANNEL8: S9xToggleSoundChannel(7); break;
        case ID_CHANNELS_ENABLEALL: S9xToggleSoundChannel(8); break;

		case ID_SOUND_MUTE:
			Settings.Mute = !Settings.Mute;
			break;
        case ID_SOUND_25MS:
            Settings.SoundBufferSize = 1;
			ReInitSound(1);
            break;
        case ID_SOUND_50MS:
            Settings.SoundBufferSize = 2;
			ReInitSound(1);
            break;
        case ID_SOUND_100MS:
            Settings.SoundBufferSize = 4;
			ReInitSound(1);
            break;
        case ID_SOUND_200MS:
            Settings.SoundBufferSize = 8;
			ReInitSound(1);
            break;
        case ID_SOUND_500MS:
            Settings.SoundBufferSize = 16;
			ReInitSound(1);
            break;
        case ID_SOUND_1S:
            Settings.SoundBufferSize = 32;
			ReInitSound(1);
            break;
        case ID_SOUND_2S:
            Settings.SoundBufferSize = 64;
			ReInitSound(1);
            break;
		case ID_SOUND_MIXINTERVAL_10MS:
		case ID_SOUND_MIXINTERVAL_20MS:
		case ID_SOUND_MIXINTERVAL_30MS:
		case ID_SOUND_MIXINTERVAL_40MS:
		case ID_SOUND_MIXINTERVAL_50MS:
		case ID_SOUND_MIXINTERVAL_60MS:
		case ID_SOUND_MIXINTERVAL_70MS:
		case ID_SOUND_MIXINTERVAL_80MS:
		case ID_SOUND_MIXINTERVAL_90MS:
		case ID_SOUND_MIXINTERVAL_100MS:
		case ID_SOUND_MIXINTERVAL_110MS:
		case ID_SOUND_MIXINTERVAL_120MS:
		case ID_SOUND_MIXINTERVAL_130MS:
		case ID_SOUND_MIXINTERVAL_140MS:
		case ID_SOUND_MIXINTERVAL_150MS: {
			int menuID = wParam & 0xffff;

			for (i = 0; i < COUNT(MixIntervals); i++) {
				if (menuID == MixIntervals[i].ident) {
					Settings.SoundMixInterval = MixIntervals[i].ms;
					ReInitSound(1);
					break;
				}
			}
		}	break;
        case ID_SOUND_STEREO:
            Settings.Stereo = !Settings.Stereo;
			ReInitSound(1);
            break;
        case ID_SOUND_REVERSE_STEREO:
            Settings.ReverseStereo = !Settings.ReverseStereo;
            break;
		case ID_SOUND_MUTEFRAMEADVANCE:
			GUI.FAMute = !GUI.FAMute;
			break;
		case ID_SOUND_16BIT:
			Settings.SixteenBitSound = !Settings.SixteenBitSound;
			ReInitSound(1);
			break;
		case ID_SOUND_INTERPOLATED:
			Settings.InterpolatedSound = !Settings.InterpolatedSound;
			S9xDisplayStateChange (WINPROC_INTERPOLATED_SND, Settings.InterpolatedSound);
			break;
		case ID_SOUND_ECHO:
			Settings.DisableSoundEcho = !Settings.DisableSoundEcho;
			S9xDisplayStateChange ("Echo", !Settings.DisableSoundEcho);
			break;
		case ID_SOUND_ENVXREADING:
			Settings.SoundEnvelopeHeightReading = !Settings.SoundEnvelopeHeightReading;
			S9xDisplayStateChange ("ENVX reading", Settings.SoundEnvelopeHeightReading);
			break;
		case ID_SOUND_FAKEMUTE:
			Settings.FakeMuteFix = !Settings.FakeMuteFix;
			S9xDisplayStateChange ("Fake mute", Settings.FakeMuteFix);
			break;
		case ID_SOUND_SYNC:
			Settings.SoundSync = !Settings.SoundSync;
			S9xDisplayStateChange (WINPROC_SYNC_SND, Settings.SoundSync);
			break;
		case ID_SOUND_FLEXIBLEMIX:
			GUI.FlexibleSoundMixMaster = !GUI.FlexibleSoundMixMaster;
			S9xDisplayStateChange ("Flexible mix", GUI.FlexibleSoundMixMaster);
			break;
        case ID_SOUND_OPTIONS:
			{
				struct SSettings orig = Settings;
				RestoreGUIDisplay ();
				if(1<=DialogBoxParam(g_hInst,MAKEINTRESOURCE(IDD_SOUND_OPTS),hWnd,DlgSoundConf, (LPARAM)&Settings))
				{
					if (orig.NextAPUEnabled != Settings.NextAPUEnabled)
					{
						if (!Settings.NextAPUEnabled)
						{
							if (MessageBox (GUI.hWnd, TEXT(WINPROC_SND_OFF),
															TEXT(SNES9X_SNDQ),
															MB_YESNO | MB_ICONQUESTION) == IDNO)
							{
								Settings.NextAPUEnabled = orig.NextAPUEnabled;
							}
							else
							{
								Settings.APUEnabled = FALSE;
								ReInitSound(0);
							}
						}
						else
						{
							if (!Settings.StopEmulation)
							{
                            MessageBox (GUI.hWnd, TEXT(WINPROC_SND_RESTART), TEXT(SNES9X_SNDQ),
														MB_OK | MB_ICONINFORMATION);
							}
							else
								Settings.APUEnabled = Settings.NextAPUEnabled;
						}
					}
					else
						if (memcmp(&orig,&Settings,sizeof(SSettings)))
						{
							ReInitSound(1);
						}
				}
				RestoreSNESDisplay ();
				break;
			}
#ifdef RTC_DEBUGGER
				case IDM_7110_RTC:
					{
						struct SPC7110RTC origrtc = s7r.rtc;
						RestoreGUIDisplay ();
						if(1<=DialogBoxParam(g_hInst,MAKEINTRESOURCE(IDD_7110_RTC),hWnd,SPC7110rtc, (LPARAM)&origrtc))
						{
							rtc_f9.reg[0x00]=origrtc.reg[0x00];
							rtc_f9.reg[0x01]=origrtc.reg[0x01];
							rtc_f9.reg[0x02]=origrtc.reg[0x02];
							rtc_f9.reg[0x03]=origrtc.reg[0x03];
							rtc_f9.reg[0x04]=origrtc.reg[0x04];
							rtc_f9.reg[0x05]=origrtc.reg[0x05];
							rtc_f9.reg[0x06]=origrtc.reg[0x06];
							rtc_f9.reg[0x07]=origrtc.reg[0x07];
							rtc_f9.reg[0x08]=origrtc.reg[0x08];
							rtc_f9.reg[0x09]=origrtc.reg[0x09];
							rtc_f9.reg[0x0A]=origrtc.reg[0x0A];
							rtc_f9.reg[0x0B]=origrtc.reg[0x0B];
							rtc_f9.reg[0x0C]=origrtc.reg[0x0C];

							rtc_f9.reg[0x0D]=origrtc.reg[0x0D];

							rtc_f9.reg[0x0E]=origrtc.reg[0x0E];
							rtc_f9.reg[0x0F]=origrtc.reg[0x0F];
							rtc_f9.last_used=time(NULL);

						}
						RestoreSNESDisplay ();
						break;
					}
#endif
						case ID_RENDERMETHOD_DIRECTDRAW: {
							bool wasFullScreen = GUI.FullScreen;

							if (wasFullScreen)
								ToggleFullScreen();

							GUI.outputMethod = DIRECTDRAW;
							Direct3D.deInitialize();
							DirectDraw.InitDirectDraw();
							RestoreSNESDisplay();

							if (wasFullScreen)
								ToggleFullScreen();

							S9xSetWinPixelFormat();
							S9xInitUpdate();
							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_RENDERMETHOD_DIRECT3D: {
							bool wasFullScreen = GUI.FullScreen;

							if (wasFullScreen)
								ToggleFullScreen();

							GUI.outputMethod = DIRECT3D;
							DirectDraw.DeInitializeDirectDraw();
							Direct3D.initialize(GUI.hWnd);
							Direct3D.changeRenderSize(0,0);

							if (wasFullScreen)
								ToggleFullScreen();

							S9xSetWinPixelFormat();
							S9xInitUpdate();
							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_RENDEROPTIONS_DDRAWUSEVIDEOMEMORY: {
							GUI.ddrawUseVideoMemory = !GUI.ddrawUseVideoMemory;

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_RENDEROPTIONS_DDRAWUSELOCALVIDEOMEM: {
							GUI.ddrawUseLocalVidMem = !GUI.ddrawUseLocalVidMem;

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_RENDEROPTIONS_TRIPLEBUFFERING: {
							GUI.tripleBuffering = !GUI.tripleBuffering;

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_RENDEROPTIONS_D3DNOFILTER: {
							GUI.d3dFilter = NEAREST;

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_RENDEROPTIONS_D3DBILINEAR: {
							GUI.d3dFilter = BILINEAR;

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_WINDOW_STRETCH: {
							GUI.Stretch = !GUI.Stretch;

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_WINDOW_TVPIXELRATIO: {
							if(abs(GUI.AspectWidth-301) >= 12)
							{
								GUI.AspectWidth = 301; // since our 2x blargg blitter uses 602, this has to be 301 instead of 300 to prevent stretching
								
								// adjust the size right away in the common cases to make it a little more convenient
								for(int i = 0; i < 4; i++)
									if(abs(GUI.window_size.right - GUI.window_size.left - 256*(i+1)) < 12)
										SendMessage(hWnd, WM_COMMAND, (WPARAM)(ID_WINDOW_X1+i),(LPARAM)(NULL));
							}
							else
							{
								GUI.AspectWidth = 256;

								// adjust the size right away in the common cases to make it a little more convenient
								for(int i = 0; i < 4; i++)
									if(abs(GUI.window_size.right - GUI.window_size.left - 301*(i+1)) < 12)
										SendMessage(hWnd, WM_COMMAND, (WPARAM)(ID_WINDOW_X1+i),(LPARAM)(NULL));
							}

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_WINDOW_ASPECTRATIO: {
							GUI.AspectRatio = !GUI.AspectRatio;

							if(GUI.outputMethod==DIRECT3D)
								Direct3D.changeRenderSize(0,0);
							else
								RestoreSNESDisplay ();

							if(DirectDraw.Clipped) S9xReRefresh();
						}	break;
						case ID_WINDOW_X1: {
							float snesAspect = (float) GUI.AspectWidth / (GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT);
							int newWidth = GUI.AspectWidth;
							int newHeight = (snesAspect == 1.0)
									? (GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT)
									: (int) (newWidth / snesAspect);
							S9xResizeWindow(newWidth, newHeight, true);
						}	break;
						case ID_WINDOW_X2: {
							float snesAspect = (float) GUI.AspectWidth / (GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT);
							int newWidth = GUI.AspectWidth * 2;
							int newHeight = (snesAspect == 1.0)
									? ((GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT) * 2)
									: (int) (newWidth / snesAspect);
							S9xResizeWindow(newWidth, newHeight, true);
						}	break;
						case ID_WINDOW_X3: {
							float snesAspect = (float) GUI.AspectWidth / (GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT);
							int newWidth = GUI.AspectWidth * 3;
							int newHeight = (snesAspect == 1.0)
									? ((GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT) * 3)
									: (int) (newWidth / snesAspect);
							S9xResizeWindow(newWidth, newHeight, true);
						}	break;
						case ID_WINDOW_X4: {
							float snesAspect = (float) GUI.AspectWidth / (GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT);
							int newWidth = GUI.AspectWidth * 4;
							int newHeight = (snesAspect == 1.0)
									? ((GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT) * 4)
									: (int) (newWidth / snesAspect);
							S9xResizeWindow(newWidth, newHeight, true);
						}	break;
						case ID_WINDOW_LOCKRESIZE: {
							LONG dwStyle;
							RECT clientRect;
							int width, height;

							GUI.windowResizeLocked = !GUI.windowResizeLocked;

							GetClientRect(GUI.hWnd, &clientRect);
							width = clientRect.right - clientRect.left;
							height = clientRect.bottom - clientRect.top;

							dwStyle = GetWindowLong(GUI.hWnd, GWL_STYLE);
							dwStyle = GUI.windowResizeLocked ? (dwStyle & ~WS_THICKFRAME) : (dwStyle | WS_THICKFRAME);
							SetWindowLong(GUI.hWnd, GWL_STYLE, dwStyle);

							// refresh client size
							S9xResizeWindow(width, height, false);
						}	break;
						case ID_WINDOW_FULLSCREEN:
							ToggleFullScreen ();
							break;
						case ID_VIDEO_TEXTINIMAGE:
							GUI.MessagesInImage = !GUI.MessagesInImage;

							if(DirectDraw.Clipped) S9xReRefresh();
							break;
						case ID_VIDEO_LUAGUIINIMAGE:
							Settings.LuaDrawingsInScreen = !Settings.LuaDrawingsInScreen;

							if(DirectDraw.Clipped) S9xReRefresh();
							break;
						case ID_VIDEO_LAYERS_BG1:
							Settings.BG_Forced ^= 1;
							S9xDisplayStateChange (WINPROC_BG1, !(Settings.BG_Forced & 1));
							break;
						case ID_VIDEO_LAYERS_BG2:
							Settings.BG_Forced ^= 2;
							S9xDisplayStateChange (WINPROC_BG2, !(Settings.BG_Forced & 2));
							break;
						case ID_VIDEO_LAYERS_BG3:
							Settings.BG_Forced ^= 4;
							S9xDisplayStateChange (WINPROC_BG3, !(Settings.BG_Forced & 4));
							break;
						case ID_VIDEO_LAYERS_BG4:
							Settings.BG_Forced ^= 8;
							S9xDisplayStateChange (WINPROC_BG4, !(Settings.BG_Forced & 8));
							break;
						case ID_VIDEO_LAYERS_SPRITES:
							Settings.BG_Forced ^= 16;
							S9xDisplayStateChange (WINPROC_SPRITES, !(Settings.BG_Forced & 16));
							break;
						case ID_VIDEO_CLIPPINGWIDOWS:
							Settings.DisableGraphicWindows = !Settings.DisableGraphicWindows;
							S9xDisplayStateChange (WINPROC_CLIPWIN, !Settings.DisableGraphicWindows);
							break;
						case ID_VIDEO_TRANSPARENCY:
							//	if (Settings.SixteenBit)
								{
									Settings.Transparency = !Settings.Transparency;
									S9xDisplayStateChange (WINPROC_TRANSPARENCY,
										Settings.Transparency);
								}
							//	else
							//	{
							//		S9xSetInfoString ("Transparency requires Sixteen Bit mode.");
							//	}
							break;
						case ID_EMULATOR_HDMAEMULATION:
							Settings.DisableHDMA = !Settings.DisableHDMA;
							S9xDisplayStateChange (WINPROC_HDMA_TEXT, !Settings.DisableHDMA);
							break;
						case ID_EMULATOR_HIRES:
							Settings.SupportHiRes = !Settings.SupportHiRes;
							S9xDisplayStateChange ("Hi Res", Settings.SupportHiRes);
							break;
						case ID_EMULATOR_EXTENDHEIGHT:
							GUI.HeightExtend = !GUI.HeightExtend;
							S9xDisplayStateChange ("Extend Height", GUI.HeightExtend);
							break;
						case ID_DISPLAY_FRAMERATE:
							Settings.DisplayFrameRate = !Settings.DisplayFrameRate;
							//S9xDisplayStateChange ("Frame rate display", Settings.DisplayFrameRate);
							break;
						case ID_DISPLAY_INPUT:
							Settings.DisplayPressedKeys = !Settings.DisplayPressedKeys;
							//S9xDisplayStateChange ("Input display", Settings.DisplayPressedKeys);
							break;
						case ID_DISPLAY_FRAMECOUNTER:
							Settings.DisplayFrame = !Settings.DisplayFrame;
							//S9xDisplayStateChange ("Frame counter", Settings.DisplayFrame);
							break;
						case ID_DISPLAY_LAGCOUNTER:
							Settings.DisplayLagCounter = !Settings.DisplayLagCounter;
							//S9xDisplayStateChange ("Lag counter", Settings.DisplayLagCounter);
							break;
						case ID_COUNTER_IN_FRAMES:
							Settings.CounterInFrames = !Settings.CounterInFrames;
							break;
						case ID_EMULATOR_CUSTOMROMOPEN:
							GUI.CustomRomOpen = !GUI.CustomRomOpen;
							break;
						case ID_EMULATOR_SAVECOMPRESS0: Settings.CompressionLevel = 0; break;
						case ID_EMULATOR_SAVECOMPRESS1: Settings.CompressionLevel = 1; break;
						case ID_EMULATOR_SAVECOMPRESS2: Settings.CompressionLevel = 2; break;
						case ID_EMULATOR_SAVECOMPRESS3: Settings.CompressionLevel = 3; break;
						case ID_EMULATOR_SAVECOMPRESS4: Settings.CompressionLevel = 4; break;
						case ID_EMULATOR_SAVECOMPRESS5: Settings.CompressionLevel = 5; break;
						case ID_EMULATOR_SAVECOMPRESS6: Settings.CompressionLevel = 6; break;
						case ID_EMULATOR_SAVECOMPRESS7: Settings.CompressionLevel = 7; break;
						case ID_EMULATOR_SAVECOMPRESS8: Settings.CompressionLevel = 8; break;
						case ID_EMULATOR_SAVECOMPRESS9: Settings.CompressionLevel = 9; break;
						case ID_FILE_SAVE_SPC_DATA:
							spc_is_dumping = 1;
							break;
						case ID_FILE_SAVE_SPC_DATA_IMM:
							S9xSPCDump (S9xGetFilenameInc((".spc"), SPC_DIR));
							break;
						case ID_SAVESCREENSHOT:
							Settings.TakeScreenshot=true;
							break;
						case ID_FILE_SAVE_SRAM_DATA: {
							bool8 success = Memory.SaveSRAM (S9xGetFilename (".srm", SRAM_DIR));
							if(!success)
								S9xMessage(S9X_ERROR, S9X_FREEZE_FILE_INFO, SRM_SAVE_FAILED);
						}	break;
						case ID_FILE_RESET:
#ifdef NETPLAY_SUPPORT
							if (Settings.NetPlayServer)
							{
								S9xNPReset ();

								// FIXME: necessary?
								ReInitSound(1);
							}
							else
								if (!Settings.NetPlay)
#endif
								{
									if(S9xMoviePlaying())
										S9xMovieStop (TRUE);
									if(S9xMovieActive())
										S9xMovieRecordReset();
									else
										S9xSoftReset();

									// FIXME: necessary?
									ReInitSound(1);
								}
								if(!S9xMovieRecording())
									Settings.Paused = false;
								break;
						case ID_FILE_PAUSE:
							Settings.Paused = !Settings.Paused;
							Settings.FrameAdvance = false;
							GUI.FrameAdvanceJustPressed = 0;
							CenterCursor();
							if(!Settings.Paused)
								S9xMouseOn();
							break;
						case ID_FILE_LOAD0:
							FreezeUnfreeze (0, FALSE);
							break;
						case ID_FILE_LOAD1:
							FreezeUnfreeze (1, FALSE);
							break;
						case ID_FILE_LOAD2:
							FreezeUnfreeze (2, FALSE);
							break;
						case ID_FILE_LOAD3:
							FreezeUnfreeze (3, FALSE);
							break;
						case ID_FILE_LOAD4:
							FreezeUnfreeze (4, FALSE);
							break;
						case ID_FILE_LOAD5:
							FreezeUnfreeze (5, FALSE);
							break;
						case ID_FILE_LOAD6:
							FreezeUnfreeze (6, FALSE);
							break;
						case ID_FILE_LOAD7:
							FreezeUnfreeze (7, FALSE);
							break;
						case ID_FILE_LOAD8:
							FreezeUnfreeze (8, FALSE);
							break;
						case ID_FILE_LOAD9:
							FreezeUnfreeze (9, FALSE);
							break;
						case ID_FILE_SAVE0:
							FreezeUnfreeze (0, TRUE);
							break;
						case ID_FILE_SAVE1:
							FreezeUnfreeze (1, TRUE);
							break;
						case ID_FILE_SAVE2:
							FreezeUnfreeze (2, TRUE);
							break;
						case ID_FILE_SAVE3:
							FreezeUnfreeze (3, TRUE);
							break;
						case ID_FILE_SAVE4:
							FreezeUnfreeze (4, TRUE);
							break;
						case ID_FILE_SAVE5:
							FreezeUnfreeze (5, TRUE);
							break;
						case ID_FILE_SAVE6:
							FreezeUnfreeze (6, TRUE);
							break;
						case ID_FILE_SAVE7:
							FreezeUnfreeze (7, TRUE);
							break;
						case ID_FILE_SAVE8:
							FreezeUnfreeze (8, TRUE);
							break;
						case ID_FILE_SAVE9:
							FreezeUnfreeze (9, TRUE);
							break;
						case ID_JOYPAD_1:
						case ID_JOYPAD_2:
						case ID_JOYPAD_3:
						case ID_JOYPAD_4:
						case ID_JOYPAD_5: {
							int menuID = wParam & 0xffff;
							int pad;

							for (pad = 0; pad < COUNT(idJoypad); pad++) {
								if (menuID == idJoypad[pad])
									break;
							}
							if (pad < COUNT(idJoypad)) {
								Joypad[pad].Enabled = !Joypad[pad].Enabled;
							}

							static char str[64];
							sprintf(str, "Joypad#%d", pad + 1);
							S9xDisplayStateChange(str, Joypad[pad].Enabled);
						}	break;
						case ID_JOYPAD_ALLOWLEFTRIGHT:
							Settings.UpAndDown = !Settings.UpAndDown;
							break;
						case ID_FRAMESKIP_AUTOMATIC:
							Settings.SkipFrames = AUTO_FRAMERATE;
							break;
						case ID_FRAMESKIP_0:
						case ID_FRAMESKIP_1:
						case ID_FRAMESKIP_2:
						case ID_FRAMESKIP_3:
						case ID_FRAMESKIP_4:
						case ID_FRAMESKIP_5:
						case ID_FRAMESKIP_6:
						case ID_FRAMESKIP_7:
						case ID_FRAMESKIP_8:
						case ID_FRAMESKIP_9:
						{
							int menuID = wParam & 0xffff;

							for (i = 0; i < COUNT(FrameSkipAmounts); i++) {
								if (menuID == FrameSkipAmounts[i].ident) {
									Settings.SkipFrames = FrameSkipAmounts[i].amount + 1;
									break;
								}
							}
						}	break;
						case ID_FRAMESKIP_SETTINGS:
							DialogBox(g_hInst, MAKEINTRESOURCE(IDD_FRAMESKIP_SETTINGS), hWnd, DlgFrameSkipSettings);
							break;
						case ID_FRAMESKIP_THROTTLE_6:
						case ID_FRAMESKIP_THROTTLE_25:
						case ID_FRAMESKIP_THROTTLE_50:
						case ID_FRAMESKIP_THROTTLE_75:
						case ID_FRAMESKIP_THROTTLE_100:
						case ID_FRAMESKIP_THROTTLE_150:
						case ID_FRAMESKIP_THROTTLE_200:
						case ID_FRAMESKIP_THROTTLE_400:
						{
							int menuID = wParam & 0xffff;

							for (i = 0; i < COUNT(FrameTimingRates); i++) {
								if (menuID == FrameTimingRates[i].ident) {
									S9xSetRunSpeed(FrameTimingRates[i].rate);
									break;
								}
							}
						}	break;
						case ID_FRAMESKIP_THROTTLE_OTHER: {
							const char *str;
							int throttle;

							do {
								str = S9xStringInput("Throttle (5%... 1000%)");
								if (!str)
									break;

								throttle = atoi(str);
							} while (throttle < 5 || throttle > 1000);

							if (str)
								S9xSetRunSpeed((double) throttle / 100);
						}	break;
						case ID_FRAMESKIP_THROTTLE_INCREASE: {
							int i;
							uint32 normalFrameTime = (Settings.PAL?Settings.FrameTimePAL:Settings.FrameTimeNTSC);
							for(i=1; (uint32)(normalFrameTime/FrameTimingRates[i].rate+0.5)<Settings.FrameTime; ++i)
								;
							S9xSetRunSpeed(FrameTimingRates[i-1].rate);
						}	break;
						case ID_FRAMESKIP_THROTTLE_DECREASE: {
							int i;
							uint32 normalFrameTime = (Settings.PAL?Settings.FrameTimePAL:Settings.FrameTimeNTSC);
							for(i=1; (uint32)(normalFrameTime/FrameTimingRates[i].rate+0.5)<Settings.FrameTime; ++i)
								;

							S9xSetRunSpeed(FrameTimingRates[i+1].rate);
						}	break;
						case ID_PRIORITY_HIGHEST:
						case ID_PRIORITY_ABOVENORMAL:
						case ID_PRIORITY_NORMAL:
						case ID_PRIORITY_BELOWNORMAL: {
							int menuID = wParam & 0xffff;
							HANDLE hThread = GetCurrentThread();

							for (i = 0; i < COUNT(ThreadPriorities); i++) {
								if (menuID == ThreadPriorities[i].ident) {
									GUI.threadPriority = ThreadPriorities[i].priority;
									SetThreadPriority(hThread, GUI.threadPriority);
									break;
								}
							}
						}	break;
						case ID_CHEAT_ENTER:
							RestoreGUIDisplay ();
							S9xRemoveCheats ();
							DialogBox(g_hInst, MAKEINTRESOURCE(IDD_CHEATER), hWnd, DlgCheater);
							S9xSaveCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
							S9xApplyCheats ();
							RestoreSNESDisplay ();
							break;
						case ID_RAM_SEARCH:
							if(!RamSearchHWnd)
							{
								reset_address_info();
								RamSearchHWnd = CreateDialog(g_hInst, MAKEINTRESOURCE(IDD_RAMSEARCH), hWnd, (DLGPROC) RamSearchProc);
							}
							else
								SetForegroundWindow(RamSearchHWnd);
							break;
						case ID_RAM_WATCH:
							if(!RamWatchHWnd)
							{
								RamWatchHWnd = CreateDialog(g_hInst, MAKEINTRESOURCE(IDD_RAMWATCH), hWnd, (DLGPROC) RamWatchProc);
								//				DialogsOpen++;
							}
							else
								SetForegroundWindow(RamWatchHWnd);
							break;
						case ID_RAM_SEARCH_OLD:
							RestoreGUIDisplay ();
							if(!oldRamSearchHWND) // create and show non-modal RAM search window
							{
								oldRamSearchHWND = CreateDialog(g_hInst, MAKEINTRESOURCE(IDD_RAM_SEARCH), hWnd, DlgRAMSearch); // non-modal/modeless
								ShowWindow(oldRamSearchHWND, SW_SHOW);
							}
							else // already open so just reactivate the window
							{
								SetActiveWindow(oldRamSearchHWND);
							}
							RestoreSNESDisplay ();
							break;
						case ID_TRACE_LOGGER: {
							if (S9xTraceLogStream) {
								fclose(S9xTraceLogStream);
								S9xTraceLogStream = NULL;
							}
							else {
								RestoreGUIDisplay ();  //exit DirectX
								OPENFILENAME  ofn;
								char  szFileName[MAX_PATH];
								char  szPathName[MAX_PATH];

								//SetCurrentDirectory(S9xGetDirectory(DEFAULT_DIR));
								//_fullpath(szPathName, GUI.SPCDir, MAX_PATH);
								//mkdir(szPathName);
								strcpy(szPathName, "");
								strcpy(szFileName, S9xGetFilenameRel("log"));

								ZeroMemory( (LPVOID)&ofn, sizeof(OPENFILENAME) );
								ofn.lStructSize = sizeof(OPENFILENAME);
								ofn.hwndOwner = GUI.hWnd;
								ofn.lpstrFilter = "Log Text (*.log;*.txt)" "\0*.log;*.txt\0" FILE_INFO_ANY_FILE_TYPE "\0*.*\0\0";
								ofn.lpstrFile = szFileName;
								ofn.lpstrDefExt = "log";
								ofn.nMaxFile = MAX_PATH;
								ofn.Flags = OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT;
								ofn.lpstrInitialDir = szPathName;
								if(GetSaveFileName( &ofn ))
								{
									S9xTraceLogStream = fopen(szFileName, "w");
								}
								RestoreSNESDisplay ();// re-enter after dialog
							}
						}	break;
						case ID_CHEAT_DISABLE:
							Settings.ApplyCheats = !Settings.ApplyCheats;
							if (!Settings.ApplyCheats){
								S9xRemoveCheats ();
								S9xMessage (S9X_INFO, S9X_GAME_GENIE_CODE_ERROR, CHEATS_INFO_DISABLED);
							}else{
								S9xApplyCheats ();
								bool on = false;
								extern struct SCheatData Cheat;
								for (uint32 i = 0; i < Cheat.num_cheats && !on; i++)
									if (Cheat.c [i].enabled)
										on = true;
								S9xMessage (S9X_INFO, S9X_GAME_GENIE_CODE_ERROR, on ? CHEATS_INFO_ENABLED : CHEATS_INFO_ENABLED_NONE);
							}
							break;
						case ID_OPTIONS_SETTINGS:
							RestoreGUIDisplay ();
							DialogBox(g_hInst, MAKEINTRESOURCE(IDD_EMU_SETTINGS), hWnd, DlgEmulatorProc);
							RestoreSNESDisplay ();
							break;
						case ID_HELP_ABOUT:
							RestoreGUIDisplay ();
							DialogBox(g_hInst, MAKEINTRESOURCE(IDD_ABOUT), hWnd, DlgAboutProc);
							RestoreSNESDisplay ();
							break;
#ifdef DEBUGGER
						case ID_DEBUG_TRACE:
							{
								Trace ();
								break;
							}
						case ID_DEBUG_FRAME_ADVANCE:
							CPU.Flags |= FRAME_ADVANCE_FLAG;
							ICPU.FrameAdvanceCount = 1;
							Settings.Paused = FALSE;
							break;
#endif
						case IDM_7110_CACHE:
							RestoreGUIDisplay ();
							DialogBox(g_hInst, MAKEINTRESOURCE(IDD_SPC7110_CACHE), hWnd, DlgSP7PackConfig);
							RestoreSNESDisplay ();
							break;
						case IDM_LOG_7110:
							Do7110Logging();
							break;
						case IDM_ROM_INFO:
							RestoreGUIDisplay ();
							DialogBox(g_hInst, MAKEINTRESOURCE(IDD_ROM_INFO), hWnd, DlgInfoProc);
							RestoreSNESDisplay ();
							break;
						case ID_EMULATOR_BACKGROUNDRUN:
							GUI.InactivePause = !GUI.InactivePause;
							GUI.BackgroundInput &= !GUI.InactivePause;
							break;
						case ID_EMULATOR_BACKGROUNDINPUT:
							GUI.BackgroundInput = !GUI.BackgroundInput;
							GUI.InactivePause &= !GUI.BackgroundInput;
							break;
						default: {
							int menuID = wParam & 0xffff;
							if (menuID >= ID_FILTER_MIN && menuID <= ID_FILTER_MAX)
							{
								int filter = menuID - ID_FILTER_MIN;
								RenderFilter oldFilter, oldFilterHiRes;
								RenderFilter newFilter, newFilterHiRes;
								int oldScale, newScale;

								newFilter = (RenderFilter)filter;
								if (GetFilterHiResSupport(newFilter))
									newFilterHiRes = newFilter;
								else {
									switch(GetFilterScale(GUI.Scale)){
										case 1: newFilterHiRes = FILTER_SIMPLE1X; break;
										default:
										case 2: newFilterHiRes = FILTER_SIMPLE2X; break;
										case 3: newFilterHiRes = FILTER_SIMPLE3X; break;
									}
								}

								oldFilter = GUI.Scale;
								oldFilterHiRes = GUI.ScaleHiRes;
								if (oldFilter != newFilter || oldFilterHiRes != newFilterHiRes) {
									oldScale = max(GetFilterScale(oldFilter), GetFilterScale(oldFilterHiRes));
									newScale = max(GetFilterScale(newFilter), GetFilterScale(newFilterHiRes));

									GUI.Scale = GUI.NextScale = newFilter;
									GUI.ScaleHiRes = GUI.NextScaleHiRes = newFilterHiRes;

									if (oldScale != newScale)
										RestoreSNESDisplay();

									// refresh screen, so the user can see the new filter
									if(DirectDraw.Clipped) S9xReRefresh();
								}
							}
							else if (menuID >= ID_RECENT_MIN && menuID <= ID_RECENT_MAX)
							{
								int i = menuID - ID_RECENT_MIN;
								int j = 0;
								{
									while (j < MAX_RECENT_GAMES_LIST_SIZE && j != i)
										j++;
									if (i == j)
									{
										if (!WinLoadROM (GUI.RecentGames [i]))
										{
											sprintf (String, ERR_ROM_NOT_FOUND, GUI.RecentGames [i]);
											S9xMessage (S9X_ERROR, S9X_ROM_NOT_FOUND, String);
											S9xRemoveFromRecentGames(i);
										}
									}
								}
							}
						}	break;
            }
            break;

	case WM_EXITMENULOOP:
		UpdateWindow(GUI.hWnd);
		UpdateBackBuffer();
		S9xClearPause (PAUSE_MENU);
		break;

	case WM_ENTERMENULOOP:
		S9xSetPause (PAUSE_MENU);
#ifdef USE_GLIDE
		S9xGlideEnable (FALSE);
#endif
		CheckMenuStates ();

		SwitchToGDI();
		DrawMenuBar( GUI.hWnd);
		break;

	case WM_CLOSE: {
		bool maximized = GUI.window_maximized;
		ShowWindow(GUI.hWnd, SW_RESTORE);
		GUI.window_maximized = maximized;
		if (!VOODOO_MODE && !GUI.FullScreen && !GUI.EmulatedFullScreen && !GUI.window_maximized)
			GetWindowRect (GUI.hWnd, &GUI.window_size);
	}	break;

	case WM_DESTROY:
		Memory.SaveSRAM(S9xGetFilename(".srm", SRAM_DIR));
		if(CleanUp7110)
			(*CleanUp7110)();
		GUI.hWnd = NULL;
		PostQuitMessage (0);
		return (0);
	case WM_PAINT:
		{
			PAINTSTRUCT paint;

			BeginPaint (GUI.hWnd, &paint);

			// refresh screen
			if ((Settings.Paused || Settings.ForcedPause) && !Settings.StopEmulation)
			{
				if(DirectDraw.Clipped) S9xReRefresh();
			}

			EndPaint (GUI.hWnd, &paint);
			return 0;
		}
	case WM_SYSCOMMAND:
        {
            // Prevent screen saver from starting if not paused
			//kode54 says add the ! to fix the screensaver pevention.
            if (!(Settings.ForcedPause || Settings.StopEmulation ||
				(Settings.Paused && !Settings.FrameAdvance)) &&
                (wParam == SC_SCREENSAVE || wParam == SC_MONITORPOWER))
                return (0);
            break;
        }
	case WM_ACTIVATE:
		if (LOWORD(wParam) == WA_INACTIVE)
		{
#ifdef USE_GLIDE
			if (VOODOO_MODE)
			{
				S9xGlideEnable (FALSE);
#if 0
				MoveWindow (GUI.hWnd, GUI.window_size.left,
					GUI.window_size.top,
					GUI.window_size.right - GUI.window_size.left,
					GUI.window_size.bottom - GUI.window_size.top,
					TRUE);
#endif
			}
#endif
			if(GUI.InactivePause)
			{
				S9xSetPause (PAUSE_INACTIVE_WINDOW);
			}
		}
		else
		{
///			if(GUI.InactivePause)
			{
				S9xClearPause (PAUSE_INACTIVE_WINDOW);
			}
			IPPU.ColorsChanged = TRUE;

			UpdateWindow(GUI.hWnd);
			UpdateBackBuffer();
		}

		// refresh screen
		if ((Settings.Paused || Settings.ForcedPause) && !Settings.StopEmulation)
		{
			if(GUI.outputMethod==DIRECT3D)
				Direct3D.changeRenderSize(0,0); // XXX: refresh drawing rect?
			if(DirectDraw.Clipped) S9xReRefresh();
		}
		return 0;
	case WM_ACTIVATEAPP: {
		// refresh screen
		if (Settings.Paused || Settings.ForcedPause && !Settings.StopEmulation)
		{
			if(DirectDraw.Clipped) S9xReRefresh();
		}
		return 0;
	}
	case WM_QUERYNEWPALETTE:
		//            if (!GUI.FullScreen && GUI.ScreenDepth == 8)
		//                RealizePalette (GUI.WindowDC);
		break;
	case WM_SIZE:
		if (wParam == SIZE_MAXIMIZED)
		{
			if (!GUI.FullScreen)
				GUI.window_maximized = true;
		}
		if (wParam == SIZE_RESTORED)
		{
			if (!GUI.FullScreen)
				GUI.window_maximized = false;
			if(GUI.InactivePause)
			{
				S9xClearPause (PAUSE_WINDOW_ICONISED);
			}
			//if (!GUI.FullScreen && !GUI.window_maximized) {
			//	GetWindowRect (GUI.hWnd, &GUI.window_size);
			//}
		}
		if (wParam == SIZE_MINIMIZED || wParam == SIZE_MAXHIDE)
		{
#ifdef USE_GLIDE
			S9xGlideEnable (FALSE);
#endif
///			if(GUI.InactivePause)
			{
				S9xClearPause (PAUSE_WINDOW_ICONISED);
			}
		}
		if(GUI.outputMethod==DIRECT3D)
			Direct3D.changeRenderSize(LOWORD(lParam),HIWORD(lParam));
		break;
	case WM_MOVE:
		//if (!GUI.FullScreen && !GUI.window_maximized) {
		//	GetWindowRect (GUI.hWnd, &GUI.window_size);
		//}
		break;
	case WM_ENTERSIZEMOVE:
		S9xSetPause(PAUSE_MENU);
		break;
	case WM_EXITSIZEMOVE:
		S9xClearPause(PAUSE_MENU);
		break;
	case WM_DISPLAYCHANGE:
		// FIXME: SetDisplayMode often crashes snes9x, So I finally decided to do...nothing.
		// It allows snes9x to toggle the mode, even while another snes9x process is running.
		// Instead, if you change display settings via Display Properties, the main screen will be lost.
		// However, you can restore it by opening Display Configuration dialog (then close it soon,
		// all you need to do is just open it). It's 256x better than a crash, isn't it?
		// 
		// If you will try to fix it, be careful. A bad fix may cause another new glitch.
		// Some examples you should look carefully:
		// - startup with GUI.FullScreen = TRUE
		// - toggle fullscreen mode while another snes9x process is running
		// - change display settings via Display Properties while running snes9x
/*
		if (!GUI.FullScreen)
		{
			if (!VOODOO_MODE && !OPENGL_MODE &&
				(GUI.outputMethod==DIRECT3D)?Direct3D.changeRenderSize(0,0):
				DirectDraw.SetDisplayMode (GUI.Width, GUI.Height, max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes)), GUI.Depth, GUI.RefreshRate,
				!GUI.FullScreen, GUI.tripleBuffering))
			{
				S9xGraphicsDeinit();
				S9xSetWinPixelFormat ();
				S9xInitUpdate();
				S9xGraphicsInit();
			}
		}
*/
		break;
	case WM_MOUSEMOVE:
		if(Settings.StopEmulation)
		{
			SetCursor (GUI.Arrow);
			break;
		}
		// Lo-word of lparam is xpos, hi-word is ypos
//		if (!GUI.IgnoreNextMouseMove)
		{
			//POINT p;
			//p.x = GET_X_LPARAM(lParam);
			//p.y = GET_Y_LPARAM(lParam);
			//ClientToScreen (GUI.hWnd, &p);
			if ((!Settings.ForcedPause && !Settings.StopEmulation &&
				!(Settings.Paused && !Settings.FrameAdvance)) &&
				(GUI.ControllerOption==SNES_MOUSE || GUI.ControllerOption==SNES_MOUSE_SWAPPED)
			   )
			{
				CenterCursor();
			}
			else if (GUI.ControllerOption==SNES_SUPERSCOPE || GUI.ControllerOption==SNES_JUSTIFIER || GUI.ControllerOption==SNES_JUSTIFIER_2)
			{
				POINT mouse = { GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam) };
				ClientToSNESScreen(&mouse, true);
				GUI.MouseX = mouse.x;
				GUI.MouseY = mouse.y;
			}
			else
			{
//				GUI.MouseX = p.x;
//				GUI.MouseY = p.y;
			}
		}
//		else
//			GUI.IgnoreNextMouseMove = false;

		if(!GUI.IgnoreNextMouseMove)
			S9xMouseOn ();
		else
			GUI.IgnoreNextMouseMove = false;
		return 0;
	case WM_LBUTTONDOWN:
		S9xMouseOn ();
		GUI.MouseButtons |= 1;
		break;
	case WM_LBUTTONUP:
		S9xMouseOn ();
		GUI.MouseButtons &= ~1;
		break;
	case WM_RBUTTONDOWN:
		S9xMouseOn ();
		GUI.MouseButtons |= 2;
		break;
	case WM_RBUTTONUP:
		S9xMouseOn ();
		GUI.MouseButtons &= ~2;
		if(GUI.ControllerOption==SNES_JUSTIFIER || GUI.ControllerOption==SNES_JUSTIFIER_2)
		{
			RECT size;
			GetClientRect (GUI.hWnd, &size);
			GUI.MouseButtons&=~1;
			GUI.MouseX=(IPPU.RenderedScreenWidth*(lParam & 0xffff))/(size.right-size.left);
			GUI.MouseY=(((lParam >> 16) & 0xffff)*IPPU.RenderedScreenHeight)/(size.bottom-size.top);
		}
		break;
	case WM_MBUTTONDOWN:
		S9xMouseOn ();
		GUI.MouseButtons |= 4;
		break;
	case WM_MBUTTONUP:
		S9xMouseOn ();
		GUI.MouseButtons &= ~4;
		break;
#ifdef NETPLAY_SUPPORT
	case WM_USER + 3:
		NetPlay.Answer = S9xLoadROMImage ((const char *) lParam);
		SetEvent (NetPlay.ReplyEvent);
		break;
	case WM_USER + 2:
		S9xMessage (0, 0, NetPlay.WarningMsg);
		break;
	case WM_USER + 1:
		RestoreGUIDisplay ();
		S9xRestoreWindowTitle ();
		MessageBox (GUI.hWnd, NetPlay.ErrorMsg,
			TEXT(SNES9X_NP_ERROR), MB_OK | MB_ICONSTOP);
		RestoreSNESDisplay ();
		break;
	case WM_USER:
		if (NetPlay.ActionMsg [0] == 0)
			S9xRestoreWindowTitle ();
		else
		{
			TCHAR buf [NP_MAX_ACTION_LEN + 10];

			sprintf (buf, TEXT("%s %3d%%"), NetPlay.ActionMsg, (int) lParam);
			SetWindowText (GUI.hWnd, buf);
		}
#if 0
		if ((int) lParam >= 0)
		{
			RestoreGUIDisplay ();
			DialogBox(g_hInst, MAKEINTRESOURCE(IDD_NETPLAYPROGRESS), hWnd, DlgNPProgress);
		}
		else
		{
			DialogBox(g_hInst, MAKEINTRESOURCE(IDD_NETPLAYPROGRESS), hWnd, DlgNPProgress);
			RestoreSNESDisplay ();
		}
#endif
		break;
#endif
    }
    return DefWindowProc (hWnd, uMsg, wParam, lParam);
}

/*****************************************************************************/
/* Hotkeys                                                                   */
/*****************************************************************************/
void HotkeyRecentROM (UINT i)
{
	UINT cmdID = ID_RECENT_MIN + i;

	if (cmdID <= ID_RECENT_MAX)
		PostMenuCommand(cmdID);
}
void HotkeyRecentROM0 (bool justPressed) { HotkeyRecentROM(0); }
void HotkeyRecentROM1 (bool justPressed) { HotkeyRecentROM(1); }
void HotkeyRecentROM2 (bool justPressed) { HotkeyRecentROM(2); }
void HotkeyRecentROM3 (bool justPressed) { HotkeyRecentROM(3); }
void HotkeyRecentROM4 (bool justPressed) { HotkeyRecentROM(4); }
void HotkeyRecentROM5 (bool justPressed) { HotkeyRecentROM(5); }
void HotkeyRecentROM6 (bool justPressed) { HotkeyRecentROM(6); }
void HotkeyRecentROM7 (bool justPressed) { HotkeyRecentROM(7); }
void HotkeyRecentROM8 (bool justPressed) { HotkeyRecentROM(8); }
void HotkeyRecentROM9 (bool justPressed) { HotkeyRecentROM(9); }

void HotkeyOpenROM (bool justPressed) { PostMenuCommand(ID_FILE_LOAD_GAME); }
void HotkeyOpenMultiCart (bool justPressed) { PostMenuCommand(ID_FILE_LOADMULTICART); }
void HotkeyPause (bool justPressed) { PostMenuCommand(ID_FILE_PAUSE); }
void HotkeyResetGame (bool justPressed) { PostMenuCommand(ID_FILE_RESET); }
void HotkeyUpResetGame () { }
void HotkeySaveScreenShot (bool justPressed) { PostMenuCommand(ID_SAVESCREENSHOT); }
void HotkeySaveSPC (bool justPressed) { PostMenuCommand(ID_FILE_SAVE_SPC_DATA); }
void HotkeySaveSRAM (bool justPressed) { PostMenuCommand(ID_FILE_SAVE_SRAM_DATA); }
void HotkeySaveSPC7110Log (bool justPressed) { PostMenuCommand(IDM_LOG_7110); }
void HotkeyRecordAVI (bool justPressed) { PostMenuCommand(ID_FILE_AVI_RECORDING); }

void HotkeySave0 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE0); }
void HotkeySave1 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE1); }
void HotkeySave2 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE2); }
void HotkeySave3 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE3); }
void HotkeySave4 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE4); }
void HotkeySave5 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE5); }
void HotkeySave6 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE6); }
void HotkeySave7 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE7); }
void HotkeySave8 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE8); }
void HotkeySave9 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_SAVE9); }
void HotkeyLoad0 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD0); }
void HotkeyLoad1 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD1); }
void HotkeyLoad2 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD2); }
void HotkeyLoad3 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD3); }
void HotkeyLoad4 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD4); }
void HotkeyLoad5 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD5); }
void HotkeyLoad6 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD6); }
void HotkeyLoad7 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD7); }
void HotkeyLoad8 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD8); }
void HotkeyLoad9 (bool justPressed) { if (justPressed) PostMenuCommand(ID_FILE_LOAD9); }

void HotkeySelectSave (int i)
{
	GUI.CurrentSaveSlot = i;

	static char str [64];
	sprintf(str, FREEZE_INFO_SET_SLOT_N, GUI.CurrentSaveSlot);
	S9xSetInfoString(str);
}
void HotkeySelectSave0 (bool justPressed) { HotkeySelectSave(0); }
void HotkeySelectSave1 (bool justPressed) { HotkeySelectSave(1); }
void HotkeySelectSave2 (bool justPressed) { HotkeySelectSave(2); }
void HotkeySelectSave3 (bool justPressed) { HotkeySelectSave(3); }
void HotkeySelectSave4 (bool justPressed) { HotkeySelectSave(4); }
void HotkeySelectSave5 (bool justPressed) { HotkeySelectSave(5); }
void HotkeySelectSave6 (bool justPressed) { HotkeySelectSave(6); }
void HotkeySelectSave7 (bool justPressed) { HotkeySelectSave(7); }
void HotkeySelectSave8 (bool justPressed) { HotkeySelectSave(8); }
void HotkeySelectSave9 (bool justPressed) { HotkeySelectSave(9); }

void HotkeySlotPlus (bool justPressed)
{
	GUI.CurrentSaveSlot++;
	if(GUI.CurrentSaveSlot > 9)
		GUI.CurrentSaveSlot = 0;

	static char str [64];
	sprintf(str, FREEZE_INFO_SET_SLOT_N, GUI.CurrentSaveSlot);
	S9xSetInfoString(str);
}

void HotkeySlotMinus (bool justPressed)
{
	GUI.CurrentSaveSlot--;
	if(GUI.CurrentSaveSlot < 0)
		GUI.CurrentSaveSlot = 9;

	static char str [64];
	sprintf(str, FREEZE_INFO_SET_SLOT_N, GUI.CurrentSaveSlot);
	S9xSetInfoString(str);
}

void HotkeySlotSave (bool justPressed)
{
	FreezeUnfreeze (GUI.CurrentSaveSlot, true);
}

void HotkeySlotLoad (bool justPressed)
{
	FreezeUnfreeze (GUI.CurrentSaveSlot, false);
}

void HotkeyToggleJoypad0 (bool justPressed) { PostMenuCommand(ID_JOYPAD_1); }
void HotkeyToggleJoypad1 (bool justPressed) { PostMenuCommand(ID_JOYPAD_2); }
void HotkeyToggleJoypad2 (bool justPressed) { PostMenuCommand(ID_JOYPAD_3); }
void HotkeyToggleJoypad3 (bool justPressed) { PostMenuCommand(ID_JOYPAD_4); }
void HotkeyToggleJoypad4 (bool justPressed) { PostMenuCommand(ID_JOYPAD_5); }
void HotkeyToggleJoypad5 (bool justPressed) { }
void HotkeyToggleJoypad6 (bool justPressed) { }
void HotkeyToggleJoypad7 (bool justPressed) { }

void HotkeyJoypadSwap (bool justPressed)
{
	if(!S9xMoviePlaying())
	{
		S9xApplyCommand(S9xGetCommandT("SwapJoypads"),1,0);
	}
}

void HotkeySwitchControllers (bool justPressed)
{
	if((!S9xMovieActive() || !S9xMovieGetFrameCounter()))
	{
//		int prevControllerOption = GUI.ControllerOption;
//		do {
			++GUI.ControllerOption %= SNES_MAX_CONTROLLER_OPTIONS;
//		} while(!((1<<GUI.ControllerOption) & GUI.ValidControllerOptions) && prevControllerOption != GUI.ControllerOption);

		ChangeInputDevice();

		if (GUI.ControllerOption == SNES_MOUSE || GUI.ControllerOption == SNES_MOUSE_SWAPPED)
			CenterCursor();

		S9xReportControllers();
	}
}

void HotkeyTurboA (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_A),(LPARAM)(NULL)); }
void HotkeyTurboB (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_B),(LPARAM)(NULL)); }
void HotkeyTurboY (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_Y),(LPARAM)(NULL)); }
void HotkeyTurboX (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_X),(LPARAM)(NULL)); }
void HotkeyTurboL (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_L),(LPARAM)(NULL)); }
void HotkeyTurboR (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_R),(LPARAM)(NULL)); }
void HotkeyTurboStart (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_START),(LPARAM)(NULL)); }
void HotkeyTurboSelect (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_SELECT),(LPARAM)(NULL)); }
void HotkeyTurboLeft (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_LEFT),(LPARAM)(NULL)); }
void HotkeyTurboUp (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_UP),(LPARAM)(NULL)); }
void HotkeyTurboRight (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_RIGHT),(LPARAM)(NULL)); }
void HotkeyTurboDown (bool justPressed) { PostMessage(GUI.hWnd, WM_COMMAND, (WPARAM)(ID_TURBO_DOWN),(LPARAM)(NULL)); }

void HotkeyScopeTurbo (bool justPressed)
{
	GUI.superscope_turbo = 1;
}

void HotkeyScopePause (bool justPressed)
{
	GUI.superscope_pause = 1;
}
void HotkeyUpScopePause ()
{
	GUI.superscope_pause = 0;
}

void HotkeySpeedUp (bool justPressed) { PostMenuCommand(ID_FRAMESKIP_THROTTLE_INCREASE); }
void HotkeySpeedDown (bool justPressed) { PostMenuCommand(ID_FRAMESKIP_THROTTLE_DECREASE); }

void HotkeySkipUp (bool justPressed)
{
	if (Settings.SkipFrames == AUTO_FRAMERATE)
		Settings.SkipFrames = 1;
	else
	{
		if (Settings.SkipFrames < 10)
			Settings.SkipFrames++;
	}

	if (Settings.SkipFrames == AUTO_FRAMERATE)
		S9xSetInfoString (WINPROC_AUTOSKIP);
	else
	{
		sprintf (InfoString, WINPROC_FRAMESKIP, Settings.SkipFrames - 1);
		S9xSetInfoString (InfoString);
	}
}

void HotkeySkipDown (bool justPressed)
{
	if (Settings.SkipFrames <= 1)
		Settings.SkipFrames = AUTO_FRAMERATE;
	else
	{
		if (Settings.SkipFrames != AUTO_FRAMERATE)
			Settings.SkipFrames--;
	}

	if (Settings.SkipFrames == AUTO_FRAMERATE)
		S9xSetInfoString (WINPROC_AUTOSKIP);
	else
	{
		sprintf (InfoString, WINPROC_FRAMESKIP, Settings.SkipFrames - 1);
		S9xSetInfoString (InfoString);
	}
}

void HotkeyFastForward (bool justPressed)
{
	if(Settings.SPC7110RTC)
		return;

	//if(!Settings.TurboMode)
	//	S9xMessage (S9X_INFO, S9X_TURBO_MODE, WINPROC_TURBOMODE_TEXT);
	Settings.TurboMode = TRUE;
}
void HotkeyUpFastForward ()
{
	Settings.TurboMode = FALSE;
}

void HotkeyToggleFastForward (bool justPressed)
{
	if(Settings.SPC7110RTC)
		return;

	Settings.TurboMode ^= TRUE;
	//if (Settings.TurboMode)
	//	S9xMessage (S9X_INFO, S9X_TURBO_MODE, WINPROC_TURBOMODE_ON);
	//else
	//	S9xMessage (S9X_INFO, S9X_TURBO_MODE, WINPROC_TURBOMODE_OFF);
}

void HotkeyFrameAdvance (bool justPressed)
{
	static DWORD lastTime = 0;
	if((int)(timeGetTime() - lastTime) > 20 && (!ICPU.SavedAtOp || prevPadReadFrame == Timings.TotalEmulatedFrames))
	{
		lastTime = timeGetTime();
		if(Settings.Paused || GUI.FASkipsNonInput)
		{
			if(!ICPU.SavedAtOp)
				prevPadReadFrame = (uint32)-1;
			Settings.Paused = false;
			S9xMouseOn();
			GUI.IgnoreNextMouseMove = true;
			Settings.Paused = true;

			Settings.FrameAdvance = true;
			GUI.FrameAdvanceJustPressed = 2;
			// kick the main thread out of GetMessage (just in case)
			if(!ICPU.SavedAtOp)
			SendMessage(GUI.hWnd, WM_NULL, 0, 0);
		}
		else
		{
			Settings.Paused = true;
		}

		CenterCursor();
	}
}

void HotkeyBGL1 (bool justPressed) { PostMenuCommand(ID_VIDEO_LAYERS_BG1); }
void HotkeyBGL2 (bool justPressed) { PostMenuCommand(ID_VIDEO_LAYERS_BG2); }
void HotkeyBGL3 (bool justPressed) { PostMenuCommand(ID_VIDEO_LAYERS_BG3); }
void HotkeyBGL4 (bool justPressed) { PostMenuCommand(ID_VIDEO_LAYERS_BG4); }
void HotkeyBGL5 (bool justPressed) { PostMenuCommand(ID_VIDEO_LAYERS_SPRITES); }
void HotkeyClippingWindows (bool justPressed) { PostMenuCommand(ID_VIDEO_CLIPPINGWIDOWS); }
void HotkeyTransparency (bool justPressed) { PostMenuCommand(ID_VIDEO_TRANSPARENCY); }
void HotkeyHDMA (bool justPressed) { PostMenuCommand(ID_EMULATOR_HDMAEMULATION); }

void HotkeyBGLHack (bool justPressed)
{
//	Settings.BGLayering = !Settings.BGLayering;
//	S9xDisplayStateChange (WINPROC_BGHACK, Settings.BGLayering);
}

void HotkeyInterpMode7 (bool justPressed)
{
//	Settings.Mode7Interpolate ^= TRUE;
//	S9xDisplayStateChange (WINPROC_MODE7INTER, Settings.Mode7Interpolate);
}

void HotkeyGLCube (bool justPressed)
{
	#ifdef USE_OPENGL
		OpenGL.draw_cube ^= TRUE;
	#else
		S9xSetInfoString ("GLCube requires USE_OPENGL on.");
	#endif
}

void HotkeyToggleSound0 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL1); }
void HotkeyToggleSound1 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL2); }
void HotkeyToggleSound2 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL3); }
void HotkeyToggleSound3 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL4); }
void HotkeyToggleSound4 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL5); }
void HotkeyToggleSound5 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL6); }
void HotkeyToggleSound6 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL7); }
void HotkeyToggleSound7 (bool justPressed) { PostMenuCommand(ID_CHANNELS_CHANNEL8); }

void HotkeyMoviePlay (bool justPressed) { PostMenuCommand(ID_FILE_MOVIE_PLAY); }
void HotkeyMovieRecord (bool justPressed) { PostMenuCommand(ID_FILE_MOVIE_RECORD); }
void HotkeyMovieStop (bool justPressed) { PostMenuCommand(ID_FILE_MOVIE_STOP); }
void HotkeyReadOnly (bool justPressed) { PostMenuCommand(ID_MOVIE_READONLY); }

void HotkeyShowPressed (bool justPressed)
{
	Settings.DisplayPressedKeys = Settings.DisplayPressedKeys?0:2;

	if(Settings.DisplayPressedKeys==2)
		S9xMessage(S9X_INFO, S9X_MOVIE_INFO, INPUT_INFO_DISPLAY_ENABLED);
	else
		S9xMessage(S9X_INFO, S9X_MOVIE_INFO, INPUT_INFO_DISPLAY_DISABLED);
}

void HotkeyFrameCount (bool justPressed)
{
	S9xMovieToggleFrameDisplay ();
}

void S9xReRefresh();

void HotkeyLagCount (bool justPressed)
{
	Settings.DisplayLagCounter = !Settings.DisplayLagCounter;
	S9xReRefresh();
}

void HotkeyFrameAndLagCount (bool justPressed)
{
	Settings.DisplayFrame = !Settings.DisplayFrame;
	Settings.DisplayLagCounter = Settings.DisplayFrame;
	// updating the frame counter string here won't work, because it may or may not be 1 too high now
	S9xReRefresh();
}

void HotkeyResetLagCounter (bool justPressed)
{
	Timings.LagCounter = 0;
}

void HotkeyToggleMacro (int i)
{
	MacroToggleState(i);
}
void HotkeyToggleMacro0 (bool justPressed) { HotkeyToggleMacro(0); }
void HotkeyToggleMacro1 (bool justPressed) { HotkeyToggleMacro(1); }
void HotkeyToggleMacro2 (bool justPressed) { HotkeyToggleMacro(2); }
void HotkeyToggleMacro3 (bool justPressed) { HotkeyToggleMacro(3); }
void HotkeyToggleMacro4 (bool justPressed) { HotkeyToggleMacro(4); }
void HotkeyToggleMacro5 (bool justPressed) { HotkeyToggleMacro(5); }
void HotkeyToggleMacro6 (bool justPressed) { HotkeyToggleMacro(6); }
void HotkeyToggleMacro7 (bool justPressed) { HotkeyToggleMacro(7); }
void HotkeyEditMacro (bool justPressed) { PostMenuCommand(ID_OPTIONS_INPUT_MACRO); }
void HotkeyToggleCheats (bool justPressed) { PostMenuCommand(ID_CHEAT_DISABLE); }
void HotkeyLoadLuaScript (bool justPressed) { PostMenuCommand(IDD_FILE_LUA_OPEN); }
void HotkeyReloadLuaScript (bool justPressed) { S9xReloadLuaCode(); }
void HotkeyStopLuaScript (bool justPressed) { S9xLuaStop(); }

/*****************************************************************************/
/* WinInit                                                                   */
/*****************************************************************************/
BOOL WinInit( HINSTANCE hInstance)
{
    WNDCLASSEX wndclass;
	ZeroMemory(&wndclass, sizeof(wndclass));
	wndclass.cbSize = sizeof(wndclass);

    wndclass.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
    wndclass.lpfnWndProc = WinProc;
    wndclass.cbClsExtra = 0;
    wndclass.cbWndExtra = 0;
    wndclass.hInstance = hInstance;
    wndclass.hIcon = LoadIcon (hInstance, MAKEINTRESOURCE(IDI_ICON1));
    wndclass.hIconSm = (HICON)LoadImage(hInstance, MAKEINTRESOURCE(IDI_ICON1), IMAGE_ICON, 16, 16, 0);
    wndclass.hCursor = NULL; //LoadCursor (NULL, IDC_ARROW);
    wndclass.lpszMenuName = NULL;
    wndclass.lpszClassName = TEXT("Snes9X: WndClass");
	wndclass.hbrBackground=(HBRUSH)GetStockObject(BLACK_BRUSH);

	//// Initialize the struct to zero
	//ZeroMemory(&wcx,sizeof(WNDCLASSEX));
	//wcx.cbSize = sizeof(WNDCLASSEX); // Must always be sizeof(WNDCLASSEX)
	//wcx.style = CS_HREDRAW|CS_VREDRAW |CS_DBLCLKS ; // Class styles
	//wcx.lpfnWndProc = (WNDPROC)MainWndProc; // Pointer to the callback procedure
	//wcx.cbClsExtra = 0; // Extra byte to allocate following the wndclassex structure
	//wcx.cbWndExtra = 0; // Extra byte to allocate following an instance of the structure
	//wcx.hInstance = hInstance; // Instance of the application
	//wcx.hIcon = NULL; // Class Icon
	//wcx.hCursor = LoadCursor(NULL, IDC_ARROW); // Class Cursor
	//wcx.hbrBackground = (HBRUSH)(COLOR_WINDOW); // Background brush
	//wcx.lpszMenuName = NULL; // Menu resource
	//wcx.lpszClassName = "Lesson2"; // Name of this class
	//wcx.hIconSm = NULL; // Small icon for this class

    GUI.hInstance = hInstance;

    if (!RegisterClassEx (&wndclass))
	{
		MessageBox (NULL, "Failed to register windows class", "Internal Error", MB_OK | MB_ICONSTOP);
        return FALSE;
	}

	GUI.hMenu = LoadMenu (hInstance, MAKEINTRESOURCE( Languages[ GUI.Language].idMenu));
    if (GUI.hMenu == NULL)
	{
		MessageBox (NULL, "Failed to initialize the menu.\nThis could indicate a failure of your operating system;\ntry closing some other windows or programs, or restart your computer, before opening Snes9x again.\nOr, if you compiled this program yourself, ensure that Snes9x was built with the proper resource files.", "Snes9X - Menu Initialization Failure", MB_OK | MB_ICONSTOP);
//        return FALSE; // disabled: try to function without the menu
	}

    TCHAR buf [100];
    sprintf (buf, TEXT(WINDOW_TITLE), SNES9X_NAME_AND_VERSION);

    DWORD dwExStyle;
    DWORD dwStyle;
    RECT rect;

    rect.left = rect.top = 0;
    rect.right = MAX_SNES_WIDTH;
    rect.bottom = MAX_SNES_HEIGHT;
    dwExStyle = WS_EX_APPWINDOW | WS_EX_WINDOWEDGE;
    dwStyle = WS_OVERLAPPEDWINDOW & ~(GUI.windowResizeLocked?WS_THICKFRAME:0);

    AdjustWindowRectEx (&rect, dwStyle, FALSE, dwExStyle);
    if ((GUI.hWnd = CreateWindowEx (
        dwExStyle,
        TEXT("Snes9X: WndClass"),
        buf,
        WS_CLIPSIBLINGS |
        WS_CLIPCHILDREN |
        dwStyle,
        0, 0,
        rect.right - rect.left, rect.bottom - rect.top,
        NULL,
        GUI.hMenu,
        hInstance,
        NULL)) == NULL)
        return FALSE;

    GUI.hDC = GetDC (GUI.hWnd);
	LoadExts();
    GUI.GunSight = LoadCursor (hInstance, MAKEINTRESOURCE (IDC_CURSOR_SCOPE));
    GUI.Arrow = LoadCursor (NULL, IDC_ARROW);
    GUI.Accelerators = LoadAccelerators (hInstance, MAKEINTRESOURCE (IDR_SNES9X_ACCELERATORS));
    Settings.ForcedPause = 0;
    Settings.StopEmulation = TRUE;
    Settings.Paused = FALSE;

	GUI.WAVOut = NULL;
	GUI.AVIOut = NULL;

    return TRUE;
}



void S9xExtraUsage ()
{
}

typedef struct {
	bool wasPressed;
	DWORD firstPressedTime;
	DWORD lastPressedTime;
	WORD repeatCount;
} JoyState;

// handles key presses
VOID CALLBACK KeyInputTimer(UINT idEvent, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1, DWORD_PTR dw2)
{
	bool S9xGetState (WORD KeyIdent);

	static DWORD lastTime = timeGetTime();
	DWORD currentTime = timeGetTime();

	if(GUI.JoystickHotkeys)
	{
		static JoyState joyState [256];
		static bool joyStateInited = false;

		if (!joyStateInited) {
			for(int i = 0; i < 256; i++) {
				joyState[i].wasPressed = false;
				joyState[i].repeatCount = 1;
			}
			joyStateInited = true;
		}

		for (int i = 0; i < 255; i++) {
			bool active = !S9xGetState(0x8000|i);

			if (active) {
				bool keyRepeat = !joyState[i].wasPressed && (currentTime - joyState[i].firstPressedTime) >= (DWORD)KeyInDelayMSec;
				if (!joyState[i].wasPressed || keyRepeat) {
					if (!joyState[i].wasPressed)
						joyState[i].firstPressedTime = currentTime;
					joyState[i].lastPressedTime = currentTime;
					if (keyRepeat && joyState[i].repeatCount < 0xffff)
						joyState[i].repeatCount++;
					PostMessage(GUI.hWnd, WM_CUSTKEYDOWN, (WPARAM)(0x8000|i),(LPARAM)(joyState[i].repeatCount | (joyState[i].wasPressed ? 0x40000000 : 0)));
				}
			}
			else {
				joyState[i].repeatCount = 1;
				if (joyState[i].wasPressed)
					PostMessage(GUI.hWnd, WM_CUSTKEYUP, (WPARAM)(0x8000|i),(LPARAM)(joyState[i].repeatCount | (joyState[i].wasPressed ? 0x40000000 : 0)));
			}
			joyState[i].wasPressed = active;
		}
	}
	if((!GUI.InactivePause || !Settings.ForcedPause)
			|| (GUI.BackgroundInput || !(Settings.ForcedPause & (PAUSE_INACTIVE_WINDOW | PAUSE_WINDOW_ICONISED))))
	{
		static JoyState joyState [256];
		static bool joyStateInited = false;

		if (!joyStateInited) {
			for(int i = 0; i < 256; i++) {
				joyState[i].wasPressed = false;
				joyState[i].repeatCount = 1;
			}
			joyStateInited = true;
		}

		for (int i = 0; i < 256; i++) {
			bool active = !S9xGetState(i);

			if (active) {
				bool keyRepeat = (currentTime - joyState[i].firstPressedTime) >= (DWORD)KeyInDelayMSec;
				if (!joyState[i].wasPressed || keyRepeat) {
					if (!joyState[i].wasPressed)
						joyState[i].firstPressedTime = currentTime;
					joyState[i].lastPressedTime = currentTime;
					if (keyRepeat && joyState[i].repeatCount < 0xffff)
						joyState[i].repeatCount++;
					PostMessage(GUI.hWnd, WM_CUSTKEYDOWN, (WPARAM)(i),(LPARAM)(joyState[i].repeatCount | (joyState[i].wasPressed ? 0x40000000 : 0)));
				}
			}
			else {
				joyState[i].repeatCount = 1;
				if (joyState[i].wasPressed)
					PostMessage(GUI.hWnd, WM_CUSTKEYUP, (WPARAM)(i),(LPARAM)(joyState[i].repeatCount | (joyState[i].wasPressed ? 0x40000000 : 0)));
			}
			joyState[i].wasPressed = active;
		}
	}
	lastTime = currentTime;
}

VOID CALLBACK FrameTimer( UINT idEvent, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1, DWORD_PTR dw2)
{
	// QueryPerformanceCounter is unreliable on newfangled frequency-switching computers,
	// yet is absolutely necessary for best performance on somewhat older computers (even ones that are capable of frequency switching but don't do it very often).
	// Thus, we keep two timers and use the QueryPerformanceCounter one unless the other (more accurate but less precise)
	// one differs from it by more than a few milliseconds.

    QueryPerformanceCounter((LARGE_INTEGER*)&PCEnd);
	PCEndTicks = timeGetTime()*1000;

	const __int64 PCElapsedPrecise = PCEnd - PCStart;
	const __int64 PCElapsedAccurate = (__int64)(PCEndTicks - PCStartTicks) * PCBase / 1000000;
	const bool useTicksTimer = (abs((int)(PCElapsedPrecise - PCElapsedAccurate)) > (PCBase >> 7)); // if > 7.8 ms difference, settle for accuracy at the sacrifice of precision

    while ((!useTicksTimer && (PCEnd      - PCStart     ) >= PCFrameTime) ||
		   ( useTicksTimer && (PCEndTicks - PCStartTicks) >= PCFrameTime * 1000000 / PCBase))
	{
        if (GUI.FrameCount == GUI.LastFrameCount)
            GUI.IdleCount++;
        else
        {
            GUI.IdleCount = 0;
            GUI.LastFrameCount = GUI.FrameCount;
        }

#ifdef NETPLAY_SUPPORT
		//    if (Settings.NetPlay && !Settings.NetPlayServer)
		//        return;
        if (Settings.NetPlay && !Settings.NetPlayServer)
            return;

		//-    if (Settings.NetPlayServer)
		//-    {
		//-        if (Settings.Paused || Settings.StopEmulation || Settings.ForcedPause)
        if (Settings.NetPlayServer)
		{
			//-            WaitForSingleObject (GUI.ServerTimerSemaphore, 0);
            if ((Settings.Paused && !Settings.FrameAdvance) || Settings.StopEmulation || Settings.ForcedPause)
            {
                WaitForSingleObject (GUI.ServerTimerSemaphore, 0);
                return;
            }
            ReleaseSemaphore (GUI.ServerTimerSemaphore, 1, NULL);

            if (Settings.NetPlay)
                return;
        }
        else
#endif
		{
			if (Settings.SkipFrames != AUTO_FRAMERATE || Settings.TurboMode ||
				(Settings.Paused /*&& !Settings.FrameAdvance*/) || Settings.StopEmulation || Settings.ForcedPause)
			{
				WaitForSingleObject (GUI.FrameTimerSemaphore, 0);
				PCStart = PCEnd;
				PCStartTicks = PCEndTicks;
				return;
			}
			//        ReleaseSemaphore (GUI.ServerTimerSemaphore, 1, NULL);
			ReleaseSemaphore (GUI.FrameTimerSemaphore, 1, NULL);

			//        if (Settings.NetPlay)
			//            return;
			//    }
			//    else
			//#endif
			//    if (Settings.SkipFrames != AUTO_FRAMERATE || Settings.TurboMode ||
			//        Settings.Paused || Settings.StopEmulation || Settings.ForcedPause)
			//    {
			//        WaitForSingleObject (GUI.FrameTimerSemaphore, 0);
			//        return;
			//    }
			//    ReleaseSemaphore (GUI.FrameTimerSemaphore, 1, NULL);
			PCStart += PCFrameTime;
			PCStartTicks += (DWORD)(PCFrameTime * 1000000 / PCBase);
		}
	}
}

static void EnsureInputDisplayUpdated()
{
	if(GUI.FrameAdvanceJustPressed==1 && Settings.Paused && Settings.DisplayPressedKeys==2 && GUI.ControllerOption != SNES_JOYPAD && GUI.ControllerOption != SNES_MULTIPLAYER5 && GUI.ControllerOption != SNES_MULTIPLAYER8)
		S9xReRefresh();
}

// for "frame advance skips non-input frames" feature
void S9xOnSNESPadRead()
{
	if(!GUI.FASkipsNonInput)
		return;

	if(prevPadReadFrame != Timings.TotalEmulatedFrames) // we want <= 1 calls per frame
	{
		prevPadReadFrame = Timings.TotalEmulatedFrames;

		if(Settings.FrameAdvance && Settings.Paused && !skipNextFrameStop)
		{
			Settings.FrameAdvance = false;
			ICPU.SavedAtOp = TRUE;

			EnsureInputDisplayUpdated();

			// wait until either unpause or next frame advance
			// note: using GUI.hWnd instead of NULL for PeekMessage/GetMessage breaks some non-modal dialogs
			MSG msg;
			while (Settings.Paused && !Settings.FrameAdvance)
			{
				if (!GetMessage (&msg, NULL, 0, 0))
				{
					PostMessage(GUI.hWnd, WM_QUIT, 0,0);
					return;
				}

				// do not process non-modal dialog messages
				if ((oldRamSearchHWND && IsDialogMessage(oldRamSearchHWND, &msg))
				 || (inputMacroHWND && IsDialogMessage(inputMacroHWND, &msg))
				 || (RamSearchHWnd && IsDialogMessage(RamSearchHWnd, &msg))
				 || (LuaConsoleHWnd && IsDialogMessage(LuaConsoleHWnd, &msg)))
					continue;

				if (RamWatchHWnd && IsDialogMessage(RamWatchHWnd, &msg)) {
					if(msg.message == WM_KEYDOWN) // send keydown messages to the dialog (for accelerators, and also needed for the Alt key to work)
						SendMessage(RamWatchHWnd, msg.message, msg.wParam, msg.lParam);
					continue;
				}

				if (!TranslateAccelerator (GUI.hWnd, GUI.Accelerators, &msg))
				{
					TranslateMessage (&msg);
					DispatchMessage (&msg);
				}
			}

			ICPU.SavedAtOp = !ICPU.SavedAtOp;
		}
		else
		{
			skipNextFrameStop = false;
		}
	}
}


enum
{
	k_HD = 0x80000000,

	k_JP = 0x01000000,
	k_MO = 0x02000000,
	k_SS = 0x04000000,
	k_LG = 0x08000000,

	k_BT = 0x00100000,
	k_PT = 0x00200000,
	k_PS = 0x00400000,

	k_C1 = 0x00000100,
	k_C2 = 0x00000200,
	k_C3 = 0x00000400,
	k_C4 = 0x00000800,
	k_C5 = 0x00001000,
	k_C6 = 0x00002000,
	k_C7 = 0x00004000,
	k_C8 = 0x00008000
};

enum
{
	kWinCMapPad1PX            = k_HD | k_BT | k_JP | k_C1,
	kWinCMapPad1PA,
	kWinCMapPad1PB,
	kWinCMapPad1PY,
	kWinCMapPad1PL,
	kWinCMapPad1PR,
	kWinCMapPad1PSelect,
	kWinCMapPad1PStart,
	kWinCMapPad1PUp,
	kWinCMapPad1PDown,
	kWinCMapPad1PLeft,
	kWinCMapPad1PRight,

	kWinCMapPad2PX            = k_HD | k_BT | k_JP | k_C2,
	kWinCMapPad2PA,
	kWinCMapPad2PB,
	kWinCMapPad2PY,
	kWinCMapPad2PL,
	kWinCMapPad2PR,
	kWinCMapPad2PSelect,
	kWinCMapPad2PStart,
	kWinCMapPad2PUp,
	kWinCMapPad2PDown,
	kWinCMapPad2PLeft,
	kWinCMapPad2PRight,

	kWinCMapPad3PX            = k_HD | k_BT | k_JP | k_C3,
	kWinCMapPad3PA,
	kWinCMapPad3PB,
	kWinCMapPad3PY,
	kWinCMapPad3PL,
	kWinCMapPad3PR,
	kWinCMapPad3PSelect,
	kWinCMapPad3PStart,
	kWinCMapPad3PUp,
	kWinCMapPad3PDown,
	kWinCMapPad3PLeft,
	kWinCMapPad3PRight,

	kWinCMapPad4PX            = k_HD | k_BT | k_JP | k_C4,
	kWinCMapPad4PA,
	kWinCMapPad4PB,
	kWinCMapPad4PY,
	kWinCMapPad4PL,
	kWinCMapPad4PR,
	kWinCMapPad4PSelect,
	kWinCMapPad4PStart,
	kWinCMapPad4PUp,
	kWinCMapPad4PDown,
	kWinCMapPad4PLeft,
	kWinCMapPad4PRight,

	kWinCMapPad5PX            = k_HD | k_BT | k_JP | k_C5,
	kWinCMapPad5PA,
	kWinCMapPad5PB,
	kWinCMapPad5PY,
	kWinCMapPad5PL,
	kWinCMapPad5PR,
	kWinCMapPad5PSelect,
	kWinCMapPad5PStart,
	kWinCMapPad5PUp,
	kWinCMapPad5PDown,
	kWinCMapPad5PLeft,
	kWinCMapPad5PRight,

	kWinCMapPad6PX            = k_HD | k_BT | k_JP | k_C6,
	kWinCMapPad6PA,
	kWinCMapPad6PB,
	kWinCMapPad6PY,
	kWinCMapPad6PL,
	kWinCMapPad6PR,
	kWinCMapPad6PSelect,
	kWinCMapPad6PStart,
	kWinCMapPad6PUp,
	kWinCMapPad6PDown,
	kWinCMapPad6PLeft,
	kWinCMapPad6PRight,

	kWinCMapPad7PX            = k_HD | k_BT | k_JP | k_C7,
	kWinCMapPad7PA,
	kWinCMapPad7PB,
	kWinCMapPad7PY,
	kWinCMapPad7PL,
	kWinCMapPad7PR,
	kWinCMapPad7PSelect,
	kWinCMapPad7PStart,
	kWinCMapPad7PUp,
	kWinCMapPad7PDown,
	kWinCMapPad7PLeft,
	kWinCMapPad7PRight,

	kWinCMapPad8PX            = k_HD | k_BT | k_JP | k_C8,
	kWinCMapPad8PA,
	kWinCMapPad8PB,
	kWinCMapPad8PY,
	kWinCMapPad8PL,
	kWinCMapPad8PR,
	kWinCMapPad8PSelect,
	kWinCMapPad8PStart,
	kWinCMapPad8PUp,
	kWinCMapPad8PDown,
	kWinCMapPad8PLeft,
	kWinCMapPad8PRight,

	kWinCMapMouse1PL          = k_HD | k_BT | k_MO | k_C1,
	kWinCMapMouse1PR,
	kWinCMapMouse2PL          = k_HD | k_BT | k_MO | k_C2,
	kWinCMapMouse2PR,

	kWinCMapScopeOffscreen    = k_HD | k_BT | k_SS | k_C1,
	kWinCMapScopeFire,
	kWinCMapScopeCursor,
	kWinCMapScopeTurbo,
	kWinCMapScopePause,

	kWinCMapLGun1Offscreen    = k_HD | k_BT | k_LG | k_C1,
	kWinCMapLGun1Trigger,
	kWinCMapLGun1Start,
	kWinCMapLGun2Offscreen    = k_HD | k_BT | k_LG | k_C2,
	kWinCMapLGun2Trigger,
	kWinCMapLGun2Start,

	kWinCMapMouse1Pointer     = k_HD | k_PT | k_MO | k_C1,
	kWinCMapMouse2Pointer     = k_HD | k_PT | k_MO | k_C2,
	kWinCMapSuperscopePointer = k_HD | k_PT | k_SS | k_C1,
	kWinCMapJustifier1Pointer = k_HD | k_PT | k_LG | k_C1,

	kWinCMapPseudoPtrBase     = k_HD | k_PS | k_LG | k_C2	// for Justifier 2P
};



#define	ASSIGN_BUTTONf(n, s)	S9xMapButton (n, cmd = S9xGetCommandT(s), false)
#define	ASSIGN_BUTTONt(n, s)	S9xMapButton (n, cmd = S9xGetCommandT(s), true)
#define	ASSIGN_POINTRf(n, s)	S9xMapPointer(n, cmd = S9xGetCommandT(s), false)
#define	ASSIGN_POINTRt(n, s)	S9xMapPointer(n, cmd = S9xGetCommandT(s), true)

#define KeyIsPressed(km, k)		(1 & (((unsigned char *) km) [(k) >> 3] >> ((k) & 7)))

void S9xSetupDefaultKeymap(void)
{
	s9xcommand_t	cmd;

	ASSIGN_BUTTONf(kWinCMapPad1PX,         "Joypad1 X");
	ASSIGN_BUTTONf(kWinCMapPad1PA,         "Joypad1 A");
	ASSIGN_BUTTONf(kWinCMapPad1PB,         "Joypad1 B");
	ASSIGN_BUTTONf(kWinCMapPad1PY,         "Joypad1 Y");
	ASSIGN_BUTTONf(kWinCMapPad1PL,         "Joypad1 L");
	ASSIGN_BUTTONf(kWinCMapPad1PR,         "Joypad1 R");
	ASSIGN_BUTTONf(kWinCMapPad1PSelect,    "Joypad1 Select");
	ASSIGN_BUTTONf(kWinCMapPad1PStart,     "Joypad1 Start");
	ASSIGN_BUTTONf(kWinCMapPad1PUp,        "Joypad1 Up");
	ASSIGN_BUTTONf(kWinCMapPad1PDown,      "Joypad1 Down");
	ASSIGN_BUTTONf(kWinCMapPad1PLeft,      "Joypad1 Left");
	ASSIGN_BUTTONf(kWinCMapPad1PRight,     "Joypad1 Right");

	ASSIGN_BUTTONf(kWinCMapPad2PX,         "Joypad2 X");
	ASSIGN_BUTTONf(kWinCMapPad2PA,         "Joypad2 A");
	ASSIGN_BUTTONf(kWinCMapPad2PB,         "Joypad2 B");
	ASSIGN_BUTTONf(kWinCMapPad2PY,         "Joypad2 Y");
	ASSIGN_BUTTONf(kWinCMapPad2PL,         "Joypad2 L");
	ASSIGN_BUTTONf(kWinCMapPad2PR,         "Joypad2 R");
	ASSIGN_BUTTONf(kWinCMapPad2PSelect,    "Joypad2 Select");
	ASSIGN_BUTTONf(kWinCMapPad2PStart,     "Joypad2 Start");
	ASSIGN_BUTTONf(kWinCMapPad2PUp,        "Joypad2 Up");
	ASSIGN_BUTTONf(kWinCMapPad2PDown,      "Joypad2 Down");
	ASSIGN_BUTTONf(kWinCMapPad2PLeft,      "Joypad2 Left");
	ASSIGN_BUTTONf(kWinCMapPad2PRight,     "Joypad2 Right");

	ASSIGN_BUTTONf(kWinCMapPad3PX,         "Joypad3 X");
	ASSIGN_BUTTONf(kWinCMapPad3PA,         "Joypad3 A");
	ASSIGN_BUTTONf(kWinCMapPad3PB,         "Joypad3 B");
	ASSIGN_BUTTONf(kWinCMapPad3PY,         "Joypad3 Y");
	ASSIGN_BUTTONf(kWinCMapPad3PL,         "Joypad3 L");
	ASSIGN_BUTTONf(kWinCMapPad3PR,         "Joypad3 R");
	ASSIGN_BUTTONf(kWinCMapPad3PSelect,    "Joypad3 Select");
	ASSIGN_BUTTONf(kWinCMapPad3PStart,     "Joypad3 Start");
	ASSIGN_BUTTONf(kWinCMapPad3PUp,        "Joypad3 Up");
	ASSIGN_BUTTONf(kWinCMapPad3PDown,      "Joypad3 Down");
	ASSIGN_BUTTONf(kWinCMapPad3PLeft,      "Joypad3 Left");
	ASSIGN_BUTTONf(kWinCMapPad3PRight,     "Joypad3 Right");

	ASSIGN_BUTTONf(kWinCMapPad4PX,         "Joypad4 X");
	ASSIGN_BUTTONf(kWinCMapPad4PA,         "Joypad4 A");
	ASSIGN_BUTTONf(kWinCMapPad4PB,         "Joypad4 B");
	ASSIGN_BUTTONf(kWinCMapPad4PY,         "Joypad4 Y");
	ASSIGN_BUTTONf(kWinCMapPad4PL,         "Joypad4 L");
	ASSIGN_BUTTONf(kWinCMapPad4PR,         "Joypad4 R");
	ASSIGN_BUTTONf(kWinCMapPad4PSelect,    "Joypad4 Select");
	ASSIGN_BUTTONf(kWinCMapPad4PStart,     "Joypad4 Start");
	ASSIGN_BUTTONf(kWinCMapPad4PUp,        "Joypad4 Up");
	ASSIGN_BUTTONf(kWinCMapPad4PDown,      "Joypad4 Down");
	ASSIGN_BUTTONf(kWinCMapPad4PLeft,      "Joypad4 Left");
	ASSIGN_BUTTONf(kWinCMapPad4PRight,     "Joypad4 Right");

	ASSIGN_BUTTONf(kWinCMapPad5PX,         "Joypad5 X");
	ASSIGN_BUTTONf(kWinCMapPad5PA,         "Joypad5 A");
	ASSIGN_BUTTONf(kWinCMapPad5PB,         "Joypad5 B");
	ASSIGN_BUTTONf(kWinCMapPad5PY,         "Joypad5 Y");
	ASSIGN_BUTTONf(kWinCMapPad5PL,         "Joypad5 L");
	ASSIGN_BUTTONf(kWinCMapPad5PR,         "Joypad5 R");
	ASSIGN_BUTTONf(kWinCMapPad5PSelect,    "Joypad5 Select");
	ASSIGN_BUTTONf(kWinCMapPad5PStart,     "Joypad5 Start");
	ASSIGN_BUTTONf(kWinCMapPad5PUp,        "Joypad5 Up");
	ASSIGN_BUTTONf(kWinCMapPad5PDown,      "Joypad5 Down");
	ASSIGN_BUTTONf(kWinCMapPad5PLeft,      "Joypad5 Left");
	ASSIGN_BUTTONf(kWinCMapPad5PRight,     "Joypad5 Right");

	ASSIGN_BUTTONf(kWinCMapPad6PX,         "Joypad6 X");
	ASSIGN_BUTTONf(kWinCMapPad6PA,         "Joypad6 A");
	ASSIGN_BUTTONf(kWinCMapPad6PB,         "Joypad6 B");
	ASSIGN_BUTTONf(kWinCMapPad6PY,         "Joypad6 Y");
	ASSIGN_BUTTONf(kWinCMapPad6PL,         "Joypad6 L");
	ASSIGN_BUTTONf(kWinCMapPad6PR,         "Joypad6 R");
	ASSIGN_BUTTONf(kWinCMapPad6PSelect,    "Joypad6 Select");
	ASSIGN_BUTTONf(kWinCMapPad6PStart,     "Joypad6 Start");
	ASSIGN_BUTTONf(kWinCMapPad6PUp,        "Joypad6 Up");
	ASSIGN_BUTTONf(kWinCMapPad6PDown,      "Joypad6 Down");
	ASSIGN_BUTTONf(kWinCMapPad6PLeft,      "Joypad6 Left");
	ASSIGN_BUTTONf(kWinCMapPad6PRight,     "Joypad6 Right");

	ASSIGN_BUTTONf(kWinCMapPad7PX,         "Joypad7 X");
	ASSIGN_BUTTONf(kWinCMapPad7PA,         "Joypad7 A");
	ASSIGN_BUTTONf(kWinCMapPad7PB,         "Joypad7 B");
	ASSIGN_BUTTONf(kWinCMapPad7PY,         "Joypad7 Y");
	ASSIGN_BUTTONf(kWinCMapPad7PL,         "Joypad7 L");
	ASSIGN_BUTTONf(kWinCMapPad7PR,         "Joypad7 R");
	ASSIGN_BUTTONf(kWinCMapPad7PSelect,    "Joypad7 Select");
	ASSIGN_BUTTONf(kWinCMapPad7PStart,     "Joypad7 Start");
	ASSIGN_BUTTONf(kWinCMapPad7PUp,        "Joypad7 Up");
	ASSIGN_BUTTONf(kWinCMapPad7PDown,      "Joypad7 Down");
	ASSIGN_BUTTONf(kWinCMapPad7PLeft,      "Joypad7 Left");
	ASSIGN_BUTTONf(kWinCMapPad7PRight,     "Joypad7 Right");

	ASSIGN_BUTTONf(kWinCMapPad8PX,         "Joypad8 X");
	ASSIGN_BUTTONf(kWinCMapPad8PA,         "Joypad8 A");
	ASSIGN_BUTTONf(kWinCMapPad8PB,         "Joypad8 B");
	ASSIGN_BUTTONf(kWinCMapPad8PY,         "Joypad8 Y");
	ASSIGN_BUTTONf(kWinCMapPad8PL,         "Joypad8 L");
	ASSIGN_BUTTONf(kWinCMapPad8PR,         "Joypad8 R");
	ASSIGN_BUTTONf(kWinCMapPad8PSelect,    "Joypad8 Select");
	ASSIGN_BUTTONf(kWinCMapPad8PStart,     "Joypad8 Start");
	ASSIGN_BUTTONf(kWinCMapPad8PUp,        "Joypad8 Up");
	ASSIGN_BUTTONf(kWinCMapPad8PDown,      "Joypad8 Down");
	ASSIGN_BUTTONf(kWinCMapPad8PLeft,      "Joypad8 Left");
	ASSIGN_BUTTONf(kWinCMapPad8PRight,     "Joypad8 Right");

	ASSIGN_BUTTONt(kWinCMapMouse1PL,       "Mouse1 L");
	ASSIGN_BUTTONt(kWinCMapMouse1PR,       "Mouse1 R");
	ASSIGN_BUTTONt(kWinCMapMouse2PL,       "Mouse2 L");
	ASSIGN_BUTTONt(kWinCMapMouse2PR,       "Mouse2 R");

	ASSIGN_BUTTONt(kWinCMapScopeOffscreen, "Superscope AimOffscreen");
	ASSIGN_BUTTONt(kWinCMapScopeFire,      "Superscope Fire");
	ASSIGN_BUTTONt(kWinCMapScopeCursor,    "Superscope Cursor");
	ASSIGN_BUTTONt(kWinCMapScopeTurbo,     "Superscope ToggleTurbo");
	ASSIGN_BUTTONt(kWinCMapScopePause,     "Superscope Pause");

	ASSIGN_BUTTONt(kWinCMapLGun1Offscreen, "Justifier1 AimOffscreen");
	ASSIGN_BUTTONt(kWinCMapLGun1Trigger,   "Justifier1 Trigger");
	ASSIGN_BUTTONt(kWinCMapLGun1Start,     "Justifier1 Start");
	ASSIGN_BUTTONt(kWinCMapLGun2Offscreen, "Justifier2 AimOffscreen");
	ASSIGN_BUTTONt(kWinCMapLGun2Trigger,   "Justifier2 Trigger");
	ASSIGN_BUTTONt(kWinCMapLGun2Start,     "Justifier2 Start");

	ASSIGN_POINTRt(kWinCMapMouse1Pointer,     "Pointer Mouse1");
	ASSIGN_POINTRt(kWinCMapMouse2Pointer,     "Pointer Mouse2");
	ASSIGN_POINTRt(kWinCMapSuperscopePointer, "Pointer Superscope");
	ASSIGN_POINTRt(kWinCMapJustifier1Pointer, "Pointer Justifier1");

	ASSIGN_POINTRf(PseudoPointerBase,         "Pointer Justifier2");
	ASSIGN_BUTTONf(kWinCMapPseudoPtrBase + 0, "ButtonToPointer 1u Med");
	ASSIGN_BUTTONf(kWinCMapPseudoPtrBase + 1, "ButtonToPointer 1d Med");
	ASSIGN_BUTTONf(kWinCMapPseudoPtrBase + 2, "ButtonToPointer 1l Med");
	ASSIGN_BUTTONf(kWinCMapPseudoPtrBase + 3, "ButtonToPointer 1r Med");
}

void ControlPadFlagsToS9xReportButtons(int n, uint32 p)
{
	uint32	base = k_HD | k_BT | k_JP | (0x100 << n);

	S9xReportButton(base +  0, (p & 0x0040) != 0);
	S9xReportButton(base +  1, (p & 0x0080) != 0);
	S9xReportButton(base +  2, (p & 0x8000) != 0);
	S9xReportButton(base +  3, (p & 0x4000) != 0);
	S9xReportButton(base +  4, (p & 0x0020) != 0);
	S9xReportButton(base +  5, (p & 0x0010) != 0);
	S9xReportButton(base +  6, (p & 0x2000) != 0);
	S9xReportButton(base +  7, (p & 0x1000) != 0);
	S9xReportButton(base +  8, (p & 0x0800) != 0);
	S9xReportButton(base +  9, (p & 0x0400) != 0);
	S9xReportButton(base + 10, (p & 0x0200) != 0);
	S9xReportButton(base + 11, (p & 0x0100) != 0);
}

void ControlPadFlagsToS9xPseudoPointer(uint32 p)
{
	// prevent screwiness caused by trying to move the pointer left+right or up+down
	if((p & 0x0c00) == 0x0c00) p &= ~0x0c00;
	if((p & 0x0300) == 0x0300) p &= ~0x0300;

	// checks added to prevent a lack of right/down movement from breaking left/up movement
	if(!(p & 0x0400))
		S9xReportButton(kWinCMapPseudoPtrBase + 0, (p & 0x0800) != 0);
	if(!(p & 0x0800))
		S9xReportButton(kWinCMapPseudoPtrBase + 1, (p & 0x0400) != 0);
	if(!(p & 0x0100))
		S9xReportButton(kWinCMapPseudoPtrBase + 2, (p & 0x0200) != 0);
	if(!(p & 0x0200))
		S9xReportButton(kWinCMapPseudoPtrBase + 3, (p & 0x0100) != 0);
}

static void ProcessInput(void)
{
	extern void S9xWinScanJoypads ();	
#ifdef NETPLAY_SUPPORT
    if (!Settings.NetPlay)
#endif
		S9xWinScanJoypads ();

	extern uint32 joypads [8];
	for(int i = 0 ; i < 8 ; i++)
		ControlPadFlagsToS9xReportButtons(i, joypads[i]);

	if (GUI.ControllerOption==SNES_JUSTIFIER_2)
		ControlPadFlagsToS9xPseudoPointer(joypads[1]);
}

static void WinDisplayString (const char *string, int lines, bool linesFromBottom, int pixelsFromLeft, bool allowWrap);




/*****************************************************************************/
/* WinMain                                                                   */
/*****************************************************************************/
extern "C" void S9xMainLoop(void);
int Init3d (HWND);
extern "C" void DeinitS9x(void);
int WINAPI WinMain(
				   HINSTANCE hInstance,
				   HINSTANCE hPrevInstance,
				   LPSTR lpCmdLine,
				   int nCmdShow)
{
	Settings.StopEmulation = TRUE;

	// Redirect stderr and stdout to file. It wouldn't go to any commandline anyway.
	FILE* fout = freopen("stdout.txt", "w", stdout);
	if(fout) setvbuf(fout, NULL, _IONBF, 0);
	FILE* ferr = freopen("stderr.txt", "w", stderr);
	if(ferr) setvbuf(ferr, NULL, _IONBF, 0);

	InitializeCriticalSection(&GUI.SoundCritSect);

	DWORD wmTimerRes;
	TIMECAPS tc;
	if (timeGetDevCaps(&tc, sizeof(TIMECAPS))== TIMERR_NOERROR)
	{
#ifdef __MINGW32__
		wmTimerRes = min<int>(max<int>(tc.wPeriodMin, 1), tc.wPeriodMax);
#else
		wmTimerRes = min(max(tc.wPeriodMin, 1), tc.wPeriodMax);
#endif
		timeBeginPeriod (wmTimerRes);
	}
	else
	{
		wmTimerRes = 5;
		timeBeginPeriod (wmTimerRes);
	}

	InitCustomKeys(&CustomKeys); // must be called before WinRegisterConfigItems
	WinRegisterConfigItems ();

	ConfigFile::SetAlphaSort(false);
	ConfigFile::SetTimeSort(false);
    rom_filename = WinParseCommandLineAndLoadConfigFile (lpCmdLine);
    WinSaveConfigFile ();
	WinLockConfigFile ();

	bool maximized = GUI.window_maximized;

    WinInit (hInstance);
	if(GUI.HideMenu)
	{
		SetMenu (GUI.hWnd, NULL);
	}

#ifdef USE_GLIDE
    if (VOODOO_MODE)
    {
        if (!S9xVoodooInitialise ())
        {
            GUI.Scale = 0;
            GUI.NextScale = 0;
        }
        else
        {
            GUI.FullScreen = FALSE;
            Settings.SixteenBit = TRUE;
        }
    }
#endif

#ifdef USE_OPENGL
	OpenGL.initialized = false;
    if (OPENGL_MODE)
    {
        if (!S9xOpenGLInit ())
        {
            GUI.Scale = FILTER_NONE;
            GUI.NextScale = FILTER_NONE;
        }
    }
#endif

	extern void InitLUTs(); // init hq2x
	InitLUTs();

	S9xCustomDisplayString = WinDisplayString;

    if (!VOODOO_MODE && !OPENGL_MODE &&
		(GUI.outputMethod==DIRECT3D)?!Direct3D.initialize(GUI.hWnd):!DirectDraw.InitDirectDraw()
		)
    {
        MessageBox (GUI.hWnd, Languages[ GUI.Language].errInitDD, TEXT("Snes9X - DirectX"), MB_OK | MB_ICONSTOP);
        return false;
    }
	if(GUI.outputMethod==DIRECT3D)
		DirectDraw.Clipped = true;

    if (!GUI.FullScreen)
    {
        MoveWindow (GUI.hWnd, GUI.window_size.left,
			GUI.window_size.top,
			GUI.window_size.right - GUI.window_size.left,
			GUI.window_size.bottom - GUI.window_size.top, TRUE);
    }


    if ((GUI.outputMethod==DIRECTDRAW) && !VOODOO_MODE && !OPENGL_MODE &&
        !DirectDraw.SetDisplayMode (GUI.Width, GUI.Height, max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes)), GUI.Depth, GUI.RefreshRate,
		!GUI.FullScreen, GUI.tripleBuffering))
    {
        MessageBox( GUI.hWnd, Languages[ GUI.Language].errModeDD, TEXT("Snes9X - DirectDraw(7)"), MB_OK | MB_ICONSTOP);
        GUI.FullScreen = FALSE;
        if (!DirectDraw.SetDisplayMode (GUI.Width, GUI.Height, max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes)), GUI.Depth, GUI.RefreshRate,
			!GUI.FullScreen, GUI.tripleBuffering))
            return (false);
    }

    if (!GUI.FullScreen)
    {
        RECT rect;
        GetClientRect (GUI.hWnd, &rect);
        InvalidateRect (GUI.hWnd, &rect, true);
    }

    GUI.ControlForced = 0xff;

    S9xSetFilters ();
    S9xSetRecentGames ();
	ShowWindow (GUI.hWnd, maximized ? SW_MAXIMIZE : SW_SHOWNORMAL);
    /*SetForegroundWindow (GUI.hWnd);
    SetFocus (GUI.hWnd);*/

	// hack for borders-not-shown bug (Windows bug?) on startup
	if(!maximized)
	{
		ShowWindow (GUI.hWnd, SW_HIDE);
		ShowWindow (GUI.hWnd, SW_SHOWNORMAL);
	}

	if((GUI.outputMethod==DIRECT3D) && GUI.FullScreen) {
		GUI.FullScreen = false;
		ToggleFullScreen();
	}

    void InitSnes9X (void);
    InitSnes9X ();

    QueryPerformanceFrequency((LARGE_INTEGER*)&PCBase);
    QueryPerformanceCounter((LARGE_INTEGER*)&PCStart);
	PCEnd = PCStart;
	PCEndTicks = timeGetTime()*1000;
	PCStartTicks = timeGetTime()*1000;
    PCFrameTime = PCFrameTimeNTSC = (__int64)((float)PCBase / 59.948743718592964824120603015098f);
    PCFrameTimePAL = PCBase / 50;

	KeyInDelayMSec = GUI.KeyInDelayMSec;
	KeyInRepeatMSec = GUI.KeyInRepeatMSec;
	if (KeyInDelayMSec == 0) {
		DWORD dwKeyboardDelay;
		SystemParametersInfo(SPI_GETKEYBOARDDELAY, 0, &dwKeyboardDelay, 0);
		KeyInDelayMSec = 250 * (dwKeyboardDelay + 1);
	}
	if (KeyInRepeatMSec == 0) {
		DWORD dwKeyboardSpeed;
		SystemParametersInfo(SPI_GETKEYBOARDSPEED, 0, &dwKeyboardSpeed, 0);
		KeyInRepeatMSec = (int)(1000.0/(((30.0-2.5)/31.0)*dwKeyboardSpeed+2.5));
	}
	if (KeyInRepeatMSec < (int)wmTimerRes)
		KeyInRepeatMSec = (int)wmTimerRes;
	if (KeyInDelayMSec < KeyInRepeatMSec)
		KeyInDelayMSec = KeyInRepeatMSec;

    Settings.StopEmulation = TRUE;
    GUI.hFrameTimer = timeSetEvent (20, 0, FrameTimer, 0, TIME_PERIODIC);
	GUI.hKeyInputTimer = timeSetEvent (KeyInRepeatMSec, 0, KeyInputTimer, 0, TIME_PERIODIC);

    GUI.FrameTimerSemaphore = CreateSemaphore (NULL, 0, 10, NULL);
    GUI.ServerTimerSemaphore = CreateSemaphore (NULL, 0, 10, NULL);

    if (GUI.hFrameTimer == 0)
    {
        MessageBox( GUI.hWnd, Languages[ GUI.Language].errFrameTimer, TEXT("Snes9X - Frame Timer"), MB_OK | MB_ICONINFORMATION);
    }

    Settings.StopEmulation = !LoadROM (rom_filename);

    if (!Settings.StopEmulation)
    {
		bool8 loaded = Memory.LoadSRAM (S9xGetFilename (".srm", SRAM_DIR));
		if(!loaded) // help migration from earlier Snes9x versions by checking ROM directory for savestates
			Memory.LoadSRAM (S9xGetFilename (".srm", ROMFILENAME_DIR));
        S9xLoadCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
        CheckDirectoryIsWritable (S9xGetFilename (".---", SNAPSHOT_DIR));
        CheckMenuStates ();
    }

    if (!Settings.StopEmulation)
    {
        if (GUI.ControllerOption == SNES_SUPERSCOPE)
            SetCursor (GUI.GunSight);
        else
        {
            SetCursor (GUI.Arrow);
            GUI.CursorTimer = 60;
        }
    }

	S9xUnmapAllControls();
	S9xSetupDefaultKeymap();
	ChangeInputDevice();

	DWORD lastTime = timeGetTime();

	MSG msg;
	
	while (TRUE)
	{
		EnsureInputDisplayUpdated();

		DispatchMessagesInQueue();

		// note: using GUI.hWnd instead of NULL for PeekMessage/GetMessage breaks some non-modal dialogs
		while (Settings.StopEmulation || (Settings.Paused && !Settings.FrameAdvance) ||
			Settings.ForcedPause ||
			PeekMessage (&msg, NULL, 0, 0, PM_NOREMOVE))
		{
			if (!GetMessage (&msg, NULL, 0, 0))
				goto loop_exit; // got WM_QUIT

			// do not process non-modal dialog messages
			if ((oldRamSearchHWND && IsDialogMessage(oldRamSearchHWND, &msg))
			 || (inputMacroHWND && IsDialogMessage(inputMacroHWND, &msg))
			 || (RamSearchHWnd && IsDialogMessage(RamSearchHWnd, &msg))
			 || (LuaConsoleHWnd && IsDialogMessage(LuaConsoleHWnd, &msg)))
				continue;

			if (RamWatchHWnd && IsDialogMessage(RamWatchHWnd, &msg)) {
				if(msg.message == WM_KEYDOWN) // send keydown messages to the dialog (for accelerators, and also needed for the Alt key to work)
					SendMessage(RamWatchHWnd, msg.message, msg.wParam, msg.lParam);
				continue;
			}

			if (!TranslateAccelerator (GUI.hWnd, GUI.Accelerators, &msg))
			{
				TranslateMessage (&msg);
				DispatchMessage (&msg);
			}
		}

#ifdef NETPLAY_SUPPORT
		if (!Settings.NetPlay || !NetPlay.PendingWait4Sync ||
			WaitForSingleObject (GUI.ClientSemaphore, 100) != WAIT_TIMEOUT)
		{
			if (NetPlay.PendingWait4Sync)
			{
				NetPlay.PendingWait4Sync = FALSE;
				NetPlay.FrameCount++;
				S9xNPStepJoypadHistory ();
			}
#endif
			// OldWatch: update delays in 1 frame
			Update_Old_RAM_Watch();

			if(oldRamSearchHWND)
			{
				if(timeGetTime() - lastTime >= 100)
				{
					SendMessage(oldRamSearchHWND, WM_COMMAND, (WPARAM)(IDC_REFRESHLIST),(LPARAM)(NULL));
					lastTime = timeGetTime();
				}
			}

			// the following is a hack to allow frametimes greater than 100ms,
			// without affecting the responsiveness of the GUI
			BOOL run_loop=false;
			do_frame_adjust=false;

			if (Settings.HighSpeedSeek > 0)
			{
				if (--Settings.HighSpeedSeek == 0)
				{
					Settings.Paused = true;
					IPPU.RenderThisFrame = true;
				}
				run_loop=true;
			}
			else if (Settings.TurboMode || Settings.FrameAdvance || Settings.SkipFrames != AUTO_FRAMERATE  || S9xLuaSpeed() > 0
#ifdef NETPLAY_SUPPORT
			|| Settings.NetPlay
#endif
			)
			{
				run_loop=true;
			}
			else
			{
				LONG prev;
				BOOL success;
				if ((success = ReleaseSemaphore (GUI.FrameTimerSemaphore, 1, &prev)) &&
					prev == 0)
				{
					WaitForSingleObject (GUI.FrameTimerSemaphore, 0);
					if (WaitForSingleObject (GUI.FrameTimerSemaphore, 100) == WAIT_OBJECT_0)
					{
						run_loop=true;
					}
				}
				else
				{
					if (success)
						WaitForSingleObject (GUI.FrameTimerSemaphore, 0);
					WaitForSingleObject (GUI.FrameTimerSemaphore, 0);

					run_loop=true;
					do_frame_adjust=true;
				}
			}


			if(Settings.FrameAdvance)
			{
				if(GFX.InfoStringTimeout > 4)
					GFX.InfoStringTimeout = 4;

				if(!GUI.FASkipsNonInput)
					Settings.FrameAdvance = false;
			}
			if(GUI.FrameAdvanceJustPressed)
				GUI.FrameAdvanceJustPressed--;

			ProcessInput(); // report input first for joypad.read()
			if (S9xLuaRunning())
			{
				S9xLuaFrameBoundary();
				if (S9xLuaSpeed() > 0) {
					run_loop = 1;
				}
			}

			if(run_loop)
			{
				//ProcessInput();

				S9xMainLoop();

				GUI.FrameCount++;

				Update_RAM_Search(); // Update_RAM_Watch() is also called.
			}

#ifdef NETPLAY_SUPPORT
		}
#endif
		if (CPU.Flags & DEBUG_MODE_FLAG)
		{
			Settings.Paused = TRUE;
			Settings.FrameAdvance = false;
			CPU.Flags &= ~DEBUG_MODE_FLAG;
		}
		if (GUI.CursorTimer)
		{
			if (--GUI.CursorTimer == 0)
			{
				if(!Settings.SuperScopeMaster)
					SetCursor (NULL);
			}
		}
	}

loop_exit:
	WarnIfDeferredMessagesExist();

	//stop any lua we might already have had running
	S9xLuaStop();

#ifdef USE_GLIDE
	S9xGlideEnable (FALSE);
#endif

	Settings.StopEmulation = TRUE;

    if (GUI.hKeyInputTimer)
        timeKillEvent (GUI.hKeyInputTimer);

    if( GUI.hFrameTimer)
    {
        timeKillEvent (GUI.hFrameTimer);
    }

	timeEndPeriod (wmTimerRes);

    if (!Settings.StopEmulation)
    {
        Memory.SaveSRAM (S9xGetFilename (".srm", SRAM_DIR));
        S9xSaveCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
    }
    //if (!VOODOO_MODE && !GUI.FullScreen)
    //    GetWindowRect (GUI.hWnd, &GUI.window_size);

#ifdef USE_OPENGL
    if (OPENGL_MODE)
        S9xOpenGLDeinit ();
#endif

	// this goes here, because the avi
	// recording might have messed with
	// the auto frame skip setting
	// (it needs to come before WinSave)
	DoAVIClose(0);

	S9xMovieShutdown (); // must happen before saving config

	WinUnlockConfigFile ();
    WinSaveConfigFile ();
	WinCleanupConfigData();

	Memory.Deinit();
#ifdef USE_GLIDE
	if(Settings.GlideEnable)
		S9xGlideDeinit();
	else if (Settings.OpenGLEnable)
#else
	if (Settings.OpenGLEnable)
#endif
	{
#ifdef USE_OPENGL
		S9xOpenGLDeinit();
#endif
	}
		S9xGraphicsDeinit();
		WinDeleteRecentGamesList ();
		DeinitS9x();

		MacroInit();

#ifdef CHECK_MEMORY_LEAKS
		_CrtDumpMemoryLeaks();
#endif
		return msg.wParam;
}

void RestoreGUIDisplay ()
{

    S9xSetPause (PAUSE_RESTORE_GUI);
#ifdef USE_GLIDE
    S9xGlideEnable (FALSE);
#endif
    if ((GUI.outputMethod==DIRECTDRAW) && !VOODOO_MODE && !OPENGL_MODE && GUI.FullScreen &&
        (GUI.Width < 640 || GUI.Height < 400) &&
        !DirectDraw.SetDisplayMode (640, 480, 1, 0, 60, !GUI.FullScreen, false))
    {
        MessageBox (GUI.hWnd, Languages[ GUI.Language].errModeDD, TEXT("Snes9X - DirectDraw(1)"), MB_OK | MB_ICONSTOP);
        S9xClearPause (PAUSE_RESTORE_GUI);
        return;
    }
    SwitchToGDI();
    S9xClearPause (PAUSE_RESTORE_GUI);
}

void RestoreSNESDisplay ()
{
    if ((GUI.outputMethod==DIRECTDRAW) && !VOODOO_MODE && !OPENGL_MODE &&
		!DirectDraw.SetDisplayMode (GUI.Width, GUI.Height, max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes)), GUI.Depth, GUI.RefreshRate,
		!GUI.FullScreen, GUI.tripleBuffering))
    {
        MessageBox (GUI.hWnd, Languages[ GUI.Language].errModeDD, TEXT("Snes9X - DirectDraw(4)"), MB_OK | MB_ICONSTOP);
        return;
    }

//	S9xInitUpdate();
#ifdef USE_GLIDE
    if (VOODOO_MODE && Glide.voodoo_present)
    {
        //S9xGlideEnable (TRUE);
    }
#endif

    UpdateBackBuffer();
}

char *S9xGetFreezeFilename(int slot) {
    TCHAR filename [_MAX_PATH + 1];
    TCHAR drive [_MAX_DRIVE + 1];
    TCHAR dir [_MAX_DIR + 1];
    TCHAR fname [_MAX_FNAME + 1];
    TCHAR ext [_MAX_EXT + 1];

    _splitpath (Memory.ROMFilename, drive, dir, fname, ext);
    static char *digits = "t123456789";
	for(int oldDir = 0; oldDir <= 1; oldDir++)
	{
		int zmv=0, freeze=1;
//		for(int zmv = 0; zmv <= 1; zmv++)
		{
		    if((!oldDir && !zmv) || (!freeze && _access (filename, 0) != 0 && slot < 10))
			{
				if(!zmv)
					sprintf (ext, TEXT(".%03d"), slot);
				else
					sprintf (ext, TEXT(".zs%c"), digits [slot]);
				if (GUI.FreezeFileDir [0])
				{
					strcpy (filename, oldDir ? S9xGetDirectory(ROMFILENAME_DIR) : S9xGetDirectory(SNAPSHOT_DIR));
					strcat (filename, TEXT("\\"));
					strcat (filename, fname);
					strcat (filename, ext);
				}
				else
					_makepath (filename, drive, dir, fname, ext);
			}
		}
	}


	return strdup(filename);
}

void FreezeUnfreeze (int slot, bool8 freeze)
{
    char *filename;

#ifdef NETPLAY_SUPPORT
    if (!freeze && Settings.NetPlay && !Settings.NetPlayServer)
    {
        S9xMessage (S9X_INFO, S9X_NETPLAY_NOT_SERVER,
			"Only the server is allowed to load freeze files.");
        return;
    }
#endif

    filename = S9xGetFreezeFilename(slot);
    if (!filename)
	return;

    S9xSetPause (PAUSE_FREEZE_FILE);

    if (freeze)
	{
//		extern bool diagnostic_freezing;
//		if(GetAsyncKeyState('Q') && GetAsyncKeyState('W') && GetAsyncKeyState('E') && GetAsyncKeyState('R'))
//		{
//			diagnostic_freezing = true;
//		}
        S9xFreezeGame (filename);
//
//		diagnostic_freezing = false;
	}
    else
    {
		const int prevSavedAtOp = ICPU.SavedAtOp;

        if (S9xUnfreezeGame (filename))
        {
//	        S9xMessage (S9X_INFO, S9X_FREEZE_FILE_INFO, S9xBasename (filename));
#ifdef NETPLAY_SUPPORT
            S9xNPServerQueueSendingFreezeFile (filename);
#endif
//            UpdateBackBuffer();
        }

		// fix next frame advance after loading non-skipping state from a skipping state
		if(prevSavedAtOp && !ICPU.SavedAtOp)
			skipNextFrameStop = true;
    }

    S9xClearPause (PAUSE_FREEZE_FILE);
    free(filename);
}

void CheckDirectoryIsWritable (const char *filename)
{
    FILE *fs = fopen (filename, "w+");

    if (fs == NULL)
	MessageBox (GUI.hWnd, TEXT("The folder where Snes9X saves emulated save RAM files and\ngame save positions (freeze files) is currently set to a\nread-only folder.\n\nIf you do not change the game save folder, Snes9X will be\nunable to save your progress in this game. Change the folder\nfrom the Settings Dialog available from the Options menu.\n\nThe default save folder is called Saves, if no value is set.\n"),
							 TEXT("Snes9X: Unable to save file warning"),
							 MB_OK | MB_ICONINFORMATION);
    else
    {
        fclose (fs);
        remove (filename);
    }
}

static void CheckMenuStates ()
{
    MENUITEMINFO mii;
    unsigned int i;

    ZeroMemory( &mii, sizeof( mii));
    mii.cbSize = sizeof( mii);
    mii.fMask = MIIM_STATE;

	mii.fState = (GUI.outputMethod == DIRECTDRAW) ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_RENDERMETHOD_DIRECTDRAW, FALSE, &mii);
	mii.fState = (GUI.outputMethod == DIRECT3D) ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_RENDERMETHOD_DIRECT3D, FALSE, &mii);

	mii.fState = GUI.ddrawUseVideoMemory ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_RENDEROPTIONS_DDRAWUSEVIDEOMEMORY, FALSE, &mii);

	mii.fState = GUI.ddrawUseLocalVidMem ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_RENDEROPTIONS_DDRAWUSELOCALVIDEOMEM, FALSE, &mii);

	mii.fState = GUI.tripleBuffering ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_RENDEROPTIONS_TRIPLEBUFFERING, FALSE, &mii);

	mii.fState = (GUI.d3dFilter == NEAREST) ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_RENDEROPTIONS_D3DNOFILTER, FALSE, &mii);
	mii.fState = (GUI.d3dFilter == BILINEAR) ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_RENDEROPTIONS_D3DBILINEAR, FALSE, &mii);

	for (int filter = 0; filter < (int)NUM_FILTERS; filter++) {
		mii.fState = (GUI.Scale == (RenderFilter)filter) ? MFS_CHECKED : MFS_UNCHECKED;
		SetMenuItemInfo (GUI.hMenu, ID_FILTER_MIN + filter, FALSE, &mii);
	}

	mii.fState = MFS_UNCHECKED;
	if (GUI.windowResizeLocked || GUI.FullScreen)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_WINDOW_X1, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_WINDOW_X2, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_WINDOW_X3, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_WINDOW_X4, FALSE, &mii);
	mii.fState = GUI.windowResizeLocked ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_WINDOW_LOCKRESIZE, FALSE, &mii);

    mii.fState = GUI.FullScreen ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_WINDOW_FULLSCREEN, FALSE, &mii);

	mii.fState = GUI.Stretch ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_WINDOW_STRETCH, FALSE, &mii);

	mii.fState = (abs(GUI.AspectWidth-301)<12) ? MFS_CHECKED : MFS_UNCHECKED; // note: enabled even if stretch is disabled because this option affects things other than stretch too
    SetMenuItemInfo (GUI.hMenu, ID_WINDOW_TVPIXELRATIO, FALSE, &mii);

	mii.fState = GUI.Stretch ? (GUI.AspectRatio ? MFS_CHECKED : MFS_UNCHECKED) : MFS_CHECKED|MFS_DISABLED;
    SetMenuItemInfo (GUI.hMenu, ID_WINDOW_ASPECTRATIO, FALSE, &mii);

	mii.fState = (Settings.Paused && !Settings.StopEmulation) ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_FILE_PAUSE, FALSE, &mii);

	mii.fState = GUI.MessagesInImage ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_VIDEO_TEXTINIMAGE, FALSE, &mii);
	mii.fState = Settings.LuaDrawingsInScreen ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_VIDEO_LUAGUIINIMAGE, FALSE, &mii);

	mii.fState = !(Settings.BG_Forced & 1) ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_VIDEO_LAYERS_BG1, FALSE, &mii);
	mii.fState = !(Settings.BG_Forced & 2) ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_VIDEO_LAYERS_BG2, FALSE, &mii);
	mii.fState = !(Settings.BG_Forced & 4) ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_VIDEO_LAYERS_BG3, FALSE, &mii);
	mii.fState = !(Settings.BG_Forced & 8) ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_VIDEO_LAYERS_BG4, FALSE, &mii);
	mii.fState = !(Settings.BG_Forced & 16) ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_VIDEO_LAYERS_SPRITES, FALSE, &mii);
	mii.fState = !Settings.DisableGraphicWindows ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_VIDEO_CLIPPINGWIDOWS, FALSE, &mii);
	mii.fState = Settings.Transparency ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_VIDEO_TRANSPARENCY, FALSE, &mii);
	mii.fState = !Settings.DisableHDMA ? MFS_CHECKED : MFS_UNCHECKED;
	if (S9xMovieActive())
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_EMULATOR_HDMAEMULATION, FALSE, &mii);

	mii.fState = Settings.SupportHiRes ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_EMULATOR_HIRES, FALSE, &mii);
	mii.fState = GUI.HeightExtend ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_EMULATOR_EXTENDHEIGHT, FALSE, &mii);

	mii.fState = Settings.DisplayFrameRate ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_DISPLAY_FRAMERATE, FALSE, &mii);
	mii.fState = Settings.DisplayPressedKeys ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_DISPLAY_INPUT, FALSE, &mii);
	mii.fState = Settings.DisplayFrame ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_DISPLAY_FRAMECOUNTER, FALSE, &mii);
	mii.fState = Settings.DisplayLagCounter ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_DISPLAY_LAGCOUNTER, FALSE, &mii);
	mii.fState = Settings.CounterInFrames ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_COUNTER_IN_FRAMES, FALSE, &mii);

	mii.fState = GUI.CustomRomOpen ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_EMULATOR_CUSTOMROMOPEN, FALSE, &mii);

	for(int i = 0; i < 10; i++)
	{
		mii.fState = (Settings.CompressionLevel == i) ? MFS_CHECKED : MFS_UNCHECKED;
		SetMenuItemInfo (GUI.hMenu, ID_EMULATOR_SAVECOMPRESS0 + i, FALSE, &mii);
	}

    mii.fState = MFS_UNCHECKED;
    if (Settings.StopEmulation)
        mii.fState |= MFS_DISABLED;
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE_SPC_DATA, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_SAVESCREENSHOT, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE_SRAM_DATA, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE0, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE1, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE2, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE3, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE4, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE5, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE6, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE7, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE8, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_SAVE9, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD0, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD1, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD2, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD3, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD4, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD5, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD6, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD7, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD8, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_LOAD9, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_RESET, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_CHEAT_ENTER, FALSE, &mii);

	mii.fState = RamWatchHWnd ? MFS_CHECKED : MFS_UNCHECKED;
	if (Settings.StopEmulation || GUI.FullScreen)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_RAM_WATCH, FALSE, &mii);
	mii.fState = RamSearchHWnd ? MFS_CHECKED : MFS_UNCHECKED;
	if (Settings.StopEmulation || GUI.FullScreen)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_RAM_SEARCH, FALSE, &mii);

	mii.fState = oldRamSearchHWND ? MFS_CHECKED : MFS_UNCHECKED;
	if (Settings.StopEmulation || GUI.FullScreen)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_RAM_SEARCH_OLD, FALSE, &mii);

	//mii.fState = Settings.StopEmulation ? MFS_DISABLED : MFS_ENABLED;
	mii.fState = MFS_ENABLED;
	SetMenuItemInfo (GUI.hMenu, ID_TRACE_LOGGER, FALSE, &mii);

	mii.fState = (inputMacroHWND && IsWindowVisible(inputMacroHWND)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (Settings.StopEmulation || GUI.FullScreen)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_OPTIONS_INPUT_MACRO, FALSE, &mii);

	mii.fState = (LuaConsoleHWnd ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, IDD_FILE_LUA_OPEN, FALSE, &mii);
	mii.fState = (LuaConsoleHWnd ? 0 : MFS_DISABLED);
	SetMenuItemInfo (GUI.hMenu, IDD_FILE_LUA_CLOSE_ALL, FALSE, &mii);

	for (i = 0; i < COUNT(idJoypad); i++) {
		mii.fState = Joypad[i].Enabled ? MFS_CHECKED : MFS_UNCHECKED;
		SetMenuItemInfo (GUI.hMenu, idJoypad[i], FALSE, &mii);
	}

	mii.fState = Settings.UpAndDown ? MFS_CHECKED : MFS_UNCHECKED;
	if (S9xMovieActive())
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_JOYPAD_ALLOWLEFTRIGHT, FALSE, &mii);

	mii.fState = (Settings.SkipFrames == AUTO_FRAMERATE) ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_FRAMESKIP_AUTOMATIC, FALSE, &mii);

	for (i = 0; i < COUNT(FrameSkipAmounts); i++) {
		if (FrameSkipAmounts[i].amount == (Settings.SkipFrames - 1))
			mii.fState = MFS_CHECKED;
		else
			mii.fState = MFS_UNCHECKED;
		SetMenuItemInfo (GUI.hMenu, FrameSkipAmounts[i].ident, FALSE, &mii);
	}

	bool customSpeed = true;
	for (i = 0; i < COUNT(FrameTimingRates); i++) {
		uint32 normalFrameTime = (Settings.PAL?Settings.FrameTimePAL:Settings.FrameTimeNTSC);

		if ((uint32)(normalFrameTime/FrameTimingRates[i].rate+0.5) == Settings.FrameTime) {
			mii.fState = MFS_CHECKED;
			customSpeed = false;
		}
		else
			mii.fState = MFS_UNCHECKED;
		SetMenuItemInfo (GUI.hMenu, FrameTimingRates[i].ident, FALSE, &mii);
	}
	mii.fState = customSpeed ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_FRAMESKIP_THROTTLE_OTHER, FALSE, &mii);

	for (i = 0; i < COUNT(ThreadPriorities); i++) {
		if (ThreadPriorities[i].priority == GUI.threadPriority)
			mii.fState = MFS_CHECKED;
		else
			mii.fState = MFS_UNCHECKED;
		SetMenuItemInfo (GUI.hMenu, ThreadPriorities[i].ident, FALSE, &mii);
	}

#ifdef NETPLAY_SUPPORT
    if (Settings.NetPlay && !Settings.NetPlayServer)
        mii.fState = MFS_DISABLED;
    else
        mii.fState = Settings.NetPlayServer ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_NETPLAY_SERVER, FALSE, &mii);

    mii.fState = Settings.NetPlay && !Settings.NetPlayServer ? 0 : MFS_DISABLED;
    SetMenuItemInfo (GUI.hMenu, ID_NETPLAY_DISCONNECT, FALSE, &mii);

    mii.fState = Settings.NetPlay || Settings.NetPlayServer ? MFS_DISABLED : 0;
    SetMenuItemInfo (GUI.hMenu, ID_NETPLAY_CONNECT, FALSE, &mii);

    mii.fState = NPServer.SendROMImageOnConnect ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_NETPLAY_SEND_ROM_ON_CONNECT, FALSE, &mii);

    mii.fState = NPServer.SyncByReset ? MFS_CHECKED : MFS_UNCHECKED;
    SetMenuItemInfo (GUI.hMenu, ID_NETPLAY_SYNC_BY_RESET, FALSE, &mii);

    mii.fState = Settings.NetPlayServer ? 0 : MFS_DISABLED;
    SetMenuItemInfo (GUI.hMenu, ID_NETPLAY_SYNC, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_NETPLAY_ROM, FALSE, &mii);
#endif

    mii.fState = !Settings.ApplyCheats ? MFS_CHECKED : MFS_UNCHECKED;
    if (Settings.StopEmulation)
        mii.fState |= MFS_DISABLED;
    SetMenuItemInfo( GUI.hMenu, ID_CHEAT_DISABLE, FALSE, &mii);

	bool soundIsActive = Settings.APUEnabled!=0;
//	bool soundIsActive = !(/*!DirectSound.DSAvailable ||*/ Settings.Mute || !Settings.APUEnabled);

#ifndef FMOD_SUPPORT
	RemoveMenu(GUI.hMenu, ID_SOUNDINTERFACE_FMOD_DIRECTSOUND, MF_BYCOMMAND);
	RemoveMenu(GUI.hMenu, ID_SOUNDINTERFACE_FMOD_WAVE, MF_BYCOMMAND);
	RemoveMenu(GUI.hMenu, ID_SOUNDINTERFACE_FMOD_A3D, MF_BYCOMMAND);
#endif
#ifndef FMODEX_SUPPORT
	RemoveMenu(GUI.hMenu, ID_SOUNDINTERFACE_FMODEX_DEFAULT, MF_BYCOMMAND);
	RemoveMenu(GUI.hMenu, ID_SOUNDINTERFACE_FMODEX_ASIO, MF_BYCOMMAND);
#endif

	mii.fState = (soundIsActive ? MFS_ENABLED : MFS_DISABLED)
		| ((Settings.SoundDriver == WIN_SNES9X_DIRECT_SOUND_DRIVER) ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, ID_SOUNDINTERFACE_DIRECTSOUND, FALSE, &mii);
	mii.fState = (soundIsActive ? MFS_ENABLED : MFS_DISABLED)
		| ((Settings.SoundDriver == WIN_XAUDIO2_SOUND_DRIVER) ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, ID_SOUNDINTERFACE_XAUDIO2, FALSE, &mii);
#ifdef FMOD_SUPPORT
	mii.fState = (soundIsActive ? MFS_ENABLED : MFS_DISABLED)
		| ((Settings.SoundDriver == WIN_FMOD_DIRECT_SOUND_DRIVER) ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, ID_SOUNDINTERFACE_FMOD_DIRECTSOUND, FALSE, &mii);
	mii.fState = (soundIsActive ? MFS_ENABLED : MFS_DISABLED)
		| ((Settings.SoundDriver == WIN_FMOD_WAVE_SOUND_DRIVER) ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, ID_SOUNDINTERFACE_FMOD_WAVE, FALSE, &mii);
	mii.fState = (soundIsActive ? MFS_ENABLED : MFS_DISABLED)
		| ((Settings.SoundDriver == WIN_FMOD_A3D_SOUND_DRIVER) ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, ID_SOUNDINTERFACE_FMOD_A3D, FALSE, &mii);
#endif
#ifdef FMODEX_SUPPORT
	mii.fState = (soundIsActive ? MFS_ENABLED : MFS_DISABLED)
		| ((Settings.SoundDriver == WIN_FMODEX_DEFAULT_DRIVER) ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, ID_SOUNDINTERFACE_FMODEX_DEFAULT, FALSE, &mii);
	mii.fState = (soundIsActive ? MFS_ENABLED : MFS_DISABLED)
		| ((Settings.SoundDriver == WIN_FMODEX_ASIO_DRIVER) ? MFS_CHECKED : MFS_UNCHECKED);
	SetMenuItemInfo (GUI.hMenu, ID_SOUNDINTERFACE_FMODEX_ASIO, FALSE, &mii);
#endif

	for (i = 0; i < COUNT(SoundRates); i++) {
		mii.fState = (SoundRates [i].rate == Settings.SoundPlaybackRate) ? MFS_CHECKED : MFS_UNCHECKED;
		if (!soundIsActive || /*S9xMovieActive() ||*/ GUI.WAVOut || GUI.AVIOut)
			mii.fState |= MFS_DISABLED;
		SetMenuItemInfo (GUI.hMenu, SoundRates[i].ident, FALSE, &mii);
	}

	mii.fState = Settings.SixteenBitSound ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive || GUI.WAVOut || GUI.AVIOut)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_16BIT, FALSE, &mii);

	mii.fState = Settings.Stereo ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive || GUI.WAVOut || GUI.AVIOut)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_STEREO, FALSE, &mii);

	mii.fState = Settings.InterpolatedSound ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_INTERPOLATED, FALSE, &mii);

	mii.fState = !Settings.DisableSoundEcho ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_ECHO, FALSE, &mii);

	mii.fState = Settings.ReverseStereo ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive || !Settings.Stereo)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_REVERSE_STEREO, FALSE, &mii);

	mii.fState = Settings.Mute ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_MUTE, FALSE, &mii);

	mii.fState = GUI.FAMute ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_MUTEFRAMEADVANCE, FALSE, &mii);

	mii.fState = (GUI.SoundChannelEnable & (1 << 0)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL1, FALSE, &mii);
	mii.fState = (GUI.SoundChannelEnable & (1 << 1)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL2, FALSE, &mii);
	mii.fState = (GUI.SoundChannelEnable & (1 << 2)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL3, FALSE, &mii);
	mii.fState = (GUI.SoundChannelEnable & (1 << 3)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL4, FALSE, &mii);
	mii.fState = (GUI.SoundChannelEnable & (1 << 4)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL5, FALSE, &mii);
	mii.fState = (GUI.SoundChannelEnable & (1 << 5)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL6, FALSE, &mii);
	mii.fState = (GUI.SoundChannelEnable & (1 << 6)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL7, FALSE, &mii);
	mii.fState = (GUI.SoundChannelEnable & (1 << 7)) ? MFS_CHECKED : MFS_UNCHECKED;
	if (!soundIsActive)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_CHANNEL8, FALSE, &mii);
	mii.fState = soundIsActive ? MFS_ENABLED : MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_CHANNELS_ENABLEALL, FALSE, &mii);

	mii.fState = (soundIsActive && (Settings.SoundDriver<1||Settings.SoundDriver>3)) ? MFS_ENABLED : MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_25MS, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_50MS, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_100MS, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_200MS, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_500MS, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_1S, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_2S, FALSE, &mii);
	mii.fState |= MFS_CHECKED;
	int id;
	switch (Settings.SoundBufferSize)
	{
	case 1:  id = ID_SOUND_25MS; break;
	case 2:  id = ID_SOUND_50MS; break;
	default:
	case 4:  id = ID_SOUND_100MS; break;
	case 8:  id = ID_SOUND_200MS; break;
	case 16: id = ID_SOUND_500MS; break;
	case 32: id = ID_SOUND_1S; break;
	case 64: id = ID_SOUND_2S; break;
	}
	SetMenuItemInfo (GUI.hMenu, id, FALSE, &mii);

	for (i = 0; i < COUNT(MixIntervals); i++) {
		mii.fState = (MixIntervals[i].ms == Settings.SoundMixInterval) ? MFS_CHECKED : MFS_UNCHECKED;
		if (!soundIsActive || !(Settings.SoundDriver<1||Settings.SoundDriver>3))
			mii.fState |= MFS_DISABLED;
		SetMenuItemInfo (GUI.hMenu, MixIntervals[i].ident, FALSE, &mii);
	}

	mii.fState = Settings.SoundEnvelopeHeightReading ? MFS_CHECKED : MFS_UNCHECKED;
	if (!Settings.APUEnabled || S9xMovieActive())
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_ENVXREADING, FALSE, &mii);

	mii.fState = Settings.FakeMuteFix ? MFS_CHECKED : MFS_UNCHECKED;
	if (!Settings.APUEnabled || S9xMovieActive())
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_FAKEMUTE, FALSE, &mii);

	mii.fState = Settings.SoundSync ? MFS_CHECKED : MFS_UNCHECKED;
	if (!Settings.APUEnabled/* || S9xMovieActive()*/)
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_SYNC, FALSE, &mii);

	mii.fState = GUI.FlexibleSoundMixMaster ? MFS_CHECKED : MFS_UNCHECKED;
	if (!Settings.APUEnabled || S9xMovieActive())
		mii.fState |= MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_SOUND_FLEXIBLEMIX, FALSE, &mii);

#ifndef DEBUGGER
    mii.fState = MFS_DISABLED;
#else
    mii.fState = MFS_UNCHECKED;
#endif
    SetMenuItemInfo (GUI.hMenu, ID_DEBUG_TRACE, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_DEBUG_TRACE_SPC, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_DEBUG_TRACE_SA1, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_DEBUG_TRACE_DSP1, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_DEBUG_FRAME_ADVANCE, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_DEBUG_SNES_STATUS, FALSE, &mii);

	mii.fState = (!Settings.StopEmulation) ? MFS_ENABLED : MFS_DISABLED;
    SetMenuItemInfo (GUI.hMenu, ID_FILE_MOVIE_PLAY, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_MOVIE_RECORD, FALSE, &mii);

	mii.fState = (S9xMovieActive () && !Settings.StopEmulation) ? MFS_ENABLED : MFS_DISABLED;
    SetMenuItemInfo (GUI.hMenu, ID_FILE_MOVIE_STOP, FALSE, &mii);
    SetMenuItemInfo (GUI.hMenu, ID_FILE_MOVIE_SEEK, FALSE, &mii);

	mii.fState = (S9xMovieActive() ? S9xMovieReadOnly() : GUI.MovieReadOnly) ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_MOVIE_READONLY, FALSE, &mii);

	mii.fState = !Settings.StopEmulation ? MFS_ENABLED : MFS_DISABLED;
	SetMenuItemInfo (GUI.hMenu, ID_FILE_WAV_RECORDING, FALSE, &mii);
	SetMenuItemInfo (GUI.hMenu, ID_FILE_AVI_RECORDING, FALSE, &mii);

	ZeroMemory(&mii, sizeof(mii));
	mii.cbSize = sizeof(mii);
	mii.fMask = MIIM_STRING;

	mii.dwTypeData = !GUI.WAVOut ? "Start WAV Recording..." : "Stop WAV Recording";
	SetMenuItemInfo (GUI.hMenu, ID_FILE_WAV_RECORDING, FALSE, &mii);
	mii.dwTypeData = !GUI.AVIOut ? "Start AVI Recording..." : "Stop AVI Recording";
	SetMenuItemInfo (GUI.hMenu, ID_FILE_AVI_RECORDING, FALSE, &mii);

	mii.dwTypeData = !S9xTraceLogStream ? "Start Trace Logger..." : "Stop Trace Logger";
	SetMenuItemInfo (GUI.hMenu, ID_TRACE_LOGGER, FALSE, &mii);

	ZeroMemory( &mii, sizeof( mii));
	mii.cbSize = sizeof( mii);
	mii.fMask = MIIM_STATE;

    mii.fState = GUI.AVIDoubleScale ? MFS_CHECKED : MFS_UNCHECKED;
    if (Settings.StopEmulation || GUI.AVIOut)
        mii.fState |= MFS_DISABLED;
    SetMenuItemInfo (GUI.hMenu, ID_AVI_DOUBLE_SCALE, FALSE, &mii);

	mii.fState = !GUI.InactivePause ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_EMULATOR_BACKGROUNDRUN, FALSE, &mii);
	mii.fState = GUI.BackgroundInput ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo (GUI.hMenu, ID_EMULATOR_BACKGROUNDINPUT, FALSE, &mii);

	UINT validFlag;
	enum controllers controller[2];
	int8 ids[4];
	S9xGetController(0, &controller[0], &ids[0],&ids[1],&ids[2],&ids[3]);
	S9xGetController(1, &controller[1], &ids[0],&ids[1],&ids[2],&ids[3]);

	validFlag = (((1<<SNES_JOYPAD) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_JOYPAD && controller[1] == CTL_JOYPAD) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_SNES_JOYPAD, FALSE, &mii);

	validFlag = (((1<<SNES_MULTIPLAYER5) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_JOYPAD && controller[1] == CTL_MP5) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_ENABLE_MULTITAP, FALSE, &mii);

	validFlag = (((1<<SNES_MULTIPLAYER8) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_MP5 && controller[1] == CTL_MP5) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_MULTITAP8, FALSE, &mii);

	validFlag = (((1<<SNES_MOUSE) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_MOUSE && controller[1] == CTL_JOYPAD) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_MOUSE_TOGGLE, FALSE, &mii);

	validFlag = (((1<<SNES_MOUSE_SWAPPED) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_JOYPAD && controller[1] == CTL_MOUSE) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_MOUSE_SWAPPED, FALSE, &mii);

	validFlag = (((1<<SNES_SUPERSCOPE) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_JOYPAD && controller[1] == CTL_SUPERSCOPE) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_SCOPE_TOGGLE, FALSE, &mii);

	validFlag = (((1<<SNES_JUSTIFIER) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_JOYPAD && controller[1] == CTL_JUSTIFIER && !ids[0]) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_JUSTIFIER, FALSE, &mii);

	validFlag = (((1<<SNES_JUSTIFIER_2) & GUI.ValidControllerOptions) && (!S9xMovieActive() || !S9xMovieGetFrameCounter())) ? MFS_ENABLED : MFS_DISABLED;
	mii.fState = validFlag|((controller[0] == CTL_JOYPAD && controller[1] == CTL_JUSTIFIER && ids[0]) ? MFS_CHECKED : MFS_UNCHECKED);
    SetMenuItemInfo (GUI.hMenu, IDM_JUSTIFIERS, FALSE, &mii);
}

static void ResetFrameTimer ()
{
    QueryPerformanceCounter((LARGE_INTEGER*)&PCStart);
	PCStartTicks = timeGetTime()*1000;
    if (Settings.FrameTime == Settings.FrameTimeNTSC) PCFrameTime = PCFrameTimeNTSC;
    else if (Settings.FrameTime == Settings.FrameTimePAL) PCFrameTime = PCFrameTimePAL;
    else PCFrameTime = (__int64)((double)(PCBase * Settings.FrameTime) * .000001);


    if (GUI.hFrameTimer)
        timeKillEvent (GUI.hFrameTimer);

    GUI.hFrameTimer = timeSetEvent ((Settings.FrameTime+500)/1000, 0, FrameTimer, 0, TIME_PERIODIC);
}

void Update_Old_RAM_Watch ()
{
	if(watches[0].on)
	{
		// copy the memory used by each active watch
		for(unsigned int i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
		{
			if(watches[i].on)
			{
				int address = watches[i].address - 0x7E0000;
				const uint8* source;
				if(address < 0x20000)
					source = Memory.RAM + address ;
				else if(address < 0x30000)
					source = Memory.SRAM + address  - 0x20000;
				else
					source = Memory.FillRAM + address  - 0x30000;

				CopyMemory(Cheat.CWatchRAM + address, source, watches[i].size);
			}
		}
	}
}

bool8 LoadROM (const char *filename)
{
	if (!filename || !*filename)
		return (FALSE);
	SetCurrentDirectory(S9xGetDirectory(ROM_DIR));
    if (Memory.LoadROM (filename))
    {
		S9xStartCheatSearch (&Cheat);
        ReInitSound(1);
        ResetFrameTimer ();
        return (TRUE);
    }
    return (FALSE);
}

bool8 LoadMultiROM (const char *filename, const char *filename2)
{
	SetCurrentDirectory(S9xGetDirectory(ROM_DIR));
    if (Memory.LoadMultiCart (filename, filename2))
    {
		S9xStartCheatSearch (&Cheat);
        ReInitSound(1);
        ResetFrameTimer ();
        return (TRUE);
    }
    return (FALSE);
}

bool8 S9xLoadROMImage (const TCHAR *string)
{
    RestoreGUIDisplay ();
    TCHAR *buf = new TCHAR [200 + strlen (string)];
    sprintf (buf, TEXT("The NetPlay server is requesting you load the following game:\n '%s'"),
		string);

    MessageBox (GUI.hWnd, buf,
		TEXT(SNES9X_INFO),
		MB_OK | MB_ICONINFORMATION);

    delete buf;

    TCHAR FileName [_MAX_PATH];

	if(DoOpenRomDialog(FileName))
    {
        if (!Settings.StopEmulation)
        {
            Memory.SaveSRAM (S9xGetFilename (".srm", SRAM_DIR));
            S9xSaveCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
        }
        Settings.StopEmulation = !LoadROM (FileName);
        if (!Settings.StopEmulation)
        {
			bool8 loaded = Memory.LoadSRAM (S9xGetFilename (".srm", SRAM_DIR));
			if(!loaded) // help migration from earlier Snes9x versions by checking ROM directory for savestates
				Memory.LoadSRAM (S9xGetFilename (".srm", ROMFILENAME_DIR));
            S9xLoadCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
            CheckDirectoryIsWritable (S9xGetFilename (".---", SNAPSHOT_DIR));
            CheckMenuStates ();
        }
        else
            return (FALSE);
    }
    else
        return (FALSE);

    return (TRUE);
}

/*****************************************************************************/
#ifdef NETPLAY_SUPPORT
void EnableServer (bool8 enable)
{
    if (enable != Settings.NetPlayServer)
    {
        if (Settings.NetPlay && !Settings.NetPlayServer)
        {
            Settings.NetPlay = FALSE;
            S9xNPDisconnect ();
        }

        if (enable)
        {
            S9xSetPause (PAUSE_NETPLAY_CONNECT);
            Settings.NetPlayServer = S9xNPStartServer (Settings.Port);
            Sleep (1000);

            if (!S9xNPConnectToServer ("127.0.0.1", Settings.Port,
				Memory.ROMName))
            {
                S9xClearPause (PAUSE_NETPLAY_CONNECT);
            }
        }
        else
        {
            Settings.NetPlayServer = FALSE;
            S9xNPStopServer ();
        }
    }
}
#endif
#ifdef USE_OPENGL
bool8 S9xOpenGLInit ()
{
    int PixelFormat;

    if (GUI.FullScreen)
    {
        DEVMODE dmScreenSettings;

        memset (&dmScreenSettings, 0, sizeof(dmScreenSettings));
        dmScreenSettings.dmSize = sizeof(dmScreenSettings);
        dmScreenSettings.dmPelsWidth = GUI.Width;
        dmScreenSettings.dmPelsHeight = GUI.Height;
        dmScreenSettings.dmBitsPerPel = 16; //bits;
        dmScreenSettings.dmFields = DM_BITSPERPEL|DM_PELSWIDTH|DM_PELSHEIGHT;

        if (ChangeDisplaySettings (&dmScreenSettings, CDS_FULLSCREEN) != DISP_CHANGE_SUCCESSFUL)
        {
            GUI.FullScreen = FALSE;
        }
    }
    if (GUI.FullScreen)
    {
        SetWindowLong (GUI.hWnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowPos (GUI.hWnd, HWND_TOP, 0, 0, GUI.Width,
			GUI.Height,
			SWP_DRAWFRAME|SWP_FRAMECHANGED);
    }
    else
    {
		bool maximized = GUI.window_maximized;
        SetWindowLong (GUI.hWnd, GWL_STYLE, WS_POPUPWINDOW|WS_CAPTION|
			WS_VISIBLE|WS_MINIMIZEBOX|WS_MAXIMIZEBOX|(GUI.windowResizeLocked?0:WS_THICKFRAME));
        SetWindowPos (GUI.hWnd, HWND_TOP,
			GUI.window_size.left,
			GUI.window_size.top,
			GUI.window_size.right - GUI.window_size.left,
			GUI.window_size.bottom - GUI.window_size.top,
			SWP_DRAWFRAME|SWP_FRAMECHANGED);
		if(maximized)
			ShowWindow(GUI.hWnd, SW_MAXIMIZE);
    }

	if(OpenGL.initialized)
		return (TRUE);

	static PIXELFORMATDESCRIPTOR pfd =
    {
        sizeof(PIXELFORMATDESCRIPTOR),
			1,
			PFD_DRAW_TO_WINDOW |
			PFD_SUPPORT_OPENGL |
			PFD_DOUBLEBUFFER,
			PFD_TYPE_RGBA,
			16, //bits,
			0, 0, 0, 0, 0, 0,
			0,
			0,
			0,
			0, 0, 0, 0,
			16,
			0,
			0,
			PFD_MAIN_PLANE,
			0,
			0, 0, 0
    };

    if (!(PixelFormat = ChoosePixelFormat (GUI.hDC, &pfd)))
    {
        MessageBox(NULL,TEXT("Can't Find A Suitable PixelFormat."),TEXT("ERROR"),MB_OK|MB_ICONEXCLAMATION);
        return FALSE;
    }

    if (!SetPixelFormat (GUI.hDC, PixelFormat, &pfd))
    {
        MessageBox(NULL,TEXT("Can't Set The PixelFormat."),TEXT("ERROR"),MB_OK|MB_ICONEXCLAMATION);
        return (FALSE);
    }

    if (!(GUI.hRC = wglCreateContext (GUI.hDC)))
    {
        MessageBox(NULL,TEXT("Can't Create A GL Rendering Context."),TEXT("ERROR"),MB_OK|MB_ICONEXCLAMATION);
        return (FALSE);
    }

    if (!wglMakeCurrent (GUI.hDC, GUI.hRC))
    {
        MessageBox(NULL,TEXT("Can't Activate The GL Rendering Context."),TEXT("ERROR"),MB_OK|MB_ICONEXCLAMATION);
        return (FALSE);
    }

    glGetIntegerv (GL_MAX_TEXTURE_SIZE, &OpenGL.max_texture_size);

    if (OpenGL.max_texture_size >= 512)
    {
        OpenGL.texture_size = 512;
        OpenGL.num_textures = 2;
    }
    else
    {
        OpenGL.texture_size = OpenGL.max_texture_size;
        OpenGL.num_textures = 1;
    }

    Settings.OpenGLEnable = TRUE;

    const char *ext = (const char *) glGetString (GL_EXTENSIONS);

    if (ext && strstr (ext, "EXT_packed_pixels") != NULL)
        OpenGL.packed_pixels_extension_present = TRUE;

    const char *version = (const char *) glGetString (GL_VERSION);

    if (version && strlen (version) < 100)
    {
		char ver [100];
		strcpy (ver,  version);

		// Strip dots from the version string
		char *ptr;
		while (ptr = strchr (ver, '.'))
			memmove (ptr, ptr + 1, strlen (ptr + 1) + 1);

		// Pad the string with zeros to 4 digits
		while (strlen (ver) < 4)
			strcat (ver, "0");

		OpenGL.version = atoi (ver);
    }
    else
		OpenGL.version = 1100;

#ifndef GL_UNSIGNED_SHORT_5_5_5_1_EXT
#define GL_UNSIGNED_SHORT_5_5_5_1_EXT     0x8034
#endif

    if (OpenGL.version >= 1200)
    {
        OpenGL.internal_format = GL_RGB5_A1;
        OpenGL.format = GL_RGBA;
        OpenGL.type = GL_UNSIGNED_SHORT_5_5_5_1_EXT;
    }
    else
		if (OpenGL.packed_pixels_extension_present)
		{
			OpenGL.internal_format = GL_RGB5_A1;
			OpenGL.format = GL_RGBA;
			OpenGL.type = GL_UNSIGNED_SHORT_5_5_5_1_EXT;
		}
		else
		{
			OpenGL.internal_format = GL_RGB;
			OpenGL.format = GL_RGB;
			OpenGL.type = GL_UNSIGNED_BYTE;
		}

		glGenTextures (OpenGL.num_textures, OpenGL.textures);

		if (OpenGL.num_textures == 2)
		{
			glBindTexture (GL_TEXTURE_2D, OpenGL.textures [1]);
			glTexImage2D (GL_TEXTURE_2D, 0, OpenGL.internal_format, 256, 256, 0,
				OpenGL.format, OpenGL.type, NULL);

			glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
			glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
			glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
			glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			glTexEnvf (GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
		}

		glBindTexture (GL_TEXTURE_2D, OpenGL.textures [0]);
		glTexImage2D (GL_TEXTURE_2D, 0, OpenGL.internal_format,
			OpenGL.texture_size, OpenGL.texture_size, 0,
			OpenGL.format, OpenGL.type, NULL);

		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		glTexParameteri (GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexEnvf (GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

		glPolygonMode (GL_FRONT, GL_FILL);
		glEnable (GL_CULL_FACE);
		glCullFace (GL_BACK);

		glEnable (GL_DITHER);
		glEnable (GL_LIGHTING);
		glEnable (GL_LIGHT0);

		glEnable (GL_POINT_SMOOTH);
		glHint (GL_POINT_SMOOTH_HINT, GL_NICEST);

		glEnable (GL_TEXTURE_2D);

		glShadeModel (GL_SMOOTH);
		glClearColor (0.0f, 0.0f, 0.0f, 1.0f);
		glClearDepth (1.0f);
		glDisable (GL_DEPTH_TEST);
		glDepthFunc (GL_LESS);
		glHint (GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);

		GLfloat LightAmbient[]    = { 0.2f, 0.2f, 0.2f, 1.0f };
		GLfloat LightDiffuse[]    = { 1.0f, 1.0f, 1.0f, 1.0f };
		GLfloat LightSpecular[]   = { 0.5f, 0.5f, 0.5f, 1.0f };
		GLfloat LightPosition[]   = { 0.0f, 0.0f, 2.0f, 1.0f };

		glLightfv (GL_LIGHT0, GL_AMBIENT, LightAmbient);
		glLightfv (GL_LIGHT0, GL_DIFFUSE, LightDiffuse);
		glLightfv (GL_LIGHT0, GL_SPECULAR, LightSpecular);
		glLightfv (GL_LIGHT0, GL_POSITION, LightPosition);

		//Set common material properties
		GLfloat MatSpecular[]    = { 1.0f, 1.0f, 1.0f, 1.1f };
		GLfloat WhMat[]          = { 1.0f, 1.0f, 1.0f, 1.0f };

		glMaterialf (GL_FRONT_AND_BACK, GL_SHININESS, 128.0f);
		glMaterialfv (GL_FRONT_AND_BACK, GL_SPECULAR, MatSpecular);
		glMaterialfv (GL_FRONT_AND_BACK, GL_AMBIENT_AND_DIFFUSE, WhMat);

		glMatrixMode (GL_PROJECTION);
		glLoadIdentity ();
		glMatrixMode (GL_MODELVIEW);

	    glDisable (GL_DEPTH_TEST);
	    glDisable (GL_LIGHTING);
		glEnable (GL_TEXTURE_2D);
		glTexEnvf (GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);
		glDisable (GL_BLEND);

		OpenGL.initialized = TRUE;

		return (TRUE);
}

void S9xOpenGLDeinit ()
{
    if (GUI.FullScreen)
        ChangeDisplaySettings (NULL, 0);

    if (GUI.hRC)
    {
        wglMakeCurrent (NULL, NULL);
        wglDeleteContext (GUI.hRC);
        GUI.hRC = NULL;
    }
}

void S9xOpenGLResize (int width, int height)
{
    if (height == 0)
        height = 1;

    glViewport (0, 0, width, height);

    glMatrixMode (GL_PROJECTION);
    glLoadIdentity();

    gluPerspective (45.0f, (GLfloat)width / (GLfloat)height, 0.1f, 100.0f);
    glMatrixMode (GL_MODELVIEW);
}
#endif

HMENU GetSubMenuFromID(HMENU hMenu, UINT wID)
{
	HMENU hSubMenu;

	if (hMenu == NULL)
		return NULL;

	for (int i = 0; i < GetMenuItemCount(hMenu); i++) {
		if (GetMenuItemID(hMenu, i) == wID)
			return hMenu;

		// if submenu, do recursive search
		hSubMenu = GetSubMenuFromID(GetSubMenu(hMenu, i), wID);
		if (hSubMenu != NULL)
			return hSubMenu;
	}
	return NULL;
}

HMENU GetSubMenuFromString(HMENU hMenu, LPCTSTR lpString, bool recursive)
{
	HMENU hSubMenu;
	TCHAR menuString[0x1000];

	if (hMenu == NULL)
		return NULL;

	for (int i = 0; i < GetMenuItemCount(hMenu); i++) {
		GetMenuString(hMenu, i, menuString, COUNT(menuString), MF_BYPOSITION);
fprintf(stderr, "%s\n", menuString);
		if (lstrcmp(lpString, menuString) == 0)
			return GetSubMenu(hMenu, i);

		if (recursive) {
			hSubMenu = GetSubMenuFromString(GetSubMenu(hMenu, i), lpString, recursive);
			if (hSubMenu != NULL)
				return hSubMenu;
		}
	}
	return NULL;
}

HMENU GetRecentSubMenu ()
{
	HMENU file, recent = NULL;

	file = GetSubMenu (GUI.hMenu, 0);
	if (file)
		recent = GetSubMenu (file, 1);
	return recent;
}

void S9xAddToRecentGames (const char *filename)
{
    // Make sure its not in the list already
    int i;
	for(i = 0; i < MAX_RECENT_GAMES_LIST_SIZE; i++)
        if (!*GUI.RecentGames[i] || strcmp (filename, GUI.RecentGames[i]) == 0)
            break;

	const bool underMax = (i < MAX_RECENT_GAMES_LIST_SIZE);
	if(underMax && *GUI.RecentGames[i])
	{
		// It is in the list, move it to the head of the list.
		char temp [MAX_PATH];
		strcpy(temp, GUI.RecentGames[i]);
		for(int j = i; j > 0; j--)
			strcpy(GUI.RecentGames[j], GUI.RecentGames[j-1]);

		strcpy(GUI.RecentGames[0], temp);
	}
	else
	{
		// Not in the list, add it.
		if(underMax)
			// Extend the recent game list length by 1.
			memmove(&GUI.RecentGames[1], &GUI.RecentGames[0], MAX_PATH*i);
		else
			// Throw the last item off the end of the list
			memmove(&GUI.RecentGames[1], &GUI.RecentGames[0], MAX_PATH*(i-1));

		strcpy(GUI.RecentGames[0], filename);

		WinSaveConfigFile();
	}

    S9xSetRecentGames();
}

void S9xRemoveFromRecentGames (int i)
{
	if (*GUI.RecentGames [i])
	{
		for (int j = i; j < MAX_RECENT_GAMES_LIST_SIZE-1; j++)
			strcpy(GUI.RecentGames [j], GUI.RecentGames [j + 1]);
		*GUI.RecentGames [MAX_RECENT_GAMES_LIST_SIZE-1] = '\0';

		S9xSetRecentGames ();
	}
}

void S9xSetRecentGames ()
{
	HMENU recent = GetRecentSubMenu();
	if (recent)
	{
		MENUITEMINFO mii;
		TCHAR name [256 + 10];
		int i;

		// Clear out the menu first
		for (i = GetMenuItemCount (recent) - 1; i >= 0; i--)
			RemoveMenu (recent, i, MF_BYPOSITION);

		mii.cbSize = sizeof (mii);
		mii.fMask = MIIM_TYPE | MIIM_DATA | MIIM_STATE | MIIM_ID;
		mii.fType = MFT_STRING;
		mii.fState = MFS_UNCHECKED;

		for (i = 0; i < MAX_RECENT_GAMES_LIST_SIZE && i < GUI.MaxRecentGames && *GUI.RecentGames [i]; i++)
		{
			// Build up a menu item string in the form:
			// 1. <basename of ROM image name>

			sprintf (name, TEXT("&%c. "), i < 9 ? '1' + i : 'A' + i - 9);

			// append the game title to name, with formatting modifications as necessary
			{
				TCHAR baseName [256];
				strcpy (baseName, S9xBasename (GUI.RecentGames [i]));
				int pos = strlen (name), baseNameLen = strlen (baseName);
				for (int j = 0; j < baseNameLen ; j++)
				{
					char c = baseName [j];
					name [pos++] = c;

					// & is a special character in Windows menus,
					// so we have to change & to && when copying over the game title
					// otherwise "Pocky & Rocky (U).smc" will show up as "Pocky _Rocky (U).smc", for example
					if(c == '&')
						name [pos++] = '&';
				}
				name [pos] = '\0';
			}

			mii.dwTypeData = name;
			mii.cch = strlen (name) + 1;
			mii.wID = ID_RECENT_MIN + i;

			InsertMenuItem (recent, ID_RECENT_MIN + i, FALSE, &mii);
		}
	}
}

void WinDeleteRecentGamesList ()
{
	for(int i=0;i<MAX_RECENT_GAMES_LIST_SIZE;i++)
		GUI.RecentGames[i][0]='\0';
}

HMENU GetFilterSubMenu ()
{
	HMENU config, filter = NULL;

	config = GetSubMenu(GUI.hMenu, 1);
	if (config) {
		filter = GetSubMenuFromString(config, _T("F&ilter"), true);
	}
	return filter;
}

void S9xSetFilters ()
{
	HMENU menuFilter;

	menuFilter = GetFilterSubMenu();
	if (menuFilter)
	{
		MENUITEMINFO mii;
		static TCHAR name [256];

		// Clear out the menu first
		for (int i = GetMenuItemCount(menuFilter) - 1; i >= 0; i--)
			RemoveMenu(menuFilter, i, MF_BYPOSITION);

		mii.cbSize = sizeof (mii);
		mii.fMask = MIIM_TYPE | MIIM_DATA | MIIM_STATE | MIIM_ID;
		mii.fType = MFT_STRING;
		mii.fState = MFS_UNCHECKED;

		for(int filter = 0 ; filter < (int)NUM_FILTERS ; filter++)
		{
			strcpy(name, GetFilterName((RenderFilter)filter));

			mii.dwTypeData = name;
			mii.cch = strlen (name) + 1;
			mii.wID = ID_FILTER_MIN + filter;

			InsertMenuItem (menuFilter, ID_FILTER_MIN + filter, FALSE, &mii);
		}
	}
}

struct QueuedMessage {
    HWND hWnd;
    UINT uMsg;
    WPARAM wParam;
    LPARAM lParam;
    int modifiers;
    QueuedMessage(const HWND &h, const UINT &u, const WPARAM &w, const LPARAM &l, int m):
        hWnd(h), uMsg(u), wParam(w), lParam(l), modifiers(m) {}
};
static queue<QueuedMessage> QueuedMessages;

void WarnIfDeferredMessagesExist()
{
    if (!QueuedMessages.empty())
        MessageBox(NULL,TEXT("Couldn't finish deferred actions. Savestate data may have been lost. Report this in the \"desyncless 1.51\" thread."),
                TEXT("BUG"), MB_OK|MB_ICONEXCLAMATION);
}

void DispatchMessagesInQueue()
{
    while (!QueuedMessages.empty())
    {
        const QueuedMessage &q = QueuedMessages.front();
        if (q.uMsg == WM_KEYDOWN || q.uMsg == WM_CUSTKEYDOWN || q.uMsg == WM_SYSKEYDOWN)
            HandleKeyMessage(q.wParam, q.lParam, q.modifiers);
        else
        WinProc(q.hWnd, q.uMsg, q.wParam, q.lParam);
        QueuedMessages.pop();
    }
}

bool8 MustDeferMessage(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    /* Note:
     * Not worrying about options like soundskip settings, 7110 RTC, cheats,
     * or disabling HDMA: they'll ruin sync anyway.
     */
    int action = PROCESS_NOW;
    int modifiers = 0;

    switch (uMsg)
    {
        case WM_KEYDOWN:
            if(wParam != VK_PAUSE && GUI.BackgroundInput)
                break;
        case WM_SYSKEYDOWN:
        case WM_CUSTKEYDOWN:
            modifiers = GetModifiers(wParam);
			if(uMsg == WM_CUSTKEYDOWN)
			{
				if(wParam == VK_PAUSE || modifiers == CUSTKEY_ALT_MASK)
				{
					// WM_KEYDOWN or WM_SYSKEYDOWN should handle this
					break;
				}
			}

            action = MustDeferKeyMessage(wParam, lParam, modifiers);
            break;

        case WM_COMMAND:
            switch (wParam & 0xffff)
            {
                case IDM_SNES_JOYPAD:
                case IDM_ENABLE_MULTITAP:
                case IDM_SCOPE_TOGGLE:
                case IDM_JUSTIFIER:
                case IDM_MOUSE_TOGGLE:
                case IDM_MOUSE_SWAPPED:
                case IDM_MULTITAP8:
                case IDM_JUSTIFIERS:
                    MOVIE_LOCKED_SETTING
                    /* Fall through */
                case ID_FILE_SAVE1: case ID_FILE_SAVE2: case ID_FILE_SAVE3:
                case ID_FILE_SAVE4: case ID_FILE_SAVE5: case ID_FILE_SAVE6:
                case ID_FILE_SAVE7: case ID_FILE_SAVE8: case ID_FILE_SAVE9:
                case ID_FILE_SAVE0:
                case ID_FILE_RESET:
                        action = PROCESS_AFTER_MANUAL_ADVANCE;
                        break;

                case ID_FILE_MOVIE_STOP:
                case ID_FILE_MOVIE_PLAY:
                case ID_FILE_MOVIE_RECORD:
                case ID_FILE_SAVE_SRAM_DATA:
                case ID_FILE_LOAD1: case ID_FILE_LOAD2: case ID_FILE_LOAD3:
                case ID_FILE_LOAD4: case ID_FILE_LOAD5: case ID_FILE_LOAD6:
                case ID_FILE_LOAD7: case ID_FILE_LOAD8: case ID_FILE_LOAD9:
                case ID_FILE_LOAD0:
                case ID_FILE_LOADMULTICART:
                case ID_FILE_LOAD_GAME:
                case ID_FILE_EXIT:
                        action = PROCESS_AFTER_AUTO_ADVANCE;
                        break;

                default: {
                    int menuID = wParam & 0xffff;
                    if (menuID >= ID_RECENT_MIN && menuID <= ID_RECENT_MAX)
                    {
                        int i = menuID - ID_RECENT_MIN;
                        int j = 0;
                        {
                            while (j < MAX_RECENT_GAMES_LIST_SIZE && j != i)
                                j++;
                            if (i == j)
                                action = PROCESS_AFTER_AUTO_ADVANCE;
                        }
                    }
                }
            }
            break;
	case WM_DESTROY:
	case WM_NCDESTROY:
            // Deferred FILE_SAVE actions must be processed first!
            // S9xOnSNESPadRead() and we can finish FILE_SAVEs before quitting.
            PostMessage(NULL, WM_SYSCOMMAND, SC_SCREENSAVE, 0);
            action = PROCESS_AFTER_AUTO_ADVANCE;
            break;
    }

    switch (action)
    {
        case PROCESS_AFTER_AUTO_ADVANCE:
            Settings.Paused = false;
            S9xMouseOn();
            GUI.IgnoreNextMouseMove = true;
            Settings.Paused = true;
            Settings.FrameAdvance = true;
            GUI.FrameAdvanceJustPressed = 2;
            QueuedMessages.push(QueuedMessage(hWnd, uMsg, wParam, lParam, modifiers));
            return true;

        case PROCESS_AFTER_MANUAL_ADVANCE:
            S9xMessage (S9X_INFO, S9X_MOVIE_INFO, MOVIE_INFO_ACTION_DEFERRED);
            QueuedMessages.push(QueuedMessage(hWnd, uMsg, wParam, lParam, modifiers));
            return true;

        case PROCESS_NOW:
        default:
            return false;
    }
}

int MustDeferKeyMessage(WPARAM wParam, LPARAM lParam, int modifiers)
{
	if(wParam == 0 || wParam == VK_ESCAPE)
		return PROCESS_NOW;

	SCustomKey *key = CustomKeys.key;
	while (!IsLastCustomKey(key)) {
		if (wParam == key->key && modifiers == key->modifiers) {
			return key->timing;
		}
		key++;
	}
	return PROCESS_NOW;
}

int GetModifiers(int key)
{
    int modifiers = 0;

    if (key == VK_MENU || key == VK_CONTROL || key == VK_SHIFT)
        return 0;

    if(GetAsyncKeyState(VK_MENU   )) modifiers |= CUSTKEY_ALT_MASK;
    if(GetAsyncKeyState(VK_CONTROL)) modifiers |= CUSTKEY_CTRL_MASK;
    if(GetAsyncKeyState(VK_SHIFT  )) modifiers |= CUSTKEY_SHIFT_MASK;
    return modifiers;
}

INT_PTR CALLBACK DlgSoundConf(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	static struct SSettings* set;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		hBmp=(HBITMAP)LoadImage(NULL, TEXT("Jerremy.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
		set= (struct SSettings *)lParam;
		// FIXME: these strings should come from wlanguage.h
#ifndef MK_APU
		SetDlgItemText(hDlg,IDC_LINEAR_INTER, "Gaussian &Interpolation of Sample Data");
#endif
		int pos;
		pos = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_INSERTSTRING,-1,(LPARAM)TEXT("Snes9x DirectSound"));
		SendDlgItemMessage(hDlg, IDC_DRIVER, CB_SETITEMDATA,pos,WIN_SNES9X_DIRECT_SOUND_DRIVER);
		pos = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_INSERTSTRING,-1,(LPARAM)TEXT("XAudio2"));
		SendDlgItemMessage(hDlg, IDC_DRIVER, CB_SETITEMDATA,pos,WIN_XAUDIO2_SOUND_DRIVER);
#ifdef FMOD_SUPPORT
		pos = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_INSERTSTRING,-1,(LPARAM)TEXT("FMOD DirectSound"));
		SendDlgItemMessage(hDlg, IDC_DRIVER, CB_SETITEMDATA,pos,WIN_FMOD_DIRECT_SOUND_DRIVER);
		pos = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_INSERTSTRING,-1,(LPARAM)TEXT("FMOD Windows Multimedia"));
		SendDlgItemMessage(hDlg, IDC_DRIVER, CB_SETITEMDATA,pos,WIN_FMOD_WAVE_SOUND_DRIVER);
		pos = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_INSERTSTRING,-1,(LPARAM)TEXT("FMOD A3D"));
		SendDlgItemMessage(hDlg, IDC_DRIVER, CB_SETITEMDATA,pos,WIN_FMOD_A3D_SOUND_DRIVER);
#elif defined FMODEX_SUPPORT
		pos = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_INSERTSTRING,-1,(LPARAM)TEXT("FMOD Ex Default"));
		SendDlgItemMessage(hDlg, IDC_DRIVER, CB_SETITEMDATA,pos,WIN_FMODEX_DEFAULT_DRIVER);
		pos = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_INSERTSTRING,-1,(LPARAM)TEXT("FMOD Ex ASIO"));
		SendDlgItemMessage(hDlg, IDC_DRIVER, CB_SETITEMDATA,pos,WIN_FMODEX_ASIO_DRIVER);
#endif
		SendDlgItemMessage(hDlg, IDC_DRIVER,CB_SETCURSEL,0,0);
		for(pos = 0;pos<SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETCOUNT,0,0);pos++) {
			if(SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETITEMDATA,pos,0)==set->SoundDriver) {
				SendDlgItemMessage(hDlg, IDC_DRIVER,CB_SETCURSEL,pos,0);
				break;
			}
		}

		SendDlgItemMessage(hDlg, IDC_SKIP_TYPE, CB_INSERTSTRING,0,(LPARAM)TEXT("Skip style #1"));
		SendDlgItemMessage(hDlg, IDC_SKIP_TYPE, CB_INSERTSTRING,1,(LPARAM)TEXT("Skip style #2"));
		SendDlgItemMessage(hDlg, IDC_SKIP_TYPE, CB_INSERTSTRING,2,(LPARAM)TEXT("Skip style #3"));
		SendDlgItemMessage(hDlg, IDC_SKIP_TYPE, CB_INSERTSTRING,3,(LPARAM)TEXT("Skip style #4"));
		SendDlgItemMessage(hDlg, IDC_SKIP_TYPE, CB_INSERTSTRING,4,(LPARAM)TEXT("Skip style #5"));

		SendDlgItemMessage(hDlg,IDC_SKIP_TYPE,CB_SETCURSEL,set->SoundSkipMethod,0);

		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,0,(LPARAM)TEXT("8 KHz"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,1,(LPARAM)TEXT("11 KHz"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,2,(LPARAM)TEXT("16 KHz"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,3,(LPARAM)TEXT("22 KHz"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,4,(LPARAM)TEXT("30 KHz"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,5,(LPARAM)TEXT("32 KHz (SNES)"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,6,(LPARAM)TEXT("35 KHz"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,7,(LPARAM)TEXT("44 KHz"));
		SendDlgItemMessage(hDlg, IDC_RATE, CB_INSERTSTRING,8,(LPARAM)TEXT("48 KHz"));

		int temp;
		switch(set->SoundPlaybackRate)
		{
		case 8000:temp=0;break;
		case 11025:temp=1;break;
		case 16000:temp=2;break;
		case 22050:temp=3;break;
		case 30000:temp=4;break;
		case 0:
		default:
		case 32000:temp=5;break;
		case 35000:temp=6;break;
		case 44000:
		case 44100:temp=7;break;
		case 48000:temp=8;break;
		}
		SendDlgItemMessage(hDlg,IDC_RATE,CB_SETCURSEL,temp,0);

		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,0,(LPARAM)TEXT("10 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,1,(LPARAM)TEXT("20 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,2,(LPARAM)TEXT("30 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,3,(LPARAM)TEXT("40 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,4,(LPARAM)TEXT("50 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,5,(LPARAM)TEXT("60 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,6,(LPARAM)TEXT("70 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,7,(LPARAM)TEXT("80 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,8,(LPARAM)TEXT("90 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,9,(LPARAM)TEXT("100 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,10,(LPARAM)TEXT("110 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,11,(LPARAM)TEXT("120 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,12,(LPARAM)TEXT("130 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,13,(LPARAM)TEXT("140 ms"));
		SendDlgItemMessage(hDlg, IDC_MIX, CB_INSERTSTRING,14,(LPARAM)TEXT("150 ms"));

		SendDlgItemMessage(hDlg,IDC_MIX,CB_SETCURSEL,((set->SoundMixInterval/10)-1),0);

		SendDlgItemMessage(hDlg, IDC_BUFLEN, CB_INSERTSTRING,0,(LPARAM)TEXT("Mix Interval * 1"));
		SendDlgItemMessage(hDlg, IDC_BUFLEN, CB_INSERTSTRING,1,(LPARAM)TEXT("Mix Interval * 2"));
		SendDlgItemMessage(hDlg, IDC_BUFLEN, CB_INSERTSTRING,2,(LPARAM)TEXT("Mix Interval * 4"));
		SendDlgItemMessage(hDlg, IDC_BUFLEN, CB_INSERTSTRING,3,(LPARAM)TEXT("Mix Interval * 8"));
		SendDlgItemMessage(hDlg, IDC_BUFLEN, CB_INSERTSTRING,4,(LPARAM)TEXT("Mix Interval * 16"));
		SendDlgItemMessage(hDlg, IDC_BUFLEN, CB_INSERTSTRING,5,(LPARAM)TEXT("Mix Interval * 32"));
		SendDlgItemMessage(hDlg, IDC_BUFLEN, CB_INSERTSTRING,6,(LPARAM)TEXT("Mix Interval * 64"));

		switch(set->SoundBufferSize)
		{
		case 2:temp=1; break;
		case 4:temp=2; break;
		case 8:temp=3; break;
		case 16:temp=4; break;
		case 32:temp=5; break;
		case 64: temp=6;break;
		case 1:
		default:temp=0;break;
		}
		SendDlgItemMessage(hDlg,IDC_BUFLEN,CB_SETCURSEL,temp,0);

		if(set->SixteenBitSound)
			SendDlgItemMessage(hDlg,IDC_16BIT,BM_SETCHECK,BST_CHECKED,0);
		if(set->Stereo)
			SendDlgItemMessage(hDlg,IDC_STEREO,BM_SETCHECK,BST_CHECKED,0);
		else EnableWindow(GetDlgItem(hDlg, IDC_REV_STEREO), FALSE);
		if(set->ReverseStereo)
			SendDlgItemMessage(hDlg,IDC_REV_STEREO,BM_SETCHECK,BST_CHECKED,0);
		set->AltSampleDecode = FALSE;
		EnableWindow(GetDlgItem(hDlg, IDC_ANTIRES), FALSE);
		//if(set->AltSampleDecode)
		//{
			EnableWindow(GetDlgItem(hDlg, IDC_CACHING), FALSE);
		//	SendDlgItemMessage(hDlg,IDC_ANTIRES,BM_SETCHECK,BST_CHECKED,0);
		//}
		if(set->Mute)
			SendDlgItemMessage(hDlg,IDC_MUTE,BM_SETCHECK,BST_CHECKED,0);
		if(GUI.FAMute)
			SendDlgItemMessage(hDlg,IDC_FAMT,BM_SETCHECK,BST_CHECKED,0);

		if(set->NextAPUEnabled)
		{
			EnableWindow(GetDlgItem(hDlg, IDC_SKIP_TYPE), FALSE);
			SendDlgItemMessage(hDlg,IDC_SPC700ON,BM_SETCHECK,BST_CHECKED,0);
			//Gray out the skip feature!
		}
		if(set->InterpolatedSound)
			SendDlgItemMessage(hDlg,IDC_LINEAR_INTER,BM_SETCHECK,BST_CHECKED,0);
		if(set->SoundSync)
			SendDlgItemMessage(hDlg,IDC_SYNC_TO_SOUND_CPU,BM_SETCHECK,BST_CHECKED,0);
		if(set->SoundEnvelopeHeightReading)
			SendDlgItemMessage(hDlg,IDC_ENVX,BM_SETCHECK,BST_CHECKED,0);
		if(set->FakeMuteFix)
			SendDlgItemMessage(hDlg,IDC_FMUT,BM_SETCHECK,BST_CHECKED,0);
		//if(set->UseWIPAPUTiming)
		//	SendDlgItemMessage(hDlg,IDC_WIP1,BM_SETCHECK,BST_CHECKED,0);
		if(!set->DisableSoundEcho)
			SendDlgItemMessage(hDlg,IDC_ECHO,BM_SETCHECK,BST_CHECKED,0);

//		if(!set->DisableSampleCaching)
//			SendDlgItemMessage(hDlg,IDC_CACHING,BM_SETCHECK,BST_CHECKED,0);
		if(!set->DisableMasterVolume)
			SendDlgItemMessage(hDlg,IDC_MASTER_VOL,BM_SETCHECK,BST_CHECKED,0);

		if(set->SoundDriver>0&&set->SoundDriver<4)
		{
			EnableWindow(GetDlgItem(hDlg, IDC_MIX), false);
			EnableWindow(GetDlgItem(hDlg, IDC_BUFLEN), false);
			//EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), false);
		}

		if(!IsDlgButtonChecked(hDlg,IDC_SPC700ON))
		{
			//EnableWindow(GetDlgItem(hDlg, IDC_CACHING), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_MUTE), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_FAMT), FALSE);
			//EnableWindow(GetDlgItem(hDlg, IDC_ANTIRES), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_SKIP_TYPE), TRUE);
			EnableWindow(GetDlgItem(hDlg, IDC_DRIVER), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_16BIT), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_ENVX), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_STEREO), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_MASTER_VOL), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_RATE), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_ECHO), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_LINEAR_INTER), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_REV_STEREO), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_BUFLEN), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_MIX), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), FALSE);
			EnableWindow(GetDlgItem(hDlg, IDC_FMUT), FALSE);
		}

		return true;
		case WM_PAINT:
		{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		};

		EndPaint (hDlg, &ps);
		}
		return true;
		case WM_COMMAND:
			switch(LOWORD(wParam))
			{
				case IDOK:
				{
					set->SoundDriver=SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETITEMDATA,
										SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETCURSEL, 0,0),0);
					set->SoundSkipMethod=(unsigned char)SendDlgItemMessage(hDlg,IDC_SKIP_TYPE,CB_GETCURSEL,0,0);
					set->SixteenBitSound=IsDlgButtonChecked(hDlg, IDC_16BIT);
					//set->AltSampleDecode=IsDlgButtonChecked(hDlg, IDC_ANTIRES);
					set->SoundSync=IsDlgButtonChecked(hDlg, IDC_SYNC_TO_SOUND_CPU);
					set->InterpolatedSound=IsDlgButtonChecked(hDlg, IDC_LINEAR_INTER);
					set->Stereo=IsDlgButtonChecked(hDlg, IDC_STEREO);
					set->ReverseStereo=IsDlgButtonChecked(hDlg, IDC_REV_STEREO);
					set->Mute=IsDlgButtonChecked(hDlg, IDC_MUTE);
					GUI.FAMute=IsDlgButtonChecked(hDlg, IDC_FAMT)!=0;
					set->NextAPUEnabled=IsDlgButtonChecked(hDlg, IDC_SPC700ON);
					set->SoundEnvelopeHeightReading=IsDlgButtonChecked(hDlg, IDC_ENVX);
					set->FakeMuteFix=IsDlgButtonChecked(hDlg, IDC_FMUT);

					set->DisableSoundEcho=(!IsDlgButtonChecked(hDlg, IDC_ECHO));
					//set->DisableSampleCaching=(!IsDlgButtonChecked(hDlg, IDC_CACHING));
					set->DisableMasterVolume=(!IsDlgButtonChecked(hDlg, IDC_MASTER_VOL));

					switch(SendDlgItemMessage(hDlg, IDC_RATE,CB_GETCURSEL,0,0))
					{
					case 0: set->SoundPlaybackRate=8000;	break;
					case 1: set->SoundPlaybackRate=11025;	break;
					case 2: set->SoundPlaybackRate=16000;	break;
					case 3: set->SoundPlaybackRate=22050;	break;
					case 4: set->SoundPlaybackRate=30000;	break;
					case 5: set->SoundPlaybackRate=32000;	break;
					case 6: set->SoundPlaybackRate=35000;	break;
					case 7: set->SoundPlaybackRate=44100;	break;
					case 8: set->SoundPlaybackRate=48000;	break;
					}

					set->SoundMixInterval=(10*(1+(SendDlgItemMessage(hDlg,IDC_MIX,CB_GETCURSEL,0,0))));

					set->SoundBufferSize=1<<SendDlgItemMessage(hDlg,IDC_BUFLEN,CB_GETCURSEL,0,0);

					{
						bool8 wasEnabled = set->APUEnabled;
						set->APUEnabled = set->NextAPUEnabled;

						WinSaveConfigFile();

						set->APUEnabled = wasEnabled;
					}

					// already done in WinProc on return
					// ReInitSound(1); 

				}	/* FALL THROUGH */

				case IDCANCEL:
					EndDialog(hDlg, 1);
					if(hBmp)
					{
						DeleteObject(hBmp);
						hBmp=NULL;
					}
					return true;

				case IDC_SPC700ON:
					if(BN_CLICKED==HIWORD(wParam)||BN_DBLCLK==HIWORD(wParam))
					{
						if(!IsDlgButtonChecked(hDlg,IDC_SPC700ON))
						{
							//EnableWindow(GetDlgItem(hDlg, IDC_CACHING), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_MUTE), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_FAMT), FALSE);
							//EnableWindow(GetDlgItem(hDlg, IDC_ANTIRES), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_SKIP_TYPE), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_DRIVER), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_16BIT), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_ENVX), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_STEREO), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_MASTER_VOL), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_RATE), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_ECHO), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_LINEAR_INTER), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_REV_STEREO), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_BUFLEN), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_MIX), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), FALSE);
							EnableWindow(GetDlgItem(hDlg, IDC_FMUT), FALSE);

						}
						else
						{
							EnableWindow(GetDlgItem(hDlg, IDC_LINEAR_INTER), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_ECHO), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_RATE), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_MASTER_VOL), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_STEREO), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_DRIVER), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_16BIT), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_ENVX), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_SKIP_TYPE), FALSE);
							//EnableWindow(GetDlgItem(hDlg, IDC_ANTIRES), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_MUTE), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_FAMT), TRUE);
							EnableWindow(GetDlgItem(hDlg, IDC_FMUT), TRUE);
							//if(!IsDlgButtonChecked(hDlg,IDC_ANTIRES))
							//	EnableWindow(GetDlgItem(hDlg, IDC_CACHING), TRUE);
							if(IsDlgButtonChecked(hDlg,IDC_STEREO))
								EnableWindow(GetDlgItem(hDlg, IDC_REV_STEREO), TRUE);
							int i = SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETITEMDATA,
										SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETCURSEL, 0,0),0);
							if(i<1||i>3)
							{
								//enable stuff.
								EnableWindow(GetDlgItem(hDlg, IDC_MIX), true);
								EnableWindow(GetDlgItem(hDlg, IDC_BUFLEN), true);
								//EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), true);

							}
							return true;
							//Check Driver States for other options.
						}
						return true;
					}
					else return false;
				case IDC_DRIVER:
					if(CBN_SELCHANGE==HIWORD(wParam))
					{
						//get index
						int i;
						i=SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETITEMDATA,
										SendDlgItemMessage(hDlg, IDC_DRIVER, CB_GETCURSEL, 0,0),0);
						if(i>0&&i<4)
						{
							EnableWindow(GetDlgItem(hDlg, IDC_MIX), false);
							EnableWindow(GetDlgItem(hDlg, IDC_BUFLEN), false);
							//EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), false);
						}
						//disable if index...
						else
						{
							//enable stuff.
							EnableWindow(GetDlgItem(hDlg, IDC_MIX), true);
							EnableWindow(GetDlgItem(hDlg, IDC_BUFLEN), true);
							//EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), true);

						}
						return true;
					}
					else return false;
				case IDC_ANTIRES:
					{
						if(BN_CLICKED==HIWORD(wParam)||BN_DBLCLK==HIWORD(wParam))
						{
							//if(!IsDlgButtonChecked(hDlg,IDC_ANTIRES))
							//{
							//	EnableWindow(GetDlgItem(hDlg, IDC_CACHING), TRUE);
							//}
							//else EnableWindow(GetDlgItem(hDlg, IDC_CACHING), FALSE);
							return true;

						}
						else return false;
					}
				case IDC_STEREO:
					{
						if(BN_CLICKED==HIWORD(wParam)||BN_DBLCLK==HIWORD(wParam))
						{
							if(IsDlgButtonChecked(hDlg,IDC_STEREO))
							{
								EnableWindow(GetDlgItem(hDlg, IDC_REV_STEREO), TRUE);
							}
							else EnableWindow(GetDlgItem(hDlg, IDC_REV_STEREO), FALSE);
							return true;

						}
						else return false;
					}
				default: return false;


		}
	}
	return false;
}

INT_PTR CALLBACK DlgSP7PackConfig(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("Gogo.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);

			if(LoadUp7110==&SPC7110Grab)
				SendDlgItemMessage(hDlg, IDC_SPC7110_SOME, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
			else if(LoadUp7110==&SPC7110Open)
				SendDlgItemMessage(hDlg, IDC_SPC7110_FILE, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
			else	SendDlgItemMessage(hDlg, IDC_SPC7110_ALL, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
			SendDlgItemMessage(hDlg, IDC_SPIN_CACHE, UDM_SETRANGE, 0, MAKELONG(20,1));
			SendDlgItemMessage(hDlg, IDC_SPIN_CACHE, UDM_SETPOS, 0, MAKELONG(cacheMegs,0));
			return true;
		}
	case WM_PAINT:
		{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
		}
		return true;
	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDOK:
				if(BST_CHECKED==SendDlgItemMessage(hDlg, IDC_SPC7110_ALL, BM_GETCHECK,0,0))
				{
					LoadUp7110=&SPC7110Load;
				}
				else if(BST_CHECKED==SendDlgItemMessage(hDlg, IDC_SPC7110_FILE, BM_GETCHECK,0,0))
				{
					LoadUp7110=&SPC7110Open;
				}
				else
				{
					LoadUp7110=&SPC7110Grab;
					cacheMegs=(uint16)SendDlgItemMessage(hDlg,IDC_SPIN_CACHE,UDM_GETPOS,0,0);
				}
				WinSaveConfigFile();
			case IDCANCEL:
				EndDialog(hDlg,0);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
			default: return false;
			}
		}
	default: return false;
	}
}

#ifdef RTC_DEBUGGER
INT_PTR CALLBACK SPC7110rtc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static int month;
	static int day;
	static int year;
	static int hour;
	static int minutes;
	static int seconds;
	static int dayinmonth;
	static struct SPC7110RTC* rtc;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();

		rtc= (struct SPC7110RTC *)lParam;

		seconds=rtc->reg[0]+rtc->reg[1]*10;
		minutes=rtc->reg[2]+rtc->reg[3]*10;
		hour=rtc->reg[4]+rtc->reg[5]*10;
		day=rtc->reg[6]+rtc->reg[7]*10;
		month=rtc->reg[8]+rtc->reg[9]*10;
		year=rtc->reg[10]+rtc->reg[11]*10;
		dayinmonth=S9xRTCDaysInMonth(month,year);


		SendDlgItemMessage(hDlg,IDC_MONTH, TBM_SETRANGE,(WPARAM)1,(LPARAM)MAKELONG(0,11));
		SendDlgItemMessage(hDlg,IDC_MONTH, TBM_SETTICFREQ,(WPARAM)5,0);
		SendDlgItemMessage(hDlg,IDC_MONTH, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)month-1);
		SetDlgItemInt(hDlg,IDC_LBLMONTH,month,0);

		SendDlgItemMessage(hDlg,IDC_DAY, TBM_SETRANGE,(WPARAM)1,(LPARAM)MAKELONG(0,dayinmonth-1));
		SendDlgItemMessage(hDlg,IDC_DAY, TBM_SETTICFREQ,(WPARAM)5,0);
		SendDlgItemMessage(hDlg,IDC_DAY, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)day-1);
		SetDlgItemInt(hDlg,IDC_LBLDAY,day,0);

		SendDlgItemMessage(hDlg,IDC_YEAR, TBM_SETRANGE,(WPARAM)1,(LPARAM)MAKELONG(0,99));
		SendDlgItemMessage(hDlg,IDC_YEAR, TBM_SETTICFREQ,(WPARAM)5,0);
		if(year<95)
		{
			SendDlgItemMessage(hDlg,IDC_YEAR, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)year+5);
			SetDlgItemInt(hDlg,IDC_LBLYEAR,year+2000,0);
		}
		else
		{
			SendDlgItemMessage(hDlg,IDC_YEAR, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)year-95);
			SetDlgItemInt(hDlg,IDC_LBLYEAR,year+1900,0);
		}

		SendDlgItemMessage(hDlg, IDC_HOUR, TBM_SETRANGE,(WPARAM)1,(LPARAM)MAKELONG(0,23));
		SendDlgItemMessage(hDlg,IDC_HOUR, TBM_SETTICFREQ,(WPARAM)5,0);
		SendDlgItemMessage(hDlg,IDC_HOUR, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)hour);
		SetDlgItemInt(hDlg,IDC_LBLHOUR,hour,0);

		SendDlgItemMessage(hDlg, IDC_MINUTE, TBM_SETRANGE,(WPARAM)1,(LPARAM)MAKELONG(0,59));
		SendDlgItemMessage(hDlg,IDC_MINUTE, TBM_SETTICFREQ,(WPARAM)5,0);
		SendDlgItemMessage(hDlg,IDC_MINUTE, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)minutes);
		SetDlgItemInt(hDlg,IDC_LBLMINUTE,minutes,0);

		SendDlgItemMessage(hDlg, IDC_SECOND, TBM_SETRANGE,(WPARAM)1,(LPARAM)MAKELONG(0,59));
		SendDlgItemMessage(hDlg,IDC_SECOND, TBM_SETTICFREQ,(WPARAM)5,0);
		SendDlgItemMessage(hDlg,IDC_SECOND, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)seconds);
		SetDlgItemInt(hDlg,IDC_LBLSECOND,seconds,0);



		if(rtc->reg[0x0D]&0x01)
			SendDlgItemMessage(hDlg,IDC_RTC_D1,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0D]&0x02)
			SendDlgItemMessage(hDlg,IDC_RTC_D2,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0D]&0x04)
			SendDlgItemMessage(hDlg,IDC_RTC_D4,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0D]&0x08)
			SendDlgItemMessage(hDlg,IDC_RTC_D8,BM_SETCHECK,BST_CHECKED,0);

		if(rtc->reg[0x0E]&0x01)
			SendDlgItemMessage(hDlg,IDC_RTC_E1,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0E]&0x02)
			SendDlgItemMessage(hDlg,IDC_RTC_E2,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0E]&0x04)
			SendDlgItemMessage(hDlg,IDC_RTC_E4,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0E]&0x08)
			SendDlgItemMessage(hDlg,IDC_RTC_E8,BM_SETCHECK,BST_CHECKED,0);

		if(rtc->reg[0x0F]&0x01)
			SendDlgItemMessage(hDlg,IDC_RTC_F1,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0F]&0x02)
			SendDlgItemMessage(hDlg,IDC_RTC_F2,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0F]&0x04)
			SendDlgItemMessage(hDlg,IDC_RTC_F4,BM_SETCHECK,BST_CHECKED,0);
		if(rtc->reg[0x0F]&0x08)
			SendDlgItemMessage(hDlg,IDC_RTC_F8,BM_SETCHECK,BST_CHECKED,0);

		return true;
	case  WM_COMMAND:
		switch(LOWORD(wParam))
		{
		case IDOK:
			rtc->reg[0x0]=seconds%10;
			rtc->reg[0x1]=seconds/10;
			rtc->reg[0x2]=minutes%10;
			rtc->reg[0x3]=minutes/10;
			rtc->reg[0x4]=hour%10;
			rtc->reg[0x5]=hour/10;
			rtc->reg[0x6]=day%10;
			rtc->reg[0x7]=day/10;
			rtc->reg[0x8]=month%10;
			rtc->reg[0x9]=month/10;
			rtc->reg[0xA]=year%10;
			rtc->reg[0xB]=year/10;

			if(IsDlgButtonChecked(hDlg, IDC_RTC_D1))
				rtc->reg[0x0D]|=0x01;
			else
				rtc->reg[0x0D]&=0x0E;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_D2))
				rtc->reg[0x0D]|=0x02;
			else
				rtc->reg[0x0D]&=0x0D;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_D4))
				rtc->reg[0x0D]|=0x04;
			else
				rtc->reg[0x0D]&=0x0B;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_D8))
				rtc->reg[0x0D]|=0x08;
			else
				rtc->reg[0x0D]&=0x07;

			if(IsDlgButtonChecked(hDlg, IDC_RTC_E1))
				rtc->reg[0x0E]|=0x01;
			else
				rtc->reg[0x0E]&=0x0E;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_E2))
				rtc->reg[0x0E]|=0x02;
			else
				rtc->reg[0x0E]&=0x0D;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_E4))
				rtc->reg[0x0E]|=0x04;
			else
				rtc->reg[0x0E]&=0x0B;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_E8))
				rtc->reg[0x0E]|=0x08;
			else
				rtc->reg[0x0E]&=0x07;

			if(IsDlgButtonChecked(hDlg, IDC_RTC_F1))
				rtc->reg[0x0F]|=0x01;
			else
				rtc->reg[0x0F]&=0x0E;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_F2))
				rtc->reg[0x0F]|=0x02;
			else
				rtc->reg[0x0F]&=0x0D;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_F4))
				rtc->reg[0x0F]|=0x04;
			else
				rtc->reg[0x0F]&=0x0B;
			if(IsDlgButtonChecked(hDlg, IDC_RTC_F8))
				rtc->reg[0x0F]|=0x08;
			else
				rtc->reg[0x0F]&=0x07;

			WinSaveConfigFile();

		case IDCANCEL:
			EndDialog(hDlg, 1);
			return true;
			/*case IDC_MONTH:
			int dinmonth;
			dinmonth = S9xRTCDaysInMonth(
			*/
		default: return false;
		}
		case WM_HSCROLL:
			month=1+SendDlgItemMessage(hDlg,IDC_MONTH,TBM_GETPOS,0,0);
			SetDlgItemInt(hDlg,IDC_LBLMONTH,month,0);
			day=1+SendDlgItemMessage(hDlg,IDC_DAY,TBM_GETPOS,0,0);
			year=SendDlgItemMessage(hDlg,IDC_YEAR,TBM_GETPOS,0,0);
			hour=SendDlgItemMessage(hDlg,IDC_HOUR,TBM_GETPOS,0,0);
			minutes=SendDlgItemMessage(hDlg,IDC_MINUTE,TBM_GETPOS,0,0);
			seconds=SendDlgItemMessage(hDlg,IDC_SECOND,TBM_GETPOS,0,0);
			SetDlgItemInt(hDlg,IDC_LBLHOUR,hour,0);
			SetDlgItemInt(hDlg,IDC_LBLMINUTE,minutes,0);
			SetDlgItemInt(hDlg,IDC_LBLSECOND,seconds,0);
			if(year<5)
			{
				year+=95;
				SetDlgItemInt(hDlg,IDC_LBLYEAR,year+1900,0);
			}
			else
			{
				year-=5;
				SetDlgItemInt(hDlg,IDC_LBLYEAR,year+2000,0);
			}
			dayinmonth=S9xRTCDaysInMonth(month,year);
			if(day>dayinmonth)
			{
				day=dayinmonth;
				SendDlgItemMessage(hDlg,IDC_DAY, TBM_SETPOS,(WPARAM)(BOOL)TRUE,(LPARAM)day-1);
			}
			SendDlgItemMessage(hDlg,IDC_DAY, TBM_SETRANGE,(WPARAM)1,(LPARAM)MAKELONG(0,dayinmonth-1));
			SetDlgItemInt(hDlg,IDC_LBLDAY,day,0);
			return true;

		default: return false;
	}

}
#endif

//  SetSelProc
//  Callback procedure to set the initial selection of the (folder) browser.
int CALLBACK SetSelProc( HWND hWnd, UINT uMsg, LPARAM lParam, LPARAM lpData )
{
    if (uMsg==BFFM_INITIALIZED) {
        ::SendMessage(hWnd, BFFM_SETSELECTION, TRUE, lpData );
    }
    return 0;
}

const char *StaticRAMBitSize ()
{
    static char tmp [20];

    sprintf (tmp, " (%dKbit)", 8*(Memory.SRAMMask + 1) / 1024);
    return (tmp);
}

INT_PTR CALLBACK DlgInfoProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("anomie.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			char temp[100];
			char romtext[4096];
			sprintf(romtext, "File: %s\r\nName: %s\r\n", Memory.ROMFilename, Memory.ROMName);
			sprintf(temp, "Speed: %02X/%s\r\nROM Map: %s\r\nType: %02x\r\n", Memory.ROMSpeed, ((Memory.ROMSpeed&0x10)!=0)?"FastROM":"SlowROM",(Memory.HiROM)?"HiROM":"LoROM",Memory.ROMType);
			strcat(romtext, temp);
			strcat(romtext, "Kart contents: ");
			strcat(romtext, Memory.KartContents ());
			strcat(romtext, "\r\nHeader ROM Size: ");
			strcat(romtext, Memory.Size());
			sprintf(temp, "\r\nCalculated ROM Size: %d Mbits", Memory.CalculatedSize/0x20000);
			strcat(romtext, temp);

			strcat(romtext, "\r\nSRAM size: ");
			strcat(romtext, Memory.StaticRAMSize ());
			strcat(romtext, StaticRAMBitSize());
			strcat(romtext, "\r\nActual Checksum: ");
			sprintf(temp, "%04X", Memory.CalculatedChecksum);
			strcat(romtext, temp);
			strcat(romtext, "\r\nHeader Checksum: ");
			sprintf(temp, "%04X", Memory.ROMChecksum);
			strcat(romtext, temp);
			strcat(romtext, "\r\nHeader Checksum Compliment: ");
			sprintf(temp, "%04X", Memory.ROMComplementChecksum);
			strcat(romtext, temp);
			strcat(romtext, "\r\nOutput: ");
			if(Memory.ROMRegion>12||Memory.ROMRegion<2)
				strcat(romtext, "NTSC 60Hz");
			else strcat(romtext, "PAL 50Hz");

			sprintf(temp, "\r\nCRC32:\t%08X", Memory.ROMCRC32);
			strcat(romtext, temp);


			strcat(romtext, "\r\nLicensee: ");

			int tmp=-1;
			//				sscanf(Memory.CompanyId, "%02X", &tmp);
			if(Memory.CompanyId[0]=='0')
				tmp=0;
			if(Memory.CompanyId[0]=='1')
				tmp=16;
			if(Memory.CompanyId[0]=='2')
				tmp=32;
			if(Memory.CompanyId[0]=='3')
				tmp=48;
			if(Memory.CompanyId[0]=='4')
				tmp=64;
			if(Memory.CompanyId[0]=='5')
				tmp=80;
			if(Memory.CompanyId[0]=='6')
				tmp=96;
			if(Memory.CompanyId[0]=='7')
				tmp=112;
			if(Memory.CompanyId[0]=='8')
				tmp=128;
			if(Memory.CompanyId[0]=='9')
				tmp=144;
			if(Memory.CompanyId[0]=='A')
				tmp=160;
			if(Memory.CompanyId[0]=='B')
				tmp=176;
			if(Memory.CompanyId[0]=='C')
				tmp=192;
			if(Memory.CompanyId[0]=='D')
				tmp=208;
			if(Memory.CompanyId[0]=='E')
				tmp=224;
			if(Memory.CompanyId[0]=='F')
				tmp=240;
			if(tmp!=-1)
			{
				if(Memory.CompanyId[1]=='0')
					tmp+=0;
				else if(Memory.CompanyId[1]=='1')
					tmp+=1;
				else if(Memory.CompanyId[1]=='2')
					tmp+=2;
				else if(Memory.CompanyId[1]=='3')
					tmp+=3;
				else if(Memory.CompanyId[1]=='4')
					tmp+=4;
				else if(Memory.CompanyId[1]=='5')
					tmp+=5;
				else if(Memory.CompanyId[1]=='6')
					tmp+=6;
				else if(Memory.CompanyId[1]=='7')
					tmp+=7;
				else if(Memory.CompanyId[1]=='8')
					tmp+=8;
				else if(Memory.CompanyId[1]=='9')
					tmp+=9;
				else if(Memory.CompanyId[1]=='A')
					tmp+=10;
				else if(Memory.CompanyId[1]=='B')
					tmp+=11;
				else if(Memory.CompanyId[1]=='C')
					tmp+=12;
				else if(Memory.CompanyId[1]=='D')
					tmp+=13;
				else if(Memory.CompanyId[1]=='E')
					tmp+=14;
				else if(Memory.CompanyId[1]=='F')
					tmp+=15;
				else tmp=0;
			}
			else tmp=0;
			if(tmp==0)
				tmp=(Memory.HiROM)?Memory.ROM[0x0FFDA]:Memory.ROM[0x7FDA];
			switch(tmp)
				//				switch(((Memory.ROMSpeed&0x0F)!=0)?Memory.ROM[0x0FFDA]:Memory.ROM[0x7FDA])
				//				switch(atoi(Memory.CompanyId))
				//				switch(((Memory.CompanyId[0]-'0')*16)+(Memory.CompanyId[1]-'0'))
			{
			case 0:strcat(romtext, "INVALID COMPANY");break;
			case 1:strcat(romtext, "Nintendo");break;
			case 2:strcat(romtext, "Ajinomoto");break;
			case 3:strcat(romtext, "Imagineer-Zoom");break;
			case 4:strcat(romtext, "Chris Gray Enterprises Inc.");break;
			case 5:strcat(romtext, "Zamuse");break;
			case 6:strcat(romtext, "Falcom");break;
			case 7:strcat(romtext, NOTKNOWN "7");break;
			case 8:strcat(romtext, "Capcom");break;
			case 9:strcat(romtext, "HOT-B");break;
			case 10:strcat(romtext, "Jaleco");break;
			case 11:strcat(romtext, "Coconuts");break;
			case 12:strcat(romtext, "Rage Software");break;
			case 13:strcat(romtext, "Micronet"); break; //Acc. ZFE
			case 14:strcat(romtext, "Technos");break;
			case 15:strcat(romtext, "Mebio Software");break;
			case 16:strcat(romtext, "SHOUEi System"); break; //Acc. ZFE
			case 17:strcat(romtext, "Starfish");break; //UCON 64
			case 18:strcat(romtext, "Gremlin Graphics");break;
			case 19:strcat(romtext, "Electronic Arts");break;
			case 20:strcat(romtext, "NCS / Masaya"); break; //Acc. ZFE
			case 21:strcat(romtext, "COBRA Team");break;
			case 22:strcat(romtext, "Human/Field");break;
			case 23:strcat(romtext, "KOEI");break;
			case 24:strcat(romtext, "Hudson Soft");break;
			case 25:strcat(romtext, "Game Village");break;//uCON64
			case 26:strcat(romtext, "Yanoman");break;
			case 27:strcat(romtext, NOTKNOWN "27");break;
			case 28:strcat(romtext, "Tecmo");break;
			case 29:strcat(romtext, NOTKNOWN "29");break;
			case 30:strcat(romtext, "Open System");break;
			case 31:strcat(romtext, "Virgin Games");break;
			case 32:strcat(romtext, "KSS");break;
			case 33:strcat(romtext, "Sunsoft");break;
			case 34:strcat(romtext, "POW");break;
			case 35:strcat(romtext, "Micro World");break;
			case 36:strcat(romtext, NOTKNOWN "36");break;
			case 37:strcat(romtext, NOTKNOWN "37");break;
			case 38:strcat(romtext, "Enix");break;
			case 39:strcat(romtext, "Loriciel/Electro Brain");break;//uCON64
			case 40:strcat(romtext, "Kemco");break;
			case 41:strcat(romtext, "Seta Co.,Ltd.");break;
			case 42:strcat(romtext, "Culture Brain"); break; //Acc. ZFE
			case 43:strcat(romtext, "Irem Japan");break;//Irem? Gun Force J
			case 44:strcat(romtext, "Pal Soft"); break; //Acc. ZFE
			case 45:strcat(romtext, "Visit Co.,Ltd.");break;
			case 46:strcat(romtext, "INTEC Inc."); break; //Acc. ZFE
			case 47:strcat(romtext, "System Sacom Corp."); break; //Acc. ZFE
			case 48:strcat(romtext, "Viacom New Media");break; //Zoop!
			case 49:strcat(romtext, "Carrozzeria");break;
			case 50:strcat(romtext, "Dynamic");break;
			case 51:strcat(romtext, "Nintendo");break;
			case 52:strcat(romtext, "Magifact");break;
			case 53:strcat(romtext, "Hect");break;
			case 54:strcat(romtext, NOTKNOWN "54");break;
			case 55:strcat(romtext, NOTKNOWN "55");break;
			case 56:strcat(romtext, "Capcom Europe");break;//Capcom? BOF2(E) MM7 (E)
			case 57:strcat(romtext, "Accolade Europe");break;//Accolade?Bubsy 2 (E)
			case 58:strcat(romtext, NOTKNOWN "58");break;
			case 59:strcat(romtext, "Arcade Zone");break;//uCON64
			case 60:strcat(romtext, "Empire Software");break;
			case 61:strcat(romtext, "Loriciel");break;
			case 62:strcat(romtext, "Gremlin Graphics"); break; //Acc. ZFE
			case 63:strcat(romtext, NOTKNOWN "63");break;
			case 64:strcat(romtext, "Seika Corp.");break;
			case 65:strcat(romtext, "UBI Soft");break;
			case 66:strcat(romtext, NOTKNOWN "66");break;
			case 67:strcat(romtext, NOTKNOWN "67");break;
			case 68:strcat(romtext, "LifeFitness Exertainment");break;//?? Exertainment Mountain Bike Rally (U).zip
			case 69:strcat(romtext, NOTKNOWN "69");break;
			case 70:strcat(romtext, "System 3");break;
			case 71:strcat(romtext, "Spectrum Holobyte");break;
			case 72:strcat(romtext, NOTKNOWN "72");break;
			case 73:strcat(romtext, "Irem");break;
			case 74:strcat(romtext, NOTKNOWN "74");break;
			case 75:strcat(romtext, "Raya Systems/Sculptured Software");break;
			case 76:strcat(romtext, "Renovation Products");break;
			case 77:strcat(romtext, "Malibu Games/Black Pearl");break;
			case 78:strcat(romtext, NOTKNOWN "78");break;
			case 79:strcat(romtext, "U.S. Gold");break;
			case 80:strcat(romtext, "Absolute Entertainment");break;
			case 81:strcat(romtext, "Acclaim");break;
			case 82:strcat(romtext, "Activision");break;
			case 83:strcat(romtext, "American Sammy");break;
			case 84:strcat(romtext, "GameTek");break;
			case 85:strcat(romtext, "Hi Tech Expressions");break;
			case 86:strcat(romtext, "LJN Toys");break;
			case 87:strcat(romtext, NOTKNOWN "87");break;
			case 88:strcat(romtext, NOTKNOWN "88");break;
			case 89:strcat(romtext, NOTKNOWN "89");break;
			case 90:strcat(romtext, "Mindscape");break;
			case 91:strcat(romtext, "Romstar, Inc."); break; //Acc. ZFE
			case 92:strcat(romtext, NOTKNOWN "92");break;
			case 93:strcat(romtext, "Tradewest");break;
			case 94:strcat(romtext, NOTKNOWN "94");break;
			case 95:strcat(romtext, "American Softworks Corp.");break;
			case 96:strcat(romtext, "Titus");break;
			case 97:strcat(romtext, "Virgin Interactive Entertainment");break;
			case 98:strcat(romtext, "Maxis");break;
			case 99:strcat(romtext, "Origin/FCI/Pony Canyon");break;//uCON64
			case 100:strcat(romtext, NOTKNOWN "100");break;
			case 101:strcat(romtext, NOTKNOWN "101");break;
			case 102:strcat(romtext, NOTKNOWN "102");break;
			case 103:strcat(romtext, "Ocean");break;
			case 104:strcat(romtext, NOTKNOWN "104");break;
			case 105:strcat(romtext, "Electronic Arts");break;
			case 106:strcat(romtext, NOTKNOWN "106");break;
			case 107:strcat(romtext, "Laser Beam");break;
			case 108:strcat(romtext, NOTKNOWN "108");break;
			case 109:strcat(romtext, NOTKNOWN "109");break;
			case 110:strcat(romtext, "Elite");break;
			case 111:strcat(romtext, "Electro Brain");break;
			case 112:strcat(romtext, "Infogrames");break;
			case 113:strcat(romtext, "Interplay");break;
			case 114:strcat(romtext, "LucasArts");break;
			case 115:strcat(romtext, "Parker Brothers");break;
			case 116:strcat(romtext, "Konami");break;//uCON64
			case 117:strcat(romtext, "STORM");break;
			case 118:strcat(romtext, NOTKNOWN "118");break;
			case 119:strcat(romtext, NOTKNOWN "119");break;
			case 120:strcat(romtext, "THQ Software");break;
			case 121:strcat(romtext, "Accolade Inc.");break;
			case 122:strcat(romtext, "Triffix Entertainment");break;
			case 123:strcat(romtext, NOTKNOWN "123");break;
			case 124:strcat(romtext, "Microprose");break;
			case 125:strcat(romtext, NOTKNOWN "125");break;
			case 126:strcat(romtext, NOTKNOWN "126");break;
			case 127:strcat(romtext, "Kemco");break;
			case 128:strcat(romtext, "Misawa");break;
			case 129:strcat(romtext, "Teichio");break;
			case 130:strcat(romtext, "Namco Ltd.");break;
			case 131:strcat(romtext, "Lozc");break;
			case 132:strcat(romtext, "Koei");break;
			case 133:strcat(romtext, NOTKNOWN "133");break;
			case 134:strcat(romtext, "Tokuma Shoten Intermedia");break;
			case 135:strcat(romtext, "Tsukuda Original"); break; //Acc. ZFE
			case 136:strcat(romtext, "DATAM-Polystar");break;
			case 137:strcat(romtext, NOTKNOWN "137");break;
			case 138:strcat(romtext, NOTKNOWN "138");break;
			case 139:strcat(romtext, "Bullet-Proof Software");break;
			case 140:strcat(romtext, "Vic Tokai");break;
			case 141:strcat(romtext, NOTKNOWN "141");break;
			case 142:strcat(romtext, "Character Soft");break;
			case 143:strcat(romtext, "I\'\'Max");break;
			case 144:strcat(romtext, "Takara");break;
			case 145:strcat(romtext, "CHUN Soft");break;
			case 146:strcat(romtext, "Video System Co., Ltd.");break;
			case 147:strcat(romtext, "BEC");break;
			case 148:strcat(romtext, NOTKNOWN "148");break;
			case 149:strcat(romtext, "Varie");break;
			case 150:strcat(romtext, "Yonezawa / S'Pal Corp."); break; //Acc. ZFE
			case 151:strcat(romtext, "Kaneco");break;
			case 152:strcat(romtext, NOTKNOWN "152");break;
			case 153:strcat(romtext, "Pack in Video");break;
			case 154:strcat(romtext, "Nichibutsu");break;
			case 155:strcat(romtext, "TECMO");break;
			case 156:strcat(romtext, "Imagineer Co.");break;
			case 157:strcat(romtext, NOTKNOWN "157");break;
			case 158:strcat(romtext, NOTKNOWN "158");break;
			case 159:strcat(romtext, NOTKNOWN "159");break;
			case 160:strcat(romtext, "Telenet");break;
			case 161:strcat(romtext, "Hori"); break; //Acc. uCON64
			case 162:strcat(romtext, NOTKNOWN "162");break;
			case 163:strcat(romtext, NOTKNOWN "163");break;
			case 164:strcat(romtext, "Konami");break;
			case 165:strcat(romtext, "K.Amusement Leasing Co.");break;
			case 166:strcat(romtext, NOTKNOWN "166");break;
			case 167:strcat(romtext, "Takara");break;
			case 168:strcat(romtext, NOTKNOWN "168");break;
			case 169:strcat(romtext, "Technos Jap.");break;
			case 170:strcat(romtext, "JVC");break;
			case 171:strcat(romtext, NOTKNOWN "171");break;
			case 172:strcat(romtext, "Toei Animation");break;
			case 173:strcat(romtext, "Toho");break;
			case 174:strcat(romtext, NOTKNOWN "174");break;
			case 175:strcat(romtext, "Namco Ltd.");break;
			case 176:strcat(romtext, "Media Rings Corp."); break; //Acc. ZFE
			case 177:strcat(romtext, "ASCII Co. Activison");break;
			case 178:strcat(romtext, "Bandai");break;
			case 179:strcat(romtext, NOTKNOWN "179");break;
			case 180:strcat(romtext, "Enix America");break;
			case 181:strcat(romtext, NOTKNOWN "181");break;
			case 182:strcat(romtext, "Halken");break;
			case 183:strcat(romtext, NOTKNOWN "183");break;
			case 184:strcat(romtext, NOTKNOWN "184");break;
			case 185:strcat(romtext, NOTKNOWN "185");break;
			case 186:strcat(romtext, "Culture Brain");break;
			case 187:strcat(romtext, "Sunsoft");break;
			case 188:strcat(romtext, "Toshiba EMI");break;
			case 189:strcat(romtext, "Sony Imagesoft");break;
			case 190:strcat(romtext, NOTKNOWN "190");break;
			case 191:strcat(romtext, "Sammy");break;
			case 192:strcat(romtext, "Taito");break;
			case 193:strcat(romtext, NOTKNOWN "193");break;
			case 194:strcat(romtext, "Kemco");break;
			case 195:strcat(romtext, "Square");break;
			case 196:strcat(romtext, "Tokuma Soft");break;
			case 197:strcat(romtext, "Data East");break;
			case 198:strcat(romtext, "Tonkin House");break;
			case 199:strcat(romtext, NOTKNOWN "199");break;
			case 200:strcat(romtext, "KOEI");break;
			case 201:strcat(romtext, NOTKNOWN "201");break;
			case 202:strcat(romtext, "Konami USA");break;
			case 203:strcat(romtext, "NTVIC");break;
			case 204:strcat(romtext, NOTKNOWN "204");break;
			case 205:strcat(romtext, "Meldac");break;
			case 206:strcat(romtext, "Pony Canyon");break;
			case 207:strcat(romtext, "Sotsu Agency/Sunrise");break;
			case 208:strcat(romtext, "Disco/Taito");break;
			case 209:strcat(romtext, "Sofel");break;
			case 210:strcat(romtext, "Quest Corp.");break;
			case 211:strcat(romtext, "Sigma");break;
			case 212:strcat(romtext, "Ask Kodansha Co., Ltd."); break; //Acc. ZFE
			case 213:strcat(romtext, NOTKNOWN "213");break;
			case 214:strcat(romtext, "Naxat");break;
			case 215:strcat(romtext, NOTKNOWN "215");break;
			case 216:strcat(romtext, "Capcom Co., Ltd.");break;
			case 217:strcat(romtext, "Banpresto");break;
			case 218:strcat(romtext, "Tomy");break;
			case 219:strcat(romtext, "Acclaim");break;
			case 220:strcat(romtext, NOTKNOWN "220");break;
			case 221:strcat(romtext, "NCS");break;
			case 222:strcat(romtext, "Human Entertainment");break;
			case 223:strcat(romtext, "Altron");break;
			case 224:strcat(romtext, "Jaleco");break;
			case 225:strcat(romtext, NOTKNOWN "225");break;
			case 226:strcat(romtext, "Yutaka");break;
			case 227:strcat(romtext, NOTKNOWN "227");break;
			case 228:strcat(romtext, "T&ESoft");break;
			case 229:strcat(romtext, "EPOCH Co.,Ltd.");break;
			case 230:strcat(romtext, NOTKNOWN "230");break;
			case 231:strcat(romtext, "Athena");break;
			case 232:strcat(romtext, "Asmik");break;
			case 233:strcat(romtext, "Natsume");break;
			case 234:strcat(romtext, "King Records");break;
			case 235:strcat(romtext, "Atlus");break;
			case 236:strcat(romtext, "Sony Music Entertainment");break;
			case 237:strcat(romtext, NOTKNOWN "237");break;
			case 238:strcat(romtext, "IGS");break;
			case 239:strcat(romtext, NOTKNOWN "239");break;
			case 240:strcat(romtext, NOTKNOWN "240");break;
			case 241:strcat(romtext, "Motown Software");break;
			case 242:strcat(romtext, "Left Field Entertainment");break;
			case 243:strcat(romtext, "Beam Software");break;
			case 244:strcat(romtext, "Tec Magik");break;
			case 245:strcat(romtext, NOTKNOWN "245");break;
			case 246:strcat(romtext, NOTKNOWN "246");break;
			case 247:strcat(romtext, NOTKNOWN "247");break;
			case 248:strcat(romtext, NOTKNOWN "248");break;
			case 249:strcat(romtext, "Cybersoft");break;
			case 250:strcat(romtext, NOTKNOWN "250");break;
			case 251:strcat(romtext, "Psygnosis"); break; //Acc. ZFE
			case 252:strcat(romtext, NOTKNOWN "252");break;
			case 253:strcat(romtext, NOTKNOWN "253");break;
			case 254:strcat(romtext, "Davidson"); break; //Acc. uCON64
			case 255:strcat(romtext, NOTKNOWN "255");break;
			default:strcat(romtext, NOTKNOWN);break;
				}

				strcat(romtext, "\r\nROM Version: ");
				sprintf(temp, "1.%d", (Memory.HiROM)?Memory.ROM[0x0FFDB]:Memory.ROM[0x7FDB]);
				strcat(romtext, temp);
				strcat(romtext, "\r\nRegion: ");
				switch(Memory.ROMRegion)
				{
				case 0:
					strcat(romtext, "Japan");
					break;
				case 1:
					strcat(romtext, "USA/Canada");
					break;
				case 2:
					strcat(romtext, "Oceania, Europe, and Asia");
					break;
				case 3:
					strcat(romtext, "Sweden");
					break;
				case 4:
					strcat(romtext, "Finland");
					break;
				case 5:
					strcat(romtext, "Denmark");
					break;
				case 6:
					strcat(romtext, "France");
					break;
				case 7:
					strcat(romtext, "Holland");
					break;
				case 8:
					strcat(romtext, "Spain");
					break;
				case 9:
					strcat(romtext, "Germany, Austria, and Switzerland");
					break;
				case 10:
					strcat(romtext, "Italy");
					break;
				case 11:
					strcat(romtext, "Hong Kong and China");
					break;
				case 12:
					strcat(romtext, "Indonesia");
					break;
				case 13:
					strcat(romtext, "South Korea");
					break;
				case 14:strcat(romtext, "Unknown region 14");break;
				default:strcat(romtext, "Unknown region 15");break;
				}
				SendDlgItemMessage(hDlg, IDC_ROM_DATA, WM_SETTEXT, 0, (LPARAM)romtext);
				return true;
				break;
			}
			case WM_CTLCOLORSTATIC:

				if(GUI.InfoColor!=WIN32_WHITE)
				{
					SetTextColor((HDC)wParam, GUI.InfoColor);
					SetBkColor((HDC)wParam, RGB(0,0,0));
				}
				return true;break;
		case WM_PAINT:
		{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
		}
		return true;

			case WM_COMMAND:
				{
					switch(LOWORD(wParam))
					{
					case IDOK:
					case IDCANCEL:
						EndDialog(hDlg, 0);
						if(hBmp)
						{
							DeleteObject(hBmp);
							hBmp=NULL;
						}
						return true;
						break;
					default: return false; break;
					}
				}
			default:return false;
	}
}

INT_PTR CALLBACK DlgAboutProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;

	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			TCHAR buf[2048];//find better way of dealing.
			sprintf(buf,TEXT(DISCLAIMER_TEXT),SNES9X_NAME_AND_VERSION);
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("RedChaos1.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			SetDlgItemText(hDlg, IDC_DISCLAIMER, buf);
			SetWindowText(hDlg, TEXT(ABOUT_DIALOG_TITLE) TEXT(APP_NAME));
		}
		return true;
	case WM_PAINT:
		{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
		}
		return true;
	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDOK:
			case IDCANCEL:
				EndDialog(hDlg, 0);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				return true;
				break;
			default: return false; break;
			}
		}
	default:return false;
	}
}

INT_PTR CALLBACK DlgEmulatorProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static char paths[8][MAX_PATH];
	static int which = 0;
	static HBITMAP hBmp;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("MKendora.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			SetWindowText(hDlg, TEXT(EMUSET_TITLE));
			SetDlgItemText(hDlg, IDC_LABEL_FREEZE, EMUSET_LABEL_DIRECTORY);
			SetDlgItemText(hDlg, IDOK, BUTTON_OK);
			SetDlgItemText(hDlg, IDCANCEL, BUTTON_CANCEL);
			SetDlgItemText(hDlg, IDC_LABEL_ASRAM, EMUSET_LABEL_ASRAM);
			SetDlgItemText(hDlg, IDC_LABEL_ASRAM_TEXT, EMUSET_LABEL_ASRAM_TEXT);
			SetDlgItemText(hDlg, IDC_BROWSE, EMUSET_BROWSE);
			SetDlgItemText(hDlg, IDC_CUSTOM_FOLDER_FIELD, GUI.FreezeFileDir);
			extern bool using_conf_not_cfg;
			SetDlgItemText(hDlg, IDC_CONFIG_NAME_BOX, using_conf_not_cfg ? "snes9x.conf" : "snes9x.cfg");
			SendDlgItemMessage(hDlg, IDC_SRAM_SPIN, UDM_SETRANGE, 0, MAKELPARAM((short)99, (short)0));
			SendDlgItemMessage(hDlg, IDC_SRAM_SPIN,UDM_SETPOS,0, Settings.AutoSaveDelay);

			int inum = 0;
			strcpy(paths[inum++],GUI.RomDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_ROMS);
			strcpy(paths[inum++],GUI.ScreensDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_SCREENS);
			strcpy(paths[inum++],GUI.MovieDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_MOVIES);
			strcpy(paths[inum++],GUI.SPCDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_SPCS);
			strcpy(paths[inum++],GUI.FreezeFileDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_SAVES);
			strcpy(paths[inum++],GUI.SRAMFileDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_SRAM);
			strcpy(paths[inum++],GUI.PatchDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_PATCHESANDCHEATS);
			strcpy(paths[inum++],GUI.BiosDir);
			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)SETTINGS_OPTION_DIRECTORY_BIOS);

			SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_SETCURSEL,(WPARAM)0,0);
 			SetDlgItemText(hDlg, IDC_CUSTOM_FOLDER_FIELD, paths[0]);
			which = 0;

			SetCurrentDirectory(S9xGetDirectory(DEFAULT_DIR));
		}
		case WM_PAINT:
		{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
		}
		return true;
	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDC_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					TCHAR path[MAX_PATH];
					_fullpath(path, paths[which], MAX_PATH);
					TCHAR title[]=TEXT(SETTINGS_TITLE_SELECTFOLDER);
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					bi.lpfn = SetSelProc;
					bi.lParam = (LPARAM)(LPCSTR) path;
					iidl=SHBrowseForFolder(&bi);
					if(iidl) SHGetPathFromIDList(iidl, path);

					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					absToRel(paths[which], path, S9xGetDirectory(DEFAULT_DIR));
 					SetDlgItemText(hDlg, IDC_CUSTOM_FOLDER_FIELD, paths[which]);
				}
				break;
			case IDC_CUSTOM_FOLDER_FIELD:
				which = SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_GETCURSEL,0,0);
				GetDlgItemText(hDlg, IDC_CUSTOM_FOLDER_FIELD, paths[which], MAX_PATH);
				break;
			case IDC_DIRCOMBO:
				which = SendDlgItemMessage(hDlg,IDC_DIRCOMBO,CB_GETCURSEL,0,0);
 				SetDlgItemText(hDlg, IDC_CUSTOM_FOLDER_FIELD, paths[which]);
				break;
			case IDOK:
				{
					int inum = 0;
					strcpy(GUI.RomDir,paths[inum++]);
					strcpy(GUI.ScreensDir,paths[inum++]);
					strcpy(GUI.MovieDir,paths[inum++]);
					strcpy(GUI.SPCDir,paths[inum++]);
					strcpy(GUI.FreezeFileDir,paths[inum++]);
					strcpy(GUI.SRAMFileDir,paths[inum++]);
					strcpy(GUI.PatchDir,paths[inum++]);
					strcpy(GUI.BiosDir,paths[inum++]);

					Settings.AutoSaveDelay=SendDlgItemMessage(hDlg, IDC_SRAM_SPIN, UDM_GETPOS, 0,0);

					WinSaveConfigFile();
				}
				/* fall through */
			case IDCANCEL:
				EndDialog(hDlg, 0);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				return true;
				break;
			default: return false; break;
			}
		}
	default:return false;
	}
}
INT_PTR CALLBACK DlgSeekProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	char frame_number_text[MAX_PATH];
        int frame_dest;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("ThisPortIsAMess.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			sprintf(frame_number_text, "%d .. %d", (int)S9xMovieGetFrameCounter() + 1, (int)S9xMovieGetLength());
			SetDlgItemText(hDlg, IDC_FRAME_NUMBER, frame_number_text);
		}
		return true;

		case WM_PAINT:
		{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
		}
		return true;
	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDOK:
				GetDlgItemText(hDlg, IDC_FRAME_NUMBER, frame_number_text, MAX_PATH);
                                frame_dest = atoi(frame_number_text);
                                if (frame_dest > (int)S9xMovieGetFrameCounter() && frame_dest <= (int)S9xMovieGetLength())
                                {
					Settings.HighSpeedSeek = frame_dest - S9xMovieGetFrameCounter();
					Settings.Paused = false;
					Settings.FrameAdvance = false;
					GUI.FrameAdvanceJustPressed = 0;
                                }
			case IDCANCEL:
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				EndDialog(hDlg, 0);
				return true;
				break;
			default: return false; break;
				}
			}
		default:return false;
	}
}

#define SKIP_FLOPPY

bool ExtensionIsValid(const TCHAR * filename)
{
	ExtList* curr=valid_ext;
	while(curr!=NULL)
	{
		if(curr->extension==NULL)
		{
			if(NULL==strstr(filename, TEXT(".")))
				return true;
		}
		else if(filename[(strlen(filename)-1)-strlen(curr->extension)]=='.')
		{
			if(0==_strnicmp(&filename[(strlen(filename))-strlen(curr->extension)],
				curr->extension, strlen(curr->extension)))
				return true;
		}
		curr=curr->next;
	}
	return false;
}

bool IsCompressed(const TCHAR* filename)
{
	ExtList* curr=valid_ext;
	while(curr!=NULL)
	{
		if(curr->extension==NULL)
		{
			if(NULL==strstr(filename, TEXT(".")))
				return curr->compressed;
		}
		else if(filename[(strlen(filename)-1)-strlen(curr->extension)]=='.')
		{
			if(0==_strnicmp(&filename[(strlen(filename))-strlen(curr->extension)],
				curr->extension, strlen(curr->extension)))
				return curr->compressed;
		}
		curr=curr->next;
	}
	return false;
}

inline bool AllASCII(char *b, int size)
{
	for (int i = 0; i < size; i++)
	{
		if (b[i] < 32 || b[i] > 126)
		{
			return(false);
		}
	}
	return(true);
}

inline int InfoScore(char *Buffer)
{
	int score = 0;
	if (Buffer[28] + (Buffer[29] << 8) +
		Buffer[30] + (Buffer[31] << 8) == 0xFFFF)
	{	score += 3; }

	if (Buffer[26] == 0x33) { score += 2; }
	if ((Buffer[21] & 0xf) < 4) {	score += 2; }
	if (!(Buffer[61] & 0x80)) { score -= 4; }
	if ((1 << (Buffer[23] - 7)) > 48) { score -= 1; }
	if (Buffer[25] < 14) { score += 1; }
	if (!AllASCII(Buffer, 20)) { score -= 1; }

	return (score);
}

inline unsigned short sum(unsigned char *array, unsigned int size = HEADER_SIZE)
{
	register unsigned short theSum = 0;
	for (register unsigned int i = 0; i < size; i++)
	{
		theSum += array[i];
	}
	return(theSum);
}

void rominfo(const TCHAR *filename, TCHAR *namebuffer, TCHAR *sizebuffer)
{
	strcpy(namebuffer, ROM_ITEM_DESCNOTAVAILABLE);
	strcpy(sizebuffer, "? Mbits");

	if(IsCompressed(filename))
	{
		unzFile uf = unzOpen(filename);
		if(uf)
		{
			unz_file_info info;
			if(UNZ_OK == unzGetCurrentFileInfo(uf, &info, 0,0,0,0,0,0))
			{
				if (info.uncompressed_size < 0x8000) // Smaller than a block
					strcpy(namebuffer, ROM_ITEM_NOTAROM);
				else
					strcpy(namebuffer, ROM_ITEM_COMPRESSEDROMDESCRIPTION);

				// should subtract header size, so this may be slightly off, but it's better than "? MBits"
				double MBitD = (double)(info.uncompressed_size - 0) / 0x100000 * 8;
				int MBitI = (int)MBitD;
				int sizeIndex;
				if(0!=(MBitI / 10))
				{
					sizebuffer[0] = MBitI / 10 + '0';
					sizeIndex = 1;
				}
				else
					sizeIndex = 0;
				sizebuffer[sizeIndex+0] = MBitI % 10 + '0';
				sizebuffer[sizeIndex+1] = '.';
				sizebuffer[sizeIndex+2] = (char)((MBitD - MBitI) * 10) + '0';
				sizebuffer[sizeIndex+3] = (char)((int)((MBitD - MBitI) * 100) % 10) + '0';
				sizebuffer[sizeIndex+4] = ' ';
				sizebuffer[sizeIndex+5] = 'M';
				sizebuffer[sizeIndex+6] = 'b';
				sizebuffer[sizeIndex+7] = 'i';
				sizebuffer[sizeIndex+8] = 't';
				sizebuffer[sizeIndex+9] = '\0';
			}
			unzClose(uf);
		}
		return;
	}

	struct stat filestats;
	stat(filename, &filestats);

	int HeaderSize = 0;

	if (filestats.st_size >= 0x8000)
	{
		ifstream ROMFile(filename, ios::in | ios::binary);
		if (ROMFile)
		{
			int HasHeadScore = 0, NoHeadScore = 0,
				HeadRemain = filestats.st_size & 0x7FFF;

			switch(HeadRemain)
			{
			case 0:
				NoHeadScore += 3;
				break;

			case HEADER_SIZE:
				HasHeadScore += 2;
				break;
			}

			unsigned char HeaderBuffer[HEADER_SIZE];
			ROMFile.read((char *)HeaderBuffer, HEADER_SIZE);

			if (sum(HeaderBuffer) < 2500) { HasHeadScore += 2; }

			//SMC/SWC Header
			if (HeaderBuffer[8] == 0xAA &&
				HeaderBuffer[9] == 0xBB &&
				HeaderBuffer[10]== 4)
			{ HasHeadScore += 3; }
			//FIG Header
			else if ((HeaderBuffer[4] == 0x77 && HeaderBuffer[5] == 0x83) ||
				(HeaderBuffer[4] == 0xDD && HeaderBuffer[5] == 0x82) ||
				(HeaderBuffer[4] == 0xDD && HeaderBuffer[5] == 2) ||
				(HeaderBuffer[4] == 0xF7 && HeaderBuffer[5] == 0x83) ||
				(HeaderBuffer[4] == 0xFD && HeaderBuffer[5] == 0x82) ||
				(HeaderBuffer[4] == 0x00 && HeaderBuffer[5] == 0x80) ||
				(HeaderBuffer[4] == 0x47 && HeaderBuffer[5] == 0x83) ||
				(HeaderBuffer[4] == 0x11 && HeaderBuffer[5] == 2))
			{ HasHeadScore += 2; }
			else if (!strncmp("GAME DOCTOR SF 3", (char *)HeaderBuffer, 16))
			{ HasHeadScore += 5; }

			HeaderSize = HasHeadScore > NoHeadScore ? HEADER_SIZE : 0;

			bool EHi = false;
			if (filestats.st_size - HeaderSize >= 0x500000)
			{
				ROMFile.seekg(0x40FFC0 + HeaderSize, ios::beg);
				ROMFile.read((char *)HeaderBuffer, INFO_LEN);
				if (InfoScore((char *)HeaderBuffer) > 1)
				{
					EHi = true;
					strncpy(namebuffer, (char *)HeaderBuffer, 21);
				}
			}

			if (!EHi)
			{
				if (filestats.st_size - HeaderSize >= 0x10000)
				{
					char LoHead[INFO_LEN], HiHead[INFO_LEN];

					ROMFile.seekg(0x7FC0 + HeaderSize, ios::beg);
					ROMFile.read(LoHead, INFO_LEN);
					int LoScore = InfoScore(LoHead);

					ROMFile.seekg(0xFFC0 + HeaderSize, ios::beg);
					ROMFile.read(HiHead, INFO_LEN);
					int HiScore = InfoScore(HiHead);

					strncpy(namebuffer, LoScore > HiScore ? LoHead : HiHead, 21);

					if (filestats.st_size - HeaderSize >= 0x20000)
					{
						ROMFile.seekg((filestats.st_size - HeaderSize) / 2 + 0x7FC0 + HeaderSize, ios::beg);
						ROMFile.read(LoHead, INFO_LEN);
						int IntLScore = InfoScore(LoHead) / 2;

						if (IntLScore > LoScore && IntLScore > HiScore)
						{
							strncpy(namebuffer, LoHead, 21);
						}
					}
				}
				else //ROM only has one block
				{
					ROMFile.seekg(0x7FC0 + HeaderSize, ios::beg);
					ROMFile.read(namebuffer, 21);
				}
			}
			ROMFile.close();
		}
		else //Couldn't open file
		{
			strcpy(namebuffer, ROM_ITEM_CANTOPEN);
		}
	}
	else //Smaller than a block
	{
		strcpy(namebuffer, ROM_ITEM_NOTAROM);
	}

	double MBitD = (double)(filestats.st_size - HeaderSize) / 0x100000 * 8;
	int MBitI = (int)MBitD;
	int sizeIndex;
	if(0!=(MBitI / 10))
	{
		sizebuffer[0] = MBitI / 10 + '0';
		sizeIndex = 1;
	}
	else
		sizeIndex = 0;
	sizebuffer[sizeIndex+0] = MBitI % 10 + '0';
	sizebuffer[sizeIndex+1] = '.';
	sizebuffer[sizeIndex+2] = (char)((MBitD - MBitI) * 10) + '0';
	sizebuffer[sizeIndex+3] = (char)((int)((MBitD - MBitI) * 100) % 10) + '0';
	sizebuffer[sizeIndex+4] = ' ';
	sizebuffer[sizeIndex+5] = 'M';
	sizebuffer[sizeIndex+6] = 'b';
	sizebuffer[sizeIndex+7] = 'i';
	sizebuffer[sizeIndex+8] = 't';
	sizebuffer[sizeIndex+9] = '\0';
	namebuffer[21] = '\0';
}

void GetPathFromTree( HWND hDlg, UINT tree, TCHAR* selected, HTREEITEM hItem)
{
	TVITEM tv;
	TCHAR temp[MAX_PATH];
	temp[0]='\0';
	ZeroMemory(&tv, sizeof(TVITEM));
	HTREEITEM hTreeTemp=hItem;

	if(tv.iImage==7)
	{
		tv.mask=TVIF_HANDLE|TVIF_IMAGE;
		tv.hItem=hTreeTemp;
		tv.iImage=6;
		TreeView_SetItem(GetDlgItem(hDlg, tree),&tv);
		ZeroMemory(&tv, sizeof(TVITEM));
	}

	tv.mask=TVIF_HANDLE|TVIF_TEXT;
	tv.hItem=hTreeTemp;
	tv.pszText=temp;
	tv.cchTextMax =MAX_PATH;
	TreeView_GetItem(GetDlgItem(hDlg, tree), &tv);

	sprintf(selected, TEXT("%s"), temp);
	while(TreeView_GetParent(GetDlgItem(hDlg, tree), hTreeTemp))
	{
		temp[0]='\0';
		hTreeTemp=TreeView_GetParent(GetDlgItem(hDlg, tree), hTreeTemp);
		tv.mask=TVIF_HANDLE|TVIF_TEXT;
		tv.hItem=hTreeTemp;
		tv.pszText=temp;
		tv.cchTextMax =MAX_PATH;
		TreeView_GetItem(GetDlgItem(hDlg, tree), &tv);
		sprintf(temp, TEXT("%s\\%s"),temp, selected);
		strcpy(selected, temp);
	}
}

typedef struct RomDataCacheNode
{
	char* fname;
	char* rname;
	char* rmbits;
	struct RomDataCacheNode* next;
} RomDataList;

void ClearCacheList(RomDataList* rdl)
{
	RomDataList* temp=rdl;
	RomDataList* temp2=NULL;
	if(rdl==NULL)
		return;
	do
	{
		temp2=temp->next;
		if(temp->fname)
			delete [] temp->fname;
		if(temp->rmbits)
			delete [] temp->rmbits;
		if(temp->rname)
			delete [] temp->rname;
		delete temp;
		temp=temp2;
	}
	while(temp!=NULL);
}


void ExpandDir(char * selected, HTREEITEM hParent, HWND hDlg)
{
	TCHAR temp[MAX_PATH];
	WIN32_FIND_DATA wfd;
	ZeroMemory(&wfd, sizeof(WIN32_FIND_DATA));
	strcat(selected, TEXT("\\*"));
	HANDLE hFind=FindFirstFile(selected,&wfd);
	selected[(strlen(selected)-1)]='\0';

	do
	{
		if(wfd.dwFileAttributes&FILE_ATTRIBUTE_DIRECTORY)
		{
			if(strcmp(wfd.cFileName, TEXT("."))&&strcmp(wfd.cFileName, TEXT("..")))
			{
				//skip these, add the rest.
				TV_INSERTSTRUCT tvis;
				ZeroMemory(&tvis, sizeof(TV_INSERTSTRUCT));
				tvis.hParent=hParent;
				tvis.hInsertAfter=TVI_SORT;
				tvis.item.mask = TVIF_STATE | TVIF_TEXT | TVIF_IMAGE | TVIF_SELECTEDIMAGE;
				tvis.item.pszText=wfd.cFileName;
				tvis.item.cchTextMax=MAX_PATH;
				const bool locked = (wfd.dwFileAttributes&(FILE_ATTRIBUTE_READONLY|FILE_ATTRIBUTE_ENCRYPTED|FILE_ATTRIBUTE_OFFLINE))!=0;
				const bool hidden = (wfd.dwFileAttributes&(FILE_ATTRIBUTE_HIDDEN))!=0;
				tvis.item.iImage=hidden?9:(locked?8:7);
				tvis.item.iSelectedImage=locked?8:6;
				HTREEITEM hNewTree=TreeView_InsertItem(GetDlgItem(hDlg, IDC_ROM_DIR),&tvis);

				strcpy(temp, selected);
				strcat(temp, wfd.cFileName);
				strcat(temp, TEXT("\\*"));

				bool subdir=false;
				WIN32_FIND_DATA wfd2;
				ZeroMemory(&wfd2, sizeof(WIN32_FIND_DATA));
				HANDLE hFind2=FindFirstFile(temp,&wfd2);
				do
				{
					if(wfd2.dwFileAttributes&FILE_ATTRIBUTE_DIRECTORY)
					{
						if(strcmp(wfd2.cFileName, TEXT("."))&&strcmp(wfd2.cFileName, TEXT("..")))
						{
							subdir=true;
						}
					}
				}
				while(FindNextFile(hFind2, &wfd2)&&!subdir);

				if(subdir)
				{
					TV_INSERTSTRUCT tvis;
					ZeroMemory(&tvis, sizeof(TV_INSERTSTRUCT));
					tvis.hParent=hNewTree;
					tvis.hInsertAfter=TVI_SORT;
					TreeView_InsertItem(GetDlgItem(hDlg, IDC_ROM_DIR),&tvis);

				}
				FindClose(hFind2);

			}
		}
	}
	while(FindNextFile(hFind, &wfd));

	FindClose(hFind);
	//scan for folders
}



void ListFilesFromFolder(HWND hDlg, RomDataList** prdl)
{
	RomDataList* rdl= *prdl;
	RomDataList* current=NULL;
	int count=0;
	TVITEM tv;
	TCHAR temp[MAX_PATH];
	TCHAR selected[MAX_PATH]; // directory path
	temp[0]='\0';
	ZeroMemory(&tv, sizeof(TVITEM));
	HTREEITEM hTreeItem=TreeView_GetSelection(GetDlgItem(hDlg, IDC_ROM_DIR));

	GetPathFromTree(hDlg, IDC_ROM_DIR, selected, hTreeItem);

	SendDlgItemMessage(hDlg, IDC_ROMLIST, WM_SETREDRAW, FALSE, 0);
	ListView_DeleteAllItems(GetDlgItem(hDlg, IDC_ROMLIST));
	ClearCacheList(rdl);
	rdl=NULL;
	//Add items here.

	WIN32_FIND_DATA wfd;
	ZeroMemory(&wfd, sizeof(WIN32_FIND_DATA));

	strcat(selected, TEXT("\\*"));

	HANDLE hFind=FindFirstFile(selected, &wfd);
	selected[(strlen(selected)-1)]='\0';
	do
	{
		if(wfd.dwFileAttributes&FILE_ATTRIBUTE_DIRECTORY)
			continue;
		if(ExtensionIsValid(wfd.cFileName))
		{
			RomDataList* newitem=new RomDataList;
			ZeroMemory(newitem, sizeof(RomDataList));
			newitem->fname=new char[1+strlen(wfd.cFileName)];
			strcpy(newitem->fname, wfd.cFileName);

			// hide ntldr and no-name files
			if(!newitem->fname || !*newitem->fname || (!strcmp(newitem->fname, "ntldr") && strlen(selected)<4))
				continue;

			// too small to be a ROM
			if (wfd.nFileSizeLow < 0x8000 && !IsCompressed(wfd.cFileName))
				continue;

			count++;

			if(!rdl)
				rdl=newitem;
			else
			{
				if(0>stricmp(newitem->fname,rdl->fname))
				{
					newitem->next=rdl;
					rdl=newitem;
				}
				else
				{
					RomDataList* trail=rdl;
					current=rdl->next;
					while(current!=NULL&&0<stricmp(newitem->fname,current->fname))
					{
						trail=current;
						current=current->next;
					}
					newitem->next=current;
					trail->next=newitem;
				}
			}
		}
	}
	while(FindNextFile(hFind, &wfd));

	FindClose(hFind);

		SendDlgItemMessage(hDlg, IDC_ROMLIST, WM_SETREDRAW, TRUE, 0);
	*prdl=rdl;
	ListView_SetItemCountEx (GetDlgItem(hDlg, IDC_ROMLIST), count, 0);
	ListView_SetItemState (GetDlgItem(hDlg,IDC_ROMLIST), 0, LVIS_SELECTED|LVIS_FOCUSED,LVIS_SELECTED|LVIS_FOCUSED);
}

// load multicart rom dialog
INT_PTR CALLBACK DlgMultiROMProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch(msg)
	{
	case WM_INITDIALOG:{
		if(DirectDraw.Clipped) S9xReRefresh();
		TCHAR path[MAX_PATH];
		SetCurrentDirectory(S9xGetDirectory(BIOS_DIR));
		_fullpath(path, "stbios.bin", MAX_PATH);
		SetDlgItemText(hDlg, IDC_MULTICART_BIOSEDIT, path);
		FILE* ftemp = fopen(path, "rb");
		if(ftemp)
		{
			fclose(ftemp);
			SetDlgItemText(hDlg, IDC_MULTICART_BIOSNOTFOUND, MULTICART_BIOS_FOUND);
		}
		else
			SetDlgItemText(hDlg, IDC_MULTICART_BIOSNOTFOUND, MULTICART_BIOS_NOT_FOUND);
		SetDlgItemText(hDlg, IDC_MULTICART_EDITA, multiRomA);
		SetDlgItemText(hDlg, IDC_MULTICART_EDITB, multiRomB);
		break;}
	case WM_COMMAND:
		{
			char rom1[MAX_PATH]={0}, rom2[MAX_PATH]={0};
			SetCurrentDirectory(S9xGetDirectory(ROM_DIR));
			switch(LOWORD(wParam))
			{
			case IDOK:
				GetDlgItemText(hDlg, IDC_MULTICART_EDITA, multiRomA, MAX_PATH);
				GetDlgItemText(hDlg, IDC_MULTICART_EDITB, multiRomB, MAX_PATH);
				if(*multiRomA) _fullpath(multiRomA, multiRomA, MAX_PATH);
				if(*multiRomB) _fullpath(multiRomB, multiRomB, MAX_PATH);
				EndDialog(hDlg, 1);
				return true;
			case IDCANCEL:
				EndDialog(hDlg, 0);
				return true;
			case IDC_MULTICART_SWAP:
				GetDlgItemText(hDlg, IDC_MULTICART_EDITA, rom2, MAX_PATH);
				GetDlgItemText(hDlg, IDC_MULTICART_EDITB, rom1, MAX_PATH);
				if(*rom1) _fullpath(rom1, rom1, MAX_PATH);
				if(*rom2) _fullpath(rom2, rom2, MAX_PATH);
				SetDlgItemText(hDlg, IDC_MULTICART_EDITA, rom1);
				SetDlgItemText(hDlg, IDC_MULTICART_EDITB, rom2);
				break;
			case IDC_MULTICART_BROWSEA:
				if(!DoOpenRomDialog(rom1, true))
					break;
				_fullpath(rom1, rom1, MAX_PATH);
				SetDlgItemText(hDlg, IDC_MULTICART_EDITA, rom1);
				break;
			case IDC_MULTICART_BROWSEB:
				if(!DoOpenRomDialog(rom2, true))
					break;
				_fullpath(rom2, rom2, MAX_PATH);
				SetDlgItemText(hDlg, IDC_MULTICART_EDITB, rom2);
				break;
			case IDC_MULTICART_CLEARA:
				rom1[0] = '\0';
				SetDlgItemText(hDlg, IDC_MULTICART_EDITA, rom1);
				break;
			case IDC_MULTICART_CLEARB:
				rom1[1] = '\0';
				SetDlgItemText(hDlg, IDC_MULTICART_EDITB, rom2);
				break;
			}
		}
	}
	return false;
}

INT_PTR CALLBACK DlgOpenROMProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	int rv=0;
	static HWND hSplit;
	static HIMAGELIST hIcons;
	static TCHAR *filename;
	static RomDataList* rdl;
	static int selectionMarkOverride = -1;
	static bool initDone = false;
	static RomDataList* nextInvalidatedROM = NULL;
	static int nextInvalidatedROMCounter = 0;
	static HWND romList = NULL;
	static HWND dirList = NULL;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			initDone = false;

			// suppress annoying "no disk in drive" errors
			SetErrorMode(SEM_FAILCRITICALERRORS);

			romList = GetDlgItem(hDlg,IDC_ROMLIST);
			dirList = GetDlgItem(hDlg,IDC_ROM_DIR);
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("Koji.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			filename=(TCHAR*)lParam;
			RECT treeRect;
			RECT listRect;
			WNDCLASSEX wcex;
			TCHAR tempclassname[]=TEXT("S9xSplitter");
			ZeroMemory(&wcex, sizeof(WNDCLASSEX));
			wcex.cbSize=sizeof(WNDCLASSEX);
			wcex.hInstance=g_hInst;
			wcex.lpfnWndProc=DlgChildSplitProc;
			wcex.lpszClassName=tempclassname;
			wcex.hbrBackground=(HBRUSH)GetStockObject(LTGRAY_BRUSH);
			wcex.hCursor=LoadCursor(NULL, IDC_SIZEWE);
///			wcex.hCursor=LoadCursor(NULL, MAKEINTRESOURCE(IDC_SIZEWE));
///			ATOM aSplitter=RegisterClassEx(&wcex);
			GetWindowRect(dirList, &treeRect);
			GetWindowRect(romList, &listRect);
			POINT p;

			ListView_SetExtendedListViewStyle(romList, LVS_EX_FULLROWSELECT);

			p.x=treeRect.right;
			p.y=treeRect.top;
			ScreenToClient(hDlg, &p);
			hSplit=CreateWindow(TEXT("S9xSplitter"), TEXT(""),WS_CHILD|WS_VISIBLE , p.x, p.y, listRect.left-treeRect.right , listRect.bottom-listRect.top, hDlg,NULL, g_hInst,0);

			LVCOLUMN col;
			static const LPSTR temp1 = TEXT(ROM_COLUMN_FILENAME);
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=0;
			col.cx=196;
			col.cchTextMax=5;
			col.pszText=temp1;

			ListView_InsertColumn(romList,    0,   &col);

			static const LPSTR temp2 = TEXT(ROM_COLUMN_DESCRIPTION);
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH|LVCF_SUBITEM;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=1;
			col.cx=112;
			col.cchTextMax=32;
			col.pszText=temp2;
			col.iSubItem=1;

			ListView_InsertColumn(romList,    1,   &col);


			static const LPSTR temp3 = TEXT(ROM_COLUMN_SIZE);
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH|LVCF_SUBITEM;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=2;
			col.cx=67;
			col.cchTextMax=32;
			col.pszText=temp3;
			col.iSubItem=2;

			ListView_InsertColumn(romList,    2,   &col);


			SendDlgItemMessage(hDlg, IDC_MEM_TYPE,CB_INSERTSTRING,0,(LPARAM)TEXT(ROM_OPTION_AUTODETECT));
			SendDlgItemMessage(hDlg, IDC_MEM_TYPE,CB_INSERTSTRING,1,(LPARAM)TEXT(ROM_OPTION_FORCEHIROM));
			SendDlgItemMessage(hDlg, IDC_MEM_TYPE,CB_INSERTSTRING,2,(LPARAM)TEXT(ROM_OPTION_FORCELOROM));
			SendDlgItemMessage(hDlg, IDC_MEM_TYPE,CB_SETCURSEL,0,0);

			SendDlgItemMessage(hDlg, IDC_VIDEO_MODE,CB_INSERTSTRING,0,(LPARAM)TEXT(ROM_OPTION_AUTODETECT));
			SendDlgItemMessage(hDlg, IDC_VIDEO_MODE,CB_INSERTSTRING,1,(LPARAM)TEXT(ROM_OPTION_FORCEPAL));
			SendDlgItemMessage(hDlg, IDC_VIDEO_MODE,CB_INSERTSTRING,2,(LPARAM)TEXT(ROM_OPTION_FORCENTSC));
			SendDlgItemMessage(hDlg, IDC_VIDEO_MODE,CB_SETCURSEL,0,0);

			SendDlgItemMessage(hDlg, IDC_HEADER,CB_INSERTSTRING,0,(LPARAM)TEXT(ROM_OPTION_AUTODETECT));
			SendDlgItemMessage(hDlg, IDC_HEADER,CB_INSERTSTRING,1,(LPARAM)TEXT(ROM_OPTION_FORCEHEADER));
			SendDlgItemMessage(hDlg, IDC_HEADER,CB_INSERTSTRING,2,(LPARAM)TEXT(ROM_OPTION_FORCENOHEADER));
			SendDlgItemMessage(hDlg, IDC_HEADER,CB_SETCURSEL,0,0);

			SendDlgItemMessage(hDlg, IDC_INTERLEAVE,CB_INSERTSTRING,0,(LPARAM)TEXT(ROM_OPTION_AUTODETECT));
			SendDlgItemMessage(hDlg, IDC_INTERLEAVE,CB_INSERTSTRING,1,(LPARAM)TEXT(ROM_OPTION_NONINTERLEAVED));
			SendDlgItemMessage(hDlg, IDC_INTERLEAVE,CB_INSERTSTRING,2,(LPARAM)TEXT(ROM_OPTION_MODE1));
			SendDlgItemMessage(hDlg, IDC_INTERLEAVE,CB_INSERTSTRING,3,(LPARAM)TEXT(ROM_OPTION_MODE2));
			SendDlgItemMessage(hDlg, IDC_INTERLEAVE,CB_INSERTSTRING,4,(LPARAM)TEXT(ROM_OPTION_GD24));
			SendDlgItemMessage(hDlg, IDC_INTERLEAVE,CB_SETCURSEL,0,0);

			hIcons=ImageList_Create(16,16,ILC_COLOR24,10,10);

			HANDLE hBitmap;

#define ADD_IMAGE(IDB_NAME) \
			hBitmap=LoadImage(g_hInst, MAKEINTRESOURCE(IDB_NAME), IMAGE_BITMAP, 0,0, LR_DEFAULTCOLOR|LR_CREATEDIBSECTION); \
			ImageList_Add(hIcons, (HBITMAP)hBitmap, NULL); \
			DeleteObject(hBitmap);

			ADD_IMAGE(IDB_HARDDRIVE); // 0
			ADD_IMAGE(IDB_CDDRIVE); // 1
			ADD_IMAGE(IDB_NETDRIVE); // 2
			ADD_IMAGE(IDB_REMOVABLE); // 3
			ADD_IMAGE(IDB_RAMDISK); // 4
			ADD_IMAGE(IDB_UNKNOWN); // 5
			ADD_IMAGE(IDB_OPENFOLDER); // 6
			ADD_IMAGE(IDB_CLOSEDFOLDER); // 7
			ADD_IMAGE(IDB_LOCKEDFOLDER); // 8
			ADD_IMAGE(IDB_HIDDENFOLDER); // 9

			TreeView_SetImageList(dirList, hIcons, TVSIL_NORMAL);

//			DWORD dw;
			TCHAR buffer[MAX_PATH];
			TCHAR blah[MAX_PATH];
			long result=ERROR_SUCCESS/*-1*/;
			HTREEITEM hTreeDrive=NULL;

			char drive [_MAX_DRIVE + 1];
			strcpy (drive,"C:\\");


			_fullpath (buffer, S9xGetDirectory(ROM_DIR), MAX_PATH);
			_splitpath (buffer, drive, NULL, NULL, NULL);

			DWORD driveMask=GetLogicalDrives();

#ifndef SKIP_FLOPPY
			for (int i=0;i<26;i++)
#else
				for (int i=2;i<26;i++)
#endif
				{
					if(driveMask&(1<<i))
					{
						TCHAR driveName[4];
						driveName[0]='A'+i;
						driveName[1]=':';
						driveName[2]='\\';
						driveName[3]='\0';
						UINT driveType=GetDriveType(driveName);
						driveName[2]='\0';

						TVINSERTSTRUCT tvis;
						ZeroMemory(&tvis, sizeof(TVINSERTSTRUCT));

						tvis.hParent=NULL;
						tvis.hInsertAfter=TVI_ROOT;
						tvis.item.mask=TVIF_TEXT|TVIF_IMAGE|TVIF_SELECTEDIMAGE;

						switch(driveType)
						{
							case DRIVE_FIXED:     tvis.item.iSelectedImage=tvis.item.iImage=0; break;
							case DRIVE_CDROM:     tvis.item.iSelectedImage=tvis.item.iImage=1; break;
							case DRIVE_REMOTE:    tvis.item.iSelectedImage=tvis.item.iImage=2; break;
							case DRIVE_REMOVABLE: tvis.item.iSelectedImage=tvis.item.iImage=3; break;
							case DRIVE_RAMDISK:   tvis.item.iSelectedImage=tvis.item.iImage=4; break;
							default:              tvis.item.iSelectedImage=tvis.item.iImage=5; break;
						}

						tvis.item.pszText=driveName;

						HTREEITEM hTwee=TreeView_InsertItem(dirList,&tvis);

						if(result==ERROR_SUCCESS && !strncasecmp(drive, driveName, 2))
							hTreeDrive=hTwee;

						TCHAR temp[10];
						strcpy(temp, driveName);
						strcat(temp, TEXT("\\*"));
						bool subdir=false;

						if(driveType==DRIVE_REMOVABLE || driveType == DRIVE_CDROM || driveType == DRIVE_UNKNOWN)
						{
								TV_INSERTSTRUCT tvis;
								ZeroMemory(&tvis, sizeof(TV_INSERTSTRUCT));
								tvis.hParent=hTwee;
								tvis.hInsertAfter=TVI_SORT;
								TreeView_InsertItem(dirList,&tvis);

						}
						else
						{
							WIN32_FIND_DATA wfd2;
							ZeroMemory(&wfd2, sizeof(WIN32_FIND_DATA));
							HANDLE hFind2=FindFirstFile(temp,&wfd2);
							do
							{
								if(wfd2.dwFileAttributes&FILE_ATTRIBUTE_DIRECTORY)
								{
									if(strcmp(wfd2.cFileName, TEXT("."))&&strcmp(wfd2.cFileName, TEXT("..")))
									{
										subdir=true;
									}
								}
							}
							while(FindNextFile(hFind2, &wfd2)&&!subdir);

							if(subdir)
							{
								TV_INSERTSTRUCT tvis;
								ZeroMemory(&tvis, sizeof(TV_INSERTSTRUCT));
								tvis.hParent=hTwee;
								tvis.hInsertAfter=TVI_SORT;
								TreeView_InsertItem(dirList,&tvis);
							}
							FindClose(hFind2);
						}
					}
				}

				SendDlgItemMessage(hDlg, IDC_ROM_DIR, WM_SETREDRAW, FALSE, 0);

				if(result==ERROR_SUCCESS)
				{
					HTREEITEM hTreePrev;//,hTreeRoot;
				//	hTreePrev=TreeView_GetRoot(dirList);
				//	hTreeRoot=hTreeDrive;
					hTreePrev=hTreeDrive;
					HTREEITEM hTemp=hTreePrev;
					TCHAR* temp=buffer;
					TCHAR* temp2, * temp3;

					do
					{
						temp2=strstr(temp, TEXT("\\"));
						temp3=strstr(temp, TEXT("/"));
						if(temp3 && temp3 < temp2)
							temp2 = temp3;

						TVITEM tvi;
						ZeroMemory(&tvi, sizeof(TVITEM));
						tvi.mask=TVIF_TEXT;
						tvi.pszText=blah;
						tvi.cchTextMax=MAX_PATH;
						blah[0]='\0';

						if(temp2)
							*temp2='\0';

						tvi.hItem=hTemp;
						TreeView_GetItem(dirList, &tvi);

						if(strcasecmp(blah, temp) != 0)
						{
							do
							{
								tvi.mask=TVIF_TEXT;
								tvi.pszText=blah;
								tvi.cchTextMax=MAX_PATH;
								hTemp=TreeView_GetNextSibling(dirList, hTemp);
								tvi.hItem=hTemp;
								TreeView_GetItem(dirList, &tvi);
							}
							while((hTemp != NULL) && (strcasecmp(blah, temp) != 0));

							if(hTemp!=NULL)
							{
								hTreePrev=hTemp;

								TreeView_SelectItem(dirList, hTreePrev);
								TreeView_EnsureVisible(dirList, hTreePrev);
								if(temp2)
									TreeView_Expand(dirList, hTreePrev, TVE_EXPAND);

								hTemp=TreeView_GetChild(dirList, hTreePrev);
							}
						}
						else
						{
							TreeView_SelectItem(dirList, hTemp);
							TreeView_EnsureVisible(dirList, hTemp);
							if(temp2)
								TreeView_Expand(dirList, hTemp, TVE_EXPAND);

							hTemp=TreeView_GetChild(dirList, hTemp);
						}
						if(temp2)
							temp=temp2+1;
						else
							temp=NULL;
					}
					while(temp);

					if(Memory.ROMFilename[0]!='\0')
					{
						LVFINDINFO lvfi;
						ZeroMemory(&lvfi, sizeof(LVFINDINFO));
						TCHAR *tmp, *tmp2;
						tmp=Memory.ROMFilename;
						while(tmp2=strstr(tmp, TEXT("\\")))
							tmp=tmp2+1;

						lvfi.flags=LVFI_STRING;
						lvfi.psz=tmp;

						int idx=ListView_FindItem(romList, -1, &lvfi);
						ListView_SetSelectionMark(romList, idx);
						ListView_SetItemState(romList, idx, LVIS_SELECTED|LVIS_FOCUSED,LVIS_FOCUSED|LVIS_SELECTED);
						ListView_EnsureVisible(romList, idx, FALSE);

					}
					SendDlgItemMessage(hDlg, IDC_ROM_DIR, WM_SETREDRAW, TRUE, 0);
				}
				initDone = true;

				ListView_EnsureVisible (romList, (int)SendMessage(romList, LVM_GETNEXTITEM, (WPARAM)-1, LVNI_SELECTED), FALSE);

				// start up the WM_TIMER event
				nextInvalidatedROM = rdl;
				nextInvalidatedROMCounter = 0;
				SetTimer(hDlg,42,600,NULL);

				return true; //true sets the keyboard focus, in case we need this elsewhere
		}
		case WM_TIMER:
			{
				if(!initDone || !nextInvalidatedROM || !rdl)
					return false;

				// see if current selection needs filling in, and skip to that if so
				int selected = (int)SendMessage(romList, LVM_GETNEXTITEM, (WPARAM)-1, LVNI_SELECTED);
				if(selected>=0)
				{
					RomDataList* curr=rdl;
					for(int i=0;i<selected;i++)
						if(curr) curr=curr->next;
					if(curr && !curr->rname)
					{
						nextInvalidatedROM = curr;
						nextInvalidatedROMCounter = selected;
					}
				}

				LVHITTESTINFO lvhi;
				lvhi.flags = LVHT_ONITEM;
				lvhi.iItem = 0;
				lvhi.iSubItem = 0;
				lvhi.pt.x = 0;
				lvhi.pt.y = 0;
				ListView_HitTest(romList, &lvhi);
				int firstVisibleItem = lvhi.iItem+1;
				int lastVisibleItem = firstVisibleItem+20;

				// skip up to 100 things that don't need updating
				bool enteredValid = false;
				if(nextInvalidatedROM->rname || nextInvalidatedROMCounter<firstVisibleItem || nextInvalidatedROMCounter>lastVisibleItem)
				for(int i = 0 ; i < 100 ; i++)
					if(nextInvalidatedROM->rname || (!enteredValid && (nextInvalidatedROMCounter<firstVisibleItem || nextInvalidatedROMCounter>lastVisibleItem)))
					{
						if(!enteredValid && nextInvalidatedROMCounter>=firstVisibleItem && nextInvalidatedROMCounter<lastVisibleItem)
							enteredValid = true;
						nextInvalidatedROM = nextInvalidatedROM->next;
						if(nextInvalidatedROM)
							nextInvalidatedROMCounter++;
						else
						{
							nextInvalidatedROM = rdl;
							nextInvalidatedROMCounter = 0;
						}
					}

				// update 1 item, if it needs updating
				if(!nextInvalidatedROM->rname)
				{
					TCHAR path[MAX_PATH];
					TCHAR buffer[32];
					TCHAR buffer2[32];
					GetPathFromTree(hDlg, IDC_ROM_DIR, path, TreeView_GetSelection(dirList));
					strcat(path, "\\");
					strcat(path, nextInvalidatedROM->fname);
					rominfo(path, buffer, buffer2);
					nextInvalidatedROM->rname=new char[strlen(buffer)+1];
					strcpy(nextInvalidatedROM->rname, buffer);
					nextInvalidatedROM->rmbits=new char[strlen(buffer2)+1];
					strcpy(nextInvalidatedROM->rmbits, buffer2);

					ListView_RedrawItems(romList,nextInvalidatedROMCounter,nextInvalidatedROMCounter);
				}

				// next timer
				nextInvalidatedROM = nextInvalidatedROM->next;
				if(nextInvalidatedROM)
					nextInvalidatedROMCounter++;
				else
				{
					nextInvalidatedROM = rdl;
					nextInvalidatedROMCounter = 0;
				}
				SetTimer(hDlg,42,600,NULL);
				return true;
			}
	case WM_PAINT:
		{
			PAINTSTRUCT ps;
			BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
		}
		return true;

	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDOK:
				{
					LVITEM lvi;
					ZeroMemory(&lvi, sizeof(LVITEM));
					//get selections
					int list_index = selectionMarkOverride == -1 ? ListView_GetSelectionMark(romList) : selectionMarkOverride;
					if(list_index!=-1 && (int)SendMessage(romList, LVM_GETNEXTITEM, (WPARAM)-1, LVNI_SELECTED)!=-1)
					{
						rv=1;
						TCHAR temp[MAX_PATH];
						temp[0]='\0';
						lvi.iItem=list_index;
						lvi.mask=LVIF_TEXT;
						lvi.pszText=filename;
						lvi.cchTextMax=MAX_PATH;
						ListView_GetItem(romList, &lvi);

						strcpy(temp, filename);

						HTREEITEM hTreeTemp=TreeView_GetSelection(dirList);
						TVITEM tv;
						ZeroMemory(&tv, sizeof(TVITEM));

						tv.mask=TVIF_HANDLE|TVIF_TEXT;
						tv.hItem=hTreeTemp;
						tv.pszText=temp;
						tv.cchTextMax =MAX_PATH;
						TreeView_GetItem(dirList, &tv);
						sprintf(temp, TEXT("%s\\%s"), temp, filename);

						strcpy(filename, temp);

						while(TreeView_GetParent(dirList, hTreeTemp)!=NULL)
						{
							temp[0]='\0';
							hTreeTemp=TreeView_GetParent(dirList, hTreeTemp);
							tv.mask=TVIF_HANDLE|TVIF_TEXT;
							tv.hItem=hTreeTemp;
							tv.pszText=temp;
							tv.cchTextMax =MAX_PATH;
							TreeView_GetItem(dirList, &tv);
							sprintf(temp, TEXT("%s\\%s"),temp, filename);
							strcpy(filename, temp);
						}

						int iTemp=SendDlgItemMessage(hDlg, IDC_MEM_TYPE,CB_GETCURSEL,0,0);

						Settings.ForceHiROM=Settings.ForceLoROM=FALSE;
						if(iTemp==1)
							Settings.ForceHiROM=TRUE;
						else if(iTemp==2)
							Settings.ForceLoROM=TRUE;

						iTemp=SendDlgItemMessage(hDlg, IDC_INTERLEAVE,CB_GETCURSEL,0,0);

						Settings.ForceNotInterleaved=Settings.ForceInterleaved=Settings.ForceInterleaved2=Settings.ForceInterleaveGD24=FALSE;
						if(iTemp==1)
							Settings.ForceNotInterleaved=TRUE;
						else if(iTemp==2)
							Settings.ForceInterleaved=TRUE;
						else if(iTemp==3)
							Settings.ForceInterleaved2=TRUE;
						else if(iTemp==4)
							Settings.ForceInterleaveGD24=TRUE;

						iTemp=SendDlgItemMessage(hDlg, IDC_VIDEO_MODE,CB_GETCURSEL,0,0);

						Settings.ForceNTSC=Settings.ForcePAL=FALSE;
						if(iTemp==1)
							Settings.ForcePAL=TRUE;
						else if(iTemp==2)
							Settings.ForceNTSC=TRUE;


						iTemp=SendDlgItemMessage(hDlg, IDC_HEADER,CB_GETCURSEL,0,0);

						Settings.ForceNoHeader=Settings.ForceHeader=FALSE;
						if(iTemp==1)
							Settings.ForceHeader=TRUE;
						else if(iTemp==2)
							Settings.ForceNoHeader=TRUE;

						strcpy(temp, filename);
						int i=strlen(temp);
						while(temp[i]!='\\' && temp[i]!='/')
						{
							temp[i]='\0';
							i--;
						}
						temp[i]='\0';

						if(!GUI.LockDirectories)
							absToRel(GUI.RomDir, temp, S9xGetDirectory(DEFAULT_DIR));
					}
					else
					{
						return false;
					}
				}
			case IDCANCEL:
				EndDialog(hDlg, rv);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				ClearCacheList(rdl);
				rdl=NULL;
				DestroyWindow(hSplit);
				UnregisterClass(TEXT("S9xSplitter"), g_hInst);
				TreeView_DeleteAllItems(dirList);
				ListView_DeleteAllItems(romList);
				return true;
				break;
			default: return false; break;
			}
		}
	case WM_NOTIFY:
		{
			if(lParam == 0)
				return false;
			NMHDR* pNmh=(NMHDR*)lParam;
			static int foundItemOverride = -1;
			switch(pNmh->idFrom)
			{
			case IDC_ROMLIST:
				{
					switch(pNmh->code)
					{
					// allow typing in a ROM filename (or part of it) to jump to it
					// necessary to implement ourselves because Windows doesn't provide
					// this functionality for virtual (owner data) lists such as this
					case LVN_ODFINDITEM:
						{
							LRESULT pResult;

							// pNMHDR has information about the item we should find
							// In pResult we should save which item that should be selected
							NMLVFINDITEM* pFindInfo = (NMLVFINDITEM*)lParam;

							/* pFindInfo->iStart is from which item we should search.
							We search to bottom, and then restart at top and will stop
							at pFindInfo->iStart, unless we find an item that match
							*/

							// Set the default return value to -1
							// That means we didn't find any match.
							pResult = -1;

							//Is search NOT based on string?
							if( (pFindInfo->lvfi.flags & LVFI_STRING) == 0 )
							{
								//This will probably never happend...
								return pResult;
							}

							//This is the string we search for
							LPCSTR searchstr = pFindInfo->lvfi.psz;

							int startPos = pFindInfo->iStart;
							//Is startPos outside the list (happens if last item is selected)
							if(startPos >= ListView_GetItemCount(romList))
								startPos = 0;

							if(rdl==NULL)
								return pResult;

							RomDataList* curr=rdl;
							for(int i=0;i<startPos;i++)
								curr=curr->next;

							int currentPos=startPos;
							pResult=startPos;

							bool looped = false;

							// perform search
							do
							{
								// does this word begin with all characters in searchstr?
								if( _tcsnicmp(curr->fname, searchstr, strlen(searchstr)) == 0)
								{
									// select this item and stop search
									pResult = currentPos;
									break;
								}
								else if( _tcsnicmp(curr->fname, searchstr, strlen(searchstr)) > 0)
								{
									if(looped)
									{
										pResult = currentPos;
										break;
									}

									// optimization: the items are ordered alphabetically, so go back to the top since we know it can't be anything further down
									curr=rdl;
									currentPos = 0;
									looped = true;
									continue;
								}

								//Go to next item
								currentPos++;
								curr=curr->next;

								//Need to restart at top?
								if(currentPos >= ListView_GetItemCount(romList))
								{
									currentPos = 0;
									curr = rdl;
								}

							//Stop if back to start
							}while(currentPos != startPos);

							foundItemOverride = pResult;

							// in case previously-selected item is 0
							ListView_SetItemState (romList, 1, LVIS_SELECTED|LVIS_FOCUSED,LVIS_SELECTED|LVIS_FOCUSED);

							return pResult; // HACK: for some reason this selects the first item instead of what it's returning... current workaround is to manually re-select this return value upon the next changed event
						}
						break;
					case LVN_ITEMCHANGED:
						{
							// hack - see note directly above
							LPNMLISTVIEW lpnmlv = (LPNMLISTVIEW)lParam;
							if(lpnmlv->uNewState & (LVIS_SELECTED|LVIS_FOCUSED))
							{
								if(foundItemOverride != -1 && lpnmlv->iItem == 0)
								{
									ListView_SetItemState (romList, foundItemOverride, LVIS_SELECTED|LVIS_FOCUSED,LVIS_SELECTED|LVIS_FOCUSED);
									ListView_EnsureVisible (romList, foundItemOverride, FALSE);
									selectionMarkOverride = foundItemOverride;
									foundItemOverride = -1;
								}
								else
								{
									selectionMarkOverride = lpnmlv->iItem;
								}
							}
						}
						break;
					case LVN_GETDISPINFO:
						{
							if(!initDone)
								return false;
							int i, j;
							RomDataList* curr=rdl;
							if(rdl==NULL)
								return false;
							NMLVDISPINFO * nmlvdi=(NMLVDISPINFO*)lParam;
							j=nmlvdi->item.iItem;
							for(i=0;i<j;i++)
								if(curr) curr=curr->next;
							if(curr==NULL)
								return false;
							//if(curr->rname==NULL && j==(int)SendMessage(romList, LVM_GETNEXTITEM, -1, LVNI_SELECTED))
							//{
							//	TCHAR path[MAX_PATH];
							//	TCHAR buffer[32];
							//	TCHAR buffer2[32];
							//	GetPathFromTree(hDlg, IDC_ROM_DIR, path, TreeView_GetSelection(dirList));
							//	strcat(path, "\\");
							//	strcat(path, curr->fname);
							//	rominfo(path, buffer, buffer2);
							//	curr->rname=new char[strlen(buffer)+1];
							//	strcpy(curr->rname, buffer);
							//	curr->rmbits=new char[strlen(buffer2)+1];
							//	strcpy(curr->rmbits, buffer2);
							//}

							if(nmlvdi->item.iSubItem==0)
							{
								nmlvdi->item.pszText=curr->fname?curr->fname:(char*)"";
								nmlvdi->item.cchTextMax=MAX_PATH;
							}
							if(nmlvdi->item.iSubItem==1)
							{
								nmlvdi->item.pszText=curr->rname?curr->rname:(char*)"";
								nmlvdi->item.cchTextMax=24;
							}

							if(nmlvdi->item.iSubItem==2)
							{
								nmlvdi->item.pszText=curr->rmbits?curr->rmbits:(char*)"";
								nmlvdi->item.cchTextMax=11;
							}
							// nmlvdi->item.mask=LVIF_TEXT; // This is bad as wine relies on this to not change.
						}
						break;
					case NM_DBLCLK:
						{
							PostMessage(hDlg, WM_COMMAND, (WPARAM)(IDOK),(LPARAM)(NULL));
						}
					default:break;
					}
				}
				break;
			case IDC_ROM_DIR:
				{
					switch(pNmh->code)
					{
					case TVN_ITEMEXPANDING:
						{
							TCHAR selected[MAX_PATH];
							NMTREEVIEW* nmTv=(NMTREEVIEW*)lParam;

							while(TreeView_GetChild(dirList,nmTv->itemNew.hItem))
							{
								TreeView_DeleteItem(dirList, TreeView_GetChild(dirList,nmTv->itemNew.hItem));
							}

							if(nmTv->action&TVE_EXPAND)
							{

								GetPathFromTree(hDlg, IDC_ROM_DIR, selected,nmTv->itemNew.hItem);
								ExpandDir(selected, nmTv->itemNew.hItem, hDlg);
							}
							else
							{
								TVITEM tv;
								ZeroMemory(&tv, sizeof(TVITEM));
								HTREEITEM hTreeTemp=nmTv->itemNew.hItem;

								if(tv.iImage==6)
								{
									tv.mask=TVIF_HANDLE|TVIF_IMAGE;
									tv.hItem=hTreeTemp;
									tv.iImage=7;
									TreeView_SetItem(dirList,&tv);
								}


								TV_INSERTSTRUCT tvis;
								ZeroMemory(&tvis, sizeof(TV_INSERTSTRUCT));
								tvis.hParent=nmTv->itemNew.hItem;
								tvis.hInsertAfter=TVI_SORT;
								TreeView_InsertItem(dirList,&tvis);

							}
						}
						return false;
						break;
					case TVN_SELCHANGED:
						{
							ListFilesFromFolder(hDlg, &rdl);
							nextInvalidatedROM = rdl;
							nextInvalidatedROMCounter = 0;
						}
					default:return false;
					}
				}
			default:return false;
			}
		}
	default:return false;
	}
}
LRESULT CALLBACK DlgChildSplitProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static bool PaintSpecial;
	static short drag_x;
	short temp_x;
	switch(msg)
	{
	case WM_CREATE:
		return 0;
	case WM_SIZE:
        return 0;
    case WM_PAINT:
		PAINTSTRUCT ps;
		GetUpdateRect (hWnd, &ps.rcPaint, true);
		ps.hdc=GetDC(hWnd);
		ps.fErase=true;
		BeginPaint(hWnd, &ps);
		EndPaint(hWnd, &ps);
		ReleaseDC(hWnd, ps.hdc);
        return 0;
    case WM_LBUTTONDOWN:
		PaintSpecial=true;
		drag_x=GET_X_LPARAM(lParam);
		SetCapture(hWnd);
        return 0;
    case WM_LBUTTONUP:
		PaintSpecial=false;
		temp_x=(GET_X_LPARAM(lParam)-drag_x);
		HWND hDlg,hTree,hList;
		RECT treeRect;
		RECT listRect;
		hDlg=GetParent(hWnd);
		hTree=GetDlgItem(hDlg, IDC_ROM_DIR);
		hList=GetDlgItem(hDlg, IDC_ROMLIST);
		GetWindowRect(hTree, &treeRect);

		POINT p;
		p.x=temp_x+treeRect.right;
		p.y=treeRect.top;

		GetWindowRect(hList, &listRect);

		if(p.x>(listRect.right-50))
		{
			temp_x-=(short)(p.x-(listRect.right-50));
			p.x=listRect.right-50;
		}


		ScreenToClient(hDlg, &p);

		if(p.x<50)
		{
			temp_x+=(short)(50-p.x);
			p.x=50;
		}


		MoveWindow( hWnd, p.x, p.y, listRect.left-treeRect.right, listRect.bottom-listRect.top, FALSE);
		MoveWindow(hList, p.x+(listRect.left-treeRect.right), p.y,listRect.right-listRect.left-temp_x, listRect.bottom-listRect.top, TRUE);
		p.x=treeRect.left;
		p.y=treeRect.top;
		ScreenToClient(hDlg, &p);
		MoveWindow(hTree, p.x, p.y,treeRect.right-treeRect.left+temp_x,treeRect.bottom-treeRect.top, true);
		InvalidateRect(hWnd,NULL, true);
		ReleaseCapture();
        return 0;
    case WM_MOUSEMOVE:
        if (wParam & MK_LBUTTON)
		{
			//move paint location
			PaintSpecial=true;
			temp_x=(GET_X_LPARAM(lParam)-drag_x);
			hDlg=GetParent(hWnd);
			hTree=GetDlgItem(hDlg, IDC_ROM_DIR);
			hList=GetDlgItem(hDlg, IDC_ROMLIST);
			GetWindowRect(hTree, &treeRect);

			p.x=temp_x+treeRect.right;

			p.y=treeRect.top;
			GetWindowRect(hList, &listRect);

			if(p.x>(listRect.right-50))
			{
				temp_x-=(short)(p.x-(listRect.right-50));
				p.x=listRect.right-50;
			}


			ScreenToClient(hDlg, &p);

			if(p.x<50)
			{
				temp_x+=(short)(50-p.x);
				p.x=50;
			}

			MoveWindow(hWnd, p.x, p.y, listRect.left-treeRect.right, listRect.bottom-listRect.top, FALSE);
			MoveWindow(hList, p.x+(listRect.left-treeRect.right), p.y,listRect.right-listRect.left-temp_x, listRect.bottom-listRect.top, TRUE);
			p.x=treeRect.left;
			p.y=treeRect.top;
			ScreenToClient(hDlg, &p);
			MoveWindow(hTree, p.x, p.y,treeRect.right-treeRect.left+temp_x,treeRect.bottom-treeRect.top, true);
			InvalidateRect(hWnd,NULL, true);
		}
        return 0;
    case WM_CAPTURECHANGED:
		PaintSpecial=false;
		ReleaseCapture();
		return 0;
    case WM_DESTROY:
        return 0;
	default:return DefWindowProc(hWnd, msg, wParam, lParam);
	}
}





INT_PTR CALLBACK DlgPackConfigProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("LinkHylia.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			SetDlgItemText(hDlg, IDC_STAR_OCEAN, GUI.StarOceanPack);
			SetDlgItemText(hDlg, IDC_SFA2, GUI.SFA2NTSCPack);
			SetDlgItemText(hDlg, IDC_SFA2E, GUI.SFA2PALPack);
			SetDlgItemText(hDlg, IDC_SJNS, GUI.SJNSPack);
			SetDlgItemText(hDlg, IDC_SFZ2, GUI.SFZ2Pack);
			SetDlgItemText(hDlg, IDC_MDH, GUI.MDHPack);
			SetDlgItemText(hDlg, IDC_SPL4, GUI.SPL4Pack);
			SetDlgItemText(hDlg, IDC_FEOEZ, GUI.FEOEZPack);
		}
		return true;

		case WM_PAINT:
		{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
		}
		return true;
	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDC_SO_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="Star Ocean Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_STAR_OCEAN, path);
				}
				break;
			case IDC_SFA2_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="Street Fighter Alpha 2 Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_SFA2, path);
				}
				break;
			case IDC_SFA2E_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="Street Fighter Alpha 2 Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_SFA2E, path);
				}
				break;
			case IDC_SFZ2_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="Street Fighter Zero 2 Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_SFZ2, path);
				}
				break;
			case IDC_FEOEZ_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="Far East of Eden Zero Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_FEOEZ, path);
				}
				break;
			case IDC_MDH_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="Momotarou Densetsu Happy Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_MDH, path);
				}
				break;
			case IDC_FEOEZ_SJNS_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="FEOEZ - Shounen Jump no Shou Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_SJNS, path);
				}
				break;
			case IDC_SPL4_BROWSE:
				{
					LPMALLOC lpm=NULL;
					LPITEMIDLIST iidl=NULL;
					BROWSEINFO bi;
					ZeroMemory(&bi, sizeof(BROWSEINFO));
					char path[MAX_PATH];
					char title[]="FEOEZ - Shounen Jump no Shou Graphics Pack";
					//CoInitialize(NULL);
					bi.hwndOwner=hDlg;
					bi.pszDisplayName=path;
					bi.lpszTitle=title;
					iidl=SHBrowseForFolder(&bi);
					SHGetPathFromIDList(iidl, path);
					SHGetMalloc(&lpm);
					lpm->Free(iidl);
					//CoUninitialize();
					SetDlgItemText(hDlg, IDC_SPL4, path);
				}
				break;
			case IDOK:
				GetDlgItemText(hDlg, IDC_STAR_OCEAN, GUI.StarOceanPack, MAX_PATH);
				GetDlgItemText(hDlg, IDC_SFA2, GUI.SFA2NTSCPack, MAX_PATH);
				GetDlgItemText(hDlg, IDC_SPL4, GUI.SPL4Pack, MAX_PATH);
				GetDlgItemText(hDlg, IDC_SJNS, GUI.SJNSPack, MAX_PATH);
				GetDlgItemText(hDlg, IDC_SFZ2, GUI.SFZ2Pack, MAX_PATH);
				GetDlgItemText(hDlg, IDC_SFA2E, GUI.SFA2PALPack, MAX_PATH);
				GetDlgItemText(hDlg, IDC_MDH, GUI.MDHPack, MAX_PATH);
				GetDlgItemText(hDlg, IDC_FEOEZ, GUI.FEOEZPack, MAX_PATH);
				WinSaveConfigFile();
			case IDCANCEL:
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				EndDialog(hDlg, 0);
				return true;
				break;
			default: return false; break;
				}
			}
		default:return false;
	}
}

extern "C"
{
	char*osd_GetPackDir()
	{
		static char filename[MAX_PATH];
		memset(filename, 0, MAX_PATH);

		if(strlen(GUI.FreezeFileDir)!=0)
			strcpy (filename, GUI.FreezeFileDir);
		else
		{
			char dir [_MAX_DIR + 1];
			char drive [_MAX_DRIVE + 1];
			char name [_MAX_FNAME + 1];
			char ext [_MAX_EXT + 1];
			_splitpath(Memory.ROMFilename,drive, dir, name, ext);
			_makepath(filename,drive, dir, NULL, NULL);
		}

		if(!strncmp((char*)&Memory.ROM [0xffc0], "SUPER POWER LEAG 4   ", 21))
		{
			if(strlen(GUI.SPL4Pack))
				return GUI.SPL4Pack;
			else strcat(filename, "\\SPL4-SP7");
		}
		else if(!strncmp((char*)&Memory.ROM [0xffc0], "MOMOTETSU HAPPY      ",21))
		{
			if(strlen(GUI.MDHPack))
				return GUI.MDHPack;
			else strcat(filename, "\\SMHT-SP7");
		}
		else if(!strncmp((char*)&Memory.ROM [0xffc0], "HU TENGAI MAKYO ZERO ", 21))
		{
			if(strlen(GUI.FEOEZPack))
				return GUI.FEOEZPack;
			else strcat(filename, "\\FEOEZSP7");
		}
		else if(!strncmp((char*)&Memory.ROM [0xffc0], "JUMP TENGAIMAKYO ZERO",21))
		{
			if(strlen(GUI.SJNSPack))
				return GUI.SJNSPack;
			else strcat(filename, "\\SJUMPSP7");
		}
		else strcat(filename, "\\MISC-SP7");
		return filename;
	}
}
#ifdef NETPLAY_SUPPORT
INT_PTR CALLBACK DlgNetConnect(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
//	HKEY hKey;
	char defPort[5];
	char portTemp[5];
	char temp[100];
//	char temp2[5];
	static char* hostname;
//	unsigned long cbData;
//	static int i;
	if(Settings.Port==0)
	{
		_itoa(1996,defPort,10);
	}
	else
	{
		_itoa(Settings.Port,defPort,10);
	}

	WORD chkLength;
//	if(RegCreateKeyEx(HKEY_CURRENT_USER,MY_REG_KEY "\\1.x\\NetPlayServerHistory",0,NULL,REG_OPTION_NON_VOLATILE,KEY_ALL_ACCESS, NULL, &hKey,NULL) == ERROR_SUCCESS){}

	switch (msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		SetWindowText(hDlg,NPCON_TITLE);
		SetDlgItemText(hDlg,IDC_LABEL_SERVERADDY,NPCON_LABEL_SERVERADDY);
		SetDlgItemText(hDlg,IDC_LABEL_PORTNUM,NPCON_LABEL_PORTNUM);
		SetDlgItemText(hDlg,IDC_CLEARHISTORY, NPCON_CLEARHISTORY);
		SetDlgItemText(hDlg,IDOK,BUTTON_OK);
		SetDlgItemText(hDlg,IDCANCEL,BUTTON_CANCEL);
		hBmp=(HBITMAP)LoadImage(NULL, TEXT("Overload.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
		hostname = (char *)lParam;
		{
			for(int i=0; i<MAX_RECENT_HOSTS_LIST_SIZE && *GUI.RecentHostNames[i]; i++)
				SendDlgItemMessage(hDlg, IDC_HOSTNAME, CB_INSERTSTRING,i,(LPARAM)GUI.RecentHostNames[i]);
		}

		SendDlgItemMessage(hDlg, IDC_PORTNUMBER, WM_SETTEXT, 0, (LPARAM)defPort);

		SendDlgItemMessage(hDlg, IDC_HOSTNAME, WM_SETTEXT, 0, (LPARAM)NPCON_ENTERHOST);

		return TRUE;
		case WM_PAINT:
		{
			PAINTSTRUCT ps;
			BeginPaint (hDlg, &ps);
			if(hBmp)
			{
				BITMAP bmp;
				ZeroMemory(&bmp, sizeof(BITMAP));
				RECT r;
				GetClientRect(hDlg, &r);
				HDC hdc=GetDC(hDlg);
				HDC hDCbmp=CreateCompatibleDC(hdc);
				GetObject(hBmp, sizeof(BITMAP), &bmp);
				HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
				StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
				SelectObject(hDCbmp, hOldBmp);
				DeleteDC(hDCbmp);
				ReleaseDC(hDlg, hdc);
			}

			EndPaint (hDlg, &ps);
		}
		return true;

	case WM_COMMAND:
		switch(LOWORD(wParam))
		{
		case IDC_CLEARHISTORY:
			{
				{
					SendDlgItemMessage(hDlg,IDC_HOSTNAME,CB_RESETCONTENT,0,0);
					SendDlgItemMessage(hDlg,IDC_HOSTNAME,CB_INSERTSTRING,0,(LPARAM)GUI.RecentHostNames[0]);
					for(int i=1; i<MAX_RECENT_HOSTS_LIST_SIZE; i++)
						*GUI.RecentHostNames[i] = '\0';
				}
				break;
			}
		case IDOK:
			{

				chkLength = (WORD) SendDlgItemMessage(hDlg,IDC_PORTNUMBER,EM_LINELENGTH,0,0);
				*((LPWORD)portTemp) = chkLength;
				SendDlgItemMessage(hDlg,IDC_PORTNUMBER,EM_GETLINE,0,(LPARAM)(LPCTSTR)portTemp);

				if(atoi(portTemp)>65535||atoi(portTemp)<1024)
				{
					MessageBox(hDlg,"Port Number needs to be between 1024 and 65535","Error",MB_OK);
					break;
				}
				else
				{
					Settings.Port = atoi(portTemp);
				}
				//chkLength = (WORD) SendDlgItemMessage(hDlg,IDC_HOSTNAME,EM_LINELENGTH,0,0);
				//if(chkLength > 0)
				//{
				//SendDlgItemMessage(hDlg,IDC_HOSTNAME,EM_GETLINE,0,(LPARAM)hostname);
				SendDlgItemMessage(hDlg,IDC_HOSTNAME,WM_GETTEXT,100,(LPARAM)temp);
				if(!strcmp(temp, NPCON_ENTERHOST))
				{
					MessageBox(hDlg,NPCON_PLEASE_ENTERHOST,"Error",MB_OK);
					break;
				}
				strcpy(hostname,temp);
				//MessageBox(hDlg,temp,"hola",MB_OK);

				// save hostname in recent list
				{
					int i;
					for(i=0; i<MAX_RECENT_HOSTS_LIST_SIZE; i++)
					{
						if(!*GUI.RecentHostNames[i])
						{
							strcpy(GUI.RecentHostNames[i], hostname);
							break;
						}
						else if(!stricmp(GUI.RecentHostNames[i], hostname))
							break;
					}
					if(i == MAX_RECENT_HOSTS_LIST_SIZE)
						strcpy(GUI.RecentHostNames[1+(rand()%(MAX_RECENT_HOSTS_LIST_SIZE-1))], hostname);
				}

				unsigned long len;
				len = strlen(temp);
				if(len > 0)
				{
					EndDialog(hDlg,1);
					if(hBmp)
					{
						DeleteObject(hBmp);
						hBmp=NULL;
					}
					return TRUE;
				}
				else
				{
					EndDialog(hDlg,0);
					if(hBmp)
					{
						DeleteObject(hBmp);
						hBmp=NULL;
					}
					return TRUE;
				}

				break;
				//}
			}
		case IDCANCEL:
			{
				EndDialog(hDlg, 0);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				return TRUE;
			}
		default:break;
		}
	}
	return FALSE;
}
#endif
void SetInfoDlgColor(unsigned char r, unsigned char g, unsigned char b)
{
	GUI.InfoColor=RGB(r,g,b);
}

void ClearExts(void)
{
	ExtList* temp;
	ExtList* curr=valid_ext;
	while(curr!=NULL)
	{
		temp=curr->next;
		if(curr->extension)
			delete [] curr->extension;
		delete curr;
		curr=temp;
	}
	valid_ext=NULL;

}

void LoadExts(void)
{
	char buffer[MAX_PATH+2];
	if(valid_ext!=NULL)
	{
		ClearExts();
	}
	ExtList* curr;
	valid_ext=new ExtList;
	curr=valid_ext;
	ZeroMemory(curr, sizeof(ExtList));
	ifstream in;

#if (((defined(_MSC_VER) && _MSC_VER >= 1300)) || defined(__MINGW32__))
	in.open("Valid.Ext", ios::in);
#else
	in.open("Valid.Ext", ios::in|ios::nocreate);
#endif
	if (!in.is_open())
	{
		in.clear();
		MakeExtFile();
	#if (((defined(_MSC_VER) && _MSC_VER >= 1300)) || defined(__MINGW32__))
		in.open("Valid.Ext", ios::in);
	#else
		in.open("Valid.Ext", ios::in|ios::nocreate);
	#endif
		if(!in.is_open())
		{
			MessageBox(GUI.hWnd, "Fatal Error: The File \"Valid.Ext\" could not be found or created.", "Error", MB_ICONERROR|MB_OK);
			PostQuitMessage(-1);
		}
	}

	do
	{
		buffer[0]='\0';
		in.getline(buffer,MAX_PATH+2);
		if((*buffer)!='\0')
		{
			curr->next=new ExtList;
			curr=curr->next;
			ZeroMemory(curr, sizeof(ExtList));
			if(_strnicmp(buffer+strlen(buffer)-1, "Y", 1)==0)
				curr->compressed=true;
			if(strlen(buffer)>1)
			{
				curr->extension=new char[strlen(buffer)-1];
				strncpy(curr->extension, buffer, strlen(buffer)-1);
				curr->extension[strlen(buffer)-1]='\0';
			}
			else curr->extension=NULL;
		}
	}
	while(!in.eof());
	in.close();
	curr=valid_ext;
	valid_ext=valid_ext->next;
	delete curr;
}

void MakeExtFile(void)
{
	ofstream out;
	out.open("Valid.Ext");

	out<<"N"   <<endl<<"smcN"<<endl<<"zipY"<<endl<<"gzY" <<endl<<"swcN"<<endl<<"figN"<<endl;
	out<<"058N"<<endl<<"078N"<<endl<<"japN"<<endl<<"usaN"<<endl<<"048N"<<endl;
	out<<"eurN"<<endl<<"sfcN"<<endl<<"1N"  <<endl<<"mgdN"<<endl<<"ufoN"<<endl;
	out<<"binN"<<endl<<"gd3N"<<endl<<"mghN"<<endl<<"gd7N"<<endl<<"ausN"<<endl;
	out<<"dx2N"<<endl<<"aN"<<endl<<"jmaY";
	out.close();
	SetFileAttributes("Valid.Ext", FILE_ATTRIBUTE_ARCHIVE|FILE_ATTRIBUTE_READONLY);
};
#ifdef NETPLAY_SUPPORT
INT_PTR CALLBACK DlgNPOptions(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	char defPort[5];
	WORD chkLength;
	if(Settings.Port==0)
	{
		_itoa(1996,defPort,10);
	}
	else
	{
		_itoa(Settings.Port,defPort,10);
	}

	switch (msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		SetWindowText(hDlg,NPOPT_TITLE);
		SetDlgItemText(hDlg,IDC_LABEL_PORTNUM,NPOPT_LABEL_PORTNUM);
		SetDlgItemText(hDlg,IDC_LABEL_PAUSEINTERVAL,NPOPT_LABEL_PAUSEINTERVAL);
		SetDlgItemText(hDlg,IDC_LABEL_PAUSEINTERVAL_TEXT,NPOPT_LABEL_PAUSEINTERVAL_TEXT);
		SetDlgItemText(hDlg,IDC_LABEL_MAXSKIP,NPOPT_LABEL_MAXSKIP);
		SetDlgItemText(hDlg,IDC_SYNCBYRESET,NPOPT_SYNCBYRESET);
		SetDlgItemText(hDlg,IDC_SENDROM,NPOPT_SENDROM);
		SetDlgItemText(hDlg,IDC_ACTASSERVER,NPOPT_ACTASSERVER);
		SetDlgItemText(hDlg,IDC_PORTNUMBLOCK,NPOPT_PORTNUMBLOCK);
		SetDlgItemText(hDlg,IDC_CLIENTSETTINGSBLOCK,NPOPT_CLIENTSETTINGSBLOCK);
		SetDlgItemText(hDlg,IDC_SERVERSETTINGSBLOCK,NPOPT_SERVERSETTINGSBLOCK);
		SetDlgItemText(hDlg,IDOK,BUTTON_OK);
		SetDlgItemText(hDlg,IDCANCEL,BUTTON_CANCEL);

		hBmp=(HBITMAP)LoadImage(NULL, TEXT("TheDumper.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
		SendDlgItemMessage(hDlg, IDC_PORTNUMBERA, WM_SETTEXT, 0, (LPARAM)defPort);
		if(Settings.NetPlayServer)
		{
			SendDlgItemMessage(hDlg, IDC_ACTASSERVER, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		}
		if(NPServer.SendROMImageOnConnect)
		{
			SendDlgItemMessage(hDlg, IDC_SENDROM, BM_SETCHECK, BST_CHECKED,0);
		}

		if(NPServer.SyncByReset)
		{
			SendDlgItemMessage(hDlg, IDC_SYNCBYRESET, BM_SETCHECK, BST_CHECKED,0);
		}
		SendDlgItemMessage(hDlg, IDC_MAXSPIN, UDM_SETRANGE,0,MAKELPARAM((short)60,(short)0));
		SendDlgItemMessage(hDlg, IDC_MAXSPIN, UDM_SETPOS,0,MAKELONG(NetPlay.MaxFrameSkip,0));
		SendDlgItemMessage(hDlg, IDC_PAUSESPIN, UDM_SETRANGE,0,MAKELONG(30,0));
		SendDlgItemMessage(hDlg, IDC_PAUSESPIN, UDM_SETPOS,0,MAKELONG(NetPlay.MaxBehindFrameCount,0));
		return TRUE;
	case WM_PAINT:
		{
			PAINTSTRUCT ps;
			BeginPaint (hDlg, &ps);
			if(hBmp)
			{
				BITMAP bmp;
				ZeroMemory(&bmp, sizeof(BITMAP));
				RECT r;
				GetClientRect(hDlg, &r);
				HDC hdc=GetDC(hDlg);
				HDC hDCbmp=CreateCompatibleDC(hdc);
				GetObject(hBmp, sizeof(BITMAP), &bmp);
				HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
				StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
				SelectObject(hDCbmp, hOldBmp);
				DeleteDC(hDCbmp);
				ReleaseDC(hDlg, hdc);
			}

			EndPaint (hDlg, &ps);
		}
		return true;

	case WM_COMMAND:
		switch(LOWORD(wParam))
		{
		case IDOK:
			{
				NetPlay.MaxFrameSkip=(uint32)SendDlgItemMessage(hDlg, IDC_MAXSPIN, UDM_GETPOS,0,0);
				NetPlay.MaxBehindFrameCount=(uint32)SendDlgItemMessage(hDlg, IDC_PAUSESPIN, UDM_GETPOS,0,0);
				chkLength=(WORD)SendDlgItemMessage(hDlg,IDC_PORTNUMBERA,EM_LINELENGTH,0,0);
				*((LPWORD)defPort) = chkLength;
				SendDlgItemMessage(hDlg,IDC_PORTNUMBERA,EM_GETLINE,0,(LPARAM)defPort);
				if(atoi(defPort)<1024||atoi(defPort)>65535)
				{
					MessageBox(hDlg,"Port Number needs to be betweeb 1024 and 65535","Error",MB_OK);
					break;
				}
				else
				{
					Settings.Port = atoi(defPort);
				}
				//MessageBox(hDlg,defPort,defPort,MB_OK);
				Settings.NetPlayServer = IsDlgButtonChecked(hDlg,IDC_ACTASSERVER);
				NPServer.SendROMImageOnConnect = IsDlgButtonChecked(hDlg,IDC_SENDROM);
				NPServer.SyncByReset = IsDlgButtonChecked(hDlg,IDC_SYNCBYRESET);

				EndDialog(hDlg,0);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				WinSaveConfigFile();
				return TRUE;
			}
		case IDCANCEL:
			{
				EndDialog(hDlg,0);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
				return TRUE;
			}
		}
	}
	return FALSE;
}
#endif
HRESULT CALLBACK EnumModesCallback( LPDDSURFACEDESC lpDDSurfaceDesc, LPVOID lpContext)
{
	char depmode[80];
	char s[80];
	dMode curmode;
	HWND hDlg = (HWND)lpContext;
	int index;
//	HKEY hKey;
	DWORD type;
//	BYTE val[4];
//	DWORD nuv=4;
	type=REG_DWORD;

	if((lpDDSurfaceDesc->ddpfPixelFormat.dwRGBBitCount != 15 &&
		lpDDSurfaceDesc->ddpfPixelFormat.dwRGBBitCount != 16) ||
        (lpDDSurfaceDesc->dwWidth < SNES_WIDTH ||
		lpDDSurfaceDesc->dwHeight < SNES_HEIGHT_EXTENDED))
	{
		// let them muck with the .cfg file if they really want to set such a poor display mode
		return DDENUMRET_OK; // keep going without adding mode to list
	}
	sprintf( depmode, "%dx%d %dbit %dHz", (int)lpDDSurfaceDesc->dwWidth, (int)lpDDSurfaceDesc->dwHeight, (int)lpDDSurfaceDesc->ddpfPixelFormat.dwRGBBitCount, (int)lpDDSurfaceDesc->dwRefreshRate);
//	RegOpenKeyEx(HKEY_CURRENT_USER,MY_REG_KEY  "\\1.x\\DisplayModes",0,KEY_ALL_ACCESS, &hKey);



	curmode.width=lpDDSurfaceDesc->dwWidth;
	curmode.height=lpDDSurfaceDesc->dwHeight;
	curmode.depth=lpDDSurfaceDesc->ddpfPixelFormat.dwRGBBitCount;
	curmode.rate=lpDDSurfaceDesc->dwRefreshRate;
	// NYI: loading which display modes have been tested
//	if(RegQueryValueEx(hKey,depmode,0,&type,val,&nuv)==ERROR_SUCCESS)
//	{
//		//sprintf(s,"%d %d %d %d",val[0],val[1],val[2],val[3]);
//		//MessageBox(hDlg,s,temp,MB_OK);
//		switch(val[0])
//		{
//		case 1:
//			strcpy(s,"Failed");
//			curmode.status=1;
//			break;
//		case 3:
//			strcpy(s,"Works");
//			curmode.status=3;
//			break;
//		default:
//			strcpy(s,"Untested");
//			curmode.status=0;
//		}
//	}
//	else
	{
		strcpy(s,"Untested");
		curmode.status=0;
		//MessageBox(hDlg,s,temp,MB_OK);
	}
	dm.push_back(curmode);
	LVITEM lvi;
	ZeroMemory(&lvi, sizeof(LVITEM));
	lvi.iItem=dm.size();
	lvi.mask=LVIF_TEXT;
	lvi.pszText=depmode;
	lvi.cchTextMax=80;
	//lvi.lParam=dmindex;
	index=ListView_InsertItem(hDlg, &lvi);



	//dmindex++;
	ZeroMemory(&lvi, sizeof(LVITEM));
	lvi.mask=LVIF_TEXT;
	lvi.iItem=index;
	lvi.iSubItem=1;
	lvi.pszText=s;
	lvi.cchTextMax=10;
	ListView_SetItem(hDlg, &lvi);
//	RegCloseKey(hKey);

	return DDENUMRET_OK;
}

void EnableDisableKeyFields (int index, HWND hDlg)
{
	bool enableUnTurboable;
	if(index < 5)
	{
		SetDlgItemText(hDlg,IDC_LABEL_RIGHT,INPUTCONFIG_LABEL_RIGHT);
		SetDlgItemText(hDlg,IDC_LABEL_UPLEFT,INPUTCONFIG_LABEL_UPLEFT);
		SetDlgItemText(hDlg,IDC_LABEL_UPRIGHT,INPUTCONFIG_LABEL_UPRIGHT);
		SetDlgItemText(hDlg,IDC_LABEL_DOWNRIGHT,INPUTCONFIG_LABEL_DOWNRIGHT);
		SetDlgItemText(hDlg,IDC_LABEL_UP,INPUTCONFIG_LABEL_UP);
		SetDlgItemText(hDlg,IDC_LABEL_LEFT,INPUTCONFIG_LABEL_LEFT);
		SetDlgItemText(hDlg,IDC_LABEL_DOWN,INPUTCONFIG_LABEL_DOWN);
		SetDlgItemText(hDlg,IDC_LABEL_DOWNLEFT,INPUTCONFIG_LABEL_DOWNLEFT);
		enableUnTurboable = true;
	}
	else
	{
		SetDlgItemText(hDlg,IDC_LABEL_UP,INPUTCONFIG_LABEL_MAKE_TURBO);
		SetDlgItemText(hDlg,IDC_LABEL_LEFT,INPUTCONFIG_LABEL_MAKE_HELD);
		SetDlgItemText(hDlg,IDC_LABEL_DOWN,INPUTCONFIG_LABEL_MAKE_TURBO_HELD);
		SetDlgItemText(hDlg,IDC_LABEL_RIGHT,INPUTCONFIG_LABEL_CLEAR_TOGGLES_AND_TURBO);
		SetDlgItemText(hDlg,IDC_LABEL_UPLEFT,INPUTCONFIG_LABEL_UNUSED);
		SetDlgItemText(hDlg,IDC_LABEL_UPRIGHT,INPUTCONFIG_LABEL_UNUSED);
		SetDlgItemText(hDlg,IDC_LABEL_DOWNLEFT,INPUTCONFIG_LABEL_UNUSED);
		SetDlgItemText(hDlg,IDC_LABEL_DOWNRIGHT,INPUTCONFIG_LABEL_UNUSED);
		SetDlgItemText(hDlg,IDC_UPLEFT,INPUTCONFIG_LABEL_UNUSED);
		SetDlgItemText(hDlg,IDC_UPRIGHT,INPUTCONFIG_LABEL_UNUSED);
		SetDlgItemText(hDlg,IDC_DWNLEFT,INPUTCONFIG_LABEL_UNUSED);
		SetDlgItemText(hDlg,IDC_DWNRIGHT,INPUTCONFIG_LABEL_UNUSED);
		enableUnTurboable = false;
	}

	EnableWindow(GetDlgItem(hDlg,IDC_UPLEFT), enableUnTurboable);
	EnableWindow(GetDlgItem(hDlg,IDC_UPRIGHT), enableUnTurboable);
	EnableWindow(GetDlgItem(hDlg,IDC_DWNRIGHT), enableUnTurboable);
	EnableWindow(GetDlgItem(hDlg,IDC_DWNLEFT), enableUnTurboable);
}

void UpdateModeListBox(HWND hListView)
{
	LVCOLUMN col;

	ListView_DeleteAllItems(hListView);
	ListView_DeleteColumn(hListView,1);
	ListView_DeleteColumn(hListView,0);
	dm.clear();

	if(GUI.outputMethod==DIRECTDRAW) {
		ZeroMemory(&col, sizeof(LVCOLUMN));
		col.mask=LVCF_FMT|LVCF_TEXT|LVCF_WIDTH;
		col.fmt=LVCFMT_LEFT;
		col.iOrder=0;
		col.cx=60;
		col.cchTextMax=80;
		col.pszText="Status";

		ListView_InsertColumn(hListView,    1,   &col);
	}

	ZeroMemory(&col, sizeof(LVCOLUMN));
	col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH;
	col.fmt=LVCFMT_LEFT;
	col.iOrder=0;
	col.cx=(GUI.outputMethod==DIRECT3D)?185:125;
	col.cchTextMax=80;
	col.pszText="Video Mode";

	ListView_InsertColumn(hListView,    0,   &col);
	if(GUI.outputMethod==DIRECT3D)
		Direct3D.fillModesListView(hListView,&dm);
	else
		DirectDraw.lpDD->EnumDisplayModes(DDEDM_REFRESHRATES,NULL,hListView,(LPDDENUMMODESCALLBACK)EnumModesCallback);
}

INT_PTR CALLBACK DlgFrameSkipSettings(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	char temp[80];

	switch(msg)
	{
	case WM_PAINT:
		{
			PAINTSTRUCT ps;
			BeginPaint (hDlg, &ps);
			if(hBmp)
			{
				BITMAP bmp;
				ZeroMemory(&bmp, sizeof(BITMAP));
				RECT r;
				GetClientRect(hDlg, &r);
				HDC hdc=GetDC(hDlg);
				HDC hDCbmp=CreateCompatibleDC(hdc);
				GetObject(hBmp, sizeof(BITMAP), &bmp);
				HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
				StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
				SelectObject(hDCbmp, hOldBmp);
				DeleteDC(hDCbmp);
				ReleaseDC(hDlg, hdc);
			}
			
			EndPaint (hDlg, &ps);
		}
		return true;

	case WM_INITDIALOG:
		hBmp=(HBITMAP)LoadImage(NULL, TEXT("yurine.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);

		SendDlgItemMessage(hDlg,IDC_FRAMERATESKIPSLIDER,TBM_SETRANGE,(WPARAM)true,(LPARAM)MAKELONG(0,9));
		if(Settings.SkipFrames!=AUTO_FRAMERATE)
			SendDlgItemMessage(hDlg,IDC_FRAMERATESKIPSLIDER,TBM_SETPOS,(WPARAM)true,(LPARAM)Settings.SkipFrames);
		
		if(Settings.SkipFrames==AUTO_FRAMERATE)
			SendDlgItemMessage(hDlg, IDC_AUTOFRAME, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);

		if(Settings.SkipFrames==AUTO_FRAMERATE) {
			SendDlgItemMessage(hDlg, IDC_AUTOFRAME, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
			EnableWindow(GetDlgItem(hDlg, IDC_SKIPCOUNT),FALSE);
			SetDlgItemText(hDlg,IDC_SKIPCOUNT,TEXT("0"));
		} else {
			SendDlgItemMessage(hDlg, IDC_FIXEDSKIP, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
			EnableWindow(GetDlgItem(hDlg, IDC_MAXSKIP),FALSE);
			_itot(Settings.SkipFrames - 1,temp,10);
			SetDlgItemText(hDlg,IDC_SKIPCOUNT,temp);
		}
		_itot(Settings.AutoMaxSkipFrames,temp,10);
		SetDlgItemText(hDlg,IDC_MAXSKIP,temp);
		_itot(Settings.TurboSkipFrames,temp,10);
		SetDlgItemText(hDlg,IDC_TURBO_SKIP,temp);
		Edit_LimitText(GetDlgItem(hDlg, IDC_SKIPCOUNT),80);
		Edit_LimitText(GetDlgItem(hDlg, IDC_MAXSKIP),80);
		Edit_LimitText(GetDlgItem(hDlg, IDC_TURBO_SKIP),80);
		return true;

	case WM_CLOSE:
		EndDialog(hDlg, false);
		return true;

	case WM_DESTROY:
		if (hBmp)
		{
			DeleteObject(hBmp);
			hBmp=NULL;
		}
		return true;

	case WM_COMMAND:
		switch(LOWORD(wParam))
		{
		case IDC_AUTOFRAME:
			if(BN_CLICKED==HIWORD(wParam)||BN_DBLCLK==HIWORD(wParam))
			{
				EnableWindow(GetDlgItem(hDlg, IDC_MAXSKIP), TRUE);
				EnableWindow(GetDlgItem(hDlg, IDC_SKIPCOUNT), FALSE);
				return true;
			}
			else return false;

			break;

		case IDC_FIXEDSKIP:
			if(BN_CLICKED==HIWORD(wParam)||BN_DBLCLK==HIWORD(wParam))
			{
				EnableWindow(GetDlgItem(hDlg, IDC_MAXSKIP), FALSE);
				EnableWindow(GetDlgItem(hDlg, IDC_SKIPCOUNT), TRUE);
				return true;

			}
			break;

		case IDOK:
			if(IsDlgButtonChecked(hDlg, IDC_AUTOFRAME))
			{
				Settings.SkipFrames=AUTO_FRAMERATE;
				GetDlgItemText(hDlg,IDC_MAXSKIP,temp,80);
				Settings.AutoMaxSkipFrames=(uint32)_ttoi(temp);
			}
			else
			{
				GetDlgItemText(hDlg,IDC_SKIPCOUNT,temp,80);
				Settings.SkipFrames=(uint32)_ttoi(temp) + 1;
			}
			GetDlgItemText(hDlg,IDC_TURBO_SKIP,temp,80);
			Settings.TurboSkipFrames=(uint32)_ttoi(temp);

			WinSaveConfigFile();

			EndDialog(hDlg, true);
			return true;

		case IDCANCEL:
			SendMessage(hDlg, WM_CLOSE, 0, 0);
			return true;
		}
	}
	
	return false;
}

bool in_display_dlg = false;

INT_PTR CALLBACK DlgFunky(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	int index;
	char s[80],temp[80];

	// temporary GUI state for restoring after previewing while selecting options
	static int prevScale, prevScaleHiRes, prevPPL;
	static bool prevStretch, prevAspectRatio, prevHeightExtend, prevMessagesInImage, prevVideoMemory;
	static OutputMethod prevOutputMethod;
	static D3DFilter prevD3dFilter;

	switch(msg)
	{
	case WM_PAINT:
		{
			PAINTSTRUCT ps;
			BeginPaint (hDlg, &ps);
			if(hBmp)
			{
				BITMAP bmp;
				ZeroMemory(&bmp, sizeof(BITMAP));
				RECT r;
				GetClientRect(hDlg, &r);
				HDC hdc=GetDC(hDlg);
				HDC hDCbmp=CreateCompatibleDC(hdc);
				GetObject(hBmp, sizeof(BITMAP), &bmp);
				HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
				StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
				SelectObject(hDCbmp, hOldBmp);
				DeleteDC(hDCbmp);
				ReleaseDC(hDlg, hdc);
			}

			EndPaint (hDlg, &ps);
		}
		return true;

	case WM_INITDIALOG:
		in_display_dlg = true;
		if(DirectDraw.Clipped) S9xReRefresh();

		prevOutputMethod = GUI.outputMethod;
		prevScale = GUI.Scale;
		prevScaleHiRes = GUI.ScaleHiRes;
		prevPPL = GFX.RealPPL;
		prevStretch = GUI.Stretch;
		prevVideoMemory = GUI.ddrawUseVideoMemory;
		prevD3dFilter = GUI.d3dFilter;
		prevAspectRatio = GUI.AspectRatio;
		prevHeightExtend = GUI.HeightExtend;
		prevMessagesInImage = GUI.MessagesInImage;

		hBmp=(HBITMAP)LoadImage(NULL, TEXT("lantus.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
		sprintf(s,"Current: %dx%d %dbit %dHz",GUI.Width,GUI.Height,GUI.Depth,GUI.RefreshRate);
		SendDlgItemMessage(hDlg,IDC_CURRMODE,WM_SETTEXT,0,(LPARAM)s);

		if((GUI.outputMethod==DIRECTDRAW) && DirectDraw.lpDD==NULL)
			DirectDrawCreate( NULL, &DirectDraw.lpDD, NULL);

		if(GUI.tripleBuffering)
			SendDlgItemMessage(hDlg, IDC_DBLBUFFER, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if(Settings.Transparency)
			SendDlgItemMessage(hDlg, IDC_TRANS, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if(Settings.SupportHiRes)
			SendDlgItemMessage(hDlg, IDC_HIRES, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if(GUI.HeightExtend)
			SendDlgItemMessage(hDlg, IDC_HEIGHT_EXTEND, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
//		if(Settings.Mode7Interpolate)
//			SendDlgItemMessage(hDlg, IDC_BILINEARMD7, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if(GUI.MessagesInImage)
			SendDlgItemMessage(hDlg, IDC_MESSAGES_IN_IMAGE, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if(Settings.SkipFrames==AUTO_FRAMERATE)
			SendDlgItemMessage(hDlg, IDC_AUTOFRAME, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if(GUI.Stretch)
		{
			SendDlgItemMessage(hDlg, IDC_STRETCH, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_VIDEOCARD,WM_SETTEXT,0,(LPARAM)"Bi-linear Filtering");
		}
		else
		{
			if(GUI.outputMethod==DIRECTDRAW)
				SendDlgItemMessage(hDlg,IDC_VIDEOCARD,WM_SETTEXT,0,(LPARAM)"Use Video Memory");
			else {
				EnableWindow(GetDlgItem(hDlg, IDC_VIDEOCARD), FALSE);
				SendDlgItemMessage(hDlg,IDC_VIDEOCARD,WM_SETTEXT,0,(LPARAM)"Bi-linear Filtering");
			}
		}

		if(GUI.AspectRatio)
			SendDlgItemMessage(hDlg, IDC_ASPECT, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if(GUI.FullScreen)
			SendDlgItemMessage(hDlg, IDC_FULLSCREEN, BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
		if((GUI.outputMethod==DIRECT3D&&GUI.d3dFilter==BILINEAR)||(GUI.outputMethod==DIRECTDRAW&&GUI.ddrawUseVideoMemory))
			SendDlgItemMessage(hDlg,IDC_VIDEOCARD, BM_SETCHECK, (WPARAM)BST_CHECKED,0);
		EnableWindow(GetDlgItem(hDlg, IDC_LOCALVIDMEM),
			(GUI.outputMethod==DIRECT3D)?FALSE:GUI.ddrawUseVideoMemory);
		if(GUI.outputMethod==DIRECT3D)
			EnableWindow(GetDlgItem(hDlg,IDC_TESTMODE),FALSE);
		if(GUI.ddrawUseLocalVidMem)
			SendDlgItemMessage(hDlg,IDC_LOCALVIDMEM, BM_SETCHECK, (WPARAM)BST_CHECKED,0);

		EnableWindow(GetDlgItem(hDlg, IDC_ASPECT), GUI.Stretch);

		SendDlgItemMessage(hDlg,IDC_OUTPUTMETHOD,CB_ADDSTRING,0,(LPARAM)"DirectDraw");
		SendDlgItemMessage(hDlg,IDC_OUTPUTMETHOD,CB_ADDSTRING,0,(LPARAM)"Direct3D");
		SendDlgItemMessage(hDlg,IDC_OUTPUTMETHOD,CB_SETCURSEL,(WPARAM)GUI.outputMethod,0);
		// add all the GUI.Scale filters to the combo box
		for(int filter = 0 ; filter < (int)NUM_FILTERS ; filter++)
		{
#ifdef USE_OPENGL // hide OpenGL Bilinear from the dialog. It can still be set in the .cfg file, but right now OpenGL mode is a little too buggy (some black things render as white) to be in the GUI's display menu.
			if(filter == (int)FILTER_OPENGL && (int)FILTER_OPENGL+1 == (int)NUM_FILTERS && GUI.Scale != (int)FILTER_OPENGL) break;
#endif
			strcpy(temp,GetFilterName((RenderFilter)filter));
			SendDlgItemMessage(hDlg,IDC_FILTERBOX,CB_ADDSTRING,0,(LPARAM) (LPCTSTR)temp);
		}
		for(int filter = 0 ; filter < (int)NUM_FILTERS ; filter++)
		{
#ifdef USE_OPENGL
			if(filter == (int)FILTER_OPENGL && (int)FILTER_OPENGL+1 == (int)NUM_FILTERS && GUI.Scale != (int)FILTER_OPENGL) continue;
#endif
			if(GetFilterHiResSupport((RenderFilter)filter))
			{
				strcpy(temp,GetFilterName((RenderFilter)filter));
				SendDlgItemMessage(hDlg,IDC_FILTERBOX2,CB_ADDSTRING,0,(LPARAM) (LPCTSTR)temp);
			}
		}

//		SendDlgItemMessage(hDlg,IDC_FILTERBOX2,CB_SETCURSEL,(WPARAM)GUI.NextScaleHiRes,0);
		SendDlgItemMessage(hDlg,IDC_FILTERBOX,CB_SETCURSEL,(WPARAM)GUI.NextScale,0);

		UpdateModeListBox(GetDlgItem(hDlg,IDC_VIDMODELIST));

		// have to start focus on something like this or Escape won't exit the dialog
		SetFocus(hDlg);

		goto checkUpdateFilterBox2;

		break;
	case WM_CLOSE:
	case WM_DESTROY:
		break;
	case WM_COMMAND:

		switch(LOWORD(wParam))
		{
		case IDC_STRETCH:
			if(!in_display_dlg) break;
			// for some reason this screws up the fullscreen mode clipper if it happens before the refresh
			if(IsDlgButtonChecked(hDlg, IDC_STRETCH)==BST_CHECKED)
			{
				EnableWindow(GetDlgItem(hDlg, IDC_ASPECT), TRUE);
				if(GUI.outputMethod==DIRECTDRAW)
					SendDlgItemMessage(hDlg,IDC_VIDEOCARD,WM_SETTEXT,0,(LPARAM)"Bi-linear Filtering");
				else
					EnableWindow(GetDlgItem(hDlg, IDC_VIDEOCARD),TRUE);
			}
			else
			{
				EnableWindow(GetDlgItem(hDlg, IDC_ASPECT), FALSE);
				if(GUI.outputMethod==DIRECTDRAW)
					SendDlgItemMessage(hDlg,IDC_VIDEOCARD,WM_SETTEXT,0,(LPARAM)"Use Video Memory");
				else
					EnableWindow(GetDlgItem(hDlg, IDC_VIDEOCARD),FALSE);
			}

			GUI.Stretch = (bool)(IsDlgButtonChecked(hDlg,IDC_STRETCH)==BST_CHECKED);
			// refresh screen, so the user can see the new mode
			// (assuming the dialog box isn't completely covering the game window)
			if(GUI.outputMethod==DIRECT3D)
				Direct3D.changeRenderSize(0,0);

			if(DirectDraw.Clipped) S9xReRefresh();


			// make video memory option match stretching option, but allow it to be changed via the other checkbox
			CheckDlgButton(hDlg,IDC_VIDEOCARD,GUI.Stretch);
			/* fall through */
		case IDC_VIDEOCARD:
			if(GUI.outputMethod==DIRECT3D)
			{
				GUI.d3dFilter = (IsDlgButtonChecked(hDlg,IDC_VIDEOCARD)==BST_CHECKED)?BILINEAR:NEAREST;
			}
			else if(GUI.outputMethod==DIRECTDRAW)
			{
				EnableWindow(GetDlgItem(hDlg, IDC_LOCALVIDMEM), (bool)(IsDlgButtonChecked(hDlg,IDC_VIDEOCARD)==BST_CHECKED));
				GUI.ddrawUseVideoMemory = (bool)(IsDlgButtonChecked(hDlg,IDC_VIDEOCARD)==BST_CHECKED);
			}
			//RestoreSNESDisplay ();
			//// refresh screen, so the user can see the new stretch mode
			if(GUI.outputMethod==DIRECT3D)
				Direct3D.changeRenderSize(0,0);

			if(DirectDraw.Clipped) S9xReRefresh();
			break;

		case IDC_MESSAGES_IN_IMAGE:
			if(!in_display_dlg) break;
			GUI.MessagesInImage = (bool)(IsDlgButtonChecked(hDlg,IDC_MESSAGES_IN_IMAGE)==BST_CHECKED);

			if(!GFX.InfoString || !*GFX.InfoString){
				GFX.InfoString = "Test message!";
				GFX.InfoStringTimeout = 1;
			}

			// refresh screen, so the user can see the new mode
			if(DirectDraw.Clipped) S9xReRefresh();
			break;

		case IDC_ASPECT:
			if(!in_display_dlg) break;
			GUI.AspectRatio = (bool)(IsDlgButtonChecked(hDlg,IDC_ASPECT)==BST_CHECKED);
			// refresh screen, so the user can see the new mode
			if(GUI.outputMethod==DIRECT3D)
				Direct3D.changeRenderSize(0,0);

			if(DirectDraw.Clipped) S9xReRefresh();
			break;

		case IDC_HEIGHT_EXTEND:
			if(!in_display_dlg) break;
			GUI.HeightExtend = (bool)(IsDlgButtonChecked(hDlg,IDC_HEIGHT_EXTEND)==BST_CHECKED);
			// refresh screen, so the user can see the new mode
			if(GUI.outputMethod==DIRECT3D)
				Direct3D.changeRenderSize(0,0);

			if(DirectDraw.Clipped) S9xReRefresh();
			break;
		case IDC_OUTPUTMETHOD:
			if(!in_display_dlg) break;
			if(HIWORD(wParam)==CBN_SELCHANGE) {
				OutputMethod newOut = (OutputMethod)SendDlgItemMessage(hDlg,IDC_OUTPUTMETHOD,CB_GETCURSEL,0,0);
				if(GUI.outputMethod==newOut)
					break;
				if(GUI.FullScreen)
					ToggleFullScreen();
				GUI.outputMethod=newOut;
				if(GUI.outputMethod==DIRECT3D) {
					CheckDlgButton(hDlg,IDC_VIDEOCARD,(GUI.d3dFilter==BILINEAR)?TRUE:FALSE);
					EnableWindow(GetDlgItem(hDlg, IDC_LOCALVIDMEM), FALSE);
					EnableWindow(GetDlgItem(hDlg, IDC_TESTMODE), FALSE);
					DirectDraw.DeInitializeDirectDraw();
					Direct3D.initialize(GUI.hWnd);
					Direct3D.changeRenderSize(0,0);
				} else {
					CheckDlgButton(hDlg,IDC_VIDEOCARD,GUI.ddrawUseVideoMemory);
					EnableWindow(GetDlgItem(hDlg, IDC_TESTMODE), TRUE);
					EnableWindow(GetDlgItem(hDlg, IDC_LOCALVIDMEM), IsDlgButtonChecked(hDlg, IDC_VIDEOCARD)==BST_CHECKED);
					Direct3D.deInitialize();
					DirectDraw.InitDirectDraw();
					RestoreSNESDisplay();
				}
				UpdateModeListBox(GetDlgItem(hDlg,IDC_VIDMODELIST));
				S9xSetWinPixelFormat();
				S9xInitUpdate();
				if(DirectDraw.Clipped) S9xReRefresh();
				UpdateWindow(GUI.hWnd);
			}
			break;
		case IDC_FILTERBOX:
			if(!in_display_dlg) break;
			{
				int scale = (int)SendDlgItemMessage(hDlg,IDC_FILTERBOX,CB_GETCURSEL,0,0);
				if(scale == GUI.Scale)
					break;

				const int oldScaleScale = max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes));

//				UpdateScale(GUI.Scale, scale);
				if (IS_GL_OR_GLIDE(GUI.Scale) == IS_GL_OR_GLIDE(scale))
					GUI.Scale = (RenderFilter)scale;
				else //if(!IS_GL_OR_GLIDE(GUI.Scale) && IS_GL_OR_GLIDE(scale))
					GUI.Scale = FILTER_NONE;

				const int newScaleScale = max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes));

				if(oldScaleScale != newScaleScale)
					RestoreSNESDisplay();
			}

			// refresh screen, so the user can see the new filter
			// (assuming the dialog box isn't completely covering the game window)
			if(DirectDraw.Clipped) S9xReRefresh();

			// set hi-res combo box to match the lo-res output filter as best as possible
//			if(GetFilterHiResSupport(GUI.Scale))
checkUpdateFilterBox2:
			{
				char textOriginal [256];
				SendMessage(GetDlgItem(hDlg, IDC_FILTERBOX), WM_GETTEXT, 256,(LPARAM)textOriginal);
				int count = SendDlgItemMessage(hDlg,IDC_FILTERBOX2,CB_GETCOUNT,0,0);
//				int scale = GUI.Scale;
				bool switched = false;
				for(int j=0; j<2 && !switched; j++){
					if(j){
						RenderFilter filter; // no match, set default for chosen scale
						switch(GetFilterScale(GUI.Scale)){
							case 1: filter = FILTER_SIMPLE1X; break;
				   default: case 2: filter = FILTER_SIMPLE2X; break;
							case 3: filter = FILTER_SIMPLE3X; break;
						}
						strcpy(textOriginal, GetFilterName(filter));
					}
					for(int i=0; i<=count && !switched; i++){
						int textLen = SendDlgItemMessage(hDlg,IDC_FILTERBOX2,CB_GETLBTEXTLEN,(WPARAM)i,0);
						char* text = new char[textLen+1];
						SendDlgItemMessage(hDlg,IDC_FILTERBOX2,CB_GETLBTEXT,(WPARAM)i,(LPARAM)text);
						if(!stricmp(textOriginal, text)){
							SendDlgItemMessage(hDlg,IDC_FILTERBOX2,CB_SETCURSEL,(WPARAM)i,0);
							switched = true;
						}
						delete[] text;
					}
				}
				goto updateFilterBox2;
			}
			break;
		case IDC_FILTERBOX2: // hi-res
updateFilterBox2:
			if(!in_display_dlg) break;
			{
				char text [256];
				text[0] = '\0';
				SendMessage(GetDlgItem(hDlg, IDC_FILTERBOX2), WM_GETTEXT, 256,(LPARAM)text);

				int scale = GUI.Scale;
				for(int i=0; i<NUM_FILTERS; i++)
					if(!stricmp(GetFilterName((RenderFilter)i), text))
						scale = i;

				if(scale == GUI.ScaleHiRes)
					break;

				const int oldScaleScale = max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes));

//				UpdateScale(GUI.Scale, scale);
				if (IS_GL_OR_GLIDE(GUI.ScaleHiRes) == IS_GL_OR_GLIDE(scale))
					GUI.ScaleHiRes = (RenderFilter)scale;
				else //if(!IS_GL_OR_GLIDE(GUI.ScaleHiRes) && IS_GL_OR_GLIDE(scale))
					GUI.ScaleHiRes = FILTER_NONE;

				const int newScaleScale = max(GetFilterScale(GUI.Scale), GetFilterScale(GUI.ScaleHiRes));

				if(oldScaleScale != newScaleScale)
					RestoreSNESDisplay();

				// refresh screen, so the user can see the new filter
				// (assuming the dialog box isn't completely covering the game window)
				if(DirectDraw.Clipped) S9xReRefresh();
			}
			break;

		case IDOK:
			in_display_dlg = false;
 			Settings.Transparency = IsDlgButtonChecked(hDlg, IDC_TRANS);
//			GUI.ddrawUseVideoMemory = (bool)(IsDlgButtonChecked(hDlg,IDC_VIDEOCARD)==BST_CHECKED);
			GUI.ddrawUseLocalVidMem = (bool)(IsDlgButtonChecked(hDlg,IDC_LOCALVIDMEM)==BST_CHECKED);
			if(!GUI.FullScreen || (GUI.Width >= 512 && GUI.Height >= 478) || GUI.Stretch)
				Settings.SupportHiRes = IsDlgButtonChecked(hDlg, IDC_HIRES);
			else
				Settings.SupportHiRes = false;
			GUI.HeightExtend = IsDlgButtonChecked(hDlg, IDC_HEIGHT_EXTEND)!=0;
			GUI.MessagesInImage = IsDlgButtonChecked(hDlg, IDC_MESSAGES_IN_IMAGE)!=0;
//			Settings.Mode7Interpolate = IsDlgButtonChecked(hDlg, IDC_BILINEARMD7);
			GUI.tripleBuffering = (bool)(IsDlgButtonChecked(hDlg, IDC_DBLBUFFER)==BST_CHECKED);

			GUI.Stretch = (bool)(IsDlgButtonChecked(hDlg, IDC_STRETCH)==BST_CHECKED);
			GUI.AspectRatio = (bool)(IsDlgButtonChecked(hDlg, IDC_ASPECT)==BST_CHECKED);
			GUI.FullScreen = (bool)(IsDlgButtonChecked(hDlg, IDC_FULLSCREEN)==BST_CHECKED);

			// we might've changed the region that the game draws over
			// (by turning on "maintain aspect ratio", or turning on "extend height" when "maintain aspect ratio" is already on),
			// so we must invalidate the window to redraw black
			// behind the possibly-newly-revealed areas of the window
			RedrawWindow((HWND)GetParent(hDlg),NULL,NULL, RDW_INVALIDATE | RDW_INTERNALPAINT | RDW_ERASENOW);

			WinSaveConfigFile();

			if(!GUI.FullScreen || (GUI.Width >= 512 && GUI.Height >= 478))
				GUI.NextScale = (RenderFilter)SendDlgItemMessage(hDlg,IDC_FILTERBOX,CB_GETCURSEL,0,0);
			else
				GUI.NextScale = FILTER_NONE;

			if(!GUI.FullScreen || (GUI.Width >= 512 && GUI.Height >= 478))
				GUI.NextScaleHiRes = GUI.ScaleHiRes;
			else
				GUI.NextScaleHiRes = FILTER_SIMPLE1X;

			EndDialog(hDlg,0);
			if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}
			if(GUI.outputMethod==DIRECT3D) {
				Direct3D.changeRenderSize(0,0);
				if(GUI.FullScreen) {
					GUI.FullScreen = false;
					ToggleFullScreen();
				}
			}
			return false;



		case IDCANCEL:
			ComboBox_SetCurSel(GetDlgItem(hDlg,IDC_OUTPUTMETHOD),prevOutputMethod);
			SendMessage(hDlg,WM_COMMAND,MAKEWPARAM(IDC_OUTPUTMETHOD,CBN_SELCHANGE),0);

			in_display_dlg = false;

			{
				//UpdateScale(GUI.Scale, prevScale);
				GUI.Scale = GUI.NextScale = (RenderFilter)prevScale;
				GUI.ScaleHiRes = GUI.NextScaleHiRes = (RenderFilter)prevScaleHiRes;
				GFX.RealPPL = prevPPL;
				GUI.Stretch = prevStretch;
				GUI.MessagesInImage = prevMessagesInImage;
				GUI.ddrawUseVideoMemory = prevVideoMemory;
				GUI.d3dFilter = prevD3dFilter;
				GUI.AspectRatio = prevAspectRatio;
				GUI.HeightExtend = prevHeightExtend;

//				if(DirectDraw.Clipped) S9xReRefresh();
			}

			EndDialog(hDlg,0);
			if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				}

			return false;
		case IDC_TESTMODE:
			if(GUI.outputMethod==DIRECT3D)
				return false;

			//DirectDraw.lpDD->SetCooperativeLevel(hDlg,DDSCL_FULLSCREEN|DDSCL_ALLOWMODEX|DDSCL_EXCLUSIVE|DDSCL_ALLOWREBOOT);
			index=ListView_GetSelectionMark(GetDlgItem(hDlg,IDC_VIDMODELIST));
			if(index != -1)
			{
				sprintf(temp,"%dx%d %dbit %dHz",(int)dm.at(index).width,(int)dm.at(index).height,(int)dm.at(index).depth,(int)dm.at(index).rate);

				// XXX: TODO: set the selected refresh rate too! (this defaults to highest possible rate instead)
				if(DirectDraw.lpDD->SetDisplayMode(dm.at(index).width,dm.at(index).height,dm.at(index).depth)!=DD_OK)
				{

					MessageBox(hDlg,"There was an error testing the selected mode","DD_NOTOK",MB_OK);
					LVITEM lvi;
					dm.at(index).status=1;
					strcpy(s,"Failed");
					ZeroMemory(&lvi, sizeof(LVITEM));
					lvi.mask=LVIF_TEXT;
					lvi.iItem=index;
					lvi.iSubItem=1;
					lvi.pszText=s;
					lvi.cchTextMax=10;
					ListView_SetItem(GetDlgItem(hDlg,IDC_VIDMODELIST), &lvi);
					// NYI: saving which display modes have been tested
					//if(RegOpenKeyEx(HKEY_CURRENT_USER,MY_REG_KEY  "\\1.x\\DisplayModes",0,KEY_ALL_ACCESS, &hKey)==ERROR_SUCCESS)
					//{
					//
					//	RegSetValueEx(hKey,temp,0,REG_DWORD,(LPBYTE)&dm.at(index).status,sizeof(DWORD));
					//
					//	RegCloseKey(hKey);
					//
					//}
				}
				else
				{




					if(MessageBox(hDlg,"Did it Work?","Test Display Mode",MB_YESNO)==IDYES)
					{
						dm.at(index).status=3;
						LVITEM lvi;
						strcpy(s,"Works");
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.mask=LVIF_TEXT;
						lvi.iItem=index;
						lvi.iSubItem=1;
						lvi.pszText=s;
						lvi.cchTextMax=10;
						ListView_SetItem(GetDlgItem(hDlg,IDC_VIDMODELIST), &lvi);
						// NYI: saving which display modes have been tested
						//if(RegOpenKeyEx(HKEY_CURRENT_USER,MY_REG_KEY  "\\1.x\\DisplayModes",0,KEY_ALL_ACCESS, &hKey)==ERROR_SUCCESS)
						//{
						//
						//	if(RegSetValueEx(hKey,temp,0,REG_DWORD,(LPBYTE)&dm.at(index).status,sizeof(DWORD))==ERROR_SUCCESS)
						//	{
						//
						//	}
						//
						//	RegCloseKey(hKey);
						//
						//}
					}
					else
					{
						dm.at(index).status=1;
						LVITEM lvi;
						strcpy(s,"Failed");
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.mask=LVIF_TEXT;
						lvi.iItem=index;
						lvi.iSubItem=1;
						lvi.pszText=s;
						lvi.cchTextMax=10;
						ListView_SetItem(GetDlgItem(hDlg,IDC_VIDMODELIST), &lvi);
						// NYI: saving which display modes have been tested
						//if(RegOpenKeyEx(HKEY_CURRENT_USER,
						//	MY_REG_KEY  "\\1.x\\DisplayModes",
						//	0,KEY_ALL_ACCESS, &hKey)==ERROR_SUCCESS)
						//{
						//	RegSetValueEx(hKey,temp,0,REG_DWORD,(LPBYTE)&dm.at(index).status,sizeof(DWORD));
						//
						//	RegCloseKey(hKey);
						//
						//}

					}


				}


				// XXX: TODO: set the selected refresh rate too! (this defaults to highest possible rate instead)
				DirectDraw.lpDD->SetDisplayMode(GUI.Width,GUI.Height,GUI.Depth);
			}
			else
			{
				MessageBox(hDlg,"Please select a mode to test","No Mode Selected",MB_OK);

			}
			return false;
		case IDC_SETDMODE:
			bool go;


			index=ListView_GetSelectionMark(GetDlgItem(hDlg,IDC_VIDMODELIST));
			if(index==-1)
				return false;

			switch (dm.at(index).status)
			{
			case 3:
				if(MessageBox(hDlg,"Are you sure?","Confirm Set Display Mode",MB_YESNO)==IDYES)
				{
					go=true;
				}
				else
				{
					go=false;
				}
				break;
			case 1:
				if(MessageBox(hDlg,"This mode failed in testing, are you sure?","Confirm Set Display Mode",MB_YESNO)==IDYES)
				{
					go=true;
				}
				else
				{
					go=false;
				}
				break;
			case 0:
				if(MessageBox(hDlg,"This Mode Hasn't been tested, are you sure?","Confirm Set Display Mode",MB_YESNO)==IDYES)
				{
					go=true;
				}
				else
				{
					go=false;
				}
				break;
			}
			if(go)
			{
				GUI.Width=dm.at(index).width;
				GUI.Height=dm.at(index).height;
				GUI.Depth=dm.at(index).depth;
				GUI.RefreshRate=dm.at(index).rate;
			}
			if(!GUI.FullScreen || (GUI.Width >= 512 && GUI.Height >= 478) || GUI.Stretch)
			{
				EnableWindow(GetDlgItem(hDlg,IDC_FILTERBOX),true);
				EnableWindow(GetDlgItem(hDlg,IDC_HIRES),true);
			}
			else
			{
				EnableWindow(GetDlgItem(hDlg,IDC_FILTERBOX),false);
				EnableWindow(GetDlgItem(hDlg,IDC_HIRES),false);
			}
			sprintf(s,"Current: %dx%d %dbit %dHz",GUI.Width,GUI.Height,GUI.Depth,GUI.RefreshRate);
			SendDlgItemMessage(hDlg,IDC_CURRMODE,WM_SETTEXT,0,(LPARAM)s);
			return false;


		}


	}

	return false;
}

static void set_buttoninfo(int index, HWND hDlg)
{
	SendDlgItemMessage(hDlg,IDC_UP,WM_USER+44,Joypad[index].Up,0);
	SendDlgItemMessage(hDlg,IDC_LEFT,WM_USER+44,Joypad[index].Left,0);
	SendDlgItemMessage(hDlg,IDC_DOWN,WM_USER+44,Joypad[index].Down,0);
	SendDlgItemMessage(hDlg,IDC_RIGHT,WM_USER+44,Joypad[index].Right,0);
	SendDlgItemMessage(hDlg,IDC_A,WM_USER+44,Joypad[index].A,0);
	SendDlgItemMessage(hDlg,IDC_B,WM_USER+44,Joypad[index].B,0);
	SendDlgItemMessage(hDlg,IDC_X,WM_USER+44,Joypad[index].X,0);
	SendDlgItemMessage(hDlg,IDC_Y,WM_USER+44,Joypad[index].Y,0);
	SendDlgItemMessage(hDlg,IDC_L,WM_USER+44,Joypad[index].L,0);
	SendDlgItemMessage(hDlg,IDC_R,WM_USER+44,Joypad[index].R,0);
	SendDlgItemMessage(hDlg,IDC_START,WM_USER+44,Joypad[index].Start,0);
	SendDlgItemMessage(hDlg,IDC_SELECT,WM_USER+44,Joypad[index].Select,0);
	if(index < 5)
	{
		SendDlgItemMessage(hDlg,IDC_UPLEFT,WM_USER+44,Joypad[index].Left_Up,0);
		SendDlgItemMessage(hDlg,IDC_UPRIGHT,WM_USER+44,Joypad[index].Right_Up,0);
		SendDlgItemMessage(hDlg,IDC_DWNLEFT,WM_USER+44,Joypad[index].Left_Down,0);
		SendDlgItemMessage(hDlg,IDC_DWNRIGHT,WM_USER+44,Joypad[index].Right_Down,0);
	}
}

void TranslateKey(WORD keyz,char *out);
//HWND funky;
SJoyState JoystickF [16];


#ifdef NETPLAY_SUPPORT
INT_PTR CALLBACK DlgNPProgress(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	SendDlgItemMessage(hDlg,IDC_NPPROGRESS,PBM_SETRANGE,0,(LPARAM)MAKELPARAM (0, 100));
	SendDlgItemMessage(hDlg,IDC_NPPROGRESS,PBM_SETPOS,(WPARAM)(int)NetPlay.PercentageComplete,0);

	return false;
}
#endif
INT_PTR CALLBACK DlgInputConfig(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	char temp[256];
	short C;
	int i, which;
	static int index=0;


	static SJoypad pads[10];


	//HBRUSH g_hbrBackground;

	InitInputCustomControl();
switch(msg)
	{
		case WM_PAINT:
		{
			PAINTSTRUCT ps;
			BeginPaint (hDlg, &ps);
			if(hBmp)
			{
				BITMAP bmp;
				ZeroMemory(&bmp, sizeof(BITMAP));
				RECT r;
				GetClientRect(hDlg, &r);
				HDC hdc=GetDC(hDlg);
				HDC hDCbmp=CreateCompatibleDC(hdc);
				GetObject(hBmp, sizeof(BITMAP), &bmp);
				HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
				StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
				SelectObject(hDCbmp, hOldBmp);
				DeleteDC(hDCbmp);
				ReleaseDC(hDlg, hdc);
			}

			EndPaint (hDlg, &ps);
		}
		return true;
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		SetWindowText(hDlg,INPUTCONFIG_TITLE);
		SetDlgItemText(hDlg,IDC_JPTOGGLE,INPUTCONFIG_JPTOGGLE);
		SetDlgItemText(hDlg,IDC_OK,BUTTON_OK);
		SetDlgItemText(hDlg,IDC_CANCEL,BUTTON_CANCEL);
///		SetDlgItemText(hDlg,IDC_DIAGTOGGLE,INPUTCONFIG_DIAGTOGGLE);
		SetDlgItemText(hDlg,IDC_LABEL_UP,INPUTCONFIG_LABEL_UP);
		SetDlgItemText(hDlg,IDC_LABEL_DOWN,INPUTCONFIG_LABEL_DOWN);
		SetDlgItemText(hDlg,IDC_LABEL_LEFT,INPUTCONFIG_LABEL_LEFT);
		SetDlgItemText(hDlg,IDC_LABEL_A,INPUTCONFIG_LABEL_A);
		SetDlgItemText(hDlg,IDC_LABEL_B,INPUTCONFIG_LABEL_B);
		SetDlgItemText(hDlg,IDC_LABEL_X,INPUTCONFIG_LABEL_X);
		SetDlgItemText(hDlg,IDC_LABEL_Y,INPUTCONFIG_LABEL_Y);
		SetDlgItemText(hDlg,IDC_LABEL_L,INPUTCONFIG_LABEL_L);
		SetDlgItemText(hDlg,IDC_LABEL_R,INPUTCONFIG_LABEL_R);
		SetDlgItemText(hDlg,IDC_LABEL_START,INPUTCONFIG_LABEL_START);
		SetDlgItemText(hDlg,IDC_LABEL_SELECT,INPUTCONFIG_LABEL_SELECT);
		SetDlgItemText(hDlg,IDC_LABEL_UPRIGHT,INPUTCONFIG_LABEL_UPRIGHT);
		SetDlgItemText(hDlg,IDC_LABEL_UPLEFT,INPUTCONFIG_LABEL_UPLEFT);
		SetDlgItemText(hDlg,IDC_LABEL_DOWNRIGHT,INPUTCONFIG_LABEL_DOWNRIGHT);
		SetDlgItemText(hDlg,IDC_LABEL_DOWNLEFT,INPUTCONFIG_LABEL_DOWNLEFT);
		SetDlgItemText(hDlg,IDC_LABEL_BLUE,INPUTCONFIG_LABEL_BLUE);

		for(i=5;i<10;i++)
			Joypad[i].Left_Up = Joypad[i].Right_Up = Joypad[i].Left_Down = Joypad[i].Right_Down = 0;

		hBmp=(HBITMAP)LoadImage(NULL, TEXT("PBortas.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
		memcpy(pads, Joypad, 10*sizeof(SJoypad));

		for( i=0;i<256;i++)
			GetAsyncKeyState(i);

		for( C = 0; C != 16; C ++)
	        JoystickF[C].Attached = joyGetDevCaps( JOYSTICKID1+C, &JoystickF[C].Caps, sizeof( JOYCAPS)) == JOYERR_NOERROR;

		for(i=1;i<6;i++)
		{
			sprintf(temp,INPUTCONFIG_JPCOMBO,i);
			SendDlgItemMessage(hDlg,IDC_JPCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)temp);
		}

		for(i=6;i<11;i++)
		{
			sprintf(temp,INPUTCONFIG_JPCOMBO INPUTCONFIG_LABEL_CONTROLLER_TURBO_PANEL_MOD,i-5);
			SendDlgItemMessage(hDlg,IDC_JPCOMBO,CB_ADDSTRING,0,(LPARAM)(LPCTSTR)temp);
		}

		SendDlgItemMessage(hDlg,IDC_JPCOMBO,CB_SETCURSEL,(WPARAM)0,0);

		SendDlgItemMessage(hDlg,IDC_JPTOGGLE,BM_SETCHECK, Joypad[index].Enabled ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
		SendDlgItemMessage(hDlg,IDC_ALLOWLEFTRIGHT,BM_SETCHECK, Settings.UpAndDown ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);

		set_buttoninfo(index,hDlg);

		EnableDisableKeyFields(index,hDlg);

		PostMessage(hDlg,WM_COMMAND, CBN_SELCHANGE<<16, 0);

		SetFocus(GetDlgItem(hDlg,IDC_JPCOMBO));

		return true;
		break;
	case WM_CLOSE:
		EndDialog(hDlg, 0);
		return TRUE;
	case WM_USER+46:
		// refresh command, for clicking away from a selected field
		index = SendDlgItemMessage(hDlg,IDC_JPCOMBO,CB_GETCURSEL,0,0);
		if(index > 4) index += 3; // skip controllers 6, 7, and 8 in the input dialog
		set_buttoninfo(index,hDlg);
		return TRUE;
	case WM_USER+43:
		//MessageBox(hDlg,"USER+43 CAUGHT","moo",MB_OK);
		which = GetDlgCtrlID((HWND)lParam);
		switch(which)
		{
		case IDC_UP:
			Joypad[index].Up = wParam;

			break;
		case IDC_DOWN:
			Joypad[index].Down = wParam;

			break;
		case IDC_LEFT:
			Joypad[index].Left = wParam;

			break;
		case IDC_RIGHT:
			Joypad[index].Right = wParam;

			break;
		case IDC_A:
			Joypad[index].A = wParam;

			break;
		case IDC_B:
			Joypad[index].B = wParam;

			break;
		case IDC_X:
			Joypad[index].X = wParam;

			break;
		case IDC_Y:
			Joypad[index].Y = wParam;

			break;
		case IDC_L:
			Joypad[index].L = wParam;
			break;

		case IDC_R:
			Joypad[index].R = wParam;

			break;
		case IDC_SELECT:
			Joypad[index].Select = wParam;

			break;
		case IDC_START:
			Joypad[index].Start = wParam;

			break;
		case IDC_UPLEFT:
			Joypad[index].Left_Up = wParam;

			break;
		case IDC_UPRIGHT:
			Joypad[index].Right_Up = wParam;

			break;
		case IDC_DWNLEFT:
			Joypad[index].Left_Down = wParam;

			break;
		case IDC_DWNRIGHT:
			Joypad[index].Right_Down = wParam;

			break;

		}

		set_buttoninfo(index,hDlg);

		PostMessage(hDlg,WM_NEXTDLGCTL,0,0);
		return true;
	case WM_COMMAND:
		switch(LOWORD(wParam))
		{
		case IDCANCEL:
			memcpy(Joypad, pads, 10*sizeof(SJoypad));
			EndDialog(hDlg,0);
			if(hBmp)
			{
				DeleteObject(hBmp);
				hBmp=NULL;
			}
			break;

		case IDOK:
			Settings.UpAndDown = IsDlgButtonChecked(hDlg, IDC_ALLOWLEFTRIGHT);
			WinSaveConfigFile();
			EndDialog(hDlg,0);
			if(hBmp)
			{
				DeleteObject(hBmp);
				hBmp=NULL;
			}
			break;

		case IDC_JPTOGGLE: // joypad Enable toggle
			index = SendDlgItemMessage(hDlg,IDC_JPCOMBO,CB_GETCURSEL,0,0);
			if(index > 4) index += 3; // skip controllers 6, 7, and 8 in the input dialog
			Joypad[index].Enabled=IsDlgButtonChecked(hDlg,IDC_JPTOGGLE);
			set_buttoninfo(index, hDlg); // update display of conflicts
			break;

		}
		switch(HIWORD(wParam))
		{
			case CBN_SELCHANGE:
				index = SendDlgItemMessage(hDlg,IDC_JPCOMBO,CB_GETCURSEL,0,0);
				SendDlgItemMessage(hDlg,IDC_JPCOMBO,CB_SETCURSEL,(WPARAM)index,0);
				if(index > 4) index += 3; // skip controllers 6, 7, and 8 in the input dialog
				if(index < 8)
				{
					SendDlgItemMessage(hDlg,IDC_JPTOGGLE,BM_SETCHECK, Joypad[index].Enabled ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
					EnableWindow(GetDlgItem(hDlg,IDC_JPTOGGLE),TRUE);
				}
				else
				{
					SendDlgItemMessage(hDlg,IDC_JPTOGGLE,BM_SETCHECK, Joypad[index-8].Enabled ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
					EnableWindow(GetDlgItem(hDlg,IDC_JPTOGGLE),FALSE);
				}

				set_buttoninfo(index,hDlg);

				EnableDisableKeyFields(index,hDlg);

				break;
		}
		return FALSE;

	}

	return FALSE;
}

static void set_hotkeyinfo(HWND hDlg)
{
	HotkeyPage page = (HotkeyPage) SendDlgItemMessage(hDlg,IDC_HKCOMBO,CB_GETCURSEL,0,0);
	SCustomKey *key = CustomKeys.key;
	int i = 0;

	while (!IsLastCustomKey(key) && i < NUM_HOTKEY_CONTROLS) {
		if (page == key->page) {
			SendDlgItemMessage(hDlg, IDC_HOTKEY_Table[i], WM_USER+44, key->key, key->modifiers);
			SetDlgItemText(hDlg, IDC_LABEL_HK_Table[i], key->name);
			ShowWindow(GetDlgItem(hDlg, IDC_HOTKEY_Table[i]), SW_SHOW);
			i++;
		}
		key++;
	}
	// disable unused controls
	for (; i < NUM_HOTKEY_CONTROLS; i++) {
		SendDlgItemMessage(hDlg, IDC_HOTKEY_Table[i], WM_USER+44, 0, 0);
		SetDlgItemText(hDlg, IDC_LABEL_HK_Table[i], INPUTCONFIG_LABEL_UNUSED);
		ShowWindow(GetDlgItem(hDlg, IDC_HOTKEY_Table[i]), SW_HIDE);
	}
}

// DlgHotkeyConfig
INT_PTR CALLBACK DlgHotkeyConfig(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	int i, which;
	static HotkeyPage page = (HotkeyPage) 0;


	static SCustomKeys keys;

	//HBRUSH g_hbrBackground;
	InitKeyCustomControl();
switch(msg)
	{
		case WM_PAINT:
		{
			PAINTSTRUCT ps;
			BeginPaint (hDlg, &ps);

			EndPaint (hDlg, &ps);
		}
		return true;
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		SetWindowText(hDlg,HOTKEYS_TITLE);

		// insert hotkey page list items
		for(i = 0 ; i < NUM_HOTKEY_PAGE ; i++)
		{
			SendDlgItemMessage(hDlg, IDC_HKCOMBO, CB_ADDSTRING, 0, (LPARAM)(LPCTSTR)hotkeyPageTitle[i]);
		}

		SendDlgItemMessage(hDlg,IDC_HKCOMBO,CB_SETCURSEL,(WPARAM)0,0);

		InitCustomKeys(&keys);
		CopyCustomKeys(&keys, &CustomKeys);
		for( i=0;i<256;i++)
		{
			GetAsyncKeyState(i);
		}

		SetDlgItemText(hDlg,IDC_LABEL_BLUE,HOTKEYS_LABEL_BLUE);

		set_hotkeyinfo(hDlg);

		PostMessage(hDlg,WM_COMMAND, CBN_SELCHANGE<<16, 0);

		SetFocus(GetDlgItem(hDlg,IDC_HKCOMBO));


		return true;
		break;
	case WM_CLOSE:
		EndDialog(hDlg, 0);
		return TRUE;
	case WM_USER+46:
		// refresh command, for clicking away from a selected field
		page = (HotkeyPage) SendDlgItemMessage(hDlg, IDC_HKCOMBO, CB_GETCURSEL, 0, 0);
		set_hotkeyinfo(hDlg);
		return TRUE;
	case WM_USER+43:
	{
		//MessageBox(hDlg,"USER+43 CAUGHT","moo",MB_OK);
		int modifiers = GetModifiers(wParam);

		page = (HotkeyPage) SendDlgItemMessage(hDlg, IDC_HKCOMBO, CB_GETCURSEL, 0, 0);
		TCHAR text[256];

		which = GetDlgCtrlID((HWND)lParam);
		for (i = 0; i < NUM_HOTKEY_CONTROLS; i++) {
			if (which == IDC_HOTKEY_Table[i])
				break;
		}
		GetDlgItemText(hDlg, IDC_LABEL_HK_Table[i], text, COUNT(text));

		SCustomKey *key = CustomKeys.key;
		while (!IsLastCustomKey(key)) {
			if (page == key->page) {
				if (lstrcmp(text, key->name) == 0) {
					key->key = wParam;
					key->modifiers = modifiers;
					break;
				}
			}
			key++;
		}

		set_hotkeyinfo(hDlg);
		PostMessage(hDlg,WM_NEXTDLGCTL,0,0);
//		PostMessage(hDlg,WM_KILLFOCUS,0,0);
	}
		return true;
	case WM_COMMAND:
		switch(LOWORD(wParam))
		{
		case IDCANCEL:
			CopyCustomKeys(&CustomKeys, &keys);
			EndDialog(hDlg,0);
			break;
		case IDOK:
			WinSaveConfigFile();
			EndDialog(hDlg,0);
			break;
		}
		switch(HIWORD(wParam))
		{
			case CBN_SELCHANGE:
				page = (HotkeyPage) SendDlgItemMessage(hDlg, IDC_HKCOMBO, CB_GETCURSEL, 0, 0);
				SendDlgItemMessage(hDlg, IDC_HKCOMBO, CB_SETCURSEL, (WPARAM)page, 0);

				set_hotkeyinfo(hDlg);

				SetFocus(GetDlgItem(hDlg, IDC_HKCOMBO));

				break;
		}
		return FALSE;

	}

	return FALSE;
}

static void set_movieinfo(const char* path, HWND hDlg)
{
	MovieInfo m;
	int i;
	int getInfoResult=FILE_NOT_FOUND;

	if(strlen(path))
		getInfoResult = S9xMovieGetInfo(path, &m);

	if(getInfoResult!=FILE_NOT_FOUND)
	{
		char* p;
		char tmpstr[128];
		strncpy(tmpstr, ctime(&m.TimeCreated), 127);
		tmpstr[127]='\0';
		if((p=strrchr(tmpstr, '\n')))
			*p='\0';
		SetWindowTextA(GetDlgItem(hDlg, IDC_MOVIE_DATE), tmpstr);

		uint32 div = Memory.ROMFramesPerSecond;
		if(!div) div = 60;
		uint32 l=(m.LengthFrames+(div>>1))/div;
		uint32 seconds=l%60;
		l/=60;
		uint32 minutes=l%60;
		l/=60;
		uint32 hours=l%60;
		sprintf(tmpstr, "%02d:%02d:%02d", hours, minutes, seconds);
		SetWindowTextA(GetDlgItem(hDlg, IDC_MOVIE_LENGTH), tmpstr);
		sprintf(tmpstr, "%u", m.LengthFrames);
		SetWindowTextA(GetDlgItem(hDlg, IDC_MOVIE_FRAMES), tmpstr);
		sprintf(tmpstr, "%u", m.RerecordCount);
		SetWindowTextA(GetDlgItem(hDlg, IDC_MOVIE_RERECORD), tmpstr);
	}
	else
	{
		SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_DATE), _T(""));
		SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_LENGTH), _T(""));
		SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_FRAMES), _T(""));
		SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_RERECORD), _T(""));
	}

	if(getInfoResult==SUCCESS)
	{
		// set author comment:
		{
///			SetWindowTextW(GetDlgItem(hDlg, IDC_MOVIE_METADATA), m.Metadata); // won't work, because of & symbol

			WCHAR metadata [MOVIE_MAX_METADATA];
			int j, pos = 0, len = wcslen(m.Metadata);
			for (j = 0; j < len ; j++)
			{
				WCHAR c = m.Metadata [j];
				metadata [pos++] = c;

				// & is a special character in Windows fields,
				// so we have to change & to && when copying over the game title
				// otherwise "Pocky & Rocky" will show up as "Pocky  Rocky", for example
				if(c == (WCHAR)'&')
					metadata [pos++] = (WCHAR)'&';
			}
			metadata [pos] = (WCHAR)'\0';

			SetWindowTextW(GetDlgItem(hDlg, IDC_MOVIE_METADATA), metadata);
		}
		SetWindowText(GetDlgItem(hDlg, IDC_LABEL_MOVIEINFOBOX), _T(MOVIE_LABEL_AUTHORINFO));

		if(m.ReadOnly)
		{
			EnableWindow(GetDlgItem(hDlg, IDC_READONLY), FALSE);
			SendDlgItemMessage(hDlg,IDC_READONLY,BM_SETCHECK,BST_CHECKED,0);
		}
		else
		{
			EnableWindow(GetDlgItem(hDlg, IDC_READONLY), TRUE);
///			SendDlgItemMessage(hDlg,IDC_READONLY,BM_SETCHECK,BST_UNCHECKED,0);
		}
		EnableWindow(GetDlgItem(hDlg, IDC_DISPLAY_INPUT), TRUE);

		for(i=0; i<5; ++i)
		{
			SendDlgItemMessage(hDlg,IDC_JOY1+i,BM_SETCHECK,(m.ControllersMask & (1<<i)) ? BST_CHECKED : BST_UNCHECKED,0);
		}

		if(m.Opts & MOVIE_OPT_FROM_RESET)
		{
			SendDlgItemMessage(hDlg,IDC_RECORD_NOW,BM_SETCHECK,BST_UNCHECKED,0);
			SendDlgItemMessage(hDlg,IDC_RECORD_RESET,BM_SETCHECK,BST_CHECKED,0);
		}
		else
		{
			SendDlgItemMessage(hDlg,IDC_RECORD_RESET,BM_SETCHECK,BST_UNCHECKED,0);
			SendDlgItemMessage(hDlg,IDC_RECORD_NOW,BM_SETCHECK,BST_CHECKED,0);
		}


//		if(m.SyncFlags & MOVIE_SYNC_DATA_EXISTS)
		{
			SendDlgItemMessage(hDlg,IDC_ALLOWLEFTRIGHT,BM_SETCHECK,    (m.SyncFlags & MOVIE_SYNC_LEFTRIGHT)!=0  ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_ENVX,BM_SETCHECK,              (m.SyncFlags & MOVIE_SYNC_VOLUMEENVX)!=0 ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_FMUT,BM_SETCHECK,              (m.SyncFlags & MOVIE_SYNC_FAKEMUTE)!=0   ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_SYNC_TO_SOUND_CPU,BM_SETCHECK, (m.SyncFlags & MOVIE_SYNC_SYNCSOUND)!=0  ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
	//		SetWindowText(GetDlgItem(hDlg, IDC_LOADEDFROMMOVIE), _T(MOVIE_LABEL_SYNC_DATA_FROM_MOVIE));
		}

		{
			char str [256];

			if(m.SyncFlags & MOVIE_SYNC_HASROMINFO)
			{
				sprintf(str, MOVIE_INFO_MOVIEROM MOVIE_INFO_ROMINFO, m.ROMCRC32, m.ROMName);
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIEROMINFO), _T(str));
			}
			else
			{
				sprintf(str, MOVIE_INFO_MOVIEROM MOVIE_INFO_ROMNOTSTORED);
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIEROMINFO), _T(str));
			}

			bool mismatch = (m.SyncFlags & MOVIE_SYNC_HASROMINFO) && m.ROMCRC32 != Memory.ROMCRC32;
			sprintf(str, MOVIE_INFO_CURRENTROM MOVIE_INFO_ROMINFO "%s", Memory.ROMCRC32, Memory.ROMName, mismatch?MOVIE_INFO_MISMATCH:"");
			SetWindowText(GetDlgItem(hDlg, IDC_CURRENTROMINFO), _T(str));

			sprintf(str, "%s", mismatch?MOVIE_WARNING_MISMATCH:MOVIE_WARNING_OK);
			SetWindowText(GetDlgItem(hDlg, IDC_PLAYWARN), _T(str));
		}

		EnableWindow(GetDlgItem(hDlg, IDOK), TRUE);
	}
	else
	{
		// get the path of (where we think) the movie is
		char tempPathStr [512];
		strncpy(tempPathStr, path, 512);
		tempPathStr[511] = '\0';
		char* slash = strrchr(tempPathStr, '\\');
		slash = max(slash, strrchr(tempPathStr, '/'));
		if(slash) *slash = '\0';
		char tempDirStr [512];
		char dirStr [768];
		_fullpath(tempDirStr, tempPathStr, 512);
		char* documeStr = strstr(tempDirStr, "Documents and Settings");
		if(documeStr) { // abbreviate
			strcpy(documeStr, documeStr+14);
			strncpy(documeStr, "docume~1", 8);
		}
		sprintf(dirStr, MOVIE_INFO_DIRECTORY, tempDirStr);

		switch(getInfoResult)
		{
			default:
				SetWindowText(GetDlgItem(hDlg, IDC_PLAYWARN), _T(""));
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_METADATA), _T(""));
				break;
			case FILE_NOT_FOUND:
				SetWindowText(GetDlgItem(hDlg, IDC_PLAYWARN), _T(MOVIE_ERR_NOT_FOUND_SHORT));
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_METADATA), _T(MOVIE_ERR_NOT_FOUND));
				break;
			case WRONG_FORMAT:
				SetWindowText(GetDlgItem(hDlg, IDC_PLAYWARN), _T(MOVIE_ERR_WRONG_FORMAT_SHORT));
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_METADATA), _T(MOVIE_ERR_WRONG_FORMAT));
				break;
			case WRONG_VERSION:
				SetWindowText(GetDlgItem(hDlg, IDC_PLAYWARN), _T(MOVIE_ERR_WRONG_VERSION_SHORT));
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_METADATA), _T(MOVIE_ERR_WRONG_VERSION));
				break;
		}
		SetWindowText(GetDlgItem(hDlg, IDC_LABEL_MOVIEINFOBOX), _T(MOVIE_LABEL_ERRORINFO));

		EnableWindow(GetDlgItem(hDlg, IDC_READONLY), FALSE);
		EnableWindow(GetDlgItem(hDlg, IDC_DISPLAY_INPUT), FALSE);
//		SendDlgItemMessage(hDlg,IDC_READONLY,BM_SETCHECK,BST_UNCHECKED,0);
//		SendDlgItemMessage(hDlg,IDC_DISPLAY_INPUT,BM_SETCHECK,BST_UNCHECKED,0);


		{
			SendDlgItemMessage(hDlg,IDC_ALLOWLEFTRIGHT,BM_SETCHECK, Settings.UpAndDown ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_ENVX,BM_SETCHECK, Settings.SoundEnvelopeHeightReading ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_FMUT,BM_SETCHECK, Settings.FakeMuteFix ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_SYNC_TO_SOUND_CPU,BM_SETCHECK, Settings.SoundSync ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
		}

		{
			char str [256];

			// no movie loaded
			SetWindowText(GetDlgItem(hDlg, IDC_MOVIEROMINFO), dirStr);

			sprintf(str, MOVIE_INFO_CURRENTROM MOVIE_INFO_ROMINFO, Memory.ROMCRC32, Memory.ROMName);
			SetWindowText(GetDlgItem(hDlg, IDC_CURRENTROMINFO), _T(str));
		}

		EnableWindow(GetDlgItem(hDlg, IDOK), FALSE);
	}

}

INT_PTR CALLBACK DlgOpenMovie(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static OpenMovieParams* op=NULL;
	static char movieDirectory [MAX_PATH];
	static char movieDirectoryOFN [MAX_PATH];

	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			SetCurrentDirectory(S9xGetDirectory(DEFAULT_DIR));
			_fullpath (movieDirectory, GUI.MovieDir, MAX_PATH);
			mkdir(movieDirectory);
			SetCurrentDirectory(movieDirectory);
			lstrcpy(movieDirectoryOFN, movieDirectory);

			op=(OpenMovieParams*)lParam;

			// get default filename
			if(Memory.ROMFilename[0]!='\0')
			{
				static TCHAR filename [_MAX_PATH + 1];
				TCHAR drive [_MAX_DRIVE + 1];
				TCHAR dir [_MAX_DIR + 1];
				TCHAR fname [_MAX_FNAME + 1];
				TCHAR ext [_MAX_EXT + 1];
				_splitpath (Memory.ROMFilename, drive, dir, fname, ext);
				_makepath (filename, "", "", fname, "smv");
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_PATH), filename);
				set_movieinfo(filename, hDlg);
			}
			else
			{
				set_movieinfo("", hDlg);
			}

			SendDlgItemMessage(hDlg,IDC_READONLY,BM_SETCHECK,GUI.MovieReadOnly ? BST_CHECKED : BST_UNCHECKED,0);

			//EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), Settings.SoundDriver<1||Settings.SoundDriver>3); // can't sync sound to CPU unless using "Snes9x DirectSound" driver

			EnableWindow(GetDlgItem(hDlg, IDC_ENVX), FALSE); // shouldn't change these two from how it was recorded
			EnableWindow(GetDlgItem(hDlg, IDC_FMUT), FALSE);

			SendDlgItemMessage(hDlg,IDC_SEEK_TO_FRAME,BM_SETCHECK,BST_UNCHECKED,0);
			EnableWindow(GetDlgItem(hDlg, IDC_FRAME_NUMBER), FALSE);

			SetDlgItemText(hDlg,IDC_LABEL_STARTSETTINGS, MOVIE_LABEL_STARTSETTINGS);
			SetDlgItemText(hDlg,IDC_LABEL_CONTROLLERSETTINGS, MOVIE_LABEL_CONTSETTINGS);
			SetDlgItemText(hDlg,IDC_LABEL_SYNCSETTINGS, MOVIE_LABEL_SYNCSETTINGS);
		}
		DragAcceptFiles(hDlg, true);
		return true;

	case WM_DROPFILES: {
		HDROP hDrop;
		//UINT fileNo;
		UINT fileCount;
		char filename[PATH_MAX];

		hDrop = (HDROP)wParam;
		fileCount = DragQueryFile(hDrop, 0xFFFFFFFF, NULL, 0);
		if (fileCount > 0) {
			DragQueryFile(hDrop, 0, filename, COUNT(filename));
			SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_PATH), filename);
		}
		DragFinish(hDrop);
		SetCurrentDirectory(movieDirectory);
		return true;
	}

	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDC_ALLOWLEFTRIGHT:
			case IDC_ENVX:
			case IDC_FMUT:
			case IDC_SYNC_TO_SOUND_CPU:
				SetWindowText(GetDlgItem(hDlg, IDC_LOADEDFROMMOVIE), _T(""));
				break;
			case IDC_BROWSE_MOVIE:
				{
					OPENFILENAME  ofn;
					char  szFileName[MAX_PATH];

					strcpy(szFileName, S9xGetFilenameRel("smv"));

					ZeroMemory( (LPVOID)&ofn, sizeof(OPENFILENAME) );
					ofn.lStructSize = sizeof(OPENFILENAME);
					ofn.hwndOwner = hDlg;
					ofn.lpstrFilter = MOVIE_FILETYPE_DESCRIPTION "\0*.smv\0" FILE_INFO_ANY_FILE_TYPE "\0*.*\0\0";
					ofn.lpstrFile = szFileName;
					ofn.lpstrDefExt = "smv";
					ofn.nMaxFile = MAX_PATH;
					ofn.Flags = OFN_HIDEREADONLY | OFN_FILEMUSTEXIST; // hide previously-ignored read-only checkbox (the real read-only box is in the open-movie dialog itself)
					ofn.lpstrInitialDir = movieDirectoryOFN;
					if(GetOpenFileName( &ofn ))
					{
						SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_PATH), szFileName);

						TCHAR temp [MAX_PATH];
						GetCurrentDirectory(MAX_PATH, temp);
						lstrcpy(movieDirectoryOFN, temp);
						if(!GUI.LockDirectories) {
							absToRel(GUI.MovieDir, temp, S9xGetDirectory(DEFAULT_DIR));
						}

						set_movieinfo(szFileName, hDlg);
					}
					SetCurrentDirectory(movieDirectory);
				}
				return true;

			case IDC_MOVIE_PATH:
				{
					char  szFileName[MAX_PATH];
					GetWindowText(GetDlgItem(hDlg, IDC_MOVIE_PATH), szFileName, MAX_PATH);
					set_movieinfo(szFileName, hDlg);
				}
				break;

			case IDC_SEEK_TO_FRAME:
				{
					if(IsDlgButtonChecked(hDlg, IDC_SEEK_TO_FRAME)==BST_CHECKED)
					{
						EnableWindow(GetDlgItem(hDlg, IDC_FRAME_NUMBER), TRUE);
						SendDlgItemMessage(hDlg, IDC_FRAME_NUMBER, EM_SETSEL, 0, -1);
					}
					else
					{
						EnableWindow(GetDlgItem(hDlg, IDC_FRAME_NUMBER), FALSE);
					}
				}
				break;

			case IDOK:
				{
					if(BST_CHECKED==SendDlgItemMessage(hDlg, IDC_SEEK_TO_FRAME, BM_GETCHECK,0,0))
					{
						BOOL length_is_set;
						unsigned int movie_length = GetDlgItemInt(hDlg, IDC_MOVIE_FRAMES, &length_is_set, FALSE);
						unsigned int frame_dest = GetDlgItemInt(hDlg, IDC_FRAME_NUMBER, NULL, FALSE);
						if (length_is_set && frame_dest <= movie_length)
						{
							Settings.HighSpeedSeek = frame_dest;
							Settings.Paused = false;
							Settings.FrameAdvance = false;
							GUI.FrameAdvanceJustPressed = 0;
						}
						else
						{
							MessageBox(hDlg, TEXT("You have entered an out of range or invalid value for the frame number"), TEXT("Range Error"), MB_OK);
							return true;
						}
					}
					else
					{
						Settings.HighSpeedSeek = 0;
					}
					if(BST_CHECKED==SendDlgItemMessage(hDlg, IDC_READONLY, BM_GETCHECK,0,0))
					{
						op->ReadOnly=TRUE;
						GUI.MovieReadOnly=TRUE;
					}
					else
						GUI.MovieReadOnly=FALSE;
					if(BST_CHECKED==SendDlgItemMessage(hDlg, IDC_DISPLAY_INPUT, BM_GETCHECK,0,0))
						op->DisplayInput=TRUE;
					GetDlgItemText(hDlg, IDC_MOVIE_PATH, op->Path, MAX_PATH);
					SetCurrentDirectory(movieDirectory);
				}
				Settings.UpAndDown = IsDlgButtonChecked(hDlg, IDC_ALLOWLEFTRIGHT);
//				Settings.SoundEnvelopeHeightReading = IsDlgButtonChecked(hDlg, IDC_ENVX); // set in movie.cpp on playback
//				Settings.FakeMuteFix = IsDlgButtonChecked(hDlg, IDC_FMUT); // set in movie.cpp on playbacks
				Settings.SoundSync = IsDlgButtonChecked(hDlg, IDC_SYNC_TO_SOUND_CPU);
				EndDialog(hDlg, 1);
				return true;

			case IDCANCEL:
				EndDialog(hDlg, 0);
				return true;

			default:
				break;
			}
		}

	default:
		return false;
	}
}

// checks if the currently loaded ROM has an SRAM file in the saves directory that we have write access to
static bool existsSRAM ()
{
  return(!access(S9xGetFilename(".srm", SRAM_DIR), R_OK|W_OK));
}

INT_PTR CALLBACK DlgCreateMovie(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static OpenMovieParams* op=NULL;
	static char movieDirectory [MAX_PATH];
	static char movieDirectoryOFN [MAX_PATH];

	switch(msg)
	{
	case WM_INITDIALOG:
		{
			if(DirectDraw.Clipped) S9xReRefresh();
			SetCurrentDirectory(S9xGetDirectory(DEFAULT_DIR));
			_fullpath (movieDirectory, GUI.MovieDir, MAX_PATH);
			mkdir(movieDirectory);
			SetCurrentDirectory(movieDirectory);
			lstrcpy(movieDirectoryOFN, movieDirectory);

			// have to save here or the SRAM file might not exist when we check for it
			// (which would cause clear SRAM option to not work)
			Memory.SaveSRAM(S9xGetFilename (".srm", SRAM_DIR));


			op=(OpenMovieParams*)lParam;

			SendDlgItemMessage(hDlg,IDC_RECORD_RESET,BM_SETCHECK,BST_UNCHECKED,0);

			int i;
			for(i=1; i<5; ++i)
			{
				SendDlgItemMessage(hDlg,IDC_JOY1+i,BM_SETCHECK,BST_UNCHECKED,0);
			}
			SendDlgItemMessage(hDlg,IDC_JOY1,BM_SETCHECK,BST_CHECKED,0);

			// get default filename
			if(Memory.ROMFilename[0]!='\0')
			{
				static TCHAR filename [_MAX_PATH + 1];
				TCHAR drive [_MAX_DRIVE + 1];
				TCHAR dir [_MAX_DIR + 1];
				TCHAR fname [_MAX_FNAME + 1];
				TCHAR ext [_MAX_EXT + 1];
				_splitpath (Memory.ROMFilename, drive, dir, fname, ext);
				_makepath (filename, "", "", fname, "smv");
				SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_PATH), filename);
			}

			SendDlgItemMessage(hDlg,IDC_ALLOWLEFTRIGHT,BM_SETCHECK, Settings.UpAndDown ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
//			SendDlgItemMessage(hDlg,IDC_ENVX,BM_SETCHECK, Settings.SoundEnvelopeHeightReading ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
//			SendDlgItemMessage(hDlg,IDC_FMUT,BM_SETCHECK, Settings.FakeMuteFix ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SendDlgItemMessage(hDlg,IDC_ENVX,BM_SETCHECK, (WPARAM)BST_UNCHECKED, 0); // I realized that this should almost *always* be off when recording, because whenever it makes any difference to have it off, it prevents desyncs. Let the user turn it on  each time if they really want it on  when recording.
			SendDlgItemMessage(hDlg,IDC_FMUT,BM_SETCHECK, (WPARAM)BST_CHECKED, 0);   // I realized that this should almost *always* be on  when recording, because whenever it makes any difference to have it on,  it prevents desyncs. Let the user turn it off each time if they really want it off when recording.
			SendDlgItemMessage(hDlg,IDC_SYNC_TO_SOUND_CPU,BM_SETCHECK, Settings.SoundSync ? (WPARAM)BST_CHECKED : (WPARAM)BST_UNCHECKED, 0);
			SetWindowText(GetDlgItem(hDlg, IDC_LOADEDFROMMOVIE), _T(""));

			//EnableWindow(GetDlgItem(hDlg, IDC_SYNC_TO_SOUND_CPU), Settings.SoundDriver<1||Settings.SoundDriver>3); // can't sync sound to CPU unless using "Snes9x DirectSound" driver

			SendDlgItemMessage(hDlg,IDC_RECORD_RESET,BM_SETCHECK, (WPARAM)(GUI.MovieStartFromReset ? BST_CHECKED : BST_UNCHECKED), 0);
			SendDlgItemMessage(hDlg,IDC_RECORD_NOW,BM_SETCHECK, (WPARAM)(GUI.MovieStartFromReset ? BST_UNCHECKED : BST_CHECKED), 0);
			if(existsSRAM())
			{
				EnableWindow(GetDlgItem(hDlg, IDC_CLEARSRAM), GUI.MovieStartFromReset);
				SendDlgItemMessage(hDlg,IDC_CLEARSRAM,BM_SETCHECK, (WPARAM)(GUI.MovieClearSRAM ? BST_CHECKED : BST_UNCHECKED), 0);
			}
			else
			{
				EnableWindow(GetDlgItem(hDlg, IDC_CLEARSRAM), false);
				SendDlgItemMessage(hDlg,IDC_CLEARSRAM,BM_SETCHECK, (WPARAM)BST_CHECKED, 0);
			}

			SetDlgItemText(hDlg,IDC_LABEL_STARTSETTINGS, MOVIE_LABEL_STARTSETTINGS);
			SetDlgItemText(hDlg,IDC_LABEL_CONTROLLERSETTINGS, MOVIE_LABEL_CONTSETTINGS);
			SetDlgItemText(hDlg,IDC_LABEL_SYNCSETTINGS, MOVIE_LABEL_SYNCSETTINGS);
		}
		DragAcceptFiles(hDlg, true);
		return true;

	case WM_DROPFILES: {
		HDROP hDrop;
		//UINT fileNo;
		UINT fileCount;
		char filename[PATH_MAX];

		hDrop = (HDROP)wParam;
		fileCount = DragQueryFile(hDrop, 0xFFFFFFFF, NULL, 0);
		if (fileCount > 0) {
			DragQueryFile(hDrop, 0, filename, COUNT(filename));
			SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_PATH), filename);
		}
		DragFinish(hDrop);
		SetCurrentDirectory(movieDirectory);
		return true;
	}

	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDC_BROWSE_MOVIE:
				{
					OPENFILENAME  ofn;
					char  szFileName[MAX_PATH];

					strcpy(szFileName, S9xGetFilenameRel("smv"));

					ZeroMemory( (LPVOID)&ofn, sizeof(OPENFILENAME) );
					ofn.lStructSize = sizeof(OPENFILENAME);
					ofn.hwndOwner = hDlg;
					ofn.lpstrFilter = MOVIE_FILETYPE_DESCRIPTION "\0*.smv\0" FILE_INFO_ANY_FILE_TYPE "\0*.*\0\0";
					ofn.lpstrFile = szFileName;
					ofn.lpstrDefExt = "smv";
					ofn.nMaxFile = MAX_PATH;
					ofn.Flags = OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT;
					ofn.lpstrInitialDir = movieDirectoryOFN;
					if(GetSaveFileName( &ofn ))
					{
						SetWindowText(GetDlgItem(hDlg, IDC_MOVIE_PATH), szFileName);

						TCHAR temp [MAX_PATH];
						GetCurrentDirectory(MAX_PATH, temp);
						lstrcpy(movieDirectoryOFN, temp);
						if(!GUI.LockDirectories) {
							absToRel(GUI.MovieDir, temp, S9xGetDirectory(DEFAULT_DIR));
						}
					}
					SetCurrentDirectory(movieDirectory);
				}
				return true;

			case IDOK:
				{
					GetDlgItemText(hDlg, IDC_MOVIE_PATH, op->Path, MAX_PATH);
					GetDlgItemTextW(hDlg, IDC_MOVIE_METADATA, (WCHAR*)op->Metadata, MOVIE_MAX_METADATA);
					int i;
					for(i=wcslen(op->Metadata); i<32; i++)
						wcscat(op->Metadata, L" ");
					op->ControllersMask=0;
					op->Opts=0;
					for(i=0; i<5; ++i) // TODO: should we even bother with 8-controller recording? right now there are only 5 controller buttons in the dialog, so...
					{
						if(BST_CHECKED==SendDlgItemMessage(hDlg, IDC_JOY1+i, BM_GETCHECK,0,0))
							op->ControllersMask |= (1<<i);
					}
					if(BST_CHECKED==SendDlgItemMessage(hDlg, IDC_RECORD_RESET, BM_GETCHECK,0,0))
					{
						op->Opts |= MOVIE_OPT_FROM_RESET;
						GUI.MovieStartFromReset = TRUE;
					}
					else
						GUI.MovieStartFromReset = FALSE;

					Settings.UpAndDown = IsDlgButtonChecked(hDlg, IDC_ALLOWLEFTRIGHT);
					Settings.SoundEnvelopeHeightReading = IsDlgButtonChecked(hDlg, IDC_ENVX);
					Settings.FakeMuteFix = IsDlgButtonChecked(hDlg, IDC_FMUT);
					Settings.SoundSync = IsDlgButtonChecked(hDlg, IDC_SYNC_TO_SOUND_CPU);

					op->SyncFlags = MOVIE_SYNC_DATA_EXISTS | MOVIE_SYNC_HASROMINFO;
					if(Settings.UpAndDown) op->SyncFlags |= MOVIE_SYNC_LEFTRIGHT;
					if(Settings.SoundEnvelopeHeightReading) op->SyncFlags |= MOVIE_SYNC_VOLUMEENVX;
					if(Settings.FakeMuteFix) op->SyncFlags |= MOVIE_SYNC_FAKEMUTE;
					if(Settings.SoundSync) op->SyncFlags |= MOVIE_SYNC_SYNCSOUND;

					if(IsDlgButtonChecked(hDlg, IDC_CLEARSRAM) && IsDlgButtonChecked(hDlg, IDC_RECORD_RESET) && existsSRAM())
					{
						GUI.MovieClearSRAM = TRUE;
						remove(S9xGetFilename (".srm", SRAM_DIR)); // delete SRAM if it exists (maybe unnecessary?)
						remove(S9xGetFilename (".srm", ROMFILENAME_DIR));
						Memory.LoadSRAM(S9xGetFilename (".srm", SRAM_DIR)); // refresh memory (hard reset)
					}
					else if(!IsDlgButtonChecked(hDlg, IDC_CLEARSRAM) && IsDlgButtonChecked(hDlg, IDC_RECORD_RESET) && existsSRAM())
					{
						GUI.MovieClearSRAM = FALSE;
					}
					SetCurrentDirectory(movieDirectory);
				}
				EndDialog(hDlg, 1);
				return true;

			case IDCANCEL:
				EndDialog(hDlg, 0);
				return true;

			case IDC_RECORD_NOW:
				if(existsSRAM())
				{
					EnableWindow(GetDlgItem(hDlg, IDC_CLEARSRAM), false);
				}
				break;

			case IDC_RECORD_RESET:
				if(existsSRAM())
				{
					EnableWindow(GetDlgItem(hDlg, IDC_CLEARSRAM), true);
				}
				break;

			default:
				break;
			}
		}

	default:
		return false;
	}
}



// MYO
void S9xHandlePortCommand(s9xcommand_t cmd, int16 data1, int16 data2)
{
	return;
}

//  NYI
const char *S9xChooseFilename (bool8 read_only)
{
	return NULL;
}

// NYI
const char *S9xChooseMovieFilename (bool8 read_only)
{
	return NULL;
}

static char WinStringInputBuf[0x1000];
static char WinStringInputMsg[0x1000];
static char WinStringInputTitle[0x1000];
INT_PTR CALLBACK DlgStringInputProc(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	switch(msg)
	{
	case WM_INITDIALOG:
		if(DirectDraw.Clipped) S9xReRefresh();
		{
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("Tsundere.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			SetDlgItemText(hDlg, IDC_STRING_INPUT, WinStringInputBuf);
			SetDlgItemText(hDlg, IDC_MESSAGE, WinStringInputMsg);
			SetWindowText(hDlg, WinStringInputTitle);
		}
		return true;

	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		BeginPaint (hDlg, &ps);
		if(hBmp)
		{
			BITMAP bmp;
			ZeroMemory(&bmp, sizeof(BITMAP));
			RECT r;
			GetClientRect(hDlg, &r);
			HDC hdc=GetDC(hDlg);
			HDC hDCbmp=CreateCompatibleDC(hdc);
			GetObject(hBmp, sizeof(BITMAP), &bmp);
			HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, hBmp);
			StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
			SelectObject(hDCbmp, hOldBmp);
			DeleteDC(hDCbmp);
			ReleaseDC(hDlg, hdc);
		}

		EndPaint (hDlg, &ps);
	}	return true;

	case WM_CLOSE:
		EndDialog(hDlg, 0);
		return true;

	case WM_DESTROY:
		if(hBmp)
		{
			DeleteObject(hBmp);
			hBmp=NULL;
		}
		return true;

	case WM_COMMAND:
	{
		switch(LOWORD(wParam))
		{
		case IDOK:
			GetDlgItemText(hDlg, IDC_STRING_INPUT, WinStringInputBuf, COUNT(WinStringInputBuf));
			EndDialog(hDlg, 1);
			return true;

		case IDCANCEL:
			EndDialog(hDlg, 0);
			return true;

		default: return false;
		}
	}

	}
	return false;
}

const char * S9xStringInput(const char *msg)
{
	int ret;

	SecureZeroMemory(WinStringInputBuf, sizeof(WinStringInputBuf));
	SecureZeroMemory(WinStringInputMsg, sizeof(WinStringInputMsg));
	SecureZeroMemory(WinStringInputTitle, sizeof(WinStringInputTitle));
	lstrcpyn(WinStringInputMsg, msg, COUNT(WinStringInputMsg));
	lstrcpyn(WinStringInputTitle, msg, COUNT(WinStringInputTitle));

	RestoreGUIDisplay ();
	ret = DialogBox(g_hInst, MAKEINTRESOURCE(IDD_STRING_INPUT), GUI.hWnd, DlgStringInputProc);
	RestoreSNESDisplay ();

	if (ret)
		return (const char *) WinStringInputBuf; // FIXME: insecure?
	else
		return NULL;
}

void S9xToggleSoundChannel (int c)
{
	if (c == 8)
		GUI.SoundChannelEnable = 255;
    else
		GUI.SoundChannelEnable ^= 1 << c;

	S9xSetSoundControl(GUI.SoundChannelEnable);

	if (c == 8)
		S9xDisplayStateChange("All sound", true);
	else {
		static char str [64];
		sprintf(str, "Sound#%d", c + 1);
		S9xDisplayStateChange(str, GUI.SoundChannelEnable & (1 << c));
	}
}

bool S9xPollButton(uint32 id, bool *pressed){
	if(S9xMoviePlaying())
		return false;

	*pressed = false;

#define CHECK_KEY(controller, button) (!S9xGetState(Joypad[controller].button) || (ToggleJoypadStorage[controller].button && !TurboToggleJoypadStorage[controller].button) || (Timings.TotalEmulatedFrames%2 == ToggleJoypadStorage[controller].button && TurboToggleJoypadStorage[controller].button))

	extern bool S9xGetState (WORD KeyIdent);
	if (id & k_MO)	// mouse
	{
		switch (id & 0xFF)
		{
			case 0: *pressed = GUI.MouseButtons & 1 /* Left */ || ((id & k_C1) && (CHECK_KEY(0,A) || CHECK_KEY(0,L))) || ((id & k_C2) && (CHECK_KEY(1,A) || CHECK_KEY(1,L))); break;
			case 1: *pressed = GUI.MouseButtons & 2 /* Right */ || ((id & k_C1) && (CHECK_KEY(0,B) || CHECK_KEY(0,R))) || ((id & k_C2) && (CHECK_KEY(1,B) || CHECK_KEY(1,R))); break;
		}
	}
	else
	if (id & k_SS)	// superscope
	{
		switch (id & 0xFF)
		{
			case 0:	*pressed = GUI.MouseX <= 0 || GUI.MouseY <= 0 || GUI.MouseX >= IPPU.RenderedScreenWidth || GUI.MouseY >= ((IPPU.RenderedScreenHeight> 256) ? SNES_HEIGHT_EXTENDED<<1 : SNES_HEIGHT_EXTENDED) || CHECK_KEY(1,X); break;
			case 2:	*pressed = (GUI.MouseButtons & 2) /* Right */ || CHECK_KEY(1,B) || CHECK_KEY(1,R) ; break;
			case 3:	*pressed = (GUI.MouseButtons & 4) /* Middle */ || GUI.superscope_turbo || CHECK_KEY(1,Y);	GUI.superscope_turbo=0; GUI.MouseButtons &= ~4; break;
			case 4:	*pressed =                                        GUI.superscope_pause || CHECK_KEY(1,Start) || CHECK_KEY(1,Select);	break;
			case 1:	*pressed = (GUI.MouseButtons & 1) /* Left */ || CHECK_KEY(1,A) || CHECK_KEY(1,L); break;
		}
	}
	else
	if (id & k_LG)	// justifier
	{
		if (id & k_C1)
		{
			switch (id & 0xFF)
			{
				case 0:	*pressed = GUI.MouseX <= 0 || GUI.MouseY <= 0 || GUI.MouseX >= IPPU.RenderedScreenWidth || GUI.MouseY >= ((IPPU.RenderedScreenHeight> 256) ? SNES_HEIGHT_EXTENDED<<1 : SNES_HEIGHT_EXTENDED) || CHECK_KEY(0,X) || CHECK_KEY(0,Start); break;
				case 1:	*pressed = GUI.MouseButtons & 1 /* Left */  || CHECK_KEY(0,A) || CHECK_KEY(0,L); break;
				case 2: *pressed = GUI.MouseButtons & 2 /* Right */  || CHECK_KEY(1,B) || CHECK_KEY(1,R); break;
			}
		}
		else
		{
			switch (id & 0xFF)
			{
				case 0: *pressed = CHECK_KEY(1,Start) /* 2p Start */  || CHECK_KEY(1,X); break;
				case 1:	*pressed = CHECK_KEY(1,A) /* 2p A */ || CHECK_KEY(1,L); break;
				case 2: *pressed = CHECK_KEY(1,B) /* 2p B */ || CHECK_KEY(1,R); break;
			}
		}
	}

	return (true);
}

// ??? NYI
bool S9xPollAxis(uint32 id, int16 *value){
    return false;
}

bool S9xPollPointer(uint32 id, int16 *x, int16 *y){
	if(S9xMoviePlaying())
		return false;

	if (id & k_PT)
	{
		*x = GUI.MouseX;
		*y = GUI.MouseY;
	}
	else
		*x = *y = 0;
	return (true);
}

// adjusts settings based on ROM that was just loaded
void S9xPostRomInit()
{
	// "Cheats are on" message if cheats are on and active,
	// to make it less likely that someone will think there is some bug because of
	// a lingering cheat they don't realize is on
	if (Settings.ApplyCheats)
	{
		extern struct SCheatData Cheat;
	    for (uint32 i = 0; i < Cheat.num_cheats; i++)
		{
	        if (Cheat.c [i].enabled)
			{
				char String2 [1024];
				sprintf(String2, "(CHEATS ARE ON!) %s", String);
				strncpy(String, String2, 512);
				break;
			}
		}
	}

	if(!S9xMovieActive() && !startingMovie)
	{
		// revert previously forced control
		if(GUI.ControlForced!=0xff)
			GUI.ControllerOption = GUI.ControlForced;
		int prevController = GUI.ControllerOption;
		GUI.ValidControllerOptions = 0xFFFF;

		// NSRT controller settings
		if (!strncmp((const char *)Memory.NSRTHeader+24, "NSRT", 4))
		{
			switch(Memory.NSRTHeader[29])
			{
				default: // unknown or unsupported
					break;
				case 0x00: // Gamepad / Gamepad
					GUI.ControllerOption = SNES_JOYPAD;
					GUI.ValidControllerOptions = (1<<SNES_JOYPAD);
					break;
				case 0x10: // Mouse / Gamepad
					GUI.ControllerOption = SNES_MOUSE;
					GUI.ValidControllerOptions = (1<<SNES_MOUSE);
					break;
				case 0x20: // Mouse_or_Gamepad / Gamepad
					if(GUI.ControllerOption == SNES_MOUSE_SWAPPED)
						GUI.ControllerOption = SNES_MOUSE;
					if(GUI.ControllerOption != SNES_MOUSE)
						GUI.ControllerOption = SNES_JOYPAD;
					GUI.ValidControllerOptions = (1<<SNES_JOYPAD) | (1<<SNES_MOUSE);
					break;
				case 0x01: // Gamepad / Mouse
					GUI.ControllerOption = SNES_MOUSE_SWAPPED;
					GUI.ValidControllerOptions = (1<<SNES_MOUSE_SWAPPED);
					break;
				case 0x22: // Mouse_or_Gamepad / Mouse_or_Gamepad
					if(GUI.ControllerOption != SNES_MOUSE && GUI.ControllerOption != SNES_MOUSE_SWAPPED)
						GUI.ControllerOption = SNES_JOYPAD;
					GUI.ValidControllerOptions = (1<<SNES_JOYPAD) | (1<<SNES_MOUSE) | (1<<SNES_MOUSE_SWAPPED);
					break;
				case 0x03: // Gamepad / Superscope
					GUI.ControllerOption = SNES_SUPERSCOPE;
					GUI.ValidControllerOptions = (1<<SNES_SUPERSCOPE);
					break;
				case 0x04: // Gamepad / Gamepad_or_Superscope
					if(GUI.ControllerOption == SNES_JUSTIFIER || GUI.ControllerOption == SNES_JUSTIFIER_2)
						GUI.ControllerOption = SNES_SUPERSCOPE;
					if(GUI.ControllerOption != SNES_SUPERSCOPE)
						GUI.ControllerOption = SNES_JOYPAD;
					GUI.ValidControllerOptions = (1<<SNES_JOYPAD) | (1<<SNES_SUPERSCOPE);
					break;
				case 0x05: // Gamepad / Justifier
					if(GUI.ControllerOption != SNES_JUSTIFIER_2)
						GUI.ControllerOption = SNES_JUSTIFIER;
					GUI.ValidControllerOptions = (1<<SNES_JUSTIFIER) | (1<<SNES_JUSTIFIER_2);
					break;
				case 0x06: // Gamepad / Multitap_or_Gamepad
					GUI.ControllerOption = SNES_MULTIPLAYER5;
					GUI.ValidControllerOptions = (1<<SNES_MULTIPLAYER5) | (1<<SNES_JOYPAD);
					break;
				case 0x66: // Multitap_or_Gamepad / Multitap_or_Gamepad
					GUI.ControllerOption = SNES_MULTIPLAYER8;
					GUI.ValidControllerOptions = (1<<SNES_MULTIPLAYER8) | (1<<SNES_MULTIPLAYER5) | (1<<SNES_JOYPAD);
					break;
				case 0x24: // Gamepad_or_Mouse / Gamepad_or_Superscope
					if(GUI.ControllerOption == SNES_JUSTIFIER || GUI.ControllerOption == SNES_JUSTIFIER_2)
						GUI.ControllerOption = SNES_SUPERSCOPE;
					if(GUI.ControllerOption != SNES_SUPERSCOPE && GUI.ControllerOption != SNES_MOUSE)
						GUI.ControllerOption = SNES_JOYPAD;
					GUI.ValidControllerOptions = (1<<SNES_JOYPAD) | (1<<SNES_MOUSE) | (1<<SNES_SUPERSCOPE);
					break;
				case 0x27: // Gamepad_or_Mouse / Gamepad_or_Mouse_or_Superscope
					if(GUI.ControllerOption == SNES_JUSTIFIER || GUI.ControllerOption == SNES_JUSTIFIER_2)
						GUI.ControllerOption = SNES_SUPERSCOPE;
					if(GUI.ControllerOption != SNES_SUPERSCOPE && GUI.ControllerOption != SNES_MOUSE && GUI.ControllerOption != SNES_MOUSE_SWAPPED)
						GUI.ControllerOption = SNES_JOYPAD;
					GUI.ValidControllerOptions = (1<<SNES_JOYPAD) | (1<<SNES_MOUSE) | (1<<SNES_MOUSE_SWAPPED) | (1<<SNES_SUPERSCOPE);
					break;
				case 0x08: // Gamepad / Mouse_or_Multitap_or_Gamepad
					if(GUI.ControllerOption == SNES_MOUSE)
						GUI.ControllerOption = SNES_MOUSE_SWAPPED;
					if(GUI.ControllerOption == SNES_MULTIPLAYER8)
						GUI.ControllerOption = SNES_MULTIPLAYER5;
					if(GUI.ControllerOption != SNES_MULTIPLAYER5 && GUI.ControllerOption != SNES_MOUSE_SWAPPED)
						GUI.ControllerOption = SNES_JOYPAD;
					GUI.ValidControllerOptions = (1<<SNES_MOUSE_SWAPPED) | (1<<SNES_MULTIPLAYER5) | (1<<SNES_JOYPAD);
					break;
			}
		}
		else
		{
			// hmm, no NSRT header... let's cover a few cases anyway
			switch(Memory.ROMCRC32)
			{
			case 0x38C9626C: case 0xC6695E34:
				GUI.ControllerOption = SNES_MOUSE; break;
			case 0x59490CE8: case 0x59C00310: case 0xC3131B49:
				GUI.ControllerOption = SNES_SUPERSCOPE; break;
			}
		}

		// update menu and remember what (if anything) the control was forced from
		ChangeInputDevice();
		GUI.ControlForced = prevController;
	}

	// reset fast-forward and other input-related GUI state
	Settings.TurboMode = FALSE;
	GUI.superscope_turbo = 0;
	GUI.superscope_pause = 0;
	GUI.MouseButtons = 0;
	GUI.MouseX = 0;
	GUI.MouseY = 0;
	GUI.TurboMask = 0;
	GUI.FrameAdvanceJustPressed = 0;

	// black out the screen
 	for (uint32 y = 0; y < (uint32)IPPU.RenderedScreenHeight; y++)
		memset(GFX.Screen + y * GFX.RealPPL, 0, GFX.RealPPL*2);
}



#include "../font.h"
extern uint16* display_screen;
extern int display_ppl, display_width, display_height;
extern int display_fontwidth, display_fontheight, display_hfontaccessscale, display_vfontaccessscale;
extern bool8 display_paramsinited;

template<typename screenPtrType>
static inline void FontPixToScreen(char p, screenPtrType *s)
{
	if(p == '#')
	{
		*s = Settings.DisplayColor;
	}
	else if(p == '.')
	{
		static const screenPtrType black = BUILD_PIXEL(0,0,0);
		*s = black;
	}
}
template<>
static inline void FontPixToScreen(char p, uint32 *s)
{
#define CONVERT_16_TO_32(pixel) \
    (((((pixel) >> 11)        ) << /*RedShift+3*/  19) | \
     ((((pixel) >> 6)   & 0x1f) << /*GreenShift+3*/11) | \
      (((pixel)         & 0x1f) << /*BlueShift+3*/ 3))

	if(p == '#')
	{
		*s = CONVERT_16_TO_32(Settings.DisplayColor);
	}
	else if(p == '.')
	{
		static const uint32 black = CONVERT_16_TO_32(BUILD_PIXEL(0,0,0));
		*s = black;
	}
}

#define CHOOSE(c1) ((c1=='#'||X=='#') ? '#' : ((c1=='.'||X=='.') ? '.' : c1))

template<typename screenPtrType>
static inline void FontPixToScreenEPX(int x, int y, screenPtrType *s)
{
	const char X = font[y][x];                // E D H
	const char A = x>0  ?font[y][x-1]:' ';    // A X C
	const char C = x<143?font[y][x+1]:' ';    // F B G
	if (A != C)
	{
		const char D = y>0  ?font[y-1][x]:' ';
		const char B = y<125?font[y+1][x]:' ';
		if (B != D)
		{
			FontPixToScreen((D == A) ? CHOOSE(D) : X, s);
			FontPixToScreen((C == D) ? CHOOSE(C) : X, s+1);
			FontPixToScreen((A == B) ? CHOOSE(A) : X, s+display_ppl);
			FontPixToScreen((B == C) ? CHOOSE(B) : X, s+display_ppl+1);
			return;
		}
	}
	FontPixToScreen(X, s);
	FontPixToScreen(X, s+1);
	FontPixToScreen(X, s+display_ppl);
	FontPixToScreen(X, s+display_ppl+1);
}
#undef CHOOSE

#define CHOOSE(c1) ((X=='#') ? '#' : c1)
template<typename screenPtrType>
inline void FontPixToScreenEPXSimple3(int x, int y, screenPtrType *s)
{
	const char X = font[y][x];                // E D H
	const char A = x>0  ?font[y][x-1]:' ';    // A X C
	const char C = x<143?font[y][x+1]:' ';    // F B G
	const char D = y>0  ?font[y-1][x]:' ';
	const char B = y<125?font[y+1][x]:' ';
	const bool XnE = y>0  &&x>0  ?(X != font[y-1][x-1]):X!=' ';
	const bool XnF = y<125&&x<143?(X != font[y+1][x-1]):X!=' ';
	const bool XnG = y<125&&x>0  ?(X != font[y+1][x+1]):X!=' ';
	const bool XnH = y>0  &&x<143?(X != font[y-1][x+1]):X!=' ';
	const bool DA = D == A && (XnE || CHOOSE(D)!=X);
	const bool AB = A == B && (XnF || CHOOSE(A)!=X);
	const bool BC = B == C && (XnG || CHOOSE(B)!=X);
	const bool CD = C == D && (XnH || CHOOSE(C)!=X);
	FontPixToScreen(DA ? A : X, s);
	FontPixToScreen(X, s+1);
	FontPixToScreen(CD ? C : X, s+2);
	FontPixToScreen(X, s+display_ppl);
	FontPixToScreen(X, s+display_ppl+1);
	FontPixToScreen(X, s+display_ppl+2);
	FontPixToScreen(AB ? A : X, s+display_ppl+display_ppl);
	FontPixToScreen(X, s+display_ppl+display_ppl+1);
	FontPixToScreen(BC ? C : X, s+display_ppl+display_ppl+2);
}
#undef CHOOSE

template<typename screenPtrType>
void WinDisplayChar(screenPtrType *s, uint8 c) {
	if(c <= 32)
		return;
    int line = ((c - 32) >> 4) * display_fontheight;
    int offset = ((c - 32) & 15) * display_fontwidth;
    int h, w;
	bool hiRes = (IPPU.RenderedScreenWidth >= SNES_WIDTH*2);
	if(!display_paramsinited) display_ppl = Settings.OpenGLEnable ? IPPU.RenderedScreenWidth : GFX.RealPPL;
	if(display_hfontaccessscale == 1 && display_vfontaccessscale == 1) {
		if (hiRes && (display_screen == GFX.Screen || GUI.ScaleHiRes == FILTER_NONE)) {
			for(h=0; h<display_fontheight; h++, line++, s+=display_ppl-display_fontwidth*2)
				for(w=0; w<display_fontwidth; w++, s+=2) {
					FontPixToScreen(font [(line)] [(offset + w)], s);
					FontPixToScreen(font [(line)] [(offset + w)], s+1);
				}
		}
		else {
			for(h=0; h<display_fontheight; h++, line++, s+=display_ppl-display_fontwidth)
				for(w=0; w<display_fontwidth; w++, s++)
					FontPixToScreen(font [(line)] [(offset + w)], s);
		}
	} else if(display_hfontaccessscale == 2 && display_vfontaccessscale == 2) {
		for(h=0; h<display_fontheight; h+=2, line+=2, s+=2*display_ppl-display_fontwidth)
			for(w=0; w<display_fontwidth; w+=2, s+=2)
				FontPixToScreenEPX((offset + w)/display_hfontaccessscale, line/display_vfontaccessscale, s);
	} else if(display_hfontaccessscale == 3 && display_vfontaccessscale == 3) {
		for(h=0; h<display_fontheight; h+=3, line+=3, s+=3*display_ppl-display_fontwidth)
			for(w=0; w<display_fontwidth; w+=3, s+=3)
				FontPixToScreenEPXSimple3((offset + w)/display_hfontaccessscale, line/display_vfontaccessscale, s);
	} else {
		for(h=0; h<display_fontheight; h++, line++, s+=display_ppl-display_fontwidth)
			for(w=0; w<display_fontwidth; w++, s++)
				FontPixToScreen(font [(line)/display_vfontaccessscale] [(offset + w)/display_hfontaccessscale], s);
	}
}

template<typename screenPtrType>
static void WinDisplayStringI (const char *string, int lines, bool linesFromBottom, int pixelsFromLeft, bool allowWrap)
{
	if(lines <= 0)
		lines = 1;

	display_ppl /= (sizeof(screenPtrType)>>1);

	screenPtrType *Screen = (screenPtrType*)display_screen // text draw position, starting on the screen
                  + pixelsFromLeft // with this much horizontal offset
//				    * (Settings.SixteenBit ? 2 : 1)
                  + (linesFromBottom ? (display_height - display_fontheight * lines) : (display_fontheight * (lines - 1))) // and this much vertical offset
				    * display_ppl;

    int len = strlen(string);
    int max_chars = display_width / (display_fontwidth-display_hfontaccessscale);
    int char_count = 0;
	int prev_hfont_access_scale = display_hfontaccessscale;
	int prev_fontwidth = display_fontwidth;
	bool hiRes = (IPPU.RenderedScreenWidth >= SNES_WIDTH*2);

	// squash if it won't fit on 1 line and we're drawing greater than 1x scale and we're not allowing wrapping
	while(len > max_chars && !allowWrap && display_hfontaccessscale > 1)
	{
		display_fontwidth /= display_hfontaccessscale;
		display_hfontaccessscale--;
		display_fontwidth *= display_hfontaccessscale;

		max_chars = display_width / (display_fontwidth-display_hfontaccessscale);
	}

	// loop through and draw the characters
	for(int i = 0 ; i < len ; i++)
	{
		if(char_count >= max_chars || (unsigned char)string[i] < 32)
		{
			if(!allowWrap)
				break;

			Screen -= /*Settings.SixteenBit ? (display_fontwidth-display_hfontaccessscale)*sizeof(uint16)*char_count :*/ (display_fontwidth-display_hfontaccessscale)*char_count;
			Screen += display_fontheight * display_ppl;
			if(Screen >= (screenPtrType*)display_screen + display_ppl * display_height)
				break;

			char_count = 0;
		}
		if((unsigned char) string[i]<32) continue;

		WinDisplayChar(Screen, string[i]);
		Screen += /*Settings.SixteenBit ? (display_fontwidth-display_hfontaccessscale)*sizeof(uint16) :*/ (display_fontwidth-display_hfontaccessscale) * ((hiRes && (display_screen == GFX.Screen || GUI.ScaleHiRes == FILTER_NONE)) ? 2 : 1);
		char_count++;
	}

	// revert temporary change to font scale, if any
	if(display_hfontaccessscale != prev_hfont_access_scale)
	{
		display_hfontaccessscale = prev_hfont_access_scale;
		display_fontwidth = prev_fontwidth;
	}

	display_ppl *= (sizeof(screenPtrType)>>1);
}

static void WinDisplayString (const char *string, int lines, bool linesFromBottom, int pixelsFromLeft, bool allowWrap)
{
	if(GUI.ScreenDepth == 32 && GUI.DepthConverted && !Settings.AutoDisplayMessages)
	{
		WinDisplayStringI<uint32>(string, lines, linesFromBottom, pixelsFromLeft, allowWrap);
	}
	else
	{
		WinDisplayStringI<uint16>(string, lines, linesFromBottom, pixelsFromLeft, allowWrap);
	}
}
