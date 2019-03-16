
//---------------------------------------------------------------------------------------------------------------------------
// Double plugin - Steve Snake 2004.
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
	rpi_strcpy(&MyRPI.Name[0], "Double (Snake)");

	// Set the Version Number and other flags.

	MyRPI.Flags=RPI_VERSION | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL2;

	// Do any other setup required here.

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{
	// Make sure I can use this renderer - in this case, width/height checks.

	if(	((rpo->SrcW*2)<=rpo->DstW) && ((rpo->SrcH*2)<=rpo->DstH) )
	{
		DWORD	i, j, v;
		WORD	*src=(WORD *)rpo->SrcPtr;
		DWORD	*dst=(DWORD *)rpo->DstPtr;

		for(i=0; i<rpo->SrcH; i++)
		{
			for(j=0; j<rpo->SrcW; j++)
			{
				v=src[j];
				v=(v<<16)+v;
				dst[j]=v;
				dst[j+(rpo->DstPitch/4)]=v;
			}
			src+=rpo->SrcPitch/2;
			dst+=rpo->DstPitch/2;
		}
	}

	// Set the output size incase anybody cares.

	rpo->OutW=rpo->SrcW*2;
	rpo->OutH=rpo->SrcH*2;
}

//---------------------------------------------------------------------------------------------------------------------------
