//hq2x, hq3x, and hq4x filters
//----------------------------------------------------------
//Copyright (C) 2003 MaxSt ( maxst@hiend3d.com )

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this program; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#include <stdlib.h>
#include "common.h"

typedef char bool; // from C++ to C

static int   LUT16to32[65536];
static int   RGBtoYUV[65536];
static int   YUV1, YUV2;
static const  int   Ymask = 0x00FF0000;
static const  int   Umask = 0x0000FF00;
static const  int   Vmask = 0x000000FF;
static const  int   trY   = 0x00300000;
static const  int   trU   = 0x00000700;
static const  int   trV   = 0x00000006;
static bool inited = FALSE;

static inline void Interp1(unsigned char * pc, int c1, int c2)
{
  *((int*)pc) = (c1*3+c2) >> 2;
}

static inline void Interp2(unsigned char * pc, int c1, int c2, int c3)
{
  *((int*)pc) = (c1*2+c2+c3) >> 2;
}

static inline void Interp3(unsigned char * pc, int c1, int c2)
{
  //*((int*)pc) = (c1*7+c2)/8;

  *((int*)pc) = ((((c1 & 0x00FF00)*7 + (c2 & 0x00FF00) ) & 0x0007F800) +
                 (((c1 & 0xFF00FF)*7 + (c2 & 0xFF00FF) ) & 0x07F807F8)) >> 3;
}

static inline void Interp4(unsigned char * pc, int c1, int c2, int c3)
{
  //*((int*)pc) = (c1*2+(c2+c3)*7)/16;

  *((int*)pc) = ((((c1 & 0x00FF00)*2 + ((c2 & 0x00FF00) + (c3 & 0x00FF00))*7 ) & 0x000FF000) +
                 (((c1 & 0xFF00FF)*2 + ((c2 & 0xFF00FF) + (c3 & 0xFF00FF))*7 ) & 0x0FF00FF0)) >> 4;
}

static inline void Interp5(unsigned char * pc, int c1, int c2)
{
  *((int*)pc) = (c1+c2) >> 1;
}

static inline void Interp6(unsigned char * pc, int c1, int c2, int c3)
{
  //*((int*)pc) = (c1*5+c2*2+c3)/8;

  *((int*)pc) = ((((c1 & 0x00FF00)*5 + (c2 & 0x00FF00)*2 + (c3 & 0x00FF00) ) & 0x0007F800) +
                 (((c1 & 0xFF00FF)*5 + (c2 & 0xFF00FF)*2 + (c3 & 0xFF00FF) ) & 0x07F807F8)) >> 3;
}

static inline void Interp7(unsigned char * pc, int c1, int c2, int c3)
{
  //*((int*)pc) = (c1*6+c2+c3)/8;

  *((int*)pc) = ((((c1 & 0x00FF00)*6 + (c2 & 0x00FF00) + (c3 & 0x00FF00) ) & 0x0007F800) +
                 (((c1 & 0xFF00FF)*6 + (c2 & 0xFF00FF) + (c3 & 0xFF00FF) ) & 0x07F807F8)) >> 3;
}

static inline void Interp8(unsigned char * pc, int c1, int c2)
{
  //*((int*)pc) = (c1*5+c2*3)/8;

  *((int*)pc) = ((((c1 & 0x00FF00)*5 + (c2 & 0x00FF00)*3 ) & 0x0007F800) +
                 (((c1 & 0xFF00FF)*5 + (c2 & 0xFF00FF)*3 ) & 0x07F807F8)) >> 3;
}

static inline void Interp9(unsigned char * pc, int c1, int c2, int c3)
{
  //*((int*)pc) = (c1*2+(c2+c3)*3)/8;

  *((int*)pc) = ((((c1 & 0x00FF00)*2 + ((c2 & 0x00FF00) + (c3 & 0x00FF00))*3 ) & 0x0007F800) +
                 (((c1 & 0xFF00FF)*2 + ((c2 & 0xFF00FF) + (c3 & 0xFF00FF))*3 ) & 0x07F807F8)) >> 3;
}

static inline void Interp10(unsigned char * pc, int c1, int c2, int c3)
{
  //*((int*)pc) = (c1*14+c2+c3)/16;

  *((int*)pc) = ((((c1 & 0x00FF00)*14 + (c2 & 0x00FF00) + (c3 & 0x00FF00) ) & 0x000FF000) +
                 (((c1 & 0xFF00FF)*14 + (c2 & 0xFF00FF) + (c3 & 0xFF00FF) ) & 0x0FF00FF0)) >> 4;
}

#define HQ2X_PIXEL00_0     *((int*)(pOut)) = c[5];
#define HQ2X_PIXEL00_10    Interp1(pOut, c[5], c[1]);
#define HQ2X_PIXEL00_11    Interp1(pOut, c[5], c[4]);
#define HQ2X_PIXEL00_12    Interp1(pOut, c[5], c[2]);
#define HQ2X_PIXEL00_20    Interp2(pOut, c[5], c[4], c[2]);
#define HQ2X_PIXEL00_21    Interp2(pOut, c[5], c[1], c[2]);
#define HQ2X_PIXEL00_22    Interp2(pOut, c[5], c[1], c[4]);
#define HQ2X_PIXEL00_60    Interp6(pOut, c[5], c[2], c[4]);
#define HQ2X_PIXEL00_61    Interp6(pOut, c[5], c[4], c[2]);
#define HQ2X_PIXEL00_70    Interp7(pOut, c[5], c[4], c[2]);
#define HQ2X_PIXEL00_90    Interp9(pOut, c[5], c[4], c[2]);
#define HQ2X_PIXEL00_100   Interp10(pOut, c[5], c[4], c[2]);
#define HQ2X_PIXEL01_0     *((int*)(pOut+4)) = c[5];
#define HQ2X_PIXEL01_10    Interp1(pOut+4, c[5], c[3]);
#define HQ2X_PIXEL01_11    Interp1(pOut+4, c[5], c[2]);
#define HQ2X_PIXEL01_12    Interp1(pOut+4, c[5], c[6]);
#define HQ2X_PIXEL01_20    Interp2(pOut+4, c[5], c[2], c[6]);
#define HQ2X_PIXEL01_21    Interp2(pOut+4, c[5], c[3], c[6]);
#define HQ2X_PIXEL01_22    Interp2(pOut+4, c[5], c[3], c[2]);
#define HQ2X_PIXEL01_60    Interp6(pOut+4, c[5], c[6], c[2]);
#define HQ2X_PIXEL01_61    Interp6(pOut+4, c[5], c[2], c[6]);
#define HQ2X_PIXEL01_70    Interp7(pOut+4, c[5], c[2], c[6]);
#define HQ2X_PIXEL01_90    Interp9(pOut+4, c[5], c[2], c[6]);
#define HQ2X_PIXEL01_100   Interp10(pOut+4, c[5], c[2], c[6]);
#define HQ2X_PIXEL10_0     *((int*)(pOut+BpL)) = c[5];
#define HQ2X_PIXEL10_10    Interp1(pOut+BpL, c[5], c[7]);
#define HQ2X_PIXEL10_11    Interp1(pOut+BpL, c[5], c[8]);
#define HQ2X_PIXEL10_12    Interp1(pOut+BpL, c[5], c[4]);
#define HQ2X_PIXEL10_20    Interp2(pOut+BpL, c[5], c[8], c[4]);
#define HQ2X_PIXEL10_21    Interp2(pOut+BpL, c[5], c[7], c[4]);
#define HQ2X_PIXEL10_22    Interp2(pOut+BpL, c[5], c[7], c[8]);
#define HQ2X_PIXEL10_60    Interp6(pOut+BpL, c[5], c[4], c[8]);
#define HQ2X_PIXEL10_61    Interp6(pOut+BpL, c[5], c[8], c[4]);
#define HQ2X_PIXEL10_70    Interp7(pOut+BpL, c[5], c[8], c[4]);
#define HQ2X_PIXEL10_90    Interp9(pOut+BpL, c[5], c[8], c[4]);
#define HQ2X_PIXEL10_100   Interp10(pOut+BpL, c[5], c[8], c[4]);
#define HQ2X_PIXEL11_0     *((int*)(pOut+BpL+4)) = c[5];
#define HQ2X_PIXEL11_10    Interp1(pOut+BpL+4, c[5], c[9]);
#define HQ2X_PIXEL11_11    Interp1(pOut+BpL+4, c[5], c[6]);
#define HQ2X_PIXEL11_12    Interp1(pOut+BpL+4, c[5], c[8]);
#define HQ2X_PIXEL11_20    Interp2(pOut+BpL+4, c[5], c[6], c[8]);
#define HQ2X_PIXEL11_21    Interp2(pOut+BpL+4, c[5], c[9], c[8]);
#define HQ2X_PIXEL11_22    Interp2(pOut+BpL+4, c[5], c[9], c[6]);
#define HQ2X_PIXEL11_60    Interp6(pOut+BpL+4, c[5], c[8], c[6]);
#define HQ2X_PIXEL11_61    Interp6(pOut+BpL+4, c[5], c[6], c[8]);
#define HQ2X_PIXEL11_70    Interp7(pOut+BpL+4, c[5], c[6], c[8]);
#define HQ2X_PIXEL11_90    Interp9(pOut+BpL+4, c[5], c[6], c[8]);
#define HQ2X_PIXEL11_100   Interp10(pOut+BpL+4, c[5], c[6], c[8]);

#define HQ3X_PIXEL00_1M  Interp1(pOut, c[5], c[1]);
#define HQ3X_PIXEL00_1U  Interp1(pOut, c[5], c[2]);
#define HQ3X_PIXEL00_1L  Interp1(pOut, c[5], c[4]);
#define HQ3X_PIXEL00_2   Interp2(pOut, c[5], c[4], c[2]);
#define HQ3X_PIXEL00_4   Interp4(pOut, c[5], c[4], c[2]);
#define HQ3X_PIXEL00_5   Interp5(pOut, c[4], c[2]);
#define HQ3X_PIXEL00_C   *((int*)(pOut))   = c[5];

#define HQ3X_PIXEL01_1   Interp1(pOut+4, c[5], c[2]);
#define HQ3X_PIXEL01_3   Interp3(pOut+4, c[5], c[2]);
#define HQ3X_PIXEL01_6   Interp1(pOut+4, c[2], c[5]);
#define HQ3X_PIXEL01_C   *((int*)(pOut+4)) = c[5];

#define HQ3X_PIXEL02_1M  Interp1(pOut+8, c[5], c[3]);
#define HQ3X_PIXEL02_1U  Interp1(pOut+8, c[5], c[2]);
#define HQ3X_PIXEL02_1R  Interp1(pOut+8, c[5], c[6]);
#define HQ3X_PIXEL02_2   Interp2(pOut+8, c[5], c[2], c[6]);
#define HQ3X_PIXEL02_4   Interp4(pOut+8, c[5], c[2], c[6]);
#define HQ3X_PIXEL02_5   Interp5(pOut+8, c[2], c[6]);
#define HQ3X_PIXEL02_C   *((int*)(pOut+8)) = c[5];

#define HQ3X_PIXEL10_1   Interp1(pOut+BpL, c[5], c[4]);
#define HQ3X_PIXEL10_3   Interp3(pOut+BpL, c[5], c[4]);
#define HQ3X_PIXEL10_6   Interp1(pOut+BpL, c[4], c[5]);
#define HQ3X_PIXEL10_C   *((int*)(pOut+BpL)) = c[5];

#define HQ3X_PIXEL11     *((int*)(pOut+BpL+4)) = c[5];

#define HQ3X_PIXEL12_1   Interp1(pOut+BpL+8, c[5], c[6]);
#define HQ3X_PIXEL12_3   Interp3(pOut+BpL+8, c[5], c[6]);
#define HQ3X_PIXEL12_6   Interp1(pOut+BpL+8, c[6], c[5]);
#define HQ3X_PIXEL12_C   *((int*)(pOut+BpL+8)) = c[5];

#define HQ3X_PIXEL20_1M  Interp1(pOut+BpL+BpL, c[5], c[7]);
#define HQ3X_PIXEL20_1D  Interp1(pOut+BpL+BpL, c[5], c[8]);
#define HQ3X_PIXEL20_1L  Interp1(pOut+BpL+BpL, c[5], c[4]);
#define HQ3X_PIXEL20_2   Interp2(pOut+BpL+BpL, c[5], c[8], c[4]);
#define HQ3X_PIXEL20_4   Interp4(pOut+BpL+BpL, c[5], c[8], c[4]);
#define HQ3X_PIXEL20_5   Interp5(pOut+BpL+BpL, c[8], c[4]);
#define HQ3X_PIXEL20_C   *((int*)(pOut+BpL+BpL)) = c[5];

#define HQ3X_PIXEL21_1   Interp1(pOut+BpL+BpL+4, c[5], c[8]);
#define HQ3X_PIXEL21_3   Interp3(pOut+BpL+BpL+4, c[5], c[8]);
#define HQ3X_PIXEL21_6   Interp1(pOut+BpL+BpL+4, c[8], c[5]);
#define HQ3X_PIXEL21_C   *((int*)(pOut+BpL+BpL+4)) = c[5];

#define HQ3X_PIXEL22_1M  Interp1(pOut+BpL+BpL+8, c[5], c[9]);
#define HQ3X_PIXEL22_1D  Interp1(pOut+BpL+BpL+8, c[5], c[8]);
#define HQ3X_PIXEL22_1R  Interp1(pOut+BpL+BpL+8, c[5], c[6]);
#define HQ3X_PIXEL22_2   Interp2(pOut+BpL+BpL+8, c[5], c[6], c[8]);
#define HQ3X_PIXEL22_4   Interp4(pOut+BpL+BpL+8, c[5], c[6], c[8]);
#define HQ3X_PIXEL22_5   Interp5(pOut+BpL+BpL+8, c[6], c[8]);
#define HQ3X_PIXEL22_C   *((int*)(pOut+BpL+BpL+8)) = c[5];

#define HQ4X_PIXEL00_0     *((int*)(pOut)) = c[5];
#define HQ4X_PIXEL00_11    Interp1(pOut, c[5], c[4]);
#define HQ4X_PIXEL00_12    Interp1(pOut, c[5], c[2]);
#define HQ4X_PIXEL00_20    Interp2(pOut, c[5], c[2], c[4]);
#define HQ4X_PIXEL00_50    Interp5(pOut, c[2], c[4]);
#define HQ4X_PIXEL00_80    Interp8(pOut, c[5], c[1]);
#define HQ4X_PIXEL00_81    Interp8(pOut, c[5], c[4]);
#define HQ4X_PIXEL00_82    Interp8(pOut, c[5], c[2]);
#define HQ4X_PIXEL01_0     *((int*)(pOut+4)) = c[5];
#define HQ4X_PIXEL01_10    Interp1(pOut+4, c[5], c[1]);
#define HQ4X_PIXEL01_12    Interp1(pOut+4, c[5], c[2]);
#define HQ4X_PIXEL01_14    Interp1(pOut+4, c[2], c[5]);
#define HQ4X_PIXEL01_21    Interp2(pOut+4, c[2], c[5], c[4]);
#define HQ4X_PIXEL01_31    Interp3(pOut+4, c[5], c[4]);
#define HQ4X_PIXEL01_50    Interp5(pOut+4, c[2], c[5]);
#define HQ4X_PIXEL01_60    Interp6(pOut+4, c[5], c[2], c[4]);
#define HQ4X_PIXEL01_61    Interp6(pOut+4, c[5], c[2], c[1]);
#define HQ4X_PIXEL01_82    Interp8(pOut+4, c[5], c[2]);
#define HQ4X_PIXEL01_83    Interp8(pOut+4, c[2], c[4]);
#define HQ4X_PIXEL02_0     *((int*)(pOut+8)) = c[5];
#define HQ4X_PIXEL02_10    Interp1(pOut+8, c[5], c[3]);
#define HQ4X_PIXEL02_11    Interp1(pOut+8, c[5], c[2]);
#define HQ4X_PIXEL02_13    Interp1(pOut+8, c[2], c[5]);
#define HQ4X_PIXEL02_21    Interp2(pOut+8, c[2], c[5], c[6]);
#define HQ4X_PIXEL02_32    Interp3(pOut+8, c[5], c[6]);
#define HQ4X_PIXEL02_50    Interp5(pOut+8, c[2], c[5]);
#define HQ4X_PIXEL02_60    Interp6(pOut+8, c[5], c[2], c[6]);
#define HQ4X_PIXEL02_61    Interp6(pOut+8, c[5], c[2], c[3]);
#define HQ4X_PIXEL02_81    Interp8(pOut+8, c[5], c[2]);
#define HQ4X_PIXEL02_83    Interp8(pOut+8, c[2], c[6]);
#define HQ4X_PIXEL03_0     *((int*)(pOut+12)) = c[5];
#define HQ4X_PIXEL03_11    Interp1(pOut+12, c[5], c[2]);
#define HQ4X_PIXEL03_12    Interp1(pOut+12, c[5], c[6]);
#define HQ4X_PIXEL03_20    Interp2(pOut+12, c[5], c[2], c[6]);
#define HQ4X_PIXEL03_50    Interp5(pOut+12, c[2], c[6]);
#define HQ4X_PIXEL03_80    Interp8(pOut+12, c[5], c[3]);
#define HQ4X_PIXEL03_81    Interp8(pOut+12, c[5], c[2]);
#define HQ4X_PIXEL03_82    Interp8(pOut+12, c[5], c[6]);
#define HQ4X_PIXEL10_0     *((int*)(pOut+BpL)) = c[5];
#define HQ4X_PIXEL10_10    Interp1(pOut+BpL, c[5], c[1]);
#define HQ4X_PIXEL10_11    Interp1(pOut+BpL, c[5], c[4]);
#define HQ4X_PIXEL10_13    Interp1(pOut+BpL, c[4], c[5]);
#define HQ4X_PIXEL10_21    Interp2(pOut+BpL, c[4], c[5], c[2]);
#define HQ4X_PIXEL10_32    Interp3(pOut+BpL, c[5], c[2]);
#define HQ4X_PIXEL10_50    Interp5(pOut+BpL, c[4], c[5]);
#define HQ4X_PIXEL10_60    Interp6(pOut+BpL, c[5], c[4], c[2]);
#define HQ4X_PIXEL10_61    Interp6(pOut+BpL, c[5], c[4], c[1]);
#define HQ4X_PIXEL10_81    Interp8(pOut+BpL, c[5], c[4]);
#define HQ4X_PIXEL10_83    Interp8(pOut+BpL, c[4], c[2]);
#define HQ4X_PIXEL11_0     *((int*)(pOut+BpL+4)) = c[5];
#define HQ4X_PIXEL11_30    Interp3(pOut+BpL+4, c[5], c[1]);
#define HQ4X_PIXEL11_31    Interp3(pOut+BpL+4, c[5], c[4]);
#define HQ4X_PIXEL11_32    Interp3(pOut+BpL+4, c[5], c[2]);
#define HQ4X_PIXEL11_70    Interp7(pOut+BpL+4, c[5], c[4], c[2]);
#define HQ4X_PIXEL12_0     *((int*)(pOut+BpL+8)) = c[5];
#define HQ4X_PIXEL12_30    Interp3(pOut+BpL+8, c[5], c[3]);
#define HQ4X_PIXEL12_31    Interp3(pOut+BpL+8, c[5], c[2]);
#define HQ4X_PIXEL12_32    Interp3(pOut+BpL+8, c[5], c[6]);
#define HQ4X_PIXEL12_70    Interp7(pOut+BpL+8, c[5], c[6], c[2]);
#define HQ4X_PIXEL13_0     *((int*)(pOut+BpL+12)) = c[5];
#define HQ4X_PIXEL13_10    Interp1(pOut+BpL+12, c[5], c[3]);
#define HQ4X_PIXEL13_12    Interp1(pOut+BpL+12, c[5], c[6]);
#define HQ4X_PIXEL13_14    Interp1(pOut+BpL+12, c[6], c[5]);
#define HQ4X_PIXEL13_21    Interp2(pOut+BpL+12, c[6], c[5], c[2]);
#define HQ4X_PIXEL13_31    Interp3(pOut+BpL+12, c[5], c[2]);
#define HQ4X_PIXEL13_50    Interp5(pOut+BpL+12, c[6], c[5]);
#define HQ4X_PIXEL13_60    Interp6(pOut+BpL+12, c[5], c[6], c[2]);
#define HQ4X_PIXEL13_61    Interp6(pOut+BpL+12, c[5], c[6], c[3]);
#define HQ4X_PIXEL13_82    Interp8(pOut+BpL+12, c[5], c[6]);
#define HQ4X_PIXEL13_83    Interp8(pOut+BpL+12, c[6], c[2]);
#define HQ4X_PIXEL20_0     *((int*)(pOut+BpL+BpL)) = c[5];
#define HQ4X_PIXEL20_10    Interp1(pOut+BpL+BpL, c[5], c[7]);
#define HQ4X_PIXEL20_12    Interp1(pOut+BpL+BpL, c[5], c[4]);
#define HQ4X_PIXEL20_14    Interp1(pOut+BpL+BpL, c[4], c[5]);
#define HQ4X_PIXEL20_21    Interp2(pOut+BpL+BpL, c[4], c[5], c[8]);
#define HQ4X_PIXEL20_31    Interp3(pOut+BpL+BpL, c[5], c[8]);
#define HQ4X_PIXEL20_50    Interp5(pOut+BpL+BpL, c[4], c[5]);
#define HQ4X_PIXEL20_60    Interp6(pOut+BpL+BpL, c[5], c[4], c[8]);
#define HQ4X_PIXEL20_61    Interp6(pOut+BpL+BpL, c[5], c[4], c[7]);
#define HQ4X_PIXEL20_82    Interp8(pOut+BpL+BpL, c[5], c[4]);
#define HQ4X_PIXEL20_83    Interp8(pOut+BpL+BpL, c[4], c[8]);
#define HQ4X_PIXEL21_0     *((int*)(pOut+BpL+BpL+4)) = c[5];
#define HQ4X_PIXEL21_30    Interp3(pOut+BpL+BpL+4, c[5], c[7]);
#define HQ4X_PIXEL21_31    Interp3(pOut+BpL+BpL+4, c[5], c[8]);
#define HQ4X_PIXEL21_32    Interp3(pOut+BpL+BpL+4, c[5], c[4]);
#define HQ4X_PIXEL21_70    Interp7(pOut+BpL+BpL+4, c[5], c[4], c[8]);
#define HQ4X_PIXEL22_0     *((int*)(pOut+BpL+BpL+8)) = c[5];
#define HQ4X_PIXEL22_30    Interp3(pOut+BpL+BpL+8, c[5], c[9]);
#define HQ4X_PIXEL22_31    Interp3(pOut+BpL+BpL+8, c[5], c[6]);
#define HQ4X_PIXEL22_32    Interp3(pOut+BpL+BpL+8, c[5], c[8]);
#define HQ4X_PIXEL22_70    Interp7(pOut+BpL+BpL+8, c[5], c[6], c[8]);
#define HQ4X_PIXEL23_0     *((int*)(pOut+BpL+BpL+12)) = c[5];
#define HQ4X_PIXEL23_10    Interp1(pOut+BpL+BpL+12, c[5], c[9]);
#define HQ4X_PIXEL23_11    Interp1(pOut+BpL+BpL+12, c[5], c[6]);
#define HQ4X_PIXEL23_13    Interp1(pOut+BpL+BpL+12, c[6], c[5]);
#define HQ4X_PIXEL23_21    Interp2(pOut+BpL+BpL+12, c[6], c[5], c[8]);
#define HQ4X_PIXEL23_32    Interp3(pOut+BpL+BpL+12, c[5], c[8]);
#define HQ4X_PIXEL23_50    Interp5(pOut+BpL+BpL+12, c[6], c[5]);
#define HQ4X_PIXEL23_60    Interp6(pOut+BpL+BpL+12, c[5], c[6], c[8]);
#define HQ4X_PIXEL23_61    Interp6(pOut+BpL+BpL+12, c[5], c[6], c[9]);
#define HQ4X_PIXEL23_81    Interp8(pOut+BpL+BpL+12, c[5], c[6]);
#define HQ4X_PIXEL23_83    Interp8(pOut+BpL+BpL+12, c[6], c[8]);
#define HQ4X_PIXEL30_0     *((int*)(pOut+BpL+BpL+BpL)) = c[5];
#define HQ4X_PIXEL30_11    Interp1(pOut+BpL+BpL+BpL, c[5], c[8]);
#define HQ4X_PIXEL30_12    Interp1(pOut+BpL+BpL+BpL, c[5], c[4]);
#define HQ4X_PIXEL30_20    Interp2(pOut+BpL+BpL+BpL, c[5], c[8], c[4]);
#define HQ4X_PIXEL30_50    Interp5(pOut+BpL+BpL+BpL, c[8], c[4]);
#define HQ4X_PIXEL30_80    Interp8(pOut+BpL+BpL+BpL, c[5], c[7]);
#define HQ4X_PIXEL30_81    Interp8(pOut+BpL+BpL+BpL, c[5], c[8]);
#define HQ4X_PIXEL30_82    Interp8(pOut+BpL+BpL+BpL, c[5], c[4]);
#define HQ4X_PIXEL31_0     *((int*)(pOut+BpL+BpL+BpL+4)) = c[5];
#define HQ4X_PIXEL31_10    Interp1(pOut+BpL+BpL+BpL+4, c[5], c[7]);
#define HQ4X_PIXEL31_11    Interp1(pOut+BpL+BpL+BpL+4, c[5], c[8]);
#define HQ4X_PIXEL31_13    Interp1(pOut+BpL+BpL+BpL+4, c[8], c[5]);
#define HQ4X_PIXEL31_21    Interp2(pOut+BpL+BpL+BpL+4, c[8], c[5], c[4]);
#define HQ4X_PIXEL31_32    Interp3(pOut+BpL+BpL+BpL+4, c[5], c[4]);
#define HQ4X_PIXEL31_50    Interp5(pOut+BpL+BpL+BpL+4, c[8], c[5]);
#define HQ4X_PIXEL31_60    Interp6(pOut+BpL+BpL+BpL+4, c[5], c[8], c[4]);
#define HQ4X_PIXEL31_61    Interp6(pOut+BpL+BpL+BpL+4, c[5], c[8], c[7]);
#define HQ4X_PIXEL31_81    Interp8(pOut+BpL+BpL+BpL+4, c[5], c[8]);
#define HQ4X_PIXEL31_83    Interp8(pOut+BpL+BpL+BpL+4, c[8], c[4]);
#define HQ4X_PIXEL32_0     *((int*)(pOut+BpL+BpL+BpL+8)) = c[5];
#define HQ4X_PIXEL32_10    Interp1(pOut+BpL+BpL+BpL+8, c[5], c[9]);
#define HQ4X_PIXEL32_12    Interp1(pOut+BpL+BpL+BpL+8, c[5], c[8]);
#define HQ4X_PIXEL32_14    Interp1(pOut+BpL+BpL+BpL+8, c[8], c[5]);
#define HQ4X_PIXEL32_21    Interp2(pOut+BpL+BpL+BpL+8, c[8], c[5], c[6]);
#define HQ4X_PIXEL32_31    Interp3(pOut+BpL+BpL+BpL+8, c[5], c[6]);
#define HQ4X_PIXEL32_50    Interp5(pOut+BpL+BpL+BpL+8, c[8], c[5]);
#define HQ4X_PIXEL32_60    Interp6(pOut+BpL+BpL+BpL+8, c[5], c[8], c[6]);
#define HQ4X_PIXEL32_61    Interp6(pOut+BpL+BpL+BpL+8, c[5], c[8], c[9]);
#define HQ4X_PIXEL32_82    Interp8(pOut+BpL+BpL+BpL+8, c[5], c[8]);
#define HQ4X_PIXEL32_83    Interp8(pOut+BpL+BpL+BpL+8, c[8], c[6]);
#define HQ4X_PIXEL33_0     *((int*)(pOut+BpL+BpL+BpL+12)) = c[5];
#define HQ4X_PIXEL33_11    Interp1(pOut+BpL+BpL+BpL+12, c[5], c[6]);
#define HQ4X_PIXEL33_12    Interp1(pOut+BpL+BpL+BpL+12, c[5], c[8]);
#define HQ4X_PIXEL33_20    Interp2(pOut+BpL+BpL+BpL+12, c[5], c[8], c[6]);
#define HQ4X_PIXEL33_50    Interp5(pOut+BpL+BpL+BpL+12, c[8], c[6]);
#define HQ4X_PIXEL33_80    Interp8(pOut+BpL+BpL+BpL+12, c[5], c[9]);
#define HQ4X_PIXEL33_81    Interp8(pOut+BpL+BpL+BpL+12, c[5], c[6]);
#define HQ4X_PIXEL33_82    Interp8(pOut+BpL+BpL+BpL+12, c[5], c[8]);

static inline bool Diff(unsigned int w1, unsigned int w2)
{
  YUV1 = RGBtoYUV[w1];
  YUV2 = RGBtoYUV[w2];
  return ( ( abs((YUV1 & Ymask) - (YUV2 & Ymask)) > trY ) ||
           ( abs((YUV1 & Umask) - (YUV2 & Umask)) > trU ) ||
           ( abs((YUV1 & Vmask) - (YUV2 & Vmask)) > trV ) );
}

static void InitLUTs(void)
{
  int i, j, k, r, g, b, Y, u, v;

  for (i=0; i<65536; i++)
    LUT16to32[i] = ((i & 0xF800) << 8) + ((i & 0x07E0) << 5) + ((i & 0x001F) << 3);

  for (i=0; i<32; i++)
  for (j=0; j<64; j++)
  for (k=0; k<32; k++)
  {
    r = i << 3;
    g = j << 2;
    b = k << 3;
    Y = (r + g + b) >> 2;
    u = 128 + ((r - b) >> 2);
    v = 128 + ((-r + 2*g -b)>>3);
    RGBtoYUV[ (i << 11) + (j << 5) + k ] = (Y<<16) + (u<<8) + v;
  }
}

void hq2x ( unsigned char * pIn, unsigned char * pOut, int Xres, int Yres, int BpL )
{
  int  i, j, k;
  int  prevline, nextline;
  int  w[10];
  int  c[10];

  if (!inited)
  {
    InitLUTs();
    inited = TRUE;
  }

  //   +----+----+----+
  //   |    |    |    |
  //   | w1 | w2 | w3 |
  //   +----+----+----+
  //   |    |    |    |
  //   | w4 | w5 | w6 |
  //   +----+----+----+
  //   |    |    |    |
  //   | w7 | w8 | w9 |
  //   +----+----+----+

  for (j=0; j<Yres; j++)
  {
    if (j>0)      prevline = -Xres*2; else prevline = 0;
    if (j<Yres-1) nextline =  Xres*2; else nextline = 0;

    for (i=0; i<Xres; i++)
    {
      w[2] = *((unsigned short*)(pIn + prevline));
      w[5] = *((unsigned short*)pIn);
      w[8] = *((unsigned short*)(pIn + nextline));

      if (i>0)
      {
        w[1] = *((unsigned short*)(pIn + prevline - 2));
        w[4] = *((unsigned short*)(pIn - 2));
        w[7] = *((unsigned short*)(pIn + nextline - 2));
      }
      else
      {
        w[1] = w[2];
        w[4] = w[5];
        w[7] = w[8];
      }

      if (i<Xres-1)
      {
        w[3] = *((unsigned short*)(pIn + prevline + 2));
        w[6] = *((unsigned short*)(pIn + 2));
        w[9] = *((unsigned short*)(pIn + nextline + 2));
      }
      else
      {
        w[3] = w[2];
        w[6] = w[5];
        w[9] = w[8];
      }

      int pattern = 0;
      int flag = 1;

      YUV1 = RGBtoYUV[w[5]];

      for (k=1; k<=9; k++)
      {
        if (k==5) continue;

        if ( w[k] != w[5] )
        {
          YUV2 = RGBtoYUV[w[k]];
          if ( ( abs((YUV1 & Ymask) - (YUV2 & Ymask)) > trY ) ||
               ( abs((YUV1 & Umask) - (YUV2 & Umask)) > trU ) ||
               ( abs((YUV1 & Vmask) - (YUV2 & Vmask)) > trV ) )
            pattern |= flag;
        }
        flag <<= 1;
      }

      for (k=1; k<=9; k++)
        c[k] = LUT16to32[w[k]];

      switch (pattern)
      {
        case 0:
        case 1:
        case 4:
        case 32:
        case 128:
        case 5:
        case 132:
        case 160:
        case 33:
        case 129:
        case 36:
        case 133:
        case 164:
        case 161:
        case 37:
        case 165:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_20
          break;
        }
        case 2:
        case 34:
        case 130:
        case 162:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_20
          break;
        }
        case 16:
        case 17:
        case 48:
        case 49:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_21
          break;
        }
        case 64:
        case 65:
        case 68:
        case 69:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_22
          break;
        }
        case 8:
        case 12:
        case 136:
        case 140:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_20
          break;
        }
        case 3:
        case 35:
        case 131:
        case 163:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_20
          break;
        }
        case 6:
        case 38:
        case 134:
        case 166:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_20
          break;
        }
        case 20:
        case 21:
        case 52:
        case 53:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_21
          break;
        }
        case 144:
        case 145:
        case 176:
        case 177:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_12
          break;
        }
        case 192:
        case 193:
        case 196:
        case 197:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_11
          break;
        }
        case 96:
        case 97:
        case 100:
        case 101:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_22
          break;
        }
        case 40:
        case 44:
        case 168:
        case 172:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_20
          break;
        }
        case 9:
        case 13:
        case 137:
        case 141:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_20
          break;
        }
        case 18:
        case 50:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_21
          break;
        }
        case 80:
        case 81:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 72:
        case 76:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_20
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 10:
        case 138:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_20
          break;
        }
        case 66:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_22
          break;
        }
        case 24:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_21
          break;
        }
        case 7:
        case 39:
        case 135:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_20
          break;
        }
        case 148:
        case 149:
        case 180:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_12
          break;
        }
        case 224:
        case 228:
        case 225:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_11
          break;
        }
        case 41:
        case 169:
        case 45:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_20
          break;
        }
        case 22:
        case 54:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_21
          break;
        }
        case 208:
        case 209:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 104:
        case 108:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_20
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 11:
        case 139:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_20
          break;
        }
        case 19:
        case 51:
        {
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL00_11
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL00_60
            HQ2X_PIXEL01_90
          }
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_21
          break;
        }
        case 146:
        case 178:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
            HQ2X_PIXEL11_12
          }
          else
          {
            HQ2X_PIXEL01_90
            HQ2X_PIXEL11_61
          }
          HQ2X_PIXEL10_20
          break;
        }
        case 84:
        case 85:
        {
          HQ2X_PIXEL00_20
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL01_11
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL01_60
            HQ2X_PIXEL11_90
          }
          HQ2X_PIXEL10_21
          break;
        }
        case 112:
        case 113:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_22
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL10_12
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL10_61
            HQ2X_PIXEL11_90
          }
          break;
        }
        case 200:
        case 204:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_20
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
            HQ2X_PIXEL11_11
          }
          else
          {
            HQ2X_PIXEL10_90
            HQ2X_PIXEL11_60
          }
          break;
        }
        case 73:
        case 77:
        {
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL00_12
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL00_61
            HQ2X_PIXEL10_90
          }
          HQ2X_PIXEL01_20
          HQ2X_PIXEL11_22
          break;
        }
        case 42:
        case 170:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
            HQ2X_PIXEL10_11
          }
          else
          {
            HQ2X_PIXEL00_90
            HQ2X_PIXEL10_60
          }
          HQ2X_PIXEL01_21
          HQ2X_PIXEL11_20
          break;
        }
        case 14:
        case 142:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
            HQ2X_PIXEL01_12
          }
          else
          {
            HQ2X_PIXEL00_90
            HQ2X_PIXEL01_61
          }
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_20
          break;
        }
        case 67:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_22
          break;
        }
        case 70:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_22
          break;
        }
        case 28:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_21
          break;
        }
        case 152:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 194:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_11
          break;
        }
        case 98:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_22
          break;
        }
        case 56:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 25:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_21
          break;
        }
        case 26:
        case 31:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_21
          break;
        }
        case 82:
        case 214:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 88:
        case 248:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_22
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 74:
        case 107:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_21
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 27:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_10
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_21
          break;
        }
        case 86:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_10
          break;
        }
        case 216:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_10
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 106:
        {
          HQ2X_PIXEL00_10
          HQ2X_PIXEL01_21
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 30:
        {
          HQ2X_PIXEL00_10
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_21
          break;
        }
        case 210:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_10
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 120:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_22
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_10
          break;
        }
        case 75:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_10
          HQ2X_PIXEL11_22
          break;
        }
        case 29:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_21
          break;
        }
        case 198:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_11
          break;
        }
        case 184:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_12
          break;
        }
        case 99:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_22
          break;
        }
        case 57:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 71:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_22
          break;
        }
        case 156:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 226:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_11
          break;
        }
        case 60:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 195:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_11
          break;
        }
        case 102:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_22
          break;
        }
        case 153:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 58:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 83:
        {
          HQ2X_PIXEL00_11
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 92:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 202:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          HQ2X_PIXEL01_21
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          HQ2X_PIXEL11_11
          break;
        }
        case 78:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          HQ2X_PIXEL01_12
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 154:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 114:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 89:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 90:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 55:
        case 23:
        {
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL00_11
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL00_60
            HQ2X_PIXEL01_90
          }
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_21
          break;
        }
        case 182:
        case 150:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
            HQ2X_PIXEL11_12
          }
          else
          {
            HQ2X_PIXEL01_90
            HQ2X_PIXEL11_61
          }
          HQ2X_PIXEL10_20
          break;
        }
        case 213:
        case 212:
        {
          HQ2X_PIXEL00_20
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL01_11
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL01_60
            HQ2X_PIXEL11_90
          }
          HQ2X_PIXEL10_21
          break;
        }
        case 241:
        case 240:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_22
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL10_12
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL10_61
            HQ2X_PIXEL11_90
          }
          break;
        }
        case 236:
        case 232:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_20
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
            HQ2X_PIXEL11_11
          }
          else
          {
            HQ2X_PIXEL10_90
            HQ2X_PIXEL11_60
          }
          break;
        }
        case 109:
        case 105:
        {
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL00_12
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL00_61
            HQ2X_PIXEL10_90
          }
          HQ2X_PIXEL01_20
          HQ2X_PIXEL11_22
          break;
        }
        case 171:
        case 43:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
            HQ2X_PIXEL10_11
          }
          else
          {
            HQ2X_PIXEL00_90
            HQ2X_PIXEL10_60
          }
          HQ2X_PIXEL01_21
          HQ2X_PIXEL11_20
          break;
        }
        case 143:
        case 15:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
            HQ2X_PIXEL01_12
          }
          else
          {
            HQ2X_PIXEL00_90
            HQ2X_PIXEL01_61
          }
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_20
          break;
        }
        case 124:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_10
          break;
        }
        case 203:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_10
          HQ2X_PIXEL11_11
          break;
        }
        case 62:
        {
          HQ2X_PIXEL00_10
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 211:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_10
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 118:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_10
          break;
        }
        case 217:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_10
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 110:
        {
          HQ2X_PIXEL00_10
          HQ2X_PIXEL01_12
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 155:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_10
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 188:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_12
          break;
        }
        case 185:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_12
          break;
        }
        case 61:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 157:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 103:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_22
          break;
        }
        case 227:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_21
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_11
          break;
        }
        case 230:
        {
          HQ2X_PIXEL00_22
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_11
          break;
        }
        case 199:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_21
          HQ2X_PIXEL11_11
          break;
        }
        case 220:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 158:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 234:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          HQ2X_PIXEL01_21
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_11
          break;
        }
        case 242:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 59:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 121:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 87:
        {
          HQ2X_PIXEL00_11
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 79:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_12
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 122:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 94:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 218:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 91:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 229:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_11
          break;
        }
        case 167:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_20
          break;
        }
        case 173:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_20
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_20
          break;
        }
        case 181:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_12
          break;
        }
        case 186:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_12
          break;
        }
        case 115:
        {
          HQ2X_PIXEL00_11
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 93:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_11
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 206:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          HQ2X_PIXEL01_12
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          HQ2X_PIXEL11_11
          break;
        }
        case 205:
        case 201:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_20
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_10
          }
          else
          {
            HQ2X_PIXEL10_70
          }
          HQ2X_PIXEL11_11
          break;
        }
        case 174:
        case 46:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_10
          }
          else
          {
            HQ2X_PIXEL00_70
          }
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_20
          break;
        }
        case 179:
        case 147:
        {
          HQ2X_PIXEL00_11
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_10
          }
          else
          {
            HQ2X_PIXEL01_70
          }
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_12
          break;
        }
        case 117:
        case 116:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_10
          }
          else
          {
            HQ2X_PIXEL11_70
          }
          break;
        }
        case 189:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_12
          break;
        }
        case 231:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_11
          break;
        }
        case 126:
        {
          HQ2X_PIXEL00_10
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_10
          break;
        }
        case 219:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_10
          HQ2X_PIXEL10_10
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 125:
        {
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL00_12
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL00_61
            HQ2X_PIXEL10_90
          }
          HQ2X_PIXEL01_11
          HQ2X_PIXEL11_10
          break;
        }
        case 221:
        {
          HQ2X_PIXEL00_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL01_11
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL01_60
            HQ2X_PIXEL11_90
          }
          HQ2X_PIXEL10_10
          break;
        }
        case 207:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
            HQ2X_PIXEL01_12
          }
          else
          {
            HQ2X_PIXEL00_90
            HQ2X_PIXEL01_61
          }
          HQ2X_PIXEL10_10
          HQ2X_PIXEL11_11
          break;
        }
        case 238:
        {
          HQ2X_PIXEL00_10
          HQ2X_PIXEL01_12
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
            HQ2X_PIXEL11_11
          }
          else
          {
            HQ2X_PIXEL10_90
            HQ2X_PIXEL11_60
          }
          break;
        }
        case 190:
        {
          HQ2X_PIXEL00_10
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
            HQ2X_PIXEL11_12
          }
          else
          {
            HQ2X_PIXEL01_90
            HQ2X_PIXEL11_61
          }
          HQ2X_PIXEL10_11
          break;
        }
        case 187:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
            HQ2X_PIXEL10_11
          }
          else
          {
            HQ2X_PIXEL00_90
            HQ2X_PIXEL10_60
          }
          HQ2X_PIXEL01_10
          HQ2X_PIXEL11_12
          break;
        }
        case 243:
        {
          HQ2X_PIXEL00_11
          HQ2X_PIXEL01_10
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL10_12
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL10_61
            HQ2X_PIXEL11_90
          }
          break;
        }
        case 119:
        {
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL00_11
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL00_60
            HQ2X_PIXEL01_90
          }
          HQ2X_PIXEL10_12
          HQ2X_PIXEL11_10
          break;
        }
        case 237:
        case 233:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_20
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_100
          }
          HQ2X_PIXEL11_11
          break;
        }
        case 175:
        case 47:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_100
          }
          HQ2X_PIXEL01_12
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_20
          break;
        }
        case 183:
        case 151:
        {
          HQ2X_PIXEL00_11
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_100
          }
          HQ2X_PIXEL10_20
          HQ2X_PIXEL11_12
          break;
        }
        case 245:
        case 244:
        {
          HQ2X_PIXEL00_20
          HQ2X_PIXEL01_11
          HQ2X_PIXEL10_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_100
          }
          break;
        }
        case 250:
        {
          HQ2X_PIXEL00_10
          HQ2X_PIXEL01_10
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 123:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_10
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_10
          break;
        }
        case 95:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_10
          HQ2X_PIXEL11_10
          break;
        }
        case 222:
        {
          HQ2X_PIXEL00_10
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_10
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 252:
        {
          HQ2X_PIXEL00_21
          HQ2X_PIXEL01_11
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_100
          }
          break;
        }
        case 249:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_22
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_100
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 235:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_21
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_100
          }
          HQ2X_PIXEL11_11
          break;
        }
        case 111:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_100
          }
          HQ2X_PIXEL01_12
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_22
          break;
        }
        case 63:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_100
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_21
          break;
        }
        case 159:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_100
          }
          HQ2X_PIXEL10_22
          HQ2X_PIXEL11_12
          break;
        }
        case 215:
        {
          HQ2X_PIXEL00_11
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_100
          }
          HQ2X_PIXEL10_21
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 246:
        {
          HQ2X_PIXEL00_22
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          HQ2X_PIXEL10_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_100
          }
          break;
        }
        case 254:
        {
          HQ2X_PIXEL00_10
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_100
          }
          break;
        }
        case 253:
        {
          HQ2X_PIXEL00_12
          HQ2X_PIXEL01_11
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_100
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_100
          }
          break;
        }
        case 251:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          HQ2X_PIXEL01_10
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_100
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 239:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_100
          }
          HQ2X_PIXEL01_12
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_100
          }
          HQ2X_PIXEL11_11
          break;
        }
        case 127:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_100
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_20
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_20
          }
          HQ2X_PIXEL11_10
          break;
        }
        case 191:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_100
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_100
          }
          HQ2X_PIXEL10_11
          HQ2X_PIXEL11_12
          break;
        }
        case 223:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_20
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_100
          }
          HQ2X_PIXEL10_10
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_20
          }
          break;
        }
        case 247:
        {
          HQ2X_PIXEL00_11
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_100
          }
          HQ2X_PIXEL10_12
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_100
          }
          break;
        }
        case 255:
        {
          if (Diff(w[4], w[2]))
          {
            HQ2X_PIXEL00_0
          }
          else
          {
            HQ2X_PIXEL00_100
          }
          if (Diff(w[2], w[6]))
          {
            HQ2X_PIXEL01_0
          }
          else
          {
            HQ2X_PIXEL01_100
          }
          if (Diff(w[8], w[4]))
          {
            HQ2X_PIXEL10_0
          }
          else
          {
            HQ2X_PIXEL10_100
          }
          if (Diff(w[6], w[8]))
          {
            HQ2X_PIXEL11_0
          }
          else
          {
            HQ2X_PIXEL11_100
          }
          break;
        }
      }
      pIn+=2;
      pOut+=8;
    }
    pOut+=BpL;
  }
}

void hq3x ( unsigned char * pIn, unsigned char * pOut, int Xres, int Yres, int BpL )
{
  int  i, j, k;
  int  prevline, nextline;
  int  w[10];
  int  c[10];

  if (!inited)
  {
    InitLUTs();
    inited = TRUE;
  }

  //   +----+----+----+
  //   |    |    |    |
  //   | w1 | w2 | w3 |
  //   +----+----+----+
  //   |    |    |    |
  //   | w4 | w5 | w6 |
  //   +----+----+----+
  //   |    |    |    |
  //   | w7 | w8 | w9 |
  //   +----+----+----+

  for (j=0; j<Yres; j++)
  {
    if (j>0)      prevline = -Xres*2; else prevline = 0;
    if (j<Yres-1) nextline =  Xres*2; else nextline = 0;

    for (i=0; i<Xres; i++)
    {
      w[2] = *((unsigned short*)(pIn + prevline));
      w[5] = *((unsigned short*)pIn);
      w[8] = *((unsigned short*)(pIn + nextline));

      if (i>0)
      {
        w[1] = *((unsigned short*)(pIn + prevline - 2));
        w[4] = *((unsigned short*)(pIn - 2));
        w[7] = *((unsigned short*)(pIn + nextline - 2));
      }
      else
      {
        w[1] = w[2];
        w[4] = w[5];
        w[7] = w[8];
      }

      if (i<Xres-1)
      {
        w[3] = *((unsigned short*)(pIn + prevline + 2));
        w[6] = *((unsigned short*)(pIn + 2));
        w[9] = *((unsigned short*)(pIn + nextline + 2));
      }
      else
      {
        w[3] = w[2];
        w[6] = w[5];
        w[9] = w[8];
      }

      int pattern = 0;
      int flag = 1;

      YUV1 = RGBtoYUV[w[5]];

      for (k=1; k<=9; k++)
      {
        if (k==5) continue;

        if ( w[k] != w[5] )
        {
          YUV2 = RGBtoYUV[w[k]];
          if ( ( abs((YUV1 & Ymask) - (YUV2 & Ymask)) > trY ) ||
               ( abs((YUV1 & Umask) - (YUV2 & Umask)) > trU ) ||
               ( abs((YUV1 & Vmask) - (YUV2 & Vmask)) > trV ) )
            pattern |= flag;
        }
        flag <<= 1;
      }

      for (k=1; k<=9; k++)
        c[k] = LUT16to32[w[k]];

      switch (pattern)
      {
        case 0:
        case 1:
        case 4:
        case 32:
        case 128:
        case 5:
        case 132:
        case 160:
        case 33:
        case 129:
        case 36:
        case 133:
        case 164:
        case 161:
        case 37:
        case 165:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 2:
        case 34:
        case 130:
        case 162:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 16:
        case 17:
        case 48:
        case 49:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 64:
        case 65:
        case 68:
        case 69:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 8:
        case 12:
        case 136:
        case 140:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 3:
        case 35:
        case 131:
        case 163:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 6:
        case 38:
        case 134:
        case 166:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 20:
        case 21:
        case 52:
        case 53:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 144:
        case 145:
        case 176:
        case 177:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 192:
        case 193:
        case 196:
        case 197:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 96:
        case 97:
        case 100:
        case 101:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 40:
        case 44:
        case 168:
        case 172:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 9:
        case 13:
        case 137:
        case 141:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 18:
        case 50:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_1M
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 80:
        case 81:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 72:
        case 76:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_1M
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 10:
        case 138:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 66:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 24:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 7:
        case 39:
        case 135:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 148:
        case 149:
        case 180:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 224:
        case 228:
        case 225:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 41:
        case 169:
        case 45:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 22:
        case 54:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 208:
        case 209:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 104:
        case 108:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 11:
        case 139:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 19:
        case 51:
        {
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL00_1L
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_1M
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL00_2
            HQ3X_PIXEL01_6
            HQ3X_PIXEL02_5
            HQ3X_PIXEL12_1
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 146:
        case 178:
        {
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_1M
            HQ3X_PIXEL12_C
            HQ3X_PIXEL22_1D
          }
          else
          {
            HQ3X_PIXEL01_1
            HQ3X_PIXEL02_5
            HQ3X_PIXEL12_6
            HQ3X_PIXEL22_2
          }
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          break;
        }
        case 84:
        case 85:
        {
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL02_1U
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL02_2
            HQ3X_PIXEL12_6
            HQ3X_PIXEL21_1
            HQ3X_PIXEL22_5
          }
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          break;
        }
        case 112:
        case 113:
        {
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL20_1L
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL12_1
            HQ3X_PIXEL20_2
            HQ3X_PIXEL21_6
            HQ3X_PIXEL22_5
          }
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          break;
        }
        case 200:
        case 204:
        {
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_1M
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_1R
          }
          else
          {
            HQ3X_PIXEL10_1
            HQ3X_PIXEL20_5
            HQ3X_PIXEL21_6
            HQ3X_PIXEL22_2
          }
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          break;
        }
        case 73:
        case 77:
        {
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL00_1U
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_1M
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL00_2
            HQ3X_PIXEL10_6
            HQ3X_PIXEL20_5
            HQ3X_PIXEL21_1
          }
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 42:
        case 170:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_1D
          }
          else
          {
            HQ3X_PIXEL00_5
            HQ3X_PIXEL01_1
            HQ3X_PIXEL10_6
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 14:
        case 142:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_1R
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_5
            HQ3X_PIXEL01_6
            HQ3X_PIXEL02_2
            HQ3X_PIXEL10_1
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 67:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 70:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 28:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 152:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 194:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 98:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 56:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 25:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 26:
        case 31:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 82:
        case 214:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 88:
        case 248:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 74:
        case 107:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 27:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 86:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 216:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 106:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 30:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 210:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 120:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 75:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 29:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 198:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 184:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 99:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 57:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 71:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 156:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 226:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 60:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 195:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 102:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 153:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 58:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 83:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 92:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 202:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 78:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 154:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 114:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 89:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 90:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 55:
        case 23:
        {
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL00_1L
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL00_2
            HQ3X_PIXEL01_6
            HQ3X_PIXEL02_5
            HQ3X_PIXEL12_1
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 182:
        case 150:
        {
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
            HQ3X_PIXEL22_1D
          }
          else
          {
            HQ3X_PIXEL01_1
            HQ3X_PIXEL02_5
            HQ3X_PIXEL12_6
            HQ3X_PIXEL22_2
          }
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          break;
        }
        case 213:
        case 212:
        {
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL02_1U
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL02_2
            HQ3X_PIXEL12_6
            HQ3X_PIXEL21_1
            HQ3X_PIXEL22_5
          }
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          break;
        }
        case 241:
        case 240:
        {
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL20_1L
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_1
            HQ3X_PIXEL20_2
            HQ3X_PIXEL21_6
            HQ3X_PIXEL22_5
          }
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          break;
        }
        case 236:
        case 232:
        {
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_1R
          }
          else
          {
            HQ3X_PIXEL10_1
            HQ3X_PIXEL20_5
            HQ3X_PIXEL21_6
            HQ3X_PIXEL22_2
          }
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          break;
        }
        case 109:
        case 105:
        {
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL00_1U
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL00_2
            HQ3X_PIXEL10_6
            HQ3X_PIXEL20_5
            HQ3X_PIXEL21_1
          }
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 171:
        case 43:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_1D
          }
          else
          {
            HQ3X_PIXEL00_5
            HQ3X_PIXEL01_1
            HQ3X_PIXEL10_6
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 143:
        case 15:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_1R
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_5
            HQ3X_PIXEL01_6
            HQ3X_PIXEL02_2
            HQ3X_PIXEL10_1
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 124:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 203:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 62:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 211:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 118:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 217:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 110:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 155:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 188:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 185:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 61:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 157:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 103:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 227:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 230:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 199:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 220:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 158:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 234:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1R
          break;
        }
        case 242:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1L
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 59:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 121:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 87:
        {
          HQ3X_PIXEL00_1L
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 79:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 122:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 94:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 218:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 91:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 229:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 167:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 173:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 181:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 186:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 115:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 93:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 206:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 205:
        case 201:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_1M
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 174:
        case 46:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_1M
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 179:
        case 147:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_1M
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 117:
        case 116:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_1M
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 189:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 231:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 126:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 219:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 125:
        {
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL00_1U
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL00_2
            HQ3X_PIXEL10_6
            HQ3X_PIXEL20_5
            HQ3X_PIXEL21_1
          }
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 221:
        {
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL02_1U
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL02_2
            HQ3X_PIXEL12_6
            HQ3X_PIXEL21_1
            HQ3X_PIXEL22_5
          }
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          break;
        }
        case 207:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_1R
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_5
            HQ3X_PIXEL01_6
            HQ3X_PIXEL02_2
            HQ3X_PIXEL10_1
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 238:
        {
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_1R
          }
          else
          {
            HQ3X_PIXEL10_1
            HQ3X_PIXEL20_5
            HQ3X_PIXEL21_6
            HQ3X_PIXEL22_2
          }
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          break;
        }
        case 190:
        {
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
            HQ3X_PIXEL22_1D
          }
          else
          {
            HQ3X_PIXEL01_1
            HQ3X_PIXEL02_5
            HQ3X_PIXEL12_6
            HQ3X_PIXEL22_2
          }
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          break;
        }
        case 187:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_1D
          }
          else
          {
            HQ3X_PIXEL00_5
            HQ3X_PIXEL01_1
            HQ3X_PIXEL10_6
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 243:
        {
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL20_1L
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_1
            HQ3X_PIXEL20_2
            HQ3X_PIXEL21_6
            HQ3X_PIXEL22_5
          }
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          break;
        }
        case 119:
        {
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL00_1L
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL00_2
            HQ3X_PIXEL01_6
            HQ3X_PIXEL02_5
            HQ3X_PIXEL12_1
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 237:
        case 233:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_2
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 175:
        case 47:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_2
          break;
        }
        case 183:
        case 151:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_2
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 245:
        case 244:
        {
          HQ3X_PIXEL00_2
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 250:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 123:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 95:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1M
          break;
        }
        case 222:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 252:
        {
          HQ3X_PIXEL00_1M
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 249:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 235:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 111:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 63:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1M
          break;
        }
        case 159:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL10_3
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 215:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 246:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 254:
        {
          HQ3X_PIXEL00_1M
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_4
          }
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_4
          }
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 253:
        {
          HQ3X_PIXEL00_1U
          HQ3X_PIXEL01_1
          HQ3X_PIXEL02_1U
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 251:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL01_3
          }
          HQ3X_PIXEL02_1M
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL10_C
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL10_3
            HQ3X_PIXEL20_2
            HQ3X_PIXEL21_3
          }
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL12_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL12_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 239:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          HQ3X_PIXEL02_1R
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_1
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          HQ3X_PIXEL22_1R
          break;
        }
        case 127:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL01_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_2
            HQ3X_PIXEL01_3
            HQ3X_PIXEL10_3
          }
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL02_4
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL11
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
            HQ3X_PIXEL21_C
          }
          else
          {
            HQ3X_PIXEL20_4
            HQ3X_PIXEL21_3
          }
          HQ3X_PIXEL22_1M
          break;
        }
        case 191:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1D
          HQ3X_PIXEL21_1
          HQ3X_PIXEL22_1D
          break;
        }
        case 223:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
            HQ3X_PIXEL10_C
          }
          else
          {
            HQ3X_PIXEL00_4
            HQ3X_PIXEL10_3
          }
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL01_C
            HQ3X_PIXEL02_C
            HQ3X_PIXEL12_C
          }
          else
          {
            HQ3X_PIXEL01_3
            HQ3X_PIXEL02_2
            HQ3X_PIXEL12_3
          }
          HQ3X_PIXEL11
          HQ3X_PIXEL20_1M
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL21_C
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL21_3
            HQ3X_PIXEL22_4
          }
          break;
        }
        case 247:
        {
          HQ3X_PIXEL00_1L
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_1
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          HQ3X_PIXEL20_1L
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
        case 255:
        {
          if (Diff(w[4], w[2]))
          {
            HQ3X_PIXEL00_C
          }
          else
          {
            HQ3X_PIXEL00_2
          }
          HQ3X_PIXEL01_C
          if (Diff(w[2], w[6]))
          {
            HQ3X_PIXEL02_C
          }
          else
          {
            HQ3X_PIXEL02_2
          }
          HQ3X_PIXEL10_C
          HQ3X_PIXEL11
          HQ3X_PIXEL12_C
          if (Diff(w[8], w[4]))
          {
            HQ3X_PIXEL20_C
          }
          else
          {
            HQ3X_PIXEL20_2
          }
          HQ3X_PIXEL21_C
          if (Diff(w[6], w[8]))
          {
            HQ3X_PIXEL22_C
          }
          else
          {
            HQ3X_PIXEL22_2
          }
          break;
        }
      }
      pIn+=2;
      pOut+=12;
    }
    pOut+=BpL;
    pOut+=BpL;
  }
}

void hq4x ( unsigned char * pIn, unsigned char * pOut, int Xres, int Yres, int BpL )
{
  int  i, j, k;
  int  prevline, nextline;
  int  w[10];
  int  c[10];

  if (!inited)
  {
    InitLUTs();
    inited = TRUE;
  }

  //   +----+----+----+
  //   |    |    |    |
  //   | w1 | w2 | w3 |
  //   +----+----+----+
  //   |    |    |    |
  //   | w4 | w5 | w6 |
  //   +----+----+----+
  //   |    |    |    |
  //   | w7 | w8 | w9 |
  //   +----+----+----+

  for (j=0; j<Yres; j++)
  {
    if (j>0)      prevline = -Xres*2; else prevline = 0;
    if (j<Yres-1) nextline =  Xres*2; else nextline = 0;

    for (i=0; i<Xres; i++)
    {
      w[2] = *((unsigned short*)(pIn + prevline));
      w[5] = *((unsigned short*)pIn);
      w[8] = *((unsigned short*)(pIn + nextline));

      if (i>0)
      {
        w[1] = *((unsigned short*)(pIn + prevline - 2));
        w[4] = *((unsigned short*)(pIn - 2));
        w[7] = *((unsigned short*)(pIn + nextline - 2));
      }
      else
      {
        w[1] = w[2];
        w[4] = w[5];
        w[7] = w[8];
      }

      if (i<Xres-1)
      {
        w[3] = *((unsigned short*)(pIn + prevline + 2));
        w[6] = *((unsigned short*)(pIn + 2));
        w[9] = *((unsigned short*)(pIn + nextline + 2));
      }
      else
      {
        w[3] = w[2];
        w[6] = w[5];
        w[9] = w[8];
      }

      int pattern = 0;
      int flag = 1;

      YUV1 = RGBtoYUV[w[5]];

      for (k=1; k<=9; k++)
      {
        if (k==5) continue;

        if ( w[k] != w[5] )
        {
          YUV2 = RGBtoYUV[w[k]];
          if ( ( abs((YUV1 & Ymask) - (YUV2 & Ymask)) > trY ) ||
               ( abs((YUV1 & Umask) - (YUV2 & Umask)) > trU ) ||
               ( abs((YUV1 & Vmask) - (YUV2 & Vmask)) > trV ) )
            pattern |= flag;
        }
        flag <<= 1;
      }

      for (k=1; k<=9; k++)
        c[k] = LUT16to32[w[k]];

      switch (pattern)
      {
        case 0:
        case 1:
        case 4:
        case 32:
        case 128:
        case 5:
        case 132:
        case 160:
        case 33:
        case 129:
        case 36:
        case 133:
        case 164:
        case 161:
        case 37:
        case 165:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 2:
        case 34:
        case 130:
        case 162:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 16:
        case 17:
        case 48:
        case 49:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 64:
        case 65:
        case 68:
        case 69:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 8:
        case 12:
        case 136:
        case 140:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 3:
        case 35:
        case 131:
        case 163:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 6:
        case 38:
        case 134:
        case 166:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 20:
        case 21:
        case 52:
        case 53:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 144:
        case 145:
        case 176:
        case 177:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 192:
        case 193:
        case 196:
        case 197:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 96:
        case 97:
        case 100:
        case 101:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 40:
        case 44:
        case 168:
        case 172:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 9:
        case 13:
        case 137:
        case 141:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 18:
        case 50:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 80:
        case 81:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 72:
        case 76:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 10:
        case 138:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
            HQ4X_PIXEL11_0
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 66:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 24:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 7:
        case 39:
        case 135:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 148:
        case 149:
        case 180:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 224:
        case 228:
        case 225:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 41:
        case 169:
        case 45:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 22:
        case 54:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 208:
        case 209:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 104:
        case 108:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 11:
        case 139:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 19:
        case 51:
        {
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL00_81
            HQ4X_PIXEL01_31
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL00_12
            HQ4X_PIXEL01_14
            HQ4X_PIXEL02_83
            HQ4X_PIXEL03_50
            HQ4X_PIXEL12_70
            HQ4X_PIXEL13_21
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 146:
        case 178:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
            HQ4X_PIXEL23_32
            HQ4X_PIXEL33_82
          }
          else
          {
            HQ4X_PIXEL02_21
            HQ4X_PIXEL03_50
            HQ4X_PIXEL12_70
            HQ4X_PIXEL13_83
            HQ4X_PIXEL23_13
            HQ4X_PIXEL33_11
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_32
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_82
          break;
        }
        case 84:
        case 85:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_81
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL03_81
            HQ4X_PIXEL13_31
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL03_12
            HQ4X_PIXEL13_14
            HQ4X_PIXEL22_70
            HQ4X_PIXEL23_83
            HQ4X_PIXEL32_21
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_31
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 112:
        case 113:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL30_82
            HQ4X_PIXEL31_32
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_70
            HQ4X_PIXEL23_21
            HQ4X_PIXEL30_11
            HQ4X_PIXEL31_13
            HQ4X_PIXEL32_83
            HQ4X_PIXEL33_50
          }
          break;
        }
        case 200:
        case 204:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
            HQ4X_PIXEL32_31
            HQ4X_PIXEL33_81
          }
          else
          {
            HQ4X_PIXEL20_21
            HQ4X_PIXEL21_70
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_83
            HQ4X_PIXEL32_14
            HQ4X_PIXEL33_12
          }
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          break;
        }
        case 73:
        case 77:
        {
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL00_82
            HQ4X_PIXEL10_32
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL00_11
            HQ4X_PIXEL10_13
            HQ4X_PIXEL20_83
            HQ4X_PIXEL21_70
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_21
          }
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 42:
        case 170:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
            HQ4X_PIXEL20_31
            HQ4X_PIXEL30_81
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_21
            HQ4X_PIXEL10_83
            HQ4X_PIXEL11_70
            HQ4X_PIXEL20_14
            HQ4X_PIXEL30_12
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 14:
        case 142:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL02_32
            HQ4X_PIXEL03_82
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_83
            HQ4X_PIXEL02_13
            HQ4X_PIXEL03_11
            HQ4X_PIXEL10_21
            HQ4X_PIXEL11_70
          }
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 67:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 70:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 28:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 152:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 194:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 98:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 56:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 25:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 26:
        case 31:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 82:
        case 214:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 88:
        case 248:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          break;
        }
        case 74:
        case 107:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 27:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 86:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 216:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 106:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 30:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 210:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 120:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 75:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 29:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 198:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 184:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 99:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 57:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 71:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 156:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 226:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 60:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 195:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 102:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 153:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 58:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 83:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 92:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 202:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 78:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 154:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 114:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          break;
        }
        case 89:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 90:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 55:
        case 23:
        {
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL00_81
            HQ4X_PIXEL01_31
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL00_12
            HQ4X_PIXEL01_14
            HQ4X_PIXEL02_83
            HQ4X_PIXEL03_50
            HQ4X_PIXEL12_70
            HQ4X_PIXEL13_21
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 182:
        case 150:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_0
            HQ4X_PIXEL23_32
            HQ4X_PIXEL33_82
          }
          else
          {
            HQ4X_PIXEL02_21
            HQ4X_PIXEL03_50
            HQ4X_PIXEL12_70
            HQ4X_PIXEL13_83
            HQ4X_PIXEL23_13
            HQ4X_PIXEL33_11
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_32
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_82
          break;
        }
        case 213:
        case 212:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_81
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL03_81
            HQ4X_PIXEL13_31
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL03_12
            HQ4X_PIXEL13_14
            HQ4X_PIXEL22_70
            HQ4X_PIXEL23_83
            HQ4X_PIXEL32_21
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_31
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 241:
        case 240:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_0
            HQ4X_PIXEL30_82
            HQ4X_PIXEL31_32
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL22_70
            HQ4X_PIXEL23_21
            HQ4X_PIXEL30_11
            HQ4X_PIXEL31_13
            HQ4X_PIXEL32_83
            HQ4X_PIXEL33_50
          }
          break;
        }
        case 236:
        case 232:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
            HQ4X_PIXEL32_31
            HQ4X_PIXEL33_81
          }
          else
          {
            HQ4X_PIXEL20_21
            HQ4X_PIXEL21_70
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_83
            HQ4X_PIXEL32_14
            HQ4X_PIXEL33_12
          }
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          break;
        }
        case 109:
        case 105:
        {
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL00_82
            HQ4X_PIXEL10_32
            HQ4X_PIXEL20_0
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL00_11
            HQ4X_PIXEL10_13
            HQ4X_PIXEL20_83
            HQ4X_PIXEL21_70
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_21
          }
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 171:
        case 43:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
            HQ4X_PIXEL11_0
            HQ4X_PIXEL20_31
            HQ4X_PIXEL30_81
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_21
            HQ4X_PIXEL10_83
            HQ4X_PIXEL11_70
            HQ4X_PIXEL20_14
            HQ4X_PIXEL30_12
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 143:
        case 15:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL02_32
            HQ4X_PIXEL03_82
            HQ4X_PIXEL10_0
            HQ4X_PIXEL11_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_83
            HQ4X_PIXEL02_13
            HQ4X_PIXEL03_11
            HQ4X_PIXEL10_21
            HQ4X_PIXEL11_70
          }
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 124:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 203:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 62:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 211:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 118:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 217:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 110:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 155:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 188:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 185:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 61:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 157:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 103:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 227:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 230:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 199:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 220:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          break;
        }
        case 158:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 234:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 242:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          break;
        }
        case 59:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL11_0
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 121:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 87:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 79:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 122:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 94:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL12_0
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 218:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          break;
        }
        case 91:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL11_0
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 229:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 167:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 173:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 181:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 186:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 115:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          break;
        }
        case 93:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 206:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 205:
        case 201:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_10
            HQ4X_PIXEL21_30
            HQ4X_PIXEL30_80
            HQ4X_PIXEL31_10
          }
          else
          {
            HQ4X_PIXEL20_12
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_20
            HQ4X_PIXEL31_11
          }
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 174:
        case 46:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_80
            HQ4X_PIXEL01_10
            HQ4X_PIXEL10_10
            HQ4X_PIXEL11_30
          }
          else
          {
            HQ4X_PIXEL00_20
            HQ4X_PIXEL01_12
            HQ4X_PIXEL10_11
            HQ4X_PIXEL11_0
          }
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 179:
        case 147:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_10
            HQ4X_PIXEL03_80
            HQ4X_PIXEL12_30
            HQ4X_PIXEL13_10
          }
          else
          {
            HQ4X_PIXEL02_11
            HQ4X_PIXEL03_20
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_12
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 117:
        case 116:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_30
            HQ4X_PIXEL23_10
            HQ4X_PIXEL32_10
            HQ4X_PIXEL33_80
          }
          else
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_11
            HQ4X_PIXEL32_12
            HQ4X_PIXEL33_20
          }
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          break;
        }
        case 189:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 231:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 126:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 219:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 125:
        {
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL00_82
            HQ4X_PIXEL10_32
            HQ4X_PIXEL20_0
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL00_11
            HQ4X_PIXEL10_13
            HQ4X_PIXEL20_83
            HQ4X_PIXEL21_70
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_21
          }
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 221:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL03_81
            HQ4X_PIXEL13_31
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL03_12
            HQ4X_PIXEL13_14
            HQ4X_PIXEL22_70
            HQ4X_PIXEL23_83
            HQ4X_PIXEL32_21
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 207:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL02_32
            HQ4X_PIXEL03_82
            HQ4X_PIXEL10_0
            HQ4X_PIXEL11_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_83
            HQ4X_PIXEL02_13
            HQ4X_PIXEL03_11
            HQ4X_PIXEL10_21
            HQ4X_PIXEL11_70
          }
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 238:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL21_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
            HQ4X_PIXEL32_31
            HQ4X_PIXEL33_81
          }
          else
          {
            HQ4X_PIXEL20_21
            HQ4X_PIXEL21_70
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_83
            HQ4X_PIXEL32_14
            HQ4X_PIXEL33_12
          }
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          break;
        }
        case 190:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_0
            HQ4X_PIXEL23_32
            HQ4X_PIXEL33_82
          }
          else
          {
            HQ4X_PIXEL02_21
            HQ4X_PIXEL03_50
            HQ4X_PIXEL12_70
            HQ4X_PIXEL13_83
            HQ4X_PIXEL23_13
            HQ4X_PIXEL33_11
          }
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          break;
        }
        case 187:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
            HQ4X_PIXEL11_0
            HQ4X_PIXEL20_31
            HQ4X_PIXEL30_81
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_21
            HQ4X_PIXEL10_83
            HQ4X_PIXEL11_70
            HQ4X_PIXEL20_14
            HQ4X_PIXEL30_12
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 243:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL22_0
            HQ4X_PIXEL23_0
            HQ4X_PIXEL30_82
            HQ4X_PIXEL31_32
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL22_70
            HQ4X_PIXEL23_21
            HQ4X_PIXEL30_11
            HQ4X_PIXEL31_13
            HQ4X_PIXEL32_83
            HQ4X_PIXEL33_50
          }
          break;
        }
        case 119:
        {
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL00_81
            HQ4X_PIXEL01_31
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL12_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL00_12
            HQ4X_PIXEL01_14
            HQ4X_PIXEL02_83
            HQ4X_PIXEL03_50
            HQ4X_PIXEL12_70
            HQ4X_PIXEL13_21
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 237:
        case 233:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_60
          HQ4X_PIXEL03_20
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_70
          HQ4X_PIXEL13_60
          HQ4X_PIXEL20_0
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL30_0
          }
          else
          {
            HQ4X_PIXEL30_20
          }
          HQ4X_PIXEL31_0
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 175:
        case 47:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
          }
          else
          {
            HQ4X_PIXEL00_20
          }
          HQ4X_PIXEL01_0
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_0
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_70
          HQ4X_PIXEL23_60
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_60
          HQ4X_PIXEL33_20
          break;
        }
        case 183:
        case 151:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL03_0
          }
          else
          {
            HQ4X_PIXEL03_20
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_0
          HQ4X_PIXEL13_0
          HQ4X_PIXEL20_60
          HQ4X_PIXEL21_70
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_20
          HQ4X_PIXEL31_60
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 245:
        case 244:
        {
          HQ4X_PIXEL00_20
          HQ4X_PIXEL01_60
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_60
          HQ4X_PIXEL11_70
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_0
          HQ4X_PIXEL23_0
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 250:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          break;
        }
        case 123:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 95:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 222:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 252:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_61
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          HQ4X_PIXEL23_0
          HQ4X_PIXEL32_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 249:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_61
          HQ4X_PIXEL03_80
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_0
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL30_0
          }
          else
          {
            HQ4X_PIXEL30_20
          }
          HQ4X_PIXEL31_0
          break;
        }
        case 235:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_61
          HQ4X_PIXEL20_0
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL30_0
          }
          else
          {
            HQ4X_PIXEL30_20
          }
          HQ4X_PIXEL31_0
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 111:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
          }
          else
          {
            HQ4X_PIXEL00_20
          }
          HQ4X_PIXEL01_0
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_0
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_61
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 63:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
          }
          else
          {
            HQ4X_PIXEL00_20
          }
          HQ4X_PIXEL01_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_0
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_61
          HQ4X_PIXEL33_80
          break;
        }
        case 159:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL03_0
          }
          else
          {
            HQ4X_PIXEL03_20
          }
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          HQ4X_PIXEL13_0
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_61
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 215:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL03_0
          }
          else
          {
            HQ4X_PIXEL03_20
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_0
          HQ4X_PIXEL13_0
          HQ4X_PIXEL20_61
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 246:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_61
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_0
          HQ4X_PIXEL23_0
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 254:
        {
          HQ4X_PIXEL00_80
          HQ4X_PIXEL01_10
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_10
          HQ4X_PIXEL11_30
          HQ4X_PIXEL12_0
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          HQ4X_PIXEL23_0
          HQ4X_PIXEL32_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 253:
        {
          HQ4X_PIXEL00_82
          HQ4X_PIXEL01_82
          HQ4X_PIXEL02_81
          HQ4X_PIXEL03_81
          HQ4X_PIXEL10_32
          HQ4X_PIXEL11_32
          HQ4X_PIXEL12_31
          HQ4X_PIXEL13_31
          HQ4X_PIXEL20_0
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          HQ4X_PIXEL23_0
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL30_0
          }
          else
          {
            HQ4X_PIXEL30_20
          }
          HQ4X_PIXEL31_0
          HQ4X_PIXEL32_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 251:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_10
          HQ4X_PIXEL03_80
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_30
          HQ4X_PIXEL13_10
          HQ4X_PIXEL20_0
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL30_0
          }
          else
          {
            HQ4X_PIXEL30_20
          }
          HQ4X_PIXEL31_0
          break;
        }
        case 239:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
          }
          else
          {
            HQ4X_PIXEL00_20
          }
          HQ4X_PIXEL01_0
          HQ4X_PIXEL02_32
          HQ4X_PIXEL03_82
          HQ4X_PIXEL10_0
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_32
          HQ4X_PIXEL13_82
          HQ4X_PIXEL20_0
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_31
          HQ4X_PIXEL23_81
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL30_0
          }
          else
          {
            HQ4X_PIXEL30_20
          }
          HQ4X_PIXEL31_0
          HQ4X_PIXEL32_31
          HQ4X_PIXEL33_81
          break;
        }
        case 127:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
          }
          else
          {
            HQ4X_PIXEL00_20
          }
          HQ4X_PIXEL01_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL02_0
            HQ4X_PIXEL03_0
            HQ4X_PIXEL13_0
          }
          else
          {
            HQ4X_PIXEL02_50
            HQ4X_PIXEL03_50
            HQ4X_PIXEL13_50
          }
          HQ4X_PIXEL10_0
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL20_0
            HQ4X_PIXEL30_0
            HQ4X_PIXEL31_0
          }
          else
          {
            HQ4X_PIXEL20_50
            HQ4X_PIXEL30_50
            HQ4X_PIXEL31_50
          }
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_30
          HQ4X_PIXEL23_10
          HQ4X_PIXEL32_10
          HQ4X_PIXEL33_80
          break;
        }
        case 191:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
          }
          else
          {
            HQ4X_PIXEL00_20
          }
          HQ4X_PIXEL01_0
          HQ4X_PIXEL02_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL03_0
          }
          else
          {
            HQ4X_PIXEL03_20
          }
          HQ4X_PIXEL10_0
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          HQ4X_PIXEL13_0
          HQ4X_PIXEL20_31
          HQ4X_PIXEL21_31
          HQ4X_PIXEL22_32
          HQ4X_PIXEL23_32
          HQ4X_PIXEL30_81
          HQ4X_PIXEL31_81
          HQ4X_PIXEL32_82
          HQ4X_PIXEL33_82
          break;
        }
        case 223:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
            HQ4X_PIXEL01_0
            HQ4X_PIXEL10_0
          }
          else
          {
            HQ4X_PIXEL00_50
            HQ4X_PIXEL01_50
            HQ4X_PIXEL10_50
          }
          HQ4X_PIXEL02_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL03_0
          }
          else
          {
            HQ4X_PIXEL03_20
          }
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          HQ4X_PIXEL13_0
          HQ4X_PIXEL20_10
          HQ4X_PIXEL21_30
          HQ4X_PIXEL22_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL23_0
            HQ4X_PIXEL32_0
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL23_50
            HQ4X_PIXEL32_50
            HQ4X_PIXEL33_50
          }
          HQ4X_PIXEL30_80
          HQ4X_PIXEL31_10
          break;
        }
        case 247:
        {
          HQ4X_PIXEL00_81
          HQ4X_PIXEL01_31
          HQ4X_PIXEL02_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL03_0
          }
          else
          {
            HQ4X_PIXEL03_20
          }
          HQ4X_PIXEL10_81
          HQ4X_PIXEL11_31
          HQ4X_PIXEL12_0
          HQ4X_PIXEL13_0
          HQ4X_PIXEL20_82
          HQ4X_PIXEL21_32
          HQ4X_PIXEL22_0
          HQ4X_PIXEL23_0
          HQ4X_PIXEL30_82
          HQ4X_PIXEL31_32
          HQ4X_PIXEL32_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL33_20
          }
          break;
        }
        case 255:
        {
          if (Diff(w[4], w[2]))
          {
            HQ4X_PIXEL00_0
          }
          else
          {
            HQ4X_PIXEL00_20
          }
          HQ4X_PIXEL01_0
          HQ4X_PIXEL02_0
          if (Diff(w[2], w[6]))
          {
            HQ4X_PIXEL03_0
          }
          else
          {
            HQ4X_PIXEL03_20
          }
          HQ4X_PIXEL10_0
          HQ4X_PIXEL11_0
          HQ4X_PIXEL12_0
          HQ4X_PIXEL13_0
          HQ4X_PIXEL20_0
          HQ4X_PIXEL21_0
          HQ4X_PIXEL22_0
          HQ4X_PIXEL23_0
          if (Diff(w[8], w[4]))
          {
            HQ4X_PIXEL30_0
          }
          else
          {
            HQ4X_PIXEL30_20
          }
          HQ4X_PIXEL31_0
          HQ4X_PIXEL32_0
          if (Diff(w[6], w[8]))
          {
            HQ4X_PIXEL33_0
          }
          else
          {
            HQ4X_PIXEL33_20
          }
          break;
        }
      }
      pIn+=2;
      pOut+=16;
    }
    pOut+=BpL;
    pOut+=BpL;
    pOut+=BpL;
  }
}
