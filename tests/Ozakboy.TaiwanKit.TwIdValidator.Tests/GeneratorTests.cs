using Ozakboy.TaiwanKit.TwIdValidator;
using Xunit;

namespace Ozakboy.TaiwanKit.TwIdValidator.Tests
{
    public class GeneratorTests
    {
        [Theory]
        [InlineData(TwIdKind.NationalId)]
        [InlineData(TwIdKind.NewResidentId)]
        [InlineData(TwIdKind.OldResidentId)]
        public void Generate_TwId_AllKinds_ProducesValidNumbers(TwIdKind kind)
        {
            for (int seed = 0; seed < 1000; seed++)
            {
                string id = TwId.Generate(kind, gender: null, seed: seed);
                TwIdResult result = TwId.Validate(id);

                Assert.True(result.IsValid, $"Generated ID '{id}' (seed {seed}) should be valid.");
                Assert.Equal(kind, result.Kind);
            }
        }

        [Theory]
        [InlineData(TwIdKind.NationalId, Gender.Male)]
        [InlineData(TwIdKind.NationalId, Gender.Female)]
        [InlineData(TwIdKind.NewResidentId, Gender.Male)]
        [InlineData(TwIdKind.NewResidentId, Gender.Female)]
        [InlineData(TwIdKind.OldResidentId, Gender.Male)]
        [InlineData(TwIdKind.OldResidentId, Gender.Female)]
        public void Generate_TwId_SpecifiedGender_IsRespected(TwIdKind kind, Gender gender)
        {
            for (int seed = 0; seed < 100; seed++)
            {
                string id = TwId.Generate(kind, gender, seed);
                TwIdResult result = TwId.Validate(id);

                Assert.True(result.IsValid);
                Assert.Equal(gender, result.Gender);
            }
        }

        [Fact]
        public void Generate_TwId_SameSeed_IsReproducible()
        {
            string first = TwId.Generate(TwIdKind.NationalId, Gender.Male, seed: 12345);
            string second = TwId.Generate(TwIdKind.NationalId, Gender.Male, seed: 12345);

            Assert.Equal(first, second);
        }

        [Fact]
        public void Generate_TwId_NeverUsesObsoleteRegionLetters()
        {
            for (int seed = 0; seed < 1000; seed++)
            {
                string id = TwId.Generate(TwIdKind.NationalId, gender: null, seed: seed);

                Assert.DoesNotContain(id[0], "LRSY");
            }
        }

        [Fact]
        public void Generate_Ban_ProducesValidNumbers()
        {
            for (int seed = 0; seed < 1000; seed++)
            {
                string ban = TwBan.Generate(seed);

                Assert.True(TwBan.IsValid(ban), $"Generated BAN '{ban}' (seed {seed}) should be valid.");
            }
        }

        [Fact]
        public void Generate_Ban_SameSeed_IsReproducible()
        {
            Assert.Equal(TwBan.Generate(9999), TwBan.Generate(9999));
        }

        [Fact]
        public void Generate_Phone_ProducesValidNumbers()
        {
            for (int seed = 0; seed < 1000; seed++)
            {
                string phone = TwPhone.Generate(seed);

                Assert.True(TwPhone.IsValid(phone), $"Generated phone '{phone}' (seed {seed}) should be valid.");
            }
        }

        [Fact]
        public void Generate_Phone_SameSeed_IsReproducible()
        {
            Assert.Equal(TwPhone.Generate(7), TwPhone.Generate(7));
        }
    }
}
