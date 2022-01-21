using AutoMapper;
using Domain.Core.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Users.Domain.Services;
using UsersApi;

namespace Users.API.Services
{
    public class GrpcUsersService : UsersApi.Users.UsersBase
    {
        private readonly ILogger<GrpcUsersService> _logger;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public GrpcUsersService(ILogger<GrpcUsersService> logger,
            IUsersService usersService,
            IMapper mapper)
        {
            _logger = logger;
            _usersService = usersService;
            _mapper = mapper;
        }

        public override async Task<CredentialsResponse> CheckUserCredentials(CredentialsRequest request,
            ServerCallContext context)
        {
            try
            {
                var user = await _usersService.CheckUserCredentials(request.UserName, request.Password);
                var userDto = _mapper.Map<User>(user);
                userDto.Roles.Add(user.Roles.Select(x => x.Name));

                return new CredentialsResponse()
                {
                    IsValid = true,
                    User = userDto
                };
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, User {request.UserName}");

                return new CredentialsResponse()
                {
                    IsValid = false
                };
            }
        }
    

        public override async Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var dishes = await _usersService.GetUsersAsync(request.PageNumber, request.PageSize);

            var dishesResponse = new GetUsersResponse()
            {
                CurrentPage = dishes.CurrentPage,
                PageSize = dishes.PageSize,
                TotalCount = dishes.TotalCount
            };

            var dishesDto = _mapper.Map<List<User>>(dishes);

            dishesResponse.Users.AddRange(dishesDto);

            return dishesResponse;
        }

        public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                var dish = await _usersService.GetUserByIdAsync(Guid.Parse(request.Id));
                var dishDto = _mapper.Map<User>(dish);

                var dishResponse = new GetUserResponse()
                {
                    User = dishDto
                };

                return dishResponse;
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, User {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var dishModel = _mapper.Map<Domain.Models.User>(request);

            try
            {
                var createdUser = await _usersService.AddUserAsync(dishModel);

                return new CreateUserResponse()
                {
                    User = _mapper.Map<User>(createdUser)
                };
            }
            catch (EntityAlreadyExistsException)
            {
                _logger.LogError(string.Format(Errors.Entities_Entity_already_exits));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
        }

        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var dishModel = _mapper.Map<Domain.Models.User>(request);

            try
            {
                var updateUser = await _usersService.UpdateUser(dishModel);

                return new UpdateUserResponse()
                {
                    User = _mapper.Map<User>(updateUser)
                };
            }
            catch (EntityAlreadyExistsException)
            {
                _logger.LogError(string.Format(Errors.Entities_Entity_already_exits));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, User {dishModel.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var idForDelete = request.Id;

            try
            {
                await _usersService.DeleteUserAsync(Guid.Parse(idForDelete));
                return new Empty();
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, User {idForDelete}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }
    }
}