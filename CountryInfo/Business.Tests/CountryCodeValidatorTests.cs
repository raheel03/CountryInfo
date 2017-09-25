using Business.Exception;
using NUnit.Framework;

namespace Business.Tests
{
    public class CountryCodeValidatorTests
    {
        private CountryCodeValidator _countryCodeValidator;

        [SetUp]
        public void Setup()
        {
            _countryCodeValidator = new CountryCodeValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Validate_Throws_When_CountryCode_Is_Empty(string countryCode)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<InvalidCountryCodeException>(() => _countryCodeValidator.Validate(countryCode));

            // Assert
            Assert.IsNotNull(ex);
        }

        [Test]
        public void Validate_Throws_When_CountryCode_Lessthan_2_Chars()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<InvalidCountryCodeException>(() => _countryCodeValidator.Validate("A"));

            // Assert
            Assert.IsNotNull(ex);
        }

        [TestCase("ABCD")]
        [TestCase("ABCDE")]
        public void Validate_Throws_When_CountryCode_Morethan_3_Chars(string countryCode)
        {
            // Arrange
            // Act
            var ex = Assert.Throws<InvalidCountryCodeException>(() => _countryCodeValidator.Validate(countryCode));

            // Assert
            Assert.IsNotNull(ex);
        }

        [TestCase("A B")]
        [TestCase("A12")]
        [TestCase("A_2")]
        public void Validate_Throws_When_CountryCode_Contain_NonAlphabets(string countryCode)
        {
            // Arrange
            // Act
            var ex = Assert.Throws<InvalidCountryCodeException>(() => _countryCodeValidator.Validate(countryCode));

            // Assert
            Assert.IsNotNull(ex);
        }

        [TestCase("BR")]
        [TestCase("BRA")]
        public void Validate_DoesNotThrow_When_CountryCode_Is_Valid(string countryCode)
        {
            // Arrange
            // Act
            // Assert
            Assert.DoesNotThrow(() => _countryCodeValidator.Validate(countryCode));
        }
    }
}
