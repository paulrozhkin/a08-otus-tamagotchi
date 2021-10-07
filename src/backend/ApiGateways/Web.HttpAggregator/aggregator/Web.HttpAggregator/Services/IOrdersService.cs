using Orders.API;
using System.Threading.Tasks;

namespace Web.HttpAggregator.Services
{
    public interface IOrdersService
    {
        Task<BookRestauranResponse> BookRestaurant(int restaurantId);
    }
}