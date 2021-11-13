using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Models;

namespace Restaurants.Domain.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IUnitOfWork _restaurantsUnitOfWork;
        private readonly IRepository<Restaurant> _restaurantsRepository;
        private readonly IAddressService _addressService;

        public RestaurantsService(ILogger<RestaurantsService> logger, IUnitOfWork restaurantsUnitOfWork, IAddressService addressService)
        {
            _logger = logger;
            _restaurantsUnitOfWork = restaurantsUnitOfWork;
            _restaurantsRepository = _restaurantsUnitOfWork.Repository<Restaurant>();
            _addressService = addressService;
        }

        public async Task<PagedList<Restaurant>> GetRestaurantsAsync(int pageNumber, int pageSize, string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                _logger.LogError("User try to invoke unsupported geocoding");
                throw new NotSupportedException("Geocoding is not supported");
            }

            var paginationSpecification = new PagedSpecification<Restaurant>(pageNumber, pageSize);
            var restaurants = (await _restaurantsRepository.FindAsync(paginationSpecification)).ToList();
            var totalCount = await _restaurantsRepository.CountAsync();

            var fillAddressesTasks = restaurants.Select(FillAddress).ToList();

            await Task.WhenAll(fillAddressesTasks);

            return new PagedList<Restaurant>(restaurants, totalCount, pageNumber, pageSize);
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(Guid id)
        {
            var restaurant = await _restaurantsRepository.FindByIdAsync(id);
            restaurant.Address = await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
            return restaurant;
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            await _restaurantsRepository.AddAsync(restaurant);
            _restaurantsUnitOfWork.Complete();

            restaurant.Address = await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
            return restaurant;
        }

        private async Task FillAddress(Restaurant restaurant)
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
    }
}
