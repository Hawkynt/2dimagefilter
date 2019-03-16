#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <sys/farptr.h>
#include <string.h>

#include "allegro.h"

//declare some stuff
#define uint32 unsigned long
#define int32 signed long
#define uint16 unsigned short
#define uint8 unsigned char

extern void Super2xSaI(uint8 *srcPtr, uint32 srcPitch,
	     uint8 *deltaPtr,
         BITMAP *dstBitmap, int width, int height);
extern void _2xSaI(uint8 *srcPtr, uint32 srcPitch,
	     uint8 *deltaPtr,
         BITMAP *dstBitmap, int width, int height);
extern void SuperEagle(uint8 *srcPtr, uint32 srcPitch,
	     uint8 *deltaPtr,
         BITMAP *dstBitmap, int width, int height);
extern void Scale_2xSaI(uint8 *srcPtr, uint32 srcPitch,
	     uint8 *deltaPtr,
         BITMAP *dstBitmap, int width, int height);

#define    _2XSAI 1
#define    SUPEREAGLE 2
#define    SUPER2XSAI 3
#define    SCALE2XSAI 4

int main(int argc, char *argv[])
{
   BITMAP *srcBitmap, *dstBitmap, *loadBitmap;
   PALETTE pal;
   char *src_filename = NULL;
   char *dst_filename = NULL;
   uint8 *deltaPtr = NULL;
   int engine = _2XSAI;
   int dstwidth = 640;
   int dstheight = 480;

   clrscr();
   printf ("\n2xSaI Utility v0.7        Copyright (c) Derek Liauw Kie Fa, 1999");
   printf ("\nComments, Suggestions, Questions, Bugs etc.: derek-liauw@usa.net");
   printf ("\nhttp://members.xoom.com/derek_liauw/");
   printf ("\nThis program is free for non-commercial use.");
   printf ("\nSee 2xSai.txt for more info");

   if (argc < 3)
   {
       printf ("\n\nUsage: 2xSaI <input.bmp> <output.bmp> -e <Enginenumber> -size <width> <height>");
       printf ("\n<input.bmp> and <output.bmp> must be the first two parameters!");
       printf ("\n       <input.bmp> must be an 8 or 24 bit PCX/BMP file.");
       printf ("\n       <ouput.bmp> will be a 24 bit PCX/BMP file.");
       printf ("\n\nEnginenumbers:");
       printf ("\n       1: 2xSaI [default]");
       printf ("\n       2: SuperEagle");
       printf ("\n       3: Super2xSaI");
       printf ("\n       4: Scale2xSaI");
       printf ("\n\nwidth and height");
       printf ("\n       Size of destination Bitmap. Used by Scale2xSaI");
       printf ("\n       Default size 640x480");

       return 1;
   }


   src_filename = argv [1];  //first parameter is source file
   dst_filename = argv [2];  //second parameter is destination file


   for (int i = 0; i < argc; i++)
   {
       if (strcasecmp(argv[i], "-e") == 0)
       {
           if (i+1 < argc)
               engine = atoi (argv[++i]);
       }
       else
       if (strcasecmp(argv[i], "-size") == 0)
       {
           if (i+2 < argc)
           {
               dstwidth = atoi (argv[++i]);
               dstheight = atoi (argv[++i]);
           }
       }
   }




   //initialize allegro
   allegro_init();

   //do not delete this line !!! Allegro bitmap conversion routines won't work.
   set_color_depth(16);


   //load the bitmap from file
   loadBitmap = load_bitmap(src_filename, pal);
   if (!loadBitmap)
   {
      allegro_exit();
      printf("\nError reading source file");
      return 2;
   }


   if (engine != SCALE2XSAI)
   {
      dstwidth = 2*loadBitmap->w;
      dstheight = 2*loadBitmap->h;
   }



   //memory for source bitmap
   srcBitmap = create_bitmap_ex(16, loadBitmap->w, loadBitmap->h+1);
   if (!srcBitmap)
   {
      allegro_exit();
      printf("\nOut of Memory?");
      return 3;
   }

   //memory for destination bitmap. Dimensions are 2 times bigger than source bitmap
   dstBitmap = create_bitmap_ex(16, dstwidth, dstheight);
   if (!dstBitmap)
   {
      allegro_exit();
      printf("\nOut of Memory?");
      return 4;
   }

   deltaPtr = new unsigned char[loadBitmap->w * loadBitmap->h * 2];
   if (!dstBitmap)
   {
      allegro_exit();
      printf("\nOut of Memory?");
      return 5;
   }

   memset (deltaPtr, 0, loadBitmap->w * loadBitmap->h * 2);
   clear(srcBitmap);
   clear(dstBitmap);


   //convert loadbitmap to 16-bit and output on source bitmap.
   blit(loadBitmap, srcBitmap, 0, 0, 0, 0, loadBitmap->w, loadBitmap->h);



   switch (engine)
   {
   case _2XSAI:
   //Process the bitmap
       _2xSaI((unsigned char*) srcBitmap->dat, srcBitmap->w*2,
	     deltaPtr,
             dstBitmap, srcBitmap->w, srcBitmap->h);
       break;
   case SUPEREAGLE:
   //Process the bitmap
       SuperEagle((unsigned char*) srcBitmap->dat, srcBitmap->w*2,
	     deltaPtr,
             dstBitmap, srcBitmap->w, srcBitmap->h);
       break;
   case SUPER2XSAI:
   //Process the bitmap
       Super2xSaI((unsigned char*) srcBitmap->dat, srcBitmap->w*2,
	     deltaPtr,
             dstBitmap, srcBitmap->w, srcBitmap->h);
       break;
   default:
   case SCALE2XSAI:
   //Process the bitmap
       Scale_2xSaI((unsigned char*) srcBitmap->dat, srcBitmap->w*2,
	     deltaPtr,
             dstBitmap, srcBitmap->w, srcBitmap->h);
       break;
   }



   //save the bitmap
   if (save_bitmap(dst_filename, dstBitmap, pal) != 0)
      printf ("\nError writing file");
   else
   {
      printf ("\nFile successfully written");
      printf ("\nEngine used: %d", engine);

   }

   destroy_bitmap(loadBitmap);
   destroy_bitmap(dstBitmap);
   destroy_bitmap(srcBitmap);

   return 0;
}
