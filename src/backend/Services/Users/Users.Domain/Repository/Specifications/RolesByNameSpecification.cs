using Domain.Core.Repositories.Specifications;
using Users.Domain.Models;

namespace Users.Domain.Repository.Specifications;

public sealed class RolesByNameSpecification : BaseSpecification<Role>
{
    public RolesByNameSpecification(IEnumerable<string> roleNames) : base(role =>
        roleNames.Contains(role.Name))
    {
    }
}