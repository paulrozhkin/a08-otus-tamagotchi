﻿using Domain.Core.Models;
using Orders.Domain.Models;

namespace Orders.Domain.Services
{
    public interface IOrderService
    {
        public Task<PagedList<Order>> GetOrdersAsync(int pageNumber, int pageSize, Guid clientId);

        public Task<Order> AddOrderAsync(Order order);

        public Task<Order> GetOrderByIdAsync(Guid id);

        Task<Order> SetNewOrderStateAsync(Guid id, OrderStatus newStatus);
    }
}