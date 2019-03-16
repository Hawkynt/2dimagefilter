MAME extended mouse driver
~~~~~~~~~~~~~~~~~~~~~~~~~~
Please read very carefully on how to get MAME to work with a secondary mouse
support once figured out its really simple.. erm hopefully!!


Requirements
~~~~~~~~~~~~
2 pointing devices be it 2 Serial or 1 serial and 1 PS/2.


Files
~~~~~
README   TXT - This file!
OPTIDRV  COM - MAME second mouse driver for *SERIAL* mouse.
OPTIDRVP COM - MAME second mouse driver for *PS/2* mouse.
MOUSECHK EXE - Quick program to check to test both mouse status.
CTMOUSE  TXT - Primary Mouse Driver readme.
CTMOUSE  COM - Primary Mouse Driver *SERIAL* for use in native DOS.
CTMOUSEP COM - Primary Mouse Driver *PS/2* for use in native DOS.  
SRC          - Source code for the mouse driver programs.


Getting started
~~~~~~~~~~~~~~~

WINDOWS DOS
-----------
Windows seems to like to init all pointing devices so they all operate together
this isn't good news as our extended mouse driver will have no mouse available.
Under Windows please make sure you don't plug the secondary mouse until it has
totally finished loading once loaded plug in the secondary mouse windows shouldn't
know about, if moving it makes the pointer move you've done something wrong!

Get into a DOS prompt, change to the directory you unzipped this zip file.

NATIVE DOS
----------
Locate the directory you unzipped this package in, if you haven't got a primary
mouse driver installed then use the following:

CTMOUSE /1 - If your mouse is on COM port 1.
CTMOUSE /2 - If your mouse is on COM port 2.
CTMOUSE /3 - If your mouse is on COM port 3.
CTMOUSE /4 - If your mouse is on COM port 4.
CTMOUSEP   - If your mouse is a PS/2 type mouse.

you can use your existing primary mouse driver, you are not restrained to using
this mouse driver, although it does rock and is very simple..


Getting Secondary Mouse Support
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Type:

OPTIDRV /1 - If your secondary mouse is on COM port 1.
OPTIDRV /2 - If your secondary mouse is on COM port 2.
OPTIDRV /3 - If your secondary mouse is on COM port 3.
OPTIDRV /4 - If your secondary mouse is on COM port 4.
OPTIDRVP   - If your secondary mouse is a PS/2 type mouse.

After this please run the MOUSECHK program to check everything is ok, this will
report any problems..

If you accidently load OPTIDRV on the wrong COM port just unload it
OPTIDRV /U and select another COM port.

OPTIDRV and OPTIDRVP extended mouse extensions only function in MAME and
have no functionality in any other mouse program, you can unload the driver
at any time by typing:

OPTIDRV /U  - For *SERIAL* mouse.
OPTIDRVP /U - For *PS/2* mouse.


Extra command line functions of CTMOUSE and OPTIDRV *SERIAL*
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /n   - where n is the COM port number
  /R0  - Auto hardware resolution
  /Rnm - n,m=1-9 resolution horizontally/vertically (default is R33)
  /M   - Force Microsoft mode (2 buttons)
  /T   - Force Logitech MouseMan mode (3 buttons)
  /S   - Force Mouse Systems mode (3 buttons)
  /In  - Force IRQ number (n is in hex: n=3-F)
  /L   - Left hand mode (default is right hand mode)
  /U   - Release driver
  /?   - Show this help

If you are using a Happs Control Serial Trackball Interface, make sure the
switch on the board is set to 3 button and use the /S switch to enable *FULL*
3 button support..


Making it load on each boot up
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
If you are running MAME DOS and DOS only you can put the following in your
AUTOEXEC.BAT

<EXAMPLE 1 - Dual Serial>

C:\OPTIDRV\CTMOUSE /1
C:\OPTIDRV\OPTIDRV /2

<EXAMPLE 2 - PS/2 Serial>
C:\OPTIDRV\CTMOUSEP
C:\OPTIDRV\OPTIDRV /1

<EXAMPLE 3 - Serial PS/2>
C:\OPTIDRV\CTMOUSE /1
C:\OPTIDRV\OPTIDRVP


Special Thanks
~~~~~~~~~~~~~~
Nagy Daniel - Cute Mouse Driver, and allowing me to modify/use for MAME!
All MAMEdevs for there continued work.


E-mail
~~~~~~
Original author of OptiMAME:
	Andy Geez aka Andrew Lewis <a.geezer@dial.pipex.com>

Integration in MAME:
	Andrea Mazzoleni <am@mediacom.it>
	(email me if you have questions on the MAME support)


and remember
	two marbles are	better than one,
		double the pleasure,
			triple the fun!

