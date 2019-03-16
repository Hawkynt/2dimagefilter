#include <stdio.h>

int main (void)
{
   unsigned test;

   /* Byte order. */
   test = 1;
   if ((*(unsigned char *)&test) == 1)
      printf ("#define LSB_FIRST\n");

   /* Data type sizes. */
   printf ("#define SIZEOF_SHORT %d\n", sizeof (short));
   printf ("#define SIZEOF_INT   %d\n", sizeof (int));
   printf ("#define SIZEOF_LONG  %d\n", sizeof (long));

   return (0);
}
