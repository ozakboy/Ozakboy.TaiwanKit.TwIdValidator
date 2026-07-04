using System;

namespace Ozakboy.TaiwanKit.TwIdValidator.Core
{
    /// <summary>
    /// 身分證/統一證號檢核演算法（內政部規格：首碼英文轉兩位數字，權重 1,9,8,7,6,5,4,3,2,1,1，總和須為 10 的倍數）
    /// Checksum algorithm for Taiwan ID numbers (MOI spec: first letter maps to two digits, weights 1,9,8,7,6,5,4,3,2,1,1, sum must be a multiple of 10)
    /// </summary>
    internal static class TwIdAlgorithm
    {
        /// <summary>
        /// 已停用的戶籍縣市代碼（L 臺中縣、R 臺南縣、S 高雄縣於 2010 縣市合併停用；Y 陽明山管理局）
        /// Obsolete region codes (L/R/S retired after the 2010 county-city mergers; Y Yangmingshan Administration)
        /// </summary>
        private const string ObsoleteLetters = "LRSY";

        /// <summary>
        /// 第 3～9 碼的權重
        /// Weights for characters 3 to 9
        /// </summary>
        private static readonly int[] MiddleWeights = { 7, 6, 5, 4, 3, 2, 1 };

        /// <summary>
        /// 取得首碼英文對應的轉換值（A=10、B=11 … 非連續，依內政部對照表）
        /// Gets the mapped value of a letter (A=10, B=11 … non-sequential, per the MOI mapping table)
        /// </summary>
        /// <param name="letter">大寫英文字母 / Uppercase letter</param>
        /// <returns>轉換值；非 A-Z 回傳 -1 / Mapped value; -1 when not A-Z</returns>
        internal static int GetLetterValue(char letter)
        {
            switch (letter)
            {
                case 'A': return 10;
                case 'B': return 11;
                case 'C': return 12;
                case 'D': return 13;
                case 'E': return 14;
                case 'F': return 15;
                case 'G': return 16;
                case 'H': return 17;
                case 'I': return 34;
                case 'J': return 18;
                case 'K': return 19;
                case 'L': return 20;
                case 'M': return 21;
                case 'N': return 22;
                case 'O': return 35;
                case 'P': return 23;
                case 'Q': return 24;
                case 'R': return 25;
                case 'S': return 26;
                case 'T': return 27;
                case 'U': return 28;
                case 'V': return 29;
                case 'W': return 32;
                case 'X': return 30;
                case 'Y': return 31;
                case 'Z': return 33;
                default: return -1;
            }
        }

        /// <summary>
        /// 取得首碼英文對應的戶籍（初次登記）縣市名稱；已停用代碼附「（已停用）」標注
        /// Gets the region name for the first letter; obsolete codes are suffixed with "（已停用）"
        /// </summary>
        /// <param name="letter">大寫英文字母 / Uppercase letter</param>
        /// <returns>縣市名稱；非 A-Z 回傳 null / Region name; null when not A-Z</returns>
        internal static string? GetRegionName(char letter)
        {
            switch (letter)
            {
                case 'A': return "臺北市";
                case 'B': return "臺中市";
                case 'C': return "基隆市";
                case 'D': return "臺南市";
                case 'E': return "高雄市";
                case 'F': return "新北市";
                case 'G': return "宜蘭縣";
                case 'H': return "桃園市";
                case 'I': return "嘉義市";
                case 'J': return "新竹縣";
                case 'K': return "苗栗縣";
                case 'L': return "臺中縣（已停用）";
                case 'M': return "南投縣";
                case 'N': return "彰化縣";
                case 'O': return "新竹市";
                case 'P': return "雲林縣";
                case 'Q': return "嘉義縣";
                case 'R': return "臺南縣（已停用）";
                case 'S': return "高雄縣（已停用）";
                case 'T': return "屏東縣";
                case 'U': return "花蓮縣";
                case 'V': return "臺東縣";
                case 'W': return "金門縣";
                case 'X': return "澎湖縣";
                case 'Y': return "陽明山管理局（已停用）";
                case 'Z': return "連江縣";
                default: return null;
            }
        }

        /// <summary>
        /// 判斷首碼是否為已停用的縣市代碼
        /// Determines whether the letter is an obsolete region code
        /// </summary>
        /// <param name="letter">大寫英文字母 / Uppercase letter</param>
        /// <returns>是否已停用 / Whether obsolete</returns>
        internal static bool IsObsoleteRegion(char letter)
        {
            return ObsoleteLetters.IndexOf(letter) >= 0;
        }

        /// <summary>
        /// 目前仍發行使用的縣市代碼（供假資料產生器使用，排除已停用代碼）
        /// Region codes currently in use (for the test-data generator; obsolete codes excluded)
        /// </summary>
        internal static readonly char[] ActiveRegionLetters = new[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
            'M', 'N', 'O', 'P', 'Q', 'T', 'U', 'V', 'W', 'X', 'Z',
        };

        /// <summary>
        /// 計算證號的加權總和（不含檢核判定）。輸入必須是已通過格式檢查的 10 碼大寫證號。
        /// Computes the weighted sum of an ID number (without the final check). Input must be a format-checked 10-character uppercase ID.
        /// </summary>
        /// <param name="id">10 碼證號 / 10-character ID</param>
        /// <param name="isOldResident">是否為舊式居留證號（第 2 碼為英文） / Whether it is an old-style resident ID (2nd character is a letter)</param>
        /// <returns>加權總和 / Weighted sum</returns>
        internal static int ComputeWeightedSum(string id, bool isOldResident)
        {
            int firstValue = GetLetterValue(id[0]);
            int sum = (firstValue / 10) * 1 + (firstValue % 10) * 9;

            if (isOldResident)
            {
                // 舊式居留證第 2 碼英文：取轉換值的個位數參與計算（權重 8）
                // Old-style 2nd letter: only the last digit of the mapped value is used (weight 8)
                sum += (GetLetterValue(id[1]) % 10) * 8;
            }
            else
            {
                sum += (id[1] - '0') * 8;
            }

            for (int i = 0; i < 7; i++)
            {
                sum += (id[2 + i] - '0') * MiddleWeights[i];
            }

            sum += id[9] - '0';
            return sum;
        }

        /// <summary>
        /// 依已知的前 9 碼計算檢核碼（使加權總和為 10 的倍數）
        /// Computes the check digit for the first 9 characters (making the weighted sum a multiple of 10)
        /// </summary>
        /// <param name="idWithoutCheck">前 9 碼證號 / First 9 characters of the ID</param>
        /// <param name="isOldResident">是否為舊式居留證號 / Whether it is an old-style resident ID</param>
        /// <returns>檢核碼（0-9） / Check digit (0-9)</returns>
        internal static int ComputeCheckDigit(string idWithoutCheck, bool isOldResident)
        {
            // 補一個 0 當暫時檢核碼計算基礎總和
            // Append a temporary 0 as the check digit to compute the base sum
            int baseSum = ComputeWeightedSum(idWithoutCheck + "0", isOldResident);
            return (10 - (baseSum % 10)) % 10;
        }
    }
}
