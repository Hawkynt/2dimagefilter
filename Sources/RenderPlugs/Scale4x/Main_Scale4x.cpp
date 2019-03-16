
//---------------------------------------------------------------------------------------------------------------------------
// Scale4x plugin example - KarLKoX 2004.
// This plugin uses code by Andrea Mazzoleni - see "scale2x.h" for info
// The original code and description of the algorithm can be found at:
// http://scale2x.sourceforge.net
//---------------------------------------------------------------------------------------------------------------------------

#include "../RPI.h"

//---------------------------------------------------------------------------------------------------------------------------

#ifdef	_WINDOWS
#include	"../Scale2x/scale2x_vc.h"
#else
#include	"../Scale2x/scale2x_gcc.h"
#endif

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	// Provide a name for this Render Plugin (max 60 chars) as follows:
	// Name (Original Author)
	// Make sure the name is correct with respect to capitalisation, etc.
	// For example, this plugin is called "Scale4x (Andrea Mazzoleni)"
	// because that is what the Original Author (Andrea Mazzoleni) calls it.
	// If you cannot fit the Original Authors name in then you may shorten the Plugin name,
	// BUT DO NOT GO OVER 60 CHARS.

	//                         "............................................................"
	rpi_strcpy(&MyRPI.Name[0], "Scale4x (Andrea Mazzoleni)");

	// Set the Version Number and other flags.
	// In this case, the code requires MMX instructions, so set both the REQD and USED flags.
	// It supports 555 and 565 rendering, and scales by 4.

	MyRPI.Flags=RPI_VERSION | RPI_MMX_USED | RPI_MMX_REQD | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL4;

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

static unsigned short	LineBuffer[2560*4];

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	unsigned int y;
	unsigned char *src_prev = (unsigned char *)rpo->SrcPtr;
	unsigned char *src_curr = (unsigned char *)rpo->SrcPtr;
	unsigned char *src_next = (unsigned char *)rpo->SrcPtr + rpo->SrcPitch;
	unsigned char *dstbuf   = (unsigned char *)rpo->DstPtr;

	unsigned long	dbl_prev = 0;
	unsigned long	dbl_curr = 0;
	unsigned long	dbl_next = 2560;
	unsigned long	dbl_out1  = 0;
	unsigned long	dbl_out2  = 2560;

	// Make sure I can use this renderer - in this case, just width/height checks.
	// (Since MMX is required, it would have been rejected by this point.)

	// first make sure the doubled version will fit in my intermediate buffer.
	if((rpo->SrcW*2)<=2560)
	{
		// now make sure the 4x version will fit in the destination buffer.
		if(	((rpo->SrcW*4)<=rpo->DstW) && ((rpo->SrcH*4)<=rpo->DstH) )
		{
			scale2x_16_mmx((unsigned short *)&LineBuffer[0], (unsigned short *)&LineBuffer[2560], (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);
			src_prev = src_curr;
			src_curr = src_next;
			src_next += rpo->SrcPitch;
			scale2x_16_mmx((unsigned short *)&LineBuffer[5120], (unsigned short *)&LineBuffer[7680], (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);

			scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)&LineBuffer[dbl_prev], (unsigned short *)&LineBuffer[dbl_curr], (unsigned short *)&LineBuffer[dbl_next], rpo->SrcW*2);
			dbl_prev=dbl_curr;
			dbl_curr=dbl_next;
			dbl_next+=2560;
			if(dbl_next==10240)	dbl_next=0;
			dstbuf+=rpo->DstPitch<<1;
			scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)&LineBuffer[dbl_prev], (unsigned short *)&LineBuffer[dbl_curr], (unsigned short *)&LineBuffer[dbl_next], rpo->SrcW*2);

			for (y = 2; y < rpo->SrcH; y++)
			{
				dbl_prev=dbl_curr;
				dbl_curr=dbl_next;
				dbl_next+=2560;
				if(dbl_next==10240)	dbl_next=0;
				dstbuf+=rpo->DstPitch<<1;
				scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)&LineBuffer[dbl_prev], (unsigned short *)&LineBuffer[dbl_curr], (unsigned short *)&LineBuffer[dbl_next], rpo->SrcW*2);

				src_prev = src_curr;
				src_curr = src_next;
				src_next += rpo->SrcPitch;
				scale2x_16_mmx((unsigned short *)&LineBuffer[dbl_out1], (unsigned short *)&LineBuffer[dbl_out2], (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);
				dbl_out1^=5120;
				dbl_out2^=5120;

				dbl_prev=dbl_curr;
				dbl_curr=dbl_next;
				dbl_next+=2560;
				if(dbl_next==10240)	dbl_next=0;
				dstbuf+=rpo->DstPitch<<1;
				scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)&LineBuffer[dbl_prev], (unsigned short *)&LineBuffer[dbl_curr], (unsigned short *)&LineBuffer[dbl_next], rpo->SrcW*2);
			}

			dbl_prev=dbl_curr;
			dbl_curr=dbl_next;
			dbl_next+=2560;
			if(dbl_next==10240)	dbl_next=0;
			dstbuf+=rpo->DstPitch<<1;
			scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)&LineBuffer[dbl_prev], (unsigned short *)&LineBuffer[dbl_curr], (unsigned short *)&LineBuffer[dbl_next], rpo->SrcW*2);

			src_prev = src_curr;
			src_curr = src_next;
			scale2x_16_mmx((unsigned short *)&LineBuffer[dbl_out1], (unsigned short *)&LineBuffer[dbl_out2], (unsigned short *)src_prev, (unsigned short *)src_curr, (unsigned short *)src_next, rpo->SrcW);
			dbl_out1^=5120;
			dbl_out2^=5120;

			dbl_prev=dbl_curr;
			dbl_curr=dbl_next;
			dbl_next+=2560;
			if(dbl_next==10240)	dbl_next=0;
			dstbuf+=rpo->DstPitch<<1;
			scale2x_16_mmx((unsigned short *)dstbuf, (unsigned short *)(dstbuf + rpo->DstPitch), (unsigned short *)&LineBuffer[dbl_prev], (unsigned short *)&LineBuffer[dbl_curr], (unsigned short *)&LineBuffer[dbl_next], rpo->SrcW*2);
			
#ifdef _WINDOWS
			__asm
			{
				emms;
			}
#else
			__asm__ __volatile__("emms");
#endif

		}
	}

	// Set the output size incase anybody cares.

	rpo->OutW=rpo->SrcW*4;
	rpo->OutH=rpo->SrcH*4;
}

//---------------------------------------------------------------------------------------------------------------------------
