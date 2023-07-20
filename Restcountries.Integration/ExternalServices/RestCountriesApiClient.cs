using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RestCountries.Domain.Interfaces;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RestCountries.Infrastructure.ExternalServices
{
    public class RestCountriesApiClient : IRestCountriesApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly AsyncRetryPolicy _retryPolicy;


        public RestCountriesApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiSettings:RestCountriesApiBaseUrl"];
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                .WaitAndRetryAsync(4, retryAttempt =>
                                    TimeSpan.FromSeconds(Math.Pow(4, retryAttempt)));
        }

        public async Task<List<Country>> GetRestCountriesAll()
        {
            var apiUrl = $"{_apiBaseUrl}/v3.1/all";

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(apiUrl);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<List<Country>>(content);

                return countries;
            });
        }
        public async Task<List<Country>> GetRestCountriesByName(string name)
        {
            var apiUrl = $"{_apiBaseUrl}/v3.1/name/{name}?fullText=true";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<Country>>(content);
            return countries;
        }
        public async Task<List<Country>> GetRestCountriesByCode(string code)
        {
            var apiUrl = $"{_apiBaseUrl}/v3.1/alpha?codes={code}";

            var countries = await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var deserializedCountries = JsonSerializer.Deserialize<List<Country>>(content);
                return deserializedCountries ?? throw new InvalidOperationException("Deserialization returned null.");
            });

            return countries ?? new List<Country>();  
        }
        public async Task<List<Country>> GetRestCountriesByCurrency(string currency)
        {
            var apiUrl = $"{_apiBaseUrl}/v3.1/currency/{currency}";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<Country>>(content);
            return countries;
        }
    }
}
