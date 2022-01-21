using System.Collections.Generic;

namespace Web.HttpAggregator.Models;

public class UpdateUserRequest
{
    public string Name { get; set; }

    public IEnumerable<string> Roles { get; set; }
}