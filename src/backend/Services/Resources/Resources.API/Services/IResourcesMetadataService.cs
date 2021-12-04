using Domain.Core.Models;
using Resources.API.Models;

namespace Resources.API.Services;

public interface IResourcesMetadataService
{
    public Task<PagedList<ResourceMetadata>> GetResourcesAsync(int pageNumber, int pageSize);

    public Task<ResourceMetadata> GetResourceByIdAsync(Guid id);

    public Task<ResourceMetadata> AddResourceAsync(ResourceMetadata resource);
}