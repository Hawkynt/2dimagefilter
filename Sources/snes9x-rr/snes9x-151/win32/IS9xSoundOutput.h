/* IS9xSoundOutput.h - written by OV2 */

#ifndef IS9XSOUNDOUTPUT_H
#define IS9XSOUNDOUTPUT_H
#include "../port.h"

/* IS9xSoundOutput
	Interface for the sound output.
*/

class IS9xSoundOutput {
public:
	// InitSoundOutput should initialize the sound output but not start playback
	virtual bool InitSoundOutput(void)=0;

	// DeInitSoundOutput should stop playback and uninitialize the output
	virtual void DeInitSoundOutput(void)=0;

	// SetupSound should apply the current sound settings for outputbuffers/devices and
	// (re)start playback
	// if output doesn't support SoundSync, the pointer should be set to NULL and sample_count to 0
	virtual bool SetupSound(uint8 **syncSoundBuffer,int *sample_count)=0;
};

#endif