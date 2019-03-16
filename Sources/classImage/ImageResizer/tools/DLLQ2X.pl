use stricT;
use warnings;
use Modules::modIL;
use Modules::modGlobal;
my $szFile='classImage.pm';
my $szOutFile='classImage.ok.pm';

our $szLE=$modGlobal::szLE;

my $szData=szJoin($szLE,modGlobal::arrReadFile($szFile));
my $arrReplaceTBL=[
  ['I([0-9])([0-9])([0-9])[\(][\s]*([0-9]),[\s]*([0-9]),[\s]*([0-9])[\s]*[\)]','_varInterp3(\$varC$4,\$varC$5,\$varC$6,$1,$2,$3)'],
  ['I1411[\(][\s]*([0-9]),[\s]*([0-9]),[\s]*([0-9])[\s]*[\)]','_varInterp3(\$varC$1,\$varC$2,\$varC$3,14,1,1)'],
  ['I([0-9])([0-9])[\(][\s]*([0-9]),[\s]*([0-9])[\s]*[\)]','_varInterp2(\$varC$3,\$varC$4,$1,$2)'],
];

foreach my $arrC (@{$arrReplaceTBL}) {
  my $szWhat=$arrC->[0];
  my $szNew=$arrC->[1];
  $szData=szReplaceRegEx($szData,$szWhat,$szNew,boolTRUE());
}
modGlobal::voidWriteFile($szOutFile,arr($szData));
print 'OK';