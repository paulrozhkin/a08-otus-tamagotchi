using System;

namespace Web.HttpAggregator.Models;

public class ResourceMetadataRequest
{
    public Guid Id { get; set; }

    public string ResourceName { get; set; }

    public string ResourceType { get; set; }
}