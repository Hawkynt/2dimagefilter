# CI/CD Pipeline — 2D Image Filter

> Everything in this folder is the automated pipeline. Workflows live here, scripts live in `scripts/`.

## Files

| File                            | Trigger                             | Purpose                                      |
|---------------------------------|-------------------------------------|----------------------------------------------|
| `generate.yml`                  | working-branch push + manual        | Regenerate and commit the GUI demo screenshot|
| `ci.yml`                        | push + PR + `workflow_call`         | Build and test the solution                  |
| `release.yml`                   | tag push `v*`                       | GitHub Release (GUI + plugin zips)           |
| `nightly.yml`                   | successful CI on `main`             | `nightly-YYYY-MM-DD` + GFS prune             |
| `_build.yml`                    | `workflow_call` (internal)          | .NET Framework publish + zip                 |
| `scripts/*`                     | invoked by workflows                | version/changelog/prune tools                |

## Why Windows-only

- **.NET Framework targets** require Windows and the .NET Framework targeting packs.
- **GUI screenshot generation** builds the real WinForms application, launches its internal `--screenshot` mode with deterministic pixel-art demo data, waits for the normal auto-preview, and renders the form through WinForms itself.
- **Generated files stay on working branches**: `Hawkynt/RepositoryTemplate/commit-generated-file@v1` writes the PNG back through GitHub's contents API, producing a signed commit and refusing to modify `main`.

## Release artifacts

| Artifact                                             | Produced by          | Runtime requirement          |
|------------------------------------------------------|----------------------|------------------------------|
| `ImageResizer-win-<version>.zip`                     | release + nightly    | .NET Framework 4.8           |
| `PixelArtScalingPlugin-win-<version>.zip`            | release + nightly    | .NET Framework 4.8           |
