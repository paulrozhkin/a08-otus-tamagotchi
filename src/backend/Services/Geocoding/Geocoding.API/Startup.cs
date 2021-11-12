﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geocoding.API.Config;
using Geocoding.API.Services.Cache;
using Geocoding.API.Services.Geocoding;
using Geocoding.Google;
using Infrastructure.Core.Config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Geocoding.API
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

            var googleConfig = _configuration.GetSection(GoogleMapConfigOptions.GoogleMapConfig);
            services.Configure<GoogleMapConfigOptions>(googleConfig);

            services.AddScoped<IGeocoding, GoogleGeocoding>();
            services.AddScoped<IGeocodingCache, GeocodingCache>();

            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _configuration.GetSection("Redis")["ConnectionString"];
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
                endpoints.MapGrpcService<GeocodingService>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }
}