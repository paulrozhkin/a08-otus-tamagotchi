using System;
using Infrastructure.Core.Config;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.API.Config;
using Orders.API.Services;
using Orders.Domain.Services;
using Orders.Infrastructure.Repository;

namespace Orders.API
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
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) => { cfg.Host(_configuration["RabbitMq:Host"]); });
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddMassTransitHostedService();

            services.AddGrpc();

            services.Configure<OrdersOptions>(_configuration.GetSection(OrdersOptions.Orders));

            services.AddMenuService(_configuration);

            services.AddScoped<IOrderService, OrderService>();
            services.AddDataAccess<OrdersDataContext>(_configuration.GetConnectionString("OrdersDb"));
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
                endpoints.MapGrpcService<OrdersService>();

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
        public static IServiceCollection AddMenuService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IMenuAmountService, MenuAmountService>();

            services.AddGrpcClient<MenuApi.Menu.MenuClient>((serviceProvider, options) =>
            {
                var menuApi = serviceProvider.GetRequiredService<IOptions<OrdersOptions>>().Value.MenuGrpc;
                options.Address = new Uri(menuApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}