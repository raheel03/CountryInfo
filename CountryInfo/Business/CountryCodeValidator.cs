using System;
using System.Linq;
using Business.Exception;
using Business.Interface;

namespace Business
{
    public class CountryCodeValidator : ICountryCodeValidator
    {
        private const int MinCountryCodeLength = 2;
        private const int MaxCountryCodeLength = 3;

        public void Validate(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) ||
                countryCode.Length < MinCountryCodeLength ||
                countryCode.Length > MaxCountryCodeLength ||
                !countryCode.All(Char.IsLetter))
            {
                throw new InvalidCountryCodeException();
            }
        }
    }
}
