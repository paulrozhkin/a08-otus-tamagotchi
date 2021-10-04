using System;
using System.Collections.Generic;
using Restaurant.API.Models.Dish;

namespace Restaurant.API.Models.KitchenOrder
{
    public class KitchenOrderResponse
    {
        public int Id { get; set; }
        public KitchenOrderStatusResponse Status { get; set; }
        public List<DishResponse> Dishes { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
