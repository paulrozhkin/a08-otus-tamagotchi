using System;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Infrastructure.Core.Messages.OrderQueueMessages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Orders.API.Services
{
    public class OrdersService : Orders.OrdersBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<OrdersService> _logger;
        private readonly IMapper _mapper;

        public OrdersService(
            ISendEndpointProvider sendEndpointProvider,
            ILogger<OrdersService> logger,
            IMapper mapper)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<BookRestauranResponse> BookRestaurant(BookRestauranRequest request, ServerCallContext context)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:new-kitchen-order"));
            await endpoint.Send(_mapper.Map<NewKitchenOrderMessage>(request));

            return new BookRestauranResponse {RestaurantId = request.RestaurantId};
        }
    }
}
