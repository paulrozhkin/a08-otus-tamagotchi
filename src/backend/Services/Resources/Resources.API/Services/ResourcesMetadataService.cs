using Domain.Core.Exceptions;
using Domain.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Resources.API.Config;
using Resources.API.Models;

namespace Resources.API.Services
{
    public class ResourcesMetadataMetadataService : IResourcesMetadataService
    {
        private readonly IMongoCollection<ResourceMetadata> _resourcesCollection;

        public ResourcesMetadataMetadataService(IOptions<ResourcesDatabaseOptions> resourcesDatabaseOptions)
        {
            var mongoClient = new MongoClient(
                resourcesDatabaseOptions.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                resourcesDatabaseOptions.Value.DatabaseName);

            _resourcesCollection = mongoDatabase.GetCollection<ResourceMetadata>(
                resourcesDatabaseOptions.Value.ResourcesCollectionName);
        }

        public async Task<PagedList<ResourceMetadata>> GetResourcesAsync(int pageNumber, int pageSize)
        {
            var total = await _resourcesCollection.EstimatedDocumentCountAsync();
            var results = await QueryByPage(pageNumber, pageSize, _resourcesCollection);
            return new PagedList<ResourceMetadata>(results.readOnlyList, (int) total, pageNumber, pageSize);
        }

        public async Task<ResourceMetadata> GetResourceByIdAsync(Guid id)
        {
            var resourceMetadata = await _resourcesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (resourceMetadata == null)
            {
                throw new EntityNotFoundException();
            }

            return resourceMetadata;
        }

        public async Task<ResourceMetadata> AddResourceAsync(ResourceMetadata resource)
        {
            resource.CreatedDate = DateTimeOffset.Now;
            resource.UpdatedDate = resource.CreatedDate;
            await _resourcesCollection.InsertOneAsync(resource);
            return resource;
        }

        private static async Task<(int totalPages, IReadOnlyList<ResourceMetadata> readOnlyList)> QueryByPage(int page,
            int pageSize, IMongoCollection<ResourceMetadata> collection)
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<ResourceMetadata, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<ResourceMetadata>()
                }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<ResourceMetadata, ResourceMetadata>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(Builders<ResourceMetadata>.Sort.Ascending(x => x.CreatedDate)),
                    PipelineStageDefinitionBuilder.Skip<ResourceMetadata>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<ResourceMetadata>(pageSize),
                }));

            var filter = Builders<ResourceMetadata>.Filter.Empty;
            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var totalPages = (int) count / pageSize;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<ResourceMetadata>();

            return (totalPages, data);
        }
    }
}