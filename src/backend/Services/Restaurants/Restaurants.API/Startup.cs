using System;
using Infrastructure.Core.Config;
using Infrastructure.Core.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurants.API.Config;
using Restaurants.API.Mapping;
using Restaurants.API.Services;
using Restaurants.Domain.Services;
using Restaurants.Infrastructure.Repository;
using static Geocoding.API.Geocoding;

namespace Restaurants.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<GeocodingOptions>(_configuration.GetSection(GeocodingOptions.Geocoding));
            
            services.AddGeocodingService(_configuration);

            services.AddScoped<IRestaurantsService, RestaurantsService>();
            services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();

            services.AddDbContext<RestaurantsDataContext>(builder =>
            {
                builder.UseNpgsql(_configuration.GetConnectionString("Npgsql"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(ConfigurationSerializer.Serialize(_configuration).ToString());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcRestaurantsService>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }

    public static class GrpcServiceCollectionExtensions
    {
        public static IServiceCollection AddGeocodingService(this IServiceCollection services, IConfiguration configuration)
        {
            var geocodingOptions = configuration.GetSection(GeocodingOptions.Geocoding).Get<GeocodingOptions>();

            // Use fake object
            if (!geocodingOptions.UseGeocoding)
            {
                services.AddScoped<IAddressService, AddressServiceFake>();
                return services;
            }

            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IAddressService, AddressService>();

            services.AddGrpcClient<GeocodingClient>((serviceProvider, options) =>
            {
                var geocodingApi = serviceProvider.GetRequiredService<IOptions<GeocodingOptions>>().Value.GeocodingGrpc;
                options.Address = new Uri(geocodingApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}