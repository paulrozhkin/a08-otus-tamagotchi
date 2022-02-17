using Domain.Core.Models;
using Users.Domain.Models;

namespace Users.Domain.Services
{
    public interface IUsersService
    {
        public Task<User> CheckUserCredentials(string username, string password);

        public Task<PagedList<User>> GetUsersAsync(int pageNumber, int pageSize);

        public Task<User> GetUserByIdAsync(Guid id);

        public Task<User> AddUserAsync(User user, string password, IEnumerable<string> roleNames);

        public Task<User> UpdateUser(User user, IEnumerable<string> roleNames);

        public Task DeleteUserAsync(Guid id);
    }
}