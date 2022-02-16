using Domain.Core.Models;

namespace Orders.Domain.Models;

public class MenuPosition : BaseEntity
{
    public Guid MenuItemId { get; set; }

    public int Count { get; set; }
}