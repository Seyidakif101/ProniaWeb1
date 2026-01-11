namespace ProniaWebSeyid.Abstraction
{
    public interface ICloudinaryService
    {
        Task<string> FileUploadAsync(IFormFile file);
        Task<bool> FileDeleteAsync(string filePath);
    }
}
