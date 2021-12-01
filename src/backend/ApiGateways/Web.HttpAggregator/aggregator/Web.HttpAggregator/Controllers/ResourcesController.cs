using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Infrastructure.Core.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;

namespace Web.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly MinioClient _minioClient;
        private readonly ILogger<ResourcesController> _logger;
        private readonly string _bucketName = "files";

        public ResourcesController(MinioClient minioClient, ILogger<ResourcesController> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<DishResponse>))]
        public async Task<ActionResult> GetBucketsAsync()
        {
            var objectsObservable = _minioClient.ListObjectsAsync(_bucketName);
            var objects = new List<Item>();
            objectsObservable.Subscribe(x => objects.Add(x));
            await objectsObservable.ToTask();
            
            var bucketsDto = objects.Select(x => new
            {
                x.Key,
                x.LastModified,
                x.Size
            });

            return Ok(bucketsDto);
        }

        [HttpGet("{resourceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DishResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetDishByIdAsync(Guid resourceId)
        {
            try
            {
                Stream steamFile = new MemoryStream();
                await _minioClient.GetObjectAsync(_bucketName, resourceId.ToString(), async stream =>
                {
                    await stream.CopyToAsync(steamFile);
                });
                steamFile.Position = 0;

                // Объект Stream
                var fileInfo = new FileInfo(resourceId.ToString());

                var fileType = "image/jpeg";
                var fileName = fileInfo.Name;
                return File(steamFile, fileType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                    detail: e.ToString());
            }
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(List<IFormFile> files)
        {
            try
            {
                var file = files.First();
                var inputStream = file.OpenReadStream();
                
                var found = await _minioClient.BucketExistsAsync(_bucketName);

                if (!found)
                {
                    await _minioClient.MakeBucketAsync(_bucketName);
                }

                var fileObjectName = Guid.NewGuid();
                var name = file.Name;
                var contentType = file.ContentType;
                var objectName = "";

                // Upload a file to bucket.
                await _minioClient.PutObjectAsync(_bucketName, fileObjectName.ToString(), inputStream,
                    inputStream.Length, file.ContentType);

                return Ok(fileObjectName);
            }
            catch (Exception ex)
            {
                throw new($"Error on file processing - {ex.Message}");
            }
        }
    }
}