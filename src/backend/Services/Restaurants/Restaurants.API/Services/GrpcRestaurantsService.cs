using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Services;
using RestaurantsApi;
using static RestaurantsApi.Restaurants;

namespace Restaurants.API.Services
{
    public class GrpcRestaurantsService : RestaurantsBase
    {
        private readonly ILogger<GrpcRestaurantsService> _logger;
        private readonly IRestaurantsService _restaurantsService;

        public GrpcRestaurantsService(ILogger<GrpcRestaurantsService> logger, IRestaurantsService restaurantsService)
        {
            _logger = logger;
            _restaurantsService = restaurantsService;
        }

        public override async Task<GetRestaurantsResponse> GetRestaurants(GetRestaurantsRequest request,
            ServerCallContext context)
        {
            var restaurants =
                await _restaurantsService.GetRestaurantsAsync(request.PageNumber, request.PageSize, request.Address);

            var response = new GetRestaurantsResponse
            {
                CurrentPage = restaurants.CurrentPage,
                PageSize = restaurants.PageSize,
                TotalCount = restaurants.TotalCount
            };

            var restaurantsDto = restaurants.Select(restaurant => new Restaurant()
            {
                Id = restaurant.Id.ToString(),
                Address = restaurant.Address,
                Latitude = restaurant.Latitude,
                Longitude = restaurant.Longitude,
                IsCardPaymentPresent = restaurant.IsCardPaymentPresent,
                IsParkingPresent = restaurant.IsParkingPresent,
                IsWiFiPresent = restaurant.IsWiFiPresent,
                PhoneNumber = restaurant.PhoneNumber
            });

            response.Restaurants.Add(restaurantsDto);

            return response;
        }

        public override async Task<GetRestaurantResponse> GetRestaurant(GetRestaurantRequest request,
            ServerCallContext context)
        {
            var restaurant = await _restaurantsService.GetRestaurantByIdAsync(Guid.Parse(request.Id));

            var restaurantDto = new Restaurant()
            {
                Id = restaurant.Id.ToString(),
                Address = restaurant.Address,
                Latitude = restaurant.Latitude,
                Longitude = restaurant.Longitude,
                IsCardPaymentPresent = restaurant.IsCardPaymentPresent,
                IsParkingPresent = restaurant.IsParkingPresent,
                IsWiFiPresent = restaurant.IsWiFiPresent,
                PhoneNumber = restaurant.PhoneNumber
            };

            var response = new GetRestaurantResponse
            {
                Restaurant = restaurantDto
            };

            return response;
        }

        public override async Task<AddRestaurantResponse> AddRestaurant(AddRestaurantRequest request,
            ServerCallContext context)
        {
            var restaurantRequest = request.Restaurant;
            var restaurantModel = new Domain.Models.Restaurant()
            {
                IsCardPaymentPresent = restaurantRequest.IsCardPaymentPresent,
                IsParkingPresent = restaurantRequest.IsParkingPresent,
                IsWiFiPresent = restaurantRequest.IsWiFiPresent,
                Latitude = restaurantRequest.Latitude,
                Longitude = restaurantRequest.Longitude,
                PhoneNumber = restaurantRequest.PhoneNumber
            };

            var restaurant = await _restaurantsService.AddRestaurantAsync(restaurantModel);

            var restaurantDto = new Restaurant()
            {
                Id = restaurant.Id.ToString(),
                Address = restaurant.Address,
                Latitude = restaurant.Latitude,
                Longitude = restaurant.Longitude,
                IsCardPaymentPresent = restaurant.IsCardPaymentPresent,
                IsParkingPresent = restaurant.IsParkingPresent,
                IsWiFiPresent = restaurant.IsWiFiPresent,
                PhoneNumber = restaurant.PhoneNumber
            };

            var response = new AddRestaurantResponse
            {
                Restaurant = restaurantDto
            };

            return response;
        }
    }
}