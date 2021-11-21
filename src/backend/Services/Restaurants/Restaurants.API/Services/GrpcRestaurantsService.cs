using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Infrastructure.Core.Localization;
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
        private readonly IMapper _mapper;

        public GrpcRestaurantsService(ILogger<GrpcRestaurantsService> logger, IRestaurantsService restaurantsService,
            IMapper mapper)
        {
            _logger = logger;
            _restaurantsService = restaurantsService;
            _mapper = mapper;
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

            var restaurantsDto = _mapper.Map<List<Restaurant>>(restaurants);
            response.Restaurants.Add(restaurantsDto);

            return response;
        }

        public override async Task<GetRestaurantResponse> GetRestaurant(GetRestaurantRequest request,
            ServerCallContext context)
        {
            try
            {
                var restaurant = await _restaurantsService.GetRestaurantByIdAsync(Guid.Parse(request.Id));
                var response = new GetRestaurantResponse
                {
                    Restaurant = _mapper.Map<Restaurant>(restaurant)
                };

                return response;
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Restaurant {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }


        public override async Task<AddRestaurantResponse> AddRestaurant(AddRestaurantRequest request,
            ServerCallContext context)
        {
            var restaurantModel = _mapper.Map<Domain.Models.Restaurant>(request);

            try
            {
                var restaurant = await _restaurantsService.AddRestaurantAsync(restaurantModel);

                var response = new AddRestaurantResponse
                {
                    Restaurant = _mapper.Map<Restaurant>(restaurant)
                };

                return response;
            }
            catch (EntityAlreadyExistsException)
            {
                _logger.LogError(string.Format(
                    Errors.Restaurants_Restaurant_with_location__latitude____0___longitude____1__already_exist,
                    restaurantModel.Latitude,
                    restaurantModel.Longitude));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
        }

        public override async Task<UpdateRestaurantResponse> UpdateRestaurant(UpdateRestaurantRequest request,
            ServerCallContext context)
        {
            var restaurantModel = _mapper.Map<Domain.Models.Restaurant>(request);

            try
            {
                var updateRestaurant = await _restaurantsService.UpdateRestaurant(restaurantModel);

                return new UpdateRestaurantResponse()
                {
                    Restaurant = _mapper.Map<Restaurant>(updateRestaurant)
                };
            }
            catch (EntityAlreadyExistsException)
            {
                _logger.LogError(string.Format(
                    Errors.Restaurants_Restaurant_with_location__latitude____0___longitude____1__already_exist, restaurantModel.Latitude,
                    restaurantModel.Longitude));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Restaurant {restaurantModel.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<Empty> DeleteRestaurant(DeleteRestaurantRequest request, ServerCallContext context)
        {
            var idForDelete = request.Id;

            try
            {
                await _restaurantsService.DeleteRestaurantAsync(Guid.Parse(idForDelete));
                return new Empty();
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Restaurant {idForDelete}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }
    }
}