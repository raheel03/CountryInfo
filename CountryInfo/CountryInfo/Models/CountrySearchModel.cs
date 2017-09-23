using System.ComponentModel.DataAnnotations;
using Business;

namespace CountryInfo.Models
{
    public class CountrySearchModel
    {
        public string Query { get; set; }

        public Country Result { get; set; }

        public bool HasErrors => !string.IsNullOrWhiteSpace(ErrorMessage);

        public string ErrorMessage { get; set; }
    }
}