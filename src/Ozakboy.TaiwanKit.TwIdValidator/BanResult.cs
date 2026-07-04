namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 統一編號的詳細驗證結果（不可變）
    /// Detailed validation result of a Taiwan business administration number (immutable)
    /// </summary>
    public sealed class BanResult
    {
        /// <summary>
        /// 建立驗證結果
        /// Creates a validation result
        /// </summary>
        internal BanResult(bool isValid, ValidationFailure failure)
        {
            IsValid = isValid;
            Failure = failure;
        }

        /// <summary>
        /// 是否為有效統一編號
        /// Whether the number is valid
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// 驗證失敗原因；驗證通過時為 <see cref="ValidationFailure.None"/>
        /// Failure reason; <see cref="ValidationFailure.None"/> when valid
        /// </summary>
        public ValidationFailure Failure { get; }
    }
}
