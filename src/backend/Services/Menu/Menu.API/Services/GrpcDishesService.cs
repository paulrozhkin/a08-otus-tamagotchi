using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DishesApi;
using Domain.Core.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Menu.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Menu.API.Services
{
    public class GrpcDishesService : Dishes.DishesBase
    {
        private readonly ILogger<GrpcDishesService> _logger;
        private readonly IDishesService _dishesService;
        private readonly IMapper _mapper;

        public GrpcDishesService(ILogger<GrpcDishesService> logger,
            IDishesService dishesService,
            IMapper mapper)
        {
            _logger = logger;
            _dishesService = dishesService;
            _mapper = mapper;
        }

        public override async Task<GetDishesResponse> GetDishes(GetDishesRequest request, ServerCallContext context)
        {
            var dishes = await _dishesService.GetDishesAsync(request.PageNumber, request.PageSize);

            var dishesResponse = new GetDishesResponse()
            {
                CurrentPage = dishes.CurrentPage,
                PageSize = dishes.PageSize,
                TotalCount = dishes.TotalCount
            };

            var dishesDto = dishes.Select(dish => new Dish()
            {
                Description = dish.Description,
                Id = dish.Id.ToString(),
                Name = dish.Name,
                Photos = {Guid.Empty.ToString()}, // Not implemented,
                CreatedDate = Timestamp.FromDateTimeOffset(dish.CreatedDate),
                UpdatedDate = Timestamp.FromDateTimeOffset(dish.UpdatedDate)
            });

            dishesResponse.Dishes.AddRange(dishesDto);

            return dishesResponse;
        }

        public override async Task<GetDishResponse> GetDish(GetDishRequest request, ServerCallContext context)
        {
            try
            {
                var dish = await _dishesService.GetDishByIdAsync(Guid.Parse(request.Id));
                var dishDto = _mapper.Map<Dish>(dish);

                //var dishDto = new Dish()
                //{
                //    Descriptions = dish.Description,
                //    Id = dish.Id.ToString(),
                //    Name = dish.Name,
                //    Photos = {Guid.Empty.ToString()}, // Not implemented,
                //    CreatedDate = Timestamp.FromDateTimeOffset(dish.CreatedDate),
                //    UpdatedDate = Timestamp.FromDateTimeOffset(dish.UpdatedDate)
                //};

                var dishResponse = new GetDishResponse()
                {
                    Dish = dishDto
                };

                return dishResponse;
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Dish {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<CrateDishResponse> CrateDish(CrateDishRequest request, ServerCallContext context)
        {
            var createDishDto = request.Dish;
            var dishModel = new Domain.Models.Dish()
            {
                Description = createDishDto.Description,
                Name = createDishDto.Name
            };

            Domain.Models.Dish createdDish;

            try
            {
                createdDish = await _dishesService.CreateDishAsync(dishModel);
            }
            catch (NameAlreadyExistsException)
            {
                _logger.LogError(string.Format(Errors.Dishes_Dish_with_name__0__already_exist, dishModel.Name));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Dishes_Dish_already_exits));
            }

            return new CrateDishResponse()
            {
                Dish = new Dish()
                {
                    Id = createdDish.Id.ToString(),
                    Description = createdDish.Description,
                    Name = createdDish.Name,
                    CreatedDate = Timestamp.FromDateTimeOffset(createdDish.CreatedDate),
                    UpdatedDate = Timestamp.FromDateTimeOffset(createdDish.UpdatedDate)
                }
            };
        }

        public override async Task<UpdateDishResponse> UpdateDish(UpdateDishRequest request, ServerCallContext context)
        {
            var updateDishDto = request.Dish;
            var dishModel = new Domain.Models.Dish()
            {
                Description = updateDishDto.Description,
                Name = updateDishDto.Name,
                Id = Guid.Parse(updateDishDto.Id)
            };

            Domain.Models.Dish updateDish;

            try
            {
                updateDish = await _dishesService.UpdateDish(dishModel);
            }
            catch (NameAlreadyExistsException)
            {
                _logger.LogError(string.Format(Errors.Dishes_Dish_with_name__0__already_exist, dishModel.Name));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Dishes_Dish_already_exits));
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Dish {dishModel.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }

            return new UpdateDishResponse()
            {
                Dish = new Dish()
                {
                    Id = updateDish.Id.ToString(),
                    Description = updateDish.Description,
                    Name = updateDish.Name,
                    CreatedDate = Timestamp.FromDateTimeOffset(updateDish.CreatedDate),
                    UpdatedDate = Timestamp.FromDateTimeOffset(updateDish.UpdatedDate)
                }
            };
        }

        public override async Task<Empty> DeleteDish(DeleteDishRequest request, ServerCallContext context)
        {
            var idForDelete = request.Id;

            try
            {
                await _dishesService.DeleteDishAsync(Guid.Parse(idForDelete));
                return new Empty();
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Dish {idForDelete}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }
    }
}