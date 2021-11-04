using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestaurantsApi;
using Web.HttpAggregator.Config;
using Web.HttpAggregator.Infrastructure.Grpc;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Infrastructure.Extensions
{
    public static class GrpcServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IDishesService, DishesService>();

            services.AddGrpcClient<Restaurants.RestaurantsClient>((serviceProvider, options) =>
            {
                var basketApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.RestaurantsGrpc;
                options.Address = new Uri(basketApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Orders.API.Orders.OrdersClient>((serviceProvider, options) =>
            {
                var ordersApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.OrdersGrpc;
                options.Address = new Uri(ordersApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}