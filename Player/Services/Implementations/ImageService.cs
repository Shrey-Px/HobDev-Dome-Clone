using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace Dome.Player.Services.Implementations
{
    public class ImageService : IImageService
    {
        readonly string? bucketName;
        readonly string? accessKey;
        readonly string? secretAccessKey;
        readonly string region = "ca-central-1";

        readonly IAmazonS3 s3Client;
        private readonly IMediaPicker? mediaPicker;
        private readonly IFileSystem? fileSystem;

        public ImageService(
            IMediaPicker mediaPicker,
            ISecretsService secretsService,
            IFileSystem fileSystem
        )
        {
            this.mediaPicker = mediaPicker;
            this.fileSystem = fileSystem;
            bucketName = secretsService?.AWSS3BucketName;
            accessKey = secretsService?.AWSS3AccessKey;
            secretAccessKey = secretsService?.AWSS3SecretAccessKey;

            var credentials = new BasicAWSCredentials(accessKey, secretAccessKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.CACentral1,
                ServiceURL = "https://s3.ca-central-1.amazonaws.com",
            };

            this.s3Client = new AmazonS3Client(credentials, config);
        }

        public async Task UploadImageToAWSS3Async(byte[] imageBytes, string imageName)
        {
            // convert byte[] to Stream
            using MemoryStream photoStream = new MemoryStream(imageBytes);
            // get the file extension
            string extension = System.IO.Path.GetExtension(imageName);

            string type = string.Empty;
            if (extension == ".jpeg" || extension == ".jpg")
            {
                type = "image/jpeg";
            }
            else if (extension == ".png")
            {
                type = "image/png";
            }
            else if (extension == ".gif")
            {
                type = "image/gif";
            }
            else if (extension == ".bmp")
            {
                type = "image/bmp";
            }

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = imageName,
                InputStream = photoStream,
                ContentType = type,
            };

            await s3Client.PutObjectAsync(request);
        }

        public async Task DownloadImageFromAWSS3Async(string imageName)
        {
            var path = fileSystem?.AppDataDirectory;
            var fullPath = System.IO.Path.Combine(path, imageName);

            var request = new GetObjectRequest { BucketName = bucketName, Key = imageName };

            await Task.Run(async () =>
            {
                using GetObjectResponse? response = await s3Client.GetObjectAsync(request);
                using Stream? responseStream = response.ResponseStream;
                MemoryStream memoryStream = new MemoryStream();
                responseStream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                await File.WriteAllBytesAsync(fullPath, memoryStream.ToArray());
            });
        }

        public async Task<byte[]?> GetImageFromLocalStorageAsync(string imageName)
        {
            var path = fileSystem?.AppDataDirectory;
            var fullPath = System.IO.Path.Combine(path, imageName);
            bool exists = File.Exists(fullPath);
            if (!exists)
            {
                return null;
            }
            byte[] imageBytes = await File.ReadAllBytesAsync(fullPath);

            if (imageBytes != null)
            {
                return imageBytes;
            }

            return null;
        }

        public async Task DeleteImageFromAWSS3Async(string imageName)
        {
            var request = new DeleteObjectRequest { BucketName = bucketName, Key = imageName };

            await s3Client.DeleteObjectAsync(request);
        }

        public async Task<Dictionary<string, object>?> PickImageAsync()
        {
            byte[]? selectedImage = null;

            FileResult fileResult = await mediaPicker.PickPhotoAsync(
                new MediaPickerOptions { Title = "Please pick a photo" }
            );

            if (fileResult != null)
            {
                using Stream photoStream = await fileResult.OpenReadAsync();

                if (photoStream != null)
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    photoStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    photoStream.Position = 0;
                    selectedImage = memoryStream.ToArray();
                    string extension = System.IO.Path.GetExtension(fileResult.FileName);
                    return new Dictionary<string, object>
                    {
                        { "image", selectedImage },
                        { "extension", extension },
                    };
                }
            }

            return null;
        }
    }
}
