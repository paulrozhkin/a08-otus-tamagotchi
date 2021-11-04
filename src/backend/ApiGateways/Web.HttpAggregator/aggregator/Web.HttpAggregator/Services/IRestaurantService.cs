using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IRestaurantService
    {
        public Task<PaginationResponse<RestaurantResponse>> GetRestaurantsAsync(int pageNumber, int pageSize, string address);

        public Task<RestaurantResponse> GetRestaurantByIdAsync(int id);

        public Task<RestaurantResponse> CreateRestaurant(CreateRestaurantRequest restaurant);
    }
}
