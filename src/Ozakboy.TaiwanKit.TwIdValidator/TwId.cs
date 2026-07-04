using System;
using Ozakboy.TaiwanKit.TwIdValidator.Core;

namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 台灣身分證/統一證號驗證（涵蓋本國人身分證、新式外來人口統一證號、舊式居留證號三種證號）
    /// Taiwan ID number validator (covers national ID, new-style UI number and old-style ARC UI number)
    /// </summary>
    public static class TwId
    {
        /// <summary>
        /// 驗證證號是否有效（自動辨識三種證號類型）。null/空字串回傳 false，不拋例外。
        /// Validates the ID number (auto-detects all three kinds). Returns false for null/empty input; never throws.
        /// </summary>
        /// <param name="input">證號（容忍前後空白與小寫） / ID number (leading/trailing whitespace and lowercase tolerated)</param>
        /// <returns>是否有效 / Whether valid</returns>
        public static bool IsValid(string? input)
        {
            return Validate(input).IsValid;
        }

        /// <summary>
        /// 驗證證號並回傳詳細結果（證號類型、性別、戶籍縣市、失敗原因）。null/空字串回傳失敗結果，不拋例外。
        /// Validates the ID number and returns a detailed result (kind, gender, region, failure reason). Never throws.
        /// </summary>
        /// <param name="input">證號（容忍前後空白與小寫） / ID number (leading/trailing whitespace and lowercase tolerated)</param>
        /// <returns>詳細驗證結果 / Detailed validation result</returns>
        public static TwIdResult Validate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new TwIdResult(false, ValidationFailure.NullOrEmpty, null, null, null);
            }

            string id = input!.Trim().ToUpperInvariant();

            if (id.Length != 10)
            {
                return new TwIdResult(false, ValidationFailure.InvalidLength, null, null, null);
            }

            if (id[0] < 'A' || id[0] > 'Z')
            {
                return new TwIdResult(false, ValidationFailure.InvalidFormat, null, null, null);
            }

            // 依第 2 碼辨識證號類型
            // Detect the kind by the 2nd character
            TwIdKind kind;
            Gender gender;
            bool isOldResident = false;
            char second = id[1];

            if (second >= 'A' && second <= 'D')
            {
                kind = TwIdKind.OldResidentId;
                gender = (second == 'A' || second == 'C') ? Gender.Male : Gender.Female;
                isOldResident = true;
            }
            else if (second == '1' || second == '2')
            {
                kind = TwIdKind.NationalId;
                gender = second == '1' ? Gender.Male : Gender.Female;
            }
            else if (second == '8' || second == '9')
            {
                kind = TwIdKind.NewResidentId;
                gender = second == '8' ? Gender.Male : Gender.Female;
            }
            else
            {
                return new TwIdResult(false, ValidationFailure.InvalidFormat, null, null, null);
            }

            // 其餘位數必須為數字
            // Remaining characters must be digits
            for (int i = 2; i < 10; i++)
            {
                if (id[i] < '0' || id[i] > '9')
                {
                    return new TwIdResult(false, ValidationFailure.InvalidFormat, null, null, null);
                }
            }

            string? regionName = TwIdAlgorithm.GetRegionName(id[0]);

            if (TwIdAlgorithm.ComputeWeightedSum(id, isOldResident) % 10 != 0)
            {
                return new TwIdResult(false, ValidationFailure.InvalidChecksum, kind, gender, regionName);
            }

            return new TwIdResult(true, ValidationFailure.None, kind, gender, regionName);
        }

        /// <summary>
        /// 產生檢核碼合法的測試用證號。產生的號碼僅格式合法，不對應真實人物。
        /// Generates a checksum-valid ID number for testing. Generated numbers are format-valid only and do not correspond to real persons.
        /// </summary>
        /// <param name="kind">證號類型，預設本國人身分證 / Kind of the number, defaults to national ID</param>
        /// <param name="gender">性別；null 表示隨機 / Gender; null means random</param>
        /// <param name="seed">亂數種子；指定後產生結果可重現 / Random seed; results are reproducible when specified</param>
        /// <returns>檢核碼合法的證號 / A checksum-valid ID number</returns>
        /// <exception cref="ArgumentOutOfRangeException">kind 不是已定義的證號類型 / kind is not a defined value</exception>
        public static string Generate(TwIdKind kind = TwIdKind.NationalId, Gender? gender = null, int? seed = null)
        {
            if (kind != TwIdKind.NationalId && kind != TwIdKind.NewResidentId && kind != TwIdKind.OldResidentId)
            {
                throw new ArgumentOutOfRangeException(nameof(kind));
            }

            Random random = seed.HasValue ? new Random(seed.Value) : new Random();
            Gender actualGender = gender ?? (random.Next(2) == 0 ? Gender.Male : Gender.Female);

            char[] letters = TwIdAlgorithm.ActiveRegionLetters;
            char first = letters[random.Next(letters.Length)];

            char second;
            bool isOldResident = false;
            switch (kind)
            {
                case TwIdKind.NationalId:
                    second = actualGender == Gender.Male ? '1' : '2';
                    break;
                case TwIdKind.NewResidentId:
                    second = actualGender == Gender.Male ? '8' : '9';
                    break;
                default:
                    // 舊式居留證：男性 A/C、女性 B/D
                    // Old-style resident ID: A/C for male, B/D for female
                    isOldResident = true;
                    if (actualGender == Gender.Male)
                    {
                        second = random.Next(2) == 0 ? 'A' : 'C';
                    }
                    else
                    {
                        second = random.Next(2) == 0 ? 'B' : 'D';
                    }

                    break;
            }

            char[] buffer = new char[9];
            buffer[0] = first;
            buffer[1] = second;
            for (int i = 2; i < 9; i++)
            {
                buffer[i] = (char)('0' + random.Next(10));
            }

            string withoutCheck = new string(buffer);
            int checkDigit = TwIdAlgorithm.ComputeCheckDigit(withoutCheck, isOldResident);
            return withoutCheck + (char)('0' + checkDigit);
        }
    }
}
