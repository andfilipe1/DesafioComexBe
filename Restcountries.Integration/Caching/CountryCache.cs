using System;
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
        private const string CacheCurrencysKey = "Currencys";
        private const string CacheCodeKey = "Code";

        public CountryCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<Country> GetCountryByName(string name)
        {
            var cacheKey = $"{CacheKeyPrefix}{name}";
            var countryJson = await _distributedCache.GetStringAsync(cacheKey);
            if (countryJson != null)
            {
                var country = JsonSerializer.Deserialize<Country>(countryJson);
                return country;
            }
            return null;
        }

        public async Task SetCountryByName(string name, Country country, TimeSpan expiration)
        {
            var cacheKey = $"{CacheKeyPrefix}{name}";
            var countryJson = JsonSerializer.Serialize(country);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            await _distributedCache.SetStringAsync(cacheKey, countryJson, options);
        }

        public Task CacheCountries(List<Country> countries)
        {
            throw new NotImplementedException();
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

    }
}
