using Domain.Core.Exceptions;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Microsoft.Extensions.Logging;
using Users.Domain.Models;
using Users.Domain.Repository.Specifications;

namespace Users.Domain.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILogger<UsersService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _usersRepository;
        private readonly IRepository<Role> _rolesRepository;

        public UsersService(ILogger<UsersService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _usersRepository = _unitOfWork.Repository<User>();
            _rolesRepository = _unitOfWork.Repository<Role>();
        }

        public async Task<User> CheckUserCredentials(string username, string password)
        {
            var nameSpecification = new UsersNameWithRolesSpecification(username);
            var users = (await _usersRepository.FindAsync(nameSpecification)).ToList();

            if (!users.Any())
            {
                throw new EntityNotFoundException(nameof(User));
            }

            var user = users.First();

            var verified = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!verified)
            {
                throw new EntityNotFoundException(nameof(User));
            }

            return user;
        }

        public async Task<PagedList<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var paginationSpecification = new PagedUsersWithRolesSpecification(pageNumber, pageSize);
            var users = (await _usersRepository.FindAsync(paginationSpecification)).ToList();
            var totalCount = await _usersRepository.CountAsync();

            return new PagedList<User>(users, totalCount, pageNumber, pageSize);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var byIdWithRolesSpecification = new UsersByIdWithRolesSpecification(id);
            var user = (await _usersRepository.FindAsync(byIdWithRolesSpecification)).FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(User));
            }

            return user;
        }

        public async Task<User> AddUserAsync(User user, string password, IEnumerable<string> roleNames)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password can't be null or empty");
            }

            var specification = new UsersNameWithRolesSpecification(user.Username);
            var usersWithSameName = await _usersRepository.FindAsync(specification);

            if (usersWithSameName.Any())
            {
                _logger.LogError(
                    $"Can't create user. Users with same name already exist (Name - {user.Username})");
                throw new EntityAlreadyExistsException();
            }

            var roles = await GetRolesAsync(roleNames);
            user.Roles = roles;
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);

            await _usersRepository.AddAsync(user);
            _unitOfWork.Complete();

            return user;
        }

        public async Task<User> UpdateUser(User user, IEnumerable<string> roleNames)
        {
            var byIdWithRolesSpecification = new UsersByIdWithRolesSpecification(user.Id);
            var userWithSameId = (await _usersRepository.FindAsync(byIdWithRolesSpecification)).FirstOrDefault();
            
            if (userWithSameId == null)
            {
                throw new EntityNotFoundException(nameof(User));
            }

            userWithSameId.Name = user.Name;

            var roles = await GetRolesAsync(roleNames);
            userWithSameId.Roles = roles;

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

        private async Task<ICollection<Role>> GetRolesAsync(IEnumerable<string> roleNames)
        {
            var roleNamesList = roleNames.ToList();
            if (roleNamesList == null || !roleNamesList.Any())
            {
                throw new ArgumentException("Roles can't be null or empty");
            }

            var rolesSpecification = new RolesByNameSpecification(roleNamesList);
            var rolesEntities = (await _rolesRepository.FindAsync(rolesSpecification)).ToList();

            if (rolesEntities.Count() != roleNamesList.Count)
            {
                throw new ArgumentException("Roles invalid");
            }

            return rolesEntities;
        }
    }
}