/* CXAudio2.cpp - written by OV2 */

#include "CXaudio2.h"
#include "../soundux.h"
#include "../snes9x.h"
#include "wsnes9x.h"
#include <process.h>
#include <Dxerr.h>

/* CXAudio2
	Implements audio output through XAudio2.
	Basic idea: one master voice and one source voice are created;
	the source voice consumes buffers queued by PushBuffer, plays them through
	the master voice and calls OnBufferEnd after each buffer.
	A mixing thread runs in a loop and waits for signals from OnBufferEnd to fill
	a new buffer and push it to the source voice.
*/

extern void S9xMixSamplesNoLimitWrapped(uint8 *buffer, int sample_count);

/*  Construction/Destruction
*/
CXAudio2::CXAudio2(void)
{
	eventHandle = NULL;
	pXAudio2 = NULL;
	pSourceVoice = NULL;
	pMasterVoice = NULL;

	mixInterval = bufferCount = bufferSize = buffer_sampleCount \
		= sum_bufferSize = 0;
	SoundBuffer = NULL;
	syncSoundBuffer = NULL;
	emptyBuffers = 0;
	currentBufferPos = 0;
	exitThread = false;
	initDone = false;
}

CXAudio2::~CXAudio2(void)
{
	DeInitXAudio2();
}

/*  CXAudio2::InitXAudio2
initializes the XAudio2 object and starts the mixing thread in a waiting state
-----
returns true if successful, false otherwise
*/
bool CXAudio2::InitXAudio2(void)
{
	if(initDone)
		return true;

	HRESULT hr;
	if ( FAILED(hr = XAudio2Create( &pXAudio2, 0, XAUDIO2_DEFAULT_PROCESSOR ) ) ) {
		DXTRACE_ERR_MSGBOX(TEXT("\
Unable to initialise XAudio2. You will not be able to hear any\n\
sound effects or music while playing.\n\n\
It is usually caused by not having a recent DirectX release installed."),
                        hr);
		return false;
	}

	if(!(eventHandle = CreateEvent(NULL,0,0,NULL))) {
		MessageBox (GUI.hWnd, TEXT("Unable to create synchronization event, XAudio2 will not work."),
                        TEXT("Snes9X - XAudio2"),
                        MB_OK | MB_ICONWARNING);
		DeInitXAudio2();
		return false;
	}

	if(!(threadHandle = (HANDLE)_beginthreadex(NULL,0,&SoundThread,(void *)this,0,NULL))) {
		MessageBox (GUI.hWnd, TEXT("Unable to create mixing thread, XAudio2 will not work."),
                        TEXT("Snes9X - XAudio2"),
                        MB_OK | MB_ICONWARNING);
		DeInitXAudio2();
		return false;
	}

	initDone = true;
	return true;
}

/*  CXAudio2::InitVoices
initializes the voice objects with the current audio settings
-----
returns true if successful, false otherwise
*/
bool CXAudio2::InitVoices(void)
{
	HRESULT hr;
	if ( FAILED(hr = pXAudio2->CreateMasteringVoice( &pMasterVoice, (Settings.Stereo?2:1),
		Settings.SoundPlaybackRate, 0, 0 , NULL ) ) ) {
			DXTRACE_ERR_MSGBOX(TEXT("Unable to create mastering voice."),hr);
			return false;
	}

	WAVEFORMATEX wfx;
	wfx.wFormatTag = WAVE_FORMAT_PCM;
    wfx.nChannels = Settings.Stereo ? 2 : 1;
    wfx.nSamplesPerSec = Settings.SoundPlaybackRate;
    wfx.nBlockAlign = (Settings.SixteenBitSound ? 2 : 1) * (Settings.Stereo ? 2 : 1);
    wfx.wBitsPerSample = Settings.SixteenBitSound ? 16 : 8;
    wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
    wfx.cbSize = 0;

	if( FAILED(hr = pXAudio2->CreateSourceVoice(&pSourceVoice, (WAVEFORMATEX*)&wfx,
		XAUDIO2_VOICE_NOSRC , XAUDIO2_DEFAULT_FREQ_RATIO, this, NULL, NULL ) ) ) {
			DXTRACE_ERR_MSGBOX(TEXT("Unable to create source voice."),hr);
			return false;
	}

	return true;
}

/*  CXAudio2::DeInitXAudio2
deinitializes all objects and stops the mixing thread
*/
void CXAudio2::DeInitXAudio2(void)
{
	initDone = false;
	DeInitVoices();	
	if(threadHandle) {
		exitThread = true;
		SetEvent(eventHandle);							// signal the thread in case it is waiting
		WaitForSingleObject(threadHandle,INFINITE);		// wait for the thread to exit gracefully
		exitThread = false;
		CloseHandle(threadHandle);
		threadHandle = NULL;
	}
	if(eventHandle) {
		CloseHandle(eventHandle);
		eventHandle = NULL;
	}
	if(pXAudio2) {
		pXAudio2->Release();
		pXAudio2 = NULL;
	}
}

/*  CXAudio2::DeInitVoices
deinitializes the voice objects and buffers and sets the mixing thread in wait state
*/
void CXAudio2::DeInitVoices(void)
{
	if(pSourceVoice) {
		StopPlayback();
		while(emptyBuffers);				// wait until sound thread is in safe state
		pSourceVoice->DestroyVoice();
		pSourceVoice = NULL;
	}
	if(pMasterVoice) {
		pMasterVoice->DestroyVoice();
		pMasterVoice = NULL;
	}
	if(SoundBuffer) {
		delete SoundBuffer;
		SoundBuffer = NULL;
	}
	if(syncSoundBuffer) {
		delete syncSoundBuffer;
		syncSoundBuffer = NULL;
	}
	currentBufferPos = 0;
	emptyBuffers = 0;
}

/*  CXAudio2::OnBufferEnd
callback function called by the source voice
IN:
pBufferContext		-	userdata, unused
*/
void CXAudio2::OnBufferEnd(void *pBufferContext)
{
	InterlockedIncrement(&emptyBuffers);		// we are possibly decrementing on another thread
	SetEvent(eventHandle);						// signal mixing thread that a new buffer can be filled
}

/*  CXAudio2::PushBuffer
pushes one buffer onto the source voice playback queue
IN:
AudioBytes		-	size of the buffer
pAudioData		-	pointer to the buffer
pContext		-	context passed to the callback, unused
*/
void CXAudio2::PushBuffer(UINT32 AudioBytes,BYTE *pAudioData,void *pContext)
{
	XAUDIO2_BUFFER xa2buffer={0};
	xa2buffer.AudioBytes=AudioBytes;
	xa2buffer.pAudioData=pAudioData;
	xa2buffer.pContext=pContext;
	pSourceVoice->SubmitSourceBuffer(&xa2buffer);
}

/*  CXAudio2::SetupSound
applies current sound settings by recreating the voice objects and buffers
IN/OUT:
syncSoundBuffer		-	will point to the temp buffer that can be used for SoundSync
sample_count		-	number of samples that fit into syncSoundBuffer
-----
returns true if successful, false otherwise
*/
bool CXAudio2::SetupSound(uint8 **syncSoundBuffer,int *sample_count)
{
	*syncSoundBuffer = NULL;
	*sample_count = 0;

	if(!initDone)
		return false;

	DeInitVoices();

    mixInterval = Settings.SoundMixInterval;

    if (Settings.SoundBufferSize < 1)
        Settings.SoundBufferSize = 1;
    if (Settings.SoundBufferSize > 64)
        Settings.SoundBufferSize = 64;
	bufferCount = Settings.SoundBufferSize;

	int sampleCountNoAlign = (Settings.SoundPlaybackRate * mixInterval * (Settings.Stereo ? 2 : 1)) / 1000;
	int sampleCountAligned = 64 / (Settings.SixteenBitSound ? 2 : 1);
	while (sampleCountAligned < sampleCountNoAlign)
		sampleCountAligned *= 2;

	buffer_sampleCount = sampleCountAligned;
	bufferSize = buffer_sampleCount * (Settings.SixteenBitSound ? 2 : 1);
    sum_bufferSize = bufferSize * bufferCount;

    if (InitVoices())
    {
		SoundBuffer = new uint8[sum_bufferSize];
		this->syncSoundBuffer = new uint8[bufferSize];
		*syncSoundBuffer = this->syncSoundBuffer;
		*sample_count = buffer_sampleCount;
		//SecureZeroMemory(SoundBuffer,sum_bufferSize);				// unneccesary, we start mixing below
    }
	else
		return false;

	for(int i=0;i<bufferCount;i++) {
		BYTE *bufferBase = SoundBuffer + i * bufferSize;
		S9xMixSamplesNoLimitWrapped(bufferBase,buffer_sampleCount);
		PushBuffer(bufferSize,bufferBase,(void *)(bufferBase));
	}

	BeginPlayback();

    return true;
}

void CXAudio2::BeginPlayback()
{
	pSourceVoice->Start(0);
}

void CXAudio2::StopPlayback()
{
	pSourceVoice->Stop(0);
}

/*  CXAudio2::ProcessSound
the mixing function called by the mix thread
called once per played buffer (when the callback signals the thread)
synchronizes the syncSoundBuffer access with a critical section (if SoundSync is enabled)
*/
void CXAudio2::ProcessSound()
{
	BYTE *curBuffer;
	while(emptyBuffers) {
		curBuffer = SoundBuffer + currentBufferPos;
		if (IsSoundMuted()) {
			if (so.sixteen_bit)
				SecureZeroMemory(curBuffer, buffer_sampleCount * 2);
			else
				memset(curBuffer, 0x80, buffer_sampleCount);
		}
		else {
			EnterCriticalSection(&GUI.SoundCritSect);
			UINT32 mixed_bytes = 0;
			if(so.samples_mixed_so_far) {
				mixed_bytes = so.samples_mixed_so_far * (Settings.SixteenBitSound?2:1);
				memcpy(curBuffer,syncSoundBuffer,mixed_bytes);
			}
			S9xMixSamplesNoLimitWrapped(curBuffer+mixed_bytes,buffer_sampleCount-so.samples_mixed_so_far);
			so.samples_mixed_so_far = 0;
			LeaveCriticalSection(&GUI.SoundCritSect);
		}
		PushBuffer(bufferSize,curBuffer,(void *)curBuffer);
		currentBufferPos = (currentBufferPos + bufferSize) % sum_bufferSize;
		InterlockedDecrement(&emptyBuffers);
	}
}

/*  CXAudio2::SoundThread
this is the actual mixing thread, runs until DeInitXAudio2 sets exitThread
since this is a new thread we need CoInitialzeEx to allow XAudio2 access
IN:
lpParameter		-	pointer to the CXAudio2 object
-----
returns true if successful, false otherwise
*/
unsigned int __stdcall CXAudio2::SoundThread (LPVOID lpParameter)
{
	CXAudio2 *S9xXAudio2=(CXAudio2 *)lpParameter;
	CoInitializeEx(NULL, COINIT_MULTITHREADED);
	while(1) {
		WaitForSingleObject(S9xXAudio2->eventHandle,INFINITE);
		if(S9xXAudio2->exitThread) {
			CoUninitialize();
			_endthreadex(0);
		}
		S9xXAudio2->ProcessSound();
	}
	return 0;
}