using System;
using System.Text;

namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 台灣手機號碼驗證（嚴格格式：09 開頭 10 碼數字；另提供 TryNormalize 處理 +886 與分隔符）
    /// Taiwan mobile phone number validator (strict format: 10 digits starting with 09; TryNormalize handles +886 and separators)
    /// </summary>
    public static class TwPhone
    {
        /// <summary>
        /// 驗證手機號碼是否有效（嚴格模式：僅接受 09 開頭 10 碼純數字）。null/空字串回傳 false，不拋例外。
        /// Validates the mobile number (strict mode: only 10 pure digits starting with 09). Returns false for null/empty; never throws.
        /// </summary>
        /// <param name="input">手機號碼（容忍前後空白） / Mobile number (leading/trailing whitespace tolerated)</param>
        /// <returns>是否有效 / Whether valid</returns>
        public static bool IsValid(string? input)
        {
            return Validate(input).IsValid;
        }

        /// <summary>
        /// 驗證手機號碼並回傳詳細結果（嚴格模式）。null/空字串回傳失敗結果，不拋例外。
        /// Validates the mobile number and returns a detailed result (strict mode). Never throws.
        /// </summary>
        /// <param name="input">手機號碼（容忍前後空白） / Mobile number (leading/trailing whitespace tolerated)</param>
        /// <returns>詳細驗證結果 / Detailed validation result</returns>
        public static PhoneResult Validate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new PhoneResult(false, ValidationFailure.NullOrEmpty);
            }

            string phone = input!.Trim();

            if (phone.Length != 10)
            {
                return new PhoneResult(false, ValidationFailure.InvalidLength);
            }

            if (phone[0] != '0' || phone[1] != '9')
            {
                return new PhoneResult(false, ValidationFailure.InvalidFormat);
            }

            for (int i = 0; i < 10; i++)
            {
                if (phone[i] < '0' || phone[i] > '9')
                {
                    return new PhoneResult(false, ValidationFailure.InvalidFormat);
                }
            }

            return new PhoneResult(true, ValidationFailure.None);
        }

        /// <summary>
        /// 嘗試將常見輸入格式正規化為標準 10 碼手機號碼：容忍 +886/886 國碼、空格、dash、小括號、句點與全形數字。
        /// 正規化成功且通過驗證時回傳 true。null/空字串回傳 false，不拋例外。
        /// Attempts to normalize common input formats into the standard 10-digit mobile number: tolerates +886/886 country code,
        /// spaces, dashes, parentheses, dots and full-width digits. Returns true when normalization succeeds and the result is valid. Never throws.
        /// </summary>
        /// <param name="input">任意格式的手機號碼 / Mobile number in any common format</param>
        /// <param name="normalized">正規化後的 10 碼號碼；失敗時為空字串 / Normalized 10-digit number; empty string on failure</param>
        /// <returns>是否成功正規化為有效號碼 / Whether normalization produced a valid number</returns>
        public static bool TryNormalize(string? input, out string normalized)
        {
            normalized = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            // 移除分隔符並將全形數字轉半形；'+' 僅允許出現在開頭
            // Strip separators and convert full-width digits; '+' is only allowed at the start
            StringBuilder builder = new StringBuilder(input!.Length);
            bool seenContent = false;
            foreach (char c in input)
            {
                if (c == ' ' || c == '\t' || c == '-' || c == '(' || c == ')' || c == '.' || c == '　')
                {
                    continue;
                }

                if (c >= '０' && c <= '９')
                {
                    builder.Append((char)('0' + (c - '０')));
                    seenContent = true;
                    continue;
                }

                if (c == '+')
                {
                    if (seenContent)
                    {
                        return false;
                    }

                    builder.Append(c);
                    seenContent = true;
                    continue;
                }

                if (c >= '0' && c <= '9')
                {
                    builder.Append(c);
                    seenContent = true;
                    continue;
                }

                return false;
            }

            string candidate = builder.ToString();

            // 轉換國碼前綴：+8869xxxxxxxx / 8869xxxxxxxx / +88609xxxxxxxx → 09xxxxxxxx
            // Convert country-code prefixes into the leading 0 form
            if (candidate.StartsWith("+886", StringComparison.Ordinal))
            {
                candidate = candidate.Substring(4);
                candidate = candidate.StartsWith("0", StringComparison.Ordinal) ? candidate : "0" + candidate;
            }
            else if (candidate.StartsWith("886", StringComparison.Ordinal) && candidate.Length >= 12)
            {
                candidate = candidate.Substring(3);
                candidate = candidate.StartsWith("0", StringComparison.Ordinal) ? candidate : "0" + candidate;
            }

            if (!IsValid(candidate))
            {
                return false;
            }

            normalized = candidate;
            return true;
        }

        /// <summary>
        /// 產生格式合法的測試用手機號碼（09 開頭 10 碼）。產生的號碼不對應真實用戶。
        /// Generates a format-valid mobile number for testing (10 digits starting with 09). Not tied to any real subscriber.
        /// </summary>
        /// <param name="seed">亂數種子；指定後產生結果可重現 / Random seed; results are reproducible when specified</param>
        /// <returns>格式合法的手機號碼 / A format-valid mobile number</returns>
        public static string Generate(int? seed = null)
        {
            Random random = seed.HasValue ? new Random(seed.Value) : new Random();

            char[] buffer = new char[10];
            buffer[0] = '0';
            buffer[1] = '9';
            for (int i = 2; i < 10; i++)
            {
                buffer[i] = (char)('0' + random.Next(10));
            }

            return new string(buffer);
        }
    }
}
