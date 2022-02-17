using Domain.Core.Repositories.Specifications;
using Orders.Domain.Models;

namespace Orders.Domain.Repositories.Specifications
{
    public sealed class OrderWithMenuSpecification : BaseSpecification<Order>
    {
        public OrderWithMenuSpecification(Guid orderId) : base(order =>
            order.Id == orderId)
        {
            AddInclude(x => x.Menu);
        }
    }
}