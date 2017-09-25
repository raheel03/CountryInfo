using System;
using Business.Exception;
using Business.Interface;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
// ReSharper disable ObjectCreationAsStatement

namespace Business.Tests
{
    [TestFixture]
    public class CountryInfoServiceTests
    {
        private Mock<IWorldBankClient> _worldBankClientMock;
        private ICountryCodeValidator _countryCodeValidator;
        private ICountryInfoService _countryInfoService;

        [SetUp]
        public void Setup()
        {
            _worldBankClientMock = new Mock<IWorldBankClient>();
            _countryCodeValidator = new CountryCodeValidator();

            _countryInfoService = new CountryInfoService(_countryCodeValidator, _worldBankClientMock.Object);
        }

        [Test]
        public void Ctor_Throws_With_InvalidParams()
        {
            // Arrange

            // Act
            var ex1 = Assert.Throws<ArgumentNullException>(() => new CountryInfoService(_countryCodeValidator, null));
            var ex2 = Assert.Throws<ArgumentNullException>(() => new CountryInfoService(null, _worldBankClientMock.Object));
            
            // Assert
            StringAssert.Contains("worldBankClient", ex1.Message);
            StringAssert.Contains("countryCodeValidator", ex2.Message);
        }

        [Test]
        public void Ctor_DoesNotThrow_With_ValidParams()
        {
            // Arrange

            // Act
            // Assert
            Assert.DoesNotThrow(() => new CountryInfoService(_countryCodeValidator, _worldBankClientMock.Object));
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

            // Act
            var ex = Assert.ThrowsAsync<InvalidCountryCodeException>(() => _countryInfoService.GetCountryInfo(countryCode));

            // Assert
            Assert.IsNotNull(ex);
        }

        [TestCase("BR")]
        [TestCase("BRA")]
        public void WorldBankClient_Fetches_CountryInfo_When_CountryCode_Is_Valid(string countryCode)
        {
            // Arrange

            // Act
            _countryInfoService.GetCountryInfo(countryCode);

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
            // Act
            // Assert
            Assert.ThrowsAsync<InvalidCountryCodeException>(() => _countryInfoService.GetCountryInfo(countryCode));
            _worldBankClientMock.Verify(x => x.GetCountryAsync(countryCode), Times.Never);
        }

        [Test]
        public void When_WorldBankClient_Fetches_No_Results_Then_Null_Is_Returned()
        {
            // Arrange
            _worldBankClientMock.Setup(x => x.GetCountryAsync(It.IsAny<string>())).ReturnsAsync((Country)null);

            // Act
            var result = _countryInfoService.GetCountryInfo("BR").Result;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task When_WorldBankClient_Fetches_A_ValidResult_Then_A_ValidCountry_Is_Returned()
        {
            // Arrange
            var country = new Country
            {
                CountryName = "Brazil",
            };

            _worldBankClientMock.Setup(x => x.GetCountryAsync(It.IsAny<string>())).ReturnsAsync(country);

            // Act
            var actualResult = await _countryInfoService.GetCountryInfo("BR");

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(country.CountryName, actualResult.CountryName);
        }
    }
}
