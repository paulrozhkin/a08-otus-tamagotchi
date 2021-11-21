using Domain.Core.Models;

namespace Domain.Core.Repositories.Specifications
{
    public sealed class PagedSpecification<T> : BaseSpecification<T> where T : BaseEntity
    {
        public PagedSpecification(int pageNumber, int pageSize)
        {
            ApplyOrderBy(x => x.Id);
            ApplyPaging(pageNumber, pageSize);
        }
    }
}