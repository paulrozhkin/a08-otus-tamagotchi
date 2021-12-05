using System;

namespace Web.HttpAggregator.Models
{
    public class ResourceMetadataResponse
    {
        public Guid Id { get; set; }

        public string ResourceName { get; set; }

        public string ResourceType { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}