using System.Collections.Generic;
using Domain.Core.Models;

namespace Menu.Domain.Models
{
    public class Dish : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<MenuItem> Menu { get; set; }
    }
}
