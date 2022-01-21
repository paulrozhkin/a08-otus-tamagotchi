using System;
using System.Collections.Generic;

namespace Web.HttpAggregator.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set;}

        public IEnumerable<string> Roles { get; set; }
    }
}
