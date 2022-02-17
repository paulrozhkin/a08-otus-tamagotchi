using System;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OrderQueue.API.Protos;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Repositories;
using OrderQueue.Core.Models;
using OrderQueue.Core.Repositories.Specifications;
using KitchenOrder = OrderQueue.API.Protos.KitchenOrder;

namespace OrderQueue.API.Services
{
    public class KitchenOrdersService : KitchenOrders.KitchenOrdersBase
    {
        private readonly ILogger<KitchenOrdersService> _logger;
        private readonly IRepository<Core.Models.KitchenOrder> _kitchenOrderRepository;

        public KitchenOrdersService(ILogger<KitchenOrdersService> logger, IUnitOfWork kitchenOrderUnitOfWork)
        {
            _logger = logger;
            _kitchenOrderRepository = kitchenOrderUnitOfWork.Repository<Core.Models.KitchenOrder>();
        }

        public override async Task<GetKitchenOrdersResponse> GetKitchenOrders(GetKitchenOrdersRequest request, ServerCallContext context)
        {
            var spec = new WithoutServedSpecification();
            var orders = await _kitchenOrderRepository.FindAsync(spec);
            var ordersDto = orders.Select(x => new KitchenOrder
            {
                Id = x.Id.ToString(),
                OrderId = x.OrderId.ToString(),
                Status = ModelToDtoStatusMap(x.Status),
                CreateDate = x.CreatedDate.ToString()
            });

            var response = new GetKitchenOrdersResponse();
            response.KitchenOrders.Add(ordersDto);
            return response;
        }

        public override async Task<GetKitchenOrderResponse> GetKitchenOrder(GetKitchenOrderRequest request, ServerCallContext context)
        {
            var orderIdSpecification = new ByOrderIdSpecification(Guid.Parse(request.Id));
            var order = (await _kitchenOrderRepository.FindAsync(orderIdSpecification)).First();
            return new GetKitchenOrderResponse()
            {
                KitchenOrder = new KitchenOrder()
                {
                    Id = order.Id.ToString(),
                    OrderId = order.OrderId.ToString(),
                    Status = ModelToDtoStatusMap(order.Status),
                    CreateDate = order.CreatedDate.ToString()
                }
            };
        }

        public override Task<SetNewOrderStateResponse> SetNewOrderState(SetNewOrderStateRequest request, ServerCallContext context)
        {
            return base.SetNewOrderState(request, context);
        }

        private KitchenStatus ModelToDtoStatusMap(KitchenOrderStatus status)
        {
            switch (status)
            {
                case KitchenOrderStatus.Wait:
                    return KitchenStatus.Wait;
                case KitchenOrderStatus.Cooking:
                    return KitchenStatus.Cooking;
                case KitchenOrderStatus.ReadyToServe:
                    return KitchenStatus.ReadyToServe;
                case KitchenOrderStatus.Served:
                    return KitchenStatus.Served;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
