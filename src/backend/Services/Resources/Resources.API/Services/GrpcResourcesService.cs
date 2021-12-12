using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using ResourcesApi;

namespace Resources.API.Services
{
    public class GrpcResourcesService : ResourcesApi.Resources.ResourcesBase
    {
        private readonly ILogger<GrpcResourcesService> _logger;
        private readonly IResourcesMetadataService _resourcesMetadataService;
        private readonly IMapper _mapper;

        public GrpcResourcesService(ILogger<GrpcResourcesService> logger, IResourcesMetadataService resourcesMetadataService, IMapper mapper)
        {
            _logger = logger;
            _resourcesMetadataService = resourcesMetadataService;
            _mapper = mapper;
        }

        public override async Task<GetResourcesMetadataResponse> GetResourcesMetadata(GetResourcesMetadataRequest request,
            ServerCallContext context)
        {
            var resourcesMetadata =
                await _resourcesMetadataService.GetResourcesAsync(request.PageNumber, request.PageSize);

            var response = new GetResourcesMetadataResponse
            {
                CurrentPage = resourcesMetadata.CurrentPage,
                PageSize = resourcesMetadata.PageSize,
                TotalCount = resourcesMetadata.TotalCount
            };

            var resourcesDto = _mapper.Map<List<ResourceMetadata>>(resourcesMetadata);
            response.ResourcesMetadata.Add(resourcesDto);

            return response;
        }

        public override async Task<GetResourceMetadataResponse> GetResourceMetadata(GetResourceMetadataRequest request,
            ServerCallContext context)
        {
            try
            {
                var resourceMetadata = await _resourcesMetadataService.GetResourceByIdAsync(Guid.Parse(request.Id));
                var response = new GetResourceMetadataResponse
                {
                    ResourceMetadata = _mapper.Map<ResourceMetadata>(resourceMetadata)
                };

                return response;
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Resource {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }
    }
}