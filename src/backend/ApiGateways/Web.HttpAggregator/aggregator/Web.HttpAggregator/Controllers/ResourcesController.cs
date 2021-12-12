using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Infrastructure.Core.Localization;
using Infrastructure.Core.Minio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minio;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly MinioClient _minioClient;
        private readonly ILogger<ResourcesController> _logger;
        private readonly IResourcesMetadataService _resourcesMetadataService;

        public ResourcesController(MinioClient minioClient,
            ILogger<ResourcesController> logger,
            IResourcesMetadataService resourcesMetadataService)
        {
            _minioClient = minioClient;
            _logger = logger;
            _resourcesMetadataService = resourcesMetadataService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ResourceMetadataResponse>))]
        public async Task<ActionResult> GetResourcesMetadataAsync(
            [FromQuery] RestaurantParameters parameters)
        {
            var restaurants =
                await _resourcesMetadataService.GetResourcesMetadataAsync(parameters.PageNumber, parameters.PageSize);
            return Ok(restaurants);
        }

        [HttpGet("{resourceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IFormFile))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetResourceByIdAsync(Guid resourceId)
        {
            try
            {
                var resourcesMetadata = await _resourcesMetadataService.GetResourceMetadataAsync(resourceId);
                var objectName = resourcesMetadata.Id.ToString();
                try
                {
                    await _minioClient.StatObjectAsync(MinioBuckets.ResourcesBucketName, objectName);
                }
                catch
                {
                    _logger.LogError($"Resource {resourcesMetadata.Id} found metadata, but object not exist");
                    return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int) HttpStatusCode.NotFound,
                        detail: "Object not exist");
                }

                var steamFile = new MemoryStream();
                await _minioClient.GetObjectAsync(MinioBuckets.ResourcesBucketName, resourceId.ToString(),
                    async stream => { await stream.CopyToAsync(steamFile); });
                steamFile.Position = 0;

                var fileType = resourcesMetadata.ResourceType;
                var fileName = resourcesMetadata.ResourceName;
                return File(steamFile, fileType, fileName);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogError(e.ToString());
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int) HttpStatusCode.NotFound,
                    detail: e.ToString());
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResourceMetadataRequest))]
        public async Task<ActionResult> UploadResource(IFormFile file)
        {
            try
            {
                var fileName = file.FileName;
                var fileType = file.ContentType;
                var fileObjectId = Guid.NewGuid();
                var fileMetadata = new ResourceMetadataRequest()
                {
                    Id = fileObjectId,
                    ResourceName = fileName,
                    ResourceType = fileType
                };

                var inputStream = file.OpenReadStream();

                var found = await _minioClient.BucketExistsAsync(MinioBuckets.ResourcesBucketName);

                if (!found)
                {
                    await _minioClient.MakeBucketAsync(MinioBuckets.ResourcesBucketName);
                }

                // Upload a file to bucket.
                await _minioClient.PutObjectAsync(MinioBuckets.ResourcesBucketName, fileObjectId.ToString(), inputStream,
                    inputStream.Length, file.ContentType);

                await _resourcesMetadataService.CreateResourceMetadataAsync(fileMetadata);

                return CreatedAtAction("UploadResource", new {id = fileMetadata.Id}, fileMetadata);
            }
            catch (Exception ex)
            {
                throw new($"Error on file processing - {ex.Message}");
            }
        }
    }
}