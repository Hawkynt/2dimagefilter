use stricT;
use warnings;
use Modules::modIL;
use Modules::modGlobal;
my $szFile='classImage.pm';
my $szOutFile='classImage.ok.pm';

our $szLE=$modGlobal::szLE;

my $szData=szJoin($szLE,modGlobal::arrReadFile($szFile));
my $arrReplaceTBL=[
  ['#PIXEL00_100','$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[4],$arrC->[2],14,1,1));'],
  ['#PIXEL00_90' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[4],$arrC->[2],2,3,3));'],
  ['#PIXEL00_70' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[4],$arrC->[2],6,1,1));'],
  ['#PIXEL00_61' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[4],$arrC->[2],5,2,1));'],
  ['#PIXEL00_60' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[4],5,2,1));'],
  ['#PIXEL00_22' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[1],$arrC->[4],2,1,1));'],
  ['#PIXEL00_21' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[1],$arrC->[2],2,1,1));'],
  ['#PIXEL00_20' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp3($arrC->[5],$arrC->[4],$arrC->[2],2,1,1));'],
  ['#PIXEL00_12' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp2($arrC->[5],$arrC->[2],3,1));'],
  ['#PIXEL00_11' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp2($arrC->[5],$arrC->[4],3,1));'],
  ['#PIXEL00_10' ,'$objRet->voidSetPixel($intDX,$intDY,_varInterp2($arrC->[5],$arrC->[1],3,1));'],
  ['#PIXEL00_0'  ,'$objRet->voidSetPixel($intDX,$intDY,$arrC->[5]);'],
  
  ['#PIXEL01_100','$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[6],14,1,1));'],
  ['#PIXEL01_90' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[6],2,3,3));'],
  ['#PIXEL01_70' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[6],6,1,1));'],
  ['#PIXEL01_61' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[6],5,2,1));'],
  ['#PIXEL01_60' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[6],$arrC->[2],5,2,1));'],
  ['#PIXEL01_22' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[3],$arrC->[2],2,1,1));'],
  ['#PIXEL01_21' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[3],$arrC->[6],2,1,1));'],
  ['#PIXEL01_20' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp3($arrC->[5],$arrC->[2],$arrC->[6],2,1,1));'],
  ['#PIXEL01_12' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($arrC->[5],$arrC->[6],3,1));'],
  ['#PIXEL01_11' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($arrC->[5],$arrC->[2],3,1));'],
  ['#PIXEL01_10' ,'$objRet->voidSetPixel($intDX+1,$intDY,_varInterp2($arrC->[5],$arrC->[3],3,1));'],
  ['#PIXEL01_0'  ,'$objRet->voidSetPixel($intDX+1,$intDY,$arrC->[5]);'],
  
  ['#PIXEL10_100','$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[8],$arrC->[4],14,1,1));'],
  ['#PIXEL10_90' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[8],$arrC->[4],2,3,3));'],
  ['#PIXEL10_70' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[8],$arrC->[4],6,1,1));'],
  ['#PIXEL10_61' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[8],$arrC->[4],5,2,1));'],
  ['#PIXEL10_60' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[4],$arrC->[8],5,2,1));'],
  ['#PIXEL10_22' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[7],$arrC->[8],2,1,1));'],
  ['#PIXEL10_21' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[7],$arrC->[4],2,1,1));'],
  ['#PIXEL10_20' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp3($arrC->[5],$arrC->[8],$arrC->[4],2,1,1));'],
  ['#PIXEL10_12' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($arrC->[5],$arrC->[4],3,1));'],
  ['#PIXEL10_11' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($arrC->[5],$arrC->[8],3,1));'],
  ['#PIXEL10_10' ,'$objRet->voidSetPixel($intDX,$intDY+1,_varInterp2($arrC->[5],$arrC->[7],3,1));'],
  ['#PIXEL10_0'  ,'$objRet->voidSetPixel($intDX,$intDY+1,$arrC->[5]);'],
  
  ['#PIXEL11_100','$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[6],$arrC->[8],14,1,1));'],
  ['#PIXEL11_90' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[6],$arrC->[8],2,3,3));'],
  ['#PIXEL11_70' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[6],$arrC->[8],6,1,1));'],
  ['#PIXEL11_61' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[6],$arrC->[8],5,2,1));'],
  ['#PIXEL11_60' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[8],$arrC->[6],5,2,1));'],
  ['#PIXEL11_22' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[9],$arrC->[6],2,1,1));'],
  ['#PIXEL11_21' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[9],$arrC->[8],2,1,1));'],
  ['#PIXEL11_20' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp3($arrC->[5],$arrC->[6],$arrC->[8],2,1,1));'],
  ['#PIXEL11_12' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($arrC->[5],$arrC->[8],3,1));'],
  ['#PIXEL11_11' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($arrC->[5],$arrC->[6],3,1));'],
  ['#PIXEL11_10' ,'$objRet->voidSetPixel($intDX+1,$intDY+1,_varInterp2($arrC->[5],$arrC->[9],3,1));'],
  ['#PIXEL11_0'  ,'$objRet->voidSetPixel($intDX+1,$intDY+1,$arrC->[5]);'],
];

foreach my $arrC (@{$arrReplaceTBL}) {
  my $szWhat=$arrC->[0];
  my $szNew=$arrC->[1];
  $szData=szReplace($szData,$szWhat,$szNew);
}
modGlobal::voidWriteFile($szOutFile,arr($szData));
print 'OK';