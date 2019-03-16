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
/* NUR paging algorithm by rcharif@math.utexas.edu */

#include <stdio.h>
#include <stdlib.h>
#include <dos.h>
#include <string.h>

#include "gotypes.h"
#include "valloc.h"
#include "paging.h"
#include "tss.h"
#include "idt.h"
#include "gdt.h"
#include "dalloc.h"
#include "utils.h"
#include "vcpi.h"
#include "exphdlr.h"
#include "control.h"
#include "mswitch.h"

#define DOS_PAGE 272		/*  1MB+64K / 4KB = 272 Pages  */

struct {
  word16 limit;
  word32 base;
} gdt_phys, idt_phys;

static CLIENT client;		/*  VCPI Change Mode Structure  */
word32 abs_client;		/*  _DS * 16L + &client         */
far32 vcpi_entry;

AREAS *firstarea = NULL;

static word32 far *pd = 0;
static word8 pd_seg[1024];
word32 far *vcpi_pt = 0;
static word8 paging_buffer[4096];
static word8 USE_4M;

word32 ptr2linear(void far *ptr)
{
  return (word32)FP_SEG(ptr) * 16L + (word32)FP_OFF(ptr);
}

static word32 far2pte(void far *ptr, word16 flags)
{
  return (vcpi_pt[(int)(((word32)ptr) >> 24)] & 0xfffff000L) | flags;
}

static word32 pn2pte(unsigned pn, word16 flags)
{
  return (vcpi_pt[pn] & 0xfffff000L) | flags;
}

/*  VCPI Get Interface  */
static void link_vcpi(word32 far *dir, word32 far *table)
{
  vcpi_entry.selector = g_vcpicode * 8;
  vcpi_entry.offset32 = get_interface(table,&gdt[g_vcpicode]);
  client.page_table   = far2pte(dir, 0);
  client.gdt_address  = ptr2linear(&gdt_phys);
  client.idt_address  = ptr2linear(&idt_phys);
  client.ldt_selector = g_ldt * 8;
  client.tss_selector = g_ctss * 8;
  client.entry_eip    = (word16)protect_entry;
  client.entry_cs     = g_rcode * 8;

  abs_client = ptr2linear(&client);
}

word32 reserved;		/* Pages */

void paging_setup(void)
{
  word32 far *pt;
  int i;
  extern word8 features;

  reserved = 0;
  
  while(firstarea) {
    free(firstarea);
    firstarea = firstarea->next;
  }

  pd = (word32 far *)((long)valloc_640() << 24);
  vcpi_pt = pt = (word32 far *)((long)valloc_640() << 24);
  for (i=0; i<1024; i++) {
    pd[i] = 0;
    pd_seg[i] = 0;
  }
  
  if (vcpi_installed) {
    link_vcpi(pd,pt);           /*  Get VCPI Page Table  */
/*    for (i=0; i<1024; i++)
      if ((word16)pt[i] & PT_P)
        (word16)pt[i] |= PT_I; */
  } else {
    for (i=0; i < DOS_PAGE; i++)
      pt[i] = ((unsigned long)i<<12) | PT_P | PT_U | PT_W /*| PT_I*/;
    for (; i<1024; i++)
      pt[i] = 0;
  }

  pd[0] = far2pte(pt, PT_P | PT_U | PT_W | PT_I);  /* map 1:1 1st Mb */
  pd_seg[0] = (word32)pt >> 24;
  
  gdt_phys.limit = gdt[g_gdt].lim0;
  gdt_phys.base = ptr2linear(&gdt);
  idt_phys.limit = gdt[g_idt].lim0;
  idt_phys.base = ptr2linear(&idt);
  
  c_tss.tss_cr3 = 
  o_tss.tss_cr3 = 
  i_tss.tss_cr3 = 
  f_tss.tss_cr3 = 
  a_tss.tss_cr3 = far2pte(pd, 0);
  USE_4M = (features & 0x10);
}

int cant_ask_for(int32 amount)		/* amount is in bytes */
{
  word32 max;
  
  amount >>= 12;
  if (amount <= 0) {
    reserved += amount;
    return 0;
  }
  amount += reserved;
  max = valloc_max_size();

  if (amount > max)
    max += dalloc_max_size();		/* Slow, hits disk, avoid if can */

  if (amount <= max) {
    reserved = amount;
    return 0;
  }
  return 1;
}

int page_is_valid(word32 vaddr)
{
  AREAS *area = firstarea;
  while (area) {
    if ((vaddr <= area->last_addr) && (vaddr >= area->first_addr)) {
      word32 p4m = vaddr & 0xffc00000L;
      if( USE_4M  &&  p4m  >= area->first_addr &&
         (0x3fffffL + p4m) <= area->last_addr )
        return 2;
      return 1;
    }
    area = area->next;
  }
  return 0;
}

extern va_pn valloc4m(void);
extern void vfree4m(va_pn pn);

static word32 far *getpte(word32 vaddr)
{
  word32 far *pt;
  int pdi, pti, pn;
  static word32 boguspte;

  boguspte = PT_P | PT_U | PT_W | PT_PS;
  pdi = (word16)(vaddr >> 22);
  if (!((word16)pd[pdi] & PT_P)) {  /* put in an empty page table if required */
    if(page_is_valid(vaddr) == 2) {	/* 4M page */
      va_pn vpn;
      vpn = valloc4m();
      if(vpn) {
#ifdef VERBOSE
        errmsg(" new_4m_pd"); 
#endif
        pd[pdi] = ((word32)vpn << 12) | PT_P | PT_U | PT_W | PT_PS; /*PT_S PT_I*/
        return &boguspte;
      }
    }
    pn = valloc_640();
    pt = (word32 far *)((word32)pn << 24);
    if ((word16)pd[pdi] & PT_I) {
      da_pn dblock;
#ifdef VERBOSE
      errmsg(" swap_pd"); 
#endif
      dblock = (da_pn)(pd[pdi] >> 12);
      dread(paging_buffer, dblock);
      dfree(dblock);
      movedata(_DS, FP_OFF(paging_buffer), FP_SEG(pt), FP_OFF(pt), 4096);
      pd[pdi] = pn2pte(pn, PT_P | PT_U | PT_W | PT_I | PT_S);
      pd_seg[pdi] = pn;
    } else {
#ifdef VERBOSE
      errmsg( " new_pd"); 
#endif
      pd[pdi] = pn2pte(pn, PT_P | PT_U | PT_W | PT_I | PT_S);
      pd_seg[pdi] = pn;
      for (pti=0; pti<1024; pti++)
        pt[pti] = PT_U | PT_W | PT_S;
    }
  } else if((word16)pd[pdi] & PT_PS)		/* Existing 4M page */
    return &boguspte;
  else
    pt = (word32 far *)((word32)(pd_seg[pdi]) << 24);
  pti = (word16)(vaddr >> 12) & 0x3ff;
  return &pt[pti];
}

int page_in(void)
{
  word32 far *pte;
  word32 vaddr;
  va_pn pn;
  da_pn dblock;

#ifdef VERBOSE
  errmsg( "Paging (err: 0x%x) in vaddr %lx -", (word16)tss_ptr->tss_error&15, tss_ptr->tss_cr2); 
#endif
  vaddr = tss_ptr->tss_cr2;
  if (!page_is_valid(vaddr)) {
#ifdef VERBOSE
    errmsg( "invalid\n"); 
#endif
    return 1;
  }

  if((word8)tss_ptr->tss_error & 1) {
#ifdef VERBOSE
    errmsg( "protection\n"); 
#endif
    return 1;		/* Protection violation, we don't handle */
  }

  (word16)vaddr &= 0xF000;  /* points to beginning of page */

  pte = getpte(vaddr);

  if (!((word16)*pte & PT_P)) {
    int accdirty;
    TSS *old_util_tss;

    if(!((word16)*pte & PT_S)) {
#ifdef VERBOSE
      errmsg( "non-committed\n"); 
#endif
      return 1;					/* Non-committed page */
    }

    old_util_tss = utils_tss;
    utils_tss = &f_tss;

    dblock = (da_pn)(*pte >> 12);
    pn = valloc();
    if(pn == MAX_VPAGE) {
#ifdef VERBOSE
     errmsg( "valloc failed\n"); 
#endif
      utils_tss = old_util_tss;
      return 1;
    }
    accdirty = (word16)*pte & (PT_A | PT_D);	/* Save old access/dirty bits */
    *pte &= 0xfffL & ~(word32)(PT_A | PT_D);	/* Clear dblock and bits */
    *pte |= ((word32)pn << 12) | PT_P;	/* Set present and virt add */

    if ((word16)*pte & PT_I) {
#ifdef VERBOSE
      errmsg( " swap"); 
#endif
      dread(paging_buffer, dblock);
      dfree(dblock);
      memput(g_core*8, vaddr, paging_buffer, 4096);
      (word16)*pte &= ~(PT_A | PT_D);  /* clean dirty & accessed bits (set by memput) */
      (word16)*pte |= accdirty;		/* restore to previous */
    } else {
#ifdef VERBOSE
      errmsg( " new"); 
#endif
      (word16)*pte |= (PT_I | PT_C);		/* Once accessed save contents */
    }
    utils_tss = old_util_tss;
  }
#ifdef VERBOSE
  errmsg( "\n"); 
#endif
  return 0;
}

unsigned page_out_640(void) /* return >= 0 page which is paged out, 0xffff if not */
{
  static last_pti = 0;
  int pti;
  da_pn dblock;
  for (pti = last_pti+1; pti != last_pti; pti = (pti+1)%1024)
    if (((word16)pd[pti] & (PT_P | PT_S)) == (PT_P | PT_S)) {
      movedata(pd_seg[pti]<<8, 0, _DS, FP_OFF(paging_buffer), 4096);
      dblock = dalloc();
      dwrite(paging_buffer, dblock);
#ifdef VERBOSE
      errmsg( "out_640 0x%x\n", pti); 
#endif
      pd[pti] &= 0xfff & ~(word32)(PT_P); /* no longer present */
      pd[pti] |= (word32)dblock << 12;
      last_pti = pti;
      return pd_seg[pti];
    }
  return 0xffff;
}

va_pn page_out(void) /* return >= 0 page which is paged out, 0xffff if not */
{
  static last_po_pdi = 1;	/* Skip low 4Mb */
  static last_po_pti = 0;
  int start_pdi, start_pti;
  word32 far *pt, v;
  va_pn rv;
  da_pn dblock;
  start_pdi = last_po_pdi;
  if( (word8)pd[start_pdi] & PT_P)
    start_pti = last_po_pti;
  else
    start_pti = 0;
  do {
    if ((word16)pd[last_po_pdi] & (PT_P|PT_PS) == PT_P) {       /* PS = 4M */
      pt = (word32 far *)((word32)(pd_seg[last_po_pdi]) << 24);
      if (((word16)pt[last_po_pti] & (PT_P | PT_S)) == (PT_P | PT_S)) {
        rv = (va_pn)(pt[last_po_pti] >> 12);
        v = ((word32)last_po_pdi << 22) | ((word32)last_po_pti << 12);
        if ((word16)pt[last_po_pti] & (PT_C | PT_D)) {
          int accessed = (word16)pt[last_po_pti] & PT_A; /* Save accessed bit */
          (word16)pt[last_po_pti] |= PT_C;
          memget(g_core*8, v, paging_buffer, 4096);
#ifdef VERBOSE
          errmsg( "dout %lx", ((word32)last_po_pdi<<22) | ((word32)last_po_pti<<12)); 
#endif
          dblock = dalloc();
          dwrite(paging_buffer, dblock);
          pt[last_po_pti] &= 0xfff & ~(PT_P | PT_A); /* no longer present */
          pt[last_po_pti] |= (word32)dblock << 12;
          (word16)pt[last_po_pti] |= accessed;
        } else {
          pt[last_po_pti] = PT_U | PT_W | PT_S;
#ifdef VERBOSE
          errmsg( "dflush %lx", ((word32)last_po_pdi<<22) | ((word32)last_po_pti<<12)); 
#endif
        }
        return rv;
      }
    } else			/* imagine we just checked the last entry */
      last_po_pti = 1023;	/* Stupid.  If table not there, page them */

bad_choice:
    if (++last_po_pti == 1024) {
      last_po_pti = 0;
      if (++last_po_pdi == 1024)
        last_po_pdi = 1;	/* Skip low 4Mb */
    }
  } while ((start_pdi != last_po_pdi) || (start_pti != last_po_pti));
  return MAX_VPAGE;
}

/* We map physical addresses 1:1 here.  We also set up pd & pt so that they
   never generate a page fault (are not tested above) and are not cached */
void physical_map(word32 physical, word32 size, word32 vaddr)
{
  word32 address2;
  word16 pdi;
  word32 far *pte;
  extern word8 features;

#ifdef VERBOSE
  errmsg( "Map: 0x%lx for 0x%lx bytes to 0x%lx\n",physical,size,vaddr); 
#endif
  address2 = vaddr + size - 1;
  (word16)vaddr &= 0xf000;		/* page align */
  (word16)physical &= 0xf000;		/* page align */
  /* Minor bug - if using 640K page before overwritting, loose forever here */
  while(vaddr <= address2) {
    if(vaddr < 0x100000UL)
      return;				/* Wrap protection */
    pdi = (word16)(vaddr >> 22);
    if((vaddr & 0x3fffffL) || (physical & 0x3fffffL) || (vaddr+0x3fffffL > address2) ||
       ((word16)pd[pdi] & (PT_P|PT_PS) == PT_P) || !USE_4M ) {
      free_memory(vaddr,vaddr);         /* Free up if used */
      pte = getpte(vaddr);
#ifndef PFM686
      (word16)pd[pdi] &= ~PT_S;         /* Make sure directory no swap */
      *pte = PT_P | PT_U | PT_W | PT_CD | physical;
#else
      /* koji - page directories and do NOT disable cache - direct map memory */
      *pte = PT_P | PT_U | PT_W | physical;
#endif
      vaddr += 4096L;
      physical += 4096L;
    } else {                            /* 4M aligned, not present */
#ifdef VERBOSE
      errmsg( "4M Map: 0x%lx to 0x%lx\n",physical,vaddr); 
#endif
      pd[pdi] = physical | PT_P | PT_U | PT_W | PT_CD | PT_PS;
      vaddr += 0x400000L;
      physical += 0x400000L;
    }
  }
}

int lock_memory(word32 vaddr, word32 size, word8 unlock)
{
  word16 pdi;
  word32 far *pte;
  word32 vaddr2;
#ifdef VERBOSE
  errmsg( "Lock(0x%x): 0x%lx for 0x%lx bytes\n",unlock,vaddr,size); 
#endif
  size += vaddr;
  (word16)vaddr &= 0xf000;		/* page align */
  vaddr2 = vaddr;
  while(vaddr < size && page_is_valid(vaddr)) {
    pte = getpte(vaddr);
    if (!unlock) {
      pdi = (vaddr >> 22);
      (word16)pd[pdi] &= ~PT_S;		/* Make sure no swap */
    }
    if ((word16)*pte & PT_P) {
      if (unlock)
        (word16)*pte |= PT_S;		/* enable swap bit */
      else
        (word16)*pte &= ~PT_S;		/* clear swap bit */
    } else {
      /* Wasn't locked if no PT present, skip.  Unlocked due to no count?*/
      if (!unlock) {
        /* paged out or never accessed */
        tss_ptr->tss_cr2 = vaddr;
        (word8)tss_ptr->tss_error = 0;
        if(page_in()) {
          lock_memory(vaddr2,vaddr-vaddr2,1);	/* Undo the locking */
          return 1;				/* show error */
        }
        tss_ptr->tss_cr2 = 0L;
        (word16)*pte &= ~PT_S;			/* clear swap bit */
      }
    }
    vaddr += 4096L;
  }
  return 0;
}

/* vfirst always page aligned, vaddr maybe not (but free last page anyway) */
/* we free in reverse in the hope of freeing up pages in valloc */
void free_memory(word32 vfirst, word32 vaddr)
{
  word32 far *pte;
  word16 pdi;

  (word16)vaddr &= 0xf000;		/* page align */
  while(vfirst <= vaddr) {
    pdi = (word16)(vaddr >> 22);
    if((vaddr & 0x3fffffL == 0x3ff000L) && (vaddr & 0xffc00000L >= vfirst) &&
       ((word16)pd[pdi] & (PT_P|PT_PS)) == (PT_P|PT_PS) && USE_4M ) {
      vfree4m((va_pn)(pd[pdi]>>12));
      pd[pdi] = 0;
      vaddr -= 0x400000L;
    } else {
      pte = getpte(vaddr);
      if ((word16)*pte & PT_P) {
        if (!((word16)*pte & PT_I) || vfree((va_pn)(*pte>>12)))
	  *pte = PT_U | PT_W | PT_S;		/* Back in pool */
        else 
          (word16)*pte &= ~(PT_C | PT_D);	/* So it will be flushed */
      } else if (!((word16)*pte & PT_S)) {	/* Uncommitted */
        *pte = PT_U | PT_W | PT_S;		/* Back in pool */
      } else if ((word16)*pte & PT_I) {
        dfree((da_pn)(*pte >> 12));		/* Free swap space usage */
        *pte = PT_U | PT_W | PT_S;
      }
      vaddr -= 4096L;
    }
  }
}

int free_memory_area(word32 vaddr)
{
  AREAS *area = firstarea;
  AREAS **lasta = &firstarea;
  while (area) {
    if (vaddr == area->first_addr) {
      free_memory(area->first_addr,area->last_addr);
      cant_ask_for(area->first_addr-area->last_addr-1); /* Adjust reserved */
      *lasta = area->next;
      free(area);
      return 1;
    }
    lasta = &area->next;
    area = area->next;
  }
  return 0;
}

/* Get: Bits 0-2 page type (0=uncommit, 1=normal, 2=mapped) (PT_P, PT_I, PT_S)
             3 page is writable (PT_W)
             4 accessed/dirty available (1)
             5-6 accessed/dirty (PT_A | PT_D) (same locations)
   Set: Bits 0-2 page type (0=uncommit, 1=normal, 3=no change)
   Bugs: slow since mode swap for every page!
*/
int page_attributes(word8 set, word32 vaddr, word16 count)
{
  int ic, pte, uval;
  word32 far *pt;
#ifdef VERBOSE
  errmsg( "Attrib(0x%x): 0x%lx for 0x%x pages\n",set,vaddr,count); 
#endif
  (word16)vaddr &= 0xf000;		/* page align */
  for(ic=0;ic<count;ic++) {
    if(set)
      memget(tss_ptr->tss_es, tss_ptr->tss_edx+(ic*2), &uval, 2);
    pt = getpte(vaddr);
    pte = (word16)*pt;
    if(!set) {
      uval = pte & (PT_P | PT_I | PT_S);
      uval *= 2;	/* Trickery.  Map 0 to 0, 1 to 2, others to > 2 */
      if(uval > 2)
        uval = 1;
      if(pte & PT_W)
        uval |= 8;
      uval |= 0x10;
      uval |= pte & (PT_A | PT_D);
    } else {
      int flags = uval & 7;
      if(flags == 0) {			/* uncommitted */
        free_memory(vaddr,vaddr);
        (word16)*pt &= ~PT_S;
      } else if(flags == 1) {		/* normal */
        flags = pte & (PT_P | PT_I | PT_S);
        if(!flags || flags == PT_P)	/* was uncommitted or mapped */
          *pt = PT_U | PT_W | PT_S;
      }
      if(uval & 8)
        (word16)*pt |= PT_W;
      else
        (word16)*pt &= ~PT_W;
      if(uval & 0x10) {
        (word16)*pt &= ~(PT_A | PT_D);
        (word16)*pt |= (uval & (PT_A | PT_D));
      }
    }
    vaddr += 4096L;
    if(!set)
      memput(tss_ptr->tss_es, tss_ptr->tss_edx+(ic*2), &uval, 2);
  }
  return 0;
}

void move_pt(word32 vorig, word32 vend, word32 vnew)
{
  word32 far *pte;
  word32 save;

  while(vorig <= vend) {
    pte = getpte(vorig);
    save = *pte;
    *pte = PT_U | PT_W | PT_S;
    pte = getpte(vnew);
    *pte = save;
    if(!((word16)save & PT_S)) {
      word16 pdi = (vnew >> 22);
      (word16)pd[pdi] &= ~PT_S;		/* Make sure no swap */
    }
    vorig += 4096L;
    vnew  += 4096L;
  }
}
