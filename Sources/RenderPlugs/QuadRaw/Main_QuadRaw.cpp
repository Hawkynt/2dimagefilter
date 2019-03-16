
//---------------------------------------------------------------------------------------------------------------------------
// QuadRaw plugin - Steve Snake 2009.
//---------------------------------------------------------------------------------------------------------------------------

#include "../RPI.h"

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	// Provide a name for this Render Plugin (max 60 chars) as follows:
	// Name (Original Author)
	// Make sure the name is correct with respect to capitalisation, etc.
	// For example, this plugin is called "Double"
	// If you cannot fit the Original Authors name in then you may shorten the Plugin name,
	// BUT DO NOT GO OVER 60 CHARS.

	//                         "............................................................"
	rpi_strcpy(&MyRPI.Name[0], "QuadRaw (Snake)");

	// Set the Version Number and other flags.
	// RPI_DST_WIDE flag means this plugin will write DstW pixels instead of SrcW*SCALE pixels.
	// However, DstW must be >= SrcW*SCALE.

	MyRPI.Flags=RPI_VERSION | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL4 | RPI_DST_WIDE;

	// Do any other setup required here.

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	// Make sure I can use this renderer - in this case, width/height checks.
	// Will write DstW pixels, but there must be room for at least SrcW*4 pixels.
	if(	((rpo->SrcW*4)<=rpo->DstW) && ((rpo->SrcH*4)<=rpo->DstH) )
	{
		DWORD	i, j, j2, k, v;
		WORD	*src=(WORD *)rpo->SrcPtr;
		DWORD	*dst=(DWORD *)rpo->DstPtr;
		DWORD	p1, p2, p3, p4;
		p4=rpo->DstPitch;
		p1=p4>>2;
		p2=p1<<1;
		p3=p2+p1;

		for(i=0; i<rpo->SrcH; i++)
		{
			k=0;
			while(k<((rpo->DstW>>2)-rpo->SrcW))
			{
				dst[k]=0;
				dst[k+1]=0;
				dst[k+p1]=0;
				dst[k+1+p1]=0;
				dst[k+p2]=0;
				dst[k+1+p2]=0;
				dst[k+p3]=0;
				dst[k+1+p3]=0;
				k+=2;
			}

			for(j=0; j<rpo->SrcW; j++)
			{
				j2=(j<<1)+k;
				v=src[j];
				v=(v<<16)+v;
				dst[j2]=v;
				dst[j2+1]=v;
				dst[j2+p1]=v;
				dst[j2+1+p1]=v;
				dst[j2+p2]=v;
				dst[j2+1+p2]=v;
				dst[j2+p3]=v;
				dst[j2+1+p3]=v;
			}
			k+=j<<1;

			while(k<(rpo->DstW>>1))
			{
				dst[k]=0;
				dst[k+1]=0;
				dst[k+p1]=0;
				dst[k+1+p1]=0;
				dst[k+p2]=0;
				dst[k+1+p2]=0;
				dst[k+p3]=0;
				dst[k+1+p3]=0;
				k+=2;
			}

			src+=rpo->SrcPitch/2;
			dst+=p4;
		}
	}

	// Set the output size incase anybody cares.
	// For RPI_DST_WIDE, at least the OutW will be used.
	rpo->OutW=rpo->DstW;
	rpo->OutH=rpo->SrcH*4;
}

//---------------------------------------------------------------------------------------------------------------------------
