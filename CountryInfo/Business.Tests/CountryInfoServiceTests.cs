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

        [Test]
        public void When_WorldBankClient_Fetches_No_Results_Then_Null_Is_Returned()
        {
            // Arrange
            _worldBankClientMock.Setup(x => x.GetCountryAsync(It.IsAny<string>())).ReturnsAsync((Country)null);

            var countryInfoService = new CountryInfoService(_worldBankClientMock.Object);

            // Act
            var result = countryInfoService.GetCountryInfo("BR");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void When_WorldBankClient_Fetches_A_Valid_Result_Then_A_Valid_Country_Is_Returned()
        {
            // Arrange
            var country = new Country
            {
                CountryName = "Brazil",
            };

            _worldBankClientMock.Setup(x => x.GetCountryAsync(It.IsAny<string>())).ReturnsAsync(country);

            var countryInfoService = new CountryInfoService(_worldBankClientMock.Object);

            // Act
            var actualResult = countryInfoService.GetCountryInfo("BR");

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(country.CountryName, actualResult.CountryName);
        }
    }
}
