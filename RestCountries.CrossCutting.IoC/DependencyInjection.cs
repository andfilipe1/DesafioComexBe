using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestCountries.Application.Interfaces;
using RestCountries.Application.Services;
using RestCountries.Domain.Interfaces;
using RestCountries.Infrastructure.Caching;
using RestCountries.Infrastructure.ExternalServices;
using RestCountries.Infrastructure.Repositories;
using StackExchange.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRestCountriesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Registro das dependências

        // Application
        services.AddScoped<ICountryService, CountryService>();

        // Domain
        services.AddScoped<ICountryRepository, CountryRepository>();

        // Infrastructure
        services.AddScoped<ICountryCache, CountryCache>();
        services.AddScoped<IRestCountriesApiClient, RestCountriesApiClient>();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

        // Configurar o cache Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "MyRedisInstance";
        });

        // Adicionar o serviço IApiDescriptionGroupCollectionProvider
        services.AddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();



        // Registrar o HttpClient
        services.AddHttpClient();


        return services;
    }
}
