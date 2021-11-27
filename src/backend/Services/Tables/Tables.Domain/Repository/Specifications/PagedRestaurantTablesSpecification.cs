using Domain.Core.Repositories.Specifications;
using Tables.Domain.Models;

namespace Tables.Domain.Repository.Specifications
{
    public sealed class PagedRestaurantTablesSpecification : BaseSpecification<Table>
    {
        public PagedRestaurantTablesSpecification(Guid restaurantId, int pageNumber, int pageSize)
            : base(x =>
                x.RestaurantId == restaurantId)
        {
            ApplyOrderBy(x => x.CreatedDate);
            ApplyPaging(pageNumber, pageSize);
        }
    }
}