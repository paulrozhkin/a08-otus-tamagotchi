using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Tables.Domain.Services;
using RestaurantsApi;

namespace Tables.API.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly Restaurants.RestaurantsClient _restaurantsClient;

        public RestaurantsService(Restaurants.RestaurantsClient restaurantsClient)
        {
            _restaurantsClient = restaurantsClient;
        }

        public async Task CheckRestaurantExistAsync(Guid restaurantId)
        {
            try
            {
                await _restaurantsClient.GetRestaurantAsync(new GetRestaurantRequest() {Id = restaurantId.ToString()});
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found,
                    restaurantId));
            }
        }
    }
}
