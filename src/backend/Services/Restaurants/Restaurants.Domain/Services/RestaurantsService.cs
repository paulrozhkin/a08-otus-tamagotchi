using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Models;
using Restaurants.Domain.Repositories.Specifications;

namespace Restaurants.Domain.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Restaurant> _restaurantsRepository;
        private readonly IAddressService _addressService;

        public RestaurantsService(ILogger<RestaurantsService> logger, IUnitOfWork unitOfWork,
            IAddressService addressService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _restaurantsRepository = _unitOfWork.Repository<Restaurant>();
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

            if (restaurant == null)
            {
                throw new EntityNotFoundException();
            }

            restaurant.Address =
                await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
            return restaurant;
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            var specification = new RestaurantLocationSpecification(restaurant.Latitude, restaurant.Longitude);
            var restaurantsWithSameLocation = await _restaurantsRepository.FindAsync(specification);

            if (restaurantsWithSameLocation.Any())
            {
                _logger.LogError(
                    $"Can't create restaurant. Restaurants with same location already exist (Lat - {restaurant.Latitude}; Lon - {restaurant.Longitude})");
                throw new EntityAlreadyExistsException();
            }

            await _restaurantsRepository.AddAsync(restaurant);
            _unitOfWork.Complete();

            restaurant.Address =
                await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
            return restaurant;
        }

        public async Task<Restaurant> UpdateRestaurant(Restaurant restaurant)
        {
            var restaurantWithSameId = await _restaurantsRepository.FindByIdAsync(restaurant.Id);

            if (restaurantWithSameId == null)
            {
                throw new EntityNotFoundException();
            }

            if (restaurantWithSameId.Latitude != restaurant.Latitude ||
                restaurantWithSameId.Longitude != restaurant.Longitude)
            {
                var specification = new RestaurantLocationSpecification(restaurant.Latitude, restaurant.Longitude);
                var restaurantsWithSameLocation = await _restaurantsRepository.FindAsync(specification);

                if (restaurantsWithSameLocation.Any())
                {
                    _logger.LogError(
                        $"Can't create restaurant. Restaurants with same location already exist (Lat - {restaurant.Latitude}; Lon - {restaurant.Longitude})");
                    throw new EntityAlreadyExistsException();
                }
            }

            restaurantWithSameId.Latitude = restaurant.Latitude;
            restaurantWithSameId.Longitude = restaurant.Longitude;
            restaurantWithSameId.PhoneNumber = restaurant.PhoneNumber;
            restaurantWithSameId.IsParkingPresent = restaurant.IsParkingPresent;
            restaurantWithSameId.IsWiFiPresent = restaurant.IsWiFiPresent;
            restaurantWithSameId.IsCardPaymentPresent = restaurant.IsCardPaymentPresent;

            _restaurantsRepository.Update(restaurantWithSameId);
            _unitOfWork.Complete();

            await FillAddress(restaurantWithSameId);

            return restaurantWithSameId;
        }

        public async Task DeleteRestaurantAsync(Guid id)
        {
            var restaurant = await _restaurantsRepository.FindByIdAsync(id);

            if (restaurant == null)
            {
                throw new EntityNotFoundException();
            }

            _restaurantsRepository.Remove(restaurant);
            _unitOfWork.Complete();
        }

        private async Task FillAddress(Restaurant restaurant)
        {
            try
            {
                restaurant.Address =
                    await _addressService.GetAddressFromLocation(restaurant.Latitude, restaurant.Longitude);
            }
            catch (Exception e)
            {
                _logger.LogError($"Can't get address for restaurant {restaurant.Id} cause {e}");
            }
        }
    }
}