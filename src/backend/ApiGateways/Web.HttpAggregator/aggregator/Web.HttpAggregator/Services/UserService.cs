using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web.HttpAggregator.Config;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services;

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private readonly List<UserResponse> _users = new()
    {
        new UserResponse
        {
            Id = Guid.NewGuid(), UserName = "admin@gmail.com",
            Roles = new[] {Roles.Administrator, Roles.Client, Roles.Stuff}
        },
        new UserResponse {Id = Guid.NewGuid(), UserName = "client@gmail.com", Roles = new[] {Roles.Client}},
        new UserResponse {Id = Guid.NewGuid(), UserName = "stuff@gmail.com", Roles = new[] {Roles.Stuff}}
    };

    private readonly AuthenticateOptions _authenticateOptions;

    public UserService(IOptions<AuthenticateOptions> authenticateOptions)
    {
        _authenticateOptions = authenticateOptions.Value;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _users.SingleOrDefault(x => x.UserName == model.Username);

        // return null if user not found
        if (user == null) return null;

        // authentication successful so generate jwt token
        var token = GenerateJwtToken(user);

        return new AuthenticateResponse()
        {
            Token = token
        };
    }

    public Task<PaginationResponse<UserResponse>> GetUsersAsync(int pageNumber, int pageSize)
    {
        var result = new PaginationResponse<UserResponse>()
        {
            CurrentPage = 1,
            Items = _users,
            PageSize = pageSize,
            TotalCount = 1
        };

        return Task.FromResult(result);
    }

    public Task<UserResponse> GetUserByIdAsync(Guid id)
    {
        var user = _users.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(user);
    }

    public Task<UserResponse> CreateUserAsync(CreateUserRequest user)
    {
        var newUser = new UserResponse()
        {
            Id = Guid.NewGuid(),
            UserName = user.Username,
            Name = user.Name,
            Roles = user.Roles
        };

        _users.Add(newUser);

        return Task.FromResult(newUser);
    }

    public Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest user)
    {
        var userFromCollection = _users.Find(x => x.Id == id);
        userFromCollection.Roles = user.Roles;
        userFromCollection.Name = user.Name;
        return Task.FromResult(userFromCollection);
    }

    public Task DeleteUserAsync(Guid id)
    {
        var userFromCollection = _users.Find(x => x.Id == id);
        _users.Remove(userFromCollection);
        return Task.CompletedTask;
    }

    private string GenerateJwtToken(UserResponse user)
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