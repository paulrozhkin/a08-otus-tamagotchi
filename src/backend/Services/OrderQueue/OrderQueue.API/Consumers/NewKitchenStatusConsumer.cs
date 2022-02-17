using AutoMapper;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Repositories;
using Infrastructure.Core.Messages.OrderQueueMessages;
using OrderQueue.Core.Models;
using OrderQueue.Core.Repositories.Specifications;

namespace OrderQueue.API.Consumers
{
    public class NewKitchenStatusConsumer : IConsumer<NewKitchenStatusMessage>
    {
        private readonly IUnitOfWork _kitchenOrderUnitOfWork;
        private readonly IRepository<KitchenOrder> _kitchenOrderRepository;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public NewKitchenStatusConsumer(
            IUnitOfWork kitchenOrderUnitOfWork,
            ISendEndpointProvider sendEndpointProvider,
            IMapper mapper)
        {
            _kitchenOrderUnitOfWork = kitchenOrderUnitOfWork;
            _kitchenOrderRepository = _kitchenOrderUnitOfWork.Repository<KitchenOrder>();
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<NewKitchenStatusMessage> context)
        {
            var orderIdSpecification = new ByOrderIdSpecification(context.Message.OrderId);
            var order = (await _kitchenOrderRepository.FindAsync(orderIdSpecification)).First();
            order.Status = Enum.Parse<KitchenOrderStatus>(context.Message.NewStatus);
            _kitchenOrderRepository.Update(order);
            _kitchenOrderUnitOfWork.Complete();

            //var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:kitchen-order"));
            //await endpoint.Send(_mapper.Map<KitchenOrderMessage>(newKitchenOrder));
        }
    }
}