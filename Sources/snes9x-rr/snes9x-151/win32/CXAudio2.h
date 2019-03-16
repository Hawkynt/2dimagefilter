/* CXAudio2.h - written by OV2 */

#ifndef CXHAUDIO2_H
#define CXAUDIO2_H
#include "XAudio2.h"
#include "../snes9x.h"
#include <windows.h>
#include "IS9xSoundOutput.h"

class CXAudio2 : public IXAudio2VoiceCallback, public IS9xSoundOutput
{
private:
	IXAudio2SourceVoice *pSourceVoice;
	IXAudio2 *pXAudio2;
	IXAudio2MasteringVoice* pMasterVoice;
	HANDLE eventHandle;
	HANDLE threadHandle;

	bool initDone;							// has init been called successfully?

	int mixInterval;						// the mixing interval in ms
	int bufferCount;						// number of buffers
	UINT32 bufferSize;						// size of one mix buffer in bytes
	UINT32 buffer_sampleCount;				// samples in one buffer
	UINT32 sum_bufferSize;					// the size of SoundBuffer (sum of individual buffers)
	uint8 *SoundBuffer;						// memory address used for the buffers

	uint8 *syncSoundBuffer;					// the buffer for SoundSync (used in S9XGenerateSound)

	volatile bool exitThread;				// switch to exit the thread
	volatile LONG emptyBuffers;				// number of empty buffers
	UINT32 currentBufferPos;				// the next buffer offset we will mix into

	bool InitVoices(void);
	void DeInitVoices(void);

	void PushBuffer(UINT32 AudioBytes,BYTE *pAudioData,void *pContext);	
	void BeginPlayback(void);
	void StopPlayback(void);
	void ProcessSound(void);
	bool InitXAudio2(void);
	void DeInitXAudio2(void);

	static unsigned int __stdcall SoundThread (LPVOID lpParameter);

public:
	CXAudio2(void);
	~CXAudio2(void);
		
	// inherited from IXAudio2VoiceCallback - we only use OnBufferEnd
	STDMETHODIMP_(void) OnBufferEnd(void *pBufferContext);
	STDMETHODIMP_(void) OnBufferStart(void *pBufferContext){}
	STDMETHODIMP_(void) OnLoopEnd(void *pBufferContext){}
	STDMETHODIMP_(void) OnStreamEnd() {}
	STDMETHODIMP_(void) OnVoiceError(void *pBufferContext, HRESULT Error) {}
	STDMETHODIMP_(void) OnVoiceProcessingPassEnd() {}
	STDMETHODIMP_(void) OnVoiceProcessingPassStart(UINT32 BytesRequired) {}


	// Inherited from IS9xSoundOutput
	bool InitSoundOutput(void) { return InitXAudio2(); }
	void DeInitSoundOutput(void) { DeInitXAudio2(); }
	bool SetupSound(uint8 **syncSoundBuffer,int *sample_count);
};

#endif