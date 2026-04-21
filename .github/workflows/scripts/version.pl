#!/usr/bin/perl
# -----------------------------------------------------------------------------
#  .github/workflows/scripts/version.pl
#    Unified version tool. Prints MAJOR.MINOR.PATCH.BUILD where
#    MAJOR.MINOR.PATCH comes from (in priority order):
#       1. the repo's VERSION file
#       2. any csproj at root or one level deep (first <Version> tag wins)
#       3. Directory.Build.props at repo root
#    and BUILD is `git rev-list --count HEAD`.
#
#  Usage:
#    perl version.pl              # 1.0.0.123
#    perl version.pl --base       # 1.0.0
#    perl version.pl --build      # 123
#    perl version.pl --stamp      # writes X.Y.Z.BUILD into every csproj
#                                 # (replaces the old UpdateVersions.pl). Only
#                                 # updates <Version> tags that already exist;
#                                 # never inserts new ones.
#
#  Exit codes: 0 on success, 2 on argument error, 1 on any other failure.
# -----------------------------------------------------------------------------
use strict;
use warnings;
use FindBin;
use File::Find;
use File::Copy;
use File::Glob qw(bsd_glob);

my $mode = $ARGV[0] // '';
die "usage: $0 [--base|--build|--stamp]\n"
  unless $mode eq '' || $mode eq '--base' || $mode eq '--build' || $mode eq '--stamp';

# Repo root: walk up from this script until we find .git or hit filesystem root.
# Works whether the script lives in scripts/ or .github/workflows/scripts/.
my $repoRoot = _FindRepoRoot("$FindBin::Bin");

if ($mode eq '--build') {
    print _QueryBuildNumber($repoRoot), "\n";
    exit 0;
}

my $base = _QueryBaseVersion($repoRoot);
if ($mode eq '--base') {
    print $base, "\n";
    exit 0;
}

my $build = _QueryBuildNumber($repoRoot);
my $full  = "$base.$build";

if ($mode eq '--stamp') {
    my $stamped = _StampCsprojs($repoRoot, $full);
    print "stamped $stamped csproj file(s) with $full\n";
    exit 0;
}

print "$full\n";
exit 0;

# ---------------------------------------------------------------------------

sub _FindRepoRoot {
    my ($from) = @_;
    my $dir = $from;
    for (1..16) {
        return $dir if -d "$dir/.git";
        my $parent = $dir;
        $parent =~ s{[/\\][^/\\]+$}{};
        last if $parent eq $dir || $parent eq '';
        $dir = $parent;
    }
    # No .git found -- assume the script's great-grandparent is the root
    # (matches both scripts/ and .github/workflows/scripts/ layouts).
    return "$from/../../../..";
}

sub _QueryBaseVersion {
    my ($root) = @_;

    # 1) VERSION file at repo root.
    my $versionFile = "$root/VERSION";
    if (-r $versionFile) {
        open my $fh, '<', $versionFile or die "cannot read $versionFile: $!\n";
        chomp(my $raw = <$fh>);
        close $fh;
        $raw =~ s/^\s+|\s+$//g if defined $raw;
        if (defined $raw && length $raw) {
            my @parts = split /\./, $raw;
            push @parts, '0' while @parts < 3;
            return join('.', @parts[0..2]);
        }
    }

    # 2) csproj at root or one level deep.
    for my $file (_FindCsprojFiles($root)) {
        my $v = _ReadVersionTag($file);
        return $v if defined $v;
    }

    # 3) Directory.Build.props.
    my $props = "$root/Directory.Build.props";
    if (-f $props) {
        my $v = _ReadVersionTag($props);
        return $v if defined $v;
    }

    die "version.pl: no VERSION file, no <Version> in csproj or Directory.Build.props under $root\n";
}

sub _FindCsprojFiles {
    my ($root) = @_;
    my @files;
    # Walk the whole tree. File::Find traverses symlinks-by-default, which is
    # usually fine for a source tree. We prune noisy / generated directories
    # so a repo with hundreds of format libs doesn't take forever.
    my %skip = map { $_ => 1 } qw(
        bin obj packages node_modules .git .github .vs .idea .svn
        TestResults artifacts publish dist stage coverage
    );
    File::Find::find(
        {
            preprocess => sub {
                return grep { !$skip{$_} } @_;
            },
            wanted => sub {
                push @files, $File::Find::name if /\.csproj$/i && -f $File::Find::name;
            },
            no_chdir => 1,
        },
        $root,
    );
    return @files;
}

sub _ReadVersionTag {
    my ($file) = @_;
    open my $fh, '<', $file or return undef;
    while (my $line = <$fh>) {
        # Strict: 3 numeric dot-separated segments only. Avoids matching
        # PackageVersion / AssemblyVersion / FileVersion tags.
        if ($line =~ m{<Version>\s*(\d+\.\d+\.\d+)\s*</Version>}) {
            close $fh;
            return $1;
        }
    }
    close $fh;
    return undef;
}

sub _QueryBuildNumber {
    my ($root) = @_;
    my $count = `git -C "$root" rev-list --count HEAD 2>&1`;
    return '0' if $? != 0;
    chomp $count;
    return $count =~ /^\d+$/ ? $count : '0';
}

# Walk the whole tree (not just 1 level) and rewrite any <Version>X.Y.Z</Version>
# tags to the supplied full version. Returns the number of files changed.
sub _StampCsprojs {
    my ($root, $full) = @_;
    my @csprojs;
    File::Find::find(
        sub {
            my $p = $File::Find::name;
            push @csprojs, $p if $p =~ /\.csproj$/i && -f $p;
        },
        $root,
    );
    my $changed = 0;
    for my $file (@csprojs) {
        $changed++ if _RewriteVersionTag($file, $full);
    }
    return $changed;
}

sub _RewriteVersionTag {
    my ($file, $full) = @_;
    my $tmp = "$file.\$\$\$";
    open my $in,  '<', $file or die "cannot read $file: $!\n";
    open my $out, '>', $tmp  or die "cannot write $tmp: $!\n";
    my $hit = 0;
    while (my $line = <$in>) {
        if ($line =~ s{<Version>\s*(\d+\.\d+\.\d+(?:\.\d+)?)\s*</Version>}{<Version>$full</Version>}) {
            $hit = 1;
        }
        print $out $line;
    }
    close $out;
    close $in;
    if ($hit) {
        File::Copy::move($tmp, $file) or die "cannot replace $file: $!\n";
        return 1;
    }
    unlink $tmp;
    return 0;
}
