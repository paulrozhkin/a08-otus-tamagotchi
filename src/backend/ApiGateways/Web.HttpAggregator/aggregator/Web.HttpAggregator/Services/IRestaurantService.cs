using System.Collections.Generic;
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

    class RestaurantService : IRestaurantService
    {
        public Task<PaginationResponse<RestaurantResponse>> GetRestaurantsAsync(int pageNumber, int pageSize, string address)
        {
            var result = new PaginationResponse<RestaurantResponse>()
            {
                CurrentPage = 1,
                PageSize = int.MaxValue,
                TotalCount = 2,
                Items = new List<RestaurantResponse>()
                {
                    new()
                    {
                        Id = 1
                    },
                    new()
                    {
                        Id = 2
                    }
                }
            };

            return Task.FromResult(result);
        }

        public Task<RestaurantResponse> GetRestaurantByIdAsync(int id)
        {
            return Task.FromResult(new RestaurantResponse
            {
                Id = id
            });
        }

        public Task<RestaurantResponse> CreateRestaurant(CreateRestaurantRequest restaurant)
        {
            return Task.FromResult(new RestaurantResponse()
            {
                Id = restaurant.Id
            });
        }
    }
}
