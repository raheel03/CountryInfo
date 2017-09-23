using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IWorldBankClient
    {
        Task<Country> GetCountryAsync(string countryCode);
    }
}
