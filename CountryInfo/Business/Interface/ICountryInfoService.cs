using System.Threading.Tasks;

namespace Business.Interface
{
    public interface ICountryInfoService
    {
       Task<Country> GetCountryInfo(string countryCode);
    }
}
