namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 身分證/統一證號的詳細驗證結果（不可變）
    /// Detailed validation result of a Taiwan identification number (immutable)
    /// </summary>
    public sealed class TwIdResult
    {
        /// <summary>
        /// 建立驗證結果
        /// Creates a validation result
        /// </summary>
        internal TwIdResult(bool isValid, ValidationFailure failure, TwIdKind? kind, Gender? gender, string? regionName)
        {
            IsValid = isValid;
            Failure = failure;
            Kind = kind;
            Gender = gender;
            RegionName = regionName;
        }

        /// <summary>
        /// 是否為有效證號
        /// Whether the number is valid
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// 驗證失敗原因；驗證通過時為 <see cref="ValidationFailure.None"/>
        /// Failure reason; <see cref="ValidationFailure.None"/> when valid
        /// </summary>
        public ValidationFailure Failure { get; }

        /// <summary>
        /// 證號類型；格式無法辨識時為 null
        /// Kind of the number; null when the format is unrecognizable
        /// </summary>
        public TwIdKind? Kind { get; }

        /// <summary>
        /// 證號標示的性別；格式無法辨識時為 null
        /// Gender encoded in the number; null when the format is unrecognizable
        /// </summary>
        public Gender? Gender { get; }

        /// <summary>
        /// 首碼英文對應的戶籍（初次登記）縣市名稱；已停用的代碼會標注「（已停用）」；格式無法辨識時為 null
        /// Region name mapped from the first letter (household/first registration); obsolete codes are marked "（已停用）"; null when unrecognizable
        /// </summary>
        public string? RegionName { get; }
    }
}
