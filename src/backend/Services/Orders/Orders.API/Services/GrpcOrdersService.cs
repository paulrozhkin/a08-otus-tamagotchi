using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Microsoft.Extensions.Logging;
using Orders.Domain.Services;
using OrdersApi;

namespace Orders.API.Services
{
    public class OrdersService : OrdersApi.Orders.OrdersBase
    {
        private readonly ILogger<OrdersService> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrdersService(ILogger<OrdersService> logger,
            IMapper mapper,
            IOrderService orderService)
        {
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
                
                return response;
            }
            catch (EntityAlreadyExistsException)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
        }


        public override async Task<GetOrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(Guid.Parse(request.Id));
                var response = new GetOrderResponse()
                {
                    Order = _mapper.Map<Order>(order)
                };

                return response;
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Order {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }
    }
}
