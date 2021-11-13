using System;
using Domain.Core.Models;

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
        public Guid OrderId { get; set; }

        /// <summary>
        /// Статус заказа кухни
        /// </summary>
        public KitchenOrderStatus Status { get; set; }
    }
}