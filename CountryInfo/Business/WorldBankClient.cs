using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Interface;
using Newtonsoft.Json.Linq;

namespace Business
{
    public class WorldBankClient : IWorldBankClient
    {
        private const string ApiBaseUrl = "http://api.worldbank.org/";
        private const string ApiCountriesEndpoint = "countries";
        private const string ApiResponseFormat = "format=json";

        private static readonly HttpClient Client;

        static WorldBankClient()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseUrl)
            };
        }

        public async Task<Country> GetCountryAsync(string countryCode)
        {
            string countryInfoUrl = $"{ApiCountriesEndpoint}/{countryCode}?{ApiResponseFormat}";

            var response = await Client.GetAsync(countryInfoUrl);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseToken = await response.Content.ReadAsAsync<JToken>();
            
            return ConvertToCountry(responseToken);
        }

        private static Country ConvertToCountry(JToken responseToken)
        {
            Country country;

            try
            {
                if (responseToken == null ||
                    responseToken.Count() < 2 ||
                    !responseToken[1].Any() ||
                    responseToken[1].FirstOrDefault() == null)
                {
                    return null;
                }

                var countryToken = responseToken[1].FirstOrDefault();

                country = new Country
                {
                    CountryName = countryToken.Value<string>(WorldBankApiConstants.CountryName),
                    Region = countryToken.Value<JObject>(WorldBankApiConstants.Region)?.Value<string>(WorldBankApiConstants.Value),
                    CapitalCity = countryToken.Value<string>(WorldBankApiConstants.CapitalCity),
                    Latitude = countryToken.Value<string>(WorldBankApiConstants.Latitude),
                    Longitude = countryToken.Value<string>(WorldBankApiConstants.Longitude)
                };
            }
            catch
            {
                return null;
            }

            return country;
        }
    }
}
