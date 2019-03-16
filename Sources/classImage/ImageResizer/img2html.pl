#!/usr/bin/perl -w
#==============================================================================#
#
#         FILE:  img2html.pl
#
#        USAGE:  ./img2html.pl [/?] [-help] [--help] [<$szFile>]
#
#  DESCRIPTION:  loads an image and creates a html file
#
#      OPTIONS:  $szFile
# REQUIREMENTS:  ---
#         BUGS:  ---
#        NOTES:  ---
#       AUTHOR:  »SynthelicZ« Hawkynt
#      COMPANY:  »SynthelicZ«
#      VERSION:  1.000
#      CREATED:  17.10.2008 07:10:26 CET
#     REVISION:  ---
#==============================================================================#
package img2html;
use base 'Exporter';              # we could export if we want to

use strict;
use warnings;

use Modules::modIL;               # independant language library
use Modules::IL::Filter;
use Modules::modGlobal;           # everyday routines for programmers
use Classes::classImage;
use Classes::classObject;         # to create new classes

our @EXPORT_OK = qw();            # we don't want to export by default
our @EXPORT = qw();               # we never export anything
our $VERSION=1.000;               # yes thats our applications version

our $szLE=$modGlobal::szLE;       # simple line-ending

# typical self-loader
sub voidSelfLoad {
  my ($arrArguments)=\@_;
  my $szArgumentList=szPrintf(' %s ',arr(szJoin(' ', $arrArguments)));
  if ( boolMatchRegEx( $szArgumentList,'/[\s]((\/\?)|(\-help)|(\-\-help))[\s]/i' ) or (intArrLen($arrArguments)<1) ) {
    my $szHelp=szJoin($szLE,arr(
      'img2html.pl [/?] [-help] [--help] [<$szFile>]',
      ' loads an image and creates a html file',
      '        /? | -help | --help  : shows this help',
      '                     $szFile : image file name',
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
  my ($szFile)=@_;
  # default configuration
  
  # now we come to the real main code
  voidProcess($szFile);
  
}

# main processing
sub voidProcess {
  my ($szFile)=@_;
  # sourcecode here
  if (modGlobal::boolFileExists($szFile)) {
    my $objImage=classImage::objLoad_ByTK($szFile);
    if (boolNot(boolIsDefined($objImage->varGetAttribute('szErrorMessage')))) {

      my $szTargetFile=szCombineStrings($szFile,'.htm');
      my $intWidth=$objImage->intGetWidth();
      my $intHeight=$objImage->intGetHeight();
      my $intDX=3;
      my $intDY=2;
      my $boolUseStyle=boolTRUE();
      my $boolUseTable=boolFALSE();
      my $intMethod=3;
      # 0 - table
      # 1 - table-CSS
      # 2 - div
      # 3 - bbcode
      
      my $arrRawImage=$objImage->arrGetRawImage();
      my $arrLines=arr();
      my $szHTML;
      #voidPush($arrLines,'<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">');
      voidPush($arrLines,'<html>');
      voidPush($arrLines,szPrintf('  <head><title>%s [%dx%d]</title></head>',arr($szFile,$intWidth,$intHeight)));
      voidPush($arrLines,'  <body>');
      if ($intMethod==1 || $intMethod==2) {
        voidPush($arrLines,'    <style type="text/CSS">');
        voidPush($arrLines,'      .classP {');
        $szHTML=szPrintf('        width:%dpx;height:%dpx;',arr($intDX,$intDY));
        voidPush($arrLines,$szHTML);
        voidPush($arrLines,'        font:1px Arial;');
        voidPush($arrLines,'        display:inline;');
        voidPush($arrLines,'        overflow:hidden;');
        $szHTML=szPrintf('        clip:rect(0,%dpx,%dpx,0);',arr($intDX,$intDY));
        voidPush($arrLines,$szHTML);
        voidPush($arrLines,'      }');
        voidPush($arrLines,'    </style>');
      }
      if ($intMethod==0) {
        # normal table
        voidPush($arrLines,szPrintf('    <table border="0" cellpadding="0" cellspacing="0" style="width:%dpx;height:%dpx;table-layout:fixed;">',arr($intWidth*$intDX,$intHeight*$intDY)));
        for my $intY(0..$intHeight-1) {
          my $arrLine=varArrayItem($arrRawImage,$intY);
          $szHTML=szPrintf('       <tr style="height:%dpx">',arr($intDY));
          voidPush($arrLines,$szHTML);
          for my $intX(0..$intWidth-1) {
            my $szColor=szPrintf('#%06x',arr(varArrayItem($arrLine,$intX)));
            $szHTML=szPrintf('        <td style="background:%s;width:%dpx"></td>',arr($szColor,$intDX));
            voidPush($arrLines,$szHTML);
          }
          voidPush($arrLines,'      </tr>');
        }
        voidPush($arrLines,'    </table>');
      } elsif ($intMethod==1) {
        # table using css
        voidPush($arrLines,szPrintf('    <table border="0" cellpadding="0" cellspacing="0" style="width:%dpx;height:%dpx;table-layout:fixed;">',arr($intWidth*$intDX,$intHeight*$intDY)));
        for my $intY(0..$intHeight-1) {
          my $arrLine=varArrayItem($arrRawImage,$intY);
          voidPush($arrLines,'      <tr>');
          for my $intX(0..$intWidth-1) {
            my $szColor=szPrintf('#%06x',arr(varArrayItem($arrLine,$intX)));
            $szHTML=szPrintf('        <td class="classP" style="background:%s;"></td>',arr($szColor,$intDX));
            voidPush($arrLines,$szHTML);
          }
          voidPush($arrLines,'      </tr>');
        }
        voidPush($arrLines,'    </table>');
      } elsif ($intMethod==2) {
        # generate div style
        $szHTML=szPrintf('    <div style="width:%dpx;height:%dpx;overflow:hidden;white-space:nowrap;font:1px Arial;">',arr($intWidth*$intDX,$intHeight*$intDY));
        voidPush($arrLines,$szHTML);
        for my $intY(0..$intHeight-1) {
          my $arrLine=varArrayItem($arrRawImage,$intY);
          my $arrPixLine=arr();
          voidPush($arrLines,'      <div>');
          for my $intX(0..$intWidth-1) {
            my $szColor=szPrintf('#%06x',arr(varArrayItem($arrLine,$intX)));
            $szHTML=szPrintf('        <div class="classP" style="background:%s;"></div>',arr($szColor,$intDX));
            voidPush($arrLines,$szHTML);
          }
          voidPush($arrLines,'      </div>');
        }
        voidPush($arrLines,'    </div>');
      } elsif ($intMethod==3) {
        # generate bbcode
        for my $intY(0..$intHeight-1) {
          my $arrLine=varArrayItem($arrRawImage,$intY);
          my $arrPixLine=arr();
          $szHTML='';
          for my $intX(0..$intWidth-1) {
            my $szColor=szPrintf('#%06x',arr(varArrayItem($arrLine,$intX)));
            $szHTML.=szPrintf('[bgcolor=%s][color=%s]_[/color][/bgcolor]',arr($szColor,$szColor));
          }
          voidPush($arrLines,$szHTML);
        }
      } else {
        # error unknown method
      }
      voidPush($arrLines,'  </body>');
      voidPush($arrLines,'</html>');
      modGlobal::voidWriteFile($szTargetFile, $arrLines);
    } else {
      voidWarn(szPrintf('could not create classImage-object:%s',arr($objImage->varGetAttribute('szErrorMessage'))));
    }
  } else {
    voidWarn(szPrintf('file not found "%s"',arr($szFile)));
  }
  
}



if ($0 eq __FILE__) {
  # Self Loader
  #&voidSelfLoad(@ARGV);
  &voidSelfLoad('input/test-1x6.bmp');
} else {
  return (1==1);
}