// simple but powerful trace logger from bsnes

#ifndef SNES9X_DISASM_H_INCLUDED
#define SNES9X_DISASM_H_INCLUDED

#include <stdio.h>

extern FILE *S9xTraceLogStream;

void S9xTraceCPUToBuf(char *output);
void S9xTraceCPU();

#endif // !SNES9X_DISASM_H_INCLUDED
