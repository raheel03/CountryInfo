using Business.Interface;
using System.Threading.Tasks;

namespace Business
{
    public class CountryInfoService : ICountryInfoService
    {
        private readonly IWorldBankClient _worldBankClient;
        private readonly ICountryCodeValidator _countryCodeValidator;

        public CountryInfoService(ICountryCodeValidator countryCodeValidator, IWorldBankClient worldBankClient)
        {
            Ensure.IsNotNull(countryCodeValidator, nameof(countryCodeValidator));
            Ensure.IsNotNull(worldBankClient, nameof(worldBankClient));

            _countryCodeValidator = countryCodeValidator;
            _worldBankClient = worldBankClient;
        }

        public async Task<Country> GetCountryInfo(string countryCode)
        {
            _countryCodeValidator.Validate(countryCode);

            var country = await _worldBankClient.GetCountryAsync(countryCode);
            return country;
        }
    }
}
