#ifndef CFMODEX_H
#define CFMODEX_H
#include "FMODEx/api/inc/fmod.hpp"
#include "IS9xSoundOutput.h"

/* Note: FMOD Ex is currently unusable in SNES9X
		 There is too much latency, and I've been unable to get "normal" sound output.
		 Init also seems to force CoInitializeEx with COINIT_APARTMENTTHREADED, which conflicts
		 with XAudio2 */

class CFMODEx: public IS9xSoundOutput
{
	bool initDone;

	FMOD::System *fmodSystem;
	FMOD::Sound *fmodStream;


	bool InitFMODEx();
	void DeInitFMODEx();

	bool InitStream();
	void DeInitStream();

	static FMOD_RESULT F_CALLBACK FMODExStreamCallback(
	  FMOD_SOUND *  sound, 
	  void *  data, 
	  unsigned int  datalen
	);

public:
	CFMODEx(void);
	~CFMODEx(void);

	// Inherited from IS9xSoundOutput
	bool InitSoundOutput(void) { return InitFMODEx(); }
	void DeInitSoundOutput(void) { DeInitFMODEx(); }
	bool SetupSound(uint8 **syncSoundBuffer,int *sample_count);
};

#endif