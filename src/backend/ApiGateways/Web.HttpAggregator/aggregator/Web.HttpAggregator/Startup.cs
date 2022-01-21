using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Infrastructure.Core.Config;
using Web.HttpAggregator.Config;
using Web.HttpAggregator.Infrastructure.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.HttpOverrides;
using Web.HttpAggregator.Consumers;
using Web.HttpAggregator.Hubs;
using Web.HttpAggregator.Infrastructure.Extensions;
using Web.HttpAggregator.Mapping;
using System.Reflection;
using Infrastructure.Logging;
using Minio.AspNetCore;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            LogConfigs.ConfigureLogging(assemblyName, (IConfigurationRoot)configuration, environment);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddAuthenticationServices(_configuration);

            var urlsConfig = _configuration.GetSection(UrlsOptions.Urls);
            services.Configure<UrlsOptions>(urlsConfig);

            services.AddGrpcServices();
            services.AddFluentValidation();
            services.AddMinio(new Uri(_configuration["MinIO:ConnectionString"]));

            services.AddMassTransit(config =>
            {
                config.AddConsumer<KitchenOrderConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(_configuration["RabbitMq:Host"]);
                    cfg.ReceiveEndpoint("kitchen-order", c => { c.ConfigureConsumer<KitchenOrderConsumer>(ctx); });
                });
            });

            services.AddMassTransitHostedService();
            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Tamagotchi", Version = "v1"});
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((_) => true)
                        .AllowCredentials());
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(ConfigurationSerializer.Serialize(_configuration).ToString());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tamagotchi v1"));
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseJsonExceptionHandler();
                app.UseForwardedHeaders();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<KitchenOrderHub>("/hubs/kitchen-order");
                }
            );
        }
    }
}