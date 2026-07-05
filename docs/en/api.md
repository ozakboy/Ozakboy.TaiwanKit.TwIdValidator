# API Reference

## TwId — National ID / UI numbers

| Member | Returns | Description |
|--------|---------|-------------|
| `IsValid(string?)` | `bool` | Validates all three ID kinds (auto-detected); `false` for null/empty, never throws |
| `Validate(string?)` | `TwIdResult` | Detailed validation result |
| `Generate(TwIdKind, Gender?, int? seed)` | `string` | Generates a checksum-valid sample ID |

### TwIdResult

| Property | Type | Description |
|----------|------|-------------|
| `IsValid` | `bool` | Whether the number is valid |
| `Failure` | `ValidationFailure` | Failure reason; `None` when valid |
| `Kind` | `TwIdKind?` | Number kind; null when the format is unrecognizable |
| `Gender` | `Gender?` | Gender; null when unrecognizable |
| `RegionName` | `string?` | Household (first registration) region; obsolete codes marked "（已停用）" |

### TwIdKind

| Value | Description |
|-------|-------------|
| `NationalId` | National ID for citizens (2nd char 1/2) |
| `NewResidentId` | New-style UI number (2nd char 8/9, 2021+) |
| `OldResidentId` | Old-style ARC number (first 2 chars are letters, valid until 2031) |

## TwBan — Business administration number

| Member | Returns | Description |
|--------|---------|-------------|
| `IsValid(string?)` | `bool` | 8-digit BAN validation (2021 rule + 7th-digit-7 special case) |
| `Validate(string?)` | `BanResult` | Detailed result (`IsValid` + `Failure`) |
| `Generate(int? seed)` | `string` | Generates a checksum-valid sample BAN |

## TwPhone — Mobile phone number

| Member | Returns | Description |
|--------|---------|-------------|
| `IsValid(string?)` | `bool` | Strict validation: 10 digits starting with `09` |
| `Validate(string?)` | `PhoneResult` | Detailed result |
| `TryNormalize(string?, out string)` | `bool` | Normalizes `+886`/`886`, spaces, dashes, parentheses, dots and full-width digits, then validates |
| `Generate(int? seed)` | `string` | Generates a format-valid sample number |

## ValidationFailure

| Value | Description |
|-------|-------------|
| `None` | Validation passed |
| `NullOrEmpty` | Null, empty or whitespace-only input |
| `InvalidLength` | Wrong length |
| `InvalidFormat` | Character kind or position violates the rule |
| `InvalidChecksum` | Checksum mismatch |

## Exception behavior

Validation paths **never throw**. The only throwing case: `TwId.Generate` throws `ArgumentOutOfRangeException` for an undefined `TwIdKind`.
