#!/usr/bin/perl -w
#===============================================================================
#
#         FILE:  classImage.pm
#
#  DESCRIPTION:  image class
#
#        FILES:  ---
#         BUGS:  ---
#        NOTES:  this file is for free-private use, please give a credit
#                if you (ab)use or convert it. (This also includes
#                translation(s) to other programming languages)
#                This file was created by Hawkynt to demonstrate the algorithms
#                behind certain image filters used in emulators, to enhance
#                low resolution graphics. It took me a huge amount of time to
#                get all these together and I will try to continue implementing
#                new algorithms when I found them or someone sends them to me.
#                For the newest version check http://www.SynthelicZ.de . If you
#                have a clever idea how to get a new algorithm in or how to
#                optimize the code to make things clearer not only faster, feel
#                free to contact me Hawkynt(aT)SynthelicZ[d0T]de
#       AUTHOR:  »SynthelicZ« Hawkynt
#      COMPANY:  »SynthelicZ«
#      VERSION:  1.009 [200907211054]
#      CREATED:  20071109-0849 CET
#     REVISION:  ---
#===============================================================================
package classImage;
use base 'Exporter';              # we could export if we want to

use strict;
use warnings;

use Modules::modIL;               # independant language library
use Modules::IL::Filter;
use Modules::modGlobal;           # everyday routines for programmers
use Classes::classObject;         # to create new classes

use base 'classObject';           # not needed if other parents !

our @EXPORT_OK = qw();            # we don't want to export by default
our @EXPORT = qw();               # we never export anything
our $VERSION=1.010;               # yes thats our applications version
# 1.010 200908051222 Hawkynt
#     + added "_intG_H" to get H from HUV color space
#     * changed temporarely load casepath from "Projects/ImageResizer/src/AdvMAME/output.pl" for develop purposes
# 1.009 200907211054 Hawkynt
#     + added "_voidComplex_PnQwXh" for pattern free filtering
#     + added "_arrHQ2X" for filtering
#     + added "_arrHQ2X3" for filtering
#     + added "_arrHQ2X4" for filtering
#     + added "_arrHQ3X" for filtering
#     + added "_arrHQ4X" for filtering
#     + added "_arrLQ2X" for filtering
#     + added "_arrLQ2X3" for filtering
#     + added "_arrLQ2X4" for filtering
#     + added "_arrLQ3X" for filtering
#     + added "_arrLQ4X" for filtering
#     + added "_voidComplex_nQwXh" to replace all the single HQ and LQ filters
#     + added "_voidComplex_nQwXhBold" to replace all the single HQ and LQ filters
#     + added "_voidComplex_nQwXhSmart" to replace all the single HQ and LQ filters
#     - removed "_voidComplex_HQ4X"
#     - removed "_voidComplex_HQ4XBold"
#     - removed "_voidComplex_HQ4XSmart"
#     - removed "_voidComplex_HQ3X"
#     - removed "_voidComplex_HQ3XBold"
#     - removed "_voidComplex_HQ3XSmart"
#     - removed "_voidComplex_HQ2X"
#     - removed "_voidComplex_HQ2XBold"
#     - removed "_voidComplex_HQ2XSmart"
#     - removed "_voidComplex_LQ4X"
#     - removed "_voidComplex_LQ4XBold"
#     - removed "_voidComplex_LQ4XSmart"
#     - removed "_voidComplex_LQ3X"
#     - removed "_voidComplex_LQ3XBold"
#     - removed "_voidComplex_LQ3XSmart"
#     - removed "_voidComplex_LQ2X"
#     - removed "_voidComplex_LQ2XBold"
#     - removed "_voidComplex_LQ2XSmart"
# 1.008 200907202144 Hawkynt
#     + added "_voidComplex_HQ4X"
#     + added "_voidComplex_HQ4XBold"
#     + added "_voidComplex_HQ4XSmart"
#     + added "_voidComplex_LQ4X"
#     + added "_voidComplex_LQ4XBold"
#     + added "_voidComplex_LQ4XSmart"
#     + added "_voidComplex_LQ3X"
#     + added "_voidComplex_LQ3XBold"
#     + added "_voidComplex_LQ3XSmart"
#     + added "_arrLQ3X"
# 1.007 200907200954 Hawkynt
#     * changed "_voidComplex_Eagle3X" to avoid star/plus patterns
# 1.006 200811051001 Hawkynt
#     + added "arr_objSplitYuv", "_intG_v", "_intG_u" to get different YUV profiles
# 1.005 200810170733 Hawkynt
#     + added "objLoad_ByTK", "voidSave_ByTK" to save/load images using Perl/TK
# 1.004 200806300717 Hawkynt
#     + added objDifference for differing two images
#     + added rows needed before and after current line in filter description
#       possible multithreading in later releases
#     * minor optimizations
# 1.003 200802121258 Hawkynt
#     + added filters: sobel, sobelX and sobelY
# 1.002 Hawkynt
#     * rename arr_objGetRGB and arr_objGetYUV to arr_objSplitRGB and arr_objSplitYUV 
#     + added arr_objSplitGmC  to split into minimum greyscale and color plane
#     + added arr_objSplitGMC  to split into maximum greyscale and color plane
# 1.001 Hawkynt
#     * fixed color creation bug with color values above 255
#     * fixed several comments
# 1.000 200711090849 Hawkynt
#     + initial official version

our $_varMissingCol=_varC_RGB(255,0,255); # color where no pixel has been set
our $_hashFilters=hash(
  
  #                    ptr         newWidth,newHeight,rows needed before, rows needed after
  
  'Sobel',        arr(\&_voidComplex_Sobel,       1,1,1,1),
  'SobelX',       arr(\&_voidComplex_SobelX,      1,1,1,1),
  'SobelY',       arr(\&_voidComplex_SobelY,      1,1,1,1),
  'EPXA',         arr(\&_voidComplex_Scale2X,     2,2,1,1),
  'EPXB',         arr(\&_voidComplex_EPXB,        2,2,1,1),
  'EPXC',         arr(\&_voidComplex_EPXC,        2,2,1,1),
  'EPX3',         arr(\&_voidComplex_EPX3,        3,3,1,1),
    
  'Eagle',        arr(\&_voidComplex_Eagle,       2,2,1,1),
  'Eagle3X',      arr(\&_voidComplex_Eagle3X,     3,3,1,1),
  'SuperEagle',   arr(\&_voidComplex_SuperEagle,  2,2,1,2),
  'SaI2X',        arr(\&_voidComplex_SaI2X,       2,2,1,2),
  'SuperSaI2X',   arr(\&_voidComplex_SuperSaI2X,  2,2,1,2),
  'Scale2X',      arr(\&_voidComplex_Scale2X,     2,2,1,1),
  'Scale3X',      arr(\&_voidComplex_Scale3X,     3,3,1,1),
  'AdvInterp2X',  arr(\&_voidComplex_AdvInterp2X, 2,2,1,1),
  'AdvInterp3X',  arr(\&_voidComplex_AdvInterp3X, 3,3,1,1),
  'Interp2X',     arr(\&_voidComplex_Interp2X,    2,2,0,1),
  'Scan2X',       arr(\&_voidComplex_Scan2X,      2,2,0,0),
  'Scan3X',       arr(\&_voidComplex_Scan3X,      3,3,0,0),
  'RGB2X',        arr(\&_voidComplex_RGB2X,       2,2,0,0),
  'RGB3X',        arr(\&_voidComplex_RGB3X,       3,3,0,0),
  'TV2X',         arr(\&_voidComplex_TV2X,        2,2,0,0),
  'HawkyntTV2X',  arr(\&_voidComplex_HawkyntTV2X, 2,2,0,0),
  'HawkyntTV3X',  arr(\&_voidComplex_HawkyntTV3X, 3,2,0,0),
  'TV3X',         arr(\&_voidComplex_TV3X,        3,3,0,0),
  'NormalDH',     arr(\&_voidComplex_NormalDH,    1,2,0,0),
  'NormalDW',     arr(\&_voidComplex_NormalDW,    2,1,0,0),
  'Normal1X',     arr(\&_voidComplex_Normal1X,    1,1,0,0),
  'Normal2X',     arr(\&_voidComplex_Normal2X,    2,2,0,0),
  'Normal3X',     arr(\&_voidComplex_Normal3X,    3,3,0,0),
);

#                                                                              #
#------------------------------------------------------------------------------#
#                                                                              #

#===  CONSTRUCTOR  ============================================================#
#         NAME:  new
#  DESCRIPTION:  creates an instance (object) of a class
#   PARAMETERS:  $intW,$intH, $opt_hashAttributes
#      RETURNS:  obj
#==============================================================================#
sub new {
  my ( $varInvocant, $intW,$intH, $opt_hashAttributes ) = @_;
  my $objRet;
  my $arrClass = ref($varInvocant) || $varInvocant;
  $objRet = $varInvocant->SUPER::new(
    hash(
      'szErrorMessage',varUNDEF(),
      '_boolIsDestroyed',boolFALSE(),
      '_intWidth',$intW,
      '_intHeight',$intH,
      '_arrImgData',arr()
    )
  );
  bless( $objRet, $arrClass );
  my $arrKeys=arrGetKeys($opt_hashAttributes);
  for (my $intI=0;$intI<intArrLen($arrKeys);$intI++) {
    my $szKey=varArrayItem($arrKeys,$intI);
    $objRet->voidSetAttribute($szKey,varHashItem($opt_hashAttributes,$szKey));
  }
  $objRet->initialize();
  return ($objRet);
}    #=======================[ END OF CONSTRUCTOR ]============================#
sub initialize {
  my ($objThis)=@_;
  # initialize object
  for (my $intY=0;$intY<$objThis->intGetHeight();$intY++) {
    $objThis->{'_arrImgData'}->[$intY]=[];
    for (my $intX=0;$intX<$objThis->intGetWidth();$intX++) {
      $objThis->{'_arrImgData'}->[$intY]->[$intX]=$_varMissingCol;
    }
  }
}

#===  DESTRUCTOR  =============================================================#
#         NAME:  Destroy
#  DESCRIPTION:  deletes an instance (object) of a class
#   PARAMETERS:  
#      RETURNS:  
#==============================================================================#
sub DESTROY {
  my ($objThis) = @_;
  if (boolNot(varHashItem($objThis,'_boolIsDestroyed'))) {
    $objThis->Destroy();
  }
  $objThis->SUPER::DESTROY();
}    #=======================[ END OF DESTRUCTOR ]=============================#
sub Destroy {
  my ($objThis) = @_;
  # close handles etc.
  voidHashItem($objThis,'_boolIsDestroyed',boolTRUE());
}
#                                                                              #
#------------------------------------------------------------------------------#
#                                                                              #
#                                                                              #
#                    -=[ P R I V A T E   M E T H O D S ]=-                     #
#                                                                              #
#                                                                              #
#-----------------------------VIDEO FILTERS------------------------------------#
#                                                                              #
# null filter
sub _voidComplex_Normal1X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
}
# double in x and y
sub _voidComplex_Normal2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
}
# triple in x and y
sub _voidComplex_Normal3X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+2,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
  $objRet->voidSetPixel($intDX+2,$intDY+1,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+2,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY+2,$varC4);
  $objRet->voidSetPixel($intDX+2,$intDY+2,$varC4);
}
# double width
sub _voidComplex_NormalDW {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
}
# double height
sub _voidComplex_NormalDH {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
}
# MAME's TV2X effect
sub _voidComplex_TV2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varSubPixel=_varC_RGB(
    intInt(_intG_R($varC4)*5/8),
    intInt(_intG_G($varC4)*5/8),
    intInt(_intG_B($varC4)*5/8)
  );
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,$varSubPixel);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varSubPixel);
}
# MAME's TV3X effect
sub _voidComplex_TV3X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varSubPixel=_varC_RGB(
    intInt(_intG_R($varC4)*5/8),
    intInt(_intG_G($varC4)*5/8),
    intInt(_intG_B($varC4)*5/8)
  );
  my $varSubPixel2=_varC_RGB(
    intInt(_intG_R($varC4)*5/16),
    intInt(_intG_G($varC4)*5/16),
    intInt(_intG_B($varC4)*5/16)
  );
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+2,$intDY,$varC4);
  
  $objRet->voidSetPixel($intDX,$intDY+1,$varSubPixel);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varSubPixel);
  $objRet->voidSetPixel($intDX+2,$intDY+1,$varSubPixel);
  
  $objRet->voidSetPixel($intDX,$intDY+2,$varSubPixel2);
  $objRet->voidSetPixel($intDX+1,$intDY+2,$varSubPixel2);
  $objRet->voidSetPixel($intDX+2,$intDY+2,$varSubPixel2);
}
# MAME's RGB2X effect
sub _voidComplex_RGB2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX+1,$intDY,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,_intG_B($varC4)));
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
}
# MAME's RGB3X effect
sub _voidComplex_RGB3X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX+2,$intDY,_varC_RGB(0,0,_intG_B($varC4)));
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,_intG_B($varC4)));
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
  $objRet->voidSetPixel($intDX+2,$intDY+1,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX,$intDY+2,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX+1,$intDY+2,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX+2,$intDY+2,$varC4);
}
# Hawkynt's TV2X (created to display real-color images in 8-Bit modes)
sub _voidComplex_HawkyntTV2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $intI=_intG_Y($varC4);
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX+1,$intDY,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,_intG_B($varC4)));
  $objRet->voidSetPixel($intDX+1,$intDY+1,_varC_RGB($intI,$intI,$intI));
}
# Hawkynt's TV3X (created to display real-color images in 8-Bit modes)
sub _voidComplex_HawkyntTV3X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $intAP=1 - 2 * ($intX % 2);
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX+1,$intDY,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX+2,$intDY,_varC_RGB(0,0,_intG_B($varC4)));
  
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX+1,$intDY+1,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX+2,$intDY+1,_varC_RGB(0,0,0));
  
  $objRet->voidSetPixel($intDX,$intDY-$intAP,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX+1,$intDY+$intAP,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX+2,$intDY-$intAP,_varC_RGB(0,0,_intG_B($varC4)));
  
}

sub _voidComplex_Scan2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX+1,$intDY+1,_varC_RGB(0,0,0));
}

sub _voidComplex_Scan3X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+2,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX+1,$intDY+1,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX+2,$intDY+1,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX,$intDY+2,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX+1,$intDY+2,_varC_RGB(0,0,0));
  $objRet->voidSetPixel($intDX+2,$intDY+2,_varC_RGB(0,0,0));
}
# Hawkynt's bilinear interpolate (my idea is possibly used already, but to separate here I gave it my name...)
sub _voidComplex_Interp2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX  ,$intY  );
  my $varC5=$objThis->varGetPixel($intX+1,$intY  );
  my $varC7=$objThis->varGetPixel($intX  ,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;
  $varE01=_varInterp2($varC4,$varC5,1,1);
  $varE10=_varInterp2($varC4,$varC7,1,1);
  $varE11=_varInterp4($varC4,$varC5,$varC7,$varC8,1,1,1,1);
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
}
# MAME's AdvInterp2X (very similar to Scale2X but uses interpolation)
sub _voidComplex_AdvInterp2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC1=$objThis->varGetPixel($intX  ,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY  );
  my $varC4=$objThis->varGetPixel($intX  ,$intY  );
  my $varC5=$objThis->varGetPixel($intX+1,$intY  );
  my $varC7=$objThis->varGetPixel($intX  ,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;
  if (_boolNE($varC1,$varC7) and _boolNE($varC3,$varC5)) {
    if (_boolE($varC3,$varC1)) { $varE00=_varInterp2($varC3,$varC4,5,3); };
    if (_boolE($varC1,$varC5)) { $varE01=_varInterp2($varC5,$varC4,5,3); };
    if (_boolE($varC3,$varC7)) { $varE10=_varInterp2($varC3,$varC4,5,3); };
    if (_boolE($varC7,$varC5)) { $varE11=_varInterp2($varC5,$varC4,5,3); };
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
}
# MAME's AdvInterp3X (very similar to Scale3X but uses interpolation)
sub _voidComplex_AdvInterp3X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;my $varE02=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;my $varE12=$varC4;
  my $varE20=$varC4;my $varE21=$varC4;my $varE22=$varC4;
  
  if (_boolNE($varC1,$varC7) and _boolNE($varC3,$varC5)) {
    if (_boolE($varC3,$varC1)) { $varE00=_varInterp2($varC3,$varC4,5,3); };
    if (
      (_boolE($varC3,$varC1) and _boolNE($varC4,$varC2)) or
      (_boolE($varC5,$varC1) and _boolNE($varC4,$varC0))
    ) { $varE01=$varC1; };
    if (_boolE($varC1,$varC5)) { $varE02=_varInterp2($varC5,$varC4,5,3); };
    if (
      (_boolE($varC3,$varC1) and _boolNE($varC4,$varC6)) or
      (_boolE($varC3,$varC7) and _boolNE($varC4,$varC0))
    ) { $varE10=$varC3; };
    if (
      (_boolE($varC5,$varC1) and _boolNE($varC4,$varC8)) or
      (_boolE($varC5,$varC7) and _boolNE($varC4,$varC2))
    ) { $varE12=$varC5; };
    if (_boolE($varC3,$varC7)) { $varE20=_varInterp2($varC3,$varC4,5,3); };
    if (
      (_boolE($varC3,$varC7) and _boolNE($varC4,$varC8)) or
      (_boolE($varC5,$varC7) and _boolNE($varC4,$varC6))
    ) { $varE21=$varC7; };
    if (_boolE($varC7,$varC5)) { $varE22=_varInterp2($varC5,$varC4,5,3); };
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX+2,$intDY  ,$varE02);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
  $objRet->voidSetPixel($intDX+2,$intDY+1,$varE12);
  $objRet->voidSetPixel($intDX  ,$intDY+2,$varE20);
  $objRet->voidSetPixel($intDX+1,$intDY+2,$varE21);
  $objRet->voidSetPixel($intDX+2,$intDY+2,$varE22);
}

# Andrea Mazzoleni's Scale3X 
sub _voidComplex_Scale3X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;my $varE02=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;my $varE12=$varC4;
  my $varE20=$varC4;my $varE21=$varC4;my $varE22=$varC4;
  if (_boolNE($varC1,$varC7) and _boolNE($varC3,$varC5)) {
    if (_boolE($varC3,$varC1)) { $varE00=$varC3; };
    if (
      (_boolE($varC3,$varC1) and _boolNE($varC4,$varC2)) or
      (_boolE($varC5,$varC1) and _boolNE($varC4,$varC0))
    ) { $varE01=$varC1; };
    if (_boolE($varC1,$varC5)) { $varE02=$varC5; };
    if (
      (_boolE($varC3,$varC1) and _boolNE($varC4,$varC6))or
      (_boolE($varC3,$varC7) and _boolNE($varC4,$varC0))
    ) { $varE10=$varC3; };
    if (
      (_boolE($varC5,$varC1) and _boolNE($varC4,$varC8))or
      (_boolE($varC5,$varC7) and _boolNE($varC4,$varC2))
    ) { $varE12=$varC5; };
    if (_boolE($varC3,$varC7)) { $varE20=$varC3; };
    if (
      (_boolE($varC3,$varC7) and _boolNE($varC4,$varC8))or
      (_boolE($varC5,$varC7) and _boolNE($varC4,$varC6))
    ) { $varE21=$varC7; };
    if (_boolE($varC7,$varC5)) { $varE22=$varC5; };
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX+2,$intDY  ,$varE02);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
  $objRet->voidSetPixel($intDX+2,$intDY+1,$varE12);
  $objRet->voidSetPixel($intDX  ,$intDY+2,$varE20);
  $objRet->voidSetPixel($intDX+1,$intDY+2,$varE21);
  $objRet->voidSetPixel($intDX+2,$intDY+2,$varE22);
}
# Hawkynt's Pattern free filter routine
sub _voidComplex_PnQwXh {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY,$szFilter,$intScaleX,$intScaleY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $arrResult=eval('return [_arr'.$szFilter.'(
      $varC0,$varC1,$varC2,
      $varC3,$varC4,$varC5,
      $varC6,$varC7,$varC8
  )];');
  my $intK=0;
  for (my $intI=0;$intI<$intScaleY;$intI++) {
    for (my $intJ=0;$intJ<$intScaleX;$intJ++) {
      $objRet->voidSetPixel($intDX+$intJ,$intDY+$intI,$arrResult->[$intK]);
      $intK++;
    }
  }
}
# Hawkynt's nQwXh (is basically derived from Maxim Stepin's HQ2X and used in VBA and MAME)
sub _voidComplex_nQwXh {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY,$szFilter,$intScaleX,$intScaleY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $intPattern=0;
  if (_boolYUV_NE($varC4,$varC0)) { $intPattern|=0b00000001};
  if (_boolYUV_NE($varC4,$varC1)) { $intPattern|=0b00000010};
  if (_boolYUV_NE($varC4,$varC2)) { $intPattern|=0b00000100};
  if (_boolYUV_NE($varC4,$varC3)) { $intPattern|=0b00001000};
  if (_boolYUV_NE($varC4,$varC5)) { $intPattern|=0b00010000};
  if (_boolYUV_NE($varC4,$varC6)) { $intPattern|=0b00100000};
  if (_boolYUV_NE($varC4,$varC7)) { $intPattern|=0b01000000};
  if (_boolYUV_NE($varC4,$varC8)) { $intPattern|=0b10000000};
  my $arrResult=eval('return [_arr'.$szFilter.'(
      $varC0,$varC1,$varC2,
      $varC3,$varC4,$varC5,
      $varC6,$varC7,$varC8,
      $intPattern
  )];');
  my $intK=0;
  for (my $intI=0;$intI<$intScaleY;$intI++) {
    for (my $intJ=0;$intJ<$intScaleX;$intJ++) {
      $objRet->voidSetPixel($intDX+$intJ,$intDY+$intI,$arrResult->[$intK]);
      $intK++;
    }
  }
}
# Hawkynt's nQwXh Bold Version (derived from SNES 9X's) 
sub _voidComplex_nQwXhBold {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY,$szFilter,$intScaleX,$intScaleY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $floatAvg=(
    _intG_Brightness($varC0)+
    _intG_Brightness($varC1)+
    _intG_Brightness($varC2)+
    _intG_Brightness($varC3)+
    _intG_Brightness($varC4)+
    _intG_Brightness($varC5)+
    _intG_Brightness($varC6)+
    _intG_Brightness($varC7)+
    _intG_Brightness($varC8)
  )/9;
  my $boolDC4=(_intG_Brightness($varC4)>$floatAvg);
  my $intPattern=0;
  if (_boolNE($varC0,$varC4) and ((_intG_Brightness($varC0)>$floatAvg)!=$boolDC4)) { $intPattern|=0b00000001};
  if (_boolNE($varC1,$varC4) and ((_intG_Brightness($varC1)>$floatAvg)!=$boolDC4)) { $intPattern|=0b00000010};
  if (_boolNE($varC2,$varC4) and ((_intG_Brightness($varC2)>$floatAvg)!=$boolDC4)) { $intPattern|=0b00000100};
  if (_boolNE($varC3,$varC4) and ((_intG_Brightness($varC3)>$floatAvg)!=$boolDC4)) { $intPattern|=0b00001000};
  if (_boolNE($varC5,$varC4) and ((_intG_Brightness($varC5)>$floatAvg)!=$boolDC4)) { $intPattern|=0b00010000};
  if (_boolNE($varC6,$varC4) and ((_intG_Brightness($varC6)>$floatAvg)!=$boolDC4)) { $intPattern|=0b00100000};
  if (_boolNE($varC7,$varC4) and ((_intG_Brightness($varC7)>$floatAvg)!=$boolDC4)) { $intPattern|=0b01000000};
  if (_boolNE($varC8,$varC4) and ((_intG_Brightness($varC8)>$floatAvg)!=$boolDC4)) { $intPattern|=0b10000000};
  my $arrResult=eval('return [_arr'.szReplace($szFilter,'Bold','').'(
      $varC0,$varC1,$varC2,
      $varC3,$varC4,$varC5,
      $varC6,$varC7,$varC8,
      $intPattern
  )];');
  my $intK=0;
  for (my $intI=0;$intI<$intScaleY;$intI++) {
    for (my $intJ=0;$intJ<$intScaleX;$intJ++) {
      $objRet->voidSetPixel($intDX+$intJ,$intDY+$intI,$arrResult->[$intK]);
      $intK++;
    }
  }
}
# Hawkynt's nQwXh Smart Version (derived from SNES9X's HQ2X Smart)
sub _voidComplex_nQwXhSmart {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY,$szFilter,$intScaleX,$intScaleY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  if (_boolE($varC0,$varC4) or _boolE($varC2,$varC4) or _boolE($varC6,$varC4) or _boolE($varC8,$varC4)) {
    $objThis->_voidComplex_nQwXh($intX,$intY,$objRet,$intDX,$intDY,szReplace($szFilter,'Smart',''),$intScaleX,$intScaleY);
  } else {
    $objThis->_voidComplex_nQwXhBold($intX,$intY,$objRet,$intDX,$intDY,szReplace($szFilter,'Smart',''),$intScaleX,$intScaleY);
  }
}

# SNES9X's EPXB
sub _voidComplex_EPXB {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;
  if (
    _boolNE($varC3,$varC5) and 
    _boolNE($varC1,$varC7) and ( # diagonal
      (
        _boolE($varC4,$varC3) or
        _boolE($varC4,$varC7) or
        _boolE($varC4,$varC5) or
        _boolE($varC4,$varC1) or ( # edge smoothing
          (
            _boolNE($varC0,$varC8) or
            _boolE($varC4,$varC6) or
            _boolE($varC4,$varC2)
          ) and (
            _boolNE($varC6,$varC2) or
            _boolE($varC4,$varC0) or
            _boolE($varC4,$varC8)
          )
        )
      )
    )
  ) {
    if (
      _boolE($varC1,$varC3) and (
        _boolNE($varC4,$varC0) or
        _boolNE($varC4,$varC8) or
        _boolNE($varC1,$varC2) or
        _boolNE($varC3,$varC6)
      )
    ) { $varE00=$varC1; }
    if (
      _boolE($varC5,$varC1) and (
        _boolNE($varC4,$varC2) or
        _boolNE($varC4,$varC6) or
        _boolNE($varC5,$varC8) or
        _boolNE($varC1,$varC0)
      )
    ) { $varE01=$varC5; }
    if (
      _boolE($varC3,$varC7) and (
        _boolNE($varC4,$varC6) or
        _boolNE($varC4,$varC2) or
        _boolNE($varC3,$varC0) or
        _boolNE($varC7,$varC8)
      )
    ) { $varE10=$varC3; }
    if (
      _boolE($varC7,$varC5) and (
        _boolNE($varC4,$varC8) or
        _boolNE($varC4,$varC0) or
        _boolNE($varC7,$varC6) or
        _boolNE($varC5,$varC2)
      )
    ) { $varE11=$varC7; }
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
}
# SNES9X's EPX3
sub _voidComplex_EPX3 {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX  ,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX  ,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX  ,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;my $varE02=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;my $varE12=$varC4;
  my $varE20=$varC4;my $varE21=$varC4;my $varE22=$varC4;
  if (
    _boolNE($varC3 , $varC5) and
    _boolNE($varC7 , $varC1)
  ) {
    my $bool4N0 = _boolNE($varC4 , $varC0);
    my $bool4N6 = _boolNE($varC4 , $varC6);
    my $bool4N8 = _boolNE($varC4 , $varC8);
    my $bool4N2 = _boolNE($varC4 , $varC2);
    my $boolDA = _boolE($varC1 , $varC3) and ($bool4N0 or $bool4N8 or _boolNE($varC1 , $varC2) or _boolNE($varC3 , $varC6));
    my $boolAB = _boolE($varC3 , $varC7) and ($bool4N6 or $bool4N2 or _boolNE($varC3 , $varC0) or _boolNE($varC7 , $varC8));
    my $boolBC = _boolE($varC7 , $varC5) and ($bool4N8 or $bool4N0 or _boolNE($varC7 , $varC6) or _boolNE($varC5 , $varC2));
    my $boolCD = _boolE($varC5 , $varC1) and ($bool4N2 or $bool4N6 or _boolNE($varC5 , $varC8) or _boolNE($varC1 , $varC0));
    if ((_boolNE($varC3 , $varC5) and _boolNE($varC7 , $varC1) and
      (_boolE($varC4 , $varC3) or _boolE($varC4,$varC7) or _boolE($varC4,$varC5) or _boolE($varC4,$varC1) or _boolE($varC4,$varC0) or _boolE($varC4,$varC6) or _boolE($varC4,$varC8) or _boolE($varC4,$varC2))))
    {
      if ($boolDA)                                          { $varE00=$varC3; };
      if (($boolCD and $bool4N0) or ($boolDA and $bool4N2)) { $varE01=$varC1; }; 
      if ($boolCD)                                          { $varE02=$varC5; };
      if (($boolDA and $bool4N6) or ($boolAB and $bool4N0)) { $varE10=$varC3; };
      if (($boolBC and $bool4N2) or ($boolCD and $bool4N8)) { $varE12=$varC5; };
      if ($boolAB)                                          { $varE20=$varC3; };
      if (($boolAB and $bool4N8) or ($boolBC and $bool4N6)) { $varE21=$varC7; };
      if ($boolBC)                                          { $varE22=$varC5; };
    } else {
      if ($boolDA and (_boolNE($varC4,$varC7) and _boolNE($varC4,$varC5))) { $varE00=$varC3; };
      if ($boolCD and (_boolNE($varC4,$varC3) and _boolNE($varC4,$varC7))) { $varE02=$varC5; };
      if ($boolAB and (_boolNE($varC4,$varC5) and _boolNE($varC4,$varC1))) { $varE20=$varC3; };
      if ($boolBC and (_boolNE($varC4,$varC1) and _boolNE($varC4,$varC3))) { $varE22=$varC5; };
    }
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX+2,$intDY  ,$varE02);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
  $objRet->voidSetPixel($intDX+2,$intDY+1,$varE12);
  $objRet->voidSetPixel($intDX  ,$intDY+2,$varE20);
  $objRet->voidSetPixel($intDX+1,$intDY+2,$varE21);
  $objRet->voidSetPixel($intDX+2,$intDY+2,$varE22);
}
# SNES9X's EPXC (basically this is EPX3 scaled down to 2X)
sub _voidComplex_EPXC {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;
  
  if (
    _boolNE($varC3 , $varC5) and 
    _boolNE($varC7 , $varC1)
  ) {
    my $bool4N0 = _boolNE($varC4 , $varC0);
    my $bool4N6 = _boolNE($varC4 , $varC6);
    my $bool4N8 = _boolNE($varC4 , $varC8);
    my $bool4N2 = _boolNE($varC4 , $varC2);
    my $boolDA = _boolE($varC1 , $varC3) and ($bool4N0 or $bool4N8 or _boolNE($varC1 , $varC2) or _boolNE($varC3 , $varC6));
    my $boolAB = _boolE($varC3 , $varC7) and ($bool4N6 or $bool4N2 or _boolNE($varC3 , $varC0) or _boolNE($varC7 , $varC8));
    my $boolBC = _boolE($varC7 , $varC5) and ($bool4N8 or $bool4N0 or _boolNE($varC7 , $varC6) or _boolNE($varC5 , $varC2));
    my $boolCD = _boolE($varC5 , $varC1) and ($bool4N2 or $bool4N6 or _boolNE($varC5 , $varC8) or _boolNE($varC1 , $varC0));
    if (
    (
      (
        _boolE($varC4 , $varC3) or 
        _boolE($varC4,$varC7) or 
        _boolE($varC4,$varC5) or 
        _boolE($varC4,$varC1) or 
        _boolE($varC4,$varC0) or 
        _boolE($varC4,$varC6) or 
        _boolE($varC4,$varC8) or 
        _boolE($varC4,$varC2)
    ))) {
      my $varC3A = (($boolDA and $bool4N6) or ($boolAB and $bool4N0)) ? $varC3 : $varC4;
      my $varC7B = (($boolAB and $bool4N8) or ($boolBC and $bool4N6)) ? $varC7 : $varC4;
      my $varC5C = (($boolBC and $bool4N2) or ($boolCD and $bool4N8)) ? $varC5 : $varC4;
      my $varC1D = (($boolCD and $bool4N0) or ($boolDA and $bool4N2)) ? $varC1 : $varC4;
      if ($boolDA) { $varE00=$varC3; };
      if ($boolCD) { $varE01=$varC5; };
      if ($boolAB) { $varE10=$varC3; };
      if ($boolBC) { $varE11=$varC5; };
      $varE00=_varInterp4($varE00, $varC1D, $varC3A, $varC4,5,1,1,1);
      $varE01=_varInterp4($varE01, $varC7B, $varC5C, $varC4,5,1,1,1);
      $varE10=_varInterp4($varE10, $varC3A, $varC7B, $varC4,5,1,1,1);
      $varE11=_varInterp4($varE11, $varC5C, $varC1D, $varC4,5,1,1,1);
    } else {
      if ($boolDA and (_boolNE($varC4,$varC7) and _boolNE($varC4,$varC5))) { $varE00=$varC3; };
      if ($boolCD and (_boolNE($varC4,$varC3) and _boolNE($varC4,$varC7))) { $varE01=$varC5; };
      if ($boolAB and (_boolNE($varC4,$varC5) and _boolNE($varC4,$varC1))) { $varE10=$varC3; };
      if ($boolBC and (_boolNE($varC4,$varC1) and _boolNE($varC4,$varC3))) { $varE11=$varC5; };
      $varE00=_varInterp2($varC4,$varE00,3,1);
      $varE01=_varInterp2($varC4,$varE01,3,1);
      $varE10=_varInterp2($varC4,$varE10,3,1);
      $varE11=_varInterp2($varC4,$varE11,3,1);
    }
  }
  
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
}

# Kreed's SuperEagle
sub _voidComplex_SuperEagle {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varD0=$objThis->varGetPixel($intX-1,$intY+2);
  my $varD1=$objThis->varGetPixel($intX,$intY+2);
  my $varD2=$objThis->varGetPixel($intX+1,$intY+2);
  my $varD3=$objThis->varGetPixel($intX+2,$intY-1);
  my $varD4=$objThis->varGetPixel($intX+2,$intY);
  my $varD5=$objThis->varGetPixel($intX+2,$intY+1);
  my $varE00=$varC4;my $varE01=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;
  if (_boolNE($varC4, $varC8)) {
    if (_boolE($varC7 , $varC5)) {
      $varE01=$varC7;
      $varE10=$varC7;
      if (_boolE($varC6 , $varC7) or _boolE($varC5 , $varC2)) {
        $varE00=_varInterp2($varC7,$varC4,3,1);
      } else {
        $varE00=_varInterp2($varC4,$varC5,1,1);
      }
      
      if (_boolE($varC5 , $varD4) or _boolE($varC7 , $varD1)) {
        $varE11=_varInterp2($varC7,$varC8,3,1);
      } else {
        $varE11=_varInterp2($varC7,$varC8,1,1);
      }
    } else {
      $varE11=_varInterp3($varC8,$varC7,$varC5,6,1,1);
      $varE00=_varInterp3($varC4,$varC7,$varC5,6,1,1);
      $varE10=_varInterp3($varC7,$varC4,$varC8,6,1,1);
      $varE01=_varInterp3($varC5,$varC4,$varC8,6,1,1);
    }
  } else {
    if (_boolNE($varC7 , $varC5)) {
      if (_boolE($varC1 , $varC4) or _boolE($varC8 , $varD5)) {
        $varE01=_varInterp2($varC4,$varC5,3,1);
      } else {
        $varE01=_varInterp2($varC4,$varC5,1,1);
      }
      
      if (_boolE($varC8 , $varD2) or _boolE($varC3 , $varC4)) {
        $varE10=_varInterp2($varC4,$varC7,3,1);
      } else {
        $varE10=_varInterp2($varC7,$varC8,1,1);
      }
    } else {
      my $intR= 0;
      $intR += _intConc2d($varC5,$varC4,$varC6,$varD1);
      $intR += _intConc2d($varC5,$varC4,$varC3,$varC1);
      $intR += _intConc2d($varC5,$varC4,$varD2,$varD5);
      $intR += _intConc2d($varC5,$varC4,$varC2,$varD4);
      
      if ($intR > 0) {
        $varE10=$varC7;
        $varE01=$varC7;
        $varE11=_varInterp2($varC4,$varC5,1,1);
        $varE00=_varInterp2($varC4,$varC5,1,1);
      } elsif ($intR < 0) {
        $varE10=_varInterp2($varC4,$varC5,1,1);
        $varE01=_varInterp2($varC4,$varC5,1,1);
      } else {
        $varE10=$varC7;
        $varE01=$varC7;
      }
    }
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
}
# Derek Liauw Kie Fa's 2XSaI
sub _voidComplex_SaI2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varD0=$objThis->varGetPixel($intX-1,$intY+2);
  my $varD1=$objThis->varGetPixel($intX,$intY+2);
  my $varD2=$objThis->varGetPixel($intX+1,$intY+2);
  my $varD3=$objThis->varGetPixel($intX+2,$intY-1);
  my $varD4=$objThis->varGetPixel($intX+2,$intY);
  my $varD5=$objThis->varGetPixel($intX+2,$intY+1);
  #my $varD6=$objThis->varGetPixel($intX+2,$intY+2);
  my $varE00=$varC4;my $varE01=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;
  
  if (_boolE($varC4,$varC8) and _boolNE($varC5, $varC7)) {
    if ((_boolE($varC4, $varC1) and _boolE($varC5 , $varD5)) or
      (_boolE($varC4, $varC7) and _boolE($varC4 , $varC2) and _boolNE($varC5 , $varC1) and _boolE($varC5, $varD3))) {
      #nothing
    } else {
      $varE01=_varInterp2($varC4,$varC5,1,1);
    }
    
    if ((_boolE($varC4 , $varC3) and _boolE($varC7 , $varD2)) or
      (_boolE($varC4 , $varC5) and _boolE($varC4 , $varC6) and _boolNE($varC3 , $varC7)  and _boolE($varC7 , $varD0))) {
      #nothing
    } else {
      $varE10=_varInterp2($varC4,$varC7,1,1);
    }
  } elsif (_boolE($varC5, $varC7) and _boolNE($varC4 , $varC8)) {
    if ((_boolE($varC5 , $varC2) and _boolE($varC4 , $varC6)) or
      (_boolE($varC5 , $varC1) and _boolE($varC5 , $varC8) and _boolNE($varC4 , $varC2) and _boolE($varC4 , $varC0))) {
      $varE01=$varC5;
    } else {
      $varE01=_varInterp2($varC4,$varC5,1,1);
    }
    
    if ((_boolE($varC7 , $varC6) and _boolE($varC4 , $varC2)) or
      (_boolE($varC7 , $varC3) and _boolE($varC7 , $varC8) and _boolNE($varC4 , $varC6) and _boolE($varC4 , $varC0))) {
      $varE10=$varC7;
    } else {
      $varE10=_varInterp2($varC4,$varC7,1,1);
    }
    $varE11=$varC5;
  } elsif (_boolE($varC4 , $varC8) and _boolE($varC5, $varC7)) {
    if (_boolNE($varC4 , $varC5)) {
      my $intR = 0;
      $intR += _intConc2d($varC4,$varC5,$varC3,$varC1);
      $intR -= _intConc2d($varC5,$varC4,$varD4,$varC2);
      $intR -= _intConc2d($varC5,$varC4,$varC6,$varD1);
      $intR += _intConc2d($varC4,$varC5,$varD5,$varD2);
      
      if ($intR < 0) {
        $varE11=$varC5;
      } elsif ($intR==0) {
        $varE11=_varInterp4($varC4,$varC5,$varC7,$varC8,1,1,1,1);
      }
      $varE10=_varInterp2($varC4,$varC7,1,1);
      $varE01=_varInterp2($varC4,$varC5,1,1);
    }
  } else {
    $varE11=_varInterp4($varC4,$varC5,$varC7,$varC8,1,1,1,1);
    
    if (_boolE($varC4 , $varC7) and _boolE($varC4 , $varC2)
      and _boolNE($varC5 , $varC1) and _boolE($varC5 , $varD3)) {
      #nothing
    } elsif (_boolE($varC5 , $varC1) and _boolE($varC5 , $varC8)
      and _boolNE($varC4 , $varC2) and _boolE($varC4 , $varC0)) {
      $varE01=$varC5;
    } else {
      $varE01=_varInterp2($varC4,$varC5,1,1);
    }
    
    if (_boolE($varC4 , $varC5) and _boolE($varC4 , $varC6)
      and _boolNE($varC3 , $varC7) and _boolE($varC7 , $varD0)) {
      #nothing
    } elsif (_boolE($varC7 , $varC3) and _boolE($varC7 , $varC8)
      and _boolNE($varC4 , $varC6) and _boolE($varC4 , $varC0)) {
      $varE10=$varC7;
    } else {
      $varE10=_varInterp2($varC4,$varC7,1,1);
    }
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
}
# Kreed's Super2XSaI
sub _voidComplex_SuperSaI2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC0=$objThis->varGetPixel($intX-1,$intY-1);
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC2=$objThis->varGetPixel($intX+1,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC6=$objThis->varGetPixel($intX-1,$intY+1);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  my $varD0=$objThis->varGetPixel($intX-1,$intY+2);
  my $varD1=$objThis->varGetPixel($intX,$intY+2);
  my $varD2=$objThis->varGetPixel($intX+1,$intY+2);
  my $varD3=$objThis->varGetPixel($intX+2,$intY-1);
  my $varD4=$objThis->varGetPixel($intX+2,$intY);
  my $varD5=$objThis->varGetPixel($intX+2,$intY+1);
  my $varD6=$objThis->varGetPixel($intX+2,$intY+2);
  my $varE00=$varC4;my $varE01=$varC4;
  my $varE10=$varC4;my $varE11=$varC4;
  
  if (_boolE($varC7 , $varC5) and _boolNE($varC4 , $varC8)) {
    $varE11=$varC7;
    $varE01=$varC7;
  } elsif (_boolE($varC4 , $varC8) and _boolNE($varC7 , $varC5)) {
    #nothing
  } elsif (_boolE($varC4 , $varC8) and _boolE($varC7 , $varC5)) {
    my $intR = 0;
    $intR  += _intConc2d($varC5,$varC4,$varC6,$varD1);
    $intR  += _intConc2d($varC5,$varC4,$varC3,$varC1);
    $intR  += _intConc2d($varC5,$varC4,$varD2,$varD5);
    $intR  += _intConc2d($varC5,$varC4,$varC2,$varD4);
    
    if ($intR > 0) {
      $varE11=$varC5;
      $varE01=$varC5;
    } elsif ($intR  == 0) {
      $varE11=_varInterp2($varC4,$varC5,1,1);
      $varE01=_varInterp2($varC4,$varC5,1,1);
    }
  } else {
    if (_boolE($varC5 , $varC8) and _boolE($varC8 , $varD1) and _boolNE($varC7 , $varD2) and _boolNE($varC8 , $varD0)) {
      $varE11=_varInterp2($varC8,$varC7,3,1);
    } elsif (_boolE($varC4 , $varC7) and _boolE($varC7 , $varD2) and _boolNE($varD1 , $varC8) and _boolNE($varC7 , $varD6)) {
      $varE11=_varInterp2($varC7,$varC8,3,1);
    } else {
      $varE11=_varInterp2($varC7,$varC8,1,1);
    }
    if (_boolE($varC5 , $varC8) and _boolE($varC5 , $varC1) and _boolNE($varC4 , $varC2) and _boolNE($varC5 , $varC0)) {
      $varE01=_varInterp2($varC5,$varC4,3,1);
    } elsif (_boolE($varC4, $varC7) and _boolE($varC4 , $varC2) and _boolNE($varC1 , $varC5) and _boolNE($varC4 , $varD3)) {
      $varE01=_varInterp2($varC4,$varC5,3,1);
    } else {
      $varE01=_varInterp2($varC4,$varC5,1,1);
    }
  }
  if (_boolE($varC4 , $varC8) and _boolNE($varC7 , $varC5) and _boolE($varC3 , $varC4) and _boolNE($varC4 , $varD2)) {
    $varE10=_varInterp2($varC7,$varC4,1,1);
  } elsif (_boolE($varC4 , $varC6) and _boolE($varC5 , $varC4) and _boolNE($varC3 , $varC7) and _boolNE($varC4 , $varD0)) {
    $varE10=_varInterp2($varC7,$varC4,1,1);
  } else {
    $varE10=$varC7;
  }
  if (_boolE($varC7 , $varC5) and _boolNE($varC4 , $varC8) and _boolE($varC6 , $varC7) and _boolNE($varC7 , $varC2)) {
    $varE00=_varInterp2($varC7,$varC4,1,1);
  } elsif (_boolE($varC3 , $varC7) and _boolE($varC8 , $varC7) and _boolNE($varC6 , $varC4) and _boolNE($varC7 , $varC0)) {
    $varE00=_varInterp2($varC7,$varC4,1,1);
  }
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
}


my $_arr_arr_floatSobelMatrixX=arr(
  arr( 1, 0,-1),
  arr( 2, 0,-2),
  arr( 1, 0,-1)
);

my $_arr_arr_floatSobelMatrixY=arr(
  arr( 1, 2, 1),
  arr( 0, 0, 0),
  arr(-1,-2,-1)
);

sub _voidComplex_Sobel {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $floatY_GX=0;
  my $floatY_GY=0;
  for (my $intSY=-1;$intSY<=1;$intSY++) {
    for (my $intSX=-1;$intSX<=1;$intSX++) {
      my $intCol_Y=_intG_Y($objThis->varGetPixel($intX+$intSX,$intY+$intSY));
      $floatY_GX+=$intCol_Y*$_arr_arr_floatSobelMatrixX->[$intSY+1]->[$intSX+1];
      $floatY_GY+=$intCol_Y*$_arr_arr_floatSobelMatrixY->[$intSY+1]->[$intSX+1];
    }
  }
  $floatY_GX/=4;
  $floatY_GY/=4;
  my $intCol_Y=0;
  $intCol_Y=sqrt($floatY_GX*$floatY_GX+$floatY_GY*$floatY_GY)/sqrt(2);
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB($intCol_Y,$intCol_Y,$intCol_Y));
}

sub _voidComplex_SobelX {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $floatY_GX=0;
  for (my $intSY=-1;$intSY<=1;$intSY++) {
    for (my $intSX=-1;$intSX<=1;$intSX++) {
      my $intCol_Y=_intG_Y($objThis->varGetPixel($intX+$intSX,$intY+$intSY));
      $floatY_GX+=$intCol_Y*$_arr_arr_floatSobelMatrixX->[$intSY+1]->[$intSX+1];
    }
  }
  $floatY_GX/=4;
  my $intCol_Y=0;
  $intCol_Y=127.5+$floatY_GX/2;
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB($intCol_Y,$intCol_Y,$intCol_Y));
}

sub _voidComplex_SobelY {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $floatY_GY=0;
  for (my $intSY=-1;$intSY<=1;$intSY++) {
    for (my $intSX=-1;$intSX<=1;$intSX++) {
      my $intCol_Y=_intG_Y($objThis->varGetPixel($intX+$intSX,$intY+$intSY));
      $floatY_GY+=$intCol_Y*$_arr_arr_floatSobelMatrixY->[$intSY+1]->[$intSX+1];
    }
  }
  $floatY_GY/=4;
  my $intCol_Y=0;
  $intCol_Y=127.5+$floatY_GY/2;
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB($intCol_Y,$intCol_Y,$intCol_Y));
}

#                                                                              #
#                 -=[ P R I V A T E   P R O P E R T I E S ]=-                  #
#                                                                              #
# create rgb color
sub _varC_RGB {
  my ($intR,$intG,$intB)=@_;
  if ($intR < 0) { $intR = 0; };
  if ($intG < 0) { $intG = 0; };
  if ($intB < 0) { $intB = 0; };
  # v1.001 Hawkynt:changed borderline error
  if ($intR > 255) { $intR = 255; };
  if ($intG > 255) { $intG = 255; };
  if ($intB > 255) { $intB = 255; };
  # end v1.001
  my $varColor=$intR << 16 | $intG << 8 | $intB;
  return($varColor);
}
# get r from color
sub _intG_R {
  my ($varColor)=@_;
  my $intRet=($varColor >> 16) & 255;
  return($intRet);
}
# get g from color
sub _intG_G {
  my ($varColor)=@_;
  my $intRet=($varColor >> 8) & 255;
  return($intRet);
}
# get b from color
sub _intG_B {
  my ($varColor)=@_;
  my $intRet=($varColor) & 255;
  return($intRet);
}
# get y from color
our $opt_arrG_Y=arr(); # Y convert cache
sub _intG_Y {
  my ($varColor)=@_;
  my $intRet;
  if (not(defined($opt_arrG_Y->[$varColor]))) {
    $opt_arrG_Y->[$varColor]=int(0.299*_intG_R($varColor)+0.587*_intG_G($varColor)+0.114*_intG_B($varColor));
  }
  $intRet=$opt_arrG_Y->[$varColor];
  return($intRet);
}
# get u from color
our $opt_arrG_U=arr(); # U convert cache
sub _intG_U {
  my ($varColor)=@_;
  my $intRet;
  if (not(defined($opt_arrG_U->[$varColor]))) {
    $opt_arrG_U->[$varColor]=int(127.5+0.5*_intG_R($varColor)-0.418688*_intG_G($varColor)-0.081312*_intG_B($varColor));
  }
  $intRet=$opt_arrG_U->[$varColor];
  return($intRet);
}
# get v from color
our $opt_arrG_V=arr(); # V convert cache
sub _intG_V {
  my ($varColor)=@_;
  my $intRet;
  if (not(defined($opt_arrG_V->[$varColor]))) {
    $opt_arrG_V->[$varColor]=int(127.5+(-0.168736)*_intG_R($varColor)-0.331264*_intG_G($varColor)+0.5*_intG_B($varColor));
  }
  $intRet=$opt_arrG_V->[$varColor];
  return($intRet);
}
# get u from color
our $opt_arrG_u=arr(); # u convert cache
sub _intG_u {
  my ($varColor)=@_;
  my $intRet;
  if (not(defined($opt_arrG_u->[$varColor]))) {
    $opt_arrG_u->[$varColor]=int(0.5*_intG_R($varColor)+0.418688*_intG_G($varColor)+0.081312*_intG_B($varColor));
  }
  $intRet=$opt_arrG_u->[$varColor];
  return($intRet);
}
# get v from color
our $opt_arrG_v=arr(); # v convert cache
sub _intG_v {
  my ($varColor)=@_;
  my $intRet;
  if (not(defined($opt_arrG_v->[$varColor]))) {
    $opt_arrG_v->[$varColor]=int(0.168736*_intG_R($varColor)+0.331264*_intG_G($varColor)+0.5*_intG_B($varColor));
  }
  $intRet=$opt_arrG_v->[$varColor];
  return($intRet);
}
# get minimum grey part
sub _intG_Gm { 
  my ($varColor)=@_;
  my $intRet;
  my $intC_R=_intG_R($varColor);
  my $intC_G=_intG_G($varColor);
  my $intC_B=_intG_B($varColor);
  if (($intC_R<=$intC_G) and ($intC_R<=$intC_B)) {
    $intRet=$intC_R;
  } elsif (($intC_G<=$intC_R) and ($intC_G<=$intC_B)) {
    $intRet=$intC_G;
  } else {
    $intRet=$intC_B;
  }
  return($intRet);
}
# get maximum grey part
sub _intG_GM { 
  my ($varColor)=@_;
  my $intRet;
  my $intC_R=_intG_R($varColor);
  my $intC_G=_intG_G($varColor);
  my $intC_B=_intG_B($varColor);
  if (($intC_R>=$intC_G) and ($intC_R>=$intC_B)) {
    $intRet=$intC_R;
  } elsif (($intC_G>=$intC_R) and ($intC_G>=$intC_B)) {
    $intRet=$intC_G;
  } else {
    $intRet=$intC_B;
  }
  return($intRet);
}
# get brightness from color
our $opt_arrG_Brightness=arr(); # Brightness convert cache
sub _intG_Brightness {
  my ($varColor)=@_;
  my $intRet;
  if (not(defined($opt_arrG_Brightness->[$varColor]))) {
    $opt_arrG_Brightness->[$varColor]=_intG_R($varColor)*3+_intG_G($varColor)*3+_intG_B($varColor)*2;
  }
  $intRet=$opt_arrG_Brightness->[$varColor];
  return($intRet);
}
# get H from color
sub _intG_H {
  my ($varColor)=@_;
  my $intRet;
  my $intR=_intG_R($varColor);
  my $intG=_intG_G($varColor);
  my $intB=_intG_B($varColor);
  my $intMax=($intR>$intG and $intR>$intB)?$intR:($intG>$intB)?$intG:$intB;
  my $intMin=($intR<$intG and $intR<$intB)?$intR:($intG<$intB)?$intG:$intB;
  if ($intMin==$intMax) {
    $intRet=0;
  } elsif ($intMax=$intR) {
    $intRet=60* (0+($intG-$intB)/($intMax-$intMin));
  } elsif ($intMax=$intG) {
    $intRet=60* (2+($intB-$intR)/($intMax-$intMin));
  } elsif ($intMax=$intB) {
    $intRet=60* (4+($intR-$intG)/($intMax-$intMin));
  }
  if ($intRet<0) { $intRet+=360;};
  $intRet=intInt($intRet*255/360);
  return($intRet);
}
# discompare YUV against threshold
# thresholds for comparisons
our $intTrY=48;
our $intTrU=7;
our $intTrV=6;
our $opt_hashYUV_NE=hash();
sub _boolYUV_NE {
  my ($varC1,$varC2)=@_;
  my $boolRet;
  my $szI=sprintf('%d-%d',$varC1,$varC2);
  if (not(defined($opt_hashYUV_NE->{$szI}))) { 
    if (
      (abs(_intG_Y($varC1)-_intG_Y($varC2))>$intTrY) or 
      (abs(_intG_U($varC1)-_intG_U($varC2))>$intTrU) or 
      (abs(_intG_V($varC1)-_intG_V($varC2))>$intTrV)
    ) {
      $opt_hashYUV_NE->{$szI}=boolTRUE();
    } else {
      $opt_hashYUV_NE->{$szI}=boolFALSE();
    }
  }
  $boolRet=$opt_hashYUV_NE->{$szI};
  return($boolRet);
}
# compare YUV against threshold
sub _boolYUV_E {
  my ($varC1,$varC2)=@_;
  my $boolRet=( not ( _boolYUV_NE($varC1,$varC2) ) );
  return($boolRet);
}
# compare colors
sub _boolE {
  my ($varColor1,$varColor2)=@_;
  return($varColor1==$varColor2);
}
# discompare colors
sub _boolNE {
  my ($varColor1,$varColor2)=@_;
  return($varColor1!=$varColor2);
}
# interpolate 2 colors
our $opt_hashInterp2=hash();
sub _varInterp2 {
  my ($varCol1,$varCol2,$intW1,$intW2)=@_;
  my $varRet;
  my $szI=sprintf('%d;%d;%d;%d',$varCol1,$intW1,$varCol2,$intW2);
  if (not(defined($opt_hashInterp2->{$szI}))) {
    my $intW=$intW1+$intW2;
    my $floatW1=$intW1/$intW;
    my $floatW2=$intW2/$intW;
    $opt_hashInterp2->{$szI}=_varC_RGB(
      _intG_R($varCol1)*$floatW1+_intG_R($varCol2)*$floatW2,
      _intG_G($varCol1)*$floatW1+_intG_G($varCol2)*$floatW2,
      _intG_B($varCol1)*$floatW1+_intG_B($varCol2)*$floatW2
    );
  }
  $varRet=$opt_hashInterp2->{$szI};
  return($varRet);
}
# interpolate 3 colors
our $opt_hashInterp3=hash();
sub _varInterp3 {
  my ($varCol1,$varCol2,$varCol3,$intW1,$intW2,$intW3)=@_;
  my $varRet;
  my $szI=sprintf('%d;%d;%d;%d;%d;%d',$varCol1,$intW1,$varCol2,$intW2,$varCol3,$intW3);
  if (not(defined($opt_hashInterp3->{$szI}))) {
    my $intW=$intW1+$intW2+$intW3;
    my $floatW1=$intW1/$intW;
    my $floatW2=$intW2/$intW;
    my $floatW3=$intW3/$intW;
    $opt_hashInterp3->{$szI}=_varC_RGB(
      _intG_R($varCol1)*$floatW1+_intG_R($varCol2)*$floatW2+_intG_R($varCol3)*$floatW3,
      _intG_G($varCol1)*$floatW1+_intG_G($varCol2)*$floatW2+_intG_G($varCol3)*$floatW3,
      _intG_B($varCol1)*$floatW1+_intG_B($varCol2)*$floatW2+_intG_B($varCol3)*$floatW3
    );
  }
  $varRet=$opt_hashInterp3->{$szI};
  return($varRet);
}
# interpolate 4 colors
our $opt_hashInterp4=hash();
sub _varInterp4 {
  my ($varCol1,$varCol2,$varCol3,$varCol4,$intW1,$intW2,$intW3,$intW4)=@_;
  my $varRet;
  my $szI=sprintf('%d;%d;%d;%d;%d;%d;%d;%d',$varCol1,$intW1,$varCol2,$intW2,$varCol3,$intW3,$varCol4,$intW4);
  if (not(defined($opt_hashInterp4->{$szI}))) {
    my $intW=$intW1+$intW2+$intW3+$intW4;
    my $floatW1=$intW1/$intW;
    my $floatW2=$intW2/$intW;
    my $floatW3=$intW3/$intW;
    my $floatW4=$intW4/$intW;
    $opt_hashInterp4->{$szI}=_varC_RGB(
      _intG_R($varCol1)*$floatW1+_intG_R($varCol2)*$floatW2+_intG_R($varCol3)*$floatW3+_intG_R($varCol4)*$floatW4,
      _intG_G($varCol1)*$floatW1+_intG_G($varCol2)*$floatW2+_intG_G($varCol3)*$floatW3+_intG_G($varCol4)*$floatW4,
      _intG_B($varCol1)*$floatW1+_intG_B($varCol2)*$floatW2+_intG_B($varCol3)*$floatW3+_intG_B($varCol4)*$floatW4
    );
  }
  $varRet=$opt_hashInterp4->{$szI};
  return($varRet);
}
# used for 2xSaI, Super Eagle, Super 2xSaI
sub _intConc2d {
  my ($varColA,$varColB,$varColC,$varColD)=@_;
  my $intRet;
  my $boolAC=_boolE($varColA,$varColC);
  my $intX1=$boolAC?1:0;
  my $intY1=(_boolE($varColB,$varColC) and !($boolAC))?1:0;
  my $boolAD=_boolE($varColA,$varColD);
  my $intX2=$boolAD?1:0;
  my $intY2=(_boolE($varColB,$varColD) and !($boolAD))?1:0;
  my $intX=$intX1+$intX2;
  my $intY=$intY1+$intY2;
  $intRet=[[0,0,-1],[0,0,-1],[1,1,0]]->[$intY]->[$intX];
  return($intRet);
}
#                                                                              #
#                     -=[ P U B L I C   M E T H O D S ]=-                      #
#                                                                              #
sub voidImportRaw {
  my ($objThis,$ptrSub)=@_;
  my $arrImg=$objThis->arrGetRawImage();
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  for my $intY(0..$intHeight-1) {
    my $arrLine=$arrImg->[$intY];
    for my $intX(0..$intWidth-1) {
      $arrLine->[$intX]=&{$ptrSub}($intX,$intY);
    }
  }
  return($objThis);
}

sub voidExportRaw {
  my ($objThis,$ptrSub)=@_;
  my $arrImg=$objThis->arrGetRawImage();
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  for my $intY(0..$intHeight-1) {
    my $arrLine=$arrImg->[$intY];
    for my $intX(0..$intWidth-1) {
      &{$ptrSub}($intX,$intY,$arrLine->[$intX]);
    }
  }
  return($objThis);
}
#                                                                              #
#                  -=[ P U B L I C   P R O P E R T I E S ]=-                   #
#                                                                              #
# load using TK
sub objLoad_ByTK {
  my ($szFile)=@_;
  my $objRet;
  require Tk;
  my $objMainWindow=Tk::MainWindow->new();
  my $objTKPhoto=$objMainWindow->Photo();
  $objTKPhoto->read($szFile);
  $objRet=classImage->new($objTKPhoto->width(),$objTKPhoto->height());
  $objRet->voidImportRaw(sub {
    return(classImage::_varC_RGB(@{$objTKPhoto->get(@_)}));
  });
  $objTKPhoto=varUNDEF();
  $objMainWindow->destroy();
  $objMainWindow=varUNDEF();
  return($objRet);
}

# save using TK
sub voidSave_ByTK {
  my ($objThis,$szFile,$opt_szFormat)=@_;
  if (boolNot(boolIsDefined($opt_szFormat))) {
    $opt_szFormat= szLCase(modGlobal::szExtractExtension($szFile));
  }
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  require Tk;
  my $objMainWindow=Tk::MainWindow->new();
  my $objTKPhoto=$objMainWindow->Photo(
    '-width'=>$intWidth,
    '-height'=>$intHeight
  );
  my $arrRawImage=$objThis->arrGetRawImage();
  my $arrPixMap=[];
  for my $intY(0..$intHeight-1) {
    my $arrLine=$arrRawImage->[$intY];
    my $arrPixLine=[];
    for my $intX(0..$intWidth-1) {
      $arrPixLine->[$intX]=sprintf(
        '#%06X',
        $arrLine->[$intX]
      );
    }
    $arrPixMap->[$intY]=$arrPixLine;
    #unpack('(A7)*','#'.join('#',unpack('(xH6)*',pack('(N)*',@{$arrLine}))));
    #unpack('(A7)*',sprintf('#%06X' x $intWidth,@{$arrLine}));
  }
  $objTKPhoto->put($arrPixMap);
  $objTKPhoto->write($szFile,'-format'=>$opt_szFormat);
  $objTKPhoto=varUNDEF();
  $objMainWindow->destroy();
  $objMainWindow=varUNDEF();
}

# get red component
sub intGetR {
  my ($objThis,$varColor)=@_;
  my $intRet=_intG_R($varColor);
  return ($intRet);
}

# get green component
sub intGetG {
  my ($objThis,$varColor)=@_;
  my $intRet=_intG_G($varColor);
  return ($intRet);
}

# get blue component
sub intGetB {
  my ($objThis,$varColor)=@_;
  my $intRet=_intG_B($varColor);
  return ($intRet);
}

# create a color entry
sub varCreateColorRGB {
  my ($objThis,$intR,$intG,$intB)=@_;
  my $varColor=_varC_RGB($intR,$intG,$intB);
  return ($varColor);
}

# apply image magnifier/minifier filter
sub objFilterImage {
  my ($objThis,$szFilter)=@_;
  my $objRet;
  if (boolFilterExists($szFilter)) {
    my $arrFilter=varHashItem($_hashFilters,$szFilter);
    # progress
    my $ptrFilter=varArrayItem($arrFilter,0);
    my $intScaleX=varArrayItem($arrFilter,1);
    my $intScaleY=varArrayItem($arrFilter,2);
    
    my $intW=$objThis->intGetWidth();
    my $intH=$objThis->intGetHeight();
  
    # Destination image
    $objRet=classImage->new($intW*$intScaleX,$intH*$intScaleY);
    my $intDY=0;
    for my $intY(0..$intH-1) {
      my $intDX=0;
      for my $intX(0..$intW-1) {
        # call filter
        &{$ptrFilter}($objThis,$intX,$intY,$objRet,$intDX,$intDY,$szFilter,$intScaleX,$intScaleY);
        $intDX+=$intScaleX;
      }
      $intDY+=$intScaleY;
    }
  } else {
    $objRet=classImage->new(0,0);
    $objRet->voidSetAttribute('szErrorMessage',szPrintf('Filter "%s" not found',arr($szFilter)));
  }
  return($objRet);
}

# check whether a filter exists
sub boolFilterExists {
  my ($szFilter)=@_;
  my $boolRet=boolIsDefined(varHashItem($_hashFilters,$szFilter));
  return($boolRet);
}

# get filter list
sub arrGetFilters {
  my $arrRet=arrGetKeys($_hashFilters);
  return($arrRet);
}

# generate image difference
sub objDifference {
  my ($objThis,$objImage)=@_;
  my $objRet=classImage->new($objThis->intGetWidth(),$objThis->intGetHeight());
  $objRet->voidImportRaw(sub{
    my ($intX,$intY)=@_;
    my $varC1=$objThis->varGetPixel($intX,$intY);
    my $varC2=$objImage->varGetPixel($intX,$intY);
    my $varC=_varC_RGB(
      intAbs(_intG_R($varC1)-_intG_R($varC2)),
      intAbs(_intG_G($varC1)-_intG_G($varC2)),
      intAbs(_intG_B($varC1)-_intG_B($varC2))
    );
    return ($varC);
  });
  return($objRet);
}

# simple nearest neigbour
sub objRescale {
  my ($objThis,$intW,$intH)=@_;
  my $objRet=classImage->new($intW,$intH);
  my $floatXf=$intW/$objThis->intGetWidth();
  my $floatYf=$intH/$objThis->intGetHeight();
  $objRet->voidImportRaw(sub{
    my ($intX,$intY)=@_;
    my $varC=$objThis->varGetPixel(intInt($intX/$floatXf+0.5),intInt($intY/$floatYf+0.5));
    return ($varC);
  });
  return($objRet);
}

# bilinear redither
sub objRedither_BiLinear {
  my ($objThis,$intW,$intH)=@_;
  my $objRet=classImage->new($intW,$intH);
  my $floatXf=$intW/$objThis->intGetWidth();
  my $floatYf=$intH/$objThis->intGetHeight();
  $objRet->voidImportRaw(sub{
    my ($intX,$intY)=@_;
    my $varColor=$objThis->varGetPixel(
      intInt($intX/$floatXf+1* (($intY % 2)* ($intX % 2))+1* ((1-($intY % 2))* (1-($intX % 2)))),
      intInt($intY/$floatYf)
    );
    return ($varColor);
  });
  return($objRet);
}

# bilinear resize
sub objResample_BiLinear {
  my ($objThis,$intW,$intH)=@_;
  my $objRet=classImage->new($intW,$intH);
  my $floatXf=$intW/$objThis->intGetWidth();
  my $floatYf=$intH/$objThis->intGetHeight();
  $objRet->voidImportRaw(sub{
    my ($intX,$intY)=@_;
    # calc influence and source points
    my $floatFactorX=($intX/$floatXf);
    my $floatFactorY=($intY/$floatYf);
    my $intRealX=intInt($floatFactorX);
    my $intRealY=intInt($floatFactorY);
    $floatFactorX-=$intRealX;
    $floatFactorY-=$intRealY;
    my $floatInflA=(1-$floatFactorX)*(1-$floatFactorY);
    my $floatInflB=($floatFactorX)*(1-$floatFactorY);
    my $floatInflC=(1-$floatFactorX)*($floatFactorY);
    my $floatInflD=($floatFactorX)*($floatFactorY);
    # get all 4 source points
    my $varC00=$objThis->varGetPixel($intRealX,$intRealY);
    my $varC01=$objThis->varGetPixel($intRealX+1,$intRealY);
    my $varC10=$objThis->varGetPixel($intRealX,$intRealY+1);
    my $varC11=$objThis->varGetPixel($intRealX+1,$intRealY+1);
    
    # calc colours based on influence
    my $varColor=_varInterp4($varC00,$varC01,$varC10,$varC11,$floatInflA,$floatInflB,$floatInflC,$floatInflD);
    return ($varColor);
  });
  return($objRet);
}

# trilinear resize TODO: fix the shadows
sub objResample_TriLinear {
  my ($objThis,$intW,$intH)=@_;
  my $objRet=classImage->new($intW,$intH);
  my $floatXf=$intW/$objThis->intGetWidth();
  my $floatYf=$intH/$objThis->intGetHeight();
  $objRet->voidImportRaw(sub{
    my ($intX,$intY)=@_;
    # calc influence and source points
    my $floatFactorX=($intX/$floatXf);
    my $floatFactorY=($intY/$floatYf);
    my $intRealX=intInt($floatFactorX);
    my $intRealY=intInt($floatFactorY);
    $floatFactorX-=$intRealX;
    $floatFactorY-=$intRealY;
    my $floatInflA=(1-$floatFactorX)*(1-$floatFactorY);
    my $floatInflB=($floatFactorX)*(1-$floatFactorY);
    my $floatInflC=(1-$floatFactorX)*($floatFactorY);
    my $floatInflD=($floatFactorX)*($floatFactorY);
    # get all 4 source points
    my $varC00=$objThis->varGetPixel($intRealX,$intRealY);
    my $varC01=$objThis->varGetPixel($intRealX+1,$intRealY);
    my $varC10=$objThis->varGetPixel($intRealX,$intRealY+1);
    my $varC11=$objThis->varGetPixel($intRealX+1,$intRealY+1);
    # get nearest neigbour
    my $varC=$objThis->varGetPixel(intInt($intX/$floatXf+0.5),intInt($intY/$floatYf+0.5));
    
    # calc colours based on influence
    my $varColor=_varInterp4($varC00,$varC01,$varC10,$varC11,$floatInflA,$floatInflB,$floatInflC,$floatInflD);
    return (_varInterp2($varC,$varColor,1,2));
  });
  return($objRet);
}

# split image to R,G and B greyscale
sub arr_objSplitRGB {
  my ($objThis)=@_;
  my $arrRet=arr();
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  my $objImage_R=classImage->new($intWidth,$intHeight);
  my $objImage_G=classImage->new($intWidth,$intHeight);
  my $objImage_B=classImage->new($intWidth,$intHeight);
  for (my $intY=0;$intY<$intHeight;$intY++) {
    for (my $intX=0;$intX<$intWidth;$intX++) {
      my $varC=$objThis->varGetPixel($intX,$intY);
      my $intC_R=_intG_R($varC);
      my $intC_G=_intG_G($varC);
      my $intC_B=_intG_B($varC);
      $objImage_R->voidSetPixel($intX,$intY,_varC_RGB($intC_R,$intC_R,$intC_R));
      $objImage_G->voidSetPixel($intX,$intY,_varC_RGB($intC_G,$intC_G,$intC_G));
      $objImage_B->voidSetPixel($intX,$intY,_varC_RGB($intC_B,$intC_B,$intC_B));
    }
  }
  voidPush($arrRet,$objImage_R);
  voidPush($arrRet,$objImage_G);
  voidPush($arrRet,$objImage_B);
  return($arrRet);
}

# split image to Y,U and V greyscale
sub arr_objSplitYUV {
  my ($objThis)=@_;
  my $arrRet=arr();
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  my $objImage_Y=classImage->new($intWidth,$intHeight);
  my $objImage_U=classImage->new($intWidth,$intHeight);
  my $objImage_V=classImage->new($intWidth,$intHeight);
  for (my $intY=0;$intY<$intHeight;$intY++) {
    for (my $intX=0;$intX<$intWidth;$intX++) {
      my $varC=$objThis->varGetPixel($intX,$intY);
      my $intC_Y=_intG_Y($varC);
      my $intC_U=_intG_U($varC);
      my $intC_V=_intG_V($varC);
      $objImage_Y->voidSetPixel($intX,$intY,_varC_RGB($intC_Y,$intC_Y,$intC_Y));
      $objImage_U->voidSetPixel($intX,$intY,_varC_RGB($intC_U,$intC_U,$intC_U));
      $objImage_V->voidSetPixel($intX,$intY,_varC_RGB($intC_V,$intC_V,$intC_V));
    }
  }
  voidPush($arrRet,$objImage_Y);
  voidPush($arrRet,$objImage_U);
  voidPush($arrRet,$objImage_V);
  return($arrRet);
}

# split image to Y,u and v greyscale
sub arr_objSplitYuv {
  my ($objThis)=@_;
  my $arrRet=arr();
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  my $objImage_Y=classImage->new($intWidth,$intHeight);
  my $objImage_u=classImage->new($intWidth,$intHeight);
  my $objImage_v=classImage->new($intWidth,$intHeight);
  for (my $intY=0;$intY<$intHeight;$intY++) {
    for (my $intX=0;$intX<$intWidth;$intX++) {
      my $varC=$objThis->varGetPixel($intX,$intY);
      my $intC_Y=_intG_Y($varC);
      my $intC_u=_intG_u($varC);
      my $intC_v=_intG_v($varC);
      $objImage_Y->voidSetPixel($intX,$intY,_varC_RGB($intC_Y,$intC_Y,$intC_Y));
      $objImage_u->voidSetPixel($intX,$intY,_varC_RGB($intC_u,$intC_u,$intC_u));
      $objImage_v->voidSetPixel($intX,$intY,_varC_RGB($intC_v,$intC_v,$intC_v));
    }
  }
  voidPush($arrRet,$objImage_Y);
  voidPush($arrRet,$objImage_u);
  voidPush($arrRet,$objImage_v);
  return($arrRet);
}

# split image to color and greyscale plane
sub arr_objSplitGmC {
  my ($objThis)=@_;
  my $arrRet=arr();
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  my $objImage_Y=classImage->new($intWidth,$intHeight);
  my $objImage_C=classImage->new($intWidth,$intHeight);
  for (my $intY=0;$intY<$intHeight;$intY++) {
    for (my $intX=0;$intX<$intWidth;$intX++) {
      my $varC=$objThis->varGetPixel($intX,$intY);
      my $intC_Y=_intG_Gm($varC);
      $objImage_Y->voidSetPixel($intX,$intY,_varC_RGB($intC_Y,$intC_Y,$intC_Y));
      $objImage_C->voidSetPixel($intX,$intY,_varC_RGB(_intG_R($varC)-$intC_Y,_intG_G($varC)-$intC_Y,_intG_B($varC)-$intC_Y));
    }
  }
  voidPush($arrRet,$objImage_Y);
  voidPush($arrRet,$objImage_C);
  return($arrRet);
}

# split image to color and greyscale plane
sub arr_objSplitGMC {
  my ($objThis)=@_;
  my $arrRet=arr();
  my $intWidth=$objThis->intGetWidth();
  my $intHeight=$objThis->intGetHeight();
  my $objImage_Y=classImage->new($intWidth,$intHeight);
  my $objImage_C=classImage->new($intWidth,$intHeight);
  for (my $intY=0;$intY<$intHeight;$intY++) {
    for (my $intX=0;$intX<$intWidth;$intX++) {
      my $varC=$objThis->varGetPixel($intX,$intY);
      my $intC_Y=_intG_GM($varC);
      $objImage_Y->voidSetPixel($intX,$intY,_varC_RGB($intC_Y,$intC_Y,$intC_Y));
      $objImage_C->voidSetPixel($intX,$intY,_varC_RGB($intC_Y-_intG_R($varC),$intC_Y-_intG_G($varC),$intC_Y-_intG_B($varC)));
    }
  }
  voidPush($arrRet,$objImage_Y);
  voidPush($arrRet,$objImage_C);
  return($arrRet);
}

sub varGetPixel {
  my ($objThis,$intX,$intY)=@_;
  my $intW= $objThis->{'_intWidth'};
  my $intH= $objThis->{'_intHeight'};
  if ($intX<0) { $intX=0 };
  if ($intY<0) { $intY=0 };
  if ($intX>=$intW) { $intX=$intW-1 };
  if ($intY>=$intH) { $intY=$intH-1 };
  return($objThis->{'_arrImgData'}->[$intY]->[$intX]);
}

sub voidSetPixel {
  my ($objThis,$intX,$intY,$varColor)=@_;
  my $intW= $objThis->{'_intWidth'};
  my $intH= $objThis->{'_intHeight'};
  if (not( ($intX<0) or ($intY<0) or ($intX>=$intW) or ($intY>=$intH))) {
    $objThis->{'_arrImgData'}->[$intY]->[$intX]=$varColor;
  } 
  return($objThis);
}

sub arrGetRawImage {
  my ($objThis)=@_;
  return($objThis->{'_arrImgData'});
}

sub intGetWidth {
  my ($objThis)=@_;
  return($objThis->{'_intWidth'});
}

sub intGetHeight {
  my ($objThis)=@_;
  return($objThis->{'_intHeight'});
}
#                                                                              #
#-------------------------------CASEPATHS FOR LQnX/HQnX------------------------#
#                                                                              #
my $szFile= modGlobal::szLookupPath('Projects/ImageResizer/src/AdvMAME/output.pl');
if (defined($szFile)) {
  eval(modGlobal::szReadFileBinary($szFile));
} else {
  voidWarn('could not find filter output file');
}

#                                                                              #
#                                                                              #
#------------------------------------------------------------------------------#
#                                                                              #
#                                                                              #
#                    -=[ S E L F - L O A D E R ]=-                             #
#                                                                              #
#                                                                              #
sub voidSelfLoad {
  my ($arrArguments)=\@_;
  my $szArgumentList=szPrintf(' %s ',arr(szJoin(' ', $arrArguments)));
  if ( boolMatchRegEx( $szArgumentList,'/[\s]((\/\?)|(\-help)|(\-\-help))[\s]/i' ) or (intArrLen($arrArguments)<1) ) {
    my $szHelp=szJoin($modGlobal::szLE,arr(
      'classImage.pm [/?] [-help] [--help] [<options>]',
      ' image class',
      '        /? | -help | --help  : shows this help',
      '',''
    ));
    voidWarn($szHelp,'Help');
  }
  else {
    # initialize working environment
    &voidProcess(@{$arrArguments});
  }
}

sub voidProcess {
  voidSelfLoad();
}

if ($0 eq __FILE__) {
  # Self Loader
  &voidSelfLoad(@ARGV);
} else {
  return (1==1);
}