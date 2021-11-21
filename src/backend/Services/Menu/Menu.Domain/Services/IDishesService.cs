using System;
using System.Threading.Tasks;
using Domain.Core.Models;
using Menu.Domain.Models;

namespace Menu.Domain.Services
{
    public interface IDishesService
    {
        public Task<PagedList<Dish>> GetDishesAsync(int pageNumber, int pageSize);

        public Task<Dish> GetDishByIdAsync(Guid id);

        public Task<Dish> CreateDishAsync(Dish dish);

        public Task<Dish> UpdateDish(Dish dish);

        public Task DeleteDishAsync(Guid id);
    }
}