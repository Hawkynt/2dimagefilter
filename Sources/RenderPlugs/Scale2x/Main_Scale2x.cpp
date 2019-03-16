
//---------------------------------------------------------------------------------------------------------------------------
// Scale2x plugin example - KarLKoX 2004.
// This plugin uses code by Andrea Mazzoleni - see "scale2x.h" for info
// The original code and description of the algorithm can be found at:
// http://scale2x.sourceforge.net
//---------------------------------------------------------------------------------------------------------------------------

#include "../RPI.h"

//---------------------------------------------------------------------------------------------------------------------------

#ifdef	_WINDOWS
#include	"scale2x_vc.h"
#else
#include	"scale2x_gcc.h"
#endif

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	// Provide a name for this Render Plugin (max 60 chars) as follows:
	// Name (Original Author)
	// Make sure the name is correct with respect to capitalisation, etc.
	// For example, this plugin is called "Scale2x (Andrea Mazzoleni)"
	// because that is what the Original Author (Andrea Mazzoleni) calls it.
	// If you cannot fit the Original Authors name in then you may shorten the Plugin name,
	// BUT DO NOT GO OVER 60 CHARS.

	//                         "............................................................"
	rpi_strcpy(&MyRPI.Name[0], "Scale2x (Andrea Mazzoleni)");

	// Set the Version Number and other flags.
	// In this case, the code requires MMX instructions, so set both the REQD and USED flags.
	// It supports 555 and 565 rendering.

	MyRPI.Flags=RPI_VERSION | RPI_MMX_USED | RPI_MMX_REQD | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL2;

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
	// (Since MMX is required, it would have been rejected by this point.)

	if(	((rpo->SrcW*2)<=rpo->DstW) && ((rpo->SrcH*2)<=rpo->DstH) )
	{
		scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);

		for (y = 2; y < rpo->SrcH; y++)
		{
			dstbuf += 2 * rpo->DstPitch;
			src_prev = src_curr;
			src_curr = src_next;
			src_next += rpo->SrcPitch;
			scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);
		}

		dstbuf += 2 * rpo->DstPitch;
		src_prev = src_curr;
		src_curr = src_next;

		scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);
		
#ifdef _WINDOWS
		__asm
		{
			emms;
		}
#else
		__asm__ __volatile__("emms");
#endif

	}

	// Set the output size incase anybody cares.

	rpo->OutW=rpo->SrcW*2;
	rpo->OutH=rpo->SrcH*2;
}

//---------------------------------------------------------------------------------------------------------------------------
