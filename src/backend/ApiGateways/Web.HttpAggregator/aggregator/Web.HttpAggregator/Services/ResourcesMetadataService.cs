using System;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Infrastructure.Core.Messages.ResourcesMessages;
using MassTransit;
using Microsoft.Extensions.Logging;
using ResourcesApi;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public class ResourcesMetadataService : IResourcesMetadataService
    {
        private readonly ILogger<ResourcesMetadataService> _logger;
        private readonly Resources.ResourcesClient _resourcesClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public ResourcesMetadataService(ILogger<ResourcesMetadataService> logger,
            Resources.ResourcesClient resourcesClient, 
            ISendEndpointProvider sendEndpointProvider,
            IMapper mapper)
        {
            _logger = logger;
            _resourcesClient = resourcesClient;
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<ResourceMetadataResponse>> GetResourcesMetadataAsync(int pageNumber, int pageSize)
        {
            var request = new GetResourcesMetadataRequest()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var resourcesMetadataResponse = await _resourcesClient.GetResourcesMetadataAsync(request);

            return _mapper.Map<PaginationResponse<ResourceMetadataResponse>>(resourcesMetadataResponse);
        }

        public async Task<ResourceMetadataResponse> GetResourceMetadataAsync(Guid id)
        {
            try
            {
                var resourceMetadataResponse =
                    await _resourcesClient.GetResourceMetadataAsync(new GetResourceMetadataRequest()
                        {Id = id.ToString()});
                return _mapper.Map<ResourceMetadataResponse>(resourceMetadataResponse.ResourceMetadata);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, id));
            }
        }

        public async Task CreateResourceMetadataAsync(ResourceMetadataRequest resourceMetadata)
        {
            var message = _mapper.Map<ResourceMetadataMessage>(resourceMetadata);
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:resources"));
            await endpoint.Send(message);
        }
    }
}