/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   mmc5.cpp: MMC5 sound hardware emulation by randilyn. */

#include "../include/common.h"
#include "sound.hpp"
#include "mmc5.hpp"
             
namespace Sound {
namespace MMC5 {

static const uint8 length_lut[32] = {
   0x0A, 0x14, 0x28, 0x50, 0xA0, 0x3C, 0x0E, 0x1A, 0x0C, 0x18, 0x30, 0x60,
   0xC0, 0x48, 0x10, 0x20, 0xFE, 0x02, 0x04, 0x06, 0x08, 0x0A, 0x0C, 0x0E,
   0x10, 0x12, 0x14, 0x16, 0x18, 0x1A, 0x1C, 0x1E
};

/* Pulse sequences for each step 0-7 of each duty cycle 0-3 on the square
   wave channels. */
static const uint8 square_duty_lut[4][8] = {
   { 0x0, 0xF, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 },
   { 0x0, 0xF, 0xF, 0x0, 0x0, 0x0, 0x0, 0x0 },
   { 0x0, 0xF, 0xF, 0xF, 0xF, 0x0, 0x0, 0x0 },
   { 0xF, 0x0, 0x0, 0xF, 0xF, 0xF, 0xF, 0xF }
};

void Square::reset (void)
{
   timer = 0;
   period = 0;
   length = 0;
   halt = false;
   volume = 0;
   duty = 0;
   clamped = false;

   envelope.timer = 0;
   envelope.period = 0;
   envelope.fixed = false;
   envelope.fixed_volume = 0;
   envelope.reset = false;

   envelope.counter = 0;

   step = 0;
   output = 0;
}

void Square::write (uint16 address, uint8 value)
{
   switch (address)
   {
      case 0x5000:
      case 0x5004:
      {
         regs[0] = value;

         volume = (value & 0x0f);
         halt = (value & 0x20);
         duty = (value >> 6);

         envelope.period = ((value & 0x0f) + 1);
         envelope.fixed = (value & 0x10);
         envelope.fixed_volume = (value & 0x0f);

         break;        
      }

      case 0x5001:
      case 0x5005:
      {
         regs[1] = value;  // unused placeholder
         break;
      }

      case 0x5002:
      case 0x5006:
      {
         regs[2] = value;

         period &= ~0xff;
         period |= value;

         break;
      }

      case 0x5003:
      case 0x5007:
      {
         regs[3] = value;

         period &= ~0x700;
         period |= ((value & 0x07) << 8);

         if (!clamped)
            length = length_lut[(value >> 3)];

         // Reset envelope generator.
         envelope.reset = true;

         // Reset sequencer(?)
         step = 0;

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

   timer += ((period + 2) << 1);

   if (length > 0)
      output = (volume & square_duty_lut[duty][step]);
   else
      output = 0;

   step++;
   if (step > 7)
      step = 0;
}

void Square::save (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   for (int index = 0; index < 4; index++)
      pack_putc (regs[index], file);

   pack_iputw (timer, file);
   pack_putc (length, file);
   pack_putc (volume, file);
   pack_putc (step, file);
   pack_putc (output, file);

   pack_putc (envelope.timer, file);
   pack_putc ((envelope.reset ? 1 : 0), file);
   pack_putc (envelope.counter, file);
}

void Square::load (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   for (int index = 0; index < 4; index++)
      write ((0x5000 + index), pack_getc (file));  // should work for both

   timer = pack_igetw (file);
   length = pack_getc (file);
   volume = pack_getc (file);
   step = pack_getc (file);
   output = pack_getc (file);

   envelope.timer = pack_getc (file);
   envelope.reset = pack_getc (file);
   envelope.counter = pack_getc (file);
}

void Square::update_120hz (void)
{
   if ((length > 0) && !halt)
      length--;
}

void Square::update_240hz (void)
{
   if (envelope.reset)
   {
      envelope.timer = 0;
      envelope.counter = 0xF;

      envelope.reset = false;

      return;
   }

   if (envelope.timer > 0)
   {
      envelope.timer--;
      if (envelope.timer > 0)
         return;
   }

   envelope.timer += envelope.period;
                      
   if (envelope.counter > 0)
      envelope.counter--;
   else if (halt)
      envelope.counter = 0xF;

   if (envelope.fixed)
      volume = envelope.fixed_volume;
   else
      volume = envelope.counter;
}

void PCM::reset (void)
{
   output = 0;
}

void PCM::write (uint16 address, uint8 value)
{
   switch (address)
   {
      case 0x5011:
      {
         output = value;
         break;
      }

      default:
         break;
   }
}

void PCM::save (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   pack_putc (output, file);
}

void PCM::load (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   output = pack_getc (file);
}

void Interface::reset (void)
{
   square1.reset ();
   square2.reset ();
   pcm.reset ();

   timer = 0;
   flip = false;

   output = 0;
}

uint8 Interface::read (uint16 address)
{
   uint8 value = 0;

   switch (address)
   {
      case 0x5015:
      {
         if (square1.length > 0)
            value |= 0x01;
         if (square2.length > 0)
            value |= 0x02;

         break;
      }

      default:
         break;
   }

   return (value);
}

void Interface::write (uint16 address, uint8 value)
{
   switch (address)
   {
      case 0x5000:
      case 0x5001:
      case 0x5002:
      case 0x5003:
      {
         square1.write (address, value);
         break;
      }

      case 0x5004:
      case 0x5005:
      case 0x5006:
      case 0x5007:
      {
         square2.write (address, value);
         break;
      }

      case 0x5011:
      {
         pcm.write (address, value);
         break;
      }

      case 0x5015:
      {
         square1.clamped = (value & 0x01);
         if (square1.clamped)
            square1.length = 0;

         square2.clamped = (value & 0x02);
         if (square2.clamped)
            square2.length = 0;

         break;
      }

      default:
         break;
   }
}

void Interface::process (cpu_time_t cycles)
{
   if (timer > 0)
      timer -= cycles;
   if (timer <= 0)
   {
      // This should actually be 7457.5 - but close enough.
      // Effective rate: ~240Hz (239.996...)
      if (flip)
         timer += 7458;
      else
         timer += 7457;

      if (apu_options.enable_extra_1)
         square1.update_240hz ();
      if (apu_options.enable_extra_2)
         square2.update_240hz ();

      if (flip)
      {
         if (apu_options.enable_extra_1)
            square1.update_120hz ();
         if (apu_options.enable_extra_2)
            square2.update_120hz ();

         flip = false;
      }
      else
         flip = true;
   }

   if (apu_options.enable_extra_1)
      square1.process (cycles);
   if (apu_options.enable_extra_2)
      square2.process (cycles);
}

void Interface::save (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   pack_iputl (timer, file);
   pack_putc ((flip ? 1 : 0), file);

   square1.save (file, version);
   square2.save (file, version);
   pcm.save (file, version);
}

void Interface::load (PACKFILE *file, int version)
{
   RT_ASSERT(file);

   timer = pack_igetl (file);
   flip = pack_getc (file);

   square1.load (file, version);
   square2.load (file, version);
   pcm.load (file, version);
}

void Interface::mix (void)
{
   output = 0.0;

   if (apu_options.enable_extra_1)
      output += (square1.output / 15.0);
   if (apu_options.enable_extra_2)
      output += (square2.output / 15.0);

   if (apu_options.enable_extra_3)
      output += (pcm.output / 255.0);

   output /= 3.0;
}

}  /* namespace MMC5 */
}  /* namespace Sound */
