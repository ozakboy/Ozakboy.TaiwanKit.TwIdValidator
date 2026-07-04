namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 驗證失敗原因
    /// Reason why validation failed
    /// </summary>
    public enum ValidationFailure
    {
        /// <summary>
        /// 驗證通過，無失敗原因
        /// Validation passed; no failure
        /// </summary>
        None = 0,

        /// <summary>
        /// 輸入為 null、空字串或全空白
        /// Input is null, empty or whitespace only
        /// </summary>
        NullOrEmpty = 1,

        /// <summary>
        /// 長度錯誤
        /// Invalid length
        /// </summary>
        InvalidLength = 2,

        /// <summary>
        /// 格式錯誤（字元種類或位置不符規則）
        /// Invalid format (character kind or position does not match the rule)
        /// </summary>
        InvalidFormat = 3,

        /// <summary>
        /// 檢核碼錯誤
        /// Checksum mismatch
        /// </summary>
        InvalidChecksum = 4,
    }
}
