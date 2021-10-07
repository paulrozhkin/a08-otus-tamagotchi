using OrderQueue.API.Models.Dish;
using System;
using System.Collections.Generic;

namespace OrderQueue.API.Models.KitchenOrder
{
    public class KitchenOrderResponse
    {
        public int Id { get; set; }
        public KitchenOrderStatusResponse Status { get; set; }
        public List<DishResponse> Dishes { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
