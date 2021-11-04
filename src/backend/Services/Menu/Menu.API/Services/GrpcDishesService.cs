using System.Threading.Tasks;
using DishesApi;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Menu.API.Services
{
    public class GrpcDishesService : Dishes.DishesBase
    {
        private readonly ILogger<GrpcDishesService> _logger;

        public GrpcDishesService(ILogger<GrpcDishesService> logger)
        {
            _logger = logger;
        }

        public override Task<GetDishesResponse> GetDishes(GetDishesRequest request, ServerCallContext context)
        {
            return base.GetDishes(request, context);
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