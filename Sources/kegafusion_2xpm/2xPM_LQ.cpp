
//---------------------------------------------------------------------------------------------------------------------------
// 2xPM plugin for Kega Fusion - Pablo Medina (aka "pm") (pjmedina3@yahoo.com)
// Check for updated versions at: http://2xpm.freeservers.com
//---------------------------------------------------------------------------------------------------------------------------

#define		WIN32_LEAN_AND_MEAN
#include	<windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h> 
#include <time.h>

#define ENHANCE_COLORS 0
#define LQ_ALPHA_BLEND 0
#define SHOW_CHANGES 0

unsigned short int pg_red_mask;
unsigned short int pg_green_mask;
unsigned short int pg_blue_mask;
unsigned short int pg_lbmask;

#define RED_MASK565   0xF800
#define GREEN_MASK565 0x07E0
#define BLUE_MASK565  0x001F

#define RED_MASK555 0x7C00
#define GREEN_MASK555 0x03E0
#define BLUE_MASK555 0x001F

#define PG_LBMASK565 0xF7DE
#define PG_LBMASK555 0x7BDE

#if LQ_ALPHA_BLEND
#define ALPHA_BLEND_128_W(dst, src) dst = (src + dst) >> 1
#else
#define ALPHA_BLEND_128_W(dst, src) dst = ((src & pg_lbmask) >> 1) + ((dst & pg_lbmask) >> 1)
#endif

//---------------------------------------------------------------------------------------------------------------------------

typedef struct
{
	unsigned long	Size;
	unsigned long	Flags;
	void			*SrcPtr;
	unsigned long	SrcPitch;
	unsigned long	SrcW;
	unsigned long	SrcH;
	void			*DstPtr;
	unsigned long	DstPitch;
	unsigned long	DstW;
	unsigned long	DstH;
	unsigned long	OutW;
	unsigned long	OutH;
} RENDER_PLUGIN_OUTP;

//---------------------------------------------------------------------------------------------------------------------------

typedef	void	(*RENDPLUG_Output)(RENDER_PLUGIN_OUTP *);

//---------------------------------------------------------------------------------------------------------------------------

typedef struct
{
	char			Name[60];
	unsigned long	Flags;
	HMODULE			Handle;
	RENDPLUG_Output	Output;
} RENDER_PLUGIN_INFO;

RENDER_PLUGIN_INFO	MyRPI;

//---------------------------------------------------------------------------------------------------------------------------

typedef	RENDER_PLUGIN_INFO	*(*RENDPLUG_GetInfo)(void);

//---------------------------------------------------------------------------------------------------------------------------

#define	RPI_VERSION		0x02

#define	RPI_MMX_USED	0x000000100
#define	RPI_MMX_REQD	0x000000200
#define	RPI_555_SUPP	0x000000400
#define	RPI_565_SUPP	0x000000800
#define	RPI_888_SUPP	0x000001000

#define	RPI_OUT_SCL2	0x000020000
#define	RPI_OUT_SCL3	0x000030000
#define	RPI_OUT_SCL4	0x000040000

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	__declspec(dllexport)	RENDER_PLUGIN_INFO *RenderPluginGetInfo(void);
extern	"C"	__declspec(dllexport)	void	RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo);

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	void Render_2xSaI(unsigned char *Src, unsigned char *Dst, DWORD sw, DWORD sh, DWORD dPitch, DWORD sPitch);
extern	"C"	BYTE	VideoFormat=0;

//---------------------------------------------------------------------------------------------------------------------------

extern	"C"	RENDER_PLUGIN_INFO *
RenderPluginGetInfo(void)
{
	//                    "............................................................"
	strcpy(&MyRPI.Name[0], "2xPM LQ (pm)");

	// MyRPI.Flags=RPI_VERSION | RPI_MMX_USED | RPI_MMX_REQD | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL2;
	MyRPI.Flags=RPI_VERSION | RPI_555_SUPP | RPI_565_SUPP | RPI_OUT_SCL2;

	// Return pointer to the info structure.
	return(&MyRPI);
}

//---------------------------------------------------------------------------------------------------------------------------

static unsigned short int PBASE[32];
/*
static unsigned short int &PA = PBASE[0], &PB = PBASE[1], &PC = PBASE[2],
		               &PD = PBASE[3], &PE = PBASE[4], &PF = PBASE[5],
					   &PG = PBASE[6], &PH = PBASE[7], &PI = PBASE[8];
*/

/*
#define PB start_addr1[1]
#define PE start_addr2[1]
#define PH start_addr3[1]
#define PA start_addr1[pprev]
#define PD start_addr2[pprev]
#define PG start_addr3[pprev]
#define PC start_addr1[2]
#define PF start_addr2[2]
#define PI start_addr3[2]
*/

extern	"C"	void
RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned short int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	register unsigned short int *start_addr1, *start_addr2, *start_addr3;
	unsigned long next_line, next_line_src;	
	unsigned short int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned char auto_blend;
	unsigned short int E[4];
	unsigned long src_pitch;
	unsigned char pprev;	
	unsigned char dont_reblit;
	
#if ENHANCE_COLORS
	unsigned short int PAX, PDX, PGX, PCX, PFX, PIX;
#endif
	
	/* Don't blit anything if we don't have enough screen space */
	if(!(((rpo->SrcW << 1)<=rpo->DstW) && ((rpo->SrcH << 1)<=rpo->DstH)))
		return;

	if(rpo->Flags & RPI_565_SUPP)
	{
		pg_red_mask = RED_MASK565;
		pg_green_mask = GREEN_MASK565;
		pg_blue_mask = BLUE_MASK565;
		pg_lbmask = PG_LBMASK565;
	} else {
		pg_red_mask = RED_MASK555;
		pg_green_mask = GREEN_MASK555;
		pg_blue_mask = BLUE_MASK555;
		pg_lbmask = PG_LBMASK555;
	}

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch >> 1;				
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;
	complete_line_src = src_pitch - (rpo->SrcW << 1);
	complete_line_dst = rpo->DstPitch - rpo->DstW;
	
	start_addr2 = (unsigned short int *)(src - 2);
	start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;

	dst_pixel = (unsigned short int *)(dest);	

	y = src_height - 1;
	//for (y = 0; y < src_height; y++)
	do
	{	
		//if (y == src_height - 1)
		if (!y)
			start_addr3 = start_addr2;		
		auto_blend = 0;
		pprev = 1;
		x = src_width - 1;
		
		//for (x = 0; x < src_width; x++)
		do
		{
			PB = start_addr1[1];
			PE = start_addr2[1];			
			PH = start_addr3[1];
			
			PA = start_addr1[pprev];
			PD = start_addr2[pprev];			
			PG = start_addr3[pprev];

			//if (x < src_width - 1)
			if (x)
			{
				PC = start_addr1[2];
				PF = start_addr2[2];
				PI = start_addr3[2];
			} else {
				PC = PB;
				PF = PE;
				PI = PH;
			}

#if SHOW_CHANGES
			unsigned short int PDE = PE;
			PE = 0;
#endif
			E[0] = E[1] = E[2] = E[3] = PE;

			dont_reblit = 0;
						
			// Horizontal
			if (!dont_reblit)						
			if (PD != PF)
			{
				if ((PE != PD) && (PD == PH) && (PD == PI) && (PE != PG)
					&& ((PD != PG) || (PE != PF) || (PA != PD))
					&& (!((PD == PA) && (PD == PG) && (PE == PB) && (PE == PF)))
					)
				{				
					E[2] = PH;
					ALPHA_BLEND_128_W(E[3], PH);					
					dont_reblit = 1;
				}
						
				else if ((PE != PF) && (PF == PH) && (PF == PG) && (PE != PI)
					&& ((PF != PI) || (PE != PD) || (PC != PF))
					&& (!((PF == PC) && (PF == PI) && (PE == PB) && (PE == PD)))
					)
				{
					ALPHA_BLEND_128_W(E[2], PH);				
					E[3] = PH;	
					dont_reblit = 1;
				}			
			}		
			
			if (!dont_reblit)
			{				
				if ((PB != PH) && (PD != PF))
				{						
					if ((PB == PD) && (PE != PD)
						&& (!((PE == PF) && (((PE == PA) && (PB == PC))
							             || ((PD == PA) && (PD == PC) && (PG != PD) && (PG != PE)))))
						&& (!((PB == PA) && (PB == PG))))					
						ALPHA_BLEND_128_W(E[0], PB);
					
					else if ((PB == PF) && (PE != PF)		
						&& (!((PE == PD) && (((PE == PC) && (PB == PA))
						                 || ((PF == PA) && (PF == PC) && (PE == PD) && (PI != PF) && (PI != PE)))))
						&& (!((PB == PC) && (PB == PI)))						
						)
						ALPHA_BLEND_128_W(E[1], PB);
									
					if ((PH == PD) && ((PE != PG) || (PE != PD) || ((PE != PF) && (PF == PI)))
						&& (!((PE == PF) && (((PE == PG) && (PH == PI))
						                 || ((PD == PG) && (PD == PI) && (PE == PF) && (PA != PD) && (PA != PE))
										 || ((PE == PG) && (PH == PI)))))						
						&& (!((PH == PG) && (PH == PA)))											
						)									
						ALPHA_BLEND_128_W(E[2], PH);					
				
					else if ((PH == PF) && ((PE != PI) || (PE != PF))
						&& (!((PE == PD) && (((PE == PI) && (PH == PG))
						                 || ((PF == PG) && (PF == PI) && (PC != PF) && (PI != PE)))))
						&& (!((PH == PI) && (PH == PC)))
						)
						ALPHA_BLEND_128_W(E[3], PH);									
				} else				
#if SHOW_CHANGES
				PE = PDE;
#endif
				if ((PD == PB) && (PD == PF) && (PD == PH) && (PD != PE))					
				{					
					if ((PD == PG) || (PD == PC))
					{						
						ALPHA_BLEND_128_W(E[1], PD);
						E[2] = E[1];						
					}
					if ((PD == PA) || (PD == PI))
					{					
						ALPHA_BLEND_128_W(E[0], PD);
						E[3] = E[0];						
					}
				}
			}			
			
			dst_pixel[0] = E[0];
			dst_pixel[1] = E[1];
			dst_pixel[next_line] = E[2];
			dst_pixel[next_line + 1] = E[3];
		
			start_addr1++;
			start_addr2++;
			start_addr3++;				
			
			dst_pixel += 2;
			pprev = 0;		
		} while (x--);

		start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;
		start_addr3 = start_addr2 + next_line_src;
		dst_pixel += complete_line_dst;				
	} while(y--);
	
	// Set the output size incase anybody cares.
	rpo->OutW=(rpo->SrcW << 1);
	rpo->OutH=(rpo->SrcH << 1);

}

//---------------------------------------------------------------------------------------------------------------------------
