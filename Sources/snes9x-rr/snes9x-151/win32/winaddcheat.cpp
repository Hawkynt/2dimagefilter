
#include <windows.h>
#include <commctrl.h>

#include "../snes9x.h"
#include "../port.h"
#include "../display.h"
#include "../cheats.h"
#include "wsnes9x.h"
#include "wlanguage.h"
#include "CDirectDraw.h"
#include "Direct3D.h"
#include "rsrc/resource.h"
#include "oldramsearch.h"

extern SCheatData Cheat;
extern void S9xReRefresh();

//------------------------------------------------------------------------------

static struct ICheat* m_newCheat;
static HBITMAP m_hBmp = NULL;

//------------------------------------------------------------------------------

bool CheatTestRange(int val_type, S9xCheatDataSize bytes, uint32 value)
{
	if(val_type!=2)
	{
		if(bytes==S9X_8_BITS)
		{
			if(value<256)
				return true;
			else return false;
		}
		if(bytes==S9X_16_BITS)
		{
			if(value<65536)
				return true;
			else return false;
		}
		if(bytes==S9X_24_BITS)
		{
			if(value<16777216)
				return true;
			else return false;
		}
		//if it reads in, it's a valid 32-bit unsigned!
		return true;
	}
	else
	{
		if(bytes==S9X_8_BITS)
		{
			if((int32)value<128 && (int32)value >= -128)
				return true;
			else return false;
		}
		if(bytes==S9X_16_BITS)
		{
			if((int32)value<32768 && (int32)value >= -32768)
				return true;
			else return false;
		}
		if(bytes==S9X_24_BITS)
		{
			if((int32)value<8388608 && (int32)value >= -8388608)
				return true;
			else return false;
		}
		//should be handled by sscanf
		return true;
	}
}

//------------------------------------------------------------------------------

static void OnOK(HWND hDlg)
{
	BOOL ret = FALSE;
	TCHAR buf[23];
	int temp=m_newCheat->size;
	S9xCheatDataSize tmp = S9X_8_BITS;
	ZeroMemory(m_newCheat, sizeof(struct SCheat));
	m_newCheat->size=temp;
	GetDlgItemText(hDlg, IDC_NC_ADDRESS, buf, 7);
	ScanAddress(buf, m_newCheat->address);

	if(temp==1)
		tmp=S9X_8_BITS;
	if(temp==2)
		tmp=S9X_16_BITS;
	if(temp==3)
		tmp=S9X_24_BITS;
	if(temp==4)
		tmp=S9X_32_BITS;

	if(0!=GetDlgItemText(hDlg, IDC_NC_NEWVAL, buf, 12))
	{
		if(m_newCheat->format==2)
			ret = (sscanf(buf, "%d", &m_newCheat->new_val) == 1);
		else if(m_newCheat->format==1)
			ret = (sscanf(buf, "%u", &m_newCheat->new_val) == 1);
		else if(m_newCheat->format==3)
			ret = (sscanf(buf, "%x", &m_newCheat->new_val) == 1);

		if(!ret || !CheatTestRange(m_newCheat->format, tmp, m_newCheat->new_val))
		{
			MessageBox(hDlg, SEARCH_ERR_INVALIDNEWVALUE, SEARCH_TITLE_RANGEERROR, MB_OK);
			return;
		}

		if(0==GetDlgItemText(hDlg, IDC_NC_CURRVAL, buf, 12))
			m_newCheat->saved=FALSE;
		else
		{
			int i;
			if(m_newCheat->format==2)
				ret = (sscanf(buf, "%d", &i) == 1);
			else if(m_newCheat->format==1)
				ret = (sscanf(buf, "%u", &i) == 1);
			else if(m_newCheat->format==3)
				ret = (sscanf(buf, "%x", &i) == 1);

			if(!ret || !CheatTestRange(m_newCheat->format, tmp, i))
			{
				MessageBox(hDlg, SEARCH_ERR_INVALIDCURVALUE, SEARCH_TITLE_RANGEERROR, MB_OK);
				return;
			}

			m_newCheat->saved_val=i;
			m_newCheat->saved=TRUE;
		}
		GetDlgItemText(hDlg, IDC_NC_DESC, m_newCheat->name, 23);

		m_newCheat->enabled=TRUE;

		// don't add new cheat here, parent should do.
		//S9xAddCheat(m_newCheat->enabled,m_newCheat->saved_val,m_newCheat->address,m_newCheat->new_val);
		//strcpy(Cheat.c[Cheat.num_cheats-1].name,m_newCheat->name);

		ret = TRUE;
	}
	EndDialog(hDlg, ret);
}

static void OnCancel(HWND hDlg)
{
	EndDialog(hDlg, FALSE);
}

//------------------------------------------------------------------------------

static BOOL OnInitDialog(HWND hDlg, struct ICheat* cheatEntry)
{
	if(DirectDraw.Clipped) S9xReRefresh();
	{
		TCHAR buf [12];
		m_newCheat=cheatEntry;
		m_hBmp=(HBITMAP)LoadImage(NULL, TEXT("Gary.bmp"), IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
		sprintf(buf, "%06X", m_newCheat->address);
		SetDlgItemText(hDlg, IDC_NC_ADDRESS, buf);
		switch(m_newCheat->format)
		{
		default:
		case 1://UNSIGNED
			memset(buf,0,12);
			sprintf(buf, "%u", m_newCheat->new_val);
			SetDlgItemText(hDlg, IDC_NC_CURRVAL, buf);
			memset(buf,0,12);
			sprintf(buf, "%u", m_newCheat->saved_val);
			SetDlgItemText(hDlg, IDC_NC_PREVVAL, buf);
			SetWindowLong(GetDlgItem(hDlg, IDC_NC_NEWVAL), GWL_STYLE, ES_NUMBER |GetWindowLong(GetDlgItem(hDlg, IDC_NC_NEWVAL),GWL_STYLE));
			SetWindowLong(GetDlgItem(hDlg, IDC_NC_CURRVAL), GWL_STYLE, ES_NUMBER |GetWindowLong(GetDlgItem(hDlg, IDC_NC_CURRVAL),GWL_STYLE));
			SetWindowLong(GetDlgItem(hDlg, IDC_NC_PREVVAL), GWL_STYLE, ES_NUMBER |GetWindowLong(GetDlgItem(hDlg, IDC_NC_PREVVAL),GWL_STYLE));
			if(m_newCheat->size==1)
			{
				SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 3, 0);
				SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 3, 0);
				SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 3, 0);

			}
			if(m_newCheat->size==2)
			{
				SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 5, 0);
				SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 5, 0);
				SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 5, 0);

			}
			if(m_newCheat->size==3)
			{
				SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 8, 0);
				SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 8, 0);
				SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 8, 0);

			}
			if(m_newCheat->size==4)
			{
				SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 10, 0);
				SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 10, 0);
				SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 10, 0);

			}
			break;
		case 3:
			{
				char formatstring[10];
				sprintf(formatstring, "%%%02dX",m_newCheat->size*2);
				memset(buf,0,12);
				sprintf(buf, formatstring, m_newCheat->new_val);
				SetDlgItemText(hDlg, IDC_NC_CURRVAL, buf);
				memset(buf,0,12);
				sprintf(buf, formatstring, m_newCheat->saved_val);
				SetDlgItemText(hDlg, IDC_NC_PREVVAL, buf);
				SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, m_newCheat->size*2, 0);
				SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, m_newCheat->size*2, 0);
				SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, m_newCheat->size*2, 0);

			}
			break; //HEX
		case 2:
		memset(buf,0,12);
		sprintf(buf, "%d", m_newCheat->new_val);
		SetDlgItemText(hDlg, IDC_NC_CURRVAL, buf);
		memset(buf,0,12);
		sprintf(buf, "%d", m_newCheat->saved_val);
		SetDlgItemText(hDlg, IDC_NC_PREVVAL, buf);
		if(m_newCheat->size==1)
		{
			//-128
			SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 4, 0);
			SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 4, 0);
			SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 4, 0);
		}
		if(m_newCheat->size==2)
		{
			//-32768
			SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 6, 0);
			SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 6, 0);
			SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 6, 0);
		}
		if(m_newCheat->size==3)
		{
			//-8388608
			SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 8, 0);
			SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 8, 0);
			SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 8, 0);
		}
		if(m_newCheat->size==4)
		{
			//-2147483648
			SendDlgItemMessage(hDlg, IDC_NC_CURRVAL,EM_SETLIMITTEXT, 11, 0);
			SendDlgItemMessage(hDlg, IDC_NC_PREVVAL,EM_SETLIMITTEXT, 11, 0);
			SendDlgItemMessage(hDlg, IDC_NC_NEWVAL,EM_SETLIMITTEXT, 11, 0);
		}
		break;
		}
	}
	return true;
}

static void OnClose(HWND hDlg)
{
	PostMessage(hDlg, WM_COMMAND, IDCANCEL, 0);
}

static void OnDestroy(HWND hDlg)
{
	if(m_hBmp)
	{
		DeleteObject(m_hBmp);
		m_hBmp=NULL;
	}
}

static void OnPaint(HWND hDlg)
{
	PAINTSTRUCT ps;
	BeginPaint (hDlg, &ps);
	if(m_hBmp)
	{
		BITMAP bmp;
		ZeroMemory(&bmp, sizeof(BITMAP));
		RECT r;
		GetClientRect(hDlg, &r);
		HDC hdc=GetDC(hDlg);
		HDC hDCbmp=CreateCompatibleDC(hdc);
		GetObject(m_hBmp, sizeof(BITMAP), &bmp);
		HBITMAP hOldBmp=(HBITMAP)SelectObject(hDCbmp, m_hBmp);
		StretchBlt(hdc, 0,0,r.right,r.bottom,hDCbmp,0,0,bmp.bmWidth,bmp.bmHeight,SRCCOPY);
		SelectObject(hDCbmp, hOldBmp);
		DeleteDC(hDCbmp);
		ReleaseDC(hDlg, hdc);
	}
	EndPaint (hDlg, &ps);
}

//------------------------------------------------------------------------------

static BOOL OnCommand(HWND hDlg, WPARAM wParam, LPARAM lParam)
{
	WORD wID = LOWORD(wParam);
	switch(wID)
	{
	case IDOK:
		OnOK(hDlg);
		return TRUE;
	case IDCANCEL:
		OnCancel(hDlg);
		return TRUE;
	default:
		return FALSE;
	}
}

//------------------------------------------------------------------------------

INT_PTR CALLBACK DlgAddCheat(HWND hDlg, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch(msg)
	{
	case WM_INITDIALOG:
		return OnInitDialog(hDlg, (struct ICheat*) lParam);
	case WM_CLOSE:
		OnClose(hDlg);
		return TRUE;
	case WM_DESTROY:
		OnDestroy(hDlg);
		return TRUE;
	case WM_PAINT:
		OnPaint(hDlg);
		return TRUE;
	case WM_COMMAND:
		return OnCommand(hDlg, wParam, lParam);
	default:
		return FALSE;
	}
}
