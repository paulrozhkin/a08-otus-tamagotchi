using Domain.Core.Exceptions;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Microsoft.Extensions.Logging;
using Users.Domain.Models;

namespace Users.Domain.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILogger<UsersService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _usersRepository;

        public UsersService(ILogger<UsersService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _usersRepository = _unitOfWork.Repository<User>();
        }

        public Task<bool> CheckUserCredentials(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var paginationSpecification = new PagedSpecification<User>(pageNumber, pageSize);
            var users = (await _usersRepository.FindAsync(paginationSpecification)).ToList();
            var totalCount = await _usersRepository.CountAsync();

            return new PagedList<User>(users, totalCount, pageNumber, pageSize);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _usersRepository.FindByIdAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(User));
            }

            return user;
        }

        public async Task<User> AddUserAsync(User user)
        {
            //var specification = new UserLocationSpecification(user.Latitude, user.Longitude);
            //var usersWithSameLocation = await _usersRepository.FindAsync(specification);

            //if (usersWithSameLocation.Any())
            //{
            //    _logger.LogError(
            //        $"Can't create user. Users with same location already exist (Lat - {user.Latitude}; Lon - {user.Longitude})");
            //    throw new EntityAlreadyExistsException();
            //}

            await _usersRepository.AddAsync(user);
            _unitOfWork.Complete();

            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            var userWithSameId = await _usersRepository.FindByIdAsync(user.Id);

            if (userWithSameId == null)
            {
                throw new EntityNotFoundException(nameof(User));
            }

            userWithSameId.Name = user.Name;
            userWithSameId.Roles = user.Roles;

            _usersRepository.Update(userWithSameId);
            _unitOfWork.Complete();

            return userWithSameId;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _usersRepository.FindByIdAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(User));
            }

            _usersRepository.Remove(user);
            _unitOfWork.Complete();
        }
    }
}