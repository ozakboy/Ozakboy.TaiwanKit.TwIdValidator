using System;

namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 台灣統一編號驗證（8 碼，含財政部 2021 年 4 月新制檢核：加權總和為 5 的倍數）
    /// Taiwan business administration number (BAN) validator (8 digits, incl. the April 2021 rule: weighted sum is a multiple of 5)
    /// </summary>
    public static class TwBan
    {
        /// <summary>
        /// 統編各位數的權重
        /// Weights for each digit of the BAN
        /// </summary>
        private static readonly int[] Weights = { 1, 2, 1, 2, 1, 2, 4, 1 };

        /// <summary>
        /// 驗證統一編號是否有效。null/空字串回傳 false，不拋例外。
        /// Validates the BAN. Returns false for null/empty input; never throws.
        /// </summary>
        /// <param name="input">統一編號（容忍前後空白） / BAN (leading/trailing whitespace tolerated)</param>
        /// <returns>是否有效 / Whether valid</returns>
        public static bool IsValid(string? input)
        {
            return Validate(input).IsValid;
        }

        /// <summary>
        /// 驗證統一編號並回傳詳細結果。null/空字串回傳失敗結果，不拋例外。
        /// Validates the BAN and returns a detailed result. Never throws.
        /// </summary>
        /// <param name="input">統一編號（容忍前後空白） / BAN (leading/trailing whitespace tolerated)</param>
        /// <returns>詳細驗證結果 / Detailed validation result</returns>
        public static BanResult Validate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new BanResult(false, ValidationFailure.NullOrEmpty);
            }

            string ban = input!.Trim();

            if (ban.Length != 8)
            {
                return new BanResult(false, ValidationFailure.InvalidLength);
            }

            for (int i = 0; i < 8; i++)
            {
                if (ban[i] < '0' || ban[i] > '9')
                {
                    return new BanResult(false, ValidationFailure.InvalidFormat);
                }
            }

            int sum = ComputeSum(ban);

            // 2021 年 4 月新制：總和為 5 的倍數即合法（涵蓋舊制 10 的倍數）
            // April 2021 rule: valid when the sum is a multiple of 5 (superset of the old multiple-of-10 rule)
            if (sum % 5 == 0)
            {
                return new BanResult(true, ValidationFailure.None);
            }

            // 第 7 碼為 7 的特例：7×4=28 → 2+8=10，可視為 10 或 1，兩種取法擇一成立即合法
            // Special case when the 7th digit is 7: 7×4=28 → 2+8=10, which may count as 10 or 1
            if (ban[6] == '7' && (sum + 1) % 5 == 0)
            {
                return new BanResult(true, ValidationFailure.None);
            }

            return new BanResult(false, ValidationFailure.InvalidChecksum);
        }

        /// <summary>
        /// 產生檢核合法的測試用統一編號。產生的號碼僅格式合法，不對應真實公司。
        /// Generates a checksum-valid BAN for testing. Generated numbers are format-valid only and do not correspond to real companies.
        /// </summary>
        /// <param name="seed">亂數種子；指定後產生結果可重現 / Random seed; results are reproducible when specified</param>
        /// <returns>檢核合法的統一編號 / A checksum-valid BAN</returns>
        public static string Generate(int? seed = null)
        {
            Random random = seed.HasValue ? new Random(seed.Value) : new Random();

            char[] buffer = new char[8];
            for (int i = 0; i < 7; i++)
            {
                buffer[i] = (char)('0' + random.Next(10));
            }

            // 第 8 碼權重為 1（貢獻值即其本身），推算使總和為 5 的倍數的檢核碼
            // The 8th digit has weight 1 (contributes its own value); derive it so the sum is a multiple of 5
            buffer[7] = '0';
            int baseSum = ComputeSum(new string(buffer));
            int check = (5 - (baseSum % 5)) % 5;
            buffer[7] = (char)('0' + check);
            return new string(buffer);
        }

        /// <summary>
        /// 計算統編的加權總和（乘積超過一位數時取十位數與個位數相加）
        /// Computes the weighted sum of the BAN (tens and units of each product are added)
        /// </summary>
        /// <param name="ban">8 碼純數字統編 / 8-digit BAN</param>
        /// <returns>加權總和 / Weighted sum</returns>
        private static int ComputeSum(string ban)
        {
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                int product = (ban[i] - '0') * Weights[i];
                sum += (product / 10) + (product % 10);
            }

            return sum;
        }
    }
}
