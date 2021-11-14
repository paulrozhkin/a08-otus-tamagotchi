using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DishesApi;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Microsoft.Extensions.Logging;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public class DishesService : IDishesService
    {
        private readonly ILogger<DishesService> _logger;
        private readonly Dishes.DishesClient _dishesClient;
        private readonly IMapper _mapper;

        public DishesService(ILogger<DishesService> logger, Dishes.DishesClient dishesClient, IMapper mapper)
        {
            _logger = logger;
            _dishesClient = dishesClient;
            _mapper = mapper;
        }


        public async Task<PaginationResponse<DishResponse>> GetDishesAsync(int pageNumber, int pageSize)
        {
            var dishesResponse = await _dishesClient.GetDishesAsync(new GetDishesRequest()
                {PageNumber = pageNumber, PageSize = pageSize});

            var dishesDto = _mapper.Map<List<DishResponse>>(dishesResponse.Dishes);

            return new PaginationResponse<DishResponse>()
            {
                CurrentPage = dishesResponse.CurrentPage,
                PageSize = dishesResponse.PageSize,
                TotalCount = dishesResponse.TotalCount,
                Items = dishesDto
            };
        }

        public async Task<DishResponse> GetDishByIdAsync(Guid id)
        {
            try
            {
                var dishResponse = await _dishesClient.GetDishAsync(new GetDishRequest() {Id = id.ToString()});
                return _mapper.Map<DishResponse>(dishResponse.Dish);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Dishes_Dish_with_id__0__not_found, id));
            }
        }

        public async Task<DishResponse> CreateDishAsync(DishRequest dish)
        {
            try
            {
                var dishResponse = await _dishesClient.CrateDishAsync(new CrateDishRequest()
                {
                    Dish = _mapper.Map<Dish>(dish)
                });

                return _mapper.Map<DishResponse>(dishResponse.Dish);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new NameAlreadyExistsException(string.Format(Errors.Dishes_Dish_with_name__0__already_exist,
                    dish.Name));
            }
        }

        public async Task<DishResponse> UpdateDish(Guid id, DishRequest dish)
        {
            try
            {
                var dishForRequest = _mapper.Map<Dish>(dish);
                dishForRequest.Id = id.ToString();

                var dishResponse = await _dishesClient.UpdateDishAsync(new UpdateDishRequest()
                {
                    Dish = dishForRequest
                });

                return _mapper.Map<DishResponse>(dishResponse.Dish);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Dishes_Dish_with_id__0__not_found, id));
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new NameAlreadyExistsException(string.Format(Errors.Dishes_Dish_with_name__0__already_exist,
                    dish.Name));
            }
        }

        public async Task DeleteDishAsync(Guid id)
        {
            try
            {
                await _dishesClient.DeleteDishAsync(new DeleteDishRequest() {Id = id.ToString()});
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Dishes_Dish_with_id__0__not_found, id));
            }
        }
    }
}