
#include <windows.h>
#include <commctrl.h>

#include "../snes9x.h"
#include "../port.h"
#include "../cheats.h"
#include "wsnes9x.h"
#include "wlanguage.h"
#include "CDirectDraw.h"
#include "rsrc/resource.h"

extern SCheatData Cheat;
extern void S9xReRefresh();

enum CheatStatus{
	Untouched,
	Deleted,
	Modified
};

typedef struct{
	int* index;
	DWORD* state;
} CheatTracker;

#define ITEM_QUERY(a, b, c, d, e)  ZeroMemory(&a, sizeof(LV_ITEM)); \
						a.iItem= ListView_GetSelectionMark(GetDlgItem(hDlg, b)); \
						a.iSubItem=c; \
						a.mask=LVIF_TEXT; \
						a.pszText=d; \
						a.cchTextMax=e; \
						ListView_GetItem(GetDlgItem(hDlg, b), &a);

INT_PTR CALLBACK DlgCheater(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	static HBITMAP hBmp;
	static bool internal_change;
	static bool has_sel;
	static int  sel_idx;
	static uint8 new_sel;
	static CheatTracker ct;
	switch(msg)
	{
	case WM_INITDIALOG:
			if(DirectDraw.Clipped) S9xReRefresh();
			
			hBmp=(HBITMAP)LoadImage(NULL, TEXT("funkyass.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
			ListView_SetExtendedListViewStyle(GetDlgItem(hDlg, IDC_CHEAT_LIST), LVS_EX_FULLROWSELECT|LVS_EX_CHECKBOXES);

			SendDlgItemMessage(hDlg, IDC_CHEAT_CODE, EM_LIMITTEXT, 14, 0);
			SendDlgItemMessage(hDlg, IDC_CHEAT_DESCRIPTION, EM_LIMITTEXT, 22, 0);
			SendDlgItemMessage(hDlg, IDC_CHEAT_ADDRESS, EM_LIMITTEXT, 6, 0);
			SendDlgItemMessage(hDlg, IDC_CHEAT_BYTE, EM_LIMITTEXT, 3, 0);

			LVCOLUMN col;
			char temp[32];
			strcpy(temp,SEARCH_COLUMN_ADDRESS);
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=0;
			col.cx=70;
			col.cchTextMax=7;
			col.pszText=temp;
			
			ListView_InsertColumn(GetDlgItem(hDlg,IDC_CHEAT_LIST),    0,   &col);
			
			strcpy(temp,SEARCH_COLUMN_VALUE);
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH|LVCF_SUBITEM;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=1;
			col.cx=43;
			col.cchTextMax=3;
			col.pszText=temp;
			col.iSubItem=1;
			
			ListView_InsertColumn(GetDlgItem(hDlg,IDC_CHEAT_LIST),    1,   &col);

			strcpy(temp,SEARCH_COLUMN_DESCRIPTION);
			ZeroMemory(&col, sizeof(LVCOLUMN));
			col.mask=LVCF_FMT|LVCF_ORDER|LVCF_TEXT|LVCF_WIDTH|LVCF_SUBITEM;
			col.fmt=LVCFMT_LEFT;
			col.iOrder=2;
			col.cx=165;
			col.cchTextMax=32;
			col.pszText=temp;
			col.iSubItem=2;

			ListView_InsertColumn(GetDlgItem(hDlg,IDC_CHEAT_LIST),    2,   &col);
			
			ct.index=new int[Cheat.num_cheats];
			ct.state=new DWORD[Cheat.num_cheats];

			uint32 counter;
			for(counter=0; counter<Cheat.num_cheats; counter++)
			{
				char buffer[7];
				int curr_idx=-1;
				sprintf(buffer, "%06X", Cheat.c[counter].address);
				LVITEM lvi;
				ZeroMemory(&lvi, sizeof(LVITEM));
				lvi.mask=LVIF_TEXT;
				lvi.pszText=buffer;
				lvi.cchTextMax=7;
				lvi.iItem=counter;
				curr_idx=ListView_InsertItem(GetDlgItem(hDlg,IDC_CHEAT_LIST), &lvi);

				unsigned int k;
				for(k=0;k<counter;k++)
				{
					if(ct.index[k]>=curr_idx)
						ct.index[k]++;
				}
				ct.index[counter]=curr_idx;
				ct.state[counter]=Untouched;

				sprintf(buffer, "%02X", Cheat.c[counter].byte);
				ZeroMemory(&lvi, sizeof(LVITEM));
				lvi.iItem=curr_idx;
				lvi.iSubItem=1;
				lvi.mask=LVIF_TEXT;
				lvi.pszText=buffer;
				lvi.cchTextMax=3;
				SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);

				ZeroMemory(&lvi, sizeof(LVITEM));
				lvi.iItem=curr_idx;
				lvi.iSubItem=2;
				lvi.mask=LVIF_TEXT;
				lvi.pszText=Cheat.c[counter].name;
				lvi.cchTextMax=23;
				SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);

				ListView_SetCheckState(GetDlgItem(hDlg,IDC_CHEAT_LIST), curr_idx, Cheat.c[counter].enabled);

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
	case WM_NOTIFY:
		{
			switch(LOWORD(wParam))
			{
			case IDC_CHEAT_LIST:
				if(0==ListView_GetSelectedCount(GetDlgItem(hDlg, IDC_CHEAT_LIST)))
				{
					EnableWindow(GetDlgItem(hDlg, IDC_DELETE_CHEAT), false);
					EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), false);
					has_sel=false;
					sel_idx=-1;
				}
				else
				{
					EnableWindow(GetDlgItem(hDlg, IDC_DELETE_CHEAT), true);
					if(!has_sel||sel_idx!=ListView_GetSelectionMark(GetDlgItem(hDlg, IDC_CHEAT_LIST)))
					{
						new_sel=3;
						//change
						char buf[25];
						LV_ITEM lvi;

						ITEM_QUERY (lvi, IDC_CHEAT_LIST, 0, buf, 7);

						SetDlgItemText(hDlg, IDC_CHEAT_ADDRESS, lvi.pszText);

						ITEM_QUERY (lvi, IDC_CHEAT_LIST, 1, &buf[strlen(buf)], 3);

						SetDlgItemText(hDlg, IDC_CHEAT_CODE, buf);
						char temp[4];
						int q;
						sscanf(lvi.pszText, "%02X", &q);
						sprintf(temp, "%d", q);
						SetDlgItemText(hDlg, IDC_CHEAT_BYTE, temp);
						
						ITEM_QUERY (lvi, IDC_CHEAT_LIST, 2, buf, 24);

						internal_change=true;
						SetDlgItemText(hDlg, IDC_CHEAT_DESCRIPTION, lvi.pszText);

					}
					sel_idx=ListView_GetSelectionMark(GetDlgItem(hDlg, IDC_CHEAT_LIST));
					has_sel=true;
				}

					return true;
			default: return false;
			}
		}
	case WM_COMMAND:
		{
			switch(LOWORD(wParam))
			{
			case IDC_CHEAT_DESCRIPTION:
				{
					switch(HIWORD(wParam))
					{
					case EN_CHANGE:
						
						if(internal_change)
						{
							internal_change=!internal_change;
							return false;
						}
						if(!has_sel)
							return true;
							EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), true);
							return true;
					}
					break;

				}
			case IDC_ADD_CHEAT:
				{
					char temp[24];
					GetDlgItemText(hDlg, IDC_CHEAT_CODE, temp, 23);
					if(strcmp(temp, ""))
					{
						int j;
						bool skip=false;
						int count=1;
						uint32 addy;
						uint8 byte[3];
						//test game genie
						if (NULL==S9xGameGenieToRaw (temp, addy, byte[0]))
							skip=true;
						//test goldfinger

					//	if(!skip

						//test PAR

						if(!skip)
						{
							if(NULL==S9xProActionReplayToRaw(temp, addy, byte[0]))
								skip=true;
						}

						if(!skip)
							return 0;

						for(j=0; j<count; j++)
						{
							char buffer[7];
							int curr_idx=-1;
							sprintf(buffer, "%06X", addy);
							LVITEM lvi;
							ZeroMemory(&lvi, sizeof(LVITEM));
							lvi.mask=LVIF_TEXT;
							lvi.pszText=buffer;
							lvi.cchTextMax=6;
							lvi.iItem=ListView_GetItemCount(GetDlgItem(hDlg,IDC_CHEAT_LIST));
							curr_idx=ListView_InsertItem(GetDlgItem(hDlg,IDC_CHEAT_LIST), &lvi);
							
							unsigned int k;
							for(k=0;k<Cheat.num_cheats;k++)
							{
								if(ct.index[k]>=curr_idx)
									ct.index[k]++;
							}


							sprintf(buffer, "%02X", byte[j]);
							ZeroMemory(&lvi, sizeof(LVITEM));
							lvi.iItem=curr_idx;
							lvi.iSubItem=1;
							lvi.mask=LVIF_TEXT;
							lvi.pszText=buffer;
							lvi.cchTextMax=2;
							SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
							
							GetDlgItemText(hDlg, IDC_CHEAT_DESCRIPTION, temp, 23);
							
							ZeroMemory(&lvi, sizeof(LVITEM));
							lvi.iItem=curr_idx;
							lvi.iSubItem=2;
							lvi.mask=LVIF_TEXT;
							lvi.pszText=temp;
							lvi.cchTextMax=23;
							SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
							
							addy++;
							
							
						}
					}
					else
					{
						uint8 byte;
						char buffer[7];
						char buffer2[7];

						GetDlgItemText(hDlg, IDC_CHEAT_ADDRESS, buffer, 7);
						GetDlgItemText(hDlg, IDC_CHEAT_BYTE, buffer2, 7);

						int curr_idx=-1;
						LVITEM lvi;
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.mask=LVIF_TEXT;
						lvi.pszText=buffer;
						lvi.cchTextMax=6;
						lvi.iItem=0;
						curr_idx=ListView_InsertItem(GetDlgItem(hDlg,IDC_CHEAT_LIST), &lvi);

						int scanres;
						if(buffer2[0]=='$')
							sscanf(buffer2,"$%2X", (unsigned int*)&scanres);
						else sscanf(buffer2,"%d", &scanres);
						byte = (uint8)(scanres & 0xff);

						sprintf(buffer2, "%02X", byte);
						
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.iItem=curr_idx;
						lvi.iSubItem=1;
						lvi.mask=LVIF_TEXT;
						lvi.pszText=buffer2;
						lvi.cchTextMax=2;
						SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
						
						GetDlgItemText(hDlg, IDC_CHEAT_DESCRIPTION, temp, 23);
						
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.iItem=curr_idx;
						lvi.iSubItem=2;
						lvi.mask=LVIF_TEXT;
						lvi.pszText=temp;
						lvi.cchTextMax=23;
						SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
					}
				}
				break;
			case IDC_UPDATE_CHEAT:
				{
									char temp[24];
					GetDlgItemText(hDlg, IDC_CHEAT_CODE, temp, 23);
					if(strcmp(temp, ""))
					{
						int j;
						bool skip=false;
						int count=1;
						uint32 addy;
						uint8 byte[3];
						//test game genie
						if (NULL==S9xGameGenieToRaw (temp, addy, byte[0]))
							skip=true;
						//test goldfinger

					//	if(!skip

						//test PAR

						if(!skip)
						{
							if(NULL==S9xProActionReplayToRaw(temp, addy, byte[0]))
								skip=true;
						}

						if(!skip)
							return 0;

						for(j=0;j<(int)Cheat.num_cheats;j++)
						{
							if(ct.index[j]==sel_idx)
								ct.state[j]=Modified;
						}

						for(j=0; j<count; j++)
						{
							char buffer[7];
//							int curr_idx=-1;
							sprintf(buffer, "%06X", addy);
							LVITEM lvi;
							ZeroMemory(&lvi, sizeof(LVITEM));
							lvi.mask=LVIF_TEXT;
							lvi.pszText=buffer;
							lvi.cchTextMax=6;
							lvi.iItem=sel_idx;
							ListView_SetItem(GetDlgItem(hDlg,IDC_CHEAT_LIST), &lvi);
							
							sprintf(buffer, "%02X", byte[j]);
							ZeroMemory(&lvi, sizeof(LVITEM));
							lvi.iItem=sel_idx;
							lvi.iSubItem=1;
							lvi.mask=LVIF_TEXT;
							lvi.pszText=buffer;
							lvi.cchTextMax=2;
							SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
							
							GetDlgItemText(hDlg, IDC_CHEAT_DESCRIPTION, temp, 23);
							
							ZeroMemory(&lvi, sizeof(LVITEM));
							lvi.iItem=sel_idx;
							lvi.iSubItem=2;
							lvi.mask=LVIF_TEXT;
							lvi.pszText=temp;
							lvi.cchTextMax=23;
							SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
							
							addy++;
							
							
						}
					}
					else
					{
												uint8 byte;
						char buffer[7];
						
						GetDlgItemText(hDlg, IDC_CHEAT_ADDRESS, buffer, 7);
						int j;
						for(j=0;j<(int)Cheat.num_cheats;j++)
						{
							if(ct.index[j]==sel_idx)
								ct.state[j]=Modified;
						}

//						int curr_idx=-1;
						LVITEM lvi;
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.mask=LVIF_TEXT;
						lvi.pszText=buffer;
						lvi.cchTextMax=6;
						lvi.iItem=sel_idx;
						ListView_SetItem(GetDlgItem(hDlg,IDC_CHEAT_LIST), &lvi);
						
						
						GetDlgItemText(hDlg, IDC_CHEAT_BYTE, buffer, 7);
						
						int scanres;
						if(buffer[0]=='$')
							sscanf(buffer,"$%2X", (unsigned int*)&scanres);
						else sscanf(buffer,"%d", &scanres);
						byte = (uint8)(scanres & 0xff);
						
						sprintf(buffer, "%02X", byte);
						
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.iItem=sel_idx;
						lvi.iSubItem=1;
						lvi.mask=LVIF_TEXT;
						lvi.pszText=buffer;
						lvi.cchTextMax=2;
						SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
						
						GetDlgItemText(hDlg, IDC_CHEAT_DESCRIPTION, temp, 23);
						
						ZeroMemory(&lvi, sizeof(LVITEM));
						lvi.iItem=sel_idx;
						lvi.iSubItem=2;
						lvi.mask=LVIF_TEXT;
						lvi.pszText=temp;
						lvi.cchTextMax=23;
						SendDlgItemMessage(hDlg,IDC_CHEAT_LIST, LVM_SETITEM, 0, (LPARAM)&lvi);
					}
				}
				
				break;
			case IDC_DELETE_CHEAT:
				{
					unsigned int j;
				for(j=0;j<Cheat.num_cheats;j++)
				{
					if(ct.index[j]==sel_idx)
						ct.state[j]=Deleted;
				}
				for(j=0;j<Cheat.num_cheats;j++)
				{
					if(ct.index[j]>sel_idx)
						ct.index[j]--;
				}
				}
				ListView_DeleteItem(GetDlgItem(hDlg, IDC_CHEAT_LIST), sel_idx);
				
				break;
			case IDC_CLEAR_CHEATS:
				internal_change = true;
				SetDlgItemText(hDlg,IDC_CHEAT_CODE,"");
				SetDlgItemText(hDlg,IDC_CHEAT_ADDRESS,"");
				SetDlgItemText(hDlg,IDC_CHEAT_BYTE,"");
				SetDlgItemText(hDlg,IDC_CHEAT_DESCRIPTION,"");
				ListView_SetItemState(GetDlgItem(hDlg, IDC_CHEAT_LIST),sel_idx, 0, LVIS_SELECTED|LVIS_FOCUSED);
				ListView_SetSelectionMark(GetDlgItem(hDlg, IDC_CHEAT_LIST), -1);
				sel_idx=-1;
				has_sel=false;
				break;
			case IDC_CHEAT_CODE:
				{
					uint32 j, k;
					long index;
					char buffer[15];
					char buffer2[15];
					POINT point;
					switch(HIWORD(wParam))
					{
					case EN_CHANGE:
						if(internal_change)
						{
							internal_change=false;
							return true;
						}
						SendMessage((HWND)lParam, WM_GETTEXT, 15,(LPARAM)buffer);
						GetCaretPos(&point);

						index = SendMessage((HWND)lParam,(UINT) EM_CHARFROMPOS, 0, (LPARAM) ((point.x&0x0000FFFF) | (((point.y&0x0000FFFF))<<16)));  

						k=0;
						for(j=0; j<strlen(buffer);j++)
						{
							if( (buffer[j]>='0' && buffer[j]<='9') || (buffer[j]>='A' && buffer[j]<='F') || buffer[j]=='-' || buffer[j]=='X')
							{
								buffer2[k]=buffer[j];
								k++;
							}
							else index --;
						}
						buffer2[k]='\0';
						
						if(has_sel&&!new_sel&&strlen(buffer2)!=0)
						{
							SetDlgItemText(hDlg, IDC_CHEAT_ADDRESS, "");
							SetDlgItemText(hDlg, IDC_CHEAT_BYTE, "");

						}

						if(new_sel!=0)
							new_sel--;

						internal_change=true;
						SendMessage((HWND)lParam, WM_SETTEXT, 0,(LPARAM)buffer2);
						SendMessage((HWND)lParam,  (UINT) EM_SETSEL, (WPARAM) (index), index);
						
						uint32 addy;
						uint8 val;
						bool8 sram;
						uint8 bytes[3];
						if(NULL==S9xGameGenieToRaw(buffer2, addy, val)||NULL==S9xProActionReplayToRaw(buffer2, addy, val)||NULL==S9xGoldFingerToRaw(buffer2, addy, sram, val, bytes))
						{
							if(has_sel)
								EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), true);
							else EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), false);
							EnableWindow(GetDlgItem(hDlg, IDC_ADD_CHEAT), true);
						}
						else
						{
							EnableWindow(GetDlgItem(hDlg, IDC_ADD_CHEAT), false);
							EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), false);
						}

					//	SetDlgItemText(hDlg, IDC_CHEAT_ADDRESS, "");
					//	SetDlgItemText(hDlg, IDC_CHEAT_BYTE, "");
						break;
					}
					break;
				}
			case IDC_CHEAT_ADDRESS:
				{
					uint32 j, k;
					long index;
					char buffer[7];
					char buffer2[7];
					POINT point;
					switch(HIWORD(wParam))
					{
					case EN_CHANGE:
						if(internal_change)
						{
							internal_change=false;
							return true;
						}
						SendMessage((HWND)lParam, WM_GETTEXT, 7,(LPARAM)buffer);
						GetCaretPos(&point);

						index = SendMessage((HWND)lParam,(UINT) EM_CHARFROMPOS, 0, (LPARAM) ((point.x&0x0000FFFF) | (((point.y&0x0000FFFF))<<16)));  

						k=0;
						for(j=0; j<strlen(buffer);j++)
						{
							if( (buffer[j]>='0' && buffer[j]<='9') || (buffer[j]>='A' && buffer[j]<='F'))
							{
								buffer2[k]=buffer[j];
								k++;
							}
							else index --;
						}
						buffer2[k]='\0';

					
						internal_change=true;
						SendMessage((HWND)lParam, WM_SETTEXT, 0,(LPARAM)buffer2);
						SendMessage((HWND)lParam,  (UINT) EM_SETSEL, (WPARAM) (index), index);

						SendMessage(GetDlgItem(hDlg, IDC_CHEAT_BYTE), WM_GETTEXT, 4,(LPARAM)buffer);
						
						if(has_sel&&!new_sel&&0!=strlen(buffer2))
							SetDlgItemText(hDlg, IDC_CHEAT_CODE, "");

						if(new_sel!=0)
							new_sel--;

						if(strlen(buffer2)!=0 && strlen(buffer) !=0)
						{
							if(has_sel)
								EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), true);
							else EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), false);
							EnableWindow(GetDlgItem(hDlg, IDC_ADD_CHEAT), true);
						}
						else
						{
							EnableWindow(GetDlgItem(hDlg, IDC_ADD_CHEAT), false);
							EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), false);
						}

						break;
					}
					break;
				}
				case IDC_CHEAT_BYTE:
				{
					uint32 j, k;
					long index;
					char buffer[4];
					char buffer2[4];
					POINT point;
					switch(HIWORD(wParam))
					{
					case EN_CHANGE:
						if(internal_change)
						{
							internal_change=false;
							return true;
						}
						SendMessage((HWND)lParam, WM_GETTEXT, 4,(LPARAM)buffer);
						GetCaretPos(&point);

						index = SendMessage((HWND)lParam,(UINT) EM_CHARFROMPOS, 0, (LPARAM) ((point.x&0x0000FFFF) | (((point.y&0x0000FFFF))<<16)));  

						k=0;
						for(j=0; j<strlen(buffer);j++)
						{
							if( (buffer[j]>='0' && buffer[j]<='9') || (buffer[j]>='A' && buffer[j]<='F') || buffer[j]=='$')
							{
								buffer2[k]=buffer[j];
								k++;
							}
							else index --;
						}
						buffer2[k]='\0';
						
						if(has_sel&&!new_sel&&0!=strlen(buffer2))
							SetDlgItemText(hDlg, IDC_CHEAT_CODE, "");
						
						if(new_sel!=0)
							new_sel--;

						internal_change=true;
						SendMessage((HWND)lParam, WM_SETTEXT, 0,(LPARAM)buffer2);
						SendMessage((HWND)lParam,  (UINT) EM_SETSEL, (WPARAM) (index), index);
						
						SendMessage(GetDlgItem(hDlg, IDC_CHEAT_ADDRESS), WM_GETTEXT, 7,(LPARAM)buffer);
						if(strlen(buffer2)!=0 && strlen(buffer) !=0)
						{
							if(has_sel)
								EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), true);
							else EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), false);
							EnableWindow(GetDlgItem(hDlg, IDC_ADD_CHEAT), true);
						}
						else
						{
							EnableWindow(GetDlgItem(hDlg, IDC_ADD_CHEAT), false);
							EnableWindow(GetDlgItem(hDlg, IDC_UPDATE_CHEAT), false);
						}
						//SetDlgItemText(hDlg, IDC_CHEAT_CODE, "");
						break;
					}
					break;
				}
				case IDOK:
					{
						int k,l;
						bool hit;
						unsigned int scanned;
						for(k=0;k<ListView_GetItemCount(GetDlgItem(hDlg, IDC_CHEAT_LIST)); k++)
						{
							hit=false;
							for(l=0;l<(int)Cheat.num_cheats;l++)
							{
								if(ct.index[l]==k)
								{
									hit=true;
									Cheat.c[l].enabled=ListView_GetCheckState(GetDlgItem(hDlg, IDC_CHEAT_LIST),l);
									if(ct.state[l]==Untouched)
										l=Cheat.num_cheats;
									else if(ct.state[l]==(unsigned long)Modified)
									{
										if(Cheat.c[l].enabled)
											S9xDisableCheat(l);
										//update me!
										//get these!
										
										char buf[25];
										LV_ITEM lvi;
										ZeroMemory(&lvi, sizeof(LV_ITEM));
										lvi.iItem= k;
										lvi.mask=LVIF_TEXT;
										lvi.pszText=buf;
										lvi.cchTextMax=7;
										
										ListView_GetItem(GetDlgItem(hDlg, IDC_CHEAT_LIST), &lvi);
										
										ScanAddress(lvi.pszText, scanned);
										Cheat.c[l].address = scanned;
										
										ZeroMemory(&lvi, sizeof(LV_ITEM));
										lvi.iItem= k;
										lvi.iSubItem=1;
										lvi.mask=LVIF_TEXT;
										lvi.pszText=buf;
										lvi.cchTextMax=3;
										
										ListView_GetItem(GetDlgItem(hDlg, IDC_CHEAT_LIST), &lvi);
										
										sscanf(lvi.pszText, "%02X", &scanned);
										Cheat.c[l].byte = (uint8)(scanned & 0xff);
										
										ZeroMemory(&lvi, sizeof(LV_ITEM));
										lvi.iItem= k;
										lvi.iSubItem=2;
										lvi.mask=LVIF_TEXT;
										lvi.pszText=buf;
										lvi.cchTextMax=24;
										
										ListView_GetItem(GetDlgItem(hDlg, IDC_CHEAT_LIST), &lvi);
										
										strcpy(Cheat.c[l].name,lvi.pszText);
										
										Cheat.c[l].enabled=ListView_GetCheckState(GetDlgItem(hDlg, IDC_CHEAT_LIST),k);
										
										if(Cheat.c[l].enabled)
											S9xEnableCheat(l);
									}
									
								}
							}
							if(!hit)
							{
								uint32 address;
								uint8 byte;
								bool8 enabled;
								char buf[25];
								LV_ITEM lvi;
								ZeroMemory(&lvi, sizeof(LV_ITEM));
								lvi.iItem= k;
								lvi.mask=LVIF_TEXT;
								lvi.pszText=buf;
								lvi.cchTextMax=7;
								
								ListView_GetItem(GetDlgItem(hDlg, IDC_CHEAT_LIST), &lvi);
								
								ScanAddress(lvi.pszText, scanned);
								address = scanned;
								
								ZeroMemory(&lvi, sizeof(LV_ITEM));
								lvi.iItem= k;
								lvi.iSubItem=1;
								lvi.mask=LVIF_TEXT;
								lvi.pszText=buf;
								lvi.cchTextMax=3;
								
								ListView_GetItem(GetDlgItem(hDlg, IDC_CHEAT_LIST), &lvi);
								
								sscanf(lvi.pszText, "%02X", &scanned);
								byte = (uint8)(scanned & 0xff);
								
								enabled=ListView_GetCheckState(GetDlgItem(hDlg, IDC_CHEAT_LIST),k);

								S9xAddCheat(enabled,true,address,byte);

								ZeroMemory(&lvi, sizeof(LV_ITEM));
								lvi.iItem= k;
								lvi.iSubItem=2;
								lvi.mask=LVIF_TEXT;
								lvi.pszText=buf;
								lvi.cchTextMax=24;
								
								ListView_GetItem(GetDlgItem(hDlg, IDC_CHEAT_LIST), &lvi);
								
								strcpy(Cheat.c[Cheat.num_cheats-1].name, lvi.pszText);
								
								
							}
						}

						for(l=(int)Cheat.num_cheats;l>=0;l--)
						{
							if(ct.state[l]==Deleted)
							{
								S9xDeleteCheat(l);
							}
						}
					}
				case IDCANCEL:
					delete [] ct.index;
					delete [] ct.state;
					EndDialog(hDlg, 0);
					if(hBmp)
					{
						DeleteObject(hBmp);
						hBmp=NULL;
					}
					return true;
				default:return false;
					}
		}
	default: return false;
	}
}
