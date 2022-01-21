using Domain.Core.Models;
using Users.Domain.Models;

namespace Users.Domain.Services
{
    public interface IUsersService
    {
        public Task<User> CheckUserCredentials(string username, string password);

        public Task<PagedList<User>> GetUsersAsync(int pageNumber, int pageSize);

        public Task<User> GetUserByIdAsync(Guid id);

        public Task<User> AddUserAsync(User user);

        public Task<User> UpdateUser(User user);

        public Task DeleteUserAsync(Guid id);
    }
}