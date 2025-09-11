namespace krov_nad_glavom_api.Application.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder = "krovNadGlavom");
        Task<bool> DeleteImageAsync(string url);
    }
}