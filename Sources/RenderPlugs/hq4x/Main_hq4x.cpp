
//---------------------------------------------------------------------------------------------------------------------------
// hq4x plugin example - Steve Snake 2004.
// This plugin uses (modified) code by Maxim Stepin - see "hq4x16.asm" for info
// The original code and description of the algorithm can be found at:
// http://www.hiend3d.com/hq2x.html
//---------------------------------------------------------------------------------------------------------------------------

#include "../RPI.h"

//---------------------------------------------------------------------------------------------------------------------------

extern "C"
{
	void hq4x_16_565(unsigned char *src, unsigned char *dst, unsigned long sw, unsigned long sh, unsigned long dpitch, unsigned long spitch);
	void hq4x_16_555(unsigned char *src, unsigned char *dst, unsigned long sw, unsigned long sh, unsigned long dpitch, unsigned long spitch);
	unsigned int	LUT16to32[65536];
	unsigned int	RGBtoYUV[65536];
	unsigned char	VideoFormat;
}

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	// Provide a name for this Render Plugin (max 60 chars) as follows:
	// Name (Original Author)
	// Make sure the name is correct with respect to capitalisation, etc.
	// For example, this plugin is called "hq4x Magnification Filter"
	// because that is what the Original Author (Maxim Stepin) calls it.
	// If you cannot fit the Original Authors name in then you may shorten the Plugin name,
	// BUT DO NOT GO OVER 60 CHARS.

	//                         "............................................................"
	rpi_strcpy(&MyRPI.Name[0], "hq4x (Maxim Stepin)");

	// Set the Version Number and other flags.
	// In this case, the code requires MMX instructions, so set both the REQD and USED flags.
	// It supports 555 and 565 rendering.
	// It scales output by 4x.

	MyRPI.Flags=RPI_VERSION | RPI_MMX_USED | RPI_MMX_REQD | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL4;

	// Do any other setup required here.
	// We can't do the setup until we know what format is required, so set invalid format.
	VideoFormat=0xff;

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

extern "C"	void
SetupFormat(unsigned char format)
{
	VideoFormat=format;

	// This code originally taken from "hq2x filter demo program" by Maxim Stepin,
	// cleaned up and modified by Steve Snake to also handle 555 format.

	int i, j, k, r, g, b, Y, u, v;

	if(VideoFormat==0)	//565
	{
		for(i=0; i<65536; i++)	LUT16to32[i]=((i&0xf800)<<8) + ((i&0x07e0)<<5) + ((i&0x001f)<<3);

		for(i=0; i<32; i++)
		{
			for(j=0; j<64; j++)
			{
				for(k=0; k<32; k++)
				{
					r = i << 3;
					g = j << 2;
					b = k << 3;
					Y = (r + g + b) >> 2;
					u = 128 + ((r - b) >> 2);
					v = 128 + ((-r + 2*g -b)>>3);
					RGBtoYUV[ (i << 11) + (j << 5) + k ] = (Y<<16) + (u<<8) + v;
				}
			}
		}
	}

	if(VideoFormat==1)	//555
	{
		for(i=0; i<65536; i++)	LUT16to32[i]=((i&0x7c00)<<9) + ((i&0x03e0)<<6) + ((i&0x001f)<<3);

		for(i=0; i<32; i++)
		{
			for(j=0; j<32; j++)
			{
				for(k=0; k<32; k++)
				{
					r=i<<3;
					g=j<<3;
					b=k<<3;
					Y=(r + g + b)>>2;
					u=128 + ((r - b) >> 2);
					v=128 + ((-r + 2*g -b)>>3);
					RGBtoYUV[ (i << 10) + (j << 5) + k ] = (Y<<16) + (u<<8) + v;
					RGBtoYUV[ 0x8000 + (i << 10) + (j << 5) + k ] = (Y<<16) + (u<<8) + v;
				}
			}
		}
	}
}

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	// not initialised yet?
	if(VideoFormat==0xff)
	{
		if(rpo->Flags&RPI_565_SUPP)	SetupFormat(0);
		if(rpo->Flags&RPI_555_SUPP)	SetupFormat(1);
	}

	// Make sure I can use this renderer - in this case, just width/height checks.
	// (Since MMX is required, it would have been rejected by this point.)

	if(	((rpo->SrcW*4)<=rpo->DstW) && ((rpo->SrcH*4)<=rpo->DstH) )
	{
		if(VideoFormat==0)	hq4x_16_565((unsigned char *)rpo->SrcPtr, (unsigned char *)rpo->DstPtr, rpo->SrcW, rpo->SrcH, rpo->DstPitch, rpo->SrcPitch);
		if(VideoFormat==1)	hq4x_16_555((unsigned char *)rpo->SrcPtr, (unsigned char *)rpo->DstPtr, rpo->SrcW, rpo->SrcH, rpo->DstPitch, rpo->SrcPitch);
	}

	// Set the output size incase anybody cares.

	rpo->OutW=rpo->SrcW*4;
	rpo->OutH=rpo->SrcH*4;
}

//---------------------------------------------------------------------------------------------------------------------------
