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
using MassTransit;
using Web.HttpAggregator.Consumers;
using Web.HttpAggregator.Hubs;
using Web.HttpAggregator.Mapping;
using static Orders.API.Orders;

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

            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<UrlsOptions>(Configuration.GetSection(
                UrlsOptions.Urls));

            services.AddGrpcServices();

            services.AddMassTransit(config =>
            {
                config.AddConsumer<KitchenOrderConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("amqp://guest:guest@localhost:5672");
                    cfg.ReceiveEndpoint("kitchen-order", c =>
                    {
                        c.ConfigureConsumer<KitchenOrderConsumer>(ctx);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tamagotchi", Version = "v1" });
            });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .WithOrigins("http://localhost:3001")
                        .WithOrigins("http://localhost:3002")
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST")
                        .AllowCredentials();
                });

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

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<KitchenOrderHub>("/hubs/kitchen-order");
                }
            );
        }
    }

    public static class GrpcServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IOrdersService, OrdersService>();

            services.AddGrpcClient<Restaurants.RestaurantsClient>((serviceProvider, options) =>
            {
                var basketApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.RestaurantsGrpc;
                options.Address = new Uri(basketApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<OrdersClient>((serviceProvider, options) =>
            {
                var ordersApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.OrdersGrpc;
                options.Address = new Uri(ordersApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}