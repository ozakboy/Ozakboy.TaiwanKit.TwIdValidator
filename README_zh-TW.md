<p align="center">
  <img src="https://raw.githubusercontent.com/ozakboy/Ozakboy.TaiwanKit.TwIdValidator/main/logo.png" width="128" alt="Ozakboy.TaiwanKit logo" />
</p>

# Ozakboy.TaiwanKit.TwIdValidator

[English](https://github.com/ozakboy/Ozakboy.TaiwanKit.TwIdValidator/blob/main/README.md)

.NET 台灣身分證/統一編號/手機號碼驗證套件——附詳細解析結果、輸入正規化與測試假資料產生。**零執行期依賴。**

**Ozakboy.TaiwanKit** 系列套件之一。

## 功能特色

- **身分證驗證** — 涵蓋台灣三種證號：
  - 本國人身分證字號（`A123456789`）
  - 新式外來人口統一證號，2021 年起發行（`A800000014`）
  - 舊式外僑居留證統一證號，2031 年前仍有效（`AB12345678`）
- **詳細解析結果** — 從證號解出證號類型、性別、戶籍（初次登記）縣市
- **統一編號驗證** — 8 碼統編，含財政部 2021 年 4 月新制檢核與第 7 碼為 7 的特例
- **手機號碼驗證** — 嚴格 `09xxxxxxxx` 檢查；另提供 `TryNormalize` 處理 `+886`、空格、dash、括號與全形數字
- **測試假資料產生** — 產生檢核碼合法的樣本號碼（指定 seed 可重現）
- 線程安全、低配置、免設定即用

## 安裝

```
dotnet add package Ozakboy.TaiwanKit.TwIdValidator
```

支援框架：`netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`

## 快速開始

```csharp
using Ozakboy.TaiwanKit.TwIdValidator;

// --- 身分證（三種證號自動辨識） ---
TwId.IsValid("A123456789");            // true
TwId.IsValid("A800000014");            // true  （新式統一證號）
TwId.IsValid("AB12345677");            // true  （舊式居留證號）

TwIdResult r = TwId.Validate("A123456789");
// r.IsValid    → true
// r.Kind       → TwIdKind.NationalId
// r.Gender     → Gender.Male
// r.RegionName → "臺北市"
// r.Failure    → ValidationFailure.None

// --- 統一編號 ---
TwBan.IsValid("04595257");             // true
TwBan.Validate("04595258").Failure;    // ValidationFailure.InvalidChecksum

// --- 手機號碼 ---
TwPhone.IsValid("0912345678");         // true（嚴格模式）
TwPhone.TryNormalize("+886-912-345-678", out string n);  // true，n = "0912345678"

// --- 測試假資料（僅格式合法，不對應真實人物/公司） ---
TwId.Generate(TwIdKind.NationalId, Gender.Female);  // 例如 "B234567891"
TwBan.Generate();                                    // 例如 "51233458"
TwPhone.Generate(seed: 42);                          // 指定 seed 可重現
```

## API 總覽

| 類別 | 成員 | 說明 |
|------|------|------|
| `TwId` | `IsValid(string?)` | 驗證三種證號（自動辨識）；null/空字串回 `false`，不拋例外 |
| `TwId` | `Validate(string?)` | 回傳 `TwIdResult`：證號類型/性別/縣市/失敗原因 |
| `TwId` | `Generate(kind, gender?, seed?)` | 產生檢核碼合法的樣本證號 |
| `TwBan` | `IsValid(string?)` / `Validate(string?)` | 統編驗證（含 2021 新制） |
| `TwBan` | `Generate(seed?)` | 產生檢核合法的樣本統編 |
| `TwPhone` | `IsValid(string?)` / `Validate(string?)` | 手機號碼嚴格驗證 |
| `TwPhone` | `TryNormalize(string?, out string)` | 正規化 `+886`/分隔符/全形數字後驗證 |
| `TwPhone` | `Generate(seed?)` | 產生格式合法的樣本號碼 |

行為說明：

- 驗證方法**一律不拋例外**——無效輸入（含 `null`）回傳 `false` / 失敗結果。
- 驗證前自動去除前後空白、英文轉大寫。
- 已停用縣市代碼（L/R/S/Y）驗證時**仍接受**（歷史證號仍有效），縣市名稱標注「（已停用）」；產生器只使用現行代碼。

> **聲明**：產生的號碼*僅格式合法*，不對應任何真實人物或公司，僅供測試資料使用。

## 連結

- 更新紀錄：[docs/zh-TW/changelog.md](https://github.com/ozakboy/Ozakboy.TaiwanKit.TwIdValidator/blob/main/docs/zh-TW/changelog.md)
- 授權：MIT

## Ozakboy.TaiwanKit 系列

| 套件 | 說明 |
|------|------|
| `Ozakboy.TaiwanKit.TwIdValidator` | 台灣身分證/統編/手機驗證（本套件） |
| `Ozakboy.TaiwanKit.TaiwanHolidays` | 台灣國定假日/補班日查詢 |
| `Ozakboy.TaiwanKit.Ganzhi.NET` | 農曆/節氣/干支轉換 |
