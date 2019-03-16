#! /bin/bash
# Make SVGALIB /dev nodes.

if [ -z $udev_root ]; then
  . /etc/udev/udev.conf
fi

mknod -m 666 /dev/svga c 209 0
mknod -m 666 /dev/svga1 c 209 1
mknod -m 666 /dev/svga2 c 209 2
mknod -m 666 /dev/svga3 c 209 3
mknod -m 666 /dev/svga4 c 209 4
