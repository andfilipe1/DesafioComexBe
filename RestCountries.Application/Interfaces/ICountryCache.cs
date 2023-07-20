using System;
using System.Threading.Tasks;

namespace RestCountries.Application.Interfaces
{
    public interface ICountryCache
    {
        Task CacheCountries(List<Country> countries);
        Task<List<Country>> GetCountriesFromCache();
        Task<Country> GetCountryByName(string name);
        Task SetCountryByName(string name, Country country, TimeSpan expiration);
    }
}
