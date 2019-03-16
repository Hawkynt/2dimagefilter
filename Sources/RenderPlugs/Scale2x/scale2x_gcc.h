/*
 * This file is part of the Scale2x project.
 *
 * Copyright (C) 2001, 2002, 2003, 2004 Andrea Mazzoleni
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 */

/*
 * This file contains an MMX implentation of the Scale2x effect, for GCC.
 * The file has been stripped from the original, to just the code required.
 *
 * You can find an high level description of the effect at :
 *
 * http://scale2x.sourceforge.net/
 *
 * Alternatively at the previous license terms, you are allowed to use this
 * code in your program with these conditions:
 * - the program is not used in commercial activities.
 * - the whole source code of the program is released with the binary.
 * - derivative works of the program are allowed.
 */

typedef unsigned char scale2x_uint8;
typedef unsigned short scale2x_uint16;
typedef unsigned scale2x_uint32;

__inline void scale2x_16_def_single(scale2x_uint16* dst, const scale2x_uint16* src0, const scale2x_uint16* src1, const scale2x_uint16* src2, unsigned count)
{
	/* first pixel */
	dst[0] = src1[0];
	if (src1[1] == src0[0] && src2[0] != src0[0])
		dst[1] = src0[0];
	else
		dst[1] = src1[0];
	++src0;
	++src1;
	++src2;
	dst += 2;

	/* central pixels */
	count -= 2;
	while (count) {
		if (src0[0] != src2[0] && src1[-1] != src1[1]) {
			dst[0] = src1[-1] == src0[0] ? src0[0] : src1[0];
			dst[1] = src1[1] == src0[0] ? src0[0] : src1[0];
		} else {
			dst[0] = src1[0];
			dst[1] = src1[0];
		}

		++src0;
		++src1;
		++src2;
		dst += 2;
		--count;
	}

	/* last pixel */
	if (src1[-1] == src0[0] && src2[0] != src0[0])
		dst[0] = src0[0];
	else
		dst[0] = src1[0];
	dst[1] = src1[0];
}

/**
 * Scale by a factor of 2 a row of pixels of 16 bits.
 * This function operates like scale2x_8_def() but for 16 bits pixels.
 * \param src0 Pointer at the first pixel of the previous row.
 * \param src1 Pointer at the first pixel of the current row.
 * \param src2 Pointer at the first pixel of the next row.
 * \param count Length in pixels of the src0, src1 and src2 rows.
 * It must be at least 2.
 * \param dst0 First destination row, double length in pixels.
 * \param dst1 Second destination row, double length in pixels.
 */
__inline void scale2x_16_def(scale2x_uint16* dst0, scale2x_uint16* dst1, const scale2x_uint16* src0, const scale2x_uint16* src1, const scale2x_uint16* src2, unsigned count)
{
	scale2x_16_def_single(dst0, src0, src1, src2, count);
	scale2x_16_def_single(dst1, src2, src1, src0, count);
}

/***************************************************************************/
/* Scale2x MMX implementation */

__inline void scale2x_16_mmx_single(scale2x_uint16* dst, const scale2x_uint16* src0, const scale2x_uint16* src1, const scale2x_uint16* src2, unsigned count)
{
	/* always do the first and last run */
	count -= 2*4;

	__asm__ __volatile__(
/* first run */
		/* set the current, current_pre, current_next registers */
		"movq 0(%1), %%mm0\n"
		"movq 0(%1), %%mm7\n"
		"movq 8(%1), %%mm1\n"
		"psllq $48, %%mm0\n"
		"psllq $48, %%mm1\n"
		"psrlq $48, %%mm0\n"
		"movq %%mm7, %%mm2\n"
		"movq %%mm7, %%mm3\n"
		"psllq $16, %%mm2\n"
		"psrlq $16, %%mm3\n"
		"por %%mm2, %%mm0\n"
		"por %%mm3, %%mm1\n"

		/* current_upper */
		"movq (%0), %%mm6\n"

		/* compute the upper-left pixel for dst on %%mm2 */
		/* compute the upper-right pixel for dst on %%mm4 */
		"movq %%mm0, %%mm2\n"
		"movq %%mm1, %%mm4\n"
		"movq %%mm0, %%mm3\n"
		"movq %%mm1, %%mm5\n"
		"pcmpeqw %%mm6, %%mm2\n"
		"pcmpeqw %%mm6, %%mm4\n"
		"pcmpeqw (%2), %%mm3\n"
		"pcmpeqw (%2), %%mm5\n"
		"pandn %%mm2, %%mm3\n"
		"pandn %%mm4, %%mm5\n"
		"movq %%mm0, %%mm2\n"
		"movq %%mm1, %%mm4\n"
		"pcmpeqw %%mm1, %%mm2\n"
		"pcmpeqw %%mm0, %%mm4\n"
		"pandn %%mm3, %%mm2\n"
		"pandn %%mm5, %%mm4\n"
		"movq %%mm2, %%mm3\n"
		"movq %%mm4, %%mm5\n"
		"pand %%mm6, %%mm2\n"
		"pand %%mm6, %%mm4\n"
		"pandn %%mm7, %%mm3\n"
		"pandn %%mm7, %%mm5\n"
		"por %%mm3, %%mm2\n"
		"por %%mm5, %%mm4\n"

		/* set *dst */
		"movq %%mm2, %%mm3\n"
		"punpcklwd %%mm4, %%mm2\n"
		"punpckhwd %%mm4, %%mm3\n"
		"movq %%mm2, (%3)\n"
		"movq %%mm3, 8(%3)\n"

		/* next */
		"addl $8, %0\n"
		"addl $8, %1\n"
		"addl $8, %2\n"
		"addl $16, %3\n"

/* central runs */
		"shrl $2, %4\n"
		"jz 1f\n"

		"0:\n"

		/* set the current, current_pre, current_next registers */
		"movq -8(%1), %%mm0\n"
		"movq (%1), %%mm7\n"
		"movq 8(%1), %%mm1\n"
		"psrlq $48, %%mm0\n"
		"psllq $48, %%mm1\n"
		"movq %%mm7, %%mm2\n"
		"movq %%mm7, %%mm3\n"
		"psllq $16, %%mm2\n"
		"psrlq $16, %%mm3\n"
		"por %%mm2, %%mm0\n"
		"por %%mm3, %%mm1\n"

		/* current_upper */
		"movq (%0), %%mm6\n"

		/* compute the upper-left pixel for dst on %%mm2 */
		/* compute the upper-right pixel for dst on %%mm4 */
		"movq %%mm0, %%mm2\n"
		"movq %%mm1, %%mm4\n"
		"movq %%mm0, %%mm3\n"
		"movq %%mm1, %%mm5\n"
		"pcmpeqw %%mm6, %%mm2\n"
		"pcmpeqw %%mm6, %%mm4\n"
		"pcmpeqw (%2), %%mm3\n"
		"pcmpeqw (%2), %%mm5\n"
		"pandn %%mm2, %%mm3\n"
		"pandn %%mm4, %%mm5\n"
		"movq %%mm0, %%mm2\n"
		"movq %%mm1, %%mm4\n"
		"pcmpeqw %%mm1, %%mm2\n"
		"pcmpeqw %%mm0, %%mm4\n"
		"pandn %%mm3, %%mm2\n"
		"pandn %%mm5, %%mm4\n"
		"movq %%mm2, %%mm3\n"
		"movq %%mm4, %%mm5\n"
		"pand %%mm6, %%mm2\n"
		"pand %%mm6, %%mm4\n"
		"pandn %%mm7, %%mm3\n"
		"pandn %%mm7, %%mm5\n"
		"por %%mm3, %%mm2\n"
		"por %%mm5, %%mm4\n"

		/* set *dst */
		"movq %%mm2, %%mm3\n"
		"punpcklwd %%mm4, %%mm2\n"
		"punpckhwd %%mm4, %%mm3\n"
		"movq %%mm2, (%3)\n"
		"movq %%mm3, 8(%3)\n"

		/* next */
		"addl $8, %0\n"
		"addl $8, %1\n"
		"addl $8, %2\n"
		"addl $16, %3\n"

		"decl %4\n"
		"jnz 0b\n"
		"1:\n"

/* final run */
		/* set the current, current_pre, current_next registers */
		"movq (%1), %%mm1\n"
		"movq (%1), %%mm7\n"
		"movq -8(%1), %%mm0\n"
		"psrlq $48, %%mm1\n"
		"psrlq $48, %%mm0\n"
		"psllq $48, %%mm1\n"
		"movq %%mm7, %%mm2\n"
		"movq %%mm7, %%mm3\n"
		"psllq $16, %%mm2\n"
		"psrlq $16, %%mm3\n"
		"por %%mm2, %%mm0\n"
		"por %%mm3, %%mm1\n"

		/* current_upper */
		"movq (%0), %%mm6\n"

		/* compute the upper-left pixel for dst on %%mm2 */
		/* compute the upper-right pixel for dst on %%mm4 */
		"movq %%mm0, %%mm2\n"
		"movq %%mm1, %%mm4\n"
		"movq %%mm0, %%mm3\n"
		"movq %%mm1, %%mm5\n"
		"pcmpeqw %%mm6, %%mm2\n"
		"pcmpeqw %%mm6, %%mm4\n"
		"pcmpeqw (%2), %%mm3\n"
		"pcmpeqw (%2), %%mm5\n"
		"pandn %%mm2, %%mm3\n"
		"pandn %%mm4, %%mm5\n"
		"movq %%mm0, %%mm2\n"
		"movq %%mm1, %%mm4\n"
		"pcmpeqw %%mm1, %%mm2\n"
		"pcmpeqw %%mm0, %%mm4\n"
		"pandn %%mm3, %%mm2\n"
		"pandn %%mm5, %%mm4\n"
		"movq %%mm2, %%mm3\n"
		"movq %%mm4, %%mm5\n"
		"pand %%mm6, %%mm2\n"
		"pand %%mm6, %%mm4\n"
		"pandn %%mm7, %%mm3\n"
		"pandn %%mm7, %%mm5\n"
		"por %%mm3, %%mm2\n"
		"por %%mm5, %%mm4\n"

		/* set *dst */
		"movq %%mm2, %%mm3\n"
		"punpcklwd %%mm4, %%mm2\n"
		"punpckhwd %%mm4, %%mm3\n"
		"movq %%mm2, (%3)\n"
		"movq %%mm3, 8(%3)\n"

		: "+r" (src0), "+r" (src1), "+r" (src2), "+r" (dst), "+r" (count)
		:
		: "cc"
	);
}

/**
 * Scale by a factor of 2 a row of pixels of 16 bits.
 * This function operates like scale2x_8_mmx() but for 16 bits pixels.
 * \param src0 Pointer at the first pixel of the previous row.
 * \param src1 Pointer at the first pixel of the current row.
 * \param src2 Pointer at the first pixel of the next row.
 * \param count Length in pixels of the src0, src1 and src2 rows. It must
 * be at least 8 and a multiple of 4.
 * \param dst0 First destination row, double length in pixels.
 * \param dst1 Second destination row, double length in pixels.
 */

static void scale2x_16_mmx(scale2x_uint16* dst0, scale2x_uint16* dst1, const scale2x_uint16* src0, const scale2x_uint16* src1, const scale2x_uint16* src2, unsigned count)
{
	if((count&3) || (count<8))
	{
		scale2x_16_def(dst0, dst1, src0, src1, src2, count);
	}
	else
	{
		scale2x_16_mmx_single(dst0, src0, src1, src2, count);
		scale2x_16_mmx_single(dst1, src2, src1, src0, count);
	}
}
