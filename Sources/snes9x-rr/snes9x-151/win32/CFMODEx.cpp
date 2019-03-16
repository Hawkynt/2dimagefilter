#ifdef FMODEX_SUPPORT
#ifndef FMOD_SUPPORT
#include "CFMODEx.h"
#include "wsnes9x.h"
#include "../snes9x.h"
#include "../soundux.h"

/* Note: FMOD Ex is currently unusable in SNES9X
		 There is too much latency, and I've been unable to get "normal" sound output.
		 Init also seems to force CoInitializeEx with COINIT_APARTMENTTHREADED, which conflicts
		 with XAudio2 */

extern void S9xMixSamplesNoLimitWrapped(uint8 *buffer, int sample_count);

CFMODEx::CFMODEx(void)
{
	initDone = false;
	fmodStream = NULL;
	
		fmodSystem = NULL;
}

CFMODEx::~CFMODEx(void)
{
	DeInitFMODEx();
	if(fmodSystem)
		fmodSystem->release();
}

bool CFMODEx::InitStream()
{
	unsigned int temp;// = (Settings.SoundPlaybackRate * Settings.SoundMixInterval * (Settings.SixteenBitSound ? 2 : 1) * (Settings.Stereo ? 2 : 1)) / 1000;
	FMOD_CREATESOUNDEXINFO createSoundExInfo={0};
	createSoundExInfo.cbsize = sizeof(FMOD_CREATESOUNDEXINFO);
	createSoundExInfo.defaultfrequency = Settings.SoundPlaybackRate;
	createSoundExInfo.numchannels = (Settings.Stereo?2:1);
	createSoundExInfo.format = (Settings.SixteenBitSound?FMOD_SOUND_FORMAT_PCM16:FMOD_SOUND_FORMAT_PCM8);
	createSoundExInfo.pcmreadcallback = FMODExStreamCallback;
	createSoundExInfo.suggestedsoundtype = FMOD_SOUND_TYPE_USER;
	fmodSystem->getDSPBufferSize(&temp,NULL);
	// 768 was the bufferSize in FMOD
	//temp = 768;
	temp *= (Settings.SixteenBitSound?2:1) * (Settings.Stereo?2:1);
	// the docs state that length does not have to be set for FMOD_OPENUSER, but if this is not set then
	// playSound will fail with FMOD_ERR_INVALID_PARAM
	createSoundExInfo.length = temp;
	// the docs state that decodebuffersize should influence the datalen passed to the callback function,
	// but this seems to be wrong
	createSoundExInfo.decodebuffersize = temp * 2;

	FMOD_RESULT fr = fmodSystem->createStream(NULL,FMOD_OPENUSER | FMOD_LOOP_NORMAL | FMOD_OPENRAW,&createSoundExInfo,&fmodStream);

	if(!(fr==FMOD_OK)) {
		return false;
	}

	fr = fmodSystem->playSound(FMOD_CHANNEL_FREE,fmodStream,0,NULL);
	
	return true;
}

void CFMODEx::DeInitStream()
{
	if (fmodStream)
    {
		fmodStream->release();
		fmodStream = NULL;
    }
}

bool CFMODEx::InitFMODEx()
{
	if(initDone)
		return true;

	FMOD_RESULT fr;

	if(!(FMOD::System_Create(&fmodSystem)==FMOD_OK))
		return false;

    switch (Settings.SoundDriver)
    {
        default:
        case WIN_FMODEX_DEFAULT_DRIVER:
			fr = fmodSystem->setOutput(FMOD_OUTPUTTYPE_AUTODETECT);
            break;
        case WIN_FMODEX_ASIO_DRIVER:
            fr = fmodSystem->setOutput(FMOD_OUTPUTTYPE_ASIO);
            break;
    }

    fr = fmodSystem->init(2,FMOD_INIT_NORMAL,0);

	if(fr!=FMOD_OK)
		return false;

	initDone = true;
	return true;
}

void CFMODEx::DeInitFMODEx()
{
	initDone = false;
	DeInitStream();
	if(fmodSystem) {
		fmodSystem->release();
		fmodSystem = NULL;
	}
}

bool CFMODEx::SetupSound(uint8 **syncSoundBuffer,int *sample_count)
{
	*syncSoundBuffer = NULL;
	*sample_count = 0;

	if(!initDone)
		return false;

	DeInitStream();
	return InitStream();
}

FMOD_RESULT F_CALLBACK CFMODEx::FMODExStreamCallback(
	  FMOD_SOUND *  sound, 
	  void *  data, 
	  unsigned int  datalen
	)
{
    int sample_count = datalen;

	sample_count >>= (Settings.SixteenBitSound?1:0);

	if (IsSoundMuted()) {
		if (so.sixteen_bit)
			SecureZeroMemory(buff, datalen);
		else
			memset(buff, 0x80, datalen);
	}
	else {
		S9xMixSamplesNoLimitWrapped ((unsigned char *) data, sample_count);
	}

    return FMOD_OK;
}
#endif
#endif