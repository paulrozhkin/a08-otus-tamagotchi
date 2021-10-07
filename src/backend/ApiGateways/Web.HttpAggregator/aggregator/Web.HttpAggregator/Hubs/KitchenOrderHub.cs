using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Web.HttpAggregator.Hubs
{
    public class KitchenOrderHub : Hub
    {
        public async Task Send(KitchenOrderHub kitchenOrder)
        {
            await Clients.All.SendAsync("messageReceived", kitchenOrder);
        }
    }
}
