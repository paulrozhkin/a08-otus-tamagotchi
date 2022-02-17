using Domain.Core.Models;

namespace Orders.Domain.Models
{
    public class Order : BaseEntity
    {
        public Guid RestaurantId { get; set; }

        public int NumberOfPersons { get; set; }

        public DateTimeOffset VisitTime { get; set; }

        public string Comment { get; set; }

        public int AmountOfRubles { get; set; }

        public OrderStatus Status { get; set; }

        public Guid ClientId { get; set; }

        public List<MenuPosition> Menu { get; set; }
    }
}