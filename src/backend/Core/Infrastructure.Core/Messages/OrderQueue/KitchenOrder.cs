using System;
using System.Collections.Generic;

namespace Infrastructure.Core.Messages.OrderQueue
{
    public class KitchenOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreateTime { get; set; }
        public KitchenOrderStatus Status { get; set; }
        public List<KitchenOrderDish> Dishes { get; set; }
    }
}
