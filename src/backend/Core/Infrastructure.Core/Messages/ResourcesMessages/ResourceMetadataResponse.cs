using System;

namespace Infrastructure.Core.Messages.ResourcesMessages
{
    public class ResourceMetadataMessage
    {
        public Guid Id { get; set; }

        public string ResourceName { get; set; }

        public string ResourceType { get; set; }
    }
}