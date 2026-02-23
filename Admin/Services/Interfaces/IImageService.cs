namespace Dome.Admin.Services.Interfaces
{
    public interface IImageService
    {
        Task UploadImageToAWSS3Async(byte[] imageBytes, string imageName);

        Task DownloadImageFromAWSS3Async(string imageName);

        Task<byte[]?> GetImageFromLocalStorageAsync(string imageName);

        Task DeleteImageFromAWSS3Async(string imageName);

        Task<Dictionary<string, object>?> PickImageAsync();
    }
}
