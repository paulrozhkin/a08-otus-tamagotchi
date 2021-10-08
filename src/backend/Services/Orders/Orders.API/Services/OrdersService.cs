using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Infrastructure.Core.OrderQueue;

namespace Orders.API
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
            await endpoint.Send(_mapper.Map<NewKitchenOrder>(request));

            return new BookRestauranResponse {RestaurantId = request.RestaurantId};
        }
    }
}
