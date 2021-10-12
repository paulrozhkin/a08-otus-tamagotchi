namespace Infrastructure.Core.Messages.OrderQueueMessages
{
    public class NewOrderDishMessage
    {
        public string Name { get; set; }
        public int MenuId { get; set; }
    }
}
