using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;

namespace Menu.Domain.Repositories.Specifications
{
    public class DishNameSpecification : BaseSpecification<Dish>
    {
        public DishNameSpecification(string name) : base(dish => dish.Name == name)
        {
        }
    }
}
