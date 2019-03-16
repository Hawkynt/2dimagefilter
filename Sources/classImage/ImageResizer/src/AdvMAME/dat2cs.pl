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

modGlobal::voidWriteFileBinary($FindBin::Bin.'/output.cs', $szData);

sub _szProcess {
  my ($szFile)=@_;
  my $szData=szJoin("\n",modGlobal::arrReadFile($szFile));
  $szFile= modGlobal::szExtractFileName($szFile);
  print 'Processing '.$szFile."\n";
  my $szCode= szLeft($szFile, intLen($szFile)-intLen(modGlobal::szExtractExtension($szFile))-1);
  $szData=~s/\/\*\s.*?\s\*\///gs;
  $szData=~s/^[\s]*$//gm;
  $szData=~s/\t/    /g;
  
  $szData=~s/!MUH/stC1.IsLike( stC0 )/g;
  $szData=~s/!MUE/stC1.IsLike( stC2 )/g;
  $szData=~s/!MUL/stC1.IsLike( stC3 )/g;
  $szData=~s/!MUR/stC1.IsLike( stC5 )/g;
  $szData=~s/!MUD/stC1.IsLike( stC7 )/g;
  $szData=~s/!MDL/stC7.IsLike( stC3 )/g;
  $szData=~s/!MDR/stC7.IsLike( stC5 )/g;
  $szData=~s/!MDG/stC7.IsLike( stC6 )/g;
  $szData=~s/!MDF/stC7.IsLike( stC8 )/g;
  $szData=~s/!MLR/stC3.IsLike( stC5 )/g;
  $szData=~s/!MLH/stC3.IsLike( stC0 )/g;
  $szData=~s/!MLG/stC3.IsLike( stC6 )/g;
  $szData=~s/!MRE/stC5.IsLike( stC2 )/g;
  $szData=~s/!MRF/stC5.IsLike( stC8 )/g;
  $szData=~s/!MHE/stC0.IsLike( stC2 )/g;
  $szData=~s/!MGF/stC6.IsLike( stC8 )/g;
  $szData=~s/!MHG/stC0.IsLike( stC6 )/g;
  $szData=~s/!MEF/stC2.IsLike( stC8 )/g;
  $szData=~s/!MCH/stC4.IsLike( stC0 )/g;
  $szData=~s/!MCU/stC4.IsLike( stC1 )/g;
  $szData=~s/!MCE/stC4.IsLike( stC2 )/g;
  $szData=~s/!MCL/stC4.IsLike( stC3 )/g;
  $szData=~s/!MCR/stC4.IsLike( stC5 )/g;
  $szData=~s/!MCG/stC4.IsLike( stC6 )/g;
  $szData=~s/!MCD/stC4.IsLike( stC7 )/g;
  $szData=~s/!MCF/stC4.IsLike( stC8 )/g;
  
  $szData=~s/MUH/stC1.IsNotLike( stC0 )/g;
  $szData=~s/MUE/stC1.IsNotLike( stC2 )/g;
  $szData=~s/MUL/stC1.IsNotLike( stC3 )/g;
  $szData=~s/MUR/stC1.IsNotLike( stC5 )/g;
  $szData=~s/MUD/stC1.IsNotLike( stC7 )/g;
  $szData=~s/MDL/stC7.IsNotLike( stC3 )/g;
  $szData=~s/MDR/stC7.IsNotLike( stC5 )/g;
  $szData=~s/MDG/stC7.IsNotLike( stC6 )/g;
  $szData=~s/MDF/stC7.IsNotLike( stC8 )/g;
  $szData=~s/MLR/stC3.IsNotLike( stC5 )/g;
  $szData=~s/MLH/stC3.IsNotLike( stC0 )/g;
  $szData=~s/MLG/stC3.IsNotLike( stC6 )/g;
  $szData=~s/MRE/stC5.IsNotLike( stC2 )/g;
  $szData=~s/MRF/stC5.IsNotLike( stC8 )/g;
  $szData=~s/MHE/stC0.IsNotLike( stC2 )/g;
  $szData=~s/MGF/stC6.IsNotLike( stC8 )/g;
  $szData=~s/MHG/stC0.IsNotLike( stC6 )/g;
  $szData=~s/MEF/stC2.IsNotLike( stC8 )/g;
  $szData=~s/MCH/stC4.IsNotLike( stC0 )/g;
  $szData=~s/MCU/stC4.IsNotLike( stC1 )/g;
  $szData=~s/MCE/stC4.IsNotLike( stC2 )/g;
  $szData=~s/MCL/stC4.IsNotLike( stC3 )/g;
  $szData=~s/MCR/stC4.IsNotLike( stC5 )/g;
  $szData=~s/MCG/stC4.IsNotLike( stC6 )/g;
  $szData=~s/MCD/stC4.IsNotLike( stC7 )/g;
  $szData=~s/MCF/stC4.IsNotLike( stC8 )/g;
  
  $szData=~s/!PUH/(stC1== stC0 )/g;
  $szData=~s/!PUE/(stC1== stC2 )/g;
  $szData=~s/!PUL/(stC1== stC3 )/g;
  $szData=~s/!PUR/(stC1== stC5 )/g;
  $szData=~s/!PUD/(stC1== stC7 )/g;
  $szData=~s/!PDL/(stC7== stC3 )/g;
  $szData=~s/!PDR/(stC7== stC5 )/g;
  $szData=~s/!PDG/(stC7== stC6 )/g;
  $szData=~s/!PDF/(stC7== stC8 )/g;
  $szData=~s/!PLR/(stC3== stC5 )/g;
  $szData=~s/!PLH/(stC3== stC0 )/g;
  $szData=~s/!PLG/(stC3== stC6 )/g;
  $szData=~s/!PRE/(stC5== stC2 )/g;
  $szData=~s/!PRF/(stC5== stC8 )/g;
  $szData=~s/!PHE/(stC0== stC2 )/g;
  $szData=~s/!PGF/(stC6== stC8 )/g;
  $szData=~s/!PHG/(stC0== stC6 )/g;
  $szData=~s/!PEF/(stC2== stC8 )/g;
  $szData=~s/!PCH/(stC4== stC0 )/g;
  $szData=~s/!PCU/(stC4== stC1 )/g;
  $szData=~s/!PCE/(stC4== stC2 )/g;
  $szData=~s/!PCL/(stC4== stC3 )/g;
  $szData=~s/!PCR/(stC4== stC5 )/g;
  $szData=~s/!PCG/(stC4== stC6 )/g;
  $szData=~s/!PCD/(stC4== stC7 )/g;
  $szData=~s/!PCF/(stC4== stC8 )/g;
  
  $szData=~s/PUH/(stC1!= stC0 )/g;
  $szData=~s/PUE/(stC1!= stC2 )/g;
  $szData=~s/PUL/(stC1!= stC3 )/g;
  $szData=~s/PUR/(stC1!= stC5 )/g;
  $szData=~s/PUD/(stC1!= stC7 )/g;
  $szData=~s/PDL/(stC7!= stC3 )/g;
  $szData=~s/PDR/(stC7!= stC5 )/g;
  $szData=~s/PDG/(stC7!= stC6 )/g;
  $szData=~s/PDF/(stC7!= stC8 )/g;
  $szData=~s/PLR/(stC3!= stC5 )/g;
  $szData=~s/PLH/(stC3!= stC0 )/g;
  $szData=~s/PLG/(stC3!= stC6 )/g;
  $szData=~s/PRE/(stC5!= stC2 )/g;
  $szData=~s/PRF/(stC5!= stC8 )/g;
  $szData=~s/PHE/(stC0!= stC2 )/g;
  $szData=~s/PGF/(stC6!= stC8 )/g;
  $szData=~s/PHG/(stC0!= stC6 )/g;
  $szData=~s/PEF/(stC2!= stC8 )/g;
  $szData=~s/PCH/(stC4!= stC0 )/g;
  $szData=~s/PCU/(stC4!= stC1 )/g;
  $szData=~s/PCE/(stC4!= stC2 )/g;
  $szData=~s/PCL/(stC4!= stC3 )/g;
  $szData=~s/PCR/(stC4!= stC5 )/g;
  $szData=~s/PCG/(stC4!= stC6 )/g;
  $szData=~s/PCD/(stC4!= stC7 )/g;
  $szData=~s/PCF/(stC4!= stC8 )/g;
  
  $szData=~s/\nif\s\(/\n    if (/sg;
  $szData=~s/\}\selse\s\{/    } else {/g;
  $szData=~s/\}\n/    }\n/g;
  # point set
  $szData=~s/P\(\s*(\d)\s*,\s*(\d)\s*\)/    stE$2$1/g;
  # interpolate neighbour points
  $szData=~s/IC\(\s*(\d+)\s*\)/stC$1/g;
  $szData=~s/I1\(\s*(\d+)\s*\)/stC$1/g;
  $szData=~s/I2\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$3,stC$4,$1,$2)/g;
  $szData=~s/I3\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$4,stC$5,stC$6,$1,$2,$3)/g;
  $szData=~s/I4\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$5,stC$6,stC$7,stC$8,$1,$2,$3,$4)/g;
  # interpolate whatever
  $szData=~s/I1\(\s*(\S+)\s*\)/\$1/g;
  $szData=~s/I2\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\S+)\s*,\s*(\S+)\s*\)/sPixel.Interpolate($3,$4,$1,$2)/g;
  $szData=~s/I3\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\S+)\s*,\s*(\S+)\s*,\s*(\S+)\s*\)/sPixel.Interpolate($4,$5,$6,$1,$2,$3)/g;
  $szData=~s/I4\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\S+)\s*,\s*(\S+)\s*,\s*(\S+)\s*,\s*(\S+)\s*\)/sPixel.Interpolate($5,$6,$7,$8,$1,$2,$3,$4)/g;
  # interpolate hard
  $szData=~s/I31\(\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$1,stC$2,3,1)/g;
  $szData=~s/I211\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$1,stC$2,stC$3,2,1,1)/g;
  $szData=~s/I332\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$1,stC$2,stC$3,3,3,2)/g;
  $szData=~s/I611\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$1,stC$2,stC$3,6,1,1)/g;
  $szData=~s/I1411\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/sPixel.Interpolate(stC$1,stC$2,stC$3,14,1,1)/g;
  
  my $szData2='
    #region standard '.$szCode.' casepath
    public static sPixel[] _arr'.$szCode.'(byte bytePattern,sPixel stC0,sPixel stC1,sPixel stC2,sPixel stC3,sPixel stC4,sPixel stC5,sPixel stC6,sPixel stC7,sPixel stC8) {';
  my ($intX,$intY)=@{arrMatchRegEx($szCode, '/(\\d+)X(\\d+)?/i')};
  if (not defined $intY) {$intY=$intX};
  for (my $intI=0;$intI<$intY;$intI++) {
    # x
    for (my $intJ=0;$intJ<$intX;$intJ++) {
      $szData2.='
      sPixel stE'.$intI.$intJ.'=stC4;';
    }
  }
  my $boolNeedsPattern=boolFALSE();
  if(boolInstr($szData,"case")) {
    $szData2.='
      switch(bytePattern){';
    $boolNeedsPattern=boolTRUE();
  }
  
  $szData2.='
      #region '.$szCode.' PATTERNS
'.$szData.'
      #endregion';
  if($boolNeedsPattern) {
    $szData2.='
    }';
  }
  $szData2.='
      return (new sPixel[]{';
  for (my $intI=0;$intI<$intY;$intI++) {
    $szData2.='
      ';
    # x
    for (my $intJ=0;$intJ<$intX;$intJ++) {
      $szData2.='stE'.($intI).($intJ).', '
    }
  }
  $szData2.='
      });
    }
    #endregion';
  $hashFilter->{$szCode}= [$intX,$intY,$boolNeedsPattern];
  return($szData2);
}