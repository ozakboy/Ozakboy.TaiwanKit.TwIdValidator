namespace Ozakboy.TaiwanKit.TwIdValidator
{
    /// <summary>
    /// 台灣證號類型
    /// Kind of Taiwan identification number
    /// </summary>
    public enum TwIdKind
    {
        /// <summary>
        /// 本國人身分證字號（第 2 碼為 1/2，如 A123456789）
        /// National ID number for R.O.C. citizens (2nd character is 1/2, e.g. A123456789)
        /// </summary>
        NationalId = 0,

        /// <summary>
        /// 新式外來人口統一證號（2021 年起發行，第 2 碼為 8/9，如 A800000014）
        /// New-style UI number for foreign residents (issued since 2021, 2nd character is 8/9, e.g. A800000014)
        /// </summary>
        NewResidentId = 1,

        /// <summary>
        /// 舊式外僑居留證統一證號（前 2 碼為英文字母，如 AB12345678，2031 年前仍有效）
        /// Old-style ARC UI number (first 2 characters are letters, e.g. AB12345678, valid until 2031)
        /// </summary>
        OldResidentId = 2,
    }
}
