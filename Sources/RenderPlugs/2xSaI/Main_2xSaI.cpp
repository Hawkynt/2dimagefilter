
//---------------------------------------------------------------------------------------------------------------------------
// 2xSaI plugin example - Steve Snake 2004.
// This plugin uses (modified) code by Derek Liauw Kie Fa aka Kreed - see "2xSaI.asm" for info
// Kreeds 2xSaI webpage can be found here:
// http://elektron.its.tudelft.nl/~dalikifa/
//---------------------------------------------------------------------------------------------------------------------------

#include "../RPI.h"

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void Render_2xSaI(unsigned char *Src, unsigned char *Dst, DWORD sw, DWORD sh, DWORD dPitch, DWORD sPitch);

extern	"C"	BYTE	VideoFormat=0;

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	// Provide a name for this Render Plugin (max 60 chars) as follows:
	// Name (Original Author)
	// Make sure the name is correct with respect to capitalisation, etc.
	// For example, this plugin is called "2xSaI advanced 2x Scale and Interpolation engine"
	// because that is what the Original Author (Kreed) calls it.
	// If you cannot fit the Original Authors name in then you may shorten the Plugin name,
	// BUT DO NOT GO OVER 60 CHARS.

	//                         "............................................................"
	rpi_strcpy(&MyRPI.Name[0], "2xSaI (Kreed)");

	// Set the Version Number and other flags.
	// In this case, the code requires MMX instructions, so set both the REQD and USED flags.
	// It supports 555 and 565 rendering.
	// It scales output by 2x.

	MyRPI.Flags=RPI_VERSION | RPI_MMX_USED | RPI_MMX_REQD | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL2;

	// Do any other setup required here.

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	// Make sure I can use this renderer - in this case, width/height checks.
	// (Since MMX and 565 or 555 formats are required, it would have been rejected by this point.)

	if(	((rpo->SrcW*2)<=rpo->DstW) && ((rpo->SrcH*2)<=rpo->DstH) )
	{
		if(rpo->Flags&RPI_565_SUPP)	VideoFormat=0;
		else						VideoFormat=1;

		Render_2xSaI((unsigned char *)rpo->SrcPtr, (unsigned char *)rpo->DstPtr, rpo->SrcW, rpo->SrcH, rpo->DstPitch, rpo->SrcPitch);
	}

	// Set the output size incase anybody cares.

	rpo->OutW=rpo->SrcW*2;
	rpo->OutH=rpo->SrcH*2;
}

//---------------------------------------------------------------------------------------------------------------------------
