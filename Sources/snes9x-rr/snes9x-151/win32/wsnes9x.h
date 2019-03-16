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




/*****************************************************************************/
/*  Snes9X: Win32                                                            */
/*****************************************************************************/
#if !defined(SNES9X_H_INCLUDED)
#define SNES9X_H_INCLUDED
/*****************************************************************************/

#include <stdio.h>
#include <stdlib.h>
#ifndef STRICT
#define STRICT
#endif
#include <windows.h>
#include <windowsx.h>
#include <tchar.h>
#include "ddraw.h"
#include <mmsystem.h>

#include "../port.h"

#ifndef __BORLANDC__

//#ifndef __MINGW32__
//#include <afxres.h> // disabled because it seems to be missing by default and it's not needed
//#endif

#include <dsound.h>
#endif
#include "rsrc/resource.h"

#define COUNT(a) (sizeof (a) / sizeof (a[0]))
#define GUI_VERSION 1008

extern unsigned char* SoundBuffer;
#define MAX_RECENT_GAMES_LIST_SIZE 32
#define MAX_RECENT_HOSTS_LIST_SIZE 16
#define MAX_RECENT_MACROS_LIST_SIZE 32
#define MACRO_MAX_TEXT_LENGTH   8192

/****************************************************************************/
inline static void Log (const char *str)
{
    FILE *fs = fopen ("snes9x.log", "a");

    if (fs)
    {
      fprintf (fs, "%s\n", str);
      fflush (fs);
      fclose (fs);
    }

}

enum RenderFilter{
	FILTER_NONE = 0,
	FILTER_SIMPLE1X,

	FILTER_SIMPLE2X,
	FILTER_SCANLINES,
	FILTER_TVMODE,
	FILTER_SUPEREAGLE,
	FILTER_SUPER2XSAI,
	FILTER_2XSAI,
	FILTER_HQ2X,
	FILTER_HQ2XS,
	FILTER_HQ2XBOLD,
	FILTER_EPXA,
	FILTER_EPXB,
	FILTER_EPXC,

	FILTER_SIMPLE3X,
	FILTER_TVMODE3X,
	FILTER_DOTMATRIX3X,
	FILTER_HQ3X,
	FILTER_HQ3XS,
	FILTER_HQ3XBOLD,
	FILTER_LQ3XBOLD,
	FILTER_EPX3,

	FILTER_BLARGGCOMP,
	FILTER_BLARGGSVID,
	FILTER_BLARGGRGB,
	FILTER_BLARGGCOMPBRIGHT,
	FILTER_BLARGGSVIDBRIGHT,
	FILTER_BLARGGRGBBRIGHT,

#ifdef USE_GLIDE
	FILTER_GLIDE,
#endif
#ifdef USE_OPENGL
	FILTER_OPENGL,
#endif

	NUM_FILTERS
};

enum MacroInputType {
	MACRO_INPUT_MOV = 0,
	MACRO_INPUT_OR,
	MACRO_INPUT_XOR,
};

enum OutputMethod {
	DIRECTDRAW = 0,
	DIRECT3D
};

enum D3DFilter {
	NEAREST = 0,
	BILINEAR
};

struct sGUI {
    HWND hWnd;
    HMENU hMenu;
    HINSTANCE hInstance;

    DWORD hFrameTimer;
    //DWORD hSoundTimer;
    DWORD hKeyInputTimer;
    HANDLE ClientSemaphore;
    HANDLE FrameTimerSemaphore;
    HANDLE ServerTimerSemaphore;
	unsigned int KeyInDelayMSec;
	unsigned int KeyInRepeatMSec;

    BYTE Language;

    //unsigned long PausedFramesBeforeMutingSound;
    int  Width;
    int  Height;
    int  Depth;
    int  RefreshRate;
    RenderFilter Scale;
    RenderFilter NextScale;
    RenderFilter ScaleHiRes;
    RenderFilter NextScaleHiRes;
    bool FullScreen;
    bool Stretch;
    bool HeightExtend;
    bool AspectRatio;
    bool ScreenCleared;
    bool IgnoreNextMouseMove;
    RECT window_size;
	bool window_maximized;
    bool windowResizeLocked;
    bool allowDropFiles;
	int  threadPriority;
    int  MouseX;
    int  MouseY;
    unsigned int MouseButtons;
	int superscope_turbo;
	int superscope_pause;
	int FrameAdvanceJustPressed;
    HCURSOR Blank;
    HCURSOR GunSight;
    HCURSOR Arrow;
    int CursorTimer;
    HDC  hDC;
    HACCEL Accelerators;
    bool NeedDepthConvert;
    bool DepthConverted;
    bool BGR;
	bool InactivePause;
	bool CustomRomOpen;
    bool FASkipsNonInput;
    bool FAMute;
    int  ScreenDepth;
    int  RedShift;
    int  GreenShift;
    int  BlueShift;
    int  ControlForced;
	int  CurrentSaveSlot;
    int  MaxRecentGames;
	int  ControllerOption;
	int  ValidControllerOptions;
	int  SoundChannelEnable;
	bool BackgroundInput;
	bool JoystickHotkeys;
	bool MovieClearSRAM;
	bool MovieStartFromReset;
	bool MovieReadOnly;
	bool NetplayUseJoypad1;
    RECT SizeHistory [10];
    unsigned int FlipCounter;
    unsigned int NumFlipFrames;
	bool MessagesInImage;

    char RomDir [_MAX_PATH];
    char ScreensDir [_MAX_PATH];
    char MovieDir [_MAX_PATH];
    char SPCDir [_MAX_PATH];
    char FreezeFileDir [_MAX_PATH];
    char SRAMFileDir [_MAX_PATH];
    char PatchDir [_MAX_PATH];
    char BiosDir [_MAX_PATH];
	bool LockDirectories;

#ifdef USE_OPENGL
    HGLRC hRC;
#endif
    char RecentGames [MAX_RECENT_GAMES_LIST_SIZE][MAX_PATH];
    char RecentHostNames [MAX_RECENT_HOSTS_LIST_SIZE][MAX_PATH];

	//turbo switches -- SNES-wide
	unsigned short TurboMask;
	char StarOceanPack[MAX_PATH];
	char SFA2PALPack[MAX_PATH];
	char SFA2NTSCPack[MAX_PATH];
	char SFZ2Pack[MAX_PATH];
	char SJNSPack[MAX_PATH];
	char FEOEZPack[MAX_PATH];
	char SPL4Pack[MAX_PATH];
	char MDHPack[MAX_PATH];
	COLORREF InfoColor;
	bool HideMenu;
	bool ddrawUseVideoMemory;
	bool ddrawUseLocalVidMem;
    bool tripleBuffering;
	D3DFilter d3dFilter;
	bool Vsync; // XXX: unused - OV2: used in Direct3D mode

	struct WAVFile* WAVOut;

	// avi writing
	struct AVIFile* AVIOut;
	bool AVIDoubleScale;

	OutputMethod outputMethod;
	int AspectWidth;
	bool EmulateFullscreen;
	bool EmulatedFullScreen;
	long FrameCount;
	long LastFrameCount;
	unsigned long IdleCount;
	// used for sync sound synchronization
	CRITICAL_SECTION SoundCritSect;

	MacroInputType MacroInputMode;
	bool PauseWithMacro;
	bool SaveMacroSnap;
	bool PlatformSnapIntoTempDir;
	int  MaxRecentMacros;
	char RecentMacros [MAX_RECENT_MACROS_LIST_SIZE][MACRO_MAX_TEXT_LENGTH];

	bool NotifySoundDSPRead;
	bool FlexibleSoundMixMaster;
};

//TURBO masks
#define TURBO_A_MASK 0x0001
#define TURBO_B_MASK 0x0002
#define TURBO_X_MASK 0x0004
#define TURBO_Y_MASK 0x0008
#define TURBO_L_MASK 0x0010
#define TURBO_R_MASK 0x0020
#define TURBO_STA_MASK 0x0040
#define TURBO_SEL_MASK 0x0080
#define TURBO_LEFT_MASK 0x0100
#define TURBO_UP_MASK 0x0200
#define TURBO_RIGHT_MASK 0x0400
#define TURBO_DOWN_MASK 0x0800

struct sLanguages {
    int idMenu;
    TCHAR *errInitDD;
    TCHAR *errModeDD;
    TCHAR *errInitDS;
    TCHAR *ApplyNeedRestart;
    TCHAR *errFrameTimer;
};

#define CUSTKEY_ALT_MASK   0x01
#define CUSTKEY_CTRL_MASK  0x02
#define CUSTKEY_SHIFT_MASK 0x04

enum HotkeyPage {
	HOTKEY_PAGE_FILE=0,
	HOTKEY_PAGE_SAVESTATE_1,
	HOTKEY_PAGE_SAVESTATE_2,
	HOTKEY_PAGE_CONTROLS_1,
	HOTKEY_PAGE_CONTROLS_2,
	HOTKEY_PAGE_SPEED,
	HOTKEY_PAGE_GRAPHICS,
	HOTKEY_PAGE_SOUND,
	HOTKEY_PAGE_MOVIE,
	HOTKEY_PAGE_TOOLS,
	NUM_HOTKEY_PAGE,
};

typedef void (*HotkeyHandlerDown) (bool);
typedef void (*HotkeyHandlerUp) (void);

// Hack to avoid processing certain events while SavedAtOp.
enum HotkeyTiming { PROCESS_NOW, PROCESS_AFTER_AUTO_ADVANCE, PROCESS_AFTER_MANUAL_ADVANCE };

typedef struct {
	WORD key;
	WORD modifiers;
	HotkeyHandlerDown handleKeyDown;
	HotkeyHandlerUp handleKeyUp;
	HotkeyPage page;
	LPCTSTR name;
	HotkeyTiming timing;
} SCustomKey;

typedef union {
	struct {
		SCustomKey RecentROM[10];
		SCustomKey OpenROM;
		SCustomKey OpenMultiCart;
		SCustomKey Pause;
		SCustomKey ResetGame;
		SCustomKey SaveScreenShot;
		SCustomKey SaveSPC;
		SCustomKey SaveSRAM;
		SCustomKey SaveSPC7110Log;
		SCustomKey RecordAVI;
		SCustomKey Save [10];
		SCustomKey Load [10];
		SCustomKey SelectSave [10];
		SCustomKey SlotPlus;
		SCustomKey SlotMinus;
		SCustomKey SlotSave;
		SCustomKey SlotLoad;
		SCustomKey ToggleJoypad [8];
		SCustomKey JoypadSwap;
		SCustomKey SwitchControllers;
		SCustomKey TurboA, TurboB, TurboY, TurboX, TurboL, TurboR, TurboStart, TurboSelect, TurboLeft, TurboUp, TurboRight, TurboDown;
		SCustomKey ScopeTurbo;
		SCustomKey ScopePause;
		SCustomKey SpeedUp;
		SCustomKey SpeedDown;
		SCustomKey SkipUp;
		SCustomKey SkipDown;
		SCustomKey FastForward;
		SCustomKey ToggleFastForward;
		SCustomKey FrameAdvance;
		SCustomKey BGL1;
		SCustomKey BGL2;
		SCustomKey BGL3;
		SCustomKey BGL4;
		SCustomKey BGL5;
		SCustomKey ClippingWindows;
		SCustomKey Transparency;
		SCustomKey HDMA;
		SCustomKey BGLHack;
		SCustomKey InterpMode7;
		SCustomKey GLCube;
		SCustomKey ToggleSound [8];
		SCustomKey MoviePlay;
		SCustomKey MovieRecord;
		SCustomKey MovieStop;
		SCustomKey ReadOnly;
		SCustomKey ShowPressed;
		SCustomKey FrameCount;
		SCustomKey FrameCountOnly;
		SCustomKey LagCountOnly;
		SCustomKey ResetLagCounter;
		SCustomKey ToggleMacro [8];
		SCustomKey EditMacro;
		SCustomKey ToggleCheats;
		SCustomKey LoadLuaScript;
		SCustomKey ReloadLuaScript;
		SCustomKey StopLuaScript;
		SCustomKey LastItem; // dummy, must be last
	};
	SCustomKey key[1]; // array of all the keys above (the size is not really 1)
} SCustomKeys;

struct SJoypad {
    BOOL Enabled;
    WORD Left;
    WORD Right;
    WORD Up;
    WORD Down;
    WORD Left_Up;
    WORD Left_Down;
    WORD Right_Up;
    WORD Right_Down;
    WORD Start;
    WORD Select;
    WORD A;
    WORD B;
    WORD X;
    WORD Y;
    WORD L;
    WORD R;
};

#define S9X_JOY_NEUTRAL 60

struct SJoyState{
    bool Attached;
    JOYCAPS Caps;
    int Threshold;
    bool Left;
    bool Right;
    bool Up;
    bool Down;
    bool PovLeft;
    bool PovRight;
    bool PovUp;
    bool PovDown;
    bool PovDnLeft;
    bool PovDnRight;
    bool PovUpLeft;
    bool PovUpRight;
    bool RUp;
    bool RDown;
    bool UUp;
    bool UDown;
    bool VUp;
    bool VDown;
    bool ZUp;
    bool ZDown;
    bool Button[32];
};

enum
{
	SNES_JOYPAD,
	SNES_MOUSE,
	SNES_SUPERSCOPE,
	SNES_MULTIPLAYER5,
	SNES_JUSTIFIER,
	SNES_MOUSE_SWAPPED,
	SNES_MULTIPLAYER8,
	SNES_JUSTIFIER_2,
	SNES_MAX_CONTROLLER_OPTIONS
};

struct dMode
{
	long height;
	long width;
	long depth;
	long rate;
	int status;
};

/*****************************************************************************/

void SetInfoDlgColor(unsigned char r, unsigned char g, unsigned char b);

extern struct sGUI GUI;
extern struct sLanguages Languages[];
extern struct SJoypad Joypad[16];
extern struct SJoypad ToggleJoypadStorage[8];
extern struct SJoypad TurboToggleJoypadStorage[8];
extern SCustomKeys CustomKeys;

enum
{
	WIN_SNES9X_DIRECT_SOUND_DRIVER=0,
	WIN_FMOD_DIRECT_SOUND_DRIVER,
	WIN_FMOD_WAVE_SOUND_DRIVER,
	WIN_FMOD_A3D_SOUND_DRIVER,
	WIN_XAUDIO2_SOUND_DRIVER,
	WIN_FMODEX_DEFAULT_DRIVER,
	WIN_FMODEX_ASIO_DRIVER
};

#define S9X_REG_KEY_BASE MY_REG_KEY
#define S9X_REG_KEY_VERSION REG_KEY_VER

#define EXT_WIDTH (MAX_SNES_WIDTH + 4)
#define EXT_PITCH (EXT_WIDTH * 2)
#define EXT_HEIGHT (MAX_SNES_HEIGHT + 4)
// Offset into buffer to allow a two pixel border around the whole rendered
// SNES image. This is a speed up hack to allow some of the image processing
// routines to access black pixel data outside the normal bounds of the buffer.
#define EXT_OFFSET (EXT_PITCH * 2 + 2 * 2)

#define WIN32_WHITE RGB(255,255,255)

#define SET_UI_COLOR(r,g,b) SetInfoDlgColor(r,g,b)

/*****************************************************************************/

bool IsLastCustomKey (const SCustomKey *key);
void SetLastCustomKey (SCustomKey *key);
void ZeroCustomKeys (SCustomKeys *keys);
void InitCustomKeys (SCustomKeys *keys);
void CopyCustomKeys (SCustomKeys *dst, const SCustomKeys *src);

void S9xSetWinPixelFormat ();
//int CheckKey( WORD Key, int OldJoypad);
//void TranslateKey(WORD keyz,char *out);



const char* GetFilterName(RenderFilter filterID);
int GetFilterScale(RenderFilter filterID);
void GetFilterRect(RenderFilter filterID, LPRECT filterRect);
bool GetFilterHiResSupport(RenderFilter filterID);

#ifdef USE_GLIDE
#define VOODOO_MODE (GUI.Scale == FILTER_GLIDE)
#define IS_GLIDE_MODE(f) (f == FILTER_GLIDE)
#else
#define VOODOO_MODE (false)
#define IS_GLIDE_MODE(f) (false)
#endif
#ifdef USE_OPENGL
#define OPENGL_MODE (GUI.Scale == FILTER_OPENGL)
#define IS_GL_MODE(f) (f == FILTER_OPENGL)
#else
#define OPENGL_MODE (false)
#define IS_GL_MODE(f) (false)
#endif
#define IS_GL_OR_GLIDE(f) ((IS_GLIDE_MODE(f)) || (IS_GL_MODE(f)))

extern uint8 *FrameSound;
bool IsSoundMuted();
bool FlexibleSoundMixMode();
void S9xWinInitSound();
void S9xWinDeinitSound();
bool S9xWinIsSoundActive();

extern HINSTANCE g_hInst;

const char *S9xGetFilenameRel (const char *ex);

struct ICheat
{
	uint32  address;
	uint32  new_val;
	uint32  saved_val;
	int     size;
	bool8   enabled;
	bool8   saved;
	char    name [22];
	int     format;
};

extern INT_PTR CALLBACK DlgAddCheat(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);
extern INT_PTR CALLBACK DlgCheater(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam);

// from ramwatch.cpp
extern bool AutoRWLoad;         //Keeps track of whether Auto-load is checked
extern bool RWSaveWindowPos;    //Keeps track of whether Save Window position is checked
extern int ramw_x, ramw_y;      //Used to store ramwatch dialog window positions

#endif // !defined(SNES9X_H_INCLUDED)
