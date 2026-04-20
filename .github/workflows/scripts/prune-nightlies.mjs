// Grandfather-Father-Son pruning for nightly releases.
//
// Keep:
//   * the 7 most recent daily nightly-YYYY-MM-DD releases
//   * the newest release from each of the 4 most recent ISO-weeks
//   * the newest release from each of the 3 most recent calendar months
//
// Everything else is deleted (release + tag) via the `gh` CLI.
//
// Dry-run locally with `node .github/workflows/scripts/prune-nightlies.mjs --dry-run`.

import { spawnSync } from 'node:child_process';

export const DAILY_KEEP   = 7;
export const WEEKLY_KEEP  = 4;
export const MONTHLY_KEEP = 3;

const DRY = process.argv.includes('--dry-run');

function gh(args, opts = {}) {
    const r = spawnSync('gh', args, { encoding: 'utf8', ...opts });
    if (r.status !== 0) throw new Error(`gh ${args.join(' ')} failed: ${r.stderr || r.stdout}`);
    return r.stdout;
}

function listNightlies() {
    const out = gh(['release', 'list', '--limit', '200', '--json', 'tagName,createdAt,isPrerelease']);
    let all;
    try { all = JSON.parse(out); }
    catch (e) {
        console.error(`warning: unparseable gh output (${e.message}); skipping prune.`);
        return [];
    }
    if (!Array.isArray(all)) return [];
    return parseNightlies(all);
}

/** Exported so tests can exercise it against synthetic input. Silently drops
 *  entries that don't match the nightly-YYYY-MM-DD tag shape or that throw
 *  while constructing their Date. */
export function parseNightlies(raw) {
    if (!Array.isArray(raw)) return [];
    const re = /^nightly-(\d{4})-(\d{2})-(\d{2})$/;
    const out = [];
    for (const r of raw) {
        try {
            if (!r || typeof r.tagName !== 'string') continue;
            const m = re.exec(r.tagName);
            if (!m) continue;
            const date = new Date(`${m[1]}-${m[2]}-${m[3]}T00:00:00Z`);
            if (isNaN(date.getTime())) continue;
            out.push({
                tag:  r.tagName,
                date,
                iso:  `${m[1]}-${m[2]}-${m[3]}`,
            });
        } catch { /* skip this entry */ }
    }
    out.sort((a, b) => b.date.getTime() - a.date.getTime());
    return out;
}

export function isoWeekKey(d) {
    // ISO-8601 week number.
    const t = new Date(Date.UTC(d.getUTCFullYear(), d.getUTCMonth(), d.getUTCDate()));
    const dayNum = (t.getUTCDay() + 6) % 7;                // Mon=0
    t.setUTCDate(t.getUTCDate() - dayNum + 3);             // Thursday of same week
    const firstThu = new Date(Date.UTC(t.getUTCFullYear(), 0, 4));
    const week = 1 + Math.round(((t - firstThu) / 86400000 - 3 + ((firstThu.getUTCDay() + 6) % 7)) / 7);
    return `${t.getUTCFullYear()}-W${String(week).padStart(2, '0')}`;
}

/**
 * Promotion-based Grandfather-Father-Son retention.
 *
 *   son         = the N newest releases (a rolling window of nightly
 *                 activity; the dates do not need to be consecutive).
 *   father      = from releases OLDER than the son window, one representative
 *                 per distinct ISO-week, capped at N weeks. Weeks that son
 *                 already covers are skipped so a sparse father tier still
 *                 reaches further into the past instead of wasting slots.
 *   grandfather = from releases OLDER than everything son and father claimed,
 *                 one representative per distinct calendar month, capped at
 *                 N months. Months that son or father touched are skipped.
 *
 * Each release ends up in at most one tier and each time-bucket is claimed
 * by at most one tier, so gaps in activity never waste retention slots.
 *
 * `releases` must be sorted newest-first (parseNightlies does that).
 */
export function planRetention(releases, opts = {}) {
    const dailyN   = opts.daily   ?? DAILY_KEEP;
    const weeklyN  = opts.weekly  ?? WEEKLY_KEEP;
    const monthlyN = opts.monthly ?? MONTHLY_KEEP;
    const keep     = new Set();

    // --- Son: N newest releases -----------------------------------------
    const sonSlice = releases.slice(0, dailyN);
    for (const r of sonSlice) keep.add(r.tag);
    const sonWeeks  = new Set(sonSlice.map(r => isoWeekKey(r.date)));
    const sonMonths = new Set(sonSlice.map(r => r.iso.slice(0, 7)));

    // --- Father: older, newest-per-week, skipping weeks son owns ---------
    const fatherPicks = [];
    const seenWeeks   = new Set(sonWeeks);
    for (const r of releases.slice(dailyN)) {
        if (fatherPicks.length >= weeklyN) break;
        const w = isoWeekKey(r.date);
        if (seenWeeks.has(w)) continue;
        seenWeeks.add(w);
        fatherPicks.push(r);
    }
    for (const r of fatherPicks) keep.add(r.tag);
    const fatherMonths = new Set(fatherPicks.map(r => r.iso.slice(0, 7)));

    // --- Grandfather: older still, newest-per-month, skipping months
    //                  already claimed by son or father -------------------
    const touchedMonths = new Set([...sonMonths, ...fatherMonths]);
    const grandfatherPicks = [];
    const seenMonths = new Set(touchedMonths);
    for (const r of releases) {
        if (keep.has(r.tag)) continue;                      // already kept
        if (grandfatherPicks.length >= monthlyN) break;
        const m = r.iso.slice(0, 7);
        if (seenMonths.has(m)) continue;
        seenMonths.add(m);
        grandfatherPicks.push(r);
    }
    for (const r of grandfatherPicks) keep.add(r.tag);

    return {
        keep: releases.filter(r =>  keep.has(r.tag)),
        drop: releases.filter(r => !keep.has(r.tag)),
    };
}

// --- Entry point -------------------------------------------------------------
// Skipped when imported as a module (for tests).
if (import.meta.url === `file://${process.argv[1]}` || import.meta.url.endsWith(process.argv[1]?.replace(/\\/g,'/'))) {
    main();
}
function main() {

const releases = listNightlies();
console.log(`Found ${releases.length} nightly release(s).`);

const { keep, drop } = planRetention(releases);

console.log(`Keeping ${keep.length}:`);
for (const r of keep) console.log(`  + ${r.tag}`);

console.log(`Deleting ${drop.length}:`);
for (const r of drop) console.log(`  - ${r.tag}`);

if (DRY) {
    console.log('Dry run -- no changes applied.');
    process.exit(0);
}

for (const r of drop) {
    try {
        gh(['release', 'delete', r.tag, '--cleanup-tag', '--yes']);
        console.log(`deleted ${r.tag}`);
    } catch (e) {
        console.error(`failed to delete ${r.tag}: ${e.message}`);
    }
}
}
