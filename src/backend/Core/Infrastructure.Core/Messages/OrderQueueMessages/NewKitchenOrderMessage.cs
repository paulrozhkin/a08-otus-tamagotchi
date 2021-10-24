using System.Collections.Generic;

namespace Infrastructure.Core.Messages.OrderQueueMessages
{
    public class NewKitchenOrderMessage
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public List<NewOrderDishMessage> Dishes { get; set; }
    }
}
