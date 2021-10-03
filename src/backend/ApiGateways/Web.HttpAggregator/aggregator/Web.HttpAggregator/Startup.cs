using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestaurantsApi;
using Web.HttpAggregator.Config;
using Web.HttpAggregator.Infrastructure.Exceptions;
using Web.HttpAggregator.Infrastructure.Grpc;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<UrlsOptions>(Configuration.GetSection(
                UrlsOptions.Urls));

            services.AddGrpcServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Tamagotchi", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tamagotchi v1"));
            }
            else
            {
                app.UseJsonExceptionHandler();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public static class GrpcServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IRestaurantService, RestaurantService>();

            services.AddGrpcClient<Restaurants.RestaurantsClient>((serviceProvider, options) =>
            {
                var basketApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.RestaurantsGrpc;
                options.Address = new Uri(basketApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}