using Ozakboy.TaiwanKit.TwIdValidator;
using Xunit;

namespace Ozakboy.TaiwanKit.TwIdValidator.Tests
{
    public class TwIdTests
    {
        // ---- 有效樣本（手算檢核碼） / Valid samples (hand-computed checksums) ----

        [Theory]
        [InlineData("A123456789")]  // 經典教科書樣本 / classic textbook sample
        [InlineData("F131104093")]  // 新北市男性 / male, New Taipei
        [InlineData("T112663836")]  // 屏東縣男性 / male, Pingtung
        public void IsValid_NationalId_ValidSamples_ReturnsTrue(string id)
        {
            Assert.True(TwId.IsValid(id));
        }

        [Fact]
        public void Validate_NationalId_ParsesKindGenderRegion()
        {
            TwIdResult result = TwId.Validate("A123456789");

            Assert.True(result.IsValid);
            Assert.Equal(ValidationFailure.None, result.Failure);
            Assert.Equal(TwIdKind.NationalId, result.Kind);
            Assert.Equal(Gender.Male, result.Gender);
            Assert.Equal("臺北市", result.RegionName);
        }

        [Fact]
        public void Validate_NewResidentId_ValidSample_ParsesKindAndGender()
        {
            // A800000014：內政部新式統一證號範例 / MOI sample of the new-style UI number
            TwIdResult result = TwId.Validate("A800000014");

            Assert.True(result.IsValid);
            Assert.Equal(TwIdKind.NewResidentId, result.Kind);
            Assert.Equal(Gender.Male, result.Gender);
        }

        [Fact]
        public void Validate_NewResidentId_Female_ParsesGender()
        {
            // 以產生器產出固定樣本驗證第 2 碼 9 為女性 / second digit 9 means female
            string id = TwId.Generate(TwIdKind.NewResidentId, Gender.Female, seed: 42);
            TwIdResult result = TwId.Validate(id);

            Assert.True(result.IsValid);
            Assert.Equal(TwIdKind.NewResidentId, result.Kind);
            Assert.Equal(Gender.Female, result.Gender);
        }

        [Fact]
        public void Validate_OldResidentId_ValidSample_ParsesKindAndGender()
        {
            // AB12345677：手算樣本（A→1、B→11 取個位 1×8、加權後檢核碼 7）
            // AB12345677: hand-computed sample (A→1, B→11 last digit 1×8, check digit 7)
            TwIdResult result = TwId.Validate("AB12345677");

            Assert.True(result.IsValid);
            Assert.Equal(TwIdKind.OldResidentId, result.Kind);
            Assert.Equal(Gender.Female, result.Gender);
            Assert.Equal("臺北市", result.RegionName);
        }

        // ---- 輸入容忍 / Input tolerance ----

        [Theory]
        [InlineData("a123456789")]
        [InlineData(" A123456789 ")]
        [InlineData("\tA123456789\n")]
        public void IsValid_LowercaseAndWhitespace_Tolerated(string id)
        {
            Assert.True(TwId.IsValid(id));
        }

        // ---- 無效輸入 / Invalid inputs ----

        [Theory]
        [InlineData(null, ValidationFailure.NullOrEmpty)]
        [InlineData("", ValidationFailure.NullOrEmpty)]
        [InlineData("   ", ValidationFailure.NullOrEmpty)]
        [InlineData("A12345678", ValidationFailure.InvalidLength)]
        [InlineData("A1234567890", ValidationFailure.InvalidLength)]
        [InlineData("1123456789", ValidationFailure.InvalidFormat)]  // 首碼非英文 / first char not a letter
        [InlineData("A323456789", ValidationFailure.InvalidFormat)]  // 第 2 碼 3 不屬任何證號 / 2nd char 3 matches no kind
        [InlineData("A12345678X", ValidationFailure.InvalidFormat)]  // 尾碼非數字 / last char not a digit
        [InlineData("A123456780", ValidationFailure.InvalidChecksum)]
        [InlineData("A800000015", ValidationFailure.InvalidChecksum)]
        [InlineData("AB12345678", ValidationFailure.InvalidChecksum)]
        public void Validate_InvalidInputs_ReturnsExpectedFailure(string? id, ValidationFailure expected)
        {
            TwIdResult result = TwId.Validate(id);

            Assert.False(result.IsValid);
            Assert.Equal(expected, result.Failure);
        }

        [Fact]
        public void Validate_FullWidthInput_IsInvalid()
        {
            Assert.False(TwId.IsValid("Ａ１２３４５６７８９"));
        }

        // ---- 縣市對照 / Region mapping ----

        [Theory]
        [InlineData('A', "臺北市")]
        [InlineData('B', "臺中市")]
        [InlineData('E', "高雄市")]
        [InlineData('L', "臺中縣（已停用）")]
        [InlineData('R', "臺南縣（已停用）")]
        [InlineData('S', "高雄縣（已停用）")]
        [InlineData('Y', "陽明山管理局（已停用）")]
        [InlineData('Z', "連江縣")]
        public void Validate_RegionMapping_MatchesFirstLetter(char letter, string expectedRegion)
        {
            // 用產生器造出合法數字段，再替換首碼並重算檢核碼不可行；
            // 改為直接暴力找出該首碼的合法樣本（第 2 碼固定 1，其餘位數列舉檢核碼）
            // Build a valid sample for the letter by enumerating the check digit
            string? valid = null;
            for (int check = 0; check <= 9; check++)
            {
                string candidate = letter + "10000000" + (char)('0' + check);
                if (TwId.IsValid(candidate))
                {
                    valid = candidate;
                    break;
                }
            }

            Assert.NotNull(valid);
            Assert.Equal(expectedRegion, TwId.Validate(valid).RegionName);
        }
    }
}
