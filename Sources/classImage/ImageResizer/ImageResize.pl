#!/usr/bin/perl -w
#==============================================================================#
#
#         FILE:  ImageResize.pl
#
#        USAGE:  ./ImageResize.pl [/?] [-help] [--help] [/display] <$szFile, $intW, $intH,$szMethod>]
#
#  DESCRIPTION:  resizes images
#
#      OPTIONS:  $szFile, $intW, $intH,$szMethod
# REQUIREMENTS:  ---
#         BUGS:  ---
#        NOTES:  ---
#       AUTHOR:  »SynthelicZ« Hawkynt
#      COMPANY:  »SynthelicZ«
#      VERSION:  1.000
#      CREATED:  08.11.2007 13:19:55 CET
#     REVISION:  ---
#==============================================================================#
package ImageResize;
use base 'Exporter';              # we could export if we want to

use strict;
use warnings;

use Tk;
use Tk::Photo;
use Tk::JPEG;
use Tk::PNG;
use Time::HiRes qw(time);

use Modules::modIL;               # independant language library
use Modules::modGlobal;           # everyday routines for programmers
use Classes::classObject;         # to create new classes
use Classes::classImage;

our @EXPORT_OK = qw();            # we don't want to export by default
our @EXPORT = qw();               # we never export anything
our $VERSION=1.000;               # yes thats our applications version

our $szLE=$modGlobal::szLE;       # simple line-ending
our $_floatTimer;

# typical self-loader
sub voidSelfLoad {
  my ($arrArguments)=\@_;
  my $szArgumentList=szPrintf(' %s ',arr(szJoin(' ', $arrArguments)));
  if ( boolMatchRegEx( $szArgumentList,'/[\s]((\/\?)|(\-help)|(\-\-help))[\s]/i' ) or (intArrLen($arrArguments)<1) ) {
    my $szHelp=szJoin($szLE,arr(
      'ImageResize.pl [/?] [-help] [--help] [/display] [<$szFile, $intW, $intH,$szMethod>]',
      ' resizes images',
      '        /? | -help | --help  : shows this help',
      '                    /display : show available filters',
      '                     $szFile : BMP/JPG/GIF/PNG File to process',
      '                       $intW : new width or 0 for automatic',
      '                       $intH : new height or 0 for automatic',
      '                   $szMethod : differRGB, splitRGB, splitYUV, splitGmC, splitGMC, benchmark, display or a filter name',
      '',''
    ));
    voidWarn($szHelp,'Help');
  }
  else {
    &voidMain(@{$arrArguments});
  }
}

# typical main
sub voidMain {
  my ($szFile, $intW, $intH,$szMethod)=@_;
  
  # now we come to the real main code
  voidProcess($szFile, $intW, $intH,$szMethod);
}

# parses image object and returns array containing pixels
sub _objLoadImage {
  my ($szFile)=@_;
  my $objRet=classImage::objLoad_ByTK($szFile);
  return($objRet);
}

# construct imager object from hexadecimal pixel array
sub _voidSaveImage {
  my ($szFile,$objImage)=@_;
  print szPrintf('Saving "%s": ',arr($szFile));
  my $floatKPix=$objImage->intGetWidth()*$objImage->intGetHeight()/1000;
  print szPrintf('%5.3f MPix',arr($floatKPix/1000));
  my $floatTimer=Time::HiRes::time();
  $objImage->voidSave_ByTK($szFile);
  $floatTimer=Time::HiRes::time()-$floatTimer;
  print szPrintf('[%9.3f kPix/sec.]',arr($floatKPix/$floatTimer));
  print $szLE;
}

#process scaler
sub voidScaleNSave {
  my ($objImage_Source,$szMethod,$intW,$intH,$szTargetFile)=@_;
  # load resizer
  if (classImage::boolFilterExists($szMethod)) {
    print szPrintf(' +%-15s: ',arr($szMethod));
    my $floatTimer=Time::HiRes::time();
    my $objImage_Target=$objImage_Source->objFilterImage($szMethod);
    print szPrintf('%4dx%-4d Pix ',arr($objImage_Target->intGetWidth(),$objImage_Target->intGetHeight()));
    $floatTimer=Time::HiRes::time()-$floatTimer;
    print szPrintf('[%9.3f Pixel/sec.]',arr($objImage_Source->intGetWidth()*$objImage_Source->intGetHeight()/$floatTimer));
    print $szLE;
    
    if (boolNot(boolIsDefined($objImage_Target->varGetAttribute('szErrorMessage')))) {
      if (($intW>0) and ($intH>0)) {
        print szPrintf('Resampling to %4dx%-4d Pix:',arr($intW,$intH));
        print szPrintf('(%5.3f MPix)',arr($objImage_Target->intGetWidth()*$objImage_Target->intGetHeight()/1000000));
        print $szLE;
        $objImage_Target= $objImage_Target->objResample_TriLinear($intW,$intH);
      }
      
      # save new image
      _voidSaveImage($szTargetFile,$objImage_Target);
    } else {
      voidWarn(szPrintf('Error creating target bitmap: %s',arr($objImage_Target->varGetAttribute('szErrorMessage'))));
    }
  
  } else {
    voidWarn(szPrintf('Filter "%s" not found ',arr($szMethod)));
  }
}

# main processing
sub voidProcess {
  my ($szFile, $intW, $intH,$szMethod)=@_;
  
  my $arrBenchmarks=arr(
    'Scale3X',
    'Scale3x',
  );
  
  if (boolIsEqual($szMethod,'display') or boolIsEqual($szFile,'/display')) {
    my $arrFilters=arrSort(classImage->arrGetFilters());
    print 'Available Image Filters:';
    print $szLE;
    for (my $intI=0;$intI<intArrLen($arrFilters);$intI++) {
      print szPrintf('   %s',arr(varArrayItem($arrFilters,$intI)));
      print $szLE;
    }
    print $szLE;
  } else {
    # load image
    print szPrintf('Loading "%s":',arr($szFile));
    my $floatTimer=Time::HiRes::time();
    my $objImage_Source=_objLoadImage($szFile);
    $floatTimer=Time::HiRes::time()-$floatTimer;
    print szPrintf('%dx%d Pix [%9.3f kPix/sec.]',arr($objImage_Source->intGetWidth(),$objImage_Source->intGetHeight(),$objImage_Source->intGetWidth()*$objImage_Source->intGetHeight()/$floatTimer/1000));
    print $szLE;
    
    # process image
    if (boolIsEqual($szMethod,'benchmark')) {
      # benchmark mode
      for (my $intI=0;$intI<intArrLen($arrBenchmarks);$intI++) {
        my $szCurMethod=varArrayItem($arrBenchmarks,$intI);
        my $szTargetFile=szPrintf('output/%02d - %s.bmp',arr($intI,$szCurMethod));
        voidScaleNSave($objImage_Source,$szCurMethod,$intW,$intH,$szTargetFile);
      }
    } elsif (boolIsEqual($szMethod,'differRGB')) {
      my ($objR,$objG,$objB)=@{$objImage_Source->arr_objSplitRGB()};
      _voidSaveImage('output/channelRG.bmp',$objR->objDifference($objG));
      _voidSaveImage('output/channelRB.bmp',$objR->objDifference($objB));
      _voidSaveImage('output/channelGB.bmp',$objG->objDifference($objB));
    } elsif (boolIsEqual($szMethod,'splitRGB')) {
      my $arr_objRGB=$objImage_Source->arr_objSplitRGB();
      _voidSaveImage('output/channelR.bmp',varArrayItem($arr_objRGB,0));
      _voidSaveImage('output/channelG.bmp',varArrayItem($arr_objRGB,1));
      _voidSaveImage('output/channelB.bmp',varArrayItem($arr_objRGB,2));
    } elsif (boolIsEqual($szMethod,'splitYUV')) {
      my $arr_objRGB=$objImage_Source->arr_objSplitRGB();
      _voidSaveImage('output/channelY.bmp',varArrayItem($arr_objRGB,0));
      _voidSaveImage('output/channelU.bmp',varArrayItem($arr_objRGB,1));
      _voidSaveImage('output/channelV.bmp',varArrayItem($arr_objRGB,2));
    } elsif (boolIsEqual($szMethod,'splitGmC')) {
      my $arr_objRGB=$objImage_Source->arr_objSplitGmC();
      _voidSaveImage('output/channelGm.bmp',varArrayItem($arr_objRGB,0));
      _voidSaveImage('output/channelC.bmp',varArrayItem($arr_objRGB,1));
    } elsif (boolIsEqual($szMethod,'splitGMC')) {
      my $arr_objRGB=$objImage_Source->arr_objSplitGMC();
      _voidSaveImage('output/channelGM.bmp',varArrayItem($arr_objRGB,0));
      _voidSaveImage('output/channelC.bmp',varArrayItem($arr_objRGB,1));
    } else {
      # normal processing
      my $szTargetFile='output/output.bmp';
      voidScaleNSave($objImage_Source,$szMethod,$intW,$intH,$szTargetFile);
    }
  }
}



if ($0 eq __FILE__) {
  # Self Loader
  if (0) { # for treshold use 0
    $classImage::intTrY=0;
    $classImage::intTrU=0;
    $classImage::intTrV=0;
  }
  system('perl src\\AdvMAME\\dat2pl.pl');
  &voidSelfLoad('input/test-1x4.bmp',0,0,'benchmark');
  #&voidSelfLoad(@ARGV);
} else {
  return (1==1);
}