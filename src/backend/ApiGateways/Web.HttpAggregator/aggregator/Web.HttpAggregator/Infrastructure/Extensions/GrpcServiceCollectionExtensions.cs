using System;
using DishesApi;
using Infrastructure.Core.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderQueue.API.Protos;
using OrdersApi;
using ResourcesApi;
using RestaurantsApi;
using TablesApi;
using UsersApi;
using Web.HttpAggregator.Config;
using Web.HttpAggregator.Services;
using Menu = MenuApi.Menu;

namespace Web.HttpAggregator.Infrastructure.Extensions
{
    public static class GrpcServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IRestaurantsService, RestaurantsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IOrderQueueService, OrderQueueService>();
            services.AddScoped<IDishesService, DishesService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ITablesService, TablesService>();
            services.AddScoped<IResourcesMetadataService, ResourcesMetadataService>();

            services.AddGrpcClient<KitchenOrders.KitchenOrdersClient>((serviceProvider, options) =>
            {
                var orderQueueApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.OrderQueueGrpc;
                options.Address = new Uri(orderQueueApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Restaurants.RestaurantsClient>((serviceProvider, options) =>
            {
                var basketApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.RestaurantsGrpc;
                options.Address = new Uri(basketApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Orders.OrdersClient>((serviceProvider, options) =>
            {
                var ordersApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.OrdersGrpc;
                options.Address = new Uri(ordersApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Dishes.DishesClient>((serviceProvider, options) =>
            {
                var dishesApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.DishesGrpc;
                options.Address = new Uri(dishesApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Menu.MenuClient>((serviceProvider, options) =>
            {
                var menuApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.MenuGrpc;
                options.Address = new Uri(menuApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Tables.TablesClient>((serviceProvider, options) =>
            {
                var tablesApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.TablesGrpc;
                options.Address = new Uri(tablesApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Resources.ResourcesClient>((serviceProvider, options) =>
            {
                var resourcesApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.ResourcesGrpc;
                options.Address = new Uri(resourcesApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<Users.UsersClient>((serviceProvider, options) =>
            {
                var usersApi = serviceProvider.GetRequiredService<IOptions<UrlsOptions>>().Value.UsersGrpc;
                options.Address = new Uri(usersApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}