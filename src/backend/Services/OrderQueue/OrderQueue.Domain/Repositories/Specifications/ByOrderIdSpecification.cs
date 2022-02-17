using System;
using Domain.Core.Repositories.Specifications;
using OrderQueue.Core.Models;

namespace OrderQueue.Core.Repositories.Specifications;

public sealed class ByOrderIdSpecification : BaseSpecification<KitchenOrder>
{
    public ByOrderIdSpecification(Guid orderId)
        : base(x =>
            x.OrderId == orderId)
    {
    }
}