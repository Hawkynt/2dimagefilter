This directory contains some patches for the Linux SVGALIB.

The *-force.diff patch ensures the correct behaviour of the library
with the advv and advcfg utilities. Without this patch you may not able
to change correctly the modelines at runtime.

The *-noirq.diff patch disables the vsync irq support. Apply this patch
if you have random freezes or generally if applications hang with
a black screen.
This problem is reported for GeForce and Radeon boards with version
SVGALIB 1.9.17/18/19.

The *-radeon.diff patch improves the support for low clock modes with
ATI Rage 128 and Radeon boards and fixes some bugs.

The *-nvidia.diff patch improves the support for low clock modes with
nVidia Riva and GeForce boards.

The *-trident.diff patch improves the support for low clock modes with
Trident boards.

The *-kernel26.diff patch fixes the compilation with linux kernel 2.6
when you get the warning of missing "pci_find_class" function.

If you system use "udev" the svgalib-devfs.sh file must be copied
in the /etc/udev/scripts directory to force the creation of
the svgalib devices under /dev. Ensure also to make it executable.

