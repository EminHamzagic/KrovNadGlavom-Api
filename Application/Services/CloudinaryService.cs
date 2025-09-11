using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Application.Utils;

namespace krov_nad_glavom_api.Application.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var account = new Account("dp6gqdlbn", "961455162665623", "27eOLLBRIXRniKJPqXwAJ2-nFuA");
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder = "krovNadGlavom")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.AbsoluteUri;
            }

            throw new Exception($"Upload failed: {uploadResult.Error?.Message}");
        }

        public async Task<bool> DeleteImageAsync(string url)
        {
            var publicId = StringHelper.GetPublicIdFromUrl(url);
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ArgumentException("Invalid public ID");

            var deletionParams = new DeletionParams($"KrovNadGlavom/{publicId}")
            {
                ResourceType = ResourceType.Image
            };

            var result = await _cloudinary.DestroyAsync(deletionParams);

            return result.Result == "ok";
        }
    }
}