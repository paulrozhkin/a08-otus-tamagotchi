using System;
using System.Linq;
using System.Threading.Tasks;
using DishesApi;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Menu.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Menu.API.Services
{
    public class GrpcDishesService : Dishes.DishesBase
    {
        private readonly ILogger<GrpcDishesService> _logger;
        private readonly IDishesService _dishesService;

        public GrpcDishesService(ILogger<GrpcDishesService> logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
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

            var dishesDto = dishes.Select(x => new Dish()
            {
                Descriptions = x.Description,
                Id = x.Id.ToString(),
                Name = x.Name,
                Photos = { Guid.Empty.ToString() } // Not implemented
            });

            dishesResponse.Dishes.AddRange(dishesDto);

            return dishesResponse;
        }

        public override Task<GetDishResponse> GetDish(GetDishRequest request, ServerCallContext context)
        {
            return base.GetDish(request, context);
        }

        public override Task<CrateDishResponse> CrateDish(CrateDishRequest request, ServerCallContext context)
        {
            return base.CrateDish(request, context);
        }

        public override Task<UpdateDishResponse> UpdateDish(UpdateDishRequest request, ServerCallContext context)
        {
            return base.UpdateDish(request, context);
        }

        public override Task<Empty> DeleteDish(DeleteDishRequest request, ServerCallContext context)
        {
            return base.DeleteDish(request, context);
        }
    }
}