using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderQueue.API.Mapping;
using OrderQueue.DataAccess;
using OrderQueue.DataAccess.Data;
using OrderQueue.API.Consumers;

namespace OrderQueue.API
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
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddRepositories();
            services.AddDataAccess(Configuration["ConnectionStrings:OrderQueueDb"]);
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMassTransit(config =>
            {
                config.AddConsumer<NewKitchenOrderConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMq:Host"]);
                    cfg.ReceiveEndpoint("new-kitchen-order", c =>
                    {
                        c.ConfigureConsumer<NewKitchenOrderConsumer>(ctx);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurant.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("RabbitMQ");
                });
            });

            dbInitializer.Init();
        }
    }
}
