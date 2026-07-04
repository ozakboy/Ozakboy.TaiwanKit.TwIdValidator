using Ozakboy.TaiwanKit.TwIdValidator;
using Xunit;

namespace Ozakboy.TaiwanKit.TwIdValidator.Tests
{
    public class TwPhoneTests
    {
        // ---- 嚴格驗證 / Strict validation ----

        [Theory]
        [InlineData("0912345678")]
        [InlineData("0987654321")]
        [InlineData(" 0912345678 ")]
        public void IsValid_ValidNumbers_ReturnsTrue(string phone)
        {
            Assert.True(TwPhone.IsValid(phone));
        }

        [Theory]
        [InlineData(null, ValidationFailure.NullOrEmpty)]
        [InlineData("", ValidationFailure.NullOrEmpty)]
        [InlineData("091234567", ValidationFailure.InvalidLength)]
        [InlineData("09123456789", ValidationFailure.InvalidLength)]
        [InlineData("0812345678", ValidationFailure.InvalidFormat)]   // 非 09 開頭 / not starting with 09
        [InlineData("1912345678", ValidationFailure.InvalidFormat)]
        [InlineData("09123A5678", ValidationFailure.InvalidFormat)]
        [InlineData("0912-45678", ValidationFailure.InvalidFormat)]   // 嚴格模式不容忍 dash / strict mode rejects dashes
        public void Validate_InvalidInputs_ReturnsExpectedFailure(string? phone, ValidationFailure expected)
        {
            PhoneResult result = TwPhone.Validate(phone);

            Assert.False(result.IsValid);
            Assert.Equal(expected, result.Failure);
        }

        // ---- 正規化 / Normalization ----

        [Theory]
        [InlineData("+886912345678", "0912345678")]
        [InlineData("+886-912-345-678", "0912345678")]
        [InlineData("+886 912 345 678", "0912345678")]
        [InlineData("886912345678", "0912345678")]
        [InlineData("+8860912345678", "0912345678")]
        [InlineData("0912-345-678", "0912345678")]
        [InlineData("0912 345 678", "0912345678")]
        [InlineData("(0912)345678", "0912345678")]
        [InlineData("0912.345.678", "0912345678")]
        [InlineData("０９１２３４５６７８", "0912345678")]  // 全形數字 / full-width digits
        [InlineData("0912345678", "0912345678")]
        public void TryNormalize_CommonFormats_ReturnsNormalized(string input, string expected)
        {
            bool ok = TwPhone.TryNormalize(input, out string normalized);

            Assert.True(ok);
            Assert.Equal(expected, normalized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-a-phone")]
        [InlineData("0812345678")]        // 正規化後仍非 09 開頭 / still not 09 after normalization
        [InlineData("091234567")]         // 位數不足 / too short
        [InlineData("8861234567")]        // 非手機的國碼形式 / country code but not a mobile number
        [InlineData("09123+45678")]       // '+' 不在開頭 / '+' not at the start
        public void TryNormalize_InvalidInputs_ReturnsFalse(string? input)
        {
            bool ok = TwPhone.TryNormalize(input, out string normalized);

            Assert.False(ok);
            Assert.Equal(string.Empty, normalized);
        }
    }
}
