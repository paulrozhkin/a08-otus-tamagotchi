using System;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface IResourcesMetadataService
    {
        public Task<PaginationResponse<ResourceMetadataResponse>> GetResourcesMetadataAsync(int pageNumber, int pageSize);

        public Task<ResourceMetadataResponse> GetResourceMetadataAsync(Guid id);

        public Task CreateResourceMetadataAsync(ResourceMetadataRequest resourceMetadata);
    }
}
