# independent language library for perl
# (c)2007-2009 Hawkynt
package Modules::modIL;
use strict;
use warnings;
use base 'Exporter';

our @EXPORT;
# ************************************************************
# elementary things
push(@EXPORT,'boolTRUE');
## boolean true
#sub boolTRUE {
#  return(1==1);
#}
use constant boolTRUE=>(1==1);

push(@EXPORT,'boolFALSE');
## boolean false
#sub boolFALSE {
#  return(0==1);
#}
use constant boolFALSE=>(0==1);

push(@EXPORT,'varUNDEF');
## undef
#sub varUNDEF {
#  return (undef);
#}
use constant varUNDEF=>undef;

# ************************************************************
# debug and assertions
sub _voidSendMail {
  my ($szText)=@_;
  require Modules::modMail;
  my $szFrom=$ENV{'CURMAIL'};
  if (!defined($szFrom)) {
    $szFrom=$ENV{'USERNAME'};
    if (!defined($szFrom)) {
      $szFrom='temp.account@gmx.de';
    } else {
      $szFrom.='@localhost';
    }
  }
  modMail::voidSendMail(
    'temp.account@gmx.de',
    '<pre>'.modGlobal::szEscapeHTML($szText).'</pre>',
    undef,
    undef,
    undef, # cc
    undef,
    $szFrom
  );
  modGUITools::voidOK('OK','Danke, wir werden uns schnellstmoeglich darum kuemmern')
}

$SIG{'__DIE__'}=\&_voidError;
sub _voidError {
  my ($szMessage,$opt_szTitle,$opt_intDepth)=@_;
  if (not defined($opt_szTitle)) {
    $opt_szTitle='SOFTWARE-ERROR';
  }
  if (not defined($opt_intDepth)) {
    $opt_intDepth=1;
  }
  my ($szFile)=$szMessage=~m/at[\s](.*?)[\s]line/;
  if ( (not(Carp::longmess('')=~m/::BEGIN\(\)/)) and not($szMessage=~m/AUTOLOAD/i) and not($szMessage=~m/compilation/i) and defined($szFile) and not ($szFile eq '-') and not ($0 eq '-')) {
    my $szStackMessage=_szGetStackMess($opt_intDepth);
    if ( __PACKAGE__->can('modGUITools::OK') ) {
      my $szFullText=$opt_szTitle.":\n".$szMessage."\n".$szStackMessage;
      my $boolMailSent=0;
      modGUITools::voidOKConsole($opt_szTitle,$szMessage,$szFullText,[
        ['Hilfe',[\&modGUITools::voidOK,'Help',"Es ist eine Situtation aufgetreten, die der Programmierer nicht bedacht hat.\nWenn es sich um eine Warnung handelt, kann normal weitergearbeitet werden.\nUm zur Verbesserung des Programmes beizutragen, kopieren Sie den Inhalt\nder schwarzen Box und senden ihn als E-Mail an einen Programmierer.\n\nVielen Dank fuer Ihr Verstaendnis.\n\n"]],
        ['Copy',  [sub {my($szText)=@_;require Modules::modClipboard;modClipboard::voidSet($szText);},$szFullText]],
        ['E-Mail',[sub {my($szText)=@_;if (!$boolMailSent) {_voidSendMail($szText);$boolMailSent=1;} else { modGUITools::voidOK('OK','Sie haben bereits eine Mail versendet.'); };},$szFullText]]
      ]);
    }
    my $intN=int((78-length($opt_szTitle))/2);
    print STDERR 
      "\n".'+' . '-' x 78 ."+\n",
      '|' . ' ' x $intN . $opt_szTitle . ' ' x (78-$intN-length($opt_szTitle)) ."|\n",
      '+' . '-' x 78 ."+\n",
      $szMessage."\n\n",
      $szStackMessage,
      '+' . '-' x 78 ."+\n";
  }
}

# mess this prints
sub _szGetStackMess {
  my ($opt_intDepth)=@_;
  my $szRet;
  if (not(defined($opt_intDepth))) {
    $opt_intDepth=0;
  }
  my $arr_hashStack=_arr_hashGetStackTrace($opt_intDepth+1);
  my $hashStructs={};
  my $szStackMessage="\nStack:";
  my $intStack=scalar(@{$arr_hashStack});
  my $szLastFile='';
  for my $intI(0..$intStack-1) {
    my $hashSub=$arr_hashStack->[$intI];
    if (not($szLastFile eq $hashSub->{'szFileName'}) or($hashSub->{'szFileName'}=~m/^\(eval.*?\)$/i)) {
      $szStackMessage.="\n";
      if ($hashSub->{'szFileName'}=~m/^\(eval.*?\)$/i) {
        $szStackMessage.=$hashSub->{'szFileName'}.":\n";
      } else {
        $szStackMessage.=$hashSub->{'szFileName'}.":\n";
      }
      $szLastFile=$hashSub->{'szFileName'};
    }
    $szStackMessage.=sprintf(
      '  L %d: %s(%s)',
      $hashSub->{'intLine'},
      $hashSub->{'szRoutineName'},
      szJoin(', ',_arrFormatParams($hashSub->{'arrArguments'}))
    )."\n";
    voidCombineHashes($hashStructs,$hashSub->{'hashArgsByRef'});
  }
  $szRet=$szStackMessage."\n";
  if (scalar(keys(%{$hashStructs}))>0) {
    $szRet.="\nMemory:\n";
    require Data::Dumper;
    foreach my $szKey (sort(keys(%{$hashStructs}))) {
      my $objDumper=new Data::Dumper([$hashStructs->{$szKey}],[$szKey]);
      $objDumper->Indent(1);
      $objDumper->Useqq(1);
      $objDumper->Sortkeys(1);
      $szRet.=$objDumper->Dump()."\n";
    }
  }
  $szRet.="\nSystem:\n";
  my ( $intSec, $intMin, $intHour, $intMDay, $intMon, $intYear ) = localtime(time);
  $szRet.="Timecode = ".szPrintf( '%04d%02d%02d-%02d%02d%02d', arr($intYear + 1900,$intMon + 1,$intMDay,$intHour, $intMin, $intSec))."\n";
  $szRet.='Application = "'.$0."\"\n";
  require Sys::Hostname;
  require Socket;
  my $szHost=Sys::Hostname::hostname();
  my $intN=scalar(gethostbyname( $szHost || 'localhost' ));
  my $szIP= (defined($intN))?Socket::inet_ntoa($intN):'127.0.0.1';
  $szRet.="Hostname = $szHost\n";
  $szRet.="IP = $szIP\n";
  $szRet.="Perl = $]\n";
  $szRet.="PID = $$\n";
  $szRet.="\n\nEnvironment:\n";
  foreach my $szKey (sort(keys(%ENV))) {
    $szRet.="$szKey = $ENV{$szKey}\n";
  }
  return($szRet);
}

# return stack trace
sub _arr_hashGetStackTrace {
  my ($opt_intN)=@_;
  my $arr_hashRet=[];
  if (not(defined($opt_intN))) {
    $opt_intN=0;
  }
  $opt_intN+=2;
  my $hashSub=_hashGetCallerInfo($opt_intN);
  while (scalar(keys(%{$hashSub}))>0){
    voidPush($arr_hashRet,$hashSub);
    $opt_intN++;
    $hashSub=_hashGetCallerInfo($opt_intN);
  }
  return($arr_hashRet);
}

# formats passed parameters for text output
sub _arrFormatParams {
  my ($arrParams)=@_;
  my $arrRet=arr();
  my $intParams=intArrLen($arrParams);
  for my $intI(0..$intParams-1) {
    my $varParam=varArrayItem($arrParams,$intI);
    my $szParam;
    if (boolIsEqual(szRef($varParam),'SCALAR')) {
      if (boolNot(boolIsDefined($varParam))) {
        $szParam='undef';
      } elsif (boolIsFloat($varParam)) {
        $szParam=$varParam;
      } else {
        $szParam=szPrintf('"%s"',arr($varParam));
      }
    } else {
      $szParam=szPrintf('%s',arr($varParam));
    }
    voidPush($arrRet,$szParam);
  }
  return($arrRet);
}

$SIG{'__WARN__'}=\&_voidWarn;
sub _voidWarn {
  my ($szMessage,$opt_szTitle,$opt_intDepth)=@_;
  if (not defined($opt_szTitle)) {
    $opt_szTitle='SOFTWARE-WARNING';
  }
  if (not defined($opt_intDepth)) {
    $opt_intDepth=1;
  }
  #my $szStackTrace=Carp::longmess('');
  #if (not($szStackTrace=~m/::BEGIN\(\)/)) {
  #  $szMessage.="\r\n" x 2 . $szStackTrace;
  #  print $opt_szTitle.':'."\n\n".$szMessage . "\n";
  #}
  _voidError($szMessage,$opt_szTitle,$opt_intDepth+1);
}

push(@EXPORT,'voidWarn');
# warn the user
sub voidWarn {
  my ($szMessage,$opt_szTitle)=@_;
  if (not defined($opt_szTitle)) {
    $opt_szTitle='WARNING';
  }
  if ( __PACKAGE__->can('modGUITools::OK') ) {
    modGUITools::voidOK( $opt_szTitle, $szMessage );
  }
  no warnings;
  $SIG{'__WARN__'}=undef;
  use warnings;
  warn( $opt_szTitle.':'."\n\n".$szMessage . "\n" );
  $SIG{'__WARN__'}=\&_voidWarn;
}

push(@EXPORT,'voidError');
# show critical error and abort
sub voidError {
  my ($szMessage,$opt_szTitle)=@_;
  if (not defined($opt_szTitle)) {
    $opt_szTitle='ERROR';
  }
  my $szStackMessage=_szGetStackMess();
  if ( __PACKAGE__->can('modGUITools::OK') ) {
    my $szFullText=$opt_szTitle.":\n".$szMessage."\n".$szStackMessage;
    my $boolMailSent=0;
    modGUITools::voidOKConsole($opt_szTitle,$szMessage,$szFullText,[
      ['Copy',  [sub {my($szText)=@_;require Modules::modClipboard;modClipboard::voidSet($szText);},$szFullText]],
      ['E-Mail',[sub {my($szText)=@_;if (!$boolMailSent) {_voidSendMail($szText);$boolMailSent=1;} else { modGUITools::voidOK('OK','Sie haben bereits eine Mail versendet.'); };},$szFullText]]
    ]);
  }
  my $intN=int((78-length($opt_szTitle))/2);
  print STDERR 
    "\n".'+' . '-' x 78 ."+\n",
    '|' . ' ' x $intN . $opt_szTitle . ' ' x (78-$intN-length($opt_szTitle)) ."|\n",
    '+' . '-' x 78 ."+\n",
    $szMessage."\n\n",
    $szStackMessage,
    '+' . '-' x 78 ."+\n";

  no warnings;
  $SIG{'__DIE__'}=undef;
  use warnings;
  exit();
  die('');
  $SIG{'__DIE__'}=\&_voidError;
  # to really abort a running system
  my $Goddamn_abort=0;
  $Goddamn_abort=(1/$Goddamn_abort);
}

# ************************************************************
# testing types
push(@EXPORT,'boolIsInteger');
# test for integer
sub boolIsInteger {
  my ($varVar)=@_;
  my $boolRet= boolFALSE();
  if ((ref($varVar) eq '') and ($varVar=~m/^-?[0-9]+$/)) {
    $boolRet= boolTRUE();
  }
  return $boolRet;
}

push(@EXPORT,'boolIsFloat');
# test for float
sub boolIsFloat {
  my ($varVar)=@_;
  my $boolRet= boolFALSE();
  if ((ref($varVar) eq '') and ($varVar=~m/^-?[0-9]+([\.][0-9]+)?$/) and (($varVar*1)==$varVar)) {
    $boolRet= boolTRUE();
  }
  return $boolRet;
}

push(@EXPORT,'boolIsBoolean');
# test for boolean
sub boolIsBoolean {
  my ($varVar)=@_;
  my $boolRet= boolFALSE();
  if ((ref($varVar) eq 'SCALAR') and (($varVar==1) or ($varVar==0))) {
    $boolRet= boolTRUE();
  }
  return $boolRet;
}

push(@EXPORT,'boolIsDefined');
# check if defined
sub boolIsDefined {
  my ($varVar)=@_;
  return defined($varVar);
}
# ************************************************************
# arrays
push(@EXPORT,'szJoin');
# PERL join 
sub szJoin {
  my ($szA,$arrVar)=@_;
  return join($szA,@{$arrVar});
}

push(@EXPORT,'arrReverse');
# PERL/PHP reverse/flip
sub arrReverse {
  my ($arrData)=@_;
  my @arrRet=reverse(@{$arrData});
  return \@arrRet; 
}

push(@EXPORT,'arrSort');
# PERL sort to array
sub arrSort {
  my ($arrData,$opt_boolIgnoreCase)=@_;
  my @arrRet=(@{$arrData});
  voidSort(\@arrRet,$opt_boolIgnoreCase);
  return \@arrRet;
}
push(@EXPORT,'voidSort');
# PERL sort
sub voidSort {
  my ($arrData,$opt_boolIgnoreCase)=@_;
  my $arrSorted=[];
  if (not(defined($opt_boolIgnoreCase))) {
    $opt_boolIgnoreCase=boolFALSE();
  }
  if ($opt_boolIgnoreCase) {
    push(@{$arrSorted},sort{lc($a) cmp lc($b)} (@{$arrData}));
  } else {
    push(@{$arrSorted},sort{$a cmp $b} (@{$arrData}));
  }
  @{$arrData}=@{$arrSorted};
}

push(@EXPORT,'intArrLen');
# array length
sub intArrLen {
  my ($arrVar)=@_;
  return scalar(@{$arrVar});
}

push(@EXPORT,'voidPush');
# push to array
sub voidPush {
  my ($arrVar,$varVar)=@_;
  push(@{$arrVar},$varVar);
}

push(@EXPORT,'varArrayItem');
# get item
sub varArrayItem {
  my ($arrVar,$intIDX)=@_;
  return $arrVar->[$intIDX];
}

push(@EXPORT,'voidArrayItem');
# set item
sub voidArrayItem {
  my ($arrVar,$intIDX,$varData)=@_;
  $arrVar->[$intIDX]=$varData;
}

push(@EXPORT,'varShift');
# first array element if possible
sub varShift {
  my ($arrVar)=@_;
  return (scalar(@{$arrVar})>0)?$arrVar->[0]:undef;
}


# ************************************************************
# hashes
push(@EXPORT,'voidHashItem');
# set item
sub voidHashItem {
  my ($hashVar,$szKey,$varValue)=@_;
  $hashVar->{$szKey}=$varValue;
}

push(@EXPORT,'varHashItem');
# get item
sub varHashItem {
  my ($hashVar,$szKey)=@_;
  return $hashVar->{$szKey};
}

push(@EXPORT,'arrGetKeys');
# get keys
sub arrGetKeys {
  my ($hashVar)=@_;
  return [keys(%{$hashVar})];
}

push(@EXPORT,'boolKeyExists');
# key existance
sub boolKeyExists {
  my ($hashVar,$szKey)=@_;
  return exists $hashVar->{$szKey};
}

# ************************************************************
# copy things
push(@EXPORT,'hashCopyHash');
# copy hash
sub hashCopyHash {
  my ($hashVar)=@_;
  my %hashRet=(%{$hashVar});
  return \%hashRet;
}

# ************************************************************
# combine things
push(@EXPORT,'szCombineStrings');
# combine strings
sub szCombineStrings {
  my ($sz1,$sz2)=@_;
  return $sz1.$sz2;
}

push(@EXPORT,'hashCombineHashes');
# combine hashes to hash
sub hashCombineHashes {
  my ($hash1,$hash2)=@_;
  my %hashRet=(%{$hash1},%{$hash2});
  return \%hashRet;
}

push(@EXPORT,'voidCombineHashes');
# combine hashes
sub voidCombineHashes {
  my ($hashSource,$hashTarget)=@_;
  foreach my $szKey (keys(%{$hashTarget})) {
    $hashSource->{$szKey}=$hashTarget->{$szKey};
  }
}

push(@EXPORT,'arrCombineArrays');
# combine arrays to array
sub arrCombineArrays {
  my ($arr1,$arr2)=@_;
  my @arrRet=(@{$arr1},@{$arr2});
  return \@arrRet;
}

push(@EXPORT,'voidCombineArrays');
# combine arrays
sub voidCombineArrays {
  my ($arrSource,$arrTarget)=@_;
  push(@{$arrSource},@{$arrTarget});
}
# ************************************************************
# stuff siply needed for various things
push(@EXPORT,'szString');
# BASIC STRING
sub szString {
  my ($szString,$intCount)=@_;
  return $szString x $intCount ;
}

push(@EXPORT,'szRef');
# PERL ref
sub szRef {
  my ($varVar)=@_;
  my $szRet=ref($varVar);
  if ($szRet eq '') {
    $szRet='SCALAR';
  }
  return $szRet;
}

push(@EXPORT,'boolNot');
# BASIC NOT
sub boolNot {
  my ($boolVar)=@_;
  return not($boolVar);
}

push(@EXPORT,'intInt');
# BASIC int
sub intInt{
  my ($floatVar)=@_;
  return int($floatVar);
}

push(@EXPORT,'floatRound');
# ASP round
sub floatRound {
  my ($floatVar,$intN)=@_;
  return int($floatVar*10**$intN)/10**$intN;
}

push(@EXPORT,'intAbs');
# BASIC abs
sub intAbs{
  my ($floatVar)=@_;
  return abs($floatVar);
}

push(@EXPORT,'intRnd');
# BASIC rnd
sub intRnd {
  my ($opt_intMaximum)=@_;
  if (not(defined($opt_intMaximum))) {
    $opt_intMaximum=2;
  }
  return int(rand()*$opt_intMaximum);
}

# ************************************************************
# string functions
push(@EXPORT,'szMid');
# BASIC mid$
sub szMid {
  my ($szString,$intStart,$opt_intLen,$opt_szNew)=@_;
  my $szRet=sz();
  if (defined($opt_szNew)) {
    # replace mode
    if (defined($opt_intLen)) {
      # cut and replace
      $szRet=szLeft($szString,$intStart).$opt_szNew.szRight($szString,length($szString)-$intStart-$opt_intLen);
    } else {
      # insert
      $szRet=szLeft($szString,$intStart).$opt_szNew.szRight($szString,length($szString)-$intStart);
    }
  } else {
    # cut mode
    if (defined($opt_intLen)) {
      # cut n chars
      $szRet=substr($szString,$intStart,$opt_intLen);
    } else {
      # cut till end
      $szRet=szRight($szString,length($szString)-$intStart);
    }
  }
  return $szRet;
}

push(@EXPORT,'szLeft');
# BASIC left$
sub szLeft {
  my ( $szString, $intLength ) = @_;
  return substr( $szString, 0, $intLength);
}

push(@EXPORT,'szRight');
# BASIC right$
sub szRight {
  my ( $szString, $intLength ) = @_;
  if ($intLength<1) { 
    return ''; 
  } else {
    return substr( $szString, -1 * ($intLength) );
  }
}

push(@EXPORT,'boolMatchRegEx');
# javascript/PERL test/=~m
sub boolMatchRegEx {
  my ($szString,$regexRegExp)=@_;
  return eval('return($szString=~m'.$regexRegExp.');');
}

push(@EXPORT,'arrMatchRegEx');
# PERL ()=?=~m
sub arrMatchRegEx {
  my ($szString,$regexRegExp)=@_;
  my $arrRet=[];
  eval('@{$arrRet}=$szString=~m'.$regexRegExp.';');
  return $arrRet;
}

push(@EXPORT,'szReplaceRegEx');
# JavaScript/PERL replace/=~s
sub szReplaceRegEx {
  my ($szWhere,$regexWhat,$szHow,$opt_boolGlobal,$opt_boolIgnoreCase,$opt_boolMultiline)=@_;
  my $szRet=$szWhere;
  if (not defined($opt_boolGlobal)) { $opt_boolGlobal=0; };
  if (not defined($opt_boolIgnoreCase)) { $opt_boolIgnoreCase=0; };
  if (not defined($opt_boolMultiline)) { $opt_boolMultiline=0; };
  
  my $szModifier='';
  if ($opt_boolIgnoreCase) { $szModifier.='i'; }
  if ($opt_boolGlobal)     { $szModifier.='g'; }
  if ($opt_boolMultiline)  { $szModifier.='m'; }
  eval('$szRet=~s/'.$regexWhat.'/'.$szHow.'/'.$szModifier.';');
  return $szRet;
}

push(@EXPORT,'intLen');
# BASIC len
sub intLen {
  my ($szVar)=@_;
  return length($szVar);
}

push(@EXPORT,'boolIsEqual');
# PERL string equal
sub boolIsEqual {
  my ($sz1,$sz2)=@_;
  return $sz1 eq $sz2;
}

push(@EXPORT,'szReplaceMulti');
# replace multiple times
sub szReplaceMulti {
  my ($szWhere,$hashList,$opt_boolIgnoreCase)=@_;
  my $szRet=$szWhere;
  foreach my $szKey (keys(%{$hashList})) {
    $szRet=szReplace($szRet,$szKey,$hashList->{$szKey},$opt_boolIgnoreCase);
  }
  return $szRet;
}

push(@EXPORT,'szReplaceMultiArr');
# replace multiple times in order
sub szReplaceMultiArr {
  my ($szWhere,$arr_arrList,$opt_boolIgnoreCase)=@_;
  my $szRet=$szWhere;
  foreach my $arrKey (@{$arr_arrList}) {
    $szRet=szReplace($szRet,$arrKey->[0],$arrKey->[1],$opt_boolIgnoreCase);
  }
  return $szRet;
}

push(@EXPORT,'szReplace');
# BASIC replace
sub szReplace {
  my ($szWhere,$szWhat,$szNew,$opt_boolIgnoreCase)=@_;
  my $szRet=$szWhere;
  if (not defined($opt_boolIgnoreCase)) { $opt_boolIgnoreCase=boolFALSE(); };
  if (defined($szNew)) {
    if ($opt_boolIgnoreCase) {
      $szRet=~s/\Q$szWhat\E/$szNew/ig;
    } else {
      $szRet=~s/\Q$szWhat\E/$szNew/g;
    }
  }
  return $szRet;
}

push(@EXPORT,'intCharCode');
# PERL/BASIC ord/asc
sub intCharCode {
  my ($charVar)=@_;
  return ord($charVar);
}

push(@EXPORT,'charFromCode');
# JS/PERL/BASIC String.fromCharCode/chr/CHR
sub charFromCode {
  my ($intVar)=@_;
  return chr($intVar);
}

push(@EXPORT,'charCharAt');
# JS char at position
sub charCharAt {
  my ($szVar,$intIDX)=@_;
  return substr($szVar,$intIDX,1);
}

push(@EXPORT,'szLCase');
# BASIC LCASE$
sub szLCase {
  my ($szVar)=@_;
  return lc($szVar);
}

push(@EXPORT,'szUCase');
# BASIC UCASE$
sub szUCase {
  my ($szVar)=@_;
  return uc($szVar);
}

push(@EXPORT,'szTrim');
# BASIC TRIM$
sub szTrim {
  my ($szRet)=@_;
  $szRet=~s/^[\s]+|[\s]+$//g;
  return $szRet;
}

push(@EXPORT,'intInstr');
# PERL/BASIC index/instr
sub intInstr {
  my ( $opt_intStartPos, $szWhere, $szWhat, $opt_boolIgnoreCase ) = @_;
  my $intRet=-1;
  if ( defined($szWhat) ) {
    my $boolIgnoreCase = 0;
    if ( defined($opt_boolIgnoreCase) ) {
      $boolIgnoreCase = $opt_boolIgnoreCase;
    }
    if ($boolIgnoreCase) {
      $szWhere= lc($szWhere);
       $szWhat= lc($szWhat);
    }
    if (defined($opt_intStartPos)) {
      $intRet = index( $szWhere, $szWhat, $opt_intStartPos );
    } else {
      $intRet = index( $szWhere, $szWhat);
    }
  } else {
    $szWhat=$szWhere;
    $szWhere=$opt_intStartPos;
    $intRet = index( $szWhere,$szWhat );
  }
  return $intRet;
}

push(@EXPORT,'boolInstr');
# BASIC instr
sub boolInstr {
  my ($szWhere,$szWhat,$opt_boolIgnoreCase)=@_;
  my $boolRet= (intInstr(varUNDEF(),$szWhere,$szWhat,$opt_boolIgnoreCase)>-1);
  return $boolRet;
}

push(@EXPORT,'intRInstr');
# PERL/BASIC rindex/rinstr
sub intRInstr {
  my ( $opt_intStartPos, $szWhere, $szWhat, $opt_boolIgnoreCase ) = @_;
  my $intRet=-1;
  if ( defined($szWhat) ) {
    my $boolIgnoreCase = boolFALSE();
    if ( defined($opt_boolIgnoreCase) ) {
      $boolIgnoreCase = $opt_boolIgnoreCase;
    }
    if ($boolIgnoreCase) {
      $intRet = rindex( lc($szWhere), lc($szWhat), $opt_intStartPos );
    } else {
      $intRet = rindex( $szWhere, $szWhat, $opt_intStartPos );
    }
  } else {
    $intRet = rindex( $opt_intStartPos, $szWhere );
  }
  return $intRet;
}

push(@EXPORT,'boolInArr');
# PERL grep
sub boolInArr {
  my ($arrWhere,$szWhat)=@_;
  if (defined($szWhat)) {
    foreach (@{$arrWhere}) { if (defined($_) and ($szWhat eq $_)){return 1}};
  } else {
    foreach (@{$arrWhere}) { if (not defined($_)){return 1}};
  }
  return 0;
}

push(@EXPORT,'intInArr');
# PERL grep
sub intInArr {
  my ($arrWhere,$szWhat)=@_;
  my  $intI=0;
  if (defined($szWhat)) {
    foreach (@{$arrWhere}) { if (defined($_) and ($szWhat eq $_)){return $intI} else {$intI++}};
  } else {
    foreach (@{$arrWhere}) { if (not defined($_)){return $intI} else {$intI++}};
  }
  return -1;
}

push(@EXPORT,'szPrintf');
# PERL/C sprintf
sub szPrintf {
  my ($szVar,$arrVar)=@_;
  return sprintf($szVar,@{$arrVar});
}

push(@EXPORT,'arrSplit');
# perl split
sub arrSplit {
  my ($regexHow,$szString,$opt_intMaxFields)=@_;
  my $arrRet=[];
  if (defined($opt_intMaxFields)) {
    push(@{$arrRet},split(/$regexHow/,$szString,$opt_intMaxFields));
  } else {
    push(@{$arrRet},split(/$regexHow/,$szString));
  }
  return $arrRet;
}
# ************************************************************
# constructors
push(@EXPORT,'sz');
# string constructor
sub sz {
  return defined($_[0])?$_[0]:'';
}

push(@EXPORT,'arr');
# array constructor
sub arr {
  my @arr;
  push(@arr,@_);
  return \@arr;
}

push(@EXPORT,'hash');
# hash constructor
#sub hash {
#  my $hashRet={};
#  for (my $intI=0;$intI<scalar(@_);$intI+=2) {
#    $hashRet->{$_[$intI]}=$_[$intI+1];
#  }
#  return $hashRet;
#}
sub hash {
  my %hashRet;
  if (scalar(@_)>0) {
    %hashRet=@_;
  }
  return \%hashRet;
}


push(@EXPORT,'char');
# char constructor
sub char {
  my ($opt_charData)=@_;
  my $charRet='';
  if (defined($opt_charData)) {
    $charRet.=$opt_charData;
  }
  return $charRet;
}

# return caller info
sub _hashGetCallerInfo {
  my ($intI)=@_;
  my $hashRet=hash();
  package DB;
  if (my @arrData=caller($intI)) {
    my @arrArgs = (@DB::args);
    my $arrArgs = \@arrArgs;
    my $hashArgs={};
    if (defined($arrArgs)) {
      for (my $intI=0;$intI<scalar(@{$arrArgs});$intI++) {
        my $varArgument=$arrArgs->[$intI];
        if (ref($varArgument)) {
          $hashArgs->{sprintf('%s',$varArgument)}=$varArgument;
        }
      }
    } else {
      $arrArgs=[];
    }
    my $hashData={
      'intStackDepth'   =>$intI,
      'szPackage'       =>$arrData[0],
      'szFileName'      =>$arrData[1],
      'intLine'         =>$arrData[2],
      'szRoutineName'   =>$arrData[3],
      'boolHasArguments'=>$arrData[4],
      'boolWantsArray'  =>$arrData[5],
      'szEvalText'      =>$arrData[6],
      'boolIsRequire'   =>$arrData[7],
      'varReserved'     =>$arrData[8],
      'varReserved2'    =>$arrData[9],
      'arrArguments'    =>$arrArgs,
      'hashArgsByRef'   =>$hashArgs
    };
    foreach my $szKey (keys(%{$hashData})) {
      $hashRet->{$szKey}=$hashData->{$szKey};
    }
  }
  return($hashRet);
}

if ($0 eq __FILE__) {
  # Self Loader
} else {
  return (boolTRUE());
}