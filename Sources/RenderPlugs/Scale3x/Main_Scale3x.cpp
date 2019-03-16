
//---------------------------------------------------------------------------------------------------------------------------
// Scale3x plugin example - KarLKoX 2004.
// This plugin uses code by Andrea Mazzoleni - see "scale3x.h" for info
// The original code and description of the algorithm can be found at:
// http://scale2x.sourceforge.net
//---------------------------------------------------------------------------------------------------------------------------

#include "../RPI.h"

//---------------------------------------------------------------------------------------------------------------------------

#include	"scale3x.h"

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	// Provide a name for this Render Plugin (max 60 chars) as follows:
	// Name (Original Author)
	// Make sure the name is correct with respect to capitalisation, etc.
	// For example, this plugin is called "hq2x Magnification Filter"
	// because that is what the Original Author (Maxim Stepin) calls it.
	// If you cannot fit the Original Authors name in then you may shorten the Plugin name,
	// BUT DO NOT GO OVER 60 CHARS.

	//                         "............................................................"
	rpi_strcpy(&MyRPI.Name[0], "Scale3x (Andrea Mazzoleni)");

	// Set the Version Number and other flags.
	// In this case, the code doesn't require MMX instructions.
	// It supports 555 and 565 rendering, and scales by 3.

	MyRPI.Flags=RPI_VERSION | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL3;

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	unsigned int y;
	unsigned char *src_prev = (unsigned char *)rpo->SrcPtr;
	unsigned char *src_curr = (unsigned char *)rpo->SrcPtr;
	unsigned char *src_next = (unsigned char *)rpo->SrcPtr + rpo->SrcPitch;
	unsigned char *dstbuf   = (unsigned char *)rpo->DstPtr;
	
	// Make sure I can use this renderer - in this case, just width/height checks.

	if(	((rpo->SrcW*3)<=rpo->DstW) && ((rpo->SrcH*3)<=rpo->DstH) )
	{
		
		scale3x_16_def((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)(dstbuf + 2 * rpo->DstPitch), (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);

		for (y = 3; y < rpo->SrcH; y++)
		{
			dstbuf += 3 * rpo->DstPitch;
			src_prev = src_curr;
			src_curr = src_next;
			src_next += rpo->SrcPitch;
			scale3x_16_def((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)(dstbuf + 2 * rpo->DstPitch), (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);
		}

		dstbuf += 3 * rpo->DstPitch;
		src_prev = src_curr;
		src_curr = src_next;

		scale3x_16_def((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)(dstbuf + 2 * rpo->DstPitch), (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);
	}

	// Set the output size incase anybody cares.

	rpo->OutW=rpo->SrcW*3;
	rpo->OutH=rpo->SrcH*3;
}

//---------------------------------------------------------------------------------------------------------------------------

