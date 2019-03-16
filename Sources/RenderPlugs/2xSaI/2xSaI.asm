
;/*---------------------------------------------------------------------*
; * The following (piece of) code, (part of) the 2xSaI engine,          *
; * copyright (c) 1999 by Derek Liauw Kie Fa.                           *
; * Non-Commercial use of the engine is allowed and is encouraged,      *
; * provided that appropriate credit be given and that this copyright   *
; * notice will not be removed under any circumstance.                  *
; * You may freely modify this code, but I request                      *
; * that any improvements to the engine be submitted to me, so          *
; * that I can implement these improvements in newer versions of        *
; * the engine.                                                         *
; * If you need more information, have any comments or suggestions,     *
; * you can e-mail me. My e-mail: derek-liauw@usa.net.                  *
; *---------------------------------------------------------------------*/

;----------------------------------------------------------------------------------------------------------------------------
; This code contains minor modifications by Steve Snake.
;----------------------------------------------------------------------------------------------------------------------------

;----------------------------------------------------------------------------------------------------------------------------
; This is version 0.50 of 2xSaI.
;----------------------------------------------------------------------------------------------------------------------------

%ifdef ELF
%define _Render_2xSaI	Render_2xSaI
%define _VideoFormat	VideoFormat
%endif

global _Render_2xSaI
extern	_VideoFormat

inbuffer     equ 8
outbuffer    equ 12
Xres         equ 16
Yres         equ 20
pitch        equ 24
spitch       equ 28

colorI   equ -2
colorE   equ 0
colorF   equ 2
colorJ   equ 4

colorG   equ -2
colorA   equ 0
colorB   equ 2
colorK   equ 4

colorH   equ -2
colorC   equ 0
colorD   equ 2
colorL   equ 4

colorM   equ -2
colorN   equ 0
colorO   equ 2
colorP   equ 4

;----------------------------------------------------------------------------------------------------------------------------
; Vars, probably should be aligned to 64 as they are mostly qwords.
;----------------------------------------------------------------------------------------------------------------------------

section .data align=64

;----------------------------------------------------------------------------------------------------------------------------

colorMask:	dd	0,0
lowPixelMask:	dd	0,0

qcolorMask:	dd	0,0
qlowpixelMask:	dd	0,0

ONE:		dd	000010001h,000010001h

ACPixel:	dd	0,0
Mask1:		dd	0,0
Mask2:		dd	0,0

xloopctr:	dd	0

;----------------------------------------------------------------------------------------------------------------------------

SECTION .text

;----------------------------------------------------------------------------------------------------------------------------

_Render_2xSaI:

	push	ebp
	mov	ebp,esp
	pushad

	cmp	BYTE [_VideoFormat],0
	jnz	Is555

Is565:	mov	edx,0F7DEF7DEh
	mov	eax,colorMask
	mov	[eax],edx
	mov	[eax+4],edx
	mov	edx,008210821h
	mov	eax,lowPixelMask
	mov	[eax],edx
	mov	[eax+4],edx
	mov	edx,0E79CE79Ch
	mov	eax,qcolorMask
	mov	[eax],edx
	mov	[eax+4],edx
	mov	edx,018631863h
	mov	eax,qlowpixelMask
	mov	[eax],edx
	mov	[eax+4],edx
	jmp	Rendr

Is555:	mov	edx,07BDE7BDEh
	mov	eax,colorMask
	mov	[eax],edx
	mov	[eax+4],edx
	mov	edx,004210421h
	mov	eax,lowPixelMask
	mov	[eax],edx
	mov	[eax+4],edx
	mov	edx,0739C739Ch
	mov	eax,qcolorMask
	mov	[eax],edx
	mov	[eax+4],edx
	mov	edx,00C630C63h
	mov	eax,qlowpixelMask
	mov	[eax],edx
	mov	[eax+4],edx

Rendr:	mov	ecx,[ebp+Xres]
	mov	eax,[ebp+inbuffer]
	mov	[xloopctr],ecx
	mov	edx,[ebp+outbuffer]
	mov	ebx,[ebp+spitch]
	mov	ecx,[ebp+Yres]
	mov	ebp,[ebp+pitch]

RendL:	pushad
	mov	ecx,[xloopctr]
	call	_2xSaILine
	popad

	add	eax,ebx
	lea	edx,[edx+ebp*2]
	dec	ecx
	jnz	RendL

	emms
	popad
	pop	ebp
	ret

_2xSaILine:

         ; eax now points to colorE
         sub eax, ebx

; Main Loop

xLoop:	push ecx

         ;--------------------------------------------

;1
;if ((colorA == colorD) && (colorB != colorC) && (colorA == colorE) && (colorB == colorL)

         movq mm0, [eax+ebx+colorA]        ;mm0 and mm1 contain colorA
         movq mm2, [eax+ebx+colorB]        ;mm2 and mm3 contain colorB

         movq mm1, mm0
         movq mm3, mm2

         pcmpeqw mm0, [eax+(ebx*2)+colorD]
         pcmpeqw mm1, [eax+colorE]
         pcmpeqw mm2, [eax+(ebx*2)+colorL]
         pcmpeqw mm3, [eax+(ebx*2)+colorC]

         pand mm0, mm1
         pxor mm1, mm1
         pand mm0, mm2
         pcmpeqw mm3, mm1
         pand mm0, mm3                 ;result in mm0

         ;if ((colorA == colorC) && (colorB != colorE) && (colorA == colorF) && (colorB == colorJ)
         movq mm4, [eax+ebx+colorA]        ;mm4 and mm5 contain colorA
         movq mm6, [eax+ebx+colorB]        ;mm6 and mm7 contain colorB
         movq mm5, mm4
         movq mm7, mm6

         pcmpeqw mm4, [eax+(ebx*2)+colorC]
         pcmpeqw mm5, [eax+colorF]
         pcmpeqw mm6, [eax+colorJ]
         pcmpeqw mm7, [eax+colorE]

         pand mm4, mm5
         pxor mm5, mm5
         pand mm4, mm6
         pcmpeqw mm7, mm5
         pand mm4, mm7                 ;result in mm4

         por mm0, mm4                  ;combine the masks
         movq [Mask1], mm0

         ;--------------------------------------------

;2
;if ((colorB == colorC) && (colorA != colorD) && (colorB == colorF) && (colorA == colorH)

         movq mm0, [eax+ebx+colorB]        ;mm0 and mm1 contain colorB
         movq mm2, [eax+ebx+colorA]        ;mm2 and mm3 contain colorA
         movq mm1, mm0
         movq mm3, mm2

         pcmpeqw mm0, [eax+(ebx*2)+colorC]
         pcmpeqw mm1, [eax+colorF]
         pcmpeqw mm2, [eax+(ebx*2)+colorH]
         pcmpeqw mm3, [eax+(ebx*2)+colorD]

         pand mm0, mm1
         pxor mm1, mm1
         pand mm0, mm2
         pcmpeqw mm3, mm1
         pand mm0, mm3                 ;result in mm0

         ;if ((colorB == colorE) && (colorB == colorD) && (colorA != colorF) && (colorA == colorI)
         movq mm4, [eax+ebx+colorB]        ;mm4 and mm5 contain colorB
         movq mm6, [eax+ebx+colorA]        ;mm6 and mm7 contain colorA
         movq mm5, mm4
         movq mm7, mm6

         pcmpeqw mm4, [eax+(ebx*2)+colorD]
         pcmpeqw mm5, [eax+colorE]
         pcmpeqw mm6, [eax+colorI]
         pcmpeqw mm7, [eax+colorF]

         pand mm4, mm5
         pxor mm5, mm5
         pand mm4, mm6
         pcmpeqw mm7, mm5
         pand mm4, mm7                 ;result in mm4

         por mm0, mm4                  ;combine the masks
         movq [Mask2], mm0


;interpolate colorA and colorB

         movq mm0, [eax+ebx+colorA]
         movq mm1, [eax+ebx+colorB]

         movq mm2, mm0
         movq mm3, mm1

         pand mm0, [colorMask]
         pand mm1, [colorMask]

         psrlw mm0, 1
         psrlw mm1, 1

         pand mm3, [lowPixelMask]
         paddw mm0, mm1

         pand mm3, mm2
         paddw mm0, mm3                ;mm0 contains the interpolated values

         ;assemble the pixels
         movq mm1, [eax+ebx+colorA]
         movq mm2, [eax+ebx+colorB]

         movq mm3, [Mask1]
         movq mm5, mm1
         movq mm4, [Mask2]
         movq mm6, mm1

         pand mm1, mm3
         por mm3, mm4
         pxor mm7, mm7
         pand mm2, mm4

         pcmpeqw mm3, mm7
         por mm1, mm2
         pand mm0, mm3

         por mm0, mm1

         punpcklwd mm5, mm0
         punpckhwd mm6, mm0

         movq [edx], mm5
         movq [edx+8], mm6

;------------------------------------------------
;        Create the Nextline
;------------------------------------------------
;3
;if ((colorA == colorD) && (colorB != colorC) && (colorA == colorG) && (colorC == colorO)

         movq mm0, [eax+ebx+colorA]        ;mm0 and mm1 contain colorA
         movq mm2, [eax+(ebx*2)+colorC]        ;mm2 and mm3 contain colorC
         movq mm1, mm0
         movq mm3, mm2

         push eax
         add eax, ebx
         pcmpeqw mm0, [eax+ebx+colorD]
         pcmpeqw mm1, [eax+colorG]
         pcmpeqw mm2, [eax+(ebx*2)+colorO]
         pcmpeqw mm3, [eax+colorB]
         pop eax

         pand mm0, mm1
         pxor mm1, mm1
         pand mm0, mm2
         pcmpeqw mm3, mm1
         pand mm0, mm3                 ;result in mm0

;if ((colorA == colorB) && (colorG != colorC) && (colorA == colorH) && (colorC == colorM)

         movq mm4, [eax+ebx+colorA]        ;mm4 and mm5 contain colorA
         movq mm6, [eax+(ebx*2)+colorC]        ;mm6 and mm7 contain colorC
         movq mm5, mm4
         movq mm7, mm6

         push eax
         add eax, ebx
         pcmpeqw mm4, [eax+ebx+colorH]
         pcmpeqw mm5, [eax+colorB]
         pcmpeqw mm6, [eax+(ebx*2)+colorM]
         pcmpeqw mm7, [eax+colorG]
         pop eax

         pand mm4, mm5
         pxor mm5, mm5
         pand mm4, mm6
         pcmpeqw mm7, mm5
         pand mm4, mm7                 ;result in mm4

         por mm0, mm4                  ;combine the masks
         movq [Mask1], mm0

         ;--------------------------------------------

;4
;if ((colorB == colorC) && (colorA != colorD) && (colorC == colorH) && (colorA == colorF)

         movq mm0, [eax+(ebx*2)+colorC]        ;mm0 and mm1 contain colorC
         movq mm2, [eax+ebx+colorA]        ;mm2 and mm3 contain colorA
         movq mm1, mm0
         movq mm3, mm2

         pcmpeqw mm0, [eax+ebx+colorB]
         pcmpeqw mm1, [eax+(ebx*2)+colorH]
         pcmpeqw mm2, [eax+colorF]
         pcmpeqw mm3, [eax+(ebx*2)+colorD]

         pand mm0, mm1
         pxor mm1, mm1
         pand mm0, mm2
         pcmpeqw mm3, mm1
         pand mm0, mm3                 ;result in mm0

;if ((colorC == colorG) && (colorC == colorD) && (colorA != colorH) && (colorA == colorI)

         movq mm4, [eax+(ebx*2)+colorC]        ;mm4 and mm5 contain colorC
         movq mm6, [eax+ebx+colorA]        ;mm6 and mm7 contain colorA
         movq mm5, mm4
         movq mm7, mm6

         pcmpeqw mm4, [eax+(ebx*2)+colorD]
         pcmpeqw mm5, [eax+ebx+colorG]
         pcmpeqw mm6, [eax+colorI]
         pcmpeqw mm7, [eax+(ebx*2)+colorH]

         pand mm4, mm5
         pxor mm5, mm5
         pand mm4, mm6
         pcmpeqw mm7, mm5
         pand mm4, mm7                 ;result in mm4

         por mm0, mm4                  ;combine the masks
         movq [Mask2], mm0

         ;----------------------------------------------

;interpolate colorA and colorC

         movq mm0, [eax+ebx+colorA]
         movq mm1, [eax+(ebx*2)+colorC]

         movq mm2, mm0
         movq mm3, mm1

         pand mm0, [colorMask]
         pand mm1, [colorMask]

         psrlw mm0, 1
         psrlw mm1, 1

         pand mm3, [lowPixelMask]
         paddw mm0, mm1

         pand mm3, mm2
         paddw mm0, mm3                ;mm0 contains the interpolated values
         ;-------------

         ;assemble the pixels
         movq mm1, [eax+ebx+colorA]
         movq mm2, [eax+(ebx*2)+colorC]

         movq mm3, [Mask1]
         movq mm4, [Mask2]

         pand mm1, mm3
         pand mm2, mm4

         por mm3, mm4
         pxor mm7, mm7
         por mm1, mm2

         pcmpeqw mm3, mm7
         pand mm0, mm3
         por mm0, mm1
         movq [ACPixel], mm0

;////////////////////////////////
; Decide which "branch" to take
;--------------------------------
         movq mm0, [eax+ebx+colorA]
         movq mm1, [eax+ebx+colorB]
         movq mm6, mm0
         movq mm7, mm1
         pcmpeqw mm0, [eax+(ebx*2)+colorD]
         pcmpeqw mm1, [eax+(ebx*2)+colorC]
         pcmpeqw mm6, mm7

         movq mm2, mm0
         movq mm3, mm0

         pand mm0, mm1       ;colorA == colorD && colorB == colorC
         pxor mm7, mm7

         pcmpeqw mm2, mm7
         pand mm6, mm0
         pand mm2, mm1       ;colorA != colorD && colorB == colorC

         pcmpeqw mm1, mm7

         pand mm1, mm3       ;colorA == colorD && colorB != colorC
         pxor mm0, mm6
         por mm1, mm6
         movq mm7, mm0
         movq [Mask2], mm2
         packsswb mm7, mm7
         movq [Mask1], mm1

         movd ecx, mm7
         test ecx, ecx

		jz	SKIP_GUESS

;---------------------------------------------
; Map of the pixels:                    I|E F|J
;                                       G|A B|K
;                                       H|C D|L
;                                       M|N O|P
         movq mm6, mm0
         movq mm4, [eax+ebx+colorA]
         movq mm5, [eax+ebx+colorB]
         pxor mm7, mm7
         pand mm6, [ONE]

         movq mm0, [eax+colorE]
         movq mm1, [eax+ebx+colorG]
         movq mm2, mm0
         movq mm3, mm1
         pcmpeqw mm0, mm4
         pcmpeqw mm1, mm4
         pcmpeqw mm2, mm5
         pcmpeqw mm3, mm5
         pand mm0, mm6
         pand mm1, mm6
         pand mm2, mm6
         pand mm3, mm6
         paddw mm0, mm1
         paddw mm2, mm3

         pxor mm3, mm3
         pcmpgtw mm0, mm6
         pcmpgtw mm2, mm6
         pcmpeqw mm0, mm3
         pcmpeqw mm2, mm3
         pand mm0, mm6
         pand mm2, mm6
         paddw mm7, mm0
         psubw mm7, mm2

         movq mm0, [eax+colorF]
         movq mm1, [eax+ebx+colorK]
         movq mm2, mm0
         movq mm3, mm1
         pcmpeqw mm0, mm4
         pcmpeqw mm1, mm4
         pcmpeqw mm2, mm5
         pcmpeqw mm3, mm5
         pand mm0, mm6
         pand mm1, mm6
         pand mm2, mm6
         pand mm3, mm6
         paddw mm0, mm1
         paddw mm2, mm3

         pxor mm3, mm3
         pcmpgtw mm0, mm6
         pcmpgtw mm2, mm6
         pcmpeqw mm0, mm3
         pcmpeqw mm2, mm3
         pand mm0, mm6
         pand mm2, mm6
         paddw mm7, mm0
         psubw mm7, mm2

         push eax
         add eax, ebx
         movq mm0, [eax+ebx+colorH]
         movq mm1, [eax+(ebx*2)+colorN]
         movq mm2, mm0
         movq mm3, mm1
         pcmpeqw mm0, mm4
         pcmpeqw mm1, mm4
         pcmpeqw mm2, mm5
         pcmpeqw mm3, mm5
         pand mm0, mm6
         pand mm1, mm6
         pand mm2, mm6
         pand mm3, mm6
         paddw mm0, mm1
         paddw mm2, mm3

         pxor mm3, mm3
         pcmpgtw mm0, mm6
         pcmpgtw mm2, mm6
         pcmpeqw mm0, mm3
         pcmpeqw mm2, mm3
         pand mm0, mm6
         pand mm2, mm6
         paddw mm7, mm0
         psubw mm7, mm2

         movq mm0, [eax+ebx+colorL]
         movq mm1, [eax+(ebx*2)+colorO]
         movq mm2, mm0
         movq mm3, mm1
         pcmpeqw mm0, mm4
         pcmpeqw mm1, mm4
         pcmpeqw mm2, mm5
         pcmpeqw mm3, mm5
         pand mm0, mm6
         pand mm1, mm6
         pand mm2, mm6
         pand mm3, mm6
         paddw mm0, mm1
         paddw mm2, mm3

         pxor mm3, mm3
         pcmpgtw mm0, mm6
         pcmpgtw mm2, mm6
         pcmpeqw mm0, mm3
         pcmpeqw mm2, mm3
         pand mm0, mm6
         pand mm2, mm6
         paddw mm7, mm0
         psubw mm7, mm2

         pop eax
         movq mm1, mm7
         pxor mm0, mm0
         pcmpgtw mm7, mm0
         pcmpgtw mm0, mm1

         por mm7, [Mask1]
         por mm1, [Mask2]
         movq [Mask1], mm7
         movq [Mask2], mm1

SKIP_GUESS:
         ;----------------------------
         ;interpolate A, B, C and D
         movq mm0, [eax+ebx+colorA]
         movq mm1, [eax+ebx+colorB]
         movq mm4, mm0
         movq mm2, [eax+(ebx*2)+colorC]
         movq mm5, mm1
         movq mm3, [qcolorMask]
         movq mm6, mm2
         movq mm7, [qlowpixelMask]

         pand mm0, mm3
         pand mm1, mm3
         pand mm2, mm3
         pand mm3, [eax+(ebx*2)+colorD]

         psrlw mm0, 2
         pand mm4, mm7
         psrlw mm1, 2
         pand mm5, mm7
         psrlw mm2, 2
         pand mm6, mm7
         psrlw mm3, 2
         pand mm7, [eax+(ebx*2)+colorD]

         paddw mm0, mm1
         paddw mm2, mm3

         paddw mm4, mm5
         paddw mm6, mm7

         paddw mm4, mm6
         paddw mm0, mm2
         psrlw mm4, 2
         pand mm4, [qlowpixelMask]
         paddw mm0, mm4      ;mm0 contains the interpolated value of A, B, C and D

;\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
         ;assemble the pixels
         movq mm1, [Mask1]
         movq mm2, [Mask2]
         movq mm4, [eax+ebx+colorA]
         movq mm5, [eax+ebx+colorB]
         pand mm4, mm1
         pand mm5, mm2

         pxor mm7, mm7
         por mm1, mm2
         por mm4, mm5
         pcmpeqw mm1, mm7
         pand mm0, mm1
         por mm4, mm0        ;mm4 contains the diagonal pixels

         movq mm0, [ACPixel]
         movq mm1, mm0
         punpcklwd mm0, mm4
         punpckhwd mm1, mm4

         movq [edx+ebp], mm0
         movq [edx+ebp+8], mm1

         add edx, 16
         add eax, 8

         pop ecx
         sub ecx, 4
         cmp ecx, 0

	jg	xLoop
	ret

;-------------------------------------------------------------------------

