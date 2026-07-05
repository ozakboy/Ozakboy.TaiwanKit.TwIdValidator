# API 參考

## TwId — 身分證/統一證號

| 成員 | 回傳 | 說明 |
|------|------|------|
| `IsValid(string?)` | `bool` | 驗證三種證號（自動辨識）；null/空字串回 false，不拋例外 |
| `Validate(string?)` | `TwIdResult` | 詳細驗證結果 |
| `Generate(TwIdKind, Gender?, int? seed)` | `string` | 產生檢核碼合法的樣本證號 |

### TwIdResult

| 屬性 | 型別 | 說明 |
|------|------|------|
| `IsValid` | `bool` | 是否有效 |
| `Failure` | `ValidationFailure` | 失敗原因；通過時為 `None` |
| `Kind` | `TwIdKind?` | 證號類型；格式無法辨識時為 null |
| `Gender` | `Gender?` | 性別；格式無法辨識時為 null |
| `RegionName` | `string?` | 戶籍（初次登記）縣市；已停用代碼附「（已停用）」 |

### TwIdKind

| 值 | 說明 |
|-----|------|
| `NationalId` | 本國人身分證（第 2 碼 1/2） |
| `NewResidentId` | 新式外來人口統一證號（第 2 碼 8/9，2021+） |
| `OldResidentId` | 舊式居留證號（前 2 碼英文，2031 年前有效） |

## TwBan — 統一編號

| 成員 | 回傳 | 說明 |
|------|------|------|
| `IsValid(string?)` | `bool` | 8 碼統編驗證（含 2021 新制與第 7 碼 = 7 特例） |
| `Validate(string?)` | `BanResult` | 詳細結果（`IsValid` + `Failure`） |
| `Generate(int? seed)` | `string` | 產生檢核合法的樣本統編 |

## TwPhone — 手機號碼

| 成員 | 回傳 | 說明 |
|------|------|------|
| `IsValid(string?)` | `bool` | 嚴格驗證：`09` 開頭 10 碼純數字 |
| `Validate(string?)` | `PhoneResult` | 詳細結果 |
| `TryNormalize(string?, out string)` | `bool` | 正規化 `+886`/`886` 國碼、空格、dash、括號、句點、全形數字後驗證 |
| `Generate(int? seed)` | `string` | 產生格式合法的樣本號碼 |

## ValidationFailure

| 值 | 說明 |
|-----|------|
| `None` | 驗證通過 |
| `NullOrEmpty` | null、空字串或全空白 |
| `InvalidLength` | 長度錯誤 |
| `InvalidFormat` | 字元種類或位置不符規則 |
| `InvalidChecksum` | 檢核碼錯誤 |

## 例外行為

驗證路徑**一律不拋例外**。唯一會拋例外的情況：`TwId.Generate` 收到未定義的 `TwIdKind` 時拋 `ArgumentOutOfRangeException`。
