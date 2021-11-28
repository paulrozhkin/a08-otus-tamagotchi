using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;

namespace Web.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly MinioClient _minioClient;

        public ResourcesController(MinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        [HttpPost]
        public async Task UploadFiles(List<IFormFile> files)
        {
            try
            {
                var file = files.First();
                var inputStream = file.OpenReadStream();

                var bucketName = "files";
                var found = await _minioClient.BucketExistsAsync(bucketName);

                if (!found)
                {
                    await _minioClient.MakeBucketAsync(bucketName);
                }

                // Upload a file to bucket.
                await _minioClient.PutObjectAsync(bucketName, Guid.NewGuid().ToString(), inputStream,
                    inputStream.Length, file.ContentType);
            }
            catch (Exception ex)
            {
                throw new($"Error on file processing - {ex.Message}");
            }
        }
    }
}