using Infrastructure.Core.Messages.OrderQueue;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Web.HttpAggregator.Hubs;

namespace Web.HttpAggregator.Consumers
{
    public class KitchenOrderConsumer : IConsumer<KitchenOrder>
    {
        private readonly IHubContext<KitchenOrderHub> _kitchenOrderHubContext;

        public KitchenOrderConsumer(IHubContext<KitchenOrderHub> kitchenOrderHubContext)
        {
            _kitchenOrderHubContext = kitchenOrderHubContext;
        }

        public async Task Consume(ConsumeContext<KitchenOrder> context)
        {
            await _kitchenOrderHubContext.Clients.All.SendAsync("messageReceived", context.Message);
        }
    }
}
