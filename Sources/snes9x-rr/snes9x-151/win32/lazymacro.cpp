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



#include "win32-snapshot.h"
#include "lazymacro.h"
#include "wlanguage.h"
#include "CDirectDraw.h"
#include "../snes9x.h"
#include "../display.h"
#include "../ppu.h"

#include <stdio.h>
#include <stdlib.h>

#ifndef max
#define max(a,b) (((a) > (b)) ? (a) : (b))
#endif

#ifndef min
#define min(a,b) (((a) < (b)) ? (a) : (b))
#endif

UINT CharOf(LPCTSTR lpsz)
{
#ifdef UNICODE
	return (UINT) (lpsz[0]);
#else
	return (UINT) (lpsz[0] | (IsDBCSLeadByte((BYTE) lpsz[0]) ? (lpsz[1] << 8) : 0));
#endif
}

/*****************************************************************************/
/* Global variables                                                          */
/*****************************************************************************/
const int idMacroCheck[5] = {
	IDC_MACRO1_CHECK,
	IDC_MACRO2_CHECK,
	IDC_MACRO3_CHECK,
	IDC_MACRO4_CHECK,
	IDC_MACRO5_CHECK,
};
const int idMacroInput[5] = {
	IDC_MACRO1_INPUT,
	IDC_MACRO2_INPUT,
	IDC_MACRO3_INPUT,
	IDC_MACRO4_INPUT,
	IDC_MACRO5_INPUT,
};
const int idMacroStep[5] = {
	IDC_MACRO1_STEP,
	IDC_MACRO2_STEP,
	IDC_MACRO3_STEP,
	IDC_MACRO4_STEP,
	IDC_MACRO5_STEP,
};

typedef struct tagMacroData
{
	bool   enabled;     // on/off switch
	size_t pos;         // current position
	size_t len;         // total length
	size_t loopPoint;   // loop point
	bool   loopEnabled; // loop on/off
	size_t prevPos;     // previous input position
	uint16 code[MACRO_MAX_LENGTH];
	TCHAR  text[MACRO_MAX_TEXT_LENGTH];
} MacroData;

HWND inputMacroHWND = NULL;
MacroData macroData[5];

bool MacroAddFrameInput(int player, uint16 padState);
bool MacroCompileText(int player);
bool MacroUpdateCounter(int player);
bool MacroUpdateCounterAll(void);
bool MacroRestoreCounter(int player);
bool MacroRestoreCounterAll(void);

/*****************************************************************************/
/* WinProc                                                                   */
/*****************************************************************************/
extern void S9xReRefresh();

int CALLBACK DlgInputMacro(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	MacroData* playerMacro;

	switch(msg)
	{
	case WM_INITDIALOG:
		{
			if(DirectDraw.Clipped) S9xReRefresh();

			inputMacroHWND = hDlg;
			MacroInit();
		}
		return true;

	case WM_DESTROY:
		{
			inputMacroHWND = NULL;
		}
		break;

	case WM_COMMAND:
		{
			int player;

			switch(LOWORD(wParam))
			{
			case IDC_MACRO1_CHECK:
				player = 0;
				goto OnChecked;
			case IDC_MACRO2_CHECK:
				player = 1;
				goto OnChecked;
			case IDC_MACRO3_CHECK:
				player = 2;
				goto OnChecked;
			case IDC_MACRO4_CHECK:
				player = 3;
				goto OnChecked;
			case IDC_MACRO5_CHECK:
				player = 4;
				goto OnChecked;
OnChecked:
			if(player >= 0 && player < 5)
			{
				playerMacro = &macroData[player];
				MacroChangeState(player, IsDlgButtonChecked(hDlg, idMacroCheck[player])!=BST_UNCHECKED);
				return true;
			}

			case IDC_MACRO_METHOD_NONE:
			case IDC_MACRO_METHOD_OVERWRITE:
			case IDC_MACRO_METHOD_TOGGLE:
				if(IsDlgButtonChecked(hDlg, IDC_MACRO_METHOD_NONE)!=BST_UNCHECKED)
				{
					GUI.MacroInputMode = MACRO_INPUT_MOV;
				}
				else if(IsDlgButtonChecked(hDlg, IDC_MACRO_METHOD_OVERWRITE)!=BST_UNCHECKED)
				{
					GUI.MacroInputMode = MACRO_INPUT_OR;
				}
				else if(IsDlgButtonChecked(hDlg, IDC_MACRO_METHOD_TOGGLE)!=BST_UNCHECKED)
				{
					GUI.MacroInputMode = MACRO_INPUT_XOR;
				}
				return true;

			case IDC_PAUSE_WITH_MACRO:
				GUI.PauseWithMacro = (IsDlgButtonChecked(hDlg, IDC_PAUSE_WITH_MACRO)!=BST_UNCHECKED);
				return true;

			case IDCANCEL:
//				MacroDisableAll();

				if(inputMacroHWND)
//					DestroyWindow(hDlg);
					ShowWindow(hDlg, SW_HIDE);
				else
					EndDialog(hDlg, 0);
				return true;

			default: break;
			}
		}
		break;

	default: false;
	}
	return false;
}

/*****************************************************************************/
bool MacroChangeState(int player, bool state)
{
	MacroData* playerMacro;

	if(player < 0 || player >= 5)
		return false;

	playerMacro = &macroData[player];

	playerMacro->enabled = state;
	if(inputMacroHWND)
	{
		if(playerMacro->enabled)
		{
			GetDlgItemText(inputMacroHWND, idMacroInput[player], playerMacro->text, MACRO_MAX_TEXT_LENGTH);
			MacroCompileText(player);
		}

		CheckDlgButton(inputMacroHWND, idMacroCheck[player], playerMacro->enabled ? BST_CHECKED : BST_UNCHECKED);
		EnableWindow(GetDlgItem(inputMacroHWND, idMacroInput[player]), !playerMacro->enabled ? TRUE : FALSE);

		if(playerMacro->enabled && playerMacro->len)
		{
			int i;
			int numItems;
			static TCHAR tempMacroText[MACRO_MAX_TEXT_LENGTH];

			numItems = SendDlgItemMessage(inputMacroHWND, idMacroInput[0], CB_GETCOUNT, 0, 0);

			for(i = 0; i < 5; i++)
			{
				int itemIndex;
				itemIndex = SendDlgItemMessage(inputMacroHWND, idMacroInput[i], CB_FINDSTRINGEXACT, -1, (LPARAM) playerMacro->text);
				if(itemIndex == CB_ERR)
				{
					if(numItems >= GUI.MaxRecentMacros)
					{
						int lastIndex;
						lastIndex = SendDlgItemMessage(inputMacroHWND, idMacroInput[i], CB_GETCOUNT, itemIndex, 0);
						lastIndex = (lastIndex != CB_ERR) ? (lastIndex - 1) : (GUI.MaxRecentMacros - 1);

						SendDlgItemMessage(inputMacroHWND, idMacroInput[i], CB_DELETESTRING, lastIndex, 0);
					}
					SendDlgItemMessage(inputMacroHWND, idMacroInput[i], CB_INSERTSTRING, 0, (LPARAM) playerMacro->text);
				}
				else
				{
					GetDlgItemText(inputMacroHWND, idMacroInput[i], tempMacroText, MACRO_MAX_TEXT_LENGTH);
					SendDlgItemMessage(inputMacroHWND, idMacroInput[i], CB_DELETESTRING, itemIndex, 0);
					SendDlgItemMessage(inputMacroHWND, idMacroInput[i], CB_INSERTSTRING, 0, (LPARAM) playerMacro->text);
					SetDlgItemText(inputMacroHWND, idMacroInput[i], tempMacroText);
				}
			}
			SendDlgItemMessage(inputMacroHWND, idMacroInput[player], CB_SETCURSEL, 0, 0);

			// update recent macros
			numItems = SendDlgItemMessage(inputMacroHWND, idMacroInput[0], CB_GETCOUNT, 0, 0);
			for(i = 0; i < min(numItems, MAX_RECENT_MACROS_LIST_SIZE); i++)
			{
				SendDlgItemMessage(inputMacroHWND, idMacroInput[0], CB_GETLBTEXT, i, (LPARAM) GUI.RecentMacros[i]);
			}
			for(; i < MAX_RECENT_MACROS_LIST_SIZE; i++)
			{
				GUI.RecentMacros[i][0] = '\0';
			}
		}
	}

	MacroUpdateCounter(player);
	return true;
}

bool MacroToggleState(int player)
{
	if(player < 0 || player >= 5)
		return false;

	return MacroChangeState(player, !macroData[player].enabled);
}

void MacroDisableAll(void)
{
	for(int player = 0; player < 5; player++)
	{
		MacroChangeState(player, false);
	}
}

bool MacroIsEnabled(int player)
{
	MacroData* playerMacro;

	if(player < 0 || player >= 5)
		return false;

	playerMacro = &macroData[player];
	return playerMacro->enabled;
}

bool MacroSeekPos(int player, size_t pos)
{
	MacroData* playerMacro;

	if(player < 0 || player >= 5)
		return false;

	playerMacro = &macroData[player];
	if(pos < playerMacro->len)
		playerMacro->pos = pos;

//	MacroUpdateCounter(player);
	return true;
}

bool MacroSetText(int player, LPCTSTR text)
{
	MacroData* playerMacro;

	if(player < 0 || player >= 5)
		return false;

	playerMacro = &macroData[player];
	lstrcpy(playerMacro->text, text);
	if(inputMacroHWND)
	{
		SetDlgItemText(inputMacroHWND, idMacroInput[player], text);
	}
	return true;
}

uint16 MacroInput(int player)
{
	MacroData* playerMacro;
	uint16 padState;

	if(player < 0 || player >= 5 || !macroData[player].enabled)
		return 0;

	playerMacro = &macroData[player];
	MacroUpdateCounter(player);

	if(playerMacro->len == 0)
		return 0;

	padState = playerMacro->code[playerMacro->pos];
	playerMacro->prevPos = playerMacro->pos;
	if((playerMacro->pos + 1) >= playerMacro->len)
		if(playerMacro->loopEnabled)
			playerMacro->pos = playerMacro->loopPoint;
		else
		{
			MacroChangeState(player, false);

			if(GUI.PauseWithMacro)
			{
				Settings.Paused = true;
				// FIXME: This doesn't seem to be a perfect way of repainting.
				// Input display may not display correct information. Take care.
				IPPU.RenderThisFrame = true;
			}
		}
	else
		playerMacro->pos++;

	return padState;
}

extern bool S9xGetState (WORD KeyIdent);

bool MacroSaveState(LPCTSTR filename)
{
	MacroData* playerMacro;
	static TCHAR snapPath[_MAX_PATH + 1];
	TCHAR secName[64];

	GetFullPathName(filename, COUNT(snapPath), snapPath, NULL);

	WritePrivateProfileInt(_T("Macro"), _T("Version"), MACRO_VERSION, snapPath);

	for(int player = 0; player < 5; player++)
	{
		playerMacro = &macroData[player];
		wsprintf(secName, _T("Player%d"), player+1);

		WritePrivateProfileBool(secName, _T("Enabled"), playerMacro->enabled, snapPath);
		WritePrivateProfileInt(secName, _T("Position"), playerMacro->pos, snapPath);
		WritePrivateProfileInt(secName, _T("LastPosition"), playerMacro->prevPos, snapPath);
		WritePrivateProfileString(secName, _T("Text"), playerMacro->text, snapPath);
	}
	return true;
}

bool MacroLoadState(LPCTSTR filename)
{
	MacroData* playerMacro;
	static MacroData tempData;
	static TCHAR snapPath[_MAX_PATH + 1];
	TCHAR secName[64];
	int snapVersion;

	GetFullPathName(filename, COUNT(snapPath), snapPath, NULL);

	snapVersion = GetPrivateProfileInt(_T("Macro"), _T("Version"), 0, snapPath);

	for(int player = 0; player < 5; player++)
	{
		bool macroWasEnabled;

		playerMacro = &macroData[player];
		wsprintf(secName, _T("Player%d"), player+1);

		tempData.enabled = GetPrivateProfileBool(secName, _T("Enabled"), false, snapPath);
		tempData.pos = GetPrivateProfileInt(secName, _T("Position"), 0, snapPath);
		tempData.prevPos = GetPrivateProfileInt(secName, _T("LastPosition"), 0, snapPath);
		GetPrivateProfileString(secName, _T("Text"), _T(""), tempData.text, MACRO_MAX_TEXT_LENGTH, snapPath);

		macroWasEnabled = playerMacro->enabled;
		//if (macroWasEnabled || tempData.enabled)
		{
			MacroSetText(player, tempData.text);
			MacroChangeState(player, tempData.enabled);
			MacroSeekPos(player, tempData.pos);
			playerMacro->prevPos = tempData.prevPos;
			MacroRestoreCounter(player);
		}
	}
	return true;
}

/*****************************************************************************/
void MacroInit(void)
{
	static bool initialized = false;
	MacroData* playerMacro;

	if(!initialized)
	{
		for(int player = 0; player < 5; player++)
		{
			playerMacro = &macroData[player];
			playerMacro->pos = 0;
			playerMacro->len = 0;
			playerMacro->loopPoint = 0;
			playerMacro->loopEnabled = true;
			playerMacro->prevPos = 0;
			lstrcpy(playerMacro->text, _T(""));
			MacroChangeState(player, false);

			for(int i = 0; i < GUI.MaxRecentMacros; i++)
			{
				if(strcmp(GUI.RecentMacros[i], ""))
					SendDlgItemMessage(inputMacroHWND, idMacroInput[player], CB_ADDSTRING, 0, (LPARAM) GUI.RecentMacros[i]);
			}
		}

		initialized = true;
	}

	if(inputMacroHWND)
	{
		MacroRestoreCounterAll();
		switch(GUI.MacroInputMode)
		{
		case MACRO_INPUT_MOV:
			CheckRadioButton(inputMacroHWND, IDC_MACRO_METHOD_NONE, IDC_MACRO_METHOD_TOGGLE, IDC_MACRO_METHOD_NONE);
			break;
		case MACRO_INPUT_OR:
			CheckRadioButton(inputMacroHWND, IDC_MACRO_METHOD_NONE, IDC_MACRO_METHOD_TOGGLE, IDC_MACRO_METHOD_OVERWRITE);
			break;
		case MACRO_INPUT_XOR:
			CheckRadioButton(inputMacroHWND, IDC_MACRO_METHOD_NONE, IDC_MACRO_METHOD_TOGGLE, IDC_MACRO_METHOD_TOGGLE);
			break;

		default: break;
		}

		CheckDlgButton(inputMacroHWND, IDC_PAUSE_WITH_MACRO, GUI.PauseWithMacro ? BST_CHECKED : BST_UNCHECKED);
	}
}

bool MacroAddFrameInput(int player, uint16 padState)
{
	MacroData* playerMacro;

	if(player < 0 || player >= 5)
		return false;

	playerMacro = &macroData[player];

	if(playerMacro->len >= MACRO_MAX_LENGTH)
		return false;

	playerMacro->code[playerMacro->len] = padState;
	playerMacro->len++;
	return true;
}

bool MacroCompileText(int player)
{
	MacroData* playerMacro;
	uint16 padState;
	uint16 lastInput;
	uint16 toggleState;
	LPTSTR pText;
	bool exitParse;
	bool needToAdd;
	bool inComb;
	UINT modifyPressHold;
	size_t loopLevel;
	size_t ofsLoopBegin[MACRO_MAX_LOOP];
	size_t ofsLoopEnd[MACRO_MAX_LOOP];
	UINT chr;

	if(player < 0 || player >= 5)
		return false;

	playerMacro = &macroData[player];

	playerMacro->pos = 0;
	playerMacro->len = 0;
	playerMacro->loopPoint = 0;
	playerMacro->loopEnabled = true;
	playerMacro->prevPos = 0;
	pText = playerMacro->text;

	padState = 0;
	lastInput = 0;
	toggleState = 0;
	inComb = false;
	modifyPressHold = 0;
	loopLevel = 0;
	exitParse = false;
	needToAdd = false;
	while(!exitParse && (chr = CharOf(pText)) != _T('\0'))
	{
		pText = CharNext(pText);

		switch(chr)
		{
		case _T('.'):
			padState |= 0x0000;
			goto OnKeyCode;
//		case _T('0'):
//			padState |= 0x0002;
//			goto OnKeyCode;
//		case _T('1'):
//			padState |= 0x0004;
//			goto OnKeyCode;
//		case _T('2'):
//			padState |= 0x0008;
//			goto OnKeyCode;
		case _T('R'):
			padState |= 0x0010;
			goto OnKeyCode;
		case _T('L'):
			padState |= 0x0020;
			goto OnKeyCode;
		case _T('X'):
			padState |= 0x0040;
			goto OnKeyCode;
		case _T('A'):
			padState |= 0x0080;
			goto OnKeyCode;
		case _T('>'):
			padState |= 0x0100;
			goto OnKeyCode;
		case _T('<'):
			padState |= 0x0200;
			goto OnKeyCode;
		case _T('v'):
			padState |= 0x0400;
			goto OnKeyCode;
		case _T('^'):
			padState |= 0x0800;
			goto OnKeyCode;
		case _T('S'):
			padState |= 0x1000;
			goto OnKeyCode;
		case _T('s'):
			padState |= 0x2000;
			goto OnKeyCode;
		case _T('Y'):
			padState |= 0x4000;
			goto OnKeyCode;
		case _T('B'):
			padState |= 0x8000;
			goto OnKeyCode;
OnKeyCode:
		{
			if(!inComb)
			{
				if(!modifyPressHold)
				{
					exitParse = !MacroAddFrameInput(player, padState ^ toggleState);
					lastInput = padState;
				}
				else
				{
					switch(modifyPressHold)
					{
					case '+':
						toggleState |= padState;
						break;
					case '-':
						toggleState &= ~padState;
						break;
					}
					modifyPressHold = 0;
				}
				padState = 0;
				needToAdd = false;
			}
			else
			{
				needToAdd = true;
			}
			break;
		}
		case _T('*'):
			if(modifyPressHold)
			{
				switch(modifyPressHold)
				{
				case '-':
					toggleState = 0;
					break;
				}
				modifyPressHold = 0;
			}
			break;

		case _T('!'):
			if(!inComb && !loopLevel)
			{
				playerMacro->loopEnabled = false;
				needToAdd = false;
				exitParse = true;
			}
			break;

		case _T('('):
			inComb = true;
			break;

		case _T(')'):
			if(!modifyPressHold)
			{
				exitParse = !MacroAddFrameInput(player, padState ^ toggleState);
				lastInput = padState;
			}
			else
			{
				switch(modifyPressHold)
				{
				case '+':
					toggleState |= padState;
					break;
				case '-':
					toggleState &= ~padState;
					break;
				}
				modifyPressHold = 0;
			}
			padState = 0;
			needToAdd = false;

			inComb = false;
			break;

		case _T('['):
			if(loopLevel < MACRO_MAX_LOOP)
			{
				ofsLoopBegin[loopLevel] = playerMacro->len;
				inComb = false;
				loopLevel++;
			}
			break;

		case _T(']'):
			if(needToAdd)
			{
				if(!modifyPressHold)
				{
					exitParse = !MacroAddFrameInput(player, padState ^ toggleState);
					lastInput = padState;
				}
				else
				{
					switch(modifyPressHold)
					{
					case '+':
						toggleState |= padState;
						break;
					case '-':
						toggleState &= ~padState;
						break;
					}
					modifyPressHold = 0;
				}
				padState = 0;
				needToAdd = false;

				inComb = false;
			}

			if(loopLevel)
			{
				loopLevel--;

				size_t loopCodeLen;
				bool noLoopNum;
				int loopNum;

				loopNum = 0;
				noLoopNum = true;
				while((chr = CharOf(pText)) != _T('\0'))
				{
					if(chr < '0' || chr > '9')
						break;

					if(noLoopNum)
						noLoopNum = false;

					pText = CharNext(pText);
					loopNum = (loopNum * 10) + (chr - '0');
				}
				if (noLoopNum)
					loopNum = 1;

				if(playerMacro->len > ofsLoopBegin[loopLevel])
				{
					ofsLoopEnd[loopLevel] = playerMacro->len - 1;
					loopCodeLen = ofsLoopEnd[loopLevel] - ofsLoopBegin[loopLevel] + 1;

					if(loopNum == 0)
					{
						playerMacro->len = ofsLoopBegin[loopLevel];
					}
					else
					{
						for(int loopCnt = 1; !exitParse && loopCnt < loopNum; loopCnt++)
						{
							for(size_t ofsLoopCode = ofsLoopBegin[loopLevel]; !exitParse && 
								ofsLoopCode <= ofsLoopEnd[loopLevel]; ofsLoopCode++)
							{
								exitParse = !MacroAddFrameInput(player, 
									playerMacro->code[ofsLoopCode]);
							}
						}
					}
				}
			}
			break;

		case _T('|'):
			if(!inComb && !loopLevel)
			{
				playerMacro->loopPoint = playerMacro->len;
				playerMacro->loopEnabled = true;
			}
			break;

		case _T('w'):
			if(needToAdd)
			{
				if(!modifyPressHold)
				{
					exitParse = !MacroAddFrameInput(player, padState ^ toggleState);
					lastInput = padState;
				}
				else
				{
					switch(modifyPressHold)
					{
					case '+':
						toggleState |= padState;
						break;
					case '-':
						toggleState &= ~padState;
						break;
					}
					modifyPressHold = 0;
				}
				padState = 0;
				needToAdd = false;

				inComb = false;
			}

			{
				bool noFrameNum;
				int frameNum;

				frameNum = 0;
				noFrameNum = true;
				while((chr = CharOf(pText)) != _T('\0'))
				{
					if(chr < '0' || chr > '9')
						break;

					if(noFrameNum)
						noFrameNum = false;

					pText = CharNext(pText);
					frameNum = (frameNum * 10) + (chr - '0');
				}
				if (noFrameNum)
					frameNum = 1;

				for(int frameCnt = 0; !exitParse && frameCnt < frameNum; frameCnt++)
				{
					exitParse = !MacroAddFrameInput(player, lastInput ^ toggleState);
				}
			}
			break;

		case _T('r'):
			if(needToAdd)
			{
				if(!modifyPressHold)
				{
					exitParse = !MacroAddFrameInput(player, padState ^ toggleState);
					lastInput = padState;
				}
				else
				{
					switch(modifyPressHold)
					{
					case '+':
						toggleState |= padState;
						break;
					case '-':
						toggleState &= ~padState;
						break;
					}
					modifyPressHold = 0;
				}
				padState = 0;
				needToAdd = false;

				inComb = false;
			}

			{
				bool noFrameNum;
				int frameNum;

				frameNum = 0;
				noFrameNum = true;
				while((chr = CharOf(pText)) != _T('\0'))
				{
					if(chr < '0' || chr > '9')
						break;

					if(noFrameNum)
						noFrameNum = false;

					pText = CharNext(pText);
					frameNum = (frameNum * 10) + (chr - '0');
				}
				if (noFrameNum)
					frameNum = 1;

				for(int frameCnt = 0; !exitParse && frameCnt < frameNum; frameCnt++)
				{
					exitParse = !MacroAddFrameInput(player, 0 ^ toggleState);
				}
			}
			break;

		case _T('+'):
			if(!inComb)
			{
				modifyPressHold = '+';
			}
			break;

		case _T('-'):
			if(!inComb)
			{
				modifyPressHold = '-';
			}
			break;

		default: break;
		}
	}
	if(needToAdd)
	{
		if(!modifyPressHold)
		{
			exitParse = !MacroAddFrameInput(player, padState ^ toggleState);
			lastInput = padState;
		}
		else
		{
			switch(modifyPressHold)
			{
			case '+':
				toggleState |= padState;
				break;
			case '-':
				toggleState &= ~padState;
				break;
			}
			modifyPressHold = 0;
		}
	}

	return true;
}

bool MacroSetCounterValue(int player, size_t pos)
{
	MacroData* playerMacro;
	TCHAR stepText[64];

	if(player < 0 || player >= 5)
		return false;

	playerMacro = &macroData[player];
	if(playerMacro->len && pos >= playerMacro->len)
		return false;

	wsprintf(stepText, _T("%u/%u"), playerMacro->len ? (pos + 1) : 0, playerMacro->len);
	if(inputMacroHWND)
	{
		if(playerMacro->enabled)
			SetDlgItemText(inputMacroHWND, idMacroStep[player], stepText);
		else
			SetDlgItemText(inputMacroHWND, idMacroStep[player], _T(""));
	}
	return true;
}

bool MacroUpdateCounter(int player)
{
	return MacroSetCounterValue(player, macroData[player].pos);
}

bool MacroUpdateCounterAll(void)
{
	bool result = true;
	for(int player = 0; player < 5; player++)
	{
		result = result && MacroUpdateCounter(player);
	}
	return result;
}

bool MacroRestoreCounter(int player)
{
	return MacroSetCounterValue(player, macroData[player].prevPos);
}

bool MacroRestoreCounterAll(void)
{
	bool result = true;
	for(int player = 0; player < 5; player++)
	{
		result = result && MacroRestoreCounter(player);
	}
	return result;
}
