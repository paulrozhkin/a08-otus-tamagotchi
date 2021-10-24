using System;
using System.Collections.Generic;

namespace Infrastructure.Core.Messages.OrderQueueMessages
{
    public class KitchenOrderMessage
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreateTime { get; set; }
        public KitchenOrderStatusMessage StatusMessage { get; set; }
        public List<KitchenOrderDishMessage> Dishes { get; set; }
    }
}