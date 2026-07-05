# Getting Started

## Install

```bash
dotnet add package Ozakboy.TaiwanKit.TwIdValidator
```

Supported frameworks: `netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`, zero runtime dependencies.

## National ID validation (all three kinds auto-detected)

```csharp
using Ozakboy.TaiwanKit.TwIdValidator;

TwId.IsValid("A123456789");   // true  national ID
TwId.IsValid("A800000014");   // true  new-style UI number (2021+)
TwId.IsValid("AB12345677");   // true  old-style ARC number (valid until 2031)
```

Use `Validate` when you need details:

```csharp
TwIdResult r = TwId.Validate("A123456789");
// r.IsValid    → true
// r.Kind       → TwIdKind.NationalId
// r.Gender     → Gender.Male
// r.RegionName → "臺北市"
// r.Failure    → ValidationFailure.None
```

On failure, `Failure` tells you why (`NullOrEmpty` / `InvalidLength` / `InvalidFormat` / `InvalidChecksum`). When the format is recognizable but the checksum fails, `Kind` / `Gender` / `RegionName` are still populated — handy for "right format, wrong number" hints.

## BAN validation

```csharp
TwBan.IsValid("04595257");            // true
TwBan.Validate("04595258").Failure;   // ValidationFailure.InvalidChecksum
```

The April 2021 rule (weighted sum divisible by 5) and the 7th-digit-7 special case are built in.

## Mobile number validation & normalization

```csharp
TwPhone.IsValid("0912345678");   // true (strict: 10 digits starting with 09)

// Real-world input often has country codes and separators — use TryNormalize
TwPhone.TryNormalize("+886-912-345-678", out string n);   // true, n = "0912345678"
TwPhone.TryNormalize("０９１２３４５６７８", out n);        // true (full-width digits too)
```

## Test-data generation

```csharp
TwId.Generate(TwIdKind.NationalId, Gender.Female);   // e.g. "B234567891"
TwId.Generate(TwIdKind.OldResidentId, seed: 42);     // reproducible with a seed
TwBan.Generate();                                     // e.g. "51233458"
TwPhone.Generate();                                   // e.g. "0987654321"
```

> ⚠ Generated numbers are **format-valid only** — they never correspond to real persons or companies. Test data only.

## Behavioral contract

- Validation methods **never throw** — invalid input (incl. `null`) returns `false` / a failure result
- Input is trimmed and upper-cased automatically
- Obsolete region codes (L/R/S/Y) are still accepted (historical IDs remain valid), with `RegionName` marked "（已停用）"; the generator only uses active codes
- Pure functions, thread-safe — use freely in any multi-threaded scenario
