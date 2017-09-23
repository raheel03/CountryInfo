using System;
using Business.Exception;
using Business.Interface;
using Moq;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace Business.Tests
{
    [TestFixture]
    public class CountryInfoServiceTests
    {
        private Mock<IWorldBankClient> _worldBankClientMock = new Mock<IWorldBankClient>();

        [SetUp]
        public void Setup()
        {
            _worldBankClientMock = new Mock<IWorldBankClient>();
        }

        [Test]
        public void Ctor_Throws_With_InvalidParams()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => new CountryInfoService(null));

            // Assert
            Assert.IsNotNull(ex);
            StringAssert.Contains("worldBankClient", ex.Message);
        }

        [Test]
        public void Ctor_DoesNotThrow_With_ValidParams()
        {
            // Arrange

            // Act
            // Assert
            Assert.DoesNotThrow(() => new CountryInfoService(_worldBankClientMock.Object));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("A1")]
        [TestCase("A")]
        [TestCase("A__")]
        [TestCase("123")]
        [TestCase("FOUR")]
        public void GetCountryInfo_Throws_When_CountryCode_Is_Invalid(string countryCode)
        {
            // Arrange
            var countryInfoService = new CountryInfoService(_worldBankClientMock.Object);

            // Act
            var ex = Assert.Throws<InvalidCountryCodeException>(() => countryInfoService.GetCountryInfo(countryCode));

            // Assert
            Assert.IsNotNull(ex);
        }

        [TestCase("BR")]
        [TestCase("BRA")]
        public void WorldBankClient_Fetches_CountryInfo_When_CountryCode_Is_Valid(string countryCode)
        {
            // Arrange
            var countryInfoService = new CountryInfoService(_worldBankClientMock.Object);

            // Act
            countryInfoService.GetCountryInfo(countryCode);

            // Assert
            _worldBankClientMock.Verify(x => x.GetCountryAsync(countryCode), Times.Once);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("A1")]
        [TestCase("A")]
        [TestCase("A__")]
        [TestCase("123")]
        [TestCase("FOUR")]
        public void WorldBankClient_DoesNot_Fetch_CountryInfo_When_CountryCode_Is_InValid(string countryCode)
        {
            // Arrange
            var countryInfoService = new CountryInfoService(_worldBankClientMock.Object);

            // Act
            // Assert
            Assert.Throws<InvalidCountryCodeException>(() => countryInfoService.GetCountryInfo(countryCode));
            _worldBankClientMock.Verify(x => x.GetCountryAsync(countryCode), Times.Never);
        }
    }
}
