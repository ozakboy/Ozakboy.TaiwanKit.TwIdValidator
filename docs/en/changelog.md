# Changelog

All notable changes to **Ozakboy.TaiwanKit.TwIdValidator** are documented here.

## [1.0.1] - 2026-07-05

### Improved

- Added the Ozakboy.TaiwanKit series logo as the NuGet package icon and README header

## [1.0.0] - 2026-07-04

### Added

- Taiwan national ID validation covering all three kinds with auto-detection:
  - National ID for R.O.C. citizens (2nd character 1/2)
  - New-style UI number for foreign residents issued since 2021 (2nd character 8/9)
  - Old-style ARC UI number (first two characters are letters)
- Detailed validation results (`TwIdResult`): number kind, gender, household region name (obsolete region codes marked), failure reason
- BAN (business administration number) validation with the April 2021 checksum rule (multiple of 5) and the 7th-digit-7 special case
- Mobile phone number validation (strict `09xxxxxxxx`) and `TryNormalize` for `+886`/`886` country codes, spaces, dashes, parentheses, dots and full-width digits
- Test-data generators for all three number types (`Generate`), producing checksum-valid samples; reproducible via optional seed
- Bilingual (Traditional Chinese / English) XML documentation for the whole public API

### Technical

- Target frameworks: `netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`
- Zero runtime dependencies; thread-safe pure functions
- Deterministic build, SourceLink, symbol package (`snupkg`), XML docs included
