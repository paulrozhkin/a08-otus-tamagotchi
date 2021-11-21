using System;
using System.Threading.Tasks;
using Domain.Core.Models;
using Menu.Domain.Models;

namespace Menu.Domain.Services
{
    public interface IMenuService
    {
        public Task<PagedList<MenuItem>> GetMenuAsync(Guid restaurantId, int pageNumber, int pageSize);

        public Task<MenuItem> GetMenuItemByIdAsync(Guid id);

        public Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);

        public Task<MenuItem> UpdateMenuItem(MenuItem menuItem);

        public Task DeleteMenuItemAsync(Guid id);
    }
}