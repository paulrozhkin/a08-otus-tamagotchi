using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Web.HttpAggregator.Hubs;

namespace Web.HttpAggregator.Consumers
{
    public class KitchenOrderConsumer : IConsumer<KitchenOrderMessage>
    {
        private readonly IHubContext<KitchenOrderHub> _kitchenOrderHubContext;

        public KitchenOrderConsumer(IHubContext<KitchenOrderHub> kitchenOrderHubContext)
        {
            _kitchenOrderHubContext = kitchenOrderHubContext;
        }

        public async Task Consume(ConsumeContext<KitchenOrderMessage> context)
        {
            await _kitchenOrderHubContext.Clients.All.SendAsync("messageReceived", context.Message);
        }
    }
}
