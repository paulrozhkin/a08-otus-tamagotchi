using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Infrastructure.Core.Messages.OrderQueueMessages;
using MassTransit;
using OrderQueue.API.Protos;
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
        private readonly KitchenOrders.KitchenOrdersClient _kitchenOrdersClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrdersService(Orders.OrdersClient ordersClient,
            ILogger<OrdersService> logger, IMapper mapper,
            IMenuService menuService,
            IRestaurantsService restaurantsService,
            KitchenOrders.KitchenOrdersClient kitchenOrdersClient,
            ISendEndpointProvider sendEndpointProvider)
        {
            _ordersClient = ordersClient;
            _logger = logger;
            _mapper = mapper;
            _menuService = menuService;
            _restaurantsService = restaurantsService;
            _kitchenOrdersClient = kitchenOrdersClient;
            _sendEndpointProvider = sendEndpointProvider;
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
            var statuses = new Dictionary<string, KitchenStatus>();

            foreach (var orderDto in ordersResponse.Orders)
            {
                var orderId = Guid.Parse(orderDto.Id);
                var orderResponse = orders.Items.First(x => x.Id == orderId);
                await FillRestaurantResponse(orderDto, orderResponse, menus, restaurants, statuses);
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
                var statuses = new Dictionary<string, KitchenStatus>();

                await FillRestaurantResponse(orderResponse.Order, result, menus, restaurants, statuses);

                return result;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new EntityAlreadyExistsException();
            }
        }

        public async Task<OrderResponse> GetOrderByIdAsync(Guid id,
            Dictionary<string, MenuItemResponse> menus,
            IDictionary<string, RestaurantResponse> restaurants,
            IDictionary<string, KitchenStatus> orderStatuses)
        {
            try
            {
                var orderResponse =
                    await _ordersClient.GetOrderAsync(new GetOrderRequest() {Id = id.ToString()});
                var order = _mapper.Map<OrderResponse>(orderResponse.Order);
                await FillRestaurantResponse(orderResponse.Order, order, menus, restaurants, orderStatuses);
                return order;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
            }
        }

        public async Task<NextStatusResponse> GoToNextStatusAsync(Guid orderId)
        {
            var orderResponse = (await _ordersClient.GetOrderAsync(new GetOrderRequest() {Id = orderId.ToString()}))
                .Order;
            var statuses = new Dictionary<string, KitchenStatus>();
            var orderStatus = await GetOrderStatusAsync(orderResponse, statuses);
            var nextStatus = GetNextStatus(orderStatus);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:new-status"));

            switch (nextStatus)
            {
                case OrderStatus.Work:
                    await endpoint.Send(new NewKitchenStatusMessage()
                    {
                        OrderId = orderId,
                        NewStatus = KitchenStatus.Cooking.ToString()
                    });

                    break;
                case OrderStatus.Ready:
                    await endpoint.Send(new NewKitchenStatusMessage()
                    {
                        OrderId = orderId,
                        NewStatus = KitchenStatus.ReadyToServe.ToString()
                    });

                    break;
                case OrderStatus.Completed:
                    await _ordersClient.SetNewOrderStateAsync(new OrdersApi.SetNewOrderStateRequest()
                    {
                        Id = orderResponse.Id,
                        Status = OrdersApi.OrderStatus.Completed
                    });

                    await endpoint.Send(new NewKitchenStatusMessage()
                    {
                        OrderId = orderId,
                        NewStatus = KitchenStatus.Served.ToString()
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new NextStatusResponse() {OrderStatus = nextStatus};
        }

        private OrderStatus GetNextStatus(OrderStatus currentStatus)
        {
            switch (currentStatus)
            {
                case OrderStatus.Wait:
                    return OrderStatus.Work;
                case OrderStatus.Work:
                    return OrderStatus.Ready;
                case OrderStatus.Ready:
                    return OrderStatus.Completed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentStatus), currentStatus, null);
            }
        }

        // TODO: Store order in NoSQL
        private async Task FillRestaurantResponse(Order orderDto,
            OrderResponse orderResponse,
            IDictionary<string, MenuItemResponse> menus,
            IDictionary<string, RestaurantResponse> restaurants,
            IDictionary<string, KitchenStatus> statuses)
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
                var restaurantItemCache =
                    await _restaurantsService.GetRestaurantByIdAsync(Guid.Parse(orderDto.RestaurantId));
                restaurants.Add(orderDto.RestaurantId, restaurantItemCache);
            }

            orderResponse.Restaurant = restaurants[orderDto.RestaurantId];
            orderResponse.OrderStatus = await GetOrderStatusAsync(orderDto, statuses);
        }

        private async Task<OrderStatus> GetOrderStatusAsync(Order orderDto,
            IDictionary<string, KitchenStatus> statuses)
        {
            switch (orderDto.Status)
            {
                case OrdersApi.OrderStatus.Created:
                    return OrderStatus.Created;
                case OrdersApi.OrderStatus.Service:
                    if (!statuses.ContainsKey(orderDto.Id))
                    {
                        var response = await _kitchenOrdersClient.GetKitchenOrderAsync(new GetKitchenOrderRequest()
                        {
                            Id = orderDto.Id
                        });
                        statuses.Add(orderDto.Id, response.KitchenOrder.Status);
                    }

                    var status = statuses[orderDto.Id];

                    switch (status)
                    {
                        case KitchenStatus.Wait:
                            return OrderStatus.Wait;
                        case KitchenStatus.Cooking:
                            return OrderStatus.Work;
                        case KitchenStatus.ReadyToServe:
                            return OrderStatus.Ready;
                        case KitchenStatus.Served:
                            return OrderStatus.Completed;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case OrdersApi.OrderStatus.Completed:
                    return OrderStatus.Completed;
                case OrdersApi.OrderStatus.Skipped:
                    return OrderStatus.Skipped;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderDto.Status));
            }
        }
    }
}