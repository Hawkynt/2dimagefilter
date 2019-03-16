// Programmed by Hyllian - 2011

#include "lodepng.h"
#include <stdio.h>

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


unsigned int pg_red_mask;
unsigned int pg_green_mask;
unsigned int pg_blue_mask;
unsigned int pg_alpha_mask;
unsigned int pg_lbmask;

int eq(unsigned int A, unsigned int B);
float df(unsigned int A, unsigned int B);

void Filter_2xBR(RENDER_PLUGIN_OUTP *rpo);
void Filter_2xBR_v2(RENDER_PLUGIN_OUTP *rpo);
void Filter_2xBR_v3(RENDER_PLUGIN_OUTP *rpo);
void Filter_2xBR_v4(RENDER_PLUGIN_OUTP *rpo);
void Filter_2xBR_v5(RENDER_PLUGIN_OUTP *rpo);
void Filter_3xBR(RENDER_PLUGIN_OUTP *rpo);
void Filter_4xBR(RENDER_PLUGIN_OUTP *rpo);
void Filter_4xBR_v2(RENDER_PLUGIN_OUTP *rpo);
void Filter_5xBR(RENDER_PLUGIN_OUTP *rpo);
void Filter_6xBR(RENDER_PLUGIN_OUTP *rpo);
void Filter_7xBR(RENDER_PLUGIN_OUTP *rpo);
void Filter_2xPM(RENDER_PLUGIN_OUTP *rpo);
void Filter_Lanczos(RENDER_PLUGIN_OUTP *rpo);

#define INTERP_Y_LIMIT (0x30*4)
#define INTERP_U_LIMIT (0x07*4)
#define INTERP_V_LIMIT (0x06*8)

#define RED_MASK   0x000000FF
#define GREEN_MASK 0x0000FF00
#define BLUE_MASK  0x00FF0000
#define ALPHA_MASK 0xFF000000

#define STEP_MASK  0x00101010


#define PG_LBMASK  0xFEFEFEFE

#define AB_128_W(dst, src) dst = ((src & pg_lbmask) >> 1) + ((dst & pg_lbmask) >> 1)

#define ALPHA_BLEND_128_W(dst, src) dst = ((src & pg_lbmask) >> 1) + ((dst & pg_lbmask) >> 1)

#define ALPHA_BLEND_16_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask))) >>4)) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask))) >>4)) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask) - \
        (dst & pg_blue_mask))) >>4)) ) +\
        pg_alpha_mask

#define ALPHA_BLEND_32_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask))) >>3))) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask))) >>3))) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask) - \
        (dst & pg_blue_mask))) >>3))) ) +\
        pg_alpha_mask
         

#define ALPHA_BLEND_64_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask))) >>2))) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask))) >>2))) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask) - \
        (dst & pg_blue_mask))) >>2))) ) +\
        pg_alpha_mask

#define ALPHA_BLEND_96_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask)) * 96) >>8))) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask)) * 96) >>8))) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask) - \
        (dst & pg_blue_mask)) * 96) >>8))) ) +\
        pg_alpha_mask
        
#define ALPHA_BLEND_128_subpixel_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask)*5))/6))) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask))) >>1))) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask)*5 - \
        (dst & pg_blue_mask))) /6))) ) +\
        pg_alpha_mask

#define ALPHA_BLEND_160_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask)) * 160) >>8))) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask)) * 160) >>8))) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask) - \
        (dst & pg_blue_mask)) * 160) >>8))) ) +\
        pg_alpha_mask

#define ALPHA_BLEND_192_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask)) * 192) >>8))) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask)) * 192) >>8))) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask) - \
        (dst & pg_blue_mask)) * 192) >>8))) ) +\
        pg_alpha_mask
        
#define ALPHA_BLEND_224_W(dst, src) \
	dst = ( \
    (pg_red_mask & ((dst & pg_red_mask) + \
        ((((src & pg_red_mask) - \
        (dst & pg_red_mask)) * 224) >>8))) | \
    (pg_green_mask & ((dst & pg_green_mask) + \
        ((((src & pg_green_mask) - \
        (dst & pg_green_mask)) * 224) >>8))) | \
    (pg_blue_mask & ((dst & pg_blue_mask) + \
        ((((src & pg_blue_mask) - \
        (dst & pg_blue_mask)) * 224) >>8))) ) +\
        pg_alpha_mask


#define LEFT_3(N15, N14, N11, N13, N12, N10, N9, nc, PIXEL)\
                                ALPHA_BLEND_224_W(E[N11], PIXEL); \
                                ALPHA_BLEND_224_W(E[N12], PIXEL); \
                                ALPHA_BLEND_128_W(E[N10], PIXEL); \
                                ALPHA_BLEND_32_W( E[N9 ], PIXEL); \
                                E[N13] = PIXEL; \
                                E[N14] = PIXEL; \
                                E[N15] = PIXEL; nc++; \

#define LEFT_3R(N15, N14, nb, PIXEL)\
                  ALPHA_BLEND_128_W(E[N15], PIXEL); \
                  ALPHA_BLEND_32_W( E[N14], PIXEL); nb++;\



#define LEFT_2(N15, N14, N11, N13, N12, N10, nc, PIXEL)\
                                ALPHA_BLEND_192_W(E[N11], PIXEL); \
                                ALPHA_BLEND_192_W(E[N13], PIXEL); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_64_W( E[N12], PIXEL); \
                                E[N14] = PIXEL; \
                                E[N15] = PIXEL;nc++; \

#define UP_2(N15, N14, N11, N3, N7, N10, nc, PIXEL)\
                                ALPHA_BLEND_192_W(E[N14], PIXEL); \
                                ALPHA_BLEND_192_W(E[N7 ], PIXEL); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_64_W( E[N3 ], PIXEL); \
                                E[N11] = PIXEL; \
                                E[N15] = PIXEL; nc++;\

#define DIA(N15, N14, N11, nd, PIXEL)\
                        ALPHA_BLEND_128_W(E[N11], PIXEL); \
                        ALPHA_BLEND_128_W(E[N14], PIXEL); \
                        E[N15] = PIXEL; \
                        nd++;\

#define LEFT_2L(N15, N14, N11, N13, N12, N10, nc, PIXEL)\
                                ALPHA_BLEND_192_W(E[N11], PIXEL); \
                                ALPHA_BLEND_128_W(E[N13], 0); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_128_W( E[N12], 0); \
                                ALPHA_BLEND_128_W( E[N12], 0); \
                                E[N12] = E[N14] = E[N15] = E[N13]; \
                                nc++; \

#define UP_2L(N15, N14, N11, N3, N7, N10, nc, PIXEL)\
                                ALPHA_BLEND_192_W(E[N14], 0); \
                                ALPHA_BLEND_192_W(E[N7 ], PIXEL); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_128_W( E[N3 ], 0); \
                                E[N11] = PIXEL; \
                                E[N15] = E[N3]; nc++;\

#define DIA_L(N15, N14, N11, nd, PIXEL)\
                        ALPHA_BLEND_128_W(E[N11], PIXEL); \
                        ALPHA_BLEND_128_W(E[N14], 0); \
                        E[N15] = E[N14]; \
                        nd++;\

#define LEFT_2R(N15, N14, N11, N13, N12, N10, nc, PIXEL)\
                                ALPHA_BLEND_128_W(E[N11], 0); \
                                ALPHA_BLEND_192_W(E[N13], PIXEL); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_64_W( E[N12], 0); \
                                E[N14] = PIXEL; \
                                E[N15] = E[N11];nc++; \

#define UP_2R(N15, N14, N11, N3, N7, N10, nc, PIXEL)\
                                ALPHA_BLEND_192_W(E[N14], PIXEL); \
                                ALPHA_BLEND_192_W(E[N7 ], 0); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_64_W( E[N3 ], 0); \
                                E[N11] = E[N7]; \
                                E[N15] = E[N7]; nc++;\

#define DIA_R(N15, N14, N11, nd, PIXEL)\
                        ALPHA_BLEND_128_W(E[N11], 0); \
                        ALPHA_BLEND_128_W(E[N14], PIXEL); \
                        E[N15] = E[N11]; \
                        nd++;\


#define LEFT_UP_2_3X(N7, N5, N6, N2, N8, nc, PIXEL)\
             E[N8] = E[N7] = E[N5] = PIXEL; nc++; \

#define LEFT_2_3X(N7, N5, N6, N8, nc, PIXEL)\
             E[N8] = E[N7] = PIXEL; nc++; \

#define UP_2_3X(N5, N7, N2, N8, nc, PIXEL)\
             E[N8] = E[N5] = PIXEL; nc++; \

#define DIA_3X(N8, N5, N7, nd, PIXEL)\
             E[N8] = PIXEL; nd++; \


#define LEFT_2X(N3, N2, nc, PIXEL)\
                                ALPHA_BLEND_192_W(E[N3], PIXEL); \
                                ALPHA_BLEND_64_W( E[N2], PIXEL); \
                                nc++; \

#define UP_2X(N3, N1, nc, PIXEL)\
                                ALPHA_BLEND_192_W(E[N3], PIXEL); \
                                ALPHA_BLEND_64_W( E[N1 ], PIXEL); \
                                nc++;\

#define DIA_2X(N3, nd, PIXEL)\
                        ALPHA_BLEND_128_W(E[N3], PIXEL); \
                        nd++;\


/*
#define LEFT_UP_2_3X(N7, N5, N6, N2, N8, nc, PIXEL)\
             ALPHA_BLEND_192_W(E[N7], PIXEL); \
             ALPHA_BLEND_64_W( E[N6], PIXEL); \
             E[N5] = E[N7]; \
             E[N2] = E[N6]; \
             E[N8] =  PIXEL;\

        
#define LEFT_2_3X(N7, N5, N6, N8, nc, PIXEL)\
             ALPHA_BLEND_192_W(E[N7], PIXEL); \
             ALPHA_BLEND_64_W( E[N5], PIXEL); \
             ALPHA_BLEND_64_W( E[N6], PIXEL); \
             E[N8] =  PIXEL;\

#define UP_2_3X(N5, N7, N2, N8, nc, PIXEL)\
             ALPHA_BLEND_192_W(E[N5], PIXEL); \
             ALPHA_BLEND_64_W( E[N7], PIXEL); \
             ALPHA_BLEND_64_W( E[N2], PIXEL); \
             E[N8] =  PIXEL;\

#define DIA_3X(N8, N5, N7, nc, PIXEL)\
             ALPHA_BLEND_224_W(E[N8], PIXEL); \
             ALPHA_BLEND_32_W(E[N5], PIXEL); \
             ALPHA_BLEND_32_W(E[N7], PIXEL); \
*/

#define IN_2_SQUARES(PE, PH, PF, PG, PC, N15, N14, N11, N3, N7, N13, N12, N10, na, nb, nc, nd, PIXEL)\
                       if (PF==PG && PH==PC) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PIXEL); \
                                ALPHA_BLEND_192_W(E[N13], PIXEL); \
                                ALPHA_BLEND_128_W( E[N10], PIXEL); \
                                ALPHA_BLEND_64_W( E[N12], PIXEL); \
                                E[N14] = PIXEL; \
                                ALPHA_BLEND_192_W(E[N14], PIXEL); \
                                ALPHA_BLEND_192_W(E[N7 ], PIXEL); \
                                ALPHA_BLEND_64_W( E[N3 ], PIXEL); \
                                E[N11] = PIXEL; \
                                E[N15] = PIXEL; na++;\
                       }\
                       else if (PF==PG) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PIXEL); \
                                ALPHA_BLEND_192_W(E[N13], PIXEL); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_64_W( E[N12], PIXEL); \
                                E[N14] = PIXEL; \
                                E[N15] = PIXEL;nc++; \
                       }\
                       else if (PH==PC) \
                       {\
                                ALPHA_BLEND_192_W(E[N14], PIXEL); \
                                ALPHA_BLEND_192_W(E[N7 ], PIXEL); \
                                ALPHA_BLEND_64_W( E[N10], PIXEL); \
                                ALPHA_BLEND_64_W( E[N3 ], PIXEL); \
                                E[N11] = PIXEL; \
                                E[N15] = PIXEL; nd++;\
                       }\
                       else \
                       {\
                        ALPHA_BLEND_128_W(E[N11], PIXEL); \
                        ALPHA_BLEND_128_W(E[N14], PIXEL); \
                        E[N15] = PIXEL; \
                        na++;\
                        }\





#define FILTRODSDS(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
       if ( PH==PF && PH!=PE && PE!=PI && ( PF!=PB && PF!=PC || PH!=PD && PH!=PG )) \
       {\
           if (PH==PG && PG!=PD) \
           {\
             E[N8] = E[N7] = PF; \
             if (PG==G0 && G0!=D0)\
                     E[N6] = PF; \
             nc++;\
           }\
           else if (PF==PC && PB!=PC) \
           {\
             E[N8] = E[N5] = PF; \
             if (PC==C1 && B1!=C1)\
                     E[N2] = PF; \
             nc++;\
           }\
           else \
           {\
             E[N8] = PF; \
             nc++;\
            }\
       }\
       else if ( PH==PF && PH!=PE && ((PE==PG && (PE!=PI && (PE!=PA || PG!=D0) || PH!=PD && (PF!=I4 || PH!=I5))) \
                                   || (PE==PC && (PE!=PI && (PE!=PA || PC!=B1) || PF!=PB && (PH!=I5 || PF!=I4))) ) ) \
       {\
           if (PH==PG && PG!=PD) \
           {\
             E[N8] = E[N7] = PF; \
             if (PG==G0 && G0!=D0)\
                     E[N6] = PF; \
             nc++;\
           }\
           else if (PF==PC && PB!=PC) \
           {\
             E[N8] = E[N5] = PF; \
             if (PC==C1 && B1!=C1)\
                     E[N2] = PF; \
             nc++;\
           }\
           else \
           {\
             E[N8] = PF; \
             nc++;\
            }\
       }\
       else if (PE!=PH && PE!=PF && PF!=PI && PE==PC && PE!=PI && PF!=PB && (PD!=PH || PH==PI))\
	   {\
           if (PF==PG) \
           {\
             E[N8] = E[N7] = PF; \
             nc++;\
            }\
            else \
            {\
               E[N8] = PF; \
               nc++;\
            }\
	   }\
       else if (PE!=PF && PE!=PH && PH!=PI && PE==PG && PE!=PI && PH!=PD && (PB!=PF || PF==PI))\
	   {\
           if (PH==PC) \
           {\
             E[N8] = E[N5] = PH; \
             nc++;\
           }\
           else \
           {\
               E[N8] = PH; \
               nc++;\
            }\
	   }\
       else if (PE!=PH && PE!=PF && PE!=PI && PE==PG && PE==PC) \
       {\
             E[N8] = PF; \
             nc++;\
       }\


#define FILTRO_MELHOR_INEXATO(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
       if (eq(PE,PC) && !eq(PE,PF) && !eq(PE,PH) && !eq(PE,PI) && !eq(PF,PI) && (!eq(PF,I4) || eq(PI,PH))) \
       {\
            int e, i, p[10];\
            p[0]=eq(PB,C1); p[1]=eq(PB,PD); p[2]=eq(PH,PF); p[3]=eq(PF,C4);\
            p[4]=eq(PE,PG); p[5]=eq(PE,PC); p[6]=eq(PC,P04); p[7]=eq(PA,B1); p[8]=eq(PI,F4);\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=eq(PE,PA); p[1]=eq(PE,PI); p[2]=eq(PC,B1); p[3]=eq(PC,F4);\
            p[4]=eq(PF,I4); p[5]=eq(PB,PF); p[6]=eq(PB,A1); p[7]=eq(C1,C4); p[8]=eq(PD,PH);\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            if ((e > i)) {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PF); \
                E[N15] = PF; \
                na++;\
            }\
       }\
       else if (eq(PE,PG) && !eq(PE,PH) && !eq(PE,PF) && !eq(PE,PI) && !eq(PH,PI) && (!eq(PH,I5) || eq(PI,PF))) \
       {\
            int e, i, p[10];\
            p[0]=eq(PD,G0); p[1]=eq(PB,PD); p[2]=eq(PH,PF); p[3]=eq(PH,G5);\
            p[4]=eq(PE,PC); p[5]=eq(PE,PG); p[6]=eq(PG,P40); p[7]=eq(PA,D0); p[8]=eq(PI,H5);\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=eq(PE,PA); p[1]=eq(PE,PI); p[2]=eq(PG,D0); p[3]=eq(PG,H5);\
            p[4]=eq(PH,I5); p[5]=eq(PD,PH); p[6]=eq(PD,A0); p[7]=eq(G0,G5); p[8]=eq(PB,PF);\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            if ((e > i)) {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nb++;\
            }\
       }\
       else if ( eq(PH,PF) && PH!=PE && (df(PH,PF) <= df(PE,PI))) \
       {\
            int e, i, p[9];\
            p[0]=eq(PE,PC); p[1]=eq(PE,PG); p[2]=eq(PI,H5); p[3]=eq(PI,F4);\
            p[4]=eq(G5,PH); p[5]=eq(PH,PF); p[6]=eq(PF,C4); p[7]=eq(PB,PD); p[8]=eq(I4,I5);\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=eq(PH,PD); p[1]=eq(PH,I5); p[2]=eq(PF,I4); p[3]=eq(PF,PB);\
            p[4]=eq(PE,PA); p[5]=eq(PE,PI); p[6]=eq(PI,P44); p[7]=eq(PC,F4); p[8]=eq(PG,H5);\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            if ((e > i)) {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PF; \
                nc++;\
            }\
            else if ((e >= i) && (PH!=PI || PE!=PF)) {\
       ALPHA_BLEND_192_W(E[N10], PE); \
       ALPHA_BLEND_32_W(E[N10], PF); \
       ALPHA_BLEND_32_W(E[N10], PH); \
       ALPHA_BLEND_128_W(E[N11], PE); \
       ALPHA_BLEND_96_W(E[N11], PF); \
       ALPHA_BLEND_32_W(E[N11], PH); \
       ALPHA_BLEND_128_W(E[N14], PE); \
       ALPHA_BLEND_32_W(E[N14], PF); \
       ALPHA_BLEND_96_W(E[N14], PH); \
       ALPHA_BLEND_96_W(E[N15], PE); \
       ALPHA_BLEND_64_W(E[N15], PF); \
       ALPHA_BLEND_64_W(E[N15], PH); \
       ALPHA_BLEND_32_W(E[N15], PI); \
       nd++;\
            }\
       }\

#define FILTRO_ANTES_DAS_FERIAS(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            int e, i, p[10];\
            int ke, ki, me, mi;\
            \
            p[0]=PE==PC; p[1]=PE==PG; p[2]=PI==H5; p[3]=PI==F4;\
            p[4]=G5==PH; p[5]=PH==PF; p[6]=PF==C4; p[7]=PB==PD; p[8]=I4==I5;\
            e = 3*(p[0]+p[1]+p[2]+p[3])+1*(p[4]+p[6])+p[7]+p[8]+6*p[5] + 2*p[5]*(p[4]+p[6]);\
            p[0]=PH==PD; p[1]=PH==I5; p[2]=PF==I4; p[3]=PF==PB;\
            p[4]=PE==PA; p[5]=PE==PI; p[6]=PI==P44; p[7]=PC==F4; p[8]=PG==H5;\
            i = 3*(p[0]+p[1]+p[2]+p[3])+1*(p[4]+p[6])+p[7]+p[8]+6*p[5] + 2*p[5]*(p[4]+p[6]);\
       if (PH==PF && PE!=PH && (e > i))\
       {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PF; \
                na++;\
       }\
}\



#define FILTROSDFFDS(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            int e, i, p[10];\
            int ke, ki, me, mi;\
            \
            p[0]=PE==PC; p[1]=PE==PG; p[2]=PI==H5; p[3]=PI==F4;\
            p[4]=G5==PH; p[5]=PH==PF; p[6]=PF==C4; p[7]=PB==PD; p[8]=I4==I5;\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=PH==PD; p[1]=PH==I5; p[2]=PF==I4; p[3]=PF==PB;\
            p[4]=PE==PA; p[5]=PE==PI; p[6]=PI==P44; p[7]=PC==F4; p[8]=PG==H5;\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            \
            p[0]=PB==C1; p[1]=PB==PD; p[2]=PH==PF; p[3]=PF==C4;\
            p[4]=PE==PG; p[5]=PE==PC; p[6]=PC==P04; p[7]=PA==B1; p[8]=PI==F4;\
            ke = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=PE==PA; p[1]=PE==PI; p[2]=PC==B1; p[3]=PC==F4;\
            p[4]=PF==I4; p[5]=PB==PF; p[6]=PB==A1; p[7]=C1==C4; p[8]=PD==PH;\
            ki = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            \
            p[0]=PD==G0; p[1]=PB==PD; p[2]=PH==PF; p[3]=PH==G5;\
            p[4]=PE==PC; p[5]=PE==PG; p[6]=PG==P40; p[7]=PA==D0; p[8]=PI==H5;\
            me = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=PE==PA; p[1]=PE==PI; p[2]=PG==D0; p[3]=PG==H5;\
            p[4]=PH==I5; p[5]=PD==PH; p[6]=PD==A0; p[7]=G0==G5; p[8]=PB==PF;\
            mi = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
       if (PH==PF && PE!=PH && (e > i))\
       {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PF; \
                na++;\
       }\
       else if ((ke >= ki) && (e>i) && PE==PC && (PE!=PH && PE!=PF && PE!=PI && (PF!=PI || PE!=PG || PH==I5))) \
       {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PF); \
                E[N15] = PF; \
                nb++;\
       }\
       else if ((me >= mi) && (e>i) && PE==PG && (PE!=PH && PE!=PF && PE!=PI && (PH!=PI || PE!=PC || PF==I4))) \
       {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nc++;\
       }\
}\


/*
       else if (PE==PG && PH!=PG) \
       {\
            int e, i, p[10];\
            p[0]=PD==G0; p[1]=PB==PD; p[2]=PH==PF; p[3]=PH==G5;\
            p[4]=PE==PC; p[5]=PE==PG; p[6]=PG==P40; p[7]=PA==D0; p[8]=PI==H5;\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=PE==PA; p[1]=PE==PI; p[2]=PG==D0; p[3]=PG==H5;\
            p[4]=PH==I5; p[5]=PD==PH; p[6]=PD==A0; p[7]=G0==G5; p[8]=PB==PF;\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            if ((e > i)) {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nd++;\
            }\
       }\


Curioso resultado:
    
           else if (PE==PC && PF!=PC && (ke >= ki) && ((e > i) && PH==PI) || PF==PI) \
       {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PF); \
                E[N15] = PF; \
                nb++;\
       }\
       else if (PE==PG && PH!=PG && (me >= mi) && ((e > i) && PF==PI) || PH==PI) \
       {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nc++;\
       }\


       else if ((ke >= ki) && PE==PC && PF!=PC) \
       {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PF); \
                E[N15] = PF; \
                nb++;\
       }\
       else if ((me >= mi) && PE==PG && PH!=PG) \
       {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nc++;\
       }\



       else if ((ke>=ki) && PE==PC && PE!=PI && PF!=I4 && (PE!=PH && (PH!=PI || PE!=PF) || PE!=PF && (PF!=PI || PE!=PH))) \
       {\
               if (PF==PI) \
               {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nc++;\
               }\
               else \
               {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PF); \
                E[N15] = PF; \
                nb++;\
               }\
       }\
       else if ((me>=mi) && PE==PG && PE!=PI && PH!=I5 && (PE!=PH && (PH!=PI || PE!=PF) || PE!=PF && (PF!=PI || PE!=PH))) \
       {\
               if (PH==PI) \
               {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PF); \
                E[N15] = PF; \
                nb++;\
               }\
               else \
               {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nc++;\
               }\
       }\

*/


#define FILTRO_EXP(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, ne, nf, nt) \
      if (PH==PF && PE!=PH) {\
            int e, i, k1, k2, k3, k4, k5, p[10], q[4];\
            q[0]=PD==PG && PE==PD; q[1]=PD==PA && PE==PA; q[2]=PA==PB && PE==PB; q[3]=PB==PC && PE==PC;\
            k1 = q[0]+q[1]+q[2]+q[3];\
            q[0]=H5==I5 && PH==H5; q[1]=H5==G5 && PH==G5; q[2]=G5==PG && PH==PG; q[3]=PG==PD && PH==PD;\
            k2 = q[0]+q[1]+q[2]+q[3];\
            q[0]=I4==F4 && PI==I4; q[1]=I4==P44&& PI==P44; q[2]=P44==I5 && PI==I5; q[3]=I5==H5 && PI==H5;\
            k3 = q[0]+q[1]+q[2]+q[3];\
            q[0]=PC==PB && PF==PC; q[1]=PC==C4 && PF==C4; q[2]=C4==F4 && PF==F4; q[3]=F4==I4 && PF==I4;\
            k4 = q[0]+q[1]+q[2]+q[3];\
            k5 = (PH==PF && PE==PI);\
            p[0]=PE==PC; p[1]=PE==PG; p[2]=PI==H5; p[3]=PI==F4;\
            p[4]=G5==PH; p[5]=PH==PF; p[6]=PF==C4; p[7]=PB==PD; p[8]=I4==I5;\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5]  +  6*(k1*k3)*k5;\
            p[0]=PH==PD; p[1]=PH==I5; p[2]=PF==I4; p[3]=PF==PB;\
            p[4]=PE==PA; p[5]=PE==PI; p[6]=PI==P44; p[7]=PC==F4; p[8]=PG==H5; p[9]=(PF==PB || PF==PC) && (PH==PD || PH==PG);\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5]  +  6*(k2*k4)*k5;\
            if ((e > i) && ( PF!=PB && PF!=PC || PH!=PD && PH!=PG || PE==PG || PE==PC || PH!=H5 && PH!=I5 || PF!=F4 && PF!=I4)) {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PF; \
                na++;\
            }\
            else if ((e >= i)) {\
                ALPHA_BLEND_128_W(E[N15], PF); \
                na++;\
            }\
      }\
       else if (PE==PC && PE!=PF && PE!=PH && PE!=PI && PF!=PI && (PF!=I4 || PI==PH)) \
       {\
            int e, i, p[10];\
            p[0]=PB==C1; p[1]=PB==PD; p[2]=PH==PF; p[3]=PF==C4;\
            p[4]=PE==PG; p[5]=PE==PC; p[6]=PC==P04; p[7]=PA==B1; p[8]=PI==F4;\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=PE==PA; p[1]=PE==PI; p[2]=PC==B1; p[3]=PC==F4;\
            p[4]=PF==I4; p[5]=PB==PF; p[6]=PB==A1; p[7]=C1==C4; p[8]=PD==PH;\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            if ((e > i)) {\
                ALPHA_BLEND_128_W(E[N11], PF); \
                ALPHA_BLEND_128_W(E[N14], PF); \
                E[N15] = PF; \
                nb++;\
            }\
       }\
       else if (PE==PG && PE!=PH && PE!=PF && PE!=PI && PH!=PI && (PH!=I5 || PI==PF)) \
       {\
            int e, i, p[10];\
            p[0]=PD==G0; p[1]=PB==PD; p[2]=PH==PF; p[3]=PH==G5;\
            p[4]=PE==PC; p[5]=PE==PG; p[6]=PG==P40; p[7]=PA==D0; p[8]=PI==H5;\
            e = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            p[0]=PE==PA; p[1]=PE==PI; p[2]=PG==D0; p[3]=PG==H5;\
            p[4]=PH==I5; p[5]=PD==PH; p[6]=PD==A0; p[7]=G0==G5; p[8]=PB==PF;\
            i = 3*(p[0]+p[1]+p[2]+p[3])+2*(p[4]+p[6])+p[7]+p[8]+6*p[5];\
            if ((e > i)) {\
                ALPHA_BLEND_128_W(E[N11], PH); \
                ALPHA_BLEND_128_W(E[N14], PH); \
                E[N15] = PH; \
                nc++;\
            }\
       }\


#define FILTRO_BYUU(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            int e, i, p[10];\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(G5,PH); p[5]=df(PH,PF); p[6]=df(PF,C4); p[7]=df(PB,PD); p[8]=df(I4,I5);\
            e = (p[0]+p[1]+p[2]+p[3])+4*p[5];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PE,PA); p[5]=df(PE,PI); p[6]=df(PI,P44); p[7]=df(PC,F4); p[8]=df(PG,H5);\
            i = (p[0]+p[1]+p[2]+p[3])+4*p[5];\
       if ((!eq(PE,PH) && !eq(PE,PF)) && (e<i) && ( !eq(PF,PB) && !eq(PF,PC) || !eq(PH,PD) && !eq(PH,PG) || eq(PE,PI) && (!eq(PF,F4) && !eq(PF,I4) || !eq(PH,H5) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
                       if ( (df(PE,PF) <= df(PE,PH)) ) \
                       {\
                       if ( !eq(PG,PD) && (eq(PH,PG) || eq(PF,PG)) && !eq(PB,PC) && (eq(PF,PC) || eq(PH,PC)) ) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PF); \
                                ALPHA_BLEND_192_W(E[N13], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N12], PF); \
                                E[N14] = PF; \
                                ALPHA_BLEND_192_W(E[N14], PF); \
                                ALPHA_BLEND_192_W(E[N7 ], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N3 ], PF); \
                                E[N11] = PF; \
                                E[N15] = PF; na++;\
                       }\
                       else if (!eq(PG,PD) && (eq(PH,PG) || eq(PF,PG)) ) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PF); \
                                ALPHA_BLEND_192_W(E[N13], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N12], PF); \
                                E[N14] = PF; \
                                E[N15] = PF;na++; \
                       }\
                       else if (!eq(PB,PC) && (eq(PF,PC) || eq(PH,PC)) ) \
                       {\
                                ALPHA_BLEND_192_W(E[N14], PF); \
                                ALPHA_BLEND_192_W(E[N7 ], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N3 ], PF); \
                                E[N11] = PF; \
                                E[N15] = PF; na++;\
                       }\
                       else \
                       {\
                        ALPHA_BLEND_128_W(E[N11], PF); \
                        ALPHA_BLEND_128_W(E[N14], PF); \
                        E[N15] = PF; \
                        na++;\
                        }\
                       }\
                       else \
                       {\
                       if ( !eq(PG,PD) && (eq(PH,PG) || eq(PF,PG)) && !eq(PB,PC) && (eq(PF,PC) || eq(PH,PC)) ) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PH); \
                                ALPHA_BLEND_192_W(E[N13], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N12], PH); \
                                E[N14] = PH; \
                                ALPHA_BLEND_192_W(E[N14], PH); \
                                ALPHA_BLEND_192_W(E[N7 ], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N3 ], PH); \
                                E[N11] = PH; \
                                E[N15] = PH; na++;\
                       }\
                       else if (!eq(PG,PD) && (eq(PH,PG) || eq(PF,PG)) ) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PH); \
                                ALPHA_BLEND_192_W(E[N13], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N12], PH); \
                                E[N14] = PH; \
                                E[N15] = PH;na++; \
                       }\
                       else if (!eq(PB,PC) && (eq(PF,PC) || eq(PH,PC)) ) \
                       {\
                                ALPHA_BLEND_192_W(E[N14], PH); \
                                ALPHA_BLEND_192_W(E[N7 ], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N3 ], PH); \
                                E[N11] = PH; \
                                E[N15] = PH; na++;\
                       }\
                       else \
                       {\
                        ALPHA_BLEND_128_W(E[N11], PH); \
                        ALPHA_BLEND_128_W(E[N14], PH); \
                        E[N15] = PH; \
                        na++;\
                        }\
                       }\
       }\
       else if ((PE!=PH && PE!=PF) && (e<i) && ( !eq(PF,PB) && !eq(PF,PC) || !eq(PH,PD) && !eq(PH,PG) || eq(PE,PI) && (!eq(PF,F4) && !eq(PF,I4) || !eq(PH,H5) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
            if ( (df(PE,PF) <= df(PE,PH)) ) \
            {\
               IN_2_SQUARES(PE, PH, PF, PG, PC, N15, N14, N11, N3, N7, N13, N12, N10, na, nb, nc, nd, PF);\
            }\
            else \
            {\
               IN_2_SQUARES(PE, PH, PF, PG, PC, N15, N14, N11, N3, N7, N13, N12, N10, na, nb, nc, nd, PH);\
            }\
       }\
       else if (PE!=PH && PE!=PF && (e<=i)) {\
                if ( df(PE,PF) <= df(PE,PH) ) \
                {ALPHA_BLEND_128_W(E[N15], PF); \
                nd++;}\
                else {\
                ALPHA_BLEND_128_W(E[N15], PH); \
                nd++;}\
       }\
}\



#define FILTRO_BYUU2(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            int e, i, p[10];\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(G5,PH); p[5]=df(PH,PF); p[6]=df(PF,C4); p[7]=df(PB,PD); p[8]=df(I4,I5);\
            e = (p[0]+p[1]+p[2]+p[3])+4*p[5];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PE,PA); p[5]=df(PE,PI); p[6]=df(PI,P44); p[7]=df(PC,F4); p[8]=df(PG,H5);\
            i = (p[0]+p[1]+p[2]+p[3])+4*p[5];\
       if ((e<i) && ( !eq(PF,PB) && !eq(PF,PC) || !eq(PH,PD) && !eq(PH,PG) || eq(PE,PI) && (!eq(PF,F4) && !eq(PF,I4) || !eq(PH,H5) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
                       if ( (df(PE,PF) <= df(PE,PH)) ) \
                       {\
                       if ( (!eq(PE,PH) && !eq(PE,PF)) && (eq(PF,PG) || eq(PH,PG)) && !eq(PG,PD) && (eq(PH,PC) || eq(PF,PC)) && !eq(PB,PC) || (PE!=PH && PE!=PF) && PF==PG && PH==PC ) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PF); \
                                ALPHA_BLEND_192_W(E[N13], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N12], PF); \
                                E[N14] = PF; \
                                ALPHA_BLEND_192_W(E[N14], PF); \
                                ALPHA_BLEND_192_W(E[N7 ], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N3 ], PF); \
                                E[N11] = PF; \
                                E[N15] = PF; na++;\
                       }\
                       else if ( (!eq(PE,PH) && !eq(PE,PF)) && (eq(PF,PG) || eq(PH,PG)) && !eq(PG,PD)  || (PE!=PH && PE!=PF) && PF==PG) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PF); \
                                ALPHA_BLEND_192_W(E[N13], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N12], PF); \
                                E[N14] = PF; \
                                E[N15] = PF;na++; \
                       }\
                       else if ( (!eq(PE,PH) && !eq(PE,PF)) && (eq(PH,PC) || eq(PF,PC)) && !eq(PB,PC)  || (PE!=PH && PE!=PF) && PH==PC ) \
                       {\
                                ALPHA_BLEND_192_W(E[N14], PF); \
                                ALPHA_BLEND_192_W(E[N7 ], PF); \
                                ALPHA_BLEND_64_W( E[N10], PF); \
                                ALPHA_BLEND_64_W( E[N3 ], PF); \
                                E[N11] = PF; \
                                E[N15] = PF; na++;\
                       }\
                       else \
                       {\
                        ALPHA_BLEND_128_W(E[N11], PF); \
                        ALPHA_BLEND_128_W(E[N14], PF); \
                        E[N15] = PF; \
                        na++;\
                        }\
                       }\
                       else \
                       {\
                       if ( (!eq(PE,PH) && !eq(PE,PF)) && (eq(PF,PG) || eq(PH,PG)) && !eq(PG,PD) && (eq(PH,PC) || eq(PF,PC)) && !eq(PB,PC) || (PE!=PH && PE!=PF) && PF==PG && PH==PC ) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PH); \
                                ALPHA_BLEND_192_W(E[N13], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N12], PH); \
                                E[N14] = PH; \
                                ALPHA_BLEND_192_W(E[N14], PH); \
                                ALPHA_BLEND_192_W(E[N7 ], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N3 ], PH); \
                                E[N11] = PH; \
                                E[N15] = PH; na++;\
                       }\
                       else if ( (!eq(PE,PH) && !eq(PE,PF)) && (eq(PF,PG) || eq(PH,PG)) && !eq(PG,PD)  || (PE!=PH && PE!=PF) && PF==PG) \
                       {\
                                ALPHA_BLEND_192_W(E[N11], PH); \
                                ALPHA_BLEND_192_W(E[N13], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N12], PH); \
                                E[N14] = PH; \
                                E[N15] = PH;na++; \
                       }\
                       else if ( (!eq(PE,PH) && !eq(PE,PF)) && (eq(PH,PC) || eq(PF,PC)) && !eq(PB,PC)  || (PE!=PH && PE!=PF) && PH==PC ) \
                       {\
                                ALPHA_BLEND_192_W(E[N14], PH); \
                                ALPHA_BLEND_192_W(E[N7 ], PH); \
                                ALPHA_BLEND_64_W( E[N10], PH); \
                                ALPHA_BLEND_64_W( E[N3 ], PH); \
                                E[N11] = PH; \
                                E[N15] = PH; na++;\
                       }\
                       else \
                       {\
                        ALPHA_BLEND_128_W(E[N11], PH); \
                        ALPHA_BLEND_128_W(E[N14], PH); \
                        E[N15] = PH; \
                        na++;\
                        }\
                       }\
       }\
       else if (PE!=PH && PE!=PF && (e<=i)) {\
                if ( df(PE,PF) <= df(PE,PH) ) \
                {ALPHA_BLEND_128_W(E[N15], PF); \
                nd++;}\
                else {\
                ALPHA_BLEND_128_W(E[N15], PH); \
                nd++;}\
       }\
}\



#define FILTRO_BYUU3(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            int e, i, p[10]; unsigned int px;\
            int esq, cima, esq2, cima2, ex, inex;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(G5,PH); p[5]=df(PH,PF); p[6]=df(PF,C4); p[7]=df(PB,PD); p[8]=df(I4,I5);\
            e = (p[0]+p[1]+p[2]+p[3])+4*p[5];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PE,PA); p[5]=df(PE,PI); p[6]=df(PI,P44); p[7]=df(PC,F4); p[8]=df(PG,H5);\
            i    = (p[0]+p[1]+p[2]+p[3])+4*p[5];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            esq2  = ((eq(PF,PG) || eq(PH,PG)) && PG!=PD); \
            cima2 = ((eq(PH,PC) || eq(PF,PC)) && PB!=PC); \
            esq  = ((eq(PF,PG) || eq(PH,PG)) && !eq(PG,PD)); \
            cima = ((eq(PH,PC) || eq(PF,PC)) && !eq(PB,PC)); \
            ex   = (PE!=PH && PE!=PF); \
            inex = (!eq(PE,PH) && !eq(PE,PF)); \
       if ((e<i) && ( !eq(PF,PB) && !eq(PF,PC) || !eq(PH,PD) && !eq(PH,PG) || eq(PE,PI) && (!eq(PF,F4) && !eq(PF,I4) || !eq(PH,H5) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
                if ( inex && esq && cima || ex && PF==PG && PH==PC ) \
                {\
                        LEFT_2(N15, N14, N11, N13, N12, N10, nc, px);\
                        UP_2(N15, N14, N11, N3, N7, N10, nc, px);\
                }\
                else if ( inex && esq || ex && PF==PG) \
                {\
                        LEFT_2(N15, N14, N11, N13, N12, N10, nc, px);\
                }\
                else if ( inex && cima || ex && PH==PC) \
                {\
                        UP_2(N15, N14, N11, N3, N7, N10, nc, px);\
                }\
                else \
                {\
                        DIA(N15, N14, N11, nd, px);\
                }\
       }\
       else if ( ex && (e<=i) )\
       {\
                ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
}\


#define FILTRO_BYUU4(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, p[10], wp=4.0; unsigned int px;\
            int ex, ex2, ex3;\
            float ke, ki, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ke = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ki = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); cf = 2.0; cfe = 1.0;\
       if ( ex && ((cfe*e)<i)  && ( !eq(PF,PB) && !eq(PF,PC) || !eq(PH,PD) && !eq(PH,PG) || eq(PE,PI) && (!eq(PF,F4) && !eq(PF,I4) || !eq(PH,H5) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
            if ( ((cf*ke)<=ki) && ex3 || (ke>=(cf*ki)) && ex2 ) \
            {\
                if ( ((cf*ke)<=ki) && ex3 ) \
                {\
                        LEFT_2(N15, N14, N11, N13, N12, N10, nc, px);\
                }\
                if ( (ke>=(cf*ki)) && ex2 ) \
                {\
                        UP_2(N15, N14, N11, N3, N7, N10, nc, px);\
                }\
            }\
            else \
            {\
                  DIA(N15, N14, N11, nd, px);\
            }\
       }\
       else if ( ((cfe*e)<=i) )\
       {\
                ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
}\


#define FILTRO(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            int e, i, p[10]; unsigned int px;\
            int ex, ex2, ex3;\
            float ke, ki, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+4*p[5]; ke = 0*(p[6]+p[7]+p[8]+p[9])+4*p[4];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+4*p[5]; ki = 0*(p[6]+p[7]+p[8]+p[9])+4*p[4];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); cf = 2.0; cfe = 1.0;\
       if ( ex && ((cfe*e)<i) && ( !eq(PF,PB) && !eq(PH,PD) || eq(PE,PI) && (!eq(PF,I4) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
            if ( ((cf*ke)<=ki) && ex3 && (ke>=(cf*ki)) && ex2 ) \
            {\
                   LEFT_UP_2_3X(N7, N5, N6, N2, N8, nc, px);\
            }\
            else if ( ((cf*ke)<=ki) && ex3 ) \
            {\
                   LEFT_2_3X(N7, N5, N6, N8, nc, px);\
            }\
            else if ( (ke>=(cf*ki)) && ex2 ) \
            {\
                   UP_2_3X(N5, N7, N2, N8, nc, px);\
            }\
            else \
            {\
                   DIA_3X(N8, N5, N7, nc, px);\
            }\
       }\
}\

/*
       else if (ex && ((cfe*e)<=i))\
       {\
             ALPHA_BLEND_128_W( E[N8], px); \
       }\
*/

#define FILTRO_L3(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, ef3, if3, eh3, ih3, p[10], wp=4.0; unsigned int px;\
            int ex, ex2, ex3, ex4, exf3, exf3l, exh3, exh3u;\
            float ke, ki, je, ji, kef3, kif3, keh3, kih3, jef3, jif3, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PF,C4); p[7]=df(PH,F4); p[8]=df(PF,G0); p[9]=df(PG,F4);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ke = p[4]; je = p[8]; ef3 = p[5]+p[6]+2*p[3]; kef3 = p[7]; jef3 = p[9];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,PB); p[3]=df(PF,I4);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PC,F4); p[7]=df(PI,C4); p[8]=df(PH,C1); p[9]=df(PI,P04);\
            i = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ki = p[4]; ji = p[8]; if3 = p[5]+p[6]+2*p[3]; kif3 = p[7]; jif3 = p[9];\
            px  = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex  = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); ex4 = (PD!=G0 && D0!=G0); cf = 2.0; cfe = 1.0;\
            exf3 = (PF!=PI && PF!=F4); exf3l = (PF!=PH && PE!=PH);\
       if ( ex3 && ((cf*jef3)<=jif3) && exf3l && ((cf*kef3)<=kif3) && exf3 && (ef3<if3) ) \
       {\
               LEFT_3R(N15, N14, nb, px);\
       }\
       else if ( ex4 && ((cf*je)<=ji) && ex3 && ((cf*ke)<=ki) && ex && (e<i) ) \
       {\
               LEFT_3(N15, N14, N11, N13, N12, N10, N9, nc, px);\
       }\
       else if ( ex3 && ((cf*ke)<=ki) && ex2 && ((cf*ki)<=ke) && ex && (e<i) ) \
       {\
               LEFT_2(N15, N14, N11, N13, N12, N10, nc, px);\
               UP_2(N15, N14, N11, N3, N7, N10, nc, px);\
       }\
       else if ( ex3 && ((cf*ke)<=ki) && ex && (e<i) ) \
       {\
               LEFT_2(N15, N14, N11, N13, N12, N10, nc, px);\
       }\
       else if ( ex2 && ((cf*ki)<=ke) && ex && (e<i) ) \
       {\
               UP_2(N15, N14, N11, N3, N7, N10, nc, px);\
       }\
       else if ( ex && ((cfe*e)<i) )\
       {\
                  DIA(N15, N14, N11, nd, px); nc++;\
       }\
       else if ( ex && (e<=i) )\
       {\
                ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
}\


#define FILTRO_TEST(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, p[10], wp=4.0; unsigned int px;\
            int ex, ex2, ex3;\
            float ke, ki, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ke = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ki = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); cf = 2.0; cfe = 1.0;\
       if ( ex && ((cfe*e)<i))\
       {\
            if ( ((cf*ke)<=ki) && ex3 || (ke>=(cf*ki)) && ex2 ) \
            {\
                px   = ((df(PE,PF) + df(PE,PC) + 2*df(PH,PI) + df(PF,PG)) <= (df(PE,PH) +  df(PE,PG) + 2*df(PF,PI)) + df(PH,PC)) ? PF : PH; \
                if ( ((cf*ke)<=ki) && ex3 ) \
                {\
                        LEFT_2(N15, N14, N11, N13, N12, N10, nc, px);\
                }\
                if ( (ke>=(cf*ki)) && ex2 ) \
                {\
                        UP_2(N15, N14, N11, N3, N7, N10, nc, px);\
                }\
            }\
            else \
            {\
                  DIA(N15, N14, N11, nd, px);\
            }\
       }\
       else if ( ((cfe*e)<=i) )\
       {\
                ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
}\

#define FILTRO_LV1(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, p[10], wp=4.0; unsigned int px;\
            int ex;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); \
       if ( ex && (e<i)  && ( !eq(PF,PB) && !eq(PF,PC) || !eq(PH,PD) && !eq(PH,PG) || eq(PE,PI) && (!eq(PF,F4) && !eq(PF,I4) || !eq(PH,H5) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
               DIA(N15, N14, N11, nd, px);\
       }\
       else if ( ex && (e<=i) )\
       {\
               ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
}\



#define FILTROL(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, p[10], wp=4.0; unsigned int px;\
            int ex, ex2, ex3;\
            float ke, ki, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ke = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ki = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); cf = 2.0; cfe = 1.0;\
       if ( ex && ((cfe*e)<i)  && ( !eq(PF,PB) && !eq(PH,PD) || eq(PE,PI) && (!eq(PF,I4) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
            if ( ((cf*ke)<=ki) && ex3 || (ke>=(cf*ki)) && ex2 ) \
            {\
                if ( ((cf*ke)<=ki) && ex3 ) \
                {\
                        LEFT_2L(N15, N14, N11, N13, N12, N10, nc, px);\
                }\
                if ( (ke>=(cf*ki)) && ex2 ) \
                {\
                        UP_2L(N15, N14, N11, N3, N7, N10, nc, px);\
                }\
            }\
            else \
            {\
                  DIA_L(N15, N14, N11, nd, px);\
            }\
       }\
       else if ( ex && ((cfe*e)<=i) )\
       {\
                ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
       else \
       {\
                ALPHA_BLEND_64_W(E[N15], 0);\
                E[N12] = E[N13] = E[N14] = E[N15];\
       }\
}

#define FILTROR(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, p[10], wp=4.0; unsigned int px;\
            int ex, ex2, ex3;\
            float ke, ki, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ke = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ki = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); cf = 2.0; cfe = 1.0;\
       if ( ex && ((cfe*e)<i)  && ( !eq(PF,PB) && !eq(PH,PD) || eq(PE,PI) && (!eq(PF,I4) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
            if ( ((cf*ke)<=ki) && ex3 || (ke>=(cf*ki)) && ex2 ) \
            {\
                if ( ((cf*ke)<=ki) && ex3 ) \
                {\
                        LEFT_2R(N15, N14, N11, N13, N12, N10, nc, px);\
                }\
                if ( (ke>=(cf*ki)) && ex2 ) \
                {\
                        UP_2R(N15, N14, N11, N3, N7, N10, nc, px);\
                }\
            }\
            else \
            {\
                  DIA_R(N15, N14, N11, nd, px);\
            }\
       }\
       else if ( ex && ((cfe*e)<=i) )\
       {\
                ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
       else \
       {\
                ALPHA_BLEND_64_W(E[N15], 0);\
                E[N11] = E[N3] = E[N7] = E[N15];\
       }\
}


#define FILTROLLL(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, p[10], wp=4.0; unsigned int px;\
            int ex, ex2, ex3;\
            float ke, ki, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ke = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ki = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); cf = 2.0; cfe = 1.0;\
       if ( ex && ((cfe*e)<i)  && ( !eq(PF,PB) && !eq(PH,PD) || eq(PE,PI) && (!eq(PF,I4) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
            if ( ((cf*ke)<=ki) && ex3 || (ke>=(cf*ki)) && ex2 ) \
            {\
                if ( ((cf*ke)<=ki) && ex3 ) \
                {\
                        LEFT_2(N15, N14, N11, N13, N12, N10, nc, px);\
                }\
                if ( (ke>=(cf*ki)) && ex2 ) \
                {\
                        UP_2(N15, N14, N11, N3, N7, N10, nc, px);\
                }\
            }\
            else \
            {\
                  DIA(N15, N14, N11, nd, px);\
            }\
       }\
       else if ( ex && ((cfe*e)<=i) )\
       {\
                ALPHA_BLEND_128_W(E[N15], px); nd++;\
       }\
}


#define FILTRO_2X(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, N15, N14, N11, N3, N7, N10, N13, N12, N9, N6, N2, N1, N5, N8, N4, N0, na, nb, nc, nd, nt) \
{\
            float e, i, p[10], wp=4.0; unsigned int px;\
            int ex, ex2, ex3;\
            float ke, ki, cf, cfe;\
            \
            p[0]=df(PE,PC); p[1]=df(PE,PG); p[2]=df(PI,H5); p[3]=df(PI,F4);\
            p[4]=df(PF,PG); p[5]=df(PH,PF); p[6]=df(PE,C4); p[7]=df(PE,G0); p[8]=df(PH,F4); p[9]=df(PH,P40);\
            e = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ke = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            p[0]=df(PH,PD); p[1]=df(PH,I5); p[2]=df(PF,I4); p[3]=df(PF,PB);\
            p[4]=df(PH,PC); p[5]=df(PE,PI); p[6]=df(PE,C1); p[7]=df(PE,G5); p[8]=df(PF,H5); p[9]=df(PF,P04);\
            i    = 1*(p[0]+p[1]+p[2]+p[3])+wp*p[5]; ki = 0*(p[6]+p[7]+p[8]+p[9])+wp*p[4];\
            px   = (df(PE,PF) <= df(PE,PH)) ? PF : PH; \
            ex   = (PE!=PH && PE!=PF); ex2 = (PE!=PC && PB!=PC); ex3 = (PE!=PG && PD!=PG); cf = 2.0; cfe = 1.0;\
       if ( ex && ((cfe*e)<i)  && ( !eq(PF,PB) && !eq(PH,PD) || eq(PE,PI) && (!eq(PF,I4) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC)) )\
       {\
            if ( ((cf*ke)<=ki) && ex3 || (ke>=(cf*ki)) && ex2 ) \
            {\
                if ( ((cf*ke)<=ki) && ex3 ) \
                {\
                        LEFT_2X(N3, N2, nc, px);\
                }\
                if ( (ke>=(cf*ki)) && ex2 ) \
                {\
                        UP_2X(N3, N1, nc, px);\
                }\
            }\
            else \
            {\
                  DIA_2X(N3, nd, px);\
            }\
       }\
       else if ( ex && ((cfe*e)<=i) )\
       {\
                ALPHA_BLEND_64_W(E[N3], px); nd++;\
       }\
}


int eq(unsigned int A, unsigned int B)
{
    unsigned int r, g, b;
    unsigned int y, u, v;
   
    b = abs(((A & pg_blue_mask  )>>16) - ((B & pg_blue_mask  )>> 16));
    g = abs(((A & pg_green_mask)>>8  ) - ((B & pg_green_mask )>>  8));
    r = abs(( A & pg_red_mask        ) - ( B & pg_red_mask         ));
   
    y = abs(0.299*r + 0.587*g + 0.114*b);
    u = abs(-0.169*r - 0.331*g + 0.500*b);
    v = abs(0.500*r - 0.419*g - 0.081*b);

    return ((48 >= y) && (7 >= u) && (6 >= v)) ? 1 : 0;
}


float df(unsigned int A, unsigned int B)
{
    unsigned int r, g, b;
    unsigned int y, u, v;
   
    b = abs(((A & pg_blue_mask  )>>16) - ((B & pg_blue_mask  )>> 16));
    g = abs(((A & pg_green_mask)>>8  ) - ((B & pg_green_mask )>>  8));
    r = abs(( A & pg_red_mask        ) - ( B & pg_red_mask         ));
   
    y = abs(0.299*r + 0.587*g + 0.114*b);
    u = abs(-0.169*r - 0.331*g + 0.500*b);
    v = abs(0.500*r - 0.419*g - 0.081*b);

    return 48*y + 7*u + 6*v;
}




int eq2(unsigned int A, unsigned int B)
{
    return (df(A, B) < 155.0) ? 1 : 0;
}


int eq1(unsigned int A, unsigned int B)
{
    unsigned int r, g, b;
    unsigned int y, u, v;
   
    b = abs(((A & pg_blue_mask  )>>16) - ((B & pg_blue_mask  )>> 16));
    g = abs(((A & pg_green_mask)>>8  ) - ((B & pg_green_mask )>>  8));
    r = abs(( A & pg_red_mask        ) - ( B & pg_red_mask         ));

//    y = abs((r<<3) + (g<<4) + (b<<2));
//    u = abs(-(r<<2) - (g<<2) + (b<<3));
//    v = abs((r<<3) - (g<<3) - (b<<1));

    y = abs((r<<4) + (g<<5) + (b<<3));
    u = abs(-(r<<2) - (g<<2) + (b<<3));
    v = abs((r<<1) - (g<<1) - (b>>1));

   
//    y = abs((r<<4) + (g<<5) + (b<<2));
//    u = abs(-r - (g<<1) + (b<<2));
//    v = abs((r<<1) - (g<<1) - (b>>1));

    return ((5200 >= y) && (98 >= u) && (72 >= v)) ? 1 : 0;
}

float df1(unsigned int A, unsigned int B)
{
    unsigned int r, g, b;
    unsigned int y, u, v;
   
    b = abs(((A & pg_blue_mask  )>>16) - ((B & pg_blue_mask  )>> 16));
    g = abs(((A & pg_green_mask)>>8  ) - ((B & pg_green_mask )>>  8));
    r = abs(( A & pg_red_mask        ) - ( B & pg_red_mask         ));

    y = abs((r<<4) + (g<<5) + (b<<3));
    u = abs(-(r<<2) - (g<<2) + (b<<3));
    v = abs((r<<1) - (g<<1) - (b>>1));

//    y = abs((r<<4) + (g<<5) + (b<<2));
//    u = abs(-r - (g<<1) + (b<<2));
//    v = abs((r<<1) - (g<<1) - (b>>1));


//    return (y << 16) + (u << 8) + v;
    return y + u + v;
}

//    y = abs((r*16) + (g*32) + (b*8));
//    u = abs(-(r*4) - (g*4) + (b*8));
//    v = abs((r*2) - (g*2) - (b*0.5));

// && ( !eq(PF,PB) && !eq(PH,PD) || eq(PE,PI) && (!eq(PF,I4) && !eq(PH,I5)) || eq(PE,PG) || eq(PE,PC))
// && ( PF!=PB && PH!=PD || PE==PI && (PF!=I4 && PH!=I5) || PE==PG || PE==PC)

//        y = abs((r*16) + (g*32) + (b*4));
//        u = abs((-(r*1) - (g*2) + (b*4)));
//        v = abs(((r*2) - (g*2) - (b>>1)));


// 14.352, 28.176, 5.472
// -1.183, -2.317, 3.5, 
//  3.0,   -2.514 -0.486


// r = numero de regioes, n = numero de pixels vizinhos iguais
void ambi(int *pix, int *r, int *n)
{
    int i, ant=0;
    *r=*n=0;
    for(i=0; i<5; i++) {
         if (pix[i]) {
            if (!ant) {(*r)++;}
            (*n)++;
         }
         ant=pix[i];
    }
}


int main(int argc, const char * argv[])
{
      unsigned char *in, *out, *out_filtered;
      unsigned char *buffer;
      size_t buffersize;
      unsigned w, h;
      unsigned error;
      RENDER_PLUGIN_OUTP image;
      unsigned int mag;
      unsigned int algorithm;
      
      /*check if user gave a filename*/
      //if(argc < 5)
      if(argc < 3 || argc > 4)
      {
//          printf("Usage: filtros <input> <output> <magnification[2-7]> <algorithm[1-10]>\n");
//            printf("Uso: filtro_4xBR <input png 32bit> <output png 32bit>\n*** Programado por Hyllian. ***\n");
            printf("Usage: xBR <input png> <output png> [<scale_factor>]\n*** Programmed by Hyllian. ***\n");
            printf("scale_factor: 2x or 3x or 4x. (default: 4x)\n");
          return;
      }
      
//      mag       = atoi(argv[3]);      
//      algorithm = atoi(argv[4]);

        if (argc == 3) {mag = 4; algorithm = 7;}
        else if (argc == 4 && (!strcmp(argv[3], "2x") || !strcmp(argv[3], "2X"))) {mag = 2; algorithm = 8;}
        else if (argc == 4 && (!strcmp(argv[3], "3x") || !strcmp(argv[3], "3X"))) {mag = 3; algorithm = 9;}
        else if (argc == 4 && (!strcmp(argv[3], "4x") || !strcmp(argv[3], "4X"))) {mag = 4; algorithm = 7;}
        else {
            printf("Usage: xBR <input png> <output png> [<scale_factor>]\n*** Programmed by Hyllian. ***\n");
            printf("scale_factor: 2x or 3x or 4x. (default: 4x)\n");
            return;
             }
//        mag = 4;
//        algorithm = 4;
      
      if (mag < 2 || mag > 7)
     {
          printf("Usage: filtros <input> <output> <magnification[2-7]>\n");
          return;
      }
      
//      printf("mag = %d\n", mag);
      
      
      
      //LodePNG_loadFile(&buffer, &buffersize, argv[1]); 
      error = LodePNG_decode32_file(&in, &w, &h, argv[1]);     
      //error = LodePNG_decode32(&in, &w, &h, buffer, buffersize);
      
      /*if ((out = (unsigned char *) malloc(4*mag*mag*w*h*sizeof(char))) == NULL)
      {
          fprintf(stderr, "ERR: memory exhausted\n");
          return -1;
      }*/
      
      if(!error)
      {
         // size_t x, y;
          /*ghostify image*/
         // for(y = 0; y < h; y++)
         //     for(x = 0; x < w; x++)
         //     {
          //        out[6*(2*y*w+x)  ] = out[6*(2*y*w+x)+3] = out[6*((2*y+1)*w+x)  ] = out[6*((2*y+1)*w+x)+3] = in[3*(y*w+x)  ];
           //       out[6*(2*y*w+x)+1] = out[6*(2*y*w+x)+4] = out[6*((2*y+1)*w+x)+1] = out[6*((2*y+1)*w+x)+4] = in[3*(y*w+x)+1];
           //       out[6*(2*y*w+x)+2] = out[6*(2*y*w+x)+5] = out[6*((2*y+1)*w+x)+2] = out[6*((2*y+1)*w+x)+5] = in[3*(y*w+x)+2];
                  //size_t index = 4 * w * y + 4 * x;
                  //in[index + 3] = (unsigned char)(((int)(in[index + 0]) + (int)(in[index + 1]) + (int)(in[index + 2])) / 3);
          //    }
        
          /*encode and save*/
        //  free(buffer);
        //  buffer = 0;
        //  buffersize = 0;
          
          if ((out_filtered = (unsigned char *) malloc(4*mag*mag*w*h*sizeof(char))) == NULL)
          {
               fprintf(stderr, "ERR: memory exhausted\n");
               return -1;
          }

          image.SrcPtr   = in;
          image.SrcPitch = 2*w;
          image.SrcW     = w;
          image.SrcH     = h;
          image.DstPtr   = out_filtered;
          image.DstPitch = 2*mag*w;
          image.DstW     = mag*w;
          image.DstH     = mag*h;
          
          switch(algorithm)
          {
              case  2: Filter_2xBR(&image); break;
              case  3: Filter_2xBR_v2(&image); break;
              case  4: Filter_4xBR(&image); break;
              case  5: Filter_2xBR_v3(&image); break;
              case  6: Filter_2xBR_v4(&image); break;
              case  7: Filter_4xBR_v2(&image); break;
              case  8: Filter_2xBR_v5(&image); break;
              case  9: Filter_3xBR(&image); break;
              default: break;
          };

          //LodePNG_encode32(&buffer, &buffersize, out_filtered, mag*w, mag*h);
          LodePNG_encode32_file(argv[2], out_filtered, mag*w, mag*h);
          //LodePNG_saveFile(buffer, buffersize, argv[2]);
       }

       printf("done.\n");
      
       /*cleanup*/
       free(in);
       //free(buffer);
       free(out);
       free(out_filtered);
    
       return 0;
}




void Filter_4xBR_v2(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	unsigned int A0, D0, G0, A1, B1, C1, C4, F4, I4, G5, H5, I5, P00, P04, P40, P44; 
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	register unsigned int *start_addr0, *start_addr4;
	unsigned long next_line, next_line_src;
    unsigned long next_line1, next_line2;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned int E[16];
	unsigned long src_pitch;
	unsigned char pprev, pprev2;	
	unsigned int cont1, cont2, cont3, cont4, ct, temp, temp2;
	int credit;
	
	cont1 = cont2 = cont3 = cont4 = ct = 0;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW << 2)<=rpo->DstW) && ((rpo->SrcH << 2)<=rpo->DstH)))
		return;

	
	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_lbmask     = PG_LBMASK;
	pg_alpha_mask = ALPHA_MASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch/2;
	next_line1= rpo->DstPitch;
	next_line2= rpo->DstPitch*3/2;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = (rpo->DstPitch*2) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 8);
	start_addr0 = start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;
	start_addr4 = start_addr3 + src_pitch;

	dst_pixel = (unsigned int *)(dest);	

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{		
		//if (y == src_height - 1)
		if (!y) 
            start_addr4 = start_addr3 = start_addr2;
        
        if (y == 1)
            start_addr4 = start_addr3;
        		
		pprev  = 2;
		pprev2 = 2;
        
        x = src_width;
        
		//for (x = 0; x < src_width; x++)
		while(x--) 
		{			
			B1 = start_addr0[2];
            PB = start_addr1[2];
			PE = start_addr2[2];			
			PH = start_addr3[2];
			H5 = start_addr4[2];
			
			A1 = start_addr0[pprev];
			PA = start_addr1[pprev];
			PD = start_addr2[pprev];			
			PG = start_addr3[pprev];
			G5 = start_addr4[pprev];
			
			P00= start_addr0[pprev2];
			A0 = start_addr1[pprev2];
			D0 = start_addr2[pprev2];			
			G0 = start_addr3[pprev2];
			P40= start_addr4[pprev2];

			//if (x < src_width - 1)
			if (x > 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
                P04= start_addr0[4];
				C4 = start_addr1[4];
				F4 = start_addr2[4];
				I4 = start_addr3[4];
				P44= start_addr4[4];

			}
			else if (x == 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
				P04= start_addr0[3];
				C4 = start_addr1[3];
				F4 = start_addr2[3];
				I4 = start_addr3[3];
				P44= start_addr4[3];
			}
            else
            {
                C1 = start_addr0[2];
				PC = start_addr1[2];
				PF = start_addr2[2];
				PI = start_addr3[2];
				I5 = start_addr4[2];
				
				P04= start_addr0[2];
				C4 = start_addr1[2];
				F4 = start_addr2[2];
				I4 = start_addr3[2];
				P44= start_addr4[2];
			}
			
			E[0]  = E[1]  = E[2]  = E[3]  = PE;
			E[4]  = E[5]  = E[6]  = E[7]  = PE;
			E[8]  = E[9]  = E[10] = E[11] = PE;
			E[12] = E[13] = E[14] = E[15] = PE;


            FILTROLLL(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, 15, 14, 11,  3,  7, 10, 13, 12,  9,  6, 2,  1, 5, 8, 4, 0, cont1, cont2, cont3, cont4, ct);
            FILTROLLL(PE, PC, PF, PB, PI, PA, PH, PD, PG, I4, A1, I5, H5, A0, D0, B1, C1, F4, C4, G5, G0, P00, P44, P04, P40,  3,  7,  2,  0,  1,  6, 11, 15, 10,  5, 4,  8, 9,14,13,12, cont1, cont2, cont3, cont4, ct);
            FILTROLLL(PE, PA, PB, PD, PC, PG, PF, PH, PI, C1, G0, C4, F4, G5, H5, D0, A0, B1, A1, I4, I5, P40, P04, P00, P44,  0,  1,  4, 12,  8,  5,  2,  3,  6,  9, 13,14,10, 7,11,15, cont1, cont2, cont3, cont4, ct);
            FILTROLLL(PE, PG, PD, PH, PA, PI, PB, PF, PC, A0, I5, A1, B1, I4, F4, H5, G5, D0, G0, C1, C4, P44, P00, P40, P04, 12,  8, 13, 15, 14,  9,  4,  0,  5, 10, 11, 7, 6, 1, 2, 3, cont1, cont2, cont3, cont4, ct);
            
            

			dst_pixel[0] = E[0];
			dst_pixel[1] = E[1];
			dst_pixel[2] = E[2];
			dst_pixel[3] = E[3];
			dst_pixel[next_line] = E[4];
			dst_pixel[next_line + 1] = E[5];
			dst_pixel[next_line + 2] = E[6];
			dst_pixel[next_line + 3] = E[7];
			dst_pixel[next_line1] = E[8];
			dst_pixel[next_line1 + 1] = E[9];
			dst_pixel[next_line1 + 2] = E[10];
			dst_pixel[next_line1 + 3] = E[11];
			dst_pixel[next_line2] = E[12];
			dst_pixel[next_line2 + 1] = E[13];
			dst_pixel[next_line2 + 2] = E[14];
			dst_pixel[next_line2 + 3] = E[15];
		
		    start_addr0++;
			start_addr1++;
			start_addr2++;
			start_addr3++;
			start_addr4++;
			
			dst_pixel += 4;
			pprev = 1;
			
			if (pprev2)
			    pprev2--;
		}

		
        start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;		
		start_addr3 = start_addr2 + next_line_src;
		
		
		if (y == src_height - 1)
		{
            start_addr0 = start_addr1;		
		    start_addr4 = start_addr3 + next_line_src;
        }
        else if (!y)
		{
            start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3;
        }                
        else
        {
            start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3 + next_line_src;
        }
                
        dst_pixel += complete_line_dst;				
	}
//	printf("Sucesso!\n%5d pixels analisados.\n", ct);
	printf("Wait, saving png file... ");
/*	printf("  na = %6d\n", cont1);
	printf("  nb = %6d\n", cont2);
	printf("  nc = %6d\n", cont3);         
    printf("  nd = %6d\n", cont4);
    printf("soma = %6d\n", cont1 + cont2 + cont3 + cont4);
    printf("  nt = %6d\n", ct);
    */
}


void Filter_3xBR(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	unsigned int A0, D0, G0, A1, B1, C1, C4, F4, I4, G5, H5, I5, P00, P04, P40, P44; 
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	register unsigned int *start_addr0, *start_addr4;
	unsigned long next_line, next_line_src;
    unsigned long next_line1;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned int E[9];
	unsigned long src_pitch;
	unsigned char pprev, pprev2;	
	unsigned int cont1, cont2, cont3, cont4, ct, temp, temp2;
	int credit;
	
	cont1 = cont2 = cont3 = cont4 = ct = 0;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW * 3)<=rpo->DstW) && ((rpo->SrcH * 3)<=rpo->DstH)))
		return;

	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_lbmask     = PG_LBMASK;
	pg_alpha_mask = ALPHA_MASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch >> 1;
	next_line1= rpo->DstPitch;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = ((rpo->DstPitch*3)>>1) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 8);
	start_addr0 = start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;
	start_addr4 = start_addr3 + src_pitch;

	dst_pixel = (unsigned int *)(dest);	

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{		
		//if (y == src_height - 1)
		if (!y) 
            start_addr4 = start_addr3 = start_addr2;
        
        if (y == 1)
            start_addr4 = start_addr3;
        		
		pprev  = 2;
		pprev2 = 2;
        
        x = src_width;
        
		//for (x = 0; x < src_width; x++)
		while(x--) 
		{			
			B1 = start_addr0[2];
            PB = start_addr1[2];
			PE = start_addr2[2];			
			PH = start_addr3[2];
			H5 = start_addr4[2];
			
			A1 = start_addr0[pprev];
			PA = start_addr1[pprev];
			PD = start_addr2[pprev];			
			PG = start_addr3[pprev];
			G5 = start_addr4[pprev];
			
			P00= start_addr0[pprev2];
			A0 = start_addr1[pprev2];
			D0 = start_addr2[pprev2];			
			G0 = start_addr3[pprev2];
			P40= start_addr4[pprev2];

			//if (x < src_width - 1)
			if (x > 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
                P04= start_addr0[4];
				C4 = start_addr1[4];
				F4 = start_addr2[4];
				I4 = start_addr3[4];
				P44= start_addr4[4];

			}
			else if (x == 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
				P04= start_addr0[3];
				C4 = start_addr1[3];
				F4 = start_addr2[3];
				I4 = start_addr3[3];
				P44= start_addr4[3];
			}
            else
            {
                C1 = start_addr0[2];
				PC = start_addr1[2];
				PF = start_addr2[2];
				PI = start_addr3[2];
				I5 = start_addr4[2];
				
				P04= start_addr0[2];
				C4 = start_addr1[2];
				F4 = start_addr2[2];
				I4 = start_addr3[2];
				P44= start_addr4[2];
			}
			
			E[0]  = E[1]  = E[2]  = PE;
			E[3]  = E[4]  = E[5]  = PE;
			E[6]  = E[7]  = E[8]  = PE;


            FILTRO(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, 15, 14, 11,  3,  7, 10, 13, 12,  9,  6, 2,  1, 5, 8, 4, 0, cont1, cont2, cont3, cont4, ct);
            FILTRO(PE, PC, PF, PB, PI, PA, PH, PD, PG, I4, A1, I5, H5, A0, D0, B1, C1, F4, C4, G5, G0, P00, P44, P04, P40,  3,  7,  2,  7,  5,  6, 11, 15, 10,  8, 0,  3, 1, 2, 4, 6, cont1, cont2, cont3, cont4, ct);
            FILTRO(PE, PA, PB, PD, PC, PG, PF, PH, PI, C1, G0, C4, F4, G5, H5, D0, A0, B1, A1, I4, I5, P40, P04, P00, P44,  0,  1,  4,  5,  1,  5,  2,  3,  6,  2,  6, 7, 3, 0, 4, 8, cont1, cont2, cont3, cont4, ct);
            FILTRO(PE, PG, PD, PH, PA, PI, PB, PF, PC, A0, I5, A1, B1, I4, F4, H5, G5, D0, G0, C1, C4, P44, P00, P40, P04, 12,  8, 13,  1,  3,  9,  4,  0,  5,  0,  8, 5, 7, 6, 4, 2, cont1, cont2, cont3, cont4, ct);


			dst_pixel[0] = E[0];
			dst_pixel[1] = E[1];
			dst_pixel[2] = E[2];
			dst_pixel[next_line] = E[3];
			dst_pixel[next_line + 1] = E[4];
			dst_pixel[next_line + 2] = E[5];
			dst_pixel[next_line1] = E[6];
			dst_pixel[next_line1 + 1] = E[7];
			dst_pixel[next_line1 + 2] = E[8];
		
		    start_addr0++;
			start_addr1++;
			start_addr2++;
			start_addr3++;
			start_addr4++;
			
			dst_pixel += 3;
			pprev = 1;
			
			if (pprev2)
			    pprev2--;
		}

		
        start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;		
		start_addr3 = start_addr2 + next_line_src;
		
		
		if (y == src_height - 1)
		{
            start_addr0 = start_addr1;		
		    start_addr4 = start_addr3 + next_line_src;
        }
        else if (!y)
		{
            start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3;
        }                
        else
        {
            start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3 + next_line_src;
        }
                
        dst_pixel += complete_line_dst;				
	}
//	printf("Sucesso!\n%5d pixels analisados.\n", ct);
	printf("Wait, saving png file... ");
/*	printf("  na = %6d\n", cont1);
	printf("  nb = %6d\n", cont2);
	printf("  nc = %6d\n", cont3);         
    printf("  nd = %6d\n", cont4);
    printf("soma = %6d\n", cont1 + cont2 + cont3 + cont4);
    printf("  nt = %6d\n", ct);
    */
}

void Filter_2xBR_v5(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	unsigned int A0, D0, G0, A1, B1, C1, C4, F4, I4, G5, H5, I5, P00, P04, P40, P44; 
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	register unsigned int *start_addr0, *start_addr4;
	unsigned long next_line, next_line_src;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned int E[4];
	unsigned long src_pitch;
	unsigned char pprev, pprev2;	
	unsigned int cont1, cont2, cont3, cont4, ct, temp, temp2;
	int credit;
	
	cont1 = cont2 = cont3 = cont4 = ct = 0;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW << 1)<=rpo->DstW) && ((rpo->SrcH << 1)<=rpo->DstH)))
		return;

	
	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_lbmask     = PG_LBMASK;
	pg_alpha_mask = ALPHA_MASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch >> 1;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = (rpo->DstPitch) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 8);
	start_addr0 = start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;
	start_addr4 = start_addr3 + src_pitch;

	dst_pixel = (unsigned int *)(dest);	

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{	
		//if (y == src_height - 1)
		if (!y) 
            start_addr4 = start_addr3 = start_addr2;
        
        if (y == 1)
            start_addr4 = start_addr3;
        		
		pprev  = 2;
		pprev2 = 2;
        
        x = src_width;
        
		
		//for (x = 0; x < src_width; x++)
		while(x--) 
		{			
			B1 = start_addr0[2];
            PB = start_addr1[2];
			PE = start_addr2[2];			
			PH = start_addr3[2];
			H5 = start_addr4[2];
			
			A1 = start_addr0[pprev];
			PA = start_addr1[pprev];
			PD = start_addr2[pprev];			
			PG = start_addr3[pprev];
			G5 = start_addr4[pprev];
			
			P00= start_addr0[pprev2];
			A0 = start_addr1[pprev2];
			D0 = start_addr2[pprev2];			
			G0 = start_addr3[pprev2];
			P40= start_addr4[pprev2];

			//if (x < src_width - 1)
			if (x > 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
                P04= start_addr0[4];
				C4 = start_addr1[4];
				F4 = start_addr2[4];
				I4 = start_addr3[4];
				P44= start_addr4[4];

			}
			else if (x == 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
				P04= start_addr0[3];
				C4 = start_addr1[3];
				F4 = start_addr2[3];
				I4 = start_addr3[3];
				P44= start_addr4[3];
			}
            else
            {
                C1 = start_addr0[2];
				PC = start_addr1[2];
				PF = start_addr2[2];
				PI = start_addr3[2];
				I5 = start_addr4[2];
				
				P04= start_addr0[2];
				C4 = start_addr1[2];
				F4 = start_addr2[2];
				I4 = start_addr3[2];
				P44= start_addr4[2];
			}
            
           // printf("erro 6\n");		

			
			
			E[0]  = E[1]  = E[2]  = E[3]  = PE;
			
			//ATENCAO!! COLUNA 3, 1, 0, 2 ALTERADA PARA FUNCIONAR COM 2XBR!!
			
            FILTRO_2X(PE, PI, PH, PF, PG, PC, PD, PB, PA, G5, C4, G0, D0, C1, B1, F4, I4, H5, I5, A0, A1, P04, P40, P44, P00, 15, 14, 11,  3,  7, 10, 13, 12,  9,  6, 2,  1, 5, 8, 4, 0, cont1, cont2, cont3, cont4, ct);
            FILTRO_2X(PE, PC, PF, PB, PI, PA, PH, PD, PG, I4, A1, I5, H5, A0, D0, B1, C1, F4, C4, G5, G0, P00, P44, P04, P40,  3,  7,  2,  1,  1,  6, 11, 15, 10,  5, 3,  0, 9,14,13, 2, cont1, cont2, cont3, cont4, ct);
            FILTRO_2X(PE, PA, PB, PD, PC, PG, PF, PH, PI, C1, G0, C4, F4, G5, H5, D0, A0, B1, A1, I4, I5, P40, P04, P00, P44,  0,  1,  4,  0,  8,  5,  2,  3,  6,  9, 1,  2,10, 7,11, 3, cont1, cont2, cont3, cont4, ct);
            FILTRO_2X(PE, PG, PD, PH, PA, PI, PB, PF, PC, A0, I5, A1, B1, I4, F4, H5, G5, D0, G0, C1, C4, P44, P00, P40, P04, 12,  8, 13,  2, 14,  9,  4,  0,  5, 10, 0,  3, 6, 1, 2, 1, cont1, cont2, cont3, cont4, ct);

        	
			dst_pixel[0] = E[0];
			dst_pixel[1] = E[1];
			dst_pixel[next_line] = E[2];
			dst_pixel[next_line + 1] = E[3];
		
		    start_addr0++;
			start_addr1++;
			start_addr2++;
			start_addr3++;
			start_addr4++;
			
			dst_pixel += 2;
			pprev = 1;
			
			if (pprev2)
			    pprev2--;
			    
			
            
		} // while(x);
		
///		printf("erro 1\n");
        
        start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;		
		start_addr3 = start_addr2 + next_line_src;
		
//		printf("erro 2\n");
		
		if (y == src_height - 1)
		{
//            printf("fez 1\n");
		    start_addr0 = start_addr1;		
		    start_addr4 = start_addr3 + next_line_src;
        }
        else if (!y)
		{
//            printf("fez 2\n");
		    start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3;
        }                
        else
        {
//            printf("fez 3\n");
		    start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3 + next_line_src;
        }
        
//        printf("erro 3\n");
        
        dst_pixel += complete_line_dst;				
	} // while(y);
	printf("Wait, saving png file... ");
/*	printf("  na = %6d\n", cont1);
	printf("  nb = %6d\n", cont2);
	printf("  nc = %6d\n", cont3);         
    printf("  nd = %6d\n", cont4);
    printf("soma = %6d\n", cont1 + cont2 + cont3 + cont4);
    printf("  nt = %6d\n", ct);*/
}



void Filter_2xBR_v2(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	unsigned long next_line, next_line_src;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned char auto_blend;
	unsigned int E[4];
	unsigned long src_pitch;
	unsigned char pprev;	
	unsigned char dont_reblit;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW << 1)<=rpo->DstW) && ((rpo->SrcH << 1)<=rpo->DstH)))
		return;

	
	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_alpha_mask = ALPHA_MASK;
	pg_lbmask     = PG_LBMASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch >> 1;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = (rpo->DstPitch) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 4);
	start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;

	dst_pixel = (unsigned int *)(dest);

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{	
		//if (y == src_height - 1)
		if (!y)
			start_addr3 = start_addr2;		
		auto_blend = 0;
		pprev = 1;
		x = src_width;
		
		//for (x = 0; x < src_width; x++)
		while(x--) 
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
				PC = start_addr1[1];
				PF = start_addr2[1];
				PI = start_addr3[1];
			}		

			
			dont_reblit = 0;
			
			E[0]  = E[1]  = E[2]  = E[3]  = PE;
			
            if ( (eq(PH, PF)) && (PH != PE) )
            //if ((PH == PF)&&(PH != PE))
            {
                 if (
                      ( (eq(PE, PG)) && ((eq(PI, PH)) || (eq(PE, PD))))
                      ||
                      ( (eq(PE, PC)) && ((eq(PI, PH)) || (eq(PE, PB))))
                     )
                 {
                      ALPHA_BLEND_128_W(E[3], PF);                      
                 }
            }
            
            if ( (eq(PF, PB)) &&  (PF != PE) )
            {
                 if (
                      ( (eq(PE, PI)) && ( (eq(PF, PC)) || (eq(PE, PH)) ) )
                      ||
                      ( (eq(PE, PA)) && ( (eq(PF, PC)) || (eq(PE, PD)) ) )
                    )
                 {
                      ALPHA_BLEND_128_W(E[1], PB);                      
                 }
            }
            
            if (eq(PB, PD) && (PB != PE))
            {
                 if (
                      (eq(PE, PC) && (eq(PB, PA) || eq(PE, PF)))
                      ||
                      (eq(PE, PG) && (eq(PB, PA) || eq(PE, PH)))
                    )                        
                 {
                      ALPHA_BLEND_128_W(E[0], PD);                      
                 }
            }
            
            if (eq(PD, PH) && (PD != PE))
            {
                 if (
                      (eq(PE, PA) && (eq(PD, PG) || eq(PE, PB)))
                      ||
                      (eq(PE, PI) && (eq(PD, PG) || eq(PE, PF)))
                    )
                 {
                      ALPHA_BLEND_128_W(E[2], PH);
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
		}

		start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;
		start_addr3 = start_addr2 + next_line_src;
		dst_pixel += complete_line_dst;				
	}
}



void Filter_2xBR_v4(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	unsigned long next_line, next_line_src;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned char auto_blend;
	unsigned int E[4];
	unsigned long src_pitch;
	unsigned char pprev;	
	unsigned char dont_reblit, dra, drb, drc, drd;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW << 1)<=rpo->DstW) && ((rpo->SrcH << 1)<=rpo->DstH)))
		return;

	
	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_lbmask     = PG_LBMASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch >> 1;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = (rpo->DstPitch) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 4);
	start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;

	dst_pixel = (unsigned int *)(dest);	

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{	
		//if (y == src_height - 1)
		if (!y)
			start_addr3 = start_addr2;		
		auto_blend = 0;
		pprev = 1;
		x = src_width;
		
		//for (x = 0; x < src_width; x++)
		while(x--) 
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
				PC = start_addr1[1];
				PF = start_addr2[1];
				PI = start_addr3[1];
			}		

			
			dont_reblit = 0;
			dra = drb = drc = drd = 0;
			
			E[0]  = E[1]  = E[2]  = E[3]  = PE;
			
            if ((PH == PF)&&(PH != PE))
            {
//                 if ( ((PE==PG) && ((PI == PH) || (PE == PD))) || ((PE==PC) && ((PI == PH) || (PE == PB))) )
//                 {
//                      ALPHA_BLEND_128_W(E[3], PF);
//                      dra = 1;
//                 }
                 
                 //if (!dra && (PE != PI))
                 if ((PE != PI && ((PE != PB && PE != PD) || (PE == PC || PE == PG)) ) || (PE == PD && PE == PG) || (PE == PB && PE == PC))
                 {
                      ALPHA_BLEND_128_W(E[3], PF);
                 }

            }
            else if (((PE != PI) && (PE != PH) && (PE != PF) && (PI == PH || PI == PF)) && (PE == PG || PE == PC))
//            else if ((!eq(PE, PI) && !eq(PE, PH) && !eq(PE, PF)) && (PE == PG || PE == PC))
            {
//                 ALPHA_BLEND_128_W(E[3], PI);
                 ALPHA_BLEND_128_W(E[3], PH);
                 ALPHA_BLEND_128_W(E[3], PF);
            }

            
            if ((PF == PB)&&(PF != PE))
            {
//                 if ( ((PE==PI) && ((PF == PC) || (PE == PH))) || ((PE==PA) && ((PF == PC) || (PE == PD))) )
//                 {
//                      ALPHA_BLEND_128_W(E[1], PB);                      
//                      drb = 1;
//                 }
                 
                 //if (!drb && (PE != PC))
                 if ((PE != PC && ((PE != PD && PE != PH) || (PE == PA || PE == PI)) ) || (PE == PD && PE == PA) || (PE == PH && PE == PI))
                 {
                      ALPHA_BLEND_128_W(E[1], PB);
                 }
            }
            else if (((PE != PC) && (PE != PF) && (PE != PB) && (PC == PF || PC == PB)) && (PE == PI || PE == PA))
  //        else if ((!eq(PE, PC) && !eq(PE, PF) && !eq(PE, PB)) && (PE == PI || PE == PA))
            {
    //           ALPHA_BLEND_128_W(E[1], PC);
                 ALPHA_BLEND_128_W(E[1], PF);
                 ALPHA_BLEND_128_W(E[1], PB);
            }
            
            if ((PB == PD)&&(PB != PE))
            {
//                 if ( ((PE==PC) && ((PB == PA) || (PE == PF))) || ((PE==PG) && ((PB == PA) || (PE == PH))) )                        
//                 {
//                      ALPHA_BLEND_128_W(E[0], PD);                      
//                      drc = 1;
//                 }
                 
                 //if (!drc && (PE != PA))
                 if ((PE != PA && ((PE != PH && PE != PF) || (PE == PG || PE == PC))) || (PE == PF && PE == PC) || (PE == PH && PE == PG))
                 {
                      ALPHA_BLEND_128_W(E[0], PD);
                 }                 
            }
            else if (((PE != PA) && (PE != PB) && (PE != PD) && (PA == PB || PA == PD)) && (PE == PC || PE == PG))
 //         else if ((!eq(PE, PA) && !eq(PE, PB) && !eq(PE, PD)) && (PE == PC || PE == PG))
            {
   //            ALPHA_BLEND_128_W(E[0], PA);
                 ALPHA_BLEND_128_W(E[0], PB);
                 ALPHA_BLEND_128_W(E[0], PD);
            }
            
            if ((PD == PH)&&(PD != PE))
            {
//                 if ( ((PE==PA) && ((PD == PG) || (PE == PB))) || ((PE==PI) && ((PD == PG) || (PE == PF))) )
//                 {
//                      ALPHA_BLEND_128_W(E[2], PH);
//                      drd = 1;
//                 }
                 
                 //if (!drd && (PE != PG))
                 if ((PE != PG && ((PE != PF && PE != PB) || (PE == PI || PE == PA))) || (PE == PF && PE == PI) || (PE == PB && PE == PA))
                 {
                      ALPHA_BLEND_128_W(E[2], PH);
                 }
            }
            else if (((PE != PG) && (PE != PD) && (PE != PH) && (PG == PD || PG == PH)) && (PE == PA || PE == PI))
  //          else if ((!eq(PE, PG) && !eq(PE, PD) && !eq(PE, PH)) && (PE == PA || PE == PI))
            {
    //             ALPHA_BLEND_128_W(E[2], PG);
                 ALPHA_BLEND_128_W(E[2], PD);
                 ALPHA_BLEND_128_W(E[2], PH);
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
		}

		start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;
		start_addr3 = start_addr2 + next_line_src;
		dst_pixel += complete_line_dst;				
	}
}


void Filter_2xBR_v3(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	unsigned int A0, D0, G0, A1, B1, C1, C4, F4, I4, G5, H5, I5, P00, P04, P40, P44; 
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	register unsigned int *start_addr0, *start_addr4;
	unsigned long next_line, next_line_src;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned char auto_blend;
	unsigned int E[4];
	unsigned long src_pitch;
	unsigned char pprev, pprev2;	
	unsigned char dont_reblit;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW << 1)<=rpo->DstW) && ((rpo->SrcH << 1)<=rpo->DstH)))
		return;

	
	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_lbmask     = PG_LBMASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch >> 1;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = (rpo->DstPitch) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 8);
	start_addr0 = start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;
	start_addr4 = start_addr3 + src_pitch;

	dst_pixel = (unsigned int *)(dest);	

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{	
		//if (y == src_height - 1)
		if (!y) 
            start_addr4 = start_addr3 = start_addr2;
        
        if (y == 1)
            start_addr4 = start_addr3;
        		
		auto_blend = 0;
		pprev  = 2;
		pprev2 = 2;
        
        x = src_width;
        
        printf("erro 4\n");
		
		//for (x = 0; x < src_width; x++)
		while(x--) 
		{			
			B1 = start_addr0[2];
            PB = start_addr1[2];
			PE = start_addr2[2];			
			PH = start_addr3[2];
			H5 = start_addr4[2];
			
			A1 = start_addr0[pprev];
			PA = start_addr1[pprev];
			PD = start_addr2[pprev];			
			PG = start_addr3[pprev];
			G5 = start_addr4[pprev];
			
			P00= start_addr0[pprev2];
			A0 = start_addr1[pprev2];
			D0 = start_addr2[pprev2];			
			G0 = start_addr3[pprev2];
			P40= start_addr4[pprev2];

			//if (x < src_width - 1)
			if (x > 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
                P04= start_addr0[4];
				C4 = start_addr1[4];
				F4 = start_addr2[4];
				I4 = start_addr3[4];
				P44= start_addr4[4];

			}
			else if (x == 1)
			{
                C1 = start_addr0[3];
				PC = start_addr1[3];
				PF = start_addr2[3];
				PI = start_addr3[3];
				I5 = start_addr4[3];
				
				P04= start_addr0[3];
				C4 = start_addr1[3];
				F4 = start_addr2[3];
				I4 = start_addr3[3];
				P44= start_addr4[3];
			}
            else
            {
                C1 = start_addr0[2];
				PC = start_addr1[2];
				PF = start_addr2[2];
				PI = start_addr3[2];
				I5 = start_addr4[2];
				
				P04= start_addr0[2];
				C4 = start_addr1[2];
				F4 = start_addr2[2];
				I4 = start_addr3[2];
				P44= start_addr4[2];
			}
            
           // printf("erro 6\n");		

			
			dont_reblit = 0;
			
			E[0]  = E[1]  = E[2]  = E[3]  = PE;
			
            if ((PH == PF)&&(PH != PE))
            {
                 if (
                      ((PE==PG) && ((PI == PH) || (PE == PD)))
                      ||
                      ((PE==PC) && ((PI == PH) || (PE == PB)))
                     )
                 {
                      ALPHA_BLEND_128_W(E[3], PF);                      
                 }
            }
            
            if ((PF == PB)&&(PF != PE))
            {
                 if (
                      ((PE==PI) && ((PF == PC) || (PE == PH)))
                      ||
                      ((PE==PA) && ((PF == PC) || (PE == PD)))
                    )
                 {
                      ALPHA_BLEND_128_W(E[1], PB);                      
                 }
            }
            
            if ((PB == PD)&&(PB != PE))
            {
                 if (
                      ((PE==PC) && ((PB == PA) || (PE == PF)))
                      ||
                      ((PE==PG) && ((PB == PA) || (PE == PH)))
                    )                        
                 {
                      ALPHA_BLEND_128_W(E[0], PD);                      
                 }
            }
            
            if ((PD == PH)&&(PD != PE))
            {
                 if (
                      ((PE==PA) && ((PD == PG) || (PE == PB)))
                      ||
                      ((PE==PI) && ((PD == PG) || (PE == PF)))
                    )
                 {
                      ALPHA_BLEND_128_W(E[2], PH);
                 }
            }
        	
			dst_pixel[0] = E[0];
			dst_pixel[1] = E[1];
			dst_pixel[next_line] = E[2];
			dst_pixel[next_line + 1] = E[3];
		
		    start_addr0++;
			start_addr1++;
			start_addr2++;
			start_addr3++;
			start_addr4++;
			
			dst_pixel += 2;
			pprev = 1;
			
			if (pprev2)
			    pprev2--;
			    
			
            
		} // while(x);
		
		printf("erro 1\n");
        
        start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;		
		start_addr3 = start_addr2 + next_line_src;
		
		printf("erro 2\n");
		
		if (y == src_height - 1)
		{
            printf("fez 1\n");
		    start_addr0 = start_addr1;		
		    start_addr4 = start_addr3 + next_line_src;
        }
        else if (!y)
		{
            printf("fez 2\n");
		    start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3;
        }                
        else
        {
            printf("fez 3\n");
		    start_addr0 = start_addr1 - next_line_src;		
		    start_addr4 = start_addr3 + next_line_src;
        }
        
        printf("erro 3\n");
        
        dst_pixel += complete_line_dst;				
	} // while(y);
}


void Filter_4xBR(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	unsigned long next_line, next_line_src;
    unsigned long next_line1, next_line2;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned char auto_blend;
	unsigned int E[16];
	unsigned long src_pitch;
	unsigned char pprev;	
	unsigned char dont_reblit;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW << 2)<=rpo->DstW) && ((rpo->SrcH << 2)<=rpo->DstH)))
		return;

	
	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_lbmask     = PG_LBMASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch/2;
	next_line1= rpo->DstPitch;
	next_line2= rpo->DstPitch*3/2;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = (rpo->DstPitch*2) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 4);
	start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;

	dst_pixel = (unsigned int *)(dest);	

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{	
		//if (y == src_height - 1)
		if (!y)
			start_addr3 = start_addr2;		
		auto_blend = 0;
		pprev = 1;
		x = src_width;
		
		//for (x = 0; x < src_width; x++)
		while(x--) 
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
				PC = start_addr1[1];
				PF = start_addr2[1];
				PI = start_addr3[1];
			}		

			
			dont_reblit = 0;
			
			E[0]  = E[1]  = E[2]  = E[3]  = PE;
			E[4]  = E[5]  = E[6]  = E[7]  = PE;
			E[8]  = E[9]  = E[10] = E[11] = PE;
			E[12] = E[13] = E[14] = E[15] = PE;
			
            if ((PH == PF)&&(PH != PE))
            {
                 if (
                      ((PE==PG) && ((PI == PH) || (PE == PD)))
                      ||
                      ((PE==PC) && ((PI == PH) || (PE == PB)))
                     )
                 {
                      ALPHA_BLEND_128_W(E[11], PF);
                      E[14] = E[11];
                      E[15] = PF;
                 }
            }
            
            if ((PF == PB)&&(PF != PE))
            {
                 if (
                      ((PE==PI) && ((PF == PC) || (PE == PH)))
                      ||
                      ((PE==PA) && ((PF == PC) || (PE == PD)))
                    )
                 {
                      ALPHA_BLEND_128_W(E[2], PB);
                      E[7] = E[2];
                      E[3] = PB;
                 }
            }
            
            if ((PB == PD)&&(PB != PE))
            {
                 if (
                      ((PE==PC) && ((PB == PA) || (PE == PF)))
                      ||
                      ((PE==PG) && ((PB == PA) || (PE == PH)))
                    )                        
                 {
                      ALPHA_BLEND_128_W(E[1], PD);
                      E[4] = E[1];
                      E[0] = PD;
                 }
            }
            
            if ((PD == PH)&&(PD != PE))
            {
                 if (
                      ((PE==PA) && ((PD == PG) || (PE == PB)))
                      ||
                      ((PE==PI) && ((PD == PG) || (PE == PF)))
                    )
                 {
                      ALPHA_BLEND_128_W(E[8], PH);
                      E[13] = E[8];
                      E[12] = PH;
                 }
            }
        	
			dst_pixel[0] = E[0];
			dst_pixel[1] = E[1];
			dst_pixel[2] = E[2];
			dst_pixel[3] = E[3];
			dst_pixel[next_line] = E[4];
			dst_pixel[next_line + 1] = E[5];
			dst_pixel[next_line + 2] = E[6];
			dst_pixel[next_line + 3] = E[7];
			dst_pixel[next_line1] = E[8];
			dst_pixel[next_line1 + 1] = E[9];
			dst_pixel[next_line1 + 2] = E[10];
			dst_pixel[next_line1 + 3] = E[11];
			dst_pixel[next_line2] = E[12];
			dst_pixel[next_line2 + 1] = E[13];
			dst_pixel[next_line2 + 2] = E[14];
			dst_pixel[next_line2 + 3] = E[15];
		
			start_addr1++;
			start_addr2++;
			start_addr3++;				
			
			dst_pixel += 4;
			pprev = 0;			
		}

		start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;
		start_addr3 = start_addr2 + next_line_src;
		dst_pixel += complete_line_dst;				
	}
}


void Filter_2xBR(RENDER_PLUGIN_OUTP *rpo)
{	
	unsigned long x, y;
	unsigned char *src, *dest;			
	unsigned int PA, PB, PC, PD, PE, PF, PG, PH, PI;
	register unsigned int *start_addr1, *start_addr2, *start_addr3;
	unsigned long next_line, next_line_src;
    unsigned int *dst_pixel;
	unsigned long src_width, src_height;
	unsigned long complete_line_src, complete_line_dst;	
	unsigned char auto_blend;
	unsigned int E[4];
	unsigned long src_pitch;
	unsigned char pprev;	
	unsigned char dont_reblit;
	
	/* Don't blit anything if we don't have enough screen space */
   if(!(((rpo->SrcW << 1)<=rpo->DstW) && ((rpo->SrcH << 1)<=rpo->DstH)))
		return;

	
	pg_red_mask   = RED_MASK;
	pg_green_mask = GREEN_MASK;
	pg_blue_mask  = BLUE_MASK;
	pg_lbmask     = PG_LBMASK;

	src = (unsigned char *)rpo->SrcPtr;	
	dest = (unsigned char *)rpo->DstPtr;
	src_pitch = rpo->SrcPitch;

	next_line_src = src_pitch >> 1;
	next_line = rpo->DstPitch >> 1;
	
	src_width = rpo->SrcW;
	src_height = rpo->SrcH;

	// fixed by Steve Snake
	complete_line_src = (src_pitch>>1) - rpo->SrcW;
	complete_line_dst = (rpo->DstPitch) - rpo->DstW;
	
	start_addr2 = (unsigned int *)(src - 4);
	start_addr1 = start_addr2;
	start_addr3 = start_addr2 + src_pitch;

	dst_pixel = (unsigned int *)(dest);	

	y = src_height;
	//for (y = 0; y < src_height; y++)
	while(y--)
	{	
		//if (y == src_height - 1)
		if (!y)
			start_addr3 = start_addr2;		
		auto_blend = 0;
		pprev = 1;
		x = src_width;
		
		//for (x = 0; x < src_width; x++)
		while(x--) 
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
				PC = start_addr1[1];
				PF = start_addr2[1];
				PI = start_addr3[1];
			}		

			
			dont_reblit = 0;
			
			E[0]  = E[1]  = E[2]  = E[3]  = PE;
			
            if ((PH == PF)&&(PH != PE))
            {
                 if (
                      ((PE==PG) && ((PI == PH) || (PE == PD)))
                      ||
                      ((PE==PC) && ((PI == PH) || (PE == PB)))
                     )
                 {
                      ALPHA_BLEND_128_W(E[3], PF);                      
                 }
            }
            
            if ((PF == PB)&&(PF != PE))
            {
                 if (
                      ((PE==PI) && ((PF == PC) || (PE == PH)))
                      ||
                      ((PE==PA) && ((PF == PC) || (PE == PD)))
                    )
                 {
                      ALPHA_BLEND_128_W(E[1], PB);                      
                 }
            }
            
            if ((PB == PD)&&(PB != PE))
            {
                 if (
                      ((PE==PC) && ((PB == PA) || (PE == PF)))
                      ||
                      ((PE==PG) && ((PB == PA) || (PE == PH)))
                    )                        
                 {
                      ALPHA_BLEND_128_W(E[0], PD);                      
                 }
            }
            
            if ((PD == PH)&&(PD != PE))
            {
                 if (
                      ((PE==PA) && ((PD == PG) || (PE == PB)))
                      ||
                      ((PE==PI) && ((PD == PG) || (PE == PF)))
                    )
                 {
                      ALPHA_BLEND_128_W(E[2], PH);
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
		}

		start_addr2 += complete_line_src;
		start_addr1 = start_addr2 - next_line_src;
		start_addr3 = start_addr2 + next_line_src;
		dst_pixel += complete_line_dst;				
	}
}


