package Modules::IL::Filter;
use strict;
use warnings;

# makro speedup filter for modIL
use Filter::Simple;
FILTER_ONLY 'all'=>sub {
  s/(\W)varUNDEF[\(][\)]/$1undef/mg;
  s/(\W)boolTRUE[\(][\)]/${1}1/mg;
  s/(\W)boolFALSE[\(][\)]/${1}0/mg;
  
  # repeat 3
  s/(\W)arr[\(]([^\(\)]*)[\)]/$1\[$2\]/mg;
  s/(\W)hash[\(]([^\(\)]*)[\)]/$1\{$2\}/mg;
  
  s/(\W)varArrayItem[\(]([^\(\),]*),([^\(\)]*)[\)]/$1$2\-\>\[$3\]/mg;
  s/(\W)varHashItem[\(]([^\(\),]*),([^\(\)]*)[\)]/$1$2\-\>\{$3\}/mg;
  s/(\W)szCombineStrings[\(]([^\(\),]*),([^\(\)]*)[\)]/$1$2\.$3/mg;
  
  # repeat 2
  s/(\W)arr[\(]([^\(\)]*)[\)]/$1\[$2\]/mg;
  s/(\W)hash[\(]([^\(\)]*)[\)]/$1\{$2\}/mg;
  
  s/(\W)varArrayItem[\(]([^\(\),]*),([^\(\)]*)[\)]/$1$2\-\>\[$3\]/mg;
  s/(\W)varHashItem[\(]([^\(\),]*),([^\(\)]*)[\)]/$1$2\-\>\{$3\}/mg;
  s/(\W)szCombineStrings[\(]([^\(\),]*),([^\(\)]*)[\)]/$1$2\.$3/mg;
  
  # repeat 1
  s/(\W)arr[\(]([^\(\)]*)[\)]/$1\[$2\]/mg;
  s/(\W)hash[\(]([^\(\)]*)[\)]/$1\{$2\}/mg;
  
  s/(\W)varArrayItem[\(]([^\(\),]+),([^\(\)]+)[\)]/$1$2\-\>\[$3\]/mg;
  s/(\W)varHashItem[\(]([^\(\),]+),([^\(\)]+)[\)]/$1$2\-\>\{$3\}/mg;
  s/(\W)szCombineStrings[\(]([^\(\),]+),([^\(\)]+)[\)]/$1$2\.$3/mg;
  
  # rest
  s/(\W)voidPush[\(]([^\(\),]*),([^\(\)]+)[\)];/$1push \@\{$2\},$3;/mg;
  s/(\W)boolIsEqual[\(]([^\(\),]+),([^\(\)]+)[\)]/$1\($2 eq $3\)/mg;
  s/(\W)charCharAt[\(]([^\(\),]+),([^\(\)]+)[\)]/$1substr\($2,$3,1\)/mg;
  s/(\W)boolKeyExists[\(]([^\(\),]+),([^\(\)]+)[\)]/$1exists\($2->\{$3\}\)/mg;
  s/(\W)szRight[\(]([^\(\),]+),([^\(\)]+)[\)]/$1substr\($2,-1*\($3\)\)/mg;
  s/(\W)szLeft[\(]([^\(\),]+),([^\(\)]+)[\)]/$1substr\($2,0,$3\)/mg;
  
  s/(\W)intInstr[\(]([^\(\),]+),([^\(\),]+)[\)]/$1index\($2,$3\)/mg;
  s/(\W)intRInstr[\(]([^\(\),]+),([^\(\),]+)[\)]/$1rindex\($2,$3\)/mg;
  s/(\W)boolInstr[\(]([^\(\),]+),([^\(\),]+)[\)]/$1\(index\($2,$3\)>-1\)/mg;
  s/(\W)boolInstr[\(]([^\(\),]+),([^\(\),]+),[\s]*1[\s]*[\)]/$1\(index\(lc\($2\),lc\($3\)\)>-1\)/mg;
  
  s/(\W)szMid[\(]([^\(\),]+),([^\(\),]+),([^\(\)]+)[\)]/$1substr\($2,$3,$4\)/mg;
  
  s/(\W)voidArrayItem[\(]([^\(\),]+),([^\(\),]+),([^\(\)]+)[\)]/$1$2\-\>\[$3\]\=$4/mg;
  s/(\W)voidHashItem[\(]([^\(\),]+),([^\(\),]+),([^\(\)]+)[\)]/$1$2\-\>\{$3\}\=$4/mg;
  
  s/(\W)arrGetKeys[\(](\$[^\(\)]+)[\)]/$1\[keys\%\{$2\}\]/mg;
  s/(\W)arrSort[\(](\$[^\(\)]+)[\)]/$1\[sort\{\$a cmp \$b\}\(\@\{$2\}\)\]/mg;
  
  s/(\W)szPrintf[\(]([^,]+)[\s]*,[\s]*[\[]([^\(\)]+)[\]][\s]*[\)]/$1sprintf\($2,$3\)/mg;
  s/(\W)szPrintf[\(](\'[^\']+\')[\s]*,[\s]*[\[]([^\(\)]+)[\]][\s]*[\)]/$1sprintf\($2,$3\)/mg;
  
  s/(\W)szJoin[\(]([^,]+)[\s]*,[\s]*[\[]([^\(\)]+)[\]][\s]*[\)]/$1join\($2,$3\)/mg;
  s/(\W)szJoin[\(]([^,]+)[\s]*,[\s]*[\$]([^\(\)]+)[\s]*[\)]/$1join\($2,@\{\$$3\}\)/mg;
  
  s/(\W)boolNot[\(]/$1not\(/mg;
  s/(\W)intLen[\(]/$1length\(/mg;
  s/(\W)boolIsDefined[\(]/$1defined\(/mg;
  s/(\W)intInt[\(]/$1int\(/mg;
  s/(\W)intAbs[\(]/$1abs\(/mg;
  s/(\W)szLCase[\(]/$1lc\(/mg;
  s/(\W)szUCase[\(]/$1uc\(/mg;
  s/(\W)charFromCode[\(]/$1chr\(/mg;
  s/(\W)intCharCode[\(]/$1ord\(/mg;
  
  s/(\W)intArrLen[\(]([^\(\)]+)[\)]/$1scalar\(\@\{$2\}\)/mg;
  
};

1;
