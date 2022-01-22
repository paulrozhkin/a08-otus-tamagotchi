using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UsersApi;
using Web.HttpAggregator.Config;
using Web.HttpAggregator.Models;
using CreateUserRequest = Web.HttpAggregator.Models.CreateUserRequest;
using UpdateUserRequest = Web.HttpAggregator.Models.UpdateUserRequest;

namespace Web.HttpAggregator.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly Users.UsersClient _usersClient;
    private readonly AuthenticateOptions _authenticateOptions;

    public UserService(ILogger<UserService> logger, 
        IOptions<AuthenticateOptions> authenticateOptions,
        IMapper mapper,
        Users.UsersClient usersClient)
    {
        _logger = logger;
        _mapper = mapper;
        _usersClient = usersClient;
        _authenticateOptions = authenticateOptions.Value;
    }

    public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
    {
        var checkResponse = await _usersClient.CheckUserCredentialsAsync(new CredentialsRequest()
        {
            UserName = model.Username,
            Password = model.Password
        });

        if (!checkResponse.IsValid)
        {
            return null;
        }

        // authentication successful so generate jwt token
        var token = GenerateJwtToken(checkResponse.User);

        return new AuthenticateResponse()
        {
            Token = token
        };
    }

    public async Task<PaginationResponse<UserResponse>> GetUsersAsync(int pageNumber, int pageSize)
    {
        var request = new GetUsersRequest()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var usersResponse = await _usersClient.GetUsersAsync(request);

        return _mapper.Map<PaginationResponse<UserResponse>>(usersResponse);
    }

    public async Task<UserResponse> GetUserByIdAsync(Guid id)
    {
        try
        {
            var userResponse =
                await _usersClient.GetUserAsync(new GetUserRequest() { Id = id.ToString() });
            return _mapper.Map<UserResponse>(userResponse.User);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
        }
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest user)
    {
        try
        {
            var userCreateResponse = await _usersClient.CreateUserAsync(new UsersApi.CreateUserRequest()
            {
                User = _mapper.Map<User>(user)
            });

            return _mapper.Map<UserResponse>(userCreateResponse.User);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
        {
            throw new EntityAlreadyExistsException(Errors.Entities_Entity_already_exits);
        }
    }

    public async Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest user)
    {
        try
        {
            var userForRequest = _mapper.Map<User>(user);
            userForRequest.Id = id.ToString();
            var userCreateResponse = await _usersClient.UpdateUserAsync(
                new UsersApi.UpdateUserRequest()
                {
                    User = userForRequest
                });

            return _mapper.Map<UserResponse>(userCreateResponse.User);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
        }
    }

    public async Task DeleteUserAsync(Guid id)
    {
        try
        {
            await _usersClient.DeleteUserAsync(new DeleteUserRequest()
            {
                Id = id.ToString()
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
        }
    }

    private string GenerateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authenticateOptions.Secret);

        var userClaims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };

        foreach (var item in user.Roles)
        {
            userClaims.Add(new Claim(ClaimTypes.Role, item));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(userClaims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}