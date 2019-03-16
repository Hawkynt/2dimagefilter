/* FakeNES - A free, portable, Open Source NES emulator.

   netplay.c: Implementation of the NetPlay engine.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#include "common.h"
#include "data.h"
#include "debug.h"
#include "net.h"
#include "netplay.h"
#include "shared/bufferfile.h"
#include "timing.h"
#include "types.h"
#include "video.h"

ENUM netplay_mode = NETPLAY_MODE_INACTIVE;

static void parse_packet (PACKFILE *file);

/* Hack for lobby chat - this needs to be replaced with something better. */
static USTRING netplay_chat_buffer;

int netplay_init (void)
{
   USTRING_CLEAR(netplay_chat_buffer);

   return (0);
}

void netplay_exit (void)
{
   if (netplay_mode != NETPLAY_MODE_INACTIVE)
      netplay_close ();
}

BOOL netplay_open_server (int port)
{
   if (netplay_mode != NETPLAY_MODE_INACTIVE)
      return (FALSE);

   if (net_open (port) > 0)
      return (FALSE);

   netplay_mode = NETPLAY_MODE_SERVER_OPEN;

   /* Call it once just to set everything up. */
   net_listen ();

   return (TRUE);
}

BOOL netplay_open_client (const CHAR *host, int port)
{
   if (netplay_mode != NETPLAY_MODE_INACTIVE)
      return (FALSE);

   if (net_open (0) > 0)
      return (FALSE);

   if (!net_connect (host, port))
   {
      net_close ();
      return (FALSE);
   }

   netplay_mode = NETPLAY_MODE_CLIENT;

   return (TRUE);
}

void netplay_close (void)
{
   if (netplay_mode == NETPLAY_MODE_INACTIVE)
      return;

   net_close ();

   netplay_mode = NETPLAY_MODE_INACTIVE;
}

void netplay_process (void)
{
   static int wait_frames = 0;
   int index;

   if (netplay_mode == NETPLAY_MODE_INACTIVE)
      return;

   /* Sync state. */
   net_process ();

   switch (netplay_mode)
   {
      case NETPLAY_MODE_SERVER_OPEN:
      {
         /* Listen for incoming connections. */
         net_listen ();

         break;
      }

      default:
         break;
   }

   /* Send empty packets so that the connection stays alive. */
   if (wait_frames > 0)
      wait_frames--;

   if (wait_frames == 0)
   {
      UINT8 type;

      type = NETPLAY_PACKET_NULL;

      net_send_packet (NULL, &type, sizeof(type));
   
      /* 5 times per second should be plenty... maybe TOO much. */
      /* I guess this probably won't work so well in the GUI... oh well. */
      wait_frames = ROUND(timing_get_speed () / 5.0);
   }

   /* Check for incoming packets. */
   for (index = 0; index < NET_MAX_CLIENTS; index++)
   {
      /* Recieve buffer. */
      UINT8 buffer[NET_MAX_PACKET_SIZE_RECIEVE];
      unsigned size;

      /* Grab next packet. */
      size = net_get_packet (index, buffer, sizeof(buffer));
      while (size > 0)
      {
         PACKFILE *file;

         /* Open buffer file. */
         file = BufferFile_open ();
         if (!file)
         {
            WARN_GENERIC();
            continue;
         }

         /* Copy packet to buffer file. */
         pack_fwrite (buffer, size, file);

         /* Parse it. */
         parse_packet (file);

         /* Close buffer file */
         pack_fclose (file);

         /* Grab next packet. */
         size = net_get_packet (index, buffer, sizeof(buffer));
      }
   }
}

void netplay_set_nickname (const UCHAR *nickname)
{
   /* This function sets the nickname for client #0, which is always the
      local "player". */

   NET_CLIENT *client = &net_clients[NET_LOCAL_CLIENT];

   RT_ASSERT(nickname);

   USTRING_CLEAR(client->nickname);
   ustrncat (client->nickname, nickname, (USTRING_SIZE - 1));
}

void netplay_send_message (const UCHAR *message)
{
   /* This function sends a Unicode chat message over the network from
      client #0, prefixed with the nickname set by netplay_set_nickname().
      */

   NET_CLIENT *client = &net_clients[NET_LOCAL_CLIENT];
   USTRING text;
   PACKFILE *file;
   UINT16 length;
   UINT8 *buffer;
   long size;

   RT_ASSERT(message);

   /* Prefix message with nickname. */
   USTRING_CLEAR(text);
   uszprintf (text, (sizeof (text) - 1), "<%s> %s", client->nickname, message);

   /* Build packet. */
   file = BufferFile_open ();
   if (!file)
   {
      WARN_GENERIC();
      return;
   }

   pack_putc (NETPLAY_PACKET_CHAT, file);

   length = MIN( 65535, ustrsize (text) );
                                    
   pack_iputw (length, file);
   pack_fwrite (text, length, file);

   /* Send packet. */
   BufferFile_get_buffer (file, &buffer, &size);
   net_send_packet (NET_PACKET_FLAG_BROADCAST, buffer, size);

   pack_fclose (file);
}

void netplay_enumerate_clients (UCHAR *buffer, unsigned size)
{
   /* This function stores a list of all clients' nicknames in 'buffer',
      suitable for display in a text box. */

   /* TODO: Fix this to work on the client side by asking the server for
      an enumeration.  Right now, it only works for the server. */

   int index;

   RT_ASSERT(buffer);

   USTRING_CLEAR_SIZE(buffer, size);

   for (index = 0; index < NET_MAX_CLIENTS; index++)
   {
      NET_CLIENT *client = &net_clients[index];

      if (!client->active)
         continue;

      ustrncat (buffer, "\n", (size - 1));
      ustrncat (buffer, client->nickname, (size - 1));
   }
}

void netplay_enumerate_chat (UCHAR *buffer, unsigned size)
{
   RT_ASSERT(buffer);

   USTRING_CLEAR_SIZE(buffer, size);
   ustrncat (buffer, netplay_chat_buffer, (size - 1));
}

/* ---- Private functions ---- */

static void parse_packet (PACKFILE *file)
{
   UINT8 type;

   RT_ASSERT(file);

   /* Skip header. */
   pack_fseek (file, NET_PACKET_HEADER_SIZE);

   /* Fetch packet type. */
   type = pack_getc (file);

   switch (type)
   {
      case NETPLAY_PACKET_NULL:
         break;

      case NETPLAY_PACKET_CHAT:
      {
         unsigned length;
         USTRING text;

         /* Determine how many bytes to read. */
         length = pack_igetw (file);
         if (length == 0)
         {
            WARN("Recieved empty chat packet");
            return;
         }

         /* Load UTF8 string. */
         USTRING_CLEAR(text);
         pack_fread (text, MIN( length, (USTRING_SIZE - 1) ), file);

         /* Display it. */
         video_message (text);
         video_message_duration = 5000;

         /* Play sound. */
         /* TODO: Support OpenAL by piping this through an audiolib wrapper. */
         play_sample (DATA_TO_SAMPLE(CHAT_RECIEVE_SOUND), 255, 128, 1000, FALSE);

         /* Append it to the buffer. */
         ustrncat (netplay_chat_buffer, "\n", (sizeof(netplay_chat_buffer) - 1));
         ustrncat (netplay_chat_buffer, text, (sizeof(netplay_chat_buffer) - 1));

         break;
      }

      default:
      {
         WARN_GENERIC();
         break;
      }
   }
}
