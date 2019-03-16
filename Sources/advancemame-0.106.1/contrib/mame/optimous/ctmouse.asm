;Cute Mouse Driver v1.6 source
;Copyright (c) 1997-1999 Nagy Daniel

;The comments are missing somewhere, excuse me. If you have any comments
;or ideas for heavy optimization, please let me know.
;Compile: tasm /m9 /ml ctmouse.asm
;Link: use any linker that can produce a COM file from OBJ files

;mailto:nagyd@almos.vein.hu
;mailto:heartwork@deathsdoor.com

;Remove the following semicolon before the 'PS2=1' line to assemble the
;driver for PS2 mice

;PS2=1

movSeg		macro	dest, src
		push	src
		pop	dest
	endm

saveFAR		macro	addr, segm, offs
		mov	word ptr addr,offs
		mov	word ptr addr[2],segm
	endm

IFz		macro	var, addr
		cmp	var,0
		jz	addr
	endm

IFnz		macro	var, addr
		cmp	var,0
		jnz	addr
	endm

PS2serv		macro	serv, errlabel
		mov	ax,serv
		int	15h
		jc	errlabel
		or	ah,ah
		jne	errlabel
	endm

driverversion	equ	626h		; Microsoft driver version

.model tiny				; it is a COM file
.code
.286
		org	100h

start:		jmp	real_start

;------ FAR pointer storage ------

oldint10	dd	0		; old INT 10h handler address
oldint33	dd	0		; old INT 33h handler address
oldIRQaddr	dd	0		; old IRQ handler address
newIRQaddr	dd	0		; new IRQ handler address
userproc	dd	0		; user handler address

mem_type	db	0		; 1 = DOS 5.0+ UMB
					; 2 = XMS UMB
					; 3 = conventional memory

;---- driver state begins here -----

StartSaveArea = $

int10pointer	dd	0	; pointer to INT 10 handler
Xmovement	dw	0
Ymovement	dw	0

disabled?	db	0	; indicates if driver disabled/enabled

StartInitArea = $

nowdrawing?	db	0	; indicates if cursor drawing is in progress
userproc?	db	0	; indicates if user defined proc is in progress
regionchk?	db	0	; indicates if have to check region

callmask	dw	0	; user call mask
hrangemin	dw	0	; horizontal range min
vrangemin	dw	0	; vertical range min
hrangemax	dw	0	; horizontal range max
vrangemax	dw	0	; vertical range max
Xcalc		dw	0
Ycalc		dw	0
upleftx		dw	0	; upper left X for updating
uplefty		dw	0	; upper left Y fot updating
lowrightx	dw	0	; lower right X for updating
lowrighty	dw	0	; lower right Y for updating
mickeyX		dw	0
mickeyY		dw	0

newbuttstat	dw	0
buttstatus	dw	0	; button status
butt1press	dw	0	; pressed
butt1pc		dw	0	; column of pressing
butt1pr		dw	0	; row of pressing
butt1rel	dw	0	; released
butt1rc		dw	0	; column of releasing
butt1rr		dw	0	; row of releasing
butt3press	dw	0
butt3pc		dw	0
butt3pr		dw	0
butt3rel	dw	0
butt3rc		dw	0
butt3rr		dw	0
butt2press	dw	0
butt2pc		dw	0
butt2pr		dw	0
butt2rel	dw	0
butt2rc		dw	0
butt2rr		dw	0

cursortype	db	0	; hardware/software

LenInitArea = $ - StartInitArea

column		dw	320
row		dw	64h
column2		dw	320
row2		dw	60h
Xcoord		dw	0	; X coord
Ycoord		dw	0	; Y coord

autores?	db	0	; indicates whether auto/man resolution
lefthand?	db	0	; 0 - right, 1 - left

Xcoordold	dw	320	; old X coordinate
Ycoordold	dw	60h	; hot Y coordinate
h8mickey	dw	8	; horizontal mickeys per 8 pixel
v8mickey	dw	10h	; vertical mickeys per 9 pixel
mul2indic	db	0
hspotcol	dw	0	; hot spot X in cursor bitmap
hspotrow	dw	0	; hot spot Y in cursor bitmap
treshspeed	dw	2	; double threshold speed
lightpen?	db	1	; light pen emulation on/off
startscan	dw	0FFFFh	; cursor start scanline
endscan		dw	7700h	; cursor end scanline
restoreindic	db	1	; indicate to restore screen data (0)
Xcheck		db	0
screenmask	db	48 dup (0)	; user defined screen mask
cursormask	db	48 dup (0)	; user defined cursor mask
grbuf1		dw	72 dup (0)	; screen data
grbuf2		dw	72 dup (0)	; pointer data
activepage	db	0FFh		; active video page
vidmemoffs	dw	0
datav1		db	7 dup (0)
datav1b		db	0,0
mapmask		db	0
datav2		db	5 dup (0)
graphwritemod	db	1
datav3		db	0Fh,0FFh
graphsegindic	db	0		; inicates A000 graphics mode, else 0
VGAindicat	db	0		; is it a VGA card?
EGAport		dw	3D4h		; EGA port address
graphmodeseg	dw	0		; segment of video buffer
ptr2gbuf2	dw	offset grbuf2	; pointer data
ptr2gbuf1	dw	offset grbuf1	; screen data
cursorshown	db	0FFh		; visible? 0 - shown, FF - hidden
rowcheck	db	0

mresolution	dw	0303h		; mouse resolution
IRQintnum	db	0Ch		; INT numer of selected IRQ

IFNDEF PS2
COM_incoming	dw	0		; indicates next incoming serial byte
IO_number	dw	3F8h		; IO port number
PICstate	db	10h		; indicates which bit to clear in PIC
forced		db	0		; Command line force mode?
MSCbuttstate	dw	0		; button state in MSC protocol
logitech?	db	0		; 0 if no, 1 if yes
extrabyte	db	0		; extra byte of MouseMan protocol
LGbstat		db	0		; temporary for MouseMan
X_LO		db	0		; temporary for MouseMan

ELSE
newPS2data	db	0
ENDIF

mousetype	db	2		; Mouse type

shape		dw	03FFFh, 01FFFh, 00FFFh, 007FFh
		dw	003FFh, 001FFh, 000FFh, 0007Fh
		dw	0003Fh, 0001Fh, 001FFh, 000FFh
		dw	030FFh, 0F87Fh, 0F87Fh, 0FCFFh
		dw	00000h, 04000h, 06000h, 07000h
		dw	07800h, 07C00h, 07E00h, 07F00h
		dw	07F80h, 07C00h, 06C00h, 04600h
		dw	00600h, 00300h, 00300h, 00000h

LenSaveArea = $ - StartSaveArea

butpresstatus	dw	offset butt1press	; button 1 press status
		dw	offset butt2press	; button 2
		dw	offset butt3press	; button 3
		dw	offset butt1press	;!!! fool prof protection

butrelstatus	dw	offset butt1rel		; button 1 release status
		dw	offset butt2rel		; button 2
		dw	offset butt3rel		; button 3
		dw	offset butt1rel		;!!! fool prof protection

crightmsg	db	'Cute Mouse Driver v1.6',0
msversion	db	6,26h

;----------------------------

funcsoffsets	dw	offset resetdriver_00
		dw	offset showcursor_01
		dw	offset hidecursor_02
		dw	offset status_03
		dw	offset setpos_04
		dw	offset butpresdata_05
		dw	offset buttreldata_06
		dw	offset hrange_07
		dw	offset vrange_08
		dw	offset graphcursor_09
		dw	offset textcursor_0A
		dw	offset readmcounter_0B
		dw	offset intpar_0C
		dw	offset lightpenon_0D
		dw	offset lightpenoff_0E
		dw	offset micperpixel_0F
		dw	offset defregion_10
		dw	offset nullfunc		;11 - genius driver only
		dw	offset nullfunc		;12 - large graphics cursor
		dw	offset doublespeed_13
		dw	offset exchangeint_14
		dw	offset storagereq_15
		dw	offset savestate_16
		dw	offset restorestate_17
		dw	offset nullfunc		;18 - set alternate handler
		dw	offset nullfunc		;19 - return alternate address
		dw	offset setsens_1A
		dw	offset getsens_1B
		dw	offset nullfunc		;1C - InPort mouse only
		dw	offset setpage_1D
		dw	offset getpage_1E
		dw	offset disabledrv_1F
		dw	offset enabledriver_20
		dw	offset softreset_21
		dw	offset nullfunc		;22 - set language
		dw	offset nullfunc		;23 - get language
		dw	offset getversion_24
		dw	offset nullfunc		;25 - get general information
		dw	offset getmaxvirt_26

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				INT 33 handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

handler		proc	far
		cmp	ax,26h			; is it implemented?
		ja	@checkother		; jump if not

		sti
		cld
		push	ax bx cx dx ds es di si bp
		mov	bp,sp
		movSeg	ds,cs
		shl	ax,1
		mov	si,ax
		call	funcsoffsets[si]	; call by calculated offset
IFNDEF PS2
		movSeg	ds,cs
		IFnz	disabled?,@rethandler
		call	enableCOMint
ENDIF

@rethandler:	pop	bp si di es ds dx cx bx ax
		iret

@checkother:	cmp	ax,4dh			; version string query?
		je	@msstr
		cmp	ax,6dh			; ms version number?
		je	@msver

;------------ Old handler

		IFz	<cs:word ptr oldint33[2]>,@iret ; is there another
							; INT 33 handler?
		jmp	cs:oldint33		; call it if there is

;------------ Version string

@msstr:		mov	di,offset crightmsg
@retstr:	movSeg	es,cs
@iret:		iret

;------------ Version number

@msver:		mov	di,offset msversion
		jmp	@retstr
handler		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 00 - Reset driver
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[AX] = 0/FFFFh				(status)
;	[BX] = 0/2/3/FFFFh			(number of buttons)
; Use:	none
; Modf: none
; Call: checkPS2, disableCOMint, chkcom, setCOMparams,
;	enabledriver_20, softreset_21
;
resetdriver_00	proc
IFDEF PS2
		call	checkPS2
		jc	@ret
ELSE
		call	disableCOMint
		call	chkcom
		jnz	@ret			; jump if port not available
		call	setCOMparams
ENDIF
		call	enabledriver_20
		jmp	softreset_21
resetdriver_00	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 01 - Show mouse cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	maxycoord
; Modf: AX, regionchk?, lowrighty, cursorshown
; Call: showpointer
;
showcursor_01	proc
		mov	regionchk?,0
		mov	ax,maxycoord		;*** ??? ***
		add	ax,1Fh			;*** ??? ***
		mov	lowrighty,ax		;*** ??? ***
		IFz	cursorshown,@ret	; jump if already shown
		inc	cursorshown
		jmp	@showptr
showcursor_01	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 02 - Hide mouse cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	none
; Modf: ES, cursorshown
; Call: restorescreen
;
hidecursor_02	proc
		dec	cursorshown
		movSeg	es,cs			;*** ??? ***
		jmp	restorescreen
hidecursor_02	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 03 - Return position and button status
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[BX]					(buttons status)
;	[CX]					(column)
;	[DX]					(row)
; Use:	column2, row2, buttstatus, lefthand?
; Modf: AX
; Call: none
;
status_03	proc
		cli
		mov	ax,column2
		mov	[bp+0Ch],ax
		mov	ax,row2
		mov	[bp+0Ah],ax
		mov	ax,buttstatus
		call	swap2lowbits
		mov	[bp+0Eh],ax
@ret:		ret
status_03	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 04 - Position mouse cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(column)
;	DX					(row)
; Out:	none
; Use:	vrangemin, vrangemax, hrangemin, hrangemax,
;	colgranu, rowgranu, cursorshown
; Modf: AX, DX, SI, DI, column, row, column2, row2,
;	cursortype, startscan, endscan
; Call: cutrange, showpointer
;
setpos_04	proc
		mov	ax,dx
		mov	si,vrangemin
		mov	di,vrangemax
		call	cutrange
		mov	bx,ax

		mov	ax,cx
		mov	si,hrangemin
		mov	di,hrangemax
		call	cutrange

@pcurs:		cli
		mov	column,ax
		xor	dx,dx
		div	colgranu
		mul	colgranu
		mov	column2,ax

		mov	ax,bx
		mov	row,ax
		div	rowgranu
		mul	rowgranu
		mov	row2,ax

		IFnz	cursorshown,@ret
		mov	cursortype,0		;*** ??? ***
		mov	startscan,77FFh		;*** ??? ***
		mov	endscan,7700h		;*** ??? ***
		jmp	showpointer
setpos_04	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 05 - Return button press data
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	BX					(button number)
; Out:	[AX]					(buttons states)
;	[BX]					(press times)
;	[CX]					(last press column)
;	[DX]					(last press row)
; Use:	butpresstatus, buttstatus, lefthand?
; Modf: AX, BX
; Call: none
;
butpresdata_05	proc
		and	bx,7
		shl	bx,1
		mov	bx,butpresstatus[bx]
		jmp	@retbuttstat
butpresdata_05	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 06 - Return button release data
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	BX					(button number)
; Out:	[AX]					(buttons states)
;	[BX]					(release times)
;	[CX]					(last release column)
;	[DX]					(last release row)
; Use:	butrelstatus, buttstatus, lefthand?
; Modf: AX, BX
; Call: none
;
buttreldata_06	proc
		and	bx,7
		shl	bx,1
		mov	bx,butrelstatus[bx]

@retbuttstat:	cli
		xor	ax,ax
		xchg	[bx],ax
		mov	[bp+0Eh],ax
		mov	ax,[bx+2]
		mov	[bp+0Ch],ax
		mov	ax,[bx+4]
		mov	[bp+0Ah],ax
		mov	ax,buttstatus
		call	swap2lowbits
		mov	[bp+10h],ax
@ret2:		ret
buttreldata_06	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 07 - Define horizontal cursor range
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(min row)
;	DX					(max row)
; Out:	none
; Use:	column, row
; Modf: CX, DX, hrangemin, hrangemax
; Call: setpos_04
;
hrange_07	proc
		cmp	dx,cx
		jb	@swminmaxh
		xchg	dx,cx

@swminmaxh:	cli
		mov	hrangemin,dx
		mov	hrangemax,cx
@hsetpos:	mov	cx,column
		mov	dx,row
		jmp	setpos_04
hrange_07	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 08 - Define vertical cursor range
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(min column)
;	DX					(max column)
; Out:	none
; Use:	column, row
; Modf: CX, DX, vrangemin, vrangemax
; Call: setpos_04
;
vrange_08	proc
		cmp	dx,cx
		jb	@swminmaxv
		xchg	dx,cx

@swminmaxv:	cli
		mov	vrangemin,dx
		mov	vrangemax,cx
		jmp	@hsetpos
vrange_08	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 09 - Define graphics cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	BX					(hot spot column)
;	CX					(hot spot row)
;	ES:DX					(pointer to bitmaps)
; Out:	none
; Use:	colgranu, cursorshown
; Modf: AX, ES, DS, SI, hspotrow, hspotcol
; Call: restorescreen, setusershape, showpointer
;
graphcursor_09	proc
		push	dx es
		mov	hspotrow,cx
		mov	ax,bx			;*** ??? ***
		xor	dx,dx			;*** ??? ***
		div	colgranu		;*** ??? ***
		mul	colgranu		;*** ??? ***
		mov	hspotcol,ax

		movSeg	es,cs			;*** ??? ***
		call	restorescreen
		pop	ds si
		call	setusershape
		movSeg	ds,cs

@showptr:	IFnz	cursorshown,@ret2	; jump if hidden
		jmp	showpointer		; else redisplay
graphcursor_09	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 0A - Define text cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(start scanline/screen mask)
;	DX					(end scanline/cursor mask)
;	BX = 0/1				(HW/SW text cursor)
; Out:	none
; Use:	none
; Modf: AX, CX, cursortype, startscan, endscan, cursorshown
; Call: showpointer
;
textcursor_0A	proc
		mov	cursortype,bl
		mov	startscan,cx
		mov	endscan,dx
		test	bl,bl			; software cursor?
		jz	@showptr

		mov	ch,cl
		mov	cl,dl
		and	cx,0F0Fh
		mov	ah,1
		int	10h			; set cursor mode in cx
		ret
textcursor_0A	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 0B - Read motion counters
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[CX]			(number of mickeys mouse moved
;	[DX]			 horizontally/vertically since last call)
; Use:	none
; Modf: AX, mickeyX, mickeyY
; Call: none
;
readmcounter_0B	proc
		cli
		xor	ax,ax
		xchg	mickeyX,ax
		mov	[bp+0Ch],ax
		xor	ax,ax
		xchg	mickeyY,ax
		mov	[bp+0Ah],ax
		ret
readmcounter_0B	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 0C - Define interrupt subroutine parameters
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(call mask)
;	ES:DX					(FAR routine)
; Out:	none
; Use:	lefthand?
; Modf: CX, DX, userproc, callmask
; Call: none
;
intpar_0C	proc
		cli
		saveFAR userproc,es,dx
		call	swap12to34bits
		mov	callmask,cx
		ret
intpar_0C	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 0D - Light pen emulation On
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	none
; Modf: lightpen?
; Call: none
;
lightpenon_0D	proc
		mov	lightpen?,1
		ret
lightpenon_0D	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 0E - Light pen emulation Off
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	none
; Modf: lightpen?
; Call: none
;
lightpenoff_0E	proc
		mov	lightpen?,0FFh
		ret
lightpenoff_0E	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 0F - Define Mickey/Pixel ratio
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(number of mickeys per 8 pix
;	DX					 horizontally/vertically)
; Out:	none
; Use:	none
; Modf: h8mickey, v8mickey
; Call: none
;
micperpixel_0F	proc
		cli
		mov	h8mickey,cx
		mov	v8mickey,dx
		ret
micperpixel_0F	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 10 - Define screen region for updating
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX, DX					(X/Y of upper left corner)
;	SI, DI					(X/Y of lower right corner)
; Out:	none
; Use:	cursorshown
; Modf: CX, DX, SI, DI, lowrightx, upleftx,
;	lowrighty, uplefty, regionchk?
; Call: showpointer
;
defregion_10	proc
		cmp	si,cx
		jb	@nosw1
		xchg	si,cx
@nosw1:		cmp	di,dx
		jb	@nosw2
		xchg	di,dx
@nosw2:		cli
		mov	lowrightx,si
		mov	upleftx,cx
		mov	lowrighty,di
		mov	uplefty,dx
		mov	regionchk?,1
		jmp	@showptr
defregion_10	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 14 - Exchange interrupt subroutines
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(new call mask)
;	ES:DX					(new FAR routine)
; Out:	[CX]					(new call mask)
;	[ES:DX]					(old FAR routine)
; Use:	none
; Modf: AX, BX, CX, ES, callmask, userproc
; Call: none
;
exchangeint_14	proc
		cli
		push	es cx
		mov	cx,callmask
		call	swap12to34bits
		mov	[bp+0Ch],cx
		les	ax,userproc
		mov	[bp+0Ah],ax
		mov	[bp+6],es
		pop	cx es
		jmp	intpar_0C
exchangeint_14	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 15 - Return driver storage requirements
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[BX]					(buffer size)
; Use:	none
; Modf: none
; Call: none
;
storagereq_15	proc
		mov	word ptr [bp+0Eh],LenSaveArea+1
		ret
storagereq_15	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 16 - Save driver state
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	BX					(buffer size)
;	ES:DX					(buffer)
; Out:	none
; Use:	none
; Modf: CX, SI, DI
; Call: none
;
savestate_16	proc
		mov	di,dx
		mov	si,offset cursorshown
		movsb
		mov	si,offset StartSaveArea
@savemov:	mov	cx,LenSaveArea
		rep	movsb
		ret
savestate_16	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 17 - Restore driver state
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	BX					(buffer size)
;	ES:DX					(saved state buffer)
; Out:	none
; Use:	none
; Modf: CX, SI, DI, DS, ES
; Call: none
;
restorestate_17	proc
		push	ds es
		pop	ds es
		mov	si,dx
		mov	di,offset cursorshown
		movsb
		mov	di,offset StartSaveArea
		jmp	@savemov
restorestate_17	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 1A - Set mouse sensitivity
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	BX			(number of mickeys per 8 pix
;	CX			 horizontally/vertically)
;	DX			(threshold speed in mickeys/second)
; Out:	none
; Use:	none
; Modf: AX, DX, BX, h8mickey, v8mickey, treshspeed
; Call: none
;
setsens_1A	proc
		cli
		mov	h8mickey,bx
		mov	v8mickey,cx

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 13 - Define double speed threshold
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	DX			(threshold speed in mickeys/second)
; Out:	none
; Use:	none
; Modf: AX, BX, DX, treshspeed
; Call: none
;
doublespeed_13	proc
		mov	ax,dx
		xor	dx,dx
		mov	bx,30
		div	bx
		mov	treshspeed,ax
		ret
doublespeed_13	endp

setsens_1A 	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 1B - Return mouse sensitivity
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[BX]			(number of mickeys per 8 pix
;	[CX]			 horizontally/vertically)
;	[DX]			(threshold speed in mickeys/second)
; Use:	h8mickey, v8mickey, treshspeed
; Modf: AX, DX
; Call: none
;
getsens_1B	proc
		cli
		mov	ax,h8mickey
		mov	[bp+0Eh],ax
		mov	ax,v8mickey
		mov	[bp+0Ch],ax
		mov	ax,30
		mul	treshspeed
		mov	[bp+0Ah],ax
		ret
getsens_1B	endp


;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 1D - Define display page number
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	BX					(display page number)
; Out:	none
; Use:	0:44Ch
; Modf: AX, BX, DX, ES, activepage, vidmemoffs
; Call: none
;
setpage_1D	proc
		and	bx,1Fh
		mov	activepage,bl
		xor	ax,ax
		mov	es,ax
		mov	ax,es:[44ch]
		mul	bx
		mov	vidmemoffs,ax
		ret
setpage_1D	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 1E - Return display page number
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[BX]					(display page number)
; Use:	0:462h
; Modf: AX, ES
; Call: none
;
getpage_1E	proc
		mov	al,activepage
		cmp	al,0FFh
		jne	@penab
		xor	ax,ax
		mov	es,ax
		mov	al,es:[462h]
@penab:		xor	ah,ah
		mov	[bp+0Eh],ax
		ret
getpage_1E	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 1F - Disable mouse driver
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[AX] = 1/FFFFh				(status)
;	[ES:BX]					(old int33 handler)
; Use:	oldint33, IRQintnum, oldIRQaddr, oldint10
; Modf: AX, DX, ES, disabled?
; Call: disablePS2, disableCOMint
;
disabledrv_1F	proc
		IFnz	disabled?,@disabd	; jump if already disabled
IFDEF PS2
		call	disablePS2
ELSE
		call	disableCOMint
ENDIF
;------------ restore old IRQ handler

		push	ds
		mov	al,IRQintnum
		mov	ah,25h
		lds	dx,oldIRQaddr
		int	21h			; set intrpt vector al to ds:dx
		pop	ds

;------------ set up INT 10 handler

		push	ds
		lds	dx,oldint10		; set intrpt vector to old10
		mov	ax,2510h
		int	21h
		pop	ds
;------------ 
		mov	disabled?,1

@disabd:	les	ax,oldint33
		mov	[bp+0Eh],ax
		mov	[bp+06],es
		ret
disabledrv_1F	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 20 - Enable mouse driver
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[AX] = 20h/FFFFh			(status)
; Use:	int10pointer
; Modf: disabled?
; Call: disablePS2, disableCOMint, setCOMparams,
;	setnewIRQh, enablePS2, enableCOMint
;
enabledriver_20	proc
IFDEF PS2
		call	disablePS2
ELSE
		call	disableCOMint
ENDIF
		IFz	disabled?,@enabd
IFNDEF PS2
		call	setCOMparams
ENDIF
		push	ds
		lds	dx,int10pointer
		mov	ax,2510h
		int	21h			; set new INT 10 handler
		pop	ds

@enabd:		call	setnewIRQh
		mov	disabled?,0
IFDEF PS2
		jmp	enablePS2
ELSE
		jmp	enableCOMint
ENDIF
enabledriver_20	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 21 - Software reset
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[AX] = 21/FFFFh				(status)
;	[BX] = 0/2/3/FFFFh			(number of buttons)
; Use:	mousetype, maxycoord, maxxcoord
; Modf: AX, BX, CX, ES, DI, startscan, endscan, h8mickey, v8mickey,
;	treshspeed, vrangemax, hrangemax, cursorshown, lowrighty
; Call: setvidparams, graphcursor_09, hidecursor_02, @pcurs
;
softreset_21	proc
		mov	word ptr [bp+0Eh],2
IFNDEF PS2
		cmp	mousetype,3		; Mouse Systems mouse?
		jne	@no3but
		inc	word ptr [bp+0Eh]
ENDIF

@no3but:	mov	word ptr [bp+10h],0FFFFh
		mov	ah,0Fh
		int	10h			; get state, al=mode, bh=page
						; ah=columns on screen
		call	setvidparams

		movSeg	es,cs
		xor	al,al
		mov	di,offset StartInitArea
		mov	cx,LenInitArea
		rep	stosb

		mov	startscan,77FFh
		mov	endscan,7700h
		mov	h8mickey,8
		mov	v8mickey,16
		mov	treshspeed,2
		mov	ax,maxycoord
		dec	ax
		mov	vrangemax,ax
		mov	ax,maxxcoord
		dec	ax
		mov	cl,mul2indic
		shl	ax,cl
		mov	hrangemax,ax
		xor	cx,cx
		mov	bx,cx
		mov	dx,offset shape
		call	graphcursor_09
		mov	cursorshown,0
		call	hidecursor_02
		mov	bx,maxycoord
		mov	cx,bx			;*** ??? ***
		add	cx,1Fh			;*** ??? ***
		mov	lowrighty,cx		;*** ??? ***
		mov	ax,maxxcoord
		shr	ax,1
		shr	bx,1
		call	@pcurs
		mov	cursorshown,0FFh
		ret
softreset_21	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 24 - Get software version, mouse type and IRQ
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[AX]					(status)
;	[BX]					(version)
;	[CH]					(mouse type)
;	[CL]					(interrupt No)
; Use:	driverversion, mousetype, IRQintnum
; Modf: AX
; Call: none
;
getversion_24	proc
		mov	ax,driverversion	; version number
		mov	[bp+0Eh],ax
IFNDEF PS2
		mov	ah,2
		mov	al,IRQintnum
		sub	al,8
ELSE
		mov	ah,mousetype
		xor	al,al
ENDIF
		mov	[bp+0Ch],ax
		ret
getversion_24	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 26 - Get maximum virtual coordinates
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	[BX]					(mouse disabled flag)
;	[CX]					(maximum virtual X)
;	[DX]					(maximum virtual Y)
; Use:	disabled?, maxxcoord, maxycoord
; Modf: AX
; Call: none
;
getmaxvirt_26	proc
		xor	ah,ah
		mov	al,disabled?
		mov	[bp+0Eh],ax
		mov	ax,maxxcoord
		dec	ax
		mov	[bp+0Ch],ax
		mov	ax,maxycoord
		dec	ax
		mov	[bp+0Ah],ax

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; 11, 18, 19, 1C, 22, 23, 25 - Null function for not implemented calls
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

nullfunc	proc
		ret
nullfunc	endp

getmaxvirt_26	endp


;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;€€€€€€€€€€€€€€€€€€€€€€€€€ END OF INT 33 SERVICES €€€€€€€€€€€€€€€€€€€€€€€€€
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹


;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Swap two lower bits of buttons status
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	AX					(buttons status)
; Out:	AX
; Use:	none
; Modf: none
; Call: none
;
swap2lowbits	proc
		IFz	lefthand?,@rhand
		ror	ax,1			; 165_432
		ror	al,1			; 165_243
		rol	ax,1			; 652_431
		ror	ah,1			; 265_431
		rol	ax,1			; 654_312
@rhand:		ret
swap2lowbits	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Swap 1-2 with 3-4 bits of buttons status
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CX					(buttons status)
; Out:	CX
; Use:	none
; Modf: none
; Call: none
;
swap12to34bits	proc
		IFz	lefthand?,@rhand2
		ror	cx,3			; 210FEDCB_A9876543
		ror	cl,2			; 210FEDCB_43A98765
		rol	cx,2			; 0FEDCB43_A9876521
		ror	ch,2			; 430FEDCB_A9876521
		rol	cx,3			; FEDCBA98_76521430
@rhand2:	ret
swap12to34bits	endp

IFNDEF PS2
;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Set communication parameters (speed, parity, etc.)
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	IO_number, mousetype, logitech?
; Modf: AX, DX
; Call: none
;
setCOMparams	proc
		cli
		mov	al,80h
		mov	dx,IO_number
		add	dx,3
		out	dx,al			; set DLAB on

		mov	al,60h
		sub	dx,3
		out	dx,al			; speed LO byte

		xor	al,al
		inc	dx
		out	dx,al			; speed HI byte

		mov	al,mousetype
		IFz	logitech?,@nologicor
		dec	al

@nologicor:	inc	dx
		inc	dx
		out	dx,al			; set comm params and DLAB=0
		mov	al,0Bh
		inc	dx
		out	dx,al			; reset hardware
						; (Activate DTR, RTS and OUT2)
		mov	al,1
		sub	dx,3
		out	dx,al			; DR int enable

		add	dx,4
		in	al,dx			; read LSR thus clearing error
		sub	dx,5
		in	al,dx			; flush reveive buffer

		sti
		ret
setCOMparams	endp

ENDIF

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Set new IRQ handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	IRQintnum, newIRQaddr
; Modf: AL, DX
; Call: none
;
setnewIRQh	proc
		push	ds
		mov	al,IRQintnum
		mov	ah,25h
		lds	dx,newIRQaddr
		int	21h			; set intrpt vector al to ds:dx
		pop	ds
		ret
setnewIRQh	endp

IFNDEF PS2
;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Enable serial interrupt in PIC
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	PICstate
; Modf: AX
; Call: none
;
enableCOMint	proc
		cli
		in	al,21h			; port 21h, int IMR
		mov	ah,PICstate
		not	ah
		and	al,ah
@outserial:	out	21h,al			; enable serial interrupt
		sti
		ret
enableCOMint	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Disable serial interrupt of PIC
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	PICstate
; Modf: AL
; Call: none
;
disableCOMint	proc
		cli
		in	al,21h			; get PIC mask in al
		or	al,PICstate
		jmp	@outserial		; disable serial port interrupt
disableCOMint	endp

ELSE
;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				Disable PS2
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	none
; Modf: AX, BX, ES
; Call: none
;
disablePS2	proc
		cli
		mov	bh,0
		mov	ax,0C200h
		int	15h			; set mouse off
		xor	bx,bx
		mov	es,bx
		mov	ax,0C207h
		int	15h			; es:bx=ptr to handler
		sti
		ret
disablePS2	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				EnablePS2
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	none
; Use:	none
; Modf: AX, BX, ES
; Call: none
;
enablePS2	proc
		cli
		movSeg	es,cs
		mov	bx,offset IRQhandler
		mov	ax,0C207h
		int	15h			; es:bx=ptr to handler
		mov	bh,1
		mov	ax,0C200h
		int	15h			; set mouse on
		sti
		ret
enablePS2	endp
ENDIF

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Set up user defined graphics cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	DS:SI					(pointer to bitmaps)
; Out:	none
; Use:	none
; Modf: BX, ES, SI, DI, screenmask, cursormask, Xcheck
; Call: none
;
setusershape	proc
		movSeg	es,cs
		mov	bx,0FFFFh
		lea	di,screenmask		; copy screen mask
		call	@copybitmap

		xor	bx,bx
		mov	cs:Xcheck,bl
		lea	di,cursormask		; copy cursor mask

@copybitmap:	mov	cx,16
@loccp:		lodsw
		xchg	al,ah
		stosw
		mov	al,bl
		stosb
		loop	@loccp
		ret
setusershape	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Compare with maximum ranges
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	SI					(minimum)
;	DI					(maximum)
;	AX					(current)
; Out:	AX
; Use:	none
; Modf: none
; Call: none
;
cutrange	proc
		cmp	ax,si
		jl	@toosmall
		cmp	ax,di
		jle	@valueOK
		mov	ax,di
		ret
@toosmall:	mov	ax,si
@valueOK:	ret
cutrange	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Draw mouse pointer
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

showpointer	proc
		mov	nowdrawing?,1		; indicate drawing
		cmp	rowgranu,1		; text mode?
		jne	@textshp		; jump if yes
		call	drawgraphcurs		; else draw graphics cursor
		jmp	@graphshp

@textshp:	call	drawtextcursor
@graphshp:	mov	nowdrawing?,0		; drawing stopped
		ret
showpointer	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Draw text mode cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

drawtextcursor	proc
		mov	ax,row2
		mov	bx,column2		; get coordinates
		IFz	cursortype,@swcur	; jump if software cursor
		call	drawHWcursor		; else draw hardware text cursor
		jmp	@exitdwcurs		; and exit

@swcur:		call	softtextcurs		; draw software text cursor

@exitdwcurs:	cli
		mov	ax,Xcoord		; update hot spot params
		mov	Xcoordold,ax
		mov	ax,Ycoord
		mov	Ycoordold,ax
		sti
		ret
drawtextcursor	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Draw hardware text mode cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

drawHWcursor	proc
		call	calcaddress
		shr	di,1
		mov	bx,di
		mov	dx,EGAport
		mov	al,0Fh
		out	dx,al			; al = 0Fh, cursor position lo
		inc	dx
		mov	al,bl
		out	dx,al
		dec	dx
		mov	al,0Eh
		out	dx,al			; al = 0Eh, cursor position hi
		mov	al,bh
		inc	dx
		out	dx,al
		ret
drawHWcursor	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Draw software text mode cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

softtextcurs	proc
		cli
		mov	Ycoord,ax
		mov	Xcoord,bx
		sti
		call	restorescreen
		mov	ax,Ycoord
		mov	bx,Xcoord
		mov	cx,ax
		mov	dx,bx
		add	cx,pixboxwidth
		add	dx,pixboxheight
		call	checkregion		; out of update region?
		jc	@exitswcurs

		call	calcaddress
		cli
		call	wait_VRT
		mov	bx,es:[di]		; store char under cursor
		sti
		mov	grbuf1,bx
		and	bx,startscan
		xor	bx,endscan
		mov	es:[di],bx		; draw to new position
		mov	restoreindic,0		; we have to restore later

@exitswcurs:	ret
softtextcurs	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Draw graphics cursor
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

drawgraphcurs	proc
		cli
		mov	ax,row2
		sub	ax,hspotrow
		mov	Ycoord,ax		; Y calculated

		mov	bx,column2
		sub	bx,hspotcol
		mov	cl,mul2indic
		shr	bx,cl
		mov	Xcoord,bx		; X calculated

		sti
		IFz	regionchk?,@notout	; have to check region?
						; jump if not
		mov	cx,ax
		add	cx,10h
		sub	ax,8
		and	bl,0F8h
		mov	dx,bx
		add	dx,18h
		call	checkregion		; out of update region?
		jc	restorescreen

@notout:	call	graphportparams
		mov	ax,Ycoord
		mov	bx,Xcoord
		cmp	ax,Ycoordold
		jne	@moved
		cmp	bx,Xcoordold
		jne	@moved
		IFz	restoreindic,@nomoved

@moved:		mov	cx,Ycoordold
		cmp	cx,maxycoord
		jg	@dontrest
		call	restoreoldscr		; restore old screen content
@dontrest:	mov	ax,Xcoord		; store bitmap under new pos
		mov	si,Ycoord
		call	retvidmem
		mov	di,ptr2gbuf1
		mov	dx,Ycoord
		mov	rowcheck,0
		mov	es,graphmodeseg
		mov	ds,videoseg
		call	copyscrmap
		call	setsegs

@nomoved:	call	trans			; transform
		call	setsegs			; CS=DS=ES
		cli
		mov	ax,Xcoord
		mov	Xcoordold,ax
		mov	si,Ycoord
		mov	Ycoordold,si
		sti
		mov	di,ptr2gbuf2
		call	spritecopy		; draw pointer
		mov	restoreindic,0
		jmp	restorevidport
drawgraphcurs	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

restorescreen	proc
		mov	nowdrawing?,1
		call	graphportparams
		call	restoreoldscr
		call	restorevidport
		mov	nowdrawing?,0
		ret
restorescreen	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Wait for VRT in text modes
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

wait_VRT	proc
		mov	ax,es
		cmp	ax,0B800h
		jne	@notextmod

		push	ds
		xor	ax,ax
		mov	ds,ax
		mov	dx,ds:[463h]		; video port
		pop	ds
		add	dx,6

;---- Wait for vertical retrace ----

@vrt1:		in	al,dx
		test	al,1
		jnz	@vrt1

@vrt2:		in	al,dx
		test	al,1
		jz	@vrt2

@notextmod:	ret
wait_VRT	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Restore old screen contents
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

restoreoldscr	proc
		IFnz	restoreindic,@norestor	; do we have to restore
						; jump if not
		mov	restoreindic,1		; clear indicator
		cmp	rowgranu,1		; text mode?
		jne	@textmod		; jump if yes

		mov	di,ptr2gbuf1
		mov	ax,Xcoordold
		mov	si,Ycoordold		; restore graphic mode
		jmp	spritecopy		;  screen data

@textmod:	IFnz	cursortype,@norestor	; exit if hardware cursor

		mov	ax,Ycoordold
		mov	bx,Xcoordold
		call	calcaddress
		mov	bx,grbuf1
		cli
		call	wait_VRT
		mov	es:[di],bx		; restore old text char attrib
		sti
		movSeg	es,cs
@norestor:	ret
restoreoldscr	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Copy sprite back and forth
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

spritecopy	proc
		call	retvidmem
		xchg	di,si
		mov	dx,Ycoordold
		mov	rowcheck,1
		mov	es,videoseg
		mov	ds,graphmodeseg
		call	copyscrmap

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Set segment registers equal to CS
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	CS
; Out:	DS ES
; Use:	none
; Modf: none
; Call: none
;
setsegs		proc
		push	cs cs
		pop	ds es
		ret
setsegs		endp

spritecopy	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;	Copies pointer or under-pointer sprites back and forth
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	DX				(Y coordinate)
;
copyscrmap	proc
		cmp	dx,cs:maxycoord
		jge	@maxed
		mov	ax,cs:nextrowoffset
		mov	bx,cs:nextpagoffset
		mov	cx,dx
		add	cx,16
		cmp	cx,cs:maxycoord
		jl	@nomaxx
		mov	cx,cs:maxycoord

@nomaxx:	sub	cx,dx

@dwrow:		push	cx
		mov	cx,3
		cmp	cs:videomode,13h
		jne	@not13m
		mov	cx,16

@not13m:	rep	movsb
		pop	cx

		IFz	cs:rowcheck,@norowck
		add	di,ax
		cmp	di,bx
		jb	@looprow
		sub	di,bx
		jmp	@looprow

@norowck:	add	si,ax
		cmp	si,bx
		jb	@looprow
		sub	si,bx
@looprow:	loop	@dwrow

@maxed:		ret
copyscrmap	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

trans		proc
		cmp	videomode,13h
		je	@tvid13
		call	trans2

@tvid13:	mov	si,ptr2gbuf1		; copy saved buffer to
		mov	di,ptr2gbuf2		;  buffer 2 because we will
		push	di ds			;  transform there
		mov	cx,100h
		cmp	videomode,13h
		je	@tvid13_2
		mov	cx,30h
@tvid13_2:	mov	es,graphmodeseg
		mov	ds,graphmodeseg
		rep	movsb
		pop	ds di

		mov	dx,Ycoord
		mov	si,Xcoord
		cmp	cs:videomode,13h
		je	@tvid13_3
		and	si,0FFF8h
@tvid13_3:	xor	bx,bx
		call	setvidpar
		call	transform
		jmp	resvidpar
trans		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

setvidpar	proc
		push	dx
		IFz	cs:graphsegindic,@nogrv
		mov	dx,3CEh
		mov	ax,5
		cmp	cs:videomode,14h
		jae	@ta14h
		mov	ah,graphwritemod
		and	ah,0FEh
		out	dx,ax			; set write mode
		mov	al,3
		out	dx,al			; set data rotate mode
		mov	dx,Ycoord
		jmp	@nogrv

@ta14h:		out	dx,ax			; set graphics mode
		mov	al,8
		out	dx,al			; set data bit mask
@nogrv:		pop	dx
		ret
setvidpar	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

resvidpar	proc
		push	dx
		IFz	cs:graphsegindic,@resnogr
		mov	dx,3CFh
		xor	al,al
		out	dx,al
		dec	dx
		mov	al,5
		mov	ah,graphwritemod
		out	dx,al			; select mode
		inc	dx
		mov	al,ah
		out	dx,al			; set write mode

@resnogr:	pop	dx
		ret
resvidpar	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

transform	proc
		mov	cx,10h			; 16 rows

@loopytr:	test	dx,dx
		jl	@noloopdx
		cmp	dx,maxycoord		; exit if out of screen
		jg	@moreymax

		push	bx cx di si
		mov	cx,3
		cmp	cs:videomode,13h
		jne	@noloop13
		dec	cx

@nextx13tr:	call	mode13trans
		inc	bx
		loop	@nextx13tr

		jmp	@nextytr

@noloop13:	call	modeno13curs
		inc	bx
		loop	@noloop13

@nextytr:	pop	si di cx bx

@noloopdx:	mov	ax,3
		cmp	cs:videomode,13h
		jne	@no13lo
		mov	ax,10h

@no13lo:	add	di,ax
		add	bx,3
		inc	dx
		loop	@loopytr

@moreymax:	ret
transform	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Transform the pointer mask to screen content
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

mode13trans	proc
		push	cx dx
		mov	dh,screenmask[bx]
		mov	dl,cursormask[bx]
		mov	cx,8			; 2*8 bytes per row

@nextrb:	test	si,si
		jl	@maxxskip
		cmp	si,maxxcoord
		jge	@maxxskip
		mov	al,es:[di]
		shl	dh,1
		jc	@pthere
		xor	al,al
@pthere:	shl	dl,1
		jnc	@nopthere
		xor	al,0Fh
@nopthere:	mov	es:[di],al
@maxxskip:	inc	di
		inc	si
		loop	@nextrb

		pop	dx cx
		ret
mode13trans	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Display cursor in other than mode 13 modes
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

modeno13curs	proc
		push	dx
		test	si,si
		jl	@skipn13x
		cmp	si,maxxcoord
		jge	@skipn13x

		IFnz	cs:graphsegindic,@textn13
		mov	al,es:[di]
		and	al,screenmask[bx]
		xor	al,cursormask[bx]
		mov	es:[di],al
		jmp	@skipn13x

@textn13:	mov	dx,3CFh
		cmp	cs:videomode,14h
		jb	@b14tr
		mov	al,screenmask[bx]
		out	dx,al
		mov	al,es:[di]
		mov	al,cursormask[bx]
		mov	es:[di],al
		jmp	@skipn13x

@b14tr:		mov	al,8			; data ANDed with latched data
		out	dx,al
		mov	al,es:[di]
		mov	al,screenmask[bx]
		mov	es:[di],al
		mov	al,18h			; data XORed with latched data
		out	dx,al
		mov	al,es:[di]
		mov	al,cursormask[bx]
		mov	es:[di],al

@skipn13x:	inc	di
		add	si,8
		pop	dx
		ret
modeno13curs	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

trans2		proc
		mov	al,byte ptr Xcoord
		and	al,7
		mov	bl,Xcheck
		mov	Xcheck,al
		sub	al,bl
		jz	@noscroll
		jl	@scrleft
		lea	si,cursormask
		call	scrollright
		lea	si,screenmask
		jmp	scrollright

@scrleft:	neg	al
		mov	si,offset cursormask
		call	scrolleft
		mov	si,offset screenmask
		jmp	scrolleft

@noscroll:	ret
trans2		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

scrollright	proc
		push	di
		mov	bl,al

@scrn:		mov	di,si
		mov	cx,30h

@scrloop:	rcr	byte ptr [di],1
		inc	di
		loop	@scrloop

		jnc	@scrc
		or	byte ptr [si],80h
		jmp	@scrnc

@scrc:		and	byte ptr [si],7Fh
@scrnc:		dec	bl
		jnz	@scrn
		pop	di
		ret
scrollright	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

scrolleft	proc
		push	di
		mov	bl,al
		add	si,30h

@scln:		mov	di,si
		mov	cx,30h

@sclloop:	dec	di
		rcl	byte ptr [di],1
		loop	@sclloop

		mov	di,si
		jnc	@sclc
		or	byte ptr [di-1],1
		jmp	@sclnc

@sclc:		and	byte ptr [di-1],0FEh
@sclnc:		dec	bl
		jnz	@scln
		pop	di
		ret
scrolleft	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Return graphic mode video memory offset
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	AX					(X coordinate)
;	SI					(Y coordinate)
; Out:	SI					(video memory offset)
;	BX
; Use:	graphsegindic, videomode
; Modf: BX
; Call: none
;
retvidmem	proc
		xor	bx,bx
		xchg	si,ax
		IFnz	graphsegindic,@cvidmem
		cmp	videomode,13h
		je	@cvidmem
		mov	bx,ax
		cmp	videomode,14h
		jg	@m14cvid
		and	bx,1
		shr	ax,1
		jmp	@cvidmem

@m14cvid:	and	bx,3
		shr	ax,1
		shr	ax,1

@cvidmem:	mul	colswidth
		cmp	videomode,13h
		je	@m13calc
		mov	cl,3
		sar	si,cl

@m13calc:	add	si,ax
		mov	ax,vidmemoffs
		cmp	activepage,0FFh
		jne	@pageyes
		push	ds
		xor	ax,ax
		mov	ds,ax
		mov	ax,ds:[44eh]
		pop	ds
@pageyes:	add	si,ax
		mov	cl,3
		ror	bx,cl
		add	si,bx
		ret
retvidmem	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Return text mode video memory offset
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	AX					(Y coordinate in pixels)
;	BX					(X coordinate in pixels)
; Out:	ES:DI					(video memory pointer)
; Use:	pixboxwidth, colswidth, pixboxheight,
;	vidmemoffs, activepage, 0:44eh, videoseg
; Modf: DX
; Call: none
;
calcaddress	proc
		div	byte ptr pixboxwidth	; calculate Y in characters
		xor	ah,ah
		mul	colswidth
		mov	di,ax
		mov	ax,bx
		div	byte ptr pixboxheight	; calculate X in characters
		xor	ah,ah
		add	di,ax			; add the values
		shl	di,1			; and mul by 2 to get address
		mov	ax,vidmemoffs
		cmp	activepage,0FFh
		jne	@pageno
		xor	ax,ax
		mov	es,ax
		mov	ax,es:[44eh]
@pageno:	add	di,ax			; add active page offset if any
		mov	es,videoseg
		ret
calcaddress	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Check update region
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

checkregion	proc
		dec	cx
		dec	dx
		cmp	ax,uplefty
		jg	@regexit
		cmp	bx,upleftx
		jg	@regexit
		cmp	cx,lowrighty
		jl	@regexit
		cmp	dx,lowrightx
		jl	@regexit
		stc
		ret

@regexit:	clc
		ret
checkregion	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Updates graphics card port values
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

graphportparams	proc
		IFz	graphsegindic,@nogrparam ; jump if not A000 graphics
		push	ax bx cx dx es
		movSeg	es,cs

		mov	cx,9
		mov	dx,10h			; reads all graphics controller
		mov	bx,offset datav1	; regs (3CEh) to ES:BX
		mov	ah,0F2h
		int	10h

		mov	bx,2			; read register 2 of sequencer
		mov	dx,8			; (3C4h) - map mask register
		mov	ah,0F0h
		int	10h
		mov	mapmask,bl		; store it

		mov	bx,0F02h		; write 0Fh to 2nd register
		mov	dx,8			; of sequencer (3C4h):
		mov	ah,0F1h			; Enable all maps
		int	10h

		mov	cx,6			; write 6 values from ES:BX
		mov	dx,10h			; to graphics controller
		mov	bx,offset datav2	; regs (3CEh)
		mov	ah,0F3h
		int	10h

		mov	ch,7
		mov	cl,2			; write 7th and 8th regs of
		mov	dx,10h			; 3CEh from ES:BX
		mov	bx,offset datav3	; Enable all maps and color
		mov	ah,0F3h			; don't care
		int	10h

@popEDCBA:	pop	es
@popDCBA:	pop	dx cx bx ax

@nogrparam:	ret
graphportparams	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Restore video adapter port values
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

restorevidport	proc
		IFz	graphsegindic,@nogrparam ; jump if not A000 graphics
		push	ax bx cx dx

		mov	bl,2
		mov	dx,8			; restore map mask register
		mov	bh,mapmask
		mov	ah,0F1h
		int	10h

		mov	cx,6
		mov	dx,10h
		mov	bx,offset datav1
		mov	ah,0F3h
		int	10h			; write register range
		mov	ch,7
		mov	cl,2
		mov	dx,10h
		mov	bx,offset datav1b
		mov	ah,0F3h
		int	10h			; write register range
		jmp	@popDCBA
restorevidport	endp

;-------------------------------------------------------------

videomode	db	7		; video mode number
videoseg	dw	0B000h		; video mode seg
maxxcoord	dw	640		; max X coordinate
maxycoord	dw	200		; max Y coordinate
nextpagoffset	dw	16304
nextrowoffset	dw	8189		; add this to reach next row
pixboxheight	dw	8
pixboxwidth	dw	8
colgranu	dw	8		; column gran
rowgranu	dw	8		; row gran
colswidth	dw	80		; number of columns

vmodeparams	db	0
		dw	0B800h,640,200,16304,8189
		dw	16,8,16,8
		dw	40

		db	1
		dw	0B800h,640,200,16304,8189
		dw	16,8,16,8
		dw	40

		db	2
		dw	0B800h,640,200,16304,8189
		dw	8,8,8,8
		dw	80

		db	3
		dw	0B800h,640,200,16304,8189
		dw	8,8,8,8
		dw	80

		db	4
		dw	0B800h,640,200,16304,8189
		dw	8,8,2,1
		dw	80

		db	5
		dw	0B800h,640,200,16304,8189
		dw	8,8,2,1
		dw	80

		db	6
		dw	0B800h,640,200,16304,8189
		dw	8,8,1,1
		dw	80

		db	7
		dw	0B000h,640,200,16304,8189
		dw	8,8,8,8
		dw	80

		db	8
		dw	0B800h,640,200,32688,8189
		dw	8,8,4,1
		dw	80

		db	9
		dw	0B800h,1280,200,32608,8189
		dw	8,8,1,1
		dw	160

		db	0Ah
		dw	0B800h,1280,200,32608,8189
		dw	8,8,1,1
		dw	160

		db	0Bh
		dw	0B800h,640,200,16304,8189
		dw	8,8,1,1
		dw	80

		db	0Ch
		dw	0B800h,640,200,16304,8189
		dw	8,8,1,1
		dw	80

		db	0Dh
		dw	0A000h,320,200,0,37
		dw	8,8,1,1			; !!! 8,8,2,1
		dw	40

		db	0Eh
		dw	0A000h,640,200,0,77
		dw	8,8,1,1
		dw	80

		db	0Fh
		dw	0A000h,640,350,0,77
		dw	8,14,1,1
		dw	80

		db	10h
		dw	0A000h,640,350,0,77
		dw	8,14,1,1
		dw	80

		db	11h
		dw	0A000h,640,480,0,77
		dw	9,16,1,1
		dw	80

		db	12h
		dw	0A000h,640,480,0,77
		dw	9,16,1,1
		dw	80

		db	13h
		dw	0A000h,320,200,0,304
		dw	8,8,1,1			; !!! 8,8,2,1
		dw	320

		db	14h
		dw	0A000h,640,200,0,317
		dw	8,8,1,1
		dw	320

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Set parameters for current video mode
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
;AL - current video mode
;
setvidparams	proc
		push	ax bx cx dx es ds si di
		cli
		xchg	bl,al
		movSeg	ds,cs
		cmp	bl,13h
		jbe	@l13set
		mov	bl,14h			; if bigger than standard,
						;  then be 14h
@l13set:	xor	ah,ah
		mov	al,bl			; AL=BL=videomode
		mov	cx,15h
		imul	cx
		mov	si,ax
		lea	si,vmodeparams[si]
		lea	di,videomode		; copy params from table
		movSeg	es,cs
		rep	movsb

		mov	videomode,bl

		mov	ax,maxycoord		; set ranges for current mode
		dec	ax
		mov	vrangemax,ax
		mov	ax,maxxcoord
		dec	ax
		shl	ax,1
		mov	hrangemax,ax
		mov	mul2indic,0

		mov	ptr2gbuf1,offset grbuf1
		mov	ptr2gbuf2,offset grbuf2
		mov	graphmodeseg,cs
		mov	graphsegindic,0
		cmp	bl,0Dh			; is videmode below 0Dh?
		jb	@otherset		;  jump if yes
		cmp	bl,13h			; is videomode bigger than 13h?
		ja	@otherset		;  jump if yes
		jnz	@d13set

		mov	graphmodeseg,0A000h	; do these if vidmode=13h
		mov	ptr2gbuf1,0FA00h	; first free byte in vidmem
		mov	ptr2gbuf2,0FB80h	; we'll be storing the pointer
		inc	mul2indic		;  shape and the hided screen
		jmp	@otherset		;  contents here to save mem

@d13set:	cmp	bl,0Dh
		jne	@nod13set
		inc	mul2indic		; inc if videomode=0Dh

@nod13set:	call	firstfreevid		; call if 0Ch < videomode < 13h
@otherset:	sti
		pop	di si ds
		jmp	@popEDCBA

;-------------

firstfreevid:	mov	graphwritemod,1
		mov	ptr2gbuf1,3E82h		; 0D, 0E
		mov	ptr2gbuf2,3F12h		; 16002
		mov	graphmodeseg,0A000h
		mov	graphsegindic,bl
		cmp	bl,0Eh
		jle	@l11set

		mov	ax,4000h
		add	ptr2gbuf1,ax		; 0F, 10h, 11h, 12h
		add	ptr2gbuf2,ax		; 32386
		cmp	bl,11h
		jl	@l11set

		mov	ax,2000h
		add	ptr2gbuf1,ax		; 11h, 12h
		add	ptr2gbuf2,ax		; 40578

@l11set:	mov	ah,0F0h			; 0Dh - 12h
		mov	bx,5
		mov	dx,10h
		int	10h			; read mode register

		test	bl,2
		jz	@svidex
		mov	graphwritemod,10h

@svidex:	ret
setvidparams	endp

;----------------- Adapter port numbers and commands for RIL -----------------

RD1		dw	0,0
		db	0
RD2		db	0
d121		db	0
		db	24 dup (0)
d125		db	21 dup (0)
d122		db	8 dup (0)
RD3		db	0
RD4		db	0
RD123		db	0
RD124		db	0
RD125		db	0
		db	0,0,0,0
RD126		db	0
d127		db	0
		db	24 dup (0)
d126		db	21 dup (0)
d128		db	8 dup (0)
RD127		db	0
RD128		db	0
RD129		db	0
RD130		db	0

CRTCbase	dw	3D4h		; CRTC
RD132		dw	offset d121
RD133		dw	offset d127
RD134		db	19h
RD135		db	0

		dw	3C4h		; sequencer
		dw	offset RD1
		dw	offset RD125
		db	5
RD136		db	0

		dw	3CEh		; graphics controller
		dw	offset d122
		dw	offset d128
		db	9
RD137		db	0

		dw	3C0h		; VGA attrib controller
		dw	offset d125
		dw	offset d126
VGAmoncol	db	15h
RD139		db	0

		dw	3C2h		; VGA misc output and input
		dw	offset RD2
		dw	offset RD126
		db	1
		db	0

STATbase	dw	3DAh		; VGA status
		dw	offset RD4
		dw	offset RD128
		db	1
		db	0

		dw	3CCh		; VGA misc output read
datapro		dw	offset RD123
		dw	offset RD129
		db	1
		db	0

		dw	3CAh		; VGA feature control
		dw	offset RD124
		dw	offset RD130
		db	1
		db	0

RD142		db	0
RD143		db	0
		dw	0101h
		db	0

RILfuncs	dw	offset RIL_F0	; RIL functions
		dw	offset RIL_F1
		dw	offset RIL_F2
		dw	offset RIL_F3
		dw	offset RIL_F4
		dw	offset RIL_F5
		dw	offset RIL_F6
		dw	offset RIL_F7
RD152		dw	offset orig10func
		dw	offset orig10func
		dw	offset RIL_FA

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			RIL functions
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

RIL_F0		proc
		mov	si,dx
		mov	si,RD132[si]
		cmp	dx,20h
		jge	@R0
		add	si,bx
@R0:		mov	bl,[si]
		ret
RIL_F0		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_F1		proc
		mov	ax,bx
		mov	si,dx
		cmp	dl,20h
		mov	dx,CRTCbase[si]
		mov	RD135[si],1
		mov	si,RD132[si]
		jge	@R4
		xor	bh,bh
		mov	[bx+si],ah
		cmp	dl,0C0h
		jne	@R1
		push	ax
		mov	dl,byte ptr STATbase
		in	al,dx			; port 3DAh, CGA/EGA vid status
		pop	ax
		mov	dl,0C0h
		out	dx,ax
		mov	al,ah
		out	dx,al
		ret

@R4:		mov	[si],al
		mov	RD142,1
		out	dx,al			; port 3DAh, VGA feature contrl
		ret

@R1:		out	dx,ax
		ret
RIL_F1		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_F2		proc
		sti
		mov	di,bx
		mov	si,dx
		mov	si,RD132[si]
		xor	ax,ax
		xchg	al,ch
		add	si,ax
		shr	cx,1
		rep	movsw
		adc	cx,cx
		rep	movsb
		ret
RIL_F2		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_F3		proc
		sti
		push	es
		mov	si,bx
		mov	di,dx
		mov	RD135[di],1
		mov	dx,CRTCbase[di]
		mov	di,RD132[di]
		mov	ax,es
		mov	bx,ds
		mov	es,bx
		mov	ds,ax
		xor	ax,ax
		xchg	al,ch
		add	di,ax

		push	cx
		shr	cx,1
		rep	movsw
		adc	cx,cx
		rep	movsb
		mov	ds,bx
		pop	cx
		sub	di,cx

@looprega:	mov	ah,[di]
		cmp	dl,0C0h
		jne	@setREGA
		push	ax
		mov	dl,byte ptr STATbase
		in	al,dx			; port 3DAh, CGA/EGA vid status
		pop	ax
		mov	dl,0C0h
		out	dx,al
		push	ax
		mov	al,ah
		out	dx,al
		pop	ax
		jmp	@rega2

@setREGA:	out	dx,ax
@rega2:		inc	di
		inc	al
		loop	@looprega
		pop	es
		ret
RIL_F3		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_F4		proc
		sti
		mov	di,bx

@RILoo1:	mov	si,es:[di]
		mov	si,RD132[si]
		mov	al,es:[di+2]
		cbw
		add	si,ax
		add	di,3
		movsb
		loop	@RILoo1

		ret
RIL_F4		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_F5		proc
		sti
		push	dx
		mov	di,bx

@RILoo2:	mov	si,es:[di]
		mov	RD135[si],1
		mov	dx,CRTCbase[si]
		mov	si,RD132[si]
		mov	al,es:[di+2]
		cbw
		add	si,ax
		mov	ah,es:[di+3]
		mov	[si],ah
		out	dx,al
		inc	dx
		mov	al,ah
		out	dx,al
		dec	dx
		add	di,4
		loop	@RILoo2

		pop	dx
		ret
RIL_F5		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_F6		proc
		sti
		push	bx cx dx es
		movSeg	es,ds
		lea	bx,CRTCbase
		xor	cx,cx
@R12:		cmp	[bx+7],ch
		je	@R18
		mov	[bx+7],ch
		mov	cl,[bx+6]
		mov	si,[bx+4]
		mov	di,[bx+2]
		mov	dx,[bx]
		mov	al,ch

		push	cx
		shr	cx,1
		rep	movsw
		adc	cx,cx
		rep	movsb
		pop	cx
		sub	si,cx

@RILoo3:	mov	ah,[si]
		cmp	dx,03ceh
		jne	@Rlo3
		cmp	al,6
		jz	@R17

@Rlo3:		out	dx,al
		inc	dx
		push	ax
		mov	al,ah
		out	dx,al
		dec	dx
		pop	ax

@R17:		inc	si
		inc	ax
		loop	@RILoo3

@R18:		add	bx,8
		cmp	bx,offset datapro+1
		jl	@R12
		cmp	RD142,ch
		je	@R19
		mov	RD142,ch
		mov	dx,[bx+8]
		mov	al,RD128
		mov	RD4,al
		out	dx,al			; port 3CEh, EGA graphic index
						;  al = 0, set/reset bit
		mov	dx,3C2h
		mov	al,RD126
		mov	RD2,al
		out	dx,al			; port 3C2h, EGA misl out reg
		mov	dx,3cch
		mov	al,RD129
		mov	RD123,al
		out	dx,al			; port 3CCh, EGA graphics 1 pos
		mov	dx,3CAh
		mov	al,RD130
		mov	RD124,al
		out	dx,al			; port 3CAh, EGA graphics 2 pos

@R19:		mov	dx,03c0h
		mov	al,20h
		out	dx,al
		mov	dx,STATbase
		in	al,dx			; port 3DAh, CGA/EGA vid status
		pop	es dx cx bx
		ret
RIL_F6		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_F7		proc
		sti
		push	cx
		mov	si,bx
		mov	di,dx
		xor	ch,ch
		mov	cl,RD134[di]
		mov	RD135[di],cl
		mov	RD142,cl
		mov	di,RD133[di]

		mov	ax,es
		mov	bx,ds
		mov	es,bx
		mov	ds,ax
		shr	cx,1
		rep	movsw
		adc	cx,cx
		rep	movsb
		mov	es,ax
		pop	cx
		ret
RIL_F7		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

RIL_FA		proc
		movSeg	es,ds
		mov	bx,offset RD152
		ret
RIL_FA		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;	Called if not a valid RIL function requested (ah=Fx, INT 10h)
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

orig10func	proc
		push	bp
		mov	bp,sp
		mov	ds,[bp+8]
		mov	si,[bp+6]
		mov	ax,[bp+4]
		pop	bp
		pushf
		call	cs:oldint10
		push	bp
		mov	bp,sp
		mov	[bp+4],ax
		mov	[bp+6],si
		mov	[bp+8],dx
		pop	bp
		ret
orig10func	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			INT 10 handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

int10handler	proc
		cld
		cli
		cmp	ah,4			; light pen func?
		jne	@nolpen			; jump if not
		cmp	cs:lightpen?,1
		je	@lightpen

@nolpen:	test	ah,ah			; set video mode?
		jz	setmodreq		; jump if yes

		cmp	ah,0F0h			; RIL func requested?
		jae	RILreq			; jump if yes
@exitRIL:	jmp	cs:oldint10		; else call original handler

;------------- RIL

RILreq:		cmp	ah,0FBh
		jae	@exitRIL
		push	ax ds si di
		movSeg	ds,cs
		mov	al,ah
		and	ax,0Fh
		shl	ax,1
		mov	si,ax
		call	RILfuncs[si]
		pop	di si ds ax
		iret

;------------ emulate lightpen

@lightpen:	IFnz	cs:buttstatus,@lightbutt
		xor	ah,ah
		jmp	@lightbutt

@lightbutt:	sti
		mov	ax,cs:row2
		cmp	cs:videomode,0Fh
		jb	@lpenbf
		mov	cx,ax
		jmp	@lpenaf

@lpenbf:	mov	ch,al
@lpenaf:	div	cs:pixboxwidth
		mov	dh,al
		mov	ax,cs:column2
		mov	bx,ax
		cwd
		div	cs:colgranu
		xchg	bx,ax
		div	cs:pixboxheight
		mov	dl,al
		mov	ah,1
		iret

;------------ Set video mode

setmodreq:	push	ax
		mov	cs:cursorshown,0
		mov	ax,2
		int	33h			; mouse driver, hide cursor
		pop	ax

		call	setvidparams

		push	ax
		pushf
		call	cs:oldint10
		pop	ax

		mov	cs:RD143,al
		call	monoorcolor		; set up proper color mode

		iret
int10handler	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;	Determines and sets up mono or color monitor operation
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

monoorcolor	proc
		push	ds
		xor	ax,ax
		mov	ds,ax
		mov	al,ds:[410h]		; get equipment byte
		and	al,30h			; get initial video mode
		test	byte ptr ds:[487h],2	; color or mono monitor?
		jz	@cmon			; jump if color
		cmp	al,30h			; initial vmode is 80x25 mono?
		jne	@80mon			; jump if not

		mov	ah,cs:RD143		; setup mono operation
		and	ah,7Fh
		mov	byte ptr cs:CRTCbase,0B4h
		mov	byte ptr cs:STATbase,0BAh
		cmp	ah,0Fh
		je	@skipcol
		mov	ah,7
		jmp	@skipcol

@cmon:		cmp	al,30h			; initial vmode is 80x25 mono?
		je	@80mon			; jump if yes

		mov	ah,cs:RD143		; setup color operation
		and	ah,7Fh
		mov	cs:byte ptr CRTCbase,0D4h
		mov	cs:byte ptr STATbase,0DAh

@skipcol:	call	displayports		; setup display port funcs
@80mon:		pop	ds
		ret
monoorcolor	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Determine display adapter and set up ports
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
;AH - current videomode
;
displayports	proc
		push	cx di si es
		pushf
		cli
		xor	cx,cx
		mov	ds,cx
		les	si,dword ptr ds:[4a8h]
		les	si,dword ptr es:[si]
		test	byte ptr ds:[487h],60h
		jz	@noRAM			; jump if no RAM on adapter

		cmp	ah,0Fh			; is videomode 640x350 mono?
		jne	@nogmono

		add	si,440h
		jmp	@disdet

@nogmono:	cmp	ah,10h			; is videomode 640x350x16?
		jne	@noRAM

		add	si,480h
		jmp	@disdet

@noRAM:		cmp	ah,3			; is videomode 80x25x16?
		ja	@noMDAEGA

		mov	al,ds:[488h]		; get display combination
		and	al,0Fh
		cmp	al,3			; MDA+EGA?
		je	@MDAEGA
		cmp	al,9			; EGA+MDA?
		je	@MDAEGA
		jmp	@noMDAEGA

@MDAEGA:	add	si,4c0h

@noMDAEGA:	cmp	ah,11h			; is videomode 640x480 mono?
		jl	@noVmono
		add	ah,9
@noVmono:	xor	al,al
		shr	ax,1
		shr	ax,1
		add	si,ax

@disdet:	movSeg	ds,es
		movSeg	es,cs
		lea	di,RD1
		mov	al,3
		stosb
		add	si,5
		mov	cx,3Ch SHR 1
		push	cx si
		rep	movsw
		pop	si cx
		lea	di,RD125
		mov	al,3
		stosb
		rep	movsw
		movSeg	ds,cs
		std
		lea	di,RD3
		mov	cl,9
		mov	si,di
		dec	si
		rep	movsb
		lea	di,RD127
		mov	cl,9
		mov	si,di
		dec	si
		rep	movsb
		IFz	cs:VGAindicat,@nommon
		mov	VGAmoncol,14h

@nommon:	mov	cx,0100h
		mov	RD123,cl
		mov	RD129,cl
		mov	RD124,ch
		mov	RD130,ch
		mov	RD142,cl
		mov	RD136,cl
		mov	RD135,cl
		mov	RD139,cl
		mov	RD137,cl
		popf
		pop	es si di cx
		sti
		ret
displayports	endp

IFNDEF PS2
;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Check if COM port available
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	Zero flag
; Use:	IO_number
; Modf: AX, DX
; Call: none
;
chkcom		proc
		mov	dx,IO_number
		xor	al,al
		add	dx,3
		out	dx,al			; port 3FBh, reset comm params

		dec	dx
		dec	dx
		in	al,dx			; port 3F9h, get int enable reg
		and	al,0F0h
		mov	ah,al			; store reserved bits

		add	dx,3
		in	al,dx			; port 3FCh, get modem ctrl reg
		and	al,0E0h			; get reserved bits
		or	al,ah			; AL must be 0 if port exists
		ret
chkcom		endp

ELSE
;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				Check for PS/2
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
; In:	none
; Out:	Carry flag
; Use:	none
; Modf: AX, BX, CX, DX
; Call: none
;
checkPS2	proc
		cli
		mov	bh,3
		PS2serv 0C205h,@noPSdet		; initialize mouse, bh=datasize
		PS2serv 0C201h,@noPSdet		; reset mouse, returns bh=ID

		mov	cx,2
@try2:		push	es
		movSeg	es,cs
		mov	bx,offset IRQhandler
		mov	ax,0C207h
		int	15h			; mouse, es:bx=ptr to handler
		pop	es
		jnc	@PSokyet
		cmp	ah,4
		jne	@noPSdet
		loop	@try2
		jmp	@noPSdet

@PSokyet:	mov	bh,3
		PS2serv 0C203h, @noPSdet	; set mouse resolution bh
		mov	bh,1
		PS2serv 0C200h, @noPSdet	; set mouse on
		xor	dl,dl
		xor	bh,bh
		PS2serv 0C206h, @noPSdet	; mouse,bh=0 status,1-2=scaling

		test	bl,20h
		jz	@noPSdet
		or	dl,dl
		jz	@noPSdet
		mov	mousetype,4
		clc
		sti
		ret

@noPSdet:	stc
		sti
		ret
checkPS2	endp
ENDIF

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Update button status regs to new values
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

updatebuttstat	proc
		mov	cx,buttstatus
		mov	di,row2
		mov	si,column2
		mov	newbuttstat,0
		xor	cl,al
		mov	byte ptr buttstatus,al

		test	cl,1			; 1 pressed?
		jz	@chk2press		; jump if not
		test	al,1			; 1 released?
		jz	@rel1			; jump if yes

		or	newbuttstat,2		; indicate that 1 is pressed
		mov	butt1pc,si
		mov	butt1pr,di
		inc	butt1press
		jmp	@chk2press

@rel1:		or	newbuttstat,4		; indicate that 1 is released
		mov	butt1rc,si
		mov	butt1rr,di
		inc	butt1rel

@chk2press:	test	cl,2			; 2 pressed?
		jz	@chk3press		; jump if not
		test	al,2			; 2 released?
		jz	@rel2			; jump if yes

		or	newbuttstat,8		; indicate that 2 is pressed
		mov	butt2pr,di
		mov	butt2pc,si
		inc	butt2press
		jmp	@chk3press

@rel2:		or	newbuttstat,10h		; indicate that 2 is released
		mov	butt2rc,si
		mov	butt2rr,di
		inc	butt2rel

IFNDEF PS2
		cmp	mousetype,3		; 3 button mouse?
		jne	@nomorebutt		; jump if not
ELSE
		ret
ENDIF

@chk3press:	test	cl,4			; 3 pressed?
		jz	@nomorebutt		; quit if not
		test	al,4			; 3 released?
		jz	@rel3			; jump if yes

		or	newbuttstat,20h		; indicate that 3 is pressed
		mov	butt3pc,si
		mov	butt3pr,di
		inc	butt3press
		ret

@rel3:		or	newbuttstat,40h		; indicate that 3 is released
		mov	butt3rc,si
		mov	butt3rr,di
		inc	butt3rel

@nomorebutt:	ret

updatebuttstat	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				IRQ handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹


IRQhandler	proc	far
		cld
IFNDEF PS2
		cli
		push	ax bx cx dx ds es di si bp
		push	cs cs
		pop	ds es

		mov	dx,cs:IO_number
		add	dx,5
		in	al,dx			; 3FDh, check for overrun
		sub	dx,5
		test	al,2
		jz	@nooverrun		; jump if no overrun occured

		in	al,dx			; else flush receive buffer,
		mov	COM_incoming,0		;  zero counter,
		jmp	@exitIRQh		;  and exit

@nooverrun:	test	al,1			; check if data ready
		jnz	@dataready		; jump if yes
		in	al,dx			; else flush receive buffer,
		jmp	@exitIRQh		;  and exit

@dataready:	in	al,dx			; get that bastard
		cmp	cs:mousetype,2		; Microsoft mouse?
		je	@MSproc			; process that way if yes

		IFnz	cs:logitech?,@proclogi	; Logitech?
		call	msystemsproc		; else treat as MSM mode
		jmp	@exitIRQh

@MSproc:	call	microsoftproc
		jmp	@exitIRQh

@proclogi:	call	logiproc

@exitIRQh:	mov	al,20h
		out	20h,al			; port 20h, end of interrupt
		jmp	@rethandler
ELSE
		push	bp			; this is for PS2
		mov	bp,sp
		push	ax bx cx dx ds es di si
		push	cs cs
		pop	ds es

		mov	ax,[bp+0Ch]
		test	ah,ah
		jnz	@invPS2data
		and	al,3
		call	updatebuttstat
		mov	ax,[bp+0Ch]
		mov	bx,[bp+0Ah]
		mov	cx,[bp+8]
		test	al,10h
		jz	@PSxneg
		mov	bh,0FFh

@PSxneg:	test	cx,cx
		jz	@noymov
		neg	cl
		test	al,20h
		jnz	@noymov
		mov	ch,0FFh

@noymov:	add	Ymovement,cx
		add	Xmovement,bx
		mov	newPS2data,1

@invPS2data: 	pop	si di es ds dx cx bx ax bp
		retf
ENDIF

IRQhandler	endp

IFNDEF PS2
;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Process mouse bytes the Microsoft way
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

microsoftproc	proc
		cbw
		IFnz	COM_incoming,@MSsecond	; jump if not first byte

		cmp	al,40h			; synchro check
		jb	@nosync1		; jump if out of synchro

		xor	cx,cx
		mov	Xmovement,cx
		mov	Ymovement,cx
		shr	al,1			; bit 0 - X increment HI
		rcr	byte ptr Xmovement,1
		shr	al,1			; bit 1 - X increment HI
		rcr	byte ptr Xmovement,1
		shr	al,1			; bit 2 - Y increment HI
		rcr	byte ptr Ymovement,1
		shr	al,1			; bit 3 - Y increment HI
		rcr	byte ptr Ymovement,1
		and	al,3
		sar	al,1			; bit 4 - right button?
		jnc	@MSrrel			; jump if not pressed
		or	al,2			; set bit 1 if pressed

@MSrrel:	call	updatebuttstat
		mov	COM_incoming,1		; request second byte
		ret

@MSsecond:	cmp	COM_incoming,1		; second byte?
		jne	@MSthird		; jump if not
		mov	COM_incoming,2		; request third byte
		and	al,3Fh
		or	byte ptr Xmovement,al	; set X increment LO
		ret

@MSthird:	cmp	COM_incoming,2		; third byte?
		jne	@nosync1		; jump if not
		mov	COM_incoming,0		; request new triad
		and	al,3Fh
		or	al,byte ptr Ymovement	; set Y increment LO

		cbw
		mov	cx,ax
		mov	al,byte ptr Xmovement
		cbw
		mov	bx,ax
		jmp	movepointer

@nosync1:	mov	COM_incoming,0
		ret
microsoftproc	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Process mouse bytes the Logitech way
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

logiproc	proc
		mov	cx,COM_incoming		; CL = which byte is it
		test	al,40h			; first?
		jz	@3f47			; jump if not
		test	cx,cx			; CX = 0?
		jz	@3f47			; jump if yes
		cmp	cx,3
		jz	@3f35			; jump if CX = 3
		xor	cx,cx
		mov	COM_incoming,cx		; request new triad
		jmp	@3f54

@3f35:		xor	cx,cx			; request new 3/4
		mov	COM_incoming,cx
		test	extrabyte,4		; middlepressed?
		jz	@3f54			; jump if yes
		and	extrabyte,0FBh
		jmp	@3f54

@3f47:		test	al,40h			; first?
		jnz	@3f54			; jump if yes
		test	cx,cx			; first?
		jnz	@3f54			; jump if not

@endLGproc:	ret

@3f54:		test	cx,cx			; first?
		jnz	@3f5e			; jump if not
		mov	LGbstat,al		; store it
		jmp	@3f67

@3f5e:		cmp	cx,1			; second?
		jnz	@3f67			; jump if not
		mov	X_LO,al 		; store it
@3f67:		inc	COM_incoming		; req next byte
		cmp	cx,2			; third?
		jb	@endLGproc		; jump if below
		ja	@LG4th			; jump if fourth

		mov	bl,X_LO
		shl	bl,2
		mov	bh,LGbstat
		shr	bx,2
		mov	dl,bl			; DL = Xmovement

		mov	bl,al
		shl	bl,2
		shr	bx,2
		mov	dh,bl			; DH = Ymovement

		ror	bx,1
		ror	bh,1
		shl	bl,1
		rcl	bh,2
		and	bh,3
		mov	bl,bh			; BL = Left/Right status

		mov	bh,extrabyte
		test	bh,4
		jz	@3faa
		or	bl,4
@3faa:		mov	extrabyte,bl		; BL = Button status
		mov	al,dl
		cbw
		mov	Xmovement,ax
		mov	al,dh
		cbw
		mov	Ymovement,ax
		jmp	@3fed

@LG4th:		mov	COM_incoming,0		; req next 3/4
		mov	bl,extrabyte
		and	bl,4
		mov	cl,3
		and	al,20h
		shr	al,cl
		xor	bl,al
		jnz	@3fd9
		jmp	@endLGproc

@3fd9:		xor	bl,extrabyte
		mov	extrabyte,bl
		xor	bx,bx
		mov	Ymovement,bx
		mov	Xmovement,bx

@3fed:		mov	al,extrabyte
		call	updatebuttstat
		mov	cx,Ymovement
		mov	bx,Xmovement
		jmp	movepointer

logiproc	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Process mouse bytes the Mouse Systems way
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

MSMoffsets	dw	offset MSM1	; funcs for each 5 msm bytes
		dw	offset MSM24
		dw	offset MSM3
		dw	offset MSM24
		dw	offset MSM5

msystemsproc	proc
		cbw
		mov	si,COM_incoming
		shl	si,1
		jmp	MSMoffsets[si]

;ƒƒƒƒƒƒƒ 1st MSM byte ƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒ

MSM1:		push	ax
		and	al,0F8h
		cmp	al,80h			; synchro check
		jne	@nosync2		; jump if out of synchron
		pop	ax

		not	ax
		and	ax,7
		mov	COM_incoming,1		; request next byte
		mov	MSCbuttstate,ax		; save button state
		xor	ax,ax
		mov	Xmovement,ax
		mov	Ymovement,ax		; clear movement regs
		ret

@nosync2:	xor	ax,ax
		mov	COM_incoming,ax		; restart receiving
		mov	Xmovement,ax
		mov	Ymovement,ax		; clear movement regs
		pop	ax
		ret

;ƒƒƒƒƒ 2nd and 4th MSM bytes ƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒ

MSM24:		add	Xmovement,ax
		inc	COM_incoming
		ret

;ƒƒƒƒƒ 3rd MSM byte ƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒ

MSM3:		mov	Ymovement,ax
		inc	COM_incoming
		ret

;ƒƒƒƒƒ 5th MSM byte ƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒƒ

MSM5:		add	Ymovement,ax
		mov	COM_incoming,0

;--- process new info

		mov	ax,MSCbuttstate
		mov	cl,0Eh
		shl	ax,cl
		rcl	ax,1
		mov	cl,3
		rcl	ah,cl
		or	al,ah
		call	updatebuttstat
		mov	bx,Xmovement
		mov	cx,Ymovement
		neg	cx
		jmp	movepointer

msystemsproc	endp

ELSE

handleIRQ	proc
		cli
		pushf
		call	cs:oldIRQaddr
		IFz	cs:newPS2data,@iret2

		cld
		push	ax bx cx dx ds es di si bp
		push	cs cs
		pop	ds es
		mov	newPS2data,0
		mov	cx,Ymovement
		mov	bx,Xmovement
		call	movepointer
		xor	ax,ax
		mov	Ymovement,ax
		mov	Xmovement,ax
		jmp	@rethandler

@iret2: 	iret

handleIRQ	endp
ENDIF

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Move pointer to position
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
;BX - X movement
;CX - Y movement
;
movepointer	proc
		mov	ax,cx
		or	ax,bx			; was there any movement?
		jnz	@itsmoved		; jump if yes
		jmp	@buttonly

;---- calculate X mickey ----

@itsmoved:	mov	ax,bx
		IFz	autores?,@noautor
		sar	ax,2
		push	ax
		and	ax,0Fh
		cmp	ax,0Ah
		jle	@leax
		mov	ax,0Ah

@leax:		mov	ah,al
		mov	mresolution,ax
		pop	ax

@noautor:	xor	bx,bx
		call	resolute
		add	mickeyX,ax

;---- calculate Y mickey ----

		push	ax
		mov	ax,cx
		IFz	autores?,@noautor2
		sar	ax,2
		push	ax
		and	ax,0Fh
		cmp	ax,0Ah
		jle	@leax2
		mov	al,0Ah

@leax2:		mov	ah,al
		mov	mresolution,ax
		pop	ax

@noautor2:	mov	bx,1
		call	resolute
		add	mickeyY,ax

		mov	cx,ax			; CX - Y mickey movement
		pop	bx			; BX - X mickey movement

;---- calculate X movement in pixels ----

		test	bx,bx			; is X movement 0?
		jz	@xmov0			;  jump if yes
		shl	bx,3
		mov	ax,Xcalc
		add	ax,bx
		cwd
		idiv	h8mickey
		add	ax,column
		mov	Xcalc,dx
		mov	si,hrangemin
		sub	si,10h
		mov	di,hrangemax
		add	di,10h
		call	cutrange
		mov	column,ax
		cmp	ax,hrangemin
		jl	@xsmall
		cmp	ax,hrangemax
		jle	@xgood
		mov	ax,hrangemax
		jmp	@xgood

@xsmall:	mov	ax,hrangemin

@xgood:		cwd
		div	colgranu
		mul	colgranu
		mov	column2,ax		; the new column is ready
		sub	ax,Xcoordold
		mov	bx,ax			; BX - hot spot column

;---- calculate Y movement in pixels ----

@xmov0:		test	cx,cx			; is Y movement 0?
		jz	@ymov0			; jump if yes
		shl	cx,3
		mov	ax,Ycalc
		add	ax,cx
		cwd
		idiv	v8mickey
		add	ax,row
		mov	Ycalc,dx
		mov	si,vrangemin
		sub	si,10h
		mov	di,vrangemax
		add	di,10h
		call	cutrange
		mov	row,ax
		cmp	ax,vrangemin
		jl	@ysmall
		cmp	ax,vrangemax
		jle	@ygood
		mov	ax,vrangemax
		jmp	@ygood

@ysmall:	mov	ax,vrangemin

@ygood:		cwd
		idiv	rowgranu
		imul	rowgranu
		mov	row2,ax			; the new row is ready
		sub	ax,Ycoordold
		mov	cx,ax			; CX - hot spot row

@ymov0:		or	cx,bx			; if both are 0
		jz	@buttonly		; then no movement -> jump
		or	newbuttstat,1		; indicate movement

@buttonly:	mov	ax,newbuttstat
		IFnz	userproc?,@exitmove	; exit if user proc running
		call	calluserproc		; call user proc if available
		test	al,1			; was there movement?
		jz	@exitmove		;  exit if not
		mov	al,nowdrawing?
		or	al,cursorshown		; is drawing in progress?
		jnz	@exitmove		;  exit if yes
		sti
		call	showpointer		; else draw pointer
		cli

@exitmove:	ret

movepointer	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Use selected resolution
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹
;
;AX - mouse movement
;
resolute	proc
		mov	bl,byte ptr mresolution[bx]
		cmp	bx,1		; is resolution 1 or 0?
		jle	@res01		;  exit if yes

		cmp	ax,3		; was movement smaller than +3?
		jb	@res01		;  exit if yes
		cmp	ax,-3		; was movement larger than -3?
		ja	@res01		;  exit if yes

		cmp	ax,8		; was movement between +3 and +8?
		jb	@resother	;  jump if yes
		cmp	ax,-8		; was movement between -3 and -8?
		ja	@resother	;  jump if yes

		cwd
		imul	bx		; else multiply it with resolution
;*		shr	ax,1		; divide by 2
		ret			; and exit

@resother:	shl	ax,1		; small movement -> multiply with 2

@res01:		ret

resolute	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Call User Defined Handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

calluserproc	proc
		mov	userproc?,1
		push	ax
		and	ax,callmask		; is there a user call mask?
		jz	@nocmask		; exit if not

IFNDEF PS2
		push	ax
		mov	al,20h
		out	20h,al			; port 20h, end of interrupt
		pop	ax
ENDIF

		mov	si,mickeyX
		mov	di,mickeyY
		mov	bx,buttstatus
		mov	cx,column2
		mov	dx,row2
		sti
		call	userproc
		cli
		call	setsegs
@nocmask:	pop	ax
		mov	userproc?,0
		ret
calluserproc	endp

;==========================================================================
;------------------------ Below is not resident --------------------------

IRQ_number	db	0		; mouse IRQ
indicator	db	0		; 1 - show help, 2 - unload driver

IFNDEF PS2
cutestring	db	'Cute Mouse Driver v1.6 Copyright (c) 1997-1999 Nagy Daniel.',0dh,0ah
		db	'Type ctmouse /? for help',0dh,0ah,'$'
MSMStr		db	'Driver Mode: Mouse Systems',0dh,0ah,'$'
MSStr		db	'Driver Mode: Microsoft',0dh,0ah,'$'
datalogi	db	'Driver Mode: Logitech MouseMan',0dh,0ah,'$'
ELSE
cutestring	db	'Cute Mouse Driver v1.6 for PS/2 mice. Copyright (c) 1997-1999 Nagy Daniel.',0dh,0ah
		db	'Type ctmousep /? for help',0dh,0ah,'$'
drivmod		db	'Driver Mode: PS/2',0dh,0ah,'$'
ENDIF

relStr		db	'Mouse driver has been released from memory',0dh,0ah,'$'
mnsStr		db	'Mouse driver is not installed!',0dh,0ah,'$'

IFNDEF PS2
ncfStr		db	'Error: Cannot find COM port',0dh,0ah,'$'
ps2not		db	'Error: PS/2 not supported',0dh,0ah,'$'
ELSE
dnfStr		db	'Cannot find pointer device',0dh,0ah,'$'
ENDIF

alrStr		db	'Mouse already installed',0dh,0ah,'$'
instStr		db	'Installed on: COM $'
resol		db	'Resolution: $'
auto		db	'Auto',0dh,0ah,'$'
times		db	'3/3 times horizontally/vertically',0dh,0ah,'$'
badswstring	db	0dh,0ah,'Error: Invalid parameter',0dh,0ah
		db	'Enter /? on command line for help',0dh,0ah,'$'
com_port	db	0,0dh,0ah,'$'

IFNDEF PS2
CMDStr		db	'Cute Mouse Driver v1.6. Copyright (c) 1997-1999 Nagy Daniel',0dh,0ah
		db	'Options:',0dh,0ah
		db	'  /n   - where n is the COM port number',0dh,0ah
		db	'  /R0  - Auto hardware resolution',0dh,0ah
		db	'  /Rnm - n,m=1-9 resolution horizontally/vertically (default is R33)',0dh,0ah
		db	'  /M   - Force Microsoft mode (2 buttons)',0dh,0ah
		db	'  /T   - Force Logitech MouseMan mode (3 buttons)',0dh,0ah
		db	'  /S   - Force Mouse Systems mode (3 buttons)',0dh,0ah
		db	'  /In  - Force IRQ number (n is in hex: n=3-F)',0dh,0ah
ELSE
CMDStr		db	'Cute Mouse Driver v1.6 for PS/2 mice, Copyright (c) 1997-1999 Nagy Daniel',0dh,0ah
		db	'Options:',0dh,0ah
		db	'  /R0  - Auto hardware resolution',0dh,0ah
		db	'  /Rnm - n,m=1-9 resolution horizontally/vertically (default is R33)',0dh,0ah
ENDIF
		db	'  /L   - Left hand mode (default is right hand mode)',0dh,0ah
		db	'  /U   - Release driver',0dh,0ah
		db	'  /?   - Show help',0dh,0ah,'$'

IFNDEF PS2
microsoft?	db	0		; reg for detection
ENDIF

noemem		db	'Not enough memory!$'
ctstr		db	'CTMOUSE '

paragraphs	dw	0
dest_seg	dw	0
XMSentry	dd	0

;€€€€€€€€€€€€€€€€€€€€€€€€€€€€ Real Start €€€€€€€€€€€€€€€€€€€€€€€€€€€€

real_start:	cld
		mov	ah,4Ah			; free all the conv memory
		mov	bx,15+offset endProgram ; that the prog will not use
		shr	bx,4
		int	21h

		call	commandline		; examine command line
		jnc	@goodparam
		jmp	@badparam

IFNDEF PS2
@goodparam:	lea	dx,cutestring
		mov	ah,9
		int	21h			; display 'Cute driver'
		IFnz	com_port,@COMforce	; jump if COM forced
		IFnz	forced,@dontchps2	; jump if mode is forced
		call	checkps2		; check for PS2
@dontchps2:	mov	com_port,'1'
		push	ds
		push	0
		pop	ds
		mov	ax,ds:[400h]
		mov	IO_number,ax
		pop	ds

@COMforce:	IFz	forced,@detit		; jump if mode not forced
		call	chkcom
		jnz	@comnotfound		; jump if not found
		call	instexam		; test if driver is installed
		jnz	@notinstyet
		call	changeparam		; change params and quit

@detit:		call	detmoustype		; is mouse present?
		jc	@comnotfound		; jump if not found

@notinstyet:	call	allochmem		; allocate memory
		call	saveoldIRQ		; save old IRQ handler address

ELSE

@goodparam:	lea	dx,cutestring
		mov	ah,9
		int	21h			; display 'Cute driver'

		call	checkPS2		; check PS2
		jnc	@devfound
		lea	dx,dnfStr		; 'Cannot find pointer device'
		jmp	@printexit

@devfound:	call	instexam		; check if driver installed
		jnz	@notinstalled
		call	changeparam		; if yes, then change params
						;  and exit
@notinstalled:	call	allochmem		; allocate memory
		mov	IRQintnum,74h
		call	saveoldIRQ		; save old IRQ handler address
ENDIF
		call	newvideohandler		; set up new video handler
		call	saveold33		; save old INT 33h handler

		call	relocate		; relocate resident portion

		cli				; do not disturb until we
						;  install the handlers
		call	setnewIRQ		; setup new IRQ handler
		lds	dx,int10pointer
		mov	ax,2510h
		int	21h			; setup new 10 handler

		movSeg	ds,es
		lea	dx,handler
		mov	ax,2533h
		int	21h			; setup new 33 handler
		sti
		movSeg	ds,cs

		xor	ax,ax
		int	33h			; Reset driver

		call	printmod&res		; print mode and resolution
IFNDEF PS2
		call	printport		; print port number
ENDIF
		cmp	mem_type,3		; if conventional, TSR
		je	tsrexit

		mov	ax,4c00h
		int	21h			; exit with no errorlevel

; TSR exit if installed in conventional memory

tsrexit:	movSeg	ds,cs
		mov	es,ds:[2ch]		; release environment
		mov	ah,49h
		int	21h

		mov	ax,3100h
		lea	dx,IRQ_number
		shr	dx,4			; TSR exit
		inc	dx
		int	21h

;--------------------------
IFNDEF PS2
@comnotfound:	lea	dx,ncfStr		; 'Cannot find COM: port...'
		jmp	@printexit
ENDIF
;-------------------------

@badparam:	lea	dx,badswstring		; 'Invalid parameter'
@printexit:	mov	ah,9
		int	21h
		mov	ax,4CFFh		; exit with errorlevel
		int	21h

IFNDEF PS2
;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Check if requested mouse type available
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

checktype	proc

		cmp	mousetype,2	; microsoft Mouse?
		je	@Mavail
		cmp	mousetype,3	; mouse systems mouse?
		jne	@Mnotavail

@Mavail:	call	disableCOMint
		call	chkcom		; check COM port
		jnz	@Mnotavail	; jump if not available

		mov	si,IO_number
		call	mouseHWreset
		cmp	mousetype,2	; microsoft mouse?
		jne	@notMSm
		call	detmicrosoft
		jc	@Mnotavail
		jmp	@setMSm

@notMSm:	mov	al,8
		IFz	logitech?,@write8
		add	al,3
@write8:	mov	dx,si
		add	dx,4
		out	dx,al		; port 3FCh, clear RTS and DTR,
					; set output 2
@setMSm:	mov	al,1
		mov	dx,si
		inc	dx
		out	dx,al		; port 3F9h, DR int enable
		clc
		ret

@Mnotavail:	stc
		ret

checktype	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Reset mouse hardware
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

mouseHWreset	proc

		mov	al,80h
		mov	dx,si
		add	dx,3
		out	dx,al		; set DLAB on

		mov	al,60h
		mov	dx,si
		out	dx,al		; speed LO byte

		xor	al,al
		inc	dx
		out	dx,al		; speed HI byte

		mov	al,mousetype
		inc	dx
		inc	dx
		out	dx,al		; set comm params and DLAB=0

		xor	al,al
		dec	dx
		dec	dx
		out	dx,al		; all interrupts off

		mov	al,1
		add	dx,3
		out	dx,al		; activate DTR and clear RTS
		xor	cx,cx
		loop	$

		inc	dx
		in	al,dx		; clear error bits
		ret

mouseHWreset	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		Detect if Microsoft Mouse present
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

detmicrosoft	proc

		mov	al,0Bh
		mov	dx,si
		add	dx,4
		out	dx,al		; activate DTR, RTS and output 2
		mov	dx,si
		in	al,dx		; flush receive buffer
		xor	bx,bx

getM:		xor	cx,cx
chkDTR:		mov	dx,si
		add	dx,5
		in	al,dx		; get line stat reg
		and	al,1		; check if data ready
		jnz	DTRok		; jump if yes
		loop	chkDTR		; if not, try again
		jmp	DTRnotok	; if no data received, exit with error

DTRok:		mov	dx,si
		in	al,dx		; get that bastard byte
		cmp	al,33h		; '3' received?
		jne	@not3
		mov	logitech?,1
@not3:		cmp	al,4Dh		; 'M' received?
		jne	@notM		; jump if not
		mov	microsoft?,1
@notM:		inc	bx
		cmp	bx,4		; if there is no 'M' within 4 received
		jb	getM		;  bytes, then exit with error

DTRnotok:	IFz	microsoft?,@setdeterror
		IFnz	logitech?,@setdeterror
		clc
		ret

@setdeterror:	stc
		ret

detmicrosoft	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				Set Mouse Port
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

comport		proc

		push	ds
		push	0
		pop	ds
		cmp	al,11h			; '1'
		jne	notcom1
		mov	cs:com_port,'1'
		mov	ax,ds:[400h]
		mov	cs:IO_number,ax
		mov	cs:PICstate,10h		; PIC interrupt enabler (COM1)
		mov	cs:IRQ_number,4
		mov	cs:IRQintnum,0Ch
		jmp	endportset

notcom1:	cmp	al,12h			; '2'
		jne	notcom2
		mov	cs:com_port,'2'
		mov	ax,ds:[402h]
		mov	cs:IO_number,ax
		mov	cs:PICstate,8		; PIC interrupt enabler (COM2)
		mov	cs:IRQ_number,3
		mov	cs:IRQintnum,0Bh
		jmp	endportset

notcom2:	cmp	al,13h			; '3'
		jne	notcom3
		mov	cs:com_port,'3'
		mov	ax,ds:[404h]
		mov	cs:IO_number,ax
		mov	cs:PICstate,10h
		mov	cs:IRQ_number,4
		mov	cs:IRQintnum,0Ch
		jmp	endportset

notcom3:	cmp	al,14h			; '4'
		jne	notCOM
		mov	cs:com_port,'4'
		mov	ax,ds:[406h]
		mov	cs:IO_number,ax
		mov	cs:PICstate,8
		mov	cs:IRQ_number,3
		mov	cs:IRQintnum,0Bh

endportset:	pop	ds
		clc
		ret

notCOM:		pop	ds
		stc
		ret

comport		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Print COM port number
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

printport	proc

		lea	dx,instStr		; 'Installed on: COM '
		mov	ah,9
		int	21h

		lea	dx,com_port		; 'COM x'
		mov	ah,9
		int	21h			; display string
		ret

printport	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Set Manual IRQ num
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

setIRQ		proc

		cmp	al,'I'
		jne	endsetIRQ
		lodsb
		cmp	al,33h
		jb	endsetIRQ
		cmp	al,40h
		jb	numa
		and	al,0dfh
		cmp	al,'F'
		ja	endsetIRQ
		sub	al,7
numa:		sub	al,30h
		mov	cs:IRQ_number,al
		cmp	al,8
		jb	numa2
		add	al,60h
numa2:		add	al,8
		mov	cs:IRQintnum,al
		jmp	oksetIRQ

endsetIRQ:	stc
		ret

oksetIRQ:	clc
		ret

setIRQ		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Check for PS/2
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

checkps2	proc

		int	11h
		test	al,4			; PS/2?
		jz	notPS2			; Jump if not

		lea	dx,ps2not
		mov	ah,9			;print error and exit
		int	21h
		mov	ax,4cffh
		int	21h

notPS2:		ret

checkps2	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Determine Mouse Type
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

detmoustype	proc

		mov	mousetype,2		; Set Microsoft mouse type
		call	checktype
		jnc	microinst
		mov	mousetype,3		; Set Mouse systems mode
		call	checktype
		jc	nomice

microinst:	call	instexam		; already installed?
		jnz	notyetinst		; if no, jump
		call	changeparam		; else change params and exit

notyetinst:	clc
nomice:		ret

detmoustype	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Change IRQ handler to new IRQ number
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

changeIRQ	proc

		push	cs
		pop	ds
		cli
		call	setIRQparams
		mov	al,PICstate
		mov	es:PICstate,al
		mov	ax,IO_number
		mov	es:IO_number,ax
		mov	al,IRQ_number
		mov	es:IRQ_number,al
		mov	ah,25h
		lds	dx,oldIRQaddr
		int	21h			; restore handler for old IRQ

		call	setnewIRQ		; install handler for new IRQ
		mov	al,cs:IRQintnum
		mov	es:IRQintnum,al
		sti
		ret

changeIRQ	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				Set IRQ params
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

setIRQparams	proc

		push	dx si
		mov	si,es:IO_number
		xor	al,al
		mov	dx,si
		inc	dx
		out	dx,al		; disable all interrupts

		add	dx,3
		in	al,dx		; modem ctrl
		and	al,0F3h		; disable auxilary outputs
		out	dx,al

		inc	dx
		in	al,dx		; clear error bits

		mov	dx,si
		in	al,dx		; flush receive buffer
		pop	si dx
		ret

setIRQparams	endp

ENDIF

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Print Help and Quit
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

printhelp	proc

		lea	dx,CMDStr
		mov	ah,9
		int	21h			; display char string at ds:dx

		mov	ax,4C00h
		int	21h			; terminate with al=return code

printhelp	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Examine if installed
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

instexam	proc

		mov	ax,3533h
		int	21h			; get intrpt vector al in es:bx
		cmp	bx,offset handler
		ret

instexam	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Examine Command Line
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

commandline	proc

		mov	si,81h
		cld
newopt:		call	parsecmdl
		lodsb
		cmp	al,0Dh			; ENTER?
		je	noswitch
		cmp	al,0Ah			; ENTER?
		je	noswitch
		test	al,al			; NULL?
		je	noswitch
		cmp	al,'/'
		jne	badswitch

		lodsb
		cmp	al,'?'
		jne	@nohelp			; '/?' -> print help and exit
		call	printhelp

@nohelp:	and	al,0dfh
		cmp	al,'U'			; '/U' -> release drv and exit
		jne	@noreldriv
		call	reldriver

@noreldriv:	call	resolution
		jnc	newopt
		cmp	al,'L'
		jne	@notleft
		mov	lefthand?,1
		jmp	newopt

@notleft:
IFNDEF PS2
		call	comport
		jnc	newopt
		call	setIRQ
		jnc	newopt
		cmp	al,'M'			; force microsoft
		jne	nofmi
		mov	mousetype,2
		mov	logitech?,0
force:		mov	forced,1
		jmp	newopt

nofmi:		cmp	al,'T'			; force logitech
		jne	noflog
		mov	mousetype,3
		mov	logitech?,1
		jmp	force

noflog:		cmp	al,'S'			; force mouse systems
		jne	nofsys
		mov	mousetype,3
		mov	logitech?,0
		jmp	force
nofsys:
ENDIF
badswitch:	stc
		ret

noswitch:	clc
		ret

commandline	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

parsecmdl	proc

pars_st:	lodsb
		cmp	al,' '
		je	pars_st
		cmp	al,9			;tab?
		je	pars_st
		dec	si
		ret

parsecmdl	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				Set hand mode
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

whichhand	proc

		cmp	al,'L'
		jne	righthand
		mov	lefthand?,1		; set left handed mode
		clc
		ret

righthand:	mov	lefthand?,0		; set right handed mode
		stc
		ret

whichhand	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				Set Resolution
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

resolution	proc

IFNDEF PS2
		and	al,0DFh
ENDIF
		cmp	al,'R'
		jne	@endsetres

		lodsb
		cmp	al,'0'
		je	@autores
		cmp	al,'1'			; smaller than 1?
		jb	@endsetres
		cmp	al,'9'			; bigger than 9?
		ja	@endsetres
		mov	times,al		; 'x/x  Times  Resolution '
		sub	al,31h
		mov	byte ptr mresolution,al
		lodsb
		cmp	al,'0'
		je	@autores
		cmp	al,'1'
		jb	@no2ndres
		cmp	al,'9'
		ja	@no2ndres
@store2nd:	mov	times[2],al
		sub	al,31h
		mov	byte ptr mresolution[1],al
		mov	autores?,0
		jmp	@oksetres

@no2ndres:	mov	al,byte ptr times
		dec	si			; fixup for command line
		jmp	@store2nd

@autores:	mov	autores?,1
		mov	word ptr mresolution,0
@oksetres:	clc
		ret

@endsetres:	stc
		ret

resolution	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Release driver and quit
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

reldriver	proc

		call	instexam
		jnz	notinst

		push	es			; store segment of handler
		lea	dx,relStr		; 'Current Mouse Driver...'
		mov	ah,9
		int	21h			; display char string
		mov	ax,1Fh
		int	33h			; disable driver
		pop	es

		push	ds
		lds	dx,es:oldint33
		mov	ax,2533h
		int	21h			; set intrpt vector al to ds:dx

		mov	ax,es
		dec	ax
		mov	ds,ax
		mov	ah,62h
		int	21h
		mov	ds:[1],bx		; modify MCB
		pop	ds
		call	FreeMem
		jc	notinst

		mov	ax,4C00h
		int	21h			; terminate with al=return code

notinst:	lea	dx,mnsStr		; 'Mouse Driver Not Installed'
		mov	ah,9
		int	21h			; display char string at ds:dx
		mov	ax,4CFFh
		int	21h			; terminate with al=return code

reldriver	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;		If already installed, change params and exit
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

changeparam	proc

		mov	al,lefthand?
		mov	es:lefthand?,al
		mov	ax,mresolution
		mov	es:mresolution,ax
		mov	al,autores?		; set up new params
		mov	es:autores?,al
		call	printmod&res		; print new params

		push	es
		mov	ax,1Fh
		int	33h			; disable driver
		pop	es

		mov	al,mousetype
		mov	es:mousetype,al		; setup new type

IFNDEF PS2
		mov	al,logitech?
		mov	es:logitech?,al
ENDIF
		mov	ax,20h
		int	33h			; enable driver

IFNDEF PS2
		call	printport
ENDIF

		call	printalready

IFNDEF PS2
		mov	al,IRQintnum		; check if we have to change
		cmp	al,es:IRQintnum		;  interrupt number
		je	nointch			; jump if not
		call	changeIRQ		; else change
ENDIF

nointch:	mov	ax,4C00h
		int	21h			; terminate with al=return code

changeparam	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ

newvideohandler	proc

		call	detVGA			; detect VGA card
		xor	ax,ax
		mov	ds,ax
		mov	ah,ds:[449h]		; AH - current videomode
		call	displayports		; set up display port funcs
		movSeg	ds,cs

		push	es
		mov	ax,3510h
		int	21h			; save old 10 handler
		saveFAR	oldint10,es,bx
		pop	es
		saveFAR int10pointer,es,<offset int10handler>

		ret

newvideohandler	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				Detect VGA card
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

detVGA		proc

		mov	ax,1A00h
		xor	bx,bx
		int	10h		; get display type in bx
		cmp	al,1Ah
		jne	@itsnotVGA

		cmp	bl,7		; VGA with monochrome?
		je	@itsaVGA
		cmp	bl,8		; Color VGA?
		jne	@itsnotVGA

@itsaVGA:	mov	VGAindicat,1

@itsnotVGA:	ret

detVGA		endp



;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Save old IRQ handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

saveoldIRQ	proc

IFNDEF PS2
		lea	dx,IRQhandler
ELSE
		lea	dx,handleIRQ
ENDIF
		saveFAR newIRQaddr,es,dx

		push	es
		mov	al,IRQintnum
		mov	ah,35h
		int	21h			;get old IRQ handler pointer
		saveFAR oldIRQaddr,es,bx
		pop	es
		ret

saveoldIRQ	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Save old INT33h handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

saveold33	proc

		push	es
		mov	ax,3533h
		int	21h			; save old 33 handler
		mov	word ptr oldint33,bx
		mov	word ptr oldint33+2,es
		pop	es
		ret

		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Set new IRQ handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

setnewIRQ	proc

		push	ds
		mov	al,IRQintnum
		mov	ah,25h
		lds	dx,newIRQaddr
		int	21h			; set new IRQ handler
		pop	ds
		ret

		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Print if already installed
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

printalready	proc

		lea	dx,alrStr		; 'Mouse already installed'
		mov	ah,9
		int	21h			; display char string at ds:dx

		ret

printalready	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Print mode and resolution
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

printmod&res	proc

		push	es
IFNDEF PS2
		cmp	mousetype,3		; Mouse systems mouse?
		jne	@micromode
		IFz	logitech?,@notlogi		; Logitech mouse?

		lea	dx,datalogi
		jmp	@printmode

@notlogi:	lea	dx,MSMStr		; 'Driver Installed: Mouse'
		jmp	@printmode

@micromode:	lea	dx,MSStr		; 'Driver Installed: Microsoft'
ELSE
		lea	dx,drivmod
ENDIF

@printmode:	mov	ah,9
		int	21h

		lea	dx,resol		; 'Resolution:'
		mov	ah,9
		int	21h

		IFz	autores?,@noprauto
		lea	dx,auto
		jmp	@prres
@noprauto:	lea	dx,times
@prres:		mov	ah,9
		int	21h

		pop	es
		ret

printmod&res	endp

;-------------------

; DOS 5.0+ UMB's
SaveMemStrat	dw	0
SaveUMBLink	db	0

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;
; In:	none
; Out:	Zero flag
; Use:	none
; Modf: AX, BX, ES, XMSentry
; Call: none
;
getXMSaddr	proc
		xor	bx,bx
		mov	es,bx
		mov	ax,4310h	; XMS: Get Driver Address
		int	2Fh
		saveFAR XMSentry,es,bx
		mov	ax,es
		or	ax,bx		; ZF indicates error: JZ error
		ret
getXMSaddr	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; Get Allocation Srategy
;
; In:	none
; Out:	Carry flag
; Use:	none
; Modf: AX, SaveMemStrat, SaveUMBLink
; Call: none
;
GetAllocStrat	proc
		mov	ax,5800h	; get DOS memory alloc strategy
		int	21h
		jc	@@fingas	; not supported
		mov	SaveMemStrat,ax
		mov	ax,5802h	; get UMB link state
		int	21h
		mov	SaveUMBLink,al
@@fingas:	ret
GetAllocStrat	endp


;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; Restore allocation strategy
;
; In:	none
; Out:	none
; Use:	SaveMemStrat, SaveUMBLink
; Modf: AX, BX
; Call: none
;
ResAllocStrat	proc
		mov	ax,5801h		; set DOS memory alloc strategy
		mov	bx,SaveMemStrat
		int	21h
		mov	ax,5803h		; set UMB link state
		mov	bl,SaveUMBLink
		xor	bh,bh
		int	21h
		ret
ResAllocStrat	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
; function AllocMem
;In:	AX - memory required
;Out:	AX - segment (or 0 if error)
;	dest_seg
;	mem_type

AllocMem	proc				; call with AX = n of bytes

		push	ds es
		mov	mem_type,0
		mov	dest_seg,0
		add	ax,0Fh
		shr	ax,4
		mov	paragraphs,ax

; Check if UMB is DOS5+ type

		mov	ah,30h
		int	21h
		cmp	al,5			; DOS >= 5.0, supports UMBs
		jb	@@noDOS5UMBs

		call	GetAllocStrat
		jc	@@noDOS5UMBs

		mov	ax,5801h		; set DOS memory alloc strategy
		xor	bx,bx			; low mem, first fit
		int	21h
		jc	@@noDOS5UMBs		; reports >= 5.0 no support

		mov	ax,5803h		; set UMB link state
		mov	bx,1			; add UMB to MCB chain
		int	21h
		jc	@@noDOS5UMBs

; try to set a good strategy to allocate DOS supported UMBs

		mov	ax,5801h		; set alloc strategy
		mov	bx,41h			; hi mem, best fit
		int	21h
		jnc	@@linkUMB		; jump if success

; try a worse one then

		mov	ax,5801h	; set alloc strategy
		mov	bx,81h		; hi mem then low mem, best fit
		int	21h
		jc	@@dev5		; jump if error

@@linkUMB:	mov	ax,5803h	; add UMBs to link state
		mov	bx,1
		int	21h
		mov	ah,48h		; allocate UMB memory
		mov	bx,paragraphs
		int	21h
		jc	@@dev5
		cmp	ax,0A000h	; check if allocated mem is
		ja	@@Dumb_OK	; beyond 640k. Jump if yes

		mov	es,ax		; if below, then free it
		mov	ah,49h
		int	21h
		stc			; indicate it
		jmp	@@dev5

@@Dumb_OK:	clc
@@dev5:		pushf
		push	ax
		call	ResAllocStrat	; restore allocation strategy
		pop	ax
		popf
		jc	@@noDOS5UMBs	; jump if UMB allocating not successful
		mov	dest_seg,ax	; else set proper variables and exit
		mov	mem_type,1	; 1 = DOS 5.0+ UMB
		jmp	@@fin

; try XMS driver to allocate UMB

@@noDOS5UMBs:	mov	ax,4300h	; XMS: Installation Check
		int	2Fh
		cmp	al,80h
		jne	@@noXMS_UMB
		call	getXMSaddr
		jz	@@noXMS_UMB
		mov	ah,10h		; XMS: Request Upper Memory Block
		mov	dx,paragraphs
		call	XMSentry
		cmp	ax,1
		jne	@@noXMS_UMB
		cmp	dx,paragraphs
		jb	@@noAlcanza
		mov	dest_seg,bx
		mov	mem_type,2	; 2 = XMS UMB
		jmp	@@fin

@@noAlcanza:	mov	dx,bx
		mov	ah,11h		; XMS: Release Upper Memory Block
		call	XMSentry

; use conventional memory

@@noXMS_UMB:	mov	ax,cs
		mov	dest_seg,ax	; AX=segment
		mov	mem_type,3	; 3 = it's in conventional memory
		clc

@@fin:		pop	es ds
		mov	ax,dest_seg
		ret
AllocMem	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;
;IN: ES - segment to free
;
FreeMem		proc

		push	ds es
		mov	al,es:mem_type
		cmp	al,2
		je	@@xms
		cmp	al,3
		je	@@finConv

@@dos5:		call	GetAllocStrat
		jc	@@finfree	; error?
		mov	ax,5803h	; unlink UMBs
		xor	bx,bx
		int	21h
		mov	ah,49h		; free allocated memory
		int	21h
		call	ResAllocStrat
		jmp	@@finfree

@@xms:		call	getXMSaddr
		jz	@@finfree	; error?
		mov	ah,11h		; XMS: Release Upper Memory Block
		mov	dx,es
		call	XMSentry
		jmp	@@finfree

@@finConv:	mov	ah,49h		; free allocated memory
		int	21h
@@finfree:	pop	es ds
		ret

FreeMem		endp


;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
allochmem	proc

		mov	ax,offset IRQ_number	; get number of bytes
		call	AllocMem		;  we need memory for
		jnc	@okmem

		mov	ah,9
		lea	dx,noemem		; error if no available
		int	21h
		mov	ax,4cffh
		int	21h

@okmem:		cmp	mem_type,3
		je	convent
		mov	bx,ax
		dec	bx
		mov	es,bx			; modify MCB
		mov	word ptr es:[1],ax

		lea	si,ctstr
		mov	di,8
		mov	cx,8
		rep	movsb			; copy process name

		mov	bx,ax
		add	bx,word ptr es:[3h]

		mov	es,ax
		mov	dx,ax
		mov	ah,26h			; create PSP
		int	21h
		mov	es:[2h],bx		; fix upper segment number
		ret

convent:	mov	es,ax
		ret

		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
relocate	proc

		mov	si,100h			; relocate the resident
		mov	di,si			;  portion
		mov	cx,offset IRQ_number-offset start
		rep	movsb
		ret

		endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
label endProgram

		end	start
