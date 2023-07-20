using Microsoft.OpenApi.Models;
using RestCountries.Application.Interfaces;
using RestCountries.Application.Services;
using RestCountries.Domain.Interfaces;
using RestCountries.Infrastructure.Caching;
using RestCountries.Infrastructure.ExternalServices;
using RestCountries.Infrastructure.Repositories;
using StackExchange.Redis;

namespace RestCountries.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Configure Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            // Add services to the container.
            builder.Services.AddControllers();


            var configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            // Configure dependency injection
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
            builder.Services.AddScoped<ICountryCache, CountryCache>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddHttpClient<IRestCountriesApiClient, RestCountriesApiClient>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "MyRedisInstance";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Configure Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
            });

            app.Run();
        }
    }
}
