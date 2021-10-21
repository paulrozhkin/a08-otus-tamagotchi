using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestaurantsApi;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    // TODO: GRPC error handling
    public class RestaurantService : IRestaurantService
    {
        private readonly ILogger<RestaurantService> _logger;
        private readonly Restaurants.RestaurantsClient _restaurantsClient;

        public RestaurantService(ILogger<RestaurantService> logger, Restaurants.RestaurantsClient restaurantsClient)
        {
            _logger = logger;
            _restaurantsClient = restaurantsClient;
        }

        public async Task<PaginationResponse<RestaurantResponse>> GetRestaurantsAsync(int pageNumber, int pageSize,
            string address)
        {
            var request = new GetRestaurantsRequest()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Address = address ?? string.Empty
            };

            var restaurants = await _restaurantsClient.GetRestaurantsAsync(request);

            var result = new PaginationResponse<RestaurantResponse>()
            {
                CurrentPage = restaurants.CurrentPage,
                PageSize = restaurants.PageSize,
                TotalCount = restaurants.TotalCount,
                Items = restaurants.Restaurants.Select(MapFromGrpcResponse).ToList()
            };

            return result;
        }

        public async Task<RestaurantResponse> GetRestaurantByIdAsync(int id)
        {
            var restaurantDto = (await _restaurantsClient.GetRestaurantAsync(new GetRestaurantRequest() {Id = id}))
                .Restaurant;
            return MapFromGrpcResponse(restaurantDto);
        }

        public async Task<RestaurantResponse> CreateRestaurant(CreateRestaurantRequest restaurant)
        {
            var restaurantDto = (await _restaurantsClient.AddRestaurantAsync(new AddRestaurantRequest()
                {Restaurant = MapToGrpcRequest(restaurant)})).Restaurant;
            return MapFromGrpcResponse(restaurantDto);
        }

        private RestaurantResponse MapFromGrpcResponse(Restaurant restaurant)
        {
            return new RestaurantResponse()
            {
                Id = restaurant.Id,
                Address = restaurant.Address,
                Latitude = restaurant.Latitude,
                Longitude = restaurant.Longitude,
                IsCardPaymentPresent = restaurant.IsCardPaymentPresent,
                IsParkingPresent = restaurant.IsParkingPresent,
                IsWiFiPresent = restaurant.IsWiFiPresent,
                PhoneNumber = restaurant.PhoneNumber
            };
        }

        private Restaurant MapToGrpcRequest(CreateRestaurantRequest restaurant)
        {
            return new Restaurant()
            {
                Latitude = restaurant.Latitude,
                Longitude = restaurant.Longitude,
                IsCardPaymentPresent = restaurant.IsCardPaymentPresent,
                IsParkingPresent = restaurant.IsParkingPresent,
                IsWiFiPresent = restaurant.IsWiFiPresent,
                PhoneNumber = restaurant.PhoneNumber
            };
        }
    }
}