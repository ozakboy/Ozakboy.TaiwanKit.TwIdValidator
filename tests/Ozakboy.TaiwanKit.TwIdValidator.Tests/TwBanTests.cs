using Ozakboy.TaiwanKit.TwIdValidator;
using Xunit;

namespace Ozakboy.TaiwanKit.TwIdValidator.Tests
{
    public class TwBanTests
    {
        // ---- 有效樣本 / Valid samples ----

        [Theory]
        [InlineData("04595257")]  // 常見教學樣本（總和 40，同時符合新舊制） / classic sample (sum 40, valid under both rules)
        [InlineData("00000000")]  // 總和 0 / sum 0
        [InlineData("00000005")]  // 總和 5：僅新制合法（舊制 10 的倍數不成立） / sum 5: valid only under the 2021 rule
        [InlineData("00000074")]  // 第 7 碼為 7 的特例（10 視為 1 才成立） / 7th-digit-7 special case
        public void IsValid_ValidSamples_ReturnsTrue(string ban)
        {
            Assert.True(TwBan.IsValid(ban));
        }

        [Fact]
        public void IsValid_WhitespaceAroundInput_Tolerated()
        {
            Assert.True(TwBan.IsValid(" 04595257 "));
        }

        // ---- 無效輸入 / Invalid inputs ----

        [Theory]
        [InlineData(null, ValidationFailure.NullOrEmpty)]
        [InlineData("", ValidationFailure.NullOrEmpty)]
        [InlineData("   ", ValidationFailure.NullOrEmpty)]
        [InlineData("0459525", ValidationFailure.InvalidLength)]
        [InlineData("045952570", ValidationFailure.InvalidLength)]
        [InlineData("0459525A", ValidationFailure.InvalidFormat)]
        [InlineData("04595258", ValidationFailure.InvalidChecksum)]
        [InlineData("00000071", ValidationFailure.InvalidChecksum)]  // 第 7 碼為 7 但兩種取法皆不成立 / 7th digit 7 but neither variant passes
        [InlineData("12345678", ValidationFailure.InvalidChecksum)]
        public void Validate_InvalidInputs_ReturnsExpectedFailure(string? ban, ValidationFailure expected)
        {
            BanResult result = TwBan.Validate(ban);

            Assert.False(result.IsValid);
            Assert.Equal(expected, result.Failure);
        }

        [Fact]
        public void Validate_SeventhDigitSeven_BothVariantsAccepted()
        {
            // d8=0 或 5 走一般路徑；d8=4 或 9 走特例路徑，皆合法
            // d8 of 0/5 passes normally; 4/9 passes via the special case
            Assert.True(TwBan.IsValid("00000070"));
            Assert.True(TwBan.IsValid("00000075"));
            Assert.True(TwBan.IsValid("00000074"));
            Assert.True(TwBan.IsValid("00000079"));
            Assert.False(TwBan.IsValid("00000071"));
            Assert.False(TwBan.IsValid("00000072"));
            Assert.False(TwBan.IsValid("00000073"));
        }
    }
}
