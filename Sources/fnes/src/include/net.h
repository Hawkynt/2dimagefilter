/* FakeNES - A free, portable, Open Source NES emulator.

   net.h: Declarations for the network interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#ifndef NET_H_INCLUDED
#define NET_H_INCLUDED
#include <allegro.h>
#include "common.h"
#include "types.h"
#ifdef __cplusplus
extern "C" {
#endif

enum
{
   NET_MAX_CLIENTS = 4
};

typedef struct _NET_CLIENT
{
   BOOL active;      /* Whether slot is occupied or not. */
   USTRING nickname; /* Nickname, UTF8.  */
   unsigned timeout; /* Time elapsed since last data transfer (in
                        net_process() ticks). */

} NET_CLIENT;

NET_CLIENT net_clients[NET_MAX_CLIENTS];

ENUM net_mode;

int net_init (void);
void net_exit (void);
int net_open (int port);
void net_close (void);
int net_listen (void);
int net_connect (const CHAR *host, int port);
void net_process (void);
unsigned net_get_packet (ENUM client_id, void *buffer, unsigned size);
unsigned net_send_packet (FLAGS flags, void *buffer, unsigned size);

enum
{            
   NET_MODE_INACTIVE,
   NET_MODE_SERVER,
   NET_MODE_CLIENT
};

enum
{
   NET_LOCAL_CLIENT        = 0,
   NET_FIRST_REMOTE_CLIENT = ( NET_LOCAL_CLIENT + 1 )
};

typedef struct _NET_PACKET_HEADER
{
   UINT16 size;
   UINT8 flags;

} NET_PACKET_HEADER;

enum
{
   NET_PACKET_FLAG_BROADCAST = (1 << 0)
};

enum
{
   /* (16+8)/8 */
   NET_PACKET_HEADER_SIZE = 3
};

enum
{
   NET_MAX_PACKET_SIZE_SEND    = 2048,
   NET_MAX_PACKET_SIZE_RECIEVE = ( NET_MAX_PACKET_SIZE_SEND + NET_PACKET_HEADER_SIZE ),
   NET_MAX_PACKET_SIZE_ANY     = NET_MAX_PACKET_SIZE_RECIEVE
};

#ifdef __cplusplus
}
#endif   /* __cplusplus */
#endif   /* !NET_H_INCLUDED */
