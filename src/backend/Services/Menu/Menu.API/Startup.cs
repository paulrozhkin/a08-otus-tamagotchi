using System;
using Infrastructure.Core.Config;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Grpc;
using Menu.API.Config;
using Menu.API.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Menu.API.Services;
using Menu.Domain.Services;
using Menu.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestaurantsApi;

namespace Menu.API
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

            services.Configure<RestaurantsOptions>(_configuration.GetSection(RestaurantsOptions.Restaurants));

            services.AddScoped<IDishesService, DishesService>();
            services.AddScoped<IMenuService, MenuService>();

            services.AddRestaurantsGrpcService();

            services.AddDataAccess<MenuDataContext>(_configuration.GetConnectionString("MenuDb"));
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
                endpoints.MapGrpcService<GrpcDishesService>();
                endpoints.MapGrpcService<GrpcMenuService>();

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
        public static IServiceCollection AddRestaurantsGrpcService(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IRestaurantsService, RestaurantsService>();

            services.AddGrpcClient<Restaurants.RestaurantsClient>((serviceProvider, options) =>
            {
                var restaurantsGrpc = serviceProvider.GetRequiredService<IOptions<RestaurantsOptions>>().Value
                    .RestaurantsGrpc;
                options.Address = new Uri(restaurantsGrpc);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}