using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.HttpAggregator.Models.OrderQueue;
using static OrderQueue.API.Protos.KitchenOrders;

namespace Web.HttpAggregator.Services
{
    public class OrderQueueService : IOrderQueueService
    {
        private readonly KitchenOrdersClient _kitchenOrdersClient;
        private readonly ILogger _logger;

        public OrderQueueService(KitchenOrdersClient kitchenOrdersClient, ILogger<OrderQueueService> logger)
        {
            _kitchenOrdersClient = kitchenOrdersClient;
            _logger = logger;
        }

        public async Task<IEnumerable<KitchenOrderResponse>> GetKitchenOrdersAsync()
        {
            var result = new List<KitchenOrderResponse>();
            var ordersResponse = await _kitchenOrdersClient.GetKitchenOrdersAsync(new OrderQueue.API.Protos.GetKitchenOrdersRequest());
            if (ordersResponse != null)
            {
                result = ordersResponse.KitchenOrders.Select(x => new KitchenOrderResponse
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    Status = x.Status,
                    CreateDate = x.CreateDate
                }).ToList();
            }
            return result;
        }
    }
}
