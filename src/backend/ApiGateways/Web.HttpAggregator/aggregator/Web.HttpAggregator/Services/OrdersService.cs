using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using OrdersApi;
using Web.HttpAggregator.Models;
using OrderStatus = Web.HttpAggregator.Models.OrderStatus;

namespace Web.HttpAggregator.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly Orders.OrdersClient _ordersClient;
        private ILogger<OrdersService> _logger;
        private readonly IMapper _mapper;
        private readonly IMenuService _menuService;
        private readonly IRestaurantsService _restaurantsService;

        public OrdersService(Orders.OrdersClient ordersClient, 
            ILogger<OrdersService> logger, IMapper mapper,
            IMenuService menuService,
            IRestaurantsService restaurantsService)
        {
            _ordersClient = ordersClient;
            _logger = logger;
            _mapper = mapper;
            _menuService = menuService;
            _restaurantsService = restaurantsService;
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
            var restaurants = new Dictionary<string, RestaurantResponse>();

            foreach (var orderDto in ordersResponse.Orders)
            {
                var orderId = Guid.Parse(orderDto.Id);
                var orderResponse = orders.Items.First(x => x.Id == orderId);
                await FillRestaurantResponse(orderDto, orderResponse, menus, restaurants);
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
                var restaurants = new Dictionary<string, RestaurantResponse>();

                await FillRestaurantResponse(orderResponse.Order, result, menus, restaurants);

                return result;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new EntityAlreadyExistsException();
            }
        }

        // TODO: Store order in NoSQL
        private async Task FillRestaurantResponse(Order orderDto,
            OrderResponse orderResponse,
            IDictionary<string, MenuItemResponse> menus,
            IDictionary<string, RestaurantResponse> restaurants)
        {
            orderResponse.Menu = new List<OrderPositionResponse>();
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

            if (!restaurants.ContainsKey(orderDto.RestaurantId))
            {
                var restaurantItemCache = await _restaurantsService.GetRestaurantByIdAsync(Guid.Parse(orderDto.RestaurantId));
                restaurants.Add(orderDto.RestaurantId, restaurantItemCache);
            }

            orderResponse.Restaurant = restaurants[orderDto.RestaurantId];

            switch (orderDto.Status)
            {
                case OrdersApi.OrderStatus.Created:
                    orderResponse.OrderStatus = OrderStatus.Created;
                    break;
                case OrdersApi.OrderStatus.Service:
                    // TODO: invoke service
                    break;
                case OrdersApi.OrderStatus.Completed:
                    orderResponse.OrderStatus = OrderStatus.Completed;
                    break;
                case OrdersApi.OrderStatus.Skipped:
                    orderResponse.OrderStatus = OrderStatus.Skipped;
                    break;
            }
        }
    }
}