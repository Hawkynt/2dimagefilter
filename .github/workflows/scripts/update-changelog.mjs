// Maintains CHANGELOG.md. Invoked from the nightly and release workflows.
//
// Usage:
//   node .github/workflows/scripts/update-changelog.mjs --nightly          # "## Nightly YYYY-MM-DD (<version>)"
//   node .github/workflows/scripts/update-changelog.mjs --release v1.2.3   # "## v1.2.3 (YYYY-MM-DD)"
//
// Commit subject conventions (see bucketize() below):
//   + Added  * Changed  # Fixed  - Removed  ! TODO
// Anything else goes into "Other".

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
export const BUCKET_ORDER = ['Added', 'Changed', 'Fixed', 'Removed', 'TODO', 'Other'];
const PREFIX_TO_BUCKET = {
    '+': 'Added',
    '*': 'Changed',
    '#': 'Fixed',
    '-': 'Removed',
    '!': 'TODO',
};

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
function gitLastTag() {
    const r = spawnSync('git', ['describe', '--tags', '--abbrev=0'], { encoding: 'utf8' });
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

    for (let i = 0; i < argv.length; i++) {
        const a = argv[i];
        if (a === '--nightly') { mode = 'nightly'; }
        else if (a === '--release') { mode = 'release'; tag = argv[++i]; }
        else if (a === '--version') { version = argv[++i]; }
    }
    if (!mode) {
        console.error('usage: update-changelog.mjs --nightly | --release <tag> [--version X.Y.Z.B]');
        process.exit(2);
    }

    const since = gitLastTag();
    const commits = gitCommits(since);
    if (commits.length === 0) {
        console.log('No new commits since last tag -- CHANGELOG unchanged.');
        return;
    }

    let header;
    if (mode === 'nightly') {
        const suffix = version ? ` (${version})` : '';
        header = `Nightly ${isoToday()}${suffix}`;
    } else {
        header = `${tag} (${isoToday()})`;
    }

    const section = renderSection(header, bucketize(commits));
    const existing = fs.existsSync(CHANGELOG) ? fs.readFileSync(CHANGELOG, 'utf8') : '';
    const next = prependSection(existing, section);
    fs.writeFileSync(CHANGELOG, next);
    console.log(`CHANGELOG updated with ${commits.length} commit(s) under "${header}".`);
}

const invokedPath = process.argv[1] ? process.argv[1].replace(/\\/g, '/') : '';
if (import.meta.url === `file://${invokedPath}` || import.meta.url.endsWith(invokedPath)) {
    main();
}
