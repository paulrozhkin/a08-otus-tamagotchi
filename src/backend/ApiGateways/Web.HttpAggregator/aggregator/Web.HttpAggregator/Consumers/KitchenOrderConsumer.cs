using System.Collections.Generic;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Web.HttpAggregator.Hubs;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Consumers
{
    public class KitchenOrderConsumer : IConsumer<KitchenOrderMessage>
    {
        private readonly IHubContext<KitchenOrderHub> _kitchenOrderHubContext;
        private readonly IOrdersService _ordersService;

        public KitchenOrderConsumer(IHubContext<KitchenOrderHub> kitchenOrderHubContext, IOrdersService ordersService)
        {
            _kitchenOrderHubContext = kitchenOrderHubContext;
            _ordersService = ordersService;
        }

        public async Task Consume(ConsumeContext<KitchenOrderMessage> context)
        {
            var menus = new Dictionary<string, MenuItemResponse>();
            var restaurants = new Dictionary<string, RestaurantResponse>();
            var statuses = new Dictionary<string, string>();

            var order = await _ordersService.GetOrderByIdAsync(context.Message.OrderId, menus, restaurants, statuses);
            await _kitchenOrderHubContext.Clients.All.SendAsync("messageReceived", order);
        }
    }
}
