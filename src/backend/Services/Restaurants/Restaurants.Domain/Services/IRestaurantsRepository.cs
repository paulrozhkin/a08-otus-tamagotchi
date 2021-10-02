using System.Threading.Tasks;
using Restaurants.Domain.Models;

namespace Restaurants.Domain.Services
{
    public interface IRestaurantsRepository
    {
        Task<PagedList<Restaurant>> GetRestaurantsAsync(int pageNumber, int pageSize);

        public Task<Restaurant> GetRestaurantByIdAsync(int id);

        public Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
    }
}
