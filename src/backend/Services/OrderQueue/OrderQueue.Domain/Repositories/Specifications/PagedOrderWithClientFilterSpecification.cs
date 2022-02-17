using Domain.Core.Repositories.Specifications;
using OrderQueue.Core.Models;

namespace OrderQueue.Core.Repositories.Specifications
{
    public sealed class WithoutServedSpecification : BaseSpecification<KitchenOrder>
    {
        public WithoutServedSpecification()
            : base(x =>
                x.Status != KitchenOrderStatus.Served)
        {
            ApplyOrderBy(x => x.CreatedDate);
        }
    }
}