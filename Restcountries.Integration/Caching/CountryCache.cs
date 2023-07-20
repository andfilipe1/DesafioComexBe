using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using RestCountries.Application.Interfaces;

namespace RestCountries.Infrastructure.Caching
{
    public class CountryCache : ICountryCache
    {
        private readonly IDistributedCache _distributedCache;
        private const string CacheKeyPrefix = "Country:";

        public CountryCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<List<Country>> GetCountriesFromCache()
        {
            var countriesObject = await _distributedCache.GetStringAsync(CacheKeyPrefix);

            if (countriesObject != null)
            {
                var countries = JsonSerializer.Deserialize<List<Country>>(countriesObject);
                return countries;
            }

            return null;
        }

        public async Task<List<Country>> GetCountriesFromCacheByName(string name)
        {
            var allCountries = await GetCountriesFromCache();

            if (allCountries != null)
            {
                var filteredCountries = allCountries.Where(c => c.name.common.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
                return filteredCountries;
            }

            return null;
        }
        public async Task<List<Country>> GetCountriesFromCacheByCurrency(string currency)
        {
            var allCountries = await GetCountriesFromCache();

            if (allCountries != null)
            {
                var filteredCountries = allCountries.Where(c => c.currencies != null && c.currencies.ContainsKey(currency)).ToList();
                return filteredCountries;
            }

            return null;
        }
        public async Task<List<Country>> GetCountriesFromCacheByCode(string code)
        {
            var allCountries = await GetCountriesFromCache();

            if (allCountries != null)
            {
                var filteredCountries = allCountries.Where(c => c.cca3.Equals(code, StringComparison.OrdinalIgnoreCase)).ToList();
                return filteredCountries;
            }

            return null;
        }
        public async Task SetCountriesInCache(List<Country> countries)
        {
            var jsonString = JsonSerializer.Serialize(countries);
            await _distributedCache.SetStringAsync(CacheKeyPrefix, jsonString);
        }
    }
}
