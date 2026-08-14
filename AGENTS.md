# Agent guide — 2dimagefilter

Working agreement for **all** coding agents and human contributors working in
this repository. These rules are not optional. The full house spec lives in
the `Hawkynt/project-template` repo (`STANDARD.md`); this file is the
per-repo distillation.

## What this is

The flagship **pixel-art scaling collection**: dozens of upscalers (HQ, XBR,
Eagle, Scale…), resamplers, downsamplers, filters, quantizers and ditherers.
Solution `ImageResizer.slnx`: GUI app (`ImageResizer`), the algorithm library
(`ImageResizerLibrary`), `PixelArtScalingPlugin`, and the test projects under
`Tests/`. Filter names follow
the category-prefix scheme documented in the README — keep code, GUI
dropdown and README in sync.

## Commits

- **Group changes semantically/logically** — one algorithm/concern per
  commit.
- **Every subject line starts with a prefix**: `+` added · `-` removed ·
  `*` changed · `#` bug fixed · `!` critical todo.
- Never start a subject with "fix"/"bugfix"/"changed"/"modified".
- **No AI traces anywhere**: no `Co-Authored-By` AI lines, no "Generated
  with" footers, no agent mentions in messages, comments, or authorship.

## The loop (always, in this order)

1. **Before committing**: `dotnet build ImageResizer.slnx -c Release` and
   `dotnet test ImageResizer.slnx -c Release` until green; scaling changes get an eyeball comparison against the
   reference output of the affected algorithm — visual regressions don't
   show in unit tests.
2. **Commit** (rules above) and **push**.
3. **Wait for CI**; on `main` a green CI triggers the nightly (prerelease +
   GFS prune, same-day replace). Fix and loop until everything is green.

Stable releases are **manual** (`gh workflow run release.yml`) — never cut
one unless explicitly asked.

## Versioning

- Each project declares a **three-part base** in `<Version>` and nothing else.
  The fourth part (revision) belongs to CI: `version.pl --stamp` appends the
  commit count of the folder that declares the version.
- **Never** pin `<AssemblyVersion>` or `<FileVersion>` — they derive from
  `<Version>`, and pinning them silently overrides the stamp, which is how the
  binaries ended up reporting 1.1.3.3 while releases were named 1.1.3.21.
- Bump the base by hand only for a deliberate major/minor/patch release.

## Code conventions

- Latest C# features; pixel kernels are hot paths — measure before and
  after, never make a scaler slower.
- New algorithms: cite the source/paper in `## 🏆 Algorithm Credits`, follow
  the existing kernel patterns, and register under the correct category
  prefix (`Upscaler:`, `Resampler:`, `Downsampler:`, `Filter:`, …).

## README & repo conventions

- Standard frame: title → badges → one-line `>` blockquote (no Overview
  header); fixed emoji mapping for the standard sections (`## ✨ Features`,
  `## 📦 Installation & Usage`, `## ❤️ Support`, `## 📜 License`);
  repo-specific sections keep their consistent topical emojis.
- License is LGPL-3.0-or-later; the `## ❤️ Support` section and
  `.github/FUNDING.yml` stay intact.
