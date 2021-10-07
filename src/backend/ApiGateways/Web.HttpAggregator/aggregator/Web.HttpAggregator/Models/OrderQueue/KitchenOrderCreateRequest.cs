using System.Collections.Generic;

namespace Web.HttpAggregator.Models.OrderQueue
{
    public class KitchenOrderCreateRequest
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public List<KitchenOrderDish> Dishes { get; set; }
    }
}
