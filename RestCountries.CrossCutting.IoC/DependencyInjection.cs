using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestCountries.Application.Interfaces;
using RestCountries.Application.Services;
using RestCountries.Domain.Interfaces;
using RestCountries.Infrastructure.Caching;
using RestCountries.Infrastructure.ExternalServices;
using StackExchange.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRestCountriesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Registro das dependências
        services.AddScoped<ICountryService, CountryService>();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
        services.AddScoped<ICountryCache, CountryCache>();
        services.AddHttpClient<IRestCountriesApiClient, RestCountriesApiClient>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "MyRedisInstance";
        });

        return services;
    }
}
