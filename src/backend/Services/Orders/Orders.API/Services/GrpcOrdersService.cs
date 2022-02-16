using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Infrastructure.Core.Messages.OrderQueueMessages;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orders.Domain.Services;
using OrdersApi;

namespace Orders.API.Services
{
    public class OrdersService : OrdersApi.Orders.OrdersBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<OrdersService> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrdersService(
            ISendEndpointProvider sendEndpointProvider,
            ILogger<OrdersService> logger,
            IMapper mapper,
            IOrderService orderService)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _logger = logger;
            _mapper = mapper;
            _orderService = orderService;
        }

        public override async Task<GetOrdersResponse> GetOrders(GetOrdersRequest request,
            ServerCallContext context)
        {
            var restaurants =
                await _orderService.GetOrdersAsync(request.PageNumber, request.PageSize, Guid.Parse(request.UserId));

            var response = new GetOrdersResponse
            {
                CurrentPage = restaurants.CurrentPage,
                PageSize = restaurants.PageSize,
                TotalCount = restaurants.TotalCount
            };

            var restaurantsDto = _mapper.Map<List<Order>>(restaurants);
            response.Orders.Add(restaurantsDto);

            return response;
        }

        public override async Task<BookRestauranResponse> BookRestaurant(BookRestauranRequest request,
            ServerCallContext context)
        {
            var orderModel = _mapper.Map<Domain.Models.Order>(request);
            
            try
            {
                var restaurant = await _orderService.AddOrderAsync(orderModel);

                var response = new BookRestauranResponse
                {
                    Order = _mapper.Map<Order>(restaurant)
                };

                //var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:new-kitchen-order"));
                //await endpoint.Send(_mapper.Map<NewKitchenOrderMessage>(request));

                return response;
            }
            catch (EntityAlreadyExistsException)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
        }


    }
}
