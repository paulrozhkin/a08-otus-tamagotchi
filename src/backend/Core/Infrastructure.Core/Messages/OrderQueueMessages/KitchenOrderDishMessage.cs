namespace Infrastructure.Core.Messages.OrderQueueMessages
{
    public class KitchenOrderDishMessage
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public DishStatusMessage StatusMessage { get; set; }
    }
}
