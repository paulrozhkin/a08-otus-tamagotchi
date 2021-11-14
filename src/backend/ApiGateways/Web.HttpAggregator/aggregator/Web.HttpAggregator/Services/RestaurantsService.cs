using System;
using System.Collections.Generic;
using System.Linq;
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
    // TODO: GRPC error handling
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

            var restaurants = await _restaurantsClient.GetRestaurantsAsync(request);

            var result = new PaginationResponse<RestaurantResponse>()
            {
                CurrentPage = restaurants.CurrentPage,
                PageSize = restaurants.PageSize,
                TotalCount = restaurants.TotalCount,
                Items = _mapper.Map<List<RestaurantResponse>>(restaurants.Restaurants)
            };

            return result;
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
            var restaurantCreateResponse = await _restaurantsClient.AddRestaurantAsync(new AddRestaurantRequest()
            {
                Restaurant = _mapper.Map<Restaurant>(restaurant)
            });

            return _mapper.Map<RestaurantResponse>(restaurantCreateResponse.Restaurant);
        }

        public Task<RestaurantResponse> UpdateRestaurantAsync(Guid id, RestaurantRequest restaurant)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
            }
        }

        public Task DeleteRestaurantAsync(Guid id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
            }
        }
    }
}