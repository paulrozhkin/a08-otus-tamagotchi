using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Models;

namespace Restaurants.Domain.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IRestaurantsRepository _restaurantsRepository;

        public RestaurantsService(ILogger<RestaurantsService> logger, IRestaurantsRepository restaurantsRepository)
        {
            _logger = logger;
            _restaurantsRepository = restaurantsRepository;
        }

        public async Task<PagedList<Restaurant>> GetRestaurantsAsync(int pageNumber, int pageSize, string address)
        {
            // TODO: implement geocoding
            if (!string.IsNullOrEmpty(address))
            {
                _logger.LogError("User try to invoke unsupported geocoding");
                throw new NotSupportedException("Geocoding is not supported");
            }

            var restaurants = await _restaurantsRepository.GetRestaurantsAsync(pageNumber, pageSize);

            restaurants.ForEach(restaurant =>
            {
                restaurant.Address =
                    $"representation of the address by coordinates: {restaurant.Latitude} {restaurant.Longitude}"; // TODO: implement geocoding
            });

            return restaurants;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(id);
            restaurant.Address =
                $"representation of the address by coordinates: {restaurant.Latitude} {restaurant.Longitude}"; // TODO: implement geocoding
            return restaurant;
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            var newRestaurant = await _restaurantsRepository.AddRestaurantAsync(restaurant);
            newRestaurant.Address =
                $"representation of the address by coordinates: {newRestaurant.Latitude} {newRestaurant.Longitude}"; // TODO: implement geocoding
            return newRestaurant;
        }
    }
}
