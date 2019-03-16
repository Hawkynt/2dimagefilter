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



#include "../port.h"
#include "../snes9x.h"
#include "../display.h"

#include "wsnes9x.h"
#include "lazymacro.h"
#include "win32-snapshot.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>

#include <windows.h>

/*****************************************************************************/

bool GetPrivateProfileBool(LPCTSTR lpAppName, LPCTSTR lpKeyName, bool bDefault, LPCTSTR lpFileName)
{
	static TCHAR text[256];
	GetPrivateProfileString(lpAppName, lpKeyName, bDefault ? _T("true") : _T("false"), text, COUNT(text), lpFileName);
	if(lstrcmpi(text, _T("true")) == 0)
		return true;
	else if(lstrcmpi(text, _T("false")) == 0)
		return false;
	else
		return bDefault;
}

BOOL WritePrivateProfileInt(LPCTSTR lpAppName, LPCTSTR lpKeyName, INT nValue, LPCTSTR lpFileName)
{
	static TCHAR intText[256];
	wsprintf(intText, _T("%d"), nValue);
	return WritePrivateProfileString(lpAppName, lpKeyName, intText, lpFileName);
}

BOOL WritePrivateProfileBool(LPCTSTR lpAppName, LPCTSTR lpKeyName, bool bBoolean, LPCTSTR lpFileName)
{
	return WritePrivateProfileString(lpAppName, lpKeyName, bBoolean ? _T("true") : _T("false"), lpFileName);
}

/*****************************************************************************/

void GetPlatformSnapPath (char *path, const char *base)
{
	TCHAR tempDir[MAX_PATH + 1];

	GetTempPath(MAX_PATH, tempDir);
	wsprintf(path, _T("%s\\%s.s9xw"), GUI.PlatformSnapIntoTempDir ? 
		tempDir : S9xGetDirectory(SNAPSHOT_DIR), S9xBasename(base));
}

EXTERN_C bool8 S9xFreezePlatformDepends (const char *basefilename)
{
	static TCHAR filepath [_MAX_PATH + 1];
	bool result = true;

	// GetPlatformSnapPath should return full-path always
	GetPlatformSnapPath(filepath, basefilename);

	// TODO: more abstract implementation
	result &= MacroSaveState(filepath);
	// TODO/FIXME?: they must be removed when they're stored in platform-independent snapshot
//	WritePrivateProfileInt(_T("Timings"), _T("TotalEmulatedFrames"), Timings.TotalEmulatedFrames, filepath);
//	WritePrivateProfileInt(_T("Timings"), _T("LagCounter"), Timings.LagCounter, filepath);
	return result;
}

EXTERN_C bool8 S9xUnfreezePlatformDepends (const char *basefilename)
{
	static TCHAR filepath [_MAX_PATH + 1];
	bool result = true;

	GetPlatformSnapPath(filepath, basefilename);

	// TODO: more abstract implementation
	result &= MacroLoadState(filepath);
	// TODO/FIXME?: they must be removed when they're stored in platform-independent snapshot
//	Timings.TotalEmulatedFrames = GetPrivateProfileInt(_T("Timings"), _T("TotalEmulatedFrames"), Timings.TotalEmulatedFrames, filepath);
//	Timings.LagCounter = GetPrivateProfileInt(_T("Timings"), _T("LagCounter"), Timings.LagCounter, filepath);
	return result;
}
