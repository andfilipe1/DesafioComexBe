using RestCountries.Application.Interfaces;
using RestCountries.Domain.Interfaces;

namespace RestCountries.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryCache _countryCache;
        private readonly IRestCountriesApiClient _restCountriesApiClient;

        public CountryService(ICountryCache countryCache, IRestCountriesApiClient restCountriesApiClient)
        {
            _countryCache = countryCache;
            _restCountriesApiClient = restCountriesApiClient;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            var countries = await _restCountriesApiClient.GetRestCountriesAll();
            await SetIDatanCache(countries);
            return countries;
        }
        public async Task<List<Country>> GetCountryByName(string name)
        {
            var countries = await _countryCache.GetCountriesFromCacheByName(name);

            if (countries == null)
            {
                countries = await _restCountriesApiClient.GetRestCountriesByName(name);
                await SetIDatanCache(countries);
            }
            return countries;
        }
        public async Task<List<Country>> GetCountryByCode(string code)
        {
            var countries = await _countryCache.GetCountriesFromCacheByCode(code);

            if (countries == null)
            {
                countries = await _restCountriesApiClient.GetRestCountriesByCode(code);
                await SetIDatanCache(countries);
            }
            return countries;
        } 
        public async Task<List<Country>> GetCountriesByCurrency(string currency)
        {
            var countries = await _countryCache.GetCountriesFromCacheByCurrency(currency);

            if (countries == null)
            {
                countries = await _restCountriesApiClient.GetRestCountriesByCurrency(currency);
                if (countries != null)
                {
                    await _countryCache.SetCountriesInCache(countries);
                }

            }
            return countries;
        }
        public async Task<List<Country>> GetTradeRoute(string countryA, string countryB)
        {
            var allCountries = await _restCountriesApiClient.GetRestCountriesAll();
            var startCountry = allCountries.FirstOrDefault(c => c.name.common.Equals(countryA));
            var endCountry = allCountries.FirstOrDefault(c => c.name.common.Equals(countryB));

            if (startCountry == null || endCountry == null)
                throw new ArgumentException("One or both of the input countries do not exist.");

            var adjacencyList = new Dictionary<Country, List<Country>>();

            foreach (var country in allCountries)
            {
                adjacencyList[country] = await GetNeighbouringCountries(country);
            }

            if (!adjacencyList.ContainsKey(endCountry))
            {
                throw new ArgumentException("O país de destino não existe no conjunto de dados.");
            }

            var predecessorMap = BFS(adjacencyList, startCountry, endCountry);

            if (predecessorMap == null)
            {
                return null;
            }

            var path = new List<Country>();
            var currentCountry = endCountry;
            while (currentCountry != null)
            {
                path.Add(currentCountry);
                if (predecessorMap.ContainsKey(currentCountry))
                {
                    currentCountry = predecessorMap[currentCountry];
                }
                else
                {
                    currentCountry = null;
                    break;
                }
            }
            path.Reverse();
            return path;
        }

        #region private methods
        private async Task SetIDatanCache(List<Country>? countries)
        {
            if (countries != null)
            {
                await _countryCache.SetCountriesInCache(countries);
            }
        }
        private Dictionary<Country, Country> BFS(Dictionary<Country, List<Country>> adjacencyList, Country start, Country end)
        {
            var queue = new Queue<Country>();
            var visited = new HashSet<Country>();
            var predecessorMap = new Dictionary<Country, Country>();

            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                try
                {
                    var current = queue.Dequeue();

                    if (current.altSpellings.SequenceEqual(end.altSpellings))
                    {
                        return predecessorMap;
                    }
                    if (adjacencyList.TryGetValue(current, out var neighbours))
                    {
                        foreach (var neighbour in neighbours)
                        {
                            if (!visited.Contains(neighbour))
                            {
                                queue.Enqueue(neighbour);
                                predecessorMap[neighbour] = current;
                                visited.Add(neighbour);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    // Lidar com a exceção aqui, seja imprimindo uma mensagem de erro, registrando o erro ou realizando alguma ação adequada.
                    Console.WriteLine($"Erro durante a busca em largura (BFS): {ex.Message}");
                }
            }


            // Não encontramos o final, então retornamos nulo.
            return null;
        }
        private async Task<List<Country>> GetNeighbouringCountries(Country country)
        {
            var neighbouringCountriesCodes = country.borders;
            var neighbouringCountries = new List<Country>();
            if (neighbouringCountriesCodes != null)
            {
                foreach (var countryCode in neighbouringCountriesCodes)
                {
                    var neighbour = await GetCountryByCode(countryCode);
                    if (neighbour != null && neighbour.Count > 0)
                    {
                        neighbouringCountries.Add(neighbour[0]);
                    }
                }
            }
            return neighbouringCountries;
        }
        #endregion
    }
}
