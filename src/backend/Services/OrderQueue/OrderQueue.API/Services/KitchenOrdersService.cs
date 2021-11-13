using Grpc.Core;
using Microsoft.Extensions.Logging;
using OrderQueue.API.Protos;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Repositories;

namespace OrderQueue.API.Services
{
    public class KitchenOrdersService : KitchenOrders.KitchenOrdersBase
    {
        private readonly ILogger<KitchenOrdersService> _logger;
        private readonly IRepository<Core.Domain.KitchenOrder> _kitchenOrderRepository;

        public KitchenOrdersService(ILogger<KitchenOrdersService> logger, IUnitOfWork kitchenOrderUnitOfWork)
        {
            _logger = logger;
            _kitchenOrderRepository = kitchenOrderUnitOfWork.Repository<Core.Domain.KitchenOrder>();
        }

        public override async Task<GetKitchenOrdersResponse> GetKitchenOrders(GetKitchenOrdersRequest request, ServerCallContext context)
        {
            var orders = await _kitchenOrderRepository.FindAsync();
            var ordersDto = orders.Select(x => new KitchenOrder
            {
                Id = x.Id.ToString(),
                OrderId = x.OrderId.ToString(),
                Status = x.Status.ToString(),
                CreateDate = x.CreatedDate.ToString()
            });
            var response = new GetKitchenOrdersResponse();
            response.KitchenOrders.Add(ordersDto);
            return response;
        }
    }
}
