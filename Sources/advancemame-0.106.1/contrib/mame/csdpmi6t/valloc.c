/* Copyright (C) 1995-1999 CW Sandmann (sandmann@clio.rice.edu) 1206 Braelinn, Sugar Land, TX 77479
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
/* Modified for VCPI Implement by Y.Shibata Aug 5th 1991 */
/* Large physical mem size detection by Prashant TR. (prashant_tr@yahoo.com) */

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <dos.h>

#include "gotypes.h"
#include "valloc.h"
#include "xms.h"
#include "vcpi.h"
#include "paging.h"
#include "control.h"
#include "mswitch.h"

#define NPAGEDIR CWSpar.pagedir /* In low memory * 4K, user gets 3 less, 4Mb/entry */
#define MINAPPMEM CWSpar.minapp	/* Min pages extended before we use DOS memory */
#define SAVEPARA CWSpar.savepar	/* If we use DOS memory for paging, amt to save */
#define MINPAGEDIR 4		/* May round down, 1PD, 2PT */
#define PAGE2PARA 256
#define KB2PARA 64

static word8 far *map;		/* Expanded/Extended paged by valloc() */
extern word8 far *dmap;		/* Paging memory allocated here too */

static va_pn mem_avail, mem_used;

static unsigned pn_lo_first, pn_lo_last, pn_lo_next;
static va_pn pn_hi_first, pn_hi_last, pn_hi_next;
static char valloc_initted = 0;
static char use_vcpi = 0;

static int16 emb_handle=-1;

static void xms_free(void)
{
  if(use_xms && emb_handle != -1) {
    xms_unlock_emb(emb_handle);
    xms_emb_free(emb_handle);
    emb_handle = -1;
  }
}

static void xms_alloc_init(void)
{
  word32 linear_base;
  word32 emb_size;
  if((emb_size = xms_query_extended_memory()) != 0) {
    emb_handle = xms_emb_allocate(emb_size);
    linear_base = xms_lock_emb(emb_handle);
    pn_hi_first = (va_pn)((linear_base + 4095)/4096);
    pn_hi_last = (va_pn)((linear_base + (emb_size << 10))/4096 - 1);
  } else {
    pn_hi_first = 1;
    pn_hi_last = 0;
  }
  SHOW_MEM_INFO("XMS memory: %ld Kb",(((word32)pn_hi_last-pn_hi_first+1) * 4));
}

static unsigned valloc_lowmem_page;
static unsigned lol;
static unsigned desired_pt;
static unsigned strategy, umbstat;
static unsigned mempid;
static unsigned oldmempid;
static unsigned extrapara;
static word8 valid;

static void set_umb(void)
{
  oldmempid = get_pid();
  if(mempid) {
    if(oldmempid != mempid)
      set_pid(mempid);
  } else
    mempid = oldmempid;

  if (_osmajor >= 5) {
    _AX = 0x5800;
    geninterrupt(0x21);	/* Get allocation strategy */
    strategy = _AX;

    _AX = 0x5802;
    geninterrupt(0x21);	/* Get UMB status */
    umbstat = _AX;
    
    _AX = 0x5801;
    _BX = 0x0080;
    geninterrupt(0x21);	/* Set first fit high, then low */

    _AX = 0x5803;
    _BX = 0x0001;
    geninterrupt(0x21);	/* Include UMB in memory chain */
  }
}

static void restore_umb(void)
{
  if (_osmajor >= 5) {
    _AX = 0x5803;
    _BX = umbstat;
    _BH = 0;
    geninterrupt(0x21);	/* Restore memory chain UMB usage */

    _AX = 0x5801;
    _BX = strategy;
    geninterrupt(0x21);	/* Restore allocation stragegy */
  }
  if(oldmempid != mempid)
      set_pid(oldmempid);
}

static int alloc_pagetables(int mintable, int wanttable)
{
  unsigned pagebase;

  set_umb();
  _AH = 0x48;		/* get real memory size */
  _BX = 0xffff;
  geninterrupt(0x21);	/* lol == size of largest free memory block */
  lol = _BX;

  if (lol < mintable*PAGE2PARA + extrapara)	/* 1 PD, 1 PT (real), 1 PT (user) */
    goto mem_exit;

  lol -= extrapara;
  if (lol > wanttable*PAGE2PARA) {	/* 8 will probably result in 5 user pt */
    if (mem_avail > MINAPPMEM)			/* 256K extended */
      lol = wanttable*PAGE2PARA;
    else {
      if (lol > wanttable*PAGE2PARA+SAVEPARA)
        lol -= SAVEPARA;			/* Reserve extra DOS memory */
      mem_avail += (lol >> 8) - wanttable;
    }
  }

  lol += extrapara;
  _BX = lol;
  _AH = 0x48;
  geninterrupt(0x21);		/* get the block */
  valloc_lowmem_page = _AX;
  if (_FLAGS & 1) {
mem_exit:
    valid = 0;
    restore_umb();
    return 1;
  }

  valid = 1;
  pagebase = valloc_lowmem_page + extrapara;
  /* shrink memory to 4K align */
  if (pagebase & 0xFF) {
    lol -= (pagebase & 0xFF);
    _ES = valloc_lowmem_page;
    _BX = lol;
    _AH = 0x4A;
    geninterrupt(0x21);
  }
  restore_umb();

  pn_lo_next = pn_lo_first = (pagebase+0xFF) >> 8;	/* lowest real mem 4K block */
  pn_lo_last = (valloc_lowmem_page+lol-0x100)>>8;	/* highest real mem 4K block */
  return 0;
}

void valloc_init(word16 ies)
{
  if (valloc_initted)
    return;

  if (vcpi_installed) {
    pn_hi_first = 0;
    pn_hi_last  = vcpi_maxpage();
    if (vcpi_capacity()) {
      use_vcpi = 1;
      SHOW_MEM_INFO("VCPI memory: %ld Kb", (vcpi_capacity() * 4L));
    } else if(use_xms) {
      use_vcpi = 0;	/* Just in case multiple pass with all allocated */
      xms_alloc_init();	/* Use XMS memory with VCPI mode switch */
    }
  } else if (use_xms) {
    xms_alloc_init();	/* Try XMS allocation */
    if (cpumode()) {
      errmsg("\nError: Using XMS switched CPU into V86 mode.\n");
      xms_free();
      _exit(1);
    }
  } else if (mtype == PC98) {
    pn_hi_first = 256;
    /* modified for RAW memory support by kaho Aug 5 1998
     * e-mail address is mxk02052@nifty.ne.jp or kaho@elam.kais.kyoto-u.ac.jp */
    /* get 1-16MB region memory size */
    pn_hi_last = 255 + (*(unsigned char far *)0x401L)<<5;
    /* get above 16MB region memory size */
    if(pn_hi_last == 4095)
      pn_hi_last += (*(unsigned short far *)0x594L)<<8;
  } else {
    /* int 15/vdisk memory allocation */
    /* Bug here - we should hook int 0x15 and reduce size, but who cares? */
    unsigned char far *vdisk;
    _AH = 0x88;		/* get extended memory size */
    geninterrupt(0x15);
    pn_hi_last = _AX / 4 + 255;

    /* Suggested interrupt call by Prashant TR. (prashant_tr@yahoo.com). */
    _AX=0xE801;
    _BX=0;
    _CX=0;
    _DX=0;
    geninterrupt(0x15);
    if ( ! (_FLAGS & 1) && _AX == 0x3c00)
    	pn_hi_last = ((va_pn)_BX << 4) + 0xfff;		/* 0x3c00 / 4 + 255 */

    /* get ivec 19h, seg only */
    vdisk = MK_FP(peek(0,0x19*4+2),0);
    if(*(word32 far *)(vdisk+18) == 0x53494456UL)  /* 'VDIS' */
      pn_hi_first = 0xfff & (va_pn)((0xfffL + *(word32 far *)(vdisk+44)) >> 12);
    else
      pn_hi_first = 256;
    SHOW_MEM_INFO("Extended memory: %ld Kb", (((word32)pn_hi_last-pn_hi_first) * 4));
  }
  pn_hi_next = pn_hi_first;
  if(pn_hi_last > 524279UL)
    pn_hi_last = 524279UL;
  mem_avail = (use_vcpi)? vcpi_capacity():((long)pn_hi_last-pn_hi_first+1);
  
  if(NPAGEDIR)				/* Specified */
    desired_pt = 3 + NPAGEDIR;
  else {				/* Zero means automatic */
    desired_pt = 4 + (mem_avail>>10);	/* All physical mem plus 1 extra */
    if(desired_pt < 8)
      desired_pt = 8;
    else if(desired_pt > 36)		/* Arbitrary 128Mb worth max */
      desired_pt = 36;
  }

  extrapara = (word16)(CWSpar.maxdblock >> 7) + (word16)(pn_hi_last >> 7) + 2;
  mempid = 0;

  if(CWSFLAG_EARLY) {
    valid = 0;						/* Give up on resize */
    valloc_lowmem_page = ies + 16;			/* For TSS */
    pn_lo_next = pn_lo_first = (valloc_lowmem_page + extrapara + 0xff) >> 8;
    pn_lo_last = (ies >> 8) + CWSpar.pagedir + 4;
  } else if(alloc_pagetables(MINPAGEDIR, desired_pt)) {
    errmsg("Error: could not allocate page table memory\n");
    xms_free();
    _exit(1);
  }
  
  map = MK_FP(valloc_lowmem_page, 0);
  memsetf(0, 0, (word16)((pn_hi_last + 7) >> 3), valloc_lowmem_page);

  dmap = MK_FP(valloc_lowmem_page + 1 + (word16)(pn_hi_last >> 7), 0);
  memsetf(0, 0, (word16)((CWSpar.maxdblock + 7) >> 3), FP_SEG(dmap) );

  extrapara = 0;
  mem_used = 0;
  valloc_initted = 1;
  set_a20();
}

static void vset(va_pn i, char b)
{
  unsigned o;
  word8 m;
  o = (unsigned)(i>>3);
  m = 1<<((unsigned)i&7);
  if (b)
    map[o] |= m;
  else
    map[o] &= ~m;
}

static word8 vtest(va_pn i)
{
  unsigned o;
  word8 m;
  o = (unsigned)(i>>3);
  m = 1<<((unsigned)i&7);
  return map[o] & m;
}

static void vcpi_flush(void)		/* only called on exit */
{
  va_pn pn;

  if (!use_vcpi)
    return;			/*  Not Initaialized Map[]  */
  for(pn = 0; pn <= pn_hi_last; pn++)
    if (vtest(pn))
      vcpi_free(pn);
}

void valloc_uninit(void)
{
  if (!valloc_initted)
    return;

  /* free the block we allocated - DOS does this
  _ES = valloc_lowmem_page;
  _AH = 0x49;
  geninterrupt(0x21); */

  xms_free();
  vcpi_flush();		/*  Deallocated VCPI Pages  */
  valloc_initted = 0;
  reset_a20();
}

unsigned valloc_640(void)
{
  unsigned pn;
  if (pn_lo_next <= pn_lo_last) {
    return pn_lo_last--;		/* Never deallocated! */
  }

  /* First try to resize current block instead of paging */
  if(valid) {
    int failure;
    set_umb();
    lol += PAGE2PARA;
    _ES = valloc_lowmem_page;
    _BX = lol;
    _AH = 0x4A;
    geninterrupt(0x21);
    failure = _FLAGS & 1;
    restore_umb();
    if(!failure)
      return (valloc_lowmem_page+lol-0x100)>>8;
  }
  /* Okay, not unexpected.  Allocate a new block, we can 
     hopefully resize it later if needed. */
  if(!alloc_pagetables(2,2))
    return pn_lo_last--;

  pn = page_out_640();
  if (pn == 0xffff) {
    errmsg("Error: could not allocate page table memory\n");
    cleanup(1);
  }
  return pn;
}

va_pn valloc(void)
{
  va_pn pn;
  if (use_vcpi) {
    if ((pn = vcpi_alloc()) != 0) {
      mem_used++;
      vset(pn, 1);
      return pn;
    }
  } else {
    for (pn=pn_hi_next; pn<=pn_hi_last; pn++) 
      if (!vtest(pn)) {
        pn_hi_next = pn+1;
        mem_used++;
        vset(pn, 1);
        return pn;
      }
  }

  /* This section is only used if we are paging in 1Mb area; save for PDs */
  /* Note, if VCPI memory runs out before we get mem_avail also end up here */
  if (mem_used < mem_avail && pn_lo_next < 4+pn_lo_last-desired_pt) {
    mem_used++;
    return (va_pn)(vcpi_pt[pn_lo_next++] >> 12);
  }

  return page_out();
}

va_pn valloc4m(void)
{
  va_pn pn;
  word16 i, j;
  if (use_vcpi)
    return 0;				/* Never valid */

  pn = (pn_hi_next + 0x3ff) & ~0x3ff;
  while(pn+1023 <= pn_hi_last) {
    i = pn/8;
    for(j=0; j<128; j++)
      if(map[i+j])
        break;
    if(j == 128) {	 	/* for(j=0; j<128; j++) map[i+j] = 0xff; */
      memsetf(FP_OFF(map)+i, 0xff, 128, FP_SEG(map));
      return pn;
    }
    pn += 1024;
  }
  return 0;
}

void vfree4m(va_pn pn)
{
  word16 j;

  for(j=0; j<1024; j++)
    vfree(pn++);
}

/* If we are able to find the page, return 1 */
int vfree(va_pn pn)
{
  if (vtest(pn)) {
    vset(pn, 0);
    if (use_vcpi)
      vcpi_free(pn);
    else if(pn < pn_hi_next)
      pn_hi_next = pn;
    mem_used--;
    return 1;
  } 
  if (pn == vcpi_pt[pn_lo_next-1]) {
    pn_lo_next--;
    mem_used--;
    return 1;
  }
  return 0;
}

va_pn valloc_max_size(void)
{
  return mem_avail;
}

va_pn valloc_used(void)
{
  return mem_used;
}
