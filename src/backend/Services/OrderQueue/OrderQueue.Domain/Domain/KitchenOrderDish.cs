namespace OrderQueue.Core.Domain
{
    /// <summary>
    /// Блюдо в заказе кухни
    /// </summary>
    public class KitchenOrderDish : BaseEntity
    {
        /// <summary>
        /// Название блюда
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Идентификатор блюда в БД сервиса Restaurants
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// Идентификатор статуса блюда
        /// </summary>
        public int DishStatusId { get; set; }
        /// <summary>
        /// Статус блюда в заказе
        /// </summary>
        public virtual DishStatus Status { get; set; }
    }
}
