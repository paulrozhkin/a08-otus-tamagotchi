using System;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        Task<PaginationResponse<UserResponse>> GetUsersAsync(int pageNumber, int pageSize);

        Task<UserResponse> GetUserByIdAsync(Guid id);

        public Task<UserResponse> CreateUserAsync(CreateUserRequest user);

        public Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest user);

        public Task DeleteUserAsync(Guid id);
    }
}
