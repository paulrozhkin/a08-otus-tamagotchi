using System;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IDishesService
    {
        public Task<PaginationResponse<DishResponse>> GetDishesAsync(int pageNumber, int pageSize);

        public Task<DishResponse> GetDishByIdAsync(Guid id);

        public Task<DishResponse> CreateDishAsync(DishRequest dish);

        public Task<DishResponse> UpdateDishAsync(Guid id, DishRequest dish);

        public Task DeleteDishAsync(Guid id);
    }
}