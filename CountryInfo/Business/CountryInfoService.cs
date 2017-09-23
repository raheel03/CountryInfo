using Business.Interface;

namespace Business
{
    public class CountryInfoService : ICountryInfoService
    {
        private readonly IWorldBankClient _worldBankClient;
        private readonly CountryCodeValidator _countryCodeValidator;
        
        public CountryInfoService(IWorldBankClient worldBankClient)
        {
            Ensure.IsNotNull(worldBankClient, nameof(worldBankClient));

            _worldBankClient = worldBankClient;
            _countryCodeValidator = new CountryCodeValidator();
        }

        public Country GetCountryInfo(string countryCode)
        {
            _countryCodeValidator.Validate(countryCode);
            
            var country = _worldBankClient.GetCountryAsync(countryCode).Result;
            return country;
        }
    }
}
