
#ifdef _WINDOWS

#define		WIN32_LEAN_AND_MEAN
#include	<windows.h>

#else

typedef	unsigned long	DWORD;
typedef unsigned short	WORD;
typedef unsigned char	BYTE;
typedef void *			HMODULE;

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

#define	RPI_DST_WIDE	0x000008000

#define	RPI_OUT_SCL1	0x000010000
#define	RPI_OUT_SCL2	0x000020000
#define	RPI_OUT_SCL3	0x000030000
#define	RPI_OUT_SCL4	0x000040000

//---------------------------------------------------------------------------------------------------------------------------

#ifdef _WINDOWS
extern	"C"	__declspec(dllexport)	RENDER_PLUGIN_INFO *RenderPluginGetInfo(void);
extern	"C"	__declspec(dllexport)	void	RenderPluginOutput(RENDER_PLUGIN_OUTP *rpo);
#endif

//---------------------------------------------------------------------------------------------------------------------------

static void rpi_strcpy(char *out, char *in)
{
	while(1)
	{
		*out++=*in;
		if(!(*in++))	break;
	}
}

//---------------------------------------------------------------------------------------------------------------------------
