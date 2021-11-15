using System;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Microsoft.Extensions.Logging;
using RestaurantsApi;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly ILogger<RestaurantsService> _logger;
        private readonly Restaurants.RestaurantsClient _restaurantsClient;
        private readonly IMapper _mapper;

        public RestaurantsService(ILogger<RestaurantsService> logger, Restaurants.RestaurantsClient restaurantsClient,
            IMapper mapper)
        {
            _logger = logger;
            _restaurantsClient = restaurantsClient;
            _mapper = mapper;
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

            var restaurantsResponse = await _restaurantsClient.GetRestaurantsAsync(request);

            return _mapper.Map<PaginationResponse<RestaurantResponse>>(restaurantsResponse);
        }

        public async Task<RestaurantResponse> GetRestaurantByIdAsync(Guid id)
        {
            try
            {
                var restaurantResponse =
                    await _restaurantsClient.GetRestaurantAsync(new GetRestaurantRequest() {Id = id.ToString()});
                return _mapper.Map<RestaurantResponse>(restaurantResponse.Restaurant);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
            }
        }

        public async Task<RestaurantResponse> CreateRestaurantAsync(RestaurantRequest restaurant)
        {
            try
            {
                var restaurantCreateResponse = await _restaurantsClient.AddRestaurantAsync(new AddRestaurantRequest()
                {
                    Restaurant = _mapper.Map<Restaurant>(restaurant)
                });

                return _mapper.Map<RestaurantResponse>(restaurantCreateResponse.Restaurant);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new EntityAlreadyExistsException(string.Format(
                    Errors.Restaurants_Restaurant_with_location__latitude____0___longitude____1__already_exist,
                    restaurant.Latitude, restaurant.Longitude));
            }
        }

        public async Task<RestaurantResponse> UpdateRestaurantAsync(Guid id, RestaurantRequest restaurant)
        {
            try
            {
                var restaurantForRequest = _mapper.Map<Restaurant>(restaurant);
                restaurantForRequest.Id = id.ToString();
                var restaurantCreateResponse = await _restaurantsClient.UpdateRestaurantAsync(
                    new UpdateRestaurantRequest()
                    {
                        Restaurant = restaurantForRequest
                    });

                return _mapper.Map<RestaurantResponse>(restaurantCreateResponse.Restaurant);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new EntityAlreadyExistsException(string.Format(
                    Errors.Restaurants_Restaurant_with_location__latitude____0___longitude____1__already_exist,
                    restaurant.Latitude, restaurant.Longitude));
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
            }
        }

        public async Task DeleteRestaurantAsync(Guid id)
        {
            try
            {
                await _restaurantsClient.DeleteRestaurantAsync(new DeleteRestaurantRequest()
                {
                    Id = id.ToString()
                });
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
            }
        }
    }
}