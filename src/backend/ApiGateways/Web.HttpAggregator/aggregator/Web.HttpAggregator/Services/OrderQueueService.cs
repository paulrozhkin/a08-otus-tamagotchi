using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.OrderQueue;
using static OrderQueue.API.Protos.KitchenOrders;

namespace Web.HttpAggregator.Services
{
    public class OrderQueueService : IOrderQueueService
    {
        private readonly KitchenOrdersClient _kitchenOrdersClient;
        private readonly ILogger _logger;
        private readonly IOrdersService _ordersService;

        public OrderQueueService(KitchenOrdersClient kitchenOrdersClient, ILogger<OrderQueueService> logger,
            IOrdersService ordersService)
        {
            _kitchenOrdersClient = kitchenOrdersClient;
            _logger = logger;
            _ordersService = ordersService;
        }

        public async Task<IEnumerable<OrderResponse>> GetKitchenOrdersAsync()
        {
            var result = new List<KitchenOrderResponse>();
            var ordersResponse =
                await _kitchenOrdersClient.GetKitchenOrdersAsync(new OrderQueue.API.Protos.GetKitchenOrdersRequest());

            var response = new List<OrderResponse>();

            var menus = new Dictionary<string, MenuItemResponse>();
            var restaurants = new Dictionary<string, RestaurantResponse>();
            var statuses = new Dictionary<string, string>();

            foreach (var ordersResponseKitchenOrder in ordersResponse.KitchenOrders)
            {
                statuses.Add(ordersResponseKitchenOrder.OrderId, ordersResponseKitchenOrder.Status);
            }

            foreach (var orderResponse in ordersResponse.KitchenOrders)
            {
                var order = await _ordersService.GetOrderByIdAsync(Guid.Parse(orderResponse.OrderId), menus, restaurants, statuses);
                response.Add(order);
            }

            return response;
        }
    }
}