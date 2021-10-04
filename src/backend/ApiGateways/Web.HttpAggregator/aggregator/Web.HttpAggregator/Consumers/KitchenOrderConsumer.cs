using ExchangeModels;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Web.HttpAggregator.Consumers
{
    public class KitchenOrderConsumer : IConsumer<KitchenOrder>
    {
        public async Task Consume(ConsumeContext<KitchenOrder> context)
        {
            await Console.Out.WriteLineAsync("New kitchen order: " + context.Message.Id);
        }
    }
}
