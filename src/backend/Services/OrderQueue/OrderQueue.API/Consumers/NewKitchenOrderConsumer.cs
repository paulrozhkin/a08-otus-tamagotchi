using AutoMapper;
using MassTransit;
using OrderQueue.Core.Domain;
using System;
using System.Threading.Tasks;
using Domain.Core.Repositories;
using Infrastructure.Core.Messages.OrderQueueMessages;

namespace OrderQueue.API.Consumers
{
    public class NewKitchenOrderConsumer : IConsumer<NewKitchenOrderMessage>
    {
        private readonly IUnitOfWork _kitchenOrderUnitOfWork;
        private readonly IRepository<KitchenOrder> _kitchenOrderRepository;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public NewKitchenOrderConsumer(
            IUnitOfWork kitchenOrderUnitOfWork,
            ISendEndpointProvider sendEndpointProvider,
            IMapper mapper)
        {
            _kitchenOrderUnitOfWork = kitchenOrderUnitOfWork;
            _kitchenOrderRepository = _kitchenOrderUnitOfWork.Repository<KitchenOrder>();
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<NewKitchenOrderMessage> context)
        {
            var newKitchenOrder = _mapper.Map<KitchenOrder>(context.Message);
            await _kitchenOrderRepository.AddAsync(newKitchenOrder);
            _kitchenOrderUnitOfWork.Complete();
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:kitchen-order"));
            await endpoint.Send(_mapper.Map<KitchenOrderMessage>(newKitchenOrder));
        }
    }
}