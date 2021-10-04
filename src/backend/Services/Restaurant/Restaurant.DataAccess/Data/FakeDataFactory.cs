using Restaurant.Core.Domain;
using System.Collections.Generic;

namespace Restaurant.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static IEnumerable<KitchenOrderStatus> KitchenOrderStatuses => new List<KitchenOrderStatus>
        {
            new KitchenOrderStatus { Name = "Новый" },
            new KitchenOrderStatus { Name = "В работе" },
            new KitchenOrderStatus { Name = "Выполнен" }
        };

        public static IEnumerable<DishStatus> DishStatuses => new List<DishStatus>
        {
            new DishStatus { Name = "Готовится" },
            new DishStatus { Name = "Готово к подаче" },
            new DishStatus { Name = "Подано" }
        };
    }
}
