using System;
using System.Threading.Tasks;

namespace RestCountries.Application.Interfaces
{
    public interface ICountryCache
    {
        Task<List<Country>> GetCountriesFromCache();
        Task<List<Country>> GetCountriesFromCacheByCode(string code);
        Task<List<Country>> GetCountriesFromCacheByCurrency(string currency);
        Task<List<Country>> GetCountriesFromCacheByName(string name);
        Task SetCountriesInCache(List<Country> countries);
    }
}
