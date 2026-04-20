# CI/CD Pipeline — 2D Image Filter

> Everything in this folder is the automated pipeline. Workflows live here, scripts live in `scripts/`.

## Files

| File                            | Trigger                             | Purpose                                 |
|---------------------------------|-------------------------------------|-----------------------------------------|
| `ci.yml`                        | push + PR + `workflow_call`         | Build-only (no tests in repo)           |
| `release.yml`                   | tag push `v*`                       | GitHub Release (GUI + plugin zips)      |
| `nightly.yml`                   | CI success on `master`              | `nightly-YYYY-MM-DD` + GFS prune        |
| `_build.yml`                    | `workflow_call` (internal)          | .NET Framework publish + zip            |
| `scripts/*`                     | invoked by workflows                | version/changelog/prune tools           |

## Why windows-only, build-only

- **net45 + net47 targets**: require Windows and the .NET Framework targeting packs.
- **No test projects in this repo**: CI's only signal is whether the build succeeds.
- **Version source**: `<Version>1.1.3</Version>` in `ImageResizerLibrary/ImageResizerLibrary.csproj` (the other csprojs use only `<AssemblyVersion>`).

## Release artifacts

| Artifact                                             | Produced by          | Runtime requirement          |
|------------------------------------------------------|----------------------|------------------------------|
| `ImageResizer-win-<version>.zip`                     | release + nightly    | .NET Framework 4.5 + WPF     |
| `PixelArtScalingPlugin-win-<version>.zip`            | release + nightly    | .NET Framework 4.7           |
