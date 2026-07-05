<p align="center">
  <img src="https://raw.githubusercontent.com/ozakboy/Ozakboy.TaiwanKit.TwIdValidator/main/logo.png" width="128" alt="Ozakboy.TaiwanKit logo" />
</p>

# Ozakboy.TaiwanKit.TwIdValidator

[繁體中文](https://github.com/ozakboy/Ozakboy.TaiwanKit.TwIdValidator/blob/main/README_zh-TW.md)

Taiwan national ID, business administration number (BAN) and mobile phone number validator for .NET — with detailed parse results, input normalization and test-data generation. **Zero runtime dependencies.**

Part of the **Ozakboy.TaiwanKit** series.

## Features

- **National ID validation** — covers all three kinds of Taiwan ID numbers:
  - National ID for R.O.C. citizens (`A123456789`)
  - New-style UI number for foreign residents, issued since 2021 (`A800000014`)
  - Old-style ARC UI number, valid until 2031 (`AB12345678`)
- **Detailed parse results** — kind, gender and household region decoded from the number
- **BAN validation** — 8-digit business administration numbers, including the April 2021 checksum rule and the 7th-digit-7 special case
- **Mobile number validation** — strict `09xxxxxxxx` check, plus `TryNormalize` for `+886`, spaces, dashes, parentheses and full-width digits
- **Test-data generation** — produce checksum-valid sample numbers (reproducible with a seed)
- Thread-safe, allocation-light, no configuration required

## Install

```
dotnet add package Ozakboy.TaiwanKit.TwIdValidator
```

Supported frameworks: `netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`

## Quick start

```csharp
using Ozakboy.TaiwanKit.TwIdValidator;

// --- National ID (all three kinds auto-detected) ---
TwId.IsValid("A123456789");            // true
TwId.IsValid("A800000014");            // true  (new-style resident ID)
TwId.IsValid("AB12345677");            // true  (old-style resident ID)

TwIdResult r = TwId.Validate("A123456789");
// r.IsValid    → true
// r.Kind       → TwIdKind.NationalId
// r.Gender     → Gender.Male
// r.RegionName → "臺北市"
// r.Failure    → ValidationFailure.None

// --- BAN (business administration number) ---
TwBan.IsValid("04595257");             // true
TwBan.Validate("04595258").Failure;    // ValidationFailure.InvalidChecksum

// --- Mobile phone ---
TwPhone.IsValid("0912345678");         // true (strict)
TwPhone.TryNormalize("+886-912-345-678", out string n);  // true, n = "0912345678"

// --- Test data generation (format-valid only, not real persons/companies) ---
TwId.Generate(TwIdKind.NationalId, Gender.Female);  // e.g. "B234567891"
TwBan.Generate();                                    // e.g. "51233458"
TwPhone.Generate(seed: 42);                          // reproducible with a seed
```

## API overview

| Type | Member | Description |
|------|--------|-------------|
| `TwId` | `IsValid(string?)` | Validates any of the three ID kinds; `false` for null/empty, never throws |
| `TwId` | `Validate(string?)` | Returns `TwIdResult` with kind / gender / region / failure reason |
| `TwId` | `Generate(kind, gender?, seed?)` | Generates a checksum-valid sample ID |
| `TwBan` | `IsValid(string?)` / `Validate(string?)` | BAN validation (2021 rule included) |
| `TwBan` | `Generate(seed?)` | Generates a checksum-valid sample BAN |
| `TwPhone` | `IsValid(string?)` / `Validate(string?)` | Strict mobile number validation |
| `TwPhone` | `TryNormalize(string?, out string)` | Normalizes `+886`/separators/full-width digits, then validates |
| `TwPhone` | `Generate(seed?)` | Generates a format-valid sample number |

Behavioral notes:

- Validation methods **never throw** — invalid input (including `null`) simply returns `false` / a failure result.
- Input is trimmed and letters are upper-cased before validation.
- Obsolete region codes (L/R/S/Y) are still **accepted** in validation (historical IDs remain valid) and their region names are marked `（已停用）`; the generator only uses active codes.

> **Disclaimer**: generated numbers are *format-valid only*. They do not correspond to real persons or companies and must only be used as test data.

## Links

- Changelog: [docs/en/changelog.md](https://github.com/ozakboy/Ozakboy.TaiwanKit.TwIdValidator/blob/main/docs/en/changelog.md)
- License: MIT

## Ozakboy.TaiwanKit series

| Package | Description |
|---------|-------------|
| `Ozakboy.TaiwanKit.TwIdValidator` | Taiwan ID / BAN / mobile number validation (this package) |
| `Ozakboy.TaiwanKit.TaiwanHolidays` | Taiwan national holidays / make-up workday queries |
| `Ozakboy.TaiwanKit.Ganzhi.NET` | Lunar calendar / solar terms / Ganzhi conversion |
