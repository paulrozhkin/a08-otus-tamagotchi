using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IOrdersService
    {
        public Task<PaginationResponse<OrderResponse>> GetOrdersAsync(int pageNumber, int pageSize, Guid userId);

        Task<OrderResponse> BookRestaurantAsync(OrderRequest order, Guid userId);

        Task<OrderResponse> GetOrderByIdAsync(Guid id,
            Dictionary<string, MenuItemResponse> menus,
            IDictionary<string, RestaurantResponse> restaurants,
            IDictionary<string, string> orderStatuses);


        Task<NextStatusResponse> GoToNextStatusAsync(Guid orderId);
    }
}