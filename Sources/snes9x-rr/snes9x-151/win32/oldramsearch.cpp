/*******************************************************************************
  Snes9x - Portable Super Nintendo Entertainment System (TM) emulator.
 
  (c) Copyright 1996 - 2002 Gary Henderson (gary.henderson@ntlworld.com) and
                            Jerremy Koot (jkoot@snes9x.com)

  (c) Copyright 2001 - 2004 John Weidman (jweidman@slip.net)

  (c) Copyright 2002 - 2004 Brad Jorsch (anomie@users.sourceforge.net),
                            funkyass (funkyass@spam.shaw.ca),
                            Joel Yliluoma (http://iki.fi/bisqwit/)
                            Kris Bleakley (codeviolation@hotmail.com),
                            Matthew Kendora,
                            Nach (n-a-c-h@users.sourceforge.net),
                            Peter Bortas (peter@bortas.org) and
                            zones (kasumitokoduck@yahoo.com)

  C4 x86 assembler and some C emulation code
  (c) Copyright 2000 - 2003 zsKnight (zsknight@zsnes.com),
                            _Demo_ (_demo_@zsnes.com), and Nach

  C4 C++ code
  (c) Copyright 2003 Brad Jorsch

  DSP-1 emulator code
  (c) Copyright 1998 - 2004 Ivar (ivar@snes9x.com), _Demo_, Gary Henderson,
                            John Weidman, neviksti (neviksti@hotmail.com),
                            Kris Bleakley, Andreas Naive

  DSP-2 emulator code
  (c) Copyright 2003 Kris Bleakley, John Weidman, neviksti, Matthew Kendora, and
                     Lord Nightmare (lord_nightmare@users.sourceforge.net

  OBC1 emulator code
  (c) Copyright 2001 - 2004 zsKnight, pagefault (pagefault@zsnes.com) and
                            Kris Bleakley
  Ported from x86 assembler to C by sanmaiwashi

  SPC7110 and RTC C++ emulator code
  (c) Copyright 2002 Matthew Kendora with research by
                     zsKnight, John Weidman, and Dark Force

  S-DD1 C emulator code
  (c) Copyright 2003 Brad Jorsch with research by
                     Andreas Naive and John Weidman
 
  S-RTC C emulator code
  (c) Copyright 2001 John Weidman
  
  ST010 C++ emulator code
  (c) Copyright 2003 Feather, Kris Bleakley, John Weidman and Matthew Kendora

  Super FX x86 assembler emulator code 
  (c) Copyright 1998 - 2003 zsKnight, _Demo_, and pagefault 

  Super FX C emulator code 
  (c) Copyright 1997 - 1999 Ivar, Gary Henderson and John Weidman


  SH assembler code partly based on x86 assembler code
  (c) Copyright 2002 - 2004 Marcus Comstedt (marcus@mc.pp.se) 

 
  Specific ports contains the works of other authors. See headers in
  individual files.
 
  Snes9x homepage: http://www.snes9x.com
 
  Permission to use, copy, modify and distribute Snes9x in both binary and
  source form, for non-commercial purposes, is hereby granted without fee,
  providing that this license information and copyright notice appear with
  all copies and any derived work.
 
  This software is provided 'as-is', without any express or implied
  warranty. In no event shall the authors be held liable for any damages
  arising from the use of this software.
 
  Snes9x is freeware for PERSONAL USE only. Commercial users should
  seek permission of the copyright holders first. Commercial use includes
  charging money for Snes9x or software derived from Snes9x.
 
  The copyright holders request that bug fixes and improvements to the code
  should be forwarded to them so everyone can benefit from the modifications
  in future versions.
 
  Super NES and Super Nintendo Entertainment System are trademarks of
  Nintendo Co., Limited and its subsidiary companies.
*******************************************************************************/



#include <windows.h>
#include <commctrl.h>

#include "../snes9x.h"
#include "../port.h"
#include "../display.h"
#include "../cheats.h"
#include "../memmap.h"
#include "wsnes9x.h"
#include "wlanguage.h"
#include "CDirectDraw.h"
#include "Direct3D.h"
#include "rsrc/resource.h"
#include "oldramsearch.h"

extern SCheatData Cheat;
extern void S9xReRefresh();
extern bool CheatTestRange(int val_type, S9xCheatDataSize bytes, uint32 value);

HWND oldRamSearchHWND = NULL;

static char Str_Tmp[1024];

#define TEST_BIT(a,v) \
((a)[(v) >> 5] & (1 << ((v) & 31)))

#define ITEM_QUERY(a, b, c, d, e)  ZeroMemory(&a, sizeof(LV_ITEM)); \
						a.iItem= ListView_GetSelectionMark(GetDlgItem(hDlg, b)); \
						a.iSubItem=c; \
						a.mask=LVIF_TEXT; \
						a.pszText=d; \
						a.cchTextMax=e; \
						ListView_GetItem(GetDlgItem(hDlg, b), &a);

static inline int CheatCount(int byteSub)
{
	int a, b=0;
//	for(a=0;a<0x32000-byteSub;a++)
	for(a=0;a<0x30000-byteSub;a++) // hide IRAM from cheat dialog (it seems not to be searhed correctly)
	{
		if(TEST_BIT(Cheat.ALL_BITS, a))
			b++;
	}
	return b;
}

// must return 6 characters if succeeded
static char* GetAddressStr(char* buf, int address)
{
	if (buf == NULL)
		return NULL;

	if(address < 0x7E0000 + 0x20000)
		sprintf(buf, "%06X", address);
	else if(address < 0x7E0000 + 0x30000) {
		sprintf(buf, "s%05X", address - 0x7E0000 - 0x20000);
	}
	else
		sprintf(buf, "i%05X", address - 0x7E0000 - 0x30000);
	return buf;
}

bool ResetWatchesS9X(void)
{
	for(unsigned int i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
		watches[i].on = false;
	return true;
}

bool Load_Watches_S9X(const char* filename)
{
	FILE *file = fopen(filename, "r");

	if (file == NULL)
		return false;

	ResetWatchesS9X();
	for(unsigned int i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
	{
		char nameStr [256];
		nameStr[0]='?'; nameStr[1]='\0';
		fscanf(file, " address = 0x%x, name = \"%31[^\"]\", size = %d, format = %d\n", &watches[i].address, nameStr, &watches[i].size, &watches[i].format);
		if(watches[i].size == 3)
			watches[i].size = 4;
		if(nameStr[0] == '\0' || nameStr[0] == '?')
		{
			if(watches[i].address < 0x7E0000 + 0x20000)
				sprintf(nameStr, "%06X", watches[i].address);
			else if(watches[i].address < 0x7E0000 + 0x30000)
				sprintf(nameStr, "s%05X", watches[i].address - 0x7E0000 - 0x20000);
			else
				sprintf(nameStr, "i%05X", watches[i].address - 0x7E0000 - 0x30000);
		}
		nameStr[255] = '\0';
		if(!ferror(file))
		{
			watches[i].on = true;
			watches[i].buf[0] = '\0';
			strncpy(watches[i].desc, nameStr, sizeof(watches[i].desc)); watches[i].desc[sizeof(watches[i].desc)-1]='\0';
		}
		if(ferror(file) || feof(file))
			break;
	}
	fclose(file);
	return true;
}

bool Save_Watches_S9X(const char* filename)
{
	FILE *file = fopen(filename, "w");

	if (file == NULL)
		return false;

	for(unsigned int i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
		if(watches[i].on)
//			fprintf(file, "address = 0x%x, name = \"%6X\", size = %d, format = %d\n", watches[i].address, watches[i].address, watches[i].size, watches[i].format);
			fprintf(file, "address = 0x%x, name = \"?\", size = %d, format = %d\n", watches[i].address, watches[i].size, watches[i].format);

	fclose(file);
	return true;
}

bool Load_Watches_wch(const char* filename)
{
	const char DELIM = '\t';
	FILE* WatchFile = fopen(filename,"rb");
	if (!WatchFile)
	{
		//MessageBox(MESSAGEBOXPARENT,"Error opening file.","ERROR",MB_OK);
		return false;
	}
	ResetWatchesS9X();
	char mode;
	fgets(Str_Tmp,1024,WatchFile);
	sscanf(Str_Tmp,"%c%*s",&mode);
	int WatchAdd;
	int WatchCount = 0;
	fgets(Str_Tmp,1024,WatchFile);
	sscanf(Str_Tmp,"%d%*s",&WatchAdd);
	WatchAdd+=WatchCount;
	for (int i = WatchCount; i < WatchAdd && i < MAX_WATCH_COUNT_S9X; i++)
	{
		char addressStr[16];
		char sizeChar;
		char typeChar;
		int wrongEndian;

		while (i < 0)
			i++;
		do {
			fgets(Str_Tmp,1024,WatchFile);
		} while (Str_Tmp[0] == '\n');
		sscanf(Str_Tmp,"%*05X%*c%6s%*c%c%*c%c%*c%d",&addressStr,&sizeChar,&typeChar,&wrongEndian);
		char *Comment = strrchr(Str_Tmp,DELIM) + 1;
		*strrchr(Comment,'\n') = '\0';

		//InsertWatch(Temp,Comment);
		watches[i].on = true;
		switch(sizeChar) {
		case 'b':
			watches[i].size = 1;
			break;
		case 'w':
			watches[i].size = 2;
			break;
		default:
			watches[i].size = 4;
		}
		switch(typeChar) {
		case 'h':
			watches[i].format = 3;
			break;
		case 's':
			watches[i].format = 2;
			break;
		default:
			watches[i].format = 1;
		}
		ScanAddress(addressStr, watches[i].address);
		watches[i].buf[0] = '\0';
		strncpy(watches[i].desc, Comment, sizeof(watches[i].desc)); watches[i].desc[sizeof(watches[i].desc)-1] = '\0';
	}
	
	fclose(WatchFile);
	return true;
}

bool Save_Watches_wch(const char* filename)
{
	FILE *WatchFile;// = fopen(filename,"r+b");
	//if (!WatchFile)
		WatchFile = fopen(filename,"w+b");
	fputc('\n',WatchFile);

	int WatchCount = 0;
	for(unsigned int i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
	{
		if(watches[i].on)
			WatchCount++;
	}
	sprintf(Str_Tmp,"%d\n",WatchCount);
	fputs(Str_Tmp,WatchFile);

	const char DELIM = '\t';
	int WatchIndex = 0;
	for(unsigned int i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
	{
		if(watches[i].on) {
			static char addressStr[16];
			char sizeChar;
			char typeChar;

			GetAddressStr(addressStr, watches[i].address);

			switch(watches[i].size)
			{
			case 1:
				sizeChar = 'b';
				break;
			case 2:
				sizeChar = 'w';
				break;
			default:
				sizeChar = 'd';
			}

			switch(watches[i].format)
			{
			case 3:
				typeChar = 'h';
				break;
			case 2:
				typeChar = 's';
				break;
			default:
				typeChar = 'u';
			}

			sprintf(Str_Tmp,"%05X%c%-6s%c%c%c%c%c%d%c%s\n",WatchIndex,DELIM,addressStr,DELIM,sizeChar,DELIM,typeChar,DELIM,0,DELIM,watches[i].desc);
			fputs(Str_Tmp,WatchFile);
			WatchIndex++;
		}
	}
	
	fclose(WatchFile);
	return true;
}

bool Load_Watches(const char* filename)
{
	FILE *file = fopen(filename, "r");
	if(file)
	{
		char c;

		fscanf(file, "%c%*s", &c);
		fclose(file);

		if (c == 'a')
			return Load_Watches_S9X(filename);
		else
			return Load_Watches_wch(filename);
	}
	else
		return false;
}

bool Save_Watches(const char* filename)
{
	return Save_Watches_wch(filename);
}

INT_PTR CALLBACK DlgRAMSearch(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	static S9xCheatDataSize bytes;
	static int val_type;
	static int use_entered;
	static S9xCheatComparisonType comp_type;
	switch(msg)
	{
	case WM_INITDIALOG:
		{
		if(DirectDraw.Clipped) S9xReRefresh();
			if(val_type==0)
				val_type=1;
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("Raptor.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			ListView_SetExtendedListViewStyle(GetDlgItem(hDlg, IDC_ADDYS), LVS_EX_FULLROWSELECT);
			
			//defaults
			SendDlgItemMessage(hDlg, IDC_LESS_THAN, BM_SETCHECK, BST_CHECKED, 0);
			if(!use_entered)
				SendDlgItemMessage(hDlg, IDC_PREV, BM_SETCHECK, BST_CHECKED, 0);
			else if(use_entered==1)
			{
				SendDlgItemMessage(hDlg, IDC_ENTERED, BM_SETCHECK, BST_CHECKED, 0);
				EnableWindow(GetDlgItem(hDlg, IDC_VALUE_ENTER), true);
				EnableWindow(GetDlgItem(hDlg, IDC_ENTER_LABEL), true);
			}
			else if(use_entered==2)
			{
				SendDlgItemMessage(hDlg, IDC_ENTEREDADDRESS, BM_SETCHECK, BST_CHECKED, 0);
				EnableWindow(GetDlgItem(hDlg, IDC_VALUE_ENTER), true);
				EnableWindow(GetDlgItem(hDlg, IDC_ENTER_LABEL), true);
			}
			SendDlgItemMessage(hDlg, IDC_UNSIGNED, BM_SETCHECK, BST_CHECKED, 0);
			SendDlgItemMessage(hDlg, IDC_1_BYTE, BM_SETCHECK, BST_CHECKED, 0);

			if(comp_type==S9X_GREATER_THAN)
			{
				SendDlgItemMessage(hDlg, IDC_LESS_THAN, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_GREATER_THAN, BM_SETCHECK, BST_CHECKED, 0);
			}
			else if(comp_type==S9X_LESS_THAN_OR_EQUAL)
			{
				SendDlgItemMessage(hDlg, IDC_LESS_THAN, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_LESS_THAN_EQUAL, BM_SETCHECK, BST_CHECKED, 0);
				
			}
			else if(comp_type==S9X_GREATER_THAN_OR_EQUAL)
			{
				SendDlgItemMessage(hDlg, IDC_LESS_THAN, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_GREATER_THAN_EQUAL, BM_SETCHECK, BST_CHECKED, 0);
				
			}
			else if(comp_type==S9X_EQUAL)
			{
				SendDlgItemMessage(hDlg, IDC_LESS_THAN, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_EQUAL, BM_SETCHECK, BST_CHECKED, 0);
				
			}
			else if(comp_type==S9X_NOT_EQUAL)
			{
				SendDlgItemMessage(hDlg, IDC_LESS_THAN, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_NOT_EQUAL, BM_SETCHECK, BST_CHECKED, 0);
				
			}
			
			if(val_type==2)
			{
				SendDlgItemMessage(hDlg, IDC_UNSIGNED, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_SIGNED, BM_SETCHECK, BST_CHECKED, 0);
				
			}
			else if(val_type==3)
			{
				SendDlgItemMessage(hDlg, IDC_UNSIGNED, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_HEX, BM_SETCHECK, BST_CHECKED, 0);
			}

			if(bytes==S9X_16_BITS)
			{
				SendDlgItemMessage(hDlg, IDC_1_BYTE, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_2_BYTE, BM_SETCHECK, BST_CHECKED, 0);
			}
			else if(bytes==S9X_24_BITS)
			{
				SendDlgItemMessage(hDlg, IDC_1_BYTE, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_3_BYTE, BM_SETCHECK, BST_CHECKED, 0);
			}
			else if(bytes==S9X_32_BITS)
			{
				SendDlgItemMessage(hDlg, IDC_1_BYTE, BM_SETCHECK, BST_UNCHECKED, 0);
				SendDlgItemMessage(hDlg, IDC_4_BYTE, BM_SETCHECK, BST_CHECKED, 0);
			}

			LVCOLUMN col;
			char temp[256];
			strcpy(temp,"Address");
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=0;
			col.cx=70;
			col.cchTextMax=7;
			col.pszText=temp;
			
			ListView_InsertColumn(GetDlgItem(hDlg,IDC_ADDYS),   0,   &col);
			
			strcpy(temp,"Curr. Value");
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH|LVCF_SUBITEM;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=1;
			col.cx=104;
			col.cchTextMax=3;
			col.pszText=temp;
			col.iSubItem=1;
			
			ListView_InsertColumn(GetDlgItem(hDlg,IDC_ADDYS),    1,   &col);

			strcpy(temp,"Prev. Value");
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH|LVCF_SUBITEM;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=2;
			col.cx=104;
			col.cchTextMax=32;
			col.pszText=temp;
			col.iSubItem=2;

			ListView_InsertColumn(GetDlgItem(hDlg,IDC_ADDYS),    2,   &col);
			
			{
					int l = CheatCount(bytes);
					ListView_SetItemCount (GetDlgItem(hDlg, IDC_ADDYS), l);
			}

			DragAcceptFiles(hDlg, TRUE);
			EnableWindow(GetDlgItem(hDlg, IDC_3_BYTE), FALSE);
		}
		return true;

		case WM_DESTROY:
			{
				oldRamSearchHWND = NULL;
				S9xSaveCheatFile (S9xGetFilename (".cht", CHEAT_DIR));
				DragAcceptFiles(hDlg, FALSE);
				break;
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
	case WM_DROPFILES:
	{
		HDROP hDrop = (HDROP)wParam;
		DragQueryFile(hDrop, 0, Str_Tmp, 1024);
		DragFinish(hDrop);
		return Load_Watches(Str_Tmp);
	}	break;
	case WM_NOTIFY:
		{
			static int selectionMarkOverride = -1;
			static int foundItemOverride = -1;
			if(wParam == IDC_ADDYS)
			{
				NMHDR * nmh=(NMHDR*)lParam;
				if(nmh->hwndFrom == GetDlgItem(hDlg, IDC_ADDYS) && nmh->code == LVN_GETDISPINFO)
				{
					static TCHAR buf[12]; // the following code assumes this variable is static
					int i, j;
					NMLVDISPINFO * nmlvdi=(NMLVDISPINFO*)lParam;
					j=nmlvdi->item.iItem;
					j++;
					for(i=0;i<(0x32000-bytes)&& j>0;i++)
					{
						if(TEST_BIT(Cheat.ALL_BITS, i))
							j--;
					}
					if (i>=0x32000 && j!=0)
					{
						return false;
					}
					i--;
					if(j=nmlvdi->item.iSubItem==0)
					{
						if(i < 0x20000)
							sprintf(buf, "%06X", i+0x7E0000);
						else if(i < 0x30000)
							sprintf(buf, "s%05X", i-0x20000);
						else
							sprintf(buf, "i%05X", i-0x30000);
						nmlvdi->item.pszText=buf;
						nmlvdi->item.cchTextMax=8;
					}
					if(j=nmlvdi->item.iSubItem==1)
					{
						int q=0, r=0;
						if(i < 0x20000)
							for(r=0;r<=bytes;r++)
								q+=(Cheat.RAM[i+r])<<(8*r);
						else if(i < 0x30000)
							for(r=0;r<=bytes;r++)
								q+=(Cheat.SRAM[(i-0x20000)+r])<<(8*r);
						else
							for(r=0;r<=bytes;r++)
								q+=(Cheat.FillRAM[(i-0x30000)+r])<<(8*r);
						//needs to account for size
						switch(val_type)
						{
						case 1:
							sprintf(buf, "%u", q);
							break;
						case 3:
							{
								switch(bytes)
								{
									default:
									case S9X_8_BITS:sprintf(buf, "%02X", q&0xFF);break;
									case S9X_16_BITS: sprintf(buf, "%04X", q&0xFFFF); break;
									case S9X_24_BITS: sprintf(buf, "%06X", q&0xFFFFFF);break;
									case S9X_32_BITS: sprintf(buf, "%08X", q);break;
								}
							}
							break;
						case 2:
							default:
								switch(bytes)
								{
									default:
									case S9X_8_BITS:  
										if((q-128)<0)
											sprintf(buf, "%d", q&0xFF);
										else sprintf(buf, "%d", q-256);
										break;
									case S9X_16_BITS:
										if((q-32768)<0)
											sprintf(buf, "%d", q&0xFFFF);
										else sprintf(buf, "%d", q-65536);
										break;
									case S9X_24_BITS:
										if((q-0x800000)<0)
											sprintf(buf, "%d", q&0xFFFFFF);
										else sprintf(buf, "%d", q-0x1000000);
										break;

									case S9X_32_BITS: sprintf(buf, "%d", q);break;
								}
								break;
						}
						nmlvdi->item.pszText=buf;
						nmlvdi->item.cchTextMax=4;
					}
					if(j=nmlvdi->item.iSubItem==2)
					{
						int q=0, r=0;
						if(i < 0x20000)
							for(r=0;r<=bytes;r++)
								q+=(Cheat.CWRAM[i+r])<<(8*r);
						else if(i < 0x30000)
							for(r=0;r<=bytes;r++)
								q+=(Cheat.CSRAM[(i-0x20000)+r])<<(8*r);
						else
							for(r=0;r<=bytes;r++)
								q+=(Cheat.CIRAM[(i-0x30000)+r])<<(8*r);
						//needs to account for size
						switch(val_type)
						{
						case 1:
							sprintf(buf, "%u", q);
							break;
						case 3:
							{
								switch(bytes)
								{
									default:
									case S9X_8_BITS:sprintf(buf, "%02X", q&0xFF);break;
									case S9X_16_BITS: sprintf(buf, "%04X", q&0xFFFF); break;
									case S9X_24_BITS: sprintf(buf, "%06X", q&0xFFFFFF);break;
									case S9X_32_BITS: sprintf(buf, "%08X", q);break;
								}
								break;
							}
						case 2:
							default:
								switch(bytes)
								{
									default:
									case S9X_8_BITS:  
										if((q-128)<0)
											sprintf(buf, "%d", q&0xFF);
										else sprintf(buf, "%d", q-256);
										break;
									case S9X_16_BITS:
										if((q-32768)<0)
											sprintf(buf, "%d", q&0xFFFF);
										else sprintf(buf, "%d", q-65536);
										break;
									case S9X_24_BITS:
										if((q-0x800000)<0)
											sprintf(buf, "%d", q&0xFFFFFF);
										else sprintf(buf, "%d", q-0x1000000);
										break;

									case S9X_32_BITS: sprintf(buf, "%d", q);break;
								}
								break;
						}
						nmlvdi->item.pszText=buf;
						nmlvdi->item.cchTextMax=4;
					}
					// nmlvdi->item.mask=LVIF_TEXT; // This is bad as wine relies on this to not change.
					
				}
				else if(nmh->hwndFrom == GetDlgItem(hDlg, IDC_ADDYS) && (nmh->code == (UINT)LVN_ITEMACTIVATE||nmh->code == (UINT)NM_CLICK))
				{
					bool enable=true;
					if(-1==ListView_GetSelectionMark(nmh->hwndFrom))
					{
						enable=false;
					}
					EnableWindow(GetDlgItem(hDlg, IDC_C_ADD), enable);
				}
				// allow typing in an address to jump to it
				else if(nmh->hwndFrom == GetDlgItem(hDlg, IDC_ADDYS) && nmh->code == (UINT)LVN_ODFINDITEM)
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
					if(startPos >= ListView_GetItemCount(GetDlgItem(hDlg,IDC_ADDYS)))
						startPos = 0;

					int currentPos, addrPos;
					for(addrPos=0,currentPos=0;addrPos<(0x32000-bytes)&&currentPos<startPos;addrPos++)
					{
						if(TEST_BIT(Cheat.ALL_BITS, addrPos))
							currentPos++;
					}

					pResult=currentPos;

					if (addrPos>=0x32000 && addrPos!=0)
						break;

					// ignore leading 0's
					while(searchstr[0] == '0' && searchstr[1] != '\0')
						searchstr++;

					int searchNum = 0;

					ScanAddress(searchstr, searchNum);


//					if (searchstr[0] != '7')
//						break; // all searchable addresses begin with a 7

					bool looped = false;

					// perform search
					do
					{

						if(addrPos == searchNum)
						{
							// select this item and stop search
							pResult = currentPos;
							break;
						}
						else if(addrPos > searchNum)
						{
							if(looped)
							{
								pResult = currentPos;
								break;
							}

							// optimization: the items are ordered alphabetically, so go back to the top since we know it can't be anything further down
							currentPos = 0;
							addrPos = 0;
							while(!TEST_BIT(Cheat.ALL_BITS, addrPos))
								addrPos++;
							looped = true;
							continue;
						}

						//Go to next item
						addrPos++;
						while(!TEST_BIT(Cheat.ALL_BITS, addrPos))
							addrPos++;
						currentPos++;

						//Need to restart at top?
						if(currentPos >= ListView_GetItemCount(GetDlgItem(hDlg,IDC_ADDYS)))
						{
							currentPos = 0;
							addrPos = 0;
							while(!TEST_BIT(Cheat.ALL_BITS, addrPos))
								addrPos++;
						}

					//Stop if back to start
					}while(currentPos != startPos);

					foundItemOverride = pResult;

					// in case previously-selected item is 0
					ListView_SetItemState (GetDlgItem(hDlg,IDC_ADDYS), 1, LVIS_SELECTED|LVIS_FOCUSED,LVIS_SELECTED|LVIS_FOCUSED);

					return pResult; // HACK: for some reason this selects the first item instead of what it's returning... current workaround is to manually re-select this return value upon the next changed event
				}
				else if(nmh->hwndFrom == GetDlgItem(hDlg, IDC_ADDYS) && nmh->code == LVN_ITEMCHANGED)
				{
					// hack - see note directly above
					LPNMLISTVIEW lpnmlv = (LPNMLISTVIEW)lParam;
					if(lpnmlv->uNewState & (LVIS_SELECTED|LVIS_FOCUSED))
					{
						if(foundItemOverride != -1 && lpnmlv->iItem == 0)
						{
							ListView_SetItemState (GetDlgItem(hDlg,IDC_ADDYS), foundItemOverride, LVIS_SELECTED|LVIS_FOCUSED,LVIS_SELECTED|LVIS_FOCUSED);
							ListView_EnsureVisible (GetDlgItem(hDlg,IDC_ADDYS), foundItemOverride, FALSE);
							selectionMarkOverride = foundItemOverride;
							foundItemOverride = -1;
						}
						else
						{
							selectionMarkOverride = lpnmlv->iItem;
						}
					}
				}
			}
		}
		break;
	case WM_ACTIVATE:
		ListView_RedrawItems(GetDlgItem(hDlg, IDC_ADDYS),0, 0x32000);
		break;
	case WM_COMMAND:
		{
			WORD wID = LOWORD(wParam);

			if (Settings.StopEmulation && (wID != IDOK && wID != IDCANCEL))
				break; // that's too unsafe!

			switch(wID)
			{
			case IDC_LESS_THAN:
			case IDC_GREATER_THAN:
			case IDC_LESS_THAN_EQUAL:
			case IDC_GREATER_THAN_EQUAL:
			case IDC_EQUAL:
			case IDC_NOT_EQUAL:
				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_LESS_THAN))
					comp_type=S9X_LESS_THAN;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_GREATER_THAN))
					comp_type=S9X_GREATER_THAN;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_LESS_THAN_EQUAL))
					comp_type=S9X_LESS_THAN_OR_EQUAL;	
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_GREATER_THAN_EQUAL))
					comp_type=S9X_GREATER_THAN_OR_EQUAL;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_EQUAL))
					comp_type=S9X_EQUAL;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_NOT_EQUAL))
					comp_type=S9X_NOT_EQUAL;

				break;
			case IDC_1_BYTE:
			case IDC_2_BYTE:
			case IDC_3_BYTE:
			case IDC_4_BYTE:
				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_1_BYTE))
					bytes=S9X_8_BITS;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_2_BYTE))
					bytes=S9X_16_BITS;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_3_BYTE))
					bytes=S9X_24_BITS;
				else bytes=S9X_32_BITS;
				{
					int l = CheatCount(bytes);
					ListView_SetItemCount (GetDlgItem(hDlg, IDC_ADDYS), l);
				}

				break;

			case IDC_SIGNED:
			case IDC_UNSIGNED:
			case IDC_HEX:
				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_UNSIGNED))
					val_type=1;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_SIGNED))
					val_type=2;
				else val_type=3;
				ListView_RedrawItems(GetDlgItem(hDlg, IDC_ADDYS),0, 0x32000);
				break;
			case IDC_C_ADD:
				{
					// account for size
					struct ICheat cht;
//					int idx=-1;
					LVITEM lvi;
					static TCHAR buf[12]; // the following code assumes this variable is static, I think
					ZeroMemory(&cht, sizeof(struct SCheat));

					//retrieve and convert to SCheat

					if(bytes==S9X_8_BITS)
						cht.size=1;
					else if (bytes==S9X_16_BITS)
						cht.size=2;
					else if (bytes==S9X_24_BITS)
						cht.size=3;
					else if (bytes==S9X_32_BITS)
						cht.size=4;


					ITEM_QUERY(lvi, IDC_ADDYS, 0, buf, 7);


					ScanAddress(buf, cht.address);
					// cheap hack, but should work better than before
					if (cht.address >= 0x800000) {
						cht.address = 0x700000 | (cht.address & 0xfffff);
					}

					memset(buf, 0, 7);
					if(val_type==1)
					{
						ITEM_QUERY(lvi, IDC_ADDYS, 1, buf, 12);
						sscanf(buf, "%u", &cht.new_val);
						memset(buf, 0, 7);
						ITEM_QUERY(lvi, IDC_ADDYS, 2, buf, 12);
						sscanf(buf, "%u", &cht.saved_val);
					}
					else if(val_type==3)
					{
						ITEM_QUERY(lvi, IDC_ADDYS, 1, buf, 12);
						sscanf(buf, "%x", &cht.new_val);
						memset(buf, 0, 7);
						ITEM_QUERY(lvi, IDC_ADDYS, 2, buf, 12);
						sscanf(buf, "%x", &cht.saved_val);
					}
					else
					{
						ITEM_QUERY(lvi, IDC_ADDYS, 1, buf, 12);
						sscanf(buf, "%d", &cht.new_val);
						memset(buf, 0, 7);
						ITEM_QUERY(lvi, IDC_ADDYS, 2, buf, 12);
						sscanf(buf, "%d", &cht.saved_val);
					}
					cht.format=val_type;
					//invoke dialog
					if(DialogBoxParam(g_hInst, MAKEINTRESOURCE(IDD_CHEAT_FROM_SEARCH), hDlg, DlgAddCheat, (LPARAM)&cht))
					{
						int p;
						for(p=0; p<cht.size; p++)
						{
							S9xAddCheat(TRUE, cht.saved, cht.address +p, ((cht.new_val>>(8*p))&0xFF));
							//add cheat
							strcpy(Cheat.c[Cheat.num_cheats-1].name, cht.name);
						}
					}
				}
				break;
			case IDC_C_RESET:
				S9xStartCheatSearch(&Cheat);
				{
					int l = CheatCount(bytes);
					ListView_SetItemCount (GetDlgItem(hDlg, IDC_ADDYS), l);
				}
				ListView_RedrawItems(GetDlgItem(hDlg, IDC_ADDYS),0, 0x32000);
				//val_type=1;
				//SendDlgItemMessage(hDlg, IDC_UNSIGNED, BM_SETCHECK, BST_CHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_SIGNED, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_HEX, BM_SETCHECK, BST_UNCHECKED, 0);

				//bytes=S9X_8_BITS;
				//SendDlgItemMessage(hDlg, IDC_1_BYTE, BM_SETCHECK, BST_CHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_2_BYTE, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_3_BYTE, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_4_BYTE, BM_SETCHECK, BST_UNCHECKED, 0);


				//use_entered=0;
				//SendDlgItemMessage(hDlg, IDC_PREV, BM_SETCHECK, BST_CHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_ENTERED, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_ENTEREDADDRESS, BM_SETCHECK, BST_UNCHECKED, 0);
				//EnableWindow(GetDlgItem(hDlg, IDC_VALUE_ENTER), false);
				//EnableWindow(GetDlgItem(hDlg, IDC_ENTER_LABEL), false);

				//comp_type=S9X_LESS_THAN;
				//SendDlgItemMessage(hDlg, IDC_LESS_THAN, BM_SETCHECK, BST_CHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_GREATER_THAN, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_LESS_THAN_EQUAL, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_GREATER_THAN_EQUAL, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_EQUAL, BM_SETCHECK, BST_UNCHECKED, 0);
				//SendDlgItemMessage(hDlg, IDC_NOT_EQUAL, BM_SETCHECK, BST_UNCHECKED, 0);
				return true;
			case IDC_C_WATCH:
				{
					uint32 address = (uint32)-1;
					char buf [12];
					LVITEM lvi;
					ITEM_QUERY(lvi, IDC_ADDYS, 0, buf, 7);
					if(lvi.iItem != -1)
					{
						int size;
						int format = val_type;
						bool alreadyExist = false;

						ScanAddress(buf, address);
						memset(buf, 0, 7);

						// account for size
						if(bytes==S9X_8_BITS)
							size=1;
						else if (bytes==S9X_16_BITS)
							size=2;
						else if (bytes==S9X_24_BITS)
							size=3;
						else if (bytes==S9X_32_BITS)
							size=4;

						unsigned int i;
						for(i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
						{
							if(!watches[i].on)
								break;
							if(watches[i].address == address && watches[i].size == size && watches[i].format == format) {
								alreadyExist = true;
								break;
							}
						}
						if(i >= sizeof(watches)/sizeof(*watches))
							i = (unsigned int)(sizeof(watches)/sizeof(*watches)-1);

						if (!alreadyExist)
						{
							watches[i].on = true;
							watches[i].size=size;
							watches[i].format=val_type;
							watches[i].address=address;
							strncpy(watches[i].buf,buf,12);
							if(address < 0x7E0000 + 0x20000)
								sprintf(watches[i].desc, "%6X", address);
							else if(address < 0x7E0000 + 0x30000)
								sprintf(watches[i].desc, "s%05X", address - 0x7E0000 - 0x20000);
							else
								sprintf(watches[i].desc, "i%05X", address - 0x7E0000 - 0x30000);
						}
					}
					{
						RECT rect;
						GetClientRect (GUI.hWnd, &rect);
						InvalidateRect (GUI.hWnd, &rect, true);
					}
				}
				break;
			case IDC_C_CLEARWATCH:
				{
					for(unsigned int i = 0 ; i < sizeof(watches)/sizeof(*watches) ; i++)
						watches[i].on = false;
					{
						RECT rect;
						GetClientRect (GUI.hWnd, &rect);
						InvalidateRect (GUI.hWnd, &rect, true);
					}
				}
				break;
			case IDC_C_LOADWATCH:
				{
					OPENFILENAME  ofn;
					char  szFileName[MAX_PATH];
					char  szPathName[MAX_PATH];

					strcpy(szFileName, S9xGetFilenameRel("wch"));

					_fullpath(szPathName, S9xGetDirectory(CHEAT_DIR), MAX_PATH);

					ZeroMemory( (LPVOID)&ofn, sizeof(OPENFILENAME) );
					ofn.lStructSize = sizeof(OPENFILENAME);
					ofn.hwndOwner = GUI.hWnd;
					ofn.lpstrFilter = "Watchlist (*.wch)" "\0*.wch\0" FILE_INFO_TXT_FILE_TYPE "\0*.txt\0" FILE_INFO_ANY_FILE_TYPE "\0*.*\0\0";
					ofn.lpstrFile = szFileName;
					ofn.lpstrDefExt = "wch";
					ofn.nMaxFile = MAX_PATH;
					ofn.Flags = OFN_HIDEREADONLY | OFN_FILEMUSTEXIST;
					ofn.lpstrInitialDir = szPathName;
					if(GetOpenFileName( &ofn ))
					{
						Load_Watches(szFileName);
					}
					{
						RECT rect;
						GetClientRect (GUI.hWnd, &rect);
						InvalidateRect (GUI.hWnd, &rect, true);
					}
				}
				break;
			case IDC_C_SAVEWATCH:
				{
					OPENFILENAME  ofn;
					char  szFileName[MAX_PATH];
					char  szPathName[MAX_PATH];

					strcpy(szFileName, S9xGetFilenameRel("wch"));

					_fullpath(szPathName, S9xGetDirectory(CHEAT_DIR), MAX_PATH);

					ZeroMemory( (LPVOID)&ofn, sizeof(OPENFILENAME) );
					ofn.lStructSize = sizeof(OPENFILENAME);
					ofn.hwndOwner = GUI.hWnd;
					ofn.lpstrFilter = "Watchlist (*.wch)" "\0*.wch\0" FILE_INFO_TXT_FILE_TYPE "\0*.txt\0" FILE_INFO_ANY_FILE_TYPE "\0*.*\0\0";
					ofn.lpstrFile = szFileName;
					ofn.lpstrDefExt = "wch";
					ofn.nMaxFile = MAX_PATH;
					ofn.Flags = OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT;
					ofn.lpstrInitialDir = szPathName;
					if(GetSaveFileName( &ofn ))
					{
						Save_Watches(szFileName);
					}
					{
						RECT rect;
						GetClientRect (GUI.hWnd, &rect);
						InvalidateRect (GUI.hWnd, &rect, true);
					}
				}
				break;

			case IDC_REFRESHLIST:
				ListView_RedrawItems(GetDlgItem(hDlg, IDC_ADDYS),0, 0x32000);
				break;

			case IDC_ENTERED:
			case IDC_ENTEREDADDRESS:
			case IDC_PREV:
				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_ENTERED))
				{
					use_entered=1;
					EnableWindow(GetDlgItem(hDlg, IDC_VALUE_ENTER), true);
					EnableWindow(GetDlgItem(hDlg, IDC_ENTER_LABEL), true);
				}
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_ENTEREDADDRESS))
				{
					use_entered=2;
					EnableWindow(GetDlgItem(hDlg, IDC_VALUE_ENTER), true);
					EnableWindow(GetDlgItem(hDlg, IDC_ENTER_LABEL), true);
				}
				else
				{
					use_entered=0;
					EnableWindow(GetDlgItem(hDlg, IDC_VALUE_ENTER), false);
					EnableWindow(GetDlgItem(hDlg, IDC_ENTER_LABEL), false);
				}
				return true;
				break;
			case IDC_C_SEARCH:
				{
				val_type=0;

				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_LESS_THAN))
					comp_type=S9X_LESS_THAN;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_GREATER_THAN))
					comp_type=S9X_GREATER_THAN;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_LESS_THAN_EQUAL))
					comp_type=S9X_LESS_THAN_OR_EQUAL;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_GREATER_THAN_EQUAL))
					comp_type=S9X_GREATER_THAN_OR_EQUAL;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_EQUAL))
					comp_type=S9X_EQUAL;
				else comp_type=S9X_NOT_EQUAL;

				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_UNSIGNED))
					val_type=1;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_SIGNED))
					val_type=2;
				else val_type=3;



				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_1_BYTE))
					bytes=S9X_8_BITS;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_2_BYTE))
					bytes=S9X_16_BITS;
				else if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_3_BYTE))
					bytes=S9X_24_BITS;
				else bytes=S9X_32_BITS;


				if(BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_ENTERED) ||
				   BST_CHECKED==IsDlgButtonChecked(hDlg, IDC_ENTEREDADDRESS))
				{
					TCHAR buf[20];
					GetDlgItemText(hDlg, IDC_VALUE_ENTER, buf, 20);
					uint32 value;
					int ret;
					if(use_entered==2)
					{
						ret = ScanAddress(buf, value);
						value -= 0x7E0000;
						S9xSearchForAddress (&Cheat, comp_type, bytes, value, FALSE);
					}
					else
					{
						if(val_type==1)
							ret=_stscanf(buf, "%ul", &value);
						else if (val_type==2)
							ret=_stscanf(buf, "%d", &value);
						else ret=_stscanf(buf, "%x", &value);


						if(ret!=1||!CheatTestRange(val_type, bytes, value))
						{
							MessageBox(hDlg, TEXT(SEARCH_ERR_INVALIDSEARCHVALUE), TEXT(SEARCH_TITLE_CHEATERROR), MB_OK);
							return true;
						}

						S9xSearchForValue (&Cheat, comp_type,
							bytes, value,
							(val_type==2), FALSE);
					}

				}
				else
				{
					S9xSearchForChange (&Cheat, comp_type,
                         bytes, (val_type==2), FALSE);
				}
				int l = CheatCount(bytes);
				ListView_SetItemCount (GetDlgItem(hDlg, IDC_ADDYS), l);
				}

				// if non-modal, update "Prev. Value" column after Search
				if(oldRamSearchHWND)
				{
					CopyMemory(Cheat.CWRAM, Cheat.RAM, 0x20000);
					CopyMemory(Cheat.CSRAM, Cheat.SRAM, 0x10000);
					CopyMemory(Cheat.CIRAM, Cheat.FillRAM, 0x2000);
				}


				ListView_RedrawItems(GetDlgItem(hDlg, IDC_ADDYS),0, 0x32000);
				return true;
				break;
			case IDOK:
				if (!Settings.StopEmulation) {
					CopyMemory(Cheat.CWRAM, Cheat.RAM, 0x20000);
					CopyMemory(Cheat.CSRAM, Cheat.SRAM, 0x10000);
					CopyMemory(Cheat.CIRAM, Cheat.FillRAM, 0x2000);
				}
				/* fall through */
			case IDCANCEL:
				if(oldRamSearchHWND)
					DestroyWindow(hDlg);
				else
					EndDialog(hDlg, 0);
				if(hBmp)
				{
					DeleteObject(hBmp);
					hBmp=NULL;
				};
				return true;
			default: break;
			}
		}
	default: return false;
	}
	return false;
}
