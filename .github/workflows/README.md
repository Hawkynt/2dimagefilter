# CI/CD Pipeline — 2D Image Filter

> Everything in this folder is the automated pipeline. Workflows live here, scripts live in `scripts/`.

## Files

| File                            | Trigger                             | Purpose                                      |
|---------------------------------|-------------------------------------|----------------------------------------------|
| `generate.yml`                  | working-branch push + manual        | Regenerate and commit documentation screenshots|
| `ci.yml`                        | push + PR + `workflow_call`         | Build and test the solution                  |
| `release.yml`                   | tag push `v*`                       | GitHub Release (GUI + plugin zips)           |
| `nightly.yml`                   | successful CI on `main`             | `nightly-YYYY-MM-DD` + GFS prune             |
| `_build.yml`                    | `workflow_call` (internal)          | .NET Framework publish + zip                 |
| `scripts/*`                     | invoked by workflows                | version/changelog/prune tools                |

## Why Windows-only

- **.NET Framework targets** require Windows and the .NET Framework targeting packs.
- **GUI screenshot generation** builds the real WinForms application and runs its internal `--screenshots` mode. That mode creates a deterministic pixel-art source for the scaler window and a separate high-colour test card for the quantization/dithering dialog, then waits for the corresponding live previews before rendering each form through WinForms itself.
- **Debuggable generated inputs**: both demo source PNGs and both resulting window captures are uploaded as the `generated-screenshots` workflow artifact. The demo inputs are not documentation assets; they exist so a failed or visually suspicious generation run can be inspected directly.
- **Generated documentation stays on working branches**: `Hawkynt/RepositoryTemplate/commit-generated-file@v1` writes the two window captures back through GitHub's contents API, producing signed commits and refusing to modify `main`.

## Generated screenshots

| File | Demonstrates |
|------|--------------|
| `docs/screenshots/image-resizer.png` | Main application with deterministic pixel art and live HQ 2x output |
| `docs/screenshots/reduce-colours.png` | Reduce Colours dialog with histogram, quantizer/ditherer thumbnails, and a selected 16-colour Median Cut + Floyd–Steinberg detail preview |

## Release artifacts

| Artifact                                             | Produced by          | Runtime requirement          |
|------------------------------------------------------|----------------------|------------------------------|
| `ImageResizer-win-<version>.zip`                     | release + nightly    | .NET Framework 4.8           |
| `PixelArtScalingPlugin-win-<version>.zip`            | release + nightly    | .NET Framework 4.8           |
