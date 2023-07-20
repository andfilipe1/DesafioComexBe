namespace RestCountries.Domain.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountries();
        Task<Country> GetCountryByName(string name);
        Task<Country> GetCountryByCode(string code);
        Task<List<Country>> GetCountriesByCurrency(string currency);
    }
}
