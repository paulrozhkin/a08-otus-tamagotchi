using Infrastructure.Core.Config;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Grpc;
using Tables.Domain.Services;
using Tables.Infrastructure.Repository;
using Microsoft.Extensions.Options;
using RestaurantsApi;
using Tables.API.Config;
using Tables.API.Mapping;
using Tables.API.Services;


namespace Tables.API
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

            services.AddScoped<ITablesService, TablesService>();

            services.AddRestaurantsGrpcService();

            services.AddDataAccess<TablesDataContext>(_configuration.GetConnectionString("TablesDb"));
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
                endpoints.MapGrpcService<GrpcTablesService>();

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