using Domain.Core.Repositories.Specifications;
using Orders.Domain.Models;

namespace Orders.Domain.Repositories.Specifications;

public sealed class VisitTimeAndStatusSpecification : BaseSpecification<Order>
{
    public VisitTimeAndStatusSpecification(DateTimeOffset visitTime, OrderStatus orderStatus)
        : base(x =>
            x.VisitTime <= visitTime && x.Status == orderStatus)
    {
    }
}