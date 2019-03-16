/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   vrc6.cpp: VRC6 sound hardware emulation by randilyn. */

#include "../include/common.h"
#include "sound.hpp"
#include "vrc6.hpp"
             
namespace Sound {
namespace VRC6 {

void Square::reset (void)
{
   enabled = false;
   timer = 0;
   period = 0;
   volume = 0;
   duty = 0;
   force = false;

   step = 0;
   output = 0;
}

void Square::write (uint16 address, uint8 value)
{
   switch (address)
   {
      case 0x9000:
      case 0xA000:
      {
         regs[0] = value;

         volume = (value & 0x0f);
         duty = ((value >> 4) & 0x07);
         force = (value & 0x80);

         if (force)
            output = volume;

         break;
      }

      case 0x9001:
      case 0xA001:
      {
         regs[1] = value;

         period &= ~0xff;
         period |= value;

         break;
      }

      case 0x9002:
      case 0xA002:
      {
         regs[2] = value;

         enabled = (value & 0x80);
         if (!enabled)
            output = 0;

         period &= ~0xf00;
         period |= ((value & 0x0f) << 8);

         break;
      }

      default:
         break;
   }
}

void Square::process (cpu_time_t cycles)
{
   if (timer > 0)
   {
      timer -= cycles;
      if (timer > 0)
         return;
   }

   timer += (period + 1);

   if (enabled &&
       (force || (step <= duty)))
      output = volume;
   else
      output = 0;

   step++;
   if (step > 15)
      step = 0;
}

void Square::save (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   for (int index = 0; index < 3; index++)
      pack_putc (regs[index], file);

   pack_iputw (timer, file);
   pack_putc (step, file);
   pack_putc (output, file);
}

void Square::load (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   for (int index = 0; index < 3; index++)
      write ((0x9000 + index), pack_getc (file));  // should work for both

   timer = pack_igetw (file);
   step = pack_getc (file);
   output = pack_getc (file);
}

void Saw::reset (void)
{
   enabled = false;
   timer = 0;
   period = 0;
   rate = 0;

   step = 0;
   volume = 0;
   output = 0;
}

void Saw::write (uint16 address, uint8 value)
{
   switch (address)
   {
      case 0xB000:
      {
         regs[0] = value;

         rate = (value & 0x3f);
         break;
      }

      case 0xB001:
      {
         regs[1] = value;

         period &= ~0xff;
         period |= value;

         break;
      }

      case 0xB002:
      {
         regs[2] = value;

         enabled = (value & 0x80);
         if (!enabled)
            output = 0;

         period &= ~0xf00;
         period |= ((value & 0x0f) << 8);

         break;
      }

      default:
         break;
   }
}

void Saw::process (cpu_time_t cycles)
{
   if (timer > 0)
   {
      timer -= cycles;
      if (timer > 0)
         return;
   }

   timer += ((period + 1) << 1);

   if (step == 6)
   {
      step = 0;
      volume = 0;

      return;
   }
   else
      step++;

   if (!enabled)
   {
      output = 0;
      return;
   }

   volume += rate;

   output = (volume >> 3);
}            

void Saw::save (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   for (int index = 0; index < 3; index++)
      pack_putc (regs[index], file);

   pack_iputw (timer, file);
   pack_putc (step, file);
   pack_putc (volume, file);
   pack_putc (output, file);
}

void Saw::load (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   for (int index = 0; index < 3; index++)
      write ((0xB000 + index), pack_getc (file));

   timer = pack_igetw (file);
   step = pack_getc (file);
   volume = pack_getc (file);
   output = pack_getc (file);
}

void Interface::reset (void)
{
   square1.reset ();
   square2.reset ();
   saw.reset ();

   output = 0;
}

void Interface::write (uint16 address, uint8 value)
{
   switch (address)
   {
      case 0x9000:
      case 0x9001:
      case 0x9002:
      {
         square1.write (address, value);
         break;
      }

      case 0xA000:
      case 0xA001:
      case 0xA002:
      {
         square2.write (address, value);
         break;
      }

      case 0xB000:
      case 0xB001:
      case 0xB002:
      {
         saw.write (address, value);
         break;
      }

      default:
         break;
   }
}

void Interface::process (cpu_time_t cycles)
{
   if (apu_options.enable_extra_1)
      square1.process (cycles);
   if (apu_options.enable_extra_2)
      square2.process (cycles);

   if (apu_options.enable_extra_3)
      saw.process (cycles);
}

void Interface::save (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   square1.save (file, version);
   square2.save (file, version);
   saw.save (file, version);
}

void Interface::load (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   square1.load (file, version);
   square2.load (file, version);
   saw.load (file, version);
}

void Interface::mix (void)
{
   uint8 total = 0;

   if (apu_options.enable_extra_1)
      total += square1.output;
   if (apu_options.enable_extra_2)
      total += square2.output;

   if (apu_options.enable_extra_3)
      total += saw.output;

   output = (total / 61.0);
}

}  /* namespace VRC6 */
}  /* namespace Sound */
