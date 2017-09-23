namespace Business.Interface
{
    public interface ICountryInfoService
    {
        Country GetCountryInfo(string countryCode);
    }
}
