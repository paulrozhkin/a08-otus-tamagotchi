using Domain.Core.Models;

namespace Tables.Domain.Models
{
    public class Table : BaseEntity
    {
        public string Name { get; set; } = null!;

        public int NumberOfPlaces { get; set; }

        public Guid RestaurantId { get; set; }
    }
}