using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Repositories;
using Infrastructure.Core.Messages.OrderQueueMessages;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Orders.Domain.Models;
using Orders.Domain.Repositories.Specifications;

namespace Orders.API.Services
{
    public class OrdersStatusUpdater
    {
        public const string JobId = nameof(OrdersStatusUpdater);
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public OrdersStatusUpdater(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public async Task UpdateStatusForOrders()
        {
            var scope = _serviceScopeFactory.CreateScope();

            using var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            Debug.Assert(unitOfWork != null, nameof(unitOfWork) + " != null");
            var orderRepository = unitOfWork.Repository<Order>();

            var currentTime = DateTimeOffset.Now;
            var nextTime = currentTime.AddHours(1);

            var ordersForWork =
                await GetOrdersByVisitTimeAndStateAsync(orderRepository, nextTime, OrderStatus.Created);

            foreach (var order in ordersForWork)
            {
                if (order.VisitTime < currentTime)
                {
                    order.Status = OrderStatus.Skipped;
                }
                else
                {
                    order.Status = OrderStatus.Service;
                    var endpoint = await scope.ServiceProvider.GetService<ISendEndpointProvider>()!.GetSendEndpoint(new Uri("queue:new-kitchen-order"));
                    await endpoint.Send(_mapper.Map<NewKitchenOrderMessage>(order));
                }

                orderRepository.Update(order);
            }

            unitOfWork.Complete();
        }

        public async Task<List<Order>> GetOrdersByVisitTimeAndStateAsync(IRepository<Order> orderRepository, DateTimeOffset visitTime, OrderStatus status)
        {
            var specification = new VisitTimeAndStatusSpecification(visitTime, status);
            var orders = (await orderRepository.FindAsync(specification)).ToList();

            return orders;
        }
    }
}
