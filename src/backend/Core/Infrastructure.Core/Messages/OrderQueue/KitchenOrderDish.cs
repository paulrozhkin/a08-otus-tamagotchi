namespace Infrastructure.Core.Messages.OrderQueue
{
    public class KitchenOrderDish
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public DishStatus Status { get; set; }
    }
}
