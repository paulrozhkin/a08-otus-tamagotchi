using System;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IRestaurantsService
    {
        public Task<PaginationResponse<RestaurantResponse>> GetRestaurantsAsync(int pageNumber, int pageSize, string address);

        public Task<RestaurantResponse> GetRestaurantByIdAsync(Guid id);

        public Task<RestaurantResponse> CreateRestaurantAsync(RestaurantRequest restaurant);

        public Task<RestaurantResponse> UpdateRestaurantAsync(Guid id, RestaurantRequest restaurant);

        public Task DeleteRestaurantAsync(Guid id);
    }
}
