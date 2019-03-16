/* FakeNES - A free, portable, Open Source NES emulator.

   netplay.h: Declarations for the NetPlay engine.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef NETPLAY_H_INCLUDED
#define NETPLAY_H_INCLUDED
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

enum
{
   NETPLAY_DEFAULT_PORT = 0x2A03,
   NETPLAY_LATENCY      = 12        /* In frames. */
};

ENUM netplay_mode;

int netplay_init (void);
void netplay_exit (void);
BOOL netplay_open_server (int port);
BOOL netplay_open_client (const CHAR *host, int port);
void netplay_close (void);
void netplay_process (void);
void netplay_set_nickname (const UCHAR *nickname);
void netplay_send_message (const UCHAR *message);
void netplay_enumerate_clients (UCHAR *buffer, unsigned size);
void netplay_enumerate_chat (UCHAR *buffer, unsigned size);

enum
{
   NETPLAY_MODE_INACTIVE = 0,
   NETPLAY_MODE_SERVER_OPEN,
   NETPLAY_MODE_SERVER_CLOSED,
   NETPLAY_MODE_CLIENT
};

enum
{
   NETPLAY_PACKET_NULL = 0,   /* Keep alive. */
   NETPLAY_PACKET_START,      /* Start emulation. */
   NETPLAY_PACKET_STOP,       /* Stop(pause) emulation. */
   NETPLAY_PACKET_CHAT,       /* Chat message(UTF8). */
   NETPLAY_PACKET_PAD_DATA    /* Multiple frames of controller data. */
};

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !NETPLAY_H_INCLUDED */
