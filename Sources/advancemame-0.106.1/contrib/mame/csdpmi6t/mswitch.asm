; Copyright (C) 1995-1999 CW Sandmann (sandmann@clio.rice.edu) 1206 Braelinn, Sugar Land, TX 77479
; Copyright (C) 1993 DJ Delorie, 24 Kirsten Ave, Rochester NH 03867-2954
;
; This file is distributed under the terms listed in the document
; "copying.cws", available from CW Sandmann at the address above.
; A copy of "copying.cws" should accompany this file; if not, a copy
; should be available from where this file was obtained.  This file
; may not be distributed without a verbatim copy of "copying.cws".
;
; This file is distributed WITHOUT ANY WARRANTY; without even the implied
; warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

; Modified for VCPI Implement by Y.Shibata Aug 5th 1991
; Modified for PC98 A20 line control by kaho Aug 5 1998

	title	switch between real and protected mode
	include segdefs.inc
	include tss.inc
	include gdt.inc
	include vcpi.inc

;------------------------------------------------------------------------

	start_data16

	extrn	_gdt:gdt_s
	extrn	_gdt_phys:gdt_s
	extrn	_idt_phys:gdt_s
	extrn	_tss_ptr:word
	extrn	_use_xms:byte
	extrn	_hard_master_lo:byte
	extrn	_hard_master_hi:byte
	extrn	_hard_slave_lo:byte
	extrn	_hard_slave_hi:byte
	extrn	_mtype:byte

	extrn	_vcpi_installed:byte	;VCPI Installed set this not Zero
	extrn	_vcpi_entry:fword
	extrn	_abs_client:dword
	extrn	_DPMIsp:word
	extrn	_saved_interrupt_vector:dword

	end_data16
	start_bss

	public	_dr
_dr	label	dword
_dr0	dd	?
_dr1	dd	?
_dr2	dd	?
_dr3	dd	?
	dd	?
	dd	?
_dr6	dd	?
_dr7	dd	?

	public	_c_tss, _a_tss, _o_tss, _f_tss
_c_tss	label	tss_s	; for "real mode" state
	db	type tss_s dup (?)
_a_tss	label	tss_s	; for running program
	db	type tss_s dup (?)
_o_tss	label	tss_s	; for convenience functions
	db	type tss_s dup (?)
_f_tss	label	tss_s	; for page handling
	db	type tss_s dup (?)

	public	_features
_features	dd	?		; CPUID features
	public	_was_exception
_was_exception	db	?		; exceptions set this to 1

	end_bss

;------------------------------------------------------------------------

	start_code16

	extrn	_xms_local_enable_a20:near
	extrn	_xms_local_disable_a20:near

real_stack	dd	?
real_idt	dw	03ffh, 0000h, 0000h

	public	_go32
_go32	proc	near
	push	si
	push	di
	push	bx
_go32_1:
;	push	0b000h				;dbg
;	pop	fs				;dbg
;	mov	word ptr fs:[0f01h],07h		;dbg - normal
	mov	ax,DGROUP			; Compute physical offset
	xor	dx,dx				; of tss_ptr
	shld	dx,ax,4				; Seg * 16
	shl	ax,4
	add	ax,_tss_ptr			; plus tss_ptr
	adc	dx,0
	mov	_gdt[g_atss].base0,ax			; Trick here - make atss
	mov	_gdt[g_atss].base1,dl			; selector point to
	mov	_gdt[g_atss].base2,dh			; tss_ptr (jmpt g_atss)


	mov	al,0fdh					; clear busy flag
	and	byte ptr _gdt[g_atss].stype,al
	and	byte ptr _gdt[g_ctss].stype,al		; non-VCPI LDTR below

	mov	_was_exception,0

; set real_stack for return from _go32
	mov	word ptr cs:real_stack,sp
	mov	word ptr cs:real_stack+2,ss
	movzx	esp,sp				; make sure it's OK

	cli

	cmp	_vcpi_installed,0
	je	short real_to_protect		;Not VCPI Mode
	mov	esi,_abs_client
	mov	ax,VCPI_MODE_CHANGE
	int	VCPI_REQ			;Change Protect Mode
;end_loop0:
;	jmp	short end_loop0			;Never Come here!! Safety Loop

real_to_protect:
	call	_set_a20
	or	_gdt[g_rdata].lim1,40h		;Set the big bit (non-vcpi only)
	lgdt	fword ptr _gdt_phys
	lidt	fword ptr _idt_phys

	mov	eax,cr0
	or	al,1
	mov	cr0,eax				; we're in protected mode!
	db	0eah				; far jmp
	dw	offset go_protect_far_jump
	dw	g_rcode
;
;	Entry Protect Mode
;
	public	_protect_entry
_protect_entry	label	near

go_protect_far_jump:
	cli
	mov     ax,g_rdata
	mov	ds,ax
	mov	es,ax
	mov	fs,ax
	mov	gs,ax
	mov	ss,ax
	movzx	esp,word ptr cs:real_stack	;Need VCPI Inialize
	xor	eax,eax
	mov	cr2,eax		 ;zero so we can tell INT 0E from page fault

	mov	eax,_dr0
	mov	dr0,eax
	mov	eax,_dr1
	mov	dr1,eax
	mov	eax,_dr2
	mov	dr2,eax
	mov	eax,_dr3
	mov	dr3,eax
	mov	eax,_dr6
	mov	dr6,eax
	mov	eax,_dr7
	mov	dr7,eax

	test	byte ptr _features,8		;Page Size Extensions?
	jz	short nopse
	db	0fh,20h,0e0h			;mov eax,cr4
	or	al,10h				;Enable 4Mb pages
	db	0fh,22h,0e0h			;mov cr4,eax
nopse:

	cmp	_vcpi_installed,0
	jne	short no_tss_load		;Now Paging Mode in VCPI
	mov	bx,_tss_ptr
	mov	eax,[bx].tss_cr3
	or	eax,eax				;Test for zero
	je	short set_paging_far_jump
	mov	cr3,eax
	mov	eax,cr0
	or	eax,80000000h
	mov	cr0,eax				; paging enabled!
	db	0eah				; far jmp
	dw	offset set_paging_far_jump
	dw	g_rcode
set_paging_far_jump:
	mov	ax,g_ctss
	ltr	ax
	jmp	short no_tss_load		; Chip errata fix from Symantec
no_tss_load:
	jmpt	g_atss				; load state from VCPU

_go32	endp

; _go_real_mode must follow the task jump, so a task jump to return
; is valid

	public	_go_real_mode
_go_real_mode	proc	near

	mov	eax,dr6
	mov	_dr6,eax

	cmp	_vcpi_installed,0
	je	short protect_to_real

;Make VCPI call to return to real
	clts				;TS Clear
	mov	eax,DGROUP
	push	eax			;GS
	push	eax			;FS
	push	eax			;DS
	push	eax			;ES
	push	eax			;SS
	movzx	eax,word ptr cs:real_stack
	push	eax			;ESP
	pushfd				;EFLAGS
	mov	ax,_TEXT		;high word still zero
	push	eax			;CS
	mov	ax,offset back_to_v86	;high word still zero
	push	eax			;EIP
;	movzx	esp,sp

	mov	ax,g_core
	mov	ds,ax
	mov	ax,VCPI_MODE_CHANGE
	assume	es:DGROUP
	call	fword ptr es:_vcpi_entry
;end_loop1:
;	jmp	short end_loop1		;Never Come here!! Safety Loop

protect_to_real:
	and	_gdt[g_rdata].lim1,0bfh		;Clear the big bit (non-vcpi only)
	push	ss
	pop	ss				;Reload
	mov	eax,cr0
	and	eax,07ffffff6h ; clear PE, TS, PG
	mov	cr0,eax

	db	0eah				; far jmp
	dw	offset back_to_real_far_jump
	dw	_TEXT
;
;	Entry Real Mode
;
back_to_real_far_jump:

	lidt	fword ptr cs:real_idt
	lss	sp,cs:real_stack
back_to_v86:

	mov	ax,ss
	mov	ds,ax
	mov	es,ax
	mov	fs,ax
	mov	gs,ax

;	call	_reset_a20

	cmp	_was_exception,0
	je	short not_hard

	mov	bx,_tss_ptr
	mov	al,[bx].tss_irqn

;	push	0b000h				;dbg
;	pop	fs				;dbg
;	mov	ah, 70h				;dbg - reverse
;	mov	word ptr fs:[0f00h],ax		;dbg

	mov	bx,16				;pseudo IRQ
	cmp	al,1ch
	je	short is_hard_1
	xor	bx,bx				;IRQ base
	cmp	al,_hard_master_lo
	jb	short try_slavepic
	cmp	al,_hard_master_hi
	jbe	short is_hard
try_slavepic:
	mov	bl,8				;IRQ base
	cmp	al,_hard_slave_lo
	jb	short not_hard
	cmp	al,_hard_slave_hi
	ja	short not_hard

is_hard:
;	mov	cl,_hard_slave_lo
;	add	cl,5
;	cmp	al,cl
;	je	short not_hard		; for NPX errors

;	mov	cl,_hard_master_lo
;	inc	cl
;	cmp	al,cl
;	je	short not_hard		; to check for ^C

	mov	ah,al
	and	ah,7			; IRQ offset
	add	bl,ah			; IRQ #
is_hard_1:
	shl	bx,2
	add	bx,offset _saved_interrupt_vector	;ES is already DS
	cmp	word ptr [bx+2],0
	jne	short redirect

	push	0			; Not redirected, use interrupt table
	pop	es
	movzx	bx,al
	shl	bx,2			; 4 bytes per interrupt entry
redirect:
	push	_DPMIsp			; Needed for RMCB's to be recursive
	mov	_DPMIsp,sp
	sub	_DPMIsp,spare_stack
	push	3002h
	call	dword ptr es:[bx]		; Really an interrupt
	pop	_DPMIsp
	jmp	_go32_1

not_hard:
	pop	bx
	pop	di
	pop	si
	ret

_go_real_mode	endp

;------------------------------------------------------------------------
	public	_reset_a20
_reset_a20	proc	near
	cmp	_vcpi_installed,0
	jne	short reset_a20_nop
	cmp	_use_xms,0
	je	short reset_a20_local
	call	_xms_local_disable_a20
reset_a20_nop:
	ret

reset_a20_pc98:
	mov	al,3
	out	0f6h,al		; disable the A20 line
	ret
reset_a20_local:
	cmp	_mtype,0	; PC98 raw mode
	jne	short reset_a20_pc98
	in	al,092h		; 092h PS/2 & clone system control port "A"
	and	al,not 2	; this resets the A20 bit in register al
	jmp	short $+2	; forget the instruction fetch
	out	092h,al		; set the A20 bit off
	ret
_reset_a20	endp

	public	_set_a20
_set_a20 proc	near
	cmp	_vcpi_installed,0
	jne	short set_a20_nop
	call	check_a20	; If already enabled skip A20 enable
	jz	short set_a20_nop
	cmp	_use_xms,0
	je	short set_a20_local
	call	_xms_local_enable_a20
set_a20_nop:
	ret

set_a20_pc98:
	mov	al,2
	out	0f6h,al		; enable the A20 line
	ret	
set_a20_local:
	cmp	_mtype,0	; PC98 raw mode
	jne	short set_a20_pc98
	pushf
	cli

	in	al,092h		; 092h PS/2 & clone system control port "A"
	or	al,2		; this sets the A20 bit in register al
	jmp	short $+2	; forget the instruction fetch
	out	092h,al		; set the A20 bit on

	call	check_a20
	jz	short a20_done

need_to_set_a20:
	call	waitkb
	mov	al,0d1h
	out	64h,al
	call	waitkb
	mov	al,0dfh		; Patrick
	out	60h,al
	call	waitkb
	mov	al,0ffh		; Patrick
	out	64h,al		; Patrick
	call	waitkb		; Patrick

wait_for_valid_a20:
	call	check_a20
	jnz	short wait_for_valid_a20
a20_done:
	popf
	ret

; interrupts disabled, ax, dx, es destroyed, returns z bit if a20 enabled
check_a20:
	; Modified by Prashant TR - don't use fs and gs here before switching 
	; to protmode.  FS/GS left from TC++ 3.0 can kill us when we come here.
	push	bx
	push    ds
	xor	ax,ax		; zero it
	mov     ds,ax
	dec	ax		; ax = 0ffffh
	mov     es,ax
	xor	bx,bx

	mov     ax,ds:[bx]      ; word from 0:0 (int 0 interrupt)
	mov	dx,ax		; save it
	not	ax		; make a different value
	xchg    ax,es:[bx+10h]  ; modify ffff:10h to known different value
	xchg    cx,ds:[bx]      ; get ds:[0] with a write
	cmp	dx,cx		; compare 0:0 with saved; z bit if same = a20
	mov     es:[bx+10h],ax  ; restore
	mov     ds:[bx],dx

	pop     ds
	pop	bx
	ret

waitkb:
	xor	cx,cx		; zero it
waitkb1:
	jmp	short $+2
	jcxz	short $+2
	in	al,64h
	test	al,2
	loopnz	waitkb1
	ret
_set_a20 endp

;------------------------------------------------------------------------

	public	_cpumode	; 0=real mode, 1=V86
_cpumode:
	smsw	ax
	and	ax,1
	ret

;------------------------------------------------------------------------
; Determination of Cpu type.  EAX, EBX, ECX, EDX destroyed.
;	0  for 8086/80186
;	2  for 80286
;	3  for 386
;	4  for 486
;	5  for 586 (Pentium) or better (thanks, Morten!)

cpuid	macro
	db	0fh,0a2h
	endm

	public	_cputype	; from Intel 80486 reference manual
_cputype:
	xor	cx,cx		; default type is 8086
	pushf
	pop	bx
	and	bh,0fh
	push	bx
	popf
	pushf
	pop	ax
	and	ax,0f000h
	cmp	ax,0f000h
	jz	short cpu8086
	or	bh,0f0h
	push	bx
	popf
	pushf
	pop	ax
	and	ax,0f000h
	jz	short cpu286

	mov	dx,sp		; Save old SP
	and	sp,not 3	; Align it! (we will set AC flag)
	mov	al,18		; AC flag number
	call	cpuflipflag
	mov	sp,dx
	jnc	short cpu386
	mov	al,21		; ID flag number
	call	cpuflipflag
	jnc	short cpu486

;The cpu supports the CPUID instruction.  Maybe Pentium or late 486?
	xor	eax,eax
	cpuid			; ebx:edx:ecx = vendor id; eax = max support
	or	eax,eax
	mov	al,5
	jz	short cpu_pentium
	xor	eax,eax
	inc	ax		; call with eax = 1
	cpuid			; edx=features; ah&0f = family; al=model/step
	mov	al,ah
	mov	_features,edx
cpu_pentium:
	and	ax,0fh
	ret	
cpu486:
	inc	cx
cpu386:
	inc	cx
cpu286:
	inc	cx
	inc	cx
cpu8086:				; or 186
	mov	ax,cx
	ret

cpuflipflag:		; Try to flip flag #Al in Eflags, Return C=1 if possible
	push	si	; This plus the pushed ret address still stack aligned
	movzx	esi,al
	pushfd
	pop	eax
	mov	ebx,eax
	btc	eax,esi
	push	eax
	popfd
	pushfd
	pop	eax
	xor	eax,ebx
	push	ebx
	popfd
	bt	eax,esi
	pop	si
	ret
;------------------------------------------------------------------------

	end_code16

	end
