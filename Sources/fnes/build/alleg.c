#include <allegro.h>

/* Check if the current version is compatible with Allegro 4.2.0 */
#if (MAKE_VERSION(ALLEGRO_VERSION, ALLEGRO_SUB_VERSION, ALLEGRO_WIP_VERSION) < MAKE_VERSION(4, 2, 0))
#  error You need at least version 4.2.0 of Allegro installed.
#endif
