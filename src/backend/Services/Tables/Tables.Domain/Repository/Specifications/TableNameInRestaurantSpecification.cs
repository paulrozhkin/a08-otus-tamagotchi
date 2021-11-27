using Domain.Core.Repositories.Specifications;
using Tables.Domain.Models;

namespace Tables.Domain.Repository.Specifications
{
    public class TableNameInRestaurantSpecification : BaseSpecification<Table>
    {
        public TableNameInRestaurantSpecification(string tableName, Guid restaurantId) : base(menuItem =>
            menuItem.Name == tableName && menuItem.RestaurantId == restaurantId)
        {
        }
    }
}