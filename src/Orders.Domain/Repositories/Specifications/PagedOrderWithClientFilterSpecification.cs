using Domain.Core.Repositories.Specifications;
using Orders.Domain.Models;

namespace Orders.Domain.Repositories.Specifications
{
    public sealed class PagedOrderWithClientFilterSpecification : BaseSpecification<Order>
    {
        public PagedOrderWithClientFilterSpecification(Guid clientId, int pageNumber, int pageSize)
            : base(x =>
                x.ClientId == clientId)
        {
            ApplyOrderBy(x => x.CreatedDate);
            ApplyPaging(pageNumber, pageSize);
        }
    }
}
