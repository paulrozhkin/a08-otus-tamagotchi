using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Core.Models;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Models;

namespace Restaurants.Domain.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly IAddressService _addressService;

        public RestaurantsService(ILogger<RestaurantsService> logger, IRestaurantsRepository restaurantsRepository, IAddressService addressService)
        {
            _logger = logger;
            _restaurantsRepository = restaurantsRepository;
            _addressService = addressService;
        }

        public async Task<PagedList<Restaurant>> GetRestaurantsAsync(int pageNumber, int pageSize, string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                _logger.LogError("User try to invoke unsupported geocoding");
                throw new NotSupportedException("Geocoding is not supported");
            }

            var restaurants = await _restaurantsRepository.GetRestaurantsAsync(pageNumber, pageSize);

            async Task FillAddress(Restaurant restaurant)
            {
                try
                {
                    restaurant.Address = await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Can't get address for restaurant {restaurant.Id} cause {e}");
                }
            }

            var fillAddressesTasks = new List<Task>();
            foreach (var restaurant in restaurants)
            {
                fillAddressesTasks.Add(FillAddress(restaurant));

            }

            await Task.WhenAll(fillAddressesTasks);

            return restaurants;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(id);
            restaurant.Address = await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
            return restaurant;
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            var newRestaurant = await _restaurantsRepository.AddRestaurantAsync(restaurant);
            newRestaurant.Address = await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
            return newRestaurant;
        }
    }
}
