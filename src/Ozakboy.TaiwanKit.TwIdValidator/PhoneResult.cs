namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 手機號碼的詳細驗證結果（不可變）
    /// Detailed validation result of a Taiwan mobile phone number (immutable)
    /// </summary>
    public sealed class PhoneResult
    {
        /// <summary>
        /// 建立驗證結果
        /// Creates a validation result
        /// </summary>
        internal PhoneResult(bool isValid, ValidationFailure failure)
        {
            IsValid = isValid;
            Failure = failure;
        }

        /// <summary>
        /// 是否為有效手機號碼（嚴格格式：09 開頭 10 碼數字）
        /// Whether the number is valid (strict format: 10 digits starting with 09)
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// 驗證失敗原因；驗證通過時為 <see cref="ValidationFailure.None"/>
        /// Failure reason; <see cref="ValidationFailure.None"/> when valid
        /// </summary>
        public ValidationFailure Failure { get; }
    }
}
