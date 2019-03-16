#!/usr/bin/perl -w
#===============================================================================
#
#         FILE:  classImage.pm
#
#  DESCRIPTION:  image class
#
#        FILES:  ---
#         BUGS:  ---
#        NOTES:  ---
#       AUTHOR:  »SynthelicZ« Hawkynt
#      COMPANY:  »SynthelicZ«
#      VERSION:  1.000
#      CREATED:  09.11.2007 08:49:29 CET
#     REVISION:  ---
#===============================================================================
package classImage;
use base 'Exporter';              # we could export if we want to

use strict;
use warnings;

use Modules::modIL;               # independant language library
use Modules::modGlobal;           # everyday routines for programmers
use Classes::classObject;         # to create new classes

use base 'classObject';           # not needed if other parents !

our @EXPORT_OK = qw();            # we don't want to export by default
our @EXPORT = qw();               # we never export anything
our $VERSION=1.000;          # yes thats our applications version

our $szLE=$modGlobal::szLE;       # simple line-ending
our $_varMissingCol=_varC_RGB(255,0,255);
our $_hashFilters=hash(
  'LQ2X',arr(\&_voidComplex_LQ2X,2,2),
  'HQ2X',arr(\&_voidComplex_HQ2X,2,2),
  'HQ3X',arr(\&_voidComplex_HQ3X,3,3),
  'Eagle',arr(\&_voidComplex_Eagle,2,2),
  'Eagle3X',arr(\&_voidComplex_Eagle3X,3,3),
  'SuperEagle',arr(\&_voidComplex_SuperEagle,2,2),
  'SaI2X',arr(\&_voidComplex_SaI2X,2,2),
  'SuperSaI2X',arr(\&_voidComplex_SuperSaI2X,2,2),
  'Scale2X',arr(\&_voidComplex_Scale2X,2,2),
  'Scale3X',arr(\&_voidComplex_Scale3X,3,3),
  'AdvInterp2X',arr(\&_voidComplex_AdvInterp2X,2,2),
  'AdvInterp3X',arr(\&_voidComplex_AdvInterp3X,3,3),
  'Interp2X',arr(\&_voidComplex_Interp2X,2,2),
  'Scan2X',arr(\&_voidComplex_Scan2X,2,2),
  'Scan3X',arr(\&_voidComplex_Scan3X,3,3),
  'RGB2X',arr(\&_voidComplex_RGB2X,2,2),
  'RGB3X',arr(\&_voidComplex_RGB3X,3,3),
  'TV2X',arr(\&_voidComplex_TV2X,2,2),
  'HawkyntTV2X',arr(\&_voidComplex_HawkyntTV2X,2,2),
  'HawkyntTV3X',arr(\&_voidComplex_HawkyntTV3X,3,2),
  'TV3X',arr(\&_voidComplex_TV3X,3,3),
  'NormalDH',arr(\&_voidComplex_NormalDH,1,2),
  'NormalDW',arr(\&_voidComplex_NormalDW,2,1),
  'Normal1X',arr(\&_voidComplex_Normal1X,1,1),
  'Normal2X',arr(\&_voidComplex_Normal2X,2,2),
  'Normal3X',arr(\&_voidComplex_Normal3X,3,3),
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
sub _voidComplex_Normal1X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
}

sub _voidComplex_Normal2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
}

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

sub _voidComplex_NormalDW {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
}

sub _voidComplex_NormalDH {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
}

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

sub _voidComplex_RGB2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX+1,$intDY,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,_intG_B($varC4)));
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
}

sub _voidComplex_HawkyntTV2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $intI=_intG_Y($varC4);
  $objRet->voidSetPixel($intDX,$intDY,_varC_RGB(_intG_R($varC4),0,0));
  $objRet->voidSetPixel($intDX+1,$intDY,_varC_RGB(0,_intG_G($varC4),0));
  $objRet->voidSetPixel($intDX,$intDY+1,_varC_RGB(0,0,_intG_B($varC4)));
  $objRet->voidSetPixel($intDX+1,$intDY+1,_varC_RGB($intI,$intI,$intI));
}

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
# Hawkynt's bilinear interpolate (my idea is possibly used already, but to separate here...)
sub _voidComplex_Interp2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  my $varC8=$objThis->varGetPixel($intX+1,$intY+1);
  
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
  $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
  $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC4,$varC7,1,1));
  $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp4($varC4,$varC5,$varC7,$varC8,1,1,1,1));
}

sub _voidComplex_AdvInterp2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  
  if (_boolNE($varC1,$varC7) and _boolNE($varC3,$varC5)) {
    $objRet->voidSetPixel($intDX,$intDY,
      _boolE($varC3,$varC1)
      ?_varInterp2($varC3,$varC4,5,3)
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY,
      _boolE($varC1,$varC5)
      ?_varInterp2($varC5,$varC4,5,3)
      :$varC4
    );
    $objRet->voidSetPixel($intDX,$intDY+1,
      _boolE($varC3,$varC7)
      ?_varInterp2($varC3,$varC4,5,3)
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY+1,
      _boolE($varC7,$varC5)
      ?_varInterp2($varC5,$varC4,5,3)
      :$varC4
    );
  } else {
    $objRet->voidSetPixel($intDX,$intDY,$varC4);
    $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
    $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
  }
}

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
  
  if (_boolNE($varC1,$varC7) and _boolNE($varC3,$varC5)) {
    $objRet->voidSetPixel($intDX,$intDY,
      _boolE($varC3,$varC1)
      ?_varInterp2($varC3,$varC4,5,3)
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY,
      (
        (_boolE($varC3,$varC1) and _boolNE($varC4,$varC2))or
        (_boolE($varC5,$varC1) and _boolNE($varC4,$varC0))
      )
      ?$varC1
      :$varC4
    );
    $objRet->voidSetPixel($intDX+2,$intDY,
      _boolE($varC1,$varC5)
      ?_varInterp2($varC5,$varC4,5,3)
      :$varC4
    );
    
    $objRet->voidSetPixel($intDX,$intDY+1,
      (
        (_boolE($varC3,$varC1) and _boolNE($varC4,$varC6))or
        (_boolE($varC3,$varC7) and _boolNE($varC4,$varC0))
      )
      ?$varC3
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
    $objRet->voidSetPixel($intDX+2,$intDY+1,
      (
        (_boolE($varC5,$varC1) and _boolNE($varC4,$varC8))or
        (_boolE($varC5,$varC7) and _boolNE($varC4,$varC2))
      )
      ?$varC5
      :$varC4
    );
    
    $objRet->voidSetPixel($intDX,$intDY+2,
      _boolE($varC3,$varC7)
      ?_varInterp2($varC3,$varC4,5,3)
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY+2,
      (
        (_boolE($varC3,$varC7) and _boolNE($varC4,$varC8))or
        (_boolE($varC5,$varC7) and _boolNE($varC4,$varC6))
      )
      ?$varC7
      :$varC4
    );
    $objRet->voidSetPixel($intDX+2,$intDY+2,
      _boolE($varC7,$varC5)
      ?_varInterp2($varC5,$varC4,5,3)
      :$varC4
    );
  } else {
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
}
# Andrea Mazzoleni's Scale2X
sub _voidComplex_Scale2X {
  my ($objThis,$intX,$intY,$objRet,$intDX,$intDY)=@_;
  my $varC1=$objThis->varGetPixel($intX,$intY-1);
  my $varC3=$objThis->varGetPixel($intX-1,$intY);
  my $varC4=$objThis->varGetPixel($intX,$intY);
  my $varC5=$objThis->varGetPixel($intX+1,$intY);
  my $varC7=$objThis->varGetPixel($intX,$intY+1);
  
  if (_boolNE($varC1,$varC7) and _boolNE($varC3,$varC5)) {
    $objRet->voidSetPixel($intDX,$intDY,
      _boolE($varC3,$varC1)
      ?$varC3
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY,
      _boolE($varC1,$varC5)
      ?$varC5
      :$varC4
    );
    $objRet->voidSetPixel($intDX,$intDY+1,
      _boolE($varC3,$varC7)
      ?$varC3
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY+1,
      _boolE($varC7,$varC5)
      ?$varC5
      :$varC4
    );
  } else {
    $objRet->voidSetPixel($intDX,$intDY,$varC4);
    $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
    $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
  }
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
  
  if (_boolNE($varC1,$varC7) and _boolNE($varC3,$varC5)) {
    $objRet->voidSetPixel($intDX,$intDY,
      _boolE($varC3,$varC1)
      ?$varC3
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY,
      (
        (_boolE($varC3,$varC1) and _boolNE($varC4,$varC2))or
        (_boolE($varC5,$varC1) and _boolNE($varC4,$varC0))
      )
      ?$varC1
      :$varC4
    );
    $objRet->voidSetPixel($intDX+2,$intDY,
      _boolE($varC1,$varC5)
      ?$varC5
      :$varC4
    );
    
    $objRet->voidSetPixel($intDX,$intDY+1,
      (
        (_boolE($varC3,$varC1) and _boolNE($varC4,$varC6))or
        (_boolE($varC3,$varC7) and _boolNE($varC4,$varC0))
      )
      ?$varC3
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
    $objRet->voidSetPixel($intDX+2,$intDY+1,
      (
        (_boolE($varC5,$varC1) and _boolNE($varC4,$varC8))or
        (_boolE($varC5,$varC7) and _boolNE($varC4,$varC2))
      )
      ?$varC5
      :$varC4
    );
    
    $objRet->voidSetPixel($intDX,$intDY+2,
      _boolE($varC3,$varC7)
      ?$varC3
      :$varC4
    );
    $objRet->voidSetPixel($intDX+1,$intDY+2,
      (
        (_boolE($varC3,$varC7) and _boolNE($varC4,$varC8))or
        (_boolE($varC5,$varC7) and _boolNE($varC4,$varC6))
      )
      ?$varC7
      :$varC4
    );
    $objRet->voidSetPixel($intDX+2,$intDY+2,
      _boolE($varC7,$varC5)
      ?$varC5
      :$varC4
    );
  } else {
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
}
# standard LQ2X casepath
sub _arrLQ2X {
  my ($intPattern,
    $varC0,$varC1,$varC2,
    $varC3,$varC4,$varC5,
    $varC6,$varC7,$varC8
  )=@_;
  my $varE00; my $varE01;
  my $varE10; my $varE11;
  # BEGIN LQ2X PATTERNS
  if (
    ($intPattern==0) or
    ($intPattern==2) or
    ($intPattern==4) or
    ($intPattern==6) or
    ($intPattern==8) or
    ($intPattern==12) or
    ($intPattern==16) or
    ($intPattern==20) or
    ($intPattern==24) or
    ($intPattern==28) or
    ($intPattern==32) or
    ($intPattern==34) or
    ($intPattern==36) or
    ($intPattern==38) or
    ($intPattern==40) or
    ($intPattern==44) or
    ($intPattern==48) or
    ($intPattern==52) or
    ($intPattern==56) or
    ($intPattern==60) or
    ($intPattern==64) or
    ($intPattern==66) or
    ($intPattern==68) or
    ($intPattern==70) or
    ($intPattern==96) or
    ($intPattern==98) or
    ($intPattern==100) or
    ($intPattern==102) or
    ($intPattern==128) or
    ($intPattern==130) or
    ($intPattern==132) or
    ($intPattern==134) or
    ($intPattern==136) or
    ($intPattern==140) or
    ($intPattern==144) or
    ($intPattern==148) or
    ($intPattern==152) or
    ($intPattern==156) or
    ($intPattern==160) or
    ($intPattern==162) or
    ($intPattern==164) or
    ($intPattern==166) or
    ($intPattern==168) or
    ($intPattern==172) or
    ($intPattern==176) or
    ($intPattern==180) or
    ($intPattern==184) or
    ($intPattern==188) or
    ($intPattern==192) or
    ($intPattern==194) or
    ($intPattern==196) or
    ($intPattern==198) or
    ($intPattern==224) or
    ($intPattern==226) or
    ($intPattern==228) or
    ($intPattern==230) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
  } elsif (
    ($intPattern==1) or
    ($intPattern==5) or
    ($intPattern==9) or
    ($intPattern==13) or
    ($intPattern==17) or
    ($intPattern==21) or
    ($intPattern==25) or
    ($intPattern==29) or
    ($intPattern==33) or
    ($intPattern==37) or
    ($intPattern==41) or
    ($intPattern==45) or
    ($intPattern==49) or
    ($intPattern==53) or
    ($intPattern==57) or
    ($intPattern==61) or
    ($intPattern==65) or
    ($intPattern==69) or
    ($intPattern==97) or
    ($intPattern==101) or
    ($intPattern==129) or
    ($intPattern==133) or
    ($intPattern==137) or
    ($intPattern==141) or
    ($intPattern==145) or
    ($intPattern==149) or
    ($intPattern==153) or
    ($intPattern==157) or
    ($intPattern==161) or
    ($intPattern==165) or
    ($intPattern==169) or
    ($intPattern==173) or
    ($intPattern==177) or
    ($intPattern==181) or
    ($intPattern==185) or
    ($intPattern==189) or
    ($intPattern==193) or
    ($intPattern==197) or
    ($intPattern==225) or
    ($intPattern==229) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
  } elsif (
    ($intPattern==3) or
    ($intPattern==35) or
    ($intPattern==67) or
    ($intPattern==99) or
    ($intPattern==131) or
    ($intPattern==163) or
    ($intPattern==195) or
    ($intPattern==227) 
  ) {
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
  } elsif (
    ($intPattern==7) or
    ($intPattern==39) or
    ($intPattern==71) or
    ($intPattern==103) or
    ($intPattern==135) or
    ($intPattern==167) or
    ($intPattern==199) or
    ($intPattern==231) 
  ) {
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
  } elsif (
    ($intPattern==10) or
    ($intPattern==138) 
  ) {
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==11) or
    ($intPattern==27) or
    ($intPattern==75) or
    ($intPattern==139) or
    ($intPattern==155) or
    ($intPattern==203) 
  ) {
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
    } else {
      $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==14) or
    ($intPattern==142) 
  ) {
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
      $varE01 = $varC0;
    } else {
      $varE00 = _varInterp3($varC1,$varC3,$varC0,3,3,2);
      $varE01 = _varInterp2($varC0,$varC1,3,1);
    }
  } elsif (
    ($intPattern==15) or
    ($intPattern==143) or
    ($intPattern==207) 
  ) {
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
      $varE01 = $varC4;
    } else {
      $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
      $varE01 = _varInterp2($varC4,$varC1,3,1);
    }
  } elsif (
    ($intPattern==18) or
    ($intPattern==22) or
    ($intPattern==30) or
    ($intPattern==50) or
    ($intPattern==54) or
    ($intPattern==62) or
    ($intPattern==86) or
    ($intPattern==118) 
  ) {
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==19) or
    ($intPattern==51) 
  ) {
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE00 = $varC2;
      $varE01 = $varC2;
    } else {
      $varE00 = _varInterp2($varC2,$varC1,3,1);
      $varE01 = _varInterp3($varC1,$varC5,$varC2,3,3,2);
    }
  } elsif (
    ($intPattern==23) or
    ($intPattern==55) or
    ($intPattern==119) 
  ) {
    $varE10 = $varC3;
    $varE11 = $varC3;
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE00 = $varC3;
      $varE01 = $varC3;
    } else {
      $varE00 = _varInterp2($varC3,$varC1,3,1);
      $varE01 = _varInterp3($varC1,$varC5,$varC3,3,3,2);
    }
  } elsif (
    ($intPattern==26) 
  ) {
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==31) or
    ($intPattern==95) 
  ) {
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC4;
    } else {
      $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==42) or
    ($intPattern==170) 
  ) {
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
      $varE10 = $varC0;
    } else {
      $varE00 = _varInterp3($varC1,$varC3,$varC0,3,3,2);
      $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ($intPattern==43) or
    ($intPattern==171) or
    ($intPattern==187) 
  ) {
    $varE01 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
      $varE10 = $varC2;
    } else {
      $varE00 = _varInterp3($varC1,$varC3,$varC2,3,3,2);
      $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ($intPattern==46) or
    ($intPattern==174) 
  ) {
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ($intPattern==47) or
    ($intPattern==175) 
  ) {
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ($intPattern==58) or
    ($intPattern==154) or
    ($intPattern==186) 
  ) {
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==59) 
  ) {
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
    } else {
      $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC2;
    } else {
      $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==63) 
  ) {
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC4;
    } else {
      $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==72) or
    ($intPattern==76) or
    ($intPattern==104) or
    ($intPattern==106) or
    ($intPattern==108) or
    ($intPattern==110) or
    ($intPattern==120) or
    ($intPattern==124) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ($intPattern==73) or
    ($intPattern==77) or
    ($intPattern==105) or
    ($intPattern==109) or
    ($intPattern==125) 
  ) {
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE00 = $varC1;
      $varE10 = $varC1;
    } else {
      $varE00 = _varInterp2($varC1,$varC3,3,1);
      $varE10 = _varInterp3($varC3,$varC7,$varC1,3,3,2);
    }
  } elsif (
    ($intPattern==74) 
  ) {
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==78) or
    ($intPattern==202) or
    ($intPattern==206) 
  ) {
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ($intPattern==79) 
  ) {
    $varE01 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC4;
    } else {
      $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==80) or
    ($intPattern==208) or
    ($intPattern==210) or
    ($intPattern==216) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ($intPattern==81) or
    ($intPattern==209) or
    ($intPattern==217) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC1;
    } else {
      $varE11 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ($intPattern==82) or
    ($intPattern==214) or
    ($intPattern==222) 
  ) {
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==83) or
    ($intPattern==115) 
  ) {
    $varE00 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC2;
    } else {
      $varE11 = _varInterp3($varC2,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC2;
    } else {
      $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==84) or
    ($intPattern==212) 
  ) {
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE01 = $varC0;
      $varE11 = $varC0;
    } else {
      $varE01 = _varInterp2($varC0,$varC5,3,1);
      $varE11 = _varInterp3($varC5,$varC7,$varC0,3,3,2);
    }
  } elsif (
    ($intPattern==85) or
    ($intPattern==213) or
    ($intPattern==221) 
  ) {
    $varE00 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE01 = $varC1;
      $varE11 = $varC1;
    } else {
      $varE01 = _varInterp2($varC1,$varC5,3,1);
      $varE11 = _varInterp3($varC5,$varC7,$varC1,3,3,2);
    }
  } elsif (
    ($intPattern==87) 
  ) {
    $varE00 = $varC3;
    $varE10 = $varC3;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC3;
    } else {
      $varE11 = _varInterp3($varC3,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC3;
    } else {
      $varE01 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==88) or
    ($intPattern==248) or
    ($intPattern==250) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ($intPattern==89) or
    ($intPattern==93) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC1;
    } else {
      $varE10 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC1;
    } else {
      $varE11 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ($intPattern==90) 
  ) {
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==91) 
  ) {
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC2;
    } else {
      $varE10 = _varInterp3($varC2,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC2;
    } else {
      $varE11 = _varInterp3($varC2,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
    } else {
      $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC2;
    } else {
      $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==92) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ($intPattern==94) 
  ) {
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==107) or
    ($intPattern==123) 
  ) {
    $varE01 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC2;
    } else {
      $varE10 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
    } else {
      $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==111) 
  ) {
    $varE01 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC4;
    } else {
      $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ($intPattern==112) or
    ($intPattern==240) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE10 = $varC0;
      $varE11 = $varC0;
    } else {
      $varE10 = _varInterp2($varC0,$varC7,3,1);
      $varE11 = _varInterp3($varC5,$varC7,$varC0,3,3,2);
    }
  } elsif (
    ($intPattern==113) or
    ($intPattern==241) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE10 = $varC1;
      $varE11 = $varC1;
    } else {
      $varE10 = _varInterp2($varC1,$varC7,3,1);
      $varE11 = _varInterp3($varC5,$varC7,$varC1,3,3,2);
    }
  } elsif (
    ($intPattern==114) 
  ) {
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==116) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ($intPattern==117) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC1;
    } else {
      $varE11 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ($intPattern==121) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC1;
    } else {
      $varE10 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC1;
    } else {
      $varE11 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ($intPattern==122) 
  ) {
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==126) 
  ) {
    $varE00 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==127) 
  ) {
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC4;
    } else {
      $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC4;
    } else {
      $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==146) or
    ($intPattern==150) or
    ($intPattern==178) or
    ($intPattern==182) or
    ($intPattern==190) 
  ) {
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
      $varE11 = $varC0;
    } else {
      $varE01 = _varInterp3($varC1,$varC5,$varC0,3,3,2);
      $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ($intPattern==147) or
    ($intPattern==179) 
  ) {
    $varE00 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC2;
    } else {
      $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==151) or
    ($intPattern==183) 
  ) {
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC3;
    } else {
      $varE01 = _varInterp3($varC3,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ($intPattern==158) 
  ) {
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==159) 
  ) {
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC4;
    } else {
      $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ($intPattern==191) 
  ) {
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC4;
    } else {
      $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ($intPattern==200) or
    ($intPattern==204) or
    ($intPattern==232) or
    ($intPattern==236) or
    ($intPattern==238) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
      $varE11 = $varC0;
    } else {
      $varE10 = _varInterp3($varC3,$varC7,$varC0,3,3,2);
      $varE11 = _varInterp2($varC0,$varC7,3,1);
    }
  } elsif (
    ($intPattern==201) or
    ($intPattern==205) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC1;
    } else {
      $varE10 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
    }
  } elsif (
    ($intPattern==211) 
  ) {
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC2;
    } else {
      $varE11 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ($intPattern==215) 
  ) {
    $varE00 = $varC3;
    $varE10 = $varC3;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC3;
    } else {
      $varE11 = _varInterp3($varC3,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC3;
    } else {
      $varE01 = _varInterp3($varC3,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ($intPattern==218) 
  ) {
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==219) 
  ) {
    $varE01 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC2;
    } else {
      $varE11 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
    } else {
      $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==220) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ($intPattern==223) 
  ) {
    $varE10 = $varC4;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC4;
    } else {
      $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC4;
    } else {
      $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ($intPattern==233) or
    ($intPattern==237) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC1;
    } else {
      $varE10 = _varInterp3($varC1,$varC3,$varC7,14,1,1);
    }
  } elsif (
    ($intPattern==234) 
  ) {
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC0;
    } else {
      $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ($intPattern==235) 
  ) {
    $varE01 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC2;
    } else {
      $varE10 = _varInterp3($varC2,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
    } else {
      $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==239) 
  ) {
    $varE01 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC4;
    } else {
      $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ($intPattern==242) 
  ) {
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ($intPattern==243) 
  ) {
    $varE00 = $varC2;
    $varE01 = $varC2;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE10 = $varC2;
      $varE11 = $varC2;
    } else {
      $varE10 = _varInterp2($varC2,$varC7,3,1);
      $varE11 = _varInterp3($varC5,$varC7,$varC2,3,3,2);
    }
  } elsif (
    ($intPattern==244) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ($intPattern==245) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC1;
    } else {
      $varE11 = _varInterp3($varC1,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ($intPattern==246) 
  ) {
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==247) 
  ) {
    $varE00 = $varC3;
    $varE10 = $varC3;
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC3;
    } else {
      $varE11 = _varInterp3($varC3,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC3;
    } else {
      $varE01 = _varInterp3($varC3,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ($intPattern==249) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC1;
    } else {
      $varE10 = _varInterp3($varC1,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC1;
    } else {
      $varE11 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ($intPattern==251) 
  ) {
    $varE01 = $varC2;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC2;
    } else {
      $varE10 = _varInterp3($varC2,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC2;
    } else {
      $varE11 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC2;
    } else {
      $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ($intPattern==252) 
  ) {
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ($intPattern==253) 
  ) {
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC1;
    } else {
      $varE10 = _varInterp3($varC1,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC1;
    } else {
      $varE11 = _varInterp3($varC1,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ($intPattern==254) 
  ) {
    $varE00 = $varC0;
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC0;
    } else {
      $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC0;
    } else {
      $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC0;
    } else {
      $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ($intPattern==255) 
  ) {
    if (_boolYUV_NE($varC3,$varC7)) {
      $varE10 = $varC4;
    } else {
      $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC7)) {
      $varE11 = $varC4;
    } else {
      $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE($varC3,$varC1)) {
      $varE00 = $varC4;
    } else {
      $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5,$varC1)) {
      $varE01 = $varC4;
    } else {
      $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  }
  # END LQ2X PATTERNS
  return(
    $varE00,$varE01,
    $varE10,$varE11
  );
}
# standard HQ3X casepath
sub _arrHQ3X {
  my ($intPattern,
    $varC0,$varC1,$varC2,
    $varC3,$varC4,$varC5,
    $varC6,$varC7,$varC8
  )=@_;
  my $varE00; my $varE01; my $varE02;
  my $varE10; my $varE11; my $varE12;
  my $varE20; my $varE21; my $varE22;
  
  #BEGIN HQ3X PATTERNS
  if (
    ($intPattern==0) or
    ($intPattern==1) or
    ($intPattern==4) or
    ($intPattern==32) or
    ($intPattern==128) or
    ($intPattern==5) or
    ($intPattern==132) or
    ($intPattern==160) or
    ($intPattern==33) or
    ($intPattern==129) or
    ($intPattern==36) or
    ($intPattern==133) or
    ($intPattern==164) or
    ($intPattern==161) or
    ($intPattern==37) or
    ($intPattern==165)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==2) or
    ($intPattern==34) or
    ($intPattern==130) or
    ($intPattern==162)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==16) or
    ($intPattern==17) or
    ($intPattern==48) or
    ($intPattern==49)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==64) or
    ($intPattern==65) or
    ($intPattern==68) or
    ($intPattern==69)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==8) or
    ($intPattern==12) or
    ($intPattern==136) or
    ($intPattern==140)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==3) or
    ($intPattern==35) or
    ($intPattern==131) or
    ($intPattern==163)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==6) or
    ($intPattern==38) or
    ($intPattern==134) or
    ($intPattern==166)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==20) or
    ($intPattern==21) or
    ($intPattern==52) or
    ($intPattern==53)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==144) or
    ($intPattern==145) or
    ($intPattern==176) or
    ($intPattern==177)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==192) or
    ($intPattern==193) or
    ($intPattern==196) or
    ($intPattern==197)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==96) or
    ($intPattern==97) or
    ($intPattern==100) or
    ($intPattern==101)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==40) or
    ($intPattern==44) or
    ($intPattern==168) or
    ($intPattern==172)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==9) or
    ($intPattern==13) or
    ($intPattern==137) or
    ($intPattern==141)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==18) or
    ($intPattern==50)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=_varInterp2($varC4,$varC2,3,1);
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==80) or
    ($intPattern==81)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==72) or
    ($intPattern==76)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=_varInterp2($varC4,$varC6,3,1);
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==10) or
    ($intPattern==138)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==66)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==24)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==7) or
    ($intPattern==39) or
    ($intPattern==135)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==148) or
    ($intPattern==149) or
    ($intPattern==180)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==224) or
    ($intPattern==228) or
    ($intPattern==225)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==41) or
    ($intPattern==169) or
    ($intPattern==45)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==22) or
    ($intPattern==54)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==208) or
    ($intPattern==209)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==104) or
    ($intPattern==108)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==11) or
    ($intPattern==139)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==19) or
    ($intPattern==51)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE00=_varInterp2($varC4,$varC3,3,1);
      $varE01=$varC4;
      $varE02=_varInterp2($varC4,$varC2,3,1);
      $varE12=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
      $varE01=_varInterp2($varC1,$varC4,3,1);
      $varE02=_varInterp2($varC1,$varC5,1,1);
      $varE12=_varInterp2($varC4,$varC5,3,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==146) or
    ($intPattern==178)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=_varInterp2($varC4,$varC2,3,1);
      $varE12=$varC4;
      $varE22=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE02=_varInterp2($varC1,$varC5,1,1);
      $varE12=_varInterp2($varC5,$varC4,3,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==84) or
    ($intPattern==85)
  )
  {
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE02=_varInterp2($varC4,$varC1,3,1);
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
      $varE12=_varInterp2($varC5,$varC4,3,1);
      $varE21=_varInterp2($varC4,$varC7,3,1);
      $varE22=_varInterp2($varC5,$varC7,1,1);
    }
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    
  }
  elsif (
    ($intPattern==112) or
    ($intPattern==113)
  )
  {
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE20=_varInterp2($varC4,$varC3,3,1);
      $varE21=$varC4;
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,3,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
      $varE21=_varInterp2($varC7,$varC4,3,1);
      $varE22=_varInterp2($varC5,$varC7,1,1);
    }
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    
  }
  elsif (
    ($intPattern==200) or
    ($intPattern==204)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=_varInterp2($varC4,$varC6,3,1);
      $varE21=$varC4;
      $varE22=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,3,1);
      $varE20=_varInterp2($varC7,$varC3,1,1);
      $varE21=_varInterp2($varC7,$varC4,3,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==73) or
    ($intPattern==77)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE00=_varInterp2($varC4,$varC1,3,1);
      $varE10=$varC4;
      $varE20=_varInterp2($varC4,$varC6,3,1);
      $varE21=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
      $varE10=_varInterp2($varC3,$varC4,3,1);
      $varE20=_varInterp2($varC7,$varC3,1,1);
      $varE21=_varInterp2($varC4,$varC7,3,1);
    }
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==42) or
    ($intPattern==170)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
      $varE01=$varC4;
      $varE10=$varC4;
      $varE20=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE00=_varInterp2($varC3,$varC1,1,1);
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE10=_varInterp2($varC3,$varC4,3,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==14) or
    ($intPattern==142)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
      $varE01=$varC4;
      $varE02=_varInterp2($varC4,$varC5,3,1);
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp2($varC3,$varC1,1,1);
      $varE01=_varInterp2($varC1,$varC4,3,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
      $varE10=_varInterp2($varC4,$varC3,3,1);
    }
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==67)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==70)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==28)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==152)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==194)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==98)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==56)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==25)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==26) or
    ($intPattern==31)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==82) or
    ($intPattern==214)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==88) or
    ($intPattern==248)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==74) or
    ($intPattern==107)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==27)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==86)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==216)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==106)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==30)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==210)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==120)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==75)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==29)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==198)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==184)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==99)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==57)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==71)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==156)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==226)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==60)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==195)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==102)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==153)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==58)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==83)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==92)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==202)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==78)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==154)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==114)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==89)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==90)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==55) or
    ($intPattern==23)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE00=_varInterp2($varC4,$varC3,3,1);
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
      $varE01=_varInterp2($varC1,$varC4,3,1);
      $varE02=_varInterp2($varC1,$varC5,1,1);
      $varE12=_varInterp2($varC4,$varC5,3,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==182) or
    ($intPattern==150)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
      $varE22=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE02=_varInterp2($varC1,$varC5,1,1);
      $varE12=_varInterp2($varC5,$varC4,3,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==213) or
    ($intPattern==212)
  )
  {
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE02=_varInterp2($varC4,$varC1,3,1);
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
      $varE12=_varInterp2($varC5,$varC4,3,1);
      $varE21=_varInterp2($varC4,$varC7,3,1);
      $varE22=_varInterp2($varC5,$varC7,1,1);
    }
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    
  }
  elsif (
    ($intPattern==241) or
    ($intPattern==240)
  )
  {
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE20=_varInterp2($varC4,$varC3,3,1);
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,3,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
      $varE21=_varInterp2($varC7,$varC4,3,1);
      $varE22=_varInterp2($varC5,$varC7,1,1);
    }
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    
  }
  elsif (
    ($intPattern==236) or
    ($intPattern==232)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
      $varE22=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,3,1);
      $varE20=_varInterp2($varC7,$varC3,1,1);
      $varE21=_varInterp2($varC7,$varC4,3,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==109) or
    ($intPattern==105)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE00=_varInterp2($varC4,$varC1,3,1);
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
      $varE10=_varInterp2($varC3,$varC4,3,1);
      $varE20=_varInterp2($varC7,$varC3,1,1);
      $varE21=_varInterp2($varC4,$varC7,3,1);
    }
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==171) or
    ($intPattern==43)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
      $varE20=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE00=_varInterp2($varC3,$varC1,1,1);
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE10=_varInterp2($varC3,$varC4,3,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==143) or
    ($intPattern==15)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE02=_varInterp2($varC4,$varC5,3,1);
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp2($varC3,$varC1,1,1);
      $varE01=_varInterp2($varC1,$varC4,3,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
      $varE10=_varInterp2($varC4,$varC3,3,1);
    }
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==124)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==203)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==62)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==211)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==118)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==217)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==110)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==155)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==188)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==185)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==61)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==157)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==103)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==227)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==230)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==199)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==220)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==158)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==234)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==242)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==59)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==121)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==87)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==79)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==122)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==94)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==218)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==91)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==229)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==167)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==173)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==181)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==186)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==115)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==93)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==206)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==205) or
    ($intPattern==201)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==174) or
    ($intPattern==46)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==179) or
    ($intPattern==147)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==117) or
    ($intPattern==116)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==189)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==231)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==126)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==219)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==125)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE00=_varInterp2($varC4,$varC1,3,1);
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
      $varE10=_varInterp2($varC3,$varC4,3,1);
      $varE20=_varInterp2($varC7,$varC3,1,1);
      $varE21=_varInterp2($varC4,$varC7,3,1);
    }
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==221)
  )
  {
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE02=_varInterp2($varC4,$varC1,3,1);
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
      $varE12=_varInterp2($varC5,$varC4,3,1);
      $varE21=_varInterp2($varC4,$varC7,3,1);
      $varE22=_varInterp2($varC5,$varC7,1,1);
    }
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    
  }
  elsif (
    ($intPattern==207)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE02=_varInterp2($varC4,$varC5,3,1);
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp2($varC3,$varC1,1,1);
      $varE01=_varInterp2($varC1,$varC4,3,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
      $varE10=_varInterp2($varC4,$varC3,3,1);
    }
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==238)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
      $varE22=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,3,1);
      $varE20=_varInterp2($varC7,$varC3,1,1);
      $varE21=_varInterp2($varC7,$varC4,3,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==190)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
      $varE22=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE02=_varInterp2($varC1,$varC5,1,1);
      $varE12=_varInterp2($varC5,$varC4,3,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==187)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
      $varE20=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE00=_varInterp2($varC3,$varC1,1,1);
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE10=_varInterp2($varC3,$varC4,3,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==243)
  )
  {
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE20=_varInterp2($varC4,$varC3,3,1);
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,3,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
      $varE21=_varInterp2($varC7,$varC4,3,1);
      $varE22=_varInterp2($varC5,$varC7,1,1);
    }
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    
  }
  elsif (
    ($intPattern==119)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE00=_varInterp2($varC4,$varC3,3,1);
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
      $varE01=_varInterp2($varC1,$varC4,3,1);
      $varE02=_varInterp2($varC1,$varC5,1,1);
      $varE12=_varInterp2($varC4,$varC5,3,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==237) or
    ($intPattern==233)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==175) or
    ($intPattern==47)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==183) or
    ($intPattern==151)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==245) or
    ($intPattern==244)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=$varC4;
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==250)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==123)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==95)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==222)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==252)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=$varC4;
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==249)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==235)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==111)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==63)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==159)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==215)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==246)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=$varC4;
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==254)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
    }
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==253)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE02=_varInterp2($varC4,$varC1,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=$varC4;
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==251)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE01=_varInterp2($varC4,$varC1,7,1);
    }
    $varE02=_varInterp2($varC4,$varC2,3,1);
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE10=_varInterp2($varC4,$varC3,7,1);
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE12=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE12=_varInterp2($varC4,$varC5,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==239)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    $varE02=_varInterp2($varC4,$varC5,3,1);
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    $varE22=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==127)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,7,7);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE11=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
      $varE21=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,7,7);
      $varE21=_varInterp2($varC4,$varC7,7,1);
    }
    $varE22=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==191)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC7,3,1);
    $varE21=_varInterp2($varC4,$varC7,3,1);
    $varE22=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==223)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,7,7);
      $varE10=_varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE02=$varC4;
      $varE12=$varC4;
    }
    else
    {
      $varE01=_varInterp2($varC4,$varC1,7,1);
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
      $varE12=_varInterp2($varC4,$varC5,7,1);
    }
    $varE11=$varC4;
    $varE20=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE21=$varC4;
      $varE22=$varC4;
    }
    else
    {
      $varE21=_varInterp2($varC4,$varC7,7,1);
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,7,7);
    }
    
  }
  elsif (
    ($intPattern==247)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=$varC4;
    $varE12=$varC4;
    $varE20=_varInterp2($varC4,$varC3,3,1);
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=$varC4;
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==255)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=$varC4;
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE02=$varC4;
    }
    else
    {
      $varE02=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=$varC4;
    $varE11=$varC4;
    $varE12=$varC4;
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE20=$varC4;
    }
    else
    {
      $varE20=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE21=$varC4;
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE22=$varC4;
    }
    else
    {
      $varE22=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  #END HQ3X PATTERNS
  
  return(
    $varE00,$varE01,$varE02,
    $varE10,$varE11,$varE12,
    $varE20,$varE21,$varE22
  );
}
# standard HQ2X casepath
sub _arrHQ2X {
  my ($intPattern,
    $varC0,$varC1,$varC2,
    $varC3,$varC4,$varC5,
    $varC6,$varC7,$varC8
  )=@_;
  my $varE00; my $varE01;
  my $varE10; my $varE11;
  # BEGIN HQ2X PATTERNS
  if (
    ($intPattern==0) or
    ($intPattern==1) or
    ($intPattern==4) or
    ($intPattern==32) or
    ($intPattern==128) or
    ($intPattern==5) or
    ($intPattern==132) or
    ($intPattern==160) or
    ($intPattern==33) or
    ($intPattern==129) or
    ($intPattern==36) or
    ($intPattern==133) or
    ($intPattern==164) or
    ($intPattern==161) or
    ($intPattern==37) or
    ($intPattern==165)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==2) or
    ($intPattern==34) or
    ($intPattern==130) or
    ($intPattern==162)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==16) or
    ($intPattern==17) or
    ($intPattern==48) or
    ($intPattern==49)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==64) or
    ($intPattern==65) or
    ($intPattern==68) or
    ($intPattern==69)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==8) or
    ($intPattern==12) or
    ($intPattern==136) or
    ($intPattern==140)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==3) or
    ($intPattern==35) or
    ($intPattern==131) or
    ($intPattern==163)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==6) or
    ($intPattern==38) or
    ($intPattern==134) or
    ($intPattern==166)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==20) or
    ($intPattern==21) or
    ($intPattern==52) or
    ($intPattern==53)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==144) or
    ($intPattern==145) or
    ($intPattern==176) or
    ($intPattern==177)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==192) or
    ($intPattern==193) or
    ($intPattern==196) or
    ($intPattern==197)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==96) or
    ($intPattern==97) or
    ($intPattern==100) or
    ($intPattern==101)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==40) or
    ($intPattern==44) or
    ($intPattern==168) or
    ($intPattern==172)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==9) or
    ($intPattern==13) or
    ($intPattern==137) or
    ($intPattern==141)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==18) or
    ($intPattern==50)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==80) or
    ($intPattern==81)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==72) or
    ($intPattern==76)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==10) or
    ($intPattern==138)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==66)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==24)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==7) or
    ($intPattern==39) or
    ($intPattern==135)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==148) or
    ($intPattern==149) or
    ($intPattern==180)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==224) or
    ($intPattern==228) or
    ($intPattern==225)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==41) or
    ($intPattern==169) or
    ($intPattern==45)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==22) or
    ($intPattern==54)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==208) or
    ($intPattern==209)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==104) or
    ($intPattern==108)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==11) or
    ($intPattern==139)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==19) or
    ($intPattern==51)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE00=_varInterp2($varC4,$varC3,3,1);
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC1,$varC3,5,2,1);
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,3,3);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==146) or
    ($intPattern==178)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
      $varE11=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,3,3);
      $varE11=_varInterp3($varC4,$varC5,$varC7,5,2,1);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    
  }
  elsif (
    ($intPattern==84) or
    ($intPattern==85)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC5,$varC1,5,2,1);
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,3,3);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    
  }
  elsif (
    ($intPattern==112) or
    ($intPattern==113)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE10=_varInterp2($varC4,$varC3,3,1);
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,5,2,1);
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,3,3);
    }
    
  }
  elsif (
    ($intPattern==200) or
    ($intPattern==204)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
      $varE11=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,3,3);
      $varE11=_varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    
  }
  elsif (
    ($intPattern==73) or
    ($intPattern==77)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE00=_varInterp2($varC4,$varC1,3,1);
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,5,2,1);
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,3,3);
    }
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==42) or
    ($intPattern==170)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
      $varE10=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,3,3);
      $varE10=_varInterp3($varC4,$varC3,$varC7,5,2,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==14) or
    ($intPattern==142)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
      $varE01=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,3,3);
      $varE01=_varInterp3($varC4,$varC1,$varC5,5,2,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==67)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==70)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==28)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==152)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==194)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==98)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==56)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==25)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==26) or
    ($intPattern==31)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==82) or
    ($intPattern==214)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==88) or
    ($intPattern==248)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==74) or
    ($intPattern==107)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==27)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==86)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==216)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==106)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==30)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==210)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==120)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==75)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC6,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==29)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==198)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==184)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==99)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==57)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==71)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==156)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==226)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==60)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==195)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==102)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==153)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==58)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==83)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==92)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==202)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==78)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    $varE01=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==154)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==114)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==89)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==90)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==55) or
    ($intPattern==23)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE00=_varInterp2($varC4,$varC3,3,1);
      $varE01=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC1,$varC3,5,2,1);
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,3,3);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==182) or
    ($intPattern==150)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE11=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,3,3);
      $varE11=_varInterp3($varC4,$varC5,$varC7,5,2,1);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    
  }
  elsif (
    ($intPattern==213) or
    ($intPattern==212)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE11=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC5,$varC1,5,2,1);
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,3,3);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    
  }
  elsif (
    ($intPattern==241) or
    ($intPattern==240)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE10=_varInterp2($varC4,$varC3,3,1);
      $varE11=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,5,2,1);
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,3,3);
    }
    
  }
  elsif (
    ($intPattern==236) or
    ($intPattern==232)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE11=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,3,3);
      $varE11=_varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    
  }
  elsif (
    ($intPattern==109) or
    ($intPattern==105)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE00=_varInterp2($varC4,$varC1,3,1);
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,5,2,1);
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,3,3);
    }
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==171) or
    ($intPattern==43)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE10=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,3,3);
      $varE10=_varInterp3($varC4,$varC3,$varC7,5,2,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==143) or
    ($intPattern==15)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,3,3);
      $varE01=_varInterp3($varC4,$varC1,$varC5,5,2,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==124)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==203)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC6,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==62)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==211)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==118)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==217)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==110)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==155)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==188)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==185)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==61)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==157)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==103)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==227)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==230)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==199)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==220)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==158)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==234)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==242)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==59)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==121)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==87)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==79)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==122)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==94)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==218)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==91)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==229)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==167)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==173)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==181)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==186)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==115)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==93)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==206)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    $varE01=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==205) or
    ($intPattern==201)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=_varInterp2($varC4,$varC6,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,6,1,1);
    }
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==174) or
    ($intPattern==46)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=_varInterp2($varC4,$varC0,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,6,1,1);
    }
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==179) or
    ($intPattern==147)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=_varInterp2($varC4,$varC2,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==117) or
    ($intPattern==116)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=_varInterp2($varC4,$varC8,3,1);
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    
  }
  elsif (
    ($intPattern==189)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==231)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==126)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==219)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp2($varC4,$varC2,3,1);
    $varE10=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==125)
  )
  {
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE00=_varInterp2($varC4,$varC1,3,1);
      $varE10=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,5,2,1);
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,3,3);
    }
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==221)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE01=_varInterp2($varC4,$varC1,3,1);
      $varE11=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC5,$varC1,5,2,1);
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,3,3);
    }
    $varE10=_varInterp2($varC4,$varC6,3,1);
    
  }
  elsif (
    ($intPattern==207)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE01=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,3,3);
      $varE01=_varInterp3($varC4,$varC1,$varC5,5,2,1);
    }
    $varE10=_varInterp2($varC4,$varC6,3,1);
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==238)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
      $varE11=_varInterp2($varC4,$varC5,3,1);
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,3,3);
      $varE11=_varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    
  }
  elsif (
    ($intPattern==190)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
      $varE11=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,3,3);
      $varE11=_varInterp3($varC4,$varC5,$varC7,5,2,1);
    }
    $varE10=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==187)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
      $varE10=_varInterp2($varC4,$varC7,3,1);
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,3,3);
      $varE10=_varInterp3($varC4,$varC3,$varC7,5,2,1);
    }
    $varE01=_varInterp2($varC4,$varC2,3,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==243)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    $varE01=_varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE10=_varInterp2($varC4,$varC3,3,1);
      $varE11=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,5,2,1);
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,3,3);
    }
    
  }
  elsif (
    ($intPattern==119)
  )
  {
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE00=_varInterp2($varC4,$varC3,3,1);
      $varE01=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC1,$varC3,5,2,1);
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,3,3);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==237) or
    ($intPattern==233)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,14,1,1);
    }
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==175) or
    ($intPattern==47)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,14,1,1);
    }
    $varE01=_varInterp2($varC4,$varC5,3,1);
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==183) or
    ($intPattern==151)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
    $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==245) or
    ($intPattern==244)
  )
  {
    $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    $varE10=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    
  }
  elsif (
    ($intPattern==250)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    $varE01=_varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==123)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==95)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC6,3,1);
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==222)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==252)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    
  }
  elsif (
    ($intPattern==249)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp3($varC4,$varC2,$varC1,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==235)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp3($varC4,$varC2,$varC5,2,1,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,14,1,1);
    }
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==111)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,14,1,1);
    }
    $varE01=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp3($varC4,$varC8,$varC5,2,1,1);
    
  }
  elsif (
    ($intPattern==63)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,14,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp3($varC4,$varC8,$varC7,2,1,1);
    
  }
  elsif (
    ($intPattern==159)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==215)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
    $varE10=_varInterp3($varC4,$varC6,$varC3,2,1,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==246)
  )
  {
    $varE00=_varInterp3($varC4,$varC0,$varC3,2,1,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    
  }
  elsif (
    ($intPattern==254)
  )
  {
    $varE00=_varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    
  }
  elsif (
    ($intPattern==253)
  )
  {
    $varE00=_varInterp2($varC4,$varC1,3,1);
    $varE01=_varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    
  }
  elsif (
    ($intPattern==251)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    $varE01=_varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==239)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,14,1,1);
    }
    $varE01=_varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,14,1,1);
    }
    $varE11=_varInterp2($varC4,$varC5,3,1);
    
  }
  elsif (
    ($intPattern==127)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,14,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,2,1,1);
    }
    $varE11=_varInterp2($varC4,$varC8,3,1);
    
  }
  elsif (
    ($intPattern==191)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,14,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
    $varE10=_varInterp2($varC4,$varC7,3,1);
    $varE11=_varInterp2($varC4,$varC7,3,1);
    
  }
  elsif (
    ($intPattern==223)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,2,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
    $varE10=_varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    
  }
  elsif (
    ($intPattern==247)
  )
  {
    $varE00=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
    $varE10=_varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    
  }
  elsif (
    ($intPattern==255)
  )
  {
    if (_boolYUV_NE($varC3, $varC1))
    {
      $varE00=$varC4;
    }
    else
    {
      $varE00=_varInterp3($varC4,$varC3,$varC1,14,1,1);
    }
    if (_boolYUV_NE($varC1, $varC5))
    {
      $varE01=$varC4;
    }
    else
    {
      $varE01=_varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
    if (_boolYUV_NE($varC7, $varC3))
    {
      $varE10=$varC4;
    }
    else
    {
      $varE10=_varInterp3($varC4,$varC7,$varC3,14,1,1);
    }
    if (_boolYUV_NE($varC5, $varC7))
    {
      $varE11=$varC4;
    }
    else
    {
      $varE11=_varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
  }
  # END HQ2X PATTERNS
  return(
    $varE00,$varE01,
    $varE10,$varE11
  );
}
# Maxim Stepin's HQ2X
sub _voidComplex_HQ2X {
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
  my $intPattern=0;
  if (_boolYUV_NE($varC4,$varC0)) { $intPattern|=1   };
  if (_boolYUV_NE($varC4,$varC1)) { $intPattern|=2   };
  if (_boolYUV_NE($varC4,$varC2)) { $intPattern|=4   };
  if (_boolYUV_NE($varC4,$varC3)) { $intPattern|=8   };
  if (_boolYUV_NE($varC4,$varC5)) { $intPattern|=16  };
  if (_boolYUV_NE($varC4,$varC6)) { $intPattern|=32  };
  if (_boolYUV_NE($varC4,$varC7)) { $intPattern|=64  };
  if (_boolYUV_NE($varC4,$varC8)) { $intPattern|=128 };
  my (
    $varE00,$varE01,
    $varE10,$varE11
  )=_arrHQ2X(
    $intPattern,
    $varC0,$varC1,$varC2,
    $varC3,$varC4,$varC5,
    $varC6,$varC7,$varC8
  );
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
  
}
# Maxim Stepin's HQ3X
sub _voidComplex_HQ3X {
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
  my $intPattern=0;
  if (_boolYUV_NE($varC4,$varC0)) { $intPattern|=1   };
  if (_boolYUV_NE($varC4,$varC1)) { $intPattern|=2   };
  if (_boolYUV_NE($varC4,$varC2)) { $intPattern|=4   };
  if (_boolYUV_NE($varC4,$varC3)) { $intPattern|=8   };
  if (_boolYUV_NE($varC4,$varC5)) { $intPattern|=16  };
  if (_boolYUV_NE($varC4,$varC6)) { $intPattern|=32  };
  if (_boolYUV_NE($varC4,$varC7)) { $intPattern|=64  };
  if (_boolYUV_NE($varC4,$varC8)) { $intPattern|=128 };
  my (
    $varE00,$varE01,$varE02,
    $varE10,$varE11,$varE12,
    $varE20,$varE21,$varE22
  )=_arrHQ3X(
    $intPattern,
    $varC0,$varC1,$varC2,
    $varC3,$varC4,$varC5,
    $varC6,$varC7,$varC8
  );
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

sub _voidComplex_LQ2X {
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
  my $intPattern=0;
  if (_boolYUV_NE($varC4,$varC0)) { $intPattern|=1   };
  if (_boolYUV_NE($varC4,$varC1)) { $intPattern|=2   };
  if (_boolYUV_NE($varC4,$varC2)) { $intPattern|=4   };
  if (_boolYUV_NE($varC4,$varC3)) { $intPattern|=8   };
  if (_boolYUV_NE($varC4,$varC5)) { $intPattern|=16  };
  if (_boolYUV_NE($varC4,$varC6)) { $intPattern|=32  };
  if (_boolYUV_NE($varC4,$varC7)) { $intPattern|=64  };
  if (_boolYUV_NE($varC4,$varC8)) { $intPattern|=128 };
  my (
    $varE00,$varE01,
    $varE10,$varE11
  )=_arrLQ2X(
    $intPattern,
    $varC0,$varC1,$varC2,
    $varC3,$varC4,$varC5,
    $varC6,$varC7,$varC8
  );
  $objRet->voidSetPixel($intDX  ,$intDY  ,$varE00);
  $objRet->voidSetPixel($intDX+1,$intDY  ,$varE01);
  $objRet->voidSetPixel($intDX  ,$intDY+1,$varE10);
  $objRet->voidSetPixel($intDX+1,$intDY+1,$varE11);
  
}
# Dirk Stevens' Eagle
sub _voidComplex_Eagle {
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
  if (_boolE($varC0,$varC1) and _boolE($varC1,$varC3)) {
    $objRet->voidSetPixel($intDX,$intDY,$varC0);
  } else {
    $objRet->voidSetPixel($intDX,$intDY,$varC4);
  }
  if (_boolE($varC1,$varC2) and _boolE($varC2,$varC5)) {
    $objRet->voidSetPixel($intDX+1,$intDY,$varC1);
  } else {
    $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  }
  if (_boolE($varC3,$varC6) and _boolE($varC6,$varC7)) {
    $objRet->voidSetPixel($intDX,$intDY+1,$varC3);
  } else {
    $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
  }
  if (_boolE($varC5,$varC7) and _boolE($varC7,$varC8)) {
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC5);
  } else {
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
  }
}
# Hawkynt's 3XEagle
sub _voidComplex_Eagle3X {
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
  my $varE00; my $varE01; my $varE02;
  my $varE10; my $varE11; my $varE12;
  my $varE20; my $varE21; my $varE22;
  
  # center
  $varE11=$varC4;
  # from eagle 2x
  if (_boolE($varC0,$varC1) and _boolE($varC1,$varC3)) {
    $varE00=$varC0;
  } else {
    $varE00=$varC4;
  }
  if (_boolE($varC1,$varC2) and _boolE($varC2,$varC5)) {
    $varE02=$varC1;
  } else {
    $varE02=$varC4;
  }
  if (_boolE($varC3,$varC6) and _boolE($varC6,$varC7)) {
    $varE20=$varC3;
  } else {
    $varE20=$varC4;
  }
  if (_boolE($varC5,$varC7) and _boolE($varC7,$varC8)) {
    $varE22=$varC5;
  } else {
    $varE22=$varC4;
  }
  # from Hawkynt to match a 3X Eagle
  $varE10=$varC4;
  $varE01=$varC4;
  $varE21=$varC4;
  $varE12=$varC4;
  
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
  
  if (_boolNE($varC4, $varC8)) {
    if (_boolE($varC7 , $varC5)) {
      $objRet->voidSetPixel($intDX+1,$intDY,$varC7);
      $objRet->voidSetPixel($intDX,$intDY+1,$varC7);
      if (_boolE($varC6 , $varC7) or _boolE($varC5 , $varC2)) {
        $objRet->voidSetPixel($intDX,$intDY,_varInterp2($varC7,$varC4,3,1));
      } else {
        $objRet->voidSetPixel($intDX,$intDY,_varInterp2($varC4,$varC5,1,1));
      }
      
      if (_boolE($varC5 , $varD4) or _boolE($varC7 , $varD1)) {
        $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($varC7,$varC8,3,1));
      } else {
        $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($varC7,$varC8,1,1));
      }
    } else {
      $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($varC8,$varC7,$varC5,6,1,1));
      $objRet->voidSetPixel($intDX,$intDY, _varInterp3($varC4,$varC7,$varC5,6,1,1));
      
      $objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($varC7,$varC4,$varC8,6,1,1));
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($varC5,$varC4,$varC8,6,1,1));
    }
  } else {
    if (_boolNE($varC7 , $varC5)) {
      $objRet->voidSetPixel($intDX,$intDY,$varC4);
      $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
      
      if (_boolE($varC1 , $varC4) or _boolE($varC8 , $varD5)) {
        $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,3,1));
      } else {
        $objRet->voidSetPixel($intDX+1,$intDY, _varInterp2($varC4,$varC5,1,1));
      }
      
      if (_boolE($varC8 , $varD2) or _boolE($varC3 , $varC4)) {
        $objRet->voidSetPixel($intDX,$intDY+1, _varInterp2($varC4,$varC7,3,1));
      } else {
        $objRet->voidSetPixel($intDX,$intDY+1, _varInterp2($varC7,$varC8,1,1));
      }
    } else {
      my $intR= 0;
      $intR += _intConc2d($varC5,$varC4,$varC6,$varD1);
      $intR += _intConc2d($varC5,$varC4,$varC3,$varC1);
      $intR += _intConc2d($varC5,$varC4,$varD2,$varD5);
      $intR += _intConc2d($varC5,$varC4,$varC2,$varD4);
      
      if ($intR > 0) {
        $objRet->voidSetPixel($intDX,$intDY+1,$varC7);
        $objRet->voidSetPixel($intDX+1,$intDY,$varC7);
        $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($varC4,$varC5,1,1));
        $objRet->voidSetPixel($intDX,$intDY,_varInterp2($varC4,$varC5,1,1));
      } elsif ($intR < 0) {
        $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
        $objRet->voidSetPixel($intDX,$intDY,$varC4);
        $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC4,$varC5,1,1));
        $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
      } else {
        $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
        $objRet->voidSetPixel($intDX,$intDY,$varC4);
        $objRet->voidSetPixel($intDX,$intDY+1,$varC7);
        $objRet->voidSetPixel($intDX+1,$intDY,$varC7);
      }
    }
  }
}
# Dirk Stevens' 2XSaI
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
  
  if (_boolE($varC4,$varC8) and _boolNE($varC5, $varC7)) {
    if ((_boolE($varC4, $varC1) and _boolE($varC5 , $varD5)) or
      (_boolE($varC4, $varC7) and _boolE($varC4 , $varC2) and _boolNE($varC5 , $varC1) and _boolE($varC5, $varD3))) {
        $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
    } else {
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
    }
    
    if ((_boolE($varC4 , $varC3) and _boolE($varC7 , $varD2)) or
      (_boolE($varC4 , $varC5) and _boolE($varC4 , $varC6) and _boolNE($varC3 , $varC7)  and _boolE($varC7 , $varD0))) {
        $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
    } else {
      $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC4,$varC7,1,1));
    }
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
  } elsif (_boolE($varC5, $varC7) and _boolNE($varC4 , $varC8)) {
    if ((_boolE($varC5 , $varC2) and _boolE($varC4 , $varC6)) or
      (_boolE($varC5 , $varC1) and _boolE($varC5 , $varC8) and _boolNE($varC4 , $varC2) and _boolE($varC4 , $varC0))) {
        $objRet->voidSetPixel($intDX+1,$intDY,$varC5);
    } else {
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
    }
    
    if ((_boolE($varC7 , $varC6) and _boolE($varC4 , $varC2)) or
      (_boolE($varC7 , $varC3) and _boolE($varC7 , $varC8) and _boolNE($varC4 , $varC6) and _boolE($varC4 , $varC0))) {
        $objRet->voidSetPixel($intDX,$intDY+1,$varC7);
    } else {
      $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC4,$varC7,1,1));
    }
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC5);
  } elsif (_boolE($varC4 , $varC8) and _boolE($varC5, $varC7)) {
    if (_boolE($varC4 , $varC5)) {
      $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
      $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
      $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
    } else {
      my $intR = 0;
      $intR += _intConc2d($varC4,$varC5,$varC3,$varC1);
      $intR -= _intConc2d($varC5,$varC4,$varD4,$varC2);
      $intR -= _intConc2d($varC5,$varC4,$varC6,$varD1);
      $intR += _intConc2d($varC4,$varC5,$varD5,$varD2);
      
      if ($intR > 0) {
        $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
      } elsif ($intR < 0) {
        $objRet->voidSetPixel($intDX+1,$intDY+1,$varC5);
      } else {
        $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp4($varC4,$varC5,$varC7,$varC8,1,1,1,1));
      }
      
      $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC4,$varC7,1,1));
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
    }
  } else {
    $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp4($varC4,$varC5,$varC7,$varC8,1,1,1,1));
    
    if (_boolE($varC4 , $varC7) and _boolE($varC4 , $varC2)
      and _boolNE($varC5 , $varC1) and _boolE($varC5 , $varD3)) {
        $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
    } elsif (_boolE($varC5 , $varC1) and _boolE($varC5 , $varC8)
      and _boolNE($varC4 , $varC2) and _boolE($varC4 , $varC0)) {
        $objRet->voidSetPixel($intDX+1,$intDY,$varC5);
    } else {
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
    }
    
    if (_boolE($varC4 , $varC5) and _boolE($varC4 , $varC6)
      and _boolNE($varC3 , $varC7) and _boolE($varC7 , $varD0)) {
        $objRet->voidSetPixel($intDX,$intDY+1,$varC4);
    } elsif (_boolE($varC7 , $varC3) and _boolE($varC7 , $varC8)
      and _boolNE($varC4 , $varC6) and _boolE($varC4 , $varC0)) {
        $objRet->voidSetPixel($intDX,$intDY+1,$varC7);
    } else {
      $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC4,$varC7,1,1));
    }
  }
  $objRet->voidSetPixel($intDX,$intDY,$varC4);
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
  if (_boolE($varC7 , $varC5) and _boolNE($varC4 , $varC8)) {
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC7);
    $objRet->voidSetPixel($intDX+1,$intDY,$varC7);
  } elsif (_boolE($varC4 , $varC8) and _boolNE($varC7 , $varC5)) {
    $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
    $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
  } elsif (_boolE($varC4 , $varC8) and _boolE($varC7 , $varC5)) {
    my $intR = 0;
    $intR  += _intConc2d($varC5,$varC4,$varC6,$varD1);
    $intR  += _intConc2d($varC5,$varC4,$varC3,$varC1);
    $intR  += _intConc2d($varC5,$varC4,$varD2,$varD5);
    $intR  += _intConc2d($varC5,$varC4,$varC2,$varD4);

    if ($intR > 0) {
      $objRet->voidSetPixel($intDX+1,$intDY+1,$varC5);
      $objRet->voidSetPixel($intDX+1,$intDY,$varC5);
    } elsif ($intR  < 0) {
      $objRet->voidSetPixel($intDX+1,$intDY+1,$varC4);
      $objRet->voidSetPixel($intDX+1,$intDY,$varC4);
    } else {
      $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($varC4,$varC5,1,1));
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
    }
  } else {
    if (_boolE($varC5 , $varC8) and _boolE($varC8 , $varD1) and _boolNE($varC7 , $varD2) and _boolNE($varC8 , $varD0)) {
      $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($varC8,$varC7,3,1));
    } elsif (_boolE($varC4 , $varC7) and _boolE($varC7 , $varD2) and _boolNE($varD1 , $varC8) and _boolNE($varC7 , $varD6)) {
      $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($varC7,$varC8,3,1));
    } else {
      $objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($varC7,$varC8,1,1));
    }
    if (_boolE($varC5 , $varC8) and _boolE($varC5 , $varC1) and _boolNE($varC4 , $varC2) and _boolNE($varC5 , $varC0)) {
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC5,$varC4,3,1));
    } elsif (_boolE($varC4, $varC7) and _boolE($varC4 , $varC2) and _boolNE($varC1 , $varC5) and _boolNE($varC4 , $varD3)) {
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,3,1));
    } else {
      $objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($varC4,$varC5,1,1));
    }
  }

  if (_boolE($varC4 , $varC8) and _boolNE($varC7 , $varC5) and _boolE($varC3 , $varC4) and _boolNE($varC4 , $varD2)) {
    $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC7,$varC4,1,1));
  } elsif (_boolE($varC4 , $varC6) and _boolE($varC5 , $varC4) and _boolNE($varC3 , $varC7) and _boolNE($varC4 , $varD0)) {
    $objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($varC7,$varC4,1,1));
  } else {
    $objRet->voidSetPixel($intDX,$intDY+1,$varC7);
  }
  if (_boolE($varC7 , $varC5) and _boolNE($varC4 , $varC8) and _boolE($varC6 , $varC7) and _boolNE($varC7 , $varC2)) {
    $objRet->voidSetPixel($intDX,$intDY,_varInterp2($varC7,$varC4,1,1));
  } elsif (_boolE($varC3 , $varC7) and _boolE($varC8 , $varC7) and _boolNE($varC6 , $varC4) and _boolNE($varC7 , $varC0)) {
    $objRet->voidSetPixel($intDX,$intDY,_varInterp2($varC7,$varC4,1,1));
  } else {
    $objRet->voidSetPixel($intDX,$intDY,$varC4);
  }
}

#                                                                              #
#                 -=[ P R I V A T E   P R O P E R T I E S ]=-                  #
#                                                                              #
# create rgb color
sub _varC_RGB {
  my ($intR,$intG,$intB)=@_;
  my $varColor=($intR & 255) << 16 | ($intG & 255) << 8 | ($intB & 255);
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
  for (my $intY=0;$intY<$objThis->intGetHeight();$intY++) {
    my $arrLine=varArrayItem($arrImg,$intY);
    for (my $intX=0;$intX<$objThis->intGetWidth();$intX++) {
      my $varColor=&{$ptrSub}($intX,$intY);
      voidArrayItem($arrLine,$intX,$varColor);
    }
  }
  return($objThis);
}

sub voidExportRaw {
  my ($objThis,$ptrSub)=@_;
  my $arrImg=$objThis->arrGetRawImage();
  my $intW=$objThis->intGetWidth();
  my $intH=$objThis->intGetHeight();
  for (my $intY=0;$intY<$intH;$intY++) {
    my $arrLine=$arrImg->[$intY];
    for (my $intX=0;$intX<$intW;$intX++) {
      my $varColor=$arrLine->[$intX];
      &{$ptrSub}($intX,$intY,$varColor);
    }
  }
  return($objThis);
}
#                                                                              #
#                  -=[ P U B L I C   P R O P E R T I E S ]=-                   #
#                                                                              #
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

# apply image maginifer/minifier filter
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
    for (my $intY=0;$intY<$intH;$intY++) {
      my $intDX=0;
      for (my $intX=0;$intX<$intW;$intX++) {
        # call filter
        &{$ptrFilter}(
          $objThis,$intX,$intY,$objRet,$intDX,$intDY);
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

# bilinear resize
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
sub arr_objGetRGB {
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
      $objImage_R->voidSetPixel($intX,$intY,_varC_RGB(_intG_R($varC),_intG_R($varC),_intG_R($varC)));
      $objImage_G->voidSetPixel($intX,$intY,_varC_RGB(_intG_G($varC),_intG_G($varC),_intG_G($varC)));
      $objImage_B->voidSetPixel($intX,$intY,_varC_RGB(_intG_B($varC),_intG_B($varC),_intG_B($varC)));
    }
  }
  voidArrayItem($arrRet,0,$objImage_R);
  voidArrayItem($arrRet,1,$objImage_G);
  voidArrayItem($arrRet,2,$objImage_B);
  return($arrRet);
}

# split image to Y,U and V greyscale
sub arr_objGetYUV {
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
      $objImage_Y->voidSetPixel($intX,$intY,_varC_RGB(_intG_Y($varC),_intG_Y($varC),_intG_Y($varC)));
      $objImage_U->voidSetPixel($intX,$intY,_varC_RGB(_intG_U($varC),_intG_U($varC),_intG_U($varC)));
      $objImage_V->voidSetPixel($intX,$intY,_varC_RGB(_intG_V($varC),_intG_V($varC),_intG_V($varC)));
    }
  }
  voidArrayItem($arrRet,0,$objImage_Y);
  voidArrayItem($arrRet,1,$objImage_U);
  voidArrayItem($arrRet,2,$objImage_V);
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
  if (boolNot( ($intX<0) or ($intY<0) or ($intX>=$intW) or ($intY>=$intH))) {
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
#------------------------------------------------------------------------------#
#                                                                              #
#                                                                              #
#                    -=[ S E L F - L O A D E R ]=-                             #
#                                                                              #
sub voidSelfLoad {
  my ($arrArguments)=\@_;
  my $szArgumentList=szPrintf(' %s ',arr(szJoin(' ', $arrArguments)));
  if ( boolMatchRegEx( $szArgumentList,'/[\s]((\/\?)|(\-help)|(\-\-help))[\s]/i' ) or (intArrLen($arrArguments)<1) ) {
    my $szHelp=szJoin($szLE,arr(
      'classImage.pm [/?] [-help] [--help] [<options>]',
      ' image class',
      '        /? | -help | --help  : shows this help',
      '',''
    ));
    voidWarn($szHelp,'Help');
  }
  else {
    # TODO: initialize working environment
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