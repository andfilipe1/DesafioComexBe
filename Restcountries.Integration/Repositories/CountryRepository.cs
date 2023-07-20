using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestCountries.Domain.Interfaces;
using StackExchange.Redis;

namespace RestCountries.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDatabase _redisDatabase;

        public CountryRepository(IConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase();
        }

        public async Task<List<Country>> GetCountries()
        {
            var countriesJson = await _redisDatabase.StringGetAsync("Countries");
            if (!countriesJson.IsNullOrEmpty)
            {
                var countries = JsonConvert.DeserializeObject<List<Country>>(countriesJson);
                return countries;
            }
            return null;
        }
        public async Task<Country> GetCountryByName(string name)
        {
            return null;
        }

        public async Task<Country> GetCountryByCode(string code)
        {
            return null;
        }

        public async Task<List<Country>> GetCountriesByCurrency(string currency)
        {
            return new List<Country>();
        }
    }
}
