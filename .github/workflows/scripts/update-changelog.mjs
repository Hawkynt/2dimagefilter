// Maintains CHANGELOG.md. Invoked from the nightly and release workflows.
//
// Usage:
//   node .github/workflows/scripts/update-changelog.mjs --nightly --notes-only  # "## Nightly YYYY-MM-DD (<version>)"
//   node .github/workflows/scripts/update-changelog.mjs --release v1.2.3        # "## v1.2.3 (YYYY-MM-DD)"
//
// Flags:
//   --notes <file>   write the release-notes body (section minus header) there
//   --notes-only     generate notes but do NOT touch CHANGELOG.md — used by
//                    nightly.yml, which never commits the changelog (only
//                    release.yml does), so writing it would be a dead write
//   --version X.Y.Z  decorates the nightly header
//
// Commit subject conventions (see bucketize() below):
//   + Added  * Changed  # Fixed  - Removed  ! TODO
// Anything else goes into "Other". The workflow's own changelog-refresh
// commits ("* update changelog for vyyyyMMdd") are bookkeeping, not change —
// they are filtered out entirely (see isChangelogCommit()).

import fs   from 'node:fs';
import path from 'node:path';
import url  from 'node:url';
import { spawnSync } from 'node:child_process';

const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
// Script lives at <repo>/.github/workflows/scripts/ -- repo root is three up.
const REPO_ROOT = path.resolve(__dirname, '..', '..', '..');
const CHANGELOG = path.join(REPO_ROOT, 'CHANGELOG.md');

// ---------------------------------------------------------------------------
// Pure helpers (testable)
// ---------------------------------------------------------------------------
// Commit-subject conventions used by this repo:
//   + <text>   Added
//   * <text>   Changed
//   # <text>   Fixed
//   - <text>   Removed
//   ! <text>   open TODO (known-but-not-yet-done item worth recording)
// Anything else is bucketed as "Other".
//
// Bucket order in the rendered section is fixed: additions first, then
// changes, then bug-fixes, then removals, then open TODOs, then the
// catch-all.
// Order matches the commit-prefix convention +  -  *  #  !
export const BUCKET_ORDER = ['Added', 'Removed', 'Changed', 'Fixed', 'TODO', 'Other'];
const PREFIX_TO_BUCKET = {
    '+': 'Added',
    '*': 'Changed',
    '#': 'Fixed',
    '-': 'Removed',
    '!': 'TODO',
};

// The release workflow's own bookkeeping commit ("* update changelog for
// vyyyyMMdd [skip ci]"). It must never appear in notes: even though release.yml
// tags the release ON that commit, a manual tag or a resurrected history could
// still leak it into a later range — so the generator filters it defensively.
export function isChangelogCommit(subject) {
    return /^\*\s*update changelog for v\d{8}\b/.test(subject || '');
}

export function bucketize(commits) {
    const buckets = Object.fromEntries(BUCKET_ORDER.map(b => [b, []]));
    for (const c of commits) {
        const subject = (c && c.subject) || '';
        const hash    = (c && c.hash)    || '';
        const m = /^([+\-#!*])\s*(.+)$/.exec(subject);
        let label, text;
        if (m && PREFIX_TO_BUCKET[m[1]]) {
            label = PREFIX_TO_BUCKET[m[1]];
            text  = m[2].trim();
        } else {
            label = 'Other';
            text  = subject;
        }
        buckets[label].push({ hash, text });
    }
    return buckets;
}

export function renderSection(header, buckets) {
    const lines = [`## ${header}`, ''];
    let any = false;
    for (const name of BUCKET_ORDER) {
        const items = buckets[name];
        if (!items || items.length === 0) continue;
        any = true;
        lines.push(`### ${name}`);
        for (const it of items) {
            const ref = it.hash ? ` (${it.hash})` : '';
            lines.push(`- ${it.text}${ref}`);
        }
        lines.push('');
    }
    if (!any) lines.push('_No notable changes._', '');
    return lines.join('\n');
}

export function prependSection(existing, section) {
    const body = existing || '';
    const header = body.startsWith('# ') ? body.split('\n', 1)[0] + '\n\n' : '# Changelog\n\n';
    const rest   = body.startsWith('# ') ? body.split('\n').slice(1).join('\n').replace(/^\n*/, '') : body;
    return header + section.trimEnd() + '\n\n' + rest;
}

// ---------------------------------------------------------------------------
// Git helpers (used when invoked as a script)
// ---------------------------------------------------------------------------
function gitLastTag(matchPattern) {
    const args = ['describe', '--tags', '--abbrev=0'];
    if (matchPattern) args.push(`--match=${matchPattern}`);
    const r = spawnSync('git', args, { encoding: 'utf8' });
    if (r.status !== 0) return null;
    return (r.stdout || '').trim() || null;
}

function gitCommits(sinceRef) {
    const range = sinceRef ? `${sinceRef}..HEAD` : 'HEAD';
    const r = spawnSync('git', ['log', range, '--pretty=format:%h|%s'], { encoding: 'utf8' });
    if (r.status !== 0) return [];
    return (r.stdout || '').split('\n').filter(Boolean).map(line => {
        const idx = line.indexOf('|');
        if (idx < 0) return { hash: '', subject: line };
        return { hash: line.slice(0, idx), subject: line.slice(idx + 1) };
    });
}

function isoToday() {
    return new Date().toISOString().slice(0, 10);
}

// ---------------------------------------------------------------------------
// CLI entry
// ---------------------------------------------------------------------------
function main() {
    const argv = process.argv.slice(2);
    let mode = null;            // 'nightly' | 'release'
    let tag  = null;
    let version = null;
    let notesPath = null;       // --notes <file>: write the release-notes body here
    let notesOnly = false;      // --notes-only: leave CHANGELOG.md untouched

    for (let i = 0; i < argv.length; i++) {
        const a = argv[i];
        if (a === '--nightly') { mode = 'nightly'; }
        else if (a === '--release') { mode = 'release'; tag = argv[++i]; }
        else if (a === '--version') { version = argv[++i]; }
        else if (a === '--notes') { notesPath = argv[++i]; }
        else if (a === '--notes-only') { notesOnly = true; }
    }
    if (!mode) {
        console.error('usage: update-changelog.mjs --nightly | --release <tag> [--version X.Y.Z.B] [--notes <file>] [--notes-only]');
        process.exit(2);
    }

    // Releases measure from the last STABLE tag (v1.2.3 / vyyyyMMdd) so a
    // same-day nightly-* tag never swallows the release's commit range.
    // Nightlies keep the nearest tag of any kind: their notes are the delta
    // since the previous nightly (or stable, whichever is closer).
    const since   = gitLastTag(mode === 'release' ? 'v[0-9]*' : null);
    const commits = gitCommits(since).filter(c => !isChangelogCommit(c.subject));

    const header = mode === 'nightly'
        ? `Nightly ${isoToday()}${version ? ` (${version})` : ''}`
        : `${tag} (${isoToday()})`;

    // The functional changes only: commit subjects bucketed by + - * # ! prefix.
    const section = renderSection(header, bucketize(commits));

    // Release-notes body = the buckets without the leading "## <header>" line
    // (the GitHub release title already carries it). Written even when empty so
    // the workflow's body_path always resolves.
    if (notesPath) {
        const body = section.replace(/^##[^\n]*\n\n?/, '');
        fs.writeFileSync(notesPath, body.trimEnd() + '\n');
        console.log(`Wrote release notes to ${notesPath}.`);
    }

    if (notesOnly) {
        console.log('--notes-only: CHANGELOG.md left untouched.');
        return;
    }

    if (commits.length === 0) {
        console.log('No new commits since last tag -- CHANGELOG unchanged.');
        return;
    }

    const existing = fs.existsSync(CHANGELOG) ? fs.readFileSync(CHANGELOG, 'utf8') : '';
    fs.writeFileSync(CHANGELOG, prependSection(existing, section));
    console.log(`CHANGELOG updated with ${commits.length} commit(s) under "${header}".`);
}

// pathToFileURL handles Windows separators AND percent-encodes blanks, so the
// comparison also holds for working copies living in paths with spaces.
if (process.argv[1] && import.meta.url === url.pathToFileURL(process.argv[1]).href) {
    main();
}
