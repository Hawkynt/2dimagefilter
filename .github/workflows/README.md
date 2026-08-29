# CI/CD Pipeline — 2D Image Filter

> Everything in this folder is the automated pipeline. Workflows live here, scripts live in `scripts/`.

## Files

| File                            | Trigger                             | Purpose                                      |
|---------------------------------|-------------------------------------|----------------------------------------------|
| `ci.yml`                        | push + PR + `workflow_call`         | Build and test the solution                  |
| `screenshots.yml`               | push + `workflow_dispatch`          | Regenerate and commit the GUI demo screenshot|
| `release.yml`                   | tag push `v*`                       | GitHub Release (GUI + plugin zips)           |
| `nightly.yml`                   | CI success on `master`              | `nightly-YYYY-MM-DD` + GFS prune             |
| `_build.yml`                    | `workflow_call` (internal)          | .NET Framework publish + zip                 |
| `scripts/*`                     | invoked by workflows                | version/changelog/prune/screenshot tools     |

## Why Windows-only

- **.NET Framework targets**: require Windows and the .NET Framework targeting packs.
- **GUI screenshot**: launches the WinForms application with deterministic generated pixel-art demo data, waits for its auto-preview, captures the real application window, and commits only when the rendered pixels changed.
- **Version source**: `<Version>1.1.3</Version>` in `ImageResizerLibrary/ImageResizerLibrary.csproj` (the other csprojs use only `<AssemblyVersion>`).

## Release artifacts

| Artifact                                             | Produced by          | Runtime requirement          |
|------------------------------------------------------|----------------------|------------------------------|
| `ImageResizer-win-<version>.zip`                     | release + nightly    | .NET Framework 4.5 + WPF     |
| `PixelArtScalingPlugin-win-<version>.zip`            | release + nightly    | .NET Framework 4.7           |
