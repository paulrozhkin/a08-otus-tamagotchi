using Domain.Core.Repositories.Specifications;
using Users.Domain.Models;

namespace Users.Domain.Repository.Specifications
{
    public sealed class UsersNameSpecification : BaseSpecification<User>
    {
        public UsersNameSpecification(string username) : base(user =>
            user.Username == username)
        {
            AddInclude((x) => x.Roles);
        }
    }
}