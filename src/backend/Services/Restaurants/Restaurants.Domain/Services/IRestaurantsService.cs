using System;
using System.Threading.Tasks;
using Domain.Core.Models;
using Restaurants.Domain.Models;

namespace Restaurants.Domain.Services
{
    public interface IRestaurantsService
    {
        public Task<PagedList<Restaurant>> GetRestaurantsAsync(int pageNumber, int pageSize, string address);

        public Task<Restaurant> GetRestaurantByIdAsync(Guid id);

        public Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
    }
}