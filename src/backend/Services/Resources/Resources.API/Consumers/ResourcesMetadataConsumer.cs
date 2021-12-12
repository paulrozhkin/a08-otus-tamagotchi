using System.ComponentModel.Design;
using AutoMapper;
using Infrastructure.Core.Messages.ResourcesMessages;
using MassTransit;
using Resources.API.Models;
using Resources.API.Services;

namespace Resources.API.Consumers
{
    public class ResourcesMetadataConsumer : IConsumer<ResourceMetadataMessage>
    {
        private readonly IResourcesMetadataService _resourceService;
        private readonly IMapper _mapper;

        public ResourcesMetadataConsumer(
            IResourcesMetadataService resourceService,
            IMapper mapper)
        {
            _resourceService = resourceService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ResourceMetadataMessage> context)
        {
            var newResourceMetadata = _mapper.Map<ResourceMetadata>(context.Message);
            await _resourceService.AddResourceAsync(newResourceMetadata);
        }
    }
}