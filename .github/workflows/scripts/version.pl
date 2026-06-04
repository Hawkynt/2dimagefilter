#!/usr/bin/perl
# -----------------------------------------------------------------------------
#  version.pl — the ONE versioner, identical in every Hawkynt repo.
#
#  Model: FILES drive versions, never git tags. For EACH version-bearing file,
#  the build number is the commit count of THAT file's PARENT FOLDER — so a
#  version advances by how much the directory that declares it actually changed.
#  A project that inherits its version from a Directory.Build.props is therefore
#  versioned by the props file's folder, not the project's.
#
#  Version sources (kind -> file -> field):
#    dotnet : *.csproj/*.fsproj/*.vbproj, Directory.Build.props/.targets   <Version>
#    node   : package.json                                                  "version"
#    php    : composer.json                                                 "version"
#    perl   : *.pm that declares $VERSION                                    $VERSION
#  Composition respects each grammar:  dotnet/perl -> X.Y.Z.BUILD ,
#  semver node/php -> X.Y.Z+BUILD (a 4th numeric part is invalid there).
#
#  Two integration styles, both supported:
#    * STAMP (files drive): `--stamp` rewrites the version in every source file;
#      the build then packs straight from those files.
#    * COMPUTE-AND-PASS (one coordinated version): a repo that centralises its
#      version in Directory.Build.props runs bare `version.pl` to get a single
#      string and passes it via `-p:Version=...` to every pack/publish.
#
#  Usage:
#    perl version.pl            # print the repo's single version  (X.Y.Z.BUILD)
#    perl version.pl --base     # print just the base              (X.Y.Z)
#    perl version.pl --build    # print just the build number      (commit count)
#    perl version.pl --stamp    # rewrite the version in every source file
#    perl version.pl --list     # print "<file>\t<composed-version>" per source
#
#  The single-version modes use the PRIMARY source: a root VERSION file, else the
#  shallowest Directory.Build.props with a <Version>, else the shallowest csproj
#  with one. Their build number is that primary source's parent-folder count.
#
#  Exit: 0 success, 2 bad usage.
# -----------------------------------------------------------------------------
use strict;
use warnings;
use FindBin;
use File::Find;
use File::Copy;

my $mode = $ARGV[0] // '';
exit 2 unless $mode eq '' || $mode eq '--base' || $mode eq '--build'
           || $mode eq '--stamp' || $mode eq '--list';

my $root = _RepoRoot("$FindBin::Bin");

# ---- single-version modes (compute-and-pass repos) -------------------------
if ($mode eq '' || $mode eq '--base' || $mode eq '--build') {
    my ($base, $dir) = _PrimarySource($root);
    if ($mode eq '--build') {
        print _BuildNumber($root, $dir), "\n";   # $dir undef -> repo-wide
        exit 0;
    }
    die "version.pl: no version source (VERSION / Directory.Build.props / csproj <Version>)\n"
        unless defined $base;
    print(($mode eq '--base') ? $base : _Compose('dotnet', $base, _BuildNumber($root, $dir)), "\n");
    exit 0;
}

# ---- per-source modes (stamp / list) ---------------------------------------
my @manifests = _Manifests($root);

if ($mode eq '--list') {
    for my $m (@manifests) {
        my ($file, $kind) = @$m;
        my $base = _ReadBase($kind, $file);
        next unless defined $base;
        print "$file\t" . _Compose($kind, $base, _BuildNumber($root, _ParentDir($root, $file))) . "\n";
    }
    exit 0;
}

# --stamp
my $n = 0;
for my $m (@manifests) {
    my ($file, $kind) = @$m;
    my $base = _ReadBase($kind, $file);
    next unless defined $base;
    my $full = _Compose($kind, $base, _BuildNumber($root, _ParentDir($root, $file)));
    $n++ if _Rewrite($kind, $file, $full);
}
print "stamped $n source file(s) with per-folder build numbers\n";
exit 0;

# --------------------------------------------------------------------------- #

sub _Compose {
    my ($kind, $base, $build) = @_;
    return undef unless defined $base;
    my @p = split /\./, $base;
    @p = @p[0 .. 2] if @p > 3;
    my $core = join('.', @p);
    return ($kind eq 'node' || $kind eq 'php') ? "$core+$build" : "$core.$build";
}

sub _Kind {
    my ($f) = @_;
    return 'dotnet' if $f =~ /\.(?:csproj|fsproj|vbproj)$/i;
    return 'dotnet' if $f =~ m{(?:^|[/\\])Directory\.Build\.(?:props|targets)$}i;
    return 'node'   if $f =~ m{(?:^|[/\\])package\.json$}i;
    return 'php'    if $f =~ m{(?:^|[/\\])composer\.json$}i;
    return 'perl'   if $f =~ /\.pm$/i;
    return undef;
}

sub _Manifests {
    my ($r) = @_;
    my @out;
    my %skip = map { $_ => 1 } qw(
        bin obj packages node_modules .git .vs .idea TestResults
        artifacts publish dist stage coverage vendor .svn
    );
    File::Find::find(
        {
            no_chdir   => 1,
            preprocess => sub { grep { !$skip{$_} } @_ },
            wanted     => sub {
                my $f = $File::Find::name;
                return unless -f $f;
                my $kind = _Kind($f);
                push @out, [$f, $kind] if $kind;
            },
        },
        $r,
    );
    return sort { $a->[0] cmp $b->[0] } @out;
}

# Primary source for the single repo version: VERSION, else the shallowest
# Directory.Build.props with a <Version>, else the shallowest csproj with one.
# Returns ($base, $parentDirRelativeToRoot) or (undef, undef).
sub _PrimarySource {
    my ($r) = @_;
    my $vf = "$r/VERSION";
    if (-r $vf) {
        my $b = _VersionFile($r);
        return ($b, '') if defined $b;
    }
    my (@props, @projs);
    for my $m (_Manifests($r)) {
        my ($file, $kind) = @$m;
        next unless $kind eq 'dotnet';
        next unless defined _ReadDotnet($file);
        if ($file =~ m{Directory\.Build\.(?:props|targets)$}i) { push @props, $file }
        else { push @projs, $file }
    }
    my $depth = sub { my $d = _ParentDir($r, $_[0]); ($d eq '') ? 0 : ($d =~ tr{/\\}{}) + 1 };
    for my $list (\@props, \@projs) {
        next unless @$list;
        my ($best) = sort { $depth->($a) <=> $depth->($b) || $a cmp $b } @$list;
        return (_ReadDotnet($best), _ParentDir($r, $best));
    }
    return (undef, undef);
}

# ---- per-kind readers ------------------------------------------------------

sub _ReadBase {
    my ($kind, $f) = @_;
    return _ReadDotnet($f) if $kind eq 'dotnet';
    return _ReadJson($f)   if $kind eq 'node' || $kind eq 'php';
    return _ReadPerl($f)   if $kind eq 'perl';
    return undef;
}

sub _Slurp {
    my ($f) = @_;
    open my $fh, '<', $f or return undef;
    local $/;
    my $c = <$fh>;
    close $fh;
    return $c;
}

sub _ReadDotnet {
    my ($f) = @_;
    my $c = _Slurp($f) // return undef;
    return $c =~ m{<Version>\s*(\d+(?:\.\d+){0,2})\s*</Version>}i ? $1 : undef;
}

sub _ReadJson {
    my ($f) = @_;
    my $c = _Slurp($f) // return undef;
    return $c =~ m{"version"\s*:\s*"v?(\d+(?:\.\d+){0,2})}i ? $1 : undef;
}

sub _ReadPerl {
    my ($f) = @_;
    my $c = _Slurp($f) // return undef;
    return $c =~ m{\$VERSION\s*=\s*['"]v?(\d+(?:\.\d+){0,3})}i ? $1 : undef;
}

# ---- per-kind rewriters (return 1 if the file changed) ---------------------

sub _Rewrite {
    my ($kind, $f, $full) = @_;
    my $c = _Slurp($f);
    return 0 unless defined $c;
    my $orig = $c;
    if ($kind eq 'dotnet') {
        $c =~ s{<Version>\s*[\w.+\-]+\s*</Version>}{<Version>$full</Version>}ig;
    } elsif ($kind eq 'node' || $kind eq 'php') {
        $c =~ s{("version"\s*:\s*")[^"]*(")}{$1$full$2}i;   # first occurrence only
    } elsif ($kind eq 'perl') {
        $c =~ s{(\$VERSION\s*=\s*['"])[^'"]*(['"])}{$1$full$2};
    } else {
        return 0;
    }
    return 0 if $c eq $orig;
    my $tmp = "$f.\$\$\$";
    open my $out, '>', $tmp or die "write $tmp: $!";
    print $out $c;
    close $out;
    File::Copy::move($tmp, $f) or die "replace $f: $!";
    return 1;
}

# ---- git / path helpers ----------------------------------------------------

sub _RepoRoot {
    my ($d) = @_;
    for (1 .. 20) {
        return $d if -d "$d/.git";
        my $p = $d;
        $p =~ s{[/\\][^/\\]+$}{};
        last if $p eq $d || $p eq '';
        $d = $p;
    }
    return $d;
}

sub _BuildNumber {
    my ($r, $rel) = @_;
    my $spec = (defined $rel && length $rel) ? " -- \"$rel\"" : "";
    my $c = `git -C "$r" rev-list --count HEAD$spec 2>&1`;
    chomp $c;
    return $c =~ /^\d+$/ ? $c : '0';
}

sub _VersionFile {
    my ($r) = @_;
    my $vf = "$r/VERSION";
    return undef unless -r $vf;
    open my $fh, '<', $vf or return undef;
    chomp(my $v = <$fh>);
    close $fh;
    $v =~ s/^\s+|\s+$//g if defined $v;
    return (defined $v && length $v) ? $v : undef;
}

# Path of a source file's directory, relative to the repo root ('' = repo root).
sub _ParentDir {
    my ($root, $file) = @_;
    (my $dir = $file) =~ s{[/\\][^/\\]+$}{};
    (my $r   = $root) =~ s{[/\\]$}{};
    $dir =~ s{^\Q$r\E[/\\]?}{};
    return $dir;
}
