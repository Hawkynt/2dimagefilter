use stricT;
use warnings;
use Modules::modIL;
use Modules::modGlobal;
my $szFile='classImage.pm';
my $szOutFile='classImage.ok.pm';

our $szLE=$modGlobal::szLE;

my $szData=szJoin($szLE,modGlobal::arrReadFile($szFile));

while ($szData=~m/(boolInstr\(';([0-9;]+);',\$szPattern\))/g) {
  my $arrIDs=arrSplit(';',$2);
  my $szWhat=$1;
  my $szReplace=$szLE;
  for (my $intI=0;$intI<intArrLen($arrIDs);$intI++) {
    $szReplace.=szPrintf('    ($intPattern==%d)',arr($arrIDs->[$intI]));
    if ($intI<(intArrLen($arrIDs)-1)) {
      $szReplace.=' or'
    }
    $szReplace.=$szLE;
  }
  $szReplace.='  ';
  $szData=szReplace($szData,$szWhat,$szReplace);
}
modGlobal::voidWriteFile($szOutFile,arr($szData));
print 'OK';