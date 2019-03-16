use strict;
use warnings;
use Modules::modIL;
use Modules::modGlobal;
use FindBin;
my $arrFiles= modGlobal::arrGetFiles($FindBin::Bin);
my $szData='';
our $hashFilter=hash();
foreach my $szFile (@{$arrFiles}) {
  if (boolIsEqual('dat', modGlobal::szExtractExtension($szFile))) {
    $szData.=_szProcess($FindBin::Bin.'/'.$szFile);
  }
}

my $szFilters='';
foreach my $szFilter(sort(keys(%{$hashFilter}))) {
  my ($intX,$intY,$boolNeedsPattern)=@{$hashFilter->{$szFilter}};
  if ($boolNeedsPattern) {
    $szFilters.="\$_hashFilters->{'${szFilter}'}     =arr(\\\&_voidComplex_nQwXh     ,$intX,$intY,1,1);\n";
    $szFilters.="\$_hashFilters->{'${szFilter}Bold'} =arr(\\\&_voidComplex_nQwXhBold ,$intX,$intY,1,1);\n";
    $szFilters.="\$_hashFilters->{'${szFilter}Smart'}=arr(\\\&_voidComplex_nQwXhSmart,$intX,$intY,1,1);\n";
  } else {
    $szFilters.="\$_hashFilters->{'${szFilter}'}     =arr(\\\&_voidComplex_PnQwXh    ,$intX,$intY,1,1);\n";
  }
}
$szData= $szFilters.$szData;

modGlobal::voidWriteFileBinary($FindBin::Bin.'/output.pl', $szData);

sub _szProcess {
  my ($szFile)=@_;
  my $szData=szJoin("\n",modGlobal::arrReadFile($szFile));
  $szFile= modGlobal::szExtractFileName($szFile);
  print 'Processing '.$szFile."\n";
  my $szCode= szLeft($szFile, intLen($szFile)-intLen(modGlobal::szExtractExtension($szFile))-1);
  $szData=~s/\/\*\s.*?\s\*\///gs;
  $szData=~s/^[\s]*$//gm;
  $szData=~s/^\s*case/  if \(\ncase/s;
  $szData=~s/(case\s[^:]*)\:\s*\n\{/$1\n  \)\{/g;
  $szData=~s/case\s(\d+)\s*\:/"    ( \$intPattern == 0b".modGlobal::szBIN($1, 8)." ) or"/eg;
  $szData=~s/case\s(\d+)/"    ( \$intPattern == 0b".modGlobal::szBIN($1, 8)." )"/eg;
  $szData=~s/\}\sbreak;/  \} elsif (/g;
  $szData=~s/\t/    /g;
  
  $szData=~s/!MUH/_boolYUV_E( \$varC1, \$varC0 )/g;
  $szData=~s/!MUE/_boolYUV_E( \$varC1, \$varC2 )/g;
  $szData=~s/!MUL/_boolYUV_E( \$varC1, \$varC3 )/g;
  $szData=~s/!MUR/_boolYUV_E( \$varC1, \$varC5 )/g;
  $szData=~s/!MUD/_boolYUV_E( \$varC1, \$varC7 )/g;
  $szData=~s/!MDL/_boolYUV_E( \$varC7, \$varC3 )/g;
  $szData=~s/!MDR/_boolYUV_E( \$varC7, \$varC5 )/g;
  $szData=~s/!MDG/_boolYUV_E( \$varC7, \$varC6 )/g;
  $szData=~s/!MDF/_boolYUV_E( \$varC7, \$varC8 )/g;
  $szData=~s/!MLR/_boolYUV_E( \$varC3, \$varC5 )/g;
  $szData=~s/!MLH/_boolYUV_E( \$varC3, \$varC0 )/g;
  $szData=~s/!MLG/_boolYUV_E( \$varC3, \$varC6 )/g;
  $szData=~s/!MRE/_boolYUV_E( \$varC5, \$varC2 )/g;
  $szData=~s/!MRF/_boolYUV_E( \$varC5, \$varC8 )/g;
  $szData=~s/!MHE/_boolYUV_E( \$varC0, \$varC2 )/g;
  $szData=~s/!MGF/_boolYUV_E( \$varC6, \$varC8 )/g;
  $szData=~s/!MHG/_boolYUV_E( \$varC0, \$varC6 )/g;
  $szData=~s/!MEF/_boolYUV_E( \$varC2, \$varC8 )/g;
  $szData=~s/!MCH/_boolYUV_E( \$varC4, \$varC0 )/g;
  $szData=~s/!MCU/_boolYUV_E( \$varC4, \$varC1 )/g;
  $szData=~s/!MCE/_boolYUV_E( \$varC4, \$varC2 )/g;
  $szData=~s/!MCL/_boolYUV_E( \$varC4, \$varC3 )/g;
  $szData=~s/!MCR/_boolYUV_E( \$varC4, \$varC5 )/g;
  $szData=~s/!MCG/_boolYUV_E( \$varC4, \$varC6 )/g;
  $szData=~s/!MCD/_boolYUV_E( \$varC4, \$varC7 )/g;
  $szData=~s/!MCF/_boolYUV_E( \$varC4, \$varC8 )/g;
  
  $szData=~s/MUH/_boolYUV_NE( \$varC1, \$varC0 )/g;
  $szData=~s/MUE/_boolYUV_NE( \$varC1, \$varC2 )/g;
  $szData=~s/MUL/_boolYUV_NE( \$varC1, \$varC3 )/g;
  $szData=~s/MUR/_boolYUV_NE( \$varC1, \$varC5 )/g;
  $szData=~s/MUD/_boolYUV_NE( \$varC1, \$varC7 )/g;
  $szData=~s/MDL/_boolYUV_NE( \$varC7, \$varC3 )/g;
  $szData=~s/MDR/_boolYUV_NE( \$varC7, \$varC5 )/g;
  $szData=~s/MDG/_boolYUV_NE( \$varC7, \$varC6 )/g;
  $szData=~s/MDF/_boolYUV_NE( \$varC7, \$varC8 )/g;
  $szData=~s/MLR/_boolYUV_NE( \$varC3, \$varC5 )/g;
  $szData=~s/MLH/_boolYUV_NE( \$varC3, \$varC0 )/g;
  $szData=~s/MLG/_boolYUV_NE( \$varC3, \$varC6 )/g;
  $szData=~s/MRE/_boolYUV_NE( \$varC5, \$varC2 )/g;
  $szData=~s/MRF/_boolYUV_NE( \$varC5, \$varC8 )/g;
  $szData=~s/MHE/_boolYUV_NE( \$varC0, \$varC2 )/g;
  $szData=~s/MGF/_boolYUV_NE( \$varC6, \$varC8 )/g;
  $szData=~s/MHG/_boolYUV_NE( \$varC0, \$varC6 )/g;
  $szData=~s/MEF/_boolYUV_NE( \$varC2, \$varC8 )/g;
  $szData=~s/MCH/_boolYUV_NE( \$varC4, \$varC0 )/g;
  $szData=~s/MCU/_boolYUV_NE( \$varC4, \$varC1 )/g;
  $szData=~s/MCE/_boolYUV_NE( \$varC4, \$varC2 )/g;
  $szData=~s/MCL/_boolYUV_NE( \$varC4, \$varC3 )/g;
  $szData=~s/MCR/_boolYUV_NE( \$varC4, \$varC5 )/g;
  $szData=~s/MCG/_boolYUV_NE( \$varC4, \$varC6 )/g;
  $szData=~s/MCD/_boolYUV_NE( \$varC4, \$varC7 )/g;
  $szData=~s/MCF/_boolYUV_NE( \$varC4, \$varC8 )/g;
  
  $szData=~s/!PUH/_boolE( \$varC1, \$varC0 )/g;
  $szData=~s/!PUE/_boolE( \$varC1, \$varC2 )/g;
  $szData=~s/!PUL/_boolE( \$varC1, \$varC3 )/g;
  $szData=~s/!PUR/_boolE( \$varC1, \$varC5 )/g;
  $szData=~s/!PUD/_boolE( \$varC1, \$varC7 )/g;
  $szData=~s/!PDL/_boolE( \$varC7, \$varC3 )/g;
  $szData=~s/!PDR/_boolE( \$varC7, \$varC5 )/g;
  $szData=~s/!PDG/_boolE( \$varC7, \$varC6 )/g;
  $szData=~s/!PDF/_boolE( \$varC7, \$varC8 )/g;
  $szData=~s/!PLR/_boolE( \$varC3, \$varC5 )/g;
  $szData=~s/!PLH/_boolE( \$varC3, \$varC0 )/g;
  $szData=~s/!PLG/_boolE( \$varC3, \$varC6 )/g;
  $szData=~s/!PRE/_boolE( \$varC5, \$varC2 )/g;
  $szData=~s/!PRF/_boolE( \$varC5, \$varC8 )/g;
  $szData=~s/!PHE/_boolE( \$varC0, \$varC2 )/g;
  $szData=~s/!PGF/_boolE( \$varC6, \$varC8 )/g;
  $szData=~s/!PHG/_boolE( \$varC0, \$varC6 )/g;
  $szData=~s/!PEF/_boolE( \$varC2, \$varC8 )/g;
  $szData=~s/!PCH/_boolE( \$varC4, \$varC0 )/g;
  $szData=~s/!PCU/_boolE( \$varC4, \$varC1 )/g;
  $szData=~s/!PCE/_boolE( \$varC4, \$varC2 )/g;
  $szData=~s/!PCL/_boolE( \$varC4, \$varC3 )/g;
  $szData=~s/!PCR/_boolE( \$varC4, \$varC5 )/g;
  $szData=~s/!PCG/_boolE( \$varC4, \$varC6 )/g;
  $szData=~s/!PCD/_boolE( \$varC4, \$varC7 )/g;
  $szData=~s/!PCF/_boolE( \$varC4, \$varC8 )/g;
  
  $szData=~s/PUH/_boolNE( \$varC1, \$varC0 )/g;
  $szData=~s/PUE/_boolNE( \$varC1, \$varC2 )/g;
  $szData=~s/PUL/_boolNE( \$varC1, \$varC3 )/g;
  $szData=~s/PUR/_boolNE( \$varC1, \$varC5 )/g;
  $szData=~s/PUD/_boolNE( \$varC1, \$varC7 )/g;
  $szData=~s/PDL/_boolNE( \$varC7, \$varC3 )/g;
  $szData=~s/PDR/_boolNE( \$varC7, \$varC5 )/g;
  $szData=~s/PDG/_boolNE( \$varC7, \$varC6 )/g;
  $szData=~s/PDF/_boolNE( \$varC7, \$varC8 )/g;
  $szData=~s/PLR/_boolNE( \$varC3, \$varC5 )/g;
  $szData=~s/PLH/_boolNE( \$varC3, \$varC0 )/g;
  $szData=~s/PLG/_boolNE( \$varC3, \$varC6 )/g;
  $szData=~s/PRE/_boolNE( \$varC5, \$varC2 )/g;
  $szData=~s/PRF/_boolNE( \$varC5, \$varC8 )/g;
  $szData=~s/PHE/_boolNE( \$varC0, \$varC2 )/g;
  $szData=~s/PGF/_boolNE( \$varC6, \$varC8 )/g;
  $szData=~s/PHG/_boolNE( \$varC0, \$varC6 )/g;
  $szData=~s/PEF/_boolNE( \$varC2, \$varC8 )/g;
  $szData=~s/PCH/_boolNE( \$varC4, \$varC0 )/g;
  $szData=~s/PCU/_boolNE( \$varC4, \$varC1 )/g;
  $szData=~s/PCE/_boolNE( \$varC4, \$varC2 )/g;
  $szData=~s/PCL/_boolNE( \$varC4, \$varC3 )/g;
  $szData=~s/PCR/_boolNE( \$varC4, \$varC5 )/g;
  $szData=~s/PCG/_boolNE( \$varC4, \$varC6 )/g;
  $szData=~s/PCD/_boolNE( \$varC4, \$varC7 )/g;
  $szData=~s/PCF/_boolNE( \$varC4, \$varC8 )/g;
  
  $szData=~s/ && / and /g;
  $szData=~s/\nif\s\(/\n    if (/sg;
  $szData=~s/\}\selse\s\{/    } else {/g;
  $szData=~s/\}\n/    }\n/g;
  # point set
  $szData=~s/P\(\s*(\d)\s*,\s*(\d)\s*\)/    \$varE$2$1/g;
  # interpolate neighbour points
  $szData=~s/I1\(\s*(\d+)\s*\)/\$varC$1/g;
  $szData=~s/I2\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/_varInterp2(\$varC$3,\$varC$4,$1,$2)/g;
  $szData=~s/I3\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/_varInterp3(\$varC$4,\$varC$5,\$varC$6,$1,$2,$3)/g;
  $szData=~s/I4\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/_varInterp4(\$varC$5,\$varC$6,\$varC$7,\$varC$8,$1,$2,$3,$4)/g;
  # interpolate whatever
  $szData=~s/I1\(\s*(\S+)\s*\)/\$1/g;
  $szData=~s/I2\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\S+)\s*,\s*(\S+)\s*\)/_varInterp2($3,$4,$1,$2)/g;
  $szData=~s/I3\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\S+)\s*,\s*(\S+)\s*,\s*(\S+)\s*\)/_varInterp3($4,$5,$6,$1,$2,$3)/g;
  $szData=~s/I4\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\S+)\s*,\s*(\S+)\s*,\s*(\S+)\s*,\s*(\S+)\s*\)/_varInterp4($5,$6,$7,$8,$1,$2,$3,$4)/g;
  
  $szData=~s/\n\s\s\}\selsif\s\($/\n  \}/s;
  
  my $szData2='
# standard '.$szCode.' casepath
sub _arr'.$szCode.' {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8';
  my $boolNeedsPattern= boolFALSE();
  if (boolInstr($szData, '$intPattern', boolTRUE())) {
  $szData2.=',
    $intPattern';
    $boolNeedsPattern= boolTRUE();
  }
  $szData2.='
  )=@_;';
  my ($intX,$intY)=@{arrMatchRegEx($szCode, '/(\\d+)X(\\d+)?/i')};
  if (not defined $intY) {$intY=$intX};
  for (my $intI=0;$intI<$intY;$intI++) {
    $szData2.='
  my ( ';
    # x
    for (my $intJ=0;$intJ<$intX;$intJ++) {
      $szData2.='$varE'.$intI.$intJ.', '
    }
    $szData2.=' ) = ( ';
    # x
    for (my $intJ=0;$intJ<$intX;$intJ++) {
      $szData2.='$varC4, '
    }
    $szData2.=');';
  }
  $szData2.='
  #BEGIN '.$szCode.' PATTERNS
'.$szData.'
  #END '.$szCode.' PATTERNS';
  $szData2.='
  return (';
  for (my $intI=0;$intI<$intY;$intI++) {
    $szData2.='
    ';
    # x
    for (my $intJ=0;$intJ<$intX;$intJ++) {
      $szData2.='$varE'.($intI).($intJ).', '
    }
  }
  $szData2.='
  );
}
';
  $hashFilter->{$szCode}= [$intX,$intY,$boolNeedsPattern];
  return($szData2);
}