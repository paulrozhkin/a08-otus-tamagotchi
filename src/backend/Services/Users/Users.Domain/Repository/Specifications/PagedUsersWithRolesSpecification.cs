using Domain.Core.Repositories.Specifications;
using Users.Domain.Models;

namespace Users.Domain.Repository.Specifications;

public sealed class PagedUsersWithRolesSpecification : BaseSpecification<User>
{
    public PagedUsersWithRolesSpecification(int pageNumber, int pageSize)
    {
        ApplyOrderBy(x => x.CreatedDate);
        ApplyPaging(pageNumber, pageSize);
        AddInclude(x => x.Roles);
    }
}