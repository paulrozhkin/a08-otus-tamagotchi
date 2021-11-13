using System;

namespace OrderQueue.API.Models.KitchenOrder
{
    public class KitchenOrderResponse
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
