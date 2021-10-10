using AutoMapper;
using MassTransit;
using OrderQueue.Core.Abstractions.Repositories;
using OrderQueue.Core.Domain;
using System;
using System.Threading.Tasks;
using OrderMessages = Infrastructure.Core.Messages.OrderQueue;

namespace OrderQueue.API.Consumers
{
    public class NewKitchenOrderConsumer : IConsumer<OrderMessages.NewKitchenOrder>
    {
        private readonly IRepository<KitchenOrder> _kitchenOrderRepository;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public NewKitchenOrderConsumer(
            IRepository<KitchenOrder> kitchenOrderRepository,
            ISendEndpointProvider sendEndpointProvider,
            IMapper mapper)
        {
            _kitchenOrderRepository = kitchenOrderRepository;
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderMessages.NewKitchenOrder> context)
        {
            var newKitchenOrder = _mapper.Map<KitchenOrder>(context.Message);
            await _kitchenOrderRepository.CreateAsync(newKitchenOrder);
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:kitchen-order"));
            await endpoint.Send(_mapper.Map<OrderMessages.KitchenOrder>(newKitchenOrder));
        }
    }
}