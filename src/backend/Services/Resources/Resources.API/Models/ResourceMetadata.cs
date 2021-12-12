using Domain.Core.Models;

namespace Resources.API.Models
{
    public class ResourceMetadata : BaseEntity
    {
        public string ResourceName { get; set; } = null!;

        public string ResourceType { get; set; } = null!;
    }
}