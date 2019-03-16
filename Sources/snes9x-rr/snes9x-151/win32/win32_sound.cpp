#include <assert.h>

#include "IS9xSoundOutput.h"
#include "../port.h"
#include "../soundux.h"
#include "../snes9x.h"
#include "../memmap.h"
#include "../apu.h"
#include "wsnes9x.h"
#include "WAVOutput.h"
#include "AVIOutput.h"
#include "CDirectSound.h"
#include "CXAudio2.h"
// FMOD and FMOD Ex cannot be used at the same time
#ifdef FMOD_SUPPORT
#include "CFMOD.h"
#pragma comment(linker,"/DEFAULTLIB:fmodvc.lib")
#elif defined FMODEX_SUPPORT
#include "CFMODEx.h"
#pragma comment(linker,"/DEFAULTLIB:fmodexp_vc.lib")
#endif

// available sound output methods
CDirectSound S9xDirectSound;
CXAudio2 S9xXAudio2;
// FMOD and FMOD Ex cannot be used at the same time
#ifdef FMOD_SUPPORT
CFMOD S9xFMOD;
#elif defined FMODEX_SUPPORT
CFMODEx S9xFMODEx;	// FMOD Ex is currently unusable
#endif

// Interface used to access the sound output
IS9xSoundOutput *S9xSoundOutput;

// vars for S9XGenerateSound
int _samplecount;
uint8 *syncSoundBuffer;

// vars for sound recording (and movie recording)
uint8 *FrameSound = NULL;               // samples of last frame
size_t FrameSoundWritten = 0;           // sample count for AVIAddSoundSamples

// Wrapper function for S9xMixSamples that takes care of its upper limit
void S9xMixSamplesNoLimit(uint8 *buffer, int sample_count)
{
	int max_mixSampleCount = (Settings.SixteenBitSound ? SOUND_BUFFER_SIZE / 2 : SOUND_BUFFER_SIZE);
	while(sample_count > max_mixSampleCount) {
		S9xMixSamples(buffer,max_mixSampleCount);
		buffer += MAX_BUFFER_SIZE;
		sample_count-=max_mixSampleCount;
	}
	S9xMixSamples(buffer,sample_count);
}

// since so.mute_sound can be switched snes9x core,
// S9xSetSoundMute doesn't work properly (gocha: it's just a guess, though).
// this function is provided for manual sound mute processing.
bool IsSoundMuted() {
	if (Settings.Mute || Settings.StopEmulation || Settings.ForcedPause
			|| (Settings.Paused && (GUI.FAMute || (!Settings.FrameAdvance && GUI.IdleCount >= 8)))) // gives frame advance sound
		return true;
	//else if (so.mute_sound)
	//	return true;
	else
		return false;
}

// Wrapper function for S9xMixSamplesNoLimit that takes care of deterministic sample mix mode.
// This function doesn't call S9xMixSamples unless FlexibleSoundMixMode returns true.
void S9xMixSamplesNoLimitWrapped(uint8 *buffer, int sample_count)
{
	const int byte_count = so.sixteen_bit ? 2 : 1;

	if (sample_count == 0)
		return;
	else if (!FlexibleSoundMixMode())
	{
		if (so.sixteen_bit)
			SecureZeroMemory(buffer, sample_count * byte_count);
		else
			memset(buffer, 0x80, sample_count/* * byte_count*/);
	}
	else
		S9xMixSamplesNoLimit(buffer, sample_count);
}

// if snes9x requires deterministic sound mix
bool FlexibleSoundMixMode()
{
	if (GUI.AVIOut || GUI.WAVOut)
		return false;
	else
		return GUI.FlexibleSoundMixMaster;
}

/*  ReInitSound
reinitializes the sound core with current settings
IN:
mode		-	0 disables sound output, 1 enables
-----
returns true if successful, false otherwise
*/
bool ReInitSound(int mode)
{
	bool result = true;

	EnterCriticalSection(&GUI.SoundCritSect);

	S9xSoundOutput->DeInitSoundOutput();
	if(mode)
		result = S9xInitSound(mode,Settings.Stereo,0) != 0;
	else
		result = true;

	LeaveCriticalSection(&GUI.SoundCritSect);

	return result;
}

/*  SetupSound
applies current sound settings that do not require a reinit
these are currently only buffersize and playback rate changes
-----
returns true if successful, false otherwise
*/
bool SetupSound (void)
{
	bool result;

	EnterCriticalSection(&GUI.SoundCritSect);

	S9xSetPlaybackRate(Settings.SoundPlaybackRate);

	//if (FrameSound)
	//{
	//	delete [] FrameSound;
	//	FrameSound = NULL;
	//}
	if (!FrameSound)
	{
		FrameSound = new uint8 [48000 * 4 / 50];
		SecureZeroMemory(FrameSound, 48000 * 4 / 50);
	}
	FrameSoundWritten = 0;

	// we get the temp buffer for sound synchronization from the output object
	result = S9xSoundOutput->SetupSound(&syncSoundBuffer,&_samplecount);

	LeaveCriticalSection(&GUI.SoundCritSect);

	S9xSetSoundMute(TRUE); // whether SetupSound succeed
	return result;
}

/*  S9xOpenSoundDevice
called by S9xInitSound - initializes the currently selected sound output and
applies the current sound settings
IN:
mode		-	unused
pStereo		-	unused
BufferSize	-	unused
-----
returns true if successful, false otherwise
*/
bool8 S9xOpenSoundDevice (int mode, bool8 pStereo, int BufferSize)
{
	// point the interface to the correct output object
	switch(Settings.SoundDriver) {
		case WIN_SNES9X_DIRECT_SOUND_DRIVER:
			S9xSoundOutput = &S9xDirectSound;
			break;
#ifdef FMOD_SUPPORT
		case WIN_FMOD_DIRECT_SOUND_DRIVER:
		case WIN_FMOD_WAVE_SOUND_DRIVER:
		case WIN_FMOD_A3D_SOUND_DRIVER:
			S9xSoundOutput = &S9xFMOD;
			break;
#elif defined FMODEX_SUPPORT
		case WIN_FMODEX_DEFAULT_DRIVER:
		case WIN_FMODEX_ASIO_DRIVER:
			S9xSoundOutput = &S9xFMODEx;
			break;
#endif
			case WIN_XAUDIO2_SOUND_DRIVER:
			S9xSoundOutput = &S9xXAudio2;
			break;
		default:	// we default to DirectSound
			Settings.SoundDriver = WIN_SNES9X_DIRECT_SOUND_DRIVER;
			S9xSoundOutput = &S9xDirectSound;
	}
	if(!S9xSoundOutput->InitSoundOutput())
		return false;
	if(/*Settings.Mute || */!Settings.APUEnabled)
		return true;
	return SetupSound();
}

#define FIXED_POINT 0x10000
#define FIXED_POINT_SHIFT 16
#define FIXED_POINT_REMAINDER 0xffff

/*  S9xGenerateSound
called by the sound core if Settings.SoundSync is enabled - synchronizes access
to the temp buffer with a critical section
if the sound output does not offer sound sync then syncSoundBuffer will be NULL and
S9xGenerateSound returns immediately
*/
extern "C" void S9xGenerateSound(void)
{
	if (!S9xWinIsSoundActive() || !syncSoundBuffer)
		return;

	// to avoid redundant mixing
	if (so.samples_mixed_so_far >= _samplecount && FlexibleSoundMixMode())
		return;

	if (FlexibleSoundMixMode()) {
		if(!TryEnterCriticalSection(&GUI.SoundCritSect))
			return;
	}
	else
		EnterCriticalSection(&GUI.SoundCritSect);
	so.err_counter += so.err_rate;
	if (so.err_counter >= FIXED_POINT)
	{
		const int stereo_multiplier = so.stereo ? 2 : 1;
		const int byte_count = so.sixteen_bit ? 2 : 1;
		const int framerate = Memory.ROMFramesPerSecond;
//		const int frameskip = (Settings.SkipFrames == AUTO_FRAMERATE) ? 1 : (Settings.SkipFrames + 1);
//		const int sound_samples_per_update = (so.playback_rate * frameskip) / framerate;
		const int sound_samples_per_update = so.playback_rate / framerate;
		int sample_count = so.err_counter >> FIXED_POINT_SHIFT;
		int sample_count_for_syncSoundBuffer;
		int byte_offset_FrameSound;
		int byte_offset;

		so.err_counter &= FIXED_POINT_REMAINDER;

		// limit sample count per frame (just in case)
		if ((int)FrameSoundWritten + sample_count > sound_samples_per_update)
		{
			sample_count = sound_samples_per_update - FrameSoundWritten;
		}
		sample_count *= stereo_multiplier;

		if (sample_count == 0)
			goto finishGenerateSound;

		if (so.samples_mixed_so_far >= _samplecount)
			sample_count_for_syncSoundBuffer = 0;
		else if (so.samples_mixed_so_far + sample_count > _samplecount)
			sample_count_for_syncSoundBuffer = _samplecount - so.samples_mixed_so_far;
		else
			sample_count_for_syncSoundBuffer = sample_count;

		byte_offset = so.samples_mixed_so_far * byte_count;
		byte_offset_FrameSound = FrameSoundWritten * stereo_multiplier * byte_count;

		S9xMixSamplesNoLimit (&FrameSound[byte_offset_FrameSound], sample_count);
		FrameSoundWritten += sample_count / stereo_multiplier;
		if (syncSoundBuffer && sample_count_for_syncSoundBuffer)
		{
			memcpy(&syncSoundBuffer[byte_offset], &FrameSound[byte_offset_FrameSound], sample_count_for_syncSoundBuffer * byte_count);
			so.samples_mixed_so_far += sample_count_for_syncSoundBuffer;
		}
	}
finishGenerateSound:
	LeaveCriticalSection(&GUI.SoundCritSect);
}

extern "C" void S9xGenerateFrameSound(void)
{
	so.err_counter = 0;

	if (!S9xWinIsSoundActive() || FlexibleSoundMixMode())
	{
		FrameSoundWritten = 0;
		return;
	}

	// to avoid redundant mixing
	if (so.samples_mixed_so_far >= _samplecount && FlexibleSoundMixMode())
		return;

	if (FlexibleSoundMixMode()) {
		if(!TryEnterCriticalSection(&GUI.SoundCritSect))
			return;
	}
	else
		EnterCriticalSection(&GUI.SoundCritSect);

	if (Settings.SoundSync == 0)
		FrameSoundWritten = 0;

	const int stereo_multiplier = so.stereo ? 2 : 1;
	const int byte_count = so.sixteen_bit ? 2 : 1;
	const int framerate = Memory.ROMFramesPerSecond;
//	const int frameskip = (Settings.SkipFrames == AUTO_FRAMERATE) ? 1 : (Settings.SkipFrames + 1);
//	const int sound_samples_per_update = (so.playback_rate * frameskip) / framerate;
	const int sound_samples_per_update = so.playback_rate / framerate;
	const int byte_offset = FrameSoundWritten * byte_count * stereo_multiplier;
	const int byte_offset_FrameSound = FrameSoundWritten * stereo_multiplier * byte_count;
	int sample_count = sound_samples_per_update;
	int sample_count_for_syncSoundBuffer;

	if (Settings.SoundSync == 1)
	{
		// make sound length compatible with SoundSync == 0
		sample_count = sound_samples_per_update - FrameSoundWritten;
		if (sample_count < 0)
			sample_count = 0; // just in case

		so.err_counter = 0;
	}
	sample_count *= stereo_multiplier;

	if (so.samples_mixed_so_far >= _samplecount)
		sample_count_for_syncSoundBuffer = 0;
	else if (so.samples_mixed_so_far + sample_count > _samplecount)
		sample_count_for_syncSoundBuffer = _samplecount - so.samples_mixed_so_far;
	else
		sample_count_for_syncSoundBuffer = sample_count;

	S9xMixSamplesNoLimit (&FrameSound[byte_offset_FrameSound], sample_count);
	FrameSoundWritten += sample_count / stereo_multiplier;
	if (syncSoundBuffer && sample_count_for_syncSoundBuffer)
	{
		memcpy(&syncSoundBuffer[byte_offset], &FrameSound[byte_offset_FrameSound], sample_count_for_syncSoundBuffer * byte_count);
		so.samples_mixed_so_far += sample_count_for_syncSoundBuffer;
	}

//	if (Settings.Mute)
//		SecureZeroMemory(FrameSound, FrameSoundWritten * stereo_multiplier * byte_count);
	if (GUI.WAVOut)
		WAVAddSoundSamples(FrameSound, FrameSoundWritten, GUI.WAVOut);
	if (GUI.AVIOut)
		AVIAddSoundSamples(FrameSound, FrameSoundWritten, GUI.AVIOut);
//	if (!Settings.Mute)
		SecureZeroMemory(FrameSound, FrameSoundWritten * stereo_multiplier * byte_count);
	FrameSoundWritten = 0;

	LeaveCriticalSection(&GUI.SoundCritSect);
}

// initialize sound output
void S9xWinInitSound()
{
	S9xInitAPU();
	S9xInitSound (7, Settings.Stereo, 0);
}

// deinitialize sound output
void S9xWinDeinitSound()
{
	if (FrameSound)
	{
		delete [] FrameSound;
		FrameSound = NULL;
	}

	// stop sound playback
	ReInitSound(0);

	S9xDeinitAPU();
}

// return if the sound output is active
bool S9xWinIsSoundActive()
{
	// TODO: add code
	return true;
}
