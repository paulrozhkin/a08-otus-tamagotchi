using Grpc.Core;
using Microsoft.Extensions.Logging;
using OrderQueue.API.Protos;
using OrderQueue.Core.Abstractions.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace OrderQueue.API.Services
{
    public class KitchenOrdersService : KitchenOrders.KitchenOrdersBase
    {
        private readonly ILogger<KitchenOrdersService> _logger;
        private readonly IRepository<Core.Domain.KitchenOrder> _kitchenOrderRepository;

        public KitchenOrdersService(ILogger<KitchenOrdersService> logger, IRepository<Core.Domain.KitchenOrder> kitchenOrderRepository)
        {
            _logger = logger;
            _kitchenOrderRepository = kitchenOrderRepository;
        }

        public override async Task<GetKitchenOrdersResponse> GetKitchenOrders(GetKitchenOrdersRequest request, ServerCallContext context)
        {
            var orders = await _kitchenOrderRepository.GetAllAsync();
            var orderDtos = orders.Select(x => new KitchenOrder
            {
                Id = x.Id.ToString(),
                OrderId = x.OrderId.ToString(),
                Status = x.Status.ToString(),
                CreateDate = x.CreateDate.ToString()
            });
            var response = new GetKitchenOrdersResponse();
            response.KitchenOrders.Add(orderDtos);
            return response;
        }
    }
}
