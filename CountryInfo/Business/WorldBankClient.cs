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

            var response = Client.GetAsync(countryInfoUrl);

            if (!response.Result.IsSuccessStatusCode)
            {
                return null;
            }

            var responseToken = await response.Result.Content.ReadAsAsync<JToken>();
            
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
                    CountryName = countryToken.Value<string>("name"),
                    Region = countryToken.Value<JObject>("region")?.Value<string>("value"),
                    CapitalCity = countryToken.Value<string>("capitalCity"),
                    Latitude = countryToken.Value<string>("latitude"),
                    Longitude = countryToken.Value<string>("longitude")
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
