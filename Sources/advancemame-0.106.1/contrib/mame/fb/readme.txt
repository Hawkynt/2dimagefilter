This directory contains some patches for the Linux Frame Buffer
Video Drivers.

The radeonfb-lowclock patch improves the support for low clock modes with
ATI Radeon boards.

The rivafb-lowclock patch improves the support for low clock modes with
nVidia Riva and GeForce boards.

The radeonfb-id patch adds support for new ATI radeon boards.

The rivafb-0.9.4c patch adds support for new nVidia GeForge boards, the
rivafb-id adds some more new boards (don't use rivafb-lowclock if you want to
use these patches).

The fb.h patch fixes a problem in the Linux 2.6.0/2.6.1/2.6.2
FrameBuffer include file "linux/fb.h" which prevent a correct compilation of
the FrameBuffer support. This problem is fixed in version 2.6.3.

