/* Copyright (C) 1995-2000 CW Sandmann (sandmann@clio.rice.edu) 1206 Braelinn, Sugar Land, TX 77479
** Copyright (C) 1993 DJ Delorie, 24 Kirsten Ave, Rochester NH 03867-2954
**
** This file is distributed under the terms listed in the document
** "copying.cws", available from CW Sandmann at the address above.
** A copy of "copying.cws" should accompany this file; if not, a copy
** should be available from where this file was obtained.  This file
** may not be distributed without a verbatim copy of "copying.cws".
**
** This file is distributed WITHOUT ANY WARRANTY; without even the implied
** warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
/* PC98 support contributed 6-95 tantan SGL00213@niftyserve.or.jp */

#include <dos.h>
#include <stdlib.h>
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <io.h>

#include "gotypes.h"
#include "gdt.h"
#include "idt.h"
#include "tss.h"
#include "valloc.h"
#include "utils.h"
#include "vcpi.h"
#include "paging.h"
#include "exphdlr.h"
#include "dalloc.h"
#include "mswitch.h"
#include "xms.h"
#include "control.h"
#include "dpmisim.h"

/* Note, the near heap and stack are pooled.  EHDRFIX.C reads the lines below */
/* and adds paragraphs required to the exe header.  Each task takes 304 bytes */
/* (dpmisim stack + 0x30), each memory zone 14 bytes, each HW int around 850 */
/* paging space flags need 32 bytes per Mb (8Kb for 256Mb max) return unused */
extern unsigned _stklen = 4096U;	/* Plus heap added in exe hdr */
extern unsigned int _brklvl[2];		/* hi word is free seg after our prog */
extern void _restorezero(void);

static word16 current_es = 0;
static word16 old_env,current_psp;
static word8 one_pass = 1;
word8 mtype = 0;			/* Machine type */

DESC_S gdt[g_num];
DESC_S ldt[l_num];
IDT idt[256];
TSS *tss_ptr;

word8 vcpi_installed = 0;	/*  VCPI Installed Flag  */
word8 use_xms=0;

CWSDPMI_pblk CWSpar = { "CWSPBLK", "c:\\cwsdpmi.swp", 0, 0, 128, 3840, 32768UL };

static char *exception_names[] = {
  "Division by Zero",
  "Debug",
  "NMI",
  "Breakpoint",
  "Overflow",
  "Bounds Check",
  "Invalid Opcode",
  "FPU unavailable",
  "Double Fault",
  "FPU overrun",
  "Invalid TSS",
  "Segment Not Present",
  "Stack Fault",
  "General Protection Fault",
  "Page Fault",
  0,
  "FPU Error",
};
#define EXCEPTION_COUNT (sizeof(exception_names)/sizeof(exception_names[0]))

word16 get_pid(void)
{
  _AH = 0x62;
  geninterrupt(0x21);
  return _BX;
}

void set_pid(word16 pid)
{
  _BX = pid;
  _AH = 0x50;
  geninterrupt(0x21);
}

static void fill_desc(DESC_S *g, word32 limit, word32 base, word8 type, int G)
{
  if (G & 2)			/* Granularity bit = 4K */
    limit = limit >> 12;
  g->lim0 = (word16)limit;
  g->lim1 = (limit>>16) & 0x0f;
  g->base0 = (word16)base;
  g->base1 = (word8)(base>>16);
  g->base2 = (word8)(base>>24);
  g->stype = type;
  g->lim1 |= G * 0x40;		/* Shift field to set G & D bits */
}

static void setup_tss(TSS *t, void (*eip)())
{
  memset(t, 0, sizeof(TSS));
  /* At this point EFLAGS = 0 (interrupt disabled) and all selectors null */
  t->tss_cs = g_rcode*8;
  (word16)t->tss_eip = FP_OFF(eip);
  t->tss_ss0 = t->tss_ss = t->tss_ds = g_rdata*8;
  (word16)t->tss_esp0 = (word16)t->tss_esp = FP_OFF(t->tss_stack);
  t->tss_ldt = g_ldt*8;
}

/* This routine is called in context of the other process */
void cleanup(int exitcode)
{
  int lastused;
  word16 far *envptr;
  envptr = MK_FP(current_psp,0x2c);
  *envptr = old_env;

  lastused = a_tss.tss_lastused;
  current_es = a_tss.tss_cur_es;		/* Saved ES if needed */
  old_env = a_tss.tss_old_env;
  current_psp = a_tss.tss_cur_psp;
  if (current_es) {			/* There is back link, restore old TSS */
    AREAS *area = firstarea;
    AREAS **lasta = &firstarea;
    word16 lua = (word16)lastused >> 8;
    while (lua--) {
      lasta = &area->next;
      area = area->next;
    }
    while (*lasta && free_memory_area((*lasta)->first_addr)) ;
    lastused &= 0xff;
    for(lastused++; lastused<l_num; lastused++)
      ldt[lastused].stype = 0;		/* Mark them all as available */

    movedata(current_es, 0, _DS, FP_OFF(&a_tss), 6*16);
  } else {				/* This is the last nested process */
    dalloc_uninit();
    _AH = 0x19; geninterrupt(0x21);	/* DOS call to work around hangup bug */
    uninit_controllers();
    valloc_uninit();
    if(CWSFLAG_EARLY)
      init_size = (CWSpar.pagedir + 5) << 8;
    if (one_pass || a_tss.tss_ebx == ONE_PASS_MAGIC) { /* "magic" for unload */
      setvect(0x2f,oldint2f);

      ems_free();		/* Deallocated EMS Page */

      disable();
      _ES = _psp;		/* Deallocate TSR we are currently executing! */
      _AH = 0x49;
      geninterrupt(0x21);
    }
  }

  _AL = (word8)exitcode;
  _AH = 0x4c;
  geninterrupt(0x21);
}

static void itox(char *buf, int v)
{
  _DX = v;
  buf += 3;
  for(_CX=4;_CX;_CX--) {
    _AL = _DL & 15;
    _DX >>= 4;
    _AL += '0';
    if (_AL > '9')
      _AL += 'a' - '9' - 1;
    *buf-- = _AL;
  }
}

void errmsg(char *fmt, ...)
{
  char *p,*s;
  int *q;
  q = &(int)fmt + 1;
  p = s = fmt;
  while((_AL=*p) != 0) {
    if(_AL == '\n'){
      _write(2,s,p-s);
      _write(2,"\r\n",2);
      s = ++p;
    } else if(_AL == '%') {
      _write(2,s,p-s);
      _AL = *++p;
      if(_AL == 's')
        errmsg((char *)(*q++));
      else {
        int n = 0;
        int v2 = 0;
        int v1 = *q++;
        char buf[8];
        _AL = *p;
        while(_AL != 'x') {
          if(_AL >= '0' && _AL <= '8')
            n = _AL - '0';
          else if(_AL == 'l')
            v2 = *q++;
          _AL = *++p;
        }
        itox(buf,v2);
        itox(buf+4,v1);
        if(n == 0) {
          n=8;
          while(buf[8-n] == '0' && n > 1) n--;
        }
        _write(2,buf+8-n,n);
      }
      s = ++p;
    } else
      p++;
  }
  _write(2,s,p-s);
}

void do_faulting_finish_message(void)
{
  extern char in_rmcb;
  char *en = (tss_ptr->tss_irqn >= EXCEPTION_COUNT) ? 0 : exception_names[tss_ptr->tss_irqn];
  if (en == 0)
    errmsg("Int 0x%02x", tss_ptr->tss_irqn);
  else
    errmsg("%s", en);
  if(tss_ptr->tss_irqn == 14)
    errmsg(" cr2=%08lx", tss_ptr->tss_cr2);
  if(in_rmcb)
    errmsg(" in RMCB");
  errmsg(" at eip=%lx; flags=%x\n", tss_ptr->tss_eip, (word16)tss_ptr->tss_eflags);
  errmsg("eax=%08lx ebx=%08lx ecx=%08lx edx=%08lx esi=%08lx edi=%08lx\n",
	tss_ptr->tss_eax, tss_ptr->tss_ebx, tss_ptr->tss_ecx, tss_ptr->tss_edx,
	tss_ptr->tss_esi, tss_ptr->tss_edi);
  errmsg("ebp=%08lx esp=%08lx cs=%x ds=%x es=%x fs=%x gs=%x ss=%x error=%04x\n",
	tss_ptr->tss_ebp, tss_ptr->tss_esp,
	tss_ptr->tss_cs, tss_ptr->tss_ds, tss_ptr->tss_es, tss_ptr->tss_fs,
	tss_ptr->tss_gs, tss_ptr->tss_ss, (word16)tss_ptr->tss_error);
  cleanup(1);
}

void main1(void)	/* int argc, char **argv) */
{
  /* Check for cpu type in assembly main to avoid 8088 problems with 'enter' */

#ifndef STUB
  if (_osmajor < 3) {
    errmsg("DOS 3 required.\n");
    exit(1);
  }

  _restorezero();
#endif

  if(CWSFLAG_NOUMB)
    _osmajor = 4;	/* Don't try to use UMB */

  if(*(word16 far *)MK_FP(0xf000, 0xfff3) == 0xfd80) {
    hard_slave_lo = 0x10;		/* PC98 slave */
    mtype = PC98;
  }

#if run_ring != 0
  dalloc_file(CWSpar.swapname);		/* default */
#else
  CWSpar.maxdblock = 0;
#endif

#ifndef STUB
  {
  char far *ptr;
  int i,nc;

  ptr = MK_FP(_psp, 0x80);
  nc = *ptr++;
  ptr[nc] = 0;
  for(i=0;i<nc;i++) {
    if(ptr[i] == '-') {
      char test = 0x20 | ptr[++i];	/* make lower case if upper */
      errmsg("CWSDPMI V0.90+ (6b) Copyright (C) 2003 CW Sandmann  ABSOLUTELY NO WARRANTY\n");
      if(test == 'p')			/* persistent, permanent */
        one_pass = 0;
      else if(test == 'x')		/* no eXtensions */
        CWSpar.flags |= 4;
      else if(test == 'u') {		/* unload */
        extern void unload_tsr(void);
        unload_tsr();
      } else if(test == 's'){		/* swapfile; -s- means no virtual mem */
        int j;
        char t;
        char *swap = malloc(nc-i);
        for(j=0,i++,t=32;t;i++){
          t = ptr[i];
          if(t != ' ' && t != 9)	/* skip whitespace */
            swap[j++] = t;
        }
#if run_ring != 0
        if(swap[0] == '-')
          swap[0] = 0;
        dalloc_file(swap);
#endif
      }
    }
  }
  }
#endif

  use_xms = xms_installed();

  ems_init();
  if (cpumode()) {		/* We are in V86 mode */
    if (!(vcpi_installed = vcpi_present())) {
      ems_free();
      errmsg("Protected mode not accessible.\n");
      exit(1);
    }
  }

#if run_ring != 0
  gdt[g_iret].lim0 = (word16)ring0_iret;	/* Call gate to do IRET */
  gdt[g_iret].base0 = g_rcode*8;
  gdt[g_iret].stype = SEL_PRV | 0x8c;
#else
  fill_desc(&gdt[g_iret], sizeof(TSS)-1, ptr2linear(&f_tss), 0x89, 1);
#endif
  fill_desc(&gdt[g_gdt], sizeof(gdt)-1, ptr2linear(gdt), 0x92, 0);
  fill_desc(&gdt[g_idt], sizeof(idt)-1, ptr2linear(idt), 0x92, 0);
  fill_desc(&gdt[g_ldt], sizeof(ldt)-1, ptr2linear(ldt), 0x82, 1);
  fill_desc(&gdt[g_rcode], 0xffff, (word32)_CS*16L, 0x9a, 0);
  fill_desc(&gdt[g_rdata], 0xffff, (word32)_DS*16L, 0x92, 1); /* 1 for ring 3 */
  fill_desc(&gdt[g_core], 0xffffffffL, 0, SEL_PRV | 0x92, 3);
  fill_desc(&gdt[g_BIOSdata], 0xffff, (word32)0x400, SEL_PRV | 0x92, 0);
  fill_desc(&gdt[g_pcode], 0xffff, (word32)_CS*16L, SEL_PRV | 0x9a, 0);
  fill_desc(&gdt[g_pdata], 0xffff, (word32)_DS*16L, SEL_PRV | 0x92, 1);

  fill_desc(&gdt[g_ctss], sizeof(TSS)-1, ptr2linear(&c_tss), SEL_PRV | 0x89, 1);
  fill_desc(&gdt[g_atss], sizeof(TSS)-1, ptr2linear(&a_tss), 0x89, 1);
  fill_desc(&gdt[g_itss], sizeof(TSS)-1, ptr2linear(&i_tss), SEL_PRV | 0x89, 1);

  oldint2f = getvect(0x2f);
  setvect(0x2f, dpmiint2f);

  if(CWSFLAG_EARLY)
    init_size = (CWSpar.pagedir + 5) << 8;

  DPMIsp = _SP;

#ifndef STUB
  _ES = peek(_psp,0x2c);	/* Deallocate TSR environment */
  _AH = 0x49;
  geninterrupt(0x21);
  _close(0); _close(1);		/* Close stdin, stdout, AUX, PRN */
  _close(3); _close(4);

  _BX = 2; _AH = 0x3e; geninterrupt(0x21);	/* Close stderr, tc thinks open */

  _DX = _brklvl[1] - _psp;
  _AX = 0x3100;
  geninterrupt(0x21);
#endif
}

void DPMIstartup(void)
{
  {
  int use32,myES;
  int acode,adata,apsp,aenv,astack;
  int lastused;		/* Keep track of selectors */
  word16 dpmipsp;
  word16 far *envptr;
  myES = _ES;
  tss_ptr = &a_tss;
  if (current_es){
    int lua = 0;		/* Last used area */
    AREAS *area = firstarea;
    while (area) {
     lua++;
     area = area->next;
    }
    for(lastused=l_num-1;lastused>l_free && !(ldt[lastused].stype); lastused--);
    lastused |= lua << 8;
    movedata(_DS, FP_OFF(&a_tss), current_es, 0, 6*16); /* Save old TSS */
    acode = alloc_ldt(1);
    adata = alloc_ldt(1);
    apsp = alloc_ldt(1);
    if((aenv = alloc_ldt(1)) == 0) {
      errmsg("Descriptors exhausted.\n");
      exit(1);
    }
  } else {
    int i,n;
    setup_tss(&c_tss, go_real_mode);
    setup_tss(&a_tss, go_real_mode);
    setup_tss(&o_tss, go_real_mode);
#if run_ring == 0
    { void double_fault(void); setup_tss(&f_tss, double_fault); }
#else
    setup_tss(&f_tss, go_real_mode);
#endif
    setup_tss(&i_tss, interrupt_common);

    memset(ldt,0,sizeof(ldt));	/* Zero it; this make all stypes = 0 not used */
    lastused=l_free-1;		/* lua = 0 */
    /* Note - this shortcut will make these look free to alloc_ldt until set */
    acode = l_acode;
    adata = l_adata;
    apsp = l_apsp;
    aenv = l_aenv;
    dr[7] = 0L;			/* Clear all breakpoints */

    dalloc_init();
    valloc_init(myES);
    paging_setup();
    init_controllers();

    n = (int)ivec1-(int)ivec0;
    for (i=0; i<256; i++)
    {
      idt[i].selector = GDT_SEL(g_pcode);
      idt[i].stype = 0xee00;
      idt[i].offset0 = (int)FP_OFF(ivec0) + n*i;
      idt[i].offset1 = 0;
    }
    for (i=0; i<16; i++)
      idt[i].selector = g_rcode*8;
    for (i=hard_master_lo; i<=hard_master_hi; i++)
      idt[i].selector = g_rcode*8;
    for (i=hard_slave_lo; i<=hard_slave_hi; i++)
      idt[i].selector = g_rcode*8;
#ifdef I31PROT
    idt[0x31].offset0 = (int)FP_OFF(ivec31);
#endif
    idt[7].offset0 = (int)ivec7;		/* To handle TS bit set faults */
#if run_ring == 0
    i = 8;	/* Double fault */
    idt[i].selector = g_iret*8; idt[i].stype = 0xe500; idt[i].offset0 = 0;
#endif
  }
  movedata(myES, 32, _DS, FP_OFF(&a_tss.tss_eip), 4*14);
  init_size = 6;

  use32 = (word16)a_tss.tss_eax & 1;

  /* Hack alert! Since ring1 & ring2 not used, I re-used this storage to keep
     track of selector usage (to aid clean up), back link to the previous
     allocated area segment, the old environment segment, and the PSP */

  a_tss.tss_cur_es = current_es;	/* Back link to saved TSS */
  a_tss.tss_lastused = lastused;
  current_es = myES;			/* Save mine for child if any */
  a_tss.tss_old_env = old_env;		/* Saved */
  a_tss.tss_cur_psp = current_psp;

  a_tss.tss_fs = a_tss.tss_gs = 0;
  fill_desc(&ldt[acode], 0xffff, (word32)a_tss.tss_cs*16L, SEL_PRV | 0x9a, 0);
  a_tss.tss_cs = LDT_SEL(acode);
  fill_desc(&ldt[adata], 0xffff, (word32)a_tss.tss_ds*16L, SEL_PRV | 0x92, use32);
  a_tss.tss_eflags = 0x3202;

  _AH = 0x62;				/* Get PSP */
  geninterrupt(0x21);
  dpmipsp = _BX;
  current_psp = dpmipsp;
  envptr = MK_FP(dpmipsp,0x2c);
  old_env = *envptr;
  *envptr = LDT_SEL(aenv);
  fill_desc(&ldt[apsp], 0xffff, (word32)dpmipsp*16L, SEL_PRV | 0x92, 0); /* lim should be ff */
  a_tss.tss_es = LDT_SEL(apsp);
  fill_desc(&ldt[aenv], 0xffff, (word32)old_env*16L, SEL_PRV | 0x92, 0);
  /* Must come after all other descriptors set up, since calls alloc_ldt() */
  if (a_tss.tss_ds != a_tss.tss_ss) {
    astack = alloc_ldt(1);
    fill_desc(&ldt[astack], 0xffff, (word32)a_tss.tss_ss*16L, SEL_PRV | 0x92, use32);
  } else
    astack = adata;

  a_tss.tss_ds = LDT_SEL(adata);
  a_tss.tss_ss = LDT_SEL(astack);
  setvect(0x23,int23);
  setvect(0x24,int24);
  }
}
