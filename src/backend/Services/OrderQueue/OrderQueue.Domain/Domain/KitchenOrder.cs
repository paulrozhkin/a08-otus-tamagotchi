using System;
using System.Collections.Generic;

namespace OrderQueue.Core.Domain
{
    /// <summary>
    /// Заказ кухни
    /// </summary>
    public class KitchenOrder : BaseEntity
    {
        /// <summary>
        /// Идентификатор заказа из БД сервиса Orders
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Идентификатор ресторана
        /// </summary>
        public int RestaurantId { get; set; }
        /// <summary>
        /// Идентификатор статуса заказа
        /// </summary>
        public int KitchenOrderStatusId { get; set; }
        /// <summary>
        /// Статус заказа
        /// </summary>
        public virtual KitchenOrderStatus Status { get; set; }
        /// <summary>
        /// Блюда в заказе
        /// </summary>
        public virtual ICollection<KitchenOrderDish> Dishes { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
