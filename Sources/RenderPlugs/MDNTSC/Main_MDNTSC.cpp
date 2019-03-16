
// MegaDrive/Genesis and SMS NTSC filter plugin
//
// Filters developed by Shay Green (Blargg)
//  - see various source files for more information.
// 
// Plugin originally written by AamirM
// Rewritten by Steve Snake to:
// 1. Fix Source Pitch
// 2. Support both 555 and 565 modes.

#include "../RPI.h"
#include "md/md_ntsc.h"

static	md_ntsc_setup_t		mdsetup;
static	md_ntsc_t			mdntsc;

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	rpi_strcpy(&MyRPI.Name[0], "MD NTSC (Blargg)");
	MyRPI.Flags= RPI_VERSION | RPI_MMX_USED | RPI_MMX_REQD | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL2;
	mdsetup=md_ntsc_composite;
	md_ntsc_init(&mdntsc, &mdsetup);
	return(&MyRPI);
}

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	if(((rpo->SrcW*2)<=rpo->DstW) && ((rpo->SrcH*2)<=rpo->DstH))
	{
		if(rpo->Flags&RPI_565_SUPP)	md_ntsc_blit_565(&mdntsc, (unsigned short *)rpo->SrcPtr, rpo->SrcPitch>>1, rpo->SrcW, rpo->SrcH, rpo->DstPtr, rpo->DstPitch);
		if(rpo->Flags&RPI_555_SUPP)	md_ntsc_blit_555(&mdntsc, (unsigned short *)rpo->SrcPtr, rpo->SrcPitch>>1, rpo->SrcW, rpo->SrcH, rpo->DstPtr, rpo->DstPitch);
	}
	rpo->OutW=rpo->SrcW*2;
	rpo->OutH=rpo->SrcH*2;
}
