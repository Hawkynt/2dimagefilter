#
#===============================================================================
#
#         FILE:  modGlobal.pm
#
#  DESCRIPTION:  contains global functions
#
#        FILES:  ---
#         BUGS:  ---
#        NOTES:  ---
#       AUTHOR:  »SynthelicZ« Hawkynt
#      COMPANY:  »SynthelicZ«
#      VERSION:  1.000
#      CREATED:  21.03.2007 11:44:46 CET
#     REVISION:  ---
#===============================================================================
package modGlobal;

use strict;
use warnings;

use Encode;

use Modules::modIL;
use Modules::IL::Filter;
our $VERSION = 1.025;
# 1.025 20090602-0855 Hawkynt
#     + added "hashLCaseKeys" to make all keys lowercase
# 1.024 20090323-1324 Hawkynt
#     * changed "szFormatDate" to detect possible time given in date format
# 1.023 20090303-0841 Hawkynt
#     + added "szEscapeHTML" to escape html chars
# 1.022 20090220-0924 Hawkynt
#     + added "voidMoveFile" to move files
# 1.021 20090213-0639 Hawkynt
#     * changed "_intXDayDiff" to avoid deadlock when giving from dates (eg. 20090232)
# 1.020 20090212-1043 Hawkynt
#     + added "_boolIsXCY" to detect "schaltjahre" including excel 1900 bug
#     + added "_intXDayDiff" to get difference between two dates using excel bug
#     + added "_szXDateAdd" to add days to a date according to excel
#     + added global var "szDateBase" to change the datebase on which date serials are calculated
#     + added "szSerial2TimeCode" to convert date serials to a timecode
#     + added "floatTimeCode2Serial" to convert a tc to a date serial
# 1.010 20090119-1012 Hawkynt
#     * changed "arrGetLines" to accept string also
# 1.009 20081119-0954 Hawkynt
#     + added contant floatPI
# 1.008 20081105-0652 Hawkynt
#     * changed "szString2HEX" added parameter to set length of hex bytes
# 1.007 20081102-1820 Hawkynt
#     + added functions to disable errors and warnings (criticalsection)
# 1.006 20081028-0520 Hawkynt
#     * changed "voidCreateDir" can create recursive structures
#     - removed Exporter
#     + added "voidRenameFile" to rename files
# 1.005 20081015-0935 Hawkynt
#     * changed File::Copy, IO::Select, Data::Dumper, Carp, FindBin, Encode only included when used
# 1.004 20081010-1113 Hawkynt
#     * bugfix "voidDeprecated" routine crashed since il::filter
# 1.003 20080729-1208 Hawkynt
#     +added IL::Filter for better performance
# 1.002 20080626-1401 Hawkynt
#     +added routines for getting caller information including parameters
# 1.001 20071011-0911 Hawkynt 
#     +added routines for saving and loading struct to/from xml files
#     *requires Classes::classXMLNode
#     *requires Modules::Convert::modQuotetPrint

our $szLE        = "\n";
our $szCRLF      = "\x0D\x0A";
our $szHEXDigits = '0123456789ABCDEF';
our $kB=1024;
our $MB=$kB*1024;
our $GB=$MB*1024;
our $TB=$GB*1024;
our $EB=$TB*1024;
our $PB=$EB*1024;
our $floatPI=4*CORE::atan2(1,1);
our $szDateBase='19000101';
our $arrHTMLReplacer=arr(
  arr('&','&amp;'),
  arr('<','&lt;'),
  arr('>','&gt;'),
  arr('\'','&acute;'),
  arr('ä','&auml;'),
  arr('ö','&ouml;'),
  arr('ü','&uuml;'),
  arr('Ä','&Auml;'),
  arr('Ö','&Ouml;'),
  arr('Ü','&Uuml;'),
  arr('"','&quot;')
);

# makes all hash keys to lower case
sub hashLCaseKeys {
  my ($hashHash)=@_;
  my $hashRet=hash();
  foreach my $szKey (keys(%{$hashHash})) {
    $hashRet->{lc($szKey)}= $hashHash->{$szKey};
  }
  return($hashRet);
}

# replaces special chars to be displayed with html
sub szEscapeHTML {
  my ($szText)=@_;
  my $szRet= szReplaceMultiArr($szText, $arrHTMLReplacer, boolFALSE());
  return($szRet);
}

# return actual functions name
sub szCurFunction {
  my ($opt_intDepth)=@_;
  my $szRet;
  if (boolNot(boolIsDefined($opt_intDepth))) {
    $opt_intDepth=0;
  }
  require Carp;
  my $arrCallStack=arrSplit('[\\n]',Carp::longmess(''));
  #print szGetStructure($arrCallStack);
  if (intArrLen($arrCallStack)>1) {
    my $szLine=varArrayItem($arrCallStack,1-$opt_intDepth);
    if (boolIsDefined($szLine)) {
      my $arrItems=arrMatchRegEx($szLine,'/^(.*?)[\\s]called[\\s]at[\\s](.*?)$/');
      $szRet=szTrim(varArrayItem($arrItems,0));
      $szRet=szReplaceRegEx($szRet,'[\\(].*?$','');
      #$szRet.='['.szReplace(szTrim(varArrayItem($arrItems,1)),' line ',':').']';
    }
  } else {
    $szRet='Unknown';
  }
  return($szRet);
}

sub szCurFile {
  my ($opt_intDepth)=@_;
  my $szRet=sz();
  if (boolNot(boolIsDefined($opt_intDepth))) {
    $opt_intDepth=0;
  }
  my $arrCallStack=arrSplit('[\\n]',Carp::longmess(''));
  #print szGetStructure($arrCallStack);
  if (intArrLen($arrCallStack)>1) {
    my $szLine=varArrayItem($arrCallStack,1-$opt_intDepth);
    my $arrItems=arrMatchRegEx($szLine,'/^(.*?)[\\s]called[\\s]at[\\s](.*?)$/');
    $szRet=szTrim(varArrayItem($arrItems,1));
    $szRet=szReplaceRegEx($szRet,'[\\s]line.*?$','');
  } else {
    $szRet='Unknown';
  }
  return($szRet);
}

sub dwordCurLine {
  my ($opt_intDepth)=@_;
  my $dwordRet=0;
  if (boolNot(boolIsDefined($opt_intDepth))) {
    $opt_intDepth=0;
  }
  my $arrCallStack=arrSplit('[\\n]',Carp::longmess(''));
  #print szGetStructure($arrCallStack);
  if (intArrLen($arrCallStack)>1) {
    my $szLine=varArrayItem($arrCallStack,1-$opt_intDepth);
    my $arrItems=arrMatchRegEx($szLine,'/^(.*?)[\\s]called[\\s]at[\\s](.*?)$/');
    $dwordRet=szReplaceRegEx(szTrim(varArrayItem($arrItems,1)),'^.*?line[\\s]','');
  }return($dwordRet);
}

our $arrCriticalStack=arr();

sub voidEnterCriticalSection {
  voidPush($arrCriticalStack,arr($SIG{'__WARN__'},$SIG{'__DIE__'}));
  $SIG{'__WARN__'}=sub {};
  $SIG{'__DIE__'}=sub {};
}

sub voidLeaveCriticalSection {
  my $intCriticalStack=intArrLen($arrCriticalStack);
  if ($intCriticalStack>0) {
    my $arrCurSection=varArrayItem($arrCriticalStack,$intCriticalStack-1);
    my $arrTmpStack=arr();
    for (my $intI=0;$intI<($intCriticalStack-1);$intI++) {
      voidPush($arrTmpStack,varArrayItem($arrCriticalStack,$intI));
    }
    $arrCriticalStack=varUNDEF();
    $arrCriticalStack=$arrTmpStack;
    $SIG{'__WARN__'}=varArrayItem($arrCurSection,0);
    $SIG{'__DIE__'}=varArrayItem($arrCurSection,1);
  } else {
    voidWarn('could not leave critical section, because there is none');
  }
  voidPush($arrCriticalStack,arr($SIG{'__WARN__'},$SIG{'__DIE__'}));
}

# show deprecated function warning
sub voidDeprecated {
  my ($opt_szUser,$opt_szText)=@_;
  my $szText=sz('');
  my $arrCallStack;
  {
    my $boolV=$Carp::Verbose;
    $Carp::Verbose=boolTRUE();
    $arrCallStack=arrSplit('[\n]',Carp::longmess(''));
    $Carp::Verbose=$boolV;
  };
  #print szGetStructure($arrCallStack);
  if (boolNot(boolIsDefined($opt_szUser))) {
    $opt_szUser='Someone';
  }
  if (boolNot(boolIsDefined($opt_szText)) or boolIsEqual(szTrim($opt_szText),'')) {
    $opt_szText='';
  } else {
    $opt_szText=szCombineStrings($opt_szText,$szLE);
  }
  for (my $intI=0;$intI<intArrLen($arrCallStack);$intI++) {
    voidArrayItem($arrCallStack,$intI,szTrim(varArrayItem($arrCallStack,$intI)));
  }
  if (intArrLen($arrCallStack)>1) {
    my $szLine=varArrayItem($arrCallStack,1);
    my $arrItems=arrMatchRegEx($szLine,'/^(.*?)[\\s]called[\\s]at[\\s](.*?)$/');
    my $arrStack=arr();
    for (my $intI=1;$intI<intArrLen($arrCallStack);$intI++) {
      voidPush($arrStack,varArrayItem($arrCallStack,$intI));
    }
    my $szText=szJoin($szLE,arr('%s noted:','call to %s is deprecated !','%s','Call Stack:','%s',''));
    $szText=szPrintf(
      $szText,
      arr(
        $opt_szUser,
        varArrayItem($arrItems,0),
        $opt_szText,
        szReplace(szJoin($szLE,$arrStack),'%','%%')
      )
    );
    voidWarn($szText,'found deprecated code');
  }
}

# creates directory
sub voidCreateDir {
  my ($szDir,$opt_intMask,$opt_boolRecursive,$opt_boolVerbose)=@_;
  if (boolNot(boolIsDefined($opt_intMask))) {
    $opt_intMask=0777;
  }
  if (boolNot(boolIsDefined($opt_boolRecursive))) {
    $opt_boolRecursive=boolFALSE();
  }
  if (boolNot(boolIsDefined($opt_boolVerbose))) {
    $opt_boolVerbose=boolFALSE();
  }
  my $intTries=128;
  while (($intTries>0) and boolNot(boolDirectoryExists($szDir))) {
    if ($opt_boolRecursive) {
      require File::Path; 
      File::Path::mkpath( Encode::encode('iso-8859-1',$szDir), $opt_boolVerbose, $opt_intMask );
    } else {
      mkdir Encode::encode('iso-8859-1',$szDir),$opt_intMask;
    }
    $intTries--;
  }
}

# removes a directory
sub voidDeleteDir {
  my ($szDir,$opt_boolRecursive,$opt_boolVerbose)=@_;
  if (boolNot(boolIsDefined($opt_boolRecursive))) {
    $opt_boolRecursive=boolFALSE();
  }
  if (boolNot(boolIsDefined($opt_boolVerbose))) {
    $opt_boolVerbose=boolFALSE();
  }
  if ($opt_boolRecursive) {
    require File::Path;
    File::Path::rmtree(Encode::encode('iso-8859-1',$szDir),$opt_boolVerbose);
  } else {
    rmdir $szDir;
  }
}

# moves a directory
sub voidMoveDir {
  my ($szSrcDir,$szTgtDir)=@_;
  if (boolIsDefined($szSrcDir) and boolIsDefined($szTgtDir)) {
    if (boolDirectoryExists($szSrcDir) and boolDirectoryExists($szTgtDir)) {
      my $arrItems=arrReadDir($szSrcDir);
      $szSrcDir=szCombineStrings($szSrcDir,'/');
      $szTgtDir=szCombineStrings($szTgtDir,'/');
      my $intItems=intArrLen($arrItems);
      # move files first
      for (my $intI=0;$intI<$intItems;$intI++) {
        my $szSrcItem=szCombineStrings($szSrcDir,varArrayItem($arrItems,$intI));
        my $szTgtItem=szCombineStrings($szTgtDir,varArrayItem($arrItems,$intI));
        if (boolNot(boolIsDirectory($szSrcItem))) {
          voidMoveFile($szSrcItem,$szTgtItem);
        }
      }
      # then move folders
      for (my $intI=0;$intI<$intItems;$intI++) {
        my $szSrcItem=szCombineStrings($szSrcDir,varArrayItem($arrItems,$intI));
        my $szTgtItem=szCombineStrings($szTgtDir,varArrayItem($arrItems,$intI));
        if (boolIsDirectory($szSrcItem)) {
          voidCreateDir($szTgtItem);
          voidMoveDir($szSrcItem,$szTgtItem);
          voidDeleteDir($szSrcItem);
        }
      }
    }
  }
}

# removes a file
sub voidDeleteFile {
  my ($szFile)=@_;
  unlink Encode::encode('iso-8859-1',$szFile);
}

# copies a file
sub voidCopyFile {
  my ($szSource,$szTarget)=@_;
  #print "from $szSource to $szTarget \n";
  require File::Copy;
  File::Copy::copy(Encode::encode('iso-8859-1',$szSource),Encode::encode('iso-8859-1',$szTarget));
  #File::Copy::copy($szSource,$szTarget);
  #my $szData=szReadFileBinary($szSource);
  #if (boolIsDefined($szData) and boolNot(boolIsEqual($szData, ''))) {
  #  voidWriteFileBinary($szTarget,$szData);
  #}
}

# moves a file
sub voidMoveFile {
  my ($szSource,$szTarget)=@_;
  #print "from $szSource to $szTarget \n";
  require File::Copy;
  File::Copy::move(Encode::encode('iso-8859-1',$szSource),Encode::encode('iso-8859-1',$szTarget));
}

# renames a file
sub voidRenameFile {
  my ($szSource,$szTarget)=@_;
  rename(Encode::encode('iso-8859-1',$szSource),Encode::encode('iso-8859-1',$szTarget));
}


# return file array
sub arrGetFiles {
  my ($szPath)=@_;
  my $arrRet=arr();
  my $arrItems=arrReadDir($szPath);
  for (my $intI=0;$intI<intArrLen($arrItems);$intI++) {
    my $szItem=varArrayItem($arrItems,$intI);
    if (boolFileExists(szJoin('/',arr($szPath,$szItem)))) {
      voidPush($arrRet,$szItem);
    }
  }
  return ($arrRet);
}

# return subfolders
sub arrGetDirectories {
  my ($szPath)=@_;
  my $arrRet=arr();
  my $arrItems=arrReadDir($szPath);
  my $intItems=intArrLen($arrItems);
  for (my $intI=0;$intI<$intItems;$intI++) {
    my $szItem=varArrayItem($arrItems,$intI);
    if (boolDirectoryExists(szJoin('/',arr($szPath,$szItem)))) {
      voidPush($arrRet,$szItem);
    }
  }
  return($arrRet);
}

# returns reference to an array or hash
sub ptrReference {
  my (@arrTemp) = @_;
  # deprecated code
  modGlobal::voidDeprecated('10.09.2007-07:35:56 de5129','');
  my $ptrRet = \@arrTemp;
  return ($ptrRet);
}

#===  FUNCTION  ================================================================
#         NAME:  GetLines
#  DESCRIPTION:  gets lines with CRLF CR LF LFCR
#   PARAMETERS:  $arrLines or $szLines
#      RETURNS:  arr
#===============================================================================
sub arrGetLines {
  my ($varLines) = @_;
  my $arrRet=arr();
  if (boolNot(boolIsEqual(szRef($varLines),'ARRAY'))) {
    $varLines=arr($varLines);
  } else {
    # got array already
  }
  my $szLines = szJoin('', $varLines);
  @{$arrRet }= split( /[\r][\n]/, $szLines );
  $szLines = join( "\n\r", @{$arrRet} );

  @{$arrRet }= split( /[\n][\r]/, $szLines );
  $szLines = join( "\n", @{$arrRet} );

  @{$arrRet }= split( /[\n]/, $szLines );
  $szLines = join( "\r", @{$arrRet} );

  @{$arrRet }=split( /[\r]/, $szLines );

  return ($arrRet);
}    #=======================[ END OF FUNCTION GetLines ]=======================

#===  FUNCTION  ================================================================
#         NAME:  LookupPath
#  DESCRIPTION:  searches for a file or folder in @INC and returns full path or undef
#   PARAMETERS:  $szFileOrFolder
#      RETURNS:  sz
#===============================================================================
sub szLookupPath {
  my ($szFileOrFolder) = @_;
  my $szRet;
  my $boolFound = boolFALSE();
  require FindBin;
  no warnings;
    my $arrPaths  = arr('.',$FindBin::Bin);
  use warnings;
  push( @{$arrPaths}, @INC );
  #print szGetStructure($arrPaths);
  my $intI    = 0;
  my $intMaxI = scalar( @{$arrPaths} );
  while ( ( $intI < $intMaxI ) and not($boolFound) ) {
    my $szCurDir = $arrPaths->[$intI];
    $szCurDir =~ s/[\\\/]+$//;
    my $szCheckFile = $szCurDir . '/' . $szFileOrFolder;
    #print 'Pruefe : ' . $szCheckFile . "\n";
    if ( boolExists($szCheckFile) ) {
      $szRet     = $szCheckFile;
      $boolFound = boolTRUE();
    }
    else {
      $intI++;
    }
  }
  return ($szRet);
}    #=======================[ END OF FUNCTION LookupPath ]========================

# checks file or folder existence
sub boolExists {
  my ($szFileFolder) = @_;
  my $boolRet = boolFALSE();
  if ( -e Encode::encode('iso-8859-1',$szFileFolder) ) {
    $boolRet = boolTRUE();
  }
  return ($boolRet);
}

# test if a directory exists
sub boolIsDirectory {
  my ($szFolder) = @_;
  my $boolRet = boolDirectoryExists($szFolder);
  return ($boolRet);
}

# test if a directory exists
sub boolDirectoryExists {
  my ($szFolder) = @_;
  my $boolRet = boolFALSE();
  if (boolIsDefined($szFolder) and  boolExists($szFolder) and (-d Encode::encode('iso-8859-1',$szFolder))) {
    $boolRet = boolTRUE();
  }
  return ($boolRet);
}

# check whether a file exists
sub boolFileExists {
  my ($szFile) = @_;
  my $boolRet = boolFALSE();
  if (boolIsDefined($szFile) and boolExists($szFile) and (-f Encode::encode('iso-8859-1',$szFile))) {
    $boolRet = boolTRUE();
  }
  return ($boolRet);
}

# converts seconds into hours, minutes and seconds
sub szParseSec {
  my ($intSecs) = @_;
  my $szRet = sz('');
  my $intHours=intInt( $intSecs / 3600 );
  $intSecs = $intSecs % 3600;
  my $intMinutes=intInt($intSecs / 60);
  $intSecs = $intSecs % 60;
  $szRet=szPrintf('%d:%02d:%02d',arr($intHours,$intMinutes,$intSecs));
  return ($szRet);
}

# returns a time code that allows ascii sorting, used for log files mostly
sub szCurTimeCode {
  my $szRet=sz();
  my $arrTime=arrSplit('[\:]',szTime());
  $szRet = szPrintf( '%s-%02d%02d%02d', arr(szCurDateCode(),varArrayItem($arrTime,0),varArrayItem($arrTime,1),varArrayItem($arrTime,2)));
  return ($szRet);
}

#format date
sub szFormatDate {
  my ($szFormat,$opt_szDate)=@_;
  my $szRet=sz();
  if (boolNot(boolIsDefined($opt_szDate))) {
    $opt_szDate=szDate();
  }
  my $arrComp;
  if (boolInstr($opt_szDate, ' ', )) {
    $opt_szDate= szLeft($opt_szDate, intInstr($opt_szDate, ' ')-1);
  }
  if (boolInstr($opt_szDate,'/')) {
    $arrComp=arrSplit('/',$opt_szDate);
    my $intT=varArrayItem($arrComp,0);
    voidArrayItem($arrComp,0,varArrayItem($arrComp,1));
    voidArrayItem($arrComp,1,$intT);
  } else {
    $arrComp=arrSplit('[\\.]',$opt_szDate);
  }
  if (boolIsEqual($szFormat,'YYMMDD')) {
    $szRet=szPrintf('%02d%02d%02d',arr(szRight(varArrayItem($arrComp,2),2),varArrayItem($arrComp,1),varArrayItem($arrComp,0)));
  } elsif (boolIsEqual($szFormat,'YYYY-MM-DD')) {
    $szRet=szPrintf('%04d-%02d-%02d',arr(varArrayItem($arrComp,2),varArrayItem($arrComp,1),varArrayItem($arrComp,0)));
  } elsif (boolIsEqual($szFormat,'YYYYMMDD')) {
    $szRet=szPrintf('%04d%02d%02d',arr(varArrayItem($arrComp,2),varArrayItem($arrComp,1),varArrayItem($arrComp,0)));
  } 
  return($szRet);
}

# returns a date code that allows ascii sorting, used for log files mostly
sub szCurDateCode {
  my $szRet=sz();
  my $arrDate=arrSplit('[\.]',szDate());
  $szRet = szPrintf( '%04d%02d%02d',arr(varArrayItem($arrDate,2),varArrayItem($arrDate,1),varArrayItem($arrDate,0)));
  return ($szRet);
}

#===  FUNCTION  ================================================================
#         NAME:  Date
#  DESCRIPTION:  returns the current date in Format dd.mm.yyyy
#   PARAMETERS:
#      RETURNS:  sz
#===============================================================================
sub szDate {
  my $szRet;
  my ( $intSec, $intMin, $intHour, $intMDay, $intMon, $intYear ) = localtime(time);
  $szRet = szPrintf( '%02d.%02d.%4d', arr($intMDay, $intMon + 1, $intYear + 1900 ));
  return ($szRet);
}    #=======================[ END OF FUNCTION Date ]==============================

#===  FUNCTION  ================================================================
#         NAME:  Time
#  DESCRIPTION:  returns the current time in Format hh:mm:ss
#   PARAMETERS:
#      RETURNS:  sz
#===============================================================================
sub szTime {
  my $szRet;
  my ( $intSec, $intMin, $intHour, $intMDay, $intMon, $intYear ) = localtime(time);
  $szRet = szPrintf( '%02d:%02d:%02d', arr($intHour, $intMin, $intSec ));
  return ($szRet);
}    #=======================[ END OF FUNCTION Time ]=============================

# copies directory trees
sub boolXCopy {
  my ( $szSourceDir, $szDestDir, $boolDebug ) = @_;
  require File::Copy;
  my $boolRet = boolTRUE();
  if ( boolNot(boolIsDefined($boolDebug))) {
    $boolDebug = boolFALSE();
  }
  my $arrFiles = arr();
  if ($boolDebug) {
    print 'Recursive Copy ' . $szSourceDir . ' to ' . $szDestDir . "\n";
  }
  if ( opendir( handleInDir, $szSourceDir ) ) {
    @{$arrFiles} = readdir(handleInDir);
    if ( not( closedir(handleInDir) ) ) {
      warn $0 . '> failed to close  input directory ' . $szSourceDir . ' : ' . $! . "\n";
    }
    if ( mkdir( $szDestDir, 0777 ) ) {
      if ($boolDebug) {
        print $0. '> copy ' . intArrLen( $arrFiles ) . ' items to output directory ' . $szDestDir . "\n";
      }
      foreach my $szItem ( @{$arrFiles} ) {
        if ( not( $szItem =~ m/^[\.][\.]?$/ ) ) {
          if ( -f $szSourceDir . '/' . $szItem ) {
            if ($boolDebug) {
              print 'File Copy ' . $szSourceDir . '/' . $szItem . ' to ' . $szDestDir . '/' . $szItem . "\n";
            }
            File::Copy::copy( $szSourceDir . '/' . $szItem, $szDestDir . '/' . $szItem );
          }
          elsif ( -d $szItem ) {
            &boolXCopy( $szSourceDir . '/' . $szItem, $szDestDir . '/' . $szItem );
          }
        }
      }
    }
    else {
      $boolRet = boolFALSE();
      if ($boolDebug) {
        print $0. '> failed to make output directory ' . $szDestDir . ' : ' . $! . "\n";
      }
    }
  }
  else {
    $boolRet = boolFALSE();
    if ($boolDebug) {
      print $0. '> failed to open  input directory ' . $szSourceDir . ' : ' . $! . "\n";
    }
  }
  return ($boolRet);
}

# gets a structure as perl string
sub szGetStructure {
  my ( $varStruct, $opt_szVarName) = @_;
  my $szRet = sz('');
  if (boolNot(boolIsDefined($opt_szVarName))) {
    $opt_szVarName='varStruct';
  }
  require Data::Dumper;
  my $objDumper=new Data::Dumper(arr($varStruct),arr($opt_szVarName));
  $objDumper->Indent(boolTRUE());
  $objDumper->Useqq(boolTRUE());
  $objDumper->Sortkeys(boolTRUE());
  $szRet=szCombineStrings($szRet,$objDumper->Dump());
  return ($szRet);
}

# converts FF to 255
sub intHEX2Byte {
  my ($szHEX) = @_;
  $szHEX = szLeft( $szHEX, 2 );
  my $intRet = intInstr( $szHEXDigits, szLeft( $szHEX, 1 ) ) * 16 + intInstr( $szHEXDigits, szRight( $szHEX, 1 ) );
  return ($intRet);
}

# converts FFFF to 65536
sub intHEX2Word {
  my ($szHEX) = @_;
  $szHEX = szLeft( $szHEX, 4 );
  my $intRet = intHEX2Byte( szLeft( $szHEX, 2 ) ) + ( 16**2 ) * intHEX2Byte( szRight( $szHEX, 2 ) );
  return ($intRet);
}

# converts FFFF0000 to 65536
sub intHEX2DWord {
  my ($szHEX) = @_;
  $szHEX = szLeft( $szHEX, 8 );
  my $intRet = intHEX2Byte( szLeft( $szHEX, 4 ) ) + ( 16**4 ) * intHEX2Byte( szRight( $szHEX, 4 ) );
  return ($intRet);
}

#===  FUNCTION  ================================================================
#         NAME:  ConvertUnit
#  DESCRIPTION:  convert a given unit to an other unit
#   PARAMETERS:  $szMode, $floatValue
#      RETURNS:  float
#===============================================================================
sub floatConvertUnit {
  my ($szMode, $floatValue)=@_;
  my $floatRet=0;
  my $floatINCH2CM=2.54;
  my $hashFactor = hash(
    'inch2mil',1000,
    'inch2cm',$floatINCH2CM,
    'inch2dm',$floatINCH2CM/10,
    'inch2m',$floatINCH2CM/100,
		'inch2mm',$floatINCH2CM*10,
		'inch2µm',$floatINCH2CM*10000,
		
		'mil2inch',1/1000,
		'mil2mm',$floatINCH2CM/100,
		'mil2µm',$floatINCH2CM*10,
		
		'mm2inch',1/$floatINCH2CM/10,
		'mm2mil',100/$floatINCH2CM,
		'mm2µm',1000,
		
		'µm2inch',1/$floatINCH2CM/10000,
		'µm2mil',1/$floatINCH2CM/10,
		'µm2mm',1/1000
	 );
		 
	if (boolNot(boolIsDefined(varHashItem($hashFactor,$szMode)))) {
	  voidError(szPrintf('conversion mode "%s" is not supported !',arr($szMode)));
	}
	$floatRet = ($floatValue * varHashItem($hashFactor,$szMode));
  return ($floatRet);
} #=======================[ END OF FUNCTION ConvertUnit ]=============================

# returns homedir of current user
sub szHomeDir {
  my ($szOS) = @_;
  my $szRet;
  if ( boolNot(boolIsDefined($szOS) ) ) {
    $szOS = $^O;
  }
  if ( $szOS =~ m/win/i ) {
    $szRet = $ENV{'HOMEDRIVE'} . $ENV{'HOMEPATH'};
  }
  else {
    $szRet = $ENV{'HOME'};
  }
  return ($szRet);
}

# write ini files
sub voidWriteINI {
  my ($szFileName,$hashConfig)=@_;
  my $arrLines=arr();
  # write values
  my $arrKeys=arrSort(arrGetKeys($hashConfig));
  for (my $intI=0;$intI<intArrLen($arrKeys);$intI++) {
    my $szName=varArrayItem($arrKeys,$intI);
    my $varValue=varHashItem($hashConfig,$szName);
    if (boolNot(boolIsEqual('HASH',szRef($varValue)))) {
      # Escape \\ and \"
      $szName=szReplace($szName,'\\','\\\\');
      $szName=szReplace($szName,'"','\\"');
      $varValue=szReplace($varValue,'\\','\\\\');
      $varValue=szReplace($varValue,'"','\\"');
      if (boolMatchRegEx($szName,'/[^A-Za-z0-9\\-_]/')) {
        $szName=szPrintf('"%s"',arr($szName));
      }
      voidPush($arrLines,szPrintf('%s = "%s"',arr($szName,$varValue)));
    }
  }
  # write sections
  for (my $intI=0;$intI<intArrLen($arrKeys);$intI++) {
    my $szName=varArrayItem($arrKeys,$intI);
    my $varValue=varHashItem($hashConfig,$szName);
    if (boolIsEqual('HASH',szRef($varValue))) {
      # Write Hashes
      $szName=szReplace($szName,'\\','\\\\');
      $szName=szReplace($szName,'"','\\"');
      voidPush($arrLines,'');
      voidPush($arrLines,szPrintf('[%s]',arr($szName)));
      my $arrKeys2=arrSort(arrGetKeys($varValue));
      for (my $intJ=0;$intJ<intArrLen($arrKeys2);$intJ++) {
        my $szName2=varArrayItem($arrKeys2,$intJ);
        my $szValue=varHashItem($varValue,$szName2);
        # Escape \\ and \"
        $szName2=szReplace($szName2,'\\','\\\\');
        $szName2=szReplace($szName2,'"','\\"');
        $szValue=szReplace($szValue,'\\','\\\\');
        $szValue=szReplace($szValue,'"','\\"');
        if (boolMatchRegEx($szName2,'[^A-Za-z0-9\-_]')) {
          $szName2=szPrintf('"%s"',arr($szName2));
        }
        voidPush($arrLines,szPrintf('%s = "%s"',arr($szName2,$szValue)));
      }
    }
  }
  voidWriteFile($szFileName,$arrLines);
}

# reads ini files
sub hashReadINI {
  my ($varFile) = @_;
  my $hashRet = hash();
  my $arrLines = arr();
  
  if (boolIsEqual(szRef($varFile), 'ARRAY')) {
    voidCombineArrays($arrLines,$varFile);
  } else {
    voidCombineArrays($arrLines,arrReadFile($varFile));
  }
   
  my $szSection;
  foreach my $szLine ( @{$arrLines} ) {
    $szLine =~ s/^[\s]+|[\s]+$//g;
    if ( ( not substr( $szLine, 0, 1 ) eq '#' ) and (not substr( $szLine, 0, 1 ) eq ';')){
      if ( substr( $szLine, 0, 1 ) eq '[' ) {

        # section found
        $szLine =~ s/^[\[]|[\]]$//g;
        $szSection = $szLine;
        # unescape strings
        $szSection =~ s/\\\\/\\/g;
        $szSection =~ s/\\"/"/g;
        if ( not exists( $hashRet->{$szSection} ) ) {
          $hashRet->{$szSection} = {};
        }
      }
      elsif ( index( $szLine, '=' ) > -1 ) {

        # name, value pair found
        my ( $szName, $szValue ) = $szLine =~ m/^([^=]*)=(.*)$/;
        # trim hyphens
        $szName  =~ s/^[\s]+|[\s]+$//g;
        $szName  =~ s/^["]+|["]+$//g;
        $szName  =~ s/^[']+|[']+$//g;
        $szValue =~ s/^[\s]+|[\s]+$//g;
        $szValue =~ s/^["]+|["]+$//g;
        $szValue =~ s/^[']+|[']+$//g;
          
        # unescape strings
        $szName =~ s/\\\\/\\/g;
        $szName =~ s/\\"/"/g;
        $szValue =~ s/\\\\/\\/g;
        $szValue =~ s/\\"/"/g;
        if ( not defined($szSection) ) {
          $hashRet->{$szName} = $szValue;
        }
        else {
          $hashRet->{$szSection}->{$szName} = $szValue;
        }
      }
    }
  }

  return ($hashRet);
}

# logfile open
sub hashLogOpen {
  my ($szLogFileName,$opt_szCaller)=@_;
  my $hashRet=hash();
  if (boolNot(boolIsDefined($opt_szCaller))) {
    ($opt_szCaller)=$0=~m/([^\\\/]*)$/;
  }
  require Symbol;
  voidCombineHashes($hashRet,hash(
    'szFileName',$szLogFileName,
    'handleOutLog',Symbol::gensym(),
    'szCaller',$opt_szCaller,
    'boolLogEnabled',boolFALSE()
  ));
  if ( not( open( $hashRet->{'handleOutLog'}, '>>' . $szLogFileName ) ) ) {
    die $0 . '> failed to open output log file "' . $szLogFileName . '" : ' . $! . "\n";
  } else {
    voidHashItem($hashRet,'boolLogEnabled',boolTRUE());
    voidLogWriteLine($hashRet,'-' x  80);
    voidLogWriteLine($hashRet,'Logging started');
  }
  return ($hashRet);
}

# logfile close
sub voidLogClose {
  my ($hashLog)=@_;
  voidLogWriteLine($hashLog,'Logging ended');
  if ( not( close($hashLog->{'handleOutLog'}) ) ) {
    warn $0 . '> failed to close output log file ' . $hashLog->{'szFileName'}. ' : ' . $! . "\n";
  }
  chmod(0777,$hashLog->{'szFileName'});
}

# write a line into logfile
sub voidLogWriteLine {
  my ($hashLog ,$szLine) = @_;
  #print $szLE. '> ' . $szLine;
  if ($hashLog->{'boolLogEnabled'}) {
    my $handleOutLog=$hashLog->{'handleOutLog'};
    print $handleOutLog $szLE . szCurTimeCode() . ' ' . $hashLog->{'szCaller'} . '> ' . $szLine;
  }
}

# write a warning to logfile
sub voidLogWarn {
  my ($hashLog ,$szLine) = @_;
  &voidLogWriteLine( $hashLog,'WARNING: ' . $szLine );
}

# write an error into logfile
sub voidLogError {
  my ($hashLog,$szLine) = @_;
  &voidLogWriteLine( $hashLog,'ERROR: ' . $szLine );
}

# write simple textstring into logfile
sub voidLogWrite {
  my ($hashLog ,$szText) = @_;
  #print $szText;
  if ($hashLog->{'boolLogEnabled'}) {
    my $handleOutLog=$hashLog->{'handleOutLog'};
    print $handleOutLog $szText;
  }
}

# unflatens a 1D-Array to a 2D-Array
sub arrUnflatArray {
	my ($arrInput,$intSubArraySize)=@_;
	my $arrRet=arr();
	for (my $intI=0;$intI<scalar(@{$arrInput});$intI+=$intSubArraySize) {
	  my $arrSubArray=arr();
	  for (my $intJ=0;$intJ<$intSubArraySize;$intJ++ ) {
	    push(@{$arrSubArray},$arrInput->[$intI+$intJ]);
	  }
	  push(@{$arrRet},$arrSubArray);
	}
	return($arrRet);
}

# writes an array to file
sub voidWriteFile {
  my ($szFileName,$arrLines)=@_;
  if (open ( handleOutFile, '>'.Encode::encode('iso-8859-1',$szFileName) )) {
  	print handleOutFile join($szLE,@{$arrLines});
    if (not (close ( handleOutFile ))) {
  	  warn $0.'> failed to close output file '.$szFileName.' : '.$!."\n";
  	}
  } else {
    warn  $0.'> failed to open output file '.$szFileName.' : '.$!."\n";
  }
}

sub voidWriteFileBinary {
  my ($szFile,$szData)=@_;
  #print 'Speichere :'.Encode::encode('iso-8859-1',$szFile).$szLE;
  if (open ( handleOutFile, '>'.Encode::encode('iso-8859-1',$szFile) )) {
    binmode (handleOutFile);
    print handleOutFile $szData;
    if (not (close ( handleOutFile ))) {
      warn $0.'> failed to close output file '.$szFile.' : '.$!."\n";
    }
    chmod (0777,Encode::encode('iso-8859-1',$szFile));
  } else {
    warn  $0.'> failed to open output file '.$szFile.' : '.$!."\n";
  }
}

sub szReadFileBinary {
  my ($szFile)=@_;
  my $szRet=sz('');
  if (open ( handleInFile, '<'.Encode::encode('iso-8859-1',$szFile) )) {
    binmode(handleInFile);
    read(handleInFile,$szRet,hugeFileSize($szFile));
    if (not (close ( handleInFile ))) {
      warn $0.'> failed to close  input file '.$szFile.' : '.$!."\n";
    }
  } else {
    warn  $0.'> failed to open  input file '.$szFile.' : '.$!."\n";
  }
  return($szRet);
}

# get the files checksum
sub szFileChecksum {
  my ($szFile,$opt_szMethod)=@_;
  my $szRet;
  if (boolNot(boolIsDefined($opt_szMethod))) {
    $opt_szMethod='MD5:HEX+Size:HEX16+MD4:HEX';
  }
  if (boolFileExists($szFile)) {
    my $arrMethods=arrSplit('[\\+]',$opt_szMethod);
    require IO::File; 
    for (my$intI=0;$intI<intArrLen($arrMethods);$intI++) {
      my $szMethod=varArrayItem($arrMethods,$intI);
      my $szPart='';
      if (boolIsEqual($szMethod,'MD5')) {
        require Digest::MD5;
        $szPart=Digest::MD5->new->addfile(IO::File->new($szFile,'r'))->digest();
      } elsif (boolIsEqual($szMethod,'MD5:HEX')) {
        require Digest::MD5;
        $szPart=uc(Digest::MD5->new->addfile(IO::File->new($szFile,'r'))->hexdigest());
      } elsif (boolIsEqual($szMethod,'MD5:BASE64')) {
        require Digest::MD5;
        $szPart=Digest::MD5->new->addfile(IO::File->new($szFile,'r'))->b64digest();
      } elsif (boolIsEqual($szMethod,'MD4')) {
        require Digest::MD4;
        $szPart=Digest::MD4->new->addfile(IO::File->new($szFile,'r'))->digest();
      } elsif (boolIsEqual($szMethod,'MD4:HEX')) {
        require Digest::MD4;
        $szPart=uc(Digest::MD4->new->addfile(IO::File->new($szFile,'r'))->hexdigest());
      } elsif (boolIsEqual($szMethod,'MD4:BASE64')) {
        require Digest::MD4;
        $szPart=Digest::MD4->new->addfile(IO::File->new($szFile,'r'))->b64digest();
      } elsif (boolIsEqual($szMethod,'Size:HEX16')) {
        $szPart=szHEX(hugeFileSize($szFile),16);
      }
      voidArrayItem($arrMethods,$intI,$szPart);
    }
    $szRet=szJoin('',$arrMethods);
  }
  return($szRet);
}

sub szHEX {
  my ($hugeNum,$opt_byteMinLength)=@_;
  my $szRet;
  if (boolIsDefined($opt_byteMinLength)) {
    $szRet=sprintf('%0'.$opt_byteMinLength.'X',$hugeNum);
  } else {
    $szRet=sprintf('%X',$hugeNum);
  }
  return($szRet);
}

sub hugeFileSize {
  my ($szFile)=@_;
  my $hugeRet=(-s $szFile);
  return($hugeRet);
}

# reads a directory into an array
sub arrReadDir {
  my ($szDir,$opt_boolRecursive,$_opt_szPrefix)=@_;
  my $arrRet=arr();
  if (boolNot(boolIsDefined($opt_boolRecursive))) {
    $opt_boolRecursive= boolFALSE();
  }
  if (boolNot(boolIsDefined($_opt_szPrefix))) {
    $_opt_szPrefix='';
  }
  require Symbol;
  my $handleDir=Symbol::gensym();
  if (opendir ( $handleDir, Encode::encode('iso-8859-1',$szDir) )) {
    my $arrSubDirs=arr();
    foreach my $szItem (readdir($handleDir)) {
      if (boolNot(boolIsEqual($szItem,'.') or boolIsEqual($szItem,'..'))) {
        my $szFullItem=szCombineStrings($szDir,szCombineStrings('/',$szItem));
        voidPush($arrRet,szCombineStrings($_opt_szPrefix,$szItem));
        if (boolIsDirectory($szFullItem)) {
          voidPush($arrSubDirs,$szItem);
        }
      }  
    }
    if (not (closedir ( $handleDir ))) {
      voidWarn(szPrintf('failed to close input directory %s : %s',arr($szDir,$!)));
    }
    if ($opt_boolRecursive) {
      foreach my $szItem(@{$arrSubDirs}) {
        my $szFullItem=szCombineStrings($szDir,szCombineStrings('/',$szItem));
        voidCombineArrays($arrRet,arrReadDir($szFullItem,$opt_boolRecursive,szCombineStrings(szCombineStrings($_opt_szPrefix,$szItem),'/')));
      }
    }
  } else {
    voidWarn(szPrintf('failed to open input directory %s : %s',arr($szDir,$!)));
  }
  return($arrRet);
}

# reads a file into an array
sub arrReadFile {
  my ($szFile)=@_;
  my $arrRet=arr();
  if (boolFileExists($szFile)) {
    $szFile=Encode::encode('iso-8859-1',$szFile);
    if (open ( handleInFile, '<'.$szFile )) {
      my $arrLines=arr();
      @{$arrLines}=<handleInFile>;
      voidCombineArrays($arrRet,arrGetLines( $arrLines ));
      if (not (close ( handleInFile ))) {
        warn $0.'> failed to close  input file '.$szFile.' : '.$!."\n";
      }
    } else {
      warn $0.'> failed to open  input file '.$szFile.' : '.$!."\n";
    }
  } else {
    voidWarn(szPrintf('file "%s" does not exist',arr($szFile)));
  }
  return ($arrRet);
}

# reads genesis data file file into a hash
sub hashReadGDF {
  my ($varFile)=@_;
  my $hashRet={};
  my $szSection;
  my $hashSubRecord;
  my $arrLines=arr();
  if (ref($varFile) eq 'ARRAY') {
    push(@{$arrLines},@{$varFile});  
  } else {
    push(@{$arrLines},@{arrReadFile($varFile)});
  }
  foreach my $szLine (@{$arrLines}) {
    $szLine=~s/^[\s]+|[\s]+$|[\n]$|[\r]$//g;
    $szLine=~s/[\s]+$|[\n]$|[\r]$//g;
    if (substr($szLine,0,1) eq '#') {
      # skip comment line
    } elsif ($szLine=~m/[\{]$/) {
      # handles section open
      $szSection=substr($szLine,0,length($szLine)-1);
      $szSection=~s/[\s]+$//g;
      $hashSubRecord={};
    } elsif ($szLine=~m/[\}]$/) {
      # handles section close
      if (not exists($hashRet->{$szSection})) {
        $hashRet->{$szSection}=[];
      }
      push(@{$hashRet->{$szSection}},$hashSubRecord);
      $hashSubRecord=undef;
      $szSection=undef;
    } elsif (index($szLine,'=')>-1) {
      # handle attributes
      my ($szName,$szValue)=$szLine=~m/^(.*?)[\s]*=[\s]*(.*?)$/;
      if (defined($szSection)) {
        $hashSubRecord->{$szName}=$szValue;
      } else {
        $hashRet->{$szName}=$szValue;
      }
    }
  }
  return ($hashRet);
}

# serializes genesis data file data in form of a hash
sub arrWriteGDF {
  my ($hashData)=@_;
  my $arrRet=arr();
  # write sections first
  foreach my $szKey (sort(keys(%{$hashData}))) {
    if (ref($hashData->{$szKey}) eq 'ARRAY') {
      foreach my $hashRecord (@{$hashData->{$szKey}}) {
        push(@{$arrRet},sprintf('%s {',$szKey));
        foreach my $szName (sort(keys(%{$hashRecord}))) {
          push(@{$arrRet},sprintf('  %s = %s',$szName,$hashRecord->{$szName}));
        }
        push(@{$arrRet},'}');
        push(@{$arrRet},'');
      }
    }
  }
  # write non sections
  foreach my $szKey (sort(keys(%{$hashData}))) {
    if (not(ref($hashData->{$szKey}) eq 'ARRAY')) {
      push(@{$arrRet},sprintf('%s = %s',$szKey,$hashData->{$szKey}));
    }
  }
  
  return ($arrRet);
}

# writes a genesis data file
sub voidWriteGDF {
  my ($szFileName,$hashData)=@_;
  my $arrLines=arrWriteGDF($hashData);
  voidWriteFile($szFileName,$arrLines);
}

# serializes tbl data either in form of a hash or an array
sub arrWriteTBL {
  my ($varData,$hashConfig)=@_;
  my $arrRet=arr();
  my $hashHeader=hash(
    'version','1.0',
    'title',$0.' generated using '.__PACKAGE__,
    'primary',''
  );
  my $hashFirstRecord=hash();
  if (ref($varData) eq 'HASH') {
    my $arrKeys=arr();
    @{$arrKeys}=keys(%{$varData});
    %{$hashFirstRecord}=%{$varData->{$arrKeys->[0]}};
  } else {
    %{$hashFirstRecord}=%{$varData->[0]};
  }
  # lookup fields
  {
    my $arrFields=arr();
    foreach my $szKey (sort(keys(%{$hashFirstRecord}))) {
      push (@{$arrFields},[ucfirst($szKey),'var',undef]);
    }
  
    $hashHeader->{'header'}=$arrFields;
  };
  # get user specified header flags
  if (defined($hashConfig)) {
    foreach my $szKey (keys(%{$hashConfig})) {
      $hashHeader->{$szKey}=$hashConfig->{$szKey};
    }
  }
  # get field widths
  if (ref($varData) eq 'HASH') {
    for (my $intI=0;$intI<scalar(@{$hashHeader->{'header'}});$intI++) {
      my $intWidth=length(sprintf('%s(%s)',$hashHeader->{'header'}->[$intI]->[0],$hashHeader->{'header'}->[$intI]->[1]));
      foreach my $szKey (keys(%{$varData})) {
        my $hashRecord=$varData->{$szKey};
        my $intLen=length($hashRecord->{lc($hashHeader->{'header'}->[$intI]->[0])});
        if ($intLen>$intWidth) {
          $intWidth=$intLen;
        }
        $hashHeader->{'header'}->[$intI]->[2]=$intWidth;
      }  
    }
  } else {
    for (my $intI=0;$intI<scalar(@{$hashHeader->{'header'}});$intI++) {
      my $intWidth=length(sprintf('%s(%s)',$hashHeader->{'header'}->[$intI]->[0],$hashHeader->{'header'}->[$intI]->[1]));
      foreach my $hashRecord (@{$varData}) {
        my $intLen=length($hashRecord->{lc($hashHeader->{'header'}->[$intI]->[0])});
        if ($intLen>$intWidth) {
          $intWidth=$intLen;
        }
        $hashHeader->{'header'}->[$intI]->[2]=$intWidth;
      }  
    }
  }
  # push file-header to output
  push(@{$arrRet},'#TBL V'.$hashHeader->{'version'});
  push(@{$arrRet},'#TITLE: '.$hashHeader->{'title'});
  push(@{$arrRet},'#PRIMARY: '.$hashHeader->{'primary'});
  push(@{$arrRet},'#HEADER:');
  # build the table-header
  my $szLine='#';
  foreach my $arrField (@{$hashHeader->{'header'}}) {
    my $intWidth=$arrField->[2];
    $szLine.=sprintf('%-'.$intWidth.'s ',sprintf('%s(%s)',$arrField->[0],$arrField->[1]));
  }
  push(@{$arrRet},$szLine);
  # add 1 to first field because of the #-sign
  $hashHeader->{'header'}->[0]->[2]++;
  
  # push data to output
  if (ref($varData) eq 'HASH') {
    foreach my $szKey (sort(keys(%{$varData}))) {
      my $hashRecord=$varData->{$szKey};
      
      my $szLine='';
      foreach my $arrField (@{$hashHeader->{'header'}}) {
        my $szValue=$hashRecord->{lc($arrField->[0])};
        my $intWidth=$arrField->[2];
        $szLine.=sprintf('%-'.$intWidth.'s ',$szValue);
      }
      push(@{$arrRet},$szLine);
      
    }
  } else {
    foreach my $hashRecord (@{$varData}) {
      
      my $szLine='';
      foreach my $arrField (@{$hashHeader->{'header'}}) {
        my $szValue=$hashRecord->{lc($arrField->[0])};
        my $intWidth=$arrField->[2];
        $szLine.=sprintf('%-'.$intWidth.'s ',$szValue);
      }
      push(@{$arrRet},$szLine);
      
    }
  }
  return ($arrRet);
}

# writes tbl file
sub voidWriteTBL {
  my ($szFileName,$varData,$hashConfig)=@_;
  my $arrLines=arrWriteTBL($varData,$hashConfig);
  voidWriteFile($szFileName,$arrLines);
}

# should be used for reading tbl files into array of hashes
sub arrReadTBL {
  my ($varTableFile) = @_;
  my $arrRet = arr();
  my $hashData=hashReadTBL($varTableFile,{'szPrimary'=>'$(row)'});
  foreach my $szID (sort{$a>$b}(keys(%{$hashData}))) {
    push(@{$arrRet},$hashData->{$szID});
  }
  return ($arrRet);
}

# reads a tbl file to hash of hashes assuming the first column is a unique identifier or #PRIMARY is set
sub hashReadTBL {
  my ($varTableFile,$opt_varPrimaryOrOptions) = @_;
  my $hashRet = hash();
  my $hashOptions=hash(
    'szPrimary',varUNDEF(),
    'szDelimiter','[\\s]+'
  );
  
  if ( (defined($opt_varPrimaryOrOptions)) and (ref($opt_varPrimaryOrOptions) eq 'HASH')){
    foreach my $szKey(keys(%{$opt_varPrimaryOrOptions})) {
      $hashOptions->{$szKey}=$opt_varPrimaryOrOptions->{$szKey};
    }
    $opt_varPrimaryOrOptions=undef;
  }
  my $arrLines=arr();
  if (ref($varTableFile) eq 'ARRAY') {
    push(@{$arrLines},@{$varTableFile}); 
  } else {
    push(@{$arrLines},@{arrReadFile($varTableFile)});
  }
  my $arr_hashFields  = arr();
  my $boolInDataSpace = boolFALSE();
  my $boolInHeader= boolFALSE();
  my $szDelim=$hashOptions->{'szDelimiter'}; # default delimiter is one or more spaces
  my $szPrimaryKey;
  my $intRowIDX=0;
  for (my $intLine=0;$intLine<scalar(@{$arrLines});$intLine++) {
    my $szLine=$arrLines->[$intLine];
    $szLine = szTrim($szLine);

    # comment line ?
    if ( ( $szLine =~ m/^#/ ) and ( not($boolInDataSpace) ) ) {
      if ( $szLine =~ m/^#TBL V/i ) {

        # Table Format Version
      }
      elsif ( $szLine =~ m/^#TITLE:/i ) {

        # Table title
      }
      elsif ( $szLine =~ m/^#COMMENT:/i ) {

        # ignore comments
      }
      elsif ( $szLine =~ m/^#PRIMARY:/i ) {

        # Primary Key to remember
        ($szPrimaryKey) = $szLine =~ m/^#PRIMARY:[\s]*([^\s]*)[\s]*/i;
      }
      elsif ( $szLine =~ m/^#DELIM:/i ) {

        # delimiter found
        ($szDelim) = $szLine =~ m/^#DELIM:[\s]*([^\s]*)[\s]*/i;

      }
      elsif ( $szLine =~ m/^#HEADER:/i ) {
          
        # Following Line is the header
        $boolInHeader= boolTRUE();
      } else {
        if ($boolInHeader) {
          my $szRLine=$szLine;
          $szLine =~ s/^#+[\s]*//;
          my $szHeaderDelim=$szDelim;
          if ($szDelim=~m/usecols/i) {
            $szHeaderDelim ='[\s]+';
          }
          foreach my $szField (split( /$szDelim/, $szLine )) {
            my $hashField=hash();
            $hashField->{'intColumn'}=index($szRLine,$szField);
            ($hashField->{'szType'})=$szField=~ m/[\(](.*?)[\)]/;
            ($hashField->{'szComment'})=$szField=~ m/[\[](.*?)[\]]/;
            $hashField->{'szName'}=lc( $szField );
            # trim comment and type
            $hashField->{'szName'}=~ s/[\(](.*?)[\)]//g;
            $hashField->{'szName'}=~ s/[\[](.*?)[\]]//g;
            # push field
            push(@{$arr_hashFields},$hashField);
          }
          # switch first column
          $arr_hashFields->[0]->{'intColumn'}=0;
          # get primary key structure
          if ( ( not defined($szPrimaryKey) ) or (not ($szPrimaryKey=~m/[^\s]/))) {
            $szPrimaryKey = '$(' . $arr_hashFields->[0]->{'szName'} . ')';
          }
          # if there is an explicit primary key given
          if (defined($hashOptions->{'szPrimary'})) {
            $szPrimaryKey=$hashOptions->{'szPrimary'};
          }
          #print modGlobal::szGetStructure($arr_hashFields,'HEADER');
        }
      }
    }
    else {

      # process line
      if ( ( $szLine =~ m/^#/ ) or ( $szLine eq '' ) or ( not( defined($szLine) ) ) ) {

        # comment in data block ? must be record commented out so ignore it
        # could be empty line, too
      }
      else {
        $boolInDataSpace = boolTRUE();
        if (not ($boolInHeader)) {
          warn sprintf('WARNING: there was no header in the tbl you try to read !');
        }
        # oh we got a real record, cool
        # process it !
        # split first
        my $arrData=arr();
        if (not($szDelim=~m/usecols/i)) {
          # split by delimiter
          @{$arrData}=split( /$szDelim/, $szLine );
        } else {
          # split by column not by spaces
          my $intMaxI=scalar(@{$arr_hashFields})-1;
          my $intMaxLen=length($szLine);
          #print $szLine.'!'.$szLE;
          for (my $intI=0;$intI<=$intMaxI;$intI++) {
            my $intStart=$arr_hashFields->[$intI]->{'intColumn'};
            my $intEnd;
            if ($intI==$intMaxI) {
              # process last field
              $intEnd=$intMaxLen;
            } else {
              # process all other fields
              $intEnd=$arr_hashFields->[$intI+1]->{'intColumn'};
            }
            # if string is shorter than end
            if ($intEnd>$intMaxLen) {
              $intEnd=$intMaxLen;
            }
            # only if string length is at minimum field start
            if ($intStart<$intMaxLen) {
              #print sprintf('%s:%s to %s',$intI,$intStart,$intEnd).$szLE;
              my $intLength=$intEnd-$intStart;
              my $szData=substr($szLine,$intStart,$intLength);
              $szData=szTrim($szData);
              push(@{$arrData},$szData);
            }
          }
          #<STDIN>;
        }
        my $hashRecord = hash();
        for ( my $intI = 0 ; $intI < scalar( @{$arr_hashFields} ) ; $intI++ ) {
          if ( defined( $arrData->[$intI] ) ) {
            $hashRecord->{ $arr_hashFields->[$intI]->{'szName'} } = $arrData->[$intI];
          }
        }
        # make key
        my $szKey = $szPrimaryKey;
        #print szGetStructure($arr_hashFields);
        while ( $szKey =~ m/[\$][\(][^\)]*[\)]/g ) {
          my ($szFieldName) = $szKey =~ m/[\$][\(]([^\)]*)[\)]/;
          my $szValue; 
          if ($szFieldName eq 'linenumber')  {
            $szValue=$intLine;
          } elsif ( $szFieldName eq 'row') {
            $szValue=$intRowIDX;
          } else {
            $szValue = $hashRecord->{ lc($szFieldName) };
          }
          $szKey =~ s/[\$][\(]$szFieldName[\)]/$szValue/g;
        }
        # write record
        $hashRet->{ lc($szKey) } = $hashRecord;
        $intRowIDX++;
      }
    }
  }
  return ($hashRet);
}

# makes a deep copy of something
sub varDeepCopy {
  my ($varData)=@_;
  my $varRet;
  require Storable;
  $varRet=Storable::dclone($varData);
  return ($varRet);
}

# convert hex color to float, possible with maxbase
sub szColorHEX2Float {
  my ($szColor,$opt_floatMax)=@_;
  my $szRet='';
  if (not defined($opt_floatMax)) { $opt_floatMax=1; };
  # trim #-char
  $szColor=~s/^#//g;
  $szColor=uc($szColor);
  my $floatR=0;
  my $floatG=0;
  my $floatB=0;
  if (length($szColor)==3) {
    $floatR=index($szHEXDigits,substr($szColor,0,1))/15*$opt_floatMax;
    $floatG=index($szHEXDigits,substr($szColor,1,1))/15*$opt_floatMax;
    $floatB=index($szHEXDigits,substr($szColor,2,1))/15*$opt_floatMax;
  } elsif (length($szColor)==6) {
    $floatR=(index($szHEXDigits,substr($szColor,0,1))*16+index($szHEXDigits,substr($szColor,1,1)))/255*$opt_floatMax;
    $floatG=(index($szHEXDigits,substr($szColor,2,1))*16+index($szHEXDigits,substr($szColor,3,1)))/255*$opt_floatMax;
    $floatB=(index($szHEXDigits,substr($szColor,4,1))*16+index($szHEXDigits,substr($szColor,5,1)))/255*$opt_floatMax;
  }
  $szRet.=sprintf('%f,%f,%f',$floatR,$floatG,$floatB);
  return($szRet);
}

# convert hex color to int, possibly with maxbase
sub szColorHEX2Int {
  my ($szColor,$opt_intMax)=@_;
  my $szRet='';
  if (not defined($opt_intMax)) {
    $opt_intMax=255;
  }
  my $arrRGB=arr();
  @{$arrRGB}=split(/,/,szColorHEX2Float($szColor));
  foreach my $szComponent (@{$arrRGB}) {
    $szComponent *= $opt_intMax;
    $szRet.=sprintf(',%s',int($szComponent));
  }
  $szRet=~s/^,//;
  return($szRet);
}

# 
sub szGenerateUnique {
  my ($opt_intLen)=@_;
  my $szRet;
  if (boolNot(boolIsDefined($opt_intLen))) {
    $opt_intLen=16;
  }
  my $arr_charT=arr();
  my $szChars='0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
  my $intL=intLen($szChars);
  for (my $intI=0;$intI<$opt_intLen;$intI++) {
    my $intR= intRnd($intL);
    voidPush($arr_charT,charCharAt($szChars, $intR));
  }
  $szRet=szJoin('',$arr_charT);
  return ($szRet);
}

sub _szGetTempDir {
  my ($opt_szTempDir)=@_;
  my $szRet=$opt_szTempDir;
  if (boolNot(boolIsDefined($szRet))) {
    $szRet=$ENV{'TMP'};
  }
  # try TEMP
  if (boolNot(boolIsDefined($szRet))) {
    $szRet=$ENV{'TEMP'};
  }
  # try common paths
  if (boolNot(boolIsDefined($szRet))) {
    foreach my $szDir ('/tmp','c:/temp','c:/windows/temp') {
      if (boolDirectoryExists($szDir)) {
        $szRet=$szDir;
      }
    }
  }
  # just use curdir
  if (boolNot(boolIsDefined($szRet))) {
    $szRet='.';
  }
  return($szRet);
}

# return name of a temporary file
sub szGetTmpFileName {
  my ( $opt_szType, $opt_szExtension,$opt_szTempDir ) = @_;
  my $szRet;
  if (boolNot(boolIsDefined($opt_szType))) {
    $opt_szType='tmp';
  }
  if (boolNot(boolIsDefined($opt_szExtension))) {
    $opt_szExtension='tmp';
  }
  $opt_szTempDir=_szGetTempDir($opt_szTempDir);
  $opt_szTempDir=szReplaceRegEx($opt_szTempDir,'[\\\\\\/]$','');
  
  do {
    my $szID = szGenerateUnique(5);
    $szRet = szPrintf('%s/%s%s_%s.%s',arr($opt_szTempDir,$opt_szType,$$,$szID,$opt_szExtension));
  } while ( boolFileExists( $szRet  ) );
  return ($szRet);
}

# return name of a temporary directory and create it
sub szGetTmpDirName {
  my ( $opt_szType, $opt_szTempDir ) = @_;
  my $szRet;
  if (boolNot(boolIsDefined($opt_szType))) {
    $opt_szType='tmp';
  }
  $opt_szTempDir=_szGetTempDir($opt_szTempDir);
  $opt_szTempDir=szReplaceRegEx($opt_szTempDir,'[\\\\\\/]$','');
  
  do {
    my $szID = szGenerateUnique(5);
    $szRet = szPrintf('%s/%s%s_%s',arr($opt_szTempDir,$opt_szType,$$,$szID));
  } while ( boolDirectoryExists( $szRet ) );
  voidCreateDir($szRet);
  return ($szRet);
}


# fetches a html or any other file using ftp or http
sub objFetchURL {
  my ($szURL,$opt_szProxy)=@_;
  my $objRet;
  require LWP;
  my $objUserAgent = LWP::UserAgent->new();
  if ( boolIsDefined($opt_szProxy) and boolNot(boolIsEqual(szTrim($opt_szProxy),'')) ) {
    if (boolNot(boolMatchRegEx($opt_szProxy,'/((http)|(ftp)):[\\/][\\/]/i'))) {
      $opt_szProxy=szCombineStrings('http://',$opt_szProxy);
    }
    $objUserAgent->proxy( arr('http', 'ftp' ), $opt_szProxy );
  }
  $objRet = $objUserAgent->get( $szURL, 'User-Agent' => 'Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)' );
  if (boolNot( $objRet->is_success() )) {
    $objRet=varUNDEF();
  }
  return($objRet);
}

# determine handles readability
sub boolIsReadable {
  my ($handleIn,$opt_floatTimeout)=@_;
  my $boolRet= boolFALSE();
  require IO::Select;
  my $objIOSelect=new IO::Select($handleIn);
  my $arrReady=arr();
  @{$arrReady}=$objIOSelect->can_read($opt_floatTimeout);
  foreach my $handleH (@{$arrReady}) {
    if ($handleH==$handleIn) {
      $boolRet= boolTRUE();
    }
  }
  return($boolRet); 
}

sub szOCT {
  my ($intNum,$opt_intLen)=@_;
  my $szRet=szDec2Base($intNum,8,$opt_intLen);
  return($szRet);
}

sub szBIN {
  my ($intNum,$opt_intLen)=@_;
  my $szRet=szDec2Base($intNum,2,$opt_intLen);
  return($szRet);
}

sub szString2HEX {
  my ($szString,$opt_intLen)=@_;
  my $szRet='';
  if (boolNot(boolIsDefined($opt_intLen))) {
    $opt_intLen=4;
  }
  for (my $intI=0;$intI<intLen($szString);$intI++) {
    $szRet.=szHEX(intCharCode(charCharAt($szString, $intI)),$opt_intLen).' ';
  }
  return($szRet);
}

sub intDeHEX {
  my ($szNum)=@_;
  my $intRet=(substr($szNum,0,1)eq'-')?-hex(substr($szNum,1)):hex($szNum);
  return($intRet); 
}

#sub szHEX2 {
#  my ($intNum,$opt_intLen)=@_;
#  my $szRet=szDec2Base($intNum,16,$opt_intLen);
#  return($szRet);
#}

#sub intDeHEX2 {
#  my ($szNum)=@_;
#  my $intRet=intBase2Dec($szNum,16);
#  return($intRet); 
#}

sub intBase2Dec {
  my ($szNum,$intBase)=@_;
  my $intRet=0;
  my $boolNegative=boolFALSE();
  if (boolIsEqual(charCharAt($szNum,0),'-')) {
    $boolNegative=boolTRUE();
    $szNum=szRight($szNum,intLen($szNum)-1);
  }
  my $szDigits='0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  $szNum=szUCase($szNum);
  if ( ($intBase<2) or ($intBase>intLen($szDigits))) {
    voidError(
      szPrintf(
        'szDec2Base: base must be in range 2-%s but is %s',
        arr(intLen($szDigits),$intBase)
      )
    );
  }
  $szDigits=szLeft($szDigits,$intBase);
  for (my $intI=0;$intI<intLen($szNum);$intI++) {
    my $charChar=charCharAt($szNum,$intI);
    if (boolNot(boolInstr($szDigits,$charChar))) {
      voidError(
        szPrintf(
          'szDec2Base: error converting %s as a base %s to decimal',
          arr($szNum,$intBase)
        )
      );
    } else {
      $intRet*=$intBase;
      $intRet+=intInstr($szDigits,$charChar);
    }
  }
  
  if ($boolNegative) {
    $intRet*=(-1);
  }
  return($intRet);
}

# convert from decimal to any other base
sub szDec2Base {
  my ($intNum,$intBase,$opt_intLen)=@_;
  my $szRet='';
  if (boolNot(boolIsDefined($opt_intLen))) {
    $opt_intLen=0;
  }
  my $boolNegative=boolFALSE();
  $intNum=intInt($intNum);
  if ($intNum<0) {
    $intNum*=(-1);
    $boolNegative=boolTRUE();
  }
  my $szDigits='0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  if ( ($intBase<2) or ($intBase>intLen($szDigits))) {
    voidError(
      szPrintf(
        'szDec2Base: base must be in range 2-%s but is %s',
        arr(intLen($szDigits),$intBase)
      )
    );
  }
  my $intI=0;
  do {
    my $intRest=$intNum % $intBase;
    $intNum-=$intRest;
    $szRet=szCombineStrings(charCharAt($szDigits,$intRest),$szRet);
    $intNum/=$intBase;
    $intI++;
  } until (($intNum<=0) and ($intI>=$opt_intLen));
  if ($boolNegative) {
    $szRet=szCombineStrings('-',$szRet);
  }
  return($szRet);
}

# save struct to disk
sub voidSaveStruct {
  my ($szFile,$varStruct,$opt_szType)=@_;
  voidWriteFile($szFile,arr(szSaveStruct($varStruct,$opt_szType)));
}

sub szSaveStruct {
  my ($varStruct,$opt_szType)=@_;
  my $szRet;
  if (boolNot(boolIsDefined($opt_szType))) {
    $opt_szType='XML';
  }
  $opt_szType=szUCase($opt_szType);
  if (boolIsEqual($opt_szType,'XML')) {
    $szRet= szJoin($szLE,arr(
      '<?xml version="1.0" encoding="iso-8859-1" ?>',
      objXMLNodeFromStruct($varStruct)->szGetXML(hash('AutoCloseEmptyTags','true'))
    ));
  }
  return ($szRet);
}

# laod struct to disk
sub varLoadStruct {
  my ($varFile,$opt_szType)=@_;
  my $varRet;
  require Classes::classXMLNode;
  if (boolNot(boolIsDefined($opt_szType))) {
    $opt_szType='XML';
  }
  $opt_szType=szUCase($opt_szType);
  if (boolIsEqual(szRef($varFile),'SCALAR')) {
    $varFile=arrReadFile($varFile);
  }
  if (boolIsEqual($opt_szType,'XML')) {
    $varRet=varStructFromXMLNode(
      classXMLNode::objParseFromString(
        szJoin($szLE,$varFile)
      )
    );
  }
  return($varRet);
}

# parse xml tree to struct
sub varStructFromXMLNode {
  my ($objNode)=@_;
  my $varRet;
  if (boolIsDefined($objNode)) {
    require Modules::Convert::modQuotedPrint;
    my $szName=$objNode->szGetNodeName();
    if (boolIsEqual($szName,'scalar')) {
      $varRet=_szXMLDecode($objNode->szGetAttribute('value'));
    } elsif (boolIsEqual($szName,'hash')) {
      $varRet=hash();
      my $arrChildren=$objNode->arrGetChildNodes();
      for (my $intI=0;$intI<intArrLen($arrChildren);$intI++) {
        my $objCNode=varArrayItem($arrChildren,$intI);
        my $szKey=_szXMLDecode($objCNode->szGetAttribute('id'));
        voidHashItem($varRet,$szKey,varStructFromXMLNode($objCNode));
      }
    } elsif (boolIsEqual($szName,'array')) {
      $varRet=arr();
      my $arrChildren=$objNode->arrGetChildNodes();
      for (my $intI=0;$intI<intArrLen($arrChildren);$intI++) {
        my $objCNode=varArrayItem($arrChildren,$intI);
        my $intKey=$objCNode->szGetAttribute('id');
        if (boolNot(boolIsDefined($intKey))) {
          $intKey=$intI;
        }
        voidArrayItem($varRet,$intKey,varStructFromXMLNode($objCNode));
      }
    } elsif (boolIsEqual($szName,'object')) {
      $varRet=hash();
      my $szClass=_szXMLDecode($objNode->szGetAttribute('class'));
      bless($varRet,$szClass);
      my $arrChildren=$objNode->arrGetChildNodes();
      for (my $intI=0;$intI<intArrLen($arrChildren);$intI++) {
        my $objCNode=varArrayItem($arrChildren,$intI);
        my $szKey=_szXMLDecode($objCNode->szGetAttribute('id'));
        voidHashItem($varRet,$szKey,varStructFromXMLNode($objCNode));
      }
    } elsif (boolIsEqual($szName,'undefined')) {
      $varRet=varUNDEF();
    } else {
      voidWarn(szPrintf('Found unknown node type:%s, must be [scalar, hash, array, object, undefined]',arr($szName)));
    }
  }
  return($varRet);
}

# parse a struct to xml tree
sub objXMLNodeFromStruct {
  my ($varStruct)=@_;
  my $objRet;
  require Classes::classXMLNode;
  require Modules::Convert::modQuotedPrint;
  if (boolIsDefined($varStruct)) {
    my $szRef=szRef($varStruct);
    if (boolIsEqual($szRef,'SCALAR')) {
      $objRet=classXMLNode->new('scalar');
      if (boolIsInteger($varStruct)) {
        $objRet->voidSetAttribute('type','int');
      } elsif (boolIsFloat($varStruct)) {
        $objRet->voidSetAttribute('type','float');
      } else {
        $objRet->voidSetAttribute('type','string');
      }
      $objRet->voidSetAttribute('value',_szXMLEncode($varStruct));
    } elsif (boolIsEqual($szRef,'HASH')) {
      $objRet=classXMLNode->new('hash');
      my $arrKeys=arrSort(arrGetKeys($varStruct));
      for (my $intI=0;$intI<intArrLen($arrKeys);$intI++) {
        my $szKey=varArrayItem($arrKeys,$intI);
        my $objNode=objXMLNodeFromStruct(varHashItem($varStruct,$szKey));
        $objNode->voidSetAttribute('id',_szXMLEncode($szKey));
        $objRet->voidAddChildNode($objNode);
      }
    } elsif (boolIsEqual($szRef,'ARRAY')) {
      $objRet=classXMLNode->new('array');
      for (my $intI=0;$intI<intArrLen($varStruct);$intI++) {
        my $objNode=objXMLNodeFromStruct(varArrayItem($varStruct,$intI));
        $objRet->voidAddChildNode($objNode);
      }
    } else {
      $objRet=classXMLNode->new('object');
      $objRet->voidSetAttribute('class',_szXMLEncode($szRef));
      my $arrKeys=arrSort(arrGetKeys($varStruct));
      for (my $intI=0;$intI<intArrLen($arrKeys);$intI++) {
        my $szKey=varArrayItem($arrKeys,$intI);
        my $objNode=objXMLNodeFromStruct(varHashItem($varStruct,$szKey));
        $objNode->voidSetAttribute('id',_szXMLEncode($szKey));
        $objRet->voidAddChildNode($objNode);
      }
    }
  } else {
    $objRet=classXMLNode->new('undefined');
  }
  return($objRet);
}

sub _szXMLEncode {
  my ($szData)=@_;
  require Encode;
  return(szReplaceMultiArr(Convert::modQuotedPrint::szEncode(Encode::encode_utf8($szData)),arr(
    arr('=20',' '),
    arr('#','#stream;'),
    arr('&','#amp;'),
    arr('"','#quot;'),
    arr('<','#lt;'),
    arr('>','#gt;')
  )));
}

sub _szXMLDecode {
  my ($szData)=@_;
  require Encode;
  return(Encode::decode_utf8(Convert::modQuotedPrint::szDecode(szReplaceMultiArr($szData,arr(
    arr('#gt;','>'),
    arr('#lt;','<'),
    arr('#quot;','"'),
    arr('#amp;','&'),
    arr('#stream;','#')
  )))));
}

# return the file basename
sub szExtractFileName {
  my ($szFile)=@_;
  my $szRet=varArrayItem(arrMatchRegEx($szFile,'/([^\\\\\\/]*)$/'),0);
  return($szRet);
}

# returns the path
sub szExtractPathName {
  my ($szFile)=@_;
  my $szRet=varArrayItem(arrMatchRegEx($szFile,'/^(.*?)([^\\\\\\/]*)$/'),0);
  return($szRet);
}

# returns the file extension
sub szExtractExtension {
  my ($szFile)=@_;
  my $szRet=varArrayItem(arrMatchRegEx($szFile,'/([^\\.]*)$/'),0);
  return($szRet);
}

# checks for "schaltjahre" according to excel 1900 bug
sub _boolIsXCY {
  my ($intY)=@_;
  my $boolRet=($intY%4==0 and (boolNot($intY%100==0) or ($intY%400==0)))?boolTRUE():boolFALSE();
  # excel anomaly
  if ($intY==1900) {
    $boolRet=boolTRUE();
  } else {
    # no anomaly detected
  }
  
  return($boolRet);
}

# get difference of days between two dates according excel conventions
our $opt_hashIntXDayDiff=hash();
sub _intXDayDiff {
  my ($szDate1,$szDate2)=@_;
  my $intRet=0;
  my $szKey=szPrintf('%s^%s',arr($szDate1,$szDate2));
  if (boolNot(boolKeyExists($opt_hashIntXDayDiff,$szKey))) {
    my $boolInvert=boolFALSE();
    if ($szDate1>$szDate2) {
      my $szTmp=$szDate1;
      $szDate1=$szDate2;
      $szDate2=$szTmp;
      $boolInvert=boolTRUE();
    } else {
      # order is ok
    }
    my $intY1=szMid($szDate1,0,4)+0;
    my $intM1=szMid($szDate1,4,2)+0;
    my $intD1=szMid($szDate1,6,2)+0;
    my $intY2=szMid($szDate2,0,4)+0;
    my $intM2=szMid($szDate2,4,2)+0;
    my $intD2=szMid($szDate2,6,2)+0;
    while (
      ( ($intY1!=$intY2) or ($intM1!=$intM2) or ($intD1!=$intD2)) and boolNot
      ( 
        ($intY1>$intY2) or
        (($intY1==$intY2) and ($intM1>$intM2)) or
        (($intY1==$intY2) and ($intM1==$intM2) and ($intD1>$intD2))
      ) 
    ) {
      $intRet++;
      $intD1++;
      #print "Entscheidung für $intD1.$intM1.$intY1\n";
      if ( ($intD1==29) and ($intM1==2) and boolNot(_boolIsXCY($intY1))) {
        $intD1=1;
        $intM1++;
      } elsif ( ($intD1==30) and ($intM1==2) and _boolIsXCY($intY1)) {
        $intD1=1;
        $intM1++;
      } elsif ( ($intD1==31) and (boolInArr(arr('4','6','9','11'),"$intM1"))) {
        $intD1=1;
        $intM1++;
      } elsif ( ($intD1==32) and (boolInArr(arr('1','3','5','7','8','10','12'),"$intM1"))) {
        $intD1=1;
        $intM1++;
      }
      
      if ($intM1==13) {
        $intM1=1;
        $intY1++;
      }
      #print "Tag $intRet, $intD1.$intM1.$intY1\n";
    };
    $intRet++;
    if ($boolInvert) {
      $intRet*=-1;
    } else {
      # else did not invert dates
    }
    voidHashItem($opt_hashIntXDayDiff,$szKey,$intRet);
  } else {
    # calculation already done, using cache
  }
  $intRet=varHashItem($opt_hashIntXDayDiff,$szKey);
  return($intRet);
}

# get a new date from an existing
sub _szXDateAdd {
  my ($szDate,$intDays)=@_;
  my $szRet;
  my $intY=szMid($szDate,0,4)+0;
  my $intM=szMid($szDate,4,2)+0;
  my $intD=szMid($szDate,6,2)+0;
  # correct the excel plus-1 thingy
  if ($intDays>=0) {
    $intDays--;
  } else {
    $intDays++;
  }
  # direciton up or down the timeline
  my $intDX;
  if ($intDays>=0) {
    $intDX=1;
  } else {
    $intDX=-1;
  }
  while ($intDays!=0) {
    $intD+=$intDX;
    $intDays-=$intDX;
    if ( ($intD==29) and ($intM==2) and boolNot(_boolIsXCY($intY))) {
      $intD=1;
      $intM++;
    } elsif ( ($intD==30) and ($intM==2) and _boolIsXCY($intY)) {
      $intD=1;
      $intM++;
    } elsif ( ($intD==31) and (boolInArr(arr('4','6','9','11'),"$intM"))) {
      $intD=1;
      $intM++;
    } elsif ( ($intD==32) and (boolInArr(arr('1','3','5','7','8','10','12'),"$intM"))) {
      $intD=1;
      $intM++;
    } elsif ( ($intD==0) and ($intM==3) and boolNot(_boolIsXCY($intY))) {
      $intD=28;
      $intM--;
    } elsif ( ($intD==0) and ($intM==3) and _boolIsXCY($intY)) {
      $intD=29;
      $intM--;
    } elsif ( ($intD==0) and (boolInArr(arr('1','2','4','6','8','9','11'),"$intM"))) {
      $intD=31;
      $intM--;
    } elsif ( ($intD==0) and (boolInArr(arr('5','7','10','12'),"$intM"))) {
      $intD=30;
      $intM--;
    }
    
    if ($intM==13) {
      $intM=1;
      $intY++;
    } elsif ($intM==0) {
      $intM=12;
      $intY--;
    }
    #print "$intDays: $intD.$intM.$intY\n";
  }
  $szRet=szPrintf('%04d%02d%02d',arr($intY,$intM,$intD));
  return($szRet);
} 

# convert timecode to dateserial
sub floatTimeCode2Serial {
  my ($szTC)=@_;
  my $floatRet;
  my $arrParts=arrMatchRegEx($szTC,'/^([0-9]{4})([0-9]{2})?([0-9]{2})?[^0-9]?([0-9]{2})?([0-9]{2})?([0-9]{2})?/');
  if (intArrLen($arrParts)>0) {
    $arrParts->[1]=not(defined($arrParts->[1]))?1:$arrParts->[1];
    $arrParts->[2]=not(defined($arrParts->[2]))?1:$arrParts->[2];
    $arrParts->[3]=not(defined($arrParts->[3]))?0:$arrParts->[3];
    $arrParts->[4]=not(defined($arrParts->[4]))?0:$arrParts->[4];
    $arrParts->[5]=not(defined($arrParts->[5]))?0:$arrParts->[5];
    my $intEpoch=_intXDayDiff($szDateBase,szPrintf('%04d%02d%02d',arr($arrParts->[0],$arrParts->[1],$arrParts->[2])));
    my $floatTime=$arrParts->[3]/24+$arrParts->[4]/24/60+$arrParts->[5]/24/60/60;
    if ($intEpoch<0) {
      $floatRet=$intEpoch-$floatTime;
    } else {
      $floatRet=$intEpoch+$floatTime;
    }
  } else {
    voidWarn(szPrintf('timecode %s was not recognised',arr($szTC)));
  }
  return($floatRet);
}

# convert date serial to timecode
sub szSerial2TimeCode {
  my ($floatSerial)=@_;
  my $szRet;
  my $floatTime=intAbs($floatSerial);
  my $floatHour=($floatTime-intInt($floatTime))*24;
  my $floatMin=($floatHour-intInt($floatHour))*60;$floatHour=intInt($floatHour);
  my $floatSec=($floatMin-intInt($floatMin))*60;$floatMin=intInt($floatMin);
  $szRet=szPrintf('%0s-%02d%02d%02d',arr(_szXDateAdd($szDateBase,intInt($floatSerial)),$floatHour,$floatMin,$floatSec));
  return($szRet);
}


if ( $0 eq __FILE__ ) {

  # Loader goes here
} else {
  return ( 1 == 1 );
}


