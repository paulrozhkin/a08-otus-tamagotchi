using System;
using Orders.API;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IOrdersService
    {
        public Task<PaginationResponse<OrderResponse>> GetOrdersAsync(int pageNumber, int pageSize, Guid userId);

        Task<OrderResponse> BookRestaurantAsync(OrderRequest order, Guid userId);

    }
}