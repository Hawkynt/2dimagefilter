.model tiny
.code


saveFAR		macro	addr, segm, offs
		mov	word ptr addr,offs
		mov	word ptr addr[2],segm
	endm

		org	100h
start:		jmp	realstart



oldIRQaddr	dd	0		; old IRQ handler address
newIRQaddr	dd	0		; new IRQ handler address
incoming	db	0
limit		db	3
e1200		db	'1200 bps, $'
e2400		db	'2400 bps, $'
databits	db	' data bits, $'
stopbit		db	' stop bit.$'


;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;				IRQ handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹


IRQhandler	proc	far
		cld
		cli
		push	ax bx cx dx ds es di si bp
		push	cs cs
		pop	ds es

		mov	dx,3f8h			; COM 1
		add	dx,5
		in	al,dx			; 3FDh, check for overrun
		sub	dx,5
		test	al,2
		jz	@nooverrun		; jump if no overrun occured

		in	al,dx			; else flush receive buffer,
		mov	incoming,0
		jmp	@exitIRQh		;  and exit

@nooverrun:	test	al,1			; check if data ready
		jnz	@dataready		; jump if yes
		in	al,dx			; else flush receive buffer,
		jmp	@exitIRQh		;  and exit

@dataready:	in	al,dx			; get that bastard
		inc	incoming
		call	tobin
		mov	al,limit
		cmp	al,incoming
		ja	@exitIRQh

		mov	incoming,0
		mov	ah,2
		mov	dl,0dh
		int	21h
		mov	dl,0ah
		int	21h

@exitIRQh:	mov	al,20h
		out	20h,al			; port 20h, end of interrupt

@rethandler:	pop	bp si di es ds dx cx bx ax
		iret


IRQhandler	endp

;-----------------------------------------------------------

tobin		proc
		mov	ah,02h			; print one char
		mov	cx,8			; 8 bits
nextbit:	mov	dl,'0'
		shl	al,1
		adc	dl,0
		push	ax
		int	21h			; print it
		pop	ax
		loop	nextbit
		mov	dl,' '
		int	21h
		ret
		endp

;--------------------------------------------------------------

getcomparams	proc

                cli
		mov	dx,3fbh
		in	al,dx
		mov	bl,al			; store params in BL
		or	al,80h
		out	dx,al			; DLAB=1
		mov	dx,3f8h
		in	ax,dx			; get speed
                push    ax

		mov	dx,3fbh
		mov	al,bl
		out	dx,al			; port 3FBh, set comm params
		mov	al,0Bh
		inc	dx
		out	dx,al			; port 3FCh, reset hardware
		mov	al,1
		sub	dx,3
		out	dx,al			; port 3F9h, DR int enable
		add	dx,4
		in	al,dx			; port 3FDh, read LSR thus
		sub	dx,5
		in	al,dx			; port 3F8h, flush reveive buffer
                sti

                pop     ax
		cmp	ax,60h
		jne	not1200
		lea	dx,e1200
		jmp	printspeed
not1200:	lea	dx,e2400
printspeed:	mov	ah,9
		int	21h

		mov	dl,'7'
		mov	al,bl
		and	al,3
		cmp	al,2
		je	bits7
		inc	dl
bits7:		mov	ah,2
		int	21h
		mov	ah,9
		lea	dx,databits
		int	21h

		mov	dl,'1'
		mov	al,bl
		and	al,4
		jz	onestop
		inc	dl
onestop:	mov	ah,2
		int	21h
		mov	ah,9
		lea	dx,stopbit
		int	21h


		mov	ah,2
		mov	dl,0dh
		int	21h
		mov	dl,0ah
		int	21h
		
		ret
		endp

;--------------------------------------------------------------

saveoldIRQ	proc

		lea	dx,IRQhandler
		saveFAR newIRQaddr,es,dx

		push	es
		mov	al,0ch
		mov	ah,35h
		int	21h			;get old IRQ handler pointer
		saveFAR oldIRQaddr,es,bx
		pop	es
		ret

saveoldIRQ	endp

;ﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂﬂ
;			Set new IRQ handler
;‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹‹

setnewIRQ	proc
;DX - handler

		mov	al,0ch
		mov	ah,25h
		int	21h			; set new IRQ handler
		ret
		endp

;------------------------------------------------      
realstart:	cmp	byte ptr ds:[80h],0
		je	noparam
		mov	al,byte ptr ds:[82h]
		sub	al,30h
		mov	limit,al

noparam:	call	getcomparams
		cli
		call	saveoldIRQ
		lds	dx,newIRQaddr
		call	setnewIRQ
		sti

		xor	ax,ax
		int	16h

		cli
		lds	dx,oldIRQaddr
		call	setnewIRQ
		sti

		int	20h
		ends
		end	start
