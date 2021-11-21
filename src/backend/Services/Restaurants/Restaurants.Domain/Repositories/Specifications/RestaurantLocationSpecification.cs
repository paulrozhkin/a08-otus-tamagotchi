using Domain.Core.Repositories.Specifications;
using Restaurants.Domain.Models;

namespace Restaurants.Domain.Repositories.Specifications
{
    public class RestaurantLocationSpecification : BaseSpecification<Restaurant>
    {
        public RestaurantLocationSpecification(double latitude, double longitude) 
            : base(restaurant => restaurant.Latitude == latitude && restaurant.Longitude == longitude)
        {
        }
    }
}
