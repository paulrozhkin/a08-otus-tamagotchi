using Domain.Core.Repositories.Specifications;
using Users.Domain.Models;

namespace Users.Domain.Repository.Specifications;

public sealed class UsersByIdWithRolesSpecification : BaseSpecification<User>
{
    public UsersByIdWithRolesSpecification(Guid id) : base(user =>
        user.Id == id)
    {
        AddInclude((x) => x.Roles);
    }
}