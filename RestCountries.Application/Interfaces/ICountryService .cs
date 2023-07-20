using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestCountries.Application.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllCountries();
        Task<List<Country>> GetCountryByName(string name);
        Task<List<Country>> GetCountryByCode(string code);
        Task<List<Country>> GetCountriesByCurrency(string currency);
        Task<List<Country>> GetTradeRoute(string countryA, string countryB);

    }
}
