using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ProniaWebSeyid.Abstraction;
using ProniaWebSeyid.ViewModels.CloudinaryViewModels;
using System.Net;

namespace ProniaWebSeyid.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly IConfiguration _configuration;
        private readonly CloudinaryOptionsVM _options;
        private readonly Cloudinary _cloudinary = null!;

        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _options = _configuration.GetSection("CloudinarySettings").Get<CloudinaryOptionsVM>() ?? new();

            var myAccount = new Account { ApiKey = _options.ApiKey, ApiSecret = _options.ApiSecret, Cloud = _options.CloudName };

            _cloudinary = new Cloudinary(myAccount);
            _cloudinary.Api.Secure = true;
        }


        public async Task<bool> FileDeleteAsync(string filePath)
        {
            try
            {
                string publicIdWithExtension = filePath.Substring(filePath.LastIndexOf("Pronia-Mpa101"));
                string publicId = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.'));

                var deleteParams = new DelResParams()
                {
                    PublicIds = new List<string> { publicId },
                    Type = "upload",
                    ResourceType = ResourceType.Image
                };
                var result = await _cloudinary.DeleteResourcesAsync(deleteParams);

                return result.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<string> FileUploadAsync(IFormFile file)
        {
            string fileName = string.Concat(Guid.NewGuid(), file.FileName.Substring(file.FileName.LastIndexOf('.')));

            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    Folder = "Pronia-Mpa101"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            string url = uploadResult.SecureUrl.ToString();

            return url;
        }
    }
}
