using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class AmazonS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public AmazonS3Service(IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection("AWS");

            _s3Client = new AmazonS3Client(
                awsOptions["AccessKey"],
                awsOptions["SecretKey"],
                Amazon.RegionEndpoint.GetBySystemName(awsOptions["Region"])
            );

            _bucketName = awsOptions["BucketName"];
        }

        /// <summary>
        /// Uploads a file to S3 and returns the file URL.
        /// </summary>
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                if (fileStream == null || fileStream.Length == 0)
                {
                    throw new ArgumentException("Invalid file stream.");
                }

                // Tạo tên file duy nhất để tránh trùng lặp
                string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = uniqueFileName,
                    InputStream = fileStream,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead // Public Read nếu cần
                };

                await _s3Client.PutObjectAsync(request);

                return $"https://{_bucketName}.s3.amazonaws.com/{uniqueFileName}";
            }
            catch (AmazonS3Exception awsEx)
            {
                throw new Exception($"AWS S3 Error: {awsEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a file from S3.
        /// </summary>
        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                {
                    throw new ArgumentException("Invalid file URL.");
                }

                // Lấy file name từ URL
                var fileName = fileUrl.Split('/').Last();

                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                await _s3Client.DeleteObjectAsync(request);
                return true;
            }
            catch (AmazonS3Exception awsEx)
            {
                throw new Exception($"AWS S3 Error: {awsEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a file exists in S3.
        /// </summary>
        public async Task<bool> FileExistsAsync(string fileUrl)
        {
            try
            {
                var fileName = fileUrl.Split('/').Last();
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                await _s3Client.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }
    }
}
