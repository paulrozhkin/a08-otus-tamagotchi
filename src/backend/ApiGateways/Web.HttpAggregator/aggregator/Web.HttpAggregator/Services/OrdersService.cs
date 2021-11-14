using System;
using Microsoft.Extensions.Logging;
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

        public async Task<Orders.API.BookRestauranResponse> BookRestaurantAsync(Guid restaurantId)
        {
            var restaurantResult = await _ordersClient
                .BookRestaurantAsync(new Orders.API.BookRestauranRequest { RestaurantId = restaurantId.ToString() });

            return restaurantResult;
        }
    }
}
