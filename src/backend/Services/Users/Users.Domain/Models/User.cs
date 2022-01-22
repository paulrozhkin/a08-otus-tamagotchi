using Domain.Core.Models;

namespace Users.Domain.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}