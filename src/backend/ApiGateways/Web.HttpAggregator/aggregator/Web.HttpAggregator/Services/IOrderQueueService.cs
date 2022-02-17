using System.Collections.Generic;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IOrderQueueService
    {
        Task<IEnumerable<OrderResponse>> GetKitchenOrdersAsync();
    }
}
