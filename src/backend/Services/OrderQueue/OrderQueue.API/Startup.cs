using Infrastructure.Core.Config;
using Infrastructure.Core.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderQueue.API.Mapping;
using OrderQueue.DataAccess;
using OrderQueue.DataAccess.Data;
using OrderQueue.API.Consumers;
using OrderQueue.API.Services;

namespace OrderQueue.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddScoped<IDbInitializer, DbInitializer>();
            
            services.AddDataAccess<OrderQueueDbContext>(_configuration.GetConnectionString("OrderQueueDb"));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMassTransit(config =>
            {
                config.AddConsumer<NewKitchenOrderConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(_configuration["RabbitMq:Host"]);
                    cfg.ReceiveEndpoint("new-kitchen-order",
                        c => { c.ConfigureConsumer<NewKitchenOrderConsumer>(ctx); });
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer,
            ILogger<Startup> logger)
        {
            logger.LogInformation(ConfigurationSerializer.Serialize(_configuration).ToString());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<KitchenOrdersService>();

                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("RabbitMQ"); });
            });

            dbInitializer.Init();
        }
    }
}