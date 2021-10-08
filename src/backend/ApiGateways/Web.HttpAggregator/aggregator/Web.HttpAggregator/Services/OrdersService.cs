using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Orders.API.Orders;

namespace Web.HttpAggregator.Services
{
    public class OrdersService : IOrdersService
    {
        private OrdersClient _ordersClient;
        private ILogger logger;

        public OrdersService(OrdersClient ordersClient, ILogger<OrdersService> logger)
        {
            _ordersClient = ordersClient;
            this.logger = logger;
        }

        public async Task<Orders.API.BookRestauranResponse> BookRestaurant(int restaurantId)
        {
            var restaurantResult = await _ordersClient
                .BookRestaurantAsync(new Orders.API.BookRestauranRequest { RestaurantId = restaurantId });

            return restaurantResult;
        }
    }
}
