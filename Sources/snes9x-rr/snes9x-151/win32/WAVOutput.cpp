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



/*
  Win32 WAV Output module
  written by gocha in 2008, based on AVIOutput
*/

#pragma comment(lib, "winmm.lib")

#include <windows.h>
#include <mmsystem.h>
#include "WAVOutput.h"

struct WAVFile
{
	bool				valid;

	WAVEFORMATEX		wav_format_master;
	WAVEFORMATEX		wav_format;
	HMMIO				wav_file;

	MMCKINFO			waveChunk;
	MMCKINFO			fmtChunk;
	MMCKINFO			dataChunk;
};

static void clean_up(WAVFile* _wav)
{
	WAVFile& wav = *_wav;

	if(wav.wav_file)
	{
		mmioAscend(wav.wav_file, &wav.dataChunk, 0);
		mmioAscend(wav.wav_file, &wav.waveChunk, 0);
//		mmioFlush(wav.wav_file, 0);
		mmioClose(wav.wav_file, 0);
		wav.wav_file = NULL;
	}
}

#ifdef __cplusplus
extern "C" {
#endif

void WAVCreate(struct WAVFile** wav_out)
{
	*wav_out = new WAVFile;
	memset(*wav_out, 0, sizeof(WAVFile));
}

void WAVClose(struct WAVFile** wav_out)
{
	if(*wav_out)
	{
		clean_up(*wav_out);
		delete *wav_out;
	}
	*wav_out = NULL;
}

void WAVSetSoundFormat(const WAVEFORMATEX* format, struct WAVFile* wav_out)
{
	memcpy(&(wav_out->wav_format_master), format, sizeof(WAVEFORMATEX));
}

int WAVBegin(const char* _filename, struct WAVFile* _wav_out)
{
	WAVFile& wav = *_wav_out;
	LPSTR filename = NULL;
	int result = 0;

	do
	{
		filename = new char[strlen(_filename)+1];
		strcpy(filename, _filename);

		// open the file
		if(!(wav.wav_file = mmioOpen(filename, NULL, MMIO_CREATE|MMIO_WRITE)))
			break;

		delete filename;

		// create WAVE chunk
		wav.waveChunk.fccType = mmioFOURCC('W', 'A', 'V', 'E');
		mmioCreateChunk(wav.wav_file, &wav.waveChunk, MMIO_CREATERIFF);

		// create Format chunk
		wav.fmtChunk.ckid = mmioFOURCC('f', 'm', 't', ' ');
		mmioCreateChunk(wav.wav_file, &wav.fmtChunk, 0);
		// then write header
		memcpy(&wav.wav_format, &wav.wav_format_master, sizeof(WAVEFORMATEX));
		wav.wav_format.cbSize = 0;
		mmioWrite(wav.wav_file, (HPSTR) &wav.wav_format, sizeof(WAVEFORMATEX));
		mmioAscend(wav.wav_file, &wav.fmtChunk, 0);

		// create Data chunk
		wav.dataChunk.ckid = mmioFOURCC('d', 'a', 't', 'a');
		mmioCreateChunk(wav.wav_file, &wav.dataChunk, 0);

		// success
		result = 1;
		wav.valid = true;

	} while(false);

	if(!result)
	{
		clean_up(&wav);
		wav.valid = false;
	}

	return result;
}

int WAVGetSoundFormat(const struct WAVFile* wav_out, const WAVEFORMATEX** ppFormat)
{
	if(!wav_out->valid)
	{
		if(ppFormat)
		{
			*ppFormat = NULL;
		}
		return 0;
	}
	if(ppFormat)
	{
		*ppFormat = &wav_out->wav_format;
	}
	return 1;
}

void WAVAddSoundSamples(void* sound_data, const int num_samples, struct WAVFile* wav_out)
{
	if(!wav_out->valid)
	{
		return;
	}

	int data_length = num_samples * wav_out->wav_format.nBlockAlign;
	// assumes mmio system has been opened data chunk
	mmioWrite(wav_out->wav_file, (HPSTR) sound_data, data_length);
//	mmioFlush(wav.wav_file, 0);
}

#ifdef __cplusplus
}
#endif
