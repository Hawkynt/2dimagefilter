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

#ifdef FMOD_SUPPORT
#include "CFMOD.h"
#include "wsnes9x.h"
#include "../snes9x.h"
#include "../soundux.h"

extern void S9xMixSamplesNoLimitWrapped(uint8 *buffer, int sample_count);

/*  Construction/Destruction
*/
CFMOD::CFMOD(void)
{
	initDone = false;
	fmod_stream = NULL;
}

CFMOD::~CFMOD(void)
{
	DeInitFMOD();
}

/*  CFMOD::InitStream
initializes FMOD and the output stream that will call our callback function
-----
returns true if successful, false otherwise
*/
bool CFMOD::InitStream()
{
    if (!FSOUND_Init(Settings.SoundPlaybackRate, 16, 0))
    {
        MessageBox (GUI.hWnd, "\
Unable to initialise FMOD sound system. You will not be able to hear\n\
any sound effects or music while playing.\n\n\
It is usually caused by not having DirectX installed or another\n\
application that has already opened DirectSound in exclusive\n\
mode or the Windows WAVE device has been opened.",
                    "Snes9X - Unable to Open FMOD",
                    MB_OK | MB_ICONWARNING);
			DeInitStream();
			return false;
    }

	sampleCount = FSOUND_DSP_GetBufferLength (); // could be user controlled via "(Settings.SoundPlaybackRate * Settings.SoundMixInterval) / 1000;"
    if (Settings.Stereo)
		sampleCount *= 2;
	bufferSize = sampleCount * (Settings.SixteenBitSound?2:1);
	fmod_stream = FSOUND_Stream_Create (FMODStreamCallback, bufferSize,
                                        FSOUND_LOOP_OFF |
                                        FSOUND_STREAMABLE |
                                        FSOUND_LOOP_NORMAL |
                                        (Settings.SixteenBitSound ?
                                           (FSOUND_16BITS | FSOUND_SIGNED) :
                                           (FSOUND_8BITS  | FSOUND_SIGNED)) | // FIXME: FSOUND_UNSIGNED didn't work properly (3.7.5 final). For the time being, snes9x converts the mixed samples manually in FMODStreamCallback
                                        (Settings.Stereo ?
                                           FSOUND_STEREO : FSOUND_MONO),
                                        Settings.SoundPlaybackRate, (void *)this);

	if (!fmod_stream ||
            FSOUND_Stream_Play (FSOUND_FREE, fmod_stream) == -1)
	{
            MessageBox (GUI.hWnd, "\
Unable to create or play an FMOD sound stream. You will not be able\n\
to hear any sound effects or music while playing.",
                        "Snes9X - Unable to Open FMOD",
                        MB_OK | MB_ICONWARNING);
			DeInitStream();
			return false;
	}
	syncSoundBuffer = new uint8[bufferSize];
	return true;
}

/*  CFMOD::DeInitStream
stops playback and closes the stream
*/
void CFMOD::DeInitStream()
{
	if (fmod_stream)
    {
        FSOUND_StopSound (0);
        FSOUND_Stream_Stop (fmod_stream);
        FSOUND_Stream_Close (fmod_stream);
        FSOUND_Close ();
        fmod_stream = NULL;
    }
	if(syncSoundBuffer) {
		delete syncSoundBuffer;
		syncSoundBuffer = NULL;
	}
}

/*  CFMOD::InitFMOD
sets the sound driver to be used by FMOD
-----
returns true if successful, false otherwise
*/
bool CFMOD::InitFMOD()
{
	if(initDone)
		return true;

    switch (Settings.SoundDriver)
    {
        default:
        case WIN_FMOD_DIRECT_SOUND_DRIVER:
            FSOUND_SetOutput(FSOUND_OUTPUT_DSOUND);
            break;
        case WIN_FMOD_WAVE_SOUND_DRIVER:
            FSOUND_SetOutput(FSOUND_OUTPUT_WINMM);
            break;
        case WIN_FMOD_A3D_SOUND_DRIVER:
            FSOUND_SetOutput(FSOUND_OUTPUT_A3D);
            break;
    }

    FSOUND_SetDriver(0);

	initDone = true;
	return true;
}

void CFMOD::DeInitFMOD()
{
	DeInitStream();
}

/*  CFMOD::SetupSound
applies current sound settings by recreating the stream
IN/OUT:
syncSoundBuffer		-	will point to the temp buffer that can be used for SoundSync
sample_count		-	number of samples that fit into syncSoundBuffer
-----
returns true if successful, false otherwise
*/
bool CFMOD::SetupSound(uint8 **syncSoundBuffer,int *sample_count)
{
	*syncSoundBuffer = NULL;
	*sample_count = 0;

	DeInitStream();
	if(!InitStream()) {
		return false;
	}

	*syncSoundBuffer = this->syncSoundBuffer;
	*sample_count = sampleCount;
	return true;
}

/*  CFMOD::FMODStreamCallback
the callback that mixes into the stream
synchronizes the syncSoundBuffer access with a critical section (if SoundSync is enabled)
IN:
stream		-	the stream object, unused
buff		-	the buffer to mix into
len			-	number of bytes in the buffer
param		-	pointer to the CFMOD object
*/

// The FMOD API changed the return type of the stream callback function
// somewhere between version 3.20 and 3.33. The FMOD API defines a version
// string but you can't test for that at compile time. Instead, I've picked on
// a symbol that wasn't defined in version 3.20 to test for the change in API.
#if !defined (FSOUND_LOADRAW)
void
#else
signed char
#endif
F_CALLBACKAPI CFMOD::FMODStreamCallback (FSOUND_STREAM *stream, void *buff, int len, void *param)
{
	CFMOD *S9xFMOD=(CFMOD *)param;
    int sample_count = len;

    if (Settings.SixteenBitSound)
        sample_count /= 2;

	if (IsSoundMuted()) {
		if (so.sixteen_bit)
			SecureZeroMemory(buff, len);
		else
			memset(buff, 0x80, len);
	}
	else {
		EnterCriticalSection(&GUI.SoundCritSect);
		UINT32 mixed_bytes = 0;
		if(so.samples_mixed_so_far) {
			mixed_bytes = so.samples_mixed_so_far * (Settings.SixteenBitSound?2:1);
			memcpy(buff,S9xFMOD->syncSoundBuffer,mixed_bytes);
		}
		S9xMixSamplesNoLimitWrapped((unsigned char *)buff+mixed_bytes,sample_count-so.samples_mixed_so_far);
		so.samples_mixed_so_far = 0;
		LeaveCriticalSection(&GUI.SoundCritSect);

		// FIXME: use FSOUND_UNSIGNED instead, if that gets working properly.
		if (!Settings.SixteenBitSound) {
			for (int i = 0; i < len; i++)
				((uint8*)buff)[i] ^= 0x80;
		}
	}

#if defined (FSOUND_LOADRAW)
    return (1);
#endif
}

#endif