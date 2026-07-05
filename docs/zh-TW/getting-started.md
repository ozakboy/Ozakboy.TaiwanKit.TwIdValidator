# 快速開始

## 安裝

```bash
dotnet add package Ozakboy.TaiwanKit.TwIdValidator
```

支援框架：`netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`，零執行期依賴。

## 身分證驗證（三種證號自動辨識）

```csharp
using Ozakboy.TaiwanKit.TwIdValidator;

TwId.IsValid("A123456789");   // true  本國人身分證
TwId.IsValid("A800000014");   // true  新式外來人口統一證號（2021+）
TwId.IsValid("AB12345677");   // true  舊式居留證號（2031 年前有效）
```

需要細節時用 `Validate` 拿完整結果：

```csharp
TwIdResult r = TwId.Validate("A123456789");
// r.IsValid    → true
// r.Kind       → TwIdKind.NationalId
// r.Gender     → Gender.Male
// r.RegionName → "臺北市"
// r.Failure    → ValidationFailure.None
```

驗證失敗時 `Failure` 會告訴你原因（`NullOrEmpty` / `InvalidLength` / `InvalidFormat` / `InvalidChecksum`）；格式可辨識但檢核碼錯誤時，`Kind` / `Gender` / `RegionName` 仍會回填，方便提示使用者「格式對但號碼錯」。

## 統一編號驗證

```csharp
TwBan.IsValid("04595257");            // true
TwBan.Validate("04595258").Failure;   // ValidationFailure.InvalidChecksum
```

已內建財政部 2021 年 4 月新制（加權總和為 5 的倍數）與第 7 碼為 7 的特例。

## 手機號碼驗證與正規化

```csharp
TwPhone.IsValid("0912345678");   // true（嚴格模式：09 開頭 10 碼純數字）

// 使用者輸入常帶國碼與分隔符——用 TryNormalize
TwPhone.TryNormalize("+886-912-345-678", out string n);   // true, n = "0912345678"
TwPhone.TryNormalize("０９１２３４５６７８", out n);        // true（全形數字也可）
```

## 測試假資料產生

```csharp
TwId.Generate(TwIdKind.NationalId, Gender.Female);   // 例如 "B234567891"
TwId.Generate(TwIdKind.OldResidentId, seed: 42);     // 指定 seed 可重現
TwBan.Generate();                                     // 例如 "51233458"
TwPhone.Generate();                                   // 例如 "0987654321"
```

> ⚠ 產生的號碼**僅格式合法**，不對應真實人物或公司，僅供測試資料使用。

## 行為約定

- 驗證方法**一律不拋例外**——無效輸入（含 `null`）回傳 `false` / 失敗結果
- 輸入自動去除前後空白、英文轉大寫
- 已停用縣市代碼（L/R/S/Y）驗證時仍接受（歷史證號仍有效），`RegionName` 附「（已停用）」標注；產生器只使用現行代碼
- 純函式、線程安全，可在任何多執行緒場景直接使用
