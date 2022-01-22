using Domain.Core.Repositories.Specifications;
using Users.Domain.Models;

namespace Users.Domain.Repository.Specifications
{
    public sealed class UsersNameWithRolesSpecification : BaseSpecification<User>
    {
        public UsersNameWithRolesSpecification(string username) : base(user =>
            user.Username == username)
        {
            AddInclude((x) => x.Roles);
        }
    }
}