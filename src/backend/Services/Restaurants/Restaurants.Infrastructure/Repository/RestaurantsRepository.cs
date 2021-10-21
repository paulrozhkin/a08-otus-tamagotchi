using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Models;
using Restaurants.Domain.Services;

namespace Restaurants.Infrastructure.Repository
{
    public class RestaurantsRepository : IRestaurantsRepository
    {
        private readonly RestaurantsDataContext _dataContext;

        public RestaurantsRepository(RestaurantsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<PagedList<Restaurant>> GetRestaurantsAsync(int pageNumber, int pageSize)
        {
            return Task.FromResult(PagedList<Restaurant>.ToPagedList(_dataContext.Restaurants.OrderBy(on => on.Id),
                pageNumber,
                pageSize));
        }

        public Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            return _dataContext.Restaurants.FirstAsync(x => x.Id == id);
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        { 
            await _dataContext.AddAsync(restaurant);
            await _dataContext.SaveChangesAsync();
            return restaurant;
        }
    }
}
