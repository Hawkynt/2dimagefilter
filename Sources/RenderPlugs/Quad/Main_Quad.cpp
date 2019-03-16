
//---------------------------------------------------------------------------------------------------------------------------
// Quad plugin - Steve Snake 2009.
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
	rpi_strcpy(&MyRPI.Name[0], "Quad (Snake)");

	// Set the Version Number and other flags.

	MyRPI.Flags=RPI_VERSION | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL4;

	// Do any other setup required here.

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	// Make sure I can use this renderer - in this case, width/height checks.

	if(	((rpo->SrcW*4)<=rpo->DstW) && ((rpo->SrcH*4)<=rpo->DstH) )
	{
		DWORD	i, j, j2, v;
		WORD	*src=(WORD *)rpo->SrcPtr;
		DWORD	*dst=(DWORD *)rpo->DstPtr;
		DWORD	p1, p2, p3, p4;
		p4=rpo->DstPitch;
		p1=p4>>2;
		p2=p1<<1;
		p3=p2+p1;

		for(i=0; i<rpo->SrcH; i++)
		{
			for(j=0; j<rpo->SrcW; j++)
			{
				j2=j<<1;
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
			src+=rpo->SrcPitch/2;
			dst+=p4;
		}
	}

	// Set the output size incase anybody cares.

	rpo->OutW=rpo->SrcW*4;
	rpo->OutH=rpo->SrcH*4;
}

//---------------------------------------------------------------------------------------------------------------------------
