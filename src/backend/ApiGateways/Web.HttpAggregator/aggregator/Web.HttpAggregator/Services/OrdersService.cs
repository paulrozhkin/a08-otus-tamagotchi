using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Orders.API;
using Web.HttpAggregator.Models;
using static Orders.API.Orders;
using OrderStatus = Orders.API.OrderStatus;

namespace Web.HttpAggregator.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OrdersClient _ordersClient;
        private ILogger<OrdersService> _logger;
        private readonly IMapper _mapper;
        private readonly IMenuService _menuService;

        public OrdersService(OrdersClient ordersClient, ILogger<OrdersService> logger, IMapper mapper,
            IMenuService menuService)
        {
            _ordersClient = ordersClient;
            _logger = logger;
            _mapper = mapper;
            _menuService = menuService;
        }

        public async Task<PaginationResponse<OrderResponse>> GetOrdersAsync(int pageNumber, int pageSize, Guid userId)
        {
            var request = new GetOrdersRequest()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                UserId = userId.ToString()
            };

            var ordersResponse = await _ordersClient.GetOrdersAsync(request);
            var orders = _mapper.Map<PaginationResponse<OrderResponse>>(ordersResponse);

            var menus = new Dictionary<string, MenuItemResponse>();
            foreach (var orderDto in ordersResponse.Orders)
            {
                var orderId = Guid.Parse(orderDto.Id);
                var orderResponse = orders.Items.First(x => x.Id == orderId);
                await FillRestaurantResponse(orderDto, orderResponse, menus);
            }

            return orders;
        }

        public async Task<OrderResponse> BookRestaurantAsync(OrderRequest order, Guid userId)
        {
            try
            {
                var orderRequest = _mapper.Map<Order>(order);
                orderRequest.ClientId = userId.ToString();
                var orderResponse = await _ordersClient.BookRestaurantAsync(new BookRestauranRequest()
                {
                    Order = orderRequest
                });

                var result = _mapper.Map<OrderResponse>(orderResponse.Order);
                var menus = new Dictionary<string, MenuItemResponse>();
                await FillRestaurantResponse(orderResponse.Order, result, menus);

                return result;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new EntityAlreadyExistsException();
            }
        }

        private async Task FillRestaurantResponse(Order orderDto, OrderResponse orderResponse,
            IDictionary<string, MenuItemResponse> menus)
        {
            foreach (var menuItem in orderDto.Menu)
            {
                if (!menus.ContainsKey(menuItem.Id))
                {
                    var menuItemCache = await _menuService.GetMenuByIdAsync(Guid.Parse(menuItem.Id));
                    menus.Add(menuItem.Id, menuItemCache);
                }

                var menuItemResponse = menus[menuItem.Id];
                orderResponse.Menu.Add(new OrderPositionResponse()
                {
                    MenuItem = menuItemResponse,
                    Count = menuItem.Count
                });
            }

            switch (orderDto.Status)
            {
                case OrderStatus.Created:
                    orderResponse.OrderStatus = Models.OrderStatus.Created;
                    break;
                case OrderStatus.Service:
                    // TODO: invoke service
                    break;
                case OrderStatus.Completed:
                    orderResponse.OrderStatus = Models.OrderStatus.Completed;
                    break;
            }
        }
    }
}