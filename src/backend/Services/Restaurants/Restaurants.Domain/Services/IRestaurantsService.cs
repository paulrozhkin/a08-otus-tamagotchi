using System.Threading.Tasks;
using Restaurants.Domain.Models;

namespace Restaurants.Domain.Services
{
    public interface IRestaurantsService
    {
        public Task<PagedList<Restaurant>> GetRestaurantsAsync(int pageNumber, int pageSize, string address);

        public Task<Restaurant> GetRestaurantByIdAsync(int id);

        public Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
    }
}
