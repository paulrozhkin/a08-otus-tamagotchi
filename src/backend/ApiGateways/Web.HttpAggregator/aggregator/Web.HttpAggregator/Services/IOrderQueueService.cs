using System.Collections.Generic;
using System.Threading.Tasks;
using Web.HttpAggregator.Models.OrderQueue;

namespace Web.HttpAggregator.Services
{
    public interface IOrderQueueService
    {
        Task<IEnumerable<KitchenOrderResponse>> GetKitchenOrders();
    }
}
