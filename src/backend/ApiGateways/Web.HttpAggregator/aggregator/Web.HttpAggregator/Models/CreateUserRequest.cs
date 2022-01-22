using System.Collections.Generic;

namespace Web.HttpAggregator.Models;

public class CreateUserRequest
{
    public string Username { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public IEnumerable<string> Roles { get; set; }
}