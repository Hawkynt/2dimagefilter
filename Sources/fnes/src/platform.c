/* FakeNES - A free, portable, Open Source NES emulator.

   platform.c: Implementation of the platform interface.

   Copyright (c) 2001-2006, FakeNES Team.
   This is free software.  See 'LICENSE' for details.
   You must read and accept the license prior to use. */

#include <allegro.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "common.h"
#include "log.h"
#include "platform.h"
#include "types.h"

#ifdef POSIX
#include <sys/stat.h>
#include <sys/types.h>
#include <dirent.h>
#include <errno.h>
extern int errno;
#endif

#ifdef POSIX

UINT8 * homedir = NIL;

UINT8 * logdir = NIL;

UINT8 * confdir = NIL;


static DIR * tmpdir = NIL;

#endif


UINT8 logfile [256];


int platform_init (void)
{
    UINT8 buffer [256];

    UINT8 buffer2 [256];


#ifdef POSIX

    /* by amit */


    /* Configuration directory checking */

    homedir = getenv ("HOME");


    if (homedir)
    {
        const UINT8 confdir_base [] = "/.fakenes";


        const UINT8 logdir_base [] = "/logs";


        confdir = ((UINT8 *) malloc (strlen (homedir) + sizeof (confdir_base)));


        if (confdir)
        {
            strcpy (confdir, homedir);

            strcat (confdir, confdir_base);


            logdir = ((UINT8 *) malloc (strlen (confdir) + sizeof (logdir_base)));


            if (logdir)
            {
                strcpy (logdir, confdir);

                strcat (logdir, logdir_base);


                memset (logfile, NIL, sizeof (logfile));


                strcat (logfile, logdir);

                strcat (logfile, "/messages");
            }
        }
        else
        {
            logdir = NIL;
        }


        if (! confdir)
        {
            fprintf (stderr, "Error when generating configuration path.\nConfiguration will not be saved.\n\n");
        }
        else
        {
            /* Just see if we can open the directory. */
              
            if (! (tmpdir = opendir (confdir)))
            {
                /* Directory doesn't exist, create it. */

                if (errno == ENOENT)
                {
                    if (mkdir (confdir, (S_IRWXU | S_IRGRP | S_IXGRP | S_IROTH | S_IXOTH)) == -1)
                    {
                        fprintf (stderr, "Error creating \"%s\".\nConfiguration will not be saved.\n\n", confdir);


                        free (confdir);

                        confdir = NIL;
                    }
                    else    /* mkdir was successful, make the log dir. */
                    {
                        /* Error checking for logdir happens later. */

                        mkdir (logdir, (S_IRWXU | S_IRGRP | S_IXGRP | S_IROTH | S_IXOTH));
                    }
                }
                else
                {
                    UINT8 errorbuf [300] = { NIL };


                    strcat (errorbuf, confdir);

                    perror (errorbuf);


                    fprintf (stderr, "%s.\nConfiguration will not be saved.\n\n", errorbuf);


                    free (confdir);

                    confdir = NIL;
                }
            }
            else
            {
                /* Close the directory we just opened. */

                closedir (tmpdir);
            }
        }

    }
    else
    {
        fprintf (stderr, "$HOME appears to not be set.\nConfiguration will not be saved.\n\n");
    }


    /* Load up the configuration file. */

    if (confdir)
    {
        const UINT8 conffile_base [] = "/config";


        UINT8 * conffile = malloc (strlen (confdir) + sizeof (conffile_base));


        strcpy (conffile, confdir);

        strcat (conffile, conffile_base);


        set_config_file (conffile);
    }
    else
    {
        set_config_file ("/dev/null");
    }


    /* Check the logs directory. */

    if (! logdir)
    {
        /* If we have a valid home directory, there was an error. */

        if (homedir)
        {
            fprintf (stderr, "Error when generating log path.\nLogs will not be saved.\n\n");
        }
    }
    else
    {
        if (! (tmpdir = opendir (logdir)))
        {
            if (errno == ENOENT)
            {
                if (mkdir (logdir, (S_IRWXU | S_IRGRP | S_IXGRP | S_IROTH | S_IXOTH)) == -1)
                {
                    fprintf (stderr, "Error creating \"%s\"\n\n", logdir);


                    free (logdir);

                    logdir = NIL;
                }
            }
            else
            {
                UINT8 errorbuf [300] = { NIL };


                strcat (errorbuf, confdir);

                perror (errorbuf);


                fprintf (stderr, "%s.\nConfiguration will not be saved.\n\n", errorbuf);


                free (confdir);

                logdir = NIL;
            }
        }
        else
        {
            /* Close the directory we just opened. */

            closedir (tmpdir);
        }
    }


    log_open (logfile);

#else


    memset (buffer, NIL, sizeof (buffer));

    memset (buffer2, NIL, sizeof (buffer2));


    get_executable_name (buffer, sizeof (buffer));


#ifdef ALLEGRO_WINDOWS

   replace_filename (buffer2, buffer, "fakenesw.cfg", sizeof (buffer2));

#else

   replace_filename (buffer2, buffer, "fakenes.cfg", sizeof (buffer2));

#endif


    set_config_file (buffer2);


    replace_filename (buffer2, buffer, "messages.log", sizeof (buffer2));


    log_open (buffer2);


    memset (logfile, NIL, sizeof (logfile));


    strcat (logfile, buffer2);

#endif


   return (0);
}

void platform_exit (void)
{
    log_close ();
}
