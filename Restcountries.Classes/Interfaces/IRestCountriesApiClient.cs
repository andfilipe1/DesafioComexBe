namespace RestCountries.Domain.Interfaces
{
    public interface IRestCountriesApiClient
    {
        Task<List<Country>> GetRestCountriesAll();
        Task<List<Country>> GetRestCountriesByName(string name);
        Task<List<Country>> GetRestCountriesByCode(string code);
        Task<List<Country>> GetRestCountriesByCurrency(string currency);
    }
}
