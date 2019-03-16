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



// CDirectSound.cpp: implementation of the CDirectSound class.
//
//////////////////////////////////////////////////////////////////////

#include "wsnes9x.h"
#include "../snes9x.h"
#include "../soundux.h"
#include "CDirectSound.h"
#include <process.h>

extern void S9xMixSamplesNoLimitWrapped(uint8 *buffer, int sample_count);

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
CDirectSound::CDirectSound()
{
	lpDS = NULL;
	lpDSB = NULL;
	lpDSBPrimary = NULL;

	initDone = NULL;
	blockCount = 0;
	blockSize = 0;
	bufferSize = 0;
	sampleCount = 0;
	mixInterval = 0;
//	threadExit = false;
	syncSoundBuffer = NULL;
}

CDirectSound::~CDirectSound()
{
	DeInitDirectSound();
}

/*  CDirectSound::InitDirectSound
initializes the DirectSound object and sets the cooperation level
-----
returns true if successful, false otherwise
*/
bool CDirectSound::InitDirectSound ()
{
	HRESULT dErr;

	if(initDone)
		return true;

	if (!lpDS)
	{
		dErr = DirectSoundCreate (NULL, &lpDS, NULL);
		if (dErr != DS_OK)
		{
			MessageBox (GUI.hWnd, TEXT("\
Unable to initialise DirectSound. You will not be able to hear any\n\
sound effects or music while playing.\n\n\
It is usually caused by not having DirectX installed, another\n\
application that has already opened DirectSound in exclusive\n\
mode or the Windows WAVE device has been opened."),
						TEXT("Snes9X - Unable to Open DirectSound"),
						MB_OK | MB_ICONWARNING);
			return (false);
		}
	}
	initDone = true;
	dErr = lpDS->SetCooperativeLevel (GUI.hWnd, DSSCL_PRIORITY | DSSCL_EXCLUSIVE);
	if (!SUCCEEDED(dErr))
	{
		dErr = lpDS->SetCooperativeLevel (GUI.hWnd, DSSCL_PRIORITY);
		if (!SUCCEEDED(dErr))
		{
			if (!SUCCEEDED(lpDS -> SetCooperativeLevel (GUI.hWnd, DSSCL_NORMAL)))
			{
				lpDS -> Release();
				lpDS = NULL;
				initDone = false;
			}
			if (initDone)
				MessageBox (GUI.hWnd, TEXT("\
Unable to set DirectSound's  priority cooperative level.\n\
Another application is dicating the sound playback rate,\n\
sample size and mono/stereo setting."),
					TEXT("Snes9X - Unable to Set DirectSound priority"),
							MB_OK | MB_ICONWARNING);
			else
				MessageBox (GUI.hWnd, TEXT("\
Unable to set any DirectSound cooperative level. You will\n\
not be able to hear any sound effects or music while playing.\n\n\
It is usually caused by another application that has already\n\
opened DirectSound in exclusive mode."),
					TEXT("Snes9X - Unable to DirectSound"),
							MB_OK | MB_ICONWARNING);
		}
	}

	return (initDone);
}

/*  CDirectSound::DeInitDirectSound
releases all DirectSound objects and buffers
*/
void CDirectSound::DeInitDirectSound()
{
	initDone = false;

	DeInitSoundBuffer();
	
	if( lpDS != NULL)
	{
		lpDS->SetCooperativeLevel (GUI.hWnd, DSSCL_NORMAL);
		lpDS->Release ();
		lpDS = NULL;
	}
}

/*  CDirectSound::InitSoundBuffer
creates the DirectSound buffers and allocates the temp buffer for SoundSync
-----
returns true if successful, false otherwise
*/
bool CDirectSound::InitSoundBuffer()
{
	DSBUFFERDESC dsbd;
	WAVEFORMATEX wfx;
	HRESULT dErr;

	mixInterval = Settings.SoundMixInterval;

	if (Settings.SoundBufferSize < 1)
		Settings.SoundBufferSize = 1;
	if (Settings.SoundBufferSize > 64)
		Settings.SoundBufferSize = 64;
	blockCount = Settings.SoundBufferSize * 4;

	int sampleCountNoAlign = (Settings.SoundPlaybackRate * mixInterval * (Settings.Stereo ? 2 : 1)) / 1000;
	int sampleCountAligned = 64 / (Settings.SixteenBitSound ? 2 : 1);
	while (sampleCountAligned < sampleCountNoAlign)
		sampleCountAligned *= 2;

	sampleCount = sampleCountAligned;
	blockSize = sampleCount * (Settings.SixteenBitSound ? 2 : 1);
	bufferSize = blockSize * blockCount;

	wfx.wFormatTag = WAVE_FORMAT_PCM;
	wfx.nChannels = Settings.Stereo ? 2 : 1;
	wfx.nSamplesPerSec = Settings.SoundPlaybackRate;
	wfx.nBlockAlign = (Settings.SixteenBitSound ? 2 : 1) * (Settings.Stereo ? 2 : 1);
	wfx.wBitsPerSample = Settings.SixteenBitSound ? 16 : 8;
	wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
	wfx.cbSize = 0;

	ZeroMemory (&dsbd, sizeof(DSBUFFERDESC) );
	dsbd.dwSize = sizeof(dsbd);
	dsbd.dwFlags = DSBCAPS_PRIMARYBUFFER | DSBCAPS_STICKYFOCUS;

	dErr = lpDS->CreateSoundBuffer (&dsbd, &lpDSBPrimary, NULL);
	if (dErr != DS_OK)
	{
		lpDSB = NULL;
		return (false);
	}

	lpDSBPrimary->SetFormat (&wfx);
	if (lpDSBPrimary->GetFormat (&wfx, sizeof (wfx), NULL) == DS_OK)
	{
		so.playback_rate = wfx.nSamplesPerSec;
		so.stereo = wfx.nChannels > 1;
		so.sixteen_bit = wfx.wBitsPerSample == 16;
	}

	lpDSBPrimary->Play (0, 0, DSBPLAY_LOOPING);

	ZeroMemory (&dsbd, sizeof (dsbd));
	dsbd.dwSize = sizeof( dsbd);
	dsbd.dwFlags = DSBCAPS_CTRLFREQUENCY | DSBCAPS_CTRLVOLUME |
		DSBCAPS_GETCURRENTPOSITION2 | DSBCAPS_STICKYFOCUS | DSBCAPS_CTRLPOSITIONNOTIFY;
	dsbd.dwBufferBytes = bufferSize;
	dsbd.lpwfxFormat = &wfx;

	if (lpDS->CreateSoundBuffer (&dsbd, &lpDSB, NULL) != DS_OK)
	{
		lpDSBPrimary->Release ();
		lpDSBPrimary = NULL;
		lpDSB->Release();
		lpDSB = NULL;
		return (false);
	}

	syncSoundBuffer = new uint8[blockSize];
	return true;
}

/*  CDirectSound::DeInitSoundBuffer
deinitializes the DirectSound/temp buffers and stops the mixing thread
*/
void CDirectSound::DeInitSoundBuffer()
{
	if (timerHandle) {
		timeKillEvent(timerHandle);
		timerHandle = NULL;
	}
	//if(threadHandle) {
	//	threadExit = true;
	//	WaitForSingleObject(threadHandle,INFINITE);
	//	threadExit = false;
	//	CloseHandle(threadHandle);
	//	threadHandle = NULL;
	//}
	if(lpDSB != NULL)
	{
		lpDSB->Stop ();
		lpDSB->Release();
		lpDSB = NULL;
	}
	if(lpDSBPrimary != NULL)
	{
		lpDSBPrimary->Stop ();
		lpDSBPrimary->Release();
		lpDSBPrimary = NULL;
	}
	if(syncSoundBuffer) {
		delete syncSoundBuffer;
		syncSoundBuffer = NULL;
	}
}

/*  CDirectSound::SetupSound
applies sound setting changes by recreating the buffers and starting a new mixing thread
it fills the DirectSound before starting playback
IN/OUT:
syncSoundBuffer		-	will point to the temp buffer that can be used for SoundSync
sample_count		-	number of samples that fit into syncSoundBuffer
-----
returns true if successful, false otherwise
*/
bool CDirectSound::SetupSound(uint8 **syncSoundBuffer,int *sample_count)
{
	HRESULT hResult;

	*syncSoundBuffer = NULL;
	*sample_count = 0;

	if(!initDone)
		return false;
	
	DeInitSoundBuffer();
	InitSoundBuffer();
	
	BYTE  *B1;
	DWORD S1;
	hResult = lpDSB->Lock (0, 0, (void **)&B1, &S1, NULL, NULL, DSBLOCK_ENTIREBUFFER);
	if (hResult == DSERR_BUFFERLOST)
	{
		lpDSB->Restore ();
		hResult = lpDSB->Lock (0, 0, (void **)&B1, &S1, NULL, NULL, DSBLOCK_ENTIREBUFFER);
	}
	if (!SUCCEEDED(hResult))
	{
		hResult = lpDSB -> Unlock (B1, S1, NULL, NULL);
		return true;
	}

	S9xMixSamplesNoLimitWrapped(B1,sampleCount * blockCount);
	lpDSB->Unlock(B1,S1,NULL,NULL);

	lpDSB->Play (0, 0, DSBPLAY_LOOPING);

	last_block = blockCount - 1;

	// For some reasons, we use old good timeSetEvent method instead.
	timerHandle = timeSetEvent (mixInterval/2, 0, &SoundTimerCallback, (DWORD)this, TIME_PERIODIC);
	if (!timerHandle) {
		MessageBox (GUI.hWnd, TEXT("Unable to create mixing thread, DirectSound will not work."),
			TEXT("Snes9X - DirectSound"),
			MB_OK | MB_ICONWARNING);
		DeInitSoundBuffer();
		return false;
	}
	//if(!(threadHandle = (HANDLE)_beginthreadex(NULL,0,&SoundThread,(void *)this,0,NULL))) {
	//	MessageBox (GUI.hWnd, TEXT("Unable to create mixing thread, DirectSound will not work."),
	//		TEXT("Snes9X - DirectSound"),
	//		MB_OK | MB_ICONWARNING);
	//	DeInitSoundBuffer();
	//	return false;
	//}

	*syncSoundBuffer = this->syncSoundBuffer;
	*sample_count = sampleCount;

	return (true);
}

/*  CDirectSound::ProcessSound
the mixing function called by the mix thread
uses the current play position to decide if a new block can be filled with audio data
synchronizes the syncSoundBuffer access with a critical section
*/
void CDirectSound::ProcessSound()
{
	DWORD play_pos = 0, write_pos = 0;
	HRESULT hResult;
	DWORD curr_block;

	// we need to enter before GetCurrentPosition, otherwise the play cursor could already be too far
	// ahead when we get into the critical section
	EnterCriticalSection(&GUI.SoundCritSect);

	if(!lpDSB)
		goto finishDirectSoundWrite;

	lpDSB->GetCurrentPosition (&play_pos, &write_pos);

	curr_block = ((play_pos / blockSize) + Settings.SoundBufferSize) % blockCount;

	if (curr_block != last_block)
	{
		BYTE  *B1, *B2;
		DWORD S1, S2;

		write_pos = curr_block * blockSize;
		last_block = curr_block;

		hResult = lpDSB->Lock (write_pos, blockSize, (void **)&B1, &S1, (void **)&B2, &S2, 0);
		if (hResult == DSERR_BUFFERLOST)
		{
			lpDSB->Restore ();
			hResult = lpDSB->Lock (write_pos, blockSize, (void **)&B1, &S1, (void **)&B2, &S2, 0);
		}
		if (!SUCCEEDED(hResult))
		{
			hResult = lpDSB -> Unlock (B1, S1, B2, S2);
			goto finishDirectSoundWrite;
		}
		int byte_offset = 0;
		int sample_count = sampleCount;

		if (IsSoundMuted()) {
			if (so.sixteen_bit)
				SecureZeroMemory(syncSoundBuffer, sampleCount * 2);
			else
				memset(syncSoundBuffer, 0x80, sampleCount);
		}
		else if (so.samples_mixed_so_far < (int32) sampleCount)
		{
			byte_offset = (so.sixteen_bit ? (so.samples_mixed_so_far << 1) : so.samples_mixed_so_far);
			sample_count -= so.samples_mixed_so_far;
			S9xMixSamplesNoLimitWrapped(syncSoundBuffer + byte_offset,sample_count);
		}
		so.samples_mixed_so_far = 0;

		if (B1)
		{
			memcpy(B1,syncSoundBuffer,S1);
		}
		if (B2)
		{
			memcpy(B2,syncSoundBuffer+S1,S2);
		}

		hResult = lpDSB -> Unlock (B1, S1, B2, S2);
		if (!SUCCEEDED(hResult))
			goto finishDirectSoundWrite;
	}
finishDirectSoundWrite:
	LeaveCriticalSection(&GUI.SoundCritSect);
}

/*  CDirectSound::SoundThread
this is the actual mixing thread, runs until DeInitSoundBuffer sets threadExit
IN:
lpParameter		-	pointer to the CDirectSound object
*/
//unsigned int __stdcall CDirectSound::SoundThread (LPVOID lpParameter)
//{
//	CDirectSound *S9xDirectSound=(CDirectSound *)lpParameter;
//	while(!S9xDirectSound->threadExit) {
//		S9xDirectSound->ProcessSound();
//	}
//	_endthreadex(0);
//	return 0;
//}

/*  CDirectSound::SoundTimerCallback
this is the actual mixing thread, which works instead of SoundThread
*/
VOID CALLBACK CDirectSound::SoundTimerCallback(UINT idEvent, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1, DWORD_PTR dw2) {
	CDirectSound *S9xDirectSound = (CDirectSound *)dwUser;
	S9xDirectSound->ProcessSound();
}
