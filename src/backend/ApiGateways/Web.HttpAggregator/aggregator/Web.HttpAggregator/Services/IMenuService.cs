using System;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IMenuService
    {
        public Task<PaginationResponse<MenuItemResponse>> GetMenuAsync(Guid restaurantId, int pageNumber, int pageSize);

        public Task<MenuItemResponse> GetMenuByIdAsync(Guid menuItemId);

        public Task<MenuItemResponse> CreateMenuAsync(Guid restaurantId, MenuItemRequest menu);

        public Task<MenuItemResponse> UpdateMenu(Guid menuItemId, MenuItemRequest menu);

        public Task DeleteMenuAsync(Guid menuItemId);
    }
}