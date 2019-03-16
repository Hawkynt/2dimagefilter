/* FakeNES - A free, portable, Open Source NES emulator.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use.

   vrc6.cpp: VRC6 sound hardware emulation by randilyn. */

#ifndef _SOUND__VRC6_HPP                                
#define _SOUND__VRC6_HPP
#include "../include/common.h"
#include "sound.hpp"

namespace Sound {
namespace VRC6 {

class Interface;

class Square : public Channel {
protected:
   friend class Interface;

   void reset (void);
   void write (uint16 address, uint8 value);
   void process (cpu_time_t cycles);
   void save (PACKFILE *file, int version);
   void load (PACKFILE *file, int version);

   uint8 output;     // save

private:
   uint8 regs[3];    // save

   bool enabled;     // do not save
   int16 timer;      // save
   uint16 period;    // do not save
   uint8 volume;     // do not save
   uint8 duty;       // do not save
   bool force;       // do not save

   uint8 step;       // save
};

class Saw : public Channel {
protected:
   friend class Interface;

   void reset (void);
   void write (uint16 address, uint8 value);
   void process (cpu_time_t cycles);
   void save (PACKFILE *file, int version);
   void load (PACKFILE *file, int version);

   uint8 output;     // save

private:
   uint8 regs[3];    // save

   bool enabled;     // do not save
   int16 timer;      // save
   uint16 period;    // do not save
   uint8 rate;       // do not save

   uint8 step;       // save
   uint8 volume;     // save
};

class Interface : public Sound::Interface {
public:
   void reset (void);
   void write (uint16 address, uint8 value);
   void process (cpu_time_t cycles);
   void save (PACKFILE *file, int version);
   void load (PACKFILE *file, int version);
   void mix (void);

private:                    
   Square square1;
   Square square2;
   Saw saw;
};
                        
}  /* namespace VRC6 */
}  /* namespace Sound */

#endif   /* !_SOUND__VRC6_HPP */
