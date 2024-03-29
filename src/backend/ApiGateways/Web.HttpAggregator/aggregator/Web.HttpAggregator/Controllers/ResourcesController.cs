﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Domain.Core.Models;
using Infrastructure.Core.Localization;
using Infrastructure.Core.Minio;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly MinioClient _minioClient;
        private readonly ILogger<ResourcesController> _logger;
        private readonly IResourcesMetadataService _resourcesMetadataService;
        private const int CacheAgeSeconds = 60 * 60 * 24 * 30; // 30 days 

        public ResourcesController(MinioClient minioClient,
            ILogger<ResourcesController> logger,
            IResourcesMetadataService resourcesMetadataService)
        {
            _minioClient = minioClient;
            _logger = logger;
            _resourcesMetadataService = resourcesMetadataService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ResourceMetadataResponse>))]
        public async Task<ActionResult> GetResourcesMetadataAsync(
            [FromQuery] RestaurantParameters parameters)
        {
            var resourcesMetadata =
                await _resourcesMetadataService.GetResourcesMetadataAsync(parameters.PageNumber, parameters.PageSize);
            return Ok(resourcesMetadata);
        }

        [HttpGet("{resourceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IFormFile))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
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

                try
                {
                    await _minioClient.GetObjectAsync(MinioBuckets.ResourcesBucketName, resourceId.ToString(),
                        stream => { stream.CopyTo(steamFile); });
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Resource {resourcesMetadata.Id} can't extract cause: {exception}");
                }

                steamFile.Position = 0;

                Response.Headers["Cache-Control"] = $"public,max-age={CacheAgeSeconds}";

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
        [Authorize(Roles = Roles.Administrator)]
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
                await _minioClient.PutObjectAsync(MinioBuckets.ResourcesBucketName, fileObjectId.ToString(),
                    inputStream,
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