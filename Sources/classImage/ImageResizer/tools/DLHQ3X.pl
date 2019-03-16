use stricT;
use warnings;
use Modules::modIL;
use Modules::modGlobal;
my $szFile='classImage.pm';
my $szOutFile='classImage.ok.pm';

our $szLE=$modGlobal::szLE;

my $szData=szJoin($szLE,modGlobal::arrReadFile($szFile));
my $arrReplaceTBL=[
  ['#PIXEL00_1M','$objRet->voidSetPixel($intDX,$intDY,_varInterp2($arrC->[5],$arrC->[1],3,1));'],
  ['#PIXEL00_1U','$objRet->voidSetPixel($intDX,$intDY,_varInterp2($arrC->[5],$arrC->[2],3,1));'],
  ['#PIXEL00_1L','$objRet->voidSetPixel($intDX,$intDY,_varInterp2($arrC->[5],$arrC->[4],3,1));'],
  ['#PIXEL00_2' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[4],$arrC->[2],2,1,1));'],
  ['#PIXEL00_4' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[4],$arrC->[2],2,7,7));'],
  ['#PIXEL00_5' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp2($arrC->[4],$arrC->[2],1,1));'],
  ['#PIXEL00_C' ,'$objRet->voidSetPixel($intDX,$intDY,$arrC->[5]);'],
  
  ['#PIXEL01_1' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($arrC->[5],$arrC->[2],3,1));'],
  ['#PIXEL01_3' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($arrC->[5],$arrC->[2],7,1));'],
  ['#PIXEL01_6' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($arrC->[2],$arrC->[5],3,1));'],
  ['#PIXEL01_C' ,'$objRet->voidSetPixel($intDX+1,$intDY,$arrC->[5]);'],
  
  ['#PIXEL02_1M','$objRet->voidSetPixel($intDX+2,$intDY,_varInterp2($arrC->[5],$arrC->[3],3,1));'],
  ['#PIXEL02_1U','$objRet->voidSetPixel($intDX+2,$intDY,_varInterp2($arrC->[5],$arrC->[2],3,1));'],
  ['#PIXEL02_1R','$objRet->voidSetPixel($intDX+2,$intDY,_varInterp2($arrC->[5],$arrC->[6],3,1));'],
  ['#PIXEL02_2' ,'$objRet->voidSetPixel($intDX+2,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[6],2,1,1));'],
  ['#PIXEL02_4' ,'$objRet->voidSetPixel($intDX+2,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[6],2,7,7));'],
  ['#PIXEL02_5' ,'$objRet->voidSetPixel($intDX+2,$intDY,_varInterp2($arrC->[2],$arrC->[6],1,1));'],
  ['#PIXEL02_C' ,'$objRet->voidSetPixel($intDX+2,$intDY,$arrC->[5]);'],
  
  ['#PIXEL10_1' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($arrC->[5],$arrC->[4],3,1));'],
  ['#PIXEL10_3' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($arrC->[5],$arrC->[4],7,1));'],
  ['#PIXEL10_6' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($arrC->[4],$arrC->[5],3,1));'],
  ['#PIXEL10_C' ,'$objRet->voidSetPixel($intDX,$intDY+1,$arrC->[5]);'],
  
  ['#PIXEL11'   ,'$objRet->voidSetPixel($intDX+1,$intDY+1,$arrC->[5]);'],
  
  ['#PIXEL12_1' ,'$objRet->voidSetPixel($intDX+2,$intDY+1,_varInterp2($arrC->[5],$arrC->[6],3,1));'],
  ['#PIXEL12_3' ,'$objRet->voidSetPixel($intDX+2,$intDY+1,_varInterp2($arrC->[5],$arrC->[6],7,1));'],
  ['#PIXEL12_6' ,'$objRet->voidSetPixel($intDX+2,$intDY+1,_varInterp2($arrC->[6],$arrC->[5],3,1));'],
  ['#PIXEL12_C' ,'$objRet->voidSetPixel($intDX+2,$intDY+1,$arrC->[5]);'],
  
  ['#PIXEL20_1M','$objRet->voidSetPixel($intDX,$intDY+2,_varInterp2($arrC->[5],$arrC->[7],3,1));'],
  ['#PIXEL20_1D','$objRet->voidSetPixel($intDX,$intDY+2,_varInterp2($arrC->[5],$arrC->[8],3,1));'],
  ['#PIXEL20_1L','$objRet->voidSetPixel($intDX,$intDY+2,_varInterp2($arrC->[5],$arrC->[4],3,1));'],
  ['#PIXEL20_2' ,'$objRet->voidSetPixel($intDX,$intDY+2,_varInterp3($arrC->[5],$arrC->[8],$arrC->[4],2,1,1));'],
  ['#PIXEL20_4' ,'$objRet->voidSetPixel($intDX,$intDY+2,_varInterp3($arrC->[5],$arrC->[8],$arrC->[4],2,7,7));'],
  ['#PIXEL20_5' ,'$objRet->voidSetPixel($intDX,$intDY+2,_varInterp2($arrC->[8],$arrC->[4],1,1));'],
  ['#PIXEL20_C' ,'$objRet->voidSetPixel($intDX,$intDY+2,$arrC->[5]);'],
  
  ['#PIXEL21_1' ,'$objRet->voidSetPixel($intDX+1,$intDY+2,_varInterp2($arrC->[5],$arrC->[8],3,1));'],
  ['#PIXEL21_3' ,'$objRet->voidSetPixel($intDX+1,$intDY+2,_varInterp2($arrC->[5],$arrC->[8],7,1));'],
  ['#PIXEL21_6' ,'$objRet->voidSetPixel($intDX+1,$intDY+2,_varInterp2($arrC->[8],$arrC->[5],3,1));'],
  ['#PIXEL21_C' ,'$objRet->voidSetPixel($intDX+1,$intDY+2,$arrC->[5]);'],
  
  ['#PIXEL22_1M','$objRet->voidSetPixel($intDX+2,$intDY+2,_varInterp2($arrC->[5],$arrC->[9],3,1));'],
  ['#PIXEL22_1D','$objRet->voidSetPixel($intDX+2,$intDY+2,_varInterp2($arrC->[5],$arrC->[8],3,1));'],
  ['#PIXEL22_1R','$objRet->voidSetPixel($intDX+2,$intDY+2,_varInterp2($arrC->[5],$arrC->[6],3,1));'],
  ['#PIXEL22_2' ,'$objRet->voidSetPixel($intDX+2,$intDY+2,_varInterp3($arrC->[5],$arrC->[6],$arrC->[8],2,1,1));'],
  ['#PIXEL22_4' ,'$objRet->voidSetPixel($intDX+2,$intDY+2,_varInterp3($arrC->[5],$arrC->[6],$arrC->[8],2,7,7));'],
  ['#PIXEL22_5' ,'$objRet->voidSetPixel($intDX+2,$intDY+2,_varInterp2($arrC->[6],$arrC->[8],1,1));'],
  ['#PIXEL22_C' ,'$objRet->voidSetPixel($intDX+2,$intDY+2,$arrC->[5]);'],
];

foreach my $arrC (@{$arrReplaceTBL}) {
  my $szWhat=$arrC->[0];
  my $szNew=$arrC->[1];
  $szData=szReplace($szData,$szWhat,$szNew);
}
modGlobal::voidWriteFile($szOutFile,arr($szData));
print 'OK';